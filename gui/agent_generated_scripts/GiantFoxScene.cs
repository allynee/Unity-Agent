using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class GiantFoxScene : SceneAPI
{
    private Object3D foxObject3D;

    private void Start()
    {
        CreateOrFindFox();
        EditFoxSize();
        PositionFoxInFrontOfUser();
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

    public void EditFoxSize()
    {
        if (foxObject3D == null)
        {
            Debug.LogError("Fox doesn't exist. Create it first before resizing it.");
            return;
        }

        Vector3D foxSize = foxObject3D.GetSize();
        Vector3D newFoxSize = new Vector3D(foxSize.x * 3f, foxSize.y * 3f, foxSize.z * 3f);
        foxObject3D.SetSize(newFoxSize);
        Debug.Log("The size of the fox has been updated.");
    }

    public void PositionFoxInFrontOfUser()
    {
        if (foxObject3D == null)
        {
            Vector3D positionToCreateFox = GetUsersFeetPosition();
            foxObject3D = CreateObject("UserFox", "Fox", positionToCreateFox, new Vector3D(0, 0, 0));
        }

        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();

        float defaultDistance = 1.0f; // 1 meter in front

        Vector3D newPosition = new Vector3D(
            userFeetPosition.x + userOrientation.x * defaultDistance,
            userFeetPosition.y,
            userFeetPosition.z + userOrientation.z * defaultDistance
        );

        foxObject3D.SetPosition(newPosition);
    }
}