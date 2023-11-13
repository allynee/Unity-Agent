using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class DancingTableScript : SceneAPI
{
    private Object3D userTable;
    private float bounceHeight = 0.5f;
    private float bounceFrequency = 1.5f;
    private float timeElapsed = 0f;

    private void Start()
    {
        FindTableInFieldOfView();
        StartTableBouncing();
    }

    private void Update()
    {
        StartTableBouncing();
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

    private void StartTableBouncing()
    {
        if (userTable == null)
        {
            Debug.Log("Table object not found. Creating a new table object.");
            Vector3D tablePosition = new Vector3D(0, 0, 0);  // Set the initial position of the table
            userTable = CreateObject("TableObject", "Table", tablePosition, new Vector3D(0, 0, 0));
        }

        // Continuously update the rotation to make the table bounce
        timeElapsed += Time.deltaTime;
        float bounceAmplitude = Mathf.Sin(2 * Mathf.PI * bounceFrequency * timeElapsed) * bounceHeight;
        Vector3D currentRotation = userTable.GetRotation();
        userTable.SetRotation(new Vector3D(bounceAmplitude, currentRotation.y, currentRotation.z));
    }
}