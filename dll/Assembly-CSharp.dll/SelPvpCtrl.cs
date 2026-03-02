using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CriWare;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016C RID: 364
public class SelPvpCtrl : MonoBehaviour
{
	// Token: 0x1700038E RID: 910
	// (get) Token: 0x0600158C RID: 5516 RVA: 0x0010DC40 File Offset: 0x0010BE40
	// (set) Token: 0x0600158B RID: 5515 RVA: 0x0010DC2C File Offset: 0x0010BE2C
	private IEnumerator currentEnumerator
	{
		get
		{
			return this._currentEnumerator;
		}
		set
		{
			if (value == null || this._currentEnumerator == null)
			{
				this._currentEnumerator = value;
			}
		}
	}

	// Token: 0x0600158D RID: 5517 RVA: 0x0010DC48 File Offset: 0x0010BE48
	public bool IsBusy()
	{
		return this.currentEnumerator != null;
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x0600158E RID: 5518 RVA: 0x0010DC53 File Offset: 0x0010BE53
	// (set) Token: 0x0600158F RID: 5519 RVA: 0x0010DC5B File Offset: 0x0010BE5B
	public SceneManager.SceneName requestNextScene { get; private set; }

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x06001590 RID: 5520 RVA: 0x0010DC64 File Offset: 0x0010BE64
	// (set) Token: 0x06001591 RID: 5521 RVA: 0x0010DC6C File Offset: 0x0010BE6C
	public object requestNextArgs { get; private set; }

	// Token: 0x06001592 RID: 5522 RVA: 0x0010DC78 File Offset: 0x0010BE78
	public void Init()
	{
		this.guiTopData = new SelPvpCtrl.GUITop(AssetManager.InstantiateAssetData("ScenePvp/GUI/Prefab/GUI_Pvp_Top", base.transform).transform);
		this.guiTopData.Btn_Nomal.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUITopNormalButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiTopData.Btn_Event.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUITopSpecialButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiTopData.baseObj.SetActive(false);
		this.guiData = new SelPvpCtrl.GUI(AssetManager.InstantiateAssetData("ScenePvp/GUI/Prefab/GUI_Pvp_Select", base.transform).transform);
		this.guiData.guiNormalEnemyScrollData.ScrollView.InitForce();
		this.guiData.guiNormalEnemyScrollData.ScrollView.onStartItem = new Action<int, GameObject>(this.OnStartItemNormalScroll);
		this.guiData.guiNormalEnemyScrollData.ScrollView.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemNormalScroll);
		this.guiData.guiNormalEnemyScrollData.ScrollView.Setup(10, 0);
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Hard.InitForce();
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Hard.onStartItem = new Action<int, GameObject>(this.OnStartItemSpecialScroll_Hard);
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Hard.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemSpecialScroll_Hard);
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Hard.Setup(10, 0);
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Normal.InitForce();
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Normal.onStartItem = new Action<int, GameObject>(this.OnStartItemSpecialScroll_Normal);
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Normal.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemSpecialScroll_Normal);
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Normal.Setup(10, 0);
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Easy.InitForce();
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Easy.onStartItem = new Action<int, GameObject>(this.OnStartItemSpecialScroll_Easy);
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Easy.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemSpecialScroll_Easy);
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Easy.Setup(10, 0);
		this.guiData.baseObj.SetActive(false);
		this.guiData.Btn_MoreInfo_Normal.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUICommonMoreInfoButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiNormalData.Btn_CharaDeck.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUICommonCharaDeckButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiNormalData.Btn_Shop.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUINormalShopButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiNormalData.Btn_Plus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUICommonPlusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiNormalData.Btn_ModeChange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUINormalModeChangeButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiNormalEnemyScrollData.Btn_Update.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickUpdateButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_MoreInfo_Special.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUICommonMoreInfoButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiSpecialData.Btn_ModeChange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUISpecialModeChangeButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiSpecialData.Btn_CharaDeck.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUICommonCharaDeckButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiSpecialData.Btn_Shop.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUISpecialShopButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiSpecialData.Btn_Plus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUICommonPlusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiSpecialData.Btn_Gacha.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUISpecialGachaButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiSpecialData.Btn_Mission.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUISpecialMissionButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiSpecialData.Btn_Tips.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUISpecialTipsButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiSpecialEnemyScrollData.Btn_Scenerio.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGUISpecialSenarioButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiSpecialEnemyScrollData.Btn_Update.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickUpdateButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiEnemyWindow = new SelPvpCtrl.GUIEnemyWindow(AssetManager.InstantiateAssetData("ScenePvp/GUI/Prefab/Pvp_Deck_Window", Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		this.guiRewardWindow = new SelPvpCtrl.GUIRewardWindow(AssetManager.InstantiateAssetData("ScenePvp/GUI/Prefab/Pvp_Reward_Window", Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		this.guiRewardWindow.TabGroup.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectRewardWindowTab));
		this.OnSelectRewardWindowTab(0);
		this.guiRewardWindow.ScrollView_RankUp.InitForce();
		this.guiRewardWindow.ScrollView_RankUp.onStartItem = new Action<int, GameObject>(this.OnStartItemOwRW01);
		this.guiRewardWindow.ScrollView_RankUp.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemOwRW01);
		this.guiRewardWindow.ScrollView_RankUp.Setup(10, 0);
		this.guiRewardWindow.ScrollView_Bonus.InitForce();
		this.guiRewardWindow.ScrollView_Bonus.onStartItem = new Action<int, GameObject>(this.OnStartItemOwRW02);
		this.guiRewardWindow.ScrollView_Bonus.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemOwRW02);
		this.guiRewardWindow.ScrollView_Bonus.Setup(10, 0);
		this.guiDefenseResultWindow = new SelPvpCtrl.GUIDefenseResultWindow(AssetManager.InstantiateAssetData("ScenePvp/GUI/Prefab/Pvp_DayItem_Window", Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		this.guiDefenseResultWindow.ScrollView.InitForce();
		this.guiDefenseResultWindow.ScrollView.onStartItem = new Action<int, GameObject>(this.OnStartItemOwDR);
		this.guiDefenseResultWindow.ScrollView.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemOwDR);
		this.guiDefenseResultWindow.ScrollView.Setup(10, 0);
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x0010E2AC File Offset: 0x0010C4AC
	public void Setup(int fastPvpSeasonId, bool isStartTutorial, bool isStartSpecialPvpTutorial, bool isReturnFromBattle, bool isReturnFromPvpDeck)
	{
		this.requestNextScene = SceneManager.SceneName.None;
		this.requestNextArgs = null;
		this.DispNormal3x();
		this.DispSpecial3x();
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		PvpPackData pvpPackDataBySeasonID = DataManager.DmPvp.GetPvpPackDataBySeasonID(fastPvpSeasonId);
		if (pvpPackDataBySeasonID != null && !isStartTutorial)
		{
			this.currentEnumerator = this.SetupMainGui(pvpPackDataBySeasonID, true, true, isStartSpecialPvpTutorial, isReturnFromBattle, isReturnFromPvpDeck);
		}
		else
		{
			this.guiTopData.baseObj.SetActive(false);
			this.guiData.baseObj.SetActive(false);
			this.currentEnumerator = this.SetupInternal(isStartTutorial);
		}
		PvpStaticData pvpStaticData = ((pvpPackDataBySeasonID != null) ? pvpPackDataBySeasonID.staticData : null);
		if (pvpStaticData == null)
		{
			int seasonIdByNow = DataManager.DmPvp.GetSeasonIdByNow(TimeManager.Now, PvpStaticData.Type.NORMAL);
			pvpStaticData = this.GetPvpStaticDataNormalForGuiTop(seasonIdByNow);
		}
		this.ChangeSetting(pvpStaticData);
	}

	// Token: 0x06001594 RID: 5524 RVA: 0x0010E36F File Offset: 0x0010C56F
	public void Setup()
	{
		this.Setup(0, false, false, false, false);
	}

	// Token: 0x06001595 RID: 5525 RVA: 0x0010E37C File Offset: 0x0010C57C
	public void Disable()
	{
		base.gameObject.SetActive(false);
		this.guiTopData.renderTexture.Destroy();
		if (this.playingVoice.GetStatus() == CriAtomExPlayback.Status.Playing)
		{
			this.playingVoice.Stop();
		}
		if (null != this.guiData.guiSpecialData.renderTexture)
		{
			Object.Destroy(this.guiData.guiSpecialData.renderTexture.gameObject);
			this.guiData.guiSpecialData.renderTexture = null;
		}
		UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
		if (this.cloneUserOptionData.LastPlaySpPvpSeasonId != userOptionData.LastPlaySpPvpSeasonId || this.cloneUserOptionData.CurrentSpPvpDifficultyTab != userOptionData.CurrentSpPvpDifficultyTab)
		{
			userOptionData.LastPlaySpPvpSeasonId = this.cloneUserOptionData.LastPlaySpPvpSeasonId;
			userOptionData.CurrentSpPvpDifficultyTab = this.cloneUserOptionData.CurrentSpPvpDifficultyTab;
			DataManager.DmUserInfo.RequestActionUpdateUserOption(userOptionData);
		}
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x0010E464 File Offset: 0x0010C664
	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
		if (this.currentPvpPackData != null)
		{
			StaminaInfo.NowInfo infoByNow = this.currentPvpPackData.dynamicData.userInfo.pvpStaminaInfo.GetInfoByNow(TimeManager.Now);
			if (PvpStaticData.Type.SPECIAL == this.currentPvpPackData.staticData.type)
			{
				this.guiData.guiSpecialData.Txt_Stamina.text = infoByNow.stackNum.ToString() + "/" + infoByNow.stackMaxNum.ToString();
				if (infoByNow.stackNum < infoByNow.stackMaxNum)
				{
					this.guiData.guiSpecialData.Num_Txt_StaminaRecovery.ReplaceTextByDefault("Param01", infoByNow.nextRecoveryTime.Minute.ToString() + ":" + infoByNow.nextRecoveryTime.Second.ToString("D2"));
					return;
				}
				this.guiData.guiSpecialData.Num_Txt_StaminaRecovery.text = "";
				return;
			}
			else
			{
				this.guiData.guiNormalData.Txt_Stamina.text = infoByNow.stackNum.ToString() + "/" + infoByNow.stackMaxNum.ToString();
				if (infoByNow.stackNum < infoByNow.stackMaxNum)
				{
					this.guiData.guiNormalData.Num_Txt_StaminaRecovery.ReplaceTextByDefault("Param01", infoByNow.nextRecoveryTime.Minute.ToString() + ":" + infoByNow.nextRecoveryTime.Second.ToString("D2"));
				}
				else
				{
					this.guiData.guiNormalData.Num_Txt_StaminaRecovery.text = "";
				}
				if (this.guiData.guiNormalData.CampaignObj.activeSelf)
				{
					this.guiData.guiNormalData.Txt_CampaignTime.text = TimeManager.MakeTimeResidueText(TimeManager.Now, this.currentCampaignEndTime, true, true);
					if (TimeManager.Now > this.currentCampaignEndTime)
					{
						this.guiData.guiNormalData.CampaignObj.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x0010E694 File Offset: 0x0010C894
	public void Destroy()
	{
		if (this.guiEnemyWindow != null)
		{
			Object.Destroy(this.guiEnemyWindow.baseObj);
			this.guiEnemyWindow = null;
		}
		if (this.guiRewardWindow != null)
		{
			Object.Destroy(this.guiRewardWindow.baseObj);
			this.guiRewardWindow = null;
		}
		if (this.guiDefenseResultWindow != null)
		{
			Object.Destroy(this.guiDefenseResultWindow.baseObj);
			this.guiDefenseResultWindow = null;
		}
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x0010E6FE File Offset: 0x0010C8FE
	private IEnumerator SetupTopGui()
	{
		PvpStaticData pvpStaticNormal = this.GetPvpStaticDataNormalForGuiTop(this.currentNormalSeasonId);
		if (this.guiData.baseObj.activeSelf)
		{
			SelPvpCtrl.<>c__DisplayClass45_0 CS$<>8__locals1 = new SelPvpCtrl.<>c__DisplayClass45_0();
			CS$<>8__locals1.isFinishBase = false;
			this.guiData.baseAnime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
				CS$<>8__locals1.isFinishBase = true;
			});
			CS$<>8__locals1.isFinishEvent = true;
			if (this.guiData.MoreInfoEvent.activeSelf)
			{
				CS$<>8__locals1.isFinishEvent = false;
				this.guiData.EventAnime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
				{
					CS$<>8__locals1.isFinishEvent = true;
				});
			}
			while (!CS$<>8__locals1.isFinishBase && !CS$<>8__locals1.isFinishEvent)
			{
				yield return null;
			}
			this.guiData.baseObj.SetActive(false);
			if (PvpStaticData.Type.SPECIAL == this.currentPvpPackData.staticData.type)
			{
				if (this.playingVoice.GetStatus() == CriAtomExPlayback.Status.Playing)
				{
					this.playingVoice.Stop();
				}
				this.ChangeSetting(pvpStaticNormal);
			}
			CS$<>8__locals1 = null;
		}
		this.guiTopData.baseAnime.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.guiTopData.baseObj.SetActive(true);
		int num = Random.Range(0, 4);
		for (int i = 0; i < this.guiTopData.LuckyImageList.Count; i++)
		{
			this.guiTopData.LuckyImageList[i].gameObject.SetActive(i != num);
		}
		RenderTextureChara renderTextureChara = this.guiTopData.renderTexture.Create();
		if (!renderTextureChara.FinishedSetup)
		{
			renderTextureChara.Setup(1004, 0, CharaMotionDefine.ActKey.SCENARIO_STAND_BY, 1, false, true, null, false, null, 0f, null, false, false, false);
		}
		renderTextureChara.DispLuckyEyeEffect(true, num);
		this.guiTopData.renderTexture.GetCharaTransform().localPosition = new Vector3(0f, 0.5f, 0f);
		DateTime now = TimeManager.Now;
		bool flag = this.ChkPvpSeasonIsEnableByTime(now, pvpStaticNormal);
		if (pvpStaticNormal != null)
		{
			QuestStaticQuestOne questStaticQuestOne = new QuestStaticQuestOne();
			questStaticQuestOne.SolutionStageTraits(pvpStaticNormal.baseData.stagePresetId);
			this.guiTopData.Txt_Stage.text = CharaDef.GetAbilityTraitsName(questStaticQuestOne.traitsType);
			this.guiTopData.Txt_Term.text = TimeManager.FormattedTime(pvpStaticNormal.seasonStartTime, TimeManager.Format.yyyyMMdd_hhmm) + "～" + TimeManager.FormattedTime(pvpStaticNormal.seasonEndTime, TimeManager.Format.yyyyMMdd_hhmm);
		}
		this.guiTopData.Txt_Stage.gameObject.SetActive(pvpStaticNormal != null);
		this.guiTopData.Txt_Term.gameObject.SetActive(pvpStaticNormal != null);
		this.guiTopData.Btn_Nomal.SetActEnable(flag, false, false);
		this.guiTopData.Btn_Nomal_Disable_Info.SetActive(pvpStaticNormal != null && !flag);
		int num2 = -1;
		int num3 = 0;
		foreach (DataManagerCampaign.CampaignPvpCoinData campaignPvpCoinData in DataManager.DmCampaign.CampaignPvPDataList)
		{
			if (now.Ticks >= campaignPvpCoinData.startTime.Ticks && now.Ticks <= campaignPvpCoinData.endTime.Ticks && num2 < campaignPvpCoinData.campaignId)
			{
				num2 = campaignPvpCoinData.campaignId;
				num3 = campaignPvpCoinData.pvpCoinRatio;
			}
		}
		this.guiTopData.CampaignObj.SetActive(flag && num2 >= 0);
		if (0 <= num2)
		{
			this.guiTopData.Txt_Campaign.text = "獲得メダル<size=24><color=#FFEE00>" + PguiCmnMenuCtrl.Ratio2String(num3) + "</color></size>倍！";
		}
		PvpStaticData pvpStaticDataBySeasonID = DataManager.DmPvp.GetPvpStaticDataBySeasonID(this.currentSpecialSeasonId);
		bool flag2 = this.ChkPvpSeasonIsEnableByTime(now, pvpStaticDataBySeasonID);
		if (flag2)
		{
			QuestStaticQuestOne questStaticQuestOne2 = new QuestStaticQuestOne();
			questStaticQuestOne2.SolutionStageTraits(pvpStaticDataBySeasonID.baseData.stagePresetId);
			this.guiTopData.Txt_Stage_Event.text = CharaDef.GetAbilityTraitsName(questStaticQuestOne2.traitsType);
			this.guiTopData.Txt_Term_Event.text = TimeManager.FormattedTime(pvpStaticDataBySeasonID.seasonStartTime, TimeManager.Format.yyyyMMdd_hhmm) + "～" + TimeManager.FormattedTime(pvpStaticDataBySeasonID.seasonEndTime, TimeManager.Format.yyyyMMdd_hhmm);
		}
		this.guiTopData.Btn_Event.SetActEnable(flag2, false, false);
		string text = (flag2 ? "Texture2D/PvpTopPhoto/pvptop_photo_1001" : "Texture2D/PvpTopPhoto/pvptop_photo_1002");
		this.guiTopData.Texture_EventOpen.SetRawImage(text, true, false, null);
		this.guiTopData.Txt_Term_Event.gameObject.SetActive(flag2);
		this.guiTopData.Txt_Stage_Event.gameObject.SetActive(flag2);
		yield return null;
		this.guiTopData.baseAnime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		yield break;
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x0010E70D File Offset: 0x0010C90D
	private IEnumerator SetupMainGui(PvpPackData info, bool isSubAnim, bool isConnectReset, bool isStartSpecialPvpTutorial, bool isReturnFromBattle, bool isReturnFromPvpDeck)
	{
		this.guiData.baseAnime.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		this.guiData.EventAnime.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		DateTime now = TimeManager.Now;
		if (info != null && !this.ChkPvpSeasonIsEnableByTime(now, info.staticData))
		{
			this.currentPvpPackData = info;
		}
		else if (isConnectReset)
		{
			DataManager.DmPvp.RequestGetPvpInfo(true, info.seasonId);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			this.currentPvpPackData = DataManager.DmPvp.GetPvpPackDataBySeasonID(info.seasonId);
			this.UpdateSelectorEffectTemporary(this.currentPvpPackData.seasonId);
		}
		else
		{
			this.currentPvpPackData = info;
		}
		bool isSpecial = PvpStaticData.Type.SPECIAL == this.currentPvpPackData.staticData.type;
		if (isSpecial)
		{
			this.currentSpecialSeasonId = this.currentPvpPackData.seasonId;
			this.currentBonusCharaList = DataManager.DmChara.GetBonusCharaDataList(this.currentPvpPackData.staticData.spEventId);
		}
		else
		{
			this.currentNormalSeasonId = this.currentPvpPackData.seasonId;
			this.currentBonusCharaList = null;
		}
		if (this.guiTopData.baseObj.activeSelf)
		{
			SelPvpCtrl.<>c__DisplayClass46_1 CS$<>8__locals2 = new SelPvpCtrl.<>c__DisplayClass46_1();
			CS$<>8__locals2.isFinish = false;
			this.guiTopData.baseAnime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
				CS$<>8__locals2.isFinish = true;
			});
			while (!CS$<>8__locals2.isFinish)
			{
				yield return null;
			}
			this.guiTopData.baseObj.SetActive(false);
			if (isSpecial)
			{
				if (!DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.SpecialPvpFirst && !isStartSpecialPvpTutorial)
				{
					this.requestNextScene = SceneManager.SceneName.SceneScenario;
					this.requestNextArgs = new SceneScenario.Args
					{
						questId = 10,
						storyType = 1,
						scenarioName = DataManager.DmQuest.QuestStaticData.oneDataMap[10].scenarioBeforeId,
						nextSceneName = SceneManager.SceneName.ScenePvp,
						nextSceneArgs = new ScenePvp.Args
						{
							fastPvpSeasonId = this.currentPvpPackData.seasonId,
							isStartSpecialPvpTutorial = true
						}
					};
					yield break;
				}
				this.ChangeSetting(this.currentPvpPackData.staticData);
			}
			CS$<>8__locals2 = null;
		}
		this.guiData.baseObj.SetActive(true);
		this.guiData.baseAnime.ExPauseAnimation(isSubAnim ? SimpleAnimation.ExPguiStatus.START_SUB : SimpleAnimation.ExPguiStatus.START, null);
		PvpRankInfo pvpRankInfo = this.currentPvpPackData.GetPvpRankInfo();
		UserDeckData deck = DataManager.DmDeck.GetUserDeckById(info.dynamicData.userInfo.currentDeckId);
		int num = 0;
		bool flag = true;
		QuestStaticQuestOne questStaticQuestOne = new QuestStaticQuestOne();
		questStaticQuestOne.SolutionStageTraits(this.currentPvpPackData.staticData.baseData.stagePresetId);
		if (isSpecial)
		{
			this.guiData.guiNormalData.baseObj.SetActive(false);
			this.guiData.LocationInfoNormal.SetActive(false);
			this.guiData.guiNormalEnemyScrollData.baseObj.SetActive(false);
			this.guiData.guiSpecialData.baseObj.SetActive(true);
			this.guiData.MoreInfoEvent.SetActive(true);
			this.guiData.guiSpecialEnemyScrollData.baseObj.SetActive(true);
			this.guiData.EventAnime.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
			string text = ScenePvp.voiceNameList[0];
			if (isReturnFromBattle)
			{
				DataManagerPvp.InformationPvpSpecialEffect.BattleResult battleResult = DataManager.DmPvp.GetInformationPvpSpecialEffect(false).battleResult;
				if (battleResult != DataManagerPvp.InformationPvpSpecialEffect.BattleResult.WIN)
				{
					if (battleResult == DataManagerPvp.InformationPvpSpecialEffect.BattleResult.LOSE)
					{
						text = ScenePvp.voiceNameList[3];
					}
				}
				else
				{
					text = ScenePvp.voiceNameList[2];
				}
			}
			else if (isReturnFromPvpDeck)
			{
				text = ScenePvp.voiceNameList[1];
			}
			else
			{
				List<PvpSpecialEffectData> releasePvpSpecialEffectList = DataManager.DmPvp.GetReleasePvpSpecialEffectList();
				if (releasePvpSpecialEffectList != null && 0 < releasePvpSpecialEffectList.Count)
				{
					int num2 = -1;
					string text2 = string.Empty;
					for (int n = 0; n < releasePvpSpecialEffectList.Count; n++)
					{
						if (!string.IsNullOrEmpty(releasePvpSpecialEffectList[n].VoiceType) && num2 < releasePvpSpecialEffectList[n].Id)
						{
							num2 = releasePvpSpecialEffectList[n].Id;
							text2 = releasePvpSpecialEffectList[n].VoiceType;
						}
					}
					if (!string.IsNullOrEmpty(text2))
					{
						text = text2;
					}
				}
			}
			if (null == this.guiData.guiSpecialData.renderTexture)
			{
				this.guiData.guiSpecialData.renderTexture = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.guiData.guiSpecialData.renderTextureBase.transform).GetComponent<RenderTextureChara>();
				this.guiData.guiSpecialData.renderTexture.postion = new Vector2(-468f, 0f);
				this.guiData.guiSpecialData.renderTexture.rotation = Vector3.zero;
				this.guiData.guiSpecialData.renderTexture.fieldOfView = 22f;
				this.guiData.guiSpecialData.renderTexture.transform.localPosition = new Vector3(468f, 0f, 0f);
			}
			this.guiData.guiSpecialData.renderTexture.Setup(197, 1, CharaMotionDefine.ActKey.JUMP_IN, 0, false, false, delegate
			{
				this.guiData.guiSpecialData.renderTexture.SetAnimation(CharaMotionDefine.ActKey.SPECIAL, true);
			}, false, null, 0f, null, true, true, false);
			this.guiData.guiSpecialData.renderTexture.AddOnTouchCharaModelListener(new UnityAction(this.OnTouchCharaModel), new UnityAction(this.OnTouchCharaModel));
			this.playingVoice = SoundManager.PlayVoice(ScenePvp.voiceSheet, text);
			this.guiData.guiSpecialData.Num_Txt_BattleRank.text = pvpRankInfo.id.ToString();
			this.guiData.guiSpecialData.Num_Txt_ItemNum.text = DataManager.DmItem.GetUserItemData(info.staticData.rewardItemId).num.ToString();
			this.guiData.guiSpecialData.Img_Item.SetRawImage(DataManager.DmItem.GetUserItemData(info.staticData.rewardItemId).staticData.GetIconName(), true, false, null);
			this.guiData.guiSpecialData.Num_Rank.ReplaceTextByDefault("Param01", DataManager.DmUserInfo.level.ToString());
			this.guiData.guiSpecialData.Txt_UserName.text = DataManager.DmUserInfo.userName;
			if (pvpRankInfo.nexRankInfo != null)
			{
				this.guiData.guiSpecialData.Num_Txt_NextBattlePoint.gameObject.SetActive(true);
				this.guiData.guiSpecialData.Obj_NextClass.SetActive(true);
				int num3 = pvpRankInfo.nexRankInfo.pointRangeLow - this.currentPvpPackData.dynamicData.userInfo.pvpPoint;
				this.guiData.guiSpecialData.Num_Txt_NextBattlePoint.text = num3.ToString() + PrjUtil.MakeMessage("pt");
			}
			else
			{
				this.guiData.guiSpecialData.Num_Txt_NextBattlePoint.gameObject.SetActive(false);
				this.guiData.guiSpecialData.Obj_NextClass.SetActive(false);
			}
			this.guiData.guiSpecialData.Txt_PartyName.text = deck.name;
			int num4 = -1;
			DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.currentPvpPackData.staticData.spEventId);
			if (eventData != null)
			{
				num4 = DataManager.DmQuest.QuestStaticData.chapterDataMap[eventData.eventChapterId].mapDataList[0].questGroupList[0].questOneList[0].questId;
			}
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(num4);
			List<DataManagerChara.BonusCharaData> list = ((num4 > 0) ? DataManager.DmChara.GetBonusCharaDataList(QuestUtil.GetEventId(num4, false)) : new List<DataManagerChara.BonusCharaData>());
			List<DataManagerChara.BonusCharaData> list2 = new List<DataManagerChara.BonusCharaData>();
			List<int> list3 = new List<int>();
			List<PhotoPackData> list4 = new List<PhotoPackData>();
			List<PhotoPackData> list5 = new List<PhotoPackData>();
			int num6;
			int i;
			for (i = 0; i < deck.charaIdList.Count; i = num6 + 1)
			{
				CharaPackData cpd = DataManager.DmChara.GetUserCharaData(deck.charaIdList[i]);
				List<PhotoPackData> list6 = deck.equipPhotoList[i].ConvertAll<PhotoPackData>((long item) => DataManager.DmPhoto.GetUserPhotoData(item));
				if (cpd != null)
				{
					PrjUtil.ParamPreset activeKizunaBuff = DataManager.DmChara.GetActiveKizunaBuff();
					PrjUtil.ParamPreset paramPreset = PrjUtil.CalcBattleParamByChara(cpd.dynamicData, list6, this.currentBonusCharaList, activeKizunaBuff);
					num += paramPreset.totalParam;
					if (DataManager.DmKemoBoard.KemoBoardBonusParamMap.ContainsKey(cpd.staticData.baseData.attribute))
					{
						num += DataManager.DmKemoBoard.KemoBoardBonusParamMap[cpd.staticData.baseData.attribute].KemoStatus;
					}
					DataManagerChara.BonusCharaData bonusCharaData = list.Find((DataManagerChara.BonusCharaData itm) => itm.charaId == cpd.dynamicData.id && (itm.increaseItemId01 != 0 || itm.increaseItemId02 != 0));
					if (bonusCharaData != null)
					{
						list2.Add(bonusCharaData);
						int num5 = cpd.dynamicData.PhotoPocket.FindAll((CharaDynamicData.PPParam itm) => itm.Flag).Count<CharaDynamicData.PPParam>();
						list3.Add(num5);
					}
					int j;
					for (j = 0; j < deck.equipPhotoList[i].Count; j = num6 + 1)
					{
						PhotoPackData photoPackData = list6.Find((PhotoPackData item) => item != null && item.dataId == deck.equipPhotoList[i][j]);
						bool flag2 = !cpd.IsInvalid() && cpd.dynamicData.PhotoPocket[j].Flag;
						bool flag3 = photoPackData != null && !photoPackData.IsInvalid();
						bool flag4 = flag3 && photoPackData.staticData.baseData.kizunaPhotoFlg && (cpd.IsInvalid() || photoPackData.staticData.GetId() != cpd.staticData.baseData.kizunaPhotoId);
						if (flag2 && flag3 && !flag4)
						{
							list4.Add(photoPackData);
							PhotoUtil.RefDropItemEffectPhotoList(ref list5, photoPackData, false);
						}
						num6 = j;
					}
				}
				this.guiData.guiSpecialData.iconCharaList[i].Setup(DataManager.DmChara.GetUserCharaData(deck.charaIdList[i]), SortFilterDefine.SortType.LEVEL, false, null, this.currentPvpPackData.staticData.spEventId, -1, 0);
				this.guiData.guiSpecialData.iconCharaList[i].DispPhotoPocketLevel(true);
				num6 = i;
			}
			for (int m = deck.charaIdList.Count; m < this.guiData.guiSpecialData.iconCharaList.Count; m++)
			{
				this.guiData.guiSpecialData.iconCharaList[m].Setup(null, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			}
			this.guiData.guiSpecialData.Info_PhotoItemEffect.Setup(QuestUtil.GetCalcDropBonusResultDeck(questOnePackData, list4, list2, list3), PhotoUtil.GetDropItemEffectPhotoDeck(questOnePackData, list5));
			this.guiData.guiSpecialData.Num_TotalAttack.text = num.ToString();
			int num7 = deck.CalcTotalPlasmPoint(false);
			this.guiData.guiSpecialData.Num_TotalPlasm.text = num7.ToString();
			bool flag5 = eventData != null && eventData.eventGachaId != 0;
			this.guiData.guiSpecialData.Btn_Gacha.gameObject.SetActive(flag5);
			flag = false;
			this.guiData.Num_Txt_Traits_Special.text = PrjUtil.MakeMessage("地形\u3000") + CharaDef.GetAbilityTraitsName(questStaticQuestOne.traitsType);
			this.guiData.Tex_Mark_Night_Special.gameObject.SetActive(questStaticQuestOne.isNightTraits);
			this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Hard = new List<PvpDynamicData.EnemyInfo>();
			this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Normal = new List<PvpDynamicData.EnemyInfo>();
			this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Easy = new List<PvpDynamicData.EnemyInfo>();
			foreach (PvpDynamicData.EnemyInfo enemyInfo in new List<PvpDynamicData.EnemyInfo>(this.currentPvpPackData.dynamicData.enemyInfoList))
			{
				switch (enemyInfo.difficulty)
				{
				case PvpDynamicData.EnemyInfo.Difficulty.HARD:
					this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Hard.Add(enemyInfo);
					break;
				case PvpDynamicData.EnemyInfo.Difficulty.NORMAL:
					this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Normal.Add(enemyInfo);
					break;
				case PvpDynamicData.EnemyInfo.Difficulty.EASY:
					this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Easy.Add(enemyInfo);
					break;
				}
			}
			this.guiData.guiSpecialEnemyScrollData.ScrollView_Hard.Resize(this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Hard.Count, 0);
			this.guiData.guiSpecialEnemyScrollData.ScrollView_Normal.Resize(this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Normal.Count, 0);
			this.guiData.guiSpecialEnemyScrollData.ScrollView_Easy.Resize(this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Easy.Count, 0);
			if (this.currentPvpPackData.seasonId != this.cloneUserOptionData.LastPlaySpPvpSeasonId)
			{
				this.cloneUserOptionData.LastPlaySpPvpSeasonId = this.currentPvpPackData.seasonId;
				this.cloneUserOptionData.CurrentSpPvpDifficultyTab = 1;
			}
			int currentSpPvpDifficultyTab = this.cloneUserOptionData.CurrentSpPvpDifficultyTab;
			this.guiData.guiSpecialEnemyScrollData.tabGroup.Setup(currentSpPvpDifficultyTab, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectGUISpecialTab));
			this.OnSelectGUISpecialTab(currentSpPvpDifficultyTab);
			int userClearEventMissionNum = DataManager.DmMission.GetUserClearEventMissionNum(this.currentPvpPackData.staticData.spEventId);
			bool flag6 = DataManager.DmEvent.GetValidEventIdListWithoutMissionEvent().Contains(this.currentPvpPackData.staticData.spEventId);
			this.guiData.guiSpecialData.Txt_Mission_Num.transform.parent.transform.parent.gameObject.SetActive(flag6 && 0 < userClearEventMissionNum);
			this.guiData.guiSpecialData.Txt_Mission_Num.text = userClearEventMissionNum.ToString();
		}
		else
		{
			this.guiData.guiNormalData.baseObj.SetActive(true);
			this.guiData.LocationInfoNormal.SetActive(true);
			this.guiData.guiNormalEnemyScrollData.baseObj.SetActive(true);
			this.guiData.guiSpecialData.baseObj.SetActive(false);
			this.guiData.MoreInfoEvent.SetActive(false);
			this.guiData.guiSpecialEnemyScrollData.baseObj.SetActive(false);
			this.guiData.guiNormalData.Num_Txt_BattleRank.text = pvpRankInfo.rankName;
			this.guiData.guiNormalData.Num_Txt_BattlePoint.text = this.currentPvpPackData.dynamicData.userInfo.pvpPoint.ToString() + PrjUtil.MakeMessage("pt");
			this.guiData.guiNormalData.Num_Txt_ItemNum.text = DataManager.DmItem.GetUserItemData(info.staticData.rewardItemId).num.ToString();
			this.guiData.guiNormalData.Num_Rank.ReplaceTextByDefault("Param01", DataManager.DmUserInfo.level.ToString());
			this.guiData.guiNormalData.Txt_UserName.text = DataManager.DmUserInfo.userName;
			if (pvpRankInfo.nexRankInfo != null)
			{
				this.guiData.guiNormalData.Num_Txt_NextBattlePoint.transform.parent.gameObject.SetActive(true);
				int num8 = pvpRankInfo.nexRankInfo.pointRangeLow - this.currentPvpPackData.dynamicData.userInfo.pvpPoint;
				this.guiData.guiNormalData.Num_Txt_NextBattlePoint.text = num8.ToString() + PrjUtil.MakeMessage("pt");
			}
			else
			{
				this.guiData.guiNormalData.Num_Txt_NextBattlePoint.transform.parent.gameObject.SetActive(false);
			}
			this.guiData.guiNormalData.Txt_PartyName.text = deck.name;
			for (int k = 0; k < deck.charaIdList.Count; k++)
			{
				CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(deck.charaIdList[k]);
				List<PhotoPackData> list7 = deck.equipPhotoList[k].ConvertAll<PhotoPackData>((long item) => DataManager.DmPhoto.GetUserPhotoData(item));
				if (userCharaData != null)
				{
					PrjUtil.ParamPreset activeKizunaBuff2 = DataManager.DmChara.GetActiveKizunaBuff();
					PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcBattleParamByChara(userCharaData.dynamicData, list7, this.currentBonusCharaList, activeKizunaBuff2);
					num += paramPreset2.totalParam;
					if (DataManager.DmKemoBoard.KemoBoardBonusParamMap.ContainsKey(userCharaData.staticData.baseData.attribute))
					{
						num += DataManager.DmKemoBoard.KemoBoardBonusParamMap[userCharaData.staticData.baseData.attribute].KemoStatus;
					}
				}
				this.guiData.guiNormalData.iconCharaList[k].Setup(DataManager.DmChara.GetUserCharaData(deck.charaIdList[k]), SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
				this.guiData.guiNormalData.iconCharaList[k].DispMarkEvent(false, false, false);
				this.guiData.guiNormalData.iconCharaList[k].DispPhotoPocketLevel(true);
			}
			for (int l = deck.charaIdList.Count; l < this.guiData.guiNormalData.iconCharaList.Count; l++)
			{
				this.guiData.guiNormalData.iconCharaList[l].Setup(null, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			}
			this.guiData.guiNormalData.Num_TotalAttack.text = num.ToString();
			int num9 = -1;
			int num10 = 0;
			foreach (DataManagerCampaign.CampaignPvpCoinData campaignPvpCoinData in DataManager.DmCampaign.CampaignPvPDataList)
			{
				if (now.Ticks >= campaignPvpCoinData.startTime.Ticks && now.Ticks <= campaignPvpCoinData.endTime.Ticks && num9 < campaignPvpCoinData.campaignId)
				{
					num9 = campaignPvpCoinData.campaignId;
					num10 = campaignPvpCoinData.pvpCoinRatio;
				}
			}
			this.guiData.guiNormalData.CampaignObj.SetActive(0 <= num9);
			if (0 <= num9)
			{
				this.currentCampaignEndTime = DataManager.DmCampaign.CampaignPvPDataList[0].endTime;
				this.guiData.guiNormalData.Txt_Campaign.text = "獲得メダル<size=24><color=#FFEE00>" + PguiCmnMenuCtrl.Ratio2String(num10) + "</color></size>倍！";
			}
			this.guiData.Num_Txt_Traits_Normal.text = PrjUtil.MakeMessage("地形\u3000") + CharaDef.GetAbilityTraitsName(questStaticQuestOne.traitsType);
			this.guiData.Tex_Mark_Night_Normal.gameObject.SetActive(questStaticQuestOne.isNightTraits);
			this.guiData.guiNormalEnemyScrollData.enemyInfoList = new List<PvpDynamicData.EnemyInfo>(this.currentPvpPackData.dynamicData.enemyInfoList);
			this.guiData.guiNormalEnemyScrollData.ScrollView.Resize(this.guiData.guiNormalEnemyScrollData.enemyInfoList.Count, 0);
		}
		this.guiRewardWindow.ScrollView_RankUp.Resize(this.currentPvpPackData.staticData.rankMstList.Count, 0);
		this.guiRewardWindow.ScrollView_Bonus.Resize(this.currentPvpPackData.staticData.pvpDefenseList.Count, 0);
		this.guiRewardWindow.Num_Txt_PvpRank.text = ((pvpRankInfo != null) ? pvpRankInfo.rankName : "");
		if (flag)
		{
			this.guiRewardWindow.Num_Txt_ResetDate.gameObject.SetActive(true);
			DateTime seasonEndTime = this.currentPvpPackData.staticData.seasonEndTime;
			this.guiRewardWindow.Num_Txt_ResetDate.ReplaceTextByDefault("Param01", TimeManager.MakeTimeResidueText(TimeManager.Now, seasonEndTime, false, true));
		}
		else
		{
			this.guiRewardWindow.Num_Txt_ResetDate.gameObject.SetActive(false);
		}
		yield return null;
		if (isSpecial && isStartSpecialPvpTutorial)
		{
			SelPvpCtrl.<>c__DisplayClass46_5 CS$<>8__locals6 = new SelPvpCtrl.<>c__DisplayClass46_5();
			this.guiData.baseAnime.ExPlayAnimation(isSubAnim ? SimpleAnimation.ExPguiStatus.START_SUB : SimpleAnimation.ExPguiStatus.START, null);
			this.guiData.EventAnime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			CS$<>8__locals6.isFinishWindow = false;
			CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/PvP_special/tutorial_pvp_special_01", "Texture2D/Tutorial_Window/PvP_special/tutorial_pvp_special_02", "Texture2D/Tutorial_Window/PvP_special/tutorial_pvp_special_03", "Texture2D/Tutorial_Window/PvP_special/tutorial_pvp_special_04", "Texture2D/Tutorial_Window/PvP_special/tutorial_pvp_special_05" }, delegate(bool b)
			{
				CS$<>8__locals6.isFinishWindow = true;
			});
			while (!CS$<>8__locals6.isFinishWindow)
			{
				yield return null;
			}
			DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
			userFlagData.TutorialFinishFlag.SpecialPvpFirst = true;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			CS$<>8__locals6 = null;
		}
		else
		{
			SelPvpCtrl.<>c__DisplayClass46_6 CS$<>8__locals7 = new SelPvpCtrl.<>c__DisplayClass46_6();
			CanvasManager.SetEnableCmnTouchMask(true);
			CS$<>8__locals7.isFinishBase = false;
			this.guiData.baseAnime.ExPlayAnimation(isSubAnim ? SimpleAnimation.ExPguiStatus.START_SUB : SimpleAnimation.ExPguiStatus.START, delegate
			{
				CS$<>8__locals7.isFinishBase = true;
			});
			CS$<>8__locals7.isFinishEvent = true;
			if (this.guiData.MoreInfoEvent.activeSelf)
			{
				CS$<>8__locals7.isFinishEvent = false;
				this.guiData.EventAnime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
				{
					CS$<>8__locals7.isFinishEvent = true;
				});
			}
			while (!CS$<>8__locals7.isFinishBase && !CS$<>8__locals7.isFinishEvent)
			{
				yield return null;
			}
			CanvasManager.SetEnableCmnTouchMask(false);
			CS$<>8__locals7 = null;
		}
		if (isSpecial && isReturnFromBattle)
		{
			IEnumerator notifyIenum = this.NotifyInfo();
			while (notifyIenum.MoveNext())
			{
				yield return null;
			}
			notifyIenum = null;
		}
		IEnumerator ienum = this.CheckAndSolutionEventInfo();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x0010E74C File Offset: 0x0010C94C
	private void UpdateSelectorEffectTemporary(int tgtSeasonId)
	{
		PvpPackData pvpPackDataBySeasonID = DataManager.DmPvp.GetPvpPackDataBySeasonID(tgtSeasonId);
		if (pvpPackDataBySeasonID == null)
		{
			return;
		}
		if (this.selectorEffectTemporary == null || this.selectorEffectTemporary.seasonId != tgtSeasonId)
		{
			this.selectorEffectTemporary = new SelPvpCtrl.SelectorEffect
			{
				seasonId = pvpPackDataBySeasonID.seasonId
			};
		}
		SelPvpCtrl.SelectorEffect selectorEffect = pvpPackDataBySeasonID.dynamicData.GetSelectorEffect();
		if (selectorEffect.isHappenReset)
		{
			this.selectorEffectTemporary.isHappenReset = true;
			this.selectorEffectTemporary.pvpRankBeforReset = selectorEffect.pvpRankBeforReset;
			this.selectorEffectTemporary.pvpSeasonIdBeforReset = selectorEffect.pvpSeasonIdBeforReset;
		}
		if (selectorEffect.isHappenDefenseBonus)
		{
			this.selectorEffectTemporary.isHappenDefenseBonus = true;
			this.selectorEffectTemporary.defenseResultList = selectorEffect.defenseResultList;
		}
	}

	// Token: 0x0600159B RID: 5531 RVA: 0x0010E7FD File Offset: 0x0010C9FD
	private bool ChkPvpSeasonIsEnableByTime(DateTime now, PvpStaticData staticData)
	{
		return staticData != null && staticData.seasonStartTime <= now && now <= staticData.seasonEndTime;
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x0010E820 File Offset: 0x0010CA20
	private void SetupCommonMenu(bool isSpecial)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, isSpecial ? "とくべつくんれん" : "ちからくらべ", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickBackButton), "", null, null);
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x0010E850 File Offset: 0x0010CA50
	private void ChangeSetting(PvpStaticData psd)
	{
		bool flag = psd != null && PvpStaticData.Type.SPECIAL == psd.type;
		this.SetupCommonMenu(flag);
		SoundManager.PlayBGM(flag ? "prd_bgm0081" : "prd_bgm0004");
		this.ChangeBGSetting(psd);
		CanvasManager.HdlHelpWindowCtrl.SetDisplaySpecialPvpHelp(flag);
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x0010E89C File Offset: 0x0010CA9C
	public void ChangeBGSetting(PvpStaticData psd)
	{
		string text = ((psd == null) ? "" : psd.baseData.bgFile);
		if (string.IsNullOrEmpty(text))
		{
			text = "PanelBg_PvP";
		}
		CanvasManager.SetBgObj(text);
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x0010E8D4 File Offset: 0x0010CAD4
	public PvpStaticData GetPvpStaticDataNormalForGuiTop(int currentSeasonId)
	{
		PvpStaticData pvpStaticData = DataManager.DmPvp.GetPvpStaticDataBySeasonID(currentSeasonId);
		if (!this.ChkPvpSeasonIsEnableByTime(TimeManager.Now, pvpStaticData))
		{
			int num = currentSeasonId - 1;
			PvpStaticData pvpStaticDataBySeasonID = DataManager.DmPvp.GetPvpStaticDataBySeasonID(num);
			if (pvpStaticDataBySeasonID != null)
			{
				pvpStaticData = pvpStaticDataBySeasonID;
			}
		}
		return pvpStaticData;
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x0010E911 File Offset: 0x0010CB11
	private IEnumerator SetupInternal(bool isStartTutorial)
	{
		DateTime now = TimeManager.Now;
		this.currentNormalSeasonId = DataManager.DmPvp.GetSeasonIdByNow(now, PvpStaticData.Type.NORMAL);
		this.currentSpecialSeasonId = DataManager.DmPvp.GetSeasonIdByNow(now, PvpStaticData.Type.SPECIAL);
		PvpStaticData pvpStaticDataBySeasonID = DataManager.DmPvp.GetPvpStaticDataBySeasonID(this.currentNormalSeasonId);
		bool flag = this.ChkPvpSeasonIsEnableByTime(now, pvpStaticDataBySeasonID);
		PvpStaticData pvpStaticDataBySeasonID2 = DataManager.DmPvp.GetPvpStaticDataBySeasonID(this.currentSpecialSeasonId);
		bool flag2 = this.ChkPvpSeasonIsEnableByTime(now, pvpStaticDataBySeasonID2);
		if (flag || flag2)
		{
			DataManager.DmPvp.RequestGetPvpInfo(false, 0);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			DataManager.DmDeck.RequestActionGetPvpDeck();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
		}
		IEnumerator ienum = this.SetupTopGui();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		if (isStartTutorial)
		{
			SelPvpCtrl.<>c__DisplayClass53_0 CS$<>8__locals1 = new SelPvpCtrl.<>c__DisplayClass53_0();
			CS$<>8__locals1.isFinishWindow = false;
			CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/PvP/tutorial_pvp_01", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_02", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_03", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_04", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_05", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_06", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_07" }, delegate(bool b)
			{
				CS$<>8__locals1.isFinishWindow = true;
			});
			while (!CS$<>8__locals1.isFinishWindow)
			{
				yield return null;
			}
			DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
			userFlagData.TutorialFinishFlag.PvpFirst = true;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			CS$<>8__locals1 = null;
		}
		yield break;
	}

	// Token: 0x060015A1 RID: 5537 RVA: 0x0010E927 File Offset: 0x0010CB27
	private IEnumerator NotifyInfo()
	{
		DataManagerPvp.InformationPvpSpecialEffect effectInfo = DataManager.DmPvp.GetInformationPvpSpecialEffect(true);
		if (effectInfo != null)
		{
			if (0 < effectInfo.releaseEffectList.Count)
			{
				foreach (PvpSpecialEffectData pvpSpecialEffectData in effectInfo.releaseEffectList)
				{
					CanvasManager.HdlOpenWindowBasic.Setup("シナリオ追加", "思い出にとくべつくんれんシナリオ\n\n" + pvpSpecialEffectData.ScenarioName + "\n\nが追加されました", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
					do
					{
						yield return null;
					}
					while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				}
				HashSet<PvpSpecialEffectData>.Enumerator enumerator = default(HashSet<PvpSpecialEffectData>.Enumerator);
			}
			if (effectInfo.specialRewardType != DataManagerPvp.SpecialRewardType.INVALID)
			{
				SelPvpCtrl.<>c__DisplayClass54_0 CS$<>8__locals1 = new SelPvpCtrl.<>c__DisplayClass54_0();
				string text = string.Empty;
				CS$<>8__locals1.message = string.Empty;
				DataManagerPvp.SpecialRewardType specialRewardType = effectInfo.specialRewardType;
				if (specialRewardType != DataManagerPvp.SpecialRewardType.CONSECUTIVE_WIN_BONUS)
				{
					if (specialRewardType == DataManagerPvp.SpecialRewardType.CONSECUTIVE_LOSE_BONUS)
					{
						text = "おうえんボーナス";
						CS$<>8__locals1.message = "おうえんボーナスを獲得しました";
					}
				}
				else
				{
					text = "連勝ボーナス";
					CS$<>8__locals1.message = "連勝ボーナスを獲得しました\n※イベント中1回";
				}
				CS$<>8__locals1.isFinishWindow = false;
				CanvasManager.HdlGetItemWindowCtrl.Setup(new List<ItemData> { effectInfo.specialRewardItem }, new GetItemWindowCtrl.SetupParam
				{
					strTitle = text,
					strCharaCb = (GetItemWindowCtrl.WordingCallbackParam param) => CS$<>8__locals1.message,
					strPhotoCb = (GetItemWindowCtrl.WordingCallbackParam param) => CS$<>8__locals1.message,
					strItemCb = (GetItemWindowCtrl.WordingCallbackParam param) => CS$<>8__locals1.message,
					isDispItemNum = true,
					windowFinishedCallback = delegate(int x)
					{
						CS$<>8__locals1.isFinishWindow = true;
						return true;
					}
				});
				CanvasManager.HdlGetItemWindowCtrl.Open();
				while (!CS$<>8__locals1.isFinishWindow)
				{
					yield return null;
				}
				CS$<>8__locals1 = null;
			}
		}
		yield break;
		yield break;
	}

	// Token: 0x060015A2 RID: 5538 RVA: 0x0010E92F File Offset: 0x0010CB2F
	private IEnumerator CheckAndSolutionEventInfo()
	{
		DateTime now = TimeManager.Now;
		if (!this.ChkPvpSeasonIsEnableByTime(now, this.currentPvpPackData.staticData))
		{
			bool isWindowFinish = false;
			CanvasManager.HdlOpenWindowBasic.Setup("", "シーズンが終了しました", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.OK), true, delegate(int index)
			{
				isWindowFinish = true;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (!isWindowFinish)
			{
				yield return null;
			}
			this.requestNextScene = SceneManager.SceneName.SceneHome;
			this.requestNextArgs = null;
			yield break;
		}
		if (this.selectorEffectTemporary != null && this.selectorEffectTemporary.isHappenDefenseBonus)
		{
			SelPvpCtrl.<>c__DisplayClass55_1 CS$<>8__locals2 = new SelPvpCtrl.<>c__DisplayClass55_1();
			this.guiDefenseResultWindow.ScrollView.Resize(this.selectorEffectTemporary.defenseResultList.Count, 0);
			CS$<>8__locals2.isWindowFinish = false;
			CS$<>8__locals2.owAnswer = 0;
			this.guiDefenseResultWindow.window.Setup(PrjUtil.MakeMessage("防衛ボーナス"), "", null, true, delegate(int index)
			{
				CS$<>8__locals2.owAnswer = index;
				CS$<>8__locals2.isWindowFinish = true;
				return true;
			}, null, false);
			this.guiDefenseResultWindow.window.Open();
			while (!CS$<>8__locals2.isWindowFinish)
			{
				yield return null;
			}
			this.selectorEffectTemporary.isHappenDefenseBonus = false;
			CS$<>8__locals2 = null;
		}
		if (PvpStaticData.Type.SPECIAL != this.currentPvpPackData.staticData.type && this.selectorEffectTemporary != null && this.selectorEffectTemporary.isHappenReset)
		{
			SelPvpCtrl.<>c__DisplayClass55_2 CS$<>8__locals3 = new SelPvpCtrl.<>c__DisplayClass55_2();
			CS$<>8__locals3.isWindowFinish = false;
			CS$<>8__locals3.owAnswer = 0;
			int pvpPoint = this.currentPvpPackData.dynamicData.userInfo.pvpPoint;
			string rankName = this.currentPvpPackData.GetPvpRankInfo().rankName;
			PvpStaticData pvpStaticDataBySeasonID = DataManager.DmPvp.GetPvpStaticDataBySeasonID(this.selectorEffectTemporary.pvpSeasonIdBeforReset);
			PvpRankInfo pvpRankInfo = ((pvpStaticDataBySeasonID != null) ? pvpStaticDataBySeasonID.GetPvpRankInfoByRankId(this.selectorEffectTemporary.pvpRankBeforReset) : null);
			string text = ((pvpRankInfo != null) ? pvpRankInfo.rankName : "");
			PguiOpenWindowCtrl component = Object.Instantiate<GameObject>(Resources.Load("ScenePvp/GUI/Prefab/Pvp_NewSeason_Window") as GameObject, Singleton<CanvasManager>.Instance.SystemMiddleArea).GetComponent<PguiOpenWindowCtrl>();
			component.Setup("新シーズン開始のお知らせ", "", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
			{
				CS$<>8__locals3.owAnswer = index;
				CS$<>8__locals3.isWindowFinish = true;
				return true;
			}, null, false);
			component.transform.Find("Base/Window/Info_01/Num_Class").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", text);
			component.transform.Find("Base/Window/Info_02/Num_Pt").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", pvpPoint.ToString());
			component.Open();
			while (!CS$<>8__locals3.isWindowFinish)
			{
				yield return null;
			}
			IEnumerator ienum = SelPvpCtrl.RankUpEvent(this.currentPvpPackData.seasonId, 0, this.currentPvpPackData.dynamicData.userInfo.pvpPoint);
			while (ienum.MoveNext())
			{
				yield return null;
			}
			ienum = null;
			CS$<>8__locals3 = null;
		}
		this.selectorEffectTemporary.isHappenReset = false;
		yield break;
	}

	// Token: 0x060015A3 RID: 5539 RVA: 0x0010E93E File Offset: 0x0010CB3E
	public static IEnumerator RankUpEvent(int pvpSeasonId, int befPvpPoint, int nowPvpPoint)
	{
		PvpStaticData pvpStaticDataBySeasonID = DataManager.DmPvp.GetPvpStaticDataBySeasonID(pvpSeasonId);
		if (pvpStaticDataBySeasonID == null)
		{
			yield break;
		}
		PvpRankInfo pvpRankInfoByPoint = pvpStaticDataBySeasonID.GetPvpRankInfoByPoint(befPvpPoint);
		PvpRankInfo pvpRankInfoByPoint2 = pvpStaticDataBySeasonID.GetPvpRankInfoByPoint(nowPvpPoint);
		if (pvpRankInfoByPoint == null)
		{
			yield break;
		}
		if (pvpRankInfoByPoint2 == null)
		{
			yield break;
		}
		if (pvpRankInfoByPoint == pvpRankInfoByPoint2)
		{
			yield break;
		}
		List<PvpRankInfo> pvpRankInfoByPointRange = pvpStaticDataBySeasonID.GetPvpRankInfoByPointRange(befPvpPoint, nowPvpPoint);
		pvpRankInfoByPointRange.Remove(pvpRankInfoByPoint);
		List<ItemData> rewardItemList = new List<ItemData>();
		for (int i = 0; i < pvpRankInfoByPointRange.Count; i++)
		{
			rewardItemList.AddRange(pvpRankInfoByPointRange[i].rewardItemList);
		}
		GameObject gameObject = Resources.Load("ScenePvp/GUI/Prefab/Pvp_Rankup_Window") as GameObject;
		Transform baseTr = Object.Instantiate<GameObject>(gameObject, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform;
		PguiAECtrl AEImageBg = baseTr.Find("Window_PvPRankUp/AEImage_Bg").GetComponent<PguiAECtrl>();
		baseTr.Find("Window_PvPRankUp/AEImage").GetComponent<PguiReplaceAECtrl>().Replace((pvpStaticDataBySeasonID.type == PvpStaticData.Type.SPECIAL) ? "Event" : "Normal");
		PguiAECtrl AEImage = baseTr.Find("Window_PvPRankUp/AEImage").GetComponent<PguiAECtrl>();
		PguiButtonCtrl component = baseTr.Find("Window_PvPRankUp/Window/ButtonC").GetComponent<PguiButtonCtrl>();
		bool isClick = false;
		component.androidBackKeyTarget = true;
		component.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			isClick = true;
		}, PguiButtonCtrl.SoundType.DEFAULT);
		PguiTextCtrl component2 = baseTr.Find("Window_PvPRankUp/Window/Img_PvPBadge_Before/Txt_PvPRank").GetComponent<PguiTextCtrl>();
		PguiTextCtrl component3 = baseTr.Find("Window_PvPRankUp/Window/Img_PvPBadge_After/Txt_PvPRank").GetComponent<PguiTextCtrl>();
		component2.text = pvpRankInfoByPoint.rankName;
		component3.text = pvpRankInfoByPoint2.rankName;
		ReuseScroll component4 = baseTr.Find("Window_PvPRankUp/Window/ItemInfo/ScrollView").GetComponent<ReuseScroll>();
		RectTransform ScrollViewContent = component4.transform.Find("Viewport/Content").transform as RectTransform;
		component4.InitForce();
		ReuseScroll reuseScroll = component4;
		reuseScroll.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onUpdateItem, new Action<int, GameObject>(delegate(int index, GameObject go)
		{
			SelPvpCtrl.OnUpdateSVRankUp(index, go, rewardItemList);
		}));
		component4.Setup(5, 0);
		int num = rewardItemList.Count / 6 + ((rewardItemList.Count % 6 != 0) ? 1 : 0);
		component4.Resize(num, 0);
		yield return null;
		bool isStopScrollViewContent = true;
		AEImageBg.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
			AEImageBg.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
		});
		AEImage.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
			isStopScrollViewContent = false;
			AEImage.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
		});
		while (!isClick)
		{
			if (isStopScrollViewContent)
			{
				ScrollViewContent.anchoredPosition = Vector2.zero;
			}
			yield return null;
		}
		bool isFinish = false;
		AEImageBg.PlayAnimation(PguiAECtrl.AmimeType.END, null);
		AEImage.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
		{
			isFinish = true;
		});
		while (!isFinish)
		{
			yield return null;
		}
		Object.Destroy(baseTr.gameObject);
		yield break;
	}

	// Token: 0x060015A4 RID: 5540 RVA: 0x0010E95C File Offset: 0x0010CB5C
	private static void OnUpdateSVRankUp(int index, GameObject go, List<ItemData> itemList)
	{
		for (int i = 0; i < 6; i++)
		{
			int num = index * 6 + i;
			Transform transform = go.transform.Find("icon" + i.ToString());
			if (num < itemList.Count)
			{
				transform.gameObject.SetActive(true);
				transform.GetComponent<IconItemCtrl>().Setup(itemList[num].staticData, itemList[num].num);
			}
			else
			{
				transform.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x0010E9DE File Offset: 0x0010CBDE
	private IEnumerator ChangeNormal3x()
	{
		UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
		userOptionData.Pvp3x = !DataManager.DmUserInfo.optionData.Pvp3x;
		DataManager.DmUserInfo.RequestActionUpdateUserOption(userOptionData);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.DispNormal3x();
		yield break;
	}

	// Token: 0x060015A6 RID: 5542 RVA: 0x0010E9F0 File Offset: 0x0010CBF0
	private void DispNormal3x()
	{
		this.guiData.guiNormalData.Btn_ModeChange.transform.Find("BaseImage/Mode_Normal").gameObject.SetActive(!DataManager.DmUserInfo.optionData.Pvp3x);
		this.guiData.guiNormalData.Btn_ModeChange.transform.Find("BaseImage/Mode_x3").gameObject.SetActive(DataManager.DmUserInfo.optionData.Pvp3x);
	}

	// Token: 0x060015A7 RID: 5543 RVA: 0x0010EA70 File Offset: 0x0010CC70
	private IEnumerator ChangeSpecial3x()
	{
		UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
		userOptionData.SpPvp3x = !DataManager.DmUserInfo.optionData.SpPvp3x;
		DataManager.DmUserInfo.RequestActionUpdateUserOption(userOptionData);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.DispSpecial3x();
		yield break;
	}

	// Token: 0x060015A8 RID: 5544 RVA: 0x0010EA80 File Offset: 0x0010CC80
	private void DispSpecial3x()
	{
		this.guiData.guiSpecialData.Btn_ModeChange.transform.Find("BaseImage/Mode_Normal").gameObject.SetActive(!DataManager.DmUserInfo.optionData.SpPvp3x);
		this.guiData.guiSpecialData.Btn_ModeChange.transform.Find("BaseImage/Mode_x3").gameObject.SetActive(DataManager.DmUserInfo.optionData.SpPvp3x);
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x0010EB00 File Offset: 0x0010CD00
	private IEnumerator RefreshEnemyList()
	{
		int pvpSeasonId = this.currentPvpPackData.seasonId;
		int stackNum = this.currentPvpPackData.dynamicData.userInfo.pvpStaminaInfo.GetInfoByNow(TimeManager.Now).stackNum;
		if (0 >= stackNum)
		{
			bool isWindowFinish = false;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("対戦相手更新"), PrjUtil.MakeMessage("挑戦回数を1消費して\n対戦相手を更新します\n\n挑戦回数：" + stackNum.ToString() + "\n\n挑戦回数が不足しています。"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
			{
				isWindowFinish = true;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (!isWindowFinish)
			{
				yield return null;
			}
			yield break;
		}
		SelPvpCtrl.<>c__DisplayClass62_0 CS$<>8__locals2 = new SelPvpCtrl.<>c__DisplayClass62_0();
		CS$<>8__locals2.isWindowFinish = false;
		CS$<>8__locals2.owAnswer = 0;
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("対戦相手更新"), PrjUtil.MakeMessage("挑戦回数を1消費して\n対戦相手を更新します\n\nよろしいですか？\n\n挑戦回数：" + stackNum.ToString()), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			CS$<>8__locals2.owAnswer = index;
			CS$<>8__locals2.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals2.isWindowFinish)
		{
			yield return null;
		}
		if (1 != CS$<>8__locals2.owAnswer)
		{
			yield break;
		}
		CS$<>8__locals2 = null;
		DataManager.DmPvp.RequestRefreshEnemyList(pvpSeasonId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		IEnumerator ienum = this.SetupMainGui(DataManager.DmPvp.GetPvpPackDataBySeasonID(pvpSeasonId), false, false, false, false, false);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x0010EB0F File Offset: 0x0010CD0F
	private IEnumerator RecoveryPvpStamina()
	{
		int pvpSeasonId = this.currentPvpPackData.seasonId;
		SelPvpCtrl.<>c__DisplayClass63_0 CS$<>8__locals1 = new SelPvpCtrl.<>c__DisplayClass63_0();
		bool isCloseOW = false;
		StaminaInfo.NowInfo infoByNow = this.currentPvpPackData.dynamicData.userInfo.pvpStaminaInfo.GetInfoByNow(TimeManager.Now);
		int num = infoByNow.stackMaxNum - infoByNow.stackNum;
		ItemData userItemData = DataManager.DmItem.GetUserItemData(30100);
		string name = userItemData.staticData.GetName();
		string text;
		if (infoByNow.stackNum >= infoByNow.stackMaxNum)
		{
			isCloseOW = true;
			text = "挑戦回数は満タンです";
		}
		else if (num > userItemData.num)
		{
			isCloseOW = true;
			text = name + "が不足しています";
		}
		else
		{
			text = name + "を" + num.ToString() + "使用して挑戦回数を満タンにします\nよろしいですか？";
		}
		CS$<>8__locals1.isWindowFinish = false;
		CS$<>8__locals1.owAnswer = 0;
		if (isCloseOW)
		{
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("挑戦回数回復"), text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
			{
				CS$<>8__locals1.owAnswer = index;
				CS$<>8__locals1.isWindowFinish = true;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
		}
		else
		{
			CanvasManager.HdlOpenWindowUseItem.SetupByUseItem(PrjUtil.MakeMessage("挑戦回数回復"), text, delegate(int index)
			{
				CS$<>8__locals1.owAnswer = index;
				CS$<>8__locals1.isWindowFinish = true;
				return true;
			}, num, userItemData.num, false);
			CanvasManager.HdlOpenWindowUseItem.Open();
		}
		while (!CS$<>8__locals1.isWindowFinish)
		{
			yield return null;
		}
		if (1 != CS$<>8__locals1.owAnswer || isCloseOW)
		{
			yield break;
		}
		CS$<>8__locals1 = null;
		DataManager.DmPvp.RequestRecoveryPvpStamina(pvpSeasonId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060015AB RID: 5547 RVA: 0x0010EB1E File Offset: 0x0010CD1E
	private IEnumerator DecideEnemy(PvpDynamicData.EnemyInfo enemy)
	{
		DateTime now = TimeManager.Now;
		bool isSpecial = PvpStaticData.Type.SPECIAL == this.currentPvpPackData.staticData.type;
		bool is3xMode = (isSpecial ? DataManager.DmUserInfo.optionData.SpPvp3x : DataManager.DmUserInfo.optionData.Pvp3x);
		int needStamina = (is3xMode ? 3 : 1);
		if (!this.ChkPvpSeasonIsEnableByTime(now, this.currentPvpPackData.staticData))
		{
			bool isWindowFinish2 = false;
			CanvasManager.HdlOpenWindowBasic.Setup("", "シーズンが終了しました", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.OK), true, delegate(int index)
			{
				isWindowFinish2 = true;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (!isWindowFinish2)
			{
				yield return null;
			}
			this.requestNextScene = SceneManager.SceneName.SceneHome;
			this.requestNextArgs = null;
			yield break;
		}
		if (this.currentPvpPackData.dynamicData.userInfo.pvpStaminaInfo.GetInfoByNow(TimeManager.Now).stackNum < needStamina)
		{
			bool isWindowFinish = false;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("対戦相手選択"), PrjUtil.MakeMessage("挑戦回数が不足しています"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
			{
				isWindowFinish = true;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (!isWindowFinish)
			{
				yield return null;
			}
			yield break;
		}
		UserDeckData userDeckById = DataManager.DmDeck.GetUserDeckById(this.currentPvpPackData.dynamicData.userInfo.currentDeckId);
		bool isCancel = true;
		IEnumerator ienum = QuestUtil.NoticeKizunaLimitReached(delegate
		{
			isCancel = false;
		}, 0, userDeckById.charaIdList, 1, null, this.currentPvpPackData.staticData.baseData.winAcquireKizuna, this.currentPvpPackData.staticData.spEventId, false);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		if (isCancel)
		{
			yield break;
		}
		SelPvpCtrl.<>c__DisplayClass64_3 CS$<>8__locals4 = new SelPvpCtrl.<>c__DisplayClass64_3();
		CS$<>8__locals4.isWindowFinish = false;
		CS$<>8__locals4.owAnswer = 0;
		this.guiEnemyWindow.window.Setup("このパーティーと対戦します", "", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int index)
		{
			CS$<>8__locals4.isWindowFinish = true;
			CS$<>8__locals4.owAnswer = index;
			return true;
		}, null, false);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		CharaDef.AttributeMask attributeMask = (CharaDef.AttributeMask)0;
		PrjUtil.ParamPreset activeKizunaBuffByQualifiedCount = DataManager.DmChara.GetActiveKizunaBuffByQualifiedCount(enemy.kizunaBuffQualified);
		for (int i = 0; i < enemy.deckInfo.deckData.Count; i++)
		{
			CharaPackData charaPackData = enemy.deckInfo.deckData[i];
			List<PhotoPackData> list = enemy.deckInfo.equipPhotoList[i];
			this.guiEnemyWindow.iconCharaList[i].Setup(enemy.deckInfo.deckData[i], SortFilterDefine.SortType.LEVEL, false, null, this.currentPvpPackData.staticData.spEventId, -1, 0);
			if (PvpStaticData.Type.SPECIAL != this.currentPvpPackData.staticData.type)
			{
				this.guiEnemyWindow.iconCharaList[i].DispMarkEvent(false, false, false);
			}
			for (int j = 0; j < enemy.deckInfo.equipPhotoList[i].Count; j++)
			{
				PhotoPackData photoPackData = enemy.deckInfo.equipPhotoList[i][j];
				if (photoPackData != null && !photoPackData.IsInvalid())
				{
					this.guiEnemyWindow.iconPhotoList[i][j].Replace(photoPackData.staticData.baseData.type);
					this.guiEnemyWindow.iconPhotoBlankList[i][j].gameObject.SetActive(false);
					this.guiEnemyWindow.iconPhotoStepList[i][j].text = charaPackData.dynamicData.PhotoPocket[j].Step.ToString();
				}
				else
				{
					this.guiEnemyWindow.iconPhotoList[i][j].Replace(-1);
					this.guiEnemyWindow.iconPhotoBlankList[i][j].gameObject.SetActive(true);
					this.guiEnemyWindow.iconPhotoStepList[i][j].text = "";
				}
			}
			if (charaPackData != null)
			{
				PrjUtil.ParamPreset paramPreset = PrjUtil.CalcBattleParamByChara(charaPackData.dynamicData, list, this.currentBonusCharaList, activeKizunaBuffByQualifiedCount);
				num += paramPreset.atk;
				num2 += paramPreset.def;
				num3 += paramPreset.hp;
				num4 += paramPreset.totalParam;
				if (enemy.kemoBoardParamMap.ContainsKey(charaPackData.staticData.baseData.attribute))
				{
					num4 += enemy.kemoBoardParamMap[charaPackData.staticData.baseData.attribute].KemoStatus;
				}
				attributeMask |= charaPackData.staticData.baseData.attributeMask;
			}
		}
		for (int k = enemy.deckInfo.deckData.Count; k < this.guiEnemyWindow.iconCharaList.Count; k++)
		{
			this.guiEnemyWindow.iconCharaList[k].Setup(null, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			for (int l = 0; l < this.guiEnemyWindow.iconPhotoList[k].Count; l++)
			{
				this.guiEnemyWindow.iconPhotoList[k][l].Replace(-1);
				this.guiEnemyWindow.iconPhotoBlankList[k][l].gameObject.SetActive(true);
				this.guiEnemyWindow.iconPhotoStepList[k][l].text = "";
			}
		}
		this.guiEnemyWindow.Num_Atk.text = num.ToString();
		this.guiEnemyWindow.Num_Def.text = num2.ToString();
		this.guiEnemyWindow.Num_Hp.text = num3.ToString();
		if (isSpecial)
		{
			this.guiEnemyWindow.Num_TotalAttack.gameObject.SetActive(false);
			this.guiEnemyWindow.Num_EventObj.SetActive(true);
			this.guiEnemyWindow.Num_TotalAttack_Event.ReplaceTextByDefault("Param01", num4.ToString());
			int num5 = enemy.deckInfo.CalcTotalPlasmPoint();
			this.guiEnemyWindow.Num_TotalPlasm_Event.ReplaceTextByDefault("Param01", num5.ToString());
		}
		else
		{
			this.guiEnemyWindow.Num_TotalAttack.gameObject.SetActive(true);
			this.guiEnemyWindow.Num_EventObj.SetActive(false);
			this.guiEnemyWindow.Num_TotalAttack.ReplaceTextByDefault("Param01", num4.ToString());
		}
		MstPvpOppBonusData mstPvpOppBonusData = this.currentPvpPackData.staticData.oppBonusList.Find((MstPvpOppBonusData item) => item.strength == (int)enemy.difficulty);
		this.guiEnemyWindow.Num_Pvppt.ReplaceTextByDefault("Param01", ((float)mstPvpOppBonusData.pointOdds / 100f).ToString());
		this.guiEnemyWindow.Num_Player.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
		{
			enemy.userLevel.ToString(),
			enemy.userName
		});
		SelPvpCtrl.GUIEnemyDifficulty.NameType nameType = (isSpecial ? SelPvpCtrl.GUIEnemyDifficulty.NameType.Special : SelPvpCtrl.GUIEnemyDifficulty.NameType.Normal);
		this.guiEnemyWindow.Difficulty.ChangeDifficultyName(nameType);
		this.guiEnemyWindow.Difficulty.Setup(enemy);
		this.guiEnemyWindow.BaseChamp.SetActive(enemy.difficulty == PvpDynamicData.EnemyInfo.Difficulty.CHAMPION);
		this.guiEnemyWindow.NpcObj.SetActive(enemy.deckInfo.isNpc);
		for (int m = 0; m < this.guiEnemyWindow.enemyInfoList.Count; m++)
		{
			this.guiEnemyWindow.enemyInfoList[m].Setup((attributeMask & SelBattleHelperCtrl.GUI.EnemyInfo.attributeMaskList[m]) == SelBattleHelperCtrl.GUI.EnemyInfo.attributeMaskList[m]);
		}
		SelPvpCtrl.DisAdvantageType disAdvantageType = this.GetDisAdvantageType(enemy);
		this.guiEnemyWindow.UnfavorableObj.SetActive(disAdvantageType > SelPvpCtrl.DisAdvantageType.None);
		if (this.guiEnemyWindow.UnfavorableObj.activeSelf)
		{
			string text = string.Empty;
			switch (disAdvantageType)
			{
			case SelPvpCtrl.DisAdvantageType.LittleDisadvantage:
				text = "すこし不利";
				break;
			case SelPvpCtrl.DisAdvantageType.Disadvantage:
				text = "不利";
				break;
			case SelPvpCtrl.DisAdvantageType.GreatDisadvantage:
				text = "すごい不利";
				break;
			}
			this.guiEnemyWindow.Txt_Unfavorable.text = text;
		}
		this.guiEnemyWindow.window.Open();
		while (!CS$<>8__locals4.isWindowFinish)
		{
			yield return null;
		}
		if (1 != CS$<>8__locals4.owAnswer)
		{
			yield break;
		}
		CS$<>8__locals4 = null;
		DataManager.DmPvp.RequestActionPvPStart(this.currentPvpPackData.seasonId, enemy, needStamina);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		SceneBattleArgs sceneBattleArgs = new SceneBattleArgs();
		sceneBattleArgs.hash_id = DataManager.DmPvp.lastPvPStartResponse.hash_id;
		sceneBattleArgs.selectDeckId = this.currentPvpPackData.dynamicData.userInfo.currentDeckId;
		sceneBattleArgs.helper = null;
		sceneBattleArgs.oppUser = enemy.oppUser;
		sceneBattleArgs.difficulty = enemy.difficulty;
		sceneBattleArgs.pvpBoard = new List<PrjUtil.ParamPreset>();
		foreach (Chara chara in enemy.oppUser.opp_chara_list)
		{
			PrjUtil.ParamPreset paramPreset2 = new PrjUtil.ParamPreset();
			if (chara.chara_id != 0)
			{
				CharaDef.AttributeType attribute = DataManager.DmChara.GetCharaStaticData(chara.chara_id).baseData.attribute;
				DataManagerKemoBoard.KemoBoardBonusParam kemoBoardBonusParam;
				if (!enemy.kemoBoardParamMap.TryGetValue(attribute, out kemoBoardBonusParam))
				{
					kemoBoardBonusParam = new DataManagerKemoBoard.KemoBoardBonusParam(attribute);
				}
				paramPreset2.hp = kemoBoardBonusParam.Hp;
				paramPreset2.atk = kemoBoardBonusParam.Attack;
				paramPreset2.def = kemoBoardBonusParam.Difence;
				paramPreset2.avoid = kemoBoardBonusParam.Avoid;
				paramPreset2.beatDamageRatio = kemoBoardBonusParam.BeatDamage;
				paramPreset2.actionDamageRatio = kemoBoardBonusParam.ActionDamage;
				paramPreset2.tryDamageRatio = kemoBoardBonusParam.TryDamage;
			}
			sceneBattleArgs.pvpBoard.Add(paramPreset2);
		}
		sceneBattleArgs.kizunaBuffQualified = enemy.kizunaBuffQualified;
		sceneBattleArgs.resultNextSceneName = SceneManager.SceneName.ScenePvp;
		sceneBattleArgs.resultNextSceneArgs = new ScenePvp.Args
		{
			fastPvpSeasonId = this.currentPvpPackData.seasonId,
			isReturnFromBattle = true
		};
		sceneBattleArgs.isQuestNoClear = false;
		sceneBattleArgs.pvpSeasonId = this.currentPvpPackData.seasonId;
		sceneBattleArgs.pvp3x = is3xMode;
		sceneBattleArgs.pvpTraining = (isSpecial ? this.currentPvpPackData.staticData.spBattleTurnNum : 0);
		sceneBattleArgs.startTime = TimeManager.Now;
		sceneBattleArgs.eventId = (isSpecial ? this.currentPvpPackData.staticData.spEventId : 0);
		sceneBattleArgs.trainingTurn = 0;
		SceneBattle.SetRestart(sceneBattleArgs);
		this.requestNextScene = SceneManager.SceneName.SceneBattle;
		this.requestNextArgs = sceneBattleArgs;
		yield break;
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x0010EB34 File Offset: 0x0010CD34
	private SelPvpCtrl.DisAdvantageType GetDisAdvantageType(PvpDynamicData.EnemyInfo enemy)
	{
		UserDeckData userDeckById = DataManager.DmDeck.GetUserDeckById(this.currentPvpPackData.dynamicData.userInfo.currentDeckId);
		Dictionary<CharaDef.AttributeType, double> dictionary = new Dictionary<CharaDef.AttributeType, double>
		{
			{
				CharaDef.AttributeType.RED,
				0.0
			},
			{
				CharaDef.AttributeType.GREEN,
				0.0
			},
			{
				CharaDef.AttributeType.BLUE,
				0.0
			},
			{
				CharaDef.AttributeType.PINK,
				0.0
			},
			{
				CharaDef.AttributeType.LIME,
				0.0
			},
			{
				CharaDef.AttributeType.AQUA,
				0.0
			}
		};
		double num = 0.0;
		foreach (int num2 in userDeckById.charaIdList)
		{
			if (num2 != 0)
			{
				CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(num2);
				if (charaStaticData != null && dictionary.ContainsKey(charaStaticData.baseData.attribute))
				{
					Dictionary<CharaDef.AttributeType, double> dictionary2 = dictionary;
					CharaDef.AttributeType attributeType = charaStaticData.baseData.attribute;
					dictionary2[attributeType] += 1.0;
					num += 1.0;
				}
			}
		}
		Dictionary<CharaDef.AttributeType, double> dictionary3 = new Dictionary<CharaDef.AttributeType, double>
		{
			{
				CharaDef.AttributeType.RED,
				0.0
			},
			{
				CharaDef.AttributeType.GREEN,
				0.0
			},
			{
				CharaDef.AttributeType.BLUE,
				0.0
			},
			{
				CharaDef.AttributeType.PINK,
				0.0
			},
			{
				CharaDef.AttributeType.LIME,
				0.0
			},
			{
				CharaDef.AttributeType.AQUA,
				0.0
			}
		};
		double num3 = 0.0;
		foreach (CharaPackData charaPackData in enemy.deckInfo.deckData)
		{
			if (charaPackData != null && dictionary3.ContainsKey(charaPackData.staticData.baseData.attribute))
			{
				Dictionary<CharaDef.AttributeType, double> dictionary2 = dictionary3;
				CharaDef.AttributeType attributeType = charaPackData.staticData.baseData.attribute;
				dictionary2[attributeType] += 1.0;
				num3 += 1.0;
			}
		}
		double num4 = dictionary[CharaDef.AttributeType.RED] / num * (dictionary3[CharaDef.AttributeType.GREEN] / num3);
		double num5 = dictionary[CharaDef.AttributeType.RED] / num * (dictionary3[CharaDef.AttributeType.BLUE] / num3);
		double num6 = dictionary[CharaDef.AttributeType.GREEN] / num * (dictionary3[CharaDef.AttributeType.BLUE] / num3);
		double num7 = dictionary[CharaDef.AttributeType.GREEN] / num * (dictionary3[CharaDef.AttributeType.RED] / num3);
		double num8 = dictionary[CharaDef.AttributeType.BLUE] / num * (dictionary3[CharaDef.AttributeType.RED] / num3);
		double num9 = dictionary[CharaDef.AttributeType.BLUE] / num * (dictionary3[CharaDef.AttributeType.GREEN] / num3);
		double num10 = dictionary[CharaDef.AttributeType.PINK] / num * (dictionary3[CharaDef.AttributeType.LIME] / num3);
		double num11 = dictionary[CharaDef.AttributeType.PINK] / num * (dictionary3[CharaDef.AttributeType.AQUA] / num3);
		double num12 = dictionary[CharaDef.AttributeType.LIME] / num * (dictionary3[CharaDef.AttributeType.AQUA] / num3);
		double num13 = dictionary[CharaDef.AttributeType.LIME] / num * (dictionary3[CharaDef.AttributeType.PINK] / num3);
		double num14 = dictionary[CharaDef.AttributeType.AQUA] / num * (dictionary3[CharaDef.AttributeType.PINK] / num3);
		double num15 = dictionary[CharaDef.AttributeType.AQUA] / num * (dictionary3[CharaDef.AttributeType.LIME] / num3);
		double num16 = num4 - num5 + (num6 - num7) + (num8 - num9) + (num10 - num11) + (num12 - num13) + (num14 - num15);
		if (0.0 < num16)
		{
			num16 *= 100.0;
			num16 = Math.Ceiling(num16);
			num16 /= 100.0;
		}
		else if (num16 < 0.0)
		{
			num16 *= 100.0;
			num16 = Math.Floor(num16);
			num16 /= 100.0;
		}
		SelPvpCtrl.DisAdvantageType disAdvantageType;
		if (-0.06 <= num16)
		{
			disAdvantageType = SelPvpCtrl.DisAdvantageType.None;
		}
		else if (-0.2 <= num16)
		{
			disAdvantageType = SelPvpCtrl.DisAdvantageType.LittleDisadvantage;
		}
		else if (-0.6 <= num16)
		{
			disAdvantageType = SelPvpCtrl.DisAdvantageType.Disadvantage;
		}
		else
		{
			disAdvantageType = SelPvpCtrl.DisAdvantageType.GreatDisadvantage;
		}
		return disAdvantageType;
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x0010EF44 File Offset: 0x0010D144
	private void OnClickGUITopNormalButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.currentEnumerator = this.SetupMainGui(DataManager.DmPvp.GetPvpPackDataBySeasonID(this.currentNormalSeasonId), false, true, false, false, false);
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x0010EF70 File Offset: 0x0010D170
	private void OnClickGUITopSpecialButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.currentEnumerator = this.SetupMainGui(DataManager.DmPvp.GetPvpPackDataBySeasonID(this.currentSpecialSeasonId), false, true, false, false, false);
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x0010EF9C File Offset: 0x0010D19C
	private void OnClickGUICommonCharaDeckButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.requestNextScene = SceneManager.SceneName.ScenePvpDeck;
		this.requestNextArgs = new ScenePvpDeck.Args
		{
			pvpSeasonId = this.currentPvpPackData.seasonId
		};
	}

	// Token: 0x060015B0 RID: 5552 RVA: 0x0010EFCB File Offset: 0x0010D1CB
	private void OnClickGUICommonPlusButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.currentEnumerator = this.RecoveryPvpStamina();
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x0010EFE4 File Offset: 0x0010D1E4
	private void OnClickGUINormalShopButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.requestNextScene = SceneManager.SceneName.SceneShop;
		ShopData shopData = DataManager.DmShop.GetShopDataList(true, true, ShopData.TabCategory.ALL).Find((ShopData item) => item.category == ShopData.Category.PVP);
		SceneShopArgs sceneShopArgs = new SceneShopArgs
		{
			resultNextSceneName = SceneManager.SceneName.ScenePvp,
			resultNextSceneArgs = new ScenePvp.Args
			{
				fastPvpSeasonId = this.currentPvpPackData.seasonId
			},
			shopId = ((shopData != null) ? shopData.shopId : 0)
		};
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneShop, sceneShopArgs);
	}

	// Token: 0x060015B2 RID: 5554 RVA: 0x0010F07D File Offset: 0x0010D27D
	private void OnClickGUINormalModeChangeButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.currentEnumerator = this.ChangeNormal3x();
	}

	// Token: 0x060015B3 RID: 5555 RVA: 0x0010F094 File Offset: 0x0010D294
	private void OnClickGUISpecialModeChangeButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.currentEnumerator = this.ChangeSpecial3x();
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x0010F0AC File Offset: 0x0010D2AC
	private void OnClickGUISpecialMissionButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		int spEventId = this.currentPvpPackData.staticData.spEventId;
		SceneMission.MissionOpenParam missionOpenParam = new SceneMission.MissionOpenParam(MissionType.EVENTTOTAL, spEventId)
		{
			returnSceneName = SceneManager.SceneName.ScenePvp,
			resultNextSceneArgs = new ScenePvp.Args
			{
				fastPvpSeasonId = this.currentPvpPackData.seasonId
			}
		};
		this.requestNextScene = SceneManager.SceneName.SceneMission;
		this.requestNextArgs = missionOpenParam;
	}

	// Token: 0x060015B5 RID: 5557 RVA: 0x0010F110 File Offset: 0x0010D310
	private void OnClickGUISpecialGachaButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.requestNextScene = SceneManager.SceneName.SceneGacha;
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.currentPvpPackData.staticData.spEventId);
		int num = 0;
		if (eventData != null)
		{
			num = eventData.eventGachaId;
		}
		SceneGacha.OpenParam openParam = new SceneGacha.OpenParam();
		openParam.resultNextSceneName = SceneManager.SceneName.ScenePvp;
		openParam.resultNextSceneArgs = new ScenePvp.Args
		{
			fastPvpSeasonId = this.currentPvpPackData.seasonId
		};
		openParam.gachaId = num;
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneGacha, openParam);
	}

	// Token: 0x060015B6 RID: 5558 RVA: 0x0010F194 File Offset: 0x0010D394
	private void OnClickGUISpecialShopButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.requestNextScene = SceneManager.SceneName.SceneShop;
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.currentPvpPackData.staticData.spEventId);
		ShopData shopData = null;
		if (eventData != null)
		{
			shopData = DataManager.DmShop.GetShopData(eventData.eventShopIdList[0]);
		}
		SceneShopArgs sceneShopArgs = new SceneShopArgs();
		sceneShopArgs.resultNextSceneName = SceneManager.SceneName.ScenePvp;
		sceneShopArgs.resultNextSceneArgs = new ScenePvp.Args
		{
			fastPvpSeasonId = this.currentPvpPackData.seasonId
		};
		sceneShopArgs.shopId = ((shopData != null) ? shopData.shopId : 0);
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneShop, sceneShopArgs);
	}

	// Token: 0x060015B7 RID: 5559 RVA: 0x0010F234 File Offset: 0x0010D434
	private void OnClickGUISpecialTipsButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/PvP_special_Tips/tutorial_pvpspecial_tips_01", "Texture2D/Tutorial_Window/PvP_special_Tips/tutorial_pvpspecial_tips_02", "Texture2D/Tutorial_Window/PvP_special_Tips/tutorial_pvpspecial_tips_03", "Texture2D/Tutorial_Window/PvP_special_Tips/tutorial_pvpspecial_tips_04" }, null);
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x0010F28C File Offset: 0x0010D48C
	private void OnClickGUISpecialSenarioButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		SceneStoryView.Args args = new SceneStoryView.Args
		{
			viewType = SceneStoryView.Args.VIEWTYPE.PVPEVENT,
			resultNextSceneName = SceneManager.SceneName.ScenePvp,
			resultNextSceneArgs = new ScenePvp.Args
			{
				fastPvpSeasonId = this.currentPvpPackData.seasonId
			}
		};
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneStoryView, args);
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x0010F2E0 File Offset: 0x0010D4E0
	private void OnClickGUICommonMoreInfoButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.guiRewardWindow.window.Setup("クラス報酬", "", null, true, null, null, false);
		this.guiRewardWindow.window.Open();
	}

	// Token: 0x060015BA RID: 5562 RVA: 0x0010F31A File Offset: 0x0010D51A
	private void OnClickUpdateButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.currentEnumerator = this.RefreshEnemyList();
	}

	// Token: 0x060015BB RID: 5563 RVA: 0x0010F334 File Offset: 0x0010D534
	private void OnClickEnemyButtonNormal(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		int id = button.gameObject.GetComponent<PguiDataHolder>().id;
		PvpDynamicData.EnemyInfo enemyInfo = this.guiData.guiNormalEnemyScrollData.enemyInfoList[id];
		this.currentEnumerator = this.DecideEnemy(enemyInfo);
	}

	// Token: 0x060015BC RID: 5564 RVA: 0x0010F380 File Offset: 0x0010D580
	private void OnClickEnemyButtonSpecial_Hard(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		int id = button.gameObject.GetComponent<PguiDataHolder>().id;
		PvpDynamicData.EnemyInfo enemyInfo = this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Hard[id];
		this.currentEnumerator = this.DecideEnemy(enemyInfo);
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x0010F3CC File Offset: 0x0010D5CC
	private void OnClickEnemyButtonSpecial_Normal(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		int id = button.gameObject.GetComponent<PguiDataHolder>().id;
		PvpDynamicData.EnemyInfo enemyInfo = this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Normal[id];
		this.currentEnumerator = this.DecideEnemy(enemyInfo);
	}

	// Token: 0x060015BE RID: 5566 RVA: 0x0010F418 File Offset: 0x0010D618
	private void OnClickEnemyButtonSpecial_Easy(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		int id = button.gameObject.GetComponent<PguiDataHolder>().id;
		PvpDynamicData.EnemyInfo enemyInfo = this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Easy[id];
		this.currentEnumerator = this.DecideEnemy(enemyInfo);
	}

	// Token: 0x060015BF RID: 5567 RVA: 0x0010F463 File Offset: 0x0010D663
	private bool OnSelectRewardWindowTab(int index)
	{
		this.guiRewardWindow.ScrollView_RankUp.gameObject.SetActive(index == 0);
		this.guiRewardWindow.ScrollView_Bonus.gameObject.SetActive(1 == index);
		return true;
	}

	// Token: 0x060015C0 RID: 5568 RVA: 0x0010F498 File Offset: 0x0010D698
	private void OnStartItemNormalScroll(int index, GameObject go)
	{
		SelPvpCtrl.GUIEnemy guienemy = new SelPvpCtrl.GUIEnemy(go.transform);
		this.guiData.guiNormalEnemyScrollData.enemyList.Add(guienemy);
		guienemy.Difficulty.ChangeDifficultyName(SelPvpCtrl.GUIEnemyDifficulty.NameType.Normal);
		guienemy.Pvp_ListBar_Friend.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickEnemyButtonNormal), PguiButtonCtrl.SoundType.DEFAULT);
		guienemy.Pvp_ListBar_Friend.gameObject.AddComponent<PguiDataHolder>();
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x0010F4FC File Offset: 0x0010D6FC
	private void OnUpdateItemNormalScroll(int index, GameObject go)
	{
		if (index < this.guiData.guiNormalEnemyScrollData.enemyInfoList.Count)
		{
			go.SetActive(true);
			SelPvpCtrl.GUIEnemy guienemy = this.guiData.guiNormalEnemyScrollData.enemyList.Find((SelPvpCtrl.GUIEnemy item) => item.baseObj == go);
			PvpDynamicData.EnemyInfo enemyInfo = this.guiData.guiNormalEnemyScrollData.enemyInfoList[index];
			guienemy.Pvp_ListBar_Friend.GetComponent<PguiDataHolder>().id = index;
			this.UpdateGUIEnemy(guienemy, enemyInfo);
			return;
		}
		go.SetActive(false);
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x0010F59C File Offset: 0x0010D79C
	private void UpdateGUIEnemy(SelPvpCtrl.GUIEnemy guiEnemy, PvpDynamicData.EnemyInfo enemy)
	{
		int num = 0;
		PrjUtil.ParamPreset activeKizunaBuffByQualifiedCount = DataManager.DmChara.GetActiveKizunaBuffByQualifiedCount(enemy.kizunaBuffQualified);
		for (int i = 0; i < enemy.deckInfo.deckData.Count; i++)
		{
			CharaPackData charaPackData = enemy.deckInfo.deckData[i];
			List<PhotoPackData> list = enemy.deckInfo.equipPhotoList[i];
			guiEnemy.iconCharaList[i].Setup(enemy.deckInfo.deckData[i], SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			if (PvpStaticData.Type.SPECIAL != this.currentPvpPackData.staticData.type)
			{
				guiEnemy.iconCharaList[i].DispMarkEvent(false, false, false);
			}
			if (charaPackData != null)
			{
				PrjUtil.ParamPreset paramPreset = PrjUtil.CalcBattleParamByChara(charaPackData.dynamicData, list, this.currentBonusCharaList, activeKizunaBuffByQualifiedCount);
				num += paramPreset.totalParam;
				if (enemy.kemoBoardParamMap.ContainsKey(charaPackData.staticData.baseData.attribute))
				{
					num += enemy.kemoBoardParamMap[charaPackData.staticData.baseData.attribute].KemoStatus;
				}
			}
		}
		for (int j = enemy.deckInfo.deckData.Count; j < guiEnemy.iconCharaList.Count; j++)
		{
			guiEnemy.iconCharaList[j].Setup(null, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
		}
		guiEnemy.Mark_TotalAttack.text = num.ToString();
		int num2 = enemy.deckInfo.CalcTotalPlasmPoint();
		guiEnemy.Mark_TotalPlasm.text = num2.ToString();
		guiEnemy.Mark_TotalPlasm.gameObject.SetActive(PvpStaticData.Type.SPECIAL == this.currentPvpPackData.staticData.type);
		guiEnemy.NpcObj.SetActive(enemy.deckInfo.isNpc);
		MstPvpOppBonusData mstPvpOppBonusData = this.currentPvpPackData.staticData.oppBonusList.Find((MstPvpOppBonusData item) => item.strength == (int)enemy.difficulty);
		float num3 = ((mstPvpOppBonusData == null) ? 100f : ((float)mstPvpOppBonusData.pointOdds));
		guiEnemy.Num_Pvppt.ReplaceTextByDefault("Param01", (num3 / 100f).ToString());
		guiEnemy.Txt_FriendName.text = enemy.userName;
		guiEnemy.Num_Rank.ReplaceTextByDefault("Param01", enemy.userLevel.ToString());
		guiEnemy.Difficulty.Setup(enemy);
		guiEnemy.BaseChamp.SetActive(enemy.difficulty == PvpDynamicData.EnemyInfo.Difficulty.CHAMPION);
	}

	// Token: 0x060015C3 RID: 5571 RVA: 0x0010F868 File Offset: 0x0010DA68
	private void OnStartItemSpecialScroll_Hard(int index, GameObject go)
	{
		SelPvpCtrl.GUIEnemy guienemy = new SelPvpCtrl.GUIEnemy(go.transform);
		this.guiData.guiSpecialEnemyScrollData.guiEnemyList_Hard.Add(guienemy);
		guienemy.Difficulty.ChangeDifficultyName(SelPvpCtrl.GUIEnemyDifficulty.NameType.Special);
		guienemy.Pvp_ListBar_Friend.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickEnemyButtonSpecial_Hard), PguiButtonCtrl.SoundType.DEFAULT);
		guienemy.Pvp_ListBar_Friend.gameObject.AddComponent<PguiDataHolder>();
	}

	// Token: 0x060015C4 RID: 5572 RVA: 0x0010F8CC File Offset: 0x0010DACC
	private void OnUpdateItemSpecialScroll_Hard(int index, GameObject go)
	{
		if (index < this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Hard.Count)
		{
			go.SetActive(true);
			SelPvpCtrl.GUIEnemy guienemy = this.guiData.guiSpecialEnemyScrollData.guiEnemyList_Hard.Find((SelPvpCtrl.GUIEnemy item) => item.baseObj == go);
			PvpDynamicData.EnemyInfo enemyInfo = this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Hard[index];
			guienemy.Pvp_ListBar_Friend.GetComponent<PguiDataHolder>().id = index;
			this.UpdateGUIEnemy(guienemy, enemyInfo);
			return;
		}
		go.SetActive(false);
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x0010F96C File Offset: 0x0010DB6C
	private void OnStartItemSpecialScroll_Normal(int index, GameObject go)
	{
		SelPvpCtrl.GUIEnemy guienemy = new SelPvpCtrl.GUIEnemy(go.transform);
		this.guiData.guiSpecialEnemyScrollData.guiEnemyList_Normal.Add(guienemy);
		guienemy.Difficulty.ChangeDifficultyName(SelPvpCtrl.GUIEnemyDifficulty.NameType.Special);
		guienemy.Pvp_ListBar_Friend.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickEnemyButtonSpecial_Normal), PguiButtonCtrl.SoundType.DEFAULT);
		guienemy.Pvp_ListBar_Friend.gameObject.AddComponent<PguiDataHolder>();
	}

	// Token: 0x060015C6 RID: 5574 RVA: 0x0010F9D0 File Offset: 0x0010DBD0
	private void OnUpdateItemSpecialScroll_Normal(int index, GameObject go)
	{
		if (index < this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Normal.Count)
		{
			go.SetActive(true);
			SelPvpCtrl.GUIEnemy guienemy = this.guiData.guiSpecialEnemyScrollData.guiEnemyList_Normal.Find((SelPvpCtrl.GUIEnemy item) => item.baseObj == go);
			PvpDynamicData.EnemyInfo enemyInfo = this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Normal[index];
			guienemy.Pvp_ListBar_Friend.GetComponent<PguiDataHolder>().id = index;
			this.UpdateGUIEnemy(guienemy, enemyInfo);
			return;
		}
		go.SetActive(false);
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x0010FA70 File Offset: 0x0010DC70
	private void OnStartItemSpecialScroll_Easy(int index, GameObject go)
	{
		SelPvpCtrl.GUIEnemy guienemy = new SelPvpCtrl.GUIEnemy(go.transform);
		this.guiData.guiSpecialEnemyScrollData.guiEnemyList_Easy.Add(guienemy);
		guienemy.Difficulty.ChangeDifficultyName(SelPvpCtrl.GUIEnemyDifficulty.NameType.Special);
		guienemy.Pvp_ListBar_Friend.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickEnemyButtonSpecial_Easy), PguiButtonCtrl.SoundType.DEFAULT);
		guienemy.Pvp_ListBar_Friend.gameObject.AddComponent<PguiDataHolder>();
	}

	// Token: 0x060015C8 RID: 5576 RVA: 0x0010FAD4 File Offset: 0x0010DCD4
	private void OnUpdateItemSpecialScroll_Easy(int index, GameObject go)
	{
		if (index < this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Easy.Count)
		{
			go.SetActive(true);
			SelPvpCtrl.GUIEnemy guienemy = this.guiData.guiSpecialEnemyScrollData.guiEnemyList_Easy.Find((SelPvpCtrl.GUIEnemy item) => item.baseObj == go);
			PvpDynamicData.EnemyInfo enemyInfo = this.guiData.guiSpecialEnemyScrollData.enemyInfoList_Easy[index];
			guienemy.Pvp_ListBar_Friend.GetComponent<PguiDataHolder>().id = index;
			this.UpdateGUIEnemy(guienemy, enemyInfo);
			return;
		}
		go.SetActive(false);
	}

	// Token: 0x060015C9 RID: 5577 RVA: 0x0010FB74 File Offset: 0x0010DD74
	private bool OnSelectGUISpecialTab(int index)
	{
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Hard.gameObject.SetActive(index == 0);
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Normal.gameObject.SetActive(1 == index);
		this.guiData.guiSpecialEnemyScrollData.ScrollView_Easy.gameObject.SetActive(2 == index);
		this.cloneUserOptionData.CurrentSpPvpDifficultyTab = index;
		return true;
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x0010FBE8 File Offset: 0x0010DDE8
	private void OnStartItemOwDR(int index, GameObject go)
	{
		this.guiDefenseResultWindow.plateList.Add(new SelPvpCtrl.GUIDefenseResultWindow.Plate(go.transform));
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x0010FC08 File Offset: 0x0010DE08
	private void OnUpdateItemOwDR(int index, GameObject go)
	{
		if (this.selectorEffectTemporary != null && index < this.selectorEffectTemporary.defenseResultList.Count)
		{
			go.SetActive(true);
			SelPvpCtrl.GUIDefenseResultWindow.Plate plate = this.guiDefenseResultWindow.plateList.Find((SelPvpCtrl.GUIDefenseResultWindow.Plate item) => item.baseObj == go);
			PvpDynamicData.DefenseResult defenseResult = this.selectorEffectTemporary.defenseResultList[index];
			plate.Num_Txt.ReplaceTextByDefault("Param01", defenseResult.time.ToString("M月d日(ddd)", new CultureInfo("ja-JP", false)));
			plate.Num_Results.text = defenseResult.winNum.ToString() + PrjUtil.MakeMessage("勝");
			for (int i = 0; i < plate.iconItemList.Count; i++)
			{
				plate.iconItemList[i].Setup((i < defenseResult.bonusItemList.Count) ? defenseResult.bonusItemList[i] : null);
			}
			return;
		}
		go.SetActive(false);
	}

	// Token: 0x060015CC RID: 5580 RVA: 0x0010FD1F File Offset: 0x0010DF1F
	private void OnStartItemOwRW01(int index, GameObject go)
	{
		this.guiRewardWindow.plateListRank.Add(new SelPvpCtrl.GUIRewardWindow.Plate(go.transform));
	}

	// Token: 0x060015CD RID: 5581 RVA: 0x0010FD3C File Offset: 0x0010DF3C
	private void OnUpdateItemOwRW01(int index, GameObject go)
	{
		if (index < this.currentPvpPackData.staticData.rankMstList.Count)
		{
			go.SetActive(true);
			SelPvpCtrl.GUIRewardWindow.Plate plate = this.guiRewardWindow.plateListRank.Find((SelPvpCtrl.GUIRewardWindow.Plate item) => item.baseObj == go);
			MstPvpRankData mstPvpRankData = this.currentPvpPackData.staticData.rankMstList[index];
			plate.Num_Txt.text = mstPvpRankData.pvpRankName;
			List<ItemData> list = new List<ItemData>
			{
				(mstPvpRankData.itemId00 != 0) ? new ItemData(mstPvpRankData.itemId00, mstPvpRankData.itemNum00) : null,
				(mstPvpRankData.itemId01 != 0) ? new ItemData(mstPvpRankData.itemId01, mstPvpRankData.itemNum01) : null,
				(mstPvpRankData.itemId02 != 0) ? new ItemData(mstPvpRankData.itemId02, mstPvpRankData.itemNum02) : null
			};
			for (int i = 0; i < plate.iconItemList.Count; i++)
			{
				plate.iconItemList[i].Setup(list[i]);
			}
			bool flag = PvpStaticData.Type.SPECIAL == this.currentPvpPackData.staticData.type;
			plate.pointNameObjNormal.SetActive(!flag);
			plate.pointNameObjSpecial.SetActive(flag);
			plate.point.ReplaceTextByDefault("Param01", mstPvpRankData.pvpPointMin.ToString());
			return;
		}
		go.SetActive(false);
	}

	// Token: 0x060015CE RID: 5582 RVA: 0x0010FEBC File Offset: 0x0010E0BC
	private void OnStartItemOwRW02(int index, GameObject go)
	{
		this.guiRewardWindow.plateListDefense.Add(new SelPvpCtrl.GUIRewardWindow.Plate(go.transform));
	}

	// Token: 0x060015CF RID: 5583 RVA: 0x0010FEDC File Offset: 0x0010E0DC
	private void OnUpdateItemOwRW02(int index, GameObject go)
	{
		if (index < this.currentPvpPackData.staticData.pvpDefenseList.Count)
		{
			go.SetActive(true);
			SelPvpCtrl.GUIRewardWindow.Plate plate = this.guiRewardWindow.plateListDefense.Find((SelPvpCtrl.GUIRewardWindow.Plate item) => item.baseObj == go);
			MstPvpDefenseData mstPvpDefenseData = this.currentPvpPackData.staticData.pvpDefenseList[index];
			index++;
			MstPvpDefenseData mstPvpDefenseData2 = ((index < this.currentPvpPackData.staticData.pvpDefenseList.Count) ? this.currentPvpPackData.staticData.pvpDefenseList[index] : null);
			string text = mstPvpDefenseData.winNum.ToString();
			if (mstPvpDefenseData2 != null && mstPvpDefenseData2.winNum - 1 != mstPvpDefenseData.winNum)
			{
				text = text + "～" + (mstPvpDefenseData2.winNum - 1).ToString();
			}
			text += PrjUtil.MakeMessage("勝");
			if (mstPvpDefenseData2 == null)
			{
				text += PrjUtil.MakeMessage("以上");
			}
			plate.Num_Txt.text = text;
			List<ItemData> list = new List<ItemData>
			{
				(mstPvpDefenseData.itemId00 != 0) ? new ItemData(mstPvpDefenseData.itemId00, mstPvpDefenseData.itemNum00) : null,
				(mstPvpDefenseData.itemId01 != 0) ? new ItemData(mstPvpDefenseData.itemId01, mstPvpDefenseData.itemNum01) : null,
				(mstPvpDefenseData.itemId02 != 0) ? new ItemData(mstPvpDefenseData.itemId02, mstPvpDefenseData.itemNum02) : null
			};
			for (int i = 0; i < plate.iconItemList.Count; i++)
			{
				plate.iconItemList[i].Setup(list[i]);
			}
			return;
		}
		go.SetActive(false);
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x001100AC File Offset: 0x0010E2AC
	private void OnClickBackButton()
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		if (this.guiData.baseObj.activeSelf)
		{
			this.currentEnumerator = this.SetupTopGui();
			return;
		}
		this.requestNextScene = SceneManager.SceneName.SceneHome;
		this.requestNextArgs = null;
	}

	// Token: 0x060015D1 RID: 5585 RVA: 0x001100E8 File Offset: 0x0010E2E8
	private void OnTouchCharaModel()
	{
		if (this.guiData.guiSpecialData.renderTexture.IsCurrentAnimation(CharaMotionDefine.ActKey.SPECIAL) && this.playingVoice.GetStatus() != CriAtomExPlayback.Status.Playing)
		{
			this.guiData.guiSpecialData.renderTexture.SetAnimation(CharaMotionDefine.ActKey.BIRD_OUT, false, delegate
			{
				this.guiData.guiSpecialData.renderTexture.SetAnimation(CharaMotionDefine.ActKey.SPECIAL, true);
			});
			this.playingVoice = SoundManager.PlayVoice(ScenePvp.voiceSheet, ScenePvp.voiceNameList[4]);
		}
	}

	// Token: 0x040011C9 RID: 4553
	private SelPvpCtrl.GUI guiData;

	// Token: 0x040011CA RID: 4554
	private SelPvpCtrl.GUITop guiTopData;

	// Token: 0x040011CB RID: 4555
	private SelPvpCtrl.GUIEnemyWindow guiEnemyWindow;

	// Token: 0x040011CC RID: 4556
	private SelPvpCtrl.GUIRewardWindow guiRewardWindow;

	// Token: 0x040011CD RID: 4557
	private SelPvpCtrl.GUIDefenseResultWindow guiDefenseResultWindow;

	// Token: 0x040011CE RID: 4558
	private PvpPackData currentPvpPackData;

	// Token: 0x040011CF RID: 4559
	private List<DataManagerChara.BonusCharaData> currentBonusCharaList;

	// Token: 0x040011D0 RID: 4560
	private DateTime currentCampaignEndTime;

	// Token: 0x040011D1 RID: 4561
	private IEnumerator _currentEnumerator;

	// Token: 0x040011D2 RID: 4562
	public int currentNormalSeasonId;

	// Token: 0x040011D3 RID: 4563
	public int currentSpecialSeasonId;

	// Token: 0x040011D4 RID: 4564
	private CriAtomExPlayback playingVoice;

	// Token: 0x040011D7 RID: 4567
	private SelPvpCtrl.SelectorEffect selectorEffectTemporary;

	// Token: 0x040011D8 RID: 4568
	private UserOptionData cloneUserOptionData;

	// Token: 0x02000C05 RID: 3077
	private enum DisAdvantageType
	{
		// Token: 0x04004966 RID: 18790
		None,
		// Token: 0x04004967 RID: 18791
		LittleDisadvantage,
		// Token: 0x04004968 RID: 18792
		Disadvantage,
		// Token: 0x04004969 RID: 18793
		GreatDisadvantage
	}

	// Token: 0x02000C06 RID: 3078
	public class SelectorEffect
	{
		// Token: 0x0400496A RID: 18794
		public int seasonId;

		// Token: 0x0400496B RID: 18795
		public bool isHappenReset;

		// Token: 0x0400496C RID: 18796
		public int pvpRankBeforReset;

		// Token: 0x0400496D RID: 18797
		public int pvpSeasonIdBeforReset;

		// Token: 0x0400496E RID: 18798
		public bool isHappenDefenseBonus;

		// Token: 0x0400496F RID: 18799
		public List<PvpDynamicData.DefenseResult> defenseResultList;
	}

	// Token: 0x02000C07 RID: 3079
	public class GUITop
	{
		// Token: 0x06004493 RID: 17555 RVA: 0x00208A34 File Offset: 0x00206C34
		public GUITop(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseAnime = baseTr.GetComponent<SimpleAnimation>();
			this.Btn_Nomal = baseTr.Find("Right/Null_Normal/Btn_Nomal").GetComponent<PguiButtonCtrl>();
			this.Btn_Nomal_Disable_Info = baseTr.Find("Right/Null_Normal/Btn_Nomal/BaseImage/DisableInfo").gameObject;
			this.Btn_Event = baseTr.Find("Right/Null_Event/Btn_Event").GetComponent<PguiButtonCtrl>();
			this.Texture_EventOpen = baseTr.Find("Right/Null_Event/Btn_Event/BaseImage/Texture_EventOpen").GetComponent<PguiRawImageCtrl>();
			this.Texture_Banner = baseTr.Find("Right/Null_Event/Btn_Event/BaseImage/Texture_Banner").GetComponent<PguiRawImageCtrl>();
			this.LuckyImageList = new List<PguiImageCtrl>
			{
				baseTr.Find("Group_Lucky/Lucky01").GetComponent<PguiImageCtrl>(),
				baseTr.Find("Group_Lucky/Lucky02").GetComponent<PguiImageCtrl>(),
				baseTr.Find("Group_Lucky/Lucky03").GetComponent<PguiImageCtrl>(),
				baseTr.Find("Group_Lucky/Lucky04").GetComponent<PguiImageCtrl>()
			};
			this.renderTexture = baseTr.Find("Group_Lucky/RenderChara").GetComponent<PguiRenderTextureCharaCtrl>();
			this.Txt_Term = baseTr.Find("Right/Null_Normal/Btn_Nomal/BaseImage/Term/Txt_Term").GetComponent<PguiTextCtrl>();
			this.Txt_Stage = baseTr.Find("Right/Null_Normal/Btn_Nomal/BaseImage/Txt_Stage").GetComponent<PguiTextCtrl>();
			this.Txt_Term_Event = baseTr.Find("Right/Null_Event/Btn_Event/BaseImage/Term/Txt_Term").GetComponent<PguiTextCtrl>();
			this.Txt_Stage_Event = baseTr.Find("Right/Null_Event/Btn_Event/BaseImage/Txt_Stage").GetComponent<PguiTextCtrl>();
			this.Texture_EventOpen.SetRawImage("Texture2D/PvpTopPhoto/pvptop_photo_1002", true, false, null);
			this.Texture_Banner.gameObject.SetActive(false);
			this.CampaignObj = baseTr.Find("Right/Null_Normal/Btn_Nomal/BaseImage/Popup_Campaign_Cmn").gameObject;
			this.Txt_Campaign = baseTr.Find("Right/Null_Normal/Btn_Nomal/BaseImage/Popup_Campaign_Cmn/Txt_Campaign").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x04004970 RID: 18800
		public GameObject baseObj;

		// Token: 0x04004971 RID: 18801
		public SimpleAnimation baseAnime;

		// Token: 0x04004972 RID: 18802
		public PguiButtonCtrl Btn_Nomal;

		// Token: 0x04004973 RID: 18803
		public GameObject Btn_Nomal_Disable_Info;

		// Token: 0x04004974 RID: 18804
		public PguiButtonCtrl Btn_Event;

		// Token: 0x04004975 RID: 18805
		public List<PguiImageCtrl> LuckyImageList;

		// Token: 0x04004976 RID: 18806
		public PguiRenderTextureCharaCtrl renderTexture;

		// Token: 0x04004977 RID: 18807
		public PguiRawImageCtrl Texture_EventOpen;

		// Token: 0x04004978 RID: 18808
		public PguiRawImageCtrl Texture_Banner;

		// Token: 0x04004979 RID: 18809
		public PguiTextCtrl Txt_Term;

		// Token: 0x0400497A RID: 18810
		public PguiTextCtrl Txt_Stage;

		// Token: 0x0400497B RID: 18811
		public PguiTextCtrl Txt_Term_Event;

		// Token: 0x0400497C RID: 18812
		public PguiTextCtrl Txt_Stage_Event;

		// Token: 0x0400497D RID: 18813
		public GameObject CampaignObj;

		// Token: 0x0400497E RID: 18814
		public PguiTextCtrl Txt_Campaign;
	}

	// Token: 0x02000C08 RID: 3080
	public class GUIEnemy
	{
		// Token: 0x06004494 RID: 17556 RVA: 0x00208BF0 File Offset: 0x00206DF0
		public GUIEnemy(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Pvp_ListBar_Friend = baseTr.GetComponent<PguiButtonCtrl>();
			this.Txt_FriendName = baseTr.Find("BaseImage/Txt_FriendName").GetComponent<PguiTextCtrl>();
			this.Num_Rank = baseTr.Find("BaseImage/Num_Rank").GetComponent<PguiTextCtrl>();
			this.Difficulty = new SelPvpCtrl.GUIEnemyDifficulty(baseTr.Find("BaseImage"));
			this.BaseChamp = baseTr.Find("BaseImage/Base_Champ").gameObject;
			this.Mark_TotalAttack = baseTr.Find("BaseImage/Num_TotalAttack").GetComponent<PguiTextCtrl>();
			this.Mark_TotalPlasm = baseTr.Find("BaseImage/Num_TotalPlasm").GetComponent<PguiTextCtrl>();
			this.Num_Pvppt = baseTr.Find("BaseImage/PointInfo/Num_Pvppt").GetComponent<PguiTextCtrl>();
			this.iconCharaList = new List<IconCharaCtrl>
			{
				baseTr.Find("BaseImage/Icon_Chara01/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("BaseImage/Icon_Chara02/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("BaseImage/Icon_Chara03/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("BaseImage/Icon_Chara04/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("BaseImage/Icon_Chara05/Icon_Chara").GetComponent<IconCharaCtrl>()
			};
			this.NpcObj = baseTr.Find("BaseImage/Mark_NPC").gameObject;
		}

		// Token: 0x0400497F RID: 18815
		public GameObject baseObj;

		// Token: 0x04004980 RID: 18816
		public PguiButtonCtrl Pvp_ListBar_Friend;

		// Token: 0x04004981 RID: 18817
		public PguiTextCtrl Txt_FriendName;

		// Token: 0x04004982 RID: 18818
		public PguiTextCtrl Num_Rank;

		// Token: 0x04004983 RID: 18819
		public PguiTextCtrl Mark_TotalAttack;

		// Token: 0x04004984 RID: 18820
		public PguiTextCtrl Mark_TotalPlasm;

		// Token: 0x04004985 RID: 18821
		public SelPvpCtrl.GUIEnemyDifficulty Difficulty;

		// Token: 0x04004986 RID: 18822
		public GameObject BaseChamp;

		// Token: 0x04004987 RID: 18823
		public PguiTextCtrl Num_Pvppt;

		// Token: 0x04004988 RID: 18824
		public List<IconCharaCtrl> iconCharaList = new List<IconCharaCtrl>();

		// Token: 0x04004989 RID: 18825
		public GameObject NpcObj;
	}

	// Token: 0x02000C09 RID: 3081
	public class GUIEnemyWindow
	{
		// Token: 0x06004495 RID: 17557 RVA: 0x00208D50 File Offset: 0x00206F50
		public GUIEnemyWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.NpcObj = baseTr.Find("Base/Window/Base_Deck/Mark_NPC").gameObject;
			this.UnfavorableObj = baseTr.Find("Base/Window/Unfavorable").gameObject;
			this.Txt_Unfavorable = baseTr.Find("Base/Window/Unfavorable/Base/Txt").GetComponent<PguiTextCtrl>();
			this.AtrInfo01Obj = baseTr.Find("Base/Window/AtrInfo01").gameObject;
			this.AtrInfo02Obj = baseTr.Find("Base/Window/AtrInfo02").gameObject;
			this.enemyInfoList = new List<SelBattleHelperCtrl.GUI.EnemyInfo>
			{
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Base/Window/AtrInfo01/Icon_Atr_R")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Base/Window/AtrInfo01/Icon_Atr_G")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Base/Window/AtrInfo01/Icon_Atr_B")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Base/Window/AtrInfo02/Icon_Atr_R")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Base/Window/AtrInfo02/Icon_Atr_G")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Base/Window/AtrInfo02/Icon_Atr_B"))
			};
			this.Num_Player = baseTr.Find("Base/Window/Base_Deck/Num_Player").GetComponent<PguiTextCtrl>();
			this.Num_TotalAttack = baseTr.Find("Base/Window/Base_Deck/Num_TotalAttack").GetComponent<PguiTextCtrl>();
			this.Num_EventObj = baseTr.Find("Base/Window/Base_Deck/Event_NumAll").gameObject;
			this.Num_TotalAttack_Event = baseTr.Find("Base/Window/Base_Deck/Event_NumAll/Num_TotalAttack_Event").GetComponent<PguiTextCtrl>();
			this.Num_TotalPlasm_Event = baseTr.Find("Base/Window/Base_Deck/Event_NumAll/Num_TotalPlasm_Event").GetComponent<PguiTextCtrl>();
			this.Num_Pvppt = baseTr.Find("Base/Window/Base_Deck/PointInfo/Num_Pvppt").GetComponent<PguiTextCtrl>();
			this.Difficulty = new SelPvpCtrl.GUIEnemyDifficulty(baseTr.Find("Base/Window/Base_Deck"));
			this.BaseChamp = baseTr.Find("Base/Window/Base_Deck/Base_Champ").gameObject;
			this.Num_Atk = baseTr.Find("Base/Window/Base_Deck/ParamInfo/ATK/Num").GetComponent<PguiTextCtrl>();
			this.Num_Def = baseTr.Find("Base/Window/Base_Deck/ParamInfo/DEF/Num").GetComponent<PguiTextCtrl>();
			this.Num_Hp = baseTr.Find("Base/Window/Base_Deck/ParamInfo/HP/Num").GetComponent<PguiTextCtrl>();
			for (int i = 0; i < 5; i++)
			{
				Transform transform = baseTr.Find("Base/Window/Base_Deck/Icon_Chara" + (i + 1).ToString("00") + "/CharaDeck_CharaIconSet");
				Transform transform2 = transform.Find("Cover");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(false);
				}
				Transform transform3 = transform.Find("Mark_Friend");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(false);
				}
				Transform transform4 = transform.Find("PhotoIconView");
				if (transform4 != null)
				{
					transform4.gameObject.SetActive(false);
				}
				Transform transform5 = transform.Find("Base_CharaBlank_Friend");
				if (transform5 != null)
				{
					transform5.gameObject.SetActive(false);
				}
				Transform transform6 = transform.Find("AEImage_Mark_Ban");
				if (transform6 != null)
				{
					transform6.gameObject.SetActive(false);
				}
				Transform transform7 = transform.Find("AccessoryIconView");
				if (transform7 != null)
				{
					transform7.gameObject.SetActive(false);
				}
				this.iconCharaList.Add(Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, transform.Find("Icon_Chara")).GetComponent<IconCharaCtrl>());
				this.iconPhotoList.Add(new List<PguiReplaceSpriteCtrl>());
				this.iconPhotoBlankList.Add(new List<PguiReplaceSpriteCtrl>());
				this.iconPhotoStepList.Add(new List<PguiTextCtrl>());
				for (int j = 0; j < 4; j++)
				{
					Transform transform8 = transform.Find("PhotoIconKind/Icon_PhotoKind" + (j + 1).ToString("00"));
					this.iconPhotoList[i].Add(transform8.transform.GetComponent<PguiReplaceSpriteCtrl>());
					PguiReplaceSpriteCtrl component = transform8.Find("Icon_Status").transform.GetComponent<PguiReplaceSpriteCtrl>();
					component.Replace(1);
					this.iconPhotoBlankList[i].Add(component);
					PguiTextCtrl component2 = transform8.Find("Num_Lv").transform.GetComponent<PguiTextCtrl>();
					this.iconPhotoStepList[i].Add(component2);
				}
			}
		}

		// Token: 0x0400498A RID: 18826
		public GameObject baseObj;

		// Token: 0x0400498B RID: 18827
		public PguiOpenWindowCtrl window;

		// Token: 0x0400498C RID: 18828
		public GameObject UnfavorableObj;

		// Token: 0x0400498D RID: 18829
		public PguiTextCtrl Txt_Unfavorable;

		// Token: 0x0400498E RID: 18830
		public GameObject AtrInfo01Obj;

		// Token: 0x0400498F RID: 18831
		public GameObject AtrInfo02Obj;

		// Token: 0x04004990 RID: 18832
		public List<SelBattleHelperCtrl.GUI.EnemyInfo> enemyInfoList = new List<SelBattleHelperCtrl.GUI.EnemyInfo>();

		// Token: 0x04004991 RID: 18833
		public GameObject NpcObj;

		// Token: 0x04004992 RID: 18834
		public PguiTextCtrl Num_Player;

		// Token: 0x04004993 RID: 18835
		public PguiTextCtrl Num_TotalAttack;

		// Token: 0x04004994 RID: 18836
		public GameObject Num_EventObj;

		// Token: 0x04004995 RID: 18837
		public PguiTextCtrl Num_TotalAttack_Event;

		// Token: 0x04004996 RID: 18838
		public PguiTextCtrl Num_TotalPlasm_Event;

		// Token: 0x04004997 RID: 18839
		public PguiTextCtrl Num_Pvppt;

		// Token: 0x04004998 RID: 18840
		public SelPvpCtrl.GUIEnemyDifficulty Difficulty;

		// Token: 0x04004999 RID: 18841
		public GameObject BaseChamp;

		// Token: 0x0400499A RID: 18842
		public PguiTextCtrl Num_Atk;

		// Token: 0x0400499B RID: 18843
		public PguiTextCtrl Num_Def;

		// Token: 0x0400499C RID: 18844
		public PguiTextCtrl Num_Hp;

		// Token: 0x0400499D RID: 18845
		public List<IconCharaCtrl> iconCharaList = new List<IconCharaCtrl>();

		// Token: 0x0400499E RID: 18846
		public List<List<PguiReplaceSpriteCtrl>> iconPhotoList = new List<List<PguiReplaceSpriteCtrl>>();

		// Token: 0x0400499F RID: 18847
		public List<List<PguiReplaceSpriteCtrl>> iconPhotoBlankList = new List<List<PguiReplaceSpriteCtrl>>();

		// Token: 0x040049A0 RID: 18848
		public List<List<PguiTextCtrl>> iconPhotoStepList = new List<List<PguiTextCtrl>>();
	}

	// Token: 0x02000C0A RID: 3082
	public class GUIEnemyDifficulty
	{
		// Token: 0x06004496 RID: 17558 RVA: 0x0020918C File Offset: 0x0020738C
		public GUIEnemyDifficulty(Transform baseTr)
		{
			this.HardGo = baseTr.Find("Pvp_Level_Tegowai").gameObject;
			this.Txt_Hard = baseTr.Find("Pvp_Level_Tegowai/Num_Txt").GetComponent<PguiTextCtrl>();
			this.NormalGo = baseTr.Find("Pvp_Level_Dokkoi").gameObject;
			this.Txt_Normal = baseTr.Find("Pvp_Level_Dokkoi/Num_Txt").GetComponent<PguiTextCtrl>();
			this.EasyGo = baseTr.Find("Pvp_Level_Yasashi").gameObject;
			this.Txt_Easy = baseTr.Find("Pvp_Level_Yasashi/Num_Txt").GetComponent<PguiTextCtrl>();
			this.ChampGo = baseTr.Find("Pvp_Level_Champion").gameObject;
			this.RankerGo = baseTr.Find("Pvp_Level_2nd").gameObject;
			this.Num_TopRank = baseTr.Find("Pvp_Level_2nd/Num_Txt").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x00209268 File Offset: 0x00207468
		public void ChangeDifficultyName(SelPvpCtrl.GUIEnemyDifficulty.NameType type)
		{
			if (type != SelPvpCtrl.GUIEnemyDifficulty.NameType.Normal && type == SelPvpCtrl.GUIEnemyDifficulty.NameType.Special)
			{
				this.Txt_Hard.text = SelPvpCtrl.GUIEnemyDifficulty.DIFFICULTY_NAME_STR_SPECIAL_HARD;
				this.Txt_Normal.text = SelPvpCtrl.GUIEnemyDifficulty.DIFFICULTY_NAME_STR_SPECIAL_NORMAL;
				this.Txt_Easy.text = SelPvpCtrl.GUIEnemyDifficulty.DIFFICULTY_NAME_STR_SPECIAL_EASY;
				return;
			}
			this.Txt_Hard.text = SelPvpCtrl.GUIEnemyDifficulty.DIFFICULTY_NAME_STR_NORMAL_HARD;
			this.Txt_Normal.text = SelPvpCtrl.GUIEnemyDifficulty.DIFFICULTY_NAME_STR_NORMAL_NORMAL;
			this.Txt_Easy.text = SelPvpCtrl.GUIEnemyDifficulty.DIFFICULTY_NAME_STR_NORMAL_EASY;
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x002092E0 File Offset: 0x002074E0
		public void Setup(PvpDynamicData.EnemyInfo enemy)
		{
			this.HardGo.SetActive(PvpDynamicData.EnemyInfo.Difficulty.HARD == enemy.difficulty);
			this.NormalGo.SetActive(PvpDynamicData.EnemyInfo.Difficulty.NORMAL == enemy.difficulty);
			this.EasyGo.SetActive(PvpDynamicData.EnemyInfo.Difficulty.EASY == enemy.difficulty);
			bool flag = 1 == enemy.oppUser.topRank;
			this.ChampGo.SetActive(PvpDynamicData.EnemyInfo.Difficulty.CHAMPION == enemy.difficulty && flag);
			this.RankerGo.SetActive(PvpDynamicData.EnemyInfo.Difficulty.CHAMPION == enemy.difficulty && !flag);
			if (this.RankerGo.activeSelf)
			{
				this.Num_TopRank.ReplaceTextByDefault("Param01", enemy.oppUser.topRank.ToString());
			}
		}

		// Token: 0x040049A1 RID: 18849
		private static readonly string DIFFICULTY_NAME_STR_NORMAL_HARD = "てごわい";

		// Token: 0x040049A2 RID: 18850
		private static readonly string DIFFICULTY_NAME_STR_NORMAL_NORMAL = "とんとん";

		// Token: 0x040049A3 RID: 18851
		private static readonly string DIFFICULTY_NAME_STR_NORMAL_EASY = "やさしい";

		// Token: 0x040049A4 RID: 18852
		private static readonly string DIFFICULTY_NAME_STR_SPECIAL_HARD = "げきせん";

		// Token: 0x040049A5 RID: 18853
		private static readonly string DIFFICULTY_NAME_STR_SPECIAL_NORMAL = "はくねつ";

		// Token: 0x040049A6 RID: 18854
		private static readonly string DIFFICULTY_NAME_STR_SPECIAL_EASY = "てあわせ";

		// Token: 0x040049A7 RID: 18855
		public GameObject HardGo;

		// Token: 0x040049A8 RID: 18856
		public PguiTextCtrl Txt_Hard;

		// Token: 0x040049A9 RID: 18857
		public GameObject NormalGo;

		// Token: 0x040049AA RID: 18858
		public PguiTextCtrl Txt_Normal;

		// Token: 0x040049AB RID: 18859
		public GameObject EasyGo;

		// Token: 0x040049AC RID: 18860
		public PguiTextCtrl Txt_Easy;

		// Token: 0x040049AD RID: 18861
		public GameObject ChampGo;

		// Token: 0x040049AE RID: 18862
		public GameObject RankerGo;

		// Token: 0x040049AF RID: 18863
		public PguiTextCtrl Num_TopRank;

		// Token: 0x02001194 RID: 4500
		public enum NameType
		{
			// Token: 0x04006040 RID: 24640
			Normal,
			// Token: 0x04006041 RID: 24641
			Special
		}
	}

	// Token: 0x02000C0B RID: 3083
	public class GUIRewardWindow
	{
		// Token: 0x0600449A RID: 17562 RVA: 0x002093D4 File Offset: 0x002075D4
		public GUIRewardWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Num_Txt_ResetDate = baseTr.Find("Base/Window/Base_Reward/Reward_ListAll/ScrollView_RankUp/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Num_Txt_PvpRank = baseTr.Find("Base/Window/Base_Reward/Reward_ListAll/ScrollView_RankUp/Tex_Rank/Num_Txt").GetComponent<PguiTextCtrl>();
			this.ScrollView_RankUp = baseTr.Find("Base/Window/Base_Reward/Reward_ListAll/ScrollView_RankUp").GetComponent<ReuseScroll>();
			this.ScrollView_Bonus = baseTr.Find("Base/Window/Base_Reward/Reward_ListAll/ScrollView_Bonus").GetComponent<ReuseScroll>();
			this.TabGroup = baseTr.Find("Base/Window/Base_Reward/TabGroup").GetComponent<PguiTabGroupCtrl>();
		}

		// Token: 0x040049B0 RID: 18864
		public GameObject baseObj;

		// Token: 0x040049B1 RID: 18865
		public PguiOpenWindowCtrl window;

		// Token: 0x040049B2 RID: 18866
		public PguiTextCtrl Num_Txt_PvpRank;

		// Token: 0x040049B3 RID: 18867
		public PguiTextCtrl Num_Txt_ResetDate;

		// Token: 0x040049B4 RID: 18868
		public ReuseScroll ScrollView_RankUp;

		// Token: 0x040049B5 RID: 18869
		public ReuseScroll ScrollView_Bonus;

		// Token: 0x040049B6 RID: 18870
		public PguiTabGroupCtrl TabGroup;

		// Token: 0x040049B7 RID: 18871
		public List<SelPvpCtrl.GUIRewardWindow.Plate> plateListRank = new List<SelPvpCtrl.GUIRewardWindow.Plate>();

		// Token: 0x040049B8 RID: 18872
		public List<SelPvpCtrl.GUIRewardWindow.Plate> plateListDefense = new List<SelPvpCtrl.GUIRewardWindow.Plate>();

		// Token: 0x02001195 RID: 4501
		public class Plate
		{
			// Token: 0x0600569D RID: 22173 RVA: 0x00252DAC File Offset: 0x00250FAC
			public Plate(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Num_Txt = baseTr.Find("Num_Txt").GetComponent<PguiTextCtrl>();
				this.iconItemList = new List<IconItemCtrl>
				{
					Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, baseTr.Find("Base/Day_Item/Tex_Item01")).GetComponent<IconItemCtrl>(),
					Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, baseTr.Find("Base/Day_Item/Tex_Item02")).GetComponent<IconItemCtrl>(),
					Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, baseTr.Find("Base/Day_Item/Tex_Item03")).GetComponent<IconItemCtrl>()
				};
				Transform transform = baseTr.Find("Base/Contents");
				if (null != transform)
				{
					this.pointNameObjNormal = transform.Find("Txt_01").gameObject;
					this.pointNameObjSpecial = transform.Find("Txt_02").gameObject;
					this.point = transform.Find("Num_Txt").GetComponent<PguiTextCtrl>();
				}
			}

			// Token: 0x04006042 RID: 24642
			public GameObject baseObj;

			// Token: 0x04006043 RID: 24643
			public PguiTextCtrl Num_Txt;

			// Token: 0x04006044 RID: 24644
			public List<IconItemCtrl> iconItemList;

			// Token: 0x04006045 RID: 24645
			public GameObject pointNameObjNormal;

			// Token: 0x04006046 RID: 24646
			public GameObject pointNameObjSpecial;

			// Token: 0x04006047 RID: 24647
			public PguiTextCtrl point;
		}
	}

	// Token: 0x02000C0C RID: 3084
	public class GUIDefenseResultWindow
	{
		// Token: 0x0600449B RID: 17563 RVA: 0x00209484 File Offset: 0x00207684
		public GUIDefenseResultWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.ScrollView = baseTr.Find("Base/Window/Base_Day_Item/InBase/ScrollView").GetComponent<ReuseScroll>();
		}

		// Token: 0x040049B9 RID: 18873
		public GameObject baseObj;

		// Token: 0x040049BA RID: 18874
		public PguiOpenWindowCtrl window;

		// Token: 0x040049BB RID: 18875
		public ReuseScroll ScrollView;

		// Token: 0x040049BC RID: 18876
		public List<SelPvpCtrl.GUIDefenseResultWindow.Plate> plateList = new List<SelPvpCtrl.GUIDefenseResultWindow.Plate>();

		// Token: 0x02001196 RID: 4502
		public class Plate
		{
			// Token: 0x0600569E RID: 22174 RVA: 0x00252EB4 File Offset: 0x002510B4
			public Plate(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Num_Txt = baseTr.Find("Base/TitleBase/Num_Txt").GetComponent<PguiTextCtrl>();
				this.Num_Results = baseTr.Find("Base/Num_Results").GetComponent<PguiTextCtrl>();
				this.iconItemList = new List<IconItemCtrl>
				{
					Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, baseTr.Find("Base/Day_Item01/Tex_Item01")).GetComponent<IconItemCtrl>(),
					Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, baseTr.Find("Base/Day_Item01/Tex_Item02")).GetComponent<IconItemCtrl>(),
					Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, baseTr.Find("Base/Day_Item01/Tex_Item03")).GetComponent<IconItemCtrl>()
				};
			}

			// Token: 0x04006048 RID: 24648
			public GameObject baseObj;

			// Token: 0x04006049 RID: 24649
			public PguiTextCtrl Num_Txt;

			// Token: 0x0400604A RID: 24650
			public PguiTextCtrl Num_Results;

			// Token: 0x0400604B RID: 24651
			public List<IconItemCtrl> iconItemList;
		}
	}

	// Token: 0x02000C0D RID: 3085
	public class GUI
	{
		// Token: 0x0600449C RID: 17564 RVA: 0x002094D0 File Offset: 0x002076D0
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseAnime = baseTr.GetComponent<SimpleAnimation>();
			this.Btn_MoreInfo_Special = baseTr.Find("All/EventMode/All/LocationInfo_Event/Btn_MoreInfo").GetComponent<PguiButtonCtrl>();
			this.MoreInfoEvent = baseTr.Find("All/EventMode").gameObject;
			this.EventAnime = baseTr.Find("All/EventMode").GetComponent<SimpleAnimation>();
			this.Tex_Mark_Night_Special = baseTr.Find("All/EventMode/All/LocationInfo_Event/Mark_Night").GetComponent<PguiImageCtrl>();
			this.Num_Txt_Traits_Special = baseTr.Find("All/EventMode/All/LocationInfo_Event/Txt_Location").GetComponent<PguiTextCtrl>();
			this.Btn_MoreInfo_Normal = baseTr.Find("All/LocationInfo_Nomal/Btn_MoreInfo").GetComponent<PguiButtonCtrl>();
			this.LocationInfoNormal = baseTr.Find("All/LocationInfo_Nomal").gameObject;
			this.Tex_Mark_Night_Normal = baseTr.Find("All/LocationInfo_Nomal/Mark_Night").GetComponent<PguiImageCtrl>();
			this.Num_Txt_Traits_Normal = baseTr.Find("All/LocationInfo_Nomal/Txt_Location").GetComponent<PguiTextCtrl>();
			this.guiNormalData = new SelPvpCtrl.GUINormal(baseTr.Find("All/Left"));
			this.guiSpecialData = new SelPvpCtrl.GUISpecial(baseTr.Find("All/Left_Event"));
			this.guiNormalEnemyScrollData = new SelPvpCtrl.GUINormalEnemyScroll(baseTr.Find("All/Right"));
			this.guiSpecialEnemyScrollData = new SelPvpCtrl.GUISpecialEnemyScroll(baseTr.Find("All/Right_Event"));
		}

		// Token: 0x040049BD RID: 18877
		public GameObject baseObj;

		// Token: 0x040049BE RID: 18878
		public SimpleAnimation baseAnime;

		// Token: 0x040049BF RID: 18879
		public PguiButtonCtrl Btn_MoreInfo_Special;

		// Token: 0x040049C0 RID: 18880
		public GameObject MoreInfoEvent;

		// Token: 0x040049C1 RID: 18881
		public SimpleAnimation EventAnime;

		// Token: 0x040049C2 RID: 18882
		public PguiImageCtrl Tex_Mark_Night_Special;

		// Token: 0x040049C3 RID: 18883
		public PguiTextCtrl Num_Txt_Traits_Special;

		// Token: 0x040049C4 RID: 18884
		public PguiButtonCtrl Btn_MoreInfo_Normal;

		// Token: 0x040049C5 RID: 18885
		public GameObject LocationInfoNormal;

		// Token: 0x040049C6 RID: 18886
		public PguiImageCtrl Tex_Mark_Night_Normal;

		// Token: 0x040049C7 RID: 18887
		public PguiTextCtrl Num_Txt_Traits_Normal;

		// Token: 0x040049C8 RID: 18888
		public SelPvpCtrl.GUINormal guiNormalData;

		// Token: 0x040049C9 RID: 18889
		public SelPvpCtrl.GUINormalEnemyScroll guiNormalEnemyScrollData;

		// Token: 0x040049CA RID: 18890
		public SelPvpCtrl.GUISpecial guiSpecialData;

		// Token: 0x040049CB RID: 18891
		public SelPvpCtrl.GUISpecialEnemyScroll guiSpecialEnemyScrollData;
	}

	// Token: 0x02000C0E RID: 3086
	public class GUINormal
	{
		// Token: 0x0600449D RID: 17565 RVA: 0x0020961C File Offset: 0x0020781C
		public GUINormal(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Plus = baseTr.Find("Player_InfoBase/ChallangeInfo/Btn_Plus").GetComponent<PguiButtonCtrl>();
			this.Btn_Shop = baseTr.Find("Player_InfoBase/PvPInfo/Info/Btn_Shop").GetComponent<PguiButtonCtrl>();
			this.Btn_CharaDeck = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Btn_CharaDeck").GetComponent<PguiButtonCtrl>();
			this.Btn_ModeChange = baseTr.Find("Player_InfoBase/Btn_ModeChange").GetComponent<PguiButtonCtrl>();
			this.Txt_Stamina = baseTr.Find("Player_InfoBase/ChallangeInfo/Num_Challange").GetComponent<PguiTextCtrl>();
			this.Txt_UserName = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Txt_FriendName").GetComponent<PguiTextCtrl>();
			this.Num_Rank = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Num_Rank").GetComponent<PguiTextCtrl>();
			this.Txt_PartyName = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/PartyName/Txt_PartyName").GetComponent<PguiTextCtrl>();
			this.iconCharaList = new List<IconCharaCtrl>
			{
				baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Icon_Chara01/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Icon_Chara02/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Icon_Chara03/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Icon_Chara04/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Icon_Chara05/Icon_Chara").GetComponent<IconCharaCtrl>()
			};
			this.Num_TotalAttack = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Total_Attack/Num_TotalAttack").GetComponent<PguiTextCtrl>();
			this.Num_Txt_BattleRank = baseTr.Find("Player_InfoBase/PvPInfo/Info/Contents01/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Num_Txt_BattlePoint = baseTr.Find("Player_InfoBase/PvPInfo/Info/Contents02/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Num_Txt_NextBattlePoint = baseTr.Find("Player_InfoBase/PvPInfo/Info/Contents03/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Num_Txt_Event = baseTr.Find("Player_InfoBase/PvPInfo/Info/Contents04/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Num_Txt_StaminaRecovery = baseTr.Find("Player_InfoBase/ChallangeInfo/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Num_Txt_ItemNum = baseTr.Find("Player_InfoBase/PvPInfo/Info/ItemInfo/Num_Txt").GetComponent<PguiTextCtrl>();
			this.CampaignObj = baseTr.Find("SelCmn_CampaignInfo_Quest").gameObject;
			this.Txt_Campaign = baseTr.Find("SelCmn_CampaignInfo_Quest/Txt_Campaign").GetComponent<PguiTextCtrl>();
			this.Txt_CampaignTime = baseTr.Find("SelCmn_CampaignInfo_Quest/TimeInfo/Num_Time").GetComponent<PguiTextCtrl>();
			this.Num_Txt_Event.transform.parent.gameObject.SetActive(false);
		}

		// Token: 0x040049CC RID: 18892
		public GameObject baseObj;

		// Token: 0x040049CD RID: 18893
		public PguiButtonCtrl Btn_Plus;

		// Token: 0x040049CE RID: 18894
		public PguiButtonCtrl Btn_Shop;

		// Token: 0x040049CF RID: 18895
		public PguiButtonCtrl Btn_CharaDeck;

		// Token: 0x040049D0 RID: 18896
		public PguiButtonCtrl Btn_ModeChange;

		// Token: 0x040049D1 RID: 18897
		public PguiTextCtrl Txt_Stamina;

		// Token: 0x040049D2 RID: 18898
		public PguiTextCtrl Num_Rank;

		// Token: 0x040049D3 RID: 18899
		public PguiTextCtrl Txt_UserName;

		// Token: 0x040049D4 RID: 18900
		public PguiTextCtrl Txt_PartyName;

		// Token: 0x040049D5 RID: 18901
		public List<IconCharaCtrl> iconCharaList;

		// Token: 0x040049D6 RID: 18902
		public PguiTextCtrl Num_TotalAttack;

		// Token: 0x040049D7 RID: 18903
		public PguiTextCtrl Num_Txt_BattleRank;

		// Token: 0x040049D8 RID: 18904
		public PguiTextCtrl Num_Txt_BattlePoint;

		// Token: 0x040049D9 RID: 18905
		public PguiTextCtrl Num_Txt_NextBattlePoint;

		// Token: 0x040049DA RID: 18906
		public PguiTextCtrl Num_Txt_Event;

		// Token: 0x040049DB RID: 18907
		public PguiTextCtrl Num_Txt_StaminaRecovery;

		// Token: 0x040049DC RID: 18908
		public PguiTextCtrl Num_Txt_ItemNum;

		// Token: 0x040049DD RID: 18909
		public GameObject CampaignObj;

		// Token: 0x040049DE RID: 18910
		public PguiTextCtrl Txt_Campaign;

		// Token: 0x040049DF RID: 18911
		public PguiTextCtrl Txt_CampaignTime;
	}

	// Token: 0x02000C0F RID: 3087
	public class GUINormalEnemyScroll
	{
		// Token: 0x0600449E RID: 17566 RVA: 0x0020985C File Offset: 0x00207A5C
		public GUINormalEnemyScroll(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Update = baseTr.Find("RivalList/TitleBase/Btn_Update").GetComponent<PguiButtonCtrl>();
			this.ScrollView = baseTr.Find("RivalList/ScrollView").GetComponent<ReuseScroll>();
		}

		// Token: 0x040049E0 RID: 18912
		public GameObject baseObj;

		// Token: 0x040049E1 RID: 18913
		public PguiButtonCtrl Btn_Update;

		// Token: 0x040049E2 RID: 18914
		public ReuseScroll ScrollView;

		// Token: 0x040049E3 RID: 18915
		public List<SelPvpCtrl.GUIEnemy> enemyList = new List<SelPvpCtrl.GUIEnemy>();

		// Token: 0x040049E4 RID: 18916
		public List<PvpDynamicData.EnemyInfo> enemyInfoList = new List<PvpDynamicData.EnemyInfo>();
	}

	// Token: 0x02000C10 RID: 3088
	public class GUISpecial
	{
		// Token: 0x0600449F RID: 17567 RVA: 0x002098C0 File Offset: 0x00207AC0
		public GUISpecial(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.renderTextureBase = baseTr.Find("Player_InfoBase/RenderChara_Left").gameObject;
			this.renderTextureBase.transform.localPosition = new Vector3(-160f, 42f, 0f);
			this.Btn_Tips = baseTr.Find("Player_InfoBase/Btn_Tips").GetComponent<PguiButtonCtrl>();
			this.Num_Txt_BattleRank = baseTr.Find("Player_InfoBase/ClassBase/Num").GetComponent<PguiTextCtrl>();
			this.Num_Txt_NextBattlePoint = baseTr.Find("Player_InfoBase/ClassBase/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Obj_NextClass = baseTr.Find("Player_InfoBase/ClassBase/Base").gameObject;
			this.Btn_Plus = baseTr.Find("Player_InfoBase/ChallangeInfo/Btn_Plus").GetComponent<PguiButtonCtrl>();
			this.Txt_Stamina = baseTr.Find("Player_InfoBase/ChallangeInfo/Num_Challange").GetComponent<PguiTextCtrl>();
			this.Btn_ModeChange = baseTr.Find("Player_InfoBase/Btn_ModeChange").GetComponent<PguiButtonCtrl>();
			this.Num_Txt_StaminaRecovery = baseTr.Find("Player_InfoBase/ChallangeInfo/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Img_Item = baseTr.Find("Player_InfoBase/ItemInfo/Icon_Tex_New").GetComponent<PguiRawImageCtrl>();
			this.Num_Txt_ItemNum = baseTr.Find("Player_InfoBase/ItemInfo/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Btn_Mission = baseTr.Find("Player_InfoBase/Btn_Mission").GetComponent<PguiButtonCtrl>();
			this.Mission_New = baseTr.Find("Player_InfoBase/Btn_Mission/BaseImage/Mark_New").gameObject;
			this.Mission_New.SetActive(false);
			this.Txt_Mission_Num = baseTr.Find("Player_InfoBase/Btn_Mission/BaseImage/Badge/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
			this.Btn_Gacha = baseTr.Find("Player_InfoBase/Btn_Gacha").GetComponent<PguiButtonCtrl>();
			this.Btn_Shop = baseTr.Find("Player_InfoBase/Btn_ShopEvent").GetComponent<PguiButtonCtrl>();
			this.Info_PhotoItemEffect = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Info_PhotoItemEffect").GetComponent<InfoPhotoItemEffectCtrl>();
			baseTr.Find("Player_InfoBase/UserDeck/BaseImage").GetComponent<Image>().raycastTarget = true;
			this.Num_Rank = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Num_Rank").GetComponent<PguiTextCtrl>();
			this.Txt_UserName = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Txt_FriendName").GetComponent<PguiTextCtrl>();
			this.Txt_PartyName = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/PartyName/Txt_PartyName").GetComponent<PguiTextCtrl>();
			this.iconCharaList = new List<IconCharaCtrl>
			{
				baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Icon_Chara01/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Icon_Chara02/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Icon_Chara03/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Icon_Chara04/Icon_Chara").GetComponent<IconCharaCtrl>(),
				baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Icon_Chara05/Icon_Chara").GetComponent<IconCharaCtrl>()
			};
			this.Btn_CharaDeck = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Btn_CharaDeck").GetComponent<PguiButtonCtrl>();
			this.Num_TotalAttack = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Total_Attack/Num_TotalAttack").GetComponent<PguiTextCtrl>();
			this.Num_TotalPlasm = baseTr.Find("Player_InfoBase/UserDeck/BaseImage/Total_Plasm/Num_TotalAttack").GetComponent<PguiTextCtrl>();
			this.baseObj.SetActive(false);
		}

		// Token: 0x040049E5 RID: 18917
		public GameObject baseObj;

		// Token: 0x040049E6 RID: 18918
		public RenderTextureChara renderTexture;

		// Token: 0x040049E7 RID: 18919
		public GameObject renderTextureBase;

		// Token: 0x040049E8 RID: 18920
		public PguiButtonCtrl Btn_Tips;

		// Token: 0x040049E9 RID: 18921
		public PguiTextCtrl Num_Txt_BattleRank;

		// Token: 0x040049EA RID: 18922
		public PguiTextCtrl Num_Txt_NextBattlePoint;

		// Token: 0x040049EB RID: 18923
		public GameObject Obj_NextClass;

		// Token: 0x040049EC RID: 18924
		public PguiButtonCtrl Btn_Plus;

		// Token: 0x040049ED RID: 18925
		public PguiTextCtrl Txt_Stamina;

		// Token: 0x040049EE RID: 18926
		public PguiButtonCtrl Btn_ModeChange;

		// Token: 0x040049EF RID: 18927
		public PguiTextCtrl Num_Txt_StaminaRecovery;

		// Token: 0x040049F0 RID: 18928
		public PguiRawImageCtrl Img_Item;

		// Token: 0x040049F1 RID: 18929
		public PguiTextCtrl Num_Txt_ItemNum;

		// Token: 0x040049F2 RID: 18930
		public PguiButtonCtrl Btn_Mission;

		// Token: 0x040049F3 RID: 18931
		public GameObject Mission_New;

		// Token: 0x040049F4 RID: 18932
		public PguiTextCtrl Txt_Mission_Num;

		// Token: 0x040049F5 RID: 18933
		public PguiButtonCtrl Btn_Gacha;

		// Token: 0x040049F6 RID: 18934
		public PguiButtonCtrl Btn_Shop;

		// Token: 0x040049F7 RID: 18935
		public InfoPhotoItemEffectCtrl Info_PhotoItemEffect;

		// Token: 0x040049F8 RID: 18936
		public PguiTextCtrl Num_Rank;

		// Token: 0x040049F9 RID: 18937
		public PguiTextCtrl Txt_UserName;

		// Token: 0x040049FA RID: 18938
		public PguiTextCtrl Txt_PartyName;

		// Token: 0x040049FB RID: 18939
		public List<IconCharaCtrl> iconCharaList;

		// Token: 0x040049FC RID: 18940
		public PguiButtonCtrl Btn_CharaDeck;

		// Token: 0x040049FD RID: 18941
		public PguiTextCtrl Num_TotalAttack;

		// Token: 0x040049FE RID: 18942
		public PguiTextCtrl Num_TotalPlasm;
	}

	// Token: 0x02000C11 RID: 3089
	public class GUISpecialEnemyScroll
	{
		// Token: 0x060044A0 RID: 17568 RVA: 0x00209BA4 File Offset: 0x00207DA4
		public GUISpecialEnemyScroll(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.tabGroup = baseTr.Find("TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.Btn_Update = baseTr.Find("RivalList/TitleBase/Btn_Update").GetComponent<PguiButtonCtrl>();
			this.Btn_Scenerio = baseTr.Find("Btn_Scenario").GetComponent<PguiButtonCtrl>();
			this.ScrollView_Hard = baseTr.Find("RivalList/ScrollView_Hard").GetComponent<ReuseScroll>();
			this.ScrollView_Normal = baseTr.Find("RivalList/ScrollView_Normal").GetComponent<ReuseScroll>();
			this.ScrollView_Easy = baseTr.Find("RivalList/ScrollView_Easy").GetComponent<ReuseScroll>();
		}

		// Token: 0x040049FF RID: 18943
		public GameObject baseObj;

		// Token: 0x04004A00 RID: 18944
		public PguiTabGroupCtrl tabGroup;

		// Token: 0x04004A01 RID: 18945
		public PguiButtonCtrl Btn_Update;

		// Token: 0x04004A02 RID: 18946
		public PguiButtonCtrl Btn_Scenerio;

		// Token: 0x04004A03 RID: 18947
		public ReuseScroll ScrollView_Hard;

		// Token: 0x04004A04 RID: 18948
		public ReuseScroll ScrollView_Normal;

		// Token: 0x04004A05 RID: 18949
		public ReuseScroll ScrollView_Easy;

		// Token: 0x04004A06 RID: 18950
		public List<SelPvpCtrl.GUIEnemy> guiEnemyList_Hard = new List<SelPvpCtrl.GUIEnemy>();

		// Token: 0x04004A07 RID: 18951
		public List<SelPvpCtrl.GUIEnemy> guiEnemyList_Normal = new List<SelPvpCtrl.GUIEnemy>();

		// Token: 0x04004A08 RID: 18952
		public List<SelPvpCtrl.GUIEnemy> guiEnemyList_Easy = new List<SelPvpCtrl.GUIEnemy>();

		// Token: 0x04004A09 RID: 18953
		public List<PvpDynamicData.EnemyInfo> enemyInfoList_Hard = new List<PvpDynamicData.EnemyInfo>();

		// Token: 0x04004A0A RID: 18954
		public List<PvpDynamicData.EnemyInfo> enemyInfoList_Normal = new List<PvpDynamicData.EnemyInfo>();

		// Token: 0x04004A0B RID: 18955
		public List<PvpDynamicData.EnemyInfo> enemyInfoList_Easy = new List<PvpDynamicData.EnemyInfo>();
	}
}
