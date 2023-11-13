using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class LampLightShow : SceneAPI
{       
    private Object3D userLamp;
    private Object3D wallObject;
    private Vector3D wallSize;
    private Vector3D wallCenter;
    private Object3D lampObject;
    private Vector3D wallPosition;
    private bool isIlluminated = false;
    private float illuminationToggleInterval = 1.0f;
    private float timeSinceLastToggle = 0.0f;

    private void Start()
    {
        CreateLamp();
        GetWallInfoInFrontOfUser();
        MoveLampToFrontOfWall();
    }

    private void Update()
    {
        timeSinceLastToggle += Time.deltaTime;
        if (timeSinceLastToggle >= illuminationToggleInterval)
        {
            timeSinceLastToggle = 0;
            EditLampIllumination();
        }
    }

    public void CreateLamp()
    {
        // Default spawn lamp at the user's feet
        Vector3D userFeetPosition = GetUsersFeetPosition();
        
        // Create the lamp
        userLamp = CreateObject("UserLamp", "Lamp", userFeetPosition, new Vector3D(0, 0, 0));
    }

    public void GetWallInfoInFrontOfUser()
    {
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D userOrientation = GetUserOrientation();
        Ray userRay = new Ray(userHeadPosition.ToVector3(), userOrientation.ToVector3());
        bool hasHit = false;

        // Get all walls in the scene
        List<Object3D> walls = GetAllObject3DsInScene().FindAll(obj => obj.GetType() == "Wall");

        // Determine the wall that the ray intersects
        foreach (Object3D wall in walls)
        {
            Vector3D wallPosition = wall.GetPosition();
            Plane wallPlane = new Plane(wallPosition.ToVector3(), wallPosition.ToVector3() + Vector3.up, wallPosition.ToVector3() + Vector3.right);

            if (wallPlane.Raycast(userRay, out float distance))
            {
                wallCenter = wallPosition;
                hasHit = true;
                wallObject = wall;
                wallSize = wall.GetSize();
                Debug.Log($"Wall hit: {wall.GetType()}, center: {wallCenter}, size: {wallSize}");
                break; // Exit the loop after finding the wall
            }
        }

        if (!hasHit)
        {
            Debug.LogError("No walls in front of user.");
        }
    }

    public void MoveLampToFrontOfWall()
    {
        // Check if the lamp exists before trying to move it
        if (userLamp == null) 
        {
            Debug.LogError("Lamp object not found.");
            return;
        }

        // Get the position of the wall
        wallPosition = wallCenter;

        // Set the new position for the lamp in front of the wall
        Vector3D newLampPosition = new Vector3D(wallPosition.x, userLamp.GetSize().y / 2, wallPosition.z - userLamp.GetSize().z / 2);
        userLamp.SetPosition(newLampPosition);
    }

    private void EditLampIllumination()
    {
        if (userLamp == null)
        {
            Debug.LogError("Lamp not found in the scene.");
            return;
        }

        // Toggle the illumination state
        isIlluminated = !isIlluminated;

        userLamp.Illuminate(isIlluminated);
    }
}