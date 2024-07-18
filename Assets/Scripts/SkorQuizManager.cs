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
    private string userName;
    public int currentStage; // Gunakan currentStage dari ItemCollector

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            userId = user.UserId;
            userName = user.DisplayName;

            skor_T.text = quiz.skor.ToString();
            string ranking = CalculateRanking(quiz.skor);
            ranking_T.text = ranking;

            string stageName = "stage" + currentStage; // Ubah currentStage menjadi string stageName
            SaveScore(userId, quiz.skor, ranking, userName, stageName);
        }
        else
        {
            Debug.LogError("User is not authenticated.");
        }
    }

    public string CalculateRanking(int score)
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

    public void SaveScore(string userId, int score, string ranking, string userName, string stage)
    {
        ScoreData scoreData = new ScoreData(score, ranking, userName, stage);
        string scoreJson = JsonUtility.ToJson(scoreData);

        databaseReference.Child("progress").Child("users").Child(userId).Child(stage).Child("scores").SetRawJsonValueAsync(scoreJson).ContinueWith(task =>
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
    public string userName;
    public string stage;

    public ScoreData(int score, string ranking, string userName, string stage)
    {
        this.userName = userName;
        this.score = score;
        this.ranking = ranking;
        this.stage = stage;
    }
}
