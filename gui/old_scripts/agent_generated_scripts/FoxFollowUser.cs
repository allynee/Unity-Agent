using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class FoxFollowUser : SceneAPI
{
    private Object3D foxObject3D;
    private GameObject foxGameObject;
    private NavMeshAgent navMeshAgent;
    private Animator foxAnimator;
    private Vector3D userFeetPosition3D;
    private Vector3D userOrientation3D;
    private Vector3 destination;
    private bool isFoxFar;

    private void Start()
    {
        FindOrCreateFox();
        InitializeNavMeshAgentAndAnimator();
        SetFoxNavMeshAgentProperties();
    }

    private void Update()
    {
        if (foxObject3D != null)
        {
            RotateFoxToFaceUser();
            CheckFoxDistanceAndUpdateDestination();
        }
        else
        {
            foxObject3D = FindObject3DByName("Fox");
        }
    }

    public void FindOrCreateFox()
    {
        foxObject3D = FindObject3DByName("Fox");

        if (foxObject3D == null)
        {
            Debug.Log("Fox not found in the scene.");
            Vector3D positionToCreateFox = GetUsersFeetPosition();
            foxObject3D = CreateObject("Fox", "Fox", positionToCreateFox, new Vector3D(0, 0, 0));
            Debug.Log("Fox created in the scene.");
        }
        else
        {
            Debug.Log("Fox found in the scene.");
        }
    }

    public void InitializeNavMeshAgentAndAnimator()
    {
        foxObject3D = FindObject3DByName("Fox");

        if (foxObject3D == null)
        {
            Debug.LogError("Fox not found in the scene.");
            return;
        }

        foxGameObject = foxObject3D.ToGameObject();
        navMeshAgent = foxGameObject.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            navMeshAgent = foxGameObject.AddComponent<NavMeshAgent>();
        }

        foxAnimator = foxGameObject.GetComponent<Animator>();

        if (foxAnimator == null)
        {
            foxAnimator = foxGameObject.AddComponent<Animator>();
        }

        Debug.Log("NavMeshAgent and Animator initialized for the fox.");
    }

    public void SetFoxNavMeshAgentProperties()
    {
        foxObject3D = FindObject3DByName("Fox");

        if (foxObject3D == null)
        {
            Debug.LogError("Fox object not found in the scene.");
            return;
        }

        foxGameObject = foxObject3D.ToGameObject();
        navMeshAgent = foxGameObject.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent not found on the fox GameObject.");
            return;
        }

        navMeshAgent.speed = 2.0f;
        navMeshAgent.angularSpeed = 120;
        navMeshAgent.acceleration = 8.0f;

        Debug.Log("Fox's NavMeshAgent properties have been set.");
    }

    public void RotateFoxToFaceUser()
    {
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D foxPosition = foxObject3D.GetPosition();
        Vector3 directionToUser = new Vector3(userHeadPosition.x - foxPosition.x, 0, userHeadPosition.z - foxPosition.z);
        Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);
        Quaternion correctedRotation = rotationToFaceUser * Quaternion.Euler(0, 180, 0);
        Vector3 foxRotationEuler = correctedRotation.eulerAngles;
        Vector3D foxRotation = new Vector3D(foxRotationEuler.x, foxRotationEuler.y, foxRotationEuler.z);
        foxObject3D.SetRotation(foxRotation);
    }

    public void CheckFoxDistanceAndUpdateDestination()
    {
        userFeetPosition3D = GetUsersFeetPosition();
        userOrientation3D = GetUserOrientation();
        Vector3 userFeetPosition = userFeetPosition3D.ToVector3();
        Vector3 userOrientation = userOrientation3D.ToVector3();
        destination = userFeetPosition + userOrientation * 0.5f;
        float distanceToUser = Vector3.Distance(foxObject3D.GetPosition().ToVector3(), destination);
        isFoxFar = distanceToUser > 1.5f;

        if (isFoxFar)
        {
            navMeshAgent.SetDestination(destination);
        }
    }
}