using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReportValues", menuName = "Config/ReportValues")]
public class ReportData : ScriptableObject
{
    public static List<int> numberOfInfectByDay = new List<int>();
    public static int totalInfected;
    public static List<List<int>> maskInfected = generateMaskList();
    public static List<List<int>> vaccineInfected;
    public static List<int> vaccineTotal = new List<int>{ 0, 0, 0, 0 };
    public static List<int> maskTotal = new List<int>{0,0,0};

    static List<List<int>> generateMaskList()
    {
        List<List<int>> maskInfectedArray = new List<List<int>>();
        foreach (Person.Masks i in Enum.GetValues(typeof(Person.Masks)))
        {
            maskInfectedArray.Add(new List<int>());
        }
        return maskInfectedArray;
    }

    public static void generateVaccineList(string[] vaccineNames)
    {
        vaccineInfected = new List<List<int>>();
        foreach (string s in vaccineNames)
        {
            vaccineInfected.Add(new List<int>());
        }
    }

    public static void newDay()
    {
        numberOfInfectByDay.Add(0);

        foreach (var vacList in vaccineInfected)
        {
            vacList.Add(0);
        }

        foreach (var maskList in maskInfected)
        {
            maskList.Add(0);
        }
    }

    public static float avgDailyCases()
    {
        int realTotalInfected = totalInfected - numberOfInfectByDay[0];
        return (float) (realTotalInfected / (numberOfInfectByDay.Count));
    }
}
