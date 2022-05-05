using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Latihan : MonoBehaviour
{
    public GameObject button;
    public Animator transition;
    
    public void ToggleAnimation()
    {
        if (button.activeInHierarchy)
        {
            StartCoroutine(Toggle("Hide"));
        }
        else
        {
            button.SetActive(true);
        }
    }
    
    IEnumerator Toggle(string animation)
    {
        transition.SetTrigger(animation);
        yield return new WaitForSeconds(2f);
        button.SetActive(false);
    }
}
