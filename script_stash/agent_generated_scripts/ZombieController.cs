using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ZombieController : SceneAPI
{       
    private Object3D zombieObject3D;

    private void Start()
    {
        CreateOrFindZombie();
    }

    private void Update()
    {
        if (zombieObject3D != null)
        {
            RotateZombieRandomly();
        }
        else
        {
            zombieObject3D = FindObject3DByName("Zombie");
        }
    }

    public void CreateOrFindZombie()
    {
        List<Object3D> objectsInView = GetAllObject3DsInScene();
        zombieObject3D = objectsInView.Find(obj => obj.GetType().Equals("Zombie"));

        if (zombieObject3D == null)
        {
            Debug.Log("Zombie not found in user's field of view.");
            zombieObject3D = FindObject3DByName("Zombie");
            if (zombieObject3D != null)
            {
                Debug.Log("Zombie found in the scene.");
            }
        }

        if (zombieObject3D == null)
        {
            Debug.Log("Zombie not found in the scene.");
            Vector3D positionToCreateZombie = GetUsersFeetPosition();
            zombieObject3D = CreateObject("UserZombie", "Zombie", positionToCreateZombie, new Vector3D(0, 0, 0));
            Debug.Log("Zombie created in the scene.");
        }
    }

    private void RotateZombieRandomly()
    {
        float randomX = UnityEngine.Random.Range(0f, 360f);
        float randomY = UnityEngine.Random.Range(0f, 360f);
        float randomZ = UnityEngine.Random.Range(0f, 360f);
        zombieObject3D.SetRotation(new Vector3D(randomX, randomY, randomZ));
    }
}