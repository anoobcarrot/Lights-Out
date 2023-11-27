using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class Player1Input : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 500f;
    public float sprintSpeedMultiplier = 2f;
    public float jumpForce = 5f;
    public GameObject torchPrefab;
    public GameObject lighterPrefab;

    private PlayerInput playerInput;
    private GameObject currentActiveItem;
    private Torch torchScript;
    private Lighter lighterScript;
    private PlayerHealth playerHealth;

    private Rigidbody playerRigidbody;
    private bool isGrounded;

    private bool canMove = true;
    private float damageTickInterval = 2.5f;
    private Coroutine damageCoroutine;
    private bool canEscape = false;
    private int escapePressCount = 0;
    private float timeBetweenPresses = 0.5f;
    public TextMeshProUGUI trapMessageText;
    private GameObject currentBearTrap;

    public float fontDecreaseAmount = 5f; // Font size decrease amount
    public float minFontSize = 20f; // Minimum font size

    private float originalFontSize; // Store the original font size

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        currentActiveItem = torchPrefab; // Set the initial active item
        playerHealth = GetComponentInChildren<PlayerHealth>();

        if (trapMessageText != null)
        {
            originalFontSize = trapMessageText.fontSize; // Store the original font size
        }

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

        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (canMove)
        {
            HandleMovement();
            HandleItemToggling();
            CheckForEscape();
            Jump();
        }
        else
        {
            CheckForEscape(); // Check for escape when trapped
        }

        // Display the trap message text
        if (!canMove && trapMessageText != null)
        {
            trapMessageText.gameObject.SetActive(true);

            // Change the text content to include the actual input action button
            string escapeButton = playerInput.actions["BearTrapEscape"].bindings[0].ToDisplayString();
            trapMessageText.text = $"You have stepped into a bear trap. Press {escapeButton} to struggle!";
        }
        else if (trapMessageText != null)
        {
            trapMessageText.gameObject.SetActive(false);
        }
    }

    private void HandleMovement()
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
    }

    private void HandleItemToggling()
    {
        if (playerInput.actions["SwitchLight"].triggered)
        {
            ToggleItem();
        }

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

    private void Jump()
    {
        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Check if the player is grounded
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the player is not grounded
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BearTrap"))
        {
            Debug.Log("Player entered the trigger zone");
            canMove = false;
            damageCoroutine = StartCoroutine(ApplyDamageOverTime());

            // Store the reference to the bear trap
            currentBearTrap = other.gameObject;

            // Display the trap message text
            if (trapMessageText != null)
            {
                trapMessageText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BearTrap"))
        {
            Debug.Log("Player exited the trigger zone");
            canMove = true;

            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }

            // Destroy the bear trap that the player exited
            Destroy(currentBearTrap);
        }
    }

    private void CheckForEscape()
    {
        // Check for space key input to escape
        if (!canMove && playerInput.actions["BearTrapEscape"].triggered)
        {
            escapePressCount++;

            if (escapePressCount >= 3) // Adjust the number of required presses as needed
            {
                EscapeFromTrap();
            }
            else
            {
                // Decrease the font size, but not below the minimum font size
                if (trapMessageText != null && trapMessageText.fontSize > minFontSize)
                {
                    trapMessageText.fontSize = Mathf.Max(trapMessageText.fontSize - fontDecreaseAmount, minFontSize);
                }

                Debug.Log("Press " + (3 - escapePressCount) + " more times to escape!");
                StartCoroutine(ResetEscapeCount());
            }
        }
    }

    private void EscapeFromTrap()
    {
        Debug.Log("Player escaped the bear trap!");

        // Stop the damage over time coroutine
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }

        // Enable movement
        canMove = true;

        trapMessageText.gameObject.SetActive(false);

        // Destroy the bear trap object
        GameObject bearTrap = GameObject.FindGameObjectWithTag("BearTrap");
        if (bearTrap != null)
        {
            Destroy(currentBearTrap);
        }

        // Reset escape-related variables
        ResetEscapeVariables();
    }

    private IEnumerator ResetEscapeCount()
    {
        yield return new WaitForSeconds(timeBetweenPresses);
        escapePressCount = 0;
    }

    private void ResetEscapeVariables()
    {
        canEscape = false;
        escapePressCount = 0;

        // Reset the font size to the original size
        if (trapMessageText != null)
        {
            trapMessageText.fontSize = originalFontSize;
        }
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while (true)
        {
            playerHealth.TakeDamage(10);
            yield return new WaitForSeconds(damageTickInterval);
        }
    }
}











