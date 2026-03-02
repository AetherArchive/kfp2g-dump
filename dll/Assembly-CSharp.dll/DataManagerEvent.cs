using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Login;
using SGNFW.Mst;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class DataManagerEvent
{
	// Token: 0x06000447 RID: 1095 RVA: 0x0001D670 File Offset: 0x0001B870
	public DataManagerEvent(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000448 RID: 1096 RVA: 0x0001D697 File Offset: 0x0001B897
	// (set) Token: 0x06000449 RID: 1097 RVA: 0x0001D69F File Offset: 0x0001B89F
	public DataManagerEvent.CoopData LastCoopInfo { get; private set; }

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x0600044A RID: 1098 RVA: 0x0001D6A8 File Offset: 0x0001B8A8
	private List<DataManagerEvent.ReleaseEffects> ReleaseEffectsList
	{
		get
		{
			return new List<DataManagerEvent.ReleaseEffects>
			{
				this.releaseEffects1.Clone(),
				this.releaseEffects2.Clone()
			};
		}
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x0001D6D4 File Offset: 0x0001B8D4
	public List<int> GetValidEventIdListWithoutMissionEvent()
	{
		List<int> list = new List<int>();
		DateTime now = TimeManager.Now;
		foreach (DataManagerEvent.EventData eventData in this.eventDataList)
		{
			if (eventData.eventCategory != DataManagerEvent.Category.Mission && !(now < eventData.startDatetime) && !(eventData.endDatetime < now))
			{
				list.Add(eventData.eventId);
				if (2 <= list.Count)
				{
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x0001D76C File Offset: 0x0001B96C
	public List<int> GetValidMissionEventIdList(bool isExcludeMissionEvent = false)
	{
		List<int> list = new List<int>();
		DateTime now = TimeManager.Now;
		foreach (DataManagerEvent.EventData eventData in this.eventDataList)
		{
			if (eventData.eventCategory == DataManagerEvent.Category.Mission && !(now < eventData.startDatetime) && !(eventData.endDatetime < now))
			{
				list.Add(eventData.eventId);
			}
		}
		return list;
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0001D7F8 File Offset: 0x0001B9F8
	public List<DataManagerEvent.EventData> GetEventDataList()
	{
		if (this.eventDataList == null)
		{
			this.eventDataList = new List<DataManagerEvent.EventData>();
		}
		return this.eventDataList;
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0001D813 File Offset: 0x0001BA13
	public List<DataManagerEvent.EventData> GetEventDataListWithoutMissionEvent()
	{
		return this.eventDataList.FindAll((DataManagerEvent.EventData ev) => ev.eventCategory != DataManagerEvent.Category.Mission);
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x0001D840 File Offset: 0x0001BA40
	public List<DataManagerEvent.EventData> GetEventDataList(DataManagerEvent.Category category)
	{
		return this.eventDataList.FindAll((DataManagerEvent.EventData ev) => ev.eventCategory == category);
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x0001D874 File Offset: 0x0001BA74
	public DataManagerEvent.EventData GetEventData(int evId)
	{
		return this.eventDataList.Find((DataManagerEvent.EventData ev) => ev.eventId == evId);
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x0001D8A8 File Offset: 0x0001BAA8
	public DataManagerEvent.EventData GetEventDataCompareToChapterId(int chId)
	{
		return this.eventDataList.Find((DataManagerEvent.EventData ev) => ev.eventChapterId == chId);
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x0001D8D9 File Offset: 0x0001BAD9
	public List<DataManagerEvent.ReleaseEffects> GetReleaseEffectsList()
	{
		return this.ReleaseEffectsList;
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x0001D8E4 File Offset: 0x0001BAE4
	public DataManagerEvent.ReleaseEffects GetReleaseEffects(int evId)
	{
		return this.ReleaseEffectsList.Find((DataManagerEvent.ReleaseEffects ev) => ev.EventId == evId);
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0001D915 File Offset: 0x0001BB15
	public List<DataManagerEvent.EventBannerData> GetEventBannerDataList()
	{
		if (this.eventBannerDataList == null)
		{
			this.eventBannerDataList = new List<DataManagerEvent.EventBannerData>();
		}
		return this.eventBannerDataList.FindAll((DataManagerEvent.EventBannerData x) => x.StartDatetime < TimeManager.Now && TimeManager.Now < x.EndDatetime && (x.StartQuestId == 0 || DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(x.StartQuestId)) && (x.EndQuestId == 0 || !DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(x.EndQuestId) || DataManager.DmQuest.QuestDynamicData.oneDataMap[x.EndQuestId].clearNum == 0));
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x0001D954 File Offset: 0x0001BB54
	public DataManagerEvent.LargeEventData GetLargeEventData(int eventId)
	{
		return this.largeEventDataList.Find((DataManagerEvent.LargeEventData x) => eventId == x.EventID);
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x0001D988 File Offset: 0x0001BB88
	public List<DataManagerEvent.PeriodData> GetPeriodDataList(int evId)
	{
		List<DataManagerEvent.PeriodData> list = this.periodDataList.FindAll((DataManagerEvent.PeriodData x) => x.EventId == evId);
		if (1 < list.Count)
		{
			list.Sort((DataManagerEvent.PeriodData a, DataManagerEvent.PeriodData b) => a.StartDatetime.CompareTo(b.StartDatetime));
		}
		return list;
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x0001D9EC File Offset: 0x0001BBEC
	public DataManagerEvent.PeriodData GetLatestPeriodDataList(int evId)
	{
		DataManagerEvent.PeriodData periodData = null;
		List<DataManagerEvent.PeriodData> list = this.periodDataList.FindAll((DataManagerEvent.PeriodData x) => x.EventId == evId);
		list.Sort((DataManagerEvent.PeriodData a, DataManagerEvent.PeriodData b) => a.StartDatetime.CompareTo(b.StartDatetime));
		DateTime now = TimeManager.Now;
		foreach (DataManagerEvent.PeriodData periodData2 in list)
		{
			if (!(periodData2.StartDatetime < now))
			{
				break;
			}
			periodData = periodData2;
		}
		return periodData;
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0001DA98 File Offset: 0x0001BC98
	public List<DataManagerEvent.CoopConditionData> GetCoopConditionDataList()
	{
		return this.coopConditionDataList;
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0001DAA0 File Offset: 0x0001BCA0
	public List<DataManagerEvent.CoopHardQuestData> GetCoopHardQuestDataList()
	{
		return this.coopHardQuestDataList;
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0001DAA8 File Offset: 0x0001BCA8
	public DataManagerEvent.CoopRaidTermData GetTermData(int eventId, DateTime time)
	{
		return this.coopRaidTermDataList.Find((DataManagerEvent.CoopRaidTermData item) => item.eventId == eventId && item.IsOverStartTime(time) && !item.IsOverEndTime(time));
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0001DAE0 File Offset: 0x0001BCE0
	public DataManagerEvent.CoopRaidTermData GetNowTermData(int eventId)
	{
		return this.GetTermData(eventId, TimeManager.Now);
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x0001DAF0 File Offset: 0x0001BCF0
	public DataManagerEvent.CoopRaidTermData GetNextTermData(DataManagerEvent.EventData eventData)
	{
		DataManagerEvent.CoopRaidTermData coopRaidTermData = null;
		int num = 0;
		for (;;)
		{
			num++;
			DateTime dateTime = TimeManager.Now.AddHours((double)num);
			if (num > 24)
			{
				goto IL_0042;
			}
			if (coopRaidTermData != null)
			{
				break;
			}
			if (eventData.endDatetime <= dateTime)
			{
				goto Block_3;
			}
			coopRaidTermData = this.GetTermData(eventData.eventId, dateTime);
		}
		return coopRaidTermData;
		Block_3:
		return null;
		IL_0042:
		return null;
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0001DB40 File Offset: 0x0001BD40
	public List<int> GetConvertDrawId(int eventId, int mapId, int drawId)
	{
		List<int> list = new List<int>();
		if (this.LastCoopInfo == null || !this.LastCoopInfo.MapInfoMap.ContainsKey(mapId))
		{
			list.Add(drawId);
			return list;
		}
		DataManagerEvent.CoopData.MapInfo mapInfo = this.LastCoopInfo.MapInfoMap[mapId];
		double num = (double)mapInfo.TotalPoint;
		double num2 = (double)mapInfo.EndPoint;
		int nowGaugeProgress = (int)(num / num2 * 100.0);
		List<DataManagerEvent.CoopRaidDrawData> list2 = this.coopRaidDrawDataList.FindAll((DataManagerEvent.CoopRaidDrawData data) => data.eventId == eventId && data.drawId == drawId && data.gaugeProgress <= nowGaugeProgress);
		int convertGaugeProgres = 0;
		foreach (DataManagerEvent.CoopRaidDrawData coopRaidDrawData in list2)
		{
			if (coopRaidDrawData.gaugeProgress >= convertGaugeProgres)
			{
				convertGaugeProgres = coopRaidDrawData.gaugeProgress;
			}
		}
		List<DataManagerEvent.CoopRaidDrawData> list3 = list2.FindAll((DataManagerEvent.CoopRaidDrawData item) => item.gaugeProgress == convertGaugeProgres);
		if (list3.Count<DataManagerEvent.CoopRaidDrawData>() > 0)
		{
			using (List<DataManagerEvent.CoopRaidDrawData>.Enumerator enumerator = list3.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DataManagerEvent.CoopRaidDrawData coopRaidDrawData2 = enumerator.Current;
					list.Add(coopRaidDrawData2.convertDrawId);
				}
				return list;
			}
		}
		list.Add(drawId);
		return list;
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0001DCB0 File Offset: 0x0001BEB0
	public bool isRaidByMapId(int mapId)
	{
		return this.LastCoopInfo != null && this.LastCoopInfo.MapInfoMap.ContainsKey(mapId) && DataManager.DmEvent.GetEventData(this.LastCoopInfo.MapInfoMap[mapId].EventId).raidFlg;
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0001DD00 File Offset: 0x0001BF00
	public bool isRaidByQuestOneId(int questOneId)
	{
		int mapId = DataManager.DmQuest.GetQuestOnePackData(questOneId).questMap.mapId;
		return this.isRaidByMapId(mapId);
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x0001DD2C File Offset: 0x0001BF2C
	public bool isRaidByEventId(int eventId)
	{
		DataManagerEvent.EventData eventData = this.GetEventData(eventId);
		return eventData != null && eventData.raidFlg;
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0001DD4C File Offset: 0x0001BF4C
	public bool isRaidBonusMapId(int mapId)
	{
		QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap[mapId];
		return questStaticMap != null && questStaticMap.QuestMapCategory == QuestStaticMap.MapCategory.CoopBonus;
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0001DD7E File Offset: 0x0001BF7E
	public void RequestSelectGrowthEventCharaId(int eventId, int charaId)
	{
		this.parentData.ServerRequest(SelectGrowthEventCharaIdCmd.Create(eventId, charaId), new Action<Command>(this.CbSelectGrowthEventCharaIdCmd));
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0001DD9E File Offset: 0x0001BF9E
	public void RequestGetGrowthEventCharaId(int eventId)
	{
		this.parentData.ServerRequest(GetGrowthEventCharaIdCmd.Create(eventId), new Action<Command>(this.CbGetGrowthEventCharaIdCmd));
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0001DDC0 File Offset: 0x0001BFC0
	public void RequestUpdateReleaseEffects(List<DataManagerEvent.ReleaseEffects> requestList)
	{
		if (requestList == null)
		{
			return;
		}
		if (requestList.Count == 0)
		{
			return;
		}
		if (2 < requestList.Count)
		{
			return;
		}
		if (1 == requestList.Count)
		{
			requestList.Add(new DataManagerEvent.ReleaseEffects(null));
		}
		if (this.releaseEffects2.EventId == requestList[0].EventId)
		{
			requestList.Reverse();
		}
		for (int i = 0; i < this.ReleaseEffectsList.Count; i++)
		{
			int num = requestList[i].ReleaseIdList.Count - this.ReleaseEffectsList[i].ReleaseIdList.Count;
			if (0 > num)
			{
				for (int j = 0; j < num; j++)
				{
					requestList[i].ReleaseIdList.Add(0);
				}
			}
		}
		List<DataManagerEvent.ReleaseEffects> list = new List<DataManagerEvent.ReleaseEffects>();
		foreach (DataManagerEvent.ReleaseEffects releaseEffects in requestList)
		{
			DataManagerEvent.EventData eventData = this.GetEventData(releaseEffects.EventId);
			DateTime now = TimeManager.Now;
			DataManagerEvent.ReleaseEffects releaseEffects2;
			if (eventData == null || now < eventData.startDatetime || eventData.endDatetime < now)
			{
				releaseEffects2 = new DataManagerEvent.ReleaseEffects(null)
				{
					ReleaseIdList = new List<int>()
				};
				using (List<int>.Enumerator enumerator2 = releaseEffects.ReleaseIdList.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						int num2 = enumerator2.Current;
						releaseEffects2.ReleaseIdList.Add(0);
					}
					goto IL_0157;
				}
				goto IL_0153;
			}
			goto IL_0153;
			IL_0157:
			list.Add(releaseEffects2);
			continue;
			IL_0153:
			releaseEffects2 = releaseEffects;
			goto IL_0157;
		}
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(this.CreateServerData(list)), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0001DF88 File Offset: 0x0001C188
	public void RequestGetCoopInfo(int evId, int mapId)
	{
		if (mapId != 0 && this.LastCoopInfo == null)
		{
			return;
		}
		this.parentData.ServerRequest(CoopInfoCmd.Create(evId, mapId), new Action<Command>(this.CbGetCoopInfoCmd));
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0001DFB4 File Offset: 0x0001C1B4
	private void CbSelectGrowthEventCharaIdCmd(Command cmd)
	{
		SelectGrowthEventCharaIdResponse selectGrowthEventCharaIdResponse = cmd.response as SelectGrowthEventCharaIdResponse;
		DataManagerEvent.EventData eventData = this.GetEventData(selectGrowthEventCharaIdResponse.event_id);
		eventData.SelectGrowthCharaData = new DataManagerEvent.EventData.GrowthCharaData(new DataManagerEvent.EventData.Bonus(selectGrowthEventCharaIdResponse.chara_id, eventData.GrowthSelcharaRatio), 0L, 0L);
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0001DFFC File Offset: 0x0001C1FC
	private void CbGetGrowthEventCharaIdCmd(Command cmd)
	{
		GetGrowthEventCharaIdResponse getGrowthEventCharaIdResponse = cmd.response as GetGrowthEventCharaIdResponse;
		DataManagerEvent.EventData eventData = this.GetEventData(getGrowthEventCharaIdResponse.event_id);
		DataManagerEvent.EventData.Bonus bonus = new DataManagerEvent.EventData.Bonus(getGrowthEventCharaIdResponse.chara_id, eventData.GrowthSelcharaRatio);
		eventData.SelectGrowthCharaData = new DataManagerEvent.EventData.GrowthCharaData(bonus, getGrowthEventCharaIdResponse.select_chara_datetime, getGrowthEventCharaIdResponse.quest_clear_datetime);
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0001E04C File Offset: 0x0001C24C
	private void CbNewFlgUpdateCmd(Command cmd)
	{
		NewFlgUpdateRequest newFlgUpdateRequest = cmd.request as NewFlgUpdateRequest;
		this.UpdateUserFlagByServer(newFlgUpdateRequest.new_flg_list);
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0001E074 File Offset: 0x0001C274
	private void CbGetCoopInfoCmd(Command cmd)
	{
		CoopInfoRequest coopInfoRequest = cmd.request as CoopInfoRequest;
		CoopInfoResponse coopInfoResponse = cmd.response as CoopInfoResponse;
		this.UpdateCoopInfo(coopInfoRequest, coopInfoResponse);
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x0001E0A4 File Offset: 0x0001C2A4
	public void InitializeMstData(MstManager mstManager)
	{
		List<MstEventData> mst = mstManager.GetMst<List<MstEventData>>(MstType.EVENT_DATA);
		List<MstEventMissionData> mst2 = mstManager.GetMst<List<MstEventMissionData>>(MstType.EVENT_MISSION_DATA);
		List<MstEventGrowthData> mst3 = mstManager.GetMst<List<MstEventGrowthData>>(MstType.EVENT_GROWTH_DATA);
		List<MstEventBannerData> mst4 = mstManager.GetMst<List<MstEventBannerData>>(MstType.EVENT_BANNER_DATA);
		List<MstEventImageData> mst5 = mstManager.GetMst<List<MstEventImageData>>(MstType.EVENT_IMAGE_DATA);
		List<MstEventLargeEventData> mst6 = mstManager.GetMst<List<MstEventLargeEventData>>(MstType.EVENT_LARGE_EVENT_DATA);
		List<MstEventPeriodData> mst7 = mstManager.GetMst<List<MstEventPeriodData>>(MstType.EVENT_PERIOD_DATA);
		List<MstEventCoopConditionData> mst8 = mstManager.GetMst<List<MstEventCoopConditionData>>(MstType.EVENT_COOP_CONDITION_DATA);
		List<MstEventCoopHardQuestData> mst9 = mstManager.GetMst<List<MstEventCoopHardQuestData>>(MstType.EVENT_COOP_HARD_QUEST_DATA);
		List<MstEventCoopRaidTermData> mst10 = mstManager.GetMst<List<MstEventCoopRaidTermData>>(MstType.COOP_RAID_TERM_DATA);
		List<MstEventCoopRaidDrawData> mst11 = mstManager.GetMst<List<MstEventCoopRaidDrawData>>(MstType.COOP_RAID_DRAW_DATA);
		long num = PrjUtil.ConvertTicksToTime(TimeManager.Now.Ticks);
		this.eventDataList = new List<DataManagerEvent.EventData>();
		mst.Sort((MstEventData a, MstEventData b) => b.eventId - a.eventId);
		foreach (MstEventData mstEventData in mst)
		{
			if (mstEventData.endDatetime >= num)
			{
				DataManagerEvent.EventData evData = new DataManagerEvent.EventData(mstEventData);
				List<MstEventMissionData> list = mst2.FindAll((MstEventMissionData ev) => ev.eventMissionGroupId == evData.eventMissionGroupId);
				evData.missionIdList = list.ConvertAll<int>((MstEventMissionData ev) => ev.missionId);
				evData.missionIdList.Sort();
				evData.GrowthCharaList = new List<DataManagerEvent.EventData.Bonus>();
				foreach (MstEventGrowthData mstEventGrowthData in mst3)
				{
					if (mstEventData.growthPresetId == mstEventGrowthData.growthPresetId)
					{
						evData.GrowthCharaList.Add(new DataManagerEvent.EventData.Bonus(mstEventGrowthData.charaId, mstEventGrowthData.bonusRatio));
					}
				}
				evData.GrowthQuestGroupList = new List<int>();
				foreach (QuestStaticQuestGroup questStaticQuestGroup in DataManager.DmQuest.QuestStaticData.groupDataList.FindAll((QuestStaticQuestGroup x) => x.growthEventId == evData.eventId))
				{
					evData.GrowthQuestGroupList.Add(questStaticQuestGroup.questGroupId);
				}
				this.eventDataList.Add(evData);
			}
		}
		this.eventBannerDataList = new List<DataManagerEvent.EventBannerData>();
		foreach (MstEventBannerData mstEventBannerData in mst4)
		{
			if (mstEventBannerData.platform == 0 || mstEventBannerData.platform == LoginManager.Platform)
			{
				DataManagerEvent.EventBannerData eventBannerData = new DataManagerEvent.EventBannerData(mstEventBannerData);
				this.eventBannerDataList.Add(eventBannerData);
			}
		}
		using (List<MstEventImageData>.Enumerator enumerator5 = mst5.GetEnumerator())
		{
			while (enumerator5.MoveNext())
			{
				MstEventImageData mstEvImageData = enumerator5.Current;
				DataManagerEvent.EventData eventData = this.eventDataList.Find((DataManagerEvent.EventData x) => x.eventId == mstEvImageData.eventId);
				if (eventData != null)
				{
					eventData.eventImageDataList.Add(new DataManagerEvent.EventImageData(mstEvImageData));
				}
			}
		}
		this.largeEventDataList = new List<DataManagerEvent.LargeEventData>();
		foreach (MstEventLargeEventData mstEventLargeEventData in mst6)
		{
			this.largeEventDataList.Add(new DataManagerEvent.LargeEventData(mstEventLargeEventData));
		}
		this.periodDataList = new List<DataManagerEvent.PeriodData>();
		foreach (MstEventPeriodData mstEventPeriodData in mst7)
		{
			this.periodDataList.Add(new DataManagerEvent.PeriodData(mstEventPeriodData));
		}
		this.coopConditionDataList = new List<DataManagerEvent.CoopConditionData>();
		foreach (MstEventCoopConditionData mstEventCoopConditionData in mst8)
		{
			this.coopConditionDataList.Add(new DataManagerEvent.CoopConditionData(mstEventCoopConditionData));
		}
		this.coopHardQuestDataList = new List<DataManagerEvent.CoopHardQuestData>();
		foreach (MstEventCoopHardQuestData mstEventCoopHardQuestData in mst9)
		{
			this.coopHardQuestDataList.Add(new DataManagerEvent.CoopHardQuestData(mstEventCoopHardQuestData));
		}
		this.coopRaidTermDataList = new List<DataManagerEvent.CoopRaidTermData>();
		foreach (MstEventCoopRaidTermData mstEventCoopRaidTermData in mst10)
		{
			this.coopRaidTermDataList.Add(new DataManagerEvent.CoopRaidTermData(mstEventCoopRaidTermData));
		}
		this.coopRaidDrawDataList = new List<DataManagerEvent.CoopRaidDrawData>();
		foreach (MstEventCoopRaidDrawData mstEventCoopRaidDrawData in mst11)
		{
			this.coopRaidDrawDataList.Add(new DataManagerEvent.CoopRaidDrawData(mstEventCoopRaidDrawData));
		}
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0001E6A4 File Offset: 0x0001C8A4
	public void UpdateUserFlagByServer(List<NewFlg> newFlagList)
	{
		foreach (NewFlg newFlg in newFlagList)
		{
			DataManager.NewFlgCategory category = (DataManager.NewFlgCategory)newFlg.category;
			DataManager.NewFlgCategory newFlgCategory;
			if (category != DataManager.NewFlgCategory.EVENT1)
			{
				if (category != DataManager.NewFlgCategory.EVENT2)
				{
					continue;
				}
				newFlgCategory = DataManager.NewFlgCategory.EVENT2;
			}
			else
			{
				newFlgCategory = DataManager.NewFlgCategory.EVENT1;
			}
			if (newFlgCategory != DataManager.NewFlgCategory.EVENT1)
			{
				if (newFlgCategory == DataManager.NewFlgCategory.EVENT2)
				{
					if (newFlg.any_id == 0)
					{
						this.releaseEffects2 = new DataManagerEvent.ReleaseEffects(this.GetEventData(newFlg.new_mgmt_flg))
						{
							ReleaseIdList = new List<int>()
						};
					}
					else if (1 == newFlg.any_id)
					{
						this.releaseEffects2.TutorialPhase = newFlg.new_mgmt_flg;
					}
					else
					{
						this.releaseEffects2.ReleaseIdList.Add(newFlg.new_mgmt_flg);
					}
				}
			}
			else if (newFlg.any_id == 0)
			{
				this.releaseEffects1 = new DataManagerEvent.ReleaseEffects(this.GetEventData(newFlg.new_mgmt_flg))
				{
					ReleaseIdList = new List<int>()
				};
			}
			else if (1 == newFlg.any_id)
			{
				this.releaseEffects1.TutorialPhase = newFlg.new_mgmt_flg;
			}
			else
			{
				this.releaseEffects1.ReleaseIdList.Add(newFlg.new_mgmt_flg);
			}
		}
		DataManagerEvent.<UpdateUserFlagByServer>g__CheckLength|50_0(this.releaseEffects1);
		DataManagerEvent.<UpdateUserFlagByServer>g__CheckLength|50_0(this.releaseEffects2);
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0001E7FC File Offset: 0x0001C9FC
	private void UpdateCoopInfo(CoopInfoRequest req, CoopInfoResponse res)
	{
		if (req.map_id == 0)
		{
			this.LastCoopInfo = new DataManagerEvent.CoopData(res, req.event_id);
			return;
		}
		if (this.LastCoopInfo == null)
		{
			return;
		}
		this.LastCoopInfo.UpdateMapData(res, req.event_id);
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0001E834 File Offset: 0x0001CA34
	private List<NewFlg> CreateServerData(List<DataManagerEvent.ReleaseEffects> reList)
	{
		int num = 0;
		List<NewFlg> list = new List<NewFlg>();
		foreach (DataManagerEvent.ReleaseEffects releaseEffects in reList)
		{
			list.AddRange(releaseEffects.ServerData((num == 0) ? DataManager.NewFlgCategory.EVENT1 : DataManager.NewFlgCategory.EVENT2));
			num++;
		}
		return list;
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0001E89C File Offset: 0x0001CA9C
	[CompilerGenerated]
	internal static void <UpdateUserFlagByServer>g__CheckLength|50_0(DataManagerEvent.ReleaseEffects re)
	{
		DataManagerEvent.Category category = re.Category;
		if (category != DataManagerEvent.Category.Scenario && category != DataManagerEvent.Category.Tower)
		{
			return;
		}
		while (2 > re.ReleaseIdList.Count)
		{
			re.ReleaseIdList.Add(0);
		}
	}

	// Token: 0x040004FB RID: 1275
	private DataManager parentData;

	// Token: 0x040004FC RID: 1276
	private List<DataManagerEvent.EventData> eventDataList;

	// Token: 0x040004FD RID: 1277
	private List<DataManagerEvent.EventBannerData> eventBannerDataList;

	// Token: 0x040004FE RID: 1278
	private List<DataManagerEvent.LargeEventData> largeEventDataList;

	// Token: 0x040004FF RID: 1279
	private List<DataManagerEvent.CoopConditionData> coopConditionDataList;

	// Token: 0x04000500 RID: 1280
	private List<DataManagerEvent.CoopHardQuestData> coopHardQuestDataList;

	// Token: 0x04000502 RID: 1282
	private List<DataManagerEvent.PeriodData> periodDataList;

	// Token: 0x04000503 RID: 1283
	private List<DataManagerEvent.CoopRaidTermData> coopRaidTermDataList;

	// Token: 0x04000504 RID: 1284
	private List<DataManagerEvent.CoopRaidDrawData> coopRaidDrawDataList;

	// Token: 0x04000505 RID: 1285
	private DataManagerEvent.ReleaseEffects releaseEffects1 = new DataManagerEvent.ReleaseEffects(null);

	// Token: 0x04000506 RID: 1286
	private DataManagerEvent.ReleaseEffects releaseEffects2 = new DataManagerEvent.ReleaseEffects(null);

	// Token: 0x0200066A RID: 1642
	public class EventData
	{
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x0600316B RID: 12651 RVA: 0x001BD2AF File Offset: 0x001BB4AF
		// (set) Token: 0x0600316C RID: 12652 RVA: 0x001BD2B7 File Offset: 0x001BB4B7
		public List<DataManagerEvent.EventData.Bonus> GrowthCharaList { get; set; }

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x0600316D RID: 12653 RVA: 0x001BD2C0 File Offset: 0x001BB4C0
		// (set) Token: 0x0600316E RID: 12654 RVA: 0x001BD2C8 File Offset: 0x001BB4C8
		public List<int> GrowthQuestGroupList { get; set; }

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600316F RID: 12655 RVA: 0x001BD2D1 File Offset: 0x001BB4D1
		// (set) Token: 0x06003170 RID: 12656 RVA: 0x001BD2D9 File Offset: 0x001BB4D9
		public DataManagerEvent.EventData.GrowthCharaData SelectGrowthCharaData { get; set; } = new DataManagerEvent.EventData.GrowthCharaData(new DataManagerEvent.EventData.Bonus(0, 0), 0L, 0L);

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x001BD2E2 File Offset: 0x001BB4E2
		// (set) Token: 0x06003172 RID: 12658 RVA: 0x001BD2EA File Offset: 0x001BB4EA
		public int GrowthSelcharaRatio { get; private set; }

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x001BD2F3 File Offset: 0x001BB4F3
		// (set) Token: 0x06003174 RID: 12660 RVA: 0x001BD2FB File Offset: 0x001BB4FB
		public bool homeDispFlg { get; private set; }

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06003175 RID: 12661 RVA: 0x001BD304 File Offset: 0x001BB504
		// (set) Token: 0x06003176 RID: 12662 RVA: 0x001BD30C File Offset: 0x001BB50C
		public bool raidFlg { get; private set; }

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06003177 RID: 12663 RVA: 0x001BD315 File Offset: 0x001BB515
		public bool IsEnableEvent
		{
			get
			{
				return this.startDatetime < TimeManager.Now && TimeManager.Now < this.endDatetime;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06003178 RID: 12664 RVA: 0x001BD340 File Offset: 0x001BB540
		public bool IsEnableBannerByQuestTop
		{
			get
			{
				if (this.eventCategory == DataManagerEvent.Category.Mission)
				{
					return false;
				}
				HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(this.eventBannerId);
				return homeBannerData != null && (homeBannerData.startTime < TimeManager.Now && TimeManager.Now < homeBannerData.endTime);
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x001BD398 File Offset: 0x001BB598
		public bool IsEnableChapter
		{
			get
			{
				DateTime dateTime = new DateTime(2100, 1, 1);
				DateTime dateTime2 = default(DateTime);
				if (DataManager.DmQuest.QuestStaticData.chapterDataMap.ContainsKey(this.eventChapterId))
				{
					QuestStaticChapter chapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.eventChapterId];
					List<QuestStaticMap> list = DataManager.DmQuest.QuestStaticData.mapDataList.FindAll((QuestStaticMap x) => x.chapterId == chapter.chapterId);
					if (list.Count == 0)
					{
						return false;
					}
					using (List<QuestStaticMap>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							QuestStaticMap map = enumerator.Current;
							List<QuestStaticQuestGroup> list2 = DataManager.DmQuest.QuestStaticData.groupDataList.FindAll((QuestStaticQuestGroup x) => x.mapId == map.mapId);
							if (list2.Count == 0)
							{
								return false;
							}
							list2.Sort((QuestStaticQuestGroup x, QuestStaticQuestGroup y) => x.startDatetime.CompareTo(y.startDatetime));
							dateTime = ((dateTime < list2[0].startDatetime) ? dateTime : list2[0].startDatetime);
							list2.Sort((QuestStaticQuestGroup x, QuestStaticQuestGroup y) => y.endDatetime.CompareTo(x.endDatetime));
							dateTime2 = ((dateTime2 > list2[0].endDatetime) ? dateTime2 : list2[0].endDatetime);
						}
					}
					if (dateTime < TimeManager.Now && TimeManager.Now < dateTime2)
					{
						return true;
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x0600317A RID: 12666 RVA: 0x001BD568 File Offset: 0x001BB768
		public bool GrowthCharaSelectEnabled
		{
			get
			{
				using (List<int>.Enumerator enumerator = DataManager.DmQuest.GetPlayableMapIdList(this.eventChapterId).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int map = enumerator.Current;
						using (List<QuestStaticQuestGroup>.Enumerator enumerator2 = DataManager.DmQuest.QuestStaticData.groupDataList.FindAll((QuestStaticQuestGroup x) => x.mapId == map).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								QuestStaticQuestGroup group = enumerator2.Current;
								if (this.eventId == group.growthEventId)
								{
									foreach (QuestStaticQuestOne questStaticQuestOne in DataManager.DmQuest.QuestStaticData.oneDataList.FindAll((QuestStaticQuestOne x) => x.questGroupId == group.questGroupId))
									{
										if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questStaticQuestOne.questId) && 0 < DataManager.DmQuest.QuestDynamicData.oneDataMap[questStaticQuestOne.questId].clearNum)
										{
											return false;
										}
									}
								}
							}
						}
					}
				}
				return true;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x0600317B RID: 12667 RVA: 0x001BD710 File Offset: 0x001BB910
		// (set) Token: 0x0600317C RID: 12668 RVA: 0x001BD722 File Offset: 0x001BB922
		public string StoryFilename
		{
			get
			{
				return "Texture2D/EventTopPhoto/" + this.storyFilename;
			}
			private set
			{
				this.storyFilename = value;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x0600317D RID: 12669 RVA: 0x001BD72B File Offset: 0x001BB92B
		// (set) Token: 0x0600317E RID: 12670 RVA: 0x001BD73D File Offset: 0x001BB93D
		public string StoryFilename2
		{
			get
			{
				return "Texture2D/EventTopPhoto/" + this.storyFilename2;
			}
			private set
			{
				this.storyFilename2 = value;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600317F RID: 12671 RVA: 0x001BD746 File Offset: 0x001BB946
		// (set) Token: 0x06003180 RID: 12672 RVA: 0x001BD74E File Offset: 0x001BB94E
		public string MissionBannerFilename
		{
			get
			{
				return this.missionBannerFilename;
			}
			private set
			{
				this.missionBannerFilename = value;
			}
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x001BD758 File Offset: 0x001BB958
		public EventData(MstEventData mstEventData)
		{
			this.eventId = mstEventData.eventId;
			this.eventName = mstEventData.eventName;
			this.eventCategory = (DataManagerEvent.Category)mstEventData.eventCategory;
			this.eventBannerId = mstEventData.eventBannerId;
			this.eventChapterId = mstEventData.eventChapterId;
			this.hardopenQuestOneid = mstEventData.hardopenQuestid;
			this.extraopenQuestOneid = mstEventData.extraopenQuestid;
			this.eventMissionGroupId = mstEventData.eventMissionGroupId;
			this.eventGachaId = mstEventData.eventGachaId;
			this.ResetTime = new DateTime(1900, 1, 1, 0, 0, 0);
			if (!string.IsNullOrEmpty(mstEventData.resetTime))
			{
				string[] array = mstEventData.resetTime.Split(':', StringSplitOptions.None);
				if (3 == array.Length)
				{
					int num = int.Parse(array[0]);
					int num2 = int.Parse(array[1]);
					int num3 = int.Parse(array[2]);
					this.ResetTime = new DateTime(2000, 1, 1, num, num2, num3);
				}
			}
			this.eventShopIdList = new List<int>();
			if (0 < mstEventData.eventShopId)
			{
				this.eventShopIdList.Add(mstEventData.eventShopId);
			}
			if (0 < mstEventData.eventShopId2)
			{
				this.eventShopIdList.Add(mstEventData.eventShopId2);
			}
			this.eventCoinIdList = new List<int>();
			if (0 < mstEventData.eventCoinId)
			{
				this.eventCoinIdList.Add(mstEventData.eventCoinId);
			}
			if (0 < mstEventData.eventCoinId2)
			{
				this.eventCoinIdList.Add(mstEventData.eventCoinId2);
			}
			this.dispCharaId = mstEventData.dispCharaId;
			this.dispCharaBodyMotion = mstEventData.dispCharaBodyMotion;
			this.dispCharaFaceMotion = mstEventData.dispCharaFaceMotion;
			this.modeUIType = mstEventData.modeUiType;
			this.eventTitleScenario = mstEventData.eventTitleScenario;
			this.eventTitleScenario2 = mstEventData.eventTitleScenario2;
			this.StoryFilename = mstEventData.storyFilename;
			this.StoryFilename2 = mstEventData.storyFilename2;
			this.MissionBannerFilename = mstEventData.missionBannerFilename;
			this.missionIconFilename = mstEventData.missionIconFilename;
			this.bgFilename = mstEventData.bgFilename;
			this.bgFilename2 = mstEventData.bgFilename2;
			this.GrowthSelcharaRatio = mstEventData.growthSelcharaRatio;
			this.homeDispFlg = mstEventData.homeDispFlg != 0;
			this.raidFlg = mstEventData.raidFlg != 0;
			this.startDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstEventData.startDatetime));
			this.endDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstEventData.endDatetime));
			this.openKeyPaidItemID = mstEventData.paidItemId;
			this.eventImageDataList = new List<DataManagerEvent.EventImageData>();
		}

		// Token: 0x04002EE1 RID: 12001
		public int eventId;

		// Token: 0x04002EE2 RID: 12002
		public string eventName;

		// Token: 0x04002EE3 RID: 12003
		public DataManagerEvent.Category eventCategory;

		// Token: 0x04002EE4 RID: 12004
		public int eventBannerId;

		// Token: 0x04002EE5 RID: 12005
		public int eventChapterId;

		// Token: 0x04002EE6 RID: 12006
		public int hardopenQuestOneid;

		// Token: 0x04002EE7 RID: 12007
		public int extraopenQuestOneid;

		// Token: 0x04002EE8 RID: 12008
		public int eventMissionGroupId;

		// Token: 0x04002EE9 RID: 12009
		public List<int> missionIdList;

		// Token: 0x04002EEA RID: 12010
		public int eventGachaId;

		// Token: 0x04002EEB RID: 12011
		public List<int> eventShopIdList;

		// Token: 0x04002EEC RID: 12012
		public List<int> eventCoinIdList;

		// Token: 0x04002EED RID: 12013
		public int dispCharaId;

		// Token: 0x04002EEE RID: 12014
		public string dispCharaBodyMotion;

		// Token: 0x04002EEF RID: 12015
		public string dispCharaFaceMotion;

		// Token: 0x04002EF0 RID: 12016
		public int modeUIType;

		// Token: 0x04002EF1 RID: 12017
		public string eventTitleScenario;

		// Token: 0x04002EF2 RID: 12018
		public string eventTitleScenario2;

		// Token: 0x04002EF3 RID: 12019
		private string storyFilename;

		// Token: 0x04002EF4 RID: 12020
		private string storyFilename2;

		// Token: 0x04002EF5 RID: 12021
		private string missionBannerFilename;

		// Token: 0x04002EF6 RID: 12022
		public string bgFilename;

		// Token: 0x04002EF7 RID: 12023
		public string bgFilename2;

		// Token: 0x04002EF8 RID: 12024
		public string missionIconFilename;

		// Token: 0x04002EF9 RID: 12025
		public DateTime ResetTime;

		// Token: 0x04002F00 RID: 12032
		public List<DataManagerEvent.EventImageData> eventImageDataList;

		// Token: 0x04002F01 RID: 12033
		public DateTime startDatetime;

		// Token: 0x04002F02 RID: 12034
		public DateTime endDatetime;

		// Token: 0x04002F03 RID: 12035
		public int openKeyPaidItemID;

		// Token: 0x02001112 RID: 4370
		public class Bonus
		{
			// Token: 0x17000C27 RID: 3111
			// (get) Token: 0x0600547B RID: 21627 RVA: 0x0024DCD9 File Offset: 0x0024BED9
			// (set) Token: 0x0600547C RID: 21628 RVA: 0x0024DCE1 File Offset: 0x0024BEE1
			public int Id { get; private set; }

			// Token: 0x17000C28 RID: 3112
			// (get) Token: 0x0600547D RID: 21629 RVA: 0x0024DCEA File Offset: 0x0024BEEA
			// (set) Token: 0x0600547E RID: 21630 RVA: 0x0024DCF2 File Offset: 0x0024BEF2
			public int Ratio { get; private set; }

			// Token: 0x0600547F RID: 21631 RVA: 0x0024DCFB File Offset: 0x0024BEFB
			public Bonus(int id, int ratio)
			{
				this.Id = id;
				this.Ratio = ratio;
			}
		}

		// Token: 0x02001113 RID: 4371
		public class GrowthCharaData
		{
			// Token: 0x17000C29 RID: 3113
			// (get) Token: 0x06005480 RID: 21632 RVA: 0x0024DD11 File Offset: 0x0024BF11
			public int Id
			{
				get
				{
					if (this.growthChara == null)
					{
						return 0;
					}
					return this.growthChara.Id;
				}
			}

			// Token: 0x17000C2A RID: 3114
			// (get) Token: 0x06005481 RID: 21633 RVA: 0x0024DD28 File Offset: 0x0024BF28
			public int Ratio
			{
				get
				{
					if (this.growthChara == null)
					{
						return 0;
					}
					return this.growthChara.Ratio;
				}
			}

			// Token: 0x17000C2B RID: 3115
			// (get) Token: 0x06005482 RID: 21634 RVA: 0x0024DD3F File Offset: 0x0024BF3F
			// (set) Token: 0x06005483 RID: 21635 RVA: 0x0024DD47 File Offset: 0x0024BF47
			public DateTime charaSelectDatetime { get; private set; }

			// Token: 0x17000C2C RID: 3116
			// (get) Token: 0x06005484 RID: 21636 RVA: 0x0024DD50 File Offset: 0x0024BF50
			// (set) Token: 0x06005485 RID: 21637 RVA: 0x0024DD58 File Offset: 0x0024BF58
			public DateTime questClearDatetime { get; private set; }

			// Token: 0x06005486 RID: 21638 RVA: 0x0024DD64 File Offset: 0x0024BF64
			public GrowthCharaData(DataManagerEvent.EventData.Bonus bonus, long selectTime, long clearTime)
			{
				this.growthChara = bonus;
				this.charaSelectDatetime = ((selectTime == 0L) ? new DateTime(2000, 1, 1, 0, 0, 0) : new DateTime(PrjUtil.ConvertTimeToTicks(selectTime)));
				this.questClearDatetime = ((clearTime == 0L) ? new DateTime(2000, 1, 1, 0, 0, 0) : new DateTime(PrjUtil.ConvertTimeToTicks(clearTime)));
			}

			// Token: 0x04005DFE RID: 24062
			private DataManagerEvent.EventData.Bonus growthChara;
		}
	}

	// Token: 0x0200066B RID: 1643
	public class EventBannerData
	{
		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x001BD9CF File Offset: 0x001BBBCF
		// (set) Token: 0x06003183 RID: 12675 RVA: 0x001BD9D7 File Offset: 0x001BBBD7
		public int BannerId { get; private set; }

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06003184 RID: 12676 RVA: 0x001BD9E0 File Offset: 0x001BBBE0
		// (set) Token: 0x06003185 RID: 12677 RVA: 0x001BD9E8 File Offset: 0x001BBBE8
		public string BannerFilename { get; private set; }

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06003186 RID: 12678 RVA: 0x001BD9F1 File Offset: 0x001BBBF1
		public string BannerText
		{
			get
			{
				if (string.IsNullOrEmpty(this.bannerText))
				{
					return TimeManager.MakeTimeResidueText(TimeManager.Now, this.EndDatetime, false, true);
				}
				return this.bannerText;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06003187 RID: 12679 RVA: 0x001BDA19 File Offset: 0x001BBC19
		// (set) Token: 0x06003188 RID: 12680 RVA: 0x001BDA21 File Offset: 0x001BBC21
		public int StartQuestId { get; private set; }

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06003189 RID: 12681 RVA: 0x001BDA2A File Offset: 0x001BBC2A
		// (set) Token: 0x0600318A RID: 12682 RVA: 0x001BDA32 File Offset: 0x001BBC32
		public int EndQuestId { get; private set; }

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x0600318B RID: 12683 RVA: 0x001BDA3B File Offset: 0x001BBC3B
		// (set) Token: 0x0600318C RID: 12684 RVA: 0x001BDA43 File Offset: 0x001BBC43
		public DateTime StartDatetime { get; private set; }

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x0600318D RID: 12685 RVA: 0x001BDA4C File Offset: 0x001BBC4C
		// (set) Token: 0x0600318E RID: 12686 RVA: 0x001BDA54 File Offset: 0x001BBC54
		public DateTime EndDatetime { get; private set; }

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x0600318F RID: 12687 RVA: 0x001BDA5D File Offset: 0x001BBC5D
		// (set) Token: 0x06003190 RID: 12688 RVA: 0x001BDA65 File Offset: 0x001BBC65
		public DataManagerEvent.EventBannerData.Type LinkType { get; private set; }

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06003191 RID: 12689 RVA: 0x001BDA6E File Offset: 0x001BBC6E
		// (set) Token: 0x06003192 RID: 12690 RVA: 0x001BDA76 File Offset: 0x001BBC76
		public string LinkAddress { get; private set; }

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06003193 RID: 12691 RVA: 0x001BDA7F File Offset: 0x001BBC7F
		// (set) Token: 0x06003194 RID: 12692 RVA: 0x001BDA87 File Offset: 0x001BBC87
		public int LinkValue { get; private set; }

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06003195 RID: 12693 RVA: 0x001BDA90 File Offset: 0x001BBC90
		// (set) Token: 0x06003196 RID: 12694 RVA: 0x001BDA98 File Offset: 0x001BBC98
		public int Priority { get; private set; }

		// Token: 0x06003197 RID: 12695 RVA: 0x001BDAA4 File Offset: 0x001BBCA4
		public EventBannerData(MstEventBannerData mstEventBannerData)
		{
			this.BannerId = mstEventBannerData.id;
			this.BannerFilename = "Texture2D/HomeBigBanner/home_bigbanner_" + mstEventBannerData.bannerName;
			this.bannerText = mstEventBannerData.bannerText;
			this.StartQuestId = mstEventBannerData.startQuestId;
			this.EndQuestId = mstEventBannerData.endQuestId;
			this.StartDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstEventBannerData.startTime));
			this.EndDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstEventBannerData.endTime));
			this.LinkAddress = mstEventBannerData.linkAdress;
			this.LinkType = (DataManagerEvent.EventBannerData.Type)mstEventBannerData.linkType;
			this.LinkValue = mstEventBannerData.linkValue;
			this.Priority = mstEventBannerData.priority;
		}

		// Token: 0x04002F06 RID: 12038
		private string bannerText;

		// Token: 0x02001119 RID: 4377
		public enum Type
		{
			// Token: 0x04005E09 RID: 24073
			Invalid,
			// Token: 0x04005E0A RID: 24074
			Move,
			// Token: 0x04005E0B RID: 24075
			WebView,
			// Token: 0x04005E0C RID: 24076
			Browser,
			// Token: 0x04005E0D RID: 24077
			HomeInfo,
			// Token: 0x04005E0E RID: 24078
			Noah,
			// Token: 0x04005E0F RID: 24079
			AtomInvite,
			// Token: 0x04005E10 RID: 24080
			FriendInvite
		}
	}

	// Token: 0x0200066C RID: 1644
	public class EventImageData
	{
		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06003198 RID: 12696 RVA: 0x001BDB59 File Offset: 0x001BBD59
		// (set) Token: 0x06003199 RID: 12697 RVA: 0x001BDB61 File Offset: 0x001BBD61
		public int EventId { get; private set; }

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x0600319A RID: 12698 RVA: 0x001BDB6A File Offset: 0x001BBD6A
		// (set) Token: 0x0600319B RID: 12699 RVA: 0x001BDB72 File Offset: 0x001BBD72
		public DataManagerEvent.EventImageData.ImageType Type { get; private set; }

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x0600319C RID: 12700 RVA: 0x001BDB7B File Offset: 0x001BBD7B
		// (set) Token: 0x0600319D RID: 12701 RVA: 0x001BDB83 File Offset: 0x001BBD83
		public int Sort { get; private set; }

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x0600319E RID: 12702 RVA: 0x001BDB8C File Offset: 0x001BBD8C
		// (set) Token: 0x0600319F RID: 12703 RVA: 0x001BDB94 File Offset: 0x001BBD94
		public string ImagePath { get; private set; }

		// Token: 0x060031A0 RID: 12704 RVA: 0x001BDB9D File Offset: 0x001BBD9D
		public EventImageData(MstEventImageData mstEvImgData)
		{
			this.EventId = mstEvImgData.eventId;
			this.Type = (DataManagerEvent.EventImageData.ImageType)mstEvImgData.type;
			this.Sort = mstEvImgData.sort;
			this.ImagePath = mstEvImgData.imagePath;
		}

		// Token: 0x0200111A RID: 4378
		public enum ImageType
		{
			// Token: 0x04005E12 RID: 24082
			Undefined,
			// Token: 0x04005E13 RID: 24083
			Tips,
			// Token: 0x04005E14 RID: 24084
			PickUp
		}
	}

	// Token: 0x0200066D RID: 1645
	public class LargeEventData
	{
		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060031A1 RID: 12705 RVA: 0x001BDBD5 File Offset: 0x001BBDD5
		// (set) Token: 0x060031A2 RID: 12706 RVA: 0x001BDBDD File Offset: 0x001BBDDD
		public int EventID { get; private set; }

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060031A3 RID: 12707 RVA: 0x001BDBE6 File Offset: 0x001BBDE6
		// (set) Token: 0x060031A4 RID: 12708 RVA: 0x001BDBEE File Offset: 0x001BBDEE
		public Vector2Int MapDirection { get; private set; }

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060031A5 RID: 12709 RVA: 0x001BDBF7 File Offset: 0x001BBDF7
		// (set) Token: 0x060031A6 RID: 12710 RVA: 0x001BDBFF File Offset: 0x001BBDFF
		public Vector2Int MapRangeOrigin { get; private set; }

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060031A7 RID: 12711 RVA: 0x001BDC08 File Offset: 0x001BBE08
		// (set) Token: 0x060031A8 RID: 12712 RVA: 0x001BDC10 File Offset: 0x001BBE10
		public Vector2Int MapRangeSize { get; private set; }

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060031A9 RID: 12713 RVA: 0x001BDC19 File Offset: 0x001BBE19
		// (set) Token: 0x060031AA RID: 12714 RVA: 0x001BDC21 File Offset: 0x001BBE21
		public Vector2 MapOffset { get; private set; }

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060031AB RID: 12715 RVA: 0x001BDC2A File Offset: 0x001BBE2A
		// (set) Token: 0x060031AC RID: 12716 RVA: 0x001BDC32 File Offset: 0x001BBE32
		public List<string> TipsFilePath { get; private set; }

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060031AD RID: 12717 RVA: 0x001BDC3B File Offset: 0x001BBE3B
		// (set) Token: 0x060031AE RID: 12718 RVA: 0x001BDC43 File Offset: 0x001BBE43
		public string MapFilePath { get; private set; }

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060031AF RID: 12719 RVA: 0x001BDC4C File Offset: 0x001BBE4C
		// (set) Token: 0x060031B0 RID: 12720 RVA: 0x001BDC54 File Offset: 0x001BBE54
		public List<DataManagerEvent.LargeEventData.MapFileData> MapFileDataList { get; set; }

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060031B1 RID: 12721 RVA: 0x001BDC5D File Offset: 0x001BBE5D
		// (set) Token: 0x060031B2 RID: 12722 RVA: 0x001BDC65 File Offset: 0x001BBE65
		public string BgmFilePath { get; private set; }

		// Token: 0x060031B3 RID: 12723 RVA: 0x001BDC70 File Offset: 0x001BBE70
		public LargeEventData(MstEventLargeEventData mstEvLargeEvData)
		{
			this.EventID = mstEvLargeEvData.eventId;
			this.MapDirection = new Vector2Int(mstEvLargeEvData.mapMoveX, mstEvLargeEvData.mapMoveY);
			this.MapRangeOrigin = new Vector2Int(mstEvLargeEvData.mapRangeOriginX, mstEvLargeEvData.mapRangeOriginY);
			this.MapRangeSize = new Vector2Int(mstEvLargeEvData.mapRangeWidth, mstEvLargeEvData.mapRangeHeight);
			this.MapOffset = new Vector2((float)mstEvLargeEvData.mapOffsetX, (float)mstEvLargeEvData.mapOffsetY);
			this.MapFilePath = mstEvLargeEvData.mapFilepath;
			this.TipsFilePath = new List<string> { mstEvLargeEvData.tipsFilepath01, mstEvLargeEvData.tipsFilepath02, mstEvLargeEvData.tipsFilepath03, mstEvLargeEvData.tipsFilepath04 };
			this.TipsFilePath.RemoveAll((string x) => string.Empty == x);
			this.MapFileDataList = new List<DataManagerEvent.LargeEventData.MapFileData>
			{
				new DataManagerEvent.LargeEventData.MapFileData
				{
					filepath = mstEvLargeEvData.changemapFilepath01,
					openQuestOneId = 0
				},
				new DataManagerEvent.LargeEventData.MapFileData
				{
					filepath = mstEvLargeEvData.changemapFilepath02,
					openQuestOneId = mstEvLargeEvData.changemapQuest01
				},
				new DataManagerEvent.LargeEventData.MapFileData
				{
					filepath = mstEvLargeEvData.changemapFilepath03,
					openQuestOneId = mstEvLargeEvData.changemapQuest02
				}
			};
			this.BgmFilePath = mstEvLargeEvData.bgmFilepath;
		}

		// Token: 0x0200111B RID: 4379
		public class MapFileData
		{
			// Token: 0x04005E15 RID: 24085
			public string filepath;

			// Token: 0x04005E16 RID: 24086
			public int openQuestOneId;
		}
	}

	// Token: 0x0200066E RID: 1646
	public class PeriodData
	{
		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060031B4 RID: 12724 RVA: 0x001BDDDA File Offset: 0x001BBFDA
		// (set) Token: 0x060031B5 RID: 12725 RVA: 0x001BDDE2 File Offset: 0x001BBFE2
		public int EventId { get; private set; }

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060031B6 RID: 12726 RVA: 0x001BDDEB File Offset: 0x001BBFEB
		// (set) Token: 0x060031B7 RID: 12727 RVA: 0x001BDDF3 File Offset: 0x001BBFF3
		public DateTime StartDatetime { get; private set; }

		// Token: 0x060031B8 RID: 12728 RVA: 0x001BDDFC File Offset: 0x001BBFFC
		public PeriodData(MstEventPeriodData mstPeriodData)
		{
			this.EventId = mstPeriodData.eventId;
			this.StartDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstPeriodData.startDatetime));
		}
	}

	// Token: 0x0200066F RID: 1647
	public class CoopConditionData
	{
		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060031B9 RID: 12729 RVA: 0x001BDE26 File Offset: 0x001BC026
		public int ConditionId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060031BA RID: 12730 RVA: 0x001BDE29 File Offset: 0x001BC029
		// (set) Token: 0x060031BB RID: 12731 RVA: 0x001BDE31 File Offset: 0x001BC031
		public int EventId { get; private set; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060031BC RID: 12732 RVA: 0x001BDE3A File Offset: 0x001BC03A
		// (set) Token: 0x060031BD RID: 12733 RVA: 0x001BDE42 File Offset: 0x001BC042
		public int Level { get; private set; }

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060031BE RID: 12734 RVA: 0x001BDE4B File Offset: 0x001BC04B
		// (set) Token: 0x060031BF RID: 12735 RVA: 0x001BDE53 File Offset: 0x001BC053
		public string LevelName { get; private set; }

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060031C0 RID: 12736 RVA: 0x001BDE5C File Offset: 0x001BC05C
		// (set) Token: 0x060031C1 RID: 12737 RVA: 0x001BDE64 File Offset: 0x001BC064
		public int MapId { get; private set; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060031C2 RID: 12738 RVA: 0x001BDE6D File Offset: 0x001BC06D
		// (set) Token: 0x060031C3 RID: 12739 RVA: 0x001BDE75 File Offset: 0x001BC075
		public DataManagerEvent.RewardType RewardType { get; private set; }

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060031C4 RID: 12740 RVA: 0x001BDE7E File Offset: 0x001BC07E
		// (set) Token: 0x060031C5 RID: 12741 RVA: 0x001BDE86 File Offset: 0x001BC086
		public long AchievementCondition { get; private set; }

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060031C6 RID: 12742 RVA: 0x001BDE8F File Offset: 0x001BC08F
		public bool IsStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060031C7 RID: 12743 RVA: 0x001BDE92 File Offset: 0x001BC092
		// (set) Token: 0x060031C8 RID: 12744 RVA: 0x001BDE9A File Offset: 0x001BC09A
		public ItemInput AchievementItem { get; private set; }

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060031C9 RID: 12745 RVA: 0x001BDEA3 File Offset: 0x001BC0A3
		// (set) Token: 0x060031CA RID: 12746 RVA: 0x001BDEAB File Offset: 0x001BC0AB
		public string TexturePath { get; private set; }

		// Token: 0x060031CB RID: 12747 RVA: 0x001BDEB4 File Offset: 0x001BC0B4
		public CoopConditionData(MstEventCoopConditionData mst)
		{
			this.EventId = mst.eventId;
			this.MapId = mst.questMapId;
			this.Level = mst.level;
			this.LevelName = mst.levelName;
			this.AchievementCondition = mst.achieveCondition;
			this.RewardType = (DataManagerEvent.RewardType)mst.type;
			this.AchievementItem = new ItemInput(mst.itemId, mst.itemNum);
			this.TexturePath = mst.texturePath;
		}
	}

	// Token: 0x02000670 RID: 1648
	public class CoopHardQuestData
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060031CC RID: 12748 RVA: 0x001BDF32 File Offset: 0x001BC132
		// (set) Token: 0x060031CD RID: 12749 RVA: 0x001BDF3A File Offset: 0x001BC13A
		public int EventId { get; private set; }

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060031CE RID: 12750 RVA: 0x001BDF43 File Offset: 0x001BC143
		// (set) Token: 0x060031CF RID: 12751 RVA: 0x001BDF4B File Offset: 0x001BC14B
		public int MapId { get; private set; }

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060031D0 RID: 12752 RVA: 0x001BDF54 File Offset: 0x001BC154
		// (set) Token: 0x060031D1 RID: 12753 RVA: 0x001BDF5C File Offset: 0x001BC15C
		public DateTime StartDatetime { get; private set; }

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060031D2 RID: 12754 RVA: 0x001BDF65 File Offset: 0x001BC165
		// (set) Token: 0x060031D3 RID: 12755 RVA: 0x001BDF6D File Offset: 0x001BC16D
		public long Starttime { get; private set; }

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060031D4 RID: 12756 RVA: 0x001BDF76 File Offset: 0x001BC176
		// (set) Token: 0x060031D5 RID: 12757 RVA: 0x001BDF7E File Offset: 0x001BC17E
		public DateTime EndDatetime { get; private set; }

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060031D6 RID: 12758 RVA: 0x001BDF87 File Offset: 0x001BC187
		// (set) Token: 0x060031D7 RID: 12759 RVA: 0x001BDF8F File Offset: 0x001BC18F
		public int AchievementCondition { get; private set; }

		// Token: 0x060031D8 RID: 12760 RVA: 0x001BDF98 File Offset: 0x001BC198
		public CoopHardQuestData(MstEventCoopHardQuestData mst)
		{
			this.EventId = mst.eventId;
			this.MapId = mst.questMapId;
			this.StartDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startDatetime));
			this.Starttime = mst.startDatetime;
			this.EndDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.endDatetime));
			this.AchievementCondition = mst.achieveCondition;
		}
	}

	// Token: 0x02000671 RID: 1649
	public class ReleaseEffects
	{
		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060031D9 RID: 12761 RVA: 0x001BE007 File Offset: 0x001BC207
		// (set) Token: 0x060031DA RID: 12762 RVA: 0x001BE00F File Offset: 0x001BC20F
		public int EventId { get; private set; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060031DB RID: 12763 RVA: 0x001BE018 File Offset: 0x001BC218
		// (set) Token: 0x060031DC RID: 12764 RVA: 0x001BE020 File Offset: 0x001BC220
		public DataManagerEvent.Category Category { get; private set; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060031DD RID: 12765 RVA: 0x001BE029 File Offset: 0x001BC229
		// (set) Token: 0x060031DE RID: 12766 RVA: 0x001BE031 File Offset: 0x001BC231
		public int TutorialPhase { get; set; }

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060031DF RID: 12767 RVA: 0x001BE03A File Offset: 0x001BC23A
		// (set) Token: 0x060031E0 RID: 12768 RVA: 0x001BE042 File Offset: 0x001BC242
		public List<int> ReleaseIdList { get; set; }

		// Token: 0x060031E1 RID: 12769 RVA: 0x001BE04C File Offset: 0x001BC24C
		public ReleaseEffects(DataManagerEvent.EventData evData)
		{
			if (evData == null)
			{
				this.EventId = 0;
				this.Category = DataManagerEvent.Category.INVARID;
				this.TutorialPhase = 0;
				this.ReleaseIdList = new List<int>();
				return;
			}
			this.EventId = evData.eventId;
			this.Category = evData.eventCategory;
			this.TutorialPhase = 0;
			this.ReleaseIdList = new List<int>();
			DataManagerEvent.Category eventCategory = evData.eventCategory;
			if (eventCategory == DataManagerEvent.Category.Scenario || eventCategory == DataManagerEvent.Category.Tower)
			{
				this.ReleaseIdList.AddRange(new List<int> { 0, 0 });
			}
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x001BE0DA File Offset: 0x001BC2DA
		public DataManagerEvent.ReleaseEffects Clone()
		{
			return new DataManagerEvent.ReleaseEffects(DataManager.DmEvent.GetEventData(this.EventId))
			{
				TutorialPhase = this.TutorialPhase,
				ReleaseIdList = new List<int>(this.ReleaseIdList)
			};
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x001BE110 File Offset: 0x001BC310
		public List<NewFlg> ServerData(DataManager.NewFlgCategory evCategory)
		{
			int num = 0;
			List<NewFlg> list = new List<NewFlg>();
			int num2 = 0;
			list.Add(new NewFlg
			{
				any_id = num2,
				category = (int)evCategory,
				new_mgmt_flg = this.EventId
			});
			num2++;
			list.Add(new NewFlg
			{
				any_id = num2,
				category = (int)evCategory,
				new_mgmt_flg = this.TutorialPhase
			});
			num2++;
			foreach (int num3 in this.ReleaseIdList)
			{
				list.Add(new NewFlg
				{
					any_id = num2,
					category = (int)evCategory,
					new_mgmt_flg = num3
				});
				num2++;
			}
			num++;
			return list;
		}
	}

	// Token: 0x02000672 RID: 1650
	public enum Category
	{
		// Token: 0x04002F31 RID: 12081
		INVARID,
		// Token: 0x04002F32 RID: 12082
		Scenario,
		// Token: 0x04002F33 RID: 12083
		Growth,
		// Token: 0x04002F34 RID: 12084
		Large,
		// Token: 0x04002F35 RID: 12085
		Mission,
		// Token: 0x04002F36 RID: 12086
		Tower,
		// Token: 0x04002F37 RID: 12087
		Coop,
		// Token: 0x04002F38 RID: 12088
		WildRelease,
		// Token: 0x04002F39 RID: 12089
		SpecialPvp
	}

	// Token: 0x02000673 RID: 1651
	public enum CoopType
	{
		// Token: 0x04002F3B RID: 12091
		All,
		// Token: 0x04002F3C RID: 12092
		Normal,
		// Token: 0x04002F3D RID: 12093
		Highdifficulty
	}

	// Token: 0x02000674 RID: 1652
	public enum RewardType
	{
		// Token: 0x04002F3F RID: 12095
		Undefined,
		// Token: 0x04002F40 RID: 12096
		BonusReward,
		// Token: 0x04002F41 RID: 12097
		PointReward,
		// Token: 0x04002F42 RID: 12098
		ReleaseReward
	}

	// Token: 0x02000675 RID: 1653
	public class CoopData
	{
		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060031E4 RID: 12772 RVA: 0x001BE1E4 File Offset: 0x001BC3E4
		// (set) Token: 0x060031E5 RID: 12773 RVA: 0x001BE1EC File Offset: 0x001BC3EC
		public int EventId { get; private set; }

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060031E6 RID: 12774 RVA: 0x001BE1F5 File Offset: 0x001BC3F5
		// (set) Token: 0x060031E7 RID: 12775 RVA: 0x001BE1FD File Offset: 0x001BC3FD
		public int EventItemId { get; private set; }

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060031E8 RID: 12776 RVA: 0x001BE206 File Offset: 0x001BC406
		// (set) Token: 0x060031E9 RID: 12777 RVA: 0x001BE20E File Offset: 0x001BC40E
		public int EventItemNum { get; private set; }

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060031EA RID: 12778 RVA: 0x001BE217 File Offset: 0x001BC417
		// (set) Token: 0x060031EB RID: 12779 RVA: 0x001BE21F File Offset: 0x001BC41F
		public List<DataManagerEvent.CoopData.DispLog> DispLogList { get; private set; }

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060031EC RID: 12780 RVA: 0x001BE228 File Offset: 0x001BC428
		// (set) Token: 0x060031ED RID: 12781 RVA: 0x001BE230 File Offset: 0x001BC430
		public Dictionary<int, DataManagerEvent.CoopData.MapInfo> MapInfoMap { get; set; }

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060031EE RID: 12782 RVA: 0x001BE239 File Offset: 0x001BC439
		// (set) Token: 0x060031EF RID: 12783 RVA: 0x001BE241 File Offset: 0x001BC441
		public List<Quest> QuestList { get; set; }

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060031F0 RID: 12784 RVA: 0x001BE24A File Offset: 0x001BC44A
		// (set) Token: 0x060031F1 RID: 12785 RVA: 0x001BE252 File Offset: 0x001BC452
		public DataManagerEvent.CoopData.HardQuestEndInfo HardQuestEndInfoData { get; private set; }

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060031F2 RID: 12786 RVA: 0x001BE25B File Offset: 0x001BC45B
		// (set) Token: 0x060031F3 RID: 12787 RVA: 0x001BE263 File Offset: 0x001BC463
		public long InfoGetTime { get; private set; }

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060031F4 RID: 12788 RVA: 0x001BE26C File Offset: 0x001BC46C
		public DateTime InfoGetDateTime
		{
			get
			{
				return new DateTime(PrjUtil.ConvertTimeToTicks(this.InfoGetTime));
			}
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x001BE280 File Offset: 0x001BC480
		public CoopData(CoopInfoResponse coopRes, int evId)
		{
			this.EventId = evId;
			this.EventItemId = ((DataManager.DmEvent.GetEventData(this.EventId).eventCoinIdList != null) ? ((0 < DataManager.DmEvent.GetEventData(this.EventId).eventCoinIdList.Count) ? DataManager.DmEvent.GetEventData(this.EventId).eventCoinIdList[0] : 0) : 0);
			this.EventItemNum = coopRes.event_item_num;
			this.DispLogList = new List<DataManagerEvent.CoopData.DispLog>();
			if (coopRes.log_list != null)
			{
				foreach (CoopLog coopLog in coopRes.log_list)
				{
					if (DataManager.DmEvent.GetEventData(this.EventId).raidFlg)
					{
						DataManagerEvent.CoopRaidTermData nowTermData = DataManager.DmEvent.GetNowTermData(this.EventId);
						DateTime dateTime = new DateTime(PrjUtil.ConvertTimeToTicks(coopLog.start_time));
						if (!(dateTime.Date != TimeManager.Now.Date) && (nowTermData.IsOverStartTime(dateTime) && !nowTermData.IsOverEndTime(dateTime)) && !DataManager.DmEvent.isRaidBonusMapId(coopLog.map_id))
						{
							this.DispLogList.Add(new DataManagerEvent.CoopData.DispLog(coopLog));
						}
					}
					else
					{
						this.DispLogList.Add(new DataManagerEvent.CoopData.DispLog(coopLog));
					}
				}
			}
			this.QuestList = coopRes.quests;
			this.MapInfoMap = new Dictionary<int, DataManagerEvent.CoopData.MapInfo>();
			if (coopRes.map_info_list != null)
			{
				foreach (CoopMapInfo coopMapInfo in coopRes.map_info_list)
				{
					this.MapInfoMap.Add(coopMapInfo.map_id, new DataManagerEvent.CoopData.MapInfo(coopMapInfo, evId));
				}
			}
			this.InfoGetTime = coopRes.get_info_time;
			this.HardQuestEndInfoData = new DataManagerEvent.CoopData.HardQuestEndInfo(coopRes.hard_quest_end_info);
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x001BE494 File Offset: 0x001BC694
		public void UpdateMapData(CoopInfoResponse coopRes, int evId)
		{
			if (coopRes.map_info_list != null)
			{
				foreach (CoopMapInfo coopMapInfo in coopRes.map_info_list)
				{
					if (this.MapInfoMap.ContainsKey(coopMapInfo.map_id))
					{
						this.MapInfoMap[coopMapInfo.map_id] = new DataManagerEvent.CoopData.MapInfo(coopMapInfo, evId);
					}
					else
					{
						this.MapInfoMap.Add(coopMapInfo.map_id, new DataManagerEvent.CoopData.MapInfo(coopMapInfo, evId));
					}
				}
			}
			this.InfoGetTime = coopRes.get_info_time;
		}

		// Token: 0x0200111D RID: 4381
		public class DispLog
		{
			// Token: 0x17000C2D RID: 3117
			// (get) Token: 0x06005497 RID: 21655 RVA: 0x0024DEB9 File Offset: 0x0024C0B9
			// (set) Token: 0x06005498 RID: 21656 RVA: 0x0024DEC1 File Offset: 0x0024C0C1
			public string UserName { get; private set; }

			// Token: 0x17000C2E RID: 3118
			// (get) Token: 0x06005499 RID: 21657 RVA: 0x0024DECA File Offset: 0x0024C0CA
			// (set) Token: 0x0600549A RID: 21658 RVA: 0x0024DED2 File Offset: 0x0024C0D2
			public int MapId { get; private set; }

			// Token: 0x17000C2F RID: 3119
			// (get) Token: 0x0600549B RID: 21659 RVA: 0x0024DEDB File Offset: 0x0024C0DB
			// (set) Token: 0x0600549C RID: 21660 RVA: 0x0024DEE3 File Offset: 0x0024C0E3
			public int Point { get; private set; }

			// Token: 0x17000C30 RID: 3120
			// (get) Token: 0x0600549D RID: 21661 RVA: 0x0024DEEC File Offset: 0x0024C0EC
			// (set) Token: 0x0600549E RID: 21662 RVA: 0x0024DEF4 File Offset: 0x0024C0F4
			public DateTime InsertTime { get; private set; }

			// Token: 0x0600549F RID: 21663 RVA: 0x0024DEFD File Offset: 0x0024C0FD
			public DispLog(CoopLog log)
			{
				this.UserName = log.user_name;
				this.MapId = log.map_id;
				this.Point = log.point;
				this.InsertTime = new DateTime(log.start_time);
			}
		}

		// Token: 0x0200111E RID: 4382
		public class MapInfo
		{
			// Token: 0x17000C31 RID: 3121
			// (get) Token: 0x060054A0 RID: 21664 RVA: 0x0024DF3A File Offset: 0x0024C13A
			// (set) Token: 0x060054A1 RID: 21665 RVA: 0x0024DF42 File Offset: 0x0024C142
			public int EventId { get; private set; }

			// Token: 0x17000C32 RID: 3122
			// (get) Token: 0x060054A2 RID: 21666 RVA: 0x0024DF4B File Offset: 0x0024C14B
			// (set) Token: 0x060054A3 RID: 21667 RVA: 0x0024DF53 File Offset: 0x0024C153
			public int MapId { get; private set; }

			// Token: 0x17000C33 RID: 3123
			// (get) Token: 0x060054A4 RID: 21668 RVA: 0x0024DF5C File Offset: 0x0024C15C
			// (set) Token: 0x060054A5 RID: 21669 RVA: 0x0024DF64 File Offset: 0x0024C164
			public int Level { get; private set; }

			// Token: 0x17000C34 RID: 3124
			// (get) Token: 0x060054A6 RID: 21670 RVA: 0x0024DF6D File Offset: 0x0024C16D
			// (set) Token: 0x060054A7 RID: 21671 RVA: 0x0024DF75 File Offset: 0x0024C175
			public long TotalPoint { get; private set; }

			// Token: 0x17000C35 RID: 3125
			// (get) Token: 0x060054A8 RID: 21672 RVA: 0x0024DF7E File Offset: 0x0024C17E
			// (set) Token: 0x060054A9 RID: 21673 RVA: 0x0024DF86 File Offset: 0x0024C186
			public long EndPoint { get; set; }

			// Token: 0x17000C36 RID: 3126
			// (get) Token: 0x060054AA RID: 21674 RVA: 0x0024DF90 File Offset: 0x0024C190
			public float ProgressRate
			{
				get
				{
					float num = (float)this.TotalPoint / (float)this.EndPoint;
					return Mathf.Min(1f, num);
				}
			}

			// Token: 0x17000C37 RID: 3127
			// (get) Token: 0x060054AB RID: 21675 RVA: 0x0024DFB8 File Offset: 0x0024C1B8
			public bool IsClear
			{
				get
				{
					return this.EndPoint <= this.TotalPoint;
				}
			}

			// Token: 0x17000C38 RID: 3128
			// (get) Token: 0x060054AC RID: 21676 RVA: 0x0024DFCB File Offset: 0x0024C1CB
			// (set) Token: 0x060054AD RID: 21677 RVA: 0x0024DFD3 File Offset: 0x0024C1D3
			public bool IsHardQuestOpen { get; private set; }

			// Token: 0x17000C39 RID: 3129
			// (get) Token: 0x060054AE RID: 21678 RVA: 0x0024DFDC File Offset: 0x0024C1DC
			// (set) Token: 0x060054AF RID: 21679 RVA: 0x0024DFE4 File Offset: 0x0024C1E4
			public int HardQuestClearNum { get; private set; }

			// Token: 0x17000C3A RID: 3130
			// (get) Token: 0x060054B0 RID: 21680 RVA: 0x0024DFED File Offset: 0x0024C1ED
			// (set) Token: 0x060054B1 RID: 21681 RVA: 0x0024DFF5 File Offset: 0x0024C1F5
			public long HardQuestStartTime { get; private set; }

			// Token: 0x17000C3B RID: 3131
			// (get) Token: 0x060054B2 RID: 21682 RVA: 0x0024DFFE File Offset: 0x0024C1FE
			// (set) Token: 0x060054B3 RID: 21683 RVA: 0x0024E006 File Offset: 0x0024C206
			public int BonusDefeatedCount { get; private set; }

			// Token: 0x17000C3C RID: 3132
			// (get) Token: 0x060054B4 RID: 21684 RVA: 0x0024E00F File Offset: 0x0024C20F
			// (set) Token: 0x060054B5 RID: 21685 RVA: 0x0024E017 File Offset: 0x0024C217
			public CoopPlayerInfo HardClearUser { get; private set; }

			// Token: 0x17000C3D RID: 3133
			// (get) Token: 0x060054B6 RID: 21686 RVA: 0x0024E020 File Offset: 0x0024C220
			// (set) Token: 0x060054B7 RID: 21687 RVA: 0x0024E028 File Offset: 0x0024C228
			public List<DataManagerEvent.CoopData.MapInfo.RankingInfo> RankingInfoList { get; set; }

			// Token: 0x17000C3E RID: 3134
			// (get) Token: 0x060054B8 RID: 21688 RVA: 0x0024E034 File Offset: 0x0024C234
			public List<CoopPlayerInfo> RaidRankingList
			{
				get
				{
					List<CoopPlayerInfo> list = new List<CoopPlayerInfo>();
					foreach (DataManagerEvent.CoopData.MapInfo.RankingInfo rankingInfo in this.RankingInfoList)
					{
						list.AddRange(rankingInfo.UserRankingList);
					}
					return list;
				}
			}

			// Token: 0x17000C3F RID: 3135
			// (get) Token: 0x060054B9 RID: 21689 RVA: 0x0024E094 File Offset: 0x0024C294
			// (set) Token: 0x060054BA RID: 21690 RVA: 0x0024E09C File Offset: 0x0024C29C
			public DataManagerEvent.CoopConditionData StaticNextCoopConditionData { get; private set; }

			// Token: 0x17000C40 RID: 3136
			// (get) Token: 0x060054BB RID: 21691 RVA: 0x0024E0A5 File Offset: 0x0024C2A5
			// (set) Token: 0x060054BC RID: 21692 RVA: 0x0024E0AD File Offset: 0x0024C2AD
			public DataManagerEvent.CoopHardQuestData StaticCoopHardQuestData { get; private set; }

			// Token: 0x17000C41 RID: 3137
			// (get) Token: 0x060054BD RID: 21693 RVA: 0x0024E0B8 File Offset: 0x0024C2B8
			public List<DataManagerEvent.CoopConditionData> MapRewardConditionalDataList
			{
				get
				{
					List<DataManagerEvent.CoopConditionData> list = DataManager.DmEvent.coopConditionDataList.FindAll((DataManagerEvent.CoopConditionData x) => x.MapId == this.MapId && x.Level == this.Level);
					list.Sort((DataManagerEvent.CoopConditionData a, DataManagerEvent.CoopConditionData b) => a.AchievementCondition.CompareTo(b.AchievementCondition));
					return list;
				}
			}

			// Token: 0x060054BE RID: 21694 RVA: 0x0024E108 File Offset: 0x0024C308
			public MapInfo(CoopMapInfo mapInfo, int evId)
			{
				DataManagerEvent.CoopData.MapInfo <>4__this = this;
				this.EventId = evId;
				this.MapId = mapInfo.map_id;
				this.Level = mapInfo.level;
				this.TotalPoint = mapInfo.total_point;
				this.IsHardQuestOpen = 1 == mapInfo.hard_quest_open_flg;
				this.HardQuestClearNum = mapInfo.hard_quest_clear_num;
				this.HardQuestStartTime = mapInfo.hard_quest_start_time;
				this.HardClearUser = mapInfo.hard_quest_clear_user;
				this.RankingInfoList = new List<DataManagerEvent.CoopData.MapInfo.RankingInfo>();
				this.BonusDefeatedCount = mapInfo.bonus_defeated_count;
				if (mapInfo.ranking_list != null)
				{
					foreach (CoopRanking coopRanking in mapInfo.ranking_list)
					{
						this.RankingInfoList.Add(new DataManagerEvent.CoopData.MapInfo.RankingInfo(coopRanking));
					}
				}
				DataManagerEvent.CoopConditionData coopConditionData = DataManager.DmEvent.coopConditionDataList.Find((DataManagerEvent.CoopConditionData x) => <>4__this.EventId == x.EventId && <>4__this.MapId == x.MapId && <>4__this.Level == x.Level && x.RewardType == DataManagerEvent.RewardType.ReleaseReward);
				this.EndPoint = ((coopConditionData != null) ? coopConditionData.AchievementCondition : 1L);
				List<DataManagerEvent.CoopConditionData> list = DataManager.DmEvent.coopConditionDataList.FindAll((DataManagerEvent.CoopConditionData x) => <>4__this.EventId == x.EventId && <>4__this.MapId == x.MapId && <>4__this.Level == x.Level);
				list.Sort((DataManagerEvent.CoopConditionData a, DataManagerEvent.CoopConditionData b) => a.AchievementCondition.CompareTo(b.AchievementCondition));
				DataManagerEvent.CoopConditionData coopConditionData2 = null;
				using (List<DataManagerEvent.CoopConditionData>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						coopConditionData2 = enumerator2.Current;
						if (coopConditionData2.AchievementCondition >= this.TotalPoint)
						{
							break;
						}
					}
				}
				this.StaticNextCoopConditionData = coopConditionData2;
				this.StaticCoopHardQuestData = DataManager.DmEvent.coopHardQuestDataList.Find((DataManagerEvent.CoopHardQuestData x) => mapInfo.map_id == x.MapId && mapInfo.hard_quest_start_time == x.Starttime);
			}

			// Token: 0x0200122E RID: 4654
			public class RankingInfo
			{
				// Token: 0x17000D0D RID: 3341
				// (get) Token: 0x06005806 RID: 22534 RVA: 0x0025A179 File Offset: 0x00258379
				// (set) Token: 0x06005807 RID: 22535 RVA: 0x0025A181 File Offset: 0x00258381
				public DateTime RankedTime { get; private set; }

				// Token: 0x17000D0E RID: 3342
				// (get) Token: 0x06005808 RID: 22536 RVA: 0x0025A18A File Offset: 0x0025838A
				// (set) Token: 0x06005809 RID: 22537 RVA: 0x0025A192 File Offset: 0x00258392
				public int MyPoint { get; private set; }

				// Token: 0x17000D0F RID: 3343
				// (get) Token: 0x0600580A RID: 22538 RVA: 0x0025A19B File Offset: 0x0025839B
				// (set) Token: 0x0600580B RID: 22539 RVA: 0x0025A1A3 File Offset: 0x002583A3
				public List<CoopPlayerInfo> UserRankingList { get; private set; }

				// Token: 0x0600580C RID: 22540 RVA: 0x0025A1AC File Offset: 0x002583AC
				public RankingInfo(CoopRanking rankingData)
				{
					this.RankedTime = new DateTime(PrjUtil.ConvertTimeToTicks(rankingData.target_time));
					this.MyPoint = rankingData.mine_point;
					this.UserRankingList = rankingData.ranked_user_list;
				}
			}
		}

		// Token: 0x0200111F RID: 4383
		public class HardQuestEndInfo
		{
			// Token: 0x17000C42 RID: 3138
			// (get) Token: 0x060054C0 RID: 21696 RVA: 0x0024E330 File Offset: 0x0024C530
			// (set) Token: 0x060054C1 RID: 21697 RVA: 0x0024E338 File Offset: 0x0024C538
			public int MapId { get; private set; }

			// Token: 0x17000C43 RID: 3139
			// (get) Token: 0x060054C2 RID: 21698 RVA: 0x0024E341 File Offset: 0x0024C541
			// (set) Token: 0x060054C3 RID: 21699 RVA: 0x0024E349 File Offset: 0x0024C549
			public DateTime startDateTime { get; private set; }

			// Token: 0x17000C44 RID: 3140
			// (get) Token: 0x060054C4 RID: 21700 RVA: 0x0024E352 File Offset: 0x0024C552
			// (set) Token: 0x060054C5 RID: 21701 RVA: 0x0024E35A File Offset: 0x0024C55A
			public DataManagerEvent.CoopData.HardQuestEndInfo.InfoType type { get; private set; }

			// Token: 0x060054C6 RID: 21702 RVA: 0x0024E364 File Offset: 0x0024C564
			public HardQuestEndInfo(CoopHardQuestEndInfo endInfo)
			{
				if (endInfo != null)
				{
					this.MapId = endInfo.map_id;
					this.startDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(endInfo.hard_quest_start_time));
					this.type = (DataManagerEvent.CoopData.HardQuestEndInfo.InfoType)endInfo.info_type;
					return;
				}
				this.MapId = 0;
				this.startDateTime = new DateTime(0L);
				this.type = DataManagerEvent.CoopData.HardQuestEndInfo.InfoType.Invalid;
			}

			// Token: 0x02001231 RID: 4657
			public enum InfoType
			{
				// Token: 0x04006398 RID: 25496
				Invalid,
				// Token: 0x04006399 RID: 25497
				Timeout,
				// Token: 0x0400639A RID: 25498
				Achievement,
				// Token: 0x0400639B RID: 25499
				AchievementAndClear
			}
		}
	}

	// Token: 0x02000676 RID: 1654
	public class CoopRaidTermData
	{
		// Token: 0x060031F7 RID: 12791 RVA: 0x001BE53C File Offset: 0x001BC73C
		public CoopRaidTermData(MstEventCoopRaidTermData mstEventCoopRaidTermData)
		{
			this.eventId = mstEventCoopRaidTermData.eventId;
			this.termId = mstEventCoopRaidTermData.termId;
			this.startTime = DataManagerEvent.CoopRaidTermData.<.ctor>g__GetConvertTimeSpan|4_0(mstEventCoopRaidTermData.startTime);
			this.endTime = DataManagerEvent.CoopRaidTermData.<.ctor>g__GetConvertTimeSpan|4_0(mstEventCoopRaidTermData.endTime);
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x001BE589 File Offset: 0x001BC789
		public bool IsOverStartTime(DateTime dateTime)
		{
			return dateTime.TimeOfDay >= this.startTime;
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x001BE5A2 File Offset: 0x001BC7A2
		public bool IsOverEndTime(DateTime dateTime)
		{
			return dateTime.TimeOfDay >= this.endTime;
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x001BE5BC File Offset: 0x001BC7BC
		[CompilerGenerated]
		internal static TimeSpan <.ctor>g__GetConvertTimeSpan|4_0(string timeStr)
		{
			if (!string.IsNullOrEmpty(timeStr))
			{
				string[] array = timeStr.Split(':', StringSplitOptions.None);
				if (array.Length == 3)
				{
					int num = int.Parse(array[0]);
					int num2 = int.Parse(array[1]);
					int num3 = int.Parse(array[2]);
					return new TimeSpan(num, num2, num3);
				}
			}
			return default(TimeSpan);
		}

		// Token: 0x04002F4B RID: 12107
		public int eventId;

		// Token: 0x04002F4C RID: 12108
		public int termId;

		// Token: 0x04002F4D RID: 12109
		public TimeSpan startTime;

		// Token: 0x04002F4E RID: 12110
		public TimeSpan endTime;
	}

	// Token: 0x02000677 RID: 1655
	public class CoopRaidDrawData
	{
		// Token: 0x060031FB RID: 12795 RVA: 0x001BE60C File Offset: 0x001BC80C
		public CoopRaidDrawData(MstEventCoopRaidDrawData mstEventCoopRaidDrawData)
		{
			this.eventId = mstEventCoopRaidDrawData.eventId;
			this.drawId = mstEventCoopRaidDrawData.drawId;
			this.gaugeProgress = mstEventCoopRaidDrawData.gaugeProgress;
			this.convertDrawId = mstEventCoopRaidDrawData.convertDrawId;
		}

		// Token: 0x04002F4F RID: 12111
		public int eventId;

		// Token: 0x04002F50 RID: 12112
		public int drawId;

		// Token: 0x04002F51 RID: 12113
		public int gaugeProgress;

		// Token: 0x04002F52 RID: 12114
		public int convertDrawId;
	}
}
