using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Login;
using SGNFW.Mst;
using UnityEngine;

public class DataManagerEvent
{
	public DataManagerEvent(DataManager p)
	{
		this.parentData = p;
	}

	public DataManagerEvent.CoopData LastCoopInfo { get; private set; }

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

	public List<DataManagerEvent.EventData> GetEventDataList()
	{
		if (this.eventDataList == null)
		{
			this.eventDataList = new List<DataManagerEvent.EventData>();
		}
		return this.eventDataList;
	}

	public List<DataManagerEvent.EventData> GetEventDataListWithoutMissionEvent()
	{
		return this.eventDataList.FindAll((DataManagerEvent.EventData ev) => ev.eventCategory != DataManagerEvent.Category.Mission);
	}

	public List<DataManagerEvent.EventData> GetEventDataList(DataManagerEvent.Category category)
	{
		return this.eventDataList.FindAll((DataManagerEvent.EventData ev) => ev.eventCategory == category);
	}

	public DataManagerEvent.EventData GetEventData(int evId)
	{
		return this.eventDataList.Find((DataManagerEvent.EventData ev) => ev.eventId == evId);
	}

	public DataManagerEvent.EventData GetEventDataCompareToChapterId(int chId)
	{
		return this.eventDataList.Find((DataManagerEvent.EventData ev) => ev.eventChapterId == chId);
	}

	public List<DataManagerEvent.ReleaseEffects> GetReleaseEffectsList()
	{
		return this.ReleaseEffectsList;
	}

	public DataManagerEvent.ReleaseEffects GetReleaseEffects(int evId)
	{
		return this.ReleaseEffectsList.Find((DataManagerEvent.ReleaseEffects ev) => ev.EventId == evId);
	}

	public List<DataManagerEvent.EventBannerData> GetEventBannerDataList()
	{
		if (this.eventBannerDataList == null)
		{
			this.eventBannerDataList = new List<DataManagerEvent.EventBannerData>();
		}
		return this.eventBannerDataList.FindAll((DataManagerEvent.EventBannerData x) => x.StartDatetime < TimeManager.Now && TimeManager.Now < x.EndDatetime && (x.StartQuestId == 0 || DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(x.StartQuestId)) && (x.EndQuestId == 0 || !DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(x.EndQuestId) || DataManager.DmQuest.QuestDynamicData.oneDataMap[x.EndQuestId].clearNum == 0));
	}

	public DataManagerEvent.LargeEventData GetLargeEventData(int eventId)
	{
		return this.largeEventDataList.Find((DataManagerEvent.LargeEventData x) => eventId == x.EventID);
	}

	public List<DataManagerEvent.PeriodData> GetPeriodDataList(int evId)
	{
		List<DataManagerEvent.PeriodData> list = this.periodDataList.FindAll((DataManagerEvent.PeriodData x) => x.EventId == evId);
		if (1 < list.Count)
		{
			list.Sort((DataManagerEvent.PeriodData a, DataManagerEvent.PeriodData b) => a.StartDatetime.CompareTo(b.StartDatetime));
		}
		return list;
	}

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

	public List<DataManagerEvent.CoopConditionData> GetCoopConditionDataList()
	{
		return this.coopConditionDataList;
	}

	public List<DataManagerEvent.CoopHardQuestData> GetCoopHardQuestDataList()
	{
		return this.coopHardQuestDataList;
	}

	public DataManagerEvent.CoopRaidTermData GetTermData(int eventId, DateTime time)
	{
		return this.coopRaidTermDataList.Find((DataManagerEvent.CoopRaidTermData item) => item.eventId == eventId && item.IsOverStartTime(time) && !item.IsOverEndTime(time));
	}

	public DataManagerEvent.CoopRaidTermData GetNowTermData(int eventId)
	{
		return this.GetTermData(eventId, TimeManager.Now);
	}

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

	public bool isRaidByMapId(int mapId)
	{
		return this.LastCoopInfo != null && this.LastCoopInfo.MapInfoMap.ContainsKey(mapId) && DataManager.DmEvent.GetEventData(this.LastCoopInfo.MapInfoMap[mapId].EventId).raidFlg;
	}

