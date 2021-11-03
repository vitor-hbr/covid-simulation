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
    private Slider[] VaccineSliders = new Slider[4];
    private Slider[] UsageSliders = new Slider[3];
    public TMP_InputField numberOfDaysInput;
    public TMP_InputField numberOfAgentsInput;
    public Slider percentageInfectSlider;
    private float[] prevVacValues = new float[4];
    private float[] prevUsageValues = new float[3];
    public TMP_Text[] UsageSlidersValues;
    public TMP_Text[] VaccineSlidersValues;
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
            prevUsageValues[i] = settingsData.usageProportion[i];
        }

        for (int i = 0; i < VaccineSliders.Length; i++)
        {
            Slider vaccineSlider = VaccineSliders[i];
            vaccineSlider.value = settingsData.vaccineProportion[i];
        }
        

        percentageInfectSlider.value = settingsData.percentageOfInfected;
    }   

    private void onChangeGenericSliders(Slider[] Sliders, TMP_Text[] slidersValues, float[] valuesProportions, 
    float[] prevValues, int sliderOpt) {
        float remaining = 0;
        float total = 0;
        int iterator = 0;
        foreach (Slider slider in Sliders)
        {
            total += slider.value;
            slidersValues[iterator].text = (Sliders[iterator].value).ToString();
            valuesProportions[iterator++] = slider.value/100;
        }
        remaining = 100 - total;
        if(remaining < 0)
        {
            int opt = 0;
            for (int i = 0; i < Sliders.Length; i++)
            {
                if(i != sliderOpt)
                {
                    if(Sliders[i].value > Sliders[opt].value)
                    {
                        opt = i;
                    }
                }
            }
            valuesProportions[opt] -= (remaining * -1) / 100;

            iterator = 0;
            foreach (Slider slider in Sliders)
            {
                slidersValues[iterator].text = (Sliders[iterator].value).ToString();
                slider.value = valuesProportions[iterator++] * 100;
            }

        }
    }

    public void OnChangeVacSlider(int sliderOpt)
    {
        onChangeGenericSliders(VaccineSliders, VaccineSlidersValues, settingsData.vaccineProportion, prevVacValues, sliderOpt);
    }

    public void OnChangeUsageSlider(int sliderOpt)
    {
        onChangeGenericSliders(UsageSliders, UsageSlidersValues, settingsData.usageProportion, prevUsageValues, sliderOpt);
    }

    public void OnPlay()
    {
        settingsData.numberOfDays = int.Parse(numberOfDaysInput.text);
        settingsData.numberOfAgents = int.Parse(numberOfAgentsInput.text);
        settingsData.percentageOfInfected = percentageInfectSlider.value;

        
       
    }
}
