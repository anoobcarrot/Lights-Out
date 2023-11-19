using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullManager : MonoBehaviour
{
    public GameObject skullPrefab;
    public int maxSkulls = 15;
    public float spawnRadius = 100f;

    private List<GameObject> spawnedSkulls = new List<GameObject>();

    void Start()
    {
        SpawnSkulls();
    }

    void SpawnSkulls()
    {
        for (int i = 0; i < maxSkulls; i++)
        {
            SpawnSkull();
        }
    }

    void SpawnSkull()
    {
        Vector3 randomPosition = GetRandomSpawnPosition();
        GameObject newSkull = Instantiate(skullPrefab, randomPosition, Quaternion.identity);
        spawnedSkulls.Add(newSkull);
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Get a random position within the larger spawn radius
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0.5f, randomCircle.y);

        // Ensure the position is within the map bounds
        return randomPosition;
    }

    public void RespawnSkull(GameObject collectedSkull)
    {
        spawnedSkulls.Remove(collectedSkull);

        Destroy(collectedSkull);

        SpawnSkull();
    }
}

