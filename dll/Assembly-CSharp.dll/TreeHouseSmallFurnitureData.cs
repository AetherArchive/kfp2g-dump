using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class TreeHouseSmallFurnitureData : ScriptableObject
{
	// Token: 0x04001410 RID: 5136
	public int reactionId;

	// Token: 0x04001411 RID: 5137
	public List<TreeHouseSmallFurnitureData.MotionData> motionDataList;

	// Token: 0x04001412 RID: 5138
	public List<TreeHouseSmallFurnitureData.OptData> optDataList;

	// Token: 0x02000E5B RID: 3675
	[Serializable]
	public class MotionData
	{
		// Token: 0x0400529D RID: 21149
		public CharaMotionDefine.ActKey actKey;

		// Token: 0x0400529E RID: 21150
		public string modelName;

		// Token: 0x0400529F RID: 21151
		public string nodeName;

		// Token: 0x040052A0 RID: 21152
		public Vector3 havePos;

		// Token: 0x040052A1 RID: 21153
		public Vector3 haveRot;
	}

	// Token: 0x02000E5C RID: 3676
	[Serializable]
	public class OptData
	{
		// Token: 0x040052A2 RID: 21154
		public string modelName;

		// Token: 0x040052A3 RID: 21155
		public string nodeName;

		// Token: 0x040052A4 RID: 21156
		public Vector3 putPos;

		// Token: 0x040052A5 RID: 21157
		public Vector3 putRot;
	}
}
