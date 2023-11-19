using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverHandler : MonoBehaviour
{
    public Image gameOverImage;
    public TextMeshProUGUI gameOverText;
    public Player1Input player1Input;
    public Player2Input player2Input;

    void Start()
    {
        // Hide the game over UI initially
        gameOverImage.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    public void DisplayGameOver(string winner)
    {
        // Show the game over UI
        gameOverImage.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);

        // Set the winning message
        gameOverText.text = winner + " has Sacrificed 20 skulls and wins!";

        // Disable player movements
        if (player1Input != null)
            player1Input.enabled = false;

        if (player2Input != null)
            player2Input.enabled = false;
    }
}

