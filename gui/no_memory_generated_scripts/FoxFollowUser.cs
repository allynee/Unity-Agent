using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class FoxFollowUser : SceneAPI
{
    // Declare private fields here
    private Object3D foxObject;

    private void Start()
    {
        // No one-time initialization needed for this task
    }

    private void Update()
    {
        // Method to be called repeatedly to update the Fox's behavior
        FollowUser();
    }

    public void FollowUser()
    {
        // Find the Fox object in the scene
        if (foxObject == null)
        {
            foxObject = FindFoxObject();
            if (foxObject == null)
            {
                Debug.Log("Fox object not found in the scene.");
                return;
            }
        }

        // Get the user's feet position to make the Fox follow the user
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Set the Fox's position to the user's feet position
        foxObject.SetPosition(userFeetPosition);
    }

    private Object3D FindFoxObject()
    {
        // Check if the Fox object is in the field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();
        Object3D fox = objectsInView.Find(obj => obj.GetType().Equals("Fox"));

        if (fox == null)
        {
            Debug.Log("Fox object not found in the field of view.");
        }

        return fox;
    }
}