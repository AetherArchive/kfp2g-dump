using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AEAuth3;
using Battle;
using CriWare;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011E RID: 286
public class SceneBattleResult : BaseScene
{
	// Token: 0x06000E84 RID: 3716 RVA: 0x000ABF20 File Offset: 0x000AA120
	public override void OnCreateScene()
	{
		this.basePanel = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneBattleResult/GUI/Prefab/GUI_BattleResult"));
		Graphic[] array = this.basePanel.GetComponentsInChildren<Graphic>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].raycastTarget = false;
		}
		this.backPanel = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneBattleResult/GUI/Prefab/GUI_BattleResult_Back"));
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.BACK, this.backPanel.transform, true);
		this.windowPanel = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneBattleResult/GUI/Prefab/GUI_BattleResult_Window"));
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.windowPanel.transform, true);
		this.windowPanel.transform.SetAsFirstSibling();
		PguiPanel component = this.windowPanel.GetComponent<PguiPanel>();
		if (component != null)
		{
			component.raycastTarget = false;
		}
		this.guiData = new SceneBattleResult.GUI(this.basePanel.transform);
		this.charaNorm = new SceneBattleResult.GUIChara(this.guiData.BoardNorm.transform.Find("Page01/CharaExp"), false);
		this.charaPvp = new SceneBattleResult.GUIChara(this.guiData.BoardPvp.transform.Find("Page01/CharaExp"), false);
		this.charaPvpTraining = new SceneBattleResult.GUIChara(this.guiData.BoardPvpTraining.transform.Find("Page01/CharaExp"), false);
		this.charaTraining = new SceneBattleResult.GUIChara(this.guiData.BoardTraining.transform.Find("Page01/CharaExp"), true);
		this.missionIcon = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item).GetComponent<IconItemCtrl>();
		this.missionIcon.transform.SetParent(this.guiData.MissionItem.Find("Icon_Item"), false);
		this.rankupWindow = this.windowPanel.transform.Find("Window_PlayerRankUp").GetComponent<PguiOpenWindowCtrl>();
		this.rankWinBefore = this.rankupWindow.m_UserInfoContent.Find("Txt_Rank_Before").GetComponent<PguiTextCtrl>();
		this.rankWinAfter = this.rankupWindow.m_UserInfoContent.Find("Txt_Rank_After").GetComponent<PguiTextCtrl>();
		this.rewardIcon = new List<IconItemCtrl>();
		foreach (GameObject gameObject in this.guiData.RewardItem)
		{
			IconItemCtrl component2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item).GetComponent<IconItemCtrl>();
			component2.transform.SetParent(gameObject.transform, false);
			this.rewardIcon.Add(component2);
		}
		this.kizunaWindow = this.windowPanel.transform.Find("Auth_HeartLvUp").gameObject;
		this.kizunaWinWhite = this.kizunaWindow.transform.Find("AEImage_White").GetComponent<PguiAECtrl>();
		this.kizunaWinBack = this.kizunaWindow.transform.Find("AEImage_Back").GetComponent<PguiAECtrl>();
		this.kizunaWinFront = this.kizunaWindow.transform.Find("AEImage_Front").GetComponent<PguiAECtrl>();
		this.kizunaWinInfo = this.kizunaWindow.transform.Find("AEImage_Info").GetComponent<PguiAECtrl>();
		this.kizunaWinId = 0;
		this.kizunaWinCloth = 0;
		this.kizunaWinLongSkirt = false;
		this.kizunaWinChara = null;
		this.kizunaWinCharaVoice = false;
		this.kizunaWinTime = 0f;
		this.kizunaWinChrY = 0f;
		this.kizunaWinItem = new List<ItemData>();
		this.guiData.Touch.gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.touch = true;
		}, null, null, null, null);
		this.guiData.RematchBtn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.CheckRematch), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.RematchBtn.GetComponent<Graphic>().raycastTarget = true;
		this.guiData.SkipBtn.GetComponent<Graphic>().raycastTarget = true;
		array = this.guiData.ScrollBar.GetComponentsInChildren<Graphic>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].raycastTarget = true;
		}
		this.seLoad = SoundManager.LoadCueSheetWithDownload("se_cb");
		this.expSE = false;
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x000AC36C File Offset: 0x000AA56C
	private void CheckRematch(PguiButtonCtrl btn)
	{
		if (this.requestRematch != null || this.requestNextScene)
		{
			return;
		}
		if (this.rematch <= 0)
		{
			return;
		}
		if (this.page < 2)
		{
			return;
		}
		if (!this.guiData.Rematch.gameObject.activeSelf)
		{
			return;
		}
		this.guiData.SkipBtn.SetActEnable(false, false, true);
		List<int> list = new List<int>();
		foreach (CharaPackData charaPackData in this.resultArgs.deck.deckData)
		{
			if (charaPackData != null && charaPackData.dynamicData.OwnerType == CharaDynamicData.CharaOwnerType.User)
			{
				list.Add(charaPackData.id);
			}
		}
		this.checkKizunaLimitReached = QuestUtil.NoticeKizunaLimitReached(delegate
		{
			this.rematch = -1;
		}, this.resultArgs.battleArgs.questOneId, list, 1, null, 0, 0, false);
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x000AC464 File Offset: 0x000AA664
	private IEnumerator Rematch()
	{
		SceneManager.SceneName scene = SceneManager.SceneName.SceneBattle;
		if (this.IsChangeTerm())
		{
			DataManager.DmEvent.RequestGetCoopInfo(DataManager.DmEvent.LastCoopInfo.EventId, 0);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			string text = "更新データが見つかりました\nデータ更新を行います";
			CanvasManager.HdlOpenWindowBasic.Setup("確認", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int idx) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			scene = SceneManager.SceneName.SceneQuest;
			Singleton<SceneManager>.Instance.SetNextScene(scene, null);
			this.requestRematch = null;
			yield break;
		}
		if (this.rematchOwn < 0)
		{
			StaminaRecoveryWindowCtrl hdlStaminaRecoveryWindowCtrl = CanvasManager.HdlStaminaRecoveryWindowCtrl;
			IEnumerator checkStamina = hdlStaminaRecoveryWindowCtrl.StaminaCheckAction(this.resultArgs.battleArgs.questOneId, 1);
			bool staminaChkResult = false;
			while (checkStamina.MoveNext())
			{
				staminaChkResult = checkStamina.Current != null && (bool)checkStamina.Current;
				yield return null;
			}
			if (!staminaChkResult)
			{
				this.rematch = 1;
				yield break;
			}
			checkStamina = null;
		}
		else if (this.rematchOwn < this.rematchNeed)
		{
			string text2 = this.rematchItem + "が不足しています\n\n必要数\u3000" + this.rematchNeed.ToString();
			CanvasManager.HdlOpenWindowBasic.Setup("確認", text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int idx) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.ForceOpen();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			this.page = 3;
			this.guiData.Touch.SetAsLastSibling();
			this.guiData.Rematch.gameObject.SetActive(false);
			yield break;
		}
		yield return null;
		this.page = 4;
		DataManager.DmHelper.RequestGetRentalHelper(this.resultArgs.battleArgs.questOneId, false);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		List<HelperPackData> helperList = new List<HelperPackData>(DataManager.DmHelper.GetRentalHelperList());
		helperList.RemoveAll((HelperPackData item) => 0 >= item.HelperCharaSetList.Count || item.HelperCharaSetList[0].helpChara == null || item.HelperCharaSetList[0].helpChara.IsInvalid());
		if (helperList.Count <= 0)
		{
			this.requestNextScene = true;
			yield break;
		}
		QuestOnePackData qopd = DataManager.DmQuest.GetQuestOnePackData(this.resultArgs.battleArgs.questOneId);
		List<DataManagerChara.BonusCharaData> bonusCharaData = ((qopd.questOne.questId > 0) ? DataManager.DmChara.GetBonusCharaDataList(QuestUtil.GetEventId(qopd.questOne.questId, false)) : new List<DataManagerChara.BonusCharaData>());
		if (qopd != null)
		{
			helperList.Sort((HelperPackData a, HelperPackData b) => SelBattleHelperCtrl.SortList(a, b, qopd, 0, bonusCharaData));
		}
		HelperPackData.HelperCharaSet hcs = helperList[0].HelperCharaSetList[0];
		List<long> photoDataIdList = null;
		if (hcs.helpPhotoList != null && hcs.helpPhotoList.Count > 0)
		{
			photoDataIdList = new List<long>();
			foreach (PhotoPackData photoPackData in hcs.helpPhotoList)
			{
				photoDataIdList.Add((photoPackData == null) ? 0L : photoPackData.dataId);
			}
		}
		long totalPoint = 0L;
		int nowMapId = this.resultArgs.quest.staticOneData.questMap.mapId;
		if (DataManager.DmEvent.isRaidByMapId(nowMapId))
		{
			List<int> oldDrawIdList = this.GetConvertDrawId();
			DataManager.DmEvent.RequestGetCoopInfo(DataManager.DmEvent.LastCoopInfo.EventId, nowMapId);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			List<int> convertDrawId = this.GetConvertDrawId();
			totalPoint = (DataManager.DmEvent.isRaidByMapId(nowMapId) ? DataManager.DmEvent.LastCoopInfo.MapInfoMap[nowMapId].TotalPoint : 0L);
			oldDrawIdList.Sort();
			convertDrawId.Sort();
			if (!oldDrawIdList.SequenceEqual<int>(convertDrawId))
			{
				SceneBattleResult.<>c__DisplayClass86_1 CS$<>8__locals2 = new SceneBattleResult.<>c__DisplayClass86_1();
				CS$<>8__locals2.isNoProblem = false;
				string text3 = "報酬内容が変わりました。\n再出撃しますか？";
				CanvasManager.HdlOpenWindowBasic.Setup("確認", text3, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int idx)
				{
					CS$<>8__locals2.isNoProblem = idx == 1;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				if (!CS$<>8__locals2.isNoProblem)
				{
					yield break;
				}
				CS$<>8__locals2 = null;
			}
			oldDrawIdList = null;
		}
		DataManager.DmQuest.RequestActionBattleStart(this.resultArgs.battleArgs.questOneId, this.resultArgs.battleArgs.selectDeckId, helperList[0].friendId, hcs.helpChara.id, photoDataIdList, totalPoint);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		SceneBattleArgs sceneBattleArgs = new SceneBattleArgs();
		sceneBattleArgs.oppUser = null;
		sceneBattleArgs.difficulty = PvpDynamicData.EnemyInfo.Difficulty.INVALID;
		sceneBattleArgs.hash_id = DataManager.DmQuest.LastQuestStartResponse.hash_id;
		sceneBattleArgs.questOneId = this.resultArgs.battleArgs.questOneId;
		sceneBattleArgs.waveEnemiesIdList = DataManager.DmQuest.LastQuestStartResponse.waveEnemiesIdList;
		sceneBattleArgs.dropItemList = DataManager.DmQuest.LastQuestStartResponse.drew_items;
		sceneBattleArgs.startTime = DataManager.DmQuest.LastQuestStartResponse.startTime;
		DataManagerEvent.EventData eventData = ((qopd == null) ? null : DataManager.DmEvent.GetEventDataList().Find((DataManagerEvent.EventData itm) => itm.eventChapterId == qopd.questChapter.chapterId));
		sceneBattleArgs.eventId = ((eventData == null) ? 0 : eventData.eventId);
		sceneBattleArgs.selectDeckId = this.resultArgs.battleArgs.selectDeckId;
		sceneBattleArgs.helper = helperList[0].helper;
		sceneBattleArgs.attrIndex = 0;
		sceneBattleArgs.resultNextSceneName = this.resultArgs.battleArgs.resultNextSceneName;
		sceneBattleArgs.resultNextSceneArgs = this.resultArgs.battleArgs.resultNextSceneArgs;
		sceneBattleArgs.isQuestNoClear = qopd == null || qopd.questDynamicOne == null || qopd.questDynamicOne.clearNum == 0;
		SceneBattle.SetRestart(sceneBattleArgs);
		object obj = sceneBattleArgs;
		string text4 = ((qopd == null || qopd.questDynamicOne == null || qopd.questDynamicOne.playNum == 0 || !DataManager.DmUserInfo.optionData.secondScenarioSkip) ? qopd.questOne.scenarioBeforeId : "");
		if (!string.IsNullOrEmpty(text4))
		{
			SceneScenario.Args args = new SceneScenario.Args();
			args.scenarioName = text4;
			args.questId = sceneBattleArgs.questOneId;
			args.storyType = 1;
			args.nextSceneName = scene;
			args.nextSceneArgs = obj;
			scene = SceneManager.SceneName.SceneScenario;
			obj = args;
		}
		text4 = ((qopd == null || qopd.questDynamicOne == null || this.resultArgs.battleArgs.isQuestNoClear || !DataManager.DmUserInfo.optionData.secondScenarioSkip) ? qopd.questOne.scenarioAfterId : "");
		if (!string.IsNullOrEmpty(text4))
		{
			SceneScenario.Args args2 = new SceneScenario.Args();
			args2.questId = this.resultArgs.quest.staticOneData.questOne.questId;
			args2.storyType = 2;
			args2.scenarioName = text4;
			args2.nextSceneName = scene;
			args2.nextSceneArgs = obj;
			scene = SceneManager.SceneName.SceneScenario;
			obj = args2;
		}
		Singleton<SceneManager>.Instance.SetNextScene(scene, obj);
		yield break;
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x000AC474 File Offset: 0x000AA674
	private List<int> GetConvertDrawId()
	{
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.resultArgs.battleArgs.questOneId);
		List<int> list = new List<int>();
		if (DataManager.DmEvent.isRaidByMapId(questOnePackData.questMap.mapId))
		{
			HashSet<int> hashSet = new HashSet<int>(questOnePackData.questOne.EnemyDropDrawDataList.Select<MstQuestDrawItemData, int>((MstQuestDrawItemData data) => data.drawId));
			hashSet.UnionWith(questOnePackData.questOne.QuestDropDrawDataList.Select<MstQuestQuestdropItemData, int>((MstQuestQuestdropItemData data) => data.bonusDrawId));
			foreach (int num in hashSet)
			{
				int mapId = questOnePackData.questMap.mapId;
				foreach (int num2 in DataManager.DmEvent.GetConvertDrawId(DataManager.DmEvent.LastCoopInfo.EventId, mapId, num))
				{
					list.Add((num2 == 0) ? num : num2);
				}
			}
		}
		return list;
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x000AC5D0 File Offset: 0x000AA7D0
	public override bool OnCreateSceneWait()
	{
		bool flag = true;
		if (this.seLoad.MoveNext())
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x000AC5F0 File Offset: 0x000AA7F0
	public override void OnEnableScene(object args)
	{
		string text = "次のLvまで";
		string text2 = "次のなかよしLvまで";
		this.resultArgs = args as SceneBattleResultArgs;
		this.filed = this.resultArgs.resultField;
		this.resultArgs.resultField = null;
		this.chara = this.filed.transform.GetComponentInChildren<CharaModelHandle>();
		this.lucky = this.chara.modelName.IndexOf("1004") > 0;
		this.chara.PlayAnimation(this.lucky ? CharaMotionDefine.ActKey.JOY : CharaMotionDefine.ActKey.WIN_ST, false, 1f, 0.2f, 0.1f, false);
		this.chara.SetAlpha(1f);
		this.chara.DispAccessory(0, true, false);
		this.chara.DispAccessory(3, true, true);
		if (!string.IsNullOrEmpty(this.resultArgs.resultVoiceFirstSheet) && !string.IsNullOrEmpty(this.resultArgs.resultVoiceFirst))
		{
			SoundManager.PlayVoice(this.resultArgs.resultVoiceFirstSheet, this.resultArgs.resultVoiceFirst);
			this.resultArgs.resultVoiceFirst = "";
			this.resultArgs.resultVoiceSecondTime = TimeManager.SystemNow.AddSeconds((double)this.resultArgs.resultVoiceFirstLength);
		}
		PrjUtil.SendAppsFlyerLtvIdByRankup(this.resultArgs.userLevel, DataManager.DmUserInfo.level);
		if (this.resultArgs.isSkip)
		{
			CanvasManager.SetBgTexture("selbg_charaedit");
			this.chara.transform.position = new Vector3(0f, 0f, 0f);
			this.chara.transform.LookAt(new Vector3(0f, 0.5f, 0f));
			this.chara.transform.eulerAngles = new Vector3(0f, this.chara.transform.eulerAngles.y, 0f);
			this.chara.transform.Translate(new Vector3(2f, 0f, 0f));
			GameObject gameObject = new GameObject("CamPos");
			gameObject.transform.SetParent(this.chara.transform, false);
			gameObject.transform.localPosition = new Vector3(0f, 0f, 4f);
			GameObject gameObject2 = new GameObject("LokPos");
			gameObject2.transform.SetParent(this.chara.transform, false);
			gameObject2.transform.localPosition = new Vector3(-1.4f, 0f, 0f);
			FieldCameraScaler component = this.filed.transform.Find("Result Camera").GetComponent<FieldCameraScaler>();
			Vector3 position = this.chara.transform.Find("CamPos").position;
			Vector3 position2 = this.chara.transform.Find("LokPos").position;
			Vector3 haraPos = this.chara.GetHaraPos();
			position.y = (position2.y = haraPos.y);
			component.fieldOfView = 30f;
			component.transform.localPosition = position;
			component.transform.LookAt(position2);
			List<EffectData> charaEffect = this.chara.charaEffect;
			if (charaEffect != null && charaEffect.Count > 0 && charaEffect[0] != null && charaEffect[0].effectObject.layer != LayerMask.NameToLayer("Bloom"))
			{
				charaEffect[0].effectObject.SetLayerRecursively(LayerMask.NameToLayer("Bloom"));
			}
			float num = position.y - 1f;
			string text3 = Regex.Replace(this.chara.modelName, "[^0-9]", "");
			int num2 = 0;
			int.TryParse(text3, out num2);
			CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(num2);
			if ((num > 0f && num < 0.3f) || (charaStaticData != null && !charaStaticData.baseData.isFloating && num > 0.3f))
			{
				position.y = 1f;
				component.transform.localPosition = position;
			}
		}
		CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
		SoundManager.PlayBGM(this.resultArgs.bgm);
		EffectManager.BillboardCamera = this.filed.transform.GetComponentInChildren<FieldCameraScaler>().fieldCamera;
		this.basePanel.SetActive(false);
		this.backPanel.SetActive(false);
		this.guiData.BoardNorm.gameObject.SetActive(false);
		this.guiData.BoardPvp.gameObject.SetActive(false);
		this.guiData.BoardPvpTraining.gameObject.SetActive(false);
		this.guiData.BoardTraining.gameObject.SetActive(false);
		this.guiData.StampCompAE.playTime = 0f;
		this.guiData.StampCompAE.gameObject.SetActive(false);
		this.StampCompAEnd = false;
		this.StampAEnd = new List<bool>();
		foreach (AEImage aeimage in this.guiData.StampAE)
		{
			aeimage.playTime = 0f;
			aeimage.gameObject.SetActive(false);
			this.StampAEnd.Add(false);
		}
		this.guiData.MissionItemAE.PlayAnimation(PguiAECtrl.AmimeType.START, null);
		this.guiData.MissionItemAE.m_AEImage.autoPlay = false;
		this.MissionItemAEnd = false;
		this.guiData.UserIcon.Replace((DataManager.DmUserInfo.avatarType == DataManagerUserInfo.AvatarType.TYPE_A) ? 1 : 2);
		this.guiData.RankAE.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		foreach (PguiAECtrl pguiAECtrl in this.charaNorm.CharaAE)
		{
			pguiAECtrl.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		}
		foreach (PguiAECtrl pguiAECtrl2 in this.charaNorm.KizunaAE)
		{
			pguiAECtrl2.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		}
		foreach (PguiAECtrl pguiAECtrl3 in this.charaPvp.CharaAE)
		{
			pguiAECtrl3.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		}
		foreach (PguiAECtrl pguiAECtrl4 in this.charaPvp.KizunaAE)
		{
			pguiAECtrl4.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		}
		foreach (PguiAECtrl pguiAECtrl5 in this.charaPvpTraining.CharaAE)
		{
			pguiAECtrl5.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		}
		foreach (PguiAECtrl pguiAECtrl6 in this.charaPvpTraining.KizunaAE)
		{
			pguiAECtrl6.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		}
		foreach (PguiAECtrl pguiAECtrl7 in this.charaTraining.CharaAE)
		{
			pguiAECtrl7.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		}
		foreach (PguiAECtrl pguiAECtrl8 in this.charaTraining.KizunaAE)
		{
			pguiAECtrl8.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		}
		this.requestNextScene = false;
		this.requestRematch = null;
		this.waitSequence = null;
		this.rematch = 1;
		if (this.resultArgs.debug || this.resultArgs.restart)
		{
			this.rematch = 0;
		}
		else if (this.resultArgs.battleArgs.trainingTurn > 0)
		{
			this.rematch = 0;
		}
		else if (this.resultArgs.pvpDeck != null)
		{
			this.rematch = 0;
		}
		else if (this.resultArgs.battleArgs.tutorialSequence != TutorialUtil.Sequence.INVALID)
		{
			this.rematch = 0;
		}
		else if (DataManager.DmQuest.CalcRestPlayNumByQuestOneId(this.resultArgs.battleArgs.questOneId) == 0)
		{
			this.rematch = 0;
		}
		else if (this.IsChangeTerm())
		{
			this.rematch = 0;
		}
		else
		{
			DataManagerMonthlyPack.UserPackData nowPackData = DataManager.DmMonthlyPack.nowPackData;
			if (nowPackData == null || nowPackData.MonthlypackData == null)
			{
				this.rematch = 0;
			}
			else if (!nowPackData.MonthlypackData.BattleRetryFlag)
			{
				this.rematch = 0;
			}
			else
			{
				DateTime dateTime = new DateTime(TimeManager.Now.Year, TimeManager.Now.Month, TimeManager.Now.Day);
				if ((new DateTime(nowPackData.EndDatetime.Year, nowPackData.EndDatetime.Month, DataManager.DmMonthlyPack.nowPackData.EndDatetime.Day) - dateTime).Days < 0)
				{
					this.rematch = 0;
				}
				else
				{
					DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.resultArgs.battleArgs.eventId);
					if (eventData != null && eventData.eventCategory == DataManagerEvent.Category.Coop && this.resultArgs.quest.staticOneData.questGroup.QuestGroupCategory == QuestStaticQuestGroup.GroupCategory.CoopDifficult)
					{
						this.rematch = 0;
					}
				}
			}
		}
		this.rematchNeed = this.resultArgs.quest.staticOneData.questOne.stamina;
		this.rematchOwn = -1;
		if (this.resultArgs.quest.staticOneData.questOne.useItemId > 0)
		{
			this.rematchNeed = this.resultArgs.quest.staticOneData.questOne.useItemNum;
			ItemData userItemData = DataManager.DmItem.GetUserItemData(this.resultArgs.quest.staticOneData.questOne.useItemId);
			if ((this.rematchOwn = userItemData.num) < 0)
			{
				this.rematchOwn = 0;
			}
			this.rematchItem = userItemData.staticData.GetName();
			this.guiData.RematchBtn.transform.Find("BaseImage/Inbase/Icon_Item").GetComponent<PguiRawImageCtrl>().SetRawImage(userItemData.staticData.GetIconName(), true, false, null);
		}
		else
		{
			int num3 = -1;
			foreach (DataManagerCampaign.CampaignQuestStaminaData campaignQuestStaminaData in DataManager.DmCampaign.PresentCampaignQuestStaminaDataList)
			{
				if (campaignQuestStaminaData.value >= 0 && campaignQuestStaminaData.campaignTargetList.Find((DataManagerCampaign.CampaignTarget itm) => itm.TargetType == DataManagerCampaign.TARGET_TYPE.Chapter && itm.TargetId == this.resultArgs.quest.staticOneData.questChapter.chapterId) != null)
				{
					num3 = campaignQuestStaminaData.value;
					break;
				}
			}
			if (num3 >= 0 && (this.rematchNeed = num3 - this.rematchNeed - 1) >= 0)
			{
				this.rematchNeed = -1;
			}
		}
		this.guiData.RematchBtn.transform.Find("BaseImage/Inbase/Icon_Item").gameObject.SetActive(this.rematchOwn >= 0);
		this.guiData.RematchBtn.transform.Find("BaseImage/Inbase/Txt_Stamina").gameObject.SetActive(this.rematchOwn < 0);
		this.page = 0;
		this.touch = false;
		this.rankup = 0;
		this.levelup = 0;
		this.friendFollow = null;
		this.missionIcon.Setup(this.resultArgs.quest.battleMissionPack.staticData.completeBonus, new IconItemCtrl.SetupParam
		{
			useInfo = true
		});
		this.guiData.Quest.text = this.resultArgs.quest.staticOneData.questOne.questName;
		this.guiData.QuestNum.text = this.resultArgs.quest.staticOneData.questGroup.storyName;
		this.guiData.Turn.text = "クリアターン数 <size=28>" + this.resultArgs.clearTurn.ToString() + "</size>";
		if (this.resultArgs.battleArgs.trainingTurn > 0)
		{
			int num4 = 1;
			if (this.resultArgs.trainingScore >= 10000000L)
			{
				num4 = 6;
			}
			else if (this.resultArgs.trainingScore >= 1000000L)
			{
				num4 = 5;
			}
			else if (this.resultArgs.trainingScore >= 500000L)
			{
				num4 = 4;
			}
			else if (this.resultArgs.trainingScore >= 100000L)
			{
				num4 = 3;
			}
			else if (this.resultArgs.trainingScore >= 50000L)
			{
				num4 = 2;
			}
			int num5 = 1;
			for (;;)
			{
				Transform transform = this.guiData.TrainingScore.transform.Find("Num_Score" + num5.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				transform.gameObject.SetActive(num4 == num5);
				transform.GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", this.resultArgs.trainingScore.ToString());
				num5++;
			}
			this.guiData.TrainingNewRecord.gameObject.SetActive(this.resultArgs.trainingScore > this.resultArgs.battleArgs.trainingScore);
			this.guiData.TrainingNewRecord.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
		}
		else if (this.resultArgs.pvpDeck == null)
		{
			this.rankBefore = this.resultArgs.userLevel;
			this.rankAfter = DataManager.DmUserInfo.level;
			this.expBefore = this.resultArgs.userExp;
			this.expAfter = DataManager.DmUserInfo.exp;
			this.guiData.Rank.text = this.rankBefore.ToString();
			this.expBase = DataManager.DmUserInfo.GetExpByNextLevel(this.rankBefore);
			if (this.resultArgs.debug && this.rankBefore > 1 && this.expBase > 0L)
			{
				SceneBattleResultArgs sceneBattleResultArgs = this.resultArgs;
				int num6 = this.rankBefore - 1;
				this.rankBefore = num6;
				sceneBattleResultArgs.userLevel = num6;
				this.expBase = DataManager.DmUserInfo.GetExpByNextLevel(this.rankBefore);
				this.expBefore = this.expBase - (this.expAfter = (long)(this.resultArgs.quest.staticOneData.questOne.userExp / 2));
			}
			long num7 = this.expAfter - this.expBefore;
			for (int i = this.rankBefore; i < this.rankAfter; i++)
			{
				num7 += DataManager.DmUserInfo.GetExpByNextLevel(i);
			}
			this.guiData.Exp.Find("Num_GetExp").GetComponent<PguiTextCtrl>().text = ((num7 > 0L) ? ("+" + num7.ToString()) : "");
			this.guiData.ExpGage.fillAmount = ((this.expBase > 0L) ? ((float)((double)this.expBefore / (double)this.expBase)) : 1f);
			this.rankWinBefore.ReplaceTextByDefault("Param01", this.rankBefore.ToString());
			this.rankWinAfter.ReplaceTextByDefault("Param01", this.rankAfter.ToString());
			int num8 = Mathf.Clamp(this.rankBefore - 1, 0, DataManager.DmServerMst.gameLevelInfoList.Count - 1);
			num8 = DataManager.DmServerMst.gameLevelInfoList[num8].staminaLimit + DataManager.DmTreeHouse.StaminaBonusData.staminaBonus;
			int num9 = Mathf.Clamp(this.rankAfter - 1, 0, DataManager.DmServerMst.gameLevelInfoList.Count - 1);
			num9 = DataManager.DmServerMst.gameLevelInfoList[num9].staminaLimit + DataManager.DmTreeHouse.StaminaBonusData.staminaBonus;
			this.rankupWindow.transform.Find("Base/Window/Massage_stamina01").gameObject.SetActive(true);
			PguiTextCtrl component2 = this.rankupWindow.transform.Find("Base/Window/Massage_stamina02").GetComponent<PguiTextCtrl>();
			component2.ReplaceTextByDefault("Param01", num9.ToString());
			component2.gameObject.SetActive(num9 > num8);
			if (this.rankAfter > this.rankBefore)
			{
				this.rankup = 1;
			}
		}
		else
		{
			this.pvpResult = DataManager.DmPvp.GetLastPvPEndResult();
			PvpPackData pvpPackData = DataManager.DmPvp.GetPvpPackDataBySeasonID(this.resultArgs.battleArgs.pvpSeasonId);
			PvpRankInfo pvpRankInfo = ((pvpPackData == null) ? null : pvpPackData.GetPvpRankInfoByPoint(this.pvpResult.befPvpPoint));
			PvpRankInfo pvpRankInfo2 = ((pvpPackData == null) ? null : pvpPackData.GetPvpRankInfoByPoint(this.pvpResult.nowPvpPoint));
			this.rankup = ((pvpRankInfo == pvpRankInfo2) ? 0 : 1);
			if (this.resultArgs.debug)
			{
				this.pvpResult = new DataManagerPvp.PvPEndResult
				{
					befPvpPoint = 10,
					nowPvpPoint = 50,
					calcPointByBase = 30,
					calcPointByWinning = 10,
					calcPointByTurn = 1.3f,
					calcPointByDifficulty = 1.1f,
					getPvpCoin = 100
				};
				this.rankup = 1;
			}
			SceneBattleResult.GUI.PvpExp pvpExp = ((this.resultArgs.battleArgs.pvpTraining > 0) ? this.guiData.pvpExpTraining : this.guiData.pvpExp);
			pvpExp.pvpRank.text = ((pvpRankInfo2 == null) ? "" : pvpRankInfo2.rankName);
			pvpExp.pvpPoint.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				this.pvpResult.nowPvpPoint.ToString(),
				"+" + this.pvpResult.getPvpPoint.ToString()
			});
			pvpExp.pvpPointNext.text = ((pvpRankInfo2 == null || pvpRankInfo2.nexRankInfo == null) ? "" : ("次のクラスまで<size=24>" + (pvpRankInfo2.nexRankInfo.pointRangeLow - this.pvpResult.nowPvpPoint).ToString() + "</size>Pt"));
			pvpExp.pvpPointBase.text = this.pvpResult.calcPointByBase.ToString() + "Pt";
			pvpExp.pvpPointBonus.text = this.pvpResult.calcPointByWinning.ToString() + "Pt";
			pvpExp.pvpPointTurn.text = "×" + this.pvpResult.calcPointByTurn.ToString();
			pvpExp.pvpPointVs.text = "×" + this.pvpResult.calcPointByDifficulty.ToString();
			this.guiData.pvpPoint3x.SetActive(this.resultArgs.battleArgs.pvp3x);
			this.guiData.pvpPoint3xTraining.SetActive(this.resultArgs.battleArgs.pvp3x);
			PguiTextCtrl pguiTextCtrl = ((this.resultArgs.battleArgs.pvpTraining > 0) ? this.guiData.pvpCoinTraining : this.guiData.pvpCoin);
			pguiTextCtrl.text = this.pvpResult.getPvpCoin.ToString();
			pguiTextCtrl.GetComponent<PguiGradientCtrl>().SetGameObjectById(((this.resultArgs.battleArgs.pvpTraining > 0) ? (this.pvpResult.bonusAddCoin > 0) : (this.pvpResult.campaignAddCoin > 0)) ? "CAMPAIGN" : "NORMAL");
			if (pvpPackData == null)
			{
				pvpPackData = PvpPackData.MakeDummy(1);
			}
			ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(pvpPackData.staticData.rewardItemId);
			GameObject gameObject3 = pguiTextCtrl.transform.Find("Icon_PvPCoin").gameObject;
			gameObject3.SetActive(itemStaticBase != null);
			if (itemStaticBase != null)
			{
				gameObject3.GetComponent<PguiRawImageCtrl>().SetRawImage(itemStaticBase.GetIconName(), true, false, null);
			}
			this.guiData.pvpCoin3x.SetActive(this.resultArgs.battleArgs.pvp3x);
			this.guiData.pvpCoin3xTraining.SetActive(this.resultArgs.battleArgs.pvp3x);
			this.guiData.pvpCoinChamp.SetActive(this.resultArgs.battleArgs.difficulty == PvpDynamicData.EnemyInfo.Difficulty.CHAMPION);
			this.rankBefore = (this.rankAfter = 0);
			this.expBefore = (this.expAfter = (this.expBase = 0L));
			if (this.resultArgs.battleEndStatus == DataManagerQuest.BattleEndStatus.CLEAR && this.resultArgs.battleArgs.isSeasonReplacement)
			{
				CanvasManager.HdlOpenWindowBasic.Setup("確認", "シーズンが終了したため\nバトル報酬は受け取れません", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
			}
		}
		this.pvpRankUp = null;
		this.kizunaWindow.SetActive(false);
		this.kizunaWinChara = AssetManager.InstantiateAssetData("RenderTextureChara/Prefab/RenderTextureCharaCtrl", this.kizunaWindow.transform.Find("RenderTexture")).GetComponent<RenderTextureChara>();
		this.kizunaWinChara.SetupRenderTexture(1654, 1024);
		this.charaId = new List<CharaPackData>();
		this.levelAfter = new List<int>();
		this.levelBefore = new List<int>();
		this.levelMax = new List<int>();
		this.lvexpAfter = new List<long>();
		this.lvexpBefore = new List<long>();
		this.lvexpBase = new List<long>();
		this.kizunaId = new List<CharaPackData>();
		this.kizunaBefore = new List<int>();
		this.kizunaAfter = new List<int>();
		this.kizunaMax = new List<int>();
		this.kizunaExpBefore = new List<long>();
		this.kizunaExpAfter = new List<long>();
		this.kizunaExpBase = new List<long>();
		int j = 0;
		SceneBattleResult.GUIChara guichara = ((this.resultArgs.battleArgs.trainingTurn > 0) ? this.charaTraining : ((this.resultArgs.pvpDeck == null) ? this.charaNorm : ((this.resultArgs.battleArgs.pvpTraining > 0) ? this.charaPvpTraining : this.charaPvp)));
		while (j < this.resultArgs.deck.deckData.Count)
		{
			CharaPackData charaPackData = this.resultArgs.deck.deckData[j];
			if (j >= guichara.Chara.Count)
			{
				IL_2003:
				while (j < guichara.Chara.Count)
				{
					guichara.Chara[j++].SetActive(false);
				}
				for (int k = 0; k < guichara.Blank.Count; k++)
				{
					guichara.Blank[k].SetActive(k >= guichara.Chara.Count || !guichara.Chara[k].activeSelf);
				}
				this.kizunaUp = 0;
				List<DataManagerQuest.QuestDropData> list = new List<DataManagerQuest.QuestDropData>();
				if (this.resultArgs.battleArgs.trainingTurn > 0)
				{
					List<ItemInput> list2 = ((DataManager.DmTraining.LastTrainingEndResponse == null || DataManager.DmTraining.LastTrainingEndResponse.rewardData == null || this.resultArgs.battleArgs.isPractice) ? null : DataManager.DmTraining.LastTrainingEndResponse.rewardData.rewardItemList);
					if (list2 == null)
					{
						list2 = new List<ItemInput>();
					}
					for (int l = 0; l < this.rewardIcon.Count; l++)
					{
						if (l >= list2.Count)
						{
							this.guiData.RewardItem[l].SetActive(false);
						}
						else
						{
							this.guiData.RewardItem[l].SetActive(true);
							this.rewardIcon[l].Setup(new ItemData(list2[l].itemId, list2[l].num), new IconItemCtrl.SetupParam
							{
								useInfo = true
							});
						}
					}
				}
				else if (this.resultArgs.pvpDeck == null)
				{
					if (this.resultArgs.debug || (DataManager.DmQuest.LastQuestEndResponse == null && !this.resultArgs.isSkip))
					{
						int num10 = 0;
						foreach (SceneBattle_WavePackData sceneBattle_WavePackData in this.resultArgs.quest.wavePackDataList)
						{
							foreach (ItemData itemData in sceneBattle_WavePackData.dropItemList)
							{
								if (itemData != null)
								{
									int num11 = (this.resultArgs.debug ? Random.Range(0, 3) : 0);
									int num12 = (this.resultArgs.debug ? Random.Range(0, 3) : 0);
									List<DataManagerQuest.QuestDropData> list3 = list;
									DrewItem drewItem = new DrewItem();
									drewItem.item_id = itemData.id;
									drewItem.item_num = itemData.num + num11 + num12;
									drewItem.photobonus_num = num11;
									drewItem.campaignbonus_num = num12;
									num10 = (drewItem.enemy_id = num10 + 1);
									drewItem.drop_type = 1;
									list3.Add(new DataManagerQuest.QuestDropData(drewItem));
								}
							}
						}
						if (this.resultArgs.debug && list.Count > 0)
						{
							DataManagerQuest.QuestDropData questDropData = list[0];
							list.Add(new DataManagerQuest.QuestDropData(new DrewItem
							{
								item_id = questDropData.ItemId,
								item_num = questDropData.ItemNum,
								photobonus_num = questDropData.PhotoBonusNum,
								campaignbonus_num = questDropData.CampaignBonusNum,
								enemy_id = 0,
								drop_type = 2
							}));
							list.Add(new DataManagerQuest.QuestDropData(new DrewItem
							{
								item_id = questDropData.ItemId,
								item_num = questDropData.ItemNum,
								photobonus_num = questDropData.PhotoBonusNum,
								campaignbonus_num = questDropData.CampaignBonusNum,
								enemy_id = 1,
								drop_type = 3
							}));
						}
					}
					else
					{
						if (this.resultArgs.isSkip)
						{
							using (List<DrewItem>.Enumerator enumerator6 = this.resultArgs.battleArgs.dropItemList.GetEnumerator())
							{
								while (enumerator6.MoveNext())
								{
									DrewItem drewItem2 = enumerator6.Current;
									if (drewItem2 == null)
									{
										return;
									}
									list.Add(new DataManagerQuest.QuestDropData(drewItem2));
								}
								goto IL_243B;
							}
						}
						foreach (DataManagerQuest.QuestDropData questDropData2 in DataManager.DmQuest.LastQuestEndResponse.DropItemDataList)
						{
							if (questDropData2 == null)
							{
								return;
							}
							list.Add(questDropData2);
						}
					}
				}
				IL_243B:
				list.Sort(new Comparison<DataManagerQuest.QuestDropData>(this.SortDropItem));
				int m = 0;
				this.dropIcon = new List<IconItemCtrl>();
				foreach (DataManagerQuest.QuestDropData questDropData3 in list)
				{
					if (m >= this.guiData.DropItem.Count)
					{
						break;
					}
					ItemData itemData2 = new ItemData(questDropData3.ItemId, questDropData3.ItemNum - questDropData3.PhotoBonusNum);
					this.guiData.DropItem[m].SetActive(true);
					this.guiData.DropItemBonus[m].text = ((questDropData3.PhotoBonusNum > 0) ? ("+" + questDropData3.PhotoBonusNum.ToString()) : "");
					this.guiData.DropItemBox[m].Replace((int)itemData2.staticData.GetRarity());
					this.guiData.DropItemAE[m].PlayAnimation(PguiAECtrl.AmimeType.START, null);
					this.guiData.DropItemAE[m].m_AEImage.autoPlay = false;
					IconItemCtrl component3 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item).GetComponent<IconItemCtrl>();
					component3.transform.SetParent(this.guiData.DropItem[m].transform.Find("Icon_Item"), false);
					component3.Setup(itemData2, new IconItemCtrl.SetupParam
					{
						useInfo = true,
						isCampaign = (questDropData3.CampaignBonusNum > 0),
						gentei = (questDropData3.Type == DataManagerQuest.QuestDropData.DropType.Term && !DataManager.DmEvent.isRaidByEventId(this.resultArgs.battleArgs.eventId)),
						photoDrop = (questDropData3.Type == DataManagerQuest.QuestDropData.DropType.PhotoMine || questDropData3.Type == DataManagerQuest.QuestDropData.DropType.PhotoHelper)
					});
					this.dropIcon.Add(component3);
					m++;
				}
				if (m < 11)
				{
					this.guiData.ScrollContent.sizeDelta = new Vector2(this.guiData.ScrollContent.sizeDelta.x, 0f);
				}
				else if (m < 16)
				{
					this.guiData.ScrollContent.sizeDelta = new Vector2(this.guiData.ScrollContent.sizeDelta.x, 400f);
				}
				else
				{
					this.guiData.ScrollContent.sizeDelta = new Vector2(this.guiData.ScrollContent.sizeDelta.x, 530f);
				}
				this.guiData.ScrollContent.anchoredPosition = new Vector2(this.guiData.ScrollContent.anchoredPosition.x, 0f);
				while (m < this.guiData.DropItem.Count)
				{
					this.guiData.DropItem[m++].SetActive(false);
				}
				this.gold = this.resultArgs.quest.staticOneData.questOne.goldNum * this.resultArgs.tryCount;
				this.guiData.getCoin.text = this.gold.ToString();
				this.goldAfter = DataManager.DmItem.GetUserItemData(30101).num;
				this.goldBefore = this.resultArgs.haveGoldNum;
				this.guiData.ownCoin.text = this.goldBefore.ToString();
				this.missionOld = new List<bool>(this.resultArgs.quest.battleMissionPack.clearFlag);
				while (this.missionOld.Count < this.guiData.StampAE.Count)
				{
					this.missionOld.Add(false);
				}
				this.missionNew = new List<bool>(this.resultArgs.battleMissionStatus);
				while (this.missionNew.Count < this.guiData.StampAE.Count)
				{
					this.missionNew.Add(false);
				}
				this.guiData.Rematch.gameObject.SetActive(false);
				this.guiData.SkipGroup.gameObject.SetActive(false);
				if (this.resultArgs.quest.staticOneData.questOne.questId > 0)
				{
					BattleMissionPack battleMissionPack = DataManager.DmQuest.GetBattleMissionPack(this.resultArgs.quest.staticOneData.questOne.questId);
					DataManagerMonthlyPack.PurchaseMonthlypackData validMonthlyPackData = DataManager.DmMonthlyPack.GetValidMonthlyPackData();
					this.skipInfo = new QuestUtil.UsrQuestSkipInfo();
					if (DataManager.DmQuest.CalcRestPlayNumByQuestOneId(this.resultArgs.battleArgs.questOneId) != 0 && (this.resultArgs.quest.staticOneData.questOne.skippableFlag != QuestUtil.SkipType.Disable || this.resultArgs.quest.staticOneData.questGroup.SkippableFlag != QuestUtil.SkipType.Disable) && !battleMissionPack.clearFlag.Contains(false))
					{
						this.skipInfo = QuestUtil.GetSkipInfo(validMonthlyPackData, this.resultArgs.quest.staticOneData);
						if (this.guiData.SkipBtn != null && this.skipInfo.isSkippable && (!this.skipInfo.hasSkipLimit || this.skipInfo.restSkipCount > 0 || this.skipInfo.restSkipRecoveryCount > 0))
						{
							this.guiData.SkipBtn.AddOnClickListener(delegate(PguiButtonCtrl button)
							{
								this.skipCount = -1;
								CanvasManager.HdlQuestSkipWindowsCtrl.SetReturnSkipCountAction(delegate(int action)
								{
									this.skipCount = action;
								});
								CanvasManager.HdlQuestSkipWindowsCtrl.InitializeWindow(this.resultArgs.quest.staticOneData.questOne.questId, this.resultArgs.helper, this.resultArgs.battleArgs.attrIndex, this.resultArgs.battleArgs.selectDeckId, 0);
							}, PguiButtonCtrl.SoundType.DECIDE);
						}
					}
				}
				return;
			}
			guichara.Chara[j].SetActive(charaPackData != null);
			bool flag = this.resultArgs.battleArgs.trainingTurn <= 0 && this.resultArgs.pvpDeck == null && j == this.resultArgs.deck.deckHelperIndex;
			if (guichara.Helper[j] != null)
			{
				guichara.Helper[j].gameObject.SetActive(flag);
			}
			guichara.CharaExp[j].transform.parent.gameObject.SetActive(!flag);
			long num13 = 0L;
			int id = 0;
			int num14 = 0;
			int num15 = 0;
			int num16 = 0;
			long num17 = 0L;
			long num18 = 0L;
			long num19 = 0L;
			if (charaPackData != null)
			{
				id = charaPackData.id;
				num15 = this.resultArgs.charaLevel[j];
				num14 = charaPackData.dynamicData.level;
				num16 = charaPackData.dynamicData.limitLevel;
				num18 = this.resultArgs.charaExp[j];
				num17 = charaPackData.dynamicData.exp;
				num19 = ((num15 < num16) ? DataManager.DmChara.GetExpByNextLevel(id, num15) : 0L);
				if (this.resultArgs.debug && num15 > 1 && num19 > 0L && !flag && num15 < num16 && this.resultArgs.battleArgs.trainingTurn <= 0)
				{
					num15 = (this.resultArgs.charaLevel[j] = num15 - 1);
					num19 = DataManager.DmChara.GetExpByNextLevel(id, num15);
					num18 = num19 - (num17 = (long)(this.resultArgs.quest.staticOneData.questOne.charaExp / 2));
				}
				num13 = num17 - num18;
				for (int n = num15; n < num14; n++)
				{
					num13 += DataManager.DmChara.GetExpByNextLevel(id, n);
				}
				long num20 = ((charaPackData.dynamicData.level >= charaPackData.dynamicData.limitLevel) ? 0L : (DataManager.DmChara.GetExpByNextLevel(charaPackData.id, charaPackData.dynamicData.level) - charaPackData.dynamicData.exp));
				long num21 = ((charaPackData.dynamicData.kizunaLevel >= charaPackData.dynamicData.KizunaLimitLevel) ? 0L : (DataManager.DmChara.GetKizunaExpForNextLevel(charaPackData.id, charaPackData.dynamicData.kizunaLevel) - charaPackData.dynamicData.kizunaExp));
				guichara.LeftExp[j].text = "<size=11>" + text + num20.ToString() + "</size>";
				guichara.LeftExp[j].gameObject.SetActive(false);
				guichara.LeftKizunaExp[j].text = "<size=11>" + text2 + num21.ToString() + "</size>";
				guichara.LeftKizunaExp[j].gameObject.SetActive(false);
			}
			guichara.CharaExp[j].text = ((num13 > 0L) ? ("+" + num13.ToString()) : "");
			guichara.CharaExp[j].gameObject.SetActive(false);
			guichara.CharaLv[j].text = "Lv." + num15.ToString();
			guichara.CharaExpGage[j].fillAmount = ((num19 > 0L) ? ((float)((double)num18 / (double)num19)) : 1f);
			guichara.LevelMax[j].playTime = (guichara.LevelMax[j].playInTime = 0f);
			guichara.LevelMax[j].playOutTime = ((num19 > 0L) ? 0f : guichara.LevelMax[j].duration);
			guichara.LevelMax[j].autoPlay = false;
			this.charaId.Add(charaPackData);
			this.levelAfter.Add(num14);
			this.levelBefore.Add(num15);
			this.levelMax.Add(num16);
			this.lvexpAfter.Add(num17);
			this.lvexpBefore.Add(num18);
			this.lvexpBase.Add(num19);
			guichara.CharaIcon[j].Setup(charaPackData, SortFilterDefine.SortType.LEVEL, false, null, flag ? 0 : this.resultArgs.battleArgs.eventId, -1, this.resultArgs.battleArgs.questOneId);
			guichara.CharaIcon[j].SetupLevel(num15);
			if (this.resultArgs.battleArgs.trainingTurn > 0 || (this.resultArgs.pvpDeck != null && this.resultArgs.battleArgs.eventId == 0))
			{
				guichara.CharaIcon[j].DispMarkEvent(false, false, false);
			}
			PguiGradientCtrl pguiGradientCtrl = (flag ? null : guichara.CharaExp[j].GetComponent<PguiGradientCtrl>());
			if (pguiGradientCtrl != null)
			{
				pguiGradientCtrl.SetGameObjectById(guichara.CharaIcon[j].IsDispMarkExpUp() ? "EXP" : "NORMAL");
			}
			guichara.CharaHrt[j].transform.parent.gameObject.SetActive(!flag);
			num13 = 0L;
			id = 0;
			num15 = (num14 = (num16 = 0));
			num18 = (num17 = (num19 = 0L));
			if (charaPackData != null)
			{
				id = charaPackData.id;
				num15 = this.resultArgs.kizunaLevel[j];
				num14 = charaPackData.dynamicData.kizunaLevel;
				num16 = charaPackData.staticData.baseData.maxKizunaLevel + charaPackData.dynamicData.kizunaLimitOverNum;
				num18 = this.resultArgs.kizunaExp[j];
				num17 = charaPackData.dynamicData.kizunaExp;
				num19 = ((num14 < num16) ? DataManager.DmChara.GetKizunaExpForNextLevel(id, num15) : 0L);
				if (this.resultArgs.debug && num15 > 1 && !flag && this.resultArgs.battleArgs.trainingTurn <= 0)
				{
					num15 = (this.resultArgs.kizunaLevel[j] = num15 - 1);
					num19 = DataManager.DmChara.GetKizunaExpForNextLevel(id, num15);
					num13 = (long)this.resultArgs.quest.staticOneData.questOne.kizunaExp;
					if (charaPackData != null && charaPackData.id == this.resultArgs.quest.staticOneData.questOne.kizunabonusCharaId)
					{
						num13 = (num13 * (long)(100 + this.resultArgs.quest.staticOneData.questOne.kizunabonusRatio) + 99L) / 100L;
					}
					num18 = num19 - (num17 = num13 / 2L);
				}
				num13 = num17 - num18;
				for (int num22 = num15; num22 < num14; num22++)
				{
					num13 += DataManager.DmChara.GetKizunaExpForNextLevel(id, num22);
				}
			}
			guichara.CharaHrt[j].text = ((num13 > 0L) ? ("+" + num13.ToString()) : "");
			string text4 = "NORMAL";
			List<KizunaBonus> list4;
			if (this.resultArgs.battleArgs.trainingTurn > 0)
			{
				list4 = null;
			}
			else if (this.resultArgs.pvpDeck == null)
			{
				list4 = ((DataManager.DmQuest.LastQuestEndResponse == null) ? null : DataManager.DmQuest.LastQuestEndResponse.KizunaBonus);
			}
			else
			{
				list4 = ((DataManager.DmPvp.GetLastPvPEndResult() == null) ? null : DataManager.DmPvp.GetLastPvPEndResult().KizunaBonus);
			}
			if (list4 != null)
			{
				if (list4.Find((KizunaBonus itm) => itm.chara_id == id && itm.chara_bonus_point > 0) != null)
				{
					text4 = "EVENT";
				}
				else if (list4.Find((KizunaBonus itm) => itm.chara_id == id && itm.bonus_point > 0) != null)
				{
					text4 = "CAMPAIGN";
				}
			}
			guichara.CharaHrt[j].m_Text.color = guichara.CharaHrt[j].GetComponent<PguiColorCtrl>().GetGameObjectById(text4);
			guichara.CharaHrt[j].gameObject.SetActive(false);
			guichara.CharaKz[j].text = "なかよしLv." + num15.ToString();
			guichara.CharaHrtGage[j].fillAmount = ((num19 > 0L) ? ((float)((double)num18 / (double)num19)) : 1f);
			guichara.KizunaMax[j].playTime = (guichara.KizunaMax[j].playInTime = 0f);
			guichara.KizunaMax[j].playOutTime = ((num19 > 0L) ? 0f : guichara.KizunaMax[j].duration);
			guichara.KizunaMax[j].autoPlay = false;
			this.kizunaId.Add(charaPackData);
			this.kizunaAfter.Add(num14);
			this.kizunaBefore.Add(num15);
			this.kizunaMax.Add(num16);
			this.kizunaExpAfter.Add(num17);
			this.kizunaExpBefore.Add(num18);
			this.kizunaExpBase.Add(num19);
			j++;
		}
		goto IL_2003;
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x000AF13C File Offset: 0x000AD33C
	private int SortDropItem(DataManagerQuest.QuestDropData a, DataManagerQuest.QuestDropData b)
	{
		int num = (int)a.Type;
		if (num >= 3)
		{
			num = 7 - num;
		}
		int num2 = (int)b.Type;
		if (num2 >= 3)
		{
			num2 = 7 - num2;
		}
		int num3 = num2 - num;
		if (num3 == 0)
		{
			num3 = a.EnemyId - b.EnemyId;
		}
		return num3;
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x000AF17D File Offset: 0x000AD37D
	public override bool OnEnableSceneWait()
	{
		if (!this.basePanel.activeSelf)
		{
			this.basePanel.SetActive(true);
			this.backPanel.SetActive(true);
			return false;
		}
		return true;
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x000AF1A7 File Offset: 0x000AD3A7
	public override void OnStartControl()
	{
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x000AF1AC File Offset: 0x000AD3AC
	public override void Update()
	{
		if (!this.chara.IsPlaying())
		{
			this.chara.PlayAnimation(this.lucky ? CharaMotionDefine.ActKey.POSITIVE : CharaMotionDefine.ActKey.WIN_LP, true, 1f, 0f, 0f, false);
		}
		if (!string.IsNullOrEmpty(this.resultArgs.resultVoiceSecondSheet) && !string.IsNullOrEmpty(this.resultArgs.resultVoiceSecond) && this.resultArgs.resultVoiceSecondTime <= TimeManager.SystemNow)
		{
			SoundManager.PlayVoice(this.resultArgs.resultVoiceSecondSheet, this.resultArgs.resultVoiceSecond);
			this.resultArgs.resultVoiceSecond = "";
		}
		if (this.resultArgs.battleArgs.trainingTurn > 0)
		{
			this.UpdateTraining();
		}
		else if (this.resultArgs.pvpDeck == null)
		{
			this.UpdateNorm();
		}
		else
		{
			this.UpdatePvp();
		}
		this.touch = false;
		if (this.checkKizunaLimitReached == null && this.skipCount != -1)
		{
			List<int> list = new List<int>();
			foreach (CharaPackData charaPackData in this.resultArgs.deck.deckData)
			{
				if (charaPackData != null && charaPackData.dynamicData.OwnerType == CharaDynamicData.CharaOwnerType.User)
				{
					list.Add(charaPackData.id);
				}
			}
			this.checkKizunaLimitReached = QuestUtil.NoticeKizunaLimitReached(delegate
			{
				if (!DataManager.IsServerRequesting())
				{
					CanvasManager.HdlQuestSkipWindowsCtrl.RequestExecQuestSkip();
					this.checkKizunaLimitReached = null;
				}
			}, this.resultArgs.battleArgs.questOneId, list, this.skipCount, delegate(int action)
			{
				this.skipCount = action;
			}, 0, 0, false);
		}
		if (this.resultArgs.battleArgs.trainingTurn > 0 && this.checkPracticeConfirm != null)
		{
			if (!this.checkPracticeConfirm.MoveNext())
			{
				this.checkPracticeConfirm = null;
				return;
			}
		}
		else if (this.checkKizunaLimitReached != null)
		{
			if (!this.checkKizunaLimitReached.MoveNext())
			{
				this.checkKizunaLimitReached = null;
				return;
			}
		}
		else if (this.requestRematch != null)
		{
			if (!this.requestRematch.MoveNext())
			{
				this.requestRematch = null;
				return;
			}
		}
		else if (this.requestNextScene)
		{
			string text = "";
			if (this.resultArgs.battleArgs.trainingTurn <= 0 && this.resultArgs.pvpDeck == null && (this.resultArgs.battleArgs.isQuestNoClear || !DataManager.DmUserInfo.optionData.secondScenarioSkip))
			{
				text = DataManager.DmQuest.QuestStaticData.oneDataMap[this.resultArgs.quest.staticOneData.questOne.questId].scenarioAfterId;
			}
			if (this.resultArgs.debug)
			{
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneBattle, this.resultArgs.battleArgs);
				return;
			}
			if (this.resultArgs.restart)
			{
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneHome, null);
				return;
			}
			if (this.resultArgs.battleArgs.resultNextSceneName != SceneManager.SceneName.None)
			{
				if (text == string.Empty)
				{
					Singleton<SceneManager>.Instance.SetNextScene(this.resultArgs.battleArgs.resultNextSceneName, this.resultArgs.battleArgs.resultNextSceneArgs);
					return;
				}
				SceneScenario.Args args = new SceneScenario.Args();
				args.questId = this.resultArgs.quest.staticOneData.questOne.questId;
				args.storyType = 2;
				args.scenarioName = text;
				args.nextSceneName = this.resultArgs.battleArgs.resultNextSceneName;
				args.nextSceneArgs = this.resultArgs.battleArgs.resultNextSceneArgs;
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneScenario, args);
				return;
			}
			else if (this.resultArgs.battleArgs.trainingTurn > 0)
			{
				SceneTraining.Args args2 = new SceneTraining.Args();
				if (text == string.Empty)
				{
					Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneTraining, args2);
					return;
				}
				SceneScenario.Args args3 = new SceneScenario.Args();
				args3.questId = this.resultArgs.quest.staticOneData.questOne.questId;
				args3.storyType = 2;
				args3.scenarioName = text;
				args3.nextSceneName = SceneManager.SceneName.SceneTraining;
				args3.nextSceneArgs = args2;
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneScenario, args3);
				return;
			}
			else if (DataManager.DmEvent.isRaidByQuestOneId(this.resultArgs.battleArgs.questOneId) && DataManager.DmEvent.GetNowTermData(this.resultArgs.battleArgs.eventId) != null && this.IsChangeTerm())
			{
				SceneQuest.Args args4 = this.CreateSceneQuestArgs();
				if (!(text == string.Empty))
				{
					SceneScenario.Args args5 = new SceneScenario.Args();
					args5.questId = this.resultArgs.quest.staticOneData.questOne.questId;
					args5.storyType = 2;
					args5.scenarioName = text;
					args5.nextSceneName = SceneManager.SceneName.SceneQuest;
					args5.nextSceneArgs = args4;
					Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneScenario, args5);
					return;
				}
				if (this.waitSequence == null)
				{
					this.waitSequence = this.waitSequenceTransition();
				}
				if (!this.waitSequence.MoveNext())
				{
					this.waitSequence = null;
					return;
				}
			}
			else
			{
				SceneQuest.Args args6 = this.CreateSceneQuestArgs();
				if (text == string.Empty)
				{
					Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneQuest, args6);
					return;
				}
				SceneScenario.Args args7 = new SceneScenario.Args();
				args7.questId = this.resultArgs.quest.staticOneData.questOne.questId;
				args7.storyType = 2;
				args7.scenarioName = text;
				args7.nextSceneName = SceneManager.SceneName.SceneQuest;
				args7.nextSceneArgs = args6;
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneScenario, args7);
			}
		}
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x000AF740 File Offset: 0x000AD940
	private IEnumerator waitSequenceTransition()
	{
		DataManager.DmEvent.RequestGetCoopInfo(DataManager.DmEvent.LastCoopInfo.EventId, 0);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		CanvasManager.HdlOpenWindowBasic.Setup("確認", "更新データが見つかりました\nデータ更新を行います", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		yield return null;
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneQuest, null);
		yield break;
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x000AF748 File Offset: 0x000AD948
	private SceneQuest.Args CreateSceneQuestArgs()
	{
		SceneQuest.Args.JustBeforeBattle justBeforeBattle = new SceneQuest.Args.JustBeforeBattle();
		justBeforeBattle.playQuestId = this.resultArgs.quest.staticOneData.questOne.questId;
		justBeforeBattle.endStatus = DataManagerQuest.BattleEndStatus.CLEAR;
		justBeforeBattle.isFirstClear = this.resultArgs.battleArgs.isQuestNoClear;
		if (this.resultArgs.battleArgs.trainingTurn <= 0 && this.resultArgs.pvpDeck == null && DataManager.DmQuest.LastQuestEndResponse != null && DataManager.DmQuest.LastQuestEndResponse.IsCharaScenario)
		{
			CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this.resultArgs.quest.staticOneData.questGroup.targetCharaId);
			if (userCharaData != null)
			{
				justBeforeBattle.isReleaseMaxArts = userCharaData.dynamicData.clearScenarioNum == 4;
			}
		}
		justBeforeBattle.isMapAllClearEvent = this.resultArgs.quest.staticOneData.questOne.clearPerformance;
		if (justBeforeBattle.isFirstClear && DataManager.DmQuest.QuestStaticData.oneDataMap.ContainsKey(justBeforeBattle.playQuestId))
		{
			justBeforeBattle.specialInfoItemList = DataManager.DmQuest.QuestStaticData.oneDataMap[justBeforeBattle.playQuestId].RewardItemList.ConvertAll<ItemData>((QuestStaticQuestOne.RewardItem item) => new ItemData(item.itemId, item.num));
			justBeforeBattle.specialInfoItemMovePresentBox = this.resultArgs.battleArgs.trainingTurn <= 0 && this.resultArgs.pvpDeck == null && DataManager.DmQuest.LastQuestEndResponse != null && DataManager.DmQuest.LastQuestEndResponse.MovePresentBox;
		}
		bool flag = false;
		if (this.resultArgs.battleArgs.isQuestNoClear)
		{
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(justBeforeBattle.playQuestId);
			flag = true;
			foreach (QuestStaticQuestOne questStaticQuestOne in questOnePackData.questGroup.questOneList)
			{
				QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap.TryGetValueEx(questStaticQuestOne.questId, null);
				if (questDynamicQuestOne == null || questDynamicQuestOne.clearNum == 0)
				{
					flag = false;
					break;
				}
			}
		}
		return new SceneQuest.Args
		{
			selectQuestOneId = this.resultArgs.quest.staticOneData.questOne.questId,
			initialMap = flag,
			justBeforeBattle = justBeforeBattle
		};
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x000AF9B4 File Offset: 0x000ADBB4
	private bool SkipChara(SceneBattleResult.GUIChara gc)
	{
		bool flag = true;
		for (int i = 0; i < this.charaId.Count; i++)
		{
			if (this.charaId[i] != null)
			{
				bool flag2 = gc.LevelMax[i].playOutTime != 0f;
				gc.CharaExp[i].gameObject.SetActive(!flag2);
				this.ActivateLeftExps(gc, i);
				gc.LevelMax[i].autoPlay = true;
				if (this.levelAfter[i] > this.levelBefore[i] || this.lvexpAfter[i] > this.lvexpBefore[i])
				{
					flag = false;
					this.levelBefore[i] = this.levelAfter[i];
					gc.CharaLv[i].text = "Lv." + this.levelBefore[i].ToString();
					gc.CharaIcon[i].SetupLevel(this.levelBefore[i]);
					this.lvexpBase[i] = ((this.levelBefore[i] < this.levelMax[i]) ? DataManager.DmChara.GetExpByNextLevel(this.charaId[i].id, this.levelBefore[i]) : 0L);
					this.lvexpBefore[i] = this.lvexpAfter[i];
					gc.CharaExpGage[i].fillAmount = ((this.lvexpBase[i] > 0L) ? ((float)((double)this.lvexpBefore[i] / (double)this.lvexpBase[i])) : 1f);
				}
			}
		}
		for (int j = 0; j < this.kizunaId.Count; j++)
		{
			if (this.kizunaId[j] != null)
			{
				bool flag3 = gc.KizunaMax[j].playOutTime != 0f;
				gc.CharaHrt[j].gameObject.SetActive(!flag3);
				gc.KizunaMax[j].autoPlay = true;
				if (this.kizunaAfter[j] > this.kizunaBefore[j] || this.kizunaExpAfter[j] > this.kizunaExpBefore[j])
				{
					flag = false;
					this.kizunaBefore[j] = this.kizunaAfter[j];
					gc.CharaKz[j].text = "なかよしLv." + this.kizunaBefore[j].ToString();
					this.kizunaExpBase[j] = ((this.kizunaBefore[j] < this.kizunaMax[j]) ? DataManager.DmChara.GetKizunaExpForNextLevel(this.kizunaId[j].id, this.kizunaBefore[j]) : 0L);
					this.kizunaExpBefore[j] = this.kizunaExpAfter[j];
					gc.CharaHrtGage[j].fillAmount = ((this.kizunaExpBase[j] > 0L) ? ((float)((double)this.kizunaExpBefore[j] / (double)this.kizunaExpBase[j])) : 1f);
				}
			}
		}
		if (flag)
		{
			if (this.levelup == 0)
			{
				flag = false;
			}
			else if (this.levelup == 1)
			{
				flag = false;
				if (this.rankAfter > this.resultArgs.userLevel)
				{
					this.guiData.RankAE.ForceEnd();
				}
				for (int k = 0; k < this.charaId.Count; k++)
				{
					if (this.levelAfter[k] > this.resultArgs.charaLevel[k])
					{
						gc.CharaAE[k].ForceEnd();
					}
				}
			}
			else if (this.levelup == 2)
			{
				flag = false;
				for (int l = 0; l < this.kizunaId.Count; l++)
				{
					if (this.kizunaAfter[l] > this.resultArgs.kizunaLevel[l])
					{
						gc.KizunaAE[l].ForceEnd();
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x000AFE34 File Offset: 0x000AE034
	private bool UpdateChara(SceneBattleResult.GUIChara gc)
	{
		bool flag = true;
		for (int i = 0; i < this.charaId.Count; i++)
		{
			if (this.charaId[i] != null)
			{
				bool flag2 = gc.LevelMax[i].playOutTime != 0f;
				gc.CharaExp[i].gameObject.SetActive(!flag2);
				this.ActivateLeftExps(gc, i);
				gc.LevelMax[i].autoPlay = true;
				if (this.levelAfter[i] > this.levelBefore[i] || this.lvexpAfter[i] > this.lvexpBefore[i])
				{
					flag = false;
					double num = (double)this.lvexpBase[i] * 2.0 * (double)TimeManager.DeltaTime;
					List<long> list = this.lvexpBefore;
					int num2 = i;
					list[num2] += (long)num;
					if (this.levelAfter[i] > this.levelBefore[i])
					{
						if (this.lvexpBefore[i] >= this.lvexpBase[i])
						{
							list = this.lvexpBefore;
							num2 = i;
							list[num2] -= this.lvexpBase[i];
							List<long> list2 = this.lvexpBase;
							int num3 = i;
							List<int> list3 = this.levelBefore;
							num2 = i;
							int num4 = list3[num2] + 1;
							list3[num2] = num4;
							list2[num3] = ((num4 < this.levelMax[i]) ? DataManager.DmChara.GetExpByNextLevel(this.charaId[i].id, this.levelBefore[i]) : 0L);
							if (this.levelAfter[i] <= this.levelBefore[i] && this.lvexpBefore[i] > this.lvexpAfter[i])
							{
								this.lvexpBefore[i] = this.lvexpAfter[i];
							}
							SoundManager.Play("prd_se_friends_levelup_font", false, false);
						}
					}
					else if (this.lvexpBefore[i] > this.lvexpAfter[i])
					{
						this.lvexpBefore[i] = this.lvexpAfter[i];
					}
					else if (this.lvexpBase[i] <= 0L)
					{
						this.lvexpBefore[i] = this.lvexpAfter[i];
					}
					gc.CharaLv[i].text = "Lv." + this.levelBefore[i].ToString();
					gc.CharaIcon[i].SetupLevel(this.levelBefore[i]);
					gc.CharaExpGage[i].fillAmount = ((this.lvexpBase[i] > 0L) ? ((float)((double)this.lvexpBefore[i] / (double)this.lvexpBase[i])) : 1f);
				}
			}
		}
		for (int j = 0; j < this.kizunaId.Count; j++)
		{
			if (this.kizunaId[j] != null)
			{
				bool flag3 = gc.KizunaMax[j].playOutTime != 0f;
				gc.CharaHrt[j].gameObject.SetActive(!flag3);
				gc.KizunaMax[j].autoPlay = true;
				if (this.kizunaAfter[j] > this.kizunaBefore[j] || this.kizunaExpAfter[j] > this.kizunaExpBefore[j])
				{
					flag = false;
					double num5 = (double)this.kizunaExpBase[j] * 2.0 * (double)TimeManager.DeltaTime;
					List<long> list = this.kizunaExpBefore;
					int num4 = j;
					list[num4] += (long)num5;
					if (this.kizunaAfter[j] > this.kizunaBefore[j])
					{
						if (this.kizunaExpBefore[j] >= this.kizunaExpBase[j])
						{
							list = this.kizunaExpBefore;
							num4 = j;
							list[num4] -= this.kizunaExpBase[j];
							List<long> list4 = this.kizunaExpBase;
							int num6 = j;
							List<int> list5 = this.kizunaBefore;
							num4 = j;
							int num2 = list5[num4] + 1;
							list5[num4] = num2;
							list4[num6] = ((num2 < this.kizunaMax[j]) ? DataManager.DmChara.GetKizunaExpForNextLevel(this.kizunaId[j].id, this.kizunaBefore[j]) : 0L);
							if (this.kizunaAfter[j] <= this.kizunaBefore[j] && this.kizunaExpBefore[j] > this.kizunaExpAfter[j])
							{
								this.kizunaExpBefore[j] = this.kizunaExpAfter[j];
							}
						}
					}
					else if (this.kizunaExpBefore[j] > this.kizunaExpAfter[j])
					{
						this.kizunaExpBefore[j] = this.kizunaExpAfter[j];
					}
					else if (this.kizunaExpBase[j] <= 0L)
					{
						this.kizunaExpBefore[j] = this.kizunaExpAfter[j];
					}
					gc.CharaKz[j].text = "なかよしLv." + this.kizunaBefore[j].ToString();
					gc.CharaHrtGage[j].fillAmount = ((this.kizunaExpBase[j] > 0L) ? ((float)((double)this.kizunaExpBefore[j] / (double)this.kizunaExpBase[j])) : 1f);
				}
			}
		}
		return flag;
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x000B0454 File Offset: 0x000AE654
	private bool LevelupChara(SceneBattleResult.GUIChara gc)
	{
		bool flag = true;
		if (this.levelup == 0)
		{
			flag = false;
			if (this.rankAfter > this.resultArgs.userLevel)
			{
				this.guiData.RankAE.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			}
			for (int i = 0; i < this.charaId.Count; i++)
			{
				if (this.levelAfter[i] > this.resultArgs.charaLevel[i])
				{
					gc.CharaAE[i].PlayAnimation(PguiAECtrl.AmimeType.START, null);
				}
			}
			this.levelup++;
		}
		else if (this.levelup == 1)
		{
			if (this.rankAfter > this.resultArgs.userLevel && this.guiData.RankAE.IsPlaying())
			{
				flag = false;
			}
			for (int j = 0; j < this.charaId.Count; j++)
			{
				if (this.levelAfter[j] > this.resultArgs.charaLevel[j] && gc.CharaAE[j].IsPlaying())
				{
					flag = false;
				}
			}
			if (flag)
			{
				flag = false;
				for (int k = 0; k < this.charaId.Count; k++)
				{
					if (this.levelAfter[k] > this.resultArgs.charaLevel[k])
					{
						gc.CharaAE[k].PlayAnimation(PguiAECtrl.AmimeType.END, null);
					}
				}
				bool flag2 = false;
				for (int l = 0; l < this.kizunaId.Count; l++)
				{
					if (this.kizunaAfter[l] > this.resultArgs.kizunaLevel[l])
					{
						gc.KizunaAE[l].PlayAnimation(PguiAECtrl.AmimeType.START, null);
						flag2 = true;
					}
				}
				if (flag2)
				{
					SoundManager.Play("prd_se_result_bond_levelup", false, false);
				}
				this.levelup++;
			}
		}
		else if (this.levelup == 2)
		{
			for (int m = 0; m < this.kizunaId.Count; m++)
			{
				if (this.kizunaAfter[m] > this.resultArgs.kizunaLevel[m] && gc.KizunaAE[m].IsPlaying())
				{
					flag = false;
				}
			}
			if (flag)
			{
				flag = false;
				for (int n = 0; n < this.kizunaId.Count; n++)
				{
					if (this.kizunaAfter[n] > this.resultArgs.kizunaLevel[n])
					{
						gc.KizunaAE[n].PlayAnimation(PguiAECtrl.AmimeType.END, null);
					}
				}
				this.levelup++;
			}
		}
		return flag;
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x000B06FC File Offset: 0x000AE8FC
	private void StartKizunaUp()
	{
		while (this.kizunaId.Count > this.kizunaUp)
		{
			int num = this.kizunaUp;
			this.kizunaUp = num + 1;
			int num2 = num;
			CharaPackData charaPackData = this.kizunaId[num2];
			if (charaPackData != null && this.kizunaAfter[num2] > this.resultArgs.kizunaLevel[num2])
			{
				this.kizunaWinId = charaPackData.id;
				this.kizunaWinCloth = charaPackData.equipClothImageId;
				if (this.kizunaWinCloth > 0 && num2 == this.resultArgs.deck.deckHelperIndex && !DataManager.DmUserInfo.optionData.ViewClothesAffect)
				{
					this.kizunaWinCloth = 0;
				}
				this.kizunaWinLongSkirt = this.kizunaWinCloth > 0 && charaPackData.equipLongSkirt;
				this.kizunaWinInfo.transform.Find("Lv_Info01/Txt").GetComponent<PguiTextCtrl>().text = "Lv.<size=60>" + this.resultArgs.kizunaLevel[num2].ToString() + "</size>";
				this.kizunaWinInfo.transform.Find("Lv_Info02/Txt").GetComponent<PguiTextCtrl>().text = "Lv.<size=60><color=#fb556b>" + this.kizunaAfter[num2].ToString() + "</color></size>";
				this.kizunaWinInfo.transform.Find("Serif_Info03/Txt").GetComponent<PguiTextCtrl>().text = charaPackData.staticData.baseData.kizunaupText;
				bool flag = false;
				bool flag2 = false;
				List<string> list = new List<string>();
				this.kizunaWinItem = new List<ItemData>();
				this.afterItemIdToSourceItemId = new Dictionary<int, int>();
				DataManagerChara.KiznaRewardData kiznaRewardData = DataManager.DmChara.GetKizunaRewardData(this.resultArgs.kizunaLevel[num2], charaPackData.id);
				int num3 = ((kiznaRewardData == null) ? 0 : kiznaRewardData.artsMax);
				List<int> latestAcquiredAchievementIdList = DataManager.DmAchievement.GetLatestAcquiredAchievementIdList();
				for (int i = this.resultArgs.kizunaLevel[num2]; i < this.kizunaAfter[num2]; i++)
				{
					kiznaRewardData = DataManager.DmChara.GetKizunaRewardData(i + 1, charaPackData.id);
					if (kiznaRewardData != null)
					{
						if (!flag && kiznaRewardData.artsMax > num3)
						{
							flag = true;
							list.Add("けものミラクルレベルの上限が開放されました");
						}
						if (!flag2 && kiznaRewardData.charaquest > 0)
						{
							flag2 = true;
							list.Add("新たなキャラストーリーが開放されました");
						}
						int num4 = kiznaRewardData.itemId;
						int num5 = kiznaRewardData.itemNum;
						DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(num4);
						DataManagerAchievement.AchievementData haveAchievementData = DataManager.DmAchievement.GetHaveAchievementData(num4);
						if (achievementData != null && haveAchievementData != null && !latestAcquiredAchievementIdList.Contains(num4))
						{
							num4 = achievementData.duplicateItemId;
							num5 = achievementData.duplicateItemNum;
							this.afterItemIdToSourceItemId.Add(num4, kiznaRewardData.itemId);
						}
						else if (latestAcquiredAchievementIdList.Contains(num4))
						{
							latestAcquiredAchievementIdList.Remove(num4);
						}
						if (kiznaRewardData.itemId != 0 && num4 != 0 && kiznaRewardData.itemNum != 0 && num5 != 0)
						{
							this.kizunaWinItem.Add(new ItemData(num4, num5));
						}
					}
				}
				string text = "";
				int num6 = 0;
				while (num6 < list.Count && num6 < 3)
				{
					if (num6 > 0)
					{
						text += "\n";
					}
					text += list[num6];
					num6++;
				}
				this.kizunaWinInfo.transform.Find("Item_Info04/Txt").GetComponent<PguiTextCtrl>().text = text;
				bool activeSelf = this.kizunaWindow.activeSelf;
				this.kizunaWindow.SetActive(true);
				this.kizunaWinWhite.PlayAnimation(PguiAECtrl.AmimeType.START, null);
				this.kizunaWinBack.PlayAnimation(PguiAECtrl.AmimeType.START, null);
				if (activeSelf)
				{
					this.kizunaWinWhite.m_AEImage.playTime = (this.kizunaWinBack.m_AEImage.playTime = 0.5f);
				}
				this.kizunaWinFront.gameObject.SetActive(false);
				this.kizunaWinInfo.gameObject.SetActive(false);
				this.kizunaWinChara.gameObject.SetActive(false);
				this.kizunaWinChara.StopVoice();
				this.kizunaWinChara.Setup(0, 0, CharaMotionDefine.ActKey.INVALID, 0, false, true, null, false, null, 0f, null, false, false, false);
				SoundManager.Play("prd_se_result_bond_levelup_window", false, false);
				this.StopResultVoice();
				return;
			}
		}
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x000B0B54 File Offset: 0x000AED54
	private void UpdateKizunaUp()
	{
		if (this.kizunaWinWhite.GetAnimeType() == PguiAECtrl.AmimeType.START && !this.kizunaWinWhite.IsPlaying())
		{
			this.kizunaWinWhite.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
		}
		if (this.kizunaWinBack.GetAnimeType() == PguiAECtrl.AmimeType.START)
		{
			if (!this.kizunaWinBack.IsPlaying())
			{
				this.kizunaWinBack.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				this.kizunaWinCharaVoice = false;
				this.kizunaWinChara.gameObject.SetActive(true);
				this.kizunaWinChara.Setup(this.kizunaWinId, 0, CharaMotionDefine.ActKey.GACHA_ST, this.kizunaWinCloth, this.kizunaWinLongSkirt, false, new RenderTextureChara.FinishCallback(this.CbKizunaUpChara), false, null, 1.8333334f, delegate
				{
					this.kizunaWinCharaVoice = true;
				}, false, false, false);
				this.kizunaWinChara.SetCameraPosition(new Vector3(0f, 1.07f, 5.4f));
				this.kizunaWinTime = 0f;
				this.kizunaWinChrY = 1.225f;
				return;
			}
		}
		else if (this.kizunaWinInfo.gameObject.activeSelf)
		{
			if (this.kizunaWinChara.IsCurrentAnimation(CharaMotionDefine.ActKey.GACHA_ST))
			{
				this.kizunaWinChrY = this.kizunaWinChara.GetNodePos("j_neck").y + 0.005f;
			}
			this.kizunaWinTime = Mathf.Clamp01(this.kizunaWinTime + TimeManager.DeltaTime * 3f);
			this.kizunaWinChara.SetCameraPosition(Vector3.Lerp(new Vector3(0f, 1.07f, 5.4f), new Vector3(0f, this.kizunaWinChrY, 3.7f), this.kizunaWinTime));
			if (this.kizunaWinCharaVoice)
			{
				this.kizunaWinChara.PlayVoice(VOICE_TYPE.KUP01);
				this.kizunaWinCharaVoice = false;
				return;
			}
		}
		else
		{
			float num = this.kizunaWinChara.AnimationLength();
			if (num > 0f)
			{
				float num2 = this.kizunaWinChara.AnimationTime();
				if ((1f - num2) * num < 1f)
				{
					this.SkipKizunaUp();
				}
			}
		}
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x000B0D3C File Offset: 0x000AEF3C
	private void SkipKizunaUp()
	{
		if (this.kizunaWinInfo.gameObject.activeSelf)
		{
			if (this.kizunaWinInfo.GetAnimeType() == PguiAECtrl.AmimeType.START)
			{
				if (this.kizunaWinInfo.IsPlaying())
				{
					this.kizunaWinInfo.ForceEnd();
					return;
				}
				this.kizunaWinInfo.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				return;
			}
			else if (this.kizunaWinInfo.GetAnimeType() != PguiAECtrl.AmimeType.END)
			{
				if (this.kizunaWinItem.Count > 0)
				{
					CanvasManager.HdlGetItemWindowCtrl.Setup(this.kizunaWinItem, new GetItemWindowCtrl.SetupParam
					{
						strItemCb = delegate(GetItemWindowCtrl.WordingCallbackParam param)
						{
							string text = string.Empty;
							int id = param.itemStaticBase.GetId();
							if (this.afterItemIdToSourceItemId.ContainsKey(id))
							{
								DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(this.afterItemIdToSourceItemId[id]);
								text = string.Format("{0}はすでに所持していたため\n{1}×{2}に変換されました", achievementData.GetName(), param.itemStaticBase.GetName(), achievementData.duplicateItemNum);
							}
							else
							{
								text = PrjUtil.MakeMessage(param.itemStaticBase.GetName() + "を獲得しました");
							}
							return text;
						}
					});
					CanvasManager.HdlGetItemWindowCtrl.Open();
					this.kizunaWinItem = new List<ItemData>();
					return;
				}
				if (this.kizunaWinInfo.IsPlaying())
				{
					this.kizunaWinInfo.ForceEnd();
					return;
				}
				this.kizunaWinWhite.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				this.kizunaWinBack.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				this.kizunaWinFront.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				this.kizunaWinInfo.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				return;
			}
		}
		else
		{
			this.kizunaWinFront.gameObject.SetActive(true);
			this.kizunaWinFront.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			this.kizunaWinInfo.gameObject.SetActive(true);
			this.kizunaWinInfo.PlayAnimation(PguiAECtrl.AmimeType.START, null);
		}
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x000B0E78 File Offset: 0x000AF078
	private void CbKizunaUpChara()
	{
		this.kizunaWinChrY = this.kizunaWinChara.GetNodePos("j_neck").y + 0.005f;
		this.kizunaWinChara.SetAnimation(CharaMotionDefine.ActKey.GACHA_LP, true);
		if (!this.kizunaWinInfo.gameObject.activeSelf)
		{
			this.SkipKizunaUp();
		}
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x000B0ECC File Offset: 0x000AF0CC
	private void ActivateLeftExps(SceneBattleResult.GUIChara gc, int index)
	{
		CharaPackData charaPackData = this.resultArgs.deck.deckData[index];
		int level = charaPackData.dynamicData.level;
		int limitLevel = charaPackData.dynamicData.limitLevel;
		int kizunaLevel = charaPackData.dynamicData.kizunaLevel;
		int kizunaLimitLevel = charaPackData.dynamicData.KizunaLimitLevel;
		gc.LeftExp[index].gameObject.SetActive(level != limitLevel);
		gc.LeftKizunaExp[index].gameObject.SetActive(kizunaLevel != kizunaLimitLevel);
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x000B0F58 File Offset: 0x000AF158
	private void UpdateNorm()
	{
		if (this.page < 1)
		{
			this.page++;
			this.basePanel.SetActive(true);
			this.backPanel.SetActive(true);
			this.guiData.Touch.SetAsLastSibling();
			this.guiData.BoardNorm.gameObject.SetActive(true);
			this.guiData.BoardNorm.ExPlayAnimation("START", null);
			bool flag = true;
			for (int i = 0; i < this.guiData.StampAE.Count; i++)
			{
				if (this.missionOld[i])
				{
					AEImage aeimage = this.guiData.StampAE[i];
					aeimage.gameObject.SetActive(true);
					aeimage.playTime = aeimage.playOutTime;
					this.StampAEnd[i] = true;
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.guiData.StampCompAE.gameObject.SetActive(true);
				this.StampCompAEnd = true;
				this.guiData.StampCompAE.playTime = this.guiData.StampCompAE.playOutTime;
				this.guiData.MissionItemAE.ForceEnd();
				this.MissionItemAEnd = true;
				return;
			}
		}
		else if (this.page == 1)
		{
			if (!this.guiData.BoardNorm.ExIsPlaying())
			{
				if (this.touch)
				{
					bool flag2 = true;
					bool flag3 = true;
					for (int j = 0; j < this.guiData.StampAE.Count; j++)
					{
						if (this.missionOld[j] || this.missionNew[j])
						{
							AEImage aeimage2 = this.guiData.StampAE[j];
							if (!this.StampAEnd[j])
							{
								flag2 = false;
							}
							aeimage2.gameObject.SetActive(true);
							aeimage2.playTime = aeimage2.playOutTime;
							this.StampAEnd[j] = true;
						}
						else
						{
							flag3 = false;
						}
					}
					if (flag3)
					{
						if (!this.StampCompAEnd || !this.MissionItemAEnd)
						{
							flag2 = false;
						}
						this.guiData.StampCompAE.gameObject.SetActive(true);
						this.StampCompAEnd = true;
						this.guiData.StampCompAE.playTime = this.guiData.StampCompAE.playOutTime;
						this.guiData.MissionItemAE.ForceEnd();
						this.MissionItemAEnd = true;
					}
					if (flag2)
					{
						if (this.rankAfter > this.rankBefore || this.expAfter > this.expBefore)
						{
							flag2 = false;
							this.rankBefore = this.rankAfter;
							this.expBefore = this.expAfter;
							this.guiData.Rank.text = this.rankBefore.ToString();
							this.expBase = DataManager.DmUserInfo.GetExpByNextLevel(this.rankBefore);
							this.guiData.ExpGage.fillAmount = ((this.expBase > 0L) ? ((float)((double)this.expBefore / (double)this.expBase)) : 1f);
						}
						if (!this.SkipChara(this.charaNorm))
						{
							flag2 = false;
						}
						if (this.expSE)
						{
							this.expSEHdl.Stop();
							this.expSE = false;
						}
					}
					if (this.rankup != 0 || this.kizunaId.Count > this.kizunaUp)
					{
						flag2 = false;
					}
					if (this.kizunaWindow.activeSelf)
					{
						this.SkipKizunaUp();
						flag2 = false;
					}
					if (flag2)
					{
						this.page++;
						this.guiData.BoardNorm.ExPlayAnimation("START_SUB", null);
						SoundManager.Play("prd_se_result_page_change", false, false);
						return;
					}
					SoundManager.Play("prd_se_click", false, false);
					return;
				}
				else
				{
					bool flag4 = true;
					if (this.guiData.StampCompAE.gameObject.activeSelf)
					{
						if (!this.StampCompAEnd)
						{
							this.StampCompAEnd = this.guiData.StampCompAE.end;
						}
						if (this.StampCompAEnd)
						{
							if (this.guiData.MissionItemAE.m_AEImage.autoPlay)
							{
								if (!this.MissionItemAEnd)
								{
									this.MissionItemAEnd = !this.guiData.MissionItemAE.IsPlaying();
								}
								if (this.MissionItemAEnd)
								{
									this.guiData.MissionItemAE.ForceEnd();
								}
								else
								{
									flag4 = false;
								}
							}
							else
							{
								foreach (bool flag5 in this.missionOld)
								{
									flag4 = flag4 && flag5;
								}
								this.guiData.MissionItemAE.PlayAnimation(PguiAECtrl.AmimeType.START, null);
								if (flag4)
								{
									this.MissionItemAEnd = true;
									this.guiData.MissionItemAE.ForceEnd();
								}
								else
								{
									SoundManager.Play("prd_se_result_quest_mission_complete", false, false);
								}
								flag4 = false;
							}
							this.guiData.StampCompAE.playTime = this.guiData.StampCompAE.playOutTime;
						}
						else
						{
							flag4 = false;
						}
						using (List<AEImage>.Enumerator enumerator2 = this.guiData.StampAE.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								AEImage aeimage3 = enumerator2.Current;
								if (aeimage3.gameObject.activeSelf)
								{
									aeimage3.playTime = aeimage3.playOutTime;
								}
							}
							goto IL_06A3;
						}
					}
					bool flag6 = true;
					bool flag7 = true;
					for (int k = 0; k < this.guiData.StampAE.Count; k++)
					{
						AEImage aeimage4 = this.guiData.StampAE[k];
						if (aeimage4.gameObject.activeSelf)
						{
							if (!this.StampAEnd[k])
							{
								this.StampAEnd[k] = aeimage4.end;
							}
							if (this.StampAEnd[k])
							{
								aeimage4.playTime = aeimage4.playOutTime;
							}
							else
							{
								flag4 = false;
							}
						}
						else if (this.missionOld[k])
						{
							aeimage4.gameObject.SetActive(true);
							aeimage4.playTime = aeimage4.playOutTime;
							this.StampAEnd[k] = true;
						}
						else if (this.missionNew[k])
						{
							if (flag4)
							{
								flag4 = false;
								aeimage4.gameObject.SetActive(true);
								SoundManager.Play("prd_se_result_mission_stamp", false, false);
							}
						}
						else
						{
							flag6 = false;
						}
						flag7 &= this.missionOld[k];
					}
					if (flag4 && flag6)
					{
						this.guiData.StampCompAE.gameObject.SetActive(true);
						if (flag7)
						{
							this.StampCompAEnd = true;
							this.guiData.StampCompAE.playTime = this.guiData.StampCompAE.playOutTime;
							this.guiData.MissionItemAE.ForceEnd();
							this.MissionItemAEnd = true;
						}
						else
						{
							SoundManager.Play("prd_se_result_mission_stamp_complete", false, false);
						}
						flag4 = false;
					}
					IL_06A3:
					if (flag4)
					{
						bool flag8 = true;
						if (this.rankAfter > this.rankBefore || this.expAfter > this.expBefore)
						{
							flag4 = (flag8 = false);
							double num = (double)this.expBase * 2.0 * (double)TimeManager.DeltaTime;
							this.expBefore += (long)num;
							if (this.rankAfter > this.rankBefore)
							{
								if (this.expBefore >= this.expBase)
								{
									this.expBefore -= this.expBase;
									DataManagerUserInfo dmUserInfo = DataManager.DmUserInfo;
									int num2 = this.rankBefore + 1;
									this.rankBefore = num2;
									this.expBase = dmUserInfo.GetExpByNextLevel(num2);
									if (this.rankAfter <= this.rankBefore && this.expBefore > this.expAfter)
									{
										this.expBefore = this.expAfter;
									}
									SoundManager.Play("prd_se_result_levelup_player", false, false);
								}
							}
							else if (this.expBefore > this.expAfter)
							{
								this.expBefore = this.expAfter;
							}
							else if (this.expBase <= 0L)
							{
								this.expBefore = this.expAfter;
							}
							this.guiData.Rank.text = this.rankBefore.ToString();
							this.guiData.ExpGage.fillAmount = ((this.expBase > 0L) ? ((float)((double)this.expBefore / (double)this.expBase)) : 1f);
						}
						if (!this.UpdateChara(this.charaNorm))
						{
							flag4 = (flag8 = false);
						}
						if (flag8)
						{
							if (this.expSE)
							{
								this.expSEHdl.Stop();
								this.expSE = false;
							}
						}
						else if (!this.expSE)
						{
							this.expSEHdl = SoundManager.Play("prd_se_result_exp_gain", true, false);
							this.expSE = true;
						}
					}
					if (flag4)
					{
						flag4 = this.LevelupChara(this.charaNorm);
					}
					if (flag4 && this.rankup == 1)
					{
						this.rankup = 2;
						this.rankupWindow.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.ClickRankup), null, false);
						this.rankupWindow.Open();
						SoundManager.Play("prd_se_result_levelup", false, false);
					}
					if (this.rankup == 2 && this.rankupWindow.FinishedOpen())
					{
						this.rankup = 3;
					}
					if (this.kizunaWindow.activeSelf)
					{
						if (this.kizunaWinWhite.GetAnimeType() == PguiAECtrl.AmimeType.END)
						{
							if (this.kizunaWinWhite.IsPlaying())
							{
								if (this.kizunaWinChara.gameObject.activeSelf)
								{
									if (this.kizunaWinBack.IsPlaying())
									{
										flag4 = false;
									}
									else
									{
										this.kizunaWinChara.gameObject.SetActive(false);
									}
								}
							}
							else
							{
								this.kizunaWindow.SetActive(false);
							}
						}
						else
						{
							flag4 = false;
						}
					}
					if (flag4 && this.rankup == 0 && this.kizunaId.Count > this.kizunaUp)
					{
						this.StartKizunaUp();
						return;
					}
					if (this.kizunaWindow.activeSelf)
					{
						this.UpdateKizunaUp();
						return;
					}
					if (flag4 && this.rankup == 0)
					{
						this.guiData.Touch.SetAsFirstSibling();
						return;
					}
				}
			}
		}
		else if (this.page == 2)
		{
			if (!this.guiData.BoardNorm.ExIsPlaying() && this.requestRematch == null)
			{
				if (this.touch || this.rematch < 0)
				{
					bool flag9 = true;
					for (int l = 0; l < this.guiData.DropItem.Count; l++)
					{
						if (this.guiData.DropItem[l].activeSelf)
						{
							if (!this.guiData.DropItemAE[l].m_AEImage.autoPlay)
							{
								flag9 = false;
								this.guiData.DropItemAE[l].PlayAnimation(PguiAECtrl.AmimeType.START, null);
							}
							else if (this.guiData.DropItemAE[l].IsPlaying())
							{
								flag9 = false;
							}
							this.guiData.DropItemAE[l].ForceEnd();
						}
					}
					if (this.goldBefore < this.goldAfter)
					{
						flag9 = false;
						this.goldBefore = this.goldAfter;
						this.guiData.ownCoin.text = this.goldBefore.ToString();
					}
					if (this.rematch < 0)
					{
						this.requestRematch = this.Rematch();
					}
					else if (flag9 && (this.rematch == 0 || (this.guiData.Rematch.gameObject.activeSelf && !this.guiData.Rematch.ExIsPlaying())))
					{
						this.page++;
						this.guiData.Touch.SetAsLastSibling();
						this.guiData.Rematch.gameObject.SetActive(false);
						this.guiData.SkipGroup.gameObject.SetActive(false);
					}
					SoundManager.Play("prd_se_click", false, false);
				}
				else
				{
					bool flag10 = false;
					for (int m = 0; m < this.guiData.DropItem.Count; m++)
					{
						if (this.guiData.DropItem[m].activeSelf && !this.guiData.DropItemAE[m].m_AEImage.autoPlay)
						{
							flag10 = true;
							this.guiData.DropItemAE[m].PlayAnimation(PguiAECtrl.AmimeType.START, null);
						}
					}
					if (flag10)
					{
						SoundManager.Play("prd_se_result_drop_item", false, false);
					}
					float num3 = (float)this.gold * TimeManager.DeltaTime;
					if ((this.goldBefore += (int)num3) > this.goldAfter)
					{
						this.goldBefore = this.goldAfter;
					}
					this.guiData.ownCoin.text = this.goldBefore.ToString();
				}
			}
			if (this.rematch > 0 && !this.guiData.Rematch.gameObject.activeSelf)
			{
				bool flag11 = true;
				for (int n = 0; n < this.guiData.DropItem.Count; n++)
				{
					if (this.guiData.DropItem[n].activeSelf && (!this.guiData.DropItemAE[n].m_AEImage.autoPlay || this.guiData.DropItemAE[n].IsPlaying()))
					{
						flag11 = false;
					}
				}
				if (flag11)
				{
					this.guiData.Rematch.gameObject.SetActive(true);
					this.guiData.Rematch.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					this.guiData.RematchBtn.SetActEnable(this.rematchOwn < 0 || this.rematchOwn >= this.rematchNeed, true, false);
				}
			}
			if (!this.guiData.SkipGroup.gameObject.activeSelf)
			{
				bool flag12 = true;
				for (int num4 = 0; num4 < this.guiData.DropItem.Count; num4++)
				{
					if (this.guiData.DropItem[num4].activeSelf && (!this.guiData.DropItemAE[num4].m_AEImage.autoPlay || this.guiData.DropItemAE[num4].IsPlaying()))
					{
						flag12 = false;
					}
				}
				if (flag12 && this.skipInfo.isSkippable)
				{
					this.guiData.SkipGroup.gameObject.SetActive(true);
					this.guiData.SkipGroup.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					this.guiData.SkipBtn.SetActEnable(true, false, false);
					if (this.skipInfo.restSkipCount > 0)
					{
						this.SetQuestSkipPopup(this.skipInfo);
					}
					else
					{
						this.guiData.QuestSkipPopup.SetActive(false);
						this.guiData.SkipBtn.SetActEnable(!this.skipInfo.hasSkipLimit || this.skipInfo.restSkipRecoveryCount > 0, false, false);
					}
				}
			}
			if (this.guiData.Rematch.gameObject.activeSelf)
			{
				int stackNum = this.rematchOwn;
				if (stackNum < 0)
				{
					stackNum = DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now).stackNum;
				}
				int num5 = this.rematchNeed;
				if (num5 < 0)
				{
					num5 = -1 - num5;
				}
				string text = stackNum.ToString();
				if (stackNum < num5)
				{
					text = "<color=#FF0000>" + text + "</color>";
				}
				string text2 = num5.ToString();
				if (this.rematchNeed < 0)
				{
					text2 = "<color=#00FF00>" + text2 + "</color>";
				}
				this.guiData.RematchBtn.transform.Find("BaseImage/Inbase/Num_Use").GetComponent<PguiTextCtrl>().text = text + "/" + text2;
				return;
			}
		}
		else if (this.page == 3)
		{
			if (this.friendFollow == null)
			{
				this.friendFollow = DataManagerHelper.RequestFollowApply(this.resultArgs.helper, this.resultArgs.battleArgs.attrIndex, false);
				return;
			}
			if (!this.friendFollow.MoveNext())
			{
				this.friendFollow = null;
				this.page++;
				return;
			}
		}
		else
		{
			if (this.resultArgs.battleArgs.tutorialSequence == TutorialUtil.Sequence.INVALID)
			{
				this.requestNextScene = true;
				return;
			}
			TutorialUtil.RequestNextSequence(this.resultArgs.battleArgs.tutorialSequence);
		}
	}

	// Token: 0x06000E99 RID: 3737 RVA: 0x000B1F60 File Offset: 0x000B0160
	public void SetQuestSkipPopup(QuestUtil.UsrQuestSkipInfo skipInfo)
	{
		this.guiData.QuestSkipPopup.SetActive(true);
		this.guiData.QuestSkipPopup.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		this.guiData.QuestSkipPopup.transform.Find("Txt_Campaign").GetComponent<PguiTextCtrl>().text = skipInfo.popupMessage;
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x000B1FC0 File Offset: 0x000B01C0
	private void UpdatePvp()
	{
		if (this.page >= 1)
		{
			if (this.page == 1)
			{
				if (!((this.resultArgs.battleArgs.pvpTraining > 0) ? this.guiData.BoardPvpTraining.ExIsPlaying() : this.guiData.BoardPvp.ExIsPlaying()))
				{
					if (this.touch)
					{
						bool flag = this.SkipChara((this.resultArgs.battleArgs.pvpTraining > 0) ? this.charaPvpTraining : this.charaPvp);
						if (this.rankup != 0 || this.kizunaId.Count > this.kizunaUp)
						{
							flag = false;
						}
						if (this.kizunaWindow.activeSelf)
						{
							this.SkipKizunaUp();
							flag = false;
						}
						if (flag)
						{
							this.page++;
						}
						SoundManager.Play("prd_se_click", false, false);
						return;
					}
					bool flag2 = this.UpdateChara((this.resultArgs.battleArgs.pvpTraining > 0) ? this.charaPvpTraining : this.charaPvp);
					if (flag2)
					{
						flag2 = this.LevelupChara((this.resultArgs.battleArgs.pvpTraining > 0) ? this.charaPvpTraining : this.charaPvp);
					}
					if (this.pvpRankUp != null && !this.pvpRankUp.MoveNext())
					{
						this.pvpRankUp = null;
					}
					if (flag2 && this.rankup == 1)
					{
						this.rankup++;
						this.pvpRankUp = SelPvpCtrl.RankUpEvent(this.resultArgs.battleArgs.pvpSeasonId, this.pvpResult.befPvpPoint, this.pvpResult.nowPvpPoint);
						SoundManager.Play("prd_se_pvp_rankup", false, false);
					}
					else if (this.rankup == 2 && this.pvpRankUp == null)
					{
						this.rankup = 0;
					}
					if (this.kizunaWindow.activeSelf && this.kizunaWinWhite.GetAnimeType() == PguiAECtrl.AmimeType.END)
					{
						if (this.kizunaWinWhite.IsPlaying())
						{
							if (this.kizunaWinChara.gameObject.activeSelf && !this.kizunaWinBack.IsPlaying())
							{
								this.kizunaWinChara.gameObject.SetActive(false);
							}
						}
						else
						{
							this.kizunaWindow.SetActive(false);
						}
					}
					if (this.kizunaWindow.activeSelf)
					{
						if (this.kizunaWinWhite.GetAnimeType() == PguiAECtrl.AmimeType.END)
						{
							if (this.kizunaWinWhite.IsPlaying())
							{
								if (this.kizunaWinChara.gameObject.activeSelf)
								{
									if (this.kizunaWinBack.IsPlaying())
									{
										flag2 = false;
									}
									else
									{
										this.kizunaWinChara.gameObject.SetActive(false);
									}
								}
							}
							else
							{
								this.kizunaWindow.SetActive(false);
							}
						}
						else
						{
							flag2 = false;
						}
					}
					if (flag2 && this.rankup == 0 && this.kizunaId.Count > this.kizunaUp)
					{
						this.StartKizunaUp();
						return;
					}
					if (this.kizunaWindow.activeSelf)
					{
						this.UpdateKizunaUp();
						return;
					}
					if (flag2 && this.rankup == 0)
					{
						this.guiData.Touch.SetAsFirstSibling();
						return;
					}
				}
			}
			else
			{
				this.requestNextScene = true;
			}
			return;
		}
		this.page++;
		this.basePanel.SetActive(true);
		this.backPanel.SetActive(true);
		this.guiData.Touch.SetAsLastSibling();
		if (this.resultArgs.battleArgs.pvpTraining > 0)
		{
			this.guiData.BoardPvpTraining.gameObject.SetActive(true);
			this.guiData.BoardPvpTraining.ExPlayAnimation("START", null);
			return;
		}
		this.guiData.BoardPvp.gameObject.SetActive(true);
		this.guiData.BoardPvp.ExPlayAnimation("START", null);
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x000B2354 File Offset: 0x000B0554
	private void UpdateTraining()
	{
		if (this.page < 1)
		{
			this.page++;
			this.basePanel.SetActive(true);
			this.backPanel.SetActive(true);
			this.guiData.Touch.SetAsLastSibling();
			this.guiData.BoardTraining.gameObject.SetActive(true);
			this.guiData.BoardTraining.ExPlayAnimation("START", null);
			if (this.resultArgs.battleArgs.isPractice && !DataManager.DmUserInfo.dispPracticeConfirm)
			{
				this.checkPracticeConfirm = this.CheckPracticeConfirm();
				return;
			}
		}
		else if (this.page == 1)
		{
			if (this.guiData.BoardTraining.ExIsPlaying())
			{
				this.guiData.TrainingScore.enabled = false;
				this.guiData.TrainingScore.enabled = true;
				return;
			}
			if (this.touch)
			{
				bool flag = this.SkipChara(this.charaTraining);
				if (this.rankup != 0 || this.kizunaId.Count > this.kizunaUp)
				{
					flag = false;
				}
				if (this.kizunaWindow.activeSelf)
				{
					this.SkipKizunaUp();
					flag = false;
				}
				if (flag)
				{
					this.page++;
				}
				SoundManager.Play("prd_se_click", false, false);
				return;
			}
			bool flag2 = this.UpdateChara(this.charaTraining);
			if (flag2)
			{
				flag2 = this.LevelupChara(this.charaTraining);
			}
			if (this.pvpRankUp != null && !this.pvpRankUp.MoveNext())
			{
				this.pvpRankUp = null;
			}
			if (flag2 && this.rankup == 1)
			{
				this.rankup++;
				this.pvpRankUp = SelPvpCtrl.RankUpEvent(this.resultArgs.battleArgs.pvpSeasonId, this.pvpResult.befPvpPoint, this.pvpResult.nowPvpPoint);
				SoundManager.Play("prd_se_pvp_rankup", false, false);
			}
			else if (this.rankup == 2 && this.pvpRankUp == null)
			{
				this.rankup = 0;
			}
			if (this.kizunaWindow.activeSelf && this.kizunaWinWhite.GetAnimeType() == PguiAECtrl.AmimeType.END)
			{
				if (this.kizunaWinWhite.IsPlaying())
				{
					if (this.kizunaWinChara.gameObject.activeSelf && !this.kizunaWinBack.IsPlaying())
					{
						this.kizunaWinChara.gameObject.SetActive(false);
					}
				}
				else
				{
					this.kizunaWindow.SetActive(false);
				}
			}
			if (this.kizunaWindow.activeSelf)
			{
				if (this.kizunaWinWhite.GetAnimeType() == PguiAECtrl.AmimeType.END)
				{
					if (this.kizunaWinWhite.IsPlaying())
					{
						if (this.kizunaWinChara.gameObject.activeSelf)
						{
							if (this.kizunaWinBack.IsPlaying())
							{
								flag2 = false;
							}
							else
							{
								this.kizunaWinChara.gameObject.SetActive(false);
							}
						}
					}
					else
					{
						this.kizunaWindow.SetActive(false);
					}
				}
				else
				{
					flag2 = false;
				}
			}
			if (flag2 && this.rankup == 0 && this.kizunaId.Count > this.kizunaUp)
			{
				this.StartKizunaUp();
				return;
			}
			if (this.kizunaWindow.activeSelf)
			{
				this.UpdateKizunaUp();
				return;
			}
			if (flag2 && this.rankup == 0)
			{
				this.guiData.Touch.SetAsFirstSibling();
				return;
			}
		}
		else
		{
			this.requestNextScene = true;
		}
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x000B2682 File Offset: 0x000B0882
	private IEnumerator CheckPracticeConfirm()
	{
		string text = "練習モードのため\n";
		text += "<color=#ff0000>";
		text += "ランキングおよびスコアやポイントは反映されず\n報酬は獲得できません";
		text += "</color>";
		bool isCheckActive = false;
		CanvasManager.HdlOpenWindowCheckBox.SetupCheckBox("確認", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			CanvasManager.HdlOpenWindowCheckBox.ForceClose();
			return true;
		}, delegate(bool isActive)
		{
			isCheckActive = isActive;
		}, "今日はこのウィンドウを表示しない");
		CanvasManager.HdlOpenWindowCheckBox.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowCheckBox.FinishedClose());
		if (isCheckActive)
		{
			DataManager.DmUserInfo.RequestActionUpdatePracticeConfirm(1);
		}
		yield break;
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x000B268A File Offset: 0x000B088A
	private bool ClickRankup(int index)
	{
		if (this.rankup > 0)
		{
			this.rankup = 0;
		}
		return true;
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x000B26A0 File Offset: 0x000B08A0
	private void StopResultVoice()
	{
		if (!string.IsNullOrEmpty(this.resultArgs.resultVoiceFirstSheet))
		{
			SoundManager.Stop(this.resultArgs.resultVoiceFirstSheet);
			SoundManager.UnloadCueSheet(this.resultArgs.resultVoiceFirstSheet);
		}
		if (!string.IsNullOrEmpty(this.resultArgs.resultVoiceSecondSheet))
		{
			SoundManager.Stop(this.resultArgs.resultVoiceSecondSheet);
			SoundManager.UnloadCueSheet(this.resultArgs.resultVoiceSecondSheet);
		}
		this.resultArgs.resultVoiceFirst = "";
		this.resultArgs.resultVoiceFirstSheet = "";
		this.resultArgs.resultVoiceSecond = "";
		this.resultArgs.resultVoiceSecondSheet = "";
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x000B2754 File Offset: 0x000B0954
	private bool IsChangeTerm()
	{
		if (!DataManager.DmEvent.isRaidByQuestOneId(this.resultArgs.battleArgs.questOneId))
		{
			return false;
		}
		DataManagerEvent.CoopRaidTermData nowTermData = DataManager.DmEvent.GetNowTermData(this.resultArgs.battleArgs.eventId);
		DateTime battleStartTime = this.resultArgs.battleStartTime;
		return nowTermData == null || !nowTermData.IsOverStartTime(battleStartTime) || battleStartTime.Day != TimeManager.Now.Day;
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x000B27CC File Offset: 0x000B09CC
	public override void OnDisableScene()
	{
		this.StopResultVoice();
		foreach (IconItemCtrl iconItemCtrl in this.dropIcon)
		{
			Object.Destroy(iconItemCtrl.gameObject);
		}
		this.dropIcon = new List<IconItemCtrl>();
		if (this.kizunaWinChara != null)
		{
			Object.Destroy(this.kizunaWinChara.gameObject);
		}
		this.kizunaWinChara = null;
		this.basePanel.SetActive(false);
		this.backPanel.SetActive(false);
		if (this.chara != null)
		{
			Object.Destroy(this.chara);
		}
		this.chara = null;
		EffectManager.BillboardCamera = null;
		if (this.filed != null)
		{
			Object.Destroy(this.filed);
		}
		this.filed = null;
		CanvasManager.SetBgTex(null);
		if (SceneBattle.resultRT != null)
		{
			SceneBattle.resultRT.Release();
			Object.DestroyImmediate(SceneBattle.resultRT);
			SceneBattle.resultRT = null;
		}
		CanvasManager.HdlMissionProgressCtrl.IsAfterQuestSkip = false;
		this.requestRematch = null;
		this.waitSequence = null;
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x000B28FC File Offset: 0x000B0AFC
	public override void OnDestroyScene()
	{
		SoundManager.UnloadCueSheet("se_cb");
		Object.Destroy(this.missionIcon);
		this.missionIcon = null;
		this.rankupWindow = null;
		this.rankWinBefore = null;
		this.rankWinAfter = null;
		this.kizunaWindow = null;
		this.kizunaWinWhite = null;
		this.kizunaWinBack = null;
		this.kizunaWinFront = null;
		this.kizunaWinInfo = null;
		this.kizunaWinChara = null;
		Object.Destroy(this.windowPanel);
		this.windowPanel = null;
		foreach (IconCharaCtrl iconCharaCtrl in this.charaNorm.CharaIcon)
		{
			Object.Destroy(iconCharaCtrl.gameObject);
		}
		this.charaNorm.CharaIcon = new List<IconCharaCtrl>();
		foreach (IconCharaCtrl iconCharaCtrl2 in this.charaPvp.CharaIcon)
		{
			Object.Destroy(iconCharaCtrl2.gameObject);
		}
		this.charaPvp.CharaIcon = new List<IconCharaCtrl>();
		foreach (IconCharaCtrl iconCharaCtrl3 in this.charaPvpTraining.CharaIcon)
		{
			Object.Destroy(iconCharaCtrl3.gameObject);
		}
		this.charaPvpTraining.CharaIcon = new List<IconCharaCtrl>();
		foreach (IconCharaCtrl iconCharaCtrl4 in this.charaTraining.CharaIcon)
		{
			Object.Destroy(iconCharaCtrl4.gameObject);
		}
		this.charaTraining.CharaIcon = new List<IconCharaCtrl>();
		this.charaNorm = null;
		this.charaPvp = null;
		this.charaPvpTraining = null;
		this.charaTraining = null;
		this.guiData = null;
		Object.Destroy(this.basePanel);
		this.basePanel = null;
		Object.Destroy(this.backPanel);
		this.backPanel = null;
	}

	// Token: 0x04000CE8 RID: 3304
	private GameObject filed;

	// Token: 0x04000CE9 RID: 3305
	private CharaModelHandle chara;

	// Token: 0x04000CEA RID: 3306
	private IEnumerator seLoad;

	// Token: 0x04000CEB RID: 3307
	private GameObject basePanel;

	// Token: 0x04000CEC RID: 3308
	private GameObject backPanel;

	// Token: 0x04000CED RID: 3309
	private GameObject windowPanel;

	// Token: 0x04000CEE RID: 3310
	private PguiOpenWindowCtrl rankupWindow;

	// Token: 0x04000CEF RID: 3311
	private PguiTextCtrl rankWinBefore;

	// Token: 0x04000CF0 RID: 3312
	private PguiTextCtrl rankWinAfter;

	// Token: 0x04000CF1 RID: 3313
	private GameObject kizunaWindow;

	// Token: 0x04000CF2 RID: 3314
	private PguiAECtrl kizunaWinWhite;

	// Token: 0x04000CF3 RID: 3315
	private PguiAECtrl kizunaWinBack;

	// Token: 0x04000CF4 RID: 3316
	private PguiAECtrl kizunaWinFront;

	// Token: 0x04000CF5 RID: 3317
	private PguiAECtrl kizunaWinInfo;

	// Token: 0x04000CF6 RID: 3318
	private int kizunaWinId;

	// Token: 0x04000CF7 RID: 3319
	private int kizunaWinCloth;

	// Token: 0x04000CF8 RID: 3320
	private bool kizunaWinLongSkirt;

	// Token: 0x04000CF9 RID: 3321
	private RenderTextureChara kizunaWinChara;

	// Token: 0x04000CFA RID: 3322
	private bool kizunaWinCharaVoice;

	// Token: 0x04000CFB RID: 3323
	private float kizunaWinTime;

	// Token: 0x04000CFC RID: 3324
	private float kizunaWinChrY;

	// Token: 0x04000CFD RID: 3325
	private List<ItemData> kizunaWinItem;

	// Token: 0x04000CFE RID: 3326
	private Dictionary<int, int> afterItemIdToSourceItemId;

	// Token: 0x04000CFF RID: 3327
	private DataManagerPvp.PvPEndResult pvpResult;

	// Token: 0x04000D00 RID: 3328
	private IEnumerator pvpRankUp;

	// Token: 0x04000D01 RID: 3329
	private SceneBattleResult.GUI guiData;

	// Token: 0x04000D02 RID: 3330
	private SceneBattleResult.GUIChara charaNorm;

	// Token: 0x04000D03 RID: 3331
	private SceneBattleResult.GUIChara charaPvp;

	// Token: 0x04000D04 RID: 3332
	private SceneBattleResult.GUIChara charaPvpTraining;

	// Token: 0x04000D05 RID: 3333
	private SceneBattleResult.GUIChara charaTraining;

	// Token: 0x04000D06 RID: 3334
	private int page;

	// Token: 0x04000D07 RID: 3335
	private bool touch;

	// Token: 0x04000D08 RID: 3336
	private int rankup;

	// Token: 0x04000D09 RID: 3337
	private int levelup;

	// Token: 0x04000D0A RID: 3338
	private bool expSE;

	// Token: 0x04000D0B RID: 3339
	private CriAtomExPlayback expSEHdl;

	// Token: 0x04000D0C RID: 3340
	private List<bool> StampAEnd;

	// Token: 0x04000D0D RID: 3341
	private bool StampCompAEnd;

	// Token: 0x04000D0E RID: 3342
	private bool MissionItemAEnd;

	// Token: 0x04000D0F RID: 3343
	private IEnumerator friendFollow;

	// Token: 0x04000D10 RID: 3344
	private IEnumerator waitSequence;

	// Token: 0x04000D11 RID: 3345
	private IconItemCtrl missionIcon;

	// Token: 0x04000D12 RID: 3346
	private List<IconItemCtrl> dropIcon;

	// Token: 0x04000D13 RID: 3347
	private List<IconItemCtrl> rewardIcon;

	// Token: 0x04000D14 RID: 3348
	private SceneBattleResultArgs resultArgs;

	// Token: 0x04000D15 RID: 3349
	private int rankBefore;

	// Token: 0x04000D16 RID: 3350
	private int rankAfter;

	// Token: 0x04000D17 RID: 3351
	private long expBefore;

	// Token: 0x04000D18 RID: 3352
	private long expAfter;

	// Token: 0x04000D19 RID: 3353
	private long expBase;

	// Token: 0x04000D1A RID: 3354
	private List<CharaPackData> charaId;

	// Token: 0x04000D1B RID: 3355
	private List<int> levelBefore;

	// Token: 0x04000D1C RID: 3356
	private List<int> levelAfter;

	// Token: 0x04000D1D RID: 3357
	private List<int> levelMax;

	// Token: 0x04000D1E RID: 3358
	private List<long> lvexpBefore;

	// Token: 0x04000D1F RID: 3359
	private List<long> lvexpAfter;

	// Token: 0x04000D20 RID: 3360
	private List<long> lvexpBase;

	// Token: 0x04000D21 RID: 3361
	private List<CharaPackData> kizunaId;

	// Token: 0x04000D22 RID: 3362
	private List<int> kizunaBefore;

	// Token: 0x04000D23 RID: 3363
	private List<int> kizunaAfter;

	// Token: 0x04000D24 RID: 3364
	private List<int> kizunaMax;

	// Token: 0x04000D25 RID: 3365
	private List<long> kizunaExpBefore;

	// Token: 0x04000D26 RID: 3366
	private List<long> kizunaExpAfter;

	// Token: 0x04000D27 RID: 3367
	private List<long> kizunaExpBase;

	// Token: 0x04000D28 RID: 3368
	private int kizunaUp;

	// Token: 0x04000D29 RID: 3369
	private int goldBefore;

	// Token: 0x04000D2A RID: 3370
	private int goldAfter;

	// Token: 0x04000D2B RID: 3371
	private int gold;

	// Token: 0x04000D2C RID: 3372
	private List<bool> missionStatus;

	// Token: 0x04000D2D RID: 3373
	private List<bool> missionOld;

	// Token: 0x04000D2E RID: 3374
	private List<bool> missionNew;

	// Token: 0x04000D2F RID: 3375
	private bool requestNextScene;

	// Token: 0x04000D30 RID: 3376
	private IEnumerator requestRematch;

	// Token: 0x04000D31 RID: 3377
	private int rematch;

	// Token: 0x04000D32 RID: 3378
	private int rematchOwn;

	// Token: 0x04000D33 RID: 3379
	private int rematchNeed;

	// Token: 0x04000D34 RID: 3380
	private string rematchItem;

	// Token: 0x04000D35 RID: 3381
	private bool lucky;

	// Token: 0x04000D36 RID: 3382
	private int skipCount = -1;

	// Token: 0x04000D37 RID: 3383
	private IEnumerator checkKizunaLimitReached;

	// Token: 0x04000D38 RID: 3384
	private IEnumerator checkPracticeConfirm;

	// Token: 0x04000D39 RID: 3385
	private QuestUtil.UsrQuestSkipInfo skipInfo;

	// Token: 0x02000908 RID: 2312
	public class GUI
	{
		// Token: 0x06003A79 RID: 14969 RVA: 0x001CEAC4 File Offset: 0x001CCCC4
		public GUI(Transform baseTr)
		{
			this.Touch = baseTr.Find("TouchCollision");
			this.BoardNorm = baseTr.Find("BoardAll").GetComponent<SimpleAnimation>();
			Transform transform = baseTr.Find("BoardAll/Page01");
			this.Quest = transform.Find("QuestInfo/Txt_QuestName").GetComponent<PguiTextCtrl>();
			this.QuestNum = transform.Find("QuestInfo/Num_Quest").GetComponent<PguiTextCtrl>();
			this.Turn = transform.Find("TurnInfo/Txt_Turn").GetComponent<PguiTextCtrl>();
			this.UserIcon = transform.Find("UserExp/UserIcon").GetComponent<PguiReplaceSpriteCtrl>();
			this.Rank = transform.Find("UserExp/Num_Rank").GetComponent<PguiTextCtrl>();
			this.RankAE = transform.Find("UserExp/AEImage_RankUp").GetComponent<PguiAECtrl>();
			this.Exp = transform.Find("UserExp/ExpGage_Base");
			this.ExpGage = this.Exp.Find("ExpGage_Gage").GetComponent<PguiImageCtrl>().m_Image;
			transform = transform.Find("MissionInfo");
			this.StampCompAE = transform.Find("AEImage_Stamp_Comp").GetComponent<AEImage>();
			this.StampCompAE.playLoop = false;
			this.StampCompAE.autoPlay = true;
			this.StampCompAE.playTime = (this.StampCompAE.playInTime = 0f);
			this.StampCompAE.playOutTime = this.StampCompAE.duration;
			this.StampCompAE.gameObject.SetActive(false);
			this.StampAE = new List<AEImage>();
			int num = 1;
			for (;;)
			{
				Transform transform2 = transform.Find("AEImage_Stamp0" + num.ToString());
				if (transform2 == null)
				{
					break;
				}
				AEImage component = transform2.GetComponent<AEImage>();
				component.playLoop = false;
				component.autoPlay = true;
				component.playTime = (component.playInTime = 0f);
				component.playOutTime = component.duration;
				component.gameObject.SetActive(false);
				this.StampAE.Add(component);
				num++;
			}
			this.MissionItem = transform.Find("ItemInfo");
			this.MissionItemAE = this.MissionItem.Find("AEImage").GetComponent<PguiAECtrl>();
			this.DropItem = new List<GameObject>();
			this.DropItemBonus = new List<PguiTextCtrl>();
			this.DropItemBox = new List<PguiReplaceSpriteCtrl>();
			this.DropItemAE = new List<PguiAECtrl>();
			transform = baseTr.Find("BoardAll/Page02/ItemInfo/ScrollView/Viewport/Content/ItemIconAll");
			this.ScrollContent = baseTr.Find("BoardAll/Page02/ItemInfo/ScrollView/Viewport/Content").GetComponent<RectTransform>();
			this.ScrollBar = baseTr.Find("BoardAll/Page02/ItemInfo/ScrollView/Scrollbar_Vertical");
			foreach (object obj in transform)
			{
				Transform transform3 = (Transform)obj;
				this.DropItem.Add(transform3.gameObject);
				this.DropItemBonus.Add(transform3.Find("Icon_Item/Num_EventBonus").GetComponent<PguiTextCtrl>());
				this.DropItemBox.Add(transform3.Find("Icon_TresureBox").GetComponent<PguiReplaceSpriteCtrl>());
				this.DropItemAE.Add(transform3.Find("AEImage_ItemOpen").GetComponent<PguiAECtrl>());
				transform3.gameObject.SetActive(false);
			}
			transform = baseTr.Find("BoardAll/Page02/GoldInfo");
			ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(30101);
			transform.Find("Txt_Gold").GetComponent<PguiTextCtrl>().text = "獲得" + itemStaticBase.GetName();
			this.ownCoin = transform.Find("ItemOwn/Num").GetComponent<PguiTextCtrl>();
			this.getCoin = transform.Find("Num_Gold").GetComponent<PguiTextCtrl>();
			this.BoardPvp = baseTr.Find("BoardAll_PvP").GetComponent<SimpleAnimation>();
			this.pvpExp = new SceneBattleResult.GUI.PvpExp(this.BoardPvp.transform.Find("Page01/PvPExp"));
			this.pvpPoint3x = this.BoardPvp.transform.Find("Page01/PvPExp/PvPMode_x3").gameObject;
			this.pvpCoin = this.BoardPvp.transform.Find("Page01/PvPCoin/Num_PvPCoin").GetComponent<PguiTextCtrl>();
			this.pvpCoin3x = this.BoardPvp.transform.Find("Page01/PvPCoin/PvPMode_x3").gameObject;
			this.pvpCoinChamp = this.BoardPvp.transform.Find("Page01/PvPCoin/PvPMode_Champ").gameObject;
			this.BoardPvpTraining = baseTr.Find("BoardAll_PvP_Event").GetComponent<SimpleAnimation>();
			this.pvpExpTraining = new SceneBattleResult.GUI.PvpExp(this.BoardPvpTraining.transform.Find("Page01/PvPExp"));
			this.pvpPoint3xTraining = this.BoardPvpTraining.transform.Find("Page01/PvPExp/PvPMode_x3").gameObject;
			this.pvpCoinTraining = this.BoardPvpTraining.transform.Find("Page01/PvPCoin/Num_PvPCoin").GetComponent<PguiTextCtrl>();
			this.pvpCoin3xTraining = this.BoardPvpTraining.transform.Find("Page01/PvPCoin/PvPMode_x3").gameObject;
			this.BoardTraining = baseTr.Find("BoardAll_Training").GetComponent<SimpleAnimation>();
			this.TrainingScore = this.BoardTraining.transform.Find("Page01/Score/ScoreAll").GetComponent<HorizontalLayoutGroup>();
			this.TrainingNewRecord = this.BoardTraining.transform.Find("Page01/Score/AEImage_NewRecord").GetComponent<PguiAECtrl>();
			this.RewardItem = new List<GameObject>();
			transform = baseTr.Find("BoardAll_Training/Page01/GetItem/Item_All");
			int num2 = 1;
			for (;;)
			{
				Transform transform4 = transform.Find("Tex_Item" + num2.ToString("D2"));
				if (transform4 == null)
				{
					break;
				}
				this.RewardItem.Add(transform4.gameObject);
				num2++;
			}
			this.Rematch = baseTr.Find("Btn").GetComponent<SimpleAnimation>();
			this.RematchBtn = this.Rematch.transform.Find("Btn_Again").GetComponent<PguiButtonCtrl>();
			this.SkipGroup = baseTr.Find("SkipGroup").GetComponent<SimpleAnimation>();
			this.SkipBtn = this.SkipGroup.transform.Find("Btn_QuestSkip").GetComponent<PguiButtonCtrl>();
			this.QuestSkipPopup = this.SkipBtn.transform.Find("PopupParent/Popup_Campaign_CmnRed").gameObject;
		}

		// Token: 0x04003AFB RID: 15099
		public Transform Touch;

		// Token: 0x04003AFC RID: 15100
		public SimpleAnimation BoardNorm;

		// Token: 0x04003AFD RID: 15101
		public PguiTextCtrl Quest;

		// Token: 0x04003AFE RID: 15102
		public PguiTextCtrl QuestNum;

		// Token: 0x04003AFF RID: 15103
		public PguiTextCtrl Turn;

		// Token: 0x04003B00 RID: 15104
		public PguiReplaceSpriteCtrl UserIcon;

		// Token: 0x04003B01 RID: 15105
		public PguiTextCtrl Rank;

		// Token: 0x04003B02 RID: 15106
		public PguiAECtrl RankAE;

		// Token: 0x04003B03 RID: 15107
		public Transform Exp;

		// Token: 0x04003B04 RID: 15108
		public Image ExpGage;

		// Token: 0x04003B05 RID: 15109
		public AEImage StampCompAE;

		// Token: 0x04003B06 RID: 15110
		public List<AEImage> StampAE;

		// Token: 0x04003B07 RID: 15111
		public Transform MissionItem;

		// Token: 0x04003B08 RID: 15112
		public PguiAECtrl MissionItemAE;

		// Token: 0x04003B09 RID: 15113
		public List<GameObject> DropItem;

		// Token: 0x04003B0A RID: 15114
		public List<PguiTextCtrl> DropItemBonus;

		// Token: 0x04003B0B RID: 15115
		public List<PguiReplaceSpriteCtrl> DropItemBox;

		// Token: 0x04003B0C RID: 15116
		public List<PguiAECtrl> DropItemAE;

		// Token: 0x04003B0D RID: 15117
		public PguiTextCtrl ownCoin;

		// Token: 0x04003B0E RID: 15118
		public PguiTextCtrl getCoin;

		// Token: 0x04003B0F RID: 15119
		public SimpleAnimation BoardPvp;

		// Token: 0x04003B10 RID: 15120
		public SceneBattleResult.GUI.PvpExp pvpExp;

		// Token: 0x04003B11 RID: 15121
		public GameObject pvpPoint3x;

		// Token: 0x04003B12 RID: 15122
		public PguiTextCtrl pvpCoin;

		// Token: 0x04003B13 RID: 15123
		public GameObject pvpCoin3x;

		// Token: 0x04003B14 RID: 15124
		public GameObject pvpCoinChamp;

		// Token: 0x04003B15 RID: 15125
		public SimpleAnimation BoardPvpTraining;

		// Token: 0x04003B16 RID: 15126
		public SceneBattleResult.GUI.PvpExp pvpExpTraining;

		// Token: 0x04003B17 RID: 15127
		public GameObject pvpPoint3xTraining;

		// Token: 0x04003B18 RID: 15128
		public PguiTextCtrl pvpCoinTraining;

		// Token: 0x04003B19 RID: 15129
		public GameObject pvpCoin3xTraining;

		// Token: 0x04003B1A RID: 15130
		public SimpleAnimation BoardTraining;

		// Token: 0x04003B1B RID: 15131
		public HorizontalLayoutGroup TrainingScore;

		// Token: 0x04003B1C RID: 15132
		public PguiAECtrl TrainingNewRecord;

		// Token: 0x04003B1D RID: 15133
		public List<GameObject> RewardItem;

		// Token: 0x04003B1E RID: 15134
		public SimpleAnimation Rematch;

		// Token: 0x04003B1F RID: 15135
		public PguiButtonCtrl RematchBtn;

		// Token: 0x04003B20 RID: 15136
		public SimpleAnimation SkipGroup;

		// Token: 0x04003B21 RID: 15137
		public PguiButtonCtrl SkipBtn;

		// Token: 0x04003B22 RID: 15138
		public GameObject QuestSkipPopup;

		// Token: 0x04003B23 RID: 15139
		public RectTransform ScrollContent;

		// Token: 0x04003B24 RID: 15140
		public Transform ScrollBar;

		// Token: 0x0200114B RID: 4427
		public class PvpExp
		{
			// Token: 0x06005590 RID: 21904 RVA: 0x0024F354 File Offset: 0x0024D554
			public PvpExp(Transform baseTr)
			{
				this.pvpRank = baseTr.Find("Img_PvPBadge/Txt_PvPRank").GetComponent<PguiTextCtrl>();
				this.pvpPoint = baseTr.Find("Num_Point").GetComponent<PguiTextCtrl>();
				this.pvpPointNext = baseTr.Find("Num_Point_Next").GetComponent<PguiTextCtrl>();
				this.pvpPointBase = baseTr.Find("PointInfo01/Num_Point").GetComponent<PguiTextCtrl>();
				this.pvpPointBonus = baseTr.Find("PointInfo02/Num_Point").GetComponent<PguiTextCtrl>();
				this.pvpPointTurn = baseTr.Find("PointInfo03/Num_Point").GetComponent<PguiTextCtrl>();
				this.pvpPointVs = baseTr.Find("PointInfo04/Num_Point").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x04005F04 RID: 24324
			public PguiTextCtrl pvpRank;

			// Token: 0x04005F05 RID: 24325
			public PguiTextCtrl pvpPoint;

			// Token: 0x04005F06 RID: 24326
			public PguiTextCtrl pvpPointNext;

			// Token: 0x04005F07 RID: 24327
			public PguiTextCtrl pvpPointBase;

			// Token: 0x04005F08 RID: 24328
			public PguiTextCtrl pvpPointBonus;

			// Token: 0x04005F09 RID: 24329
			public PguiTextCtrl pvpPointTurn;

			// Token: 0x04005F0A RID: 24330
			public PguiTextCtrl pvpPointVs;
		}
	}

	// Token: 0x02000909 RID: 2313
	public class GUIChara
	{
		// Token: 0x06003A7A RID: 14970 RVA: 0x001CF110 File Offset: 0x001CD310
		public GUIChara(Transform baseTr, bool isTraining = false)
		{
			this.Blank = new List<GameObject>();
			this.Chara = new List<GameObject>();
			this.Helper = new List<Transform>();
			this.CharaExp = new List<PguiTextCtrl>();
			this.CharaLv = new List<PguiTextCtrl>();
			this.CharaExpGage = new List<Image>();
			this.LeftExp = new List<PguiTextCtrl>();
			this.LeftKizunaExp = new List<PguiTextCtrl>();
			this.CharaHrt = new List<PguiTextCtrl>();
			this.CharaKz = new List<PguiTextCtrl>();
			this.CharaHrtGage = new List<Image>();
			this.CharaAE = new List<PguiAECtrl>();
			this.KizunaAE = new List<PguiAECtrl>();
			this.CharaIcon = new List<IconCharaCtrl>();
			this.LevelMax = new List<AEImage>();
			this.KizunaMax = new List<AEImage>();
			int num = 1;
			for (;;)
			{
				Transform transform = baseTr.Find("Img_Blank" + num.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				this.Blank.Add(transform.gameObject);
				num++;
			}
			int num2 = 1;
			for (;;)
			{
				Transform transform2 = baseTr.Find("Icon_CharaSet" + num2.ToString("D2"));
				if (transform2 == null)
				{
					break;
				}
				this.Chara.Add(transform2.gameObject);
				this.Helper.Add(transform2.Find("Mark_Friend"));
				this.CharaAE.Add(transform2.Find("AEImage_LevelUp").GetComponent<PguiAECtrl>());
				this.CharaExp.Add(transform2.Find("ExpGage/Num_GetExp").GetComponent<PguiTextCtrl>());
				PguiTextCtrl component = transform2.Find("ExpGage/Num_Lv").GetComponent<PguiTextCtrl>();
				component.gameObject.SetActive(!isTraining);
				this.CharaLv.Add(component);
				this.CharaExpGage.Add(transform2.Find("ExpGage/ExpGage_Gage").GetComponent<PguiImageCtrl>().m_Image);
				this.LeftExp.Add(transform2.Find("ExpGage/Num_LeftExp").GetComponent<PguiTextCtrl>());
				this.LeftKizunaExp.Add(transform2.Find("ExpGage_Heart/Num_LeftExp").GetComponent<PguiTextCtrl>());
				this.KizunaAE.Add(transform2.Find("AEImage_CharaHeartlUp").GetComponent<PguiAECtrl>());
				this.CharaHrt.Add(transform2.Find("ExpGage_Heart/Num_GetExp").GetComponent<PguiTextCtrl>());
				this.CharaKz.Add(transform2.Find("ExpGage_Heart/Num_Lv").GetComponent<PguiTextCtrl>());
				this.CharaHrtGage.Add(transform2.Find("ExpGage_Heart/HeartGage_Gage").GetComponent<PguiImageCtrl>().m_Image);
				IconCharaCtrl component2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara).GetComponent<IconCharaCtrl>();
				component2.transform.SetParent(transform2.Find("Icon_Chara"), false);
				this.CharaIcon.Add(component2);
				this.LevelMax.Add(transform2.Find("ExpGage/AEImage_Max").GetComponent<AEImage>());
				this.KizunaMax.Add(transform2.Find("ExpGage_Heart/AEImage_Max").GetComponent<AEImage>());
				num2++;
			}
		}

		// Token: 0x04003B25 RID: 15141
		public List<GameObject> Blank;

		// Token: 0x04003B26 RID: 15142
		public List<GameObject> Chara;

		// Token: 0x04003B27 RID: 15143
		public List<Transform> Helper;

		// Token: 0x04003B28 RID: 15144
		public List<PguiTextCtrl> CharaExp;

		// Token: 0x04003B29 RID: 15145
		public List<PguiTextCtrl> CharaLv;

		// Token: 0x04003B2A RID: 15146
		public List<Image> CharaExpGage;

		// Token: 0x04003B2B RID: 15147
		public List<PguiTextCtrl> LeftExp;

		// Token: 0x04003B2C RID: 15148
		public List<PguiTextCtrl> LeftKizunaExp;

		// Token: 0x04003B2D RID: 15149
		public List<PguiTextCtrl> CharaHrt;

		// Token: 0x04003B2E RID: 15150
		public List<PguiTextCtrl> CharaKz;

		// Token: 0x04003B2F RID: 15151
		public List<Image> CharaHrtGage;

		// Token: 0x04003B30 RID: 15152
		public List<PguiAECtrl> CharaAE;

		// Token: 0x04003B31 RID: 15153
		public List<PguiAECtrl> KizunaAE;

		// Token: 0x04003B32 RID: 15154
		public List<IconCharaCtrl> CharaIcon;

		// Token: 0x04003B33 RID: 15155
		public List<AEImage> LevelMax;

		// Token: 0x04003B34 RID: 15156
		public List<AEImage> KizunaMax;
	}
}
