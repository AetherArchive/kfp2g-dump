using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	[RequireComponent(typeof(ScrollRect))]
	public class ReuseScroll : MonoBehaviour
	{
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

		public int ItemNum
		{
			get
			{
				return this.itemNum;
			}
		}

		public RectTransform ContentRoot
		{
			get
			{
				return this.trsRoot;
			}
		}

		public int CurrentItemIdx
		{
			get
			{
				return this.currentItem;
			}
		}

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

		public bool IsScroll
		{
			get
			{
				return this.isScroll;
			}
		}

		public bool IsLoop
		{
			get
			{
				return this.loop;
			}
		}

		public ScrollRect RefScrollRect
		{
			get
			{
				return this.scrollRect;
			}
		}

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

		public void Setup(int itemNum, int focusIndex = 0)
		{
			GameObject[] array = new GameObject[this.reuseItemNum];
			for (int i = 0; i < this.reuseItemNum; i++)
			{
				array[i] = Manager.Create(this.prefab, this.trsRoot, null, null, Layer.UI);
			}
			this.Setup(itemNum, array, focusIndex);
		}

		public void Setup(int itemNum, GameObject go, int focusIndex = 0)
		{
			GameObject[] array = new GameObject[this.reuseItemNum];
			for (int i = 0; i < this.reuseItemNum; i++)
			{
				array[i] = Manager.Create(go, this.trsRoot, null, null, Layer.UI);
			}
			this.Setup(itemNum, array, focusIndex);
		}

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

		public void ForceFocus(int focusIndex)
		{
			this.Focus(focusIndex);
		}

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

		public void InitForce()
		{
			this.Awake();
		}

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

		public void SetPrefab(GameObject _prefab)
		{
			this.prefab = _prefab;
		}

		public Action<int, GameObject> onStartItem;

		public Action<int, GameObject> onUpdateItem;

		public Action<int> onChangeIndex;

		[SerializeField]
		protected GameObject prefab;

		[SerializeField]
		protected ReuseScroll.Direction direction;

		[SerializeField]
		protected int reuseItemNum;

		[SerializeField]
		protected float size;

		[SerializeField]
		protected ReuseScroll.Padding padding;

		[SerializeField]
		protected bool loop;

		protected ScrollRect scrollRect;

		protected RectTransform trsRoot;

		protected List<RectTransform> itemList = new List<RectTransform>();

		protected float diffPreFramePosition;

		protected int currentItem;

		protected int itemNum;

		protected bool autoScroll;

		protected int autoScrollIndex;

		protected int prevScrollIndex;

		protected float autoTimer;

		protected float autoTime;

		protected bool isScroll;

		private int nowIndex = -1;

		private bool isInit;

		protected enum Direction
		{
			Vertical,
			Horizontal
		}

		[Serializable]
		protected class Padding
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
}
