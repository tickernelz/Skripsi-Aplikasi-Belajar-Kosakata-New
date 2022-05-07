using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using UnityEngine;

public class KosakataController : MonoBehaviour
{
    public GameObject[] Kosakata;
    public ModalWindowManager modalLatihan;
    public GameObject tombolPrev;
    private int steps;

    public void Start()
    {
        PlayerPrefs.SetInt("steps", 0);
    }

    public void Update()
    {
        steps = PlayerPrefs.GetInt("steps");
        if (steps == 0)
        {
            tombolPrev.SetActive(false);
        }
        else
        {
            tombolPrev.SetActive(true);
        }
    }

    public void Next(GameObject tombol)
    {
        steps = PlayerPrefs.GetInt("steps");
        if (steps < Kosakata.Length - 1)
        {
            Kosakata[steps].SetActive(false);
            steps++;
            PlayerPrefs.SetInt("steps", steps);
            Kosakata[steps].SetActive(true);
        }
        else
        {
            modalLatihan.OpenWindow();
        }
    }
    
    public void Previous()
    {
        steps = PlayerPrefs.GetInt("steps");
        if (steps > 0)
        {
            Kosakata[steps].SetActive(false);
            steps--;
            PlayerPrefs.SetInt("steps", steps);
            Kosakata[steps].SetActive(true);
        }
    }
}
