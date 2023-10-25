using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;

public class PinkLampScript : SceneAPI
{
    private Object3D lamp;

    private void Start()
    {
        CreateLamp();
        EditLampSize();
        EditLampColor();
        EditLampPosition();
    }

    public void CreateLamp()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        
        lamp = CreateObject("UserLamp", "Lamp", userFeetPosition, new Vector3D(0, 0, 0));
        
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

            currentSize.x *= 0.5f;
            currentSize.y *= 0.5f;
            currentSize.z *= 0.5f;

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
            Debug.Log("Lamp found. Setting its color to pink.");

            // Convert RGBA values to 0-1 range
            Color3D pinkColor = new Color3D(255 / 255f, 192 / 255f, 203 / 255f, 1);
            lamp.SetColor(pinkColor);

            Debug.Log("Lamp color successfully set to pink.");
        }
        else
        {
            Debug.Log("Error: Lamp not found in the scene.");
            // Handle the case where the Lamp does not exist
        }
    }

    public void EditLampPosition()
    {
        if (lamp != null)
        {
            Debug.Log("Lamp found. Editing its position.");

            Vector3D userPosition = GetUsersHeadPosition();
            Vector3D userRotation = GetUsersHeadRotation();

            lamp.SetPosition(userPosition);
            lamp.SetRotation(userRotation);

            Debug.Log("Lamp position successfully edited.");
        }
        else
        {
            Debug.Log("Error: Lamp not found in the scene.");
            // Handle the case where the Lamp does not exist
        }
    }
}