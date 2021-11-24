using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReportUI : MonoBehaviour
{
    public SettingsData settingsData;
    public GameObject numberDaysObject;
    public GameObject numberAgentsObject;
    public GameObject initialInfectedObject;
    public GameObject vaccineProportionsObject;
    public GameObject maskProportionsObject;
    public GameObject finalInfectedObject;
    public GameObject dailyCaseObject;

    public GameObject[] pages = new GameObject[5];
    void Start()
    {
        numberDaysObject.GetComponent<TextMeshProUGUI>().text += (ReportData.numberOfInfectByDay.Count) - 1;        
        numberAgentsObject.GetComponent<TextMeshProUGUI>().text += settingsData.numberOfAgents;
        initialInfectedObject.GetComponent<TextMeshProUGUI>().text += (settingsData.percentageOfInfected * 100).ToString("N2") + "%";
        finalInfectedObject.GetComponent<TextMeshProUGUI>().text += (((float) ReportData.totalInfected / settingsData.numberOfAgents) * 100).ToString("N2") + "%";
        dailyCaseObject.GetComponent<TextMeshProUGUI>().text += ReportData.avgDailyCases();

        for (int i = 0; i < vaccineProportionsObject.transform.childCount; i++)
        {
            Transform child = vaccineProportionsObject.transform.GetChild(i);
            TextMeshProUGUI childText = child.GetComponent<TextMeshProUGUI>();
            string vacProportion = (settingsData.vaccineProportion[i] * 100).ToString("N2") + "%";
            childText.text += vacProportion;
        }

        for (int i = 0; i < maskProportionsObject.transform.childCount; i++)
        {
            Transform child = maskProportionsObject.transform.GetChild(i);
            TextMeshProUGUI childText = child.GetComponent<TextMeshProUGUI>();
            string maskProportion = (settingsData.usageProportion[i] * 100).ToString("N2") + "%";
            childText.text += maskProportion;
        }

    }

    public void showReportPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if(i == index)
            {
                pages[i].SetActive(true);
            } else
            {
                pages[i].SetActive(false);
            }
        }
    }
}
