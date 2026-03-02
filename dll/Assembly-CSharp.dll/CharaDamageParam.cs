using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
[Serializable]
public class CharaDamageParam
{
	// Token: 0x0400031A RID: 794
	public int enemyNum;

	// Token: 0x0400031B RID: 795
	public CharaDef.ConditionType conditionEnemyNum;

	// Token: 0x0400031C RID: 796
	public int mysideNum;

	// Token: 0x0400031D RID: 797
	public CharaDef.ConditionType conditionMysideNum;

	// Token: 0x0400031E RID: 798
	[EnumFlags]
	public CharaDef.AttributeMask attributeMask = (CharaDef.AttributeMask)(-1);

	// Token: 0x0400031F RID: 799
	public bool invertAttributeMask;

	// Token: 0x04000320 RID: 800
	[EnumFlags]
	public CharaDef.EnemyMask waveEnemyMask = (CharaDef.EnemyMask)(-1);

	// Token: 0x04000321 RID: 801
	public bool invertWaveEnemyMask;

	// Token: 0x04000322 RID: 802
	[EnumFlags]
	public CharaDef.HealthMask healthMask = (CharaDef.HealthMask)(-1);

	// Token: 0x04000323 RID: 803
	public bool invertHealthMask;

	// Token: 0x04000324 RID: 804
	public CharaDef.HealthMaskType healthMaskType;

	// Token: 0x04000325 RID: 805
	public CharaDef.ActionTargetType targetType;

	// Token: 0x04000326 RID: 806
	[EnumFlags]
	public CharaDef.EnemyMask targetMask = (CharaDef.EnemyMask)(-1);

	// Token: 0x04000327 RID: 807
	public bool invertTargetMask;

	// Token: 0x04000328 RID: 808
	public bool breakElement;

	// Token: 0x04000329 RID: 809
	public float damageRate;

	// Token: 0x0400032A RID: 810
	public int hitNum;

	// Token: 0x0400032B RID: 811
	public float hitInterval;

	// Token: 0x0400032C RID: 812
	public bool dispObjectOnce;

	// Token: 0x0400032D RID: 813
	public CharaDef.ArtsBlowType blowType;

	// Token: 0x0400032E RID: 814
	public Vector3 blowDirection;

	// Token: 0x0400032F RID: 815
	public float blowSpeed;

	// Token: 0x04000330 RID: 816
	public float blowTime;

	// Token: 0x04000331 RID: 817
	public bool isGrow;

	// Token: 0x04000332 RID: 818
	public float growthRate;

	// Token: 0x04000333 RID: 819
	public ActionEffectParam actionEffect;
}
