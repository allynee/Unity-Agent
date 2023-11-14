using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class LampManager : SceneAPI
{
    // Declare the lamp and table objects
    private Object3D userLamp;
    private Object3D userTable;

    private void Start()
    {
        CreateLamp();
        ChangeLampColorToBlue();
        MoveLampOnTableInView();
        ResizeLampOnTable();
    }

    public void CreateLamp()
    {
        // Get the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create the lamp at the user's feet position
        userLamp = CreateObject("UserLamp", "Lamp", userFeetPosition, new Vector3D(0, 0, 0));

        // Log the creation of the lamp
        Debug.Log("Lamp created at user's feet position");
    }

    public void ChangeLampColorToBlue()
    {
        // Set the color of the lamp to blue
        userLamp.SetColor(new Color3D(0, 0, 1, 1)); // RGBA for blue color
    }

    public void MoveLampOnTableInView()
    {
        // Get all objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Find the first table in the user's field of view
        userTable = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        // Check if the lamp exists before trying to move it
        if (userLamp == null)
        {
            Debug.LogWarning("No lamp found.");
            return;
        }

        if (userTable != null)
        {
            Debug.Log("Found a table to put the lamp on!");

            // Get the position and size of the table
            Vector3D tablePosition = userTable.GetPosition();
            Vector3D tableSize = userTable.GetSize();
            float tableHeight = tableSize.y;

            // Get the size of the lamp
            Vector3D lampSize = userLamp.GetSize();
            float lampHeight = lampSize.y;

            // Calculate the new position for the lamp at the center on top of the table
            Vector3D lampPosition = new Vector3D(tablePosition.x, tablePosition.y + tableHeight + lampHeight / 2, tablePosition.z);
            Debug.Log("Lamp position: " + lampPosition.x + ", " + lampPosition.y + ", " + lampPosition.z);

            // Set the new position for the lamp
            userLamp.SetPosition(lampPosition);
        }
        else
        {
            Debug.LogWarning("No table found in the user's field of view.");
        }
    }

    public void ResizeLampOnTable()
    {
        // Check if the lamp object exists
        if (userLamp == null)
        {
            Debug.LogError("Lamp doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the size and position of the lamp and table
        Vector3D lampSize = userLamp.GetSize();
        Vector3D lampPosition = userLamp.GetPosition();
        Vector3D tableSize = userTable.GetSize();
        Vector3D tablePosition = userTable.GetPosition();

        // Store the new ideal size of the lamp (2.5 times as per instruction)
        const float idealScale = 2.5f;

        // Calculate the maximum allowable size based on the table and ceiling height
        float scaleX = Mathf.Min(idealScale, tableSize.x / lampSize.x);
        float scaleZ = Mathf.Min(idealScale, tableSize.z / lampSize.z);
        float ceilingHeight = GetSceneSize().y;
        float distanceToCeiling = ceilingHeight - (tablePosition.y + tableSize.y);
        float scaleY = Mathf.Min(idealScale, distanceToCeiling / lampSize.y);
        float minScale = Mathf.Min(idealScale, scaleX, scaleY, scaleZ);

        // Calculate the new size of the lamp
        Vector3D newLampSize = new Vector3D(lampSize.x * minScale, lampSize.y * minScale, lampSize.z * minScale);

        // Calculate the new position for the lamp (same x & z, but y is adjusted for the new height)
        Vector3D newPosition = new Vector3D(lampPosition.x, tablePosition.y + tableSize.y, lampPosition.z);

        // Apply the new size and position to the lamp
        userLamp.SetSize(newLampSize);
        userLamp.SetPosition(newPosition);
    }
}