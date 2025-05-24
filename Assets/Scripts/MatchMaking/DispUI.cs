using UnityEngine;

// ロビーとルームでのUIの表示、非表示を管理する
public class DispUI : MonoBehaviour
{
    [SerializeField]
    GameObject lobbyUI;
    [SerializeField]
    GameObject sessionUI;

    void Start()
    {
        DispLobbyUI();
    }

    public void DispLobbyUI()
    {
        sessionUI.SetActive(false);
        lobbyUI.SetActive(true);
    }

    public void DispSessionUI()
    {
        lobbyUI.SetActive(false);
        sessionUI.SetActive(true);
    }
}
