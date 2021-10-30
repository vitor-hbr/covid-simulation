using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using TMPro;

public class UISettings : MonoBehaviour
{
    public SettingsData settingsData;
    public GameObject VacObject;
    public GameObject UsageObject;
    private Slider[] VaccineSliders;
    private Slider[] UsageSliders;
    public TMP_InputField numberOfDaysInput;
    public TMP_InputField numberOfAgentsInput;
    public Slider percentageInfectSlider;
    private float[] prevVacValues;
    private float[] preUsageValues;
    void Start()
    {
        numberOfDaysInput.text = settingsData.numberOfDays.ToString();
        numberOfAgentsInput.text = settingsData.numberOfAgents.ToString();

        for (int i = 0; i < VacObject.transform.childCount; i++)
        {
            VaccineSliders[i] = VacObject.transform.GetChild(i).GetComponent<Slider>();
            VaccineSliders[i].value = settingsData.vaccineProportion[i];
            prevVacValues[i] = settingsData.vaccineProportion[i];
        }

        for (int i = 0; i < UsageObject.transform.childCount; i++)
        {
            UsageSliders[i] = UsageObject.transform.GetChild(i).GetComponent<Slider>();
            UsageSliders[i].value = settingsData.usageProportion[i];
            preUsageValues[i] = settingsData.usageProportion[i];
        }

        percentageInfectSlider.value = settingsData.percentageOfInfected;
    }   

    public void OnChangeVacSlider()
    {

    }

    public void OnPlay()
    {
        settingsData.numberOfDays = int.Parse(numberOfDaysInput.text);
        settingsData.numberOfAgents = int.Parse(numberOfAgentsInput.text);
        settingsData.percentageOfInfected = percentageInfectSlider.value;

        float accumulate = 0;

       
    }

    private void Update()
    {
        for (int i = 0; i < VaccineSliders.Length; i++)
        {
            if(prevVacValues[i] != VaccineSliders[i].value)
            {
                float sum = 0;
                for (int j = 0; j < VaccineSliders.Length; j++)
                {
                    sum += VaccineSliders[j].value;
                }
                if(1 - sum < VaccineSliders[i].value)
                {
                    
                }
            }
        }
    }
}
