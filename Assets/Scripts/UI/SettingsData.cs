using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParametersValues", menuName = "Config/ParametersValues")]
public class SettingsData : ScriptableObject
{
    public int numberOfDays = 5;
    public int numberOfAgents = 1500;
    public float[] vaccineProportion = new float[] { 0.30f, 0.44f, 0.25f, 0.01f };
    public float[] usageProportion = new float[] { 0.10f, 0.80f, 0.10f };
    public float percentageOfInfected = 0.10f;
}