using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class GiantPinkVase : SceneAPI
{       
    private Object3D userVase;

    private void Start()
    {
        CreateVase();
        ResizeVase();
        EditVaseColorToPink();
        MoveVaseInFrontOfUser();
    }

    private void CreateVase()
    {
        // Default position for the new vase
        Vector3D defaultPosition = new Vector3D(0, 0, 0);
        
        // Create the vase with default size and color
        userVase = CreateObject("UserVase", "Vase", defaultPosition, new Vector3D(0, 0, 0));
        if (userVase == null)
        {
            Debug.LogError("Failed to create the vase.");
        }
    }

    private void ResizeVase()
    {
        if (userVase == null)
        {
            Debug.LogError("Vase doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the original size of the vase
        Vector3D originalSize = userVase.GetSize();

        // Calculate the new size of the vase (3 times its original size)
        Vector3D newSize = new Vector3D(originalSize.x * 3, originalSize.y * 3, originalSize.z * 3);

        // Check if the new size exceeds the surrounding space limitations
        Vector3D sceneSize = GetSceneSize();
        if (newSize.x > sceneSize.x || newSize.y > sceneSize.y || newSize.z > sceneSize.z)
        {
            Debug.LogWarning("The new size exceeds the surrounding space limitations. Aborting resize operation.");
            return;
        }

        // Apply the new size to the vase
        userVase.SetSize(newSize);
    }

    private void EditVaseColorToPink()
    {
        // Set the color of the vase to pink
        userVase.SetColor(new Color3D(1, 0.5f, 0.5f, 1)); // RGBA for pink color
    }

    private void MoveVaseInFrontOfUser()
    {
        try
        {
            // Get the user's head position and orientation
            Vector3D userHeadPosition = GetUsersHeadPosition();
            Vector3D userOrientation = GetUserOrientation();

            // Calculate the new position 0.2 meters in front of the user
            Vector3D newPosition = new Vector3D(
                userHeadPosition.x + userOrientation.x * 0.2f,
                userHeadPosition.y,
                userHeadPosition.z + userOrientation.z * 0.2f
            );

            // Set the position of the vase
            userVase.SetPosition(newPosition);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error moving vase in front of user: {e.Message}");
        }
    }
}