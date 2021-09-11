using UnityEngine.AI;
using UnityEngine;

public class AgentNavigation : MonoBehaviour
{
    public GameObject chair;
    private NavMeshAgent navMeshAgent;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(chair && transform.position != chair.transform.position)
            navMeshAgent.SetDestination(chair.transform.position);
    }
}
