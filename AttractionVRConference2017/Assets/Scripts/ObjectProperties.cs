using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectProperties : MonoBehaviour {

	public List<string> properties;

    //Quantitative Values
	public float attr0;
	public float attr1;
	public float attr2;
    public float attr3;
    public float attr4;
    public float attr5;
    public float attr6;
    public float attr7;
    public float attr8;
    public float attr9;

    //Qualitaive Values
    public string dataName;
    public int year;
    public List<string> genres = new List<string>();

    //Public is Selected
    public bool isSelected = false;

    public float noNormalizedattr0;
	public float noNormalizedattr1;
	public float noNormalizedattr2;
	public float noNormalizedattr3;
	public float noNormalizedattr4;
    public float noNormalizedattr5;
    public float noNormalizedattr6;
    public float noNormalizedattr7;
    public float noNormalizedattr8;
    public float noNormalizedattr9;

    private GameObject dataPrefab;
    private TextMesh textName;

    void Start()
    {

        this.GetComponentInChildren<TextMesh>().text = dataName;
        //foreach (Transform child in transform)
        //{
        //    if (child.gameObject.tag == "dataName")
        //    {
        //        child.GetComponent<Renderer>().enabled = false;
        //    }
        //}
    }
}
