using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class FoxFollower : SceneAPI
{
    private Object3D foxObject3D;
    private NavMeshAgent foxNavMeshAgent;

    private void Start()
    {
        FindOrCreateFox();
        InitializeFoxComponents();
        SetFoxNavMeshAgentProperties();
        RotateFoxToFaceUser();
        StartCoroutine(CheckFoxDistance());
    }

    private void FindOrCreateFox()
    {
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();
        foxObject3D = objectsInView.Find(obj => obj.GetType() == "Fox");

        if (foxObject3D == null)
        {
            foxObject3D = FindObject3DByName("Fox");
        }

        if (foxObject3D == null)
        {
            Vector3D positionToCreateFox = GetUsersFeetPosition();
            foxObject3D = CreateObject("Fox", "Fox", positionToCreateFox, new Vector3D(0, 0, 0));
        }
    }

    private void InitializeFoxComponents()
    {
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

        Animator foxAnimator = foxGameObject.GetComponent<Animator>();

        if (foxNavMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent not found or could not be added to the fox.");
            return;
        }

        foxNavMeshAgent.speed = 2.0f;
        foxNavMeshAgent.angularSpeed = 120;
        foxNavMeshAgent.acceleration = 8;
    }