using System;

public class ItemDef
{
	public static int GetAddCharaLevelExpRatio(int itemId, CharaDef.AttributeType type)
	{
		return ItemDef.GetAddCharaLevelExpBase(itemId, type).ratioByAttribute;
	}

	public static ItemDef.AddCharaLevelExp GetAddCharaLevelExpBase(int itemId, CharaDef.AttributeType type)
	{
		ItemDef.AddCharaLevelExp addCharaLevelExp = new ItemDef.AddCharaLevelExp();
		DataManagerServerMst.CharaLevelItem charaLevelItem = DataManager.DmServerMst.charaLevelItemDataList.Find((DataManagerServerMst.CharaLevelItem item) => item.itemId == itemId);
		if (charaLevelItem == null)
		{
			return addCharaLevelExp;
		}
		if (charaLevelItem.attribute == CharaDef.AttributeType.ALL)
		{
			addCharaLevelExp.addExp = charaLevelItem.exp;
			addCharaLevelExp.ratioByAttribute = 150;
		}
		else if (charaLevelItem.attribute == type)
		{
			addCharaLevelExp.addExp = charaLevelItem.attributeExp;
			addCharaLevelExp.ratioByAttribute = (int)(charaLevelItem.attributeExp * 100L / charaLevelItem.exp);
		}
		else
		{
			addCharaLevelExp.addExp = charaLevelItem.exp;
			addCharaLevelExp.ratioByAttribute = 100;
		}
		addCharaLevelExp.needGold = charaLevelItem.strengCoinNum;
		return addCharaLevelExp;
	}

	public static ItemDef.Kind Id2Kind(int id)
	{
		foreach (ItemDef.KindData kindData in ItemDef.KindRange)
		{
			if (kindData.startId <= id && id <= kindData.endId)
			{
				return kindData.kind;
			}
		}
		return ItemDef.Kind.INVALID;
	}

	public static bool IsGrowKindByShop(ItemDef.Kind kind)
	{
		return kind == ItemDef.Kind.PROMOTE || kind - ItemDef.Kind.RANK_UP <= 2 || kind == ItemDef.Kind.PROMOTE_EXT;
	}

	public const int ITEM_ID_INVALID = 0;

	public const int ITEM_ID_J_COIN = 30101;

	public const int ITEM_ID_J_COIN_BANK = 30090;

	public const int ITEM_ID_STONE_UNION = 30100;

	public const int ITEM_ID_STONE_CHARGE = 30002;

	public const int ITEM_ID_STONE_FREE = 30001;

	public const int ITEM_ID_FRIEND_POINT = 30102;

	public const int ITEM_ID_TRAINING_COIN = 30133;

	public const int ITEM_ID_GACHA_TICKET_RARITY4 = 100003;

	public const int CONVERT_PYROXENE_LARGE_ITEM_ID = 30104;

	public const int CONVERT_PYROXENE_SMALL_ITEM_ID = 30119;

	public const int ITEM_ID_CLOTH_COIN = 30130;

