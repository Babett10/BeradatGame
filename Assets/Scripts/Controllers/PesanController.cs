using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PesanController : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Referensi ke UI Text
    // public TextMeshProUGUI messageText; // Uncomment jika menggunakan TextMeshPro

    public GameObject messagePanel; // Referensi ke panel untuk menampilkan pesan
    public Button nextButton; // Referensi ke tombol next
    public Button backButton; // Referensi ke tombol back

    private PesanScene currentScene;
    private int currentSentenceIndex = 0;
    private bool isDialogueEnded = false;

    public float typingSpeed = 0.05f; // Kecepatan pengetikan teks

    private void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonClicked); // Tambahkan listener untuk tombol next
        backButton.onClick.AddListener(OnBackButtonClicked); // Tambahkan listener untuk tombol back
    }

    public void SetupPesan(PesanScene scene)
    {
        currentScene = scene;
        currentSentenceIndex = 0; // Reset index kalimat
        isDialogueEnded = false; // Reset status dialog
        messagePanel.SetActive(true);
        DisplayNextSentence();

        // Tampilkan tombol back hanya jika previousScene tidak kosong
        backButton.gameObject.SetActive(currentScene.previousScene != null);
    }

    public void DisplayNextSentence()
    {
        if (!isDialogueEnded)
        {
            if (currentSentenceIndex < currentScene.sentences.Count)
            {
                StopAllCoroutines();
                bool isLastSentence = currentSentenceIndex == currentScene.sentences.Count - 1;
                StartCoroutine(TypeSentence(currentScene.sentences[currentSentenceIndex].text, isLastSentence));
                currentSentenceIndex++;
            }
        }
    }

    private IEnumerator TypeSentence(string sentence, bool isLastSentence)
    {
        messageText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (isLastSentence)
        {
            isDialogueEnded = true;
            nextButton.interactable = true; // Aktifkan tombol next setelah kalimat terakhir
        }
    }

    private void Update()
    {
        if (!isDialogueEnded && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }

    private void EndDialogue()
    {
        GameController gameController = FindObjectOfType<GameController>();
        gameController.PlayScene(currentScene.nextScene);
        messagePanel.SetActive(false);
    }

    private void OnNextButtonClicked()
    {
        if (isDialogueEnded)
        {
            EndDialogue();
        }
        else
        {
            DisplayNextSentence();
        }
    }

    private void OnBackButtonClicked()
    {
        GameController gameController = FindObjectOfType<GameController>();
        if (currentScene.previousScene != null)
        {
            gameController.PlayScene(currentScene.previousScene);
        }
    }
}
