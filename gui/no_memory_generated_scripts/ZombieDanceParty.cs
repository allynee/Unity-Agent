using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class ZombieDanceParty : SceneAPI
{
    private List<Object3D> zombies = new List<Object3D>();
    private Object3D ledCube;
    private Object3D pushButton;

    private void Start()
    {
        CreateZombies();
        PositionZombiesInCircle();
        RotateZombiesToFaceCenter();
        CreateLEDCube();
        SetLEDCubeColor();
        IlluminateLEDCube();
        SetLEDCubeIntensity();
        CreatePushButton();
        SetPushButtonBehavior();
        LevitateZombies();
    }

    private void CreateZombies()
    {
        Vector3D centerPosition = new Vector3D(0f, 0f, 0f);
        for (int i = 0; i < 5; i++)
        {
            if (IsZombieTypeValid())
            {
                Object3D zombie = CreateZombieObject(centerPosition);
                zombies.Add(zombie);
            }
            else
            {
                Debug.Log("Zombie type is not valid.");
            }
        }
    }

    private bool IsZombieTypeValid()
    {
        return IsObjectTypeValid("Zombie");
    }

    private Object3D CreateZombieObject(Vector3D position)
    {
        return CreateObject("Zombie", "Zombie", position, new Vector3D(0f, 0f, 0f));
    }

    private void PositionZombiesInCircle()
    {
        int totalZombies = zombies.Count;
        float angleDifference = 360f / totalZombies;
        Vector3D centerPosition = new Vector3D(0f, 0f, 0f);

        for (int i = 0; i < totalZombies; i++)
        {
            float angle = i * angleDifference;
            float x = centerPosition.x + 2f * Mathf.Cos(Mathf.Deg2Rad * angle);
            float z = centerPosition.z + 2f * Mathf.Sin(Mathf.Deg2Rad * angle);
            Vector3D newPosition = new Vector3D(x, centerPosition.y, z);
            zombies[i].SetPosition(newPosition);
        }
    }

    private void RotateZombiesToFaceCenter()
    {
        Vector3D center = new Vector3D(0f, 0f, 0f);

        foreach (Object3D zombie in zombies)
        {
            Vector3D zombiePosition = zombie.GetPosition();
            Vector3D directionToCenter = center - zombiePosition;
            Vector3D newRotation = CalculateRotationToFaceDirection(directionToCenter);
            zombie.SetRotation(newRotation);
        }

        Debug.Log("Zombies' rotation edited to face the center of the circle");
    }

    private Vector3D CalculateRotationToFaceDirection(Vector3D direction)
    {
        return new Vector3D(0f, 0f, 0f);
    }

    private void CreateLEDCube()
    {
        Vector3D centerPosition = CalculateCenterOfZombiesCircle();
        ledCube = CreateLEDAtPosition(centerPosition);
    }

    private Vector3D CalculateCenterOfZombiesCircle()
    {
        Vector3D sumPosition = new Vector3D(0, 0, 0);
        foreach (Object3D zombie in zombies)
        {
            sumPosition.x += zombie.GetPosition().x;
            sumPosition.y += zombie.GetPosition().y;
            sumPosition.z += zombie.GetPosition().z;
        }
        int count = zombies.Count;
        sumPosition.x /= count;
        sumPosition.y /= count;
        sumPosition.z /= count;

        return sumPosition;
    }

    private Object3D CreateLEDAtPosition(Vector3D position)
    {
        if (IsObjectTypeValid("LED Cube"))
        {
            return CreateObject("LED Cube", "LED Cube", position, new Vector3D(0, 0, 0));
        }
        else
        {
            Debug.Log("LED Cube type is not valid");
            return null;
        }
    }

    private void SetLEDCubeColor()
    {
        if (ledCube != null)
        {
            Color3D redColor = new Color3D(1f, 0f, 0f, 1f);
            ledCube.SetColor(redColor);
            Debug.Log("LED Cube color set to red.");
        }
        else
        {
            Debug.Log("LED Cube not found. Color not set.");
        }
    }

    private void IlluminateLEDCube()
    {
        if (ledCube != null)
        {
            ledCube.Illuminate(true);
            Debug.Log("LED Cube illumination set to true.");
        }
        else
        {
            Debug.Log("LED Cube not found in the scene.");
        }
    }

    private void SetLEDCubeIntensity()
    {
        if (ledCube != null)
        {
            ledCube.SetLuminousIntensity(10f);
            Debug.Log("LED Cube luminous intensity set to 10 for maximum brightness");
        }
        else
        {
            Debug.Log("LED Cube not found");
        }
    }

    private void CreatePushButton()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();
        Vector3D pushButtonPosition = new Vector3D(userFeetPosition.x, userFeetPosition.y, userFeetPosition.z + 1);
        pushButton = CreateObject("PushButton", "Push Button", pushButtonPosition, new Vector3D(0, 0, 0));
    }

    private void SetPushButtonBehavior()
    {
        if (pushButton != null && zombies.Count > 0)
        {
            pushButton.WhenSelected(OnButtonPressed);
        }
        else
        {
            Debug.Log("Push button or zombies not found");
        }
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        foreach (Object3D zombie in zombies)
        {
            zombie.TriggerDanceAnimation();
        }
    }

    private void LevitateZombies()
    {
        if (zombies.Count > 0)
        {
            foreach (var zombie in zombies)
            {
                zombie.Levitate(true);
            }
        }
        else
        {
            Debug.Log("No Zombie objects found in the scene.");
        }
    }
}