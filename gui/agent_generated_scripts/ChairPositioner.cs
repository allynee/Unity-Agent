using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ChairPositioner : SceneAPI
{
    private const int CHAIR_COUNT = 4;
    private List<Object3D> chairs = new List<Object3D>();
    private Object3D chair;
    private Object3D secondChair;
    private Vector3D secondWallPosition;
    private Object3D thirdChair;
    private Object3D fourthChair;

    private void Start()
    {
        CreateChairs();
        PositionFirstChairInFrontOfFirstWall();
        PositionSecondChairInFrontOfSecondWall();
        PositionThirdChairInFrontOfThirdWall();
        PositionFourthChairInFrontOfFourthWall();
    }

    public void CreateChairs()
    {
        Vector3D initialPosition = new Vector3D(0, 0, 0);

        for (int i = 0; i < CHAIR_COUNT; i++)
        {
            Object3D chair = CreateObject($"Chair_{i}", "Chair", initialPosition, new Vector3D(0, 0, 0));
            if (chair == null)
            {
                Debug.LogError($"Failed to create Chair_{i}.");
                continue;
            }
            chairs.Add(chair);
        }
    }

    public void PositionFirstChairInFrontOfFirstWall()
    {
        List<Object3D> allObjects = GetAllObject3DsInScene();

        foreach (Object3D obj in allObjects)
        {
            if (obj.GetType() == "Chair")
            {
                chair = obj;
                break;
            }
        }

        if (chair == null)
        {
            Debug.LogError("No chair found in the scene.");
            return;
        }

        Vector3D firstWallPosition = GetWallPosition(Enums.WallName.Left);

        if (firstWallPosition == null)
        {
            Debug.LogError("Failed to get position for the first wall.");
            return;
        }

        Vector3D newChairPosition = new Vector3D(firstWallPosition.x, firstWallPosition.y, firstWallPosition.z + 0.2f);

        chair.SetPosition(newChairPosition);

        Debug.Log("Position of the first chair has been updated.");
    }

    public void PositionSecondChairInFrontOfSecondWall()
    {
        secondChair = FindObject3DByName("Chair2");
        if (secondChair == null)
        {
            Debug.LogError("Failed to find the second chair in the scene.");
            return;
        }

        secondWallPosition = GetWallPosition(Enums.WallName.BackLeft);
        if (secondWallPosition == null)
        {
            Debug.LogError("Failed to get the position of the second wall.");
            return;
        }

        Vector3D newChairPosition = CalculateNewChairPosition(secondWallPosition);

        secondChair.SetPosition(newChairPosition);
    }

    private Vector3D CalculateNewChairPosition(Vector3D wallPosition)
    {
        float xOffset = 0;
        float zOffset = -0.2f;

        if (wallPosition.x > GetSceneSize().x)
            xOffset = -0.2f;
        else if (wallPosition.x < GetSceneSize().x)
            xOffset = 0.2f;

        return new Vector3D(
            wallPosition.x + xOffset,
            0,
            wallPosition.z + zOffset
        );
    }

    public void PositionThirdChairInFrontOfThirdWall()
    {
        thirdChair = FindObject3DByName("Chair3");
        if (thirdChair == null)
        {
            Debug.LogError("Failed to find the third chair in the scene.");
            return;
        }

        Vector3D thirdWallPosition = GetWallPosition(Enums.WallName.BackLeft);
        if (thirdWallPosition == null)
        {
            Debug.LogError("Failed to get position for the third wall (BackLeft).");
            return;
        }

        Vector3D newChairPosition = CalculateNewChairPosition(thirdWallPosition);

        thirdChair.SetPosition(newChairPosition);
    }

    public void PositionFourthChairInFrontOfFourthWall()
    {
        fourthChair = FindObject3DByName("Chair4");
        if (fourthChair == null)
        {
            Debug.LogError("Failed to find the fourth chair");
            return;
        }

        Vector3D fourthWallPosition = GetWallPosition(Enums.WallName.BackRight);
        if (fourthWallPosition == null)
        {
            Debug.LogError("Failed to get position for the fourth wall");
            return;
        }

        Vector3D newChairPosition = new Vector3D(
            fourthWallPosition.x,
            fourthWallPosition.y,
            fourthWallPosition.z - 0.2f
        );

        fourthChair.SetPosition(newChairPosition);
    }
}