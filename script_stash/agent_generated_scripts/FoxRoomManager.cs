using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class FoxRoomManager : SceneAPI
{       
    private Object3D foxObject3D;
    private List<Object3D> foxObjects = new List<Object3D>();

    private void Start()
    {
        CreateOrFindFox();
        InstantiateFoxes();
    }

    private void CreateOrFindFox()
    {
        // Attempt to find the fox in front of the user first.
        List<Object3D> objectsInFieldOfView = GetAllObject3DsInFieldOfView();
        foxObject3D = objectsInFieldOfView.Find(obj => obj.GetType().Equals("Fox"));

        // If the fox is not found in the field of view, find it in the scene.
        if (foxObject3D == null)
        {
            Debug.Log("Fox not found in front of the user.");
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

    private void InstantiateFoxes()
    {
        int numberOfFoxes = 20; // Number of foxes to be instantiated
        Vector3D roomSize = GetSceneSize();
        float spacing = 2.0f; // Minimum spacing between foxes

        for (int i = 0; i < numberOfFoxes; i++)
        {
            Vector3D randomPosition = GetRandomPositionInRoom(roomSize);
            Object3D newFox = CreateFoxAtPosition(randomPosition, spacing);
            if (newFox != null)
            {
                foxObjects.Add(newFox);
            }
        }
    }

    private Vector3D GetRandomPositionInRoom(Vector3D roomSize)
    {
        float x = UnityEngine.Random.Range(-roomSize.x / 2, roomSize.x / 2);
        float y = UnityEngine.Random.Range(0, roomSize.y); // Assuming y is the vertical axis
        float z = UnityEngine.Random.Range(-roomSize.z / 2, roomSize.z / 2);
        return new Vector3D(x, y, z);
    }

    private Object3D CreateFoxAtPosition(Vector3D position, float spacing)
    {
        Object3D newFox = CreateObject("Fox", "Fox", position, new Vector3D(0, 0, 0));
        if (newFox != null)
        {
            foreach (Object3D fox in foxObjects)
            {
                if (Vector3D.Distance(fox.GetPosition(), newFox.GetPosition()) < spacing)
                {
                    // If the new fox is too close to an existing fox, destroy it and return null
                    DestroyFox(newFox);
                    return null;
                }
            }
        }
        return newFox;
    }

    private void DestroyFox(Object3D fox)
    {
        // Code to destroy the fox object
    }
}