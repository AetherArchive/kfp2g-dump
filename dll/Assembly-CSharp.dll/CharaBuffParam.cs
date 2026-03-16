using System;

[Serializable]
public class CharaBuffParam
{
	public long abTyp
	{
		get
		{
			return CharaDef.AbnormalMask(this.abnormalType, this.abnormalType2);
		}
	}

	public CharaBuffParam()
	{
	}

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

	public int enemyNum;

	public CharaDef.ConditionType conditionEnemyNum;

	public int mysideNum;

	public CharaDef.ConditionType conditionMysideNum;

	[EnumFlags]
	public CharaDef.AttributeMask attributeMask = (CharaDef.AttributeMask)(-1);

	public bool invertAttributeMask;

	[EnumFlags]
	public CharaDef.EnemyMask waveEnemyMask = (CharaDef.EnemyMask)(-1);

	public bool invertWaveEnemyMask;

	[EnumFlags]
	public CharaDef.HealthMask healthMask = (CharaDef.HealthMask)(-1);

	public bool invertHealthMask;

	public CharaDef.HealthMaskType healthMaskType;

	public CharaDef.ActionTargetType targetType;

	[EnumFlags]
	public CharaDef.EnemyMask targetMask = (CharaDef.EnemyMask)(-1);

	public bool invertTargetMask;

	[EnumFlags]
	public CharaDef.AbilityTraits traitsTerrain = (CharaDef.AbilityTraits)(-1);

	public bool invertTraitsTerrain;

	public bool breakElement;

	public CharaDef.ActionBuffType buffType;

	[EnumFlags]
	public CharaDef.ActionAbnormalMask abnormalType;

	[EnumFlags]
	public CharaDef.ActionAbnormalMask2 abnormalType2;

	[EnumFlags]
	public CharaDef.AttributeMask spAttributeMask;

	[EnumFlags]
	public CharaDef.HealthMask spHealthMask;

	[EnumFlags]
	public CharaDef.EnemyMask spEnemyMask;

	public CharaDef.ActionTimingType timingType;

	public int enhanceMpBorder;

	public int successRate;

	public int giveupReuseNum;

	public int turn;

	public int scheduledTurn;

	public double coefficient;

	public int increment;

	public bool isGrow;

	public double growthRate;

	public ActionEffectParam actionEffect;
}
