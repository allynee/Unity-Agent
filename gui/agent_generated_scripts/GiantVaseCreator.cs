using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class GiantVaseCreator : SceneAPI
{       
    private Object3D vaseObject3D;

    private void Start()
    {
        CreateOrFindVase();
        ResizeVase();
    }

    public void CreateOrFindVase()
    {
        // Attempt to find the vase in the user's field of view first.
        List<Object3D> objectsInView = GetAllObject3DsInScene();
        vaseObject3D = objectsInView.Find(obj => obj.GetType().Equals("Vase"));

        // If the vase is not found in the field of view, find it in the scene.
        if (vaseObject3D == null)
        {
            Debug.Log("Vase not found in user's field of view.");
            vaseObject3D = FindObject3DByName("Vase");
            Debug.Log("Vase found in the scene.");
        }

        // If there are no vases in the scene, create one.
        if (vaseObject3D == null)
        {
            Debug.Log("Vase not found in the scene.");
            Vector3D positionToCreateVase = GetUsersFeetPosition();
            vaseObject3D = CreateObject("UserVase", "Vase", positionToCreateVase, new Vector3D(0, 0, 0));
            Debug.Log("Vase created in the scene.");
        }
    }

    public void ResizeVase()
    {
        if (vaseObject3D == null)
        {
            Debug.LogError("Vase doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the current size and position of the vase
        Vector3D vaseSize = vaseObject3D.GetSize();
        Vector3D vasePosition = vaseObject3D.GetPosition();

        // Calculate the new size of the vase (2 times its current size)
        Vector3D newVaseSize = new Vector3D(vaseSize.x * 2, vaseSize.y * 2, vaseSize.z * 2);

        // Check if the new size exceeds the surrounding space limitations
        Vector3D sceneSize = GetSceneSize();
        if (newVaseSize.y > sceneSize.y - vasePosition.y)
        {
            Debug.LogWarning("Vase size exceeds the ceiling height. Cannot resize.");
            return;
        }

        // Apply the new size to the vase
        vaseObject3D.SetSize(newVaseSize);
    }
}