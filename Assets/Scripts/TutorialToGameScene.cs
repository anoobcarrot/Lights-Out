using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialToGameScene : MonoBehaviour
{
    // Name of the scene to transition to
    public string sceneToLoad = "GameScene";

    private void Update()
    {
        // Check if the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Load the specified scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

