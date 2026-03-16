using System;
using System.Collections.Generic;
using Battle;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerTraining
{
	public DataManagerTraining(DataManager p)
	{
		this.parentData = p;
	}

	public TrainingPackData GetTrainingPackData()
	{
		return this.trainingPackData;
	}

	public DateTime GetTrainingEndTime(int seasonId)
	{
		MstTrainingSeasonData mstTrainingSeasonData = this.mstTrainingSeasonData.Find((MstTrainingSeasonData itm) => itm.seasonId == seasonId);
		if (mstTrainingSeasonData != null)
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(mstTrainingSeasonData.seasonEndDatetime));
		}
		return new DateTime(1900, 1, 1);
	}

	public TrainingTrialInfo GetTrainingTrialInfo()
	{
		return this.trainingTrialInfo;
	}

	public TrainingMineHistory GetTrainingMineHistory()
	{
		return this.trainingMineHistory;
	}

	public TrainingRankingData GetTrainingRankingData()
	{
		return this.trainingRankingData;
	}

	public List<SeasonTrainingRankingData> GetSeasonTrainingRankingData()
	{
		return this.seasonTrainingRankingList;
	}

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

	public DataManagerTraining.TrainingStartData LastTrainingStartResponse { get; private set; }

	public DataManagerTraining.TrainingEndData LastTrainingEndResponse { get; private set; }

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

	public void UpdateLastTrainingTime(long time)
	{
		this.lastTrainingDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(time));
	}

	public void RequestGetTrainingInfo()
	{
		this.parentData.ServerRequest(TrainingInfoCmd.Create(), new Action<Command>(this.CbTrainingInfoCmd));
	}

	public void RequestActionTrainingStart(int seasonId, DayOfWeek dayOfWeek, int questOneId, int deckId)
	{
		int num = DataManager.DmDeck.GetUserDeckById(deckId).CalcDeckKemoStatusWithPhoto(false, 0);
		this.parentData.ServerRequest(TrainingStartCmd.Create(seasonId, TimeManager.ConvertDayOfWeekByClient2Server(dayOfWeek), questOneId, deckId, num), new Action<Command>(this.CbTrainingStartCmd));
	}

	public void RequestActionTrainingEnd(int seasonId, DayOfWeek dayOfWeek, DataManagerQuest.BattleEndStatus endStatus, long point, int finishTurnNum, int okawariCnt, int maxChainNum, int chainCnt, int chainSumCnt, int maxDamage, int killNum, int artsCnt, int bossId, List<int> killMobEnemyList, int playerSkillCnt)
	{
		this.parentData.ServerRequest(TrainingEndCmd.Create(seasonId, TimeManager.ConvertDayOfWeekByClient2Server(dayOfWeek), this.LastTrainingStartResponse.hashId, point, finishTurnNum, okawariCnt, maxChainNum, chainCnt, chainSumCnt, maxDamage, killNum, (int)endStatus, artsCnt, bossId, killMobEnemyList, playerSkillCnt), new Action<Command>(this.CbTrainingEndCmd));
	}

	public void RequestGetTrainingMyScore(int seasonId)
	{
		this.parentData.ServerRequest(TrainingMyScoreCmd.Create(seasonId), new Action<Command>(this.CbTrainingMyScoreCmd));
	}

	public void RequestGetTrainingRanking(int seasonId)
	{
		long num = 0L;
		if (this.trainingRankingData != null && this.trainingRankingData.seasonId == seasonId)
		{
			num = PrjUtil.ConvertTicksToTime(this.trainingRankingData.lastUpdateTime.Ticks);
		}
		this.parentData.ServerRequest(TrainingRankingCmd.Create(seasonId, -1, num), new Action<Command>(this.CbTrainingRankingCmd));
	}

	public void RequestActionRecoveryPlayNum(int seasonId, DayOfWeek dayOfWeek)
	{
		this.parentData.ServerRequest(TrainingLimitRecoveryCmd.Create(seasonId, TimeManager.ConvertDayOfWeekByClient2Server(dayOfWeek)), new Action<Command>(this.CbRecoveryPlayNumCmd));
	}

	public void RequestGetSeasonTrainingRanking(int seasonId)
	{
		this.RequestGetSeasonTrainingRankingInternal(seasonId, 0);
	}

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

	public void RequestTrainingTrialInfo(int trial_id)
	{
		this.parentData.ServerRequest(TrainingTrialInfoCmd.Create(trial_id), new Action<Command>(this.CbTrainingTrialInfoCmd));
	}

	public void RequestTrainingJoinTrial(int trial_id)
	{
		this.parentData.ServerRequest(TrainingJoinTrialCmd.Create(trial_id), new Action<Command>(this.CbTrainingJoinTrialCmd));
	}

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

	private void CbTrainingStartCmd(Command cmd)
	{
		TrainingStartResponse trainingStartResponse = cmd.response as TrainingStartResponse;
		Request request = cmd.request;
		this.LastTrainingStartResponse = new DataManagerTraining.TrainingStartData(trainingStartResponse);
		this.UpdateLastTrainingTime(trainingStartResponse.training_starttime);
	}

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

	private void CbRecoveryPlayNumCmd(Command cmd)
	{
		TrainingLimitRecoveryResponse trainingLimitRecoveryResponse = cmd.response as TrainingLimitRecoveryResponse;
		Request request = cmd.request;
		this.trainingPackData.dynamicData.todayRecoveryNum = trainingLimitRecoveryResponse.today_recovery_num;
		this.parentData.UpdateUserAssetByAssets(trainingLimitRecoveryResponse.assets);
		this.UpdateLastTrainingTime(0L);
	}

	private void CbTrainingTrialInfoCmd(Command cmd)
	{
		TrainingTrialInfoParamResponse trainingTrialInfoParamResponse = cmd.response as TrainingTrialInfoParamResponse;
		this.trainingTrialInfo = trainingTrialInfoParamResponse.info;
	}

	private void CbTrainingJoinTrialCmd(Command cmd)
	{
		TrainingJoinTrialParamResponse trainingJoinTrialParamResponse = cmd.response as TrainingJoinTrialParamResponse;
		this.trainingTrialInfo = trainingJoinTrialParamResponse.info;
	}

	public void UpdateLastTrainingStartResponse(long hash)
	{
		this.LastTrainingStartResponse = new DataManagerTraining.TrainingStartData
		{
			hashId = hash
		};
	}

	private DataManager parentData;

	private List<MstTrainingSeasonData> mstTrainingSeasonData;

	private List<MstTrainingDayofweekData> mstTrainingDayofweekData;

	private List<MstTrainingRewardData> mstTrainingRewardData;

	private List<MstTrainingPracticeTrialData> mstTrainingPracticeTrialData;

	private List<int> seasonIdListByStartTime;

	private TrainingPackData trainingPackData;

	private TrainingMineHistory trainingMineHistory;

	private TrainingRankingData trainingRankingData;

	private List<SeasonTrainingRankingData> seasonTrainingRankingList = new List<SeasonTrainingRankingData> { null, null };

	private TrainingTrialInfo trainingTrialInfo;

	private DateTime lastTrainingDateTime;

	public class TrainingStartData
	{
		public TrainingStartData(TrainingStartResponse res)
		{
			this.hashId = res.hash_id;
		}

		public TrainingStartData()
		{
		}

		public long hashId;

		public long hiscore;
	}

	public class TrainingEndData
	{
		public TrainingStaticData.RewardData rewardData;
	}
}
