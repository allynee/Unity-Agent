using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class FloatingChairManager : SceneAPI
{
    private Object3D userChair;

    private void Start()
    {
        CreateChairAtUsersFeet();
        PositionChairInFrontofUser();
        MakeChairFloat();
        EditChairPosition();
        RotateChairToFaceUser();
    }

    public void CreateChairAtUsersFeet()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        userChair = CreateObject("UserChair", "Chair", userFeetPosition, new Vector3D(0, 0, 0));
        if (userChair == null)
        {
            Debug.LogError("Failed to create the chair.");
        }
    }

    public void PositionChairInFrontofUser()
    {
        if (userChair == null)
        {
            Debug.LogError("Chair object not found.");
            return;
        }

        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D userOrientation = GetUserOrientation();
        float defaultDistance = 2.0f;
        Vector3 unityUserFeetPos = userFeetPosition.ToVector3();
        float minDistance = defaultDistance;
        var objectsInView = GetAllObject3DsInFieldOfView();

        foreach (var obj in objectsInView)
        {
            if (obj.GetType() == "Chair")
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

    public void MakeChairFloat()
    {
        if (userChair == null)
        {
            Debug.LogError("Chair object not found.");
            return;
        }

        if (userChair.IsLevitated())
        {
            Debug.Log("Chair is already levitating.");
            return;
        }

        userChair.Levitate(true);
        Debug.Log("Chair is now levitating.");
    }

    public void EditChairPosition()
    {
        if (userChair != null)
        {
            Vector3D chairPosition = userChair.GetPosition();
            userChair.SetPosition(new Vector3D(chairPosition.x, chairPosition.y + 1.5f, chairPosition.z));
        }
    }

    public void RotateChairToFaceUser()
    {
        if (userChair == null)
        {
            Debug.LogError("Chair object not found.");
            return;
        }

        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D chairPosition = userChair.GetPosition();
        Vector3 directionToUser = new Vector3(userHeadPosition.x - chairPosition.x, 0, userHeadPosition.z - chairPosition.z);
        Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);
        Quaternion correctedRotation = Quaternion.Euler(0, -90, 0) * rotationToFaceUser;
        Vector3 chairRotationEuler = correctedRotation.eulerAngles;
        Vector3D chairRotation = new Vector3D(chairRotationEuler.x, chairRotationEuler.y, chairRotationEuler.z);
        userChair.SetRotation(chairRotation);
    }
}