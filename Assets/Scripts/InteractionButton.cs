using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public delegate void InteractionDelegate();
	public static event InteractionDelegate ClickEvent, HoldEvent;
	public static bool pointerDown = false;

	bool holdEventFired = false;
	float pointerDownTime;
	[SerializeField] float holdTime = 0.3f;

	void Start(){
	}

	void Update(){
		if(!holdEventFired && pointerDown && Time.time - pointerDownTime > holdTime){
			FireHoldEvent();
		}
		if(Input.GetKey(KeyCode.Q)){
			if(!holdEventFired && pointerDown && Time.time - pointerDownTime > holdTime){
				FireHoldEvent();
			} else if (!pointerDown){
				OnPointerDown(null);
			}
		} else if(Input.GetKeyUp(KeyCode.Q)){
			OnPointerUp(null);
		}
	}

	void FireHoldEvent(){
		holdEventFired = true;
		if(HoldEvent != null) HoldEvent();
	}

    public void OnPointerDown(PointerEventData eventData)
    {
		pointerDown = true;
    	pointerDownTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
		if(!holdEventFired){
			if(ClickEvent != null) ClickEvent();
		}
		pointerDown = holdEventFired = false;
    }

	void OnDestroy(){
		HoldEvent -= FireHoldEvent;
	}
}
