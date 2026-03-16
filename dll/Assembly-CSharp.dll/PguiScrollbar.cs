using System;
using SGNFW.uGUI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PguiScrollbar : Scrollbar, IEndDragHandler, IEventSystemHandler
{
	public int PguiBehaviourVersion { get; set; }

	protected override void Awake()
	{
		base.navigation = new Navigation
		{
			mode = Navigation.Mode.None
		};
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		if (this.fixedScrollRect != null)
		{
			this.fixedScrollRect.OnBeginDrag(eventData);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (this.fixedScrollRect != null)
		{
			this.fixedScrollRect.OnEndDrag(eventData);
		}
	}

	public void SetScrollBarValue(float value)
	{
		base.value = value;
	}

	public FixedScrollRect fixedScrollRect;

	public ScrollRect scrollRect;
}
