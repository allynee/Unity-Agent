using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class RainbowLightCircles : SceneAPI
{       
    // Declare the LED Cubes list to track the created LED Cubes
    private List<Object3D> ledCubes = new List<Object3D>();
    private GameObject wallObject;
    private Vector3 wallSize;
    private Vector3 wallCenter;
    private float illuminationChangeInterval = 1.0f;
    private float timeSinceLastIlluminationChange = 0.0f;

    private void Start()
    {
        CreateThreeLEDCubes();
        GetClosestWallInfo();
        PositionLedCubesInCircle();
    }

    private void Update()
    {
        EditLEDsIllumination();
    }

    public void CreateThreeLEDCubes()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();

        for (int i = 0; i < 3; i++)
        {
            Object3D cube = CreateObject($"UserLEDCube_{i}", "LED Cube", new Vector3D(userFeetPosition.x + i, userFeetPosition.y, userFeetPosition.z), new Vector3D(0, 0, 0));
            ledCubes.Add(cube);
        }
    }

    public void GetClosestWallInfo()
    {
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D userOrientation = GetUserOrientation();
        Ray userRay = new Ray(userHeadPosition.ToVector3(), userOrientation.ToVector3());
        bool hasHit = false;

        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (WallName wall in Enum.GetValues(typeof(WallName)))
        {
            Vector3D wallPosition = GetWallPosition(wall);
            Plane wallPlane = new Plane(wallPosition.ToVector3(), wallPosition.ToVector3() + Vector3.up, wallPosition.ToVector3() + Vector3.right);

            if (wallPlane.Raycast(userRay, out float distance))
            {
                wallCenter = wallPosition.ToVector3();
                hasHit = true;

                foreach (GameObject wallObj in walls)
                {
                    if ((wallObj.transform.position - wallCenter).sqrMagnitude < 0.1f)
                    {
                        wallSize = wallObj.GetComponent<BoxCollider>().size;
                        wallObject = wallObj;
                        Debug.Log($"Wall hit: {wall}, center: {wallCenter}, size: {wallSize}");
                        break;
                    }
                }
                break;
            }
        }

        if (!hasHit)
        {
            Debug.LogError("No walls in front of user.");
        }
    }

    public void PositionLedCubesInCircle()
    {
        float circleRadius = Mathf.Min(wallSize.x, wallSize.z) / 2 * 0.2f;
        float angleStep = 360f / ledCubes.Count;
        Vector3 wallRight = wallObject.transform.right;
        Vector3 wallForward = wallObject.transform.forward;
        Vector3 wallOffset = wallObject.transform.up * 0.05f;

        for (int i = 0; i < ledCubes.Count; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 localOffset = wallRight * Mathf.Cos(angle) * circleRadius + wallForward * Mathf.Sin(angle) * circleRadius;
            Vector3 cubePosition = wallCenter + localOffset + wallOffset;
            Vector3D cubePosition3D = new Vector3D(cubePosition.x, cubePosition.y, cubePosition.z);

            ledCubes[i].Levitate(true);
            ledCubes[i].SetPosition(cubePosition3D);

            Quaternion cubeRotation = Quaternion.LookRotation(wallObject.transform.forward, wallObject.transform.up);
            Vector3 cubeEulerAngles = cubeRotation.eulerAngles;
            Vector3D cubeRotation3D = new Vector3D(cubeEulerAngles.x, cubeEulerAngles.y, cubeEulerAngles.z);
            ledCubes[i].SetRotation(cubeRotation3D);
        }
    }

    public void EditLEDsIllumination()
    {
        timeSinceLastIlluminationChange += Time.deltaTime;

        if (timeSinceLastIlluminationChange >= illuminationChangeInterval)
        {
            timeSinceLastIlluminationChange = 0;

            foreach (Object3D ledCube in ledCubes)
            {
                ledCube.Illuminate(!ledCube.IsLit());
            }
        }
    }
}