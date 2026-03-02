using System;
using System.Collections.Generic;
using SGNFW.Touch;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001E6 RID: 486
[RequireComponent(typeof(Graphic))]
public class PguiTouchTrigger : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x06002070 RID: 8304 RVA: 0x0018B50E File Offset: 0x0018970E
	public UnityEvent onClickStart
	{
		get
		{
			return this.m_EventClickTrigger;
		}
	}

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06002071 RID: 8305 RVA: 0x0018B516 File Offset: 0x00189716
	public UnityEvent onPressStart
	{
		get
		{
			return this.m_EventPressTrigger;
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06002072 RID: 8306 RVA: 0x0018B51E File Offset: 0x0018971E
	public UnityEvent onPressEnd
	{
		get
		{
			return this.m_EventPressEndTrigger;
		}
	}

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06002073 RID: 8307 RVA: 0x0018B526 File Offset: 0x00189726
	public UnityEvent onLongPressStart
	{
		get
		{
			return this.m_EventLongPressTrigger;
		}
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06002074 RID: 8308 RVA: 0x0018B52E File Offset: 0x0018972E
	public UnityEvent onLongPressEnd
	{
		get
		{
			return this.m_EventLongPressEndTrigger;
		}
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06002075 RID: 8309 RVA: 0x0018B536 File Offset: 0x00189736
	// (set) Token: 0x06002076 RID: 8310 RVA: 0x0018B53E File Offset: 0x0018973E
	public Vector2 CurrentPosition { get; private set; } = Vector2.zero;

	// Token: 0x06002077 RID: 8311 RVA: 0x0018B548 File Offset: 0x00189748
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

	// Token: 0x06002078 RID: 8312 RVA: 0x0018B5C2 File Offset: 0x001897C2
	private void Awake()
	{
		this.Refresh();
	}

	// Token: 0x06002079 RID: 8313 RVA: 0x0018B5CA File Offset: 0x001897CA
	private void OnDisable()
	{
		this.drag = false;
		this.chk = 0;
		this.pointerUp();
		this.m_callEventClick = false;
	}

	// Token: 0x0600207A RID: 8314 RVA: 0x0018B5E7 File Offset: 0x001897E7
	private void OnDestroy()
	{
		this.scroll = null;
		SGNFW.Touch.Manager.UnRegisterLongPress(new SGNFW.Touch.Manager.SingleAction(this.OnLongPress));
	}

	// Token: 0x0600207B RID: 8315 RVA: 0x0018B604 File Offset: 0x00189804
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

	// Token: 0x0600207C RID: 8316 RVA: 0x0018B670 File Offset: 0x00189870
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

	// Token: 0x0600207D RID: 8317 RVA: 0x0018B6CC File Offset: 0x001898CC
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

	// Token: 0x0600207E RID: 8318 RVA: 0x0018B71C File Offset: 0x0018991C
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

	// Token: 0x0600207F RID: 8319 RVA: 0x0018B77A File Offset: 0x0018997A
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

	// Token: 0x06002080 RID: 8320 RVA: 0x0018B7B8 File Offset: 0x001899B8
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

	// Token: 0x06002081 RID: 8321 RVA: 0x0018B81C File Offset: 0x00189A1C
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

	// Token: 0x06002082 RID: 8322 RVA: 0x0018B874 File Offset: 0x00189A74
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

	// Token: 0x06002083 RID: 8323 RVA: 0x0018B8E1 File Offset: 0x00189AE1
	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			return;
		}
		this.OnDisable();
	}

	// Token: 0x0400178F RID: 6031
	private FixedScrollRect scroll;

	// Token: 0x04001790 RID: 6032
	private bool drag;

	// Token: 0x04001791 RID: 6033
	private int chk;

	// Token: 0x04001793 RID: 6035
	[SerializeField]
	private UnityEvent m_EventLongPressTrigger = new UnityEvent();

	// Token: 0x04001794 RID: 6036
	[SerializeField]
	private UnityEvent m_EventClickTrigger = new UnityEvent();

	// Token: 0x04001795 RID: 6037
	[SerializeField]
	private UnityEvent m_EventLongPressEndTrigger = new UnityEvent();

	// Token: 0x04001796 RID: 6038
	[SerializeField]
	private UnityEvent m_EventPressTrigger = new UnityEvent();

	// Token: 0x04001797 RID: 6039
	[SerializeField]
	private UnityEvent m_EventPressEndTrigger = new UnityEvent();

	// Token: 0x04001798 RID: 6040
	private Graphic m_RaycastObj;

	// Token: 0x04001799 RID: 6041
	private bool m_callEventLongPress;

	// Token: 0x0400179A RID: 6042
	private bool m_callEventPress;

	// Token: 0x0400179B RID: 6043
	private bool m_callEventClick;
}
