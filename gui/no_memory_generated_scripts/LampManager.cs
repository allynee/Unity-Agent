using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class LampManager : SceneAPI
{
    // Class member to track the created Lamp object
    private Object3D createdLamp;

    private void Start()
    {
        CreateLampInFieldOfView();
        EditLampSize();
        EditLampPosition();
    }

    private void Update()
    {
        // No method needs to be called repeatedly for this task
    }

    public void CreateLampInFieldOfView()
    {
        // Check if Lamp is a valid object type
        if (!IsObjectTypeValid("Lamp"))
        {
            Debug.Log("Lamp is not a valid object type.");
            return;
        }

        // Get all objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Check if Lamp is already in the user's field of view
        if (objectsInView.Exists(obj => obj.GetType().Equals("Lamp")))
        {
            Debug.Log("Lamp is already in the user's field of view.");
            return;
        }

        // Get the user's feet position to create the Lamp
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create the Lamp in the user's field of view
        createdLamp = CreateObject("Lamp", "Lamp", userFeetPosition, new Vector3D(0, 0, 0));

        if (createdLamp != null)
        {
            Debug.Log("Lamp created in the user's field of view.");
        }
        else
        {
            Debug.Log("Failed to create Lamp in the user's field of view.");
        }
    }

    public void EditLampSize()
    {
        if (createdLamp != null)
        {
            // Get the current size of the Lamp
            Vector3D currentSize = createdLamp.GetSize();

            // Edit the Size property of the Lamp to be 0.5 times its current size
            Vector3D newSize = new Vector3D(currentSize.x * 0.5f, currentSize.y * 0.5f, currentSize.z * 0.5f);
            createdLamp.SetSize(newSize);

            // Log the action
            Debug.Log("Lamp size edited successfully");
        }
        else
        {
            // Log an error if the Lamp object is not found
            Debug.LogError("Lamp object not found");
        }
    }

    public void EditLampPosition()
    {
        if (createdLamp != null)
        {
            // Get the user's position
            Vector3D userPosition = GetUsersFeetPosition();

            // Calculate the new position 1m away from the user
            Vector3D newPosition = CalculatePositionInFrontOfUser(userPosition);

            // Set the new position for the Lamp
            createdLamp.SetPosition(newPosition);
        }
        else
        {
            Debug.Log("Lamp object not found");
        }
    }

    private Vector3D CalculatePositionInFrontOfUser(Vector3D userPosition)
    {
        // Assuming the user is facing the positive z-axis
        // Calculate the position 1m in front of the user
        Vector3D newPosition = new Vector3D(userPosition.x, userPosition.y, userPosition.z + 1);
        return newPosition;
    }
}