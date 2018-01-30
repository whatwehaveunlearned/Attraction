using UnityEngine;
using System.Collections;

public class inArmorWorld : MonoBehaviour {

	private GameObject attrArm;
	private GameObject attrHolder;
	private Vector3 position;
	private Quaternion rotation;
	private bool inWorld = false; 

	// Use this for initialization
	void Start () {
		attrArm = GameObject.Find ("attrArm");
		attrHolder = GameObject.Find ("attrHolder");
		//Save the position to know where to come back
		position = gameObject.transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		if (gameObject.GetComponent<AttrProperties> ().inWorld == true && inWorld == false) {
			gameObject.transform.parent = attrHolder.transform;
			gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			inWorld = true;
		} else if(gameObject.GetComponent<AttrProperties> ().inWorld == false && inWorld == true){
			gameObject.transform.parent = attrArm.transform;
			gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			gameObject.transform.localPosition = position;
			gameObject.transform.localRotation = rotation;
			inWorld = false;
		}
	}
}
