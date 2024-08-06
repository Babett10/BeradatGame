using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class FirebaseInputData : MonoBehaviour
{
    public TMP_InputField asalInputField;
    public TMP_InputField detailPakaianInputField;
    public TMP_InputField detailPakaianLakiInputField;
    public TMP_InputField detailPakaianPerempuanInputField;
    public TMP_InputField imageUrlInputField;
    public TMP_InputField nameInputField;
    public TMP_InputField sejarahInputField;
    public string selectedRegion = "Sumatera"; // Contoh region yang dipilih, bisa diubah sesuai dengan pilihan pengguna

    private DatabaseReference databaseReference;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    public void SubmitPakaianAdat()
    {
        string key = databaseReference.Child("BajuAdat").Child(selectedRegion).Push().Key;

        var pakaianAdat = new Dictionary<string, object>
        {
            {"asal", asalInputField.text},
            {"detailPakaian", detailPakaianInputField.text},
            {"detailPakaianLaki", detailPakaianLakiInputField.text},
            {"detailPakaianPerempuan", detailPakaianPerempuanInputField.text},
            {"imageUrl", imageUrlInputField.text},
            {"name", nameInputField.text},
            {"sejarah", sejarahInputField.text}
        };

        databaseReference.Child("BajuAdat").Child(selectedRegion).Child(key).SetValueAsync(pakaianAdat).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data pakaian adat berhasil disimpan.");

                // Kosongkan semua input field setelah data berhasil disimpan
                ClearInputFields();
            }
            else
            {
                Debug.LogError("Terjadi kesalahan saat menyimpan data: " + task.Exception);
            }
        });
    }

    private void ClearInputFields()
    {
        asalInputField.text = "";
        detailPakaianInputField.text = "";
        detailPakaianLakiInputField.text = "";
        detailPakaianPerempuanInputField.text = "";
        imageUrlInputField.text = "";
        nameInputField.text = "";
        sejarahInputField.text = "";
    }
}
