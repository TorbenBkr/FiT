using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints;  // Array mit Wegpunkten
    private NavMeshAgent agent;
    private int currentWaypointIndex;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        currentWaypointIndex = player.GetComponent<MoodSystem>().currentPhase;
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        currentWaypointIndex = player.GetComponent<MoodSystem>().currentPhase;

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Prüfen, ob NPC das Ziel erreicht hat
        if (agent.remainingDistance < 3f)
        {
            agent.speed = 0f;
        } else if (agent.remainingDistance > 3f) {
            agent.speed = 3f;
        }
    }
}