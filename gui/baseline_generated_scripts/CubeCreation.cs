using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class CubeCreation : SceneAPI
{
    private Object3D table;
    private Object3D cube;

    private void Start()
    {
        FindTable();
        CreateCube();
        PositionCube();
    }

    private void FindTable()
    {
        // Get all objects in the scene
        List<Object3D> objectsInScene = GetAllObject3DsInScene();

        // Find the table in the scene
        table = objectsInScene.Find(obj => obj.GetType().Equals("Table"));

        // Log if table is not found
        if (table == null)
        {
            Debug.Log("Table not found in the scene.");
        }
    }

    private void CreateCube()
    {
        // Check if the object type "LED Cube" is valid
        if (IsObjectTypeValid("LED Cube"))
        {
            // Create a new cube at the origin with no rotation
            cube = CreateObject("CoolCube", "LED Cube", new Vector3D(0, 0, 0), new Vector3D(0, 0, 0));

            // Set the cube's color to a cool blue
            cube.SetColor(new Color3D(0, 0, 1, 1));

            // Make the cube luminous
            cube.SetLuminousIntensity(1.0f);
            cube.Illuminate(true);
        }
        else
        {
            Debug.Log("LED Cube is not a valid object type.");
        }
    }

    private void PositionCube()
    {
        // Check if the table and cube were successfully found/created
        if (table != null && cube != null)
        {
            // Get the table's position and size
            Vector3D tablePos = table.GetPosition();
            Vector3D tableSize = table.GetSize();

            // Calculate the position for the cube to be floating above the table
            Vector3D cubePos = new Vector3D(tablePos.x, tablePos.y + tableSize.y + 1, tablePos.z);

            // Set the cube's position
            cube.SetPosition(cubePos);
        }
        else
        {
            Debug.Log("Table or cube not found. Cannot position cube.");
        }
    }
}