using UnityEngine;
using System.Collections;

public class flyCam : MonoBehaviour {

	public float speed = 50.0f; //Max speed
	public float sensitivity = 0.2f;
	public bool inverted =false;
	public bool showMouse = false;


	private Vector3 lastMouse = new Vector3(255,255,255);
	//smoothing
	public bool smooth = true;
	public float acceleration = 0.1f;
	private float actSpeed = 0.1f;
	private Vector3 lastDir = new Vector3();

	// Use this for initialization
	void Start () {
		if (showMouse != true) {
			Cursor.visible = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Mouse Look

		lastMouse = Input.mousePosition - lastMouse;
		if (!inverted)
			lastMouse.y = -lastMouse.y;
		lastMouse *= sensitivity;
		lastMouse = new Vector3 (transform.eulerAngles.x + lastMouse.y, transform.eulerAngles.y + lastMouse.x, 0);
		transform.eulerAngles = lastMouse;

		lastMouse = Input.mousePosition;

		//Camera movement
		Vector3 dir = new Vector3 (); //create (0,0,0)
		if(Input.GetKey(KeyCode.W)) dir.z += 1.0f;
		if(Input.GetKey(KeyCode.S)) dir.z -= 1.0f;
		if(Input.GetKey(KeyCode.A)) dir.x -= 1.0f;
		if(Input.GetKey(KeyCode.D)) dir.x += 1.0f;
		dir.Normalize ();

		if (dir != Vector3.zero) {
			//some movement
			if (actSpeed < 1)
				actSpeed += acceleration * Time.deltaTime * 40;
			else
				actSpeed = 1.0f;
			lastDir = dir;
		} else {
			if (actSpeed > 1)
				actSpeed -= acceleration * Time.deltaTime * 20;
			else
				actSpeed = 0.0f;
		}

		if(smooth)
		transform.Translate (lastDir * actSpeed * speed * Time.deltaTime);
		else
			transform.Translate (dir * speed * Time.deltaTime);
	}
		
}
