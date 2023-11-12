using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CubeController : SceneAPI
{
    private Object3D userTable;
    private Object3D ledCube;
    private bool isIlluminated = false;
    private float illuminationToggleInterval = 1.5f;
    private float timeSinceLastToggle = 0.0f;

    private void Start()
    {
        FindTableInFieldOfView();
        CreateLEDCube();
        EditLedCubePosition();
        EditLEDCubeIllumination();
        ChangeLedCubeColorToPink();
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

    public void EditLedCubePosition()
    {
        if (userTable != null)
        {
            Vector3D tablePosition = userTable.GetPosition();
            Vector3D newLedCubePosition = new Vector3D(tablePosition.x, tablePosition.y + 1, tablePosition.z);
            ledCube.SetPosition(newLedCubePosition);
        }
        else
        {
            Debug.Log("Table not found in the scene");
        }
    }

    private void EditLEDCubeIllumination()
    {
        if (ledCube != null)
        {
            ledCube.Illuminate(true);
        }
        else
        {
            Debug.LogError("LED Cube not found in the scene.");
        }
    }

    private void ChangeLedCubeColorToPink()
    {
        if (ledCube != null)
        {
            ledCube.SetColor(new Color3D(1, 0.5f, 0.5f, 1)); // RGBA for pink color
            Debug.Log("LED Cube color changed to pink.");
        }
        else
        {
            Debug.LogError("LED Cube not found in the scene.");
        }
    }

    private void ToggleLEDIllumination()
    {
        if (ledCube != null)
        {
            isIlluminated = !isIlluminated;
            ledCube.Illuminate(isIlluminated);
        }
        else
        {
            Debug.LogError("LED Cube not found in the scene.");
        }
    }
}