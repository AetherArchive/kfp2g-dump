using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x0200009B RID: 155
public class DataManagerPvp
{
	// Token: 0x06000697 RID: 1687 RVA: 0x0002BFA0 File Offset: 0x0002A1A0
	public DataManagerPvp(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0002C008 File Offset: 0x0002A208
	public PvpPackData GetPvpPackDataBySeasonID(int seasonID)
	{
		return this.pvpPackDataList.Find((PvpPackData item) => item.seasonId == seasonID);
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0002C03C File Offset: 0x0002A23C
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

	// Token: 0x0600069A RID: 1690 RVA: 0x0002C0CC File Offset: 0x0002A2CC
	public PvpStaticData GetPvpStaticDataBySeasonID(int seasonID)
	{
		return this.pvpStaticDataList.Find((PvpStaticData item) => item.seasonId == seasonID);
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0002C0FD File Offset: 0x0002A2FD
	public List<PvpSpecialEffectData> GetReleasePvpSpecialEffectList()
	{
		return this.releasePvpSpecialEffectList;
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0002C105 File Offset: 0x0002A305
	public PvpSpecialEffectData GetCurrentPvpSpecialEffectList()
	{
		if (this.releasePvpSpecialEffectList == null || this.releasePvpSpecialEffectList.Count <= 0)
		{
			return null;
		}
		return this.releasePvpSpecialEffectList.Max<PvpSpecialEffectData>();
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0002C12A File Offset: 0x0002A32A
	public DataManagerPvp.InformationPvpSpecialEffect GetInformationPvpSpecialEffect(bool isClear)
	{
		DataManagerPvp.InformationPvpSpecialEffect informationPvpSpecialEffect = this.informationPvpSpecialEffect;
		if (isClear)
		{
			this.informationPvpSpecialEffect = new DataManagerPvp.InformationPvpSpecialEffect();
		}
		return informationPvpSpecialEffect;
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x0002C140 File Offset: 0x0002A340
	public DataManagerPvp.PvPEndResult GetLastPvPEndResult()
	{
		return this.lastPvPEndResult;
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x0002C148 File Offset: 0x0002A348
	public void RequestGetPvpInfo(bool isEventSolution, int solutionPvpSeasonId)
	{
		this.parentData.ServerRequest(PvPCmd.Create(isEventSolution ? 0 : 1, solutionPvpSeasonId), new Action<Command>(this.CbPvPCmd));
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x0002C170 File Offset: 0x0002A370
	public void RequestRefreshEnemyList(int pvpSeasonId)
	{
		PvpStaticData.Type type = this.seasonId2Type.TryGetValueEx(pvpSeasonId, PvpStaticData.Type.INVALID);
		PvpStaticData pvpStaticDataBySeasonID = this.GetPvpStaticDataBySeasonID(pvpSeasonId);
		this.parentData.ServerRequest(PvPOppUpdateListCmd.Create((int)type, pvpStaticDataBySeasonID.seasonId, pvpStaticDataBySeasonID.pvpServerId), new Action<Command>(this.CbPvPOppUpdateListCmd));
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0002C1BC File Offset: 0x0002A3BC
	public void RequestRecoveryPvpStamina(int pvpSeasonId)
	{
		PvpStaticData.Type type = this.seasonId2Type.TryGetValueEx(pvpSeasonId, PvpStaticData.Type.INVALID);
		this.parentData.ServerRequest(PvPChalRecoveryCmd.Create((int)type, pvpSeasonId), new Action<Command>(this.CbPvPChalRecoveryCmd));
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0002C1F5 File Offset: 0x0002A3F5
	// (set) Token: 0x060006A3 RID: 1699 RVA: 0x0002C1FD File Offset: 0x0002A3FD
	public PvPStartResponse lastPvPStartResponse { get; private set; }

	// Token: 0x060006A4 RID: 1700 RVA: 0x0002C208 File Offset: 0x0002A408
	public void RequestActionPvPStart(int pvpSeasonId, PvpDynamicData.EnemyInfo enemy, int useStamina)
	{
		PvpStaticData.Type type = this.seasonId2Type.TryGetValueEx(pvpSeasonId, PvpStaticData.Type.INVALID);
		PvpStaticData pvpStaticDataBySeasonID = this.GetPvpStaticDataBySeasonID(pvpSeasonId);
		this.lastPvPStartResponse = null;
		this.parentData.ServerRequest(PvPStartCmd.Create((int)type, pvpStaticDataBySeasonID.seasonId, pvpStaticDataBySeasonID.pvpServerId, enemy.friendId, useStamina), new Action<Command>(this.CbPvPStartCmd));
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x0002C264 File Offset: 0x0002A464
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

	// Token: 0x060006A6 RID: 1702 RVA: 0x0002C364 File Offset: 0x0002A564
	public void RequestActionPvPDeckSelect(int pvpSeasonId, int deckId)
	{
		PvpStaticData.Type type = this.seasonId2Type.TryGetValueEx(pvpSeasonId, PvpStaticData.Type.INVALID);
		this.parentData.ServerRequest(PvPDeckSelectCmd.Create((int)type, pvpSeasonId, deckId), new Action<Command>(this.CbPvPDeckSelectCmd));
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x0002C3A0 File Offset: 0x0002A5A0
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

	// Token: 0x060006A8 RID: 1704 RVA: 0x0002C928 File Offset: 0x0002AB28
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

	// Token: 0x060006A9 RID: 1705 RVA: 0x0002CA7C File Offset: 0x0002AC7C
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

	// Token: 0x060006AA RID: 1706 RVA: 0x0002CBC0 File Offset: 0x0002ADC0
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

	// Token: 0x060006AB RID: 1707 RVA: 0x0002CC54 File Offset: 0x0002AE54
	private void CbPvPStartCmd(Command cmd)
	{
		PvPStartResponse pvPStartResponse = cmd.response as PvPStartResponse;
		Request request = cmd.request;
		this.lastPvPStartResponse = pvPStartResponse;
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x0002CC7C File Offset: 0x0002AE7C
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

	// Token: 0x060006AD RID: 1709 RVA: 0x0002D090 File Offset: 0x0002B290
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

	// Token: 0x060006AE RID: 1710 RVA: 0x0002D170 File Offset: 0x0002B370
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

	// Token: 0x04000610 RID: 1552
	private DataManager parentData;

	// Token: 0x04000611 RID: 1553
	private List<PvpStaticData> pvpStaticDataList = new List<PvpStaticData>();

	// Token: 0x04000612 RID: 1554
	private List<PvpPackData> pvpPackDataList = new List<PvpPackData>();

	// Token: 0x04000613 RID: 1555
	private Dictionary<int, PvpStaticData.Type> seasonId2Type = new Dictionary<int, PvpStaticData.Type>();

	// Token: 0x04000614 RID: 1556
	private Dictionary<int, PvpSpecialEffectData> allPvpSpecialEffectMap = new Dictionary<int, PvpSpecialEffectData>();

	// Token: 0x04000615 RID: 1557
	private List<PvpSpecialEffectData> releasePvpSpecialEffectList = new List<PvpSpecialEffectData>();

	// Token: 0x04000616 RID: 1558
	private DataManagerPvp.InformationPvpSpecialEffect informationPvpSpecialEffect = new DataManagerPvp.InformationPvpSpecialEffect();

	// Token: 0x04000617 RID: 1559
	private DataManagerPvp.PvPEndResult lastPvPEndResult = new DataManagerPvp.PvPEndResult();

	// Token: 0x02000738 RID: 1848
	public class InformationPvpSpecialEffect
	{
		// Token: 0x04003281 RID: 12929
		public HashSet<PvpSpecialEffectData> releaseEffectList = new HashSet<PvpSpecialEffectData>();

		// Token: 0x04003282 RID: 12930
		public DataManagerPvp.SpecialRewardType specialRewardType;

		// Token: 0x04003283 RID: 12931
		public ItemData specialRewardItem;

		// Token: 0x04003284 RID: 12932
		public DataManagerPvp.InformationPvpSpecialEffect.BattleResult battleResult;

		// Token: 0x02001140 RID: 4416
		public enum BattleResult
		{
			// Token: 0x04005EC2 RID: 24258
			INVALID,
			// Token: 0x04005EC3 RID: 24259
			WIN,
			// Token: 0x04005EC4 RID: 24260
			LOSE
		}
	}

	// Token: 0x02000739 RID: 1849
	public enum SpecialRewardType
	{
		// Token: 0x04003286 RID: 12934
		INVALID,
		// Token: 0x04003287 RID: 12935
		CONSECUTIVE_WIN_BONUS,
		// Token: 0x04003288 RID: 12936
		CONSECUTIVE_LOSE_BONUS
	}

	// Token: 0x0200073A RID: 1850
	public class PvPEndResult
	{
		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06003583 RID: 13699 RVA: 0x001C50AC File Offset: 0x001C32AC
		public int getPvpPoint
		{
			get
			{
				return this.nowPvpPoint - this.befPvpPoint;
			}
		}

		// Token: 0x04003289 RID: 12937
		public int befPvpPoint;

		// Token: 0x0400328A RID: 12938
		public int nowPvpPoint;

		// Token: 0x0400328B RID: 12939
		public int getPvpCoin;

		// Token: 0x0400328C RID: 12940
		public int campaignAddCoin;

		// Token: 0x0400328D RID: 12941
		public int bonusAddCoin;

		// Token: 0x0400328E RID: 12942
		public DataManagerPvp.SpecialRewardType specialRewardType;

		// Token: 0x0400328F RID: 12943
		public ItemData specialRewardItem;

		// Token: 0x04003290 RID: 12944
		public List<KizunaBonus> KizunaBonus;

		// Token: 0x04003291 RID: 12945
		public int calcPointByBase;

		// Token: 0x04003292 RID: 12946
		public int calcPointByWinning;

		// Token: 0x04003293 RID: 12947
		public float calcPointByTurn;

		// Token: 0x04003294 RID: 12948
		public float calcPointByDifficulty;
	}
}
