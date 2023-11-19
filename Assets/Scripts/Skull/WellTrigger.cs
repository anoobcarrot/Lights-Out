using UnityEngine;

public class WellTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            PlayerSkullHandler playerSkullHandler = other.GetComponent<PlayerSkullHandler>();
            if (playerSkullHandler != null)
            {
                playerSkullHandler.SetInTriggerArea(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            PlayerSkullHandler playerSkullHandler = other.GetComponent<PlayerSkullHandler>();
            if (playerSkullHandler != null)
            {
                playerSkullHandler.SetInTriggerArea(false);
            }
        }
    }
}


