using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class FloatingChair : SceneAPI
{
    private Object3D chair;

    private void Start()
    {
        CreateFloatingChair();
        OrientChairTowardsUser();
    }

    private void Update()
    {
        if (chair != null && !chair.IsLevitated())
        {
            chair.Levitate(true);
        }
    }

    public void CreateFloatingChair()
    {
        // Check if the object type "Chair" is valid
        if (!IsObjectTypeValid("Chair"))
        {
            Debug.Log("Invalid object type: Chair");
            return;
        }

        // Get the user's feet position as the base position for the chair
        Vector3D userPosition = GetUsersFeetPosition();

        // Create a new chair object at the user's position with default rotation
        chair = CreateObject("FloatingChair", "Chair", userPosition, new Vector3D(0, 0, 0));

        if (chair == null)
        {
            Debug.Log("Failed to create a chair");
            return;
        }

        // Make the chair levitate
        chair.Levitate(true);
    }

    public void OrientChairTowardsUser()
    {
        if (chair == null)
        {
            Debug.Log("No chair to orient");
            return;
        }

        // Get the user's orientation
        Vector3D userOrientation = GetUserOrientation();

        // Set the chair's rotation to face the user
        chair.SetRotation(userOrientation);
    }
}