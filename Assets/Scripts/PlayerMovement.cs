using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] float speed = 5f, carrySpeedPercentage = 0.1f;
	Vector3 movement;

	void Start(){
	}
	
	// Update is called once per frame
	void Update () {
		if(!PlayerThrow.targeting && !PlayerThrow.throwing){
			movement = JoystickKnob.PositionMagnitude;
			if(movement.z > 0f){
				Vector3 newPos = transform.position + new Vector3(movement.x, 0, movement.y) * movement.z;
				transform.LookAt(newPos, Vector3.up);
				transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * speed * (PlayerPickup.HoldingGuest ? carrySpeedPercentage : 1f));
			}
		}		
	}


}