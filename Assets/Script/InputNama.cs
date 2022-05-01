using TMPro;
using UnityEngine;

public class InputNama : MonoBehaviour
{
    private string nama, sekolah;
    private GameObject nama_obj, sekolah_obj;
    
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
