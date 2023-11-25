using UnityEngine;
using UnityEngine.UI;

public class Player1DarknessController : MonoBehaviour
{
    public Light player1Torch;
    public Image player1DarknessOverlay;
    public Color darknessColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    void Update()
    {
        bool isPlayer1TorchOn = player1Torch.enabled;

        // Check the torch state and update the darkness overlay for Player1
        if (isPlayer1TorchOn)
        {
            player1DarknessOverlay.color = Color.clear; // Fully transparent when any torch is on
        }
        else
        {
            player1DarknessOverlay.color = darknessColor; // Set darkness color when both torches are off
        }
    }
}


