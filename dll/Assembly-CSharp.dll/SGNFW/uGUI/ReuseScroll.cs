using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	// Token: 0x02000231 RID: 561
	[RequireComponent(typeof(ScrollRect))]
	public class ReuseScroll : MonoBehaviour
	{
		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06002356 RID: 9046 RVA: 0x001977A4 File Offset: 0x001959A4
		// (set) Token: 0x06002355 RID: 9045 RVA: 0x00197758 File Offset: 0x00195958
		public float Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
				this.scrollRect = base.GetComponent<ScrollRect>();
				this.scrollRect.scrollSensitivity = (this.scrollRect.vertical ? this.size : (this.size * -1f));
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06002357 RID: 9047 RVA: 0x001977AC File Offset: 0x001959AC
		public int ItemNum
		{
			get
			{
				return this.itemNum;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06002358 RID: 9048 RVA: 0x001977B4 File Offset: 0x001959B4
		public RectTransform ContentRoot
		{
			get
			{
				return this.trsRoot;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06002359 RID: 9049 RVA: 0x001977BC File Offset: 0x001959BC
		public int CurrentItemIdx
		{
			get
			{
				return this.currentItem;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600235B RID: 9051 RVA: 0x001977CD File Offset: 0x001959CD
		// (set) Token: 0x0600235A RID: 9050 RVA: 0x001977C4 File Offset: 0x001959C4
		public int ReuseItemNum
		{
			get
			{
				return this.reuseItemNum;
			}
			set
			{
				this.reuseItemNum = value;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x001977D5 File Offset: 0x001959D5
		public bool IsScroll
		{
			get
			{
				return this.isScroll;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600235D RID: 9053 RVA: 0x001977DD File Offset: 0x001959DD
		public bool IsLoop
		{
			get
			{
				return this.loop;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x0600235E RID: 9054 RVA: 0x001977E5 File Offset: 0x001959E5
		public ScrollRect RefScrollRect
		{
			get
			{
				return this.scrollRect;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x0600235F RID: 9055 RVA: 0x001977ED File Offset: 0x001959ED
		// (set) Token: 0x06002360 RID: 9056 RVA: 0x00197828 File Offset: 0x00195A28
		public float AnchoredPosition
		{
			get
			{
				if (!this.scrollRect.vertical)
				{
					return this.scrollRect.content.anchoredPosition.x;
				}
				return -this.scrollRect.content.anchoredPosition.y;
			}
			set
			{
				Vector2 anchoredPosition = this.scrollRect.content.anchoredPosition;
				if (this.scrollRect.vertical)
				{
					anchoredPosition.y = -value;
				}
				else
				{
					anchoredPosition.x = value;
				}
				this.scrollRect.content.anchoredPosition = anchoredPosition;
			}
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x00197878 File Offset: 0x00195A78
		public void ScrollReset()
		{
			this.trsRoot.localPosition = Vector3.zero;
			this.AnchoredPosition = 0f;
			this.currentItem = 0;
			this.diffPreFramePosition = 0f;
			this.scrollRect.content.anchoredPosition = Vector3.zero;
			this.Focus(0);
			for (int i = 0; i < this.itemList.Count; i++)
			{
				RectTransform rectTransform = this.itemList[i];
				if (this.scrollRect.vertical)
				{
					rectTransform.anchoredPosition = new Vector2(0f, this.size * (float)i);
				}
				else
				{
					rectTransform.anchoredPosition = new Vector2(this.size * (float)i, 0f);
				}
			}
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x00197938 File Offset: 0x00195B38
		public void Setup(int itemNum, int focusIndex = 0)
		{
			GameObject[] array = new GameObject[this.reuseItemNum];
			for (int i = 0; i < this.reuseItemNum; i++)
			{
				array[i] = Manager.Create(this.prefab, this.trsRoot, null, null, Layer.UI);
			}
			this.Setup(itemNum, array, focusIndex);
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x00197984 File Offset: 0x00195B84
		public void Setup(int itemNum, GameObject go, int focusIndex = 0)
		{
			GameObject[] array = new GameObject[this.reuseItemNum];
			for (int i = 0; i < this.reuseItemNum; i++)
			{
				array[i] = Manager.Create(go, this.trsRoot, null, null, Layer.UI);
			}
			this.Setup(itemNum, array, focusIndex);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x001979CC File Offset: 0x00195BCC
		public void Setup(int itemNum, GameObject[] goArray, int focusIndex = 0)
		{
			if (goArray == null || goArray.Length == 0)
			{
				return;
			}
			if (null == this.scrollRect)
			{
				return;
			}
			this.itemList.Clear();
			this.itemNum = itemNum;
			this.reuseItemNum = goArray.Length;
			int num = 0;
			foreach (GameObject gameObject in goArray)
			{
				gameObject.name = num.ToString();
				gameObject.SetActive(this.loop || num < itemNum);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.SetParent(this.trsRoot);
				component.anchoredPosition = (this.scrollRect.vertical ? new Vector2(0f, -(this.size * (float)num) - this.padding.start) : new Vector2(this.size * (float)num + this.padding.start, 0f));
				if (this.scrollRect.vertical)
				{
					component.anchorMin = new Vector2(0.5f, 1f);
					component.anchorMax = new Vector2(0.5f, 1f);
					component.pivot = new Vector2(0.5f, 1f);
				}
				else
				{
					component.anchorMin = new Vector2(0f, 0.5f);
					component.anchorMax = new Vector2(0f, 0.5f);
					component.pivot = new Vector2(0f, 0.5f);
				}
				if (this.onStartItem != null)
				{
					this.onStartItem(num, component.gameObject);
				}
				this.itemList.Add(component);
				num++;
			}
			this.Setup();
			this.Focus(focusIndex);
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x00197B74 File Offset: 0x00195D74
		public void Resize(int itemNum, int focusIndex = 0)
		{
			this.itemNum = itemNum;
			for (int i = 0; i < this.itemList.Count; i++)
			{
				if (int.Parse(this.itemList[i].gameObject.name) >= itemNum)
				{
					this.itemList[i].gameObject.SetActive(this.loop);
				}
			}
			this.Setup();
			this.Focus(focusIndex);
			this.Refresh();
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x00197BEC File Offset: 0x00195DEC
		public void ResizeFocesNoMove(int itemNum)
		{
			this.itemNum = itemNum;
			for (int i = 0; i < this.itemList.Count; i++)
			{
				if (int.Parse(this.itemList[i].gameObject.name) >= itemNum)
				{
					this.itemList[i].gameObject.SetActive(this.loop);
				}
			}
			this.Setup();
			this.Refresh();
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x00197C5C File Offset: 0x00195E5C
		public void Refresh()
		{
			this.Update();
			if (this.onUpdateItem == null)
			{
				return;
			}
			for (int i = 0; i < this.reuseItemNum; i++)
			{
				int num = i + this.currentItem;
				if (!this.loop && (num < 0 || num >= this.itemNum))
				{
					this.itemList[i].gameObject.SetActive(false);
				}
				else
				{
					RectTransform rectTransform = this.itemList[i];
					rectTransform.gameObject.SetActive(true);
					this.onUpdateItem(num, rectTransform.gameObject);
				}
			}
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x00197CEC File Offset: 0x00195EEC
		public void Clear()
		{
			if (this.itemList == null)
			{
				return;
			}
			for (int i = 0; i < this.itemList.Count; i++)
			{
				Object.Destroy(this.itemList[i].gameObject);
			}
			this.itemList.Clear();
			this.scrollRect.content.sizeDelta = Vector2.zero;
			this.trsRoot.localPosition = Vector3.zero;
			this.ScrollReset();
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x00197D64 File Offset: 0x00195F64
		private void UpdateItem(int idx, GameObject go)
		{
			if (this.loop)
			{
				go.SetActive(true);
				if (this.onUpdateItem != null)
				{
					this.onUpdateItem(idx, go);
					return;
				}
			}
			else
			{
				if (idx < 0 || idx >= this.itemNum)
				{
					go.SetActive(false);
					return;
				}
				go.SetActive(true);
				if (this.onUpdateItem != null)
				{
					this.onUpdateItem(idx, go);
				}
			}
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x00197DC8 File Offset: 0x00195FC8
		private void Setup()
		{
			Vector2 sizeDelta = this.scrollRect.content.sizeDelta;
			Vector3 localPosition = this.trsRoot.localPosition;
			if (this.scrollRect.vertical)
			{
				Vector2 vector = this.scrollRect.content.anchorMin;
				vector.y = 1f;
				this.scrollRect.content.anchorMin = vector;
				vector = this.scrollRect.content.anchorMax;
				vector.y = 1f;
				this.scrollRect.content.anchorMax = vector;
				sizeDelta.y = this.size * (float)this.itemNum + this.padding.total;
			}
			else
			{
				Vector2 vector2 = this.scrollRect.content.anchorMin;
				vector2.x = 0f;
				this.scrollRect.content.anchorMin = vector2;
				vector2 = this.scrollRect.content.anchorMax;
				vector2.x = 0f;
				this.scrollRect.content.anchorMax = vector2;
				sizeDelta.x = this.size * (float)this.itemNum + this.padding.total;
			}
			this.scrollRect.content.sizeDelta = sizeDelta;
			this.trsRoot.localPosition = localPosition;
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x00197F18 File Offset: 0x00196118
		public void ForceSetScrollRect(float sizeDelta)
		{
			Vector2 sizeDelta2 = this.scrollRect.content.sizeDelta;
			if (this.scrollRect.vertical)
			{
				sizeDelta2.y += sizeDelta;
			}
			else
			{
				sizeDelta2.x += sizeDelta;
			}
			this.scrollRect.content.sizeDelta = sizeDelta2;
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x00197F70 File Offset: 0x00196170
		private void Focus(int focusIndex)
		{
			int num = this.itemNum - 1;
			if (this.scrollRect.vertical)
			{
				float num2 = ((num > 0) ? ((float)(num - focusIndex) / (float)num) : 0f);
				this.scrollRect.verticalNormalizedPosition = num2;
				this.scrollRect.onValueChanged.Invoke(this.scrollRect.velocity);
				return;
			}
			float num3 = ((num > 0) ? ((float)focusIndex / (float)num) : 0f);
			this.scrollRect.horizontalNormalizedPosition = num3;
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x00197FEB File Offset: 0x001961EB
		public void ForceFocus(int focusIndex)
		{
			this.Focus(focusIndex);
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x00197FF4 File Offset: 0x001961F4
		public int CalcCurrentFocusIndex()
		{
			int num = this.itemNum - 1;
			int i;
			if (this.scrollRect.vertical)
			{
				i = num - Mathf.RoundToInt((float)num * this.scrollRect.verticalNormalizedPosition);
			}
			else
			{
				i = num - Mathf.RoundToInt((float)num * this.scrollRect.horizontalNormalizedPosition);
			}
			if (this.itemNum > 0)
			{
				while (i < 0)
				{
					i += this.itemNum;
				}
				i %= this.itemNum;
			}
			return i;
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x0019806C File Offset: 0x0019626C
		public void NextFocusIndex(float moveTime)
		{
			int num = this.itemNum - 1;
			int num2 = num - this.CalcCurrentFocusIndex();
			this.prevScrollIndex = num2;
			num2++;
			if (num2 > num && !this.loop)
			{
				num2 = 0;
			}
			this.autoScrollIndex = num2;
			this.autoScroll = true;
			this.autoTimer = 0f;
			this.autoTime = moveTime;
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x001980C4 File Offset: 0x001962C4
		public void InitForce()
		{
			this.Awake();
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x001980CC File Offset: 0x001962CC
		private void Awake()
		{
			if (this.isInit)
			{
				return;
			}
			this.isInit = true;
			this.scrollRect = base.GetComponent<ScrollRect>();
			this.scrollRect.horizontal = this.direction == ReuseScroll.Direction.Horizontal;
			this.scrollRect.vertical = this.direction == ReuseScroll.Direction.Vertical;
			this.scrollRect.scrollSensitivity = (this.scrollRect.vertical ? this.size : (this.size * -1f));
			if (this.loop)
			{
				this.scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
			}
			else
			{
				this.scrollRect.movementType = ScrollRect.MovementType.Elastic;
			}
			if (this.scrollRect.verticalScrollbar != null)
			{
				this.scrollRect.onValueChanged.AddListener(delegate(Vector2 value)
				{
					if (this.scrollRect.verticalScrollbar.size < 0.05f)
					{
						this.scrollRect.verticalScrollbar.size = 0.05f;
					}
				});
			}
			if (this.scrollRect.content == null)
			{
				Verbose<Verbose>.LogError("scrollRect.content is null", null);
				return;
			}
			this.autoScroll = false;
			this.isScroll = true;
			this.autoTimer = 0f;
			this.autoTime = 0f;
			this.trsRoot = this.scrollRect.content.GetComponent<RectTransform>();
			this.SetupScrollHandle();
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x001981FC File Offset: 0x001963FC
		private void Update()
		{
			FixedScrollRect fixedScrollRect = this.scrollRect as FixedScrollRect;
			if (this.autoScroll)
			{
				this.autoTimer += TimeManager.DeltaTime;
				if (this.autoTime <= 0f)
				{
					return;
				}
				float num = this.autoTimer / this.autoTime;
				if (num >= 1f)
				{
					num = 1f;
					this.autoScroll = false;
					this.isScroll = false;
				}
				int num2 = this.itemNum - 1;
				if (this.scrollRect.vertical)
				{
					float num3 = (float)(num2 - this.autoScrollIndex) / (float)num2;
					num3 += this.size * num;
					this.scrollRect.verticalNormalizedPosition = num3;
				}
				else
				{
					float num4 = (float)(this.autoScrollIndex - this.prevScrollIndex) / (float)num2;
					num4 *= num;
					float num5 = (float)this.prevScrollIndex / (float)num2;
					this.scrollRect.horizontalNormalizedPosition = num5 + num4;
				}
			}
			else if (fixedScrollRect != null)
			{
				if (fixedScrollRect.IsDrag)
				{
					this.isScroll = true;
				}
				else
				{
					this.isScroll = false;
				}
			}
			float num6 = (this.diffPreFramePosition - this.AnchoredPosition) / this.size - 1f;
			int num7 = 0;
			while (this.AnchoredPosition - this.diffPreFramePosition < -this.size * 2f)
			{
				num7++;
				this.diffPreFramePosition -= this.size;
				if (this.itemList.Count == 0)
				{
					break;
				}
				RectTransform rectTransform = this.itemList[0];
				this.itemList.RemoveAt(0);
				this.itemList.Add(rectTransform);
				float num8 = this.size * (float)this.reuseItemNum + this.size * (float)this.currentItem;
				rectTransform.anchoredPosition = (this.scrollRect.vertical ? new Vector2(0f, -num8 - this.padding.start) : new Vector2(num8 + this.padding.start, 0f));
				if (num6 <= (float)(this.reuseItemNum * 50) || this.reuseItemNum * 10 >= num7 || (float)num7 >= num6 - (float)(this.reuseItemNum * 10))
				{
					this.UpdateItem(this.currentItem + this.reuseItemNum, rectTransform.gameObject);
				}
				this.currentItem++;
			}
			num6 = (this.AnchoredPosition - this.diffPreFramePosition) / this.size;
			num7 = 0;
			while (this.AnchoredPosition - this.diffPreFramePosition > 0f)
			{
				num7++;
				this.diffPreFramePosition += this.size;
				if (this.itemList.Count < this.reuseItemNum)
				{
					break;
				}
				int num9 = this.reuseItemNum - 1;
				RectTransform rectTransform2 = this.itemList[num9];
				this.itemList.RemoveAt(num9);
				this.itemList.Insert(0, rectTransform2);
				this.currentItem--;
				float num10 = this.size * (float)this.currentItem;
				rectTransform2.anchoredPosition = (this.scrollRect.vertical ? new Vector2(0f, -num10 - this.padding.start) : new Vector2(num10 + this.padding.start, 0f));
				if (num6 <= (float)(this.reuseItemNum * 50) || this.reuseItemNum * 10 >= num7 || (float)num7 >= num6 - (float)(this.reuseItemNum * 10))
				{
					this.UpdateItem(this.currentItem, rectTransform2.gameObject);
				}
			}
			if (this.itemList != null && 0 < this.itemList.Count)
			{
				int num11 = (this.scrollRect.vertical ? (((int)this.itemList[0].anchoredPosition.y + (int)this.padding.start) * -1) : ((int)this.itemList[0].anchoredPosition.x + (int)this.padding.start)) / (int)this.size;
				if (this.nowIndex != num11)
				{
					this.nowIndex = num11;
					Action<int> action = this.onChangeIndex;
					if (action == null)
					{
						return;
					}
					action(this.nowIndex);
				}
			}
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0019862C File Offset: 0x0019682C
		public void SetupScrollHandle()
		{
			if (this.scrollRect.verticalScrollbar != null)
			{
				RectTransform component = this.scrollRect.verticalScrollbar.transform.GetChild(0).Find("Handle").GetComponent<RectTransform>();
				if (component.sizeDelta.x == ScrollParamDefine.BaseHandleRange)
				{
					component.sizeDelta = new Vector2(component.sizeDelta.x * (float)ScrollParamDefine.HandleAdditionalFactor, component.sizeDelta.y);
				}
			}
			if (this.scrollRect.horizontalScrollbar != null)
			{
				RectTransform component2 = this.scrollRect.horizontalScrollbar.transform.GetChild(0).Find("Handle").GetComponent<RectTransform>();
				if (component2.sizeDelta.y == ScrollParamDefine.BaseHandleRange)
				{
					component2.sizeDelta = new Vector2(component2.sizeDelta.x, component2.sizeDelta.y * (float)ScrollParamDefine.HandleAdditionalFactor);
				}
			}
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x00198720 File Offset: 0x00196920
		public void SetParameterByPgui(GameObject targetPrefab, float itemSize, float paddingStart, float paddingEnd, bool isVertical)
		{
			this.prefab = targetPrefab;
			this.size = itemSize;
			this.padding.start = paddingStart;
			this.padding.end = paddingEnd;
			this.direction = (isVertical ? ReuseScroll.Direction.Vertical : ReuseScroll.Direction.Horizontal);
			this.scrollRect = base.GetComponent<ScrollRect>();
			this.scrollRect.scrollSensitivity = (this.scrollRect.vertical ? itemSize : (itemSize * -1f));
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x00198790 File Offset: 0x00196990
		public void SetPrefab(GameObject _prefab)
		{
			this.prefab = _prefab;
		}

		// Token: 0x04001AB2 RID: 6834
		public Action<int, GameObject> onStartItem;

		// Token: 0x04001AB3 RID: 6835
		public Action<int, GameObject> onUpdateItem;

		// Token: 0x04001AB4 RID: 6836
		public Action<int> onChangeIndex;

		// Token: 0x04001AB5 RID: 6837
		[SerializeField]
		protected GameObject prefab;

		// Token: 0x04001AB6 RID: 6838
		[SerializeField]
		protected ReuseScroll.Direction direction;

		// Token: 0x04001AB7 RID: 6839
		[SerializeField]
		protected int reuseItemNum;

		// Token: 0x04001AB8 RID: 6840
		[SerializeField]
		protected float size;

		// Token: 0x04001AB9 RID: 6841
		[SerializeField]
		protected ReuseScroll.Padding padding;

		// Token: 0x04001ABA RID: 6842
		[SerializeField]
		protected bool loop;

		// Token: 0x04001ABB RID: 6843
		protected ScrollRect scrollRect;

		// Token: 0x04001ABC RID: 6844
		protected RectTransform trsRoot;

		// Token: 0x04001ABD RID: 6845
		protected List<RectTransform> itemList = new List<RectTransform>();

		// Token: 0x04001ABE RID: 6846
		protected float diffPreFramePosition;

		// Token: 0x04001ABF RID: 6847
		protected int currentItem;

		// Token: 0x04001AC0 RID: 6848
		protected int itemNum;

		// Token: 0x04001AC1 RID: 6849
		protected bool autoScroll;

		// Token: 0x04001AC2 RID: 6850
		protected int autoScrollIndex;

		// Token: 0x04001AC3 RID: 6851
		protected int prevScrollIndex;

		// Token: 0x04001AC4 RID: 6852
		protected float autoTimer;

		// Token: 0x04001AC5 RID: 6853
		protected float autoTime;

		// Token: 0x04001AC6 RID: 6854
		protected bool isScroll;

		// Token: 0x04001AC7 RID: 6855
		private int nowIndex = -1;

		// Token: 0x04001AC8 RID: 6856
		private bool isInit;

		// Token: 0x02001068 RID: 4200
		protected enum Direction
		{
			// Token: 0x04005BB9 RID: 23481
			Vertical,
			// Token: 0x04005BBA RID: 23482
			Horizontal
		}

		// Token: 0x02001069 RID: 4201
		[Serializable]
		protected class Padding
		{
			// Token: 0x17000BED RID: 3053
			// (get) Token: 0x060052E6 RID: 21222 RVA: 0x002496FE File Offset: 0x002478FE
			public float total
			{
				get
				{
					return this.start + this.end;
				}
			}

			// Token: 0x04005BBB RID: 23483
			public float start;

			// Token: 0x04005BBC RID: 23484
			public float end;
		}
	}
}
