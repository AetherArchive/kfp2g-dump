using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PguiSlider : Slider
{
	protected override void Awake()
	{
		base.navigation = new Navigation
		{
			mode = Navigation.Mode.None
		};
	}

	public bool touch { get; private set; }

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (eventData != null && eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		base.OnPointerDown(eventData);
		this.touch = true;
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (eventData != null && eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		base.OnPointerUp(eventData);
		this.touch = false;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		this.touch = false;
	}

	protected override void OnDisable()
	{
		base.OnEnable();
		this.touch = false;
	}
}
