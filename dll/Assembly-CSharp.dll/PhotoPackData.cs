using System;

public class PhotoPackData
{
	public long dataId
	{
		get
		{
			return this.dynamicData.dataId;
		}
	}

	public PhotoDynamicData dynamicData { get; private set; }

	public PhotoStaticData staticData { get; private set; }

	public PhotoPackData(PhotoDynamicData dynamicData)
	{
		this.dynamicData = dynamicData;
		this.staticData = DataManager.DmPhoto.GetPhotoStaticData(this.dynamicData.photoId);
	}

	public PhotoPackData()
	{
	}

	public bool IsInvalid()
	{
		return this.dataId == 0L;
	}

	public bool IsMine()
	{
		return this.dynamicData.OwnerType == PhotoDynamicData.PhotoOwnerType.User;
	}

	public bool IsEnableSwitchCharacteristic()
	{
		return this.staticData.baseData.AbilityEffectChange && !this.IsUseMaxAbility();
	}

	public bool IsImageChange()
	{
		return this.staticData.baseData.imageChangeFlg && !this.dynamicData.imgRevertFlag && this.dynamicData.levelRank >= 4;
	}

	public bool IsEnableImageChange()
	{
		return this.staticData.baseData.imageChangeFlg && this.IsMine() && this.dynamicData.levelRank >= 4;
	}

	public int limitLevel
	{
		get
		{
			return this.calcLimitLevel(this.dynamicData.levelRank);
		}
	}

	public int calcLimitLevel(int levelRank)
	{
		return this.staticData.getLimitLevel(levelRank);
	}

	public bool IsUseMaxAbility()
	{
		return this.dynamicData.levelRank == 4;
	}

	public CharaStaticAbility GetCurrentAbility()
	{
		if (this.IsUseMaxAbility())
		{
			return this.staticData.abilityDataMax;
		}
		return this.staticData.abilityData;
	}

	public CharaStaticAbility GetSwitchAbility(bool max)
	{
		if (max)
		{
			return this.staticData.abilityDataMax;
		}
		return this.staticData.abilityData;
	}

	public static PhotoPackData MakeDummy(long dataId, int itemId)
	{
		return new PhotoPackData(new PhotoDynamicData
		{
			OwnerType = PhotoDynamicData.PhotoOwnerType.User,
			dataId = dataId,
			level = 1,
			exp = 50L,
			levelRank = 0,
			photoId = itemId
		});
	}

	public static PhotoPackData MakeMaximum(int itemId, bool isMaxLevelRank, bool isMaxLevel)
	{
		PhotoDynamicData photoDynamicData = new PhotoDynamicData();
		photoDynamicData.dataId = -1L;
		photoDynamicData.levelRank = (isMaxLevelRank ? 4 : 0);
		photoDynamicData.level = (isMaxLevel ? DataManager.DmPhoto.GetPhotoStaticData(itemId).getLimitLevel(photoDynamicData.levelRank) : 1);
		photoDynamicData.exp = 0L;
		photoDynamicData.photoId = itemId;
		return new PhotoPackData(photoDynamicData);
	}

	public static PhotoPackData MakeInvalid()
	{
		return new PhotoPackData
		{
			dynamicData = new PhotoDynamicData(),
			dynamicData = 
			{
				dataId = 0L
			}
		};
	}

	public const int INVALID_PHOTO_DATA_ID = 0;
}
