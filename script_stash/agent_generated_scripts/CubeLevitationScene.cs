using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CubeLevitationScene : SceneAPI
{       
    // Class-level variables to maintain state across methods
    private Object3D userTable;
    private Object3D ledCube;

    private void Start()
    {
        FindTableInFieldOfView();
        CreateLEDCube();
        LevitateLedCubeAboveTable();
    }

    // Method to find the table in the user's field of view
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

    // Method to create the LED Cube object
    public void CreateLEDCube()
    {
        // Default position for the LED Cube
        Vector3D defaultPosition = new Vector3D(0, 0, 0);
        
        // Create the LED Cube object
        ledCube = CreateObject("LEDCube", "LED Cube", defaultPosition, new Vector3D(0, 0, 0));
    }

    // Method to levitate the LED Cube above the table
    private void LevitateLedCubeAboveTable()
    {
        if (ledCube != null)
        {
            // Set the levitation property of the LED Cube to true
            ledCube.Levitate(true);

            // Get the position of the table
            Vector3D tablePosition = userTable.GetPosition();

            // Set the position of the LED Cube above the table
            Vector3D cubePosition = new Vector3D(tablePosition.x, tablePosition.y + ledCube.GetSize().y / 2, tablePosition.z);
            ledCube.SetPosition(cubePosition);
        }
        else
        {
            Debug.Log("LED Cube not found in the scene");
        }
    }
}