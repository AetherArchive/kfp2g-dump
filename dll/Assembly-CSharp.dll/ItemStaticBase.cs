using System;

// Token: 0x0200008A RID: 138
public abstract class ItemStaticBase
{
	// Token: 0x06000562 RID: 1378
	public abstract int GetId();

	// Token: 0x06000563 RID: 1379
	public abstract ItemDef.Kind GetKind();

	// Token: 0x06000564 RID: 1380
	public abstract string GetName();

	// Token: 0x06000565 RID: 1381
	public abstract string GetInfo();

	// Token: 0x06000566 RID: 1382
	public abstract ItemDef.Rarity GetRarity();

	// Token: 0x06000567 RID: 1383
	public abstract int GetStackMax();

	// Token: 0x06000568 RID: 1384
	public abstract string GetIconName();

	// Token: 0x06000569 RID: 1385
	public abstract int GetSalePrice();

	// Token: 0x04000580 RID: 1408
	public DateTime? endTime;
}
