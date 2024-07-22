using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Objek ini tidak akan dihancurkan saat memuat scene lain
        }
    }
    void Start()
    {
        // Pastikan chapter pertama selalu terbuka
        if (!PlayerPrefs.HasKey("Chapter0Completed"))
        {
            PlayerPrefs.SetInt("Chapter0Completed", 1);
        }
    }

    public void SetChapterCompletion(int chapterNumber, bool isCompleted)
    {
        PlayerPrefs.SetInt("Chapter" + chapterNumber + "Completed", isCompleted ? 1 : 0);
        Debug.Log("Set Chapter " + chapterNumber + " completion to " + (isCompleted ? "true" : "false"));
    }

    public bool IsChapterCompleted(int chapterNumber)
    {
        bool isCompleted = PlayerPrefs.GetInt("Chapter" + chapterNumber + "Completed", 0) == 1;
        Debug.Log("Chapter " + chapterNumber + " completed: " + isCompleted);
        return isCompleted;
    }
}



