using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

// Token: 0x020000BC RID: 188
public class DataManagerUserInfo
{
	// Token: 0x06000821 RID: 2081 RVA: 0x0003610F File Offset: 0x0003430F
	public int getStaminaRecoveryTimeSecond(long baseDateTicks)
	{
		if (DataManager.DmCampaign.getBaseDateCampaignStaminaRecoveryData(baseDateTicks) != null)
		{
			return DataManager.DmCampaign.getBaseDateCampaignStaminaRecoveryData(baseDateTicks).staminaRecoveryTime;
		}
		return DataManager.DmServerMst.MstAppConfig.staminaRecoveryTime;
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00036140 File Offset: 0x00034340
	public StaminaIntvlChgInfo getStaminaIntvlChgInfo(long baseDateTicks)
	{
		if (DataManager.DmCampaign.getBaseDateCampaignStaminaRecoveryData(baseDateTicks) != null)
		{
			return new StaminaIntvlChgInfo(DataManager.DmCampaign.getBaseDateCampaignStaminaRecoveryData(baseDateTicks).endTime, TimeManager.Second2Tick((long)DataManager.DmServerMst.MstAppConfig.staminaRecoveryTime));
		}
		if (DataManager.DmCampaign.getBaseNextCampaignStaminaRecoveryData(baseDateTicks) != null)
		{
			return new StaminaIntvlChgInfo(DataManager.DmCampaign.getBaseNextCampaignStaminaRecoveryData(baseDateTicks).startTime.AddMilliseconds(-1.0), TimeManager.Second2Tick((long)DataManager.DmCampaign.getBaseNextCampaignStaminaRecoveryData(baseDateTicks).staminaRecoveryTime));
		}
		return null;
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x000361D0 File Offset: 0x000343D0
	public DataManagerUserInfo(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000824 RID: 2084 RVA: 0x000361DF File Offset: 0x000343DF
	// (set) Token: 0x06000825 RID: 2085 RVA: 0x000361E7 File Offset: 0x000343E7
	public string userName { get; private set; }

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06000826 RID: 2086 RVA: 0x000361F0 File Offset: 0x000343F0
	// (set) Token: 0x06000827 RID: 2087 RVA: 0x000361F8 File Offset: 0x000343F8
	public int level { get; private set; }

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06000828 RID: 2088 RVA: 0x00036201 File Offset: 0x00034401
	// (set) Token: 0x06000829 RID: 2089 RVA: 0x00036209 File Offset: 0x00034409
	public long exp { get; private set; }

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x0600082A RID: 2090 RVA: 0x00036212 File Offset: 0x00034412
	// (set) Token: 0x0600082B RID: 2091 RVA: 0x0003621A File Offset: 0x0003441A
	public int friendId { get; private set; }

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x0600082C RID: 2092 RVA: 0x00036223 File Offset: 0x00034423
	// (set) Token: 0x0600082D RID: 2093 RVA: 0x0003622B File Offset: 0x0003442B
	public StaminaInfo staminaInfo { get; private set; }

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x0600082E RID: 2094 RVA: 0x00036234 File Offset: 0x00034434
	// (set) Token: 0x0600082F RID: 2095 RVA: 0x0003623C File Offset: 0x0003443C
	public string userComment { get; private set; }

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06000830 RID: 2096 RVA: 0x00036245 File Offset: 0x00034445
	// (set) Token: 0x06000831 RID: 2097 RVA: 0x0003624D File Offset: 0x0003444D
	public DataManagerUserInfo.AvatarType avatarType { get; private set; }

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000832 RID: 2098 RVA: 0x00036256 File Offset: 0x00034456
	// (set) Token: 0x06000833 RID: 2099 RVA: 0x0003625E File Offset: 0x0003445E
	public int favoriteCharaId { get; private set; }

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000834 RID: 2100 RVA: 0x00036267 File Offset: 0x00034467
	// (set) Token: 0x06000835 RID: 2101 RVA: 0x0003626F File Offset: 0x0003446F
	public List<LoanPackData> loanPackList { get; private set; }

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06000836 RID: 2102 RVA: 0x00036278 File Offset: 0x00034478
	// (set) Token: 0x06000837 RID: 2103 RVA: 0x00036280 File Offset: 0x00034480
	public TutorialUtil.Sequence tutorialSequence { get; private set; }

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06000838 RID: 2104 RVA: 0x00036289 File Offset: 0x00034489
	// (set) Token: 0x06000839 RID: 2105 RVA: 0x00036291 File Offset: 0x00034491
	public string playedLoginScenarioList { get; private set; }

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x0600083A RID: 2106 RVA: 0x0003629A File Offset: 0x0003449A
	// (set) Token: 0x0600083B RID: 2107 RVA: 0x000362A2 File Offset: 0x000344A2
	public string playedIntroductionList { get; private set; }

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x0600083C RID: 2108 RVA: 0x000362AB File Offset: 0x000344AB
	// (set) Token: 0x0600083D RID: 2109 RVA: 0x000362B3 File Offset: 0x000344B3
	public UserOptionData optionData { get; private set; }

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x0600083E RID: 2110 RVA: 0x000362BC File Offset: 0x000344BC
	// (set) Token: 0x0600083F RID: 2111 RVA: 0x000362C4 File Offset: 0x000344C4
	public long expOverflow { get; private set; }

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06000840 RID: 2112 RVA: 0x000362CD File Offset: 0x000344CD
	// (set) Token: 0x06000841 RID: 2113 RVA: 0x000362D5 File Offset: 0x000344D5
	public int dispKizunaConfirm { get; private set; }

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06000842 RID: 2114 RVA: 0x000362DE File Offset: 0x000344DE
	// (set) Token: 0x06000843 RID: 2115 RVA: 0x000362E6 File Offset: 0x000344E6
	public bool dispPracticeConfirm { get; private set; }

	// Token: 0x06000844 RID: 2116 RVA: 0x000362F0 File Offset: 0x000344F0
	public long GetExpByNextLevel(int nowLevel)
	{
		int num = nowLevel - 1 + 1;
		if (num < DataManager.DmServerMst.gameLevelInfoList.Count)
		{
			return DataManager.DmServerMst.gameLevelInfoList[num].userLevelExp;
		}
		return 0L;
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x0003632D File Offset: 0x0003452D
	public DataManagerUserInfo.UpdateStringResult GetUpdateNameResult()
	{
		return this.updateStringResult;
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x00036335 File Offset: 0x00034535
	public DataManagerUserInfo.UpdateStringResult GetUpdateCommentResult()
	{
		return this.updateStringResult;
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00036340 File Offset: 0x00034540
	public string RequestActionUpdateUserName(string _userName)
	{
		_userName = _userName.Replace("<", "＜");
		_userName = _userName.Replace(">", "＞");
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.player_name = _userName;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
		return _userName;
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x000363A4 File Offset: 0x000345A4
	public void RequestActionUpdateUserAvatar(DataManagerUserInfo.AvatarType type)
	{
		if (this.avatarType != type)
		{
			PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
			playerInfo.player_type = (int)type;
			this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
		}
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x000363EC File Offset: 0x000345EC
	public string RequestActionUpdateUserComment(string _userComment)
	{
		_userComment = _userComment.Replace("<", "＜");
		_userComment = _userComment.Replace(">", "＞");
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.comment = _userComment;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
		return _userComment;
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00036450 File Offset: 0x00034650
	public void RequestActionUpdateFavoriteChara(int charaId)
	{
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.favorite_chara_id = charaId;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x00036490 File Offset: 0x00034690
	public void RequestActionUpdateScenarioLastId(int id, string json)
	{
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.played_login_scenario_list = json;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x000364D0 File Offset: 0x000346D0
	public void RequestActionUpdateIntroductionLastId(int id, string json)
	{
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.played_introduction_list = json;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00036510 File Offset: 0x00034710
	public void RequestActionUpdateTutorialStep(int tutorialStep)
	{
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.tutorial_step = tutorialStep;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x0003654D File Offset: 0x0003474D
	public void RequestActionSkipTutorial()
	{
		this.parentData.ServerRequest(TutorialSkipCmd.Create(), null);
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00036560 File Offset: 0x00034760
	public void RequestActionUpdateLoanPackList(List<LoanPackData> inLoanPackList)
	{
		List<MyHelper> list = inLoanPackList.ConvertAll<MyHelper>((LoanPackData item) => new MyHelper
		{
			chara_id = item.charaId,
			photo_id00 = item.photoDataIdList[0],
			photo_id01 = item.photoDataIdList[1],
			photo_id02 = item.photoDataIdList[2],
			photo_id03 = item.photoDataIdList[3]
		});
		this.parentData.ServerRequest(MyHelperChangeCmd.Create(list), new Action<Command>(this.CbMyHelperChangeCmd));
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x000365B0 File Offset: 0x000347B0
	public void RequestGetLoanPackList()
	{
		this.parentData.ServerRequest(MyHelperListCmd.Create(), new Action<Command>(this.CbMyHelperListCmd));
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x000365CE File Offset: 0x000347CE
	public void RequestGetUserOption()
	{
		this.parentData.ServerRequest(OptionGetCmd.Create(), new Action<Command>(this.CbOptionGetCmd));
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x000365EC File Offset: 0x000347EC
	public void RequestActionUpdateUserOption(UserOptionData uod)
	{
		List<int> list = uod.CreateByServerData();
		if (!this.optionData.CreateByServerData().SequenceEqual<int>(list))
		{
			this.parentData.ServerRequest(OptionSetCmd.Create(list), new Action<Command>(this.CbOptionSetCmd));
		}
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x00036630 File Offset: 0x00034830
	public void RequestActionUpdateBattleAutoFlag(QuestStaticChapter.Category category, bool flag)
	{
		List<int> list = this.optionData.CreateByServerData();
		int num = -1;
		switch (category)
		{
		case QuestStaticChapter.Category.STORY:
			num = 15;
			break;
		case QuestStaticChapter.Category.GROW:
			num = 16;
			break;
		case QuestStaticChapter.Category.CHARA:
			num = 17;
			break;
		case QuestStaticChapter.Category.PVP:
			num = 18;
			break;
		case QuestStaticChapter.Category.EVENT:
			num = 19;
			break;
		case QuestStaticChapter.Category.SIDE_STORY:
			num = 25;
			break;
		case QuestStaticChapter.Category.TRAINING:
			num = 42;
			break;
		case QuestStaticChapter.Category.CELLVAL:
			num = 15;
			break;
		case QuestStaticChapter.Category.ETCETERA:
			num = 50;
			break;
		case QuestStaticChapter.Category.STORY2:
			num = 15;
			break;
		case QuestStaticChapter.Category.STORY3:
			num = 15;
			break;
		}
		if (num != -1)
		{
			list[num] = (flag ? 1 : 0);
			this.parentData.ServerRequest(OptionSetCmd.Create(list), new Action<Command>(this.CbOptionSetCmd));
		}
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x000366F0 File Offset: 0x000348F0
	public void RequestActionUpdateBattleSpeedFlag(QuestStaticChapter.Category category, bool flag)
	{
		List<int> list = this.optionData.CreateByServerData();
		int num = -1;
		switch (category)
		{
		case QuestStaticChapter.Category.STORY:
			num = 20;
			break;
		case QuestStaticChapter.Category.GROW:
			num = 21;
			break;
		case QuestStaticChapter.Category.CHARA:
			num = 22;
			break;
		case QuestStaticChapter.Category.PVP:
			num = 23;
			break;
		case QuestStaticChapter.Category.EVENT:
			num = 24;
			break;
		case QuestStaticChapter.Category.SIDE_STORY:
			num = 26;
			break;
		case QuestStaticChapter.Category.TRAINING:
			num = 43;
			break;
		case QuestStaticChapter.Category.CELLVAL:
			num = 20;
			break;
		case QuestStaticChapter.Category.ETCETERA:
			num = 51;
			break;
		case QuestStaticChapter.Category.STORY2:
			num = 20;
			break;
		case QuestStaticChapter.Category.STORY3:
			num = 20;
			break;
		}
		if (num != -1)
		{
			list[num] = (flag ? 1 : 0);
			this.parentData.ServerRequest(OptionSetCmd.Create(list), new Action<Command>(this.CbOptionSetCmd));
		}
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x000367AD File Offset: 0x000349AD
	public void RequestActionRecoveryStamina(int useItemId, int useItemNum)
	{
		this.parentData.ServerRequest(RecoveryCmd.Create(useItemId, useItemNum, 1), new Action<Command>(this.CbRecoveryCmd));
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x000367CE File Offset: 0x000349CE
	public void RequestActionChangeTime(string dateTime, bool isBackTitle = true)
	{
		this.parentData.ServerRequest(TimeChangeCmd.Create(dateTime), isBackTitle ? new Action<Command>(this.CbChangeTimeCmd) : new Action<Command>(this.CbChangeTimeCmdWithoutTitle));
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x000367FE File Offset: 0x000349FE
	public void RequestActionPlayerLevelUp(long userExpOverflow)
	{
		this.parentData.ServerRequest(PlayerLevelUpCmd.Create(userExpOverflow), new Action<Command>(this.CbLevelUpCmd));
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0003681D File Offset: 0x00034A1D
	public void RequestActionUpdateKizunaConfirm(int confirm)
	{
		this.parentData.ServerRequest(KizunaConfirmUpdateCmd.Create(confirm), new Action<Command>(this.CbKizunaConfirmUpdateCmd));
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x0003683C File Offset: 0x00034A3C
	public void RequestActionUpdatePracticeConfirm(int confirm)
	{
		this.parentData.ServerRequest(PracticeConfirmUpdateCmd.Create(confirm), new Action<Command>(this.CbPracticeConfirmUpdateCmd));
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x0003685B File Offset: 0x00034A5B
	private void CbChangeTimeCmd(Command cmd)
	{
		DataInitializeResolver.InitializeActionDataManager();
		Singleton<SceneManager>.Instance.SetSceneReboot();
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00036870 File Offset: 0x00034A70
	private void CbChangeTimeCmdWithoutTitle(Command cmd)
	{
		DataManager.DmServerMst.RequestDownloadServerTime(delegate
		{
			DataManager.DmCampaign.InitializeMstData(Singleton<MstManager>.Instance);
		});
		PguiOpenWindowCtrl[] array = Object.FindObjectsOfType<PguiOpenWindowCtrl>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ForceClose();
		}
		CanvasManager.HdlWebViewWindowCtrl.Close();
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneHome, null);
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x000368DC File Offset: 0x00034ADC
	private void CbMyHelperListCmd(Command cmd)
	{
		MyHelperListResponse myHelperListResponse = cmd.response as MyHelperListResponse;
		this.loanPackList = new List<LoanPackData>();
		foreach (Chara chara in myHelperListResponse.helperList)
		{
			LoanPackData loanPackData = new LoanPackData();
			loanPackData.charaId = chara.chara_id;
			loanPackData.photoDataIdList = chara.photo_list.ConvertAll<long>((Photo item) => item.photo_id);
			this.loanPackList.Add(loanPackData);
		}
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0003698C File Offset: 0x00034B8C
	private void CbPlayerInfoChangeCmd(Command cmd)
	{
		PlayerInfoChangeResponse playerInfoChangeResponse = cmd.response as PlayerInfoChangeResponse;
		this.updateStringResult = new DataManagerUserInfo.UpdateStringResult();
		this.updateStringResult.isSuccess = playerInfoChangeResponse.result_name == 1 && playerInfoChangeResponse.result_comment == 1;
		this.parentData.UpdateUserAssetByAssets(playerInfoChangeResponse.assets);
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x000369E4 File Offset: 0x00034BE4
	private void CbMyHelperChangeCmd(Command cmd)
	{
		MyHelperChangeRequest myHelperChangeRequest = cmd.request as MyHelperChangeRequest;
		MyHelperChangeResponse myHelperChangeResponse = cmd.response as MyHelperChangeResponse;
		this.UpdateLoanPackListByServerData(myHelperChangeRequest.helperList);
		this.parentData.UpdateUserAssetByAssets(myHelperChangeResponse.assets);
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00036A28 File Offset: 0x00034C28
	private void CbOptionGetCmd(Command cmd)
	{
		OptionGetResponse optionGetResponse = cmd.response as OptionGetResponse;
		this.UpdateUserOptionByServer(optionGetResponse.optionList);
		Singleton<CanvasManager>.Instance.SetDisplayDirection(this.optionData.DisplayDirection);
		this.optionData.SetDisplayQuality();
		this.optionData.SetFrameRate();
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00036A78 File Offset: 0x00034C78
	private void CbOptionSetCmd(Command cmd)
	{
		OptionSetRequest optionSetRequest = cmd.request as OptionSetRequest;
		this.UpdateUserOptionByServer(optionSetRequest.optionList);
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00036AA0 File Offset: 0x00034CA0
	private void CbRecoveryCmd(Command cmd)
	{
		RecoveryResponse recoveryResponse = cmd.response as RecoveryResponse;
		this.parentData.UpdateUserAssetByAssets(recoveryResponse.assets);
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00036ACC File Offset: 0x00034CCC
	private void CbLevelUpCmd(Command cmd)
	{
		PlayerLevelUpResponse playerLevelUpResponse = cmd.response as PlayerLevelUpResponse;
		this.parentData.UpdateUserAssetByAssets(playerLevelUpResponse.assets);
		DataManager.DmUserInfo.UpdateUserExpOverflowByServer(playerLevelUpResponse.assets.exp_overflow);
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00036B0C File Offset: 0x00034D0C
	private void CbKizunaConfirmUpdateCmd(Command cmd)
	{
		KizunaConfirmUpdateCmdResponse kizunaConfirmUpdateCmdResponse = cmd.response as KizunaConfirmUpdateCmdResponse;
		DataManager.DmUserInfo.UpdateUserKizunaConfirmByServer(kizunaConfirmUpdateCmdResponse.assets.kizunaConfirm);
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00036B3C File Offset: 0x00034D3C
	private void CbPracticeConfirmUpdateCmd(Command cmd)
	{
		PracticeConfirmUpdateCmdResponse practiceConfirmUpdateCmdResponse = cmd.response as PracticeConfirmUpdateCmdResponse;
		DataManager.DmUserInfo.UpdateUserPracticeConfirmByServer(practiceConfirmUpdateCmdResponse.assets.practiceConfirm);
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00036B6C File Offset: 0x00034D6C
	public void UpdateUserDataByServer(PlayerInfo playerInfo)
	{
		int staminaLimit = DataManager.DmServerMst.gameLevelInfoList[playerInfo.player_rank - 1].staminaLimit;
		long num = PrjUtil.ConvertTimeToTicks(playerInfo.stamina_updated_at);
		this.staminaInfo = new StaminaInfo(playerInfo.stamina, num, TimeManager.Second2Tick((long)this.getStaminaRecoveryTimeSecond(num)), staminaLimit, this.getStaminaIntvlChgInfo(num));
		if (DataManager.DmTreeHouse.StaminaBonusData != null)
		{
			this.staminaInfo.limitBonus = DataManager.DmTreeHouse.StaminaBonusData.staminaBonus;
		}
		this.userName = playerInfo.player_name;
		this.level = playerInfo.player_rank;
		this.exp = DataManagerUserInfo.ConvertExp(playerInfo.player_exp);
		this.userComment = playerInfo.comment;
		this.avatarType = (DataManagerUserInfo.AvatarType)((playerInfo.player_type == 0) ? 1 : playerInfo.player_type);
		if (playerInfo.friend_id != 0)
		{
			this.friendId = playerInfo.friend_id;
		}
		this.favoriteCharaId = playerInfo.favorite_chara_id;
		this.tutorialSequence = (TutorialUtil.Sequence)playerInfo.tutorial_step;
		this.playedLoginScenarioList = playerInfo.played_login_scenario_list;
		this.playedIntroductionList = playerInfo.played_introduction_list;
		LocalPushUtil.ResolveNotification(LocalPushUtil.NotificationID.STAMINA_RECOVERY);
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00036C88 File Offset: 0x00034E88
	public void UpdateLoanPackListByServerData(List<MyHelper> myHelperList)
	{
		this.loanPackList = new List<LoanPackData>();
		foreach (MyHelper myHelper in myHelperList)
		{
			LoanPackData loanPackData = new LoanPackData();
			loanPackData.charaId = myHelper.chara_id;
			loanPackData.photoDataIdList = new List<long> { myHelper.photo_id00, myHelper.photo_id01, myHelper.photo_id02, myHelper.photo_id03 };
			this.loanPackList.Add(loanPackData);
		}
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00036D34 File Offset: 0x00034F34
	public void UpdateUserOptionByServer(List<int> option)
	{
		if (this.optionData == null)
		{
			this.optionData = new UserOptionData();
		}
		this.optionData.UpdateByServerData(option);
		foreach (object obj in Enum.GetValues(typeof(LocalPushUtil.NotificationID)))
		{
			LocalPushUtil.ResolveNotification((LocalPushUtil.NotificationID)obj);
		}
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00036DB4 File Offset: 0x00034FB4
	public void UpdateUserOptionByDebug()
	{
		if (this.optionData == null)
		{
			this.optionData = new UserOptionData();
		}
		this.optionData.UpdateByServerData(null);
		this.userName = "【しんまい】";
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00036DE0 File Offset: 0x00034FE0
	public void UpdateUserExpOverflowByServer(long exp)
	{
		this.expOverflow = exp;
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00036DE9 File Offset: 0x00034FE9
	public void UpdateUserKizunaConfirmByServer(int confirm)
	{
		this.dispKizunaConfirm = confirm;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00036DF2 File Offset: 0x00034FF2
	public void UpdateUserPracticeConfirmByServer(int confirm)
	{
		this.dispPracticeConfirm = confirm == 1;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00036E00 File Offset: 0x00035000
	private static long ConvertExp(long exp)
	{
		long num = 0L;
		foreach (GameLevelInfo gameLevelInfo in DataManager.DmServerMst.gameLevelInfoList)
		{
			if (exp < num + gameLevelInfo.userLevelExp)
			{
				break;
			}
			num += gameLevelInfo.userLevelExp;
		}
		return exp - num;
	}

	// Token: 0x04000701 RID: 1793
	private DataManager parentData;

	// Token: 0x04000712 RID: 1810
	private DataManagerUserInfo.UpdateStringResult updateStringResult;

	// Token: 0x020007B1 RID: 1969
	public enum AvatarType
	{
		// Token: 0x04003426 RID: 13350
		INVALID,
		// Token: 0x04003427 RID: 13351
		TYPE_A,
		// Token: 0x04003428 RID: 13352
		TYPE_B
	}

	// Token: 0x020007B2 RID: 1970
	public class UpdateStringResult
	{
		// Token: 0x04003429 RID: 13353
		public bool isSuccess;
	}
}
