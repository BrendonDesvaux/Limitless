using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUi : MonoBehaviour
{
    public GameObject pauseUI;

    private bool isPaused = false;

    private void Start()
    {
        // Désactive l'UI de pause au démarrage
        pauseUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        //log
        Debug.Log("TogglePause");
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
            //set mouse active
            Cursor.visible = true;
            //let mouse free
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f;
            pauseUI.SetActive(false);
            //set curson invisible
            Cursor.visible = false;
            //lock cursor
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // public void OpenSettings()
    // {
    //     settingsUI.SetActive(true);
    //     pauseUI.SetActive(false);
    // }

    public void QuitGame()
    {
        //log
        Debug.Log("QuitGame");
        Application.Quit();
    }
}
