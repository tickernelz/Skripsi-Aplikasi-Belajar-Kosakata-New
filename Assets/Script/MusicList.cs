using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicList : MonoBehaviour
{
    public AudioClip[] musicList;

    public int audioId;

    public AudioSource audioSource;


    private void Start()
    {
        audioSource.clip = musicList[audioId];
        audioSource.Play();
    }


    void Update()
    {
        AutoChangeMusic();
    }

    public void AutoChangeMusic()
    {
        if (audioId == musicList.Length - 1)
            return;
        
        if (!audioSource.isPlaying)
        {
            audioId++;

            audioSource.clip = musicList[audioId];
            audioSource.Play();
        }
    }
}
