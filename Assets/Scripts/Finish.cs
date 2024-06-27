using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    private AudioSource finishSound;
    public GameObject WinPanel;
    public GameObject orangeText;

    void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.name == "Player")
        {
            finishSound.Play();
            StartCoroutine(CompleteLevel());
        }
    }

    IEnumerator CompleteLevel()
    {
        yield return new WaitForSeconds(0.5f);
        WinPanel.SetActive(true);    
        orangeText.SetActive(false);     
    }

    
}
