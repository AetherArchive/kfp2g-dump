using System;
using System.Collections.Generic;

public class FurnitureOnlyResultRarity : global::ResultRarity
{
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

	public override int[,] PUT_PER_V
	{
		get
		{
			return new int[,] { { 15, 30, 45, 65 } };
		}
	}

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

	public override bool LotteryPromotion(GachaAuthCtrl.AuthItem authItem)
	{
		return false;
	}

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

	private enum ResultRarity
	{
		LessThanFour,
		Four,
		FiveOrMore,
		MultipleFiveOrMore
	}
}
