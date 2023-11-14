using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class TableGlowController : SceneAPI
{
    private Object3D table;

    private void Start()
    {
        FindTable();
        MakeTableGlow();
    }

    private void FindTable()
    {
        // Get all objects in the scene
        List<Object3D> objectsInScene = GetAllObject3DsInScene();

        // Find the table object
        table = objectsInScene.Find(obj => obj.GetType().Equals("Table"));

        // If table is not found, log an error
        if (table == null)
        {
            Debug.Log("Error: Table not found in the scene.");
        }
    }

    private void MakeTableGlow()
    {
        // If table is found
        if (table != null)
        {
            // Set the table's color to white
            table.SetColor(new Color3D(1, 1, 1, 1));

            // Set the table's luminous intensity to maximum
            table.SetLuminousIntensity(1);

            // Illuminate the table
            table.Illuminate(true);

            Debug.Log("Table is now glowing as bright as possible.");
        }
    }
}