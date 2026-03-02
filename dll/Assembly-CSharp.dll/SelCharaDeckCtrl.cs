using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AEAuth3;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001B8 RID: 440
public class SelCharaDeckCtrl : MonoBehaviour
{
	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x06001DF5 RID: 7669 RVA: 0x00174286 File Offset: 0x00172486
	// (set) Token: 0x06001DF6 RID: 7670 RVA: 0x0017428E File Offset: 0x0017248E
	private SelCharaDeckCtrl.Mode currentMode
	{
		get
		{
			return this._currentMode;
		}
		set
		{
			this._preMode = this._currentMode;
			this._currentMode = value;
		}
	}

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x001742A3 File Offset: 0x001724A3
	private SelCharaDeckCtrl.Mode preMode
	{
		get
		{
			return this._preMode;
		}
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x06001DF8 RID: 7672 RVA: 0x001742AB File Offset: 0x001724AB
	// (set) Token: 0x06001DF9 RID: 7673 RVA: 0x001742BA File Offset: 0x001724BA
	public bool TouchRect
	{
		get
		{
			bool flag = this.touchRect;
			this.touchRect = false;
			return flag;
		}
		private set
		{
			this.touchRect = value;
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x06001DFA RID: 7674 RVA: 0x001742C3 File Offset: 0x001724C3
	// (set) Token: 0x06001DFB RID: 7675 RVA: 0x001742D2 File Offset: 0x001724D2
	public bool AnimeFinished
	{
		get
		{
			bool flag = this.animeFinished;
			this.animeFinished = false;
			return flag;
		}
		private set
		{
			this.animeFinished = value;
		}
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x001742DB File Offset: 0x001724DB
	public int GetDeckId()
	{
		if (this.currentDeckClone != null)
		{
			return this.currentDeckClone.id;
		}
		return 0;
	}

	// Token: 0x06001DFD RID: 7677 RVA: 0x001742F2 File Offset: 0x001724F2
	public bool FinishedAnim()
	{
		return !this.guiData.charaDeck.DeckSelect.ExIsPlaying();
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x0017430C File Offset: 0x0017250C
	public bool IsPhotoEditMode()
	{
		return this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_EDIT;
	}

	// Token: 0x06001DFF RID: 7679 RVA: 0x00174318 File Offset: 0x00172518
	public void Setup(SelCharaDeckCtrl.SetupParam param, int _questOneId)
	{
		this.setupParam = param;
		int num;
		if (this.setupParam.deckCategory == UserDeckData.Category.TRAINING)
		{
			int trainingDay = (int)this.setupParam.trainingDay;
			num = ((trainingDay <= 0) ? 207 : (200 + trainingDay));
		}
		else if (this.setupParam.deckCategory == UserDeckData.Category.PVP)
		{
			PvpPackData pvpPackDataBySeasonID = DataManager.DmPvp.GetPvpPackDataBySeasonID(this.setupParam.pvpSeasonId);
			if (pvpPackDataBySeasonID != null && pvpPackDataBySeasonID.staticData.type == PvpStaticData.Type.SPECIAL)
			{
				num = DataManager.DmUserInfo.optionData.CurrentSpPvpParty;
			}
			else
			{
				num = DataManager.DmUserInfo.optionData.CurrentPvpParty;
			}
		}
		else
		{
			num = DataManager.DmUserInfo.optionData.CurrentQuestParty;
		}
		if (this.setupParam.isReload)
		{
			num = this.ReloadDeckData(num);
		}
		this.isEnemyInfo = _questOneId >= 0;
		this.questOneId = _questOneId;
		this.dayOfWeekData = null;
		if (!this.isEnemyInfo && this.setupParam.deckCategory == UserDeckData.Category.TRAINING && DataManager.DmTraining.GetTrainingPackData() != null)
		{
			TrainingStaticData staticData = DataManager.DmTraining.GetTrainingPackData().staticData;
			this.dayOfWeekData = (staticData.dayOfWeekDataList.ContainsKey(this.setupParam.trainingDay) ? staticData.dayOfWeekDataList[this.setupParam.trainingDay] : null);
			if (this.dayOfWeekData != null)
			{
				this.isEnemyInfo = true;
				this.questOneId = this.dayOfWeekData.questOneId;
			}
		}
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.questOneId);
		this.enemyAttribute = (this.isEnemyInfo ? questOnePackData.questOne.ennemyAttrMask : ((CharaDef.AttributeMask)0));
		this.helperPackData = this.setupParam.helperPackData;
		this.helperCharaData = ((this.setupParam.helperPackData != null && this.setupParam.helperPackData.HelperCharaSetList[this.setupParam.attrIndex].helpChara != null) ? this.setupParam.helperPackData.HelperCharaSetList[this.setupParam.attrIndex].helpChara : CharaPackData.MakeInvalid());
		this.guiData.charaDeck.questRuleInfo.buttonRuleInfo.gameObject.SetActive(false);
		this.guiData.charaDeck.questSealedInfo.gameObject.SetActive(false);
		if (questOnePackData != null && questOnePackData.questOne != null)
		{
			bool flag = questOnePackData.questOne.ruleId != 0;
			this.guiData.charaDeck.questRuleInfo.buttonRuleInfo.gameObject.SetActive(flag);
			if (flag)
			{
				this.guiData.charaDeck.questRuleInfo.Setup(questOnePackData.questOne.ruleId);
			}
		}
		this.guiData.charaDeck.questSealedInfo.gameObject.SetActive(false);
		if (questOnePackData != null && questOnePackData.questGroup != null && questOnePackData.questGroup.limitGroupFlag)
		{
			List<DataManagerQuest.QuestSealedCharaData> list = DataManager.DmQuest.QuestSealedCharaDatas.FindAll((DataManagerQuest.QuestSealedCharaData item) => item.target == questOnePackData.questGroup.questGroupId);
			this.guiData.charaDeck.questSealedInfo.gameObject.SetActive(list.Count != 0);
		}
		this.guiData.charaDeck.questSealedInfo.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			QuestUtil.OpenQuestSealedInfo(questOnePackData.questGroup.questGroupId).MoveNext();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.currentMode = SelCharaDeckCtrl.Mode.INVALID;
		this.currentMode = SelCharaDeckCtrl.Mode.DECK_TOP;
		this.SetupDeckTopAnimation();
		int num2 = 0;
		for (int i = 0; i < this.guiData.charaDeck.DeckTab.m_PguiTabList.Count; i++)
		{
			this.guiData.charaDeck.DeckTab.m_PguiTabList[i].SetChgMainSub(this.setupParam.deckCategory == UserDeckData.Category.PVP);
			this.SetTabName(i, (i < this.deckCloneList.Count) ? this.deckCloneList[i].name : (i + 1).ToString());
			if (i < this.deckCloneList.Count && this.deckCloneList[i].id == num)
			{
				num2 = i;
			}
			this.guiData.charaDeck.DeckTab.m_PguiTabList[i].gameObject.SetActive(i < this.deckCloneList.Count);
		}
		this.guiData.charaDeck.DeckTab.Setup(num2, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectDeckTab));
		this.guiData.charaDeck.PartyName.Replace((this.setupParam.deckCategory == UserDeckData.Category.PVP) ? 2 : 1);
		Transform transform = this.guiData.charaDeck.PartyName.transform.Find("Txt_PartyName");
		transform.GetComponent<PguiGradientCtrl>().SetGameObjectById((this.setupParam.deckCategory == UserDeckData.Category.PVP) ? "2" : "1");
		transform.GetComponent<PguiTextCtrl>().text = this.currentDeckClone.name;
		this.guiData.charaDeck.Btn_PartyName.gameObject.SetActive(this.setupParam.deckCategory != UserDeckData.Category.TRAINING);
		if (this.setupParam.callScene == SceneManager.SceneName.SceneBattleSelector || this.setupParam.callScene == SceneManager.SceneName.ScenePvpDeck || this.setupParam.callScene == SceneManager.SceneName.SceneTraining)
		{
			this.guiData.charaDeck.Btn_ToBattle.gameObject.SetActive(true);
			this.guiData.charaDeck.Btn_ToBattle.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = PrjUtil.MakeMessage((this.setupParam.callScene == SceneManager.SceneName.SceneBattleSelector || this.setupParam.callScene == SceneManager.SceneName.SceneTraining) ? "準備OK" : "決定");
			this.guiData.charaDeck.Anim_ToBattle.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		}
		else
		{
			this.guiData.charaDeck.Btn_ToBattle.gameObject.SetActive(false);
		}
		if (this.setupParam.callScene != SceneManager.SceneName.SceneBattleSelector)
		{
			this.guiData.charaDeck.Btn_QuestSkip.gameObject.SetActive(false);
		}
		this.guiData.charaDeck.DeckWindowImage.m_Image.color = ((this.setupParam.deckCategory == UserDeckData.Category.PVP) ? new Color(1f, 1f, 0.8f) : Color.white);
		for (int j = 0; j < this.currentDeckClone.charaIdList.Count; j++)
		{
			SelCharaDeckCtrl.GUI.IconChara iconChara = this.guiData.charaDeck.iconCharaPacks[j].iconChara;
			iconChara.PhotoIconView.SetActive(false);
			iconChara.AccessoryIconView.SetActive(false);
		}
		this.guiData.charaDeck.StaySkillSettingAnim.gameObject.SetActive(false);
		this.guiData.charaDeck.Btn_PhotoRemove.transform.parent.GetComponent<SimpleAnimation>().ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		this.guiData.charaDeck.Btn_PhotoRemove.transform.parent.gameObject.SetActive(false);
		this.ChangeDeckInfo(num);
		this.guiData.charaDeck.Btn_StaySkillSetting.gameObject.SetActive(true);
		this.skipInfo = new QuestUtil.UsrQuestSkipInfo();
		if (questOnePackData != null)
		{
			BattleMissionPack battleMissionPack = DataManager.DmQuest.GetBattleMissionPack(questOnePackData.questOne.questId);
			DataManagerMonthlyPack.PurchaseMonthlypackData validMonthlyPackData = DataManager.DmMonthlyPack.GetValidMonthlyPackData();
			bool flag2 = false;
			if ((questOnePackData.questOne.skippableFlag != QuestUtil.SkipType.Disable || questOnePackData.questGroup.SkippableFlag != QuestUtil.SkipType.Disable) && !battleMissionPack.clearFlag.Contains(false))
			{
				this.skipInfo = QuestUtil.GetSkipInfo(validMonthlyPackData, questOnePackData);
				if (!this.skipInfo.hasSkipLimit)
				{
					this.guiData.charaDeck.Popup_QuestSkip.SetActive(false);
				}
				else if (this.skipInfo.restSkipCount > 0)
				{
					this.SetQuestSkipPopup(this.skipInfo);
				}
				else
				{
					this.guiData.charaDeck.Popup_QuestSkip.SetActive(false);
					flag2 = this.skipInfo.restSkipRecoveryCount <= 0;
				}
			}
			this.guiData.charaDeck.Btn_QuestSkip.gameObject.SetActive(this.skipInfo.isSkippable);
			if (this.skipInfo.isSkippable)
			{
				this.guiData.charaDeck.Btn_QuestSkip.SetActEnable(!flag2, false, false);
			}
		}
		this.guiData.charaDeck.campaignInfo.go.SetActive(this.isEnemyInfo);
		if (this.isEnemyInfo)
		{
			QuestOnePackData questOnePackData4 = DataManager.DmQuest.GetQuestOnePackData(this.questOneId);
			this.guiData.charaDeck.ResetCampaignInfoCategory();
			this.guiData.charaDeck.UpdateCampaignInfoCategory(questOnePackData4.questChapter.category, questOnePackData4.questChapter.chapterId);
		}
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = ((this.setupParam.deckCategory == UserDeckData.Category.PVP) ? SortFilterDefine.RegisterType.CHARA_DECK_PVP : ((this.setupParam.deckCategory == UserDeckData.Category.TRAINING) ? SortFilterDefine.RegisterType.CHARA_DECK_TRAINING : SortFilterDefine.RegisterType.CHARA_DECK)),
			filterButton = this.guiData.charaDeck.Btn_FilterOnOff,
			sortButton = this.guiData.charaDeck.Btn_Sort,
			sortUdButton = this.guiData.charaDeck.Btn_SortUpDown,
			funcGetTargetBaseList = delegate
			{
				List<CharaPackData> list3 = new List<CharaPackData>();
				if (this.selectCharaData != null && this.selectCharaData.type == SelCharaDeckCtrl.FrameType.RESERVE)
				{
					list3.Add(this.selectCharaData.chara);
				}
				return new SortWindowCtrl.SortTarget
				{
					charaList = this.haveCharaPackList,
					disableFilterCharaList = list3
				};
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispCharaPackList = new List<CharaPackData>(item.charaList);
				this.dispCharaPackList.Insert(0, this.removeButttonCharaData);
				this.sortType = item.sortType;
				this.guiData.charaDeck.ResizeScrollView(this.dispCharaPackList.Count - 1, this.dispCharaPackList.Count / 2 + 1);
			}
		};
		List<CharaPackData> list2 = new List<CharaPackData>();
		QuestOnePackData questOnePackData2 = DataManager.DmQuest.GetQuestOnePackData(this.questOneId);
		foreach (CharaPackData charaPackData in this.dispCharaPackList)
		{
			if (QuestUtil.IsBanTarget(charaPackData.dynamicData, questOnePackData2, this.checkedCharaList))
			{
				list2.Add(charaPackData);
			}
		}
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, false, list2);
		RecommendedDeckWindowCtrl.RegisterData registerData2 = new RecommendedDeckWindowCtrl.RegisterData
		{
			filterButton = this.guiData.charaDeck.Btn_AutoDeck,
			funcGetTargetBaseList = delegate
			{
				if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP)
				{
					this.currentMode = SelCharaDeckCtrl.Mode.OW_RECOMMEND;
					RecommendedDeckWindowCtrl.SortTarget sortTarget = new RecommendedDeckWindowCtrl.SortTarget();
					List<CharaPackData> list4 = new List<CharaPackData>();
					QuestOnePackData questOnePackData3 = DataManager.DmQuest.GetQuestOnePackData(this.questOneId);
					if (questOnePackData3 != null && (questOnePackData3.questOne.ruleId != 0 || questOnePackData3.questGroup.limitGroupFlag))
					{
						List<CharaPackData> bannedCharaList = new List<CharaPackData>();
						foreach (CharaPackData charaPackData2 in this.haveCharaPackList)
						{
							if (QuestUtil.IsBanTarget(charaPackData2.dynamicData, questOnePackData3, this.checkedCharaList))
							{
								bannedCharaList.Add(charaPackData2);
							}
						}
						list4 = this.haveCharaPackList.FindAll((CharaPackData item) => !bannedCharaList.Exists((CharaPackData banned) => banned.id == item.staticData.GetId()));
					}
					if (list4.Count == 0)
					{
						list4 = this.haveCharaPackList;
					}
					sortTarget.charaList = list4;
					sortTarget.deckCharaList = this.currentDeckClone.charaIdList;
					sortTarget.photoList = this.havePhotoPackList;
					sortTarget.deckPhotoList = this.currentDeckClone.equipPhotoList;
					return sortTarget;
				}
				return null;
			},
			funcDisideTarget = delegate(RecommendedDeckWindowCtrl.SortTarget item)
			{
				this.OnSelectOpenWindowButtonCallback(-1);
				if (item != null)
				{
					this.currentDeckClone.charaIdList = item.deckCharaList;
					this.currentDeckClone.equipPhotoList = item.deckPhotoList;
					this.isChangeClone = true;
					this.ChangeDeckInfo(-1);
				}
			},
			questOneId = () => this.questOneId
		};
		CanvasManager.HdlOpenWindowRecommendedDeck.Register(registerData2);
	}

	// Token: 0x06001E00 RID: 7680 RVA: 0x00174DBC File Offset: 0x00172FBC
	public void SetQuestSkipPopup(QuestUtil.UsrQuestSkipInfo skipInfo)
	{
		this.guiData.charaDeck.Popup_QuestSkip.SetActive(true);
		this.guiData.charaDeck.Popup_QuestSkip.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		this.guiData.charaDeck.Popup_QuestSkip.transform.Find("Txt_Campaign").GetComponent<PguiTextCtrl>().text = skipInfo.popupMessage;
	}

	// Token: 0x06001E01 RID: 7681 RVA: 0x00174E2C File Offset: 0x0017302C
	private void SetTabName(int idx, string nam)
	{
		if (nam.Length > 3)
		{
			nam = nam.Substring(0, 3);
		}
		if (idx >= 0 && idx < this.guiData.charaDeck.DeckTab.m_PguiTabList.Count)
		{
			this.guiData.charaDeck.DeckTab.m_PguiTabList[idx].transform.Find("Txt").GetComponent<PguiTextCtrl>().text = nam;
		}
	}

