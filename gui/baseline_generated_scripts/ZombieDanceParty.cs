using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class ZombieDanceParty : SceneAPI
{
    private List<Object3D> zombies = new List<Object3D>();
    private Vector3D danceFloorCenter;
    private float danceRadius = 5.0f;
    private float danceSpeed = 1.0f;

    private void Start()
    {
        CreateZombies();
        SetDanceFloorCenter();
        StartDanceParty();
    }

    private void Update()
    {
        MakeZombiesDance();
    }

    private void CreateZombies()
    {
        // Create 10 zombies and add them to the list
        for (int i = 0; i < 10; i++)
        {
            string zombieName = "Zombie" + i;
            Vector3D position = new Vector3D(i * 2, 0, i * 2);
            Vector3D rotation = new Vector3D(0, 0, 0);
            Object3D zombie = CreateObject(zombieName, "Zombie", position, rotation);
            zombies.Add(zombie);
        }
    }

    private void SetDanceFloorCenter()
    {
        // Set the dance floor center to the center of the scene
        Vector3D sceneSize = GetSceneSize();
        danceFloorCenter = new Vector3D(sceneSize.x / 2, 0, sceneSize.z / 2);
    }

    private void StartDanceParty()
    {
        // Move all zombies to the dance floor
        foreach (Object3D zombie in zombies)
        {
            Vector3D position = new Vector3D(danceFloorCenter.x, 0, danceFloorCenter.z);
            zombie.SetPosition(position);
        }
    }

    private void MakeZombiesDance()
    {
        // Make all zombies dance around the dance floor center
        foreach (Object3D zombie in zombies)
        {
            Vector3D position = zombie.GetPosition();
            float angle = Mathf.Atan2(position.z - danceFloorCenter.z, position.x - danceFloorCenter.x);
            angle += danceSpeed * Time.deltaTime;
            position.x = danceFloorCenter.x + danceRadius * Mathf.Cos(angle);
            position.z = danceFloorCenter.z + danceRadius * Mathf.Sin(angle);
            zombie.SetPosition(position);
        }
    }
}