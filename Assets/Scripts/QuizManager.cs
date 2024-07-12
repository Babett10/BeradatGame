using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public AudioSource quizBenar, quizSalah;
    public TMP_Text skor_T, terjawab_T;
    public GameObject FeedbackQuizBenar, FeedbackQuizSalah, FeedbackGambarBenar, FeedBackGambarSalah;

    public int skor = 0, quizterjawab = 0;
    public enum QuizType { PilihanGanda, TebakGambar }
    public QuizType[] quizTypes;
    public string[] kunciJawaban;

    [SerializeField]
    private Transform[] pictures;

    public static bool puzzleWin = false;

    void OnEnable()
    {
        // Reset puzzleWin ketika script diaktifkan kembali
        puzzleWin = false;
    }

    void Start()
    {
        // Reset puzzleWin ketika script dimulai
        puzzleWin = false;
    }

    public int Nomor()
    {
        int a = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                a = i;
            }
        }
        return a;
    }

    public void jawab(GameObject tombol)
    {
        for (int i = 0; i < tombol.transform.parent.childCount; i++)
        {
            tombol.transform.parent.GetChild(i).GetComponent<Button>().enabled = false;
        }
        if (tombol.name == kunciJawaban[Nomor()])
        {
            quizBenar.Play();
            FeedbackQuizBenar.SetActive(true);
            skor += 20;

        }
        else if (quizTypes[Nomor()] == QuizType.TebakGambar)
        {
            if (tombol.name == kunciJawaban[Nomor()])
            {
                quizBenar.Play();
                FeedbackGambarBenar.SetActive(true);
                skor += 20;
            }
            else
            {
                quizSalah.Play();
                FeedBackGambarSalah.SetActive(true);
            }
        }
        else
        {
            quizSalah.Play();
            FeedbackQuizSalah.SetActive(true);
        }
        quizterjawab++;
        StartCoroutine(tutupQuiz());
    }

    IEnumerator tutupQuiz()
    {
        yield return new WaitForSeconds(1.7f);
        FeedbackQuizBenar.SetActive(false);
        FeedbackQuizSalah.SetActive(false);
        FeedbackGambarBenar.SetActive(false);
        FeedBackGambarSalah.SetActive(false);
        transform.GetChild(Nomor()).gameObject.SetActive(false);
    }

    void Update()
    {
        skor_T.text = "Skor : " + skor;
        terjawab_T.text = "Soal : " + quizterjawab + " / 6";

    }

    public void CheckPuzzleCompletion()
    {
        if (puzzleWin) return; // Jika puzzle sudah dimenangkan, tidak perlu melakukan pengecekan lagi

        foreach (var picture in pictures)
        {
            if (!IsCorrectRotation(picture))
            {
                Debug.Log($"{picture.name} is not correctly rotated.");
                return; // Keluar dari fungsi jika ada gambar yang tidak benar rotasinya
            }
        }

        // Jika semua gambar sudah benar, maka puzzle dimenangkan
        Debug.Log("All pieces are correctly rotated.");
        puzzleWin = true;
        quizterjawab++;
        skor += 20;
        quizBenar.Play();
        FeedbackGambarBenar.SetActive(true);
        StartCoroutine(tutupQuiz());
    }

    // Fungsi untuk memeriksa apakah rotasi gambar adalah 0
    private bool IsCorrectRotation(Transform picture)
    {
        // Anda bisa memodifikasi kondisi ini jika rotasi yang benar bukan 0
        return Mathf.Approximately(picture.rotation.eulerAngles.z, 0);
    }
}
