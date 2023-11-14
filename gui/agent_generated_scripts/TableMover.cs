using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class TableMover : SceneAPI
{
    // Private field to track the table
    private Object3D table;

    private void Start()
    {
        FindTableInFieldOfView();
        EditTablePosition();
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

    public void EditTablePosition()
    {
        if (table != null)
        {
            // Get the user's feet position
            Vector3D userFeetPosition = GetUsersFeetPosition();

            // Get the current position of the table
            Vector3D tablePosition = table.GetPosition();

            // Calculate the new position 1 meter closer to the user
            Vector3D newTablePosition = new Vector3D(
                userFeetPosition.x,
                tablePosition.y,
                userFeetPosition.z - 1.0f
            );

            // Set the new position for the table
            table.SetPosition(newTablePosition);
        }
        else
        {
            Debug.Log("Table object not found in the scene.");
        }
    }
}