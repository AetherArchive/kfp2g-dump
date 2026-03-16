using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

public class PhotoFilterWindowCtrl
{
	private string SearchText { get; set; }

	public void Initialize(GameObject go)
	{
		this.winGUI = new PhotoFilterWindowCtrl.WindowGUI(go);
		this.winGUI.resetButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickReset), PguiButtonCtrl.SoundType.DEFAULT);
		this.winGUI.baseWindow.RegistCallback(new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton));
		this.winGUI.frameRarity = this.CreateButtonFrame("☆の数", SortFilterDefine.photoFilterStarNameList);
		this.winGUI.framePhotoType = this.CreateButtonFrame("フォトタイプ", SortFilterDefine.photoFilterPhototypeNameList);
		this.winGUI.frameFavorite = this.CreateButtonFrame("お気に入り", SortFilterDefine.photoFilterFavoriteList);
		this.winGUI.frameSpecial = this.CreateButtonFrame("特殊効果", SortFilterDefine.photoFilterSpecialNameList);
		this.winGUI.frameSpecialLimit = this.CreateButtonFrame("特殊効果", SortFilterDefine.photoFilterLimitSpecialNameList);
		this.winGUI.frameAlbum = this.CreateButtonFrame("登録状態", SortFilterDefine.photoFilterAlbumNameList);
		this.winGUI.frameLevel = this.CreateButtonFrame("レベル", SortFilterDefine.photoFilterLevelNameList);
		this.windowTextSearchChange = new PhotoFilterWindowCtrl.WindowTextSearchChange(go.transform);
		this.SearchText = "";
		this.windowTextSearchChange.InputField.onEndEdit.AddListener(delegate(string str)
		{
			this.windowTextSearchChange.InputField.text = PrjUtil.ModifiedComment(str);
			this.SearchText = this.windowTextSearchChange.InputField.text;
		});
	}

	public PhotoFilterWindowCtrl.FrameGUI CreateButtonFrame(string frameName, List<string> buttonNameList)
	{
		List<PguiToggleButtonCtrl> list = new List<PguiToggleButtonCtrl>();
		GameObject gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterPhoto_Parts01", Singleton<CanvasManager>.Instance.SystemPanel.transform);
		gameObject.transform.SetParent(this.winGUI.scrollContentbasePanel.transform, false);
		gameObject.transform.Find("Base/Title").GetComponent<PguiTextCtrl>().text = frameName;
		gameObject.transform.Find("Base/Title").GetComponent<PguiTextCtrl>();
		gameObject.transform.Find("Base/Title/RadioBtns").gameObject.SetActive(false);
		int num = buttonNameList.Count / 4 + ((buttonNameList.Count % 4 == 0) ? 0 : 1);
		for (int i = 0; i < num; i++)
		{
			GameObject assetObj = this.GetAssetObj(frameName);
			assetObj.transform.SetParent(gameObject.transform.Find("Base"), false);
			List<PguiToggleButtonCtrl> filterButtonList = this.GetFilterButtonList(assetObj, frameName);
			if (frameName == "お気に入り")
			{
				list.AddRange(filterButtonList);
				break;
			}
			for (int j = 0; j < filterButtonList.Count; j++)
			{
				int num2 = i * 4 + j;
				if (buttonNameList.Count <= num2)
				{
					filterButtonList[j].gameObject.SetActive(false);
				}
				else if (buttonNameList[num2] == SortFilterDefine.BTN_DISABLE_STR_NAME)
				{
					filterButtonList[j].gameObject.SetActive(false);
				}
				else
				{
					if (buttonNameList[num2] != string.Empty)
					{
						Transform transform = filterButtonList[j].transform.Find("Img");
						if (transform != null)
						{
							transform.gameObject.SetActive(false);
						}
					}
					list.Add(filterButtonList[j]);
					filterButtonList[j].transform.Find("Num_Txt").GetComponent<PguiTextCtrl>().text = buttonNameList[num2];
				}
			}
		}
		return new PhotoFilterWindowCtrl.FrameGUI(gameObject, list);
	}

	private void ExceptionCategoryName(List<DataManagerPhoto.PhotoCharacteristicData> sourceList)
	{
		sourceList.Find((DataManagerPhoto.PhotoCharacteristicData item) => item.CategoryName != sourceList[0].CategoryName);
	}

	public PhotoFilterWindowCtrl.BuffFrameGUI CreateBuffButtonFrame(string frameName, List<string> enebleButtonNameList)
	{
		PhotoFilterWindowCtrl.<>c__DisplayClass15_0 CS$<>8__locals1 = new PhotoFilterWindowCtrl.<>c__DisplayClass15_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.frameName = frameName;
		CS$<>8__locals1.enebleButtonNameList = enebleButtonNameList;
		List<DataManagerPhoto.PhotoCharacteristicData> photoCharacteristicDataList = DataManager.DmPhoto.GetPhotoCharacteristicDataList();
		if (photoCharacteristicDataList == null)
		{
			return null;
		}
		GameObject gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterPhoto_Parts01", Singleton<CanvasManager>.Instance.SystemPanel.transform);
		gameObject.transform.SetParent(this.winGUI.scrollContentbasePanel.transform, false);
		gameObject.transform.Find("Base/Title").GetComponent<PguiTextCtrl>().text = CS$<>8__locals1.frameName;
		List<DataManagerPhoto.PhotoCharacteristicData> list = photoCharacteristicDataList.FindAll((DataManagerPhoto.PhotoCharacteristicData x) => x.Category == SortFilterDefine.CharacteristicFilterCategory.Conditions);
		List<DataManagerPhoto.PhotoCharacteristicData> list2 = photoCharacteristicDataList.FindAll((DataManagerPhoto.PhotoCharacteristicData x) => x.Category == SortFilterDefine.CharacteristicFilterCategory.Target);
		List<DataManagerPhoto.PhotoCharacteristicData> list3 = photoCharacteristicDataList.FindAll((DataManagerPhoto.PhotoCharacteristicData x) => x.Category == SortFilterDefine.CharacteristicFilterCategory.Effect);
		List<DataManagerPhoto.PhotoCharacteristicData> list4 = photoCharacteristicDataList.FindAll((DataManagerPhoto.PhotoCharacteristicData x) => x.Category == SortFilterDefine.CharacteristicFilterCategory.Abnormal);
		this.buffCategoryEnableListMap = new Dictionary<SortFilterDefine.CharacteristicFilterCategory, List<DataManagerPhoto.PhotoCharacteristicData>>();
		List<PguiToggleButtonCtrl> list5 = CS$<>8__locals1.<CreateBuffButtonFrame>g__CreatebuttonList|0(SortFilterDefine.CharacteristicFilterCategory.Conditions, gameObject, list);
		List<PguiToggleButtonCtrl> list6 = CS$<>8__locals1.<CreateBuffButtonFrame>g__CreatebuttonList|0(SortFilterDefine.CharacteristicFilterCategory.Target, gameObject, list2);
		List<PguiToggleButtonCtrl> list7 = CS$<>8__locals1.<CreateBuffButtonFrame>g__CreatebuttonList|0(SortFilterDefine.CharacteristicFilterCategory.Effect, gameObject, list3);
		List<PguiToggleButtonCtrl> list8 = CS$<>8__locals1.<CreateBuffButtonFrame>g__CreatebuttonList|0(SortFilterDefine.CharacteristicFilterCategory.Abnormal, gameObject, list4);
		new List<PguiToggleButtonCtrl>();
		return new PhotoFilterWindowCtrl.BuffFrameGUI(gameObject, list5, list6, list7, list8);
	}

	public GameObject GetAssetObj(string frameName)
	{
		GameObject gameObject;
		if (frameName == "お気に入り")
		{
			gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterFriends_PartsFavorite", Singleton<CanvasManager>.Instance.SystemPanel.transform);
		}
		else
		{
			gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterPhoto_Parts02", Singleton<CanvasManager>.Instance.SystemPanel.transform);
		}
		return gameObject;
	}

	public List<PguiToggleButtonCtrl> GetFilterButtonList(GameObject buttonObj, string frameName = "")
	{
		List<PguiToggleButtonCtrl> list = new List<PguiToggleButtonCtrl>();
		if (frameName == "お気に入り")
		{
			list.Add(buttonObj.transform.Find("Btn01").GetComponent<PguiToggleButtonCtrl>());
			list.Add(buttonObj.transform.Find("Btn02").GetComponent<PguiToggleButtonCtrl>());
		}
		else
		{
			list.Add(buttonObj.transform.Find("Btn01").GetComponent<PguiToggleButtonCtrl>());
			list.Add(buttonObj.transform.Find("Btn02").GetComponent<PguiToggleButtonCtrl>());
			list.Add(buttonObj.transform.Find("Btn03").GetComponent<PguiToggleButtonCtrl>());
			list.Add(buttonObj.transform.Find("Btn04").GetComponent<PguiToggleButtonCtrl>());
		}
		return list;
	}

	public void SetupWindow(SortFilterDefine.PhotoFilterType filterType, List<string> enebleButtonNameList, SortFilterDefine.AndOrState andOrState, SortFilterDefine.AndOrState andOrStateAbnormal)
	{
		this.winGUI.filterType = filterType;
		this.winGUI.frameSpecial.baseObj.SetActive(false);
		this.winGUI.frameSpecialLimit.baseObj.SetActive(false);
		this.winGUI.frameAlbum.baseObj.SetActive(false);
		this.winGUI.frameLevel.baseObj.SetActive(false);
		bool flag = DataManager.DmPhoto.IsBonusActive();
		bool flag2 = false;
		switch (this.winGUI.filterType)
		{
		case SortFilterDefine.PhotoFilterType.SortFilter:
			if (flag)
			{
				this.winGUI.frameSpecial.baseObj.SetActive(true);
			}
			flag2 = true;
			break;
		case SortFilterDefine.PhotoFilterType.SellPhoto:
			if (flag)
			{
				this.winGUI.frameSpecial.baseObj.SetActive(true);
			}
			this.winGUI.frameLevel.baseObj.SetActive(true);
			flag2 = true;
			break;
		case SortFilterDefine.PhotoFilterType.PhotoAlbum:
			this.winGUI.frameAlbum.baseObj.SetActive(true);
			flag2 = true;
			break;
		case SortFilterDefine.PhotoFilterType.PhotoGrow:
			if (flag)
			{
				this.winGUI.frameSpecial.baseObj.SetActive(true);
			}
			else
			{
				this.winGUI.frameSpecialLimit.baseObj.SetActive(true);
			}
			flag2 = true;
			break;
		}
		if (this.winGUI.frameBuff != null)
		{
			this.winGUI.frameBuff.ObjectDestroy();
			this.winGUI.frameBuff = null;
		}
		if (flag2)
		{
			this.winGUI.frameBuff = this.CreateBuffButtonFrame("とくせい", enebleButtonNameList);
			this.SetAndOrButtonBuff(andOrState);
			this.SetAndOrButtonBuffAbnormal(andOrStateAbnormal);
			this.winGUI.frameBuff.baseObj.SetActive(true);
		}
	}

	public void SetupRarity(List<ItemDef.Rarity> rarityList)
	{
		for (int i = 0; i < this.winGUI.frameRarity.buttonList.Count; i++)
		{
			this.winGUI.frameRarity.buttonList[i].SetToggleIndex(rarityList.Contains(i + ItemDef.Rarity.STAR1) ? 1 : 0);
			int btnIdx = i;
			this.winGUI.frameRarity.buttonList[i].AllRemoveOnClickListener();
			this.winGUI.frameRarity.buttonList[i].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickRarityButton(btnIdx, toggleIdx));
		}
	}

	public void SetUpPhotoType(List<PhotoDef.Type> photoTypeList)
	{
		for (int i = 0; i < this.winGUI.framePhotoType.buttonList.Count; i++)
		{
			this.winGUI.framePhotoType.buttonList[i].SetToggleIndex(photoTypeList.Contains(i + PhotoDef.Type.PARAMETER) ? 1 : 0);
			int btnIdx = i;
			this.winGUI.framePhotoType.buttonList[i].AllRemoveOnClickListener();
			this.winGUI.framePhotoType.buttonList[i].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickPhotoTypeButton(btnIdx, toggleIdx));
		}
	}

	public void SetupFavorite(bool[] registButtonList)
	{
		for (int i = 0; i < this.winGUI.frameFavorite.buttonList.Count; i++)
		{
			this.winGUI.frameFavorite.buttonList[i].SetToggleIndex(registButtonList[i] ? 1 : 0);
			int btnIdx = i;
			this.winGUI.frameFavorite.buttonList[i].AllRemoveOnClickListener();
			this.winGUI.frameFavorite.buttonList[i].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickFavoriteButton(btnIdx, toggleIdx));
		}
	}

	public void SetupSpecial(bool invalidBonus, bool inValidLimit)
	{
		bool flag = DataManager.DmPhoto.IsBonusActive();
		bool flag2 = this.winGUI.filterType == SortFilterDefine.PhotoFilterType.PhotoGrow;
		if (flag)
		{
			this.winGUI.frameSpecial.buttonList[0].SetToggleIndex(invalidBonus ? 1 : 0);
			this.winGUI.frameSpecial.buttonList[0].AllRemoveOnClickListener();
			this.winGUI.frameSpecial.buttonList[0].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickSpecialButton(0, toggleIdx));
			if (flag2)
			{
				this.winGUI.frameSpecial.buttonList[1].SetToggleIndex(inValidLimit ? 1 : 0);
				this.winGUI.frameSpecial.buttonList[1].AllRemoveOnClickListener();
				this.winGUI.frameSpecial.buttonList[1].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickLimitButton(1, toggleIdx));
			}
			this.winGUI.frameSpecial.buttonList[1].gameObject.SetActive(flag2);
			return;
		}
		if (flag2)
		{
			this.winGUI.frameSpecialLimit.buttonList[0].SetToggleIndex(inValidLimit ? 1 : 0);
			this.winGUI.frameSpecialLimit.buttonList[0].AllRemoveOnClickListener();
			this.winGUI.frameSpecialLimit.buttonList[0].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickLimitButton(0, toggleIdx));
			this.winGUI.frameSpecialLimit.buttonList[0].gameObject.SetActive(flag2);
		}
	}

	public void SetupAlbum(List<SortFilterDefine.PhotoAlbumRegistrationStatus> registTypeList)
	{
		for (int i = 0; i < this.winGUI.frameAlbum.buttonList.Count; i++)
		{
			this.winGUI.frameAlbum.buttonList[i].SetToggleIndex(registTypeList.Contains(i + SortFilterDefine.PhotoAlbumRegistrationStatus.Unregistered) ? 1 : 0);
			int btnIdx = i;
			this.winGUI.frameAlbum.buttonList[i].AllRemoveOnClickListener();
			this.winGUI.frameAlbum.buttonList[i].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickPhotoAlbumButton(btnIdx, toggleIdx));
		}
	}

	public void SetAndOrButtonBuff(SortFilterDefine.AndOrState andOrState)
	{
		if (andOrState == SortFilterDefine.AndOrState.And)
		{
			this.winGUI.frameEffectOrBtn.SetToggleIndex(0);
			this.winGUI.frameEffectAndBtn.SetToggleIndex(1);
			return;
		}
		if (andOrState != SortFilterDefine.AndOrState.Or)
		{
			return;
		}
		this.winGUI.frameEffectOrBtn.SetToggleIndex(1);
		this.winGUI.frameEffectAndBtn.SetToggleIndex(0);
	}

	public void SetAndOrButtonBuffAbnormal(SortFilterDefine.AndOrState andOrState)
	{
		if (andOrState == SortFilterDefine.AndOrState.And)
		{
			this.winGUI.frameEffectAbnormalOrBtn.SetToggleIndex(0);
			this.winGUI.frameEffectAbnormalAndBtn.SetToggleIndex(1);
			return;
		}
		if (andOrState != SortFilterDefine.AndOrState.Or)
		{
			return;
		}
		this.winGUI.frameEffectAbnormalOrBtn.SetToggleIndex(1);
		this.winGUI.frameEffectAbnormalAndBtn.SetToggleIndex(0);
	}

	public void SetAndOrButtonAbnormal(SortFilterDefine.AndOrState andOrState)
	{
		if (andOrState == SortFilterDefine.AndOrState.And)
		{
			this.winGUI.frameEffectAbnormalOrBtn.SetToggleIndex(0);
			this.winGUI.frameEffectAbnormalAndBtn.SetToggleIndex(1);
			return;
		}
		if (andOrState != SortFilterDefine.AndOrState.Or)
		{
			return;
		}
		this.winGUI.frameEffectAbnormalOrBtn.SetToggleIndex(1);
		this.winGUI.frameEffectAbnormalAndBtn.SetToggleIndex(0);
	}

	public bool OnClickRarityButton(int btnIdx, int toggleIdx)
	{
		this.winGUI.frameRarity.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	public bool OnClickPhotoTypeButton(int btnIdx, int toggleIdx)
	{
		this.winGUI.framePhotoType.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	public bool OnClickFavoriteButton(int btnIdx, int toggleIdx)
	{
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.winGUI.frameFavorite.buttonList)
		{
			pguiToggleButtonCtrl.SetToggleIndex(0);
		}
		this.winGUI.frameFavorite.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	public bool OnClickSpecialButton(int btnIdx, int toggleIdx)
	{
		if (DataManager.DmPhoto.IsBonusActive())
		{
			this.winGUI.frameSpecial.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		}
		else
		{
			this.winGUI.frameSpecialLimit.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		}
		return true;
	}

	public bool OnClickLimitButton(int btnIdx, int toggleIdx)
	{
		if (DataManager.DmPhoto.IsBonusActive())
		{
			this.winGUI.frameSpecial.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		}
		else
		{
			this.winGUI.frameSpecialLimit.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		}
		return true;
	}

	public bool OnClickPhotoAlbumButton(int btnIdx, int toggleIdx)
	{
		this.winGUI.frameAlbum.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	private void RegistBuff(SortFilterDefine.CharacteristicFilterCategory category, DataManagerPhoto.PhotoCharacteristicData source, int toggleIdx)
	{
		if (toggleIdx == 0)
		{
			this.buffCategoryEnableListMap[category].Add(source);
			return;
		}
		this.buffCategoryEnableListMap[category].Remove(source);
	}

	public bool OnClickBuffOrAnd(bool isOr, int toggleIdx)
	{
		if (1 == toggleIdx)
		{
			return false;
		}
		if (isOr)
		{
			this.winGUI.frameEffectAndBtn.SetToggleIndex(0);
		}
		else
		{
			this.winGUI.frameEffectOrBtn.SetToggleIndex(0);
		}
		return true;
	}

	public bool OnClickAbnormalOrAnd(bool isOr, int toggleIdx)
	{
		if (1 == toggleIdx)
		{
			return false;
		}
		if (isOr)
		{
			this.winGUI.frameEffectAbnormalAndBtn.SetToggleIndex(0);
		}
		else
		{
			this.winGUI.frameEffectAbnormalOrBtn.SetToggleIndex(0);
		}
		return true;
	}

	public bool OnClickBuffCondition(DataManagerPhoto.PhotoCharacteristicData source, int btnIdx, int toggleIdx)
	{
		this.RegistBuff(SortFilterDefine.CharacteristicFilterCategory.Conditions, source, toggleIdx);
		return true;
	}

	public bool OnClickBuffTarget(DataManagerPhoto.PhotoCharacteristicData source, int btnIdx, int toggleIdx)
	{
		this.RegistBuff(SortFilterDefine.CharacteristicFilterCategory.Target, source, toggleIdx);
		return true;
	}

	public bool OnClickBuffEffect(DataManagerPhoto.PhotoCharacteristicData source, int btnIdx, int toggleIdx)
	{
		this.RegistBuff(SortFilterDefine.CharacteristicFilterCategory.Effect, source, toggleIdx);
		return true;
	}

	public bool OnClickAbnormal(DataManagerPhoto.PhotoCharacteristicData source, int btnIdx, int toggleIdx)
	{
		this.RegistBuff(SortFilterDefine.CharacteristicFilterCategory.Abnormal, source, toggleIdx);
		return true;
	}

	public void Open()
	{
		this.winGUI.baseWindow.Setup(PrjUtil.MakeMessage("フィルターの設定"), PrjUtil.MakeMessage("選択したカテゴリで絞り込みができます"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), null, false);
		this.winGUI.baseWindow.Open();
	}

	public List<ItemDef.Rarity> GetRarityButtonstatus()
	{
		List<PguiToggleButtonCtrl> buttonList = this.winGUI.frameRarity.buttonList;
		List<ItemDef.Rarity> list = new List<ItemDef.Rarity>();
		for (int i = 0; i < buttonList.Count; i++)
		{
			if (buttonList[i].GetToggleIndex() == 1)
			{
				list.Add(i + ItemDef.Rarity.STAR1);
			}
		}
		return list;
	}

	public List<PhotoDef.Type> GetPhotoTypeButtonstatus()
	{
		List<PguiToggleButtonCtrl> buttonList = this.winGUI.framePhotoType.buttonList;
		List<PhotoDef.Type> list = new List<PhotoDef.Type>();
		for (int i = 0; i < buttonList.Count; i++)
		{
			if (buttonList[i].GetToggleIndex() == 1)
			{
				list.Add(i + PhotoDef.Type.PARAMETER);
			}
		}
		return list;
	}

	public bool[] GetFavoriteButtonStatus()
	{
		List<PguiToggleButtonCtrl> buttonList = this.winGUI.frameFavorite.buttonList;
		bool[] array = new bool[2];
		for (int i = 0; i < buttonList.Count; i++)
		{
			if (buttonList[i].GetToggleIndex() == 1)
			{
				array[i] = true;
			}
		}
		return array;
	}

	public List<SortFilterDefine.PhotoAlbumRegistrationStatus> GetPhotoAlbumButtonstatus()
	{
		List<PguiToggleButtonCtrl> buttonList = this.winGUI.frameAlbum.buttonList;
		List<SortFilterDefine.PhotoAlbumRegistrationStatus> list = new List<SortFilterDefine.PhotoAlbumRegistrationStatus>();
		for (int i = 0; i < buttonList.Count; i++)
		{
			if (buttonList[i].GetToggleIndex() == 1)
			{
				list.Add(i + SortFilterDefine.PhotoAlbumRegistrationStatus.Unregistered);
			}
		}
		return list;
	}

	public bool GetInvalidBonus()
	{
		return this.winGUI.frameSpecial.buttonList[0].GetToggleIndex() == 1;
	}

	public bool GetInvalidLimit()
	{
		if (!DataManager.DmPhoto.IsBonusActive())
		{
			return this.winGUI.frameSpecialLimit.buttonList[0].GetToggleIndex() == 1;
		}
		return this.winGUI.frameSpecial.buttonList[1].GetToggleIndex() == 1;
	}

	public SortFilterDefine.AndOrState GetAndORStatus()
	{
		if (this.winGUI.frameBuff == null)
		{
			return SortFilterDefine.AndOrState.Invalid;
		}
		int toggleIndex = this.winGUI.frameEffectAndBtn.GetToggleIndex();
		int toggleIndex2 = this.winGUI.frameEffectOrBtn.GetToggleIndex();
		SortFilterDefine.AndOrState andOrState;
		if (1 == toggleIndex && toggleIndex2 == 0)
		{
			andOrState = SortFilterDefine.AndOrState.And;
		}
		else if (toggleIndex == 0 && 1 == toggleIndex2)
		{
			andOrState = SortFilterDefine.AndOrState.Or;
		}
		else
		{
			andOrState = SortFilterDefine.AndOrState.Invalid;
		}
		return andOrState;
	}

	public SortFilterDefine.AndOrState GetAndORStatusAbnormal()
	{
		if (this.winGUI.frameBuff == null)
		{
			return SortFilterDefine.AndOrState.Invalid;
		}
		int toggleIndex = this.winGUI.frameEffectAbnormalAndBtn.GetToggleIndex();
		int toggleIndex2 = this.winGUI.frameEffectAbnormalOrBtn.GetToggleIndex();
		SortFilterDefine.AndOrState andOrState;
		if (1 == toggleIndex && toggleIndex2 == 0)
		{
			andOrState = SortFilterDefine.AndOrState.And;
		}
		else if (toggleIndex == 0 && 1 == toggleIndex2)
		{
			andOrState = SortFilterDefine.AndOrState.Or;
		}
		else
		{
			andOrState = SortFilterDefine.AndOrState.Invalid;
		}
		return andOrState;
	}

	public string GetSearchText()
	{
		return this.windowTextSearchChange.InputField.text;
	}

	public void SetupSearchText(string searchText)
	{
		this.SearchText = searchText;
		this.windowTextSearchChange.InputField.text = this.SearchText;
	}

	public List<DataManagerPhoto.PhotoCharacteristicData> GetBuffConditionsDataList()
	{
		if (this.buffCategoryEnableListMap == null)
		{
			return new List<DataManagerPhoto.PhotoCharacteristicData>();
		}
		if (!this.buffCategoryEnableListMap.ContainsKey(SortFilterDefine.CharacteristicFilterCategory.Conditions))
		{
			return new List<DataManagerPhoto.PhotoCharacteristicData>();
		}
		return this.buffCategoryEnableListMap[SortFilterDefine.CharacteristicFilterCategory.Conditions];
	}

	public List<DataManagerPhoto.PhotoCharacteristicData> GetBuffTargetDataList()
	{
		if (this.buffCategoryEnableListMap == null)
		{
			return new List<DataManagerPhoto.PhotoCharacteristicData>();
		}
		if (!this.buffCategoryEnableListMap.ContainsKey(SortFilterDefine.CharacteristicFilterCategory.Target))
		{
			return new List<DataManagerPhoto.PhotoCharacteristicData>();
		}
		return this.buffCategoryEnableListMap[SortFilterDefine.CharacteristicFilterCategory.Target];
	}

	public List<DataManagerPhoto.PhotoCharacteristicData> GetBuffEffectDataList()
	{
		if (this.buffCategoryEnableListMap == null)
		{
			return new List<DataManagerPhoto.PhotoCharacteristicData>();
		}
		if (!this.buffCategoryEnableListMap.ContainsKey(SortFilterDefine.CharacteristicFilterCategory.Effect))
		{
			return new List<DataManagerPhoto.PhotoCharacteristicData>();
		}
		return this.buffCategoryEnableListMap[SortFilterDefine.CharacteristicFilterCategory.Effect];
	}

	public List<DataManagerPhoto.PhotoCharacteristicData> GetBuffAbnormalDataList()
	{
		if (this.buffCategoryEnableListMap == null)
		{
			return new List<DataManagerPhoto.PhotoCharacteristicData>();
		}
		if (!this.buffCategoryEnableListMap.ContainsKey(SortFilterDefine.CharacteristicFilterCategory.Abnormal))
		{
			return new List<DataManagerPhoto.PhotoCharacteristicData>();
		}
		return this.buffCategoryEnableListMap[SortFilterDefine.CharacteristicFilterCategory.Abnormal];
	}

	public bool OnClickWindowButton(int index)
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
			CanvasManager.HdlOpenWindowSortFilter.RegistPhotoFilter();
			break;
		}
		return true;
	}

	private void ResetSearchText()
	{
		if (this.windowTextSearchChange.InputField.text.Length == 0)
		{
			this.windowTextSearchChange.TextTransform.gameObject.SetActive(false);
		}
		this.windowTextSearchChange.InputField.text = this.SearchText;
	}

	public void SetupSearchTextActive()
	{
		if (this.windowTextSearchChange.TextTransform != null)
		{
			this.windowTextSearchChange.TextTransform.gameObject.SetActive(true);
		}
	}

	private void OnClickReset(PguiButtonCtrl clickBtn)
	{
		PhotoFilterWindowCtrl.<OnClickReset>g__ResetButton|58_0(this.winGUI.frameRarity.buttonList);
		PhotoFilterWindowCtrl.<OnClickReset>g__ResetButton|58_0(this.winGUI.framePhotoType.buttonList);
		PhotoFilterWindowCtrl.<OnClickReset>g__ResetButton|58_0(this.winGUI.frameFavorite.buttonList);
		PhotoFilterWindowCtrl.<OnClickReset>g__ResetButton|58_0(this.winGUI.frameSpecial.buttonList);
		PhotoFilterWindowCtrl.<OnClickReset>g__ResetButton|58_0(this.winGUI.frameSpecialLimit.buttonList);
		PhotoFilterWindowCtrl.<OnClickReset>g__ResetButton|58_0(this.winGUI.frameAlbum.buttonList);
		PhotoFilterWindowCtrl.<OnClickReset>g__ResetButton|58_0(this.winGUI.frameLevel.buttonList);
		this.windowTextSearchChange.InputField.text = "";
		if (this.winGUI.frameBuff != null)
		{
			PhotoFilterWindowCtrl.<OnClickReset>g__ResetButton|58_0(this.winGUI.frameBuff.buttonList);
			this.winGUI.frameEffectOrBtn.SetToggleIndex(1);
			this.winGUI.frameEffectAndBtn.SetToggleIndex(0);
			this.winGUI.frameEffectAbnormalOrBtn.SetToggleIndex(1);
			this.winGUI.frameEffectAbnormalAndBtn.SetToggleIndex(0);
			this.buffCategoryEnableListMap = new Dictionary<SortFilterDefine.CharacteristicFilterCategory, List<DataManagerPhoto.PhotoCharacteristicData>>();
			this.buffCategoryEnableListMap.Add(SortFilterDefine.CharacteristicFilterCategory.Conditions, new List<DataManagerPhoto.PhotoCharacteristicData>());
			this.buffCategoryEnableListMap.Add(SortFilterDefine.CharacteristicFilterCategory.Target, new List<DataManagerPhoto.PhotoCharacteristicData>());
			this.buffCategoryEnableListMap.Add(SortFilterDefine.CharacteristicFilterCategory.Effect, new List<DataManagerPhoto.PhotoCharacteristicData>());
			this.buffCategoryEnableListMap.Add(SortFilterDefine.CharacteristicFilterCategory.Abnormal, new List<DataManagerPhoto.PhotoCharacteristicData>());
		}
	}

	[CompilerGenerated]
	internal static void <OnClickReset>g__ResetButton|58_0(List<PguiToggleButtonCtrl> btnList)
	{
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in btnList)
		{
			pguiToggleButtonCtrl.SetToggleIndex(0);
		}
	}

	private PhotoFilterWindowCtrl.WindowGUI winGUI;

	private Dictionary<SortFilterDefine.CharacteristicFilterCategory, List<DataManagerPhoto.PhotoCharacteristicData>> buffCategoryEnableListMap;

	private PhotoFilterWindowCtrl.WindowTextSearchChange windowTextSearchChange;

	public class WindowGUI
	{
		public WindowGUI(GameObject go)
		{
			this.baseObj = go;
			this.baseWindow = this.baseObj.GetComponent<PguiOpenWindowCtrl>();
			this.baseWindow.Setup(string.Empty, string.Empty, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, null, null, false);
			this.scrollRect = this.baseObj.transform.Find("Base/Window/ScrollView").GetComponent<ScrollRect>();
			this.scrollContent = this.baseObj.transform.Find("Base/Window/ScrollView/Viewport/Content").gameObject;
			this.scrollContentSizeFilter = this.scrollContent.GetComponent<ContentSizeFitter>();
			this.radioBtns = this.baseObj.transform.Find("Base/Window/RadioBtns").gameObject;
			this.radioBtns.SetActive(false);
			this.resetButton = this.baseObj.transform.Find("Base/Window/Btn_reset").GetComponent<PguiButtonCtrl>();
			this.scrollContentbasePanel = new GameObject();
			this.scrollContentbasePanel.name = "BasePanel";
			this.scrollContentbasePanel.AddComponent<RectTransform>();
			this.scrollContentbasePanel.transform.SetParent(this.scrollContent.transform);
			(this.scrollContentbasePanel.transform as RectTransform).anchoredPosition = Vector3.zero;
			this.scrollContentbasePanel.transform.localScale = Vector3.one;
			VerticalLayoutGroup verticalLayoutGroup = this.scrollContentbasePanel.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = true;
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl baseWindow;

		public GameObject radioBtns;

		public PguiButtonCtrl resetButton;

		public ScrollRect scrollRect;

		public GameObject scrollContent;

		public ContentSizeFitter scrollContentSizeFilter;

		public GameObject scrollContentbasePanel;

		public PhotoFilterWindowCtrl.FrameGUI frameRarity;

		public PhotoFilterWindowCtrl.FrameGUI framePhotoType;

		public PhotoFilterWindowCtrl.FrameGUI frameFavorite;

		public PhotoFilterWindowCtrl.FrameGUI frameSpecial;

		public PhotoFilterWindowCtrl.FrameGUI frameSpecialLimit;

		public PhotoFilterWindowCtrl.FrameGUI frameAlbum;

		public PhotoFilterWindowCtrl.FrameGUI frameLevel;

		public PhotoFilterWindowCtrl.BuffFrameGUI frameBuff;

		public PguiToggleButtonCtrl frameEffectOrBtn;

		public PguiToggleButtonCtrl frameEffectAndBtn;

		public PguiToggleButtonCtrl frameEffectAbnormalOrBtn;

		public PguiToggleButtonCtrl frameEffectAbnormalAndBtn;

		public SortFilterDefine.PhotoFilterType filterType;
	}

	public class FrameGUI
	{
		public FrameGUI(GameObject go, List<PguiToggleButtonCtrl> btnList)
		{
			this.baseObj = go;
			this.buttonList = btnList;
		}

		public GameObject baseObj;

		public List<PguiToggleButtonCtrl> buttonList;
	}

	public class BuffFrameGUI
	{
		public BuffFrameGUI(GameObject go, List<PguiToggleButtonCtrl> conditionsList, List<PguiToggleButtonCtrl> targetList, List<PguiToggleButtonCtrl> effectList, List<PguiToggleButtonCtrl> resistList)
		{
			this.baseObj = go;
			this.baseObj.transform.Find("Base/Title/RadioBtns").gameObject.SetActive(false);
			this.conditionsButtonList = conditionsList;
			this.targetButtonList = targetList;
			this.effectButtonList = effectList;
			this.resistButtonList = resistList;
		}

		public List<PguiToggleButtonCtrl> buttonList
		{
			get
			{
				List<PguiToggleButtonCtrl> list = new List<PguiToggleButtonCtrl>();
				list.AddRange(this.conditionsButtonList);
				list.AddRange(this.targetButtonList);
				list.AddRange(this.effectButtonList);
				list.AddRange(this.resistButtonList);
				return list;
			}
		}

		public void ObjectDestroy()
		{
			Object.Destroy(this.baseObj);
			this.conditionsButtonList = null;
			this.targetButtonList = null;
			this.effectButtonList = null;
		}

		public GameObject baseObj;

		public List<PguiToggleButtonCtrl> conditionsButtonList;

		public List<PguiToggleButtonCtrl> targetButtonList;

		public List<PguiToggleButtonCtrl> effectButtonList;

		public List<PguiToggleButtonCtrl> resistButtonList;
	}

	public class ButtonObject
	{
		private CharaDef.ActionBuffType buffType;

		private string DispName;
	}

	public class WindowTextSearchChange
	{
		public WindowTextSearchChange(Transform baseTr)
		{
			this.InputField = baseTr.Find("Base/Window/ScrollView/Viewport/Content/Box00/InputField").GetComponent<InputField>();
			this.TextTransform = baseTr.Find("Base/Window/ScrollView/Viewport/Content/Box00/InputField/Txt").GetComponent<Transform>();
			this.InputField.lineType = InputField.LineType.SingleLine;
		}

		public InputField InputField;

		public Transform TextTransform;
	}
}
