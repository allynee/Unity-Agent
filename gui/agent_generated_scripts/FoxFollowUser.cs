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
    private Object3D foxObject3D;
    private NavMeshAgent foxNavMeshAgent;

    private void Start()
    {
        CreateOrFindFox();
        CreateNavMeshAgent();
    }

    private void Update()
    {
        if (foxObject3D != null)
        {
            RotateFoxToFaceUser();
            CheckDistanceAndMoveFox();
        }
    }

    public void CreateOrFindFox()
    {
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();
        foxObject3D = objectsInView.Find(obj => obj.GetType().Equals("Fox"));

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
            foxObject3D = CreateObject("UserFox", "Fox", positionToCreateFox, new Vector3D(0, 0, 0));
            Debug.Log("Fox created in the scene.");
        }
    }

    public void CreateNavMeshAgent()
    {
        if (foxObject3D == null)
        {
            Debug.LogError("Fox object not found.");
            return;
        }

        GameObject foxGameObject = foxObject3D.ToGameObject();
        foxNavMeshAgent = foxGameObject.GetComponent<NavMeshAgent>();

        if (foxNavMeshAgent == null)
        {
            foxNavMeshAgent = foxGameObject.AddComponent<NavMeshAgent>();
        }

        if (foxNavMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent not found or could not be added to the fox.");
            return;
        }

        foxNavMeshAgent.speed = 2.0f;
        foxNavMeshAgent.angularSpeed = 120;
        foxNavMeshAgent.acceleration = 8.0f;
    }

    public void RotateFoxToFaceUser()
    {
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D foxPosition = foxObject3D.GetPosition();

        Vector3 directionToUser = new Vector3(userHeadPosition.x - foxPosition.x, 0, userHeadPosition.z - foxPosition.z);

        Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);

        Vector3 foxRotationEuler = rotationToFaceUser.eulerAngles;
        Vector3D foxRotation = new Vector3D(foxRotationEuler.x, foxRotationEuler.y, foxRotationEuler.z);
        foxObject3D.SetRotation(foxRotation);
    }

    public void CheckDistanceAndMoveFox()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();

        Vector3 destination = userFeetPosition.ToVector3() + userOrientation.ToVector3() * 0.5f;

        float distanceToUser = Vector3.Distance(foxObject3D.GetPosition().ToVector3(), destination);

        bool isFarFromUser = distanceToUser > 1.5f;

        if (isFarFromUser)
        {
            foxNavMeshAgent.SetDestination(destination);
        }
    }
}