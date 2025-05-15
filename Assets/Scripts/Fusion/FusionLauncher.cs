using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class FusionLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    NetworkRunner networkRunnerPrefab;

    [SerializeField]
    NetworkPrefabRef playerPrefab;

    [SerializeField]
    NetworkPrefabRef frendPrefab;

    [SerializeField]
    NetworkPrefabRef enemyPrefab;

    [SerializeField]
    Destination destination;
    NetworkRunner networkRunner;
    
    NetworkInputData data;

    SpawnFrend spawnFrend;

    async void Start()
    {
        // NetworkRunner をシーン上に生成
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.AddCallbacks(this);
        data = new NetworkInputData();
        spawnFrend = GetComponent<SpawnFrend>();
        var result = await networkRunner.StartGame(new StartGameArgs
        {
            GameMode       = GameMode.AutoHostOrClient,   // ホストモードを使用
            SceneManager   = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok){
            Debug.Log("接続成功");
            spawnFrend.Spawn(networkRunner);
            networkRunner.Spawn(enemyPrefab, new Vector3(3f,1f,3f), Quaternion.identity, PlayerRef.None);

        }else {
            Debug.Log("接続失敗");
        }
    }
    // セッションに参加したらアバターを作成する
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
       if (!runner.IsServer) { return; }
       // ランダムな生成位置（半径5の円の内部）を取得する
       var randomValue = UnityEngine.Random.insideUnitCircle * 5f;
       var spawnPosition = new Vector3(randomValue.x, 1f, randomValue.y);
       // スポーン地点から動かないように行先にspawnPositionを設定
       data.Direction = spawnPosition;
       // 参加したプレイヤーのアバターを生成する
       var avatar = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
       // プレイヤー（PlayerRef）とアバター（NetworkObject）を関連付ける
       runner.SetPlayerObject(player, avatar);
       Debug.Log("プレイヤーを作成しました。");
    }

    // セッションから退出したらアバターを削除する
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        if(!runner.IsServer) { return; }
        if (runner.TryGetPlayerObject(player, out var avater)) {
            runner.Despawn(avater);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) {
        // マウスポジションの値をセットしている
        data.Direction = destination.Get;
        input.Set(data);
    }

    // INetworkRunnerCallbacksインターフェースの空実装
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {}
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {}
    public void OnConnectedToServer(NetworkRunner runner) {}
    public void OnDisconnectedFromServer(NetworkRunner runner) {}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.   ConnectRequest request, byte[] token) {}
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress,    NetConnectFailedReason reason) {}
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {}
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string,    object> data) {}
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)    {}
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player,    ArraySegment<byte> data) {}
    public void OnSceneLoadDone(NetworkRunner runner) {}
    public void OnSceneLoadStart(NetworkRunner runner) {}
}