	private static readonly ItemDef.KindData[] KindRange = new ItemDef.KindData[]
	{
		new ItemDef.KindData(ItemDef.Kind.INVALID, 0, 0),
		new ItemDef.KindData(ItemDef.Kind.CHARA, 1, 1000),
		new ItemDef.KindData(ItemDef.Kind.NPC, 1001, 2000),
		new ItemDef.KindData(ItemDef.Kind.PHOTO, 2001, 10000),
		new ItemDef.KindData(ItemDef.Kind.RANK_UP, 10001, 11000),
		new ItemDef.KindData(ItemDef.Kind.PHOTO_FRAME_UP, 11001, 12000),
		new ItemDef.KindData(ItemDef.Kind.ARTS_UP, 12001, 13000),
		new ItemDef.KindData(ItemDef.Kind.EXP_ADD, 13001, 13119),
		new ItemDef.KindData(ItemDef.Kind.ABILITY_RELEASE, 13120, 13130),
		new ItemDef.KindData(ItemDef.Kind.EXP_ADD, 13131, 14000),
		new ItemDef.KindData(ItemDef.Kind.PROMOTE, 14001, 14500),
		new ItemDef.KindData(ItemDef.Kind.KEMOBOARD, 15001, 16000),
		new ItemDef.KindData(ItemDef.Kind.PROMOTE_EXT, 14501, 15000),
		new ItemDef.KindData(ItemDef.Kind.PROMOTE_EXT, 16001, 17000),
		new ItemDef.KindData(ItemDef.Kind.GROWTH_MASTERSKILL, 17001, 17100),
		new ItemDef.KindData(ItemDef.Kind.KIZUNA_LIMITRELEASEITEM, 17101, 17200),
		new ItemDef.KindData(ItemDef.Kind.ACCESSORY_RELEASEITEM, 17201, 17300),
		new ItemDef.KindData(ItemDef.Kind.GROWTH_RESERVE, 17301, 20000),
		new ItemDef.KindData(ItemDef.Kind.ENEMY_CHARA, 20001, 30000),
		new ItemDef.KindData(ItemDef.Kind.STONE, 30001, 30010),
		new ItemDef.KindData(ItemDef.Kind.COMMON, 30011, 30090),
		new ItemDef.KindData(ItemDef.Kind.STONE, 30091, 30100),
		new ItemDef.KindData(ItemDef.Kind.COMMON, 30101, 31000),
		new ItemDef.KindData(ItemDef.Kind.STAMINA_RECOVERY, 31001, 32000),
		new ItemDef.KindData(ItemDef.Kind.COMMON_RESERVE, 32001, 33100),
		new ItemDef.KindData(ItemDef.Kind.COMMON, 33101, 36600),
		new ItemDef.KindData(ItemDef.Kind.COMMON_RESERVE, 36601, 40000),
		new ItemDef.KindData(ItemDef.Kind.PERFORMANCE_ONLY_OBJECT, 40001, 41000),
		new ItemDef.KindData(ItemDef.Kind.FURNITURE, 50001, 70000),
		new ItemDef.KindData(ItemDef.Kind.CLOTHES, 70001, 80000),
		new ItemDef.KindData(ItemDef.Kind.MASTER_SKILL, 80001, 90000),
		new ItemDef.KindData(ItemDef.Kind.EVENT_ITEM, 90001, 100000),
		new ItemDef.KindData(ItemDef.Kind.GACHA_TICKET, 100001, 110000),
		new ItemDef.KindData(ItemDef.Kind.ACCESSORY_ITEM, 110001, 120000),
		new ItemDef.KindData(ItemDef.Kind.PRESET, 200001, 300000),
		new ItemDef.KindData(ItemDef.Kind.LOTTERY_ITEM, 300001, 400000),
		new ItemDef.KindData(ItemDef.Kind.PICNIC_PLAYITEM, 400001, 410000),
		new ItemDef.KindData(ItemDef.Kind.CHARA_CONTACT, 500001, 600000),
		new ItemDef.KindData(ItemDef.Kind.TREEHOUSE_FURNITURE, 600001, 700000),
		new ItemDef.KindData(ItemDef.Kind.ACHIEVEMENT, 700001, 800000),
		new ItemDef.KindData(ItemDef.Kind.CLOTHES, 1000001, 2000000)
	};

	public class AddCharaLevelExp
	{
		public long addExp;

		public int needGold;

		public int ratioByAttribute;
	}

	public enum Kind
	{
		INVALID,
		CHARA,
		PHOTO,
		NPC,
		PROMOTE = 11,
		EXP_ADD,
		RANK_UP,
		PHOTO_FRAME_UP,
		ARTS_UP,
		STONE,
		COMMON,
		STAMINA_RECOVERY,
		FURNITURE,
		CLOTHES,
		PRESET,
		MASTER_SKILL,
		EVENT_ITEM,
		COMMON_RESERVE,
		GROWTH_MASTERSKILL,
		KIZUNA_LIMITRELEASEITEM,
		ACCESSORY_RELEASEITEM,
		GROWTH_RESERVE,
		GACHA_TICKET,
		ACCESSORY_ITEM,
		LOTTERY_ITEM,
		KEMOBOARD,
		CHARA_CONTACT,
		PICNIC_PLAYITEM,
		PROMOTE_EXT,
		TREEHOUSE_FURNITURE,
		ABILITY_RELEASE,
		ACHIEVEMENT,
		ENEMY_CHARA = 1001,
		PERFORMANCE_ONLY_OBJECT
	}

	private class KindData
	{
		public KindData(ItemDef.Kind kind, int startId, int endId)
		{
			this.kind = kind;
			this.startId = startId;
			this.endId = endId;
		}

		public ItemDef.Kind kind;

		public int startId;

		public int endId;
	}

	public enum Rarity
	{
		INVALID,
		STAR1,
		STAR2,
		STAR3,
		STAR4,
		STAR5
	}
}
