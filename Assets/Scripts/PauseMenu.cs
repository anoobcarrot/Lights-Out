using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    private void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        if(pauseMenu != null)
        {
            bool isPaused = !pauseMenu.activeSelf;
            pauseMenu.SetActive(isPaused);

            Time.timeScale = isPaused ? 0 : 1;

            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
