using UnityEngine;
using UnityEngine.UI;

public class Player2DarknessController : MonoBehaviour
{
    public Light player2Torch;
    public Image player2DarknessOverlay;
    public Color darknessColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    void Update()
    {
        bool isPlayer2TorchOn = player2Torch.enabled;

        // Check the torch state and update the darkness overlay for Player1
        if (isPlayer2TorchOn)
        {
            player2DarknessOverlay.color = Color.clear; // Fully transparent when any torch is on
        }
        else
        {
            player2DarknessOverlay.color = darknessColor; // Set darkness color when both torches are off
        }
    }
}

