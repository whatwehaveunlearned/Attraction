using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testasync : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		//AsyncWebRequest.Get("http://localhost:8888/UnityExternalSpeech/", printWebResponse, this);
		AsyncWebRequest.Post("http://localhost:8888/UnityExternalSpeech/",@"{""test"":""test1""}", printWebResponse, this);
		//print (@"{""test"":""test1""}");
	}
	
	// Update is called once per frame
	void printWebResponse (string data) {
		print (data);
	}
}
