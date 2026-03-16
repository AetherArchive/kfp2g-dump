using System;
using System.Collections.Generic;

public class DefaultResultRarity : global::ResultRarity
{
	public override int[,] SKY_PER
	{
		get
		{
			return new int[,]
			{
				{ 100, 0, 0 },
				{ 100, 0, 0 },
				{ 100, 0, 0 },
				{ 80, 100, 0 },
				{ 50, 80, 100 },
				{ 50, 80, 100 },
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
				{ 80, 100 },
				{ 80, 100 },
				{ 80, 100 },
				{ 80, 100 }
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
				{ 30, 80, 100 },
				{ 30, 80, 100 },
				{ 10, 35, 100 },
				{ 10, 35, 100 }
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
				{ 30, 80, 100 },
				{ 30, 80, 100 },
				{ 30, 80, 100 },
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
			switch (num)
			{
			case 2:
				if (3 == gachaResultRarity)
				{
					num = 4;
					if (DataManagerGacha.IsExistResultPuChara())
					{
						num = 6;
					}
				}
				break;
			case 3:
				if (2 == gachaResultRarity || 3 == gachaResultRarity)
				{
					num = 4;
					if (DataManagerGacha.IsExistResultPuChara())
					{
						num = 6;
					}
				}
				break;
			case 5:
				if (2 == gachaResultRarity)
				{
					num = 6;
				}
				break;
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
		ItemDef.Kind kind = ItemDef.Id2Kind(authItem.itemId);
		int num;
		if (kind != ItemDef.Kind.CHARA)
		{
			if (kind != ItemDef.Kind.PHOTO)
			{
				if (kind != ItemDef.Kind.TREEHOUSE_FURNITURE)
				{
				}
				num = 1;
			}
			else
			{
				num = DataManager.DmPhoto.GetPhotoStaticData(authItem.itemId).rarityData.rarity;
			}
		}
		else
		{
			num = DataManager.DmChara.GetCharaStaticData(authItem.itemId).baseData.rankLow;
		}
		DefaultResultRarity.ResultRarity resultRarity;
		if (4 <= num)
		{
			if (ItemDef.Kind.CHARA == kind)
			{
				resultRarity = DefaultResultRarity.ResultRarity.FourOrMoreAndChara;
				if (DataManagerGacha.IsExistResultPuChara())
				{
					resultRarity = DefaultResultRarity.ResultRarity.FourOrMoreAndPuChara;
				}
			}
			else
			{
				resultRarity = DefaultResultRarity.ResultRarity.FourOrMore;
			}
		}
		else if (3 <= num)
		{
			resultRarity = DefaultResultRarity.ResultRarity.Three;
		}
		else
		{
			resultRarity = DefaultResultRarity.ResultRarity.LessThanThree;
		}
		return (int)resultRarity;
	}

	public override string LotteryPremiumSEName(List<GachaAuthCtrl.AuthItem> authItemList, List<string> seNameList)
	{
		string text = string.Empty;
		int highRarity = this.GetHighRarity(authItemList);
		if (highRarity - 3 <= 3)
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
		bool flag = false;
		int gachaResultRarity = this.GetGachaResultRarity(authItem);
		if (3 == gachaResultRarity)
		{
			int num = new Random().Next(100);
			if (50 > num)
			{
				flag = true;
			}
		}
		return flag;
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
		case 3:
		case 5:
		case 6:
			effectType = AuthPlayer.GachaParam.EffectType.RAINBOW;
			break;
		}
		return effectType;
	}

	private enum ResultRarity
	{
		LessThanThree,
		Three,
		FourOrMore,
		FourOrMoreAndChara,
		MultipleFourOrMoreAndIncludesChara,
		FourOrMoreAndPuChara,
		MultipleFourOrMoreAndIncludesPuChara
	}
}
