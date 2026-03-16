using System;

public abstract class ItemStaticBase
{
	public abstract int GetId();

	public abstract ItemDef.Kind GetKind();

	public abstract string GetName();

	public abstract string GetInfo();

	public abstract ItemDef.Rarity GetRarity();

	public abstract int GetStackMax();

	public abstract string GetIconName();

	public abstract int GetSalePrice();

	public DateTime? endTime;
}
