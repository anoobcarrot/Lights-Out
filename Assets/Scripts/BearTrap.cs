using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    public int damagePerTick = 10;
    public float damageInterval = 1f;
    private bool isPlayerInside = false;
    private float timer = 0f;

    private void Update()
    {
        if (isPlayerInside)
        {
            timer += Time.deltaTime;

            if (timer >= damageInterval)
            {
                // Inflict damage on the player
                InflictDamage();

                // Reset the timer
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            Debug.Log("Player entered the bear trap");
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            Debug.Log("Player exited the bear trap");
            isPlayerInside = false;
            timer = 0f; // Reset the timer when the player exits
        }
    }


    private void InflictDamage()
    {
        // Inflict damage on the player
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damagePerTick);
        }
    }
}
