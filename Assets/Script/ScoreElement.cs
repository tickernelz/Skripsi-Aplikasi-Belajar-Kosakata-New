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
    public TMP_Text skorBab3Text;
    public TMP_Text totalSkorText;

    public void NewScoreElement (string _nama, string _sekolah, float _skorBab1, float _skorBab2, float _skorBab3, float _totalSkor)
    {
        namaText.text = _nama;
        sekolahText.text = _sekolah;
        skorBab1Text.text = _skorBab1.ToString();
        skorBab2Text.text = _skorBab2.ToString();
        skorBab3Text.text = _skorBab3.ToString();
        totalSkorText.text = _totalSkor.ToString();
    }

}