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
    public Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (chair && exit)
        {
            if (dayNight.time > periodBoundry[0] && dayNight.time < periodBoundry[1])
            {
                if (Vector3.Distance(chair.transform.position, transform.position) > 3)
                {
                    navMeshAgent.SetDestination(chair.transform.position);
                    animator.SetBool("isWalking", true);
                }
                else
                {
                    Vector3 newRotation = chair.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
                    newRotation.x = 0;
                    transform.Rotate(newRotation);
                    animator.SetBool("isWalking", false);
                }
            }
            else
            { 
                navMeshAgent.SetDestination(exit.transform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        transform.position = start.transform.position;
    }
}
