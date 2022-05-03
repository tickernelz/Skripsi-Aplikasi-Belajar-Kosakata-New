using System;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class InputNama : MonoBehaviour
{
    private string nama, sekolah;
    public GameObject namaObj, sekolahObj;

    private void writeDataUser(string userId, string nama, string sekolah)
    {
        DatabaseReference mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        mDatabaseRef.Child("users").Child(userId).Child("nama").SetValueAsync(nama);
        mDatabaseRef.Child("users").Child(userId).Child("sekolah").SetValueAsync(sekolah);
    }

    public void Update()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                DataSnapshot snapshot = task.Result;
                nama = snapshot.Child(user.UserId).Child("nama").Value.ToString();
                sekolah = snapshot.Child(user.UserId).Child("sekolah").Value.ToString();
            });
            if (namaObj.GetComponent<TMP_InputField>().text == "" &&
                namaObj.GetComponent<TMP_InputField>().isFocused != true)
            {
                namaObj.GetComponent<TMP_InputField>().text = nama;
            }

            if (sekolahObj.GetComponent<TMP_InputField>().text == "" &&
                sekolahObj.GetComponent<TMP_InputField>().isFocused != true)
            {
                sekolahObj.GetComponent<TMP_InputField>().text = sekolah;
            }
        }
    }

    public void SaveInput()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;
        if (namaObj != null && sekolahObj != null)
        {
            nama = namaObj.GetComponent<TMP_InputField>().text;
            sekolah = sekolahObj.GetComponent<TMP_InputField>().text;
            PlayerPrefs.SetString("nama", nama);
            PlayerPrefs.SetString("sekolah", sekolah);
            writeDataUser(user.UserId, nama, sekolah);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}