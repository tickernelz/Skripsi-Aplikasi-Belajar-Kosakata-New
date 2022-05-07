using System.Collections;
using Firebase.Auth;
using Firebase.Database;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string nextScene, previousScene, homeScene, menuScene;
    public float volumeNextScene, volumePreviousScene, volumeMenuScene;
    public Animator transition;
    public float transitionTime;
    public ModalWindowManager signUpWindow;
    private string _cekNama, _cekSekolah;
    
    public void StartGame()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            StartCoroutine(CekProfile(user.UserId));
        }
        else
        {
            signUpWindow.OpenWindow();
        }
    }
    public void HomeScene()
    {
        StartCoroutine(LoadLevel(homeScene, 1f));
    }
    public void NextScene()
    {
        StartCoroutine(LoadLevel(nextScene, volumeNextScene));
    }
    
    public void PreviousScene()
    {
        StartCoroutine(LoadLevel(previousScene, volumePreviousScene));
    }

    private IEnumerator CekProfile(string userId)
    {
        var dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        var dbTask = dbReference.Child("users").Child(userId).GetValueAsync();

        while (!dbTask.IsCompleted) yield return null;

        if (dbTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {dbTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            var snapshot = dbTask.Result;
            _cekNama = snapshot.Child("nama").Value.ToString();
            _cekSekolah = snapshot.Child("sekolah").Value.ToString();
            if (_cekNama != "" && _cekSekolah != "")
                yield return LoadLevel(menuScene, volumeMenuScene);
            else
                yield return LoadLevel(nextScene, volumeNextScene);
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
