using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkorQuizManager : MonoBehaviour
{
    public QuizManager quiz;
    public TMP_Text skor_T, ranking_T;
    private void Start()
    {
        skor_T.text = quiz.skor.ToString();
        if (quiz.skor > 80)
        {
            ranking_T.text = "Luar Biasa !!";
        }
        else if (quiz.skor > 60)
        {
            ranking_T.text = "Kamu Hebat !";
        }
        else if (quiz.skor > 40)
        {
            ranking_T.text = "Cuku Baik";
        }
        else
        {
            ranking_T.text = "Belajar Lagi Ya";
        }
    }
}
