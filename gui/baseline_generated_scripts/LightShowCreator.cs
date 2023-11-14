using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class LightShowCreator : SceneAPI
{
    private List<Object3D> ledCubes = new List<Object3D>();
    private int currentCubeIndex = 0;
    private Color3D[] colors = new Color3D[]
    {
        new Color3D(1, 0, 0, 1), // Red
        new Color3D(0, 1, 0, 1), // Green
        new Color3D(0, 0, 1, 1), // Blue
        new Color3D(1, 1, 0, 1), // Yellow
        new Color3D(1, 0, 1, 1), // Magenta
        new Color3D(0, 1, 1, 1)  // Cyan
    };
    private int currentColorIndex = 0;

    private void Start()
    {
        CreateLEDCubes();
    }

    private void Update()
    {
        ChangeCubeColors();
    }

    private void CreateLEDCubes()
    {
        Vector3D wallPosition = GetWallPosition(WallName.Right);
        for (int i = 0; i < 10; i++)
        {
            string cubeName = "LEDCube" + i;
            Vector3D cubePosition = new Vector3D(wallPosition.x, wallPosition.y + i, wallPosition.z);
            Object3D cube = CreateObject(cubeName, "LED Cube", cubePosition, new Vector3D(0, 0, 0));
            ledCubes.Add(cube);
        }
    }

    private void ChangeCubeColors()
    {
        if (ledCubes.Count > 0)
        {
            Object3D currentCube = ledCubes[currentCubeIndex];
            currentCube.SetColor(colors[currentColorIndex]);
            currentCube.Illuminate(true);

            currentCubeIndex = (currentCubeIndex + 1) % ledCubes.Count;
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
        }
    }
}