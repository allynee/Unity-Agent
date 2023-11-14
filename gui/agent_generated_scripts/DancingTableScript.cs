using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class DancingTableScript : SceneAPI
{
    // Private field to track the table
    private Object3D table;

    private void Start()
    {
        FindTableInFieldOfView();
        MoveAndSpinTable();
    }

    public void FindTableInFieldOfView()
    {
        // Get all objects in the user's field of view
        List<Object3D> objectsInView = GetAllObject3DsInFieldOfView();

        // Find the table in the user's field of view
        table = objectsInView.Find(obj => obj.GetType().Equals("Table"));

        if (table != null)
        {
            Debug.Log("Table found in the user's field of view!");
        }
        else
        {
            Debug.LogWarning("No table found in the user's field of view.");
        }
    }

    public void MoveAndSpinTable()
    {
        if (table != null)
        {
            StartCoroutine(ContinuousMovement());
        }
        else
        {
            Debug.Log("Table object not found in the scene.");
        }
    }

    private IEnumerator ContinuousMovement()
    {
        float originalY = table.GetPosition().y;
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;
            float newY = originalY + 0.5f * Mathf.Sin(time); // Moves within a range of 0.5 meters above and below its original position
            Vector3D newPosition = new Vector3D(table.GetPosition().x, newY, table.GetPosition().z);
            Vector3D newRotation = new Vector3D(0, time * 30, 0); // Spinning

            table.SetPosition(newPosition);
            table.SetRotation(newRotation);

            yield return null;
        }
    }
}