using UnityEngine;

public class Battery : MonoBehaviour
{
    public float bonusTime = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            Torch torch = other.GetComponentInChildren<Torch>();

            if (torch != null)
            {
                torch.IncreaseTimer(bonusTime);
                Debug.Log("Battery collected. Torch timer increased.");
                Destroy(gameObject); // Optional: Remove the battery from the scene

                // Notify the BatteryManager that this battery has been collected
                BatteryManager batteryManager = FindObjectOfType<BatteryManager>();
                if (batteryManager != null)
                {
                    batteryManager.RemoveBattery(gameObject);
                }
            }
            else
            {
                Debug.Log("Torch component not found on player or its children.");
            }
        }
        else
        {
            Debug.Log("Collider entered, but it's not a player.");
        }
    }
}