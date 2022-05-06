using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using Michsky.UI.ModernUIPack;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public string namaNilai;
    public Transform scoreboardContent;
    public GameObject scoreElement;

    public void Start()
    {        
        StartCoroutine(LoadScoreboardData());
    }
    
    IEnumerator LoadScoreboardData()
    {
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        //Get all the users data ordered by kills amount
        var dbTask = DBreference.Child("users").OrderByChild(namaNilai).GetValueAsync();

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
                string nama = childSnapshot.Child("nama").Value.ToString();
                string sekolah = childSnapshot.Child("sekolah").Value.ToString();
                float skorBab1 = float.Parse(childSnapshot.Child("Latihan1Bab1").Value.ToString());
                float skorBab2 = float.Parse(childSnapshot.Child("Latihan1Bab1").Value.ToString());

                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(nama, sekolah, skorBab1, skorBab2);
            }
        }
    }
}