	// Token: 0x06001E02 RID: 7682 RVA: 0x00174EA4 File Offset: 0x001730A4
	public bool OnClickMenuReturn(UnityAction cb)
	{
		if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_EDIT)
		{
			if (this.IsChangeClone())
			{
				this.currentMode = SelCharaDeckCtrl.Mode.OW_DISCARD_CHARA_RETRUEN;
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("編成が変更されています"), SelCharaDeckCtrl.questionButtonSet, true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
			}
			else
			{
				this.ChangeMode(SelCharaDeckCtrl.Mode.DECK_TOP);
			}
			return true;
		}
		if (this.IsPhotoEditMode())
		{
			if (this.IsChangeClone())
			{
				this.currentMode = SelCharaDeckCtrl.Mode.OW_DISCARD_PHOTO_RETRUEN;
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("編成が変更されています"), SelCharaDeckCtrl.questionButtonSet, true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
			}
			else
			{
				this.ChangeMode(SelCharaDeckCtrl.Mode.PHOTO_TOP);
			}
			return true;
		}
		if (this.currentMode != SelCharaDeckCtrl.Mode.DECK_TOP && this.currentMode != SelCharaDeckCtrl.Mode.PHOTO_TOP && this.currentMode != SelCharaDeckCtrl.Mode.ACCESSORY_TOP)
		{
			return true;
		}
		if (this.IsChangeClone())
		{
			this.currentMode = SelCharaDeckCtrl.Mode.OW_DISCARD_CHARA_RETRUEN;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("編成が変更されています"), SelCharaDeckCtrl.questionButtonSet, true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return true;
		}
		this.guiData.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			UnityAction cb2 = cb;
			if (cb2 == null)
			{
				return;
			}
			cb2();
		});
		this.currentMode = SelCharaDeckCtrl.Mode.INVALID;
		return false;
	}

	// Token: 0x06001E03 RID: 7683 RVA: 0x0017500C File Offset: 0x0017320C
	public bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_EDIT && this.IsChangeClone())
		{
			this.OnClickMoveSequenceName = sceneName;
			this.OnClickMoveSequenceArgs = sceneArgs;
			this.currentMode = SelCharaDeckCtrl.Mode.OW_DISCARD_CHARA_MOVE_SCENE;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("編成が変更されています"), SelCharaDeckCtrl.questionButtonSet, true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return true;
		}
		if (this.IsPhotoEditMode() && this.IsChangeClone())
		{
			this.OnClickMoveSequenceName = sceneName;
			this.OnClickMoveSequenceArgs = sceneArgs;
			this.currentMode = SelCharaDeckCtrl.Mode.OW_DISCARD_PHOTO_MOVE_SCENE;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("編成が変更されています"), SelCharaDeckCtrl.questionButtonSet, true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return true;
		}
		if ((this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP) && this.IsChangeClone())
		{
			this.OnClickMoveSequenceName = sceneName;
			this.OnClickMoveSequenceArgs = sceneArgs;
			this.currentMode = SelCharaDeckCtrl.Mode.OW_DISCARD_CHARA_MOVE_SCENE;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("編成が変更されています"), SelCharaDeckCtrl.questionButtonSet, true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return true;
		}
		return false;
	}

	// Token: 0x06001E04 RID: 7684 RVA: 0x0017515B File Offset: 0x0017335B
	public void SetActive(bool val, bool quick = false)
	{
		base.gameObject.SetActive(val);
		if (this.selPhotoEditCtrl != null && !val)
		{
			this.selPhotoEditCtrl.SetActive(val, quick);
		}
	}

	// Token: 0x06001E05 RID: 7685 RVA: 0x00175187 File Offset: 0x00173387
	public SelCharaDeckCtrl.EditResultData GetEditResultData()
	{
		return new SelCharaDeckCtrl.EditResultData
		{
			currentDeckId = this.currentDeckClone.id,
			editDeck = this.deckCloneList,
			helperData = this.setupParam.helperPackData
		};
	}

	// Token: 0x06001E06 RID: 7686 RVA: 0x001751BC File Offset: 0x001733BC
	public int ReloadDeckData(int selectDeckId = -1)
	{
		this.isChangeClone = false;
		if (this.selPhotoEditCtrl != null)
		{
			this.selPhotoEditCtrl.isChangeClone = false;
		}
		this.deckCloneList = new List<UserDeckData>();
		List<UserDeckData> userDeckList = DataManager.DmDeck.GetUserDeckList(this.setupParam.deckCategory);
		for (int i = 0; i < userDeckList.Count; i++)
		{
			this.deckCloneList.Add(userDeckList[i].Clone());
		}
		if (this.setupParam.deckCategory == UserDeckData.Category.TRAINING)
		{
			for (int j = 0; j < this.deckCloneList.Count; j++)
			{
				this.deckCloneList[j].name = SelCharaDeckCtrl.weekList[j];
			}
		}
		if (selectDeckId == -1)
		{
			selectDeckId = 1;
		}
		UserDeckData userDeckData = userDeckList.Find((UserDeckData item) => item.id == selectDeckId);
		if (userDeckData != null)
		{
			this.currentDeckClone = userDeckData.Clone();
			if (this.setupParam.deckCategory == UserDeckData.Category.TRAINING)
			{
				int num = userDeckList.IndexOf(userDeckData);
				if (num >= 0)
				{
					this.currentDeckClone.name = SelCharaDeckCtrl.weekList[num];
				}
			}
		}
		else
		{
			this.currentDeckClone = userDeckList[0].Clone();
			selectDeckId = this.currentDeckClone.id;
		}
		this.haveCharaPackList = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
		if (this.dispCharaPackList == null)
		{
			this.dispCharaPackList = new List<CharaPackData>(this.haveCharaPackList);
			this.dispCharaPackList.Insert(0, this.removeButttonCharaData);
		}
		else
		{
			CanvasManager.HdlOpenWindowSortFilter.SolutionList((this.setupParam.deckCategory == UserDeckData.Category.PVP) ? SortFilterDefine.RegisterType.CHARA_DECK_PVP : ((this.setupParam.deckCategory == UserDeckData.Category.TRAINING) ? SortFilterDefine.RegisterType.CHARA_DECK_TRAINING : SortFilterDefine.RegisterType.CHARA_DECK), null);
		}
		this.havePhotoPackList = new List<PhotoPackData>(DataManager.DmPhoto.GetUserPhotoMap().Values);
		this.haveMasterSkillList = new List<MasterSkillPackData>(DataManager.DmChara.GetUserMasterSkillMap().Values);
		this.guiData.charaDeck.ScrollView.Refresh();
		return selectDeckId;
	}

	// Token: 0x06001E07 RID: 7687 RVA: 0x001753D6 File Offset: 0x001735D6
	public bool ForceReturnTop()
	{
		bool flag = this.forceReturnTop;
		this.forceReturnTop = false;
		return flag;
	}

	// Token: 0x06001E08 RID: 7688 RVA: 0x001753E5 File Offset: 0x001735E5
	private bool IsChangeClone()
	{
		return this.isChangeClone || (this.selPhotoEditCtrl != null && this.selPhotoEditCtrl.isChangeClone);
	}

	// Token: 0x06001E09 RID: 7689 RVA: 0x0017540C File Offset: 0x0017360C
	private void activeCharaDeck()
	{
		this.guiData.charaDeck.baseObj.SetActive(true);
		this.guiData.charaDeck.Anim_ToBattle.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
	}

	// Token: 0x06001E0A RID: 7690 RVA: 0x0017543C File Offset: 0x0017363C
	private void ChangeMode(SelCharaDeckCtrl.Mode nextMode)
	{
		if (this.currentMode != nextMode)
		{
			if (this.IsPhotoEditMode() && nextMode == SelCharaDeckCtrl.Mode.PHOTO_TOP)
			{
				this.ResetCurrentIcon();
				this.currentMode = SelCharaDeckCtrl.Mode.PHOTO_TOP;
				if (this.selPhotoEditCtrl != null)
				{
					this.selPhotoEditCtrl.NotActive(delegate
					{
						this.activeCharaDeck();
					});
				}
				if (this.selPhotoEditCtrl != null)
				{
					this.selPhotoEditCtrl.ChangePhotoInfo(-1);
				}
				this.ChangeDeckInfo(-1);
				this.currentMode = nextMode;
				return;
			}
			if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_EDIT && nextMode == SelCharaDeckCtrl.Mode.DECK_TOP)
			{
				this.ResetCurrentIcon();
				this.currentMode = SelCharaDeckCtrl.Mode.DECK_TOP;
				this.guiData.charaDeck.DeckSelect.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
				{
					this.AnimeFinished = true;
				});
				if (this.guiData.charaDeck.iconTacticsSkillBase.activeSelf)
				{
					this.guiData.charaDeck.iconTacticsSkillChangeMark.gameObject.SetActive(true);
				}
				return;
			}
			if (this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP && nextMode == SelCharaDeckCtrl.Mode.DECK_TOP)
			{
				this.OnClickToggle(this.guiData.charaDeck.Btn_Chara, 0);
				this.guiData.charaDeck.Btn_Chara.SetToggleIndex(1);
				return;
			}
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP && nextMode == SelCharaDeckCtrl.Mode.DECK_TOP)
		{
			return;
		}
		Verbose<PrjLog>.LogError("Error : SelCharaDeckCtrl.ChangeMode", null);
	}

	// Token: 0x06001E0B RID: 7691 RVA: 0x00175580 File Offset: 0x00173780
	private void SetupDeckTopAnimation()
	{
		this.guiData.charaDeck.Btn_Chara.SetToggleIndex(1);
		this.guiData.charaDeck.Btn_Photo.SetToggleIndex(0);
		this.guiData.charaDeck.Btn_Accessory.SetToggleIndex(0);
		if (this.selPhotoEditCtrl != null)
		{
			this.selPhotoEditCtrl.NotActive(delegate
			{
				this.activeCharaDeck();
			});
		}
		this.guiData.charaDeck.DeckSelect.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		for (int i = 0; i < this.guiData.charaDeck.iconCharaPacks.Count; i++)
		{
			this.guiData.charaDeck.iconCharaPacks[i].iconChara.anime.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		}
		this.guiData.anim.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		this.guiData.anim.ExResumeAnimation(delegate
		{
			this.guiData.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		});
	}

	// Token: 0x06001E0C RID: 7692 RVA: 0x00175680 File Offset: 0x00173880
	private void ResetCurrentIcon()
	{
		if (this.selectCharaData != null)
		{
			SelCharaDeckCtrl.GUI.IconChara iconChara = this.guiData.SearchIconChara(this.selectCharaData);
			if (iconChara != null)
			{
				iconChara.iconCharaSet.currentFrame.SetActive(false);
			}
			this.selectCharaData = null;
			this.SetupSelectDisable();
		}
	}

	// Token: 0x06001E0D RID: 7693 RVA: 0x001756C8 File Offset: 0x001738C8
	private void ResignEquipPhotoByDataId(long dataId)
	{
		for (int i = 0; i < this.currentDeckClone.equipPhotoList.Count; i++)
		{
			for (int j = 0; j < this.currentDeckClone.equipPhotoList[i].Count; j++)
			{
				if (this.currentDeckClone.equipPhotoList[i][j] == dataId)
				{
					this.currentDeckClone.equipPhotoList[i][j] = 0L;
				}
			}
		}
	}

	// Token: 0x06001E0E RID: 7694 RVA: 0x00175744 File Offset: 0x00173944
	private void SelectCharaIcon(SelCharaDeckCtrl.SelectCharaData newSelectChara, SelCharaDeckCtrl.SelectCharaData oldSelectChara)
	{
		SelCharaDeckCtrl.GUI.IconChara iconChara = this.guiData.SearchIconChara(newSelectChara);
		SelCharaDeckCtrl.GUI.IconChara iconChara2 = this.guiData.SearchIconChara(oldSelectChara);
		List<PguiAECtrl> list = new List<PguiAECtrl>();
		if (oldSelectChara == null)
		{
			iconChara.iconCharaSet.currentFrame.SetActive(true);
			this.selectCharaData = newSelectChara;
		}
		else if (iconChara.iconCharaSet.disable.activeSelf)
		{
			iconChara.iconCharaSet.currentFrame.SetActive(false);
			if (iconChara2 != null)
			{
				iconChara2.iconCharaSet.currentFrame.SetActive(false);
			}
			this.selectCharaData = null;
			this.ChangeDeckInfo(-1);
		}
		else if (newSelectChara.type == SelCharaDeckCtrl.FrameType.RESERVE && oldSelectChara.type == SelCharaDeckCtrl.FrameType.RESERVE)
		{
			if (newSelectChara.chara == oldSelectChara.chara)
			{
				iconChara.iconCharaSet.currentFrame.SetActive(false);
				if (iconChara2 != null)
				{
					iconChara2.iconCharaSet.currentFrame.SetActive(false);
				}
				this.selectCharaData = null;
			}
			else
			{
				iconChara.iconCharaSet.currentFrame.SetActive(true);
				if (iconChara2 != null)
				{
					iconChara2.iconCharaSet.currentFrame.SetActive(false);
				}
				this.selectCharaData = newSelectChara;
			}
		}
		else if ((newSelectChara.type == SelCharaDeckCtrl.FrameType.RESERVE && oldSelectChara.type == SelCharaDeckCtrl.FrameType.DECK) || (newSelectChara.type == SelCharaDeckCtrl.FrameType.DECK && oldSelectChara.type == SelCharaDeckCtrl.FrameType.RESERVE))
		{
			SelCharaDeckCtrl.SelectCharaData selectCharaData = ((newSelectChara.type == SelCharaDeckCtrl.FrameType.RESERVE) ? newSelectChara : oldSelectChara);
			SelCharaDeckCtrl.SelectCharaData deckChara = ((newSelectChara.type == SelCharaDeckCtrl.FrameType.DECK) ? newSelectChara : oldSelectChara);
			bool flag = deckChara.chara == this.helperCharaData;
			int num = this.guiData.charaDeck.iconCharaPacks.FindIndex((SelCharaDeckCtrl.GUI.IconCharaPack item) => item.iconChara.iconCharaSet.iconCharaCtrl.charaPackData == deckChara.chara);
			if (selectCharaData.chara == this.removeButttonCharaData && !deckChara.chara.IsInvalid() && !flag)
			{
				if (this.currentDeckClone.charaIdList.FindAll((int item) => item > 0).Count <= 1)
				{
					this.currentMode = SelCharaDeckCtrl.Mode.OW_ERROR_ALLOUT_CHARA;
					CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("エラー"), PrjUtil.MakeMessage("編成中のフレンズを全て外すことは出来ません"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
				}
				else
				{
					this.currentDeckClone.charaIdList[num] = 0;
					this.isChangeClone = true;
					list.Add(this.guiData.charaDeck.iconCharaPacks[num].iconChara.iconCharaSet.aeEffChange);
				}
			}
			else if (selectCharaData.chara != this.removeButttonCharaData && !flag)
			{
				if (selectCharaData.chara.id == deckChara.chara.id)
				{
					this.currentDeckClone.charaIdList[num] = selectCharaData.chara.id;
					this.isChangeClone = true;
				}
				else if (this.currentDeckClone.charaIdList.Contains(selectCharaData.chara.id))
				{
					int num2 = this.currentDeckClone.charaIdList.IndexOf(selectCharaData.chara.id);
					int num3 = this.currentDeckClone.charaIdList[num2];
					this.currentDeckClone.charaIdList[num2] = this.currentDeckClone.charaIdList[num];
					this.currentDeckClone.charaIdList[num] = num3;
					list.Add(iconChara.iconCharaSet.aeEffChange);
					if (iconChara2 != null)
					{
						list.Add(iconChara2.iconCharaSet.aeEffChange);
					}
					list.Add(this.guiData.charaDeck.iconCharaPacks[num2].iconChara.iconCharaSet.aeEffChange);
					list.Add(this.guiData.charaDeck.iconCharaPacks[num].iconChara.iconCharaSet.aeEffChange);
					this.isChangeClone = true;
				}
				else
				{
					this.currentDeckClone.charaIdList[num] = selectCharaData.chara.id;
					list.Add(iconChara.iconCharaSet.aeEffChange);
					if (iconChara2 != null)
					{
						list.Add(iconChara2.iconCharaSet.aeEffChange);
					}
					this.isChangeClone = true;
				}
			}
			iconChara.iconCharaSet.currentFrame.SetActive(false);
			if (iconChara2 != null)
			{
				iconChara2.iconCharaSet.currentFrame.SetActive(false);
			}
			this.selectCharaData = null;
			this.ChangeDeckInfo(-1);
		}
		else if (newSelectChara.type == SelCharaDeckCtrl.FrameType.DECK && oldSelectChara.type == SelCharaDeckCtrl.FrameType.DECK)
		{
			if (newSelectChara.chara != oldSelectChara.chara)
			{
				int num4 = this.guiData.charaDeck.iconCharaPacks.FindIndex((SelCharaDeckCtrl.GUI.IconCharaPack item) => item.iconChara.iconCharaSet.iconCharaCtrl.charaPackData == newSelectChara.chara);
				int num5 = this.guiData.charaDeck.iconCharaPacks.FindIndex((SelCharaDeckCtrl.GUI.IconCharaPack item) => item.iconChara.iconCharaSet.iconCharaCtrl.charaPackData == oldSelectChara.chara);
				int num6 = this.currentDeckClone.charaIdList[num4];
				this.currentDeckClone.charaIdList[num4] = this.currentDeckClone.charaIdList[num5];
				this.currentDeckClone.charaIdList[num5] = num6;
				List<long> list2 = this.currentDeckClone.equipPhotoList[num4];
				this.currentDeckClone.equipPhotoList[num4] = this.currentDeckClone.equipPhotoList[num5];
				this.currentDeckClone.equipPhotoList[num5] = list2;
				bool flag2 = this.currentDeckClone.waitSkillList[num4];
				this.currentDeckClone.waitSkillList[num4] = this.currentDeckClone.waitSkillList[num5];
				this.currentDeckClone.waitSkillList[num5] = flag2;
				this.isChangeClone = true;
				list.Add(iconChara.iconCharaSet.aeEffChange);
				if (iconChara2 != null)
				{
					list.Add(iconChara2.iconCharaSet.aeEffChange);
				}
			}
			iconChara.iconCharaSet.currentFrame.SetActive(false);
			if (iconChara2 != null)
			{
				iconChara2.iconCharaSet.currentFrame.SetActive(false);
			}
			this.selectCharaData = null;
			this.ChangeDeckInfo(-1);
		}
		this.SetupSelectDisable();
		if (this.IsChangeClone())
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i].PlayAnimation(PguiAECtrl.AmimeType.START, null);
			}
		}
	}

	// Token: 0x06001E0F RID: 7695 RVA: 0x00175E4C File Offset: 0x0017404C
	private void SetupSelectDisable()
	{
		foreach (SelCharaDeckCtrl.GUI.IconCharaPack iconCharaPack in this.guiData.charaDeck.iconCharaPacks)
		{
			this.SetupSelectDisable(iconCharaPack.iconChara, true);
		}
		foreach (SelCharaDeckCtrl.GUI.IconChara iconChara in this.guiData.reserveCharaIcon)
		{
			this.SetupSelectDisable(iconChara, false);
		}
	}

	// Token: 0x06001E10 RID: 7696 RVA: 0x00175EF8 File Offset: 0x001740F8
	private void SetupSelectDisable(SelCharaDeckCtrl.GUI.IconChara ic, bool deck)
	{
		CharaPackData charaPackData = ic.iconCharaSet.iconCharaCtrl.charaPackData;
		if (charaPackData == this.helperCharaData || (!deck && (charaPackData == null || charaPackData.IsInvalid())))
		{
			ic.iconCharaSet.selected.SetActive(false);
			ic.iconCharaSet.disable.SetActive(false);
			ic.iconCharaSet.iconCharaCtrl.IsEnableMask(false);
			return;
		}
		int num = ((charaPackData == null) ? 0 : charaPackData.id);
		ic.iconCharaSet.selected.SetActive(!deck && num > 0 && this.currentDeckClone.charaIdList.Contains(num));
		bool flag = false;
		if (ic.iconCharaSet.selected.activeSelf)
		{
			flag = false;
		}
		else if (this.selectCharaData == null)
		{
			flag = false;
		}
		else if (this.selectCharaData.chara != null && this.selectCharaData.type == SelCharaDeckCtrl.FrameType.DECK == deck && this.selectCharaData.chara.id == num)
		{
			flag = false;
		}
		else
		{
			int selectCharaId = ((this.selectCharaData.chara == null || this.selectCharaData.chara.id <= 0 || this.selectCharaData.chara.staticData == null) ? 0 : this.selectCharaData.chara.staticData.baseData.id);
			if (selectCharaId == 0 && this.selectCharaData.type == SelCharaDeckCtrl.FrameType.RESERVE)
			{
				flag = false;
			}
			else if (this.selectCharaData.type == SelCharaDeckCtrl.FrameType.DECK && !deck)
			{
				if (DataManager.DmChara.CheckSameChara(num, this.currentDeckClone.charaIdList.FindAll((int item) => item != selectCharaId)))
				{
					flag = true;
				}
			}
			else if (this.selectCharaData.type == SelCharaDeckCtrl.FrameType.RESERVE && deck)
			{
				HashSet<int> sameCharaList = DataManager.DmChara.GetSameCharaList(selectCharaId, false);
				flag = this.currentDeckClone.charaIdList.Exists((int item) => sameCharaList.Contains(item)) && !sameCharaList.Contains(num);
			}
			else
			{
				flag = false;
			}
		}
		ic.iconCharaSet.disable.SetActive(flag);
		ic.iconCharaSet.iconCharaCtrl.IsEnableMask(ic.iconCharaSet.selected.activeSelf || ic.iconCharaSet.disable.activeSelf);
	}

	// Token: 0x06001E11 RID: 7697 RVA: 0x00176160 File Offset: 0x00174360
	private void ChangeDeckInfo(int deckId)
	{
		SelCharaDeckCtrl.<>c__DisplayClass78_0 CS$<>8__locals1 = new SelCharaDeckCtrl.<>c__DisplayClass78_0();
		CS$<>8__locals1.deckId = deckId;
		CS$<>8__locals1.<>4__this = this;
		if (this.selectCharaData != null)
		{
			SelCharaDeckCtrl.GUI.IconChara iconChara = this.guiData.SearchIconChara(this.selectCharaData);
			if (iconChara != null)
			{
				iconChara.iconCharaSet.currentFrame.SetActive(false);
			}
			this.selectCharaData = null;
			this.isChangeClone = false;
			this.SetupSelectDisable();
		}
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.questOneId);
		if (CS$<>8__locals1.deckId > 0 || this.currentDeckClone == null)
		{
			this.currentDeckClone = this.deckCloneList.Find((UserDeckData item) => item.id == CS$<>8__locals1.deckId).Clone();
		}
		this.guiData.charaDeck.PartyName.transform.Find("Txt_PartyName").GetComponent<PguiTextCtrl>().text = this.currentDeckClone.name;
		this.SetTabName(this.deckCloneList.IndexOf(this.deckCloneList.Find((UserDeckData itm) => itm.id == CS$<>8__locals1.<>4__this.currentDeckClone.id)), this.currentDeckClone.name);
		if (this.isEnemyInfo && this.setupParam.deckCategory == UserDeckData.Category.TRAINING && DataManager.DmTraining.GetTrainingPackData() != null)
		{
			int num = this.currentDeckClone.id - 200;
			if (num < 0 || num >= 7)
			{
				num = 0;
			}
			TrainingStaticData staticData = DataManager.DmTraining.GetTrainingPackData().staticData;
			this.dayOfWeekData = (staticData.dayOfWeekDataList.ContainsKey((DayOfWeek)num) ? staticData.dayOfWeekDataList[(DayOfWeek)num] : null);
			if (this.dayOfWeekData != null)
			{
				this.isEnemyInfo = true;
				this.questOneId = this.dayOfWeekData.questOneId;
				this.enemyAttribute = (questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.questOneId)).questOne.ennemyAttrMask;
			}
		}
		PrjUtil.ParamPreset paramPreset = new PrjUtil.ParamPreset();
		int num2 = 0;
		PrjUtil.ParamPreset paramPreset2 = new PrjUtil.ParamPreset();
		int num3 = 0;
		List<DataManagerChara.BonusCharaData> list = ((this.questOneId > 0) ? DataManager.DmChara.GetBonusCharaDataList(QuestUtil.GetEventId(this.questOneId, false)) : new List<DataManagerChara.BonusCharaData>());
		bool flag = questOnePackData != null && questOnePackData.questOne.QuestCategory == QuestStaticQuestOne.QuestOneCategory.NoDhole;
		CS$<>8__locals1.noEffectPhoto = DataManagerDeck.CheckDisableDropIcon(this.setupParam.deckCategory, this.setupParam.pvpSeasonId);
		List<PhotoPackData> list2 = new List<PhotoPackData>();
		List<PhotoPackData> list3 = new List<PhotoPackData>();
		List<DataManagerChara.BonusCharaData> list4 = new List<DataManagerChara.BonusCharaData>();
		List<int> list5 = new List<int>();
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		this.checkedCharaList = new List<CharaStaticData>();
		this.bannedList = new List<CharaPackData>();
		bool flag2 = true;
		SelCharaDeckCtrl.<>c__DisplayClass78_1 CS$<>8__locals2 = new SelCharaDeckCtrl.<>c__DisplayClass78_1();
		CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
		CS$<>8__locals2.i = 0;
		while (CS$<>8__locals2.i < this.currentDeckClone.charaIdList.Count)
		{
			SelCharaDeckCtrl.<>c__DisplayClass78_2 CS$<>8__locals3 = new SelCharaDeckCtrl.<>c__DisplayClass78_2();
			CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
			CS$<>8__locals3.cpd = this.haveCharaPackList.Find((CharaPackData item) => item != CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.removeButttonCharaData && item.id == CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.currentDeckClone.charaIdList[CS$<>8__locals3.CS$<>8__locals2.i]);
			CS$<>8__locals3.et = this.guiData.charaDeck.iconCharaPacks[CS$<>8__locals3.CS$<>8__locals2.i].iconChara;
			CS$<>8__locals3.isHelper = this.currentDeckClone.GetHelperIndex() == CS$<>8__locals3.CS$<>8__locals2.i;
			if (CS$<>8__locals3.isHelper)
			{
				CS$<>8__locals3.cpd = this.helperCharaData;
			}
			if (CS$<>8__locals3.cpd == null)
			{
				CS$<>8__locals3.cpd = CharaPackData.MakeInvalid();
			}
			if (CS$<>8__locals3.cpd.staticData != null && CS$<>8__locals3.cpd.staticData.orderCardList != null)
			{
				num4 += CS$<>8__locals3.cpd.staticData.orderCardList.Count<CharaOrderCard>((CharaOrderCard item) => item.type == CharaDef.OrderCardType.BEAT);
				num6 += CS$<>8__locals3.cpd.staticData.orderCardList.Count<CharaOrderCard>((CharaOrderCard item) => item.type == CharaDef.OrderCardType.ACTION);
				num5 += CS$<>8__locals3.cpd.staticData.orderCardList.Count<CharaOrderCard>((CharaOrderCard item) => item.type == CharaDef.OrderCardType.TRY);
				num7 += CS$<>8__locals3.cpd.staticData.orderCardList.Count<CharaOrderCard>((CharaOrderCard item) => item.type == CharaDef.OrderCardType.SPECIAL);
				this.guiData.orderCardWindow.charaInfo[CS$<>8__locals3.CS$<>8__locals2.i].iconCharaCtrl.Setup(CS$<>8__locals3.cpd, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
				for (int i = 0; i < this.guiData.orderCardWindow.charaInfo[CS$<>8__locals3.CS$<>8__locals2.i].guiOrderCardList.Count; i++)
				{
					this.guiData.orderCardWindow.charaInfo[CS$<>8__locals3.CS$<>8__locals2.i].guiOrderCardList[i].Setup(CS$<>8__locals3.cpd.staticData.orderCardList[i], CS$<>8__locals3.cpd.staticData.baseData);
				}
			}
			else
			{
				this.guiData.orderCardWindow.charaInfo[CS$<>8__locals3.CS$<>8__locals2.i].iconCharaCtrl.Setup(null, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
				for (int j2 = 0; j2 < this.guiData.orderCardWindow.charaInfo[CS$<>8__locals3.CS$<>8__locals2.i].guiOrderCardList.Count; j2++)
				{
					this.guiData.orderCardWindow.charaInfo[CS$<>8__locals3.CS$<>8__locals2.i].guiOrderCardList[j2].Setup(null, null);
				}
			}
			if (CS$<>8__locals3.isHelper)
			{
				if (CS$<>8__locals3.et.PhotoIconView != null)
				{
					CS$<>8__locals3.et.PhotoIconView.SetActive(this.isEnemyInfo && this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP);
				}
				if (CS$<>8__locals3.et.PhotoIconKind != null)
				{
					CS$<>8__locals3.et.PhotoIconKind.SetActive(this.isEnemyInfo);
				}
				if (CS$<>8__locals3.et.AccessoryIconView != null)
				{
					CS$<>8__locals3.et.AccessoryIconView.SetActive(this.isEnemyInfo && this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP);
				}
			}
			else
			{
				if (CS$<>8__locals3.et.PhotoIconView != null)
				{
					CS$<>8__locals3.et.PhotoIconView.SetActive(this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP);
				}
				if (CS$<>8__locals3.et.PhotoIconKind != null)
				{
					CS$<>8__locals3.et.PhotoIconKind.SetActive(true);
				}
				if (CS$<>8__locals3.et.AccessoryIconView != null)
				{
					CS$<>8__locals3.et.AccessoryIconView.SetActive(this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP);
				}
			}
			CharaWindowCtrl.DetailParamSetting.Preset preset = (this.isEnemyInfo ? CharaWindowCtrl.DetailParamSetting.Preset.NO_VIEW : CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY);
			IconCharaCtrl iconCharaCtrl = CS$<>8__locals3.et.iconCharaSet.iconCharaCtrl;
			CharaPackData cpd = CS$<>8__locals3.cpd;
			SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;
			bool flag3 = CS$<>8__locals3.et.iconCharaSet.selected.activeSelf || CS$<>8__locals3.et.iconCharaSet.disable.activeSelf;
			CharaWindowCtrl.DetailParamSetting detailParamSetting2;
			if (!this.isTutorial)
			{
				CharaWindowCtrl.DetailParamSetting detailParamSetting = new CharaWindowCtrl.DetailParamSetting(CS$<>8__locals3.isHelper ? CharaWindowCtrl.DetailParamSetting.Preset.OTHER_WITH_KEMOBOARD : preset, null);
				detailParamSetting.pvpSeasonId = this.setupParam.pvpSeasonId;
				detailParamSetting.selectEventId = QuestUtil.GetEventId(this.questOneId, false);
				detailParamSetting.selectQuestOneId = this.questOneId;
				detailParamSetting2 = detailParamSetting;
				detailParamSetting.deckCategory = this.setupParam.deckCategory;
			}
			else
			{
				detailParamSetting2 = null;
			}
			iconCharaCtrl.Setup(cpd, sortType, flag3, detailParamSetting2, CS$<>8__locals3.isHelper ? 0 : QuestUtil.GetEventId(this.questOneId, false), -1, this.questOneId);
			CS$<>8__locals3.et.iconCharaSet.iconCharaCtrl.DispPhotoPocketLevel(true);
			CS$<>8__locals3.et.iconCharaSet.iconCharaCtrl.DispAttributeMark(this.enemyAttribute);
			if (DataManagerDeck.CheckDisableDropIcon(this.setupParam.deckCategory, this.setupParam.pvpSeasonId))
			{
				CS$<>8__locals3.et.iconCharaSet.iconCharaCtrl.DispMarkEvent(false, false, false);
			}
			bool flag4 = CS$<>8__locals3.cpd.id == 0;
			bool flag5 = DataManager.DmChara.CheckSameChara(CS$<>8__locals3.cpd.id, 1);
			bool flag6 = flag && !flag4 && flag5;
			bool flag7 = QuestUtil.IsBanTarget(CS$<>8__locals3.cpd.dynamicData, questOnePackData, this.checkedCharaList);
			bool flag8 = flag6 || flag7;
			if (flag7 && CS$<>8__locals3.cpd.staticData != null)
			{
				this.bannedList.Add(CS$<>8__locals3.cpd);
			}
			else if (!flag7 && !CS$<>8__locals3.isHelper && CS$<>8__locals3.cpd.staticData != null)
			{
				flag2 = false;
			}
			CS$<>8__locals3.et.banObj.SetActive(flag8);
			CS$<>8__locals3.equipPhotoList = new List<PhotoPackData>();
			int num9;
			int j;
			for (j = 0; j < this.currentDeckClone.equipPhotoList[CS$<>8__locals3.CS$<>8__locals2.i].Count; j = num9 + 1)
			{
				PhotoPackData photoPackData;
				if (CS$<>8__locals3.isHelper)
				{
					photoPackData = ((this.helperPackData != null) ? this.helperPackData.HelperCharaSetList[this.setupParam.attrIndex].helpPhotoList[j] : null);
				}
				else
				{
					photoPackData = this.havePhotoPackList.Find((PhotoPackData item) => item.dataId == CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.currentDeckClone.equipPhotoList[CS$<>8__locals3.CS$<>8__locals2.i][j]);
				}
				bool flag9 = !CS$<>8__locals3.cpd.IsInvalid() && CS$<>8__locals3.cpd.dynamicData.PhotoPocket[j].Flag;
				bool flag10 = photoPackData != null && !photoPackData.IsInvalid();
				CS$<>8__locals3.et.iconPhotoCtrl[j].Setup(photoPackData, SortFilterDefine.SortType.LEVEL, !this.isTutorial, false, this.questOneId, CS$<>8__locals3.isHelper);
				if (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.noEffectPhoto)
				{
					CS$<>8__locals3.et.iconPhotoCtrl[j].DispDrop(false, 0);
				}
				if (photoPackData != null && !photoPackData.IsInvalid())
				{
					CS$<>8__locals3.et.iconPhotoKind[j].Replace(photoPackData.staticData.baseData.type);
				}
				else
				{
					CS$<>8__locals3.et.iconPhotoKind[j].Replace(-1);
				}
				PhotoPackData photoPackData2 = ((flag9 && flag10) ? photoPackData : null);
				bool flag11 = photoPackData != null && !photoPackData.IsInvalid() && photoPackData.staticData.baseData.kizunaPhotoFlg;
				bool flag12 = flag11 && (CS$<>8__locals3.cpd == null || CS$<>8__locals3.cpd.IsInvalid() || photoPackData.staticData.GetId() != CS$<>8__locals3.cpd.staticData.baseData.kizunaPhotoId);
				int num8 = ((CS$<>8__locals3.cpd == null || CS$<>8__locals3.cpd.IsInvalid()) ? 0 : CS$<>8__locals3.cpd.dynamicData.PhotoPocket[j].Step);
				SelCharaDeckCtrl.DecorationPhotoFrame(CS$<>8__locals3.et, j, flag10, flag9, flag11, flag12, num8, CS$<>8__locals3.isHelper);
				if (flag12)
				{
					photoPackData2 = null;
				}
				CS$<>8__locals3.equipPhotoList.Add(photoPackData);
				if (photoPackData2 != null && !CS$<>8__locals3.et.banObj.activeSelf)
				{
					list2.Add(photoPackData2);
					PhotoUtil.RefDropItemEffectPhotoList(ref list3, photoPackData2, CS$<>8__locals3.isHelper);
				}
				num9 = j;
			}
			for (int k = 0; k < this.currentDeckClone.equipPhotoList[CS$<>8__locals3.CS$<>8__locals2.i].Count; k++)
			{
				IconPhotoCtrl iconPhotoCtrl = CS$<>8__locals3.et.iconPhotoCtrl[k];
				IconPhotoCtrl.OnReturnPhotoPackDataList onReturnPhotoPackDataList;
				if ((onReturnPhotoPackDataList = CS$<>8__locals3.<>9__8) == null)
				{
					onReturnPhotoPackDataList = (CS$<>8__locals3.<>9__8 = () => CS$<>8__locals3.equipPhotoList);
				}
				iconPhotoCtrl.onReturnPhotoPackDataList = onReturnPhotoPackDataList;
				IconPhotoCtrl iconPhotoCtrl2 = CS$<>8__locals3.et.iconPhotoCtrl[k];
				IconPhotoCtrl.OnUpdateLockFlag onUpdateLockFlag;
				if ((onUpdateLockFlag = CS$<>8__locals3.<>9__9) == null)
				{
					onUpdateLockFlag = (CS$<>8__locals3.<>9__9 = delegate(IconPhotoCtrl x)
					{
						PhotoPackData photoPackData3 = x.photoPackData;
						x.Setup(photoPackData3, SortFilterDefine.SortType.LEVEL, !CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.isTutorial, false, CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.questOneId, CS$<>8__locals3.isHelper);
						if (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.noEffectPhoto)
						{
							x.DispDrop(false, 0);
						}
						int num14 = CS$<>8__locals3.et.iconPhotoCtrl.FindIndex((IconPhotoCtrl item) => item.photoPackData == x.photoPackData);
						if (0 <= num14 && num14 < CS$<>8__locals3.et.iconPhotoCtrl.Count)
						{
							bool flag17 = !CS$<>8__locals3.cpd.IsInvalid() && CS$<>8__locals3.cpd.dynamicData.PhotoPocket[num14].Flag;
							bool flag18 = photoPackData3 != null && !photoPackData3.IsInvalid();
							if (photoPackData3 != null && !photoPackData3.IsInvalid())
							{
								CS$<>8__locals3.et.iconPhotoKind[num14].Replace(photoPackData3.staticData.baseData.type);
							}
							else
							{
								CS$<>8__locals3.et.iconPhotoKind[num14].Replace(-1);
							}
							bool flag19 = photoPackData3 != null && !photoPackData3.IsInvalid() && photoPackData3.staticData.baseData.kizunaPhotoFlg;
							bool flag20 = flag19 && (CS$<>8__locals3.cpd == null || CS$<>8__locals3.cpd.IsInvalid() || photoPackData3.staticData.GetId() != CS$<>8__locals3.cpd.staticData.baseData.kizunaPhotoId);
							int num15 = ((CS$<>8__locals3.cpd == null || CS$<>8__locals3.cpd.IsInvalid()) ? 0 : CS$<>8__locals3.cpd.dynamicData.PhotoPocket[num14].Step);
							SelCharaDeckCtrl.DecorationPhotoFrame(CS$<>8__locals3.et, num14, flag18, flag17, flag19, flag20, num15, CS$<>8__locals3.isHelper);
						}
					});
				}
				iconPhotoCtrl2.AddOnUpdateStatus(onUpdateLockFlag);
			}
			DataManagerCharaAccessory.Accessory accessory = ((!CS$<>8__locals3.cpd.IsInvalid()) ? CS$<>8__locals3.cpd.dynamicData.accessory : null);
			CS$<>8__locals3.et.iconAccessoryCtrl.Setup(new IconAccessoryCtrl.SetupParam
			{
				acce = accessory,
				sortType = SortFilterDefine.SortType.LEVEL
			});
			SelCharaDeckCtrl.DecorationAccessoryFrame(CS$<>8__locals3.et, !CS$<>8__locals3.cpd.IsInvalid(), CS$<>8__locals3.isHelper);
			if (CS$<>8__locals3.cpd != null && !CS$<>8__locals3.cpd.IsInvalid() && !CS$<>8__locals3.et.banObj.activeSelf)
			{
				PrjUtil.ParamPreset activeKizunaBuff = DataManager.DmChara.GetActiveKizunaBuff();
				PrjUtil.ParamPreset paramPreset3 = PrjUtil.CalcBattleParamByChara(CS$<>8__locals3.cpd.dynamicData, CS$<>8__locals3.equipPhotoList, list, activeKizunaBuff);
				paramPreset.hp += paramPreset3.hp;
				paramPreset.atk += paramPreset3.atk;
				paramPreset.def += paramPreset3.def;
				num2 += paramPreset3.totalParam;
				paramPreset3 = PrjUtil.CalcBattleParamByChara(CS$<>8__locals3.cpd.dynamicData, CS$<>8__locals3.equipPhotoList, null, activeKizunaBuff);
				paramPreset2.hp += paramPreset3.hp;
				paramPreset2.atk += paramPreset3.atk;
				paramPreset2.def += paramPreset3.def;
				num3 += paramPreset3.totalParam;
				if (DataManager.DmKemoBoard.KemoBoardBonusParamMap.ContainsKey(CS$<>8__locals3.cpd.staticData.baseData.attribute))
				{
					DataManagerKemoBoard.KemoBoardBonusParam kemoBoardBonusParam = DataManager.DmKemoBoard.KemoBoardBonusParamMap[CS$<>8__locals3.cpd.staticData.baseData.attribute];
					paramPreset.hp += kemoBoardBonusParam.Hp;
					paramPreset.atk += kemoBoardBonusParam.Attack;
					paramPreset.def += kemoBoardBonusParam.Difence;
					num2 += kemoBoardBonusParam.KemoStatus;
					paramPreset2.hp += kemoBoardBonusParam.Hp;
					paramPreset2.atk += kemoBoardBonusParam.Attack;
					paramPreset2.def += kemoBoardBonusParam.Difence;
					num3 += kemoBoardBonusParam.KemoStatus;
				}
				DataManagerChara.BonusCharaData bonusCharaData = list.Find((DataManagerChara.BonusCharaData itm) => itm.charaId == CS$<>8__locals3.cpd.dynamicData.id && (itm.increaseItemId01 != 0 || itm.increaseItemId02 != 0));
				if (bonusCharaData != null)
				{
					list4.Add(bonusCharaData);
					int num10 = CS$<>8__locals3.cpd.dynamicData.PhotoPocket.FindAll((CharaDynamicData.PPParam itm) => itm.Flag).Count<CharaDynamicData.PPParam>();
					list5.Add(num10);
				}
			}
			if (CS$<>8__locals3.cpd != null && CS$<>8__locals3.cpd.staticData != null)
			{
				this.checkedCharaList.Add(CS$<>8__locals3.cpd.staticData);
			}
			num9 = CS$<>8__locals2.i;
			CS$<>8__locals2.i = num9 + 1;
		}
		this.guiData.charaDeck.Btn_ToBattle.SetActEnable(true, false, false);
		if (questOnePackData != null && ((questOnePackData.questOne != null && questOnePackData.questOne.ruleId != 0) || (questOnePackData.questGroup != null && questOnePackData.questGroup.limitGroupFlag)))
		{
			bool flag13 = this.checkedCharaList.Count <= this.bannedList.Count || flag2;
			this.guiData.charaDeck.Btn_ToBattle.SetActEnable(!flag13, true, false);
		}
		this.guiData.charaDeck.Txt_Deck_Red.text = num4.ToString();
		this.guiData.charaDeck.Txt_Deck_Blue.text = num6.ToString();
		this.guiData.charaDeck.Txt_Deck_Green.text = num5.ToString();
		this.guiData.charaDeck.Txt_Deck_Special.text = num7.ToString();
		this.guiData.orderCardWindow.Txt_TotalR.text = num4.ToString();
		this.guiData.orderCardWindow.Txt_TotalB.text = num6.ToString();
		this.guiData.orderCardWindow.Txt_TotalG.text = num5.ToString();
		this.guiData.orderCardWindow.Txt_TotalS.text = num7.ToString();
		for (int l = 0; l < this.guiData.charaDeck.Btn_StaySkills.Count; l++)
		{
			PguiToggleButtonCtrl pguiToggleButtonCtrl = this.guiData.charaDeck.Btn_StaySkills[l];
			bool flag14 = true;
			if (this.currentDeckClone.waitSkillList.Count > l)
			{
				flag14 = this.currentDeckClone.waitSkillList[l];
			}
			pguiToggleButtonCtrl.transform.Find("BaseImage/Txt_ONOFF").GetComponent<PguiTextCtrl>().text = (flag14 ? "ON" : "OFF");
			pguiToggleButtonCtrl.SetToggleIndex(flag14 ? 1 : 0);
		}
		for (int m = 0; m < this.guiData.charaDeck.Txt_StaySkill_Switches.Count; m++)
		{
			PguiTextCtrl pguiTextCtrl = this.guiData.charaDeck.Txt_StaySkill_Switches[m];
			pguiTextCtrl.text = ((this.currentDeckClone.waitSkillList.Count > m && this.currentDeckClone.waitSkillList[m]) ? "ON" : "OFF");
			pguiTextCtrl.m_Text.color = pguiTextCtrl.GetComponent<PguiColorCtrl>().GetGameObjectById(pguiTextCtrl.text);
		}
		Color colorByCode = PrjUtil.GetColorByCode("#6b5108ff");
		Color colorByCode2 = PrjUtil.GetColorByCode("#d047efff");
		this.guiData.charaDeck.Txt_All.text = num2.ToString();
		this.guiData.charaDeck.Txt_All.m_Text.color = ((num2 == num3) ? colorByCode : colorByCode2);
		this.guiData.charaDeck.Txt_HP.text = paramPreset.hp.ToString();
		this.guiData.charaDeck.Txt_HP.m_Text.color = ((paramPreset.hp == paramPreset2.hp) ? colorByCode : colorByCode2);
		this.guiData.charaDeck.Txt_ATK.text = paramPreset.atk.ToString();
		this.guiData.charaDeck.Txt_ATK.m_Text.color = ((paramPreset.atk == paramPreset2.atk) ? colorByCode : colorByCode2);
		this.guiData.charaDeck.Txt_DEF.text = paramPreset.def.ToString();
		this.guiData.charaDeck.Txt_DEF.m_Text.color = ((paramPreset.def == paramPreset2.def) ? colorByCode : colorByCode2);
		bool flag15 = false;
		if (this.setupParam.deckCategory == UserDeckData.Category.PVP)
		{
			PvpStaticData pvpStaticDataBySeasonID = DataManager.DmPvp.GetPvpStaticDataBySeasonID(this.setupParam.pvpSeasonId);
			if (pvpStaticDataBySeasonID != null && pvpStaticDataBySeasonID.type == PvpStaticData.Type.SPECIAL)
			{
				flag15 = true;
			}
		}
		this.guiData.charaDeck.Txt_TotalPlasm.transform.gameObject.SetActive(flag15);
		this.guiData.charaDeck.Btn_OrderCardInfo.gameObject.SetActive(true);
		this.guiData.charaDeck.iconTacticsSkillBase.SetActive(flag15);
		this.guiData.charaDeck.PlayerIcon.gameObject.SetActive(!flag15);
		this.guiData.charaDeck.Btn_SkillChange.gameObject.SetActive(!flag15);
		this.guiData.charaDeck.Btn_TacticsChange.gameObject.SetActive(flag15);
		this.guiData.charaDeck.Txt_SkillTypeName.text = (flag15 ? "作戦スキル" : "隊長スキル");
		if (flag15)
		{
			if (this.guiData.charaDeck.PlayerMarkBan != null)
			{
				this.guiData.charaDeck.PlayerMarkBan.SetActive(false);
			}
			this.guiData.charaDeck.iconTacticsSkill.Replace(this.currentDeckClone.pvpTacticsTypeId);
			TacticsStaticSkill tacticsSkillStaticData = DataManager.DmChara.GetTacticsSkillStaticData(this.currentDeckClone.pvpTacticsTypeId);
			this.guiData.charaDeck.Txt_SkillName.text = tacticsSkillStaticData.skillName;
			this.guiData.charaDeck.Txt_SkillLevel.text = "";
			int num11 = this.currentDeckClone.CalcTotalPlasmPoint(false);
			this.guiData.charaDeck.Txt_TotalPlasm.text = num11.ToString();
		}
		else
		{
			MasterStaticSkill masterSkillStaticData = DataManager.DmChara.GetMasterSkillStaticData(this.currentDeckClone.masterSkillId);
			this.guiData.charaDeck.Txt_SkillName.text = masterSkillStaticData.skillName;
			DataManagerMasterSkill.MasterSkillData masterSkillData = DataManager.DmMasterSkill.UserMasterSkillDataList.Find((DataManagerMasterSkill.MasterSkillData item) => item.SkillId == CS$<>8__locals1.<>4__this.currentDeckClone.masterSkillId);
			if (masterSkillData != null)
			{
				this.guiData.charaDeck.Txt_SkillLevel.ReplaceTextByDefault("Param01", masterSkillData.Level.ToString());
			}
			this.guiData.charaDeck.Icon_Skill.SetImageByName(masterSkillStaticData.iconMiniName);
			DataManagerUserInfo dmUserInfo = DataManager.DmUserInfo;
			this.guiData.charaDeck.PlayerIcon.Replace((dmUserInfo.avatarType == DataManagerUserInfo.AvatarType.TYPE_A) ? 1 : 2);
			if (this.guiData.charaDeck.PlayerMarkBan != null)
			{
				this.guiData.charaDeck.PlayerMarkBan.SetActive(questOnePackData != null && questOnePackData.questOne.QuestCategory == QuestStaticQuestOne.QuestOneCategory.NoPlayer);
			}
		}
		this.guiData.charaDeck.EnemyInfo.SetActive(this.isEnemyInfo && !flag15);
		this.guiData.charaDeck.Btn_EnemyInfo.gameObject.SetActive(this.setupParam.deckCategory == UserDeckData.Category.TRAINING);
		if (this.isEnemyInfo && !flag15)
		{
			for (int n = 0; n < this.guiData.charaDeck.enemyInfoList.Count; n++)
			{
				this.guiData.charaDeck.enemyInfoList[n].Setup((this.enemyAttribute & SelBattleHelperCtrl.GUI.EnemyInfo.attributeMaskList[n]) == SelBattleHelperCtrl.GUI.EnemyInfo.attributeMaskList[n]);
			}
		}
		this.guiData.charaDeck.SwitchHelperIcon(this.currentDeckClone.GetHelperIndex(), this.isEnemyInfo);
		List<CharaPackData> list6 = new List<CharaPackData>();
		foreach (CharaPackData charaPackData in this.dispCharaPackList)
		{
			if (QuestUtil.IsBanTarget(charaPackData.dynamicData, questOnePackData, this.checkedCharaList))
			{
				list6.Add(charaPackData);
			}
		}
		CanvasManager.HdlOpenWindowSortFilter.UpdateBannedCharaList(list6);
		CanvasManager.HdlOpenWindowSortFilter.SolutionList((this.setupParam.deckCategory == UserDeckData.Category.PVP) ? SortFilterDefine.RegisterType.CHARA_DECK_PVP : ((this.setupParam.deckCategory == UserDeckData.Category.TRAINING) ? SortFilterDefine.RegisterType.CHARA_DECK_TRAINING : SortFilterDefine.RegisterType.CHARA_DECK), list6);
		bool flag16 = false;
		int num12 = 0;
		while (!flag16 && num12 < this.currentDeckClone.equipPhotoList.Count)
		{
			if (this.currentDeckClone.GetHelperIndex() != num12)
			{
				int num13 = 0;
				while (!flag16 && num13 < this.currentDeckClone.equipPhotoList[num12].Count)
				{
					if (this.currentDeckClone.equipPhotoList[num12][num13] > 0L)
					{
						flag16 = true;
					}
					num13++;
				}
			}
			num12++;
		}
		this.guiData.charaDeck.Btn_PhotoRemove.SetActEnable(flag16, false, false);
		if (this.guiData.charaDeck.Info_PhotoEffect != null)
		{
			if (CS$<>8__locals1.noEffectPhoto)
			{
				this.guiData.charaDeck.Info_PhotoEffect.Setup(new InfoPhotoItemEffectCtrl.SetupParam());
				return;
			}
			this.guiData.charaDeck.Info_PhotoEffect.Setup(QuestUtil.GetCalcDropBonusResultDeck(questOnePackData, list2, list4, list5), PhotoUtil.GetDropItemEffectPhotoDeck(questOnePackData, list3));
		}
	}

	// Token: 0x06001E12 RID: 7698 RVA: 0x00177AC8 File Offset: 0x00175CC8
	public static void DecorationPhotoFrame(SelCharaDeckCtrl.GUI.IconChara iconChara, int index, bool isEnablePhoto, bool isEnableFrame, bool isKizunaPhoto, bool isDisableKizunaPhoto, int lv, bool isHelper)
	{
		if (iconChara.iconBlankFrame != null && iconChara.iconBlankFrame[index] != null)
		{
			PguiReplaceSpriteCtrl pguiReplaceSpriteCtrl = iconChara.iconBlankFrame[index];
			IconPhotoCtrl iconPhotoCtrl = iconChara.iconPhotoCtrl[index];
			iconPhotoCtrl.DispTextDisable(isDisableKizunaPhoto, null, null);
			iconPhotoCtrl.DispMarkNotYetReleased(false);
			pguiReplaceSpriteCtrl.Replace(3);
			pguiReplaceSpriteCtrl.GetComponent<Image>().color = Color.white;
			iconChara.iconPhotoKind[index].gameObject.SetActive(true);
			iconChara.iconStatusKind[index].gameObject.SetActive(true);
			if (isEnablePhoto && isEnableFrame)
			{
				pguiReplaceSpriteCtrl.Replace(2);
				iconPhotoCtrl.DispImgDisable(isKizunaPhoto && isDisableKizunaPhoto);
				if (isKizunaPhoto && isDisableKizunaPhoto)
				{
					iconChara.iconStatusKind[index].Replace(3);
				}
				else
				{
					iconChara.iconStatusKind[index].gameObject.SetActive(false);
				}
			}
			else if (isEnablePhoto && !isEnableFrame)
			{
				pguiReplaceSpriteCtrl.Replace(2);
				iconPhotoCtrl.DispImgDisable(true);
				iconPhotoCtrl.DispMarkNotYetReleased(true);
				iconChara.iconStatusKind[index].Replace(3);
			}
			else if (!isEnablePhoto && isEnableFrame)
			{
				pguiReplaceSpriteCtrl.Replace(3);
				iconChara.iconStatusKind[index].Replace(2);
				if (isHelper)
				{
					pguiReplaceSpriteCtrl.GetComponent<Image>().color = Color.gray;
				}
			}
			else if (!isEnablePhoto && !isEnableFrame)
			{
				pguiReplaceSpriteCtrl.Replace(1);
				iconChara.iconStatusKind[index].Replace(1);
				if (isHelper)
				{
					pguiReplaceSpriteCtrl.GetComponent<Image>().color = Color.gray;
				}
			}
			string text = ((isEnableFrame && lv > 0) ? lv.ToString() : "");
			iconChara.photoRect[index].Find("Num_Lv").GetComponent<PguiTextCtrl>().text = text;
			iconChara.iconPhotoKind[index].transform.Find("Num_Lv").GetComponent<PguiTextCtrl>().text = text;
		}
	}

	// Token: 0x06001E13 RID: 7699 RVA: 0x00177CAC File Offset: 0x00175EAC
	public static void DecorationAccessoryFrame(SelCharaDeckCtrl.GUI.IconChara iconChara, bool isOpen, bool isHelper)
	{
		if (iconChara != null && iconChara.iconAccessoryBlankFrame != null)
		{
			if (isOpen)
			{
				iconChara.iconAccessoryBlankFrame.Replace(3);
			}
			else
			{
				iconChara.iconAccessoryBlankFrame.Replace(1);
			}
			iconChara.iconAccessoryBlankFrame.GetComponent<Image>().color = (isHelper ? Color.gray : Color.white);
			iconChara.iconAccessoryBlankFrame.gameObject.SetActive(true);
		}
	}

	// Token: 0x06001E14 RID: 7700 RVA: 0x00177D17 File Offset: 0x00175F17
	public RectTransform GetCharaDeckRectTransform(int index)
	{
		return this.guiData.charaDeck.iconCharaPacks[index].iconChara.iconCharaSet.iconBase.transform as RectTransform;
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x00177D48 File Offset: 0x00175F48
	public RectTransform GetCharaDeckReserveRectTransform(SelCharaDeckCtrl.FrameType type, int id)
	{
		return this.guiData.SearchIconChara(type, id).iconCharaSet.iconBase.transform as RectTransform;
	}

	// Token: 0x06001E16 RID: 7702 RVA: 0x00177D6B File Offset: 0x00175F6B
	public float GetCharaDeckReserveScale(SelCharaDeckCtrl.FrameType type, int id)
	{
		return this.guiData.SearchIconChara(type, id).baseObj.transform.localScale.x;
	}

	// Token: 0x06001E17 RID: 7703 RVA: 0x00177D8E File Offset: 0x00175F8E
	public RectTransform GetCharaDeckEditOKBtnRectTransform()
	{
		return this.guiData.charaDeck.Btn_EditOk.transform as RectTransform;
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x00177DAA File Offset: 0x00175FAA
	public RectTransform GetCharaDeckEditToBattleBtnRectTransform()
	{
		return this.guiData.charaDeck.Btn_ToBattle.transform as RectTransform;
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x00177DC6 File Offset: 0x00175FC6
	public bool IsPlayingAnimCharaDeckEditToBattleBtn()
	{
		return this.guiData.anim.ExIsPlaying();
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x00177DD8 File Offset: 0x00175FD8
	public RectTransform GetCharaDeckPhotoRadioBtnRectTransform()
	{
		return this.guiData.charaDeck.Btn_Photo.transform as RectTransform;
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x00177DF4 File Offset: 0x00175FF4
	public float GetCharaDeckPhotoRadioBtnScale()
	{
		return this.guiData.charaDeck.Btn_Photo.transform.localScale.x;
	}

	// Token: 0x06001E1C RID: 7708 RVA: 0x00177E15 File Offset: 0x00176015
	public RectTransform GetCharaDeckPhotoRectTransform(int index)
	{
		return this.guiData.charaDeck.iconCharaPacks[index].iconChara.iconPhotoCtrl[0].transform as RectTransform;
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x00177E47 File Offset: 0x00176047
	public float GetCharaDeckPhotoScale(int index)
	{
		return this.guiData.charaDeck.iconCharaPacks[index].iconChara.baseObj.transform.Find("PhotoIconView/Icon_Photo01").transform.localScale.x;
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x00177E88 File Offset: 0x00176088
	public RectTransform GetPhotoDeckRectTransform()
	{
		if (!(this.selPhotoEditCtrl != null))
		{
			return base.transform as RectTransform;
		}
		return this.selPhotoEditCtrl.guiData.photoDeck.mainIconPhotoCtrl[0].baseObj.transform as RectTransform;
	}

	// Token: 0x06001E1F RID: 7711 RVA: 0x00177ED9 File Offset: 0x001760D9
	public bool IsPhotoDeckTouchRect()
	{
		return !(this.selPhotoEditCtrl != null) || this.selPhotoEditCtrl.TouchRect;
	}

	// Token: 0x06001E20 RID: 7712 RVA: 0x00177EF6 File Offset: 0x001760F6
	public bool IsPhotoEditPlayingAnim()
	{
		return this.selPhotoEditCtrl != null && this.selPhotoEditCtrl.IsPlayingdAnim();
	}

	// Token: 0x06001E21 RID: 7713 RVA: 0x00177F14 File Offset: 0x00176114
	public RectTransform GetPhotoDeckReserveRectTransform(SelCharaDeckCtrl.FrameType type, int id)
	{
		SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet = ((this.selPhotoEditCtrl != null) ? this.selPhotoEditCtrl.guiData.SearchIconPhoto(type, id) : null);
		if (iconPhotoSet != null)
		{
			return iconPhotoSet.iconBase.transform as RectTransform;
		}
		return new RectTransform();
	}

	// Token: 0x06001E22 RID: 7714 RVA: 0x00177F60 File Offset: 0x00176160
	public float GetPhotoDeckReserveScale(SelCharaDeckCtrl.FrameType type, int id)
	{
		SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet = ((this.selPhotoEditCtrl != null) ? this.selPhotoEditCtrl.guiData.SearchIconPhoto(type, id) : null);
		if (iconPhotoSet != null)
		{
			return iconPhotoSet.iconBase.transform.localScale.x;
		}
		return 1f;
	}

	// Token: 0x06001E23 RID: 7715 RVA: 0x00177FB0 File Offset: 0x001761B0
	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/GUI_CharaDeck"), base.transform);
		if (!this.isDebug)
		{
			SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, base.transform, true);
		}
		this.guiData = new SelCharaDeckCtrl.GUI(gameObject.transform);
		if (!this.isDebug)
		{
			SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.guiData.owMasterSkill.Window.transform, true);
		}
		this.currentMode = SelCharaDeckCtrl.Mode.DECK_TOP;
		this.guiData.charaDeck.DeckSelect.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.guiData.charaDeck.DeckTab.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectDeckTab));
		ReuseScroll scrollView = this.guiData.charaDeck.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartItemChara));
		ReuseScroll scrollView2 = this.guiData.charaDeck.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemChara));
		this.guiData.charaDeck.ScrollView.Setup(10, 0);
		for (int i = 0; i < this.guiData.charaDeck.iconCharaPacks.Count; i++)
		{
			this.guiData.charaDeck.iconCharaPacks[i].iconChara.Setup(new SelCharaDeckCtrl.GUI.IconChara.SetupParam
			{
				cbTouchCharaIcon = delegate(SelCharaDeckCtrl.FrameType type, SelCharaDeckCtrl.GUI.IconChara icon, int photo)
				{
					this.OnTouchCharaIcon(type, icon, photo);
					return true;
				},
				cbClickIconPhotoKind = delegate
				{
					this.OnClickToggle(this.guiData.charaDeck.Btn_Photo, 0);
					this.guiData.charaDeck.Btn_Photo.SetToggleIndex(1);
					return true;
				},
				isDeck = true,
				index = i
			});
		}
		this.guiData.charaDeck.Btn_EditOk.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.charaDeck.Btn_ToBattle.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.charaDeck.Btn_QuestSkip.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.charaDeck.Btn_SkillChange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.charaDeck.Btn_TacticsChange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickTacticsChangeButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.charaDeck.Btn_StaySkillSetting.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.charaDeck.Btn_PartyName.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.charaDeck.Btn_PhotoRemove.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.charaDeck.Btn_Chara.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		this.guiData.charaDeck.Btn_Photo.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		this.guiData.charaDeck.Btn_Accessory.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		this.guiData.charaDeck.Btn_OrderCardInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DECIDE);
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.charaDeck.Btn_StaySkills)
		{
			pguiToggleButtonCtrl.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggleStaySkill));
		}
		this.guiData.charaDeck.Btn_Chara.SetToggleIndex(1);
		this.guiData.charaDeck.Btn_EnemyInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DECIDE);
		ReuseScroll scrollView3 = this.guiData.owMasterSkill.ScrollView;
		scrollView3.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView3.onStartItem, new Action<int, GameObject>(this.OnStartItemMasterSkill));
		ReuseScroll scrollView4 = this.guiData.owMasterSkill.ScrollView;
		scrollView4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView4.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemMasterSkill));
		this.guiData.owMasterSkill.ScrollView.InitForce();
		this.guiData.owMasterSkill.ScrollView.Setup(10, 0);
		this.guiData.charaDeck.Btn_PhotoRemove.transform.parent.GetComponent<SimpleAnimation>().ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		this.guiData.charaDeck.Btn_PhotoRemove.transform.parent.gameObject.SetActive(false);
		this.guiData.charaDeck.iconTacticsSkill.transform.parent.gameObject.AddComponent<PguiTouchTrigger>().AddListener(new UnityAction(this.OnTouchTacticsSkillIcon), null, null, null, null);
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x0017848C File Offset: 0x0017668C
	public void Dest()
	{
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x0017848E File Offset: 0x0017668E
	public void CancelBattleStart()
	{
		if (this.currentMode == SelCharaDeckCtrl.Mode.GO_BATTLE)
		{
			this.currentMode = this.preMode;
		}
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x001784A5 File Offset: 0x001766A5
	private void Start()
	{
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x001784A8 File Offset: 0x001766A8
	private void Update()
	{
		if (this.currentEnumerator != null)
		{
			if (!this.currentEnumerator.MoveNext())
			{
				this.currentEnumerator = null;
			}
			if (this.isCheckKizunaLimit && (this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP))
			{
				int num = this.questOneId;
				int num2 = 0;
				if (this.setupParam != null && this.setupParam.deckCategory == UserDeckData.Category.PVP)
				{
					num2 = DataManager.DmPvp.GetPvpStaticDataBySeasonID(this.setupParam.pvpSeasonId).spEventId;
					if (num2 != 0)
					{
						num = 0;
					}
				}
				List<int> list = new List<int>();
				foreach (int num3 in this.currentDeckClone.charaIdList)
				{
					bool flag = false;
					foreach (CharaPackData charaPackData in this.bannedList)
					{
						flag = DataManager.DmChara.CheckSameChara(num3, charaPackData.id);
						if (flag)
						{
							break;
						}
					}
					if (!flag)
					{
						list.Add(num3);
					}
				}
				this.currentEnumerator = QuestUtil.NoticeKizunaLimitReached(delegate
				{
					if (this.setupParam.callbackGotoBattle != null && this.currentEnumerator != null)
					{
						this.currentEnumerator = null;
						this.isCheckBanned = true;
					}
				}, num, list, 1, null, 0, num2, this.setupParam.isPractice || !this.guiData.charaDeck.Btn_ToBattle.ActEnable);
			}
			this.isCheckKizunaLimit = false;
			if (this.isCheckBanned && (this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP))
			{
				QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.questOneId);
				this.currentEnumerator = QuestUtil.NoticeBanned(delegate
				{
					if (this.setupParam.callbackGotoBattle != null && this.currentEnumerator != null)
					{
						this.setupParam.callbackGotoBattle();
						this.currentMode = SelCharaDeckCtrl.Mode.GO_BATTLE;
						this.currentEnumerator = null;
					}
				}, questOnePackData, this.currentDeckClone, this.removeButttonCharaData, this.haveCharaPackList);
			}
			this.isCheckBanned = false;
		}
		if (this.currentEnumerator == null && this.skipCount != -1)
		{
			this.currentEnumerator = QuestUtil.NoticeKizunaLimitReached(delegate
			{
				if (!DataManager.IsServerRequesting())
				{
					CanvasManager.HdlQuestSkipWindowsCtrl.RequestExecQuestSkip();
					this.currentEnumerator = null;
				}
			}, this.questOneId, this.currentDeckClone.charaIdList, this.skipCount, delegate(int action)
			{
				this.skipCount = action;
			}, 0, 0, this.setupParam.isPractice);
		}
		SimpleAnimation sa = this.guiData.charaDeck.Btn_PhotoRemove.transform.parent.GetComponent<SimpleAnimation>();
		if (this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP)
		{
			if (!sa.gameObject.activeSelf || !sa.ExIsCurrent(SimpleAnimation.ExPguiStatus.START))
			{
				sa.gameObject.SetActive(true);
				sa.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
				{
					sa.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START);
				});
				return;
			}
		}
		else if (sa.gameObject.activeSelf)
		{
			if (sa.ExIsCurrent(SimpleAnimation.ExPguiStatus.END))
			{
				if (!sa.ExIsPlaying())
				{
					sa.gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				sa.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			}
		}
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x001787D0 File Offset: 0x001769D0
	private void OnDestroy()
	{
		if (this.guiData != null && this.guiData.owMasterSkill.Window != null)
		{
			Object.Destroy(this.guiData.owMasterSkill.baseObj.gameObject);
		}
	}

	// Token: 0x06001E29 RID: 7721 RVA: 0x0017880C File Offset: 0x00176A0C
	private IEnumerator RequestUpdateDeck(SelCharaDeckCtrl.RequestCallBack callback)
	{
		bool flag = false;
		if (this.selPhotoEditCtrl != null && this.selPhotoEditCtrl.isChangeClone)
		{
			this.selPhotoEditCtrl.isChangeClone = false;
			this.currentDeckClone.equipPhotoList[this.selPhotoEditCtrl.setupParam.indexByDeckChara] = this.selPhotoEditCtrl.setupParam.cbGetEquipPhoto();
			flag = true;
		}
		else if (this.isChangeClone)
		{
			this.isChangeClone = false;
			flag = true;
		}
		if (flag)
		{
			if (this.setupParam.deckCategory != UserDeckData.Category.TRAINING)
			{
				UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
				if (this.setupParam.deckCategory == UserDeckData.Category.PVP)
				{
					PvpPackData pvpPackDataBySeasonID = DataManager.DmPvp.GetPvpPackDataBySeasonID(this.setupParam.pvpSeasonId);
					if (pvpPackDataBySeasonID != null && pvpPackDataBySeasonID.staticData.type == PvpStaticData.Type.SPECIAL)
					{
						userOptionData.CurrentSpPvpParty = CanvasManager.HdlSelCharaDeck.GetDeckId();
					}
					else
					{
						userOptionData.CurrentPvpParty = CanvasManager.HdlSelCharaDeck.GetDeckId();
					}
				}
				else
				{
					userOptionData.CurrentQuestParty = CanvasManager.HdlSelCharaDeck.GetDeckId();
				}
				DataManager.DmUserInfo.RequestActionUpdateUserOption(userOptionData);
			}
			DataManager.DmDeck.RequestActionUpdateDeck(this.currentDeckClone);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
		}
		this.ReloadDeckData(this.currentDeckClone.id);
		if (this.selPhotoEditCtrl != null)
		{
			this.selPhotoEditCtrl.Reload();
		}
		callback();
		yield break;
	}

	// Token: 0x06001E2A RID: 7722 RVA: 0x00178824 File Offset: 0x00176A24
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_EDIT && button == this.guiData.charaDeck.Btn_EditOk)
		{
			this.currentEnumerator = this.RequestUpdateDeck(delegate
			{
				this.ChangeMode(SelCharaDeckCtrl.Mode.DECK_TOP);
				this.TouchRect = true;
			});
			return;
		}
		if ((this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP) && button == this.guiData.charaDeck.Btn_ToBattle)
		{
			this.currentEnumerator = this.RequestUpdateDeck(delegate
			{
				this.isCheckKizunaLimit = true;
			});
			return;
		}
		if ((this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP) && button == this.guiData.charaDeck.Btn_QuestSkip)
		{
			this.skipCount = -1;
			CanvasManager.HdlQuestSkipWindowsCtrl.SetReturnSkipCountAction(delegate(int action)
			{
				this.skipCount = action;
			});
			CanvasManager.HdlQuestSkipWindowsCtrl.InitializeWindow(this.questOneId, this.setupParam.helperPackData, this.setupParam.attrIndex, this.currentDeckClone.id, 0);
			return;
		}
		if ((this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP) && button == this.guiData.charaDeck.Btn_SkillChange)
		{
			this.currentMode = SelCharaDeckCtrl.Mode.OW_MASTER_SKILL;
			this.guiData.owMasterSkill.ScrollView.Resize(this.haveMasterSkillList.Count, 0);
			MasterSkillPackData masterSkillPackData = this.haveMasterSkillList.Find((MasterSkillPackData itm) => itm.id == this.currentDeckClone.masterSkillId);
			if (masterSkillPackData == null)
			{
				masterSkillPackData = DataManager.DmChara.GetUserMasterSkillData(this.currentDeckClone.masterSkillId);
			}
			this.guiData.owMasterSkill.Txt_UserSkill.text = masterSkillPackData.staticData.skillName;
			DataManagerMasterSkill.MasterSkillData masterSkillData = DataManager.DmMasterSkill.UserMasterSkillDataList.Find((DataManagerMasterSkill.MasterSkillData item) => item.SkillId == this.currentDeckClone.masterSkillId);
			if (masterSkillData != null)
			{
				this.guiData.owMasterSkill.Txt_UserSkillLevel.ReplaceTextByDefault("Param01", masterSkillData.Level.ToString());
			}
			this.guiData.owMasterSkill.Txt_SkillInfo.text = masterSkillPackData.staticData.MakeSkillText(masterSkillPackData.dynamicData.level, false);
			this.guiData.owMasterSkill.Icon_Skill.SetImageByName(masterSkillPackData.staticData.iconMiniName);
			this.guiData.owMasterSkill.choiceMasterSkillId = this.currentDeckClone.masterSkillId;
			this.guiData.owMasterSkill.ScrollView.Refresh();
			this.guiData.owMasterSkill.Window.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			this.guiData.owMasterSkill.Window.Open();
			return;
		}
		if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP && button == this.guiData.charaDeck.Btn_StaySkillSetting)
		{
			if (this.guiData.charaDeck.StaySkillSettingAnim.ExIsPlaying() && this.guiData.charaDeck.StaySkillSettingAnim.gameObject.activeSelf)
			{
				return;
			}
			this.SetStaySkillAnim();
			return;
		}
		else
		{
			if ((this.currentMode != SelCharaDeckCtrl.Mode.DECK_TOP && this.currentMode != SelCharaDeckCtrl.Mode.PHOTO_TOP && this.currentMode != SelCharaDeckCtrl.Mode.ACCESSORY_TOP) || !(button == this.guiData.charaDeck.Btn_PartyName))
			{
				if (this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP && button == this.guiData.charaDeck.Btn_PhotoRemove)
				{
					if (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
					{
						return;
					}
					bool flag = false;
					int num = 0;
					while (!flag && num < this.currentDeckClone.equipPhotoList.Count)
					{
						if (this.currentDeckClone.GetHelperIndex() != num)
						{
							int num2 = 0;
							while (!flag && num2 < this.currentDeckClone.equipPhotoList[num].Count)
							{
								if (this.currentDeckClone.equipPhotoList[num][num2] > 0L)
								{
									flag = true;
								}
								num2++;
							}
						}
						num++;
					}
					if (flag)
					{
						CanvasManager.HdlOpenWindowBasic.Setup("フォト一括解除確認", "選択中のパーティに装着されている\n全てのフォトを外します。\n\nよろしいですか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int idx)
						{
							if (idx == 1)
							{
								this.RemovePhoto();
							}
							return true;
						}, null, false);
						CanvasManager.HdlOpenWindowBasic.Open();
						return;
					}
				}
				else
				{
					if ((this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.DECK_EDIT || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP) && this.guiData.charaDeck.EnemyInfo.activeSelf && this.guiData.charaDeck.Btn_EnemyInfo.gameObject.activeSelf && button == this.guiData.charaDeck.Btn_EnemyInfo)
					{
						this.currentMode = SelCharaDeckCtrl.Mode.OW_ENEMY_INFO;
						SceneTraining.SetupInfo(this.dayOfWeekData, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback));
						return;
					}
					if ((this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP) && button == this.guiData.charaDeck.Btn_OrderCardInfo)
					{
						this.guiData.orderCardWindow.owCtrl.Open();
					}
				}
				return;
			}
			if (this.setupParam.deckCategory == UserDeckData.Category.TRAINING)
			{
				return;
			}
			if (!CanvasManager.HdlOpenWindowPartyName.FinishedClose())
			{
				return;
			}
			this.currentMode = SelCharaDeckCtrl.Mode.OW_PARTY_NAME;
			InputField component = CanvasManager.HdlOpenWindowPartyName.m_UserInfoContent.GetComponent<InputField>();
			component.text = this.currentDeckClone.name;
			component.textComponent.text = this.currentDeckClone.name;
			CanvasManager.HdlOpenWindowPartyName.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES_EMPTY), true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			CanvasManager.HdlOpenWindowPartyName.Open();
			return;
		}
	}

	// Token: 0x06001E2B RID: 7723 RVA: 0x00178DD4 File Offset: 0x00176FD4
	private void SetPartyName(string name)
	{
		int num = this.deckCloneList.IndexOf(this.deckCloneList.Find((UserDeckData itm) => itm.id == this.currentDeckClone.id));
		if (num < 0)
		{
			return;
		}
		if (string.IsNullOrEmpty(name))
		{
			name = (num + 1).ToString();
		}
		if (this.currentDeckClone.name == name)
		{
			return;
		}
		this.isChangeClone = true;
		this.currentDeckClone.name = name;
		this.guiData.charaDeck.PartyName.transform.Find("Txt_PartyName").GetComponent<PguiTextCtrl>().text = this.currentDeckClone.name;
		this.SetTabName(num, this.currentDeckClone.name);
	}

	// Token: 0x06001E2C RID: 7724 RVA: 0x00178E8C File Offset: 0x0017708C
	private void RemovePhoto()
	{
		for (int i = 0; i < this.currentDeckClone.equipPhotoList.Count; i++)
		{
			if (this.currentDeckClone.GetHelperIndex() != i)
			{
				for (int j = 0; j < this.currentDeckClone.equipPhotoList[i].Count; j++)
				{
					if (this.currentDeckClone.equipPhotoList[i][j] > 0L)
					{
						this.currentDeckClone.equipPhotoList[i][j] = 0L;
					}
				}
			}
		}
		this.isChangeClone = true;
		this.ChangeDeckInfo(-1);
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x00178F28 File Offset: 0x00177128
	private void SetStaySkillAnim()
	{
		if (!this.guiData.charaDeck.StaySkillSettingAnim.gameObject.activeSelf && this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP)
		{
			this.guiData.charaDeck.StaySkillSettingAnim.gameObject.SetActive(true);
			this.guiData.charaDeck.StaySkillSettingAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			return;
		}
		this.guiData.charaDeck.StaySkillSettingAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			this.guiData.charaDeck.StaySkillSettingAnim.gameObject.SetActive(false);
		});
	}

	// Token: 0x06001E2E RID: 7726 RVA: 0x00178FB0 File Offset: 0x001771B0
	private void OnClickMasterSkillButton(PguiButtonCtrl button)
	{
		SelCharaDeckCtrl.GUI.MasterSkillButton masterSkillButton = this.guiData.owMasterSkill.masterSkillButtonList.Find((SelCharaDeckCtrl.GUI.MasterSkillButton item) => item.CharaDeck_UserSkill_Btn == button);
		if (masterSkillButton != null)
		{
			for (int i = 0; i < this.guiData.owMasterSkill.masterSkillButtonList.Count; i++)
			{
				this.guiData.owMasterSkill.masterSkillButtonList[i].Current.SetActive(masterSkillButton == this.guiData.owMasterSkill.masterSkillButtonList[i]);
			}
		}
		this.guiData.owMasterSkill.choiceMasterSkillId = this.haveMasterSkillList[int.Parse(button.name)].id;
	}

	// Token: 0x06001E2F RID: 7727 RVA: 0x00179078 File Offset: 0x00177278
	private void OnClickTacticsChangeButton(PguiButtonCtrl button)
	{
		if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP)
		{
			this.currentMode = SelCharaDeckCtrl.Mode.OW_TACTICS_SKILL_INFO;
			this.OpenTacticsSkillInfoWindow();
		}
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x001790A4 File Offset: 0x001772A4
	private bool OnSelectOpenWindowButtonCallback(int index)
	{
		if (index == PguiOpenWindowCtrl.CLOSE_BUTTON_INDEX)
		{
			this.currentMode = this.preMode;
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.OW_MASTER_SKILL)
		{
			if (index == 1)
			{
				this.currentDeckClone.masterSkillId = this.guiData.owMasterSkill.choiceMasterSkillId;
				MasterStaticSkill masterSkillStaticData = DataManager.DmChara.GetMasterSkillStaticData(this.currentDeckClone.masterSkillId);
				this.guiData.charaDeck.Txt_SkillName.text = masterSkillStaticData.skillName;
				DataManagerMasterSkill.MasterSkillData masterSkillData = DataManager.DmMasterSkill.UserMasterSkillDataList.Find((DataManagerMasterSkill.MasterSkillData item) => item.SkillId == this.currentDeckClone.masterSkillId);
				if (masterSkillData != null)
				{
					this.guiData.charaDeck.Txt_SkillLevel.ReplaceTextByDefault("Param01", masterSkillData.Level.ToString());
				}
				this.guiData.charaDeck.Icon_Skill.SetImageByName(masterSkillStaticData.iconMiniName);
				this.isChangeClone = true;
				this.currentEnumerator = this.RequestUpdateDeck(delegate
				{
					this.TouchRect = true;
					this.currentMode = this.preMode;
				});
			}
			else
			{
				this.currentMode = this.preMode;
			}
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.OW_DISCARD_CHARA_RETRUEN || this.currentMode == SelCharaDeckCtrl.Mode.OW_DISCARD_CHARA_MOVE_SCENE)
		{
			if (index == 1)
			{
				this.currentEnumerator = this.RequestUpdateDeck(delegate
				{
					this.currentMode = this.preMode;
					if (this.preMode != SelCharaDeckCtrl.Mode.OW_DISCARD_CHARA_RETRUEN)
					{
						CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.OnClickMoveSequenceName, this.OnClickMoveSequenceArgs);
						return;
					}
					if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP)
					{
						this.forceReturnTop = true;
						return;
					}
					this.ChangeMode(SelCharaDeckCtrl.Mode.DECK_TOP);
				});
			}
			else
			{
				int id = this.currentDeckClone.id;
				this.ReloadDeckData(id);
				this.ChangeDeckInfo(id);
				this.currentMode = this.preMode;
				if (this.preMode == SelCharaDeckCtrl.Mode.OW_DISCARD_CHARA_RETRUEN)
				{
					if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP)
					{
						this.forceReturnTop = true;
					}
					else
					{
						this.ChangeMode(SelCharaDeckCtrl.Mode.DECK_TOP);
					}
				}
				else
				{
					CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.OnClickMoveSequenceName, this.OnClickMoveSequenceArgs);
				}
			}
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.OW_DISCARD_PHOTO_RETRUEN || this.currentMode == SelCharaDeckCtrl.Mode.OW_DISCARD_PHOTO_MOVE_SCENE)
		{
			if (index == 1)
			{
				this.currentEnumerator = this.RequestUpdateDeck(delegate
				{
					this.currentMode = this.preMode;
					if (this.preMode == SelCharaDeckCtrl.Mode.OW_DISCARD_PHOTO_RETRUEN)
					{
						this.ChangeMode(SelCharaDeckCtrl.Mode.PHOTO_TOP);
						return;
					}
					CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.OnClickMoveSequenceName, this.OnClickMoveSequenceArgs);
				});
			}
			else
			{
				int id2 = this.currentDeckClone.id;
				this.ReloadDeckData(id2);
				this.currentMode = this.preMode;
				if (this.preMode == SelCharaDeckCtrl.Mode.OW_DISCARD_PHOTO_RETRUEN)
				{
					this.ChangeMode(SelCharaDeckCtrl.Mode.PHOTO_TOP);
				}
				else
				{
					CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.OnClickMoveSequenceName, this.OnClickMoveSequenceArgs);
				}
			}
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.OW_DISCARD_CHARA_TAB)
		{
			if (index == 1)
			{
				this.currentEnumerator = this.RequestUpdateDeck(delegate
				{
					this.currentMode = this.preMode;
					this.guiData.charaDeck.DeckTab.SelectTab(this.selectTabIndexTemporary);
				});
			}
			else
			{
				this.currentMode = this.preMode;
				int id3 = this.currentDeckClone.id;
				this.ReloadDeckData(id3);
				this.ChangeDeckInfo(id3);
				this.guiData.charaDeck.DeckTab.SelectTab(this.selectTabIndexTemporary);
			}
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.OW_ERROR_ALLOUT_CHARA)
		{
			this.currentMode = this.preMode;
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.OW_PARTY_NAME)
		{
			if (index == 1)
			{
				this.SetPartyName(CanvasManager.HdlOpenWindowPartyName.m_UserInfoContent.GetComponent<InputField>().text);
			}
			this.currentMode = this.preMode;
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.OW_ENEMY_INFO)
		{
			this.currentMode = this.preMode;
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.OW_RECOMMEND)
		{
			this.currentMode = this.preMode;
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.OW_QUEST_SKIP)
		{
			this.currentMode = this.preMode;
		}
		return true;
	}

	// Token: 0x06001E31 RID: 7729 RVA: 0x00179408 File Offset: 0x00177608
	private bool OnClickToggle(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		this.touchRect = true;
		if (pbc == this.guiData.charaDeck.Btn_Chara)
		{
			string text = string.Empty;
			SelCharaDeckCtrl.Mode mode = this.currentMode;
			if (mode != SelCharaDeckCtrl.Mode.PHOTO_TOP)
			{
				if (mode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP)
				{
					text = "END2";
				}
			}
			else
			{
				text = "END";
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.currentMode = SelCharaDeckCtrl.Mode.DECK_TOP;
				for (int i = 0; i < this.guiData.charaDeck.iconCharaPacks.Count; i++)
				{
					SelCharaDeckCtrl.GUI.IconChara et2 = this.guiData.charaDeck.iconCharaPacks[i].iconChara;
					bool isHelper = this.currentDeckClone.GetHelperIndex() == i;
					et2.anime.ExPlayAnimation(text, delegate
					{
						if (!isHelper)
						{
							if (et2.PhotoIconView != null)
							{
								et2.PhotoIconView.SetActive(false);
							}
							if (et2.PhotoIconKind != null)
							{
								et2.PhotoIconKind.SetActive(true);
							}
						}
						else
						{
							if (et2.PhotoIconView != null)
							{
								et2.PhotoIconView.SetActive(false);
							}
							if (et2.PhotoIconKind != null)
							{
								et2.PhotoIconKind.SetActive(this.isEnemyInfo);
							}
						}
						if (et2.AccessoryIconView != null)
						{
							et2.AccessoryIconView.SetActive(false);
						}
					});
				}
				this.guiData.charaDeck.Btn_Photo.SetToggleIndex(0);
				this.guiData.charaDeck.Btn_Accessory.SetToggleIndex(0);
				this.guiData.charaDeck.Btn_StaySkillSetting.gameObject.SetActive(true);
				return true;
			}
		}
		else if (pbc == this.guiData.charaDeck.Btn_Photo)
		{
			bool flag = false;
			SelCharaDeckCtrl.Mode mode = this.currentMode;
			if (mode != SelCharaDeckCtrl.Mode.DECK_TOP)
			{
				if (mode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP)
				{
					for (int j = 0; j < this.guiData.charaDeck.iconCharaPacks.Count; j++)
					{
						SelCharaDeckCtrl.GUI.IconChara et3 = this.guiData.charaDeck.iconCharaPacks[j].iconChara;
						if (this.currentDeckClone.GetHelperIndex() != j)
						{
							if (et3.PhotoIconView != null)
							{
								et3.PhotoIconView.SetActive(true);
							}
							if (et3.PhotoIconKind != null)
							{
								et3.PhotoIconKind.SetActive(true);
							}
						}
						else
						{
							if (et3.PhotoIconView != null)
							{
								et3.PhotoIconView.SetActive(this.isEnemyInfo);
							}
							if (et3.PhotoIconKind != null)
							{
								et3.PhotoIconKind.SetActive(this.isEnemyInfo);
							}
						}
						et3.anime.ExPlayAnimation("END3", delegate
						{
							if (et3.AccessoryIconView != null)
							{
								et3.AccessoryIconView.SetActive(false);
							}
						});
					}
					flag = true;
				}
			}
			else
			{
				for (int k = 0; k < this.guiData.charaDeck.iconCharaPacks.Count; k++)
				{
					SelCharaDeckCtrl.GUI.IconChara iconChara = this.guiData.charaDeck.iconCharaPacks[k].iconChara;
					if (this.currentDeckClone.GetHelperIndex() != k)
					{
						if (iconChara.PhotoIconView != null)
						{
							iconChara.PhotoIconView.SetActive(true);
						}
						if (iconChara.PhotoIconKind != null)
						{
							iconChara.PhotoIconKind.SetActive(true);
						}
					}
					else
					{
						if (iconChara.PhotoIconView != null)
						{
							iconChara.PhotoIconView.SetActive(this.isEnemyInfo);
						}
						if (iconChara.PhotoIconKind != null)
						{
							iconChara.PhotoIconKind.SetActive(this.isEnemyInfo);
						}
					}
					if (iconChara.AccessoryIconView != null)
					{
						iconChara.AccessoryIconView.SetActive(false);
					}
					iconChara.anime.ExPlayAnimation("START", null);
				}
				flag = true;
			}
			if (flag)
			{
				this.currentMode = SelCharaDeckCtrl.Mode.PHOTO_TOP;
				this.guiData.charaDeck.Btn_Chara.SetToggleIndex(0);
				this.guiData.charaDeck.Btn_Accessory.SetToggleIndex(0);
				this.guiData.charaDeck.Btn_StaySkillSetting.gameObject.SetActive(false);
				this.SetStaySkillAnim();
				return true;
			}
		}
		else if (pbc == this.guiData.charaDeck.Btn_Accessory)
		{
			bool flag2 = false;
			SelCharaDeckCtrl.Mode mode = this.currentMode;
			if (mode != SelCharaDeckCtrl.Mode.DECK_TOP)
			{
				if (mode == SelCharaDeckCtrl.Mode.PHOTO_TOP)
				{
					for (int l = 0; l < this.guiData.charaDeck.iconCharaPacks.Count; l++)
					{
						SelCharaDeckCtrl.GUI.IconChara et = this.guiData.charaDeck.iconCharaPacks[l].iconChara;
						if (this.currentDeckClone.GetHelperIndex() != l)
						{
							if (et.AccessoryIconView != null)
							{
								et.AccessoryIconView.SetActive(true);
							}
						}
						else if (et.AccessoryIconView != null)
						{
							et.AccessoryIconView.SetActive(this.isEnemyInfo);
						}
						et.anime.ExPlayAnimation("START3", delegate
						{
							if (et.PhotoIconView != null)
							{
								et.PhotoIconView.SetActive(false);
							}
							if (et.PhotoIconKind != null)
							{
								et.PhotoIconKind.SetActive(false);
							}
						});
					}
					flag2 = true;
				}
			}
			else
			{
				for (int m = 0; m < this.guiData.charaDeck.iconCharaPacks.Count; m++)
				{
					SelCharaDeckCtrl.GUI.IconChara iconChara2 = this.guiData.charaDeck.iconCharaPacks[m].iconChara;
					bool flag3 = this.currentDeckClone.GetHelperIndex() == m;
					if (iconChara2.PhotoIconView != null)
					{
						iconChara2.PhotoIconView.SetActive(false);
					}
					if (iconChara2.PhotoIconKind != null)
					{
						iconChara2.PhotoIconKind.SetActive(false);
					}
					if (!flag3)
					{
						if (iconChara2.AccessoryIconView != null)
						{
							iconChara2.AccessoryIconView.SetActive(true);
						}
					}
					else if (iconChara2.AccessoryIconView != null)
					{
						iconChara2.AccessoryIconView.SetActive(this.isEnemyInfo);
					}
					iconChara2.anime.ExPlayAnimation("START2", null);
				}
				flag2 = true;
			}
			if (flag2)
			{
				this.currentMode = SelCharaDeckCtrl.Mode.ACCESSORY_TOP;
				this.guiData.charaDeck.Btn_Chara.SetToggleIndex(0);
				this.guiData.charaDeck.Btn_Photo.SetToggleIndex(0);
				this.guiData.charaDeck.Btn_StaySkillSetting.gameObject.SetActive(false);
				this.SetStaySkillAnim();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001E32 RID: 7730 RVA: 0x00179A60 File Offset: 0x00177C60
	private bool OnClickToggleStaySkill(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		this.touchRect = true;
		PguiToggleButtonCtrl pguiToggleButtonCtrl = this.guiData.charaDeck.Btn_StaySkills.Find((PguiToggleButtonCtrl item) => item == pbc);
		int num = this.guiData.charaDeck.Btn_StaySkills.FindIndex((PguiToggleButtonCtrl item) => item == pbc);
		if (pguiToggleButtonCtrl != null)
		{
			bool flag = true;
			if (this.currentDeckClone.waitSkillList.Count > num)
			{
				flag = (this.currentDeckClone.waitSkillList[num] = !this.currentDeckClone.waitSkillList[num]);
				this.isChangeClone = true;
			}
			PguiTextCtrl component = pguiToggleButtonCtrl.transform.Find("BaseImage/Txt_ONOFF").GetComponent<PguiTextCtrl>();
			component.text = (flag ? "ON" : "OFF");
			this.guiData.charaDeck.Txt_StaySkill_Switches[num].text = component.text;
			this.guiData.charaDeck.Txt_StaySkill_Switches[num].m_Text.color = this.guiData.charaDeck.Txt_StaySkill_Switches[num].GetComponent<PguiColorCtrl>().GetGameObjectById(component.text);
			pguiToggleButtonCtrl.SetToggleIndex(flag ? 0 : 1);
			return true;
		}
		return false;
	}

	// Token: 0x06001E33 RID: 7731 RVA: 0x00179BC0 File Offset: 0x00177DC0
	private bool OnSelectDeckTab(int index)
	{
		if (this.isChangeClone)
		{
			this.currentMode = SelCharaDeckCtrl.Mode.OW_DISCARD_CHARA_TAB;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("編成が変更されています"), SelCharaDeckCtrl.questionButtonSet, true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			this.selectTabIndexTemporary = index;
			return false;
		}
		this.ChangeDeckInfo(DataManager.DmDeck.GetUserDeckList(this.setupParam.deckCategory)[index].id);
		return true;
	}

	// Token: 0x06001E34 RID: 7732 RVA: 0x00179C48 File Offset: 0x00177E48
	private void OnTouchCharaIcon(SelCharaDeckCtrl.FrameType type, SelCharaDeckCtrl.GUI.IconChara iconChara, int selPhoto)
	{
		CharaPackData charaPackData = iconChara.iconCharaSet.iconCharaCtrl.charaPackData;
		if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP)
		{
			SoundManager.Play("prd_se_click", false, false);
			this.SelectCharaIcon(new SelCharaDeckCtrl.SelectCharaData(type, charaPackData), this.selectCharaData);
			this.guiData.charaDeck.DeckSelect.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
			{
				this.AnimeFinished = true;
			});
			List<CharaPackData> list = new List<CharaPackData>();
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.questOneId);
			foreach (CharaPackData charaPackData2 in this.dispCharaPackList)
			{
				if (QuestUtil.IsBanTarget(charaPackData2.dynamicData, questOnePackData, this.checkedCharaList))
				{
					list.Add(charaPackData2);
				}
			}
			CanvasManager.HdlOpenWindowSortFilter.SolutionList((this.setupParam.deckCategory == UserDeckData.Category.PVP) ? SortFilterDefine.RegisterType.CHARA_DECK_PVP : ((this.setupParam.deckCategory == UserDeckData.Category.TRAINING) ? SortFilterDefine.RegisterType.CHARA_DECK_TRAINING : SortFilterDefine.RegisterType.CHARA_DECK), list);
			this.currentMode = SelCharaDeckCtrl.Mode.DECK_EDIT;
			if (this.guiData.charaDeck.iconTacticsSkillBase.activeSelf)
			{
				this.guiData.charaDeck.iconTacticsSkillChangeMark.gameObject.SetActive(false);
			}
			this.TouchRect = true;
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_EDIT)
		{
			SoundManager.Play("prd_se_click", false, false);
			this.SelectCharaIcon(new SelCharaDeckCtrl.SelectCharaData(type, charaPackData), this.selectCharaData);
			this.TouchRect = true;
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP && iconChara.iconCharaSet.iconCharaCtrl.charaPackData != this.helperCharaData)
		{
			this.currentMode = SelCharaDeckCtrl.Mode.PHOTO_EDIT;
			this.guiData.charaDeck.baseObj.SetActive(false);
			if (this.selPhotoEditCtrl == null)
			{
				GameObject gameObject = new GameObject("PhotoEdit");
				RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
				rectTransform.anchorMin = new Vector2(0f, 0f);
				rectTransform.anchorMax = new Vector2(1f, 1f);
				rectTransform.offsetMin = new Vector2(0f, 0f);
				rectTransform.offsetMax = new Vector2(0f, 0f);
				this.selPhotoEditCtrl = gameObject.AddComponent<SelPhotoEditCtrl>();
				this.selPhotoEditCtrl.Init(SelPhotoEditCtrl.Type.Party);
				this.selPhotoEditCtrl.transform.SetParent(base.transform);
				Object.Destroy(this.selPhotoEditCtrl.GetComponent<SafeAreaScaler>());
			}
			SelCharaDeckCtrl.RequestCallBack <>9__12;
			this.selPhotoEditCtrl.Setup(new SelPhotoEditCtrl.SetupParam
			{
				cbButtonEditOk = delegate(PguiButtonCtrl button)
				{
					SelCharaDeckCtrl <>4__this = this;
					SelCharaDeckCtrl <>4__this2 = this;
					SelCharaDeckCtrl.RequestCallBack requestCallBack;
					if ((requestCallBack = <>9__12) == null)
					{
						requestCallBack = (<>9__12 = delegate
						{
							this.ChangeMode(SelCharaDeckCtrl.Mode.PHOTO_TOP);
						});
					}
					<>4__this.currentEnumerator = <>4__this2.RequestUpdateDeck(requestCallBack);
					return true;
				},
				cbButtonArrow = delegate(PguiButtonCtrl button)
				{
					this.selPhotoEditCtrl.ResetCurrentIcon();
					this.selPhotoEditCtrl.setupParam.indexByDeckChara += ((button == this.selPhotoEditCtrl.guiData.photoDeck.Btn_Yaji_Left) ? (-1) : 1);
					this.selPhotoEditCtrl.setupParam.indexByDeckChara = (this.selPhotoEditCtrl.setupParam.indexByDeckChara + 5) % 5;
					if (this.guiData.charaDeck.iconCharaPacks[this.selPhotoEditCtrl.setupParam.indexByDeckChara].iconChara.iconCharaSet.iconCharaCtrl.charaPackData == this.helperCharaData)
					{
						this.selPhotoEditCtrl.setupParam.indexByDeckChara += ((button == this.selPhotoEditCtrl.guiData.photoDeck.Btn_Yaji_Left) ? (-1) : 1);
						this.selPhotoEditCtrl.setupParam.indexByDeckChara = (this.selPhotoEditCtrl.setupParam.indexByDeckChara + 5) % 5;
					}
					this.selPhotoEditCtrl.ChangePhotoInfo(-1);
					this.selPhotoEditCtrl.guiData.photoDeck.ScrollView.Refresh();
					return true;
				},
				indexByDeckChara = this.guiData.charaDeck.iconCharaPacks.FindIndex((SelCharaDeckCtrl.GUI.IconCharaPack item) => item.iconChara == iconChara),
				cbGetEquipPhoto = () => this.currentDeckClone.equipPhotoList[this.selPhotoEditCtrl.setupParam.indexByDeckChara],
				cbGetCharaPackData = () => this.guiData.charaDeck.iconCharaPacks[this.selPhotoEditCtrl.setupParam.indexByDeckChara].iconChara.iconCharaSet.iconCharaCtrl.charaPackData,
				CurrentUserPartyId = () => this.guiData.charaDeck.iconCharaPacks.ConvertAll<int>((SelCharaDeckCtrl.GUI.IconCharaPack x) => x.iconChara.iconCharaSet.iconCharaCtrl.charaPackData.id),
				cbGetAllEquipPhoto = delegate
				{
					List<long> list2 = new List<long>();
					for (int i = 0; i < this.currentDeckClone.equipPhotoList.Count; i++)
					{
						for (int j = 0; j < this.currentDeckClone.equipPhotoList[i].Count; j++)
						{
							long num = this.currentDeckClone.equipPhotoList[i][j];
							if (num > 0L && !list2.Contains(num))
							{
								list2.Add(num);
							}
						}
					}
					return list2;
				},
				cbIsEquipPhoto = (long photoDataId) => this.currentDeckClone.IsEquipPhoto(photoDataId),
				cbSetPartyName = () => this.currentDeckClone.name,
				cbResignEquipPhotoByDataId = delegate(long photoDataId)
				{
					this.ResignEquipPhotoByDataId(photoDataId);
					return true;
				},
				deckCategory = this.setupParam.deckCategory,
				pvpSeasonId = this.setupParam.pvpSeasonId,
				EventId = QuestUtil.GetEventId(this.questOneId, false),
				PlayQuestOneId = this.questOneId,
				isTutorial = this.isTutorial,
				isHelper = false
			});
			this.ResetCurrentIcon();
			this.selPhotoEditCtrl.ChangePhotoInfo(selPhoto);
			this.TouchRect = true;
		}
		else if (this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP && iconChara.iconCharaSet.iconCharaCtrl.charaPackData != this.helperCharaData)
		{
			SoundManager.Play("prd_se_click", false, false);
			if (charaPackData != null && !charaPackData.IsInvalid())
			{
				CanvasManager.HdlDetachableAccessoryWindowCtrl.Open(charaPackData, delegate
				{
					this.ChangeDeckInfo(-1);
				});
			}
		}
		iconChara.iconCharaSet.iconCharaCtrl.IsEnableMask(iconChara.iconCharaSet.selected.activeSelf || iconChara.iconCharaSet.disable.activeSelf);
	}

	// Token: 0x06001E35 RID: 7733 RVA: 0x0017A0CC File Offset: 0x001782CC
	private void OnTouchTacticsSkillIcon()
	{
		if (this.currentMode == SelCharaDeckCtrl.Mode.DECK_TOP || this.currentMode == SelCharaDeckCtrl.Mode.PHOTO_TOP || this.currentMode == SelCharaDeckCtrl.Mode.ACCESSORY_TOP)
		{
			SoundManager.Play("prd_se_click", false, false);
			this.currentMode = SelCharaDeckCtrl.Mode.OW_TACTICS_SKILL_INFO;
			this.OpenTacticsSkillInfoWindow();
		}
	}

	// Token: 0x06001E36 RID: 7734 RVA: 0x0017A108 File Offset: 0x00178308
	private void OnStartItemChara(int index, GameObject go)
	{
		for (int i = 0; i < 2; i++)
		{
			SelCharaDeckCtrl.GUI.IconChara et = new SelCharaDeckCtrl.GUI.IconChara(go.transform.Find("Icon_Chara0" + (i + 1).ToString()), go.transform.Find("AEImage_Mark_Ban0" + (i + 1).ToString()), -1);
			et.iconCharaSet.iconCharaCtrl.AddOnClickListener(delegate(IconCharaCtrl x)
			{
				this.OnTouchCharaIcon(SelCharaDeckCtrl.FrameType.RESERVE, et, -1);
			});
			Transform transform = et.iconCharaSet.baseObj.transform.Find("Remove");
			if (transform != null)
			{
				PrjUtil.AddTouchEventTrigger(transform.gameObject, delegate(Transform x)
				{
					this.OnTouchCharaIcon(SelCharaDeckCtrl.FrameType.RESERVE, et, -1);
				});
			}
			this.guiData.reserveCharaIcon.Add(et);
		}
	}

	// Token: 0x06001E37 RID: 7735 RVA: 0x0017A1F8 File Offset: 0x001783F8
	private void OnUpdateItemChara(int index, GameObject go)
	{
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.questOneId);
		bool flag = questOnePackData != null && questOnePackData.questOne.QuestCategory == QuestStaticQuestOne.QuestOneCategory.NoDhole;
		for (int i = 0; i < 2; i++)
		{
			GameObject iconObj = go.transform.Find("Icon_Chara0" + (i + 1).ToString()).gameObject;
			SelCharaDeckCtrl.GUI.IconChara iconChara = this.guiData.reserveCharaIcon.Find((SelCharaDeckCtrl.GUI.IconChara item) => item.baseObj == iconObj);
			int num = index * 2 + i;
			if (this.dispCharaPackList.Count > num)
			{
				iconChara.baseObj.SetActive(true);
				CharaPackData charaPackData = this.dispCharaPackList[num];
				CharaWindowCtrl.DetailParamSetting.Preset preset = (this.isEnemyInfo ? CharaWindowCtrl.DetailParamSetting.Preset.NO_VIEW : CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY);
				IconCharaCtrl iconCharaCtrl = iconChara.iconCharaSet.iconCharaCtrl;
				CharaPackData charaPackData2 = charaPackData;
				SortFilterDefine.SortType sortType = this.sortType;
				bool flag2 = false;
				CharaWindowCtrl.DetailParamSetting detailParamSetting2;
				if (!this.isTutorial)
				{
					CharaWindowCtrl.DetailParamSetting detailParamSetting = new CharaWindowCtrl.DetailParamSetting(preset, null);
					detailParamSetting.pvpSeasonId = this.setupParam.pvpSeasonId;
					detailParamSetting.selectEventId = QuestUtil.GetEventId(this.questOneId, false);
					detailParamSetting.selectQuestOneId = this.questOneId;
					detailParamSetting2 = detailParamSetting;
					detailParamSetting.deckCategory = this.setupParam.deckCategory;
				}
				else
				{
					detailParamSetting2 = null;
				}
				iconCharaCtrl.Setup(charaPackData2, sortType, flag2, detailParamSetting2, QuestUtil.GetEventId(this.questOneId, false), -1, this.questOneId);
				iconChara.iconCharaSet.iconCharaCtrl.DispAttributeMark(this.enemyAttribute);
				iconChara.iconCharaSet.iconCharaCtrl.DispPhotoPocketLevel(true);
				if (DataManagerDeck.CheckDisableDropIcon(this.setupParam.deckCategory, this.setupParam.pvpSeasonId))
				{
					iconChara.iconCharaSet.iconCharaCtrl.DispMarkEvent(false, false, false);
				}
				this.SetupSelectDisable(iconChara, false);
				iconChara.iconCharaSet.removeFrame.SetActive(this.removeButttonCharaData == charaPackData);
				iconChara.iconCharaSet.currentFrame.SetActive(this.selectCharaData != null && this.selectCharaData.type == SelCharaDeckCtrl.FrameType.RESERVE && this.selectCharaData.chara == charaPackData);
				bool flag3 = charaPackData.id == 0;
				bool flag4 = DataManager.DmChara.CheckSameChara(charaPackData.id, 1);
				bool flag5 = flag && !flag3 && flag4;
				bool flag6 = QuestUtil.IsBanTarget(charaPackData.dynamicData, questOnePackData, this.checkedCharaList);
				bool flag7 = flag5 || flag6;
				iconChara.banObj.SetActive(flag7);
			}
			else
			{
				iconChara.baseObj.SetActive(false);
				iconChara.banObj.SetActive(false);
			}
		}
	}

	// Token: 0x06001E38 RID: 7736 RVA: 0x0017A47C File Offset: 0x0017867C
	private void OnStartItemMasterSkill(int index, GameObject go)
	{
		SelCharaDeckCtrl.GUI.MasterSkillButton masterSkillButton = new SelCharaDeckCtrl.GUI.MasterSkillButton(go.transform);
		masterSkillButton.CharaDeck_UserSkill_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMasterSkillButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.owMasterSkill.masterSkillButtonList.Add(masterSkillButton);
	}

	// Token: 0x06001E39 RID: 7737 RVA: 0x0017A4C4 File Offset: 0x001786C4
	private void OnUpdateItemMasterSkill(int index, GameObject go)
	{
		if (index < this.haveMasterSkillList.Count)
		{
			go.SetActive(true);
			MasterSkillPackData mspd = this.haveMasterSkillList[index];
			SelCharaDeckCtrl.GUI.MasterSkillButton masterSkillButton = this.guiData.owMasterSkill.masterSkillButtonList.Find((SelCharaDeckCtrl.GUI.MasterSkillButton item) => item.baseObj == go);
			masterSkillButton.Current.SetActive(this.guiData.owMasterSkill.choiceMasterSkillId == mspd.id);
			masterSkillButton.Txt_UserSkill.text = mspd.staticData.skillName;
			DataManagerMasterSkill.MasterSkillData masterSkillData = DataManager.DmMasterSkill.UserMasterSkillDataList.Find((DataManagerMasterSkill.MasterSkillData item) => item.SkillId == mspd.id);
			if (masterSkillData != null)
			{
				masterSkillButton.Txt_UserSkillLevel.ReplaceTextByDefault("Param01", masterSkillData.Level.ToString());
			}
			masterSkillButton.Txt_SkillInfo.text = mspd.staticData.MakeSkillText(mspd.dynamicData.level, false);
			masterSkillButton.Icon_Skill.SetImageByName(mspd.staticData.iconMiniName);
			return;
		}
		go.SetActive(false);
	}

	// Token: 0x06001E3A RID: 7738 RVA: 0x0017A60C File Offset: 0x0017880C
	private void OpenTacticsSkillInfoWindow()
	{
		CanvasManager.HdlOpenWindowTacticsSkillInfo.Open(new SelTacticsSkillInfoWindowCtrl.SetupParam
		{
			pvpTacticsTypeId = this.currentDeckClone.pvpTacticsTypeId,
			pvpTacticsTermsTypeId = this.currentDeckClone.pvpTacticsTermsTypeId,
			pvpTacticsTermsValueId = this.currentDeckClone.pvpTacticsTermsValueId,
			closeEndCb = new UnityAction(this.OnCloseTacticsSkillInfoWindowCallback)
		});
	}

	// Token: 0x06001E3B RID: 7739 RVA: 0x0017A670 File Offset: 0x00178870
	private void OnCloseTacticsSkillInfoWindowCallback()
	{
		SelTacticsSkillInfoWindowCtrl.EditResultData editResultData = CanvasManager.HdlOpenWindowTacticsSkillInfo.GetEditResultData();
		if (this.currentDeckClone.pvpTacticsTypeId != editResultData.pvpTacticsTypeId)
		{
			this.isChangeClone = true;
			this.currentDeckClone.pvpTacticsTypeId = editResultData.pvpTacticsTypeId;
			this.guiData.charaDeck.iconTacticsSkill.Replace(this.currentDeckClone.pvpTacticsTypeId);
			TacticsStaticSkill tacticsSkillStaticData = DataManager.DmChara.GetTacticsSkillStaticData(this.currentDeckClone.pvpTacticsTypeId);
			this.guiData.charaDeck.Txt_SkillName.text = tacticsSkillStaticData.skillName;
		}
		if (this.currentDeckClone.pvpTacticsTermsTypeId != editResultData.pvpTacticsTermsTypeId)
		{
			this.isChangeClone = true;
			this.currentDeckClone.pvpTacticsTermsTypeId = editResultData.pvpTacticsTermsTypeId;
		}
		if (this.currentDeckClone.pvpTacticsTermsValueId != editResultData.pvpTacticsTermsValueId)
		{
			this.isChangeClone = true;
			this.currentDeckClone.pvpTacticsTermsValueId = editResultData.pvpTacticsTermsValueId;
		}
		this.currentEnumerator = this.RequestUpdateDeck(delegate
		{
			this.currentMode = this.preMode;
		});
	}

	// Token: 0x04001606 RID: 5638
	private static readonly List<string> weekList = new List<string> { "月曜日", "火曜日", "水曜日", "木曜日", "金曜日", "土曜日", "日曜日" };

	// Token: 0x04001607 RID: 5639
	public bool isDebug;

	// Token: 0x04001608 RID: 5640
	public SelCharaDeckCtrl.SetupParam setupParam = new SelCharaDeckCtrl.SetupParam();

	// Token: 0x04001609 RID: 5641
	private bool isEnemyInfo;

	// Token: 0x0400160A RID: 5642
	private int questOneId = -1;

	// Token: 0x0400160B RID: 5643
	private CharaDef.AttributeMask enemyAttribute;

	// Token: 0x0400160C RID: 5644
	private IEnumerator currentEnumerator;

	// Token: 0x0400160D RID: 5645
	private bool isCheckKizunaLimit;

	// Token: 0x0400160E RID: 5646
	private bool isCheckBanned;

	// Token: 0x0400160F RID: 5647
	public bool isTutorial;

	// Token: 0x04001610 RID: 5648
	private SelCharaDeckCtrl.GUI guiData;

	// Token: 0x04001611 RID: 5649
	private SelPhotoEditCtrl selPhotoEditCtrl;

	// Token: 0x04001612 RID: 5650
	private SelCharaDeckCtrl.Mode _currentMode;

	// Token: 0x04001613 RID: 5651
	private SelCharaDeckCtrl.Mode _preMode;

	// Token: 0x04001614 RID: 5652
	private bool touchRect;

	// Token: 0x04001615 RID: 5653
	private bool animeFinished;

	// Token: 0x04001616 RID: 5654
	private List<UserDeckData> deckCloneList;

	// Token: 0x04001617 RID: 5655
	private UserDeckData currentDeckClone;

	// Token: 0x04001618 RID: 5656
	private List<CharaPackData> haveCharaPackList;

	// Token: 0x04001619 RID: 5657
	private CharaPackData helperCharaData;

	// Token: 0x0400161A RID: 5658
	private HelperPackData helperPackData;

	// Token: 0x0400161B RID: 5659
	private List<PhotoPackData> havePhotoPackList;

	// Token: 0x0400161C RID: 5660
	private CharaPackData removeButttonCharaData = CharaPackData.MakeInvalid();

	// Token: 0x0400161D RID: 5661
	private PhotoPackData removeButttonPhotoData = PhotoPackData.MakeInvalid();

	// Token: 0x0400161E RID: 5662
	private List<MasterSkillPackData> haveMasterSkillList;

	// Token: 0x0400161F RID: 5663
	private List<CharaPackData> dispCharaPackList;

	// Token: 0x04001620 RID: 5664
	private bool isChangeClone;

	// Token: 0x04001621 RID: 5665
	private int selectTabIndexTemporary;

	// Token: 0x04001622 RID: 5666
	private SelCharaDeckCtrl.SelectCharaData selectCharaData;

	// Token: 0x04001623 RID: 5667
	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	// Token: 0x04001624 RID: 5668
	private TrainingStaticData.DayOfWeekData dayOfWeekData;

	// Token: 0x04001625 RID: 5669
	private QuestUtil.UsrQuestSkipInfo skipInfo;

	// Token: 0x04001626 RID: 5670
	private int skipCount = -1;

	// Token: 0x04001627 RID: 5671
	private List<CharaStaticData> checkedCharaList = new List<CharaStaticData>();

	// Token: 0x04001628 RID: 5672
	private List<CharaPackData> bannedList = new List<CharaPackData>();

	// Token: 0x04001629 RID: 5673
	public static readonly List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> questionButtonSet = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
	{
		new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, PrjUtil.MakeMessage("破棄して移動")),
		new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, PrjUtil.MakeMessage("保存して移動"))
	};

	// Token: 0x0400162A RID: 5674
	private SceneManager.SceneName OnClickMoveSequenceName;

	// Token: 0x0400162B RID: 5675
	private object OnClickMoveSequenceArgs;

	// Token: 0x0400162C RID: 5676
	private bool forceReturnTop;

	// Token: 0x02000F77 RID: 3959
	public enum FrameType
	{
		// Token: 0x0400577B RID: 22395
		INVALID,
		// Token: 0x0400577C RID: 22396
		DECK,
		// Token: 0x0400577D RID: 22397
		RESERVE
	}

	// Token: 0x02000F78 RID: 3960
	public enum Mode
	{
		// Token: 0x0400577F RID: 22399
		INVALID,
		// Token: 0x04005780 RID: 22400
		DECK_TOP,
		// Token: 0x04005781 RID: 22401
		DECK_EDIT,
		// Token: 0x04005782 RID: 22402
		PHOTO_TOP,
		// Token: 0x04005783 RID: 22403
		PHOTO_EDIT,
		// Token: 0x04005784 RID: 22404
		GO_BATTLE,
		// Token: 0x04005785 RID: 22405
		OW_DISCARD_CHARA_RETRUEN,
		// Token: 0x04005786 RID: 22406
		OW_DISCARD_PHOTO_RETRUEN,
		// Token: 0x04005787 RID: 22407
		OW_DISCARD_CHARA_TAB,
		// Token: 0x04005788 RID: 22408
		OW_MASTER_SKILL,
		// Token: 0x04005789 RID: 22409
		OW_ERROR_ALLOUT_CHARA,
		// Token: 0x0400578A RID: 22410
		OW_DISCARD_CHARA_MOVE_SCENE,
		// Token: 0x0400578B RID: 22411
		OW_DISCARD_PHOTO_MOVE_SCENE,
		// Token: 0x0400578C RID: 22412
		OW_PARTY_NAME,
		// Token: 0x0400578D RID: 22413
		OW_ENEMY_INFO,
		// Token: 0x0400578E RID: 22414
		OW_RECOMMEND,
		// Token: 0x0400578F RID: 22415
		OW_TACTICS_SKILL_INFO,
		// Token: 0x04005790 RID: 22416
		ACCESSORY_TOP,
		// Token: 0x04005791 RID: 22417
		OW_QUEST_SKIP
	}

	// Token: 0x02000F79 RID: 3961
	public class SetupParam
	{
		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06004FB2 RID: 20402 RVA: 0x0023C6FF File Offset: 0x0023A8FF
		// (set) Token: 0x06004FB3 RID: 20403 RVA: 0x0023C707 File Offset: 0x0023A907
		public SceneManager.SceneName callScene { get; set; }

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06004FB4 RID: 20404 RVA: 0x0023C710 File Offset: 0x0023A910
		// (set) Token: 0x06004FB5 RID: 20405 RVA: 0x0023C718 File Offset: 0x0023A918
		public SelCharaDeckCtrl.OnClickGotoBattle callbackGotoBattle { get; set; }

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06004FB6 RID: 20406 RVA: 0x0023C721 File Offset: 0x0023A921
		// (set) Token: 0x06004FB7 RID: 20407 RVA: 0x0023C729 File Offset: 0x0023A929
		public HelperPackData helperPackData { get; set; }

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06004FB8 RID: 20408 RVA: 0x0023C732 File Offset: 0x0023A932
		// (set) Token: 0x06004FB9 RID: 20409 RVA: 0x0023C73A File Offset: 0x0023A93A
		public bool isReload { get; set; }

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06004FBA RID: 20410 RVA: 0x0023C743 File Offset: 0x0023A943
		// (set) Token: 0x06004FBB RID: 20411 RVA: 0x0023C74B File Offset: 0x0023A94B
		public UserDeckData.Category deckCategory { get; set; }

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06004FBC RID: 20412 RVA: 0x0023C754 File Offset: 0x0023A954
		// (set) Token: 0x06004FBD RID: 20413 RVA: 0x0023C75C File Offset: 0x0023A95C
		public int pvpSeasonId { get; set; }

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06004FBE RID: 20414 RVA: 0x0023C765 File Offset: 0x0023A965
		// (set) Token: 0x06004FBF RID: 20415 RVA: 0x0023C76D File Offset: 0x0023A96D
		public DayOfWeek trainingDay { get; set; }

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06004FC0 RID: 20416 RVA: 0x0023C776 File Offset: 0x0023A976
		// (set) Token: 0x06004FC1 RID: 20417 RVA: 0x0023C77E File Offset: 0x0023A97E
		public int attrIndex { get; set; }

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06004FC2 RID: 20418 RVA: 0x0023C787 File Offset: 0x0023A987
		// (set) Token: 0x06004FC3 RID: 20419 RVA: 0x0023C78F File Offset: 0x0023A98F
		public bool isPractice { get; set; }

		// Token: 0x06004FC4 RID: 20420 RVA: 0x0023C798 File Offset: 0x0023A998
		public SetupParam()
		{
			this.isReload = true;
			this.deckCategory = UserDeckData.Category.NORMAL;
		}
	}

	// Token: 0x02000F7A RID: 3962
	public class SelectCharaData
	{
		// Token: 0x06004FC5 RID: 20421 RVA: 0x0023C7AE File Offset: 0x0023A9AE
		public SelectCharaData(SelCharaDeckCtrl.FrameType t, CharaPackData c)
		{
			this.type = t;
			this.chara = c;
		}

		// Token: 0x0400579B RID: 22427
		public SelCharaDeckCtrl.FrameType type;

		// Token: 0x0400579C RID: 22428
		public CharaPackData chara;
	}

	// Token: 0x02000F7B RID: 3963
	public class EditResultData
	{
		// Token: 0x0400579D RID: 22429
		public int currentDeckId;

		// Token: 0x0400579E RID: 22430
		public List<UserDeckData> editDeck = new List<UserDeckData>();

		// Token: 0x0400579F RID: 22431
		public HelperPackData helperData;
	}

	// Token: 0x02000F7C RID: 3964
	// (Invoke) Token: 0x06004FC8 RID: 20424
	public delegate bool OnClickGotoBattle();

	// Token: 0x02000F7D RID: 3965
	public class GUI
	{
		// Token: 0x06004FCB RID: 20427 RVA: 0x0023C7D8 File Offset: 0x0023A9D8
		public GUI(Transform baseTr)
		{
			this.charaDeck = new SelCharaDeckCtrl.GUI.CharaDeck(baseTr.Find("DeckSelect"), 5);
			this.owMasterSkill = new SelCharaDeckCtrl.GUI.CharaDeckWindow(CanvasManager.HdlOpenWindowUserSkill.transform);
			this.anim = baseTr.GetComponent<SimpleAnimation>();
			this.anim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.orderCardWindow = new SelCharaDeckCtrl.GUI.OrderCardWindow(CanvasManager.HdlOpenWindowOrderCard.transform);
			CanvasManager.HdlQuestSkipWindowsCtrl.Initialize();
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x0023C85C File Offset: 0x0023AA5C
		public SelCharaDeckCtrl.GUI.IconChara SearchIconChara(SelCharaDeckCtrl.SelectCharaData scd)
		{
			if (scd != null)
			{
				SelCharaDeckCtrl.GUI.IconChara iconChara = null;
				SelCharaDeckCtrl.FrameType type = scd.type;
				if (type != SelCharaDeckCtrl.FrameType.DECK)
				{
					if (type == SelCharaDeckCtrl.FrameType.RESERVE)
					{
						iconChara = this.reserveCharaIcon.Find((SelCharaDeckCtrl.GUI.IconChara item) => item.iconCharaSet.iconCharaCtrl.charaPackData == scd.chara);
					}
				}
				else
				{
					iconChara = this.charaDeck.iconCharaPacks.Find((SelCharaDeckCtrl.GUI.IconCharaPack item) => item.iconChara.iconCharaSet.iconCharaCtrl.charaPackData == scd.chara).iconChara;
				}
				return iconChara;
			}
			return null;
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x0023C8D8 File Offset: 0x0023AAD8
		public SelCharaDeckCtrl.GUI.IconChara SearchIconChara(SelCharaDeckCtrl.FrameType type, int id)
		{
			if (id != 0)
			{
				SelCharaDeckCtrl.GUI.IconChara iconChara = null;
				if (type != SelCharaDeckCtrl.FrameType.DECK)
				{
					if (type == SelCharaDeckCtrl.FrameType.RESERVE)
					{
						iconChara = this.reserveCharaIcon.Find((SelCharaDeckCtrl.GUI.IconChara item) => item.iconCharaSet.iconCharaCtrl.charaPackData.id == id);
					}
				}
				else
				{
					iconChara = this.charaDeck.iconCharaPacks.Find((SelCharaDeckCtrl.GUI.IconCharaPack item) => item.iconChara.iconCharaSet.iconCharaCtrl.charaPackData.id == id).iconChara;
				}
				return iconChara;
			}
			return null;
		}

		// Token: 0x040057A0 RID: 22432
		public SelCharaDeckCtrl.GUI.CharaDeck charaDeck;

		// Token: 0x040057A1 RID: 22433
		public SelCharaDeckCtrl.GUI.CharaDeckWindow owMasterSkill;

		// Token: 0x040057A2 RID: 22434
		public SimpleAnimation anim;

		// Token: 0x040057A3 RID: 22435
		public SelCharaDeckCtrl.GUI.OrderCardWindow orderCardWindow;

		// Token: 0x040057A4 RID: 22436
		public QuestSkipWindowsCtrl.QuestSkipWindow questSkipWindow;

		// Token: 0x040057A5 RID: 22437
		public SelCharaDeckCtrl.GUI.DeckReachedLimitListWindow reachedLimitListWindow;

		// Token: 0x040057A6 RID: 22438
		public List<SelCharaDeckCtrl.GUI.IconChara> reserveCharaIcon = new List<SelCharaDeckCtrl.GUI.IconChara>();

		// Token: 0x0200120C RID: 4620
		public class CharaDeckWindow
		{
			// Token: 0x060057A2 RID: 22434 RVA: 0x00257E0C File Offset: 0x0025600C
			public CharaDeckWindow(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Window = baseTr.GetComponent<PguiOpenWindowCtrl>();
				this.Txt_UserSkill = baseTr.Find("Base/Window/CurrentSkill/Txt_UserSkill").GetComponent<PguiTextCtrl>();
				this.Txt_UserSkillLevel = baseTr.Find("Base/Window/CurrentSkill/Num_Lv").GetComponent<PguiTextCtrl>();
				this.Txt_SkillInfo = baseTr.Find("Base/Window/CurrentSkill/Txt_SkillInfo").GetComponent<PguiTextCtrl>();
				this.ScrollView = baseTr.Find("Base/Window/InBase/ScrollView").GetComponent<ReuseScroll>();
				this.Icon_Skill = baseTr.Find("Base/Window/CurrentSkill/Icon_Skill").GetComponent<PguiImageCtrl>();
			}

			// Token: 0x040062C9 RID: 25289
			public GameObject baseObj;

			// Token: 0x040062CA RID: 25290
			public PguiTextCtrl Txt_UserSkill;

			// Token: 0x040062CB RID: 25291
			public PguiTextCtrl Txt_UserSkillLevel;

			// Token: 0x040062CC RID: 25292
			public PguiTextCtrl Txt_SkillInfo;

			// Token: 0x040062CD RID: 25293
			public PguiOpenWindowCtrl Window;

			// Token: 0x040062CE RID: 25294
			public ReuseScroll ScrollView;

			// Token: 0x040062CF RID: 25295
			public PguiImageCtrl Icon_Skill;

			// Token: 0x040062D0 RID: 25296
			public List<SelCharaDeckCtrl.GUI.MasterSkillButton> masterSkillButtonList = new List<SelCharaDeckCtrl.GUI.MasterSkillButton>();

			// Token: 0x040062D1 RID: 25297
			public int choiceMasterSkillId;
		}

		// Token: 0x0200120D RID: 4621
		public class MasterSkillButton
		{
			// Token: 0x060057A3 RID: 22435 RVA: 0x00257EB0 File Offset: 0x002560B0
			public MasterSkillButton(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.CharaDeck_UserSkill_Btn = baseTr.GetComponent<PguiButtonCtrl>();
				this.Current = baseTr.Find("BaseImage/Current").gameObject;
				this.Txt_UserSkill = baseTr.Find("BaseImage/Txt_UserSkill").GetComponent<PguiTextCtrl>();
				this.Txt_UserSkillLevel = baseTr.Find("BaseImage/Num_Lv").GetComponent<PguiTextCtrl>();
				this.Txt_SkillInfo = baseTr.Find("BaseImage/Txt_SkillInfo").GetComponent<PguiTextCtrl>();
				this.Icon_Skill = baseTr.Find("BaseImage/Icon_Skill").GetComponent<PguiImageCtrl>();
			}

			// Token: 0x040062D2 RID: 25298
			public GameObject baseObj;

			// Token: 0x040062D3 RID: 25299
			public PguiButtonCtrl CharaDeck_UserSkill_Btn;

			// Token: 0x040062D4 RID: 25300
			public GameObject Current;

			// Token: 0x040062D5 RID: 25301
			public PguiTextCtrl Txt_UserSkill;

			// Token: 0x040062D6 RID: 25302
			public PguiTextCtrl Txt_UserSkillLevel;

			// Token: 0x040062D7 RID: 25303
			public PguiTextCtrl Txt_SkillInfo;

			// Token: 0x040062D8 RID: 25304
			public PguiImageCtrl Icon_Skill;
		}

		// Token: 0x0200120E RID: 4622
		public class IconCharaSet
		{
			// Token: 0x060057A4 RID: 22436 RVA: 0x00257F4C File Offset: 0x0025614C
			public IconCharaSet(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.iconBase = baseTr.Find("Icon_Chara").GetComponent<RectTransform>();
				GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, this.iconBase);
				this.iconCharaCtrl = gameObject.GetComponent<IconCharaCtrl>();
				this.currentFrame = baseTr.Find("Current").gameObject;
				this.currentFrame.SetActive(false);
				Transform transform = baseTr.Find("AEImage_Eff_Change");
				if (transform != null)
				{
					this.aeEffChange = transform.GetComponent<PguiAECtrl>();
				}
				Transform transform2 = baseTr.Find("Remove");
				if (transform2 != null)
				{
					this.removeFrame = transform2.gameObject;
					this.removeFrame.SetActive(false);
				}
				this.selected = baseTr.Find("Fnt_Selected").gameObject;
				this.selected.SetActive(false);
				this.disable = baseTr.Find("Txt_Disable").gameObject;
				this.disable.SetActive(false);
			}

			// Token: 0x060057A5 RID: 22437 RVA: 0x00258058 File Offset: 0x00256258
			public void InvalidAE()
			{
				if (this.aeEffChange != null)
				{
					this.aeEffChange.gameObject.SetActive(false);
				}
				if (this.iconBase != null)
				{
					this.iconBase.gameObject.GetComponent<AELayerConstraint>().enabled = false;
				}
			}

			// Token: 0x040062D9 RID: 25305
			public GameObject baseObj;

			// Token: 0x040062DA RID: 25306
			public RectTransform iconBase;

			// Token: 0x040062DB RID: 25307
			public IconCharaCtrl iconCharaCtrl;

			// Token: 0x040062DC RID: 25308
			public GameObject currentFrame;

			// Token: 0x040062DD RID: 25309
			public GameObject removeFrame;

			// Token: 0x040062DE RID: 25310
			public GameObject selected;

			// Token: 0x040062DF RID: 25311
			public GameObject disable;

			// Token: 0x040062E0 RID: 25312
			public PguiAECtrl aeEffChange;
		}

		// Token: 0x0200120F RID: 4623
		public class IconChara
		{
			// Token: 0x060057A6 RID: 22438 RVA: 0x002580A8 File Offset: 0x002562A8
			public IconChara(Transform baseTr, Transform banTr, int deckIndex = -1)
			{
				this.baseObj = baseTr.gameObject;
				this.anime = baseTr.GetComponent<SimpleAnimation>();
				if (this.anime != null)
				{
					this.anime.ExInit();
					this.anime.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
				}
				if (banTr == null)
				{
					banTr = baseTr.Find("AEImage_Mark_Ban");
				}
				this.banObj = banTr.gameObject;
				this.banObj.SetActive(false);
				GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_CharaSet, baseTr);
				this.iconCharaSet = new SelCharaDeckCtrl.GUI.IconCharaSet(gameObject.transform);
				Transform transform = baseTr.Find("Icon_Chara");
				if (transform != null)
				{
					this.iconCharaSet.baseObj.transform.SetParent(transform, false);
				}
				Transform transform2 = baseTr.Find("PhotoIconView");
				if (transform2)
				{
					this.photoRect = new RectTransform[]
					{
						transform2.Find("Icon_Photo01").GetComponent<RectTransform>(),
						transform2.Find("Icon_Photo02").GetComponent<RectTransform>(),
						transform2.Find("Icon_Photo03").GetComponent<RectTransform>(),
						transform2.Find("Icon_Photo04").GetComponent<RectTransform>()
					};
					this.iconPhotoCtrl = new List<IconPhotoCtrl>();
					this.iconBlankFrame = new List<PguiReplaceSpriteCtrl>();
					for (int i = 0; i < this.photoRect.Length; i++)
					{
						GameObject gameObject2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo_Mini, this.photoRect[i].Find("Icon_Photo").transform);
						gameObject2.transform.SetSiblingIndex(0);
						this.iconPhotoCtrl.Add(gameObject2.GetComponent<IconPhotoCtrl>());
						this.iconBlankFrame.Add(this.photoRect[i].Find("Img_Blank").GetComponent<PguiReplaceSpriteCtrl>());
					}
					this.PhotoIconView = transform2.gameObject;
				}
				Transform transform3 = baseTr.Find("PhotoIconKind");
				if (transform3)
				{
					this.iconPhotoKind = new List<PguiReplaceSpriteCtrl>
					{
						transform3.Find("Icon_PhotoKind01").GetComponent<PguiReplaceSpriteCtrl>(),
						transform3.Find("Icon_PhotoKind02").GetComponent<PguiReplaceSpriteCtrl>(),
						transform3.Find("Icon_PhotoKind03").GetComponent<PguiReplaceSpriteCtrl>(),
						transform3.Find("Icon_PhotoKind04").GetComponent<PguiReplaceSpriteCtrl>()
					};
					this.iconStatusKind = new List<PguiReplaceSpriteCtrl>
					{
						transform3.Find("Icon_PhotoKind01/Icon_Status").GetComponent<PguiReplaceSpriteCtrl>(),
						transform3.Find("Icon_PhotoKind02/Icon_Status").GetComponent<PguiReplaceSpriteCtrl>(),
						transform3.Find("Icon_PhotoKind03/Icon_Status").GetComponent<PguiReplaceSpriteCtrl>(),
						transform3.Find("Icon_PhotoKind04/Icon_Status").GetComponent<PguiReplaceSpriteCtrl>()
					};
					this.PhotoIconKind = transform3.gameObject;
				}
				if (baseTr.Find("Mark_Friend"))
				{
					this.Mark_Friend = baseTr.Find("Mark_Friend").gameObject;
				}
				if (baseTr.Find("Base_CharaBlank"))
				{
					this.Base_CharaBlank = baseTr.Find("Base_CharaBlank").GetComponent<PguiReplaceSpriteCtrl>();
				}
				Transform transform4 = baseTr.Find("Base_CharaBlank_Friend");
				if (transform4)
				{
					this.Base_CharaBlank_Friend = transform4.gameObject;
				}
				Transform transform5 = baseTr.Find("AccessoryIconView");
				if (transform5)
				{
					this.accessoryRect = transform5.Find("Icon_Accessory01").GetComponent<RectTransform>();
					GameObject gameObject3 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Accessory_Mini, this.accessoryRect.Find("Icon_accessory").transform);
					gameObject3.transform.SetSiblingIndex(0);
					this.iconAccessoryCtrl = gameObject3.GetComponent<IconAccessoryCtrl>();
					this.iconAccessoryBlankFrame = this.accessoryRect.Find("Img_Blank").GetComponent<PguiReplaceSpriteCtrl>();
					this.AccessoryIconView = transform5.gameObject;
				}
			}

			// Token: 0x060057A7 RID: 22439 RVA: 0x0025847C File Offset: 0x0025667C
			public void SetSprite(bool isDeck, CharaDef.AttributeType type = CharaDef.AttributeType.ALL)
			{
				this.Base_CharaBlank.InitForce();
				if (isDeck)
				{
					this.Base_CharaBlank.Replace(8);
					return;
				}
				switch (type)
				{
				case CharaDef.AttributeType.RED:
					this.Base_CharaBlank.Replace(1);
					return;
				case CharaDef.AttributeType.GREEN:
					this.Base_CharaBlank.Replace(3);
					return;
				case CharaDef.AttributeType.BLUE:
					this.Base_CharaBlank.Replace(2);
					return;
				case CharaDef.AttributeType.PINK:
					this.Base_CharaBlank.Replace(4);
					return;
				case CharaDef.AttributeType.LIME:
					this.Base_CharaBlank.Replace(6);
					return;
				case CharaDef.AttributeType.AQUA:
					this.Base_CharaBlank.Replace(5);
					return;
				default:
					this.Base_CharaBlank.Replace(7);
					return;
				}
			}

			// Token: 0x060057A8 RID: 22440 RVA: 0x00258520 File Offset: 0x00256720
			public void SetSprite(bool isDeck, int index)
			{
				CharaDef.AttributeType attributeType = CharaDef.AttributeType.ALL;
				if (!isDeck)
				{
					switch (index)
					{
					case 1:
						attributeType = CharaDef.AttributeType.RED;
						break;
					case 2:
						attributeType = CharaDef.AttributeType.GREEN;
						break;
					case 3:
						attributeType = CharaDef.AttributeType.BLUE;
						break;
					case 4:
						attributeType = CharaDef.AttributeType.PINK;
						break;
					case 5:
						attributeType = CharaDef.AttributeType.LIME;
						break;
					case 6:
						attributeType = CharaDef.AttributeType.AQUA;
						break;
					}
				}
				this.SetSprite(isDeck, attributeType);
			}

			// Token: 0x060057A9 RID: 22441 RVA: 0x00258574 File Offset: 0x00256774
			public void Setup(SelCharaDeckCtrl.GUI.IconChara.SetupParam param)
			{
				this.SetSprite(param.isDeck, param.index);
				if (param.cbTouchCharaIcon != null)
				{
					this.iconCharaSet.iconCharaCtrl.AddOnClickListener(delegate(IconCharaCtrl x)
					{
						param.cbTouchCharaIcon(SelCharaDeckCtrl.FrameType.DECK, this, 0);
					});
					PrjUtil.AddTouchEventTrigger(this.baseObj.transform.Find("Base_CharaBlank").gameObject, delegate(Transform x)
					{
						param.cbTouchCharaIcon(SelCharaDeckCtrl.FrameType.DECK, this, 0);
					});
					for (int i = 0; i < this.iconPhotoCtrl.Count; i++)
					{
						int photo2 = i;
						this.iconPhotoCtrl[i].AddOnClickListener(delegate(IconPhotoCtrl x)
						{
							param.cbTouchCharaIcon(SelCharaDeckCtrl.FrameType.DECK, this, photo2);
						});
					}
					for (int j = 0; j < this.photoRect.Length; j++)
					{
						int photo = j;
						PrjUtil.AddTouchEventTrigger(this.photoRect[j].gameObject, delegate(Transform x)
						{
							param.cbTouchCharaIcon(SelCharaDeckCtrl.FrameType.DECK, this, photo);
						});
					}
					this.iconAccessoryCtrl.AddOnClickListener(delegate(IconAccessoryCtrl x)
					{
						param.cbTouchCharaIcon(SelCharaDeckCtrl.FrameType.DECK, this, 0);
					});
					PrjUtil.AddTouchEventTrigger(this.accessoryRect.gameObject, delegate(Transform x)
					{
						param.cbTouchCharaIcon(SelCharaDeckCtrl.FrameType.DECK, this, 0);
					});
				}
				UnityAction<Transform> <>9__6;
				for (int k = 0; k < this.iconPhotoCtrl.Count; k++)
				{
					GameObject gameObject = this.iconPhotoKind[k].gameObject;
					UnityAction<Transform> unityAction;
					if ((unityAction = <>9__6) == null)
					{
						unityAction = (<>9__6 = delegate(Transform x)
						{
							SoundManager.Play("prd_se_click", false, false);
							param.cbClickIconPhotoKind();
						});
					}
					PrjUtil.AddTouchEventTrigger(gameObject, unityAction);
				}
			}

			// Token: 0x040062E1 RID: 25313
			public const int SCROLL_ITEM_NUN_H = 2;

			// Token: 0x040062E2 RID: 25314
			public GameObject baseObj;

			// Token: 0x040062E3 RID: 25315
			public SimpleAnimation anime;

			// Token: 0x040062E4 RID: 25316
			public SelCharaDeckCtrl.GUI.IconCharaSet iconCharaSet;

			// Token: 0x040062E5 RID: 25317
			public List<IconPhotoCtrl> iconPhotoCtrl;

			// Token: 0x040062E6 RID: 25318
			public List<PguiReplaceSpriteCtrl> iconPhotoKind;

			// Token: 0x040062E7 RID: 25319
			public List<PguiReplaceSpriteCtrl> iconBlankFrame;

			// Token: 0x040062E8 RID: 25320
			public List<PguiReplaceSpriteCtrl> iconStatusKind;

			// Token: 0x040062E9 RID: 25321
			public RectTransform[] photoRect;

			// Token: 0x040062EA RID: 25322
			public GameObject PhotoIconView;

			// Token: 0x040062EB RID: 25323
			public GameObject PhotoIconKind;

			// Token: 0x040062EC RID: 25324
			public GameObject Mark_Friend;

			// Token: 0x040062ED RID: 25325
			public PguiReplaceSpriteCtrl Base_CharaBlank;

			// Token: 0x040062EE RID: 25326
			public GameObject Base_CharaBlank_Friend;

			// Token: 0x040062EF RID: 25327
			public GameObject banObj;

			// Token: 0x040062F0 RID: 25328
			public GameObject AccessoryIconView;

			// Token: 0x040062F1 RID: 25329
			public RectTransform accessoryRect;

			// Token: 0x040062F2 RID: 25330
			public IconAccessoryCtrl iconAccessoryCtrl;

			// Token: 0x040062F3 RID: 25331
			public PguiReplaceSpriteCtrl iconAccessoryBlankFrame;

			// Token: 0x02001248 RID: 4680
			public class SetupParam
			{
				// Token: 0x04006433 RID: 25651
				public SelCharaDeckCtrl.GUI.IconChara.SetupParam.OnTouchCharaIcon cbTouchCharaIcon;

				// Token: 0x04006434 RID: 25652
				public SelCharaDeckCtrl.GUI.IconChara.SetupParam.OnClickIconPhotoKind cbClickIconPhotoKind;

				// Token: 0x04006435 RID: 25653
				public bool isDeck;

				// Token: 0x04006436 RID: 25654
				public int index;

				// Token: 0x0200125D RID: 4701
				// (Invoke) Token: 0x0600589A RID: 22682
				public delegate bool OnTouchCharaIcon(SelCharaDeckCtrl.FrameType type, SelCharaDeckCtrl.GUI.IconChara icon, int photo);

				// Token: 0x0200125E RID: 4702
				// (Invoke) Token: 0x0600589E RID: 22686
				public delegate bool OnClickIconPhotoKind();
			}
		}

		// Token: 0x02001210 RID: 4624
		public class IconCharaPack
		{
			// Token: 0x040062F4 RID: 25332
			public SelCharaDeckCtrl.GUI.IconChara iconChara;

			// Token: 0x040062F5 RID: 25333
			public GameObject helperBase;
		}

		// Token: 0x02001211 RID: 4625
		public class CharaDeck
		{
			// Token: 0x060057AB RID: 22443 RVA: 0x00258728 File Offset: 0x00256928
			public CharaDeck(Transform baseTr, int num = 5)
			{
				this.baseObj = baseTr.gameObject;
				Transform transform = baseTr.Find("Btn_ToBattle");
				if (transform)
				{
					this.Btn_ToBattle = transform.GetComponent<PguiButtonCtrl>();
					PguiButtonCtrl component = transform.Find("Btn_Question").GetComponent<PguiButtonCtrl>();
					this.questRuleInfo = new QuestUtil.QuestRuleInfo(component);
					this.questSealedInfo = transform.Find("Btn_SealedInfo").GetComponent<PguiButtonCtrl>();
					this.Anim_ToBattle = transform.Find("BaseImage/Txt").GetComponent<SimpleAnimation>();
				}
				Transform transform2 = baseTr.Find("Btn_QuestSkip");
				if (transform2)
				{
					this.Btn_QuestSkip = transform2.GetComponent<PguiButtonCtrl>();
					this.Popup_QuestSkip = this.Btn_QuestSkip.transform.Find("PopupParent/Popup_Campaign_CmnRed").gameObject;
				}
				Transform transform3 = baseTr.Find("Btn_EditOk");
				if (transform3)
				{
					this.Btn_EditOk = transform3.GetComponent<PguiButtonCtrl>();
				}
				Transform transform4 = baseTr.Find("DeckWindow/Btn_SkillChange");
				if (transform4)
				{
					this.Btn_SkillChange = transform4.GetComponent<PguiButtonCtrl>();
				}
				Transform transform5 = baseTr.Find("DeckWindow/Btn_TacticsChange");
				if (transform5)
				{
					this.Btn_TacticsChange = transform5.GetComponent<PguiButtonCtrl>();
				}
				Transform transform6 = baseTr.Find("CharaAll/TopBtns/Btn_FilterOnOff");
				if (transform6)
				{
					this.Btn_FilterOnOff = transform6.GetComponent<PguiButtonCtrl>();
				}
				Transform transform7 = baseTr.Find("CharaAll/TopBtns/Btn_Sort");
				if (transform7)
				{
					this.Btn_Sort = transform7.GetComponent<PguiButtonCtrl>();
				}
				Transform transform8 = baseTr.Find("CharaAll/TopBtns/Btn_SortUpDown");
				if (transform8)
				{
					this.Btn_SortUpDown = transform8.GetComponent<PguiButtonCtrl>();
				}
				Transform transform9 = baseTr.Find("DeckWindow/ParamInfo");
				if (transform9)
				{
					this.Txt_All = transform9.Find("All/Num").GetComponent<PguiTextCtrl>();
					this.Txt_HP = transform9.Find("HP/Num").GetComponent<PguiTextCtrl>();
					this.Txt_ATK = transform9.Find("ATK/Num").GetComponent<PguiTextCtrl>();
					this.Txt_DEF = transform9.Find("DEF/Num").GetComponent<PguiTextCtrl>();
				}
				Transform transform10 = baseTr.Find("DeckWindow/SkillInfo");
				if (transform10)
				{
					this.Txt_SkillTypeName = transform10.Find("Txt").GetComponent<PguiTextCtrl>();
					this.Txt_SkillName = transform10.Find("Txt_SkillName").GetComponent<PguiTextCtrl>();
					this.Txt_SkillLevel = transform10.Find("Num_Lv").GetComponent<PguiTextCtrl>();
				}
				Transform transform11 = baseTr.Find("DeckWindow/PlayerIcon");
				if (transform11)
				{
					this.Icon_Skill = transform11.Find("Icon_Skill").GetComponent<PguiImageCtrl>();
					this.PlayerIcon = transform11.GetComponent<PguiReplaceSpriteCtrl>();
				}
				Transform transform12 = baseTr.Find("DeckWindow/AEImage_Mark_Ban");
				this.PlayerMarkBan = ((transform12 == null) ? null : transform12.gameObject);
				this.DeckWindowImage = baseTr.Find("DeckWindow").GetComponent<PguiImageCtrl>();
				Transform transform13 = baseTr.Find("EnemyInfo");
				if (transform13)
				{
					this.EnemyInfo = transform13.gameObject;
					this.enemyInfoList = new List<SelBattleHelperCtrl.GUI.EnemyInfo>
					{
						new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("EnemyInfo/AtrInfo01/Icon_Atr_R")),
						new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("EnemyInfo/AtrInfo01/Icon_Atr_G")),
						new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("EnemyInfo/AtrInfo01/Icon_Atr_B")),
						new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("EnemyInfo/AtrInfo02/Icon_Atr_R")),
						new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("EnemyInfo/AtrInfo02/Icon_Atr_G")),
						new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("EnemyInfo/AtrInfo02/Icon_Atr_B"))
					};
					this.Btn_EnemyInfo = transform13.Find("Btn_Info").GetComponent<PguiButtonCtrl>();
				}
				Transform transform14 = baseTr.Find("Campaign");
				if (transform14)
				{
					this.campaignInfo = new QuestUtil.CampaignInfo(transform14);
				}
				this.DeckSelect = baseTr.GetComponent<SimpleAnimation>();
				this.iconCharaPacks = new List<SelCharaDeckCtrl.GUI.IconCharaPack>();
				for (int i = 0; i < num; i++)
				{
					string text = "DeckWindow/Icon_Chara" + (i + 1).ToString("D2");
					GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaDeck_CharaIconSet"), baseTr.Find(text));
					this.iconCharaPacks.Add(new SelCharaDeckCtrl.GUI.IconCharaPack
					{
						iconChara = new SelCharaDeckCtrl.GUI.IconChara(gameObject.transform, null, -1),
						helperBase = ((baseTr.Find(text + "/Base") != null) ? baseTr.Find(text + "/Base").gameObject : null)
					});
				}
				Transform transform15 = baseTr.Find("CharaAll/ScrollView");
				if (transform15)
				{
					this.ScrollView = transform15.GetComponent<ReuseScroll>();
				}
				Transform transform16 = baseTr.Find("DeckWindow/TabGroup");
				if (transform16)
				{
					this.DeckTab = transform16.GetComponent<PguiTabGroupCtrl>();
				}
				this.Btn_Chara = baseTr.Find("RadioBtns/Btn_Left").GetComponent<PguiToggleButtonCtrl>();
				this.Btn_Photo = baseTr.Find("RadioBtns/Btn_Right").GetComponent<PguiToggleButtonCtrl>();
				this.Btn_Accessory = baseTr.Find("RadioBtns/Btn_Right02").GetComponent<PguiToggleButtonCtrl>();
				Transform transform17 = baseTr.Find("DeckWindow/Btn_AutoDeck");
				if (transform17)
				{
					this.Btn_AutoDeck = transform17.GetComponent<PguiButtonCtrl>();
				}
				Transform transform18 = baseTr.Find("DeckWindow/StaySkillSetting");
				if (transform18)
				{
					this.StaySkillSettingAnim = transform18.GetComponent<SimpleAnimation>();
					this.Btn_StaySkills = new List<PguiToggleButtonCtrl>
					{
						transform18.Find("Btn_StaySkill01").GetComponent<PguiToggleButtonCtrl>(),
						transform18.Find("Btn_StaySkill02").GetComponent<PguiToggleButtonCtrl>(),
						transform18.Find("Btn_StaySkill03").GetComponent<PguiToggleButtonCtrl>(),
						transform18.Find("Btn_StaySkill04").GetComponent<PguiToggleButtonCtrl>(),
						transform18.Find("Btn_StaySkill05").GetComponent<PguiToggleButtonCtrl>()
					};
				}
				Transform transform19 = baseTr.Find("DeckWindow/StaySkillInfo");
				if (transform19)
				{
					this.StaySkillInfo = transform19.gameObject;
					this.Txt_StaySkill_Switches = new List<PguiTextCtrl>
					{
						transform19.Find("Txt_StaySkill_Switch01").GetComponent<PguiTextCtrl>(),
						transform19.Find("Txt_StaySkill_Switch02").GetComponent<PguiTextCtrl>(),
						transform19.Find("Txt_StaySkill_Switch03").GetComponent<PguiTextCtrl>(),
						transform19.Find("Txt_StaySkill_Switch04").GetComponent<PguiTextCtrl>(),
						transform19.Find("Txt_StaySkill_Switch05").GetComponent<PguiTextCtrl>()
					};
				}
				Transform transform20 = baseTr.Find("DeckWindow/Btn_StaySkillSetting");
				if (transform20)
				{
					this.Btn_StaySkillSetting = transform20.GetComponent<PguiButtonCtrl>();
				}
				Transform transform21 = baseTr.Find("DeckWindow/Btn_PartyName");
				if (transform21)
				{
					this.Btn_PartyName = transform21.GetComponent<PguiButtonCtrl>();
				}
				Transform transform22 = baseTr.Find("DeckWindow/PartyName");
				if (transform22)
				{
					this.PartyName = transform22.GetComponent<PguiReplaceSpriteCtrl>();
				}
				Transform transform23 = baseTr.Find("Btn_PhotoCansel/Btn_Base");
				if (transform23)
				{
					this.Btn_PhotoRemove = transform23.GetComponent<PguiButtonCtrl>();
				}
				Transform transform24 = baseTr.Find("DeckWindow/Info_PhotoEffect");
				if (transform24 != null)
				{
					this.Info_PhotoEffect = transform24.GetComponent<InfoPhotoItemEffectCtrl>();
				}
				Transform transform25 = baseTr.Find("DeckWindow/Btn_OrderCardInfo");
				this.Btn_OrderCardInfo = ((transform25 != null) ? transform25.GetComponent<PguiButtonCtrl>() : null);
				Transform transform26 = baseTr.Find("DeckWindow/Btn_OrderCardInfo/Detail/Deck_Red/Num");
				this.Txt_Deck_Red = ((transform26 != null) ? transform26.GetComponent<PguiTextCtrl>() : null);
				Transform transform27 = baseTr.Find("DeckWindow/Btn_OrderCardInfo/Detail/Deck_Blue/Num");
				this.Txt_Deck_Blue = ((transform27 != null) ? transform27.GetComponent<PguiTextCtrl>() : null);
				Transform transform28 = baseTr.Find("DeckWindow/Btn_OrderCardInfo/Detail/Deck_Green/Num");
				this.Txt_Deck_Green = ((transform28 != null) ? transform28.GetComponent<PguiTextCtrl>() : null);
				Transform transform29 = baseTr.Find("DeckWindow/Btn_OrderCardInfo/Detail/Deck_Special/Num");
				this.Txt_Deck_Special = ((transform29 != null) ? transform29.GetComponent<PguiTextCtrl>() : null);
				Transform transform30 = baseTr.Find("DeckWindow/Num_TotalPlasm");
				this.Txt_TotalPlasm = ((transform30 != null) ? transform30.GetComponent<PguiTextCtrl>() : null);
				Transform transform31 = baseTr.Find("DeckWindow/TacticsSkill");
				if (transform31)
				{
					this.iconTacticsSkillBase = transform31.gameObject;
					this.iconTacticsSkill = transform31.Find("Icon").GetComponent<PguiReplaceSpriteCtrl>();
					this.iconTacticsSkillChangeMark = transform31.Find("MarkChange").gameObject;
				}
				Transform transform32 = baseTr.Find("CharaAll/Txt_None");
				if (transform32)
				{
					this.Txt_None = transform32.gameObject;
					this.Txt_None.SetActive(false);
				}
			}

			// Token: 0x060057AC RID: 22444 RVA: 0x00258F94 File Offset: 0x00257194
			public void ResizeScrollView(int count, int resize)
			{
				if (this.Txt_None == null)
				{
					return;
				}
				this.Txt_None.SetActive(count <= 0);
				this.ScrollView.ResizeFocesNoMove(resize);
			}

			// Token: 0x060057AD RID: 22445 RVA: 0x00258FC4 File Offset: 0x002571C4
			public void SwitchHelperIcon(int index, bool isEnemyInfo = false)
			{
				for (int i = 0; i < this.iconCharaPacks.Count; i++)
				{
					SelCharaDeckCtrl.GUI.IconChara iconChara = this.iconCharaPacks[i].iconChara;
					if (isEnemyInfo)
					{
						iconChara.Mark_Friend.SetActive(i == index);
						iconChara.Base_CharaBlank_Friend.SetActive(false);
					}
					else
					{
						iconChara.Mark_Friend.SetActive(false);
						iconChara.Base_CharaBlank_Friend.SetActive(i == index);
					}
					GameObject helperBase = this.iconCharaPacks[i].helperBase;
					if (helperBase != null)
					{
						helperBase.SetActive(false);
					}
				}
			}

			// Token: 0x060057AE RID: 22446 RVA: 0x00259054 File Offset: 0x00257254
			public void UpdateCampaignInfoCategory(QuestStaticChapter.Category category, int chapterId)
			{
				List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(category, chapterId));
				List<string> list2 = new List<string>(QuestUtil.GetCampaignTimeList(category, chapterId));
				this.campaignInfo.DispCampaign(list, list2);
			}

			// Token: 0x060057AF RID: 22447 RVA: 0x00259088 File Offset: 0x00257288
			public void ResetCampaignInfoCategory()
			{
				this.campaignInfo.ResetCampaign();
			}

			// Token: 0x040062F6 RID: 25334
			public GameObject baseObj;

			// Token: 0x040062F7 RID: 25335
			public PguiButtonCtrl Btn_ToBattle;

			// Token: 0x040062F8 RID: 25336
			public SimpleAnimation Anim_ToBattle;

			// Token: 0x040062F9 RID: 25337
			public PguiButtonCtrl Btn_EditOk;

			// Token: 0x040062FA RID: 25338
			public PguiButtonCtrl Btn_QuestSkip;

			// Token: 0x040062FB RID: 25339
			public GameObject Popup_QuestSkip;

			// Token: 0x040062FC RID: 25340
			public PguiButtonCtrl Btn_SkillChange;

			// Token: 0x040062FD RID: 25341
			public PguiButtonCtrl Btn_TacticsChange;

			// Token: 0x040062FE RID: 25342
			public PguiButtonCtrl Btn_FilterOnOff;

			// Token: 0x040062FF RID: 25343
			public PguiButtonCtrl Btn_Sort;

			// Token: 0x04006300 RID: 25344
			public PguiButtonCtrl Btn_SortUpDown;

			// Token: 0x04006301 RID: 25345
			public PguiTextCtrl Txt_All;

			// Token: 0x04006302 RID: 25346
			public PguiTextCtrl Txt_HP;

			// Token: 0x04006303 RID: 25347
			public PguiTextCtrl Txt_ATK;

			// Token: 0x04006304 RID: 25348
			public PguiTextCtrl Txt_DEF;

			// Token: 0x04006305 RID: 25349
			public PguiTextCtrl Txt_SkillTypeName;

			// Token: 0x04006306 RID: 25350
			public PguiTextCtrl Txt_SkillName;

			// Token: 0x04006307 RID: 25351
			public PguiTextCtrl Txt_SkillLevel;

			// Token: 0x04006308 RID: 25352
			public PguiImageCtrl Icon_Skill;

			// Token: 0x04006309 RID: 25353
			public PguiReplaceSpriteCtrl PlayerIcon;

			// Token: 0x0400630A RID: 25354
			public GameObject PlayerMarkBan;

			// Token: 0x0400630B RID: 25355
			public GameObject EnemyInfo;

			// Token: 0x0400630C RID: 25356
			public List<SelBattleHelperCtrl.GUI.EnemyInfo> enemyInfoList = new List<SelBattleHelperCtrl.GUI.EnemyInfo>();

			// Token: 0x0400630D RID: 25357
			public PguiButtonCtrl Btn_EnemyInfo;

			// Token: 0x0400630E RID: 25358
			public QuestUtil.CampaignInfo campaignInfo;

			// Token: 0x0400630F RID: 25359
			public SimpleAnimation DeckSelect;

			// Token: 0x04006310 RID: 25360
			public List<SelCharaDeckCtrl.GUI.IconCharaPack> iconCharaPacks;

			// Token: 0x04006311 RID: 25361
			public ReuseScroll ScrollView;

			// Token: 0x04006312 RID: 25362
			public PguiTabGroupCtrl DeckTab;

			// Token: 0x04006313 RID: 25363
			public PguiToggleButtonCtrl Btn_Chara;

			// Token: 0x04006314 RID: 25364
			public PguiToggleButtonCtrl Btn_Photo;

			// Token: 0x04006315 RID: 25365
			public PguiToggleButtonCtrl Btn_Accessory;

			// Token: 0x04006316 RID: 25366
			public PguiButtonCtrl Btn_AutoDeck;

			// Token: 0x04006317 RID: 25367
			public PguiImageCtrl DeckWindowImage;

			// Token: 0x04006318 RID: 25368
			public SimpleAnimation StaySkillSettingAnim;

			// Token: 0x04006319 RID: 25369
			public List<PguiToggleButtonCtrl> Btn_StaySkills;

			// Token: 0x0400631A RID: 25370
			public GameObject StaySkillInfo;

			// Token: 0x0400631B RID: 25371
			public List<PguiTextCtrl> Txt_StaySkill_Switches;

			// Token: 0x0400631C RID: 25372
			public PguiButtonCtrl Btn_StaySkillSetting;

			// Token: 0x0400631D RID: 25373
			public PguiButtonCtrl Btn_PartyName;

			// Token: 0x0400631E RID: 25374
			public PguiReplaceSpriteCtrl PartyName;

			// Token: 0x0400631F RID: 25375
			public PguiButtonCtrl Btn_PhotoRemove;

			// Token: 0x04006320 RID: 25376
			public InfoPhotoItemEffectCtrl Info_PhotoEffect;

			// Token: 0x04006321 RID: 25377
			public PguiButtonCtrl Btn_OrderCardInfo;

			// Token: 0x04006322 RID: 25378
			public PguiTextCtrl Txt_Deck_Red;

			// Token: 0x04006323 RID: 25379
			public PguiTextCtrl Txt_Deck_Blue;

			// Token: 0x04006324 RID: 25380
			public PguiTextCtrl Txt_Deck_Green;

			// Token: 0x04006325 RID: 25381
			public PguiTextCtrl Txt_Deck_Special;

			// Token: 0x04006326 RID: 25382
			public PguiTextCtrl Txt_TotalPlasm;

			// Token: 0x04006327 RID: 25383
			public PguiReplaceSpriteCtrl iconTacticsSkill;

			// Token: 0x04006328 RID: 25384
			public GameObject iconTacticsSkillBase;

			// Token: 0x04006329 RID: 25385
			public GameObject iconTacticsSkillChangeMark;

			// Token: 0x0400632A RID: 25386
			public GameObject Txt_None;

			// Token: 0x0400632B RID: 25387
			public QuestUtil.QuestRuleInfo questRuleInfo;

			// Token: 0x0400632C RID: 25388
			public PguiButtonCtrl questSealedInfo;
		}

		// Token: 0x02001212 RID: 4626
		public class OrderCardWindow
		{
			// Token: 0x060057B0 RID: 22448 RVA: 0x00259095 File Offset: 0x00257295
			public OrderCardWindow()
			{
			}

			// Token: 0x060057B1 RID: 22449 RVA: 0x002590A8 File Offset: 0x002572A8
			public OrderCardWindow(Transform baseTr)
			{
				this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
				this.Txt_TotalR = baseTr.Find("Base/Window/OrderCard/Total_OrderCard_Red/Num").GetComponent<PguiTextCtrl>();
				this.Txt_TotalB = baseTr.Find("Base/Window/OrderCard/Total_OrderCard_Blue/Num").GetComponent<PguiTextCtrl>();
				this.Txt_TotalG = baseTr.Find("Base/Window/OrderCard/Total_OrderCard_Green/Num").GetComponent<PguiTextCtrl>();
				this.Txt_TotalS = baseTr.Find("Base/Window/OrderCard/Total_OrderCard_Special/Num").GetComponent<PguiTextCtrl>();
				for (int i = 0; i < 5; i++)
				{
					this.charaInfo.Add(new SelCharaDeckCtrl.GUI.OrderCardWindow.CharaInfo(baseTr.Find(string.Format("Base/Window/Chara0{0}", i + 1))));
				}
			}

			// Token: 0x0400632D RID: 25389
			public PguiOpenWindowCtrl owCtrl;

			// Token: 0x0400632E RID: 25390
			public PguiTextCtrl Txt_TotalR;

			// Token: 0x0400632F RID: 25391
			public PguiTextCtrl Txt_TotalB;

			// Token: 0x04006330 RID: 25392
			public PguiTextCtrl Txt_TotalG;

			// Token: 0x04006331 RID: 25393
			public PguiTextCtrl Txt_TotalS;

			// Token: 0x04006332 RID: 25394
			public List<SelCharaDeckCtrl.GUI.OrderCardWindow.CharaInfo> charaInfo = new List<SelCharaDeckCtrl.GUI.OrderCardWindow.CharaInfo>();

			// Token: 0x0200124C RID: 4684
			public class CharaInfo
			{
				// Token: 0x0600586C RID: 22636 RVA: 0x0025CF7C File Offset: 0x0025B17C
				public CharaInfo(Transform baseTr)
				{
					this.iconCharaCtrl = baseTr.Find("Icon_Chara").GetComponent<IconCharaCtrl>();
					for (int i = 0; i < 5; i++)
					{
						this.guiOrderCardList.Add(new CharaWindowCtrl.GUIOrderCard(baseTr.Find(string.Format("Grid/{0}", i)), i, false));
					}
				}

				// Token: 0x0400643E RID: 25662
				public IconCharaCtrl iconCharaCtrl;

				// Token: 0x0400643F RID: 25663
				public List<CharaWindowCtrl.GUIOrderCard> guiOrderCardList = new List<CharaWindowCtrl.GUIOrderCard>();
			}
		}

		// Token: 0x02001213 RID: 4627
		public class DeckReachedLimitListWindow
		{
			// Token: 0x060057B2 RID: 22450 RVA: 0x0025915E File Offset: 0x0025735E
			public DeckReachedLimitListWindow(Transform baseTr)
			{
				this.listWindow = baseTr.GetComponent<PguiOpenWindowCtrl>();
				this.iconCharaListBase = baseTr.Find("Base/Window/IconCharaList").gameObject;
			}

			// Token: 0x060057B3 RID: 22451 RVA: 0x00259188 File Offset: 0x00257388
			public void SetupCharaIcons(List<CharaPackData> charaList)
			{
				for (int i = 0; i < charaList.Count; i++)
				{
					IconCharaCtrl component = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara).GetComponent<IconCharaCtrl>();
					component.transform.SetParent(this.iconCharaListBase.transform, false);
					component.Setup(charaList[i], SortFilterDefine.SortType.KIZUNA, false, null, 0, -1, 0);
					component.transform.localScale = new Vector3(1f, 1f, 1f);
				}
			}

			// Token: 0x060057B4 RID: 22452 RVA: 0x00259204 File Offset: 0x00257404
			public void DestroyCharaIcons()
			{
				foreach (GameObject gameObject in this.iconCharaListBase.GetChildList())
				{
					Object.Destroy(gameObject);
				}
			}

			// Token: 0x04006333 RID: 25395
			public PguiOpenWindowCtrl listWindow;

			// Token: 0x04006334 RID: 25396
			public GameObject iconCharaListBase;
		}
	}

	// Token: 0x02000F7E RID: 3966
	// (Invoke) Token: 0x06004FCF RID: 20431
	public delegate void RequestCallBack();
}
