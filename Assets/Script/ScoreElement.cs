using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour
{

    public TMP_Text namaText;
    public TMP_Text sekolahText;
    public TMP_Text skorBab1Text;
    public TMP_Text skorBab2Text;

    public void NewScoreElement (string _nama, string _sekolah, float _skorBab1, float _skorBab2)
    {
        namaText.text = _nama;
        sekolahText.text = _sekolah;
        skorBab1Text.text = _skorBab1.ToString();
        skorBab2Text.text = _skorBab2.ToString();
    }

}