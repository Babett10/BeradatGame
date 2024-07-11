using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemCollector : MonoBehaviour
{
    public AudioSource source;
    public AudioClip collectClip;

    public PlayerMovement player, playerMovement;
    public GameObject QuizPanel;
    public GameObject InfoPanel;
    public ImageLoader imageLoader;
    public int currentStage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SignQuiz"))
        {
            QuizPanel.transform.GetChild(collision.transform.GetSiblingIndex()).gameObject.SetActive(true);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Information"))
        {
            InfoPanel.transform.GetChild(collision.transform.GetSiblingIndex()).gameObject.SetActive(true);
            playerMovement.infoPanelActive(true);

            imageLoader.LoadStageInformation(currentStage);
        }
    }
}

