using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class FusionLauncher : MonoBehaviour
{
    [SerializeField]
    NetworkRunner networkRunnerPrefab;

    [SerializeField]
    NetworkPrefabRef playerPrefab;
    NetworkRunner networkRunner;

    async void Start()
    {
        // NetworkRunner をシーン上に生成
        networkRunner = Instantiate(networkRunnerPrefab);
        
        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode       = GameMode.Shared,   // シェアモードを使用
            SceneManager   = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok){
            Debug.Log("接続成功");
            networkRunner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity, networkRunner.LocalPlayer);
        }else {
            Debug.Log("接続失敗");
        }
    }
}
