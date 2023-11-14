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
    private Object3D zombieObject;

    private void Start()
    {
        CreateZombieInFieldOfView();
        EditZombiePositionRelativeToUser();
        RotateZombieToFaceUser();
    }

    public void CreateZombieInFieldOfView()
    {
        if (!IsZombieValid())
        {
            Debug.Log("Zombie is not a valid object type.");
            return;
        }

        Vector3D userFeetPosition = GetUsersFeetPosition();

        zombieObject = CreateObject("Zombie", "Zombie", userFeetPosition, new Vector3D(0, 0, 0));
        if (zombieObject != null)
        {
            Debug.Log("Zombie created in the user's field of view.");
        }
        else
        {
            Debug.Log("Failed to create Zombie in the user's field of view.");
        }
    }

    private bool IsZombieValid()
    {
        List<string> validObjectTypes = GetAllValidObjectTypes();
        return validObjectTypes.Contains("Zombie");
    }

    public void EditZombiePositionRelativeToUser()
    {
        if (zombieObject != null)
        {
            Vector3D userFeetPosition = GetUsersFeetPosition();
            Vector3D newPosition = CalculatePositionRelativeToUser(userFeetPosition);
            zombieObject.SetPosition(newPosition);
        }
        else
        {
            Debug.Log("Zombie object not found in the scene.");
        }
    }

    private Vector3D CalculatePositionRelativeToUser(Vector3D userFeetPosition)
    {
        Vector3D newPosition = new Vector3D(userFeetPosition.x, userFeetPosition.y, userFeetPosition.z + 1);
        return newPosition;
    }

    public void RotateZombieToFaceUser()
    {
        if (zombieObject != null)
        {
            Vector3D userPosition = GetUsersFeetPosition();
            Vector3D zombiePosition = zombieObject.GetPosition();
            Vector3D directionToUser = new Vector3D(userPosition.x - zombiePosition.x, 0f, userPosition.z - zombiePosition.z);
            Vector3D newRotation = new Vector3D(0f, Mathf.Atan2(directionToUser.x, directionToUser.z) * Mathf.Rad2Deg, 0f);
            zombieObject.SetRotation(newRotation);
        }
        else
        {
            Debug.LogError("Zombie object not found");
        }
    }
}