using System;
using System.Collections.Generic;

// Token: 0x02000103 RID: 259
public class SortFilterDefine
{
	// Token: 0x1700031F RID: 799
	// (get) Token: 0x06000C81 RID: 3201 RVA: 0x0004C634 File Offset: 0x0004A834
	public static CharaDef.AbilityTraits TerrainAll
	{
		get
		{
			CharaDef.AbilityTraits abilityTraits = CharaDef.AbilityTraits.without;
			foreach (object obj in Enum.GetValues(typeof(CharaDef.AbilityTraits)))
			{
				CharaDef.AbilityTraits abilityTraits2 = (CharaDef.AbilityTraits)obj;
				abilityTraits |= abilityTraits2;
			}
			return abilityTraits;
		}
	}

	// Token: 0x040009B0 RID: 2480
	public static readonly Dictionary<SortFilterDefine.IconPlace, int> DefaultIconSizeIndexMap = new Dictionary<SortFilterDefine.IconPlace, int>
	{
		{
			SortFilterDefine.IconPlace.PhotoAll,
			0
		},
		{
			SortFilterDefine.IconPlace.PhotoGrow,
			0
		},
		{
			SortFilterDefine.IconPlace.PhotoGrowMaterial,
			0
		},
		{
			SortFilterDefine.IconPlace.PhotoSell,
			0
		},
		{
			SortFilterDefine.IconPlace.PhotoPartyEdit,
			0
		},
		{
			SortFilterDefine.IconPlace.PhotoAsistant,
			0
		},
		{
			SortFilterDefine.IconPlace.PhotoAlbum,
			0
		},
		{
			SortFilterDefine.IconPlace.AccessoryAll,
			0
		},
		{
			SortFilterDefine.IconPlace.AccessoryGrow,
			0
		},
		{
			SortFilterDefine.IconPlace.AccessoryGrowMaterial,
			0
		},
		{
			SortFilterDefine.IconPlace.AccessorySell,
			0
		},
		{
			SortFilterDefine.IconPlace.AccessoryCharaEdit,
			0
		}
	};

	// Token: 0x040009B1 RID: 2481
	public static readonly Dictionary<SortFilterDefine.RegisterType, SortFilterDefine.SortType> DefaultSortTypeMap = new Dictionary<SortFilterDefine.RegisterType, SortFilterDefine.SortType>
	{
		{
			SortFilterDefine.RegisterType.INVALID,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_ALL,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_DECK,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.PHOTO_DECK,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.PHOTO_GROW_TOP,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.PHOTO_GROW_MAIN,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.PHOTO_SELL_MAIN,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.PHOTO_ALL,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_FAVORITE,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_HELPER_LOAN,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.PHOTO_HELPER_LOAN,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_GROW_TOP,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_QUEST_TOP,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.HELP_FOLLOW,
			SortFilterDefine.SortType.LOGIN
		},
		{
			SortFilterDefine.RegisterType.HELP_FOLLOWER,
			SortFilterDefine.SortType.LOGIN
		},
		{
			SortFilterDefine.RegisterType.HOME_CLOSET,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_EVENT_GROW,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.STORY_VIEW,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_DECK_PVP,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.PHOTO_DECK_PVP,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.PICNIC_CHANGE,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_DECK_TRAINING,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.PHOTO_DECK_TRAINING,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.PHOTO_ALBUM,
			SortFilterDefine.SortType.RARITY
		},
		{
			SortFilterDefine.RegisterType.TREEHOUSE_CHANGE,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_COMMUNICATION,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.ACCESSORY_ALL,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.ACCESSORY_EQUIP,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.ACCESSORY_GROW_BASE,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.ACCESSORY_GROW_MATERIAL,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.ACCESSORY_SELL,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_SHOP_ASSISTANT,
			SortFilterDefine.SortType.LEVEL
		},
		{
			SortFilterDefine.RegisterType.CHARA_QUEST_ASSISTANT,
			SortFilterDefine.SortType.LEVEL
		}
	};

