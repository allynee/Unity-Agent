using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class TablePositionManager : SceneAPI
{
    // Private field to track the table
    private Object3D userTable;

    private void Start()
    {
        FindTableInFieldOfView();
        PositionTableCloserToUser();
    }

    public void FindTableInFieldOfView()
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

    public void PositionTableCloserToUser()
    {
        if (userTable != null)
        {
            // Get the user's feet position and orientation
            Vector3D userFeetPosition = GetUsersFeetPosition();
            Vector3D userOrientation = GetUserOrientation();

            // Define the distance to move the table closer to the user
            float distanceToMove = 0.2f;

            // Calculate the new position for the table based on the user's orientation and the distance to move
            Vector3D newPosition = new Vector3D(
                userFeetPosition.x + userOrientation.x * distanceToMove,
                userFeetPosition.y,
                userFeetPosition.z + userOrientation.z * distanceToMove
            );

            // Set the position of the table
            userTable.SetPosition(newPosition);
        }
        else
        {
            Debug.LogWarning("Table not found. Unable to position the table closer to the user.");
        }
    }
}