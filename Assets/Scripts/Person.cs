using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    private enum Actions {
        Breathing = 1,
        Coughing = 5,
        Sneezing = 10
    }

    public bool isInfected = false;
    public float infectionProbability = 0.03f;
    public int collisionThreshold = 100;
    public int collisionCounter = 0;
    public List<ParticleCollisionEvent> collisionEvents;
    public UICounter uiCounter;

    private float timeThreshold = 0;
    private Actions currentAction = Actions.Breathing;
    private ParticleSystem particles;


    void Start()
    {
        particles = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
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
            if(collisionCounter > collisionThreshold)
            {
                float infect = Random.Range(0f, 1f);
                if (infect <= infectionProbability)
                {
                    isInfected = true;
                    uiCounter.newInfection();
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
        if (actionProbability <= 0.01)
        {
            currentAction = Actions.Sneezing;
        }
        else if (actionProbability <= 0.06)
        {
            currentAction = Actions.Coughing;
        }
        else
        {
            currentAction = Actions.Breathing;
        }
        var main = particles.main;
        float currentActionFloat = (float)currentAction;
        //main.duration = 1f / currentActionFloat;
        main.startSpeed = 0.15f * currentActionFloat;
        var emission = particles.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(200f * currentActionFloat, 300f * currentActionFloat);
        var velocityOverTime = particles.velocityOverLifetime;
        velocityOverTime.x = 0f;
        velocityOverTime.y = -0.1f * currentActionFloat / 2;
        velocityOverTime.z = -0.05f * currentActionFloat;
        particles.Play();
    }

}
