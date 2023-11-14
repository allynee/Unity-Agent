using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class GlowingVasesScene : SceneAPI
{
    // Declare the vase object
    private Object3D userVase;
    private List<Object3D> vases = new List<Object3D>();

    private void Start()
    {
        GetUserPositionAndOrientation();
        PositionVaseInCircleInFrontOfUser();
        EditVasesIllumination();
    }

    public void GetUserPositionAndOrientation()
    {
        // Get the user's feet position
        Vector3D userPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();

        // Log the user's position and orientation
        Debug.Log($"User Position: ({userPosition.x}, {userPosition.y}, {userPosition.z})");
        Debug.Log($"User Orientation: ({userOrientation.x}, {userOrientation.y}, {userOrientation.z})");
    }

    public void PositionVaseInCircleInFrontOfUser()
    {
        if (userVase == null)
        {
            // Get the user's feet position
            Vector3D positionToCreateVase = GetUsersFeetPosition();
            // Create a new Vase object at the user's feet position
            userVase = CreateObject("UserVase", "Vase", positionToCreateVase, new Vector3D(0, 0, 0));
        }

        // Get the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Calculate the positions for the Vase in a circle in front of the user
        float radius = 1.5f;
        float angle = 0f;
        float angleIncrement = 360f / 8; // Divide the circle into 8 parts

        for (int i = 0; i < 8; i++)
        {
            // Calculate the position on the circle using polar coordinates
            float x = userFeetPosition.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = userFeetPosition.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

            // Set the position of the Vase
            userVase.SetPosition(new Vector3D(x, userFeetPosition.y, z));

            // Increment the angle for the next position
            angle += angleIncrement;
        }
    }

    public void EditVasesIllumination()
    {
        // Get all Vases in the scene
        vases = GetAllVasesInScene();

        // Check if Vases were found
        if (vases.Count == 0)
        {
            Debug.LogError("No Vases found in the scene.");
            return;
        }

        // Set the Illumination property of each Vase to true
        foreach (Object3D vase in vases)
        {
            vase.Illuminate(true);
        }

        Debug.Log("All Vases are now illuminated.");
    }
}