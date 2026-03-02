using System;
using SGNFW.uGUI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001DE RID: 478
public class PguiScrollbar : Scrollbar, IEndDragHandler, IEventSystemHandler
{
	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06002027 RID: 8231 RVA: 0x0018A3A7 File Offset: 0x001885A7
	// (set) Token: 0x06002028 RID: 8232 RVA: 0x0018A3AF File Offset: 0x001885AF
	public int PguiBehaviourVersion { get; set; }

	// Token: 0x06002029 RID: 8233 RVA: 0x0018A3B8 File Offset: 0x001885B8
	protected override void Awake()
	{
		base.navigation = new Navigation
		{
			mode = Navigation.Mode.None
		};
	}

	// Token: 0x0600202A RID: 8234 RVA: 0x0018A3DC File Offset: 0x001885DC
	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		if (this.fixedScrollRect != null)
		{
			this.fixedScrollRect.OnBeginDrag(eventData);
		}
	}

	// Token: 0x0600202B RID: 8235 RVA: 0x0018A3FF File Offset: 0x001885FF
	public void OnEndDrag(PointerEventData eventData)
	{
		if (this.fixedScrollRect != null)
		{
			this.fixedScrollRect.OnEndDrag(eventData);
		}
	}

	// Token: 0x0600202C RID: 8236 RVA: 0x0018A41B File Offset: 0x0018861B
	public void SetScrollBarValue(float value)
	{
		base.value = value;
	}

	// Token: 0x04001756 RID: 5974
	public FixedScrollRect fixedScrollRect;

	// Token: 0x04001757 RID: 5975
	public ScrollRect scrollRect;
}
