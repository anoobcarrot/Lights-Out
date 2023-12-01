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

    // Reference to the pickup text
    public TextMeshProUGUI pickupText;
    public Camera playerCamera;

    public GameObject skullHand;

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

        // Assuming you've assigned the pickup text in the inspector
        if (pickupText == null)
        {
            Debug.LogError("PickupText not assigned to PlayerSkullHandler.");
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
        else
        {
            // Update the pickup text
            UpdatePickupText();

            CheckBatteryInput(playerInput);

            UpdateBatteryPickupText();

            UpdateSkullHand();
        }

        // Check for the "Drop" input action
        if (playerInput.actions["Drop"].triggered)
        {
            DropSkull();
        }
    }

    public void DropSkull()
    {
        if (IsCarryingSkull())
        {
            // Find the position where the player was last standing and drop the skull there
            Vector3 dropPosition = transform.position;
            dropPosition.y = 0f; // Set the y-coordinate to 0 to avoid dropping the skull in mid-air

            // Instantiate the dropped skull
            GameObject droppedSkull = Instantiate(skullManager.skullPrefab, dropPosition, Quaternion.identity);

            // Decrement the current skulls count
            currentSkulls--;

            // Update UI
            UpdateSkullUI();

            // Set the dropped flag for the dropped skull
            Skull skullComponent = droppedSkull.GetComponent<Skull>();
            if (skullComponent != null)
            {
                skullComponent.SetDropped(true);
            }
        }
    }

    private void UpdateSkullHand()
    {
        // Activate/deactivate the body game object based on whether the player is carrying a skull
        skullHand.SetActive(IsCarryingSkull());
    }

    private void CheckBatteryInput(PlayerInput playerInput)
{
    // Check input for Player1 (G key)
    if (playerInput.actions["Interact"].triggered && CompareTag("Player1"))
    {
        CollectBattery();
    }
    // Check input for Player2 (- key)
    else if (playerInput.actions["Interact"].triggered && CompareTag("Player2"))
    {
        CollectBattery();
    }
}

private void CollectBattery()
{
    Battery battery = FindNearestBattery();

    if (battery != null)
    {
        battery.CollectBattery(this);
        UpdateBatteryPickupText();
    }
}

// Find the nearest battery within the trigger area
private Battery FindNearestBattery()
{
    Battery[] batteries = FindObjectsOfType<Battery>();
    Battery nearestBattery = null;
    float nearestDistance = float.MaxValue;

    foreach (Battery battery in batteries)
    {
        float distance = Vector3.Distance(battery.transform.position, transform.position);

        if (distance < nearestDistance)
        {
            nearestDistance = distance;
            nearestBattery = battery;
        }
    }

    return nearestBattery;
}

private void UpdateBatteryPickupText()
{
    Debug.Log("Updating battery pickup text method called.");

    // Cast a ray from the player's camera
    Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
    RaycastHit hit;

    // Set the raycast distance based on your game's requirements
    float raycastDistance = 5f;

    if (Physics.Raycast(ray, out hit, raycastDistance))
    {
        // Check if the hit object is a battery
        Battery battery = hit.collider.GetComponent<Battery>();

        if (battery != null)
        {
            Debug.Log($"Updating battery pickup text: {pickupText.text}");
            pickupText.text = $"Pick Up Battery [{GetInteractInputDisplayName()}]";
            return;
        }
    }

    // If no battery is hit, hide the pickup text
    Debug.Log("Hiding battery pickup text (no batteries nearby).");
    pickupText.text = "";
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

    // Find the nearest skull
    GameObject FindNearestSkull()
{
    GameObject[] skulls = GameObject.FindGameObjectsWithTag("Skull");
    GameObject nearestSkull = null;
    float nearestDistance = float.MaxValue;

    foreach (GameObject skull in skulls)
    {
        float distance = Vector3.Distance(skull.transform.position, transform.position);

        if (distance < nearestDistance)
        {
            nearestDistance = distance;
            nearestSkull = skull;
        }
    }

    return nearestSkull;
}

     private void UpdatePickupText()
    {
        GameObject nearestSkull = FindNearestSkull();

        if (nearestSkull != null)
        {
            float distance = Vector3.Distance(nearestSkull.transform.position, transform.position);
            float interactRadius = nearestSkull.GetComponent<Skull>().interactRadius;

            // Check if the player is within the required radius and looking at the skull
            if (distance <= interactRadius && nearestSkull.GetComponent<Skull>().IsPlayerLookingAtSkull(this))
            {
                Debug.Log($"Updating pickup text: {pickupText.text}");
                pickupText.text = $"Pick Up Skull [{GetInteractInputDisplayName()}]";
            }
            else
            {
                Debug.Log("Hiding pickup text.");
                // Hide the pickup text if the player is not close enough or not looking at the skull
                pickupText.text = "";
            }
        }
        else
        {
            Debug.Log("Hiding pickup text (no skulls nearby).");
            // Hide the pickup text if no skulls are nearby
            pickupText.text = "";
        }
    }

    public bool IsInteractButtonPressed()
{
    // Check if the interact input action is triggered
    bool isPressed = playerInput.actions["Interact"].triggered;
    Debug.Log($"Interact Button Pressed: {isPressed}");
    return isPressed;
}

   public string GetInteractInputDisplayName()
{
    int bindingIndex = CompareTag("Player1") ? 0 : 1;

    // Get the binding display name for the specified "Interact" action and binding index
    return playerInput.actions["Interact"].GetBindingDisplayString(bindingIndex);
}

    public void ShowPickupText(string text)
    {
        // Display the pickup text
        pickupText.text = text;
    }

    public void HidePickupText()
    {
        // Hide the pickup text
        pickupText.text = "";
    }

    public bool IsCarryingSkull()
    {
        // Check if the player is carrying a skull
        return currentSkulls > 0;
    }
}





