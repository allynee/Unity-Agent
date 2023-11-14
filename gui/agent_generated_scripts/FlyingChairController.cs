using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class FlyingChairController : SceneAPI
{       
    private Object3D chairObject;

    private void Start()
    {
        CreateChairInFieldOfView();
    }

    private void Update()
    {
        MoveChairTowardsUser();
    }

    public void CreateChairInFieldOfView()
    {
        if (!IsChairTypeValid())
        {
            Debug.Log("Chair type is not valid.");
            return;
        }

        Vector3D userFeetPosition = GetUsersFeetPosition();
        chairObject = CreateObjectInFieldOfView("Chair", userFeetPosition);
        if (chairObject != null)
        {
            Debug.Log("Chair created in the user's field of view.");
        }
        else
        {
            Debug.Log("Failed to create chair in the user's field of view.");
        }
    }

    private bool IsChairTypeValid()
    {
        List<string> validObjectTypes = GetAllValidObjectTypes();
        return validObjectTypes.Contains("Chair");
    }

    private Object3D CreateObjectInFieldOfView(string objectType, Vector3D position)
    {
        if (!IsObjectTypeValid(objectType))
        {
            Debug.Log("Invalid object type: " + objectType);
            return null;
        }

        return CreateObject("NewObject", objectType, position, new Vector3D(0, 0, 0));
    }

    public void MoveChairTowardsUser()
    {
        if (chairObject != null)
        {
            Vector3D userPosition = GetUsersFeetPosition();
            Vector3D chairPosition = chairObject.GetPosition();
            Vector3D direction = new Vector3D(userPosition.x - chairPosition.x, userPosition.y - chairPosition.y, userPosition.z - chairPosition.z);
            float distance = direction.ToVector3().magnitude;
            float speed = 0.5f; // meters per second
            float step = speed * Time.deltaTime;
            if (distance > step)
            {
                direction.x /= distance;
                direction.y /= distance;
                direction.z /= distance;
                Vector3D newPosition = new Vector3D(chairPosition.x + direction.x * step, chairPosition.y + direction.y * step, chairPosition.z + direction.z * step);
                chairObject.SetPosition(newPosition);
            }
            else
            {
                Debug.Log("Chair has reached the user's position");
            }
        }
        else
        {
            Debug.Log("Chair object is null");
        }
    }
}