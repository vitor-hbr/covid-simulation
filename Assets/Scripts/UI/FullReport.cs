using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullReport : MonoBehaviour
{
    public SettingsData settingsData;

    public GameObject Page3Charts;
    public GameObject Page4Charts;


    void Start()
    {
        setDataPage1();
        setDataPage2();
        setDataPage3();
        setDataPage4();
    }

    void setDataPage1()
    {

    }

    void setDataPage2()
    {

    }

    void setDataPage3()
    {
        setPieChartData(Page3Charts, ReportData.vaccineInfected, ReportData.vaccineTotal);
    }

    void setDataPage4()
    {
        setPieChartData(Page4Charts, ReportData.maskInfected, ReportData.maskTotal);
    }

    void setPieChartData(GameObject PageCharts, List<List<int>> listInfectionReportData, List<int> totalReportData)
    {
        for (int i = 0; i < PageCharts.transform.childCount; i++)
        {
            Transform chartGroup = PageCharts.transform.GetChild(i);
            Transform initial = chartGroup.GetChild(0);
            Transform final = chartGroup.GetChild(1);

            PieChart initialChart = initial.GetComponent<PieChart>();
            PieChart finalChart = final.GetComponent<PieChart>();

            int[] initialValues = new int[initialChart.colors.Length];
            int[] finalValues = new int[finalChart.colors.Length];

            for (int indexOf = 0; indexOf < initialChart.colors.Length; indexOf++)
            {
                initialValues[indexOf] = listInfectionReportData[indexOf][0];
            }

            for (int indexOf = 0; indexOf< finalChart.colors.Length; indexOf++)
            {
                int sum = 0;
                foreach (int Infects in listInfectionReportData[indexOf])
                {
                    sum += Infects;
                }
                finalValues[indexOf] = sum;
            }

            if (i == 0)
            {
                calculateNonInfectedOfUsers(initialValues, totalReportData);
                calculateNonInfectedOfUsers(finalValues, totalReportData);
            }

            initialChart.UpdateChart(initialValues);
            finalChart.UpdateChart(finalValues);
        }
    }

    void calculateNonInfectedOfUsers(int[] infectedValues, List<int> totalUsers)
    {
        for (int i = 0; i < totalUsers.Count; i++)
        {
            infectedValues[i] = (totalUsers[i] - infectedValues[i]);
        }
    }
}
