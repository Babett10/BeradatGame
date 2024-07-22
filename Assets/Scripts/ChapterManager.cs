using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterManager : MonoBehaviour
{
    public Button[] chapterButtons; // Asumsi tombol disusun sesuai urutan chapter
    private GameManager gameManager;
    public GameObject Gembok1;
    public GameObject Gembok2;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateChapterButtons();
    }

    public void UpdateChapterButtons()
    {
        // Pastikan chapter pertama selalu terbuka
        chapterButtons[0].interactable = true;
        Debug.Log("Chapter 0 is always unlocked.");

        for (int i = 1; i < chapterButtons.Length; i++) // Mulai dari 1 karena chapter pertama sudah dipastikan terbuka
        {
            if (gameManager.IsChapterCompleted(i))
            {
                chapterButtons[i].interactable = true;
                Gembok1.SetActive(false);
                Gembok2.SetActive(false);
                Debug.Log("Chapter " + i + " is completed and unlocked.");
            }
            else
            {
                chapterButtons[i].interactable = false;
                Gembok1.SetActive(true);
                Gembok2.SetActive(true);
                Debug.Log("Chapter " + i + " is not completed and remains locked.");
            }
        }
    }

    public void UnlockNextChapter(int currentChapterIndex)
    {
        if (currentChapterIndex < chapterButtons.Length - 1)
        {
            int nextChapterIndex = currentChapterIndex + 1;
            gameManager.SetChapterCompletion(nextChapterIndex, true);
            UpdateChapterButtons();
        }
    }
}
