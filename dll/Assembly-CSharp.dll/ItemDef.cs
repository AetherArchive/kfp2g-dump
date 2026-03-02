using System;

// Token: 0x0200008F RID: 143
public class ItemDef
{
	// Token: 0x0600058A RID: 1418 RVA: 0x0002546D File Offset: 0x0002366D
	public static int GetAddCharaLevelExpRatio(int itemId, CharaDef.AttributeType type)
	{
		return ItemDef.GetAddCharaLevelExpBase(itemId, type).ratioByAttribute;
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x0002547C File Offset: 0x0002367C
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

	// Token: 0x0600058C RID: 1420 RVA: 0x00025530 File Offset: 0x00023730
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

	// Token: 0x0600058D RID: 1421 RVA: 0x0002556F File Offset: 0x0002376F
	public static bool IsGrowKindByShop(ItemDef.Kind kind)
	{
		return kind == ItemDef.Kind.PROMOTE || kind - ItemDef.Kind.RANK_UP <= 2 || kind == ItemDef.Kind.PROMOTE_EXT;
	}

	// Token: 0x04000590 RID: 1424
	public const int ITEM_ID_INVALID = 0;

	// Token: 0x04000591 RID: 1425
	public const int ITEM_ID_J_COIN = 30101;

	// Token: 0x04000592 RID: 1426
	public const int ITEM_ID_J_COIN_BANK = 30090;

	// Token: 0x04000593 RID: 1427
	public const int ITEM_ID_STONE_UNION = 30100;

	// Token: 0x04000594 RID: 1428
	public const int ITEM_ID_STONE_CHARGE = 30002;

	// Token: 0x04000595 RID: 1429
	public const int ITEM_ID_STONE_FREE = 30001;

	// Token: 0x04000596 RID: 1430
	public const int ITEM_ID_FRIEND_POINT = 30102;

	// Token: 0x04000597 RID: 1431
	public const int ITEM_ID_TRAINING_COIN = 30133;

	// Token: 0x04000598 RID: 1432
	public const int ITEM_ID_GACHA_TICKET_RARITY4 = 100003;

	// Token: 0x04000599 RID: 1433
	public const int CONVERT_PYROXENE_LARGE_ITEM_ID = 30104;

	// Token: 0x0400059A RID: 1434
	public const int CONVERT_PYROXENE_SMALL_ITEM_ID = 30119;

	// Token: 0x0400059B RID: 1435
	public const int ITEM_ID_CLOTH_COIN = 30130;

	// Token: 0x0400059C RID: 1436
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

	// Token: 0x020006DD RID: 1757
	public class AddCharaLevelExp
	{
		// Token: 0x040030D8 RID: 12504
		public long addExp;

		// Token: 0x040030D9 RID: 12505
		public int needGold;

		// Token: 0x040030DA RID: 12506
		public int ratioByAttribute;
	}

	// Token: 0x020006DE RID: 1758
	public enum Kind
	{
		// Token: 0x040030DC RID: 12508
		INVALID,
		// Token: 0x040030DD RID: 12509
		CHARA,
		// Token: 0x040030DE RID: 12510
		PHOTO,
		// Token: 0x040030DF RID: 12511
		NPC,
		// Token: 0x040030E0 RID: 12512
		PROMOTE = 11,
		// Token: 0x040030E1 RID: 12513
		EXP_ADD,
		// Token: 0x040030E2 RID: 12514
		RANK_UP,
		// Token: 0x040030E3 RID: 12515
		PHOTO_FRAME_UP,
		// Token: 0x040030E4 RID: 12516
		ARTS_UP,
		// Token: 0x040030E5 RID: 12517
		STONE,
		// Token: 0x040030E6 RID: 12518
		COMMON,
		// Token: 0x040030E7 RID: 12519
		STAMINA_RECOVERY,
		// Token: 0x040030E8 RID: 12520
		FURNITURE,
		// Token: 0x040030E9 RID: 12521
		CLOTHES,
		// Token: 0x040030EA RID: 12522
		PRESET,
		// Token: 0x040030EB RID: 12523
		MASTER_SKILL,
		// Token: 0x040030EC RID: 12524
		EVENT_ITEM,
		// Token: 0x040030ED RID: 12525
		COMMON_RESERVE,
		// Token: 0x040030EE RID: 12526
		GROWTH_MASTERSKILL,
		// Token: 0x040030EF RID: 12527
		KIZUNA_LIMITRELEASEITEM,
		// Token: 0x040030F0 RID: 12528
		ACCESSORY_RELEASEITEM,
		// Token: 0x040030F1 RID: 12529
		GROWTH_RESERVE,
		// Token: 0x040030F2 RID: 12530
		GACHA_TICKET,
		// Token: 0x040030F3 RID: 12531
		ACCESSORY_ITEM,
		// Token: 0x040030F4 RID: 12532
		LOTTERY_ITEM,
		// Token: 0x040030F5 RID: 12533
		KEMOBOARD,
		// Token: 0x040030F6 RID: 12534
		CHARA_CONTACT,
		// Token: 0x040030F7 RID: 12535
		PICNIC_PLAYITEM,
		// Token: 0x040030F8 RID: 12536
		PROMOTE_EXT,
		// Token: 0x040030F9 RID: 12537
		TREEHOUSE_FURNITURE,
		// Token: 0x040030FA RID: 12538
		ABILITY_RELEASE,
		// Token: 0x040030FB RID: 12539
		ACHIEVEMENT,
		// Token: 0x040030FC RID: 12540
		ENEMY_CHARA = 1001,
		// Token: 0x040030FD RID: 12541
		PERFORMANCE_ONLY_OBJECT
	}

	// Token: 0x020006DF RID: 1759
	private class KindData
	{
		// Token: 0x0600333B RID: 13115 RVA: 0x001C1853 File Offset: 0x001BFA53
		public KindData(ItemDef.Kind kind, int startId, int endId)
		{
			this.kind = kind;
			this.startId = startId;
			this.endId = endId;
		}

		// Token: 0x040030FE RID: 12542
		public ItemDef.Kind kind;

		// Token: 0x040030FF RID: 12543
		public int startId;

		// Token: 0x04003100 RID: 12544
		public int endId;
	}

	// Token: 0x020006E0 RID: 1760
	public enum Rarity
	{
		// Token: 0x04003102 RID: 12546
		INVALID,
		// Token: 0x04003103 RID: 12547
		STAR1,
		// Token: 0x04003104 RID: 12548
		STAR2,
		// Token: 0x04003105 RID: 12549
		STAR3,
		// Token: 0x04003106 RID: 12550
		STAR4,
		// Token: 0x04003107 RID: 12551
		STAR5
	}
}
