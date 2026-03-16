using System;
using SGNFW.Mst;

public class TreeHouseFurnitureStatic : ItemStaticBase
{
	public TreeHouseFurnitureStatic.Category category
	{
		get
		{
			return (TreeHouseFurnitureStatic.Category)this.mstData.categoryMain;
		}
	}

	public int subCategory
	{
		get
		{
			return this.mstData.categorySub;
		}
	}

	public int sizeX
	{
		get
		{
			return this.mstData.sizeX;
		}
	}

	public int sizeY
	{
		get
		{
			return this.mstData.sizeY;
		}
	}

	public int sizeHeightOrDepth
	{
		get
		{
			return this.mstData.sizeHeightOrDepth;
		}
	}

	public int locatorGoods
	{
		get
		{
			return this.mstData.locatorGoods;
		}
	}

	public int charaCountReaction
	{
		get
		{
			return this.mstData.charaCountReaction;
		}
	}

	public string modelFileName
	{
		get
		{
			return this.mstData.modelFileName;
		}
	}

	public int charaActionId
	{
		get
		{
			return this.mstData.charaReactionId;
		}
	}

	public int iconCharaId
	{
		get
		{
			return this.mstData.iconCharaId;
		}
	}

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

	public int kizunaBonusExp
	{
		get
		{
			return this.mstData.kizunaBonusWeight;
		}
	}

	public TreeHouseFurnitureStatic.SpecialValue specialValue
	{
		get
		{
			return (TreeHouseFurnitureStatic.SpecialValue)this.mstData.specialValue;
		}
	}

	public string bgmFilepath
	{
		get
		{
			return this.mstData.bgmFilepath;
		}
	}

	public string bgmName
	{
		get
		{
			return this.mstData.bgmName;
		}
	}

	public int machineId
	{
		get
		{
			return this.mstData.machineId;
		}
	}

	public string sortName
	{
		get
		{
			return this.mstData.reading;
		}
	}

	public string infoThumbnailPath
	{
		get
		{
			return string.Format("Texture2D/Image_TreeHouse/Interior_{0:000000}", this.GetId());
		}
	}

	public TreeHouseFurnitureStatic(MstMasterRoomFurnitureData mst)
	{
		this.mstData = mst;
		this.categoryName = (DataManagerTreeHouse.categoryList.ContainsKey(this.category) ? DataManagerTreeHouse.categoryList[this.category] : "");
	}

	public override int GetId()
	{
		return this.mstData.id;
	}

	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.TREEHOUSE_FURNITURE;
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
		return this.mstData.stackMax;
	}

	public override string GetIconName()
	{
		if (!string.IsNullOrEmpty(this.mstData.iconTexturePath))
		{
			return "Texture2D/" + this.mstData.iconTexturePath;
		}
		return "Texture2D/Icon_Item/icon_item_" + this.mstData.id.ToString("00000");
	}

	public override int GetSalePrice()
	{
		return 0;
	}

	public string GetCategoryName()
	{
		return this.categoryName;
	}

	private MstMasterRoomFurnitureData mstData;

	private string categoryName;

	public enum Category
	{
		INVALID,
		LARGE_FURNITURE,
		STAND,
		GENERAL_MERCHANDISE,
		RUG,
		WALL_HANGINGS,
		CURTAIN,
		WALL_PAPER,
		FLOOR_PAPER,
		CEIL_LIGHT,
		CEIL_DECO
	}

	public enum SpecialValue
	{
		INVALID,
		POSTER_BOARD,
		CLOCK,
		BOX,
		CAMERA
	}
}
