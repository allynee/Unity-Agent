using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class TableCreator : SceneAPI
{       
    // Add any needed class members here
    private Object3D userTable;

    private void Start()
    {
        CreateUserTable();
    }

    public void CreateUserTable()
    {
        Vector3D tablePosition = new Vector3D(0, 0, 0); // Set the position for the table
        userTable = CreateObject("UserTable", "Table", tablePosition, new Vector3D(0, 0, 0)); // Create the table object
        if (userTable == null)
        {
            Debug.LogError("Failed to create the table.");
        }
    }
}