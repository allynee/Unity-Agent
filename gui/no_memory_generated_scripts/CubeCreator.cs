using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class CubeCreator : SceneAPI
{
    // Class members
    private Object3D ledCube;
    private Object3D table;

    private void Start()
    {
        FindTableInFieldOfView();
        CreateLEDInCenterOfTable();
        EditLedCubeSize();
        SetLedCubeColor();
        LevitateLedCube();
        EditLEDPositionAboveTable();
    }

    private void Update()
    {
        // No repeating actions needed in Update for this task
    }

    public void FindTableInFieldOfView()
    {
        // Get all the objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Find the table in the user's field of view
        table = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        // Log if the table is found or not
        if (table != null)
        {
            Debug.Log("Table found in the user's field of view.");
        }
        else
        {
            Debug.Log("Table not found in the user's field of view.");
        }
    }

    public void CreateLEDInCenterOfTable()
    {
        if (table != null)
        {
            // Get the position of the table
            Vector3D tablePosition = table.GetPosition();

            // Calculate the position for the LED Cube in the center of the table
            Vector3D ledPosition = CalculateCenterOfTable(tablePosition);

            // Create the LED Cube at the calculated position
            ledCube = SceneAPI.CreateObject("LED Cube", "LED Cube", ledPosition, new Vector3D(0, 0, 0));
        }
        else
        {
            Debug.Log("Table not found in the scene.");
        }
    }

    private Vector3D CalculateCenterOfTable(Vector3D tablePosition)
    {
        // Assuming the table is a square, calculate the center position
        float centerX = tablePosition.x;
        float centerY = tablePosition.y + (table.GetSize().y / 2); // Assuming the anchor is at the bottom center
        float centerZ = tablePosition.z;

        return new Vector3D(centerX, centerY, centerZ);
    }

    public void EditLedCubeSize()
    {
        // Check if the LED Cube object is found
        if (ledCube != null)
        {
            // Set the size of the LED Cube to be 0.5 meters in each dimension
            Vector3D newSize = new Vector3D(0.5f, 0.5f, 0.5f);
            ledCube.SetSize(newSize);
            Debug.Log("LED Cube size edited successfully");
        }
        else
        {
            Debug.LogError("LED Cube not found");
        }
    }

    public void SetLedCubeColor()
    {
        // Check if the LED Cube object is found
        if (ledCube != null)
        {
            // Set the color of the LED Cube to cool blue (RGBA: 0, 0, 255, 1)
            Color3D coolBlue = new Color3D(0f, 0f, 1f, 1f);
            ledCube.SetColor(coolBlue);
            Debug.Log("LED Cube color set to cool blue.");
        }
        else
        {
            Debug.LogError("LED Cube not found in the scene.");
        }
    }

    public void LevitateLedCube()
    {
        // Check if the LED Cube object is found
        if (ledCube != null)
        {
            // Set the levitation property of the LED Cube to true
            ledCube.Levitate(true);
            Debug.Log("LED Cube is now levitating.");
        }
        else
        {
            Debug.Log("LED Cube not found in the scene.");
        }
    }

    public void EditLEDPositionAboveTable()
    {
        // Check if the LED Cube and Table objects are found
        if (ledCube != null && table != null)
        {
            // Get the position of the Table
            Vector3D tablePosition = table.GetPosition();

            // Set the position of the LED Cube 1 meter above the Table
            Vector3D newPosition = new Vector3D(tablePosition.x, tablePosition.y + 1f, tablePosition.z);
            ledCube.SetPosition(newPosition);
        }
        else
        {
            Debug.Log("LED Cube or Table not found");
        }
    }
}