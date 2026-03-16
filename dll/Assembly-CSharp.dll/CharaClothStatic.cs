using System;
using SGNFW.Mst;

public class CharaClothStatic : ItemStaticBase
{
	private int Id { get; set; }

	public int CharaId { get; private set; }

	public int GetRank { get; private set; }

	public int SortNum { get; private set; }

	private int Rarity { get; set; }

	private string Name { get; set; }

	private string FlavorText { get; set; }

	public int ImageId { get; private set; }

	public string displayTexturePath
	{
		get
		{
			return "Texture2D/Icon_Dressup/icon_dressup_" + this.Id.ToString("D4");
		}
	}

	public string displayBgTexturePath { get; private set; }

	public int HpBonus { get; private set; }

	public int AtkBonus { get; private set; }

	public int DefBonus { get; private set; }

	public bool LongSkirt { get; private set; }

	public CharaClothStatic.PlayMotionType PlayMotion { get; private set; }

	public int replaceItemNum { get; private set; }

	public static string GetDefaultClothPath(int charaId)
	{
		return "Texture2D/Icon_Dressup_Bg/icon_dressup_bg_default";
	}

	public override int GetId()
	{
		return this.Id;
	}

	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.CLOTHES;
	}

	public override string GetName()
	{
		return this.Name;
	}

	public override string GetInfo()
	{
		return this.FlavorText;
	}

	public override ItemDef.Rarity GetRarity()
	{
		return (ItemDef.Rarity)this.Rarity;
	}

	public override int GetStackMax()
	{
		return 99999999;
	}

	public override string GetIconName()
	{
		return "Texture2D/Icon_Item/" + this.icon_name;
	}

	public override int GetSalePrice()
	{
		return 0;
	}

	public CharaClothStatic(MstCharaClothesData mst, int cId)
	{
		this.Id = mst.id;
		this.CharaId = cId;
		this.Name = mst.name;
		this.FlavorText = mst.flavorText;
		this.SortNum = mst.sort;
		this.ImageId = mst.imgId;
		this.GetRank = mst.getRank;
		this.displayBgTexturePath = mst.bgTextureName;
		this.HpBonus = mst.hpBonus;
		this.AtkBonus = mst.atkBonus;
		this.DefBonus = mst.defBonus;
		this.icon_name = mst.iconName;
		this.LongSkirt = 1 == mst.longskirt;
		this.Rarity = mst.rarity;
		this.replaceItemNum = mst.replaceItemNum;
		this.PlayMotion = (CharaClothStatic.PlayMotionType)mst.playMotionType;
	}

	private string icon_name;

	public enum PlayMotionType
	{
		Default,
		HandsNotUse
	}
}
