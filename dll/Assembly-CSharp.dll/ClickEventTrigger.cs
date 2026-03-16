using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickEventTrigger : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public UnityAction<Transform> callback { get; set; }

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData != null && eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (this.callback != null)
		{
			this.callback(base.transform);
		}
	}
}
