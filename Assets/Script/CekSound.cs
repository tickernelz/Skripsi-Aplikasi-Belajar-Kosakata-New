using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CekSound : MonoBehaviour
{
    public GameObject[] tombolJawaban;
    void Start()
    {
        foreach (GameObject go in tombolJawaban)
        {
            PlayerPrefs.SetInt(go.GetInstanceID().ToString(), 0);
        }
    }
    
    public void Benar(GameObject button)
    {
        string id = button.GetInstanceID().ToString();
        AudioSource audio = button.GetComponent<AudioSource>();
        int value = PlayerPrefs.GetInt(id);
        if (value < 2)
        {
            value++;
            PlayerPrefs.SetInt(id, value);
            audio.Play();
            Debug.Log("Value :" + value);
        }
        else
        {
            LatihanNilai latihanNilai = GameObject.Find("Canvas").GetComponent<LatihanNilai>();
            latihanNilai.Benar();
            foreach (GameObject go in tombolJawaban)
            {
                PlayerPrefs.DeleteKey(go.GetInstanceID().ToString());
            }
        }
    }
    
    public void Salah(GameObject button)
    {
        string id = button.GetInstanceID().ToString();
        AudioSource audio = button.GetComponent<AudioSource>();
        int value = PlayerPrefs.GetInt(id);
        if (value < 2)
        {
            value++;
            PlayerPrefs.SetInt(id, value);
            audio.Play();
            Debug.Log("Value :" + value);
        }
        else
        {
            LatihanNilai latihanNilai = GameObject.Find("Canvas").GetComponent<LatihanNilai>();
            latihanNilai.Salah();
            foreach (GameObject go in tombolJawaban)
            {
                PlayerPrefs.DeleteKey(go.GetInstanceID().ToString());
            }
        }
    }
}
