using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullReport : MonoBehaviour
{
    public SettingsData settingsData;
    public GameObject Page1Chart;
    public GameObject Page2Chart;
    public GameObject Page3Charts;
    public GameObject Page3LegendNI;
    public GameObject Page3LegendI;
    public GameObject Page4Charts;
    public GameObject Page4LegendNI;
    public GameObject Page4LegendI;
    public 
    void Start()
    {
        setDataPage1();
        setDataPage2();
        setDataPage3();
        setDataPage4();
    }

    void setDataPage1()
    {
        Page1Chart.GetComponent<LineChart>().ShowGraph(ReportData.numberOfInfectByDay);
    }

    void setDataPage2()
    {
        int accumulative = 0;
        List<int> accumulativeList = new List<int>();
        foreach (int infectedByDay in ReportData.numberOfInfectByDay)
        {
            accumulative += infectedByDay;
            accumulativeList.Add(accumulative);
        }
        Page2Chart.GetComponent<LineChart>().ShowGraph(accumulativeList);
    }

    void setDataPage3()
    {
        setPieChartData(Page3Charts, ReportData.vaccineInfected, ReportData.vaccineTotal, Page3LegendNI, Page3LegendI);
    }

    void setDataPage4()
    {
        setPieChartData(Page4Charts, ReportData.maskInfected, ReportData.maskTotal, Page4LegendNI, Page4LegendI);
    }

    void setPieChartData(GameObject PageCharts, List<List<int>> listInfectionReportData, List<int> totalReportData, GameObject legendObject0, GameObject legendObject1)
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

            initialChart.UpdateChart(initialValues, legendObject0, legendObject1, false, i);
            finalChart.UpdateChart(finalValues, legendObject0, legendObject1, true, i);
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
