using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;
    public GameObject soonPanel;

    public GameObject LoadingScreen;
    public Image LoadingBar;

    private void Update()
    {
        
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
        Debug.Log("chara: " + selectedCharacter);
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);
        Debug.Log("chara: " + selectedCharacter);
    }

    public void StartGame(int sceneID)
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        Debug.Log("chara dipilih :  " + selectedCharacter);

        if (selectedCharacter > 0)
        {
            soonPanel.SetActive(true);
        }

        else
        {   
        StartCoroutine(LoadSceneAsync(sceneID));
        }
    }


    IEnumerator LoadSceneAsync(int sceneID)
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        LoadingScreen.SetActive(true);

        while(!operation.isDone)
        {
        float incrementValue = 0.25f * Time.deltaTime / 5f;
        float progressValue = Mathf.Clamp01(LoadingBar.fillAmount + incrementValue);

            LoadingBar.fillAmount = progressValue;

            yield return null;
        }

    }

    
}
