using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CreateBigBlueChairOnTable : SceneAPI
{       
    private Object3D userChair;

    private void Start()
    {
        CreateChairAtUsersFeet();
        ResizeChair();
        ChangeChairColorToBlue();
        MoveChairOnTable();
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

    public void ResizeChair()
    {
        if (userChair != null)
        {
            Vector3D currentSize = userChair.GetSize();
            Vector3D newSize = new Vector3D(currentSize.x * 2, currentSize.y * 2, currentSize.z * 2);
            userChair.SetSize(newSize);
        }
    }

    public void ChangeChairColorToBlue()
    {
        if (userChair != null)
        {
            userChair.SetColor(new Color3D(0, 0, 1, 1)); // RGBA for blue color
        }
    }

    public void MoveChairOnTable()
    {
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();
        Object3D table = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        if (table != null)
        {
            Debug.Log("Found a table in view!");
            Vector3D tablePosition = table.GetPosition();
            Vector3D tableSize = table.GetSize();
            float tableHeight = tableSize.y;
            Vector3D chairPosition = new Vector3D(tablePosition.x, tablePosition.y + tableHeight, tablePosition.z);
            userChair.SetPosition(chairPosition);
        }
        else
        {
            Debug.LogWarning("No table found in the user's field of view.");
        }
    }
}