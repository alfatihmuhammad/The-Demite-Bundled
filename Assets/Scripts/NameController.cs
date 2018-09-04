using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameController : MonoBehaviour {

    GameObject mainCam;

	// Use this for initialization
	void Start () {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(2 * transform.position - mainCam.transform.position);
	}
}
