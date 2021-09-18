using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public enum periods
    {
        morning,
        afternoon,
        night
    }

    private periods[] Periods = {
        periods.morning,
        periods.afternoon,
        periods.night
    };

    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
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

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    private void Update()
    {
        time += timeRate * Time.deltaTime;

        if (time >= 1.0f)
            time = 0.0f;

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

    public float[] getPeriodBoundry(periods period)
    {
        float[] boundries = new float[2];
        switch (period)
        {
            case periods.morning:
                boundries[0] = (8f / 24f);
                boundries[1] = (12f / 24f);
                break;
            case periods.afternoon:
                boundries[0] = (12f / 24f);
                boundries[1] = (18f / 24f);
                break;
            case periods.night:
                boundries[0] = (18f / 24f);
                boundries[1] = (22f / 24f);
                break;
        }
        return boundries;
    }
}

