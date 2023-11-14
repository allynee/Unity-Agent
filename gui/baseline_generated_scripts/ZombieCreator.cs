using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class ZombieCreator : SceneAPI
{
    private void Start()
    {
        CreateZombie();
    }

    public void CreateZombie()
    {
        // Check if the object type "Zombie" is valid
        if (!IsObjectTypeValid("Zombie"))
        {
            Debug.Log("Invalid object type: Zombie");
            return;
        }

        // Get the user's feet position as the base position
        Vector3D basePosition = GetUsersFeetPosition();

        // Create a position in front of the user for the zombie
        Vector3D zombiePosition = new Vector3D(basePosition.x, basePosition.y, basePosition.z + 2);

        // Get the user's orientation
        Vector3D userOrientation = GetUserOrientation();

        // Create a rotation for the zombie to face the user
        Vector3D zombieRotation = new Vector3D(0, userOrientation.y + 180, 0);

        // Create the zombie object
        Object3D zombie = CreateObject("Zombie1", "Zombie", zombiePosition, zombieRotation);

        if (zombie == null)
        {
            Debug.Log("Failed to create Zombie");
        }
        else
        {
            Debug.Log("Zombie created successfully");
        }
    }
}