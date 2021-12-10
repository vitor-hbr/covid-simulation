using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Person : MonoBehaviour
{
    private enum Actions {
        Breathing = 2,
        Coughing = 4,
        Sneezing = 6
    }

    public enum Masks
    {
        Cloth = 45,
        N95 = 5,
        None = 100,
    }

    public bool isInfected = false;

    public float infectionProbability = 0.2057f;
    public int collisionThreshold;
    public int collisionCounter = 0;
    public List<ParticleCollisionEvent> collisionEvents;
    public UICounter uiCounter;
    public int onlyOneAction = -1;
    public Vaccine vaccine;
    public Masks mask = Masks.None;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    private float timeThreshold = 0;
    private Actions currentAction = Actions.Breathing;
    private ParticleSystem particles;


    private bool isAsymptomatic = false;
    private int numOfDaysOfLife = -1;
    public int numOfDaysAway = -1;
    public int numOfDaysToDetect = -1;
    private int numOfDaysAsymptomatic = -1;

    void Start()
    {
        particles = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        collisionThreshold = (int) (30 * (2 - ((float) mask / 100)));
    }

    void Update()
    {
        if(isInfected && timeThreshold > 1)
        {
            makeAction();
        } else if (isInfected)
        {
            timeThreshold += Time.deltaTime;
        } else
        {
            if (collisionCounter > collisionThreshold)
            {
                float infect = Random.Range(0f, 1f);
                if (infect <= infectionProbability * (1 - vaccine.efficacy))
                {
                    isInfected = true;
                    SetStatusValuesOnInfection();
                    uiCounter.newInfection(mask, vaccine);
                }
                collisionCounter = 0;
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        collisionCounter++;
    }
    void makeAction ()
    {
        particles.Stop();
        timeThreshold = 0;
        float actionProbability = Random.Range(0f, 1f);
        if (onlyOneAction == (int) Actions.Sneezing || actionProbability <= 0.01)
        {
            currentAction = Actions.Sneezing;
        }
        else if (onlyOneAction == (int)Actions.Coughing || actionProbability <= 0.06)
        {
            currentAction = Actions.Coughing;
        }
        else
        {
            currentAction = Actions.Breathing;
        }
        var main = particles.main;
        float currentActionFloat = (float)currentAction;
        main.startSpeed = 1.5f * (currentActionFloat / 2);
        var noise = particles.noise;

        var emission = particles.emission;
        if (30f * currentActionFloat * ((float)mask / 100) < 10) {
            emission.rateOverTime = 10; 
        }
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(10f * currentActionFloat * ((float) mask / 100), 30f * currentActionFloat * ((float) mask / 100));
        particles.Play();
    }

    public void SetStatusValuesOnInfection()
    {
        float asympProb = Random.Range(0f, 1f);
        if(asympProb <= vaccine.asymptomaticChance)
        {
            skinnedMeshRenderer.material.color = new Color(0.4535f, 0, 0.6320f);
            isAsymptomatic = true;
            onlyOneAction = (int) Actions.Breathing;
            numOfDaysAsymptomatic = 14;
        }
        else
        {
            skinnedMeshRenderer.material.color = new Color(1, 0, 0);

            numOfDaysToDetect = (int) Mathf.Round(Random.Range(1f, 3f));

            if (vaccine.name != "None")
            {
                float cureProb = Random.Range(0f, 1f);
                if(cureProb <= 0.98f)
                {
                    numOfDaysAway = (int)Mathf.Round(Random.Range(14f, 20f));

                } else
                {
                    float liveProb = Random.Range(0f, 1f);
                    if(liveProb <= 0.85f)
                    {
                        numOfDaysAway = 14 + (int) Mathf.Round(Random.Range(7f, 28f));
                    } else
                    {
                        numOfDaysOfLife = 14 + (int) Mathf.Round(Random.Range(1f, 42f));
                        numOfDaysAway = numOfDaysOfLife;
                    }
                }
            } else
            {
                float cureProb = Random.Range(0f, 1f);
                if (cureProb <= 0.87f)
                {
                    numOfDaysAway = (int) Mathf.Round(Random.Range(14f, 20f));

                }
                else
                {
                    float liveProb = Random.Range(0f, 1f);
                    if (liveProb <= 0.84f)
                    {
                        numOfDaysAway = 14 + (int) Mathf.Round(Random.Range(7f, 28f));
                    }
                    else
                    {
                        numOfDaysOfLife = 14 + (int) Mathf.Round(Random.Range(1f, 42f));
                        numOfDaysAway = numOfDaysOfLife;
                    }
                }
            }
        }
    }

    public void UpdateStatus(int numDay, bool report)
    {
        if(isInfected)
        {
            if (isAsymptomatic)
            {
                numOfDaysAsymptomatic--;
                if (numOfDaysAsymptomatic == -1)
                {
                    isAsymptomatic = false;
                    isInfected = false;
                    onlyOneAction = -1;
                }
            }
            else
            {
                if(numOfDaysToDetect > -1)
                {
                    numOfDaysToDetect--;
                }
                else if (numOfDaysAway > -1)
                {
                    numOfDaysAway--;
                    if(numOfDaysAway == -1)
                    {
                        isInfected = false;
                    }
                }
                if(numOfDaysOfLife > -1)
                {
                    numOfDaysOfLife--;
                    if (numOfDaysOfLife == -1) {
                        if (report)
                            ReportData.numberOfDeathsByDay[numDay]++;
                        ReportData.totalDeaths++;
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
