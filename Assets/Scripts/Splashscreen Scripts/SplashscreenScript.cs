using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashscreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(AmqpControllerScript.amqpControl != null)
        {
            if(AmqpControllerScript.amqpControl.serverTerhubung)
            {
                SceneManager.LoadScene("Login");
            }
        }
	}
}
