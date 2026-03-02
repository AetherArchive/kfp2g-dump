using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200006C RID: 108
public class DataManager : Singleton<DataManager>
{
	// Token: 0x17000081 RID: 129
	// (get) Token: 0x060002C5 RID: 709 RVA: 0x000167E4 File Offset: 0x000149E4
	public static DataManagerUserInfo DmUserInfo
	{
		get
		{
			return Singleton<DataManager>.Instance.dmUserInfo;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x060002C6 RID: 710 RVA: 0x000167F0 File Offset: 0x000149F0
	public static DataManagerMasterSkill DmMasterSkill
	{
		get
		{
			return Singleton<DataManager>.Instance.dmMasterSkill;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x060002C7 RID: 711 RVA: 0x000167FC File Offset: 0x000149FC
	public static DataManagerGameStatus DmGameStatus
	{
		get
		{
			return Singleton<DataManager>.Instance.dmGameStatus;
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x060002C8 RID: 712 RVA: 0x00016808 File Offset: 0x00014A08
	public static DataManagerMonthlyPack DmMonthlyPack
	{
		get
		{
			return Singleton<DataManager>.Instance.dmMonthlyPack;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x060002C9 RID: 713 RVA: 0x00016814 File Offset: 0x00014A14
	public static DataManagerItem DmItem
	{
		get
		{
			return Singleton<DataManager>.Instance.dmItem;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060002CA RID: 714 RVA: 0x00016820 File Offset: 0x00014A20
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

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060002CB RID: 715 RVA: 0x0001683B File Offset: 0x00014A3B
	public static DataManagerGacha DmGacha
	{
		get
		{
			return Singleton<DataManager>.Instance.dmGacha;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060002CC RID: 716 RVA: 0x00016847 File Offset: 0x00014A47
	public static DataManagerDeck DmDeck
	{
		get
		{
			return Singleton<DataManager>.Instance.dmDeck;
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060002CD RID: 717 RVA: 0x00016853 File Offset: 0x00014A53
	public static DataManagerPhoto DmPhoto
	{
		get
		{
			return Singleton<DataManager>.Instance.dmPhoto;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060002CE RID: 718 RVA: 0x0001685F File Offset: 0x00014A5F
	public static DataManagerCharaAccessory DmChAccessory
	{
		get
		{
			return Singleton<DataManager>.Instance.dmChAcc;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060002CF RID: 719 RVA: 0x0001686B File Offset: 0x00014A6B
	public static DataManagerQuest DmQuest
	{
		get
		{
			return Singleton<DataManager>.Instance.dmQuest;
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060002D0 RID: 720 RVA: 0x00016877 File Offset: 0x00014A77
	public static DataManagerHelper DmHelper
	{
		get
		{
			return Singleton<DataManager>.Instance.dmHelper;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060002D1 RID: 721 RVA: 0x00016883 File Offset: 0x00014A83
	public static DataManagerPresent DmPresent
	{
		get
		{
			return Singleton<DataManager>.Instance.dmPresent;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060002D2 RID: 722 RVA: 0x0001688F File Offset: 0x00014A8F
	public static DataManagerEvent DmEvent
	{
		get
		{
			return Singleton<DataManager>.Instance.dmEvent;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060002D3 RID: 723 RVA: 0x0001689B File Offset: 0x00014A9B
	public static DataManagerCampaign DmCampaign
	{
		get
		{
			return Singleton<DataManager>.Instance.dmCampaign;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060002D4 RID: 724 RVA: 0x000168A7 File Offset: 0x00014AA7
	public static DataManagerPicnic DmPicnic
	{
		get
		{
			return Singleton<DataManager>.Instance.dmPicnic;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060002D5 RID: 725 RVA: 0x000168B3 File Offset: 0x00014AB3
	public static DataManagerMission DmMission
	{
		get
		{
			return Singleton<DataManager>.Instance.dmMission;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060002D6 RID: 726 RVA: 0x000168BF File Offset: 0x00014ABF
	public static DataManagerCharaMission DmChMission
	{
		get
		{
			return Singleton<DataManager>.Instance.dmChMission;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060002D7 RID: 727 RVA: 0x000168CB File Offset: 0x00014ACB
	public static DataManagerPurchase DmPurchase
	{
		get
		{
			return Singleton<DataManager>.Instance.dmPurchase;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060002D8 RID: 728 RVA: 0x000168D7 File Offset: 0x00014AD7
	public static DataManagerShop DmShop
	{
		get
		{
			return Singleton<DataManager>.Instance.dmShop;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060002D9 RID: 729 RVA: 0x000168E3 File Offset: 0x00014AE3
	public static DataManagerHome DmHome
	{
		get
		{
			return Singleton<DataManager>.Instance.dmHome;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060002DA RID: 730 RVA: 0x000168EF File Offset: 0x00014AEF
	public static DataManagerPvp DmPvp
	{
		get
		{
			return Singleton<DataManager>.Instance.dmPvp;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060002DB RID: 731 RVA: 0x000168FB File Offset: 0x00014AFB
	public static DataManagerTraining DmTraining
	{
		get
		{
			return Singleton<DataManager>.Instance.dmTraining;
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060002DC RID: 732 RVA: 0x00016907 File Offset: 0x00014B07
	public static DataManagerKemoBoard DmKemoBoard
	{
		get
		{
			return Singleton<DataManager>.Instance.dmKemoBoard;
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060002DD RID: 733 RVA: 0x00016913 File Offset: 0x00014B13
	public static DataManagerTreeHouse DmTreeHouse
	{
		get
		{
			return Singleton<DataManager>.Instance.dmTreeHouse;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060002DE RID: 734 RVA: 0x0001691F File Offset: 0x00014B1F
	public static DataManagerServerMst DmServerMst
	{
		get
		{
			return Singleton<DataManager>.Instance.dmServerMst;
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060002DF RID: 735 RVA: 0x0001692B File Offset: 0x00014B2B
	public static DataManagerScenario DmScenario
	{
		get
		{
			return Singleton<DataManager>.Instance.dmScenario;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x060002E0 RID: 736 RVA: 0x00016937 File Offset: 0x00014B37
	public static DataManagerAchievement DmAchievement
	{
		get
		{
			return Singleton<DataManager>.Instance.dmAchievement;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x060002E1 RID: 737 RVA: 0x00016943 File Offset: 0x00014B43
	public static DataManagerIntroductionNewChara DmIntroduction
	{
		get
		{
			return Singleton<DataManager>.Instance.dmIntroduction;
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060002E2 RID: 738 RVA: 0x0001694F File Offset: 0x00014B4F
	public static DataManagerBookmark DmBookmark
	{
		get
		{
			return Singleton<DataManager>.Instance.dmBookmark;
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060002E3 RID: 739 RVA: 0x0001695B File Offset: 0x00014B5B
	public static DataManagerAssistant DmAssistant
	{
		get
		{
			return Singleton<DataManager>.Instance.dmAssistant;
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060002E5 RID: 741 RVA: 0x00016970 File Offset: 0x00014B70
	// (set) Token: 0x060002E4 RID: 740 RVA: 0x00016967 File Offset: 0x00014B67
	public bool lockByDebug { get; set; }

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060002E7 RID: 743 RVA: 0x00016981 File Offset: 0x00014B81
	// (set) Token: 0x060002E6 RID: 742 RVA: 0x00016978 File Offset: 0x00014B78
	public bool lockByPurchase { get; set; }

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060002E8 RID: 744 RVA: 0x00016989 File Offset: 0x00014B89
	// (set) Token: 0x060002E9 RID: 745 RVA: 0x00016991 File Offset: 0x00014B91
	public bool DisableServerRequestByTutorial { get; set; }

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060002EA RID: 746 RVA: 0x0001699A File Offset: 0x00014B9A
	// (set) Token: 0x060002EB RID: 747 RVA: 0x000169A2 File Offset: 0x00014BA2
	public bool DisableServerRequestByDebug { get; set; }

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x060002EC RID: 748 RVA: 0x000169AB File Offset: 0x00014BAB
	// (set) Token: 0x060002ED RID: 749 RVA: 0x000169B3 File Offset: 0x00014BB3
	public bool IsSetupData { get; private set; }

	// Token: 0x060002EE RID: 750 RVA: 0x000169BC File Offset: 0x00014BBC
	public static bool IsServerRequesting()
	{
		return Singleton<DataManager>.Instance.lockByServerError || Singleton<DataManager>.Instance.lockByPurchase || Singleton<DataManager>.Instance.lockByDebug || (Singleton<Manager>.Instance != null && Manager.IsCmdProcessing());
	}

	// Token: 0x060002EF RID: 751 RVA: 0x000169FC File Offset: 0x00014BFC
	public static ActionTypeMask GetServerErrorType()
	{
		return Singleton<DataManager>.Instance.actionType;
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00016A08 File Offset: 0x00014C08
	public static bool IsFinishCreateByMst()
	{
		return Singleton<DataManager>.Instance.dmItem != null && Singleton<DataManager>.Instance.dmItem.GetItemStaticMap().Count > 0;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00016A30 File Offset: 0x00014C30
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
		this.dmIntroduction = new DataManagerIntroductionNewChara(this);
		this.dmBookmark = new DataManagerBookmark(this);
		this.dmAssistant = new DataManagerAssistant(this);
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x00016BFC File Offset: 0x00014DFC
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

	// Token: 0x060002F3 RID: 755 RVA: 0x00016C8B File Offset: 0x00014E8B
	public void InitializeByEditor(UnityAction finishCallBack)
	{
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00016C8D File Offset: 0x00014E8D
	public void UpdateByEditor()
	{
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00016C90 File Offset: 0x00014E90
	public void UpdateUserAssetByLogin(LoginDmmResponse res)
	{
		this.dmChara.UpdateUserDataByServer(res.charas);
		this.dmDeck.UpdateUserDataByServer(res.decks);
		DataManager.DmKemoBoard.UpdateUserDataByServer(res.kemoboard_panels);
		this.dmItem.UpdateUserDataByServer(res.items, res.charas, res.achievements);
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

	// Token: 0x060002F6 RID: 758 RVA: 0x00016ED4 File Offset: 0x000150D4
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
			this.dmItem.UpdateUserDataByServer(assets.update_item_list, assets.update_chara_list, assets.update_achievement_list);
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
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0001716D File Offset: 0x0001536D
	public void UpdateSumUserAllCharaKemoStatus(List<int> updateKemoStatusCharaIdList)
	{
		this.dmChara.UpdateSumUserAllCharaKemoStatus(updateKemoStatusCharaIdList);
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x0001717B File Offset: 0x0001537B
	public void UpdateUserOption(List<int> optionList)
	{
		this.dmUserInfo.UpdateUserOptionByServer(optionList);
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x0001718C File Offset: 0x0001538C
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

	// Token: 0x060002FA RID: 762 RVA: 0x00017244 File Offset: 0x00015444
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

	// Token: 0x060002FB RID: 763 RVA: 0x0001730C File Offset: 0x0001550C
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

	// Token: 0x060002FC RID: 764 RVA: 0x000174CC File Offset: 0x000156CC
	public void CbServerRetry(Command cmd, ErrorCode error)
	{
		this.lockByServerError = true;
		CanvasManager.HdlWebViewWindowCtrl.Close();
		this.retryCmdList.Add(cmd);
		string text = string.Format("{0}\n\nCODE:{1}", error.msg, error.id);
		CanvasManager.HdlOpenWindowServerError.Setup(error.tit, text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), false, new PguiOpenWindowCtrl.Callback(this.OnClickServerRetryWindow), null, false);
		CanvasManager.HdlOpenWindowServerError.ForceOpen();
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00017544 File Offset: 0x00015744
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

	// Token: 0x060002FE RID: 766 RVA: 0x0001760C File Offset: 0x0001580C
	public void ServerRequest(Command cmd, Action<Command> cb)
	{
		if (this.DisableServerRequestByTutorial)
		{
			return;
		}
		this.actionType = ActionTypeMask.INVALID;
		Command.SetupRequest(cmd, cb, new Action<Command>(this.CbServerError), new Action<Command, ErrorCode>(this.CbServerRetry));
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0001763D File Offset: 0x0001583D
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

	// Token: 0x06000300 RID: 768 RVA: 0x0001764C File Offset: 0x0001584C
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

	// Token: 0x04000472 RID: 1138
	private DataManagerUserInfo dmUserInfo;

	// Token: 0x04000473 RID: 1139
	private DataManagerMasterSkill dmMasterSkill;

	// Token: 0x04000474 RID: 1140
	private DataManagerGameStatus dmGameStatus;

	// Token: 0x04000475 RID: 1141
	private DataManagerMonthlyPack dmMonthlyPack;

	// Token: 0x04000476 RID: 1142
	private DataManagerItem dmItem;

	// Token: 0x04000477 RID: 1143
	private DataManagerChara dmChara;

	// Token: 0x04000478 RID: 1144
	private DataManagerGacha dmGacha;

	// Token: 0x04000479 RID: 1145
	private DataManagerDeck dmDeck;

	// Token: 0x0400047A RID: 1146
	private DataManagerPhoto dmPhoto;

	// Token: 0x0400047B RID: 1147
	private DataManagerCharaAccessory dmChAcc;

	// Token: 0x0400047C RID: 1148
	private DataManagerQuest dmQuest;

	// Token: 0x0400047D RID: 1149
	private DataManagerHelper dmHelper;

	// Token: 0x0400047E RID: 1150
	private DataManagerPresent dmPresent;

	// Token: 0x0400047F RID: 1151
	private DataManagerEvent dmEvent;

	// Token: 0x04000480 RID: 1152
	private DataManagerCampaign dmCampaign;

	// Token: 0x04000481 RID: 1153
	private DataManagerPicnic dmPicnic;

	// Token: 0x04000482 RID: 1154
	private DataManagerMission dmMission;

	// Token: 0x04000483 RID: 1155
	private DataManagerCharaMission dmChMission;

	// Token: 0x04000484 RID: 1156
	private DataManagerPurchase dmPurchase;

	// Token: 0x04000485 RID: 1157
	private DataManagerShop dmShop;

	// Token: 0x04000486 RID: 1158
	private DataManagerHome dmHome;

	// Token: 0x04000487 RID: 1159
	private DataManagerPvp dmPvp;

	// Token: 0x04000488 RID: 1160
	private DataManagerTraining dmTraining;

	// Token: 0x04000489 RID: 1161
	private DataManagerKemoBoard dmKemoBoard;

	// Token: 0x0400048A RID: 1162
	private DataManagerTreeHouse dmTreeHouse;

	// Token: 0x0400048B RID: 1163
	private DataManagerServerMst dmServerMst;

	// Token: 0x0400048C RID: 1164
	private DataManagerScenario dmScenario;

	// Token: 0x0400048D RID: 1165
	private DataManagerAchievement dmAchievement;

	// Token: 0x0400048E RID: 1166
	private DataManagerIntroductionNewChara dmIntroduction;

	// Token: 0x0400048F RID: 1167
	private DataManagerBookmark dmBookmark;

	// Token: 0x04000490 RID: 1168
	private DataManagerAssistant dmAssistant;

	// Token: 0x04000491 RID: 1169
	private bool lockByServerError;

	// Token: 0x04000494 RID: 1172
	private ActionTypeMask actionType;

	// Token: 0x04000498 RID: 1176
	private List<Command> retryCmdList = new List<Command>();

	// Token: 0x02000622 RID: 1570
	public enum NewFlgCategory
	{
		// Token: 0x04002D9B RID: 11675
		INVALID,
		// Token: 0x04002D9C RID: 11676
		ITEM,
		// Token: 0x04002D9D RID: 11677
		QUEST,
		// Token: 0x04002D9E RID: 11678
		COMMON,
		// Token: 0x04002D9F RID: 11679
		SORTTYPE,
		// Token: 0x04002DA0 RID: 11680
		PICNIC,
		// Token: 0x04002DA1 RID: 11681
		RELEASEMODE,
		// Token: 0x04002DA2 RID: 11682
		EVENT1,
		// Token: 0x04002DA3 RID: 11683
		EVENT2,
		// Token: 0x04002DA4 RID: 11684
		GACHANEWINFO,
		// Token: 0x04002DA5 RID: 11685
		CHARAGROWTUTORIAL,
		// Token: 0x04002DA6 RID: 11686
		SHOP_GOODS,
		// Token: 0x04002DA7 RID: 11687
		PURCHASE_INFO,
		// Token: 0x04002DA8 RID: 11688
		TREEHOUSE_FURNITURE_FAVORITE,
		// Token: 0x04002DA9 RID: 11689
		IconSizeIndex
	}
}
