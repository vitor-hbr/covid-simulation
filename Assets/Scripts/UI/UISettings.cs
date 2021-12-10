using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class UISettings : MonoBehaviour
{
    public SettingsData settingsData;
    public GameObject VacObject;
    public GameObject UsageObject;
    private Slider[] VaccineSliders = new Slider[4];
    private Slider[] UsageSliders = new Slider[3];
    private TMP_Text percentageValue;
    public TMP_InputField numberOfDaysInput;
    public TMP_InputField numberOfAgentsInput;
    public Slider percentageInfectSlider;
    private float[] prevVacValues = new float[4];
    private float[] prevUsageValues = new float[3];
    public TMP_Text[] UsageSlidersValues;
    public TMP_Text[] VaccineSlidersValues;
    public GameObject percentageValueObject;
    void Awake()
    {
        percentageValue = percentageValueObject.GetComponent<TMP_Text>();
        Reset();
    }   

    private void setSliderValues(Slider[] sliders, float[] prevValues, GameObject objectN, float[] proportions)
    {

        for (int i = 0; i < objectN.transform.childCount; i++)
        {
            sliders[i] = objectN.transform.GetChild(i).GetComponent<Slider>();
            prevValues[i] = proportions[i] * 100;
        }

        float[] proportionsCopy = (float[]) proportions.Clone();
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = proportionsCopy[i] * 100;
        }
    }

    private void onChangeGenericSliders(Slider[] Sliders, TMP_Text[] slidersValues, float[] valuesProportions, 
    float[] prevValues, int sliderOpt) {
        float remaining;
        float total = 0;
        int iterator = 0;
        foreach (Slider slider in Sliders)
        {
            total += slider.value;

            slidersValues[iterator].text = (Sliders[iterator].value).ToString("0.00") + "%";
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
                slidersValues[iterator].text = (Sliders[iterator].value).ToString("0.00") + "%";
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

    public void OnChangePercentageSlider()
    {
        percentageValue.text = (percentageInfectSlider.value * 100).ToString("0.00") + "%";
    }

    private void updateSliderProportions(float[] currentProportions, Slider[] sliders) 
    {
        int i = 0;
        foreach(Slider slider in sliders)
        {
            currentProportions[i++] = slider.value/100;
        }
    }

    public void Reset()
    {
        settingsData.numberOfDays = 5;
        settingsData.numberOfAgents = 6630;
        settingsData.vaccineProportion = new float[] { 0.30f, 0.44f, 0.25f, 0.01f };
        settingsData.usageProportion = new float[] { 0.94f, 0.03f, 0.03f };
        settingsData.percentageOfInfected = 0.10f;


        numberOfDaysInput.text = settingsData.numberOfDays.ToString();
        numberOfAgentsInput.text = settingsData.numberOfAgents.ToString();
        percentageValue.text = (settingsData.percentageOfInfected * 100).ToString("0.00") + "%";

        setSliderValues(VaccineSliders, prevVacValues, VacObject, settingsData.vaccineProportion);
        setSliderValues(UsageSliders, prevUsageValues, UsageObject, settingsData.usageProportion);

        percentageInfectSlider.value = settingsData.percentageOfInfected;
    }

    public void OnPlay()
    {
        settingsData.numberOfDays = int.Parse(numberOfDaysInput.text);
        settingsData.numberOfAgents = int.Parse(numberOfAgentsInput.text);
        settingsData.percentageOfInfected = percentageInfectSlider.value;

        updateSliderProportions(settingsData.vaccineProportion, VaccineSliders);
        updateSliderProportions(settingsData.usageProportion, UsageSliders);

        SceneManager.LoadScene("Simulation", LoadSceneMode.Single);
    }

}
