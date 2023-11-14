using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CircleOfLights : SceneAPI
{       
    private List<Object3D> ledCubes = new List<Object3D>();
    private GameObject wallObject;
    private Vector3 wallSize;
    private Vector3 wallCenter;

    private void Start()
    {
        CreateTenLEDCubes();
        GetClosestWallInfo();
        PositionLedCubesInCircle();
    }

    public void CreateTenLEDCubes()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();

        for (int i = 0; i < 10; i++)
        {
            // Create LED Cube at a default position
            Object3D cube = CreateObject($"UserLEDCube_{i}", "LED Cube", new Vector3D(0, 0, 0), new Vector3D(0, 0, 0));
            ledCubes.Add(cube);
        }
    }

    public void GetClosestWallInfo()
    {
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
                        wallObject = wallObj;
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

    public void PositionLedCubesInCircle()
    {
        // Calculate the circle's radius based on the wall's size, smaller of the two dimensions, with a margin
        float circleRadius = Mathf.Min(wallSize.x, wallSize.z) / 2 * 0.2f; // 20% of the half-width or half-height
        float angleStep = 360f / ledCubes.Count;

        // Use the wall's right and forward vectors to align the circle parallel to the wall
        Vector3 wallRight = wallObject.transform.right;
        Vector3 wallForward = wallObject.transform.forward;

        // Adjust the offset to place the LEDs slightly away from the wall
        Vector3 wallOffset = wallObject.transform.up * 0.05f; // 5cm away from the wall

        // Position the LED cubes in a circle on the wall
        for (int i = 0; i < ledCubes.Count; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad; // Convert angle to radians

            Vector3 localOffset = wallRight * Mathf.Cos(angle) * circleRadius + 
                                wallForward * Mathf.Sin(angle) * circleRadius;

            // Position the LED cubes in a circle parallel to the wall using the local offset
            Vector3 cubePosition = wallCenter + localOffset + wallOffset;

            // Convert to your Vector3D type if needed
            Vector3D cubePosition3D = new Vector3D(cubePosition.x, cubePosition.y, cubePosition.z);

            ledCubes[i].Levitate(true); // Set levitate to true else it'll fall to the ground
            ledCubes[i].SetPosition(cubePosition3D);

            // Rotate the LED cube to be parallel to the wall
            Quaternion cubeRotation = Quaternion.LookRotation(wallObject.transform.forward, wallObject.transform.up);
            // Convert the rotation from a Quaternion to Euler angles
            Vector3 cubeEulerAngles = cubeRotation.eulerAngles;

            // Set the rotation of the LED cube using Vector3D
            Vector3D cubeRotation3D = new Vector3D(cubeEulerAngles.x, cubeEulerAngles.y, cubeEulerAngles.z);
            ledCubes[i].SetRotation(cubeRotation3D);
        }
    }
}