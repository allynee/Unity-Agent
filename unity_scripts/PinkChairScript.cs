using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;

public class PinkChairScript : SceneAPI
{
    private Object3D chair;

    private void Start()
    {
        CreateChair();
        EditChairColor();
    }

    public void CreateChair()
    {
        // Get the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create the chair object
        chair = CreateObject("UserChair", "Chair", userFeetPosition, new Vector3D(0, 0, 0));

        // Log the creation of the chair
        Debug.Log("Chair created");
    }

    public void EditChairColor()
    {
        if (chair != null)
        {
            Debug.Log("Chair found. Setting its color to pink.");

            // Convert RGBA values to 0-1 range
            Color3D pinkColor = new Color3D(255 / 255f, 192 / 255f, 203 / 255f, 1);
            chair.SetColor(pinkColor);

            Debug.Log("Chair color successfully set to pink.");
        }
        else
        {
            Debug.Log("Error: Chair not found in the scene.");
            // Handle the case where the chair does not exist
        }
    }
}