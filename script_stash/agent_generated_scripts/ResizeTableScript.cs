using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ResizeTableScript : SceneAPI
{       
    private Object3D userTable;

    private void Start()
    {
        ResizeTable();
    }

    public void ResizeTable()
    {
        if (userTable == null)
        {
            Debug.LogError("Table doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the size of the table
        Vector3D tableSize = userTable.GetSize();

        // Define the new size of the table (assuming it needs to be smaller)
        Vector3D newTableSize = new Vector3D(tableSize.x * 0.8f, tableSize.y * 0.8f, tableSize.z * 0.8f);

        // Apply the new size to the table
        userTable.SetSize(newTableSize);
    }
}