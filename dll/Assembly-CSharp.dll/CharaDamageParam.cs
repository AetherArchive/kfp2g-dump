using System;
using UnityEngine;

[Serializable]
public class CharaDamageParam
{
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

	public bool breakElement;

	public float damageRate;

	public int hitNum;

	public float hitInterval;

	public bool dispObjectOnce;

	public CharaDef.ArtsBlowType blowType;

	public Vector3 blowDirection;

	public float blowSpeed;

	public float blowTime;

	public bool isGrow;

	public float growthRate;

	public ActionEffectParam actionEffect;
}
