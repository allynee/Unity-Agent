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
        ChangeVaseColorToPink();
        PositionVaseInFrontOfUser();
    }

    public void CreateVase()
    {
        // Get the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create the vase at the user's feet position
        userVase = CreateObject("UserVase", "Vase", userFeetPosition, new Vector3D(0, 0, 0));

        // Log the creation of the vase
        Debug.Log("Vase created at user's feet position");
    }

    public void ResizeVase()
    {
        if (userVase == null)
        {
            Debug.LogError("Vase doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the current size of the vase
        Vector3D vaseSize = userVase.GetSize();

        // Calculate the new size for the vase
        Vector3D newVaseSize = new Vector3D(vaseSize.x * 3, vaseSize.y * 3, vaseSize.z * 3);

        // Check if the new size exceeds the surrounding space limitations
        Vector3D sceneSize = GetSceneSize();
        Vector3D vasePosition = userVase.GetPosition();
        Vector3D vaseBottom = new Vector3D(vasePosition.x, vasePosition.y, vasePosition.z);
        Vector3D vaseTop = new Vector3D(vasePosition.x, vasePosition.y + newVaseSize.y, vasePosition.z);
        Vector3D usersFeetPosition = GetUsersFeetPosition();

        if (vaseTop.y > sceneSize.y || vaseBottom.y < usersFeetPosition.y)
        {
            Debug.LogWarning("The new size exceeds the surrounding space limitations. Cannot resize the vase.");
            return;
        }

        // Set the new size for the vase
        userVase.SetSize(newVaseSize);

        // Log the action
        Debug.Log("The size of the vase has been updated.");
    }

    public void ChangeVaseColorToPink()
    {
        // Check if the Vase object is null
        if (userVase == null)
        {
            // Get the user's feet position to create the Vase
            Vector3D positionToCreateVase = GetUsersFeetPosition();

            // Create the Vase object
            userVase = CreateObject("UserVase", "Vase", positionToCreateVase, new Vector3D(0, 0, 0));
        }

        // Set the color of the Vase to pink
        userVase.SetColor(new Color3D(1, 0.5f, 0.5f, 1)); // RGBA for pink color
    }

    public void PositionVaseInFrontOfUser()
    {
        if (userVase == null)
        {
            Vector3D positionToCreateVase = GetUsersFeetPosition();
            userVase = CreateObject("UserVase", "Vase", positionToCreateVase, new Vector3D(0, 0, 0));
        }

        // Get the user's feet position and orientation
        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();

        // Assume a default 0.2m in front, but adjust based on objects and walls.
        float defaultDistance = 0.2f;

        // Convert user feet position to Vector3 for distance calculations.
        Vector3 unityUserFeetPos = userFeetPosition.ToVector3();

        float minDistance = defaultDistance;

        // Get all objects in the field of view.
        var objectsInView = GetAllObject3DsInFieldOfView();

        // Check each object in the field of view to find the minimum distance
        foreach (var obj in objectsInView)
        {
            // Do not account for object you are trying to move
            if (obj.name != "UserVase")
            {
                Vector3 objectPosition = obj.GetPosition().ToVector3();
                float distance = Vector3.Distance(new Vector3(objectPosition.x, unityUserFeetPos.y, objectPosition.z), unityUserFeetPos);
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }
        }

        // Check all wall positions to find the closest distance
        foreach (WallName wall in Enum.GetValues(typeof(WallName)))
        {
            Vector3D wallPosition = GetWallPosition(wall);
            Vector3 unityWallPosition = wallPosition.ToVector3();
            float wallDistance = Vector3.Distance(new Vector3(unityWallPosition.x, unityUserFeetPos.y, unityWallPosition.z), unityUserFeetPos);
            if (wallDistance < minDistance)
            {
                minDistance = wallDistance;
            }
        }

        // Ensure the vase is not placed too close to the user or objects
        minDistance = Mathf.Max(minDistance - 0.1f, 0.1f); // Use 0.1m offset

        // Calculate the new spawn position based on the minimum distance found
        Vector3D spawnPosition = new Vector3D(
            userFeetPosition.x + userOrientation.x * minDistance,
            userFeetPosition.y,
            userFeetPosition.z + userOrientation.z * minDistance
        );

        // Set the position of the vase
        userVase.SetPosition(spawnPosition);
    }
}