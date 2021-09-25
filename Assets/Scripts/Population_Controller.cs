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
    public GameObject UI;

    public int numberOfPeople;
    private DayNightCycle dayNightCycle;
    private UICounter uICounter;
    private UnityEvent checkActivationPeriod;
    private float lastTime;

    [SerializeField]
    public List<Vaccine> vaccines;
    private string[] vacNames = { "AstraZeneca", "Pfizer", "Coronavac" };
    private float[] vacEfficacies = { 0.70f, 0.95f, 0.50f };
    private float[] vacDistributions = { 0.70f, 0.95f, 0.50f};
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
        vaccines = generateVaccineList(vacNames, vacEfficacies, vacDistributions);
        Transform[] tableChairs = new Transform[classRooms.transform.childCount];
        dayNightCycle = dayNightObject.GetComponent<DayNightCycle>();
        uICounter = UI.GetComponent<UICounter>();
        for (int i = 0; i < classRooms.transform.childCount; i++)
        {
            tableChairs[i] = classRooms.transform.GetChild(i).transform.Find("Tables and Chairs");
        }
        
        int numberOfPairsPerClassRoom = tableChairs[0].childCount;
        List<int> chairIndexesList = new List<int>();

        for(int i = 0; i < numberOfPairsPerClassRoom; i++)
        {
            chairIndexesList.Add(i);
        }

        createPopulation(tableChairs, chairIndexesList);

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

    void createPopulation(Transform[] tableChairs, List<int> chairIndexesList)
    {
        List<List<int>> periodChairIndexesList = new List<List<int>>();

        int infectedNumber = 0;

        foreach(periods period in Periods)
        {
            periodChairIndexesList.Add(new List<int>(chairIndexesList));
        }

        int[] currentClassRoom = {0, 0, 0};
        for (int i = 0; i < numberOfPeople; i++)
        {
            int currentPeriod = i % 3;
            Instantiate(personPrefab, exit.transform.position, Quaternion.identity, transform);

            int randomIndex = UnityEngine.Random.Range(0, periodChairIndexesList[currentPeriod].Count);
            Transform personTransform = transform.GetChild(i);

            Person person = personTransform.GetComponent<Person>();

            float initialInfectedProbability = Random.Range(0f, 1f);
            if (initialInfectedProbability < 0.5f)
            {
                person.isInfected = true;
                infectedNumber++;
            }
            else
            {
                person.isInfected = false;
            }
            person.uiCounter = uICounter;

            AgentNavigation personNav = personTransform.GetComponent<AgentNavigation>();
            Transform extractedChair = tableChairs[currentClassRoom[currentPeriod]].GetChild(periodChairIndexesList[currentPeriod][randomIndex]).GetChild(0).GetChild(0);
            personNav.chair = extractedChair.gameObject;

            periodChairIndexesList[currentPeriod].RemoveAt(randomIndex);
            personNav.periodBoundry = getPeriodBoundry((periods) currentPeriod);
            personNav.dayNight = dayNightCycle;
            personNav.exit = exit; 
            personNav.start = start;

            if (periodChairIndexesList[currentPeriod].Count == 0)
            {
                currentClassRoom[currentPeriod]++;
                periodChairIndexesList[currentPeriod] = new List<int>(chairIndexesList);
            }
        }
        uICounter.setInfection(infectedNumber, numberOfPeople);
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

    public List<Vaccine> generateVaccineList(string[] names, float[] efficacies, float[] distributions)
    {
        List<Vaccine> l = new List<Vaccine>();
        for (int i = 0; i < names.Length; i++)
        {
            l.Add(new Vaccine(names[i], efficacies[i], distributions[i]));
        }
        return l;
    }
}
