using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Input : MonoBehaviour
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
            UnityEngine.Debug.LogError("No Torch component found on the torchPrefab!");
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

        // Use horizontal movement of the joystick to control camera rotation
        Vector2 lookDelta = playerInput.actions["Rotate"].ReadValue<Vector2>();
        // Adjust the rotation
        float rotateInputHorizontal = lookDelta.x * rotationSpeed * Time.deltaTime;

        // Rotate the player horizontally
        transform.Rotate(0f, rotateInputHorizontal, 0f);

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
            UnityEngine.Debug.Log("Cannot toggle torch. Timer not active or already zero.");
        }
    }
}