	public bool isRaidByQuestOneId(int questOneId)
	{
		int mapId = DataManager.DmQuest.GetQuestOnePackData(questOneId).questMap.mapId;
		return this.isRaidByMapId(mapId);
	}

	public bool isRaidByEventId(int eventId)
	{
		DataManagerEvent.EventData eventData = this.GetEventData(eventId);
		return eventData != null && eventData.raidFlg;
	}

	public bool isRaidBonusMapId(int mapId)
	{
		QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap[mapId];
		return questStaticMap != null && questStaticMap.QuestMapCategory == QuestStaticMap.MapCategory.CoopBonus;
	}

	public void RequestSelectGrowthEventCharaId(int eventId, int charaId)
	{
		this.parentData.ServerRequest(SelectGrowthEventCharaIdCmd.Create(eventId, charaId), new Action<Command>(this.CbSelectGrowthEventCharaIdCmd));
	}

	public void RequestGetGrowthEventCharaId(int eventId)
	{
		this.parentData.ServerRequest(GetGrowthEventCharaIdCmd.Create(eventId), new Action<Command>(this.CbGetGrowthEventCharaIdCmd));
	}

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

	public void RequestGetCoopInfo(int evId, int mapId)
	{
		if (mapId != 0 && this.LastCoopInfo == null)
		{
			return;
		}
		this.parentData.ServerRequest(CoopInfoCmd.Create(evId, mapId), new Action<Command>(this.CbGetCoopInfoCmd));
	}

	private void CbSelectGrowthEventCharaIdCmd(Command cmd)
	{
		SelectGrowthEventCharaIdResponse selectGrowthEventCharaIdResponse = cmd.response as SelectGrowthEventCharaIdResponse;
		DataManagerEvent.EventData eventData = this.GetEventData(selectGrowthEventCharaIdResponse.event_id);
		eventData.SelectGrowthCharaData = new DataManagerEvent.EventData.GrowthCharaData(new DataManagerEvent.EventData.Bonus(selectGrowthEventCharaIdResponse.chara_id, eventData.GrowthSelcharaRatio), 0L, 0L);
	}

	private void CbGetGrowthEventCharaIdCmd(Command cmd)
	{
		GetGrowthEventCharaIdResponse getGrowthEventCharaIdResponse = cmd.response as GetGrowthEventCharaIdResponse;
		DataManagerEvent.EventData eventData = this.GetEventData(getGrowthEventCharaIdResponse.event_id);
		DataManagerEvent.EventData.Bonus bonus = new DataManagerEvent.EventData.Bonus(getGrowthEventCharaIdResponse.chara_id, eventData.GrowthSelcharaRatio);
		eventData.SelectGrowthCharaData = new DataManagerEvent.EventData.GrowthCharaData(bonus, getGrowthEventCharaIdResponse.select_chara_datetime, getGrowthEventCharaIdResponse.quest_clear_datetime);
	}

	private void CbNewFlgUpdateCmd(Command cmd)
	{
		NewFlgUpdateRequest newFlgUpdateRequest = cmd.request as NewFlgUpdateRequest;
		this.UpdateUserFlagByServer(newFlgUpdateRequest.new_flg_list);
	}

	private void CbGetCoopInfoCmd(Command cmd)
	{
		CoopInfoRequest coopInfoRequest = cmd.request as CoopInfoRequest;
		CoopInfoResponse coopInfoResponse = cmd.response as CoopInfoResponse;
		this.UpdateCoopInfo(coopInfoRequest, coopInfoResponse);
	}

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

	private DataManager parentData;

	private List<DataManagerEvent.EventData> eventDataList;

	private List<DataManagerEvent.EventBannerData> eventBannerDataList;

	private List<DataManagerEvent.LargeEventData> largeEventDataList;

	private List<DataManagerEvent.CoopConditionData> coopConditionDataList;

	private List<DataManagerEvent.CoopHardQuestData> coopHardQuestDataList;

	private List<DataManagerEvent.PeriodData> periodDataList;

	private List<DataManagerEvent.CoopRaidTermData> coopRaidTermDataList;