	// Token: 0x040009B2 RID: 2482
	public static readonly List<SortFilterDefine.SortType> CharaSortTypeList = new List<SortFilterDefine.SortType>
	{
		SortFilterDefine.SortType.LEVEL,
		SortFilterDefine.SortType.KEMO_STATUS,
		SortFilterDefine.SortType.HP,
		SortFilterDefine.SortType.ATK,
		SortFilterDefine.SortType.DEF,
		SortFilterDefine.SortType.RARITY,
		SortFilterDefine.SortType.NEW,
		SortFilterDefine.SortType.NAME,
		SortFilterDefine.SortType.KIZUNA,
		SortFilterDefine.SortType.PHOTO_POCKET,
		SortFilterDefine.SortType.WILD_RELEASE,
		SortFilterDefine.SortType.AVOIDANCE,
		SortFilterDefine.SortType.PLASM_POINT
	};

	// Token: 0x040009B3 RID: 2483
	public static readonly List<SortFilterDefine.SortType> PhotoSortTypeList = new List<SortFilterDefine.SortType>
	{
		SortFilterDefine.SortType.LEVEL,
		SortFilterDefine.SortType.HP,
		SortFilterDefine.SortType.ATK,
		SortFilterDefine.SortType.DEF,
		SortFilterDefine.SortType.RARITY,
		SortFilterDefine.SortType.NEW,
		SortFilterDefine.SortType.BREAKTHROUGH_LIMIT
	};

	// Token: 0x040009B4 RID: 2484
	public static readonly List<SortFilterDefine.SortType> PhotoAlbumSortTypeList = new List<SortFilterDefine.SortType>
	{
		SortFilterDefine.SortType.RARITY,
		SortFilterDefine.SortType.HP,
		SortFilterDefine.SortType.ATK,
		SortFilterDefine.SortType.DEF
	};

	// Token: 0x040009B5 RID: 2485
	public static readonly List<SortFilterDefine.SortType> HelperSortTypeList = new List<SortFilterDefine.SortType>
	{
		SortFilterDefine.SortType.LOGIN,
		SortFilterDefine.SortType.LEVEL,
		SortFilterDefine.SortType.RARITY,
		SortFilterDefine.SortType.NEW
	};

	// Token: 0x040009B6 RID: 2486
	public static readonly List<SortFilterDefine.SortType> AccessorySortTypeList = new List<SortFilterDefine.SortType>
	{
		SortFilterDefine.SortType.LEVEL,
		SortFilterDefine.SortType.HP,
		SortFilterDefine.SortType.ATK,
		SortFilterDefine.SortType.DEF,
		SortFilterDefine.SortType.AVOIDANCE,
		SortFilterDefine.SortType.BEAT_DAMAGE,
		SortFilterDefine.SortType.ACTION_DAMAGE,
		SortFilterDefine.SortType.TRY_DAMAGE,
		SortFilterDefine.SortType.RARITY,
		SortFilterDefine.SortType.NEW
	};

	// Token: 0x040009B7 RID: 2487
	public static readonly Dictionary<SortFilterDefine.SortType, string> SortTypeDispNameMap = new Dictionary<SortFilterDefine.SortType, string>
	{
		{
			SortFilterDefine.SortType.LEVEL,
			"Ｌｖ順"
		},
		{
			SortFilterDefine.SortType.HP,
			"たいりょく順"
		},
		{
			SortFilterDefine.SortType.ATK,
			"こうげき順"
		},
		{
			SortFilterDefine.SortType.DEF,
			"まもり順"
		},
		{
			SortFilterDefine.SortType.RARITY,
			"☆順"
		},
		{
			SortFilterDefine.SortType.NEW,
			"新着順"
		},
		{
			SortFilterDefine.SortType.NAME,
			"なまえ順"
		},
		{
			SortFilterDefine.SortType.BREAKTHROUGH_LIMIT,
			"限界突破"
		},
		{
			SortFilterDefine.SortType.LOGIN,
			"ログイン順"
		},
		{
			SortFilterDefine.SortType.KEMO_STATUS,
			"けもステータス"
		},
		{
			SortFilterDefine.SortType.KIZUNA,
			"なかよし順"
		},
		{
			SortFilterDefine.SortType.PHOTO_POCKET,
			"フォトポケ順"
		},
		{
			SortFilterDefine.SortType.WILD_RELEASE,
			"野生解放順"
		},
		{
			SortFilterDefine.SortType.AVOIDANCE,
			"かいひ順"
		},
		{
			SortFilterDefine.SortType.PLASM_POINT,
			"ぷらずむ順"
		},
		{
			SortFilterDefine.SortType.BEAT_DAMAGE,
			"Beat!!!"
		},
		{
			SortFilterDefine.SortType.ACTION_DAMAGE,
			"Action!"
		},
		{
			SortFilterDefine.SortType.TRY_DAMAGE,
			"Try!!"
		}
	};

