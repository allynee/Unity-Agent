using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class TableResizer : SceneAPI
{       
    private Object3D userTable;

    private void Start()
    {
        FindTableInFieldOfView();
        ResizeTableProportionally();
    }

    private void FindTableInFieldOfView()
    {
        // Get all objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Find the table in the user's field of view
        userTable = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        if (userTable != null)
        {
            Debug.Log("Table found in the user's field of view!");
        }
        else
        {
            Debug.LogWarning("No table found in the user's field of view.");
        }
    }

    public void ResizeTableProportionally()
    {
        if (userTable == null)
        {
            Debug.LogError("Table doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the current size of the table
        Vector3D tableSize = userTable.GetSize();

        // Calculate the new size of the table (0.8 times its current size)
        Vector3D newTableSize = new Vector3D(tableSize.x * 0.8f, tableSize.y * 0.8f, tableSize.z * 0.8f);

        // Apply the new size to the table
        userTable.SetSize(newTableSize);
    }
}