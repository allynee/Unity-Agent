using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class FollowTheLeader : SceneAPI
{
    private Object3D fox;
    private Object3D zombie;
    private NavMeshAgent foxAgent;
    private NavMeshAgent zombieAgent;

    private void Start()
    {
        CreateFox();
        CreateZombie();
    }

    private void Update()
    {
        UpdateFoxPosition();
        UpdateZombiePosition();
    }

    public void CreateFox()
    {
        // Create a fox at the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();
        fox = CreateObject("Fox", "Fox", userFeetPosition, new Vector3D(0, 0, 0));

        // Convert the fox Object3D to a GameObject and add a NavMeshAgent component
        GameObject foxGameObject = fox.ToGameObject();
        foxAgent = foxGameObject.AddComponent<NavMeshAgent>();
    }

    public void CreateZombie()
    {
        // Create a zombie at the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();
        zombie = CreateObject("Zombie", "Zombie", userFeetPosition, new Vector3D(0, 0, 0));

        // Convert the zombie Object3D to a GameObject and add a NavMeshAgent component
        GameObject zombieGameObject = zombie.ToGameObject();
        zombieAgent = zombieGameObject.AddComponent<NavMeshAgent>();
    }

    public void UpdateFoxPosition()
    {
        // Update the fox's destination to the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();
        foxAgent.SetDestination(userFeetPosition.ToVector3());
    }

    public void UpdateZombiePosition()
    {
        // Update the zombie's destination to the fox's position
        Vector3D foxPosition = fox.GetPosition();
        zombieAgent.SetDestination(foxPosition.ToVector3());
    }
}