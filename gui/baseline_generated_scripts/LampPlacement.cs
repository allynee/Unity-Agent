using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class LampPlacement : SceneAPI
{
    private Object3D table;
    private Object3D lamp;

    private void Start()
    {
        FindTable();
        CreateAndPlaceLamp();
    }

    private void FindTable()
    {
        // Get all objects in the scene
        List<Object3D> objectsInScene = GetAllObject3DsInScene();

        // Find the table in the scene
        table = objectsInScene.Find(obj => obj.GetType().Equals("Table"));

        if (table == null)
        {
            Debug.Log("No table found in the scene.");
        }
    }

    public void CreateAndPlaceLamp()
    {
        if (table != null)
        {
            // Create a new lamp object
            lamp = CreateObject("BlueLamp", "Lamp", table.GetPosition(), new Vector3D(0, 0, 0));

            if (lamp != null)
            {
                // Set the color of the lamp to blue
                lamp.SetColor(new Color3D(0, 0, 1, 1));

                // Increase the size of the lamp
                lamp.SetSizeByScale(2.0f);

                // Place the lamp on the table
                Vector3D lampPosition = table.GetPosition();
                lampPosition.y += table.GetSize().y;
                lamp.SetPosition(lampPosition);
            }
            else
            {
                Debug.Log("Failed to create the lamp.");
            }
        }
        else
        {
            Debug.Log("No table found in the scene. Cannot place the lamp.");
        }
    }
}