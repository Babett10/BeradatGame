using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    private AudioSource finishSound;
    public GameObject WinPanel;
    public GameObject losePanel;
    public PlayerMovement playerMovement;
    public SkorQuizManager skorQuizManager; // Reference to SkorQuizManager

    void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            finishSound.Play();
            StartCoroutine(CompleteLevel());
        }
    }

    IEnumerator CompleteLevel()
    {
        yield return new WaitForSeconds(0.5f);
        playerMovement.setWinStatus(true);
        int playerScore = skorQuizManager.GetScore();
        if (playerScore > 40)
        {
            SaveStageCompletion();
            WinPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Score is too low to complete the stage.");
            losePanel.SetActive(true);
        }
    }

    void SaveStageCompletion()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentStageNumber = GetStageNumberFromSceneName(currentSceneName);

        // Simpan status penyelesaian stage saat ini
        PlayerPrefs.SetInt("Stage" + currentStageNumber, 1);

        // Buka stage berikutnya
        int nextStageNumber = currentStageNumber + 1;
        PlayerPrefs.SetInt("Stage" + nextStageNumber, 1);

        PlayerPrefs.Save(); // Pastikan data tersimpan
    }

    int GetStageNumberFromSceneName(string sceneName)
    {
        // Asumsi nama scene adalah "StageX" di mana X adalah nomor stage
        string stageNumberString = sceneName.Replace("Stage", "");
        int stageNumber;
        if (int.TryParse(stageNumberString, out stageNumber))
        {
            return stageNumber;
        }
        else
        {
            Debug.LogError("Nama scene tidak valid.");
            return 0;
        }
    }
}
