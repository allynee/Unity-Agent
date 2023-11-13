using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ZombieDanceParty : SceneAPI
{
    private List<Object3D> zombies = new List<Object3D>();

    private void Start()
    {
        CreateZombies();
        RandomlyPositionZombiesInDanceFloor();
    }

    private void CreateZombies()
    {
        for (int i = 0; i < 20; i++)
        {
            // Create Zombie at a default position
            Object3D zombie = CreateObject($"Zombie_{i}", "Zombie", new Vector3D(0, 0, 0), new Vector3D(0, 0, 0));
            zombies.Add(zombie);
        }
    }

    private void RandomlyPositionZombiesInDanceFloor()
    {
        Vector3D danceFloorSize = GetSceneSize();
        foreach (Object3D zombie in zombies)
        {
            float randomX = UnityEngine.Random.Range(-danceFloorSize.x / 2, danceFloorSize.x / 2);
            float randomZ = UnityEngine.Random.Range(-danceFloorSize.z / 2, danceFloorSize.z / 2);
            Vector3D randomPosition = new Vector3D(randomX, 0, randomZ);
            zombie.SetPosition(randomPosition);
        }
    }

    private void Update()
    {
        RotateZombiesToRandomDirection();
    }

    private void RotateZombiesToRandomDirection()
    {
        foreach (Object3D zombie in zombies)
        {
            // Generate random rotation angles
            float randomX = UnityEngine.Random.Range(0f, 360f);
            float randomY = UnityEngine.Random.Range(0f, 360f);
            float randomZ = UnityEngine.Random.Range(0f, 360f);

            // Set the rotation of the zombie to the random angles
            Vector3D randomRotation = new Vector3D(randomX, randomY, randomZ);
            zombie.SetRotation(randomRotation);
        }
    }
}