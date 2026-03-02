using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class CharaStaticWaitSkill : ScriptableObject
{
	// Token: 0x040003EB RID: 1003
	public string skillName;

	// Token: 0x040003EC RID: 1004
	[Multiline(6)]
	public string skillEffect;

	// Token: 0x040003ED RID: 1005
	public bool inBack = true;

	// Token: 0x040003EE RID: 1006
	public bool atttackJoin;

	// Token: 0x040003EF RID: 1007
	public int activationRate;

	// Token: 0x040003F0 RID: 1008
	public int activationNum;

	// Token: 0x040003F1 RID: 1009
	public string charaEffectName;

	// Token: 0x040003F2 RID: 1010
	public bool noJoint;

	// Token: 0x040003F3 RID: 1011
	public float charaEffectStartOffsetTime;

	// Token: 0x040003F4 RID: 1012
	public Vector3 charaEffectOffsetPosition;

	// Token: 0x040003F5 RID: 1013
	public Vector3 charaEffectOffsetRotation;

	// Token: 0x040003F6 RID: 1014
	public Vector3 charaEffectOffsetScale;

	// Token: 0x040003F7 RID: 1015
	public List<CharaBuffParam> buffList;

	// Token: 0x040003F8 RID: 1016
	public bool setEffectLayerToLoop;
}
