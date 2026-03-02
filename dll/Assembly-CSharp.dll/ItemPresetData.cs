using System;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x0200008C RID: 140
[Serializable]
public class ItemPresetData : ItemStaticBase
{
	// Token: 0x06000575 RID: 1397 RVA: 0x000251BD File Offset: 0x000233BD
	public override int GetId()
	{
		return this.mstData.id;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x000251CA File Offset: 0x000233CA
	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.PRESET;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x000251CE File Offset: 0x000233CE
	public override string GetName()
	{
		return this.mstData.name;
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x000251DB File Offset: 0x000233DB
	public override string GetInfo()
	{
		return this.mstData.flavorText;
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x000251E8 File Offset: 0x000233E8
	public override ItemDef.Rarity GetRarity()
	{
		return (ItemDef.Rarity)this.mstData.rarity;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x000251F5 File Offset: 0x000233F5
	public override int GetStackMax()
	{
		return 99999999;
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x000251FC File Offset: 0x000233FC
	public override string GetIconName()
	{
		return "Texture2D/Icon_Item/" + this.mstData.iconName;
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x00025213 File Offset: 0x00023413
	public override int GetSalePrice()
	{
		return 0;
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x0600057D RID: 1405 RVA: 0x00025218 File Offset: 0x00023418
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

	// Token: 0x0600057E RID: 1406 RVA: 0x00025288 File Offset: 0x00023488
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

	// Token: 0x0400058A RID: 1418
	private MstItemPreset mstData;

	// Token: 0x0400058B RID: 1419
	private List<ItemPresetData.Item> setItemList;

	// Token: 0x020006DA RID: 1754
	public enum DispType
	{
		// Token: 0x040030D1 RID: 12497
		Invalid,
		// Token: 0x040030D2 RID: 12498
		Hidden
	}

	// Token: 0x020006DB RID: 1755
	public class Item
	{
		// Token: 0x06003336 RID: 13110 RVA: 0x001C1805 File Offset: 0x001BFA05
		public Item(int id, int number, int type)
		{
			this.itemId = id;
			this.num = number;
			this.dispType = (ItemPresetData.DispType)type;
		}

		// Token: 0x040030D3 RID: 12499
		public int itemId;

		// Token: 0x040030D4 RID: 12500
		public int num;

		// Token: 0x040030D5 RID: 12501
		public ItemPresetData.DispType dispType;
	}
}
