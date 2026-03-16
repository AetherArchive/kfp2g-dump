using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	public class FixedScrollRect : ScrollRect
	{
		public bool IsDrag { get; private set; }

		private float Velocity
		{
			get
			{
				if (!base.vertical)
				{
					return base.velocity.x;
				}
				return -base.velocity.y;
			}
		}

		private float AnchoredPosition
		{
			get
			{
				if (!base.vertical)
				{
					return base.content.anchoredPosition.x;
				}
				return -base.content.anchoredPosition.y;
			}
			set
			{
				if (base.vertical)
				{
					base.content.anchoredPosition = new Vector2(0f, -value);
					return;
				}
				base.content.anchoredPosition = new Vector2(value, 0f);
			}
		}

		public override void OnBeginDrag(PointerEventData eventData)
		{
			base.OnBeginDrag(eventData);
			this.IsDrag = true;
			this.stopNow = false;
			this.wasPageIndexDecided = false;
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			base.OnEndDrag(eventData);
			this.IsDrag = false;
		}

		protected override void Awake()
		{
			base.Awake();
			this.reuseScroll = base.GetComponent<ReuseScroll>();
		}

		public void SetPage(int index)
		{
			index *= -1;
			this.AnchoredPosition = this.reuseScroll.Size * (float)index;
		}

		protected void Update()
		{
			if (this.IsDrag || Mathf.Abs(this.Velocity) > 200f)
			{
				return;
			}
			float num = this.AnchoredPosition % this.reuseScroll.Size;
			num += ((num < 0f) ? this.reuseScroll.Size : 0f);
			float num2;
			bool flag;
			if (num > this.reuseScroll.Size / 2f)
			{
				num2 = this.reuseScroll.Size - num;
				flag = false;
			}
			else
			{
				num2 = num;
				flag = true;
			}
			float num3 = num2 * Time.deltaTime * 10f;
			float height = this.reuseScroll.ContentRoot.parent.transform.GetComponent<RectTransform>().rect.height;
			if (!this.reuseScroll.IsLoop)
			{
				int num4 = (int)height / (int)this.reuseScroll.Size;
				int num5 = this.reuseScroll.ItemNum - num4;
				float num6 = this.reuseScroll.Size * (float)num5;
				if (0f > num6 + this.AnchoredPosition)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				this.AnchoredPosition += num3;
			}
			else
			{
				this.AnchoredPosition -= num3;
			}
			float num7 = Mathf.Abs(num);
			if ((num7 < 1f || this.reuseScroll.Size - 1f < num7) && !this.stopNow)
			{
				this.stopNow = true;
				if (this.onScrollStopped != null)
				{
					int num8 = Mathf.FloorToInt(this.AnchoredPosition / this.reuseScroll.Size + 0.5f);
					this.onScrollStopped(-num8);
				}
			}
		}

		private const float POWER = 10f;

		public Action<int> onScrollStopped;

		public Action<int> onPageIndexDecided;

		private ReuseScroll reuseScroll;

		private bool stopNow;

		private bool wasPageIndexDecided;
	}
}
