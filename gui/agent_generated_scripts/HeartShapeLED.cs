using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class HeartShapeLED : SceneAPI
{
    private List<Object3D> ledCubes = new List<Object3D>();
    private GameObject wallObject;
    private Vector3 wallSize;
    private Vector3 wallCenter;
    private bool isIlluminated = false;
    private float illuminationToggleInterval = 1.0f;
    private float timeSinceLastToggle = 0.0f;

    private void Start()
    {
        CreateTwentyLEDCubes();
        GetClosestWallInfo();
        PositionLedCubesInHeartShape();
    }

    private void Update()
    {
        timeSinceLastToggle += Time.deltaTime;
        if (timeSinceLastToggle >= illuminationToggleInterval)
        {
            timeSinceLastToggle = 0;
            EditLEDsIllumination();
        }
    }

    public void CreateTwentyLEDCubes()
    {
        for (int i = 0; i < 20; i++)
        {
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

    public void PositionLedCubesInHeartShape()
    {
        float heartWidth = wallSize.x * 0.4f;
        float heartHeight = wallSize.y * 0.4f;
        float xCenter = wallCenter.x;
        float yCenter = wallCenter.y;

        for (int i = 0; i < ledCubes.Count; i++)
        {
            float angle = i * Mathf.PI * 2 / ledCubes.Count;
            float x = heartWidth * Mathf.Pow(Mathf.Sin(angle), 3);
            float y = -heartHeight * (13 * Mathf.Cos(angle) - 5 * Mathf.Cos(2 * angle) - 2 * Mathf.Cos(3 * angle) - Mathf.Cos(4 * angle));
            Vector3 cubePosition = new Vector3(xCenter + x, yCenter + y, wallCenter.z);
            Vector3D cubePosition3D = new Vector3D(cubePosition.x, cubePosition.y, cubePosition.z);
            ledCubes[i].Levitate(true);
            ledCubes[i].SetPosition(cubePosition3D);
        }
    }

    public void EditLEDsIllumination()
    {
        if (ledCubes == null || ledCubes.Count == 0)
        {
            Debug.LogError("No LED cubes found in the scene.");
            return;
        }

        isIlluminated = !isIlluminated;

        foreach (Object3D ledCube in ledCubes)
        {
            ledCube.Illuminate(isIlluminated);
        }
    }
}