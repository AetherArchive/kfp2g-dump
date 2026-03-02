using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SGNFW.uGUI
{
	// Token: 0x02000228 RID: 552
	public class FixedScrollRect : ScrollRect
	{
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x0600231F RID: 8991 RVA: 0x00196266 File Offset: 0x00194466
		// (set) Token: 0x06002320 RID: 8992 RVA: 0x0019626E File Offset: 0x0019446E
		public bool IsDrag { get; private set; }

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06002321 RID: 8993 RVA: 0x00196277 File Offset: 0x00194477
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

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x00196299 File Offset: 0x00194499
		// (set) Token: 0x06002323 RID: 8995 RVA: 0x001962C5 File Offset: 0x001944C5
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

		// Token: 0x06002324 RID: 8996 RVA: 0x001962FD File Offset: 0x001944FD
		public override void OnBeginDrag(PointerEventData eventData)
		{
			base.OnBeginDrag(eventData);
			this.IsDrag = true;
			this.stopNow = false;
			this.wasPageIndexDecided = false;
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0019631B File Offset: 0x0019451B
		public override void OnEndDrag(PointerEventData eventData)
		{
			base.OnEndDrag(eventData);
			this.IsDrag = false;
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x0019632B File Offset: 0x0019452B
		protected override void Awake()
		{
			base.Awake();
			this.reuseScroll = base.GetComponent<ReuseScroll>();
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x0019633F File Offset: 0x0019453F
		public void SetPage(int index)
		{
			index *= -1;
			this.AnchoredPosition = this.reuseScroll.Size * (float)index;
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0019635C File Offset: 0x0019455C
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

		// Token: 0x04001A7E RID: 6782
		private const float POWER = 10f;

		// Token: 0x04001A7F RID: 6783
		public Action<int> onScrollStopped;

		// Token: 0x04001A80 RID: 6784
		public Action<int> onPageIndexDecided;

		// Token: 0x04001A81 RID: 6785
		private ReuseScroll reuseScroll;

		// Token: 0x04001A82 RID: 6786
		private bool stopNow;

		// Token: 0x04001A83 RID: 6787
		private bool wasPageIndexDecided;
	}
}
