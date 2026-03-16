using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestUtil
{
	public static List<string> GetCampaignMessageList(QuestStaticChapter.Category category, int chapterId)
	{
		List<QuestStaticChapter> chapterDataList = DataManager.DmQuest.QuestStaticData.chapterDataList;
		List<QuestStaticMap> mapDataList = DataManager.DmQuest.QuestStaticData.mapDataList;
		List<QuestStaticQuestGroup> groupDataList = DataManager.DmQuest.QuestStaticData.groupDataList;
		long dt = TimeManager.Now.Ticks;
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		List<DataManagerCampaign.CampaignItemDropData> list3 = new List<DataManagerCampaign.CampaignItemDropData>(DataManager.DmCampaign.CampaignItemDropDataList);
		list3.Sort((DataManagerCampaign.CampaignItemDropData a, DataManagerCampaign.CampaignItemDropData b) => b.campaignId - a.campaignId);
		foreach (DataManagerCampaign.CampaignItemDropData campaignItemDropData in list3)
		{
			if (dt >= campaignItemDropData.startTime.Ticks && dt <= campaignItemDropData.endTime.Ticks && campaignItemDropData.campaignTargetList.Count > 0)
			{
				using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignItemDropData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.QuestGroup).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DataManagerCampaign.CampaignTarget target2 = enumerator2.Current;
						QuestStaticQuestGroup group2 = groupDataList.Find((QuestStaticQuestGroup item) => item.questGroupId == target2.TargetId);
						if (group2 != null)
						{
							Predicate<QuestStaticQuestGroup> <>9__5;
							QuestStaticMap map2 = mapDataList.Find(delegate(QuestStaticMap item)
							{
								List<QuestStaticQuestGroup> questGroupList = item.questGroupList;
								Predicate<QuestStaticQuestGroup> predicate2;
								if ((predicate2 = <>9__5) == null)
								{
									predicate2 = (<>9__5 = (QuestStaticQuestGroup item1) => item1.questGroupId == group2.questGroupId);
								}
								return questGroupList.Find(predicate2) != null;
							});
							if (map2 != null)
							{
								QuestStaticChapter questStaticChapter = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == map2.chapterId);
								if (questStaticChapter != null && questStaticChapter.category == category && list2.Count < QuestUtil.CampaignMaxCount)
								{
									list2.Add(campaignItemDropData.ratio);
								}
							}
						}
					}
				}
				if (list2.Count >= QuestUtil.CampaignMaxCount)
				{
					break;
				}
				using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignItemDropData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.Chapter).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DataManagerCampaign.CampaignTarget target3 = enumerator2.Current;
						QuestStaticChapter questStaticChapter2 = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == target3.TargetId);
						if (questStaticChapter2 != null && questStaticChapter2.category == category && list2.Count < QuestUtil.CampaignMaxCount)
						{
							list2.Add(campaignItemDropData.ratio);
						}
					}
				}
				if (list2.Count >= QuestUtil.CampaignMaxCount)
				{
					break;
				}
				break;
			}
		}
		foreach (int num in list2)
		{
			list.Add(PrjUtil.MakeMessage("獲得アイテム数<size=24><color=#FFEE00>" + (((float)num + QuestUtil.Rate10000) / QuestUtil.Rate10000).ToString((num % (int)QuestUtil.Rate10000 == 0) ? "F0" : "F1") + "</color></size>倍！"));
		}
		List<int> list4 = new List<int>();
		List<DataManagerCampaign.CampaignKizunaData> list5 = new List<DataManagerCampaign.CampaignKizunaData>(DataManager.DmCampaign.CampaignKizunaDataList);
		list5.Sort((DataManagerCampaign.CampaignKizunaData a, DataManagerCampaign.CampaignKizunaData b) => b.campaignId - a.campaignId);
		foreach (DataManagerCampaign.CampaignKizunaData campaignKizunaData in list5)
		{
			if (dt >= campaignKizunaData.startTime.Ticks && dt <= campaignKizunaData.endTime.Ticks && campaignKizunaData.campaignTargetList.Count > 0)
			{
				using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignKizunaData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.QuestGroup).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DataManagerCampaign.CampaignTarget target4 = enumerator2.Current;
						QuestStaticQuestGroup group = groupDataList.Find((QuestStaticQuestGroup item) => item.questGroupId == target4.TargetId);
						if (group != null)
						{
							Predicate<QuestStaticQuestGroup> <>9__13;
							QuestStaticMap map = mapDataList.Find(delegate(QuestStaticMap item)
							{
								List<QuestStaticQuestGroup> questGroupList2 = item.questGroupList;
								Predicate<QuestStaticQuestGroup> predicate3;
								if ((predicate3 = <>9__13) == null)
								{
									predicate3 = (<>9__13 = (QuestStaticQuestGroup item1) => item1.questGroupId == group.questGroupId);
								}
								return questGroupList2.Find(predicate3) != null;
							});
							if (map != null)
							{
								QuestStaticChapter questStaticChapter3 = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == map.chapterId);
								if (questStaticChapter3 != null && questStaticChapter3.category == category && list4.Count < QuestUtil.CampaignMaxCount)
								{
									list4.Add(campaignKizunaData.ratio);
								}
							}
						}
					}
				}
				if (list4.Count >= QuestUtil.CampaignMaxCount)
				{
					break;
				}
				using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignKizunaData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.Chapter).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DataManagerCampaign.CampaignTarget target5 = enumerator2.Current;
						QuestStaticChapter questStaticChapter4 = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == target5.TargetId);
						if (questStaticChapter4 != null && questStaticChapter4.category == category && list4.Count < QuestUtil.CampaignMaxCount)
						{
							list4.Add(campaignKizunaData.ratio);
						}
					}
				}
				if (list4.Count >= QuestUtil.CampaignMaxCount)
				{
					break;
				}
				break;
			}
		}
		foreach (int num2 in list4)
		{
			int num3 = num2 % (int)QuestUtil.Rate10000;
			list.Add("なかよしPt<size=24><color=#FFEE00>" + (((float)num2 + QuestUtil.Rate10000) / QuestUtil.Rate10000).ToString((num2 % (int)QuestUtil.Rate10000 == 0) ? "F0" : "F1") + "</color></size>倍！");
		}
		int num4 = 0;
		Predicate<DataManagerQuest.DrawItemTermData> <>9__16;
		foreach (QuestStaticChapter questStaticChapter5 in chapterDataList)
		{
			if (questStaticChapter5.category == category && (chapterId == 0 || questStaticChapter5.chapterId == chapterId))
			{
				List<DataManagerQuest.DrawItemTermData> drawItemTermDataList = questStaticChapter5.drawItemTermDataList;
				Predicate<DataManagerQuest.DrawItemTermData> predicate;
				if ((predicate = <>9__16) == null)
				{
					predicate = (<>9__16 = (DataManagerQuest.DrawItemTermData item) => item.StartDateTime.Ticks <= dt && item.EndDateTime.Ticks >= dt);
				}
				if (drawItemTermDataList.Exists(predicate))
				{
					list.Add("獲得アイテム追加！");
					num4++;
				}
			}
			if (num4 >= QuestUtil.CampaignMaxCount)
			{
				break;
			}
		}
		int num5 = 0;
		foreach (DataManagerCampaign.CampaignQuestStaminaData campaignQuestStaminaData in DataManager.DmCampaign.PresentCampaignQuestStaminaDataList)
		{
			using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignQuestStaminaData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.Chapter).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					DataManagerCampaign.CampaignTarget target = enumerator2.Current;
					QuestStaticChapter questStaticChapter6 = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == target.TargetId);
					if (questStaticChapter6 != null)
					{
						if (questStaticChapter6.category == category && (chapterId == 0 || questStaticChapter6.chapterId == chapterId))
						{
							list.Add("消費スタミナ減少！");
							num5++;
						}
						if (num5 >= QuestUtil.CampaignMaxCount)
						{
							break;
						}
					}
				}
			}
			if (num5 >= QuestUtil.CampaignMaxCount)
			{
				break;
			}
		}
		if (DataManager.DmCampaign.PresentCampaignStaminaRecoveryData != null && chapterId != 0)
		{
			list.Add("スタミナ回復速度" + (300 / DataManager.DmCampaign.PresentCampaignStaminaRecoveryData.staminaRecoveryTime).ToString() + "倍!");
		}
		if (category == QuestStaticChapter.Category.ETCETERA)
		{
			List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.ETCETERA);
			List<QuestStaticQuestGroup> list6 = DataManager.DmQuest.QuestStaticData.groupDataList.FindAll((QuestStaticQuestGroup item) => playableMapIdList.Contains(item.mapId));
			DateTime dateTime = TimeManager.Now.AddDays(30.0);
			using (List<QuestStaticQuestGroup>.Enumerator enumerator7 = list6.GetEnumerator())
			{
				while (enumerator7.MoveNext())
				{
					if (enumerator7.Current.endDatetime < dateTime)
					{
						list.Add("限定クエスト\n挑戦可能！");
						break;
					}
				}
			}
		}
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed @sealed = ((homeCheckResult != null) ? homeCheckResult.sealedData : null) ?? new Sealed();
		if (DataManager.DmTraining.IsChallengePossible && @sealed.quest_training != 1)
		{
			list.Add(QuestUtil.CampaignInfo.DojoText);
		}
		return list;
	}

	public static List<string> GetCampaignTimeList(QuestStaticChapter.Category category, int chapterId)
	{
		List<QuestStaticChapter> chapterDataList = DataManager.DmQuest.QuestStaticData.chapterDataList;
		List<QuestStaticMap> mapDataList = DataManager.DmQuest.QuestStaticData.mapDataList;
		List<QuestStaticQuestGroup> groupDataList = DataManager.DmQuest.QuestStaticData.groupDataList;
		DateTime now = TimeManager.Now;
		long dt = now.Ticks;
		List<string> list = new List<string>();
		int num = 0;
		List<DataManagerCampaign.CampaignItemDropData> list2 = new List<DataManagerCampaign.CampaignItemDropData>(DataManager.DmCampaign.CampaignItemDropDataList);
		list2.Sort((DataManagerCampaign.CampaignItemDropData a, DataManagerCampaign.CampaignItemDropData b) => b.campaignId - a.campaignId);
		foreach (DataManagerCampaign.CampaignItemDropData campaignItemDropData in list2)
		{
			if (dt >= campaignItemDropData.startTime.Ticks && dt <= campaignItemDropData.endTime.Ticks && campaignItemDropData.campaignTargetList.Count > 0)
			{
				using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignItemDropData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.QuestGroup).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DataManagerCampaign.CampaignTarget target2 = enumerator2.Current;
						QuestStaticQuestGroup group2 = groupDataList.Find((QuestStaticQuestGroup item) => item.questGroupId == target2.TargetId);
						if (group2 != null)
						{
							Predicate<QuestStaticQuestGroup> <>9__5;
							QuestStaticMap map2 = mapDataList.Find(delegate(QuestStaticMap item)
							{
								List<QuestStaticQuestGroup> questGroupList = item.questGroupList;
								Predicate<QuestStaticQuestGroup> predicate2;
								if ((predicate2 = <>9__5) == null)
								{
									predicate2 = (<>9__5 = (QuestStaticQuestGroup item1) => item1.questGroupId == group2.questGroupId);
								}
								return questGroupList.Find(predicate2) != null;
							});
							if (map2 != null)
							{
								QuestStaticChapter questStaticChapter = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == map2.chapterId);
								if (questStaticChapter != null && questStaticChapter.category == category && num < QuestUtil.CampaignMaxCount)
								{
									list.Add(TimeManager.MakeTimeResidueText(now, campaignItemDropData.endTime, true, true));
									num++;
								}
							}
						}
					}
				}
				if (num >= QuestUtil.CampaignMaxCount)
				{
					break;
				}
				using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignItemDropData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.Chapter).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DataManagerCampaign.CampaignTarget target3 = enumerator2.Current;
						QuestStaticChapter questStaticChapter2 = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == target3.TargetId);
						if (questStaticChapter2 != null && questStaticChapter2.category == category && num < QuestUtil.CampaignMaxCount)
						{
							list.Add(TimeManager.MakeTimeResidueText(now, campaignItemDropData.endTime, true, true));
							num++;
						}
					}
				}
				if (num >= QuestUtil.CampaignMaxCount)
				{
					break;
				}
				break;
			}
		}
		int num2 = 0;
		List<DataManagerCampaign.CampaignKizunaData> list3 = new List<DataManagerCampaign.CampaignKizunaData>(DataManager.DmCampaign.CampaignKizunaDataList);
		list3.Sort((DataManagerCampaign.CampaignKizunaData a, DataManagerCampaign.CampaignKizunaData b) => b.campaignId - a.campaignId);
		foreach (DataManagerCampaign.CampaignKizunaData campaignKizunaData in list3)
		{
			if (dt >= campaignKizunaData.startTime.Ticks && dt <= campaignKizunaData.endTime.Ticks && campaignKizunaData.campaignTargetList.Count > 0)
			{
				using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignKizunaData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.QuestGroup).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DataManagerCampaign.CampaignTarget target4 = enumerator2.Current;
						QuestStaticQuestGroup group = groupDataList.Find((QuestStaticQuestGroup item) => item.questGroupId == target4.TargetId);
						if (group != null)
						{
							Predicate<QuestStaticQuestGroup> <>9__13;
							QuestStaticMap map = mapDataList.Find(delegate(QuestStaticMap item)
							{
								List<QuestStaticQuestGroup> questGroupList2 = item.questGroupList;
								Predicate<QuestStaticQuestGroup> predicate3;
								if ((predicate3 = <>9__13) == null)
								{
									predicate3 = (<>9__13 = (QuestStaticQuestGroup item1) => item1.questGroupId == group.questGroupId);
								}
								return questGroupList2.Find(predicate3) != null;
							});
							if (map != null)
							{
								QuestStaticChapter questStaticChapter3 = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == map.chapterId);
								if (questStaticChapter3 != null && questStaticChapter3.category == category && num2 < QuestUtil.CampaignMaxCount)
								{
									list.Add(TimeManager.MakeTimeResidueText(now, campaignKizunaData.endTime, true, true));
									num2++;
								}
							}
						}
					}
				}
				if (num2 >= QuestUtil.CampaignMaxCount)
				{
					break;
				}
				using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignKizunaData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.Chapter).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DataManagerCampaign.CampaignTarget target5 = enumerator2.Current;
						QuestStaticChapter questStaticChapter4 = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == target5.TargetId);
						if (questStaticChapter4 != null && questStaticChapter4.category == category && num2 < QuestUtil.CampaignMaxCount)
						{
							list.Add(TimeManager.MakeTimeResidueText(now, campaignKizunaData.endTime, true, true));
							num2++;
						}
					}
				}
				if (num2 >= QuestUtil.CampaignMaxCount)
				{
					break;
				}
				break;
			}
		}
		int num3 = 0;
		Predicate<DataManagerQuest.DrawItemTermData> <>9__16;
		foreach (QuestStaticChapter questStaticChapter5 in chapterDataList)
		{
			if (questStaticChapter5.category == category && (chapterId == 0 || questStaticChapter5.chapterId == chapterId))
			{
				List<DataManagerQuest.DrawItemTermData> drawItemTermDataList = questStaticChapter5.drawItemTermDataList;
				Predicate<DataManagerQuest.DrawItemTermData> predicate;
				if ((predicate = <>9__16) == null)
				{
					predicate = (<>9__16 = (DataManagerQuest.DrawItemTermData item) => item.StartDateTime.Ticks <= dt && item.EndDateTime.Ticks >= dt);
				}
				DataManagerQuest.DrawItemTermData drawItemTermData = drawItemTermDataList.Find(predicate);
				if (drawItemTermData != null)
				{
					list.Add(TimeManager.MakeTimeResidueText(now, drawItemTermData.EndDateTime, true, true));
					num3++;
				}
			}
			if (num3 >= QuestUtil.CampaignMaxCount)
			{
				break;
			}
		}
		int num4 = 0;
		foreach (DataManagerCampaign.CampaignQuestStaminaData campaignQuestStaminaData in DataManager.DmCampaign.PresentCampaignQuestStaminaDataList)
		{
			using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignQuestStaminaData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.Chapter).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					DataManagerCampaign.CampaignTarget target = enumerator2.Current;
					QuestStaticChapter questStaticChapter6 = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == target.TargetId);
					if (questStaticChapter6 != null)
					{
						if (questStaticChapter6.category == category && (chapterId == 0 || questStaticChapter6.chapterId == chapterId))
						{
							list.Add(TimeManager.MakeTimeResidueText(now, campaignQuestStaminaData.endTime, true, true));
							num4++;
						}
						if (num4 >= QuestUtil.CampaignMaxCount)
						{
							break;
						}
					}
				}
			}
			if (num4 >= QuestUtil.CampaignMaxCount)
			{
				break;
			}
		}
		if (DataManager.DmCampaign.PresentCampaignStaminaRecoveryData != null && chapterId != 0)
		{
			list.Add(TimeManager.MakeTimeResidueText(now, DataManager.DmCampaign.PresentCampaignStaminaRecoveryData.endTime, true, true));
		}
		return list;
	}

	public static bool CheckCampaignQuestGroup(int groupId)
	{
		long dt = TimeManager.Now.Ticks;
		List<DataManagerCampaign.CampaignItemDropData> list = new List<DataManagerCampaign.CampaignItemDropData>(DataManager.DmCampaign.CampaignItemDropDataList);
		list.Sort((DataManagerCampaign.CampaignItemDropData a, DataManagerCampaign.CampaignItemDropData b) => b.campaignId - a.campaignId);
		int num = 0;
		if (list.Count > 0)
		{
			num = list[0].campaignId;
		}
		Predicate<DataManagerCampaign.CampaignTarget> <>9__2;
		foreach (DataManagerCampaign.CampaignItemDropData campaignItemDropData in list)
		{
			if (num != campaignItemDropData.campaignId)
			{
				return false;
			}
			if (dt >= campaignItemDropData.startTime.Ticks && dt <= campaignItemDropData.endTime.Ticks && campaignItemDropData.campaignTargetList.Count > 0)
			{
				List<DataManagerCampaign.CampaignTarget> list2 = campaignItemDropData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.QuestGroup);
				Predicate<DataManagerCampaign.CampaignTarget> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = (DataManagerCampaign.CampaignTarget target) => target.TargetId == groupId);
				}
				if (list2.Find(predicate) != null)
				{
					return true;
				}
			}
		}
		List<DataManagerCampaign.CampaignKizunaData> list3 = new List<DataManagerCampaign.CampaignKizunaData>(DataManager.DmCampaign.CampaignKizunaDataList);
		list3.Sort((DataManagerCampaign.CampaignKizunaData a, DataManagerCampaign.CampaignKizunaData b) => b.campaignId - a.campaignId);
		int num2 = 0;
		if (list3.Count > 0)
		{
			num2 = list3[0].campaignId;
		}
		Predicate<DataManagerCampaign.CampaignTarget> <>9__5;
		foreach (DataManagerCampaign.CampaignKizunaData campaignKizunaData in list3)
		{
			if (num2 != campaignKizunaData.campaignId)
			{
				return false;
			}
			if (dt >= campaignKizunaData.startTime.Ticks && dt <= campaignKizunaData.endTime.Ticks && campaignKizunaData.campaignTargetList.Count > 0)
			{
				List<DataManagerCampaign.CampaignTarget> list4 = campaignKizunaData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.QuestGroup);
				Predicate<DataManagerCampaign.CampaignTarget> predicate2;
				if ((predicate2 = <>9__5) == null)
				{
					predicate2 = (<>9__5 = (DataManagerCampaign.CampaignTarget target) => target.TargetId == groupId);
				}
				if (list4.Find(predicate2) != null)
				{
					return true;
				}
			}
		}
		QuestStaticQuestGroup questStaticQuestGroup = DataManager.DmQuest.QuestStaticData.groupDataMap.TryGetValueEx(groupId, null);
		return questStaticQuestGroup != null && questStaticQuestGroup.drawItemTermDataList.Exists((DataManagerQuest.DrawItemTermData item) => item.StartDateTime.Ticks <= dt && item.EndDateTime.Ticks >= dt);
	}

	public static void GetEnableEventReleaseEffects(ref List<DataManagerEvent.ReleaseEffects> releaseEffectsList, ref DataManagerEvent.ReleaseEffects releaseEffects, DataManagerEvent.EventData inEventData)
	{
		releaseEffects = releaseEffectsList.Find((DataManagerEvent.ReleaseEffects item) => item.EventId == inEventData.eventId);
		if (releaseEffects == null)
		{
			releaseEffects = new DataManagerEvent.ReleaseEffects(inEventData);
		}
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(inEventData.eventId);
		if (eventData != null && eventData.IsEnableChapter)
		{
			Predicate<DataManagerEvent.ReleaseEffects> <>9__1;
			for (int i = 0; i < releaseEffectsList.Count; i++)
			{
				List<DataManagerEvent.ReleaseEffects> list = releaseEffectsList;
				Predicate<DataManagerEvent.ReleaseEffects> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = (DataManagerEvent.ReleaseEffects item) => item.EventId == inEventData.eventId);
				}
				if ((!list.Exists(predicate) && releaseEffectsList[i].EventId == 0) || releaseEffectsList[i].EventId == inEventData.eventId)
				{
					releaseEffectsList[i] = releaseEffects;
					return;
				}
			}
		}
	}

	public static int ClearConditionQuestOneId(QuestStaticChapter.Category questCategory)
	{
		DataManagerServerMst.ModeReleaseData.ModeCategory modeCategory = DataManagerServerMst.ModeReleaseData.ModeCategory.GrowthQuest;
		switch (questCategory)
		{
		case QuestStaticChapter.Category.GROW:
			modeCategory = DataManagerServerMst.ModeReleaseData.ModeCategory.GrowthQuest;
			break;
		case QuestStaticChapter.Category.CHARA:
			modeCategory = DataManagerServerMst.ModeReleaseData.ModeCategory.FriendsStory;
			break;
		case QuestStaticChapter.Category.SIDE_STORY:
			modeCategory = DataManagerServerMst.ModeReleaseData.ModeCategory.AraiDiary;
			break;
		case QuestStaticChapter.Category.TRAINING:
			modeCategory = DataManagerServerMst.ModeReleaseData.ModeCategory.TrainingMode;
			break;
		case QuestStaticChapter.Category.CELLVAL:
			modeCategory = DataManagerServerMst.ModeReleaseData.ModeCategory.Cellval;
			break;
		case QuestStaticChapter.Category.ETCETERA:
			modeCategory = DataManagerServerMst.ModeReleaseData.ModeCategory.EtceteraQuest;
			break;
		case QuestStaticChapter.Category.STORY2:
			modeCategory = DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory2;
			break;
		case QuestStaticChapter.Category.STORY3:
			modeCategory = DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory3;
			break;
		case QuestStaticChapter.Category.ASSISTANT:
			modeCategory = DataManagerServerMst.ModeReleaseData.ModeCategory.Assistant;
			break;
		}
		int num = 0;
		DataManagerServerMst.ModeReleaseData modeReleaseData = DataManager.DmServerMst.ModeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == modeCategory);
		if (modeReleaseData != null)
		{
			num = modeReleaseData.QuestId;
		}
		return num;
	}

	public static bool IsDispDhole()
	{
		DataManagerServerMst.ModeReleaseData modeReleaseData = DataManager.DmServerMst.ModeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.DholeReturns);
		if (modeReleaseData != null)
		{
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData.QuestId);
			if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questOnePackData.questOne.questId))
			{
				QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[questOnePackData.questOne.questId];
				if (questDynamicQuestOne.status == QuestOneStatus.CLEAR || questDynamicQuestOne.status == QuestOneStatus.COMPLETE)
				{
					return true;
				}
			}
		}
		DataManagerServerMst.ModeReleaseData modeReleaseData2 = DataManager.DmServerMst.ModeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.DholeInactive);
		if (modeReleaseData2 != null)
		{
			QuestOnePackData questOnePackData2 = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData2.QuestId);
			if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questOnePackData2.questOne.questId))
			{
				QuestDynamicQuestOne questDynamicQuestOne2 = DataManager.DmQuest.QuestDynamicData.oneDataMap[questOnePackData2.questOne.questId];
				if (questDynamicQuestOne2.status == QuestOneStatus.CLEAR || questDynamicQuestOne2.status == QuestOneStatus.COMPLETE)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool ClearConditionGrayOutButton(QuestStaticChapter.Category questCategory)
	{
		int num = QuestUtil.ClearConditionQuestOneId(questCategory);
		QuestDynamicQuestOne questDynamicQuestOne = null;
		if (DataManager.DmQuest.GetQuestOnePackData(num).questChapter.category == QuestStaticChapter.Category.TUTORIAL)
		{
			return DataManager.DmUserInfo.tutorialSequence == TutorialUtil.Sequence.END;
		}
		if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(num))
		{
			questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[num];
		}
		return questDynamicQuestOne != null && (questDynamicQuestOne.status == QuestOneStatus.CLEAR || questDynamicQuestOne.status == QuestOneStatus.COMPLETE);
	}

	public static List<QuestStaticChapter> CreateHardChapterDataList(QuestUtil.SelectData selectData)
	{
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(selectData.questCategory);
		List<QuestStaticChapter> list = DataManager.DmQuest.QuestStaticData.chapterDataList.FindAll((QuestStaticChapter item) => item.hardChapterId != 0);
		List<QuestStaticChapter> list2 = new List<QuestStaticChapter>();
		using (List<int>.Enumerator enumerator = playableMapIdList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int e = enumerator.Current;
				QuestStaticMap mapData = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap item) => item.mapId == e);
				QuestStaticChapter tempData = list.Find((QuestStaticChapter item) => item.hardChapterId == mapData.chapterId);
				if (tempData != null)
				{
					QuestStaticChapter chapterData = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == tempData.hardChapterId);
					if (chapterData != null && !list2.Exists((QuestStaticChapter item) => item == chapterData))
					{
						list2.Add(chapterData);
					}
				}
			}
		}
		return list2;
	}

	public static bool IsHardMode(QuestUtil.SelectData selectData)
	{
		List<QuestStaticChapter> list = QuestUtil.CreateHardChapterDataList(selectData);
		return DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == selectData.chapterId) != null && list.Find((QuestStaticChapter item) => item.chapterId == selectData.chapterId) != null;
	}

	private static List<QuestStaticChapter> CreateHardChapterDataList()
	{
		List<QuestStaticChapter> list = DataManager.DmQuest.QuestStaticData.chapterDataList.FindAll((QuestStaticChapter item) => item.hardChapterId != 0);
		List<QuestStaticChapter> list2 = new List<QuestStaticChapter>();
		using (List<QuestStaticChapter>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestStaticChapter tempData = enumerator.Current;
				QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == tempData.hardChapterId);
				if (questStaticChapter != null)
				{
					list2.Add(questStaticChapter);
				}
			}
		}
		return list2;
	}

	public static bool IsHardMode(int chapterId)
	{
		return QuestUtil.CreateHardChapterDataList().Find((QuestStaticChapter item) => item.chapterId == chapterId) != null;
	}

	public static void OpenBannerWebViewWindow(int eventId)
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(eventId);
		if (eventData == null)
		{
			return;
		}
		HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(eventData.eventBannerId);
		CanvasManager.HdlWebViewWindowCtrl.Open(homeBannerData.actionParamURL);
	}

	public static bool EnableEventGrowthExpUpFromGroupQuestId(int groupQuestId)
	{
		Predicate<int> <>9__0;
		foreach (DataManagerEvent.EventData eventData in DataManager.DmEvent.GetEventDataList())
		{
			if (eventData.IsEnableEvent && eventData.eventCategory == DataManagerEvent.Category.Growth)
			{
				List<int> growthQuestGroupList = eventData.GrowthQuestGroupList;
				Predicate<int> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (int item) => item == groupQuestId);
				}
				return growthQuestGroupList.Exists(predicate);
			}
		}
		return false;
	}

	public static bool EnableEventGrowthExpUpFromQuesOneId(int questId)
	{
		QuestOnePackData qopd = DataManager.DmQuest.GetQuestOnePackData(questId);
		if (qopd != null)
		{
			Predicate<int> <>9__0;
			Predicate<QuestStaticQuestOne> <>9__1;
			foreach (DataManagerEvent.EventData eventData in DataManager.DmEvent.GetEventDataList())
			{
				if (eventData.IsEnableEvent && eventData.eventCategory == DataManagerEvent.Category.Growth)
				{
					List<int> growthQuestGroupList = eventData.GrowthQuestGroupList;
					Predicate<int> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (int item) => item == qopd.questGroup.questGroupId);
					}
					if (growthQuestGroupList.Exists(predicate))
					{
						List<QuestStaticQuestOne> questOneList = qopd.questGroup.questOneList;
						Predicate<QuestStaticQuestOne> predicate2;
						if ((predicate2 = <>9__1) == null)
						{
							predicate2 = (<>9__1 = (QuestStaticQuestOne item) => item.questId == questId);
						}
						return questOneList.Exists(predicate2);
					}
				}
			}
			return false;
		}
		return false;
	}

	public static bool IsUnLockInformationCellvalQuest(DateTime tgtTime)
	{
		Predicate<QuestStaticMap> <>9__0;
		foreach (QuestStaticChapter questStaticChapter in DataManager.DmQuest.QuestStaticData.chapterDataMap.Values)
		{
			if (questStaticChapter.category == QuestStaticChapter.Category.CELLVAL)
			{
				List<QuestStaticMap> mapDataList = questStaticChapter.mapDataList;
				Predicate<QuestStaticMap> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (QuestStaticMap item) => item.StartDateTime <= tgtTime);
				}
				if (mapDataList.Exists(predicate))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsUnLockInformationMainStory2(DateTime tgtTime)
	{
		Predicate<QuestStaticMap> <>9__0;
		foreach (QuestStaticChapter questStaticChapter in DataManager.DmQuest.QuestStaticData.chapterDataMap.Values)
		{
			if (questStaticChapter.category == QuestStaticChapter.Category.STORY2)
			{
				List<QuestStaticMap> mapDataList = questStaticChapter.mapDataList;
				Predicate<QuestStaticMap> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (QuestStaticMap item) => item.StartDateTime <= tgtTime);
				}
				if (mapDataList.Exists(predicate))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsUnLockInformationMainStory3(DateTime tgtTime)
	{
		Predicate<QuestStaticMap> <>9__0;
		foreach (QuestStaticChapter questStaticChapter in DataManager.DmQuest.QuestStaticData.chapterDataMap.Values)
		{
			if (questStaticChapter.category == QuestStaticChapter.Category.STORY3)
			{
				List<QuestStaticMap> mapDataList = questStaticChapter.mapDataList;
				Predicate<QuestStaticMap> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (QuestStaticMap item) => item.StartDateTime <= tgtTime);
				}
				if (mapDataList.Exists(predicate))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void UpdateBG(Transform bgTr, int index, int count, string mapFolder, string mapPrefix)
	{
		Vector3 localPosition = bgTr.localPosition;
		bgTr.localPosition = new Vector3(localPosition.x, SceneQuest.BgDefaultPosY + QuestUtil.BG_OFFSET_POS_Y * (float)count, 0f);
		Vector2 sizeDelta = bgTr.gameObject.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.y = QuestUtil.BG_OFFSET_POS_Y * 2f + QuestUtil.BG_OFFSET_POS_Y * 2f * (float)count;
		bgTr.gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		Transform transform = bgTr.Find("OffsetObj/Right");
		if (transform != null)
		{
			float x = transform.localPosition.x;
			transform.localPosition = new Vector3(x, -(SceneQuest.BgDefaultPosY + QuestUtil.BG_OFFSET_POS_Y * (float)count), 0f);
		}
		bgTr.Find("Tex_Bg").GetComponent<PguiRawImageCtrl>().SetRawImage(string.Format("Texture2D/{0}/{1}{2}", mapFolder, mapPrefix, index / 3 + 1), true, false, null);
	}

	public static void SetupBG(int selectQuestOneId, QuestStaticChapter.Category category = QuestStaticChapter.Category.INVALID, int selectEventId = 0)
	{
		string text = "PanelBg_HomeOut";
		string text2 = "";
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(selectQuestOneId);
		if (questOnePackData != null)
		{
			QuestStaticChapter questChapter = questOnePackData.questChapter;
			if (SceneQuest.IsMainStory(questChapter.category))
			{
				text = "PanelBg_HomeOut";
			}
			else if (questChapter.category == QuestStaticChapter.Category.GROW)
			{
				text = "PanelBg_GrowQuest";
			}
			else if (questChapter.category == QuestStaticChapter.Category.CHARA || questChapter.category == QuestStaticChapter.Category.ETCETERA)
			{
				text = "PanelBg_CharaQuest";
			}
			else if (questChapter.category == QuestStaticChapter.Category.EVENT)
			{
				flag3 = true;
			}
			else if (questChapter.category == QuestStaticChapter.Category.SIDE_STORY)
			{
				text = null;
				flag = true;
			}
			else if (questChapter.category == QuestStaticChapter.Category.TRAINING)
			{
				text = "PanelBg_Training";
			}
		}
		else if (category == QuestStaticChapter.Category.EVENT)
		{
			flag3 = true;
		}
		else if (category == QuestStaticChapter.Category.CHARA)
		{
			text = "PanelBg_CharaQuest";
		}
		if (flag3)
		{
			int num = QuestUtil.GetEventId(selectQuestOneId, false);
			if (num <= 0)
			{
				num = selectEventId;
			}
			DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(num);
			if (eventData != null && eventData.IsEnableChapter)
			{
				text = QuestUtil.EVENT_BG;
				switch (eventData.eventCategory)
				{
				case DataManagerEvent.Category.Scenario:
					text2 = eventData.bgFilename;
					text = null;
					goto IL_014F;
				case DataManagerEvent.Category.Tower:
					text2 = eventData.bgFilename;
					text = null;
					goto IL_014F;
				case DataManagerEvent.Category.Coop:
					text = null;
					flag2 = true;
					goto IL_014F;
				case DataManagerEvent.Category.WildRelease:
					text2 = eventData.bgFilename;
					text = null;
					goto IL_014F;
				}
				text = QuestUtil.EVENT_BG;
			}
		}
		IL_014F:
		if (text != null)
		{
			CanvasManager.SetBgObj(text);
			return;
		}
		if (flag)
		{
			CanvasManager.SetScenarioBgInSideStoryBgTexture(QuestUtil.ARAI_STORY_BG);
			return;
		}
		if (flag2)
		{
			CanvasManager.SetBgTexture(SelEventCoopCtrl.BG_NAME);
			return;
		}
		CanvasManager.SetScenarioBgInQuestBgTexture(text2);
	}

	public static List<QuestStaticQuestOne.ReleaseConditions> GetEnableReleaseConditionsList(int mapId)
	{
		List<QuestStaticQuestGroup> list = new List<QuestStaticQuestGroup>(DataManager.DmQuest.QuestStaticData.mapDataMap[mapId].questGroupList);
		list.Sort((QuestStaticQuestGroup a, QuestStaticQuestGroup b) => a.questGroupId - b.questGroupId);
		List<QuestStaticQuestOne> list2 = new List<QuestStaticQuestOne>(list[0].questOneList);
		if (list2.Count <= 0)
		{
			return new List<QuestStaticQuestOne.ReleaseConditions>();
		}
		list2.Sort((QuestStaticQuestOne a, QuestStaticQuestOne b) => a.questId - b.questId);
		return list2[0].ReleaseConditionsList.FindAll((QuestStaticQuestOne.ReleaseConditions item) => item.questId != 0);
	}

	public static void OpenReleaseConditionWindow(int mapId, bool needTime, QuestUIMapInfo questUIMapInfo)
	{
		List<QuestStaticQuestOne.ReleaseConditions> enableReleaseConditionsList = QuestUtil.GetEnableReleaseConditionsList(mapId);
		List<CmnReleaseConditionWindowCtrl.SetupParam> list = new List<CmnReleaseConditionWindowCtrl.SetupParam>();
		using (List<QuestStaticQuestOne.ReleaseConditions>.Enumerator enumerator = enableReleaseConditionsList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestStaticQuestOne.ReleaseConditions e = enumerator.Current;
				QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataList.Find((QuestDynamicQuestOne item) => item.questOneId == e.questId);
				QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(e.questId);
				string text = ((questOnePackData != null && questOnePackData.questChapter != null) ? SceneQuest.GetMainStoryName(questOnePackData.questChapter.category, true) : "");
				text += ((text != "") ? "\n" : "");
				list.Add(new CmnReleaseConditionWindowCtrl.SetupParam
				{
					enableClear = (questDynamicQuestOne != null && questDynamicQuestOne.status != QuestOneStatus.NEW),
					text = string.Concat(new string[]
					{
						text,
						" ",
						questOnePackData.questMap.mapName,
						" ",
						questOnePackData.questGroup.titleName,
						" ",
						questOnePackData.questOne.questName,
						" クリア"
					})
				});
			}
		}
		if (needTime && questUIMapInfo != null)
		{
			list.Add(new CmnReleaseConditionWindowCtrl.SetupParam
			{
				enableClear = (SceneQuest.TimeStampInScene >= questUIMapInfo.openTime),
				text = questUIMapInfo.openTime.ToString("M/d HH:mm") + " 以降"
			});
		}
		if (list.Count <= 0)
		{
			list.Add(new CmnReleaseConditionWindowCtrl.SetupParam
			{
				text = "挑戦可能なクエストはありません。"
			});
			CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("確認"), list);
			return;
		}
		CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), list);
	}

	public static bool HasUnsafeAreaLeft(GameObject mapBoxObject, float leftSidePosX)
	{
		RectTransform rectTransform = mapBoxObject.transform as RectTransform;
		Rect safeArea = SafeAreaScaler.GetSafeArea();
		if ((float)SafeAreaScaler.ScreenWidth == safeArea.width)
		{
			return false;
		}
		Vector2 vector;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, safeArea.position, mapBoxObject.GetComponentInParent<Canvas>().worldCamera, out vector);
		vector.x += rectTransform.rect.width * 0.5f;
		return vector.x > leftSidePosX;
	}

	public static float GetLeftSidePosX(GameObject mapBoxObject)
	{
		RectTransform rectTransform = mapBoxObject.transform as RectTransform;
		Transform transform = Singleton<CanvasManager>.Instance.outFrame.transform.Find("OutFrame03");
		Vector2 vector = (transform.gameObject.activeSelf ? RectTransformUtility.WorldToScreenPoint(transform.GetComponentInParent<Canvas>().worldCamera, transform.position) : new Vector2(0f, 0f));
		Vector2 vector2;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, vector, mapBoxObject.GetComponentInParent<Canvas>().worldCamera, out vector2);
		return vector2.x + rectTransform.rect.width * 0.5f;
	}

	public static bool HasUnsafeAreaRight(GameObject mapBoxObject, float rightSidePosX)
	{
		RectTransform rectTransform = mapBoxObject.transform as RectTransform;
		Rect safeArea = SafeAreaScaler.GetSafeArea();
		if ((float)SafeAreaScaler.ScreenWidth == safeArea.width)
		{
			return false;
		}
		Vector2 vector = new Vector2(safeArea.x + safeArea.width, safeArea.y);
		Vector2 vector2;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, vector, mapBoxObject.GetComponentInParent<Canvas>().worldCamera, out vector2);
		vector2.x += rectTransform.rect.width * 0.5f;
		return vector2.x < rightSidePosX;
	}

	public static float GetRightSidePosX(GameObject mapBoxObject)
	{
		RectTransform rectTransform = mapBoxObject.transform as RectTransform;
		Transform transform = Singleton<CanvasManager>.Instance.outFrame.transform.Find("OutFrame04");
		Vector2 vector = (transform.gameObject.activeSelf ? RectTransformUtility.WorldToScreenPoint(transform.GetComponentInParent<Canvas>().worldCamera, transform.position) : new Vector2((float)SafeAreaScaler.ScreenWidth, 0f));
		Vector2 vector2;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, vector, mapBoxObject.GetComponentInParent<Canvas>().worldCamera, out vector2);
		return vector2.x + rectTransform.rect.width * 0.5f;
	}

	public static QuestUtil.UsrQuestSkipInfo GetSkipInfo(DataManagerMonthlyPack.PurchaseMonthlypackData mpd, QuestOnePackData questOnePackData)
	{
		QuestUtil.UsrQuestSkipInfo usrQuestSkipInfo = new QuestUtil.UsrQuestSkipInfo
		{
			monthlyPackData = mpd
		};
		if (DataManager.DmEvent.isRaidByQuestOneId(questOnePackData.questOne.questId))
		{
			return usrQuestSkipInfo;
		}
		if (mpd == null || !mpd.SkippableFlag)
		{
			return usrQuestSkipInfo;
		}
		usrQuestSkipInfo.isSkippable = true;
		if (questOnePackData.questOne.skippableFlag < QuestUtil.SkipType.EnableDailyLimit && questOnePackData.questGroup.SkippableFlag < QuestUtil.SkipType.EnableDailyLimit)
		{
			return usrQuestSkipInfo;
		}
		usrQuestSkipInfo.hasSkipLimit = true;
		bool flag = questOnePackData.questOne.skippableFlag < QuestUtil.SkipType.EnableDailyLimit;
		QuestUtil.SkipType skipType = (flag ? questOnePackData.questGroup.SkippableFlag : questOnePackData.questOne.skippableFlag);
		string text = "";
		if (flag)
		{
			text = "同タブ内で\n";
		}
		switch (skipType)
		{
		case QuestUtil.SkipType.EnableDailyLimit:
			text += "本日";
			break;
		case QuestUtil.SkipType.EnableWeeklyLimit:
			text += "今週";
			break;
		case QuestUtil.SkipType.EnableTotalLimit:
			text += "期間中";
			break;
		}
		if (flag)
		{
			DataManager.DmQuest.CalcRestSkipNumByQuestGroupId(questOnePackData, ref usrQuestSkipInfo.restSkipCount, ref usrQuestSkipInfo.restSkipRecoveryCount);
		}
		else
		{
			DataManager.DmQuest.CalcRestSkipNumByQuestOneId(questOnePackData, ref usrQuestSkipInfo.restSkipCount, ref usrQuestSkipInfo.restSkipRecoveryCount);
		}
		usrQuestSkipInfo.isSkipByGroup = flag;
		usrQuestSkipInfo.prefixStr = text.Replace("同タブ内で\n", "");
		usrQuestSkipInfo.popupMessage = string.Format("{0}残り{1}回", text, usrQuestSkipInfo.restSkipCount);
		return usrQuestSkipInfo;
	}

	public static IEnumerator NoticeKizunaLimitReached(SelCharaDeckCtrl.RequestCallBack callback, int questOneId, List<int> charaIdList, int tryCount = 1, UnityAction<int> tryCountResetAction = null, int pvpKizunaExp = 0, int eventId = 0, bool isPractice = false)
	{
		int pushedButtonIndex = 0;
		decimal num = 0m;
		int DECIDE_BUTTON = 1;
		List<CharaPackData> targetCharaPackDatas = new List<CharaPackData>();
		List<HomeFurniturePackData> userHomeFurnitureList = DataManager.DmHome.GetUserHomeFurnitureList();
		Dictionary<HomeFurnitureStatic.Category, List<HomeFurniturePackData>> dictionary = new Dictionary<HomeFurnitureStatic.Category, List<HomeFurniturePackData>>();
		foreach (HomeFurniturePackData homeFurniturePackData in userHomeFurnitureList)
		{
			if (homeFurniturePackData.staticData.category != HomeFurnitureStatic.Category.INVALID)
			{
				if (!dictionary.ContainsKey(homeFurniturePackData.staticData.category))
				{
					dictionary.Add(homeFurniturePackData.staticData.category, new List<HomeFurniturePackData>());
				}
				dictionary[homeFurniturePackData.staticData.category].Add(homeFurniturePackData);
			}
		}
		if (!dictionary.ContainsKey(HomeFurnitureStatic.Category.ORNAMENT))
		{
			dictionary.Add(HomeFurnitureStatic.Category.ORNAMENT, new List<HomeFurniturePackData>());
		}
		dictionary[HomeFurnitureStatic.Category.ORNAMENT].Insert(0, null);
		if (!dictionary.ContainsKey(HomeFurnitureStatic.Category.CARPET))
		{
			dictionary.Add(HomeFurnitureStatic.Category.CARPET, new List<HomeFurniturePackData>());
		}
		dictionary[HomeFurnitureStatic.Category.CARPET].Insert(0, null);
		if (!dictionary.ContainsKey(HomeFurnitureStatic.Category.WINDOW))
		{
			dictionary.Add(HomeFurnitureStatic.Category.WINDOW, new List<HomeFurniturePackData>());
		}
		dictionary[HomeFurnitureStatic.Category.WINDOW].Insert(0, null);
		int num2 = 0;
		foreach (List<HomeFurniturePackData> list in dictionary.Values)
		{
			num2 += ((list.Count > 0 && list[0] == null) ? (list.Count - 1) : list.Count);
		}
		DataManagerMonthlyPack.PurchaseMonthlypackData validMonthlyPackData = DataManager.DmMonthlyPack.GetValidMonthlyPackData();
		HomeFurnitureCountData crnt = DataManager.DmHome.GetHomeFurnitureCountData(num2, false);
		int treehouseDeployBonusRatio = DataManager.DmTreeHouse.KizunaBonusData.GetKizunaBonusRatio(validMonthlyPackData != null);
		decimal divide = 10000m;
		DataManagerCampaign.CampaignKizunaData presentCampaignKizunaData = DataManager.DmCampaign.PresentCampaignKizunaData;
		decimal campaignBonus = ((presentCampaignKizunaData != null) ? (1 + (int)Math.Round(presentCampaignKizunaData.ratio / divide, 4)) : 0m);
		if (DataManager.DmTreeHouse.PutCharaDataList == null)
		{
			DataManager.DmTreeHouse.RequestGetCharaListData();
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
		}
		List<TreeHousePutCharaData> putCharaDataList = DataManager.DmTreeHouse.PutCharaDataList;
		using (List<int>.Enumerator enumerator3 = charaIdList.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				int charaId = enumerator3.Current;
				if (charaId != 0 && charaId != -1)
				{
					CharaPackData charaPackData = DataManager.DmChara.GetUserCharaData(charaId);
					if (charaPackData != null)
					{
						QuestOnePackData questOnePackData = null;
						if (pvpKizunaExp == 0)
						{
							questOnePackData = DataManager.DmQuest.GetQuestOnePackData(questOneId);
						}
						num = ((questOnePackData != null) ? questOnePackData.questOne.kizunaExp : pvpKizunaExp);
						if (questOnePackData != null && questOnePackData.questOne.kizunabonusCharaId == charaId)
						{
							num = (int)Math.Ceiling((double)(num * ((100 + questOnePackData.questOne.kizunabonusRatio) / 100)));
						}
						decimal num3 = crnt.kizunaPointIncrease;
						if (putCharaDataList != null && putCharaDataList.Exists((TreeHousePutCharaData x) => x != null && x.charaId == charaId))
						{
							num3 += treehouseDeployBonusRatio;
						}
						num *= 1m + Math.Round(num3 / divide, 4);
						if (pvpKizunaExp == 0 && questOnePackData != null && QuestUtil.IsExistCampaignKizuna(questOnePackData.questChapter.category) && campaignBonus != 0m)
						{
							num *= campaignBonus;
						}
						if (questOnePackData != null || eventId != 0)
						{
							DataManagerEvent.EventData eventData = null;
							if (questOnePackData != null)
							{
								eventData = DataManager.DmEvent.GetEventDataCompareToChapterId(questOnePackData.questChapter.chapterId);
							}
							else if (eventId != 0)
							{
								eventData = DataManager.DmEvent.GetEventData(eventId);
							}
							if (eventData != null)
							{
								DateTime now = TimeManager.Now;
								DataManagerChara.BonusCharaData bonusCharaData = DataManager.DmChara.GetBonusCharaDataList(eventData.eventId, now).Find((DataManagerChara.BonusCharaData x) => x.charaId == charaId);
								if (bonusCharaData != null && bonusCharaData.kizunaBonusRatio != 0)
								{
									decimal num4 = 1000m;
									decimal num5 = 1m + Math.Round(bonusCharaData.kizunaBonusRatio / num4, 4);
									num *= num5;
								}
							}
						}
						long num6 = 0L;
						for (int i = charaPackData.dynamicData.kizunaLevel; i < charaPackData.dynamicData.KizunaLimitLevel; i++)
						{
							int idx = i + 1;
							GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList.Find((GameLevelInfo x) => x.level == idx);
							if (gameLevelInfo != null && gameLevelInfo.kizunaLevelExp.ContainsKey(charaPackData.staticData.baseData.kizunaLevelId))
							{
								num6 += gameLevelInfo.kizunaLevelExp[charaPackData.staticData.baseData.kizunaLevelId].LevelExp;
							}
						}
						int num7 = (int)(num6 - charaPackData.dynamicData.kizunaExp);
						DataManager.DmServerMst.gameLevelInfoList.Find((GameLevelInfo x) => x.level == charaPackData.dynamicData.KizunaLimitLevel + 1);
						if (charaPackData.dynamicData.KizunaLimitLevel <= charaPackData.dynamicData.kizunaLevel)
						{
							num7 = 0;
						}
						num = Math.Ceiling(num);
						num *= tryCount;
						if (num7 - num < 0m)
						{
							targetCharaPackDatas.Add(charaPackData);
						}
					}
				}
			}
		}
		GameObject gameObject = CanvasManager.HdlKizunaReachedLimitWindowCtrl.transform.Find("Base/Window/IconCharaList").gameObject;
		foreach (GameObject gameObject2 in gameObject.GetChildList())
		{
			Object.Destroy(gameObject2);
		}
		if (tryCountResetAction != null)
		{
			tryCountResetAction(-1);
		}
		if (targetCharaPackDatas.Count <= 0 || DataManager.DmUserInfo.dispKizunaConfirm == 1 || isPractice)
		{
			callback();
			yield break;
		}
		QuestUtil.<>c__DisplayClass64_4 CS$<>8__locals5 = new QuestUtil.<>c__DisplayClass64_4();
		int childCount = CanvasManager.HdlKizunaReachedLimitWindowCtrl.transform.parent.childCount;
		CanvasManager.HdlKizunaReachedLimitWindowCtrl.transform.SetSiblingIndex(childCount - 1);
		CanvasManager.HdlKizunaReachedLimitWindowCtrl.Setup("確認", "<color=red>このクエストで獲得できるなかよしポイントで\nなかよしレベル上限までの残り経験値を超過してしまうフレンズが編成されています。\n</color>\n超過分は獲得できませんがよろしいですか？\n【対象のフレンズ】\n", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int index)
		{
			pushedButtonIndex = index;
			return true;
		}, null, false);
		CS$<>8__locals5.isChecked = false;
		PguiButtonCtrl component = CanvasManager.HdlKizunaReachedLimitWindowCtrl.transform.Find("Base/Window/Btn_ConfirmCheckBox").GetComponent<PguiButtonCtrl>();
		CS$<>8__locals5.checkImage = CanvasManager.HdlKizunaReachedLimitWindowCtrl.transform.Find("Base/Window/Btn_ConfirmCheckBox/BaseImage/Img_Check").gameObject;
		CS$<>8__locals5.checkImage.SetActive(CS$<>8__locals5.isChecked);
		component.AddOnClickListener(delegate(PguiButtonCtrl x)
		{
			CS$<>8__locals5.isChecked = !CS$<>8__locals5.isChecked;
			CS$<>8__locals5.checkImage.gameObject.SetActive(CS$<>8__locals5.isChecked);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		foreach (CharaPackData charaPackData2 in targetCharaPackDatas)
		{
			IconCharaCtrl component2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara).GetComponent<IconCharaCtrl>();
			component2.transform.SetParent(gameObject.transform, false);
			component2.Setup(charaPackData2, SortFilterDefine.SortType.KIZUNA, false, null, 0, -1, 0);
			component2.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		CanvasManager.HdlKizunaReachedLimitWindowCtrl.Open();
		do
		{
			if (pushedButtonIndex == DECIDE_BUTTON)
			{
				if (CS$<>8__locals5.isChecked && !DataManager.IsServerRequesting())
				{
					DataManager.DmUserInfo.RequestActionUpdateKizunaConfirm(1);
				}
				do
				{
					yield return null;
				}
				while (DataManager.IsServerRequesting());
				callback();
			}
			yield return null;
		}
		while (!CanvasManager.HdlKizunaReachedLimitWindowCtrl.FinishedClose());
		CS$<>8__locals5 = null;
		yield return null;
		yield break;
	}

	private static bool IsExistCampaignKizuna(QuestStaticChapter.Category category)
	{
		List<QuestStaticChapter> chapterDataList = DataManager.DmQuest.QuestStaticData.chapterDataList;
		List<QuestStaticMap> mapDataList = DataManager.DmQuest.QuestStaticData.mapDataList;
		List<QuestStaticQuestGroup> groupDataList = DataManager.DmQuest.QuestStaticData.groupDataList;
		long ticks = TimeManager.Now.Ticks;
		List<DataManagerCampaign.CampaignKizunaData> list = new List<DataManagerCampaign.CampaignKizunaData>(DataManager.DmCampaign.CampaignKizunaDataList);
		list.Sort((DataManagerCampaign.CampaignKizunaData a, DataManagerCampaign.CampaignKizunaData b) => b.campaignId - a.campaignId);
		foreach (DataManagerCampaign.CampaignKizunaData campaignKizunaData in list)
		{
			if (ticks >= campaignKizunaData.startTime.Ticks && ticks <= campaignKizunaData.endTime.Ticks && campaignKizunaData.campaignTargetList.Count > 0)
			{
				using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignKizunaData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.QuestGroup).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DataManagerCampaign.CampaignTarget target2 = enumerator2.Current;
						QuestStaticQuestGroup group = groupDataList.Find((QuestStaticQuestGroup item) => item.questGroupId == target2.TargetId);
						if (group != null)
						{
							Predicate<QuestStaticQuestGroup> <>9__5;
							QuestStaticMap map = mapDataList.Find(delegate(QuestStaticMap item)
							{
								List<QuestStaticQuestGroup> questGroupList = item.questGroupList;
								Predicate<QuestStaticQuestGroup> predicate;
								if ((predicate = <>9__5) == null)
								{
									predicate = (<>9__5 = (QuestStaticQuestGroup item1) => item1.questGroupId == group.questGroupId);
								}
								return questGroupList.Find(predicate) != null;
							});
							if (map != null)
							{
								QuestStaticChapter questStaticChapter = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == map.chapterId);
								if (questStaticChapter != null && questStaticChapter.category == category)
								{
									return true;
								}
							}
						}
					}
				}
				using (List<DataManagerCampaign.CampaignTarget>.Enumerator enumerator2 = campaignKizunaData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.Chapter).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DataManagerCampaign.CampaignTarget target = enumerator2.Current;
						QuestStaticChapter questStaticChapter2 = chapterDataList.Find((QuestStaticChapter item) => item.chapterId == target.TargetId);
						if (questStaticChapter2 != null && questStaticChapter2.category == category)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public static List<DataManagerPhoto.CalcDropBonusResult> GetCalcDropBonusResultDeck(QuestOnePackData qopd, List<PhotoPackData> effectPhotoList, List<DataManagerChara.BonusCharaData> dropBonusCharaList, List<int> pocketReleaseCountList)
	{
		List<DataManagerPhoto.CalcDropBonusResult> list = new List<DataManagerPhoto.CalcDropBonusResult>();
		if (qopd != null)
		{
			List<int> photoBonusTargetItemIdByTime = DataManager.DmPhoto.GetPhotoBonusTargetItemIdByTime(TimeManager.Now);
			list = DataManager.DmPhoto.CalcPhotoBonus(effectPhotoList, TimeManager.Now, null);
			list = DataManager.DmChara.CalcDropBonus(list, dropBonusCharaList, pocketReleaseCountList);
			list.RemoveAll((DataManagerPhoto.CalcDropBonusResult item) => !qopd.questOne.DropItemList.Contains(item.targetItemId));
			using (HashSet<int>.Enumerator enumerator = qopd.questOne.DropItemList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int itm = enumerator.Current;
					if (photoBonusTargetItemIdByTime.Contains(itm) && list.Find((DataManagerPhoto.CalcDropBonusResult item) => itm == item.targetItemId) == null)
					{
						list.Add(new DataManagerPhoto.CalcDropBonusResult
						{
							targetItemId = itm,
							ratio = 0
						});
					}
				}
			}
		}
		return list;
	}

	public static int GetEventId(int questOneId, bool onlyGrowth)
	{
		int num = 0;
		QuestOnePackData qopd = DataManager.DmQuest.GetQuestOnePackData(questOneId);
		if (qopd != null)
		{
			Predicate<int> <>9__0;
			foreach (DataManagerEvent.EventData eventData in DataManager.DmEvent.GetEventDataList())
			{
				if (eventData.IsEnableEvent)
				{
					if (onlyGrowth)
					{
						if (eventData.eventCategory != DataManagerEvent.Category.Growth)
						{
							continue;
						}
						List<int> growthQuestGroupList = eventData.GrowthQuestGroupList;
						Predicate<int> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = (int item) => item == qopd.questGroup.questGroupId);
						}
						if (!growthQuestGroupList.Exists(predicate))
						{
							break;
						}
					}
					if (eventData.eventChapterId == qopd.questChapter.chapterId)
					{
						num = eventData.eventId;
						break;
					}
				}
			}
		}
		return num;
	}

	public static bool IsBanTarget(CharaStaticData csd, QuestOnePackData qopd, List<CharaStaticData> checkedList = null)
	{
		bool flag = QuestUtil.IsBanTargetBySealed(csd, qopd);
		if (flag)
		{
			return flag;
		}
		if (checkedList == null)
		{
			checkedList = new List<CharaStaticData>();
		}
		return QuestUtil.IsBanTargetByRules(csd, checkedList, qopd);
	}

	public static bool IsBanTarget(CharaDynamicData cdd, QuestOnePackData qopd, List<CharaStaticData> checkedList = null)
	{
		if (cdd == null)
		{
			return false;
		}
		HelperPackData latestHelper = DataManager.DmHelper.GetLatestHelper();
		if (latestHelper != null)
		{
			int latestAttrIdx = DataManager.DmHelper.GetLatestAttrIdx();
			bool flag = latestHelper.friendId == DataManager.DmUserInfo.friendId;
			bool flag2 = cdd.OwnerType != CharaDynamicData.CharaOwnerType.Helper && cdd.id == latestHelper.HelperCharaSetList[latestAttrIdx].helpChara.id;
			if (flag && flag2)
			{
				return true;
			}
		}
		CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(cdd.id);
		bool flag3 = QuestUtil.IsBanTargetBySealed(charaStaticData, qopd);
		if (flag3)
		{
			return flag3;
		}
		if (checkedList == null)
		{
			checkedList = new List<CharaStaticData>();
		}
		return QuestUtil.IsBanTargetByRules(charaStaticData, checkedList, qopd);
	}

	private static bool IsBanTargetByRules(CharaStaticData csd, List<CharaStaticData> checkedList, QuestOnePackData qopd)
	{
		if (qopd == null || qopd.questOne == null || qopd.questOne.ruleId == 0)
		{
			return false;
		}
		bool flag = false;
		foreach (QuestStaticRule questStaticRule in DataManager.DmQuest.QuestStaticData.ruleDataList.FindAll((QuestStaticRule item) => item.ruleId == qopd.questOne.ruleId))
		{
			flag = QuestUtil.CheckRuleType(csd, questStaticRule, checkedList);
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	private static bool IsBanTargetBySealed(CharaStaticData csd, QuestOnePackData qopd)
	{
		if (qopd == null || qopd.questGroup == null || !qopd.questGroup.limitGroupFlag || csd == null)
		{
			return false;
		}
		int target = qopd.questGroup.questGroupId;
		int questId = qopd.questOne.questId;
		List<DataManagerQuest.QuestSealedCharaData> list = DataManager.DmQuest.QuestSealedCharaDatas.FindAll((DataManagerQuest.QuestSealedCharaData item) => item.target == target && item.questOneId != questId);
		bool flag = false;
		Predicate<int> <>9__1;
		foreach (DataManagerQuest.QuestSealedCharaData questSealedCharaData in list)
		{
			List<int> sealedList = questSealedCharaData.sealedList;
			Predicate<int> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = (int item) => item == csd.baseData.id);
			}
			flag = sealedList.Exists(predicate);
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	private static bool CheckRuleType(CharaStaticData csd, QuestStaticRule rule, List<CharaStaticData> checkedList)
	{
		if (csd == null)
		{
			return false;
		}
		bool flag = false;
		bool flag2 = rule.detail == QuestStaticRule.DetailType.ENABLE;
		CharaStaticBase baseData = csd.baseData;
		switch (rule.ruleType)
		{
		case QuestStaticRule.RuleType.SAME_ATTRIBUTE:
		{
			bool flag3 = checkedList.Count != 0 && checkedList[0].baseData.attribute == baseData.attribute;
			flag = checkedList.Count != 0 && (flag2 ? (!flag3) : flag3);
			break;
		}
		case QuestStaticRule.RuleType.SPECIFIED_ATTRIBUTE:
		{
			bool flag4 = baseData.attribute == (CharaDef.AttributeType)rule.target;
			flag = (flag2 ? (!flag4) : flag4);
			break;
		}
		case QuestStaticRule.RuleType.SPECIFIED_CHARA:
		{
			bool flag5 = baseData.id == rule.target;
			flag = (flag2 ? (!flag5) : flag5);
			break;
		}
		case QuestStaticRule.RuleType.HC:
		{
			bool flag6 = baseData.OriginalId != 0;
			flag = (flag2 ? (!flag6) : flag6);
			break;
		}
		case QuestStaticRule.RuleType.SUB_ATTRIBUTE:
		{
			bool flag7 = baseData.subAttribute > CharaDef.AttributeType.ALL;
			flag = (flag2 ? (!flag7) : flag7);
			break;
		}
		}
		return flag;
	}

	public static IEnumerator NoticeBanned(SelCharaDeckCtrl.RequestCallBack callback, QuestOnePackData qopd, UserDeckData clone, CharaPackData removeButton, List<CharaPackData> haveCharaPackList)
	{
		QuestUtil.<>c__DisplayClass73_0 CS$<>8__locals1 = new QuestUtil.<>c__DisplayClass73_0();
		CS$<>8__locals1.removeButton = removeButton;
		CS$<>8__locals1.clone = clone;
		CS$<>8__locals1.callback = callback;
		bool flag = false;
		List<CharaStaticData> list = new List<CharaStaticData>();
		int num = 0;
		bool flag2 = true;
		int i;
		int j;
		for (i = 0; i < CS$<>8__locals1.clone.charaIdList.Count; i = j + 1)
		{
			CharaPackData charaPackData = haveCharaPackList.Find((CharaPackData item) => item != CS$<>8__locals1.removeButton && item.id == CS$<>8__locals1.clone.charaIdList[i]);
			if (charaPackData != null)
			{
				bool flag3 = CS$<>8__locals1.clone.GetHelperIndex() == i;
				bool flag4 = QuestUtil.IsBanTarget(charaPackData.dynamicData, qopd, list);
				if (flag4 && charaPackData != null)
				{
					flag = true;
					num++;
				}
				else if (!flag4 && !flag3 && charaPackData != null)
				{
					flag2 = false;
				}
				if (charaPackData != null)
				{
					list.Add(charaPackData.staticData);
				}
			}
			j = i;
		}
		if (!flag)
		{
			CS$<>8__locals1.callback();
			yield break;
		}
		if (list.Count <= num || flag2)
		{
			string text = "<color=red>編成されているすべてのフレンズが\n編成条件を満たしていないため、クエストに挑戦できません</color>";
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("エラー"), PrjUtil.MakeMessage(text), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		}
		else
		{
			string text2 = "<color=red>編成条件を満たさないフレンズが\nパーティに編成されています\nこのままクエストに進んでもよろしいですか\n\n※条件を満たさないフレンズはクエストに参加できません。</color>";
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage(text2), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
			{
				if (index == 1)
				{
					CS$<>8__locals1.callback();
				}
				return true;
			}, null, false);
		}
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		yield break;
	}

	public static IEnumerator OpenQuestSealedInfo(int target)
	{
		List<DataManagerQuest.QuestSealedCharaData> list = DataManager.DmQuest.QuestSealedCharaDatas.FindAll((DataManagerQuest.QuestSealedCharaData item) => item.target == target);
		if (list == null || list.Count == 0)
		{
			yield break;
		}
		CanvasManager.HdlOpenWindowQUestSealedInfo.sealedInfoData.Setup(list);
		CanvasManager.HdlOpenWindowQUestSealedInfo.sealedInfoData.owCtrl.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowQUestSealedInfo.sealedInfoData.owCtrl.FinishedClose());
		yield break;
	}

	public static readonly string ARAI_STORY_BG = "Texture2D/Bg_Scenario/Jungle/bg_jungle";

	public static readonly string EVENT_BG = "PanelBg_Event";

	public static readonly float MOVING_SQR_MAGNITUDE = 40000f;

	public static readonly float DEFAULT_SCREEN_WIDTH = 1280f;

	public static readonly float DEFAULT_BG_WIDTH = 1654f;

	public static readonly float DEFAULT_SCREEN_HEIGHT = 720f;

	public static readonly float MAP_MASK_IMAGE_WIDTH = 89f;

	public static readonly float MAP_MASK_IMAGE_HEIGHT = 42f;

	public static readonly float BG_OFFSET_POS_Y = QuestUtil.DEFAULT_SCREEN_HEIGHT;

	public static readonly DateTime QuestClearDefaultDateTime = new DateTime(2000, 1, 1, 0, 0, 0);

	public static readonly string TitleMain = "メインストーリー";

	public static readonly string TitleMain2 = "メインストーリーS2";

	public static readonly string TitleMain3 = "メインストーリーS3";

	public static readonly string TitleCellval = "セーバルぶらり旅";

	public static readonly string TitleFriends = "フレンズストーリー";

	public static readonly string TitleEvent = "イベントストーリー";

	public static readonly string TitleArai = "アライさん隊長日誌";

	public static readonly string TitleGrow = "成長クエスト";

	public static readonly string TitleEtcetera = "すぺしゃるクエスト";

	public static readonly string MapTexturePrefix = "map_texture0";

	public static readonly string Map2TexturePrefix = "map2_texture0";

	public static readonly string Map3TexturePrefix = "map3_texture0";

	public static readonly string MapTextureFolder = "QuestMap";

	public static readonly string MapTextureCellvalPrefix = "cellval_map_texture0";

	public static readonly string MapTextureCellvalFolder = "CellvalMap";

	public static readonly string WindowWord01 = "本日の残りクリア回数が0になりました。\nクリア回数は毎日0:00に回復します。";

	public static readonly string WindowWord02 = "今回の残りクリア回数が0になりました。";

	private static readonly int CampaignMaxCount = 1;

	private static readonly float Rate10000 = 10000f;

	public delegate QuestUtil.SelectData OnGetSelectData();

	public delegate bool OnCheck();

	public class ItemOwnBase
	{
		public ItemOwnBase(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Num_Txt = baseTr.Find("Num_Txt").GetComponent<PguiTextCtrl>();
			this.Icon_Stone = baseTr.Find("Icon_Stone").GetComponent<PguiRawImageCtrl>();
		}

		public GameObject baseObj;

		public PguiTextCtrl Num_Txt;

		public PguiRawImageCtrl Icon_Stone;
	}

	public class QuestListBarCmnInfo
	{
		public QuestListBarCmnInfo(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.goQuestInfo = baseTr.Find("QuestImfo").gameObject;
			this.Icon_PresentBox = baseTr.Find("Icon_PresentBox").GetComponent<PguiImageCtrl>();
			this.Mark_Complete = baseTr.Find("Mark_Complete").GetComponent<PguiImageCtrl>();
			this.DifficultInfo = baseTr.Find("ModeInfo/DifficultInfo").gameObject;
			this.numDifficult = this.DifficultInfo.transform.Find("Num_Difficult").GetComponent<PguiTextCtrl>();
			this.colorDifficult = this.DifficultInfo.transform.Find("BaseImage").GetComponent<PguiColorCtrl>();
			this.StaminaInfo = baseTr.Find("ModeInfo/StaminaInfo").gameObject;
			this.numStamina = this.StaminaInfo.transform.Find("Num_Stamina").GetComponent<PguiTextCtrl>();
			this.colorStamina = this.StaminaInfo.transform.Find("BaseImage").GetComponent<PguiColorCtrl>();
			this.Icon_Item = this.StaminaInfo.transform.Find("Icon_Item").GetComponent<PguiRawImageCtrl>();
			this.Item_Num_Use = this.Icon_Item.transform.Find("Num_Use").GetComponent<PguiTextCtrl>();
			this.textQuestName = this.goQuestInfo.transform.Find("Txt_QuestName").GetComponent<PguiTextCtrl>();
			this.Num_Count = this.goQuestInfo.transform.Find("Num_Count").GetComponent<PguiTextCtrl>();
			this.Mark_NumPlus = this.Num_Count.transform.Find("Mark_NumPlus").GetComponent<PguiImageCtrl>();
			this.Mark_NotContinue = this.goQuestInfo.transform.Find("Mark_NotContinue").GetComponent<PguiImageCtrl>();
			this.Mark_NotContinue.gameObject.SetActive(false);
			this.Mark_Story = this.goQuestInfo.transform.Find("Mark_Story").GetComponent<PguiImageCtrl>();
			this.Mark_Story.gameObject.SetActive(false);
			this.Mark_NotDhole = this.goQuestInfo.transform.Find("Mark_NotDhole").GetComponent<PguiImageCtrl>();
			this.Mark_NotDhole.gameObject.SetActive(false);
			this.Mark_NotLeader = this.goQuestInfo.transform.Find("Mark_NotLeader").GetComponent<PguiImageCtrl>();
			this.Mark_NotLeader.gameObject.SetActive(false);
			this.goMissionFlagSub = new PguiImageCtrl[]
			{
				baseTr.Find("Memori_Mission/Memori_Mission01").GetComponent<PguiImageCtrl>(),
				baseTr.Find("Memori_Mission/Memori_Mission02").GetComponent<PguiImageCtrl>(),
				baseTr.Find("Memori_Mission/Memori_Mission03").GetComponent<PguiImageCtrl>()
			};
			this.iconItemCtrl = baseTr.Find("Icon_Item").GetComponent<IconItemCtrl>();
			this.iconItemCtrl.SetRaycastTargetIconItem(false);
		}

		public void Setup(QuestUtil.QuestListBarCmnInfo.SetupParam setupParam)
		{
			int num = -1;
			foreach (DataManagerCampaign.CampaignQuestStaminaData campaignQuestStaminaData in DataManager.DmCampaign.PresentCampaignQuestStaminaDataList)
			{
				foreach (DataManagerCampaign.CampaignTarget campaignTarget in campaignQuestStaminaData.campaignTargetList.FindAll((DataManagerCampaign.CampaignTarget item) => item.TargetType == DataManagerCampaign.TARGET_TYPE.Chapter))
				{
					if (setupParam.selectData.chapterId == campaignTarget.TargetId)
					{
						num = campaignQuestStaminaData.value;
						break;
					}
				}
				if (num >= 0)
				{
					break;
				}
			}
			this.Mark_NotContinue.gameObject.SetActive(setupParam.qsqo.ContinueImpossible);
			this.Mark_Story.gameObject.SetActive(setupParam.qsqo.StoryOnly);
			this.Mark_NotLeader.gameObject.SetActive(setupParam.qsqo.QuestCategory == QuestStaticQuestOne.QuestOneCategory.NoPlayer);
			this.Mark_NotDhole.gameObject.SetActive(setupParam.qsqo.QuestCategory == QuestStaticQuestOne.QuestOneCategory.NoDhole);
			this.textQuestName.text = setupParam.qsqo.questName;
			BattleMissionPack battleMissionPack = DataManager.DmQuest.GetBattleMissionPack(setupParam.qsqo.questId);
			for (int i = 0; i < this.goMissionFlagSub.Length; i++)
			{
				bool flag = battleMissionPack.staticData.mission[i].type > BattleMissionType.INVALID;
				this.goMissionFlagSub[i].gameObject.SetActive(flag);
				if (flag)
				{
					bool flag2 = battleMissionPack.clearFlag[i];
					this.goMissionFlagSub[i].SetImageByName(flag2 ? "questbar_missionmemori_act" : "questbar_missionmemori_nor");
				}
			}
			bool flag3 = DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(setupParam.qsqo.questId);
			this.goMissionFlagSub[0].transform.parent.gameObject.SetActive(flag3);
			string text = "NORMAL";
			if (setupParam.enableChangeColor)
			{
				if (setupParam.selectData.questCategory == QuestStaticChapter.Category.SIDE_STORY)
				{
					text = "ARAI";
				}
				else
				{
					text = (QuestUtil.IsHardMode(setupParam.selectData) ? "HARD" : "NORMAL");
				}
				PguiGradientCtrl component = this.numDifficult.GetComponent<PguiGradientCtrl>();
				if (component != null)
				{
					Color outlineById = component.GetOutlineById(text);
					Outline[] array = this.numDifficult.GetComponents<Outline>();
					for (int j = 0; j < array.Length; j++)
					{
						array[j].effectColor = outlineById;
					}
					array = this.DifficultInfo.transform.Find("Txt_Difficult").GetComponents<Outline>();
					for (int j = 0; j < array.Length; j++)
					{
						array[j].effectColor = outlineById;
					}
				}
			}
			PguiGradientCtrl component2 = this.numStamina.GetComponent<PguiGradientCtrl>();
			if (component2 != null)
			{
				Color outlineById2 = component2.GetOutlineById((num >= 0) ? "DISCOUNT" : text);
				Outline[] array = this.numStamina.GetComponents<Outline>();
				for (int j = 0; j < array.Length; j++)
				{
					array[j].effectColor = outlineById2;
				}
				Gradient gradientById = component2.GetGradientById((num >= 0) ? "DISCOUNT" : text);
				this.numStamina.GetComponent<GradationText>().EffectGradient = gradientById;
				Color outlineById3 = component2.GetOutlineById(text);
				array = this.StaminaInfo.transform.Find("Txt_Stamina").GetComponents<Outline>();
				for (int j = 0; j < array.Length; j++)
				{
					array[j].effectColor = outlineById3;
				}
			}
			this.colorDifficult.GetComponent<Image>().color = this.colorDifficult.GetGameObjectById(text);
			this.colorStamina.GetComponent<Image>().color = this.colorStamina.GetGameObjectById(text);
			int useItemId = setupParam.qsqo.useItemId;
			int useItemNum = setupParam.qsqo.useItemNum;
			this.Icon_Item.gameObject.SetActive(useItemId != 0);
			this.numStamina.gameObject.SetActive(useItemId == 0);
			this.StaminaInfo.transform.Find("Txt_Stamina").gameObject.SetActive(useItemId == 0);
			if (useItemId != 0)
			{
				this.Icon_Item.SetRawImage(DataManager.DmItem.GetItemStaticBase(useItemId).GetIconName(), true, false, null);
				PguiGradientCtrl component3 = this.Item_Num_Use.GetComponent<PguiGradientCtrl>();
				if (component3 != null)
				{
					Color outlineById4 = component3.GetOutlineById(text);
					Outline[] array = this.Item_Num_Use.GetComponents<Outline>();
					for (int j = 0; j < array.Length; j++)
					{
						array[j].effectColor = outlineById4;
					}
				}
			}
			this.DifficultInfo.SetActive(!setupParam.qsqo.StoryOnly);
			this.numDifficult.text = setupParam.qsqo.difficulty.ToString();
			int num2 = ((num >= 0) ? (setupParam.qsqo.stamina - num) : setupParam.qsqo.stamina);
			if (num2 < 0)
			{
				num2 = 0;
			}
			this.numStamina.text = string.Format("{0}", num2);
			if (useItemId != 0)
			{
				int num3 = DataManager.DmItem.GetUserItemData(useItemId).num;
				this.Item_Num_Use.text = ((num3 < useItemNum) ? string.Format("{0}{1}{2}/{3}", new object[]
				{
					PrjUtil.ColorRedStartTag,
					num3,
					PrjUtil.ColorEndTag,
					useItemNum
				}) : string.Format("{0}/{1}", num3, useItemNum));
			}
			this.Mark_Complete.gameObject.SetActive(setupParam.questOneStatus == QuestOneStatus.COMPLETE);
			this.Icon_PresentBox.gameObject.SetActive(false);
			this.iconItemCtrl.Setup(battleMissionPack.staticData.completeBonus);
			int num4 = DataManager.DmQuest.CalcRestPlayNumByQuestOneId(setupParam.qsqo.questId);
			ItemInput recoveryKeyItem = setupParam.qsqo.RecoveryKeyItem;
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(setupParam.qsqo.questId);
			if (num4 == 0)
			{
				if (recoveryKeyItem != null)
				{
					bool flag4 = questOnePackData.questDynamicOne.todayRecoveryNum >= setupParam.qsqo.RecoveryMaxNum;
				}
			}
			bool flag5 = recoveryKeyItem != null;
			bool flag6 = setupParam.restGroupNum < 0;
			this.Num_Count.gameObject.SetActive(flag6 && num4 >= 0);
			string text2 = "本日残り";
			if (DataManager.DmEvent.isRaidByQuestOneId(setupParam.qsqo.questId))
			{
				text2 = "今回残り";
			}
			this.Num_Count.text = ((flag6 && num4 >= 0) ? (text2 + num4.ToString() + PrjUtil.MakeMessage("回")) : "");
			this.Mark_NumPlus.gameObject.SetActive(num4 >= 0 && flag5);
		}

		public void SetActiveQuestInfo(bool sw)
		{
			this.goQuestInfo.SetActive(sw);
		}

		public void SetActiveIconItem(bool sw)
		{
			this.iconItemCtrl.gameObject.SetActive(sw);
		}

		public void SetActiveIconPresentBox(bool sw)
		{
			this.Icon_PresentBox.gameObject.SetActive(sw);
		}

		public GameObject baseObj;

		public GameObject DifficultInfo;

		public GameObject StaminaInfo;

		public PguiRawImageCtrl Icon_Item;

		public IconItemCtrl iconItemCtrl;

		public PguiImageCtrl Icon_PresentBox;

		public PguiImageCtrl Mark_Complete;

		public GameObject goQuestInfo;

		public PguiTextCtrl numDifficult;

		public PguiTextCtrl numStamina;

		public PguiColorCtrl colorDifficult;

		public PguiColorCtrl colorStamina;

		public PguiTextCtrl Item_Num_Use;

		public PguiTextCtrl textQuestName;

		public PguiTextCtrl Num_Count;

		public PguiImageCtrl Mark_NumPlus;

		public PguiImageCtrl Mark_NotContinue;

		public PguiImageCtrl Mark_Story;

		public PguiImageCtrl Mark_NotDhole;

		public PguiImageCtrl Mark_NotLeader;

		public PguiImageCtrl[] goMissionFlagSub;

		public class SetupParam
		{
			public QuestStaticQuestOne qsqo;

			public QuestUtil.SelectData selectData;

			public QuestOneStatus questOneStatus;

			public int restGroupNum;

			public bool enableChangeColor;
		}
	}

	public class SelectData
	{
		public SelectData()
		{
			this.chapterId = 0;
			this.mapId = 0;
			this.questOneId = 0;
			this.friendId = 0;
			this.eventId = 0;
			this.questGroupId = 0;
			this.questCategory = QuestStaticChapter.Category.INVALID;
		}

		public SelectData(QuestUtil.SelectData data)
		{
			this.chapterId = data.chapterId;
			this.mapId = data.mapId;
			this.questOneId = data.questOneId;
			this.friendId = data.friendId;
			this.eventId = data.eventId;
			this.questGroupId = data.questGroupId;
			this.questCategory = data.questCategory;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			QuestUtil.SelectData selectData = obj as QuestUtil.SelectData;
			return selectData != null && (this.chapterId == selectData.chapterId && this.mapId == selectData.mapId) && this.questCategory == selectData.questCategory;
		}

		public bool Equals(QuestUtil.SelectData p)
		{
			return p != null && (this.chapterId == p.chapterId && this.mapId == p.mapId) && this.questCategory == p.questCategory;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public int chapterId;

		public int mapId;

		public int questOneId;

		public int friendId;

		public int eventId;

		public int questGroupId;

		public QuestStaticChapter.Category questCategory;
	}

	public class CampaignInfo
	{
		public CampaignInfo(Transform baseTr)
		{
			this.go = baseTr.Find("SelCmn_CampaignInfo_Quest").gameObject;
			this.Txt_Campaign = baseTr.Find("SelCmn_CampaignInfo_Quest/Txt_Campaign").GetComponent<PguiTextCtrl>();
			this.Txt_CampaignTime = baseTr.Find("SelCmn_CampaignInfo_Quest/TimeInfo/Num_Time").GetComponent<PguiTextCtrl>();
			this.count = 0;
			this.start = false;
		}

		public void ResetCampaign()
		{
			this.start = false;
			this.count = 0;
			SimpleAnimation componentInChildren = this.go.GetComponentInChildren<SimpleAnimation>();
			if (componentInChildren != null)
			{
				componentInChildren.ExInit();
				componentInChildren.ExStop(true);
			}
		}

		public void DispCampaign(List<string> msgList, List<string> timeList)
		{
			bool flag = msgList.Count > 0 && timeList.Count > 0;
			this.go.SetActive(flag);
			if (flag)
			{
				SimpleAnimation anim = this.go.GetComponentInChildren<SimpleAnimation>();
				if (this.count >= msgList.Count)
				{
					this.count = msgList.Count - 1;
					anim.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START);
				}
				if (this.count >= timeList.Count)
				{
					this.count = timeList.Count - 1;
					anim.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START);
				}
				this.Txt_Campaign.text = msgList[this.count];
				this.Txt_CampaignTime.text = timeList[this.count];
				if (msgList.Count >= 2 && timeList.Count >= 2 && !this.start)
				{
					this.start = true;
					anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START.ToString(), 0f, 0f);
					SimpleAnimation.ExFinishCallback <>9__2;
					SimpleAnimation.ExFinishCallback <>9__1;
					anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
					{
						anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START_SUB.ToString(), 0f, 0f);
						SimpleAnimation anim3 = anim;
						SimpleAnimation.ExPguiStatus exPguiStatus = SimpleAnimation.ExPguiStatus.START_SUB;
						SimpleAnimation.ExFinishCallback exFinishCallback;
						if ((exFinishCallback = <>9__1) == null)
						{
							exFinishCallback = (<>9__1 = delegate
							{
								anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END.ToString(), 0f, 0f);
								SimpleAnimation anim2 = anim;
								SimpleAnimation.ExPguiStatus exPguiStatus2 = SimpleAnimation.ExPguiStatus.END;
								SimpleAnimation.ExFinishCallback exFinishCallback2;
								if ((exFinishCallback2 = <>9__2) == null)
								{
									exFinishCallback2 = (<>9__2 = delegate
									{
										this.count++;
										if (this.count >= msgList.Count)
										{
											this.count = 0;
										}
										if (this.count >= timeList.Count)
										{
											this.count = 0;
										}
										this.start = false;
										this.DispCampaign(msgList, timeList);
									});
								}
								anim2.ExPlayAnimation(exPguiStatus2, exFinishCallback2);
							});
						}
						anim3.ExPlayAnimation(exPguiStatus, exFinishCallback);
					});
				}
			}
		}

		public static readonly string DojoText = "挑戦可能！";

		public GameObject go;

		public PguiTextCtrl Txt_Campaign;

		public PguiTextCtrl Txt_CampaignTime;

		private int count;

		private bool start;
	}

	public enum SkipType
	{
		Disable,
		EnableNoLimit,
		EnableDailyLimit,
		EnableWeeklyLimit,
		EnableTotalLimit
	}

	public class UsrQuestSkipInfo
	{
		public bool isSkippable;

		public bool hasSkipLimit;

		public bool isSkipByGroup;

		public string prefixStr = "";

		public string popupMessage = "";

		public int restSkipCount;

		public int restSkipRecoveryCount;

		public DataManagerMonthlyPack.PurchaseMonthlypackData monthlyPackData;
	}

	public class QuestRuleInfo
	{
		public QuestRuleInfo(PguiButtonCtrl btn)
		{
			this.buttonRuleInfo = btn;
		}

		public void Setup(int ruleId)
		{
			this.ruleInfo = string.Empty;
			List<QuestStaticRule> list = DataManager.DmQuest.QuestStaticData.ruleDataList.FindAll((QuestStaticRule item) => item.ruleId == ruleId);
			QuestStaticRule questStaticRule = null;
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				QuestStaticRule questStaticRule2 = list[i];
				string text = string.Empty;
				if (!flag || (flag && questStaticRule2.ruleType != QuestStaticRule.RuleType.SPECIFIED_CHARA))
				{
					if (questStaticRule == null || (questStaticRule != null && questStaticRule.detail != questStaticRule2.detail))
					{
						this.ruleInfo += "助っ人を含め\n";
					}
					if (questStaticRule != null && !this.ruleInfo.IsNullOrEmpty())
					{
						this.ruleInfo += "・";
					}
					questStaticRule = questStaticRule2;
					if (questStaticRule2.ruleType == QuestStaticRule.RuleType.SPECIFIED_CHARA)
					{
						flag = true;
					}
					DescriptionAttribute customAttribute = questStaticRule2.ruleType.GetType().GetField(questStaticRule2.ruleType.ToString()).GetCustomAttribute<DescriptionAttribute>();
					if (questStaticRule2.ruleType == QuestStaticRule.RuleType.SPECIFIED_ATTRIBUTE)
					{
						DescriptionAttribute customAttribute2 = questStaticRule2.attribute.GetType().GetField(questStaticRule2.attribute.ToString()).GetCustomAttribute<DescriptionAttribute>();
						text = customAttribute.Description.Replace("{Replace}", customAttribute2.Description);
					}
					else
					{
						text = customAttribute.Description;
					}
				}
				DescriptionAttribute customAttribute3 = questStaticRule2.detail.GetType().GetField(questStaticRule2.detail.ToString()).GetCustomAttribute<DescriptionAttribute>();
				bool flag2 = i < list.Count - 1;
				bool flag3 = false;
				if (!flag2 || (flag2 && questStaticRule2.detail != list[i + 1].detail))
				{
					flag3 = true;
					text = text + "のフレンズが\n" + customAttribute3.Description + "\n\u3000";
					questStaticRule = null;
					flag = false;
				}
				if (flag2 && flag3)
				{
					text += "\n";
				}
				this.ruleInfo += text;
			}
			this.buttonRuleInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClick), PguiButtonCtrl.SoundType.DEFAULT);
		}

		private void OnClick(PguiButtonCtrl button)
		{
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("編成条件確認"), PrjUtil.MakeMessage(this.ruleInfo), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
		}

		public PguiButtonCtrl buttonRuleInfo;

		private string ruleInfo;
	}
}
