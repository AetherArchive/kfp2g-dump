using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000183 RID: 387
public class SceneTraining : BaseScene
{
	// Token: 0x170003CF RID: 975
	// (get) Token: 0x060018F7 RID: 6391 RVA: 0x001321F9 File Offset: 0x001303F9
	// (set) Token: 0x060018F8 RID: 6392 RVA: 0x0013220B File Offset: 0x0013040B
	private float currentFadeTime
	{
		get
		{
			return Singleton<CanvasManager>.Instance.CurrentFadeTimes[0];
		}
		set
		{
			Singleton<CanvasManager>.Instance.CurrentFadeTimes[0] = value;
		}
	}

	// Token: 0x060018F9 RID: 6393 RVA: 0x00132220 File Offset: 0x00130420
	public override void OnCreateScene()
	{
		this.guiData = new SceneTraining.GUI(AssetManager.InstantiateAssetData("SceneTraining/GUI/Prefab/GUI_Training_Top", null).transform);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.guiData.baseObj.transform, true);
		this.winPanel = AssetManager.InstantiateAssetData("SceneTraining/GUI/Prefab/GUI_Training_Window", null);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.winPanel.transform, true);
		this.winItem = new SceneTraining.WIN_ITEM(this.winPanel.transform.Find("Window_GetItem"));
		this.winRank = new SceneTraining.WIN_RANK(this.winPanel.transform.Find("Window_ScoreRank"));
		this.winScore = new SceneTraining.WIN_SCORE(this.winPanel.transform.Find("Window_MyScore"));
		this.winParty = new SceneTraining.WIN_PARTY(this.winPanel.transform.Find("Window_UserParty"));
		this.winSeason = new SceneTraining.WIN_SEASON(this.winPanel.transform.Find("Window_SeasonRank"));
		this.guiData.BtnStart.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnAgain.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnMission.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnShop.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnSeason.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnRank.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnScore.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnItem.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnQuestion.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnModeChange.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		this.rewardList = new List<TrainingStaticData.RewardData>();
		this.winItem.scroll.InitForce();
		ReuseScroll scroll = this.winItem.scroll;
		scroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(scroll.onStartItem, new Action<int, GameObject>(this.SetupItem));
		ReuseScroll scroll2 = this.winItem.scroll;
		scroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scroll2.onUpdateItem, new Action<int, GameObject>(this.UpdateItem));
		this.winItem.scroll.Setup(0, 0);
		this.winItem.win.Setup(null, null, null, true, null, null, false);
		this.dayOfRankList = new List<TrainingRankingData.RankingOne>();
		this.winRank.scroll.InitForce();
		ReuseScroll scroll3 = this.winRank.scroll;
		scroll3.onStartItem = (Action<int, GameObject>)Delegate.Combine(scroll3.onStartItem, new Action<int, GameObject>(this.SetupRank));
		ReuseScroll scroll4 = this.winRank.scroll;
		scroll4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scroll4.onUpdateItem, new Action<int, GameObject>(this.UpdateRank));
		this.winRank.scroll.Setup(0, 0);
		this.winRank.win.Setup(null, null, null, true, null, null, false);
		this.winScore.win.Setup(null, null, null, true, null, null, false);
		this.winParty.win.Setup(null, null, null, true, null, null, false);
		this.seasonList = new List<SeasonTrainingRankingData.RankingOne>();
		this.winSeason.scroll.InitForce();
		ReuseScroll scroll5 = this.winSeason.scroll;
		scroll5.onStartItem = (Action<int, GameObject>)Delegate.Combine(scroll5.onStartItem, new Action<int, GameObject>(this.SetupSeason));
		ReuseScroll scroll6 = this.winSeason.scroll;
		scroll6.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scroll6.onUpdateItem, new Action<int, GameObject>(this.UpdateSeason));
		this.winSeason.scroll.Setup(0, 0);
		this.winSeason.win.Setup(null, null, null, true, null, null, false);
		this.guiData.baseObj.SetActive(false);
		this.winPanel.SetActive(false);
		this.guiData.coin.transform.parent.Find("Icon_Stone").GetComponent<PguiRawImageCtrl>().SetRawImage(DataManager.DmItem.GetItemStaticBase(30133).GetIconName(), true, false, null);
		this.guiData.BtnAgain.transform.Find("BaseImage/Inbase/Icon_Item").GetComponent<PguiRawImageCtrl>().SetRawImage(DataManager.DmItem.GetItemStaticBase(30100).GetIconName(), true, false, null);
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x001326FC File Offset: 0x001308FC
	public override void OnEnableScene(object args)
	{
		this.sta = args as SceneTraining.Args;
		this.ienum = null;
		CanvasManager.SetBgObj("PanelBg_Training");
		this.voiceTime = 0f;
		if (!DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.TrainigFirst && (this.sta == null || !this.sta.tutorial))
		{
			this.ienum = this.FirstScenaro();
		}
		else
		{
			this.guiData.baseObj.SetActive(this.sta == null || !this.sta.deck);
			if (this.sta != null && this.sta.tutorial)
			{
				this.ienum = this.FirstTutorial();
			}
			else if (!this.guiData.baseObj.activeSelf)
			{
				this.ienum = this.DeckOpen();
			}
			this.winPanel.SetActive(false);
			this.winItem.win.ForceClose();
			this.winParty.win.ForceClose();
			this.winRank.win.ForceClose();
			this.winScore.win.ForceClose();
			this.winSeason.win.ForceClose();
			if (this.guiData.baseObj.activeSelf)
			{
				this.guiData.anime.ExPauseAnimation((this.sta == null || this.sta.tutorial) ? SimpleAnimation.ExPguiStatus.START : SimpleAnimation.ExPguiStatus.START_SUB, null);
				CanvasManager.HdlCmnMenu.SetupMenu(true, "シーサーバル道場", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickButtonMenu), null);
			}
			SceneTraining.PlayBGM();
			this.guiData.charaL.Create();
			this.guiData.charaR.Create();
			this.guiData.charaL.renderTextureChara.Setup(223, 0, CharaMotionDefine.ActKey.WEAPON_STAND_BY, 0, false, true, null, false, null, 0f, null, false, false, false);
			this.guiData.charaR.renderTextureChara.Setup(222, 1, CharaMotionDefine.ActKey.WEAPON_STAND_BY, 0, false, true, null, false, null, 0f, null, false, false, false);
			DataManager.DmTraining.RequestGetTrainingInfo();
			this.voiceLoad = SoundManager.LoadCueSheetWithDownload(SceneTraining.voiceSheet);
		}
		if (this.isPractice)
		{
			DataManagerMonthlyPack.PurchaseMonthlypackData validMonthlyPackData = DataManager.DmMonthlyPack.GetValidMonthlyPackData();
			this.isPractice = (validMonthlyPackData != null && validMonthlyPackData.PracticeFlag) || (this.trialInfo != null && this.IsInTrial(this.trialInfo));
			this.guiData.TxtMode.text = (this.isPractice ? "ON" : "OFF");
			int num = (this.isPractice ? 1 : 0);
			this.guiData.BtnModeChange.SetToggleIndex(num);
			if (!this.isPractice)
			{
				this.StopPopup();
			}
			if (this.currentFadeTime != 0f)
			{
				this.currentFadeTime = CanvasManager.FADE_DURATION - this.currentFadeTime;
			}
			if (this.guiData.bg == null)
			{
				this.guiData.bg = Singleton<CanvasManager>.Instance.GetBg("PanelBg_Training").GetComponent<RawImage>();
			}
			if (this.guiData.bg == null)
			{
				return;
			}
			Graphic bg = this.guiData.bg;
			Color color2;
			if (this.isPractice)
			{
				Graphic bg2 = this.guiData.bg;
				Color color = new Color(1f, 1f, 1f, 0f);
				bg2.color = color;
				color2 = color;
			}
			else
			{
				Graphic bg3 = this.guiData.bg;
				Color color = new Color(1f, 1f, 1f, 1f);
				bg3.color = color;
				color2 = color;
			}
			bg.color = color2;
		}
	}

	// Token: 0x060018FB RID: 6395 RVA: 0x00132AA0 File Offset: 0x00130CA0
	public static void PlayBGM()
	{
		SoundManager.PlayBGM("prd_bgm0063");
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x00132AAC File Offset: 0x00130CAC
	public override bool OnEnableSceneWait()
	{
		if (!this.guiData.baseObj.activeSelf)
		{
			return true;
		}
		if (this.voiceLoad.MoveNext() | DataManager.IsServerRequesting())
		{
			return false;
		}
		this.dayOfWeek = DataManager.DmTraining.GetTrainingPackData().dynamicData.currentDayOfWeek;
		TrainingStaticData staticData = DataManager.DmTraining.GetTrainingPackData().staticData;
		this.seasonId = staticData.SeasonId;
		this.dayOfWeekData = (staticData.dayOfWeekDataList.ContainsKey(this.dayOfWeek) ? staticData.dayOfWeekDataList[this.dayOfWeek] : null);
		this.guiData.week.text = SceneTraining.weekList[(int)this.dayOfWeek];
		this.guiData.serifL.text = ((this.dayOfWeekData == null) ? "" : this.dayOfWeekData.charaText02);
		this.guiData.serifR.text = ((this.dayOfWeekData == null) ? "" : this.dayOfWeekData.charaText01);
		if (this.dayOfWeekData == null)
		{
			this.guiData.icon.SetTexture(null, false);
		}
		else
		{
			this.guiData.icon.SetRawImage("Texture2D/Icon_Enemy/" + this.dayOfWeekData.enemyTexturePath, true, false, null);
		}
		this.guiData.attr.SetImageByName(SceneTraining.GetBossAttr(this.dayOfWeekData));
		int num = (int)this.dayOfWeek;
		if (--num < 0)
		{
			num += 7;
		}
		this.winRank.tab.Setup(num, new PguiTabGroupCtrl.OnSelectTab(this.onClickTabRank));
		this.winScore.tab.Setup(num, new PguiTabGroupCtrl.OnSelectTab(this.onClickTabScore));
		this.winSeason.tab.Setup(1, new PguiTabGroupCtrl.OnSelectTab(this.onClickTabSeason));
		this.guiData.coin.text = DataManager.DmItem.GetUserItemData(30133).num.ToString();
		this.SetButton();
		return true;
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x00132CB8 File Offset: 0x00130EB8
	private void SetButton()
	{
		if (this.dayOfWeekData == null)
		{
			this.guiData.BtnStart.gameObject.SetActive(true);
			this.guiData.BtnAgain.gameObject.SetActive(false);
			this.guiData.BtnStart.SetActEnable(false, false, false);
			this.guiData.BtnAgain.SetActEnable(false, false, false);
			this.guiData.BtnStart.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = "挑戦不可";
			return;
		}
		if (DataManager.DmTraining.GetTrainingPackData().IsEnablePlay())
		{
			this.guiData.BtnStart.gameObject.SetActive(true);
			this.guiData.BtnAgain.gameObject.SetActive(false);
			this.guiData.BtnStart.SetActEnable(true, false, false);
			this.guiData.BtnAgain.SetActEnable(false, false, false);
			this.guiData.BtnStart.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = "バトル\n開始";
			return;
		}
		if (DataManager.DmTraining.GetTrainingPackData().IsEnableRecovery())
		{
			if (!this.isPractice)
			{
				this.guiData.BtnStart.gameObject.SetActive(false);
				this.guiData.BtnAgain.gameObject.SetActive(true);
				this.guiData.BtnStart.SetActEnable(false, false, false);
				this.guiData.BtnAgain.SetActEnable(true, false, false);
			}
			else
			{
				this.guiData.BtnStart.gameObject.SetActive(true);
				this.guiData.BtnAgain.gameObject.SetActive(false);
				this.guiData.BtnStart.SetActEnable(true, false, false);
				this.guiData.BtnAgain.SetActEnable(false, false, false);
			}
			int num = DataManager.DmItem.GetUserItemData(30100).num;
			int recoveryStoneNum = DataManager.DmTraining.GetTrainingPackData().staticData.RecoveryStoneNum;
			string text = num.ToString();
			if (num < recoveryStoneNum)
			{
				text = string.Concat(new string[]
				{
					"<color=",
					PrjUtil.WARNING_COLOR_CODE,
					">",
					text,
					"</color>"
				});
			}
			this.guiData.BtnAgain.transform.Find("BaseImage/Inbase/Num_Use").GetComponent<PguiTextCtrl>().text = text + "/" + recoveryStoneNum.ToString();
			int num2 = DataManager.DmTraining.GetTrainingPackData().staticData.RecoveryMax - DataManager.DmTraining.GetTrainingPackData().dynamicData.todayRecoveryNum;
			this.guiData.BtnAgain.transform.Find("BaseImage/Txt02").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", num2.ToString());
			return;
		}
		this.guiData.BtnStart.gameObject.SetActive(true);
		this.guiData.BtnAgain.gameObject.SetActive(false);
		if (!this.isPractice)
		{
			this.guiData.BtnStart.SetActEnable(false, true, false);
		}
		else
		{
			this.guiData.BtnStart.SetActEnable(true, false, false);
		}
		string text2 = (this.isPractice ? "バトル\n開始" : "挑戦済");
		this.guiData.BtnStart.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = text2;
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x00133028 File Offset: 0x00131228
	public override void OnStartControl()
	{
		if (!this.guiData.baseObj.activeSelf)
		{
			return;
		}
		if (this.guiData.baseObj.activeSelf)
		{
			this.guiData.anime.ExResumeAnimation(delegate
			{
				this.guiData.anime.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START);
			});
		}
		if (this.ienum == null)
		{
			this.SetVoice();
		}
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x00133084 File Offset: 0x00131284
	private IEnumerator FirstScenaro()
	{
		yield return null;
		SceneScenario.Args args = new SceneScenario.Args
		{
			questId = 50100,
			storyType = 1,
			scenarioName = DataManager.DmQuest.QuestStaticData.oneDataMap[50100].scenarioBeforeId,
			nextSceneName = SceneManager.SceneName.SceneTraining,
			nextSceneArgs = new SceneTraining.Args
			{
				tutorial = true
			}
		};
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneScenario, args);
		yield break;
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x0013308C File Offset: 0x0013128C
	private IEnumerator FirstTutorial()
	{
		yield return null;
		bool isFinishWindow = false;
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/Training/tutorial_training_01", "Texture2D/Tutorial_Window/Training/tutorial_training_02", "Texture2D/Tutorial_Window/Training/tutorial_training_03", "Texture2D/Tutorial_Window/Training/tutorial_training_04", "Texture2D/Tutorial_Window/Training/tutorial_training_05" }, delegate(bool b)
		{
			isFinishWindow = true;
		});
		while (!isFinishWindow)
		{
			yield return null;
		}
		DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
		userFlagData.TutorialFinishFlag.TrainigFirst = true;
		DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.SetVoice();
		yield break;
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x0013309C File Offset: 0x0013129C
	private void SetVoice()
	{
		string text = SceneTraining.voiceList[(int)this.dayOfWeek, 0];
		SoundManager.PlayVoice(SceneTraining.voiceSheet, text);
		this.voiceTime = SoundManager.GetVoiceLength(SceneTraining.voiceSheet, text);
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x001330D8 File Offset: 0x001312D8
	public override void Update()
	{
		if (CanvasManager.HdlSelCharaDeck.gameObject.activeSelf && CanvasManager.HdlSelCharaDeck.ForceReturnTop())
		{
			CanvasManager.HdlSelCharaDeck.SetActive(false, false);
			this.ienum = this.DeckClose();
		}
		if (this.winPanel.activeSelf && this.winItem.win.FinishedClose() && this.winRank.win.FinishedClose() && this.winScore.win.FinishedClose() && this.winParty.win.FinishedClose() && this.winSeason.win.FinishedClose())
		{
			this.winPanel.SetActive(false);
		}
		if (this.ienum != null && !this.ienum.MoveNext())
		{
			this.ienum = null;
		}
		if (this.voiceTime > 0f && (this.voiceTime -= TimeManager.DeltaTime) <= 0f)
		{
			SoundManager.PlayVoice(SceneTraining.voiceSheet, SceneTraining.voiceList[(int)this.dayOfWeek, 1]);
		}
		if (this.toggleEvent != null && !this.toggleEvent.MoveNext())
		{
			this.toggleEvent = null;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(this.guiData.baseObj.activeSelf || CanvasManager.HdlSelCharaDeck.gameObject.activeSelf, true);
		if (this.animateBg == null)
		{
			return;
		}
		if (!this.animateBg.MoveNext())
		{
			this.animateBg = null;
		}
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x00133258 File Offset: 0x00131458
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.winPanel.activeSelf)
		{
			return;
		}
		if (this.ienum != null)
		{
			return;
		}
		if (button == this.guiData.BtnStart)
		{
			if (this.dayOfWeekData != null)
			{
				this.ienum = (this.guiData.BtnStart.ActEnable ? this.DeckOpen() : this.PlayOver());
				return;
			}
		}
		else if (button == this.guiData.BtnAgain)
		{
			if (this.dayOfWeekData != null)
			{
				this.ienum = this.PlayRecover();
				return;
			}
		}
		else
		{
			if (button == this.guiData.BtnInfo)
			{
				this.ienum = this.InfoDisp();
				return;
			}
			if (button == this.guiData.BtnMission)
			{
				this.ienum = this.MissionDisp();
				return;
			}
			if (button == this.guiData.BtnShop)
			{
				ShopData shopData = DataManager.DmShop.GetShopDataList(true, true, ShopData.TabCategory.ALL).Find((ShopData item) => item.category == ShopData.Category.TRAINING);
				SceneShopArgs sceneShopArgs = new SceneShopArgs();
				sceneShopArgs.resultNextSceneName = SceneManager.SceneName.SceneTraining;
				sceneShopArgs.resultNextSceneArgs = new SceneTraining.Args();
				sceneShopArgs.shopId = ((shopData == null) ? 0 : shopData.shopId);
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneShop, sceneShopArgs);
				return;
			}
			if (button == this.guiData.BtnSeason)
			{
				this.ienum = this.SeasonDisp();
				return;
			}
			if (button == this.guiData.BtnRank)
			{
				this.ienum = this.RankDisp();
				return;
			}
			if (button == this.guiData.BtnScore)
			{
				this.ienum = this.ScoreDisp();
				return;
			}
			if (button == this.guiData.BtnItem)
			{
				this.ienum = this.ItemDisp();
				return;
			}
			if (button == this.guiData.BtnQuestion)
			{
				this.ienum = this.PracticeRuleDisp();
			}
		}
	}

	// Token: 0x06001904 RID: 6404 RVA: 0x00133449 File Offset: 0x00131649
	private bool OnClickToggle(PguiToggleButtonCtrl toggle, int index)
	{
		this.toggleEvent = this.OnClickPracticeToggleEvent();
		return true;
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x00133458 File Offset: 0x00131658
	private bool OnClickButtonMenu(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.winPanel.activeSelf || this.ienum != null;
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x00133474 File Offset: 0x00131674
	private void OnClickButtonRetrun()
	{
		if (this.winPanel.activeSelf || this.ienum != null)
		{
			return;
		}
		if (this.guiData.baseObj.activeSelf)
		{
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneQuest, null);
			return;
		}
		if (CanvasManager.HdlSelCharaDeck.gameObject.activeSelf)
		{
			if (!CanvasManager.HdlSelCharaDeck.OnClickMenuReturn(delegate
			{
				CanvasManager.HdlSelCharaDeck.SetActive(false, false);
			}))
			{
				this.ienum = this.DeckClose();
			}
		}
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x001334FF File Offset: 0x001316FF
	private bool onClickTabSeason(int idx)
	{
		this.SeasonListDisp(idx);
		return true;
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x00133509 File Offset: 0x00131709
	private bool onClickTabRank(int idx)
	{
		this.RankListDisp(idx);
		return true;
	}

	// Token: 0x06001909 RID: 6409 RVA: 0x00133513 File Offset: 0x00131713
	private bool onClickTabScore(int idx)
	{
		this.ScoreDeckDisp(idx);
		return true;
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x00133520 File Offset: 0x00131720
	private void OnClickParty(PguiButtonCtrl button)
	{
		if (this.winPanel.activeSelf && this.winRank.win.FinishedOpen())
		{
			PguiDataHolder component = button.GetComponent<PguiDataHolder>();
			if (this.dayOfRankList == null || component == null || component.id < 0)
			{
				return;
			}
			this.winPanel.SetActive(true);
			this.winParty.win.ForceOpen();
			TrainingRankingData.RankingOne rankingOne = this.dayOfRankList[component.id];
			this.winParty.name.text = rankingOne.userName + "のパーティ";
			for (int i = 0; i < this.winParty.icon.Count; i++)
			{
				CharaPackData charaPackData = ((rankingOne.deckInfo.deckData.Count <= i) ? null : rankingOne.deckInfo.deckData[i]);
				this.winParty.icon[i].Setup(charaPackData, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
				this.winParty.icon[i].DispMarkEvent(false, false, false);
				List<PhotoPackData> list = ((rankingOne.deckInfo.equipPhotoList == null || rankingOne.deckInfo.equipPhotoList.Count <= i) ? null : rankingOne.deckInfo.equipPhotoList[i]);
				for (int j = 0; j < this.winParty.photo[i].Count; j++)
				{
					PhotoPackData photoPackData = ((list == null || list.Count <= j) ? null : list[j]);
					if (photoPackData != null && photoPackData.IsInvalid())
					{
						photoPackData = null;
					}
					this.winParty.photo[i][j].Replace((photoPackData == null) ? (-1) : ((photoPackData.staticData.baseData.type == PhotoDef.Type.ABILITY) ? 1 : ((photoPackData.staticData.baseData.type == PhotoDef.Type.PARAMETER) ? 2 : (-1))));
					int num = ((charaPackData == null || charaPackData.dynamicData.PhotoPocket == null || charaPackData.dynamicData.PhotoPocket.Count <= j || !charaPackData.dynamicData.PhotoPocket[j].Flag) ? 3 : 2);
					if (photoPackData == null)
					{
						if (num == 3)
						{
							num = 1;
						}
					}
					else if (num == 2 && (!photoPackData.staticData.baseData.kizunaPhotoFlg || photoPackData.staticData.GetId() == charaPackData.staticData.baseData.kizunaPhotoId))
					{
						num = 0;
					}
					PguiReplaceSpriteCtrl component2 = this.winParty.photo[i][j].transform.Find("Icon_Status").GetComponent<PguiReplaceSpriteCtrl>();
					component2.gameObject.SetActive(num > 0);
					component2.Replace(num);
					this.winParty.photo[i][j].transform.Find("Num_Lv").GetComponent<PguiTextCtrl>().text = ((num == 3 || num == 1) ? "" : charaPackData.dynamicData.PhotoPocket[j].Step.ToString());
				}
			}
		}
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x00133851 File Offset: 0x00131A51
	private void SetupItem(int index, GameObject go)
	{
		go.GetComponent<PguiButtonCtrl>().SetActEnable(false, false, true);
		this.UpdateItem(index, go);
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x0013386C File Offset: 0x00131A6C
	private void UpdateItem(int index, GameObject go)
	{
		TrainingStaticData.RewardData rewardData = ((index >= 0 && index < this.rewardList.Count) ? this.rewardList[index] : null);
		List<ItemInput> list = ((rewardData == null) ? null : rewardData.rewardItemList);
		if (list == null)
		{
			list = new List<ItemInput>();
		}
		go.transform.Find("BaseImage/Txt_Num").GetComponent<PguiTextCtrl>().text = ((rewardData == null) ? "-" : rewardData.PointRangeUnder.ToString());
		int num = 0;
		for (;;)
		{
			Transform transform = go.transform.Find("BaseImage/Icon_Item" + (num + 1).ToString("D2"));
			if (transform == null)
			{
				break;
			}
			if (num >= list.Count)
			{
				transform.gameObject.SetActive(false);
			}
			else
			{
				transform.gameObject.SetActive(true);
				IconItemCtrl iconItemCtrl = transform.GetComponentInChildren<IconItemCtrl>();
				if (iconItemCtrl == null)
				{
					(iconItemCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item).GetComponent<IconItemCtrl>()).transform.SetParent(transform, false);
				}
				iconItemCtrl.Setup(new ItemData(list[num].itemId, list[num].num));
			}
			num++;
		}
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x001339A3 File Offset: 0x00131BA3
	private void SetupSeason(int index, GameObject go)
	{
		this.UpdateSeason(index, go);
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x001339B0 File Offset: 0x00131BB0
	private void UpdateSeason(int index, GameObject go)
	{
		SeasonTrainingRankingData.RankingOne rankingOne = ((this.seasonList != null && index >= 0 && index < this.seasonList.Count) ? this.seasonList[index] : null);
		Transform transform = go.transform.Find("Num_Rank");
		transform.Find("Rank_1").gameObject.SetActive(rankingOne != null && rankingOne.number <= 1);
		transform.Find("Rank_2").gameObject.SetActive(rankingOne != null && rankingOne.number == 2);
		transform.Find("Rank_3").gameObject.SetActive(rankingOne != null && rankingOne.number == 3);
		transform.Find("Rank_4_10").gameObject.SetActive(rankingOne != null && rankingOne.number >= 4 && rankingOne.number <= 10);
		transform.Find("Rank_11_100").gameObject.SetActive(rankingOne != null && rankingOne.number >= 11 && rankingOne.number <= 100);
		transform.Find("Rank_101_200").gameObject.SetActive(rankingOne != null && rankingOne.number >= 101);
		transform.Find("Rank_4_10/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "" : rankingOne.number.ToString());
		transform.Find("Rank_11_100/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "" : rankingOne.number.ToString());
		transform.Find("Rank_101_200/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "" : rankingOne.number.ToString());
		transform = go.transform.Find("BaseImage");
		transform.Find("Num_Rank").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", (rankingOne == null) ? "-" : rankingOne.userLevel.ToString());
		transform.Find("UserName").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "" : rankingOne.userName);
		transform.Find("Ranking/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "0" : rankingOne.rankingPoint.ToString());
		transform.Find("Total/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "0" : rankingOne.totalGoodScore.ToString());
		transform.Find("Achievement").GetComponent<AchievementCtrl>().Setup((rankingOne == null) ? 0 : rankingOne.achievementId, true, false);
		IconCharaCtrl iconCharaCtrl = transform.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>();
		if (iconCharaCtrl == null)
		{
			iconCharaCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara).GetComponent<IconCharaCtrl>();
			iconCharaCtrl.transform.SetParent(transform.Find("Icon_Chara"), false);
		}
		iconCharaCtrl.Setup((rankingOne == null) ? null : CharaPackData.MakeInitial(rankingOne.favoriteCharaId), SortFilterDefine.SortType.INVALID, false, null, 0, (rankingOne == null) ? (-1) : rankingOne.favoriteCharaFaceId, 0);
		iconCharaCtrl.DispRanking();
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x00133CC0 File Offset: 0x00131EC0
	private IEnumerator SeasonDisp()
	{
		DataManager.DmTraining.RequestGetSeasonTrainingRanking(this.seasonId);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		this.winPanel.SetActive(true);
		this.winSeason.win.ForceOpen();
		this.SeasonListDisp(this.winSeason.tab.SelectIndex);
		while (this.winPanel.activeSelf)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x00133CD0 File Offset: 0x00131ED0
	private void SeasonListDisp(int idx)
	{
		idx = 1 - idx;
		SeasonTrainingRankingData seasonTrainingRankingData = ((idx >= 0 && idx < DataManager.DmTraining.GetSeasonTrainingRankingData().Count) ? DataManager.DmTraining.GetSeasonTrainingRankingData()[idx] : null);
		bool flag = idx <= 0;
		this.winSeason.resultConfirm.SetActive(seasonTrainingRankingData != null && !flag && seasonTrainingRankingData.isTallyFinish);
		this.winSeason.resultCounting.SetActive(seasonTrainingRankingData != null && !flag && !seasonTrainingRankingData.isTallyFinish);
		DateTime dateTime = ((seasonTrainingRankingData == null) ? new DateTime(1900, 1, 1) : seasonTrainingRankingData.lastUpdateTime);
		this.winSeason.lastDate.text = ((dateTime.Year >= 2000 && flag) ? dateTime.ToString("最終更新\u3000yyyy/MM/dd\u3000HH:mm") : "");
		dateTime = ((seasonTrainingRankingData == null) ? new DateTime(1900, 1, 1) : DataManager.DmTraining.GetTrainingEndTime(seasonTrainingRankingData.seasonId));
		this.winSeason.seasonDate.text = ((dateTime.Year >= 2000) ? dateTime.ToString("yyyy/MM/dd\u3000HH:mm\u3000まで") : "");
		this.winSeason.myRank.SetActive(seasonTrainingRankingData != null && seasonTrainingRankingData.myRankingNo > 0);
		if (seasonTrainingRankingData != null)
		{
			this.winSeason.myRankNo.text = "自分の順位<size=28>" + seasonTrainingRankingData.myRankingNo.ToString() + "</size>";
		}
		this.seasonList = ((seasonTrainingRankingData == null) ? null : seasonTrainingRankingData.rankingList);
		this.winSeason.scroll.Resize((this.seasonList == null) ? 0 : this.seasonList.Count, 0);
		this.winSeason.noRank.SetActive(this.seasonList == null || this.seasonList.Count <= 0);
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x00133EAC File Offset: 0x001320AC
	private void SetupRank(int index, GameObject go)
	{
		go.GetComponent<PguiButtonCtrl>().SetActEnable(false, false, true);
		PguiButtonCtrl component = go.transform.Find("BaseImage/Btn_UserParty").GetComponent<PguiButtonCtrl>();
		component.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickParty), PguiButtonCtrl.SoundType.DEFAULT);
		PguiDataHolder pguiDataHolder = component.GetComponent<PguiDataHolder>();
		if (pguiDataHolder == null)
		{
			pguiDataHolder = component.gameObject.AddComponent<PguiDataHolder>();
		}
		pguiDataHolder.id = index;
		this.UpdateRank(index, go);
	}

	// Token: 0x06001912 RID: 6418 RVA: 0x00133F1C File Offset: 0x0013211C
	private void UpdateRank(int index, GameObject go)
	{
		TrainingRankingData.RankingOne rankingOne = ((this.dayOfRankList != null && index >= 0 && index < this.dayOfRankList.Count) ? this.dayOfRankList[index] : null);
		go.transform.Find("BaseImage/Btn_UserParty").GetComponent<PguiDataHolder>().id = ((rankingOne == null) ? (-1) : index);
		Transform transform = go.transform.Find("Num_Rank");
		transform.Find("Rank_1").gameObject.SetActive(rankingOne != null && rankingOne.number <= 1);
		transform.Find("Rank_2").gameObject.SetActive(rankingOne != null && rankingOne.number == 2);
		transform.Find("Rank_3").gameObject.SetActive(rankingOne != null && rankingOne.number == 3);
		transform.Find("Rank_4_10").gameObject.SetActive(rankingOne != null && rankingOne.number >= 4 && rankingOne.number <= 10);
		transform.Find("Rank_11_20").gameObject.SetActive(rankingOne != null && rankingOne.number >= 11);
		transform.Find("Rank_4_10/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "" : rankingOne.number.ToString());
		transform.Find("Rank_11_20/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "" : rankingOne.number.ToString());
		transform = go.transform.Find("BaseImage");
		transform.Find("Num_Rank").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", (rankingOne == null) ? "-" : rankingOne.userLevel.ToString());
		transform.Find("UserName").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "" : rankingOne.userName);
		transform.Find("Contents/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "0" : rankingOne.point.ToString());
		transform.Find("Achievement").GetComponent<AchievementCtrl>().Setup((rankingOne == null) ? 0 : rankingOne.achievementId, true, false);
		IconCharaCtrl iconCharaCtrl = transform.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>();
		if (iconCharaCtrl == null)
		{
			iconCharaCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara).GetComponent<IconCharaCtrl>();
			iconCharaCtrl.transform.SetParent(transform.Find("Icon_Chara"), false);
		}
		iconCharaCtrl.Setup((rankingOne == null) ? null : CharaPackData.MakeInitial(rankingOne.favoriteCharaId), SortFilterDefine.SortType.INVALID, false, null, 0, (rankingOne == null) ? (-1) : rankingOne.favoriteCharaFaceId, 0);
		iconCharaCtrl.DispRanking();
	}

	// Token: 0x06001913 RID: 6419 RVA: 0x001341C7 File Offset: 0x001323C7
	private IEnumerator RankDisp()
	{
		DataManager.DmTraining.RequestGetTrainingRanking(this.seasonId);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		DateTime lastUpdateTime = DataManager.DmTraining.GetTrainingRankingData().lastUpdateTime;
		this.winRank.update.text = ((lastUpdateTime.Year < 2000) ? "" : lastUpdateTime.ToString("最終更新\u3000yyyy/MM/dd\u3000HH:mm"));
		this.winPanel.SetActive(true);
		this.winRank.win.ForceOpen();
		this.RankListDisp(this.winRank.tab.SelectIndex);
		while (this.winPanel.activeSelf)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001914 RID: 6420 RVA: 0x001341D8 File Offset: 0x001323D8
	private void RankListDisp(int idx)
	{
		if (++idx > 6)
		{
			idx -= 7;
		}
		this.dayOfRankList = (DataManager.DmTraining.GetTrainingRankingData().dayOfWeekRankingList.ContainsKey((DayOfWeek)idx) ? DataManager.DmTraining.GetTrainingRankingData().dayOfWeekRankingList[(DayOfWeek)idx] : new List<TrainingRankingData.RankingOne>());
		this.winRank.scroll.Resize(this.dayOfRankList.Count, 0);
		this.winRank.noRank.SetActive(this.dayOfRankList.Count <= 0);
	}

	// Token: 0x06001915 RID: 6421 RVA: 0x00134269 File Offset: 0x00132469
	private IEnumerator ScoreDisp()
	{
		DataManager.DmTraining.RequestGetTrainingMyScore(this.seasonId);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		this.winPanel.SetActive(true);
		this.winScore.win.ForceOpen();
		this.ScoreDeckDisp(this.winScore.tab.SelectIndex);
		while (this.winPanel.activeSelf)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001916 RID: 6422 RVA: 0x00134278 File Offset: 0x00132478
	private void ScoreDeckDisp(int idx)
	{
		if (++idx > 6)
		{
			idx -= 7;
		}
		this.winScore.week.text = SceneTraining.weekList[idx] + "ベストスコア";
		TrainingMineHistory.DayOfWeekHistory dayOfWeekHistory = (DataManager.DmTraining.GetTrainingMineHistory().dayOfWeekDataList.ContainsKey((DayOfWeek)idx) ? DataManager.DmTraining.GetTrainingMineHistory().dayOfWeekDataList[(DayOfWeek)idx] : null);
		this.winScore.score.text = ((dayOfWeekHistory == null) ? "記録なし" : dayOfWeekHistory.point.ToString());
		this.winScore.date.text = ((dayOfWeekHistory == null) ? "" : dayOfWeekHistory.updateTime.ToString("MM月dd日"));
		for (int i = 0; i < this.winScore.icon.Count; i++)
		{
			CharaPackData charaPackData = ((dayOfWeekHistory == null || dayOfWeekHistory.deckInfo.deckData.Count <= i) ? null : dayOfWeekHistory.deckInfo.deckData[i]);
			this.winScore.icon[i].Setup(charaPackData, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			this.winScore.icon[i].DispPhotoPocketLevel(true);
			this.winScore.icon[i].DispMarkEvent(false, false, false);
			List<PhotoPackData> list = ((dayOfWeekHistory == null || dayOfWeekHistory.deckInfo.equipPhotoList == null || dayOfWeekHistory.deckInfo.equipPhotoList.Count <= i) ? null : dayOfWeekHistory.deckInfo.equipPhotoList[i]);
			for (int j = 0; j < this.winScore.photo[i].Count; j++)
			{
				PhotoPackData photoPackData = ((list == null || list.Count <= j) ? null : list[j]);
				if (photoPackData != null && photoPackData.IsInvalid())
				{
					photoPackData = null;
				}
				int num = 2;
				if (charaPackData == null || charaPackData.dynamicData.PhotoPocket == null || charaPackData.dynamicData.PhotoPocket.Count <= j || !charaPackData.dynamicData.PhotoPocket[j].Flag)
				{
					num = 1;
				}
				else if (photoPackData == null)
				{
					num = 3;
				}
				this.winScore.photo[i][j].transform.parent.parent.Find("Img_Blank").GetComponent<PguiReplaceSpriteCtrl>().Replace(num);
				this.winScore.photo[i][j].Setup(photoPackData, SortFilterDefine.SortType.LEVEL, false, false, -1, false);
				this.winScore.photo[i][j].DispDrop(false, 0);
				if (photoPackData != null)
				{
					this.winScore.photo[i][j].DispImgDisable(num == 1 || (photoPackData.staticData.baseData.kizunaPhotoFlg && photoPackData.staticData.GetId() != charaPackData.staticData.baseData.kizunaPhotoId));
					this.winScore.photo[i][j].DispMarkNotYetReleased(num == 1);
				}
				this.winScore.photo[i][j].transform.parent.parent.Find("Num_Lv").GetComponent<PguiTextCtrl>().text = ((num == 1) ? "" : charaPackData.dynamicData.PhotoPocket[j].Step.ToString());
			}
		}
	}

	// Token: 0x06001917 RID: 6423 RVA: 0x00134612 File Offset: 0x00132812
	private IEnumerator ItemDisp()
	{
		if ((this.rewardList = DataManager.DmTraining.GetTrainingPackData().staticData.rewardList) == null)
		{
			this.rewardList = new List<TrainingStaticData.RewardData>();
		}
		this.winPanel.SetActive(true);
		this.winItem.win.ForceOpen();
		this.winItem.scroll.Resize(this.rewardList.Count, 0);
		yield return null;
		yield break;
	}

	// Token: 0x06001918 RID: 6424 RVA: 0x00134621 File Offset: 0x00132821
	private IEnumerator InfoDisp()
	{
		SceneTraining.SetupInfo(this.dayOfWeekData, null);
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowTrainingEnemyInfo.FinishedClose());
		yield break;
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x00134630 File Offset: 0x00132830
	private IEnumerator PracticeRuleDisp()
	{
		CmnFeedPageWindowCtrl hdlCmnFeedPageWindowCtrl = CanvasManager.HdlCmnFeedPageWindowCtrl;
		CmnFeedPageWindowCtrl.Type type = CmnFeedPageWindowCtrl.Type.PAGE_FEED;
		string text = "";
		List<string> list = new List<string>();
		list.Add("Texture2D/Tutorial_Window/Training/tutorial_training_06");
		hdlCmnFeedPageWindowCtrl.Open(type, text, list, delegate(bool b)
		{
		});
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlCmnFeedPageWindowCtrl.FinishedClose());
		yield break;
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x00134638 File Offset: 0x00132838
	public static void SetupInfo(TrainingStaticData.DayOfWeekData dowd, PguiOpenWindowCtrl.Callback cb)
	{
		RectTransform windowRectTransform = CanvasManager.HdlOpenWindowTrainingEnemyInfo.WindowRectTransform;
		windowRectTransform.Find("Base_Info/txt_Week").GetComponent<PguiTextCtrl>().text = "【" + SceneTraining.weekList[(int)((dowd == null) ? DayOfWeek.Sunday : dowd.dayOfWeek)] + "】";
		PguiRawImageCtrl component = windowRectTransform.Find("Base_Info/Img_Enemy/Icon_Enemy").GetComponent<PguiRawImageCtrl>();
		if (dowd == null)
		{
			component.SetTexture(null, false);
		}
		else
		{
			component.SetRawImage("Texture2D/Icon_Enemy/" + dowd.enemyTexturePath, true, false, null);
		}
		windowRectTransform.Find("Base_Info/Img_Enemy/Atr").GetComponent<PguiImageCtrl>().SetImageByName(SceneTraining.GetBossAttr(dowd));
		windowRectTransform.Find("Base_Info/Txt01").GetComponent<PguiTextCtrl>().text = ((dowd == null) ? "" : dowd.captureInfoText);
		CanvasManager.HdlOpenWindowTrainingEnemyInfo.Setup(null, null, null, true, cb, null, false);
		CanvasManager.HdlOpenWindowTrainingEnemyInfo.ForceOpen();
	}

	// Token: 0x0600191B RID: 6427 RVA: 0x0013471C File Offset: 0x0013291C
	public static string GetBossAttr(TrainingStaticData.DayOfWeekData dowd)
	{
		CharaDef.AttributeType attributeType = CharaDef.AttributeType.ALL;
		QuestOnePackData questOnePackData = ((dowd == null) ? null : DataManager.DmQuest.GetQuestOnePackData(dowd.questOneId));
		if (questOnePackData != null && questOnePackData.questOne.waveData.waveList.Count > 0)
		{
			foreach (QuestStaticWave.EnemyData enemyData in questOnePackData.questOne.waveData.waveList[0].enemyList)
			{
				EnemyStaticData enemyStaticData = DataManager.DmChara.GetEnemyStaticData(enemyData.charaId);
				if (enemyStaticData != null)
				{
					if (attributeType == CharaDef.AttributeType.ALL)
					{
						attributeType = enemyStaticData.baseData.attribute;
					}
					if (enemyStaticData.baseData.charaType == CharaDef.Type.BOSS)
					{
						attributeType = enemyStaticData.baseData.attribute;
						break;
					}
				}
			}
		}
		string text = "";
		if (attributeType == CharaDef.AttributeType.RED)
		{
			text = "icon_atr_r";
		}
		else if (attributeType == CharaDef.AttributeType.GREEN)
		{
			text = "icon_atr_g";
		}
		else if (attributeType == CharaDef.AttributeType.BLUE)
		{
			text = "icon_atr_b";
		}
		else if (attributeType == CharaDef.AttributeType.PINK)
		{
			text = "icon_atr_r2";
		}
		else if (attributeType == CharaDef.AttributeType.LIME)
		{
			text = "icon_atr_g2";
		}
		else if (attributeType == CharaDef.AttributeType.AQUA)
		{
			text = "icon_atr_b2";
		}
		return text;
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x0013484C File Offset: 0x00132A4C
	private IEnumerator MissionDisp()
	{
		string text = "\n強敵を" + this.dayOfWeekData.missionConditions.ToString() + "体倒すごとに以下のボーナス効果が発動！\n";
		text += "<color=#ff0000>";
		foreach (TrainingStaticData.DayOfWeekData.MissionBonus missionBonus in this.dayOfWeekData.missionBonusList)
		{
			switch (missionBonus.type)
			{
			case TrainingStaticData.DayOfWeekData.MissionBonus.Type.MASTER_SKILL:
				text = text + "\n隊長スキル使用回数" + missionBonus.val.ToString() + "回復";
				break;
			case TrainingStaticData.DayOfWeekData.MissionBonus.Type.WAIT_SKILL:
				text = text + "\n味方全体たいきスキル使用回数" + missionBonus.val.ToString() + "回復";
				break;
			case TrainingStaticData.DayOfWeekData.MissionBonus.Type.OKAWARI:
				text = text + "\nおかわり回数＋" + missionBonus.val.ToString();
				break;
			case TrainingStaticData.DayOfWeekData.MissionBonus.Type.HP:
				text = text + "\n味方全体たいりょく" + missionBonus.val.ToString() + "％回復";
				break;
			case TrainingStaticData.DayOfWeekData.MissionBonus.Type.MP:
				text = text + "\n味方全体ＭＰ＋" + missionBonus.val.ToString();
				break;
			}
		}
		text += "</color>";
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("道場ミッション"), PrjUtil.MakeMessage(text), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		yield break;
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x0013485B File Offset: 0x00132A5B
	private IEnumerator DeckOpen()
	{
		if (this.guiData.baseObj.activeSelf)
		{
			this.guiData.anime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
				this.guiData.baseObj.SetActive(false);
			});
		}
		while (this.guiData.baseObj.activeSelf)
		{
			yield return null;
		}
		CanvasManager.HdlCmnMenu.SetupMenu(true, "道場パーティ選択", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickButtonMenu), null);
		CanvasManager.HdlSelCharaDeck.SetActive(true, false);
		this.setupParam = new SelCharaDeckCtrl.SetupParam
		{
			callScene = SceneManager.SceneName.SceneTraining,
			callbackGotoBattle = new SelCharaDeckCtrl.OnClickGotoBattle(this.OnClickBattleButton),
			deckCategory = UserDeckData.Category.TRAINING,
			trainingDay = this.dayOfWeek,
			attrIndex = 0,
			helperPackData = null,
			isPractice = this.isPractice
		};
		CanvasManager.HdlSelCharaDeck.Setup(this.setupParam, this.dayOfWeekData.questOneId);
		if (this.sta != null)
		{
			if (this.sta.openCharaWindow)
			{
				CanvasManager.HdlCharaWindowCtrl.OpenPrev();
				this.sta.openCharaWindow = false;
			}
			else if (this.sta.openPhotoWindow)
			{
				CanvasManager.HdlPhotoWindowCtrl.OpenPrev();
				this.sta.openPhotoWindow = false;
			}
			else if (this.sta.openAccessoryWindow)
			{
				CanvasManager.HdlAccessoryWindowCtrl.OpenPrev();
				this.sta.openAccessoryWindow = false;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x0013486A File Offset: 0x00132A6A
	private bool OnClickBattleButton()
	{
		if (this.winPanel.activeSelf || this.ienum != null)
		{
			return false;
		}
		if (this.dayOfWeekData != null)
		{
			this.ienum = this.BattleStart();
		}
		return true;
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x00134898 File Offset: 0x00132A98
	private IEnumerator DeckClose()
	{
		while (CanvasManager.HdlSelCharaDeck.gameObject.activeSelf)
		{
			yield return null;
		}
		this.guiData.baseObj.SetActive(true);
		this.guiData.anime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START_SUB, delegate
		{
			this.guiData.anime.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START);
		});
		CanvasManager.HdlCmnMenu.SetupMenu(true, "シーサーバル道場", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickButtonMenu), null);
		yield return null;
		yield break;
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x001348A7 File Offset: 0x00132AA7
	private IEnumerator PlayRecover()
	{
		GameObject winObj = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/GUI_QuestRevival_Window"));
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, winObj.transform, true);
		SelQuestCountRecoveryWindowCtrl.Window win = new SelQuestCountRecoveryWindowCtrl.Window(winObj.transform);
		win.SetupWindowGui(new SelQuestCountRecoveryWindowCtrl.SetupGuiParts
		{
			itemId = 30100,
			needItemNum = DataManager.DmTraining.GetTrainingPackData().staticData.RecoveryStoneNum,
			todayRecoveryNum = DataManager.DmTraining.GetTrainingPackData().dynamicData.todayRecoveryNum,
			recoveryMaxNum = DataManager.DmTraining.GetTrainingPackData().staticData.RecoveryMax,
			limitClearNum = 1
		});
		Transform transform = winObj.transform.Find("Window_QuestRevival/Base/Window/RevivalOK/LayoutGroup/PurchaseConfirmButton");
		if (transform != null)
		{
			PguiButtonCtrl component = transform.GetComponent<PguiButtonCtrl>();
			if (component != null)
			{
				transform.gameObject.SetActive(true);
				component.AddOnClickListener(delegate(PguiButtonCtrl btn)
				{
					CanvasManager.HdlPurchaseConfirmWindow.Initialize("シーサーバル道場の挑戦回数の回復", "キラキラ", DataManager.DmTraining.GetTrainingPackData().staticData.RecoveryStoneNum, null, PurchaseConfirmWindow.TEMP_IMMEDIATE_DELIVERY, false);
				}, PguiButtonCtrl.SoundType.DEFAULT);
			}
		}
		int choice = -1;
		win.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int idx)
		{
			choice = idx;
			if (idx < 0)
			{
				choice = 0;
			}
			return true;
		}, null, false);
		win.owCtrl.Open();
		while (choice < 0)
		{
			DateTime terminalTimeByDay = new DateTime(TimeManager.Now.Year, TimeManager.Now.Month, TimeManager.Now.Day);
			terminalTimeByDay = TimeManager.GetTerminalTimeByDay(terminalTimeByDay);
			win.SetupTxtCaution("回復しても0:00になると挑戦できなくなります\n" + TimeManager.MakeTimeResidueText(TimeManager.Now, terminalTimeByDay, false, true));
			yield return null;
		}
		if (choice > 0)
		{
			DataManager.DmTraining.RequestActionRecoveryPlayNum(this.seasonId, this.dayOfWeek);
		}
		do
		{
			yield return null;
		}
		while (!win.owCtrl.FinishedClose() || DataManager.IsServerRequesting());
		win = null;
		Object.Destroy(winObj);
		this.SetButton();
		yield break;
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x001348B6 File Offset: 0x00132AB6
	private IEnumerator PlayOver()
	{
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("本日残り回数が0回となりました"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		yield break;
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x001348BE File Offset: 0x00132ABE
	private IEnumerator BattleStart()
	{
		SelCharaDeckCtrl.EditResultData erd = CanvasManager.HdlSelCharaDeck.GetEditResultData();
		QuestOnePackData qopd = DataManager.DmQuest.GetQuestOnePackData(this.dayOfWeekData.questOneId);
		if (qopd.questOne.useItemId > 0)
		{
			ItemData userItemData = DataManager.DmItem.GetUserItemData(qopd.questOne.useItemId);
			if (userItemData.num < qopd.questOne.useItemNum)
			{
				CanvasManager.HdlOpenWindowBasic.Setup("確認", userItemData.staticData.GetName() + "が不足しています\n\n必要数\u3000" + qopd.questOne.useItemNum.ToString(), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int idx) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.ForceOpen();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				CanvasManager.HdlSelCharaDeck.CancelBattleStart();
				yield break;
			}
		}
		else
		{
			StaminaRecoveryWindowCtrl hdlStaminaRecoveryWindowCtrl = CanvasManager.HdlStaminaRecoveryWindowCtrl;
			IEnumerator checkStamina = hdlStaminaRecoveryWindowCtrl.StaminaCheckAction(this.dayOfWeekData.questOneId, 1);
			bool staminaChkResult = false;
			while (checkStamina.MoveNext())
			{
				staminaChkResult = checkStamina.Current != null && (bool)checkStamina.Current;
				yield return null;
			}
			if (!staminaChkResult)
			{
				CanvasManager.HdlSelCharaDeck.CancelBattleStart();
				yield break;
			}
			checkStamina = null;
		}
		int sel = 0;
		int num = (int)this.dayOfWeek;
		num = ((num <= 0) ? 207 : (200 + num));
		if (num != erd.currentDeckId)
		{
			string text = "<color=#ff0000>選択中のパーティは本日の曜日と合っていません</color>";
			text += "\n\n選択パーティに間違いはありませんか？";
			CanvasManager.HdlOpenWindowBasic.Setup("確認", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int idx)
			{
				sel = idx;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.ForceOpen();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			if (sel < 1)
			{
				CanvasManager.HdlSelCharaDeck.CancelBattleStart();
				yield break;
			}
		}
		if (!this.isPractice)
		{
			sel = 0;
			string text = "このクエストは１日１度だけ挑戦できます";
			text += "\nバトルが開始するとやり直しはできません";
			text += "\n\nパーティ設定に間違いはありませんか？";
			CanvasManager.HdlOpenWindowBasic.Setup("確認", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int idx)
			{
				sel = idx;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.ForceOpen();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			if (sel < 1)
			{
				CanvasManager.HdlSelCharaDeck.CancelBattleStart();
				yield break;
			}
			DataManager.DmTraining.RequestActionTrainingStart(this.seasonId, this.dayOfWeek, this.dayOfWeekData.questOneId, erd.currentDeckId);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
		}
		SceneBattleArgs sceneBattleArgs = new SceneBattleArgs();
		sceneBattleArgs.oppUser = null;
		sceneBattleArgs.difficulty = PvpDynamicData.EnemyInfo.Difficulty.INVALID;
		sceneBattleArgs.pvpBoard = null;
		sceneBattleArgs.hash_id = DataManager.DmTraining.LastTrainingStartResponse.hashId;
		sceneBattleArgs.questOneId = this.dayOfWeekData.questOneId;
		sceneBattleArgs.waveEnemiesIdList = new List<int>();
		sceneBattleArgs.dropItemList = new List<DrewItem>();
		sceneBattleArgs.startTime = TimeManager.Now;
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventDataList().Find((DataManagerEvent.EventData itm) => itm.eventChapterId == qopd.questChapter.chapterId);
		sceneBattleArgs.eventId = ((eventData == null) ? 0 : eventData.eventId);
		sceneBattleArgs.selectDeckId = erd.currentDeckId;
		sceneBattleArgs.helper = null;
		sceneBattleArgs.attrIndex = 0;
		sceneBattleArgs.resultNextSceneName = SceneManager.SceneName.SceneTraining;
		sceneBattleArgs.resultNextSceneArgs = new SceneTraining.Args();
		sceneBattleArgs.isQuestNoClear = qopd.questDynamicOne.clearNum == 0;
		sceneBattleArgs.trainingDay = this.dayOfWeek;
		sceneBattleArgs.trainingSeasonId = this.seasonId;
		if ((sceneBattleArgs.trainingTurn = this.dayOfWeekData.turnLimit) < 1)
		{
			sceneBattleArgs.trainingTurn = 1;
		}
		sceneBattleArgs.trainingMission = this.dayOfWeekData.missionConditions;
		sceneBattleArgs.trainingMissionList = new List<TrainingStaticData.DayOfWeekData.MissionBonus>(this.dayOfWeekData.missionBonusList);
		sceneBattleArgs.trainingHp = this.dayOfWeekData.enemyRevivalHpratio;
		sceneBattleArgs.trainingAtk = this.dayOfWeekData.enemyRevivalAtkratio;
		sceneBattleArgs.trainingDef = this.dayOfWeekData.enemyRevivalDefratio;
		sceneBattleArgs.trainingScore = DataManager.DmTraining.GetTrainingPackData().dynamicData.hiScore;
		sceneBattleArgs.isPractice = this.isPractice;
		SceneBattle.SetRestart(sceneBattleArgs);
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneBattle, sceneBattleArgs);
		yield break;
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x001348CD File Offset: 0x00132ACD
	private IEnumerator OnClickPracticeToggleEvent()
	{
		MstTrainingPracticeTrialData trial = DataManager.DmTraining.GetValidPracticeTrialData();
		this.trialInfo = DataManager.DmTraining.GetTrainingTrialInfo();
		if (!this.isPractice && trial != null)
		{
			if (this.trialInfo != null)
			{
				TrainingTrialInfo trainingTrialInfo = this.trialInfo;
				int? num = ((trainingTrialInfo != null) ? new int?(trainingTrialInfo.trialId) : null);
				int id = trial.id;
				if ((num.GetValueOrDefault() == id) & (num != null))
				{
					goto IL_00DA;
				}
			}
			DataManager.DmTraining.RequestTrainingTrialInfo(trial.id);
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
		}
		IL_00DA:
		if (this.trialInfo == null)
		{
			this.trialInfo = DataManager.DmTraining.GetTrainingTrialInfo();
		}
		DataManagerMonthlyPack.PurchaseMonthlypackData validMonthlyPackData = DataManager.DmMonthlyPack.GetValidMonthlyPackData();
		if (this.trialInfo == null && (validMonthlyPackData == null || !validMonthlyPackData.PracticeFlag))
		{
			this.guiData.BtnModeChange.SetToggleIndex(0);
			IEnumerator check = this.CheckTrial(trial.id, trial.periodDay);
			do
			{
				yield return null;
			}
			while (check.MoveNext());
			yield break;
		}
		if (!this.IsInTrial(this.trialInfo) && validMonthlyPackData == null)
		{
			if (validMonthlyPackData == null || this.IsEnableMonthlyPurchase())
			{
				this.CheckMonthlyPurchase();
				this.guiData.BtnModeChange.SetToggleIndex(0);
				yield break;
			}
		}
		else if (!this.IsInTrial(this.trialInfo) && validMonthlyPackData != null && !validMonthlyPackData.PracticeFlag)
		{
			this.guiData.BtnModeChange.SetToggleIndex(0);
			this.OpenTrialWarning();
			yield break;
		}
		this.isPractice = !this.isPractice;
		this.guiData.TxtMode.text = (this.isPractice ? "ON" : "OFF");
		if (this.currentFadeTime != 0f)
		{
			this.currentFadeTime = CanvasManager.FADE_DURATION - this.currentFadeTime;
		}
		if (!this.isPractice)
		{
			this.StopPopup();
		}
		this.animateBg = this.AnimateBg();
		this.SetButton();
		yield break;
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x001348DC File Offset: 0x00132ADC
	private IEnumerator AnimateBg()
	{
		do
		{
			Singleton<CanvasManager>.Instance.ChangeBg(this.guiData.bg, 0, "PanelBg_Training", true, this.isPractice, false, new UnityAction(this.UpdatePopupStatus), "");
			yield return null;
		}
		while (this.currentFadeTime < CanvasManager.FADE_DURATION);
		yield break;
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x001348EC File Offset: 0x00132AEC
	private void UpdatePopupStatus()
	{
		if (this.isPractice)
		{
			this.animationQueue = new Queue<SimpleAnimation>();
			this.animationQueue.Enqueue(this.guiData.Popup.Animation);
			this.PlayPopup(this.animationQueue);
			return;
		}
		this.StopPopup();
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x0013493C File Offset: 0x00132B3C
	private void PlayPopup(Queue<SimpleAnimation> animationQueue)
	{
		this.StopPopup();
		if (0 >= animationQueue.Count)
		{
			return;
		}
		SimpleAnimation animation = animationQueue.Dequeue();
		this.guiData.Popup.baseObj.SetActive(true);
		animation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
		{
			animationQueue.Enqueue(animation);
			this.PlayPopup(animationQueue);
		});
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x001349B1 File Offset: 0x00132BB1
	private void StopPopup()
	{
		this.guiData.Popup.Animation.ExStop(true);
		this.guiData.Popup.baseObj.SetActive(false);
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x001349E0 File Offset: 0x00132BE0
	private void CheckMonthlyPurchase()
	{
		string text = "練習モードを使用するには\n月間パスポート「すごい!!!ごーじゃす」の購入が必要です\n月間パスポートの購入画面に遷移しますか？";
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage(text), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			if (index == 1)
			{
				CanvasManager.HdlSelMonthlyPackWindowCtrl.Setup();
			}
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x00134A40 File Offset: 0x00132C40
	private void OpenTrialWarning()
	{
		string text = "練習モードを使用するには\n月間パスポート「すごい!!!ごーじゃす」の購入が必要です\n※他の月間パスポート有効期間中は、使用することはできません";
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage(text), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			if (index == 1)
			{
				CanvasManager.HdlSelMonthlyPackWindowCtrl.Setup();
			}
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x00134A9F File Offset: 0x00132C9F
	private IEnumerator CheckTrial(int trialId, int periodDay)
	{
		string text = "練習モードのお試しが可能です\nお試しで練習モードを使用しますか\n※期間は<color=red>" + TimeManager.Now.Date.AddDays((double)(periodDay + 1)).AddSeconds(-1.0).ToString("yyyy/MM/dd HH:mm") + "</color>までです";
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage(text), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			if (index == 1)
			{
				DataManager.DmTraining.RequestTrainingJoinTrial(trialId);
			}
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		if (!DataManager.IsServerRequesting())
		{
			yield break;
		}
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		yield break;
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x00134AB8 File Offset: 0x00132CB8
	private bool IsEnableMonthlyPurchase()
	{
		DateTime dateTime = new DateTime(TimeManager.Now.Year, TimeManager.Now.Month, TimeManager.Now.Day);
		int num = (new DateTime(DataManager.DmMonthlyPack.nowPackData.EndDatetime.Year, DataManager.DmMonthlyPack.nowPackData.EndDatetime.Month, DataManager.DmMonthlyPack.nowPackData.EndDatetime.Day) - dateTime).Days;
		DataManagerMonthlyPack.PurchaseMonthlypackData monthlypackData = DataManager.DmMonthlyPack.nextPackData.MonthlypackData;
		int days = (new DateTime(DataManager.DmMonthlyPack.nextPackData.EndDatetime.Year, DataManager.DmMonthlyPack.nextPackData.EndDatetime.Month, DataManager.DmMonthlyPack.nextPackData.EndDatetime.Day) - dateTime).Days;
		if (num >= 0)
		{
			if (num < days)
			{
				num = days;
			}
		}
		else if (monthlypackData != null && num < days)
		{
			num = days;
		}
		DataManagerMonthlyPack.PurchaseMonthlypackMessageData purchaseMonthlypackMessageData = ((DataManager.DmMonthlyPack.purchaseMonthlypackMessageDataList.Count > 0) ? DataManager.DmMonthlyPack.purchaseMonthlypackMessageDataList[0] : null);
		return num > 0 && num - purchaseMonthlypackMessageData.ReminderDay >= 0;
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x00134C14 File Offset: 0x00132E14
	private bool IsInTrial(TrainingTrialInfo info)
	{
		if (info == null)
		{
			return false;
		}
		DateTime now = TimeManager.Now;
		DateTime dateTime = new DateTime(PrjUtil.ConvertTimeToTicks(info.endTime));
		return now < dateTime;
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x00134C43 File Offset: 0x00132E43
	public override void OnStopControl()
	{
		if (this.guiData.baseObj.activeSelf)
		{
			this.guiData.anime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		}
		this.voiceTime = 0f;
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x00134C74 File Offset: 0x00132E74
	public override void OnDisableScene()
	{
		this.ienum = null;
		this.guiData.charaL.Destroy();
		this.guiData.charaR.Destroy();
		this.winItem.scroll.Resize(0, 0);
		this.winRank.scroll.Resize(0, 0);
		foreach (IconCharaCtrl iconCharaCtrl in this.winScore.icon)
		{
			iconCharaCtrl.Setup(null, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
		}
		foreach (List<IconPhotoCtrl> list in this.winScore.photo)
		{
			foreach (IconPhotoCtrl iconPhotoCtrl in list)
			{
				iconPhotoCtrl.Setup(null, SortFilterDefine.SortType.LEVEL, true, false, -1, false);
			}
		}
		CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
		this.winPanel.SetActive(false);
		this.guiData.baseObj.SetActive(false);
		CanvasManager.HdlSelCharaDeck.SetActive(false, false);
		SoundManager.UnloadCueSheet(SceneTraining.voiceSheet);
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x00134DE4 File Offset: 0x00132FE4
	public override bool OnDisableSceneWait()
	{
		return true;
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x00134DE8 File Offset: 0x00132FE8
	public override void OnDestroyScene()
	{
		Object.Destroy(this.guiData.baseObj);
		this.guiData.baseObj = null;
		this.guiData = null;
		this.winItem = null;
		this.winRank = null;
		this.winScore = null;
		this.winParty = null;
		this.winSeason = null;
		Object.Destroy(this.winPanel);
		this.winPanel = null;
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x00134E60 File Offset: 0x00133060
	// Note: this type is marked as 'beforefieldinit'.
	static SceneTraining()
	{
		string[,] array = new string[7, 2];
		array[0, 0] = "prd_cv_dojo_shiserval_lefty_sunday_1";
		array[0, 1] = "prd_cv_dojo_shiserval_right_sunday_1";
		array[1, 0] = "prd_cv_dojo_shiserval_right_monday_1";
		array[1, 1] = "prd_cv_dojo_shiserval_lefty_monday_2";
		array[2, 0] = "prd_cv_dojo_shiserval_right_tuesday_1";
		array[2, 1] = "prd_cv_dojo_shiserval_lefty_tuesday_2";
		array[3, 0] = "prd_cv_dojo_shiserval_lefty_wednesday_1";
		array[3, 1] = "prd_cv_dojo_shiserval_right_wednesday_2";
		array[4, 0] = "prd_cv_dojo_shiserval_right_thursday_1";
		array[4, 1] = "prd_cv_dojo_shiserval_lefty_thursday_2";
		array[5, 0] = "prd_cv_dojo_shiserval_lefty_friday_2";
		array[5, 1] = "prd_cv_dojo_shiserval_right_friday_1";
		array[6, 0] = "prd_cv_dojo_shiserval_right_saturday_2";
		array[6, 1] = "prd_cv_dojo_shiserval_lefty_saturday_1";
		SceneTraining.voiceList = array;
	}

	// Token: 0x04001319 RID: 4889
	private SceneTraining.GUI guiData;

	// Token: 0x0400131A RID: 4890
	private GameObject winPanel;

	// Token: 0x0400131B RID: 4891
	private SceneTraining.WIN_ITEM winItem;

	// Token: 0x0400131C RID: 4892
	private SceneTraining.WIN_RANK winRank;

	// Token: 0x0400131D RID: 4893
	private SceneTraining.WIN_SCORE winScore;

	// Token: 0x0400131E RID: 4894
	private SceneTraining.WIN_PARTY winParty;

	// Token: 0x0400131F RID: 4895
	private SceneTraining.WIN_SEASON winSeason;

	// Token: 0x04001320 RID: 4896
	private DayOfWeek dayOfWeek;

	// Token: 0x04001321 RID: 4897
	private int seasonId;

	// Token: 0x04001322 RID: 4898
	private TrainingStaticData.DayOfWeekData dayOfWeekData;

	// Token: 0x04001323 RID: 4899
	private static readonly List<string> weekList = new List<string> { "日曜日", "月曜日", "火曜日", "水曜日", "木曜日", "金曜日", "土曜日" };

	// Token: 0x04001324 RID: 4900
	private List<TrainingRankingData.RankingOne> dayOfRankList;

	// Token: 0x04001325 RID: 4901
	private List<TrainingStaticData.RewardData> rewardList;

	// Token: 0x04001326 RID: 4902
	private List<SeasonTrainingRankingData.RankingOne> seasonList;

	// Token: 0x04001327 RID: 4903
	private static readonly string voiceSheet = "cv_dojo";

	// Token: 0x04001328 RID: 4904
	private IEnumerator voiceLoad;

	// Token: 0x04001329 RID: 4905
	private IEnumerator animateBg;

	// Token: 0x0400132A RID: 4906
	private TrainingTrialInfo trialInfo;

	// Token: 0x0400132B RID: 4907
	private bool isPractice;

	// Token: 0x0400132C RID: 4908
	private Queue<SimpleAnimation> animationQueue;

	// Token: 0x0400132D RID: 4909
	private static readonly string[,] voiceList;

	// Token: 0x0400132E RID: 4910
	private float voiceTime;

	// Token: 0x0400132F RID: 4911
	private IEnumerator ienum;

	// Token: 0x04001330 RID: 4912
	private IEnumerator toggleEvent;

	// Token: 0x04001331 RID: 4913
	private SceneTraining.Args sta;

	// Token: 0x04001332 RID: 4914
	private SelCharaDeckCtrl.SetupParam setupParam = new SelCharaDeckCtrl.SetupParam();

	// Token: 0x02000D90 RID: 3472
	public class Args
	{
		// Token: 0x04004F4F RID: 20303
		public bool tutorial;

		// Token: 0x04004F50 RID: 20304
		public bool deck;

		// Token: 0x04004F51 RID: 20305
		public bool openPhotoWindow;

		// Token: 0x04004F52 RID: 20306
		public bool openCharaWindow;

		// Token: 0x04004F53 RID: 20307
		public bool openAccessoryWindow;
	}

	// Token: 0x02000D91 RID: 3473
	public class GUI
	{
		// Token: 0x06004994 RID: 18836 RVA: 0x0022148C File Offset: 0x0021F68C
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.anime = baseTr.GetComponent<SimpleAnimation>();
			this.BtnStart = baseTr.Find("All/Center/Info_Enemy/Btn_Start").GetComponent<PguiButtonCtrl>();
			this.BtnAgain = baseTr.Find("All/Center/Info_Enemy/Btn_Again").GetComponent<PguiButtonCtrl>();
			this.BtnInfo = baseTr.Find("All/Center/Info_Enemy/Btn_Info").GetComponent<PguiButtonCtrl>();
			this.BtnMission = baseTr.Find("All/Center/Info_Enemy/Btn_Mission").GetComponent<PguiButtonCtrl>();
			this.BtnShop = baseTr.Find("All/Left/Shop/Btn_ShopEvent").GetComponent<PguiButtonCtrl>();
			this.BtnSeason = baseTr.Find("All/Right/Score/Btn_SeasonRank").GetComponent<PguiButtonCtrl>();
			this.BtnRank = baseTr.Find("All/Right/Score/Btn_ScoreRank").GetComponent<PguiButtonCtrl>();
			this.BtnScore = baseTr.Find("All/Right/Score/Btn_MyScore").GetComponent<PguiButtonCtrl>();
			this.BtnItem = baseTr.Find("All/Right/Score/Btn_GetItem").GetComponent<PguiButtonCtrl>();
			this.week = baseTr.Find("All/Center/Info_Week/Week").GetComponent<PguiTextCtrl>();
			this.serifL = baseTr.Find("All/Left/CharSerif_L/Txt_Serif").GetComponent<PguiTextCtrl>();
			this.serifR = baseTr.Find("All/Right/CharSerif_R/Txt_Serif").GetComponent<PguiTextCtrl>();
			this.charaL = baseTr.Find("All/Left/RenderChara_Left").GetComponent<PguiRenderTextureCharaCtrl>();
			this.charaR = baseTr.Find("All/Right/RenderChara_Right").GetComponent<PguiRenderTextureCharaCtrl>();
			this.icon = baseTr.Find("All/Center/Info_Enemy/Img_Enemy/Icon_Enemy").GetComponent<PguiRawImageCtrl>();
			this.attr = baseTr.Find("All/Center/Info_Enemy/Img_Enemy/Atr").GetComponent<PguiImageCtrl>();
			this.coin = baseTr.Find("All/Left/Shop/ItemOwnBase01/Num_Txt").GetComponent<PguiTextCtrl>();
			this.BtnModeChange = baseTr.Find("All/Right/Practice/Btn_ModeChange").GetComponent<PguiToggleButtonCtrl>();
			this.BtnQuestion = baseTr.Find("All/Right/Practice/Btn_Question").GetComponent<PguiButtonCtrl>();
			this.TxtMode = baseTr.Find("All/Right/Practice/Btn_ModeChange/BaseImage/Txt").GetComponent<PguiTextCtrl>();
			this.Popup = new PopUpCtrl();
			this.Popup.baseObj = baseTr.Find("All/Center/Info_Enemy/PopUpInfo").gameObject;
			this.Popup.Animation = baseTr.Find("All/Center/Info_Enemy/PopUpInfo/Practice").GetComponent<SimpleAnimation>();
			this.Popup.Animation.ExInit();
		}

		// Token: 0x04004F54 RID: 20308
		public GameObject baseObj;

		// Token: 0x04004F55 RID: 20309
		public SimpleAnimation anime;

		// Token: 0x04004F56 RID: 20310
		public PguiButtonCtrl BtnStart;

		// Token: 0x04004F57 RID: 20311
		public PguiButtonCtrl BtnAgain;

		// Token: 0x04004F58 RID: 20312
		public PguiButtonCtrl BtnInfo;

		// Token: 0x04004F59 RID: 20313
		public PguiButtonCtrl BtnMission;

		// Token: 0x04004F5A RID: 20314
		public PguiButtonCtrl BtnShop;

		// Token: 0x04004F5B RID: 20315
		public PguiButtonCtrl BtnSeason;

		// Token: 0x04004F5C RID: 20316
		public PguiButtonCtrl BtnRank;

		// Token: 0x04004F5D RID: 20317
		public PguiButtonCtrl BtnScore;

		// Token: 0x04004F5E RID: 20318
		public PguiButtonCtrl BtnItem;

		// Token: 0x04004F5F RID: 20319
		public PguiTextCtrl week;

		// Token: 0x04004F60 RID: 20320
		public PguiTextCtrl serifL;

		// Token: 0x04004F61 RID: 20321
		public PguiTextCtrl serifR;

		// Token: 0x04004F62 RID: 20322
		public PguiRenderTextureCharaCtrl charaL;

		// Token: 0x04004F63 RID: 20323
		public PguiRenderTextureCharaCtrl charaR;

		// Token: 0x04004F64 RID: 20324
		public PguiRawImageCtrl icon;

		// Token: 0x04004F65 RID: 20325
		public PguiImageCtrl attr;

		// Token: 0x04004F66 RID: 20326
		public PguiTextCtrl coin;

		// Token: 0x04004F67 RID: 20327
		public RawImage bg;

		// Token: 0x04004F68 RID: 20328
		public PguiToggleButtonCtrl BtnModeChange;

		// Token: 0x04004F69 RID: 20329
		public PguiButtonCtrl BtnQuestion;

		// Token: 0x04004F6A RID: 20330
		public PguiTextCtrl TxtMode;

		// Token: 0x04004F6B RID: 20331
		public PopUpCtrl Popup;
	}

	// Token: 0x02000D92 RID: 3474
	public class WIN_ITEM
	{
		// Token: 0x06004995 RID: 18837 RVA: 0x002216C0 File Offset: 0x0021F8C0
		public WIN_ITEM(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.scroll = baseTr.Find("List/ScrollView").GetComponent<ReuseScroll>();
		}

		// Token: 0x04004F6C RID: 20332
		public PguiOpenWindowCtrl win;

		// Token: 0x04004F6D RID: 20333
		public ReuseScroll scroll;
	}

	// Token: 0x02000D93 RID: 3475
	public class WIN_RANK
	{
		// Token: 0x06004996 RID: 18838 RVA: 0x002216F8 File Offset: 0x0021F8F8
		public WIN_RANK(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.tab = baseTr.Find("Tab_All/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.scroll = baseTr.Find("Tab_All/Rank/ScrollView").GetComponent<ReuseScroll>();
			this.noRank = baseTr.Find("Tab_All/Txt_NoRank").gameObject;
			this.update = baseTr.Find("Text_Date").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x04004F6E RID: 20334
		public PguiOpenWindowCtrl win;

		// Token: 0x04004F6F RID: 20335
		public PguiTabGroupCtrl tab;

		// Token: 0x04004F70 RID: 20336
		public ReuseScroll scroll;

		// Token: 0x04004F71 RID: 20337
		public GameObject noRank;

		// Token: 0x04004F72 RID: 20338
		public PguiTextCtrl update;
	}

	// Token: 0x02000D94 RID: 3476
	public class WIN_SCORE
	{
		// Token: 0x06004997 RID: 18839 RVA: 0x0022177C File Offset: 0x0021F97C
		public WIN_SCORE(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.tab = baseTr.Find("Tab_All/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.date = baseTr.Find("Tab_All/Header/Txt_Date02").GetComponent<PguiTextCtrl>();
			this.week = baseTr.Find("Tab_All/Header/Score_Base/Txt_PartyName").GetComponent<PguiTextCtrl>();
			this.score = baseTr.Find("Tab_All/Header/Score_Base/Num_Score").GetComponent<PguiTextCtrl>();
			this.icon = new List<IconCharaCtrl>();
			this.photo = new List<List<IconPhotoCtrl>>();
			int num = 1;
			for (;;)
			{
				Transform transform = baseTr.Find("Tab_All/Score/CharaIconSet" + num.ToString("D2") + "/Icon_Chara");
				if (transform == null)
				{
					break;
				}
				Transform transform2 = transform.parent.Find("Cover");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(false);
				}
				Transform transform3 = transform.parent.Find("Mark_Friend");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(false);
				}
				Transform transform4 = transform.parent.Find("PhotoIconKind");
				if (transform4 != null)
				{
					transform4.gameObject.SetActive(false);
				}
				Transform transform5 = transform.parent.Find("Base_CharaBlank_Friend");
				if (transform5 != null)
				{
					transform5.gameObject.SetActive(false);
				}
				Transform transform6 = transform.parent.Find("AEImage_Mark_Ban");
				if (transform6 != null)
				{
					transform6.gameObject.SetActive(false);
				}
				Transform transform7 = transform.parent.Find("AccessoryIconView");
				if (transform7 != null)
				{
					transform7.gameObject.SetActive(false);
				}
				IconCharaCtrl iconCharaCtrl = transform.GetComponentInChildren<IconCharaCtrl>();
				if (iconCharaCtrl == null)
				{
					iconCharaCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara).GetComponent<IconCharaCtrl>();
					iconCharaCtrl.transform.SetParent(transform, false);
				}
				this.icon.Add(iconCharaCtrl);
				transform = transform.parent.Find("PhotoIconView");
				List<IconPhotoCtrl> list = new List<IconPhotoCtrl>();
				int num2 = 1;
				for (;;)
				{
					Transform transform8 = transform.Find("Icon_Photo" + num2.ToString("D2") + "/Icon_Photo");
					if (transform8 == null)
					{
						break;
					}
					IconPhotoCtrl iconPhotoCtrl = transform8.GetComponentInChildren<IconPhotoCtrl>();
					if (iconPhotoCtrl == null)
					{
						iconPhotoCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo_Mini).GetComponent<IconPhotoCtrl>();
						iconPhotoCtrl.transform.SetParent(transform8, false);
					}
					list.Add(iconPhotoCtrl);
					num2++;
				}
				this.photo.Add(list);
				num++;
			}
		}

		// Token: 0x04004F73 RID: 20339
		public PguiOpenWindowCtrl win;

		// Token: 0x04004F74 RID: 20340
		public PguiTabGroupCtrl tab;

		// Token: 0x04004F75 RID: 20341
		public PguiTextCtrl date;

		// Token: 0x04004F76 RID: 20342
		public PguiTextCtrl week;

		// Token: 0x04004F77 RID: 20343
		public PguiTextCtrl score;

		// Token: 0x04004F78 RID: 20344
		public List<IconCharaCtrl> icon;

		// Token: 0x04004F79 RID: 20345
		public List<List<IconPhotoCtrl>> photo;
	}

	// Token: 0x02000D95 RID: 3477
	public class WIN_PARTY
	{
		// Token: 0x06004998 RID: 18840 RVA: 0x002219F0 File Offset: 0x0021FBF0
		public WIN_PARTY(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.name = baseTr.Find("Title/Text").GetComponent<PguiTextCtrl>();
			this.icon = new List<IconCharaCtrl>();
			this.photo = new List<List<PguiReplaceSpriteCtrl>>();
			int num = 1;
			for (;;)
			{
				Transform transform = baseTr.Find("CharaIconSet" + num.ToString("D2") + "/Icon_Chara");
				if (transform == null)
				{
					break;
				}
				Transform transform2 = transform.parent.Find("Cover");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(false);
				}
				Transform transform3 = transform.parent.Find("Mark_Friend");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(false);
				}
				Transform transform4 = transform.parent.Find("PhotoIconView");
				if (transform4 != null)
				{
					transform4.gameObject.SetActive(false);
				}
				Transform transform5 = transform.parent.Find("Base_CharaBlank_Friend");
				if (transform5 != null)
				{
					transform5.gameObject.SetActive(false);
				}
				Transform transform6 = transform.parent.Find("AEImage_Mark_Ban");
				if (transform6 != null)
				{
					transform6.gameObject.SetActive(false);
				}
				Transform transform7 = transform.parent.Find("AccessoryIconView");
				if (transform7 != null)
				{
					transform7.gameObject.SetActive(false);
				}
				IconCharaCtrl iconCharaCtrl = transform.GetComponentInChildren<IconCharaCtrl>();
				if (iconCharaCtrl == null)
				{
					iconCharaCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara).GetComponent<IconCharaCtrl>();
					iconCharaCtrl.transform.SetParent(transform, false);
				}
				this.icon.Add(iconCharaCtrl);
				transform = transform.parent.Find("PhotoIconKind");
				List<PguiReplaceSpriteCtrl> list = new List<PguiReplaceSpriteCtrl>();
				int num2 = 1;
				for (;;)
				{
					Transform transform8 = transform.Find("Icon_PhotoKind" + num2.ToString("D2"));
					if (transform8 == null)
					{
						break;
					}
					list.Add(transform8.GetComponent<PguiReplaceSpriteCtrl>());
					num2++;
				}
				this.photo.Add(list);
				num++;
			}
		}

		// Token: 0x04004F7A RID: 20346
		public PguiOpenWindowCtrl win;

		// Token: 0x04004F7B RID: 20347
		public PguiTextCtrl name;

		// Token: 0x04004F7C RID: 20348
		public List<IconCharaCtrl> icon;

		// Token: 0x04004F7D RID: 20349
		public List<List<PguiReplaceSpriteCtrl>> photo;
	}

	// Token: 0x02000D96 RID: 3478
	public class WIN_SEASON
	{
		// Token: 0x06004999 RID: 18841 RVA: 0x00221BE8 File Offset: 0x0021FDE8
		public WIN_SEASON(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.tab = baseTr.Find("Tab_All/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.resultConfirm = baseTr.Find("Result").gameObject;
			this.resultCounting = baseTr.Find("Result_No").gameObject;
			this.lastDate = baseTr.Find("Text_Date").GetComponent<PguiTextCtrl>();
			this.seasonDate = baseTr.Find("Text_Date_Limit").GetComponent<PguiTextCtrl>();
			this.myRank = baseTr.Find("MyRank").gameObject;
			this.myRankNo = this.myRank.transform.Find("Txt").GetComponent<PguiTextCtrl>();
			this.noRank = baseTr.Find("Tab_All/Txt_NoRank").gameObject;
			this.scroll = baseTr.Find("Tab_All/Rank/ScrollView").GetComponent<ReuseScroll>();
		}

		// Token: 0x04004F7E RID: 20350
		public PguiOpenWindowCtrl win;

		// Token: 0x04004F7F RID: 20351
		public PguiTabGroupCtrl tab;

		// Token: 0x04004F80 RID: 20352
		public GameObject resultConfirm;

		// Token: 0x04004F81 RID: 20353
		public GameObject resultCounting;

		// Token: 0x04004F82 RID: 20354
		public PguiTextCtrl lastDate;

		// Token: 0x04004F83 RID: 20355
		public PguiTextCtrl seasonDate;

		// Token: 0x04004F84 RID: 20356
		public GameObject myRank;

		// Token: 0x04004F85 RID: 20357
		public PguiTextCtrl myRankNo;

		// Token: 0x04004F86 RID: 20358
		public GameObject noRank;

		// Token: 0x04004F87 RID: 20359
		public ReuseScroll scroll;
	}
}
