using System;
using UnityEngine;

[Serializable]
public class CharaBuffParamAbility
{
	public long abTyp
	{
		get
		{
			return CharaDef.AbnormalMask(this.abnormalType, this.abnormalType2);
		}
	}

	public bool isCountSpecified
	{
		get
		{
			return this.maxCount > 1;
		}
	}

	public int count
	{
		get
		{
			return this._count;
		}
	}

	public void AddCount()
	{
		this._count++;
	}

	public void ResetCount()
	{
		this._count = 0;
	}

	private int _count;

	[Multiline(6)]
	public string infoMessage;

	public int infoTime;

	public CharaDef.ConditionType condition;

	public int conditionHpRate;

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

	[EnumFlags]
	public CharaDef.AbilityTraits traitsTerrain = (CharaDef.AbilityTraits)(-1);

	public bool invertTraitsTerrainMask;

	[EnumFlags]
	public CharaDef.AbilityTraits2 traitsTimezone = (CharaDef.AbilityTraits2)(-1);

	public bool invertTraitsTimezoneMask;

	public CharaDef.ActionTargetType targetType;

	[EnumFlags]
	public CharaDef.EnemyMask targetMask = (CharaDef.EnemyMask)(-1);

	public bool invertTargetMask;

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

	public int waveReuseNum;

	public int giveupReuseNum;

	public int turn;

	public int sucheduledTurn;

	public float coefficient;

	public int increment;

	public bool isInvalidResistance;

	public int maxCount;
}