	// Token: 0x040009B8 RID: 2488
	public static readonly string BTN_DISABLE_STR_NAME = "非表示";

	// Token: 0x040009B9 RID: 2489
	public static readonly List<string> sellPhotoFilterPhotoNameList = new List<string>
	{
		"☆１",
		"☆２",
		"☆３",
		"☆４",
		"",
		"",
		"その他",
		SortFilterDefine.BTN_DISABLE_STR_NAME,
		"ＬＶ１",
		"すべて"
	};

	// Token: 0x040009BA RID: 2490
	public static readonly List<string> photoFilterStarNameList = new List<string> { "☆１", "☆２", "☆３", "☆４" };

	// Token: 0x040009BB RID: 2491
	public static readonly List<string> photoFilterPhototypeNameList = new List<string>
	{
		"",
		"",
		"強化素材",
		SortFilterDefine.BTN_DISABLE_STR_NAME
	};

	// Token: 0x040009BC RID: 2492
	public static readonly List<string> photoFilterFavoriteList = new List<string> { "", "" };

	// Token: 0x040009BD RID: 2493
	public static readonly List<string> photoFilterSpecialNameList = new List<string> { "獲得量アップ", "限界突破" };

	// Token: 0x040009BE RID: 2494
	public static readonly List<string> photoFilterLimitSpecialNameList = new List<string> { "限界突破" };

	// Token: 0x040009BF RID: 2495
	public static readonly List<string> photoFilterLevelNameList = new List<string> { "ＬＶ１", "すべて" };

	// Token: 0x040009C0 RID: 2496
	public static readonly List<string> photoFilterAlbumNameList = new List<string> { "未登録", "登録済み", "\u3000\u3000登録済み\u3000\u3000\n(限界突破最大)", "\u3000\u3000登録済み\u3000\u3000\n(強化最大)" };

	// Token: 0x040009C1 RID: 2497
	public static readonly List<string> friendsFilterFavoriteList = new List<string> { "", "" };

	// Token: 0x040009C2 RID: 2498
	public static readonly List<string> friendsFilterTypeList = new List<string> { "", "", "", "", "", "" };

	// Token: 0x040009C3 RID: 2499
	public static readonly List<string> friendsFilterFriendsList = new List<string> { "", "" };

	// Token: 0x040009C4 RID: 2500
	public static readonly List<string> friendsFilterEventList = new List<string> { "", "" };

	// Token: 0x040009C5 RID: 2501
	public static readonly List<string> friendsFilterMiracleList = new List<string> { "効果対象", "効果内容" };

	// Token: 0x040009C6 RID: 2502
	public static readonly List<string> friendsFilterMiracleTargetList = new List<string> { "敵全体", "敵単体", "自身", "味方", "ランダム" };

	// Token: 0x040009C7 RID: 2503
	public static readonly List<string> friendsFilterMiracleEffectList = new List<string> { "攻撃", "与ダメージ", "被ダメージ", "MP", "体力回復", "攻撃命中率", "回避", "プラズムチャージ", "いかく/かくれみ" };

