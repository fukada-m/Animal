using UnityEngine;

public class RegisterTarget : MonoBehaviour
{
    Frend frend;

    void Start(){
        frend = GetComponent<Frend>();
    }
    // TODOUniRXを使ってイベント化する
    // プレイヤーをターゲットとして登録する
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger Enter] {other.gameObject.name} が入りました");
        if (other.gameObject.tag == "Player"){
            frend.Target = other.transform;
        }
    }

}
