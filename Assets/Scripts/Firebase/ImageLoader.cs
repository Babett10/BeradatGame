using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using TMPro;
using System;

public class ImageLoader : MonoBehaviour
{
    public GameObject imagePrefab; // Prefab with RawImage, TextMeshProUGUI, and Button component
    public Transform imageContainer; // Parent transform to hold images
    public GameObject panelDeskripsi; // Panel deskripsi
    public RawImage deskripsiImage; // RawImage untuk gambar di panel deskripsi
    public TextMeshProUGUI deskripsiNama; // TMP untuk nama di panel deskripsi
    public TextMeshProUGUI deskripsiAsal; // TMP untuk asal di panel deskripsi
    public TextMeshProUGUI deskripsiSejarah; // TMP untuk sejarah di panel deskripsi
    public TextMeshProUGUI deskripsiDeskripsi; // TMP untuk deskripsi di panel deskripsi
    public TextMeshProUGUI detailLaki;
    public TextMeshProUGUI detailPerempuan;
    private DatabaseReference databaseReference;

    private List<GameObject> currentImages = new List<GameObject>(); // List to hold current displayed images

    IEnumerator LoadImage(string MediaUrl, RawImage rawImage)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (rawImage != null)
            {
                rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            }
            else
            {
                Debug.LogError("RawImage is null");
            }
        }
    }

    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                // Retrieve and display all images initially
                LoadAllImages();
            }
            else
            {
                Debug.LogError(task.Exception);
            }
        });
    }

    public void LoadAllImages()
    {
        // Clear current images
        foreach (GameObject image in currentImages)
        {
            Destroy(image);
        }
        currentImages.Clear();

        // Retrieve image URLs and names from the database
        databaseReference.Child("BajuAdat").GetValueAsync().ContinueWithOnMainThread(dbTask =>
        {
            if (dbTask.IsCompleted)
            {
                DataSnapshot snapshot = dbTask.Result;
                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    // Assuming each child contains a dictionary with "imageUrl", "name", "asal", "sejarah", and "deskripsi"
                    string imageUrl = childSnapshot.Child("imageUrl").Value.ToString();
                    string name = childSnapshot.Child("name").Value.ToString();
                    string asal = childSnapshot.Child("asal").Value.ToString();
                    string sejarah = childSnapshot.Child("sejarah").Value.ToString();
                    string deskripsi = childSnapshot.Child("detailPakaian").Value.ToString();
                    string detailLk = childSnapshot.Child("detailPakaianLaki").Value.ToString();
                    string detailPr = childSnapshot.Child("detailPakaianPerempuan").Value.ToString();

                    // Create a new prefab instance for each entry
                    GameObject newImage = Instantiate(imagePrefab, imageContainer);
                    currentImages.Add(newImage);
                    if (newImage != null)
                    {
                        RawImage rawImage = newImage.GetComponentInChildren<RawImage>();
                        TextMeshProUGUI nameText = newImage.GetComponentInChildren<TextMeshProUGUI>();
                        Button button = newImage.GetComponentInChildren<Button>();

                        if (rawImage != null)
                        {
                            // Load and display the image
                            StartCoroutine(LoadImage(imageUrl, rawImage));
                        }
                        else
                        {
                            Debug.LogError("RawImage component not found in prefab.");
                        }

                        if (nameText != null)
                        {
                            // Set the name text
                            nameText.text = name;
                        }
                        else
                        {
                            Debug.LogError("TextMeshProUGUI component not found in prefab.");
                        }

                        if (button != null)
                        {
                            // Add click event to display description panel
                            button.onClick.AddListener(() => ShowDescriptionPanel(imageUrl, name, asal, sejarah, deskripsi, detailLk, detailPr));
                        }
                        else
                        {
                            Debug.LogError("Button component not found in prefab.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Instantiated prefab is null.");
                    }
                }
            }
            else
            {
                Debug.LogError(dbTask.Exception);
            }
        });
    }

    public void DisplayImages(List<DataSnapshot> filteredSnapshots)
    {
        // Clear current images
        foreach (GameObject image in currentImages)
        {
            Destroy(image);
        }
        currentImages.Clear();

        foreach (DataSnapshot childSnapshot in filteredSnapshots)
        {
            // Assuming each child contains a dictionary with "imageUrl", "name", "asal", "sejarah", and "deskripsi"
            string imageUrl = childSnapshot.Child("imageUrl").Value.ToString();
            string name = childSnapshot.Child("name").Value.ToString();
            string asal = childSnapshot.Child("asal").Value.ToString();
            string sejarah = childSnapshot.Child("sejarah").Value.ToString();
            string deskripsi = childSnapshot.Child("detailPakaian").Value.ToString();
            string detailLk = childSnapshot.Child("detailPakaianLaki").Value.ToString();
            string detailPr = childSnapshot.Child("detailPakaianPerempuan").Value.ToString();

            // Create a new prefab instance for each entry
            GameObject newImage = Instantiate(imagePrefab, imageContainer);
            currentImages.Add(newImage);
            if (newImage != null)
            {
                RawImage rawImage = newImage.GetComponentInChildren<RawImage>();
                TextMeshProUGUI nameText = newImage.GetComponentInChildren<TextMeshProUGUI>();
                Button button = newImage.GetComponentInChildren<Button>();

                if (rawImage != null)
                {
                    // Load and display the image
                    StartCoroutine(LoadImage(imageUrl, rawImage));
                }
                else
                {
                    Debug.LogError("RawImage component not found in prefab.");
                }

                if (nameText != null)
                {
                    // Set the name text
                    nameText.text = name;
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI component not found in prefab.");
                }

                if (button != null)
                {
                    // Add click event to display description panel
                    button.onClick.AddListener(() => ShowDescriptionPanel(imageUrl, name, asal, sejarah, deskripsi, detailLk, detailPr));
                }
                else
                {
                    Debug.LogError("Button component not found in prefab.");
                }
            }
            else
            {
                Debug.LogError("Instantiated prefab is null.");
            }
        }
    }

    public void LoadStageInformation(int stage)
    {
        // Format key sesuai dengan struktur database Anda
        string stageKey = "BajuAdat/BajuAdat" + stage.ToString();

        databaseReference.Child(stageKey).GetValueAsync().ContinueWithOnMainThread(dbTask =>
        {
            if (dbTask.IsCompleted)
            {
                DataSnapshot snapshot = dbTask.Result;
                if (snapshot.Exists)
                {
                    string imageUrl = snapshot.Child("imageUrl").Value.ToString();
                    string name = snapshot.Child("name").Value.ToString();
                    string asal = snapshot.Child("asal").Value.ToString();
                    string sejarah = snapshot.Child("sejarah").Value.ToString();
                    string deskripsi = snapshot.Child("detailPakaian").Value.ToString();
                    string detailLk = snapshot.Child("detailPakaianLaki").Value.ToString();
                    string detailPr = snapshot.Child("detailPakaianPerempuan").Value.ToString();

                    // Menampilkan informasi pada panel
                    ShowDescriptionPanel(imageUrl, name, asal, sejarah, deskripsi, detailLk, detailPr);
                }
                else
                {
                    Debug.LogError("Data snapshot tidak ditemukan untuk stage " + stage);
                }
            }
            else
            {
                Debug.LogError(dbTask.Exception);
            }
        });
    }


    void ShowDescriptionPanel(string imageUrl, string name, string asal, string sejarah, string deskripsi, string detailPakaianLaki, string detailPakaianPerempuan)
    {
        // Show the description panel
        panelDeskripsi.SetActive(true);

        // Load and display the image
        StartCoroutine(LoadImage(imageUrl, deskripsiImage));

        // Set the text fields
        deskripsiNama.text = name;
        deskripsiAsal.text = asal;
        deskripsiSejarah.text = sejarah;
        deskripsiDeskripsi.text = deskripsi;
        detailLaki.text = detailPakaianLaki;
        detailPerempuan.text = detailPakaianPerempuan;
    }
}
