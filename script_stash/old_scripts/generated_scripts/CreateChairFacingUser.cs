using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CreateChairFacingUser : SceneAPI
{       
    private void Start()
    {
        CreateChairFacingUser();
    }

    public void CreateChairFacingUser()
    {
        Debug.Log("Creating a chair facing the user");
        
        // Get user's head position
        Vector3D userHeadPos = GetUsersHeadPosition();
        
        // Create a chair object
        Object3D chair = CreateObject("Chair", "Chair", userHeadPos, new Vector3D(0, 180, 0));
        
        Debug.Log("Chair created at position: " + chair.GetPosition().x + ", " + chair.GetPosition().y + ", " + chair.GetPosition().z);
        Debug.Log("Chair rotation: " + chair.GetRotation().x + ", " + chair.GetRotation().y + ", " + chair.GetRotation().z);
    }
}