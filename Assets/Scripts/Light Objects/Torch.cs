using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Torch : MonoBehaviour
{
    public Transform torchLightTransform;
    public float durationInSeconds = 60f;
    public float flickerDuration = 3f;
    public float flickerInterval = 0.3f;
    public float delayBeforeTurnOff = 0.5f;
    public Image batteryImage;

    private Light torchLight;
    private float timer;
    private bool isFlickering = false;
    private bool isTimerZero = false;

    void Start()
    {
        timer = durationInSeconds;
        torchLight = torchLightTransform.GetComponentInChildren<Light>();

        if (torchLight == null)
        {
            Debug.LogError("Light component not found on the Torch light GameObject!");
        }
    }

    void Update()
    {
        if (torchLightTransform.gameObject.activeSelf)
        {
            if (!isFlickering)
            {
                timer -= Time.deltaTime;
                UpdateBatteryImage();

                if (timer <= 0f)
                {
                    isTimerZero = true;
                    StartCoroutine(FlickerAndTurnOff());
                }
            }
        }
    }

    public void ToggleTorchLight()
    {
        if (isFlickering || timer <= 0f)
        {
            Debug.Log("Cannot toggle torch. Torch is currently flickering or the timer is zero.");
            return;
        }

        // Toggle the visibility of the torchLightTransform
        torchLightTransform.gameObject.SetActive(!torchLightTransform.gameObject.activeSelf);

        // Toggle the enabled state of the Light component on the child object
        Light torchLight = torchLightTransform.GetComponentInChildren<Light>();
        if (torchLight != null)
        {
            torchLight.enabled = torchLightTransform.gameObject.activeSelf;
        }
    }

    public void DisableTorchLight()
    {
        if (torchLightTransform != null)
        {
            torchLightTransform.gameObject.SetActive(false);
        }
    }

    public bool IsTimerActive()
    {
        return timer > 0f && !isFlickering;
    }

    public bool IsTimerZero()
    {
        return timer <= 0f && !isFlickering;
    }

    IEnumerator FlickerAndTurnOff()
    {
        isFlickering = true;

        // Flicker effect for 3 seconds
        float flickerEndTime = Time.time + flickerDuration;
        while (Time.time < flickerEndTime)
        {
            torchLightTransform.gameObject.SetActive(!torchLightTransform.gameObject.activeSelf);
            yield return new WaitForSeconds(flickerInterval);
        }

        // Wait for a short delay before turning off the torch light
        yield return new WaitForSeconds(delayBeforeTurnOff);

        // Keep the torchLightTransform disabled
        torchLightTransform.gameObject.SetActive(false);

        Light torchLight = torchLightTransform.GetComponentInChildren<Light>();
        if (torchLight != null)
        {
            torchLight.enabled = torchLightTransform.gameObject.activeSelf;
        }

        isFlickering = false;
        isTimerZero = false;
    }

    void UpdateBatteryImage()
    {
        float fillAmount = Mathf.Clamp01(timer / durationInSeconds);
        batteryImage.fillAmount = fillAmount;
    }

    public void IncreaseTimer(float bonusTime)
    {
        float remainingTime = durationInSeconds - timer;
        float addedTime = Mathf.Min(bonusTime, remainingTime);
        timer += addedTime;

        // Re-enable the torchLightTransform
        torchLightTransform.gameObject.SetActive(true);

        UpdateBatteryImage();
    }
}











