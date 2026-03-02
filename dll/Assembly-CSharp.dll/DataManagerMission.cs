using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x02000092 RID: 146
public class DataManagerMission
{
	// Token: 0x060005B6 RID: 1462 RVA: 0x000264AF File Offset: 0x000246AF
	public DataManagerMission(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x060005B7 RID: 1463 RVA: 0x000264BE File Offset: 0x000246BE
	public List<DataManagerMission.StaticMissionData> StaticMissionDataList
	{
		get
		{
			return this.staticMissionDataList;
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x060005B8 RID: 1464 RVA: 0x000264C6 File Offset: 0x000246C6
	public List<UserMissionOne> UserMissionOneList
	{
		get
		{
			return this.userMissionOneList;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x060005B9 RID: 1465 RVA: 0x000264CE File Offset: 0x000246CE
	// (set) Token: 0x060005BA RID: 1466 RVA: 0x000264D6 File Offset: 0x000246D6
	public List<GachaResult> MissionBonusSpecialResult { get; private set; }

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x060005BB RID: 1467 RVA: 0x000264DF File Offset: 0x000246DF
	// (set) Token: 0x060005BC RID: 1468 RVA: 0x000264E7 File Offset: 0x000246E7
	public List<GachaResult> MultipleMissionBonusSpecialResult { get; private set; }

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x060005BD RID: 1469 RVA: 0x000264F0 File Offset: 0x000246F0
	// (set) Token: 0x060005BE RID: 1470 RVA: 0x000264F8 File Offset: 0x000246F8
	public Dictionary<int, int> MissionBonusResultItemMap { get; private set; }

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x00026501 File Offset: 0x00024701
	// (set) Token: 0x060005C0 RID: 1472 RVA: 0x00026509 File Offset: 0x00024709
	private Dictionary<int, int> LastReqestMissionItemMap { get; set; }

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00026512 File Offset: 0x00024712
	// (set) Token: 0x060005C2 RID: 1474 RVA: 0x0002651A File Offset: 0x0002471A
	public bool isAchievementInRewardItemMap { get; set; }

	// Token: 0x060005C3 RID: 1475 RVA: 0x00026523 File Offset: 0x00024723
	public List<UserMissionGroup> GetUserMissionGroupList()
	{
		return this.userMissionGroupList;
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x0002652C File Offset: 0x0002472C
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

	// Token: 0x060005C5 RID: 1477 RVA: 0x0002673C File Offset: 0x0002493C
	public List<UserMissionGroup> GetValidEventMissionGroupe()
	{
		List<UserMissionGroup> list = new List<UserMissionGroup>();
		foreach (int num in DataManager.DmEvent.GetValidEventIdListWithoutMissionEvent())
		{
			list.Add(this.GetEventMissionGroup(num));
		}
		return list;
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x000267A0 File Offset: 0x000249A0
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

	// Token: 0x060005C7 RID: 1479 RVA: 0x000269CC File Offset: 0x00024BCC
	public int GetUserClearMissionNum(MissionType type, bool isSpecial)
	{
		if (this.userMissionOneList == null)
		{
			return 0;
		}
		return this.userMissionGroupList.Find((UserMissionGroup x) => type == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received && x.IsSpecial == isSpecial).Count;
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x00026A2C File Offset: 0x00024C2C
	public int GetUserClearEventMissionNum(int eventId)
	{
		if (this.userMissionOneList == null)
		{
			return 0;
		}
		return this.GetEventMissionGroup(eventId).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received).Count;
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x00026A78 File Offset: 0x00024C78
	public int GetUserClearSpecialMissionNum(MissionType type)
	{
		if (this.userMissionOneList == null)
		{
			return 0;
		}
		return this.userMissionGroupList.Find((UserMissionGroup x) => type == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received && x.IsSpecial).Count;
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00026AE4 File Offset: 0x00024CE4
	public int GetUserClearAllSpecialMissionNum()
	{
		if (this.userMissionOneList == null)
		{
			return 0;
		}
		return 0 + this.userMissionGroupList.Find((UserMissionGroup x) => MissionType.DAILY == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received && x.IsSpecial).Count + this.userMissionGroupList.Find((UserMissionGroup x) => MissionType.WEEKLY == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received && x.IsSpecial).Count + this.userMissionGroupList.Find((UserMissionGroup x) => MissionType.TOTAL == x.type).viewDataList.FindAll((UserMissionOne x) => x.isClear && !x.Received && x.IsSpecial).Count;
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x00026C08 File Offset: 0x00024E08
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

	// Token: 0x060005CC RID: 1484 RVA: 0x00026C94 File Offset: 0x00024E94
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

	// Token: 0x060005CD RID: 1485 RVA: 0x00026D38 File Offset: 0x00024F38
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

	// Token: 0x060005CE RID: 1486 RVA: 0x00026E40 File Offset: 0x00025040
	public void RequestGetMissionList()
	{
		List<int> list = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
		this.parentData.ServerRequest(MissionListCmd.Create(list), new Action<Command>(this.CbMissionListCmd));
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x00026EA1 File Offset: 0x000250A1
	public void DummyRequestClearMissionResultItem()
	{
		this.LastReqestMissionItemMap = new Dictionary<int, int>();
		this.MissionBonusResultItemMap = new Dictionary<int, int>();
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00026EBC File Offset: 0x000250BC
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

	// Token: 0x060005D1 RID: 1489 RVA: 0x00026F28 File Offset: 0x00025128
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

	// Token: 0x060005D2 RID: 1490 RVA: 0x00026F94 File Offset: 0x00025194
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

	// Token: 0x060005D3 RID: 1491 RVA: 0x00027190 File Offset: 0x00025390
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

	// Token: 0x060005D4 RID: 1492 RVA: 0x0002728C File Offset: 0x0002548C
	public void RequestActionMissionRewardAll()
	{
		this.isAchievementInRewardItemMap = this.userMissionOneList.Find((UserMissionOne item) => item.GetRewardItemData().staticData.GetKind() == ItemDef.Kind.ACHIEVEMENT && item.isClear) != null;
		List<AcceptMission> list = this.userMissionOneList.FindAll((UserMissionOne item) => item.isClear && !item.Received && item.GetRewardItemData().staticData.GetKind() != ItemDef.Kind.ACHIEVEMENT).ConvertAll<AcceptMission>((UserMissionOne item) => item.AcceptMission);
		this.MissionBonusResultItemMap = new Dictionary<int, int>();
		this.MissionBonusSpecialResult = new List<GachaResult>();
		this.MultipleMissionBonusSpecialResult = new List<GachaResult>();
		this.parentData.ServerRequest(MissionBonusAcceptCmd.Create(null, list), new Action<Command>(this.CbMissionBonusAcceptCmd));
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0002735C File Offset: 0x0002555C
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

	// Token: 0x060005D6 RID: 1494 RVA: 0x000276A0 File Offset: 0x000258A0
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
					Dictionary<int, int> missionBonusResultItemMap = this.MissionBonusResultItemMap;
					int num3 = content_id;
					missionBonusResultItemMap[num3] += (int)num2;
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
				Dictionary<int, int> missionBonusResultItemMap2 = this.MissionBonusResultItemMap;
				int num3 = photo.item_id;
				int num4 = missionBonusResultItemMap2[num3] + 1;
				missionBonusResultItemMap2[num3] = num4;
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
				Dictionary<int, int> missionBonusResultItemMap3 = this.MissionBonusResultItemMap;
				int num4 = accessory.item_id;
				int num3 = missionBonusResultItemMap3[num4] + 1;
				missionBonusResultItemMap3[num4] = num3;
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
				Dictionary<int, int> missionBonusResultItemMap4 = this.MissionBonusResultItemMap;
				int num3 = achievement.achievement_id;
				int num4 = missionBonusResultItemMap4[num3];
				missionBonusResultItemMap4[num3] = num4 + 1;
			}
		}
		this.parentData.UpdateUserAssetByAssets(missionBonusAcceptResponse.assets);
		using (List<AcceptMission>.Enumerator enumerator6 = missionBonusAcceptRequest.accept_mission_list.GetEnumerator())
		{
			while (enumerator6.MoveNext())
			{
				AcceptMission serverMission = enumerator6.Current;
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

	// Token: 0x060005D7 RID: 1495 RVA: 0x00027B5C File Offset: 0x00025D5C
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

	// Token: 0x060005D8 RID: 1496 RVA: 0x00027CFC File Offset: 0x00025EFC
	public void AddWaitDisplayMission(List<Mission> serverMissionList)
	{
		if (this.waitDisplayMissionList == null)
		{
			this.waitDisplayMissionList = new List<Mission>();
		}
		this.waitDisplayMissionList.AddRange(serverMissionList);
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x00027D20 File Offset: 0x00025F20
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

	// Token: 0x060005DA RID: 1498 RVA: 0x00027F34 File Offset: 0x00026134
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

	// Token: 0x060005DB RID: 1499 RVA: 0x00027FA0 File Offset: 0x000261A0
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

	// Token: 0x060005DC RID: 1500 RVA: 0x00027FD8 File Offset: 0x000261D8
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

	// Token: 0x040005AA RID: 1450
	private DataManager parentData;

	// Token: 0x040005AB RID: 1451
	private List<DataManagerMission.StaticMissionData> staticMissionDataList;

	// Token: 0x040005AC RID: 1452
	private List<UserMissionOne> userMissionOneList;

	// Token: 0x040005AD RID: 1453
	private List<UserMissionGroup> userMissionGroupList;

	// Token: 0x040005AE RID: 1454
	private List<Mission> waitDisplayMissionList;

	// Token: 0x020006F0 RID: 1776
	public class StaticMissionData
	{
		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060033B0 RID: 13232 RVA: 0x001C209B File Offset: 0x001C029B
		// (set) Token: 0x060033B1 RID: 13233 RVA: 0x001C20A3 File Offset: 0x001C02A3
		public int MissionId { get; private set; }

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060033B2 RID: 13234 RVA: 0x001C20AC File Offset: 0x001C02AC
		// (set) Token: 0x060033B3 RID: 13235 RVA: 0x001C20B4 File Offset: 0x001C02B4
		public MissionType MissionType { get; private set; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060033B4 RID: 13236 RVA: 0x001C20BD File Offset: 0x001C02BD
		// (set) Token: 0x060033B5 RID: 13237 RVA: 0x001C20C5 File Offset: 0x001C02C5
		public string MissionContents { get; private set; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060033B6 RID: 13238 RVA: 0x001C20CE File Offset: 0x001C02CE
		// (set) Token: 0x060033B7 RID: 13239 RVA: 0x001C20D6 File Offset: 0x001C02D6
		public int SortNum { get; private set; }

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060033B8 RID: 13240 RVA: 0x001C20DF File Offset: 0x001C02DF
		// (set) Token: 0x060033B9 RID: 13241 RVA: 0x001C20E7 File Offset: 0x001C02E7
		public bool AlwaysDispFlg { get; private set; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x001C20F0 File Offset: 0x001C02F0
		// (set) Token: 0x060033BB RID: 13243 RVA: 0x001C20F8 File Offset: 0x001C02F8
		public int Denominator { get; private set; }

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060033BC RID: 13244 RVA: 0x001C2101 File Offset: 0x001C0301
		// (set) Token: 0x060033BD RID: 13245 RVA: 0x001C2109 File Offset: 0x001C0309
		public int RewardItemId { get; private set; }

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060033BE RID: 13246 RVA: 0x001C2112 File Offset: 0x001C0312
		// (set) Token: 0x060033BF RID: 13247 RVA: 0x001C211A File Offset: 0x001C031A
		public int RewardItemNum { get; private set; }

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060033C0 RID: 13248 RVA: 0x001C2123 File Offset: 0x001C0323
		// (set) Token: 0x060033C1 RID: 13249 RVA: 0x001C212B File Offset: 0x001C032B
		public int NeedMissionId { get; private set; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060033C2 RID: 13250 RVA: 0x001C2134 File Offset: 0x001C0334
		// (set) Token: 0x060033C3 RID: 13251 RVA: 0x001C213C File Offset: 0x001C033C
		public bool IsSpecial { get; private set; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060033C4 RID: 13252 RVA: 0x001C2145 File Offset: 0x001C0345
		// (set) Token: 0x060033C5 RID: 13253 RVA: 0x001C214D File Offset: 0x001C034D
		public DateTime StartDateTime { get; private set; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x060033C6 RID: 13254 RVA: 0x001C2156 File Offset: 0x001C0356
		// (set) Token: 0x060033C7 RID: 13255 RVA: 0x001C215E File Offset: 0x001C035E
		public DateTime EndDateTime { get; private set; }

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x060033C8 RID: 13256 RVA: 0x001C2167 File Offset: 0x001C0367
		// (set) Token: 0x060033C9 RID: 13257 RVA: 0x001C216F File Offset: 0x001C036F
		public SceneManager.SceneName TransitionScene { get; private set; }

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x060033CA RID: 13258 RVA: 0x001C2178 File Offset: 0x001C0378
		// (set) Token: 0x060033CB RID: 13259 RVA: 0x001C2180 File Offset: 0x001C0380
		public int TransitionId { get; private set; }

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x060033CC RID: 13260 RVA: 0x001C2189 File Offset: 0x001C0389
		// (set) Token: 0x060033CD RID: 13261 RVA: 0x001C2191 File Offset: 0x001C0391
		public int RelType { get; private set; }

		// Token: 0x060033CE RID: 13262 RVA: 0x001C219C File Offset: 0x001C039C
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

		// Token: 0x060033CF RID: 13263 RVA: 0x001C2280 File Offset: 0x001C0480
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
