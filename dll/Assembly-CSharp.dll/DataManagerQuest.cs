using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class DataManagerQuest
{
	// Token: 0x060006C5 RID: 1733 RVA: 0x0002DCD6 File Offset: 0x0002BED6
	public DataManagerQuest(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x0002DCFB File Offset: 0x0002BEFB
	public int GetContinueStoneNum()
	{
		return DataManager.DmServerMst.MstAppConfig.continueStone;
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0002DD0C File Offset: 0x0002BF0C
	// (set) Token: 0x060006C8 RID: 1736 RVA: 0x0002DD14 File Offset: 0x0002BF14
	public QuestStaticData QuestStaticData { get; private set; }

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x060006C9 RID: 1737 RVA: 0x0002DD1D File Offset: 0x0002BF1D
	// (set) Token: 0x060006CA RID: 1738 RVA: 0x0002DD25 File Offset: 0x0002BF25
	public QuestDynamicData QuestDynamicData { get; private set; }

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x060006CB RID: 1739 RVA: 0x0002DD2E File Offset: 0x0002BF2E
	// (set) Token: 0x060006CC RID: 1740 RVA: 0x0002DD36 File Offset: 0x0002BF36
	public DataManagerQuest.QuestStartData LastQuestStartResponse { get; private set; }

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x060006CD RID: 1741 RVA: 0x0002DD3F File Offset: 0x0002BF3F
	// (set) Token: 0x060006CE RID: 1742 RVA: 0x0002DD47 File Offset: 0x0002BF47
	public DataManagerQuest.QuestEndData LastQuestEndResponse { get; private set; }

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x060006CF RID: 1743 RVA: 0x0002DD50 File Offset: 0x0002BF50
	// (set) Token: 0x060006D0 RID: 1744 RVA: 0x0002DD58 File Offset: 0x0002BF58
	public DataManagerQuest.QuestRestartData LastQuestRestartResponse { get; private set; }

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0002DD61 File Offset: 0x0002BF61
	// (set) Token: 0x060006D2 RID: 1746 RVA: 0x0002DD69 File Offset: 0x0002BF69
	public int QuestTopCharaId { get; private set; }

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0002DD72 File Offset: 0x0002BF72
	// (set) Token: 0x060006D4 RID: 1748 RVA: 0x0002DD7A File Offset: 0x0002BF7A
	public Dictionary<int, List<DataManagerQuest.WaveTextInfoData>> WaveTextInfoDataListMap { get; set; }

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0002DD83 File Offset: 0x0002BF83
	// (set) Token: 0x060006D6 RID: 1750 RVA: 0x0002DD8B File Offset: 0x0002BF8B
	public List<MstQuestDrawItemData> MstDrawItemDataList { get; set; }

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0002DD94 File Offset: 0x0002BF94
	// (set) Token: 0x060006D8 RID: 1752 RVA: 0x0002DD9C File Offset: 0x0002BF9C
	public List<DataManagerQuest.QuestSealedCharaData> QuestSealedCharaDatas { get; set; }

	// Token: 0x060006D9 RID: 1753 RVA: 0x0002DDA5 File Offset: 0x0002BFA5
	public bool IsFirstAccessEventByChapterId(int chapterId)
	{
		return this.finishEventChapterId.Contains(chapterId);
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x0002DDB3 File Offset: 0x0002BFB3
	private bool IsCoopQuestGroup(QuestStaticQuestGroup.GroupCategory category)
	{
		return QuestStaticQuestGroup.GroupCategory.CoopNormal == category || QuestStaticQuestGroup.GroupCategory.CoopDifficult == category || QuestStaticQuestGroup.GroupCategory.CoopBonus == category;
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0002DDC8 File Offset: 0x0002BFC8
	public BattleMissionPack GetBattleMissionPack(int questOneId)
	{
		BattleMissionPack battleMissionPack = this.battleMissionPackList.Find((BattleMissionPack x) => x.quest_one_id == questOneId);
		if (this.QuestDynamicData.oneDataMap.ContainsKey(questOneId))
		{
			battleMissionPack.clearFlag = this.QuestDynamicData.oneDataMap[questOneId].evalList.ConvertAll<bool>((int item) => item != 0);
		}
		else
		{
			battleMissionPack.clearFlag = new List<bool> { false, false, false };
		}
		return battleMissionPack;
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0002DE7C File Offset: 0x0002C07C
	public QuestStaticChapter GetChapterByNewestStory(QuestStaticChapter.Category category)
	{
		List<int> playableMapIdList = this.GetPlayableMapIdList(category);
		int num = 0;
		foreach (int num2 in playableMapIdList)
		{
			QuestStaticMap map = this.QuestStaticData.mapDataMap.TryGetValueEx(num2, null);
			if (map != null && map.chapterId > num && this.QuestStaticData.chapterDataList.Find((QuestStaticChapter x) => x.chapterId == map.chapterId).hardChapterId != 0)
			{
				num = map.chapterId;
			}
		}
		if (num == 0)
		{
			List<QuestStaticChapter> list = this.QuestStaticData.chapterDataList.FindAll((QuestStaticChapter x) => x.category == category);
			if (list.Count > 0)
			{
				list.Sort((QuestStaticChapter a, QuestStaticChapter b) => a.chapterId - b.chapterId);
				num = list[0].chapterId;
			}
		}
		return this.QuestStaticData.chapterDataMap.TryGetValueEx(num, null);
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x0002DFB8 File Offset: 0x0002C1B8
	public int CalcRestPlayNumByQuestOneId(int questOneId)
	{
		QuestStaticQuestOne questStaticQuestOne = this.QuestStaticData.oneDataMap.TryGetValueEx(questOneId, null);
		QuestDynamicQuestOne questDynamicQuestOne = this.QuestDynamicData.oneDataMap.TryGetValueEx(questOneId, null);
		if (questStaticQuestOne == null || questStaticQuestOne.limitClearNum <= 0)
		{
			return -1;
		}
		int num = questStaticQuestOne.limitClearNum - ((questDynamicQuestOne != null) ? questDynamicQuestOne.todayClearNum : 0);
		if (num >= 0)
		{
			return num;
		}
		return 0;
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x0002E014 File Offset: 0x0002C214
	public int CalcRestPlayNumByQuestGroupId(int questGroupId)
	{
		QuestStaticQuestGroup questStaticQuestGroup = this.QuestStaticData.groupDataMap.TryGetValueEx(questGroupId, null);
		if (questStaticQuestGroup == null || questStaticQuestGroup.limitClearNum <= 0)
		{
			return -1;
		}
		int num = questStaticQuestGroup.limitClearNum;
		foreach (QuestStaticQuestOne questStaticQuestOne in questStaticQuestGroup.questOneList)
		{
			if (this.QuestDynamicData.oneDataMap.ContainsKey(questStaticQuestOne.questId))
			{
				num -= this.QuestDynamicData.oneDataMap[questStaticQuestOne.questId].todayClearNum;
			}
		}
		if (num >= 0)
		{
			return num;
		}
		return 0;
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x0002E0C8 File Offset: 0x0002C2C8
	public void CalcRestSkipNumByQuestOneId(QuestOnePackData questOnePackData, ref int skipCount, ref int skipRecoveryCount)
	{
		if (questOnePackData.questOne.skippableFlag < QuestUtil.SkipType.EnableDailyLimit)
		{
			skipCount = -1;
			skipRecoveryCount = -1;
			return;
		}
		skipCount = questOnePackData.questOne.limitSkipNum + questOnePackData.questDynamicOne.skipRecoveryCount - questOnePackData.questDynamicOne.skipCount;
		skipCount = ((skipCount < 0) ? 0 : skipCount);
		skipRecoveryCount = questOnePackData.questOne.limitSkipRecoveryNum - questOnePackData.questDynamicOne.skipRecoveryCount;
		skipRecoveryCount = ((skipRecoveryCount < 0) ? 0 : skipRecoveryCount);
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x0002E140 File Offset: 0x0002C340
	public void CalcRestSkipNumByQuestGroupId(QuestOnePackData questOnePackData, ref int skipCount, ref int skipRecoveryCount)
	{
		if (questOnePackData.questGroup.SkippableFlag < QuestUtil.SkipType.EnableDailyLimit)
		{
			skipCount = -1;
			skipRecoveryCount = -1;
			return;
		}
		int groupSkipSum = 0;
		int groupSkipRecoverySum = 0;
		this.QuestStaticData.groupDataMap[questOnePackData.questOne.questGroupId].questOneList.ForEach(delegate(QuestStaticQuestOne x)
		{
			if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(x.questId))
			{
				groupSkipSum += DataManager.DmQuest.QuestDynamicData.oneDataMap[x.questId].skipCount;
				groupSkipRecoverySum += DataManager.DmQuest.QuestDynamicData.oneDataMap[x.questId].skipRecoveryCount;
			}
		});
		skipCount = questOnePackData.questGroup.LimitSkipNum + groupSkipRecoverySum - groupSkipSum;
		skipCount = ((skipCount < 0) ? 0 : skipCount);
		skipRecoveryCount = questOnePackData.questGroup.LimitSkipRecoveryNum - groupSkipRecoverySum;
		skipRecoveryCount = ((skipRecoveryCount < 0) ? 0 : skipRecoveryCount);
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x0002E1F0 File Offset: 0x0002C3F0
	public List<int> GetPlayableQuestIdList(bool excludeNoClear = false)
	{
		List<int> list = new List<int>();
		foreach (QuestDynamicQuestOne questDynamicQuestOne in this.QuestDynamicData.oneDataList)
		{
			if (!excludeNoClear || questDynamicQuestOne.clearNum > 0)
			{
				list.Add(questDynamicQuestOne.questOneId);
			}
		}
		list.Sort((int a, int b) => a - b);
		return list;
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x0002E288 File Offset: 0x0002C488
	public List<int> GetPlayableMapIdList(int chapterId = -1)
	{
		List<int> list = new List<int>();
		foreach (int num in this.GetPlayableQuestIdList(false))
		{
			int mapId = this.QuestStaticData.groupDataMap[this.QuestStaticData.oneDataMap[num].questGroupId].mapId;
			if ((chapterId == -1 || this.QuestStaticData.mapDataMap[mapId].chapterId == chapterId) && !list.Contains(mapId))
			{
				list.Add(mapId);
			}
		}
		list.Sort((int a, int b) => a - b);
		return list;
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x0002E35C File Offset: 0x0002C55C
	public List<int> GetPlayableMapIdList(QuestStaticChapter.Category category)
	{
		List<int> list = new List<int>();
		foreach (int num in this.GetPlayableQuestIdList(false))
		{
			QuestOnePackData questOnePackData = this.GetQuestOnePackData(num);
			if (questOnePackData.questChapter.category == category && !list.Contains(questOnePackData.questMap.mapId))
			{
				list.Add(questOnePackData.questMap.mapId);
			}
		}
		list.Sort((int a, int b) => a - b);
		return list;
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x0002E410 File Offset: 0x0002C610
	public List<int> GetPlayableChapterIdList(QuestStaticChapter.Category category)
	{
		List<int> list = new List<int>();
		foreach (int num in this.GetPlayableQuestIdList(false))
		{
			QuestOnePackData questOnePackData = this.GetQuestOnePackData(num);
			if (questOnePackData.questChapter.category == category && !list.Contains(questOnePackData.questChapter.chapterId))
			{
				list.Add(questOnePackData.questChapter.chapterId);
			}
		}
		return list;
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0002E4A0 File Offset: 0x0002C6A0
	public List<int> GetAllClearGroupIdList()
	{
		List<int> list = new List<int>();
		foreach (QuestStaticQuestGroup questStaticQuestGroup in this.QuestStaticData.groupDataList)
		{
			bool flag = true;
			using (List<QuestStaticQuestOne>.Enumerator enumerator2 = questStaticQuestGroup.questOneList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					QuestStaticQuestOne questOne = enumerator2.Current;
					if (this.QuestDynamicData.oneDataList.Find((QuestDynamicQuestOne item) => item.clearNum > 0 && item.questOneId == questOne.questId) == null)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				list.Add(questStaticQuestGroup.questGroupId);
			}
		}
		return list;
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0002E578 File Offset: 0x0002C778
	public List<int> GetAllClearMapIdList()
	{
		List<int> list = new List<int>();
		foreach (QuestStaticMap questStaticMap in this.QuestStaticData.mapDataList)
		{
			bool flag = true;
			foreach (QuestStaticQuestGroup questStaticQuestGroup in questStaticMap.questGroupList)
			{
				using (List<QuestStaticQuestOne>.Enumerator enumerator3 = questStaticQuestGroup.questOneList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						QuestStaticQuestOne questOne = enumerator3.Current;
						if (this.QuestDynamicData.oneDataList.Find((QuestDynamicQuestOne item) => item.clearNum > 0 && item.questOneId == questOne.questId) == null)
						{
							flag = false;
							break;
						}
					}
				}
			}
			if (flag)
			{
				list.Add(questStaticMap.mapId);
			}
		}
		return list;
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x0002E690 File Offset: 0x0002C890
	public QuestOnePackData GetQuestOnePackData(int questOneId)
	{
		if (questOneId == 0)
		{
			return null;
		}
		QuestOnePackData questOnePackData = null;
		if (this.QuestStaticData.oneDataMap.ContainsKey(questOneId))
		{
			questOnePackData = new QuestOnePackData();
			this.QuestStaticData.oneDataMap.ContainsKey(questOneId);
			questOnePackData.questOne = this.QuestStaticData.oneDataMap[questOneId];
			this.QuestStaticData.groupDataMap.ContainsKey(questOnePackData.questOne.questGroupId);
			questOnePackData.questGroup = this.QuestStaticData.groupDataMap[questOnePackData.questOne.questGroupId];
			this.QuestStaticData.mapDataMap.ContainsKey(questOnePackData.questGroup.mapId);
			questOnePackData.questMap = this.QuestStaticData.mapDataMap[questOnePackData.questGroup.mapId];
			this.QuestStaticData.chapterDataMap.ContainsKey(questOnePackData.questMap.chapterId);
			questOnePackData.questChapter = this.QuestStaticData.chapterDataMap[questOnePackData.questMap.chapterId];
			if (this.QuestDynamicData.oneDataMap.ContainsKey(questOneId))
			{
				questOnePackData.questDynamicOne = this.QuestDynamicData.oneDataMap[questOneId];
			}
			else
			{
				questOnePackData.questDynamicOne = new QuestDynamicQuestOne
				{
					questOneId = questOneId,
					evalList = new List<int> { 0, 0, 0 }
				};
			}
		}
		return questOnePackData;
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x0002E7FF File Offset: 0x0002C9FF
	public List<DataManagerQuest.WaveTextInfoData> GetWaveTextInfoDataList(int infoId)
	{
		if (this.WaveTextInfoDataListMap == null)
		{
			return new List<DataManagerQuest.WaveTextInfoData>();
		}
		if (!this.WaveTextInfoDataListMap.ContainsKey(infoId))
		{
			return new List<DataManagerQuest.WaveTextInfoData>();
		}
		return this.WaveTextInfoDataListMap[infoId];
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0002E82F File Offset: 0x0002CA2F
	public void RequestGetUserQuestInfo()
	{
		this.parentData.ServerRequest(QuestCmd.Create(0), new Action<Command>(this.CbQuestCmd));
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x0002E84E File Offset: 0x0002CA4E
	public void RequestGetSealedCharaDatas()
	{
		this.parentData.ServerRequest(QuestSealedCharaCmd.Create(), new Action<Command>(this.CbSealedCharaCmd));
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x0002E86C File Offset: 0x0002CA6C
	public void RequestActionBattleStart(int questOneId, int deckId, int helperFriendId, int helperFriendCharaId, List<long> helperFriendPhotoIdList, long raidLastUpdatePoint)
	{
		if (!this.QuestStaticData.oneDataMap.ContainsKey(questOneId))
		{
			return;
		}
		QuestStaticQuestOne questStaticQuestOne = this.QuestStaticData.oneDataMap[questOneId];
		if (!this.QuestStaticData.groupDataMap.ContainsKey(questStaticQuestOne.questGroupId))
		{
			return;
		}
		QuestStaticQuestGroup.GroupCategory questGroupCategory = this.QuestStaticData.groupDataMap[questStaticQuestOne.questGroupId].QuestGroupCategory;
		if (this.IsCoopQuestGroup(questGroupCategory))
		{
			this.RequestCoopBattleStart(questOneId, deckId, helperFriendId, helperFriendCharaId, helperFriendPhotoIdList, raidLastUpdatePoint);
			return;
		}
		bool flag = questStaticQuestOne.QuestCategory == QuestStaticQuestOne.QuestOneCategory.NoDhole;
		this.RequestActionNormalBattleStart(questOneId, deckId, helperFriendId, helperFriendCharaId, helperFriendPhotoIdList, flag);
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x0002E908 File Offset: 0x0002CB08
	private void RequestActionNormalBattleStart(int questOneId, int deckId, int helperFriendId, int helperFriendCharaId, List<long> helperFriendPhotoIdList, bool withoutDhole)
	{
		this.LastQuestStartResponse = null;
		int num = DataManager.DmDeck.GetUserDeckById(deckId).CalcDeckKemoStatusWithPhoto(withoutDhole, questOneId);
		this.parentData.ServerRequest(QuestStartCmd.Create(questOneId, deckId, helperFriendId, helperFriendCharaId, num, helperFriendPhotoIdList), new Action<Command>(this.CbQuestStartCmd));
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x0002E954 File Offset: 0x0002CB54
	public void RequestActionBattleEnd(int questOneId, DataManagerQuest.BattleEndStatus endStatus, List<bool> battleMissionResult, int finishTurnNum, int arrivalWave, int continueCnt, int okawariCnt, int maxChainNum, int chainCnt, int chainSumCnt, int maxDamage, int artsCnt, int playerSkillCnt, int tickleSuccessCnt, int raidTotalDamage, int bonusDefeatedCount, List<int> banned_chara_list)
	{
		if (!this.QuestStaticData.oneDataMap.ContainsKey(questOneId))
		{
			return;
		}
		int questGroupId = this.QuestStaticData.oneDataMap[questOneId].questGroupId;
		if (!this.QuestStaticData.groupDataMap.ContainsKey(questGroupId))
		{
			return;
		}
		QuestStaticQuestGroup.GroupCategory questGroupCategory = this.QuestStaticData.groupDataMap[questGroupId].QuestGroupCategory;
		if (this.IsCoopQuestGroup(questGroupCategory))
		{
			this.RequestCoopBattleEnd(questOneId, endStatus, battleMissionResult, finishTurnNum, arrivalWave, continueCnt, okawariCnt, maxChainNum, chainCnt, chainSumCnt, maxDamage, artsCnt, playerSkillCnt, tickleSuccessCnt, raidTotalDamage, bonusDefeatedCount);
			return;
		}
		this.RequestActionNormalBattleEnd(questOneId, endStatus, battleMissionResult, finishTurnNum, arrivalWave, continueCnt, okawariCnt, maxChainNum, chainCnt, chainSumCnt, maxDamage, artsCnt, playerSkillCnt, tickleSuccessCnt, banned_chara_list);
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x0002EA08 File Offset: 0x0002CC08
	private void RequestActionNormalBattleEnd(int questOneId, DataManagerQuest.BattleEndStatus endStatus, List<bool> battleMissionResult, int finishTurnNum, int arrivalWave, int continueCnt, int okawariCnt, int maxChainNum, int chainCnt, int chainSumCnt, int maxDamage, int artsCnt, int playerSkillCnt, int tickleSuccessCnt, List<int> banned_chara_list)
	{
		this.LastQuestEndResponse = null;
		int num = 0;
		for (int i = 0; i < battleMissionResult.Count; i++)
		{
			if (battleMissionResult[i])
			{
				num += 1 << i;
			}
		}
		long num2 = ((this.LastQuestStartResponse != null) ? this.LastQuestStartResponse.hash_id : 0L);
		this.parentData.ServerRequest(QuestEndCmd.Create(questOneId, (int)endStatus, num, finishTurnNum, num2, arrivalWave, continueCnt, okawariCnt, maxChainNum, chainCnt, chainSumCnt, maxDamage, artsCnt, playerSkillCnt, tickleSuccessCnt, banned_chara_list), new Action<Command>(this.CbQuestEndCmd));
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x0002EA94 File Offset: 0x0002CC94
	public void RequestActionBattleContinue(int questOneId, int arrivalWave, int continueCnt, int okawariCnt, int maxChainNum, int chainCnt, int chainSumCnt, int maxDamage, int artsCnt, int playerSkillCnt, int tickleSuccessCnt)
	{
		long num = ((this.LastQuestStartResponse != null) ? this.LastQuestStartResponse.hash_id : 0L);
		this.parentData.ServerRequest(QuestEndCmd.Create(questOneId, 4, 0, 0, num, arrivalWave, continueCnt, okawariCnt, maxChainNum, chainCnt, chainSumCnt, maxDamage, artsCnt, playerSkillCnt, tickleSuccessCnt, null), new Action<Command>(this.CbQuestEndCmd));
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0002EAF0 File Offset: 0x0002CCF0
	public void RequestActionStoryOnlyQuestStart(int questOneId)
	{
		this.LastQuestStartResponse = null;
		int num = DataManager.DmDeck.GetUserDeckById(1).CalcDeckKemoStatusWithPhoto(false, 0);
		this.parentData.ServerRequest(QuestStartCmd.Create(questOneId, 1, 0, 0, num, new List<long>()), new Action<Command>(this.CbQuestStartCmd));
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x0002EB40 File Offset: 0x0002CD40
	public void RequestActionStoryOnlyQuestEnd(int questOneId)
	{
		long num = ((this.LastQuestStartResponse != null) ? this.LastQuestStartResponse.hash_id : 0L);
		this.parentData.ServerRequest(QuestEndCmd.Create(questOneId, 1, 7, 0, num, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null), new Action<Command>(this.CbQuestEndCmd));
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x0002EB94 File Offset: 0x0002CD94
	public void RequestActionUpdateFinishChapterEvent(int chapterId)
	{
		List<NewFlg> list = new List<NewFlg>
		{
			new NewFlg
			{
				category = 2,
				any_id = chapterId,
				new_mgmt_flg = 1
			}
		};
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(list), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0002EBE4 File Offset: 0x0002CDE4
	public void RequestActionBattleRestartCheck(long hash_id)
	{
		this.LastQuestRestartResponse = null;
		this.parentData.ServerRequest(QuestReStartCheckCmd.Create(hash_id), new Action<Command>(this.CbQuestRestartCheckCmd));
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x0002EC0A File Offset: 0x0002CE0A
	public void RequestActionBattleRestart(int restart_type)
	{
		this.parentData.ServerRequest(QuestReStartCmd.Create(restart_type), null);
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0002EC1E File Offset: 0x0002CE1E
	public void RequestStorySkip(int questId, int storyType)
	{
		this.parentData.ServerRequest(StorySkipCmd.Create(questId, storyType), null);
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x0002EC33 File Offset: 0x0002CE33
	public void RequestActionQuestLimitRecoveryByQuestOne(int questOneId)
	{
		this.parentData.ServerRequest(QuestLimitRecoveryCmd.Create(questOneId, false), new Action<Command>(this.CbQuestLimitRecovery));
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0002EC53 File Offset: 0x0002CE53
	public void RequestActionQuestLimitRecoveryByQuestOne(int questOneId, bool isRaid)
	{
		this.parentData.ServerRequest(QuestLimitRecoveryCmd.Create(questOneId, isRaid), new Action<Command>(this.CbQuestLimitRecovery));
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x0002EC74 File Offset: 0x0002CE74
	public void RequestActionQuestLimitRecoveryByQuestGroup(int questGroupId)
	{
		int questId = DataManager.DmQuest.QuestStaticData.groupDataMap[questGroupId].questOneList[0].questId;
		this.parentData.ServerRequest(QuestLimitRecoveryCmd.Create(questId, false), new Action<Command>(this.CbQuestLimitRecovery));
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0002ECC5 File Offset: 0x0002CEC5
	public void RequestActionRecoverySkip(int questOneId, int useItemId, int skipRecoveryNum)
	{
		this.parentData.ServerRequest(QuestSkipRecoveryCmd.Create(questOneId, useItemId, skipRecoveryNum), new Action<Command>(this.CbQuestSkipRecovery));
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x0002ECE8 File Offset: 0x0002CEE8
	private void RequestCoopBattleStart(int questOneId, int deckId, int helperFriendId, int helperFriendCharaId, List<long> helperFriendPhotoIdList, long raidLastUpdatePoint)
	{
		this.LastQuestStartResponse = null;
		int num = DataManager.DmDeck.GetUserDeckById(deckId).CalcDeckKemoStatusWithPhoto(false, 0);
		this.parentData.ServerRequest(CoopStartCmd.Create(questOneId, deckId, helperFriendId, helperFriendCharaId, num, helperFriendPhotoIdList, DataManager.DmEvent.LastCoopInfo.InfoGetTime, DataManager.DmEvent.LastCoopInfo.EventId, raidLastUpdatePoint), new Action<Command>(this.CbCoopStartCmd));
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x0002ED54 File Offset: 0x0002CF54
	private void RequestCoopBattleEnd(int questOneId, DataManagerQuest.BattleEndStatus endStatus, List<bool> battleMissionResult, int finishTurnNum, int arrivalWave, int continueCnt, int okawariCnt, int maxChainNum, int chainCnt, int chainSumCnt, int maxDamage, int artsCnt, int playerSkillCnt, int tickleSuccessCnt, int raidTotalDamage, int bonusDefeatedCount)
	{
		this.LastQuestEndResponse = null;
		int num = 0;
		for (int i = 0; i < battleMissionResult.Count; i++)
		{
			if (battleMissionResult[i])
			{
				num += 1 << i;
			}
		}
		long num2 = ((this.LastQuestStartResponse != null) ? this.LastQuestStartResponse.hash_id : 0L);
		this.parentData.ServerRequest(CoopEndCmd.Create(questOneId, (int)endStatus, num, finishTurnNum, num2, arrivalWave, continueCnt, okawariCnt, maxChainNum, chainCnt, chainSumCnt, maxDamage, artsCnt, playerSkillCnt, tickleSuccessCnt, raidTotalDamage, bonusDefeatedCount), new Action<Command>(this.CbCoopEndCmd));
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x0002EDE0 File Offset: 0x0002CFE0
	public void RequestOpenQuestByItem(int mapId)
	{
		int questId = this.QuestStaticData.mapDataMap[mapId].largeEventUIData.openItemInfoOneData.questId;
		this.parentData.ServerRequest(QuestOpenCmd.Create(questId), new Action<Command>(this.CbQuestOpen));
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0002EE2C File Offset: 0x0002D02C
	private void RequestActionUpdateLastPlayQuestByOne(int questOneId)
	{
		QuestOnePackData pack = this.GetQuestOnePackData(questOneId);
		if (pack != null)
		{
			int num = -1;
			switch (pack.questChapter.category)
			{
			case QuestStaticChapter.Category.STORY:
				num = 27;
				break;
			case QuestStaticChapter.Category.GROW:
				num = 28;
				break;
			case QuestStaticChapter.Category.CHARA:
				num = 29;
				break;
			case QuestStaticChapter.Category.PVP:
				num = 30;
				break;
			case QuestStaticChapter.Category.EVENT:
			{
				DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventDataList().Find((DataManagerEvent.EventData x) => x.eventChapterId == pack.questChapter.chapterId);
				if (eventData != null && DataManagerEvent.Category.Large == eventData.eventCategory)
				{
					num = 31;
				}
				break;
			}
			case QuestStaticChapter.Category.SIDE_STORY:
				num = 32;
				break;
			case QuestStaticChapter.Category.CELLVAL:
				num = 47;
				break;
			case QuestStaticChapter.Category.ETCETERA:
				num = 52;
				break;
			case QuestStaticChapter.Category.STORY2:
				num = 55;
				break;
			case QuestStaticChapter.Category.STORY3:
				num = 57;
				break;
			}
			if (num != -1)
			{
				List<int> list = DataManager.DmUserInfo.optionData.CreateByServerData();
				list[num] = questOneId;
				if (SceneQuest.IsMainStory(pack.questChapter.category))
				{
					list[49] = questOneId;
				}
				this.parentData.ServerRequest(OptionSetCmd.Create(list), new Action<Command>(this.CbOptionSetCmd));
			}
		}
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0002EF60 File Offset: 0x0002D160
	public void RequestActionUpdateLastPlayQuestByMap(int questMapId)
	{
		QuestStaticMap questStaticMap = this.QuestStaticData.mapDataMap.TryGetValueEx(questMapId, null);
		if (questStaticMap == null)
		{
			return;
		}
		if (questStaticMap.questGroupList.Count <= 0)
		{
			return;
		}
		if (questStaticMap.questGroupList[0].questOneList.Count <= 0)
		{
			return;
		}
		this.RequestActionUpdateLastPlayQuestByOne(questStaticMap.questGroupList[0].questOneList[0].questId);
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0002EFCF File Offset: 0x0002D1CF
	public void RequestActionQuestSkip(int quest_id, int deck_id, int friend_id, int helper_chara_id, int skip_num, int kemostatus, List<long> helper_photo_id_List)
	{
		this.parentData.ServerRequest(QuestSkipCmd.Create(quest_id, deck_id, friend_id, helper_chara_id, skip_num, kemostatus, helper_photo_id_List), new Action<Command>(this.CbQuestSkipCmd));
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0002EFF8 File Offset: 0x0002D1F8
	public void CbQuestCmd(Command cmd)
	{
		QuestResponse questResponse = cmd.response as QuestResponse;
		QuestDynamicData questDynamicData = new QuestDynamicData();
		foreach (Quest quest in questResponse.quests)
		{
			QuestDynamicQuestOne questDynamicQuestOne = new QuestDynamicQuestOne();
			questDynamicQuestOne.UpdateByServer(quest);
			questDynamicData.oneDataMap.Add(questDynamicQuestOne.questOneId, questDynamicQuestOne);
		}
		questDynamicData.oneDataList = new List<QuestDynamicQuestOne>(questDynamicData.oneDataMap.Values);
		this.QuestDynamicData = questDynamicData;
		foreach (BattleMissionPack battleMissionPack in this.battleMissionPackList)
		{
			if (questDynamicData.oneDataMap.ContainsKey(battleMissionPack.quest_one_id))
			{
				battleMissionPack.clearFlag = questDynamicData.oneDataMap[battleMissionPack.quest_one_id].evalList.ConvertAll<bool>((int item) => item != 0);
			}
			else
			{
				battleMissionPack.clearFlag = new List<bool> { false, false, false };
			}
		}
		DataManager.DmPurchase.UpdateBadgeDispLimitedTime(questResponse.quests, 0);
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x0002F164 File Offset: 0x0002D364
	public void CbSealedCharaCmd(Command cmd)
	{
		QuestSealedCharaResponse questSealedCharaResponse = cmd.response as QuestSealedCharaResponse;
		this.QuestSealedCharaDatas = new List<DataManagerQuest.QuestSealedCharaData>();
		foreach (QuestSealedDatas questSealedDatas in questSealedCharaResponse.sealedDatas)
		{
			DataManagerQuest.QuestSealedCharaData questSealedCharaData = new DataManagerQuest.QuestSealedCharaData(questSealedDatas.target, questSealedDatas.questOneId, questSealedDatas.json);
			this.QuestSealedCharaDatas.Add(questSealedCharaData);
		}
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x0002F1EC File Offset: 0x0002D3EC
	private void CbQuestStartCmd(Command cmd)
	{
		QuestStartRequest questStartRequest = cmd.request as QuestStartRequest;
		QuestStartResponse questStartResponse = cmd.response as QuestStartResponse;
		this.parentData.UpdateUserAssetByAssets(questStartResponse.assets);
		this.LastQuestStartResponse = new DataManagerQuest.QuestStartData(questStartResponse);
		this.RequestActionUpdateLastPlayQuestByOne(questStartRequest.quest_id);
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x0002F23C File Offset: 0x0002D43C
	private void CbQuestEndCmd(Command cmd)
	{
		QuestEndResponse questEndResponse = cmd.response as QuestEndResponse;
		QuestEndRequest questEndRequest = cmd.request as QuestEndRequest;
		this.parentData.UpdateUserAssetByAssets(questEndResponse.assets);
		this.LastQuestEndResponse = new DataManagerQuest.QuestEndData(questEndResponse);
		this.QuestEndUpdate(questEndResponse.quests, questEndRequest.quest_id, questEndRequest.end_type);
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x0002F298 File Offset: 0x0002D498
	private void CbQuestRestartCheckCmd(Command cmd)
	{
		QuestReStartCheckRequest questReStartCheckRequest = cmd.request as QuestReStartCheckRequest;
		QuestReStartCheckResponse questReStartCheckResponse = cmd.response as QuestReStartCheckResponse;
		this.LastQuestRestartResponse = new DataManagerQuest.QuestRestartData(questReStartCheckResponse);
		this.LastQuestStartResponse = new DataManagerQuest.QuestStartData(new QuestStartResponse
		{
			hash_id = questReStartCheckRequest.hash_id,
			drew_items = new List<DrewItem>()
		});
		DataManager.DmTraining.UpdateLastTrainingStartResponse(questReStartCheckRequest.hash_id);
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x0002F300 File Offset: 0x0002D500
	private void CbQuestLimitRecovery(Command cmd)
	{
		QuestLimitRecoveryRequest questLimitRecoveryRequest = cmd.request as QuestLimitRecoveryRequest;
		QuestLimitRecoveryResponse questLimitRecoveryResponse = cmd.response as QuestLimitRecoveryResponse;
		if (this.QuestDynamicData.oneDataMap.ContainsKey(questLimitRecoveryRequest.quest_id))
		{
			this.QuestDynamicData.oneDataMap[questLimitRecoveryRequest.quest_id].todayRecoveryNum = questLimitRecoveryResponse.today_recovery_num;
			this.QuestDynamicData.oneDataMap[questLimitRecoveryRequest.quest_id].todayClearNum = 0;
		}
		this.parentData.UpdateUserAssetByAssets(questLimitRecoveryResponse.assets);
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x0002F38C File Offset: 0x0002D58C
	private void CbQuestSkipRecovery(Command cmd)
	{
		Request request = cmd.request;
		QuestSkipRecoveryResponse questSkipRecoveryResponse = cmd.response as QuestSkipRecoveryResponse;
		foreach (Quest quest in questSkipRecoveryResponse.quests)
		{
			if (this.QuestDynamicData.oneDataMap.ContainsKey(quest.quest_id))
			{
				this.QuestDynamicData.oneDataMap[quest.quest_id].UpdateByServer(quest);
			}
			else
			{
				this.QuestDynamicData.oneDataMap[quest.quest_id] = new QuestDynamicQuestOne();
				this.QuestDynamicData.oneDataMap[quest.quest_id].UpdateByServer(quest);
				this.QuestDynamicData.oneDataList.Add(this.QuestDynamicData.oneDataMap[quest.quest_id]);
			}
		}
		this.parentData.UpdateUserAssetByAssets(questSkipRecoveryResponse.assets);
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x0002F498 File Offset: 0x0002D698
	private void CbCoopStartCmd(Command cmd)
	{
		Request request = cmd.request;
		CoopStartResponse coopStartResponse = cmd.response as CoopStartResponse;
		this.parentData.UpdateUserAssetByAssets(coopStartResponse.assets);
		this.LastQuestStartResponse = new DataManagerQuest.QuestStartData(coopStartResponse);
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x0002F4D8 File Offset: 0x0002D6D8
	private void CbCoopEndCmd(Command cmd)
	{
		CoopEndRequest coopEndRequest = cmd.request as CoopEndRequest;
		CoopEndResponse coopEndResponse = cmd.response as CoopEndResponse;
		this.parentData.UpdateUserAssetByAssets(coopEndResponse.assets);
		this.LastQuestEndResponse = new DataManagerQuest.QuestEndData(coopEndResponse);
		this.QuestEndUpdate(coopEndResponse.quests, coopEndRequest.quest_id, coopEndRequest.end_type);
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0002F534 File Offset: 0x0002D734
	private void CbQuestOpen(Command cmd)
	{
		Request request = cmd.request;
		QuestOpenResponse questOpenResponse = cmd.response as QuestOpenResponse;
		if (this.QuestDynamicData.oneDataMap.ContainsKey(questOpenResponse.oepn_quest.quest_id))
		{
			this.QuestDynamicData.oneDataMap[questOpenResponse.oepn_quest.quest_id].UpdateByServer(questOpenResponse.oepn_quest);
		}
		else
		{
			this.QuestDynamicData.oneDataMap[questOpenResponse.oepn_quest.quest_id] = new QuestDynamicQuestOne();
			this.QuestDynamicData.oneDataMap[questOpenResponse.oepn_quest.quest_id].UpdateByServer(questOpenResponse.oepn_quest);
			this.QuestDynamicData.oneDataList.Add(this.QuestDynamicData.oneDataMap[questOpenResponse.oepn_quest.quest_id]);
		}
		this.parentData.UpdateUserAssetByAssets(questOpenResponse.assets);
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x0002F61C File Offset: 0x0002D81C
	private void CbOptionSetCmd(Command cmd)
	{
		OptionSetRequest optionSetRequest = cmd.request as OptionSetRequest;
		this.parentData.UpdateUserOption(optionSetRequest.optionList);
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0002F648 File Offset: 0x0002D848
	public void CbNewFlgUpdateCmd(Command cmd)
	{
		foreach (NewFlg newFlg in (cmd.request as NewFlgUpdateRequest).new_flg_list)
		{
			if (newFlg.new_mgmt_flg == 1 && newFlg.category == 2)
			{
				this.finishEventChapterId.Add(newFlg.any_id);
			}
		}
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x0002F6C4 File Offset: 0x0002D8C4
	public void InsertNewList(List<NewFlg> newFlagList)
	{
		this.finishEventChapterId = new HashSet<int>();
		foreach (NewFlg newFlg in newFlagList)
		{
			if (newFlg.new_mgmt_flg == 1 && newFlg.category == 2)
			{
				this.finishEventChapterId.Add(newFlg.any_id);
			}
		}
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x0002F73C File Offset: 0x0002D93C
	private void CbQuestSkipCmd(Command cmd)
	{
		QuestSkipResponse questSkipResponse = cmd.response as QuestSkipResponse;
		QuestSkipRequest questSkipRequest = cmd.request as QuestSkipRequest;
		this.parentData.UpdateUserAssetByAssets(questSkipResponse.assets);
		CanvasManager.HdlQuestSkipWindowsCtrl.GoNextScene(questSkipResponse.drew_items);
		this.QuestEndUpdate(questSkipResponse.quests, questSkipRequest.quest_id, 1);
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0002F798 File Offset: 0x0002D998
	private void QuestEndUpdate(List<Quest> quests, int questId, int endType)
	{
		DataManager.DmHelper.ClearRentalHelperNextResetTime();
		foreach (Quest quest in quests)
		{
			if (this.QuestDynamicData.oneDataMap.ContainsKey(quest.quest_id))
			{
				this.QuestDynamicData.oneDataMap[quest.quest_id].UpdateByServer(quest);
			}
			else
			{
				this.QuestDynamicData.oneDataMap[quest.quest_id] = new QuestDynamicQuestOne();
				this.QuestDynamicData.oneDataMap[quest.quest_id].UpdateByServer(quest);
				this.QuestDynamicData.oneDataList.Add(this.QuestDynamicData.oneDataMap[quest.quest_id]);
			}
		}
		DataManager.DmPurchase.UpdateBadgeDispLimitedTime(quests, questId);
		bool flag = endType == 1 && this.QuestDynamicData.oneDataMap[questId].clearNum == 1;
		DataManagerServerMst.ModeReleaseData modeReleaseData = DataManager.DmServerMst.ModeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.QuestId == questId);
		if (flag && modeReleaseData != null)
		{
			PrjUtil.SendAppsFlyerLtvIdByQuestClear(modeReleaseData.Category);
		}
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0002F8F4 File Offset: 0x0002DAF4
	public IEnumerator InitializeMstData(MstManager mstManager, Dictionary<int, EnemyStaticData> enemyMap)
	{
		this.loadAssetQuestparamList = AssetManager.LoadAssetDataByCategory(AssetManager.ASSET_CATEGORY_QUESTPARAM, AssetManager.OWNER.DataManagerQuest, 0, null);
		foreach (string assetName in this.loadAssetQuestparamList)
		{
			while (!AssetManager.IsLoadFinishAssetData(AssetManager.PREFIX_PATH_QUEST_PARAM + assetName))
			{
				yield return null;
			}
			assetName = null;
		}
		List<string>.Enumerator enumerator = default(List<string>.Enumerator);
		this.InitializeMstDataInternal(mstManager, enemyMap);
		yield break;
		yield break;
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0002F914 File Offset: 0x0002DB14
	internal void SetMstDataByEvent(DataManagerEvent dmEvent)
	{
		foreach (DataManagerEvent.EventData eventData in dmEvent.GetEventDataList())
		{
			if (eventData.eventCategory == DataManagerEvent.Category.SpecialPvp)
			{
				DataManagerQuest.DrawItemData drawItemData = new DataManagerQuest.DrawItemData();
				drawItemData.DropItemList = new List<ItemInput>
				{
					new ItemInput(eventData.eventCoinIdList[0], 1)
				};
				drawItemData.EndDateTime = new DateTime(2100, 1, 1);
				this.QuestStaticData.chapterDataMap[eventData.eventChapterId].mapDataList[0].questGroupList[0].questOneList[0].AddDrawItemIdListExternal(drawItemData);
			}
		}
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0002F9E8 File Offset: 0x0002DBE8
	private void InitializeMstDataInternal(MstManager mstManager, Dictionary<int, EnemyStaticData> enemyMap)
	{
		List<MstQuestChapterData> mst = mstManager.GetMst<List<MstQuestChapterData>>(MstType.QUEST_CHAPTER_DATA);
		List<MstQuestMapData> mst2 = mstManager.GetMst<List<MstQuestMapData>>(MstType.QUEST_MAP_DATA);
		List<MstQuestQuestgroupData> mst3 = mstManager.GetMst<List<MstQuestQuestgroupData>>(MstType.QUEST_QUEST_QUESTGROUP_DATA);
		List<MstQuestQuestoneData> mst4 = mstManager.GetMst<List<MstQuestQuestoneData>>(MstType.QUEST_QUESTONE_DATA);
		List<MstQuestRuleData> mst5 = mstManager.GetMst<List<MstQuestRuleData>>(MstType.QUEST_RULE_DATA);
		List<MstQuestWaveData> mst6 = mstManager.GetMst<List<MstQuestWaveData>>(MstType.QUEST_WAVE_DATA);
		List<MstQuestTextinfoData> mst7 = mstManager.GetMst<List<MstQuestTextinfoData>>(MstType.QUEST_TEXTINFO_DATA);
		List<MstQuestEnemiesData> mst8 = mstManager.GetMst<List<MstQuestEnemiesData>>(MstType.QUEST_ENEMIES_DATA);
		List<MstQuestEnemyData> mst9 = mstManager.GetMst<List<MstQuestEnemyData>>(MstType.QUEST_ENEMY_DATA);
		List<MstQuestDrawItemData> mst10 = mstManager.GetMst<List<MstQuestDrawItemData>>(MstType.QUEST_DRAW_ITEM_DATA);
		this.MstDrawItemDataList = mst10;
		List<MstQuestEvalData> mst11 = mstManager.GetMst<List<MstQuestEvalData>>(MstType.QUEST_EVAL_DATA);
		List<MstQuestEvalSetData> mst12 = mstManager.GetMst<List<MstQuestEvalSetData>>(MstType.QUEST_EVAL_SET_DATA);
		List<MstQuestRewardgroupData> mst13 = mstManager.GetMst<List<MstQuestRewardgroupData>>(MstType.QUEST_REWARDGROUP_DATA);
		List<MstQuestCompData> mst14 = mstManager.GetMst<List<MstQuestCompData>>(MstType.QUEST_COMP_DATA);
		List<MstQuestEnemyfriendsData> mst15 = mstManager.GetMst<List<MstQuestEnemyfriendsData>>(MstType.QUEST_ENEMYFRIENDS_DATA);
		List<MstQuestQuestdropItemData> mst16 = mstManager.GetMst<List<MstQuestQuestdropItemData>>(MstType.QUEST_QUESTDROP_ITEM_DATA);
		this.QuestStaticData = new QuestStaticData();
		Dictionary<int, List<DataManagerQuest.DrawItemTermData>> dictionary = new Dictionary<int, List<DataManagerQuest.DrawItemTermData>>();
		Dictionary<int, List<DataManagerQuest.DrawItemTermData>> dictionary2 = new Dictionary<int, List<DataManagerQuest.DrawItemTermData>>();
		Dictionary<int, List<DataManagerQuest.DrawItemTermData>> dictionary3 = new Dictionary<int, List<DataManagerQuest.DrawItemTermData>>();
		Dictionary<int, MstQuestEnemiesData> dictionary4 = new Dictionary<int, MstQuestEnemiesData>();
		foreach (MstQuestEnemiesData mstQuestEnemiesData in mst8)
		{
			dictionary4.Add(mstQuestEnemiesData.enemiesId, mstQuestEnemiesData);
		}
		Dictionary<int, MstQuestEnemyData> dictionary5 = new Dictionary<int, MstQuestEnemyData>();
		Dictionary<int, List<MstQuestDrawItemData>> dictionary6 = new Dictionary<int, List<MstQuestDrawItemData>>();
		using (List<MstQuestEnemyData>.Enumerator enumerator2 = mst9.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				MstQuestEnemyData mstQuestEnemy = enumerator2.Current;
				dictionary5.Add(mstQuestEnemy.enemyId, mstQuestEnemy);
				if (!dictionary6.ContainsKey(mstQuestEnemy.drawId))
				{
					dictionary6.Add(mstQuestEnemy.drawId, mst10.FindAll((MstQuestDrawItemData i) => i.drawId == mstQuestEnemy.drawId));
				}
			}
		}
		using (List<MstQuestEnemyfriendsData>.Enumerator enumerator3 = mst15.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				MstQuestEnemyfriendsData mstQuestEnemyfriends = enumerator3.Current;
				if (!dictionary6.ContainsKey(mstQuestEnemyfriends.drawId))
				{
					dictionary6.Add(mstQuestEnemyfriends.drawId, mst10.FindAll((MstQuestDrawItemData i) => i.drawId == mstQuestEnemyfriends.drawId));
				}
			}
		}
		this.WaveTextInfoDataListMap = new Dictionary<int, List<DataManagerQuest.WaveTextInfoData>>();
		foreach (MstQuestTextinfoData mstQuestTextinfoData in mst7)
		{
			if (!this.WaveTextInfoDataListMap.ContainsKey(mstQuestTextinfoData.infoId))
			{
				this.WaveTextInfoDataListMap.Add(mstQuestTextinfoData.infoId, new List<DataManagerQuest.WaveTextInfoData>());
			}
			this.WaveTextInfoDataListMap[mstQuestTextinfoData.infoId].Add(new DataManagerQuest.WaveTextInfoData(mstQuestTextinfoData));
		}
		using (List<MstQuestQuestoneData>.Enumerator enumerator5 = mst4.GetEnumerator())
		{
			while (enumerator5.MoveNext())
			{
				MstQuestQuestoneData mstQuestoneData = enumerator5.Current;
				List<MstQuestQuestdropItemData> list = mst16.FindAll((MstQuestQuestdropItemData x) => x.targetId == mstQuestoneData.questOneId && 4 == x.category);
				QuestStaticQuestOne questStaticQuestOne = new QuestStaticQuestOne(mstQuestoneData, mst13, DataManagerQuest.<InitializeMstDataInternal>g__CreateDrawItemList|99_0(mst10, list), DataManager.DmPhoto.PhotoQuestDropItemList);
				questStaticQuestOne.QuestDropDrawDataList = new HashSet<MstQuestQuestdropItemData>(list);
				if (0 < questStaticQuestOne.DrawItemIdList.Count)
				{
					if (!dictionary.ContainsKey(questStaticQuestOne.questGroupId))
					{
						dictionary.Add(questStaticQuestOne.questGroupId, new List<DataManagerQuest.DrawItemTermData>());
					}
					List<DataManagerQuest.DrawItemTermData> list2 = new List<DataManagerQuest.DrawItemTermData>();
					foreach (DataManagerQuest.DrawItemData drawItemData in questStaticQuestOne.DrawItemIdList)
					{
						list2.Add(new DataManagerQuest.DrawItemTermData
						{
							Id = questStaticQuestOne.questGroupId,
							StartDateTime = drawItemData.StartDateTime,
							EndDateTime = drawItemData.EndDateTime
						});
					}
					questStaticQuestOne.drawItemTermDataList = DataManagerQuest.<InitializeMstDataInternal>g__DistinctDrawItemTermData|99_1(list2);
					dictionary[questStaticQuestOne.questGroupId].AddRange(questStaticQuestOne.drawItemTermDataList);
				}
				else
				{
					questStaticQuestOne.drawItemTermDataList = new List<DataManagerQuest.DrawItemTermData>();
				}
				questStaticQuestOne.waveData = ScriptableObject.CreateInstance(typeof(QuestStaticWave)) as QuestStaticWave;
				List<MstQuestWaveData> list3 = mst6.FindAll((MstQuestWaveData item) => item.questOneId == mstQuestoneData.questOneId);
				if (1 == list3.Count && list3[0].vsFriendsSetId != 0)
				{
					MstQuestWaveData mstQuestWaveData = list3[0];
					QuestStaticWave.WaveStatic waveStatic = new QuestStaticWave.WaveStatic
					{
						id = mstQuestWaveData.waveId,
						InfoId = mstQuestWaveData.textinfoId,
						enemiesId = mstQuestWaveData.vsFriendsSetId,
						authName = mstQuestWaveData.bossAuth,
						bgmName = mstQuestWaveData.waveStartBgmId,
						enemyList = new List<QuestStaticWave.EnemyData>(),
						victoryBgmName = mstQuestWaveData.victoryBgmId
					};
					MstQuestEnemiesData mstQuestEnemiesData2 = (dictionary4.ContainsKey(mstQuestWaveData.vsFriendsSetId) ? dictionary4[mstQuestWaveData.vsFriendsSetId] : null);
					List<int> enemyFriendsIdList = new List<int>();
					new List<int>();
					if (mstQuestEnemiesData2 != null)
					{
						enemyFriendsIdList = new List<int> { mstQuestEnemiesData2.enemyId00, mstQuestEnemiesData2.enemyId01, mstQuestEnemiesData2.enemyId02, mstQuestEnemiesData2.enemyId03, mstQuestEnemiesData2.enemyId04 };
						List<int> list4 = new List<int>();
						list4.Add(mstQuestEnemiesData2.escapeEnemyHpratio00);
						list4.Add(mstQuestEnemiesData2.escapeEnemyHpratio01);
						list4.Add(mstQuestEnemiesData2.escapeEnemyHpratio02);
						list4.Add(mstQuestEnemiesData2.escapeEnemyHpratio03);
						list4.Add(mstQuestEnemiesData2.escapeEnemyHpratio04);
					}
					List<ScenarioParty.Friends> list5 = new List<ScenarioParty.Friends>();
					int num;
					int i;
					Predicate<MstQuestEnemyfriendsData> <>9__9;
					for (i = 0; i < enemyFriendsIdList.Count; i = num)
					{
						List<MstQuestEnemyfriendsData> list6 = mst15;
						Predicate<MstQuestEnemyfriendsData> predicate;
						if ((predicate = <>9__9) == null)
						{
							predicate = (<>9__9 = (MstQuestEnemyfriendsData x) => enemyFriendsIdList[i] == x.enemyId);
						}
						MstQuestEnemyfriendsData mstQuestEnemyfriendsData = list6.Find(predicate);
						if (mstQuestEnemyfriendsData != null)
						{
							ScenarioParty.Friends friends = new ScenarioParty.Friends();
							friends.id = mstQuestEnemyfriendsData.charaId;
							friends.clothItem = mstQuestEnemyfriendsData.clothId;
							CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(mstQuestEnemyfriendsData.charaId);
							if (charaStaticData != null)
							{
								questStaticQuestOne.ennemyAttrMask |= charaStaticData.baseData.attributeMask;
							}
							if (dictionary6.ContainsKey(mstQuestEnemyfriendsData.drawId))
							{
								foreach (MstQuestDrawItemData mstQuestDrawItemData in dictionary6[mstQuestEnemyfriendsData.drawId])
								{
									if (mstQuestDrawItemData.itemId != 0 && -1 != mstQuestDrawItemData.itemId)
									{
										questStaticQuestOne.EnemyDropDrawDataList.Add(mstQuestDrawItemData);
										questStaticQuestOne.EnemyDropItemIdList.Add(mstQuestDrawItemData.itemId);
										if (!this.QuestStaticData.dropItemQuestMap.ContainsKey(mstQuestDrawItemData.itemId))
										{
											this.QuestStaticData.dropItemQuestMap.Add(mstQuestDrawItemData.itemId, new HashSet<int>());
										}
										this.QuestStaticData.dropItemQuestMap[mstQuestDrawItemData.itemId].Add(mstQuestoneData.questOneId);
									}
								}
							}
							friends.rank = mstQuestEnemyfriendsData.rank;
							friends.level = mstQuestEnemyfriendsData.level;
							friends.kizunaLevel = mstQuestEnemyfriendsData.kizunaLevel;
							friends.yasei = mstQuestEnemyfriendsData.yasei;
							friends.miracleLevel = mstQuestEnemyfriendsData.miracleLevel;
							friends.miracleMax = 1 == mstQuestEnemyfriendsData.miracleMax;
							friends.dropItemId = mstQuestEnemyfriendsData.drawId;
							friends.photo = new ScenarioParty.Photo[]
							{
								new ScenarioParty.Photo
								{
									id = mstQuestEnemyfriendsData.photoId1,
									level = mstQuestEnemyfriendsData.level,
									limit = mstQuestEnemyfriendsData.photoLimit1
								},
								new ScenarioParty.Photo
								{
									id = mstQuestEnemyfriendsData.photoId2,
									level = mstQuestEnemyfriendsData.level,
									limit = mstQuestEnemyfriendsData.photoLimit2
								},
								new ScenarioParty.Photo
								{
									id = mstQuestEnemyfriendsData.photoId3,
									level = mstQuestEnemyfriendsData.level,
									limit = mstQuestEnemyfriendsData.photoLimit3
								},
								new ScenarioParty.Photo
								{
									id = mstQuestEnemyfriendsData.photoId4,
									level = mstQuestEnemyfriendsData.level,
									limit = mstQuestEnemyfriendsData.photoLimit4
								}
							};
							friends.nanairoAbilityReleaseFlag = mstQuestEnemyfriendsData.releaseNanairoAbility == 1;
							list5.Add(friends);
						}
						num = i + 1;
					}
					waveStatic.vsFriendsList = ScriptableObject.CreateInstance<ScenarioParty>();
					waveStatic.vsFriendsList.friends = list5.ToArray();
					waveStatic.vsFriendsList == null;
					questStaticQuestOne.waveData.waveList.Add(waveStatic);
				}
				else
				{
					foreach (MstQuestWaveData mstQuestWaveData2 in list3)
					{
						QuestStaticWave.WaveStatic waveStatic2 = new QuestStaticWave.WaveStatic
						{
							id = mstQuestWaveData2.waveId,
							enemiesId = mstQuestWaveData2.enemiesId,
							InfoId = mstQuestWaveData2.textinfoId,
							authName = mstQuestWaveData2.bossAuth,
							bgmName = mstQuestWaveData2.waveStartBgmId,
							enemyList = new List<QuestStaticWave.EnemyData>(),
							victoryBgmName = mstQuestWaveData2.victoryBgmId
						};
						MstQuestEnemiesData mstQuestEnemiesData3 = (dictionary4.ContainsKey(mstQuestWaveData2.enemiesId) ? dictionary4[mstQuestWaveData2.enemiesId] : null);
						List<int> list7 = new List<int>();
						List<int> list8 = new List<int>();
						if (mstQuestEnemiesData3 != null)
						{
							list7 = new List<int> { mstQuestEnemiesData3.enemyId00, mstQuestEnemiesData3.enemyId01, mstQuestEnemiesData3.enemyId02, mstQuestEnemiesData3.enemyId03, mstQuestEnemiesData3.enemyId04 };
							list8 = new List<int> { mstQuestEnemiesData3.escapeEnemyHpratio00, mstQuestEnemiesData3.escapeEnemyHpratio01, mstQuestEnemiesData3.escapeEnemyHpratio02, mstQuestEnemiesData3.escapeEnemyHpratio03, mstQuestEnemiesData3.escapeEnemyHpratio04 };
						}
						for (int j = 0; j < list7.Count; j++)
						{
							int num2 = list7[j];
							int num3 = list8[j];
							MstQuestEnemyData mstQuestEnemyData = (dictionary5.ContainsKey(num2) ? dictionary5[num2] : null);
							if (mstQuestEnemyData != null)
							{
								if (enemyMap.ContainsKey(mstQuestEnemyData.enemyCharaId))
								{
									questStaticQuestOne.ennemyAttrMask |= enemyMap[mstQuestEnemyData.enemyCharaId].baseData.attributeMask;
								}
								if (dictionary6.ContainsKey(mstQuestEnemyData.drawId))
								{
									foreach (MstQuestDrawItemData mstQuestDrawItemData2 in dictionary6[mstQuestEnemyData.drawId])
									{
										if (mstQuestDrawItemData2.itemId != 0 && -1 != mstQuestDrawItemData2.itemId)
										{
											questStaticQuestOne.EnemyDropDrawDataList.Add(mstQuestDrawItemData2);
											questStaticQuestOne.EnemyDropItemIdList.Add(mstQuestDrawItemData2.itemId);
											if (!this.QuestStaticData.dropItemQuestMap.ContainsKey(mstQuestDrawItemData2.itemId))
											{
												this.QuestStaticData.dropItemQuestMap.Add(mstQuestDrawItemData2.itemId, new HashSet<int>());
											}
											this.QuestStaticData.dropItemQuestMap[mstQuestDrawItemData2.itemId].Add(mstQuestoneData.questOneId);
										}
									}
								}
								QuestStaticWave.EnemyData enemyData = new QuestStaticWave.EnemyData
								{
									id = mstQuestEnemyData.enemyId,
									charaId = mstQuestEnemyData.enemyCharaId,
									level = mstQuestEnemyData.level,
									hpratio = num3
								};
								waveStatic2.enemyList.Add(enemyData);
							}
						}
						questStaticQuestOne.waveData.waveList.Add(waveStatic2);
					}
					questStaticQuestOne.waveData.waveList.Sort((QuestStaticWave.WaveStatic a, QuestStaticWave.WaveStatic b) => a.id - b.id);
				}
				this.QuestStaticData.oneDataMap.Add(mstQuestoneData.questOneId, questStaticQuestOne);
			}
		}
		this.QuestStaticData.oneDataList = new List<QuestStaticQuestOne>(this.QuestStaticData.oneDataMap.Values);
		using (List<MstQuestQuestgroupData>.Enumerator enumerator9 = mst3.GetEnumerator())
		{
			while (enumerator9.MoveNext())
			{
				MstQuestQuestgroupData mstQuestGroup = enumerator9.Current;
				List<MstQuestQuestdropItemData> list9 = mst16.FindAll((MstQuestQuestdropItemData x) => x.targetId == mstQuestGroup.questGroupId && 3 == x.category);
				QuestStaticQuestGroup questStaticQuestGroup = new QuestStaticQuestGroup(mstQuestGroup, DataManagerQuest.<InitializeMstDataInternal>g__CreateDrawItemList|99_0(mst10, list9), DataManager.DmPhoto.PhotoQuestDropItemList);
				questStaticQuestGroup.questOneList = this.QuestStaticData.oneDataList.FindAll((QuestStaticQuestOne item) => item.questGroupId == mstQuestGroup.questGroupId);
				if (0 < questStaticQuestGroup.DrawItemIdList.Count)
				{
					if (!dictionary.ContainsKey(questStaticQuestGroup.questGroupId))
					{
						dictionary.Add(questStaticQuestGroup.questGroupId, new List<DataManagerQuest.DrawItemTermData>());
					}
					foreach (DataManagerQuest.DrawItemData drawItemData2 in questStaticQuestGroup.DrawItemIdList)
					{
						dictionary[questStaticQuestGroup.questGroupId].Add(new DataManagerQuest.DrawItemTermData
						{
							Id = questStaticQuestGroup.questGroupId,
							StartDateTime = drawItemData2.StartDateTime,
							EndDateTime = drawItemData2.EndDateTime
						});
					}
				}
				if (dictionary.ContainsKey(questStaticQuestGroup.questGroupId))
				{
					if (!dictionary2.ContainsKey(questStaticQuestGroup.mapId))
					{
						dictionary2.Add(questStaticQuestGroup.mapId, new List<DataManagerQuest.DrawItemTermData>());
					}
					dictionary2[questStaticQuestGroup.mapId].AddRange(dictionary[questStaticQuestGroup.questGroupId]);
					dictionary[questStaticQuestGroup.questGroupId] = DataManagerQuest.<InitializeMstDataInternal>g__DistinctDrawItemTermData|99_1(dictionary[questStaticQuestGroup.questGroupId]);
					questStaticQuestGroup.drawItemTermDataList = dictionary[questStaticQuestGroup.questGroupId];
				}
				else
				{
					questStaticQuestGroup.drawItemTermDataList = new List<DataManagerQuest.DrawItemTermData>();
				}
				this.QuestStaticData.groupDataMap.Add(mstQuestGroup.questGroupId, questStaticQuestGroup);
			}
		}
		this.QuestStaticData.groupDataList = new List<QuestStaticQuestGroup>(this.QuestStaticData.groupDataMap.Values);
		using (List<MstQuestMapData>.Enumerator enumerator10 = mst2.GetEnumerator())
		{
			while (enumerator10.MoveNext())
			{
				MstQuestMapData mstMap = enumerator10.Current;
				List<MstQuestQuestdropItemData> list10 = mst16.FindAll((MstQuestQuestdropItemData x) => x.targetId == mstMap.mapId && 2 == x.category);
				QuestStaticMap questStaticMap = new QuestStaticMap(mstMap, DataManagerQuest.<InitializeMstDataInternal>g__CreateDrawItemList|99_0(mst10, list10), DataManager.DmPhoto.PhotoQuestDropItemList);
				questStaticMap.questGroupList = this.QuestStaticData.groupDataList.FindAll((QuestStaticQuestGroup item) => item.mapId == mstMap.mapId);
				questStaticMap.HighEndTimeByGroup = questStaticMap.questGroupList.Max<QuestStaticQuestGroup, DateTime>((QuestStaticQuestGroup item) => item.endDatetime);
				questStaticMap.LowStartTimeByGroup = questStaticMap.questGroupList.Min<QuestStaticQuestGroup, DateTime>((QuestStaticQuestGroup item) => item.startDatetime);
				if (0 < questStaticMap.DrawItemIdList.Count)
				{
					if (!dictionary2.ContainsKey(questStaticMap.mapId))
					{
						dictionary2.Add(questStaticMap.mapId, new List<DataManagerQuest.DrawItemTermData>());
					}
					foreach (DataManagerQuest.DrawItemData drawItemData3 in questStaticMap.DrawItemIdList)
					{
						dictionary2[questStaticMap.mapId].Add(new DataManagerQuest.DrawItemTermData
						{
							Id = questStaticMap.mapId,
							StartDateTime = drawItemData3.StartDateTime,
							EndDateTime = drawItemData3.EndDateTime
						});
					}
				}
				if (dictionary2.ContainsKey(questStaticMap.mapId))
				{
					if (!dictionary3.ContainsKey(questStaticMap.chapterId))
					{
						dictionary3.Add(questStaticMap.chapterId, new List<DataManagerQuest.DrawItemTermData>());
					}
					dictionary3[questStaticMap.chapterId].AddRange(dictionary2[questStaticMap.mapId]);
					dictionary2[questStaticMap.mapId] = DataManagerQuest.<InitializeMstDataInternal>g__DistinctDrawItemTermData|99_1(dictionary2[questStaticMap.mapId]);
					questStaticMap.drawItemTermDataList = dictionary2[questStaticMap.mapId];
				}
				else
				{
					questStaticMap.drawItemTermDataList = new List<DataManagerQuest.DrawItemTermData>();
				}
				this.QuestStaticData.mapDataMap.Add(mstMap.mapId, questStaticMap);
			}
		}
		this.QuestStaticData.mapDataList = new List<QuestStaticMap>(this.QuestStaticData.mapDataMap.Values);
		using (List<MstQuestChapterData>.Enumerator enumerator11 = mst.GetEnumerator())
		{
			while (enumerator11.MoveNext())
			{
				MstQuestChapterData mstChapter = enumerator11.Current;
				List<MstQuestQuestdropItemData> list11 = mst16.FindAll((MstQuestQuestdropItemData x) => x.targetId == mstChapter.chapterId && 1 == x.category);
				QuestStaticChapter questStaticChapter = new QuestStaticChapter(mstChapter, DataManagerQuest.<InitializeMstDataInternal>g__CreateDrawItemList|99_0(mst10, list11), DataManager.DmPhoto.PhotoQuestDropItemList);
				questStaticChapter.mapDataList = this.QuestStaticData.mapDataList.FindAll((QuestStaticMap item) => item.chapterId == mstChapter.chapterId);
				if (0 < questStaticChapter.DrawItemIdList.Count)
				{
					if (!dictionary3.ContainsKey(questStaticChapter.chapterId))
					{
						dictionary3.Add(questStaticChapter.chapterId, new List<DataManagerQuest.DrawItemTermData>());
					}
					foreach (DataManagerQuest.DrawItemData drawItemData4 in questStaticChapter.DrawItemIdList)
					{
						dictionary3[questStaticChapter.chapterId].Add(new DataManagerQuest.DrawItemTermData
						{
							Id = questStaticChapter.chapterId,
							StartDateTime = drawItemData4.StartDateTime,
							EndDateTime = drawItemData4.EndDateTime
						});
					}
					dictionary3[questStaticChapter.chapterId] = DataManagerQuest.<InitializeMstDataInternal>g__DistinctDrawItemTermData|99_1(dictionary3[questStaticChapter.chapterId]);
					questStaticChapter.drawItemTermDataList = dictionary3[questStaticChapter.chapterId];
				}
				else
				{
					questStaticChapter.drawItemTermDataList = new List<DataManagerQuest.DrawItemTermData>();
				}
				this.QuestStaticData.chapterDataMap.Add(mstChapter.chapterId, questStaticChapter);
			}
		}
		this.QuestStaticData.chapterDataList = new List<QuestStaticChapter>(this.QuestStaticData.chapterDataMap.Values);
		this.QuestStaticData.ruleDataList = new List<QuestStaticRule>();
		foreach (MstQuestRuleData mstQuestRuleData in mst5)
		{
			QuestStaticRule questStaticRule = new QuestStaticRule(mstQuestRuleData);
			this.QuestStaticData.ruleDataList.Add(questStaticRule);
		}
		this.QuestDynamicData = new QuestDynamicData();
		this.battleMissionPackList = new List<BattleMissionPack>();
		using (List<MstQuestQuestoneData>.Enumerator enumerator5 = mst4.GetEnumerator())
		{
			while (enumerator5.MoveNext())
			{
				MstQuestQuestoneData qqd = enumerator5.Current;
				BattleMissionPack battleMissionPack = new BattleMissionPack();
				battleMissionPack.quest_one_id = qqd.questOneId;
				battleMissionPack.staticData = new BattleMissionStatic();
				MstQuestCompData mstQuestCompData = mst14.Find((MstQuestCompData x) => x.compItemId == qqd.compItemId);
				battleMissionPack.staticData.completeBonus = new ItemData(mstQuestCompData.itemId00, mstQuestCompData.itemNum00);
				List<BattleMissionOneStatic> list12 = new List<BattleMissionOneStatic>();
				MstQuestEvalSetData mstQuestEvalSetData = mst12.Find((MstQuestEvalSetData x) => x.evalSetId == qqd.evalSetId);
				using (List<int>.Enumerator enumerator13 = new List<int> { mstQuestEvalSetData.evalId00, mstQuestEvalSetData.evalId01, mstQuestEvalSetData.evalId02 }.GetEnumerator())
				{
					while (enumerator13.MoveNext())
					{
						int evalId = enumerator13.Current;
						MstQuestEvalData mstQuestEvalData = mst11.Find((MstQuestEvalData x) => x.evalId == evalId);
						if (mstQuestEvalData != null)
						{
							list12.Add(new BattleMissionOneStatic
							{
								type = (BattleMissionType)Enum.ToObject(typeof(BattleMissionType), mstQuestEvalData.evalKey00),
								value0 = mstQuestEvalData.evalValue00,
								text = mstQuestEvalData.evalName
							});
						}
						else
						{
							list12.Add(new BattleMissionOneStatic
							{
								type = BattleMissionType.INVALID,
								value0 = 0,
								text = ""
							});
						}
					}
				}
				battleMissionPack.staticData.mission = new List<BattleMissionOneStatic>(list12);
				this.battleMissionPackList.Add(battleMissionPack);
			}
		}
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00031084 File Offset: 0x0002F284
	public void UpdateQuestCharaSealedByAssets(QuestSealedDatas updateData)
	{
		int num = this.QuestSealedCharaDatas.FindIndex((DataManagerQuest.QuestSealedCharaData item) => item.target == updateData.target && item.questOneId == updateData.questOneId);
		List<int> list = new List<int>();
		DataManagerQuest.QuestSealedJsonData questSealedJsonData = JsonUtility.FromJson<DataManagerQuest.QuestSealedJsonData>(updateData.json);
		list.AddRange(questSealedJsonData.sealedList);
		if (num != -1)
		{
			this.QuestSealedCharaDatas[num].sealedList = list;
			return;
		}
		DataManagerQuest.QuestSealedCharaData questSealedCharaData = new DataManagerQuest.QuestSealedCharaData(updateData.target, updateData.questOneId, updateData.json);
		this.QuestSealedCharaDatas.Add(questSealedCharaData);
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x00031128 File Offset: 0x0002F328
	public void Destory()
	{
		foreach (string text in this.loadAssetQuestparamList)
		{
			AssetManager.UnloadAssetData(AssetManager.PREFIX_PATH_QUEST_PARAM + text, AssetManager.OWNER.DataManagerQuest);
		}
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x00031188 File Offset: 0x0002F388
	[CompilerGenerated]
	internal static List<DataManagerQuest.DrawItemData> <InitializeMstDataInternal>g__CreateDrawItemList|99_0(List<MstQuestDrawItemData> questDrawItemDataList, List<MstQuestQuestdropItemData> questDropItemList)
	{
		List<DataManagerQuest.DrawItemData> list = new List<DataManagerQuest.DrawItemData>();
		using (List<MstQuestQuestdropItemData>.Enumerator enumerator = questDropItemList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MstQuestQuestdropItemData mstDropItem = enumerator.Current;
				DataManagerQuest.DrawItemData drawItemData = new DataManagerQuest.DrawItemData();
				drawItemData.StartDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstDropItem.startDatetime));
				drawItemData.EndDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstDropItem.endDatetime));
				drawItemData.DropItemList = new List<ItemInput>();
				foreach (MstQuestDrawItemData mstQuestDrawItemData in questDrawItemDataList.FindAll((MstQuestDrawItemData x) => x.drawId == mstDropItem.bonusDrawId))
				{
					if (mstQuestDrawItemData.itemId != 0)
					{
						drawItemData.DropItemList.Add(new ItemInput(mstQuestDrawItemData.itemId, mstQuestDrawItemData.itemNum));
					}
				}
				list.Add(drawItemData);
			}
		}
		return list;
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x000312A8 File Offset: 0x0002F4A8
	[CompilerGenerated]
	internal static List<DataManagerQuest.DrawItemTermData> <InitializeMstDataInternal>g__DistinctDrawItemTermData|99_1(List<DataManagerQuest.DrawItemTermData> drawItemTermDataList)
	{
		List<DataManagerQuest.DrawItemTermData> list = new List<DataManagerQuest.DrawItemTermData>();
		using (List<DataManagerQuest.DrawItemTermData>.Enumerator enumerator = drawItemTermDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DataManagerQuest.DrawItemTermData termData = enumerator.Current;
				if (list.Find((DataManagerQuest.DrawItemTermData x) => x.Id == termData.Id && x.StartDateTime == termData.StartDateTime && x.EndDateTime == termData.EndDateTime) == null)
				{
					list.Add(termData);
				}
			}
		}
		return list;
	}

	// Token: 0x0400063B RID: 1595
	private DataManager parentData;

	// Token: 0x0400063C RID: 1596
	private List<string> loadAssetQuestparamList = new List<string>();

	// Token: 0x0400063D RID: 1597
	private HashSet<int> finishEventChapterId = new HashSet<int>();

	// Token: 0x0400063E RID: 1598
	private List<BattleMissionPack> battleMissionPackList;

	// Token: 0x0200074E RID: 1870
	public enum BattleEndStatus
	{
		// Token: 0x040032C9 RID: 13001
		INVALID,
		// Token: 0x040032CA RID: 13002
		CLEAR,
		// Token: 0x040032CB RID: 13003
		DEAD,
		// Token: 0x040032CC RID: 13004
		RETIRE
	}

	// Token: 0x0200074F RID: 1871
	public class QuestStartData
	{
		// Token: 0x060035BE RID: 13758 RVA: 0x001C55A6 File Offset: 0x001C37A6
		public QuestStartData(QuestStartResponse qsr)
		{
			this.SetResponseData(qsr.hash_id, qsr.drew_items, qsr.enemies, qsr.start_time);
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x001C55CC File Offset: 0x001C37CC
		public QuestStartData(CoopStartResponse csr)
		{
			this.SetResponseData(csr.hash_id, csr.drew_items, csr.enemies, csr.start_time);
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x001C55F2 File Offset: 0x001C37F2
		private void SetResponseData(long hashId, List<DrewItem> drewItemList, List<int> WaveEnemiesIdList, long StartTime)
		{
			this.hash_id = hashId;
			this.drew_items = drewItemList;
			this.waveEnemiesIdList = WaveEnemiesIdList;
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(StartTime));
		}

		// Token: 0x040032CD RID: 13005
		public long hash_id;

		// Token: 0x040032CE RID: 13006
		public List<DrewItem> drew_items;

		// Token: 0x040032CF RID: 13007
		public List<int> waveEnemiesIdList;

		// Token: 0x040032D0 RID: 13008
		public DateTime startTime;
	}

	// Token: 0x02000750 RID: 1872
	public class QuestEndData
	{
		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060035C1 RID: 13761 RVA: 0x001C561B File Offset: 0x001C381B
		// (set) Token: 0x060035C2 RID: 13762 RVA: 0x001C5623 File Offset: 0x001C3823
		public int QuestResult { get; private set; }

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060035C3 RID: 13763 RVA: 0x001C562C File Offset: 0x001C382C
		// (set) Token: 0x060035C4 RID: 13764 RVA: 0x001C5634 File Offset: 0x001C3834
		public List<Quest> Quests { get; private set; }

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060035C5 RID: 13765 RVA: 0x001C563D File Offset: 0x001C383D
		// (set) Token: 0x060035C6 RID: 13766 RVA: 0x001C5645 File Offset: 0x001C3845
		public bool IsCharaScenario { get; private set; }

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x060035C7 RID: 13767 RVA: 0x001C564E File Offset: 0x001C384E
		// (set) Token: 0x060035C8 RID: 13768 RVA: 0x001C5656 File Offset: 0x001C3856
		public List<DataManagerQuest.QuestDropData> DropItemDataList { get; private set; }

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x060035C9 RID: 13769 RVA: 0x001C565F File Offset: 0x001C385F
		// (set) Token: 0x060035CA RID: 13770 RVA: 0x001C5667 File Offset: 0x001C3867
		public bool MovePresentBox { get; private set; }

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x060035CB RID: 13771 RVA: 0x001C5670 File Offset: 0x001C3870
		// (set) Token: 0x060035CC RID: 13772 RVA: 0x001C5678 File Offset: 0x001C3878
		public List<KizunaBonus> KizunaBonus { get; private set; }

		// Token: 0x060035CD RID: 13773 RVA: 0x001C5681 File Offset: 0x001C3881
		public QuestEndData(QuestEndResponse qer)
		{
			this.SetResponseData(qer.result_quest, qer.quests, qer.chara_scenario, qer.drew_items, qer.item_presentbox, qer.kizuna_bonuspoint);
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x001C56B3 File Offset: 0x001C38B3
		public QuestEndData(CoopEndResponse cer)
		{
			this.SetResponseData(cer.result_quest, cer.quests, 0, cer.drew_items, cer.item_presentbox, cer.kizuna_bonuspoint);
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x001C56E0 File Offset: 0x001C38E0
		private void SetResponseData(int questResult, List<Quest> quests, int charaScenario, List<DrewItem> drewItems, int itemPresentBox, List<KizunaBonus> kizunaBonus)
		{
			this.QuestResult = questResult;
			this.Quests = quests;
			this.IsCharaScenario = charaScenario != 0;
			this.DropItemDataList = new List<DataManagerQuest.QuestDropData>();
			foreach (DrewItem drewItem in drewItems)
			{
				this.DropItemDataList.Add(new DataManagerQuest.QuestDropData(drewItem));
			}
			this.MovePresentBox = itemPresentBox != 0;
			this.KizunaBonus = kizunaBonus;
		}
	}

	// Token: 0x02000751 RID: 1873
	public class QuestRestartData
	{
		// Token: 0x060035D0 RID: 13776 RVA: 0x001C5770 File Offset: 0x001C3970
		public QuestRestartData(QuestReStartCheckResponse qsr)
		{
			this.error_type = qsr.error_type;
			this.questing = qsr.questing;
		}

		// Token: 0x040032D7 RID: 13015
		public int error_type;

		// Token: 0x040032D8 RID: 13016
		public int questing;
	}

	// Token: 0x02000752 RID: 1874
	public class DrawItemData
	{
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x060035D1 RID: 13777 RVA: 0x001C5790 File Offset: 0x001C3990
		// (set) Token: 0x060035D2 RID: 13778 RVA: 0x001C5798 File Offset: 0x001C3998
		public List<ItemInput> DropItemList { get; set; }

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x060035D3 RID: 13779 RVA: 0x001C57A1 File Offset: 0x001C39A1
		// (set) Token: 0x060035D4 RID: 13780 RVA: 0x001C57A9 File Offset: 0x001C39A9
		public DateTime StartDateTime { get; set; }

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x060035D5 RID: 13781 RVA: 0x001C57B2 File Offset: 0x001C39B2
		// (set) Token: 0x060035D6 RID: 13782 RVA: 0x001C57BA File Offset: 0x001C39BA
		public DateTime EndDateTime { get; set; }
	}

	// Token: 0x02000753 RID: 1875
	public class DrawItemTermData
	{
		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x060035D8 RID: 13784 RVA: 0x001C57CB File Offset: 0x001C39CB
		// (set) Token: 0x060035D9 RID: 13785 RVA: 0x001C57D3 File Offset: 0x001C39D3
		public int Id { get; set; }

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x060035DA RID: 13786 RVA: 0x001C57DC File Offset: 0x001C39DC
		// (set) Token: 0x060035DB RID: 13787 RVA: 0x001C57E4 File Offset: 0x001C39E4
		public DateTime StartDateTime { get; set; }

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x060035DC RID: 13788 RVA: 0x001C57ED File Offset: 0x001C39ED
		// (set) Token: 0x060035DD RID: 13789 RVA: 0x001C57F5 File Offset: 0x001C39F5
		public DateTime EndDateTime { get; set; }
	}

	// Token: 0x02000754 RID: 1876
	public class WaveTextInfoData
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x060035DF RID: 13791 RVA: 0x001C5806 File Offset: 0x001C3A06
		// (set) Token: 0x060035E0 RID: 13792 RVA: 0x001C580E File Offset: 0x001C3A0E
		public int InfoId { get; set; }

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x060035E1 RID: 13793 RVA: 0x001C5817 File Offset: 0x001C3A17
		// (set) Token: 0x060035E2 RID: 13794 RVA: 0x001C581F File Offset: 0x001C3A1F
		public DataManagerQuest.WaveTextInfoData.DispType InfoDispType { get; set; }

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x060035E3 RID: 13795 RVA: 0x001C5828 File Offset: 0x001C3A28
		// (set) Token: 0x060035E4 RID: 13796 RVA: 0x001C5830 File Offset: 0x001C3A30
		public int DispTime { get; set; }

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060035E5 RID: 13797 RVA: 0x001C5839 File Offset: 0x001C3A39
		// (set) Token: 0x060035E6 RID: 13798 RVA: 0x001C5841 File Offset: 0x001C3A41
		public string InfoText { get; set; }

		// Token: 0x060035E7 RID: 13799 RVA: 0x001C584C File Offset: 0x001C3A4C
		public WaveTextInfoData(MstQuestTextinfoData mstInfo)
		{
			this.InfoId = mstInfo.infoId;
			this.InfoDispType = (DataManagerQuest.WaveTextInfoData.DispType)mstInfo.dispType;
			this.TurnCount = mstInfo.turnCnt;
			this.DispTime = mstInfo.dispTime;
			this.InfoText = mstInfo.text;
		}

		// Token: 0x040032E1 RID: 13025
		public int TurnCount;

		// Token: 0x02001142 RID: 4418
		public enum DispType
		{
			// Token: 0x04005ECC RID: 24268
			PLAYER,
			// Token: 0x04005ECD RID: 24269
			ENEMY,
			// Token: 0x04005ECE RID: 24270
			LOSE,
			// Token: 0x04005ECF RID: 24271
			SKILL
		}
	}

	// Token: 0x02000755 RID: 1877
	public class QuestDropData
	{
		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x060035E8 RID: 13800 RVA: 0x001C589B File Offset: 0x001C3A9B
		// (set) Token: 0x060035E9 RID: 13801 RVA: 0x001C58A3 File Offset: 0x001C3AA3
		public int EnemyId { get; private set; }

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x060035EA RID: 13802 RVA: 0x001C58AC File Offset: 0x001C3AAC
		// (set) Token: 0x060035EB RID: 13803 RVA: 0x001C58B4 File Offset: 0x001C3AB4
		public int ItemId { get; private set; }

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060035EC RID: 13804 RVA: 0x001C58BD File Offset: 0x001C3ABD
		// (set) Token: 0x060035ED RID: 13805 RVA: 0x001C58C5 File Offset: 0x001C3AC5
		public int ItemNum { get; private set; }

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060035EE RID: 13806 RVA: 0x001C58CE File Offset: 0x001C3ACE
		// (set) Token: 0x060035EF RID: 13807 RVA: 0x001C58D6 File Offset: 0x001C3AD6
		public int PhotoBonusNum { get; private set; }

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060035F0 RID: 13808 RVA: 0x001C58DF File Offset: 0x001C3ADF
		// (set) Token: 0x060035F1 RID: 13809 RVA: 0x001C58E7 File Offset: 0x001C3AE7
		public int CampaignBonusNum { get; private set; }

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060035F2 RID: 13810 RVA: 0x001C58F0 File Offset: 0x001C3AF0
		// (set) Token: 0x060035F3 RID: 13811 RVA: 0x001C58F8 File Offset: 0x001C3AF8
		public DataManagerQuest.QuestDropData.DropType Type { get; private set; }

		// Token: 0x060035F4 RID: 13812 RVA: 0x001C5904 File Offset: 0x001C3B04
		public QuestDropData(DrewItem srvItem)
		{
			this.EnemyId = srvItem.enemy_id;
			this.ItemId = srvItem.item_id;
			this.ItemNum = srvItem.item_num;
			this.PhotoBonusNum = srvItem.photobonus_num;
			this.CampaignBonusNum = srvItem.campaignbonus_num;
			this.Type = (DataManagerQuest.QuestDropData.DropType)srvItem.drop_type;
		}

		// Token: 0x02001143 RID: 4419
		public enum DropType
		{
			// Token: 0x04005ED1 RID: 24273
			Invalid,
			// Token: 0x04005ED2 RID: 24274
			Enemy,
			// Token: 0x04005ED3 RID: 24275
			Term,
			// Token: 0x04005ED4 RID: 24276
			PhotoMine,
			// Token: 0x04005ED5 RID: 24277
			PhotoHelper
		}
	}

	// Token: 0x02000756 RID: 1878
	[Serializable]
	public class QuestSealedCharaData
	{
		// Token: 0x060035F5 RID: 13813 RVA: 0x001C5960 File Offset: 0x001C3B60
		public QuestSealedCharaData(int target, int questOneId, string json)
		{
			this.target = target;
			this.questOneId = questOneId;
			this.sealedList = new List<int>();
			DataManagerQuest.QuestSealedJsonData questSealedJsonData = JsonUtility.FromJson<DataManagerQuest.QuestSealedJsonData>(json);
			this.sealedList.AddRange(questSealedJsonData.sealedList);
		}

		// Token: 0x040032EA RID: 13034
		public int target;

		// Token: 0x040032EB RID: 13035
		public int questOneId;

		// Token: 0x040032EC RID: 13036
		public List<int> sealedList;
	}

	// Token: 0x02000757 RID: 1879
	public class QuestSealedJsonData
	{
		// Token: 0x040032ED RID: 13037
		public List<int> sealedList;
	}

	// Token: 0x02000758 RID: 1880
	[Serializable]
	public class JsonData<T>
	{
		// Token: 0x040032EE RID: 13038
		public T[] data;
	}
}
