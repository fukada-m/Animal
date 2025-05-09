using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RegisterTarget : MonoBehaviour
{
    readonly float targetInterval = 3;
    NavMeshAgent agent;

    Transform target;

    GameObject leader;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        if (agent == null){
            throw new System.Exception("Nav Mesh Agentをアタッチしてください");
        }
    }
    // リーダーを登録してリーダーの最後尾にいるオブジェクトについていく
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger Enter] {other.gameObject.name} が入りました");
        if (gameObject.tag == "Player"){
            // リーダーが後退したときにtargetを登録する
            if (leader == null && leader != gameObject){
                leader = gameObject;
                var player = other.GetComponent<Player>();
                if(player == null){throw new System.Exception("PlayerにPlayerクラスをアタッチしよう");}
                target = player.LastFrend.transform;
                player.LastFrend = this.gameObject;
            }
        }
    }

    // トリガーから出た瞬間に呼ばれる
    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[Trigger Exit] {other.gameObject.name} が出ました");
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
