using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C7 RID: 199
public class EffectSeParameter : ScriptableObject
{
	// Token: 0x0400075E RID: 1886
	public List<EffectSeParameter.PackData> packList;

	// Token: 0x020007C1 RID: 1985
	[Serializable]
	public class PackData
	{
		// Token: 0x04003492 RID: 13458
		public string effectName;

		// Token: 0x04003493 RID: 13459
		public string seName;

		// Token: 0x04003494 RID: 13460
		public string cuesheetName = "se_cb";
	}
}
