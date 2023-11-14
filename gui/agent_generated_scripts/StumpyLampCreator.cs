using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class StumpyLampCreator : SceneAPI
{
    // Declare the lamp object
    private Object3D lamp;

    private void Start()
    {
        CreateLamp();
        PositionLampInFrontOfUser();
        ResizeLamp();
        EnlargeLamp();
    }

    public void CreateLamp()
    {
        // Get the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create the lamp at the user's feet position
        lamp = CreateObject("UserLamp", "Lamp", userFeetPosition, new Vector3D(0, 0, 0));

        // Log the creation of the lamp
        Debug.Log("Lamp created at user's feet position");
    }

    public void PositionLampInFrontOfUser()
    {
        // Get the user's feet position and orientation
        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();

        // Assume a default 0.5m in front
        float defaultDistance = 0.5f;

        // Calculate the new position for the lamp based on the user's orientation and the default distance
        Vector3D newPosition = new Vector3D(
            userFeetPosition.x + userOrientation.x * defaultDistance,
            userFeetPosition.y,
            userFeetPosition.z + userOrientation.z * defaultDistance
        );

        // Set the position of the lamp
        lamp.SetPosition(newPosition);
    }

    public void ResizeLamp()
    {
        // Get the current size of the lamp
        Vector3D lampSize = lamp.GetSize();

        // Calculate the new size for the lamp on the Y axis
        float newYSize = lampSize.y * 0.8f;

        // Create a new Vector3D for the new size
        Vector3D newSize = new Vector3D(lampSize.x, newYSize, lampSize.z);

        // Set the new size for the lamp
        lamp.SetSize(newSize);
    }

    public void EnlargeLamp()
    {
        // Get the current size of the lamp
        Vector3D lampSize = lamp.GetSize();

        // Calculate the new size for the lamp on the X and Z axis
        Vector3D newLampSize = new Vector3D(lampSize.x * 2.5f, lampSize.y, lampSize.z * 2.5f);

        // Set the new size for the lamp
        lamp.SetSize(newLampSize);

        // Log the action
        Debug.Log("The size of the lamp has been updated.");
    }
}