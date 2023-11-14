using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class TableDance : SceneAPI
{
    private Object3D table;
    private Vector3D initialPosition;
    private float danceSpeed = 0.5f;
    private float danceRadius = 0.5f;
    private float danceHeight = 0.1f;
    private float danceTime = 0;

    private void Start()
    {
        FindTable();
    }

    private void Update()
    {
        if (table != null)
        {
            DanceTable();
        }
    }

    private void FindTable()
    {
        // Get all objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Find the table among the objects
        table = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        if (table != null)
        {
            // Save the initial position of the table
            initialPosition = table.GetPosition();
        }
        else
        {
            Debug.Log("No table found in the user's field of view.");
        }
    }

    private void DanceTable()
    {
        // Calculate the new position of the table
        float x = initialPosition.x + Mathf.Sin(danceTime * danceSpeed) * danceRadius;
        float y = initialPosition.y + Mathf.Abs(Mathf.Sin(danceTime * danceSpeed)) * danceHeight;
        float z = initialPosition.z + Mathf.Cos(danceTime * danceSpeed) * danceRadius;

        // Set the new position of the table
        table.SetPosition(new Vector3D(x, y, z));

        // Update the dance time
        danceTime += Time.deltaTime;
    }
}