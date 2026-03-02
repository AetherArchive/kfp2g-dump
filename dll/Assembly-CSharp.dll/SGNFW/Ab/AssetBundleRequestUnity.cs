using System;
using UnityEngine;

namespace SGNFW.Ab
{
	// Token: 0x0200027C RID: 636
	public class AssetBundleRequestUnity : AssetBundleRequest
	{
		// Token: 0x060026CC RID: 9932 RVA: 0x001A3A21 File Offset: 0x001A1C21
		public AssetBundleRequestUnity(AssetBundleRequest request)
		{
			this.request_ = request;
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x060026CD RID: 9933 RVA: 0x001A3A30 File Offset: 0x001A1C30
		public override Object[] allAssets
		{
			get
			{
				return this.request_.allAssets;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x001A3A3D File Offset: 0x001A1C3D
		public override Object asset
		{
			get
			{
				return this.request_.asset;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x060026CF RID: 9935 RVA: 0x001A3A4A File Offset: 0x001A1C4A
		public override bool isDone
		{
			get
			{
				return this.request_.isDone;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x001A3A57 File Offset: 0x001A1C57
		// (set) Token: 0x060026D1 RID: 9937 RVA: 0x001A3A64 File Offset: 0x001A1C64
		public override int priority
		{
			get
			{
				return this.request_.priority;
			}
			set
			{
				this.request_.priority = value;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x060026D2 RID: 9938 RVA: 0x001A3A72 File Offset: 0x001A1C72
		public override float progress
		{
			get
			{
				return this.request_.progress;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x060026D3 RID: 9939 RVA: 0x001A3A7F File Offset: 0x001A1C7F
		public override bool keepWaiting
		{
			get
			{
				return !this.request_.isDone;
			}
		}

		// Token: 0x04001C71 RID: 7281
		private AssetBundleRequest request_;
	}
}
