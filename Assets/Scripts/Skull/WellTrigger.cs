using UnityEngine;

public class WellTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyUp(KeyCode.G) && other.CompareTag("Player1"))
        {
            PlayerSkullHandler playerSkullHandler = other.GetComponent<PlayerSkullHandler>();
            if (playerSkullHandler != null)
            {
                playerSkullHandler.TrySacrificeSkull();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Minus) && other.CompareTag("Player2"))
        {
            PlayerSkullHandler playerSkullHandler = other.GetComponent<PlayerSkullHandler>();
            if (playerSkullHandler != null)
            {
                playerSkullHandler.TrySacrificeSkull();
            }
        }
    }
}

