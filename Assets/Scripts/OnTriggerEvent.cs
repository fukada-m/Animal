using UnityEngine;
using UnityEngine.AI;

public class OnTriggerEvent : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;

    Transform target;

    void Start(){
        if (agent == null){
            throw new System.Exception("Nav Mesh Agentをアタッチしてください");
        }
    }
    // トリガーに入った瞬間に呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger Enter] {other.gameObject.name} が入りました");
        
        if (other.tag == "Player"){
            target = other.transform;
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
    }
}
