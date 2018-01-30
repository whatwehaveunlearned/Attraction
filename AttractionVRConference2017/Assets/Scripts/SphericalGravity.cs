using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphericalGravity : MonoBehaviour {

	public List<GameObject> attributes;
    public GameObject wand;
    public GameObject positionArm;
    public GameObject dataHolder;
	public float adjustment;
    public bool forceActive = true;
	private List<string> attrsNames = new List<string>();
	private List<float> attr0 = new List<float>();
	private List<float> attr1 = new List<float>();
	private List<float> attr2 = new List<float>();
	private List<float> attr3 = new List<float>();
	private List<float> attr4 = new List<float>();
    private List<float> attr5 = new List<float>();
    private List<float> attr6 = new List<float>();
    private List<float> attr7 = new List<float>();
    private List<float> attr8 = new List<float>();
    private List<float> attr9 = new List<float>();
    private List<GameObject> dataPoints = new List<GameObject>();
    private List<GameObject> activeAttrs = new List<GameObject>();
    private GameObject attrArm;
	private int i;
    private int count = 0;
    private Renderer rend;
    Color dataColor = Color.clear;

    void Start(){
		attrsNames.Add("duration");
		attrsNames.Add("income");
		attrsNames.Add("num voted users");
		attrsNames.Add ("cast popularity");
		attrsNames.Add("imdb score");
        attrsNames.Add("budget");
        attrsNames.Add("number faces in poster");
        attrsNames.Add("movie popularity");
        attrsNames.Add("aspect ratio");
        //Delete Physics
		Physics.gravity = new Vector3(0, 0, 0);
        //Set the attributes as a child of wand to carry with you.
        attrArm = GameObject.Find("attrArm");
        attrArm.transform.position = positionArm.transform.position;
        attrArm.transform.parent = wand.transform;
        attrArm.transform.localScale -= new Vector3(0.7f, 0.7f, 0.7f);
        attrArm.transform.localRotation = new Quaternion(0, -180, 0,1);
        //Get all the dataPoints and set the values.
        foreach (GameObject o in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            //foreach(Transform o in dataHolder.transform){
            if (o.name == "dataPrefab(Clone)")
            {
                attr0.Add(o.GetComponent<ObjectProperties>().attr0);
                attr1.Add(o.GetComponent<ObjectProperties>().attr1);
                attr2.Add(o.GetComponent<ObjectProperties>().attr2);
                attr3.Add(o.GetComponent<ObjectProperties>().attr3);
                attr4.Add(o.GetComponent<ObjectProperties>().attr4);
                attr5.Add(o.GetComponent<ObjectProperties>().attr5);
                attr6.Add(o.GetComponent<ObjectProperties>().attr6);
                attr7.Add(o.GetComponent<ObjectProperties>().attr7);
                attr8.Add(o.GetComponent<ObjectProperties>().attr8);
                attr9.Add(o.GetComponent<ObjectProperties>().attr9);
                dataPoints.Add(o);
            }
        }
    }

	void FixedUpdate(){
        if (forceActive)
        {
            //Color actualColor = Color.clear;
            Transform[] attrHolderChildren = GameObject.Find("attrHolder").GetComponentsInChildren<Transform>();
            List<Transform> activeAttr = new List<Transform>();
            //List<Color> activeColors = new List<Color>();
            List<float> weights;
            List<GameObject> updatedDataPoints = new List<GameObject>();
            foreach (Transform attr in attrHolderChildren)
            {
                if (attr.GetComponent<AttrProperties>() != null)
                {
                    if (attr.GetComponent<AttrProperties>().isActive == true)
                    {
                        //activeColors.Add(attr.GetComponent<Renderer>().material.color);
                        activeAttr.Add(attr);
                    }
                }
            }
            i = 0;
            count++;
            //We apply the force every five FixedUpdates
            if (count == 60)
            {
                //foreach (GameObject o in updatedDataPoints) {
                foreach (GameObject o in dataPoints)
                {
                    weights = new List<float>();
                    foreach (Transform activeAttribute in activeAttr)
                    {
                        if (activeAttribute.name == attrsNames[0])
                        {
                            weights.Add(attr0[i]);
                        }
                        else if (activeAttribute.name == attrsNames[1])
                        {
                            weights.Add(attr1[i]);
                        }
                        else if (activeAttribute.name == attrsNames[2])
                        {
                            weights.Add(attr2[i]);
                        }
                        else if (activeAttribute.name == attrsNames[3])
                        {
                            weights.Add(attr3[i]);
                        }
                        else if (activeAttribute.name == attrsNames[4])
                        {
                            weights.Add(attr4[i]);
                        }
                        else if (activeAttribute.name == attrsNames[5])
                        {
                            weights.Add(attr5[i]);
                        }
                        else if (activeAttribute.name == attrsNames[6])
                        {
                            weights.Add(attr6[i]);
                        }
                        else if (activeAttribute.name == attrsNames[7])
                        {
                            weights.Add(attr7[i]);
                        }
                        else if (activeAttribute.name == attrsNames[8])
                        {
                            weights.Add(attr8[i]);
                        }
                    }
                    //actualColor = calculatedColor(activeColors, weights);
                    applyForce(o, activeAttr, weights);
                    //o.GetComponent<Renderer>().material.SetColor("_Color", actualColor);
                    //StartCoroutine(setKinematicFalse(o));
                    i++;
                }
                count = 0;
            }
        }
    }

    IEnumerator setKinematicFalse(GameObject o)
    {
        yield return new WaitForSeconds(50);
		o.GetComponent<Rigidbody>().velocity = new Vector3 (0,0,0);
    }

    Color calculatedColor(List<Color> colorList, List<float> weights){
		Color outputColor=Color.clear;
		int weightIndex = 0;
		//float adjustment = adjustment;
		foreach (Color color in colorList) {
			outputColor.a += weights[weightIndex] * color.a;
			outputColor.r += weights[weightIndex]/(200 * colorList.Count) * color.r;
			outputColor.g += weights[weightIndex]/(200 * colorList.Count) * color.g;
			outputColor.b += weights[weightIndex]/(200 * colorList.Count) * color.b;
			weightIndex++;
		}
		return outputColor;
	}

	void applyForce(GameObject o, List<Transform> activeAttr, List<float> weights){
		int i=0;
		foreach (Transform attr in activeAttr) {
			o.GetComponent<Rigidbody> ().AddForce ((attr.transform.position - o.transform.position) * weights[i]);
			i++;
		}
	}
}
