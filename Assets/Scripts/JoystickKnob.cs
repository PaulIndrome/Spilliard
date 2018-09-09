using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickKnob : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

	Camera cam;
	RectTransform cageRect, knobRect;
	Vector2 pos;
	// y component is "velocity"
	static Vector3 positionMagnitude;
	Vector3 tempPosMag;
	public static Vector3 PositionMagnitude {
		set{
			positionMagnitude = value;
		}
		get {
			return positionMagnitude;
		}
	}

	[SerializeField] float maxMagnitude = 10f;

	public void Start(){
		cam = Camera.main;
		cageRect = GetComponent<RectTransform>();
		knobRect = transform.GetChild(0).GetComponent<RectTransform>();
	}

    public void OnDrag(PointerEventData eventData)
    {
		/*if(RectTransformUtility.ScreenPointToLocalPointInRectangle(cageRect, eventData.position, eventData.pressEventCamera, out pos)){
			pos = pos / cageRect.sizeDelta;
			float x = (cageRect.pivot.x == 1 ) ? pos.x * 2 + 1 : pos.x * 2 -1;
			float y = (cageRect.pivot.y == 1 ) ? pos.y * 2 + 1 : pos.y * 2 -1;
			PositionMagnitude = new Vector3(x, y, 0);
			PositionMagnitude = PositionMagnitude.magnitude > 1 ? PositionMagnitude.normalized : PositionMagnitude;
			knobRect.anchoredPosition = PositionMagnitude * (cageRect.sizeDelta / 3);
		}*/

		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(cageRect, eventData.position, eventData.pressEventCamera, out pos)){
			knobRect.localPosition = pos;
			knobRect.anchoredPosition = Vector2.ClampMagnitude(knobRect.anchoredPosition, maxMagnitude);
			PositionMagnitude = new Vector3(knobRect.anchoredPosition.x, knobRect.anchoredPosition.y, knobRect.anchoredPosition.magnitude) / maxMagnitude;
		}

	}

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
		PositionMagnitude = Vector3.zero;
		knobRect.anchoredPosition = Vector3.zero;
    }

}
