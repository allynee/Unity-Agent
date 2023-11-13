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
    private NavMeshAgent foxNavMeshAgent;

    private void Start()
    {
        CreateOrFindFox();
        InitializeFoxComponents();
        SetFoxNavMeshAgentProperties();
        RotateFoxToFaceUser();
    }

    private void Update()
    {
        UpdateFoxDestination();
    }

    public void CreateOrFindFox()
    {
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();
        foxObject3D = objectsInView.Find(obj => obj.GetType().Equals("Fox"));
        Debug.Log("Fox found in user's field of view");

        if (foxObject3D == null)
        {
            Debug.Log("Fox not found in user's field of view.");
            foxObject3D = FindObject3DByName("Fox");
            Debug.Log("Fox found in the scene.");
        }

        if (foxObject3D == null)
        {
            Debug.Log("Fox not found in the scene.");
            Vector3D positionToCreateFox = GetUsersFeetPosition();
            foxObject3D = CreateObject("Fox", "Fox", positionToCreateFox, new Vector3D(0, 0, 0));
            Debug.Log("Fox created in the scene.");
        }
    }

    public void InitializeFoxComponents()
    {
        foxObject3D = FindObject3DByName("Fox");

        if (foxObject3D == null)
        {
            Vector3D positionToCreateFox = GetUsersFeetPosition();
            foxObject3D = CreateObject("Fox", "Fox", positionToCreateFox, new Vector3D(0, 0, 0));
        }

        GameObject foxGameObject = foxObject3D.ToGameObject();

        foxNavMeshAgent = foxGameObject.GetComponent<NavMeshAgent>();
        if (foxNavMeshAgent == null)
        {
            foxNavMeshAgent = foxGameObject.AddComponent<NavMeshAgent>();
        }

        foxNavMeshAgent.speed = 2.0f;
        foxNavMeshAgent.angularSpeed = 120.0f;
        foxNavMeshAgent.acceleration = 8.0f;
    }

    public void SetFoxNavMeshAgentProperties()
    {
        foxObject3D = FindObject3DByName("Fox");

        if (foxObject3D != null)
        {
            GameObject foxGameObject = foxObject3D.ToGameObject();

            foxNavMeshAgent = foxGameObject.GetComponent<NavMeshAgent>();
            if (foxNavMeshAgent == null)
            {
                foxNavMeshAgent = foxGameObject.AddComponent<NavMeshAgent>();
            }

            foxNavMeshAgent.speed = 2.0f;
            foxNavMeshAgent.angularSpeed = 120.0f;
            foxNavMeshAgent.acceleration = 8.0f;

            Debug.Log("Fox NavMeshAgent properties set successfully.");
        }
        else
        {
            Debug.LogError("Fox object not found in the scene.");
        }
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

    public void UpdateFoxDestination()
    {
        if (foxObject3D == null)
        {
            foxObject3D = FindObject3DByName("Fox");
            if (foxObject3D == null)
            {
                Debug.Log("Fox object not found.");
                return;
            }
        }

        if (foxNavMeshAgent == null)
        {
            foxNavMeshAgent = foxObject3D.ToGameObject().GetComponent<NavMeshAgent>();
            if (foxNavMeshAgent == null)
            {
                Debug.Log("NavMeshAgent component not found on the Fox object.");
                return;
            }
        }

        Vector3D userPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();
        Vector3D destination = userPosition + userOrientation * 0.5f;

        float distanceToUser = Vector3.Distance(foxObject3D.GetPosition().ToVector3(), destination.ToVector3());

        if (distanceToUser > 1.5f)
        {
            foxNavMeshAgent.SetDestination(destination.ToVector3());
        }
    }
}