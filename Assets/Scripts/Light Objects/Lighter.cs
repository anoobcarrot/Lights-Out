using UnityEngine;
using UnityEngine.InputSystem;

public class Lighter : MonoBehaviour
{
    private Light lighterLight;
    public Transform lighterLightTransform;
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
        // Toggle the light component
        lighterLight.enabled = !lighterLight.enabled;

        // Toggle the visibility of the lighterLightTransform
        if (lighterLightTransform != null)
        {
            lighterLightTransform.gameObject.SetActive(lighterLight.enabled);
        }
    }
}


