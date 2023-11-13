using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;

```
public class FoxFollower : SceneAPI
{
    private Object3D foxObject;
    private NavMeshAgent foxNavMeshAgent;

    private void Start()
    {
        FindFox();
        InitializeFoxComponents();
        StartCoroutine(RotateFoxToFaceUser());
        StartCoroutine(UpdateFoxDestination());
        StartCoroutine(FoxWalkingTowardsPoint());
    }

    private void FindFox()
    {
        List<Object3D> objectsInView = GetAllObject3DsInScene();
        foxObject = objectsInView.Find(obj => obj.GetType().Equals("Fox"));

        if (foxObject != null)
        {
            Debug.Log("Found the Fox object!");
            // Perform any additional actions with the foxObject here
        }
        else
        {
            Debug.Log("Fox object not found in the scene.");
        }
    }

    private void InitializeFoxComponents()
    {
        if (foxObject == null)
        {
            Debug.LogError("Fox object not found in the scene!");
            return;
        }

        GameObject foxGameObject = foxObject.ToGameObject();
        if (foxGameObject == null)
        {
            Debug.LogError("Failed to get the GameObject for the Fox object!");
            return;
        }

        foxNavMeshAgent = foxGameObject.GetComponent<NavMeshAgent>();
        if (foxNavMeshAgent == null)
        {
            foxNavMeshAgent = foxGameObject.AddComponent<NavMeshAgent>();
        }

        Animator animator = foxGameObject.GetComponent<Animator>();
        if (animator == null)
        {
            animator = foxGameObject.AddComponent<Animator>();
        }

        // Set default values
        foxNavMeshAgent.speed = 2.0f;
        foxNavMeshAgent.angularSpeed = 120f;
        foxNavMeshAgent.acceleration = 8f;

        Debug.Log("Fox components initialized successfully!");
    }

    private IEnumerator RotateFoxToFaceUser()
    {
        // Continuously rotate the fox to face the user
        while (true)
        {
            // Get the user's head position and the fox's position
            Vector3D userHeadPosition = GetUsersHeadPosition();
            Vector3D foxPosition = foxObject.GetPosition();

            // Calculate the direction from the fox to the user
            Vector3 directionToUser = new Vector3(userHeadPosition.x - foxPosition.x, 0, userHeadPosition.z - foxPosition.z);

            // Calculate the rotation to face the user
            Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);

            // Apply the rotation to the fox
            foxObject.SetRotation(new Vector3D(rotationToFaceUser.eulerAngles.x, rotationToFaceUser.eulerAngles.y, rotationToFaceUser.eulerAngles.z));

            // Wait for the next frame
            yield return null;
        }
    }

    private IEnumerator UpdateFoxDestination()
    {
        while (true)
        {
            if (foxObject != null)
            {
                Vector3D userOrientation = GetUserOrientation();
                Vector3D userHeadPosition = GetUsersHeadPosition();
                Vector3D destination = userHeadPosition + userOrientation * 0.5f;
                foxObject.SetPosition(destination);
                Debug.Log("Updated fox's destination to " + destination.ToString());
            }
            else
            {
                Debug.LogError("Fox object not found!");
            }

            yield return null;
        }
    }

    private IEnumerator FoxWalkingTowardsPoint()
    {
        Vector3D point = new Vector3D(x, y, z); // Replace x, y, z with the coordinates of the point

        while (true)
        {
            if (foxObject != null)
            {
                float distance = Vector3.Distance(foxObject.GetPosition().ToVector3(), point.ToVector3());

                if (distance > 0.7f)
                {
                    foxObject.SetPosition(point);
                }
            }

            yield return null;
        }
    }
}