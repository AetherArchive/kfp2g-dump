using System;
using System.Collections.Generic;
using SGNFW.Mst;

public class ItemLotteryData : ItemStaticBase
{
	public override int GetId()
	{
		return this.mstData.id;
	}

	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.LOTTERY_ITEM;
	}

	public override string GetName()
	{
		return this.mstData.name;
	}

	public override string GetInfo()
	{
		return this.mstData.flavorText;
	}

	public override ItemDef.Rarity GetRarity()
	{
		return (ItemDef.Rarity)this.mstData.rarity;
	}

	public override int GetStackMax()
	{
		return 99999999;
	}

	public override string GetIconName()
	{
		return "Texture2D/Icon_Item/" + this.mstData.iconName;
	}

	public override int GetSalePrice()
	{
		return 0;
	}

	public ItemLotteryData(MstItemLottery mstLottery, List<MstItemLotteryLineup> mstLineupList)
	{
		this.mstData = mstLottery;
		this.lineupList = mstLineupList;
	}

	private MstItemLottery mstData;

	public List<MstItemLotteryLineup> lineupList;
}
