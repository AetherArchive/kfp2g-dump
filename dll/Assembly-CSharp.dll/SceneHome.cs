using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SGNFW.Common;
using SGNFW.Mst;
using SGNFW.Touch;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneHome : BaseScene
{
	private bool IsNight
	{
		get
		{
			return this.stageType > SceneHome.StageType.EVENING;
		}
	}

	public static void StartNotice()
	{
		SceneHome.notice = 0;
		SceneHome.monthlyNotice = 0;
		SceneHome.purchaseNotice = 0;
	}

	public override void OnCreateScene()
	{
		this.basePanel = AssetManager.InstantiateAssetData("SceneHome/GUI/Prefab/GUI_Home", null);
		PguiPanel pguiPanel = this.basePanel.GetComponent<PguiPanel>();
		if (pguiPanel != null)
		{
			pguiPanel.raycastTarget = false;
		}
		this.viewPanel = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_CharaCamera", null);
		pguiPanel = this.viewPanel.GetComponent<PguiPanel>();
		if (pguiPanel != null)
		{
			pguiPanel.raycastTarget = false;
		}
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.viewPanel.transform, true);
		this.windowPanel = AssetManager.InstantiateAssetData("SceneHome/GUI/Prefab/GUI_Home_Window", null);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.windowPanel.transform, true);
		this.windowPanel.transform.SetAsLastSibling();
		pguiPanel = this.windowPanel.GetComponent<PguiPanel>();
		if (pguiPanel != null)
		{
			pguiPanel.raycastTarget = false;
		}
		this.listPanelH = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_CharaCamera_Window_H", null);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.listPanelH.transform, true);
		this.listPanelH.transform.SetAsLastSibling();
		pguiPanel = this.listPanelH.GetComponent<PguiPanel>();
		if (pguiPanel != null)
		{
			pguiPanel.raycastTarget = false;
		}
		this.listPanelV = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_CharaCamera_Window_V", null);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.listPanelV.transform, true);
		this.listPanelV.transform.SetAsLastSibling();
		pguiPanel = this.listPanelV.GetComponent<PguiPanel>();
		if (pguiPanel != null)
		{
			pguiPanel.raycastTarget = false;
		}
		this.rankUpPanel = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneBattleResult/GUI/Prefab/GUI_BattleResult_Window"));
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.rankUpPanel.transform, true);
		this.rankUpPanel.transform.SetAsFirstSibling();
		pguiPanel = this.rankUpPanel.GetComponent<PguiPanel>();
		if (pguiPanel != null)
		{
			pguiPanel.raycastTarget = false;
		}
		this.rankupWindow = this.rankUpPanel.transform.Find("Window_PlayerRankUp").GetComponent<PguiOpenWindowCtrl>();
		this.rankWinBefore = this.rankupWindow.m_UserInfoContent.Find("Txt_Rank_Before").GetComponent<PguiTextCtrl>();
		this.rankWinAfter = this.rankupWindow.m_UserInfoContent.Find("Txt_Rank_After").GetComponent<PguiTextCtrl>();
		this.hidePanel = new GameObject("hide").transform;
		this.hidePanel.SetParent(this.basePanel.transform, false);
		this.hidePanel.gameObject.SetActive(false);
		this.charaPanel = AssetManager.InstantiateAssetData("SceneCharaEdit/GUI/Prefab/CharaGrow_Btn_CharaSelect", null);
		this.charaPanel.transform.SetParent(this.hidePanel, false);
		this.bgmPanel = AssetManager.InstantiateAssetData("SceneHome/GUI/Prefab/Home_BGM_Btn_BGMSelect", null);
		this.bgmPanel.transform.SetParent(this.hidePanel, false);
		this.homeField = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneHome/FieldSceneHome"));
		SceneManager.Add3DObjectByBaseField(this.homeField.transform);
		this.camera = this.homeField.GetComponentInChildren<Camera>(true).gameObject.AddComponent<FieldCameraScaler>();
		this.camBCG = this.camera.GetComponent<CC_BrightnessContrastGamma>();
		if (this.camBCG != null)
		{
			this.camBCG.enabled = false;
		}
		this.furnitureCtrl = new GameObject("furnitureCtrl", new Type[] { typeof(HomeFurnitureCtrl) }).GetComponent<HomeFurnitureCtrl>();
		this.furnitureCtrl.transform.SetParent(this.homeField.transform, false);
		this._homeAuthCtrl = new HomeAuthCtrl();
		if (DataManager.DmIntroduction.IsPlayable())
		{
			List<int> list = (from item in DataManager.DmIntroduction.GetEnableIntroductionList()
				select item.charaId).ToList<int>();
			bool characterFlag = DataManager.DmIntroduction.GetEnableIntroductionList()[0].characterFlag;
			this._introduction = this._homeAuthCtrl.IntroductionFriends(characterFlag, list, delegate
			{
				this.homeField.SetActive(false);
			}, delegate
			{
				this._introduction = null;
				this.homeField.SetActive(true);
				this.SetupMenu();
				List<DataManagerIntroductionNewChara.IntroductionPlayableCharaData> enableIntroductionList = DataManager.DmIntroduction.GetEnableIntroductionList();
				string jsonPlayedIntroductionList = DataManager.DmIntroduction.GetJsonPlayedIntroductionList();
				DataManager.DmUserInfo.RequestActionUpdateIntroductionLastId(enableIntroductionList[enableIntroductionList.Count - 1].introductionId, jsonPlayedIntroductionList);
				SoundManager.PlayBGM(SceneHome.homeBgm);
				HomeCharaCtrl homeCharaCtrl = this.charaCtrl;
				if (homeCharaCtrl == null)
				{
					return;
				}
				homeCharaCtrl.ReplayAnimation();
			});
		}
		this.charaCtrl = new GameObject("CharaCtrl", new Type[] { typeof(HomeCharaCtrl) }).GetComponent<HomeCharaCtrl>();
		this.charaCtrl.transform.SetParent(this.homeField.transform, false);
		this.bannerCtrl = this.basePanel.transform.Find("HomeBanner").gameObject.AddComponent<HomeBannerCtrl>();
		this.bannerCtrl.Init();
		this.btnMonthlyPack = this.basePanel.transform.Find("HomeBanner/Btn_Passport").GetComponent<PguiButtonCtrl>();
		this.btnMonthlyPack.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMonthlyPack), PguiButtonCtrl.SoundType.DEFAULT);
		this.monthlyPack = null;
		this.monthlyDays = -99;
		this.monthly = -1;
		this.monthlyMission = -1;
		this.bigBannerCtrl = this.basePanel.transform.Find("BigBanner").gameObject.AddComponent<HomeBigBannerCtrl>();
		this.bigBannerCtrl.Init();
		this.friendPoint = this.windowPanel.transform.Find("Window_FriendPoint").gameObject;
		this.monthlyNoticeWindow = this.windowPanel.transform.Find("Window_Passport").GetComponent<PguiOpenWindowCtrl>();
		this.monthlyNoticeMark = this.monthlyNoticeWindow.m_UserInfoContent.Find("BaseImage/Img_Check").gameObject;
		this.monthlyNoticeWindow.m_UserInfoContent.GetComponent<PguiButtonCtrl>().AddOnClickListener(delegate(PguiButtonCtrl pbc)
		{
			if (SceneHome.monthlyNotice == 1)
			{
				this.monthlyNoticeMark.SetActive(!this.monthlyNoticeMark.activeSelf);
			}
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.leftButton = this.basePanel.transform.Find("LeftTopBtns").GetComponent<SimpleAnimation>();
		this.leftButton.transform.Find("Btn_Furniture").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFurniture), PguiButtonCtrl.SoundType.DEFAULT);
		this.leftButton.transform.Find("Btn_Closet").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickCloset), PguiButtonCtrl.SoundType.DEFAULT);
		this.leftButton.transform.Find("Btn_View").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFunitureCamera), PguiButtonCtrl.SoundType.DEFAULT);
		this.leftButton.transform.Find("Btn_CharaCome").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickCharaCome), PguiButtonCtrl.SoundType.DEFAULT);
		this.leftButton.transform.Find("Btn_TreeHouse").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickTreeHouse), PguiButtonCtrl.SoundType.DEFAULT);
		this.furnitureBadge = this.leftButton.transform.Find("Btn_Furniture/BaseImage/Cmn_Badge_Exclam").gameObject;
		this.treehouseBadge = this.leftButton.transform.Find("Btn_TreeHouse/Cmn_Badge_Exclam").gameObject;
		this.onClick = 0;
		this.appEnd = -1;
		this.appEndObj = new GameObject("AppEnd", new Type[] { typeof(RectTransform) });
		RectTransform component = this.appEndObj.GetComponent<RectTransform>();
		component.SetParent(this.basePanel.transform, false);
		component.sizeDelta = new Vector2(1280f, 720f);
		this.appEndObj.AddComponent<PguiCollider>().raycastTarget = true;
		this.friendsMenu = this.basePanel.transform.Find("FriendsMenu").GetComponent<SimpleAnimation>();
		this.friendsMenu.transform.Find("Btn_01").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFriendsCloset), PguiButtonCtrl.SoundType.DEFAULT);
		this.friendsMenu.transform.Find("Btn_02").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFriendsFriends), PguiButtonCtrl.SoundType.DEFAULT);
		this.friendsMenu.transform.Find("Btn_03").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFriendsDetail), PguiButtonCtrl.SoundType.DEFAULT);
		this.friendsMenu.transform.Find("Btn_04").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFriendsGlow), PguiButtonCtrl.SoundType.DEFAULT);
		this.friendsId = -1;
		this.friendsTouch = 0;
		this.friendsMenuTime = 0f;
		this.modeCloset = this.basePanel.transform.Find("Mode_Closet").GetComponent<SimpleAnimation>();
		this.modeClosetBack = this.modeCloset.transform.Find("Mode_Title/Btn_Back").GetComponent<PguiButtonCtrl>();
		this.modeClosetBack.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickClosetReturn), PguiButtonCtrl.SoundType.DEFAULT);
		this.closet = -1;
		this.haveCharaPackList = new List<CharaPackData>();
		this.dispCharaPackList = new List<CharaPackData>();
		this.sortType = SortFilterDefine.SortType.LEVEL;
		this.closetChara = new List<Transform>();
		this.closetScroll = this.modeCloset.transform.Find("WindowAll/CharaAll/ScrollView").GetComponent<ReuseScroll>();
		this.closetScroll.InitForce();
		ReuseScroll reuseScroll = this.closetScroll;
		reuseScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onStartItem, new Action<int, GameObject>(this.SetupClosetChara));
		ReuseScroll reuseScroll2 = this.closetScroll;
		reuseScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll2.onUpdateItem, new Action<int, GameObject>(this.SetupClosetChara));
		this.closetScroll.Setup(0, 0);
		this.characome = -1;
		this.bgmList = new List<DataManagerHome.HomeBgmPlaybackData>();
		this.bgmId = 0;
		this.bgmLineup = new List<Transform>();
		this.modeBgm = this.basePanel.transform.Find("Mode_BGM").GetComponent<SimpleAnimation>();
		this.modeBgmBack = this.modeBgm.transform.Find("Mode_Title/Btn_Back").GetComponent<PguiButtonCtrl>();
		this.modeBgmBack.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickBgmLineupReturn), PguiButtonCtrl.SoundType.DEFAULT);
		this.chgBgm = -1;
		this.bgmScroll = this.modeBgm.transform.Find("WindowAll/BGMAll/ScrollView").GetComponent<ReuseScroll>();
		this.bgmScroll.InitForce();
		ReuseScroll reuseScroll3 = this.bgmScroll;
		reuseScroll3.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll3.onStartItem, new Action<int, GameObject>(this.SetupBgmLineup));
		ReuseScroll reuseScroll4 = this.bgmScroll;
		reuseScroll4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll4.onUpdateItem, new Action<int, GameObject>(this.SetupBgmLineup));
		this.bgmScroll.Setup(0, 0);
		this.viewAnm = this.viewPanel.transform.Find("Horizontal").GetComponent<SimpleAnimation>();
		this.viewAnm.transform.Find("Btn_Back").GetComponent<PguiToggleButtonCtrl>().AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickBack));
		this.viewAnm.transform.Find("Btn_Screen").GetComponent<PguiToggleButtonCtrl>().AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickScreen));
		this.viewAnm.transform.Find("Btn_On").GetComponent<PguiToggleButtonCtrl>().AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickOn));
		this.viewAnm.transform.Find("Btn_Position").GetComponent<PguiToggleButtonCtrl>().AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickPosition));
		this.viewAnm.transform.Find("Btn_List").GetComponent<PguiToggleButtonCtrl>().AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickList));
		this.viewName = this.viewAnm.transform.Find("Serif").gameObject;
		this.viewNameTim = 0f;
		this.viewFade = this.viewPanel.transform.Find("Fade").GetComponent<PguiRawImageCtrl>();
		this.viewAnm.transform.Find("Btn_Screen").gameObject.SetActive(false);
		this.haveContactPackList = new List<CharaContactStatic>();
		this.notContactPackList = new List<CharaContactStatic>();
		this.dispCharaPackList = new List<CharaPackData>();
		this.contactSituationList = new List<CharaContactStatic.Situation>();
		this.contactSituation = 0;
		this.motListWinH = this.listPanelH.transform.Find("Window_MotionList").GetComponent<PguiOpenWindowCtrl>();
		this.motListWinV = this.listPanelV.transform.Find("Window_MotionList").GetComponent<PguiOpenWindowCtrl>();
		this.motListWinV.AddCloseListener();
		this.motListScrollH = this.motListWinH.transform.Find("Base/Window/MotionList/ScrollViewAll/ScrollView").GetComponent<ReuseScroll>();
		this.motListScrollH.InitForce();
		ReuseScroll reuseScroll5 = this.motListScrollH;
		reuseScroll5.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll5.onStartItem, new Action<int, GameObject>(this.SetupMotListH));
		ReuseScroll reuseScroll6 = this.motListScrollH;
		reuseScroll6.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll6.onUpdateItem, new Action<int, GameObject>(this.UpdateMotListH));
		this.motListScrollH.Setup(0, 0);
		this.motListScrollV = this.motListWinV.transform.Find("Base/Window/MotionList/ScrollViewAll/ScrollView").GetComponent<ReuseScroll>();
		this.motListScrollV.InitForce();
		ReuseScroll reuseScroll7 = this.motListScrollV;
		reuseScroll7.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll7.onStartItem, new Action<int, GameObject>(this.SetupMotListV));
		ReuseScroll reuseScroll8 = this.motListScrollV;
		reuseScroll8.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll8.onUpdateItem, new Action<int, GameObject>(this.UpdateMotListV));
		this.motListScrollV.Setup(0, 0);
		this.actListScrollH = this.motListWinH.transform.Find("Base/Window/ActionList/ScrollViewAll").GetComponent<ReuseScroll>();
		this.actListScrollH.InitForce();
		ReuseScroll reuseScroll9 = this.actListScrollH;
		reuseScroll9.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll9.onStartItem, new Action<int, GameObject>(this.SetupActList));
		ReuseScroll reuseScroll10 = this.actListScrollH;
		reuseScroll10.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll10.onUpdateItem, new Action<int, GameObject>(this.UpdateActList));
		this.actListScrollH.Setup(0, 0);
		this.actListScrollV = this.motListWinV.transform.Find("Base/Window/ActionList/ScrollViewAll").GetComponent<ReuseScroll>();
		this.actListScrollV.InitForce();
		ReuseScroll reuseScroll11 = this.actListScrollV;
		reuseScroll11.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll11.onStartItem, new Action<int, GameObject>(this.SetupActList));
		ReuseScroll reuseScroll12 = this.actListScrollV;
		reuseScroll12.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll12.onUpdateItem, new Action<int, GameObject>(this.UpdateActList));
		this.actListScrollV.Setup(0, 0);
		this.basePanel.SetActive(false);
		this.viewPanel.SetActive(false);
		this.viewName.SetActive(true);
		this.windowPanel.SetActive(false);
		this.listPanelH.SetActive(false);
		this.listPanelV.SetActive(false);
		this.bannerCtrl.gameObject.SetActive(false);
		this.bigBannerCtrl.gameObject.SetActive(false);
		this.friendPoint.SetActive(false);
		this.friendsMenu.gameObject.SetActive(false);
		this.modeCloset.gameObject.SetActive(false);
		this.modeBgm.gameObject.SetActive(false);
		this.homeField.SetActive(false);
		this.motListWinH.gameObject.SetActive(false);
		this.motListWinV.gameObject.SetActive(false);
		this.rankUpPanel.transform.Find("Auth_HeartLvUp").gameObject.SetActive(false);
		this.bonus = null;
		this.furnitureMap = null;
		this.furnitureNew = new List<int>();
		this.questNotice = 0;
		this.questNoClear = true;
		this.growRewardInfo = -1;
		this.firstDownload = -1;
		this.downloadResolver = null;
		AssetManager.LoadAssetData(SceneHome.STAGE_ROOM_LOCATOR, AssetManager.OWNER.HomeStage, 0, null);
		this.stageLocator = null;
		this.stageCtrl = null;
		this.stageLight = null;
		this.stageLoad = null;
		this.stageType = SceneHome.StageType.INVALID;
		EffectManager.ReqLoadEffect(SceneHome.tvEffName, AssetManager.OWNER.HomeStage, 0, null);
		this.tvEff = null;
		EffectManager.ReqLoadEffect(SceneHome.bgmEffName, AssetManager.OWNER.HomeStage, 0, null);
		this.bgmEff = null;
	}

	private bool CheckButton()
	{
		return this.questNotice == 0 && this.growRewardInfo == 0 && this.firstDownload == 0 && SceneHome.notice < 0 && SceneHome.monthlyNotice < 0 && SceneHome.purchaseNotice < 0 && this.monthly == 0 && this.onClick == 0 && this.appEnd <= 0 && !this.appEndObj.activeSelf && this.viewChara == null && this.quest_guide == 0 && this.chgBgm == 0 && !this.modeBgm.gameObject.activeSelf && this.closet == 0 && !this.modeCloset.gameObject.activeSelf && this.characome == 0 && !this.furnitureCtrl.isActive && this.friendsId == 0 && !this.friendsMenu.gameObject.activeSelf;
	}

	private void OnClickFurniture(PguiButtonCtrl button)
	{
		if (this.CheckButton())
		{
			this.onClick = -1;
		}
	}

	private void OnClickCloset(PguiButtonCtrl button)
	{
		if (this.CheckButton())
		{
			this.onClick = -2;
		}
	}

	private bool CloseCloset()
	{
		if ((this.closet > 0 || this.characome == 1) && this.modeCloset.gameObject.activeSelf)
		{
			this.modeCloset.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			this.closet = 0;
			this.characome = 0;
			return true;
		}
		return false;
	}

	private void OnClickClosetReturn(PguiButtonCtrl button)
	{
		this.CloseCloset();
	}

	private void OnClickClosetCharaShort(GameObject go)
	{
		int id = int.Parse(go.name);
		if (id > 0)
		{
			CharaPackData cpd = this.haveCharaPackList.Find((CharaPackData itm) => itm.id == id);
			if (cpd != null)
			{
				if (this.closet > 0)
				{
					CanvasManager.HdlDressUpWipeCtrl.Play(delegate
					{
						CanvasManager.HdlDressUpWindowCtrl.Open(cpd, new DressUpWindowCtrl.OpenParameter(DressUpWindowCtrl.OpenParameter.Preset.HOME_LIST, this.dispCharaPackList));
					});
					return;
				}
				if (this.characome == 1 && CanvasManager.HdlOpenWindowBasic.FinishedClose())
				{
					this.characome = 2;
					string text = ((this.stayFriends == id) ? "待機フレンズの設定を解除しますか？" : "待機フレンズに設定されたフレンズは\nホーム画面で固定で登場するようになります\n設定しますか？");
					CanvasManager.HdlOpenWindowBasic.Setup("待機フレンズ設定", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.CharaComeWindow), null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
					this.stayFriends = ((this.stayFriends == id) ? 0 : id);
				}
			}
		}
	}

	private bool CharaComeWindow(int index)
	{
		if (this.characome == 2)
		{
			if (index == 1)
			{
				this.characome = 3;
			}
			else
			{
				this.characome = 1;
				this.stayFriends = DataManager.DmUserInfo.optionData.StayFriendsId;
			}
		}
		return true;
	}

	private void OnClickClosetCharaLong(GameObject go)
	{
		int id = int.Parse(go.name);
		if (id > 0)
		{
			CharaPackData charaPackData = this.haveCharaPackList.Find((CharaPackData itm) => itm.id == id);
			if (charaPackData != null && (this.closet > 0 || this.characome == 1))
			{
				CanvasManager.HdlCharaWindowCtrl.Open(charaPackData, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.HOME_LIST, this.dispCharaPackList), null);
			}
		}
	}

	private void SetupClosetChara(int index, GameObject go)
	{
		List<Transform> list = new List<Transform>();
		foreach (object obj in go.transform)
		{
			Transform transform = (Transform)obj;
			list.Add(transform);
		}
		foreach (Transform transform2 in list)
		{
			transform2.SetParent(this.hidePanel, false);
		}
		int num = index * 3;
		int num2 = num + 3;
		int i = num;
		Predicate<Transform> <>9__0;
		while (i < num2 && i < this.dispCharaPackList.Count)
		{
			List<Transform> list2 = this.closetChara;
			Predicate<Transform> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (Transform itm) => itm.name == this.dispCharaPackList[i].id.ToString());
			}
			Transform transform3 = list2.Find(predicate);
			if (transform3 != null)
			{
				transform3.SetParent(go.transform, false);
				transform3.GetComponent<IconCharaCtrl>().Setup(this.dispCharaPackList[i], this.sortType, false, null, 0, -1, 0);
				if (this.characome == 1 && this.stayFriends == this.dispCharaPackList[i].id)
				{
					transform3.GetComponent<IconCharaCtrl>().DispCurrentFrame(true);
				}
			}
			int j = i;
			i = j + 1;
		}
	}

	private void OnClickFunitureCamera(PguiButtonCtrl button)
	{
		this.clickCamera = true;
	}

	private void OnClickCharaCome(PguiButtonCtrl button)
	{
		if (this.CheckButton())
		{
			this.onClick = -5;
		}
	}

	private void OnClickTreeHouse(PguiButtonCtrl button)
	{
		if (this.CheckButton())
		{
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneTreeHouse, null);
		}
	}

	private bool CloseFriendsMenu()
	{
		if (this.friendsMenu.gameObject.activeSelf && this.friendsId > 0)
		{
			this.friendsMenu.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			this.friendsId = 0;
			return true;
		}
		return false;
	}

	private void OnClickFriendsCloset(PguiButtonCtrl button)
	{
		if (this.friendsData != null && !CanvasManager.HdlCharaWindowCtrl.IsActive() && !CanvasManager.HdlDressUpWipeCtrl.IsActive() && !CanvasManager.HdlDressUpWindowCtrl.IsActive())
		{
			CanvasManager.HdlDressUpWipeCtrl.Play(delegate
			{
				if (this.friendsData != null)
				{
					CanvasManager.HdlDressUpWindowCtrl.Open(this.friendsData, new DressUpWindowCtrl.OpenParameter(DressUpWindowCtrl.OpenParameter.Preset.HOME, null));
				}
			});
		}
	}

	private void OnClickFriendsFriends(PguiButtonCtrl button)
	{
		if (this.friendsData != null && !CanvasManager.HdlCharaWindowCtrl.IsActive() && !CanvasManager.HdlDressUpWipeCtrl.IsActive() && !CanvasManager.HdlDressUpWindowCtrl.IsActive())
		{
			if (this.CloseFriendsMenu())
			{
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneHome, new SceneHome.Args
				{
					charaPackData = this.friendsData,
					sceneName = SceneManager.SceneName.SceneHome
				});
			}
			this.friendsData = null;
			this.friendsTouch = 0;
		}
	}

	private void OnClickFriendsDetail(PguiButtonCtrl button)
	{
		if (this.friendsData != null && !CanvasManager.HdlCharaWindowCtrl.IsActive() && !CanvasManager.HdlDressUpWipeCtrl.IsActive() && !CanvasManager.HdlDressUpWindowCtrl.IsActive())
		{
			CanvasManager.HdlCharaWindowCtrl.Open(this.friendsData, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.HOME_DRESS, null), null);
		}
	}

	private void OnClickFriendsGlow(PguiButtonCtrl button)
	{
		if (this.friendsData != null && !CanvasManager.HdlCharaWindowCtrl.IsActive() && !CanvasManager.HdlDressUpWipeCtrl.IsActive() && !CanvasManager.HdlDressUpWindowCtrl.IsActive())
		{
			if (this.CloseFriendsMenu())
			{
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneCharaEdit, new SceneCharaEdit.Args
				{
					growCharaId = this.friendsData.id
				});
			}
			this.friendsData = null;
			this.friendsTouch = 0;
		}
	}

	private bool OnClickBack(PguiToggleButtonCtrl toggle, int index)
	{
		if (!this.viewEnd && this.viewChara != null && !this.hideView && !this.motListWinH.gameObject.activeSelf && !this.motListWinV.gameObject.activeSelf)
		{
			this.viewEnd = true;
			this.vertView = false;
			SceneHome.nowVertView = false;
			SoundManager.Play("prd_se_cancel", false, false);
		}
		return false;
	}

	private bool OnClickScreen(PguiToggleButtonCtrl toggle, int index)
	{
		return false;
	}

	private bool OnClickOn(PguiToggleButtonCtrl toggle, int index)
	{
		if (!this.viewEnd && this.viewChara != null && !this.hideView && !this.motListWinH.gameObject.activeSelf && !this.motListWinV.gameObject.activeSelf)
		{
			this.hideView = true;
			this.viewAnm.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.charaCtrl.SetOpe();
			SoundManager.Play("prd_se_menu_slide", false, false);
		}
		return false;
	}

	private bool OnClickPosition(PguiToggleButtonCtrl toggle, int index)
	{
		if (!this.viewEnd && this.viewChara != null && !this.hideView && !this.motListWinH.gameObject.activeSelf && !this.motListWinV.gameObject.activeSelf)
		{
			this.viewChg = true;
			this.charaCtrl.SetOpe();
			SoundManager.Play("prd_se_click", false, false);
		}
		return false;
	}

	private bool OnClickList(PguiToggleButtonCtrl toggle, int index)
	{
		if (!this.viewEnd && this.viewChara != null && !this.hideView && !this.motListWinH.gameObject.activeSelf && !this.motListWinV.gameObject.activeSelf && this.contactSituationList.Count > 0)
		{
			this.MakeMotList();
			PguiOpenWindowCtrl pguiOpenWindowCtrl = ((SafeAreaScaler.ScreenWidth < SafeAreaScaler.ScreenHeight) ? this.motListWinV : this.motListWinH);
			pguiOpenWindowCtrl.gameObject.SetActive(true);
			pguiOpenWindowCtrl.Setup(null, null, null, true, null, null, false);
			pguiOpenWindowCtrl.ForceOpen();
			this.charaCtrl.SetOpe();
			SoundManager.Play("prd_se_click", false, false);
		}
		return false;
	}

	private void MakeMotList()
	{
		this.dispContactPackList = new List<CharaContactStatic>();
		this.dispContactPackList.AddRange(this.haveContactPackList.FindAll((CharaContactStatic itm) => itm.SituationType == this.contactSituationList[this.contactSituation]));
		this.dispContactPackList.AddRange(this.notContactPackList.FindAll((CharaContactStatic itm) => itm.SituationType == this.contactSituationList[this.contactSituation]));
		this.motListScrollH.Resize((this.dispContactPackList.Count + 1) / 2, 0);
		this.motListScrollV.Resize(this.dispContactPackList.Count, 0);
		string text = (SceneHome.situationName.ContainsKey(this.contactSituationList[this.contactSituation]) ? SceneHome.situationName[this.contactSituationList[this.contactSituation]] : "");
		this.motListWinH.WindowRectTransform.Find("ActionInfo/Txt_Info").GetComponent<PguiTextCtrl>().text = text;
		this.motListWinV.WindowRectTransform.Find("ActionInfo/Txt_Info").GetComponent<PguiTextCtrl>().text = text;
	}

	private void SetupBgmLineup(int index, GameObject go)
	{
		List<Transform> list = new List<Transform>();
		foreach (object obj in go.transform)
		{
			Transform transform = (Transform)obj;
			list.Add(transform);
		}
		foreach (Transform transform2 in list)
		{
			transform2.SetParent(this.hidePanel, false);
		}
		int num = index * 3;
		int num2 = num + 3;
		int i = num;
		Predicate<Transform> <>9__0;
		while (i < num2 && i < this.bgmList.Count)
		{
			List<Transform> list2 = this.bgmLineup;
			Predicate<Transform> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (Transform itm) => itm.name == this.bgmList[i].id.ToString());
			}
			Transform transform3 = list2.Find(predicate);
			if (transform3 != null)
			{
				transform3.SetParent(go.transform, false);
			}
			int j = i;
			i = j + 1;
		}
	}

	private void OnClickBgmLineup(GameObject go)
	{
		int id = (go.transform.Find("BaseImage/Disable").gameObject.activeSelf ? 0 : int.Parse(go.name));
		if (id > 0)
		{
			DataManagerHome.HomeBgmPlaybackData homeBgmPlaybackData = this.bgmList.Find((DataManagerHome.HomeBgmPlaybackData itm) => itm.id == id);
			if (homeBgmPlaybackData != null && homeBgmPlaybackData.id != this.bgmId)
			{
				Transform transform = this.bgmLineup.Find((Transform itm) => itm.name == this.bgmId.ToString());
				if (transform != null)
				{
					transform.Find("BaseImage/Playing").gameObject.SetActive(false);
				}
				this.bgmId = homeBgmPlaybackData.id;
				SoundManager.PlayBGM(Path.GetFileName(homeBgmPlaybackData.fileName), 500, 500, 0);
				transform = this.bgmLineup.Find((Transform itm) => itm.name == this.bgmId.ToString());
				if (transform != null)
				{
					transform.Find("BaseImage/Playing").gameObject.SetActive(true);
				}
			}
		}
	}

	private bool CloseBgmLineup()
	{
		if (this.chgBgm > 0 && this.modeBgm.gameObject.activeSelf)
		{
			this.modeBgm.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			this.chgBgm = 0;
			return true;
		}
		return false;
	}

	private void OnClickBgmLineupReturn(PguiButtonCtrl button)
	{
		this.CloseBgmLineup();
	}

	private void OnClickMonthlyPack(PguiButtonCtrl button)
	{
		if (this.CheckButton())
		{
			CanvasManager.HdlSelMonthlyPackAfterWindowCtrl.Setup();
			this.monthly = 1;
		}
	}

	private void SetupMotListH(int index, GameObject go)
	{
		this.SetupMotList(index * 2, go.transform.GetChild(0));
		this.SetupMotList(index * 2 + 1, go.transform.GetChild(1));
	}

	private void UpdateMotListH(int index, GameObject go)
	{
		int num = index * 2;
		Transform transform = go.transform.GetChild(0);
		if (index >= 0 && num < this.dispContactPackList.Count)
		{
			transform.gameObject.SetActive(true);
			this.UpdateMotList(num, transform);
		}
		else
		{
			transform.gameObject.SetActive(false);
		}
		num++;
		transform = go.transform.GetChild(1);
		if (index >= 0 && num < this.dispContactPackList.Count)
		{
			transform.gameObject.SetActive(true);
			this.UpdateMotList(num, transform);
			return;
		}
		transform.gameObject.SetActive(false);
	}

	private void SetupMotListV(int index, GameObject go)
	{
		this.SetupMotList(index, go.transform);
	}

	private void UpdateMotListV(int index, GameObject go)
	{
		if (index >= 0 && index < this.dispContactPackList.Count)
		{
			this.UpdateMotList(index, go.transform);
		}
	}

	private void SetupMotList(int index, Transform trs)
	{
		trs.Find("Cmn_Mark_New").gameObject.SetActive(false);
	}

	private void UpdateMotList(int index, Transform trs)
	{
		CharaContactStatic charaContactStatic = this.dispContactPackList[index];
		trs.Find("Txt_ItemName").GetComponent<PguiTextCtrl>().text = charaContactStatic.GetName();
		bool flag = this.viewChara.dynamicData.haveContactItemIdList.Contains(charaContactStatic.GetId());
		trs.Find("Icon_Item").GetComponent<IconItemCtrl>().Setup(new ItemData(charaContactStatic.GetId(), 0), new IconItemCtrl.SetupParam
		{
			useInfo = true,
			viewItemCount = false
		});
		trs.Find("Icon_Item").GetComponent<IconItemCtrl>().SetActEnable(flag);
		trs.Find("Disable").gameObject.SetActive(!flag);
	}

	private void SetupActList(int index, GameObject go)
	{
		go.GetComponent<PguiToggleButtonCtrl>().AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickActList));
		go.AddComponent<PguiDataHolder>();
		go.transform.Find("BaseImage/Badge").gameObject.SetActive(false);
	}

	private void UpdateActList(int index, GameObject go)
	{
		if (index >= 0 && index < this.contactSituationList.Count)
		{
			go.GetComponent<PguiToggleButtonCtrl>().SetToggleIndex((index == this.contactSituation) ? 1 : 0);
			go.GetComponent<PguiDataHolder>().id = index;
			go.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = (go.GetComponent<PguiDataHolder>().str = (SceneHome.situationName.ContainsKey(this.contactSituationList[index]) ? SceneHome.situationName[this.contactSituationList[index]] : ""));
		}
	}

	private bool OnClickActList(PguiToggleButtonCtrl toggle, int index)
	{
		int id = toggle.GetComponent<PguiDataHolder>().id;
		if (this.contactSituation == id)
		{
			return false;
		}
		SoundManager.Play("prd_se_click", false, false);
		this.contactSituation = id;
		foreach (object obj in toggle.transform.parent)
		{
			PguiToggleButtonCtrl component = ((Transform)obj).GetComponent<PguiToggleButtonCtrl>();
			component.SetToggleIndex((component == toggle) ? 1 : 0);
		}
		this.MakeMotList();
		return false;
	}

	public override bool OnCreateSceneWait()
	{
		if (!AssetManager.IsLoadFinishAssetData(SceneHome.STAGE_ROOM_LOCATOR))
		{
			return false;
		}
		if (!EffectManager.IsLoadFinishEffect(SceneHome.tvEffName) || !EffectManager.IsLoadFinishEffect(SceneHome.bgmEffName))
		{
			return false;
		}
		this.stageLocator = AssetManager.InstantiateAssetData(SceneHome.STAGE_ROOM_LOCATOR, null);
		this.stageLocator.transform.SetParent(this.homeField.transform, false);
		this.stageLocator.layer = LayerMask.NameToLayer("FieldStage");
		this.stageLocator.SetActive(false);
		AssetManager.AddLoadList(SceneHome.STAGE_ROOM_LOCATOR, AssetManager.OWNER.HomeStage);
		this.camera.fieldCamera.cullingMask = 1 << this.stageLocator.layer;
		this.camNo = 0;
		this.camPos = new List<Vector3>();
		this.camRot = new List<Vector3>();
		this.camMin = new List<Vector3>();
		this.camMax = new List<Vector3>();
		char c = 'a';
		for (;;)
		{
			Transform transform = this.stageLocator.transform.Find("cam_pos_" + c.ToString());
			if (transform == null)
			{
				break;
			}
			this.camPos.Add(transform.localPosition);
			Vector3 vector = transform.localEulerAngles;
			vector.x = Mathf.DeltaAngle(0f, vector.x);
			vector.y = Mathf.DeltaAngle(0f, vector.y);
			vector.z = this.camera.fieldOfView;
			this.camRot.Add(vector);
			transform = this.stageLocator.transform.Find(transform.name + "_param");
			Vector3 vector2 = vector;
			Vector3 vector3 = vector;
			if (transform != null)
			{
				if (transform.localPosition.x < transform.localPosition.y)
				{
					vector2.x += transform.localPosition.x;
					vector3.x += transform.localPosition.y;
				}
				else
				{
					vector2.x += transform.localPosition.y;
					vector3.x += transform.localPosition.x;
				}
				vector = transform.localEulerAngles;
				vector.x = Mathf.DeltaAngle(0f, vector.x);
				vector.y = Mathf.DeltaAngle(0f, vector.y);
				if (vector.x < vector.y)
				{
					vector2.y += vector.x;
					vector3.y += vector.y;
				}
				else
				{
					vector2.y += vector.y;
					vector3.y += vector.x;
				}
				if (transform.localScale.x < transform.localScale.y)
				{
					vector2.z = transform.localScale.x;
					vector3.z = transform.localScale.y;
				}
				else
				{
					vector2.z = transform.localScale.y;
					vector3.z = transform.localScale.x;
				}
			}
			this.camMin.Add(vector2);
			this.camMax.Add(vector3);
			c += '\u0001';
		}
		if (this.camPos.Count <= 0)
		{
			this.camPos.Add(this.camera.transform.localPosition);
			Vector3 localEulerAngles = this.camera.transform.localEulerAngles;
			localEulerAngles.x = Mathf.DeltaAngle(0f, localEulerAngles.x);
			localEulerAngles.y = Mathf.DeltaAngle(0f, localEulerAngles.y);
			localEulerAngles.z = this.camera.fieldOfView;
			this.camRot.Add(localEulerAngles);
			this.camMin.Add(localEulerAngles);
			this.camMax.Add(localEulerAngles);
		}
		this.viewNo = 0;
		this.viewPos = new List<List<Transform>>();
		char c2 = 'a';
		for (;;)
		{
			Transform transform2 = this.stageLocator.transform.Find("cam_pos_view_" + c2.ToString());
			if (transform2 == null)
			{
				break;
			}
			List<Transform> list = new List<Transform>();
			list.Add(transform2);
			transform2 = this.stageLocator.transform.Find(transform2.name + "_param");
			if (!(transform2 == null))
			{
				list.Add(transform2);
				transform2 = this.stageLocator.transform.Find(transform2.name.Replace("_param", "_char"));
				if (transform2 == null)
				{
					if (c2 == 'c')
					{
						transform2 = this.stageLocator.transform.Find("pos_bed_a");
					}
					else if (c2 == 'd')
					{
						transform2 = this.stageLocator.transform.Find("pos_chair_a");
					}
					if (transform2 == null)
					{
						goto IL_04DF;
					}
				}
				list.Add(transform2);
				this.viewPos.Add(list);
			}
			IL_04DF:
			c2 += '\u0001';
		}
		this.viewChg = false;
		this.viewEnd = false;
		this.furnitureCtrl.Init(this.stageLocator, this.basePanel.transform, this.windowPanel.transform);
		this.charaCtrl.Init(this.stageLocator, this.camera.fieldCamera, this.furnitureCtrl);
		return true;
	}

	public override void OnEnableScene(object args)
	{
		this.args = args as SceneHome.Args;
		this.viewChara = ((this.args == null) ? null : this.args.charaPackData);
		this.quest_guide = ((this.args == null || this.args.tutorialSequence != TutorialUtil.Sequence.QUEST_GUIDE) ? 0 : 1);
		CanvasManager.SetBgTexture("selbg_home_out");
		CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpId(501);
		CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
		this.leftButton.gameObject.SetActive(false);
		this.bannerCtrl.gameObject.SetActive(false);
		this.bigBannerCtrl.gameObject.SetActive(false);
		this.bgmList = new List<DataManagerHome.HomeBgmPlaybackData>();
		this.bgmId = -1;
		SoundManager.PlayBGM(SceneHome.homeBgm);
		EffectManager.BillboardCamera = this.camera.fieldCamera;
		SGNFW.Touch.Manager.RegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
		SGNFW.Touch.Manager.RegisterRelease(new SGNFW.Touch.Manager.SingleAction(this.OnTouchEnd));
		SGNFW.Touch.Manager.RegisterMove(new SGNFW.Touch.Manager.SingleAction(this.OnTouchMove));
		SGNFW.Touch.Manager.RegisterPinch(new SGNFW.Touch.Manager.DoubleAction(this.OnPinch));
		SGNFW.Touch.Manager.RegisterMouseWheel(new SGNFW.Touch.Manager.WheelAction(this.OnWheel));
		SGNFW.Touch.Manager.RegisterTap(new SGNFW.Touch.Manager.SingleAction(this.OnTap));
		this.touchView = false;
		this.moveView = Vector2.zero;
		this.stroke = false;
		this.pinchView = 0f;
		this.pinchFov = this.camera.fieldOfView;
		this.wheelView = 0f;
		this.viewItr = this.stageLocator.transform.position;
		this.viewItr.y = (this.viewHeight = 0.85f);
		this.viewLen = 3f;
		this.viewCam = this.viewItr + new Vector3(0f, 0.1f, -this.viewLen);
		this.viewBas = (this.viewRot = Vector2.zero);
		this.viewMin = (this.viewMax = Vector3.zero);
		if (this.viewChara == null)
		{
			this.camNo = 0;
			this.camera.transform.localPosition = this.camPos[this.camNo];
			Vector3 vector = this.camRot[this.camNo];
			this.camera.fieldOfView = vector.z;
			vector.z = 0f;
			this.camera.transform.localEulerAngles = vector;
		}
		else
		{
			this.viewNo = 0;
			float num = 50f;
			if (this.viewNo < this.viewPos.Count)
			{
				this.viewCam = this.viewPos[this.viewNo][0].position;
				this.viewItr = this.viewPos[this.viewNo][2].position;
				this.viewItr.y = this.viewItr.y + this.viewHeight;
				this.camera.transform.localPosition = this.viewCam;
				this.camera.transform.LookAt(this.viewItr);
				this.viewLen = Vector3.Magnitude(this.viewCam - this.viewItr);
				this.viewBas = new Vector2(Mathf.DeltaAngle(0f, this.camera.transform.eulerAngles.x), Mathf.DeltaAngle(0f, this.camera.transform.eulerAngles.y));
				Transform transform = this.viewPos[this.viewNo][1];
				this.viewMin = new Vector3(-transform.localScale.x, -transform.localScale.y, transform.localScale.z);
				this.viewMax = new Vector3(-transform.localPosition.x, -transform.localPosition.y, transform.localPosition.z);
				num = (this.viewMin.z + this.viewMax.z) * 0.5f;
			}
			else
			{
				this.camera.transform.localPosition = this.viewCam;
				this.camera.transform.LookAt(this.viewItr);
			}
			this.camera.fieldOfView = num;
		}
		this.camera.fieldCamera.depth = -10f;
		this.vertView = false;
		SceneHome.nowVertView = false;
		this.vertViewChg = 0f;
		this.hideView = false;
		PguiToggleButtonCtrl[] componentsInChildren = this.viewAnm.transform.GetComponentsInChildren<PguiToggleButtonCtrl>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetToggleIndex(0);
		}
		this.viewChg = false;
		this.viewEnd = false;
		this.viewFade.m_RawImage.color = new Color(1f, 1f, 1f, 0f);
		this.basePanel.SetActive(true);
		this.windowPanel.SetActive(true);
		this.listPanelH.SetActive(true);
		this.listPanelV.SetActive(true);
		this.homeField.SetActive(true);
		if (this.viewChara != null)
		{
			this.viewPanel.SetActive(true);
			this.viewAnm.gameObject.SetActive(true);
			this.viewAnm.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
			this.camera.fieldCamera.depth = (float)((SceneManager.CameraDepth[SceneManager.CanvasType.BACK] + SceneManager.CameraDepth[SceneManager.CanvasType.FRONT]) / 2);
			CanvasManager.SetBgEnable(false);
			this.viewName.transform.Find("Name/Txt_Name").GetComponent<PguiTextCtrl>().text = this.viewChara.staticData.baseData.charaName;
			this.viewNameTim = 0f;
		}
		else if (this.quest_guide != 0)
		{
			this.SetupMenu();
			CanvasManager.HdlTutorialMaskCtrl.SetEnable(true);
			CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(true);
			CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
			CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
			CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		}
		this.stageType = SceneHome.StageType.INVALID;
		this.stageCtrl = null;
		this.stageLight = null;
		this.stageLoad = null;
		this.DispRoom();
		this.furnitureMap = new List<HomeFurnitureMapping>();
		foreach (HomeFurnitureMapping homeFurnitureMapping in DataManager.DmHome.GetUserHomeeFurnitureMappingList())
		{
			this.furnitureMap.Add(new HomeFurnitureMapping
			{
				placementId = homeFurnitureMapping.placementId,
				furnitureId = homeFurnitureMapping.furnitureId
			});
		}
		this.furnitureNew = new List<int>();
		List<HomeFurniturePackData> userHomeFurnitureList = DataManager.DmHome.GetUserHomeFurnitureList();
		using (List<HomeFurnitureMapping>.Enumerator enumerator = this.furnitureMap.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				HomeFurnitureMapping hfm = enumerator.Current;
				if (userHomeFurnitureList.Find((HomeFurniturePackData itm) => itm.id == hfm.furnitureId) == null || DataManager.DmHome.GetHomeFurnitureStaticData(hfm.furnitureId) == null)
				{
					hfm.furnitureId = 0;
				}
			}
		}
		this.furnitureCtrl.Setup(this.furnitureMap, this.furnitureNew);
		this.charaCtrl.SetViewPos((this.viewNo < this.viewPos.Count) ? this.viewPos[this.viewNo][2] : null);
		this.charaCtrl.Setup(this.furnitureMap, this.viewChara, this.IsNight, this.stayFriends = DataManager.DmUserInfo.optionData.StayFriendsId);
		this.furnitureBadge.SetActive(false);
		this.treehouseBadge.SetActive(false);
		this.onClick = 0;
		this.appEnd = -1;
		this.appEndObj.SetActive(false);
		this.friendsId = -1;
		this.friendsTouch = 0;
		this.closet = -1;
		this.characome = -1;
		this.chgBgm = -1;
		this.monthlyPack = null;
		this.monthly = -1;
		this.monthlyDays = -99;
		this.monthlyMission = -1;
		this.friendsMenu.gameObject.SetActive(false);
		this.modeCloset.gameObject.SetActive(false);
		this.modeBgm.gameObject.SetActive(false);
		this.friendsMenuTime = 0f;
		this.questNotice = 0;
		this.questNoClear = this.viewChara == null && this.quest_guide == 0 && DataManager.DmQuest.GetQuestOnePackData(10010101).questDynamicOne.clearNum <= 0;
		this.questNoClearIcon = CanvasManager.HdlCmnMenu.GetQuestBtn().transform.Find("Icon_tutorial").gameObject;
		this.questNoClearIcon.SetActive(false);
		this.haveCharaPackList = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
		this.dispCharaPackList = new List<CharaPackData>(this.haveCharaPackList);
		this.closetChara = new List<Transform>();
		foreach (CharaPackData charaPackData in this.dispCharaPackList)
		{
			GameObject obj2 = Object.Instantiate<GameObject>(this.charaPanel);
			obj2.transform.SetParent(this.hidePanel, false);
			obj2.name = charaPackData.id.ToString();
			if (obj2.GetComponent<PguiTouchTrigger>() == null)
			{
				obj2.AddComponent<PguiTouchTrigger>().AddListener(delegate
				{
					this.OnClickClosetCharaShort(obj2);
				}, delegate
				{
					this.OnClickClosetCharaLong(obj2);
				}, null, null, null);
			}
			obj2.GetComponent<IconCharaCtrl>().Setup(charaPackData, this.sortType, false, null, 0, -1, 0);
			this.closetChara.Add(obj2.transform);
		}
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.HOME_CLOSET,
			filterButton = this.modeCloset.transform.Find("WindowAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>(),
			sortButton = this.modeCloset.transform.Find("WindowAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>(),
			sortUdButton = this.modeCloset.transform.Find("WindowAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>(),
			funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
			{
				charaList = this.haveCharaPackList
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispCharaPackList = item.charaList;
				this.sortType = item.sortType;
				this.closetScroll.Resize((this.dispCharaPackList.Count + 3 - 1) / 3, 0);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
		this.bgmLineup = new List<Transform>();
		foreach (DataManagerHome.HomeBgmPlaybackData homeBgmPlaybackData in DataManager.DmHome.GetMstBgmPlaybackDataList())
		{
			if (SceneHome.homeBgm == Path.GetFileName(homeBgmPlaybackData.fileName))
			{
				this.bgmId = homeBgmPlaybackData.id;
			}
			GameObject obj = Object.Instantiate<GameObject>(this.bgmPanel);
			obj.transform.SetParent(this.hidePanel, false);
			obj.name = homeBgmPlaybackData.id.ToString();
			obj.transform.Find("BaseImage/Txt_BGMTitle").GetComponent<PguiTextCtrl>().text = homeBgmPlaybackData.name;
			obj.transform.Find("BaseImage/Playing").gameObject.SetActive(homeBgmPlaybackData.id == this.bgmId);
			PguiTextCtrl component = obj.transform.Find("BaseImage/Disable/Txt_Clear").GetComponent<PguiTextCtrl>();
			string text = "";
			if (homeBgmPlaybackData.anyTime <= 0)
			{
				if (homeBgmPlaybackData.level > DataManager.DmUserInfo.level)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "\n";
					}
					text = text + "探検隊Lv." + homeBgmPlaybackData.level.ToString() + "で解放";
				}
				if (homeBgmPlaybackData.friendsCount > DataManager.DmChara.GetUserCharaMap().Count)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "\n";
					}
					text = text + "合計" + homeBgmPlaybackData.friendsCount.ToString() + "人のフレンズが加入すると解放";
				}
				QuestOnePackData questOnePackData = ((homeBgmPlaybackData.questId > 0) ? DataManager.DmQuest.GetQuestOnePackData(homeBgmPlaybackData.questId) : null);
				if (questOnePackData != null && questOnePackData.questDynamicOne.clearNum <= 0)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "\n";
					}
					if (questOnePackData.questChapter.category == QuestStaticChapter.Category.STORY)
					{
						text = string.Concat(new string[]
						{
							text,
							SceneQuest.GetMainStoryName(questOnePackData.questChapter.category, false),
							"\n",
							questOnePackData.questChapter.chapterName,
							questOnePackData.questGroup.titleName,
							"クリアで解放"
						});
					}
					else if (questOnePackData.questChapter.category == QuestStaticChapter.Category.CHARA)
					{
						text = text + questOnePackData.questChapter.chapterName + questOnePackData.questGroup.titleName + "クリアで解放";
					}
					else if (questOnePackData.questChapter.category == QuestStaticChapter.Category.SIDE_STORY)
					{
						text = text + questOnePackData.questChapter.chapterName + questOnePackData.questGroup.titleName + "クリアで解放";
					}
					else
					{
						text = text + questOnePackData.questOne.questName + "クリアで解放";
					}
				}
			}
			component.text = text;
			component.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(text));
			string text2 = homeBgmPlaybackData.imgPath;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "icon_Home_BGM_001";
			}
			obj.transform.Find("BaseImage/Texture_Chara").GetComponent<PguiRawImageCtrl>().SetRawImage("Texture2D/Home_BGM/" + text2, true, false, null);
			if (obj.GetComponent<PguiTouchTrigger>() == null)
			{
				obj.AddComponent<PguiTouchTrigger>().AddListener(delegate
				{
					this.OnClickBgmLineup(obj);
				}, null, null, null, null);
			}
			obj.GetComponent<PguiButtonCtrl>().SetActEnable(string.IsNullOrEmpty(text), false, false);
			this.bgmLineup.Add(obj.transform);
		}
		if (this.viewChara == null && this.quest_guide == 0)
		{
			DataManager.DmHome.RequestCheckHome();
		}
		this.growRewardInfo = -1;
		this.firstDownload = -1;
		this.downloadResolver = null;
		this.ChkMonthly();
		this.haveContactPackList = new List<CharaContactStatic>();
		this.notContactPackList = new List<CharaContactStatic>();
		this.dispContactPackList = new List<CharaContactStatic>();
		this.contactSituationList = new List<CharaContactStatic.Situation>();
		this.contactSituation = 0;
		if (this.viewChara != null)
		{
			this.haveContactPackList = DataManager.DmChara.GetContactByChara(this.viewChara.id);
			this.notContactPackList = this.haveContactPackList.FindAll((CharaContactStatic itm) => !this.viewChara.dynamicData.haveContactItemIdList.Contains(itm.GetId()) && itm.IsNotHaveDisp);
			this.haveContactPackList.RemoveAll((CharaContactStatic itm) => !this.viewChara.dynamicData.haveContactItemIdList.Contains(itm.GetId()));
			SortedSet<CharaContactStatic.Situation> sortedSet = new SortedSet<CharaContactStatic.Situation>();
			foreach (CharaContactStatic charaContactStatic in this.haveContactPackList)
			{
				sortedSet.Add(charaContactStatic.SituationType);
			}
			foreach (CharaContactStatic charaContactStatic2 in this.notContactPackList)
			{
				sortedSet.Add(charaContactStatic2.SituationType);
			}
			this.contactSituationList = new List<CharaContactStatic.Situation>(sortedSet);
		}
		this.motListScrollH.Resize(0, 0);
		this.motListScrollV.Resize(0, 0);
		this.actListScrollH.Resize(this.contactSituationList.Count, 0);
		this.actListScrollV.Resize(this.contactSituationList.Count, 0);
		this.leftButton.transform.Find("Btn_TreeHouse/Mark_Lock").gameObject.SetActive(false);
		this.voiceSheet = null;
	}

	private bool DispRoom()
	{
		if (this.stageLoad == null)
		{
			SceneHome.StageType stageType = SceneHome.GetStageType();
			if (this.stageType != stageType)
			{
				this.stageType = stageType;
				this.stageLoad = this.StageLoad();
			}
		}
		else if (!this.stageLoad.MoveNext())
		{
			this.stageLoad = null;
		}
		return this.stageLoad == null;
	}

	private IEnumerator StageLoad()
	{
		if (this.camBCG != null)
		{
			this.camBCG.enabled = true;
		}
		float f = 0f;
		MstGameAppearanceData mstGameAppearanceData = GameAppearanceUtill.GetMstGameAppearanceData(TimeManager.Now);
		List<string> list = new List<string> { mstGameAppearanceData.homeModelFllePathA, mstGameAppearanceData.homeModelFllePathB, mstGameAppearanceData.homeModelFllePathC, mstGameAppearanceData.homeModelFllePathD };
		for (int i = 0; i < SceneHome.STAGE_NAME.Count; i++)
		{
			if (string.IsNullOrEmpty(list[i]))
			{
				list[i] = SceneHome.STAGE_NAME[i];
			}
		}
		string nm = StagePresetCtrl.PackDataPath + list[this.stageType - SceneHome.StageType.MORNING];
		AssetManager.LoadAssetData(nm, AssetManager.OWNER.HomeStage, 0, null);
		do
		{
			if ((f += TimeManager.DeltaTime * 200f) > 100f)
			{
				f = 100f;
			}
			if (this.camBCG != null)
			{
				this.camBCG.brightness = -f;
			}
			yield return null;
		}
		while (f < 100f);
		while (!AssetManager.IsLoadFinishAssetData(nm))
		{
			yield return null;
		}
		this.stageLight = null;
		if (this.stageCtrl != null)
		{
			Object.Destroy(this.stageCtrl.gameObject);
		}
		PrjUtil.ReleaseMemory(PrjUtil.UnloadUnused / 10);
		this.stageCtrl = AssetManager.InstantiateAssetData(nm, null).GetComponent<StagePresetCtrl>();
		this.stageCtrl.transform.SetParent(this.homeField.transform, false);
		this.stageCtrl.Setting(this.camera.fieldCamera);
		this.stageCtrl.RenderSettingParam.Param2Scene();
		AssetManager.AddLoadList(nm, AssetManager.OWNER.HomeStage);
		yield return null;
		if (this.IsNight)
		{
			Transform transform = this.stageCtrl.transform.Find("StageLight_dark");
			Light light = ((transform == null) ? null : transform.GetComponent<Light>());
			transform = this.stageCtrl.transform.Find("CharaLight_dark");
			Light light2 = ((transform == null) ? null : transform.GetComponent<Light>());
			if (light2 == null && light != null)
			{
				light2 = light;
				light2.cullingMask |= 1 << LayerMask.NameToLayer("FieldPlayer");
				light2.cullingMask |= 1 << LayerMask.NameToLayer("FieldPlayerAlpha");
			}
			this.stageLight = new List<Light> { light, light2 };
			if ((SceneHome.stageDark > 0) & (SceneHome.stageDark != SceneHome.GetStageTime()))
			{
				SceneHome.stageDark = 0;
			}
			this.ChangeStageLight();
		}
		do
		{
			if ((f -= TimeManager.DeltaTime * 200f) < 0f)
			{
				f = 0f;
			}
			if (this.camBCG != null)
			{
				this.camBCG.brightness = -f;
			}
			yield return null;
		}
		while (f > 0f);
		if (this.camBCG != null)
		{
			this.camBCG.enabled = false;
		}
		yield break;
	}

	public static SceneHome.StageType GetStageType()
	{
		int num = (TimeManager.Now.Hour * 3600 + TimeManager.Now.Minute * 60 + TimeManager.Now.Second) / 3600;
		num %= 24;
		SceneHome.StageType stageType = SceneHome.StageType.INVALID;
		foreach (int num2 in SceneHome.STAGE_TIME)
		{
			if (num < num2)
			{
				break;
			}
			stageType++;
		}
		if (stageType == SceneHome.StageType.INVALID)
		{
			stageType = SceneHome.StageType.NIGHT;
		}
		return stageType;
	}

	public static int GetStageTime()
	{
		DateTime dateTime = TimeManager.Now.AddHours(-(double)SceneHome.STAGE_TIME[0]);
		return dateTime.Year * 1000 + dateTime.DayOfYear;
	}

	private void ChangeStageLight()
	{
		if (this.stageLight != null)
		{
			this.stageCtrl.lightByStage.gameObject.SetActive(SceneHome.stageDark == 0 || this.stageLight[0] == null);
			this.stageCtrl.lightByPlayer.gameObject.SetActive(SceneHome.stageDark == 0 || this.stageLight[1] == null);
			if (this.stageLight[0] != null)
			{
				this.stageLight[0].gameObject.SetActive(!this.stageCtrl.lightByStage.gameObject.activeSelf);
			}
			if (this.stageLight[1] != null)
			{
				this.stageLight[1].gameObject.SetActive(!this.stageCtrl.lightByPlayer.gameObject.activeSelf);
			}
		}
	}

	private void ChkMonthly()
	{
		if ((this.monthlyPack = DataManager.DmMonthlyPack.nowPackData.MonthlypackData) == null)
		{
			this.monthlyDays = -99;
		}
		else
		{
			DateTime dateTime = new DateTime(TimeManager.Now.Year, TimeManager.Now.Month, TimeManager.Now.Day);
			DateTime dateTime2 = new DateTime(DataManager.DmMonthlyPack.nowPackData.EndDatetime.Year, DataManager.DmMonthlyPack.nowPackData.EndDatetime.Month, DataManager.DmMonthlyPack.nowPackData.EndDatetime.Day);
			this.monthlyDays = (dateTime2 - dateTime).Days;
			DataManagerMonthlyPack.PurchaseMonthlypackData monthlypackData = DataManager.DmMonthlyPack.nextPackData.MonthlypackData;
			dateTime2 = new DateTime(DataManager.DmMonthlyPack.nextPackData.EndDatetime.Year, DataManager.DmMonthlyPack.nextPackData.EndDatetime.Month, DataManager.DmMonthlyPack.nextPackData.EndDatetime.Day);
			int days = (dateTime2 - dateTime).Days;
			if (this.monthlyDays >= 0)
			{
				if (this.monthlyDays < days)
				{
					this.monthlyDays = days;
				}
			}
			else if (monthlypackData != null && this.monthlyDays < days)
			{
				this.monthlyPack = monthlypackData;
				this.monthlyDays = days;
			}
		}
		this.btnMonthlyPack.gameObject.SetActive(this.monthlyPack != null && this.monthlyDays >= 0);
		if (this.btnMonthlyPack.gameObject.activeSelf)
		{
			this.btnMonthlyPack.transform.Find("BaseImage/Txt_Time").GetComponent<PguiTextCtrl>().text = ((this.monthlyDays > 0) ? ("<size=16>残り</size>" + this.monthlyDays.ToString() + "日") : "本日終了");
			if (this.monthlyMission < 0)
			{
				Transform transform = this.btnMonthlyPack.transform.Find("BaseImage/Cmn_Badge");
				if ((this.monthlyMission = DataManager.DmMission.GetUserClearAllSpecialMissionNum()) < 0)
				{
					this.monthlyMission = 0;
				}
				transform.gameObject.SetActive(this.monthlyMission > 0);
				if (transform.gameObject.activeSelf)
				{
					transform.Find("Num").GetComponent<PguiTextCtrl>().text = this.monthlyMission.ToString();
				}
			}
			this.btnMonthlyPack.transform.Find("BaseImage/Img_Plus").gameObject.SetActive(false);
			return;
		}
		this.monthlyMission = -1;
	}

	public override bool OnEnableSceneWait()
	{
		return !DataManager.IsServerRequesting() && this.DispRoom() && this.charaCtrl.isSetup;
	}

	public override void OnStartSceneFade()
	{
		if (this.viewChara == null && this.quest_guide == 0)
		{
			this.bonus = SelLoginBonus.ExeLoginBonus(this.basePanel.transform);
			this.rouletteProcess = SelRouletteCtrl.ExeRoulette(this.basePanel.transform);
		}
		this.noticeList = new List<HomeBannerData>();
		this.purchaseNoticeList = null;
		this.DispRoom();
		GameObject furnitureModel = this.furnitureCtrl.GetFurnitureModel(HomeFurnitureStatic.Category.ELECTRONICS);
		Transform transform = ((furnitureModel == null) ? null : furnitureModel.transform.Find("pos_ef_television_a"));
		if (transform != null)
		{
			this.tvEff = EffectManager.InstantiateEffect(SceneHome.tvEffName, this.homeField.transform, furnitureModel.layer, 1f);
			if (this.tvEff != null)
			{
				this.tvEff.effectObject.transform.position = transform.position;
				this.tvEff.effectObject.transform.rotation = transform.rotation;
				this.tvEff.effectObject.transform.localScale = transform.localScale;
				this.tvEff.PlayEffect(false);
			}
		}
		if ((transform = this.stageLocator.transform.Find("pos_boom_box_a")) != null)
		{
			this.bgmEff = EffectManager.InstantiateEffect(SceneHome.bgmEffName, this.homeField.transform, this.stageLocator.layer, 1f);
			if (this.bgmEff != null)
			{
				this.bgmEff.effectObject.transform.position = transform.position;
				this.bgmEff.effectObject.transform.rotation = transform.rotation;
				this.bgmEff.effectObject.transform.localScale = transform.localScale;
				this.bgmEff.PlayEffect(false);
			}
		}
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		this.treehouseBadge.SetActive(DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.TreeHouseFirst != DataManagerGameStatus.UserFlagData.TREE_HOUSE_TUTORIAL.LATEST || (homeCheckResult != null && (homeCheckResult.treeHouseBadgeFlag || homeCheckResult.IsTreeHouseCharge())));
	}

	public override void OnStartSceneFadeWait()
	{
		this.LoginBonus();
		this.DispRoom();
	}

	public override void OnStartControl()
	{
		this.clickCamera = false;
		this.viewChg = false;
		this.viewNameTim = 0f;
		this.friendsId = 0;
		this.closet = 0;
		this.characome = 0;
		this.chgBgm = 0;
		this.monthly = 0;
		this.DispRoom();
	}

	private void OnPlayAnimationLB(SimpleAnimation.ExPguiStatus uiType)
	{
		if (this.onClick < 0)
		{
			this.onClick = -this.onClick % 10;
		}
		if (uiType == SimpleAnimation.ExPguiStatus.START)
		{
			this.leftButton.gameObject.SetActive(true);
			this.leftButton.ExPlayAnimation(uiType, null);
		}
		else
		{
			this.leftButton.ExPlayAnimation(uiType, delegate
			{
				this.leftButton.gameObject.SetActive(false);
			});
		}
		if (this.appEnd <= 0)
		{
			this.appEnd = ((uiType == SimpleAnimation.ExPguiStatus.START) ? 0 : (-1));
		}
		SoundManager.Play("prd_se_menu_slide", false, false);
	}

	private bool AppEndWindow(int index)
	{
		if (this.appEnd == 1)
		{
			this.appEnd = ((index == 1) ? 2 : 3);
		}
		return true;
	}

	public override void Update()
	{
		if (this.onClick > 0)
		{
			if (!this.leftButton.gameObject.activeSelf || !this.leftButton.ExIsPlaying())
			{
				if (this.onClick == 1)
				{
					this.furnitureCtrl.OnClickFunitureStart();
				}
				else if (this.onClick == 2)
				{
					this.modeCloset.transform.Find("Mode_Title/Img_Line/Txt_Title").GetComponent<PguiTextCtrl>().text = "着替え";
					this.modeCloset.gameObject.SetActive(true);
					this.modeCloset.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					Transform transform = this.closetChara.Find((Transform itm) => itm.name == this.stayFriends.ToString());
					if (transform != null)
					{
						transform.GetComponent<IconCharaCtrl>().DispCurrentFrame(false);
					}
					this.closet = 1;
				}
				else if (this.onClick == 5)
				{
					this.modeCloset.transform.Find("Mode_Title/Img_Line/Txt_Title").GetComponent<PguiTextCtrl>().text = "待機フレンズ設定";
					this.modeCloset.gameObject.SetActive(true);
					this.modeCloset.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					Transform transform2 = this.closetChara.Find((Transform itm) => itm.name == this.stayFriends.ToString());
					if (transform2 != null)
					{
						transform2.GetComponent<IconCharaCtrl>().DispCurrentFrame(true);
					}
					this.characome = 1;
				}
				else if (this.onClick == 4)
				{
					this.modeBgm.gameObject.SetActive(true);
					this.modeBgm.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					this.chgBgm = 1;
				}
				else if (this.friendsData != null)
				{
					this.friendsMenu.gameObject.SetActive(true);
					this.friendsMenu.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
				}
				this.onClick = 0;
			}
		}
		else if (this.onClick < -100)
		{
			if ((this.onClick -= 10) < -1000)
			{
				this.onClick = -this.onClick % 10;
			}
		}
		else if (this.onClick < 0)
		{
			if (CanvasManager.HdlCmnMenu.IsActiveMenu() && CanvasManager.HdlCmnMenu.SetActiveMenu(false))
			{
				this.onClick -= 100;
			}
			else
			{
				this.onClick = -this.onClick;
			}
		}
		if (this.monthly != 0 && !CanvasManager.HdlSelMonthlyPackAfterWindowCtrl.IsActiveWindow())
		{
			this.monthly = 0;
		}
		if (!this.LoginBonus())
		{
			if (!DataManager.DmScenario.IsPlayed() && this.viewChara == null && this.quest_guide == 0)
			{
				this.PlayLoginScenario();
			}
			else if (this._introduction != null && this.viewChara == null && this.quest_guide == 0)
			{
				this.PlayIntroductions();
			}
			else if (this.quest_guide == 0)
			{
				if (SceneHome.monthlyNotice == 0)
				{
					if ((SceneHome.monthlyNoticeWait < 0 || (SceneHome.monthlyNoticeWait == 0 && DataManager.DmPurchase.IsFinishSetupProduct)) && this.closet == 0 && this.monthlyNoticeWindow.FinishedClose())
					{
						List<DataManagerMonthlyPack.PurchaseMonthlypackData> list = new List<DataManagerMonthlyPack.PurchaseMonthlypackData>();
						foreach (DataManagerMonthlyPack.PurchaseMonthlypackData purchaseMonthlypackData in DataManager.DmMonthlyPack.purchaseMonthlypackDataList)
						{
							if (purchaseMonthlypackData.PackType != 4)
							{
								list.Add(purchaseMonthlypackData);
							}
						}
						DataManagerMonthlyPack.PurchaseMonthlypackMessageData purchaseMonthlypackMessageData = ((DataManager.DmMonthlyPack.purchaseMonthlypackMessageDataList.Count > 0) ? DataManager.DmMonthlyPack.purchaseMonthlypackMessageDataList[0] : null);
						DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
						string text = null;
						if (this.monthlyPack != null && list.Count > 0 && purchaseMonthlypackMessageData != null && this.monthlyDays >= -purchaseMonthlypackMessageData.ContinueLimitDay && this.monthlyDays <= purchaseMonthlypackMessageData.ReminderDay)
						{
							if (this.monthlyDays < 0)
							{
								if (!userFlagData.InformationsFlag.DisableMonthlyPackInfo2)
								{
									text = DataManager.DmMonthlyPack.purchaseMonthlypackMessageDataList[0].AfterPeriodText;
									int num = purchaseMonthlypackMessageData.ContinueLimitDay + this.monthlyDays;
									text = text.Replace("{day}", (num > 0) ? ("残り" + num.ToString() + "日") : "本日まで");
								}
							}
							else if (!userFlagData.InformationsFlag.DisableMonthlyPackInfo1)
							{
								text = DataManager.DmMonthlyPack.purchaseMonthlypackMessageDataList[0].BeforePeriodText;
							}
						}
						if (string.IsNullOrEmpty(text))
						{
							SceneHome.monthlyNotice = -1;
						}
						else if (SceneHome.monthlyNoticeWait < 0)
						{
							if (this.purchaseNoticeList == null)
							{
								this.purchaseNoticeList = new List<PurchaseProductOne>();
								DataManager.DmPurchase.SetupProduct();
							}
							SceneHome.monthlyNoticeWait = 0;
							this.monthlyNoticeWaitTime = Time.time;
						}
						else
						{
							string text2 = "";
							using (List<DataManagerMonthlyPack.PurchaseMonthlypackData>.Enumerator enumerator = list.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									DataManagerMonthlyPack.PurchaseMonthlypackData m = enumerator.Current;
									if (DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne itm) => itm.MonthlyPackId == m.PackId) != null)
									{
										if (!string.IsNullOrEmpty(text2))
										{
											text2 += "\n";
										}
										text2 = text2 + "【" + m.PackName + "】 ";
										DataManagerMonthlyPack.MonthlypackContinueData monthlypackContinueData = DataManager.DmMonthlyPack.monthlypackContinueDataList.Find((DataManagerMonthlyPack.MonthlypackContinueData mst) => mst.PrevMonthlyPackId == this.monthlyPack.PackId && mst.NextMonthlyPackId == m.PackId);
										if (monthlypackContinueData != null)
										{
											ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(monthlypackContinueData.AddItemId);
											text2 = text2 + itemStaticBase.GetName() + "×" + monthlypackContinueData.AddItemNum.ToString();
										}
									}
								}
							}
							text = text.Substring(0, text.IndexOf("{ID=1}") - 1) + text2 + text.Substring(text.IndexOf("{count2}") + 8);
							SceneHome.monthlyNotice = 1;
							string[] array = text.Split(new char[] { '\n' });
							string text3 = array[0] + "\n" + array[1];
							Transform transform3 = this.monthlyNoticeWindow.WindowRectTransform.Find("Base");
							if (transform3 == null)
							{
								for (int i = 2; i < array.Length; i++)
								{
									text3 = text3 + "\n" + array[i];
								}
							}
							else
							{
								transform3.Find("txt_title").GetComponent<PguiTextCtrl>().text = array[2];
								string text4 = array[3];
								for (int j = 4; j < array.Length; j++)
								{
									text4 = text4 + "\n" + array[j];
								}
								transform3.Find("txt_info").GetComponent<PguiTextCtrl>().text = text4;
							}
							this.monthlyNoticeMark.SetActive(false);
							this.monthlyNoticeWindow.Setup("お知らせ", text3, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int idx)
							{
								if (SceneHome.monthlyNotice == 1)
								{
									SceneHome.monthlyNotice = ((idx == 1) ? 2 : 3);
								}
								return true;
							}, null, false);
							this.monthlyNoticeWindow.Open();
						}
					}
				}
				else if (SceneHome.monthlyNotice > 0)
				{
					if (this.monthlyNoticeWindow.FinishedClose())
					{
						if (this.monthlyNoticeMark.activeSelf)
						{
							DataManagerGameStatus.UserFlagData userFlagData2 = DataManager.DmGameStatus.MakeUserFlagData();
							if (this.monthlyDays < 0)
							{
								userFlagData2.InformationsFlag.DisableMonthlyPackInfo2 = true;
							}
							else
							{
								userFlagData2.InformationsFlag.DisableMonthlyPackInfo1 = true;
							}
							DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData2);
						}
						if (SceneHome.monthlyNotice == 2)
						{
							Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneShop, null);
						}
						SceneHome.monthlyNotice = -1;
					}
				}
				else if (SceneHome.purchaseNotice == 0)
				{
					if (this.purchaseNoticeList == null)
					{
						this.purchaseNoticeList = new List<PurchaseProductOne>();
						DataManager.DmPurchase.SetupProduct();
					}
					else if (DataManager.DmPurchase.IsFinishSetupProduct)
					{
						SceneHome.purchaseNotice++;
						this.purchaseNoticeList = DataManager.DmPurchase.CreateTabPurchaseProductOneListList()[0].FindAll((PurchaseProductOne itm) => itm.infoType == PurchaseProductOne.InfoType.OnceADayForThePeriod || itm.infoType == PurchaseProductOne.InfoType.OnceForThePeriod);
						this.purchaseNoticeList.RemoveAll((PurchaseProductOne itm) => itm.isSoldOut);
						this.purchaseNoticeList.RemoveAll((PurchaseProductOne itm) => DataManager.DmPurchase.InfoHiddenList.Contains(itm.productId));
						this.purchaseNoticeShop = false;
						this.purchaseNoticeData = null;
					}
				}
				else if (SceneHome.purchaseNotice > 0)
				{
					if (CanvasManager.HdlCmnFeedPageWindowCtrl.FinishedClose() && !CanvasManager.HdlSelPurchaseStoneWindowCtrl.IsActiveWindow() && !DataManager.IsServerRequesting())
					{
						if (this.purchaseNoticeShop)
						{
							this.purchaseNoticeShop = false;
							CanvasManager.HdlSelPurchaseStoneWindowCtrl.Setup(this.purchaseNoticeData.tabType);
						}
						else if (this.purchaseNoticeData != null)
						{
							if (this.purchaseNoticeData.infoType == PurchaseProductOne.InfoType.OnceForThePeriod || CanvasManager.HdlCmnFeedPageWindowCtrl.IsActiveCheckBox())
							{
								DataManager.DmPurchase.RequestUpdateAddHiddenInfo(this.purchaseNoticeData.productId);
							}
							this.purchaseNoticeData = null;
						}
						else if (this.purchaseNoticeList.Count > 0)
						{
							this.purchaseNoticeData = this.purchaseNoticeList[0];
							this.purchaseNoticeList.RemoveAt(0);
							CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.SHOP_ESCORT, "お知らせ", new List<string> { "Texture2D/Tutorial_Window/Shop_Info/" + this.purchaseNoticeData.InfoPicturePath }, delegate(bool opn)
							{
								this.purchaseNoticeShop = opn;
							});
							if (this.purchaseNoticeData.infoType == PurchaseProductOne.InfoType.OnceADayForThePeriod)
							{
								CanvasManager.HdlCmnFeedPageWindowCtrl.SetupCheckBox();
							}
						}
						else
						{
							SceneHome.purchaseNotice = -1;
						}
					}
				}
				else if (SceneHome.notice == 0)
				{
					if (this.closet == 0)
					{
						SceneHome.notice = 1;
						this.noticeList = DataManager.DmHome.GetHomeBannerList().FindAll((HomeBannerData itm) => itm.actionType == HomeBannerData.ActionType.NOTICE);
						if (this.questNoClear)
						{
							this.questNotice = -1;
						}
					}
				}
				else if (SceneHome.notice > 0)
				{
					if (CanvasManager.HdlWebViewWindowCtrl.FinishedClose())
					{
						if (this.noticeList.Count > 0)
						{
							if (CanvasManager.winClose == 0)
							{
								CanvasManager.HdlWebViewWindowCtrl.Open(this.noticeList[0].actionParamURL);
								this.noticeList.RemoveAt(0);
							}
						}
						else
						{
							SceneHome.notice = -1;
						}
					}
				}
				else if (this.questNotice < 0)
				{
					if (this.closet == 0)
					{
						this.questNotice = 1;
						CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "メインストーリーをクリアしよう！", new List<string> { "Texture2D/Loading/Screenshot/Screenshots_21", "Texture2D/Loading/Screenshot/Screenshots_22" }, null);
					}
				}
				else if (this.questNotice > 0)
				{
					if (this.questNotice < 3)
					{
						this.questNotice++;
					}
					else if (CanvasManager.HdlCmnFeedPageWindowCtrl.FinishedClose())
					{
						this.questNotice = 0;
					}
				}
				else if (this.growRewardInfo < 0)
				{
					if (this.viewChara == null && this.quest_guide == 0)
					{
						List<DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo> photoGrowRewardInfoList = DataManager.DmHome.GetPhotoGrowRewardInfoList(true);
						if (photoGrowRewardInfoList.Count > 0)
						{
							this.growRewardInfo = 1;
							List<ItemData> list2 = new List<ItemData>();
							foreach (DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo growRewardInfo in photoGrowRewardInfoList)
							{
								list2.Add(new ItemData(growRewardInfo.ItemId, -1));
							}
							GetMultiItemWindowCtrl.SetupParam setupParam = new GetMultiItemWindowCtrl.SetupParam
							{
								titleText = "確認",
								messageText = "フォトアルバムの登録状態に応じて報酬を入手できるようになりました\n（登録状態が「限界突破最大」「強化最大」にて報酬を入手）\n※入手条件達成済みのフォトが既に存在するため対象の報酬を入手しました",
								innerTitleText = "入手したアイテム",
								callBack = delegate(int x)
								{
									this.growRewardInfo = 0;
									return true;
								}
							};
							CanvasManager.HdlGetItemSetWindowCtrl.Setup(list2, setupParam, false, 24);
							CanvasManager.HdlGetItemSetWindowCtrl.Open();
						}
						else
						{
							this.growRewardInfo = 0;
						}
					}
					else
					{
						this.growRewardInfo = 0;
					}
				}
				else if (this.growRewardInfo <= 0)
				{
					if (this.friendPoint.activeSelf)
					{
						if (!this.friendPoint.GetComponent<SimpleAnimation>().ExIsPlaying())
						{
							this.friendPoint.SetActive(false);
						}
						else if (this.furnitureCtrl.isActive)
						{
							this.friendPoint.SetActive(false);
						}
						else if (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
						{
							this.friendPoint.SetActive(false);
						}
					}
					else if (DataManager.DmHome.GetHomeCheckResult() != null && DataManager.DmHome.GetHomeCheckResult().usedHelperNum > 0 && DataManager.DmHome.GetHomeCheckResult().usedHelperPoint > 0)
					{
						this.friendPoint.SetActive(true);
						this.friendPoint.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
						this.friendPoint.transform.Find("Txt_people").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", ((DataManager.DmHome.GetHomeCheckResult().usedHelperNum > 999) ? 999 : DataManager.DmHome.GetHomeCheckResult().usedHelperNum).ToString());
						this.friendPoint.transform.Find("Txt_people/Txt_peopleplus").gameObject.SetActive(DataManager.DmHome.GetHomeCheckResult().usedHelperNum > 999);
						this.friendPoint.transform.Find("Txt_point").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", ((DataManager.DmHome.GetHomeCheckResult().usedHelperPoint > 9999) ? 9999 : DataManager.DmHome.GetHomeCheckResult().usedHelperPoint).ToString());
						this.friendPoint.transform.Find("Txt_point/Txt_pointplus").gameObject.SetActive(DataManager.DmHome.GetHomeCheckResult().usedHelperPoint > 9999);
						DataManager.DmHome.GetHomeCheckResult().usedHelperNum = 0;
						DataManager.DmHome.GetHomeCheckResult().usedHelperPoint = 0;
					}
					else if (this.firstDownload < 0)
					{
						if (this.viewChara == null && this.quest_guide == 0)
						{
							if (DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.FirstAssetDownload)
							{
								this.firstDownload = 0;
							}
							else
							{
								this.firstDownload = 1;
								this.downloadResolver = AssetDownloadResolver.ResolveActionFull(false);
								DataManagerGameStatus.UserFlagData userFlagData3 = DataManager.DmGameStatus.MakeUserFlagData();
								userFlagData3.TutorialFinishFlag.FirstAssetDownload = true;
								DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData3);
							}
						}
						else
						{
							this.firstDownload = 0;
						}
					}
					else if (this.downloadResolver != null)
					{
						if (!this.downloadResolver.MoveNext())
						{
							this.downloadResolver = null;
						}
					}
					else if (this.firstDownload != 0)
					{
						if (!DataManager.IsServerRequesting())
						{
							this.firstDownload = 0;
						}
					}
					else if (DataManager.DmUserInfo.expOverflow != 0L && this.playerRankUpStatus == SceneHome.ProgressStatus.Wait && this.rankupWindow != null)
					{
						this.rankBefore = DataManager.DmUserInfo.level;
						DataManager.DmUserInfo.RequestActionPlayerLevelUp(DataManager.DmUserInfo.expOverflow);
						this.playerRankUpStatus = SceneHome.ProgressStatus.Requesting;
					}
					else if (this.playerRankUpStatus == SceneHome.ProgressStatus.Requesting && !DataManager.IsServerRequesting() && DataManager.DmUserInfo.expOverflow == 0L)
					{
						this.playerRankUpStatus = ((this.rankBefore != DataManager.DmUserInfo.level) ? SceneHome.ProgressStatus.Result : SceneHome.ProgressStatus.End);
					}
					else if (this.playerRankUpStatus == SceneHome.ProgressStatus.Result)
					{
						this.rankWinBefore.ReplaceTextByDefault("Param01", this.rankBefore.ToString());
						this.rankWinAfter.ReplaceTextByDefault("Param01", DataManager.DmUserInfo.level.ToString());
						int num2 = Mathf.Clamp(this.rankBefore - 1, 0, DataManager.DmServerMst.gameLevelInfoList.Count - 1);
						num2 = DataManager.DmServerMst.gameLevelInfoList[num2].staminaLimit + DataManager.DmTreeHouse.StaminaBonusData.staminaBonus;
						int num3 = Mathf.Clamp(DataManager.DmUserInfo.level - 1, 0, DataManager.DmServerMst.gameLevelInfoList.Count - 1);
						num3 = DataManager.DmServerMst.gameLevelInfoList[num3].staminaLimit + DataManager.DmTreeHouse.StaminaBonusData.staminaBonus;
						this.rankupWindow.transform.Find("Base/Window/Massage_stamina01").gameObject.SetActive(true);
						PguiTextCtrl component = this.rankupWindow.transform.Find("Base/Window/Massage_stamina02").GetComponent<PguiTextCtrl>();
						component.ReplaceTextByDefault("Param01", num3.ToString());
						component.gameObject.SetActive(num3 > num2);
						CanvasManager.HdlCmnMenu.SetActiveMenu(false);
						this.rankupWindow.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
						this.rankupWindow.Open();
						this.playerRankUpStatus = SceneHome.ProgressStatus.End;
					}
					else if (this.playerRankUpStatus == SceneHome.ProgressStatus.End && this.rankupWindow != null && this.rankupWindow.FinishedClose())
					{
						CanvasManager.HdlCmnMenu.SetActiveMenu(true);
						this.rankupWindow.ForceClose();
						Object.Destroy(this.rankUpPanel);
						Object.Destroy(this.rankupWindow);
						this.rankupWindow = null;
					}
				}
			}
		}
		if (this.questNoClear && this.viewChara == null && this.quest_guide == 0)
		{
			if (!this.questNoClearIcon.activeSelf)
			{
				this.questNoClearIcon.SetActive(true);
				this.questNoClearIcon.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
			}
		}
		else if (this.questNoClearIcon.activeSelf)
		{
			this.questNoClearIcon.SetActive(false);
		}
		if (this.camPlace != this.furnitureCtrl.GetPlaceId())
		{
			this.camPlace = this.furnitureCtrl.GetPlaceId();
			HomePlacementStatic homePlacementStatic = ((this.camPlace == 0) ? null : DataManager.DmHome.GetHomePlacementStaticData(this.camPlace));
			Transform transform4 = ((homePlacementStatic == null) ? null : this.stageLocator.transform.Find("cam_" + homePlacementStatic.locatorName));
			if (transform4 == null)
			{
				this.camera.transform.localPosition = this.camPos[this.camNo];
				Vector3 vector = this.camRot[this.camNo];
				this.camera.fieldOfView = vector.z;
				vector.z = 0f;
				this.camera.transform.localEulerAngles = vector;
			}
			else
			{
				this.camera.transform.localPosition = transform4.localPosition;
				this.camera.transform.localRotation = transform4.localRotation;
				this.camera.fieldOfView = 50f;
			}
		}
		else if (this.clickCamera)
		{
			this.clickCamera = false;
			if (this.viewChara == null && this.camPlace == 0)
			{
				int num4 = this.camNo + 1;
				this.camNo = num4;
				if (num4 >= this.camPos.Count)
				{
					this.camNo = 0;
				}
				this.camera.transform.localPosition = this.camPos[this.camNo];
				Vector3 vector2 = this.camRot[this.camNo];
				this.camera.fieldOfView = vector2.z;
				vector2.z = 0f;
				this.camera.transform.localEulerAngles = vector2;
			}
		}
		if (this.viewChg)
		{
			int num4 = this.viewNo + 1;
			this.viewNo = num4;
			if (num4 >= this.viewPos.Count)
			{
				this.viewNo = 0;
			}
			if (this.viewNo < this.viewPos.Count)
			{
				this.charaCtrl.SetViewPos(this.viewPos[this.viewNo][2]);
				this.viewCam = this.viewPos[this.viewNo][0].position;
				this.viewItr = this.viewPos[this.viewNo][2].position;
				this.viewItr.y = this.viewItr.y + this.viewHeight;
				this.camera.transform.localPosition = this.viewCam;
				this.camera.transform.LookAt(this.viewItr);
				this.viewLen = Vector3.Magnitude(this.viewCam - this.viewItr);
				this.viewBas = new Vector2(Mathf.DeltaAngle(0f, this.camera.transform.eulerAngles.x), Mathf.DeltaAngle(0f, this.camera.transform.eulerAngles.y));
				this.viewRot = Vector2.zero;
				Transform transform5 = this.viewPos[this.viewNo][1];
				this.viewMin = new Vector3(-transform5.localScale.x, -transform5.localScale.y, transform5.localScale.z);
				this.viewMax = new Vector3(-transform5.localPosition.x, -transform5.localPosition.y, transform5.localPosition.z);
				float z = this.viewMin.z;
				float z2 = this.viewMax.z;
				this.camera.fieldOfView = Mathf.Clamp(this.camera.fieldOfView, z, z2);
			}
			this.viewChg = false;
		}
		float fieldOfView = this.camera.fieldOfView;
		Vector2 vector3 = (this.stroke ? Vector2.zero : (this.moveView * fieldOfView / 750f));
		float num5 = fieldOfView;
		float num6 = fieldOfView;
		if (this.viewChara == null)
		{
			if (this.camPlace == 0 && this.quest_guide == 0)
			{
				Vector3 localEulerAngles = this.camera.transform.localEulerAngles;
				localEulerAngles.z = 0f;
				Vector3 vector4 = localEulerAngles;
				Vector2 vector5 = new Vector2(Mathf.Abs(vector3.y), Mathf.Abs(vector3.x));
				if (vector5.y > 0.0001f)
				{
					vector4.y = ((vector3.x > 0f) ? this.camMin[this.camNo].y : this.camMax[this.camNo].y);
				}
				if (vector5.x > 0.0001f)
				{
					vector4.x = ((vector3.y < 0f) ? this.camMin[this.camNo].x : this.camMax[this.camNo].x);
				}
				num5 = this.camMin[this.camNo].z;
				num6 = this.camMax[this.camNo].z;
				vector4.x = Mathf.Clamp(Mathf.DeltaAngle(localEulerAngles.x, vector4.x), -vector5.x, vector5.x);
				vector4.y = Mathf.Clamp(Mathf.DeltaAngle(localEulerAngles.y, vector4.y), -vector5.y, vector5.y);
				this.camera.transform.localEulerAngles = localEulerAngles + vector4;
			}
		}
		else if (this.viewNo < this.viewPos.Count)
		{
			Vector2 vector6 = this.viewRot;
			if (vector3.x > 0.0001f)
			{
				vector6.y = this.viewMax.y;
			}
			else if (vector3.x < -0.0001f)
			{
				vector6.y = this.viewMin.y;
			}
			if (vector3.y > 0.0001f)
			{
				vector6.x = this.viewMin.x;
			}
			else if (vector3.y < -0.0001f)
			{
				vector6.x = this.viewMax.x;
			}
			Vector2 vector7 = new Vector2(Mathf.Abs(vector3.y), Mathf.Abs(vector3.x));
			vector6.x = Mathf.Clamp(vector6.x - this.viewRot.x, -vector7.x, vector7.x);
			vector6.y = Mathf.Clamp(vector6.y - this.viewRot.y, -vector7.y, vector7.y);
			this.viewRot += vector6;
			Vector3 vector8 = this.viewBas + this.viewRot;
			vector8.z = 0f;
			Matrix4x4 identity = Matrix4x4.identity;
			identity.SetTRS(this.viewItr, Quaternion.Euler(vector8), Vector3.one);
			this.viewCam = identity.MultiplyPoint(new Vector3(0f, 0f, -this.viewLen));
			this.camera.transform.localPosition = this.viewCam;
			this.camera.transform.LookAt(this.viewItr);
			num5 = this.viewMin.z;
			num6 = this.viewMax.z;
		}
		else
		{
			Vector3 localPosition = this.camera.transform.localPosition;
			Vector3 vector9 = localPosition;
			float num7 = 50f;
			Vector2 vector10 = new Vector2(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y));
			if (vector10.x > 0.0001f)
			{
				vector9.x = this.viewCam.x + ((vector3.x > 0f) ? (-1f) : 1f);
			}
			if (vector10.y > 0.0001f)
			{
				vector9.y = this.viewCam.y + ((vector3.y > 0f) ? (-1f) : 1f);
			}
			vector10 /= 20f;
			vector9.x = Mathf.Clamp(vector9.x - localPosition.x, -vector10.x, vector10.x);
			vector9.y = Mathf.Clamp(vector9.y - localPosition.y, -vector10.y, vector10.y);
			vector9.z = 0f;
			this.camera.transform.localPosition = localPosition + vector9;
			this.camera.transform.LookAt(this.viewItr);
			num5 = num7 - 20f;
			num6 = num7 + 20f;
		}
		if (this.pinchView == 0f)
		{
			this.pinchFov = this.camera.fieldOfView;
		}
		this.camera.fieldOfView = Mathf.Clamp(this.pinchFov - this.pinchView - this.wheelView, num5, num6);
		this.moveView = Vector2.zero;
		this.pinchView = 0f;
		this.wheelView = 0f;
		this.viewName.SetActive(!this.hideView && (this.viewNameTim += TimeManager.DeltaTime) < 4f);
		if (this.viewChara != null)
		{
			this.friendsId = (this.friendsTouch = 0);
		}
		else if (this.friendsTouch > 0)
		{
			if (this.onClick == 0 && this.appEnd <= 0 && !this.appEndObj.activeSelf)
			{
				this.friendsId = this.friendsTouch;
				SoundManager.Play("prd_se_click", false, false);
			}
			else
			{
				this.friendsTouch = -this.friendsId;
			}
		}
		int num8 = ((this.friendsTouch == 0) ? 0 : this.charaCtrl.GetCharaStat(this.friendsId));
		this.friendsData = ((this.questNotice == 0 && this.growRewardInfo == 0 && this.firstDownload == 0 && this.chgBgm == 0 && !this.modeBgm.gameObject.activeSelf && this.closet == 0 && !this.modeCloset.gameObject.activeSelf && this.characome == 0 && !this.furnitureCtrl.isActive && num8 > 0 && this.monthly == 0) ? DataManager.DmChara.GetUserCharaData(this.friendsId) : null);
		if (this.friendsData == null || (this.friendsMenu.gameObject.activeSelf && (this.friendsMenuTime += TimeManager.DeltaTime) > 60f))
		{
			if (this.friendsMenu.gameObject.activeSelf)
			{
				if (this.friendsId > 0)
				{
					this.friendsMenu.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
					this.friendsId = 0;
					SoundManager.Play("prd_se_cancel", false, false);
				}
				else if (!this.friendsMenu.ExIsPlaying())
				{
					this.friendsMenu.gameObject.SetActive(false);
					if (this.chgBgm == 0)
					{
						CanvasManager.HdlCmnMenu.SetActiveMenu(true);
					}
				}
			}
		}
		else if (this.onClick == 0 && this.appEnd <= 0 && !this.appEndObj.activeSelf && (this.friendsTouch > 0 || !this.friendsMenu.gameObject.activeSelf))
		{
			CharaStaticBase charaStaticBase = this.friendsData.staticData.baseData;
			this.friendsMenu.transform.Find("NameInfo/Txt_Name").GetComponent<PguiTextCtrl>().text = charaStaticBase.charaName;
			this.friendsMenu.transform.Find("NameInfo/Txt_Name_Eg").GetComponent<PguiTextCtrl>().text = charaStaticBase.charaNameEng;
			this.friendsMenu.transform.Find("NameInfo/Txt_Kind").GetComponent<PguiTextCtrl>().text = charaStaticBase.animalScientificName;
			this.friendsMenu.transform.Find("SerifWindow/Texture_Chara").GetComponent<PguiRawImageCtrl>().SetRawImage(this.friendsData.staticData.GetIconName(), true, false, null);
			List<CharaPackData> list3 = new List<CharaPackData> { this.friendsData };
			list3.Add(DataManager.DmChara.GetUserCharaData(charaStaticBase.OriginalId));
			foreach (int num9 in charaStaticBase.SynonymIdSet)
			{
				list3.Add(DataManager.DmChara.GetUserCharaData(num9));
			}
			list3.RemoveAll((CharaPackData itm) => itm == null);
			if (list3.Count > 0)
			{
				charaStaticBase = list3[Random.Range(0, list3.Count)].staticData.baseData;
			}
			List<string> list4 = charaStaticBase.hometouchStandList;
			List<VOICE_TYPE> list5 = SceneHome.standVoice;
			if (num8 == 2)
			{
				list4 = charaStaticBase.hometouchWalkList;
				list5 = SceneHome.walkVoice;
			}
			else if (num8 == 3)
			{
				list4 = charaStaticBase.hometouchSleepList;
				list5 = SceneHome.sleepVoice;
			}
			int num10 = list4.Count;
			if (num10 > list5.Count)
			{
				num10 = list5.Count;
			}
			num10 = Random.Range(0, num10);
			this.friendsMenu.transform.Find("SerifWindow/Txt").GetComponent<PguiTextCtrl>().text = list4[num10];
			if (!string.IsNullOrEmpty(this.voiceSheet))
			{
				SoundManager.Stop(this.voiceSheet);
			}
			SoundManager.PlayVoice(this.voiceSheet = SoundManager.CharaIdToSheet(charaStaticBase.id), list5[num10]);
			if (!this.friendsMenu.gameObject.activeSelf)
			{
				this.onClick = -3;
			}
			this.friendsMenuTime = 0f;
		}
		this.friendsTouch = -this.friendsId;
		if (this.touchView)
		{
			this.charaCtrl.SetOpe();
		}
		if (this.friendsId == 0)
		{
			this.charaCtrl.CancelContact();
		}
		if (this.closet == 0 && this.modeCloset.gameObject.activeSelf && this.characome == 0 && !this.modeCloset.ExIsPlaying())
		{
			this.modeCloset.gameObject.SetActive(false);
			CanvasManager.HdlCmnMenu.SetActiveMenu(true);
		}
		this.modeClosetBack.androidBackKeyTarget = (this.closet > 0 || this.characome > 0) && this.modeCloset.gameObject.activeSelf;
		if (this.chgBgm == 0 && this.modeBgm.gameObject.activeSelf && !this.modeBgm.ExIsPlaying())
		{
			this.modeBgm.gameObject.SetActive(false);
			CanvasManager.HdlCmnMenu.SetActiveMenu(true);
		}
		this.modeBgmBack.androidBackKeyTarget = this.chgBgm > 0 && this.modeBgm.gameObject.activeSelf;
		CanvasManager.HdlCmnMenu.UpdateMenu(this.viewChara == null && this.bonus == null, true);
		this.furnitureBadge.SetActive(this.furnitureCtrl.isBadge());
		if (this.quest_guide < 0)
		{
			this.quest_guide = 0;
			CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
			CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
			CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
			{
				dispType = TutorialMaskCtrl.CharaDispType.OUT_QUICK
			});
			CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
			TutorialUtil.RequestNextSequence(TutorialUtil.Sequence.QUEST_GUIDE);
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneHome, null);
		}
		else if (this.quest_guide == 1)
		{
			this.quest_guide = 2;
			TutorialMaskCtrl.CharaInfoParameter charaInfoParameter = new TutorialMaskCtrl.CharaInfoParameter();
			charaInfoParameter.dispType = TutorialMaskCtrl.CharaDispType.IN;
			charaInfoParameter.postion = new Vector2?(new Vector2(300f, 250f));
			charaInfoParameter.dispInfoChara = true;
			charaInfoParameter.enableTouchNext = true;
			charaInfoParameter.charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_0022";
			charaInfoParameter.messageList = new List<string> { "他にわからないことがあれば\nその他→ヘルプを確認してみてください" };
			charaInfoParameter.finishCallBack = delegate
			{
				this.quest_guide = 3;
			};
			CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(charaInfoParameter);
			RectTransform component2 = CanvasManager.HdlCmnMenu.GetConfigBtn().GetComponent<RectTransform>();
			CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, component2, true, 1f, 1f);
		}
		else if (this.quest_guide != 2)
		{
			if (this.quest_guide == 3)
			{
				this.quest_guide = 4;
				TutorialMaskCtrl.CharaInfoParameter charaInfoParameter2 = new TutorialMaskCtrl.CharaInfoParameter();
				charaInfoParameter2.dispType = TutorialMaskCtrl.CharaDispType.IN;
				charaInfoParameter2.postion = new Vector2?(new Vector2(300f, 250f));
				charaInfoParameter2.dispInfoChara = true;
				charaInfoParameter2.enableTouchNext = true;
				charaInfoParameter2.charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_0022";
				charaInfoParameter2.messageList = new List<string> { "さて、探検に向かいましょう" };
				charaInfoParameter2.finishCallBack = delegate
				{
					this.quest_guide = 5;
				};
				CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(charaInfoParameter2);
				RectTransform component3 = CanvasManager.HdlCmnMenu.GetQuestBtn().GetComponent<RectTransform>();
				CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, component3, true, 1f, 1f);
			}
			else if (this.quest_guide != 4 && this.quest_guide > 0)
			{
				this.quest_guide = -1;
			}
		}
		this.DispRoom();
		if (this.tvEff != null && this.tvEff.IsFinishByAnimation())
		{
			this.tvEff.PlayEffect(false);
		}
		if (this.bgmEff != null && this.bgmEff.IsFinishByAnimation())
		{
			this.bgmEff.PlayEffect(false);
		}
		this.charaCtrl.SetNight(this.IsNight);
		this.ChkMonthly();
		if (this.viewPanel.activeSelf)
		{
			if (this.hideView && this.viewAnm.gameObject.activeSelf && this.viewAnm.ExIsCurrent(SimpleAnimation.ExPguiStatus.START) && !this.viewAnm.ExIsPlaying())
			{
				this.viewAnm.gameObject.SetActive(false);
			}
			if (this.motListWinH.gameObject.activeSelf && this.motListWinH.FinishedClose())
			{
				this.motListWinH.gameObject.SetActive(false);
			}
			if (this.motListWinV.gameObject.activeSelf && this.motListWinV.FinishedClose())
			{
				this.motListWinV.gameObject.SetActive(false);
			}
			if (this.motListWinH.gameObject.activeSelf && SafeAreaScaler.ScreenWidth < SafeAreaScaler.ScreenHeight)
			{
				this.motListWinH.ForceClose();
				if (!this.motListWinV.gameObject.activeSelf)
				{
					this.MakeMotList();
					this.motListWinV.gameObject.SetActive(true);
					this.motListWinV.Setup(null, null, null, true, null, null, false);
					this.motListWinV.ForceOpen();
					this.actListScrollV.Refresh();
				}
			}
			if (this.motListWinV.gameObject.activeSelf && SafeAreaScaler.ScreenWidth >= SafeAreaScaler.ScreenHeight)
			{
				this.motListWinV.ForceClose();
				if (!this.motListWinH.gameObject.activeSelf)
				{
					this.MakeMotList();
					this.motListWinH.gameObject.SetActive(true);
					this.motListWinH.Setup(null, null, null, true, null, null, false);
					this.motListWinH.ForceOpen();
					this.actListScrollH.Refresh();
				}
			}
		}
		else
		{
			this.vertViewChg = 0f;
			if (SceneHome.nowVertView)
			{
				Singleton<CanvasManager>.Instance.SetDisplayDirection(DataManager.DmUserInfo.optionData.DisplayDirection);
			}
		}
		if (this.viewEnd)
		{
			ScreenOrientation orientation = Screen.orientation;
			if (!SceneHome.nowVertView && this.vertViewChg < 0.1f)
			{
				if (this.args != null && SceneManager.SceneName.SceneCharaEdit == this.args.sceneName)
				{
					SceneManager instance = Singleton<SceneManager>.Instance;
					SceneManager.SceneName sceneName = SceneManager.SceneName.SceneCharaEdit;
					object obj;
					if (this.args.menuBackSceneArgs != null)
					{
						obj = this.args.menuBackSceneArgs;
					}
					else
					{
						(obj = new SceneCharaEdit.Args()).detailCharaId = this.viewChara.id;
					}
					instance.SetNextScene(sceneName, obj);
				}
				else if (this.args != null && SceneManager.SceneName.SceneQuest == this.args.sceneName)
				{
					Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneQuest, new SceneQuest.Args
					{
						selectQuestOneId = this.args.questOneId
					});
				}
				else
				{
					Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneHome, null);
				}
			}
		}
		if (this.characome == 3 && CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneHome, null);
		}
	}

	private void OnClickButtonMenuRetrun()
	{
	}

	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		if (!this.CheckButton())
		{
			return true;
		}
		if (this.quest_guide == 0)
		{
			return false;
		}
		if (this.quest_guide == 2 && sceneName == SceneManager.SceneName.SceneOtherMenuTop)
		{
			this.quest_guide = 3;
		}
		if (this.quest_guide == 4 && sceneName == SceneManager.SceneName.SceneQuest)
		{
			this.quest_guide = 5;
		}
		return true;
	}

	private void SetupMenu()
	{
		this.bannerCtrl.BannerRefresh();
		this.bigBannerCtrl.BannerRefresh();
		List<PguiCmnMenuCtrl.OnPlayAnimation> list = new List<PguiCmnMenuCtrl.OnPlayAnimation>();
		list.Add(new PguiCmnMenuCtrl.OnPlayAnimation(this.bannerCtrl.OnPlayAnimation));
		list.Add(new PguiCmnMenuCtrl.OnPlayAnimation(this.bigBannerCtrl.OnPlayAnimation));
		list.Add(new PguiCmnMenuCtrl.OnPlayAnimation(this.OnPlayAnimationLB));
		CanvasManager.HdlCmnMenu.SetupMenuByHome(new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), list);
		this.camera.fieldCamera.depth = (float)((SceneManager.CameraDepth[SceneManager.CanvasType.BACK] + SceneManager.CameraDepth[SceneManager.CanvasType.FRONT]) / 2);
		CanvasManager.SetBgEnable(false);
	}

	private bool LoginBonus()
	{
		if (this.bonus != null)
		{
			if (!this.bonus.MoveNext())
			{
				this.bonus = null;
			}
			return true;
		}
		if (this.rouletteProcess != null)
		{
			if (!this.rouletteProcess.MoveNext())
			{
				this.rouletteProcess = null;
				this.SetupMenu();
			}
			return true;
		}
		return false;
	}

	private void OnTouchStart(Info info)
	{
		if (this.OnUiTap(info.CurrentPosition))
		{
			return;
		}
		this.touchView = true;
		this.stroke = this.charaCtrl.OnTouchStart(info);
	}

	private void OnTouchEnd(Info info)
	{
		this.touchView = false;
		this.charaCtrl.OnTouchEnd(info);
		this.stroke = false;
	}

	private void OnTouchMove(Info info)
	{
		if (!this.touchView)
		{
			return;
		}
		this.moveView = HomeCharaCtrl.deltaPosition(info.DeltaPosition);
		if (this.stroke && !this.charaCtrl.OnTouchMove(info))
		{
			this.stroke = false;
		}
	}

	private void OnPinch(Info fingerA, Info fingerB, float distance, float rotation)
	{
		if (!this.touchView || this.stroke)
		{
			return;
		}
		if (this.OnUiTap(fingerA.CurrentPosition))
		{
			return;
		}
		if (this.OnUiTap(fingerB.CurrentPosition))
		{
			return;
		}
		float num = (float)Screen.width;
		float num2 = (float)Screen.height;
		if (Screen.height > Screen.width)
		{
			num2 = (float)Screen.width;
			num = (float)Screen.height;
		}
		this.pinchView = distance * Mathf.Sqrt(1280f / num * (720f / num2)) / 10f;
	}

	private void OnWheel(Info info, float distance)
	{
		if (this.touchView || this.stroke)
		{
			return;
		}
		if (this.OnUiTap(info.CurrentPosition))
		{
			return;
		}
		if (!CanvasManager.CheckInWindow(info.CurrentPosition))
		{
			return;
		}
		this.wheelView = distance * 10f;
	}

	private void OnTap(Info info)
	{
		if (this.OnUiTap(info.CurrentPosition))
		{
			return;
		}
		if (this.questNotice != 0 || this.growRewardInfo != 0 || this.firstDownload != 0 || this.onClick != 0 || this.chgBgm != 0 || this.modeBgm.gameObject.activeSelf || this.closet != 0 || this.modeCloset.gameObject.activeSelf || this.characome != 0 || this.furnitureCtrl.isActive || this.monthly != 0)
		{
			return;
		}
		this.charaCtrl.SetOpe();
		float num = float.MaxValue;
		int num2 = -1;
		List<Transform> list = new List<Transform>
		{
			(this.viewChara == null) ? this.stageLocator.transform.Find("pos_boom_box_a") : null,
			(this.stageLight == null) ? null : this.stageLocator.transform.Find("switching_timezone_a"),
			(this.stageLight == null) ? null : this.stageLocator.transform.Find("switching_timezone_b")
		};
		for (int i = 0; i < list.Count; i++)
		{
			if (!(list[i] == null))
			{
				Vector3 position = list[i].position;
				if (this.camera.fieldCamera.WorldToViewportPoint(position).z >= 0.3f)
				{
					position.y -= 0.25f;
					Vector2 vector = RectTransformUtility.WorldToScreenPoint(this.camera.fieldCamera, position);
					if (vector.y <= info.CurrentPosition.y)
					{
						position.y += 0.5f;
						Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(this.camera.fieldCamera, position);
						if (vector2.y >= info.CurrentPosition.y)
						{
							float num3 = vector2.y - vector.y;
							float num4 = info.CurrentPosition.x - vector.x;
							if (num4 * num4 <= num3 * num3)
							{
								num2 = i;
								num = Vector3.Distance(list[i].position, this.camera.fieldCamera.transform.position);
							}
						}
					}
				}
			}
		}
		int num5 = this.charaCtrl.OnTap(info.CurrentPosition, num, num2 < 0 && !this.hideView);
		if (num2 < 0 || num5 != 0)
		{
			this.friendsTouch = num5;
			num2 = -1;
		}
		if (this.hideView && this.friendsTouch == 0 && num2 < 0 && !this.viewAnm.gameObject.activeSelf)
		{
			this.hideView = false;
			this.viewAnm.gameObject.SetActive(true);
			this.viewAnm.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			this.viewNameTim = 0f;
			SoundManager.Play("prd_se_menu_slide", false, false);
		}
		if (num2 == 0)
		{
			this.chgBgm = 1;
			this.onClick = -4;
			this.bgmList = new List<DataManagerHome.HomeBgmPlaybackData>();
			foreach (DataManagerHome.HomeBgmPlaybackData homeBgmPlaybackData in DataManager.DmHome.GetMstBgmPlaybackDataList())
			{
				if (TimeManager.Now.Ticks >= homeBgmPlaybackData.startDatetime.Ticks && TimeManager.Now.Ticks <= homeBgmPlaybackData.endDatetime.Ticks)
				{
					this.bgmList.Add(homeBgmPlaybackData);
				}
			}
			this.bgmList.Sort((DataManagerHome.HomeBgmPlaybackData a, DataManagerHome.HomeBgmPlaybackData b) => a.sortNum - b.sortNum);
			this.bgmScroll.Resize((this.bgmList.Count + 3 - 1) / 3, 0);
			SoundManager.Play("prd_se_click", false, false);
			return;
		}
		if (num2 > 0)
		{
			SceneHome.stageDark = ((SceneHome.stageDark > 0) ? 0 : SceneHome.GetStageTime());
			this.ChangeStageLight();
			SoundManager.Play("prd_se_room_lamp_switch", false, false);
		}
	}

	private bool OnUiTap(Vector2 pos)
	{
		if (this.questNotice != 0 || this.growRewardInfo != 0 || this.firstDownload != 0 || this.onClick != 0 || this.chgBgm != 0 || this.modeBgm.gameObject.activeSelf || this.closet != 0 || this.modeCloset.gameObject.activeSelf || this.characome != 0 || this.monthly != 0)
		{
			return true;
		}
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = pos;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list.Count > 0;
	}

	public override void OnStopControl()
	{
		if (this.friendPoint.activeSelf)
		{
			this.friendPoint.SetActive(false);
		}
	}

	public override void OnDisableScene()
	{
		this.CloseBgmLineup();
		this.CloseCloset();
		this.CloseFriendsMenu();
		this.charaCtrl.TearDown();
		this.furnitureCtrl.TearDown();
		this.furnitureMap = null;
		CanvasManager.HdlCmnMenu.SetActiveMenu(true);
		CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
		this.bonus = null;
		this.growRewardInfo = -1;
		this.firstDownload = -1;
		this.downloadResolver = null;
		this.viewChara = null;
		this.viewEnd = false;
		this.closet = -1;
		this.characome = -1;
		this.chgBgm = -1;
		this.monthly = -1;
		this.monthlyPack = null;
		this.questNotice = 0;
		this.friendsId = -1;
		this.friendsTouch = 0;
		this.friendsData = null;
		this.closetScroll.Resize(0, 0);
		this.haveCharaPackList = new List<CharaPackData>();
		this.dispCharaPackList = new List<CharaPackData>();
		foreach (Transform transform in this.closetChara)
		{
			Object.Destroy(transform.gameObject);
		}
		this.closetChara = new List<Transform>();
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData();
		registerData.register = SortFilterDefine.RegisterType.HOME_CLOSET;
		registerData.filterButton = null;
		registerData.sortButton = null;
		registerData.sortUdButton = null;
		registerData.funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget();
		registerData.funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
		{
		};
		SortWindowCtrl.RegisterData registerData2 = registerData;
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData2, false, null);
		this.bgmScroll.Resize(0, 0);
		foreach (Transform transform2 in this.bgmLineup)
		{
			Object.Destroy(transform2.gameObject);
		}
		this.bgmLineup = new List<Transform>();
		this.motListScrollH.Resize(0, 0);
		this.motListScrollV.Resize(0, 0);
		this.actListScrollH.Resize(0, 0);
		this.actListScrollV.Resize(0, 0);
		EffectManager.BillboardCamera = null;
		this.leftButton.gameObject.SetActive(false);
		this.bannerCtrl.gameObject.SetActive(false);
		this.bigBannerCtrl.gameObject.SetActive(false);
		this.friendPoint.SetActive(false);
		this.friendsMenu.gameObject.SetActive(false);
		this.modeCloset.gameObject.SetActive(false);
		this.modeBgm.gameObject.SetActive(false);
		this.motListWinH.gameObject.SetActive(false);
		this.motListWinV.gameObject.SetActive(false);
		this.listPanelH.SetActive(false);
		this.listPanelV.SetActive(false);
		this.basePanel.SetActive(false);
		this.windowPanel.SetActive(false);
		this.homeField.SetActive(false);
		this.viewPanel.SetActive(false);
		if (this.furnitureNew.Count > 0)
		{
			DataManager.DmItem.RequestActionUpdateNewFlag(this.furnitureNew);
			this.furnitureNew = new List<int>();
		}
		if (DataManager.DmUserInfo.optionData.StayFriendsId != this.stayFriends)
		{
			UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
			userOptionData.StayFriendsId = this.stayFriends;
			DataManager.DmUserInfo.RequestActionUpdateUserOption(userOptionData);
		}
		this.touchView = false;
		SGNFW.Touch.Manager.UnRegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
		SGNFW.Touch.Manager.UnRegisterRelease(new SGNFW.Touch.Manager.SingleAction(this.OnTouchEnd));
		SGNFW.Touch.Manager.UnRegisterMove(new SGNFW.Touch.Manager.SingleAction(this.OnTouchMove));
		SGNFW.Touch.Manager.UnRegisterPinch(new SGNFW.Touch.Manager.DoubleAction(this.OnPinch));
		SGNFW.Touch.Manager.UnRegisterMouseWheel(new SGNFW.Touch.Manager.WheelAction(this.OnWheel));
		SGNFW.Touch.Manager.UnRegisterTap(new SGNFW.Touch.Manager.SingleAction(this.OnTap));
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(false);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(false);
		AssetManager.UnLoadByList(AssetManager.OWNER.HomeStage, "", true);
		if (this.tvEff != null)
		{
			EffectManager.DestroyEffect(this.tvEff);
		}
		this.tvEff = null;
		if (this.bgmEff != null)
		{
			EffectManager.DestroyEffect(this.bgmEff);
		}
		this.bgmEff = null;
	}

	public override bool OnDisableSceneWait()
	{
		bool flag = !DataManager.IsServerRequesting();
		if (this.stageCtrl != null)
		{
			if (this.DispRoom())
			{
				this.stageLight = null;
				Object.Destroy(this.stageCtrl.gameObject);
				this.stageCtrl = null;
			}
			else
			{
				flag = false;
			}
		}
		if (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			CanvasManager.HdlOpenWindowBasic.ForceClose();
			flag = false;
		}
		return flag;
	}

	public override void OnDestroyScene()
	{
		this.furnitureCtrl = null;
		Object.Destroy(this._homeAuthCtrl.BaseObj);
		this._homeAuthCtrl = null;
		this.charaCtrl = null;
		this.bannerCtrl = null;
		this.bigBannerCtrl = null;
		this.hidePanel = null;
		this.leftButton = null;
		this.friendsMenu = null;
		this.modeCloset = null;
		this.modeBgm = null;
		Object.Destroy(this.basePanel);
		this.basePanel = null;
		this.viewAnm = null;
		this.viewName = null;
		Object.Destroy(this.viewPanel);
		this.viewPanel = null;
		this.friendPoint = null;
		this.monthlyNoticeWindow = null;
		Object.Destroy(this.windowPanel);
		this.windowPanel = null;
		Object.Destroy(this.charaPanel);
		this.charaPanel = null;
		Object.Destroy(this.bgmPanel);
		this.bgmPanel = null;
		this.actListScrollH = null;
		this.actListScrollV = null;
		this.motListScrollH = null;
		this.motListScrollV = null;
		this.motListWinH = null;
		this.motListWinV = null;
		Object.Destroy(this.listPanelH);
		Object.Destroy(this.listPanelV);
		this.listPanelH = null;
		this.listPanelV = null;
		Object.Destroy(this.stageLocator);
		this.stageLocator = null;
		Object.Destroy(this.homeField);
		this.homeField = null;
		EffectManager.UnloadEffect(SceneHome.tvEffName, AssetManager.OWNER.HomeStage);
		EffectManager.UnloadEffect(SceneHome.bgmEffName, AssetManager.OWNER.HomeStage);
	}

	private void PlayLoginScenario()
	{
		if (DataManager.DmScenario.IsPlayed())
		{
			return;
		}
		List<DataManagerScenario.LoginScenarioData> playableLoginScenarioList = DataManager.DmScenario.GetPlayableLoginScenarioList();
		string jsonPlayedScenarioList = DataManager.DmScenario.GetJsonPlayedScenarioList();
		SceneScenario.Args args = new SceneScenario.Args();
		string text = string.Empty;
		DataManager.DmUserInfo.RequestActionUpdateScenarioLastId(playableLoginScenarioList[playableLoginScenarioList.Count - 1].id, jsonPlayedScenarioList);
		DataManagerScenario.LoginScenarioData loginScenarioData = playableLoginScenarioList[0];
		if (loginScenarioData.randomId != 0)
		{
			text = DataManager.DmScenario.GetRandomScenarioFileName(loginScenarioData.randomId, playableLoginScenarioList[0].scenarioId);
		}
		else
		{
			text = loginScenarioData.scenarioFileName;
		}
		args.scenarioName = text;
		args.nextSceneName = SceneManager.SceneName.SceneScenario;
		this.CreateNextArgs(1, ref args);
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneScenario, args);
	}

	private void PlayIntroductions()
	{
		if (this._introduction == null)
		{
			return;
		}
		this._introduction.MoveNext();
	}

	private void CreateNextArgs(int idx, ref SceneScenario.Args args)
	{
		SceneScenario.Args args2 = new SceneScenario.Args();
		List<DataManagerScenario.LoginScenarioData> playableLoginScenarioList = DataManager.DmScenario.GetPlayableLoginScenarioList();
		if (idx >= playableLoginScenarioList.Count)
		{
			args.nextSceneName = SceneManager.SceneName.SceneHome;
			return;
		}
		string text = string.Empty;
		DataManagerScenario.LoginScenarioData loginScenarioData = playableLoginScenarioList[idx++];
		if (DataManager.DmScenario.IsRandomScenario(loginScenarioData.randomId))
		{
			text = DataManager.DmScenario.GetRandomScenarioFileName(loginScenarioData.randomId, loginScenarioData.scenarioId);
		}
		else
		{
			text = loginScenarioData.scenarioFileName;
		}
		args2.scenarioName = text;
		args2.nextSceneName = SceneManager.SceneName.SceneScenario;
		args.nextSceneArgs = args2;
		this.CreateNextArgs(idx, ref args2);
	}

	private CharaPackData viewChara;

	private GameObject basePanel;

	private GameObject viewPanel;

	private GameObject windowPanel;

	private GameObject homeField;

	private GameObject charaPanel;

	private GameObject bgmPanel;

	private GameObject rankUpPanel;

	private Transform hidePanel;

	private GameObject listPanelH;

	private GameObject listPanelV;

	private FieldCameraScaler camera;

	private CC_BrightnessContrastGamma camBCG;

	private int camNo;

	private int camPlace;

	private List<Vector3> camPos;

	private List<Vector3> camRot;

	private List<Vector3> camMin;

	private List<Vector3> camMax;

	private HomeCharaCtrl charaCtrl;

	private int stayFriends;

	private HomeBannerCtrl bannerCtrl;

	private PguiButtonCtrl btnMonthlyPack;

	private DataManagerMonthlyPack.PurchaseMonthlypackData monthlyPack;

	private int monthlyDays;

	private int monthly;

	private int monthlyMission;

	private HomeBigBannerCtrl bigBannerCtrl;

	private SimpleAnimation leftButton;

	private GameObject furnitureBadge;

	private GameObject treehouseBadge;

	private int onClick;

	private int appEnd;

	private GameObject appEndObj;

	private HomeFurnitureCtrl furnitureCtrl;

	private HomeAuthCtrl _homeAuthCtrl;

	private static readonly string STAGE_ROOM_LOCATOR = "Stage/Stage/st_room_locator_a";

	private static readonly List<string> STAGE_NAME = new List<string> { "SD_room_timezone_a_morning", "SD_room_timezone_b_noon", "SD_room_timezone_c_evening", "SD_room_timezone_d_night" };

	private static readonly List<int> STAGE_TIME = new List<int> { 5, 8, 17, 19 };

	private GameObject stageLocator;

	private StagePresetCtrl stageCtrl;

	private List<Light> stageLight;

	private IEnumerator stageLoad;

	private SceneHome.StageType stageType;

	private static int stageDark = 0;

	private GameObject friendPoint;

	private List<HomeFurnitureMapping> furnitureMap;

	private List<int> furnitureNew;

	private SimpleAnimation friendsMenu;

	private int friendsId;

	private int friendsTouch;

	private CharaPackData friendsData;

	private float friendsMenuTime;

	private SimpleAnimation modeCloset;

	private PguiButtonCtrl modeClosetBack;

	private int closet;

	private List<CharaPackData> haveCharaPackList;

	private List<CharaPackData> dispCharaPackList;

	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	private ReuseScroll closetScroll;

	private List<Transform> closetChara;

	private int characome;

	private bool touchView;

	private Vector2 moveView;

	private bool stroke;

	private float pinchView;

	private float pinchFov;

	private float wheelView;

	private bool vertView;

	public static bool nowVertView;

	private float vertViewChg;

	private bool hideView;

	private int viewNo;

	private bool viewChg;

	private bool viewEnd;

	private List<List<Transform>> viewPos;

	private Vector3 viewCam;

	private Vector3 viewItr;

	private float viewHeight;

	private float viewLen;

	private Vector2 viewBas;

	private Vector2 viewRot;

	private Vector3 viewMin;

	private Vector3 viewMax;

	private SimpleAnimation viewAnm;

	private GameObject viewName;

	private float viewNameTim;

	private PguiRawImageCtrl viewFade;

	private List<CharaContactStatic> haveContactPackList;

	private List<CharaContactStatic> notContactPackList;

	private List<CharaContactStatic> dispContactPackList;

	private List<CharaContactStatic.Situation> contactSituationList;

	private int contactSituation;

	private PguiOpenWindowCtrl motListWinH;

	private PguiOpenWindowCtrl motListWinV;

	private ReuseScroll motListScrollH;

	private ReuseScroll motListScrollV;

	private ReuseScroll actListScrollH;

	private ReuseScroll actListScrollV;

	private static readonly Dictionary<CharaContactStatic.Situation, string> situationName = new Dictionary<CharaContactStatic.Situation, string>
	{
		{
			CharaContactStatic.Situation.STAND_NEGLECT,
			"何もしないで見守る"
		},
		{
			CharaContactStatic.Situation.STAND_TAP,
			"フレンズをトントンする"
		},
		{
			CharaContactStatic.Situation.STAND_STROKING,
			"フレンズをなでる"
		},
		{
			CharaContactStatic.Situation.SLEEP_TAP,
			"ベッドで寝ている"
		},
		{
			CharaContactStatic.Situation.SITDOWN_TAP,
			"イスに座っている"
		},
		{
			CharaContactStatic.Situation.STAND_OUT_TAP,
			"フレンズ以外の場所をトントンする"
		}
	};

	private EffectData tvEff;

	private static readonly string tvEffName = "Ef_stage_surface_television";

	private EffectData bgmEff;

	private static readonly string bgmEffName = "Ef_info_home_note";

	private List<DataManagerHome.HomeBgmPlaybackData> bgmList;

	private int bgmId;

	private List<Transform> bgmLineup;

	private SimpleAnimation modeBgm;

	private PguiButtonCtrl modeBgmBack;

	private int chgBgm;

	private ReuseScroll bgmScroll;

	private static readonly string homeBgm = "prd_bgm0013";

	private IEnumerator bonus;

	private IEnumerator rouletteProcess;

	public static int notice = -1;

	private List<HomeBannerData> noticeList;

	public static int monthlyNotice = -1;

	public static int monthlyNoticeWait = -1;

	private float monthlyNoticeWaitTime;

	private PguiOpenWindowCtrl monthlyNoticeWindow;

	private GameObject monthlyNoticeMark;

	private int questNotice;

	private bool questNoClear;

	private GameObject questNoClearIcon;

	private int growRewardInfo;

	public static int purchaseNotice = -1;

	private List<PurchaseProductOne> purchaseNoticeList;

	private PurchaseProductOne purchaseNoticeData;

	private bool purchaseNoticeShop;

	private int quest_guide;

	private int firstDownload;

	private IEnumerator downloadResolver;

	private IEnumerator _introduction;

	private static readonly List<VOICE_TYPE> standVoice = new List<VOICE_TYPE>
	{
		VOICE_TYPE.HOM01,
		VOICE_TYPE.HOM02,
		VOICE_TYPE.HOM03,
		VOICE_TYPE.HOM04
	};

	private static readonly List<VOICE_TYPE> walkVoice = new List<VOICE_TYPE>
	{
		VOICE_TYPE.MOV01,
		VOICE_TYPE.MOV02,
		VOICE_TYPE.MOV03,
		VOICE_TYPE.MOV04,
		VOICE_TYPE.MOV05
	};

	private static readonly List<VOICE_TYPE> sleepVoice = new List<VOICE_TYPE>
	{
		VOICE_TYPE.SLP01,
		VOICE_TYPE.SLP02,
		VOICE_TYPE.SLP03
	};

	private string voiceSheet;

	private int rankBefore;

	private SceneHome.ProgressStatus playerRankUpStatus;

	private PguiOpenWindowCtrl rankupWindow;

	private PguiTextCtrl rankWinBefore;

	private PguiTextCtrl rankWinAfter;

	private bool clickCamera;

	private SceneHome.Args args = new SceneHome.Args();

	public class Args
	{
		public TutorialUtil.Sequence tutorialSequence;

		public CharaPackData charaPackData;

		public SceneManager.SceneName sceneName;

		public object menuBackSceneArgs;

		public int questOneId;
	}

	private enum ProgressStatus
	{
		Wait,
		Requesting,
		Result,
		End
	}

	public enum StageType
	{
		INVALID,
		MORNING,
		NOON,
		EVENING,
		NIGHT
	}
}
