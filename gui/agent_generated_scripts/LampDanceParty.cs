using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class LampDanceParty : SceneAPI
{
    // Declare private fields to track the array of lamps and the time since last color change
    private List<Object3D> lamps = new List<Object3D>();
    private float colorChangeInterval = 0.5f;
    private float timeSinceLastColorChange = 0.0f;

    private void Start()
    {
        // Find and store all lamps in the scene
        lamps = GetAllLampsInScene();
    }

    private void Update()
    {
        // If method(s) that need to be called repeatedly
        RotateLampsAroundYAxis();
        EditLampsColor();
    }

    public void RotateLampsAroundYAxis()
    {
        foreach (Object3D lamp in lamps)
        {
            // Get the current rotation of the lamp
            Vector3D currentRotation = lamp.GetRotation();

            // Calculate the new rotation around the Y-axis
            float newYRotation = currentRotation.y + 180 * Time.deltaTime;

            // Set the new rotation of the lamp
            lamp.SetRotation(new Vector3D(currentRotation.x, newYRotation, currentRotation.z));
        }
    }

    public void EditLampsColor()
    {
        timeSinceLastColorChange += Time.deltaTime;

        if (timeSinceLastColorChange >= colorChangeInterval)
        {
            timeSinceLastColorChange = 0;

            // Change the color of each lamp
            foreach (Object3D lamp in lamps)
            {
                // Generate a new random RGBA color
                Color3D newColor = new Color3D(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                lamp.SetColor(newColor);
            }
        }
    }

    private List<Object3D> GetAllLampsInScene()
    {
        List<Object3D> allObjects = GetAllObject3DsInScene();
        List<Object3D> lamps = new List<Object3D>();

        foreach (Object3D obj in allObjects)
        {
            if (obj.GetType() == "Lamp")
            {
                lamps.Add(obj);
            }
        }

        return lamps;
    }
}