using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CubeScene : SceneAPI
{
    private Object3D userTable;
    private Object3D ledCube;

    private void Start()
    {
        FindTableInFieldOfView();
        CreateLEDCube();
        EditLedCubePosition();
        EditLEDCubeIllumination();
        ChangeLedCubeColorToPink();
        LevitateLedCubeAboveTable();
    }

    private void FindTableInFieldOfView()
    {
        // Get all objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Find the table in the user's field of view
        userTable = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        if (userTable != null)
        {
            Debug.Log("Table found in the user's field of view!");
        }
        else
        {
            Debug.LogWarning("No table found in the user's field of view.");
        }
    }

    public void CreateLEDCube()
    {
        // Default position for the LED Cube
        Vector3D defaultPosition = new Vector3D(0, 0, 0);

        // Create the LED Cube object
        ledCube = CreateObject("LEDCube", "LED Cube", defaultPosition, new Vector3D(0, 0, 0));
    }

    public void EditLedCubePosition()
    {
        if (ledCube != null && userTable != null)
        {
            // Get the position of the table
            Vector3D tablePosition = userTable.GetPosition();

            // Calculate the position 1 meter above the table and centered on top of it
            Vector3D newLedCubePosition = new Vector3D(tablePosition.x, tablePosition.y + 1, tablePosition.z);

            // Set the new position for the LED Cube
            ledCube.SetPosition(newLedCubePosition);
        }
        else
        {
            Debug.Log("LED Cube or Table not found in the scene");
        }
    }

    private void EditLEDCubeIllumination()
    {
        if (ledCube == null)
        {
            Debug.LogError("LED Cube not found in the scene.");
            return;
        }

        // Set the illumination state to true
        ledCube.Illuminate(true);
    }

    private void ChangeLedCubeColorToPink()
    {
        if (ledCube != null)
        {
            // Set the color of the LED Cube to pink
            ledCube.SetColor(new Color3D(1, 0.5f, 0.5f, 1)); // RGBA for pink color
            Debug.Log("LED Cube color changed to pink.");
        }
        else
        {
            Debug.LogError("LED Cube not found in the scene.");
        }
    }

    private void LevitateLedCubeAboveTable()
    {
        if (ledCube != null)
        {
            // Set the levitation property of the LED Cube to true
            ledCube.Levitate(true);

            // Get the current position of the LED Cube
            Vector3D cubePosition = ledCube.GetPosition();

            // Set the position of the LED Cube to float above the table
            ledCube.SetPosition(new Vector3D(cubePosition.x, cubePosition.y + 0.5f, cubePosition.z));
            // Assuming the table's height is 0.5 units
        }
        else
        {
            Debug.Log("LED Cube not found in the scene");
        }
    }
}