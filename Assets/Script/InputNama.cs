using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputNama : MonoBehaviour
{
    public string sceneName;
    public float volume;
    private string nama, sekolah;
    private GameObject nama_obj, sekolah_obj;
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
        PlayerPrefs.SetFloat("volume", volume);
    }
    public void SaveInput()
    {
        nama_obj = GameObject.Find("Field/Nama");
        if (nama_obj != null)
        {
            nama = nama_obj.GetComponent<TMP_InputField>().text;
            PlayerPrefs.SetString("nama", nama);
        }
        sekolah_obj = GameObject.Find("Field/Sekolah");
        if (sekolah_obj != null)
        {
            sekolah = sekolah_obj.GetComponent<TMP_InputField>().text;
            PlayerPrefs.SetString("sekolah", sekolah);
        }
    }
    public void Exit() {
        Application.Quit();
    }
}
