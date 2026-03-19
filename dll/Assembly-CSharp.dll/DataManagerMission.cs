using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerMission
{
	public DataManagerMission(DataManager p)
	{
		this.parentData = p;
	}

	public List<DataManagerMission.StaticMissionData> StaticMissionDataList
	{
		get
		{
			return this.staticMissionDataList;
		}
	}

	public List<UserMissionOne> UserMissionOneList
	{
		get
		{
			return this.userMissionOneList;
		}
	}

	public List<GachaResult> MissionBonusSpecialResult { get; private set; }

	public List<GachaResult> MultipleMissionBonusSpecialResult { get; private set; }

	public Dictionary<int, int> MissionBonusResultItemMap { get; private set; }

	private Dictionary<int, int> LastReqestMissionItemMap { get; set; }

	public bool isAchievementInRewardItemMap { get; set; }

	public List<UserMissionGroup> GetUserMissionGroupList()
	{
		return this.userMissionGroupList;
	}

	public UserMissionGroup GetEventMissionGroup(int evId)
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(evId);
		UserMissionGroup userMissionGroup = this.userMissionGroupList.Find((UserMissionGroup x) => x.type == MissionType.EVENTDAILY) ?? new UserMissionGroup();
		UserMissionGroup userMissionGroup2 = this.userMissionGroupList.Find((UserMissionGroup x) => x.type == MissionType.EVENTTOTAL) ?? new UserMissionGroup();
		DateTime endDatetime = eventData.endDatetime;
		UserMissionGroup userMissionGroup3 = new UserMissionGroup(MissionType.EVENTTOTAL, eventData.eventId, eventData.eventName, "「" + eventData.eventName + "」限定ミッション", endDatetime);
		userMissionGroup3.viewDataList = new List<UserMissionOne>();
		using (List<int>.Enumerator enumerator = eventData.missionIdList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int missionId2 = enumerator.Current;
				List<UserMissionOne> list = userMissionGroup.viewDataList.FindAll((UserMissionOne x) => x.missionId == missionId2);
				if (list != null)
				{
					userMissionGroup3.viewDataList.AddRange(list);
				}
				List<UserMissionOne> list2 = userMissionGroup.receivedDataList.FindAll((UserMissionOne x) => x.missionId == missionId2);
				if (list2 != null)
				{
					userMissionGroup3.receivedDataList.AddRange(list2);
				}
			}
		}
		using (List<int>.Enumerator enumerator = eventData.missionIdList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int missionId = enumerator.Current;
				List<UserMissionOne> list3 = userMissionGroup2.viewDataList.FindAll((UserMissionOne x) => x.missionId == missionId);
				if (list3 != null)
				{
					userMissionGroup3.viewDataList.AddRange(list3);
				}
				List<UserMissionOne> list4 = userMissionGroup2.receivedDataList.FindAll((UserMissionOne x) => x.missionId == missionId);
				if (list4 != null)
				{
					userMissionGroup3.receivedDataList.AddRange(list4);
				}
			}
		}
		userMissionGroup3.SortOneDataList();
		return userMissionGroup3;
	}

	public List<UserMissionGroup> GetValidEventMissionGroupe()
	{
		List<UserMissionGroup> list = new List<UserMissionGroup>();
		foreach (int num in DataManager.DmEvent.GetValidEventIdListWithoutMissionEvent())
		{
			list.Add(this.GetEventMissionGroup(num));
		}
		return list;
	}

	public int GetUserClearMissionNum()
	{
		if (this.userMissionOneList == null)
		{
			return 0;
		}
		int count = this.userMissionGroupList.Find((UserMissionGroup x) => MissionType.DAILY == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received).Count;
		int count2 = this.userMissionGroupList.Find((UserMissionGroup x) => MissionType.WEEKLY == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received).Count;
		int count3 = this.userMissionGroupList.Find((UserMissionGroup x) => MissionType.TOTAL == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received).Count;
		int count4 = this.userMissionGroupList.Find((UserMissionGroup x) => MissionType.BEGINNER == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received).Count;
		int num = 0;
		List<int> validEventIdListWithoutMissionEvent = DataManager.DmEvent.GetValidEventIdListWithoutMissionEvent();
		validEventIdListWithoutMissionEvent.AddRange(DataManager.DmEvent.GetValidMissionEventIdList(false));
		foreach (int num2 in validEventIdListWithoutMissionEvent)
		{
			DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(num2);
			num += this.GetEventMissionGroup(eventData.eventId).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received).Count;
		}
		return count + count2 + count3 + num + count4;
	}

	public int GetUserClearMissionNum(MissionType type, bool isSpecial)
	{
		if (this.userMissionOneList == null)
		{
			return 0;
		}
		return this.userMissionGroupList.Find((UserMissionGroup x) => type == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received && x.IsSpecial == isSpecial).Count;
	}

	public int GetUserClearEventMissionNum(int eventId)
	{
		if (this.userMissionOneList == null)
		{
			return 0;
		}
		return this.GetEventMissionGroup(eventId).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received).Count;
	}

	public int GetUserClearSpecialMissionNum(MissionType type)
	{
		if (this.userMissionOneList == null)
		{
			return 0;
		}
		return this.userMissionGroupList.Find((UserMissionGroup x) => type == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received && x.IsSpecial).Count;
	}

	public int GetUserClearAllSpecialMissionNum()
	{
		if (this.userMissionOneList == null)
		{
			return 0;
		}
		return 0 + this.userMissionGroupList.Find((UserMissionGroup x) => MissionType.DAILY == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received && x.IsSpecial).Count + this.userMissionGroupList.Find((UserMissionGroup x) => MissionType.WEEKLY == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received && x.IsSpecial).Count + this.userMissionGroupList.Find((UserMissionGroup x) => MissionType.TOTAL == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received && x.IsSpecial).Count;
	}

	public int GetLastRequestAchievementId()
	{
		if (this.LastReqestMissionItemMap == null)
		{
			return -1;
		}
		foreach (KeyValuePair<int, int> keyValuePair in this.LastReqestMissionItemMap)
		{
			if (!this.MissionBonusResultItemMap.ContainsKey(keyValuePair.Key) && DataManager.DmAchievement.GetAchievementData(keyValuePair.Key) != null)
			{
				return keyValuePair.Key;
			}
		}
		return -1;
	}

	public bool CompareResultItemAllReceived()
	{
		if (this.MissionBonusResultItemMap == null || this.LastReqestMissionItemMap == null)
		{
			return false;
		}
		if (this.MissionBonusResultItemMap.Count != this.LastReqestMissionItemMap.Count)
		{
			return false;
		}
		bool flag = true;
		foreach (KeyValuePair<int, int> keyValuePair in this.MissionBonusResultItemMap)
		{
			if (this.GetLastRequestAchievementId() == -1 && this.LastReqestMissionItemMap[keyValuePair.Key] != keyValuePair.Value)
			{
				flag = false;
				break;
			}
		}
		return flag;
	}

	private void RequestMissionItemList(List<AcceptMission> reqList)
	{
		this.LastReqestMissionItemMap = new Dictionary<int, int>();
		using (List<AcceptMission>.Enumerator enumerator = reqList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AcceptMission req = enumerator.Current;
				DataManagerMission.StaticMissionData staticMissionData = this.StaticMissionDataList.Find((DataManagerMission.StaticMissionData x) => req.mission_id == x.MissionId);
				ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(staticMissionData.RewardItemId);
				if (itemStaticBase.GetKind() == ItemDef.Kind.PRESET)
				{
					using (List<ItemPresetData.Item>.Enumerator enumerator2 = (itemStaticBase as ItemPresetData).SetItemList.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ItemPresetData.Item item = enumerator2.Current;
							this.<RequestMissionItemList>g__addItemToList|41_0(item.itemId, item.num * staticMissionData.RewardItemNum);
						}
						continue;
					}
				}
				this.<RequestMissionItemList>g__addItemToList|41_0(staticMissionData.RewardItemId, staticMissionData.RewardItemNum);
			}
		}
	}

	public void RequestGetMissionList()
	{
		List<int> list = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
		this.parentData.ServerRequest(MissionListCmd.Create(list), new Action<Command>(this.CbMissionListCmd));
	}

	public void DummyRequestClearMissionResultItem()
	{
		this.LastReqestMissionItemMap = new Dictionary<int, int>();
		this.MissionBonusResultItemMap = new Dictionary<int, int>();
	}

	public void RequestActionMissionRewardOne(UserMissionOne targetMission)
	{
		this.MissionBonusResultItemMap = new Dictionary<int, int>();
		this.MissionBonusSpecialResult = new List<GachaResult>();
		this.MultipleMissionBonusSpecialResult = new List<GachaResult>();
		List<AcceptMission> list = new List<AcceptMission> { targetMission.AcceptMission };
		this.RequestMissionItemList(list);
		this.parentData.ServerRequest(MissionBonusAcceptCmd.Create(null, list), new Action<Command>(this.CbMissionBonusAcceptCmd));
		this.isAchievementInRewardItemMap = false;
	}

	public void RequestActionSpecialMissionRewardOne(UserMissionOne targetMission)
	{
		this.MissionBonusResultItemMap = new Dictionary<int, int>();
		this.MissionBonusSpecialResult = new List<GachaResult>();
		this.MultipleMissionBonusSpecialResult = new List<GachaResult>();
		List<AcceptMission> list = new List<AcceptMission> { targetMission.AcceptMission };
		this.RequestMissionItemList(list);
		this.parentData.ServerRequest(MissionBonusSpecialAcceptCmd.Create(null, list), new Action<Command>(this.CbMissionBonusSpecialAcceptCmd));
		this.isAchievementInRewardItemMap = false;
	}

	public void RequestActionMissionRewardAll(MissionType type, bool isSpecial, bool isOne)
	{
		this.MissionBonusResultItemMap = new Dictionary<int, int>();
		this.MissionBonusSpecialResult = new List<GachaResult>();
		this.MultipleMissionBonusSpecialResult = new List<GachaResult>();
		switch (type)
		{
		case MissionType.DAILY:
		case MissionType.WEEKLY:
		case MissionType.TOTAL:
		case MissionType.BEGINNER:
		{
			UserMissionGroup userMissionGroup = this.userMissionGroupList.Find((UserMissionGroup x) => x.type == type);
			this.isAchievementInRewardItemMap = userMissionGroup.viewDataList.Find((UserMissionOne item) => item.GetRewardItemData().staticData.GetKind() == ItemDef.Kind.ACHIEVEMENT && item.isClear) != null;
			List<AcceptMission> list = userMissionGroup.viewDataList.FindAll((UserMissionOne item) => item.isClear && !item.Received && item.GetRewardItemData().staticData.GetKind() != ItemDef.Kind.ACHIEVEMENT && isSpecial == item.IsSpecial).ConvertAll<AcceptMission>((UserMissionOne item) => item.AcceptMission);
			if (!isSpecial)
			{
				if (0 < list.Count)
				{
					this.RequestMissionItemList(list);
					this.parentData.ServerRequest(MissionBonusAcceptCmd.Create(null, list), new Action<Command>(this.CbMissionBonusAcceptCmd));
					return;
				}
				return;
			}
			else
			{
				if (0 >= list.Count)
				{
					return;
				}
				this.MultipleMissionBonusSpecialResult = new List<GachaResult>();
				if (isOne)
				{
					this.RequestMissionItemList(list);
					this.parentData.ServerRequest(MissionBonusSpecialAcceptCmd.Create(null, list), new Action<Command>(this.CbMissionBonusSpecialAcceptCmd));
					return;
				}
				using (List<AcceptMission>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AcceptMission acceptMission = enumerator.Current;
						this.parentData.ServerRequest(MissionBonusSpecialAcceptCmd.Create(null, new List<AcceptMission> { acceptMission }), new Action<Command>(this.CbMissionBonusSpecialAcceptCmd));
					}
					return;
				}
			}
			break;
		}
		}
		this.parentData.ServerRequest(MissionBonusAcceptCmd.Create(null, null), new Action<Command>(this.CbMissionBonusAcceptCmd));
	}

	public void RequestActionMissionRewardEvent(int eventId)
	{
		this.MissionBonusResultItemMap = new Dictionary<int, int>();
		this.MissionBonusSpecialResult = new List<GachaResult>();
		this.MultipleMissionBonusSpecialResult = new List<GachaResult>();
		List<int> eventList = DataManager.DmEvent.GetEventData(eventId).missionIdList;
		List<UserMissionOne> list = this.userMissionOneList.FindAll((UserMissionOne item) => item.isClear && !item.Received && eventList.Contains(item.missionId));
		List<UserMissionOne> list2 = list.FindAll((UserMissionOne item) => item.GetRewardItemData().staticData.GetKind() != ItemDef.Kind.ACHIEVEMENT);
		this.isAchievementInRewardItemMap = list.Exists((UserMissionOne item) => item.GetRewardItemData().staticData.GetKind() == ItemDef.Kind.ACHIEVEMENT);
		List<AcceptMission> list3 = list2.ConvertAll<AcceptMission>((UserMissionOne item) => item.AcceptMission);
		this.RequestMissionItemList(list3);
		this.parentData.ServerRequest(MissionBonusAcceptCmd.Create(null, list3), new Action<Command>(this.CbMissionBonusAcceptCmd));
	}

	public void RequestActionMissionRewardAll()
	{
		this.isAchievementInRewardItemMap = this.userMissionOneList.Find((UserMissionOne item) => item.GetRewardItemData().staticData.GetKind() == ItemDef.Kind.ACHIEVEMENT && item.isClear) != null;
		List<AcceptMission> list = this.userMissionOneList.FindAll((UserMissionOne item) => item.isClear && !item.Received && item.GetRewardItemData().staticData.GetKind() != ItemDef.Kind.ACHIEVEMENT).ConvertAll<AcceptMission>((UserMissionOne item) => item.AcceptMission);
		this.MissionBonusResultItemMap = new Dictionary<int, int>();
		this.MissionBonusSpecialResult = new List<GachaResult>();
		this.MultipleMissionBonusSpecialResult = new List<GachaResult>();
		this.parentData.ServerRequest(MissionBonusAcceptCmd.Create(null, list), new Action<Command>(this.CbMissionBonusAcceptCmd));
	}

	private void CbMissionListCmd(Command cmd)
	{
		MissionListResponse missionListResponse = cmd.response as MissionListResponse;
		this.userMissionGroupList = new List<UserMissionGroup>();
		this.userMissionOneList = new List<UserMissionOne>();
		foreach (object obj in Enum.GetValues(typeof(MissionType)))
		{
			MissionType missionType = (MissionType)obj;
			if (MissionType.FRIENDS != missionType && MissionType.HIDDEN != missionType)
			{
				UserMissionGroup userMissionGroup = new UserMissionGroup(missionType);
				this.userMissionGroupList.Add(userMissionGroup);
			}
		}
		this.userMissionGroupList.Sort((UserMissionGroup a, UserMissionGroup b) => DataManagerMission.MissionType2TabSortId(a.type) - DataManagerMission.MissionType2TabSortId(b.type));
		using (List<DataManagerMission.StaticMissionData>.Enumerator enumerator2 = this.staticMissionDataList.FindAll((DataManagerMission.StaticMissionData x) => x.AlwaysDispFlg && x.StartDateTime < TimeManager.Now && TimeManager.Now < x.EndDateTime).GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				DataManagerMission.StaticMissionData alwaysMission = enumerator2.Current;
				if (missionListResponse.missions.Find((Mission x) => alwaysMission.MissionId == x.mission_id) == null)
				{
					missionListResponse.missions.Add(new Mission
					{
						mission_id = alwaysMission.MissionId
					});
				}
			}
		}
		missionListResponse.missions.Sort((Mission a, Mission b) => a.mission_id - b.mission_id);
		using (List<Mission>.Enumerator enumerator3 = missionListResponse.missions.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				Mission resMission = enumerator3.Current;
				DataManagerMission.StaticMissionData missionOne = this.staticMissionDataList.Find((DataManagerMission.StaticMissionData x) => x.MissionId == resMission.mission_id);
				if (missionOne != null)
				{
					UserMissionGroup userMissionGroup2 = this.userMissionGroupList.Find((UserMissionGroup item) => item.type == missionOne.MissionType);
					if (userMissionGroup2 != null)
					{
						UserMissionOne userMissionOne = new UserMissionOne(missionOne, resMission, false);
						if (userMissionOne.Received && missionOne.MissionType != MissionType.DAILY && missionOne.MissionType != MissionType.WEEKLY && missionOne.MissionType != MissionType.EVENTTOTAL && missionOne.MissionType != MissionType.EVENTDAILY)
						{
							userMissionGroup2.receivedDataList.Add(userMissionOne);
						}
						else
						{
							userMissionGroup2.viewDataList.Add(userMissionOne);
						}
						this.userMissionOneList.Add(userMissionOne);
					}
				}
			}
		}
		foreach (UserMissionGroup userMissionGroup3 in this.userMissionGroupList)
		{
			userMissionGroup3.SortOneDataList();
		}
		this.parentData.UpdateUserAssetByAssets(missionListResponse.assets);
	}

	private void CbMissionBonusAcceptCmd(Command cmd)
	{
		MissionBonusAcceptResponse missionBonusAcceptResponse = cmd.response as MissionBonusAcceptResponse;
		MissionBonusAcceptRequest missionBonusAcceptRequest = cmd.request as MissionBonusAcceptRequest;
		if (missionBonusAcceptResponse.assets.update_item_list != null)
		{
			foreach (Item item2 in missionBonusAcceptResponse.assets.update_item_list)
			{
				if (!this.MissionBonusResultItemMap.ContainsKey(item2.item_id))
				{
					int item_id = item2.item_id;
					int num = item2.item_num - DataManager.DmItem.GetUserItemData(item_id).num;
					if (0 < num)
					{
						this.MissionBonusResultItemMap.Add(item_id, num);
					}
				}
			}
		}
		if (missionBonusAcceptResponse.assets.update_item_bank_list != null)
		{
			foreach (ItemBank itemBank in missionBonusAcceptResponse.assets.update_item_bank_list)
			{
				int item_id2 = itemBank.item_id;
				int content_id = itemBank.content_id;
				long num2 = itemBank.content_num - DataManager.DmItem.UserItemBankMap.TryGetValueEx(item_id2, new ItemBank()).content_num;
				if (!this.MissionBonusResultItemMap.ContainsKey(content_id))
				{
					this.MissionBonusResultItemMap.Add(content_id, (int)num2);
				}
				else
				{
					Dictionary<int, int> dictionary = this.MissionBonusResultItemMap;
					int num3 = content_id;
					dictionary[num3] += (int)num2;
				}
			}
		}
		if (missionBonusAcceptResponse.assets.update_photo_list != null)
		{
			foreach (Photo photo in missionBonusAcceptResponse.assets.update_photo_list)
			{
				if (!this.MissionBonusResultItemMap.ContainsKey(photo.item_id))
				{
					this.MissionBonusResultItemMap.Add(photo.item_id, 0);
				}
				Dictionary<int, int> missionBonusResultItemMap = this.MissionBonusResultItemMap;
				int num3 = photo.item_id;
				int num4 = missionBonusResultItemMap[num3] + 1;
				missionBonusResultItemMap[num3] = num4;
			}
		}
		if (missionBonusAcceptResponse.assets.update_accessory_list != null)
		{
			foreach (Accessory accessory in missionBonusAcceptResponse.assets.update_accessory_list)
			{
				if (!this.MissionBonusResultItemMap.ContainsKey(accessory.item_id))
				{
					this.MissionBonusResultItemMap.Add(accessory.item_id, 0);
				}
				Dictionary<int, int> missionBonusResultItemMap2 = this.MissionBonusResultItemMap;
				int num4 = accessory.item_id;
				int num3 = missionBonusResultItemMap2[num4] + 1;
				missionBonusResultItemMap2[num4] = num3;
			}
		}
		if (missionBonusAcceptResponse.assets.update_achievement_list != null)
		{
			foreach (Achievement achievement in missionBonusAcceptResponse.assets.update_achievement_list)
			{
				if (!this.MissionBonusResultItemMap.ContainsKey(achievement.achievement_id))
				{
					this.MissionBonusResultItemMap.Add(achievement.achievement_id, 0);
				}
				Dictionary<int, int> missionBonusResultItemMap3 = this.MissionBonusResultItemMap;
				int num3 = achievement.achievement_id;
				int num4 = missionBonusResultItemMap3[num3];
				missionBonusResultItemMap3[num3] = num4 + 1;
			}
		}
		if (missionBonusAcceptResponse.assets.update_sticker_list != null)
		{
			foreach (Sticker sticker in missionBonusAcceptResponse.assets.update_sticker_list)
			{
				int num5 = sticker.num - DataManager.DmItem.GetUserItemData(sticker.id).num;
				if (!this.MissionBonusResultItemMap.ContainsKey(sticker.id))
				{
					this.MissionBonusResultItemMap.Add(sticker.id, num5);
				}
				else
				{
					Dictionary<int, int> dictionary = this.MissionBonusResultItemMap;
					int num4 = sticker.id;
					dictionary[num4] += num5;
				}
			}
		}
		this.parentData.UpdateUserAssetByAssets(missionBonusAcceptResponse.assets);
		using (List<AcceptMission>.Enumerator enumerator7 = missionBonusAcceptRequest.accept_mission_list.GetEnumerator())
		{
			while (enumerator7.MoveNext())
			{
				AcceptMission serverMission = enumerator7.Current;
				DataManagerMission.StaticMissionData staticMission = this.staticMissionDataList.Find((DataManagerMission.StaticMissionData item) => item.MissionId == serverMission.mission_id);
				if (staticMission != null)
				{
					UserMissionGroup userMissionGroup = this.userMissionGroupList.Find((UserMissionGroup item) => item.type == staticMission.MissionType);
					if (userMissionGroup != null)
					{
						UserMissionOne userMissionOne = userMissionGroup.viewDataList.Find((UserMissionOne item) => item.CompareAcceptMission(serverMission));
						if (userMissionOne != null)
						{
							userMissionOne.Received = true;
							if (userMissionGroup.type != MissionType.DAILY && userMissionGroup.type != MissionType.WEEKLY && userMissionGroup.type != MissionType.EVENTTOTAL && userMissionGroup.type != MissionType.EVENTDAILY)
							{
								userMissionGroup.viewDataList.Remove(userMissionOne);
								userMissionGroup.receivedDataList.Add(userMissionOne);
							}
						}
					}
				}
			}
		}
		foreach (UserMissionGroup userMissionGroup2 in this.userMissionGroupList)
		{
			userMissionGroup2.SortOneDataList();
		}
	}

	private void CbMissionBonusSpecialAcceptCmd(Command cmd)
	{
		MissionBonusSpecialAcceptResponse missionBonusSpecialAcceptResponse = cmd.response as MissionBonusSpecialAcceptResponse;
		MissionBonusSpecialAcceptRequest missionBonusSpecialAcceptRequest = cmd.request as MissionBonusSpecialAcceptRequest;
		this.MissionBonusSpecialResult = missionBonusSpecialAcceptResponse.gacha_result;
		if (this.MissionBonusSpecialResult != null)
		{
			this.MultipleMissionBonusSpecialResult.AddRange(this.MissionBonusSpecialResult);
		}
		this.parentData.UpdateUserAssetByAssets(missionBonusSpecialAcceptResponse.assets);
		using (List<AcceptMission>.Enumerator enumerator = missionBonusSpecialAcceptRequest.accept_mission_list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AcceptMission serverMission = enumerator.Current;
				DataManagerMission.StaticMissionData staticMission = this.staticMissionDataList.Find((DataManagerMission.StaticMissionData item) => item.MissionId == serverMission.mission_id);
				if (staticMission != null)
				{
					UserMissionGroup userMissionGroup = this.userMissionGroupList.Find((UserMissionGroup item) => item.type == staticMission.MissionType);
					if (userMissionGroup != null)
					{
						UserMissionOne userMissionOne = userMissionGroup.viewDataList.Find((UserMissionOne item) => item.CompareAcceptMission(serverMission));
						if (userMissionOne != null)
						{
							userMissionOne.Received = true;
							if (userMissionGroup.type != MissionType.DAILY && userMissionGroup.type != MissionType.WEEKLY && userMissionGroup.type != MissionType.EVENTTOTAL && userMissionGroup.type != MissionType.EVENTDAILY)
							{
								userMissionGroup.viewDataList.Remove(userMissionOne);
								userMissionGroup.receivedDataList.Add(userMissionOne);
							}
						}
					}
				}
			}
		}
		foreach (UserMissionGroup userMissionGroup2 in this.userMissionGroupList)
		{
			userMissionGroup2.SortOneDataList();
		}
	}

	public void AddWaitDisplayMission(List<Mission> serverMissionList)
	{
		if (this.waitDisplayMissionList == null)
		{
			this.waitDisplayMissionList = new List<Mission>();
		}
		this.waitDisplayMissionList.AddRange(serverMissionList);
	}

	public void UpdateUserDataByServer(List<Mission> serverMissionList)
	{
		if (this.userMissionGroupList == null)
		{
			return;
		}
		if (this.waitDisplayMissionList != null)
		{
			serverMissionList.InsertRange(0, this.waitDisplayMissionList);
			this.waitDisplayMissionList = null;
		}
		using (List<Mission>.Enumerator enumerator = serverMissionList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Mission serverMission = enumerator.Current;
				DataManagerMission.StaticMissionData staticMission = this.staticMissionDataList.Find((DataManagerMission.StaticMissionData item) => item.MissionId == serverMission.mission_id);
				if (staticMission != null)
				{
					UserMissionGroup userMissionGroup = this.userMissionGroupList.Find((UserMissionGroup item) => item.type == staticMission.MissionType);
					if (userMissionGroup != null)
					{
						UserMissionOne userMissionOne = userMissionGroup.viewDataList.Find((UserMissionOne item) => item.missionId == serverMission.mission_id);
						if (userMissionOne != null)
						{
							if (userMissionOne.keyServerTime == 0L)
							{
								userMissionOne.keyServerTime = serverMission.mission_datetime;
							}
							if (userMissionOne.keyServerTime != serverMission.mission_datetime)
							{
								userMissionOne = null;
							}
						}
						if (userMissionOne == null)
						{
							userMissionOne = new UserMissionOne(staticMission, serverMission, true);
							userMissionGroup.viewDataList.Add(userMissionOne);
							this.userMissionOneList.Add(userMissionOne);
						}
						bool flag = false;
						if (serverMission.accept_flg == 0)
						{
							flag = true;
						}
						else if (userMissionGroup.type != MissionType.DAILY && userMissionGroup.type != MissionType.WEEKLY && userMissionGroup.type != MissionType.EVENTTOTAL && userMissionGroup.type != MissionType.EVENTDAILY)
						{
							userMissionGroup.viewDataList.Remove(userMissionOne);
							userMissionGroup.receivedDataList.Add(userMissionOne);
						}
						userMissionOne.Update(serverMission);
						if (flag)
						{
							CanvasManager.HdlMissionProgressCtrl.PushProgress(userMissionOne);
						}
					}
				}
			}
		}
		foreach (UserMissionGroup userMissionGroup2 in this.userMissionGroupList)
		{
			userMissionGroup2.SortOneDataList();
		}
	}

	public void InitializeMstData(MstManager mstManager)
	{
		List<MstMissionData> mst = mstManager.GetMst<List<MstMissionData>>(MstType.MISSION_DATA);
		this.staticMissionDataList = new List<DataManagerMission.StaticMissionData>();
		foreach (MstMissionData mstMissionData in mst)
		{
			DataManagerMission.StaticMissionData staticMissionData = new DataManagerMission.StaticMissionData(mstMissionData);
			this.staticMissionDataList.Add(staticMissionData);
		}
	}

	public static int MissionType2TabSortId(MissionType type)
	{
		switch (type)
		{
		case MissionType.DAILY:
			return 1;
		case MissionType.WEEKLY:
			return 2;
		case MissionType.TOTAL:
			return 3;
		case MissionType.EVENTTOTAL:
			return 6;
		case MissionType.SPECIAL:
			return 7;
		case MissionType.BEGINNER:
			return 4;
		case MissionType.EVENTDAILY:
			return 5;
		default:
			return 0;
		}
	}

	[CompilerGenerated]
	private void <RequestMissionItemList>g__addItemToList|41_0(int itemId, int itemNum)
	{
		if (!this.LastReqestMissionItemMap.ContainsKey(itemId))
		{
			this.LastReqestMissionItemMap.Add(itemId, itemNum);
			return;
		}
		Dictionary<int, int> lastReqestMissionItemMap = this.LastReqestMissionItemMap;
		lastReqestMissionItemMap[itemId] += itemNum;
	}

	private DataManager parentData;

	private List<DataManagerMission.StaticMissionData> staticMissionDataList;

	private List<UserMissionOne> userMissionOneList;

	private List<UserMissionGroup> userMissionGroupList;

	private List<Mission> waitDisplayMissionList;

	public class StaticMissionData
	{
		public int MissionId { get; private set; }

		public MissionType MissionType { get; private set; }

		public string MissionContents { get; private set; }

		public int SortNum { get; private set; }

		public bool AlwaysDispFlg { get; private set; }

		public int Denominator { get; private set; }

		public int RewardItemId { get; private set; }

		public int RewardItemNum { get; private set; }

		public int NeedMissionId { get; private set; }

		public bool IsSpecial { get; private set; }

		public DateTime StartDateTime { get; private set; }

		public DateTime EndDateTime { get; private set; }

		public SceneManager.SceneName TransitionScene { get; private set; }

		public int TransitionId { get; private set; }

		public int RelType { get; private set; }

		public StaticMissionData(MstMissionData mstMission)
		{
			this.MissionId = mstMission.missionId;
			bool flag = false;
			this.MissionType = this.Int2Missiontype(ref flag, mstMission.missionType);
			this.IsSpecial = flag;
			this.MissionContents = mstMission.missionContents;
			this.Denominator = mstMission.denom;
			this.SortNum = mstMission.sortNum;
			this.RewardItemId = mstMission.rewardItemId;
			this.RewardItemNum = mstMission.rewardItemNum;
			this.NeedMissionId = mstMission.needMissionId;
			this.AlwaysDispFlg = 1 == mstMission.alwaysDispFlg;
			this.StartDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstMission.startTime));
			this.EndDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstMission.endTime));
			this.TransitionScene = (SceneManager.SceneName)mstMission.transitionScene;
			this.TransitionId = mstMission.transitionId;
			this.RelType = mstMission.relType;
		}

		private MissionType Int2Missiontype(ref bool isSpecial, int type)
		{
			MissionType missionType;
			switch (type)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 6:
			case 7:
			case 9:
				return (MissionType)type;
			case 5:
				missionType = MissionType.DAILY;
				isSpecial = true;
				return missionType;
			}
			missionType = MissionType.INVALID;
			return missionType;
		}
	}
}
