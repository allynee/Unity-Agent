using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class PinkTableCreator : SceneAPI
{       
    private Object3D newTable;
    private Object3D userTable;

    private void Start()
    {
        CreateNewTable();
        ChangeTableColorToPink();
    }

    public void CreateNewTable()
    {
        Vector3D tablePosition = new Vector3D(0, 0, 0); // Set the position for the new table
        newTable = CreateObject("NewTable", "Table", tablePosition, new Vector3D(0, 0, 0)); // Create the new table object

        if (newTable == null)
        {
            Debug.LogError("Failed to create the table."); // Log an error if the table creation fails
        }
    }

    public void ChangeTableColorToPink()
    {
        // Check if the table exists before trying to change its color
        if (userTable == null) 
        {
            Vector3D positionToCreateTable = GetUsersFeetPosition(); 
            userTable = CreateObject("UserTable", "Table", positionToCreateTable, new Vector3D(0, 0, 0));
        }

        // Set the color of the table to pink with RGBA(255,105,180,1)
        userTable.SetColor(new Color3D(255f/255f, 105f/255f, 180f/255f, 1f)); // RGBA for pink color
    }
}