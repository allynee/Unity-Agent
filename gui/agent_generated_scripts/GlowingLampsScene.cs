using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class GlowingLampsScene : SceneAPI
{       
    // Declare the lamp objects
    private Object3D lamp1;
    private Object3D lamp2;
    private Object3D lamp3;
    private Object3D lamp4;
    private Object3D lamp5;
    private Object3D lamp6;

    private void Start()
    {
        CreateLamps();
        PositionLampsInCircle();
        EditLampsIlluminationContinuously();
    }

    public void CreateLamps()
    {
        // Create the lamps if they are not already created
        if (lamp1 == null)
        {
            lamp1 = CreateObject("Lamp1", "Lamp", new Vector3D(1, 0, 1), new Vector3D(0, 0, 0));
            Debug.Log("Lamp 1 created");
        }
        // Create the second lamp
        if (lamp2 == null)
        {
            lamp2 = CreateObject("Lamp2", "Lamp", new Vector3D(2, 0, 2), new Vector3D(0, 0, 0));
            Debug.Log("Lamp 2 created");
        }
        // Create the third lamp
        if (lamp3 == null)
        {
            lamp3 = CreateObject("Lamp3", "Lamp", new Vector3D(3, 0, 3), new Vector3D(0, 0, 0));
            Debug.Log("Lamp 3 created");
        }
        // Create the fourth lamp
        if (lamp4 == null)
        {
            lamp4 = CreateObject("Lamp4", "Lamp", new Vector3D(4, 0, 4), new Vector3D(0, 0, 0));
            Debug.Log("Lamp 4 created");
        }
        // Create the fifth lamp
        if (lamp5 == null)
        {
            lamp5 = CreateObject("Lamp5", "Lamp", new Vector3D(5, 0, 5), new Vector3D(0, 0, 0));
            Debug.Log("Lamp 5 created");
        }
        // Create the sixth lamp
        if (lamp6 == null)
        {
            lamp6 = CreateObject("Lamp6", "Lamp", new Vector3D(6, 0, 6), new Vector3D(0, 0, 0));
            Debug.Log("Lamp 6 created");
        }
    }

    public void PositionLampsInCircle()
    {
        // Get the user's feet position and orientation
        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();

        // Assume a default 1m in front
        float defaultDistance = 1f;

        // Calculate the positions for the lamps to form a circle 1 meter in front of the user
        for (int i = 0; i < 6; i++)
        {
            float angle = (360f / 6) * i * Mathf.Deg2Rad; // Convert angle to radians

            // Calculate the position for the lamp based on the user's orientation and the default distance
            Vector3D newPosition = new Vector3D(
                userFeetPosition.x + userOrientation.x * defaultDistance * Mathf.Cos(angle),
                userFeetPosition.y,
                userFeetPosition.z + userOrientation.z * defaultDistance * Mathf.Sin(angle)
            );

            // Set the position of the lamp
            lamps[i].SetPosition(newPosition);
        }
    }

    public void EditLampsIlluminationContinuously()
    {
        if (lamp1 == null || lamp2 == null || lamp3 == null || lamp4 == null || lamp5 == null || lamp6 == null)
        {
            Debug.LogError("Not all lamps found in the scene.");
            return;
        }

        // Continuously edit the Illumination property of the array of lamps to be on
        lamp1.Illuminate(true);
        lamp2.Illuminate(true);
        lamp3.Illuminate(true);
        lamp4.Illuminate(true);
        lamp5.Illuminate(true);
        lamp6.Illuminate(true);
    }
}