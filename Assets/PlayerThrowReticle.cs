using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowReticle : MonoBehaviour {

	Vector3 movement, playerThrowPos;
	static Vector3 currentPosition;
	[SerializeField] float speed = 2f, floatHeight = 1f, maxThrowDistance = 10f;
	GameObject reticle3D;
	[SerializeField] LayerMask reticleMask;
	Ray ray;
	RaycastHit hit;

	// Use this for initialization
	void Start () {
		reticle3D = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerThrow.targeting && !PlayerThrow.throwing){
			movement = JoystickKnob.PositionMagnitude;
			if(movement.z > 0f){
				Vector3 newPos = Vector3.Lerp(transform.position, transform.position + new Vector3(movement.x, 0, movement.y), Time.deltaTime * speed);
				Vector3 vectorToTarget = newPos - playerThrowPos;
				float ratio = vectorToTarget.magnitude / maxThrowDistance;
				if(ratio > 1f){
					newPos -= vectorToTarget * (ratio - 1f);
				}
				ray = new Ray(new Vector3(newPos.x, 5, newPos.z), -Vector3.up);
				Debug.DrawRay(new Vector3(newPos.x, 5, newPos.z), -Vector3.up * 10f, Color.blue, 1f);
				if(Physics.Raycast(ray, out hit, 10f, reticleMask)){
					transform.position = hit.point;
					transform.LookAt(hit.point + hit.normal);
				} else {
					newPos.y = floatHeight;
					transform.position = newPos;
					transform.LookAt(transform.position + Vector3.up);
				}
				currentPosition = transform.position;
			}
		}
	}

	public static Vector3 GetReticlePosition(){
		return currentPosition;
	}

	public void ToggleVisuals(bool onOff, Vector3 playerPos){
		playerThrowPos = playerPos;
		playerThrowPos.y = floatHeight;
		transform.position = playerThrowPos;
		reticle3D.SetActive(onOff);
	}
}
