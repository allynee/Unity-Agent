using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class FoxManipulation : SceneAPI
{       
    private Object3D foxObject;
    private Object3D tableObject;

    private void Start()
    {
        FindOrCreateFoxObject();
        PositionFoxOnTable();
    }

    private void FindOrCreateFoxObject()
    {
        List<Object3D> foxObjectsInView = GetAllFoxObjectsInView();
        
        if (foxObjectsInView.Count > 0)
        {
            foxObject = foxObjectsInView[0];
            Debug.Log("Found a 'Fox' object in the user's field of view.");
        }
        else
        {
            Object3D foxObjectInScene = FindObject3DByName("Fox");
            
            if (foxObjectInScene != null)
            {
                foxObject = foxObjectInScene;
                Debug.Log("Found a 'Fox' object in the scene.");
            }
            else
            {
                CreateNewFoxObject();
            }
        }
    }

    private List<Object3D> GetAllFoxObjectsInView()
    {
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();
        List<Object3D> foxObjectsInView = new List<Object3D>();
        
        foreach (Object3D obj in objectsInView)
        {
            if (obj.GetType() == "Fox")
            {
                foxObjectsInView.Add(obj);
            }
        }
        
        return foxObjectsInView;
    }

    private void CreateNewFoxObject()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D foxPosition = new Vector3D(userFeetPosition.x, userFeetPosition.y, userFeetPosition.z + 1.0f);
        
        foxObject = CreateObject("NewFox", "Fox", foxPosition, new Vector3D(0, 0, 0));
        Debug.Log("Created a new 'Fox' object.");
    }

    private void PositionFoxOnTable()
    {
        List<Object3D> allObjects = GetAllObject3DsInScene();

        foreach (Object3D obj in allObjects)
        {
            if (obj.GetType() == "Table" && IsObject3DInFieldOfView(obj))
            {
                tableObject = obj;
                break;
            }
        }

        if (tableObject != null)
        {
            foxObject = FindObject3DByName("Fox");

            if (foxObject != null)
            {
                Vector3D tablePosition = tableObject.GetPosition();
                Vector3D foxSize = foxObject.GetSize();

                foxObject.SetPosition(new Vector3D(tablePosition.x, tablePosition.y + (foxSize.y / 2), tablePosition.z));
            }
            else
            {
                Debug.Log("Fox object not found in the scene.");
            }
        }
        else
        {
            Debug.Log("No table in view.");
        }
    }
}