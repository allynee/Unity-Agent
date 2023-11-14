using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class CircleOfLights : SceneAPI
{
    private const int NumberOfLights = 10; // Number of lights in the circle
    private const float Radius = 1.0f; // Radius of the circle
    private const float Intensity = 1.0f; // Intensity of the lights

    private void Start()
    {
        CreateCircleOfLights();
    }

    private void CreateCircleOfLights()
    {
        // Get the position of the back wall
        Vector3D wallPosition = GetWallPosition(WallName.BackRight);

        // Calculate the center of the circle
        Vector3D center = new Vector3D(wallPosition.x, wallPosition.y, wallPosition.z);

        // Create the lights in a circle
        for (int i = 0; i < NumberOfLights; i++)
        {
            // Calculate the position of the light
            float angle = i * Mathf.PI * 2 / NumberOfLights;
            Vector3D position = new Vector3D(center.x + Mathf.Cos(angle) * Radius, center.y, center.z + Mathf.Sin(angle) * Radius);

            // Create the light
            Object3D light = CreateObject("Light" + i, "LED Cube", position, new Vector3D(0, 0, 0));

            // Set the color and intensity of the light
            light.SetColor(new Color3D(1, 1, 1, 1));
            light.SetLuminousIntensity(Intensity);
            light.Illuminate(true);

            // Log the creation of the light
            Debug.Log("Created light at position " + position.x + ", " + position.y + ", " + position.z);
        }
    }
}