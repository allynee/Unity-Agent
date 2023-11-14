using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class ChairManager : SceneAPI
{
    // Class member to track the chair object
    private Object3D chairObject;

    private void Start()
    {
        CreateChair();
        EditChairSize();
        EditChairColor();
        EditChairPosition();
    }

    private void Update()
    {
        // No update needed for this task
    }

    public void CreateChair()
    {
        // Check if Chair is a valid object type
        if (!IsObjectTypeValid("Chair"))
        {
            Debug.Log("Chair is not a valid object type.");
            return;
        }

        // Get the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Check if Chair is already in the user's field of view
        if (objectsInView.Exists(obj => obj.GetType() == "Chair"))
        {
            Debug.Log("Chair is already in the user's field of view.");
            return;
        }

        // Create a Chair in the user's field of view
        Vector3D userFeetPos = GetUsersFeetPosition();
        Vector3D chairPosition = new Vector3D(userFeetPos.x, userFeetPos.y, userFeetPos.z + 2f); // Place the chair 2 units in front of the user
        chairObject = CreateObject("Chair", "Chair", chairPosition, new Vector3D(0f, 0f, 0f));
        if (chairObject != null)
        {
            Debug.Log("Chair created in the user's field of view.");
        }
        else
        {
            Debug.Log("Failed to create Chair in the user's field of view.");
        }
    }

    public void EditChairSize()
    {
        // Check if the chair object is found
        if (chairObject != null)
        {
            // Get the current size of the chair
            Vector3D currentSize = chairObject.GetSize();

            // Edit the Size property of the Chair to be 2 times its current size
            Vector3D newSize = new Vector3D(currentSize.x * 2, currentSize.y * 2, currentSize.z * 2);
            chairObject.SetSize(newSize);

            // Log the action
            Debug.Log("Chair size edited successfully");
        }
        else
        {
            // Log an error if the chair object is not found
            Debug.LogError("Chair object not found");
        }
    }

    public void EditChairColor()
    {
        if (chairObject != null)
        {
            chairObject.SetColor(new Color3D(1f, 0.41f, 0.71f, 1f)); // RGBA(255, 105, 180, 1)
            Debug.Log("Chair color edited successfully");
        }
        else
        {
            Debug.Log("Chair not found");
        }
    }

    public void EditChairPosition()
    {
        // Check if the chair object is found
        if (chairObject != null)
        {
            // Get the user's position
            Vector3D userPosition = GetUsersFeetPosition();

            // Calculate the position 1m away from the user
            Vector3D newChairPosition = CalculatePositionInFrontOfUser(userPosition);

            // Set the new position for the chair
            chairObject.SetPosition(newChairPosition);
        }
        else
        {
            Debug.Log("Chair object not found");
        }
    }

    private Vector3D GetUsersFeetPosition()
    {
        // Get the user's feet position
        return GetUsersFeetPosition();
    }

    private Vector3D CalculatePositionInFrontOfUser(Vector3D userPosition)
    {
        // Calculate the position 1m away from the user
        Vector3D direction = new Vector3D(0, 0, 1); // Assuming the user is facing the positive z-axis
        Vector3D newChairPosition = new Vector3D(userPosition.x + direction.x, userPosition.y, userPosition.z + direction.z);
        return newChairPosition;
    }
}