using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerSkullHandler : MonoBehaviour
{
    public int maxSkulls = 1;
    private int currentSkulls = 0;
    private int sacrificedSkulls = 0;
    public TextMeshProUGUI skullUIText; // Reference to the TMP UI element for displaying the current skull count
    public TextMeshProUGUI sacrificedSkullsUIText; // Reference to the TMP UI element for displaying sacrificed skull count
    public SkullManager skullManager;
    public GameOverHandler gameOverHandler;
    [SerializeField] private PlayerInput playerInput;

    bool isSacrificing = false;
    private bool isInTriggerArea = false;

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

    public void SetInTriggerArea(bool value)
    {
        isInTriggerArea = value;
    }

    private void Update()
    {
        // Check if the player is in the trigger area before allowing skull sacrifice
        if (isInTriggerArea)
        {
            CheckSacrificeInput(playerInput);
        }
    }

    private void CheckSacrificeInput(PlayerInput playerInput)
    {
        // Check input for Player1 (G key)
        if (playerInput.actions["Sacrifice"].triggered && CompareTag("Player1") && !isSacrificing)
        {
            isSacrificing = true;
            TrySacrificeSkull();
        }
        // Check input for Player2 (- key)
        else if (playerInput.actions["Sacrifice"].triggered && CompareTag("Player2") && !isSacrificing)
        {
            isSacrificing = true;
            TrySacrificeSkull();
        }

        // Reset the flag if the key is released
        if (playerInput.actions["Sacrifice"].triggered)
        {
            isSacrificing = false;
        }
    }

    public void TrySacrificeSkull()
    {
        // Check if the player is within the trigger area
        if (isInTriggerArea)
        {
            if (currentSkulls > 0)
            {
                SacrificeSkull();
            }
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

    private void OnTriggerStay(Collider other)
    {
        // Check if the player stays within the trigger area
        if (other.CompareTag("Well"))
        {
            isInTriggerArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger area
        if (other.CompareTag("Well"))
        {
            isInTriggerArea = false;
        }
    }
}



