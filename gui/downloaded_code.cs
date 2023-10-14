using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;

public class TurnOffCeilingLights : SolutionClass
{
    private SceneAPI sceneAPI;
    private Object3D ceilingLight;

    public void TurnOffAllCeilingLights()
    {
        sceneAPI = GetSceneAPI();
        ceilingLight = sceneAPI.FindObject3DByName("Ceiling LED Light");

        if (ceilingLight != null)
        {
            EditCeilingLEDLightIllumination();
        }
        else
        {
            HandleCeilingLEDLightNotFound();
        }
    }

    private void EditCeilingLEDLightIllumination()
    {
        ceilingLight.Illuminate(0);
    }

    private void HandleCeilingLEDLightNotFound()
    {
        Debug.Log("Ceiling LED Light object not found in the scene.");
    }
}