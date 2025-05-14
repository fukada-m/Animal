using UnityEngine;
using Fusion;
using UnityEngine.AI;

public class Enemy : NetworkBehaviour
{
    Vector3 destination;
    NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null){
            throw new System.Exception("Nav Mesh Agentをアタッチしてください");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if(transform.position.x == destination.x && transform.position.z == destination.z) { destination = CreateMovePos(); }
        agent.SetDestination(destination);
    }

    Vector3 CreateMovePos(){
        var random = Random.insideUnitCircle * 10f;
        return new Vector3(random.x, 1, random.y);
    }
}
