using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public Text logText;
    public Button signInButton, signUpButton;
    private GameObject email, password;

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
        email = GameObject.Find("Signup Modal/Content/Content/Email");
        password = GameObject.Find("Signup Modal/Content/Content/Password");
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email.GetComponent<TMP_InputField>().text,
            password.GetComponent<TMP_InputField>().text).ContinueWith(task =>
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

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }


    public void OnClickSignUp()
    {
        Debug.Log("Clicked SignUp");
        email = GameObject.Find("Signup Modal/Content/Content/Email");
        password = GameObject.Find("Signup Modal/Content/Content/Password");
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email.GetComponent<TMP_InputField>().text,
                password.GetComponent<TMP_InputField>().text)
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
                    return;
                }

                // Firebase user has been created.
                Debug.Log("Signup Successful");
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
    }
}