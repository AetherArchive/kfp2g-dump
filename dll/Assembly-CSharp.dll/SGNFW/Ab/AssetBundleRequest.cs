using System;
using UnityEngine;

namespace SGNFW.Ab
{
	// Token: 0x0200027B RID: 635
	public abstract class AssetBundleRequest : CustomYieldInstruction
	{
		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x060026C5 RID: 9925
		public abstract Object[] allAssets { get; }

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x060026C6 RID: 9926
		public abstract Object asset { get; }

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x060026C7 RID: 9927
		public abstract bool isDone { get; }

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x060026C8 RID: 9928
		// (set) Token: 0x060026C9 RID: 9929
		public abstract int priority { get; set; }

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x060026CA RID: 9930
		public abstract float progress { get; }
	}
}
