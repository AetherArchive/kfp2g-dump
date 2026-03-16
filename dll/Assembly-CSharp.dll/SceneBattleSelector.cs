using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.ImageEffects;

public class SceneBattleSelector : BaseScene
{
	public SceneBattleSelector.Args BsArgs
	{
		get
		{
			return this.bsArgs;
		}
	}

	public override void OnCreateScene()
	{
		this.basePanel = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneBattleSelector/GUI/Prefab/GUI_BattleSelector"));
		this.selBattleHelperCtrl = this.basePanel.AddComponent<SelBattleHelperCtrl>();
		this.selBattleHelperCtrl.Init(delegate(SceneManager.SceneName callback)
		{
			this.requestNextScene = callback;
			SceneProfile.Args args = new SceneProfile.Args
			{
				isFromBattleSelecter = true,
				setActiveQuestMapDataCB = this.bsArgs.setActiveQuestMapDataCB
			};
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, args);
		});
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
	}

	public override void OnEnableScene(object args)
	{
		this.friendSelectInternalCoroutine = null;
		if (args is SceneBattleSelector.Args)
		{
			this.bsArgs = args as SceneBattleSelector.Args;
		}
		if (this.bsArgs.selectQuestOneId == 0)
		{
			this.bsArgs.selectQuestOneId = this.selectQuestOneId;
		}
		else
		{
			this.selectQuestOneId = this.bsArgs.selectQuestOneId;
		}
		SceneQuest.DefaultPlayBGM();
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.bsArgs.selectQuestOneId);
		if (questOnePackData != null)
		{
			QuestStaticChapter questChapter = questOnePackData.questChapter;
			if (this.bsArgs.recordCameSceneName == SceneManager.SceneName.SceneProfile)
			{
				this.IEHelperChange = this.selBattleHelperCtrl.OnClickButtonHelperChangeCoroutine();
			}
			if (SceneQuest.IsMainStory(questChapter.category))
			{
				if (this.bsArgs.recordCameSceneName == SceneManager.SceneName.SceneCharaEdit || this.bsArgs.recordCameSceneName == SceneManager.SceneName.SceneProfile)
				{
					UnityAction<bool> setActiveQuestMapDataCB = this.bsArgs.setActiveQuestMapDataCB;
					if (setActiveQuestMapDataCB != null)
					{
						setActiveQuestMapDataCB(true);
					}
				}
				SceneQuest.MainStoryPlayBGM();
			}
			else if (questChapter.category == QuestStaticChapter.Category.EVENT)
			{
				DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(QuestUtil.GetEventId(this.bsArgs.selectQuestOneId, false));
				if (eventData != null)
				{
					switch (eventData.eventCategory)
					{
					case DataManagerEvent.Category.Large:
						if (this.bsArgs.recordCameSceneName == SceneManager.SceneName.SceneCharaEdit || this.bsArgs.recordCameSceneName == SceneManager.SceneName.SceneProfile)
						{
							UnityAction<bool> setActiveQuestMapDataCB2 = this.bsArgs.setActiveQuestMapDataCB;
							if (setActiveQuestMapDataCB2 != null)
							{
								setActiveQuestMapDataCB2(true);
							}
						}
						SelEventLargeScaleCtrl.PlayBGM(eventData.eventId);
						break;
					case DataManagerEvent.Category.Tower:
						SelEventTowerCtrl.PlayBGM();
						break;
					case DataManagerEvent.Category.Coop:
						Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.BACK).GetComponent<Blur>().enabled = true;
						SelEventCoopCtrl.PlayBGM();
						break;
					case DataManagerEvent.Category.WildRelease:
						SelEventWildReleaseCtrl.PlayBGM();
						break;
					}
				}
			}
			else if (questChapter.category == QuestStaticChapter.Category.TRAINING)
			{
				SceneTraining.PlayBGM();
			}
		}
		if (this.bsArgs.detailCharaId == 0 && this.bsArgs.detailPhotoId == 0L && this.bsArgs.detailAccesssoryId == 0L)
		{
			this.selectHelper = null;
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("助っ人選択"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		}
		this.sceneBattleArg = new SceneBattleArgs();
		this.IERequestBattleStart = null;
		QuestUtil.SetupBG(this.bsArgs.selectQuestOneId, QuestStaticChapter.Category.INVALID, 0);
		if (this.bsArgs.tutorialSequence == TutorialUtil.Sequence.INVALID)
		{
			DataManager.DmHelper.RequestGetRentalHelper(this.bsArgs.selectQuestOneId, false);
		}
		this.basePanel.gameObject.SetActive(false);
		this.requestNextScene = SceneManager.SceneName.None;
	}

	public override bool OnEnableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	public override void OnStartSceneFade()
	{
		if (this.bsArgs.detailCharaId != 0 || this.bsArgs.detailPhotoId != 0L || this.bsArgs.detailAccesssoryId != 0L)
		{
			if (this.bsArgs.detailCharaId != 0)
			{
				CanvasManager.HdlCharaWindowCtrl.OpenPrev();
			}
			if (this.bsArgs.detailPhotoId != 0L)
			{
				CanvasManager.HdlPhotoWindowCtrl.OpenPrev();
			}
			if (this.bsArgs.detailAccesssoryId != 0L)
			{
				CanvasManager.HdlAccessoryWindowCtrl.OpenPrev();
			}
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("パーティ選択"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
			this.basePanel.SetActive(false);
			CanvasManager.HdlSelCharaDeck.SetActive(true, false);
			CanvasManager.HdlSelCharaDeck.Setup(this.setupParam, (this.bsArgs != null) ? this.bsArgs.selectQuestOneId : (-1));
			return;
		}
		this.basePanel.gameObject.SetActive(true);
		this.selBattleHelperCtrl.Setup(this.bsArgs.selectQuestOneId, new SelBattleHelperCtrl.OnFriendSelect(this.OnFriendSelect), this.bsArgs.tutorialSequence > TutorialUtil.Sequence.INVALID);
		CanvasManager.HdlSelCharaDeck.SetActive(false, false);
		if (this.bsArgs.tutorialSequence != TutorialUtil.Sequence.INVALID)
		{
			Singleton<SceneManager>.Instance.StartCoroutine(this.TutorialInternal());
		}
	}

	private IEnumerator FriendSelectInternal(HelperPackData helperPackData, int attrIndex)
	{
		this.selBattleHelperCtrl.PlayStartAnim(false);
		while (this.selBattleHelperCtrl.IsPlayingAnim())
		{
			yield return null;
		}
		CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("パーティ選択"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		this.basePanel.SetActive(false);
		CanvasManager.HdlSelCharaDeck.SetActive(true, false);
		this.setupParam = new SelCharaDeckCtrl.SetupParam
		{
			callScene = SceneManager.SceneName.SceneBattleSelector,
			callbackGotoBattle = new SelCharaDeckCtrl.OnClickGotoBattle(this.OnClickBattleButton),
			helperPackData = helperPackData,
			attrIndex = attrIndex
		};
		CanvasManager.HdlSelCharaDeck.Setup(this.setupParam, (this.bsArgs != null) ? this.bsArgs.selectQuestOneId : (-1));
		this.selectHelper = helperPackData;
		this.selectAttrIndex = attrIndex;
		if (this.bsArgs.tutorialSequence != TutorialUtil.Sequence.INVALID)
		{
			Singleton<SceneManager>.Instance.StartCoroutine(this.TutorialInternal2());
			TutorialUtil.SetHelper(helperPackData.HelperCharaSetList[attrIndex].helpChara.id);
		}
		this.friendSelectInternalCoroutine = null;
		this.SetupCaution();
		yield break;
	}

	private void SetupCaution()
	{
		string text = "";
		string text2 = "";
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.bsArgs.selectQuestOneId);
		if (questOnePackData != null && questOnePackData.questOne != null)
		{
			if (questOnePackData.questOne.QuestCategory == QuestStaticQuestOne.QuestOneCategory.NoPlayer)
			{
				text = "このバトルには隊長が参加できません";
				text2 = "\u3000・バトルはオートで進行します\n\u3000・フレンズはランダムで行動します\n\u3000・ターゲットを指示できません\n\u3000・隊長スキルが使用できません\n\u3000・おかわりできません\n\u3000・たいきスキルのタイミングを指示できません";
			}
			else if (questOnePackData.questOne.QuestCategory == QuestStaticQuestOne.QuestOneCategory.NoDhole)
			{
				text = "このバトルにはドールが参加できません";
				text2 = "\u3000・編成されているドールはバトルに参加しません\n\u3000・バトル開始時、みんなが「からげんき」状態になります";
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			CanvasManager.HdlOpenWindowPartyCaution.Setup(text, text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, (int idx) => true, null, false);
			CanvasManager.HdlOpenWindowPartyCaution.ForceOpen();
			SoundManager.Play("prd_se_quest_warning", false, false);
		}
	}

	public void OnFriendSelect(HelperPackData helperPackData, int attrIndex)
	{
		if (this.selBattleHelperCtrl.IsPlayingAnim())
		{
			return;
		}
		if (this.friendSelectInternalCoroutine != null)
		{
			return;
		}
		this.friendSelectInternalCoroutine = Singleton<SceneManager>.Instance.StartCoroutine(this.FriendSelectInternal(helperPackData, attrIndex));
	}

	private IEnumerator TutorialInternal()
	{
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(true);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(true);
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.OUT_QUICK
		});
		while (this.selBattleHelperCtrl.IsPlayingAnim())
		{
			yield return null;
		}
		if (this.bsArgs.tutorialSequence == TutorialUtil.Sequence.HELPER_SELECT_00)
		{
			SceneBattleSelector.<>c__DisplayClass26_0 CS$<>8__locals1 = new SceneBattleSelector.<>c__DisplayClass26_0();
			Transform panel = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneBattle/GUI/Prefab/Tutorial_alpha")).transform;
			SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, panel, true);
			panel.SetAsLastSibling();
			panel.Find("Image").GetComponent<PguiRawImageCtrl>().SetRawImage("Texture2D/Tutorial/Battle/tutorial_battle_07", true, false, null);
			panel.Find("Btn_Skip").gameObject.SetActive(false);
			yield return null;
			CS$<>8__locals1.touch = 0f;
			panel.Find("Image/TouchCollision").gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
			{
				if (CS$<>8__locals1.touch > 0.5f)
				{
					CS$<>8__locals1.touch = -1f;
				}
			}, null, null, null, null);
			while (CS$<>8__locals1.touch >= 0f)
			{
				CS$<>8__locals1.touch += TimeManager.DeltaTime;
				yield return null;
			}
			Object.Destroy(panel.gameObject);
			CS$<>8__locals1 = null;
			panel = null;
		}
		Transform transform = this.basePanel.transform;
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, true, new Rect?(new Rect(495f, 10f, 730f, 500f)), true, null);
		yield break;
	}

	private IEnumerator TutorialInternal2()
	{
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		yield return null;
		while (CanvasManager.HdlSelCharaDeck.IsPlayingAnimCharaDeckEditToBattleBtn())
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, CanvasManager.HdlSelCharaDeck.GetCharaDeckEditToBattleBtnRectTransform(), true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(CanvasManager.HdlSelCharaDeck.GetCharaDeckEditToBattleBtnRectTransform(), 1f);
		yield break;
	}

	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.friendSelectInternalCoroutine == null && this.IERequestBattleStart == null && this.IEMenuRetrunInternal == null && this.IEMenuRetrunInternal2 == null && CanvasManager.HdlSelCharaDeck.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	private IEnumerator MenuRetrunInternal()
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("助っ人選択"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		CanvasManager.HdlSelCharaDeck.gameObject.SetActive(false);
		this.basePanel.SetActive(true);
		yield return null;
		this.selBattleHelperCtrl.PlayStartAnim(true);
		this.selBattleHelperCtrl.UpdateCampaign(null);
		while (this.selBattleHelperCtrl.IsPlayingAnim())
		{
			yield return null;
		}
		this.IEMenuRetrunInternal = null;
		yield break;
	}

	private IEnumerator MenuRetrunInternal2()
	{
		if (this.bsArgs.menuBackSceneName != SceneManager.SceneName.None)
		{
			this.requestNextScene = this.bsArgs.menuBackSceneName;
			this.nextSceneArgs = this.bsArgs.menuBackSceneArgs;
		}
		else
		{
			this.requestNextScene = SceneManager.SceneName.SceneQuest;
			this.nextSceneArgs = new SceneQuest.Args
			{
				selectQuestOneId = this.bsArgs.selectQuestOneId,
				recordCameSceneName = SceneManager.SceneName.SceneBattleSelector
			};
		}
		this.selBattleHelperCtrl.PlayStartAnim(false);
		while (this.selBattleHelperCtrl.IsPlayingAnim())
		{
			yield return null;
		}
		this.IEMenuRetrunInternal2 = null;
		yield break;
	}

	private void OnClickButtonMenuRetrun()
	{
		if (this.friendSelectInternalCoroutine != null)
		{
			return;
		}
		if (this.IERequestBattleStart != null)
		{
			return;
		}
		if (this.IEMenuRetrunInternal != null || this.IEMenuRetrunInternal2 != null)
		{
			return;
		}
		if (CanvasManager.HdlSelCharaDeck.gameObject.activeSelf)
		{
			if (!CanvasManager.HdlSelCharaDeck.OnClickMenuReturn(delegate
			{
			}))
			{
				this.IEMenuRetrunInternal = this.MenuRetrunInternal();
				return;
			}
		}
		else
		{
			this.IEMenuRetrunInternal2 = this.MenuRetrunInternal2();
		}
	}

	private bool OnClickBattleButton()
	{
		if (this.IERequestBattleStart != null)
		{
			return false;
		}
		UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
		userOptionData.CurrentQuestParty = CanvasManager.HdlSelCharaDeck.GetDeckId();
		DataManager.DmUserInfo.RequestActionUpdateUserOption(userOptionData);
		this.IERequestBattleStart = this.IERequestBattleStart ?? this.RequestBattleStart();
		return true;
	}

	private IEnumerator RequestBattleStart()
	{
		if (this.bsArgs.tutorialSequence != TutorialUtil.Sequence.INVALID)
		{
			this.requestNextScene = SceneManager.SceneName.SceneBattle;
			yield break;
		}
		SelCharaDeckCtrl.EditResultData erd = CanvasManager.HdlSelCharaDeck.GetEditResultData();
		QuestOnePackData qopd = DataManager.DmQuest.GetQuestOnePackData(this.bsArgs.selectQuestOneId);
		if (qopd.questOne.QuestCategory == QuestStaticQuestOne.QuestOneCategory.NoDhole)
		{
			UserDeckData userDeckData = erd.editDeck.Find((UserDeckData itm) => itm.id == erd.currentDeckId);
			if (userDeckData != null)
			{
				bool flag = false;
				foreach (int num in userDeckData.charaIdList)
				{
					if (num != -1 && num != 0 && !DataManager.DmChara.CheckSameChara(num, 1))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					CanvasManager.HdlOpenWindowBasic.Setup("エラー", "バトルに参加可能なフレンズを編成してください", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int idx) => true, null, false);
					CanvasManager.HdlOpenWindowBasic.ForceOpen();
					do
					{
						yield return null;
					}
					while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
					CanvasManager.HdlSelCharaDeck.CancelBattleStart();
					this.IERequestBattleStart = null;
					yield break;
				}
			}
		}
		DataManagerEvent.EventData evtDat = ((qopd == null) ? null : DataManager.DmEvent.GetEventDataList().Find((DataManagerEvent.EventData itm) => itm.eventChapterId == qopd.questChapter.chapterId));
		int mapId = DataManager.DmQuest.GetQuestOnePackData(this.bsArgs.selectQuestOneId).questMap.mapId;
		if (DataManager.DmEvent.isRaidByMapId(mapId))
		{
			List<int> oldDrawIdList = this.GetConvertDrawId(this.bsArgs.selectQuestOneId);
			TimeSpan oldStartTime = DataManager.DmEvent.GetTermData(evtDat.eventId, DataManager.DmEvent.LastCoopInfo.InfoGetDateTime).startTime;
			DataManager.DmEvent.RequestGetCoopInfo(DataManager.DmEvent.LastCoopInfo.EventId, mapId);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			List<int> convertDrawId = this.GetConvertDrawId(this.bsArgs.selectQuestOneId);
			TimeSpan startTime = DataManager.DmEvent.GetTermData(evtDat.eventId, DataManager.DmEvent.LastCoopInfo.InfoGetDateTime).startTime;
			if (oldStartTime != startTime)
			{
				CanvasManager.HdlOpenWindowBasic.Setup("確認", "開催期間を過ぎています", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneQuest, null);
				yield break;
			}
			oldDrawIdList.Sort();
			convertDrawId.Sort();
			if (!oldDrawIdList.SequenceEqual<int>(convertDrawId))
			{
				CanvasManager.HdlOpenWindowBasic.Setup("確認", "更新データが見つかりました\nデータ更新を行います", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				SceneQuest.Args args = new SceneQuest.Args
				{
					selectEventId = DataManager.DmEvent.LastCoopInfo.EventId,
					category = QuestStaticChapter.Category.EVENT
				};
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneQuest, args);
				yield break;
			}
			oldDrawIdList = null;
			oldStartTime = default(TimeSpan);
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (qopd.questOne.useItemId > 0)
		{
			ItemData userItemData = DataManager.DmItem.GetUserItemData(qopd.questOne.useItemId);
			if (userItemData.num < qopd.questOne.useItemNum)
			{
				string text = userItemData.staticData.GetName() + "が不足しています\n\n必要数\u3000" + qopd.questOne.useItemNum.ToString();
				CanvasManager.HdlOpenWindowBasic.Setup("確認", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int idx) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.ForceOpen();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				CanvasManager.HdlSelCharaDeck.CancelBattleStart();
				this.IERequestBattleStart = null;
				yield break;
			}
		}
		else
		{
			StaminaRecoveryWindowCtrl hdlStaminaRecoveryWindowCtrl = CanvasManager.HdlStaminaRecoveryWindowCtrl;
			IEnumerator checkStamina = hdlStaminaRecoveryWindowCtrl.StaminaCheckAction(this.bsArgs.selectQuestOneId, 1);
			bool staminaChkResult = false;
			while (checkStamina.MoveNext())
			{
				staminaChkResult = checkStamina.Current != null && (bool)checkStamina.Current;
				yield return null;
			}
			if (!staminaChkResult)
			{
				CanvasManager.HdlSelCharaDeck.CancelBattleStart();
				this.IERequestBattleStart = null;
				yield break;
			}
			checkStamina = null;
		}
		List<long> list = null;
		if (this.selectHelper.HelperCharaSetList[this.selectAttrIndex].helpPhotoList != null && this.selectHelper.HelperCharaSetList[this.selectAttrIndex].helpPhotoList.Count > 0)
		{
			list = new List<long>();
			foreach (PhotoPackData photoPackData in this.selectHelper.HelperCharaSetList[this.selectAttrIndex].helpPhotoList)
			{
				list.Add((photoPackData != null) ? photoPackData.dataId : 0L);
			}
		}
		DataManager.DmQuest.RequestActionBattleStart(this.bsArgs.selectQuestOneId, erd.currentDeckId, this.selectHelper.friendId, this.selectHelper.HelperCharaSetList[this.selectAttrIndex].helpChara.id, list, this.BsArgs.coopLastUpdatePoint);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		bool flag2 = !DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(this.bsArgs.selectQuestOneId) || DataManager.DmQuest.QuestDynamicData.oneDataMap[this.bsArgs.selectQuestOneId].playNum == 0;
		bool flag3 = !DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(this.bsArgs.selectQuestOneId) || DataManager.DmQuest.QuestDynamicData.oneDataMap[this.bsArgs.selectQuestOneId].clearNum == 0;
		this.sceneBattleArg.oppUser = null;
		this.sceneBattleArg.difficulty = PvpDynamicData.EnemyInfo.Difficulty.INVALID;
		this.sceneBattleArg.pvpBoard = null;
		this.sceneBattleArg.hash_id = DataManager.DmQuest.LastQuestStartResponse.hash_id;
		this.sceneBattleArg.questOneId = this.bsArgs.selectQuestOneId;
		this.sceneBattleArg.waveEnemiesIdList = DataManager.DmQuest.LastQuestStartResponse.waveEnemiesIdList;
		this.sceneBattleArg.dropItemList = DataManager.DmQuest.LastQuestStartResponse.drew_items;
		this.sceneBattleArg.startTime = DataManager.DmQuest.LastQuestStartResponse.startTime;
		this.sceneBattleArg.eventId = ((evtDat == null) ? 0 : evtDat.eventId);
		this.sceneBattleArg.selectDeckId = erd.currentDeckId;
		this.sceneBattleArg.helper = erd.helperData.helper;
		this.sceneBattleArg.attrIndex = this.selectAttrIndex;
		this.sceneBattleArg.resultNextSceneName = this.bsArgs.menuBackSceneName;
		if (this.bsArgs.menuBackSceneArgs != null)
		{
			if (this.bsArgs.menuBackSceneArgs.GetType() == this.bsArgs.GetType())
			{
				this.sceneBattleArg.resultNextSceneArgs = null;
			}
			else if (this.bsArgs.recordCameSceneName != SceneManager.SceneName.SceneCharaEdit && this.bsArgs.recordCameSceneName != SceneManager.SceneName.SceneQuest)
			{
				this.sceneBattleArg.resultNextSceneArgs = this.bsArgs.menuBackSceneArgs;
			}
		}
		else
		{
			this.sceneBattleArg.resultNextSceneArgs = this.bsArgs.menuBackSceneArgs;
		}
		this.sceneBattleArg.isQuestNoClear = flag3;
		this.sceneBattleArg.trainingTurn = 0;
		SceneBattle.SetRestart(this.sceneBattleArg);
		this.playScenarioName = ((flag2 || !DataManager.DmUserInfo.optionData.secondScenarioSkip) ? DataManager.DmQuest.QuestStaticData.oneDataMap[this.bsArgs.selectQuestOneId].scenarioBeforeId : "");
		this.requestNextScene = SceneManager.SceneName.SceneBattle;
		yield break;
	}

	public override void Update()
	{
		bool flag = true;
		if (this.IEHelperChange != null && !this.IEHelperChange.MoveNext())
		{
			this.IEHelperChange = null;
		}
		if (CanvasManager.HdlSelCharaDeck.gameObject.activeSelf && CanvasManager.HdlSelCharaDeck.ForceReturnTop())
		{
			this.IEMenuRetrunInternal = this.MenuRetrunInternal();
		}
		if (this.IERequestBattleStart != null)
		{
			this.IERequestBattleStart.MoveNext();
		}
		if (this.IEMenuRetrunInternal != null)
		{
			this.IEMenuRetrunInternal.MoveNext();
		}
		if (this.IEMenuRetrunInternal2 != null)
		{
			this.IEMenuRetrunInternal2.MoveNext();
		}
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			if (this.bsArgs.tutorialSequence != TutorialUtil.Sequence.INVALID)
			{
				TutorialUtil.RequestNextSequence(this.bsArgs.tutorialSequence);
			}
			else
			{
				if (this.requestNextScene == SceneManager.SceneName.SceneBattle)
				{
					if (this.playScenarioName != string.Empty)
					{
						SceneScenario.Args args = new SceneScenario.Args();
						args.scenarioName = this.playScenarioName;
						args.questId = this.bsArgs.selectQuestOneId;
						args.storyType = 1;
						args.nextSceneName = SceneManager.SceneName.SceneBattle;
						args.nextSceneArgs = this.sceneBattleArg;
						this.requestNextScene = SceneManager.SceneName.SceneScenario;
						this.nextSceneArgs = args;
					}
					else
					{
						this.nextSceneArgs = this.sceneBattleArg;
					}
				}
				Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.nextSceneArgs);
			}
			flag = false;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(flag, true);
	}

	public override void OnDisableScene()
	{
		if (this.bsArgs.tutorialSequence != TutorialUtil.Sequence.INVALID)
		{
			CanvasManager.HdlTutorialMaskCtrl.SetEnable(false);
			CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(false);
		}
		this.basePanel.gameObject.SetActive(false);
		CanvasManager.HdlSelCharaDeck.SetActive(false, false);
		if (this.requestNextScene != SceneManager.SceneName.SceneQuest)
		{
			if (Singleton<SceneManager>.Instance.RequestNextScene != SceneManager.SceneName.SceneCharaEdit && Singleton<SceneManager>.Instance.RequestNextScene != SceneManager.SceneName.SceneProfile)
			{
				UnityAction destroyQuestMapDataCB = this.bsArgs.destroyQuestMapDataCB;
				if (destroyQuestMapDataCB != null)
				{
					destroyQuestMapDataCB();
				}
			}
			else
			{
				UnityAction<bool> setActiveQuestMapDataCB = this.bsArgs.setActiveQuestMapDataCB;
				if (setActiveQuestMapDataCB != null)
				{
					setActiveQuestMapDataCB(false);
				}
			}
		}
		Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.BACK).GetComponent<Blur>().enabled = false;
		CanvasManager.DestoryBgTexture();
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private List<int> GetConvertDrawId(int questOneId)
	{
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(questOneId);
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

	private GameObject basePanel;

	private SceneBattleSelector.Args bsArgs;

	private SceneManager.SceneName requestNextScene;

	private SceneBattleArgs sceneBattleArg;

	private SelBattleHelperCtrl selBattleHelperCtrl;

	private object nextSceneArgs;

	private string playScenarioName;

	private HelperPackData selectHelper;

	private int selectAttrIndex;

	private Coroutine friendSelectInternalCoroutine;

	private int selectQuestOneId;

	private IEnumerator IERequestBattleStart;

	private IEnumerator IEMenuRetrunInternal;

	private IEnumerator IEMenuRetrunInternal2;

	private IEnumerator IEHelperChange;

	private SelCharaDeckCtrl.SetupParam setupParam = new SelCharaDeckCtrl.SetupParam();

	public class Args
	{
		public TutorialUtil.Sequence tutorialSequence;

		public int selectQuestOneId;

		public SceneManager.SceneName menuBackSceneName;

		public object menuBackSceneArgs;

		public int detailCharaId;

		public long detailPhotoId;

		public long detailAccesssoryId;

		public UnityAction destroyQuestMapDataCB;

		public SceneManager.SceneName recordCameSceneName;

		public UnityAction<bool> setActiveQuestMapDataCB;

		public long coopLastUpdatePoint;
	}
}
