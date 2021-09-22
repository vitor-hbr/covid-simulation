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
        Debug.Log(Vector3.Distance(chair.transform.position, transform.position));
        if (chair && exit)
        {
            if (dayNight.time > periodBoundry[0] && dayNight.time < periodBoundry[1])
            {
                if (Vector3.Distance(chair.transform.position, transform.position) > 1)
                {
                    navMeshAgent.SetDestination(chair.transform.position);
                }
                else
                {
                    Vector3 newRotation = chair.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
                    Debug.Log(chair.transform.rotation.eulerAngles);
                    Debug.Log(transform.rotation.eulerAngles);
                    newRotation.x = 0;
                    transform.Rotate(newRotation);
                }
            }
            else
            if (Vector3.Distance(exit.transform.position, transform.position) > 1)
            {
                navMeshAgent.SetDestination(exit.transform.position);
            }
        }
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
