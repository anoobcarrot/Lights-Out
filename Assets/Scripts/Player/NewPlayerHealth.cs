using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewPlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isGameOver = false;

    public TextMeshProUGUI healthText;
    public Image healthBar;

    public GameObject gameOverUI;
    public TextMeshProUGUI gameOverText;

    public Player1Input player1Input;
    public Player2Input player2Input;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (isGameOver)
        {
            DisablePlayerControls();
            return;
        }

    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Taking Damage: " + damage);
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0 && !isGameOver)
        {
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
        DisablePlayerControls();

        gameOverUI.SetActive(true);
        gameOverText.gameObject.SetActive(true);

        DetermineWinner();
    }

    void DisablePlayerControls()
    {
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
    }

    void DetermineWinner()
    {
        if (gameObject.CompareTag("Player1"))
        {
            if (GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerHealth>().GetCurrentHealth() > 0)
            {
                gameOverText.text = "You Died!\nPlayer 2 Wins!";
            }
            else
            {
                gameOverText.text = "Game Over\nIt's a Tie!";
            }
        }
        else if (gameObject.CompareTag("Player2"))
        {
            if (GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerHealth>().GetCurrentHealth() > 0)
            {
                gameOverText.text = "You Died!\nPlayer 1 Wins!";
            }
            else
            {
                gameOverText.text = "Game Over\nIt's a Tie!";
            }
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}





