using UnityEngine;
using Fusion;

public class DespawnFrend : NetworkBehaviour
{
    Frend frend;
    NetworkObject netObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        netObj = GetComponent<NetworkObject>();
        if (netObj == null) throw new System.Exception("NetworkObjectがアタッチされていません。");
        frend = GetComponent<Frend>();
        if (frend == null) throw new System.Exception("Frendがアタッチされていません。");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return; // ホストだけが実行
        // プレイヤーの仲間ではないときにエネミーに触れられた
        if (other.gameObject.tag == "Enemy" && !frend.IsFellow)
        {
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
