using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class LEDCubeController : SceneAPI
{
    private Object3D userTable;
    private Object3D ledCube;
    private bool isIlluminated = false;
    private float illuminationToggleInterval = 1.0f;
    private float timeSinceLastToggle = 0.0f;

    private void Start()
    {
        FindTableInFieldOfView();
        CreateLEDCube();
        AdjustLedCubePosition();
        LevitateLedCubeAboveTable();
        ChangeLedCubeColorToPink();
        EditLedCubeLuminousIntensity();
    }

    private void Update()
    {
        timeSinceLastToggle += Time.deltaTime;
        if (timeSinceLastToggle >= illuminationToggleInterval)
        {
            timeSinceLastToggle = 0;
            ToggleLEDIllumination();
        }
    }

    private void FindTableInFieldOfView()
    {
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();
        userTable = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        if (userTable != null)
        {
            Debug.Log("Table found in the user's field of view!");
        }
        else
        {
            Debug.LogWarning("No table found in the user's field of view.");
        }
    }

    public void CreateLEDCube()
    {
        Vector3D defaultPosition = new Vector3D(0, 0, 0);
        ledCube = CreateObject("LEDCube", "LED Cube", defaultPosition, new Vector3D(0, 0, 0));
    }

    public void AdjustLedCubePosition()
    {
        Vector3D ledCubePosition = ledCube.GetPosition();
        Vector3D tablePosition = userTable.GetPosition();
        float newLedCubeYPosition = tablePosition.y + 1.0f;

        if (newLedCubeYPosition < ledCubePosition.y)
        {
            Debug.Log("Collision detected, adjusting LED cube position");
            newLedCubeYPosition = ledCubePosition.y + 0.1f;
        }

        ledCube.SetPosition(new Vector3D(ledCubePosition.x, newLedCubeYPosition, ledCubePosition.z));
    }

    private void LevitateLedCubeAboveTable()
    {
        if (ledCube != null)
        {
            Vector3D tablePosition = userTable.GetPosition();
            Vector3D cubePosition = new Vector3D(tablePosition.x, tablePosition.y + 1f, tablePosition.z);
            ledCube.SetPosition(cubePosition);
            ledCube.Levitate(true);
        }
        else
        {
            Debug.Log("LED Cube not found in the scene");
        }
    }

    private void ChangeLedCubeColorToPink()
    {
        if (ledCube != null)
        {
            ledCube.SetColor(new Color3D(1, 0.5f, 0.5f, 1));
            Debug.Log("LED Cube color changed to pink.");
        }
        else
        {
            Debug.LogError("LED Cube not found in the scene.");
        }
    }

    private void ToggleLEDIllumination()
    {
        if (ledCube == null)
        {
            Debug.LogError("LED Cube not found in the scene.");
            return;
        }

        isIlluminated = !isIlluminated;
        ledCube.Illuminate(isIlluminated);
    }

    private void EditLedCubeLuminousIntensity()
    {
        if (ledCube != null)
        {
            ledCube.SetLuminousIntensity(8);
            Debug.Log("LED Cube luminous intensity edited to 8 for a bright glow");
        }
        else
        {
            Debug.LogError("LED Cube not found in the scene");
        }
    }
}