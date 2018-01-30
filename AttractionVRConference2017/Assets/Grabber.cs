using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/* Simple code to let you pick up and put down objects.
 * (C) 2016 Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa
 * Version 8/21/16
 */
public class Grabber : MonoBehaviour {

	public GameObject wandr;
	public GameObject wandl;
	public GameObject cameraRig;
	public GameObject wandlabels;
	public GameObject attrHolder;
	public GameObject attrArm;

	//Count to cycle around on and off for the attributes
	private int attrCycle;

	private int count = 0;

    public List<GameObject> selectedData = new List<GameObject>();
	GameObject currentObject = null;
	GameObject grabbedObject = null;
    GameObject lastGrabbedObject = null;
	Transform grabbedObjectParent = null;
	bool wasKinematic = false;

    private bool isNormalized = false;

	void OnTriggerEnter(Collider collision){
        //print (gameObject.name + " " + collision.gameObject.name);
        if (grabbedObject == null)
        {
            currentObject = collision.gameObject;
            SetDataText(currentObject);
            //add to selected
            selectedData.Add(currentObject);
            lastGrabbedObject = currentObject;
        }
            
		//print ("COLLIDED");
	}

	void OnTriggerExit(Collider collision) {
		currentObject = null;
        selectedData = new List<GameObject>();
	}

	// Use this for initialization
	void Start () {
        wandlabels.GetComponent<ControllerLabels>().labels[11].text = "Activate/Deactivate + Attr";
        wandlabels.GetComponent<ControllerLabels>().labels[11].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[11].text;
        wandlabels.GetComponent<ControllerLabels>().labels[11].textMesh.characterSize = 0.003f;
        wandlabels.GetComponent<ControllerLabels>().labels[12].text = "Start/Stop + force";
        wandlabels.GetComponent<ControllerLabels>().labels[12].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[12].text;
        wandlabels.GetComponent<ControllerLabels>().labels[12].textMesh.characterSize = 0.003f;
        wandlabels.GetComponent<ControllerLabels>().labels[13].text = "Layout + Circle/Sphere + (selected Attributes)";
        wandlabels.GetComponent<ControllerLabels>().labels[13].textMesh.characterSize = 0.003f;
        wandlabels.GetComponent<ControllerLabels>().labels[13].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[13].text;
        wandlabels.GetComponent<ControllerLabels>().labels[14].text = "Select/Un-Select + this/Genre";
        wandlabels.GetComponent<ControllerLabels>().labels[14].textMesh.characterSize = 0.003f;
        wandlabels.GetComponent<ControllerLabels>().labels[14].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[14].text;
        wandlabels.GetComponent<ControllerLabels>().labels[15].text = "Hide/Show + Genre";
        wandlabels.GetComponent<ControllerLabels>().labels[15].textMesh.characterSize = 0.003f;
        wandlabels.GetComponent<ControllerLabels>().labels[15].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[15].text;
    }
	
