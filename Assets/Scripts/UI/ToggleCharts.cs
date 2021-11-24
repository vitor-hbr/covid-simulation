using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToggleCharts : MonoBehaviour
{
    public GameObject notInfected;
    public GameObject Infected;

    TextMeshProUGUI textLabel;
    void Start()
    {
        textLabel = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    
    public void onToggle()
    {
        notInfected.SetActive(!notInfected.activeInHierarchy);
        Infected.SetActive(!Infected.activeInHierarchy);
        
        if(textLabel.text.Equals("See Infected"))
        {
            textLabel.text = "See Not Infected";
        } else
        {
            textLabel.text = "See Infected";
        }
    }
}
