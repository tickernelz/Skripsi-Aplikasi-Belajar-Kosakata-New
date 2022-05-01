using Firebase.Auth;
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
            signinSuccessNotification.OpenNotification();
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
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
                signupSuccessNotification.OpenNotification();
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
    }
    
    public void OnClickLogout()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        logoutNotification.OpenNotification();
        Debug.Log("Logout Successful");
    }
}