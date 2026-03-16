using System;
using SGNFW.Mst;
using UnityEngine;

[Serializable]
public class ItemCommonData : ItemStaticBase
{
	public override int GetId()
	{
		return this.id;
	}

	public override ItemDef.Kind GetKind()
	{
		return this.kind;
	}

	public override string GetName()
	{
		return this.itemName;
	}

	public override string GetInfo()
	{
		return this.itemInfo;
	}

	public override ItemDef.Rarity GetRarity()
	{
		return this.rarity;
	}

	public override int GetStackMax()
	{
		return this.stackMax;
	}

	public override string GetIconName()
	{
		return this.iconName;
	}

	public override int GetSalePrice()
	{
		return this.salePrice;
	}

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

	[SerializeField]
	private int id;

	[SerializeField]
	private ItemDef.Kind kind;

	[SerializeField]
	private string itemName;

	[SerializeField]
	private string itemInfo;

	[SerializeField]
	private ItemDef.Rarity rarity;

	[SerializeField]
	private int stackMax;

	[SerializeField]
	private int salePrice;

	[SerializeField]
	private string iconName;

	public bool notForSale;
}
