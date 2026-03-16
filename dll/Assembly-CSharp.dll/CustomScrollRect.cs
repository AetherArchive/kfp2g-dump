using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class CustomScrollRect : MonoBehaviour
{
	[HideInInspector]
	public int ItemNum { get; private set; }

	[HideInInspector]
	public int PrefabNum { get; private set; }

	[HideInInspector]
	public bool IsMoving
	{
		get
		{
			return this.IsAutoMove || 0f != this.ScrollRect.velocity.x || 0f != this.ScrollRect.velocity.y;
		}
	}

	private ScrollRect ScrollRect { get; set; }

	private RectTransform ContentRectTransform { get; set; }

	private List<GameObject> ItemObjectList { get; set; }

	private bool IsInit { get; set; }

	private int TargetIndex { get; set; }

	private int NowHeadIndex { get; set; }

	private int NowFocusedIndex { get; set; }

	private bool IsAutoMove { get; set; }

	private int AutoMoveTargetIndex { get; set; }

	private float AutoMoveStart { get; set; }

	private float AutoMoveEnd { get; set; }

	private float AutoMoveElapsedTime { get; set; }

	private void Awake()
	{
		if (this.IsInit)
		{
			return;
		}
		this.ScrollRect = base.GetComponent<ScrollRect>();
		this.ScrollRect.horizontal = CustomScrollRect.Direction.Horizontal == this.direction;
		this.ScrollRect.vertical = this.direction == CustomScrollRect.Direction.Vertical;
		this.ScrollRect.scrollSensitivity = (float)(this.ScrollRect.vertical ? this.itemSize : (this.itemSize * -1));
		if (this.isLoop)
		{
			this.ScrollRect.movementType = ScrollRect.MovementType.Unrestricted;
		}
		else
		{
			this.ScrollRect.movementType = ScrollRect.MovementType.Elastic;
		}
		if (null == this.ScrollRect.content)
		{
			return;
		}
		this.ContentRectTransform = this.ScrollRect.content.GetComponent<RectTransform>();
		if (this.ScrollRect.vertical)
		{
			this.ContentRectTransform.anchorMin = new Vector2(0f, 1f);
			this.ContentRectTransform.anchorMax = new Vector2(1f, 1f);
			this.ContentRectTransform.anchoredPosition = new Vector2(this.ContentRectTransform.anchoredPosition.x, 0f);
			if (this.ScrollRect.verticalScrollbar != null)
			{
				RectTransform component = this.ScrollRect.verticalScrollbar.transform.GetChild(0).Find("Handle").GetComponent<RectTransform>();
				if (component.sizeDelta.x == ScrollParamDefine.BaseHandleRange)
				{
					component.sizeDelta = new Vector2(component.sizeDelta.x * (float)ScrollParamDefine.HandleAdditionalFactor, component.sizeDelta.y);
					return;
				}
			}
		}
		else
		{
			this.ContentRectTransform.anchorMin = new Vector2(0f, 0f);
			this.ContentRectTransform.anchorMax = new Vector2(0f, 1f);
			this.ContentRectTransform.anchoredPosition = new Vector2(0f, this.ContentRectTransform.anchoredPosition.y);
			if (this.ScrollRect.horizontalScrollbar != null)
			{
				RectTransform component2 = this.ScrollRect.horizontalScrollbar.transform.GetChild(0).Find("Handle").GetComponent<RectTransform>();
				if (component2.sizeDelta.y == ScrollParamDefine.BaseHandleRange)
				{
					component2.sizeDelta = new Vector2(component2.sizeDelta.x, component2.sizeDelta.y * (float)ScrollParamDefine.HandleAdditionalFactor);
				}
			}
		}
	}

	public void Refresh()
	{
		this.requestReflesh = true;
	}

	public void Initialize(int itemnum)
	{
		this.IsInit = false;
		this.DestroyScrollObjects();
		this.NowHeadIndex = -1;
		this.NowFocusedIndex = -1;
		this.ItemNum = itemnum;
		this.CreateScrollObject();
		this.ChangeFocusItem(0, true);
		this.IsInit = true;
	}

	public void ChangeFocusItem(int index, bool force = false)
	{
		if (this.ItemNum <= index)
		{
			return;
		}
		this.NowFocusedIndex = -1;
		if (force)
		{
			if (this.ScrollRect.vertical)
			{
				this.ContentRectTransform.anchoredPosition = new Vector2(this.ContentRectTransform.anchoredPosition.x, this.ScrollMargin.start + (float)(this.itemSize * index) - (float)this.focusOffset);
			}
			else
			{
				this.ContentRectTransform.anchoredPosition = new Vector2(-(this.ScrollMargin.start + (float)(this.itemSize * index) - (float)this.focusOffset), this.ContentRectTransform.anchoredPosition.y);
			}
			this.ScrollRect.StopMovement();
			return;
		}
		this.IsAutoMove = true;
		this.AutoMoveTargetIndex = index;
		this.AutoMoveElapsedTime = 0f;
		if (this.ScrollRect.vertical)
		{
			this.AutoMoveStart = this.ContentRectTransform.anchoredPosition.y;
			this.AutoMoveEnd = this.ScrollMargin.start + (float)(this.itemSize * index) - (float)this.focusOffset;
			return;
		}
		this.AutoMoveStart = this.ContentRectTransform.anchoredPosition.x;
		this.AutoMoveEnd = -(this.ScrollMargin.start + (float)(this.itemSize * index) - (float)this.focusOffset);
	}

	private void Update()
	{
		if (!this.IsInit)
		{
			return;
		}
		this.CalcViewItem();
		if (this.IsAutoMove)
		{
			this.AutoMove();
		}
		this.CalcFocusedItem();
		if (this.IsAutoMove)
		{
			return;
		}
		if (this.isAutoSnap)
		{
			this.ScrollSnapItem();
		}
	}

	private void AutoMove()
	{
		float num = this.autoMoveTime;
		this.AutoMoveElapsedTime += Time.deltaTime;
		float num2 = Mathf.Min(this.AutoMoveElapsedTime / num, 1f);
		if (this.ScrollRect.vertical)
		{
			float num3 = Mathf.Lerp(this.AutoMoveStart, this.AutoMoveEnd, num2);
			this.ContentRectTransform.anchoredPosition = new Vector2(this.ContentRectTransform.anchoredPosition.x, num3);
		}
		else
		{
			float num4 = Mathf.Lerp(this.AutoMoveStart, this.AutoMoveEnd, num2);
			this.ContentRectTransform.anchoredPosition = new Vector2(num4, this.ContentRectTransform.anchoredPosition.y);
		}
		if (num < this.AutoMoveElapsedTime)
		{
			if (this.ScrollRect.vertical)
			{
				this.ContentRectTransform.anchoredPosition = new Vector2(this.ContentRectTransform.anchoredPosition.x, this.AutoMoveEnd);
			}
			else
			{
				this.ContentRectTransform.anchoredPosition = new Vector2(this.AutoMoveEnd, this.ContentRectTransform.anchoredPosition.y);
			}
			this.ScrollRect.StopMovement();
			this.IsAutoMove = false;
		}
	}

	private void CreateScrollObject()
	{
		RectTransform component = this.ScrollRect.viewport.GetComponent<RectTransform>();
		this.startMargin = new GameObject();
		this.startMargin.name = "startMargin";
		this.startMargin.transform.SetParent(this.ContentRectTransform, true);
		this.startMargin.transform.localScale = Vector3.one;
		RectTransform rectTransform = this.startMargin.AddComponent<RectTransform>();
		if (this.ScrollRect.vertical)
		{
			rectTransform.anchorMin = new Vector2(0.5f, 1f);
			rectTransform.anchorMax = new Vector2(0.5f, 1f);
			rectTransform.anchoredPosition = new Vector2(0f, -this.ScrollMargin.start / 2f);
			rectTransform.sizeDelta = new Vector2(this.ScrollRect.content.sizeDelta.x, this.ScrollMargin.start);
		}
		else
		{
			rectTransform.anchorMin = new Vector2(0f, 0.5f);
			rectTransform.anchorMax = new Vector2(0f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(this.ScrollMargin.start / 2f, 0f);
			rectTransform.sizeDelta = new Vector2(this.ScrollMargin.start, this.ScrollRect.content.sizeDelta.y);
		}
		this.prefab.GetComponent<RectTransform>();
		this.PrefabNum = (int)component.rect.height / this.itemSize + ((0 < (int)component.rect.height / this.itemSize) ? 1 : 0) + 1;
		this.PrefabNum = ((this.PrefabNum < this.ItemNum) ? this.PrefabNum : this.ItemNum);
		this.ItemObjectList = new List<GameObject>();
		for (int i = 0; i < this.PrefabNum; i++)
		{
			GameObject gameObject = Manager.Create(this.prefab, this.ContentRectTransform, null, null, Layer.UI);
			gameObject.name = i.ToString();
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			if (this.ScrollRect.vertical)
			{
				component2.anchorMin = new Vector2(0.5f, 1f);
				component2.anchorMax = new Vector2(0.5f, 1f);
				component2.anchoredPosition = new Vector2(0f, -((float)this.itemSize * ((float)i + 0.5f)) - this.ScrollMargin.start);
			}
			else
			{
				component2.anchorMin = new Vector2(0f, 0.5f);
				component2.anchorMax = new Vector2(0f, 0.5f);
				component2.anchoredPosition = new Vector2((float)this.itemSize * ((float)i + 0.5f) + this.ScrollMargin.start, 0f);
			}
			this.onStartItem(i, gameObject);
			this.ItemObjectList.Add(gameObject);
		}
		this.endMargin = new GameObject();
		this.endMargin.name = "endMargin";
		this.endMargin.transform.SetParent(this.ContentRectTransform, true);
		this.endMargin.transform.localScale = Vector3.one;
		RectTransform rectTransform2 = this.endMargin.AddComponent<RectTransform>();
		if (this.ScrollRect.vertical)
		{
			rectTransform2.anchorMin = new Vector2(0.5f, 0f);
			rectTransform2.anchorMax = new Vector2(0.5f, 0f);
			rectTransform2.anchoredPosition = new Vector2(0f, this.ScrollMargin.end / 2f);
			rectTransform2.sizeDelta = new Vector2(this.ScrollRect.content.sizeDelta.x, this.ScrollMargin.end);
		}
		else
		{
			rectTransform2.anchorMin = new Vector2(1f, 0.5f);
			rectTransform2.anchorMax = new Vector2(1f, 0.5f);
			rectTransform2.anchoredPosition = new Vector2(this.ScrollMargin.end / 2f, 0f);
			rectTransform2.sizeDelta = new Vector2(this.ScrollMargin.end, this.ScrollRect.content.sizeDelta.y);
		}
		float num = (float)(this.ItemNum * this.itemSize) + this.ScrollMargin.start + this.ScrollMargin.end;
		if (this.ScrollRect.vertical)
		{
			this.ScrollRect.content.sizeDelta = new Vector2(this.ScrollRect.content.sizeDelta.x, num);
			return;
		}
		this.ScrollRect.content.sizeDelta = new Vector2(num, this.ScrollRect.content.sizeDelta.y);
	}

	private void DestroyScrollObjects()
	{
		foreach (object obj in this.ContentRectTransform)
		{
			Object.Destroy(((RectTransform)obj).gameObject);
		}
		this.ItemNum = 0;
	}

	private void CalcViewItem()
	{
		float num;
		if (this.ScrollRect.vertical)
		{
			num = this.ContentRectTransform.anchoredPosition.y - this.ScrollMargin.start;
		}
		else
		{
			num = -(this.ContentRectTransform.anchoredPosition.x - this.ScrollMargin.start);
		}
		int num2 = (int)num / this.itemSize;
		if (!this.requestReflesh && this.NowHeadIndex == num2)
		{
			return;
		}
		this.NowHeadIndex = num2;
		int num3 = 0;
		foreach (GameObject gameObject in this.ItemObjectList)
		{
			RectTransform component = gameObject.GetComponent<RectTransform>();
			int num4 = this.NowHeadIndex % this.PrefabNum;
			int num5;
			if (num3 < num4)
			{
				num5 = (this.NowHeadIndex / this.PrefabNum + 1) * this.PrefabNum + num3;
			}
			else
			{
				num5 = this.NowHeadIndex / this.PrefabNum * this.PrefabNum + num3;
			}
			while (0 > num5)
			{
				num5 += this.PrefabNum;
			}
			while (this.ItemNum <= num5)
			{
				num5 -= this.PrefabNum;
			}
			if (this.ScrollRect.vertical)
			{
				float num6 = -((float)this.itemSize * ((float)num5 + 0.5f)) - this.ScrollMargin.start;
				if (this.requestReflesh || component.anchoredPosition.y != num6)
				{
					component.anchoredPosition = new Vector2(component.anchoredPosition.x, num6);
					this.onUpdateItem(num5, gameObject);
				}
			}
			else
			{
				float num7 = (float)this.itemSize * ((float)num5 + 0.5f) + this.ScrollMargin.start;
				if (this.requestReflesh || component.anchoredPosition.x != num7)
				{
					component.anchoredPosition = new Vector2(num7, component.anchoredPosition.y);
					this.onUpdateItem(num5, gameObject);
				}
			}
			num3++;
		}
		if (this.requestReflesh)
		{
			this.requestReflesh = false;
		}
	}

	private void CalcFocusedItem()
	{
		float num;
		if (this.ScrollRect.vertical)
		{
			num = this.ContentRectTransform.anchoredPosition.y - this.ScrollMargin.start;
		}
		else
		{
			num = -(this.ContentRectTransform.anchoredPosition.x - this.ScrollMargin.start);
		}
		int num2 = ((int)num + this.itemSize / 2 + this.focusOffset) / this.itemSize;
		if (this.IsAutoMove && this.AutoMoveTargetIndex != num2)
		{
			return;
		}
		if (0 >= num2)
		{
			num2 = 0;
		}
		if (this.ItemNum <= num2)
		{
			num2 = this.ItemNum - 1;
		}
		if (this.NowFocusedIndex != num2)
		{
			this.NowFocusedIndex = num2;
			GameObject gameObject = this.ItemObjectList[this.NowFocusedIndex % this.PrefabNum];
			if (this.targetMoveFront)
			{
				gameObject.transform.SetSiblingIndex(this.endMargin.transform.GetSiblingIndex() - 1);
			}
			this.onChangeFocusItem(this.NowFocusedIndex, gameObject);
		}
	}

	private void ScrollSnapItem()
	{
		if (Input.GetMouseButton(0))
		{
			return;
		}
		if (Input.GetMouseButton(1))
		{
			return;
		}
		if (Input.GetMouseButton(2))
		{
			return;
		}
		if (0 < Input.touchCount)
		{
			return;
		}
		if (Mathf.Abs(this.ScrollRect.vertical ? this.ScrollRect.velocity.y : this.ScrollRect.velocity.x) > this.lowLimitVelocity)
		{
			return;
		}
		this.ScrollRect.StopMovement();
		float num;
		if (this.ScrollRect.vertical)
		{
			RectTransform component = this.ItemObjectList[this.NowFocusedIndex % this.PrefabNum].GetComponent<RectTransform>();
			num = component.anchoredPosition.y * -1f - component.rect.height / 2f - (float)this.focusOffset;
		}
		else
		{
			RectTransform component2 = this.ItemObjectList[this.NowFocusedIndex % this.PrefabNum].GetComponent<RectTransform>();
			num = component2.anchoredPosition.x * -1f + component2.rect.width / 2f + (float)this.focusOffset;
		}
		float num2;
		if (this.ScrollRect.vertical)
		{
			num2 = this.ContentRectTransform.anchoredPosition.y;
		}
		else
		{
			num2 = -this.ContentRectTransform.anchoredPosition.x;
		}
		int num3 = (int)num2 - (int)num;
		if (1 > Mathf.Abs(num3))
		{
			return;
		}
		if (0f != this.snapSpeed)
		{
			float num4 = this.snapSpeed;
		}
		float num5 = ((0 > num3) ? Mathf.Max(Time.deltaTime / this.snapSpeed * (float)this.itemSize, (float)num3) : Mathf.Min(Time.deltaTime / this.snapSpeed * (float)this.itemSize, (float)num3));
		if (this.ScrollRect.vertical)
		{
			if (0 > num3)
			{
				this.ContentRectTransform.anchoredPosition = new Vector2(this.ContentRectTransform.anchoredPosition.x, this.ContentRectTransform.anchoredPosition.y + num5);
				return;
			}
			this.ContentRectTransform.anchoredPosition = new Vector2(this.ContentRectTransform.anchoredPosition.x, this.ContentRectTransform.anchoredPosition.y - num5);
			return;
		}
		else
		{
			if (0 > num3)
			{
				this.ContentRectTransform.anchoredPosition = new Vector2(this.ContentRectTransform.anchoredPosition.x - num5, this.ContentRectTransform.anchoredPosition.y);
				return;
			}
			this.ContentRectTransform.anchoredPosition = new Vector2(this.ContentRectTransform.anchoredPosition.x + num5, this.ContentRectTransform.anchoredPosition.y);
			return;
		}
	}

	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	private int itemSize;

	[SerializeField]
	private CustomScrollRect.Direction direction;

	[SerializeField]
	private int focusOffset;

	[SerializeField]
	private CustomScrollRect.Margin ScrollMargin;

	[SerializeField]
	private bool isAutoSnap;

	[SerializeField]
	private float lowLimitVelocity;

	[SerializeField]
	private float snapSpeed;

	[SerializeField]
	private bool isLoop;

	[SerializeField]
	private float autoMoveTime;

	[SerializeField]
	private bool targetMoveFront;

	[HideInInspector]
	public Action<int, GameObject> onStartItem;

	[HideInInspector]
	public Action<int, GameObject> onUpdateItem;

	[HideInInspector]
	public Action<int, GameObject> onChangeFocusItem;

	private GameObject startMargin;

	private GameObject endMargin;

	private bool requestReflesh;

	protected enum Direction
	{
		Vertical,
		Horizontal
	}

	[Serializable]
	protected class Margin
	{
		public float total
		{
			get
			{
				return this.start + this.end;
			}
		}

		public float start;

		public float end;
	}
}
