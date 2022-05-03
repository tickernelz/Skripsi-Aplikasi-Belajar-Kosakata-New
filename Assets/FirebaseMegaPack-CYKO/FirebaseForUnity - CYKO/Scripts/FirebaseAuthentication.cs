using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using System;
using Firebase.Extensions;
using System.Collections;

public class FirebaseAuthentication : MonoBehaviour {

	FirebaseUser user;
	FirebaseAuth auth;
	public InputField email;
	public InputField password;
	public InputField loginemail;
	public InputField loginpassword;
	public Text message;
	public Text messageLogin;
	public Text messageUpdate;
	string msg = "";
	public GameObject ProfileObject;
	public GameObject ProfilePanel;
	public GameObject LoginPanel;
	public GameObject SignupPanel;
	public GameObject ErrorPanel;

	public InputField nameUpdate;
	public InputField photoUpdate;
	public Image profilePic;

	public InputField emailUpdateInput;
	public Text emailUpdateMsg;

	public InputField passwordUpdateInput;
	public Text passwordUpdateMsg;

	public GameObject MsgPanel;
	public Text msgPanelText;

	bool signedIn;


	void Awake ()
	{
		InitializeFirebase ();
		photoUpdate.text = "https://loremflickr.com/400/400";
	}

	void Start ()
	{
		
	}


