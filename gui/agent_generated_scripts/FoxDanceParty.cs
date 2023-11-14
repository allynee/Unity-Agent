using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class FoxDanceParty : SceneAPI
{
    private List<Object3D> foxObjects = new List<Object3D>();

    private void Start()
    {
        CreateTenFoxObjects();
        PositionFoxesInCircle();
    }

    public void CreateTenFoxObjects()
    {
        for (int i = 0; i < 10; i++)
        {
            // Create a Fox object at a default position
            Object3D fox = CreateObject($"UserFox_{i}", "Fox", new Vector3D(0, 0, 0), new Vector3D(0, 0, 0));
            foxObjects.Add(fox);
        }
    }

    public void PositionFoxesInCircle()
    {
        // Get the position of the user's feet to form the center of the circle
        Vector3D centerPosition = GetUsersFeetPosition();

        // Calculate the angle step for distributing the foxes evenly in a circle
        float angleStep = 360f / foxObjects.Count;

        // Define the radius of the circle
        float circleRadius = 2f;

        // Position the foxes in a circle on the floor
        for (int i = 0; i < foxObjects.Count; i++)
        {
            // Calculate the angle for this fox
            float angle = angleStep * i * Mathf.Deg2Rad; // Convert angle to radians

            // Calculate the position of the fox in the circle
            float x = centerPosition.x + circleRadius * Mathf.Cos(angle);
            float z = centerPosition.z + circleRadius * Mathf.Sin(angle);
            Vector3D foxPosition = new Vector3D(x, centerPosition.y, z);

            // Set the position of the fox
            foxObjects[i].SetPosition(foxPosition);
        }
    }

    private void Update()
    {
        RotateFoxesAroundYAxis();
    }

    public void RotateFoxesAroundYAxis()
    {
        foreach (Object3D fox in foxObjects)
        {
            // Get the current rotation of the fox
            Vector3D currentRotation = fox.GetRotation();

            // Calculate the new rotation around the Y-axis
            float newYRotation = currentRotation.y + 150 * Time.deltaTime;

            // Set the new rotation of the fox
            fox.SetRotation(new Vector3D(currentRotation.x, newYRotation, currentRotation.z));
        }
    }
}