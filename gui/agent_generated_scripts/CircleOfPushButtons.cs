using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class CircleOfPushButtons : SceneAPI
{       
    // Declare the Push Button objects
    private Object3D[] pushButtons = new Object3D[8];

    private void Start()
    {
        CreatePushButtonCircle();
    }

    public void CreatePushButtonCircle()
    {
        // Get the user's feet position
        Vector3D userFeetPosition = GetUsersFeetPosition();

        // Create a circle of 8 push buttons around the user, evenly spaced 1 meter away from the user's position
        for (int i = 0; i < 8; i++)
        {
            // Calculate the position for each push button in the circle
            float angle = i * (2 * Mathf.PI / 8); // Divide the circle into 8 equal parts
            float x = userFeetPosition.x + Mathf.Cos(angle);
            float z = userFeetPosition.z + Mathf.Sin(angle);
            Vector3D pushButtonPosition = new Vector3D(x, userFeetPosition.y, z);

            // Create the push button at the calculated position
            if (pushButtons[i] == null)
            {
                pushButtons[i] = CreateObject($"PushButton{i+1}", "Push Button", pushButtonPosition, new Vector3D(0, 0, 0));
                // Log the creation of each push button
                Debug.Log($"Push Button {i+1} created at position: {pushButtonPosition.x}, {pushButtonPosition.y}, {pushButtonPosition.z}");
            }
        }
    }
}