	// Token: 0x02000832 RID: 2098
	public enum SortType
	{
		// Token: 0x04003698 RID: 13976
		INVALID,
		// Token: 0x04003699 RID: 13977
		LEVEL,
		// Token: 0x0400369A RID: 13978
		HP,
		// Token: 0x0400369B RID: 13979
		ATK,
		// Token: 0x0400369C RID: 13980
		DEF,
		// Token: 0x0400369D RID: 13981
		RARITY,
		// Token: 0x0400369E RID: 13982
		NEW,
		// Token: 0x0400369F RID: 13983
		LOGIN,
		// Token: 0x040036A0 RID: 13984
		BREAKTHROUGH_LIMIT,
		// Token: 0x040036A1 RID: 13985
		USER_RANK,
		// Token: 0x040036A2 RID: 13986
		NAME,
		// Token: 0x040036A3 RID: 13987
		KEMO_STATUS,
		// Token: 0x040036A4 RID: 13988
		KIZUNA,
		// Token: 0x040036A5 RID: 13989
		PHOTO_POCKET,
		// Token: 0x040036A6 RID: 13990
		WILD_RELEASE,
		// Token: 0x040036A7 RID: 13991
		AVOIDANCE,
		// Token: 0x040036A8 RID: 13992
		PLASM_POINT,
		// Token: 0x040036A9 RID: 13993
		BEAT_DAMAGE,
		// Token: 0x040036AA RID: 13994
		ACTION_DAMAGE,
		// Token: 0x040036AB RID: 13995
		TRY_DAMAGE
	}

	// Token: 0x02000833 RID: 2099
	public enum RegisterType
	{
		// Token: 0x040036AD RID: 13997
		INVALID,
		// Token: 0x040036AE RID: 13998
		CHARA_ALL,
		// Token: 0x040036AF RID: 13999
		CHARA_DECK,
		// Token: 0x040036B0 RID: 14000
		PHOTO_DECK,
		// Token: 0x040036B1 RID: 14001
		PHOTO_GROW_TOP,
		// Token: 0x040036B2 RID: 14002
		PHOTO_GROW_MAIN,
		// Token: 0x040036B3 RID: 14003
		PHOTO_SELL_MAIN,
		// Token: 0x040036B4 RID: 14004
		PHOTO_ALL,
		// Token: 0x040036B5 RID: 14005
		CHARA_FAVORITE,
		// Token: 0x040036B6 RID: 14006
		CHARA_HELPER_LOAN,
		// Token: 0x040036B7 RID: 14007
		PHOTO_HELPER_LOAN,
		// Token: 0x040036B8 RID: 14008
		CHARA_GROW_TOP,
		// Token: 0x040036B9 RID: 14009
		CHARA_QUEST_TOP,
		// Token: 0x040036BA RID: 14010
		HELP_FOLLOW,
		// Token: 0x040036BB RID: 14011
		HELP_FOLLOWER,
		// Token: 0x040036BC RID: 14012
		HOME_CLOSET,
		// Token: 0x040036BD RID: 14013
		CHARA_EVENT_GROW,
		// Token: 0x040036BE RID: 14014
		STORY_VIEW,
		// Token: 0x040036BF RID: 14015
		CHARA_DECK_PVP,
		// Token: 0x040036C0 RID: 14016
		PHOTO_DECK_PVP,
		// Token: 0x040036C1 RID: 14017
		PICNIC_CHANGE,
		// Token: 0x040036C2 RID: 14018
		CHARA_DECK_TRAINING,
		// Token: 0x040036C3 RID: 14019
		PHOTO_DECK_TRAINING,
		// Token: 0x040036C4 RID: 14020
		PHOTO_ALBUM,
		// Token: 0x040036C5 RID: 14021
		TREEHOUSE_CHANGE,
		// Token: 0x040036C6 RID: 14022
		CHARA_COMMUNICATION,
		// Token: 0x040036C7 RID: 14023
		ACCESSORY_ALL,
		// Token: 0x040036C8 RID: 14024
		ACCESSORY_GROW_BASE,
		// Token: 0x040036C9 RID: 14025
		ACCESSORY_GROW_MATERIAL,
		// Token: 0x040036CA RID: 14026
		ACCESSORY_SELL,
		// Token: 0x040036CB RID: 14027
		ACCESSORY_EQUIP,
		// Token: 0x040036CC RID: 14028
		CHARA_SHOP_ASSISTANT,
		// Token: 0x040036CD RID: 14029
		CHARA_QUEST_ASSISTANT
	}

	// Token: 0x02000834 RID: 2100
	public enum SortFilterType
	{
		// Token: 0x040036CF RID: 14031
		INVALID,
		// Token: 0x040036D0 RID: 14032
		CHARA_SORT,
		// Token: 0x040036D1 RID: 14033
		CHARA_FILTER,
		// Token: 0x040036D2 RID: 14034
		PHOTO_SORT,
		// Token: 0x040036D3 RID: 14035
		PHOTO_FILTER,
		// Token: 0x040036D4 RID: 14036
		FOLLOW_SORT
	}

