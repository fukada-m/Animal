using Fusion;
using UnityEngine;

public class RegisterTarget : MonoBehaviour
{
    Frend frend;

    
    void Start(){
        frend = GetComponent<Frend>();
        if (frend == null) { throw new System.Exception("Frendがアタッチされていません。"); }
        
    }

    // プレイヤーをターゲットとして登録する
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger Enter] {other.gameObject.name} が入りました");
        if (other.gameObject.tag == "Player"){
            frend.Target = other.transform;
        }

    }

}