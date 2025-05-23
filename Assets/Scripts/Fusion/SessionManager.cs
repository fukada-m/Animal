using Fusion;
using TMPro;
using UnityEngine;
using UniRx;

public class SessionManager : MonoBehaviour
{
    [SerializeField]
    UpdateSessionEvent updateSessionEvent;
    [SerializeField]
    NetworkRunner runner;
    [SerializeField]
    TextMeshProUGUI sessionNameText;
    [SerializeField]
    TextMeshProUGUI playerCountText;

    void Start()
    {
        updateSessionEvent.UpdateSession.Subscribe(_ => SetSessionInfoUI()).AddTo(this);   
    }
    
    void SetSessionInfoUI()
    {
        var sessionInfo = runner.SessionInfo;
        if (!sessionInfo.IsValid)
        {
            Debug.LogWarning("SessionInfo がまだ有効ではありません");
            return;
        }
        sessionNameText.text = $"ルーム名：{sessionInfo.Name}";
        playerCountText.text = $"参加者数：{sessionInfo.PlayerCount}";

    }
}
