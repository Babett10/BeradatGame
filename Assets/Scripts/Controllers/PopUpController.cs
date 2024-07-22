using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour
{
    public Image popupImage; // Referensi ke Image di Panel
    public Button popupButton;
    public GameObject popupPanel; // Referensi ke GameObject Panel
    private PopUpScene currentScene; // PopUpScene yang sedang ditampilkan

    public void SetupPopUp(PopUpScene scene)
    {
        currentScene = scene;
        DisplayPopup();
        popupPanel.SetActive(true);
    }

    public void DisplayPopup()
    {
        if (popupPanel != null && popupImage != null && currentScene != null)
        {
            popupPanel.SetActive(true); // Aktifkan panel

            // Tampilkan sprite dari PopUpScene
            if (currentScene.popupImage != null)
            {
                popupImage.sprite = currentScene.popupImage;

                // Tambahkan komponen Button ke popupImage jika belum ada
                Button button = popupButton.GetComponent<Button>();
                if (button == null)
                {
                    button = popupButton.gameObject.AddComponent<Button>();
                }

                // Tambahkan event listener untuk memanggil ClosePopup ketika gambar diklik
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(ClosePopup);
            }
            else
            {
                Debug.LogWarning("Sprite is not set in PopUpScene.");
            }
        }
        else
        {
            Debug.LogWarning("PopupPanel, PopupImage, or PopUpScene not set properly.");
        }
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false); // Nonaktifkan panel

        // Lakukan sesuatu setelah menutup popup, misalnya pindah ke scene berikutnya
        if (currentScene != null)
        {
            GameController gameController = FindObjectOfType<GameController>();
            gameController.PlayScene(currentScene.nextSceneAfterPopup); // Pindah ke scene berikutnya setelah popup ditutup
        }
    }
}
