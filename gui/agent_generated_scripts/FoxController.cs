using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class FoxController : SceneAPI
{       
    private Object3D foxObject3D;

    private void Start()
    {
        CreateOrFindFox();
        RotateFoxToFaceUser();
    }

    public void CreateOrFindFox()
    {
        // Attempt to find the fox in the user's field of view first.
        List<Object3D> objectsInView = GetAllObject3DsInScene();
        foxObject3D = objectsInView.Find(obj => obj.GetType().Equals("Fox"));

        // If the fox is not found in the field of view, find it in the scene.
        if (foxObject3D == null)
        {
            Debug.Log("Fox not found in user's field of view.");
            foxObject3D = FindObject3DByName("Fox");
            Debug.Log("Fox found in the scene.");
        }

        // If there are no foxes in the scene, create one.
        if (foxObject3D == null)
        {
            Debug.Log("Fox not found in the scene.");
            Vector3D positionToCreateFox = GetUsersFeetPosition();
            foxObject3D = CreateObject("UserFox", "Fox", positionToCreateFox, new Vector3D(0, 0, 0));
            Debug.Log("Fox created in the scene.");
        }
    }

    public void RotateFoxToFaceUser()
    {
        // Get the user's head position to face the fox towards the user
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D foxPosition = foxObject3D.GetPosition();

        // Calculate the direction from the fox to the user's head position
        // Only considering the x and z components for horizontal rotation
        Vector3 directionToUser = new Vector3(userHeadPosition.x - foxPosition.x, 0, userHeadPosition.z - foxPosition.z);

        // Rotate the fox to face the user
        Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);

        // Convert the Quaternion rotation to Euler angles, then to Vector3D type for SetRotation
        Vector3 foxRotationEuler = rotationToFaceUser.eulerAngles;
        Vector3D foxRotation = new Vector3D(foxRotationEuler.x, foxRotationEuler.y, foxRotationEuler.z);
        foxObject3D.SetRotation(foxRotation);
    }
}