using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agents_Controller : MonoBehaviour
{
    private List<NavMeshAgent> navMeshAgents;
    private List<NavMeshPath> paths;
    void Update()
    {
        navMeshAgents = new List<NavMeshAgent>();
        paths = new List<NavMeshPath>();
        bool isReady = true;
        foreach (Transform child in transform)
        {
            GameObject gameObjectChild = child.gameObject;
            if (gameObjectChild.activeSelf)
            {
                NavMeshAgent navMeshAgent = gameObjectChild.GetComponent<NavMeshAgent>();
                AgentNavigation agentNavigations = gameObjectChild.GetComponent<AgentNavigation>();
                navMeshAgents.Add(navMeshAgent);
                if(agentNavigations.isLeaving)
                    paths.Add(agentNavigations.exitPath);
                else
                    paths.Add(agentNavigations.chairPath);
                if (navMeshAgent.pathPending || (agentNavigations.isLeaving && agentNavigations.exitPath == null) || (!agentNavigations.isLeaving && agentNavigations.chairPath == null)) isReady = false; 
            }
        }
        if(isReady)
        {

            for (int i = 0; i < navMeshAgents.Count; i++)
            {
                navMeshAgents[i].SetPath(paths[i]);
            }
        }
    }
}
