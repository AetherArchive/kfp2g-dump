using System;
using System.Collections.Generic;
using Battle;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x020000A8 RID: 168
public class DataManagerTraining
{
	// Token: 0x06000767 RID: 1895 RVA: 0x00033457 File Offset: 0x00031657
	public DataManagerTraining(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x0003347F File Offset: 0x0003167F
	public TrainingPackData GetTrainingPackData()
	{
		return this.trainingPackData;
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x00033488 File Offset: 0x00031688
	public DateTime GetTrainingEndTime(int seasonId)
	{
		MstTrainingSeasonData mstTrainingSeasonData = this.mstTrainingSeasonData.Find((MstTrainingSeasonData itm) => itm.seasonId == seasonId);
		if (mstTrainingSeasonData != null)
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(mstTrainingSeasonData.seasonEndDatetime));
		}
		return new DateTime(1900, 1, 1);
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x000334DA File Offset: 0x000316DA
	public TrainingTrialInfo GetTrainingTrialInfo()
	{
		return this.trainingTrialInfo;
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x000334E2 File Offset: 0x000316E2
	public TrainingMineHistory GetTrainingMineHistory()
	{
		return this.trainingMineHistory;
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x000334EA File Offset: 0x000316EA
	public TrainingRankingData GetTrainingRankingData()
	{
		return this.trainingRankingData;
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x000334F2 File Offset: 0x000316F2
	public List<SeasonTrainingRankingData> GetSeasonTrainingRankingData()
	{
		return this.seasonTrainingRankingList;
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x000334FC File Offset: 0x000316FC
	public MstTrainingPracticeTrialData GetValidPracticeTrialData()
	{
		MstTrainingPracticeTrialData mstTrainingPracticeTrialData = null;
		DateTime now = TimeManager.Now;
		foreach (MstTrainingPracticeTrialData mstTrainingPracticeTrialData2 in this.mstTrainingPracticeTrialData)
		{
			DateTime dateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstTrainingPracticeTrialData2.startTime));
			DateTime dateTime2 = new DateTime(PrjUtil.ConvertTimeToTicks(mstTrainingPracticeTrialData2.endTime));
			if (!(now < dateTime) && !(dateTime2 < now))
			{
				mstTrainingPracticeTrialData = mstTrainingPracticeTrialData2;
				break;
			}
		}
		return mstTrainingPracticeTrialData;
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x0600076F RID: 1903 RVA: 0x00033590 File Offset: 0x00031790
	// (set) Token: 0x06000770 RID: 1904 RVA: 0x00033598 File Offset: 0x00031798
	public DataManagerTraining.TrainingStartData LastTrainingStartResponse { get; private set; }

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000771 RID: 1905 RVA: 0x000335A1 File Offset: 0x000317A1
	// (set) Token: 0x06000772 RID: 1906 RVA: 0x000335A9 File Offset: 0x000317A9
	public DataManagerTraining.TrainingEndData LastTrainingEndResponse { get; private set; }

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000773 RID: 1907 RVA: 0x000335B4 File Offset: 0x000317B4
	public bool IsChallengePossible
	{
		get
		{
			if (DataManager.DmGameStatus.MakeUserFlagData().ReleaseModeFlag.TrainingByQuestTop == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked)
			{
				return false;
			}
			DateTime dateTime = new DateTime(this.lastTrainingDateTime.Year, this.lastTrainingDateTime.Month, this.lastTrainingDateTime.Day, 0, 0, 0);
			DateTime now = TimeManager.Now;
			DateTime dateTime2 = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
			return dateTime != dateTime2;
		}
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0003362D File Offset: 0x0003182D
	public void UpdateLastTrainingTime(long time)
	{
		this.lastTrainingDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(time));
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x00033640 File Offset: 0x00031840
	public void RequestGetTrainingInfo()
	{
		this.parentData.ServerRequest(TrainingInfoCmd.Create(), new Action<Command>(this.CbTrainingInfoCmd));
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x00033660 File Offset: 0x00031860
	public void RequestActionTrainingStart(int seasonId, DayOfWeek dayOfWeek, int questOneId, int deckId)
	{
		int num = DataManager.DmDeck.GetUserDeckById(deckId).CalcDeckKemoStatusWithPhoto(false, 0);
		this.parentData.ServerRequest(TrainingStartCmd.Create(seasonId, TimeManager.ConvertDayOfWeekByClient2Server(dayOfWeek), questOneId, deckId, num), new Action<Command>(this.CbTrainingStartCmd));
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x000336A8 File Offset: 0x000318A8
	public void RequestActionTrainingEnd(int seasonId, DayOfWeek dayOfWeek, DataManagerQuest.BattleEndStatus endStatus, long point, int finishTurnNum, int okawariCnt, int maxChainNum, int chainCnt, int chainSumCnt, int maxDamage, int killNum, int artsCnt, int bossId, List<int> killMobEnemyList, int playerSkillCnt)
	{
		this.parentData.ServerRequest(TrainingEndCmd.Create(seasonId, TimeManager.ConvertDayOfWeekByClient2Server(dayOfWeek), this.LastTrainingStartResponse.hashId, point, finishTurnNum, okawariCnt, maxChainNum, chainCnt, chainSumCnt, maxDamage, killNum, (int)endStatus, artsCnt, bossId, killMobEnemyList, playerSkillCnt), new Action<Command>(this.CbTrainingEndCmd));
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x000336FC File Offset: 0x000318FC
	public void RequestGetTrainingMyScore(int seasonId)
	{
		this.parentData.ServerRequest(TrainingMyScoreCmd.Create(seasonId), new Action<Command>(this.CbTrainingMyScoreCmd));
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x0003371C File Offset: 0x0003191C
	public void RequestGetTrainingRanking(int seasonId)
	{
		long num = 0L;
		if (this.trainingRankingData != null && this.trainingRankingData.seasonId == seasonId)
		{
			num = PrjUtil.ConvertTicksToTime(this.trainingRankingData.lastUpdateTime.Ticks);
		}
		this.parentData.ServerRequest(TrainingRankingCmd.Create(seasonId, -1, num), new Action<Command>(this.CbTrainingRankingCmd));
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x0003377A File Offset: 0x0003197A
	public void RequestActionRecoveryPlayNum(int seasonId, DayOfWeek dayOfWeek)
	{
		this.parentData.ServerRequest(TrainingLimitRecoveryCmd.Create(seasonId, TimeManager.ConvertDayOfWeekByClient2Server(dayOfWeek)), new Action<Command>(this.CbRecoveryPlayNumCmd));
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x0003379F File Offset: 0x0003199F
	public void RequestGetSeasonTrainingRanking(int seasonId)
	{
		this.RequestGetSeasonTrainingRankingInternal(seasonId, 0);
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x000337AC File Offset: 0x000319AC
	private void RequestGetSeasonTrainingRankingInternal(int seasonId, int saveIndex)
	{
		long num = 0L;
		if (this.seasonTrainingRankingList[saveIndex] != null && this.seasonTrainingRankingList[saveIndex].seasonId == seasonId)
		{
			num = PrjUtil.ConvertTicksToTime(this.seasonTrainingRankingList[saveIndex].lastUpdateTime.Ticks);
		}
		this.parentData.ServerRequest(TrainingPointRankingCmd.Create(seasonId, num), delegate(Command cmd)
		{
			this.CbTrainingPointRankingCmd(cmd, saveIndex);
		});
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x0003383E File Offset: 0x00031A3E
	public void RequestTrainingTrialInfo(int trial_id)
	{
		this.parentData.ServerRequest(TrainingTrialInfoCmd.Create(trial_id), new Action<Command>(this.CbTrainingTrialInfoCmd));
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0003385D File Offset: 0x00031A5D
	public void RequestTrainingJoinTrial(int trial_id)
	{
		this.parentData.ServerRequest(TrainingJoinTrialCmd.Create(trial_id), new Action<Command>(this.CbTrainingJoinTrialCmd));
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x0003387C File Offset: 0x00031A7C
	private void CbTrainingPointRankingCmd(Command cmd, int saveIndex)
	{
		TrainingPointRankingResponse trainingPointRankingResponse = cmd.response as TrainingPointRankingResponse;
		TrainingPointRankingRequest trainingPointRankingRequest = cmd.request as TrainingPointRankingRequest;
		if (this.seasonTrainingRankingList[saveIndex] == null || this.seasonTrainingRankingList[saveIndex].lastUpdateTime.Ticks != PrjUtil.ConvertTimeToTicks(trainingPointRankingResponse.last_update_time))
		{
			this.seasonTrainingRankingList[saveIndex] = new SeasonTrainingRankingData(trainingPointRankingResponse)
			{
				seasonId = trainingPointRankingRequest.season_id
			};
		}
		if (saveIndex == 0)
		{
			int num = this.seasonIdListByStartTime.IndexOf(trainingPointRankingRequest.season_id);
			if (num > 0)
			{
				this.RequestGetSeasonTrainingRankingInternal(this.seasonIdListByStartTime[num - 1], 1);
			}
		}
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00033924 File Offset: 0x00031B24
	public void InitializeMstData(MstManager mstManager)
	{
		this.mstTrainingSeasonData = mstManager.GetMst<List<MstTrainingSeasonData>>(MstType.TRAINING_SEASON_DATA);
		this.mstTrainingDayofweekData = mstManager.GetMst<List<MstTrainingDayofweekData>>(MstType.TRAINING_DAYOFWEEK_DATA);
		this.mstTrainingRewardData = mstManager.GetMst<List<MstTrainingRewardData>>(MstType.TRAINING_REWARD_DATA);
		this.mstTrainingPracticeTrialData = mstManager.GetMst<List<MstTrainingPracticeTrialData>>(MstType.TRAINING_PRACTICE_TRIAL_DATA);
		this.mstTrainingPracticeTrialData.Sort((MstTrainingPracticeTrialData a, MstTrainingPracticeTrialData b) => b.id.CompareTo(a.id));
		this.mstTrainingSeasonData.Sort((MstTrainingSeasonData a, MstTrainingSeasonData b) => a.seasonStartDatetime.CompareTo(b.seasonStartDatetime));
		this.seasonIdListByStartTime = this.mstTrainingSeasonData.ConvertAll<int>((MstTrainingSeasonData item) => item.seasonId);
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x000339F0 File Offset: 0x00031BF0
	private void CbTrainingInfoCmd(Command cmd)
	{
		TrainingInfoResponse trainingInfoResponse = cmd.response as TrainingInfoResponse;
		Request request = cmd.request;
		if (this.trainingPackData == null)
		{
			this.trainingPackData = new TrainingPackData();
		}
		if (this.trainingPackData.staticData == null || this.trainingPackData.staticData.SeasonId != trainingInfoResponse.season_id)
		{
			this.trainingPackData.staticData = new TrainingStaticData(trainingInfoResponse.season_id, this.mstTrainingSeasonData, this.mstTrainingDayofweekData, this.mstTrainingRewardData);
		}
		this.trainingPackData.dynamicData = new TrainingDynamicData
		{
			currentDayOfWeek = TimeManager.ConvertDayOfWeekByServer2Client(trainingInfoResponse.dayofweek),
			todayPlayNum = trainingInfoResponse.today_play_num,
			todayRecoveryNum = trainingInfoResponse.today_recovery_num,
			hiScore = trainingInfoResponse.hiscore
		};
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00033AB8 File Offset: 0x00031CB8
	private void CbTrainingMyScoreCmd(Command cmd)
	{
		TrainingMyScoreResponse trainingMyScoreResponse = cmd.response as TrainingMyScoreResponse;
		TrainingMyScoreRequest trainingMyScoreRequest = cmd.request as TrainingMyScoreRequest;
		this.trainingMineHistory = new TrainingMineHistory();
		this.trainingMineHistory.seasonId = trainingMyScoreRequest.season_id;
		foreach (TrainingScore trainingScore in trainingMyScoreResponse.training_score)
		{
			DayOfWeek dayOfWeek = TimeManager.ConvertDayOfWeekByServer2Client(trainingScore.dayofweek);
			if (trainingScore.playtime == 0L || trainingScore.party == null)
			{
				this.trainingMineHistory.dayOfWeekDataList[dayOfWeek] = null;
			}
			else
			{
				this.trainingMineHistory.dayOfWeekDataList[dayOfWeek] = new TrainingMineHistory.DayOfWeekHistory
				{
					dayOfWeek = dayOfWeek,
					point = trainingScore.hiscore,
					updateTime = new DateTime(PrjUtil.ConvertTimeToTicks(trainingScore.playtime)),
					deckInfo = new SceneBattle_DeckInfo(trainingScore.party)
				};
			}
		}
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x00033BBC File Offset: 0x00031DBC
	private void CbTrainingRankingCmd(Command cmd)
	{
		TrainingRankingResponse trainingRankingResponse = cmd.response as TrainingRankingResponse;
		TrainingRankingRequest trainingRankingRequest = cmd.request as TrainingRankingRequest;
		if (this.trainingRankingData != null && this.trainingRankingData.lastUpdateTime.Ticks == PrjUtil.ConvertTimeToTicks(trainingRankingResponse.last_update_time))
		{
			return;
		}
		this.trainingRankingData = new TrainingRankingData(trainingRankingResponse.last_update_time);
		this.trainingRankingData.seasonId = trainingRankingRequest.season_id;
		foreach (TrainingRanking trainingRanking in trainingRankingResponse.training_ranking)
		{
			DayOfWeek dayOfWeek = TimeManager.ConvertDayOfWeekByServer2Client(trainingRanking.trainingscore.dayofweek);
			this.trainingRankingData.dayOfWeekRankingList[dayOfWeek].Add(new TrainingRankingData.RankingOne
			{
				number = trainingRanking.rank,
				point = trainingRanking.trainingscore.hiscore,
				updateTime = new DateTime(PrjUtil.ConvertTimeToTicks(trainingRanking.trainingscore.playtime)),
				userName = trainingRanking.user_name,
				userLevel = trainingRanking.user_level,
				favoriteCharaId = trainingRanking.favorite_chara_id,
				favoriteCharaFaceId = trainingRanking.favorite_chara_face_id,
				achievementId = trainingRanking.achievement_id,
				deckInfo = new SceneBattle_DeckInfo(trainingRanking.trainingscore.party)
			});
		}
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00033D34 File Offset: 0x00031F34
	private void CbTrainingStartCmd(Command cmd)
	{
		TrainingStartResponse trainingStartResponse = cmd.response as TrainingStartResponse;
		Request request = cmd.request;
		this.LastTrainingStartResponse = new DataManagerTraining.TrainingStartData(trainingStartResponse);
		this.UpdateLastTrainingTime(trainingStartResponse.training_starttime);
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x00033D6C File Offset: 0x00031F6C
	private void CbTrainingEndCmd(Command cmd)
	{
		TrainingEndResponse res = cmd.response as TrainingEndResponse;
		Request request = cmd.request;
		TrainingStaticData.RewardData rewardData = this.trainingPackData.staticData.rewardList.Find((TrainingStaticData.RewardData item) => item.RewardId == res.reward_id);
		this.LastTrainingEndResponse = new DataManagerTraining.TrainingEndData
		{
			rewardData = rewardData
		};
		this.parentData.UpdateUserAssetByAssets(res.assets);
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00033DE4 File Offset: 0x00031FE4
	private void CbRecoveryPlayNumCmd(Command cmd)
	{
		TrainingLimitRecoveryResponse trainingLimitRecoveryResponse = cmd.response as TrainingLimitRecoveryResponse;
		Request request = cmd.request;
		this.trainingPackData.dynamicData.todayRecoveryNum = trainingLimitRecoveryResponse.today_recovery_num;
		this.parentData.UpdateUserAssetByAssets(trainingLimitRecoveryResponse.assets);
		this.UpdateLastTrainingTime(0L);
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00033E34 File Offset: 0x00032034
	private void CbTrainingTrialInfoCmd(Command cmd)
	{
		TrainingTrialInfoParamResponse trainingTrialInfoParamResponse = cmd.response as TrainingTrialInfoParamResponse;
		this.trainingTrialInfo = trainingTrialInfoParamResponse.info;
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x00033E5C File Offset: 0x0003205C
	private void CbTrainingJoinTrialCmd(Command cmd)
	{
		TrainingJoinTrialParamResponse trainingJoinTrialParamResponse = cmd.response as TrainingJoinTrialParamResponse;
		this.trainingTrialInfo = trainingJoinTrialParamResponse.info;
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00033E81 File Offset: 0x00032081
	public void UpdateLastTrainingStartResponse(long hash)
	{
		this.LastTrainingStartResponse = new DataManagerTraining.TrainingStartData
		{
			hashId = hash
		};
	}

	// Token: 0x04000680 RID: 1664
	private DataManager parentData;

	// Token: 0x04000681 RID: 1665
	private List<MstTrainingSeasonData> mstTrainingSeasonData;

	// Token: 0x04000682 RID: 1666
	private List<MstTrainingDayofweekData> mstTrainingDayofweekData;

	// Token: 0x04000683 RID: 1667
	private List<MstTrainingRewardData> mstTrainingRewardData;

	// Token: 0x04000684 RID: 1668
	private List<MstTrainingPracticeTrialData> mstTrainingPracticeTrialData;

	// Token: 0x04000685 RID: 1669
	private List<int> seasonIdListByStartTime;

	// Token: 0x04000686 RID: 1670
	private TrainingPackData trainingPackData;

	// Token: 0x04000687 RID: 1671
	private TrainingMineHistory trainingMineHistory;

	// Token: 0x04000688 RID: 1672
	private TrainingRankingData trainingRankingData;

	// Token: 0x04000689 RID: 1673
	private List<SeasonTrainingRankingData> seasonTrainingRankingList = new List<SeasonTrainingRankingData> { null, null };

	// Token: 0x0400068A RID: 1674
	private TrainingTrialInfo trainingTrialInfo;

	// Token: 0x0400068D RID: 1677
	private DateTime lastTrainingDateTime;

	// Token: 0x0200078E RID: 1934
	public class TrainingStartData
	{
		// Token: 0x0600369C RID: 13980 RVA: 0x001C69AD File Offset: 0x001C4BAD
		public TrainingStartData(TrainingStartResponse res)
		{
			this.hashId = res.hash_id;
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x001C69C1 File Offset: 0x001C4BC1
		public TrainingStartData()
		{
		}

		// Token: 0x040033A6 RID: 13222
		public long hashId;

		// Token: 0x040033A7 RID: 13223
		public long hiscore;
	}

	// Token: 0x0200078F RID: 1935
	public class TrainingEndData
	{
		// Token: 0x040033A8 RID: 13224
		public TrainingStaticData.RewardData rewardData;
	}
}
