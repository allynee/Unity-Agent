using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class FoxAndZombie : SceneAPI
{
    // Class members to track the Fox and Zombie objects
    private Object3D foxObject;
    private Object3D zombieObject;

    private void Start()
    {
        CreateFox();
        CreateZombie();
    }

    private void Update()
    {
        FollowUserAtDistance();
        ZombieFollowFox();
    }

    public void CreateFox()
    {
        // Check if the object type is valid
        if (IsObjectTypeValid("Fox"))
        {
            // Get the user's feet position to create the object
            Vector3D userFeetPos = GetUsersFeetPosition();

            // Create a new Fox object at the user's feet position
            foxObject = CreateObject("NewFox", "Fox", userFeetPos, new Vector3D(0, 0, 0));

            // Log the creation of the Fox object
            Debug.Log("New Fox object created at position: " + userFeetPos.x + ", " + userFeetPos.y + ", " + userFeetPos.z);
        }
        else
        {
            // Log an error if the object type is not valid
            Debug.LogError("Invalid object type: Fox");
        }
    }

    public void CreateZombie()
    {
        // Check if the object type is valid
        if (IsObjectTypeValid("Zombie"))
        {
            // Create a new Zombie object at a specific position
            Vector3D zombiePosition = new Vector3D(0f, 0f, 0f);
            zombieObject = CreateObject("Zombie", "Zombie", zombiePosition, new Vector3D(0f, 0f, 0f));

            // Log the creation of the Zombie object
            Debug.Log("New Zombie object created at position: " + zombiePosition.x + ", " + zombiePosition.y + ", " + zombiePosition.z);
        }
        else
        {
            // Log an error if the object type is not valid
            Debug.LogError("Invalid object type: Zombie");
        }
    }

    public void FollowUserAtDistance()
    {
        // Get the user's position
        Vector3D userPosition = GetUsersFeetPosition();

        // Calculate the direction from the fox to the user
        Vector3D direction = userPosition - foxObject.GetPosition();

        // If the user is more than 1 meter away, move the fox towards the user
        if (direction.ToVector3().magnitude > 1f)
        {
            // Calculate the new position for the fox
            Vector3D newPosition = userPosition - direction.ToVector3().normalized * 1f;

            // Set the new position for the fox
            foxObject.SetPosition(newPosition);
        }
    }

    public void ZombieFollowFox()
    {
        // Check if the Fox and Zombie objects are not null
        if (foxObject != null && zombieObject != null)
        {
            // Get the positions of the Fox and Zombie objects
            Vector3D foxPosition = foxObject.GetPosition();
            Vector3D zombiePosition = zombieObject.GetPosition();

            // Calculate the direction from the Zombie to the Fox
            Vector3D direction = new Vector3D(foxPosition.x - zombiePosition.x, 0, foxPosition.z - zombiePosition.z);
            direction = direction.ToVector3().normalized;

            // Calculate the position for the Zombie to follow the Fox at a distance of 1 meter
            Vector3D followPosition = new Vector3D(foxPosition.x - direction.x, foxPosition.y, foxPosition.z - direction.z);

            // Set the position for the Zombie to follow the Fox
            zombieObject.SetPosition(followPosition);
        }
        else
        {
            // Log an error if the Fox or Zombie object is not found
            Debug.LogError("Fox or Zombie object not found");
        }
    }
}