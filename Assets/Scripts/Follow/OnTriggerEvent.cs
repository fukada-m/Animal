using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class OnTriggerEvent : MonoBehaviour
{
    NavMeshAgent agent;

    Transform target;

    GameObject leader;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        if (agent == null){
            throw new System.Exception("Nav Mesh Agentをアタッチしてください");
        }
    }
    // トリガーに入った瞬間に呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger Enter] {other.gameObject.name} が入りました");
        if (other.tag == "Player"){
            if (leader == null && leader != other.gameObject){
                leader = other.gameObject;
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
        if (target != null){
            agent.SetDestination(target.position);
        }
        if (target != null && Vector3.Distance(transform.position, target.position) > 3){
            agent.isStopped = false;
        }else if(target != null && Vector3.Distance(transform.position, target.position) <= 3) {
            agent.isStopped = true;
        }
    }
}
