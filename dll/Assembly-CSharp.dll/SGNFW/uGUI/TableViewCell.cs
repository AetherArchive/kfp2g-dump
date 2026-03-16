using System;
using UnityEngine;

namespace SGNFW.uGUI
{
	public class TableViewCell<T> : MonoBehaviour
	{
		public RectTransform CachedRectTransform
		{
			get
			{
				if (this.cachedRectTransform == null)
				{
					this.cachedRectTransform = base.GetComponent<RectTransform>();
				}
				return this.cachedRectTransform;
			}
		}

		public int DataIndex { get; set; }

		public float Height
		{
			get
			{
				return this.CachedRectTransform.sizeDelta.y;
			}
			set
			{
				Vector2 sizeDelta = this.CachedRectTransform.sizeDelta;
				sizeDelta.y = value;
				this.CachedRectTransform.sizeDelta = sizeDelta;
			}
		}

		public float Width
		{
			get
			{
				return this.CachedRectTransform.sizeDelta.x;
			}
			set
			{
				Vector2 sizeDelta = this.CachedRectTransform.sizeDelta;
				sizeDelta.x = value;
				this.CachedRectTransform.sizeDelta = sizeDelta;
			}
		}

		public float Top
		{
			get
			{
				Vector3[] array = new Vector3[4];
				this.CachedRectTransform.GetLocalCorners(array);
				return this.CachedRectTransform.anchoredPosition.y + array[1].y;
			}
			set
			{
				Vector3[] array = new Vector3[4];
				this.CachedRectTransform.GetLocalCorners(array);
				Vector2 anchoredPosition = this.CachedRectTransform.anchoredPosition;
				anchoredPosition.y = value - array[1].y;
				this.CachedRectTransform.anchoredPosition = anchoredPosition;
			}
		}

		public float Bottom
		{
			get
			{
				Vector3[] array = new Vector3[4];
				this.CachedRectTransform.GetLocalCorners(array);
				return this.CachedRectTransform.anchoredPosition.y + array[3].y;
			}
			set
			{
				Vector3[] array = new Vector3[4];
				this.CachedRectTransform.GetLocalCorners(array);
				Vector2 anchoredPosition = this.CachedRectTransform.anchoredPosition;
				anchoredPosition.y = value - array[3].y;
				this.CachedRectTransform.anchoredPosition = anchoredPosition;
			}
		}

		public float Left
		{
			get
			{
				Vector3[] array = new Vector3[4];
				this.CachedRectTransform.GetLocalCorners(array);
				return this.CachedRectTransform.anchoredPosition.x + array[0].x;
			}
			set
			{
				Vector3[] array = new Vector3[4];
				this.CachedRectTransform.GetLocalCorners(array);
				Vector2 anchoredPosition = this.CachedRectTransform.anchoredPosition;
				anchoredPosition.x = value - array[0].x;
				this.CachedRectTransform.anchoredPosition = anchoredPosition;
			}
		}

		public float Right
		{
			get
			{
				Vector3[] array = new Vector3[4];
				this.CachedRectTransform.GetLocalCorners(array);
				return this.CachedRectTransform.anchoredPosition.x + array[2].x;
			}
			set
			{
				Vector3[] array = new Vector3[4];
				this.CachedRectTransform.GetLocalCorners(array);
				Vector2 anchoredPosition = this.CachedRectTransform.anchoredPosition;
				anchoredPosition.x = value - array[2].x;
				this.CachedRectTransform.anchoredPosition = anchoredPosition;
			}
		}

		public virtual void UpdateContent(T itemData, int index)
		{
		}

		private RectTransform cachedRectTransform;
	}
}
