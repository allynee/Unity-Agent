using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ChairScript : SceneAPI
{       
    private Object3D chair;

    private void Start()
    {
        CreateChair();
        EditChairRotation();
        EditChairPosition();
    }

    public void CreateChair()
    {
        // Get the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create a new chair object at the user's feet position
        chair = CreateObject("NewChair", "Chair", userFeetPosition, new Vector3D(0, 0, 0));

        // Log the creation of the chair
        Debug.Log("Chair created");
    }

    public void EditChairRotation()
    {
        // Get the user's head position
        Vector3D userHeadPosition = GetUsersHeadPosition();

        // Check if the chair object exists
        if (chair != null)
        {
            // Get the chair's position
            Vector3D chairPosition = chair.GetPosition();

            // Calculate the direction from the chair to the user's head position
            Vector3D directionToUser = new Vector3D(userHeadPosition.x - chairPosition.x, 0, userHeadPosition.z - chairPosition.z);

            // Calculate the rotation to face the user
            Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);

            // Convert the rotation to Euler angles
            Vector3 chairRotationEuler = rotationToFaceUser.eulerAngles;

            // Set the chair's rotation to face the user
            chair.SetRotation(new Vector3D(chairRotationEuler.x, chairRotationEuler.y, chairRotationEuler.z));
        }
        else
        {
            Debug.Log("Chair object not found.");
        }
    }

    public void EditChairPosition()
    {
        // Get the user's position
        Vector3D userPosition = GetUsersFeetPosition();

        // Calculate the new position for the chair
        Vector3D chairPosition = new Vector3D(userPosition.x, userPosition.y, userPosition.z - 0.2f);

        // Check if the chair object exists
        if (chair != null)
        {
            // Set the new position for the chair
            chair.SetPosition(chairPosition);
            Debug.Log("Chair position edited successfully.");
        }
        else
        {
            Debug.LogError("Chair object not found.");
        }
    }
}