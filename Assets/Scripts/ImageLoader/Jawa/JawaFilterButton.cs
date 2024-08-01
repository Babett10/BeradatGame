using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System.Collections.Generic;
using TMPro;

public class JawaFilterButton : MonoBehaviour
{
    public List<Button> filterButtons; // List of filter buttons for each province
    public Button viewAllButton; // Button for viewing all items
    public JawaImageLoader imageLoader;
    private DatabaseReference databaseReference;

    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                // Set up filter button listeners
                foreach (Button button in filterButtons)
                {
                    string provinceName = button.name; // Assuming button name is the province name
                    button.onClick.AddListener(() => FilterByProvinceJawa(provinceName));
                }
                // Set up view all button listener
                viewAllButton.onClick.AddListener(() =>
                {
                    imageLoader.LoadAllImages();
                });
            }
            else
            {
                Debug.LogError(task.Exception);
            }
        });
    }

    void FilterByProvinceJawa(string province)
    {
        databaseReference.Child("BajuAdat").Child("Jawa").GetValueAsync().ContinueWithOnMainThread(dbTask =>
        {
            if (dbTask.IsCompleted)
            {
                DataSnapshot snapshot = dbTask.Result;
                List<DataSnapshot> filteredSnapshots = new List<DataSnapshot>();

                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    string asal = childSnapshot.Child("asal").Value.ToString();
                    if (asal == province)
                    {
                        filteredSnapshots.Add(childSnapshot);
                    }
                }

                imageLoader.DisplayImages(filteredSnapshots);
            }
            else
            {
                Debug.LogError(dbTask.Exception);
            }
        });
    }
}
