using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class GiantPinkVaseCreator : SceneAPI
{       
    private Object3D vase; // Class-level variable to preserve the vase object reference

    private void Start()
    {
        CreateVaseInCenterOfRoom();
        ResizeVase();
        EditVaseColorToPink();
    }

    private void CreateVaseInCenterOfRoom()
    {
        Vector3D roomCenter = new Vector3D(0, 0, 0);
        vase = CreateObject("Vase", "Vase", roomCenter, new Vector3D(0, 0, 0));
        if (vase != null)
        {
            Debug.Log("Vase created in the center of the room.");
        }
        else
        {
            Debug.LogError("Failed to create vase in the center of the room.");
        }
    }

    private void ResizeVase()
    {
        if (vase == null)
        {
            Debug.LogError("Vase doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the original size of the vase
        Vector3D originalSize = vase.GetSize();

        // Calculate the new size of the vase (2 times the original size)
        Vector3D newSize = new Vector3D(originalSize.x * 2, originalSize.y * 2, originalSize.z * 2);

        // Get the scene size to ensure the vase does not exceed the space limitations
        Vector3D sceneSize = GetSceneSize();

        // Check if the new size exceeds the scene size
        if (newSize.x > sceneSize.x || newSize.y > sceneSize.y || newSize.z > sceneSize.z)
        {
            Debug.LogWarning("Vase size exceeds the surrounding space limitations. Resize operation aborted.");
            return;
        }

        // Apply the new size to the vase
        vase.SetSize(newSize);
    }

    private void EditVaseColorToPink()
    {
        if (vase != null)
        {
            // Set the color of the vase to pink
            vase.SetColor(new Color3D(1, 0.5f, 0.8f, 1)); // RGBA for pink color
        }
        else
        {
            Debug.LogError("Vase not found in the scene.");
        }
    }
}