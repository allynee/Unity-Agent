using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class LampCreation : SceneAPI
{
    private Object3D lamp;

    private void Start()
    {
        CreateLamp();
    }

    private void Update()
    {
        // Any updates or checks can be done here
    }

    public void CreateLamp()
    {
        // Check if the object type "Lamp" is valid
        if (!IsObjectTypeValid("Lamp"))
        {
            Debug.Log("Invalid object type: Lamp");
            return;
        }

        // Get the user's feet position and create a new position in front of the user
        Vector3D userPosition = GetUsersFeetPosition();
        Vector3D lampPosition = new Vector3D(userPosition.x, userPosition.y, userPosition.z + 1); // 1 unit in front of the user

        // Create the lamp object
        lamp = CreateObject("StumpyLamp", "Lamp", lampPosition, new Vector3D(0, 0, 0)); // Default rotation (0, 0, 0)

        // Set the size of the lamp to make it "stumpy"
        lamp.SetSize(new Vector3D(1, 0.5f, 1)); // Half the height

        Debug.Log("Lamp created in front of the user");
    }
}