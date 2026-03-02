using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
[Serializable]
public class CharaMotionParam
{
	// Token: 0x04000395 RID: 917
	public float advanceTime;

	// Token: 0x04000396 RID: 918
	public float heightDifference;

	// Token: 0x04000397 RID: 919
	public float motionStartTime;

	// Token: 0x04000398 RID: 920
	public string charaEffectName;

	// Token: 0x04000399 RID: 921
	public bool useCharaLight;

	// Token: 0x0400039A RID: 922
	public bool jointMove;

	// Token: 0x0400039B RID: 923
	public float charaEffectStartOffsetTime;

	// Token: 0x0400039C RID: 924
	public Vector3 charaEffectOffsetPosition;

	// Token: 0x0400039D RID: 925
	public Vector3 charaEffectOffsetRotation;

	// Token: 0x0400039E RID: 926
	public Vector3 charaEffectOffsetScale;

	// Token: 0x0400039F RID: 927
	public float retreatTime;

	// Token: 0x040003A0 RID: 928
	public float nextActionTime;

	// Token: 0x040003A1 RID: 929
	public CharaMotionDefine.ActKey motionActKey;
}
