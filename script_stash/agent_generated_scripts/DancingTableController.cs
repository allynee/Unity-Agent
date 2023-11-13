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
    private float rotationSpeed = 30f;

    private void Start()
    {
        FindTableInFieldOfView();
    }

    private void Update()
    {
        RotateTable();
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

    private void RotateTable()
    {
        if (userTable != null)
        {
            // Check for collision with surrounding objects or walls
            if (!IsObject3DColliding(userTable))
            {
                // Get the current rotation of the table
                Vector3D currentRotation = userTable.GetRotation();

                // Calculate the new rotation based on the rotation speed
                float rotationAmount = rotationSpeed * Time.deltaTime;
                Vector3D newRotation = new Vector3D(currentRotation.x, currentRotation.y + rotationAmount, currentRotation.z);

                // Set the new rotation for the table
                userTable.SetRotation(newRotation);
            }
            else
            {
                Debug.Log("Table is colliding with surrounding objects or walls. Rotation halted.");
            }
        }
        else
        {
            Debug.Log("Table not found in the scene.");
        }
    }

    private bool IsObject3DColliding(Object3D obj)
    {
        // Check for collision with surrounding objects or walls
        // Implementation of collision detection logic goes here
        return false; // Placeholder return value
    }
}