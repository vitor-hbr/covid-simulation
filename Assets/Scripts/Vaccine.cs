using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vaccine
{
    public string name;
    public float efficacy;
    public float popDistribution;
    public float asymptomaticChance;

    public Vaccine(string name, float efficacy, float popDistribution, float asymptomaticChance)
    {
        this.name = name;
        this.efficacy = efficacy;
        this.popDistribution = popDistribution;
        this.asymptomaticChance= asymptomaticChance;
    }
}
