using System;
using System.Collections.Generic;

public class SortFilterDefine
{
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
		},
		{
			SortFilterDefine.RegisterType.STICKER_COLLECTION,
			SortFilterDefine.SortType.STICKER_RARITY
		}
	};

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

	public static readonly List<SortFilterDefine.SortType> PhotoAlbumSortTypeList = new List<SortFilterDefine.SortType>
	{
		SortFilterDefine.SortType.RARITY,
		SortFilterDefine.SortType.HP,
		SortFilterDefine.SortType.ATK,
		SortFilterDefine.SortType.DEF
	};

	public static readonly List<SortFilterDefine.SortType> HelperSortTypeList = new List<SortFilterDefine.SortType>
	{
		SortFilterDefine.SortType.LOGIN,
		SortFilterDefine.SortType.LEVEL,
		SortFilterDefine.SortType.RARITY,
		SortFilterDefine.SortType.NEW
	};

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

	public static readonly List<SortFilterDefine.SortType> StickerSortTypeList = new List<SortFilterDefine.SortType>
	{
		SortFilterDefine.SortType.STICKER_RARITY,
		SortFilterDefine.SortType.STICKER_NAME,
		SortFilterDefine.SortType.STICKER_COUNT
	};

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
		},
		{
			SortFilterDefine.SortType.STICKER_NAME,
			"なまえ順"
		},
		{
			SortFilterDefine.SortType.STICKER_COUNT,
			"所持枚数順"
		},
		{
			SortFilterDefine.SortType.STICKER_RARITY,
			"レアリティ順"
		}
	};

	public static readonly string BTN_DISABLE_STR_NAME = "非表示";

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

	public static readonly List<string> photoFilterStarNameList = new List<string> { "☆１", "☆２", "☆３", "☆４" };

	public static readonly List<string> photoFilterPhototypeNameList = new List<string>
	{
		"",
		"",
		"強化素材",
		SortFilterDefine.BTN_DISABLE_STR_NAME
	};

	public static readonly List<string> photoFilterFavoriteList = new List<string> { "", "" };

	public static readonly List<string> photoFilterSpecialNameList = new List<string> { "獲得量アップ", "限界突破" };

	public static readonly List<string> photoFilterLimitSpecialNameList = new List<string> { "限界突破" };

	public static readonly List<string> photoFilterLevelNameList = new List<string> { "ＬＶ１", "すべて" };

	public static readonly List<string> photoFilterAlbumNameList = new List<string> { "未登録", "登録済み", "\u3000\u3000登録済み\u3000\u3000\n(限界突破最大)", "\u3000\u3000登録済み\u3000\u3000\n(強化最大)" };

	public static readonly List<string> friendsFilterFavoriteList = new List<string> { "", "" };

	public static readonly List<string> friendsFilterTypeList = new List<string> { "", "", "", "", "", "" };

	public static readonly List<string> friendsFilterFriendsList = new List<string> { "", "" };

	public static readonly List<string> friendsFilterEventList = new List<string> { "", "" };

	public static readonly List<string> friendsFilterMiracleList = new List<string> { "効果対象", "効果内容" };

	public static readonly List<string> friendsFilterMiracleTargetList = new List<string> { "敵全体", "敵単体", "自身", "味方", "ランダム" };

	public static readonly List<string> friendsFilterMiracleEffectList = new List<string> { "攻撃", "与ダメージ", "被ダメージ", "MP", "体力回復", "攻撃命中率", "回避", "プラズムチャージ", "いかく/かくれみ" };

	public static readonly List<string> stickerFilterRarityList = new List<string> { "R", "SR", "SSR" };

	public static readonly List<string> stickerFilterTypeList = new List<string> { "アイテム", "フレンズ", "フォト", "すぺしゃる" };

	public enum SortType
	{
		INVALID,
		LEVEL,
		HP,
		ATK,
		DEF,
		RARITY,
		NEW,
		LOGIN,
		BREAKTHROUGH_LIMIT,
		USER_RANK,
		NAME,
		KEMO_STATUS,
		KIZUNA,
		PHOTO_POCKET,
		WILD_RELEASE,
		AVOIDANCE,
		PLASM_POINT,
		BEAT_DAMAGE,
		ACTION_DAMAGE,
		TRY_DAMAGE,
		STICKER_RARITY,
		STICKER_NAME,
		STICKER_COUNT
	}

	public enum RegisterType
	{
		INVALID,
		CHARA_ALL,
		CHARA_DECK,
		PHOTO_DECK,
		PHOTO_GROW_TOP,
		PHOTO_GROW_MAIN,
		PHOTO_SELL_MAIN,
		PHOTO_ALL,
		CHARA_FAVORITE,
		CHARA_HELPER_LOAN,
		PHOTO_HELPER_LOAN,
		CHARA_GROW_TOP,
		CHARA_QUEST_TOP,
		HELP_FOLLOW,
		HELP_FOLLOWER,
		HOME_CLOSET,
		CHARA_EVENT_GROW,
		STORY_VIEW,
		CHARA_DECK_PVP,
		PHOTO_DECK_PVP,
		PICNIC_CHANGE,
		CHARA_DECK_TRAINING,
		PHOTO_DECK_TRAINING,
		PHOTO_ALBUM,
		TREEHOUSE_CHANGE,
		CHARA_COMMUNICATION,
		ACCESSORY_ALL,
		ACCESSORY_GROW_BASE,
		ACCESSORY_GROW_MATERIAL,
		ACCESSORY_SELL,
		ACCESSORY_EQUIP,
		CHARA_SHOP_ASSISTANT,
		CHARA_QUEST_ASSISTANT,
		STICKER_COLLECTION
	}

	public enum SortFilterType
	{
		INVALID,
		CHARA_SORT,
		CHARA_FILTER,
		PHOTO_SORT,
		PHOTO_FILTER,
		FOLLOW_SORT,
		STICKER_SORT,
		STICKER_FILTER
	}

	public enum CharacteristicFilterCategory
	{
		Invalid,
		Conditions,
		Target,
		Effect,
		Abnormal
	}

	public enum FriendsMiracleFileterCategory
	{
		Target,
		Effect
	}

	public enum FriendsCharacteristicFilterCategory
	{
		Invalid,
		Conditions,
		Target,
		Effect,
		Resist
	}

	public enum PhotoFilterType
	{
		SortFilter,
		SellPhoto,
		PhotoAlbum,
		PhotoGrow
	}

	public enum AndOrState
	{
		Invalid,
		And,
		Or
	}

	public enum FilterElementType
	{
		Invalid,
		Condition,
		Terrain,
		Night,
		Target,
		Buff,
		Abnormal,
		Guts
	}

	public enum PhotoLevelType
	{
		One,
		All
	}

	public enum RegistrationStatus
	{
		Invalid,
		Unregistered,
		Registered,
		BreakthroughLimitMax,
		GrowthMax
	}

	public enum IconPlace
	{
		Invalid,
		PhotoAll,
		PhotoGrow,
		PhotoGrowMaterial,
		PhotoSell,
		PhotoPartyEdit,
		PhotoAsistant,
		PhotoAlbum,
		AccessoryAll,
		AccessoryGrow,
		AccessoryGrowMaterial,
		AccessorySell,
		AccessoryCharaEdit
	}

	public class RagistrationStatusEqualityComparer : IEqualityComparer<SortFilterDefine.RegistrationStatus>
	{
		public bool Equals(SortFilterDefine.RegistrationStatus x, SortFilterDefine.RegistrationStatus y)
		{
			return x == y;
		}

		public int GetHashCode(SortFilterDefine.RegistrationStatus obj)
		{
			return (int)obj;
		}
	}
}
