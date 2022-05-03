using System;
using UnityEngine;
using UnityEngine.UI;

public class CrashlyticsTester : MonoBehaviour {

        int updatesBeforeException;
        public Text msg;
        int counter = 0;
        bool isStart = false;
        // Use this for initialization
        void Start ()
        {
                updatesBeforeException = 0;
        }

        // Update is called once per frame
        void Update ()
        {
                // Call the exception-throwing method here so that it's run
                // every frame update
                if (isStart)
                        throwExceptionEvery60Updates ();
        }

        // A method that tests your Crashlytics implementation by throwing an
        // exception every 60 frame updates. You should see non-fatal errors in the
        // Firebase console a few minutes after running your app with this method.
        void throwExceptionEvery60Updates ()
        {
                if (updatesBeforeException > 0) {
                        updatesBeforeException--;
                } else {
                        counter++;
                        updatesBeforeException = 60;

                        msg.text = "Test Exception Counter: "+ counter +"\n Check Firebase Console";
                        // Throw an exception to test your Crashlytics implementation
                        throw new System.Exception ("test exception please ignore");
                        
                }
        }

        public void StartListening()
	{
                isStart = true;
	}
}