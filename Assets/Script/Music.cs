using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource audioSource;
    public float volume;
    public GameObject soundon, soundoff;
    private static Music instance = null;
    public static Music Instance
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
        volume = PlayerPrefs.GetFloat("volume", 1f);
        audioSource.volume = volume;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        soundon = GameObject.Find("Sound On");
        soundoff = GameObject.Find("Sound Off");
        volume = PlayerPrefs.GetFloat("volume", 1f);
        AudioListener.volume = volume;
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
