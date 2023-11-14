using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;
using System.Linq;

public class FoxDanceParty : SceneAPI
{
    private List<Object3D> foxes = new List<Object3D>();
    private Vector3D danceFloorCenter;
    private float danceRadius = 5.0f;

    private void Start()
    {
        CreateDanceFloor();
        CreateFoxes();
    }

    private void Update()
    {
        MakeFoxesDance();
    }

    public void CreateDanceFloor()
    {
        // Assuming the dance floor is at the center of the scene
        danceFloorCenter = new Vector3D(GetSceneSize().x / 2, 0, GetSceneSize().z / 2);
    }

    public void CreateFoxes()
    {
        // Create 10 foxes around the dance floor
        for (int i = 0; i < 10; i++)
        {
            float angle = i * Mathf.PI * 2 / 10;
            Vector3D foxPosition = new Vector3D(danceFloorCenter.x + Mathf.Cos(angle) * danceRadius, 0, danceFloorCenter.z + Mathf.Sin(angle) * danceRadius);
            Object3D fox = CreateObject("Fox" + i, "Fox", foxPosition, new Vector3D(0, -angle * Mathf.Rad2Deg, 0));
            foxes.Add(fox);
        }
    }

    public void MakeFoxesDance()
    {
        // Make each fox rotate around the dance floor
        foreach (Object3D fox in foxes)
        {
            Vector3D foxPosition = fox.GetPosition();
            float angle = Mathf.Atan2(foxPosition.z - danceFloorCenter.z, foxPosition.x - danceFloorCenter.x) + Mathf.PI / 180;
            foxPosition.x = danceFloorCenter.x + Mathf.Cos(angle) * danceRadius;
            foxPosition.z = danceFloorCenter.z + Mathf.Sin(angle) * danceRadius;
            fox.SetPosition(foxPosition);
            fox.SetRotation(new Vector3D(0, -angle * Mathf.Rad2Deg, 0));
        }
    }
}