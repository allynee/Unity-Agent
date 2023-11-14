using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class LevitateBlackObjectsTask : SceneAPI
{       
    private List<Object3D> blackObjectsInFieldOfView;

    private void Start()
    {
        FindBlackObjectsInFieldOfView();
        LevitateBlackObjects();
    }

    public void FindBlackObjectsInFieldOfView()
    {
        blackObjectsInFieldOfView = new List<Object3D>();

        List<Object3D> objectsInFieldOfView = GetAllObjectsInFieldOfView();

        foreach (Object3D obj in objectsInFieldOfView)
        {
            Color3D objColor = obj.GetColor();
            if (IsBlack(objColor))
            {
                blackObjectsInFieldOfView.Add(obj);
            }
        }

        LogBlackObjects();
    }

    private List<Object3D> GetAllObjectsInFieldOfView()
    {
        return GetAllObject3DsInFieldOfView();
    }

    private bool IsBlack(Color3D color)
    {
        return color.r == 0 && color.g == 0 && color.b == 0;
    }

    private void LogBlackObjects()
    {
        if (blackObjectsInFieldOfView.Count > 0)
        {
            foreach (Object3D obj in blackObjectsInFieldOfView)
            {
                Debug.Log(obj.GetType() + " is black and in the field of view.");
            }
        }
        else
        {
            Debug.Log("No black objects found in the field of view.");
        }
    }

    public void LevitateBlackObjects()
    {
        // Levitate black objects to 2 meters above their current position
        foreach (Object3D blackObj in blackObjectsInFieldOfView)
        {
            Vector3D currentPosition = blackObj.GetPosition();
            Vector3D newPosition = new Vector3D(currentPosition.x, currentPosition.y + 2, currentPosition.z);
            blackObj.SetPosition(newPosition);
            Debug.Log("Levitated " + blackObj.GetType() + " to 2 meters above its current position.");
        }
    }
}