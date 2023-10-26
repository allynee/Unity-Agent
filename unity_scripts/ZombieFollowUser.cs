using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
public class ZombieFollowUser : SceneAPI
{
    private NavMeshAgent navMeshAgent;
    private GameObject zombie1GameObject;
    private void Start()
    {   

        // 1. Retrieve the object
        // 2. Check if it has a NavMeshAgent component
        // 3. If it doesn't, add one
        // 4. For continuous movement of the object, use Update() to set the destination of the NavMeshAgent

        foreach (GameObject obj in allObjectsInScene)
        {
            if (obj.name=="Zombie1"){
                Debug.Log("Found Zombie1");
                zombie1GameObject = obj;
                break;
            }
        }
        
        if(zombie1GameObject == null)
        {
            Debug.LogError("Zombie1 object not found in the scene!");
            return;
        }

        // Try to get the NavMeshAgent component from the Zombie1 object
        navMeshAgent = zombie1GameObject.GetComponent<NavMeshAgent>();

        // If the zombie doesn't have a NavMeshAgent, add one
        if (navMeshAgent == null)
        {
            navMeshAgent = zombie1GameObject.AddComponent<NavMeshAgent>();

            // Set some default properties for the NavMeshAgent.
            navMeshAgent.speed = 3.5f;
            navMeshAgent.angularSpeed = 120;
            navMeshAgent.acceleration = 8;
        }
    }

    private void Update()
    {
        Vector3D userFeetPosition3D = GetUsersFeetPosition();
        Vector3 userFeetPosition = userFeetPosition3D.ToVector3(); 
        if (navMeshAgent != null)
        {
            navMeshAgent.SetDestination(userFeetPosition);
        }
    }
}
