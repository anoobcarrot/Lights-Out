using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Torch : MonoBehaviour
{
    public Light torchLight;
    public float durationInSeconds = 60f;
    public float dimmingDuration = 5f;
    public Image batteryImage; // Reference to the battery image

    private float timer;
    private bool isDimming = false; // Track whether the torch is currently dimming
    [SerializeField] private bool isTimerZero = false; // Track whether the timer has reached zero

    void Start()
    {
        timer = durationInSeconds;
    }

    void Update()
    {
        // Check if the torch light is enabled
        if (torchLight.enabled)
        {
            // Update the timer if the torch is not currently dimming
            if (!isDimming)
            {
                timer -= Time.deltaTime;
                UpdateBatteryImage();

                // Check if the timer has reached 0 seconds
                if (timer <= 0f)
                {
                    // Turn off the torch light
                    // torchLight.enabled = false;
                    isTimerZero = true; // Set the flag to indicate that the timer is zero

                    // Start dimming the torch
                    StartCoroutine(DimAndTurnOff());
                }
            }
        }
    }

    public void ToggleTorchLight()
    {
        // Check if the torch is currently dimming or the timer is zero
        if (isDimming || timer <= 0f)
        {
            // Optional: You might want to add some feedback or log a message indicating why the torch can't be toggled.
            Debug.Log("Cannot toggle torch. Torch is currently dimming or the timer is zero.");
            return;
        }

        // Toggle the torch light on/off
        torchLight.enabled = !torchLight.enabled;

        }

    public bool IsTimerActive()
    {
        return timer > 0f && !isDimming;
    }

    public bool IsTimerZero()
    {
        return timer <= 0f && !isDimming;
    }

    IEnumerator DimAndTurnOff()
    {
        isDimming = true; // Set the flag to indicate that the torch is currently dimming

        // Dim the torch over the dimming duration
        float initialIntensity = torchLight.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < dimmingDuration)
        {
            // Calculate the lerp factor based on time
            float lerpFactor = elapsedTime / dimmingDuration;

            // Lerp the intensity from initial to 0
            torchLight.intensity = Mathf.Lerp(initialIntensity, 0f, lerpFactor);

            // Update elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the torch is completely off
        torchLight.intensity = 0f;

        // Wait for a short duration to ensure the dimming effect is visible
        yield return new WaitForSeconds(0.2f);

        // Disable the torch light
        torchLight.enabled = false;

        isDimming = false; // Reset the dimming flag
    }

    void UpdateBatteryImage()
    {
        // Calculate the fill amount based on the remaining time
        float fillAmount = Mathf.Clamp01(timer / durationInSeconds);

        // Update the battery image fill amount
        batteryImage.fillAmount = fillAmount;
    }
    public void IncreaseTimer(float bonusTime)
    {
        // Calculate the remaining time to reach the maximum duration
        float remainingTime = durationInSeconds - timer;

        // Add the bonus time but ensure it doesn't exceed the maximum duration
        float addedTime = Mathf.Min(bonusTime, remainingTime);

        // Increase the timer
        timer += addedTime;

        // Reset the torch light intensity to its original value 
        torchLight.intensity = 8f;

        // Update the battery image
        UpdateBatteryImage();
    }
}






