using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Input : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;
    public float sprintSpeedMultiplier = 2f;
    public GameObject torchPrefab;

    private Torch torchScript;

    private PlayerInput playerInput;

    private void Start()
    {
        torchScript = torchPrefab.GetComponent<Torch>();
        if (torchScript == null)
        {
            Debug.LogError("No Torch component found on the torchPrefab!");
        }

        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        // Player 2 controls using Input System
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        bool sprint = playerInput.actions["Sprint"].ReadValue<float>() > 0.5f;
        float currentSpeed = sprint ? movementSpeed * sprintSpeedMultiplier : movementSpeed;

        Vector3 movement = new Vector3(horizontal, 0f, vertical) * currentSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Use horizontal and vertical movement of the joystick to control camera rotation
        Vector2 rotateInput = playerInput.actions["Rotate"].ReadValue<Vector2>();
        float rotateInputHorizontal = rotateInput.x;
        float rotateInputVertical = rotateInput.y;

        float rotationHorizontal = rotateInputHorizontal * rotationSpeed * Time.deltaTime;
        float rotationVertical = rotateInputVertical * rotationSpeed * Time.deltaTime;

        // Adjust the player's rotation
        transform.Rotate(rotationVertical, rotationHorizontal, 0f);

        if (playerInput.actions["ToggleTorch"].triggered)
        {
            ToggleTorch();
        }
    }

    private void ToggleTorch()
    {
        if (torchScript != null && torchScript.IsTimerActive() && !torchScript.IsTimerZero())
        {
            torchScript.ToggleTorchLight();
        }
        else
        {
            Debug.Log("Cannot toggle torch. Timer not active or already zero.");
        }
    }
}


