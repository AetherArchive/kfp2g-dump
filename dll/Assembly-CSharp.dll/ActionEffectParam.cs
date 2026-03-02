using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
[Serializable]
public class ActionEffectParam
{
	// Token: 0x040003A2 RID: 930
	public string effectName;

	// Token: 0x040003A3 RID: 931
	public bool useCharaLight;

	// Token: 0x040003A4 RID: 932
	public float startOffsetTime;

	// Token: 0x040003A5 RID: 933
	public CharaDef.EffectDispType dispType;

	// Token: 0x040003A6 RID: 934
	public CharaDef.TargetNodeName dispModelNodeName;

	// Token: 0x040003A7 RID: 935
	public float flightTime;

	// Token: 0x040003A8 RID: 936
	public float hitHeight;

	// Token: 0x040003A9 RID: 937
	public Vector3 offsetPosition;

	// Token: 0x040003AA RID: 938
	public Vector3 offsetRotation;

	// Token: 0x040003AB RID: 939
	public Vector3 offsetScale;

	// Token: 0x040003AC RID: 940
	public string hitEffectName;

	// Token: 0x040003AD RID: 941
	public float hitStartOffsetTime;

	// Token: 0x040003AE RID: 942
	public Vector3 hitOffsetPosition;

	// Token: 0x040003AF RID: 943
	public Vector3 hitOffsetRotation;

	// Token: 0x040003B0 RID: 944
	public Vector3 hitOffsetScale;

	// Token: 0x040003B1 RID: 945
	public float effectStartTime;
}
