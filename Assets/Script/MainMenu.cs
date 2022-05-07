using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button signupButton, loginButton, logoutButton;

    public void Update()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            signupButton.gameObject.SetActive(false);
            loginButton.gameObject.SetActive(false);
            logoutButton.gameObject.SetActive(true);
            string name = user.DisplayName;
            string email = user.Email;
            System.Uri photo_url = user.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = user.UserId;
        }
        else
        {
            signupButton.gameObject.SetActive(true);
            loginButton.gameObject.SetActive(true);
            logoutButton.gameObject.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}