using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pausePanel;
    public GameObject orangeText;


    [SerializeField] Text wintotalOrangesText;
    [SerializeField] Text losetotalOrangesText;


    private void Start()
    {
        int totalOrange = PlayerPrefs.GetInt("TotalOranges", 0);
        wintotalOrangesText.text = "Total Oranges : " + totalOrange;
        losetotalOrangesText.text = "Total Oranges : " + totalOrange;

    }

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
        Time.timeScale = 0f;
        isPaused = true;
        pausePanel.SetActive(true);
        orangeText.SetActive(false); // Aktifkan panel pause
        // Tambahkan logika lain yang relevan saat permainan di-pause
    }

    public void OpenPauseSetting()
    {
        if (!isPaused)
        {
            Pause();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);
        orangeText.SetActive(true); // Nonaktifkan panel pause
        // Tambahkan logika lain yang relevan saat permainan dilanjutkan
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
