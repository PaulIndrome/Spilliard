using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour {

	[SerializeField] float pickupRange = 5f, pickupAnimationDistance = 0.5f, pickupRunupTime = 0.5f, pickupAnimationTime = 0.5f;
	[SerializeField] LayerMask pickupMask;
	[SerializeField] Vector3Path pickupPath;
	Transform raycastOrigin;
	RaycastHit hit;
	GameObject carrier;
	static GuestBase currentTarget;
	public static GuestBase CurrentTarget { private set { currentTarget = value; } get { return currentTarget; }}
	PlayerThrow playerThrow;
	float sphereCastRadius = 0.25f;
	static bool holdingGuest = false;
	public static bool HoldingGuest { private set { 
		holdingGuest = value;
		if(!holdingGuest) CurrentTarget = null;
		Debug.LogError("CurrentTarget == " + CurrentTarget);
	} get { return holdingGuest; } }

	void Start(){
		raycastOrigin = GetComponentInChildren<RaycastOrigin>().transform;
		playerThrow = GetComponent<PlayerThrow>();
		
		SphereCollider c = raycastOrigin.GetComponent<SphereCollider>();
		if(c != null)
			sphereCastRadius = c.radius;

		InteractionButton.HoldEvent += StartPickup;
	}

	public void StartPickup(){
		if(!HoldingGuest && Physics.SphereCast(raycastOrigin.position, sphereCastRadius, transform.forward, out hit, pickupRange, pickupMask, QueryTriggerInteraction.Ignore)){
			Debug.DrawRay(raycastOrigin.position + (transform.right * sphereCastRadius), transform.forward * pickupRange, Color.red, 5f);
			Debug.DrawRay(raycastOrigin.position - (transform.right * sphereCastRadius), transform.forward * pickupRange, Color.red, 5f);
			
			CurrentTarget = hit.transform.GetComponent<GuestBase>();
			if(CurrentTarget != null && !CurrentTarget.shoved){
				Debug.Log(gameObject.name + " found " + hit.transform.gameObject.name + " to pick up");
				HoldingGuest = true;
				MoveToPickup();
			} else {
				HoldingGuest = false;
			}
		}
	}

	public void MoveToPickup(){
		Debug.Log("MoveToPickup");
		transform.LookAt(new Vector3(CurrentTarget.transform.position.x, transform.position.y, CurrentTarget.transform.position.z), Vector3.up);
		Vector3 distanceVector = (hit.point - transform.position) - transform.forward * pickupAnimationDistance;
		distanceVector.y = 0f;
		float distanceFraction = distanceVector.magnitude / pickupRange;
		Debug.DrawRay(raycastOrigin.position, distanceVector, Color.yellow, 5f);

		System.Action next = this.PickupGuest;

		if(distanceVector.magnitude > pickupAnimationDistance){
			Vector3[] path = new Vector3[]{transform.position, transform.position + distanceVector};
			iTween.MoveTo(gameObject, iTween.Hash("path", path, "position",  transform.position + distanceVector, "time", pickupRunupTime, "easetype", iTween.EaseType.easeOutExpo, "oncomplete", next.Method.Name));
		} else {
			PickupGuest();
		}
	}	

	public void PickupGuest(){
		Debug.Log("PickupTarget");
		CurrentTarget.ToggleKinematic(true);
		
		carrier = new GameObject("carrier");
		carrier.transform.parent = transform;
		carrier.transform.position = CurrentTarget.transform.position;
		
		CurrentTarget.transform.parent = carrier.transform;

		Vector3[] tempPath = new Vector3[pickupPath.path.Length+1];
		tempPath[0] = carrier.transform.localPosition;
		for(int i = 1;i<tempPath.Length;i++){
			tempPath[i] = pickupPath.path[i-1];
		}

		System.Action next = this.FinishPickup;

		iTween.RotateTo(CurrentTarget.transform.gameObject, iTween.Hash("islocal", true, "rotation", new Vector3(0f,0f,90f), "time", pickupAnimationTime));

		iTween.MoveTo(carrier, iTween.Hash(	"movetopath", false, "path", tempPath,
																		"position", gameObject.transform.position + Vector3.up * 5f, 
																		"time", pickupAnimationTime, "easetype", iTween.EaseType.easeInExpo, 
																		"oncomplete", next.Method.Name, "islocal", true,
																		"oncompletetarget", this.gameObject));
	}

	public void FinishPickup(){
		Debug.Log("FinishPickup");
		// TO DO fix NullRefereneException when picking up multiple guests in a row
		CurrentTarget.transform.parent = transform;
		Destroy(carrier, 1f);
		CurrentTarget.GetPickedUp();
		HoldingGuest = true;
	}

	public static void ThrowGuest(Vector3 velocity){
		CurrentTarget.GetThrown(velocity);
		HoldingGuest = false;
	}

	public static void GuestJumpOff(){
		HoldingGuest = false;
	}

	void OnDestroy(){
		InteractionButton.HoldEvent -= StartPickup;
	}


}
