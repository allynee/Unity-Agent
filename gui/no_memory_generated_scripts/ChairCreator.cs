using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class ChairCreator : SceneAPI
{
    private Object3D createdChair;
    private Object3D foundTable;

    private void Start()
    {
        CreateChair();
        EditChairSize();
        EditChairColor();
        FindTableInFieldOfView();
        PositionChairOnTable();
    }

    public void CreateChair()
    {
        if (!IsObjectTypeValid("Chair"))
        {
            Debug.Log("Chair is not a valid object type");
            return;
        }

        Vector3D userFeetPos = GetUsersFeetPosition();
        createdChair = CreateObject("Chair", "Chair", userFeetPos, new Vector3D(0, 0, 0));

        if (createdChair != null)
        {
            Debug.Log("Chair created in the user's field of view");
        }
        else
        {
            Debug.Log("Failed to create the chair");
        }
    }

    public void EditChairSize()
    {
        if (createdChair != null)
        {
            Vector3D currentSize = createdChair.GetSize();
            Vector3D newSize = new Vector3D(currentSize.x * 2, currentSize.y * 2, currentSize.z * 2);
            createdChair.SetSize(newSize);
            Debug.Log("Chair size edited successfully");
        }
        else
        {
            Debug.LogError("Chair object not found");
        }
    }

    public void EditChairColor()
    {
        if (createdChair != null)
        {
            createdChair.SetColor(new Color3D(0f, 0f, 1f, 1f));
            Debug.Log("Chair color edited successfully.");
        }
        else
        {
            Debug.Log("Chair object not found.");
        }
    }

    public void FindTableInFieldOfView()
    {
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();
        foundTable = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        if (foundTable != null)
        {
            Debug.Log("Table found in the user's field of view.");
        }
        else
        {
            Debug.Log("Table not found in the user's field of view.");
        }
    }

    public void PositionChairOnTable()
    {
        if (createdChair != null && foundTable != null)
        {
            Vector3D tablePosition = foundTable.GetPosition();
            Vector3D tableSize = foundTable.GetSize();
            Vector3D chairPosition = createdChair.GetPosition();

            chairPosition.x = tablePosition.x;
            chairPosition.y = tablePosition.y + (tableSize.y / 2);
            chairPosition.z = tablePosition.z;

            createdChair.SetPosition(chairPosition);
        }
        else
        {
            Debug.Log("Chair or Table not found in the scene");
        }
    }
}