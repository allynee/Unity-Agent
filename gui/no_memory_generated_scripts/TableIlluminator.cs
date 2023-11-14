using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class TableIlluminator : SceneAPI
{
    // Class member to hold the reference to the table object
    private Object3D tableObject;

    private void Start()
    {
        FindAndIlluminateTable();
        EditTableLuminousIntensity();
    }

    private void Update()
    {
        // No methods need to be called repeatedly for this task
    }

    public void FindAndIlluminateTable()
    {
        // Find the Table object
        tableObject = FindTableObject();

        // Check if the Table object is found
        if (tableObject != null)
        {
            // Set the Illumination property of the Table to True
            tableObject.Illuminate(true);
            Debug.Log("Table illumination set to True");
        }
        else
        {
            Debug.Log("Table object not found");
        }
    }

    public void EditTableLuminousIntensity()
    {
        // Check if the Table object is found
        if (tableObject != null)
        {
            // Set the Luminous Intensity property of the Table to 10
            tableObject.SetLuminousIntensity(10f);
            Debug.Log("Table's Luminous Intensity has been set to 10.");
        }
        else
        {
            Debug.Log("Table object not found.");
        }
    }

    private Object3D FindTableObject()
    {
        // Check if the Table object type is valid
        if (IsObjectTypeValid("Table"))
        {
            // Find the Table object in the scene
            return FindObject3DByName("Table");
        }
        else
        {
            Debug.Log("Table object type is not valid.");
            return null;
        }
    }
}