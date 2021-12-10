using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class AgentNavigation : MonoBehaviour
{
    public GameObject chair;
    public GameObject exit;
    public Vector3 start;
    private NavMeshAgent navMeshAgent;
    public float[] periodBoundry;
    public DayNightCycle dayNight;
    public Animator animator;
    private Person person;
    public NavMeshPath chairPath;
    public NavMeshPath exitPath;
    public bool isLeaving = false;
    public bool hasStopped = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        person = transform.GetComponent<Person>();
    }

    private void Update()
    {
        if (navMeshAgent.velocity == Vector3.zero && !hasStopped && gameObject.activeSelf)
        {
            StartCoroutine(checkHasStopped());
        }
        if (chair && exit && (person.numOfDaysAway == -1 || person.numOfDaysToDetect > -1))
        {
            if (dayNight.time > periodBoundry[0] && dayNight.time < periodBoundry[1])
            {
                if (Vector3.Distance(chair.transform.position, transform.position) > 1)
                {
                    animator.SetBool("isWalking", true);
                    isLeaving = false;
                    if (chairPath == null)
                    {
                        navMeshAgent.ResetPath();
                        chairPath = new NavMeshPath();
                        navMeshAgent.CalculatePath(chair.transform.position, chairPath);
                    }
                    else if (navMeshAgent.velocity == Vector3.zero && hasStopped)
                    {
                        navMeshAgent.ResetPath();
                        transform.position = chair.transform.position + new Vector3(0, 0.5f, 0);
                        Vector3 newRotation = chair.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
                        newRotation.x = 0;
                        transform.Rotate(newRotation);
                        animator.SetBool("isWalking", false);
                    }
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
                isLeaving = true;
                if (exitPath == null)
                {
                    navMeshAgent.ResetPath();
                    exitPath = new NavMeshPath();
                    navMeshAgent.CalculatePath(exit.transform.position, exitPath);
                    hasStopped = false;
                }
                else if (navMeshAgent.velocity == Vector3.zero && hasStopped)
                {
                    navMeshAgent.ResetPath();
                    transform.position = exit.transform.position;
                }
                animator.SetBool("isWalking", true);
            }
        }
        else if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            chairPath = null;
            exitPath = null;
            hasStopped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        chairPath = null;
        exitPath = null;
        hasStopped = false;
    }

    private void OnDisable()
    {
        transform.position = start;
    }

    private IEnumerator checkHasStopped()
    {
        yield return new WaitForSecondsRealtime(3f);
        if (navMeshAgent.velocity == Vector3.zero) hasStopped = true;
        else hasStopped = false;
    }
}
