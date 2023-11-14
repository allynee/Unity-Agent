using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CubeManager : SceneAPI
{
    // Declare the LED Cube object
    private Object3D ledCube;

    private void Start()
    {
        CreateLEDCube();
        PositionLEDCubeAboveTable();
        MakeLedCubeLevitate();
        LiftLedCubeUp();
        ChangeLEDCubeColorToBlue();
    }

    public void CreateLEDCube()
    {
        // Get the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create the LED Cube at the user's feet position
        ledCube = CreateObject("UserLEDCube", "LED Cube", userFeetPosition, new Vector3D(0, 0, 0));

        // Log the creation of the LED Cube
        Debug.Log("LED Cube created at user's feet position");
    }

    public void PositionLEDCubeAboveTable()
    {
        // Get all objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Find the first table in the user's field of view
        Object3D table = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        // Check if the LED Cube exists before trying to move it
        if (ledCube == null) 
        {
            Debug.LogWarning("No LED Cube found.");
            return;
        }

        if (table != null)
        {
            Debug.Log("Found a table to put the LED Cube above!");

            // Get the position and size of the table
            Vector3D tablePosition = table.GetPosition();
            Vector3D tableSize = table.GetSize();
            float tableHeight = tableSize.y;

            // Get the size of the LED Cube
            Vector3D ledCubeSize = ledCube.GetSize();
            float ledCubeHeight = ledCubeSize.y;

            // Calculate the new position for the LED Cube to be directly above the center of the table
            Vector3D ledCubePosition = new Vector3D(tablePosition.x, tablePosition.y + tableHeight + ledCubeHeight / 2, tablePosition.z); 

            Debug.Log("LED Cube position: " + ledCubePosition.x + ", " + ledCubePosition.y + ", " + ledCubePosition.z);

            // Set the new position for the LED Cube
            ledCube.SetPosition(ledCubePosition);
        }
        else
        {
            Debug.LogWarning("No table found in the user's field of view.");
        }
    }

    public void MakeLedCubeLevitate()
    {
        // Check if the LED Cube is already levitating
        if (ledCube.IsLevitated())
        {
            Debug.Log("LED Cube is already levitating.");
            return;
        }

        // Set the Levitate property of the LED Cube to true
        ledCube.Levitate(true);

        Debug.Log("LED Cube is now levitating.");
    }

    public void LiftLedCubeUp()
    {
        // Get the current position of the LED Cube
        Vector3D cubePosition = ledCube.GetPosition();

        // Set the new position to be 1 meter above the current position
        Vector3D newPosition = new Vector3D(cubePosition.x, cubePosition.y + 1.0f, cubePosition.z);

        // Set the LED Cube to levitate
        ledCube.Levitate(true);

        // Update the position of the LED Cube
        ledCube.SetPosition(newPosition);

        Debug.Log("LED Cube has been lifted up by 1 meter.");
    }

    public void ChangeLEDCubeColorToBlue()
    {
        // Check if the LED Cube object exists
        if (ledCube == null)
        {
            Debug.Log("LED Cube object not found.");
            return;
        }

        // Set the color of the LED Cube to blue
        ledCube.SetColor(new Color3D(0, 0, 1, 1)); // RGB for blue color

        Debug.Log("LED Cube color changed to blue.");
    }
}