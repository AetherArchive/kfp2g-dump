using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class TacticsStaticSkill : ScriptableObject
{
	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060002C3 RID: 707 RVA: 0x000167B9 File Offset: 0x000149B9
	public string iconName
	{
		get
		{
			return "icon_tacticsskill_" + this.id.ToString("00");
		}
	}

	// Token: 0x04000462 RID: 1122
	public int id;

	// Token: 0x04000463 RID: 1123
	public string skillName;

	// Token: 0x04000464 RID: 1124
	[Multiline]
	public string skillInfo;

	// Token: 0x04000465 RID: 1125
	public TacticsStaticSkill.Type type;

	// Token: 0x04000466 RID: 1126
	[EnumFlags]
	public TacticsStaticSkill.TriggerTypeMask triggerTypeMask = (TacticsStaticSkill.TriggerTypeMask)(-1);

	// Token: 0x04000467 RID: 1127
	public int turn;

	// Token: 0x04000468 RID: 1128
	public int paramI0;

	// Token: 0x04000469 RID: 1129
	public int paramI1;

	// Token: 0x0400046A RID: 1130
	public int paramI2;

	// Token: 0x0400046B RID: 1131
	public int paramI3;

	// Token: 0x0400046C RID: 1132
	public float paramF0;

	// Token: 0x0400046D RID: 1133
	public float paramF1;

	// Token: 0x0400046E RID: 1134
	public float paramF2;

	// Token: 0x0400046F RID: 1135
	public float paramF3;

	// Token: 0x04000470 RID: 1136
	public int threshold0;

	// Token: 0x04000471 RID: 1137
	public int threshold1;

	// Token: 0x02000620 RID: 1568
	public enum Type
	{
		// Token: 0x04002D8B RID: 11659
		INVALID,
		// Token: 0x04002D8C RID: 11660
		ADD_DAMADE_HITRATE,
		// Token: 0x04002D8D RID: 11661
		RCV_DAMAGE_ALLDAMAGE,
		// Token: 0x04002D8E RID: 11662
		HEAL_RESURRECT,
		// Token: 0x04002D8F RID: 11663
		ADD_PLASM_CHARGE,
		// Token: 0x04002D90 RID: 11664
		ADD_MOVE_POINT,
		// Token: 0x04002D91 RID: 11665
		BAD_STATUS,
		// Token: 0x04002D92 RID: 11666
		RCV_HATE,
		// Token: 0x04002D93 RID: 11667
		ADD_BEAT_DAMAGE,
		// Token: 0x04002D94 RID: 11668
		ADD_ACTION_DAMAGE,
		// Token: 0x04002D95 RID: 11669
		ADD_TRY_DAMAGE
	}

	// Token: 0x02000621 RID: 1569
	[Flags]
	public enum TriggerTypeMask
	{
		// Token: 0x04002D97 RID: 11671
		TURN_NUM = 1,
		// Token: 0x04002D98 RID: 11672
		GIVEUP_NUM = 2,
		// Token: 0x04002D99 RID: 11673
		HP_PER = 4
	}
}
