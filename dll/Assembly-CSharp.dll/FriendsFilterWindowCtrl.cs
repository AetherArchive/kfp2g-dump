using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

public class FriendsFilterWindowCtrl
{
	private string SearchText { get; set; }

	public void Initialize(GameObject go)
	{
		this.winGUI = new FriendsFilterWindowCtrl.WindowGUI(go);
		this.winGUI.resetButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickReset), PguiButtonCtrl.SoundType.DEFAULT);
		this.winGUI.frameType = this.CreateButtonFrame("属性", SortFilterDefine.friendsFilterTypeList);
		this.winGUI.frameFriends = this.CreateButtonFrame("フレンズ", SortFilterDefine.friendsFilterFriendsList);
		this.winGUI.frameFavorite = this.CreateButtonFrame("お気に入り", SortFilterDefine.friendsFilterFavoriteList);
		this.winGUI.frameEvent = this.CreateButtonFrame("イベント", SortFilterDefine.friendsFilterEventList);
		this.windowTextSearchChange = new FriendsFilterWindowCtrl.WindowTextSearchChange(go.transform);
		this.SearchText = "";
		this.windowTextSearchChange.InputField.onEndEdit.AddListener(delegate(string str)
		{
			this.windowTextSearchChange.InputField.text = PrjUtil.ModifiedComment(str);
			this.SearchText = this.windowTextSearchChange.InputField.text;
		});
	}

	public FriendsFilterWindowCtrl.FrameGUI CreateButtonFrame(string frameName, List<string> buttonNameList)
	{
		List<PguiToggleButtonCtrl> list = new List<PguiToggleButtonCtrl>();
		GameObject gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterPhoto_Parts01", Singleton<CanvasManager>.Instance.SystemPanel.transform);
		gameObject.transform.SetParent(this.winGUI.scrollContentbasePanel.transform, false);
		gameObject.transform.Find("Base/Title").GetComponent<PguiTextCtrl>().text = frameName;
		gameObject.transform.Find("Base/Title").GetComponent<PguiTextCtrl>();
		gameObject.transform.Find("Base/Title/RadioBtns").gameObject.SetActive(false);
		GameObject assetObj = this.GetAssetObj(frameName);
		List<PguiToggleButtonCtrl> filterButtonList = this.GetFilterButtonList(assetObj, frameName);
		assetObj.transform.SetParent(gameObject.transform.Find("Base"), false);
		list.AddRange(filterButtonList);
		return new FriendsFilterWindowCtrl.FrameGUI(gameObject, list);
	}

	private void ExceptionCategoryNameMach(List<DataManagerChara.FilterData> sourceList)
	{
		sourceList.Find((DataManagerChara.FilterData item) => item.CategoryName != sourceList[0].CategoryName);
	}

	public FriendsFilterWindowCtrl.BuffFrameGUI CreateMiracleButtonFrame(string frameName, List<string> enebleButtonNameList)
	{
		FriendsFilterWindowCtrl.<>c__DisplayClass16_0 CS$<>8__locals1 = new FriendsFilterWindowCtrl.<>c__DisplayClass16_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.enebleButtonNameList = enebleButtonNameList;
		List<DataManagerChara.FilterData> friendFilterDataList = DataManager.DmChara.GetFriendFilterDataList(true);
		if (friendFilterDataList == null)
		{
			return null;
		}
		GameObject gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterPhoto_Parts01", Singleton<CanvasManager>.Instance.SystemPanel.transform);
		gameObject.transform.SetParent(this.winGUI.scrollContentbasePanel.transform, false);
		gameObject.transform.Find("Base/Title").GetComponent<PguiTextCtrl>().text = frameName;
		List<DataManagerChara.FilterData> list = friendFilterDataList.FindAll((DataManagerChara.FilterData item) => item.Category == SortFilterDefine.CharacteristicFilterCategory.Target);
		List<DataManagerChara.FilterData> list2 = friendFilterDataList.FindAll((DataManagerChara.FilterData item) => item.Category == SortFilterDefine.CharacteristicFilterCategory.Effect);
		this.friendsMiracleEnableListMap = new Dictionary<SortFilterDefine.FriendsMiracleFileterCategory, List<DataManagerChara.FilterData>>();
		List<PguiToggleButtonCtrl> list3 = CS$<>8__locals1.<CreateMiracleButtonFrame>g__CreatebuttonList|0(SortFilterDefine.FriendsMiracleFileterCategory.Target, gameObject, list);
		List<PguiToggleButtonCtrl> list4 = CS$<>8__locals1.<CreateMiracleButtonFrame>g__CreatebuttonList|0(SortFilterDefine.FriendsMiracleFileterCategory.Effect, gameObject, list2);
		return new FriendsFilterWindowCtrl.BuffFrameGUI(gameObject, null, list3, list4, null);
	}

	public FriendsFilterWindowCtrl.BuffFrameGUI CreateCharacteristicButtonFrame(string frameName, List<string> enebleButtonNameList)
	{
		FriendsFilterWindowCtrl.<>c__DisplayClass17_0 CS$<>8__locals1 = new FriendsFilterWindowCtrl.<>c__DisplayClass17_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.enebleButtonNameList = enebleButtonNameList;
		List<DataManagerChara.FilterData> friendFilterDataList = DataManager.DmChara.GetFriendFilterDataList(false);
		if (friendFilterDataList == null)
		{
			return null;
		}
		GameObject gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterPhoto_Parts01", Singleton<CanvasManager>.Instance.SystemPanel.transform);
		gameObject.transform.SetParent(this.winGUI.scrollContentbasePanel.transform, false);
		gameObject.transform.Find("Base/Title").GetComponent<PguiTextCtrl>().text = frameName;
		List<DataManagerChara.FilterData> list = friendFilterDataList.FindAll((DataManagerChara.FilterData item) => item.Category == SortFilterDefine.CharacteristicFilterCategory.Conditions);
		List<DataManagerChara.FilterData> list2 = friendFilterDataList.FindAll((DataManagerChara.FilterData item) => item.Category == SortFilterDefine.CharacteristicFilterCategory.Target);
		List<DataManagerChara.FilterData> list3 = friendFilterDataList.FindAll((DataManagerChara.FilterData item) => item.Category == SortFilterDefine.CharacteristicFilterCategory.Effect);
		List<DataManagerChara.FilterData> list4 = friendFilterDataList.FindAll((DataManagerChara.FilterData item) => item.Category == SortFilterDefine.CharacteristicFilterCategory.Abnormal);
		this.friendsCharacteristicEnableListMap = new Dictionary<SortFilterDefine.FriendsCharacteristicFilterCategory, List<DataManagerChara.FilterData>>();
		List<PguiToggleButtonCtrl> list5 = CS$<>8__locals1.<CreateCharacteristicButtonFrame>g__CreatebuttonList|0(SortFilterDefine.FriendsCharacteristicFilterCategory.Conditions, gameObject, list);
		List<PguiToggleButtonCtrl> list6 = CS$<>8__locals1.<CreateCharacteristicButtonFrame>g__CreatebuttonList|0(SortFilterDefine.FriendsCharacteristicFilterCategory.Target, gameObject, list2);
		List<PguiToggleButtonCtrl> list7 = CS$<>8__locals1.<CreateCharacteristicButtonFrame>g__CreatebuttonList|0(SortFilterDefine.FriendsCharacteristicFilterCategory.Effect, gameObject, list3);
		List<PguiToggleButtonCtrl> list8 = CS$<>8__locals1.<CreateCharacteristicButtonFrame>g__CreatebuttonList|0(SortFilterDefine.FriendsCharacteristicFilterCategory.Resist, gameObject, list4);
		new List<PguiToggleButtonCtrl>();
		return new FriendsFilterWindowCtrl.BuffFrameGUI(gameObject, list5, list6, list7, list8);
	}

	public GameObject GetAssetObj(string frameName)
	{
		GameObject gameObject;
		if (!(frameName == "属性"))
		{
			if (!(frameName == "イベント"))
			{
				if (!(frameName == "フレンズ"))
				{
					if (!(frameName == "お気に入り"))
					{
						gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterPhoto_Parts02", Singleton<CanvasManager>.Instance.SystemPanel.transform);
					}
					else
					{
						gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterFriends_PartsFavorite", Singleton<CanvasManager>.Instance.SystemPanel.transform);
					}
				}
				else
				{
					gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterFriends_PartsFriends", Singleton<CanvasManager>.Instance.SystemPanel.transform);
				}
			}
			else
			{
				gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterFriends_PartsEvent", Singleton<CanvasManager>.Instance.SystemPanel.transform);
			}
		}
		else
		{
			gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterFriends_PartsType", Singleton<CanvasManager>.Instance.SystemPanel.transform);
		}
		return gameObject;
	}

	public List<PguiToggleButtonCtrl> GetFilterButtonList(GameObject buttonObj, string frameName = "")
	{
		List<PguiToggleButtonCtrl> list = new List<PguiToggleButtonCtrl>();
		if (!(frameName == "属性"))
		{
			if (!(frameName == "イベント"))
			{
				if (!(frameName == "フレンズ"))
				{
					if (!(frameName == "お気に入り"))
					{
						list.Add(buttonObj.transform.Find("Btn01").GetComponent<PguiToggleButtonCtrl>());
						list.Add(buttonObj.transform.Find("Btn02").GetComponent<PguiToggleButtonCtrl>());
						list.Add(buttonObj.transform.Find("Btn03").GetComponent<PguiToggleButtonCtrl>());
						list.Add(buttonObj.transform.Find("Btn04").GetComponent<PguiToggleButtonCtrl>());
					}
					else
					{
						list.Add(buttonObj.transform.Find("Btn01").GetComponent<PguiToggleButtonCtrl>());
						list.Add(buttonObj.transform.Find("Btn02").GetComponent<PguiToggleButtonCtrl>());
					}
				}
				else
				{
					list.Add(buttonObj.transform.Find("Btn01").GetComponent<PguiToggleButtonCtrl>());
					list.Add(buttonObj.transform.Find("Btn02").GetComponent<PguiToggleButtonCtrl>());
				}
			}
			else
			{
				list.Add(buttonObj.transform.Find("Btn01").GetComponent<PguiToggleButtonCtrl>());
				list.Add(buttonObj.transform.Find("Btn02").GetComponent<PguiToggleButtonCtrl>());
				list[0].gameObject.SetActive(false);
				list[1].gameObject.SetActive(false);
			}
		}
		else
		{
			list.Add(buttonObj.transform.Find("Btn01").GetComponent<PguiToggleButtonCtrl>());
			list.Add(buttonObj.transform.Find("Btn02").GetComponent<PguiToggleButtonCtrl>());
			list.Add(buttonObj.transform.Find("Btn03").GetComponent<PguiToggleButtonCtrl>());
			list.Add(buttonObj.transform.Find("Btn04").GetComponent<PguiToggleButtonCtrl>());
			list.Add(buttonObj.transform.Find("Btn05").GetComponent<PguiToggleButtonCtrl>());
			list.Add(buttonObj.transform.Find("Btn06").GetComponent<PguiToggleButtonCtrl>());
		}
		return list;
	}

	public void SetupWindow(List<string> enebleButtonNameList, SortFilterDefine.AndOrState andOrState, SortFilterDefine.AndOrState andOrStateResist)
	{
		if (this.winGUI.frameMiracle == null)
		{
			this.winGUI.frameMiracle = this.CreateMiracleButtonFrame("ミラクル効果", SortFilterDefine.friendsFilterMiracleList);
		}
		if (this.winGUI.frameCharacteristic == null)
		{
			this.winGUI.frameCharacteristic = this.CreateCharacteristicButtonFrame("とくせい", SortFilterDefine.friendsFilterMiracleList);
		}
	}

	public void SetupAttributeTupe(List<CharaDef.AttributeType> attributeList)
	{
		for (int i = 0; i < this.winGUI.frameType.buttonList.Count; i++)
		{
			this.winGUI.frameType.buttonList[i].SetToggleIndex(attributeList.Contains(i + CharaDef.AttributeType.RED) ? 1 : 0);
			int btnIdx = i;
			this.winGUI.frameType.buttonList[i].AllRemoveOnClickListener();
			this.winGUI.frameType.buttonList[i].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickAttributeButton(btnIdx, toggleIdx));
		}
	}

	public void SetupFriend(bool[] registButtonList)
	{
		for (int i = 0; i < this.winGUI.frameFriends.buttonList.Count; i++)
		{
			this.winGUI.frameFriends.buttonList[i].SetToggleIndex(registButtonList[i] ? 1 : 0);
			int btnIdx = i;
			this.winGUI.frameFriends.buttonList[i].AllRemoveOnClickListener();
			this.winGUI.frameFriends.buttonList[i].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickFriendsButton(btnIdx, toggleIdx));
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

	public void SetupEvent(List<int> registEventList)
	{
		for (int i = 0; i < this.winGUI.frameEvent.buttonList.Count; i++)
		{
			this.winGUI.frameEvent.buttonList[i].SetToggleIndex(registEventList.Contains(i + 1) ? 1 : 0);
			int btnIdx = i;
			this.winGUI.frameEvent.buttonList[i].AllRemoveOnClickListener();
			this.winGUI.frameEvent.buttonList[i].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickFriendsEventButton(btnIdx, toggleIdx));
		}
		this.winGUI.frameEvent.baseObj.SetActive(false);
		List<int> validEventIdListWithoutMissionEvent = DataManager.DmEvent.GetValidEventIdListWithoutMissionEvent();
		if (validEventIdListWithoutMissionEvent.Count > 0)
		{
			this.winGUI.frameEvent.baseObj.SetActive(true);
		}
		int num = -1;
		int num2 = 0;
		while (num2 < validEventIdListWithoutMissionEvent.Count && num2 < 2)
		{
			if (DataManager.DmChara.GetBonusCharaDataList(validEventIdListWithoutMissionEvent[num2]).Count != 0)
			{
				num++;
				Transform transform = this.winGUI.frameEvent.buttonList[num].transform;
				transform.gameObject.SetActive(true);
				transform.GetComponent<PguiToggleButtonCtrl>().SetToggleIndex(registEventList.Contains(validEventIdListWithoutMissionEvent[num2]) ? 1 : 0);
				PguiTextCtrl component = transform.Find("BaseImage/Txt_Event").GetComponent<PguiTextCtrl>();
				DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(validEventIdListWithoutMissionEvent[num2]);
				component.text = eventData.eventName;
			}
			num2++;
		}
	}

	public void SetupSearchText(string searchText)
	{
		this.SearchText = searchText;
		this.windowTextSearchChange.InputField.text = searchText;
	}

	public void SetupCharacteristic(List<int> regConditionList, List<int> targetList, List<int> effectList, List<int> resistList)
	{
		for (int i = 0; i < this.winGUI.frameCharacteristic.conditionsButtonList.Count; i++)
		{
			if (regConditionList.Contains(i))
			{
				this.winGUI.frameCharacteristic.conditionsButtonList[i].SetToggleIndex(1);
			}
			else
			{
				this.winGUI.frameCharacteristic.conditionsButtonList[i].SetToggleIndex(0);
			}
		}
		for (int j = 0; j < this.winGUI.frameCharacteristic.targetButtonList.Count; j++)
		{
			if (targetList.Contains(j))
			{
				this.winGUI.frameCharacteristic.targetButtonList[j].SetToggleIndex(1);
			}
			else
			{
				this.winGUI.frameCharacteristic.targetButtonList[j].SetToggleIndex(0);
			}
		}
		for (int k = 0; k < this.winGUI.frameCharacteristic.effectButtonList.Count; k++)
		{
			if (effectList.Contains(k))
			{
				this.winGUI.frameCharacteristic.effectButtonList[k].SetToggleIndex(1);
			}
			else
			{
				this.winGUI.frameCharacteristic.effectButtonList[k].SetToggleIndex(0);
			}
		}
		for (int l = 0; l < this.winGUI.frameCharacteristic.resistButtonList.Count; l++)
		{
			if (resistList.Contains(l))
			{
				this.winGUI.frameCharacteristic.resistButtonList[l].SetToggleIndex(1);
			}
			else
			{
				this.winGUI.frameCharacteristic.resistButtonList[l].SetToggleIndex(0);
			}
		}
	}

	public void SetupMiracle(List<int> target, List<int> effect)
	{
		for (int i = 0; i < this.winGUI.frameMiracle.targetButtonList.Count; i++)
		{
			if (target.Contains(i))
			{
				this.winGUI.frameMiracle.targetButtonList[i].SetToggleIndex(1);
			}
			else
			{
				this.winGUI.frameMiracle.targetButtonList[i].SetToggleIndex(0);
			}
		}
		for (int j = 0; j < this.winGUI.frameMiracle.effectButtonList.Count; j++)
		{
			if (effect.Contains(j))
			{
				this.winGUI.frameMiracle.effectButtonList[j].SetToggleIndex(1);
			}
			else
			{
				this.winGUI.frameMiracle.effectButtonList[j].SetToggleIndex(0);
			}
		}
	}

	public bool OnClickAttributeButton(int btnIdx, int toggleIdx)
	{
		this.winGUI.frameType.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	public bool OnClickFriendsButton(int btnIdx, int toggleIdx)
	{
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.winGUI.frameFriends.buttonList)
		{
			pguiToggleButtonCtrl.SetToggleIndex(0);
		}
		this.winGUI.frameFriends.buttonList[btnIdx].SetToggleIndex(toggleIdx);
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

	public bool OnClickFriendsEventButton(int btnIdx, int toggleIdx)
	{
		this.winGUI.frameEvent.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	private void RegistMiracle(SortFilterDefine.FriendsMiracleFileterCategory category, DataManagerChara.FilterData source, int toggleIdx)
	{
		if (toggleIdx == 0)
		{
			this.friendsMiracleEnableListMap[category].Add(source);
			return;
		}
		this.friendsMiracleEnableListMap[category].Remove(source);
	}

	private void RegistCharacteristic(SortFilterDefine.FriendsCharacteristicFilterCategory category, DataManagerChara.FilterData source, int toggleIdx)
	{
		if (toggleIdx == 0)
		{
			this.friendsCharacteristicEnableListMap[category].Add(source);
			return;
		}
		this.friendsCharacteristicEnableListMap[category].Remove(source);
	}

	public bool OnClickMiracleBuffOrAnd(bool isOr, int toggleIdx)
	{
		if (1 == toggleIdx)
		{
			return false;
		}
		if (isOr)
		{
			this.winGUI.frameMiracleEffectAndBtn.SetToggleIndex(0);
		}
		else
		{
			this.winGUI.frameMiracleEffectOrBtn.SetToggleIndex(0);
		}
		return true;
	}

	public bool OnClickCharacteristicBuffOrAnd(bool isOr, int toggleIdx)
	{
		if (1 == toggleIdx)
		{
			return false;
		}
		if (isOr)
		{
			this.winGUI.frameCharacteristicEffectAndBtn.SetToggleIndex(0);
		}
		else
		{
			this.winGUI.frameCharacteristicEffectOrBtn.SetToggleIndex(0);
		}
		return true;
	}

	private bool OnClickCharacteristicResistOrAnd(bool isOr, int toggleIdx)
	{
		if (1 == toggleIdx)
		{
			return false;
		}
		if (isOr)
		{
			this.winGUI.frameCharacteristicResistAndBtn.SetToggleIndex(0);
		}
		else
		{
			this.winGUI.frameCharacteristicResistOrBtn.SetToggleIndex(0);
		}
		return true;
	}

	public bool OnClickMiracleTarget(DataManagerChara.FilterData source, int btnIdx, int toggleIdx)
	{
		this.RegistMiracle(SortFilterDefine.FriendsMiracleFileterCategory.Target, source, toggleIdx);
		return true;
	}

	public bool OnClickMiracleEffect(DataManagerChara.FilterData source, int btnIdx, int toggleIdx)
	{
		this.RegistMiracle(SortFilterDefine.FriendsMiracleFileterCategory.Effect, source, toggleIdx);
		return true;
	}

	public bool OnClickBuffCharacteristicCondition(DataManagerChara.FilterData source, int btnIdx, int toggleIdx)
	{
		this.RegistCharacteristic(SortFilterDefine.FriendsCharacteristicFilterCategory.Conditions, source, toggleIdx);
		return true;
	}

	public bool OnClickBuffCharacteristicTarget(DataManagerChara.FilterData source, int btnIdx, int toggleIdx)
	{
		this.RegistCharacteristic(SortFilterDefine.FriendsCharacteristicFilterCategory.Target, source, toggleIdx);
		return true;
	}

	public bool OnClickBuffCharacteristicEffect(DataManagerChara.FilterData source, int btnIdx, int toggleIdx)
	{
		this.RegistCharacteristic(SortFilterDefine.FriendsCharacteristicFilterCategory.Effect, source, toggleIdx);
		return true;
	}

	public bool OnClickBuffCharacteristicResist(DataManagerChara.FilterData source, int btnIdx, int toggleIdx)
	{
		this.RegistCharacteristic(SortFilterDefine.FriendsCharacteristicFilterCategory.Resist, source, toggleIdx);
		return true;
	}

	public List<CharaDef.AttributeType> GetAttributeButtonStatus()
	{
		List<PguiToggleButtonCtrl> buttonList = this.winGUI.frameType.buttonList;
		List<CharaDef.AttributeType> list = new List<CharaDef.AttributeType>();
		for (int i = 0; i < buttonList.Count; i++)
		{
			if (buttonList[i].GetToggleIndex() == 1)
			{
				list.Add(i + CharaDef.AttributeType.RED);
			}
		}
		return list;
	}

	public List<int> GetEventButtonStatus()
	{
		List<int> validEventIdListWithoutMissionEvent = DataManager.DmEvent.GetValidEventIdListWithoutMissionEvent();
		List<int> list = new List<int>();
		foreach (int num in validEventIdListWithoutMissionEvent)
		{
			if (DataManager.DmChara.GetBonusCharaDataList(num).Count != 0)
			{
				list.Add(num);
			}
		}
		List<PguiToggleButtonCtrl> buttonList = this.winGUI.frameEvent.buttonList;
		List<int> list2 = new List<int>();
		for (int i = 0; i < buttonList.Count; i++)
		{
			if (buttonList[i].GetToggleIndex() == 1)
			{
				list2.Add(list[i]);
			}
		}
		return list2;
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

	public bool[] GetFriendButtonStatus()
	{
		List<PguiToggleButtonCtrl> buttonList = this.winGUI.frameFriends.buttonList;
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

	public SortFilterDefine.AndOrState GetMiracleAndORStatus()
	{
		if (this.winGUI.frameMiracle == null)
		{
			return SortFilterDefine.AndOrState.Invalid;
		}
		int toggleIndex = this.winGUI.frameMiracleEffectAndBtn.GetToggleIndex();
		int toggleIndex2 = this.winGUI.frameMiracleEffectOrBtn.GetToggleIndex();
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

	public SortFilterDefine.AndOrState GetCharacteristicEffectAndOrStatus()
	{
		if (this.winGUI.frameMiracle == null)
		{
			return SortFilterDefine.AndOrState.Invalid;
		}
		int toggleIndex = this.winGUI.frameCharacteristicEffectAndBtn.GetToggleIndex();
		int toggleIndex2 = this.winGUI.frameCharacteristicEffectOrBtn.GetToggleIndex();
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

	public List<DataManagerChara.FilterData> GetMiracleTargetDataList()
	{
		if (this.friendsMiracleEnableListMap == null)
		{
			return new List<DataManagerChara.FilterData>();
		}
		if (!this.friendsMiracleEnableListMap.ContainsKey(SortFilterDefine.FriendsMiracleFileterCategory.Target))
		{
			return new List<DataManagerChara.FilterData>();
		}
		List<DataManagerChara.FilterData> list = DataManager.DmChara.GetFriendFilterDataList(true).FindAll((DataManagerChara.FilterData data) => data.Category == SortFilterDefine.CharacteristicFilterCategory.Target);
		List<DataManagerChara.FilterData> list2 = new List<DataManagerChara.FilterData>();
		for (int i = 0; i < this.winGUI.frameMiracle.targetButtonList.Count; i++)
		{
			if (this.winGUI.frameMiracle.targetButtonList[i].GetToggleIndex() == 1)
			{
				list2.Add(list[i]);
			}
		}
		return list2;
	}

	public List<DataManagerChara.FilterData> GetMiracleEffectDataList()
	{
		if (this.friendsMiracleEnableListMap == null)
		{
			return new List<DataManagerChara.FilterData>();
		}
		if (!this.friendsMiracleEnableListMap.ContainsKey(SortFilterDefine.FriendsMiracleFileterCategory.Effect))
		{
			return new List<DataManagerChara.FilterData>();
		}
		List<DataManagerChara.FilterData> list = DataManager.DmChara.GetFriendFilterDataList(true).FindAll((DataManagerChara.FilterData data) => data.Category == SortFilterDefine.CharacteristicFilterCategory.Effect);
		List<DataManagerChara.FilterData> list2 = new List<DataManagerChara.FilterData>();
		for (int i = 0; i < this.winGUI.frameMiracle.effectButtonList.Count; i++)
		{
			if (this.winGUI.frameMiracle.effectButtonList[i].GetToggleIndex() == 1)
			{
				list2.Add(list[i]);
			}
		}
		return list2;
	}

	public List<DataManagerChara.FilterData> GetCharacteristicConditionsDataList()
	{
		if (this.friendsCharacteristicEnableListMap == null)
		{
			return new List<DataManagerChara.FilterData>();
		}
		if (!this.friendsCharacteristicEnableListMap.ContainsKey(SortFilterDefine.FriendsCharacteristicFilterCategory.Conditions))
		{
			return new List<DataManagerChara.FilterData>();
		}
		List<DataManagerChara.FilterData> list = DataManager.DmChara.GetFriendFilterDataList(false).FindAll((DataManagerChara.FilterData data) => data.Category == SortFilterDefine.CharacteristicFilterCategory.Conditions);
		List<DataManagerChara.FilterData> list2 = new List<DataManagerChara.FilterData>();
		for (int i = 0; i < this.winGUI.frameCharacteristic.conditionsButtonList.Count; i++)
		{
			if (this.winGUI.frameCharacteristic.conditionsButtonList[i].GetToggleIndex() == 1)
			{
				list2.Add(list[i]);
			}
		}
		return list2;
	}

	public List<DataManagerChara.FilterData> GetCharacteristicTargetDataList()
	{
		if (this.friendsCharacteristicEnableListMap == null)
		{
			return new List<DataManagerChara.FilterData>();
		}
		if (!this.friendsCharacteristicEnableListMap.ContainsKey(SortFilterDefine.FriendsCharacteristicFilterCategory.Target))
		{
			return new List<DataManagerChara.FilterData>();
		}
		List<DataManagerChara.FilterData> list = DataManager.DmChara.GetFriendFilterDataList(false).FindAll((DataManagerChara.FilterData data) => data.Category == SortFilterDefine.CharacteristicFilterCategory.Target);
		List<DataManagerChara.FilterData> list2 = new List<DataManagerChara.FilterData>();
		for (int i = 0; i < this.winGUI.frameCharacteristic.targetButtonList.Count; i++)
		{
			if (this.winGUI.frameCharacteristic.targetButtonList[i].GetToggleIndex() == 1)
			{
				list2.Add(list[i]);
			}
		}
		return list2;
	}

	public List<DataManagerChara.FilterData> GetCharacteristicEffectDataList()
	{
		if (this.friendsCharacteristicEnableListMap == null)
		{
			return new List<DataManagerChara.FilterData>();
		}
		if (!this.friendsCharacteristicEnableListMap.ContainsKey(SortFilterDefine.FriendsCharacteristicFilterCategory.Effect))
		{
			return new List<DataManagerChara.FilterData>();
		}
		List<DataManagerChara.FilterData> list = DataManager.DmChara.GetFriendFilterDataList(false).FindAll((DataManagerChara.FilterData data) => data.Category == SortFilterDefine.CharacteristicFilterCategory.Effect);
		List<DataManagerChara.FilterData> list2 = new List<DataManagerChara.FilterData>();
		for (int i = 0; i < this.winGUI.frameCharacteristic.effectButtonList.Count; i++)
		{
			if (this.winGUI.frameCharacteristic.effectButtonList[i].GetToggleIndex() == 1)
			{
				list2.Add(list[i]);
			}
		}
		return list2;
	}

	public List<DataManagerChara.FilterData> GetCharacteristicResistDataList()
	{
		if (this.friendsCharacteristicEnableListMap == null)
		{
			return new List<DataManagerChara.FilterData>();
		}
		if (!this.friendsCharacteristicEnableListMap.ContainsKey(SortFilterDefine.FriendsCharacteristicFilterCategory.Resist))
		{
			return new List<DataManagerChara.FilterData>();
		}
		List<DataManagerChara.FilterData> list = DataManager.DmChara.GetFriendFilterDataList(false).FindAll((DataManagerChara.FilterData data) => data.Category == SortFilterDefine.CharacteristicFilterCategory.Abnormal);
		List<DataManagerChara.FilterData> list2 = new List<DataManagerChara.FilterData>();
		for (int i = 0; i < this.winGUI.frameCharacteristic.resistButtonList.Count; i++)
		{
			if (this.winGUI.frameCharacteristic.resistButtonList[i].GetToggleIndex() == 1)
			{
				list2.Add(list[i]);
			}
		}
		return list2;
	}

	public SortFilterDefine.AndOrState GetCharacteristicResistAndOrStatus()
	{
		if (this.winGUI.frameMiracle == null)
		{
			return SortFilterDefine.AndOrState.Invalid;
		}
		int toggleIndex = this.winGUI.frameCharacteristicResistAndBtn.GetToggleIndex();
		int toggleIndex2 = this.winGUI.frameCharacteristicResistOrBtn.GetToggleIndex();
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

	private void OnClickReset(PguiButtonCtrl clickBtn)
	{
		FriendsFilterWindowCtrl.<OnClickReset>g__ResetButton|57_0(this.winGUI.frameType.buttonList);
		FriendsFilterWindowCtrl.<OnClickReset>g__ResetButton|57_0(this.winGUI.frameFriends.buttonList);
		FriendsFilterWindowCtrl.<OnClickReset>g__ResetButton|57_0(this.winGUI.frameFavorite.buttonList);
		FriendsFilterWindowCtrl.<OnClickReset>g__ResetButton|57_0(this.winGUI.frameEvent.buttonList);
		this.windowTextSearchChange.InputField.text = "";
		if (this.winGUI.frameCharacteristic != null)
		{
			FriendsFilterWindowCtrl.<OnClickReset>g__ResetButton|57_0(this.winGUI.frameCharacteristic.buttonList);
			FriendsFilterWindowCtrl.<OnClickReset>g__ResetButton|57_0(this.winGUI.frameMiracle.buttonList);
			this.winGUI.frameMiracleEffectOrBtn.SetToggleIndex(1);
			this.winGUI.frameMiracleEffectAndBtn.SetToggleIndex(0);
			this.winGUI.frameCharacteristicEffectOrBtn.SetToggleIndex(1);
			this.winGUI.frameCharacteristicEffectAndBtn.SetToggleIndex(0);
			this.friendsMiracleEnableListMap = new Dictionary<SortFilterDefine.FriendsMiracleFileterCategory, List<DataManagerChara.FilterData>>();
			this.friendsMiracleEnableListMap.Add(SortFilterDefine.FriendsMiracleFileterCategory.Target, new List<DataManagerChara.FilterData>());
			this.friendsMiracleEnableListMap.Add(SortFilterDefine.FriendsMiracleFileterCategory.Effect, new List<DataManagerChara.FilterData>());
			this.friendsCharacteristicEnableListMap = new Dictionary<SortFilterDefine.FriendsCharacteristicFilterCategory, List<DataManagerChara.FilterData>>();
			this.friendsCharacteristicEnableListMap.Add(SortFilterDefine.FriendsCharacteristicFilterCategory.Conditions, new List<DataManagerChara.FilterData>());
			this.friendsCharacteristicEnableListMap.Add(SortFilterDefine.FriendsCharacteristicFilterCategory.Target, new List<DataManagerChara.FilterData>());
			this.friendsCharacteristicEnableListMap.Add(SortFilterDefine.FriendsCharacteristicFilterCategory.Effect, new List<DataManagerChara.FilterData>());
			this.friendsCharacteristicEnableListMap.Add(SortFilterDefine.FriendsCharacteristicFilterCategory.Resist, new List<DataManagerChara.FilterData>());
		}
	}

	public List<int> getMiracleTargetButtonIdx()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.winGUI.frameMiracle.targetButtonList.Count; i++)
		{
			if (this.winGUI.frameMiracle.targetButtonList[i].GetToggleIndex() == 1)
			{
				list.Add(i);
			}
		}
		return list;
	}

	public List<int> getMiracleEffectButtonIdx()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.winGUI.frameMiracle.effectButtonList.Count; i++)
		{
			if (this.winGUI.frameMiracle.effectButtonList[i].GetToggleIndex() == 1)
			{
				list.Add(i);
			}
		}
		return list;
	}

	public List<int> getCharacterisiticConditionsButtonIdx()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.winGUI.frameCharacteristic.conditionsButtonList.Count; i++)
		{
			if (this.winGUI.frameCharacteristic.conditionsButtonList[i].GetToggleIndex() == 1)
			{
				list.Add(i);
			}
		}
		return list;
	}

	public List<int> getCharacterisiticTargetButtonIdx()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.winGUI.frameCharacteristic.targetButtonList.Count; i++)
		{
			if (this.winGUI.frameCharacteristic.targetButtonList[i].GetToggleIndex() == 1)
			{
				list.Add(i);
			}
		}
		return list;
	}

	public List<int> getCharacterisiticEffectButtonIdx()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.winGUI.frameCharacteristic.effectButtonList.Count; i++)
		{
			if (this.winGUI.frameCharacteristic.effectButtonList[i].GetToggleIndex() == 1)
			{
				list.Add(i);
			}
		}
		return list;
	}

	public List<int> getCharacterisiticResistButtonIdx()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.winGUI.frameCharacteristic.resistButtonList.Count; i++)
		{
			if (this.winGUI.frameCharacteristic.resistButtonList[i].GetToggleIndex() == 1)
			{
				list.Add(i);
			}
		}
		return list;
	}

	public void ResetSearchText()
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

	[CompilerGenerated]
	internal static void <OnClickReset>g__ResetButton|57_0(List<PguiToggleButtonCtrl> btnList)
	{
		if (btnList == null)
		{
			return;
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in btnList)
		{
			pguiToggleButtonCtrl.SetToggleIndex(0);
		}
	}

	private FriendsFilterWindowCtrl.WindowGUI winGUI;

	private Dictionary<SortFilterDefine.FriendsCharacteristicFilterCategory, List<DataManagerChara.FilterData>> friendsCharacteristicEnableListMap;

	private Dictionary<SortFilterDefine.FriendsMiracleFileterCategory, List<DataManagerChara.FilterData>> friendsMiracleEnableListMap;

	private FriendsFilterWindowCtrl.WindowTextSearchChange windowTextSearchChange;

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

		public FriendsFilterWindowCtrl.FrameGUI frameType;

		public FriendsFilterWindowCtrl.FrameGUI frameFriends;

		public FriendsFilterWindowCtrl.FrameGUI frameFavorite;

		public FriendsFilterWindowCtrl.FrameGUI frameEvent;

		public FriendsFilterWindowCtrl.FrameGUI frameLevel;

		public FriendsFilterWindowCtrl.BuffFrameGUI frameMiracle;

		public FriendsFilterWindowCtrl.BuffFrameGUI frameCharacteristic;

		public PguiToggleButtonCtrl frameMiracleEffectOrBtn;

		public PguiToggleButtonCtrl frameMiracleEffectAndBtn;

		public PguiToggleButtonCtrl frameCharacteristicEffectOrBtn;

		public PguiToggleButtonCtrl frameCharacteristicEffectAndBtn;

		public PguiToggleButtonCtrl frameCharacteristicResistAndBtn;

		public PguiToggleButtonCtrl frameCharacteristicResistOrBtn;

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
		public BuffFrameGUI(GameObject go, List<PguiToggleButtonCtrl> conditionList, List<PguiToggleButtonCtrl> targetList, List<PguiToggleButtonCtrl> effectList, List<PguiToggleButtonCtrl> resistList)
		{
			this.baseObj = go;
			this.baseObj.transform.Find("Base/Title/RadioBtns").gameObject.SetActive(false);
			this.conditionsButtonList = conditionList;
			this.targetButtonList = targetList;
			this.effectButtonList = effectList;
			this.resistButtonList = resistList;
		}

		public List<PguiToggleButtonCtrl> buttonList
		{
			get
			{
				List<PguiToggleButtonCtrl> list = new List<PguiToggleButtonCtrl>();
				if (this.conditionsButtonList != null)
				{
					list.AddRange(this.conditionsButtonList);
				}
				if (this.targetButtonList != null)
				{
					list.AddRange(this.targetButtonList);
				}
				if (this.effectButtonList != null)
				{
					list.AddRange(this.effectButtonList);
				}
				if (this.resistButtonList != null)
				{
					list.AddRange(this.resistButtonList);
				}
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
