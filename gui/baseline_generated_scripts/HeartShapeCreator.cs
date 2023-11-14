using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class HeartShapeCreator : SceneAPI
{
    private List<Object3D> heartLights = new List<Object3D>();
    private Vector3D wallPosition;
    private Color3D redColor = new Color3D(1, 0, 0, 1); // Red color for the heart

    private void Start()
    {
        wallPosition = GetWallPosition(WallName.Right); // Choose the wall where the heart will be created
        CreateHeartShape();
    }

    private void CreateHeartShape()
    {
        // Create a heart shape using LED Cubes
        // The heart shape is created by placing LED Cubes in specific positions relative to the center of the heart
        // The positions are chosen to resemble a heart shape when viewed from the front
        // The heart shape is made up of 15 LED Cubes
        // The positions are relative to the center of the heart and are chosen to resemble a heart shape
        Vector3D[] relativePositions = new Vector3D[]
        {
            new Vector3D(0, 0, 0),
            new Vector3D(-1, 0, 0),
            new Vector3D(1, 0, 0),
            new Vector3D(-2, -1, 0),
            new Vector3D(2, -1, 0),
            new Vector3D(-1, -1, 0),
            new Vector3D(1, -1, 0),
            new Vector3D(0, -1, 0),
            new Vector3D(-2, -2, 0),
            new Vector3D(2, -2, 0),
            new Vector3D(-1, -2, 0),
            new Vector3D(1, -2, 0),
            new Vector3D(0, -2, 0),
            new Vector3D(-1, -3, 0),
            new Vector3D(1, -3, 0),
        };

        for (int i = 0; i < relativePositions.Length; i++)
        {
            Vector3D position = new Vector3D(wallPosition.x + relativePositions[i].x, wallPosition.y + relativePositions[i].y, wallPosition.z + relativePositions[i].z);
            Object3D ledCube = CreateObject("HeartLight" + i, "LED Cube", position, new Vector3D(0, 0, 0));
            ledCube.SetColor(redColor);
            ledCube.Illuminate(true);
            heartLights.Add(ledCube);
        }
    }
}