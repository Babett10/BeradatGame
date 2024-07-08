using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemCollector : MonoBehaviour
{
    private int Oranges = 0;

    public AudioSource source;
    public AudioClip collectClip;

    public PlayerMovement player, playerMovement;
    public GameObject QuizPanel;
    public GameObject InfoPanel;

    [SerializeField] private Text orangesText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orange"))
        {
            Destroy(collision.gameObject);
            Oranges++;
            orangesText.text = "Oranges: " + Oranges;
            Debug.Log("jeruk = " + Oranges);
            PlayerPrefs.SetInt("TotalOranges", Oranges); //menyimpan jeruk yang sudah di collect ke dalam PlayerPrefs
            source.PlayOneShot(collectClip);
        }

        if (collision.gameObject.CompareTag("SignQuiz"))
        {

            QuizPanel.transform.GetChild(collision.transform.GetSiblingIndex()).gameObject.SetActive(true);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Information"))
        {
            InfoPanel.transform.GetChild(collision.transform.GetSiblingIndex()).gameObject.SetActive(true);
            playerMovement.infoPanelActive(true);
        }
    }

}
