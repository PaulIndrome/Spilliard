using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShove : MonoBehaviour {
    
	[SerializeField] float shoveRange = 5f, shoveForce = 10f, shoveAnimationDistance = 1f, shoveRunupTime = 0.5f;
	[SerializeField] LayerMask IShovableMask;
	Transform raycastOrigin;
	float sphereCastRadius = 0.25f;
	RaycastHit hit;
	GuestBase currentTarget;

    // Use this for initialization
    void Start () {
		raycastOrigin = GetComponentInChildren<RaycastOrigin>().transform;
		
		SphereCollider c = raycastOrigin.GetComponent<SphereCollider>();
		if(c != null)
			sphereCastRadius = c.radius;

		InteractionButton.ClickEvent += StartShove;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void StartShove(){
		Debug.DrawRay(raycastOrigin.position + (transform.right * sphereCastRadius), transform.forward * shoveRange, Color.red, 5f);
		Debug.DrawRay(raycastOrigin.position - (transform.right * sphereCastRadius), transform.forward * shoveRange, Color.red, 5f);
		if(Physics.SphereCast(raycastOrigin.position, sphereCastRadius, transform.forward, out hit, shoveRange, IShovableMask, QueryTriggerInteraction.Ignore)){
			Debug.Log(gameObject.name + " found " + hit.transform.gameObject.name + " to shove");
			currentTarget = hit.transform.GetComponent<GuestBase>();
			if(currentTarget != null && !currentTarget.shoved){
				MoveToShoveTarget();
			}
		}
	}

	void MoveToShoveTarget(){
		transform.LookAt(new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z), Vector3.up);
		Vector3 distanceVector = (hit.point - transform.position) - transform.forward * shoveAnimationDistance;
		distanceVector.y = 0f;
		float distanceFraction = distanceVector.magnitude / shoveRange;
		Debug.DrawRay(raycastOrigin.position, distanceVector, Color.yellow, 5f);
		Vector3[] path = new Vector3[]{transform.position - (transform.forward * shoveRange * (1-distanceFraction)), transform.position + distanceVector};
		iTween.MoveTo(gameObject, iTween.Hash("path", path, "position",  transform.position + distanceVector, "time", shoveRunupTime, "easetype", iTween.EaseType.easeInCubic, "oncomplete", "ShoveTarget"));
	}

	void ShoveTarget(){
		currentTarget.GetShoved(shoveForce, transform.forward);
	}
	void OnDestroy(){
		InteractionButton.ClickEvent -= StartShove;
	}
}
