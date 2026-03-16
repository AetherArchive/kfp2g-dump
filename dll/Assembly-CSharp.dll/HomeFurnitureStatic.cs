using System;
using SGNFW.Mst;

public class HomeFurnitureStatic : ItemStaticBase
{
	public int id { get; set; }

	public ItemDef.Rarity rarity { get; private set; }

	public HomeFurnitureStatic.Category category { get; private set; }

	public string name { get; private set; }

	public string flavorText { get; private set; }

	public string modelFileName { get; private set; }

	public HomeFurnitureStatic.CharaReaction charaReaction { get; private set; }

	public string photoTexturePath { get; private set; }

	public HomeFurnitureStatic(MstHomeFurnitureData mst)
	{
		this.id = mst.id;
		this.rarity = (ItemDef.Rarity)mst.rarity;
		this.category = (HomeFurnitureStatic.Category)mst.category;
		this.name = mst.name;
		this.flavorText = mst.flavorText;
		this.modelFileName = mst.modelFileName;
		this.charaReaction = (HomeFurnitureStatic.CharaReaction)mst.charaReactionId;
		this.photoTexturePath = "Texture2D/Photo/Card_Photo/card_photo_" + mst.photoId.ToString("0000");
	}

	public override int GetId()
	{
		return this.id;
	}

	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.FURNITURE;
	}

	public override string GetName()
	{
		return this.name;
	}

	public override string GetInfo()
	{
		return this.flavorText;
	}

	public override ItemDef.Rarity GetRarity()
	{
		return ItemDef.Rarity.STAR4;
	}

	public override int GetStackMax()
	{
		return 99999999;
	}

	public override string GetIconName()
	{
		return "Texture2D/Icon_Furniture/home_item_" + this.id.ToString("00000");
	}

	public override int GetSalePrice()
	{
		return 0;
	}

	public enum CharaReaction
	{
		INVALID,
		SIT,
		LIFT
	}

	public enum Category
	{
		INVALID,
		DESK,
		CHAIR,
		ORNAMENT,
		STORAGE,
		BED,
		ELECTRONICS,
		INTERIOR,
		CARPET,
		WINDOW
	}
}
