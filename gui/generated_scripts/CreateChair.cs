using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;

public class CreateChair : SceneAPI
{       
    private void Start()
    {
        CreateChairObject();
    }

    public void CreateChairObject()
    {
        Debug.Log("Creating a chair object");
        Vector3D chairPosition = new Vector3D(0, 0, 0);
        Vector3D chairRotation = new Vector3D(0, 0, 0);
        Object3D chair = CreateObject("Chair", "Chair", chairPosition, chairRotation);
        Debug.Log("Chair object created");
    }
}