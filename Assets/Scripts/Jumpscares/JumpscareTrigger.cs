using System.Collections;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    AudioManager audioManager;
    bool isTriggerActive = true;

    public float chanceOfCreakScare = 0.33f;
    public float chanceOfBreathScare = 0.33f;
    public float cooldownTime = 30f;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggerActive && (other.CompareTag("Player1") || other.CompareTag("Player2")))
        {
            Debug.Log("Player has entered jumpscare trigger");

            // Generate a random number between 0 and 1
            float randomValue = Random.value;

            // Determine which jumpscare to play based on the random number
            if (randomValue < chanceOfCreakScare)
            {
                audioManager.PlayJumpscare(audioManager.creakScare);
            }
            else if (randomValue < chanceOfCreakScare + chanceOfBreathScare)
            {
                audioManager.PlayJumpscare(audioManager.breathScare);
            }
            else
            {
                audioManager.PlayJumpscare(audioManager.whisperScare);
            }

            // Disable the trigger for the cooldown period
            isTriggerActive = false;
            StartCoroutine(EnableTriggerAfterCooldown());
        }
    }

    private IEnumerator EnableTriggerAfterCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        isTriggerActive = true;
    }
}



