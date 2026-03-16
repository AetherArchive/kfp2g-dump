using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ExtendedEnum;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

public class SelMissionCtrl : MonoBehaviour
{
	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneMission/GUI/Prefab/GUI_Mission"), base.transform);
		this.missionGuiData = new SelMissionCtrl.MissionGUI(gameObject.transform);
		int num = 0;
		foreach (SelMissionCtrl.MissionGUI.MissionTab missionTab in this.missionGuiData.MissionTabBtnList)
		{
			int j = num;
			missionTab.TabBtn.AddOnClickListener((PguiToggleButtonCtrl btn, int b) => this.OnClickMissionTabButton(btn, j, true));
			num++;
		}
		num = 0;
		foreach (SelMissionCtrl.MissionGUI.MissionButton missionButton in this.missionGuiData.MissionBtnList)
		{
			int i = num;
			missionButton.Button.AddOnClickListener((PguiToggleButtonCtrl btn, int b) => this.OnClickMissionButton(btn, i, true));
			num++;
		}
		this.missionGuiData.Btn_AllGet.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickAllGetButton), PguiButtonCtrl.SoundType.DEFAULT);
		foreach (SelMissionCtrl.MissionGUI.Info info in this.missionGuiData.InfoMap.Values)
		{
			info.Scroll.onStartItem = new Action<int, GameObject>(this.OnStartMission);
			info.Scroll.onUpdateItem = new Action<int, GameObject>(this.OnUpdateMission);
			info.Scroll.ReuseItemNum = 6;
			info.Scroll.Setup(0, 0);
		}
		GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneMission/GUI/Prefab/GUI_Mission_Window"), Singleton<CanvasManager>.Instance.OverlayWindowPanel);
		this.guiSpecialWindow = new SelMissionCtrl.GuiMissionSpecialWindow(gameObject2.transform);
		this.guiSpecialWindow.baseObj.AddComponent<SafeAreaScaler>();
	}

	public void Setup(object args = null, UnityAction<SceneManager.SceneName, object> transitionAction = null)
	{
		this.transitionAction = transitionAction;
		this.UpdateMissionData();
		this.GettingMission = false;
		int num = 0;
		int num2 = 0;
		if (args != null)
		{
			SceneMission.MissionOpenParam missionOpenParam = (SceneMission.MissionOpenParam)args;
			switch (missionOpenParam.missionType)
			{
			case MissionType.DAILY:
				num = 0;
				num2 = 0;
				goto IL_0145;
			case MissionType.WEEKLY:
				num = 0;
				num2 = 1;
				goto IL_0145;
			case MissionType.TOTAL:
				num = 0;
				num2 = 2;
				goto IL_0145;
			case MissionType.EVENTTOTAL:
			case MissionType.EVENTDAILY:
				break;
			case MissionType.SPECIAL:
				goto IL_0145;
			case MissionType.BEGINNER:
			{
				using (List<List<UserMissionGroup>>.Enumerator enumerator = this.missionGroupDataListList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UserMissionGroup userMissionGroup = enumerator.Current.Find((UserMissionGroup x) => MissionType.BEGINNER == x.type);
						if (userMissionGroup != null)
						{
							num = 0;
							num2 = ((0 < userMissionGroup.viewDataList.Count) ? 3 : 0);
						}
					}
					goto IL_0145;
				}
				break;
			}
			default:
				goto IL_0145;
			}
			if (missionOpenParam != null && missionOpenParam.eventId != 0)
			{
				for (int i = 0; i < this.missionGroupDataListList.Count; i++)
				{
					List<UserMissionGroup> list = this.missionGroupDataListList[i];
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].eventId == missionOpenParam.eventId)
						{
							num = i;
							num2 = j;
						}
					}
				}
			}
		}
		IL_0145:
		this.missionGuiData.SelectBtnIndex = 0;
		this.missionGuiData.DispMissionBtnIndex = 0;
		this.SetMissionTabButtonSilent(num, num2);
		foreach (SelMissionCtrl.MissionGUI.Info info in this.missionGuiData.InfoMap.Values)
		{
			info.Scroll.Refresh();
		}
		this.missionGuiData.InfoMap[SelMissionCtrl.MissionGUI.InfoType.Normal].Scroll.ForceFocus(0);
		this.missionGuiData.InfoMap[SelMissionCtrl.MissionGUI.InfoType.Event].Scroll.ForceFocus(0);
	}

	private void Update()
	{
		if (this.RequestMissionReward != null && !this.RequestMissionReward.MoveNext())
		{
			this.RequestMissionReward = null;
		}
	}

	private void SetMissionTabButtonSilent(int tabIdx, int btnIdx)
	{
		this.missionGuiData.SelectTabIndex = -1;
		this.OnClickMissionTabButton(this.missionGuiData.MissionTabBtnList[tabIdx].TabBtn, tabIdx, false);
		this.missionGuiData.DispMissionBtnIndex = -1;
		this.OnClickMissionButton(this.missionGuiData.MissionBtnList[btnIdx].Button, btnIdx, false);
	}

	private void UpdateMissionData()
	{
		this.missionGroupDataListList = new List<List<UserMissionGroup>>();
		List<UserMissionGroup> list = new List<UserMissionGroup>();
		foreach (UserMissionGroup userMissionGroup in DataManager.DmMission.GetUserMissionGroupList())
		{
			switch (userMissionGroup.type)
			{
			case MissionType.INVALID:
			case MissionType.EVENTTOTAL:
			case MissionType.SPECIAL:
			case MissionType.EVENTDAILY:
				continue;
			case MissionType.BEGINNER:
				if (userMissionGroup.viewDataList.Count == 0)
				{
					continue;
				}
				break;
			}
			list.Add(userMissionGroup);
		}
		List<UserMissionGroup> list2 = new List<UserMissionGroup>();
		List<int> validEventIdListWithoutMissionEvent = DataManager.DmEvent.GetValidEventIdListWithoutMissionEvent();
		validEventIdListWithoutMissionEvent.AddRange(DataManager.DmEvent.GetValidMissionEventIdList(false));
		validEventIdListWithoutMissionEvent.Sort((int a, int b) => a - b);
		foreach (int num in validEventIdListWithoutMissionEvent)
		{
			if (DataManager.DmEvent.GetEventData(num).eventMissionGroupId != 0)
			{
				list2.Add(DataManager.DmMission.GetEventMissionGroup(num));
			}
		}
		if (this.missionGuiData.MissionBtnList.Count < list.Count + list2.Count)
		{
			this.missionGroupDataListList.Add(list);
			this.missionGroupDataListList.Add(list2);
			return;
		}
		foreach (UserMissionGroup userMissionGroup2 in list2)
		{
			list.Add(userMissionGroup2);
		}
		this.missionGroupDataListList.Add(list);
	}

	private IEnumerator MissionDataUpdate(string windowText, long beforeBankGold, bool isOne)
	{
		this.GettingMission = true;
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		UnityAction specialMissionResult = null;
		Dictionary<int, int> bonusResultItemMap = DataManager.DmMission.MissionBonusResultItemMap;
		bool missionRewardResult = DataManager.DmMission.CompareResultItemAllReceived();
		List<ItemData> dispItemList = new List<ItemData>();
		bool includeSpecial = false;
		if (SelMissionCtrl.RequestType.Special == this.LastRequestType || SelMissionCtrl.RequestType.All == this.LastRequestType)
		{
			List<ItemData> itemList = new List<ItemData>();
			includeSpecial = true;
			if (SelMissionCtrl.RequestType.All == this.LastRequestType)
			{
				DataManager.DmMission.RequestActionMissionRewardAll(this.missionGroupDataListList[this.missionGuiData.DispMissionTabIndex][this.missionGuiData.DispMissionBtnIndex].type, true, isOne);
			}
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			List<GachaResult> missionBonusSpecialResult = DataManager.DmMission.MissionBonusSpecialResult;
			if (0 < DataManager.DmMission.MissionBonusSpecialResult.Count)
			{
				if (isOne)
				{
					using (List<GachaResult>.Enumerator enumerator = DataManager.DmMission.MultipleMissionBonusSpecialResult.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GachaResult gachaResult = enumerator.Current;
							if (SelMissionCtrl.SKIP_TICKET_ID != gachaResult.item_id)
							{
								specialMissionResult = delegate
								{
									this.guiSpecialWindow.StartSpecialMissionResult();
								};
							}
							else
							{
								ItemData itemData = new ItemData(gachaResult.item_id, gachaResult.item_num);
								itemList.Add(itemData);
							}
						}
						goto IL_02C0;
					}
				}
				if (1 < DataManager.DmMission.MultipleMissionBonusSpecialResult.Count)
				{
					using (List<GachaResult>.Enumerator enumerator = DataManager.DmMission.MultipleMissionBonusSpecialResult.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GachaResult gachaResult2 = enumerator.Current;
							if (SelMissionCtrl.SKIP_TICKET_ID != gachaResult2.item_id)
							{
								specialMissionResult = delegate
								{
									this.guiSpecialWindow.StartSpecialMissionResult();
								};
							}
							else
							{
								ItemData itemData2 = new ItemData(gachaResult2.item_id, gachaResult2.item_num);
								itemList.Add(itemData2);
							}
						}
						goto IL_02C0;
					}
				}
				foreach (GachaResult gachaResult3 in DataManager.DmMission.MissionBonusSpecialResult)
				{
					if (SelMissionCtrl.SKIP_TICKET_ID != gachaResult3.item_id)
					{
						specialMissionResult = delegate
						{
							this.guiSpecialWindow.StartSpecialMissionResult();
						};
					}
					else
					{
						ItemData itemData3 = new ItemData(gachaResult3.item_id, gachaResult3.item_num);
						itemList.Add(itemData3);
					}
				}
				IL_02C0:
				this.AddItemDataToList(dispItemList, itemList);
			}
			if (includeSpecial && itemList.Count > 0)
			{
				this.ShowItemResultWindow(dispItemList, itemList, windowText, specialMissionResult);
			}
			itemList = null;
		}
		long content_num = DataManager.DmItem.UserItemBankMap.TryGetValueEx(30090, new ItemBank()).content_num;
		bool flag = beforeBankGold < content_num;
		if (SelMissionCtrl.RequestType.One == this.LastRequestType || SelMissionCtrl.RequestType.All == this.LastRequestType || SelMissionCtrl.RequestType.Event == this.LastRequestType)
		{
			SoundManager.Play("prd_se_selector_receive", false, false);
			List<ItemData> list = new List<ItemData>();
			foreach (KeyValuePair<int, int> keyValuePair in bonusResultItemMap)
			{
				list.Add(new ItemData(keyValuePair.Key, keyValuePair.Value));
			}
			this.AddItemDataToList(dispItemList, list);
			if (0 < dispItemList.Count)
			{
				if (!missionRewardResult)
				{
					windowText += "\n※所持数上限を超えた報酬はプレゼントボックスに移動しました";
				}
				if (flag)
				{
					windowText = string.Concat(new string[]
					{
						windowText,
						"\n※所持数上限を超えた",
						DataManager.DmItem.GetItemStaticBase(30101).GetName(),
						"は",
						DataManager.DmItem.GetItemStaticBase(30090).GetName(),
						"に補充しました"
					});
				}
				if (DataManager.DmMission.isAchievementInRewardItemMap)
				{
					windowText += "\n※称号は一括受け取りの対象外です";
				}
				if (DataManager.DmMission.GetLastRequestAchievementId() != -1)
				{
					this.OpenWindowAchievement();
				}
				else
				{
					this.ShowItemResultWindow(dispItemList, list, windowText, specialMissionResult);
				}
			}
			else if (!missionRewardResult)
			{
				if (DataManager.DmMission.GetLastRequestAchievementId() != -1)
				{
					this.OpenWindowAchievement();
				}
				else if (DataManager.DmMission.isAchievementInRewardItemMap)
				{
					if (includeSpecial)
					{
						this.GettingMission = false;
						UnityAction unityAction = specialMissionResult;
						if (unityAction != null)
						{
							unityAction();
						}
					}
				}
				else
				{
					windowText = "所持数上限を超えていたため\n報酬をプレゼントボックスに移動しました";
					this.ShowItemResultWindow(new List<ItemData>(), new List<ItemData>(), windowText, specialMissionResult);
				}
			}
			else if (includeSpecial)
			{
				this.GettingMission = false;
				UnityAction unityAction2 = specialMissionResult;
				if (unityAction2 != null)
				{
					unityAction2();
				}
			}
		}
		else
		{
			this.GettingMission = false;
			UnityAction unityAction3 = specialMissionResult;
			if (unityAction3 != null)
			{
				unityAction3();
			}
		}
		this.UpdateMissionData();
		bool flag2 = true;
		if (this.missionGroupDataListList.Count > this.missionGuiData.DispMissionTabIndex)
		{
			List<UserMissionGroup> list2 = this.missionGroupDataListList[this.missionGuiData.DispMissionTabIndex];
			if (list2.Count > this.missionGuiData.DispMissionBtnIndex)
			{
				UserMissionGroup userMissionGroup = list2[this.missionGuiData.DispMissionBtnIndex];
				MissionType type = userMissionGroup.type;
				int num = ((MissionType.EVENTDAILY == userMissionGroup.type || MissionType.EVENTDAILY == userMissionGroup.type) ? userMissionGroup.eventId : 0);
				if (this.missionGuiData.DispMissionType == type && this.missionGuiData.DispMissionId == num)
				{
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			this.SetMissionTabButtonSilent(0, 0);
		}
		this.UpdateDispMissionInfo();
		yield break;
	}

	private void AddItemDataToList(List<ItemData> dispItemList, List<ItemData> itemList)
	{
		dispItemList.AddRange(SelMissionCtrl.<AddItemDataToList>g__ItemDataList|17_0(DataManager.DmItem.GetUserDispItemList(itemList, DataManagerItem.DispType.Common)));
		dispItemList.AddRange(SelMissionCtrl.<AddItemDataToList>g__ItemDataList|17_0(DataManager.DmItem.GetUserDispItemList(itemList, DataManagerItem.DispType.Growth)));
		dispItemList.AddRange(SelMissionCtrl.<AddItemDataToList>g__ItemDataList|17_0(DataManager.DmItem.GetUserDispItemList(itemList, DataManagerItem.DispType.Photo)));
		dispItemList.AddRange(SelMissionCtrl.<AddItemDataToList>g__ItemDataList|17_0(DataManager.DmItem.GetUserDispItemList(itemList, DataManagerItem.DispType.Decoration)));
		dispItemList.AddRange(SelMissionCtrl.<AddItemDataToList>g__ItemDataList|17_0(DataManager.DmItem.GetUserDispItemList(itemList, DataManagerItem.DispType.Accessory)));
		dispItemList.AddRange(SelMissionCtrl.<AddItemDataToList>g__ItemDataList|17_0(DataManager.DmItem.GetUserDispItemList(itemList, DataManagerItem.DispType.Achievement)));
	}

	private void ShowItemResultWindow(List<ItemData> dispItemList, List<ItemData> itemList, string windowText, UnityAction specialMissionResult = null)
	{
		GetMultiItemWindowCtrl.SetupParam setupParam = new GetMultiItemWindowCtrl.SetupParam
		{
			titleText = "確認",
			messageText = windowText,
			innerTitleText = "入手したアイテム",
			callBack = delegate(int x)
			{
				this.GettingMission = false;
				UnityAction specialMissionResult2 = specialMissionResult;
				if (specialMissionResult2 != null)
				{
					specialMissionResult2();
				}
				return true;
			}
		};
		CanvasManager.HdlGetItemSetWindowCtrl.Setup(dispItemList, setupParam, false, 0);
		CanvasManager.HdlGetItemSetWindowCtrl.Open();
	}

	private void OpenWindowAchievement()
	{
		int lastRequestAchievementId = DataManager.DmMission.GetLastRequestAchievementId();
		if (lastRequestAchievementId == -1)
		{
			return;
		}
		DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(lastRequestAchievementId);
		ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(achievementData.duplicateItemId);
		string text = string.Format("{0}はすでに所持していたため\n{1}×{2}に変換されました", achievementData.name, itemStaticBase.GetName(), achievementData.duplicateItemNum);
		if (!DataManager.DmMission.CompareResultItemAllReceived())
		{
			text += "\n※所持数上限を超えた報酬はプレゼントボックスに移動しました";
		}
		GetMultiItemWindowCtrl.SetupParam setupParam = new GetMultiItemWindowCtrl.SetupParam
		{
			titleText = "確認",
			messageText = text,
			innerTitleText = "入手したアイテム",
			callBack = delegate(int x)
			{
				this.GettingMission = false;
				return true;
			}
		};
		List<ItemData> list = new List<ItemData>();
		ItemData itemData = new ItemData(achievementData.duplicateItemId, achievementData.duplicateItemNum);
		list.Add(itemData);
		CanvasManager.HdlGetItemSetWindowCtrl.Setup(list, setupParam, false, 0);
		CanvasManager.HdlGetItemSetWindowCtrl.Open();
	}

	private void OnClickGetButton(int index)
	{
		if (this.GettingMission)
		{
			return;
		}
		if (this.RequestMissionReward != null)
		{
			return;
		}
		UserMissionGroup userMissionGroup = this.missionGroupDataListList[this.missionGuiData.DispMissionTabIndex][this.missionGuiData.DispMissionBtnIndex];
		UserMissionOne userMissionOne = userMissionGroup.viewDataList[index];
		SceneManager.SceneName sceneName = ((userMissionOne.transitionScene == SceneManager.SceneName.None) ? this.GetDefaultDestination(userMissionOne.relType) : userMissionOne.transitionScene);
		bool flag = false;
		if (userMissionOne.isClear || this.IsInvalidScene(sceneName))
		{
			string text = "「{missionName}」の\n報酬を受け取りました";
			text = text.Replace("{missionName}", userMissionGroup.tabName);
			long content_num = DataManager.DmItem.UserItemBankMap.TryGetValueEx(30090, new ItemBank()).content_num;
			if (!userMissionGroup.viewDataList[index].IsSpecial)
			{
				this.LastRequestType = SelMissionCtrl.RequestType.One;
				DataManager.DmMission.RequestActionMissionRewardOne(userMissionGroup.viewDataList[index]);
			}
			else
			{
				flag = true;
				if (0 < DataManager.DmMission.GetUserClearSpecialMissionNum(this.missionGroupDataListList[this.missionGuiData.DispMissionTabIndex][this.missionGuiData.DispMissionBtnIndex].type))
				{
					this.LastRequestType = SelMissionCtrl.RequestType.Special;
					DataManager.DmMission.RequestActionSpecialMissionRewardOne(userMissionGroup.viewDataList[index]);
					text = "「スペシャルデイリー」の\n報酬を受け取りました";
				}
			}
			this.RequestMissionReward = this.MissionDataUpdate(text, content_num, flag);
			return;
		}
		object nextSceneArgs = Singleton<SceneManager>.Instance.GetNextSceneArgs(sceneName, userMissionOne.transitionId);
		QuestOnePackData reason = null;
		if (this.IsLockedScene(sceneName, nextSceneArgs, delegate(QuestOnePackData action)
		{
			reason = action;
		}))
		{
			this.ShowReleaseCondition(sceneName, reason);
			return;
		}
		UnityAction<SceneManager.SceneName, object> unityAction = this.transitionAction;
		if (unityAction == null)
		{
			return;
		}
		unityAction(sceneName, nextSceneArgs);
	}

	private bool OnClickMissionTabButton(PguiToggleButtonCtrl toggleBtn, int index, bool playSE)
	{
		if (this.missionGuiData.SelectTabIndex == index)
		{
			return false;
		}
		if (1 < this.missionGroupDataListList.Count)
		{
			foreach (SelMissionCtrl.MissionGUI.MissionTab missionTab in this.missionGuiData.MissionTabBtnList)
			{
				missionTab.TabBtn.gameObject.SetActive(true);
				missionTab.TabBtn.SetToggleIndex(1);
			}
			toggleBtn.SetToggleIndex(0);
		}
		else
		{
			foreach (SelMissionCtrl.MissionGUI.MissionTab missionTab2 in this.missionGuiData.MissionTabBtnList)
			{
				missionTab2.TabBtn.gameObject.SetActive(false);
			}
		}
		this.missionGuiData.SelectTabIndex = index;
		int num = 0;
		foreach (SelMissionCtrl.MissionGUI.MissionButton missionButton in this.missionGuiData.MissionBtnList)
		{
			List<UserMissionGroup> list = this.missionGroupDataListList[this.missionGuiData.SelectTabIndex];
			if (list.Count <= num)
			{
				missionButton.BaseObj.SetActive(false);
			}
			else
			{
				missionButton.BaseObj.SetActive(true);
				missionButton.MissionName.text = list[num].tabName;
				missionButton.UpdateBadge(list[num].viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received).Count);
				bool flag = num == this.missionGuiData.DispMissionBtnIndex && this.missionGuiData.SelectTabIndex == this.missionGuiData.DispMissionTabIndex;
				switch (list[num].type)
				{
				case MissionType.DAILY:
				case MissionType.WEEKLY:
				case MissionType.TOTAL:
					missionButton.Button.SetToggleIndex(flag ? 0 : 1);
					break;
				case MissionType.EVENTTOTAL:
				case MissionType.BEGINNER:
				case MissionType.EVENTDAILY:
					missionButton.Button.SetToggleIndex(flag ? 2 : 3);
					break;
				}
			}
			num++;
		}
		if (playSE)
		{
			SoundManager.Play("prd_se_click", false, false);
		}
		return false;
	}

	private bool OnClickMissionButton(PguiToggleButtonCtrl toggleBtn, int index, bool playSE)
	{
		int selectTabIndex = this.missionGuiData.SelectTabIndex;
		int count = this.missionGuiData.MissionBtnList.Count;
		int num = 0;
		foreach (SelMissionCtrl.MissionGUI.MissionButton missionButton in this.missionGuiData.MissionBtnList)
		{
			int selectTabIndex2 = this.missionGuiData.SelectTabIndex;
			int count2 = this.missionGuiData.MissionBtnList.Count;
			List<UserMissionGroup> list = this.missionGroupDataListList[this.missionGuiData.SelectTabIndex];
			if (list.Count <= num)
			{
				break;
			}
			switch (list[num].type)
			{
			case MissionType.INVALID:
			case MissionType.SPECIAL:
				goto IL_00CF;
			case MissionType.DAILY:
			case MissionType.WEEKLY:
			case MissionType.TOTAL:
				missionButton.Button.SetToggleIndex(1);
				break;
			case MissionType.EVENTTOTAL:
			case MissionType.BEGINNER:
			case MissionType.EVENTDAILY:
				missionButton.Button.SetToggleIndex(3);
				break;
			default:
				goto IL_00CF;
			}
			IL_00DB:
			num++;
			continue;
			IL_00CF:
			missionButton.Button.SetToggleIndex(1);
			goto IL_00DB;
		}
		toggleBtn.SetToggleIndex(toggleBtn.GetToggleIndex() - 1);
		this.missionGuiData.SelectBtnIndex = index;
		if (this.missionGuiData.SelectBtnIndex != this.missionGuiData.DispMissionBtnIndex || this.missionGuiData.SelectTabIndex != this.missionGuiData.DispMissionTabIndex)
		{
			this.missionGuiData.DispMissionBtnIndex = this.missionGuiData.SelectBtnIndex;
			this.missionGuiData.DispMissionTabIndex = this.missionGuiData.SelectTabIndex;
			this.UpdateDispMissionInfo();
		}
		if (playSE)
		{
			SoundManager.Play("prd_se_click", false, false);
		}
		return false;
	}

	private void OnClickAllGetButton(PguiButtonCtrl button)
	{
		if (this.GettingMission)
		{
			return;
		}
		if (this.RequestMissionReward != null)
		{
			return;
		}
		string text = "「{missionName}」の\n報酬を受け取りました";
		long content_num = DataManager.DmItem.UserItemBankMap.TryGetValueEx(30090, new ItemBank()).content_num;
		bool flag = false;
		UserMissionGroup userMissionGroup = this.missionGroupDataListList[this.missionGuiData.DispMissionTabIndex][this.missionGuiData.DispMissionBtnIndex];
		if (userMissionGroup.type == MissionType.EVENTTOTAL || userMissionGroup.type == MissionType.EVENTDAILY)
		{
			int count = userMissionGroup.viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received).Count;
			this.LastRequestType = SelMissionCtrl.RequestType.Event;
			if (0 < count)
			{
				DataManager.DmMission.RequestActionMissionRewardEvent(userMissionGroup.eventId);
				text = text.Replace("{missionName}", userMissionGroup.tabName);
			}
			else
			{
				text = string.Empty;
			}
		}
		else
		{
			this.LastRequestType = SelMissionCtrl.RequestType.All;
			if (0 < DataManager.DmMission.GetUserClearMissionNum(userMissionGroup.type, false))
			{
				DataManager.DmMission.RequestActionMissionRewardAll(userMissionGroup.type, false, false);
				text = text.Replace("{missionName}", userMissionGroup.tabName);
			}
			else
			{
				DataManager.DmMission.DummyRequestClearMissionResultItem();
				text = string.Empty;
				if (0 < DataManager.DmMission.GetUserClearSpecialMissionNum(this.missionGroupDataListList[this.missionGuiData.DispMissionTabIndex][this.missionGuiData.DispMissionBtnIndex].type))
				{
					text = "「スペシャルデイリー」の\n報酬を受け取りました";
				}
			}
		}
		this.RequestMissionReward = this.MissionDataUpdate(text, content_num, flag);
	}

	private void UpdateTabBadgeCount()
	{
		int num = 0;
		foreach (List<UserMissionGroup> list in this.missionGroupDataListList)
		{
			int num2 = 0;
			foreach (UserMissionGroup userMissionGroup in list)
			{
				num2 += userMissionGroup.viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received).Count;
			}
			this.missionGuiData.MissionTabBtnList[num].UpdateBadge(num2);
			num++;
		}
	}

	private void UpdateDispMissionInfo()
	{
		this.UpdateTabBadgeCount();
		List<UserMissionGroup> list = this.missionGroupDataListList[this.missionGuiData.DispMissionTabIndex];
		if (list.Count <= this.missionGuiData.DispMissionBtnIndex)
		{
			return;
		}
		UserMissionGroup userMissionGroup = list[this.missionGuiData.DispMissionBtnIndex];
		SelMissionCtrl.MissionGUI.InfoType infoType = SelMissionCtrl.MissionGUI.InfoType.Undefined;
		switch (userMissionGroup.type)
		{
		case MissionType.DAILY:
		case MissionType.WEEKLY:
		case MissionType.TOTAL:
			this.missionGuiData.InfoMap[SelMissionCtrl.MissionGUI.InfoType.Normal].SetActive(true);
			this.missionGuiData.InfoMap[SelMissionCtrl.MissionGUI.InfoType.Event].SetActive(false);
			infoType = SelMissionCtrl.MissionGUI.InfoType.Normal;
			break;
		case MissionType.EVENTTOTAL:
		case MissionType.BEGINNER:
		case MissionType.EVENTDAILY:
			this.missionGuiData.InfoMap[SelMissionCtrl.MissionGUI.InfoType.Normal].SetActive(false);
			this.missionGuiData.InfoMap[SelMissionCtrl.MissionGUI.InfoType.Event].SetActive(true);
			infoType = SelMissionCtrl.MissionGUI.InfoType.Event;
			break;
		}
		this.missionGuiData.DispMissionType = userMissionGroup.type;
		this.missionGuiData.DispMissionId = ((MissionType.EVENTDAILY == userMissionGroup.type || MissionType.EVENTDAILY == userMissionGroup.type) ? userMissionGroup.eventId : 0);
		int count = userMissionGroup.viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received).Count;
		this.missionGuiData.MissionBtnList[this.missionGuiData.DispMissionBtnIndex].UpdateBadge(count);
		if (infoType != SelMissionCtrl.MissionGUI.InfoType.Normal)
		{
			if (infoType != SelMissionCtrl.MissionGUI.InfoType.Event)
			{
			}
		}
		else
		{
			this.missionGuiData.InfoMap[infoType].TitleText.text = userMissionGroup.tabName + "ミッション";
		}
		this.missionGuiData.InfoMap[infoType].LimitText.text = ((userMissionGroup.limitTime == null) ? string.Empty : TimeManager.MakeTimeResidueText(TimeManager.Now, userMissionGroup.limitTime.Value, false, true));
		int count2 = userMissionGroup.viewDataList.FindAll((UserMissionOne x) => !x.isClear).Count;
		int count3 = userMissionGroup.viewDataList.Count;
		if (count2 == 0)
		{
			this.missionGuiData.InfoMap[infoType].ClearCount.gameObject.SetActive(false);
			this.missionGuiData.InfoMap[infoType].MarkComplete.gameObject.SetActive(true);
		}
		else
		{
			this.missionGuiData.InfoMap[infoType].ClearCount.gameObject.SetActive(true);
			this.missionGuiData.InfoMap[infoType].MarkComplete.gameObject.SetActive(false);
			this.missionGuiData.InfoMap[infoType].ClearCount.text = "あと" + count2.ToString() + "/" + count3.ToString();
		}
		if (SelMissionCtrl.MissionGUI.InfoType.Event == infoType)
		{
			if (userMissionGroup.type == MissionType.BEGINNER)
			{
				this.missionGuiData.InfoMap[infoType].Banner.banner = "Texture2D/Mission/uDjjD9v4yxvvbt4ybmDvvbtg0xuJG2ITS";
			}
			else
			{
				this.missionGuiData.InfoMap[infoType].Banner.banner = "Texture2D/Mission/" + DataManager.DmEvent.GetEventData(userMissionGroup.eventId).MissionBannerFilename;
			}
		}
		this.missionGuiData.Txt_Caution.gameObject.SetActive(userMissionGroup.viewDataList.Count == 0);
		this.missionGuiData.InfoMap[infoType].Scroll.Resize(userMissionGroup.viewDataList.Count, 0);
		this.missionGuiData.InfoMap[infoType].Scroll.Refresh();
		bool flag = false;
		foreach (UserMissionOne userMissionOne in userMissionGroup.viewDataList)
		{
			if (userMissionOne.isClear && !userMissionOne.Received && userMissionOne.GetRewardItemData().staticData.GetKind() != ItemDef.Kind.ACHIEVEMENT)
			{
				flag = true;
				break;
			}
		}
		this.missionGuiData.Btn_AllGet.SetActEnable(flag, false, false);
	}

	private void OnStartMission(int index, GameObject go)
	{
		go.GetComponent<MissionBarCtrl>().Init();
	}

	private void OnUpdateMission(int barIndex, GameObject go)
	{
		if (-1 >= barIndex)
		{
			return;
		}
		if (this.missionGroupDataListList.Count <= this.missionGuiData.DispMissionTabIndex)
		{
			go.SetActive(false);
			return;
		}
		List<UserMissionGroup> list = this.missionGroupDataListList[this.missionGuiData.DispMissionTabIndex];
		if (list.Count <= this.missionGuiData.DispMissionBtnIndex)
		{
			go.SetActive(false);
			return;
		}
		UserMissionGroup userMissionGroup = list[this.missionGuiData.DispMissionBtnIndex];
		if (barIndex < userMissionGroup.viewDataList.Count)
		{
			go.SetActive(true);
			UserMissionOne viewData = userMissionGroup.viewDataList[barIndex];
			MissionBarCtrl component = go.GetComponent<MissionBarCtrl>();
			int needMissionId = DataManager.DmMission.StaticMissionDataList.Find((DataManagerMission.StaticMissionData x) => viewData.missionId == x.MissionId).NeedMissionId;
			UserMissionOne userMissionOne = DataManager.DmMission.UserMissionOneList.Find((UserMissionOne x) => needMissionId == x.missionId);
			SceneManager.SceneName sceneName = ((viewData.transitionScene == SceneManager.SceneName.None) ? this.GetDefaultDestination(viewData.relType) : viewData.transitionScene);
			component.SetupMission(new MissionBarCtrl.SetupParam
			{
				denominator = viewData.denominator,
				isClear = viewData.isClear,
				isSpecial = viewData.IsSpecial,
				itemData = viewData.GetRewardItemData(),
				missionContents = viewData.missionContents,
				numerator = viewData.numerator,
				onClick = delegate
				{
					this.OnClickGetButton(barIndex);
				},
				received = viewData.Received,
				gageDisp = (needMissionId == 0 || (userMissionOne != null && userMissionOne.isClear)),
				isContainTransition = !this.IsInvalidScene(sceneName)
			});
			return;
		}
		go.SetActive(false);
	}

	private bool IsInvalidScene(SceneManager.SceneName name)
	{
		switch (name)
		{
		case SceneManager.SceneName.None:
		case SceneManager.SceneName.SceneBoot:
		case SceneManager.SceneName.SceneAdvertise:
		case SceneManager.SceneName.SceneDataInitialize:
		case SceneManager.SceneName.SceneTitle:
		case SceneManager.SceneName.SceneBattle:
		case SceneManager.SceneName.SceneBattleResult:
		case SceneManager.SceneName.SceneBattleSelector:
		case SceneManager.SceneName.SceneScenario:
		case SceneManager.SceneName.SceneTutorialFirst:
		case SceneManager.SceneName.SceneOtherMenuTop:
		case SceneManager.SceneName.SceneTutorialEnd:
		case SceneManager.SceneName.SceneItemView:
		case SceneManager.SceneName.ScenePvpDeck:
		case SceneManager.SceneName.SceneOpening:
		case SceneManager.SceneName.SceneStoryView:
			return true;
		}
		return false;
	}

	private SceneManager.SceneName GetDefaultDestination(int relType)
	{
		return (SceneManager.SceneName)((UserMissionOne.DefaultTransition)Enum.ToObject(typeof(UserMissionOne.DefaultTransition), relType)).GetValue<UserMissionOne.DefaultTransition>(relType);
	}

	private bool IsLockedScene(SceneManager.SceneName name, object obj, UnityAction<QuestOnePackData> action)
	{
		bool isReleased = true;
		QuestOnePackData releaseData = null;
		if (name <= SceneManager.SceneName.ScenePvp)
		{
			if (name != SceneManager.SceneName.SceneQuest)
			{
				if (name != SceneManager.SceneName.ScenePvp)
				{
					goto IL_0155;
				}
			}
			else
			{
				SceneQuest.Args args = obj as SceneQuest.Args;
				if (args.category == QuestStaticChapter.Category.INVALID || args.selectEventId != 0)
				{
					return false;
				}
				Singleton<SceneManager>.Instance.CheckReleaseDataByCategory(args.category, delegate(bool isRelease, QuestOnePackData qopd)
				{
					releaseData = qopd;
					isReleased = isRelease;
				});
				if (!isReleased)
				{
					goto IL_0155;
				}
				QuestOnePackData qopdId = DataManager.DmQuest.GetQuestOnePackData(args.selectQuestOneId);
				if (DataManager.DmQuest.GetPlayableMapIdList(qopdId.questChapter.chapterId).Find((int n) => n == qopdId.questMap.mapId) <= 0)
				{
					foreach (QuestStaticQuestOne.ReleaseConditions releaseConditions in QuestUtil.GetEnableReleaseConditionsList(qopdId.questMap.mapId))
					{
						QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(releaseConditions.questId);
						if (questOnePackData != null)
						{
							releaseData = questOnePackData;
							break;
						}
					}
					isReleased = releaseData == null;
					goto IL_0155;
				}
				goto IL_0155;
			}
		}
		else if (name != SceneManager.SceneName.ScenePicnic && name != SceneManager.SceneName.SceneTraining)
		{
			goto IL_0155;
		}
		Singleton<SceneManager>.Instance.CheckReleaseDataBySceneName(name, delegate(bool isRelease, QuestOnePackData qopd)
		{
			releaseData = qopd;
			isReleased = isRelease;
		});
		IL_0155:
		action(releaseData);
		return !isReleased;
	}

	public void ShowReleaseCondition(SceneManager.SceneName name, QuestOnePackData qopd)
	{
		string text = string.Empty;
		text = string.Concat(new string[]
		{
			SceneQuest.GetMainStoryName(qopd.questChapter.category, false),
			" ",
			qopd.questChapter.chapterName,
			" ",
			qopd.questGroup.titleName,
			" ",
			qopd.questOne.questName,
			PrjUtil.MakeMessage("クリア")
		});
		CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open("解放条件", new List<CmnReleaseConditionWindowCtrl.SetupParam>
		{
			new CmnReleaseConditionWindowCtrl.SetupParam
			{
				enableClear = false,
				text = text
			}
		});
	}

	[CompilerGenerated]
	internal static List<ItemData> <AddItemDataToList>g__ItemDataList|17_0(List<ItemData> inputList)
	{
		if (inputList.Count == 0)
		{
			return new List<ItemData>();
		}
		List<ItemData> list = new List<ItemData>();
		ItemInput itemInput = new ItemInput(inputList[0].id, 0);
		foreach (ItemData itemData in inputList)
		{
			if (itemInput.itemId == itemData.id)
			{
				itemInput.num += itemData.num;
			}
			else
			{
				list.Add(new ItemData(itemInput.itemId, itemInput.num));
				itemInput = new ItemInput(itemData.id, itemData.num);
			}
		}
		list.Add(new ItemData(itemInput.itemId, itemInput.num));
		return list;
	}

	private SelMissionCtrl.RequestType LastRequestType;

	public static int SKIP_TICKET_ID = 39000;

	private SelMissionCtrl.MissionGUI missionGuiData;

	private SelMissionCtrl.GuiMissionSpecialWindow guiSpecialWindow;

	private List<List<UserMissionGroup>> missionGroupDataListList;

	private IEnumerator RequestMissionReward;

	private UnityAction<SceneManager.SceneName, object> transitionAction;

	private bool GettingMission;

	private enum RequestType
	{
		None,
		One,
		All,
		Event,
		Special
	}

	public class MissionGUI
	{
		public Dictionary<SelMissionCtrl.MissionGUI.InfoType, SelMissionCtrl.MissionGUI.Info> InfoMap { get; set; }

		public List<SelMissionCtrl.MissionGUI.MissionTab> MissionTabBtnList { get; set; }

		public List<SelMissionCtrl.MissionGUI.MissionButton> MissionBtnList { get; set; }

		public int SelectTabIndex { get; set; }

		public int SelectBtnIndex { get; set; }

		public int DispMissionTabIndex { get; set; }

		public int DispMissionBtnIndex { get; set; }

		public MissionType DispMissionType { get; set; }

		public int DispMissionId { get; set; }

		public MissionGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			SelMissionCtrl.MissionGUI.Info info = new SelMissionCtrl.MissionGUI.Info(baseTr.Find("Base/InfoBase_Normal"));
			SelMissionCtrl.MissionGUI.Info info2 = new SelMissionCtrl.MissionGUI.Info(baseTr.Find("Base/InfoBase_Event"));
			info.Scroll = baseTr.Find("Base/CmnList/ScrollViewAll").GetComponent<ReuseScroll>();
			info2.Scroll = baseTr.Find("Base/EventList/ScrollViewAll").GetComponent<ReuseScroll>();
			this.InfoMap = new Dictionary<SelMissionCtrl.MissionGUI.InfoType, SelMissionCtrl.MissionGUI.Info>
			{
				{
					SelMissionCtrl.MissionGUI.InfoType.Normal,
					info
				},
				{
					SelMissionCtrl.MissionGUI.InfoType.Event,
					info2
				}
			};
			this.MissionTabBtnList = new List<SelMissionCtrl.MissionGUI.MissionTab>
			{
				new SelMissionCtrl.MissionGUI.MissionTab(baseTr.Find("Base/MenuAll/Btn_Page01")),
				new SelMissionCtrl.MissionGUI.MissionTab(baseTr.Find("Base/MenuAll/Btn_Page02"))
			};
			this.MissionBtnList = new List<SelMissionCtrl.MissionGUI.MissionButton>
			{
				new SelMissionCtrl.MissionGUI.MissionButton(baseTr.Find("Base/MenuAll/Grid/Btn_Mission01")),
				new SelMissionCtrl.MissionGUI.MissionButton(baseTr.Find("Base/MenuAll/Grid/Btn_Mission02")),
				new SelMissionCtrl.MissionGUI.MissionButton(baseTr.Find("Base/MenuAll/Grid/Btn_Mission03")),
				new SelMissionCtrl.MissionGUI.MissionButton(baseTr.Find("Base/MenuAll/Grid/Btn_Mission04")),
				new SelMissionCtrl.MissionGUI.MissionButton(baseTr.Find("Base/MenuAll/Grid/Btn_Mission05"))
			};
			this.Txt_Caution = baseTr.Find("Base/Txt_Caution").GetComponent<PguiTextCtrl>();
			this.Btn_AllGet = baseTr.Find("Btn_Get").GetComponent<PguiButtonCtrl>();
		}

		public GameObject baseObj;

		public PguiReplaceSpriteCtrl TabHeadSpriteBase;

		public PguiTextCtrl Txt_Caution;

		public PguiButtonCtrl Btn_AllGet;

		public GameObject EventNameLabel;

		public enum InfoType
		{
			Undefined,
			Normal,
			Event
		}

		public class Info
		{
			public SelMissionCtrl.MissionGUI.InfoType InfoType { get; set; }

			public GameObject BaseObj { get; private set; }

			public PguiTextCtrl TitleText { get; private set; }

			public PguiRawImageCtrl Banner { get; private set; }

			public PguiTextCtrl LimitText { get; private set; }

			public PguiTextCtrl ClearCount { get; private set; }

			public PguiImageCtrl MarkComplete { get; private set; }

			public ReuseScroll Scroll { get; set; }

			public Info(Transform baseTr)
			{
				this.BaseObj = baseTr.gameObject;
				Transform transform = baseTr.Find("Txt_Title");
				this.TitleText = ((transform != null) ? transform.GetComponent<PguiTextCtrl>() : null);
				Transform transform2 = baseTr.Find("Texture_Banner");
				this.Banner = ((transform2 != null) ? transform2.GetComponent<PguiRawImageCtrl>() : null);
				Transform transform3 = baseTr.Find("Num_Tem");
				this.LimitText = ((transform3 != null) ? transform3.GetComponent<PguiTextCtrl>() : null);
				Transform transform4 = baseTr.Find("Num_Clear");
				this.ClearCount = ((transform4 != null) ? transform4.GetComponent<PguiTextCtrl>() : null);
				Transform transform5 = baseTr.Find("Mark_complete");
				this.MarkComplete = ((transform5 != null) ? transform5.GetComponent<PguiImageCtrl>() : null);
			}

			public void SetActive(bool isEnable)
			{
				this.BaseObj.SetActive(isEnable);
				this.Scroll.transform.parent.gameObject.SetActive(isEnable);
			}
		}

		public class MissionTab
		{
			public MissionTab(Transform baseTr)
			{
				this.BaseObj = baseTr.gameObject;
				this.TabBtn = baseTr.GetComponent<PguiToggleButtonCtrl>();
				this.BadgeNumText = baseTr.Find("BaseImage/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
			}

			public void UpdateBadge(int count)
			{
				if (count == 0)
				{
					this.BadgeNumText.transform.parent.gameObject.SetActive(false);
					return;
				}
				this.BadgeNumText.transform.parent.gameObject.SetActive(true);
				this.BadgeNumText.text = count.ToString();
			}

			public GameObject BaseObj;

			public PguiToggleButtonCtrl TabBtn;

			public PguiTextCtrl BadgeNumText;
		}

		public class MissionButton
		{
			public MissionButton(Transform baseTr)
			{
				this.BaseObj = baseTr.gameObject;
				this.Button = this.BaseObj.GetComponent<PguiToggleButtonCtrl>();
				this.MissionName = baseTr.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
				this.BadgeNumText = baseTr.Find("BaseImage/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
			}

			public void UpdateBadge(int count)
			{
				if (count == 0)
				{
					this.BadgeNumText.transform.parent.gameObject.SetActive(false);
					return;
				}
				this.BadgeNumText.transform.parent.gameObject.SetActive(true);
				this.BadgeNumText.text = count.ToString();
			}

			public GameObject BaseObj;

			public PguiToggleButtonCtrl Button;

			public PguiTextCtrl MissionName;

			public PguiTextCtrl BadgeNumText;
		}
	}

	public class GuiMissionSpecialWindow
	{
		public GuiMissionSpecialWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.basePanel = baseTr.GetComponent<PguiPanel>();
			this.basePanel.raycastTarget = false;
			this.AEImageBackGround = baseTr.Find("AEImage_RandomItemGet/AEImage_Bg").GetComponent<PguiAECtrl>();
			this.AEImageBackGround.gameObject.SetActive(false);
			this.AEImageItemGet = baseTr.Find("AEImage_RandomItemGet/AEImage").GetComponent<PguiAECtrl>();
			this.AEImageItemGet.gameObject.SetActive(false);
			this.IconItemBase = baseTr.Find("AEImage_RandomItemGet/Icon_Item").gameObject;
			this.IconItem = baseTr.Find("AEImage_RandomItemGet/Icon_Item/Icon_Item").GetComponent<IconItemCtrl>();
			this.IconItemText = baseTr.Find("AEImage_RandomItemGet/Txt_ItemName").GetComponent<PguiTextCtrl>();
			this.IconItemBase.SetActive(false);
			this.IconItem.gameObject.SetActive(false);
			this.IconItemText.gameObject.SetActive(false);
			this.TouchText = baseTr.Find("AEImage_RandomItemGet/Txt_Touch").GetComponent<PguiTextCtrl>();
			this.TouchText.gameObject.SetActive(false);
			this.TouchPanel = baseTr.Find("AEImage_RandomItemGet/TouchPanel").GetComponent<PguiButtonCtrl>();
			this.TouchPanel.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickTouchNext), PguiButtonCtrl.SoundType.DEFAULT);
			this.TouchPanel.gameObject.SetActive(false);
			this.isLoop = false;
		}

		private void SetActiveSPObject(bool isView)
		{
			this.basePanel.raycastTarget = isView;
			this.AEImageBackGround.gameObject.SetActive(isView);
			this.AEImageItemGet.gameObject.SetActive(isView);
			this.IconItemBase.SetActive(isView);
			this.IconItem.gameObject.SetActive(isView);
			this.TouchText.gameObject.SetActive(isView);
			this.TouchPanel.gameObject.SetActive(isView);
			List<GachaResult> missionBonusSpecialResult = DataManager.DmMission.MissionBonusSpecialResult;
			List<GachaResult> multipleMissionBonusSpecialResult = DataManager.DmMission.MultipleMissionBonusSpecialResult;
			if (1 <= missionBonusSpecialResult.Count && multipleMissionBonusSpecialResult.Count > 0)
			{
				ItemData itemData;
				if (multipleMissionBonusSpecialResult.Count == 1)
				{
					if (multipleMissionBonusSpecialResult[0].item_id == SelMissionCtrl.SKIP_TICKET_ID)
					{
						return;
					}
					itemData = DataManager.DmItem.GetUserItemData(multipleMissionBonusSpecialResult[0].item_id);
					this.IconItem.Setup(itemData.staticData, multipleMissionBonusSpecialResult[0].item_num);
				}
				else
				{
					if (multipleMissionBonusSpecialResult.Count != 2)
					{
						return;
					}
					if (multipleMissionBonusSpecialResult[0].item_id != SelMissionCtrl.SKIP_TICKET_ID)
					{
						itemData = DataManager.DmItem.GetUserItemData(multipleMissionBonusSpecialResult[0].item_id);
						this.IconItem.Setup(itemData.staticData, multipleMissionBonusSpecialResult[0].item_num);
					}
					else
					{
						if (multipleMissionBonusSpecialResult[1].item_id == SelMissionCtrl.SKIP_TICKET_ID)
						{
							return;
						}
						itemData = DataManager.DmItem.GetUserItemData(multipleMissionBonusSpecialResult[1].item_id);
						this.IconItem.Setup(itemData.staticData, multipleMissionBonusSpecialResult[1].item_num);
					}
				}
				this.IconItemText.gameObject.SetActive(isView);
				this.IconItemText.text = itemData.staticData.GetName();
			}
		}

		public void StartSpecialMissionResult()
		{
			this.isLoop = false;
			this.SetActiveSPObject(true);
			this.AEImageBackGround.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				this.AEImageBackGround.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			});
			this.AEImageItemGet.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				this.AEImageItemGet.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				this.isLoop = true;
			});
			SoundManager.Play("prd_se_selector_special_mission_reward_get", false, false);
		}

		public void OnClickTouchNext(PguiButtonCtrl button)
		{
			if (this.isLoop)
			{
				this.AEImageBackGround.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
				{
					this.SetActiveSPObject(false);
					this.OpenResultMessage();
				});
				this.AEImageItemGet.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				this.isLoop = false;
			}
		}

		private void OpenResultMessage()
		{
			CanvasManager.HdlOpenWindowBasic.Setup("確認", "「スペシャルデイリー」の\n報酬を受け取りました", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
		}

		public GameObject baseObj;

		public PguiPanel basePanel;

		private PguiAECtrl AEImageBackGround;

		private PguiAECtrl AEImageItemGet;

		public GameObject IconItemBase;

		public PguiTextCtrl IconItemText;

		public IconItemCtrl IconItem;

		public PguiTextCtrl TouchText;

		public PguiButtonCtrl TouchPanel;

		private bool isLoop;
	}
}
