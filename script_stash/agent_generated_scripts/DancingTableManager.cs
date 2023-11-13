using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class DancingTableManager : SceneAPI
{       
    private Object3D dancingTable;

    private void Start()
    {
        CreateNewTable();
        EditTablePosition();
        LevitateAndMoveTable();
        RotateTableToFaceUser();
        StartDancingTable();
    }

    private void CreateNewTable()
    {
        Vector3D tablePosition = new Vector3D(0, 0, 0); // Set the initial position of the table
        dancingTable = CreateObject("DancingTable", "Table", tablePosition, new Vector3D(0, 0, 0)); // Create the table object

        if (dancingTable == null)
        {
            Debug.LogError("Failed to create the table."); // Log an error if the table creation fails
        }
    }

    private void EditTablePosition()
    {
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D userOrientation = GetUserOrientation();
        
        Vector3D newPosition = new Vector3D(
            userHeadPosition.x + userOrientation.x * 0.2f,
            userHeadPosition.y,
            userHeadPosition.z + userOrientation.z * 0.2f
        );
        
        dancingTable.SetPosition(newPosition);
    }

    public void LevitateAndMoveTable()
    {
        dancingTable.Levitate(true);
        Vector3D currentPosition = dancingTable.GetPosition();
        Vector3D newPosition = new Vector3D(currentPosition.x, currentPosition.y + 0.5f, currentPosition.z);
        dancingTable.SetPosition(newPosition);
    }

    private void RotateTableToFaceUser()
    {
        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D tablePosition = dancingTable.GetPosition();
        Vector3 directionToUser = new Vector3(userHeadPosition.x - tablePosition.x, 0, userHeadPosition.z - tablePosition.z);
        Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);
        Vector3 tableRotationEuler = rotationToFaceUser.eulerAngles;
        Vector3D tableRotation = new Vector3D(tableRotationEuler.x, tableRotationEuler.y, tableRotationEuler.z);
        dancingTable.SetRotation(tableRotation);
    }

    private void StartDancingTable()
    {
        StartCoroutine(DanceTable());
    }

    private IEnumerator DanceTable()
    {
        while (true)
        {
            Vector3D currentRotation = dancingTable.GetRotation();
            Vector3D newRotation = new Vector3D(0, currentRotation.y + 1, 0);
            dancingTable.SetRotation(newRotation);
            yield return new WaitForSeconds(0.1f);
        }
    }
}