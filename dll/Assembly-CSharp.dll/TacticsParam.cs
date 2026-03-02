using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class TacticsParam : ScriptableObject
{
	// Token: 0x04000CE7 RID: 3303
	public List<TacticsParam.Tactics> tacticsParam;

	// Token: 0x02000907 RID: 2311
	[Serializable]
	public class Tactics
	{
		// Token: 0x04003AF7 RID: 15095
		public TacticsParam.Tactics.Type type;

		// Token: 0x04003AF8 RID: 15096
		public string tacticsName;

		// Token: 0x04003AF9 RID: 15097
		public string paramInfo;

		// Token: 0x04003AFA RID: 15098
		public List<int> param;

		// Token: 0x0200114A RID: 4426
		public enum Type
		{
			// Token: 0x04005F00 RID: 24320
			INVALID,
			// Token: 0x04005F01 RID: 24321
			TURN_NUM,
			// Token: 0x04005F02 RID: 24322
			GIVEUP_NUM,
			// Token: 0x04005F03 RID: 24323
			HP_PER
		}
	}
}
