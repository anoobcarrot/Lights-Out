using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isGameOver = false;

    public TextMeshProUGUI healthText;
    public Image healthBar;

    public GameObject gameOverUI;
    public TextMeshProUGUI gameOverText;

    public Player1Input player1Input; // Replace with your Player1Input script
    public Player2Input player2Input; // Replace with your Player2Input script

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (isGameOver)
        {
            // Disable player controls or any other game-related activities
            DisablePlayerControls();
            return;
        }

        // Your existing update logic here...
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0 && !isGameOver)
        {
            // Player died
            GameOver();
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth.ToString();
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    void GameOver()
    {
        Debug.Log("Game Over method called");
        isGameOver = true;

        // Disable player controls or any other game-related activities
        DisablePlayerControls();

        // Show game over UI
        gameOverUI.SetActive(true);
        gameOverText.gameObject.SetActive(true);

        // Determine the winner based on which player has more health
        if (gameObject.CompareTag("Player1"))
        {
            if (GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerHealth>().GetCurrentHealth() > 0)
            {
                // Player 2 wins
                gameOverText.text = "You Died!\nPlayer 2 Wins!";
            }
            else
            {
                // Both players lost
                gameOverText.text = "Game Over\nIt's a Tie!";
            }
        }
        else if (gameObject.CompareTag("Player2"))
        {
            if (GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerHealth>().GetCurrentHealth() > 0)
            {
                // Player 1 wins
                gameOverText.text = "You Died!\nPlayer 1 Wins!";
            }
            else
            {
                // Both players lost
                gameOverText.text = "Game Over\nIt's a Tie!";
            }
        }
    }

    void DisablePlayerControls()
    {
        // Disable specific player input scripts
        if (gameObject.CompareTag("Player1") && player1Input != null)
        {
            player1Input.enabled = false;
            player2Input.enabled = false;
        }

        if (gameObject.CompareTag("Player2") && player2Input != null)
        {
            player2Input.enabled = false;
            player1Input.enabled = false;
        }

        // Add any other control-disabling logic specific to your game
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}