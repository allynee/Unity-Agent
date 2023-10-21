@@ -0,0 +1,53 @@
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;

public class PlaceBigBlueLampOnTable : SolutionClass 
{       
    private SceneAPI sceneAPI;
    private Object3D table;
    private Object3D lamp;
    
    public void CreateBigBlueLampOnTable()
    {
        sceneAPI = GetSceneAPI();
        
        FindTable();
        CalculateLampPosition();
        CreateLampObject();
        SetLampColor();
        SetLampSize();
    }
    
    private void FindTable()
    {
        table = sceneAPI.FindObject3DByName("Table");
    }
    
    private void CalculateLampPosition()
    {
        Vector3D lampPosition = table.GetPosition();
        lampPosition.y += table.GetSize().y / 2;
        lampPosition.z += table.GetSize().z / 2; // Place the lamp in the center of the table
        lampPosition.x += table.GetSize().x / 2; // Place the lamp on the right side of the table
    }
    
    private void CreateLampObject()
    {
        lamp = sceneAPI.CreateObject("BigBlueLamp", "Lamp", lampPosition, new Vector3D(0, 0, 0));
    }
    
    private void SetLampColor()
    {
        Color3D blueColor = new Color3D(0, 0, 1, 1);
        lamp.SetColor(blueColor);
    }
    
    private void SetLampSize()
    {
        Vector3D lampSize = new Vector3D(2, 2, 2); // Make the lamp twice as big as the default size
        lamp.SetSize(lampSize);
    }
}