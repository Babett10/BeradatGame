using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pausePanel;
    public PlayerMovement playerMovement;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                Pause();
            }
        }

    }

    private void Pause()
    {
        isPaused = true;
        playerMovement.setPausedStatus(true);
        pausePanel.SetActive(true);
    }

    public void OpenPauseSetting()
    {
        if (!isPaused)
        {
            Pause();
        }
    }

    public void CloseInfoPanel(GameObject panel)
    {
        panel.SetActive(false);
        playerMovement.infoPanelActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        playerMovement.setPausedStatus(false);
        pausePanel.SetActive(false);
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void mainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
