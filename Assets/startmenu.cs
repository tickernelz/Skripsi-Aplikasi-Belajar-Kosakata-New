using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startmenu : MonoBehaviour
{
    public void exit() {
        Application.Quit();
    }

    public void play(string scene_name) {
        Application.LoadLevel(scene_name);
    }

    public void sound_volume(float volume) {
        PlayerPrefs.SetFloat("volume",volume);    
    }
}
