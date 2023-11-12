using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class TableManipulator : SceneAPI
{       
    private Object3D table;

    private void Start()
    {
        FindTableInSpace();
        ResizeTableProportionally();
    }

    private void FindTableInSpace()
    {
        // Get all objects in the scene
        List<Object3D> allObjects = GetAllObject3DsInScene();

        // Find the table among all objects
        table = allObjects.Find(obj => obj.GetType().Equals("Table"));

        if (table != null)
        {
            Debug.Log("Table found in the space!");
        }
        else
        {
            Debug.LogWarning("No table found in the space.");
        }
    }

    public void ResizeTableProportionally()
    {
        if (table == null)
        {
            Debug.LogError("Table doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the current size of the table
        Vector3D tableSize = table.GetSize();

        // Calculate the new size of the table (0.8 times its current size)
        Vector3D newTableSize = new Vector3D(tableSize.x * 0.8f, tableSize.y * 0.8f, tableSize.z * 0.8f);

        // Apply the new size to the table
        table.SetSize(newTableSize);
    }
}