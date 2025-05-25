using Fusion;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{
    [SerializeField]
    UpdateSessionEvent updateSessionEvent;
    [SerializeField]
    Text sessionNameText;
    [SerializeField]
    Text playerCountText;

    void Start()
    {
        updateSessionEvent.UpdateSession.Subscribe(_ => SetSessionInfoUI()).AddTo(this);
    }

    [System.Obsolete]
    void SetSessionInfoUI()
    {
        var runner = FindObjectOfType<NetworkRunner>().GetComponent<NetworkRunner>();
        if (runner == null) Debug.LogError("NetworkRunnerが見つかりません。");
        var sessionInfo = runner.SessionInfo;
        if (!sessionInfo.IsValid)
        {
            Debug.LogWarning("SessionInfo がまだ有効ではありません");
            return;
        }
        sessionNameText.text = $"{sessionInfo.Name}";
        playerCountText.text = $"{sessionInfo.PlayerCount}";
    }

    public async void OnclickReturnButoon()
    {
         var runner = FindObjectOfType<NetworkRunner>().GetComponent<NetworkRunner>();
        if (runner == null) Debug.LogError("NetworkRunnerが見つかりません。");
        // 自分がホストの場合は自分以外のプレイヤーをキックする
        if (runner.IsServer && runner.IsRunning)
        {
            foreach (var player in runner.ActivePlayers)
            {
                if (player != runner.LocalPlayer)
                    runner.Disconnect(player);
            }
        }
        await runner.Shutdown();
    }
}
