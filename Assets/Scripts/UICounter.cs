using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICounter : MonoBehaviour
{
    public int infectedPopulation = 0;
    public int totalPopulation = 0;
    private TextMeshProUGUI text;

    private void Awake()
    {
        Transform title = transform.GetChild(0);
        text = title.GetComponent<TextMeshProUGUI>();
        Debug.Log(text);
    }

    public void newInfection()
    {
        infectedPopulation++;
        updateUI();
    }

    public void setInfection(int infected, int totalPopulation)
    {
        infectedPopulation = infected;
        this.totalPopulation = totalPopulation;
        updateUI();
    }

    private void updateUI()
    {
        text.SetText("infected: <color=red>" + infectedPopulation + "</color>/<color=white>" + totalPopulation + "</color>");
    }
}
