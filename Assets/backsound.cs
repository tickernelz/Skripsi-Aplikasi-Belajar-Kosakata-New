using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backsound : MonoBehaviour
{
    public AudioSource audioSource;
    public float volume;
    public GameObject soundon, soundoff;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.Play();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        soundon = GameObject.Find("sound on");
        soundoff = GameObject.Find("sound off");
        if (soundoff)
        {
            audioSource.Pause();
        }
        else if (soundon)
        {
            audioSource.UnPause();
        }
    }
}