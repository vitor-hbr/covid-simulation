using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize_Population : MonoBehaviour
{
    public int numberOfPeople;
    public GameObject personPrefab;
    public GameObject classRooms;

    private void Start()
    {
        Transform chairs = classRooms.transform.GetChild(0).transform.Find("Chairs");
        int numberOfChairs = chairs.childCount;
        List<int> chairIndexesList = new List<int>();

        for(int i = 0; i < numberOfChairs; i++)
        {
            chairIndexesList.Add(i);
        }

        StartCoroutine(createPopulation(chairs, chairIndexesList));
    }

    IEnumerator createPopulation(Transform chairs, List<int> chairIndexesList)
    {
        for (int i = 0; i < numberOfPeople; i++)
        {
            Instantiate(personPrefab, new Vector3(0, -3.08f, 6.63f), Quaternion.identity, transform);
            int randomIndex = UnityEngine.Random.Range(0, chairIndexesList.Count);
            Transform person = transform.GetChild(i);
            person.GetComponent<AgentNavigation>().chair = chairs.GetChild(chairIndexesList[randomIndex]).gameObject;
            chairIndexesList.RemoveAt(randomIndex);
            yield return new WaitForSeconds(2);
        }
    }
}
