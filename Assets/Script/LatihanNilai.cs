using System;
using System.Collections;
using Firebase.Auth;
using Firebase.Database;
using Michsky.UI.ModernUIPack;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LatihanNilai : MonoBehaviour
{
    public string namaNilai;
    public bool timer;
    public float timeLeft = 4.0f, detikTimer = 20.0f;
    public NotificationManager salahNotification, benarNotification;
    public GameObject skorObject, nilaiObject, timerObject;
    public GameObject[] soal;
    private int _salahCek, _stepsSoal;
    private float _nilai, _skorSoal;
    private bool benar, _selesai;

    private void Start()
    {
        PlayerPrefs.SetFloat(namaNilai, 0);
        PlayerPrefs.SetInt("SalahCek", 0);
        PlayerPrefs.SetInt("StepsSoal", 0);
        _skorSoal = 100f / soal.Length;
    }

    private void Update()
    {
        if (timer && !_selesai)
        {
            detikTimer -= Time.deltaTime;
            timerObject.GetComponent<TMP_Text>().text = detikTimer.ToString("00");
            if(detikTimer < 0)
            {
                StartCoroutine(LanjutSoal());
                detikTimer = 20.0f;
            }
        }
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
            _nilai = PlayerPrefs.GetFloat(namaNilai) + _skorSoal;
            PlayerPrefs.SetFloat(namaNilai, _nilai);
            benarNotification.OpenNotification();
            StartCoroutine(LanjutSoal());
        }
        else
        {
            if (timer)
            {
                StartCoroutine(LanjutSoal());
            }
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
    
    private void WriteDataUser(string userId, float nilai)
    {
        DatabaseReference mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        mDatabaseRef.Child("users").Child(userId).Child(namaNilai).SetValueAsync(nilai);
    }
    
    IEnumerator LanjutSoal()
    {
        _stepsSoal = PlayerPrefs.GetInt("StepsSoal");
        if (_stepsSoal == soal.Length - 1)
        {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            FirebaseUser user = auth.CurrentUser;
            _nilai = PlayerPrefs.GetFloat(namaNilai);
            nilaiObject.GetComponent<TMP_Text>().text = _nilai.ToString();
            WriteDataUser(user.UserId, _nilai);
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
        
        if (timer)
        {
            detikTimer = 20.0f;
        }
    }
}
