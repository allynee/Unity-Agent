using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class LEDCircleCreator : SceneAPI
{
    // Class member to store the LED Cubes
    private List<Object3D> ledCubes = new List<Object3D>();

    private void Start()
    {
        CreateLEDInFieldOfView();
        ArrangeLedCubesInCircleOnWall();
        EditLEDCubeSize();
        SetLEDCubeColor();
        IlluminateLEDCubes();
        SetLEDCubeLuminousIntensity();
    }

    private void Update()
    {
        // No repetitive action needed for this task
    }

    public void CreateLEDInFieldOfView()
    {
        // Get all objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Create 10 LED Cubes in the user's field of view
        for (int i = 0; i < 10; i++)
        {
            // Create LED Cube at a random position within the user's field of view
            Vector3D randomPosition = GetRandomPositionInView(objectsInView);
            Object3D newLED = CreateObject("LED" + i, "LED Cube", randomPosition, new Vector3D(0, 0, 0));
            ledCubes.Add(newLED);
        }

        Debug.Log("10 LED Cubes created in the user's field of view.");
    }

    private Vector3D GetRandomPositionInView(List<Object3D> objectsInView)
    {
        // Get a random object in the user's field of view
        Object3D randomObject = objectsInView[Random.Range(0, objectsInView.Count)];

        // Get the position of the random object
        Vector3D randomObjectPosition = randomObject.GetPosition();

        // Add a random offset to the position of the random object
        float offsetX = Random.Range(-0.5f, 0.5f);
        float offsetY = Random.Range(-0.5f, 0.5f);
        float offsetZ = Random.Range(-0.5f, 0.5f);

        Vector3D randomPosition = new Vector3D(randomObjectPosition.x + offsetX, randomObjectPosition.y + offsetY, randomObjectPosition.z + offsetZ);

        return randomPosition;
    }

    public void ArrangeLedCubesInCircleOnWall()
    {
        // Get the wall position to form the circle
        Vector3D wallPosition = GetWallPositionForCircle();

        // Calculate the positions for the LED Cubes to form a circle
        List<Vector3D> circlePositions = CalculateCirclePositions(wallPosition, ledCubes.Count);

        // Set the positions of the LED Cubes to form a circle on the wall
        for (int i = 0; i < ledCubes.Count; i++)
        {
            ledCubes[i].SetPosition(circlePositions[i]);
        }
    }

    private Vector3D GetWallPositionForCircle()
    {
        // Assuming the circle will be formed on the "BackLeft" wall
        return GetWallPosition(WallName.BackLeft);
    }

    private List<Vector3D> CalculateCirclePositions(Vector3D center, int count)
    {
        List<Vector3D> positions = new List<Vector3D>();
        float radius = 2f; // Assuming a radius of 2 units for the circle

        for (int i = 0; i < count; i++)
        {
            float angle = i * (2 * Mathf.PI / count); // Calculate the angle for each LED Cube
            float x = center.x + radius * Mathf.Cos(angle);
            float z = center.z + radius * Mathf.Sin(angle);
            positions.Add(new Vector3D(x, center.y, z));
        }

        return positions;
    }

    public void EditLEDCubeSize()
    {
        // Edit the Size property of each LED Cube to be 0.1 meters in diameter
        foreach (Object3D cube in ledCubes)
        {
            cube.SetSize(new Vector3D(0.1f, 0.1f, 0.1f));
        }
    }

    public void SetLEDCubeColor()
    {
        // Set the color of each LED Cube to RGBA(255, 255, 255, 1)
        foreach (Object3D ledCube in ledCubes)
        {
            ledCube.SetColor(new Color3D(1f, 1f, 1f, 1f));
        }
        Debug.Log("Color of all LED Cubes has been set to RGBA(255, 255, 255, 1)");
    }

    public void IlluminateLEDCubes()
    {
        // Set the illumination property of each LED Cube to true
        foreach (Object3D ledCube in ledCubes)
        {
            ledCube.Illuminate(true);
        }
    }

    public void SetLEDCubeLuminousIntensity()
    {
        // Edit the Luminous Intensity property of each LED Cube to be 10
        foreach (Object3D cube in ledCubes)
        {
            cube.SetLuminousIntensity(10f);
        }
    }
}