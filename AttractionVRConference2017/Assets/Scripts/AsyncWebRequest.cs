using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class AsyncWebRequest : MonoBehaviour {

	private static int GET = 0;
	private static int POST = 1;
	private static string result;


	public static void Post(string url, string data, Action<string> ResultsCallBack, MonoBehaviour instance)
	{
		if(url != null || url.Length != 0)
			instance.StartCoroutine(AsyncRequest(url, POST, data, ResultsCallBack));
	}

	public static void Get(string url, Action<string> ResultsCallBack, MonoBehaviour instance)
	{
		if(url != null || url.Length != 0)
			instance.StartCoroutine(AsyncRequest(url, GET, null, ResultsCallBack));
	}
		

	private static IEnumerator AsyncRequest(string url, int method, string data, Action<string> callback)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		if (method == POST) {
			//Put hack for Post to prevent escaping/decoding (unity bug?)
			//www = UnityWebRequest.Put (url, data);
			//ok never mind, build request from scratch
			www.uploadHandler   = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(data));
			www.downloadHandler = new DownloadHandlerBuffer();
			www.method          = UnityWebRequest.kHttpVerbPOST;
		}
		www.SetRequestHeader("Content-Type", "application/json");
		//www.SetRequestHeader("Content-Type", "text/plain;charset=UTF-8");

		yield return www.Send();

		if ((www.downloadHandler.text != null || www.downloadHandler.text != "") && !www.isNetworkError)
		{
			callback(www.downloadHandler.text);
		}
		else
		{
			Debug.LogError("Could not connect to server. Error: " + www.error);

		}

	}

}

