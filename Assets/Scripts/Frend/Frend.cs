using UnityEngine;
using Fusion;
using UnityEngine.AI;

public class Frend : NetworkBehaviour
{
    public Transform Target {get; set;}
    NavMeshAgent agent;

    float targetInterval = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null){
            throw new System.Exception("Nav Mesh Agentをアタッチしてください");
        }
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        Fllow();
    }

    void Fllow()
    {
        if (Target == null) return;
        agent.SetDestination(Target.position);
        if (Vector3.Distance(transform.position, Target.position) > targetInterval){
            agent.isStopped = false;
        }else {
            agent.isStopped = true;
        }
    }
}
