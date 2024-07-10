using UnityEngine;
using UnityEngine.EventSystems;

public class TouchRotate : MonoBehaviour, IPointerClickHandler
{
    private QuizManager puzzleGame; // Referensi ke skrip PuzzleGame

    void Start()
    {
        // Cari referensi ke PuzzleGame jika belum diatur
        puzzleGame = GetComponentInParent<QuizManager>();
        if (puzzleGame == null)
        {
            Debug.LogError("PuzzleGame component not found in parent objects.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (puzzleGame != null && !QuizManager.puzzleWin)
        {
            transform.Rotate(0f, 0f, 90f);
            Debug.Log("click");

            // Panggil metode untuk memeriksa apakah semua gambar sudah benar
            puzzleGame.CheckPuzzleCompletion();
        }
        else
        {
            Debug.Log("quiz already won.");
        }
    }
}
