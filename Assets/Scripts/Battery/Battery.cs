using UnityEngine;

public class Battery : MonoBehaviour
{
    public float bonusTime = 20f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the battery for visibility (optional)
        transform.Rotate(Vector3.up * Time.deltaTime * 30f);
    }

    public void CollectBattery(PlayerSkullHandler player)
    {
        // Cast a ray from the player's camera
        Ray ray = new Ray(player.playerCamera.transform.position, player.playerCamera.transform.forward);
        RaycastHit hit;

        // Set the raycast distance based on your game's requirements
        float raycastDistance = 5f;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Check if the hit object is the battery
            if (hit.collider.gameObject == gameObject)
            {
                Torch torch = player.GetComponentInChildren<Torch>();

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
        }
    }
}
  
