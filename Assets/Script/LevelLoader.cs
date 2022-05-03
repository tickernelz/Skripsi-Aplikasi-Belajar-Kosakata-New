using System.Collections;
using Firebase.Auth;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string sceneName;
    public float volume;
    public Animator transition;
    public float transitionTime;
    public ModalWindowManager signUpWindow;
    
    public void StartGame()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            StartCoroutine(LoadLevel());
        }
        else
        {
            signUpWindow.OpenWindow();
        }
    }
    public void ChangeScene()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        PlayerPrefs.SetFloat("volume", volume);
    }
}
