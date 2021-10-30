using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UISettings : MonoBehaviour
{
    public SettingsData settingsData;
    public GameObject VacObject;
    public GameObject UsageObject;
    public Slider[] VaccineSliders;
    public TMP_Text[] VaccineSlidersValues;
    private Slider[] UsageSliders;
    public TMP_InputField numberOfDaysInput;
    public TMP_InputField numberOfAgentsInput;
    public Slider percentageInfectSlider;
    private float[] prevVacValues;
    private float[] preUsageValues;

    void Start()
    {
        //numberOfDaysInput.text = settingsData.numberOfDays.ToString();
        //numberOfAgentsInput.text = settingsData.numberOfAgents.ToString();

        //for (int i = 0; i < VacObject.transform.childCount; i++)
        //{
        //    VaccineSliders[i] = VacObject.transform.GetChild(i).GetComponent<Slider>();
        //    VaccineSliders[i].value = settingsData.vaccineProportion[i];
        //    prevVacValues[i] = settingsData.vaccineProportion[i];
        //}

        //for (int i = 0; i < UsageObject.transform.childCount; i++)
        //{
        //    UsageSliders[i] = UsageObject.transform.GetChild(i).GetComponent<Slider>();
        //    UsageSliders[i].value = settingsData.usageProportion[i];
        //    preUsageValues[i] = settingsData.usageProportion[i];
        //}

        for (int i = 0; i < VaccineSliders.Length; i++)
        {
            Slider vaccineSlider = VaccineSliders[i];
            vaccineSlider.value = settingsData.vaccineProportion[i];
        }
        

        //percentageInfectSlider.value = settingsData.percentageOfInfected;
    }   

    public void OnChangeVacSlider(int sliderOpt)
    {
        float remaining = 0;
        float total = 0;
        int iterator = 0;
        foreach (Slider vaccineSlider in VaccineSliders)
        {
            total += vaccineSlider.value;
            VaccineSlidersValues[iterator].text = (VaccineSliders[iterator].value).ToString();
            settingsData.vaccineProportion[iterator++] = vaccineSlider.value/100;
        }
        remaining = 100 - total;
            Debug.Log("Remaining" + remaining);
        if(remaining < 0)
        {
            int opt = 0;
            for (int i = 0; i < VaccineSliders.Length; i++)
            {
                if(i != sliderOpt)
                {
                    if(VaccineSliders[i].value > VaccineSliders[opt].value)
                    {
                        opt = i;
                    }
                }
            }
            settingsData.vaccineProportion[opt] -= (remaining * -1) / 100;

            iterator = 0;
            foreach (Slider vaccineSlider in VaccineSliders)
            {
                VaccineSlidersValues[iterator].text = (VaccineSliders[iterator].value).ToString();
                vaccineSlider.value = settingsData.vaccineProportion[iterator++] * 100;
            }

        }
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
        
    }
}
