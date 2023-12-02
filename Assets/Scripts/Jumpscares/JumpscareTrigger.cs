using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    AudioManager audioManager;

    public float chanceOfCreakScare = 0.33f; 
    public float chanceOfBreathScare = 0.33f;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
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

        }
    }
}


