using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;

public class FoxFollowScript : SceneAPI
{
    private Object3D foxObject;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Start()
    {
        FindFox();
        InitializeFoxComponents();
        SetFoxNavMeshAgentSpeed();
        RotateFoxToFaceUser();
        UpdateFoxDestination();
        FoxWalkTowardsPoint();
    }

    public void FindFox()
    {
        List<Object3D> objectsInView = GetAllObject3DsInScene();
        foxObject = objectsInView.Find(obj => obj.GetType().Equals("Fox"));
        Debug.Log("Found the Fox object in the scene.");
    }

    public void InitializeFoxComponents()
    {
        Debug.Log("Initializing Fox components...");

        if (foxObject == null)
        {
            Debug.LogError("Fox object not found in the scene!");
            return;
        }

        navMeshAgent = foxObject.ToGameObject().GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            navMeshAgent = foxObject.ToGameObject().AddComponent<NavMeshAgent>();
        }

        animator = foxObject.ToGameObject().GetComponent<Animator>();
        if (animator == null)
        {
            animator = foxObject.ToGameObject().AddComponent<Animator>();
        }

        navMeshAgent.speed = 2.0f;
        navMeshAgent.angularSpeed = 120f;
        navMeshAgent.acceleration = 8f;

        Debug.Log("Fox components initialized successfully.");
    }

    public void SetFoxNavMeshAgentSpeed()
    {
        if (foxObject != null)
        {
            foxObject.NavMeshAgent.speed = 2.0f;
            foxObject.NavMeshAgent.angularSpeed = 120f;
            foxObject.NavMeshAgent.acceleration = 8f;

            Debug.Log("Fox NavMeshAgent speed set to 2.0 units/s");
            Debug.Log("Fox NavMeshAgent angular speed set to 120 degrees/s");
            Debug.Log("Fox NavMeshAgent acceleration set to 8 units/s^2");
        }
        else
        {
            Debug.LogError("Fox object not found in the scene");
        }
    }

    public void RotateFoxToFaceUser()
    {
        Debug.Log("Rotating the fox to face the user");

        if (foxObject != null)
        {
            Vector3D userHeadPosition = GetUsersHeadPosition();
            Vector3D foxPosition = foxObject.GetPosition();
            Vector3 directionToUser = new Vector3(userHeadPosition.x - foxPosition.x, 0, userHeadPosition.z - foxPosition.z);
            Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);
            foxObject.SetRotation(new Vector3D(rotationToFaceUser.eulerAngles.x, rotationToFaceUser.eulerAngles.y, rotationToFaceUser.eulerAngles.z));
        }
    }

    public void UpdateFoxDestination()
    {
        Debug.Log("Updating fox's destination");

        Vector3D userOrientation = GetUsersFeetPosition();
        Vector3D destination = userOrientation + (userOrientation * 0.5f);

        if (foxObject != null)
        {
            foxObject.SetPosition(destination);
        }

        Debug.Log("Fox's destination updated");
    }

    public void FoxWalkTowardsPoint()
    {
        if (foxObject != null)
        {
            Vector3D foxPosition = foxObject.GetPosition();
            Vector3D targetPoint = new Vector3D(0, 0, 0); // Replace with the actual target point
            float distanceToTarget = Vector3.Distance(foxPosition.ToVector3(), targetPoint.ToVector3());

            if (distanceToTarget > 0.7f)
            {
                Debug.Log("Fox is walking towards the target point");
                // Code to make the fox walk towards the target point
            }
            else
            {
                Debug.Log("Fox is already close to the target point");
            }
        }
    }
}