using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleController : MonoBehaviour
{
    public List<Image> puzzleImageSlots;
    private PuzzleScene currentScene;
    public GameObject PuzzlePanel;
    private int currentPuzzleIndex = 0;
    public TextMeshProUGUI resultText;
    private int totalPoints = 0;

    public void SetupPuzzle(PuzzleScene scene)
    {
        currentScene = scene;
        DisplayPuzzle();
        PuzzlePanel.SetActive(true);
    }

    private void DisplayPuzzle()
    {
        if (currentPuzzleIndex < currentScene.puzzleImages.Count)
        {
            for (int i = 0; i < puzzleImageSlots.Count; i++)
            {
                if (i < currentScene.puzzleImages.Count)
                {
                    puzzleImageSlots[i].gameObject.SetActive(true);
                    puzzleImageSlots[i].sprite = currentScene.puzzleImages[i].image;

                    AddClickEventListener(puzzleImageSlots[i], i);

                    float randomRotation = Mathf.Round(Random.Range(0f, 3f)) * 90f;
                    puzzleImageSlots[i].rectTransform.rotation = Quaternion.Euler(0f, 0f, randomRotation);

                    // Set initial currentRotation
                    PuzzleScene.PuzzleImage modifiedImage = currentScene.puzzleImages[i];
                    modifiedImage.currentRotation = puzzleImageSlots[i].rectTransform.rotation.eulerAngles.z;
                    currentScene.puzzleImages[i] = modifiedImage;
                }
                else
                {
                    puzzleImageSlots[i].gameObject.SetActive(false);
                }
            }

            currentPuzzleIndex++;
            Debug.Log("Displaying puzzle image and handling rotations");
        }
        else
        {
            GameController gameController = FindObjectOfType<GameController>();
            gameController.PlayScene(currentScene.nextSceneAfterPuzzle);
            PuzzlePanel.SetActive(false);
            resultText.text = "" + totalPoints;
        }
    }

    private void AddClickEventListener(Image image, int index)
    {
        Button button = image.gameObject.GetComponent<Button>();
        if (button == null)
        {
            button = image.gameObject.AddComponent<Button>();
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnPuzzleImageClicked(index));
    }

    private void OnPuzzleImageClicked(int puzzleIndex)
    {
        float rotationAmount = 90f;
        puzzleImageSlots[puzzleIndex].rectTransform.Rotate(new Vector3(0f, 0f, rotationAmount));

        // Retrieve the PuzzleImage from the list, modify its currentRotation, and set it back
        PuzzleScene.PuzzleImage modifiedImage = currentScene.puzzleImages[puzzleIndex];
        modifiedImage.currentRotation = puzzleImageSlots[puzzleIndex].rectTransform.rotation.eulerAngles.z;
        currentScene.puzzleImages[puzzleIndex] = modifiedImage;

        Debug.Log("Puzzle piece rotated!");

        CheckIfPuzzleSolved();
    }

    private void CheckIfPuzzleSolved()
    {
        bool allPiecesCorrect = true;

        for (int i = 0; i < currentScene.puzzleImages.Count; i++)
        {
            float currentRotation = currentScene.puzzleImages[i].currentRotation;
            float correctRotation = currentScene.puzzleImages[i].correctRotation;

            if (Mathf.Abs(currentRotation - correctRotation) > 1f)
            {
                allPiecesCorrect = false;
                break;
            }
        }

        if (allPiecesCorrect)
        {
            totalPoints += 5;
            Debug.Log("Puzzle Solved!");
            OnPuzzleSolved();
        }
    }

    private void OnPuzzleSolved()
    {
        GameController gameController = FindObjectOfType<GameController>();
        gameController.PlayScene(currentScene.nextSceneAfterPuzzle);
        PuzzlePanel.SetActive(false);
    }
}
