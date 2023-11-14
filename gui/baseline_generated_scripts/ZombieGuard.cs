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
    private Object3D zombie;

    private void Start()
    {
        CreateZombie();
        PositionZombie();
    }

    private void Update()
    {
        GuardUser();
    }

    public void CreateZombie()
    {
        // Check if "Zombie" is a valid object type
        if (!IsObjectTypeValid("Zombie"))
        {
            Debug.Log("Zombie is not a valid object type.");
            return;
        }

        // Create a new zombie object at the user's feet position
        zombie = CreateObject("GuardZombie", "Zombie", GetUsersFeetPosition(), new Vector3D(0, 0, 0));

        // Check if the zombie was created successfully
        if (zombie == null)
        {
            Debug.Log("Failed to create zombie.");
            return;
        }

        // Make the zombie stumpy by scaling its size
        zombie.SetSizeByScale(0.5f);
    }

    public void PositionZombie()
    {
        // Get the user's orientation
        Vector3D userOrientation = GetUserOrientation();

        // Calculate the position in front of the user
        Vector3D frontPosition = new Vector3D(
            GetUsersFeetPosition().x + userOrientation.x,
            GetUsersFeetPosition().y,
            GetUsersFeetPosition().z + userOrientation.z
        );

        // Set the zombie's position to the calculated position
        zombie.SetPosition(frontPosition);
    }

    public void GuardUser()
    {
        // Continuously update the zombie's position to stay in front of the user
        PositionZombie();
    }
}