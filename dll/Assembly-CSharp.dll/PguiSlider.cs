using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001E0 RID: 480
public class PguiSlider : Slider
{
	// Token: 0x06002032 RID: 8242 RVA: 0x0018A4F4 File Offset: 0x001886F4
	protected override void Awake()
	{
		base.navigation = new Navigation
		{
			mode = Navigation.Mode.None
		};
	}

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06002033 RID: 8243 RVA: 0x0018A518 File Offset: 0x00188718
	// (set) Token: 0x06002034 RID: 8244 RVA: 0x0018A520 File Offset: 0x00188720
	public bool touch { get; private set; }

	// Token: 0x06002035 RID: 8245 RVA: 0x0018A529 File Offset: 0x00188729
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (eventData != null && eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		base.OnPointerDown(eventData);
		this.touch = true;
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x0018A545 File Offset: 0x00188745
	public override void OnPointerUp(PointerEventData eventData)
	{
		if (eventData != null && eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		base.OnPointerUp(eventData);
		this.touch = false;
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x0018A561 File Offset: 0x00188761
	protected override void OnEnable()
	{
		base.OnEnable();
		this.touch = false;
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x0018A570 File Offset: 0x00188770
	protected override void OnDisable()
	{
		base.OnEnable();
		this.touch = false;
	}
}
