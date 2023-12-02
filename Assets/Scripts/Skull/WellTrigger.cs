using UnityEngine;

public class WellTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            PlayerItemHandler playerItemHandler = other.GetComponent<PlayerItemHandler>();
            if (playerItemHandler != null)
            {
                playerItemHandler.SetInTriggerArea(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            PlayerItemHandler playerItemHandler = other.GetComponent<PlayerItemHandler>();
            if (playerItemHandler != null)
            {
                playerItemHandler.SetInTriggerArea(false);
            }
        }
    }
}


