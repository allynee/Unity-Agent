using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class TableDanceManager : SceneAPI
{
    // Class member to hold the reference to the table object
    private Object3D tableObject;

    private void Start()
    {
        FindAndValidateTableObject();
        if(tableObject != null){
            EnableTableLevitation();
            RotateTableContinuously();
        }
    }

    private void Update()
    {
        MoveTableInCircularPath();
    }

    private void FindAndValidateTableObject()
    {
        tableObject = FindTableObject();
        if(tableObject == null){
            Debug.Log("Table object not found or is not valid.");
        }
    }

    private Object3D FindTableObject()
    {
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();
        Object3D table = objectsInView.Find(obj => obj.GetType().Equals("Table"));
        if(table != null){
            return table;
        }
        else{
            Debug.Log("Table object not found in the field of view.");
            return null;
        }
    }

    private void EnableTableLevitation()
    {
        if (tableObject != null)
        {
            tableObject.Levitate(true);
            Debug.Log("Table levitation enabled");
        }
        else
        {
            Debug.Log("Table object not found");
        }
    }

    private void RotateTableContinuously()
    {
        StartCoroutine(RotateTableCoroutine());
    }

    private IEnumerator RotateTableCoroutine()
    {
        while(true){
            Vector3D currentRotation = tableObject.GetRotation();
            Vector3D newRotation = new Vector3D(0f, currentRotation.y + 360f, 0f);
            tableObject.SetRotation(newRotation);
            yield return new WaitForSeconds(5f);
        }
    }

    private void MoveTableInCircularPath()
    {
        if(tableObject != null){
            Vector3D originalPosition = tableObject.GetPosition();
            float time = Time.time;
            float x = originalPosition.x + 0.5f * Mathf.Cos(time * 0.1f);
            float z = originalPosition.z + 0.5f * Mathf.Sin(time * 0.1f);
            tableObject.SetPosition(new Vector3D(x, originalPosition.y, z));
        }
    }
}