using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ZombieDanceParty : SceneAPI
{       
    // Declare class-level variable to store the zombies
    private List<Object3D> zombies = new List<Object3D>();

    private void Start()
    {
        CreateTenZombies();
        PositionZombiesInCircle();
    }

    public void CreateTenZombies()
    {
        for (int i = 0; i < 10; i++)
        {
            // Create Zombie at a default position
            Object3D zombie = CreateObject($"UserZombie_{i}", "Zombie", new Vector3D(0, 0, 0), new Vector3D(0, 0, 0));
            zombies.Add(zombie);
        }
    }

    public void PositionZombiesInCircle()
    {
        // Get the position of the user's feet to form the center of the circle
        Vector3D centerPosition = GetUsersFeetPosition();

        // Calculate the angle step for distributing the zombies evenly in a circle
        float angleStep = 360f / zombies.Count;

        // Define the radius of the circle
        float circleRadius = 3f;

        // Position the zombies in a circle on the floor
        for (int i = 0; i < zombies.Count; i++)
        {
            // Calculate the angle for this zombie
            float angle = angleStep * i * Mathf.Deg2Rad; // Convert angle to radians

            // Calculate the position of the zombie in the circle
            float x = centerPosition.x + circleRadius * Mathf.Cos(angle);
            float z = centerPosition.z + circleRadius * Mathf.Sin(angle);
            Vector3D zombiePosition = new Vector3D(x, centerPosition.y, z);

            // Set the position of the zombie
            zombies[i].SetPosition(zombiePosition);
        }
    }

    private void Update()
    {
        LeanZombiesRhythmically();
    }

    public void LeanZombiesRhythmically()
    {
        foreach (Object3D zombie in zombies)
        {
            // Get the current rotation of the zombie
            Vector3D currentRotation = zombie.GetRotation();

            // Calculate the new rotation pitch within ±15 degrees
            float newPitch = Mathf.Sin(Time.time) * 15; // Using sine function for rhythmic motion

            // Set the new rotation of the zombie
            zombie.SetRotation(new Vector3D(currentRotation.x, currentRotation.y, currentRotation.z + newPitch));
        }
    }
}