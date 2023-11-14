using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ChairCreator : SceneAPI
{
    private Object3D userChair;

    private void Start()
    {
        CreateChairAtUsersFeet();
    }

    public void CreateChairAtUsersFeet()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        if (userChair == null)
        {
            userChair = CreateObject("UserChair", "Chair", userFeetPosition, new Vector3D(0, 0, 0));
            if (userChair == null)
            {
                Debug.LogError("Failed to create the chair.");
            }
        }
        else
        {
            Debug.Log("Chair already exists at user's feet.");
        }
    }
}