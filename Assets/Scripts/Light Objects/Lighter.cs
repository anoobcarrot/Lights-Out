using UnityEngine;
using UnityEngine.InputSystem;

public class Lighter : MonoBehaviour
{
    private Light lighterLight;
    [SerializeField] private PlayerInput playerInput;

    private void Start()
    {
        lighterLight = GetComponentInChildren<Light>();
        if (lighterLight == null)
        {
            Debug.LogError("Light component not found on the lighter object.");
        }
    }

    private void Update()
    {
        if (playerInput.actions["ToggleLighter"].triggered)
        {
            ToggleLight();
        }
    }

    public void ToggleLight()
    {
        lighterLight.enabled = !lighterLight.enabled;
    }
}


