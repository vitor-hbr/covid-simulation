using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DayNightCycle : MonoBehaviour
{

    [Range(0.0f, 1.0f)]
    public float lastTime;
    public float time;
    private float fullDayLength = 86400;
    public float startTime = 0.98f;
    private float timeRate;
    public int currentDayNumber = 1;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionsIntensityMultiplier;
    public GameObject UI;

    public SettingsData settingsData;
    public GameObject populationObject;
    private Population_Controller population_Controller;
    private UICounter uiCounter;

    private void Start()
    {
        timeRate = 10 * Time.timeScale / fullDayLength ;
        time = startTime;
        uiCounter = UI.GetComponent<UICounter>();
        population_Controller = populationObject.GetComponent<Population_Controller>();
        ReportData.newDay();
        uiCounter.setDays(currentDayNumber, settingsData.numberOfDays);
    }

    private void FixedUpdate()
    {
        time += timeRate * Time.deltaTime;
        uiCounter.setTime(CalculateTimeText());

        if (time >= 1.0f)
        {
            currentDayNumber++;
            time = 0.0f;
            if(currentDayNumber > settingsData.numberOfDays)
            {
                SceneManager.LoadScene("Report", LoadSceneMode.Single);
            } else
            {
                population_Controller.UpdatePopulationStatus(currentDayNumber-1);
                ReportData.newDay();
                uiCounter.setDays(currentDayNumber, settingsData.numberOfDays);
            }
        }

        sun.transform.eulerAngles = (time - 0.25f) * noon * 4.0f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4.0f;

        sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonIntensity.Evaluate(time);

        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);

        if (sun.intensity <= 0 && sun.gameObject.activeInHierarchy) sun.gameObject.SetActive(false);
        else if (sun.intensity > 0 && !sun.gameObject.activeInHierarchy) sun.gameObject.SetActive(true);

        if (moon.intensity <= 0 && moon.gameObject.activeInHierarchy) moon.gameObject.SetActive(false);
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy) moon.gameObject.SetActive(true);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionsIntensityMultiplier.Evaluate(time);
    }

    private string CalculateTimeText()
    {
        int numberOfHours = (int) (time * 24);
        int numberOfMinutes = (int) Mathf.Floor((time * 24 - numberOfHours) * 60);
        return $"{(numberOfHours < 10 ? "0" : "")}{numberOfHours}:{(numberOfMinutes < 10 ? "0" : "")}{numberOfMinutes}";
    }

}

