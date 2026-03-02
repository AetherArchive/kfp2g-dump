using System;
using System.Collections.Generic;

// Token: 0x02000145 RID: 325
public class DefaultResultRarity : global::ResultRarity
{
	// Token: 0x17000367 RID: 871
	// (get) Token: 0x060011D3 RID: 4563 RVA: 0x000D8D98 File Offset: 0x000D6F98
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

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x060011D4 RID: 4564 RVA: 0x000D8DAC File Offset: 0x000D6FAC
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

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x060011D5 RID: 4565 RVA: 0x000D8DC0 File Offset: 0x000D6FC0
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

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x060011D6 RID: 4566 RVA: 0x000D8DD4 File Offset: 0x000D6FD4
	public override int[,] PUT_PER_V
	{
		get
		{
			return new int[,] { { 15, 30, 45, 65 } };
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x060011D7 RID: 4567 RVA: 0x000D8DE8 File Offset: 0x000D6FE8
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

	// Token: 0x060011D9 RID: 4569 RVA: 0x000D8E04 File Offset: 0x000D7004
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

	// Token: 0x060011DA RID: 4570 RVA: 0x000D8EA4 File Offset: 0x000D70A4
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

	// Token: 0x060011DB RID: 4571 RVA: 0x000D8F30 File Offset: 0x000D7130
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

	// Token: 0x060011DC RID: 4572 RVA: 0x000D8F7C File Offset: 0x000D717C
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

	// Token: 0x060011DD RID: 4573 RVA: 0x000D8FAC File Offset: 0x000D71AC
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

	// Token: 0x02000AC2 RID: 2754
	private enum ResultRarity
	{
		// Token: 0x04004455 RID: 17493
		LessThanThree,
		// Token: 0x04004456 RID: 17494
		Three,
		// Token: 0x04004457 RID: 17495
		FourOrMore,
		// Token: 0x04004458 RID: 17496
		FourOrMoreAndChara,
		// Token: 0x04004459 RID: 17497
		MultipleFourOrMoreAndIncludesChara,
		// Token: 0x0400445A RID: 17498
		FourOrMoreAndPuChara,
		// Token: 0x0400445B RID: 17499
		MultipleFourOrMoreAndIncludesPuChara
	}
}
