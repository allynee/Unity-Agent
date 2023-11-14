using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CoolChairCreator : SceneAPI
{       
    private Object3D userChair;

    private void Start()
    {
        CreateChairAtUsersFeet();
        PositionChairInFrontofUser();
        ResizeChair();
    }

    public void CreateChairAtUsersFeet()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        userChair = FindObject3DByName("UserChair");
        if (userChair == null)
        {
            userChair = CreateObject("UserChair", "Chair", userFeetPosition, new Vector3D(0, 0, 0));
            if (userChair == null)
            {
                Debug.LogError("Failed to create the chair.");
            }
        }
    }

    public void PositionChairInFrontofUser()
    {
        if (userChair != null)
        {
            Vector3D userFeetPosition = GetUsersFeetPosition();
            Vector3D userOrientation = GetUserOrientation();
            float defaultDistance = 1.0f;
            Vector3 unityUserFeetPos = userFeetPosition.ToVector3();
            float minDistance = defaultDistance;
            var objectsInView = GetAllObject3DsInFieldOfView();

            foreach (var obj in objectsInView)
            {
                if (obj.name != "UserChair")
                {
                    Vector3 objectPosition = obj.GetPosition().ToVector3();
                    float distance = Vector3.Distance(new Vector3(objectPosition.x, unityUserFeetPos.y, objectPosition.z), unityUserFeetPos);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                }
            }

            foreach (WallName wall in Enum.GetValues(typeof(WallName)))
            {
                Vector3D wallPosition = GetWallPosition(wall);
                Vector3 unityWallPosition = wallPosition.ToVector3();
                float wallDistance = Vector3.Distance(new Vector3(unityWallPosition.x, unityUserFeetPos.y, unityWallPosition.z), unityUserFeetPos);
                if (wallDistance < minDistance)
                {
                    minDistance = wallDistance;
                }
            }

            minDistance = Mathf.Max(minDistance - 0.1f, 0.1f);
            Vector3D spawnPosition = new Vector3D(
                userFeetPosition.x + userOrientation.x * minDistance,
                userFeetPosition.y,
                userFeetPosition.z + userOrientation.z * minDistance
            );
            userChair.SetPosition(spawnPosition);
        }
    }

    public void ResizeChair()
    {
        if (userChair != null)
        {
            Vector3D currentSize = userChair.GetSize();
            Vector3D newSize = new Vector3D(currentSize.x * 1.5f, currentSize.y * 1.5f, currentSize.z * 1.5f);
            userChair.SetSize(newSize);
        }
    }
}