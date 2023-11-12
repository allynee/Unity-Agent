using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class DancingTableScript : SceneAPI
{       
    private Object3D newTable;
    private Object3D userTable;
    private Object3D roomTable;

    private void Start()
    {
        CreateNewTable();
        RotateTableToFaceUser();
        LevitateAndMoveTable();
    }

    public void CreateNewTable()
    {
        Vector3D tablePosition = new Vector3D(0, 0, 0); 
        newTable = CreateObject("NewTable", "Table", tablePosition, new Vector3D(0, 0, 0)); 

        if (newTable == null)
        {
            Debug.LogError("Failed to create the table."); 
        }
    }

    public void RotateTableToFaceUser()
    {
        if (userTable == null)
        {
            Vector3D positionToCreateTable = GetUsersFeetPosition();
            userTable = CreateObject("UserTable", "Table", positionToCreateTable, new Vector3D(0, 0, 0));
        }

        Vector3D userHeadPosition = GetUsersHeadPosition();
        Vector3D tablePosition = userTable.GetPosition();

        Vector3 directionToUser = new Vector3(userHeadPosition.x - tablePosition.x, 0, userHeadPosition.z - tablePosition.z);

        Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);

        Vector3 tableRotationEuler = rotationToFaceUser.eulerAngles;
        Vector3D tableRotation = new Vector3D(tableRotationEuler.x, tableRotationEuler.y, tableRotationEuler.z);
        userTable.SetRotation(tableRotation);
    }

    public void LevitateAndMoveTable()
    {
        if (roomTable == null)
        {
            Vector3D tablePosition = new Vector3D(0, 0, 0);  
            Vector3D tableRotation = new Vector3D(0, 0, 0);  
            roomTable = CreateObject("RoomTable", "Table", tablePosition, tableRotation);
        }

        roomTable.Levitate(true);

        Vector3D currentPosition = roomTable.GetPosition();

        Vector3D newPosition = new Vector3D(currentPosition.x, currentPosition.y + 0.5f, currentPosition.z);
        roomTable.SetPosition(newPosition);
    }
}