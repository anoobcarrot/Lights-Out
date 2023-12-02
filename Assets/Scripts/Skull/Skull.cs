using UnityEngine;

public class Skull : MonoBehaviour
{
    public float interactRadius = 5f; // Radius within which the player can pick up the skull
    public PlayerItemHandler player1ItemHandler; // Assign in the Inspector for Player1
    public PlayerItemHandler player2ItemHandler; // Assign in the Inspector for Player2
    private bool isDropped = false;

    private void Start()
    {
        if (player1ItemHandler == null)
        {
            Debug.LogError("Player1ItemHandler not assigned in the Inspector.");
        }

        if (player2ItemHandler == null)
        {
            Debug.LogError("Player2ItemlHandler not assigned in the Inspector.");
        }
    }

    private void Update()
    {
        // Check if the player is close enough and looking at the skull
        float distanceToPlayer1 = Vector3.Distance(player1ItemHandler.transform.position, transform.position);
        float distanceToPlayer2 = Vector3.Distance(player2ItemHandler.transform.position, transform.position);

        if (distanceToPlayer1 <= interactRadius && IsPlayerLookingAtSkull(player1ItemHandler))
        {
            HandleSkullInteraction(player1ItemHandler);
        }
        else if (distanceToPlayer2 <= interactRadius && IsPlayerLookingAtSkull(player2ItemHandler))
        {
            HandleSkullInteraction(player2ItemHandler);
        }
        else
        {
            // Hide the pickup text if the player is not close or not looking at the skull
            // HidePickupText();
        }
    }

    public bool IsDropped()
    {
        return isDropped;
    }

    public void SetDropped(bool dropped)
    {
        isDropped = dropped;
    }

    private void HandleSkullInteraction(PlayerItemHandler playerItemHandler)
    {
        // Display the pickup text
        playerItemHandler.ShowPickupText($"Pick Up Skull [{playerItemHandler.GetInteractInputDisplayName()}]");

        // Check for input to pick up the skull
        if (playerItemHandler.IsInteractButtonPressed())
        {
            if (!playerItemHandler.IsCarryingSkull())
            {
                // Player is picking up the skull
                playerItemHandler.PickUpSkull(gameObject);

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

    public bool IsPlayerLookingAtSkull(PlayerItemHandler playerItemHandler)
    {
        // Increase the raycast radius to make it more lenient
        float increasedInteractRadius = interactRadius * 3f;

        // Raycast from the player's camera to check if it hits the skull
        Ray ray = new Ray(playerItemHandler.playerCamera.transform.position, playerItemHandler.playerCamera.transform.forward);
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
        player1ItemHandler.HidePickupText();
        player2ItemHandler.HidePickupText();
    }
}







