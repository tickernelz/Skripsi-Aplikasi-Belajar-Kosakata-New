using Firebase.Auth;
using Firebase.Extensions;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public NotificationManager signupFailNotification;
    public GameObject email, password;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Firebase.DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }


    public void OnClickSignIn()
    {
        Debug.Log("Clicked SignIn");
        string emailText = email.GetComponent<TMP_InputField>().text;
        string passwordText = password.GetComponent<TMP_InputField>().text;
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailText,
            passwordText).ContinueWith(task =>
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
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }


    public void OnClickSignUp()
    {
        Debug.Log("Clicked SignUp");
        string emailText = email.GetComponent<TMP_InputField>().text;
        string passwordText = password.GetComponent<TMP_InputField>().text;
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailText,
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
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
    }
}