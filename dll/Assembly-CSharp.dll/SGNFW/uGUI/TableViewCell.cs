using System;
using UnityEngine;

namespace SGNFW.uGUI
{
	// Token: 0x02000233 RID: 563
	public class TableViewCell<T> : MonoBehaviour
	{
		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x0600237D RID: 9085 RVA: 0x0019890E File Offset: 0x00196B0E
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

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x0600237E RID: 9086 RVA: 0x00198930 File Offset: 0x00196B30
		// (set) Token: 0x0600237F RID: 9087 RVA: 0x00198938 File Offset: 0x00196B38
		public int DataIndex { get; set; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x00198941 File Offset: 0x00196B41
		// (set) Token: 0x06002381 RID: 9089 RVA: 0x00198954 File Offset: 0x00196B54
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

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06002382 RID: 9090 RVA: 0x00198981 File Offset: 0x00196B81
		// (set) Token: 0x06002383 RID: 9091 RVA: 0x00198994 File Offset: 0x00196B94
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

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x001989C4 File Offset: 0x00196BC4
		// (set) Token: 0x06002385 RID: 9093 RVA: 0x00198A04 File Offset: 0x00196C04
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

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06002386 RID: 9094 RVA: 0x00198A54 File Offset: 0x00196C54
		// (set) Token: 0x06002387 RID: 9095 RVA: 0x00198A94 File Offset: 0x00196C94
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

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06002388 RID: 9096 RVA: 0x00198AE4 File Offset: 0x00196CE4
		// (set) Token: 0x06002389 RID: 9097 RVA: 0x00198B24 File Offset: 0x00196D24
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

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x0600238A RID: 9098 RVA: 0x00198B74 File Offset: 0x00196D74
		// (set) Token: 0x0600238B RID: 9099 RVA: 0x00198BB4 File Offset: 0x00196DB4
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

		// Token: 0x0600238C RID: 9100 RVA: 0x00198C01 File Offset: 0x00196E01
		public virtual void UpdateContent(T itemData, int index)
		{
		}

		// Token: 0x04001ACD RID: 6861
		private RectTransform cachedRectTransform;
	}
}
