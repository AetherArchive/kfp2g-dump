using System;
using SGNFW.Mst;

// Token: 0x020000BB RID: 187
public class TreeHouseFurnitureStatic : ItemStaticBase
{
	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06000804 RID: 2052 RVA: 0x00035EE6 File Offset: 0x000340E6
	public TreeHouseFurnitureStatic.Category category
	{
		get
		{
			return (TreeHouseFurnitureStatic.Category)this.mstData.categoryMain;
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06000805 RID: 2053 RVA: 0x00035EF3 File Offset: 0x000340F3
	public int subCategory
	{
		get
		{
			return this.mstData.categorySub;
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06000806 RID: 2054 RVA: 0x00035F00 File Offset: 0x00034100
	public int sizeX
	{
		get
		{
			return this.mstData.sizeX;
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06000807 RID: 2055 RVA: 0x00035F0D File Offset: 0x0003410D
	public int sizeY
	{
		get
		{
			return this.mstData.sizeY;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06000808 RID: 2056 RVA: 0x00035F1A File Offset: 0x0003411A
	public int sizeHeightOrDepth
	{
		get
		{
			return this.mstData.sizeHeightOrDepth;
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06000809 RID: 2057 RVA: 0x00035F27 File Offset: 0x00034127
	public int locatorGoods
	{
		get
		{
			return this.mstData.locatorGoods;
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x0600080A RID: 2058 RVA: 0x00035F34 File Offset: 0x00034134
	public int charaCountReaction
	{
		get
		{
			return this.mstData.charaCountReaction;
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x0600080B RID: 2059 RVA: 0x00035F41 File Offset: 0x00034141
	public string modelFileName
	{
		get
		{
			return this.mstData.modelFileName;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x0600080C RID: 2060 RVA: 0x00035F4E File Offset: 0x0003414E
	public int charaActionId
	{
		get
		{
			return this.mstData.charaReactionId;
		}
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x0600080D RID: 2061 RVA: 0x00035F5B File Offset: 0x0003415B
	public int iconCharaId
	{
		get
		{
			return this.mstData.iconCharaId;
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x0600080E RID: 2062 RVA: 0x00035F68 File Offset: 0x00034168
	public string embedTexturePath
	{
		get
		{
			if (!string.IsNullOrEmpty(this.mstData.embedImageName))
			{
				return "Texture2D/" + this.mstData.embedImageName;
			}
			return "";
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x0600080F RID: 2063 RVA: 0x00035F97 File Offset: 0x00034197
	public string embedTexturePathSub
	{
		get
		{
			if (!string.IsNullOrEmpty(this.mstData.embedImageNameSub))
			{
				return "Texture2D/" + this.mstData.embedImageNameSub;
			}
			return "";
		}
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06000810 RID: 2064 RVA: 0x00035FC6 File Offset: 0x000341C6
	public int kizunaBonusExp
	{
		get
		{
			return this.mstData.kizunaBonusWeight;
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06000811 RID: 2065 RVA: 0x00035FD3 File Offset: 0x000341D3
	public TreeHouseFurnitureStatic.SpecialValue specialValue
	{
		get
		{
			return (TreeHouseFurnitureStatic.SpecialValue)this.mstData.specialValue;
		}
	}

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06000812 RID: 2066 RVA: 0x00035FE0 File Offset: 0x000341E0
	public string bgmFilepath
	{
		get
		{
			return this.mstData.bgmFilepath;
		}
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x06000813 RID: 2067 RVA: 0x00035FED File Offset: 0x000341ED
	public string bgmName
	{
		get
		{
			return this.mstData.bgmName;
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06000814 RID: 2068 RVA: 0x00035FFA File Offset: 0x000341FA
	public int machineId
	{
		get
		{
			return this.mstData.machineId;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06000815 RID: 2069 RVA: 0x00036007 File Offset: 0x00034207
	public string sortName
	{
		get
		{
			return this.mstData.reading;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06000816 RID: 2070 RVA: 0x00036014 File Offset: 0x00034214
	public string infoThumbnailPath
	{
		get
		{
			return string.Format("Texture2D/Image_TreeHouse/Interior_{0:000000}", this.GetId());
		}
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x0003602B File Offset: 0x0003422B
	public TreeHouseFurnitureStatic(MstMasterRoomFurnitureData mst)
	{
		this.mstData = mst;
		this.categoryName = (DataManagerTreeHouse.categoryList.ContainsKey(this.category) ? DataManagerTreeHouse.categoryList[this.category] : "");
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00036069 File Offset: 0x00034269
	public override int GetId()
	{
		return this.mstData.id;
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x00036076 File Offset: 0x00034276
	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.TREEHOUSE_FURNITURE;
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0003607A File Offset: 0x0003427A
	public override string GetName()
	{
		return this.mstData.name;
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00036087 File Offset: 0x00034287
	public override string GetInfo()
	{
		return this.mstData.flavorText;
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x00036094 File Offset: 0x00034294
	public override ItemDef.Rarity GetRarity()
	{
		return (ItemDef.Rarity)this.mstData.rarity;
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x000360A1 File Offset: 0x000342A1
	public override int GetStackMax()
	{
		return this.mstData.stackMax;
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x000360B0 File Offset: 0x000342B0
	public override string GetIconName()
	{
		if (!string.IsNullOrEmpty(this.mstData.iconTexturePath))
		{
			return "Texture2D/" + this.mstData.iconTexturePath;
		}
		return "Texture2D/Icon_Item/icon_item_" + this.mstData.id.ToString("00000");
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x00036104 File Offset: 0x00034304
	public override int GetSalePrice()
	{
		return 0;
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00036107 File Offset: 0x00034307
	public string GetCategoryName()
	{
		return this.categoryName;
	}

	// Token: 0x040006FF RID: 1791
	private MstMasterRoomFurnitureData mstData;

	// Token: 0x04000700 RID: 1792
	private string categoryName;

	// Token: 0x020007AF RID: 1967
	public enum Category
	{
		// Token: 0x04003414 RID: 13332
		INVALID,
		// Token: 0x04003415 RID: 13333
		LARGE_FURNITURE,
		// Token: 0x04003416 RID: 13334
		STAND,
		// Token: 0x04003417 RID: 13335
		GENERAL_MERCHANDISE,
		// Token: 0x04003418 RID: 13336
		RUG,
		// Token: 0x04003419 RID: 13337
		WALL_HANGINGS,
		// Token: 0x0400341A RID: 13338
		CURTAIN,
		// Token: 0x0400341B RID: 13339
		WALL_PAPER,
		// Token: 0x0400341C RID: 13340
		FLOOR_PAPER,
		// Token: 0x0400341D RID: 13341
		CEIL_LIGHT,
		// Token: 0x0400341E RID: 13342
		CEIL_DECO
	}

	// Token: 0x020007B0 RID: 1968
	public enum SpecialValue
	{
		// Token: 0x04003420 RID: 13344
		INVALID,
		// Token: 0x04003421 RID: 13345
		POSTER_BOARD,
		// Token: 0x04003422 RID: 13346
		CLOCK,
		// Token: 0x04003423 RID: 13347
		BOX,
		// Token: 0x04003424 RID: 13348
		CAMERA
	}
}
