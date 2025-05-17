using UnityEngine;
using Fusion;

public class DespawnEnemy : NetworkBehaviour
{
    NetworkObject netObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        netObj = GetComponentInParent<NetworkObject>();
        if (netObj == null) throw new System.Exception("NetworkObjectがアタッチされていません");

    }

    void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return; // ホストだけが実行
        // プレイヤーの仲間であるにエネミーが弱点に触れた
        if (other.gameObject.tag == "Frend" && other.GetComponent<Frend>().IsFellow)
        {
            Runner.Despawn(netObj);
            Debug.Log("エネミーは削除されました。");
        }
    }
}
