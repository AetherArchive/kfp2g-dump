using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
[Serializable]
public class CharaBuffParamAbility
{
	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06000285 RID: 645 RVA: 0x00015382 File Offset: 0x00013582
	public long abTyp
	{
		get
		{
			return CharaDef.AbnormalMask(this.abnormalType, this.abnormalType2);
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000286 RID: 646 RVA: 0x00015395 File Offset: 0x00013595
	public bool isCountSpecified
	{
		get
		{
			return this.maxCount > 1;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000287 RID: 647 RVA: 0x000153A0 File Offset: 0x000135A0
	public int count
	{
		get
		{
			return this._count;
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x000153A8 File Offset: 0x000135A8
	public void AddCount()
	{
		this._count++;
	}

	// Token: 0x06000289 RID: 649 RVA: 0x000153B8 File Offset: 0x000135B8
	public void ResetCount()
	{
		this._count = 0;
	}

	// Token: 0x04000356 RID: 854
	private int _count;

	// Token: 0x04000357 RID: 855
	[Multiline(6)]
	public string infoMessage;

	// Token: 0x04000358 RID: 856
	public int infoTime;

	// Token: 0x04000359 RID: 857
	public CharaDef.ConditionType condition;

	// Token: 0x0400035A RID: 858
	public int conditionHpRate;

	// Token: 0x0400035B RID: 859
	public int enemyNum;

	// Token: 0x0400035C RID: 860
	public CharaDef.ConditionType conditionEnemyNum;

	// Token: 0x0400035D RID: 861
	public int mysideNum;

	// Token: 0x0400035E RID: 862
	public CharaDef.ConditionType conditionMysideNum;

	// Token: 0x0400035F RID: 863
	[EnumFlags]
	public CharaDef.AttributeMask attributeMask = (CharaDef.AttributeMask)(-1);

	// Token: 0x04000360 RID: 864
	public bool invertAttributeMask;

	// Token: 0x04000361 RID: 865
	[EnumFlags]
	public CharaDef.EnemyMask waveEnemyMask = (CharaDef.EnemyMask)(-1);

	// Token: 0x04000362 RID: 866
	public bool invertWaveEnemyMask;

	// Token: 0x04000363 RID: 867
	[EnumFlags]
	public CharaDef.HealthMask healthMask = (CharaDef.HealthMask)(-1);

	// Token: 0x04000364 RID: 868
	public bool invertHealthMask;

	// Token: 0x04000365 RID: 869
	public CharaDef.HealthMaskType healthMaskType;

	// Token: 0x04000366 RID: 870
	[EnumFlags]
	public CharaDef.AbilityTraits traitsTerrain = (CharaDef.AbilityTraits)(-1);

	// Token: 0x04000367 RID: 871
	public bool invertTraitsTerrainMask;

	// Token: 0x04000368 RID: 872
	[EnumFlags]
	public CharaDef.AbilityTraits2 traitsTimezone = (CharaDef.AbilityTraits2)(-1);

	// Token: 0x04000369 RID: 873
	public bool invertTraitsTimezoneMask;

	// Token: 0x0400036A RID: 874
	public CharaDef.ActionTargetType targetType;

	// Token: 0x0400036B RID: 875
	[EnumFlags]
	public CharaDef.EnemyMask targetMask = (CharaDef.EnemyMask)(-1);

	// Token: 0x0400036C RID: 876
	public bool invertTargetMask;

	// Token: 0x0400036D RID: 877
	public bool breakElement;

	// Token: 0x0400036E RID: 878
	public CharaDef.ActionBuffType buffType;

	// Token: 0x0400036F RID: 879
	[EnumFlags]
	public CharaDef.ActionAbnormalMask abnormalType;

	// Token: 0x04000370 RID: 880
	[EnumFlags]
	public CharaDef.ActionAbnormalMask2 abnormalType2;

	// Token: 0x04000371 RID: 881
	[EnumFlags]
	public CharaDef.AttributeMask spAttributeMask;

	// Token: 0x04000372 RID: 882
	[EnumFlags]
	public CharaDef.HealthMask spHealthMask;

	// Token: 0x04000373 RID: 883
	[EnumFlags]
	public CharaDef.EnemyMask spEnemyMask;

	// Token: 0x04000374 RID: 884
	public int waveReuseNum;

	// Token: 0x04000375 RID: 885
	public int giveupReuseNum;

	// Token: 0x04000376 RID: 886
	public int turn;

	// Token: 0x04000377 RID: 887
	public int sucheduledTurn;

	// Token: 0x04000378 RID: 888
	public float coefficient;

	// Token: 0x04000379 RID: 889
	public int increment;

	// Token: 0x0400037A RID: 890
	public bool isInvalidResistance;

	// Token: 0x0400037B RID: 891
	public int maxCount;
}
