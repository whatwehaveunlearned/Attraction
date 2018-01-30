using UnityEngine;
using System.Collections.Generic;

public class predifinedPos : MonoBehaviour {

    public bool circle2D = false;
    public bool sphere3D = false;
    public List<GameObject> attributes;
    private Transform[] allAttrs;
    private Transform[] thisAttrs;
    private GameObject attrHolder;
    private int numberAttrs=9;
	public bool inDemo = true;
	
    // Use this for initialization
	void Awake () {
        allAttrs = GameObject.Find("attrArm").GetComponentsInChildren<Transform>();
        attrHolder = GameObject.Find("attrHolder");
    }

    // Update is called once per frame
    void Update() {
        int index = 0;
        int count = 0;
		//If it is a demo
		if (inDemo) {
			foreach (Transform attr in allAttrs) {
				if (attr.GetComponent<AttrProperties> () != null && attr.name == "budget") {
					attributes.Add (attr.gameObject);
				} else if (attr.GetComponent<AttrProperties> () != null && attr.name == "income") {
					attributes.Add (attr.gameObject);
				}else if (attr.GetComponent<AttrProperties> () != null && attr.name == "cast popularity") {
					attributes.Add (attr.gameObject);
				}
			}
			circle2D = true;
			inDemo = false;
					
		}
        if (circle2D)
        {
            foreach(Transform attr in allAttrs)
            {
                if (attr.GetComponent<AttrProperties>() != null)
                {
                    attr.GetComponent<AttrProperties>().inWorld = false;
                    attr.GetComponent<AttrProperties>().isActive = false;
                    attr.GetComponent<AttrProperties>().isSelected = false;
                    foreach (Transform text in attr)
                    {
                        {
                            text.GetComponent<TextMesh>().characterSize = 0.1f;
                        }
                    }
                }

            }
            if (attributes.Count > 0)
            {
                thisAttrs = new Transform[attributes.Count];
                numberAttrs = attributes.Count;
                foreach (GameObject attr in attributes)
                {
                    thisAttrs[count] = attr.transform;
                    count++;
                }
            }
            else
            {
                numberAttrs = 9;
                thisAttrs = allAttrs;
            }
            foreach (Transform child in thisAttrs)
            {
                if (child.GetComponent<AttrProperties>() != null) {
                    //child.transform.position = new Vector3(0.2f, 0.2f, 0.2f);
                    child.transform.position = circlePosition(numberAttrs, index);
                    child.GetComponent<AttrProperties>().inWorld = true;
                    child.GetComponent<AttrProperties>().isActive = true;
                    foreach (Transform text in child)
                    {
                        {
                            text.GetComponent<TextMesh>().characterSize = 0.5f;
                        }
                    }
                    index++;
                }
            }
            index = 0;
            circle2D = false;
        }
        if (sphere3D)
        {
            foreach (Transform attr in allAttrs)
            {
                if (attr.GetComponent<AttrProperties>() != null)
                {
                    attr.GetComponent<AttrProperties>().inWorld = false;
                    attr.GetComponent<AttrProperties>().isActive = false;
                    attr.GetComponent<AttrProperties>().isSelected = false;
                    foreach (Transform text in attr)
                    {
                        {
                            text.GetComponent<TextMesh>().characterSize = 0.1f;
                        }
                    }
                }

            }
            if (attributes.Count > 0)
            {
                thisAttrs = new Transform[attributes.Count];
                numberAttrs = attributes.Count;
                foreach (GameObject attr in attributes)
                {
                    thisAttrs[count] = attr.transform;
                    count++;
                }
            }
            else
            {
                numberAttrs = 9;
                thisAttrs = allAttrs;
            }
            foreach (Transform child in thisAttrs)
            {
                if (child.GetComponent<AttrProperties>() != null)
                {
                    child.transform.position = Random.onUnitSphere * 10;
                    child.GetComponent<AttrProperties>().inWorld = true;
                    child.GetComponent<AttrProperties>().isActive = true;
                    foreach (Transform text in child)
                    {
                        {
                            text.GetComponent<TextMesh>().characterSize = 0.5f;
                        }
                    }
                    index++;
                }
            }
            index = 0;
            sphere3D = false;
        }
	}

    Vector3 circlePosition(int numberAttrs, int index)
    {
        Vector3 position;
        float r = 10f;
        float x = r * Mathf.Cos(2*Mathf.PI * index / numberAttrs);
        float z = r * Mathf.Sin(2*Mathf.PI* index/ numberAttrs);
        return position = new Vector3(x,0,z);
    }

    Vector3 spherePosition(int numberAttrs, int index)
    {
        Vector3 position;
        float random = Random.Range(-1, 1);
        float z = 2 * (random) - 1;
        float t = 2 * Mathf.PI * random;
        float x = Mathf.Sqrt(1 - Mathf.Pow(z, 2) * Mathf.Cos(t));
        float y = Mathf.Sqrt(1 - Mathf.Pow(z, 2) * Mathf.Sin(t));
        return position = new Vector3(x, y, z)*10; 
    }
}
