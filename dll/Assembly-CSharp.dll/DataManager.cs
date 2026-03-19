using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : Singleton<DataManager>
{
	public static DataManagerUserInfo DmUserInfo
	{
		get
		{
			return Singleton<DataManager>.Instance.dmUserInfo;
		}
	}

	public static DataManagerMasterSkill DmMasterSkill
	{
		get
		{
			return Singleton<DataManager>.Instance.dmMasterSkill;
		}
	}

	public static DataManagerGameStatus DmGameStatus
	{
		get
		{
			return Singleton<DataManager>.Instance.dmGameStatus;
		}
	}

	public static DataManagerMonthlyPack DmMonthlyPack
	{
		get
		{
			return Singleton<DataManager>.Instance.dmMonthlyPack;
		}
	}

	public static DataManagerItem DmItem
	{
		get
		{
			return Singleton<DataManager>.Instance.dmItem;
		}
	}

	public static DataManagerChara DmChara
	{
		get
		{
			if (!(Singleton<DataManager>.Instance == null))
			{
				return Singleton<DataManager>.Instance.dmChara;
			}
			return null;
		}
	}

	public static DataManagerGacha DmGacha
	{
		get
		{
			return Singleton<DataManager>.Instance.dmGacha;
		}
	}

	public static DataManagerDeck DmDeck
	{
		get
		{
			return Singleton<DataManager>.Instance.dmDeck;
		}
	}

	public static DataManagerPhoto DmPhoto
	{
		get
		{
			return Singleton<DataManager>.Instance.dmPhoto;
		}
	}

	public static DataManagerCharaAccessory DmChAccessory
	{
		get
		{
			return Singleton<DataManager>.Instance.dmChAcc;
		}
	}

	public static DataManagerQuest DmQuest
	{
		get
		{
			return Singleton<DataManager>.Instance.dmQuest;
		}
	}

	public static DataManagerHelper DmHelper
	{
		get
		{
			return Singleton<DataManager>.Instance.dmHelper;
		}
	}

	public static DataManagerPresent DmPresent
	{
		get
		{
			return Singleton<DataManager>.Instance.dmPresent;
		}
	}

	public static DataManagerEvent DmEvent
	{
		get
		{
			return Singleton<DataManager>.Instance.dmEvent;
		}
	}

	public static DataManagerCampaign DmCampaign
	{
		get
		{
			return Singleton<DataManager>.Instance.dmCampaign;
		}
	}

	public static DataManagerPicnic DmPicnic
	{
		get
		{
			return Singleton<DataManager>.Instance.dmPicnic;
		}
	}

	public static DataManagerMission DmMission
	{
		get
		{
			return Singleton<DataManager>.Instance.dmMission;
		}
	}

	public static DataManagerCharaMission DmChMission
	{
		get
		{
			return Singleton<DataManager>.Instance.dmChMission;
		}
	}

	public static DataManagerPurchase DmPurchase
	{
		get
		{
			return Singleton<DataManager>.Instance.dmPurchase;
		}
	}

	public static DataManagerShop DmShop
	{
		get
		{
			return Singleton<DataManager>.Instance.dmShop;
		}
	}

	public static DataManagerHome DmHome
	{
		get
		{
			return Singleton<DataManager>.Instance.dmHome;
		}
	}

	public static DataManagerPvp DmPvp
	{
		get
		{
			return Singleton<DataManager>.Instance.dmPvp;
		}
	}

	public static DataManagerTraining DmTraining
	{
		get
		{
			return Singleton<DataManager>.Instance.dmTraining;
		}
	}

	public static DataManagerKemoBoard DmKemoBoard
	{
		get
		{
			return Singleton<DataManager>.Instance.dmKemoBoard;
		}
	}

	public static DataManagerTreeHouse DmTreeHouse
	{
		get
		{
			return Singleton<DataManager>.Instance.dmTreeHouse;
		}
	}

	public static DataManagerServerMst DmServerMst
	{
		get
		{
			return Singleton<DataManager>.Instance.dmServerMst;
		}
	}

	public static DataManagerScenario DmScenario
	{
		get
		{
			return Singleton<DataManager>.Instance.dmScenario;
		}
	}

	public static DataManagerAchievement DmAchievement
	{
		get
		{
			return Singleton<DataManager>.Instance.dmAchievement;
		}
	}

	public static DataManagerSticker DmSticker
	{
		get
		{
			return Singleton<DataManager>.Instance.dmSticker;
		}
	}

	public static DataManagerIntroductionNewChara DmIntroduction
	{
		get
		{
			return Singleton<DataManager>.Instance.dmIntroduction;
		}
	}

	public static DataManagerBookmark DmBookmark
	{
		get
		{
			return Singleton<DataManager>.Instance.dmBookmark;
		}
	}

	public static DataManagerAssistant DmAssistant
	{
		get
		{
			return Singleton<DataManager>.Instance.dmAssistant;
		}
	}

	public bool lockByDebug { get; set; }

	public bool lockByPurchase { get; set; }

	public bool DisableServerRequestByTutorial { get; set; }

	public bool DisableServerRequestByDebug { get; set; }

	public bool IsSetupData { get; private set; }

	public static bool IsServerRequesting()
	{
		return Singleton<DataManager>.Instance.lockByServerError || Singleton<DataManager>.Instance.lockByPurchase || Singleton<DataManager>.Instance.lockByDebug || (Singleton<Manager>.Instance != null && Manager.IsCmdProcessing());
	}

	public static ActionTypeMask GetServerErrorType()
	{
		return Singleton<DataManager>.Instance.actionType;
	}

	public static bool IsFinishCreateByMst()
	{
		return Singleton<DataManager>.Instance.dmItem != null && Singleton<DataManager>.Instance.dmItem.GetItemStaticMap().Count > 0;
	}

	public void Initialize()
	{
		this.lockByServerError = false;
		this.lockByPurchase = false;
		this.lockByDebug = false;
		this.IsSetupData = false;
		this.actionType = ActionTypeMask.INVALID;
		if (this.dmChara != null)
		{
			this.dmChara.Destory();
		}
		if (this.dmQuest != null)
		{
			this.dmQuest.Destory();
		}
		this.dmUserInfo = new DataManagerUserInfo(this);
		this.dmMasterSkill = new DataManagerMasterSkill(this);
		this.dmGameStatus = new DataManagerGameStatus(this);
		this.dmMonthlyPack = new DataManagerMonthlyPack(this);
		this.dmItem = new DataManagerItem(this);
		this.dmChara = new DataManagerChara(this);
		this.dmGacha = new DataManagerGacha(this);
		this.dmDeck = new DataManagerDeck(this);
		this.dmPhoto = new DataManagerPhoto(this);
		this.dmChAcc = new DataManagerCharaAccessory(this);
		this.dmQuest = new DataManagerQuest(this);
		this.dmHelper = new DataManagerHelper(this);
		this.dmServerMst = new DataManagerServerMst(this);
		this.dmPresent = new DataManagerPresent(this);
		this.dmEvent = new DataManagerEvent(this);
		this.dmCampaign = new DataManagerCampaign(this);
		this.dmPicnic = new DataManagerPicnic(this);
		this.dmMission = new DataManagerMission(this);
		this.dmChMission = new DataManagerCharaMission(this);
		this.dmPurchase = new DataManagerPurchase(this);
		this.dmShop = new DataManagerShop(this);
		this.dmHome = new DataManagerHome(this);
		this.dmPvp = new DataManagerPvp(this);
		this.dmTraining = new DataManagerTraining(this);
		this.dmKemoBoard = new DataManagerKemoBoard(this);
		this.dmTreeHouse = new DataManagerTreeHouse(this);
		this.dmScenario = new DataManagerScenario(this);
		this.dmAchievement = new DataManagerAchievement(this);
		this.dmSticker = new DataManagerSticker(this);
		this.dmIntroduction = new DataManagerIntroductionNewChara(this);
		this.dmBookmark = new DataManagerBookmark(this);
		this.dmAssistant = new DataManagerAssistant(this);
	}

	private void Update()
	{
		if (Singleton<CanvasManager>.Instance != null && CanvasManager.HdlServerConnectObj != null && CanvasManager.HdlServerConnectObj.activeSelf != DataManager.IsServerRequesting())
		{
			if (DataManager.IsServerRequesting())
			{
				CanvasManager.HdlServerConnectObj.transform.Find("SafeArea/Cmn_Connect").GetComponent<PguiAECtrl>().m_AEImage.playInTime = 0f;
			}
			CanvasManager.HdlServerConnectObj.SetActive(DataManager.IsServerRequesting());
		}
		if (this.dmPurchase != null)
		{
			this.dmPurchase.UpdateByDataManager();
		}
		this.UpdateByEditor();
	}

	public void InitializeByEditor(UnityAction finishCallBack)
	{
	}

	public void UpdateByEditor()
	{
	}

	public void UpdateUserAssetByLogin(LoginDmmResponse res)
	{
		this.dmChara.UpdateUserDataByServer(res.charas);
		this.dmDeck.UpdateUserDataByServer(res.decks);
		DataManager.DmKemoBoard.UpdateUserDataByServer(res.kemoboard_panels);
		this.dmItem.UpdateUserDataByServer(res.items, res.charas, res.achievements, res.stickers);
		this.dmItem.UpdateUserDataByServer(res.item_banks);
		this.dmChara.UpdateUserCharasClothesData();
		this.dmChara.UpdateUserCharasContactData();
		this.dmChara.UpdateSumUserAllCharaKemoStatus(null);
		this.dmHome.UpdateUserDataByServer(res.items, res.furnitures, res.rewardinfoList);
		this.dmChara.UpdateUserDataByServer(res.items);
		this.dmMasterSkill.UpdateUserDataByServer(res.master_skills);
		this.dmPhoto.UpdateUserDataByServer(res.photos);
		this.dmChAcc.UpdateUserDataByServer(res.accessories);
		this.dmChAcc.UpdateFriendsAccessoryDataByServer(res.charas, res.accessories);
		this.dmUserInfo.UpdateUserDataByServer(res.assets.player_info);
		this.dmMonthlyPack.UpdateUserDataByServer(res.assets.player_info);
		this.dmPurchase.UpdateUserDataByServer(res.assets.player_info);
		this.dmTreeHouse.UpdateUserDataByServer(res.items);
		this.dmPvp.UpdateReleasePvpSpecialEffectList(res.pvpspecialReleaseIdList, true, false);
		this.dmScenario.UpdateUserDataByServer(res.assets.player_info);
		this.dmAchievement.UpdateUserDataByServer(res.achievements);
		this.dmSticker.UpdateUserDataByServer(res.stickers);
		this.dmIntroduction.UpdateUserDataByServer(res.assets.player_info);
		this.dmUserInfo.UpdateUserExpOverflowByServer(res.assets.exp_overflow);
		this.dmUserInfo.UpdateUserKizunaConfirmByServer(res.assets.kizunaConfirm);
		this.dmUserInfo.UpdateUserPracticeConfirmByServer(res.assets.practiceConfirm);
		this.dmChara.UpdateKizunaQualifiedByServer(res.assets.qualified);
		this.dmAssistant.UpdateAssistantDataByServer(res.assets.assistant_data);
		if (res.assets.update_mission_list != null)
		{
			this.dmMission.AddWaitDisplayMission(res.assets.update_mission_list);
		}
		this.IsSetupData = true;
	}

	public void UpdateUserAssetByAssets(Assets assets)
	{
		if (assets == null)
		{
			return;
		}
		bool flag = false;
		List<int> list = null;
		bool flag2 = false;
		if (assets.update_chara_list != null)
		{
			this.dmChara.UpdateUserDataByServer(assets.update_chara_list);
			flag = true;
			flag2 = true;
			list = assets.update_chara_list.ConvertAll<int>((Chara item) => item.chara_id);
		}
		if (assets.update_item_list != null)
		{
			this.dmItem.UpdateUserDataByServer(assets.update_item_list, assets.update_chara_list, assets.update_achievement_list, assets.update_sticker_list);
			this.dmHome.UpdateUserDataByServer(assets.update_item_list);
			this.dmChara.UpdateUserDataByServer(assets.update_item_list);
			if (assets.update_item_list.Exists((Item x) => ItemDef.Id2Kind(x.item_id) == ItemDef.Kind.CLOTHES))
			{
				flag2 = true;
				flag = true;
			}
			this.dmChara.UpdateUserCharasContactData();
			this.dmTreeHouse.UpdateUserDataByServer(assets.update_item_list);
		}
		if (flag2)
		{
			this.dmChara.UpdateUserCharasClothesData();
		}
		if (flag)
		{
			this.dmChara.UpdateSumUserAllCharaKemoStatus(list);
		}
		if (assets.update_photo_list != null)
		{
			this.dmPhoto.UpdateUserDataByServer(assets.update_photo_list);
		}
		if (assets.update_accessory_list != null)
		{
			this.dmChAcc.UpdateUserDataByServer(assets.update_accessory_list);
			this.dmChAcc.UpdateFriendsAccessoryDataByServer(assets.update_chara_list, assets.update_accessory_list);
		}
		if (assets.player_info != null)
		{
			this.dmUserInfo.UpdateUserDataByServer(assets.player_info);
			this.dmMonthlyPack.UpdateUserDataByServer(assets.player_info);
			this.dmPurchase.UpdateUserDataByServer(assets.player_info);
			this.dmScenario.UpdateUserDataByServer(assets.player_info);
			this.dmIntroduction.UpdateUserDataByServer(assets.player_info);
		}
		if (assets.update_master_skill_list != null)
		{
			DataManager.DmMasterSkill.UpdateUserDataByServer(assets.update_master_skill_list);
		}
		if (assets.update_mission_list != null)
		{
			this.dmMission.UpdateUserDataByServer(assets.update_mission_list);
			this.dmChMission.UpdateUserDataByServer(assets.update_mission_list);
		}
		if (assets.present_num != 0)
		{
			HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
			if (homeCheckResult != null)
			{
				homeCheckResult.presentBoxNum = assets.present_num;
			}
		}
		if (assets.update_achievement_list != null)
		{
			this.dmAchievement.UpdateUserDataByServer(assets.update_achievement_list);
		}
		if (assets.update_item_bank_list != null)
		{
			this.dmItem.UpdateUserDataByServer(assets.update_item_bank_list);
		}
		if (assets.qualified != null)
		{
			this.dmChara.UpdateKizunaQualifiedByServer(assets.qualified);
		}
		if (assets.assistant_data != null)
		{
			this.dmAssistant.UpdateAssistantDataByServer(assets.assistant_data);
		}
		if (assets.update_sealed_data != null)
		{
			this.dmQuest.UpdateQuestCharaSealedByAssets(assets.update_sealed_data);
		}
		if (assets.update_sticker_list != null)
		{
			this.dmSticker.UpdateUserDataByServer(assets.update_sticker_list);
		}
	}

	public void UpdateSumUserAllCharaKemoStatus(List<int> updateKemoStatusCharaIdList)
	{
		this.dmChara.UpdateSumUserAllCharaKemoStatus(updateKemoStatusCharaIdList);
	}

	public void UpdateUserOption(List<int> optionList)
	{
		this.dmUserInfo.UpdateUserOptionByServer(optionList);
	}

	public void InsertNewList(List<NewFlg> newFlagList)
	{
		newFlagList.Sort((NewFlg a, NewFlg b) => a.any_id - b.any_id);
		PrjUtil.InsertionSort<NewFlg>(ref newFlagList, (NewFlg a, NewFlg b) => a.category - b.category);
		this.dmItem.InsertNewList(newFlagList);
		this.dmQuest.InsertNewList(newFlagList);
		this.dmGameStatus.UpdateUserFlagByServer(newFlagList);
		this.dmEvent.UpdateUserFlagByServer(newFlagList);
		this.dmPicnic.UpdateUserFlagByServer(newFlagList);
		this.dmShop.InsertNewList(newFlagList);
		this.dmPurchase.UpdateUserFlagByServer(newFlagList);
		this.dmTreeHouse.InsertNewList(newFlagList);
	}

	public PlayerInfo MakeProtocolByPlayerInfo()
	{
		return new PlayerInfo
		{
			player_name = this.dmUserInfo.userName,
			player_type = (int)this.dmUserInfo.avatarType,
			comment = this.dmUserInfo.userComment,
			favorite_chara_id = this.dmUserInfo.favoriteCharaId,
			tutorial_step = (int)this.dmUserInfo.tutorialSequence,
			birthday = ((this.dmPurchase.userBirthDay != null) ? PrjUtil.ConvertTicksToTime(this.dmPurchase.userBirthDay.Value.Ticks) : 0L),
			played_login_scenario_list = this.dmUserInfo.playedLoginScenarioList,
			played_introduction_list = this.dmUserInfo.playedIntroductionList
		};
	}

	public void CbServerError(Command cmd)
	{
		this.lockByServerError = true;
		CanvasManager.HdlWebViewWindowCtrl.Close();
		ErrorCode error = cmd.response.error_code;
		string text = string.Format("{0}\n\nCODE:{1}", error.msg, error.id);
		if (error.typ == 4)
		{
			CanvasManager.HdlOpenWindowServerError.Setup(error.tit, text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, delegate(int index)
			{
				if (Singleton<SceneManager>.Instance.CurrentSceneName == SceneManager.SceneName.SceneBattleSelector || Singleton<SceneManager>.Instance.CurrentSceneName == SceneManager.SceneName.SceneBattleResult)
				{
					Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneQuest, null);
				}
				else
				{
					Singleton<SceneManager>.Instance.SetNextScene(Singleton<SceneManager>.Instance.CurrentSceneName, null);
				}
				this.lockByServerError = false;
				this.actionType = (ActionTypeMask)error.typ;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowServerError.ForceOpen();
			return;
		}
		if (error.checkType(ActionTypeMask.MAINTE_WEB))
		{
			CanvasManager.HdlOpenWindowServerError.Setup(error.tit, text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.TITLE_MENT), false, delegate(int index)
			{
				if (index == 0)
				{
					CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
					CanvasManager.SetEnableCmnTouchMask(false);
					CanvasManager.HdlMissionProgressCtrl.ClaerProgress();
					Singleton<SceneManager>.Instance.SetSceneReboot();
					return true;
				}
				Application.OpenURL("https://kemono-friends-3.jp/");
				return false;
			}, null, false);
			CanvasManager.HdlOpenWindowServerError.ForceOpen();
			return;
		}
		if (error.checkType(ActionTypeMask.SHUTDOWN))
		{
			CanvasManager.HdlOpenWindowServerError.Setup(error.tit, text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, delegate(int index)
			{
				PrjUtil.ForceShutdown();
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowServerError.ForceOpen();
			return;
		}
		if (error.checkType(ActionTypeMask.RETRY))
		{
			this.CbServerRetry(cmd, error);
			return;
		}
		CanvasManager.HdlOpenWindowServerError.Setup(error.tit, text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, delegate(int index)
		{
			CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
			CanvasManager.SetEnableCmnTouchMask(false);
			CanvasManager.HdlMissionProgressCtrl.ClaerProgress();
			Singleton<SceneManager>.Instance.SetSceneReboot();
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowServerError.ForceOpen();
	}

	public void CbServerRetry(Command cmd, ErrorCode error)
	{
		this.lockByServerError = true;
		CanvasManager.HdlWebViewWindowCtrl.Close();
		this.retryCmdList.Add(cmd);
		string text = string.Format("{0}\n\nCODE:{1}", error.msg, error.id);
		CanvasManager.HdlOpenWindowServerError.Setup(error.tit, text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), false, new PguiOpenWindowCtrl.Callback(this.OnClickServerRetryWindow), null, false);
		CanvasManager.HdlOpenWindowServerError.ForceOpen();
	}

	private bool OnClickServerRetryWindow(int index)
	{
		if (index == 0)
		{
			CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
			CanvasManager.SetEnableCmnTouchMask(false);
			foreach (Command command in this.retryCmdList)
			{
				command.Exit();
			}
			Singleton<SceneManager>.Instance.SetSceneReboot();
		}
		else if (index == 1)
		{
			foreach (Command command2 in this.retryCmdList)
			{
				command2.Retry();
			}
			this.lockByServerError = false;
		}
		return true;
	}

	public void ServerRequest(Command cmd, Action<Command> cb)
	{
		if (this.DisableServerRequestByTutorial)
		{
			return;
		}
		this.actionType = ActionTypeMask.INVALID;
		Command.SetupRequest(cmd, cb, new Action<Command>(this.CbServerError), new Action<Command, ErrorCode>(this.CbServerRetry));
	}

	public IEnumerator InitializeMstData()
	{
		DataManager.DmItem.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmServerMst.InitializeMstData(Singleton<MstManager>.Instance);
		IEnumerator IEchara = DataManager.DmChara.InitializeMstData(Singleton<MstManager>.Instance);
		while (IEchara.MoveNext())
		{
			yield return null;
		}
		DataManager.DmPhoto.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmChAccessory.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmAchievement.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmSticker.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmGacha.InitializeMstData(Singleton<MstManager>.Instance);
		IEnumerator IEquest = DataManager.DmQuest.InitializeMstData(Singleton<MstManager>.Instance, DataManager.DmChara.GetEnemyStaticMap());
		while (IEquest.MoveNext())
		{
			yield return null;
		}
		DataManager.DmTreeHouse.InitializeMstData(Singleton<MstManager>.Instance);
		IEnumerator IEfurniture = DataManager.DmTreeHouse.InitializeSmallFurnitureData();
		while (IEfurniture.MoveNext())
		{
			yield return null;
		}
		DataManager.DmShop.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmPurchase.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmMonthlyPack.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmEvent.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmCampaign.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmPicnic.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmHome.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmMission.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmChMission.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmPvp.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmTraining.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmKemoBoard.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmItem.InitializeMstDataByItemEndTime(Singleton<MstManager>.Instance);
		DataManager.DmMasterSkill.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmQuest.SetMstDataByEvent(this.dmEvent);
		DataManager.DmScenario.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmIntroduction.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmAssistant.InitializeMstData(Singleton<MstManager>.Instance);
		yield break;
	}

	public IEnumerator ReInitializeMstData()
	{
		if (this.dmChara != null)
		{
			this.dmChara.Destory();
		}
		if (this.dmQuest != null)
		{
			this.dmQuest.Destory();
		}
		this.dmItem = new DataManagerItem(this);
		this.dmChara = new DataManagerChara(this);
		this.dmPhoto = new DataManagerPhoto(this);
		this.dmQuest = new DataManagerQuest(this);
		DataManager.DmItem.InitializeMstData(Singleton<MstManager>.Instance);
		IEnumerator ienum = DataManager.DmChara.InitializeMstData(Singleton<MstManager>.Instance);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		DataManager.DmPhoto.InitializeMstData(Singleton<MstManager>.Instance);
		DataManager.DmChAccessory.InitializeMstData(Singleton<MstManager>.Instance);
		ienum = DataManager.DmQuest.InitializeMstData(Singleton<MstManager>.Instance, DataManager.DmChara.GetEnemyStaticMap());
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		yield break;
	}

	private DataManagerUserInfo dmUserInfo;

	private DataManagerMasterSkill dmMasterSkill;

	private DataManagerGameStatus dmGameStatus;

	private DataManagerMonthlyPack dmMonthlyPack;

	private DataManagerItem dmItem;

	private DataManagerChara dmChara;

	private DataManagerGacha dmGacha;

	private DataManagerDeck dmDeck;

	private DataManagerPhoto dmPhoto;

	private DataManagerCharaAccessory dmChAcc;

	private DataManagerQuest dmQuest;

	private DataManagerHelper dmHelper;

	private DataManagerPresent dmPresent;

	private DataManagerEvent dmEvent;

	private DataManagerCampaign dmCampaign;

	private DataManagerPicnic dmPicnic;

	private DataManagerMission dmMission;

	private DataManagerCharaMission dmChMission;

	private DataManagerPurchase dmPurchase;

	private DataManagerShop dmShop;

	private DataManagerHome dmHome;

	private DataManagerPvp dmPvp;

	private DataManagerTraining dmTraining;

	private DataManagerKemoBoard dmKemoBoard;

	private DataManagerTreeHouse dmTreeHouse;

	private DataManagerServerMst dmServerMst;

	private DataManagerScenario dmScenario;

	private DataManagerAchievement dmAchievement;

	private DataManagerSticker dmSticker;

	private DataManagerIntroductionNewChara dmIntroduction;

	private DataManagerBookmark dmBookmark;

	private DataManagerAssistant dmAssistant;

	private bool lockByServerError;

	private ActionTypeMask actionType;

	private List<Command> retryCmdList = new List<Command>();

	public enum NewFlgCategory
	{
		INVALID,
		ITEM,
		QUEST,
		COMMON,
		SORTTYPE,
		PICNIC,
		RELEASEMODE,
		EVENT1,
		EVENT2,
		GACHANEWINFO,
		CHARAGROWTUTORIAL,
		SHOP_GOODS,
		PURCHASE_INFO,
		TREEHOUSE_FURNITURE_FAVORITE,
		IconSizeIndex
	}
}
