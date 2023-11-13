using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class LampController : SceneAPI
{       
    private Object3D lamp; // Class-level variable to maintain the lamp object state

    private void Start()
    {
        CreateLamp();
        EditLampPositionInFrontOfUser();
        ResizeLamp();
    }

    public void CreateLamp()
    {
        // Default spawn lamp at the user's feet
        Vector3D userFeetPosition = GetUsersFeetPosition();
        
        // Create the lamp
        lamp = CreateObject("NewLamp", "Lamp", userFeetPosition, new Vector3D(0, 0, 0));
    }

    private Vector3D CalculatePositionInFrontOfUser(float distance)
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();
        return new Vector3D(
            userFeetPosition.x + userOrientation.x * distance,
            userFeetPosition.y,
            userFeetPosition.z + userOrientation.z * distance
        );
    }

    public void EditLampPositionInFrontOfUser()
    {
        lamp = FindObject3DByName("Lamp");
        if (lamp == null)
        {
            Debug.LogError("Lamp not found in the scene.");
            return;
        }

        Vector3D newPosition = CalculatePositionInFrontOfUser(0.2f);
        lamp.SetPosition(newPosition);
    }

    public void ResizeLamp()
    {
        // Find the lamp object
        lamp = FindObject3DByName("Lamp");
        if (lamp == null)
        {
            Debug.LogError("Lamp not found in the scene.");
            return;
        }

        // Get the current size of the lamp
        Vector3D currentSize = lamp.GetSize();

        // Calculate the new size based on the given instructions
        float newScale = 0.8f; // 0.8 times of its original size
        float newYScale = 0.5f; // 0.5 times its current size on the Y-axis

        Vector3D newSize = new Vector3D(currentSize.x * newScale, currentSize.y * newYScale, currentSize.z * newScale);

        // Set the new size for the lamp
        lamp.SetSize(newSize);
    }
}