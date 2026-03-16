using System;
using System.Collections.Generic;

public class CharaDef
{
	public static string GetAttributeName(CharaDef.AttributeType type)
	{
		switch (type)
		{
		case CharaDef.AttributeType.ALL:
			return "オール";
		case CharaDef.AttributeType.RED:
			return "ファニー";
		case CharaDef.AttributeType.GREEN:
			return "フレンドリー";
		case CharaDef.AttributeType.BLUE:
			return "リラックス";
		case CharaDef.AttributeType.PINK:
			return "ラブリー";
		case CharaDef.AttributeType.LIME:
			return "アクティブ";
		case CharaDef.AttributeType.AQUA:
			return "マイペース";
		default:
			return "";
		}
	}

	public static CharaDef.AttributeType AttributeMask2Type(CharaDef.AttributeMask msk)
	{
		CharaDef.AttributeType attributeType = CharaDef.AttributeType.ALL;
		if ((msk & CharaDef.AttributeMask.RED) != (CharaDef.AttributeMask)0)
		{
			attributeType = CharaDef.AttributeType.RED;
		}
		else if ((msk & CharaDef.AttributeMask.GREEN) != (CharaDef.AttributeMask)0)
		{
			attributeType = CharaDef.AttributeType.GREEN;
		}
		else if ((msk & CharaDef.AttributeMask.BLUE) != (CharaDef.AttributeMask)0)
		{
			attributeType = CharaDef.AttributeType.BLUE;
		}
		else if ((msk & CharaDef.AttributeMask.PINK) != (CharaDef.AttributeMask)0)
		{
			attributeType = CharaDef.AttributeType.PINK;
		}
		else if ((msk & CharaDef.AttributeMask.LIME) != (CharaDef.AttributeMask)0)
		{
			attributeType = CharaDef.AttributeType.LIME;
		}
		else if ((msk & CharaDef.AttributeMask.AQUA) != (CharaDef.AttributeMask)0)
		{
			attributeType = CharaDef.AttributeType.AQUA;
		}
		return attributeType;
	}

	public static CharaDef.EnemyMask Type2EnemyMask(CharaDef.Type typ)
	{
		if (typ == CharaDef.Type.BOSS)
		{
			return CharaDef.EnemyMask.BOSS;
		}
		if (typ == CharaDef.Type.ENEMY)
		{
			return CharaDef.EnemyMask.MOB;
		}
		if (typ != CharaDef.Type.PARTS)
		{
			return CharaDef.EnemyMask.FRIENDS;
		}
		return CharaDef.EnemyMask.PARTS;
	}

	public static string GetEnemyName(CharaDef.EnemyMask mask)
	{
		if ((mask & CharaDef.EnemyMask.BOSS) != (CharaDef.EnemyMask)0)
		{
			return PrjUtil.MakeMessage("強敵");
		}
		if ((mask & CharaDef.EnemyMask.PARTS) != (CharaDef.EnemyMask)0)
		{
			return PrjUtil.MakeMessage("部位");
		}
		if ((mask & CharaDef.EnemyMask.MOB) != (CharaDef.EnemyMask)0)
		{
			return PrjUtil.MakeMessage("強敵以外");
		}
		if ((mask & CharaDef.EnemyMask.FRIENDS) != (CharaDef.EnemyMask)0)
		{
			return PrjUtil.MakeMessage("フレンズ");
		}
		return "";
	}

