using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using TMPro;
using System.Linq;
// TODO ボタンクリックしたことへのメッセージデフォルトのルーム名を用意する
public class LobbyManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    TMP_InputField roomNameInput;
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
    NetworkRunner runner;
    NetworkSceneManagerDefault sceneManager;
    List<string> currentSessionNames = new List<string>();
    Dictionary<string, GameObject> joinButtons = new Dictionary<string, GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            Debug.Log("ロビーに参加成功");
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
            dispUI.DispSessionUI();
            updateSessionEvent.UpdateSeesion();
        }
        else
        {
            Debug.LogError($"ルーム作成失敗: {startResult.ShutdownReason}");
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
        var text = joinButton.GetComponentInChildren<TextMeshProUGUI>();
        if (text == null)
            throw new SystemException("TMProがアタッチされていません");
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
            }
            else
            {
                Debug.Log($"セッション「{sessionName}」への参加成功");
                //UIの切替
                dispUI.DispSessionUI();
                updateSessionEvent.UpdateSeesion();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"JoinRoom例外: {ex.Message}");
            // クラッシュしそうならロビーに戻す
            await runner.JoinSessionLobby(SessionLobby.ClientServer);
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
