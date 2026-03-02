using System;
using System.Collections.Generic;

// Token: 0x02000055 RID: 85
public class CharaDef
{
	// Token: 0x06000255 RID: 597 RVA: 0x00014120 File Offset: 0x00012320
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

	// Token: 0x06000256 RID: 598 RVA: 0x00014180 File Offset: 0x00012380
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

	// Token: 0x06000257 RID: 599 RVA: 0x000141C6 File Offset: 0x000123C6
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

	// Token: 0x06000258 RID: 600 RVA: 0x000141DC File Offset: 0x000123DC
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

	// Token: 0x06000259 RID: 601 RVA: 0x00014230 File Offset: 0x00012430
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

	// Token: 0x0600025A RID: 602 RVA: 0x00014394 File Offset: 0x00012594
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

	// Token: 0x0600025B RID: 603 RVA: 0x0001445C File Offset: 0x0001265C
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

	// Token: 0x0600025C RID: 604 RVA: 0x000144C4 File Offset: 0x000126C4
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

	// Token: 0x0600025D RID: 605 RVA: 0x00014554 File Offset: 0x00012754
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

	// Token: 0x0600025E RID: 606 RVA: 0x00014591 File Offset: 0x00012791
	public static string GetRedListWording(int redListId)
	{
		if (CharaDef.IsExistRedListId(redListId))
		{
			return CharaDef.RedListInformationMap[redListId].Wording;
		}
		return "";
	}

	// Token: 0x0600025F RID: 607 RVA: 0x000145B1 File Offset: 0x000127B1
	public static bool IsExistRedListId(int redListId)
	{
		return CharaDef.RedListInformationMap.ContainsKey(redListId);
	}

	// Token: 0x06000260 RID: 608 RVA: 0x000145C0 File Offset: 0x000127C0
	public static long AbnormalMask(CharaDef.ActionAbnormalMask msk)
	{
		return (long)msk;
	}

