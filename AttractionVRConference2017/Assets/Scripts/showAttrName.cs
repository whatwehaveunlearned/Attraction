
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class showAttrName : MonoBehaviour
{
	public GameObject attrName;
	Camera camera;
	Ray ray;
	RaycastHit hit;
	TextMesh attrNameTextMesh;
	bool on=true;

	void Start(){
        camera = Camera.main;
        attrNameTextMesh = attrName.GetComponent<TextMesh>();
		attrNameTextMesh.text = gameObject.name;
	}
	void Update()
	{
		ray = camera.ScreenPointToRay(Input.mousePosition);
		if (Input.GetMouseButtonDown (1)) {
			if (Physics.Raycast (ray, out hit)) {
				if (on == true) {
					attrNameTextMesh.text = "";
					on = false;
				} else {
					attrNameTextMesh.text = gameObject.name;
					on = true;
				}

			}
		}
	}
}
