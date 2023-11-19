using UnityEngine;

public class Skull : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            PlayerSkullHandler playerSkullHandler = other.GetComponent<PlayerSkullHandler>();
            if (playerSkullHandler != null)
            {
                if (!playerSkullHandler.IsCarryingSkull())
                {
                    playerSkullHandler.PickUpSkull(gameObject);
                    Destroy(gameObject);
                }
                // If the player is already carrying a skull, you might want to add some feedback or handle it differently.
                else
                {
                    // For example, you can play a sound, display a message, etc.
                    Debug.Log("Player is already carrying a skull.");
                }
            }
        }
    }
}

