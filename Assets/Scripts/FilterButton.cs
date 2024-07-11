using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System.Collections.Generic;
using TMPro;

public class FilterButtons : MonoBehaviour
{
    public List<Button> filterButtons; // List of filter buttons for each province
    public Button viewAllButton; // Button for viewing all items
    public ImageLoader imageLoader;
    public TextMeshProUGUI filterText; // Text to display the current filter
    private DatabaseReference databaseReference;

    void Start()
    {
        // Hide the filter text initially
        filterText.gameObject.SetActive(false);

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
                    button.onClick.AddListener(() => FilterByProvince(provinceName));
                }

                // Set up view all button listener
                viewAllButton.onClick.AddListener(() =>
                {
                    imageLoader.LoadAllImages();
                    filterText.gameObject.SetActive(false); // Hide filter text when showing all items
                });
            }
            else
            {
                Debug.LogError(task.Exception);
            }
        });
    }

    void FilterByProvince(string province)
    {
        databaseReference.Child("BajuAdat").GetValueAsync().ContinueWithOnMainThread(dbTask =>
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
                filterText.text = province; // Update filter text to show current province
                filterText.gameObject.SetActive(true); // Show the filter text
            }
            else
            {
                Debug.LogError(dbTask.Exception);
            }
        });
    }
}
