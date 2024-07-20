using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizController : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<Button> optionButtons;
    private QuizScene currentScene;
    private int currentQuestionIndex = 0;
    private int totalPoints = 0;
    public TextMeshProUGUI resultText;
    public GameObject quizPanel;

    public void SetupQuiz(QuizScene scene)
    {
        currentScene = scene;
        currentQuestionIndex = 0;
        DisplayQuestion();
        quizPanel.SetActive(true);
    }

    private void DisplayQuestion()
    {
        if (currentQuestionIndex < currentScene.questions.Count)
        {
            QuizScene.QuizQuestion question = currentScene.questions[currentQuestionIndex];
            questionText.text = question.question;

            for (int i = 0; i < optionButtons.Count; i++)
            {
                if (i < question.options.Count)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    optionButtons[i].GetComponentInChildren<Text>().text = question.options[i].text;

                    // Remove existing listeners
                    optionButtons[i].onClick.RemoveAllListeners();

                    // Add new listener
                    int optionIndex = i;
                    optionButtons[i].onClick.AddListener(() => OnOptionSelected(optionIndex));
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // Move to the next scene after the quiz
            GameController gameController = FindObjectOfType<GameController>();
            gameController.PlayScene(currentScene.nextSceneAfterQuiz);
            quizPanel.SetActive(false);
            resultText.text = "" + totalPoints;
        }
    }

    private void OnOptionSelected(int optionIndex)
    {
        QuizScene.QuizQuestion question = currentScene.questions[currentQuestionIndex];
        if (question.options[optionIndex].isCorrect)
        {
            totalPoints += 1;
            Debug.Log("Correct Answer!");
        }
        else
        {
            Debug.Log("Incorrect Answer!");
        }
        
        currentQuestionIndex++;
        DisplayQuestion();
    }
}
