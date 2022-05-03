using Firebase.Auth;
using Firebase.Database;
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
    public void Exit() {
        Application.Quit();
    }
}
