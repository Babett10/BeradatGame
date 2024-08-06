using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using System.Collections.Generic;

public class EditData : MonoBehaviour
{
    public GameObject editPanel;
    public TMP_InputField nameInputField;
    public TMP_InputField asalInputField;
    public TMP_InputField detailPakaianInputField;
    public TMP_InputField detailPakaianLakiInputField;
    public TMP_InputField detailPakaianPerempuanInputField;
    public TMP_InputField imageUrlInputField;
    public TMP_InputField sejarahInputField;
    public Button saveButton;

    private string itemKey;
    private DatabaseReference databaseReference;

    public void ShowPanel(DataSnapshot data, string key)
    {
        itemKey = key;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference.Child("BajuAdat").Child("Sumatera").Child(itemKey);

        // Mengisi input fields dengan data yang ada
        nameInputField.text = data.Child("name").Value.ToString();
        asalInputField.text = data.Child("asal").Value.ToString();
        detailPakaianInputField.text = data.Child("detailPakaian").Value.ToString();
        detailPakaianLakiInputField.text = data.Child("detailPakaianLaki").Value.ToString();
        detailPakaianPerempuanInputField.text = data.Child("detailPakaianPerempuan").Value.ToString();
        imageUrlInputField.text = data.Child("imageUrl").Value.ToString();
        sejarahInputField.text = data.Child("sejarah").Value.ToString();

        editPanel.SetActive(true);
    }

    public void SaveChanges()
    {
        var updatedData = new Dictionary<string, object>
        {
            {"name", nameInputField.text},
            {"asal", asalInputField.text},
            {"detailPakaian", detailPakaianInputField.text},
            {"detailPakaianLaki", detailPakaianLakiInputField.text},
            {"detailPakaianPerempuan", detailPakaianPerempuanInputField.text},
            {"imageUrl", imageUrlInputField.text},
            {"sejarah", sejarahInputField.text}
        };

        databaseReference.UpdateChildrenAsync(updatedData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data berhasil diupdate.");
                editPanel.SetActive(false); // Tutup panel edit
            }
            else
            {
                Debug.LogError("Gagal mengupdate data: " + task.Exception);
            }
        });
    }

    public void CancelEdit()
    {
        editPanel.SetActive(false); // Menutup panel edit tanpa menyimpan perubahan
    }
}
