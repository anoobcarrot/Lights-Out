using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitManager : MonoBehaviour
{
    public GameObject medkitPrefab;
    public int maxMedkits = 10;
    public float spawnRadius = 100f; 

    private List<GameObject> medkits = new List<GameObject>();

    void Start()
    {
        // Initial medkit spawn
        SpawnMedkit();
    }

    void Update()
    {
        // Check if we need to spawn a new medkit
        if (medkits.Count < maxMedkits)
        {
            SpawnMedkit();
        }
    }

    void SpawnMedkit()
    {
        // Random position within the larger spawn radius with Y set to 1
        Vector3 randomPosition = GetRandomPosition();

        // Instantiate a new medkit at the random position
        GameObject newMedkit = Instantiate(medkitPrefab, randomPosition, Quaternion.identity);

        // Add the medkit to the list
        medkits.Add(newMedkit);
    }

    private Vector3 GetRandomPosition()
    {
        // Get a random position within the larger spawn radius
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0.5f, randomCircle.y);

        // Ensure the position is within the map bounds
        return randomPosition;
    }

    public void RemoveMedkit(GameObject medkit)
    {
        // Remove the collected battery from the list
        medkits.Remove(medkit);

        // Destroy the collected battery
        Destroy(medkit);

        // Spawn a new battery to replace the collected one
        SpawnMedkit();
    }
}
