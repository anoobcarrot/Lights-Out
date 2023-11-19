using UnityEngine;
using TMPro;

public class PlayerSkullHandler : MonoBehaviour
{
    public int maxSkulls = 1;
    private int currentSkulls = 0;
    private int sacrificedSkulls = 0;
    public TextMeshProUGUI skullUIText; // Reference to the TMP UI element for displaying the current skull count
    public TextMeshProUGUI sacrificedSkullsUIText; // Reference to the TMP UI element for displaying sacrificed skull count
    public SkullManager skullManager;
    public GameOverHandler gameOverHandler;

    bool isSacrificing = false;

    void Start()
    {
        // Assuming you've assigned the SkullManager and GameOverHandler in the inspector
        if (skullManager == null)
        {
            Debug.LogError("SkullManager not assigned to PlayerSkullHandler.");
        }

        if (gameOverHandler == null)
        {
            Debug.LogError("GameOverHandler not assigned to PlayerSkullHandler.");
        }
    }

    void Update()
    {
        // Check input for Player1 (G key)
        if (Input.GetKey(KeyCode.G) && CompareTag("Player1") && !isSacrificing)
        {
            isSacrificing = true;
            TrySacrificeSkull();
        }
        // Check input for Player2 (- key)
        else if (Input.GetKey(KeyCode.Minus) && CompareTag("Player2") && !isSacrificing)
        {
            isSacrificing = true;
            TrySacrificeSkull();
        }

        // Reset the flag if the key is released
        if (Input.GetKeyUp(KeyCode.G) || Input.GetKeyUp(KeyCode.Minus))
        {
            isSacrificing = false;
        }
    }

    public void TrySacrificeSkull()
    {
        if (currentSkulls > 0)
        {
            SacrificeSkull();
        }
    }

    public void PickUpSkull(GameObject skull)
    {
        if (currentSkulls < maxSkulls)
        {
            currentSkulls++;
            UpdateSkullUI();
            skullManager.RespawnSkull(skull);
        }
    }

    public void SacrificeSkull()
    {
        currentSkulls--;
        sacrificedSkulls++;
        UpdateSkullUI();
        UpdateSacrificedSkullsUI();

        if (sacrificedSkulls == 20)
        {
            // Implement logic for winning the game
            if (CompareTag("Player1"))
            {
                gameOverHandler.DisplayGameOver("Player 1");
            }
            else if (CompareTag("Player2"))
            {
                gameOverHandler.DisplayGameOver("Player 2");
            }
        }
    }

    void UpdateSkullUI()
    {
        // Update the TMP UI element with the current skull count
        skullUIText.text = "Carrying Skull: x" + currentSkulls;
    }

    void UpdateSacrificedSkullsUI()
    {
        // Update the TMP UI element with the number of sacrificed skulls
        sacrificedSkullsUIText.text = "Sacrificed Skulls: " + sacrificedSkulls;
    }

    public bool IsCarryingSkull()
    {
        return currentSkulls > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skull"))
        {
            // Assuming you have a reference to the SkullManager
            SkullManager skullManager = FindObjectOfType<SkullManager>();

            if (skullManager != null)
            {
                // Notify the SkullManager that a skull is collected
                skullManager.RespawnSkull(other.gameObject);
            }
        }
    }
}



