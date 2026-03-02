using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200013E RID: 318
[RequireComponent(typeof(ScrollRect))]
public class CustomScrollRect : MonoBehaviour
{
	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06001166 RID: 4454 RVA: 0x000D392A File Offset: 0x000D1B2A
	// (set) Token: 0x06001167 RID: 4455 RVA: 0x000D3932 File Offset: 0x000D1B32
	[HideInInspector]
	public int ItemNum { get; private set; }

	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06001168 RID: 4456 RVA: 0x000D393B File Offset: 0x000D1B3B
	// (set) Token: 0x06001169 RID: 4457 RVA: 0x000D3943 File Offset: 0x000D1B43
	[HideInInspector]
	public int PrefabNum { get; private set; }

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x0600116A RID: 4458 RVA: 0x000D394C File Offset: 0x000D1B4C
	[HideInInspector]
	public bool IsMoving
	{
		get
		{
			return this.IsAutoMove || 0f != this.ScrollRect.velocity.x || 0f != this.ScrollRect.velocity.y;
		}
	}

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x0600116B RID: 4459 RVA: 0x000D3989 File Offset: 0x000D1B89
	// (set) Token: 0x0600116C RID: 4460 RVA: 0x000D3991 File Offset: 0x000D1B91
	private ScrollRect ScrollRect { get; set; }

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x0600116D RID: 4461 RVA: 0x000D399A File Offset: 0x000D1B9A
	// (set) Token: 0x0600116E RID: 4462 RVA: 0x000D39A2 File Offset: 0x000D1BA2
	private RectTransform ContentRectTransform { get; set; }

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x0600116F RID: 4463 RVA: 0x000D39AB File Offset: 0x000D1BAB
	// (set) Token: 0x06001170 RID: 4464 RVA: 0x000D39B3 File Offset: 0x000D1BB3
	private List<GameObject> ItemObjectList { get; set; }

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x06001171 RID: 4465 RVA: 0x000D39BC File Offset: 0x000D1BBC
	// (set) Token: 0x06001172 RID: 4466 RVA: 0x000D39C4 File Offset: 0x000D1BC4
	private bool IsInit { get; set; }

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x06001173 RID: 4467 RVA: 0x000D39CD File Offset: 0x000D1BCD
	// (set) Token: 0x06001174 RID: 4468 RVA: 0x000D39D5 File Offset: 0x000D1BD5
	private int TargetIndex { get; set; }

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x06001175 RID: 4469 RVA: 0x000D39DE File Offset: 0x000D1BDE
	// (set) Token: 0x06001176 RID: 4470 RVA: 0x000D39E6 File Offset: 0x000D1BE6
	private int NowHeadIndex { get; set; }

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x06001177 RID: 4471 RVA: 0x000D39EF File Offset: 0x000D1BEF
	// (set) Token: 0x06001178 RID: 4472 RVA: 0x000D39F7 File Offset: 0x000D1BF7
	private int NowFocusedIndex { get; set; }

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x06001179 RID: 4473 RVA: 0x000D3A00 File Offset: 0x000D1C00
	// (set) Token: 0x0600117A RID: 4474 RVA: 0x000D3A08 File Offset: 0x000D1C08
	private bool IsAutoMove { get; set; }

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x0600117B RID: 4475 RVA: 0x000D3A11 File Offset: 0x000D1C11
	// (set) Token: 0x0600117C RID: 4476 RVA: 0x000D3A19 File Offset: 0x000D1C19
	private int AutoMoveTargetIndex { get; set; }

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x0600117D RID: 4477 RVA: 0x000D3A22 File Offset: 0x000D1C22
	// (set) Token: 0x0600117E RID: 4478 RVA: 0x000D3A2A File Offset: 0x000D1C2A
	private float AutoMoveStart { get; set; }

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x0600117F RID: 4479 RVA: 0x000D3A33 File Offset: 0x000D1C33
	// (set) Token: 0x06001180 RID: 4480 RVA: 0x000D3A3B File Offset: 0x000D1C3B
	private float AutoMoveEnd { get; set; }

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x06001181 RID: 4481 RVA: 0x000D3A44 File Offset: 0x000D1C44
	// (set) Token: 0x06001182 RID: 4482 RVA: 0x000D3A4C File Offset: 0x000D1C4C
	private float AutoMoveElapsedTime { get; set; }

