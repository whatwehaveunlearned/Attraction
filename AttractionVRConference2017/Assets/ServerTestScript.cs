using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ServerTestScript : MonoBehaviour {

	public GameObject send_input;
	public GameObject receive_output;
	public string server_api;


	void Start () {
		InputField input = send_input.GetComponent<InputField>();
		input.onEndEdit.AddListener(SubmitName);

		if (server_api == null || server_api.Length == 0)
			server_api = "http://localhost:8888/UnityExternalSpeech/";
	}
	

	private void SubmitName(string arg0)
	{
		Text output = receive_output.GetComponent<Text>();
		output.text = "connecting to server...";
		AsyncWebRequest.Post(server_api, arg0, UpdateResponseText, this);
		//Debug.Log(arg0);
	}

	private void UpdateResponseText(string text)
	{
		Text output = receive_output.GetComponent<Text>();
		output.text = text;
	}
}
