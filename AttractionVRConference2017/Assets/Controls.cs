using UnityEngine;
using System.Collections;

/* This is an example script to show the use of all the HTC Vive Wand Controls. Only the Left wand
*  controls are shown.
* (C) 2016 Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa
*/


public class Controls : MonoBehaviour {

	public GameObject leftWand;
	public GameObject rightWand;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {



		SteamVR_TrackedObject trackedObjLeft=null , trackedObjRight = null;
		SteamVR_Controller.Device deviceLeft=null, deviceRight=null;

		/* Have to keep looking for the wands at each iteration for now since the user is
		 * not guaranteed to have them in tracking range.
		 */
		if (leftWand != null)
		trackedObjLeft = leftWand.GetComponent<SteamVR_TrackedObject> ();
		if (rightWand != null)
		trackedObjRight = rightWand.GetComponent<SteamVR_TrackedObject> ();

		if (trackedObjLeft != null)
			deviceLeft = SteamVR_Controller.Input ((int)trackedObjLeft.index);
		if (trackedObjRight != null)
			deviceRight = SteamVR_Controller.Input ((int)trackedObjRight.index);

		if (deviceLeft != null) {
			if (deviceLeft.GetPress (SteamVR_Controller.ButtonMask.ApplicationMenu)) {
				print ("LEFT MENU HELD DOWN");
			}

			float x = 0, y=0;

			if (deviceLeft != null)
				x = deviceLeft.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;

			if (deviceLeft != null)
				y= deviceLeft.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y;
			
			//print ("DPAD x=" + x + " y=" + y);
			if (deviceLeft != null) {
				if (deviceLeft.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
					print ("LEFT TRIGGER HELD DOWN");
					deviceLeft.TriggerHapticPulse (2000);
				}
				if (deviceLeft.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
					print ("LEFT DPAD HELD DOWN");
				}

				if (deviceLeft.GetPressDown (Valve.VR.EVRButtonId.k_EButton_Grip)) {
					print ("LEFT GRIP BUTTON PRESSED ONCE");
				}
			}
		}
	}
}
