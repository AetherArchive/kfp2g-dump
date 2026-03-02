using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000195 RID: 405
public class ClickEventTrigger : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06001B06 RID: 6918 RVA: 0x0015C981 File Offset: 0x0015AB81
	// (set) Token: 0x06001B05 RID: 6917 RVA: 0x0015C978 File Offset: 0x0015AB78
	public UnityAction<Transform> callback { get; set; }

	// Token: 0x06001B07 RID: 6919 RVA: 0x0015C989 File Offset: 0x0015AB89
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
