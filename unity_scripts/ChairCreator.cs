using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;

using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Enums;
public class ChairCreator : SceneAPI
{
    public void Start()//public override void PerformAction()
    {
        // Object creation 
        string newObjName = "Chair1";
        string prefabName = "Chair";
        Object3D newChair = CreateObject(newObjName, ObjectType.Custom, prefabName);

        // Position
        Vector3D userPosition = GetUserPosition();

        // Orientation
        Vector3D chairDirection = new Vector3D(0, 1, 0); // Assuming the chair should be upright

        // Size

        // Editing object properties
        if (newChair != null)
            {
                newChair.SetPosition(userPosition);
                newChair.SetRotation(chairDirection);
            }
        }
    
}




