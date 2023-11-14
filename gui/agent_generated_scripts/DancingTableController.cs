using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class DancingTableController : SceneAPI
{       
    // Private field to track the table
    private Object3D dancingTable;

    private void Start()
    {
        FindTableInFieldOfView();
    }

    private void Update()
    {
        RotateTable();
    }

    public void FindTableInFieldOfView()
    {
        // Get all objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Find the table in the user's field of view
        dancingTable = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        if (dancingTable != null)
        {
            Debug.Log("Table found in the user's field of view!");
        }
        else
        {
            Debug.LogWarning("No table found in the user's field of view.");
        }
    }

    public void RotateTable()
    {
        if (dancingTable == null)
        {
            Debug.LogError("Table not found in the scene.");
            return;
        }

        // Get the current rotation of the table
        Vector3D currentRotation = dancingTable.GetRotation();

        // Calculate the new rotation around the Y-axis
        float newYRotation = currentRotation.y + 60 * Time.deltaTime;

        // Set the new rotation of the table
        dancingTable.SetRotation(new Vector3D(currentRotation.x, newYRotation, currentRotation.z));
    }
}