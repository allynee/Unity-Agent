using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ChairPlacementManager : SceneAPI
{       
    private const int CHAIR_COUNT = 4;
    private List<Object3D> chairs = new List<Object3D>();

    private void Start()
    {
        CreateChairs();
        PositionChairsInFrontOfWalls();
    }

    public void CreateChairs()
    {
        for (int i = 0; i < CHAIR_COUNT; i++)
        {
            Object3D chair = CreateObject($"Chair_{i}", "Chair", new Vector3D(0, 0, 0), new Vector3D(0, 0, 0));
            if (chair != null)
            {
                chairs.Add(chair);
                Debug.Log($"Chair {i} created successfully");
            }
            else
            {
                Debug.LogError($"Failed to create Chair {i}");
            }
        }
    }

    private void PositionChairsInFrontOfWalls()
    {
        int chairIndex = 0;
        foreach (WallName wall in Enum.GetValues(typeof(WallName)))
        {
            Vector3D wallPosition = GetWallPosition(wall);
            if (wallPosition == null)
            {
                Debug.LogError($"Failed to get position for wall: {wall}");
                return;
            }
            Vector3D chairPosition = CalculateChairPositionInFrontOfWall(wallPosition);
            chairs[chairIndex].SetPosition(chairPosition);
            chairIndex++;
        }
    }

    private Vector3D CalculateChairPositionInFrontOfWall(Vector3D wallPosition)
    {
        float xOffset = 0;
        float zOffset = 0;

        if (wallPosition.x > 0)
            xOffset = -0.5f;
        else if (wallPosition.x < 0)
            xOffset = 0.5f;

        if (wallPosition.z > 0)
            zOffset = -0.5f;
        else if (wallPosition.z < 0)
            zOffset = 0.5f;

        return new Vector3D(
            wallPosition.x + xOffset,
            0,
            wallPosition.z + zOffset
        );
    }
}