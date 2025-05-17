using UnityEngine;
using Fusion;

public class DespawnThis : NetworkBehaviour
{
    NetworkObject netObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        netObj = GetComponent<NetworkObject>();
        if (netObj == null) { throw new System.Exception("NetworkObjectがアタッチされていません。"); }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!Object.HasStateAuthority) return; // ホストだけが実行
            Runner.Despawn(netObj);
            Debug.Log("フレンドは削除されました。");
        }
    }

    public void Despawn()
    {
        if (!Object.HasStateAuthority) return; // ホストだけが実行
            Runner.Despawn(netObj);
            Debug.Log("フレンドは削除されました。");
    }
    
}
