using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poligonLines : MonoBehaviour {

	public GameObject holder;
	private Transform activeElements;
	private bool linesDrawn = false;
	private AttrProperties myScript;
	private LineRenderer thisLineRenderer;
	private GameObject lineObject;
	private int count=0;
	private List<LineRenderer> listOfLines = new List<LineRenderer>();

	void Start(){
		myScript = gameObject.GetComponent<AttrProperties>();
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (count == 500) {
			foreach (Transform child in this.gameObject.transform) {
				if (child.name != gameObject.name) {
					listOfLines.Add(child.GetComponent<LineRenderer> ());
				}
			}
			holder = GameObject.Find ("attrHolder");
			activeElements = holder.transform;
			if (myScript.isActive == true && activeElements.childCount > 0 && linesDrawn == false) {
				foreach (Transform child in activeElements) {
					lineObject = new GameObject ("line" + child.name);
					lineObject.transform.parent = this.gameObject.transform;
					drawLine (gameObject.transform, child.transform, lineObject,gameObject,child.gameObject);
				}
				Destroy (thisLineRenderer);
				//linesDrawn = true;
			}
			foreach (LineRenderer thisLine in listOfLines) {
				Destroy(thisLine);
			}
			count = 0;
		}
		count = count + 1;
	}

	void drawLine(Transform origin,Transform destination,GameObject lineObject, GameObject origObject, GameObject destObject){
		LineRenderer  line = (LineRenderer) lineObject.AddComponent<LineRenderer>();
		line.SetVertexCount(2);
		line.SetPosition (0,origin.transform.position);
		line.SetPosition (1,destination.transform.position);
		line.SetWidth(0.05f, 0.05f);
        Material  lineMaterial = new Material(Shader.Find("Particles/Additive"));
        line.material = lineMaterial;
        line.startColor = origObject.GetComponent<Renderer>().material.color;
        line.endColor = destObject.GetComponent<Renderer>().material.color;
	}
}
