using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class RainbowLightController : SceneAPI
{       
    private List<Object3D> ledCubes = new List<Object3D>();
    private float colorChangeInterval = 1.0f;
    private float timeSinceLastColorChange = 0.0f;
    private int currentRainbowColorIndex = 0;

    private void Start()
    {
        CreateThirtyLEDCubes();
        GetClosestWallInfo();
        PositionLedCubesInCircles();
    }

    private void Update()
    {
        EditLEDsIllumination();
    }

    public void CreateThirtyLEDCubes()
    {
        for (int i = 0; i < 30; i++)
        {
            // Create LED Cube at a default position
            Object3D cube = CreateObject($"UserLEDCube_{i}", "LED Cube", new Vector3D(0, 0, 0), new Vector3D(0, 0, 0));
            ledCubes.Add(cube);
        }
    }

    public void GetClosestWallInfo()
    {
        Vector3 wallSize;
        Vector3 wallCenter;
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D userOrientation = GetUserOrientation();
        Ray userRay = new Ray(userHeadPosition.ToVector3(), userOrientation.ToVector3());
        bool hasHit = false;

        // Get all walls in the scene
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        // Determine the wall that the ray intersects
        foreach (WallName wall in Enum.GetValues(typeof(WallName)))
        {
            Vector3D wallPosition = GetWallPosition(wall);
            Plane wallPlane = new Plane(wallPosition.ToVector3(), wallPosition.ToVector3() + Vector3.up, wallPosition.ToVector3() + Vector3.right);

            if (wallPlane.Raycast(userRay, out float distance))
            {
                wallCenter = wallPosition.ToVector3();
                hasHit = true;
                // Find the matching GameObject
                foreach (GameObject wallObj in walls)
                {
                    if ((wallObj.transform.position - wallCenter).sqrMagnitude < 0.1f) // Using a small threshold for accuracy
                    {
                        // Set the class variables for size of the closest wall
                        wallSize = wallObj.GetComponent<BoxCollider>().size;
                        Debug.Log($"Wall hit: {wall}, center: {wallCenter}, size: {wallSize}");
                        break; // Exit the loop after finding the wall
                    }
                }
                break; // Exit the loop after finding the wall
            }
        }

        if (!hasHit)
        {
            Debug.LogError("No walls in front of user.");
        }
    }

    public void PositionLedCubesInCircles()
    {
        // Calculate the circle's radius based on the wall's size, smaller of the two dimensions, with a margin
        float circleRadius = Mathf.Min(SceneAPI.GetSceneSize().x, SceneAPI.GetSceneSize().z) / 6; // 1/6th of the smaller dimension
        float angleStep = 360f / ledCubes.Count;
        wallCenter = SceneAPI.GetWallPosition(WallName.BackLeft);

        // Position the LED cubes in three circles on the wall
        for (int circle = 0; circle < 3; circle++)
        {
            float circleOffset = circle * 120f; // Offset each circle by 120 degrees

            for (int i = 0; i < 10; i++)
            {
                float angle = angleStep * i + circleOffset;
                float x = wallCenter.x + circleRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float y = wallCenter.y + circleRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
                float z = wallCenter.z;
                Vector3D cubePosition = new Vector3D(x, y, z);
                ledCubes[circle * 10 + i].Levitate(true);
                ledCubes[circle * 10 + i].SetPosition(cubePosition);
            }
        }
    }

    public void EditLEDsIllumination()
    {
        timeSinceLastColorChange += Time.deltaTime;

        if (timeSinceLastColorChange >= colorChangeInterval)
        {
            timeSinceLastColorChange = 0;

            foreach (Object3D ledCube in ledCubes)
            {
                Color3D newColor = GetNextRainbowColor();
                ledCube.SetColor(newColor);
                ledCube.Illuminate(!ledCube.IsLit());
            }
        }
    }

    private Color3D GetNextRainbowColor()
    {
        Color3D[] rainbowColors = new Color3D[]
        {
            new Color3D(1.0f, 0.0f, 0.0f, 1.0f),  // Red
            new Color3D(1.0f, 0.5f, 0.0f, 1.0f),  // Orange
            new Color3D(1.0f, 1.0f, 0.0f, 1.0f),  // Yellow
            new Color3D(0.0f, 1.0f, 0.0f, 1.0f),  // Green
            new Color3D(0.0f, 0.0f, 1.0f, 1.0f),  // Blue
            new Color3D(0.3f, 0.0f, 0.5f, 1.0f),  // Indigo
            new Color3D(0.5f, 0.0f, 0.5f, 1.0f)   // Violet
        };

        return rainbowColors[GetNextRainbowColorIndex()];
    }

    private int GetNextRainbowColorIndex()
    {
        currentRainbowColorIndex = (currentRainbowColorIndex + 1) % 7;
        return currentRainbowColorIndex;
    }
}