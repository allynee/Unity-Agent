using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine.AI;

public class ZombieGuard : SceneAPI
{
    private const int ZOMBIE_COUNT = 4;
    private List<Object3D> zombies = new List<Object3D>();

    private void Start()
    {
        CreateZombiesAtUsersFeet();
        PositionZombiesInFrontOfWalls();
        ResizeZombie();
    }

    public void CreateZombiesAtUsersFeet()
    {
        Vector3D userFeetPosition = GetUsersFeetPosition();

        for (int i = 0; i < ZOMBIE_COUNT; i++)
        {
            Object3D zombie = CreateObject($"UserZombie_{i}", "Zombie", userFeetPosition, new Vector3D(0, 0, 0));
            zombies.Add(zombie);
        }
    }

    public void PositionZombiesInFrontOfWalls()
    {
        foreach (WallName wall in Enum.GetValues(typeof(WallName)))
        {
            Vector3D wallPosition = GetWallPosition(wall);
            if (wallPosition == null)
            {
                Debug.LogError($"Failed to get position for wall: {wall}");
                return;
            }
            Vector3D zombiePosition = CalculateZombiePositionInFrontOfWall(wallPosition);
            zombies[wall].SetPosition(zombiePosition);
        }
    }

    private Vector3D CalculateZombiePositionInFrontOfWall(Vector3D wallPosition)
    {
        Vector3D wallNormal = GetWallNormal(wallPosition);
        Vector3D offset = wallNormal * 1.0f; // 1 meter away from the wall
        return wallPosition + offset;
    }

    private Vector3D GetWallNormal(Vector3D wallPosition)
    {
        // Assuming the wall normal is pointing towards the room
        return new Vector3D(-wallPosition.x, 0, -wallPosition.z).Normalize();
    }

    private void ResizeZombie()
    {
        if (zombies.Count > 0)
        {
            foreach (Object3D zombie in zombies)
            {
                Vector3D zombieSize = zombie.GetSize();
                Vector3D newZombieSize = new Vector3D(zombieSize.x * 0.7f, zombieSize.y * 0.7f, zombieSize.z * 0.7f);
                zombie.SetSize(newZombieSize);
            }
        }
        else
        {
            Debug.LogError("No zombies created. Create zombies before resizing.");
        }
    }
}