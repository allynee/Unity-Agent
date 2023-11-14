using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class VaseManager : SceneAPI
{
    // Declare the vase object
    private Object3D vase;

    private void Start()
    {
        CreateVase();
        ChangeVaseColorToPink();
        EnlargeVase();
        PositionVaseInFrontOfUser();
    }

    public void CreateVase()
    {
        // Get the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create the vase at the user's feet position
        vase = CreateObject("UserVase", "Vase", userFeetPosition, new Vector3D(0, 0, 0));

        // Log the creation of the vase
        Debug.Log("Vase created at user's feet position");
    }

    public void ChangeVaseColorToPink()
    {
        // Check if the Vase object is null
        if (vase == null)
        {
            Debug.LogError("Vase doesn't exist. Create it first before changing its color.");
            return;
        }

        // Set the color of the Vase to pink
        vase.SetColor(new Color3D(1, 0.75f, 0.80f, 1)); // RGBA for pink color
    }

    public void EnlargeVase()
    {
        // Check if the vase object exists
        if (vase == null)
        {
            Debug.LogError("Vase doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the current size of the vase
        Vector3D vaseSize = vase.GetSize();

        // Store the new ideal size of the vase (assuming 10 times as per instruction)
        const float idealScale = 10f;

        // When enlarging the vase, we need to consider the environment to ensure the vase does not exceed the max possible size.
        // First, account for the table that the vase is on for the max x & z size.
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();
        Object3D table = objectsInView.Find(obj => obj.GetType().Equals("Table"));
        if (table == null)
        {
            Debug.LogWarning("The vase does not seem to be on a table.");
            return;
        }

        Vector3D tableSize = table.GetSize();
        Vector3D tablePosition = table.GetPosition();
        float scaleX = Mathf.Min(idealScale, tableSize.x / vaseSize.x);
        float scaleZ = Mathf.Min(idealScale, tableSize.z / vaseSize.z);

        // Next, we get the ceiling height for the max y size.
        Vector3D sceneSize = GetSceneSize();
        float ceilingHeight = sceneSize.y;
        float distanceToCeiling = ceilingHeight - (tablePosition.y + tableSize.y);
        float scaleY = Mathf.Min(idealScale, distanceToCeiling / vaseSize.y);

        // Now we calculate the new size of the vase
        float minScale = Mathf.Min(idealScale, scaleX, scaleY, scaleZ); 
        Vector3D newVaseSize = new Vector3D(vaseSize.x * minScale, vaseSize.y * minScale, vaseSize.z * minScale);

        // Calculate the new position for the vase (same x & z, but y is adjusted for the new height)
        Vector3D newPosition = new Vector3D(
            tablePosition.x, 
            tablePosition.y + tableSize.y, 
            tablePosition.z 
        );

        // Apply the new size and position to the vase
        vase.SetSize(newVaseSize);
        vase.SetPosition(newPosition);

        // Log the action
        Debug.Log("The size of the vase has been updated.");
    }

    public void PositionVaseInFrontOfUser()
    {
        // Check if the vase object is null
        if (vase == null)
        {
            Debug.LogError("Vase doesn't exist. Create it first before positioning it.");
            return;
        }

        // Get the user's feet position and orientation
        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();

        // Assume a default 0.5m in front
        float defaultDistance = 0.5f;

        // Calculate the new position for the vase based on the user's orientation and the default distance
        Vector3D newPosition = new Vector3D(
            userFeetPosition.x + userOrientation.x * defaultDistance,
            userFeetPosition.y,
            userFeetPosition.z + userOrientation.z * defaultDistance
        );

        // Set the position of the vase
        vase.SetPosition(newPosition);
    }
}