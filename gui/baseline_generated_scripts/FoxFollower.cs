using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class FoxFollower : SceneAPI
{
    private Object3D foxObject;
    private Vector3D userPosition;

    private void Start()
    {
        InitializeFox();
    }

    private void Update()
    {
        FollowUser();
    }

    public void InitializeFox()
    {
        // Find the fox object in the scene
        foxObject = FindObject3DByName("Fox");
        if (foxObject == null)
        {
            Debug.Log("Fox object not found in the scene.");
            return;
        }
    }

    public void FollowUser()
    {
        if (foxObject == null)
        {
            Debug.Log("Fox object is not initialized.");
            return;
        }

        // Get the user's feet position
        userPosition = GetUsersFeetPosition();

        // Set the fox's position to the user's position
        foxObject.SetPosition(userPosition);
    }
}