using System.Collections;
using Firebase.Auth;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string nextScene, previousScene;
    public float volumeNextScene, volumePreviousScene;
    public Animator transition;
    public float transitionTime;
    public ModalWindowManager signUpWindow;
    
    public void StartGame()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            StartCoroutine(LoadLevel(nextScene, volumeNextScene));
        }
        else
        {
            signUpWindow.OpenWindow();
        }
    }
    public void NextScene()
    {
        StartCoroutine(LoadLevel(nextScene, volumeNextScene));
    }
    
    public void PreviousScene()
    {
        StartCoroutine(LoadLevel(previousScene, volumePreviousScene));
    }

    IEnumerator LoadLevel(string sceneName, float volume)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        PlayerPrefs.SetFloat("volume", volume);
    }
}
