using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public NotificationManager signupFailNotification,
        signupSuccessNotification,
        signinFailNotification,
        signinSuccessNotification,
        logoutNotification;

    public GameObject emailSignup, passwordSignup, emailSignin, passwordSignin;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                English Is Fun = Firebase.FirebaseApp.DefaultInstance;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                
            }
        });
    }

    public void OnClickSignIn()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        Debug.Log("Clicked SignIn");
        string emailText = emailSignin.GetComponent<TMP_InputField>().text;
        string passwordText = passwordSignin.GetComponent<TMP_InputField>().text;
        auth.SignInWithEmailAndPasswordAsync(emailText,
            passwordText).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("SignIn Canceled");
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.Log("SignIn Failed");
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                signinFailNotification.OpenNotification();
                return;
            }

            FirebaseUser newUser = task.Result;
            if (newUser != null)
            {
                signinSuccessNotification.OpenNotification();
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            }
        });
    }


    public void OnClickSignUp()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        Debug.Log("Clicked SignUp");
        string emailText = emailSignup.GetComponent<TMP_InputField>().text;
        string passwordText = passwordSignup.GetComponent<TMP_InputField>().text;
        auth.CreateUserWithEmailAndPasswordAsync(emailText,
                passwordText)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("Signup Canceled");
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.Log("Signup Failed");
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    signupFailNotification.OpenNotification();
                    return;
                }

                // Firebase user has been created.
                Debug.Log("Signup Successful");
                FirebaseUser newUser = task.Result;
                if (newUser != null)
                {
                    writeNewUser(newUser.UserId,newUser.Email);
                    signupSuccessNotification.OpenNotification();
                    Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);
                }
            });
    }
    
    public class User {
        public string email;

        public User(string email) {
            this.email = email;
        }
    }
    
    private void writeNewUser(string userId, string email) {
        DatabaseReference mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        User user = new User(email);
        string json = JsonUtility.ToJson(user);
        mDatabaseRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
        mDatabaseRef.Child("users").Child(userId).Child("nama").SetValueAsync("");
        mDatabaseRef.Child("users").Child(userId).Child("sekolah").SetValueAsync("");
        mDatabaseRef.Child("users").Child(userId).Child("Latihan1Bab1").SetValueAsync(0);
        mDatabaseRef.Child("users").Child(userId).Child("Latihan2Bab1").SetValueAsync(0);
        mDatabaseRef.Child("users").Child(userId).Child("Latihan1Bab2").SetValueAsync(0);
        mDatabaseRef.Child("users").Child(userId).Child("Latihan2Bab2").SetValueAsync(0);
        mDatabaseRef.Child("users").Child(userId).Child("Latihan1Bab3").SetValueAsync(0);
        mDatabaseRef.Child("users").Child(userId).Child("Latihan2Bab3").SetValueAsync(0);
        mDatabaseRef.Child("users").Child(userId).Child("TotalSkor").SetValueAsync(0);
    }
    
    public void OnClickLogout()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        logoutNotification.OpenNotification();
        Debug.Log("Logout Successful");
    }
}