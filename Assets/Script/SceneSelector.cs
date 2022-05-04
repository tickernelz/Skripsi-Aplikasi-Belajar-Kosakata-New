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
    
    public void NextScene()
    {
        StartCoroutine(LoadLevel(nextScene, volumeNextScene));
    }

    IEnumerator LoadLevel(string sceneName, float volume)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        PlayerPrefs.SetFloat("volume", volume);
    }
}
