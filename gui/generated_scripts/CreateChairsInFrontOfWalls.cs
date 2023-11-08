using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;

```csharp
public class CreateChairsInFrontOfWalls : SceneAPI
{   
    private void Start()
    {
        CreateChairs();
    }

    public void CreateChairs()
    {
        Debug.Log("Creating chairs in front of each wall.");

        Vector3D wallPositionLeft = GetWallPosition(WallName.Left);
        Vector3D wallPositionRight = GetWallPosition(WallName.Right);
        Vector3D wallPositionBackLeft = GetWallPosition(WallName.BackLeft);
        Vector3D wallPositionBackRight = GetWallPosition(WallName.BackRight);

        Vector3D chairPositionLeft = new Vector3D(wallPositionLeft.x, 0, wallPositionLeft.z - 1);
        Vector3D chairPositionRight = new Vector3D(wallPositionRight.x, 0, wallPositionRight.z - 1);
        Vector3D chairPositionBackLeft = new Vector3D(wallPositionBackLeft.x + 1, 0, wallPositionBackLeft.z);
        Vector3D chairPositionBackRight = new Vector3D(wallPositionBackRight.x - 1, 0, wallPositionBackRight.z);

        CreateObject("Chair1", "Chair", chairPositionLeft, new Vector3D(0, 0, 0));
        CreateObject("Chair2", "Chair", chairPositionRight, new Vector3D(0, 180, 0));
        CreateObject("Chair3", "Chair", chairPositionBackLeft, new Vector3D(0, 90, 0));
        CreateObject("Chair4", "Chair", chairPositionBackRight, new Vector3D(0, -90, 0));

        Debug.Log("Chairs created in front of each wall.");
    }
}