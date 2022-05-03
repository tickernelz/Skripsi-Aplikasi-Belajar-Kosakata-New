using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    public void GotoScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}

	private void Start ()
	{
		//Directory.CreateDirectory (Environment.GetFolderPath (Environment.SpecialFolder.Personal) + "/Library/Application Support/"+ Application.identifier);
		//Debug.Log (Application.identifier);
	}
}
