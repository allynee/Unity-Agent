using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;

public class BigBlueChairCreator : SolutionClass
{
    private Object3D chair;

    public void CreateBigBlueChair()
    {
        SceneAPI sceneAPI = GetSceneAPI();
        Vector3D roomSize = sceneAPI.GetSceneSize();

        // Create the chair object if it doesn't exist
        if (chair == null)
        {
            chair = sceneAPI.CreateObject("BigBlueChair", "Chair", new Vector3D(roomSize.x / 2, 0, roomSize.z / 2), new Vector3D(0, 0, 0));
        }

        // Set the chair's size to be bigger
        chair.SetSize(new Vector3D(2, 2, 2));

        // Set the chair's color to be blue
        chair.SetColor(new Color3D(0, 0, 1, 1));
    }
}