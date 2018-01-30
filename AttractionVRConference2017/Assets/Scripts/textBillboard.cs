using UnityEngine;
using System.Collections;

public class textBillboard : MonoBehaviour {

    private GameObject userCamera;
	// Use this for initialization
	void Start () {
        //userCamera = GameObject.Find("Camera (head)");
        userCamera = GameObject.Find("Camera (eye)");
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(userCamera.transform.position);
    }
}
