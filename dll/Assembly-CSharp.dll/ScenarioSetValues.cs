using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class ScenarioSetValues : MonoBehaviour
{
	// Token: 0x04000AF2 RID: 2802
	[SerializeField]
	public Vector3[] mCharaPosition;

	// Token: 0x04000AF3 RID: 2803
	[SerializeField]
	public Vector3[] mCharaRotation;

	// Token: 0x04000AF4 RID: 2804
	[SerializeField]
	public List<ScenarioSetValues.CharaOffset> mCharaOffset;

	// Token: 0x0200087A RID: 2170
	[Serializable]
	public class CharaOffset
	{
		// Token: 0x04003929 RID: 14633
		public string model;

		// Token: 0x0400392A RID: 14634
		public Vector3 position;

		// Token: 0x0400392B RID: 14635
		public Vector3 scale;
	}
}
