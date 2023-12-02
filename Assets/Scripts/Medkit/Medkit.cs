using UnityEngine;

public class Medkit : MonoBehaviour
{
    public int healthBonusAmount = 25;

    // Update is called once per frame
    void Update()
    {
        // Rotate the medkit for visibility (optional)
        transform.Rotate(Vector3.up * Time.deltaTime * 30f);
    }

    public void CollectMedkit(PlayerItemHandler player)
    {
        // Cast a ray from the player's camera
        Ray ray = new Ray(player.playerCamera.transform.position, player.playerCamera.transform.forward);
        RaycastHit hit;

        // Set the raycast distance based on your game's requirements
        float raycastDistance = 5f;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Check if the hit object is the medkit
           if (hit.collider.gameObject == gameObject)
    {
        // Increase player health with the medkit's bonus amount
        player.playerHealth.IncreaseHealth(healthBonusAmount);

        Debug.Log("Medkit collected. Player health increased.");

        // Destroy the medkit 
        Destroy(gameObject);

        // Notify the MedkitManager that this medkit has been collected
        MedkitManager medkitManager = FindObjectOfType<MedkitManager>();
        if (medkitManager != null)
        {
            medkitManager.RemoveMedkit(gameObject);
        }
    }
}
    }
}
  
