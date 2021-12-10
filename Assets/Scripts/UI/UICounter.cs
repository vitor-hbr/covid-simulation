using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UICounter : MonoBehaviour
{
    private TextMeshProUGUI infected;
    private TextMeshProUGUI time;
    private TextMeshProUGUI distance;
    private TextMeshProUGUI speed;
    private TextMeshProUGUI days;
    private TextMeshProUGUI state;
    private float prevTimeScale;
    public GameObject cameraObject;
    public GameObject dayNightObject;
    public SettingsData settingsData;
    private DayNightCycle dayNightCycle;
    private Camera camera;

    private void Awake()
    {
        Transform InfectedText = transform.GetChild(0);
        infected = InfectedText.GetComponent<TextMeshProUGUI>();
        Transform TimeText = transform.GetChild(1);
        time = TimeText.GetComponent<TextMeshProUGUI>();
        Transform SpeedText = transform.GetChild(2);
        speed = SpeedText.GetComponent<TextMeshProUGUI>();
        Transform DistanceText = transform.GetChild(3);
        distance = DistanceText.GetComponent<TextMeshProUGUI>();
        Transform DaysText = transform.GetChild(4);
        days = DaysText.GetComponent<TextMeshProUGUI>();
        Transform StateText = transform.GetChild(5);
        state = StateText.GetComponent<TextMeshProUGUI>();

        speed.text = $"speed: {Time.timeScale}x";

        camera = cameraObject.GetComponent<Camera>();
        dayNightCycle = dayNightObject.GetComponent<DayNightCycle>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            camera.farClipPlane += 5;
            distance.text = $"distance: {camera.farClipPlane}m";
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (camera.farClipPlane > 5)
            camera.farClipPlane -= 5;
            distance.text = $"distance: {camera.farClipPlane}m";
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Time.timeScale > 1)
            {
                Time.timeScale -= 1;
                speed.text = $"speed: {Time.timeScale}x";
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Time.timeScale += 1;
            speed.text = $"speed: {Time.timeScale}x";
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.timeScale != 0)
            {
                prevTimeScale = Time.timeScale;
                Time.timeScale = 0;
                state.text = "state: paused";
            } else
            {
                Time.timeScale = prevTimeScale;
                state.text = "state: playing";
            }
        } else if (Input.GetKeyDown(KeyCode.K))
        {
            dayNightCycle.time = 0.99f;
        }
    }

    public void newInfection(Person.Masks maskType, Vaccine vaccineType)
    {
        ReportData.numberOfInfectByDay[dayNightCycle.currentDayNumber] += 1;
        ReportData.totalInfected += 1;
            if(maskType == Person.Masks.Cloth)
                ReportData.maskInfected[0][dayNightCycle.currentDayNumber]++;
             else  if(maskType == Person.Masks.N95)
                ReportData.maskInfected[1][dayNightCycle.currentDayNumber]++;
             else
                ReportData.maskInfected[2][dayNightCycle.currentDayNumber]++;

        int iterator = 0;
        foreach (string item in Population_Controller.vacNames)
        {
            if(item == vaccineType.name)
            {
                ReportData.vaccineInfected[iterator][dayNightCycle.currentDayNumber]++;
            }
            iterator++;
        }

        updateUI();
    }

    public void setInfection(int infected)
    {
        ReportData.totalInfected = infected;
        ReportData.numberOfInfectByDay[0] = infected;
        
        updateUI();
    }
    public void setTime(string timeText)
    {
        time.SetText(timeText);
    }

    public void setDays(int currentDayNumber, int totalDaysNumber)
    {
        days.SetText($"day {currentDayNumber} of {totalDaysNumber}");
    }

    private void updateUI()
    {
        infected.SetText("deaths: <color=red>" + ReportData.totalDeaths + "</color> <color=white> | infected:</color> <color=red>" + ReportData.totalInfected + "</color>/<color=white>" + settingsData.numberOfAgents + "</color>");
    }

}
