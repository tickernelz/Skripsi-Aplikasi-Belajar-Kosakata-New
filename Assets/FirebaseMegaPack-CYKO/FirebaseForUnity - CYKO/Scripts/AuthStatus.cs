using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using System;
using Firebase.Extensions;
using System.Collections;


public class AuthStatus : MonoBehaviour
{

	FirebaseUser user;
	FirebaseAuth auth;
	bool signedIn;
	public Text msg;
	public GameObject err;

	private void Awake ()
	{
		InitializeFirebase ();
	}

	void InitializeFirebase ()
	{
		Debug.Log ("Setting up Firebase Auth");
		auth = FirebaseAuth.DefaultInstance;
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

			}
		}
	}

	public void GotoSocial()
	{
		if(signedIn)
		{
			if(auth.CurrentUser.DisplayName == "")

			{
				err.SetActive (true);
				msg.text = "Name or Photo not found! \nGo to Authentication and Update Name and PhotoUrl.";
			} else
			{
				SceneManager.LoadScene ("MiniSocial");
			}

		} else
		{
			err.SetActive (true);
			msg.text = "USER NOT SIGNED IN! \nGo to Authentication and Signup / Login";
		}
	}


}
