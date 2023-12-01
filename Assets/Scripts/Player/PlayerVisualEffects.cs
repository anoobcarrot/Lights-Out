using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerVisualEffects : MonoBehaviour
{
    public Image flashImage;
    public Image bloodImage;
    public Sprite bloodImage1;
    public Sprite bloodImage2;

    public float flashDuration = 0.1f;
    public float bloodImageThreshold1 = 75f;
    public float bloodImageThreshold2 = 50f;

    private PlayerHealth playerHealth;
    private bool isFlashing = false;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        if (flashImage == null || bloodImage == null || playerHealth == null || bloodImage1 == null || bloodImage2 == null)
        {
            Debug.LogError("Missing references in the PlayerVisualEffects script.");
            return;
        }

        // Initialize UI elements
        flashImage.gameObject.SetActive(false);
        bloodImage.gameObject.SetActive(false);

        // Subscribe to the TakeDamage event
        playerHealth.OnTakeDamage += HandleTakeDamage;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        if (playerHealth != null)
        {
            playerHealth.OnTakeDamage -= HandleTakeDamage;
        }
    }

    private void HandleTakeDamage(int damage)
{
    // Flash screen
    FlashScreen();

    // Show blood images based on health thresholds
    if (playerHealth.GetCurrentHealth() <= bloodImageThreshold2)
    {
        ShowBloodImage(bloodImage2);
    }
    else if (playerHealth.GetCurrentHealth() <= bloodImageThreshold1)
    {
        ShowBloodImage(bloodImage1);
    }
}


    private void FlashScreen()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator FlashRoutine()
    {
        flashImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        flashImage.gameObject.SetActive(false);
        isFlashing = false;
    }

    private void ShowBloodImage(Sprite bloodSprite)
    {
        bloodImage.sprite = bloodSprite;
        bloodImage.gameObject.SetActive(true);
    }
}

