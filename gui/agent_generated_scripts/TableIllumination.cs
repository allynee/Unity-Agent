using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class TableIllumination : SceneAPI
{       
    // Private field to track the table
    private Object3D table;

    private void Start()
    {
        FindTableInFieldOfView();
        IlluminateTable();
        SetTableLuminousIntensity();
    }

    public void FindTableInFieldOfView()
    {
        // Get all objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Find the table in the user's field of view
        table = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        if (table != null)
        {
            Debug.Log("Table found in the user's field of view!");
        }
        else
        {
            Debug.LogWarning("No table found in the user's field of view.");
        }
    }

    public void IlluminateTable()
    {
        // Check if the table was found
        if (table == null)
        {
            Debug.LogError("Table not found in the scene.");
            return;
        }

        // Set the Illumination property of the table to true
        table.Illuminate(true);

        Debug.Log("Table is now illuminated.");
    }

    public void SetTableLuminousIntensity()
    {
        // Check if the table object exists
        if (table == null)
        {
            Debug.LogError("Table object not found in the scene.");
            return;
        }

        // Set the luminous intensity of the table to 10
        table.SetLuminousIntensity(10.0f);
    }
}