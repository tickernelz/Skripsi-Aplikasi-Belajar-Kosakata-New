using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using Michsky.UI.ModernUIPack;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public int jumlahLatihanBab1 = 2, jumlahLatihanBab2 = 2, jumlahLatihanBab3 = 2;
    public Transform scoreboardContent;
    public GameObject scoreElement;
    private bool _cekData;

    public void Start()
    {
        StartCoroutine(LoadScoreboardData());
    }

    IEnumerator LoadScoreboardData()
    {
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        var dbTask = DBreference.Child("users").OrderByChild("TotalSkor").GetValueAsync();

        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

        if (dbTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = dbTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in scoreboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                _cekData = childSnapshot.Child("nama").Exists;
                if (_cekData)
                {
                    string nama = childSnapshot.Child("nama").Value.ToString();
                    string sekolah = childSnapshot.Child("sekolah").Value.ToString();
                    float skorBab1 = (float.Parse(childSnapshot.Child("Latihan1Bab1").Value.ToString()) + float.Parse(childSnapshot.Child("Latihan2Bab1").Value.ToString())) / jumlahLatihanBab1;
                    float skorBab2 = (float.Parse(childSnapshot.Child("Latihan1Bab2").Value.ToString()) + float.Parse(childSnapshot.Child("Latihan2Bab2").Value.ToString())) / jumlahLatihanBab2;
                    float skorBab3 = (float.Parse(childSnapshot.Child("Latihan1Bab3").Value.ToString()) + float.Parse(childSnapshot.Child("Latihan2Bab3").Value.ToString())) / jumlahLatihanBab3;
                    float TotalSkor = float.Parse(childSnapshot.Child("TotalSkor").Value.ToString());

                    if (nama != "" && sekolah != "")
                    {
                        GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                        scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(nama, sekolah, skorBab1, skorBab2, skorBab3, TotalSkor);
                    }
                }
            }
        }
    }
}
