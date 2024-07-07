using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;

public class SkorQuizManager : MonoBehaviour
{
    public QuizManager quiz;
    public TMP_Text skor_T, ranking_T;
    public DatabaseReference databaseReference;
    private string userId;
    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        skor_T.text = quiz.skor.ToString();
        string ranking = CalculateRanking(quiz.skor);
        ranking_T.text = ranking;
        SaveScore(userId, quiz.skor, ranking);
    }

    private string CalculateRanking(int score)
    {
        if (score > 80)
        {
            return "Luar Biasa !!";
        }
        else if (score > 60)
        {
            return "Kamu Hebat !";
        }
        else if (score > 40)
        {
            return "Cukup Baik";
        }
        else
        {
            return "Belajar Lagi Ya";
        }
    }

    public void SaveScore(string userId, int score, string ranking)
    {
        ScoreData scoreData = new ScoreData(score, ranking);
        string json = JsonUtility.ToJson(scoreData);

        databaseReference.Child("scores").Child(userId).SetValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Score and Ranking Saved Successfully.");
            }
            else
            {
                Debug.LogError("Failed to save score and ranking: " + task.Exception);
            }
        });
    }
}

[System.Serializable]
public class ScoreData
{
    public int score;
    public string ranking;

    public ScoreData(int score, string ranking)
    {
        this.score = score;
        this.ranking = ranking;
    }
}