	// Token: 0x06001183 RID: 4483 RVA: 0x000D3A58 File Offset: 0x000D1C58
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

	// Token: 0x06001184 RID: 4484 RVA: 0x000D3CC6 File Offset: 0x000D1EC6
	public void Refresh()
	{
		this.requestReflesh = true;
	}

	// Token: 0x06001185 RID: 4485 RVA: 0x000D3CCF File Offset: 0x000D1ECF
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

	// Token: 0x06001186 RID: 4486 RVA: 0x000D3D08 File Offset: 0x000D1F08
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

	// Token: 0x06001187 RID: 4487 RVA: 0x000D3E5C File Offset: 0x000D205C
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

	// Token: 0x06001188 RID: 4488 RVA: 0x000D3E98 File Offset: 0x000D2098
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

	// Token: 0x06001189 RID: 4489 RVA: 0x000D3FC0 File Offset: 0x000D21C0
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

	// Token: 0x0600118A RID: 4490 RVA: 0x000D44AC File Offset: 0x000D26AC
	private void DestroyScrollObjects()
	{
		foreach (object obj in this.ContentRectTransform)
		{
			Object.Destroy(((RectTransform)obj).gameObject);
		}
		this.ItemNum = 0;
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x000D4510 File Offset: 0x000D2710
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

	// Token: 0x0600118C RID: 4492 RVA: 0x000D4748 File Offset: 0x000D2948
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

	// Token: 0x0600118D RID: 4493 RVA: 0x000D484C File Offset: 0x000D2A4C
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

	// Token: 0x04000EA6 RID: 3750
	[SerializeField]
	private GameObject prefab;

	// Token: 0x04000EA7 RID: 3751
	[SerializeField]
	private int itemSize;

	// Token: 0x04000EA8 RID: 3752
	[SerializeField]
	private CustomScrollRect.Direction direction;

	// Token: 0x04000EA9 RID: 3753
	[SerializeField]
	private int focusOffset;

	// Token: 0x04000EAA RID: 3754
	[SerializeField]
	private CustomScrollRect.Margin ScrollMargin;

	// Token: 0x04000EAB RID: 3755
	[SerializeField]
	private bool isAutoSnap;

	// Token: 0x04000EAC RID: 3756
	[SerializeField]
	private float lowLimitVelocity;

	// Token: 0x04000EAD RID: 3757
	[SerializeField]
	private float snapSpeed;

	// Token: 0x04000EAE RID: 3758
	[SerializeField]
	private bool isLoop;

	// Token: 0x04000EAF RID: 3759
	[SerializeField]
	private float autoMoveTime;

	// Token: 0x04000EB0 RID: 3760
	[SerializeField]
	private bool targetMoveFront;

	// Token: 0x04000EB1 RID: 3761
	[HideInInspector]
	public Action<int, GameObject> onStartItem;

	// Token: 0x04000EB2 RID: 3762
	[HideInInspector]
	public Action<int, GameObject> onUpdateItem;

	// Token: 0x04000EB3 RID: 3763
	[HideInInspector]
	public Action<int, GameObject> onChangeFocusItem;

	// Token: 0x04000EC2 RID: 3778
	private GameObject startMargin;

	// Token: 0x04000EC3 RID: 3779
	private GameObject endMargin;

	// Token: 0x04000EC4 RID: 3780
	private bool requestReflesh;

	// Token: 0x02000A84 RID: 2692
	protected enum Direction
	{
		// Token: 0x040042FA RID: 17146
		Vertical,
		// Token: 0x040042FB RID: 17147
		Horizontal
	}

	// Token: 0x02000A85 RID: 2693
	[Serializable]
	protected class Margin
	{
		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06003F8D RID: 16269 RVA: 0x001EF9B4 File Offset: 0x001EDBB4
		public float total
		{
			get
			{
				return this.start + this.end;
			}
		}

		// Token: 0x040042FC RID: 17148
		public float start;

		// Token: 0x040042FD RID: 17149
		public float end;
	}
}
