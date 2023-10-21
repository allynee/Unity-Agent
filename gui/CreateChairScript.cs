using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;

public class CreateChairScript : SolutionClass 
{       
    private SceneAPI sceneAPI;
    private Object3D chair;
    
    public void CreateChair()
    {
        sceneAPI = GetSceneAPI();
        Vector3D roomSize = sceneAPI.GetSceneSize();
        chair = sceneAPI.CreateObject("NewChair", "Chair", new Vector3D(roomSize.x / 2, 0, roomSize.z / 2), new Vector3D(0, 0, 0));
    }
    
    public void Method1()
    {
        // Use the chair object created in CreateChair() method
        // Insert the method code here
    }
    
    public void Method2()
    {
        // Use the chair object created in CreateChair() method
        // Insert the method code here
    }
    
    // And so on... The number of methods will depend on the user's request. 
}