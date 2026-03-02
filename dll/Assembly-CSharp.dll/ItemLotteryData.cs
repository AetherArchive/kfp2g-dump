using System;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x0200008D RID: 141
public class ItemLotteryData : ItemStaticBase
{
	// Token: 0x0600057F RID: 1407 RVA: 0x000253E0 File Offset: 0x000235E0
	public override int GetId()
	{
		return this.mstData.id;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x000253ED File Offset: 0x000235ED
	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.LOTTERY_ITEM;
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x000253F1 File Offset: 0x000235F1
	public override string GetName()
	{
		return this.mstData.name;
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x000253FE File Offset: 0x000235FE
	public override string GetInfo()
	{
		return this.mstData.flavorText;
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0002540B File Offset: 0x0002360B
	public override ItemDef.Rarity GetRarity()
	{
		return (ItemDef.Rarity)this.mstData.rarity;
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x00025418 File Offset: 0x00023618
	public override int GetStackMax()
	{
		return 99999999;
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0002541F File Offset: 0x0002361F
	public override string GetIconName()
	{
		return "Texture2D/Icon_Item/" + this.mstData.iconName;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00025436 File Offset: 0x00023636
	public override int GetSalePrice()
	{
		return 0;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00025439 File Offset: 0x00023639
	public ItemLotteryData(MstItemLottery mstLottery, List<MstItemLotteryLineup> mstLineupList)
	{
		this.mstData = mstLottery;
		this.lineupList = mstLineupList;
	}

	// Token: 0x0400058C RID: 1420
	private MstItemLottery mstData;

	// Token: 0x0400058D RID: 1421
	public List<MstItemLotteryLineup> lineupList;
}
