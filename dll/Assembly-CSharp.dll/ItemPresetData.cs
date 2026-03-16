using System;
using System.Collections.Generic;
using SGNFW.Mst;

[Serializable]
public class ItemPresetData : ItemStaticBase
{
	public override int GetId()
	{
		return this.mstData.id;
	}

	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.PRESET;
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

	public List<ItemPresetData.Item> SetItemList
	{
		get
		{
			List<ItemPresetData.Item> list = new List<ItemPresetData.Item>();
			foreach (ItemPresetData.Item item in this.setItemList)
			{
				list.Add(new ItemPresetData.Item(item.itemId, item.num, (int)item.dispType));
			}
			return list;
		}
	}

	public ItemPresetData(MstItemPreset mst)
	{
		this.mstData = mst;
		this.setItemList = new List<ItemPresetData.Item>
		{
			new ItemPresetData.Item(this.mstData.itemId00, this.mstData.itemNum00, this.mstData.itemDispType00),
			new ItemPresetData.Item(this.mstData.itemId01, this.mstData.itemNum01, this.mstData.itemDispType01),
			new ItemPresetData.Item(this.mstData.itemId02, this.mstData.itemNum02, this.mstData.itemDispType02),
			new ItemPresetData.Item(this.mstData.itemId03, this.mstData.itemNum03, this.mstData.itemDispType03),
			new ItemPresetData.Item(this.mstData.itemId04, this.mstData.itemNum04, this.mstData.itemDispType04),
			new ItemPresetData.Item(this.mstData.itemId05, this.mstData.itemNum05, this.mstData.itemDispType05)
		};
		this.setItemList.RemoveAll((ItemPresetData.Item item) => item.itemId == 0 || item.num == 0);
	}

	private MstItemPreset mstData;

	private List<ItemPresetData.Item> setItemList;

	public enum DispType
	{
		Invalid,
		Hidden
	}

	public class Item
	{
		public Item(int id, int number, int type)
		{
			this.itemId = id;
			this.num = number;
			this.dispType = (ItemPresetData.DispType)type;
		}

		public int itemId;

		public int num;

		public ItemPresetData.DispType dispType;
	}
}
