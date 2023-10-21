using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;

public class BigBlueLamp : SceneAPI
{
    private Object3D lamp;

    private void Start()
    {
        CreateLamp();
        EditLampSize();
        EditLampColor();
    }

    public void CreateLamp()
    {
        // Spawn lamp at the user's feet
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create the lamp
        lamp = CreateObject("UserLamp", "Lamp", userFeetPosition, new Vector3D(0, 0, 0));

        // Log the creation of the lamp
        Debug.Log("Lamp created");
    }

    public void EditLampSize()
    {
        Debug.Log("Attempting to find and edit the size of the Lamp.");

        if (lamp != null)
        {
            Debug.Log("Lamp found. Retrieving its current size.");

            Vector3D currentSize = lamp.GetSize();

            Debug.Log($"Current Lamp size: X={currentSize.x}, Y={currentSize.y}, Z={currentSize.z}");

            currentSize.x *= 1.5f;
            currentSize.y *= 1.5f;
            currentSize.z *= 1.5f;

            Debug.Log($"Setting new Lamp size: X={currentSize.x}, Y={currentSize.y}, Z={currentSize.z}");

            lamp.SetSize(currentSize);
            Debug.Log("Lamp size successfully updated.");
        }
        else
        {
            Debug.Log("Error: Lamp not found in the scene.");
            // Handle the case where the Lamp does not exist
            // For instance, you might choose to create the Lamp here or notify the user.
        }
    }

    public void EditLampColor()
    {
        if (lamp != null)
        {
            Debug.Log("Lamp found. Setting its color to blue.");

            // Convert RGBA values to 0-1 range
            Color3D blueColor = new Color3D(0, 0, 1, 1);
            lamp.SetColor(blueColor);

            Debug.Log("Lamp color successfully set to blue.");
        }
        else
        {
            Debug.Log("Error: Lamp not found in the scene.");
            // Handle the case where the Lamp does not exist
        }
    }
}