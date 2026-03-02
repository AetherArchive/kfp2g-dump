using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001CD RID: 461
public class PguiCmnMenuCtrl : MonoBehaviour
{
	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x06001F78 RID: 8056 RVA: 0x001843D1 File Offset: 0x001825D1
	// (set) Token: 0x06001F79 RID: 8057 RVA: 0x001843D9 File Offset: 0x001825D9
	public bool IsSceneManagerMoving { get; set; }

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x06001F7A RID: 8058 RVA: 0x001843E2 File Offset: 0x001825E2
	// (set) Token: 0x06001F7B RID: 8059 RVA: 0x001843EA File Offset: 0x001825EA
	public PguiCmnMenuCtrl.OnClickMoveSequenceButton moveSequenceCallback { get; set; }

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x06001F7C RID: 8060 RVA: 0x001843F3 File Offset: 0x001825F3
	// (set) Token: 0x06001F7D RID: 8061 RVA: 0x001843FB File Offset: 0x001825FB
	public List<PguiCmnMenuCtrl.OnPlayAnimation> playAnimation { get; set; }

	// Token: 0x06001F7E RID: 8062 RVA: 0x00184404 File Offset: 0x00182604
	private void Awake()
	{
		this.guiData = new PguiCmnMenuCtrl.GUI(base.transform);
		this.guiData.BuyStone.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.OnTouchRectIconStone(this.guiData.BuyStone.transform);
		}, null, null, null, null);
		base.gameObject.SetActive(false);
		for (int i = 0; i < this.guiData.buttonList.Count; i++)
		{
			this.guiData.buttonList[i].AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), (this.guiData.buttonList[i] == this.guiData.Btn_Menu) ? PguiButtonCtrl.SoundType.MENU_SLIDE : PguiButtonCtrl.SoundType.DEFAULT);
		}
		this.guiData.TouchMask.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.OnTouchPanel();
		}, null, null, null, null);
		this.guiData.Btn_Stamina.AddComponent<PguiCollider>();
		this.guiData.Btn_Stamina.AddComponent<PguiTouchTrigger>().AddListener(null, null, null, delegate
		{
			this.isWinStamina = true;
		}, delegate
		{
			this.isWinStamina = false;
		});
	}

	// Token: 0x06001F7F RID: 8063 RVA: 0x0018451D File Offset: 0x0018271D
	private void Update()
	{
		if (this.windowCoroutine != null && !this.windowCoroutine.MoveNext())
		{
			this.windowCoroutine = null;
		}
	}

	// Token: 0x06001F80 RID: 8064 RVA: 0x0018453B File Offset: 0x0018273B
	private void OnTouchPanel()
	{
		if (this.guiData.Btn_Menu.gameObject.activeSelf)
		{
			this.isRequestSwitch = true;
		}
	}

	// Token: 0x06001F81 RID: 8065 RVA: 0x0018455C File Offset: 0x0018275C
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (!this.guiData.Btn_Menu.gameObject.activeSelf)
		{
			return;
		}
		if (this.IsSceneManagerMoving)
		{
			return;
		}
		this.nextSceneArgs = null;
		if (button == this.guiData.Btn_Menu)
		{
			this.isRequestSwitch = true;
		}
		else if (button == this.guiData.Btn_Back)
		{
			this.isRequestRetrun = true;
		}
		else if (button == this.guiData.Btn_Home)
		{
			this.requestNextScene = SceneManager.SceneName.SceneHome;
		}
		else if (button == this.guiData.Btn_Quest)
		{
			this.requestNextScene = SceneManager.SceneName.SceneQuest;
		}
		else if (button == this.guiData.Btn_Gacha)
		{
			this.requestNextScene = SceneManager.SceneName.SceneGacha;
		}
		else if (button == this.guiData.Btn_CharaEdit)
		{
			this.requestNextScene = SceneManager.SceneName.SceneCharaEdit;
		}
		else if (button == this.guiData.Btn_Shop)
		{
			this.requestNextScene = SceneManager.SceneName.SceneShop;
		}
		else if (button == this.guiData.Btn_Present)
		{
			this.requestNextScene = SceneManager.SceneName.ScenePresent;
		}
		else if (button == this.guiData.Btn_Config)
		{
			this.requestNextScene = SceneManager.SceneName.SceneOtherMenuTop;
		}
		else if (button == this.guiData.Btn_Follow)
		{
			this.requestNextScene = SceneManager.SceneName.SceneFriend;
		}
		else if (button == this.guiData.Btn_Mission)
		{
			this.requestNextScene = SceneManager.SceneName.SceneMission;
		}
		else if (button == this.guiData.Btn_MissionEvent1)
		{
			this.requestNextScene = SceneManager.SceneName.SceneMission;
			if (this.missionEventId1 >= 0)
			{
				this.nextSceneArgs = new SceneMission.MissionOpenParam((this.missionEventId1 == 0) ? MissionType.BEGINNER : MissionType.EVENTTOTAL, this.missionEventId1);
			}
		}
		else if (button == this.guiData.Btn_MissionEvent2)
		{
			this.requestNextScene = SceneManager.SceneName.SceneMission;
			if (this.missionEventId2 >= 0)
			{
				this.nextSceneArgs = new SceneMission.MissionOpenParam((this.missionEventId2 == 0) ? MissionType.BEGINNER : MissionType.EVENTTOTAL, this.missionEventId2);
			}
		}
		else if (button == this.guiData.Btn_Movie)
		{
			this.requestNextScene = SceneManager.SceneName.SceneStoryView;
			this.nextSceneArgs = new SceneStoryView.Args
			{
				viewType = SceneStoryView.Args.VIEWTYPE.MOVIE,
				resultNextSceneName = SceneManager.SceneName.SceneHome,
				resultNextSceneArgs = null
			};
		}
		else if (button == this.guiData.Btn_TreeHouse)
		{
			this.requestNextScene = SceneManager.SceneName.SceneTreeHouse;
		}
		else if (button == this.guiData.Btn_KizunaBuff)
		{
			this.windowCoroutine = this.OpenBuffInfoWindow();
		}
		else if (button == this.guiData.Btn_Pvp)
		{
			if (this.pvpLck == 0)
			{
				this.requestNextScene = SceneManager.SceneName.ScenePvp;
			}
			else
			{
				CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open("解放条件", new List<CmnReleaseConditionWindowCtrl.SetupParam>
				{
					new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = this.pvpOpenMsg(false),
						enableClear = false
					}
				});
			}
		}
		else if (button == this.guiData.Btn_Picnic)
		{
			if (this.picnicLck == 0)
			{
				this.requestNextScene = SceneManager.SceneName.ScenePicnic;
			}
			else
			{
				CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open("解放条件", new List<CmnReleaseConditionWindowCtrl.SetupParam>
				{
					new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = this.picnicOpenMsg(false),
						enableClear = false
					}
				});
			}
		}
		else if (button == this.guiData.Btn_News)
		{
			List<HomeBannerData> list = DataManager.DmHome.GetHomeBannerList().FindAll((HomeBannerData itm) => itm.actionType == HomeBannerData.ActionType.NOTICE);
			if (list.Count > 0)
			{
				CanvasManager.HdlWebViewWindowCtrl.Open(list[0].actionParamURL);
			}
		}
		else if (button == this.guiData.Btn_Question)
		{
			CanvasManager.HdlHelpWindowCtrl.Open(false);
		}
		else if (button == this.guiData.Btn_Profile)
		{
			this.requestNextScene = SceneManager.SceneName.SceneProfile;
		}
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			CanvasManager.HdlPhotoWindowCtrl.ResetPrevData();
			CanvasManager.HdlAccessoryWindowCtrl.ResetPrevData();
			SoundManager.UnloadCVAll("");
		}
	}

	// Token: 0x06001F82 RID: 8066 RVA: 0x00184989 File Offset: 0x00182B89
	public void SetupMenu(bool isDisp, string title = "", PguiCmnMenuCtrl.OnClickReturnButton cb = null, string subInfo = "", PguiCmnMenuCtrl.OnClickMoveSequenceButton cbMs = null, List<PguiCmnMenuCtrl.OnPlayAnimation> cbAnim = null)
	{
		this.SetupMenuInternal(isDisp, title, cb, false, subInfo, cbMs, cbAnim);
	}

	// Token: 0x06001F83 RID: 8067 RVA: 0x0018499B File Offset: 0x00182B9B
	public void SetupMenuByHome(PguiCmnMenuCtrl.OnClickReturnButton cb = null, PguiCmnMenuCtrl.OnClickMoveSequenceButton cbMs = null, List<PguiCmnMenuCtrl.OnPlayAnimation> cbAnim = null)
	{
		this.SetupMenuInternal(true, "", cb, true, "", cbMs, cbAnim);
	}

	// Token: 0x06001F84 RID: 8068 RVA: 0x001849B4 File Offset: 0x00182BB4
	public bool SetActiveMenu(bool sw)
	{
		if (!(this.guiData.Btn_Menu.gameObject.activeSelf ^ sw))
		{
			return false;
		}
		this.guiData.Btn_Menu.gameObject.SetActive(sw);
		this.guiData.ImgViewMode.gameObject.SetActive(sw);
		this.isRequestSwitch = false;
		if (sw ^ this.isOpen)
		{
			this.isRequestSwitch = true;
		}
		else
		{
			this.isOpen = !this.isOpen;
		}
		this.guiData.GroupObjInfoAll.SetActive((!this.isHome && sw) || (this.isHome && sw && this.isOpen && !this.isRequestSwitch));
		this.guiData.GroupObjTitle.SetActive(sw);
		foreach (object obj in this.guiData.GroupObjTitle.transform)
		{
			Transform transform = (Transform)obj;
			if (transform != this.guiData.Btn_Question.transform && transform != this.guiData.Txt_SubInfo.transform.parent)
			{
				transform.gameObject.SetActive(!this.isHome && sw);
			}
		}
		return this.isRequestSwitch;
	}

	// Token: 0x06001F85 RID: 8069 RVA: 0x00184B20 File Offset: 0x00182D20
	public bool IsActiveMenu()
	{
		return this.guiData.Btn_Menu.gameObject.activeSelf;
	}

	// Token: 0x06001F86 RID: 8070 RVA: 0x00184B38 File Offset: 0x00182D38
	public void UpdateSubInfo(string subInfo)
	{
		if (subInfo != string.Empty)
		{
			this.guiData.Txt_SubInfo.text = subInfo;
			this.guiData.Txt_SubInfo.transform.parent.gameObject.SetActive(true);
			return;
		}
		this.guiData.Txt_SubInfo.transform.parent.gameObject.SetActive(false);
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x00184BA4 File Offset: 0x00182DA4
	private void SetupMenuInternal(bool isDisp, string title, PguiCmnMenuCtrl.OnClickReturnButton cb, bool home, string subInfo, PguiCmnMenuCtrl.OnClickMoveSequenceButton cbMs, List<PguiCmnMenuCtrl.OnPlayAnimation> cbAnim)
	{
		this.isHome = home;
		this.isRequestSwitch = false;
		this.isRequestRetrun = false;
		this.requestNextScene = SceneManager.SceneName.None;
		this.nextSceneArgs = null;
		base.gameObject.SetActive(isDisp);
		this.guiData.Txt_Title.text = title;
		this.returnCallback = cb;
		this.moveSequenceCallback = cbMs;
		this.playAnimation = cbAnim;
		this.UpdateSubInfo(subInfo);
		if (isDisp)
		{
			this.guiData.GUI_CmnMenu.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		}
		if (this.isHome)
		{
			this.guiData.GroupObjHome.SetActive(true);
			this.guiData.GroupObjOther.SetActive(false);
			this.isRequestSwitch = true;
			this.isOpen = false;
			this.guiData.TitleBase.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.guiData.ImgViewMode.gameObject.SetActive(true);
			this.guiData.ImgViewMode.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		}
		else
		{
			this.guiData.GroupObjHome.SetActive(true);
			this.guiData.GroupObjOther.SetActive(true);
			this.isOpen = false;
			this.guiData.TitleBase.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.guiData.ImgViewMode.gameObject.SetActive(false);
		}
		this.guiData.GroupObjTitle.SetActive(true);
		foreach (object obj in this.guiData.GroupObjTitle.transform)
		{
			Transform transform = (Transform)obj;
			if (transform != this.guiData.Btn_Question.transform && transform != this.guiData.Txt_SubInfo.transform.parent)
			{
				transform.gameObject.SetActive(!this.isHome);
			}
		}
		this.guiData.GroupObjInfoAll.SetActive(true);
		this.guiData.Present_Badge.SetActive(false);
		this.missionCnt = -1;
		this.missionEventId1 = -1;
		this.missionEventId2 = -1;
		this.guiData.Mission_New.SetActive(false);
		this.guiData.Mission_Badge.SetActive(false);
		this.guiData.Btn_MissionEvent1.gameObject.SetActive(false);
		this.guiData.Btn_MissionEvent2.gameObject.SetActive(false);
		this.guiData.Btn_Movie.gameObject.SetActive(false);
		this.guiData.Btn_TreeHouse.gameObject.SetActive(!this.isHome);
		this.guiData.Btn_News.gameObject.SetActive(this.isHome);
		this.guiData.Quest_Campaign.SetActive(false);
		this.guiData.CharaEdit_Campaign.SetActive(false);
		this.guiData.Gacha_Campaign.SetActive(false);
		this.guiData.Shop_Campaign.SetActive(false);
		this.guiData.Pvp_Campaign.SetActive(false);
		this.guiData.Picnic_Campaign.SetActive(false);
		this.Quest_Campaign = (this.CharaEdit_Campaign = (this.Gacha_Campaign = (this.Shop_Campaign = (this.Pvp_Campaign = (this.Picnic_Campaign = 0)))));
		this.Quest_Campaign_anm = (this.CharaEdit_Campaign_anm = (this.Gacha_Campaign_anm = (this.Shop_Campaign_anm = (this.Pvp_Campaign_anm = (this.Picnic_Campaign_anm = 0f)))));
		this.guiData.Pvp_Lock.gameObject.SetActive(false);
		this.guiData.Picnic_Lock.gameObject.SetActive(false);
		this.guiData.Quest_Event.SetActive(false);
		this.guiData.Gacha_New.SetActive(false);
		this.guiData.Picnic_Badge.SetActive(false);
		this.guiData.Home_Badge.SetActive(false);
		this.guiData.Pvp_Event.SetActive(false);
		this.guiData.TreeHouse_Badge.SetActive(false);
		this.pvpLock = null;
		this.picnicLock = null;
		this.pvpLck = (this.picnicLck = 0);
		this.isWinStamina = false;
		this.userFlag = null;
		this.newGacha = null;
		this.questDrawList = null;
		this.eventList = null;
		this.pvpEvent = 0;
		if (isDisp)
		{
			this.userFlag = DataManager.DmGameStatus.MakeUserFlagData();
			DataManagerServerMst.ModeReleaseData modeReleaseData = DataManager.DmServerMst.ModeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.PvpMode);
			this.pvpLock = ((modeReleaseData == null) ? null : DataManager.DmQuest.GetQuestOnePackData(modeReleaseData.QuestId));
			modeReleaseData = DataManager.DmServerMst.ModeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic);
			this.picnicLock = ((modeReleaseData == null) ? null : DataManager.DmQuest.GetQuestOnePackData(modeReleaseData.QuestId));
			if (this.userFlag.ReleaseModeFlag.PvpMode != DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released)
			{
				this.pvpLck = 1;
			}
			if (this.userFlag.ReleaseModeFlag.Picnic != DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released)
			{
				this.picnicLck = 1;
			}
			this.UpdateUserInfo();
			this.UpdateCampaign();
			this.UpdateMission();
			HomeBannerData homeBannerData;
			if (!this.isHome)
			{
				homeBannerData = null;
			}
			else
			{
				homeBannerData = DataManager.DmHome.GetHomeBannerList().Find((HomeBannerData itm) => itm.actionType == HomeBannerData.ActionType.MOVE_MOVIE);
			}
			HomeBannerData homeBannerData2 = homeBannerData;
			if (homeBannerData2 != null)
			{
				this.guiData.Btn_Movie.gameObject.SetActive(true);
				if (string.IsNullOrEmpty(homeBannerData2.bannerImagePath))
				{
					this.guiData.Btn_Movie.transform.Find("BaseImage").GetComponent<PguiRawImageCtrl>().SetRawImage("Texture2D/Chokokemo_Icon/icon_available", true, false, null);
					return;
				}
				this.guiData.Btn_Movie.transform.Find("BaseImage").GetComponent<PguiRawImageCtrl>().banner = "Texture2D/Chokokemo_Icon/" + homeBannerData2.bannerImagePath;
			}
		}
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x001851E0 File Offset: 0x001833E0
	public void UpdateMenu(bool isEnable, bool backKey = true)
	{
		DateTime now = TimeManager.Now;
		if (this.isEnableEvent != isEnable)
		{
			this.isEnableEvent = isEnable;
			for (int i = 0; i < this.guiData.colliList.Count; i++)
			{
				this.guiData.colliList[i].raycastTarget = this.isEnableEvent;
			}
		}
		bool flag = false;
		if (this.isEnableEvent)
		{
			if (this.isRequestSwitch)
			{
				this.isOpen = !this.isOpen;
				this.guiData.TouchMask.SetActive(this.isOpen && !this.isHome);
				this.guiData.GUI_CmnMenu.ExPlayAnimation(this.isOpen ? SimpleAnimation.ExPguiStatus.START : SimpleAnimation.ExPguiStatus.END, null);
				if (this.isHome)
				{
					this.guiData.TitleBase.ExPlayAnimation(this.isOpen ? SimpleAnimation.ExPguiStatus.START : SimpleAnimation.ExPguiStatus.END, null);
				}
				if (this.playAnimation != null)
				{
					foreach (PguiCmnMenuCtrl.OnPlayAnimation onPlayAnimation in this.playAnimation)
					{
						onPlayAnimation(this.isOpen ? SimpleAnimation.ExPguiStatus.START : SimpleAnimation.ExPguiStatus.END);
					}
				}
				this.guiData.GroupObjInfoAll.SetActive(!this.isHome || this.isOpen);
				flag = this.guiData.GroupObjInfoAll.activeSelf;
			}
			else if (this.isRequestRetrun)
			{
				if (this.returnCallback != null)
				{
					this.returnCallback();
					this.requestNextScene = SceneManager.SceneName.None;
					this.nextSceneArgs = null;
					if (this.isOpen)
					{
						this.isOpen = false;
						this.guiData.TouchMask.SetActive(this.isOpen && !this.isHome);
						this.guiData.GUI_CmnMenu.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
					}
				}
				else
				{
					this.requestNextScene = SceneManager.SceneName.SceneHome;
					this.nextSceneArgs = null;
				}
			}
			if (this.requestNextScene != SceneManager.SceneName.None)
			{
				if (this.moveSequenceCallback != null && this.moveSequenceCallback(this.requestNextScene, this.nextSceneArgs))
				{
					this.requestNextScene = SceneManager.SceneName.None;
					this.nextSceneArgs = null;
				}
				else
				{
					this.MoveSceneByMenu(this.requestNextScene, this.nextSceneArgs);
				}
			}
		}
		if (this.guiData.ImgViewMode.gameObject.activeSelf && (!this.isOpen || !this.guiData.ImgViewMode.ExIsPlaying()))
		{
			this.guiData.ImgViewMode.gameObject.SetActive(false);
		}
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		int num = ((homeCheckResult == null) ? 0 : homeCheckResult.presentBoxNum);
		this.guiData.Present_Badge.SetActive(num > 0);
		this.guiData.Present_Badge_Num.text = ((num < 100) ? num.ToString() : "99+");
		if (base.gameObject.activeSelf)
		{
			this.UpdateUserInfo();
			this.UpdateCampaign();
			this.UpdateMission();
		}
		this.isRequestSwitch = false;
		this.isRequestRetrun = false;
		this.requestNextScene = SceneManager.SceneName.None;
		this.nextSceneArgs = null;
		this.guiData.Btn_Back.androidBackKeyTarget = backKey && this.guiData.GroupObjTitle.activeSelf && this.guiData.Btn_Back.gameObject.activeSelf;
		this.guiData.Btn_Menu.androidBackKeyTarget = backKey && !this.guiData.GroupObjOther.activeSelf && !this.isOpen;
		if (this.isOpen && this.pvpLck == 1)
		{
			this.pvpLck = 2;
			this.guiData.Pvp_Lock.gameObject.SetActive(true);
			this.guiData.Pvp_Lock.Setup(new MarkLockCtrl.SetupParam
			{
				updateConditionCallback = () => this.pvpLock != null && (this.pvpLock.questDynamicOne.status == QuestOneStatus.CLEAR || this.pvpLock.questDynamicOne.status == QuestOneStatus.COMPLETE),
				releaseFlag = false,
				tagetObject = this.guiData.Btn_Pvp.gameObject,
				text = this.pvpOpenMsg(true),
				updateUserFlagDataCallback = delegate
				{
					this.pvpLck = 3;
				}
			}, true);
		}
		else if (this.pvpLck == 3 && !DataManager.IsServerRequesting())
		{
			this.userFlag = DataManager.DmGameStatus.MakeUserFlagData();
			this.userFlag.ReleaseModeFlag.PvpMode = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(this.userFlag);
			this.pvpLck = 0;
		}
		if (this.isOpen && this.picnicLck == 1)
		{
			this.picnicLck = 2;
			this.guiData.Picnic_Lock.gameObject.SetActive(true);
			this.guiData.Picnic_Lock.Setup(new MarkLockCtrl.SetupParam
			{
				updateConditionCallback = () => this.picnicLock != null && (this.picnicLock.questDynamicOne.status == QuestOneStatus.CLEAR || this.picnicLock.questDynamicOne.status == QuestOneStatus.COMPLETE),
				releaseFlag = false,
				tagetObject = this.guiData.Btn_Picnic.gameObject,
				text = this.picnicOpenMsg(true),
				updateUserFlagDataCallback = delegate
				{
					this.picnicLck = 3;
				}
			}, true);
		}
		else if (this.picnicLck == 3 && !DataManager.IsServerRequesting())
		{
			this.userFlag = DataManager.DmGameStatus.MakeUserFlagData();
			this.userFlag.ReleaseModeFlag.Picnic = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(this.userFlag);
			this.picnicLck = 0;
		}
		bool flag2 = DataManager.DmPurchase.BadgeDispLimitedTime != null && DataManager.DmPurchase.BadgeDispLimitedTime >= now;
		if (flag2 && (!this.guiData.Popup_Limited.gameObject.activeSelf || flag))
		{
			this.guiData.Popup_Limited.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		}
		this.guiData.Popup_Limited.gameObject.SetActive(flag2);
		DataManagerCampaign.CampaignStaminaRecoveryData presentCampaignStaminaRecoveryData = DataManager.DmCampaign.PresentCampaignStaminaRecoveryData;
		if (presentCampaignStaminaRecoveryData != null && (!this.guiData.Popup_StaminaRecovery_Campaign.gameObject.activeSelf || flag))
		{
			this.guiData.Txt_StaminaRecovery_Campaign.text = "スタミナ回復速度" + (300 / presentCampaignStaminaRecoveryData.staminaRecoveryTime).ToString() + "倍!";
			this.guiData.Popup_StaminaRecovery_Campaign.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
			this.guiData.Popup_StaminaRecovery_Campaign.gameObject.SetActive(true);
			return;
		}
		if (presentCampaignStaminaRecoveryData == null && this.guiData.Popup_StaminaRecovery_Campaign.gameObject.activeSelf)
		{
			this.guiData.Popup_StaminaRecovery_Campaign.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001F89 RID: 8073 RVA: 0x0018586C File Offset: 0x00183A6C
	protected virtual void OnDisable()
	{
		if (this.guiData != null)
		{
			this.guiData.Popup_Limited.gameObject.SetActive(false);
			this.guiData.Popup_StaminaRecovery_Campaign.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x001858A4 File Offset: 0x00183AA4
	public void MoveSceneByMenu(SceneManager.SceneName nextScene, object args = null)
	{
		if (this.isOpen)
		{
			this.isOpen = false;
			this.guiData.TouchMask.SetActive(this.isOpen && !this.isHome);
			this.guiData.GUI_CmnMenu.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			if (this.playAnimation != null)
			{
				foreach (PguiCmnMenuCtrl.OnPlayAnimation onPlayAnimation in this.playAnimation)
				{
					onPlayAnimation(SimpleAnimation.ExPguiStatus.END);
				}
			}
		}
		this.AnimEndTitleBase();
		this.IsSceneManagerMoving = true;
		Singleton<SceneManager>.Instance.SetNextScene(nextScene, args);
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x0018595C File Offset: 0x00183B5C
	public void AnimEndTitleBase()
	{
		if (!this.isHome || this.isOpen)
		{
			this.guiData.TitleBase.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		}
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x00185980 File Offset: 0x00183B80
	private void UpdateUserInfo()
	{
		this.guiData.Num_Rank.text = DataManager.DmUserInfo.level.ToString();
		long expByNextLevel = DataManager.DmUserInfo.GetExpByNextLevel(DataManager.DmUserInfo.level);
		this.guiData.RankGage_Gage.m_Image.fillAmount = ((expByNextLevel > 0L && DataManager.DmUserInfo.exp < expByNextLevel) ? ((float)DataManager.DmUserInfo.exp / (float)expByNextLevel) : 1f);
		this.guiData.Num_Stone.text = DataManager.DmItem.GetUserItemData(30100).num.ToString();
		StaminaInfo.NowInfo infoByNow = DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now);
		Color gameObjectById = this.guiData.Num_Stamina.GetComponent<PguiColorCtrl>().GetGameObjectById((infoByNow.stackNum > infoByNow.stackMaxNum) ? "1" : "0");
		int num = (int)(gameObjectById.r * 255f);
		int num2 = (int)(gameObjectById.g * 255f);
		int num3 = (int)(gameObjectById.b * 255f);
		this.guiData.Num_Stamina.text = string.Concat(new string[]
		{
			"<color=#",
			num.ToString("x2"),
			num2.ToString("x2"),
			num3.ToString("x2"),
			">",
			infoByNow.stackNum.ToString(),
			"</color>/",
			infoByNow.stackMaxNum.ToString()
		});
		this.guiData.Nxt_Stamina.text = infoByNow.nextRecoveryTime.Minute.ToString() + ":" + infoByNow.nextRecoveryTime.Second.ToString("D2");
		this.guiData.Nxt_Stamina.gameObject.SetActive(infoByNow.stackNum < infoByNow.stackMaxNum);
		this.guiData.StaminaGage_Gage.m_Image.fillAmount = ((infoByNow.stackMaxNum > 0 && infoByNow.stackNum < infoByNow.stackMaxNum) ? ((float)infoByNow.stackNum / (float)infoByNow.stackMaxNum) : 1f);
		this.guiData.Win_Stamina.transform.Find("Num_NextRank").GetComponent<PguiTextCtrl>().text = (expByNextLevel - DataManager.DmUserInfo.exp).ToString();
		this.guiData.Win_Stamina.transform.Find("Time_RecoveryStamina").GetComponent<PguiTextCtrl>().text = ((infoByNow.stackNum < infoByNow.stackMaxNum) ? string.Concat(new string[]
		{
			infoByNow.allRecoveryTime.Hour.ToString("D2"),
			":",
			infoByNow.allRecoveryTime.Minute.ToString("D2"),
			":",
			infoByNow.allRecoveryTime.Second.ToString("D2")
		}) : "0:00:00");
		this.guiData.Win_Stamina.transform.Find("Num_PlusStanima").GetComponent<PguiTextCtrl>().text = "+" + DataManager.DmTreeHouse.StaminaBonusData.staminaBonus.ToString();
		this.guiData.Win_Stamina.SetActive(this.isWinStamina);
	}

	// Token: 0x06001F8D RID: 8077 RVA: 0x00185D00 File Offset: 0x00183F00
	private void UpdateCampaign()
	{
		DateTime dt = TimeManager.Now;
		List<string> list = new List<string>();
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed @sealed = ((homeCheckResult != null) ? homeCheckResult.sealedData : null) ?? new Sealed();
		if (DataManager.DmTraining.IsChallengePossible && @sealed.quest_training != 1)
		{
			list.Add("");
		}
		DataManagerCampaign.CampaignItemDropData presentCampaignItemDropData = DataManager.DmCampaign.PresentCampaignItemDropData;
		if (presentCampaignItemDropData != null)
		{
			list.Add("獲得アイテム数<size=24><color=#FFEE00>" + PguiCmnMenuCtrl.Ratio2String(presentCampaignItemDropData.ratio) + "</color></size>倍！");
		}
		if (this.isOpen)
		{
			if (this.questDrawList != null)
			{
				goto IL_0128;
			}
			QuestStaticData questStaticData = DataManager.DmQuest.QuestStaticData;
			this.questDrawList = new List<DataManagerQuest.DrawItemData>();
			using (List<QuestStaticChapter>.Enumerator enumerator = questStaticData.chapterDataList.GetEnumerator())
			{
				Predicate<DataManagerQuest.DrawItemData> <>9__3;
				while (enumerator.MoveNext())
				{
					QuestStaticChapter questStaticChapter = enumerator.Current;
					if (questStaticChapter.DrawItemIdList != null)
					{
						List<DataManagerQuest.DrawItemData> list2 = this.questDrawList;
						List<DataManagerQuest.DrawItemData> drawItemIdList = questStaticChapter.DrawItemIdList;
						Predicate<DataManagerQuest.DrawItemData> predicate;
						if ((predicate = <>9__3) == null)
						{
							predicate = (<>9__3 = (DataManagerQuest.DrawItemData itm) => itm.EndDateTime > dt);
						}
						list2.AddRange(drawItemIdList.FindAll(predicate));
					}
				}
				goto IL_0128;
			}
		}
		this.questDrawList = null;
		IL_0128:
		if (this.questDrawList != null && this.questDrawList.Find((DataManagerQuest.DrawItemData itm) => itm.StartDateTime < dt && itm.EndDateTime > dt) != null)
		{
			list.Add("獲得アイテム<size=24><color=#FFEE00>追加!</color></size>");
		}
		DataManagerCampaign.CampaignKizunaData presentCampaignKizunaData = DataManager.DmCampaign.PresentCampaignKizunaData;
		if (presentCampaignKizunaData != null)
		{
			list.Add("なかよしPt<size=24><color=#FFEE00>" + PguiCmnMenuCtrl.Ratio2String(presentCampaignKizunaData.ratio) + "</color></size>倍！");
		}
		if (DataManager.DmCampaign.PresentCampaignQuestStaminaDataList.Count > 0)
		{
			list.Add("消費スタミナ減少！");
		}
		if (this.DispCampaign(this.guiData.Quest_Campaign, list, ref this.Quest_Campaign, ref this.Quest_Campaign_anm))
		{
			Transform transform = this.guiData.Quest_Campaign.transform.Find("Popup_Campaign_Cmn");
			Transform transform2 = this.guiData.Quest_Campaign.transform.Find("Popup_Campaign_Doujou");
			if (string.IsNullOrEmpty(list[this.Quest_Campaign]))
			{
				transform2.GetComponent<PguiImageCtrl>().m_Image.color = Color.white;
				foreach (object obj in transform2)
				{
					((Transform)obj).gameObject.SetActive(true);
				}
				transform.GetComponent<PguiImageCtrl>().m_Image.color = Color.clear;
				using (IEnumerator enumerator2 = transform.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						((Transform)obj2).gameObject.SetActive(false);
					}
					goto IL_0398;
				}
			}
			transform2.GetComponent<PguiImageCtrl>().m_Image.color = Color.clear;
			foreach (object obj3 in transform2)
			{
				((Transform)obj3).gameObject.SetActive(false);
			}
			transform.GetComponent<PguiImageCtrl>().m_Image.color = Color.white;
			foreach (object obj4 in transform)
			{
				((Transform)obj4).gameObject.SetActive(true);
			}
			transform.Find("Txt_Campaign").GetComponent<PguiTextCtrl>().text = list[this.Quest_Campaign];
		}
		IL_0398:
		if (this.isOpen)
		{
			if (this.eventList == null)
			{
				this.eventList = DataManager.DmEvent.GetEventDataListWithoutMissionEvent().FindAll((DataManagerEvent.EventData itm) => itm.eventCategory != DataManagerEvent.Category.SpecialPvp && itm.IsEnableChapter && itm.endDatetime > dt);
				this.eventList.Sort((DataManagerEvent.EventData a, DataManagerEvent.EventData b) => a.startDatetime.CompareTo(b.startDatetime));
			}
			if (this.pvpEvent == 0)
			{
				PvpStaticData pvpStaticDataBySeasonID = DataManager.DmPvp.GetPvpStaticDataBySeasonID(DataManager.DmPvp.GetSeasonIdByNow(dt, PvpStaticData.Type.SPECIAL));
				this.pvpEvent = ((pvpStaticDataBySeasonID != null && pvpStaticDataBySeasonID.seasonStartTime <= dt && dt <= pvpStaticDataBySeasonID.seasonEndTime) ? 1 : (-1));
			}
		}
		else
		{
			this.eventList = null;
			this.pvpEvent = 0;
		}
		if (this.eventList != null && this.eventList.Count > 0 && this.eventList[0].endDatetime <= dt)
		{
			this.eventList.RemoveAt(0);
		}
		this.guiData.Quest_Event.SetActive(this.eventList != null && this.eventList.Count > 0 && this.eventList[0].startDatetime <= dt);
		this.guiData.Pvp_Event.SetActive(this.pvpEvent > 0);
		list = new List<string>();
		DataManagerCampaign.CampaignGrowData campaignGrowData = DataManager.DmCampaign.PresentCampaignGrowCharaData;
		if (campaignGrowData != null && campaignGrowData.IsDisplayCampaign)
		{
			list.Add("フレンズ成長");
		}
		if ((campaignGrowData = DataManager.DmCampaign.PresentCampaignGrowPhotoData) != null && campaignGrowData.IsDisplayCampaign)
		{
			list.Add("フォト強化");
		}
		if (this.DispCampaign(this.guiData.CharaEdit_Campaign, list, ref this.CharaEdit_Campaign, ref this.CharaEdit_Campaign_anm))
		{
			this.guiData.CharaEdit_Campaign.transform.Find("Popup_Campaign_Friends/Txt_kind").GetComponent<PguiTextCtrl>().text = list[this.CharaEdit_Campaign];
		}
		list = new List<string>();
		List<DataManagerGacha.GachaPackData> gachaPackDataList = DataManager.DmGacha.GetGachaPackDataList();
		if (gachaPackDataList == null)
		{
			if (!DataManager.IsServerRequesting())
			{
				DataManager.DmGacha.RequestGetGachaList();
			}
		}
		else
		{
			if (this.isOpen)
			{
				if (this.userFlag == null)
				{
					this.userFlag = DataManager.DmGameStatus.MakeUserFlagData();
				}
				if (this.newGacha == null)
				{
					this.newGacha = this.userFlag.GachaNewInfoData.DisplayedIDList;
				}
			}
			else
			{
				this.userFlag = null;
				this.newGacha = null;
			}
			bool flag = false;
			bool flag2 = false;
			foreach (DataManagerGacha.GachaPackData gachaPackData in gachaPackDataList)
			{
				if (!(gachaPackData.staticData.endDatetime < dt) && (!gachaPackData.staticData.dayOfWeekFlg || !(gachaPackData.staticData.EndTimeOfDayOfWeek(dt) < dt)))
				{
					int num = 0;
					while (!flag && num < gachaPackData.dynamicData.gachaTypeData.Count && num < gachaPackData.staticData.typeDataList.Count)
					{
						DataManagerGacha.DiscountData discountData = gachaPackData.staticData.typeDataList[num].discountData;
						if (discountData != null && !(discountData.startDatetime > dt) && !(dt >= discountData.endDatetime) && (discountData.availableCount <= 0 || discountData.availableCount > gachaPackData.dynamicData.gachaTypeData[num].discountPlayNum) && gachaPackData.staticData.typeDataList[num].useItemNumber <= discountData.discountNum)
						{
							if (discountData.discountType == DataManagerGacha.DiscountType.OnceADay)
							{
								DateTime dateTime = new DateTime(dt.Year, dt.Month, dt.Day);
								if (gachaPackData.dynamicData.gachaTypeData[num].lastPlayDateTime >= dateTime)
								{
									goto IL_077B;
								}
							}
							flag = true;
						}
						IL_077B:
						num++;
					}
					if (this.newGacha == null || !this.newGacha.Contains(gachaPackData.gachaId))
					{
						flag2 = true;
					}
				}
			}
			if (flag)
			{
				list.Add(null);
			}
			if (this.isOpen)
			{
				this.guiData.Gacha_New.SetActive(flag2);
			}
		}
		bool activeSelf = this.guiData.Gacha_Campaign.activeSelf;
		this.guiData.Gacha_Campaign.SetActive(list.Count > 0 && this.isOpen);
		if (this.guiData.Gacha_Campaign.activeSelf)
		{
			this.guiData.Gacha_Campaign.transform.Find("Popup_Campaign_Cmn").gameObject.SetActive(false);
			this.guiData.Gacha_Campaign.transform.Find("Popup_Campaign_CmnRed").gameObject.SetActive(true);
			if (!activeSelf)
			{
				this.guiData.Gacha_Campaign.transform.Find("Popup_Campaign_CmnRed").GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
			}
		}
		this.guiData.Shop_Campaign.SetActive(false);
		list = new List<string>();
		DataManagerCampaign.CampaignPvpCoinData presentCampaignPvPCoinData = DataManager.DmCampaign.PresentCampaignPvPCoinData;
		if (presentCampaignPvPCoinData != null)
		{
			list.Add(string.Concat(new string[]
			{
				"獲得メダル<size=24><color=#",
				this.guiData.Pvp_Lock.gameObject.activeSelf ? "B2AFA4" : "FFEE00",
				">",
				PguiCmnMenuCtrl.Ratio2String(presentCampaignPvPCoinData.pvpCoinRatio),
				"</color></size>倍！"
			}));
		}
		if (this.DispCampaign(this.guiData.Pvp_Campaign, list, ref this.Pvp_Campaign, ref this.Pvp_Campaign_anm))
		{
			this.guiData.Pvp_Campaign.transform.Find("Popup_Campaign_Cmn/Txt_Campaign").GetComponent<PguiTextCtrl>().text = list[this.Pvp_Campaign];
		}
		list = new List<string>();
		DataManagerMonthlyPack.PurchaseMonthlypackData purchaseMonthlypackData = null;
		if (DataManager.DmMonthlyPack.nowPackData.EndDatetime >= dt)
		{
			purchaseMonthlypackData = DataManager.DmMonthlyPack.nowPackData.MonthlypackData;
		}
		else if (DataManager.DmMonthlyPack.nextPackData.EndDatetime >= dt)
		{
			purchaseMonthlypackData = DataManager.DmMonthlyPack.nextPackData.MonthlypackData;
		}
		if (purchaseMonthlypackData != null)
		{
			list.Add("月間パス効果中！");
		}
		if (DataManager.DmCampaign.PresentCampaignPicnicData != null)
		{
			list.Add(string.Concat(new string[]
			{
				"小さな輝石<size=24><color=#",
				this.guiData.Picnic_Lock.gameObject.activeSelf ? "B2AFA4" : "FFEE00",
				">",
				(DataManager.DmCampaign.PresentCampaignPicnicData.picnicBuffAddratio / 100).ToString(),
				"</color></size>倍！"
			}));
		}
		if (this.DispCampaign(this.guiData.Picnic_Campaign, list, ref this.Picnic_Campaign, ref this.Picnic_Campaign_anm))
		{
			this.guiData.Picnic_Campaign.transform.Find("Popup_Campaign_Cmn/Txt_Campaign").GetComponent<PguiTextCtrl>().text = list[this.Picnic_Campaign];
		}
		if (this.picnicLck == 0)
		{
			int num2 = 999;
			DateTime dateTime2 = dt;
			bool flag3 = false;
			if (DataManager.DmPicnic.IsEnablePicnicData)
			{
				num2 = DataManager.DmPicnic.PicnicDynamicData.Energy;
				dateTime2 = DataManager.DmPicnic.PicnicDynamicData.LastUpdateTime;
				using (List<DataManagerPicnic.CharaData>.Enumerator enumerator4 = DataManager.DmPicnic.PicnicDynamicData.CharaDataList.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						if (enumerator4.Current.CharaId > 0)
						{
							flag3 = true;
							break;
						}
					}
					goto IL_0BAC;
				}
			}
			DataManagerPicnic.MenuBadge menuBadgeData = DataManager.DmPicnic.MenuBadgeData;
			if (menuBadgeData == null)
			{
				if (!DataManager.IsServerRequesting())
				{
					DataManager.DmPicnic.RequestGetMenuBadgeData();
				}
			}
			else
			{
				num2 = menuBadgeData.Energy;
				dateTime2 = menuBadgeData.LastUpdateTime;
				flag3 = menuBadgeData.IsCharaSet;
			}
			IL_0BAC:
			this.guiData.Picnic_Badge.SetActive(num2 <= (flag3 ? ((int)(dt - dateTime2).TotalSeconds) : 0));
		}
		this.guiData.Home_Badge.SetActive(SelLoginBonus.rcvDate.Date != dt.Date);
		HomeCheckResult homeCheckResult2 = DataManager.DmHome.GetHomeCheckResult();
		this.guiData.TreeHouse_Badge.SetActive((this.userFlag != null && this.userFlag.TutorialFinishFlag.TreeHouseFirst != DataManagerGameStatus.UserFlagData.TREE_HOUSE_TUTORIAL.LATEST) || (homeCheckResult2 != null && (homeCheckResult2.treeHouseBadgeFlag || homeCheckResult2.IsTreeHouseCharge())));
	}

	// Token: 0x06001F8E RID: 8078 RVA: 0x00186A18 File Offset: 0x00184C18
	public static string Ratio2String(int ratio)
	{
		string text = (ratio / 10000 + 1).ToString();
		int num = ratio % 10000 / 100;
		if (num > 0)
		{
			text = text + "." + (num / 10).ToString();
			if ((num %= 10) > 0)
			{
				text += num.ToString();
			}
		}
		return text;
	}

	// Token: 0x06001F8F RID: 8079 RVA: 0x00186A78 File Offset: 0x00184C78
	private bool DispCampaign(GameObject obj, List<string> msg, ref int cnt, ref float frm)
	{
		bool activeSelf = obj.activeSelf;
		obj.SetActive(msg.Count > 0 && this.isOpen);
		if (obj.activeSelf)
		{
			SimpleAnimation[] componentsInChildren = obj.GetComponentsInChildren<SimpleAnimation>();
			if (componentsInChildren.Length != 0)
			{
				if (!activeSelf)
				{
					SimpleAnimation[] array = componentsInChildren;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
					}
				}
				else if (frm > componentsInChildren[0].ExGetTime())
				{
					cnt++;
				}
				frm = componentsInChildren[0].ExGetTime();
			}
			if (cnt >= msg.Count)
			{
				cnt = 0;
			}
		}
		return obj.activeSelf;
	}

	// Token: 0x06001F90 RID: 8080 RVA: 0x00186B08 File Offset: 0x00184D08
	private void UpdateMission()
	{
		if (!this.isOpen)
		{
			this.missionCnt = -1;
			return;
		}
		if (this.missionCnt >= 0)
		{
			return;
		}
		if ((this.missionCnt = DataManager.DmMission.GetUserClearMissionNum()) < 0)
		{
			this.missionCnt = 0;
		}
		this.guiData.Mission_New.SetActive(false);
		this.guiData.Mission_Badge.SetActive(this.missionCnt > 0);
		this.guiData.Mission_Badge_Num.text = this.missionCnt.ToString();
		List<int> list = new List<int> { -1, -1 };
		List<Transform> list2 = new List<Transform>
		{
			this.guiData.Btn_MissionEvent1.transform.Find("BaseImage"),
			this.guiData.Btn_MissionEvent2.transform.Find("BaseImage")
		};
		int num = 0;
		List<UserMissionOne> list3 = DataManager.DmMission.GetUserMissionGroupList().Find((UserMissionGroup itm) => itm.type == MissionType.BEGINNER).viewDataList;
		if (list3.FindAll((UserMissionOne itm) => !itm.isClear || !itm.Received).Count > 0)
		{
			list[num] = 0;
			Transform transform = list2[num++];
			transform.parent.gameObject.SetActive(true);
			transform.GetComponent<PguiRawImageCtrl>().SetRawImage("Texture2D/Mission_Icon/icon_mission_beginner", true, false, null);
			transform.Find("Mark_New").gameObject.SetActive(false);
			int count = list3.FindAll((UserMissionOne itm) => itm.isClear && !itm.Received).Count;
			transform.Find("Badges/Cmn_Badge").gameObject.SetActive(count > 0);
			transform.Find("Badges/Cmn_Badge/Num").GetComponent<PguiTextCtrl>().text = count.ToString();
		}
		List<DataManagerEvent.EventData> list4 = DataManager.DmEvent.GetEventDataList().FindAll((DataManagerEvent.EventData itm) => itm.startDatetime <= TimeManager.Now && TimeManager.Now <= itm.endDatetime && itm.eventMissionGroupId > 0 && itm.homeDispFlg);
		list4.Sort((DataManagerEvent.EventData a, DataManagerEvent.EventData b) => a.endDatetime.CompareTo(b.endDatetime));
		using (List<DataManagerEvent.EventData>.Enumerator enumerator = list4.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DataManagerEvent.EventData eventData = enumerator.Current;
				if (num >= list.Count)
				{
					break;
				}
				list3 = DataManager.DmMission.GetEventMissionGroup(eventData.eventId).viewDataList;
				if (list3.FindAll((UserMissionOne itm) => !itm.isClear || !itm.Received).Count > 0)
				{
					list[num] = eventData.eventId;
					Transform transform2 = list2[num++];
					transform2.parent.gameObject.SetActive(true);
					if (string.IsNullOrEmpty(eventData.missionIconFilename))
					{
						transform2.GetComponent<PguiRawImageCtrl>().SetRawImage("Texture2D/Mission_Icon/icon_mission_eventcmn", true, false, null);
					}
					else
					{
						transform2.GetComponent<PguiRawImageCtrl>().banner = "Texture2D/Mission_Icon/" + eventData.missionIconFilename;
					}
					transform2.Find("Mark_New").gameObject.SetActive(false);
					int count2 = list3.FindAll((UserMissionOne itm) => itm.isClear && !itm.Received).Count;
					transform2.Find("Badges/Cmn_Badge").gameObject.SetActive(count2 > 0);
					transform2.Find("Badges/Cmn_Badge/Num").GetComponent<PguiTextCtrl>().text = count2.ToString();
				}
			}
			goto IL_03C9;
		}
		IL_03AE:
		list2[num++].parent.gameObject.SetActive(false);
		IL_03C9:
		if (num >= list2.Count)
		{
			this.missionEventId1 = list[0];
			this.missionEventId2 = list[1];
			return;
		}
		goto IL_03AE;
	}

	// Token: 0x06001F91 RID: 8081 RVA: 0x00186F20 File Offset: 0x00185120
	private string pvpOpenMsg(bool isMark)
	{
		string text = SceneQuest.GetMainStoryName(this.pvpLock.questChapter.category, isMark);
		text += (isMark ? "\n" : " ");
		if (this.pvpLock != null)
		{
			return text + this.pvpLock.questChapter.chapterName + this.pvpLock.questGroup.titleName + "クリア";
		}
		return "クエスト情報がありません";
	}

	// Token: 0x06001F92 RID: 8082 RVA: 0x00186F94 File Offset: 0x00185194
	private string picnicOpenMsg(bool isMark)
	{
		string text = SceneQuest.GetMainStoryName(this.picnicLock.questChapter.category, isMark);
		text += (isMark ? "\n" : " ");
		if (this.picnicLock != null)
		{
			return text + this.picnicLock.questChapter.chapterName + this.picnicLock.questGroup.titleName + "クリア";
		}
		return "クエスト情報がありません";
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x00187007 File Offset: 0x00185207
	private void OnTouchRectIconStone(Transform rect)
	{
		CanvasManager.HdlSelPurchaseStoneWindowCtrl.Setup(PurchaseProductOne.TabType.Invalid);
	}

	// Token: 0x06001F94 RID: 8084 RVA: 0x00187014 File Offset: 0x00185214
	public GameObject GetQuestBtn()
	{
		return this.guiData.Btn_Quest.gameObject;
	}

	// Token: 0x06001F95 RID: 8085 RVA: 0x00187026 File Offset: 0x00185226
	public GameObject GetConfigBtn()
	{
		return this.guiData.Btn_Config.gameObject;
	}

	// Token: 0x06001F96 RID: 8086 RVA: 0x00187038 File Offset: 0x00185238
	private IEnumerator OpenBuffInfoWindow()
	{
		CanvasManager.HdlKizunaKizunaBuffWindowCtrl.SetupBuffInfo();
		CanvasManager.HdlKizunaKizunaBuffWindowCtrl.buffInfoData.owCtrl.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlKizunaKizunaBuffWindowCtrl.buffInfoData.owCtrl.FinishedClose());
		CanvasManager.HdlKizunaKizunaBuffWindowCtrl.buffInfoData.owCtrl.ForceClose();
		yield break;
	}

	// Token: 0x040016DE RID: 5854
	private IEnumerator windowCoroutine;

	// Token: 0x040016DF RID: 5855
	private PguiCmnMenuCtrl.GUI guiData;

	// Token: 0x040016E0 RID: 5856
	private bool isHome;

	// Token: 0x040016E1 RID: 5857
	private bool isOpen;

	// Token: 0x040016E2 RID: 5858
	private bool isRequestSwitch;

	// Token: 0x040016E3 RID: 5859
	private bool isRequestRetrun;

	// Token: 0x040016E4 RID: 5860
	private bool isEnableEvent;

	// Token: 0x040016E5 RID: 5861
	private bool isWinStamina;

	// Token: 0x040016E6 RID: 5862
	private PguiCmnMenuCtrl.OnClickReturnButton returnCallback;

	// Token: 0x040016E9 RID: 5865
	private SceneManager.SceneName requestNextScene;

	// Token: 0x040016EA RID: 5866
	private object nextSceneArgs;

	// Token: 0x040016EB RID: 5867
	private int Quest_Campaign;

	// Token: 0x040016EC RID: 5868
	private int CharaEdit_Campaign;

	// Token: 0x040016ED RID: 5869
	private int Gacha_Campaign;

	// Token: 0x040016EE RID: 5870
	private int Shop_Campaign;

	// Token: 0x040016EF RID: 5871
	private int Pvp_Campaign;

	// Token: 0x040016F0 RID: 5872
	private int Picnic_Campaign;

	// Token: 0x040016F1 RID: 5873
	private float Quest_Campaign_anm;

	// Token: 0x040016F2 RID: 5874
	private float CharaEdit_Campaign_anm;

	// Token: 0x040016F3 RID: 5875
	private float Gacha_Campaign_anm;

	// Token: 0x040016F4 RID: 5876
	private float Shop_Campaign_anm;

	// Token: 0x040016F5 RID: 5877
	private float Pvp_Campaign_anm;

	// Token: 0x040016F6 RID: 5878
	private float Picnic_Campaign_anm;

	// Token: 0x040016F7 RID: 5879
	private QuestOnePackData pvpLock;

	// Token: 0x040016F8 RID: 5880
	private QuestOnePackData picnicLock;

	// Token: 0x040016F9 RID: 5881
	private int pvpLck;

	// Token: 0x040016FA RID: 5882
	private int picnicLck;

	// Token: 0x040016FB RID: 5883
	private int missionCnt;

	// Token: 0x040016FC RID: 5884
	private int missionEventId1;

	// Token: 0x040016FD RID: 5885
	private int missionEventId2;

	// Token: 0x040016FE RID: 5886
	private DataManagerGameStatus.UserFlagData userFlag;

	// Token: 0x040016FF RID: 5887
	private HashSet<int> newGacha;

	// Token: 0x04001700 RID: 5888
	private List<DataManagerQuest.DrawItemData> questDrawList;

	// Token: 0x04001701 RID: 5889
	private List<DataManagerEvent.EventData> eventList;

	// Token: 0x04001702 RID: 5890
	private int pvpEvent;

	// Token: 0x0200100D RID: 4109
	// (Invoke) Token: 0x060051BF RID: 20927
	public delegate void OnClickReturnButton();

	// Token: 0x0200100E RID: 4110
	// (Invoke) Token: 0x060051C3 RID: 20931
	public delegate bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object nextSceneArgs);

	// Token: 0x0200100F RID: 4111
	// (Invoke) Token: 0x060051C7 RID: 20935
	public delegate void OnPlayAnimation(SimpleAnimation.ExPguiStatus uiType);

	// Token: 0x02001010 RID: 4112
	public class GUI
	{
		// Token: 0x060051CA RID: 20938 RVA: 0x0024760C File Offset: 0x0024580C
		public GUI(Transform baseTr)
		{
			this.Btn_Back = baseTr.Find("TitleBase/Btn_Back").GetComponent<PguiButtonCtrl>();
			this.Txt_Title = baseTr.Find("TitleBase/Txt_Title").GetComponent<PguiTextCtrl>();
			this.Txt_SubInfo = baseTr.Find("TitleBase/AssistInfo/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Btn_Question = baseTr.Find("TitleBase/Btn_Question").GetComponent<PguiButtonCtrl>();
			this.Btn_Menu = baseTr.Find("Header/Btn_Menu").GetComponent<PguiButtonCtrl>();
			this.Btn_Quest = baseTr.Find("FooterMenu/Btn_01").GetComponent<PguiButtonCtrl>();
			this.Btn_CharaEdit = baseTr.Find("FooterMenu/Btn_02").GetComponent<PguiButtonCtrl>();
			this.Btn_Gacha = baseTr.Find("FooterMenu/Btn_03").GetComponent<PguiButtonCtrl>();
			this.Btn_Shop = baseTr.Find("FooterMenu/Btn_04").GetComponent<PguiButtonCtrl>();
			this.Btn_Pvp = baseTr.Find("FooterMenu/Btn_05").GetComponent<PguiButtonCtrl>();
			this.Btn_Picnic = baseTr.Find("FooterMenu/Btn_06").GetComponent<PguiButtonCtrl>();
			this.Btn_News = baseTr.Find("HomeMenu/Btn_01").GetComponent<PguiButtonCtrl>();
			this.Btn_Present = baseTr.Find("HomeMenu/Btn_02").GetComponent<PguiButtonCtrl>();
			this.Btn_Config = baseTr.Find("HomeMenu/Btn_03").GetComponent<PguiButtonCtrl>();
			this.Btn_Follow = baseTr.Find("HomeMenu/Btn_04").GetComponent<PguiButtonCtrl>();
			this.Btn_Mission = baseTr.Find("HomeMenu/Btn_05").GetComponent<PguiButtonCtrl>();
			this.Btn_Movie = baseTr.Find("HomeMenu/LayoutGroup/Toggle/Btn_06").GetComponent<PguiButtonCtrl>();
			this.Btn_TreeHouse = baseTr.Find("HomeMenu/LayoutGroup/Toggle/Btn_07").GetComponent<PguiButtonCtrl>();
			this.Btn_MissionEvent1 = baseTr.Find("HomeMenu/LayoutGroup/Btn_08").GetComponent<PguiButtonCtrl>();
			this.Btn_MissionEvent2 = baseTr.Find("HomeMenu/LayoutGroup/Btn_09").GetComponent<PguiButtonCtrl>();
			this.Btn_Profile = baseTr.Find("Header/InfoAll/Btn_Profile").GetComponent<PguiButtonCtrl>();
			this.Btn_KizunaBuff = baseTr.Find("HomeMenu/LayoutGroup/Btn_10").GetComponent<PguiButtonCtrl>();
			this.Present_Badge = this.Btn_Present.transform.Find("BaseImage/Badges/Cmn_Badge").gameObject;
			this.Present_Badge_Num = this.Btn_Present.transform.Find("BaseImage/Badges/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
			this.Mission_New = this.Btn_Mission.transform.Find("BaseImage/Mark_New").gameObject;
			this.Mission_Badge = this.Btn_Mission.transform.Find("BaseImage/Badges/Cmn_Badge").gameObject;
			this.Mission_Badge_Num = this.Btn_Mission.transform.Find("BaseImage/Badges/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
			this.Btn_Home = baseTr.Find("Btn_Home").GetComponent<PguiButtonCtrl>();
			this.RankGage_Gage = baseTr.Find("Header/InfoAll/RankGage_base/RankGage_Gage").GetComponent<PguiImageCtrl>();
			this.Txt_Rank = baseTr.Find("Header/InfoAll/Txt_Rank").GetComponent<PguiTextCtrl>();
			this.Num_Rank = baseTr.Find("Header/InfoAll/Num_Rank").GetComponent<PguiTextCtrl>();
			this.StaminaGage_Gage = baseTr.Find("Header/InfoAll/StaminaGage/StaminaGage_Gage").GetComponent<PguiImageCtrl>();
			this.Num_Stamina = baseTr.Find("Header/InfoAll/StaminaGage/Num_Stamina").GetComponent<PguiTextCtrl>();
			this.Num_Stamina.GetComponent<Text>().supportRichText = true;
			this.Nxt_Stamina = baseTr.Find("Header/InfoAll/StaminaGage/Next_Stamina").GetComponent<PguiTextCtrl>();
			this.Num_Stone = baseTr.Find("Header/InfoAll/StoneInfo/Num_Stone").GetComponent<PguiTextCtrl>();
			this.BuyStone = baseTr.Find("Header/InfoAll/Btn_BuyStone").gameObject;
			this.GUI_CmnMenu = baseTr.GetComponent<SimpleAnimation>();
			this.TitleBase = baseTr.Find("TitleBase").GetComponent<SimpleAnimation>();
			this.TouchMask = baseTr.Find("OverBg").gameObject;
			this.TouchMask.SetActive(false);
			this.TouchMask.GetComponent<Image>().raycastTarget = true;
			this.buttonList = new List<PguiButtonCtrl>(baseTr.GetComponentsInChildren<PguiButtonCtrl>());
			this.colliList = new List<PguiCollider>(baseTr.GetComponentsInChildren<PguiCollider>());
			this.GroupObjTitle = baseTr.Find("TitleBase").gameObject;
			this.GroupObjHome = baseTr.Find("HomeMenu").gameObject;
			this.GroupObjOther = baseTr.Find("Btn_Home").gameObject;
			this.GroupObjInfoAll = baseTr.Find("Header/InfoAll").gameObject;
			this.Btn_Stamina = baseTr.Find("Header/InfoAll/Collision_Stamina").gameObject;
			this.Win_Stamina = baseTr.Find("Header/InfoAll/Window_Stamina").gameObject;
			this.Quest_Campaign = this.Btn_Quest.transform.Find("BaseImage/Popup_Campaign").gameObject;
			this.CharaEdit_Campaign = this.Btn_CharaEdit.transform.Find("BaseImage/Popup_Campaign").gameObject;
			this.Gacha_Campaign = this.Btn_Gacha.transform.Find("BaseImage/Popup_Campaign").gameObject;
			this.Shop_Campaign = this.Btn_Shop.transform.Find("BaseImage/Popup_Campaign").gameObject;
			this.Pvp_Campaign = this.Btn_Pvp.transform.Find("BaseImage/Popup_Campaign").gameObject;
			this.Picnic_Campaign = this.Btn_Picnic.transform.Find("BaseImage/Popup_Campaign").gameObject;
			this.Pvp_Lock = this.Btn_Pvp.transform.Find("BaseImage/Mark_Lock").GetComponent<MarkLockCtrl>();
			this.Picnic_Lock = this.Btn_Picnic.transform.Find("BaseImage/Mark_Lock").GetComponent<MarkLockCtrl>();
			this.ImgViewMode = baseTr.Find("Img_Viewmode").GetComponent<SimpleAnimation>();
			this.Popup_Limited = baseTr.Find("Header/InfoAll/popup_Limited").GetComponent<SimpleAnimation>();
			this.Popup_Limited.gameObject.SetActive(false);
			this.Popup_StaminaRecovery_Campaign = baseTr.Find("Header/InfoAll/popup_Campaign_StaminaRecovery").GetComponent<SimpleAnimation>();
			this.Txt_StaminaRecovery_Campaign = this.Popup_StaminaRecovery_Campaign.transform.Find("Txt_Campaign").GetComponent<PguiTextCtrl>();
			this.Popup_StaminaRecovery_Campaign.gameObject.SetActive(false);
			this.Quest_Event = this.Btn_Quest.transform.Find("BaseImage/Mark_Event").gameObject;
			this.Quest_Event.SetActive(false);
			this.Gacha_New = this.Btn_Gacha.transform.Find("BaseImage/Cmn_Mark_New").gameObject;
			this.Gacha_New.SetActive(false);
			this.Picnic_Badge = this.Btn_Picnic.transform.Find("BaseImage/Cmn_Badge").gameObject;
			this.Picnic_Badge.transform.Find("Num").GetComponent<PguiTextCtrl>().text = "!";
			this.Picnic_Badge.SetActive(false);
			this.Home_Badge = this.Btn_Home.transform.Find("BaseImage/Cmn_Badge").gameObject;
			this.Home_Badge.transform.Find("Num").GetComponent<PguiTextCtrl>().text = "!";
			this.Home_Badge.SetActive(false);
			this.Pvp_Event = this.Btn_Pvp.transform.Find("BaseImage/Mark_Event").gameObject;
			this.Pvp_Event.SetActive(false);
			this.TreeHouse_Badge = this.Btn_TreeHouse.transform.Find("BaseImage/Badges/Cmn_Badge").gameObject;
			this.TreeHouse_Badge.transform.Find("Num").GetComponent<PguiTextCtrl>().text = "!";
			this.TreeHouse_Badge.SetActive(false);
		}

		// Token: 0x04005A0D RID: 23053
		public PguiButtonCtrl Btn_Back;

		// Token: 0x04005A0E RID: 23054
		public PguiButtonCtrl Btn_Question;

		// Token: 0x04005A0F RID: 23055
		public PguiButtonCtrl Btn_Menu;

		// Token: 0x04005A10 RID: 23056
		public PguiButtonCtrl Btn_Quest;

		// Token: 0x04005A11 RID: 23057
		public PguiButtonCtrl Btn_CharaEdit;

		// Token: 0x04005A12 RID: 23058
		public PguiButtonCtrl Btn_Gacha;

		// Token: 0x04005A13 RID: 23059
		public PguiButtonCtrl Btn_Shop;

		// Token: 0x04005A14 RID: 23060
		public PguiButtonCtrl Btn_Pvp;

		// Token: 0x04005A15 RID: 23061
		public PguiButtonCtrl Btn_Picnic;

		// Token: 0x04005A16 RID: 23062
		public PguiButtonCtrl Btn_News;

		// Token: 0x04005A17 RID: 23063
		public PguiButtonCtrl Btn_Present;

		// Token: 0x04005A18 RID: 23064
		public PguiButtonCtrl Btn_Config;

		// Token: 0x04005A19 RID: 23065
		public PguiButtonCtrl Btn_Follow;

		// Token: 0x04005A1A RID: 23066
		public PguiButtonCtrl Btn_Mission;

		// Token: 0x04005A1B RID: 23067
		public PguiButtonCtrl Btn_MissionEvent1;

		// Token: 0x04005A1C RID: 23068
		public PguiButtonCtrl Btn_MissionEvent2;

		// Token: 0x04005A1D RID: 23069
		public PguiButtonCtrl Btn_Home;

		// Token: 0x04005A1E RID: 23070
		public PguiButtonCtrl Btn_Profile;

		// Token: 0x04005A1F RID: 23071
		public PguiButtonCtrl Btn_Movie;

		// Token: 0x04005A20 RID: 23072
		public PguiButtonCtrl Btn_TreeHouse;

		// Token: 0x04005A21 RID: 23073
		public PguiButtonCtrl Btn_KizunaBuff;

		// Token: 0x04005A22 RID: 23074
		public PguiImageCtrl RankGage_Gage;

		// Token: 0x04005A23 RID: 23075
		public PguiTextCtrl Txt_Rank;

		// Token: 0x04005A24 RID: 23076
		public PguiTextCtrl Num_Rank;

		// Token: 0x04005A25 RID: 23077
		public PguiImageCtrl StaminaGage_Gage;

		// Token: 0x04005A26 RID: 23078
		public PguiTextCtrl Num_Stamina;

		// Token: 0x04005A27 RID: 23079
		public PguiTextCtrl Nxt_Stamina;

		// Token: 0x04005A28 RID: 23080
		public PguiTextCtrl Num_Stone;

		// Token: 0x04005A29 RID: 23081
		public GameObject BuyStone;

		// Token: 0x04005A2A RID: 23082
		public SimpleAnimation GUI_CmnMenu;

		// Token: 0x04005A2B RID: 23083
		public SimpleAnimation TitleBase;

		// Token: 0x04005A2C RID: 23084
		public GameObject TouchMask;

		// Token: 0x04005A2D RID: 23085
		public PguiTextCtrl Txt_Title;

		// Token: 0x04005A2E RID: 23086
		public PguiTextCtrl Txt_SubInfo;

		// Token: 0x04005A2F RID: 23087
		public List<PguiButtonCtrl> buttonList;

		// Token: 0x04005A30 RID: 23088
		public List<PguiCollider> colliList = new List<PguiCollider>();

		// Token: 0x04005A31 RID: 23089
		public GameObject Mission_New;

		// Token: 0x04005A32 RID: 23090
		public GameObject Mission_Badge;

		// Token: 0x04005A33 RID: 23091
		public PguiTextCtrl Mission_Badge_Num;

		// Token: 0x04005A34 RID: 23092
		public GameObject Present_Badge;

		// Token: 0x04005A35 RID: 23093
		public PguiTextCtrl Present_Badge_Num;

		// Token: 0x04005A36 RID: 23094
		public GameObject GroupObjTitle;

		// Token: 0x04005A37 RID: 23095
		public GameObject GroupObjHome;

		// Token: 0x04005A38 RID: 23096
		public GameObject GroupObjOther;

		// Token: 0x04005A39 RID: 23097
		public GameObject GroupObjInfoAll;

		// Token: 0x04005A3A RID: 23098
		public GameObject Btn_Stamina;

		// Token: 0x04005A3B RID: 23099
		public GameObject Win_Stamina;

		// Token: 0x04005A3C RID: 23100
		public GameObject Quest_Campaign;

		// Token: 0x04005A3D RID: 23101
		public GameObject CharaEdit_Campaign;

		// Token: 0x04005A3E RID: 23102
		public GameObject Gacha_Campaign;

		// Token: 0x04005A3F RID: 23103
		public GameObject Shop_Campaign;

		// Token: 0x04005A40 RID: 23104
		public GameObject Pvp_Campaign;

		// Token: 0x04005A41 RID: 23105
		public GameObject Picnic_Campaign;

		// Token: 0x04005A42 RID: 23106
		public MarkLockCtrl Pvp_Lock;

		// Token: 0x04005A43 RID: 23107
		public MarkLockCtrl Picnic_Lock;

		// Token: 0x04005A44 RID: 23108
		public SimpleAnimation ImgViewMode;

		// Token: 0x04005A45 RID: 23109
		public SimpleAnimation Popup_Limited;

		// Token: 0x04005A46 RID: 23110
		public SimpleAnimation Popup_StaminaRecovery_Campaign;

		// Token: 0x04005A47 RID: 23111
		public PguiTextCtrl Txt_StaminaRecovery_Campaign;

		// Token: 0x04005A48 RID: 23112
		public GameObject Quest_Event;

		// Token: 0x04005A49 RID: 23113
		public GameObject Gacha_New;

		// Token: 0x04005A4A RID: 23114
		public GameObject Picnic_Badge;

		// Token: 0x04005A4B RID: 23115
		public GameObject Home_Badge;

		// Token: 0x04005A4C RID: 23116
		public GameObject Pvp_Event;

		// Token: 0x04005A4D RID: 23117
		public GameObject TreeHouse_Badge;
	}
}
