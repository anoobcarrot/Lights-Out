using UnityEngine;

public class Player2Input : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f; // Adjust this value to control rotation speed
    public float sprintSpeedMultiplier = 2f; // Adjust this value to control sprint speed
    public GameObject torchPrefab;
    private Torch torchScript; // Reference to the Torch script

    void Start()
    {
        // Get the Torch script from the torchPrefab
        torchScript = torchPrefab.GetComponent<Torch>();
        if (torchScript == null)
        {
            Debug.LogError("No Torch component found on the torchPrefab!");
        }
    }

    void Update()
    {
        // Player 2 controls
        float horizontal = Input.GetAxis("Player2_Horizontal");
        float vertical = Input.GetAxis("Player2_Vertical");

        // Check for sprint input
        bool sprint = Input.GetKey(KeyCode.RightShift);

        // Adjust movement speed based on sprint input
        float currentSpeed = sprint ? movementSpeed * sprintSpeedMultiplier : movementSpeed;

        // Move the player
        Vector3 movement = new Vector3(horizontal, 0f, vertical) * currentSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Rotate the player's camera for Player 2
        float rotateInput = Input.GetAxis("Player2_Rotate");
        Vector3 rotation = new Vector3(0f, rotateInput * rotationSpeed * Time.deltaTime, 0f);
        transform.Rotate(rotation);

        // Check for torch activation input
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleTorch();
        }
    }

    void ToggleTorch()
    {
        // Check if the torch timer is active and not zero
        if (torchScript != null && torchScript.IsTimerActive() && !torchScript.IsTimerZero())
        {
            // Toggle the torch light on/off
            torchScript.ToggleTorchLight();
        }
        else
        {
            // Add a debug log to see why the conditions were not met
            Debug.Log("Cannot toggle torch. Timer not active or already zero.");
        }
    }
}

