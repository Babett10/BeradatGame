using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseLoadData : MonoBehaviour
{
    public Transform contentPanel; // Panel di dalam ScrollView tempat data akan ditampilkan
    public GameObject itemPrefab; // Prefab untuk item list
    public string selectedRegion = "Sumatera"; // Pilihan region, bisa diubah sesuai dengan UI

    private DatabaseReference databaseReference;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                LoadData();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    void LoadData()
    {
        databaseReference.Child("BajuAdat").Child(selectedRegion).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    GameObject newItem = Instantiate(itemPrefab, contentPanel);
                    newItem.GetComponent<ItemController>().Setup(data);
                }
            }
            else
            {
                Debug.LogError("Failed to load data: " + task.Exception);
            }
        });
    }
}
