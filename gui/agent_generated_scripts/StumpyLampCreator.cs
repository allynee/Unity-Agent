using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class StumpyLampCreator : SceneAPI
{       
    private Object3D lamp; // Class-level variable to maintain the lamp object state

    private void Start()
    {
        CreateLamp();
        EditLampPositionInFrontOfUser();
        ResizeLamp();
    }

    // Create a new object of type "Lamp" at the user's feet
    private void CreateLamp()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        lamp = CreateObject("NewLamp", "Lamp", userFeetPosition, new Vector3D(0, 0, 0));
    }

    // Edit the Position property of the Lamp to be 0.2 meters in front of the user
    private void EditLampPositionInFrontOfUser()
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

    // Edit the Size property of the lamp to be shorter on the Y-axis and wider on the X and Z axes
    private void ResizeLamp()
    {
        lamp = FindObject3DByName("Lamp");
        if (lamp == null)
        {
            Debug.LogError("Lamp not found in the scene.");
            return;
        }

        Vector3D originalSize = lamp.GetSize();
        float newYSize = originalSize.y * 0.5f; // 0.5 times of its original size on the Y-axis
        float newXZSize = originalSize.x * 1.2f; // 1.2 times of its original size on the X and Z axes
        lamp.SetSize(new Vector3D(newXZSize, newYSize, newXZSize));
    }

    // Calculate the position in front of the user based on the given distance
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
}