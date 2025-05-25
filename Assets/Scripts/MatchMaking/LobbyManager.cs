using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using System.Linq;

// TODO ボタンクリックしたことへのメッセージ
public class LobbyManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    InputField roomNameInput;
    [SerializeField]
    Button createRoomButton;
    [SerializeField]
    Transform roomListContainer;
    [SerializeField]
    GameObject roomButtonPrefab;
    [SerializeField]
    UpdateSessionEvent updateSessionEvent;
    [SerializeField]
    NetworkRunner runnerPref;
    [SerializeField]
    DispUI dispUI;
    [SerializeField]
    Text playerMessage;
    NetworkRunner runner;
    NetworkSceneManagerDefault sceneManager;
    List<string> currentSessionNames = new List<string>();
    Dictionary<string, GameObject> joinButtons = new Dictionary<string, GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMessage.text = "ロビー情報を取得中です...";
        CreateRunner();
        JoinLobby();
    }

    /// <summary>
    /// ロビーに参加
    /// </summary>
    async void JoinLobby()
    {
        var result = await runner.JoinSessionLobby(SessionLobby.ClientServer);
        if (!result.Ok)
            Debug.LogError($"ロビーに参加失敗: {result.ShutdownReason}");
        else
            playerMessage.text = "ロビーに参加しました。";
    }

    /// <summary>
    /// ロビー上のセッション一覧が更新されるたびに呼ばれるコールバック
    /// </summary>
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        List<string> newNames = sessionList.Select(s => s.Name).ToList();
        // ルームが増えていたらプレハブをUIに追加
        foreach (var name in newNames.Except(currentSessionNames))
        {
            CreateJoinButton(name);
        }

        // ルームが消えていたらUIからプレハブを削除
        foreach (var name in currentSessionNames.Except(newNames))
        {
            DestroyJoinButton(name);
        }
        // キャッシュを更新
        currentSessionNames = newNames;
    }

    public void OnClickCreateRoom()
    {
        CreateRoom(roomNameInput.text);
        createRoomButton.interactable = false;
    }

    /// <summary>
    /// ホストモードでセッションルームを作成する
    /// </summary>
    async void CreateRoom(string roomName)
    {
        playerMessage.text = "ルームを作成中です...";
        if (roomName == "") roomName = $"未設定{UnityEngine.Random.Range(0, 99)}";
        var startResult = await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Host,
            SessionName = roomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = sceneManager
        });

        if (startResult.Ok)
        {
            Debug.Log("ルーム作成 & ホスト開始");
            playerMessage.text = "ルームを作成しました。";
            dispUI.DispSessionUI();
            updateSessionEvent.UpdateSeesion();
        }
        else
        {
            Debug.LogError($"ルーム作成失敗: {startResult.ShutdownReason}");
            playerMessage.text = "ルームの作成に失敗しました。";
            createRoomButton.interactable = true;
        }
    }

    /// <summary>
    /// ルーム名からボタンを生成しテキストはsessionNameをセットし、Click 時に JoinRoom を呼ぶ
    /// JoinButtonはDictionaryで管理されセッションがなくなると削除される
    /// </summary>
    void CreateJoinButton(string sessionName)
    {
        var joinButton = Instantiate(roomButtonPrefab, roomListContainer);
        var text = joinButton.GetComponentInChildren<Text>();
        if (text == null)
            throw new SystemException("Textがアタッチされていません");
        else
            text.text = sessionName;
        var button = joinButton.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            JoinRoom(sessionName);
        });
        joinButtons.Add(sessionName, joinButton);
    }

    /// <summary>
    /// 指定ルーム名のボタンを削除する
    /// </summary>
    void DestroyJoinButton(string sessionName)
    {
        if (joinButtons.TryGetValue(sessionName, out var joinButoon))
        {
            Destroy(joinButoon);
            joinButtons.Remove(sessionName);
        }
    }
    
    /// <summary>
    /// ルーム（セッション）にクライアントとして参加するメソッド
    /// </summary>
    public async void JoinRoom(string sessionName)
    {
        if (!currentSessionNames.Contains(sessionName))
        {
            Debug.LogWarning($"[{sessionName}] はもう存在しません。ロビーを再取得します。");
            playerMessage.text = $"[{sessionName}]は存在しませんでした。";
            await runner.JoinSessionLobby(SessionLobby.ClientServer);
            return;
        }
        try
        {
            var result = await runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Client,
                SessionName = sessionName,
                DisableClientSessionCreation = true,  // 存在しない名前ならエラーにする
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = sceneManager
            });

            if (!result.Ok)
            {
                Debug.LogError($"ルーム参加失敗: {result.ShutdownReason}");
                playerMessage.text = "ルームの参加に失敗しました。";
            }
            else
            {
                Debug.Log($"セッション「{sessionName}」への参加成功");
                //UIの切替
                dispUI.DispSessionUI();
                updateSessionEvent.UpdateSeesion();
                playerMessage.text = $"[{sessionName}]への参加が成功しました。";
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"JoinRoom例外: {ex.Message}");
            // クラッシュしそうならロビーに戻す
            await runner.JoinSessionLobby(SessionLobby.ClientServer);
            playerMessage.text = "通信エラー：ロビーに再接続します。";
        }
    }

    void CreateRunner()
    {
        runner = Instantiate(runnerPref);
        if (runner == null) Debug.LogError("NetworkRunnerが見つかりません。");
        runner.ProvideInput = true;
        runner.AddCallbacks(this);
        sceneManager = runner.GetComponent<NetworkSceneManagerDefault>();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        updateSessionEvent.UpdateSeesion();
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason reason)
    {
        playerMessage.text = "ロビーに再接続します。";
        dispUI.DispLobbyUI();
        createRoomButton.interactable = true;
        CreateRunner();
        JoinLobby();
    }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}
