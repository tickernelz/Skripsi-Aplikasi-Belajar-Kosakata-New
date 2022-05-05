using System;
using System.Collections;
using Michsky.UI.ModernUIPack;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LatihanNilai : MonoBehaviour
{
    public GameObject skorObject, nilaiObject;
    public GameObject[] soal;
    public NotificationManager salahNotification, benarNotification;
    public float timeLeft = 4.0f;
    private int _salahCek, _stepsSoal;
    private float _nilai, _skorSoal;
    private bool benar, _selesai;

    private void Start()
    {
        PlayerPrefs.SetInt("NilaiBab1Latihan1", 0);
        PlayerPrefs.SetInt("SalahCek", 0);
        PlayerPrefs.SetInt("StepsSoal", 0);
        _skorSoal = 100f / soal.Length;
    }

    private void Update()
    {
        if (_selesai)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft < 0)
            {
                LevelLoader levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
                levelLoader.PreviousScene();
            }
        }
    }

    public void Benar()
    {
        benar = true;
        CekNilai();
    }
    
    public void Salah()
    {
        benar = false;
        CekNilai();
    }

    private void CekNilai()
    {
        if (benar)
        {
            _nilai = PlayerPrefs.GetFloat("NilaiBab1Latihan1") + _skorSoal;
            PlayerPrefs.SetFloat("NilaiBab1Latihan1", _nilai);
            benarNotification.OpenNotification();
            StartCoroutine(LanjutSoal());
        }
        else
        {
            _salahCek = PlayerPrefs.GetInt("SalahCek");
            if (_salahCek == 1)
            {
                StartCoroutine(LanjutSoal());
            }
            else
            {
                _salahCek += 1;
                salahNotification.OpenNotification();
                PlayerPrefs.SetInt("SalahCek", _salahCek);
            }
        }
    }
    
    IEnumerator LanjutSoal()
    {
        _stepsSoal = PlayerPrefs.GetInt("StepsSoal");
        if (_stepsSoal == soal.Length - 1)
        {
            _nilai = PlayerPrefs.GetFloat("NilaiBab1Latihan1");
            nilaiObject.GetComponent<TMP_Text>().text = _nilai.ToString();
            skorObject.SetActive(true);
            _selesai = true;
            yield return this;
        }
        else
        {
            soal[_stepsSoal].SetActive(false);
            _stepsSoal++;
            PlayerPrefs.SetInt("StepsSoal", _stepsSoal);
            soal[_stepsSoal].SetActive(true);
            PlayerPrefs.SetInt("SalahCek", 0);
            yield return this;
        }
    }
}
