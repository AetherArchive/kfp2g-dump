using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class EnemyStaticBase : ScriptableObject
{
	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000296 RID: 662 RVA: 0x000156E5 File Offset: 0x000138E5
	// (set) Token: 0x06000297 RID: 663 RVA: 0x000156ED File Offset: 0x000138ED
	public int id { get; set; }

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000298 RID: 664 RVA: 0x000156F6 File Offset: 0x000138F6
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

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000299 RID: 665 RVA: 0x00015728 File Offset: 0x00013928
	// (set) Token: 0x0600029A RID: 666 RVA: 0x00015730 File Offset: 0x00013930
	public CharaStaticAbility abilityData { get; set; }

	// Token: 0x040003FA RID: 1018
	public CharaDef.Type charaType;

	// Token: 0x040003FB RID: 1019
	public bool rare;

	// Token: 0x040003FC RID: 1020
	public string charaName;

	// Token: 0x040003FD RID: 1021
	public string eponymName;

	// Token: 0x040003FE RID: 1022
	public CharaDef.AttributeType attribute;

	// Token: 0x040003FF RID: 1023
	public float width;

	// Token: 0x04000400 RID: 1024
	public float height;

	// Token: 0x04000401 RID: 1025
	public float AbnormalEffectHeadScale;

	// Token: 0x04000402 RID: 1026
	public float AbnormalEffectHeadY;

	// Token: 0x04000403 RID: 1027
	public float AbnormalEffectHeadZ;

	// Token: 0x04000404 RID: 1028
	public float AbnormalEffectRootScale;

	// Token: 0x04000405 RID: 1029
	public float AbnormalEffectRootY;

	// Token: 0x04000406 RID: 1030
	public float AbnormalEffectRootZ;

	// Token: 0x04000407 RID: 1031
	public float modelDispOfsset;

	// Token: 0x04000408 RID: 1032
	public float nearAttackPosition;

	// Token: 0x04000409 RID: 1033
	public int actionNum;

	// Token: 0x0400040A RID: 1034
	public int increaseKpByAttack;

	// Token: 0x0400040B RID: 1035
	public int increaseKpByDamage;

	// Token: 0x0400040C RID: 1036
	public string artsParamId;

	// Token: 0x0400040D RID: 1037
	public string artsName;

	// Token: 0x0400040E RID: 1038
	public string artsInfo;

	// Token: 0x0400040F RID: 1039
	public CharaDef.EnemyActionPattern actionPattern;

	// Token: 0x04000410 RID: 1040
	public List<EnemyStaticBase.ActionParam> actionParamList;

	// Token: 0x04000411 RID: 1041
	public int hpParamLv1;

	// Token: 0x04000412 RID: 1042
	public int hpParamLvMiddle;

	// Token: 0x04000413 RID: 1043
	public int hpLvMiddleNum;

	// Token: 0x04000414 RID: 1044
	public int hpParamLv99;

	// Token: 0x04000415 RID: 1045
	public int atkParamLv1;

	// Token: 0x04000416 RID: 1046
	public int atkParamLvMiddle;

	// Token: 0x04000417 RID: 1047
	public int atkLvMiddleNum;

	// Token: 0x04000418 RID: 1048
	public int atkParamLv99;

	// Token: 0x04000419 RID: 1049
	public int defParamLv1;

	// Token: 0x0400041A RID: 1050
	public int defParamLvMiddle;

	// Token: 0x0400041B RID: 1051
	public int defLvMiddleNum;

	// Token: 0x0400041C RID: 1052
	public int defParamLv99;

	// Token: 0x0400041D RID: 1053
	public int maxStockMp;

	// Token: 0x0400041E RID: 1054
	public int avoidRatio;

	// Token: 0x0400041F RID: 1055
	public List<string> modelEffect;

	// Token: 0x04000420 RID: 1056
	public string abilityFileName;

	// Token: 0x04000421 RID: 1057
	public int modelId;

	// Token: 0x04000422 RID: 1058
	public string modelNodeName;

	// Token: 0x04000423 RID: 1059
	public string deathEffect;

	// Token: 0x04000424 RID: 1060
	public float deathEffectScale = 1f;

	// Token: 0x04000425 RID: 1061
	public CharaDef.PartsType partsType;

	// Token: 0x04000426 RID: 1062
	public string breakPartsNodeName;

	// Token: 0x04000427 RID: 1063
	public bool isHuge;

	// Token: 0x04000428 RID: 1064
	public float adjustPosX;

	// Token: 0x0400042A RID: 1066
	public List<string> deathEffectNameList;

	// Token: 0x0400042B RID: 1067
	public string escapeEffectNameS;

	// Token: 0x0400042C RID: 1068
	public string escapeEffectNameM;

	// Token: 0x0200061D RID: 1565
	[Serializable]
	public class ActionParam
	{
		// Token: 0x04002D7A RID: 11642
		public int actionPoint;

		// Token: 0x04002D7B RID: 11643
		public string attackParamId;

		// Token: 0x04002D7C RID: 11644
		public string death;

		// Token: 0x04002D7D RID: 11645
		public string alive;
	}
}
