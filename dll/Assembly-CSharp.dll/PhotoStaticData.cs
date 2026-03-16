using System;
using SGNFW.Mst;

public class PhotoStaticData : ItemStaticBase
{
	public string GetMiniIconName()
	{
		return "Texture2D/Photo/Icon_Photo/icon_photo_" + this.baseData.id.ToString("0000");
	}

	public string GetCardImageName()
	{
		return "Texture2D/Photo/Card_Photo/card_photo_" + this.baseData.id.ToString("0000");
	}

	public string GetQrImageName()
	{
		return "Texture2D/Photo/Qr_Photo/qr_photo_" + this.baseData.id.ToString("0000");
	}

	public int getLimitLevel(int levelRank)
	{
		switch (levelRank)
		{
		case 0:
			return this.rarityData.levelLimit;
		case 1:
			return this.rarityData.levelOverLimit00;
		case 2:
			return this.rarityData.levelOverLimit01;
		case 3:
			return this.rarityData.levelOverLimit02;
		case 4:
			return this.rarityData.levelOverLimit03;
		default:
			return this.rarityData.levelLimit;
		}
	}

	public override int GetId()
	{
		return this.baseData.id;
	}

	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.PHOTO;
	}

	public override string GetName()
	{
		return this.baseData.photoName;
	}

	public override string GetInfo()
	{
		return this.baseData.flavorText;
	}

	public override ItemDef.Rarity GetRarity()
	{
		return this.baseData.rarity;
	}

	public override int GetStackMax()
	{
		return 1;
	}

	public override string GetIconName()
	{
		return "Texture2D/Photo/Icon_Photo/icon_photo_" + this.baseData.id.ToString("0000");
	}

	public override int GetSalePrice()
	{
		return this.rarityData.sellPrice;
	}

	public PhotoStaticBase baseData;

	public MstPhotoRarityData rarityData;

	public CharaStaticAbility abilityData;

	public CharaStaticAbility abilityDataMax;
}
