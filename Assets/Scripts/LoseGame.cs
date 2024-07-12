using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseGame : MonoBehaviour
{
    [SerializeField] Text totalOrangesText ;
    private void Start()
    {
        //memanggil total oranges yang disimpan di PlayerPrefs
        //jika tidak ada yang disimpan nilai default = 0
        int totalOrange = PlayerPrefs.GetInt("TotalOranges", 0); 
        totalOrangesText.text = "Total Oranges : " + totalOrange;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("GamePlayScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
