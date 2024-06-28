using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject loginPanel;

    [SerializeField]
    private GameObject registrationPanel;

    [SerializeField]
    private GameObject MainMenuPanel;

    [SerializeField]
    private GameObject AuthPanel;
    [SerializeField]
    private GameObject SettingPanel;

    public TextMeshProUGUI Iduser;
    public TextMeshProUGUI Emailuser;

    void Start()
    {
        // Menunggu hingga user telah diinisialisasi sebelum mengupdate display name
        StartCoroutine(WaitForUserInitialization());
    }

    private IEnumerator WaitForUserInitialization()
    {
        // Tunggu hingga FirebaseAuthManager.user tidak null
        while (FirebaseAuthManager.user == null)
        {
            yield return null;
        }

        // Setelah user tidak null, update display name
        UpdateDisplayName();
    }

    public void UpdateDisplayName()
    {
        // Pastikan Iduser tidak null
        if (Iduser != null)
        {
            // Pastikan FirebaseAuthManager.user tidak null
            if (FirebaseAuthManager.user != null)
            {
                Iduser.text = FirebaseAuthManager.user.DisplayName;
                Emailuser.text = FirebaseAuthManager.user.Email;
            }
            else
            {
                Debug.LogWarning("FirebaseAuthManager.user is null");
                Iduser.text = "No user signed in";
            }
        }
        else
        {
            Debug.LogError("Iduser is null");
        }
    }

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        registrationPanel.SetActive(false);
    }

    public void OpenMainMenuPanel()
    {
        MainMenuPanel.SetActive(true);
        AuthPanel.SetActive(false);
    }

    public void OpenAuthPanel()
    {
        AuthPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
        SettingPanel.SetActive(false);
    }

    public void OpenRegistrationPanel()
    {
        registrationPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Debug.Log("Application Exit");
    }
}