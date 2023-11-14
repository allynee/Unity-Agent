using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class ZombieGuard : SceneAPI
{
    // Class member to hold the Zombie object
    private Object3D zombieObject;

    private void Start()
    {
        CreateZombieInFieldOfView();
        ResizeZombie();
        EditZombiePosition();
        EditZombieRotationToFaceUser();
    }

    private void Update()
    {
        // No repeated method needed for this task
    }

    public void CreateZombieInFieldOfView()
    {
        // Check if Zombie is a valid object type
        if (!IsZombieValid())
        {
            Debug.Log("Zombie is not a valid object type.");
            return;
        }

        // Get the user's feet position to create the Zombie in the field of view
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create the Zombie in the user's field of view
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

    public void ResizeZombie()
    {
        // Check if the Zombie object is found
        if (zombieObject != null)
        {
            // Get the current size of the Zombie
            Vector3D currentSize = zombieObject.GetSize();

            // Calculate the new size (0.5 times the current size)
            Vector3D newSize = new Vector3D(currentSize.x * 0.5f, currentSize.y * 0.5f, currentSize.z * 0.5f);

            // Set the new size for the Zombie
            zombieObject.SetSize(newSize);
        }
        else
        {
            Debug.Log("Zombie object not found in the scene.");
        }
    }

    public void EditZombiePosition()
    {
        // Check if the Zombie object is found
        if (zombieObject != null)
        {
            // Get the user's position
            Vector3D userPosition = GetUsersFeetPosition();

            // Calculate the position 1m away from the user
            Vector3D newPosition = CalculatePositionAwayFromUser(userPosition);

            // Set the new position for the Zombie
            zombieObject.SetPosition(newPosition);
        }
        else
        {
            Debug.Log("Zombie object not found");
        }
    }

    private Vector3D CalculatePositionAwayFromUser(Vector3D userPosition)
    {
        // Calculate the position 1m away from the user
        Vector3D direction = new Vector3D(0, 0, 1); // 1m in front of the user
        Vector3D newPosition = new Vector3D(userPosition.x + direction.x, userPosition.y + direction.y, userPosition.z + direction.z);
        return newPosition;
    }

    public void EditZombieRotationToFaceUser()
    {
        // Check if the Zombie object is found
        if (zombieObject != null)
        {
            // Get the user's position
            Vector3D userPosition = GetUsersFeetPosition();

            // Calculate the direction from the Zombie to the user
            Vector3D directionToUser = new Vector3D(userPosition.x - zombieObject.GetPosition().x,
                                                    userPosition.y - zombieObject.GetPosition().y,
                                                    userPosition.z - zombieObject.GetPosition().z);

            // Calculate the rotation to face the user
            Vector3D newRotation = CalculateRotationToFaceUser(directionToUser);

            // Set the new rotation for the Zombie object
            zombieObject.SetRotation(newRotation);
        }
        else
        {
            Debug.Log("Zombie object not found in the scene.");
        }
    }

    private Vector3D CalculateRotationToFaceUser(Vector3D direction)
    {
        // Calculate the rotation to face the user
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        return new Vector3D(0f, angle, 0f);
    }
}