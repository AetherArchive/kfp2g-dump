using System;
using UnityEngine;

namespace SGNFW.Ab
{
	// Token: 0x0200027D RID: 637
	public class AssetBundleRequestMock : AssetBundleRequest
	{
		// Token: 0x060026D4 RID: 9940 RVA: 0x001A3A8F File Offset: 0x001A1C8F
		public AssetBundleRequestMock(Object asset)
		{
			this.asset_ = asset;
			this.startTime_ = Time.realtimeSinceStartup;
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x001A3AA9 File Offset: 0x001A1CA9
		public AssetBundleRequestMock(Object[] allAssets)
		{
			this.allAssets_ = allAssets;
			this.startTime_ = Time.realtimeSinceStartup;
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x060026D6 RID: 9942 RVA: 0x001A3AC3 File Offset: 0x001A1CC3
		public override Object[] allAssets
		{
			get
			{
				return this.allAssets_;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x060026D7 RID: 9943 RVA: 0x001A3ACB File Offset: 0x001A1CCB
		public override Object asset
		{
			get
			{
				return this.asset_;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x060026D8 RID: 9944 RVA: 0x001A3AD3 File Offset: 0x001A1CD3
		public override bool isDone
		{
			get
			{
				return this.startTime_ + 0.1f < Time.realtimeSinceStartup;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x060026D9 RID: 9945 RVA: 0x001A3AE8 File Offset: 0x001A1CE8
		// (set) Token: 0x060026DA RID: 9946 RVA: 0x001A3AF0 File Offset: 0x001A1CF0
		public override int priority { get; set; }

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x060026DB RID: 9947 RVA: 0x001A3AF9 File Offset: 0x001A1CF9
		public override float progress
		{
			get
			{
				return Mathf.Clamp01(Time.realtimeSinceStartup - this.startTime_ / 0.1f);
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x060026DC RID: 9948 RVA: 0x001A3B12 File Offset: 0x001A1D12
		public override bool keepWaiting
		{
			get
			{
				return !this.isDone;
			}
		}

		// Token: 0x04001C73 RID: 7283
		private Object asset_;

		// Token: 0x04001C74 RID: 7284
		private Object[] allAssets_;

		// Token: 0x04001C75 RID: 7285
		private float startTime_;

		// Token: 0x04001C76 RID: 7286
		private const float LOAD_TIME = 0.1f;
	}
}