	public static long GetConvertHealthToAbnormal(CharaDef.HealthMask health)
	{
		if (health <= CharaDef.HealthMask.UNHEAL)
		{
			if (health <= CharaDef.HealthMask.SLEEP)
			{
				if (health == CharaDef.HealthMask.POISON)
				{
					return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask.POISON);
				}
				if (health == CharaDef.HealthMask.STUN)
				{
					return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask.STUN);
				}
				if (health == CharaDef.HealthMask.SLEEP)
				{
					return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask.SLEEP);
				}
			}
			else if (health <= CharaDef.HealthMask.ICE)
			{
				if (health == CharaDef.HealthMask.SEAL)
				{
					return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask.SEAL);
				}
				if (health == CharaDef.HealthMask.ICE)
				{
					return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask.ICE);
				}
			}
			else
			{
				if (health == CharaDef.HealthMask.BLEED)
				{
					return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask.BLEED);
				}
				if (health == CharaDef.HealthMask.UNHEAL)
				{
					return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask.UNHEAL);
				}
			}
		}
		else if (health <= CharaDef.HealthMask.SILENCE)
		{
			if (health <= CharaDef.HealthMask.BUFF_INVALID)
			{
				if (health == CharaDef.HealthMask.MP_NOCOUNT)
				{
					return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask.MP_NOCOUNT);
				}
				if (health == CharaDef.HealthMask.BUFF_INVALID)
				{
					return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask2.BUFF_INVALID);
				}
			}
			else
			{
				if (health == CharaDef.HealthMask.PARALYSIS)
				{
					return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask2.PARALYSIS);
				}
				if (health == CharaDef.HealthMask.SILENCE)
				{
					return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask2.SILENCE);
				}
			}
		}
		else if (health <= CharaDef.HealthMask.BURNED)
		{
			if (health == CharaDef.HealthMask.INAUDIBLE)
			{
				return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask2.INAUDIBLE);
			}
			if (health == CharaDef.HealthMask.BURNED)
			{
				return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask2.BURNED);
			}
		}
		else
		{
			if (health == CharaDef.HealthMask.FOCUS)
			{
				return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask2.FOCUS);
			}
			if (health == CharaDef.HealthMask.IMPATIENCE)
			{
				return CharaDef.AbnormalMask(CharaDef.ActionAbnormalMask2.IMPATIENCE);
			}
		}
		return 0L;
	}

	public static string GetAbilityTraitsName(CharaDef.AbilityTraits type)
	{
		if (type <= CharaDef.AbilityTraits.desert)
		{
			if (type <= CharaDef.AbilityTraits.jungle)
			{
				if (type == CharaDef.AbilityTraits.savanna)
				{
					return PrjUtil.MakeMessage("サバンナ");
				}
				if (type == CharaDef.AbilityTraits.jungle)
				{
					return PrjUtil.MakeMessage("ジャングル");
				}
			}
			else
			{
				if (type == CharaDef.AbilityTraits.cave)
				{
					return PrjUtil.MakeMessage("どうくつ");
				}
				if (type == CharaDef.AbilityTraits.desert)
				{
					return PrjUtil.MakeMessage("さばく");
				}
			}
		}
		else if (type <= CharaDef.AbilityTraits.cold_district)
		{
			if (type == CharaDef.AbilityTraits.waterside)
			{
				return PrjUtil.MakeMessage("みずべ");
			}
			if (type == CharaDef.AbilityTraits.cold_district)
			{
				return PrjUtil.MakeMessage("寒冷地");
			}
		}
		else
		{
			if (type == CharaDef.AbilityTraits.mountain)
			{
				return PrjUtil.MakeMessage("やま");
			}
			if (type == CharaDef.AbilityTraits.city)
			{
				return PrjUtil.MakeMessage("まち");
			}
			if (type == CharaDef.AbilityTraits.stadium)
			{
				return PrjUtil.MakeMessage("運動場");
			}
		}
		return PrjUtil.MakeMessage("-");
	}

	public static string GetRedListInformationSource(int redListId)
	{
		if (!CharaDef.IsExistRedListId(redListId))
		{
			return "";
		}
		if (redListId <= 10)
		{
			return "IUCNによる保全状況(Ver.3.1)";
		}
		if (redListId <= 110)
		{
			return "環境省RDBによる保全状況";
		}
		if (redListId <= 210)
		{
			return "環境省レッドリスト2019";
		}
		if (redListId <= 310)
		{
			return "環境省レッドリスト2020";
		}
		if (redListId <= 410)
		{
			return "";
		}
		int uma_ID = CharaDef.UMA_ID;
		return "";
	}

	public static List<string> GetIconArrangement(int redListId)
	{
		List<string> list = new List<string>();
		if (CharaDef.IsExistRedListId(redListId))
		{
			if (redListId == CharaDef.UMA_ID)
			{
				for (int i = 0; i < 7; i++)
				{
					list.Add("？");
				}
			}
			else
			{
				int num = redListId / 100;
				int num2 = num * 100 + 1;
				int num3 = num * 100 + 7;
				for (int j = num2; j <= num3; j++)
				{
					string[] array = CharaDef.RedListInformationMap[j].Wording.Split('：', StringSplitOptions.None);
					if (array.Length >= 2)
					{
						list.Add(array[0]);
					}
				}
			}
		}
		return list;
	}

	public static CharaDef.RedList.Level GetRedListLevel(int redListId)
	{
		CharaDef.RedList.Level level = CharaDef.RedList.Level.INVALID;
		if (CharaDef.IsExistRedListId(redListId))
		{
			if (redListId == CharaDef.UMA_ID)
			{
			}
			level = (CharaDef.RedList.Level)(redListId % 100 % Enum.GetValues(typeof(CharaDef.RedList.Level)).Length);
		}
		return level;
	}

	public static string GetRedListWording(int redListId)
	{
		if (CharaDef.IsExistRedListId(redListId))
		{
			return CharaDef.RedListInformationMap[redListId].Wording;
		}
		return "";
	}

	public static bool IsExistRedListId(int redListId)
	{
		return CharaDef.RedListInformationMap.ContainsKey(redListId);
	}

	public static long AbnormalMask(CharaDef.ActionAbnormalMask msk)
	{
		return (long)msk;
	}

	public static long AbnormalMask(CharaDef.ActionAbnormalMask2 msk2)
	{
		return (long)msk2 << 32;
	}

	public static long AbnormalMask(CharaDef.ActionAbnormalMask msk, CharaDef.ActionAbnormalMask2 msk2)
	{
		return ((long)msk & (long)((ulong)(-1))) | ((long)msk2 << 32);
	}

	public const int INVALID_CHARA_ID = 0;

	private static readonly int UMA_ID = 510;

	private static readonly Dictionary<int, CharaDef.RedList> RedListInformationMap = new Dictionary<int, CharaDef.RedList>
	{
		{
			1,
			new CharaDef.RedList
			{
				Wording = "LC：軽度懸念"
			}
		},
		{
			2,
			new CharaDef.RedList
			{
				Wording = "NT：準絶滅危惧"
			}
		},
		{
			3,
			new CharaDef.RedList
			{
				Wording = "VU：危急"
			}
		},
		{
			4,
			new CharaDef.RedList
			{
				Wording = "EN：絶滅危惧"
			}
		},
		{
			5,
			new CharaDef.RedList
			{
				Wording = "CR：絶滅寸前"
			}
		},
		{
			6,
			new CharaDef.RedList
			{
				Wording = "EW：野生絶滅"
			}
		},
		{
			7,
			new CharaDef.RedList
			{
				Wording = "EX：絶滅"
			}
		},
		{
			8,
			new CharaDef.RedList
			{
				Wording = "DD：情報不足"
			}
		},
		{
			9,
			new CharaDef.RedList
			{
				Wording = "NE：未評価"
			}
		},
		{
			10,
			new CharaDef.RedList
			{
				Wording = "NoData"
			}
		},
		{
			101,
			new CharaDef.RedList
			{
				Wording = "LP：絶滅のおそれのある地域個体群"
			}
		},
		{
			102,
			new CharaDef.RedList
			{
				Wording = "NT：準絶滅危惧"
			}
		},
		{
			103,
			new CharaDef.RedList
			{
				Wording = "VU：絶滅危惧II類"
			}
		},
		{
			104,
			new CharaDef.RedList
			{
				Wording = "EN：絶滅危惧IB類"
			}
		},
		{
			105,
			new CharaDef.RedList
			{
				Wording = "CR：絶滅危惧IA類"
			}
		},
		{
			106,
			new CharaDef.RedList
			{
				Wording = "EW：野生絶滅"
			}
		},
		{
			107,
			new CharaDef.RedList
			{
				Wording = "EX：絶滅"
			}
		},
		{
			108,
			new CharaDef.RedList
			{
				Wording = "DD：情報不足"
			}
		},
		{
			109,
			new CharaDef.RedList
			{
				Wording = "NE：未評価"
			}
		},
		{
			110,
			new CharaDef.RedList
			{
				Wording = "NoData"
			}
		},
		{
			201,
			new CharaDef.RedList
			{
				Wording = "LP：絶滅のおそれのある地域個体群"
			}
		},
		{
			202,
			new CharaDef.RedList
			{
				Wording = "NT：準絶滅危惧"
			}
		},
		{
			203,
			new CharaDef.RedList
			{
				Wording = "VU：絶滅危惧II類"
			}
		},
		{
			204,
			new CharaDef.RedList
			{
				Wording = "EN：絶滅危惧IB類"
			}
		},
		{
			205,
			new CharaDef.RedList
			{
				Wording = "CR：絶滅危惧IA類"
			}
		},
		{
			206,
			new CharaDef.RedList
			{
				Wording = "EW：野生絶滅"
			}
		},
		{
			207,
			new CharaDef.RedList
			{
				Wording = "EX：絶滅"
			}
		},
		{
			208,
			new CharaDef.RedList
			{
				Wording = "DD：情報不足"
			}
		},
		{
			209,
			new CharaDef.RedList
			{
				Wording = "NE：未評価"
			}
		},
		{
			210,
			new CharaDef.RedList
			{
				Wording = "NoData"
			}
		},
		{
			301,
			new CharaDef.RedList
			{
				Wording = "LP：絶滅のおそれのある地域個体群"
			}
		},
		{
			302,
			new CharaDef.RedList
			{
				Wording = "NT：準絶滅危惧"
			}
		},
		{
			303,
			new CharaDef.RedList
			{
				Wording = "VU：絶滅危惧II類"
			}
		},
		{
			304,
			new CharaDef.RedList
			{
				Wording = "EN：絶滅危惧IB類"
			}
		},
		{
			305,
			new CharaDef.RedList
			{
				Wording = "CR：絶滅危惧IA類"
			}
		},
		{
			306,
			new CharaDef.RedList
			{
				Wording = "EW：野生絶滅"
			}
		},
		{
			307,
			new CharaDef.RedList
			{
				Wording = "EX：絶滅"
			}
		},
		{
			308,
			new CharaDef.RedList
			{
				Wording = "DD：情報不足"
			}
		},
		{
			309,
			new CharaDef.RedList
			{
				Wording = "NE：未評価"
			}
		},
		{
			310,
			new CharaDef.RedList
			{
				Wording = "NoData"
			}
		},
		{
			401,
			new CharaDef.RedList
			{
				Wording = "LC：軽度懸念"
			}
		},
		{
			402,
			new CharaDef.RedList
			{
				Wording = "NT：準絶滅危惧"
			}
		},
		{
			403,
			new CharaDef.RedList
			{
				Wording = "VU：危急"
			}
		},
		{
			404,
			new CharaDef.RedList
			{
				Wording = "EN：絶滅危惧"
			}
		},
		{
			405,
			new CharaDef.RedList
			{
				Wording = "CR：絶滅寸前"
			}
		},
		{
			406,
			new CharaDef.RedList
			{
				Wording = "EW：野生絶滅"
			}
		},
		{
			407,
			new CharaDef.RedList
			{
				Wording = "EX：絶滅"
			}
		},
		{
			408,
			new CharaDef.RedList
			{
				Wording = "DD：情報不足"
			}
		},
		{
			409,
			new CharaDef.RedList
			{
				Wording = "NE：未評価"
			}
		},
		{
			410,
			new CharaDef.RedList
			{
				Wording = "NoData"
			}
		},
		{
			CharaDef.UMA_ID,
			new CharaDef.RedList
			{
				Wording = "？？？：UMA"
			}
		}
	};

	public enum Type
	{
		INVALID,
		FRIENDS,
		ENEMY,
		BOSS,
		PARTS
	}

	public enum Team
	{
		MYSELF,
		ENEMY
	}

	public enum PartsType
	{
		DEFAULT,
		SUBUNIT
	}

	public enum AttributeType
	{
		ALL,
		RED,
		GREEN,
		BLUE,
		PINK,
		LIME,
		AQUA
	}

	[Flags]
	public enum AttributeMask
	{
		RED = 1,
		GREEN = 2,
		BLUE = 4,
		PINK = 8,
		LIME = 16,
		AQUA = 32
	}

	[Flags]
	public enum EnemyMask
	{
		BOSS = 1,
		MOB = 2,
		FRIENDS = 4,
		PARTS = 8
	}

	[Flags]
	public enum HealthMask
	{
		HEALTH = 1,
		POISON = 2,
		STUN = 4,
		SLEEP = 8,
		SEAL = 16,
		ICE = 32,
		BLEED = 64,
		UNHEAL = 128,
		MP_NOCOUNT = 256,
		BUFF_INVALID = 512,
		PARALYSIS = 1024,
		SILENCE = 2048,
		INAUDIBLE = 4096,
		BURNED = 8192,
		FOCUS = 16384,
		IMPATIENCE = 32768
	}

	public enum HealthMaskType
	{
		DEFAULT,
		SELF
	}

	[Flags]
	public enum AbilityTraits
	{
		without = 1,
		savanna = 2,
		jungle = 4,
		cave = 8,
		desert = 16,
		waterside = 32,
		cold_district = 64,
		mountain = 128,
		city = 256,
		stadium = 512
	}

	[Flags]
	public enum AbilityTraits2
	{
		without = 1,
		night = 2
	}

	public class RedList
	{
		public string Wording { get; set; }

		public enum Level
		{
			INVALID,
			L1,
			L2,
			L3,
			L4,
			L5,
			L6,
			L7,
			DD,
			NE,
			NOTHING
		}
	}

	public enum OrderCardType
	{
		INVALID,
		BEAT,
		ACTION,
		TRY,
		SPECIAL
	}

	public enum ActionTargetType
	{
		INVALID,
		DEFAULT,
		ENEMY_SIDE_ALL,
		MY_SIDE_ALL,
		ENEMY_FRONT_ALL,
		ENEMY_SIDE_RANDOM,
		ENEMY_FRONT_RANDOM,
		SELF,
		INVALID2,
		MY_FRONT_ALL,
		MY_SIDE_RANDOM,
		MY_FRONT_RANDOM,
		ENEMY_LOWER_HP,
		MY_LOWER_HP,
		ENEMY_UPPER_HP,
		MY_UPPER_HP,
		WITHOUT_SELF,
		WITHOUT_SELF_ALL,
		ENEMY_SIDE_CAPITATION,
		ENEMY_FRONT_CAPITATION
	}

	public enum EffectDispType
	{
		INVALID,
		EACH,
		CENTER_DIRECT,
		GROUP,
		HEAD_TO_TARGET
	}

	public enum ActionDamageType
	{
		INVALID,
		NEAR,
		FAR,
		CENTER
	}

	public enum ActionBuffType
	{
		INVALID,
		POISON,
		STUN,
		ADD_DAMAGE,
		RCV_DAMAGE,
		INVALID_atk,
		INVALID_def,
		MP,
		AVOID,
		HATE,
		BEAT_DAMAGE,
		ACTION_DAMAGE,
		TRY_DAMAGE,
		TURN_MP,
		HEAL,
		TURN_HEAL,
		ABSORB,
		PLASM_DURATION,
		SLEEP,
		SEAL,
		HIT_RATE,
		RECOVER,
		RESIST,
		RESURRECT,
		SP_ADD_DAMAGE,
		SP_HIT_RATE,
		SP_RCV_DAMAGE,
		SP_AVOID,
		ICE,
		BLEED,
		UNHEAL,
		REFLECT,
		ANTI_REFLECT,
		COVER_LOWER,
		COVER_RANDOM,
		MP_NOCOUNT,
		RCV_ALLDAMAGE,
		HATE_HALF,
		PER_DAMAGE,
		BUFF_INVALID,
		DEBUFF_INVALID,
		TICKLING,
		MP_DOUBLE,
		PARALYSIS,
		SILENCE,
		HEAL_AMOUNT,
		MP_AMOUNT,
		INAUDIBLE,
		BURNED,
		FOCUS,
		SCHEDULED,
		ADD_OKAWARI,
		UPPER_OKAWARI,
		IMPATIENCE,
		ADD_PLASM,
		ATTACK
	}

	public enum ActionHealType
	{
		INVALID
	}

	public enum ArtsBlowType
	{
		INVALID,
		MIN,
		LARGE
	}

	public enum ActionTimingType
	{
		INVALID,
		TOP,
		END
	}

	public enum ActionCameraType
	{
		INVALID,
		POINT_FRONT,
		POINT_BACK,
		GROUP_FOCUS,
		BULLET_FOCUS,
		OVERLOOK,
		OVERLOOK_GROUP,
		FRIEND_GROUP_FOCUS,
		ALL_GROUP_FOCUS,
		SIMPLE
	}

	public enum MonitorQuakeType
	{
		INVALID,
		INCREMENT,
		DECREMENT
	}

	public enum TargetNodeName
	{
		root,
		pelvis,
		j_head,
		j_mouth,
		j_wrist_r,
		j_wrist_l,
		j_chest,
		j_toe_r,
		j_toe_l,
		j_weapon_a,
		j_weapon_b,
		j_special
	}

	public enum EnemyActionPattern
	{
		INVALID,
		LOTTERY,
		ROTATION
	}

	[Flags]
	public enum ActionAbnormalMask
	{
		POISON = 1,
		STUN = 2,
		SLEEP = 4,
		SEAL = 8,
		ICE = 16,
		BLEED = 32,
		UNHEAL = 64,
		ADD_DAMAGE = 128,
		BEAT_DAMAGE = 256,
		ACTION_DAMAGE = 512,
		TRY_DAMAGE = 1024,
		SP_ADD_DAMAGE = 2048,
		RCV_DAMAGE = 4096,
		SP_RCV_DAMAGE = 8192,
		TURN_MP = 16384,
		AVOID = 32768,
		SP_AVOID = 65536,
		HIT_RATE = 131072,
		SP_HIT_RATE = 262144,
		HATE = 524288,
		TURN_HEAL = 1048576,
		ABSORB = 2097152,
		PLASM_DURATION = 4194304,
		REFLECT = 8388608,
		ANTI_REFLECT = 16777216,
		COVER = 33554432,
		MP_NOCOUNT = 67108864,
		RCV_ALLDAMAGE = 134217728,
		HATE_HALF = 268435456,
		HEAL_AMOUNT = 536870912,
		MP_AMOUNT = 1073741824
	}

	[Flags]
	public enum ActionAbnormalMask2
	{
		PER_DAMAGE = 1,
		BUFF_INVALID = 2,
		DEBUFF_INVALID = 4,
		TICKLING = 8,
		MP_DOUBLE = 16,
		PARALYSIS = 32,
		SILENCE = 64,
		INAUDIBLE = 128,
		BURNED = 256,
		FOCUS = 512,
		SCHEDULED = 1024,
		IMPATIENCE = 2048,
		ADD_PLASM = 4096
	}

	public enum ConditionType
	{
		UPPER,
		LOWER,
		EQUAL
	}

	public enum AiType
	{
		STUPID,
		CLEVER
	}
}