	// Update is called once per frame
	void Update () {
        float x;
        float y;

		SteamVR_TrackedObject trackedObjWandr = wandr.GetComponent<SteamVR_TrackedObject> ();
		SteamVR_TrackedObject trackedObjWandl = wandl.GetComponent<SteamVR_TrackedObject> ();
		SteamVR_Controller.Device deviceR = null;
		SteamVR_Controller.Device deviceL = null;

		if (trackedObjWandr != null)
			deviceR = SteamVR_Controller.Input ((int)trackedObjWandr.index);
		if(trackedObjWandr != null)
			deviceL = SteamVR_Controller.Input ((int)trackedObjWandl.index);

		//Hold for a few iterations in order not to enter the loop several times in a row with the same interaction
		count += 1;

        x = deviceR.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;
        y = deviceR.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y;

        if (x != 0 || y != 0)
        {
            if (y > 0)//top Part of pad
            {
                if (x > 0)//Top Right
                {
                    if (y > x)
                    {
                    }
                    else
                    {
                        isNormalized = false;
                        SetDataText(lastGrabbedObject);
                    }
                }
                else//Top Left
                {
                    if (y > -x)
                    {
                    }
                    else
                    {
                        isNormalized = true;
                        SetDataText(lastGrabbedObject);
                    }
                }
            }
            else//bottom part of pad
            {
                if (x > 0)//bottom Right
                {
                    if (-y > x)
                    {
                    }
                    else
                    {
                        isNormalized = false;
                        SetDataText(lastGrabbedObject);
                    }
                }
                else//bottom Left
                {
                    if (-y > -x)
                    {
                    }
                    else
                    {
                        isNormalized = true;
                        SetDataText(lastGrabbedObject);
                    }
                }
            }
        }

        if (deviceR.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
			if (grabbedObject == null) {
				if (currentObject != null) {
					grabbedObject = currentObject;
				
					// If object had a rigidbody, grabbed save the rigidbody's kinematic state
					// so it can be restored on release of the object
					Rigidbody body = null;
                    
					body = grabbedObject.GetComponent<Rigidbody> ();
					if (body != null) {
						wasKinematic = body.isKinematic;
						body.isKinematic = true;
					}

					//Read the parameters of the data Point(Dont kn ow if I will put this here or not)
					// Save away to original parentage of the grabbed object
					if (grabbedObject.tag == "attr") {
						grabbedObjectParent = attrHolder.transform;
					} else {
						// Save away to original parentage of the grabbed object
						grabbedObjectParent = grabbedObject.transform.parent;
					}

					// Make the grabbed object a child of the wand
					grabbedObject.transform.parent = wandr.transform;
					currentObject = null;

					// Disable collision between yourself and the grabbed object so that the grabbed object
					// does not apply its physics to you and push you off the world
					Physics.IgnoreCollision(cameraRig.GetComponent<Collider>(), grabbedObject.GetComponent<Collider>(), true);

					if (deviceR.GetPress(SteamVR_Controller.ButtonMask.Grip) )
                    {
						if (grabbedObject.GetComponent<AttrProperties> ().inWorld == true) {
							if (grabbedObject.GetComponent<AttrProperties> ().isActive == true) {
								grabbedObject.GetComponent<AttrProperties> ().isActive = false;
								grabbedObject.GetComponent<Light>().enabled=false;
							} else {
								if (attrCycle == 0) {
									grabbedObject.GetComponent<AttrProperties> ().isActive = true;
									grabbedObject.GetComponent<Light>().enabled=true;
									grabbedObjectParent = attrHolder.transform;
									attrCycle = 1;
								} else if (attrCycle == 1) {
									grabbedObject.GetComponent<AttrProperties> ().inWorld = false;
									grabbedObjectParent =  attrArm.transform;
									attrCycle = 0;
								}
							}
						} else {
							grabbedObject.GetComponent<AttrProperties> ().inWorld = true;
							grabbedObjectParent = attrHolder.transform;
						}
						count = 0;
                    }
                }
			}
		} else {
			if (grabbedObject != null) {

				// Restore the original parentage of the grabbed object
				grabbedObject.transform.parent = grabbedObjectParent;

				// If object had a rigidbody, restore its kinematic state
				Rigidbody body = null;
				body = grabbedObject.GetComponent<Rigidbody> ();
				if (body != null) {
					body.isKinematic = wasKinematic;
				}

				// Re-enstate collision between self and object
				Physics.IgnoreCollision (cameraRig.GetComponent<Collider> (), grabbedObject.GetComponent<Collider> (), false);

				grabbedObject = null;
				currentObject = null;
			}

		}
	
	}

