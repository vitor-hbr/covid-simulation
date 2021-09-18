using UnityEngine.AI;
using UnityEngine;

public class AgentNavigation : MonoBehaviour
{
    public GameObject chair;
    public GameObject exit;
    public GameObject start;
    private NavMeshAgent navMeshAgent;
    public float[] periodBoundry;
    public DayNightCycle dayNight;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(chair && transform.position != chair.transform.position && dayNight.time > periodBoundry[0] && dayNight.time < periodBoundry[1])
            navMeshAgent.SetDestination(chair.transform.position);
        else navMeshAgent.SetDestination(exit.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        transform.position = start.transform.position;
    }
}
