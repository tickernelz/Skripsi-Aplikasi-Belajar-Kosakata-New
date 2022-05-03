using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;

public class ManageUserPost : MonoBehaviour
{
	FirebaseAuth auth;
	FirebaseUser user;
	FirebaseFirestore db;
	public Transform PostParent;
	public GameObject postPrefab;
	public InputField textInput;
	bool signedIn;
	public Button postBtn;
	public GameObject loadingText;
	public GameObject loadingPost;

	private void Awake ()
	{
		InitializeFirebase ();
	}

	// Start is called before the first frame update
	void Start()
	{

		
	}

	void InitializeFirebase ()
	{
		Debug.Log ("Setting up Firebase Auth");
		auth = FirebaseAuth.DefaultInstance;
		db = FirebaseFirestore.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged (this, null);
	}

	// Track state changes of the auth object.
	void AuthStateChanged (object sender, System.EventArgs eventArgs)
	{
		if (auth.CurrentUser != user) {
			signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null) {
				Debug.Log ("Signed out " + user.UserId);
			}
			user = auth.CurrentUser;
			if (signedIn) {
				Debug.Log ("Signed in " + user.UserId);
				GetPost ();
			}
		}
	}



	public void VerifyInput()
	{
		postBtn.interactable = textInput.text.Length > 1;
	}

	string uname;
	int alllikes;
	string likes = "0";

	public void GetPost ()
	{
		if (PostParent.childCount > 0) {
			foreach (Transform child in PostParent.transform) {
				Destroy (child.gameObject);
			}
		}

		Firebase.Firestore.Query allUsersQuery = db.Collection ("post").OrderByDescending ("timestamp").Limit (20);
		//Firebase.Firestore.Query allUsersQuery = db.Collection ("post");
		allUsersQuery.GetSnapshotAsync ().ContinueWithOnMainThread (task => {
			QuerySnapshot allUsersQuerySnapshot = task.Result;
			foreach (DocumentSnapshot documentSnapshot in allUsersQuerySnapshot.Documents) {
				//Debug.Log (String.Format ("Document data for {0} document:", documentSnapshot.Id));
				Dictionary<string, object> user = documentSnapshot.ToDictionary ();
				foreach (KeyValuePair<string, object> pair in user) {
					Debug.Log (String.Format ("{0}: {1}", pair.Key, pair.Value));

					if (pair.Key == "likes") {

						likes = (string)pair.Value;
					}

					if (pair.Key == "name") {

						uname = (string)pair.Value;

					}


					if (pair.Key == "text") {

						GameObject tBar = Instantiate (postPrefab.gameObject, transform.position, transform.rotation);
						tBar.transform.SetParent (PostParent, false);
						tBar.transform.GetChild (1).GetComponent<Text> ().text = uname;
						tBar.gameObject.name = documentSnapshot.Id;
						tBar.GetComponent<GetManageLikes> ().id = documentSnapshot.Id;
						tBar.transform.GetChild (2).GetComponent<Text> ().text = (string)pair.Value;
						tBar.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = "Likes ("+likes+")";
						StartCoroutine (LoadImage (auth.CurrentUser.PhotoUrl.ToString(), tBar.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Image> ()));
					}


					if(PostParent.childCount > 0) { loadingText.SetActive (false); }

				}
			}

			
		});
	}


	public void CreatePost ()
	{
		loadingPost.SetActive (true);
		Dictionary<string, object> postDoc = new Dictionary<string, object>
		{
			{ "name", auth.CurrentUser.DisplayName },
			{ "text", textInput.text },
			{ "timestamp", FieldValue.ServerTimestamp },
			{ "likes", "0" }
		};
		db.Collection ("post").AddAsync (postDoc).ContinueWithOnMainThread (task => {
			DocumentReference addedDocRef = task.Result;
			Debug.Log (String.Format ("Added document with ID: {0}.", addedDocRef.Id));

			GameObject tBar = Instantiate (postPrefab.gameObject, transform.position, transform.rotation);
			tBar.transform.SetParent (PostParent, false);
			tBar.transform.GetChild (1).GetComponent<Text> ().text = auth.CurrentUser.DisplayName;
			tBar.gameObject.name = addedDocRef.Id;
			tBar.GetComponent<GetManageLikes> ().id = addedDocRef.Id;
			tBar.transform.GetChild (2).GetComponent<Text> ().text = textInput.text;
			tBar.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = "Likes (0)";
			tBar.transform.SetAsFirstSibling ();
			textInput.text = "";
			StartCoroutine (LoadImage (auth.CurrentUser.PhotoUrl.ToString (), tBar.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Image> ()));
			loadingPost.SetActive (false);
		});

	}

	IEnumerator LoadImage (string url, Image img)
	{
		WWW www = new WWW (url);
		yield return www;

		if (www.error == null) {
			Texture2D textur = www.texture;
			Vector2 pivot = new Vector2 (0.5f, 0.5f);
			Sprite sprite = Sprite.Create (textur, new Rect (0.0f, 0.0f, textur.width, textur.height), pivot, 100.0f);
			if (img) { img.sprite = sprite; }

		}
	}

	string lk;

	public void ManageLikes(string id)
	{
		Debug.Log (id);
		alllikes = 0;
		DocumentReference docRef = db.Collection ("post").Document (id);
		docRef.GetSnapshotAsync ().ContinueWithOnMainThread (task =>
		{
			DocumentSnapshot snapshot = task.Result;
			if (snapshot.Exists) {
				Debug.Log (String.Format ("Document data for {0} document:", snapshot.Id));
				Dictionary<string, object> likesC = snapshot.ToDictionary ();
				foreach (KeyValuePair<string, object> pair in likesC) {
					Debug.Log (String.Format ("{0}: {1}", pair.Key, pair.Value));

					if (pair.Key == "likes") {

						lk = (string)pair.Value;

						alllikes = int.Parse (lk);
						alllikes++;

						docRef.UpdateAsync ("likes", alllikes.ToString())
							.ContinueWithOnMainThread (task2 => {
								Debug.Log ("Updated!");
								GameObject.Find(id).transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = "Likes (" + alllikes + ")";
							});

					}

				}
			}

		});
	}
}
