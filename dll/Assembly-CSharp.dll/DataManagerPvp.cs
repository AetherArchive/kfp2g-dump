using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerPvp
{
	public DataManagerPvp(DataManager p)
	{
		this.parentData = p;
	}

	public PvpPackData GetPvpPackDataBySeasonID(int seasonID)
	{
		return this.pvpPackDataList.Find((PvpPackData item) => item.seasonId == seasonID);
	}

	public int GetSeasonIdByNow(DateTime now, PvpStaticData.Type pvpType)
	{
		this.pvpStaticDataList.Sort((PvpStaticData a, PvpStaticData b) => a.seasonStartTime.Ticks.CompareTo(b.seasonStartTime.Ticks));
		for (int i = 0; i < this.pvpStaticDataList.Count; i++)
		{
			if (now <= this.pvpStaticDataList[i].seasonEndTime && this.pvpStaticDataList[i].type == pvpType)
			{
				return this.pvpStaticDataList[i].seasonId;
			}
		}
		return 0;
	}

	public PvpStaticData GetPvpStaticDataBySeasonID(int seasonID)
	{
		return this.pvpStaticDataList.Find((PvpStaticData item) => item.seasonId == seasonID);
	}

	public List<PvpSpecialEffectData> GetReleasePvpSpecialEffectList()
	{
		return this.releasePvpSpecialEffectList;
	}

	public PvpSpecialEffectData GetCurrentPvpSpecialEffectList()
	{
		if (this.releasePvpSpecialEffectList == null || this.releasePvpSpecialEffectList.Count <= 0)
		{
			return null;
		}
		return this.releasePvpSpecialEffectList.Max<PvpSpecialEffectData>();
	}

	public DataManagerPvp.InformationPvpSpecialEffect GetInformationPvpSpecialEffect(bool isClear)
	{
		DataManagerPvp.InformationPvpSpecialEffect informationPvpSpecialEffect = this.informationPvpSpecialEffect;
		if (isClear)
		{
			this.informationPvpSpecialEffect = new DataManagerPvp.InformationPvpSpecialEffect();
		}
		return informationPvpSpecialEffect;
	}

	public DataManagerPvp.PvPEndResult GetLastPvPEndResult()
	{
		return this.lastPvPEndResult;
	}

	public void RequestGetPvpInfo(bool isEventSolution, int solutionPvpSeasonId)
	{
		this.parentData.ServerRequest(PvPCmd.Create(isEventSolution ? 0 : 1, solutionPvpSeasonId), new Action<Command>(this.CbPvPCmd));
	}

	public void RequestRefreshEnemyList(int pvpSeasonId)
	{
		PvpStaticData.Type type = this.seasonId2Type.TryGetValueEx(pvpSeasonId, PvpStaticData.Type.INVALID);
		PvpStaticData pvpStaticDataBySeasonID = this.GetPvpStaticDataBySeasonID(pvpSeasonId);
		this.parentData.ServerRequest(PvPOppUpdateListCmd.Create((int)type, pvpStaticDataBySeasonID.seasonId, pvpStaticDataBySeasonID.pvpServerId), new Action<Command>(this.CbPvPOppUpdateListCmd));
	}

	public void RequestRecoveryPvpStamina(int pvpSeasonId)
	{
		PvpStaticData.Type type = this.seasonId2Type.TryGetValueEx(pvpSeasonId, PvpStaticData.Type.INVALID);
		this.parentData.ServerRequest(PvPChalRecoveryCmd.Create((int)type, pvpSeasonId), new Action<Command>(this.CbPvPChalRecoveryCmd));
	}

	public PvPStartResponse lastPvPStartResponse { get; private set; }

	public void RequestActionPvPStart(int pvpSeasonId, PvpDynamicData.EnemyInfo enemy, int useStamina)
	{
		PvpStaticData.Type type = this.seasonId2Type.TryGetValueEx(pvpSeasonId, PvpStaticData.Type.INVALID);
		PvpStaticData pvpStaticDataBySeasonID = this.GetPvpStaticDataBySeasonID(pvpSeasonId);
		this.lastPvPStartResponse = null;
		this.parentData.ServerRequest(PvPStartCmd.Create((int)type, pvpStaticDataBySeasonID.seasonId, pvpStaticDataBySeasonID.pvpServerId, enemy.friendId, useStamina), new Action<Command>(this.CbPvPStartCmd));
	}

	public bool RequestActionPvPEnd(bool isWin, int finishTurnNum, int pvpSeasonId, long hash_id, PvpDynamicData.EnemyInfo.Difficulty difficulty, int okawariCnt, int maxChainNum, int chainCnt, int chainSumCnt, int maxDamage, int kemoStatus, int artsCnt, int playerSkillCnt, int tickleSuccessCnt)
	{
		PvpStaticData.Type type = this.seasonId2Type.TryGetValueEx(pvpSeasonId, PvpStaticData.Type.INVALID);
		if (this.pvpPackDataList.Count<PvpPackData>((PvpPackData item) => item.seasonId == pvpSeasonId) != 0)
		{
			this.lastPvPEndResult = new DataManagerPvp.PvPEndResult
			{
				befPvpPoint = this.pvpPackDataList.Find((PvpPackData item) => item.seasonId == pvpSeasonId).dynamicData.userInfo.pvpPoint
			};
			PvpStaticData pvpStaticDataBySeasonID = this.GetPvpStaticDataBySeasonID(pvpSeasonId);
			int num = (isWin ? 1 : 2);
			this.parentData.ServerRequest(PvPEndCmd.Create((int)type, pvpStaticDataBySeasonID.seasonId, pvpStaticDataBySeasonID.pvpServerId, hash_id, finishTurnNum, num, okawariCnt, maxChainNum, chainCnt, chainSumCnt, maxDamage, kemoStatus, artsCnt, playerSkillCnt, tickleSuccessCnt), delegate(Command cmd)
			{
				this.CbPvPEndCmd(cmd, difficulty);
			});
		}
		return this.pvpPackDataList.Count<PvpPackData>((PvpPackData item) => item.seasonId == pvpSeasonId) == 0;
	}

	public void RequestActionPvPDeckSelect(int pvpSeasonId, int deckId)
	{
		PvpStaticData.Type type = this.seasonId2Type.TryGetValueEx(pvpSeasonId, PvpStaticData.Type.INVALID);
		this.parentData.ServerRequest(PvPDeckSelectCmd.Create((int)type, pvpSeasonId, deckId), new Action<Command>(this.CbPvPDeckSelectCmd));
	}

	public void InitializeMstData(MstManager mstManager)
	{
		this.pvpStaticDataList = new List<PvpStaticData>();
		List<MstPvpData> mst = mstManager.GetMst<List<MstPvpData>>(MstType.PVP_DATA);
		List<MstPvpRankData> mst2 = mstManager.GetMst<List<MstPvpRankData>>(MstType.PVP_RANK_DATA);
		List<MstPvpWinningBonusData> mst3 = mstManager.GetMst<List<MstPvpWinningBonusData>>(MstType.PVP_WIN_BONUS_DATA);
		List<MstPvpTurnBonusData> mst4 = mstManager.GetMst<List<MstPvpTurnBonusData>>(MstType.PVP_TURN_BONUS_DATA);
		List<MstPvpOppBonusData> mst5 = mstManager.GetMst<List<MstPvpOppBonusData>>(MstType.PVP_OPP_BONUS_DATA);
		List<MstPvpDefenseData> mst6 = mstManager.GetMst<List<MstPvpDefenseData>>(MstType.PVP_DEFENSE_DATA);
		List<MstPvpSeasonData> list = mstManager.GetMst<List<MstPvpSeasonData>>(MstType.PVP_SEASON_DATA) ?? new List<MstPvpSeasonData>();
		List<MstPvpspecialData> mst7 = mstManager.GetMst<List<MstPvpspecialData>>(MstType.PVPSPECIAL_DATA);
		foreach (MstPvpSeasonData mstPvpSeasonData in list)
		{
			int pvpId = mstPvpSeasonData.pvpId;
			PvpStaticData pvpStaticData = new PvpStaticData();
			MstPvpData mstPvpData = mst.Find((MstPvpData item) => item.pvpId == pvpId);
			if (mstPvpData != null)
			{
				pvpStaticData.seasonId = mstPvpSeasonData.seasonId;
				pvpStaticData.pvpServerId = mstPvpSeasonData.pvpId;
				pvpStaticData.baseData = mstPvpData;
				pvpStaticData.type = (PvpStaticData.Type)mstPvpSeasonData.typeId;
				pvpStaticData.rewardItemId = mstPvpData.rewardItemId;
				pvpStaticData.spBattleTurnNum = mstPvpData.specialMaxTurn;
				pvpStaticData.spEventId = mstPvpSeasonData.eventId;
				pvpStaticData.rankMstList = mst2.FindAll((MstPvpRankData item) => item.pvpId == pvpId);
				pvpStaticData.rankMstList.Sort((MstPvpRankData a, MstPvpRankData b) => a.pvpPointMax - b.pvpPointMax);
				pvpStaticData.winningBonusList = mst3.FindAll((MstPvpWinningBonusData item) => item.pvpId == pvpId);
				pvpStaticData.winningBonusList.Sort((MstPvpWinningBonusData a, MstPvpWinningBonusData b) => a.winningNum - b.winningNum);
				pvpStaticData.turnBonusList = mst4.FindAll((MstPvpTurnBonusData item) => item.pvpId == pvpId);
				pvpStaticData.turnBonusList.Sort((MstPvpTurnBonusData a, MstPvpTurnBonusData b) => a.turnNum - b.turnNum);
				pvpStaticData.oppBonusList = mst5.FindAll((MstPvpOppBonusData item) => item.pvpId == pvpId);
				pvpStaticData.oppBonusList.Sort((MstPvpOppBonusData a, MstPvpOppBonusData b) => a.strength - b.strength);
				pvpStaticData.rankMstList = mst2.FindAll((MstPvpRankData item) => item.pvpId == pvpId);
				pvpStaticData.rankMstList.Sort((MstPvpRankData a, MstPvpRankData b) => a.pvpRankId - b.pvpRankId);
				pvpStaticData.rankInfoList = new List<PvpRankInfo>();
				foreach (MstPvpRankData mstPvpRankData in pvpStaticData.rankMstList)
				{
					PvpRankInfo pvpRankInfo = new PvpRankInfo();
					pvpRankInfo.id = mstPvpRankData.pvpRankId;
					pvpRankInfo.pointRangeLow = mstPvpRankData.pvpPointMin;
					pvpRankInfo.rankIcon = mstPvpRankData.pvpRankIcon;
					pvpRankInfo.rankName = mstPvpRankData.pvpRankName;
					foreach (ItemInput itemInput in new List<ItemInput>
					{
						new ItemInput(mstPvpRankData.itemId00, mstPvpRankData.itemNum00),
						new ItemInput(mstPvpRankData.itemId01, mstPvpRankData.itemNum01),
						new ItemInput(mstPvpRankData.itemId02, mstPvpRankData.itemNum02),
						new ItemInput(mstPvpRankData.itemId03, mstPvpRankData.itemNum03),
						new ItemInput(mstPvpRankData.itemId04, mstPvpRankData.itemNum04)
					})
					{
						if (itemInput.itemId != 0)
						{
							pvpRankInfo.rewardItemList.Add(new ItemData(itemInput.itemId, itemInput.num));
						}
					}
					pvpStaticData.rankInfoList.Add(pvpRankInfo);
				}
				for (int i = 0; i < pvpStaticData.rankInfoList.Count - 1; i++)
				{
					pvpStaticData.rankInfoList[i].nexRankInfo = pvpStaticData.rankInfoList[i + 1];
				}
				pvpStaticData.pvpDefenseList = mst6.FindAll((MstPvpDefenseData item) => item.pvpId == pvpId);
				pvpStaticData.pvpDefenseList.Sort((MstPvpDefenseData a, MstPvpDefenseData b) => a.winNum - b.winNum);
				pvpStaticData.seasonStartTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstPvpSeasonData.seasonStartDatetime));
				pvpStaticData.seasonEndTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstPvpSeasonData.seasonEndDatetime));
				this.pvpStaticDataList.Add(pvpStaticData);
				this.seasonId2Type[pvpStaticData.seasonId] = pvpStaticData.type;
			}
		}
		this.allPvpSpecialEffectMap = mst7.ConvertAll<PvpSpecialEffectData>((MstPvpspecialData item) => new PvpSpecialEffectData(item)).ToDictionary<PvpSpecialEffectData, int, PvpSpecialEffectData>((PvpSpecialEffectData item) => item.Id, (PvpSpecialEffectData item) => item);
	}

	private void CbPvPCmd(Command cmd)
	{
		PvPResponse pvPResponse = cmd.response as PvPResponse;
		PvPRequest pvPRequest = cmd.request as PvPRequest;
		this.pvpPackDataList = new List<PvpPackData>();
		if (pvPResponse.pvp_info_list == null)
		{
			pvPResponse.pvp_info_list = new List<PvPInfo>();
		}
		using (List<PvPInfo>.Enumerator enumerator = pvPResponse.pvp_info_list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PvPInfo pvpServer = enumerator.Current;
				PvpPackData pvpPackData = new PvpPackData();
				pvpPackData.staticData = this.pvpStaticDataList.Find((PvpStaticData item) => item.seasonId == pvpServer.season_id);
				if (pvpPackData.staticData != null)
				{
					pvpPackData.seasonId = pvpServer.season_id;
					if (pvpServer.opp_user_list == null)
					{
						pvpServer.opp_user_list = new List<OppUser>();
					}
					if (pvpServer.pvp_defense_result == null)
					{
						pvpServer.pvp_defense_result = new List<PvPDefenseResult>();
					}
					pvpPackData.dynamicData = new PvpDynamicData(pvpServer, pvpPackData.staticData, this);
					this.pvpPackDataList.Add(pvpPackData);
				}
			}
		}
		this.parentData.UpdateUserAssetByAssets(pvPResponse.assets);
		if (pvPRequest.re_flg == 0)
		{
			LocalPushUtil.ResolveNotification(LocalPushUtil.NotificationID.PVP_STAMINA_RECOVERY);
		}
	}

	private void CbPvPOppUpdateListCmd(Command cmd)
	{
		PvPOppUpdateListResponse pvPOppUpdateListResponse = cmd.response as PvPOppUpdateListResponse;
		PvPOppUpdateListRequest req = cmd.request as PvPOppUpdateListRequest;
		if (this.pvpPackDataList == null)
		{
			return;
		}
		PvpPackData pvpPackData = this.pvpPackDataList.Find((PvpPackData item) => item.staticData.pvpServerId == req.pvp_id);
		if (pvpPackData == null)
		{
			return;
		}
		pvpPackData.dynamicData.enemyInfoList = new List<PvpDynamicData.EnemyInfo>();
		foreach (OppUser oppUser in pvPOppUpdateListResponse.opp_user_list)
		{
			PvpDynamicData.EnemyInfo enemyInfo = new PvpDynamicData.EnemyInfo(oppUser);
			foreach (int num in oppUser.kemoboard_panel_list)
			{
				DataManager.DmKemoBoard.UpdateKemoboardBonusParam(ref enemyInfo.kemoBoardParamMap, num);
			}
			pvpPackData.dynamicData.enemyInfoList.Add(enemyInfo);
		}
		pvpPackData.dynamicData.SortEnemyInfoList();
		pvpPackData.dynamicData.userInfo.pvpStaminaInfo.update(pvPOppUpdateListResponse.limit_chal_count, PrjUtil.ConvertTimeToTicks(pvPOppUpdateListResponse.last_chal_datetime));
		LocalPushUtil.ResolveNotification(LocalPushUtil.NotificationID.PVP_STAMINA_RECOVERY);
	}

	private void CbPvPChalRecoveryCmd(Command cmd)
	{
		PvPChalRecoveryResponse pvPChalRecoveryResponse = cmd.response as PvPChalRecoveryResponse;
		PvPChalRecoveryRequest req = cmd.request as PvPChalRecoveryRequest;
		if (this.pvpPackDataList == null)
		{
			return;
		}
		PvpPackData pvpPackData = this.pvpPackDataList.Find((PvpPackData item) => item.staticData.seasonId == req.season_id);
		if (pvpPackData == null)
		{
			return;
		}
		pvpPackData.dynamicData.userInfo.pvpStaminaInfo.update(pvPChalRecoveryResponse.limit_chal_count, PrjUtil.ConvertTimeToTicks(pvPChalRecoveryResponse.last_chal_datetime));
		this.parentData.UpdateUserAssetByAssets(pvPChalRecoveryResponse.assets);
		LocalPushUtil.ResolveNotification(LocalPushUtil.NotificationID.PVP_STAMINA_RECOVERY);
	}

	private void CbPvPStartCmd(Command cmd)
	{
		PvPStartResponse pvPStartResponse = cmd.response as PvPStartResponse;
		Request request = cmd.request;
		this.lastPvPStartResponse = pvPStartResponse;
	}

	private void CbPvPEndCmd(Command cmd, PvpDynamicData.EnemyInfo.Difficulty EnemyDifficulty)
	{
		PvPEndResponse pvPEndResponse = cmd.response as PvPEndResponse;
		PvPEndRequest req = cmd.request as PvPEndRequest;
		if (this.pvpPackDataList == null)
		{
			return;
		}
		PvpPackData pvpPackData = this.pvpPackDataList.Find((PvpPackData item) => item.staticData.pvpServerId == req.pvp_id);
		if (pvpPackData == null)
		{
			return;
		}
		pvpPackData.dynamicData.userInfo.pvpStaminaInfo.update(pvPEndResponse.pvp_result.limit_chal_count, PrjUtil.ConvertTimeToTicks(pvPEndResponse.pvp_result.last_chal_datetime));
		pvpPackData.dynamicData.userInfo.pvpPoint = pvPEndResponse.pvp_result.pvp_point;
		this.lastPvPEndResult.nowPvpPoint = pvpPackData.dynamicData.userInfo.pvpPoint;
		this.lastPvPEndResult.campaignAddCoin = pvPEndResponse.pvp_result.c_incr_coin * pvPEndResponse.pvp_result.use_pvp_stamina;
		this.lastPvPEndResult.bonusAddCoin = pvPEndResponse.pvp_result.photobonus_num * pvPEndResponse.pvp_result.use_pvp_stamina;
		if (req.battle_result == 1)
		{
			this.lastPvPEndResult.getPvpCoin = pvpPackData.staticData.baseData.winAcquireCoin;
			if (EnemyDifficulty == PvpDynamicData.EnemyInfo.Difficulty.CHAMPION)
			{
				this.lastPvPEndResult.getPvpCoin += pvpPackData.staticData.baseData.addCoinNum;
			}
		}
		else
		{
			this.lastPvPEndResult.getPvpCoin = pvpPackData.staticData.baseData.loseAcquireCoin;
		}
		this.lastPvPEndResult.getPvpCoin *= pvPEndResponse.pvp_result.use_pvp_stamina;
		this.lastPvPEndResult.getPvpCoin += this.lastPvPEndResult.campaignAddCoin + this.lastPvPEndResult.bonusAddCoin;
		if (pvPEndResponse.pvp_result.bonus_info != null && pvPEndResponse.pvp_result.bonus_info.reason_type > 0)
		{
			this.lastPvPEndResult.specialRewardType = (DataManagerPvp.SpecialRewardType)pvPEndResponse.pvp_result.bonus_info.reason_type;
			this.lastPvPEndResult.specialRewardItem = new ItemData(pvPEndResponse.pvp_result.bonus_info.rewarditem_id, pvPEndResponse.pvp_result.bonus_info.rewarditem_num);
			this.informationPvpSpecialEffect.specialRewardType = this.lastPvPEndResult.specialRewardType;
			this.informationPvpSpecialEffect.specialRewardItem = this.lastPvPEndResult.specialRewardItem;
		}
		else
		{
			this.informationPvpSpecialEffect.specialRewardType = DataManagerPvp.SpecialRewardType.INVALID;
			this.informationPvpSpecialEffect.specialRewardItem = null;
		}
		this.lastPvPEndResult.KizunaBonus = pvPEndResponse.kizuna_bonuspoint;
		this.lastPvPEndResult.calcPointByBase = pvpPackData.staticData.baseData.winAcquirePoint;
		for (int i = 0; i < pvpPackData.staticData.winningBonusList.Count; i++)
		{
			int num = pvpPackData.staticData.winningBonusList.Count - i - 1;
			if (pvPEndResponse.pvp_result.winning_num >= pvpPackData.staticData.winningBonusList[num].winningNum)
			{
				this.lastPvPEndResult.calcPointByWinning = pvpPackData.staticData.winningBonusList[num].winAcquirePoint;
				break;
			}
		}
		for (int j = 0; j < pvpPackData.staticData.turnBonusList.Count; j++)
		{
			int num2 = pvpPackData.staticData.turnBonusList.Count - j - 1;
			if (req.turn_num >= pvpPackData.staticData.turnBonusList[num2].turnNum)
			{
				this.lastPvPEndResult.calcPointByTurn = (float)pvpPackData.staticData.turnBonusList[num2].pointOdds / 100f;
				break;
			}
		}
		MstPvpOppBonusData mstPvpOppBonusData = pvpPackData.staticData.oppBonusList.Find((MstPvpOppBonusData item) => item.strength == (int)EnemyDifficulty);
		this.lastPvPEndResult.calcPointByDifficulty = ((mstPvpOppBonusData != null) ? ((float)mstPvpOppBonusData.pointOdds / 100f) : 1f);
		this.UpdateReleasePvpSpecialEffectList(pvPEndResponse.pvpspecialReleaseIdList, false, req.battle_result == 1);
		this.parentData.UpdateUserAssetByAssets(pvPEndResponse.assets);
		LocalPushUtil.ResolveNotification(LocalPushUtil.NotificationID.PVP_STAMINA_RECOVERY);
	}

	public void UpdateReleasePvpSpecialEffectList(List<int> pvpspecialReleaseIdList, bool isInit, bool isWin)
	{
		if (!isInit)
		{
			this.informationPvpSpecialEffect.battleResult = (isWin ? DataManagerPvp.InformationPvpSpecialEffect.BattleResult.WIN : DataManagerPvp.InformationPvpSpecialEffect.BattleResult.LOSE);
		}
		if (pvpspecialReleaseIdList == null)
		{
			return;
		}
		using (List<int>.Enumerator enumerator = pvpspecialReleaseIdList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int id = enumerator.Current;
				if (!this.releasePvpSpecialEffectList.Exists((PvpSpecialEffectData item) => item.Id == id) && this.allPvpSpecialEffectMap.ContainsKey(id))
				{
					if (!isInit)
					{
						this.informationPvpSpecialEffect.releaseEffectList.Add(this.allPvpSpecialEffectMap[id]);
					}
					this.releasePvpSpecialEffectList.Add(this.allPvpSpecialEffectMap[id]);
				}
			}
		}
	}

	private void CbPvPDeckSelectCmd(Command cmd)
	{
		Response response = cmd.response;
		PvPDeckSelectRequest req = cmd.request as PvPDeckSelectRequest;
		if (this.pvpPackDataList == null)
		{
			return;
		}
		PvpPackData pvpPackData = this.pvpPackDataList.Find((PvpPackData item) => item.staticData.seasonId == req.season_id);
		if (pvpPackData == null)
		{
			return;
		}
		pvpPackData.dynamicData.userInfo.currentDeckId = req.deck_id;
	}

	private DataManager parentData;

	private List<PvpStaticData> pvpStaticDataList = new List<PvpStaticData>();

	private List<PvpPackData> pvpPackDataList = new List<PvpPackData>();

	private Dictionary<int, PvpStaticData.Type> seasonId2Type = new Dictionary<int, PvpStaticData.Type>();

	private Dictionary<int, PvpSpecialEffectData> allPvpSpecialEffectMap = new Dictionary<int, PvpSpecialEffectData>();

	private List<PvpSpecialEffectData> releasePvpSpecialEffectList = new List<PvpSpecialEffectData>();

	private DataManagerPvp.InformationPvpSpecialEffect informationPvpSpecialEffect = new DataManagerPvp.InformationPvpSpecialEffect();

	private DataManagerPvp.PvPEndResult lastPvPEndResult = new DataManagerPvp.PvPEndResult();

	public class InformationPvpSpecialEffect
	{
		public HashSet<PvpSpecialEffectData> releaseEffectList = new HashSet<PvpSpecialEffectData>();

		public DataManagerPvp.SpecialRewardType specialRewardType;

		public ItemData specialRewardItem;

		public DataManagerPvp.InformationPvpSpecialEffect.BattleResult battleResult;

		public enum BattleResult
		{
			INVALID,
			WIN,
			LOSE
		}
	}

	public enum SpecialRewardType
	{
		INVALID,
		CONSECUTIVE_WIN_BONUS,
		CONSECUTIVE_LOSE_BONUS
	}

	public class PvPEndResult
	{
		public int getPvpPoint
		{
			get
			{
				return this.nowPvpPoint - this.befPvpPoint;
			}
		}

		public int befPvpPoint;

		public int nowPvpPoint;

		public int getPvpCoin;

		public int campaignAddCoin;

		public int bonusAddCoin;

		public DataManagerPvp.SpecialRewardType specialRewardType;

		public ItemData specialRewardItem;

		public List<KizunaBonus> KizunaBonus;

		public int calcPointByBase;

		public int calcPointByWinning;

		public float calcPointByTurn;

		public float calcPointByDifficulty;
	}
}
