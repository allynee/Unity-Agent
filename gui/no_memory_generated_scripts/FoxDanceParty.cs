using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class FoxDanceParty : SceneAPI
{
    private Object3D pushButton;
    private List<Object3D> foxes = new List<Object3D>();
    private Object3D ledCube;

    private void Start()
    {
        CreateFoxes();
        CreateLedCube();
        CreatePushButtonInFrontOfUser();
        EditFoxSize();
        EditFoxRotation();
        EditLedCubeProperties();
        SetPushButtonBehaviour();
    }

    private void CreateFoxes()
    {
        Vector3D centerPosition = new Vector3D(0f, 0f, 0f);
        foxes.Add(CreateObject("FirstFox", "Fox", centerPosition, new Vector3D(0f, 0f, 0f)));
        foxes.Add(CreateObject("SecondFox", "Fox", new Vector3D(centerPosition.x - 2f, centerPosition.y, centerPosition.z), new Vector3D(0f, 0f, 0f)));
        foxes.Add(CreateObject("ThirdFox", "Fox", new Vector3D(centerPosition.x + 2f, centerPosition.y, centerPosition.z), new Vector3D(0f, 0f, 0f)));
    }

    private void EditFoxSize()
    {
        foreach (Object3D fox in foxes)
        {
            Vector3D currentSize = fox.GetSize();
            Vector3D newSize = new Vector3D(currentSize.x * 1.5f, currentSize.y * 1.5f, currentSize.z * 1.5f);
            fox.SetSize(newSize);
        }
    }

    private void EditFoxRotation()
    {
        Vector3D roomCenter = new Vector3D(0f, 0f, 0f);
        foreach (Object3D fox in foxes)
        {
            Vector3D directionToCenter = roomCenter - fox.GetPosition();
            Vector3D newRotation = new Vector3D(
                Mathf.Atan2(directionToCenter.z, directionToCenter.x) * Mathf.Rad2Deg,
                0f,
                0f
            );
            fox.SetRotation(newRotation);
        }
    }

    private void CreateLedCube()
    {
        Vector3D roomCenter = new Vector3D(0f, 0f, 0f);
        Vector3D ledCubePosition = new Vector3D(roomCenter.x, roomCenter.y + 1f, roomCenter.z);
        ledCube = CreateObject("LED Cube", "LED Cube", ledCubePosition, new Vector3D(0f, 0f, 0f));
    }

    private void EditLedCubeProperties()
    {
        Color3D redColor = new Color3D(1f, 0f, 0f, 1f);
        ledCube.SetColor(redColor);
        ledCube.Illuminate(true);
        ledCube.SetLuminousIntensity(10f);
    }

    private void CreatePushButtonInFrontOfUser()
    {
        Vector3D userPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();
        Vector3D pushButtonPosition = new Vector3D(userPosition.x + userOrientation.x, userPosition.y, userPosition.z + userOrientation.z);
        pushButton = CreateObject("PushButton", "Push Button", pushButtonPosition, new Vector3D(0, 0, 0));
    }

    private void SetPushButtonBehaviour()
    {
        pushButton.WhenSelected(OnPushButtonSelected);
    }

    private void OnPushButtonSelected(SelectEnterEventArgs args)
    {
        foreach (Object3D fox in foxes)
        {
            fox.TriggerDanceAnimation();
        }
    }
}