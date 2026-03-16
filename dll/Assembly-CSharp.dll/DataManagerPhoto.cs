using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerPhoto
{
	public DataManagerPhoto(DataManager p)
	{
		this.parentData = p;
	}

	public List<DataManagerPhoto.PhotoDropItemData> PhotoQuestDropItemList { get; private set; }

	public List<DataManagerPhoto.AlbumData> AlbumDataList { get; private set; }

	private bool PhotoAlbumUpdated { get; set; }

	public int PhotoStockLimit
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.photoStockLimit + this.getAdditionalPhotoStock();
		}
	}

	private List<DataManagerPhoto.PhotoCharacteristicData> PhotoCharacteristicDataList { get; set; }

	public PhotoStaticData GetPhotoStaticData(int photoId)
	{
		if (!this.photoStaticMap.ContainsKey(photoId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerPhoto.GetPhotoStaticData : 定義されていないID[" + photoId.ToString() + "]を生成しようとしました", null);
			return null;
		}
		return this.photoStaticMap[photoId];
	}

	public Dictionary<int, PhotoStaticData> GetPhotoStaticMap()
	{
		return this.photoStaticMap;
	}

	public int GetPhotoBonuExpData(ItemDef.Rarity rarity)
	{
		MstPhotoGrowbonusData mstPhotoGrowbonusData = this.mstPhotoGrowBonusDataList.Find((MstPhotoGrowbonusData x) => x.rarity == (int)rarity);
		if (mstPhotoGrowbonusData == null)
		{
			return 0;
		}
		return mstPhotoGrowbonusData.bonus;
	}

	public PhotoPackData GetUserPhotoData(long photoDataId)
	{
		if (this.userPhotoMap.ContainsKey(photoDataId))
		{
			return this.userPhotoMap[photoDataId];
		}
		return null;
	}

	public Dictionary<long, PhotoPackData> GetUserPhotoMap()
	{
		return this.userPhotoMap;
	}

	public int GetHaveNumByPhotoItemId(int photoItemId)
	{
		return this.userHaveNumByPhotoItemId.TryGetValueEx(photoItemId, 0);
	}

	public int getAdditionalPhotoStock()
	{
		return DataManager.DmItem.GetUserItemData(DataManagerPhoto.PHOTO_STOCK_RELEASEITEM_ID).num;
	}

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

	public List<DataManagerPhoto.PhotoDropItemData> GetPhotoQuestDropItemList(DateTime targetDateTime)
	{
		return this.PhotoQuestDropItemList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.StartDateTime <= targetDateTime && targetDateTime <= x.EndDateTime);
	}

	public DataManagerPhoto.PhotoLevelupResult GetPhotoGrowResult()
	{
		return this.photoLevelupResult;
	}

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

	public long GetExpByNextLevel(PhotoPackData ppd)
	{
		return this.GetExpByNextLevel(ppd.staticData.GetId(), ppd.dynamicData.level, ppd.dynamicData.levelRank);
	}

	public long GetExpByNextLevel(DataManagerPhoto.PhotoLevelupResult plr, bool isBefore)
	{
		return this.GetExpByNextLevel(plr.itemId, isBefore ? plr.befLevel : plr.level, isBefore ? plr.befLevelRank : plr.levelRank);
	}

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

	public List<DataManagerPhoto.CalcDropBonusResult> CalcPhotoBonus(List<PhotoPackData> photoPackList, DateTime targetTime, CharaPackData charaPack = null)
	{
		return this.InternalCalcPhotoBonus(photoPackList, targetTime, charaPack, false);
	}

	public List<DataManagerPhoto.CalcDropBonusResult> CalcPhotoClampBonus(PhotoPackData basePhotoPack, List<PhotoPackData> photoPackList, DateTime targetTime)
	{
		return this.InternalCalcPhotoBonus(basePhotoPack, photoPackList, targetTime);
	}

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

	public void RequestActionPhotoGrow(PhotoPackData basePhotoData, List<PhotoPackData> feedPhotoData)
	{
		this.parentData.ServerRequest(PhotoGrowCmd.Create(basePhotoData.dynamicData.dataId, feedPhotoData.Select<PhotoPackData, long>((PhotoPackData item) => item.dataId).ToList<long>()), new Action<Command>(this.CbPhotoGrowCmd));
		this.photoLevelupResult = new DataManagerPhoto.PhotoLevelupResult();
		this.photoLevelupResult.SetBasePhoto(basePhotoData);
	}

	public void RequestActionPhotoSale(List<PhotoPackData> targetPhotoList)
	{
		this.parentData.ServerRequest(PhotoSellCmd.Create(targetPhotoList.Select<PhotoPackData, long>((PhotoPackData item) => item.dataId).ToList<long>()), new Action<Command>(this.CbPhotoSellCmd));
	}

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

	public void RequestActionPhotoRelease(long photoUniqId)
	{
		List<long> list = new List<long> { photoUniqId };
		this.parentData.ServerRequest(PhotoReleaseCmd.Create(list), new Action<Command>(this.CbPhotoReleaseCmd));
	}

	public void RequestActionPhotoAlbum()
	{
		if (this.PhotoAlbumUpdated)
		{
			this.AlbumDataList = new List<DataManagerPhoto.AlbumData>();
			this.parentData.ServerRequest(PhotoPictureBookCmd.Create(), new Action<Command>(this.CbPhotoAlbumCmd));
		}
	}

	private void CbPhotoStatusCmd(Command cmd)
	{
		PhotoStatusResponse photoStatusResponse = cmd.response as PhotoStatusResponse;
		this.parentData.UpdateUserAssetByAssets(photoStatusResponse.assets);
	}

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

	private void CbPhotoSellCmd(Command cmd)
	{
		PhotoSellResponse photoSellResponse = cmd.response as PhotoSellResponse;
		this.parentData.UpdateUserAssetByAssets(photoSellResponse.assets);
	}

	private void CbPhotoReleaseCmd(Command cmd)
	{
		PhotoReleaseResponse photoReleaseResponse = cmd.response as PhotoReleaseResponse;
		DataManager.DmDeck.UpdateUserDataByServer(photoReleaseResponse.decks);
		if (0 < photoReleaseResponse.helperList.Count)
		{
			DataManager.DmUserInfo.UpdateLoanPackListByServerData(photoReleaseResponse.helperList);
		}
	}

	private void CbPhotoAlbumCmd(Command cmd)
	{
		this.PhotoAlbumUpdated = false;
		foreach (PictureBookPhoto pictureBookPhoto in (cmd.response as PhotoPictureBookResponse).photoList)
		{
			this.AlbumDataList.Add(new DataManagerPhoto.AlbumData(pictureBookPhoto));
		}
	}

	[CompilerGenerated]
	internal static bool <ComparePhotoPackDataNyName>g__IsAlphabet|54_0(char c)
	{
		return ('A' <= c && c <= 'Z') || ('Ａ' <= c && c <= 'Ｚ') || ('a' <= c && c <= 'z') || ('ａ' <= c && c <= 'ｚ');
	}

	[CompilerGenerated]
	internal static bool <ComparePhotoPackDataNyName>g__IsHiragana|54_1(char c)
	{
		return 'ぁ' <= c && c <= 'ん';
	}

	private DataManager parentData;

	private DataManagerPhoto.PhotoLevelupResult photoLevelupResult;

	private Dictionary<int, PhotoStaticData> photoStaticMap = new Dictionary<int, PhotoStaticData>();

	private List<MstItemBonusData> mstItemBonusDataList;

	private Dictionary<int, List<MstItemBonusData>> mstItemBonusMapByPhotoId;

	private List<MstPhotoGrowbonusData> mstPhotoGrowBonusDataList;

	public static int PHOTO_STOCK_RELEASEITEM_ID = 39300;

	private Dictionary<long, PhotoPackData> userPhotoMap = new Dictionary<long, PhotoPackData>();

	private Dictionary<int, int> userHaveNumByPhotoItemId = new Dictionary<int, int>();

	public class PhotoDropItemData
	{
		public int PhotoId { get; set; }

		public DataManagerPhoto.PhotoDropItemData.QuestCategory Category { get; private set; }

		public int TargetId { get; set; }

		public int PhotoLimitOverNum { get; set; }

		public DateTime StartDateTime { get; set; }

		public DateTime EndDateTime { get; set; }

		public int BonusDrawId { get; set; }

		public int BonusDrawNum { get; set; }

		public bool HelperEnabled { get; set; }

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

		public enum QuestCategory
		{
			Invalid,
			Chapter,
			Map,
			Group,
			One
		}
	}

	public class PhotoLevelupResult
	{
		public int itemId { get; private set; }

		public long photoDataId { get; private set; }

		public int befLevelRank { get; private set; }

		public int befLevel { get; private set; }

		public long befExp { get; private set; }

		public int befLimitLevel { get; set; }

		public int levelRank { get; set; }

		public int level { get; set; }

		public long exp { get; set; }

		public int limitLevel { get; set; }

		public bool UnusedPhotos { get; set; }

		public DataManagerPhoto.PhotoLevelupResult.Status successStatus { get; set; }

		public List<ItemDef.Rarity> BonusRrityList { get; set; }

		public List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo> GrowRewardInfoList { get; set; } = new List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo>();

		public void SetBasePhoto(PhotoPackData basePhoto)
		{
			this.itemId = basePhoto.staticData.GetId();
			this.photoDataId = basePhoto.dataId;
			this.befLevelRank = basePhoto.dynamicData.levelRank;
			this.befLevel = basePhoto.dynamicData.level;
			this.befExp = basePhoto.dynamicData.exp;
			this.befLimitLevel = basePhoto.calcLimitLevel(this.befLevelRank);
		}

		public enum Status
		{
			NORMAL,
			SPECIAL_S,
			SPECIAL_L
		}

		public class GrowRewardInfo
		{
			public int ItemId { get; set; }

			public new DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo.GET_TYPE GetType { get; set; }

			public GrowRewardInfo()
			{
			}

			public GrowRewardInfo(RewardInfo serverData)
			{
				this.ItemId = serverData.rewarditem_id;
				this.GetType = (DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo.GET_TYPE)serverData.reason_type;
			}

			public enum GET_TYPE
			{
				INVALID,
				RANK_MAX,
				LEVEL_MAX
			}
		}
	}

	public class AlbumData
	{
		public int PhotoId { get; set; }

		public int LimitOverNum { get; set; }

		public bool IsLevelMax { get; set; }

		public AlbumData(PictureBookPhoto pbp)
		{
			this.PhotoId = pbp.photo_id;
			this.LimitOverNum = pbp.limit_over_num;
			this.IsLevelMax = pbp.level_max != 0;
		}
	}

	public class PhotoCharacteristicData
	{
		public SortFilterDefine.CharacteristicFilterCategory Category { get; private set; }

		public string CategoryName { get; private set; }

		public string DisplayName { get; private set; }

		public List<DataManagerPhoto.PhotoCharacteristicData.FilterElementOne> ElementList { get; set; }

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

		public int Priority { get; private set; }

		public DateTime StartDatetime { get; private set; }

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

		private bool _isEnable;

		public class FilterElementOne
		{
			public string FilterName { get; private set; }

			public DateTime FilteringStartDatetime { get; set; }

			public SortFilterDefine.FilterElementType FilterType { get; private set; }

			public CharaDef.ConditionType Condition
			{
				get
				{
					return this._condition;
				}
			}

			public CharaDef.AbilityTraits Traits
			{
				get
				{
					return this._traits;
				}
			}

			public CharaDef.AbilityTraits2 Traits2
			{
				get
				{
					return this._traits2;
				}
			}

			public CharaDef.ActionTargetType TargetType
			{
				get
				{
					return this._targetType;
				}
			}

			public CharaDef.ActionBuffType BuffType
			{
				get
				{
					return this._buffType;
				}
			}

			public CharaDef.ActionAbnormalMask AbnormalMask
			{
				get
				{
					return this._abnormalMask;
				}
			}

			public CharaDef.ActionAbnormalMask2 AbnormalMask2
			{
				get
				{
					return this._abnormalMask2;
				}
			}

			public bool GutsListEnable { get; private set; }

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

			private CharaDef.ConditionType _condition;

			private CharaDef.AbilityTraits _traits;

			private CharaDef.AbilityTraits2 _traits2;

			private CharaDef.ActionTargetType _targetType;

			private CharaDef.ActionBuffType _buffType;

			private CharaDef.ActionAbnormalMask _abnormalMask;

			private CharaDef.ActionAbnormalMask2 _abnormalMask2;
		}
	}

	public class CalcDropBonusResult
	{
		public int ratio;

		public int targetItemId;

		public int targetItemBonusRatio;
	}

	[Serializable]
	public class JsonData<T>
	{
		public T[] data;
	}
}
