using UnityEngine;
using System.Collections;

/* This is a simple VR navigation script. Attach it to your SteamVR CameraRig.
 * (C) 2016 Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa
 * 
 * DPAD forward and back is "forward" and "backward" (no need to press down on DPAD, just glide thumb over it)
 * Vector of wand determines direction of motion.
 * Motion is typically only along horizontal plane unless disableVerticalMovement is set to false.
 * walkSpeed determines navigation movement speed.
 * Trigger button resets navigation to origin.
 * 
 **/
public class navigation : MonoBehaviour {

	public GameObject wand;					// need tracking info
	public GameObject theHead;					// need tracking info

	public float walkSpeed = 10f;				// how fast to walk
	public bool disableVerticalMovement = true;	// whether you want to constrain walking to horizontal only

	// Use this for initialization
	void Start () {
	
	}

	/* To make it so you can properly land on platforms, you need to move the collider for the player
	 * to where the player's head location is. Otherwise the player collider will remain at the center of
	 * the local VR volume.
	 */
	void AdjustPlayerColliderForPlatforming() {
		
		CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();
		Transform head = theHead.transform;
		Vector3 pos = new Vector3 ();

		pos.x = head.localPosition.x;
		pos.y = playerCollider.center.y;
		pos.z = head.localPosition.z;
		playerCollider.center = pos;
	}

	void Navigate() {
		SteamVR_TrackedObject trackedObj = null;
		SteamVR_Controller.Device device = null;

		/* Have to keep looking for the wands at each iteration for now since the user is
		 * not guaranteed to have them in tracking range.
		 */
		if (wand != null)
		trackedObj = wand.GetComponent<SteamVR_TrackedObject> ();


		if (trackedObj!= null)
			device = SteamVR_Controller.Input ((int)trackedObj.index);

		float x = 0;
		float y = 0;

		if (device != null) {
			x = device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;
			y = device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y;
		}
		Vector3 direction;
		direction = wand.transform.forward;

		if (disableVerticalMovement)
			direction.y = 0f;
		transform.Translate (direction * y * walkSpeed * Time.deltaTime);

		if (device != null)
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			transform.position = new Vector3 (0, 0, 0);
		}
	}

	// Update is called once per frame
	void Update () {
		Navigate ();
		AdjustPlayerColliderForPlatforming ();
	}
	
}
