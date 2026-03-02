using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000109 RID: 265
public class TutorialEventTrigger : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x06000CD6 RID: 3286 RVA: 0x0004E610 File Offset: 0x0004C810
	public void OnPointerClick(PointerEventData eventData)
	{
		ExecuteEvents.Execute<IPointerClickHandler>(this.targetRaycastResult.gameObject, eventData, delegate(IPointerClickHandler handler, BaseEventData ed)
		{
			handler.OnPointerClick((PointerEventData)ed);
		});
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x0004E644 File Offset: 0x0004C844
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

	// Token: 0x06000CD8 RID: 3288 RVA: 0x0004E71C File Offset: 0x0004C91C
	public void OnPointerUp(PointerEventData eventData)
	{
		ExecuteEvents.Execute<IPointerUpHandler>(this.targetRaycastResult.gameObject, eventData, delegate(IPointerUpHandler handler, BaseEventData ed)
		{
			handler.OnPointerUp((PointerEventData)ed);
		});
	}

	// Token: 0x04000A2F RID: 2607
	private RaycastResult targetRaycastResult;

	// Token: 0x04000A30 RID: 2608
	public List<GameObject> maskObjectList = new List<GameObject>();
}
