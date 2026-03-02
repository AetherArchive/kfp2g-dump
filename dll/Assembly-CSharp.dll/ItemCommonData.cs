using System;
using SGNFW.Mst;
using UnityEngine;

// Token: 0x0200008B RID: 139
[Serializable]
public class ItemCommonData : ItemStaticBase
{
	// Token: 0x0600056B RID: 1387 RVA: 0x00025079 File Offset: 0x00023279
	public override int GetId()
	{
		return this.id;
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00025081 File Offset: 0x00023281
	public override ItemDef.Kind GetKind()
	{
		return this.kind;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00025089 File Offset: 0x00023289
	public override string GetName()
	{
		return this.itemName;
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00025091 File Offset: 0x00023291
	public override string GetInfo()
	{
		return this.itemInfo;
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00025099 File Offset: 0x00023299
	public override ItemDef.Rarity GetRarity()
	{
		return this.rarity;
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x000250A1 File Offset: 0x000232A1
	public override int GetStackMax()
	{
		return this.stackMax;
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x000250A9 File Offset: 0x000232A9
	public override string GetIconName()
	{
		return this.iconName;
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x000250B1 File Offset: 0x000232B1
	public override int GetSalePrice()
	{
		return this.salePrice;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x000250BC File Offset: 0x000232BC
	public static ItemCommonData CreateDirect(int itemId, string name, string itemInfo, int salePrice, ItemDef.Rarity rarity, int stackMax, string iconName = null)
	{
		ItemCommonData itemCommonData = new ItemCommonData(new MstItemCommon
		{
			id = itemId,
			name = name,
			flavorText = itemInfo,
			salePrice = salePrice,
			rarity = (int)rarity,
			stackMax = stackMax
		});
		if (iconName == null)
		{
			itemCommonData.iconName = "Texture2D/Icon_Item/icon_item_" + itemId.ToString("00000");
		}
		else
		{
			itemCommonData.iconName = iconName;
		}
		return itemCommonData;
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0002512C File Offset: 0x0002332C
	public ItemCommonData(MstItemCommon mst)
	{
		this.id = mst.id;
		this.kind = ItemDef.Id2Kind(mst.id);
		this.itemName = mst.name;
		this.itemInfo = mst.flavorText;
		this.salePrice = mst.salePrice;
		this.rarity = (ItemDef.Rarity)mst.rarity;
		this.stackMax = mst.stackMax;
		this.notForSale = mst.noSaleFlg == 1;
		this.iconName = "Texture2D/Icon_Item/" + mst.iconName;
	}

	// Token: 0x04000581 RID: 1409
	[SerializeField]
	private int id;

	// Token: 0x04000582 RID: 1410
	[SerializeField]
	private ItemDef.Kind kind;

	// Token: 0x04000583 RID: 1411
	[SerializeField]
	private string itemName;

	// Token: 0x04000584 RID: 1412
	[SerializeField]
	private string itemInfo;

	// Token: 0x04000585 RID: 1413
	[SerializeField]
	private ItemDef.Rarity rarity;

	// Token: 0x04000586 RID: 1414
	[SerializeField]
	private int stackMax;

	// Token: 0x04000587 RID: 1415
	[SerializeField]
	private int salePrice;

	// Token: 0x04000588 RID: 1416
	[SerializeField]
	private string iconName;

	// Token: 0x04000589 RID: 1417
	public bool notForSale;
}
