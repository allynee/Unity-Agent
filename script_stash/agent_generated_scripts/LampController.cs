using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class LampController : SceneAPI
{       
    private Object3D userLamp;
    private Object3D rainLamp;
    private float fallSpeed = 0.1f; // meters per second

    private void Start()
    {
        CreateLamp();
        StartRainEffect();
    }

    public void CreateLamp()
    {
        // Default spawn lamp at the user's feet
        Vector3D userFeetPosition = GetUsersFeetPosition();
        
        // Create the lamp
        userLamp = CreateObject("UserLamp", "Lamp", userFeetPosition, new Vector3D(0, 0, 0));
    }

    public void StartRainEffect()
    {
        rainLamp = SceneAPI.FindObject3DByName("RainLamp");
        if (rainLamp == null)
        {
            Debug.Log("Rain lamp not found in the scene.");
            return;
        }
        StartCoroutine(SimulateRainEffect());
    }

    private IEnumerator SimulateRainEffect()
    {
        while (rainLamp.GetPosition().y > 0)
        {
            Vector3D currentPosition = rainLamp.GetPosition();
            Vector3D newPosition = new Vector3D(currentPosition.x, currentPosition.y - (fallSpeed * Time.deltaTime), currentPosition.z);
            rainLamp.SetPosition(newPosition);
            yield return null;
        }
    }
}