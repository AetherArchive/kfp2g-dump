using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialEventTrigger : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		ExecuteEvents.Execute<IPointerClickHandler>(this.targetRaycastResult.gameObject, eventData, delegate(IPointerClickHandler handler, BaseEventData ed)
		{
			handler.OnPointerClick((PointerEventData)ed);
		});
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, list);
		foreach (RaycastResult raycastResult in list)
		{
			if (!this.maskObjectList.Contains(raycastResult.gameObject))
			{
				this.targetRaycastResult = raycastResult;
				break;
			}
		}
		ExecuteEvents.Execute<IPointerEnterHandler>(this.targetRaycastResult.gameObject, eventData, delegate(IPointerEnterHandler handler, BaseEventData ed)
		{
			handler.OnPointerEnter((PointerEventData)ed);
		});
		ExecuteEvents.Execute<IPointerDownHandler>(this.targetRaycastResult.gameObject, eventData, delegate(IPointerDownHandler handler, BaseEventData ed)
		{
			handler.OnPointerDown((PointerEventData)ed);
		});
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		ExecuteEvents.Execute<IPointerUpHandler>(this.targetRaycastResult.gameObject, eventData, delegate(IPointerUpHandler handler, BaseEventData ed)
		{
			handler.OnPointerUp((PointerEventData)ed);
		});
	}

	private RaycastResult targetRaycastResult;

	public List<GameObject> maskObjectList = new List<GameObject>();
}
