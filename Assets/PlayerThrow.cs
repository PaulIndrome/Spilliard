using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour {

	[SerializeField] float throwAngle = 33f;
	public static bool targeting = false, throwing = false;
	Coroutine targetingRoutine;
	PlayerThrowReticle reticle;
	[SerializeField] Cinemachine.CinemachineVirtualCamera playerTargetingCam;

	// Use this for initialization
	void Start () {
		if(playerTargetingCam == null) playerTargetingCam = GameObject.Find("vcam_playerTargeting").GetComponent<Cinemachine.CinemachineVirtualCamera>();
		InteractionButton.HoldEvent += StartTargeting;
		reticle = FindObjectOfType<PlayerThrowReticle>();
	}
	
	public void StartTargeting(){
		if(PlayerPickup.HoldingGuest && targetingRoutine == null){
			reticle.ToggleVisuals(true, transform.position);
			playerTargetingCam.Priority = 10;
			targetingRoutine = StartCoroutine(Targeting());
		} else if(targetingRoutine != null) {
			StopCoroutine(targetingRoutine);
			targetingRoutine = null;
		}
	}

	public void ThrowGuestAtTarget(Vector3 reticlePosition){
		if(!PlayerPickup.HoldingGuest) return;
		reticle.ToggleVisuals(false, transform.position);
		playerTargetingCam.Priority = -10;
		throwing = true;
		targeting = false;
		Debug.Log("Throw! (not implemented)");

		Vector3 p = reticlePosition;

		float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = throwAngle * Mathf.Deg2Rad;
 
        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);
 
        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;
 
        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
 
        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
 
        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
 
        // Fire!
        PlayerPickup.ThrowGuest(finalVelocity);
		throwing = false;
	}

	IEnumerator Targeting(){
		targeting = true;
		while(InteractionButton.pointerDown){
			if(!PlayerPickup.HoldingGuest) {
				Debug.LogWarning("Targeting broken");
				targeting = false;
				throwing = false;
				reticle.ToggleVisuals(false, Vector3.zero);
				playerTargetingCam.Priority = -10;
				targetingRoutine = null;
				yield break;
			}
			yield return null;
		}
		ThrowGuestAtTarget(PlayerThrowReticle.GetReticlePosition());
		targetingRoutine = null;
		yield return null;
	}
}
