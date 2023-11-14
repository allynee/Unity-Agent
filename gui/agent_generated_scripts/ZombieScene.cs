using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ZombieScene : SceneAPI
{       
    private Object3D userZombie;

    private void Start()
    {
        CreateZombie();
        PositionZombieInFrontOfUser();
    }

    public void CreateZombie()
    {
        // Get the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create the zombie at the user's feet position
        userZombie = CreateObject("UserZombie", "Zombie", userFeetPosition, new Vector3D(0, 0, 0));

        // Log the creation of the zombie
        Debug.Log("Zombie created at user's feet position");
    }

    public void PositionZombieInFrontOfUser()
    {
        if (userZombie == null)
        {
            Debug.Log("Zombie object is null. Cannot position in front of the user.");
            return;
        }

        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();

        float defaultDistance = 1.0f;

        Vector3D newPosition = new Vector3D(
            userFeetPosition.x + userOrientation.x * defaultDistance,
            userFeetPosition.y,
            userFeetPosition.z + userOrientation.z * defaultDistance
        );

        userZombie.SetPosition(newPosition);
    }
}