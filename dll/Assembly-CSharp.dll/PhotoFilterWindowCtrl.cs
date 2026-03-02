using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001AF RID: 431
public class PhotoFilterWindowCtrl
{
	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x06001D01 RID: 7425 RVA: 0x0016A994 File Offset: 0x00168B94
	// (set) Token: 0x06001D02 RID: 7426 RVA: 0x0016A99C File Offset: 0x00168B9C
	private string SearchText { get; set; }

	// Token: 0x06001D03 RID: 7427 RVA: 0x0016A9A8 File Offset: 0x00168BA8
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

	// Token: 0x06001D04 RID: 7428 RVA: 0x0016AAF4 File Offset: 0x00168CF4
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

	// Token: 0x06001D05 RID: 7429 RVA: 0x0016ACEC File Offset: 0x00168EEC
	private void ExceptionCategoryName(List<DataManagerPhoto.PhotoCharacteristicData> sourceList)
	{
		sourceList.Find((DataManagerPhoto.PhotoCharacteristicData item) => item.CategoryName != sourceList[0].CategoryName);
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x0016AD20 File Offset: 0x00168F20
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

	// Token: 0x06001D07 RID: 7431 RVA: 0x0016AE98 File Offset: 0x00169098
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

	// Token: 0x06001D08 RID: 7432 RVA: 0x0016AEEC File Offset: 0x001690EC
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

	// Token: 0x06001D09 RID: 7433 RVA: 0x0016AFB4 File Offset: 0x001691B4
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

	// Token: 0x06001D0A RID: 7434 RVA: 0x0016B164 File Offset: 0x00169364
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

	// Token: 0x06001D0B RID: 7435 RVA: 0x0016B218 File Offset: 0x00169418
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

	// Token: 0x06001D0C RID: 7436 RVA: 0x0016B2CC File Offset: 0x001694CC
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

	// Token: 0x06001D0D RID: 7437 RVA: 0x0016B378 File Offset: 0x00169578
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

	// Token: 0x06001D0E RID: 7438 RVA: 0x0016B51C File Offset: 0x0016971C
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

	// Token: 0x06001D0F RID: 7439 RVA: 0x0016B5D0 File Offset: 0x001697D0
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

	// Token: 0x06001D10 RID: 7440 RVA: 0x0016B62C File Offset: 0x0016982C
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

	// Token: 0x06001D11 RID: 7441 RVA: 0x0016B688 File Offset: 0x00169888
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

	// Token: 0x06001D12 RID: 7442 RVA: 0x0016B6E3 File Offset: 0x001698E3
	public bool OnClickRarityButton(int btnIdx, int toggleIdx)
	{
		this.winGUI.frameRarity.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x0016B702 File Offset: 0x00169902
	public bool OnClickPhotoTypeButton(int btnIdx, int toggleIdx)
	{
		this.winGUI.framePhotoType.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x0016B724 File Offset: 0x00169924
	public bool OnClickFavoriteButton(int btnIdx, int toggleIdx)
	{
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.winGUI.frameFavorite.buttonList)
		{
			pguiToggleButtonCtrl.SetToggleIndex(0);
		}
		this.winGUI.frameFavorite.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x0016B79C File Offset: 0x0016999C
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

	// Token: 0x06001D16 RID: 7446 RVA: 0x0016B7F0 File Offset: 0x001699F0
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

	// Token: 0x06001D17 RID: 7447 RVA: 0x0016B844 File Offset: 0x00169A44
	public bool OnClickPhotoAlbumButton(int btnIdx, int toggleIdx)
	{
		this.winGUI.frameAlbum.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x0016B863 File Offset: 0x00169A63
	private void RegistBuff(SortFilterDefine.CharacteristicFilterCategory category, DataManagerPhoto.PhotoCharacteristicData source, int toggleIdx)
	{
		if (toggleIdx == 0)
		{
			this.buffCategoryEnableListMap[category].Add(source);
			return;
		}
		this.buffCategoryEnableListMap[category].Remove(source);
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x0016B891 File Offset: 0x00169A91
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

	// Token: 0x06001D1A RID: 7450 RVA: 0x0016B8C1 File Offset: 0x00169AC1
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

	// Token: 0x06001D1B RID: 7451 RVA: 0x0016B8F1 File Offset: 0x00169AF1
	public bool OnClickBuffCondition(DataManagerPhoto.PhotoCharacteristicData source, int btnIdx, int toggleIdx)
	{
		this.RegistBuff(SortFilterDefine.CharacteristicFilterCategory.Conditions, source, toggleIdx);
		return true;
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x0016B8FD File Offset: 0x00169AFD
	public bool OnClickBuffTarget(DataManagerPhoto.PhotoCharacteristicData source, int btnIdx, int toggleIdx)
	{
		this.RegistBuff(SortFilterDefine.CharacteristicFilterCategory.Target, source, toggleIdx);
		return true;
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x0016B909 File Offset: 0x00169B09
	public bool OnClickBuffEffect(DataManagerPhoto.PhotoCharacteristicData source, int btnIdx, int toggleIdx)
	{
		this.RegistBuff(SortFilterDefine.CharacteristicFilterCategory.Effect, source, toggleIdx);
		return true;
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x0016B915 File Offset: 0x00169B15
	public bool OnClickAbnormal(DataManagerPhoto.PhotoCharacteristicData source, int btnIdx, int toggleIdx)
	{
		this.RegistBuff(SortFilterDefine.CharacteristicFilterCategory.Abnormal, source, toggleIdx);
		return true;
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x0016B924 File Offset: 0x00169B24
	public void Open()
	{
		this.winGUI.baseWindow.Setup(PrjUtil.MakeMessage("フィルターの設定"), PrjUtil.MakeMessage("選択したカテゴリで絞り込みができます"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), null, false);
		this.winGUI.baseWindow.Open();
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x0016B97C File Offset: 0x00169B7C
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

	// Token: 0x06001D21 RID: 7457 RVA: 0x0016B9CC File Offset: 0x00169BCC
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

	// Token: 0x06001D22 RID: 7458 RVA: 0x0016BA1C File Offset: 0x00169C1C
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

	// Token: 0x06001D23 RID: 7459 RVA: 0x0016BA68 File Offset: 0x00169C68
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

	// Token: 0x06001D24 RID: 7460 RVA: 0x0016BAB6 File Offset: 0x00169CB6
	public bool GetInvalidBonus()
	{
		return this.winGUI.frameSpecial.buttonList[0].GetToggleIndex() == 1;
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x0016BAD8 File Offset: 0x00169CD8
	public bool GetInvalidLimit()
	{
		if (!DataManager.DmPhoto.IsBonusActive())
		{
			return this.winGUI.frameSpecialLimit.buttonList[0].GetToggleIndex() == 1;
		}
		return this.winGUI.frameSpecial.buttonList[1].GetToggleIndex() == 1;
	}

	// Token: 0x06001D26 RID: 7462 RVA: 0x0016BB30 File Offset: 0x00169D30
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

	// Token: 0x06001D27 RID: 7463 RVA: 0x0016BB8C File Offset: 0x00169D8C
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

	// Token: 0x06001D28 RID: 7464 RVA: 0x0016BBE7 File Offset: 0x00169DE7
	public string GetSearchText()
	{
		return this.windowTextSearchChange.InputField.text;
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x0016BBF9 File Offset: 0x00169DF9
	public void SetupSearchText(string searchText)
	{
		this.SearchText = searchText;
		this.windowTextSearchChange.InputField.text = this.SearchText;
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x0016BC18 File Offset: 0x00169E18
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

	// Token: 0x06001D2B RID: 7467 RVA: 0x0016BC48 File Offset: 0x00169E48
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

	// Token: 0x06001D2C RID: 7468 RVA: 0x0016BC78 File Offset: 0x00169E78
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

	// Token: 0x06001D2D RID: 7469 RVA: 0x0016BCA8 File Offset: 0x00169EA8
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

	// Token: 0x06001D2E RID: 7470 RVA: 0x0016BCD8 File Offset: 0x00169ED8
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

	// Token: 0x06001D2F RID: 7471 RVA: 0x0016BD0C File Offset: 0x00169F0C
	private void ResetSearchText()
	{
		if (this.windowTextSearchChange.InputField.text.Length == 0)
		{
			this.windowTextSearchChange.TextTransform.gameObject.SetActive(false);
		}
		this.windowTextSearchChange.InputField.text = this.SearchText;
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x0016BD5C File Offset: 0x00169F5C
	public void SetupSearchTextActive()
	{
		if (this.windowTextSearchChange.TextTransform != null)
		{
			this.windowTextSearchChange.TextTransform.gameObject.SetActive(true);
		}
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x0016BD88 File Offset: 0x00169F88
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

	// Token: 0x06001D37 RID: 7479 RVA: 0x0016BF4C File Offset: 0x0016A14C
	[CompilerGenerated]
	internal static void <OnClickReset>g__ResetButton|58_0(List<PguiToggleButtonCtrl> btnList)
	{
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in btnList)
		{
			pguiToggleButtonCtrl.SetToggleIndex(0);
		}
	}

	// Token: 0x0400157E RID: 5502
	private PhotoFilterWindowCtrl.WindowGUI winGUI;

	// Token: 0x0400157F RID: 5503
	private Dictionary<SortFilterDefine.CharacteristicFilterCategory, List<DataManagerPhoto.PhotoCharacteristicData>> buffCategoryEnableListMap;

	// Token: 0x04001580 RID: 5504
	private PhotoFilterWindowCtrl.WindowTextSearchChange windowTextSearchChange;

	// Token: 0x02000F36 RID: 3894
	public class WindowGUI
	{
		// Token: 0x06004EF1 RID: 20209 RVA: 0x00238550 File Offset: 0x00236750
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

		// Token: 0x04005655 RID: 22101
		public GameObject baseObj;

		// Token: 0x04005656 RID: 22102
		public PguiOpenWindowCtrl baseWindow;

		// Token: 0x04005657 RID: 22103
		public GameObject radioBtns;

		// Token: 0x04005658 RID: 22104
		public PguiButtonCtrl resetButton;

		// Token: 0x04005659 RID: 22105
		public ScrollRect scrollRect;

		// Token: 0x0400565A RID: 22106
		public GameObject scrollContent;

		// Token: 0x0400565B RID: 22107
		public ContentSizeFitter scrollContentSizeFilter;

		// Token: 0x0400565C RID: 22108
		public GameObject scrollContentbasePanel;

		// Token: 0x0400565D RID: 22109
		public PhotoFilterWindowCtrl.FrameGUI frameRarity;

		// Token: 0x0400565E RID: 22110
		public PhotoFilterWindowCtrl.FrameGUI framePhotoType;

		// Token: 0x0400565F RID: 22111
		public PhotoFilterWindowCtrl.FrameGUI frameFavorite;

		// Token: 0x04005660 RID: 22112
		public PhotoFilterWindowCtrl.FrameGUI frameSpecial;

		// Token: 0x04005661 RID: 22113
		public PhotoFilterWindowCtrl.FrameGUI frameSpecialLimit;

		// Token: 0x04005662 RID: 22114
		public PhotoFilterWindowCtrl.FrameGUI frameAlbum;

		// Token: 0x04005663 RID: 22115
		public PhotoFilterWindowCtrl.FrameGUI frameLevel;

		// Token: 0x04005664 RID: 22116
		public PhotoFilterWindowCtrl.BuffFrameGUI frameBuff;

		// Token: 0x04005665 RID: 22117
		public PguiToggleButtonCtrl frameEffectOrBtn;

		// Token: 0x04005666 RID: 22118
		public PguiToggleButtonCtrl frameEffectAndBtn;

		// Token: 0x04005667 RID: 22119
		public PguiToggleButtonCtrl frameEffectAbnormalOrBtn;

		// Token: 0x04005668 RID: 22120
		public PguiToggleButtonCtrl frameEffectAbnormalAndBtn;

		// Token: 0x04005669 RID: 22121
		public SortFilterDefine.PhotoFilterType filterType;
	}

	// Token: 0x02000F37 RID: 3895
	public class FrameGUI
	{
		// Token: 0x06004EF2 RID: 20210 RVA: 0x002386DA File Offset: 0x002368DA
		public FrameGUI(GameObject go, List<PguiToggleButtonCtrl> btnList)
		{
			this.baseObj = go;
			this.buttonList = btnList;
		}

		// Token: 0x0400566A RID: 22122
		public GameObject baseObj;

		// Token: 0x0400566B RID: 22123
		public List<PguiToggleButtonCtrl> buttonList;
	}

	// Token: 0x02000F38 RID: 3896
	public class BuffFrameGUI
	{
		// Token: 0x06004EF3 RID: 20211 RVA: 0x002386F0 File Offset: 0x002368F0
		public BuffFrameGUI(GameObject go, List<PguiToggleButtonCtrl> conditionsList, List<PguiToggleButtonCtrl> targetList, List<PguiToggleButtonCtrl> effectList, List<PguiToggleButtonCtrl> resistList)
		{
			this.baseObj = go;
			this.baseObj.transform.Find("Base/Title/RadioBtns").gameObject.SetActive(false);
			this.conditionsButtonList = conditionsList;
			this.targetButtonList = targetList;
			this.effectButtonList = effectList;
			this.resistButtonList = resistList;
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004EF4 RID: 20212 RVA: 0x00238748 File Offset: 0x00236948
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

		// Token: 0x06004EF5 RID: 20213 RVA: 0x0023877F File Offset: 0x0023697F
		public void ObjectDestroy()
		{
			Object.Destroy(this.baseObj);
			this.conditionsButtonList = null;
			this.targetButtonList = null;
			this.effectButtonList = null;
		}

		// Token: 0x0400566C RID: 22124
		public GameObject baseObj;

		// Token: 0x0400566D RID: 22125
		public List<PguiToggleButtonCtrl> conditionsButtonList;

		// Token: 0x0400566E RID: 22126
		public List<PguiToggleButtonCtrl> targetButtonList;

		// Token: 0x0400566F RID: 22127
		public List<PguiToggleButtonCtrl> effectButtonList;

		// Token: 0x04005670 RID: 22128
		public List<PguiToggleButtonCtrl> resistButtonList;
	}

	// Token: 0x02000F39 RID: 3897
	public class ButtonObject
	{
		// Token: 0x04005671 RID: 22129
		private CharaDef.ActionBuffType buffType;

		// Token: 0x04005672 RID: 22130
		private string DispName;
	}

	// Token: 0x02000F3A RID: 3898
	public class WindowTextSearchChange
	{
		// Token: 0x06004EF7 RID: 20215 RVA: 0x002387A9 File Offset: 0x002369A9
		public WindowTextSearchChange(Transform baseTr)
		{
			this.InputField = baseTr.Find("Base/Window/ScrollView/Viewport/Content/Box00/InputField").GetComponent<InputField>();
			this.TextTransform = baseTr.Find("Base/Window/ScrollView/Viewport/Content/Box00/InputField/Txt").GetComponent<Transform>();
			this.InputField.lineType = InputField.LineType.SingleLine;
		}

		// Token: 0x04005673 RID: 22131
		public InputField InputField;

		// Token: 0x04005674 RID: 22132
		public Transform TextTransform;
	}
}
