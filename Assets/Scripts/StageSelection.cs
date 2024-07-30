using UnityEngine;
using UnityEngine.UI;

public class StageSelection : MonoBehaviour
{
    [System.Serializable]
    public class StageButton
    {
        public Button button; // Button untuk stage
        public Image lockImage; // Image untuk gembok
    }

    public StageButton[] stageButtons; // Set button dan lock image untuk setiap stage di inspector

    private void Start()
    {
        LoadStageStatus();
    }

    private void LoadStageStatus()
    {
        // Buka stage pertama secara default
        stageButtons[0].button.interactable = true;
        stageButtons[0].lockImage.gameObject.SetActive(false);

        // Periksa status penyelesaian untuk stage lainnya
        for (int i = 1; i < stageButtons.Length; i++)
        {
            int stageNumber = i + 1;
            if (PlayerPrefs.GetInt("Stage" + stageNumber, 0) == 1)
            {
                stageButtons[i].button.interactable = true;
                stageButtons[i].lockImage.gameObject.SetActive(false); // Sembunyikan gembok
            }
            else
            {
                stageButtons[i].button.interactable = false;
                stageButtons[i].lockImage.gameObject.SetActive(true); // Tampilkan gembok
            }
        }
    }
}