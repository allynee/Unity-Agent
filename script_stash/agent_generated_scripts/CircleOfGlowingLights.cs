using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CircleOfGlowingLights : SceneAPI
{       
    // Class-level variable to store LED cubes
    private List<Object3D> ledCubes = new List<Object3D>();

    // Method to create ten LED cubes
    public void CreateTenLEDCubes()
    {
        for (int i = 0; i < 10; i++)
        {
            // Create LED Cube at a default position
            CreateObject($"LEDCube_{i}", "LED Cube", new Vector3D(0, 0, 0), new Vector3D(0, 0, 0));
            ledCubes.Add(GetObject($"LEDCube_{i}"));  // Add the created LED cube to the list
        }
    }

    // Method to position the LED cubes in a circle on the wall
    public void PositionLedCubesInCircle()
    {
        // Get the user's head position and orientation
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D userOrientation = GetUserOrientation();

        // Get the wall position in front of the user
        Vector3D frontWallPosition = GetWallPosition(WallName.BackLeft);

        // Calculate the radius of the circle based on the number of LED cubes
        float circleRadius = 1.5f * ledCubes.Count;

        // Calculate the angle between each LED cube
        float angleStep = 360f / ledCubes.Count;

        // Position the LED cubes in a circle on the front wall
        for (int i = 0; i < ledCubes.Count; i++)
        {
            float angle = i * angleStep;
            float x = frontWallPosition.x + circleRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = frontWallPosition.z + circleRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3D cubePosition = new Vector3D(x, userHeadPosition.y, z);
            ledCubes[i].SetPosition(cubePosition);
        }
    }

    // Method to edit the illumination property of the LED cubes
    public void EditLEDsIllumination()
    {
        // Check if there are LED cubes available
        if (ledCubes == null || ledCubes.Count == 0)
        {
            Debug.LogError("No LED cubes found in the scene.");
            return;
        }

        // Set the illumination property of all LED cubes to true
        foreach (Object3D ledCube in ledCubes)
        {
            ledCube.Illuminate(true);
        }

        Debug.Log("LED cubes illumination edited successfully.");
    }
}