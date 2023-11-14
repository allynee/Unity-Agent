using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class FloatingChairManager : SceneAPI
{
    // Class member to track the created chair
    private Object3D createdChair;

    private void Start()
    {
        CreateChair();
        EditChairPosition();
        LevitateChair();
        EditChairRotationToFaceUser();
    }

    private void Update()
    {
        // No method needs to be called repeatedly for this task
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

        // Check if the Chair is already in the user's field of view
        if (objectsInView.Exists(obj => obj.GetType() == "Chair"))
        {
            Debug.Log("Chair is already in the user's field of view.");
            return;
        }

        // Get the user's feet position to create the Chair
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create the Chair in the user's field of view
        createdChair = CreateObject("Chair", "Chair", userFeetPosition, new Vector3D(0, 0, 0));

        if (createdChair != null)
        {
            Debug.Log("Chair created in the user's field of view.");
        }
        else
        {
            Debug.Log("Failed to create the Chair.");
        }
    }

    public void EditChairPosition()
    {
        // Move the chair 1m away from the user
        if(createdChair != null)
        {
            Vector3D userPosition = GetUsersFeetPosition();
            Vector3D chairNewPosition = CalculateNewChairPosition(userPosition);
            createdChair.SetPosition(chairNewPosition);
            Debug.Log("Chair position edited successfully");
        }
        else
        {
            Debug.Log("Chair object not found");
        }
    }

    private Vector3D CalculateNewChairPosition(Vector3D userPosition)
    {
        // Assuming the chair is initially at the user's feet position, we move it 1m forward in the z-axis
        return new Vector3D(userPosition.x, userPosition.y, userPosition.z + 1f);
    }

    public void LevitateChair()
    {
        if (createdChair != null)
        {
            // Set the chair's position to float 1m above the ground
            Vector3D newPosition = createdChair.GetPosition();
            newPosition.y += 1f;
            createdChair.SetPosition(newPosition);

            // Levitate the chair
            createdChair.Levitate(true);

            // Log the action
            Debug.Log("Chair levitated 1m above the ground");
        }
        else
        {
            // Log error if chair is not found
            Debug.LogError("Chair not found");
        }
    }

    public void EditChairRotationToFaceUser()
    {
        // Check if the chair object is found
        if (createdChair != null)
        {
            // Get the user's position
            Vector3D userPosition = GetUsersFeetPosition();

            // Calculate the direction from the chair to the user
            Vector3D directionToUser = new Vector3D(userPosition.x - createdChair.GetPosition().x, 0, userPosition.z - createdChair.GetPosition().z);

            // Calculate the rotation to face the user
            Vector3D newRotation = CalculateRotationToFaceUser(directionToUser);

            // Set the new rotation for the chair
            createdChair.SetRotation(newRotation);
        }
        else
        {
            Debug.Log("Chair object not found");
        }
    }

    private Vector3D CalculateRotationToFaceUser(Vector3D direction)
    {
        // Calculate the angle to face the user
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Create a new rotation based on the angle
        return new Vector3D(0, angle, 0);
    }
}