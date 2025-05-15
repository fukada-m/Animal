using UnityEngine;
using UnityEngine.AI;
using Fusion;

public class Enemy : NetworkBehaviour
{
    NavMeshAgent agent;
    public Vector3 Destination { get; set; }

    private const float radius = 11f;

    public override void Spawned()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) throw new System.Exception("Nav MeshAgentがアタッチされていません。");

        // NavMesh 上に Warp
        if (NavMesh.SamplePosition(transform.position, out var hit, 2f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        else
        {
            Debug.LogError("[Enemy] NavMesh に配置できませんでした");
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Destination == null) return;
        if (!Object.HasStateAuthority) return;

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("[Enemy] Agent is NOT on NavMesh");
            return;
        }

        // 到着したらランダムなポジションを新たな行先にする
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 1f)
        {
            Debug.Log("到着");
            var rnd = Random.insideUnitCircle * radius;
            var raw = new Vector3(rnd.x, transform.position.y, rnd.y);

            if (NavMesh.SamplePosition(raw, out var hit, 2f, NavMesh.AllAreas))
            {
                Destination = hit.position;
            }
        }
        agent.SetDestination(Destination);
    }
}
