using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class Welcome : MonoBehaviour
{
    public GameObject namaObj, sekolahObj;
    private string nama, sekolah;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
            namaObj.GetComponent<TMP_Text>().text = nama;
            sekolahObj.GetComponent<TMP_Text>().text = sekolah;
        }
    }
}
