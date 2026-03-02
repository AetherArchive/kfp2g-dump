using System;

// Token: 0x02000058 RID: 88
[Serializable]
public class CharaBuffParam
{
	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06000282 RID: 642 RVA: 0x000151BB File Offset: 0x000133BB
	public long abTyp
	{
		get
		{
			return CharaDef.AbnormalMask(this.abnormalType, this.abnormalType2);
		}
	}

	// Token: 0x06000283 RID: 643 RVA: 0x000151CE File Offset: 0x000133CE
	public CharaBuffParam()
	{
	}

	// Token: 0x06000284 RID: 644 RVA: 0x000151FC File Offset: 0x000133FC
	public CharaBuffParam(CharaBuffParam param)
	{
		this.enemyNum = param.enemyNum;
		this.conditionEnemyNum = param.conditionEnemyNum;
		this.mysideNum = param.mysideNum;
		this.conditionMysideNum = param.conditionMysideNum;
		this.attributeMask = param.attributeMask;
		this.waveEnemyMask = param.waveEnemyMask;
		this.healthMask = param.healthMask;
		this.healthMaskType = param.healthMaskType;
		this.targetType = param.targetType;
		this.targetMask = param.targetMask;
		this.traitsTerrain = param.traitsTerrain;
		this.breakElement = param.breakElement;
		this.buffType = param.buffType;
		this.abnormalType = param.abnormalType;
		this.abnormalType2 = param.abnormalType2;
		this.spAttributeMask = param.spAttributeMask;
		this.spHealthMask = param.spHealthMask;
		this.spEnemyMask = param.spEnemyMask;
		this.timingType = param.timingType;
		this.successRate = param.successRate;
		this.giveupReuseNum = param.giveupReuseNum;
		this.turn = param.turn;
		this.scheduledTurn = param.scheduledTurn;
		this.coefficient = param.coefficient;
		this.increment = param.increment;
		this.isGrow = param.isGrow;
		this.growthRate = param.growthRate;
		this.actionEffect = param.actionEffect;
	}

	// Token: 0x04000334 RID: 820
	public int enemyNum;

	// Token: 0x04000335 RID: 821
	public CharaDef.ConditionType conditionEnemyNum;

	// Token: 0x04000336 RID: 822
	public int mysideNum;

	// Token: 0x04000337 RID: 823
	public CharaDef.ConditionType conditionMysideNum;

	// Token: 0x04000338 RID: 824
	[EnumFlags]
	public CharaDef.AttributeMask attributeMask = (CharaDef.AttributeMask)(-1);

	// Token: 0x04000339 RID: 825
	public bool invertAttributeMask;

	// Token: 0x0400033A RID: 826
	[EnumFlags]
	public CharaDef.EnemyMask waveEnemyMask = (CharaDef.EnemyMask)(-1);

	// Token: 0x0400033B RID: 827
	public bool invertWaveEnemyMask;

	// Token: 0x0400033C RID: 828
	[EnumFlags]
	public CharaDef.HealthMask healthMask = (CharaDef.HealthMask)(-1);

	// Token: 0x0400033D RID: 829
	public bool invertHealthMask;

	// Token: 0x0400033E RID: 830
	public CharaDef.HealthMaskType healthMaskType;

	// Token: 0x0400033F RID: 831
	public CharaDef.ActionTargetType targetType;

	// Token: 0x04000340 RID: 832
	[EnumFlags]
	public CharaDef.EnemyMask targetMask = (CharaDef.EnemyMask)(-1);

	// Token: 0x04000341 RID: 833
	public bool invertTargetMask;

	// Token: 0x04000342 RID: 834
	[EnumFlags]
	public CharaDef.AbilityTraits traitsTerrain = (CharaDef.AbilityTraits)(-1);

	// Token: 0x04000343 RID: 835
	public bool invertTraitsTerrain;

	// Token: 0x04000344 RID: 836
	public bool breakElement;

	// Token: 0x04000345 RID: 837
	public CharaDef.ActionBuffType buffType;

	// Token: 0x04000346 RID: 838
	[EnumFlags]
	public CharaDef.ActionAbnormalMask abnormalType;

	// Token: 0x04000347 RID: 839
	[EnumFlags]
	public CharaDef.ActionAbnormalMask2 abnormalType2;

	// Token: 0x04000348 RID: 840
	[EnumFlags]
	public CharaDef.AttributeMask spAttributeMask;

	// Token: 0x04000349 RID: 841
	[EnumFlags]
	public CharaDef.HealthMask spHealthMask;

	// Token: 0x0400034A RID: 842
	[EnumFlags]
	public CharaDef.EnemyMask spEnemyMask;

	// Token: 0x0400034B RID: 843
	public CharaDef.ActionTimingType timingType;

	// Token: 0x0400034C RID: 844
	public int enhanceMpBorder;

	// Token: 0x0400034D RID: 845
	public int successRate;

	// Token: 0x0400034E RID: 846
	public int giveupReuseNum;

	// Token: 0x0400034F RID: 847
	public int turn;

	// Token: 0x04000350 RID: 848
	public int scheduledTurn;

	// Token: 0x04000351 RID: 849
	public double coefficient;

	// Token: 0x04000352 RID: 850
	public int increment;

	// Token: 0x04000353 RID: 851
	public bool isGrow;

	// Token: 0x04000354 RID: 852
	public double growthRate;

	// Token: 0x04000355 RID: 853
	public ActionEffectParam actionEffect;
}
