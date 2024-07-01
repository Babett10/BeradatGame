using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public TMP_Text skor_T, terjawab_T;
    public GameObject FeedbackQuizBenar, FeedbackQuizSalah;

    int skor = 0, quizterjawab = 0;


    public string[] kunciJawaban;

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
            FeedbackQuizBenar.SetActive(true);
            skor += 20;

        }
        else
        {
            FeedbackQuizSalah.SetActive(true);
        }
        quizterjawab++;
        StartCoroutine(tutupQuiz());
    }

    IEnumerator tutupQuiz()
    {
        yield return new WaitForSeconds(2f);
        FeedbackQuizBenar.SetActive(false);
        FeedbackQuizSalah.SetActive(false);
        transform.GetChild(Nomor()).gameObject.SetActive(false);
    }

    void Update()
    {
        skor_T.text = "Skor : " + skor;
        terjawab_T.text = "Soal : " + quizterjawab + " / 5";
    }
}
