using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class LEDCubeController : SceneAPI
{
    // Declare the LED Cube object
    private Object3D ledCube;
    private float luminousIntensity = 1.0f;
    private float changeInterval = 1.0f;
    private float timeSinceLastChange = 0.0f;

    private void Start()
    {
        CreateLEDCube();
        PositionLEDCubeAboveTable();
        MakeLedCubeLevitate();
        LiftLedCubeUp();
        ChangeLedCubeColorToPink();
        IlluminateLedCube();
    }

    private void Update()
    {
        RotateLedCube();
        ChangeLuminousIntensity();
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

    public void ChangeLedCubeColorToPink()
    {
        // Set the color of the LED Cube to pink
        ledCube.SetColor(new Color3D(1, 0, 0.5f, 1)); // RGBA for pink color
    }

    public void RotateLedCube()
    {
        // Get the current rotation of the LED Cube
        Vector3D currentRotation = ledCube.GetRotation();

        // Calculate the new rotation around the Y-axis
        float newYRotation = currentRotation.y + 45 * Time.deltaTime;

        // Set the new rotation of the LED Cube
        ledCube.SetRotation(new Vector3D(currentRotation.x, newYRotation, currentRotation.z));
    }

    public void IlluminateLedCube()
    {
        // Set the Illumination property of the LED Cube to true
        ledCube.Illuminate(true);

        Debug.Log("LED Cube is now illuminated.");
    }

    public void ChangeLuminousIntensity()
    {
        // Increase the time since the last change by the time passed since the last frame
        timeSinceLastChange += Time.deltaTime;

        // Check if it's time to change the luminous intensity
        if (timeSinceLastChange >= changeInterval)
        {
            // Reset the time since the last change
            timeSinceLastChange = 0;

            // Change the luminous intensity of the LED Cube
            if (luminousIntensity == 1.0f)
            {
                luminousIntensity = 10.0f;
            }
            else
            {
                luminousIntensity = 1.0f;
            }

            // Set the new luminous intensity of the LED Cube
            ledCube.SetLuminousIntensity(luminousIntensity);
        }
    }
}