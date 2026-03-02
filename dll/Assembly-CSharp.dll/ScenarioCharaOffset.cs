using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class ScenarioCharaOffset : ScriptableObject
{
	// Token: 0x04000A60 RID: 2656
	public List<ScenarioSetValues.CharaOffset> mCharaOffset;

	// Token: 0x04000A61 RID: 2657
	public List<ScenarioCharaOffset.CharaPosition> mCharaPosition;

	// Token: 0x0200084F RID: 2127
	[Serializable]
	public class CharaPosition
	{
		// Token: 0x0400376D RID: 14189
		public string name;

		// Token: 0x0400376E RID: 14190
		public Vector3 position;

		// Token: 0x0400376F RID: 14191
		public Vector3 rotation;

		// Token: 0x04003770 RID: 14192
		public Vector3 scale;

		// Token: 0x04003771 RID: 14193
		public string shake;

		// Token: 0x04003772 RID: 14194
		public string arrows;
	}
}
