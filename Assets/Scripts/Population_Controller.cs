using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population_Controller : MonoBehaviour
{
    public GameObject personPrefab;
    public GameObject classRooms;
    public GameObject start;
    public GameObject exit;
    public GameObject dayNightObject;

    public int numberOfPeople;
    public DayNightCycle dayNightCycle;

    private void Start()
    {
        Transform[] chairs = new Transform[classRooms.transform.childCount];
        dayNightCycle = dayNightObject.GetComponent<DayNightCycle>();
        for (int i = 0; i < classRooms.transform.childCount; i++)
        {
            chairs[i] = classRooms.transform.GetChild(i).transform.Find("Chairs");
        }
        
        int numberOfChairsPerClassRoom = chairs[0].childCount;
        List<int> chairIndexesList = new List<int>();

        for(int i = 0; i < numberOfChairsPerClassRoom; i++)
        {
            chairIndexesList.Add(i);
        }

        createPopulation(chairs, chairIndexesList);
    }
    
    private void Update()
    {
        foreach(periods boundry in Periods)
        {
            float[] b = dayNightCycle.getPeriodBoundry(boundry);
            if(dayNightCycle.time >= b[0] && dayNightCycle.time <= b[1])
            {
                foreach(Transform person in transform)
                {
                    AgentNavigation navAgent = person.gameObject.GetComponent<AgentNavigation>();
                    if(!person.gameObject.activeSelf && navAgent.periodBoundry[0] == dayNightCycle.time)
                    {
                        person.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    void createPopulation(Transform[] chairs, List<int> chairIndexesList)
    {
        periods currentPeriod = periods.morning;
        List<int> chairIndexesListCopy = new List<int>(chairIndexesList);
        int currentClassRoom = 0;
        for (int i = 0; i < numberOfPeople; i++)
        {
            if(i % (numberOfPeople / 3) == 0 && i != 0)
            {
                chairIndexesListCopy = new List<int>(chairIndexesList);
                currentClassRoom = 0;
                currentPeriod++;
            }
            
            Instantiate(personPrefab, start.transform.position, Quaternion.identity, transform);

            int randomIndex = UnityEngine.Random.Range(0, chairIndexesListCopy.Count);
            Transform person = transform.GetChild(i);
            AgentNavigation personNav = person.GetComponent<AgentNavigation>();
            personNav.chair = chairs[currentClassRoom].GetChild(chairIndexesListCopy[randomIndex]).gameObject;
            chairIndexesListCopy.RemoveAt(randomIndex);
            personNav.periodBoundry = dayNightCycle.getPeriodBoundry(currentPeriod);
            personNav.dayNight = dayNightCycle;
            personNav.exit = exit; 
            personNav.start = start;


            if (chairIndexesListCopy.Count == 0)
            {
                currentClassRoom++;
                chairIndexesListCopy = new List<int>(chairIndexesList);
            }
        }
    }
}
