using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuizScene", menuName = "Data/New Quiz Scene")]
[System.Serializable]
public class QuizScene : GameScene
{
    public List<QuizQuestion> questions;
    public GameScene nextSceneAfterQuiz;

    [System.Serializable]
    public struct QuizQuestion
    {
        public string question;
        public List<QuizOption> options;
    }

    [System.Serializable]
    public struct QuizOption
    {
        public string text;
        public bool isCorrect;
    }
}
