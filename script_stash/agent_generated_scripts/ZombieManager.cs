using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ZombieManager : SceneAPI
{       
    private List<Object3D> zombies = new List<Object3D>();

    private void Start()
    {
        FindOrCreateZombie();
        PositionZombiesInCircle();
    }

    private void Update()
    {
        RotateZombieToRandomDirection();
    }

    public void FindOrCreateZombie()
    {
        List<Object3D> objectsInView = GetAllObject3DsInScene();
        zombieObject3D = objectsInView.Find(obj => obj.GetType().Equals("Zombie"));

        if (zombieObject3D == null)
        {
            Debug.Log("Zombie not found in user's field of view.");
            zombieObject3D = FindObject3DByName("Zombie");
            Debug.Log("Zombie found in the scene.");
        }

        if (zombieObject3D == null)
        {
            Debug.Log("Zombie not found in the scene.");
            Vector3D positionToCreateZombie = GetUsersFeetPosition();
            for (int i = 0; i < 10; i++)
            {
                CreateObject($"Zombie_{i}", "Zombie", positionToCreateZombie, new Vector3D(0, 0, 0));
            }
            Debug.Log("Ten zombies created in the scene.");
        }
    }

    public void PositionZombiesInCircle()
    {
        Vector3D center = new Vector3D(0, 0, 0);
        float radius = 2f;
        float angleStep = 360f / zombies.Count;

        for (int i = 0; i < zombies.Count; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            float x = center.x + radius * Mathf.Cos(angle);
            float z = center.z + radius * Mathf.Sin(angle);
            Vector3D zombiePosition = new Vector3D(x, 0, z);
            zombies[i].SetPosition(zombiePosition);
        }
    }

    private void RotateZombieToRandomDirection()
    {
        if (zombieObject3D != null)
        {
            float randomX = UnityEngine.Random.Range(0f, 360f);
            float randomY = UnityEngine.Random.Range(0f, 360f);
            float randomZ = UnityEngine.Random.Range(0f, 360f);
            Vector3D randomRotation = new Vector3D(randomX, randomY, randomZ);
            zombieObject3D.SetRotation(randomRotation);
        }
        else
        {
            zombieObject3D = FindObject3DByName("Zombie");
        }
    }
}