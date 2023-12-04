using UnityEngine;
using UnityEngine.AI;

public class RatJumpscare : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float walkingSpeed = 3.0f;

    private NavMeshAgent navMeshAgent;
    private AudioManager audioManager;
    private bool jumpscareTriggered = false; // Variable to track if the jumpscare has occurred

    private void Awake()
    {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found on the rat model.");
        }
        else
        {
            // Deactivate the NavMeshAgent initially
            navMeshAgent.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!jumpscareTriggered && (other.CompareTag("Player1") || other.CompareTag("Player2")))
        {
            Debug.Log("Player has entered jumpscare trigger");

            if (audioManager != null)
            {
                audioManager.PlayJumpscare(audioManager.ratScare);
            }

            if (navMeshAgent != null)
            {
                // Activate the NavMeshAgent
                navMeshAgent.enabled = true;

                // Set the starting position for the rat model
                navMeshAgent.transform.position = startPoint.position;

                // Set the destination for the rat model to walk
                navMeshAgent.SetDestination(endPoint.position);

                // Set the speed of the NavMeshAgent
                navMeshAgent.speed = walkingSpeed;

                // Start a coroutine to continuously correct the rotation
                StartCoroutine(CorrectRotation());

                // Set the flag to indicate that the jumpscare has occurred
                jumpscareTriggered = true;
            }
        }
    }

    private System.Collections.IEnumerator CorrectRotation()
    {
        while (true)
        {
            // Get the current Y-axis rotation and set the rotation to maintain -90 degrees on the X-axis
            float currentYRotation = navMeshAgent.transform.rotation.eulerAngles.y;
            navMeshAgent.transform.rotation = Quaternion.Euler(-90f, currentYRotation, 0f);

            // Wait for the end of the frame
            yield return null;
        }
    }
}

