using System;
using System.Collections.Generic;
using SGNFW.Touch;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class PguiTouchTrigger : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	public UnityEvent onClickStart
	{
		get
		{
			return this.m_EventClickTrigger;
		}
	}

	public UnityEvent onPressStart
	{
		get
		{
			return this.m_EventPressTrigger;
		}
	}

	public UnityEvent onPressEnd
	{
		get
		{
			return this.m_EventPressEndTrigger;
		}
	}

	public UnityEvent onLongPressStart
	{
		get
		{
			return this.m_EventLongPressTrigger;
		}
	}

	public UnityEvent onLongPressEnd
	{
		get
		{
			return this.m_EventLongPressEndTrigger;
		}
	}

	public Vector2 CurrentPosition { get; private set; } = Vector2.zero;

	private void Refresh()
	{
		this.m_RaycastObj = base.GetComponent<Graphic>();
		this.m_callEventLongPress = false;
		this.m_callEventPress = false;
		this.m_callEventClick = false;
		this.scroll = base.transform.GetComponentInParent<FixedScrollRect>();
		this.drag = false;
		this.chk = 0;
		this.CurrentPosition = Vector2.zero;
		SGNFW.Touch.Manager.UnRegisterLongPress(new SGNFW.Touch.Manager.SingleAction(this.OnLongPress));
		SGNFW.Touch.Manager.RegisterLongPress(new SGNFW.Touch.Manager.SingleAction(this.OnLongPress));
	}

	private void Awake()
	{
		this.Refresh();
	}

	private void OnDisable()
	{
		this.drag = false;
		this.chk = 0;
		this.pointerUp();
		this.m_callEventClick = false;
	}

	private void OnDestroy()
	{
		this.scroll = null;
		SGNFW.Touch.Manager.UnRegisterLongPress(new SGNFW.Touch.Manager.SingleAction(this.OnLongPress));
	}

	private void Update()
	{
		if (this.drag && (this.scroll == null || !this.scroll.IsDrag))
		{
			this.drag = false;
			this.pointerUp();
		}
		if (this.m_callEventPress && !Input.GetMouseButton(0))
		{
			int num = this.chk + 1;
			this.chk = num;
			if (num > 5)
			{
				this.OnDisable();
			}
		}
	}

	public void OnPointerDown(PointerEventData baseEventData)
	{
		if (baseEventData != null && baseEventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.CurrentPosition = baseEventData.position;
		this.m_callEventPress = true;
		this.m_callEventClick = true;
		this.m_callEventLongPress = false;
		if (this.m_EventPressTrigger != null)
		{
			this.m_EventPressTrigger.Invoke();
		}
		this.drag = false;
		this.chk = 0;
	}

	public void OnPointerUp(PointerEventData baseEventData)
	{
		if (baseEventData != null && baseEventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.CurrentPosition = baseEventData.position;
		if (this.scroll != null && this.scroll.IsDrag)
		{
			this.drag = true;
			return;
		}
		this.pointerUp();
	}

	private void pointerUp()
	{
		if (!this.m_callEventPress)
		{
			return;
		}
		this.m_callEventPress = false;
		this.m_RaycastObj.raycastTarget = true;
		if (this.m_EventPressEndTrigger != null)
		{
			this.m_EventPressEndTrigger.Invoke();
		}
		if (this.m_callEventLongPress && this.m_EventLongPressEndTrigger != null)
		{
			this.m_EventLongPressEndTrigger.Invoke();
		}
		this.m_callEventLongPress = false;
	}

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (pointerEventData != null && pointerEventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.CurrentPosition = pointerEventData.position;
		if (!this.m_callEventClick)
		{
			return;
		}
		this.m_callEventClick = false;
		if (this.m_EventClickTrigger != null)
		{
			this.m_EventClickTrigger.Invoke();
		}
	}

	private void OnLongPress(Info info)
	{
		this.CurrentPosition = info.CurrentPosition;
		this.m_callEventClick = false;
		if (!this.m_callEventLongPress && this.m_callEventPress && this.hit(info.CurrentPosition))
		{
			this.m_callEventLongPress = true;
			this.m_RaycastObj.raycastTarget = false;
			if (this.m_EventLongPressTrigger != null)
			{
				this.m_EventLongPressTrigger.Invoke();
			}
		}
	}

	private bool hit(Vector2 pos)
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
		{
			position = pos
		};
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list.Count > 0 && list[0].gameObject == base.gameObject;
	}

	public void AddListener(UnityAction click, UnityAction longstart = null, UnityAction longend = null, UnityAction pressstart = null, UnityAction pressend = null)
	{
		base.gameObject.GetComponent<Graphic>().raycastTarget = true;
		if (click != null)
		{
			this.onClickStart.AddListener(click);
		}
		if (longstart != null)
		{
			this.onLongPressStart.AddListener(longstart);
		}
		if (longend != null)
		{
			this.onLongPressEnd.AddListener(longend);
		}
		if (pressstart != null)
		{
			this.onPressStart.AddListener(pressstart);
		}
		if (pressend != null)
		{
			this.onPressEnd.AddListener(pressend);
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			return;
		}
		this.OnDisable();
	}

	private FixedScrollRect scroll;

	private bool drag;

	private int chk;

	[SerializeField]
	private UnityEvent m_EventLongPressTrigger = new UnityEvent();

	[SerializeField]
	private UnityEvent m_EventClickTrigger = new UnityEvent();

	[SerializeField]
	private UnityEvent m_EventLongPressEndTrigger = new UnityEvent();

	[SerializeField]
	private UnityEvent m_EventPressTrigger = new UnityEvent();

	[SerializeField]
	private UnityEvent m_EventPressEndTrigger = new UnityEvent();

	private Graphic m_RaycastObj;

	private bool m_callEventLongPress;

	private bool m_callEventPress;

	private bool m_callEventClick;
}
