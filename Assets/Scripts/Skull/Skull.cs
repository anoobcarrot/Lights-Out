using UnityEngine;

public class Skull : MonoBehaviour
{
    public float interactRadius = 5f; // Radius within which the player can pick up the skull
    public PlayerSkullHandler player1SkullHandler; // Assign in the Inspector for Player1
    public PlayerSkullHandler player2SkullHandler; // Assign in the Inspector for Player2

    private void Start()
    {
        if (player1SkullHandler == null)
        {
            Debug.LogError("Player1SkullHandler not assigned in the Inspector.");
        }

        if (player2SkullHandler == null)
        {
            Debug.LogError("Player2SkullHandler not assigned in the Inspector.");
        }
    }

    private void Update()
    {
        // Check if the player is close enough and looking at the skull
        float distanceToPlayer1 = Vector3.Distance(player1SkullHandler.transform.position, transform.position);
        float distanceToPlayer2 = Vector3.Distance(player2SkullHandler.transform.position, transform.position);

        if (distanceToPlayer1 <= interactRadius && IsPlayerLookingAtSkull(player1SkullHandler))
        {
            HandleSkullInteraction(player1SkullHandler);
        }
        else if (distanceToPlayer2 <= interactRadius && IsPlayerLookingAtSkull(player2SkullHandler))
        {
            HandleSkullInteraction(player2SkullHandler);
        }
        else
        {
            // Hide the pickup text if the player is not close or not looking at the skull
            // HidePickupText();
        }
    }

    private void HandleSkullInteraction(PlayerSkullHandler playerSkullHandler)
    {
        // Display the pickup text
        playerSkullHandler.ShowPickupText($"Pick Up Skull [{playerSkullHandler.GetInteractInputDisplayName()}]");

        // Check for input to pick up the skull
        if (playerSkullHandler.IsInteractButtonPressed())
        {
            if (!playerSkullHandler.IsCarryingSkull())
            {
                // Player is picking up the skull
                playerSkullHandler.PickUpSkull(gameObject);

                // Destroy the skull object (customize this based on your game logic)
                Destroy(gameObject);
            }
            else
            {
                // Player is already carrying a skull
                Debug.Log("Player is already carrying a skull.");
            }
        }
    }

    public bool IsPlayerLookingAtSkull(PlayerSkullHandler playerSkullHandler)
    {
        // Increase the raycast radius to make it more lenient
        float increasedInteractRadius = interactRadius * 3f;

        // Raycast from the player's camera to check if it hits the skull
        Ray ray = new Ray(playerSkullHandler.playerCamera.transform.position, playerSkullHandler.playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, increasedInteractRadius))
        {
            // Check if the hit object is the skull
            if (hit.collider.CompareTag("Skull"))
            {
                return true;
            }
        }

        return false;
    }


    private void HidePickupText()
    {
        // Hide the pickup text if the player is not close to any skull
        player1SkullHandler.HidePickupText();
        player2SkullHandler.HidePickupText();
    }
}







