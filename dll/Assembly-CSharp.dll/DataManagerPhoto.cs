using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x02000097 RID: 151
public class DataManagerPhoto
{
	// Token: 0x060005F8 RID: 1528 RVA: 0x000289E2 File Offset: 0x00026BE2
	public DataManagerPhoto(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x060005F9 RID: 1529 RVA: 0x00028A12 File Offset: 0x00026C12
	// (set) Token: 0x060005FA RID: 1530 RVA: 0x00028A1A File Offset: 0x00026C1A
	public List<DataManagerPhoto.PhotoDropItemData> PhotoQuestDropItemList { get; private set; }

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x060005FB RID: 1531 RVA: 0x00028A23 File Offset: 0x00026C23
	// (set) Token: 0x060005FC RID: 1532 RVA: 0x00028A2B File Offset: 0x00026C2B
	public List<DataManagerPhoto.AlbumData> AlbumDataList { get; private set; }

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x060005FD RID: 1533 RVA: 0x00028A34 File Offset: 0x00026C34
	// (set) Token: 0x060005FE RID: 1534 RVA: 0x00028A3C File Offset: 0x00026C3C
	private bool PhotoAlbumUpdated { get; set; }

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x060005FF RID: 1535 RVA: 0x00028A45 File Offset: 0x00026C45
	public int PhotoStockLimit
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.photoStockLimit + this.getAdditionalPhotoStock();
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000600 RID: 1536 RVA: 0x00028A5D File Offset: 0x00026C5D
	// (set) Token: 0x06000601 RID: 1537 RVA: 0x00028A65 File Offset: 0x00026C65
	private List<DataManagerPhoto.PhotoCharacteristicData> PhotoCharacteristicDataList { get; set; }

	// Token: 0x06000602 RID: 1538 RVA: 0x00028A6E File Offset: 0x00026C6E
	public PhotoStaticData GetPhotoStaticData(int photoId)
	{
		if (!this.photoStaticMap.ContainsKey(photoId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerPhoto.GetPhotoStaticData : 定義されていないID[" + photoId.ToString() + "]を生成しようとしました", null);
			return null;
		}
		return this.photoStaticMap[photoId];
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x00028AA8 File Offset: 0x00026CA8
	public Dictionary<int, PhotoStaticData> GetPhotoStaticMap()
	{
		return this.photoStaticMap;
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x00028AB0 File Offset: 0x00026CB0
	public int GetPhotoBonuExpData(ItemDef.Rarity rarity)
	{
		MstPhotoGrowbonusData mstPhotoGrowbonusData = this.mstPhotoGrowBonusDataList.Find((MstPhotoGrowbonusData x) => x.rarity == (int)rarity);
		if (mstPhotoGrowbonusData == null)
		{
			return 0;
		}
		return mstPhotoGrowbonusData.bonus;
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x00028AED File Offset: 0x00026CED
	public PhotoPackData GetUserPhotoData(long photoDataId)
	{
		if (this.userPhotoMap.ContainsKey(photoDataId))
		{
			return this.userPhotoMap[photoDataId];
		}
		return null;
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x00028B0B File Offset: 0x00026D0B
	public Dictionary<long, PhotoPackData> GetUserPhotoMap()
	{
		return this.userPhotoMap;
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x00028B13 File Offset: 0x00026D13
	public int GetHaveNumByPhotoItemId(int photoItemId)
	{
		return this.userHaveNumByPhotoItemId.TryGetValueEx(photoItemId, 0);
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00028B22 File Offset: 0x00026D22
	public int getAdditionalPhotoStock()
	{
		return DataManager.DmItem.GetUserItemData(DataManagerPhoto.PHOTO_STOCK_RELEASEITEM_ID).num;
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x00028B38 File Offset: 0x00026D38
	public List<DataManagerPhoto.PhotoCharacteristicData> GetPhotoCharacteristicDataList()
	{
		List<DataManagerPhoto.PhotoCharacteristicData> list = new List<DataManagerPhoto.PhotoCharacteristicData>();
		DateTime now = TimeManager.Now;
		if (this.PhotoCharacteristicDataList == null)
		{
			return null;
		}
		foreach (DataManagerPhoto.PhotoCharacteristicData photoCharacteristicData in this.PhotoCharacteristicDataList)
		{
			if (photoCharacteristicData.StartDatetime < now)
			{
				list.Add(photoCharacteristicData);
			}
		}
		return list;
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x00028BB0 File Offset: 0x00026DB0
	public void UpdateUserDataByServer(List<Photo> havePhotoList)
	{
		using (List<Photo>.Enumerator enumerator = havePhotoList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Photo havePhoto = enumerator.Current;
				if (this.userPhotoMap.ContainsKey(havePhoto.photo_id))
				{
					int num = havePhoto.manage_status;
					if (num != 0)
					{
						if (num - 1 > 1)
						{
							this.userPhotoMap[havePhoto.photo_id].dynamicData.UpdateByServer(havePhoto);
						}
						else
						{
							this.userPhotoMap.Remove(havePhoto.photo_id);
							if (this.userHaveNumByPhotoItemId.ContainsKey(havePhoto.item_id))
							{
								Dictionary<int, int> dictionary = this.userHaveNumByPhotoItemId;
								int item_id = havePhoto.item_id;
								int num2 = dictionary[item_id];
								dictionary[item_id] = num2 - 1;
							}
							else
							{
								this.userHaveNumByPhotoItemId[havePhoto.item_id] = 0;
							}
						}
					}
					else
					{
						PhotoPackData photoPackData = this.userPhotoMap[havePhoto.photo_id];
						photoPackData.dynamicData.UpdateByServer(havePhoto);
						if (this.AlbumDataList != null && !this.PhotoAlbumUpdated)
						{
							DataManagerPhoto.AlbumData albumData = this.AlbumDataList.Find((DataManagerPhoto.AlbumData x) => x.PhotoId == havePhoto.item_id);
							if (4 != albumData.LimitOverNum && 4 == havePhoto.limit_over_num)
							{
								this.PhotoAlbumUpdated = true;
							}
							else if (!albumData.IsLevelMax && havePhoto.level >= photoPackData.staticData.getLimitLevel(4))
							{
								this.PhotoAlbumUpdated = true;
							}
						}
					}
				}
				else
				{
					PhotoDynamicData photoDynamicData = new PhotoDynamicData();
					photoDynamicData.OwnerType = PhotoDynamicData.PhotoOwnerType.User;
					photoDynamicData.UpdateByServer(havePhoto);
					this.userPhotoMap.Add(havePhoto.photo_id, new PhotoPackData(photoDynamicData));
					if (this.userHaveNumByPhotoItemId.ContainsKey(havePhoto.item_id))
					{
						Dictionary<int, int> dictionary2 = this.userHaveNumByPhotoItemId;
						int num = havePhoto.item_id;
						int num2 = dictionary2[num];
						dictionary2[num] = num2 + 1;
					}
					else
					{
						this.userHaveNumByPhotoItemId[havePhoto.item_id] = 1;
					}
					if (this.AlbumDataList != null && !this.PhotoAlbumUpdated && this.AlbumDataList.Find((DataManagerPhoto.AlbumData x) => x.PhotoId == havePhoto.item_id) == null)
					{
						this.PhotoAlbumUpdated = true;
					}
				}
			}
		}
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x00028E64 File Offset: 0x00027064
	public void UpdateUserDataByTutorial(List<int> setPhotoList)
	{
		this.userPhotoMap.Clear();
		this.userHaveNumByPhotoItemId.Clear();
		for (int i = 0; i < setPhotoList.Count; i++)
		{
			this.userPhotoMap.Add((long)(-(long)(i + 1)), PhotoPackData.MakeDummy((long)(-(long)(i + 1)), setPhotoList[i]));
			if (!this.userHaveNumByPhotoItemId.ContainsKey(setPhotoList[i]))
			{
				this.userHaveNumByPhotoItemId[setPhotoList[i]] = 1;
			}
			Dictionary<int, int> dictionary = this.userHaveNumByPhotoItemId;
			int num = setPhotoList[i];
			int num2 = dictionary[num];
			dictionary[num] = num2 + 1;
		}
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x00028F00 File Offset: 0x00027100
	public List<DataManagerPhoto.PhotoDropItemData> GetPhotoQuestDropItemList(DateTime targetDateTime)
	{
		return this.PhotoQuestDropItemList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.StartDateTime <= targetDateTime && targetDateTime <= x.EndDateTime);
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00028F31 File Offset: 0x00027131
	public DataManagerPhoto.PhotoLevelupResult GetPhotoGrowResult()
	{
		return this.photoLevelupResult;
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00028F3C File Offset: 0x0002713C
	public long GetExpByNextLevel(int photoId, int nowLevel, int nowLevelRank)
	{
		PhotoStaticData photoStaticData = this.GetPhotoStaticData(photoId);
		int num = nowLevel - 1 + 1;
		if (nowLevel >= photoStaticData.getLimitLevel(nowLevelRank))
		{
			return 0L;
		}
		if (num < DataManager.DmServerMst.gameLevelInfoList.Count)
		{
			return DataManager.DmServerMst.gameLevelInfoList[num].photoLevelExp[photoStaticData.baseData.levelTableId];
		}
		return 0L;
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x00028F9E File Offset: 0x0002719E
	public long GetExpByNextLevel(PhotoPackData ppd)
	{
		return this.GetExpByNextLevel(ppd.staticData.GetId(), ppd.dynamicData.level, ppd.dynamicData.levelRank);
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x00028FC7 File Offset: 0x000271C7
	public long GetExpByNextLevel(DataManagerPhoto.PhotoLevelupResult plr, bool isBefore)
	{
		return this.GetExpByNextLevel(plr.itemId, isBefore ? plr.befLevel : plr.level, isBefore ? plr.befLevelRank : plr.levelRank);
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x00028FF8 File Offset: 0x000271F8
	public void InitializeMstData(MstManager mstManager)
	{
		List<MstPhotoData> mst = Singleton<MstManager>.Instance.GetMst<List<MstPhotoData>>(MstType.PHOTO_DATA);
		List<MstPhotoRarityData> mst2 = Singleton<MstManager>.Instance.GetMst<List<MstPhotoRarityData>>(MstType.PHOTO_RARITY_DATA);
		Singleton<MstManager>.Instance.GetMst<List<MstAppConfig>>(MstType.APP_CONFIG);
		List<MstQuestPhotoDropItemData> mst3 = mstManager.GetMst<List<MstQuestPhotoDropItemData>>(MstType.QUEST_PHOTO_DROP_ITEM_DATA);
		List<MstPhotoCharacteristicData> mst4 = mstManager.GetMst<List<MstPhotoCharacteristicData>>(MstType.PHOTO_CHARACTERISTIC_DATA);
		this.mstPhotoGrowBonusDataList = mstManager.GetMst<List<MstPhotoGrowbonusData>>(MstType.PHOTO_GROWBONUS_DATA);
		this.mstItemBonusDataList = Singleton<MstManager>.Instance.GetMst<List<MstItemBonusData>>(MstType.ITEM_BONUS_DATA);
		this.mstItemBonusMapByPhotoId = new Dictionary<int, List<MstItemBonusData>>();
		foreach (MstItemBonusData mstItemBonusData in this.mstItemBonusDataList)
		{
			if (!this.mstItemBonusMapByPhotoId.ContainsKey(mstItemBonusData.bonusPhotoId))
			{
				this.mstItemBonusMapByPhotoId.Add(mstItemBonusData.bonusPhotoId, new List<MstItemBonusData>());
			}
			this.mstItemBonusMapByPhotoId[mstItemBonusData.bonusPhotoId].Add(mstItemBonusData);
		}
		PrjUtil.ConvertTicksToTime(TimeManager.Now.Ticks);
		List<ItemStaticBase> list = new List<ItemStaticBase>();
		this.photoStaticMap.Clear();
		using (List<MstPhotoData>.Enumerator enumerator2 = mst.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				MstPhotoData photoData = enumerator2.Current;
				PhotoStaticData photoStaticData = new PhotoStaticData();
				photoStaticData.baseData = new PhotoStaticBase(photoData);
				photoStaticData.rarityData = mst2.Find((MstPhotoRarityData item) => item.rarity == photoData.rarity);
				string[] array = new string[]
				{
					"ParamAbility_" + photoData.id.ToString("0000") + "_1",
					"ParamAbility_" + photoData.id.ToString("0000") + "_2"
				};
				for (int i = 0; i < array.Length; i++)
				{
					CharaStaticAbility charaStaticAbility = AssetManager.GetAssetData("Charas/Parameter/Ability/" + array[i]) as CharaStaticAbility;
					if (i == 0)
					{
						photoStaticData.abilityData = charaStaticAbility;
					}
					else
					{
						photoStaticData.abilityDataMax = charaStaticAbility;
					}
				}
				this.photoStaticMap.Add(photoData.id, photoStaticData);
				list.Add(photoStaticData);
			}
		}
		DataManager.DmItem.AddMstDataByItem(list);
		this.PhotoQuestDropItemList = new List<DataManagerPhoto.PhotoDropItemData>();
		foreach (MstQuestPhotoDropItemData mstQuestPhotoDropItemData in mst3)
		{
			DataManagerPhoto.PhotoDropItemData photoDropItemData = new DataManagerPhoto.PhotoDropItemData(mstQuestPhotoDropItemData);
			this.PhotoQuestDropItemList.Add(photoDropItemData);
		}
		this.PhotoAlbumUpdated = true;
		this.PhotoCharacteristicDataList = new List<DataManagerPhoto.PhotoCharacteristicData>();
		using (List<MstPhotoCharacteristicData>.Enumerator enumerator4 = mst4.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				MstPhotoCharacteristicData photoCharacteristicData = enumerator4.Current;
				if (!(string.Empty == photoCharacteristicData.labelText))
				{
					DataManagerPhoto.PhotoCharacteristicData photoCharacteristicData2 = this.PhotoCharacteristicDataList.Find((DataManagerPhoto.PhotoCharacteristicData x) => x.DisplayName == photoCharacteristicData.labelText);
					if (photoCharacteristicData2 == null)
					{
						this.PhotoCharacteristicDataList.Add(new DataManagerPhoto.PhotoCharacteristicData(photoCharacteristicData));
					}
					else
					{
						photoCharacteristicData2.AdditionalElement(photoCharacteristicData);
					}
				}
			}
		}
		this.PhotoCharacteristicDataList.Sort((DataManagerPhoto.PhotoCharacteristicData a, DataManagerPhoto.PhotoCharacteristicData b) => a.Priority - b.Priority);
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x000293C0 File Offset: 0x000275C0
	private List<DataManagerPhoto.CalcDropBonusResult> InternalCalcPhotoBonus(List<PhotoPackData> photoPackList, DateTime targetTime, CharaPackData charaPack, bool isClamp)
	{
		List<DataManagerPhoto.CalcDropBonusResult> list = new List<DataManagerPhoto.CalcDropBonusResult>();
		long num = PrjUtil.ConvertTicksToTime(targetTime.Ticks);
		for (int i = 0; i < photoPackList.Count; i++)
		{
			PhotoPackData photoPackData = photoPackList[i];
			if (photoPackData != null && photoPackData.staticData != null)
			{
				List<MstItemBonusData> list2 = this.mstItemBonusMapByPhotoId.TryGetValueEx(photoPackData.staticData.GetId(), new List<MstItemBonusData>());
				if (charaPack == null || (charaPack.dynamicData.PhotoPocket[i].Flag && (!photoPackData.staticData.baseData.kizunaPhotoFlg || photoPackData.staticData.GetId() == charaPack.staticData.baseData.kizunaPhotoId)))
				{
					int num2 = photoPackData.dynamicData.levelRank + 1;
					using (List<MstItemBonusData>.Enumerator enumerator = list2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MstItemBonusData mstItemBonus = enumerator.Current;
							if (mstItemBonus.startDatetime <= num && mstItemBonus.endDatetime >= num)
							{
								DataManagerPhoto.CalcDropBonusResult calcDropBonusResult = list.Find((DataManagerPhoto.CalcDropBonusResult item) => item.targetItemId == mstItemBonus.increaseItemId);
								if (calcDropBonusResult == null)
								{
									calcDropBonusResult = new DataManagerPhoto.CalcDropBonusResult();
									calcDropBonusResult.targetItemId = mstItemBonus.increaseItemId;
									calcDropBonusResult.targetItemBonusRatio = mstItemBonus.bonusRatio;
									list.Add(calcDropBonusResult);
								}
								calcDropBonusResult.ratio += mstItemBonus.bonusRatio * num2;
								if (isClamp && calcDropBonusResult.ratio >= mstItemBonus.bonusRatio * 5)
								{
									calcDropBonusResult.ratio = mstItemBonus.bonusRatio * 5;
								}
							}
						}
					}
				}
			}
		}
		list.Sort((DataManagerPhoto.CalcDropBonusResult a, DataManagerPhoto.CalcDropBonusResult b) => b.targetItemId - a.targetItemId);
		return list;
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x000295C8 File Offset: 0x000277C8
	private List<DataManagerPhoto.CalcDropBonusResult> InternalCalcPhotoBonus(PhotoPackData basePhotoPack, List<PhotoPackData> photoPackList, DateTime targetTime)
	{
		List<DataManagerPhoto.CalcDropBonusResult> list = new List<DataManagerPhoto.CalcDropBonusResult>();
		long num = PrjUtil.ConvertTicksToTime(targetTime.Ticks);
		for (int i = 0; i < photoPackList.Count; i++)
		{
			PhotoPackData photoPackData = photoPackList[i];
			if (photoPackData != null && photoPackData.staticData != null)
			{
				List<MstItemBonusData> list2 = this.mstItemBonusMapByPhotoId.TryGetValueEx(basePhotoPack.staticData.GetId(), new List<MstItemBonusData>());
				int num2 = photoPackData.dynamicData.levelRank + 1;
				using (List<MstItemBonusData>.Enumerator enumerator = list2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MstItemBonusData mstItemBonus = enumerator.Current;
						if (mstItemBonus.startDatetime <= num && mstItemBonus.endDatetime >= num)
						{
							DataManagerPhoto.CalcDropBonusResult calcDropBonusResult = list.Find((DataManagerPhoto.CalcDropBonusResult item) => item.targetItemId == mstItemBonus.increaseItemId);
							if (calcDropBonusResult == null)
							{
								calcDropBonusResult = new DataManagerPhoto.CalcDropBonusResult();
								calcDropBonusResult.targetItemId = mstItemBonus.increaseItemId;
								calcDropBonusResult.targetItemBonusRatio = mstItemBonus.bonusRatio;
								list.Add(calcDropBonusResult);
							}
							calcDropBonusResult.ratio += mstItemBonus.bonusRatio * num2;
							if (calcDropBonusResult.ratio >= mstItemBonus.bonusRatio * 5)
							{
								calcDropBonusResult.ratio = mstItemBonus.bonusRatio * 5;
							}
						}
					}
				}
			}
		}
		list.Sort((DataManagerPhoto.CalcDropBonusResult a, DataManagerPhoto.CalcDropBonusResult b) => b.targetItemId - a.targetItemId);
		return list;
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x00029770 File Offset: 0x00027970
	public List<DataManagerPhoto.CalcDropBonusResult> CalcPhotoBonus(List<PhotoPackData> photoPackList, DateTime targetTime, CharaPackData charaPack = null)
	{
		return this.InternalCalcPhotoBonus(photoPackList, targetTime, charaPack, false);
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0002977C File Offset: 0x0002797C
	public List<DataManagerPhoto.CalcDropBonusResult> CalcPhotoClampBonus(PhotoPackData basePhotoPack, List<PhotoPackData> photoPackList, DateTime targetTime)
	{
		return this.InternalCalcPhotoBonus(basePhotoPack, photoPackList, targetTime);
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00029788 File Offset: 0x00027988
	public bool IsEnablePhotoBonusByTime(int photoId, DateTime targetTime, int playQuestOneId = -1)
	{
		List<int> photoBonusTargetItemIdByTime = this.GetPhotoBonusTargetItemIdByTime(photoId, targetTime);
		if (photoBonusTargetItemIdByTime.Count <= 0)
		{
			return false;
		}
		if (playQuestOneId == -1)
		{
			return true;
		}
		QuestStaticQuestOne questStaticQuestOne = DataManager.DmQuest.QuestStaticData.oneDataMap.TryGetValueEx(playQuestOneId, null);
		if (questStaticQuestOne != null)
		{
			foreach (int num in photoBonusTargetItemIdByTime)
			{
				if (questStaticQuestOne.DropItemList.Contains(num))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x0002981C File Offset: 0x00027A1C
	public List<int> GetPhotoBonusTargetItemIdByTime(int photoId, DateTime targetTime)
	{
		long num = PrjUtil.ConvertTicksToTime(targetTime.Ticks);
		List<int> list = new List<int>();
		foreach (MstItemBonusData mstItemBonusData in this.mstItemBonusMapByPhotoId.TryGetValueEx(photoId, new List<MstItemBonusData>()))
		{
			if (mstItemBonusData.startDatetime <= num && mstItemBonusData.endDatetime >= num)
			{
				list.Add(mstItemBonusData.increaseItemId);
			}
		}
		return list;
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x000298A8 File Offset: 0x00027AA8
	public List<int> GetPhotoBonusTargetItemIdByTime(DateTime targetTime)
	{
		long num = PrjUtil.ConvertTicksToTime(targetTime.Ticks);
		List<int> list = new List<int>();
		foreach (MstItemBonusData mstItemBonusData in this.mstItemBonusDataList)
		{
			if (mstItemBonusData.startDatetime <= num && mstItemBonusData.endDatetime >= num && !list.Contains(mstItemBonusData.increaseItemId))
			{
				list.Add(mstItemBonusData.increaseItemId);
			}
		}
		return list;
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x00029934 File Offset: 0x00027B34
	public HashSet<int> GetEnablePhotoBonusByTime(DateTime targetTime)
	{
		HashSet<int> hashSet = new HashSet<int>();
		long num = PrjUtil.ConvertTicksToTime(targetTime.Ticks);
		foreach (MstItemBonusData mstItemBonusData in this.mstItemBonusDataList)
		{
			if (mstItemBonusData.startDatetime <= num && mstItemBonusData.endDatetime >= num)
			{
				hashSet.Add(mstItemBonusData.bonusPhotoId);
			}
		}
		return hashSet;
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x000299B4 File Offset: 0x00027BB4
	public bool IsBonusActive()
	{
		long num = PrjUtil.ConvertTicksToTime(TimeManager.Now.Ticks);
		bool flag = false;
		this.mstItemBonusDataList = Singleton<MstManager>.Instance.GetMst<List<MstItemBonusData>>(MstType.ITEM_BONUS_DATA);
		if (this.mstItemBonusDataList != null)
		{
			foreach (MstItemBonusData mstItemBonusData in this.mstItemBonusDataList)
			{
				if (mstItemBonusData.startDatetime <= num && mstItemBonusData.endDatetime >= num)
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x00029A4C File Offset: 0x00027C4C
	public int ComparePhotoPackDataNyName(PhotoPackData a, PhotoPackData b)
	{
		return this.ComparePhotoPackDataNyName(new string[]
		{
			a.staticData.baseData.Reading,
			a.staticData.GetName()
		}, new string[]
		{
			b.staticData.baseData.Reading,
			b.staticData.GetName()
		});
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x00029AB0 File Offset: 0x00027CB0
	public int ComparePhotoPackDataNyName(string[] dataA, string[] dataB)
	{
		int num = 0;
		char[] array = dataA[0].ToCharArray();
		char[] array2 = dataB[0].ToCharArray();
		int num2 = ((array.Length > array2.Length) ? array2.Length : array.Length);
		int i = 0;
		while (i < num2)
		{
			num = (int)(array2[i] - array[i]);
			if (num < 0)
			{
				if (char.IsDigit(array2[i]))
				{
					if (DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsAlphabet|54_0(array[i]) || DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsHiragana|54_1(array[i]))
					{
						num = 1;
						break;
					}
					break;
				}
				else if (DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsAlphabet|54_0(array2[i]))
				{
					if (DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsHiragana|54_1(array[i]))
					{
						num = 1;
						break;
					}
					break;
				}
				else
				{
					if (!DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsHiragana|54_1(array2[i]) && (char.IsDigit(array[i]) || DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsAlphabet|54_0(array[i]) || DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsHiragana|54_1(array[i])))
					{
						num = 1;
						break;
					}
					break;
				}
			}
			else if (num > 0)
			{
				if (char.IsDigit(array2[i]))
				{
					if (!char.IsDigit(array[i]) && !DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsAlphabet|54_0(array[i]) && !DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsHiragana|54_1(array[i]))
					{
						num = -1;
						break;
					}
					break;
				}
				else if (DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsAlphabet|54_0(array2[i]))
				{
					if (char.IsDigit(array[i]))
					{
						num = -1;
						break;
					}
					if (!DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsAlphabet|54_0(array[i]) && !DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsHiragana|54_1(array[i]))
					{
						num = -1;
						break;
					}
					break;
				}
				else
				{
					if (!DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsHiragana|54_1(array2[i]))
					{
						break;
					}
					if (char.IsDigit(array[i]) || DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsAlphabet|54_0(array[i]))
					{
						num = -1;
						break;
					}
					if (!DataManagerPhoto.<ComparePhotoPackDataNyName>g__IsHiragana|54_1(array[i]))
					{
						num = -1;
						break;
					}
					break;
				}
			}
			else
			{
				i++;
			}
		}
		if (num == 0)
		{
			num = array.Length - array2.Length;
		}
		if (num == 0)
		{
			char[] array3 = dataA[1].ToCharArray();
			char[] array4 = dataB[1].ToCharArray();
			int num3 = ((array3.Length > array4.Length) ? array4.Length : array3.Length);
			for (int j = 0; j < num3; j++)
			{
				num = (int)(array4[j] - array3[j]);
				if (num != 0)
				{
					break;
				}
			}
			if (num == 0)
			{
				num = array3.Length - array4.Length;
			}
		}
		return num;
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00029C98 File Offset: 0x00027E98
	public int ComparePhotoPackDataByType(PhotoPackData a, PhotoPackData b)
	{
		PhotoDef.Type type = a.staticData.baseData.type;
		PhotoDef.Type type2 = b.staticData.baseData.type;
		int num;
		if (PhotoDef.Type.OTHER == type && PhotoDef.Type.OTHER == type2)
		{
			num = b.staticData.baseData.expPhotoType - a.staticData.baseData.expPhotoType;
		}
		else if (PhotoDef.Type.OTHER != type && PhotoDef.Type.OTHER != type2)
		{
			if (b.staticData.baseData.kizunaPhotoFlg && !a.staticData.baseData.kizunaPhotoFlg)
			{
				num = 1;
			}
			else if (!b.staticData.baseData.kizunaPhotoFlg && a.staticData.baseData.kizunaPhotoFlg)
			{
				num = -1;
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			num = type2 - type;
		}
		return num;
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00029D58 File Offset: 0x00027F58
	public void RequestActionPhotoGrow(PhotoPackData basePhotoData, List<PhotoPackData> feedPhotoData)
	{
		this.parentData.ServerRequest(PhotoGrowCmd.Create(basePhotoData.dynamicData.dataId, feedPhotoData.Select<PhotoPackData, long>((PhotoPackData item) => item.dataId).ToList<long>()), new Action<Command>(this.CbPhotoGrowCmd));
		this.photoLevelupResult = new DataManagerPhoto.PhotoLevelupResult();
		this.photoLevelupResult.SetBasePhoto(basePhotoData);
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x00029DD0 File Offset: 0x00027FD0
	public void RequestActionPhotoSale(List<PhotoPackData> targetPhotoList)
	{
		this.parentData.ServerRequest(PhotoSellCmd.Create(targetPhotoList.Select<PhotoPackData, long>((PhotoPackData item) => item.dataId).ToList<long>()), new Action<Command>(this.CbPhotoSellCmd));
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x00029E24 File Offset: 0x00028024
	public void RequestActionPhotoUpdateStatus(long photoDataId, bool isLock, bool isRevert, bool isFavorite)
	{
		List<long> list = new List<long>();
		List<long> list2 = new List<long>();
		if (isLock)
		{
			list.Add(photoDataId);
		}
		else
		{
			list2.Add(photoDataId);
		}
		List<long> list3 = new List<long>();
		List<long> list4 = new List<long>();
		if (isRevert)
		{
			list3.Add(photoDataId);
		}
		else
		{
			list4.Add(photoDataId);
		}
		List<long> list5 = new List<long>();
		List<long> list6 = new List<long>();
		if (isFavorite)
		{
			list5.Add(photoDataId);
		}
		else
		{
			list6.Add(photoDataId);
		}
		this.parentData.ServerRequest(PhotoStatusCmd.Create(list, list2, list3, list4, list5, list6), new Action<Command>(this.CbPhotoStatusCmd));
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00029EB8 File Offset: 0x000280B8
	public void RequestActionPhotoRelease(long photoUniqId)
	{
		List<long> list = new List<long> { photoUniqId };
		this.parentData.ServerRequest(PhotoReleaseCmd.Create(list), new Action<Command>(this.CbPhotoReleaseCmd));
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00029EEF File Offset: 0x000280EF
	public void RequestActionPhotoAlbum()
	{
		if (this.PhotoAlbumUpdated)
		{
			this.AlbumDataList = new List<DataManagerPhoto.AlbumData>();
			this.parentData.ServerRequest(PhotoPictureBookCmd.Create(), new Action<Command>(this.CbPhotoAlbumCmd));
		}
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x00029F20 File Offset: 0x00028120
	private void CbPhotoStatusCmd(Command cmd)
	{
		PhotoStatusResponse photoStatusResponse = cmd.response as PhotoStatusResponse;
		this.parentData.UpdateUserAssetByAssets(photoStatusResponse.assets);
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x00029F4C File Offset: 0x0002814C
	private void CbPhotoGrowCmd(Command cmd)
	{
		PhotoGrowResponse photoGrowResponse = cmd.response as PhotoGrowResponse;
		this.parentData.UpdateUserAssetByAssets(photoGrowResponse.assets);
		PhotoPackData userPhotoData = DataManager.DmPhoto.GetUserPhotoData(photoGrowResponse.result.photo_id);
		this.photoLevelupResult.exp = userPhotoData.dynamicData.exp;
		this.photoLevelupResult.level = userPhotoData.dynamicData.level;
		this.photoLevelupResult.levelRank = userPhotoData.dynamicData.levelRank;
		this.photoLevelupResult.limitLevel = userPhotoData.limitLevel;
		this.photoLevelupResult.successStatus = (DataManagerPhoto.PhotoLevelupResult.Status)photoGrowResponse.result.lot_result;
		this.photoLevelupResult.UnusedPhotos = 1 == photoGrowResponse.result.notuse;
		if (photoGrowResponse.rewardinfoList != null)
		{
			this.photoLevelupResult.GrowRewardInfoList = photoGrowResponse.rewardinfoList.ConvertAll<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo>((RewardInfo item) => new DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo(item));
		}
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0002A04C File Offset: 0x0002824C
	private void CbPhotoSellCmd(Command cmd)
	{
		PhotoSellResponse photoSellResponse = cmd.response as PhotoSellResponse;
		this.parentData.UpdateUserAssetByAssets(photoSellResponse.assets);
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0002A078 File Offset: 0x00028278
	private void CbPhotoReleaseCmd(Command cmd)
	{
		PhotoReleaseResponse photoReleaseResponse = cmd.response as PhotoReleaseResponse;
		DataManager.DmDeck.UpdateUserDataByServer(photoReleaseResponse.decks);
		if (0 < photoReleaseResponse.helperList.Count)
		{
			DataManager.DmUserInfo.UpdateLoanPackListByServerData(photoReleaseResponse.helperList);
		}
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0002A0C0 File Offset: 0x000282C0
	private void CbPhotoAlbumCmd(Command cmd)
	{
		this.PhotoAlbumUpdated = false;
		foreach (PictureBookPhoto pictureBookPhoto in (cmd.response as PhotoPictureBookResponse).photoList)
		{
			this.AlbumDataList.Add(new DataManagerPhoto.AlbumData(pictureBookPhoto));
		}
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0002A13C File Offset: 0x0002833C
	[CompilerGenerated]
	internal static bool <ComparePhotoPackDataNyName>g__IsAlphabet|54_0(char c)
	{
		return ('A' <= c && c <= 'Z') || ('Ａ' <= c && c <= 'Ｚ') || ('a' <= c && c <= 'z') || ('ａ' <= c && c <= 'ｚ');
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x0002A179 File Offset: 0x00028379
	[CompilerGenerated]
	internal static bool <ComparePhotoPackDataNyName>g__IsHiragana|54_1(char c)
	{
		return 'ぁ' <= c && c <= 'ん';
	}

	// Token: 0x040005DB RID: 1499
	private DataManager parentData;

	// Token: 0x040005DC RID: 1500
	private DataManagerPhoto.PhotoLevelupResult photoLevelupResult;

	// Token: 0x040005DD RID: 1501
	private Dictionary<int, PhotoStaticData> photoStaticMap = new Dictionary<int, PhotoStaticData>();

	// Token: 0x040005DE RID: 1502
	private List<MstItemBonusData> mstItemBonusDataList;

	// Token: 0x040005DF RID: 1503
	private Dictionary<int, List<MstItemBonusData>> mstItemBonusMapByPhotoId;

	// Token: 0x040005E3 RID: 1507
	private List<MstPhotoGrowbonusData> mstPhotoGrowBonusDataList;

	// Token: 0x040005E5 RID: 1509
	public static int PHOTO_STOCK_RELEASEITEM_ID = 39300;

	// Token: 0x040005E6 RID: 1510
	private Dictionary<long, PhotoPackData> userPhotoMap = new Dictionary<long, PhotoPackData>();

	// Token: 0x040005E7 RID: 1511
	private Dictionary<int, int> userHaveNumByPhotoItemId = new Dictionary<int, int>();

	// Token: 0x02000708 RID: 1800
	public class PhotoDropItemData
	{
		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06003461 RID: 13409 RVA: 0x001C2C67 File Offset: 0x001C0E67
		// (set) Token: 0x06003462 RID: 13410 RVA: 0x001C2C6F File Offset: 0x001C0E6F
		public int PhotoId { get; set; }

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06003463 RID: 13411 RVA: 0x001C2C78 File Offset: 0x001C0E78
		// (set) Token: 0x06003464 RID: 13412 RVA: 0x001C2C80 File Offset: 0x001C0E80
		public DataManagerPhoto.PhotoDropItemData.QuestCategory Category { get; private set; }

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06003465 RID: 13413 RVA: 0x001C2C89 File Offset: 0x001C0E89
		// (set) Token: 0x06003466 RID: 13414 RVA: 0x001C2C91 File Offset: 0x001C0E91
		public int TargetId { get; set; }

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06003467 RID: 13415 RVA: 0x001C2C9A File Offset: 0x001C0E9A
		// (set) Token: 0x06003468 RID: 13416 RVA: 0x001C2CA2 File Offset: 0x001C0EA2
		public int PhotoLimitOverNum { get; set; }

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06003469 RID: 13417 RVA: 0x001C2CAB File Offset: 0x001C0EAB
		// (set) Token: 0x0600346A RID: 13418 RVA: 0x001C2CB3 File Offset: 0x001C0EB3
		public DateTime StartDateTime { get; set; }

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x0600346B RID: 13419 RVA: 0x001C2CBC File Offset: 0x001C0EBC
		// (set) Token: 0x0600346C RID: 13420 RVA: 0x001C2CC4 File Offset: 0x001C0EC4
		public DateTime EndDateTime { get; set; }

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600346D RID: 13421 RVA: 0x001C2CCD File Offset: 0x001C0ECD
		// (set) Token: 0x0600346E RID: 13422 RVA: 0x001C2CD5 File Offset: 0x001C0ED5
		public int BonusDrawId { get; set; }

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600346F RID: 13423 RVA: 0x001C2CDE File Offset: 0x001C0EDE
		// (set) Token: 0x06003470 RID: 13424 RVA: 0x001C2CE6 File Offset: 0x001C0EE6
		public int BonusDrawNum { get; set; }

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06003471 RID: 13425 RVA: 0x001C2CEF File Offset: 0x001C0EEF
		// (set) Token: 0x06003472 RID: 13426 RVA: 0x001C2CF7 File Offset: 0x001C0EF7
		public bool HelperEnabled { get; set; }

		// Token: 0x06003473 RID: 13427 RVA: 0x001C2D00 File Offset: 0x001C0F00
		public PhotoDropItemData(MstQuestPhotoDropItemData mst)
		{
			this.PhotoId = mst.photoId;
			this.Category = (DataManagerPhoto.PhotoDropItemData.QuestCategory)mst.category;
			this.TargetId = mst.targetId;
			this.PhotoLimitOverNum = mst.photoLimitOverNum;
			this.StartDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startDatetime));
			this.EndDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.endDatetime));
			this.BonusDrawId = mst.bonusDrawId;
			this.BonusDrawNum = mst.bonusDrawNum;
			this.HelperEnabled = mst.targetHelperFlg == 1;
		}

		// Token: 0x02001139 RID: 4409
		public enum QuestCategory
		{
			// Token: 0x04005E9C RID: 24220
			Invalid,
			// Token: 0x04005E9D RID: 24221
			Chapter,
			// Token: 0x04005E9E RID: 24222
			Map,
			// Token: 0x04005E9F RID: 24223
			Group,
			// Token: 0x04005EA0 RID: 24224
			One
		}
	}

	// Token: 0x02000709 RID: 1801
	public class PhotoLevelupResult
	{
		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06003474 RID: 13428 RVA: 0x001C2D96 File Offset: 0x001C0F96
		// (set) Token: 0x06003475 RID: 13429 RVA: 0x001C2D9E File Offset: 0x001C0F9E
		public int itemId { get; private set; }

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06003476 RID: 13430 RVA: 0x001C2DA7 File Offset: 0x001C0FA7
		// (set) Token: 0x06003477 RID: 13431 RVA: 0x001C2DAF File Offset: 0x001C0FAF
		public long photoDataId { get; private set; }

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06003478 RID: 13432 RVA: 0x001C2DB8 File Offset: 0x001C0FB8
		// (set) Token: 0x06003479 RID: 13433 RVA: 0x001C2DC0 File Offset: 0x001C0FC0
		public int befLevelRank { get; private set; }

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x0600347A RID: 13434 RVA: 0x001C2DC9 File Offset: 0x001C0FC9
		// (set) Token: 0x0600347B RID: 13435 RVA: 0x001C2DD1 File Offset: 0x001C0FD1
		public int befLevel { get; private set; }

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x0600347C RID: 13436 RVA: 0x001C2DDA File Offset: 0x001C0FDA
		// (set) Token: 0x0600347D RID: 13437 RVA: 0x001C2DE2 File Offset: 0x001C0FE2
		public long befExp { get; private set; }

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x0600347E RID: 13438 RVA: 0x001C2DEB File Offset: 0x001C0FEB
		// (set) Token: 0x0600347F RID: 13439 RVA: 0x001C2DF3 File Offset: 0x001C0FF3
		public int befLimitLevel { get; set; }

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06003480 RID: 13440 RVA: 0x001C2DFC File Offset: 0x001C0FFC
		// (set) Token: 0x06003481 RID: 13441 RVA: 0x001C2E04 File Offset: 0x001C1004
		public int levelRank { get; set; }

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06003482 RID: 13442 RVA: 0x001C2E0D File Offset: 0x001C100D
		// (set) Token: 0x06003483 RID: 13443 RVA: 0x001C2E15 File Offset: 0x001C1015
		public int level { get; set; }

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06003484 RID: 13444 RVA: 0x001C2E1E File Offset: 0x001C101E
		// (set) Token: 0x06003485 RID: 13445 RVA: 0x001C2E26 File Offset: 0x001C1026
		public long exp { get; set; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06003486 RID: 13446 RVA: 0x001C2E2F File Offset: 0x001C102F
		// (set) Token: 0x06003487 RID: 13447 RVA: 0x001C2E37 File Offset: 0x001C1037
		public int limitLevel { get; set; }

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06003488 RID: 13448 RVA: 0x001C2E40 File Offset: 0x001C1040
		// (set) Token: 0x06003489 RID: 13449 RVA: 0x001C2E48 File Offset: 0x001C1048
		public bool UnusedPhotos { get; set; }

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x0600348A RID: 13450 RVA: 0x001C2E51 File Offset: 0x001C1051
		// (set) Token: 0x0600348B RID: 13451 RVA: 0x001C2E59 File Offset: 0x001C1059
		public DataManagerPhoto.PhotoLevelupResult.Status successStatus { get; set; }

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x0600348C RID: 13452 RVA: 0x001C2E62 File Offset: 0x001C1062
		// (set) Token: 0x0600348D RID: 13453 RVA: 0x001C2E6A File Offset: 0x001C106A
		public List<ItemDef.Rarity> BonusRrityList { get; set; }

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x0600348E RID: 13454 RVA: 0x001C2E73 File Offset: 0x001C1073
		// (set) Token: 0x0600348F RID: 13455 RVA: 0x001C2E7B File Offset: 0x001C107B
		public List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo> GrowRewardInfoList { get; set; } = new List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo>();

		// Token: 0x06003490 RID: 13456 RVA: 0x001C2E84 File Offset: 0x001C1084
		public void SetBasePhoto(PhotoPackData basePhoto)
		{
			this.itemId = basePhoto.staticData.GetId();
			this.photoDataId = basePhoto.dataId;
			this.befLevelRank = basePhoto.dynamicData.levelRank;
			this.befLevel = basePhoto.dynamicData.level;
			this.befExp = basePhoto.dynamicData.exp;
			this.befLimitLevel = basePhoto.calcLimitLevel(this.befLevelRank);
		}

		// Token: 0x0200113A RID: 4410
		public enum Status
		{
			// Token: 0x04005EA2 RID: 24226
			NORMAL,
			// Token: 0x04005EA3 RID: 24227
			SPECIAL_S,
			// Token: 0x04005EA4 RID: 24228
			SPECIAL_L
		}

		// Token: 0x0200113B RID: 4411
		public class GrowRewardInfo
		{
			// Token: 0x17000C7E RID: 3198
			// (get) Token: 0x06005561 RID: 21857 RVA: 0x0024EF4B File Offset: 0x0024D14B
			// (set) Token: 0x06005562 RID: 21858 RVA: 0x0024EF53 File Offset: 0x0024D153
			public int ItemId { get; set; }

			// Token: 0x17000C7F RID: 3199
			// (get) Token: 0x06005563 RID: 21859 RVA: 0x0024EF5C File Offset: 0x0024D15C
			// (set) Token: 0x06005564 RID: 21860 RVA: 0x0024EF64 File Offset: 0x0024D164
			public new DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo.GET_TYPE GetType { get; set; }

			// Token: 0x06005565 RID: 21861 RVA: 0x0024EF6D File Offset: 0x0024D16D
			public GrowRewardInfo()
			{
			}

			// Token: 0x06005566 RID: 21862 RVA: 0x0024EF75 File Offset: 0x0024D175
			public GrowRewardInfo(RewardInfo serverData)
			{
				this.ItemId = serverData.rewarditem_id;
				this.GetType = (DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo.GET_TYPE)serverData.reason_type;
			}

			// Token: 0x02001234 RID: 4660
			public enum GET_TYPE
			{
				// Token: 0x040063A8 RID: 25512
				INVALID,
				// Token: 0x040063A9 RID: 25513
				RANK_MAX,
				// Token: 0x040063AA RID: 25514
				LEVEL_MAX
			}
		}
	}

	// Token: 0x0200070A RID: 1802
	public class AlbumData
	{
		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06003492 RID: 13458 RVA: 0x001C2F06 File Offset: 0x001C1106
		// (set) Token: 0x06003493 RID: 13459 RVA: 0x001C2F0E File Offset: 0x001C110E
		public int PhotoId { get; set; }

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06003494 RID: 13460 RVA: 0x001C2F17 File Offset: 0x001C1117
		// (set) Token: 0x06003495 RID: 13461 RVA: 0x001C2F1F File Offset: 0x001C111F
		public int LimitOverNum { get; set; }

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06003496 RID: 13462 RVA: 0x001C2F28 File Offset: 0x001C1128
		// (set) Token: 0x06003497 RID: 13463 RVA: 0x001C2F30 File Offset: 0x001C1130
		public bool IsLevelMax { get; set; }

		// Token: 0x06003498 RID: 13464 RVA: 0x001C2F39 File Offset: 0x001C1139
		public AlbumData(PictureBookPhoto pbp)
		{
			this.PhotoId = pbp.photo_id;
			this.LimitOverNum = pbp.limit_over_num;
			this.IsLevelMax = pbp.level_max != 0;
		}
	}

	// Token: 0x0200070B RID: 1803
	public class PhotoCharacteristicData
	{
		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06003499 RID: 13465 RVA: 0x001C2F68 File Offset: 0x001C1168
		// (set) Token: 0x0600349A RID: 13466 RVA: 0x001C2F70 File Offset: 0x001C1170
		public SortFilterDefine.CharacteristicFilterCategory Category { get; private set; }

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x0600349B RID: 13467 RVA: 0x001C2F79 File Offset: 0x001C1179
		// (set) Token: 0x0600349C RID: 13468 RVA: 0x001C2F81 File Offset: 0x001C1181
		public string CategoryName { get; private set; }

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x0600349D RID: 13469 RVA: 0x001C2F8A File Offset: 0x001C118A
		// (set) Token: 0x0600349E RID: 13470 RVA: 0x001C2F92 File Offset: 0x001C1192
		public string DisplayName { get; private set; }

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x0600349F RID: 13471 RVA: 0x001C2F9B File Offset: 0x001C119B
		// (set) Token: 0x060034A0 RID: 13472 RVA: 0x001C2FA3 File Offset: 0x001C11A3
		public List<DataManagerPhoto.PhotoCharacteristicData.FilterElementOne> ElementList { get; set; }

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060034A1 RID: 13473 RVA: 0x001C2FAC File Offset: 0x001C11AC
		public bool IsEnabled
		{
			get
			{
				if (this._isEnable)
				{
					return true;
				}
				using (List<DataManagerPhoto.PhotoCharacteristicData.FilterElementOne>.Enumerator enumerator = this.ElementList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.FilteringStartDatetime <= TimeManager.Now)
						{
							this._isEnable = true;
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x060034A2 RID: 13474 RVA: 0x001C3020 File Offset: 0x001C1220
		// (set) Token: 0x060034A3 RID: 13475 RVA: 0x001C3028 File Offset: 0x001C1228
		public int Priority { get; private set; }

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x060034A4 RID: 13476 RVA: 0x001C3031 File Offset: 0x001C1231
		// (set) Token: 0x060034A5 RID: 13477 RVA: 0x001C3039 File Offset: 0x001C1239
		public DateTime StartDatetime { get; private set; }

		// Token: 0x060034A6 RID: 13478 RVA: 0x001C3044 File Offset: 0x001C1244
		public PhotoCharacteristicData(MstPhotoCharacteristicData mst)
		{
			this.Category = (SortFilterDefine.CharacteristicFilterCategory)mst.category;
			this.CategoryName = mst.categoryName;
			this.DisplayName = mst.labelText;
			this.Priority = mst.priority;
			this.StartDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
			this.ElementList = new List<DataManagerPhoto.PhotoCharacteristicData.FilterElementOne>();
			this.AdditionalElement(mst);
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x001C30B0 File Offset: 0x001C12B0
		public void AdditionalElement(MstPhotoCharacteristicData mst)
		{
			if (this.Category != (SortFilterDefine.CharacteristicFilterCategory)mst.category)
			{
				return;
			}
			this.Priority = ((this.Priority < mst.priority) ? this.Priority : mst.priority);
			DateTime dateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
			this.StartDatetime = ((dateTime < this.StartDatetime) ? dateTime : this.StartDatetime);
			this.ElementList.Add(new DataManagerPhoto.PhotoCharacteristicData.FilterElementOne(mst));
		}

		// Token: 0x040031ED RID: 12781
		private bool _isEnable;

		// Token: 0x0200113C RID: 4412
		public class FilterElementOne
		{
			// Token: 0x17000C80 RID: 3200
			// (get) Token: 0x06005567 RID: 21863 RVA: 0x0024EF95 File Offset: 0x0024D195
			// (set) Token: 0x06005568 RID: 21864 RVA: 0x0024EF9D File Offset: 0x0024D19D
			public string FilterName { get; private set; }

			// Token: 0x17000C81 RID: 3201
			// (get) Token: 0x06005569 RID: 21865 RVA: 0x0024EFA6 File Offset: 0x0024D1A6
			// (set) Token: 0x0600556A RID: 21866 RVA: 0x0024EFAE File Offset: 0x0024D1AE
			public DateTime FilteringStartDatetime { get; set; }

			// Token: 0x17000C82 RID: 3202
			// (get) Token: 0x0600556B RID: 21867 RVA: 0x0024EFB7 File Offset: 0x0024D1B7
			// (set) Token: 0x0600556C RID: 21868 RVA: 0x0024EFBF File Offset: 0x0024D1BF
			public SortFilterDefine.FilterElementType FilterType { get; private set; }

			// Token: 0x17000C83 RID: 3203
			// (get) Token: 0x0600556D RID: 21869 RVA: 0x0024EFC8 File Offset: 0x0024D1C8
			public CharaDef.ConditionType Condition
			{
				get
				{
					return this._condition;
				}
			}

			// Token: 0x17000C84 RID: 3204
			// (get) Token: 0x0600556E RID: 21870 RVA: 0x0024EFD0 File Offset: 0x0024D1D0
			public CharaDef.AbilityTraits Traits
			{
				get
				{
					return this._traits;
				}
			}

			// Token: 0x17000C85 RID: 3205
			// (get) Token: 0x0600556F RID: 21871 RVA: 0x0024EFD8 File Offset: 0x0024D1D8
			public CharaDef.AbilityTraits2 Traits2
			{
				get
				{
					return this._traits2;
				}
			}

			// Token: 0x17000C86 RID: 3206
			// (get) Token: 0x06005570 RID: 21872 RVA: 0x0024EFE0 File Offset: 0x0024D1E0
			public CharaDef.ActionTargetType TargetType
			{
				get
				{
					return this._targetType;
				}
			}

			// Token: 0x17000C87 RID: 3207
			// (get) Token: 0x06005571 RID: 21873 RVA: 0x0024EFE8 File Offset: 0x0024D1E8
			public CharaDef.ActionBuffType BuffType
			{
				get
				{
					return this._buffType;
				}
			}

			// Token: 0x17000C88 RID: 3208
			// (get) Token: 0x06005572 RID: 21874 RVA: 0x0024EFF0 File Offset: 0x0024D1F0
			public CharaDef.ActionAbnormalMask AbnormalMask
			{
				get
				{
					return this._abnormalMask;
				}
			}

			// Token: 0x17000C89 RID: 3209
			// (get) Token: 0x06005573 RID: 21875 RVA: 0x0024EFF8 File Offset: 0x0024D1F8
			public CharaDef.ActionAbnormalMask2 AbnormalMask2
			{
				get
				{
					return this._abnormalMask2;
				}
			}

			// Token: 0x17000C8A RID: 3210
			// (get) Token: 0x06005574 RID: 21876 RVA: 0x0024F000 File Offset: 0x0024D200
			// (set) Token: 0x06005575 RID: 21877 RVA: 0x0024F008 File Offset: 0x0024D208
			public bool GutsListEnable { get; private set; }

			// Token: 0x06005576 RID: 21878 RVA: 0x0024F014 File Offset: 0x0024D214
			public FilterElementOne(MstPhotoCharacteristicData mst)
			{
				this.FilterName = mst.filterName;
				this.FilterType = SortFilterDefine.FilterElementType.Invalid;
				switch (mst.category)
				{
				case 1:
				{
					bool flag = Enum.TryParse<CharaDef.ConditionType>(this.FilterName, out this._condition);
					if (!flag)
					{
						this._condition = CharaDef.ConditionType.UPPER;
					}
					else
					{
						this.FilterType = SortFilterDefine.FilterElementType.Condition;
					}
					if (!flag)
					{
						CharaDef.AbilityTraits terrainAll = SortFilterDefine.TerrainAll;
						flag = Enum.TryParse<CharaDef.AbilityTraits>(this.FilterName, out this._traits);
						if (!flag)
						{
							this._traits = terrainAll;
						}
						else
						{
							this.FilterType = SortFilterDefine.FilterElementType.Terrain;
						}
					}
					if (!flag)
					{
						flag = Enum.TryParse<CharaDef.AbilityTraits2>(this.FilterName, out this._traits2);
						if (!flag)
						{
							this._traits2 = CharaDef.AbilityTraits2.without;
						}
						else
						{
							this.FilterType = SortFilterDefine.FilterElementType.Night;
						}
					}
					if (!flag)
					{
					}
					break;
				}
				case 2:
				{
					bool flag2 = Enum.TryParse<CharaDef.ActionTargetType>(this.FilterName, out this._targetType);
					if (!flag2)
					{
						this._targetType = CharaDef.ActionTargetType.INVALID;
					}
					else
					{
						this.FilterType = SortFilterDefine.FilterElementType.Target;
					}
					if (!flag2)
					{
					}
					break;
				}
				case 3:
				{
					bool flag3 = Enum.TryParse<CharaDef.ActionBuffType>(this.FilterName, out this._buffType);
					if (!flag3)
					{
						this._buffType = CharaDef.ActionBuffType.INVALID;
					}
					else
					{
						this.FilterType = SortFilterDefine.FilterElementType.Buff;
					}
					if (!flag3 && "GUTS_LIST" == this.FilterName)
					{
						this.GutsListEnable = true;
						flag3 = true;
						this.FilterType = SortFilterDefine.FilterElementType.Guts;
					}
					if (!flag3)
					{
					}
					break;
				}
				case 4:
				{
					bool flag4 = Enum.TryParse<CharaDef.ActionAbnormalMask>(this.FilterName, out this._abnormalMask);
					bool flag5 = Enum.TryParse<CharaDef.ActionAbnormalMask2>(this.FilterName, out this._abnormalMask2);
					if (!flag4 && !flag5)
					{
						this._abnormalMask = (CharaDef.ActionAbnormalMask)0;
					}
					else
					{
						this.FilterType = SortFilterDefine.FilterElementType.Abnormal;
					}
					break;
				}
				}
				this.FilteringStartDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
			}

			// Token: 0x04005EAA RID: 24234
			private CharaDef.ConditionType _condition;

			// Token: 0x04005EAB RID: 24235
			private CharaDef.AbilityTraits _traits;

			// Token: 0x04005EAC RID: 24236
			private CharaDef.AbilityTraits2 _traits2;

			// Token: 0x04005EAD RID: 24237
			private CharaDef.ActionTargetType _targetType;

			// Token: 0x04005EAE RID: 24238
			private CharaDef.ActionBuffType _buffType;

			// Token: 0x04005EAF RID: 24239
			private CharaDef.ActionAbnormalMask _abnormalMask;

			// Token: 0x04005EB0 RID: 24240
			private CharaDef.ActionAbnormalMask2 _abnormalMask2;
		}
	}

	// Token: 0x0200070C RID: 1804
	public class CalcDropBonusResult
	{
		// Token: 0x040031F0 RID: 12784
		public int ratio;

		// Token: 0x040031F1 RID: 12785
		public int targetItemId;

		// Token: 0x040031F2 RID: 12786
		public int targetItemBonusRatio;
	}

	// Token: 0x0200070D RID: 1805
	[Serializable]
	public class JsonData<T>
	{
		// Token: 0x040031F3 RID: 12787
		public T[] data;
	}
}
