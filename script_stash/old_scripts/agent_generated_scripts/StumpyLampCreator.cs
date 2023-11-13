using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class StumpyLampCreator : SceneAPI
{       
    private Object3D lamp;

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

    private void ResizeLamp()
    {
        if (lamp == null)
        {
            Debug.LogError("Lamp doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the current size of the lamp
        Vector3D currentSize = lamp.GetSize();

        // Calculate the new size based on the given instructions
        float newX = currentSize.x * 0.8f;
        float newY = currentSize.y * 0.6f;
        float newZ = currentSize.z * 0.8f;

        // Set the new size for the lamp
        lamp.SetSize(new Vector3D(newX, newY, newZ));
    }
}