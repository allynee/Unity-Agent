using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;

```csharp
public class ChairManager : SceneAPI
{
    private Object3D chair;

    private void Start()
    {
        CreateChair();
        EditChairPosition();
        EditChairRotation();
        EditChairSize();
        EditChairColor();
        EditChairIllumination();
        EditChairLevitation(false);
    }

    public void CreateChair()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        chair = CreateObject("NewChair", "Chair", userFeetPosition, new Vector3D(0, 0, 0));
        Debug.Log("Chair created");
    }

    public void EditChairPosition()
    {
        Vector3D userPosition = GetUsersFeetPosition();
        Vector3D chairPosition = new Vector3D(userPosition.x, userPosition.y, userPosition.z - 0.2f);
        if (chair != null)
        {
            chair.SetPosition(chairPosition);
            Debug.Log("Chair position edited successfully.");
        }
        else
        {
            Debug.LogError("Chair object not found.");
        }
    }

    public void EditChairRotation()
    {
        Vector3D userHeadPosition = GetUsersHeadPosition();
        if (chair != null)
        {
            Vector3D chairPosition = chair.GetPosition();
            Vector3 directionToUser = new Vector3(userHeadPosition.x - chairPosition.x, 0, userHeadPosition.z - chairPosition.z);
            Quaternion rotationToFaceUser = Quaternion.LookRotation(directionToUser);
            Vector3 chairRotationEuler = rotationToFaceUser.eulerAngles;
            Vector3D chairRotation = new Vector3D(chairRotationEuler.x, chairRotationEuler.y, chairRotationEuler.z);
            chair.SetRotation(chairRotation);
        }
        else
        {
            Debug.Log("Chair object not found.");
        }
    }

    public void EditChairSize()
    {
        if (chair != null)
        {
            Vector3D userHeight = GetUsersFeetPosition() - GetUsersHeadPosition();
            float chairHeight = userHeight.y * 0.8f;
            Vector3D currentSize = chair.GetSize();
            currentSize.y = chairHeight;
            chair.SetSize(currentSize);
            Debug.Log("Chair size successfully updated.");
        }
        else
        {
            Debug.Log("Error: Chair not found in the scene.");
        }
    }

    public void EditChairColor()
    {
        if (chair != null)
        {
            Color3D desiredColor = new Color3D(0.5f, 0.2f, 0.8f, 1f);
            chair.SetColor(desiredColor);
            Debug.Log("Chair color successfully set to desired RGBA value.");
        }
        else
        {
            Debug.Log("Error: Chair not found in the scene.");
        }
    }

    public void EditChairIllumination()
    {
        if (chair != null)
        {
            chair.Illuminate(false);
            Debug.Log("Chair illumination successfully set to false.");
        }
        else
        {
            Debug.Log("Error: Chair not found in the scene.");
        }
    }

    public void EditChairLevitation(bool isLevitated)
    {
        if (chair != null)
        {
            chair.Levitate(isLevitated);
            Debug.Log("Chair levitation edited successfully.");
        }
        else
        {
            Debug.LogError("Chair not found.");
        }
    }
}