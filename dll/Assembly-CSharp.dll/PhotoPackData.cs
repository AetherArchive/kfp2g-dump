using System;

// Token: 0x020000CF RID: 207
public class PhotoPackData
{
	// Token: 0x170001DB RID: 475
	// (get) Token: 0x0600093C RID: 2364 RVA: 0x0003A48F File Offset: 0x0003868F
	public long dataId
	{
		get
		{
			return this.dynamicData.dataId;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x0600093D RID: 2365 RVA: 0x0003A49C File Offset: 0x0003869C
	// (set) Token: 0x0600093E RID: 2366 RVA: 0x0003A4A4 File Offset: 0x000386A4
	public PhotoDynamicData dynamicData { get; private set; }

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x0600093F RID: 2367 RVA: 0x0003A4AD File Offset: 0x000386AD
	// (set) Token: 0x06000940 RID: 2368 RVA: 0x0003A4B5 File Offset: 0x000386B5
	public PhotoStaticData staticData { get; private set; }

	// Token: 0x06000941 RID: 2369 RVA: 0x0003A4BE File Offset: 0x000386BE
	public PhotoPackData(PhotoDynamicData dynamicData)
	{
		this.dynamicData = dynamicData;
		this.staticData = DataManager.DmPhoto.GetPhotoStaticData(this.dynamicData.photoId);
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0003A4E8 File Offset: 0x000386E8
	public PhotoPackData()
	{
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0003A4F0 File Offset: 0x000386F0
	public bool IsInvalid()
	{
		return this.dataId == 0L;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x0003A4FC File Offset: 0x000386FC
	public bool IsMine()
	{
		return this.dynamicData.OwnerType == PhotoDynamicData.PhotoOwnerType.User;
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x0003A50C File Offset: 0x0003870C
	public bool IsEnableSwitchCharacteristic()
	{
		return this.staticData.baseData.AbilityEffectChange && !this.IsUseMaxAbility();
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0003A52B File Offset: 0x0003872B
	public bool IsImageChange()
	{
		return this.staticData.baseData.imageChangeFlg && !this.dynamicData.imgRevertFlag && this.dynamicData.levelRank >= 4;
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x0003A561 File Offset: 0x00038761
	public bool IsEnableImageChange()
	{
		return this.staticData.baseData.imageChangeFlg && this.IsMine() && this.dynamicData.levelRank >= 4;
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06000948 RID: 2376 RVA: 0x0003A592 File Offset: 0x00038792
	public int limitLevel
	{
		get
		{
			return this.calcLimitLevel(this.dynamicData.levelRank);
		}
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x0003A5A5 File Offset: 0x000387A5
	public int calcLimitLevel(int levelRank)
	{
		return this.staticData.getLimitLevel(levelRank);
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0003A5B3 File Offset: 0x000387B3
	public bool IsUseMaxAbility()
	{
		return this.dynamicData.levelRank == 4;
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0003A5C3 File Offset: 0x000387C3
	public CharaStaticAbility GetCurrentAbility()
	{
		if (this.IsUseMaxAbility())
		{
			return this.staticData.abilityDataMax;
		}
		return this.staticData.abilityData;
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0003A5E4 File Offset: 0x000387E4
	public CharaStaticAbility GetSwitchAbility(bool max)
	{
		if (max)
		{
			return this.staticData.abilityDataMax;
		}
		return this.staticData.abilityData;
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0003A600 File Offset: 0x00038800
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

	// Token: 0x0600094E RID: 2382 RVA: 0x0003A638 File Offset: 0x00038838
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

	// Token: 0x0600094F RID: 2383 RVA: 0x0003A697 File Offset: 0x00038897
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

	// Token: 0x0400078E RID: 1934
	public const int INVALID_PHOTO_DATA_ID = 0;
}
