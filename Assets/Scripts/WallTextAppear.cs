using UnityEngine;
using TMPro;

public class WallTextAppear : MonoBehaviour
{
    public TextMeshPro targetText;
    public float typeSpeed = 0.05f;

    private string originalText;
    private string currentText = "";

    private bool hasEnteredTrigger = false;

    private void Start()
    {
        if (targetText == null)
        {
            Debug.LogError("Target TextMeshProUGUI component not assigned!");
            return;
        }

        // Save the original text from the TMP component
        originalText = targetText.text;

        // Clear the TMP text
        targetText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the trigger was entered by a player (with the tags "Player1" or "Player2")
        if ((other.CompareTag("Player1") || other.CompareTag("Player2")) && !hasEnteredTrigger)
        {
            hasEnteredTrigger = true;

            // Start the coroutine to gradually reveal the text
            StartCoroutine(TypeText());
        }
    }

    private System.Collections.IEnumerator TypeText()
    {
        for (int i = 0; i <= originalText.Length; i++)
        {
            currentText = originalText.Substring(0, i);
            targetText.text = currentText;

            yield return new WaitForSeconds(typeSpeed);
        }
    }
}


