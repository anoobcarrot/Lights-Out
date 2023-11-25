using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Input : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;
    public float sprintSpeedMultiplier = 2f;
    public GameObject torchPrefab;
    public GameObject lighterPrefab;

    private PlayerInput playerInput;
    private GameObject currentActiveItem;
    private Torch torchScript;
    private Lighter lighterScript;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        currentActiveItem = torchPrefab; // Set the initial active item

        torchScript = torchPrefab.GetComponent<Torch>();
        if (torchScript == null)
        {
            Debug.LogError("No Torch component found on the torchPrefab!");
        }

        lighterScript = lighterPrefab.GetComponent<Lighter>();
        if (lighterScript == null)
        {
            Debug.LogError("No Lighter component found on the lighterPrefab!");
        }
    }

    private void Update()
    {
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        bool sprint = playerInput.actions["Sprint"].ReadValue<float>() > 0.5f;
        float currentSpeed = sprint ? movementSpeed * sprintSpeedMultiplier : movementSpeed;

        Vector3 movement = new Vector3(horizontal, 0f, vertical) * currentSpeed * Time.deltaTime;
        transform.Translate(movement);

        Vector2 lookDelta = playerInput.actions["Rotate"].ReadValue<Vector2>();
        float rotateInputHorizontal = lookDelta.x * rotationSpeed * Time.deltaTime;

        transform.Rotate(0f, rotateInputHorizontal, 0f);

        // Handle item toggling
        if (playerInput.actions["SwitchLight"].triggered)
        {
            ToggleItem();
        }

        // Toggle Torch separately
        if (playerInput.actions["ToggleTorch"].triggered)
        {
            ToggleTorch();
        }
    }

    private void ToggleItem()
    {
        // Switch between items based on the current active item
        if (currentActiveItem == torchPrefab)
        {
            SwitchToLighter();
        }
        else if (currentActiveItem == lighterPrefab)
        {
            SwitchToTorch();
        }
    }

    private void SwitchToTorch()
    {
        // Disable the current active item
        if (currentActiveItem != null)
        {
            currentActiveItem.SetActive(false);
        }

        // Enable the torch if the conditions are met
        if (torchScript != null)
        {
            torchPrefab.SetActive(true);
            currentActiveItem = torchPrefab;
        }
        else
        {
            Debug.Log("Cannot switch to torch. Timer not active or already zero.");
        }
    }

    private void SwitchToLighter()
{
    // Disable the current active item
    if (currentActiveItem != null)
    {
        currentActiveItem.SetActive(false);

        // If the current active item is the torch, disable its light component
        if (currentActiveItem == torchPrefab)
        {
            Torch torch = currentActiveItem.GetComponent<Torch>();
            if (torch != null)
            {
                torch.DisableTorchLight();
            }
        }
    }

    // Enable the lighter
    lighterPrefab.SetActive(true);
    currentActiveItem = lighterPrefab;
}

    private void ToggleTorch()
    {
        // Toggle the torch on and off only if the torch is the active item
        if (currentActiveItem == torchPrefab && torchScript != null && torchScript.IsTimerActive() && !torchScript.IsTimerZero())
        {
            torchScript.ToggleTorchLight();
        }
        else
        {
            Debug.Log("Cannot toggle torch. Timer not active or already zero, or torch is not the active item.");
        }
    }
}










