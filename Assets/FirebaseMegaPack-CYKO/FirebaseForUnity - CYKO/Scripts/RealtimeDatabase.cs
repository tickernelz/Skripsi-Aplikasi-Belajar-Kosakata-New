using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
//using Firebase.Unity.Editor;
using UnityEngine.UI;
using Firebase.Extensions;
using Firebase.Firestore;
using System;

public class User {

        public string name;
        public string email;
        public string bio;
        public string timeStamp;

        public User ()
        {
        }

        public User (string name, string email, string bio, string timeStamp)
        {
                this.name = name;
                this.email = email;
                this.bio = bio;
                this.timeStamp = timeStamp;
        }
}

public class RealtimeDatabase : MonoBehaviour {
        DatabaseReference reference;
        DatabaseReference reference2;
        public InputField nameInput;
        public InputField emailInput;
        public InputField bioInput;
        public Text msg;
        string docEmail;
        public GameObject prefab;
        public Transform userBarParent;
        public Text eventListener;
        string eventText;


        void Start ()
        {
                //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl (Constant.FirebaseDatabaseURL);
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                reference2 = FirebaseDatabase.DefaultInstance.GetReference ("users");

                reference2.ChildAdded += HandleChildAdded;
                reference2.ChildChanged += HandleChildChanged;
                reference2.ChildRemoved += HandleChildRemoved;
                reference2.ChildMoved += HandleChildMoved;
        }

        public void AddNewData ()
        {
                msg.text = "Adding Data...";
                User user = new User (nameInput.text, emailInput.text, bioInput.text, DateTime.Now.ToString());

                string json = JsonUtility.ToJson (user);

                reference.Child ("users").Child (user.email).SetRawJsonValueAsync (json);
                Debug.Log ("RTDB Updated");
                msg.text = "Data Added !";
                eventText = "New Child Added";
        }

        public void ClearData ()
        {

		foreach (Transform child in userBarParent.transform) {
			Destroy (child.gameObject);
		}
	}



        public void GetUsersData ()
        {
                FirebaseDatabase db = FirebaseDatabase.DefaultInstance;
                db.GetReference ("users").OrderByChild("timeStamp").LimitToFirst (10).GetValueAsync ().ContinueWithOnMainThread (task => {
                        if (task.IsFaulted) {
                                 
                        } else if (task.IsCompleted) {
                                DataSnapshot snapshot = task.Result;
                                foreach (DataSnapshot user in snapshot.Children) {
                                        IDictionary dictUser = (IDictionary)user.Value;
                                        Debug.Log ( dictUser ["bio"]);

                                        GameObject userBar = Instantiate (prefab, transform.position, transform.rotation);
                                        userBar.transform.SetParent (userBarParent, false);
                                        userBar.transform.GetChild (0).GetComponent<Text> ().text = "Name: "+dictUser ["name"];
                                        userBar.transform.GetChild (1).GetComponent<Text> ().text = "Bio: "+dictUser ["bio"];
                                }
                        }
                });
        }

        void HandleChildAdded (object sender, ChildChangedEventArgs args)
        {
                if (args.DatabaseError != null) {
                        Debug.LogError (args.DatabaseError.Message);
                        return;
                }
                // This is fired when new child is added
                Debug.Log ("Child Added");
                eventListener.text = eventText;
        }

        void HandleChildChanged (object sender, ChildChangedEventArgs args)
        {
                if (args.DatabaseError != null) {
                        Debug.LogError (args.DatabaseError.Message);
                        return;
                }
                // This is fired when child gets updated

                Debug.Log ("Child Changed");
        }

        void HandleChildRemoved (object sender, ChildChangedEventArgs args)
        {
                if (args.DatabaseError != null) {
                        Debug.LogError (args.DatabaseError.Message);
                        return;
                }
                // This is fired when child gets removed

                Debug.Log ("Child Removed");
        }

        void HandleChildMoved (object sender, ChildChangedEventArgs args)
        {
                if (args.DatabaseError != null) {
                        Debug.LogError (args.DatabaseError.Message);
                        return;
                }
                // This is fired when child reorder (order-by query)

                Debug.Log ("Child Moved");
        }


        //Remove Listner OnDestroy

	private void OnDestroy ()
	{
                reference2.ChildAdded -= HandleChildAdded;
                reference2.ChildChanged -= HandleChildChanged;
                reference2.ChildRemoved -= HandleChildRemoved;
                reference2.ChildMoved -= HandleChildMoved;
        }
}
