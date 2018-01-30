using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttrProperties : MonoBehaviour {

    public List<string> properties;
    public Color attrColor;
    public bool isActive = false;
	public bool inWorld = false;
	public bool isSelected = false;
	public Vector3 position;
	public Quaternion rotation;

    private Color color1 = new Vector4(0.5F, 1, 0.5F, 1);
    private GameObject attrPrefab;
    private Renderer rend;
    private List<Color> colorList;

    void Start()
    {
		colorList = new List<Color> { Color.red, Color.grey, Color.yellow, Color.green, Color.blue , Color.cyan, Color.white, Color.magenta, color1 };
        attrPrefab = gameObject;
        rend = attrPrefab.GetComponent<Renderer>();
        rend.material.SetColor("_Color",colorList[System.Int32.Parse(attrPrefab.GetComponent<AttrProperties>().properties[1])]);
		position = gameObject.transform.localPosition;
		rotation = gameObject.transform.localRotation;
    }
}