    void SetDataText(GameObject o)
    {
        if (isNormalized)
        {
            wandlabels.GetComponent<ControllerLabels>().labels[1].text = "Income: " + o.gameObject.GetComponent<ObjectProperties>().attr1.ToString("n2");
            wandlabels.GetComponent<ControllerLabels>().labels[1].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[1].text;
            wandlabels.GetComponent<ControllerLabels>().labels[2].text = "User Votes: " + o.gameObject.GetComponent<ObjectProperties>().attr2.ToString("n2");
            wandlabels.GetComponent<ControllerLabels>().labels[2].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[2].text;
            wandlabels.GetComponent<ControllerLabels>().labels[3].text = "Cast Popularity: " + o.gameObject.GetComponent<ObjectProperties>().attr3.ToString("n2");
            wandlabels.GetComponent<ControllerLabels>().labels[3].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[3].text;
            wandlabels.GetComponent<ControllerLabels>().labels[4].text = "Movie Popularity: " + o.gameObject.GetComponent<ObjectProperties>().attr7.ToString("n2");
            wandlabels.GetComponent<ControllerLabels>().labels[4].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[4].text;
            wandlabels.GetComponent<ControllerLabels>().labels[6].text = "Budget: " + o.gameObject.GetComponent<ObjectProperties>().attr5.ToString("n2");
            wandlabels.GetComponent<ControllerLabels>().labels[6].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[6].text;
            wandlabels.GetComponent<ControllerLabels>().labels[8].text = "Score: " + o.gameObject.GetComponent<ObjectProperties>().attr4.ToString("n2");
            wandlabels.GetComponent<ControllerLabels>().labels[8].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[8].text;
            wandlabels.GetComponent<ControllerLabels>().labels[9].text = "Name: " + o.gameObject.GetComponent<ObjectProperties>().dataName;
            wandlabels.GetComponent<ControllerLabels>().labels[9].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[9].text;
            wandlabels.GetComponent<ControllerLabels>().labels[10].text = "NORMALIZED VALUES 0-10";
            wandlabels.GetComponent<ControllerLabels>().labels[10].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[10].text;
        }
        else
        {
            wandlabels.GetComponent<ControllerLabels>().labels[1].text = "Income: " + o.gameObject.GetComponent<ObjectProperties>().noNormalizedattr1.ToString("c2");
            wandlabels.GetComponent<ControllerLabels>().labels[1].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[1].text;
            wandlabels.GetComponent<ControllerLabels>().labels[2].text = "User Votes: " + o.gameObject.GetComponent<ObjectProperties>().noNormalizedattr2.ToString();
            wandlabels.GetComponent<ControllerLabels>().labels[2].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[2].text;
            wandlabels.GetComponent<ControllerLabels>().labels[3].text = "Cast Popularity: " + o.gameObject.GetComponent<ObjectProperties>().noNormalizedattr3.ToString();
            wandlabels.GetComponent<ControllerLabels>().labels[3].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[3].text;
            wandlabels.GetComponent<ControllerLabels>().labels[4].text = "Movie Popularity: " + o.gameObject.GetComponent<ObjectProperties>().noNormalizedattr7.ToString();
            wandlabels.GetComponent<ControllerLabels>().labels[4].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[4].text;
            wandlabels.GetComponent<ControllerLabels>().labels[6].text = "Budget: " + o.gameObject.GetComponent<ObjectProperties>().noNormalizedattr5.ToString("c2");
            wandlabels.GetComponent<ControllerLabels>().labels[6].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[6].text;
            wandlabels.GetComponent<ControllerLabels>().labels[8].text = "Score: " + o.gameObject.GetComponent<ObjectProperties>().noNormalizedattr4.ToString();
            wandlabels.GetComponent<ControllerLabels>().labels[8].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[8].text;
            wandlabels.GetComponent<ControllerLabels>().labels[9].text = "Name: " + o.gameObject.GetComponent<ObjectProperties>().dataName;
            wandlabels.GetComponent<ControllerLabels>().labels[9].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[9].text;
            wandlabels.GetComponent<ControllerLabels>().labels[10].text = "NO NORMALIZED VALUES";
            wandlabels.GetComponent<ControllerLabels>().labels[10].textMesh.text = wandlabels.GetComponent<ControllerLabels>().labels[10].text;
        }
    }
}
