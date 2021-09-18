using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Population_Controller : MonoBehaviour
{
    public GameObject personPrefab;
    public GameObject classRooms;
    public GameObject start;
    public GameObject exit;
    public GameObject dayNightObject;

    public int numberOfPeople;
    public DayNightCycle dayNightCycle;

    private UnityEvent checkActivationPeriod;
    private float lastTime;

    public enum periods
    {
        morning,
        afternoon,
        night
    }

    private periods[] Periods = {
        periods.morning,
        periods.afternoon,
        periods.night
    };

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

        lastTime = dayNightCycle.time;
        checkActivationPeriod = new UnityEvent();
        checkActivationPeriod.AddListener(activatePersons);
    }
    
    private void Update()
    {
        foreach (periods p in Periods) {
            float[] boundry = getPeriodBoundry(p);
            if (lastTime <= boundry[0] && dayNightCycle.time >= boundry[0]) {
                checkActivationPeriod.Invoke();
            }
        }
        lastTime = dayNightCycle.time;
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
            personNav.periodBoundry = getPeriodBoundry(currentPeriod);
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

    public float[] getPeriodBoundry(periods period)
    {
        float[] boundries = new float[2];
        switch (period)
        {
            case periods.morning:
                boundries[0] = (8f / 24f);
                boundries[1] = (12f / 24f);
                break;
            case periods.afternoon:
                boundries[0] = (12f / 24f);
                boundries[1] = (18f / 24f);
                break;
            case periods.night:
                boundries[0] = (18f / 24f);
                boundries[1] = (22f / 24f);
                break;
        }
        return boundries;
    }

    public void activatePersons(){
        foreach(Transform person in transform)
        {
            AgentNavigation navAgent = person.gameObject.GetComponent<AgentNavigation>();
            GameObject actualPerson = person.gameObject;
            if(!(actualPerson.activeSelf) && navAgent.periodBoundry[0] <= dayNightCycle.time && navAgent.periodBoundry[1] >= dayNightCycle.time)
            {
                actualPerson.SetActive(true);
            }
        }
    }
}
