using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemCollector : MonoBehaviour
{
    private int Oranges = 0;

    public AudioSource source;
    public AudioClip collectClip;

    public PlayerMovement player;

    [SerializeField] private Text orangesText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orange"))
        {
            Destroy(collision.gameObject);
            Oranges++;
            orangesText.text = "Oranges: " + Oranges;
            Debug.Log ("jeruk = " + Oranges);
            PlayerPrefs.SetInt("TotalOranges", Oranges); //menyimpan jeruk yang sudah di collect ke dalam PlayerPrefs
            source.PlayOneShot(collectClip);
        }
    }
    
}
