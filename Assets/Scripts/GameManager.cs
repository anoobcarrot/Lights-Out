using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject enemy;
    public GameObject torch1;
    public GameObject torch2;
    public GameOverHandler gameOverHandler;

    void Update()
    {
        // Check if the torch is aimed directly at the enemy for Player 1
        CheckTorchAim(player1, torch1, enemy);

        // Check if the torch is aimed directly at the enemy for Player 2
        CheckTorchAim(player2, torch2, enemy);
    }

    void CheckTorchAim(GameObject player, GameObject torch, GameObject enemy)
    {
        if (player == null || torch == null || enemy == null)
        {
            return;
        }

        // Get the direction from the torch to the enemy
        Vector3 directionToEnemy = enemy.transform.position - torch.transform.position;

        // Get the EnemyAI component
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

        if (enemyAI == null)
        {
            Debug.LogError("EnemyAI component not found on the enemy.");
            return;
        }

        // Check if the torch light is enabled
        Light torchLight = torch.GetComponent<Light>();
        if (torchLight == null || !torchLight.enabled)
        {
            return; // Torch light is not enabled, exit the method
        }

        float increasedTeleportRadius = enemyAI.GetTeleportRadius() * 2f;
        // Check if the direction aligns with the torch's forward direction and within the teleport radius
        if (Vector3.Dot(torch.transform.forward, directionToEnemy.normalized) > 0.7f &&
    directionToEnemy.magnitude <= increasedTeleportRadius)
        {
            // Torch is aimed directly at the enemy within the increased teleport radius and the torch light is enabled
            // Trigger teleportation by setting the flag in the EnemyAI script
            enemyAI.SetTeleportFlag();
        }
    }
}