	// Token: 0x02000835 RID: 2101
	public enum CharacteristicFilterCategory
	{
		// Token: 0x040036D6 RID: 14038
		Invalid,
		// Token: 0x040036D7 RID: 14039
		Conditions,
		// Token: 0x040036D8 RID: 14040
		Target,
		// Token: 0x040036D9 RID: 14041
		Effect,
		// Token: 0x040036DA RID: 14042
		Abnormal
	}

	// Token: 0x02000836 RID: 2102
	public enum FriendsMiracleFileterCategory
	{
		// Token: 0x040036DC RID: 14044
		Target,
		// Token: 0x040036DD RID: 14045
		Effect
	}

	// Token: 0x02000837 RID: 2103
	public enum FriendsCharacteristicFilterCategory
	{
		// Token: 0x040036DF RID: 14047
		Invalid,
		// Token: 0x040036E0 RID: 14048
		Conditions,
		// Token: 0x040036E1 RID: 14049
		Target,
		// Token: 0x040036E2 RID: 14050
		Effect,
		// Token: 0x040036E3 RID: 14051
		Resist
	}

	// Token: 0x02000838 RID: 2104
	public enum PhotoFilterType
	{
		// Token: 0x040036E5 RID: 14053
		SortFilter,
		// Token: 0x040036E6 RID: 14054
		SellPhoto,
		// Token: 0x040036E7 RID: 14055
		PhotoAlbum,
		// Token: 0x040036E8 RID: 14056
		PhotoGrow
	}

	// Token: 0x02000839 RID: 2105
	public enum AndOrState
	{
		// Token: 0x040036EA RID: 14058
		Invalid,
		// Token: 0x040036EB RID: 14059
		And,
		// Token: 0x040036EC RID: 14060
		Or
	}

	// Token: 0x0200083A RID: 2106
	public enum FilterElementType
	{
		// Token: 0x040036EE RID: 14062
		Invalid,
		// Token: 0x040036EF RID: 14063
		Condition,
		// Token: 0x040036F0 RID: 14064
		Terrain,
		// Token: 0x040036F1 RID: 14065
		Night,
		// Token: 0x040036F2 RID: 14066
		Target,
		// Token: 0x040036F3 RID: 14067
		Buff,
		// Token: 0x040036F4 RID: 14068
		Abnormal,
		// Token: 0x040036F5 RID: 14069
		Guts
	}

	// Token: 0x0200083B RID: 2107
	public enum PhotoLevelType
	{
		// Token: 0x040036F7 RID: 14071
		One,
		// Token: 0x040036F8 RID: 14072
		All
	}

	// Token: 0x0200083C RID: 2108
	public enum PhotoAlbumRegistrationStatus
	{
		// Token: 0x040036FA RID: 14074
		Invalid,
		// Token: 0x040036FB RID: 14075
		Unregistered,
		// Token: 0x040036FC RID: 14076
		Registered,
		// Token: 0x040036FD RID: 14077
		BreakthroughLimitMax,
		// Token: 0x040036FE RID: 14078
		GrowthMax
	}

	// Token: 0x0200083D RID: 2109
	public enum IconPlace
	{
		// Token: 0x04003700 RID: 14080
		Invalid,
		// Token: 0x04003701 RID: 14081
		PhotoAll,
		// Token: 0x04003702 RID: 14082
		PhotoGrow,
		// Token: 0x04003703 RID: 14083
		PhotoGrowMaterial,
		// Token: 0x04003704 RID: 14084
		PhotoSell,
		// Token: 0x04003705 RID: 14085
		PhotoPartyEdit,
		// Token: 0x04003706 RID: 14086
		PhotoAsistant,
		// Token: 0x04003707 RID: 14087
		PhotoAlbum,
		// Token: 0x04003708 RID: 14088
		AccessoryAll,
		// Token: 0x04003709 RID: 14089
		AccessoryGrow,
		// Token: 0x0400370A RID: 14090
		AccessoryGrowMaterial,
		// Token: 0x0400370B RID: 14091
		AccessorySell,
		// Token: 0x0400370C RID: 14092
		AccessoryCharaEdit
	}
}
