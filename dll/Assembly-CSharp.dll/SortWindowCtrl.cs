using System;
using System.Collections.Generic;
using UnityEngine;

public class SortWindowCtrl : MonoBehaviour
{
	public CharaDef.AttributeType AttributeType { get; private set; }

	private Dictionary<SortFilterDefine.RegisterType, DataManagerGameStatus.UserFlagData.SortTypeData> SortTypeDataMap { get; set; }

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

	public void RegistSortCharaAttribute(CharaDef.AttributeType attr)
	{
		this.AttributeType = attr;
	}

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

	public SortWindowCtrl.RegisterData GetRegisterData(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerMap[regType];
		}
		return null;
	}

	public List<CharaDef.AttributeType> GetAttributeType(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].includeCharaAttribute;
		}
		return null;
	}

	public List<int> GetBonusCharaIdList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].GetBonusCharaIdList();
		}
		return null;
	}

	public bool[] GetHanamaru(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].isFilterHanamaru;
		}
		return null;
	}

	public bool[] GetFavorite(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].isFilterFavoriteList;
		}
		return null;
	}

	public List<DataManagerChara.FilterData> GetMiracleTargetList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].miracleTargetList;
		}
		return null;
	}

	public List<DataManagerChara.FilterData> GetMiracleEffectList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].miracleEffectList;
		}
		return null;
	}

	public SortFilterDefine.AndOrState GetMiracleAndOrStatus(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].miracleEffectAndOrStatus;
		}
		return SortFilterDefine.AndOrState.Invalid;
	}

	public List<DataManagerChara.FilterData> GetCharacteristicTargetList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicTargetList;
		}
		return null;
	}

	public List<DataManagerChara.FilterData> GetCharacteristicConditionList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicConditionList;
		}
		return null;
	}

	public List<DataManagerChara.FilterData> GetCharacteristicEffectList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicEffectList;
		}
		return null;
	}

	public List<DataManagerChara.FilterData> GetCharacteristicResistList(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicResistList;
		}
		return null;
	}

	public SortFilterDefine.AndOrState GetCharacteristicEffectAndOrStatus(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicEffectAndOrStatus;
		}
		return SortFilterDefine.AndOrState.Invalid;
	}

	public SortFilterDefine.AndOrState GetCharacteristicResistAndOrStatus(SortFilterDefine.RegisterType regType)
	{
		if (this.registerMap.ContainsKey(regType))
		{
			return this.registerSaveMap[regType].characteristicResistAndOrStatus;
		}
		return SortFilterDefine.AndOrState.Invalid;
	}

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
		case SortFilterDefine.RegisterType.STICKER_COLLECTION:
			sortFilterType = SortFilterDefine.SortFilterType.STICKER_SORT;
			break;
		}
		return sortFilterType;
	}

	public void RequestActionUpdateSortType()
	{
		if (this.SortTypeDataMap == null)
		{
			return;
		}
		DataManager.DmGameStatus.RequestActionUpdateSortType(new List<DataManagerGameStatus.UserFlagData.SortTypeData>(this.SortTypeDataMap.Values));
		this.SortTypeDataMap.Clear();
	}

	private void DecorationRegisterButton(SortWindowCtrl.RegisterData regData)
	{
		SortFilterDefine.RegisterType register = regData.register;
		if (regData.filterButton != null)
		{
			Transform transform = regData.filterButton.transform.Find("BaseImage/On");
			Transform transform2 = regData.filterButton.transform.Find("BaseImage/Off");
			if (transform != null && transform2 != null)
			{
				bool flag = regData.funcGetTargetBaseList().photoList != null;
				bool flag2 = regData.funcGetTargetBaseList().stickerList != null;
				bool flag3;
				if (flag)
				{
					flag3 = this.registerSaveMap[register].includePhotoBonus || this.registerSaveMap[register].includePhotoLimit || this.registerSaveMap[register].isFilterFavoritePhotoList[0] || this.registerSaveMap[register].isFilterFavoritePhotoList[1] || 0 < this.registerSaveMap[register].includePhotoRarityList.Count || 0 < this.registerSaveMap[register].includePhotoTypeList.Count || 0 < this.registerSaveMap[register].includeRegistrationStatusList.Count || 0 < this.registerSaveMap[register].BuffConditionsList.Count || 0 < this.registerSaveMap[register].BuffTargetList.Count || 0 < this.registerSaveMap[register].BuffEffectList.Count || 0 < this.registerSaveMap[register].BuffAbnormalEnablelList.Count || 0 < this.registerSaveMap[register].includePhotoSearchText.Length;
				}
				else if (flag2)
				{
					flag3 = 0 < this.registerSaveMap[register].includeStickerRarityList.Count || 0 < this.registerSaveMap[register].includeStickerTypeList.Count || 0 < this.registerSaveMap[register].includeStickerSearchText.Length;
				}
				else
				{
					flag3 = this.registerSaveMap[register].includeCharaAttribute.Count > 0 || this.registerSaveMap[register].includeCharaBonus.Count > 0 || this.registerSaveMap[register].isFilterHanamaru[0] || this.registerSaveMap[register].isFilterHanamaru[1] || this.registerSaveMap[register].isFilterFavoriteList[0] || this.registerSaveMap[register].isFilterFavoriteList[1] || this.registerSaveMap[register].miracleTargetList.Count > 0 || this.registerSaveMap[register].miracleEffectList.Count > 0 || this.registerSaveMap[register].characteristicConditionList.Count > 0 || this.registerSaveMap[register].characteristicTargetList.Count > 0 || this.registerSaveMap[register].characteristicEffectList.Count > 0 || this.registerSaveMap[register].characteristicResistList.Count > 0 || this.registerSaveMap[register].includeFriendsSearchText.Length > 0;
				}
				transform.gameObject.SetActive(flag3);
				transform2.gameObject.SetActive(!flag3);
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
					goto IL_0469;
				case SortFilterDefine.SortFilterType.PHOTO_SORT:
					transform3.GetComponent<PguiTextCtrl>().text = SortFilterDefine.SortTypeDispNameMap[this.registerSaveMap[register].sortType];
					goto IL_0469;
				}
				transform3.GetComponent<PguiTextCtrl>().text = SortFilterDefine.SortTypeDispNameMap[this.registerSaveMap[register].sortType];
			}
		}
		IL_0469:
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
				CanvasManager.HdlPhotoFilterWindowCtrl.SetupAlbum(registerSaveData.includeRegistrationStatusList);
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
		case SortFilterDefine.SortFilterType.STICKER_SORT:
		{
			List<SortFilterDefine.SortType> stickerSortTypeList = SortFilterDefine.StickerSortTypeList;
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl6 in this.guiData.BtnList)
			{
				pguiToggleButtonCtrl6.gameObject.SetActive(false);
			}
			for (int n = 0; n < stickerSortTypeList.Count; n++)
			{
				this.guiData.BtnList[n].gameObject.SetActive(true);
				this.guiData.Num_TxtList[n].text = SortFilterDefine.SortTypeDispNameMap[stickerSortTypeList[n]];
			}
			this.guiData.baseWindow.Setup(PrjUtil.MakeMessage("ソート順の設定"), PrjUtil.MakeMessage("並び順の基準とする条件を設定できます"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), null, false);
			for (int num3 = 0; num3 < stickerSortTypeList.Count; num3++)
			{
				this.guiData.BtnList[num3].SetToggleIndex((stickerSortTypeList[num3] == registerSaveData.sortType) ? 1 : 0);
			}
			this.guiData.baseWindow.Open();
			return;
		}
		case SortFilterDefine.SortFilterType.STICKER_FILTER:
			CanvasManager.HdlStickerFilterWindowCtrl.SetupRarity(registerSaveData.includeStickerRarityList);
			CanvasManager.HdlStickerFilterWindowCtrl.SetupType(registerSaveData.includeStickerTypeList);
			CanvasManager.HdlStickerFilterWindowCtrl.SetupSearchText(registerSaveData.includeStickerSearchText);
			CanvasManager.HdlStickerFilterWindowCtrl.Open();
			return;
		default:
			return;
		}
	}

	private void cb(bool b, SortFilterDefine.SortType t)
	{
	}

	private void ResetSearchText()
	{
		if (this.currentFilterType == SortFilterDefine.SortFilterType.CHARA_FILTER)
		{
			CanvasManager.HdlFriendsFilterWindowCtrl.ResetSearchText();
		}
	}

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
				if (regType == SortFilterDefine.RegisterType.STICKER_COLLECTION)
				{
					this.currentFilterType = SortFilterDefine.SortFilterType.STICKER_FILTER;
				}
				this.Open();
				return;
			}
			if (registerData.sortButton == button)
			{
				if (regType == SortFilterDefine.RegisterType.STICKER_COLLECTION)
				{
					this.currentFilterType = ((this.currentSortTarget.stickerList != null) ? SortFilterDefine.SortFilterType.STICKER_SORT : SortFilterDefine.SortFilterType.CHARA_SORT);
				}
				else if (this.currentSortTarget.photoList == null)
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
			case SortFilterDefine.SortFilterType.STICKER_SORT:
				registerSaveData.sortType = SortFilterDefine.StickerSortTypeList[this.guiData.BtnList.FindIndex((PguiToggleButtonCtrl item) => item.GetToggleIndex() == 1)];
				this.RegistStickerSort();
				break;
			}
			break;
		}
		}
		return true;
	}

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
			registerSaveData.includeRegistrationStatusList = CanvasManager.HdlPhotoFilterWindowCtrl.GetPhotoAlbumButtonstatus();
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

	public void RegistStickerSort()
	{
		SortWindowCtrl.RegisterData registerData = this.registerMap[this.currentRegisterType];
		RegisterSaveData registerSaveData = this.registerSaveMap[this.currentRegisterType];
		registerSaveData.SolutionList(ref this.currentSortTarget, null);
		this.currentSortTarget.sortType = registerSaveData.sortType;
		registerData.funcDisideTarget(this.currentSortTarget);
		this.DecorationRegisterButton(registerData);
		this.SortTypeDataMap[this.currentRegisterType] = new DataManagerGameStatus.UserFlagData.SortTypeData(this.currentRegisterType, this.currentSortTarget.sortType, registerSaveData.SortOrder);
	}

	public void RegistStickerFilter()
	{
		SortWindowCtrl.RegisterData registerData = this.registerMap[this.currentRegisterType];
		RegisterSaveData registerSaveData = this.registerSaveMap[this.currentRegisterType];
		registerSaveData.includeStickerRarityList = CanvasManager.HdlStickerFilterWindowCtrl.GetRarityButtonstatus();
		registerSaveData.includeStickerTypeList = CanvasManager.HdlStickerFilterWindowCtrl.GetTypeButtonstatus();
		registerSaveData.includeStickerSearchText = CanvasManager.HdlStickerFilterWindowCtrl.GetSearchText();
		registerSaveData.SolutionList(ref this.currentSortTarget, null);
		this.currentSortTarget.sortType = registerSaveData.sortType;
		registerData.funcDisideTarget(this.currentSortTarget);
		this.DecorationRegisterButton(registerData);
		this.SortTypeDataMap[this.currentRegisterType] = new DataManagerGameStatus.UserFlagData.SortTypeData(this.currentRegisterType, this.currentSortTarget.sortType, registerSaveData.SortOrder);
	}

	public void UpdateBannedCharaList(List<CharaPackData> list)
	{
		this.bannedCharaList = list;
	}

	private SortWindowCtrl.GUI guiData;

	private SortWindowCtrl.GUICharaFilter guiFriendsFilterData;

	private SortFilterDefine.SortFilterType currentFilterType;

	private Dictionary<SortFilterDefine.RegisterType, SortWindowCtrl.RegisterData> registerMap;

	private Dictionary<SortFilterDefine.RegisterType, RegisterSaveData> registerSaveMap;

	private SortFilterDefine.RegisterType currentRegisterType;

	private SortWindowCtrl.SortTarget currentSortTarget;

	private List<CharaPackData> bannedCharaList;

	public class GUI
	{
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

		public GameObject baseObj;

		public List<PguiTextCtrl> Num_TxtList;

		public List<PguiToggleButtonCtrl> BtnList;

		public List<PguiToggleButtonCtrl> Btn_EventList;

		public PguiOpenWindowCtrl baseWindow;
	}

	public class GUICharaFilter
	{
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

		public GameObject baseObj;

		public List<PguiToggleButtonCtrl> Btn;

		public PguiToggleButtonCtrl Btn_Hanamaru;

		public PguiToggleButtonCtrl Btn_Favorite;

		public List<PguiToggleButtonCtrl> Btn_EventList;

		public PguiOpenWindowCtrl baseWindow;
	}

	public class SortTarget
	{
		public SortTarget()
		{
		}

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
			if (st.stickerList != null)
			{
				this.stickerList = new List<DataManagerSticker.StickerPackData>(st.stickerList);
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
			if (st.registrationStatusMap != null)
			{
				this.registrationStatusMap = new Dictionary<SortFilterDefine.RegistrationStatus, HashSet<int>>(st.registrationStatusMap);
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

		public List<PhotoPackData> photoList;

		public List<CharaPackData> charaList;

		public List<HelperPackData> helperList;

		public List<DataManagerSticker.StickerPackData> stickerList;

		public List<PhotoPackData> disableFilterPhotoList;

		public List<CharaPackData> disableFilterCharaList;

		public List<HelperPackData> disableFilterHelperList;

		public List<PhotoPackData> upperDisableSortPhotoList;

		public List<PhotoPackData> lowerDisableSortPhotoList;

		public List<CharaPackData> disableSortCharaList;

		public List<HelperPackData> disableSortHelperList;

		public Dictionary<SortFilterDefine.RegistrationStatus, HashSet<int>> registrationStatusMap;

		public PhotoPackData basePhotoPackData;

		public SortFilterDefine.SortType sortType;

		public bool includePhotoBonus;

		public string includeFriendsSearchText = "";

		public string includePhotoSearchText = "";
	}

	public class RegisterData
	{
		public SortFilterDefine.RegisterType register;

		public PguiButtonCtrl filterButton;

		public PguiButtonCtrl sortButton;

		public PguiButtonCtrl sortUdButton;

		public SortWindowCtrl.FuncGetTarget funcGetTargetBaseList;

		public SortWindowCtrl.FuncDisideTarget funcDisideTarget;
	}

	public delegate SortWindowCtrl.SortTarget FuncGetTarget();

	public delegate void FuncDisideTarget(SortWindowCtrl.SortTarget result);
}
