using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class SortWindowCtrl : MonoBehaviour
{
	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x06001F17 RID: 7959 RVA: 0x001813E7 File Offset: 0x0017F5E7
	// (set) Token: 0x06001F18 RID: 7960 RVA: 0x001813EF File Offset: 0x0017F5EF
	public CharaDef.AttributeType AttributeType { get; private set; }

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x06001F19 RID: 7961 RVA: 0x001813F8 File Offset: 0x0017F5F8
	// (set) Token: 0x06001F1A RID: 7962 RVA: 0x00181400 File Offset: 0x0017F600
	private Dictionary<SortFilterDefine.RegisterType, DataManagerGameStatus.UserFlagData.SortTypeData> SortTypeDataMap { get; set; }

	// Token: 0x06001F1B RID: 7963 RVA: 0x0018140C File Offset: 0x0017F60C
	public bool Initialize()
	{
		this.registerSaveMap = new Dictionary<SortFilterDefine.RegisterType, RegisterSaveData>();
		DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
		foreach (object obj in Enum.GetValues(typeof(SortFilterDefine.RegisterType)))
		{
			SortFilterDefine.RegisterType registerType = (SortFilterDefine.RegisterType)obj;
			if (registerType != SortFilterDefine.RegisterType.INVALID)
			{
				this.registerSaveMap.Add(registerType, new RegisterSaveData(userFlagData, registerType));
			}
		}
		return true;
	}

	// Token: 0x06001F1C RID: 7964 RVA: 0x00181494 File Offset: 0x0017F694
	public void Create(GameObject baseWindow)
	{
		this.guiData = new SortWindowCtrl.GUI(baseWindow.transform);
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.BtnList)
		{
			pguiToggleButtonCtrl.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickButton));
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.guiData.Btn_EventList)
		{
			pguiToggleButtonCtrl2.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickButton));
		}
		this.registerMap = new Dictionary<SortFilterDefine.RegisterType, SortWindowCtrl.RegisterData>();
		this.registerSaveMap = new Dictionary<SortFilterDefine.RegisterType, RegisterSaveData>();
		this.SortTypeDataMap = new Dictionary<SortFilterDefine.RegisterType, DataManagerGameStatus.UserFlagData.SortTypeData>();
	}

	// Token: 0x06001F1D RID: 7965 RVA: 0x00181578 File Offset: 0x0017F778
	public void InitializeFriendsFilter(GameObject friendsFilterWindow)
	{
		this.guiFriendsFilterData = new SortWindowCtrl.GUICharaFilter(friendsFilterWindow.transform);
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiFriendsFilterData.Btn)
		{
			pguiToggleButtonCtrl.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickButton));
		}
		this.guiFriendsFilterData.Btn_Hanamaru.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickButton));
		this.guiFriendsFilterData.Btn_Favorite.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickButton));
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.guiFriendsFilterData.Btn_EventList)
		{
			pguiToggleButtonCtrl2.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickButton));
		}
	}

	// Token: 0x06001F1E RID: 7966 RVA: 0x00181674 File Offset: 0x0017F874
	public void Register(SortWindowCtrl.RegisterData regData, bool isSolutionList, List<CharaPackData> bannedCharaList = null)
	{
		this.bannedCharaList = bannedCharaList;
		this.registerMap[regData.register] = regData;
		SortFilterDefine.RegisterType registType = regData.register;
		if (regData.filterButton != null)
		{
			regData.filterButton.AddOnClickListener(delegate(PguiButtonCtrl btn)
			{
				this.OnClickRegisterButton(registType, btn);
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}
		if (regData.sortButton != null)
		{
			regData.sortButton.AddOnClickListener(delegate(PguiButtonCtrl btn)
			{
				this.OnClickRegisterButton(registType, btn);
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}
		if (regData.sortUdButton != null)
		{
			regData.sortUdButton.AddOnClickListener(delegate(PguiButtonCtrl btn)
			{
				this.OnClickRegisterButton(registType, btn);
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}
		this.DecorationRegisterButton(regData);
		if (isSolutionList)
		{
			this.SolutionList(regData.register, null);
		}
	}

	// Token: 0x06001F1F RID: 7967 RVA: 0x0018173C File Offset: 0x0017F93C
	public void RegistSortCharaAttribute(CharaDef.AttributeType attr)
	{
		this.AttributeType = attr;
	}

	// Token: 0x06001F20 RID: 7968 RVA: 0x00181748 File Offset: 0x0017F948
	public void SolutionList(SortFilterDefine.RegisterType regType, List<CharaPackData> bannedCharaList = null)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			RegisterSaveData registerSaveData = this.registerSaveMap[regType];
			registerSaveData.EventFilterButtonNum = this.guiFriendsFilterData.Btn_EventList.Count;
			SortWindowCtrl.SortTarget sortTarget = this.registerMap[regType].funcGetTargetBaseList();
			sortTarget.includeFriendsSearchText = registerSaveData.includeFriendsSearchText;
			sortTarget.includePhotoSearchText = registerSaveData.includePhotoSearchText;
			SortWindowCtrl.SortTarget sortTarget2 = new SortWindowCtrl.SortTarget(sortTarget);
			List<CharaPackData> list = ((bannedCharaList == null) ? this.bannedCharaList : bannedCharaList);
			registerSaveData.SolutionList(ref sortTarget2, list);
			sortTarget2.sortType = registerSaveData.sortType;
			sortTarget2.includePhotoBonus = registerSaveData.includePhotoBonus;
			this.registerMap[regType].funcDisideTarget(sortTarget2);
		}
	}

	// Token: 0x06001F21 RID: 7969 RVA: 0x00181802 File Offset: 0x0017FA02
	public SortWindowCtrl.RegisterData GetRegisterData(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerMap[regType];
		}
		return null;
	}

	// Token: 0x06001F22 RID: 7970 RVA: 0x00181820 File Offset: 0x0017FA20
	public List<CharaDef.AttributeType> GetAttributeType(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].includeCharaAttribute;
		}
		return null;
	}

	// Token: 0x06001F23 RID: 7971 RVA: 0x00181843 File Offset: 0x0017FA43
	public List<int> GetBonusCharaIdList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].GetBonusCharaIdList();
		}
		return null;
	}

	// Token: 0x06001F24 RID: 7972 RVA: 0x00181866 File Offset: 0x0017FA66
	public bool[] GetHanamaru(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].isFilterHanamaru;
		}
		return null;
	}

	// Token: 0x06001F25 RID: 7973 RVA: 0x00181889 File Offset: 0x0017FA89
	public bool[] GetFavorite(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].isFilterFavoriteList;
		}
		return null;
	}

	// Token: 0x06001F26 RID: 7974 RVA: 0x001818AC File Offset: 0x0017FAAC
	public List<DataManagerChara.FilterData> GetMiracleTargetList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].miracleTargetList;
		}
		return null;
	}

	// Token: 0x06001F27 RID: 7975 RVA: 0x001818CF File Offset: 0x0017FACF
	public List<DataManagerChara.FilterData> GetMiracleEffectList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].miracleEffectList;
		}
		return null;
	}

	// Token: 0x06001F28 RID: 7976 RVA: 0x001818F2 File Offset: 0x0017FAF2
	public SortFilterDefine.AndOrState GetMiracleAndOrStatus(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].miracleEffectAndOrStatus;
		}
		return SortFilterDefine.AndOrState.Invalid;
	}

	// Token: 0x06001F29 RID: 7977 RVA: 0x00181915 File Offset: 0x0017FB15
	public List<DataManagerChara.FilterData> GetCharacteristicTargetList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicTargetList;
		}
		return null;
	}

	// Token: 0x06001F2A RID: 7978 RVA: 0x00181938 File Offset: 0x0017FB38
	public List<DataManagerChara.FilterData> GetCharacteristicConditionList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicConditionList;
		}
		return null;
	}

	// Token: 0x06001F2B RID: 7979 RVA: 0x0018195B File Offset: 0x0017FB5B
	public List<DataManagerChara.FilterData> GetCharacteristicEffectList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicEffectList;
		}
		return null;
	}

	// Token: 0x06001F2C RID: 7980 RVA: 0x0018197E File Offset: 0x0017FB7E
	public List<DataManagerChara.FilterData> GetCharacteristicResistList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicResistList;
		}
		return null;
	}

	// Token: 0x06001F2D RID: 7981 RVA: 0x001819A1 File Offset: 0x0017FBA1
	public SortFilterDefine.AndOrState GetCharacteristicEffectAndOrStatus(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicEffectAndOrStatus;
		}
		return SortFilterDefine.AndOrState.Invalid;
	}

	// Token: 0x06001F2E RID: 7982 RVA: 0x001819C4 File Offset: 0x0017FBC4
	public SortFilterDefine.AndOrState GetCharacteristicResistAndOrStatus(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicResistAndOrStatus;
		}
		return SortFilterDefine.AndOrState.Invalid;
	}

	// Token: 0x06001F2F RID: 7983 RVA: 0x001819E8 File Offset: 0x0017FBE8
	public static SortFilterDefine.SortFilterType RegisterType2SortType(SortFilterDefine.RegisterType regType)
	{
		SortFilterDefine.SortFilterType sortFilterType = SortFilterDefine.SortFilterType.INVALID;
		switch (regType)
		{
		case SortFilterDefine.RegisterType.CHARA_ALL:
		case SortFilterDefine.RegisterType.CHARA_DECK:
		case SortFilterDefine.RegisterType.CHARA_FAVORITE:
		case SortFilterDefine.RegisterType.CHARA_HELPER_LOAN:
		case SortFilterDefine.RegisterType.CHARA_GROW_TOP:
		case SortFilterDefine.RegisterType.CHARA_QUEST_TOP:
		case SortFilterDefine.RegisterType.HOME_CLOSET:
		case SortFilterDefine.RegisterType.CHARA_EVENT_GROW:
		case SortFilterDefine.RegisterType.STORY_VIEW:
		case SortFilterDefine.RegisterType.CHARA_DECK_PVP:
		case SortFilterDefine.RegisterType.PICNIC_CHANGE:
		case SortFilterDefine.RegisterType.CHARA_DECK_TRAINING:
		case SortFilterDefine.RegisterType.TREEHOUSE_CHANGE:
		case SortFilterDefine.RegisterType.CHARA_COMMUNICATION:
			sortFilterType = SortFilterDefine.SortFilterType.CHARA_SORT;
			break;
		case SortFilterDefine.RegisterType.PHOTO_DECK:
		case SortFilterDefine.RegisterType.PHOTO_GROW_TOP:
		case SortFilterDefine.RegisterType.PHOTO_GROW_MAIN:
		case SortFilterDefine.RegisterType.PHOTO_SELL_MAIN:
		case SortFilterDefine.RegisterType.PHOTO_ALL:
		case SortFilterDefine.RegisterType.PHOTO_HELPER_LOAN:
		case SortFilterDefine.RegisterType.PHOTO_DECK_PVP:
		case SortFilterDefine.RegisterType.PHOTO_DECK_TRAINING:
		case SortFilterDefine.RegisterType.PHOTO_ALBUM:
			sortFilterType = SortFilterDefine.SortFilterType.PHOTO_SORT;
			break;
		case SortFilterDefine.RegisterType.HELP_FOLLOW:
		case SortFilterDefine.RegisterType.HELP_FOLLOWER:
			sortFilterType = SortFilterDefine.SortFilterType.FOLLOW_SORT;
			break;
		}
		return sortFilterType;
	}

	// Token: 0x06001F30 RID: 7984 RVA: 0x00181A70 File Offset: 0x0017FC70
	public void RequestActionUpdateSortType()
	{
		if (this.SortTypeDataMap == null)
		{
			return;
		}
		DataManager.DmGameStatus.RequestActionUpdateSortType(new List<DataManagerGameStatus.UserFlagData.SortTypeData>(this.SortTypeDataMap.Values));
		this.SortTypeDataMap.Clear();
	}

	// Token: 0x06001F31 RID: 7985 RVA: 0x00181AA0 File Offset: 0x0017FCA0
	private void DecorationRegisterButton(SortWindowCtrl.RegisterData regData)
	{
		SortFilterDefine.RegisterType register = regData.register;
		if (regData.filterButton != null)
		{
			Transform transform = regData.filterButton.transform.Find("BaseImage/On");
			Transform transform2 = regData.filterButton.transform.Find("BaseImage/Off");
			if (transform != null && transform2 != null)
			{
				bool flag;
				if (regData.funcGetTargetBaseList().photoList != null)
				{
					flag = this.registerSaveMap[register].includePhotoBonus || this.registerSaveMap[register].includePhotoLimit || this.registerSaveMap[register].isFilterFavoritePhotoList[0] || this.registerSaveMap[register].isFilterFavoritePhotoList[1] || 0 < this.registerSaveMap[register].includePhotoRarityList.Count || 0 < this.registerSaveMap[register].includePhotoTypeList.Count || 0 < this.registerSaveMap[register].includePhotoAlbumRegistrationStatusList.Count || 0 < this.registerSaveMap[register].BuffConditionsList.Count || 0 < this.registerSaveMap[register].BuffTargetList.Count || 0 < this.registerSaveMap[register].BuffEffectList.Count || 0 < this.registerSaveMap[register].BuffAbnormalEnablelList.Count || 0 < this.registerSaveMap[register].includePhotoSearchText.Length;
				}
				else
				{
					flag = this.registerSaveMap[register].includeCharaAttribute.Count > 0 || this.registerSaveMap[register].includeCharaBonus.Count > 0 || this.registerSaveMap[register].isFilterHanamaru[0] || this.registerSaveMap[register].isFilterHanamaru[1] || this.registerSaveMap[register].isFilterFavoriteList[0] || this.registerSaveMap[register].isFilterFavoriteList[1] || this.registerSaveMap[register].miracleTargetList.Count > 0 || this.registerSaveMap[register].miracleEffectList.Count > 0 || this.registerSaveMap[register].characteristicConditionList.Count > 0 || this.registerSaveMap[register].characteristicTargetList.Count > 0 || this.registerSaveMap[register].characteristicEffectList.Count > 0 || this.registerSaveMap[register].characteristicResistList.Count > 0 || this.registerSaveMap[register].includeFriendsSearchText.Length > 0;
				}
				transform.gameObject.SetActive(flag);
				transform2.gameObject.SetActive(!flag);
			}
		}
		if (regData.sortButton != null)
		{
			Transform transform3 = regData.sortButton.transform.Find("BaseImage/Txt_btn");
			if (transform3 != null)
			{
				List<PhotoPackData> photoList = regData.funcGetTargetBaseList().photoList;
				switch (SortWindowCtrl.RegisterType2SortType(register))
				{
				case SortFilterDefine.SortFilterType.CHARA_SORT:
					transform3.GetComponent<PguiTextCtrl>().text = SortFilterDefine.SortTypeDispNameMap[this.registerSaveMap[register].sortType];
					goto IL_03F8;
				case SortFilterDefine.SortFilterType.PHOTO_SORT:
					transform3.GetComponent<PguiTextCtrl>().text = SortFilterDefine.SortTypeDispNameMap[this.registerSaveMap[register].sortType];
					goto IL_03F8;
				}
				transform3.GetComponent<PguiTextCtrl>().text = SortFilterDefine.SortTypeDispNameMap[this.registerSaveMap[register].sortType];
			}
		}
		IL_03F8:
		if (regData.sortUdButton != null)
		{
			Transform transform4 = regData.sortUdButton.transform.Find("BaseImage/Img_Up");
			Transform transform5 = regData.sortUdButton.transform.Find("BaseImage/Img_Down");
			if (transform4 != null && transform5 != null)
			{
				transform4.gameObject.SetActive(!this.registerSaveMap[register].SortOrder);
				transform5.gameObject.SetActive(this.registerSaveMap[register].SortOrder);
			}
		}
	}

	// Token: 0x06001F32 RID: 7986 RVA: 0x00181F34 File Offset: 0x00180134
	private void Open()
	{
		RegisterSaveData registerSaveData = this.registerSaveMap[this.currentRegisterType];
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.Btn_EventList)
		{
			pguiToggleButtonCtrl.gameObject.SetActive(false);
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.guiFriendsFilterData.Btn_EventList)
		{
			pguiToggleButtonCtrl2.gameObject.SetActive(false);
		}
		switch (this.currentFilterType)
		{
		case SortFilterDefine.SortFilterType.CHARA_SORT:
		{
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl3 in this.guiData.BtnList)
			{
				pguiToggleButtonCtrl3.gameObject.SetActive(false);
			}
			int num = 0;
			while (num < SortFilterDefine.CharaSortTypeList.Count && num < this.guiData.BtnList.Count)
			{
				this.guiData.BtnList[num].gameObject.SetActive(true);
				this.guiData.Num_TxtList[num].text = SortFilterDefine.SortTypeDispNameMap[SortFilterDefine.CharaSortTypeList[num]];
				num++;
			}
			this.guiData.baseWindow.Setup(PrjUtil.MakeMessage("ソート順の設定"), PrjUtil.MakeMessage("並び順の基準とする条件を設定できます"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), null, false);
			int num2 = 0;
			while (num2 < SortFilterDefine.CharaSortTypeList.Count && num2 < this.guiData.BtnList.Count)
			{
				this.guiData.BtnList[num2].SetToggleIndex((SortFilterDefine.CharaSortTypeList[num2] == registerSaveData.sortType) ? 1 : 0);
				num2++;
			}
			this.guiData.baseWindow.Open();
			return;
		}
		case SortFilterDefine.SortFilterType.CHARA_FILTER:
		{
			this.guiFriendsFilterData.baseWindow.Setup(PrjUtil.MakeMessage("フィルターの設定"), PrjUtil.MakeMessage("選択したカテゴリで絞り込みができます"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), null, false);
			for (int i = 0; i < this.guiFriendsFilterData.Btn.Count; i++)
			{
				this.guiFriendsFilterData.Btn[i].SetToggleIndex(registerSaveData.includeCharaAttribute.Contains(i + CharaDef.AttributeType.RED) ? 1 : 0);
			}
			this.guiFriendsFilterData.Btn_Hanamaru.SetToggleIndex(registerSaveData.isFilterHanamaru[0] ? 1 : 0);
			this.guiFriendsFilterData.Btn_Hanamaru.SetToggleIndex(registerSaveData.isFilterHanamaru[1] ? 1 : 0);
			this.guiFriendsFilterData.Btn_Favorite.SetToggleIndex(registerSaveData.isFilterFavoriteList[0] ? 1 : 0);
			this.guiFriendsFilterData.Btn_Favorite.SetToggleIndex(registerSaveData.isFilterFavoriteList[1] ? 1 : 0);
			CanvasManager.HdlFriendsFilterWindowCtrl.SetupWindow(registerSaveData.enableButtonNameList, registerSaveData.AndORStatusEffect, registerSaveData.AndORStatusAbnormal);
			CanvasManager.HdlFriendsFilterWindowCtrl.SetupAttributeTupe(registerSaveData.includeCharaAttribute);
			CanvasManager.HdlFriendsFilterWindowCtrl.SetupFavorite(registerSaveData.isFilterFavoriteList);
			CanvasManager.HdlFriendsFilterWindowCtrl.SetupFriend(registerSaveData.isFilterHanamaru);
			CanvasManager.HdlFriendsFilterWindowCtrl.SetupEvent(registerSaveData.includeCharaBonus);
			CanvasManager.HdlFriendsFilterWindowCtrl.SetupSearchText(registerSaveData.includeFriendsSearchText);
			CanvasManager.HdlFriendsFilterWindowCtrl.SetupCharacteristic(registerSaveData.characteristicConditionButtonIdxList, registerSaveData.characteristicTargetButtonIdxList, registerSaveData.characteristicEffectButtonIdxList, registerSaveData.characteristicResistButtonIdxList);
			CanvasManager.HdlFriendsFilterWindowCtrl.SetupMiracle(registerSaveData.miracleTargetButtonIdxList, registerSaveData.miracleEffectButtonIdxList);
			CanvasManager.HdlFriendsFilterWindowCtrl.SetupSearchTextActive();
			this.guiFriendsFilterData.baseWindow.Open();
			return;
		}
		case SortFilterDefine.SortFilterType.PHOTO_SORT:
		{
			List<SortFilterDefine.SortType> list = ((SortFilterDefine.RegisterType.PHOTO_ALBUM == registerSaveData.RegisterType) ? SortFilterDefine.PhotoAlbumSortTypeList : SortFilterDefine.PhotoSortTypeList);
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl4 in this.guiData.BtnList)
			{
				pguiToggleButtonCtrl4.gameObject.SetActive(false);
			}
			for (int j = 0; j < list.Count; j++)
			{
				this.guiData.BtnList[j].gameObject.SetActive(true);
				this.guiData.Num_TxtList[j].text = SortFilterDefine.SortTypeDispNameMap[list[j]];
			}
			this.guiData.baseWindow.Setup(PrjUtil.MakeMessage("ソート順の設定"), PrjUtil.MakeMessage("並び順の基準とする条件を設定できます"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), null, false);
			for (int k = 0; k < list.Count; k++)
			{
				this.guiData.BtnList[k].SetToggleIndex((list[k] == registerSaveData.sortType) ? 1 : 0);
			}
			this.guiData.baseWindow.Open();
			return;
		}
		case SortFilterDefine.SortFilterType.PHOTO_FILTER:
		{
			SortFilterDefine.RegisterType registerType = registerSaveData.RegisterType;
			SortFilterDefine.PhotoFilterType photoFilterType;
			if (registerType != SortFilterDefine.RegisterType.PHOTO_GROW_MAIN)
			{
				if (registerType == SortFilterDefine.RegisterType.PHOTO_ALBUM)
				{
					photoFilterType = SortFilterDefine.PhotoFilterType.PhotoAlbum;
				}
				else
				{
					photoFilterType = SortFilterDefine.PhotoFilterType.SortFilter;
				}
			}
			else
			{
				photoFilterType = SortFilterDefine.PhotoFilterType.PhotoGrow;
			}
			this.guiFriendsFilterData.Btn_Favorite.SetToggleIndex(registerSaveData.isFilterFavoritePhotoList[0] ? 1 : 0);
			this.guiFriendsFilterData.Btn_Favorite.SetToggleIndex(registerSaveData.isFilterFavoritePhotoList[1] ? 1 : 0);
			CanvasManager.HdlPhotoFilterWindowCtrl.SetupWindow(photoFilterType, registerSaveData.enableButtonNameList, registerSaveData.AndORStatusEffect, registerSaveData.AndORStatusAbnormal);
			CanvasManager.HdlPhotoFilterWindowCtrl.SetupRarity(registerSaveData.includePhotoRarityList);
			CanvasManager.HdlPhotoFilterWindowCtrl.SetUpPhotoType(registerSaveData.includePhotoTypeList);
			CanvasManager.HdlPhotoFilterWindowCtrl.SetupSpecial(registerSaveData.includePhotoBonus, registerSaveData.includePhotoLimit);
			CanvasManager.HdlPhotoFilterWindowCtrl.SetupFavorite(registerSaveData.isFilterFavoritePhotoList);
			if (SortFilterDefine.RegisterType.PHOTO_ALBUM == registerSaveData.RegisterType)
			{
				CanvasManager.HdlPhotoFilterWindowCtrl.SetupAlbum(registerSaveData.includePhotoAlbumRegistrationStatusList);
			}
			CanvasManager.HdlPhotoFilterWindowCtrl.SetupSearchText(registerSaveData.includePhotoSearchText);
			CanvasManager.HdlPhotoFilterWindowCtrl.SetupSearchTextActive();
			CanvasManager.HdlPhotoFilterWindowCtrl.Open();
			return;
		}
		case SortFilterDefine.SortFilterType.FOLLOW_SORT:
		{
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl5 in this.guiData.BtnList)
			{
				pguiToggleButtonCtrl5.gameObject.SetActive(false);
			}
			for (int l = 0; l < SortFilterDefine.HelperSortTypeList.Count; l++)
			{
				this.guiData.BtnList[l].gameObject.SetActive(true);
				this.guiData.Num_TxtList[l].text = SortFilterDefine.SortTypeDispNameMap[SortFilterDefine.HelperSortTypeList[l]];
			}
			this.guiData.baseWindow.Setup(PrjUtil.MakeMessage("ソート順の設定"), PrjUtil.MakeMessage("並び順の基準とする条件を設定できます"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), null, false);
			for (int m = 0; m < SortFilterDefine.HelperSortTypeList.Count; m++)
			{
				this.guiData.BtnList[m].SetToggleIndex((SortFilterDefine.HelperSortTypeList[m] == registerSaveData.sortType) ? 1 : 0);
			}
			this.guiData.baseWindow.Open();
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06001F33 RID: 7987 RVA: 0x001826B4 File Offset: 0x001808B4
	private void cb(bool b, SortFilterDefine.SortType t)
	{
	}

	// Token: 0x06001F34 RID: 7988 RVA: 0x001826B6 File Offset: 0x001808B6
	private void ResetSearchText()
	{
		if (this.currentFilterType == SortFilterDefine.SortFilterType.CHARA_FILTER)
		{
			CanvasManager.HdlFriendsFilterWindowCtrl.ResetSearchText();
		}
	}

	// Token: 0x06001F35 RID: 7989 RVA: 0x001826CC File Offset: 0x001808CC
	private void OnClickRegisterButton(SortFilterDefine.RegisterType regType, PguiButtonCtrl button)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			SortWindowCtrl.RegisterData registerData = this.registerMap[regType];
			this.currentRegisterType = regType;
			this.currentSortTarget = new SortWindowCtrl.SortTarget(registerData.funcGetTargetBaseList());
			if (registerData.filterButton == button)
			{
				this.currentFilterType = ((this.currentSortTarget.photoList != null) ? SortFilterDefine.SortFilterType.PHOTO_FILTER : SortFilterDefine.SortFilterType.CHARA_FILTER);
				this.Open();
				return;
			}
			if (registerData.sortButton == button)
			{
				if (this.currentSortTarget.photoList == null)
				{
					this.currentFilterType = ((this.currentSortTarget.helperList != null) ? SortFilterDefine.SortFilterType.FOLLOW_SORT : SortFilterDefine.SortFilterType.CHARA_SORT);
				}
				else if (this.currentSortTarget.helperList == null)
				{
					this.currentFilterType = ((this.currentSortTarget.photoList != null) ? SortFilterDefine.SortFilterType.PHOTO_SORT : SortFilterDefine.SortFilterType.CHARA_SORT);
				}
				else
				{
					this.currentFilterType = ((this.currentSortTarget.photoList != null) ? SortFilterDefine.SortFilterType.PHOTO_SORT : SortFilterDefine.SortFilterType.FOLLOW_SORT);
				}
				this.Open();
				return;
			}
			if (registerData.sortUdButton == button)
			{
				RegisterSaveData registerSaveData = this.registerSaveMap[this.currentRegisterType];
				registerSaveData.SortOrder = !registerSaveData.SortOrder;
				registerSaveData.SolutionList(ref this.currentSortTarget, null);
				this.currentSortTarget.sortType = registerSaveData.sortType;
				this.currentSortTarget.includePhotoBonus = registerSaveData.includePhotoBonus;
				registerData.funcDisideTarget(this.currentSortTarget);
				this.DecorationRegisterButton(registerData);
				this.SortTypeDataMap[this.currentRegisterType] = new DataManagerGameStatus.UserFlagData.SortTypeData(this.currentRegisterType, this.currentSortTarget.sortType, registerSaveData.SortOrder);
			}
		}
	}

	// Token: 0x06001F36 RID: 7990 RVA: 0x00182860 File Offset: 0x00180A60
	private bool OnClickWindowButton(int index)
	{
		switch (index)
		{
		case -1:
			this.ResetSearchText();
			break;
		case 0:
			this.ResetSearchText();
			break;
		case 1:
		{
			SortWindowCtrl.RegisterData registerData = this.registerMap[this.currentRegisterType];
			RegisterSaveData registerSaveData = this.registerSaveMap[this.currentRegisterType];
			switch (this.currentFilterType)
			{
			case SortFilterDefine.SortFilterType.CHARA_SORT:
				registerSaveData.sortType = SortFilterDefine.CharaSortTypeList[this.guiData.BtnList.FindIndex((PguiToggleButtonCtrl item) => item.GetToggleIndex() == 1)];
				this.RegistFriendsSort();
				break;
			case SortFilterDefine.SortFilterType.CHARA_FILTER:
				this.RegistFriendsFilter();
				break;
			case SortFilterDefine.SortFilterType.PHOTO_SORT:
			{
				List<SortFilterDefine.SortType> list = ((SortFilterDefine.RegisterType.PHOTO_ALBUM == registerSaveData.RegisterType) ? SortFilterDefine.PhotoAlbumSortTypeList : SortFilterDefine.PhotoSortTypeList);
				registerSaveData.sortType = list[this.guiData.BtnList.FindIndex((PguiToggleButtonCtrl item) => item.GetToggleIndex() == 1)];
				this.RegistPhotoSort();
				break;
			}
			case SortFilterDefine.SortFilterType.FOLLOW_SORT:
				registerSaveData.sortType = SortFilterDefine.HelperSortTypeList[this.guiData.BtnList.FindIndex((PguiToggleButtonCtrl item) => item.GetToggleIndex() == 1)];
				this.RegistFriendsSort();
				break;
			}
			break;
		}
		}
		return true;
	}

	// Token: 0x06001F37 RID: 7991 RVA: 0x001829D4 File Offset: 0x00180BD4
	private bool OnClickButton(PguiToggleButtonCtrl button, int index)
	{
		if (this.currentFilterType == SortFilterDefine.SortFilterType.CHARA_FILTER || this.currentFilterType == SortFilterDefine.SortFilterType.PHOTO_FILTER)
		{
			return true;
		}
		if (index == 0)
		{
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.BtnList)
			{
				if (pguiToggleButtonCtrl != button)
				{
					pguiToggleButtonCtrl.SetToggleIndex(0);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001F38 RID: 7992 RVA: 0x00182A50 File Offset: 0x00180C50
	public void RegistFriendsFilter()
	{
		RegisterSaveData registerSaveData = this.registerSaveMap[this.currentRegisterType];
		SortWindowCtrl.RegisterData registerData = this.registerMap[this.currentRegisterType];
		registerSaveData.includeCharaAttribute.Clear();
		registerSaveData.includeCharaAttribute = CanvasManager.HdlFriendsFilterWindowCtrl.GetAttributeButtonStatus();
		registerSaveData.includeCharaBonus = CanvasManager.HdlFriendsFilterWindowCtrl.GetEventButtonStatus();
		registerSaveData.isFilterFavoriteList = CanvasManager.HdlFriendsFilterWindowCtrl.GetFavoriteButtonStatus();
		registerSaveData.includeFriendsSearchText = CanvasManager.HdlFriendsFilterWindowCtrl.GetSearchText();
		registerSaveData.isFilterHanamaru = CanvasManager.HdlFriendsFilterWindowCtrl.GetFriendButtonStatus();
		registerSaveData.miracleTargetList = CanvasManager.HdlFriendsFilterWindowCtrl.GetMiracleTargetDataList();
		registerSaveData.miracleEffectList = CanvasManager.HdlFriendsFilterWindowCtrl.GetMiracleEffectDataList();
		registerSaveData.miracleEffectAndOrStatus = CanvasManager.HdlFriendsFilterWindowCtrl.GetMiracleAndORStatus();
		registerSaveData.characteristicConditionList = CanvasManager.HdlFriendsFilterWindowCtrl.GetCharacteristicConditionsDataList();
		registerSaveData.characteristicTargetList = CanvasManager.HdlFriendsFilterWindowCtrl.GetCharacteristicTargetDataList();
		registerSaveData.characteristicEffectList = CanvasManager.HdlFriendsFilterWindowCtrl.GetCharacteristicEffectDataList();
		registerSaveData.characteristicEffectAndOrStatus = CanvasManager.HdlFriendsFilterWindowCtrl.GetCharacteristicEffectAndOrStatus();
		registerSaveData.characteristicResistList = CanvasManager.HdlFriendsFilterWindowCtrl.GetCharacteristicResistDataList();
		registerSaveData.characteristicResistAndOrStatus = CanvasManager.HdlFriendsFilterWindowCtrl.GetCharacteristicResistAndOrStatus();
		registerSaveData.miracleTargetButtonIdxList = CanvasManager.HdlFriendsFilterWindowCtrl.getMiracleTargetButtonIdx();
		registerSaveData.miracleEffectButtonIdxList = CanvasManager.HdlFriendsFilterWindowCtrl.getMiracleEffectButtonIdx();
		registerSaveData.characteristicConditionButtonIdxList = CanvasManager.HdlFriendsFilterWindowCtrl.getCharacterisiticConditionsButtonIdx();
		registerSaveData.characteristicTargetButtonIdxList = CanvasManager.HdlFriendsFilterWindowCtrl.getCharacterisiticTargetButtonIdx();
		registerSaveData.characteristicEffectButtonIdxList = CanvasManager.HdlFriendsFilterWindowCtrl.getCharacterisiticEffectButtonIdx();
		registerSaveData.characteristicResistButtonIdxList = CanvasManager.HdlFriendsFilterWindowCtrl.getCharacterisiticResistButtonIdx();
		registerSaveData.SolutionList(ref this.currentSortTarget, this.bannedCharaList);
		this.currentSortTarget.sortType = registerSaveData.sortType;
		this.currentSortTarget.includePhotoBonus = registerSaveData.includePhotoBonus;
		this.currentSortTarget.includeFriendsSearchText = registerSaveData.includeFriendsSearchText;
		registerData.funcDisideTarget(this.currentSortTarget);
		this.DecorationRegisterButton(registerData);
		this.SortTypeDataMap[this.currentRegisterType] = new DataManagerGameStatus.UserFlagData.SortTypeData(this.currentRegisterType, this.currentSortTarget.sortType, registerSaveData.SortOrder);
	}

	// Token: 0x06001F39 RID: 7993 RVA: 0x00182C58 File Offset: 0x00180E58
	public void RegistFriendsSort()
	{
		RegisterSaveData registerSaveData = this.registerSaveMap[this.currentRegisterType];
		SortWindowCtrl.RegisterData registerData = this.registerMap[this.currentRegisterType];
		registerSaveData.SolutionList(ref this.currentSortTarget, null);
		this.currentSortTarget.sortType = registerSaveData.sortType;
		registerData.funcDisideTarget(this.currentSortTarget);
		this.DecorationRegisterButton(registerData);
		this.SortTypeDataMap[this.currentRegisterType] = new DataManagerGameStatus.UserFlagData.SortTypeData(this.currentRegisterType, this.currentSortTarget.sortType, registerSaveData.SortOrder);
	}

	// Token: 0x06001F3A RID: 7994 RVA: 0x00182CEC File Offset: 0x00180EEC
	public void RegistPhotoFilter()
	{
		SortWindowCtrl.RegisterData registerData = this.registerMap[this.currentRegisterType];
		RegisterSaveData registerSaveData = this.registerSaveMap[this.currentRegisterType];
		registerSaveData.includePhotoRarityList = CanvasManager.HdlPhotoFilterWindowCtrl.GetRarityButtonstatus();
		registerSaveData.includePhotoTypeList = CanvasManager.HdlPhotoFilterWindowCtrl.GetPhotoTypeButtonstatus();
		registerSaveData.includePhotoSearchText = CanvasManager.HdlPhotoFilterWindowCtrl.GetSearchText();
		registerSaveData.isFilterFavoritePhotoList = CanvasManager.HdlPhotoFilterWindowCtrl.GetFavoriteButtonStatus();
		if (SortFilterDefine.RegisterType.PHOTO_ALBUM == registerSaveData.RegisterType)
		{
			registerSaveData.includePhotoAlbumRegistrationStatusList = CanvasManager.HdlPhotoFilterWindowCtrl.GetPhotoAlbumButtonstatus();
		}
		registerSaveData.includePhotoBonus = CanvasManager.HdlPhotoFilterWindowCtrl.GetInvalidBonus();
		if (!DataManager.DmPhoto.IsBonusActive())
		{
			registerSaveData.includePhotoBonus = false;
		}
		registerSaveData.includePhotoLimit = CanvasManager.HdlPhotoFilterWindowCtrl.GetInvalidLimit();
		registerSaveData.AndORStatusEffect = CanvasManager.HdlPhotoFilterWindowCtrl.GetAndORStatus();
		registerSaveData.AndORStatusAbnormal = CanvasManager.HdlPhotoFilterWindowCtrl.GetAndORStatusAbnormal();
		registerSaveData.BuffConditionsList = CanvasManager.HdlPhotoFilterWindowCtrl.GetBuffConditionsDataList();
		registerSaveData.BuffTargetList = CanvasManager.HdlPhotoFilterWindowCtrl.GetBuffTargetDataList();
		registerSaveData.BuffEffectList = CanvasManager.HdlPhotoFilterWindowCtrl.GetBuffEffectDataList();
		registerSaveData.BuffAbnormalEnablelList = CanvasManager.HdlPhotoFilterWindowCtrl.GetBuffAbnormalDataList();
		registerSaveData.enableButtonNameList = new List<string>();
		foreach (DataManagerPhoto.PhotoCharacteristicData photoCharacteristicData in registerSaveData.BuffConditionsList)
		{
			registerSaveData.enableButtonNameList.Add(photoCharacteristicData.DisplayName);
		}
		foreach (DataManagerPhoto.PhotoCharacteristicData photoCharacteristicData2 in registerSaveData.BuffTargetList)
		{
			registerSaveData.enableButtonNameList.Add(photoCharacteristicData2.DisplayName);
		}
		foreach (DataManagerPhoto.PhotoCharacteristicData photoCharacteristicData3 in registerSaveData.BuffEffectList)
		{
			registerSaveData.enableButtonNameList.Add(photoCharacteristicData3.DisplayName);
		}
		foreach (DataManagerPhoto.PhotoCharacteristicData photoCharacteristicData4 in registerSaveData.BuffAbnormalEnablelList)
		{
			registerSaveData.enableButtonNameList.Add(photoCharacteristicData4.DisplayName);
		}
		registerSaveData.SolutionList(ref this.currentSortTarget, null);
		this.currentSortTarget.sortType = registerSaveData.sortType;
		this.currentSortTarget.includePhotoBonus = registerSaveData.includePhotoBonus;
		this.currentSortTarget.includePhotoSearchText = registerSaveData.includePhotoSearchText;
		registerData.funcDisideTarget(this.currentSortTarget);
		this.DecorationRegisterButton(registerData);
		this.SortTypeDataMap[this.currentRegisterType] = new DataManagerGameStatus.UserFlagData.SortTypeData(this.currentRegisterType, this.currentSortTarget.sortType, registerSaveData.SortOrder);
	}

	// Token: 0x06001F3B RID: 7995 RVA: 0x00182FD4 File Offset: 0x001811D4
	public void RegistPhotoSort()
	{
		SortWindowCtrl.RegisterData registerData = this.registerMap[this.currentRegisterType];
		RegisterSaveData registerSaveData = this.registerSaveMap[this.currentRegisterType];
		registerSaveData.SolutionList(ref this.currentSortTarget, null);
		this.currentSortTarget.sortType = registerSaveData.sortType;
		this.currentSortTarget.includePhotoBonus = registerSaveData.includePhotoBonus;
		registerData.funcDisideTarget(this.currentSortTarget);
		this.DecorationRegisterButton(registerData);
		this.SortTypeDataMap[this.currentRegisterType] = new DataManagerGameStatus.UserFlagData.SortTypeData(this.currentRegisterType, this.currentSortTarget.sortType, registerSaveData.SortOrder);
	}

	// Token: 0x06001F3C RID: 7996 RVA: 0x00183079 File Offset: 0x00181279
	public void UpdateBannedCharaList(List<CharaPackData> list)
	{
		this.bannedCharaList = list;
	}

	// Token: 0x040016AB RID: 5803
	private SortWindowCtrl.GUI guiData;

	// Token: 0x040016AC RID: 5804
	private SortWindowCtrl.GUICharaFilter guiFriendsFilterData;

	// Token: 0x040016AD RID: 5805
	private SortFilterDefine.SortFilterType currentFilterType;

	// Token: 0x040016AE RID: 5806
	private Dictionary<SortFilterDefine.RegisterType, SortWindowCtrl.RegisterData> registerMap;

	// Token: 0x040016AF RID: 5807
	private Dictionary<SortFilterDefine.RegisterType, RegisterSaveData> registerSaveMap;

	// Token: 0x040016B0 RID: 5808
	private SortFilterDefine.RegisterType currentRegisterType;

	// Token: 0x040016B1 RID: 5809
	private SortWindowCtrl.SortTarget currentSortTarget;

	// Token: 0x040016B4 RID: 5812
	private List<CharaPackData> bannedCharaList;

	// Token: 0x02000FFA RID: 4090
	public class GUI
	{
		// Token: 0x06005197 RID: 20887 RVA: 0x00246C78 File Offset: 0x00244E78
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseWindow = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Num_TxtList = new List<PguiTextCtrl>
			{
				baseTr.Find("Base/Window/Sort/Btn01/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn02/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn03/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn04/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn05/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn06/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn07/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn08/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn09/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn10/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn11/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn12/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn13/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn14/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn15/Num_Txt").GetComponent<PguiTextCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn16/Num_Txt").GetComponent<PguiTextCtrl>()
			};
			this.BtnList = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Base/Window/Sort/Btn01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn03").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn04").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn05").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn06").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn07").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn08").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn09").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn10").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn11").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn12").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn13").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn14").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn15").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn16").GetComponent<PguiToggleButtonCtrl>()
			};
			this.Btn_EventList = new List<PguiToggleButtonCtrl> { baseTr.Find("Base/Window/Sort/Btn_Event01").GetComponent<PguiToggleButtonCtrl>() };
		}

		// Token: 0x040059AD RID: 22957
		public GameObject baseObj;

		// Token: 0x040059AE RID: 22958
		public List<PguiTextCtrl> Num_TxtList;

		// Token: 0x040059AF RID: 22959
		public List<PguiToggleButtonCtrl> BtnList;

		// Token: 0x040059B0 RID: 22960
		public List<PguiToggleButtonCtrl> Btn_EventList;

		// Token: 0x040059B1 RID: 22961
		public PguiOpenWindowCtrl baseWindow;
	}

	// Token: 0x02000FFB RID: 4091
	public class GUICharaFilter
	{
		// Token: 0x06005198 RID: 20888 RVA: 0x00246F9C File Offset: 0x0024519C
		public GUICharaFilter(Transform baseTr)
		{
			baseTr.Find("Base/Window/ScrollView/Scrollbar_Vertical").GetComponent<PguiScrollbar>().SetScrollBarValue(1f);
			this.baseObj = baseTr.gameObject;
			this.baseWindow = baseTr.GetComponent<PguiOpenWindowCtrl>();
			string text = "Base/Window/ScrollView/Viewport/Content/Filter/";
			this.Btn = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find(text + "Btn01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find(text + "Btn02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find(text + "Btn03").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find(text + "Btn04").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find(text + "Btn05").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find(text + "Btn06").GetComponent<PguiToggleButtonCtrl>()
			};
			this.Btn_Hanamaru = baseTr.Find(text + "Btn_Hanamaru").GetComponent<PguiToggleButtonCtrl>();
			this.Btn_Favorite = baseTr.Find(text + "Btn_Favorite").GetComponent<PguiToggleButtonCtrl>();
			this.Btn_EventList = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find(text + "Btn_Event01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find(text + "Btn_Event02").GetComponent<PguiToggleButtonCtrl>()
			};
		}

		// Token: 0x040059B2 RID: 22962
		public GameObject baseObj;

		// Token: 0x040059B3 RID: 22963
		public List<PguiToggleButtonCtrl> Btn;

		// Token: 0x040059B4 RID: 22964
		public PguiToggleButtonCtrl Btn_Hanamaru;

		// Token: 0x040059B5 RID: 22965
		public PguiToggleButtonCtrl Btn_Favorite;

		// Token: 0x040059B6 RID: 22966
		public List<PguiToggleButtonCtrl> Btn_EventList;

		// Token: 0x040059B7 RID: 22967
		public PguiOpenWindowCtrl baseWindow;
	}

	// Token: 0x02000FFC RID: 4092
	public class SortTarget
	{
		// Token: 0x06005199 RID: 20889 RVA: 0x00247115 File Offset: 0x00245315
		public SortTarget()
		{
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x00247134 File Offset: 0x00245334
		public SortTarget(SortWindowCtrl.SortTarget st)
		{
			if (st == null)
			{
				return;
			}
			if (st.photoList != null)
			{
				this.photoList = new List<PhotoPackData>(st.photoList);
			}
			if (st.charaList != null)
			{
				this.charaList = new List<CharaPackData>(st.charaList);
			}
			if (st.helperList != null)
			{
				this.helperList = new List<HelperPackData>(st.helperList);
			}
			if (st.disableFilterPhotoList != null)
			{
				this.disableFilterPhotoList = new List<PhotoPackData>(st.disableFilterPhotoList);
			}
			if (st.disableFilterCharaList != null)
			{
				this.disableFilterCharaList = new List<CharaPackData>(st.disableFilterCharaList);
			}
			if (st.disableFilterHelperList != null)
			{
				this.disableFilterHelperList = new List<HelperPackData>(st.disableFilterHelperList);
			}
			if (st.lowerDisableSortPhotoList != null)
			{
				this.lowerDisableSortPhotoList = new List<PhotoPackData>(st.lowerDisableSortPhotoList);
			}
			if (st.upperDisableSortPhotoList != null)
			{
				this.upperDisableSortPhotoList = new List<PhotoPackData>(st.upperDisableSortPhotoList);
			}
			if (st.disableSortCharaList != null)
			{
				this.disableSortCharaList = new List<CharaPackData>(st.disableSortCharaList);
			}
			if (st.disableSortHelperList != null)
			{
				this.disableSortHelperList = new List<HelperPackData>(st.disableSortHelperList);
			}
			if (st.photoAlbumRegistrationStatusMap != null)
			{
				this.photoAlbumRegistrationStatusMap = new Dictionary<SortFilterDefine.PhotoAlbumRegistrationStatus, HashSet<int>>(st.photoAlbumRegistrationStatusMap);
			}
			if (st.basePhotoPackData != null)
			{
				this.basePhotoPackData = st.basePhotoPackData;
			}
			this.sortType = st.sortType;
			this.includePhotoBonus = st.includePhotoBonus;
			this.includeFriendsSearchText = st.includeFriendsSearchText;
			this.includePhotoSearchText = st.includePhotoSearchText;
		}

		// Token: 0x040059B8 RID: 22968
		public List<PhotoPackData> photoList;

		// Token: 0x040059B9 RID: 22969
		public List<CharaPackData> charaList;

		// Token: 0x040059BA RID: 22970
		public List<HelperPackData> helperList;

		// Token: 0x040059BB RID: 22971
		public List<PhotoPackData> disableFilterPhotoList;

		// Token: 0x040059BC RID: 22972
		public List<CharaPackData> disableFilterCharaList;

		// Token: 0x040059BD RID: 22973
		public List<HelperPackData> disableFilterHelperList;

		// Token: 0x040059BE RID: 22974
		public List<PhotoPackData> upperDisableSortPhotoList;

		// Token: 0x040059BF RID: 22975
		public List<PhotoPackData> lowerDisableSortPhotoList;

		// Token: 0x040059C0 RID: 22976
		public List<CharaPackData> disableSortCharaList;

		// Token: 0x040059C1 RID: 22977
		public List<HelperPackData> disableSortHelperList;

		// Token: 0x040059C2 RID: 22978
		public Dictionary<SortFilterDefine.PhotoAlbumRegistrationStatus, HashSet<int>> photoAlbumRegistrationStatusMap;

		// Token: 0x040059C3 RID: 22979
		public PhotoPackData basePhotoPackData;

		// Token: 0x040059C4 RID: 22980
		public SortFilterDefine.SortType sortType;

		// Token: 0x040059C5 RID: 22981
		public bool includePhotoBonus;

		// Token: 0x040059C6 RID: 22982
		public string includeFriendsSearchText = "";

		// Token: 0x040059C7 RID: 22983
		public string includePhotoSearchText = "";
	}

	// Token: 0x02000FFD RID: 4093
	public class RegisterData
	{
		// Token: 0x040059C8 RID: 22984
		public SortFilterDefine.RegisterType register;

		// Token: 0x040059C9 RID: 22985
		public PguiButtonCtrl filterButton;

		// Token: 0x040059CA RID: 22986
		public PguiButtonCtrl sortButton;

		// Token: 0x040059CB RID: 22987
		public PguiButtonCtrl sortUdButton;

		// Token: 0x040059CC RID: 22988
		public SortWindowCtrl.FuncGetTarget funcGetTargetBaseList;

		// Token: 0x040059CD RID: 22989
		public SortWindowCtrl.FuncDisideTarget funcDisideTarget;
	}

	// Token: 0x02000FFE RID: 4094
	// (Invoke) Token: 0x0600519D RID: 20893
	public delegate SortWindowCtrl.SortTarget FuncGetTarget();

	// Token: 0x02000FFF RID: 4095
	// (Invoke) Token: 0x060051A1 RID: 20897
	public delegate void FuncDisideTarget(SortWindowCtrl.SortTarget result);
}
