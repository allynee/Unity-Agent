using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class FoxAndZombieController : SceneAPI
{       
    private Object3D foxObject3D;
    private Object3D zombieObject3D;
    private NavMeshAgent foxNavMeshAgent;
    private NavMeshAgent zombieNavMeshAgent;

    private void Start()
    {
        CreateOrFindFox();
        InitializeZombie();
    }

    private void Update()
    {
        MoveFoxToUser();
        CheckDistanceAndUpdateZombieDestination();
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

        foxNavMeshAgent = foxObject3D.ToGameObject().GetComponent<NavMeshAgent>();
        if (foxNavMeshAgent == null)
        {
            foxNavMeshAgent = foxObject3D.ToGameObject().AddComponent<NavMeshAgent>();
        }

        foxNavMeshAgent.speed = 2.0f;
        foxNavMeshAgent.angularSpeed = 120;
        foxNavMeshAgent.acceleration = 8.0f;
    }

    public void MoveFoxToUser()
    {
        Vector3D userFeetPosition3D = GetUsersFeetPosition();
        Vector3D userOrientation3D = GetUserOrientation();
        Vector3 userFeetPosition = userFeetPosition3D.ToVector3();
        Vector3 userOrientation = userOrientation3D.ToVector3();

        Vector3 destination = userFeetPosition + userOrientation * 0.5f;
        float distanceToUser = Vector3.Distance(foxObject3D.GetPosition().ToVector3(), destination);
        bool shouldMoveTowardsUser = distanceToUser > 0.7f;

        if (shouldMoveTowardsUser)
        {
            foxNavMeshAgent.SetDestination(destination);
        }
    }

    public void InitializeZombie()
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

        Vector3D foxPosition = foxObject3D.GetPosition();
        Vector3D zombiePosition = new Vector3D(foxPosition.x, foxPosition.y, foxPosition.z + 1);

        List<Object3D> zombiesInFrontOfFox = GetAllObject3DsInFieldOfView().FindAll(obj => obj.GetType().Equals("Zombie") && obj.GetPosition().Equals(zombiePosition));

        if (zombiesInFrontOfFox.Count == 0)
        {
            Debug.Log("Zombie not found in front of the Fox.");
            zombieObject3D = GetAllObject3DsInScene().Find(obj => obj.GetType().Equals("Zombie"));
            if (zombieObject3D != null)
            {
                Debug.Log("Zombie found in the scene.");
            }
        }

        if (zombieObject3D == null)
        {
            Debug.Log("Zombie not found in the scene.");
            zombieObject3D = CreateObject("UserZombie", "Zombie", zombiePosition, new Vector3D(0, 0, 0));
            Debug.Log("Zombie created in the scene.");
        }

        zombieNavMeshAgent = zombieObject3D.ToGameObject().GetComponent<NavMeshAgent>();
        if (zombieNavMeshAgent == null)
        {
            zombieNavMeshAgent = zombieObject3D.ToGameObject().AddComponent<NavMeshAgent>();
        }

        zombieNavMeshAgent.speed = 2.0f;
        zombieNavMeshAgent.angularSpeed = 120;
        zombieNavMeshAgent.acceleration = 8.0f;
    }

    public void CheckDistanceAndUpdateZombieDestination()
    {
        if (zombieObject3D != null && foxObject3D != null && zombieNavMeshAgent != null)
        {
            Vector3D zombiePosition = zombieObject3D.GetPosition();
            Vector3D foxPosition = foxObject3D.GetPosition();
            float distanceToFox = Vector3.Distance(zombiePosition.ToVector3(), foxPosition.ToVector3());

            if (distanceToFox > 1.5f)
            {
                zombieNavMeshAgent.SetDestination(foxPosition.ToVector3());
            }
        }
    }
}