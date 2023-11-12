using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class GiantPinkVaseCreator : SceneAPI
{       
    private Object3D newVase; // Class-level variable to maintain the state of the created vase

    private void Start()
    {
        CreateVase();
        ResizeVase();
        ChangeVaseColorToPink();
    }

    public void CreateVase()
    {
        // Get the user's feet position to spawn the vase
        Vector3D userFeetPosition = GetUsersFeetPosition();
        
        // Create the vase
        newVase = CreateObject("NewVase", "Vase", userFeetPosition, new Vector3D(0, 0, 0));
        if (newVase == null)
        {
            Debug.LogError("Failed to create the vase.");
        }
    }

    public void ResizeVase()
    {
        if (newVase == null)
        {
            Debug.LogError("Vase doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the current size of the vase
        Vector3D vaseSize = newVase.GetSize();

        // Calculate the new size of the vase (3 times its current size)
        Vector3D newVaseSize = new Vector3D(vaseSize.x * 3, vaseSize.y * 3, vaseSize.z * 3);

        // Apply the new size to the vase
        newVase.SetSize(newVaseSize);
    }

    public void ChangeVaseColorToPink()
    {
        if (newVase == null)
        {
            Debug.LogError("Vase doesn't exist. Create it first before changing its color.");
            return;
        }

        // Set the color of the vase to pink with RGBA(255, 192, 203, 1)
        newVase.SetColor(new Color3D(255f / 255f, 192f / 255f, 203f / 255f, 1f)); // RGBA for pink color
    }
}