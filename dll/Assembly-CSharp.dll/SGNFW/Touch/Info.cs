using System;
using UnityEngine;

namespace SGNFW.Touch
{
	// Token: 0x0200023C RID: 572
	public class Info
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060023F5 RID: 9205 RVA: 0x0019AFF7 File Offset: 0x001991F7
		// (set) Token: 0x060023F6 RID: 9206 RVA: 0x0019AFFF File Offset: 0x001991FF
		public Vector2 CurrentPosition
		{
			get
			{
				return this.currentPosition;
			}
			set
			{
				this.currentPosition = value;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060023F7 RID: 9207 RVA: 0x0019B008 File Offset: 0x00199208
		// (set) Token: 0x060023F8 RID: 9208 RVA: 0x0019B010 File Offset: 0x00199210
		public Vector2 InitPosition
		{
			get
			{
				return this.initPosition;
			}
			set
			{
				this.initPosition = value;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060023F9 RID: 9209 RVA: 0x0019B019 File Offset: 0x00199219
		// (set) Token: 0x060023FA RID: 9210 RVA: 0x0019B021 File Offset: 0x00199221
		public Vector2 Direction
		{
			get
			{
				return this.direction;
			}
			set
			{
				this.direction = value;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060023FB RID: 9211 RVA: 0x0019B02A File Offset: 0x0019922A
		// (set) Token: 0x060023FC RID: 9212 RVA: 0x0019B032 File Offset: 0x00199232
		public Vector2 DeltaPosition
		{
			get
			{
				return this.deltaPosition;
			}
			set
			{
				this.deltaPosition = value;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x0019B03B File Offset: 0x0019923B
		// (set) Token: 0x060023FE RID: 9214 RVA: 0x0019B043 File Offset: 0x00199243
		public float Speed
		{
			get
			{
				return this.speed;
			}
			set
			{
				this.speed = value;
			}
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x0019B04C File Offset: 0x0019924C
		public Info()
		{
			this.currentPosition = Vector2.zero;
			this.initPosition = Vector2.zero;
			this.direction = Vector2.zero;
			this.deltaPosition = Vector2.zero;
			this.speed = 0f;
		}

		// Token: 0x04001B01 RID: 6913
		private Vector2 currentPosition;

		// Token: 0x04001B02 RID: 6914
		private Vector2 initPosition;

		// Token: 0x04001B03 RID: 6915
		private Vector2 direction;

		// Token: 0x04001B04 RID: 6916
		private Vector2 deltaPosition;

		// Token: 0x04001B05 RID: 6917
		private float speed;
	}
}