	private List<DataManagerEvent.CoopRaidDrawData> coopRaidDrawDataList;

	private DataManagerEvent.ReleaseEffects releaseEffects1 = new DataManagerEvent.ReleaseEffects(null);

	private DataManagerEvent.ReleaseEffects releaseEffects2 = new DataManagerEvent.ReleaseEffects(null);

	public class EventData
	{
		public List<DataManagerEvent.EventData.Bonus> GrowthCharaList { get; set; }

		public List<int> GrowthQuestGroupList { get; set; }

		public DataManagerEvent.EventData.GrowthCharaData SelectGrowthCharaData { get; set; } = new DataManagerEvent.EventData.GrowthCharaData(new DataManagerEvent.EventData.Bonus(0, 0), 0L, 0L);

		public int GrowthSelcharaRatio { get; private set; }

		public bool homeDispFlg { get; private set; }

		public bool raidFlg { get; private set; }

		public bool IsEnableEvent
		{
			get
			{
				return this.startDatetime < TimeManager.Now && TimeManager.Now < this.endDatetime;
			}
		}

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

		public int eventId;

		public string eventName;

		public DataManagerEvent.Category eventCategory;

		public int eventBannerId;

		public int eventChapterId;

		public int hardopenQuestOneid;

		public int extraopenQuestOneid;

		public int eventMissionGroupId;

		public List<int> missionIdList;

		public int eventGachaId;

		public List<int> eventShopIdList;

		public List<int> eventCoinIdList;

		public int dispCharaId;

		public string dispCharaBodyMotion;

		public string dispCharaFaceMotion;

		public int modeUIType;

		public string eventTitleScenario;

		public string eventTitleScenario2;

		private string storyFilename;

		private string storyFilename2;

		private string missionBannerFilename;

		public string bgFilename;

		public string bgFilename2;

		public string missionIconFilename;

		public DateTime ResetTime;

		public List<DataManagerEvent.EventImageData> eventImageDataList;

		public DateTime startDatetime;

		public DateTime endDatetime;

		public int openKeyPaidItemID;

		public class Bonus
		{
			public int Id { get; private set; }

			public int Ratio { get; private set; }

			public Bonus(int id, int ratio)
			{
				this.Id = id;
				this.Ratio = ratio;
			}
		}

		public class GrowthCharaData
		{
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

			public DateTime charaSelectDatetime { get; private set; }

			public DateTime questClearDatetime { get; private set; }

			public GrowthCharaData(DataManagerEvent.EventData.Bonus bonus, long selectTime, long clearTime)
			{
				this.growthChara = bonus;
				this.charaSelectDatetime = ((selectTime == 0L) ? new DateTime(2000, 1, 1, 0, 0, 0) : new DateTime(PrjUtil.ConvertTimeToTicks(selectTime)));
				this.questClearDatetime = ((clearTime == 0L) ? new DateTime(2000, 1, 1, 0, 0, 0) : new DateTime(PrjUtil.ConvertTimeToTicks(clearTime)));
			}

			private DataManagerEvent.EventData.Bonus growthChara;
		}
	}

	public class EventBannerData
	{
		public int BannerId { get; private set; }

		public string BannerFilename { get; private set; }

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

		public int StartQuestId { get; private set; }

		public int EndQuestId { get; private set; }

		public DateTime StartDatetime { get; private set; }

		public DateTime EndDatetime { get; private set; }

		public DataManagerEvent.EventBannerData.Type LinkType { get; private set; }

		public string LinkAddress { get; private set; }

		public int LinkValue { get; private set; }

		public int Priority { get; private set; }

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

		private string bannerText;

		public enum Type
		{
			Invalid,
			Move,
			WebView,
			Browser,
			HomeInfo,
			Noah,
			AtomInvite,
			FriendInvite
		}
	}

	public class EventImageData
	{
		public int EventId { get; private set; }

		public DataManagerEvent.EventImageData.ImageType Type { get; private set; }

		public int Sort { get; private set; }

		public string ImagePath { get; private set; }

