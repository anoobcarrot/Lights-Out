using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    public GameObject batteryPrefab;
    public int maxBatteries = 10;
    public float spawnRadius = 100f; 

    private List<GameObject> batteries = new List<GameObject>();

    void Start()
    {
        // Initial battery spawn
        SpawnBattery();
    }

    void Update()
    {
        // Check if we need to spawn a new battery
        if (batteries.Count < maxBatteries)
        {
            SpawnBattery();
        }
    }

    void SpawnBattery()
    {
        // Random position within the larger spawn radius with Y set to 1
        Vector3 randomPosition = GetRandomPosition();

        // Instantiate a new battery at the random position
        GameObject newBattery = Instantiate(batteryPrefab, randomPosition, Quaternion.identity);

        // Add the battery to the list
        batteries.Add(newBattery);
    }

    private Vector3 GetRandomPosition()
    {
        // Get a random position within the larger spawn radius
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0.5f, randomCircle.y);

        // Ensure the position is within the map bounds
        return randomPosition;
    }

    public void RemoveBattery(GameObject battery)
    {
        // Remove the collected battery from the list
        batteries.Remove(battery);

        // Destroy the collected battery
        Destroy(battery);

        // Spawn a new battery to replace the collected one
        SpawnBattery();
    }
}

