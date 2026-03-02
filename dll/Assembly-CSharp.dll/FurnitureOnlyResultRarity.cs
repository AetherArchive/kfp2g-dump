using System;
using System.Collections.Generic;

// Token: 0x02000146 RID: 326
public class FurnitureOnlyResultRarity : global::ResultRarity
{
	// Token: 0x1700036C RID: 876
	// (get) Token: 0x060011DE RID: 4574 RVA: 0x000D8FEE File Offset: 0x000D71EE
	public override int[,] SKY_PER
	{
		get
		{
			return new int[,]
			{
				{ 100, 0, 0 },
				{ 100, 0, 0 },
				{ 80, 100, 0 },
				{ 50, 80, 100 }
			};
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x060011DF RID: 4575 RVA: 0x000D9002 File Offset: 0x000D7202
	public override int[,] POST_PER
	{
		get
		{
			return new int[,]
			{
				{ 100, 0 },
				{ 100, 0 },
				{ 100, 0 },
				{ 100, 0 }
			};
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x060011E0 RID: 4576 RVA: 0x000D9016 File Offset: 0x000D7216
	public override int[,] PUT_PER
	{
		get
		{
			return new int[,]
			{
				{ 100, 0, 0 },
				{ 100, 0, 0 },
				{ 60, 100, 0 },
				{ 20, 100, 0 }
			};
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x060011E1 RID: 4577 RVA: 0x000D902A File Offset: 0x000D722A
	public override int[,] PUT_PER_V
	{
		get
		{
			return new int[,] { { 15, 30, 45, 65 } };
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x060011E2 RID: 4578 RVA: 0x000D903E File Offset: 0x000D723E
	public override int[,] EFF_PER
	{
		get
		{
			return new int[,]
			{
				{ 100, 0, 0 },
				{ 100, 0, 0 },
				{ 60, 100, 0 },
				{ 30, 80, 100 }
			};
		}
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x000D905C File Offset: 0x000D725C
	public override int GetHighRarity(List<GachaAuthCtrl.AuthItem> authItemList)
	{
		int num = 0;
		foreach (GachaAuthCtrl.AuthItem authItem in authItemList)
		{
			int gachaResultRarity = this.GetGachaResultRarity(authItem);
			if (num == 2 && 2 == gachaResultRarity)
			{
				num = 3;
			}
			if (gachaResultRarity > num)
			{
				num = gachaResultRarity;
			}
		}
		return num;
	}

	// Token: 0x060011E5 RID: 4581 RVA: 0x000D90C0 File Offset: 0x000D72C0
	public override int GetGachaResultRarity(GachaAuthCtrl.AuthItem authItem)
	{
		ItemDef.Rarity rarity;
		if (ItemDef.Id2Kind(authItem.itemId) == ItemDef.Kind.TREEHOUSE_FURNITURE)
		{
			rarity = DataManager.DmTreeHouse.GetTreeHouseFurnitureStaticData(authItem.itemId).GetRarity();
		}
		else
		{
			rarity = ItemDef.Rarity.STAR1;
		}
		FurnitureOnlyResultRarity.ResultRarity resultRarity;
		if (rarity != ItemDef.Rarity.STAR4)
		{
			if (rarity == ItemDef.Rarity.STAR5)
			{
				resultRarity = FurnitureOnlyResultRarity.ResultRarity.FiveOrMore;
			}
			else
			{
				resultRarity = FurnitureOnlyResultRarity.ResultRarity.LessThanFour;
			}
		}
		else
		{
			resultRarity = FurnitureOnlyResultRarity.ResultRarity.Four;
		}
		return (int)resultRarity;
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x000D910C File Offset: 0x000D730C
	public override string LotteryPremiumSEName(List<GachaAuthCtrl.AuthItem> authItemList, List<string> seNameList)
	{
		string text = string.Empty;
		int highRarity = this.GetHighRarity(authItemList);
		if (highRarity - 2 <= 1)
		{
			Random random = new Random();
			int num = random.Next(100);
			if (20 > num)
			{
				text = seNameList[random.Next(seNameList.Count)];
			}
		}
		return text;
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x000D9155 File Offset: 0x000D7355
	public override bool LotteryPromotion(GachaAuthCtrl.AuthItem authItem)
	{
		return false;
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x000D9158 File Offset: 0x000D7358
	public override AuthPlayer.GachaParam.EffectType GetEffectTypeForGachaParamAfter(GachaAuthCtrl.AuthItem authItem)
	{
		AuthPlayer.GachaParam.EffectType effectType = AuthPlayer.GachaParam.EffectType.BLUE;
		switch (this.GetGachaResultRarity(authItem))
		{
		case 1:
			effectType = AuthPlayer.GachaParam.EffectType.GOLD;
			break;
		case 2:
			effectType = AuthPlayer.GachaParam.EffectType.RAINBOW;
			break;
		}
		return effectType;
	}

	// Token: 0x02000AC3 RID: 2755
	private enum ResultRarity
	{
		// Token: 0x0400445D RID: 17501
		LessThanFour,
		// Token: 0x0400445E RID: 17502
		Four,
		// Token: 0x0400445F RID: 17503
		FiveOrMore,
		// Token: 0x04004460 RID: 17504
		MultipleFiveOrMore
	}
}
