using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;

public class CreateLampScript : SceneAPI
{
    private Object3D lamp;

    private void Start()
    {
        CreateLamp();
        // Call other methods here
    }

    public void CreateLamp()
    {
        Vector3D userPosition = GetUsersHeadPosition();
        lamp = CreateObject("Lamp", "Lamp", userPosition, new Vector3D(0, 0, 0));
    }

    // Add other methods here
}