	public void SignupUsingEmail ()
	{
		msg = "Creating User Account...";
		message.text = msg;
		auth.CreateUserWithEmailAndPasswordAsync (email.text, password.text).ContinueWithOnMainThread (task => {
			if (task.IsCanceled) {
				msg = "Create User With Email And Password was canceled.";
				return;
			}
			if (task.IsFaulted) {

				if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.EmailAlreadyInUse)) {
					msg = "Email is already in use";
				} else if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.AccountExistsWithDifferentCredentials)) {
					msg = "Account Exists With Different Credentials";
				} else if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.InvalidEmail)) {
					msg = "Invalid Email";
				} else if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.MissingPassword)) {
					msg = "Password is Missing";
				} else if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.MissingEmail)) {
					msg = "Email is Missing";
				} else {
					msg = "Something went wrong, Try again later.";
				}

				return;
			}

			PlayerPrefs.SetString ("password", password.text);
			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat ("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
			msg = "Account Created, Your UID: "+newUser.UserId;
			ProfilePanel.SetActive (true);
			SignupPanel.SetActive (false);

		});
	}

	public void LoginUsingEmail()
	{
		msg = "Loging In User Account...";
		message.text = msg;
		auth.SignInWithEmailAndPasswordAsync (loginemail.text, loginpassword.text).ContinueWithOnMainThread (task => {
			if (task.IsCanceled) {
				msg = "SignIn With Email And Password Async was canceled.";
				return;
			}
			if (task.IsFaulted) {

				if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.WrongPassword)) {
					msg = "Password is Wrong";
				} else if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.AccountExistsWithDifferentCredentials)) {
					msg = "Account Exists With Different Credentials";
				} else if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.InvalidEmail)) {
					msg = "Invalid Email";
				} else if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.MissingPassword)) {
					msg = "Password is Missing";
				} else if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.MissingEmail)) {
					msg = "Email is Missing";
				} else if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.UserNotFound)) {
					msg = "User Not Found";
				} else if (CheckError (task.Exception, (int)Firebase.Auth.AuthError.TooManyRequests)) {
					msg = "Too Many Requests, Please wait for 60 Seconds";
				} else {
					msg = "Something went wrong, Try again later.";
				}

				return;
			}

			PlayerPrefs.SetString ("password", loginpassword.text);
			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat ("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
			msg = "Account Logged In, Your UID: " + newUser.UserId;
			ProfilePanel.SetActive (true);
			LoginPanel.SetActive (false);
		});
	}


	

	bool CheckError (AggregateException exception, int firebaseExceptionCode)
	{
		Firebase.FirebaseException fbEx = null;
		foreach (Exception e in exception.Flatten ().InnerExceptions) {
			fbEx = e as Firebase.FirebaseException;
			if (fbEx != null)
				break;
		}

		if (fbEx != null) {
			if (fbEx.ErrorCode == firebaseExceptionCode) {
				return true;
			} else {
				return false;
			}
		}
		return false;
	}

	private void Update ()
	{
		if(msg != "")
		{
		   message.text = msg;
		   messageLogin.text = msg;
		}
		
	}

	// Handle initialization of the necessary firebase modules:
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

				if(user.PhotoUrl != null)
				{
					StartCoroutine (LoadProfileImage (user.PhotoUrl.ToString(), profilePic));
				}


				if(ProfileObject)
				{
					ProfileObject.transform.GetChild (0).GetComponent<Text> ().text = "NAME: " + user.DisplayName;
					ProfileObject.transform.GetChild (1).GetComponent<Text> ().text = "EMAIL: " + user.Email;
					ProfileObject.transform.GetChild (2).GetComponent<Text> ().text = "UID: " + user.UserId;
				}
				
			}
		}
	}

	public void OpenProfilePanel()
	{
		if(signedIn)
		{
			ProfilePanel.SetActive (true);
		} else { ErrorPanel.SetActive (true); }
	}

	public void Logout()
	{

		auth.SignOut ();
		SceneManager.LoadScene ("FirebaseAuth");
	}

	public void UpdateInfo()
	{
		messageUpdate.text = "Updating Info...";
		FirebaseUser user = auth.CurrentUser;
		if (user != null) {
			UserProfile profile = new UserProfile {
				DisplayName = nameUpdate.text,
				PhotoUrl = new Uri (photoUpdate.text)
			};
			user.UpdateUserProfileAsync (profile).ContinueWithOnMainThread (task => {
				if (task.IsCanceled) {
					messageUpdate.text = "UpdateUserProfileAsync was canceled.";
					return;
				}
				if (task.IsFaulted) {
					messageUpdate.text = "UpdateUserProfileAsync encountered an error: " + task.Exception;
					return;
				}

				messageUpdate.text = "User profile updated successfully.";
				Debug.Log ("User profile updated successfully.");
				StartCoroutine (LoadProfileImage (photoUpdate.text, profilePic));

				if (ProfileObject) {
					ProfileObject.transform.GetChild (0).GetComponent<Text> ().text = "NAME: " + user.DisplayName;
					ProfileObject.transform.GetChild (1).GetComponent<Text> ().text = "EMAIL: " + user.Email;
					ProfileObject.transform.GetChild (2).GetComponent<Text> ().text = "UID: " + user.UserId;
				}
			});
		}
	}

	IEnumerator LoadProfileImage (string url, Image img)
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

	public void OpenPhotoLink()
	{
		if(auth.CurrentUser.PhotoUrl != null)
		{
			Application.OpenURL (auth.CurrentUser.PhotoUrl.ToString ());
		}
	}

	public void ReAuthEmailPassword(string type)
	{
		emailUpdateMsg.text = "Re-authenticating user...";
		passwordUpdateMsg.text = "Re-authenticating user...";

		if(type == "Delete")
		{
			MsgPanel.SetActive (true);
			msgPanelText.text = "Deleting User...";
		}


		FirebaseUser user = auth.CurrentUser;

		Credential credential = EmailAuthProvider.GetCredential (user.Email, PlayerPrefs.GetString("password").ToString());

		if (user != null) {
			user.ReauthenticateAsync (credential).ContinueWithOnMainThread (task => {
				if (task.IsCanceled) {
					emailUpdateMsg.text = "ReauthenticateAsync was canceled.";
					passwordUpdateMsg.text = "ReauthenticateAsync was canceled.";
					msgPanelText.text = "ReauthenticateAsync was canceled.";


					return;
				}
				if (task.IsFaulted) {
					emailUpdateMsg.text = "ReauthenticateAsync encountered an error: " + task.Exception;
					passwordUpdateMsg.text = "ReauthenticateAsync encountered an error: " + task.Exception;
					msgPanelText.text = "ReauthenticateAsync encountered an error: " + task.Exception;
					return;
				}

				

				if(type == "Email")
				{
					emailUpdateMsg.text = "User reauthenticated successfully, Updating Email...";
					UpdateEmail ();
				}

				if (type == "Password") {

					passwordUpdateMsg.text = "User reauthenticated successfully, Updating Password...";
					UpdatePassword ();
				}

				if (type == "Delete") {

					msgPanelText.text = "User reauthenticated successfully, Deleting User...";
					DeleteUser ();
				}
			});
		}
	}


	public void UpdateEmail()
	{
		FirebaseUser user = auth.CurrentUser;
		if (user != null) {
			user.UpdateEmailAsync (emailUpdateInput.text).ContinueWithOnMainThread (task => {
				if (task.IsCanceled) {
					emailUpdateMsg.text = "UpdateEmailAsync was canceled.";
					return;
				}
				if (task.IsFaulted) {
					emailUpdateMsg.text = "UpdateEmailAsync encountered an error: " + task.Exception;
					return;
				}

				emailUpdateMsg.text = "User email updated successfully.";
				ProfileObject.transform.GetChild (1).GetComponent<Text> ().text = "EMAIL: " + user.Email;

			});
		}
	}

	public void SendVerificationEmail()
	{
		MsgPanel.SetActive (true);

		FirebaseUser user = auth.CurrentUser;
		msgPanelText.text = "Sending Verification Email on: " + user.Email;

		if (user != null) {
			user.SendEmailVerificationAsync ().ContinueWithOnMainThread (task => {
				if (task.IsCanceled) {
				msgPanelText.text = "SendEmailVerificationAsync was canceled.";
					return;
				}
				if (task.IsFaulted) {
					msgPanelText.text = "SendEmailVerificationAsync encountered an error: " + task.Exception;
					return;
				}

				Debug.Log ("Email sent successfully.");
				msgPanelText.text = "Email sent successfully to" + user.Email;

			});
		}
	}

	public void UpdatePassword()
	{
		FirebaseUser user = auth.CurrentUser;
		string newPassword = passwordUpdateInput.text;
		if (user != null) {
			user.UpdatePasswordAsync (newPassword).ContinueWithOnMainThread (task => {
				if (task.IsCanceled) {
				passwordUpdateMsg.text = "Update Password Async was canceled.";
					return;
				}
				if (task.IsFaulted) {
					passwordUpdateMsg.text = " Update Password Async encountered an error: " + task.Exception;
					return;
				}

				passwordUpdateMsg.text = "Password updated successfully.";
				PlayerPrefs.SetString ("password", newPassword);
			});
		}
	}

	public void GetPasswordResetEmail()
	{
		MsgPanel.SetActive (true);
		
		FirebaseUser user = auth.CurrentUser;
		msgPanelText.text = "Sending Reset Email on: " +user.Email;
		if (user != null) {
			auth.SendPasswordResetEmailAsync (user.Email).ContinueWithOnMainThread (task => {
				if (task.IsCanceled) {
					msgPanelText.text = "SendPasswordResetEmailAsync was canceled.";
					return;
				}
				if (task.IsFaulted) {
					msgPanelText.text = "SendPasswordResetEmailAsync encountered an error: " + task.Exception;
					return;
				}


				msgPanelText.text = "Password reset email sent successfully to " + user.Email;
			});
		}
	}

	public void DeleteUser()
	{
		
		if (user != null) {
			user.DeleteAsync ().ContinueWithOnMainThread (task => {
				if (task.IsCanceled) {
				msgPanelText.text = "DeleteAsync was canceled.";
					return;
				}
				if (task.IsFaulted) {
					msgPanelText.text = "DeleteAsync encountered an error: " + task.Exception;
					return;
				}

				Debug.Log ("User deleted successfully.");
				msgPanelText.text = "User deleted successfully.";
				SceneManager.LoadScene ("FirebaseAuth");
			});
		}
	}
}

