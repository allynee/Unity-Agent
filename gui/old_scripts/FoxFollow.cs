using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;

```csharp
public class FoxFollow : SceneAPI
{
    private Object3D foxObject;
    private GameObject foxGameObject;
    private NavMeshAgent foxNavMeshAgent;
    private Animator foxAnimator;

    private void Start()
    {
        FindFox();
        InitializeFoxComponents();
        SetFoxNavMeshAgentSpeed();
    }

    private void Update()
    {
        RotateFoxToFaceUser();
        UpdateFoxDestination();
        FoxWalkingTowardsPoint();
    }

    public void FindFox()
    {
        List<Object3D> objectsInView = GetAllObject3DsInScene();
        foxObject = objectsInView.Find(obj => obj.GetType().Equals("Fox"));

        if (foxObject != null)
        {
            Debug.Log("Found the Fox object!");
        }
        else
        {
            Debug.Log("Fox object not found in the scene.");
        }
    }

    public void InitializeFoxComponents()
    {
        if (foxObject == null)
        {
            Debug.LogError("Fox object not found in the scene!");
            return;
        }

        foxGameObject = foxObject.ToGameObject();
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

        foxAnimator = foxGameObject.GetComponent<Animator>();
        if (foxAnimator == null)
        {
            foxAnimator = foxGameObject.AddComponent<Animator>();
        }

        foxNavMeshAgent.speed = 2.0f;
        foxNavMeshAgent.angularSpeed = 120f;
        foxNavMeshAgent.acceleration = 8f;

        Debug.Log("Fox components initialized successfully!");
    }

    public void SetFoxNavMeshAgentSpeed()
    {
        if (foxNavMeshAgent != null)
        {
            foxNavMeshAgent.speed = 2.0f;
            foxNavMeshAgent.angularSpeed = 120f;
            foxNavMeshAgent.acceleration = 8f;
            Debug.Log("Fox NavMeshAgent speed, angular speed, and acceleration set to default values.");
        }
        else
        {
            Debug.LogError("Fox NavMeshAgent component not found.");
        }
    }

    public void RotateFoxToFaceUser()
    {
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D foxPosition = foxObject.GetPosition();

        Vector3 directionToUser = new Vector3(userHeadPosition.x - foxPosition.x, 0, userHeadPosition.z - foxPosition.z);

        Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);

        foxObject.SetRotation(new Vector3D(rotationToFaceUser.eulerAngles.x, rotationToFaceUser.eulerAngles.y, rotationToFaceUser.eulerAngles.z));
    }

    public void UpdateFoxDestination()
    {
        Vector3D userOrientation = GetUserOrientation();
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D destination = userHeadPosition + userOrientation * 0.5f;
        foxObject.SetPosition(destination);
        Debug.Log("Updated fox's destination to " + destination.ToString());
    }

    public void FoxWalkingTowardsPoint()
    {
        Vector3D point = new Vector3D(x, y, z); // Replace x, y, z with the coordinates of the point

        if (foxObject != null)
        {
            float distance = Vector3.Distance(foxObject.GetPosition().ToVector3(), point.ToVector3());

            if (distance > 0.7f)
            {
                foxObject.SetPosition(point);
            }
        }
    }
}