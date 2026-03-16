using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

public class DataManagerUserInfo
{
	public int getStaminaRecoveryTimeSecond(long baseDateTicks)
	{
		if (DataManager.DmCampaign.getBaseDateCampaignStaminaRecoveryData(baseDateTicks) != null)
		{
			return DataManager.DmCampaign.getBaseDateCampaignStaminaRecoveryData(baseDateTicks).staminaRecoveryTime;
		}
		return DataManager.DmServerMst.MstAppConfig.staminaRecoveryTime;
	}

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

	public DataManagerUserInfo(DataManager p)
	{
		this.parentData = p;
	}

	public string userName { get; private set; }

	public int level { get; private set; }

	public long exp { get; private set; }

	public int friendId { get; private set; }

	public StaminaInfo staminaInfo { get; private set; }

	public string userComment { get; private set; }

	public DataManagerUserInfo.AvatarType avatarType { get; private set; }

	public int favoriteCharaId { get; private set; }

	public List<LoanPackData> loanPackList { get; private set; }

	public TutorialUtil.Sequence tutorialSequence { get; private set; }

	public string playedLoginScenarioList { get; private set; }

	public string playedIntroductionList { get; private set; }

	public UserOptionData optionData { get; private set; }

	public long expOverflow { get; private set; }

	public int dispKizunaConfirm { get; private set; }

	public bool dispPracticeConfirm { get; private set; }

	public long GetExpByNextLevel(int nowLevel)
	{
		int num = nowLevel - 1 + 1;
		if (num < DataManager.DmServerMst.gameLevelInfoList.Count)
		{
			return DataManager.DmServerMst.gameLevelInfoList[num].userLevelExp;
		}
		return 0L;
	}

	public DataManagerUserInfo.UpdateStringResult GetUpdateNameResult()
	{
		return this.updateStringResult;
	}

	public DataManagerUserInfo.UpdateStringResult GetUpdateCommentResult()
	{
		return this.updateStringResult;
	}

	public string RequestActionUpdateUserName(string _userName)
	{
		_userName = _userName.Replace("<", "＜");
		_userName = _userName.Replace(">", "＞");
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.player_name = _userName;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
		return _userName;
	}

	public void RequestActionUpdateUserAvatar(DataManagerUserInfo.AvatarType type)
	{
		if (this.avatarType != type)
		{
			PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
			playerInfo.player_type = (int)type;
			this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
		}
	}

	public string RequestActionUpdateUserComment(string _userComment)
	{
		_userComment = _userComment.Replace("<", "＜");
		_userComment = _userComment.Replace(">", "＞");
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.comment = _userComment;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
		return _userComment;
	}

	public void RequestActionUpdateFavoriteChara(int charaId)
	{
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.favorite_chara_id = charaId;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
	}

	public void RequestActionUpdateScenarioLastId(int id, string json)
	{
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.played_login_scenario_list = json;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
	}

	public void RequestActionUpdateIntroductionLastId(int id, string json)
	{
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.played_introduction_list = json;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
	}

	public void RequestActionUpdateTutorialStep(int tutorialStep)
	{
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.tutorial_step = tutorialStep;
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
	}

	public void RequestActionSkipTutorial()
	{
		this.parentData.ServerRequest(TutorialSkipCmd.Create(), null);
	}

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

	public void RequestGetLoanPackList()
	{
		this.parentData.ServerRequest(MyHelperListCmd.Create(), new Action<Command>(this.CbMyHelperListCmd));
	}

	public void RequestGetUserOption()
	{
		this.parentData.ServerRequest(OptionGetCmd.Create(), new Action<Command>(this.CbOptionGetCmd));
	}

	public void RequestActionUpdateUserOption(UserOptionData uod)
	{
		List<int> list = uod.CreateByServerData();
		if (!this.optionData.CreateByServerData().SequenceEqual<int>(list))
		{
			this.parentData.ServerRequest(OptionSetCmd.Create(list), new Action<Command>(this.CbOptionSetCmd));
		}
	}

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

	public void RequestActionRecoveryStamina(int useItemId, int useItemNum)
	{
		this.parentData.ServerRequest(RecoveryCmd.Create(useItemId, useItemNum, 1), new Action<Command>(this.CbRecoveryCmd));
	}

	public void RequestActionChangeTime(string dateTime, bool isBackTitle = true)
	{
		this.parentData.ServerRequest(TimeChangeCmd.Create(dateTime), isBackTitle ? new Action<Command>(this.CbChangeTimeCmd) : new Action<Command>(this.CbChangeTimeCmdWithoutTitle));
	}

	public void RequestActionPlayerLevelUp(long userExpOverflow)
	{
		this.parentData.ServerRequest(PlayerLevelUpCmd.Create(userExpOverflow), new Action<Command>(this.CbLevelUpCmd));
	}

