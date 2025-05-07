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
        target = null;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[Trigger Enter] {collision.gameObject.name} が入りました");
        target = null;
    }

    // トリガーから出た瞬間に呼ばれる
    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[Trigger Exit] {other.gameObject.name} が出ました");
        if (other.tag == "Player"){
            target = other.transform;
        }
    }

    void OnCollisionExit(Collision collision)
    {
         Debug.Log($"[Trigger Exit] {collision.gameObject.name} が出ました");
        if (collision.gameObject.tag == "Player"){
            target = collision.transform;
        }
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
