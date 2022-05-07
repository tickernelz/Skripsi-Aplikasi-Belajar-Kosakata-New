using System.Collections;
using Firebase.Auth;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public string nextScene;
    public float volumeNextScene = 0.5f;
    public Animator transition;
    public float transitionTime = 1f;
    public bool profileScene;
    public ModalWindowManager signUpWindow;
    
    public void NextScene()
    {
        if (profileScene)
        {
            var auth = FirebaseAuth.DefaultInstance;
            var user = auth.CurrentUser;
            if (user != null)
                StartCoroutine(LoadLevel(nextScene, volumeNextScene));
            else
                signUpWindow.OpenWindow();
        }
        else
        {
            StartCoroutine(LoadLevel(nextScene, volumeNextScene));
        }
    }

    private IEnumerator LoadLevel(string sceneName, float volume)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        PlayerPrefs.SetFloat("volume", volume);
    }
}