	// Token: 0x06000261 RID: 609 RVA: 0x000145C4 File Offset: 0x000127C4
	public static long AbnormalMask(CharaDef.ActionAbnormalMask2 msk2)
	{
		return (long)msk2 << 32;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x000145CB File Offset: 0x000127CB
	public static long AbnormalMask(CharaDef.ActionAbnormalMask msk, CharaDef.ActionAbnormalMask2 msk2)
	{
		return ((long)msk & (long)((ulong)(-1))) | ((long)msk2 << 32);
	}

	// Token: 0x040002BA RID: 698
	public const int INVALID_CHARA_ID = 0;

	// Token: 0x040002BB RID: 699
	private static readonly int UMA_ID = 510;

	// Token: 0x040002BC RID: 700
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

	// Token: 0x02000602 RID: 1538
	public enum Type
	{
		// Token: 0x04002C7A RID: 11386
		INVALID,
		// Token: 0x04002C7B RID: 11387
		FRIENDS,
		// Token: 0x04002C7C RID: 11388
		ENEMY,
		// Token: 0x04002C7D RID: 11389
		BOSS,
		// Token: 0x04002C7E RID: 11390
		PARTS
	}

	// Token: 0x02000603 RID: 1539
	public enum Team
	{
		// Token: 0x04002C80 RID: 11392
		MYSELF,
		// Token: 0x04002C81 RID: 11393
		ENEMY
	}

	// Token: 0x02000604 RID: 1540
	public enum PartsType
	{
		// Token: 0x04002C83 RID: 11395
		DEFAULT,
		// Token: 0x04002C84 RID: 11396
		SUBUNIT
	}

	// Token: 0x02000605 RID: 1541
	public enum AttributeType
	{
		// Token: 0x04002C86 RID: 11398
		ALL,
		// Token: 0x04002C87 RID: 11399
		RED,
		// Token: 0x04002C88 RID: 11400
		GREEN,
		// Token: 0x04002C89 RID: 11401
		BLUE,
		// Token: 0x04002C8A RID: 11402
		PINK,
		// Token: 0x04002C8B RID: 11403
		LIME,
		// Token: 0x04002C8C RID: 11404
		AQUA
	}

	// Token: 0x02000606 RID: 1542
	[Flags]
	public enum AttributeMask
	{
		// Token: 0x04002C8E RID: 11406
		RED = 1,
		// Token: 0x04002C8F RID: 11407
		GREEN = 2,
		// Token: 0x04002C90 RID: 11408
		BLUE = 4,
		// Token: 0x04002C91 RID: 11409
		PINK = 8,
		// Token: 0x04002C92 RID: 11410
		LIME = 16,
		// Token: 0x04002C93 RID: 11411
		AQUA = 32
	}

	// Token: 0x02000607 RID: 1543
	[Flags]
	public enum EnemyMask
	{
		// Token: 0x04002C95 RID: 11413
		BOSS = 1,
		// Token: 0x04002C96 RID: 11414
		MOB = 2,
		// Token: 0x04002C97 RID: 11415
		FRIENDS = 4,
		// Token: 0x04002C98 RID: 11416
		PARTS = 8
	}

	// Token: 0x02000608 RID: 1544
	[Flags]
	public enum HealthMask
	{
		// Token: 0x04002C9A RID: 11418
		HEALTH = 1,
		// Token: 0x04002C9B RID: 11419
		POISON = 2,
		// Token: 0x04002C9C RID: 11420
		STUN = 4,
		// Token: 0x04002C9D RID: 11421
		SLEEP = 8,
		// Token: 0x04002C9E RID: 11422
		SEAL = 16,
		// Token: 0x04002C9F RID: 11423
		ICE = 32,
		// Token: 0x04002CA0 RID: 11424
		BLEED = 64,
		// Token: 0x04002CA1 RID: 11425
		UNHEAL = 128,
		// Token: 0x04002CA2 RID: 11426
		MP_NOCOUNT = 256,
		// Token: 0x04002CA3 RID: 11427
		BUFF_INVALID = 512,
		// Token: 0x04002CA4 RID: 11428
		PARALYSIS = 1024,
		// Token: 0x04002CA5 RID: 11429
		SILENCE = 2048,
		// Token: 0x04002CA6 RID: 11430
		INAUDIBLE = 4096,
		// Token: 0x04002CA7 RID: 11431
		BURNED = 8192,
		// Token: 0x04002CA8 RID: 11432
		FOCUS = 16384,
		// Token: 0x04002CA9 RID: 11433
		IMPATIENCE = 32768
	}

	// Token: 0x02000609 RID: 1545
	public enum HealthMaskType
	{
		// Token: 0x04002CAB RID: 11435
		DEFAULT,
		// Token: 0x04002CAC RID: 11436
		SELF
	}

	// Token: 0x0200060A RID: 1546
	[Flags]
	public enum AbilityTraits
	{
		// Token: 0x04002CAE RID: 11438
		without = 1,
		// Token: 0x04002CAF RID: 11439
		savanna = 2,
		// Token: 0x04002CB0 RID: 11440
		jungle = 4,
		// Token: 0x04002CB1 RID: 11441
		cave = 8,
		// Token: 0x04002CB2 RID: 11442
		desert = 16,
		// Token: 0x04002CB3 RID: 11443
		waterside = 32,
		// Token: 0x04002CB4 RID: 11444
		cold_district = 64,
		// Token: 0x04002CB5 RID: 11445
		mountain = 128,
		// Token: 0x04002CB6 RID: 11446
		city = 256,
		// Token: 0x04002CB7 RID: 11447
		stadium = 512
	}

	// Token: 0x0200060B RID: 1547
	[Flags]
	public enum AbilityTraits2
	{
		// Token: 0x04002CB9 RID: 11449
		without = 1,
		// Token: 0x04002CBA RID: 11450
		night = 2
	}

	// Token: 0x0200060C RID: 1548
	public class RedList
	{
		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x001B9BDB File Offset: 0x001B7DDB
		// (set) Token: 0x06002FEA RID: 12266 RVA: 0x001B9BE3 File Offset: 0x001B7DE3
		public string Wording { get; set; }

		// Token: 0x0200110E RID: 4366
		public enum Level
		{
			// Token: 0x04005DDC RID: 24028
			INVALID,
			// Token: 0x04005DDD RID: 24029
			L1,
			// Token: 0x04005DDE RID: 24030
			L2,
			// Token: 0x04005DDF RID: 24031
			L3,
			// Token: 0x04005DE0 RID: 24032
			L4,
			// Token: 0x04005DE1 RID: 24033
			L5,
			// Token: 0x04005DE2 RID: 24034
			L6,
			// Token: 0x04005DE3 RID: 24035
			L7,
			// Token: 0x04005DE4 RID: 24036
			DD,
			// Token: 0x04005DE5 RID: 24037
			NE,
			// Token: 0x04005DE6 RID: 24038
			NOTHING
		}
	}

	// Token: 0x0200060D RID: 1549
	public enum OrderCardType
	{
		// Token: 0x04002CBD RID: 11453
		INVALID,
		// Token: 0x04002CBE RID: 11454
		BEAT,
		// Token: 0x04002CBF RID: 11455
		ACTION,
		// Token: 0x04002CC0 RID: 11456
		TRY,
		// Token: 0x04002CC1 RID: 11457
		SPECIAL
	}

	// Token: 0x0200060E RID: 1550
	public enum ActionTargetType
	{
		// Token: 0x04002CC3 RID: 11459
		INVALID,
		// Token: 0x04002CC4 RID: 11460
		DEFAULT,
		// Token: 0x04002CC5 RID: 11461
		ENEMY_SIDE_ALL,
		// Token: 0x04002CC6 RID: 11462
		MY_SIDE_ALL,
		// Token: 0x04002CC7 RID: 11463
		ENEMY_FRONT_ALL,
		// Token: 0x04002CC8 RID: 11464
		ENEMY_SIDE_RANDOM,
		// Token: 0x04002CC9 RID: 11465
		ENEMY_FRONT_RANDOM,
		// Token: 0x04002CCA RID: 11466
		SELF,
		// Token: 0x04002CCB RID: 11467
		INVALID2,
		// Token: 0x04002CCC RID: 11468
		MY_FRONT_ALL,
		// Token: 0x04002CCD RID: 11469
		MY_SIDE_RANDOM,
		// Token: 0x04002CCE RID: 11470
		MY_FRONT_RANDOM,
		// Token: 0x04002CCF RID: 11471
		ENEMY_LOWER_HP,
		// Token: 0x04002CD0 RID: 11472
		MY_LOWER_HP,
		// Token: 0x04002CD1 RID: 11473
		ENEMY_UPPER_HP,
		// Token: 0x04002CD2 RID: 11474
		MY_UPPER_HP,
		// Token: 0x04002CD3 RID: 11475
		WITHOUT_SELF,
		// Token: 0x04002CD4 RID: 11476
		WITHOUT_SELF_ALL,
		// Token: 0x04002CD5 RID: 11477
		ENEMY_SIDE_CAPITATION,
		// Token: 0x04002CD6 RID: 11478
		ENEMY_FRONT_CAPITATION
	}

	// Token: 0x0200060F RID: 1551
	public enum EffectDispType
	{
		// Token: 0x04002CD8 RID: 11480
		INVALID,
		// Token: 0x04002CD9 RID: 11481
		EACH,
		// Token: 0x04002CDA RID: 11482
		CENTER_DIRECT,
		// Token: 0x04002CDB RID: 11483
		GROUP,
		// Token: 0x04002CDC RID: 11484
		HEAD_TO_TARGET
	}

	// Token: 0x02000610 RID: 1552
	public enum ActionDamageType
	{
		// Token: 0x04002CDE RID: 11486
		INVALID,
		// Token: 0x04002CDF RID: 11487
		NEAR,
		// Token: 0x04002CE0 RID: 11488
		FAR,
		// Token: 0x04002CE1 RID: 11489
		CENTER
	}

	// Token: 0x02000611 RID: 1553
	public enum ActionBuffType
	{
		// Token: 0x04002CE3 RID: 11491
		INVALID,
		// Token: 0x04002CE4 RID: 11492
		POISON,
		// Token: 0x04002CE5 RID: 11493
		STUN,
		// Token: 0x04002CE6 RID: 11494
		ADD_DAMAGE,
		// Token: 0x04002CE7 RID: 11495
		RCV_DAMAGE,
		// Token: 0x04002CE8 RID: 11496
		INVALID_atk,
		// Token: 0x04002CE9 RID: 11497
		INVALID_def,
		// Token: 0x04002CEA RID: 11498
		MP,
		// Token: 0x04002CEB RID: 11499
		AVOID,
		// Token: 0x04002CEC RID: 11500
		HATE,
		// Token: 0x04002CED RID: 11501
		BEAT_DAMAGE,
		// Token: 0x04002CEE RID: 11502
		ACTION_DAMAGE,
		// Token: 0x04002CEF RID: 11503
		TRY_DAMAGE,
		// Token: 0x04002CF0 RID: 11504
		TURN_MP,
		// Token: 0x04002CF1 RID: 11505
		HEAL,
		// Token: 0x04002CF2 RID: 11506
		TURN_HEAL,
		// Token: 0x04002CF3 RID: 11507
		ABSORB,
		// Token: 0x04002CF4 RID: 11508
		PLASM_DURATION,
		// Token: 0x04002CF5 RID: 11509
		SLEEP,
		// Token: 0x04002CF6 RID: 11510
		SEAL,
		// Token: 0x04002CF7 RID: 11511
		HIT_RATE,
		// Token: 0x04002CF8 RID: 11512
		RECOVER,
		// Token: 0x04002CF9 RID: 11513
		RESIST,
		// Token: 0x04002CFA RID: 11514
		RESURRECT,
		// Token: 0x04002CFB RID: 11515
		SP_ADD_DAMAGE,
		// Token: 0x04002CFC RID: 11516
		SP_HIT_RATE,
		// Token: 0x04002CFD RID: 11517
		SP_RCV_DAMAGE,
		// Token: 0x04002CFE RID: 11518
		SP_AVOID,
		// Token: 0x04002CFF RID: 11519
		ICE,
		// Token: 0x04002D00 RID: 11520
		BLEED,
		// Token: 0x04002D01 RID: 11521
		UNHEAL,
		// Token: 0x04002D02 RID: 11522
		REFLECT,
		// Token: 0x04002D03 RID: 11523
		ANTI_REFLECT,
		// Token: 0x04002D04 RID: 11524
		COVER_LOWER,
		// Token: 0x04002D05 RID: 11525
		COVER_RANDOM,
		// Token: 0x04002D06 RID: 11526
		MP_NOCOUNT,
		// Token: 0x04002D07 RID: 11527
		RCV_ALLDAMAGE,
		// Token: 0x04002D08 RID: 11528
		HATE_HALF,
		// Token: 0x04002D09 RID: 11529
		PER_DAMAGE,
		// Token: 0x04002D0A RID: 11530
		BUFF_INVALID,
		// Token: 0x04002D0B RID: 11531
		DEBUFF_INVALID,
		// Token: 0x04002D0C RID: 11532
		TICKLING,
		// Token: 0x04002D0D RID: 11533
		MP_DOUBLE,
		// Token: 0x04002D0E RID: 11534
		PARALYSIS,
		// Token: 0x04002D0F RID: 11535
		SILENCE,
		// Token: 0x04002D10 RID: 11536
		HEAL_AMOUNT,
		// Token: 0x04002D11 RID: 11537
		MP_AMOUNT,
		// Token: 0x04002D12 RID: 11538
		INAUDIBLE,
		// Token: 0x04002D13 RID: 11539
		BURNED,
		// Token: 0x04002D14 RID: 11540
		FOCUS,
		// Token: 0x04002D15 RID: 11541
		SCHEDULED,
		// Token: 0x04002D16 RID: 11542
		ADD_OKAWARI,
		// Token: 0x04002D17 RID: 11543
		UPPER_OKAWARI,
		// Token: 0x04002D18 RID: 11544
		IMPATIENCE,
		// Token: 0x04002D19 RID: 11545
		ADD_PLASM,
		// Token: 0x04002D1A RID: 11546
		ATTACK
	}

	// Token: 0x02000612 RID: 1554
	public enum ActionHealType
	{
		// Token: 0x04002D1C RID: 11548
		INVALID
	}

	// Token: 0x02000613 RID: 1555
	public enum ArtsBlowType
	{
		// Token: 0x04002D1E RID: 11550
		INVALID,
		// Token: 0x04002D1F RID: 11551
		MIN,
		// Token: 0x04002D20 RID: 11552
		LARGE
	}

	// Token: 0x02000614 RID: 1556
	public enum ActionTimingType
	{
		// Token: 0x04002D22 RID: 11554
		INVALID,
		// Token: 0x04002D23 RID: 11555
		TOP,
		// Token: 0x04002D24 RID: 11556
		END
	}

	// Token: 0x02000615 RID: 1557
	public enum ActionCameraType
	{
		// Token: 0x04002D26 RID: 11558
		INVALID,
		// Token: 0x04002D27 RID: 11559
		POINT_FRONT,
		// Token: 0x04002D28 RID: 11560
		POINT_BACK,
		// Token: 0x04002D29 RID: 11561
		GROUP_FOCUS,
		// Token: 0x04002D2A RID: 11562
		BULLET_FOCUS,
		// Token: 0x04002D2B RID: 11563
		OVERLOOK,
		// Token: 0x04002D2C RID: 11564
		OVERLOOK_GROUP,
		// Token: 0x04002D2D RID: 11565
		FRIEND_GROUP_FOCUS,
		// Token: 0x04002D2E RID: 11566
		ALL_GROUP_FOCUS,
		// Token: 0x04002D2F RID: 11567
		SIMPLE
	}

	// Token: 0x02000616 RID: 1558
	public enum MonitorQuakeType
	{
		// Token: 0x04002D31 RID: 11569
		INVALID,
		// Token: 0x04002D32 RID: 11570
		INCREMENT,
		// Token: 0x04002D33 RID: 11571
		DECREMENT
	}

	// Token: 0x02000617 RID: 1559
	public enum TargetNodeName
	{
		// Token: 0x04002D35 RID: 11573
		root,
		// Token: 0x04002D36 RID: 11574
		pelvis,
		// Token: 0x04002D37 RID: 11575
		j_head,
		// Token: 0x04002D38 RID: 11576
		j_mouth,
		// Token: 0x04002D39 RID: 11577
		j_wrist_r,
		// Token: 0x04002D3A RID: 11578
		j_wrist_l,
		// Token: 0x04002D3B RID: 11579
		j_chest,
		// Token: 0x04002D3C RID: 11580
		j_toe_r,
		// Token: 0x04002D3D RID: 11581
		j_toe_l,
		// Token: 0x04002D3E RID: 11582
		j_weapon_a,
		// Token: 0x04002D3F RID: 11583
		j_weapon_b,
		// Token: 0x04002D40 RID: 11584
		j_special
	}

	// Token: 0x02000618 RID: 1560
	public enum EnemyActionPattern
	{
		// Token: 0x04002D42 RID: 11586
		INVALID,
		// Token: 0x04002D43 RID: 11587
		LOTTERY,
		// Token: 0x04002D44 RID: 11588
		ROTATION
	}

	// Token: 0x02000619 RID: 1561
	[Flags]
	public enum ActionAbnormalMask
	{
		// Token: 0x04002D46 RID: 11590
		POISON = 1,
		// Token: 0x04002D47 RID: 11591
		STUN = 2,
		// Token: 0x04002D48 RID: 11592
		SLEEP = 4,
		// Token: 0x04002D49 RID: 11593
		SEAL = 8,
		// Token: 0x04002D4A RID: 11594
		ICE = 16,
		// Token: 0x04002D4B RID: 11595
		BLEED = 32,
		// Token: 0x04002D4C RID: 11596
		UNHEAL = 64,
		// Token: 0x04002D4D RID: 11597
		ADD_DAMAGE = 128,
		// Token: 0x04002D4E RID: 11598
		BEAT_DAMAGE = 256,
		// Token: 0x04002D4F RID: 11599
		ACTION_DAMAGE = 512,
		// Token: 0x04002D50 RID: 11600
		TRY_DAMAGE = 1024,
		// Token: 0x04002D51 RID: 11601
		SP_ADD_DAMAGE = 2048,
		// Token: 0x04002D52 RID: 11602
		RCV_DAMAGE = 4096,
		// Token: 0x04002D53 RID: 11603
		SP_RCV_DAMAGE = 8192,
		// Token: 0x04002D54 RID: 11604
		TURN_MP = 16384,
		// Token: 0x04002D55 RID: 11605
		AVOID = 32768,
		// Token: 0x04002D56 RID: 11606
		SP_AVOID = 65536,
		// Token: 0x04002D57 RID: 11607
		HIT_RATE = 131072,
		// Token: 0x04002D58 RID: 11608
		SP_HIT_RATE = 262144,
		// Token: 0x04002D59 RID: 11609
		HATE = 524288,
		// Token: 0x04002D5A RID: 11610
		TURN_HEAL = 1048576,
		// Token: 0x04002D5B RID: 11611
		ABSORB = 2097152,
		// Token: 0x04002D5C RID: 11612
		PLASM_DURATION = 4194304,
		// Token: 0x04002D5D RID: 11613
		REFLECT = 8388608,
		// Token: 0x04002D5E RID: 11614
		ANTI_REFLECT = 16777216,
		// Token: 0x04002D5F RID: 11615
		COVER = 33554432,
		// Token: 0x04002D60 RID: 11616
		MP_NOCOUNT = 67108864,
		// Token: 0x04002D61 RID: 11617
		RCV_ALLDAMAGE = 134217728,
		// Token: 0x04002D62 RID: 11618
		HATE_HALF = 268435456,
		// Token: 0x04002D63 RID: 11619
		HEAL_AMOUNT = 536870912,
		// Token: 0x04002D64 RID: 11620
		MP_AMOUNT = 1073741824
	}

	// Token: 0x0200061A RID: 1562
	[Flags]
	public enum ActionAbnormalMask2
	{
		// Token: 0x04002D66 RID: 11622
		PER_DAMAGE = 1,
		// Token: 0x04002D67 RID: 11623
		BUFF_INVALID = 2,
		// Token: 0x04002D68 RID: 11624
		DEBUFF_INVALID = 4,
		// Token: 0x04002D69 RID: 11625
		TICKLING = 8,
		// Token: 0x04002D6A RID: 11626
		MP_DOUBLE = 16,
		// Token: 0x04002D6B RID: 11627
		PARALYSIS = 32,
		// Token: 0x04002D6C RID: 11628
		SILENCE = 64,
		// Token: 0x04002D6D RID: 11629
		INAUDIBLE = 128,
		// Token: 0x04002D6E RID: 11630
		BURNED = 256,
		// Token: 0x04002D6F RID: 11631
		FOCUS = 512,
		// Token: 0x04002D70 RID: 11632
		SCHEDULED = 1024,
		// Token: 0x04002D71 RID: 11633
		IMPATIENCE = 2048,
		// Token: 0x04002D72 RID: 11634
		ADD_PLASM = 4096
	}

	// Token: 0x0200061B RID: 1563
	public enum ConditionType
	{
		// Token: 0x04002D74 RID: 11636
		UPPER,
		// Token: 0x04002D75 RID: 11637
		LOWER,
		// Token: 0x04002D76 RID: 11638
		EQUAL
	}

	// Token: 0x0200061C RID: 1564
	public enum AiType
	{
		// Token: 0x04002D78 RID: 11640
		STUPID,
		// Token: 0x04002D79 RID: 11641
		CLEVER
	}
}
