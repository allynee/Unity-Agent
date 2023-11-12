using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class LampController : SceneAPI
{       
    private Object3D userLamp;

    private void Start()
    {
        CreateLamp();
        ChangeLampColorToBlue();
        PlaceLampOnTableInView();
        ResizeLamp();
    }

    public void CreateLamp()
    {
        // Default spawn lamp at the user's feet
        Vector3D userFeetPosition = GetUsersFeetPosition();
        
        // Create the lamp
        userLamp = CreateObject("UserLamp", "Lamp", userFeetPosition, new Vector3D(0, 0, 0));
    }

    public void ChangeLampColorToBlue()
    {
        if (userLamp == null)
        {
            Vector3D positionToCreateLamp = GetUsersFeetPosition(); 
            userLamp = CreateObject("UserLamp", "Lamp", positionToCreateLamp, new Vector3D(0, 0, 0));
        }
        // Set the color of the lamp to blue
        userLamp.SetColor(new Color3D(0, 0, 1, 1)); // RGBA for blue color
    }

    public void PlaceLampOnTableInView()
    {
        // Get all objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Find the first table in the user's field of view
        Object3D table = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        if (table != null)
        {   
            Debug.Log("Found a table to put the lamp on!");
            // Get the position and size of the table
            Vector3D tablePosition = table.GetPosition();
            Vector3D tableSize = table.GetSize();
            float tableHeight = tableSize.y;

            // Get the size of the lamp
            Vector3D lampSize = userLamp.GetSize();
            float lampHeight = lampSize.y;

            Vector3D lampPosition = new Vector3D(tablePosition.x, tablePosition.y + tableHeight + (lampHeight / 2), tablePosition.z); 
            Debug.Log("Lamp position: " + lampPosition.x + ", " + lampPosition.y + ", " + lampPosition.z);

            // Set the new position for the lamp
            userLamp.SetPosition(lampPosition);
        }
        else
        {
            Debug.LogWarning("No table found in the user's field of view.");
        }
    }

    public void ResizeLamp()
    {
        if (userLamp == null)
        {
            Debug.LogError("Lamp doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the current size of the lamp
        Vector3D lampSize = userLamp.GetSize();

        // Calculate the new size of the lamp (1.2 times of its original size)
        Vector3D newLampSize = new Vector3D(lampSize.x * 1.2f, lampSize.y * 1.2f, lampSize.z * 1.2f);

        // Apply the new size to the lamp
        userLamp.SetSize(newLampSize);
    }
}