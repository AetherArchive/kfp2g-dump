using System;
using SGNFW.Mst;

// Token: 0x020000D1 RID: 209
public class PhotoStaticData : ItemStaticBase
{
	// Token: 0x06000955 RID: 2389 RVA: 0x0003A838 File Offset: 0x00038A38
	public string GetMiniIconName()
	{
		return "Texture2D/Photo/Icon_Photo/icon_photo_" + this.baseData.id.ToString("0000");
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0003A868 File Offset: 0x00038A68
	public string GetCardImageName()
	{
		return "Texture2D/Photo/Card_Photo/card_photo_" + this.baseData.id.ToString("0000");
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0003A898 File Offset: 0x00038A98
	public string GetQrImageName()
	{
		return "Texture2D/Photo/Qr_Photo/qr_photo_" + this.baseData.id.ToString("0000");
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0003A8C8 File Offset: 0x00038AC8
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

	// Token: 0x06000959 RID: 2393 RVA: 0x0003A938 File Offset: 0x00038B38
	public override int GetId()
	{
		return this.baseData.id;
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0003A945 File Offset: 0x00038B45
	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.PHOTO;
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0003A948 File Offset: 0x00038B48
	public override string GetName()
	{
		return this.baseData.photoName;
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0003A955 File Offset: 0x00038B55
	public override string GetInfo()
	{
		return this.baseData.flavorText;
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0003A962 File Offset: 0x00038B62
	public override ItemDef.Rarity GetRarity()
	{
		return this.baseData.rarity;
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x0003A96F File Offset: 0x00038B6F
	public override int GetStackMax()
	{
		return 1;
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0003A974 File Offset: 0x00038B74
	public override string GetIconName()
	{
		return "Texture2D/Photo/Icon_Photo/icon_photo_" + this.baseData.id.ToString("0000");
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0003A9A3 File Offset: 0x00038BA3
	public override int GetSalePrice()
	{
		return this.rarityData.sellPrice;
	}

	// Token: 0x0400079B RID: 1947
	public PhotoStaticBase baseData;

	// Token: 0x0400079C RID: 1948
	public MstPhotoRarityData rarityData;

	// Token: 0x0400079D RID: 1949
	public CharaStaticAbility abilityData;

	// Token: 0x0400079E RID: 1950
	public CharaStaticAbility abilityDataMax;
}
