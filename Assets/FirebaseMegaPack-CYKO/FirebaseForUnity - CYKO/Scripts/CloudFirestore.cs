using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using UnityEngine.UI;
using Firebase.Auth;
using System.Linq;
//using Firebase.Unity.Editor;
using Firebase;

[FirestoreData]
public class Users {

	[FirestoreProperty]
	public string name { get; set; }

	[FirestoreProperty]
	public string email { get; set; }

	[FirestoreProperty]
	public string bio { get; set; }

	[FirestoreProperty]
	public string timestamp { get; set; }
}

public class CloudFirestore : MonoBehaviour
{
	// Start is called before the first frame update

	FirebaseFirestore db;
	public InputField emailField;
	public InputField nameField;
	public InputField bioField;
	public Text msg;

	public static string UserEmail;
	public GameObject ErrorPanel;

	public InputField emailFieldUpdate;
	public InputField nameFieldUpdate;
	public InputField bioFieldUpdate;
	public Text msgUpdate;

	public InputField emailFieldFetch;
	public InputField nameFieldFetch;
	public InputField bioFieldFetch;
	public Text timeStamp;
	public Text msgFetch;


	public GameObject UpdatePanel;
	public GameObject ReadPanel;



	private void Awake ()
	{
		
	}


    void Start()
    {
		//FirebaseApp.DefaultInstance.SetEditorDatabaseUrl (Constant.FirebaseDatabaseURL);

		db = FirebaseFirestore.DefaultInstance;
    }

    
    void Update()
    {
		emailFieldUpdate.text = UserEmail;
    }

	public void AddNewEmail ()
	{
		msg.text = "";

		if (emailField.text.Length == 0) { msg.text = "Email is Missing !"; return; }
		if (nameField.text.Length == 0) { msg.text = "Name is Missing !"; return; }
		if (bioField.text.Length == 0) { msg.text = "Bio is Missing !"; return; }

		msg.text = "Adding Data ...";
		DocumentReference docRef = db.Collection ("user").Document (emailField.text);
		//DocumentReference docRef1 = db.Collection ("users").Document (auth.CurrentUser.UserId);

		docRef.GetSnapshotAsync ().ContinueWithOnMainThread (task => {
			DocumentReference nameRef = db.Collection ("users").Document (emailField.text);
			Dictionary<string, object> adddata = new Dictionary<string, object>
			{
					{ "name", nameField.text },
					{ "email", emailField.text },
					{ "bio", bioField.text },
					{ "timestamp", FieldValue.ServerTimestamp }
				};

			nameRef.SetAsync (adddata).ContinueWithOnMainThread (task2 => {


				if (task2.IsCanceled) { msg.text = "An Error Occurred !"; return; }
				if (task2.IsFaulted) { msg.text = "Add Data Failed Failed !"; return; }
				if (task2.IsCompleted) {

					msg.text = "New Data Added, Now You can read and update data using: " + emailField.text;
					UserEmail = emailField.text;
				}
			});
		});
	}

	public void UpdateButtonClick()
	{
		if(UserEmail != null)
		{
			emailFieldUpdate.text = UserEmail;
			UpdatePanel.SetActive (true);
		} else
		{
			ErrorPanel.SetActive (true);
		}
		
	}

	public void ReadButtonClick ()
	{
		if (UserEmail != null) {
			emailFieldFetch.text = UserEmail;
			ReadPanel.SetActive (true);
		} else {
			ErrorPanel.SetActive (true);
		}

	}


	public void UpdateEmailData ()
	{
		msgUpdate.text = "Updating Data ...";

		if (emailFieldUpdate.text.Length == 0) { msgUpdate.text = "Email is Missing !"; return; }

		
		DocumentReference docRef = db.Collection ("user").Document (emailFieldUpdate.text);
		//DocumentReference docRef1 = db.Collection ("users").Document (auth.CurrentUser.UserId);

		docRef.GetSnapshotAsync ().ContinueWithOnMainThread (task => {
			DocumentReference nameRef = db.Collection ("users").Document (emailFieldUpdate.text);
			Dictionary<string, object> adddata = new Dictionary<string, object>
			{
					{ "name", nameFieldUpdate.text },
					{ "bio", bioFieldUpdate.text }
				};

			nameRef.UpdateAsync (adddata).ContinueWithOnMainThread (task2 => {


				if (task2.IsCanceled) { msgUpdate.text = "An Error Occurred !"; return; }
				if (task2.IsFaulted) { msgUpdate.text = "Add Data Failed Failed !"; return; }
				if (task2.IsCompleted) {

					msgUpdate.text = "Data Updated for: " + UserEmail;
				}
			});
		});
	}


	public void FetchEmaiData ()
	{
		msgFetch.text = "Fetching data for: " + UserEmail;

		DocumentReference docRef = db.Collection ("users").Document (emailFieldFetch.text);
		docRef.GetSnapshotAsync ().ContinueWithOnMainThread (task => {
			DocumentSnapshot snapshot = task.Result;
			Dictionary<string, object> data = snapshot.ToDictionary ();
			foreach (KeyValuePair<string, object> pair in data) {



				if (pair.Key == "bio") {
					bioFieldFetch.text = (string)pair.Value;
				}

				if (pair.Key == "name") {
					nameFieldFetch.text = (string)pair.Value;
				}
				if (pair.Key == "timestamp") {
					timeStamp.text = (string)pair.Value;

				}

				msgFetch.text = "Data Fetched!";

			}
		});
	}
}
