using System;
using UnityEngine;

// Token: 0x0200005C RID: 92
[Serializable]
public class CharaActionParam
{
	// Token: 0x04000389 RID: 905
	public CharaDef.ActionDamageType attackType;

	// Token: 0x0400038A RID: 906
	public bool noLook;

	// Token: 0x0400038B RID: 907
	public Vector3 attackPositionOffset;

	// Token: 0x0400038C RID: 908
	public float quakeStartTime;

	// Token: 0x0400038D RID: 909
	public float quakeEndTime;

	// Token: 0x0400038E RID: 910
	public float quakeWidth;

	// Token: 0x0400038F RID: 911
	public int quakeNum;

	// Token: 0x04000390 RID: 912
	public CharaDef.MonitorQuakeType quakeType;

	// Token: 0x04000391 RID: 913
	public float voiceDelay;

	// Token: 0x04000392 RID: 914
	public CharaDef.ActionCameraType cameraType;

	// Token: 0x04000393 RID: 915
	public Vector3 skillCameraOffsetPosition;

	// Token: 0x04000394 RID: 916
	public Vector3 skillCameraOffsetTarget;
}
