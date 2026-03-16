using System;
using UnityEngine;

public class TacticsStaticSkill : ScriptableObject
{
	public string iconName
	{
		get
		{
			return "icon_tacticsskill_" + this.id.ToString("00");
		}
	}

	public int id;

	public string skillName;

	[Multiline]
	public string skillInfo;

	public TacticsStaticSkill.Type type;

	[EnumFlags]
	public TacticsStaticSkill.TriggerTypeMask triggerTypeMask = (TacticsStaticSkill.TriggerTypeMask)(-1);

	public int turn;

	public int paramI0;

	public int paramI1;

	public int paramI2;

	public int paramI3;

	public float paramF0;

	public float paramF1;

	public float paramF2;

	public float paramF3;

	public int threshold0;

	public int threshold1;

	public enum Type
	{
		INVALID,
		ADD_DAMADE_HITRATE,
		RCV_DAMAGE_ALLDAMAGE,
		HEAL_RESURRECT,
		ADD_PLASM_CHARGE,
		ADD_MOVE_POINT,
		BAD_STATUS,
		RCV_HATE,
		ADD_BEAT_DAMAGE,
		ADD_ACTION_DAMAGE,
		ADD_TRY_DAMAGE
	}

	[Flags]
	public enum TriggerTypeMask
	{
		TURN_NUM = 1,
		GIVEUP_NUM = 2,
		HP_PER = 4
	}
}