		public EventImageData(MstEventImageData mstEvImgData)
		{
			this.EventId = mstEvImgData.eventId;
			this.Type = (DataManagerEvent.EventImageData.ImageType)mstEvImgData.type;
			this.Sort = mstEvImgData.sort;
			this.ImagePath = mstEvImgData.imagePath;
		}

		public enum ImageType
		{
			Undefined,
			Tips,
			PickUp
		}
	}

	public class LargeEventData
	{
		public int EventID { get; private set; }

		public Vector2Int MapDirection { get; private set; }

		public Vector2Int MapRangeOrigin { get; private set; }

		public Vector2Int MapRangeSize { get; private set; }

		public Vector2 MapOffset { get; private set; }

		public List<string> TipsFilePath { get; private set; }

		public string MapFilePath { get; private set; }

		public List<DataManagerEvent.LargeEventData.MapFileData> MapFileDataList { get; set; }

		public string BgmFilePath { get; private set; }

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

		public class MapFileData
		{
			public string filepath;

			public int openQuestOneId;
		}
	}

	public class PeriodData
	{
		public int EventId { get; private set; }

		public DateTime StartDatetime { get; private set; }

		public PeriodData(MstEventPeriodData mstPeriodData)
		{
			this.EventId = mstPeriodData.eventId;
			this.StartDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mstPeriodData.startDatetime));
		}
	}

	public class CoopConditionData
	{
		public int ConditionId
		{
			get
			{
				return 0;
			}
		}

		public int EventId { get; private set; }

		public int Level { get; private set; }

		public string LevelName { get; private set; }

		public int MapId { get; private set; }

		public DataManagerEvent.RewardType RewardType { get; private set; }

		public long AchievementCondition { get; private set; }

		public bool IsStart
		{
			get
			{
				return false;
			}
		}

		public ItemInput AchievementItem { get; private set; }

		public string TexturePath { get; private set; }

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

	public class CoopHardQuestData
	{
		public int EventId { get; private set; }

		public int MapId { get; private set; }

		public DateTime StartDatetime { get; private set; }

		public long Starttime { get; private set; }

		public DateTime EndDatetime { get; private set; }

		public int AchievementCondition { get; private set; }

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

	public class ReleaseEffects
	{
		public int EventId { get; private set; }

		public DataManagerEvent.Category Category { get; private set; }

		public int TutorialPhase { get; set; }

		public List<int> ReleaseIdList { get; set; }

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

		public DataManagerEvent.ReleaseEffects Clone()
		{
			return new DataManagerEvent.ReleaseEffects(DataManager.DmEvent.GetEventData(this.EventId))
			{
				TutorialPhase = this.TutorialPhase,
				ReleaseIdList = new List<int>(this.ReleaseIdList)
			};
		}

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

	public enum Category
	{
		INVARID,
		Scenario,
		Growth,
		Large,
		Mission,
		Tower,
		Coop,
		WildRelease,
		SpecialPvp
	}

	public enum CoopType
	{
		All,
		Normal,
		Highdifficulty
	}

	public enum RewardType
	{
		Undefined,
		BonusReward,
		PointReward,
		ReleaseReward
	}

	public class CoopData
	{
		public int EventId { get; private set; }

		public int EventItemId { get; private set; }

		public int EventItemNum { get; private set; }

		public List<DataManagerEvent.CoopData.DispLog> DispLogList { get; private set; }

		public Dictionary<int, DataManagerEvent.CoopData.MapInfo> MapInfoMap { get; set; }

		public List<Quest> QuestList { get; set; }

		public DataManagerEvent.CoopData.HardQuestEndInfo HardQuestEndInfoData { get; private set; }

		public long InfoGetTime { get; private set; }

		public DateTime InfoGetDateTime
		{
			get
			{
				return new DateTime(PrjUtil.ConvertTimeToTicks(this.InfoGetTime));
			}
		}

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

		public class DispLog
		{
			public string UserName { get; private set; }

			public int MapId { get; private set; }

			public int Point { get; private set; }

			public DateTime InsertTime { get; private set; }

			public DispLog(CoopLog log)
			{
				this.UserName = log.user_name;
				this.MapId = log.map_id;
				this.Point = log.point;
				this.InsertTime = new DateTime(log.start_time);
			}
		}

		public class MapInfo
		{
			public int EventId { get; private set; }

			public int MapId { get; private set; }

			public int Level { get; private set; }

			public long TotalPoint { get; private set; }

			public long EndPoint { get; set; }

			public float ProgressRate
			{
				get
				{
					float num = (float)this.TotalPoint / (float)this.EndPoint;
					return Mathf.Min(1f, num);
				}
			}

			public bool IsClear
			{
				get
				{
					return this.EndPoint <= this.TotalPoint;
				}
			}

			public bool IsHardQuestOpen { get; private set; }

			public int HardQuestClearNum { get; private set; }

			public long HardQuestStartTime { get; private set; }

			public int BonusDefeatedCount { get; private set; }

			public CoopPlayerInfo HardClearUser { get; private set; }

			public List<DataManagerEvent.CoopData.MapInfo.RankingInfo> RankingInfoList { get; set; }

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

			public DataManagerEvent.CoopConditionData StaticNextCoopConditionData { get; private set; }

			public DataManagerEvent.CoopHardQuestData StaticCoopHardQuestData { get; private set; }

			public List<DataManagerEvent.CoopConditionData> MapRewardConditionalDataList
			{
				get
				{
					List<DataManagerEvent.CoopConditionData> list = DataManager.DmEvent.coopConditionDataList.FindAll((DataManagerEvent.CoopConditionData x) => x.MapId == this.MapId && x.Level == this.Level);
					list.Sort((DataManagerEvent.CoopConditionData a, DataManagerEvent.CoopConditionData b) => a.AchievementCondition.CompareTo(b.AchievementCondition));
					return list;
				}
			}

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

			public class RankingInfo
			{
				public DateTime RankedTime { get; private set; }

				public int MyPoint { get; private set; }

				public List<CoopPlayerInfo> UserRankingList { get; private set; }

				public RankingInfo(CoopRanking rankingData)
				{
					this.RankedTime = new DateTime(PrjUtil.ConvertTimeToTicks(rankingData.target_time));
					this.MyPoint = rankingData.mine_point;
					this.UserRankingList = rankingData.ranked_user_list;
				}
			}
		}

		public class HardQuestEndInfo
		{
			public int MapId { get; private set; }

			public DateTime startDateTime { get; private set; }

			public DataManagerEvent.CoopData.HardQuestEndInfo.InfoType type { get; private set; }

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

			public enum InfoType
			{
				Invalid,
				Timeout,
				Achievement,
				AchievementAndClear
			}
		}
	}

	public class CoopRaidTermData
	{
		public CoopRaidTermData(MstEventCoopRaidTermData mstEventCoopRaidTermData)
		{
			this.eventId = mstEventCoopRaidTermData.eventId;
			this.termId = mstEventCoopRaidTermData.termId;
			this.startTime = DataManagerEvent.CoopRaidTermData.<.ctor>g__GetConvertTimeSpan|4_0(mstEventCoopRaidTermData.startTime);
			this.endTime = DataManagerEvent.CoopRaidTermData.<.ctor>g__GetConvertTimeSpan|4_0(mstEventCoopRaidTermData.endTime);
		}

		public bool IsOverStartTime(DateTime dateTime)
		{
			return dateTime.TimeOfDay >= this.startTime;
		}

		public bool IsOverEndTime(DateTime dateTime)
		{
			return dateTime.TimeOfDay >= this.endTime;
		}

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

		public int eventId;

		public int termId;

		public TimeSpan startTime;

		public TimeSpan endTime;
	}

	public class CoopRaidDrawData
	{
		public CoopRaidDrawData(MstEventCoopRaidDrawData mstEventCoopRaidDrawData)
		{
			this.eventId = mstEventCoopRaidDrawData.eventId;
			this.drawId = mstEventCoopRaidDrawData.drawId;
			this.gaugeProgress = mstEventCoopRaidDrawData.gaugeProgress;
			this.convertDrawId = mstEventCoopRaidDrawData.convertDrawId;
		}

		public int eventId;

		public int drawId;

		public int gaugeProgress;

		public int convertDrawId;
	}
}
