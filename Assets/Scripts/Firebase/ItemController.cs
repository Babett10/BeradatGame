using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;

public class ItemController : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text asalText;
    public Button editButton;
    public Button deleteButton;
    public GameObject editPanel;

    private string itemKey;
    private DatabaseReference databaseReference;

    public void Setup(DataSnapshot data)
    {
        itemKey = data.Key;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference.Child("BajuAdat").Child("Sumatera").Child(itemKey);

        nameText.text = data.Child("name").Value.ToString();
        asalText.text = data.Child("asal").Value.ToString();


        deleteButton.onClick.AddListener(DeleteItem);
    }

    void EditItem()
    {
        if (editPanel != null)
        {
            Debug.Log("Mengaktifkan Panel Edit");
            editPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("editPanel belum diassign di inspector");
        }
    }


    void DeleteItem()
    {
        databaseReference.RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Failed to delete item: " + task.Exception);
            }
        });
    }
}
