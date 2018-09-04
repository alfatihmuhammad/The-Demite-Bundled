using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using CymaticLabs.Unity3D.Amqp;
using UnityEngine.UI;

public class InsertItem : MonoBehaviour
{
    string id;
    Button insertButton;

    // Use this for initialization
    void Start()
    {
        id = Guid.NewGuid().ToString();

        insertButton = this.GetComponent<Button>();
        insertButton.onClick.AddListener(CobaInsert);
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                // Insert Code Here (I.E. Load Scene, Etc)
                // OR Application.Quit();

                SceneManager.LoadScene("Login");

                return;
            }
        }
    }

    void ProcessCoba(AmqpExchangeReceivedMessage received)
    {
        var receivedJson = System.Text.Encoding.UTF8.GetString(received.Message.Body);
        var msg = CymaticLabs.Unity3D.Amqp.SimpleJSON.JSON.Parse(receivedJson);

        if (msg != null)
        {
            string msgId = (string)msg["id"];
            if (msgId == id)
            {
                string type = (string)msg["type"];
                if (type == "insertdata")
                {
                    /*
                    int result = (int)msg["result"];
                    if (result == -2)
                    {
                        Debug.Log("username already exist");
                    }
                    else if (result == -1)
                    {
                        Debug.Log("Server Error");
                    }
                    */

                    Debug.Log("OKE");
                }
            }
        }
    }

    void CobaInsert()
    {
        int iduser = 50;
        int iditem = 60;
        int itemtotal = 110;

        AmqpControllerScript.amqpControl.exchangeSubscription.Handler = ProcessCoba;

        RequestJson request = new RequestJson();
        request.id = id;
        request.type = "insertdata";
        request.iduser = iduser;
        request.iditem = iditem;
        request.itemtotal = itemtotal;

        string requestJson = JsonUtility.ToJson(request);
        Debug.Log(requestJson);

        AmqpClient.Publish(AmqpControllerScript.amqpControl.requestExchange, AmqpControllerScript.amqpControl.requestRoutingKey, requestJson);

    }

    [Serializable]
    public class RequestJson
    {
        public string id;
        public string type;
        public int iduser;
        public int iditem;
        public int itemtotal;
    }
}
