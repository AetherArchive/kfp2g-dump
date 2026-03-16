using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStaticBase : ScriptableObject
{
	public int id { get; set; }

	public CharaDef.AttributeMask attributeMask
	{
		get
		{
			if (this.attribute != CharaDef.AttributeType.ALL)
			{
				return (CharaDef.AttributeMask)Enum.Parse(typeof(CharaDef.AttributeMask), this.attribute.ToString(), true);
			}
			return (CharaDef.AttributeMask)0;
		}
	}

	public CharaStaticAbility abilityData { get; set; }

	public CharaDef.Type charaType;

	public bool rare;

	public string charaName;

	public string eponymName;

	public CharaDef.AttributeType attribute;

	public float width;

	public float height;

	public float AbnormalEffectHeadScale;

	public float AbnormalEffectHeadY;

	public float AbnormalEffectHeadZ;

	public float AbnormalEffectRootScale;

	public float AbnormalEffectRootY;

	public float AbnormalEffectRootZ;

	public float modelDispOfsset;

	public float nearAttackPosition;

	public int actionNum;

	public int increaseKpByAttack;

	public int increaseKpByDamage;

	public string artsParamId;

	public string artsName;

	public string artsInfo;

	public CharaDef.EnemyActionPattern actionPattern;

	public List<EnemyStaticBase.ActionParam> actionParamList;

	public int hpParamLv1;

	public int hpParamLvMiddle;

	public int hpLvMiddleNum;

	public int hpParamLv99;

	public int atkParamLv1;

	public int atkParamLvMiddle;

	public int atkLvMiddleNum;

	public int atkParamLv99;

	public int defParamLv1;

	public int defParamLvMiddle;

	public int defLvMiddleNum;

	public int defParamLv99;

	public int maxStockMp;

	public int avoidRatio;

	public List<string> modelEffect;

	public string abilityFileName;

	public int modelId;

	public string modelNodeName;

	public string deathEffect;

	public float deathEffectScale = 1f;

	public CharaDef.PartsType partsType;

	public string breakPartsNodeName;

	public bool isHuge;

	public float adjustPosX;

	public List<string> deathEffectNameList;

	public string escapeEffectNameS;

	public string escapeEffectNameM;

	[Serializable]
	public class ActionParam
	{
		public int actionPoint;

		public string attackParamId;

		public string death;

		public string alive;
	}
}
