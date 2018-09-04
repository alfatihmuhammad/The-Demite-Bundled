using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using CymaticLabs.Unity3D.Amqp;

public class MapToHome : MonoBehaviour {

    private Button homeButton;

	// Use this for initialization
	void Start ()
    {
        homeButton = this.GetComponent<Button>();
        homeButton.onClick.AddListener(ReturnHome);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ReturnHome()
    {
        GameObject mapController = GameObject.Find("Map");
        var mapScript = mapController.GetComponent<MapController>();

        MapToHomeRequest request = new MapToHomeRequest();
        request.id = mapScript.uniqueId;
        request.type = "maptohome";
        request.username = PlayerPrefs.GetString("username");
        request.tileX = mapScript.tileX;
        request.tileY = mapScript.tileY;
        string jsonRequest = JsonUtility.ToJson(request);

        AmqpClient.Publish(AmqpControllerScript.amqpControl.requestExchange, AmqpControllerScript.amqpControl.requestRoutingKey, jsonRequest);
    }

    [Serializable]
    class MapToHomeRequest
    {
        public string id;
        public string type;
        public string username;
        public int tileX;
        public int tileY;
    }
}
