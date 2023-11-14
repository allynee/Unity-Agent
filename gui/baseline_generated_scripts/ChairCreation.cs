using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class ChairCreation : SceneAPI
{
    private Object3D table;
    private Object3D chair;

    private void Start()
    {
        FindTable();
        CreateChair();
    }

    private void FindTable()
    {
        // Get all objects in the scene
        List<Object3D> objectsInScene = GetAllObject3DsInScene();

        // Find the table in the scene
        table = objectsInScene.Find(obj => obj.GetType().Equals("Table"));

        if (table == null)
        {
            Debug.Log("Table not found in the scene.");
        }
    }

    public void CreateChair()
    {
        if (table != null)
        {
            // Create a new chair object
            chair = CreateObject("BigBlueChair", "Chair", table.GetPosition(), new Vector3D(0, 0, 0));

            // Set the chair's color to blue
            chair.SetColor(new Color3D(0, 0, 1, 1));

            // Set the chair's size to be big
            chair.SetSizeByScale(2.0f);

            Debug.Log("Big blue chair created on the table.");
        }
        else
        {
            Debug.Log("Cannot create chair. Table not found.");
        }
    }
}