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
    public SettingsData settingsData;

    private DayNightCycle dayNightCycle;
    private UICounter uICounter;
    private UnityEvent checkActivationPeriod;
    private float lastTime;

    public float[] maskUsage;

    [SerializeField]
    public List<Vaccine> vaccines;
    public static string[] vacNames = { "AstraZeneca", "Pfizer", "Coronavac", "None" };
    private float[] vacEfficacies = { 0.70f, 0.95f, 0.50f, 0.0f };
    private float[] vacUsage;
    private Color[] vacColor = { new Color(1, 0.9096057f, 0.2688679f), new Color(0.1417534f, 1, 0), new Color(1, 0.5414941f, 0), new Color(1, 0, 0) };
    private float[] vacAsymptomaticChance = { 0.85f, 0.94f, 0.70f, 0.40f };

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
        maskUsage = buildAccumulativeArray(settingsData.usageProportion);
        vacUsage = buildAccumulativeArray(settingsData.vaccineProportion);

        vaccines = generateVaccineList(vacNames, vacEfficacies, vacUsage, vacAsymptomaticChance);

        Transform[] tableChairs = new Transform[classRooms.transform.childCount];
        dayNightCycle = dayNightObject.GetComponent<DayNightCycle>();
        uICounter = UI.GetComponent<UICounter>();
        for (int i = 0; i < classRooms.transform.childCount; i++)
        {
            tableChairs[i] = classRooms.transform.GetChild(i).transform.Find("Tables and Chairs");
        }

        int numberOfPairsPerClassRoom = tableChairs[0].childCount;
        List<int> chairIndexesList = new List<int>();

        for (int i = 0; i < numberOfPairsPerClassRoom; i++)
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
        foreach (periods p in Periods)
        {
            float[] boundry = getPeriodBoundry(p);
            if (lastTime <= boundry[0] && dayNightCycle.time >= boundry[0])
            {
                checkActivationPeriod.Invoke();
            }
        }
        lastTime = dayNightCycle.time;
    }

    float[] buildAccumulativeArray(float[] array)
    {
        float[] copy = (float[])array.Clone();
        float counter = 0;
        for (int i = 0; i < array.Length; i++)
        {
            counter += array[i];
            copy[i] = counter;
        }
        return copy;
    }

    void createPopulation(Transform[] tableChairs, List<int> chairIndexesList)
    {
        List<List<int>> periodChairIndexesList = new List<List<int>>();

        List<int> initialInfectionList = generateInfectedList();

        ReportData.generateVaccineList(vacNames);
        ReportData.newDay();

        int infectedNumber = 0;

        foreach (periods period in Periods)
        {
            periodChairIndexesList.Add(new List<int>(chairIndexesList));
        }

        int[] currentClassRoom = { 0, 0, 0 };
        for (int i = 0; i < settingsData.numberOfAgents; i++)
        {
            int currentPeriod = i % 3;
            Instantiate(personPrefab, exit.transform.position, Quaternion.identity, transform);

            int randomIndex = UnityEngine.Random.Range(0, periodChairIndexesList[currentPeriod].Count);
            Transform personTransform = transform.GetChild(i);

            Person person = personTransform.GetComponent<Person>();

            person.skinnedMeshRenderer = person.transform.GetChild(3).GetComponent<SkinnedMeshRenderer>();

            int infectionIndex = Random.Range(0, initialInfectionList.Count);
            if (initialInfectionList[infectionIndex] == 1)
            {
                person.isInfected = true;
                person.SetStatusValuesOnInfection();
                infectedNumber++;
                for(int daysBefore = 0; daysBefore < (int)Mathf.Round(Random.Range(1f, 7f)); daysBefore++)
                {
                    person.UpdateStatus(daysBefore, false);
                }
                person.skinnedMeshRenderer.material.color = new Color(1, 0, 0);
            }
            else
            {
                person.isInfected = false;
            }
            initialInfectionList.RemoveAt(infectionIndex);

            float randomMaskProb = Random.Range(0f, 1f);

            if (randomMaskProb <= maskUsage[0])
            {
                person.mask = Person.Masks.Cloth;
                if (person.isInfected)
                    ReportData.maskInfected[0][0]++;
                ReportData.maskTotal[0]++;
            }
            else if (randomMaskProb <= maskUsage[1])
            {
                person.mask = Person.Masks.N95;
                if (person.isInfected)
                    ReportData.maskInfected[1][0]++;
                ReportData.maskTotal[1]++;
            }
            else
            {
                person.mask = Person.Masks.None;
                if (person.isInfected)
                    ReportData.maskInfected[2][0]++;
                ReportData.maskTotal[2]++;
            }

            float randomVaccineProb = Random.Range(0f, 1f);
            Outline personOutline = person.transform.GetChild(3).GetComponent<Outline>();

            if (randomVaccineProb <= vacUsage[0])
            {
                person.vaccine = vaccines[0];
                personOutline.OutlineColor = vacColor[0];
                if (person.isInfected)
                    ReportData.vaccineInfected[0][0]++;
                ReportData.vaccineTotal[0]++;
            }
            else if (randomVaccineProb <= vacUsage[1])
            {
                person.vaccine = vaccines[1];
                personOutline.OutlineColor = vacColor[1];
                if (person.isInfected)
                    ReportData.vaccineInfected[1][0]++;
                ReportData.vaccineTotal[1]++;

            }
            else if (randomVaccineProb <= vacUsage[2])
            {
                person.vaccine = vaccines[2];
                personOutline.OutlineColor = vacColor[2];
                if (person.isInfected)
                    ReportData.vaccineInfected[2][0]++;
                ReportData.vaccineTotal[2]++;

            }
            else
            {
                person.vaccine = vaccines[3];
                personOutline.OutlineColor = vacColor[3];
                if (person.isInfected)
                    ReportData.vaccineInfected[3][0]++;
                ReportData.vaccineTotal[3]++;
            }

            person.uiCounter = uICounter;

            AgentNavigation personNav = personTransform.GetComponent<AgentNavigation>();
            Transform extractedChair = tableChairs[currentClassRoom[currentPeriod]].GetChild(periodChairIndexesList[currentPeriod][randomIndex]).GetChild(0).GetChild(0);
            personNav.chair = extractedChair.gameObject;

            periodChairIndexesList[currentPeriod].RemoveAt(randomIndex);
            personNav.periodBoundry = getPeriodBoundry((periods)currentPeriod);
            personNav.dayNight = dayNightCycle;
            personNav.exit = exit;

            int randomStartingPoint = UnityEngine.Random.Range(0, start.transform.childCount);
            Bounds[] startingBounds = new Bounds[start.transform.childCount];
            for (int j = 0; j < start.transform.childCount; j++)
            {
                startingBounds[j] = start.transform.GetChild(randomStartingPoint).GetComponent<BoxCollider>().bounds;
            }
            personNav.start = RandomPointInBounds(startingBounds[randomStartingPoint]);

            if (periodChairIndexesList[currentPeriod].Count == 0)
            {
                currentClassRoom[currentPeriod]++;
                periodChairIndexesList[currentPeriod] = new List<int>(chairIndexesList);
            }
        }
        uICounter.setInfection(infectedNumber);
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

    public void activatePersons()
    {
        foreach (Transform person in transform)
        {
            AgentNavigation navAgent = person.gameObject.GetComponent<AgentNavigation>();
            GameObject actualPerson = person.gameObject;
            if (!(actualPerson.activeSelf) && navAgent.periodBoundry[0] <= dayNightCycle.time && navAgent.periodBoundry[1] >= dayNightCycle.time)
            {
                actualPerson.SetActive(true);
            }
        }
    }

    public List<Vaccine> generateVaccineList(string[] names, float[] efficacies, float[] distributions, float[] asymptomaticChance)
    {
        List<Vaccine> l = new List<Vaccine>();
        for (int i = 0; i < names.Length; i++)
        {
            l.Add(new Vaccine(names[i], efficacies[i], distributions[i], asymptomaticChance[i]));
        }
        return l;
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private List<int> generateInfectedList()
    {
        List<int> infectedArray = new List<int>();
        for (int i = 0; i < settingsData.numberOfAgents * settingsData.percentageOfInfected; i++)
        {
            infectedArray.Add(1);
        }
        for (int i = 0; i < settingsData.numberOfAgents * (1 - settingsData.percentageOfInfected); i++)
        {
            infectedArray.Add(0);
        }
        return infectedArray;
    }

    public void UpdatePopulationStatus(int dayNumber)
    {
        foreach (Transform person in transform)
        {
            person.GetComponent<Person>().UpdateStatus(dayNumber, true);
        }
    }
}
