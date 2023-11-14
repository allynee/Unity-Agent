using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CreateBigPinkChair : SceneAPI
{       
    private Object3D userChair;

    private void Start()
    {
        CreateChairAtUsersFeet();
        ResizeChair();
        ChangeChairColorToPink();
        EditChairPosition();
    }

    public void CreateChairAtUsersFeet()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        userChair = CreateObject("UserChair", "Chair", userFeetPosition, new Vector3D(0, 0, 0));
        if (userChair == null)
        {
            Debug.LogError("Failed to create the chair.");
        }
    }

    public void ResizeChair()
    {
        if (userChair != null)
        {
            Vector3D currentSize = userChair.GetSize();
            Vector3D newSize = new Vector3D(currentSize.x * 2, currentSize.y * 2, currentSize.z * 2);
            userChair.SetSize(newSize);
        }
    }

    public void ChangeChairColorToPink()
    {
        if (userChair == null)
        {
            Vector3D positionToCreateChair = GetUsersFeetPosition(); 
            userChair = CreateObject("UserChair", "Chair", positionToCreateChair, new Vector3D(0, 0, 0));
        }
        userChair.SetColor(new Color3D(1, 0.5f, 0.5f, 1)); // RGBA for pink color
    }

    public void EditChairPosition()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();
        Vector3D newPosition = new Vector3D(
            userFeetPosition.x + userOrientation.x,
            userFeetPosition.y,
            userFeetPosition.z + userOrientation.z
        );
        userChair.SetPosition(newPosition);
    }
}