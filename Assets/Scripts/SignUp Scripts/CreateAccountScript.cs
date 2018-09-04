using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using CymaticLabs.Unity3D.Amqp;

public class CreateAccountScript : MonoBehaviour {

    string id;
    Button createButton;
    Text warningText;

	// Use this for initialization
	void Start () {
        id = Guid.NewGuid().ToString();

        createButton = this.GetComponent<Button>();
        createButton.onClick.AddListener(CreateNewAccount);

        warningText = GameObject.Find("Warning_Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
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

    void ProcessAccount(AmqpExchangeReceivedMessage received)
    {
        var receivedJson = System.Text.Encoding.UTF8.GetString(received.Message.Body);
        var msg = CymaticLabs.Unity3D.Amqp.SimpleJSON.JSON.Parse(receivedJson);

        if( msg != null)
        {
            string msgId = (string)msg["id"];
            if (msgId == id)
            {
                string type = (string)msg["type"];
                if(type == "newaccount")
                {
                    int result = (int)msg["result"];
                    if (result == -2)
                    {
                        warningText.text = "username already exist";
                    }
                    else if (result == -1)
                    {
                        warningText.text = "Server Error";
                    }
                    else
                    {
                        SceneManager.LoadScene("Login");
                    }
                }
            }
        }
    }

    void CreateNewAccount()
    {
        warningText.text = "";

        InputField firstNameField = GameObject.Find("InputField_FirstName").GetComponent<InputField>();
        InputField lastNameField = GameObject.Find("InputField_LastName").GetComponent<InputField>();
        InputField emailField = GameObject.Find("InputField_Email").GetComponent<InputField>();
        InputField usernameField = GameObject.Find("InputField_Username").GetComponent<InputField>();
        InputField passwordField = GameObject.Find("InputField_Password").GetComponent<InputField>();
        InputField petNameField = GameObject.Find("InputField_PetName").GetComponent<InputField>();

        string firstName = firstNameField.text;
        string lastName = lastNameField.text;
        string email = emailField.text;
        string username = usernameField.text;
        string password = passwordField.text;
        string petName = petNameField.text;


        if (firstName == "")
        {
            warningText.text = "first name cannot be empty";
        }

        if (lastName == "")
        {
            warningText.text = "last name cannot be empty";
        }

        if(email == "")
        {
            warningText.text = "email cannot be empty";
        }

        if(username == "")
        {
            warningText.text = "username cannot be empty";
        }

        if(password == "")
        {
            warningText.text = "password cannot be empty";
        }

        if (petName == "")
        {
            warningText.text = "pet name cannot be empty";
        }

        if (firstName != "" && lastName != "" && email != "" && username != "" && password != "" && petName != "")
        {
            AmqpControllerScript.amqpControl.exchangeSubscription.Handler = ProcessAccount;

            RequestJson request = new RequestJson();
            request.id = id;
            request.type = "newaccount";
            request.firstName = firstName;
            request.lastName = lastName;
            request.email = email;
            request.username = username;
            request.password = password;
            request.petName = petName;

            string requestJson = JsonUtility.ToJson(request);

            AmqpClient.Publish(AmqpControllerScript.amqpControl.requestExchange, AmqpControllerScript.amqpControl.requestRoutingKey, requestJson);
        }
    }

    [Serializable]
    public class RequestJson
    {
        public string id;
        public string type;
        public string firstName;
        public string lastName;
        public string email;
        public string username;
        public string password;
        public string petName;
    }
}
