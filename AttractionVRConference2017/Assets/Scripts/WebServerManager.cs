/*
 * A Unity script receive HTTP POST requests and send responds to clients.
 * 
 * SendResponse - returns a string of what to display on web page
 * ReceiveRequest - processes the POST request from clients
 * 
 * Parameters: LOCAL_HOST - server's addess to process requests (e.g. http://localhost:8080/test/)
 * 
 * Code was modified from https://codehosting.net/blog/BlogEngine/post/Simple-C-Web-Server
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleWebServer;
using System.Net;
using System;

public class WebServerManager : MonoBehaviour {

	private WebServerWorker ws;
	[SerializeField]
	private string LOCAL_HOST;

	void Start () {
		if (LOCAL_HOST == null || LOCAL_HOST.Length == 0) {
			LOCAL_HOST = "http://localhost:8000/";
		}
		Debug.Log("Webserver listening on " + LOCAL_HOST);

		//ws = new WebServer(SendResponse, LOCAL_HOST);
		//ws = new WebServer(ReceiveRequest, LOCAL_HOST);
		ws = new WebServerWorker(SendResponse, ReceiveRequest, LOCAL_HOST);
		ws.Run();
	}
		
	void OnDestroy () {
		ws.Stop();
		Debug.Log("Webserver shutting down");
	}


	public static string SendResponse(HttpListenerRequest request)
	{
		// Add response processor function here
		// This function returns a string to display to webserver
		return string.Format("{{\"time\":\"{0}\"}}", DateTime.Now);    
	}


	public static void ReceiveRequest(string postText)
	{
		// Add request processor function here
		if(postText.Length > 0)
			Debug.Log(postText);
	}
}
	


