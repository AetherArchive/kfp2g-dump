using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Login;

// Token: 0x020000FF RID: 255
public class RegisterSaveData
{
	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06000C2F RID: 3119 RVA: 0x00048A5B File Offset: 0x00046C5B
	// (set) Token: 0x06000C30 RID: 3120 RVA: 0x00048A63 File Offset: 0x00046C63
	public bool SortOrder { get; set; }

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00048A6C File Offset: 0x00046C6C
	// (set) Token: 0x06000C32 RID: 3122 RVA: 0x00048A74 File Offset: 0x00046C74
	public SortFilterDefine.RegisterType RegisterType { get; set; }

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06000C33 RID: 3123 RVA: 0x00048A7D File Offset: 0x00046C7D
	// (set) Token: 0x06000C34 RID: 3124 RVA: 0x00048A85 File Offset: 0x00046C85
	public int EventFilterButtonNum { get; set; }

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06000C35 RID: 3125 RVA: 0x00048A8E File Offset: 0x00046C8E
	public bool IsNeedCharaPackDataSortType
	{
		get
		{
			return SortFilterDefine.SortType.LOGIN != this.sortType && SortFilterDefine.SortType.USER_RANK != this.sortType && SortFilterDefine.SortType.NEW != this.sortType;
		}
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x00048AB4 File Offset: 0x00046CB4
	public RegisterSaveData(DataManagerGameStatus.UserFlagData userFlagData, SortFilterDefine.RegisterType regType)
	{
		this.RegisterType = regType;
		DataManagerGameStatus.UserFlagData.SortTypeData sortTypeData = userFlagData.GetSortTypeData(this.RegisterType);
		this.sortType = sortTypeData.SortType;
		this.SortOrder = sortTypeData.Order;
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x00048C54 File Offset: 0x00046E54
	private void FilteringPhotoList(ref List<PhotoPackData> targetList, List<PhotoPackData> ignoreList, PhotoPackData basePhoto)
	{
		if (0 < this.includePhotoSearchText.Length)
		{
			targetList = targetList.Where<PhotoPackData>((PhotoPackData item) => item.staticData.baseData.photoName.Contains(this.includePhotoSearchText)).ToList<PhotoPackData>();
		}
		if (0 < this.includePhotoRarityList.Count)
		{
			targetList.RemoveAll((PhotoPackData item) => (ignoreList == null || !ignoreList.Contains(item)) && !this.includePhotoRarityList.Contains(item.staticData.baseData.rarity));
		}
		if (0 < this.includePhotoTypeList.Count)
		{
			List<PhotoPackData> pendingList = new List<PhotoPackData>();
			pendingList.AddRange(targetList.FindAll((PhotoPackData x) => ignoreList != null && ignoreList.Contains(x)));
			targetList.RemoveAll((PhotoPackData x) => pendingList.Contains(x));
			using (List<PhotoDef.Type>.Enumerator enumerator = this.includePhotoTypeList.GetEnumerator())
			{
				Predicate<PhotoPackData> <>9__11;
				while (enumerator.MoveNext())
				{
					PhotoDef.Type includePhotoType = enumerator.Current;
					PhotoDef.Type includePhotoType2 = includePhotoType;
					if (includePhotoType2 - PhotoDef.Type.PARAMETER > 1)
					{
						if (includePhotoType2 == PhotoDef.Type.OTHER)
						{
							pendingList.AddRange(targetList.FindAll((PhotoPackData x) => x.staticData.baseData.expPhotoType > PhotoDef.ExpPhotoType.Invalid));
						}
					}
					else
					{
						pendingList.AddRange(targetList.FindAll((PhotoPackData x) => includePhotoType == x.staticData.baseData.type));
					}
					List<PhotoPackData> list2 = targetList;
					Predicate<PhotoPackData> predicate;
					if ((predicate = <>9__11) == null)
					{
						predicate = (<>9__11 = (PhotoPackData x) => pendingList.Contains(x));
					}
					list2.RemoveAll(predicate);
				}
			}
			targetList = pendingList;
		}
		if (this.includePhotoBonus)
		{
			HashSet<int> list = DataManager.DmPhoto.GetEnablePhotoBonusByTime(TimeManager.Now);
			if (0 < list.Count)
			{
				targetList.RemoveAll((PhotoPackData item) => !list.Contains(item.staticData.GetId()));
			}
		}
		if (this.includePhotoLimit && basePhoto != null)
		{
			if (4 == basePhoto.dynamicData.levelRank)
			{
				targetList = new List<PhotoPackData>();
			}
			else if (basePhoto.staticData.baseData.forbiddenDiscardFlg)
			{
				targetList.RemoveAll((PhotoPackData item) => basePhoto.staticData.GetId() != item.staticData.GetId());
			}
			else
			{
				targetList.RemoveAll((PhotoPackData item) => basePhoto.staticData.baseData.rarity > item.staticData.baseData.rarity);
				targetList.RemoveAll((PhotoPackData item) => item.staticData.GetId() != basePhoto.staticData.GetId() && !PhotoUtil.IsLevelLimitOverPhoto(item));
			}
		}
		if (this.isFilterFavoritePhotoList[0])
		{
			targetList.RemoveAll((PhotoPackData item) => !item.dynamicData.favoriteFlag);
		}
		if (this.isFilterFavoritePhotoList[1])
		{
			targetList.RemoveAll((PhotoPackData item) => item.dynamicData.favoriteFlag);
		}
		this.PhotoFilteringBuffStatus(ref targetList);
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x00048F38 File Offset: 0x00047138
	private void FilteringSortTargetPhotoList(ref List<PhotoPackData> photoPackListwithoutDisable, List<PhotoPackData> ignoreList, List<PhotoPackData> disableListAfter, List<PhotoPackData> disableListbefore, PhotoPackData basePhoto)
	{
		this.FilteringPhotoList(ref photoPackListwithoutDisable, ignoreList, basePhoto);
		Comparison<PhotoPackData> comparison = delegate(PhotoPackData a, PhotoPackData b)
		{
			int num = a.dataId.CompareTo(b.dataId);
			int num2 = a.dynamicData.photoId.CompareTo(b.dynamicData.photoId);
			int num3;
			if (this.SortOrder)
			{
				num3 = b.staticData.baseData.type.CompareTo(a.staticData.baseData.type);
			}
			else
			{
				num3 = a.staticData.baseData.type.CompareTo(b.staticData.baseData.type);
			}
			int num4 = this.SortPhotoList(a, b);
			int num5 = a.dynamicData.favoriteFlag.CompareTo(b.dynamicData.favoriteFlag);
			int num6 = a.dynamicData.lockFlag.CompareTo(b.dynamicData.lockFlag);
			if (num4 != 0)
			{
				return num4;
			}
			if (num3 != 0)
			{
				return num3;
			}
			if (num2 != 0)
			{
				return num2;
			}
			if (num5 != 0)
			{
				return num5;
			}
			if (num6 == 0)
			{
				return num;
			}
			return num6;
		};
		PrjUtil.InsertionSortLight<PhotoPackData>(ref photoPackListwithoutDisable, comparison);
		if (this.SortOrder)
		{
			photoPackListwithoutDisable.Reverse();
		}
		if (disableListAfter != null)
		{
			this.FilteringPhotoList(ref disableListAfter, ignoreList, basePhoto);
			PrjUtil.InsertionSortLight<PhotoPackData>(ref disableListAfter, comparison);
			if (this.SortOrder)
			{
				disableListAfter.Reverse();
			}
			photoPackListwithoutDisable.AddRange(disableListAfter);
		}
		if (disableListbefore != null)
		{
			this.FilteringPhotoList(ref disableListbefore, ignoreList, basePhoto);
			PrjUtil.InsertionSortLight<PhotoPackData>(ref disableListbefore, comparison);
			if (this.SortOrder)
			{
				disableListbefore.Reverse();
			}
			photoPackListwithoutDisable.InsertRange(0, disableListbefore);
		}
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x00048FD0 File Offset: 0x000471D0
	private void SortAlbumPhotoList(ref List<PhotoPackData> photoPackList, Dictionary<SortFilterDefine.PhotoAlbumRegistrationStatus, HashSet<int>> registeredPhotoStatusMap)
	{
		HashSet<int> includePhotoIdHashSet = new HashSet<int>();
		if (0 < this.includePhotoAlbumRegistrationStatusList.Count && registeredPhotoStatusMap != null)
		{
			HashSet<SortFilterDefine.PhotoAlbumRegistrationStatus> hashSet = new HashSet<SortFilterDefine.PhotoAlbumRegistrationStatus>();
			foreach (SortFilterDefine.PhotoAlbumRegistrationStatus photoAlbumRegistrationStatus in this.includePhotoAlbumRegistrationStatusList)
			{
				switch (photoAlbumRegistrationStatus)
				{
				case SortFilterDefine.PhotoAlbumRegistrationStatus.Registered:
					hashSet.Add(SortFilterDefine.PhotoAlbumRegistrationStatus.Registered);
					hashSet.Add(SortFilterDefine.PhotoAlbumRegistrationStatus.BreakthroughLimitMax);
					hashSet.Add(SortFilterDefine.PhotoAlbumRegistrationStatus.GrowthMax);
					break;
				case SortFilterDefine.PhotoAlbumRegistrationStatus.BreakthroughLimitMax:
					hashSet.Add(SortFilterDefine.PhotoAlbumRegistrationStatus.BreakthroughLimitMax);
					hashSet.Add(SortFilterDefine.PhotoAlbumRegistrationStatus.GrowthMax);
					break;
				case SortFilterDefine.PhotoAlbumRegistrationStatus.GrowthMax:
					hashSet.Add(SortFilterDefine.PhotoAlbumRegistrationStatus.GrowthMax);
					break;
				default:
					hashSet.Add(photoAlbumRegistrationStatus);
					break;
				}
			}
			foreach (SortFilterDefine.PhotoAlbumRegistrationStatus photoAlbumRegistrationStatus2 in hashSet)
			{
				if (registeredPhotoStatusMap.ContainsKey(photoAlbumRegistrationStatus2))
				{
					includePhotoIdHashSet.UnionWith(registeredPhotoStatusMap[photoAlbumRegistrationStatus2]);
				}
			}
		}
		if (0 < this.includePhotoSearchText.Length)
		{
			photoPackList = photoPackList.Where<PhotoPackData>((PhotoPackData item) => item.staticData.baseData.photoName.Contains(this.includePhotoSearchText)).ToList<PhotoPackData>();
		}
		photoPackList.RemoveAll((PhotoPackData item) => 0 < this.includePhotoRarityList.Count && !this.includePhotoRarityList.Contains(item.staticData.baseData.rarity));
		photoPackList.RemoveAll((PhotoPackData item) => 0 < this.includePhotoTypeList.Count && !this.includePhotoTypeList.Contains(item.staticData.baseData.type));
		photoPackList.RemoveAll((PhotoPackData item) => 0 < this.includePhotoAlbumRegistrationStatusList.Count && !includePhotoIdHashSet.Contains(item.staticData.GetId()));
		PrjUtil.InsertionSort<PhotoPackData>(ref photoPackList, new Comparison<PhotoPackData>(DataManager.DmPhoto.ComparePhotoPackDataNyName));
		PrjUtil.InsertionSort<PhotoPackData>(ref photoPackList, new Comparison<PhotoPackData>(DataManager.DmPhoto.ComparePhotoPackDataByType));
		PrjUtil.InsertionSort<PhotoPackData>(ref photoPackList, new Comparison<PhotoPackData>(this.SortPhotoList));
		if (this.SortOrder)
		{
			photoPackList.Reverse();
		}
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x000491AC File Offset: 0x000473AC
	private void SortTargetCharaList(ref List<CharaPackData> charaPackList, List<CharaPackData> ignoreList, List<CharaPackData> disableListAfter, List<CharaPackData> bannedCharaList = null)
	{
		if (ignoreList != null)
		{
			ignoreList = ignoreList.Where<CharaPackData>((CharaPackData item) => item.staticData.baseData.charaName.Contains(this.includeFriendsSearchText)).ToList<CharaPackData>();
		}
		charaPackList.RemoveAll((CharaPackData item) => item != null && !item.IsInvalid() && (ignoreList == null || !ignoreList.Contains(item)) && this.includeCharaAttribute.Count > 0 && !this.includeCharaAttribute.Contains(item.staticData.baseData.attribute));
		if (0 < this.includeFriendsSearchText.Length)
		{
			charaPackList = charaPackList.Where<CharaPackData>((CharaPackData item) => item.staticData.baseData.charaName.Contains(this.includeFriendsSearchText)).ToList<CharaPackData>();
		}
		if (this.isFilterHanamaru[0])
		{
			charaPackList.RemoveAll((CharaPackData item) => item.staticData.baseData.OriginalId == 0);
		}
		if (this.isFilterHanamaru[1])
		{
			charaPackList.RemoveAll((CharaPackData item) => item.staticData.baseData.OriginalId != 0);
		}
		if (this.isFilterFavoriteList[0])
		{
			charaPackList.RemoveAll((CharaPackData item) => !item.dynamicData.favoriteFlag);
		}
		if (this.isFilterFavoriteList[1])
		{
			charaPackList.RemoveAll((CharaPackData item) => item.dynamicData.favoriteFlag);
		}
		List<int> bonusIdList = this.GetBonusCharaIdList();
		if (bonusIdList != null && bonusIdList.Count > 0)
		{
			charaPackList.RemoveAll((CharaPackData item) => !bonusIdList.Contains(item.id));
		}
		this.FriendsFileterMiracle(ref charaPackList);
		this.FriendsFitlerCharacteristic(ref charaPackList);
		PrjUtil.InsertionSort<CharaPackData>(ref charaPackList, delegate(CharaPackData a, CharaPackData b)
		{
			if (!this.SortOrder)
			{
				return a.id.CompareTo(b.id);
			}
			return b.id.CompareTo(a.id);
		});
		if (SortFilterDefine.SortType.RARITY == this.sortType)
		{
			PrjUtil.InsertionSort<CharaPackData>(ref charaPackList, (CharaPackData a, CharaPackData b) => ((a.dynamicData.levelLimitId <= 3) ? 0 : a.dynamicData.levelLimitId).CompareTo((b.dynamicData.levelLimitId <= 3) ? 0 : b.dynamicData.levelLimitId));
			PrjUtil.InsertionSort<CharaPackData>(ref charaPackList, (CharaPackData a, CharaPackData b) => a.staticData.baseData.rankHigh.CompareTo(b.staticData.baseData.rankHigh));
		}
		else if (SortFilterDefine.SortType.KIZUNA == this.sortType)
		{
			PrjUtil.InsertionSort<CharaPackData>(ref charaPackList, (CharaPackData a, CharaPackData b) => a.dynamicData.kizunaExp.CompareTo(b.dynamicData.kizunaExp));
		}
		if (SortFilterDefine.RegisterType.CHARA_HELPER_LOAN == this.RegisterType)
		{
			PrjUtil.InsertionSort<CharaPackData>(ref charaPackList, new Comparison<CharaPackData>(this.SortCharaList));
		}
		else
		{
			PrjUtil.InsertionSort<CharaPackData>(ref charaPackList, new Comparison<CharaPackData>(this.SortIncludeKemoBoardBonusParamCharaList));
		}
		if (this.SortOrder)
		{
			charaPackList.Reverse();
		}
		if (bannedCharaList != null)
		{
			List<CharaPackData> list = new List<CharaPackData>();
			foreach (CharaPackData charaPackData in charaPackList)
			{
				if (bannedCharaList.Contains(charaPackData))
				{
					list.Add(charaPackData);
				}
			}
			foreach (CharaPackData charaPackData2 in list)
			{
				charaPackList.Remove(charaPackData2);
				charaPackList.Add(charaPackData2);
			}
		}
		if (disableListAfter != null)
		{
			List<CharaPackData> list2 = new List<CharaPackData>();
			foreach (CharaPackData charaPackData3 in charaPackList)
			{
				if (disableListAfter.Contains(charaPackData3))
				{
					list2.Add(charaPackData3);
				}
			}
			foreach (CharaPackData charaPackData4 in list2)
			{
				charaPackList.Remove(charaPackData4);
				charaPackList.Add(charaPackData4);
			}
		}
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x00049558 File Offset: 0x00047758
	private void SortTargetHelperList(ref List<HelperPackData> helperPackList, List<HelperPackData> ignoreList, List<HelperPackData> disableListAfter)
	{
		helperPackList.RemoveAll((HelperPackData item) => item != null && (ignoreList == null || !ignoreList.Contains(item)) && this.includeCharaAttribute.Count > 0);
		List<HelperPackData> list = new List<HelperPackData>();
		if (this.IsNeedCharaPackDataSortType)
		{
			list = helperPackList.FindAll((HelperPackData item) => item.HelperCharaSetList[(int)CanvasManager.HdlOpenWindowSortFilter.AttributeType].helpChara == null);
			if (list.Count > 0)
			{
				foreach (HelperPackData helperPackData in list)
				{
					if (helperPackList.Contains(helperPackData))
					{
						helperPackList.Remove(helperPackData);
					}
				}
				PrjUtil.InsertionSort<HelperPackData>(ref list, (HelperPackData a, HelperPackData b) => b.lastLoginTime.CompareTo(a.lastLoginTime));
			}
		}
		PrjUtil.InsertionSort<HelperPackData>(ref helperPackList, (HelperPackData a, HelperPackData b) => a.friendId.CompareTo(b.friendId));
		if (SortFilterDefine.SortType.RARITY == this.sortType)
		{
			PrjUtil.InsertionSort<HelperPackData>(ref helperPackList, (HelperPackData a, HelperPackData b) => a.HelperCharaSetList[(int)CanvasManager.HdlOpenWindowSortFilter.AttributeType].helpChara.staticData.baseData.rankHigh.CompareTo(b.HelperCharaSetList[(int)CanvasManager.HdlOpenWindowSortFilter.AttributeType].helpChara.staticData.baseData.rankHigh));
		}
		PrjUtil.InsertionSort<HelperPackData>(ref helperPackList, new Comparison<HelperPackData>(this.SortHelperList));
		if (this.SortOrder)
		{
			helperPackList.Reverse();
		}
		if (list.Count > 0)
		{
			helperPackList.AddRange(list);
		}
		if (disableListAfter != null)
		{
			List<HelperPackData> list2 = new List<HelperPackData>();
			foreach (HelperPackData helperPackData2 in helperPackList)
			{
				if (disableListAfter.Contains(helperPackData2))
				{
					list2.Add(helperPackData2);
				}
			}
			foreach (HelperPackData helperPackData3 in list2)
			{
				helperPackList.Remove(helperPackData3);
				helperPackList.Add(helperPackData3);
			}
		}
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x0004976C File Offset: 0x0004796C
	public List<int> GetBonusCharaIdList()
	{
		List<int> list = new List<int>();
		List<int> eventIdList = DataManager.DmEvent.GetValidEventIdListWithoutMissionEvent();
		int i = 0;
		Predicate<int> <>9__0;
		while (i < eventIdList.Count && i < this.EventFilterButtonNum)
		{
			List<DataManagerChara.BonusCharaData> bonusCharaDataList = DataManager.DmChara.GetBonusCharaDataList(eventIdList[i]);
			if (bonusCharaDataList.Count != 0)
			{
				List<int> list2 = this.includeCharaBonus;
				Predicate<int> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (int item) => item == eventIdList[i]);
				}
				if (list2.Exists(predicate))
				{
					foreach (DataManagerChara.BonusCharaData bonusCharaData in bonusCharaDataList)
					{
						if (!list.Contains(bonusCharaData.charaId))
						{
							list.Add(bonusCharaData.charaId);
						}
					}
				}
			}
			int num = i + 1;
			i = num;
		}
		return list;
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x00049884 File Offset: 0x00047A84
	public void SolutionList(ref SortWindowCtrl.SortTarget target, List<CharaPackData> bannedCharaList = null)
	{
		if (target.photoList != null)
		{
			if (SortFilterDefine.RegisterType.PHOTO_ALBUM == this.RegisterType)
			{
				this.SortAlbumPhotoList(ref target.photoList, target.photoAlbumRegistrationStatusMap);
				return;
			}
			this.FilteringSortTargetPhotoList(ref target.photoList, target.disableFilterPhotoList, target.lowerDisableSortPhotoList, target.upperDisableSortPhotoList, target.basePhotoPackData);
			return;
		}
		else
		{
			if (target.charaList != null)
			{
				this.SortTargetCharaList(ref target.charaList, target.disableFilterCharaList, target.disableSortCharaList, bannedCharaList);
				return;
			}
			if (target.helperList != null)
			{
				this.SortTargetHelperList(ref target.helperList, target.disableFilterHelperList, target.disableSortHelperList);
			}
			return;
		}
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x00049930 File Offset: 0x00047B30
	private int CalcSortCharaList(CharaPackData a, CharaPackData b, DataManagerKemoBoard.KemoBoardBonusParam kbpA, DataManagerKemoBoard.KemoBoardBonusParam kbpB)
	{
		int num = 0;
		PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(a.dynamicData, null, null, null);
		PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByChara(b.dynamicData, null, null, null);
		switch (this.sortType)
		{
		case SortFilterDefine.SortType.LEVEL:
			num = a.dynamicData.level - b.dynamicData.level;
			break;
		case SortFilterDefine.SortType.HP:
			num = paramPreset.hp + kbpA.Hp - (paramPreset2.hp + kbpB.Hp);
			break;
		case SortFilterDefine.SortType.ATK:
			num = paramPreset.atk + kbpA.Attack - (paramPreset2.atk + kbpB.Attack);
			break;
		case SortFilterDefine.SortType.DEF:
			num = paramPreset.def + kbpA.Difence - (paramPreset2.def + kbpB.Difence);
			break;
		case SortFilterDefine.SortType.RARITY:
			num = a.dynamicData.rank - b.dynamicData.rank;
			break;
		case SortFilterDefine.SortType.NEW:
			num = a.dynamicData.insertTime.CompareTo(b.dynamicData.insertTime);
			break;
		case SortFilterDefine.SortType.NAME:
			num = CharaUtil.SortCharaName(a.staticData, b.staticData);
			break;
		case SortFilterDefine.SortType.KEMO_STATUS:
			num = paramPreset.totalParam + kbpA.KemoStatus - (paramPreset2.totalParam + kbpB.KemoStatus);
			break;
		case SortFilterDefine.SortType.KIZUNA:
			num = a.dynamicData.kizunaLevel - b.dynamicData.kizunaLevel;
			break;
		case SortFilterDefine.SortType.PHOTO_POCKET:
			num = a.dynamicData.PhotoFrameTotalStep - b.dynamicData.PhotoFrameTotalStep;
			break;
		case SortFilterDefine.SortType.WILD_RELEASE:
			num = a.dynamicData.promoteNum - b.dynamicData.promoteNum;
			break;
		case SortFilterDefine.SortType.AVOIDANCE:
			num = paramPreset.avoid + kbpA.Avoid - (paramPreset2.avoid + kbpB.Avoid);
			break;
		case SortFilterDefine.SortType.PLASM_POINT:
			num = a.staticData.baseData.plasmPoint - b.staticData.baseData.plasmPoint;
			break;
		}
		return num;
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x00049B3C File Offset: 0x00047D3C
	private int SortIncludeKemoBoardBonusParamCharaList(CharaPackData a, CharaPackData b)
	{
		return this.CalcSortCharaList(a, b, DataManager.DmKemoBoard.KemoBoardBonusParamMap[a.staticData.baseData.attribute], DataManager.DmKemoBoard.KemoBoardBonusParamMap[b.staticData.baseData.attribute]);
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x00049B8F File Offset: 0x00047D8F
	private int SortCharaList(CharaPackData a, CharaPackData b)
	{
		return this.CalcSortCharaList(a, b, new DataManagerKemoBoard.KemoBoardBonusParam(a.staticData.baseData.attribute), new DataManagerKemoBoard.KemoBoardBonusParam(b.staticData.baseData.attribute));
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x00049BC4 File Offset: 0x00047DC4
	private int SortPhotoList(PhotoPackData a, PhotoPackData b)
	{
		int num = 0;
		PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByPhoto(a);
		PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByPhoto(b);
		switch (this.sortType)
		{
		case SortFilterDefine.SortType.LEVEL:
			num = a.dynamicData.level - b.dynamicData.level;
			break;
		case SortFilterDefine.SortType.HP:
			num = paramPreset.hp - paramPreset2.hp;
			break;
		case SortFilterDefine.SortType.ATK:
			num = paramPreset.atk - paramPreset2.atk;
			break;
		case SortFilterDefine.SortType.DEF:
			num = paramPreset.def - paramPreset2.def;
			break;
		case SortFilterDefine.SortType.RARITY:
			num = a.staticData.baseData.rarity - b.staticData.baseData.rarity;
			break;
		case SortFilterDefine.SortType.NEW:
			num = a.dynamicData.insertTime.CompareTo(b.dynamicData.insertTime);
			break;
		case SortFilterDefine.SortType.BREAKTHROUGH_LIMIT:
			num = a.dynamicData.levelRank - b.dynamicData.levelRank;
			break;
		}
		return num;
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x00049CC0 File Offset: 0x00047EC0
	private int SortHelperList(HelperPackData a, HelperPackData b)
	{
		int num = 0;
		if (this.IsNeedCharaPackDataSortType)
		{
			CharaPackData helpChara = a.HelperCharaSetList[(int)CanvasManager.HdlOpenWindowSortFilter.AttributeType].helpChara;
			CharaPackData helpChara2 = b.HelperCharaSetList[(int)CanvasManager.HdlOpenWindowSortFilter.AttributeType].helpChara;
			PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(helpChara.dynamicData, null, null, null);
			PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByChara(helpChara2.dynamicData, null, null, null);
			switch (this.sortType)
			{
			case SortFilterDefine.SortType.LEVEL:
				num = helpChara.dynamicData.level - helpChara2.dynamicData.level;
				break;
			case SortFilterDefine.SortType.HP:
				num = paramPreset.hp - paramPreset2.hp;
				break;
			case SortFilterDefine.SortType.ATK:
				num = paramPreset.atk - paramPreset2.atk;
				break;
			case SortFilterDefine.SortType.DEF:
				num = paramPreset.def - paramPreset2.def;
				break;
			case SortFilterDefine.SortType.RARITY:
				num = helpChara.dynamicData.rank - helpChara2.dynamicData.rank;
				break;
			case SortFilterDefine.SortType.KEMO_STATUS:
				num = paramPreset.totalParam - paramPreset2.totalParam;
				break;
			}
		}
		else
		{
			switch (this.sortType)
			{
			case SortFilterDefine.SortType.NEW:
				num = ((this.RegisterType == SortFilterDefine.RegisterType.HELP_FOLLOW) ? a.sendFollowTime.CompareTo(b.sendFollowTime) : a.receiveFollowTime.CompareTo(b.receiveFollowTime));
				break;
			case SortFilterDefine.SortType.LOGIN:
				num = a.lastLoginTime.CompareTo(b.lastLoginTime);
				break;
			case SortFilterDefine.SortType.USER_RANK:
				num = a.level - b.level;
				break;
			}
		}
		return num;
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x00049E70 File Offset: 0x00048070
	private void PhotoFilteringBuffStatus(ref List<PhotoPackData> photoPackList)
	{
		this.BuffFilteringCondition(ref photoPackList);
		this.BuffFilteringTarget(ref photoPackList);
		switch (this.AndORStatusEffect)
		{
		case SortFilterDefine.AndOrState.And:
			this.BuffFilteringEffectAnd(ref photoPackList);
			break;
		case SortFilterDefine.AndOrState.Or:
			this.BuffFilteringEffectOr(ref photoPackList);
			break;
		}
		this.BuffFilteringAbnormal(ref photoPackList, this.AndORStatusAbnormal);
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x00049EC4 File Offset: 0x000480C4
	private CharaStaticAbility GetPhotoAbilityData(PhotoPackData ppd)
	{
		CharaStaticAbility charaStaticAbility;
		if (ppd.dynamicData.levelRank < 4)
		{
			charaStaticAbility = ppd.staticData.abilityData;
		}
		else
		{
			charaStaticAbility = ppd.staticData.abilityDataMax;
		}
		return charaStaticAbility;
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x00049EFC File Offset: 0x000480FC
	private void BuffFilteringCondition(ref List<PhotoPackData> photoPackList)
	{
		DateTime now = TimeManager.Now;
		if (0 < this.BuffConditionsList.Count)
		{
			List<PhotoPackData> list = new List<PhotoPackData>();
			foreach (PhotoPackData photoPackData in photoPackList)
			{
				CharaStaticAbility photoAbilityData = this.GetPhotoAbilityData(photoPackData);
				bool flag = false;
				foreach (DataManagerPhoto.PhotoCharacteristicData photoCharacteristicData in this.BuffConditionsList)
				{
					if (flag)
					{
						break;
					}
					foreach (DataManagerPhoto.PhotoCharacteristicData.FilterElementOne filterElementOne in photoCharacteristicData.ElementList)
					{
						if (flag)
						{
							break;
						}
						if (!(now < filterElementOne.FilteringStartDatetime))
						{
							switch (filterElementOne.FilterType)
							{
							case SortFilterDefine.FilterElementType.Condition:
							{
								if (((photoAbilityData != null) ? photoAbilityData.buffList : null) == null)
								{
									flag = true;
									continue;
								}
								using (List<CharaBuffParamAbility>.Enumerator enumerator4 = photoAbilityData.buffList.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										CharaBuffParamAbility charaBuffParamAbility = enumerator4.Current;
										if (flag)
										{
											break;
										}
										CharaDef.ConditionType condition = filterElementOne.Condition;
										if (condition != CharaDef.ConditionType.UPPER)
										{
											if (condition == CharaDef.ConditionType.LOWER)
											{
												if (charaBuffParamAbility.condition == CharaDef.ConditionType.LOWER && 100 != charaBuffParamAbility.conditionHpRate)
												{
													list.Add(photoPackData);
													flag = true;
												}
											}
										}
										else if (charaBuffParamAbility.condition == CharaDef.ConditionType.UPPER && charaBuffParamAbility.conditionHpRate != 0)
										{
											list.Add(photoPackData);
											flag = true;
										}
									}
									continue;
								}
								break;
							}
							case SortFilterDefine.FilterElementType.Terrain:
								break;
							case SortFilterDefine.FilterElementType.Night:
								goto IL_01D2;
							default:
								continue;
							}
							if (((photoAbilityData != null) ? photoAbilityData.buffList : null) == null)
							{
								flag = true;
								continue;
							}
							using (List<CharaBuffParamAbility>.Enumerator enumerator4 = photoAbilityData.buffList.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									CharaBuffParamAbility charaBuffParamAbility2 = enumerator4.Current;
									if (flag)
									{
										break;
									}
									if (charaBuffParamAbility2.traitsTerrain == filterElementOne.Traits)
									{
										list.Add(photoPackData);
										flag = true;
										break;
									}
								}
								continue;
							}
							IL_01D2:
							if (((photoAbilityData != null) ? photoAbilityData.buffList : null) == null)
							{
								flag = true;
							}
							else
							{
								foreach (CharaBuffParamAbility charaBuffParamAbility3 in photoAbilityData.buffList)
								{
									if (flag)
									{
										break;
									}
									if (charaBuffParamAbility3.traitsTimezone == CharaDef.AbilityTraits2.night)
									{
										list.Add(photoPackData);
										flag = true;
										break;
									}
								}
							}
						}
					}
				}
			}
			photoPackList = list;
		}
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x0004A22C File Offset: 0x0004842C
	private void BuffFilteringTarget(ref List<PhotoPackData> photoPackList)
	{
		DateTime now = TimeManager.Now;
		if (0 < this.BuffTargetList.Count)
		{
			List<PhotoPackData> list = new List<PhotoPackData>();
			foreach (PhotoPackData photoPackData in photoPackList)
			{
				CharaStaticAbility photoAbilityData = this.GetPhotoAbilityData(photoPackData);
				bool flag = false;
				foreach (DataManagerPhoto.PhotoCharacteristicData photoCharacteristicData in this.BuffTargetList)
				{
					if (flag)
					{
						break;
					}
					foreach (DataManagerPhoto.PhotoCharacteristicData.FilterElementOne filterElementOne in photoCharacteristicData.ElementList)
					{
						if (flag)
						{
							break;
						}
						if (!(now < filterElementOne.FilteringStartDatetime) && filterElementOne.FilterType == SortFilterDefine.FilterElementType.Target)
						{
							if (((photoAbilityData != null) ? photoAbilityData.buffList : null) == null)
							{
								flag = true;
							}
							else
							{
								foreach (CharaBuffParamAbility charaBuffParamAbility in photoAbilityData.buffList)
								{
									if (filterElementOne.TargetType == charaBuffParamAbility.targetType)
									{
										list.Add(photoPackData);
										flag = true;
										break;
									}
								}
							}
						}
					}
				}
			}
			photoPackList = list;
		}
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x0004A3F8 File Offset: 0x000485F8
	private void BuffFilteringEffectAnd(ref List<PhotoPackData> photoPackList)
	{
		DateTime now = TimeManager.Now;
		if (0 < this.BuffEffectList.Count)
		{
			List<PhotoPackData> list = new List<PhotoPackData>();
			foreach (PhotoPackData photoPackData in photoPackList)
			{
				CharaStaticAbility photoAbilityData = this.GetPhotoAbilityData(photoPackData);
				foreach (DataManagerPhoto.PhotoCharacteristicData photoCharacteristicData in this.BuffEffectList)
				{
					bool flag = false;
					bool flag2 = false;
					foreach (DataManagerPhoto.PhotoCharacteristicData.FilterElementOne filterElementOne in photoCharacteristicData.ElementList)
					{
						if (flag)
						{
							break;
						}
						if (!(now < filterElementOne.FilteringStartDatetime))
						{
							SortFilterDefine.FilterElementType filterType = filterElementOne.FilterType;
							if (filterType != SortFilterDefine.FilterElementType.Buff)
							{
								if (filterType != SortFilterDefine.FilterElementType.Guts)
								{
									continue;
								}
							}
							else
							{
								if (((photoAbilityData != null) ? photoAbilityData.buffList : null) == null)
								{
									flag = true;
									continue;
								}
								using (List<CharaBuffParamAbility>.Enumerator enumerator4 = photoAbilityData.buffList.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										CharaBuffParamAbility charaBuffParamAbility = enumerator4.Current;
										if (filterElementOne.BuffType == charaBuffParamAbility.buffType)
										{
											flag2 = true;
											flag = true;
											break;
										}
									}
									continue;
								}
							}
							if (((photoAbilityData != null) ? photoAbilityData.gutsList : null) == null)
							{
								flag = true;
							}
							else if (0 < photoAbilityData.gutsList.Count)
							{
								flag2 = true;
								flag = true;
							}
						}
					}
					if (!flag2)
					{
						list.Add(photoPackData);
						break;
					}
				}
			}
			foreach (PhotoPackData photoPackData2 in list)
			{
				photoPackList.Remove(photoPackData2);
			}
		}
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x0004A648 File Offset: 0x00048848
	private void BuffFilteringEffectOr(ref List<PhotoPackData> photoPackList)
	{
		DateTime now = TimeManager.Now;
		if (0 < this.BuffEffectList.Count)
		{
			List<PhotoPackData> list = new List<PhotoPackData>();
			foreach (PhotoPackData photoPackData in photoPackList)
			{
				CharaStaticAbility photoAbilityData = this.GetPhotoAbilityData(photoPackData);
				bool flag = false;
				foreach (DataManagerPhoto.PhotoCharacteristicData photoCharacteristicData in this.BuffEffectList)
				{
					if (flag)
					{
						break;
					}
					foreach (DataManagerPhoto.PhotoCharacteristicData.FilterElementOne filterElementOne in photoCharacteristicData.ElementList)
					{
						if (flag)
						{
							break;
						}
						if (!(now < filterElementOne.FilteringStartDatetime))
						{
							SortFilterDefine.FilterElementType filterType = filterElementOne.FilterType;
							if (filterType != SortFilterDefine.FilterElementType.Buff)
							{
								if (filterType != SortFilterDefine.FilterElementType.Guts)
								{
									continue;
								}
							}
							else
							{
								if (((photoAbilityData != null) ? photoAbilityData.buffList : null) == null)
								{
									flag = true;
									continue;
								}
								using (List<CharaBuffParamAbility>.Enumerator enumerator4 = photoAbilityData.buffList.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										CharaBuffParamAbility charaBuffParamAbility = enumerator4.Current;
										if (filterElementOne.BuffType == charaBuffParamAbility.buffType)
										{
											list.Add(photoPackData);
											flag = true;
											break;
										}
									}
									continue;
								}
							}
							if (((photoAbilityData != null) ? photoAbilityData.gutsList : null) == null)
							{
								flag = true;
							}
							else if (0 < photoAbilityData.gutsList.Count)
							{
								list.Add(photoPackData);
								flag = true;
							}
						}
					}
				}
			}
			photoPackList = list;
		}
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x0004A854 File Offset: 0x00048A54
	private void BuffFilteringAbnormal(ref List<PhotoPackData> photoPackList, SortFilterDefine.AndOrState AndOrState)
	{
		if (this.BuffAbnormalEnablelList.Count <= 0)
		{
			return;
		}
		long num = 0L;
		foreach (DataManagerPhoto.PhotoCharacteristicData photoCharacteristicData in this.BuffAbnormalEnablelList)
		{
			foreach (DataManagerPhoto.PhotoCharacteristicData.FilterElementOne filterElementOne in photoCharacteristicData.ElementList)
			{
				num |= CharaDef.AbnormalMask(filterElementOne.AbnormalMask, filterElementOne.AbnormalMask2);
			}
		}
		List<PhotoPackData> list = new List<PhotoPackData>();
		foreach (PhotoPackData photoPackData in photoPackList)
		{
			long num2 = 0L;
			CharaStaticAbility photoAbilityData = this.GetPhotoAbilityData(photoPackData);
			if (!(photoAbilityData == null))
			{
				foreach (CharaBuffParamAbility charaBuffParamAbility in photoAbilityData.buffList)
				{
					num2 |= charaBuffParamAbility.abTyp;
				}
				switch (AndOrState)
				{
				case SortFilterDefine.AndOrState.Invalid:
					return;
				case SortFilterDefine.AndOrState.And:
					if ((num & num2) == num)
					{
						list.Add(photoPackData);
					}
					break;
				case SortFilterDefine.AndOrState.Or:
					if ((num & num2) != 0L)
					{
						list.Add(photoPackData);
					}
					break;
				}
			}
		}
		photoPackList = list;
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x0004A9E4 File Offset: 0x00048BE4
	public void FriendsFileterMiracle(ref List<CharaPackData> charaPackDataList)
	{
		this.FriendsMiracleTarget(ref charaPackDataList);
		this.FriendsMiracleEffect(ref charaPackDataList);
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x0004A9F4 File Offset: 0x00048BF4
	public void FriendsMiracleTarget(ref List<CharaPackData> charaPackDataList)
	{
		List<CharaPackData> list = new List<CharaPackData>();
		using (List<CharaPackData>.Enumerator enumerator = charaPackDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CharaPackData charaPackData = enumerator.Current;
				Predicate<CharaPackData> <>9__2;
				foreach (DataManagerChara.FilterData filterData in this.miracleTargetList)
				{
					using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
							CharaStaticAction artsData = charaPackData.staticData.artsData;
							CharaDamageParam charaDamageParam = artsData.damageList.Find((CharaDamageParam attack) => attack.targetType == element.TargetType);
							CharaBuffParam charaBuffParam = artsData.buffList.Find((CharaBuffParam buff) => buff.targetType == element.TargetType);
							if (charaDamageParam != null || charaBuffParam != null)
							{
								List<CharaPackData> list2 = list;
								Predicate<CharaPackData> predicate;
								if ((predicate = <>9__2) == null)
								{
									predicate = (<>9__2 = (CharaPackData item) => item == charaPackData);
								}
								if (list2.Find(predicate) == null)
								{
									list.Add(charaPackData);
									break;
								}
								break;
							}
						}
					}
				}
			}
		}
		if (0 < this.miracleTargetList.Count)
		{
			charaPackDataList = list;
		}
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0004AB98 File Offset: 0x00048D98
	public void FriendsMiracleEffect(ref List<CharaPackData> charaPackDataList)
	{
		switch (this.miracleEffectAndOrStatus)
		{
		case SortFilterDefine.AndOrState.Invalid:
			break;
		case SortFilterDefine.AndOrState.And:
			this.FriendsMiracleEffectAnd(ref charaPackDataList);
			return;
		case SortFilterDefine.AndOrState.Or:
			this.FriendsMiracleEffectOr(ref charaPackDataList);
			break;
		default:
			return;
		}
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x0004ABD0 File Offset: 0x00048DD0
	private void FriendsMiracleEffectAnd(ref List<CharaPackData> charaPackDataList)
	{
		if (this.miracleEffectList.Count <= 0)
		{
			return;
		}
		List<CharaPackData> list = new List<CharaPackData>();
		foreach (CharaPackData charaPackData in charaPackDataList)
		{
			int num = 0;
			if (this.isCharaHaveAttack(charaPackData))
			{
				num++;
			}
			foreach (DataManagerChara.FilterData filterData in this.miracleEffectList)
			{
				using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
						if (charaPackData.staticData.artsData.buffList.Find((CharaBuffParam buff) => buff.buffType == element.BuffType) != null)
						{
							num++;
							break;
						}
					}
				}
			}
			if (num == this.miracleEffectList.Count)
			{
				list.Add(charaPackData);
			}
		}
		charaPackDataList = list;
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x0004AD08 File Offset: 0x00048F08
	private void FriendsMiracleEffectOr(ref List<CharaPackData> charaPackDataList)
	{
		if (this.miracleEffectList.Count <= 0)
		{
			return;
		}
		List<CharaPackData> list = new List<CharaPackData>();
		foreach (CharaPackData charaPackData in charaPackDataList)
		{
			bool flag = false;
			foreach (DataManagerChara.FilterData filterData in this.miracleEffectList)
			{
				using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
						if (charaPackData.staticData.artsData.buffList.Find((CharaBuffParam buff) => buff.buffType == element.BuffType) != null || this.isCharaHaveAttack(charaPackData))
						{
							list.Add(charaPackData);
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		charaPackDataList = list;
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x0004AE38 File Offset: 0x00049038
	private bool isCharaHaveAttack(CharaPackData charaPackData)
	{
		bool flag = false;
		foreach (DataManagerChara.FilterData filterData in this.miracleEffectList)
		{
			using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator2 = filterData.ElementList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.BuffType == CharaDef.ActionBuffType.ATTACK)
					{
						flag = true;
					}
				}
			}
		}
		return flag && charaPackData.staticData.artsData.damageList.Count != 0;
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x0004AEE8 File Offset: 0x000490E8
	private void FriendsFitlerCharacteristic(ref List<CharaPackData> charaPackDataList)
	{
		this.FriendsCharacteristicCondition(ref charaPackDataList);
		this.FriendsCharacteristicTarget(ref charaPackDataList);
		this.FriendsCharacteristicEffect(ref charaPackDataList);
		this.FriendsCharacteristicResist(ref charaPackDataList);
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x0004AF08 File Offset: 0x00049108
	private void FriendsCharacteristicCondition(ref List<CharaPackData> charaPackDataList)
	{
		if (this.characteristicConditionList.Count <= 0)
		{
			return;
		}
		List<CharaPackData> list = new List<CharaPackData>();
		list = this.GetConditionRegistration(this.characteristicConditionList);
		if (list.Count <= 0)
		{
			foreach (CharaPackData charaPackData2 in DataManager.DmChara.GetUserCharaMap().Values)
			{
				bool flag = false;
				foreach (DataManagerChara.FilterData filterData in this.characteristicConditionList)
				{
					if (flag)
					{
						break;
					}
					foreach (DataManagerChara.FilterData.FilterElementOne filterElementOne in filterData.ElementList)
					{
						foreach (CharaBuffParamAbility charaBuffParamAbility in this.GetCharaAbilityList(charaPackData2))
						{
							switch (filterElementOne.FilterType)
							{
							case SortFilterDefine.FilterElementType.Condition:
								if (filterElementOne.Condition != CharaDef.ConditionType.EQUAL && !flag)
								{
									if (charaBuffParamAbility.condition == CharaDef.ConditionType.UPPER && charaBuffParamAbility.conditionHpRate != 0)
									{
										list.Add(charaPackData2);
										flag = true;
									}
									else if (charaBuffParamAbility.condition == CharaDef.ConditionType.LOWER && 100 != charaBuffParamAbility.conditionHpRate)
									{
										list.Add(charaPackData2);
										flag = true;
									}
								}
								break;
							case SortFilterDefine.FilterElementType.Terrain:
								if (!flag && charaBuffParamAbility.traitsTerrain == filterElementOne.Traits)
								{
									list.Add(charaPackData2);
									flag = true;
								}
								break;
							case SortFilterDefine.FilterElementType.Night:
								if (!flag && charaBuffParamAbility.traitsTimezone == CharaDef.AbilityTraits2.night)
								{
									list.Add(charaPackData2);
									flag = true;
								}
								break;
							}
						}
					}
				}
			}
		}
		this.registCondition(this.characteristicConditionList, list);
		List<CharaPackData> list2 = new List<CharaPackData>();
		using (List<CharaPackData>.Enumerator enumerator5 = charaPackDataList.GetEnumerator())
		{
			while (enumerator5.MoveNext())
			{
				CharaPackData charaPackData = enumerator5.Current;
				if (list.Find((CharaPackData data) => data.id == charaPackData.id) != null)
				{
					list2.Add(charaPackData);
				}
			}
		}
		charaPackDataList = list2;
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x0004B1CC File Offset: 0x000493CC
	private void FriendsCharacteristicTarget(ref List<CharaPackData> charaPackDataList)
	{
		if (this.characteristicTargetList.Count <= 0)
		{
			return;
		}
		List<CharaPackData> list = new List<CharaPackData>();
		foreach (CharaPackData charaPackData in charaPackDataList)
		{
			bool flag = false;
			List<CharaBuffParamAbility> buffList = charaPackData.staticData.abilityData[0].buffList;
			foreach (DataManagerChara.FilterData filterData in this.characteristicTargetList)
			{
				if (flag)
				{
					break;
				}
				using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
						if (this.GetCharaAbilityList(charaPackData).Find((CharaBuffParamAbility item) => item.targetType == element.TargetType) != null)
						{
							list.Add(charaPackData);
							flag = true;
							break;
						}
					}
				}
			}
		}
		charaPackDataList = list;
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x0004B300 File Offset: 0x00049500
	private void FriendsCharacteristicEffect(ref List<CharaPackData> charaPackDataList)
	{
		if (this.characteristicEffectList.Count <= 0)
		{
			return;
		}
		switch (this.characteristicEffectAndOrStatus)
		{
		case SortFilterDefine.AndOrState.Invalid:
			break;
		case SortFilterDefine.AndOrState.And:
			this.FriendsCharacteristicEffectAnd(ref charaPackDataList);
			return;
		case SortFilterDefine.AndOrState.Or:
			this.FriendsCharacteristicEffectOr(ref charaPackDataList);
			break;
		default:
			return;
		}
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x0004B348 File Offset: 0x00049548
	private void FriendsCharacteristicEffectAnd(ref List<CharaPackData> charaPackDataList)
	{
		List<CharaPackData> list = new List<CharaPackData>();
		foreach (CharaPackData charaPackData in charaPackDataList)
		{
			int num = 0;
			foreach (DataManagerChara.FilterData filterData in this.characteristicEffectList)
			{
				using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
						if (this.GetCharaAbilityList(charaPackData).Find((CharaBuffParamAbility buff) => buff.buffType == element.BuffType) != null)
						{
							num++;
							break;
						}
					}
				}
			}
			if (num == this.characteristicEffectList.Count)
			{
				list.Add(charaPackData);
			}
		}
		charaPackDataList = list;
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x0004B45C File Offset: 0x0004965C
	private void FriendsCharacteristicEffectOr(ref List<CharaPackData> charaPackDataList)
	{
		List<CharaPackData> list = new List<CharaPackData>();
		foreach (CharaPackData charaPackData in charaPackDataList)
		{
			bool flag = false;
			foreach (DataManagerChara.FilterData filterData in this.characteristicEffectList)
			{
				if (flag)
				{
					break;
				}
				using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
						if (this.GetCharaAbilityList(charaPackData).Find((CharaBuffParamAbility buff) => buff.buffType == element.BuffType) != null)
						{
							list.Add(charaPackData);
							flag = true;
							break;
						}
					}
				}
			}
		}
		charaPackDataList = list;
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x0004B568 File Offset: 0x00049768
	private void FriendsCharacteristicResist(ref List<CharaPackData> charaPackDataList)
	{
		if (this.characteristicResistList.Count <= 0)
		{
			return;
		}
		long num = 0L;
		foreach (DataManagerChara.FilterData filterData in this.characteristicResistList)
		{
			foreach (DataManagerChara.FilterData.FilterElementOne filterElementOne in filterData.ElementList)
			{
				num |= CharaDef.AbnormalMask(filterElementOne.AbnormalMask, filterElementOne.AbnormalMask2);
			}
		}
		List<CharaPackData> list = new List<CharaPackData>();
		foreach (CharaPackData charaPackData in charaPackDataList)
		{
			long num2 = 0L;
			foreach (CharaBuffParamAbility charaBuffParamAbility in this.GetCharaAbilityList(charaPackData))
			{
				num2 |= charaBuffParamAbility.abTyp;
			}
			switch (this.characteristicResistAndOrStatus)
			{
			case SortFilterDefine.AndOrState.Invalid:
				return;
			case SortFilterDefine.AndOrState.And:
				if ((num & num2) == num)
				{
					list.Add(charaPackData);
				}
				break;
			case SortFilterDefine.AndOrState.Or:
				if ((num & num2) != 0L)
				{
					list.Add(charaPackData);
				}
				break;
			}
		}
		charaPackDataList = list;
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x0004B6EC File Offset: 0x000498EC
	private List<CharaBuffParamAbility> GetCharaAbilityList(CharaPackData charaData)
	{
		List<CharaBuffParamAbility> list = new List<CharaBuffParamAbility>();
		if (this.charaAbilityMemoData.ContainsKey(charaData.id))
		{
			return this.charaAbilityMemoData[charaData.id];
		}
		foreach (CharaStaticAbility charaStaticAbility in charaData.staticData.abilityData)
		{
			foreach (CharaBuffParamAbility charaBuffParamAbility in charaStaticAbility.buffList)
			{
				list.Add(charaBuffParamAbility);
			}
		}
		if (charaData.staticData.spAbilityData != null)
		{
			using (List<CharaBuffParamAbility>.Enumerator enumerator2 = charaData.staticData.spAbilityData.buffList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CharaBuffParamAbility spBuff = enumerator2.Current;
					if (list.Find((CharaBuffParamAbility buff) => buff == spBuff) == null)
					{
						list.Add(spBuff);
					}
				}
			}
		}
		if (charaData.staticData.nanairoAbilityData != null)
		{
			using (List<CharaBuffParamAbility>.Enumerator enumerator2 = charaData.staticData.nanairoAbilityData.buffList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CharaBuffParamAbility nanairoBuff = enumerator2.Current;
					if (list.Find((CharaBuffParamAbility buff) => buff == nanairoBuff) == null)
					{
						list.Add(nanairoBuff);
					}
				}
			}
		}
		this.charaAbilityMemoData.Add(charaData.id, list);
		return list;
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x0004B8CC File Offset: 0x00049ACC
	private List<CharaPackData> GetConditionRegistration(List<DataManagerChara.FilterData> conditionList)
	{
		string assetBundleURL = LoginManager.AssetBundleURL;
		if (this.beforeAssetNum != assetBundleURL)
		{
			this.conditionRegistMemoData.Clear();
			this.beforeAssetNum = assetBundleURL;
		}
		if (this.conditionRegistMemoData.ContainsKey(conditionList))
		{
			return this.conditionRegistMemoData[conditionList];
		}
		return new List<CharaPackData>();
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x0004B91F File Offset: 0x00049B1F
	private void registCondition(List<DataManagerChara.FilterData> conditionList, List<CharaPackData> filterCharaPackdataList)
	{
		if (this.conditionRegistMemoData.ContainsKey(conditionList))
		{
			this.conditionRegistMemoData.Add(conditionList, filterCharaPackdataList);
		}
	}

	// Token: 0x04000971 RID: 2417
	public string includePhotoSearchText = "";

	// Token: 0x04000972 RID: 2418
	public List<ItemDef.Rarity> includePhotoRarityList = new List<ItemDef.Rarity>();

	// Token: 0x04000973 RID: 2419
	public List<PhotoDef.Type> includePhotoTypeList = new List<PhotoDef.Type>();

	// Token: 0x04000974 RID: 2420
	public List<SortFilterDefine.PhotoAlbumRegistrationStatus> includePhotoAlbumRegistrationStatusList = new List<SortFilterDefine.PhotoAlbumRegistrationStatus>();

	// Token: 0x04000975 RID: 2421
	public bool includePhotoBonus;

	// Token: 0x04000976 RID: 2422
	public bool includePhotoLimit;

	// Token: 0x04000977 RID: 2423
	public List<int> includeCharaBonus = new List<int>();

	// Token: 0x04000978 RID: 2424
	public bool[] isFilterFavoritePhotoList = new bool[2];

	// Token: 0x04000979 RID: 2425
	public List<DataManagerPhoto.PhotoCharacteristicData> BuffConditionsList = new List<DataManagerPhoto.PhotoCharacteristicData>();

	// Token: 0x0400097A RID: 2426
	public List<DataManagerPhoto.PhotoCharacteristicData> BuffTargetList = new List<DataManagerPhoto.PhotoCharacteristicData>();

	// Token: 0x0400097B RID: 2427
	public List<DataManagerPhoto.PhotoCharacteristicData> BuffEffectList = new List<DataManagerPhoto.PhotoCharacteristicData>();

	// Token: 0x0400097C RID: 2428
	public SortFilterDefine.AndOrState AndORStatusEffect;

	// Token: 0x0400097D RID: 2429
	public List<DataManagerPhoto.PhotoCharacteristicData> BuffAbnormalEnablelList = new List<DataManagerPhoto.PhotoCharacteristicData>();

	// Token: 0x0400097E RID: 2430
	public SortFilterDefine.AndOrState AndORStatusAbnormal;

	// Token: 0x0400097F RID: 2431
	public List<string> enableButtonNameList = new List<string>();

	// Token: 0x04000980 RID: 2432
	public SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	// Token: 0x04000981 RID: 2433
	public string includeFriendsSearchText = "";

	// Token: 0x04000982 RID: 2434
	public List<CharaDef.AttributeType> includeCharaAttribute = new List<CharaDef.AttributeType>();

	// Token: 0x04000983 RID: 2435
	public bool[] isFilterHanamaru = new bool[2];

	// Token: 0x04000984 RID: 2436
	public bool[] isFilterFavoriteList = new bool[2];

	// Token: 0x04000985 RID: 2437
	public List<DataManagerChara.FilterData> miracleTargetList = new List<DataManagerChara.FilterData>();

	// Token: 0x04000986 RID: 2438
	public List<DataManagerChara.FilterData> miracleEffectList = new List<DataManagerChara.FilterData>();

	// Token: 0x04000987 RID: 2439
	public SortFilterDefine.AndOrState miracleEffectAndOrStatus = SortFilterDefine.AndOrState.Or;

	// Token: 0x04000988 RID: 2440
	public List<DataManagerChara.FilterData> characteristicConditionList = new List<DataManagerChara.FilterData>();

	// Token: 0x04000989 RID: 2441
	public List<DataManagerChara.FilterData> characteristicTargetList = new List<DataManagerChara.FilterData>();

	// Token: 0x0400098A RID: 2442
	public List<DataManagerChara.FilterData> characteristicEffectList = new List<DataManagerChara.FilterData>();

	// Token: 0x0400098B RID: 2443
	public SortFilterDefine.AndOrState characteristicEffectAndOrStatus = SortFilterDefine.AndOrState.Or;

	// Token: 0x0400098C RID: 2444
	public List<DataManagerChara.FilterData> characteristicResistList = new List<DataManagerChara.FilterData>();

	// Token: 0x0400098D RID: 2445
	public SortFilterDefine.AndOrState characteristicResistAndOrStatus = SortFilterDefine.AndOrState.Or;

	// Token: 0x0400098E RID: 2446
	public List<int> miracleTargetButtonIdxList = new List<int>();

	// Token: 0x0400098F RID: 2447
	public List<int> miracleEffectButtonIdxList = new List<int>();

	// Token: 0x04000990 RID: 2448
	public List<int> characteristicConditionButtonIdxList = new List<int>();

	// Token: 0x04000991 RID: 2449
	public List<int> characteristicTargetButtonIdxList = new List<int>();

	// Token: 0x04000992 RID: 2450
	public List<int> characteristicEffectButtonIdxList = new List<int>();

	// Token: 0x04000993 RID: 2451
	public List<int> characteristicResistButtonIdxList = new List<int>();

	// Token: 0x04000994 RID: 2452
	public Dictionary<List<DataManagerChara.FilterData>, List<CharaPackData>> conditionRegistMemoData = new Dictionary<List<DataManagerChara.FilterData>, List<CharaPackData>>();

	// Token: 0x04000995 RID: 2453
	public string beforeAssetNum;

	// Token: 0x04000996 RID: 2454
	public Dictionary<int, List<CharaBuffParamAbility>> charaAbilityMemoData = new Dictionary<int, List<CharaBuffParamAbility>>();
}
