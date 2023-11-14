using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CreateChairScene : SceneAPI
{       
    private Object3D userChair;

    private void Start()
    {
        CreateChairAtUsersFeet();
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
        if (userChair != null)
        {
            Vector3D userFeetPosition = GetUsersFeetPosition();
            Vector3D userOrientation = GetUserOrientation();
            
            Vector3D newPosition = new Vector3D(
                userFeetPosition.x + userOrientation.x * 0.2f,
                userFeetPosition.y,
                userFeetPosition.z + userOrientation.z * 0.2f
            );
            
            userChair.SetPosition(newPosition);
        }
        else
        {
            Debug.Log("Chair object not found");
        }
    }
}