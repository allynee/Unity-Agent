using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class TableManager : SceneAPI
{       
    private Object3D userTable;

    private void Start()
    {
        FindOrCreateTable();
        ResizeUserTable();
    }

    public void FindOrCreateTable()
    {
        // Attempt to find the table in the user's field of view first.
        List<Object3D> objectsInView = GetAllObject3DsInScene();
        userTable = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        // If the table is not found in the field of view, find it in the scene.
        if (userTable == null)
        {
            Debug.Log("Table not found in user's field of view.");
            userTable = FindObject3DByName("Table");
            Debug.Log("Table found in the scene.");
        }

        // If there are no tables in the scene, create one.
        if (userTable == null)
        {
            Debug.Log("Table not found in the scene.");
            Vector3D positionToCreateTable = GetUsersFeetPosition();
            userTable = CreateObject("UserTable", "Table", positionToCreateTable, new Vector3D(0, 0, 0));
            Debug.Log("Table created in the scene.");
        }
    }

    public void ResizeUserTable()
    {
        if (userTable == null)
        {
            Debug.LogError("UserTable doesn't exist. Create it first before resizing it.");
            return;
        }

        // Get the current size of the table
        Vector3D tableSize = userTable.GetSize();

        // Calculate the new size of the table (0.5 times of its current size)
        Vector3D newTableSize = new Vector3D(tableSize.x * 0.5f, tableSize.y * 0.5f, tableSize.z * 0.5f);

        // Apply the new size to the table
        userTable.SetSize(newTableSize);
    }
}