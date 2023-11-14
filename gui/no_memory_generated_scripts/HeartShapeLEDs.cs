using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class HeartShapeLEDs : SceneAPI
{
    // Class member to store all LED Cubes in the scene
    private List<Object3D> ledCubes = new List<Object3D>();

    private void Start()
    {
        FindAndStoreAllLEDCubes();
        SetLEDColor();
        IlluminateLEDs();
        SetLEDIntensity();
        CreateLEDInFieldOfView();
        FormHeartShapeWithLEDs();
    }

    private void FindAndStoreAllLEDCubes()
    {
        // Find and store all LED Cubes in the scene
        List<Object3D> allObjects = GetAllObject3DsInScene();
        foreach (Object3D obj in allObjects)
        {
            if (obj.GetType() == "LED Cube")
            {
                ledCubes.Add(obj);
            }
        }
    }

    public void SetLEDColor()
    {
        // Set the color of each LED Cube to RGBA(255, 0, 0, 1)
        foreach (Object3D ledCube in ledCubes)
        {
            ledCube.SetColor(new Color3D(1f, 0f, 0f, 1f));
        }
    }

    public void IlluminateLEDs()
    {
        // Edit the Illumination property of each LED Cube to True
        foreach (Object3D ledCube in ledCubes)
        {
            ledCube.Illuminate(true);
        }
    }

    public void SetLEDIntensity()
    {
        // Edit the Luminous Intensity property of each LED Cube to 10
        foreach (Object3D cube in ledCubes)
        {
            cube.SetLuminousIntensity(10f);
        }
    }

    public void CreateLEDInFieldOfView()
    {
        // Create 50 LED Cubes in the user's field of view
        for (int i = 0; i < 50; i++)
        {
            // Generate random position within the user's field of view
            Vector3D randomPosition = GenerateRandomPositionInFieldOfView();

            // Create LED Cube at the random position
            Object3D newLED = CreateObject("NewLED" + ledCubes.Count, "LED Cube", randomPosition, new Vector3D(0, 0, 0));
            ledCubes.Add(newLED);
        }
    }

    private Vector3D GenerateRandomPositionInFieldOfView()
    {
        // Generate random position within the user's field of view
        Vector3D usersFeetPosition = GetUsersFeetPosition();
        Vector3D randomPosition = new Vector3D(Random.Range(usersFeetPosition.x - 2, usersFeetPosition.x + 2),
                                               usersFeetPosition.y,
                                               Random.Range(usersFeetPosition.z - 2, usersFeetPosition.z + 2));
        return randomPosition;
    }

    public void FormHeartShapeWithLEDs()
    {
        // Get the wall position to form the heart shape
        Vector3D wallPosition = GetWallPositionForHeartShape();

        // Calculate the positions for the LED Cubes to form a heart shape
        List<Vector3D> heartShapePositions = CalculateHeartShapePositions(wallPosition);

        // Edit the Position property of each LED Cube to form a heart shape
        for (int i = 0; i < ledCubes.Count; i++)
        {
            if (i < heartShapePositions.Count)
            {
                ledCubes[i].SetPosition(heartShapePositions[i]);
            }
            else
            {
                Debug.Log("Not enough positions to form the complete heart shape");
                break;
            }
        }
    }

    private Vector3D GetWallPositionForHeartShape()
    {
        // Assuming the heart shape is to be formed on the "BackLeft" wall
        return GetWallPosition(WallName.BackLeft);
    }

    private List<Vector3D> CalculateHeartShapePositions(Vector3D wallPosition)
    {
        // Calculate the positions for the LED Cubes to form a heart shape
        // This can be a pre-defined set of positions or calculated based on a specific algorithm
        // For simplicity, let's assume a pre-defined set of positions
        List<Vector3D> heartShapePositions = new List<Vector3D>
        {
            new Vector3D(wallPosition.x, wallPosition.y, wallPosition.z + 1),
            new Vector3D(wallPosition.x - 1, wallPosition.y, wallPosition.z),
            new Vector3D(wallPosition.x + 1, wallPosition.y, wallPosition.z),
            new Vector3D(wallPosition.x - 1, wallPosition.y, wallPosition.z - 1),
            new Vector3D(wallPosition.x + 1, wallPosition.y, wallPosition.z - 1),
            new Vector3D(wallPosition.x, wallPosition.y, wallPosition.z - 2)
        };

        return heartShapePositions;
    }
}