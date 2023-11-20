using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SlowTextAppear : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    public float typeSpeed = 0.05f;

    public string additionalMessage = "Press Spacebar to Start";
    public TextMeshProUGUI additionalText;

    public float zoomSpeed = 0.5f; // Adjust the speed of the zoom
    public float fadeSpeed = 1.0f; // Adjust the speed of the fade

    private string originalText;
    private string currentText = "";

    private bool isTextFullyRevealed = false;
    private bool isAdditionalTextVisible = false;

    public string sceneToLoad = ""; // Set the scene name in the inspector

    private void Start()
    {
        if (targetText == null || additionalText == null)
        {
            Debug.LogError("Target TextMeshProUGUI or Additional TextMeshProUGUI component not assigned!");
            return;
        }

        // Save the original text from the TMP component
        originalText = targetText.text;

        // Clear the TMP text, and start the coroutine to gradually reveal the text
        targetText.text = "";
        additionalText.text = "";
        StartCoroutine(TypeText());
    }

    private System.Collections.IEnumerator TypeText()
    {
        for (int i = 0; i <= originalText.Length; i++)
        {
            currentText = originalText.Substring(0, i);
            targetText.text = currentText;

            yield return new WaitForSeconds(typeSpeed);
        }

        isTextFullyRevealed = true;
        // Display additional message
        additionalText.text = additionalMessage;


        // Start zooming and fading the additional message
        StartCoroutine(ZoomAndFadeAdditionalText());
    }

    private System.Collections.IEnumerator ZoomAndFadeAdditionalText()
    {
        float startScale = 1.0f;
        float zoomIncrement = 0.1f;

        while (true)
        {
            // Only start displaying the additional text after the initial text is fully revealed
            if (isTextFullyRevealed)
            {
                isAdditionalTextVisible = true;
            }

            // Zoom in
            additionalText.rectTransform.localScale += new Vector3(zoomIncrement, zoomIncrement, 0);

            // Fade in
            while (additionalText.color.a < 1.0f)
            {
                float newAlpha = Mathf.MoveTowards(additionalText.color.a, 1.0f, fadeSpeed * Time.deltaTime);
                additionalText.color = new Color(additionalText.color.r, additionalText.color.g, additionalText.color.b, newAlpha);
                yield return null;
            }

            // If the additional text is visible, wait for a short time at full zoom and fade
            if (isAdditionalTextVisible)
            {
                yield return new WaitForSeconds(0.5f);
            }

            // Zoom out
            additionalText.rectTransform.localScale -= new Vector3(zoomIncrement, zoomIncrement, 0);

            // Fade out
            while (additionalText.color.a > 0.0f)
            {
                float newAlpha = Mathf.MoveTowards(additionalText.color.a, 0.0f, fadeSpeed * Time.deltaTime);
                additionalText.color = new Color(additionalText.color.r, additionalText.color.g, additionalText.color.b, newAlpha);
                yield return null;
            }

            // Reset position and scale
            additionalText.rectTransform.localScale = new Vector3(startScale, startScale, 0);
            additionalText.color = new Color(additionalText.color.r, additionalText.color.g, additionalText.color.b, 0.0f);

            // If the additional text is not visible, break out of the loop
            if (!isAdditionalTextVisible)
            {
                break;
            }

            yield return null;
        }
    }


    private void Update()
    {
        if (isTextFullyRevealed && !string.IsNullOrEmpty(sceneToLoad) && Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneToLoad);
    }
}