	public void RequestActionUpdateKizunaConfirm(int confirm)
	{
		this.parentData.ServerRequest(KizunaConfirmUpdateCmd.Create(confirm), new Action<Command>(this.CbKizunaConfirmUpdateCmd));
	}

	public void RequestActionUpdatePracticeConfirm(int confirm)
	{
		this.parentData.ServerRequest(PracticeConfirmUpdateCmd.Create(confirm), new Action<Command>(this.CbPracticeConfirmUpdateCmd));
	}

	private void CbChangeTimeCmd(Command cmd)
	{
		DataInitializeResolver.InitializeActionDataManager();
		Singleton<SceneManager>.Instance.SetSceneReboot();
	}

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

	private void CbPlayerInfoChangeCmd(Command cmd)
	{
		PlayerInfoChangeResponse playerInfoChangeResponse = cmd.response as PlayerInfoChangeResponse;
		this.updateStringResult = new DataManagerUserInfo.UpdateStringResult();
		this.updateStringResult.isSuccess = playerInfoChangeResponse.result_name == 1 && playerInfoChangeResponse.result_comment == 1;
		this.parentData.UpdateUserAssetByAssets(playerInfoChangeResponse.assets);
	}

	private void CbMyHelperChangeCmd(Command cmd)
	{
		MyHelperChangeRequest myHelperChangeRequest = cmd.request as MyHelperChangeRequest;
		MyHelperChangeResponse myHelperChangeResponse = cmd.response as MyHelperChangeResponse;
		this.UpdateLoanPackListByServerData(myHelperChangeRequest.helperList);
		this.parentData.UpdateUserAssetByAssets(myHelperChangeResponse.assets);
	}

	private void CbOptionGetCmd(Command cmd)
	{
		OptionGetResponse optionGetResponse = cmd.response as OptionGetResponse;
		this.UpdateUserOptionByServer(optionGetResponse.optionList);
		Singleton<CanvasManager>.Instance.SetDisplayDirection(this.optionData.DisplayDirection);
		this.optionData.SetDisplayQuality();
		this.optionData.SetFrameRate();
	}

	private void CbOptionSetCmd(Command cmd)
	{
		OptionSetRequest optionSetRequest = cmd.request as OptionSetRequest;
		this.UpdateUserOptionByServer(optionSetRequest.optionList);
	}

	private void CbRecoveryCmd(Command cmd)
	{
		RecoveryResponse recoveryResponse = cmd.response as RecoveryResponse;
		this.parentData.UpdateUserAssetByAssets(recoveryResponse.assets);
	}

	private void CbLevelUpCmd(Command cmd)
	{
		PlayerLevelUpResponse playerLevelUpResponse = cmd.response as PlayerLevelUpResponse;
		this.parentData.UpdateUserAssetByAssets(playerLevelUpResponse.assets);
		DataManager.DmUserInfo.UpdateUserExpOverflowByServer(playerLevelUpResponse.assets.exp_overflow);
	}

	private void CbKizunaConfirmUpdateCmd(Command cmd)
	{
		KizunaConfirmUpdateCmdResponse kizunaConfirmUpdateCmdResponse = cmd.response as KizunaConfirmUpdateCmdResponse;
		DataManager.DmUserInfo.UpdateUserKizunaConfirmByServer(kizunaConfirmUpdateCmdResponse.assets.kizunaConfirm);
	}

	private void CbPracticeConfirmUpdateCmd(Command cmd)
	{
		PracticeConfirmUpdateCmdResponse practiceConfirmUpdateCmdResponse = cmd.response as PracticeConfirmUpdateCmdResponse;
		DataManager.DmUserInfo.UpdateUserPracticeConfirmByServer(practiceConfirmUpdateCmdResponse.assets.practiceConfirm);
	}

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

	public void UpdateUserOptionByDebug()
	{
		if (this.optionData == null)
		{
			this.optionData = new UserOptionData();
		}
		this.optionData.UpdateByServerData(null);
		this.userName = "【しんまい】";
	}

	public void UpdateUserExpOverflowByServer(long exp)
	{
		this.expOverflow = exp;
	}

	public void UpdateUserKizunaConfirmByServer(int confirm)
	{
		this.dispKizunaConfirm = confirm;
	}

	public void UpdateUserPracticeConfirmByServer(int confirm)
	{
		this.dispPracticeConfirm = confirm == 1;
	}

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

	private DataManager parentData;

	private DataManagerUserInfo.UpdateStringResult updateStringResult;

	public enum AvatarType
	{
		INVALID,
		TYPE_A,
		TYPE_B
	}

	public class UpdateStringResult
	{
		public bool isSuccess;
	}
}
