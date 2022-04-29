using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backsound : MonoBehaviour
{
    public AudioSource audioSource;
    public float volume;
    public GameObject soundon, soundoff;
    private static backsound instance = null;
    public static backsound Instance
    {
        get { return instance; }
    }
    
    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.Play();
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