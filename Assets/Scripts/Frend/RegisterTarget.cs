using Fusion;
using UnityEngine;

public class RegisterTarget : NetworkBehaviour
{
    Frend frend;

    NetworkObject netObj;
    void Start(){
        frend = GetComponent<Frend>();
        if (frend == null) { throw new System.Exception("Frendがアタッチされていません。"); }
        netObj = GetComponent<NetworkObject>();
        if (netObj == null) { throw new System.Exception("NetworkObjectがアタッチされていません。"); }
    }
    // TODOUniRXを使ってイベント化する
    // プレイヤーをターゲットとして登録する
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger Enter] {other.gameObject.name} が入りました");
        if (other.gameObject.tag == "Player"){
            frend.Target = other.transform;
        }
        if (other.gameObject.tag == "Enemy"){
            if(!Object.HasStateAuthority) return; // ホストだけが実行
            Runner.Despawn(netObj);
            Debug.Log("フレンドは削除されました。");
        }
    }

}