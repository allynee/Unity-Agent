using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class ChairPlacement : SceneAPI
{
    private void Start()
    {
        CreateChairs();
    }

    public void CreateChairs()
    {
        // Get all the wall names in the enum
        WallName[] wallNames = (WallName[])System.Enum.GetValues(typeof(WallName));

        // Iterate over each wall
        foreach (WallName wallName in wallNames)
        {
            // Get the position of the current wall
            Vector3D wallPosition = GetWallPosition(wallName);

            // Create a new chair object in front of the wall
            // We assume that "in front of the wall" means 1 unit away from the wall in the z-axis
            // We also assume that the chair should be facing away from the wall, which means a rotation of (0, 180, 0)
            Object3D chair = CreateObject("Chair_" + wallName.ToString(), "Chair", new Vector3D(wallPosition.x, wallPosition.y, wallPosition.z + 1), new Vector3D(0, 180, 0));

            // Log the creation of the chair
            Debug.Log("Created a chair in front of the " + wallName.ToString() + " wall at position " + chair.GetPosition().ToString());
        }
    }
}