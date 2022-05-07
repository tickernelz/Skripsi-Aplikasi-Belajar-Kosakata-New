using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KosakataController : MonoBehaviour
{
    public GameObject[] Kosakata;
    private int steps;

    public void Start()
    {
        PlayerPrefs.SetInt("steps", 0);
    }

    public void Next()
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
            Kosakata[steps].SetActive(false);
            steps = 0;
            PlayerPrefs.SetInt("steps", steps);
            Kosakata[steps].SetActive(true);
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
        else
        {
            Kosakata[steps].SetActive(false);
            steps = Kosakata.Length - 1;
            PlayerPrefs.SetInt("steps", steps);
            Kosakata[steps].SetActive(true);
        }
    }
}
