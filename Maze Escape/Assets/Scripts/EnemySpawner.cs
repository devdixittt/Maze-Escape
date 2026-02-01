using UnityEngine;
using UnityEngine.AI;

public class EnemyRespawner : MonoBehaviour
{
    public Transform player;
    public float despawnDistance = 40f;
    public float respawnRadius = 15f;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > despawnDistance)
        {
            RespawnNearPlayer();
        }
    }

    void RespawnNearPlayer()
    {
        Vector3 randomDir = Random.insideUnitSphere * respawnRadius;
        randomDir.y = 0;
        Vector3 targetPos = player.position + randomDir;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, respawnRadius, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);   // IMPORTANT
            agent.ResetPath();
        }
    }
}
