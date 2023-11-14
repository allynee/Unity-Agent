using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ChairFacingUser : SceneAPI
{
    private Object3D userChair;

    private void Start()
    {
        CreateChairAtUsersFeet();
        RotateChairToFaceUser();
    }

    public void CreateChairAtUsersFeet()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        userChair = FindObject3DByName("UserChair");

        if (userChair == null)
        {
            userChair = CreateObject("UserChair", "Chair", userFeetPosition, new Vector3D(0, 0, 0));
            if (userChair == null)
            {
                Debug.LogError("Failed to create the chair.");
            }
        }
    }

    public void RotateChairToFaceUser()
    {
        if (userChair == null)
        {
            Debug.LogError("User chair not found. Please create the chair first.");
            return;
        }

        // Get the user's head position to face the chair towards the user
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D chairPosition = userChair.GetPosition();

        // Calculate the direction from the chair to the user's head position
        // Only considering the x and z components for horizontal rotation
        Vector3 directionToUser = new Vector3(userHeadPosition.x - chairPosition.x, 0, userHeadPosition.z - chairPosition.z);

        // Rotate the chair to face the user, correcting for the initial chair orientation
        Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);
        Quaternion correctedRotation = Quaternion.Euler(0, -90, 0) * rotationToFaceUser; // Adjust for the chair's initial right-facing orientation

        // Convert the corrected Quaternion rotation to Euler angles, then Vector3D type for SetRotation
        Vector3 chairRotationEuler = correctedRotation.eulerAngles;
        Vector3D chairRotation = new Vector3D(chairRotationEuler.x, chairRotationEuler.y, chairRotationEuler.z);
        userChair.SetRotation(chairRotation);
    }
}