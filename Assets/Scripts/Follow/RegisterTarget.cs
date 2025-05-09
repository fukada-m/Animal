using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RegisterTarget : MonoBehaviour
{
    readonly float targetInterval = 3;
    NavMeshAgent agent;

    Transform target;

    GameObject currentLeader;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        if (agent == null){
            throw new System.Exception("Nav Mesh Agentをアタッチしてください");
        }
    }
    // TODOUniRXを使ってイベント化する
    // リーダーを登録してリーダーの最後尾にいるオブジェクトについていく
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger Enter] {other.gameObject.name} が入りました");
        if (other.gameObject.tag == "Player"){
            // リーダーが変わっていればをリーダーを交代してその最後尾を自分のターゲットにセットする。そして、自分が最後尾になる。
            if (IsChange(other.gameObject, currentLeader)){
                currentLeader = other.gameObject;
                var player = currentLeader.GetComponent<Player>();
                if(player == null){throw new System.Exception("PlayerにPlayerクラスをアタッチしよう");}
                target = player.LastFrend.transform;
                player.LastFrend = this.gameObject;
            }
        }
    }

    bool IsChange(GameObject leader, GameObject currentleader){
        if(currentLeader == null)return true;
        if (currentleader == leader){
            return false;
        } else{
            return true;
        }
    }

    void Update()
    {
        Fllow();
    }

    // targetとの距離がtargetInterval以上離れたら追跡する。
    void Fllow(){
        if (target == null) {return;}
        agent.SetDestination(target.position);
        if (Vector3.Distance(transform.position, target.position) > targetInterval){
            agent.isStopped = false;
        }else {
            agent.isStopped = true;
        }
    }
}
