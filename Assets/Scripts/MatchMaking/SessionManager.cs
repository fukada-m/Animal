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
    [SerializeField]
    Text returnButton;
    [SerializeField]
    Button startButton;
    int gameSceneBuildIndex = 1;

    [System.Obsolete]
    void Start()
    {
        updateSessionEvent.UpdateSession.Subscribe(_ => SetSessionInfoUI()).AddTo(this);
        updateSessionEvent.UpdateSession.Subscribe(_ => SetActiveStartButton()).AddTo(this);

    }

    [System.Obsolete]
    void SetSessionInfoUI()
    {
        var runner = GetNetworkRunner();
        var sessionInfo = runner.SessionInfo;
        if (!sessionInfo.IsValid)
        {
            Debug.LogWarning("SessionInfo がまだ有効ではありません");
            return;
        }
        sessionNameText.text = $"{sessionInfo.Name}";
        playerCountText.text = $"{sessionInfo.PlayerCount}";
        SetReturnButtonMessage(runner);
    }

    void SetActiveStartButton()
    {
        var runner = GetNetworkRunner();
        if (runner.IsServer)
            startButton.interactable = true;
    }

    public void OnClickStartButton()
    {
        var runner = GetNetworkRunner();
        runner.SetActiveScene(gameSceneBuildIndex);
        
    }

    public async void OnclickReturnButoon()
    {
        var runner = GetNetworkRunner();
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

    void SetReturnButtonMessage(NetworkRunner runner)
    {
        if (runner.IsServer)
            returnButton.text = "ルームを解散する";
        else
            returnButton.text = "ルームを抜ける";
    }

    NetworkRunner GetNetworkRunner()
    {
        var runner = FindObjectOfType<NetworkRunner>().GetComponent<NetworkRunner>();
        if (runner == null) Debug.LogError("NetworkRunnerが見つかりません。");
        return runner;
    }
}
