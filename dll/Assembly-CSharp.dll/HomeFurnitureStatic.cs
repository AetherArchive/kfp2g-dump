using System;
using SGNFW.Mst;

// Token: 0x02000083 RID: 131
public class HomeFurnitureStatic : ItemStaticBase
{
	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x060004EE RID: 1262 RVA: 0x00023136 File Offset: 0x00021336
	// (set) Token: 0x060004EF RID: 1263 RVA: 0x0002313E File Offset: 0x0002133E
	public int id { get; set; }

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00023147 File Offset: 0x00021347
	// (set) Token: 0x060004F1 RID: 1265 RVA: 0x0002314F File Offset: 0x0002134F
	public ItemDef.Rarity rarity { get; private set; }

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00023158 File Offset: 0x00021358
	// (set) Token: 0x060004F3 RID: 1267 RVA: 0x00023160 File Offset: 0x00021360
	public HomeFurnitureStatic.Category category { get; private set; }

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00023169 File Offset: 0x00021369
	// (set) Token: 0x060004F5 RID: 1269 RVA: 0x00023171 File Offset: 0x00021371
	public string name { get; private set; }

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x060004F6 RID: 1270 RVA: 0x0002317A File Offset: 0x0002137A
	// (set) Token: 0x060004F7 RID: 1271 RVA: 0x00023182 File Offset: 0x00021382
	public string flavorText { get; private set; }

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x060004F8 RID: 1272 RVA: 0x0002318B File Offset: 0x0002138B
	// (set) Token: 0x060004F9 RID: 1273 RVA: 0x00023193 File Offset: 0x00021393
	public string modelFileName { get; private set; }

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x060004FA RID: 1274 RVA: 0x0002319C File Offset: 0x0002139C
	// (set) Token: 0x060004FB RID: 1275 RVA: 0x000231A4 File Offset: 0x000213A4
	public HomeFurnitureStatic.CharaReaction charaReaction { get; private set; }

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x060004FC RID: 1276 RVA: 0x000231AD File Offset: 0x000213AD
	// (set) Token: 0x060004FD RID: 1277 RVA: 0x000231B5 File Offset: 0x000213B5
	public string photoTexturePath { get; private set; }

	// Token: 0x060004FE RID: 1278 RVA: 0x000231C0 File Offset: 0x000213C0
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

	// Token: 0x060004FF RID: 1279 RVA: 0x00023247 File Offset: 0x00021447
	public override int GetId()
	{
		return this.id;
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0002324F File Offset: 0x0002144F
	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.FURNITURE;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00023253 File Offset: 0x00021453
	public override string GetName()
	{
		return this.name;
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0002325B File Offset: 0x0002145B
	public override string GetInfo()
	{
		return this.flavorText;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x00023263 File Offset: 0x00021463
	public override ItemDef.Rarity GetRarity()
	{
		return ItemDef.Rarity.STAR4;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00023266 File Offset: 0x00021466
	public override int GetStackMax()
	{
		return 99999999;
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00023270 File Offset: 0x00021470
	public override string GetIconName()
	{
		return "Texture2D/Icon_Furniture/home_item_" + this.id.ToString("00000");
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x0002329A File Offset: 0x0002149A
	public override int GetSalePrice()
	{
		return 0;
	}

	// Token: 0x020006C4 RID: 1732
	public enum CharaReaction
	{
		// Token: 0x04003065 RID: 12389
		INVALID,
		// Token: 0x04003066 RID: 12390
		SIT,
		// Token: 0x04003067 RID: 12391
		LIFT
	}

	// Token: 0x020006C5 RID: 1733
	public enum Category
	{
		// Token: 0x04003069 RID: 12393
		INVALID,
		// Token: 0x0400306A RID: 12394
		DESK,
		// Token: 0x0400306B RID: 12395
		CHAIR,
		// Token: 0x0400306C RID: 12396
		ORNAMENT,
		// Token: 0x0400306D RID: 12397
		STORAGE,
		// Token: 0x0400306E RID: 12398
		BED,
		// Token: 0x0400306F RID: 12399
		ELECTRONICS,
		// Token: 0x04003070 RID: 12400
		INTERIOR,
		// Token: 0x04003071 RID: 12401
		CARPET,
		// Token: 0x04003072 RID: 12402
		WINDOW
	}
}
