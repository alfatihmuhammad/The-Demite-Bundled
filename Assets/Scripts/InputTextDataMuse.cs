using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System;

public class InputTextDataMuse : MonoBehaviour {
    public InputField inputField;
    public Text phonetic;

    // Use this for initialization
    void Start () {
        //StartCoroutine(getUnityWebRequest());
    }

    void Awake() {
        inputField.onEndEdit.AddListener(AcceptStringInput);
    }

    void AcceptStringInput(string userInput) {
        userInput = userInput.ToLower();
        string phoneticResult = getUnityWebRequest().ToString();
        phonetic.text = phoneticResult;
        InputComplete();
    }

    IEnumerator getUnityWebRequest() {
        UnityWebRequest www = UnityWebRequest.Get("https://api.datamuse.com/words?sl=adventure");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        } else {
            Debug.Log(www.downloadHandler.text);
        }
    }

    void InputComplete() {
        inputField.ActivateInputField();
        inputField.text = null;
    }
}
