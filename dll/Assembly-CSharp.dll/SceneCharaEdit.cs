using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

public class SceneCharaEdit : BaseScene
{
	public static bool IsStoryPhoto(ItemStaticBase isb)
	{
		return isb != null && 7001 <= isb.GetId() && isb.GetId() <= 8000;
	}

	public SceneCharaEdit.Mode getCurrentMode()
	{
		return this.currentMode;
	}

	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		RectTransform rectTransform = this.basePanel.AddComponent<RectTransform>();
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.offsetMin = new Vector2(0f, 0f);
		rectTransform.offsetMax = new Vector2(0f, 0f);
		this.basePanel.name = "SceneCharaEdit";
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Top"), this.basePanel.transform);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		GameObject gameObject2 = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window");
		this.guiData = new SceneCharaEdit.GUI(gameObject.transform);
		this.guiData.Btn_CharaDeck.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_CharaGrow.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_PhotoGrow.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_CharaAll.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_PhotoSell.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_HelperChange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_KemoBoard.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Training.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_MasterSkillGrow.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Accessory.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.baseObj.gameObject.SetActive(false);
		GameObject gameObject3 = new GameObject();
		RectTransform rectTransform2 = gameObject3.AddComponent<RectTransform>();
		rectTransform2.anchorMin = new Vector2(0f, 0f);
		rectTransform2.anchorMax = new Vector2(1f, 1f);
		rectTransform2.offsetMin = new Vector2(0f, 0f);
		rectTransform2.offsetMax = new Vector2(0f, 0f);
		gameObject3.name = "SelCharaGrow";
		gameObject3.transform.SetParent(this.basePanel.transform, false);
		this.selCharaGrowCtrl = gameObject3.AddComponent<SelCharaGrowCtrl>();
		this.selCharaGrowCtrl.Init();
		this.selCharaGrowCtrl.gameObject.SetActive(false);
		GameObject gameObject4 = new GameObject();
		gameObject4.AddComponent<RectTransform>();
		gameObject4.name = "SelPhotoGrow";
		gameObject4.transform.SetParent(this.basePanel.transform, false);
		this.selPhotoGrowCtrl = gameObject4.AddComponent<SelPhotoGrowCtrl>();
		this.selPhotoGrowCtrl.Init();
		this.selPhotoGrowCtrl.gameObject.SetActive(false);
		GameObject gameObject5 = new GameObject();
		gameObject5.AddComponent<RectTransform>();
		gameObject5.name = "SelCharaPhotoAll";
		gameObject5.transform.SetParent(this.basePanel.transform, false);
		this.selCharaPhotoAllCtrl = gameObject5.AddComponent<SelCharaPhotoAllCtrl>();
		this.selCharaPhotoAllCtrl.Init();
		this.selCharaPhotoAllCtrl.PhotoAlbumButtonCallBack = new UnityAction(this.MovePhotoAlubum);
		this.selCharaPhotoAllCtrl.gameObject.SetActive(false);
		GameObject gameObject6 = new GameObject();
		gameObject6.AddComponent<RectTransform>();
		gameObject6.name = "SelPhotoSellCtrl";
		gameObject6.transform.SetParent(this.basePanel.transform, false);
		this.selPhotoSellCtrl = gameObject6.AddComponent<SelPhotoSellCtrl>();
		this.selPhotoSellCtrl.Init();
		this.selPhotoSellCtrl.gameObject.SetActive(false);
		GameObject gameObject7 = new GameObject();
		gameObject7.AddComponent<RectTransform>();
		gameObject7.name = "SelMasterSkillCtrl";
		gameObject7.transform.SetParent(this.basePanel.transform, false);
		this.selMasterSkillCtrl = gameObject7.AddComponent<SelMasterSkillCtrl>();
		this.selMasterSkillCtrl.Initialize();
		this.selMasterSkillCtrl.gameObject.SetActive(false);
		GameObject gameObject8 = new GameObject();
		gameObject8.AddComponent<RectTransform>();
		gameObject8.name = "SelAccessoryGrow";
		gameObject8.transform.SetParent(this.basePanel.transform, false);
		this.selAccessoryGrowCtrl = gameObject8.AddComponent<SelAccessoryGrowCtrl>();
		this.selAccessoryGrowCtrl.Init();
		this.selAccessoryGrowCtrl.gameObject.SetActive(false);
		GameObject gameObject9 = new GameObject();
		gameObject9.AddComponent<RectTransform>();
		gameObject9.name = "SelAccessorySellCtrl";
		gameObject9.transform.SetParent(this.basePanel.transform, false);
		this.selAccessorySellCtrl = gameObject9.AddComponent<SelAccessorySellCtrl>();
		this.selAccessorySellCtrl.Initialize();
		this.selAccessorySellCtrl.gameObject.SetActive(false);
	}

	public override void OnEnableScene(object args)
	{
		this.basePanel.gameObject.SetActive(true);
		this.renderTextureChara = AssetManager.InstantiateAssetData("RenderTextureChara/Prefab/RenderTextureCharaCtrl", this.guiData.baseObj.transform).GetComponent<RenderTextureChara>();
		this.renderTextureChara.postion = new Vector2(-440f, -124f);
		this.renderTextureChara.fieldOfView = 17f;
		this.renderTextureChara.SetupEnableTouch(22, 0, false, 0, false);
		this.renderTextureChara.gameObject.SetActive(false);
		CanvasManager.SetBgObj("PanelBg_CharaEdit");
		CanvasManager.HdlCmnMenu.SetupMenu(true, "フレンズ", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		this.requestNextScene = SceneManager.SceneName.None;
		this.requestArgs = null;
		this.requestMode = SceneCharaEdit.Mode.TOP;
		this.currentMode = SceneCharaEdit.Mode.INVALID;
		SoundManager.PlayBGM("prd_bgm0003");
		this.sceneArgs = args as SceneCharaEdit.Args;
		if (this.sceneArgs != null)
		{
			if (this.sceneArgs.tutorialSequence == TutorialUtil.Sequence.PARTY_EDIT)
			{
				Singleton<SceneManager>.Instance.StartCoroutine(this.TutorialPartyEdit());
			}
			else if (this.sceneArgs.tutorialSequence == TutorialUtil.Sequence.CHARA_GROW)
			{
				this.selCharaGrowCtrl.SetIsTutorial(true);
				Singleton<SceneManager>.Instance.StartCoroutine(this.TutorialCharaGrow());
			}
			else if (this.sceneArgs.requestMode != SceneCharaEdit.Mode.INVALID)
			{
				this.currentMode = SceneCharaEdit.Mode.TOP;
				this.requestMode = this.sceneArgs.requestMode;
				this.requestDeckCategory = UserDeckData.Category.NORMAL;
				CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
				this.Update();
			}
			else if (this.sceneArgs.growCharaId != 0)
			{
				this.currentMode = SceneCharaEdit.Mode.TOP;
				this.requestMode = SceneCharaEdit.Mode.CHARA_GROW;
				CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
				this.Update();
			}
			else if (this.sceneArgs.detailCharaId != 0)
			{
				this.currentMode = SceneCharaEdit.Mode.TOP;
				this.requestMode = this.sceneArgs.menuBackRequestMode;
				CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
				this.Update();
			}
			else if (this.sceneArgs.growPhotoId != 0L)
			{
				this.currentMode = SceneCharaEdit.Mode.TOP;
				this.requestMode = SceneCharaEdit.Mode.PHOTO_GROW;
				CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
				this.Update();
			}
			else if (this.sceneArgs.detailPhotoId != 0L)
			{
				this.currentMode = SceneCharaEdit.Mode.TOP;
				this.requestMode = SceneCharaEdit.Mode.PHOTO_GROW;
				CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
				this.Update();
			}
			else if (this.sceneArgs.growAccessoryId != 0L)
			{
				this.currentMode = SceneCharaEdit.Mode.TOP;
				this.requestMode = SceneCharaEdit.Mode.ACCESSORY_GROW;
				CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
				this.Update();
			}
			else if (this.sceneArgs.detailAccessoryId != 0L && this.sceneArgs.openDetailWindow)
			{
				CanvasManager.HdlAccessoryWindowCtrl.OpenPrev();
			}
		}
		else
		{
			this.selCharaGrowCtrl.SetIsTutorial(false);
		}
		this.guiData.baseObj.SetActive(false);
		this.isTapReturnButton = false;
		this.IEnumOnEnableSceneTask = this.OnEnableSceneTask();
	}

	private IEnumerator OnEnableSceneTask()
	{
		DataManager.DmTraining.RequestGetTrainingInfo();
		if (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (DataManager.DmChara.CharaMissionUpdateRequired)
		{
			DataManager.DmChMission.RequestGetMissionList();
		}
		if (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (DataManager.DmChara.ShopUpdateRequired)
		{
			DataManager.DmShop.RequestGetShopList();
		}
		if (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	public override bool OnEnableSceneWait()
	{
		if (this.IEnumOnEnableSceneTask != null)
		{
			if (!this.IEnumOnEnableSceneTask.MoveNext())
			{
				this.IEnumOnEnableSceneTask = null;
			}
			return false;
		}
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.TRAINING));
		string text;
		if (questOnePackData != null)
		{
			string mainStoryName = SceneQuest.GetMainStoryName(questOnePackData.questChapter.category, true);
			text = mainStoryName + ((mainStoryName != "") ? "\n" : "") + questOnePackData.questChapter.chapterName + questOnePackData.questGroup.titleName + PrjUtil.MakeMessage("クリア");
		}
		else
		{
			text = "クエスト情報がありません";
		}
		DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
		MarkLockCtrl markLockTraining = this.guiData.markLockTraining;
		MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();
		setupParam.updateConditionCallback = () => QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.TRAINING);
		setupParam.releaseFlag = userFlagData.ReleaseModeFlag.TrainingByFriendsGrow == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
		setupParam.tagetObject = this.guiData.Btn_Training.gameObject;
		setupParam.text = text;
		setupParam.updateUserFlagDataCallback = delegate
		{
			userFlagData.ReleaseModeFlag.TrainingByFriendsGrow = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		};
		markLockTraining.Setup(setupParam, true);
		return true;
	}

	public override void OnStartSceneFade()
	{
		if (this.renderTextureChara != null)
		{
			this.renderTextureChara.gameObject.SetActive(true);
		}
	}

	public override void OnStartControl()
	{
	}

	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.currentMode == SceneCharaEdit.Mode.TOP)
		{
			if (button == this.guiData.Btn_CharaAll)
			{
				this.requestMode = SceneCharaEdit.Mode.CHARA_PHOTO_ALL;
				return;
			}
			if (button == this.guiData.Btn_CharaDeck)
			{
				this.requestMode = SceneCharaEdit.Mode.DECK;
				this.requestDeckCategory = UserDeckData.Category.NORMAL;
				return;
			}
			if (button == this.guiData.Btn_CharaGrow)
			{
				this.requestMode = SceneCharaEdit.Mode.CHARA_GROW;
				return;
			}
			if (button == this.guiData.Btn_PhotoGrow)
			{
				this.requestMode = SceneCharaEdit.Mode.PHOTO_GROW;
				return;
			}
			if (button == this.guiData.Btn_PhotoSell)
			{
				this.requestMode = SceneCharaEdit.Mode.PHOTO_SELL;
				return;
			}
			if (button == this.guiData.Btn_MasterSkillGrow)
			{
				this.requestMode = SceneCharaEdit.Mode.MASTER_SKILL_GROW;
				return;
			}
			if (button == this.guiData.Btn_HelperChange)
			{
				this.requestNextScene = SceneManager.SceneName.SceneProfile;
				this.requestArgs = new SceneProfile.Args
				{
					isHelperSettingStartFromCharaEdit = true
				};
				return;
			}
			if (button == this.guiData.Btn_KemoBoard)
			{
				this.requestNextScene = SceneManager.SceneName.SceneKemoBoard;
				return;
			}
			if (button == this.guiData.Btn_Training)
			{
				if (!this.guiData.markLockTraining.IsActive())
				{
					this.requestMode = SceneCharaEdit.Mode.DECK;
					this.requestDeckCategory = UserDeckData.Category.TRAINING;
					return;
				}
				QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.TRAINING));
				if (questOnePackData != null)
				{
					CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
					{
						new CmnReleaseConditionWindowCtrl.SetupParam
						{
							text = string.Concat(new string[]
							{
								SceneQuest.GetMainStoryName(questOnePackData.questChapter.category, false),
								" ",
								questOnePackData.questChapter.chapterName,
								questOnePackData.questGroup.titleName,
								PrjUtil.MakeMessage("クリア")
							}),
							enableClear = false
						}
					});
					return;
				}
			}
			else if (button == this.guiData.Btn_Accessory)
			{
				this.selAccessoryGrowCtrl.SetActive(true);
				AccessoryUtil.OpenTutorialWindow();
				this.requestMode = SceneCharaEdit.Mode.ACCESSORY_GROW;
			}
		}
	}

	private void MovePhotoAlubum()
	{
		this.requestNextScene = SceneManager.SceneName.ScenePhotoAlbum;
		this.requestArgs = new ScenePhotoAlbum.OpenParam
		{
			resultNextSceneName = SceneManager.SceneName.SceneCharaEdit,
			resultNextSceneArgs = new SceneCharaEdit.Args
			{
				requestMode = SceneCharaEdit.Mode.CHARA_PHOTO_ALL
			}
		};
	}

	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.currentMode == SceneCharaEdit.Mode.DECK && CanvasManager.HdlSelCharaDeck.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	private void OnClickButtonMenuRetrun()
	{
		if (this.currentMode == SceneCharaEdit.Mode.DECK)
		{
			CanvasManager.HdlSelCharaDeck.OnClickMenuReturn(delegate
			{
				this.requestMode = SceneCharaEdit.Mode.TOP;
			});
		}
		else if (this.currentMode == SceneCharaEdit.Mode.CHARA_GROW)
		{
			if (this.sceneArgs != null && this.sceneArgs.menuBackSceneName != SceneManager.SceneName.None)
			{
				this.requestNextScene = this.sceneArgs.menuBackSceneName;
			}
			else if (!this.selCharaGrowCtrl.OnClickMenuReturn(delegate
			{
				this.requestMode = SceneCharaEdit.Mode.TOP;
			}))
			{
			}
		}
		else if (this.currentMode == SceneCharaEdit.Mode.PHOTO_GROW)
		{
			if (this.sceneArgs != null && this.sceneArgs.menuBackSceneName != SceneManager.SceneName.None && this.selPhotoGrowCtrl.CurrentMode == this.sceneArgs.requestSubMode)
			{
				this.requestNextScene = this.sceneArgs.menuBackSceneName;
			}
			else
			{
				this.selPhotoGrowCtrl.OnClickMenuReturn(delegate
				{
					this.requestMode = SceneCharaEdit.Mode.TOP;
				});
			}
		}
		else if (this.currentMode == SceneCharaEdit.Mode.CHARA_PHOTO_ALL)
		{
			this.selCharaPhotoAllCtrl.OnClickMenuReturn(delegate
			{
				this.requestMode = SceneCharaEdit.Mode.TOP;
			});
		}
		else if (this.currentMode == SceneCharaEdit.Mode.PHOTO_SELL)
		{
			if (this.sceneArgs != null && this.sceneArgs.menuBackSceneName != SceneManager.SceneName.None)
			{
				this.requestNextScene = this.sceneArgs.menuBackSceneName;
			}
			else
			{
				this.selPhotoSellCtrl.OnClickMenuReturn(delegate
				{
					this.requestMode = SceneCharaEdit.Mode.TOP;
				});
			}
		}
		else if (this.currentMode == SceneCharaEdit.Mode.MASTER_SKILL_GROW)
		{
			if (this.sceneArgs != null && this.sceneArgs.menuBackSceneName != SceneManager.SceneName.None)
			{
				this.requestNextScene = this.sceneArgs.menuBackSceneName;
			}
			else
			{
				this.selMasterSkillCtrl.OnClickMenuReturn(delegate
				{
					this.requestMode = SceneCharaEdit.Mode.TOP;
				});
			}
		}
		else if (this.currentMode == SceneCharaEdit.Mode.ACCESSORY_GROW)
		{
			if (this.sceneArgs != null && this.sceneArgs.menuBackSceneName != SceneManager.SceneName.None && this.selAccessoryGrowCtrl.CurrentMode == this.sceneArgs.requestAccessoryGrowSubMode)
			{
				this.requestNextScene = this.sceneArgs.menuBackSceneName;
			}
			else
			{
				this.selAccessoryGrowCtrl.OnClickMenuReturn(delegate
				{
					this.requestMode = SceneCharaEdit.Mode.TOP;
				});
			}
		}
		else if (this.currentMode == SceneCharaEdit.Mode.ACCESSORY_SELL)
		{
			this.selAccessorySellCtrl.OnClickMenuReturn();
		}
		else
		{
			this.requestNextScene = SceneManager.SceneName.SceneHome;
		}
		this.isTapReturnButton = true;
	}

	public override void Update()
	{
		bool flag = true;
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			if (this.sceneArgs != null && this.sceneArgs.menuBackSceneName == SceneManager.SceneName.SceneBattleSelector)
			{
				SceneBattleSelector.Args args = this.sceneArgs.menuBackSceneArgs as SceneBattleSelector.Args;
				SceneBattleSelector sceneBattleSelector = args.menuBackSceneArgs as SceneBattleSelector;
				if (sceneBattleSelector != null)
				{
					sceneBattleSelector.BsArgs.recordCameSceneName = args.recordCameSceneName;
					sceneBattleSelector.BsArgs.detailCharaId = args.detailCharaId;
					sceneBattleSelector.BsArgs.detailPhotoId = args.detailPhotoId;
					sceneBattleSelector.BsArgs.detailAccesssoryId = args.detailAccesssoryId;
				}
				Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, (sceneBattleSelector != null) ? sceneBattleSelector.BsArgs : this.sceneArgs.menuBackSceneArgs);
			}
			else
			{
				Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, (this.sceneArgs != null && this.sceneArgs.menuBackSceneName != SceneManager.SceneName.None) ? this.sceneArgs.menuBackSceneArgs : this.requestArgs);
			}
			flag = false;
		}
		if (this.isTapReturnButton)
		{
			this.isTapReturnButton = false;
			flag = false;
		}
		if (this.currentMode == SceneCharaEdit.Mode.DECK && CanvasManager.HdlSelCharaDeck.ForceReturnTop())
		{
			this.requestMode = SceneCharaEdit.Mode.TOP;
		}
		if (this.requestMode != this.currentMode)
		{
			switch (this.currentMode)
			{
			case SceneCharaEdit.Mode.TOP:
				this.renderTextureChara.gameObject.SetActive(false);
				break;
			case SceneCharaEdit.Mode.DECK:
				CanvasManager.HdlSelCharaDeck.SetActive(false, false);
				if (this.requestDeckCategory == UserDeckData.Category.TRAINING)
				{
					CanvasManager.SetBgObj("PanelBg_CharaEdit");
				}
				break;
			case SceneCharaEdit.Mode.CHARA_GROW:
				this.selCharaGrowCtrl.gameObject.SetActive(false);
				break;
			case SceneCharaEdit.Mode.PHOTO_GROW:
				this.selPhotoGrowCtrl.gameObject.SetActive(false);
				break;
			case SceneCharaEdit.Mode.CHARA_PHOTO_ALL:
				this.selCharaPhotoAllCtrl.gameObject.SetActive(false);
				break;
			case SceneCharaEdit.Mode.PHOTO_SELL:
				this.selPhotoSellCtrl.gameObject.SetActive(false);
				break;
			case SceneCharaEdit.Mode.MASTER_SKILL_GROW:
				this.selMasterSkillCtrl.gameObject.SetActive(false);
				break;
			case SceneCharaEdit.Mode.ACCESSORY_GROW:
				this.selAccessoryGrowCtrl.gameObject.SetActive(false);
				break;
			case SceneCharaEdit.Mode.ACCESSORY_SELL:
				this.selAccessorySellCtrl.gameObject.SetActive(false);
				break;
			}
			if (this.requestMode == SceneCharaEdit.Mode.TOP)
			{
				if (this.currentMode != SceneCharaEdit.Mode.INVALID)
				{
					CanvasManager.HdlCmnMenu.SetupMenu(true, "フレンズ", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
				}
				this.guiData.baseObj.SetActive(true);
				this.guiData.CharaEditTop_SE.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
				this.renderTextureChara.gameObject.SetActive(true);
				this.guiData.CampaignIcon_CharaGrow.SetActive(DataManager.DmCampaign.PresentCampaignGrowCharaData != null && 1 != DataManager.DmCampaign.PresentCampaignGrowCharaData.campaignId);
				this.guiData.CampaignIcon_PhotoGrow.SetActive(DataManager.DmCampaign.PresentCampaignGrowPhotoData != null && 1 != DataManager.DmCampaign.PresentCampaignGrowPhotoData.campaignId);
				this.guiData.CharaGrowCampaignAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
				this.guiData.PhotoGrowCampaignAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
				CanvasManager.HdlOpenWindowSortFilter.RequestActionUpdateSortType();
				SortFilterManager.RequestUpdateSortTypeData();
			}
			else if (this.requestMode == SceneCharaEdit.Mode.DECK)
			{
				string text = ((this.requestDeckCategory == UserDeckData.Category.TRAINING) ? PrjUtil.TRAINING_FORMATION : PrjUtil.PARTY_FORMATION);
				CanvasManager.HdlCmnMenu.SetupMenu(true, text, new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
				if (this.sceneArgs != null && (this.sceneArgs.growCharaId != 0 || this.sceneArgs.detailCharaId != 0 || this.sceneArgs.requestMode != SceneCharaEdit.Mode.INVALID))
				{
					this.guiData.baseObj.SetActive(false);
					CanvasManager.HdlSelCharaDeck.SetActive(true, false);
					CanvasManager.HdlSelCharaDeck.Setup(new SelCharaDeckCtrl.SetupParam
					{
						callScene = SceneManager.SceneName.SceneCharaEdit,
						attrIndex = 0,
						deckCategory = this.requestDeckCategory,
						trainingDay = TimeManager.Now.DayOfWeek
					}, -1);
					if (this.sceneArgs.openDetailWindow)
					{
						CanvasManager.HdlCharaWindowCtrl.OpenPrev();
					}
					if (this.sceneArgs.openDressWindow)
					{
						CanvasManager.HdlDressUpWindowCtrl.OpenPrev();
					}
				}
				else
				{
					this.guiData.CharaEditTop_SE.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
					{
						this.guiData.baseObj.SetActive(false);
						CanvasManager.HdlSelCharaDeck.SetActive(true, false);
						CanvasManager.HdlSelCharaDeck.Setup(new SelCharaDeckCtrl.SetupParam
						{
							callScene = SceneManager.SceneName.SceneCharaEdit,
							attrIndex = 0,
							deckCategory = this.requestDeckCategory,
							trainingDay = TimeManager.Now.DayOfWeek
						}, -1);
					});
				}
				if (this.sceneArgs != null && this.sceneArgs.tutorialSequence != TutorialUtil.Sequence.INVALID)
				{
					this.gotoNextStepByTutorial = true;
				}
				if (this.requestDeckCategory == UserDeckData.Category.TRAINING)
				{
					CanvasManager.SetBgObj("PanelBg_Training");
				}
			}
			else if (this.requestMode == SceneCharaEdit.Mode.CHARA_GROW)
			{
				CanvasManager.HdlCmnMenu.SetupMenu(true, "フレンズ成長", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
				if (this.sceneArgs != null && (this.sceneArgs.growCharaId != 0 || this.sceneArgs.requestMode != SceneCharaEdit.Mode.INVALID))
				{
					this.guiData.baseObj.SetActive(false);
					this.selCharaGrowCtrl.gameObject.SetActive(true);
					this.selCharaGrowCtrl.Setup();
					this.selCharaGrowCtrl.SetupParam(this.sceneArgs.menuBackSceneName == SceneManager.SceneName.None, this.sceneArgs.menuBackSceneName != SceneManager.SceneName.SceneBattleSelector);
					if (this.sceneArgs.openDetailWindow)
					{
						CanvasManager.HdlCharaWindowCtrl.Open(this.selCharaGrowCtrl.DispCharaPackList.Find((CharaPackData item) => item.id == this.sceneArgs.growCharaId), new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY_NO_GROW, null), null);
					}
					if (this.sceneArgs.openDressWindow)
					{
						CanvasManager.HdlDressUpWindowCtrl.Open(this.selCharaGrowCtrl.DispCharaPackList.Find((CharaPackData item) => item.id == this.sceneArgs.growCharaId), new DressUpWindowCtrl.OpenParameter(DressUpWindowCtrl.OpenParameter.Preset.MINE_EASY_NO_GROW, null));
					}
				}
				else
				{
					this.guiData.CharaEditTop_SE.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
					{
						this.guiData.baseObj.SetActive(false);
						this.selCharaGrowCtrl.gameObject.SetActive(true);
						this.selCharaGrowCtrl.Setup();
						this.selCharaGrowCtrl.SetupParam(true, true);
					});
				}
				if (this.sceneArgs != null && this.sceneArgs.tutorialSequence != TutorialUtil.Sequence.INVALID)
				{
					this.gotoNextStepByTutorial = true;
				}
				else if (this.sceneArgs != null && this.sceneArgs.growCharaId != 0)
				{
					this.selCharaGrowCtrl.SetupBySceneForce(this.sceneArgs.growCharaId, this.sceneArgs.growTab, this.sceneArgs.openItemWindow);
					this.sceneArgs.growCharaId = 0;
				}
			}
			else if (this.requestMode == SceneCharaEdit.Mode.PHOTO_GROW)
			{
				CanvasManager.HdlCmnMenu.SetupMenu(true, "フォト強化", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "強化するベースとなるフォトを選んでください", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
				if (this.sceneArgs != null && (this.sceneArgs.growPhotoId != 0L || this.sceneArgs.requestMode != SceneCharaEdit.Mode.INVALID))
				{
					this.guiData.baseObj.SetActive(false);
					this.selPhotoGrowCtrl.gameObject.SetActive(true);
					this.selPhotoGrowCtrl.Setup(new SelPhotoGrowCtrl.SetupParam
					{
						onReturnSceneNameCB = delegate
						{
							this.requestNextScene = ((this.sceneArgs != null && this.sceneArgs.menuBackSceneName != SceneManager.SceneName.None && this.selPhotoGrowCtrl.CurrentMode == this.sceneArgs.requestSubMode) ? this.sceneArgs.menuBackSceneName : SceneManager.SceneName.None);
							return this.requestNextScene;
						}
					});
					if (this.sceneArgs.openDetailWindow)
					{
						CanvasManager.HdlPhotoWindowCtrl.OpenPrev();
					}
				}
				else
				{
					this.guiData.CharaEditTop_SE.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
					{
						this.guiData.baseObj.SetActive(false);
						this.selPhotoGrowCtrl.gameObject.SetActive(true);
						this.selPhotoGrowCtrl.Setup(new SelPhotoGrowCtrl.SetupParam
						{
							onReturnSceneNameCB = delegate
							{
								this.requestNextScene = ((this.sceneArgs != null && this.sceneArgs.menuBackSceneName != SceneManager.SceneName.None && this.selPhotoGrowCtrl.CurrentMode == this.sceneArgs.requestSubMode) ? this.sceneArgs.menuBackSceneName : SceneManager.SceneName.None);
								return this.requestNextScene;
							}
						});
					});
				}
				if (this.sceneArgs != null && this.sceneArgs.growPhotoId != 0L)
				{
					this.selPhotoGrowCtrl.SetupBySceneForce(this.sceneArgs.growPhotoId);
					this.sceneArgs.growPhotoId = 0L;
				}
			}
			else if (this.requestMode == SceneCharaEdit.Mode.CHARA_PHOTO_ALL)
			{
				string text2 = "\u00a0";
				CanvasManager.HdlCmnMenu.SetupMenu(true, "フレンズ・フォト・おしゃれアクセ" + text2 + "一覧", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
				if (this.sceneArgs != null && (this.sceneArgs.growCharaId != 0 || this.sceneArgs.detailCharaId != 0 || this.sceneArgs.requestMode != SceneCharaEdit.Mode.INVALID))
				{
					this.guiData.baseObj.SetActive(false);
					this.selCharaPhotoAllCtrl.gameObject.SetActive(true);
					this.selCharaPhotoAllCtrl.Setup();
					if (this.sceneArgs.openDetailWindow)
					{
						CanvasManager.HdlCharaWindowCtrl.Open(DataManager.DmChara.GetUserCharaMap().TryGetValueEx(this.sceneArgs.detailCharaId, null), new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.MINE_DETAIL, this.selCharaPhotoAllCtrl.OriginalDispCharaPackList), null);
					}
					if (this.sceneArgs.openDressWindow)
					{
						CanvasManager.HdlDressUpWindowCtrl.Open(DataManager.DmChara.GetUserCharaMap().TryGetValueEx(this.sceneArgs.detailCharaId, null), new DressUpWindowCtrl.OpenParameter(DressUpWindowCtrl.OpenParameter.Preset.DEFAULT, this.selCharaPhotoAllCtrl.OriginalDispCharaPackList));
					}
				}
				else
				{
					this.guiData.CharaEditTop_SE.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
					{
						this.guiData.baseObj.SetActive(false);
						this.selCharaPhotoAllCtrl.gameObject.SetActive(true);
						this.selCharaPhotoAllCtrl.Setup();
					});
				}
			}
			else if (this.requestMode == SceneCharaEdit.Mode.PHOTO_SELL)
			{
				CanvasManager.HdlCmnMenu.SetupMenu(true, "フォト整理（売却）", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
				if (this.sceneArgs != null && (this.sceneArgs.growCharaId != 0 || this.sceneArgs.detailCharaId != 0 || this.sceneArgs.requestMode != SceneCharaEdit.Mode.INVALID))
				{
					this.guiData.baseObj.SetActive(false);
					this.selPhotoSellCtrl.gameObject.SetActive(true);
					this.selPhotoSellCtrl.Setup();
				}
				else
				{
					this.guiData.CharaEditTop_SE.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
					{
						this.guiData.baseObj.SetActive(false);
						this.selPhotoSellCtrl.gameObject.SetActive(true);
						this.selPhotoSellCtrl.Setup();
					});
				}
			}
			else if (this.requestMode == SceneCharaEdit.Mode.MASTER_SKILL_GROW)
			{
				CanvasManager.HdlCmnMenu.SetupMenu(true, "隊長スキル成長", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
				if (this.sceneArgs != null && this.sceneArgs.requestMode != SceneCharaEdit.Mode.INVALID)
				{
					this.guiData.baseObj.SetActive(false);
					this.selMasterSkillCtrl.gameObject.SetActive(true);
					this.selMasterSkillCtrl.Setup();
				}
				else
				{
					this.guiData.CharaEditTop_SE.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
					{
						this.guiData.baseObj.SetActive(false);
						this.selMasterSkillCtrl.gameObject.SetActive(true);
						this.selMasterSkillCtrl.Setup();
					});
				}
			}
			else if (this.requestMode == SceneCharaEdit.Mode.ACCESSORY_GROW)
			{
				CanvasManager.HdlCmnMenu.SetupMenu(true, "おしゃれアクセ強化", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
				if (this.sceneArgs != null && (this.sceneArgs.growAccessoryId != 0L || this.sceneArgs.requestMode != SceneCharaEdit.Mode.INVALID))
				{
					this.guiData.baseObj.SetActive(false);
					this.selAccessoryGrowCtrl.gameObject.SetActive(true);
					this.selAccessoryGrowCtrl.Setup(new SelAccessoryGrowCtrl.SetupParam
					{
						onReturnSceneNameCB = delegate
						{
							this.requestNextScene = ((this.sceneArgs != null && this.sceneArgs.menuBackSceneName != SceneManager.SceneName.None && this.selAccessoryGrowCtrl.CurrentMode == this.sceneArgs.requestAccessoryGrowSubMode) ? this.sceneArgs.menuBackSceneName : SceneManager.SceneName.None);
							return this.requestNextScene;
						}
					});
				}
				else
				{
					this.guiData.CharaEditTop_SE.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
					{
						this.guiData.baseObj.SetActive(false);
						this.selAccessoryGrowCtrl.gameObject.SetActive(true);
						this.selAccessoryGrowCtrl.Setup(new SelAccessoryGrowCtrl.SetupParam
						{
							onReturnSceneNameCB = delegate
							{
								this.requestNextScene = ((this.sceneArgs != null && this.sceneArgs.menuBackSceneName != SceneManager.SceneName.None && this.selAccessoryGrowCtrl.CurrentMode == this.sceneArgs.requestAccessoryGrowSubMode) ? this.sceneArgs.menuBackSceneName : SceneManager.SceneName.None);
								return this.requestNextScene;
							}
						});
					});
				}
				if (this.sceneArgs != null && this.sceneArgs.growAccessoryId != 0L)
				{
					this.selAccessoryGrowCtrl.SetupBySceneForce(this.sceneArgs.growAccessoryId);
					this.sceneArgs.growAccessoryId = 0L;
				}
			}
			else if (this.requestMode == SceneCharaEdit.Mode.ACCESSORY_SELL)
			{
				CanvasManager.HdlCmnMenu.SetupMenu(true, "おしゃれアクセ売却", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
				this.guiData.baseObj.SetActive(false);
				this.selAccessorySellCtrl.gameObject.SetActive(true);
				this.selAccessorySellCtrl.Setup();
			}
			if (this.sceneArgs != null && this.sceneArgs.detailCharaId != 0)
			{
				this.sceneArgs.detailCharaId = 0;
				CanvasManager.SetBgObj("PanelBg_CharaEdit");
			}
			this.currentMode = this.requestMode;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(flag, true);
	}

	public override void OnDisableScene()
	{
		CanvasManager.HdlSelCharaDeck.SetActive(false, true);
		if (this.renderTextureChara != null)
		{
			Object.Destroy(this.renderTextureChara.gameObject);
		}
		this.renderTextureChara = null;
		this.basePanel.gameObject.SetActive(false);
		this.selCharaGrowCtrl.SetDisable();
		this.selPhotoGrowCtrl.Dest();
		this.selPhotoGrowCtrl.gameObject.SetActive(false);
		this.selCharaPhotoAllCtrl.Dest();
		this.selCharaPhotoAllCtrl.gameObject.SetActive(false);
		this.selPhotoSellCtrl.Dest();
		this.selPhotoSellCtrl.gameObject.SetActive(false);
		this.selMasterSkillCtrl.gameObject.SetActive(false);
		if (this.selAccessoryGrowCtrl != null)
		{
			this.selAccessoryGrowCtrl.Dest();
			this.selAccessoryGrowCtrl.SetActive(false);
		}
		this.selAccessorySellCtrl.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.selCharaGrowCtrl.gameObject);
		this.selCharaGrowCtrl = null;
		Object.Destroy(this.selPhotoGrowCtrl.gameObject);
		this.selPhotoGrowCtrl = null;
		Object.Destroy(this.selCharaPhotoAllCtrl.gameObject);
		this.selCharaPhotoAllCtrl = null;
		Object.Destroy(this.selPhotoSellCtrl.gameObject);
		this.selPhotoSellCtrl = null;
		Object.Destroy(this.selMasterSkillCtrl.gameObject);
		this.selMasterSkillCtrl = null;
		if (this.selAccessoryGrowCtrl != null)
		{
			Object.Destroy(this.selAccessoryGrowCtrl.gameObject);
			this.selAccessoryGrowCtrl = null;
		}
		if (this.selAccessorySellCtrl != null)
		{
			this.selAccessorySellCtrl.Deestroy();
			Object.Destroy(this.selAccessorySellCtrl.gameObject);
			this.selAccessorySellCtrl = null;
		}
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private IEnumerator TutorialPartyEdit()
	{
		CanvasManager.HdlSelCharaDeck.isTutorial = true;
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(true);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(true);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.renderTextureChara.transform, true);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN,
			postion = new Vector2?(new Vector2(200f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_0022",
			dispInfoChara = false,
			messageList = new List<string> { "戦いの前に、まずはパーティを編成しましょう" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.OUT_QUICK,
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, this.guiData.Btn_CharaDeck.transform as RectTransform, true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(this.guiData.Btn_CharaDeck.transform as RectTransform, 1f);
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.renderTextureChara.transform, true);
		this.renderTextureChara.transform.SetParent(this.guiData.baseObj.transform, true);
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(true);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(true);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN,
			postion = new Vector2?(new Vector2(200f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_0022",
			messageList = new List<string> { "新たに来てくれたフレンズを、\nパーティに加えるのですわ", "ここをタッチですの" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, CanvasManager.HdlSelCharaDeck.GetCharaDeckRectTransform(3), true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(CanvasManager.HdlSelCharaDeck.GetCharaDeckRectTransform(3), 1f);
		while (!CanvasManager.HdlSelCharaDeck.TouchRect)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		while (!CanvasManager.HdlSelCharaDeck.AnimeFinished)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, CanvasManager.HdlSelCharaDeck.GetCharaDeckReserveRectTransform(SelCharaDeckCtrl.FrameType.RESERVE, TutorialUtil.TutorialGuestCharaId), true, CanvasManager.HdlSelCharaDeck.GetCharaDeckReserveScale(SelCharaDeckCtrl.FrameType.RESERVE, TutorialUtil.TutorialGuestCharaId), CanvasManager.HdlSelCharaDeck.GetCharaDeckReserveScale(SelCharaDeckCtrl.FrameType.RESERVE, TutorialUtil.TutorialGuestCharaId));
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(CanvasManager.HdlSelCharaDeck.GetCharaDeckReserveRectTransform(SelCharaDeckCtrl.FrameType.RESERVE, TutorialUtil.TutorialGuestCharaId), 1f);
		while (!CanvasManager.HdlSelCharaDeck.TouchRect)
		{
			yield return null;
		}
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN_QUICK,
			postion = new Vector2?(new Vector2(200f, 500f)),
			messageList = new List<string> { "よくできました！\nでは次に、フォトを身に着けますわよ" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.OUT_QUICK,
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, CanvasManager.HdlSelCharaDeck.GetCharaDeckEditOKBtnRectTransform(), true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(CanvasManager.HdlSelCharaDeck.GetCharaDeckEditOKBtnRectTransform(), 1f);
		while (!CanvasManager.HdlSelCharaDeck.TouchRect)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		while (!CanvasManager.HdlSelCharaDeck.AnimeFinished)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, CanvasManager.HdlSelCharaDeck.GetCharaDeckPhotoRadioBtnRectTransform(), true, CanvasManager.HdlSelCharaDeck.GetCharaDeckPhotoRadioBtnScale(), 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(CanvasManager.HdlSelCharaDeck.GetCharaDeckPhotoRadioBtnRectTransform(), CanvasManager.HdlSelCharaDeck.GetCharaDeckPhotoRadioBtnScale());
		while (!CanvasManager.HdlSelCharaDeck.TouchRect)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN_QUICK,
			postion = new Vector2?(new Vector2(200f, 500f)),
			messageList = new List<string> { "ピーチパンサーさんからいただいたフォトを、\nフォトポケに収めましょう" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		while (!CanvasManager.HdlSelCharaDeck.FinishedAnim())
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, CanvasManager.HdlSelCharaDeck.GetCharaDeckPhotoRectTransform(2), true, CanvasManager.HdlSelCharaDeck.GetCharaDeckPhotoScale(1), CanvasManager.HdlSelCharaDeck.GetCharaDeckPhotoScale(1));
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(CanvasManager.HdlSelCharaDeck.GetCharaDeckPhotoRectTransform(2), 1f);
		while (!CanvasManager.HdlSelCharaDeck.TouchRect)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.OUT_QUICK,
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		while (!CanvasManager.HdlSelCharaDeck.IsPhotoEditMode())
		{
			yield return null;
		}
		while (CanvasManager.HdlSelCharaDeck.IsPhotoEditPlayingAnim())
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, CanvasManager.HdlSelCharaDeck.GetPhotoDeckRectTransform(), true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(CanvasManager.HdlSelCharaDeck.GetPhotoDeckRectTransform(), 1f);
		while (!CanvasManager.HdlSelCharaDeck.IsPhotoDeckTouchRect())
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		long waitTick = TimeManager.Now.Ticks + TimeManager.Second2Tick(3L);
		while (!CanvasManager.HdlSelCharaDeck.IsPhotoEditPlayingAnim())
		{
			if (TimeManager.Now.Ticks >= waitTick)
			{
				break;
			}
			yield return null;
		}
		while (CanvasManager.HdlSelCharaDeck.IsPhotoEditPlayingAnim() && TimeManager.Now.Ticks < waitTick)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, CanvasManager.HdlSelCharaDeck.GetPhotoDeckReserveRectTransform(SelCharaDeckCtrl.FrameType.RESERVE, TutorialUtil.TutorialPhotoId), true, CanvasManager.HdlSelCharaDeck.GetPhotoDeckReserveScale(SelCharaDeckCtrl.FrameType.RESERVE, TutorialUtil.TutorialPhotoId), CanvasManager.HdlSelCharaDeck.GetPhotoDeckReserveScale(SelCharaDeckCtrl.FrameType.RESERVE, TutorialUtil.TutorialPhotoId));
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(CanvasManager.HdlSelCharaDeck.GetPhotoDeckReserveRectTransform(SelCharaDeckCtrl.FrameType.RESERVE, TutorialUtil.TutorialPhotoId), 1f);
		while (!CanvasManager.HdlSelCharaDeck.IsPhotoDeckTouchRect())
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN_QUICK,
			postion = new Vector2?(new Vector2(200f, 500f)),
			messageList = new List<string> { "ばっちりですの！\u3000これで準備万端ですわ！" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlSelCharaDeck.isTutorial = false;
		TutorialUtil.RequestNextSequence(this.sceneArgs.tutorialSequence);
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(false);
		yield return null;
		yield break;
	}

	private IEnumerator TutorialCharaGrow()
	{
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(true);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(true);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN,
			postion = new Vector2?(new Vector2(200f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_1001",
			dispInfoChara = true,
			messageList = new List<string> { "さあ、ドールさん！\n秘めた野生の力を解き放ちましょう！" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, this.guiData.Btn_CharaGrow.transform as RectTransform, true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(this.guiData.Btn_CharaGrow.transform as RectTransform, 1f);
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.OUT_QUICK,
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		yield return null;
		while (this.guiData.CharaEditTop_SE.ExIsPlaying())
		{
			if (!this.guiData.baseObj.activeSelf)
			{
				this.guiData.baseObj.SetActive(true);
			}
			yield return null;
		}
		if (this.guiData.baseObj.activeSelf)
		{
			this.guiData.baseObj.SetActive(false);
		}
		while (this.selCharaGrowCtrl.IsPlayingAnimCharaSelect())
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, this.selCharaGrowCtrl.GetCharaTopRectTransform(1), true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(this.selCharaGrowCtrl.GetCharaTopRectTransform(1), 1f);
		while (!this.selCharaGrowCtrl.TouchRect)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		while (!this.selCharaGrowCtrl._isFinishedAnimCharaTopButton)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, this.selCharaGrowCtrl.GetTabRectTransform(SelCharaGrowCtrl.TabType.WildRelease), true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(this.selCharaGrowCtrl.GetTabRectTransform(SelCharaGrowCtrl.TabType.WildRelease), 1f);
		while (!this.selCharaGrowCtrl.TouchRect)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN,
			postion = new Vector2?(new Vector2(200f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_1001",
			dispInfoChara = true,
			messageList = new List<string> { "野生解放アイテムを使うことで、\nドールさんを成長させることが出来ます", "ここを押してくださいね" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.OUT,
			postion = new Vector2?(new Vector2(200f, 500f)),
			dispInfoChara = true,
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, this.selCharaGrowCtrl.GetWildReleaseBtnRectTransform(), true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(this.selCharaGrowCtrl.GetWildReleaseBtnRectTransform(), 1f);
		while (!this.selCharaGrowCtrl.TouchRect)
		{
			yield return null;
		}
		TutorialUtil.UpdateDataManagerByCharaGrow();
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		while (!this.selCharaGrowCtrl.FinishedOpenConfirmationWindow())
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, this.selCharaGrowCtrl.GetWindowButtonRectTransform(2), true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(this.selCharaGrowCtrl.GetWindowButtonRectTransform(2), 1f);
		while (!this.selCharaGrowCtrl.TouchRect)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN,
			postion = new Vector2?(new Vector2(200f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_0001",
			dispInfoChara = true,
			messageList = new List<string> { "すごい…力がみなぎってくるみたいです！" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN,
			postion = new Vector2?(new Vector2(200f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_1001",
			dispInfoChara = true,
			messageList = new List<string> { "詳しい原理は、まだ研究中なんですが…", "フレンズさんたちは、この野生解放という現象によって、\nパワーアップすることができるんです！", "野生解放アイテムを手に入れたら、\n積極的に使ってあげてくださいね" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		TutorialUtil.RequestNextSequence(this.sceneArgs.tutorialSequence);
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(false);
		yield return null;
		yield break;
	}

	private SceneCharaEdit.Mode requestMode;

	private UserDeckData.Category requestDeckCategory;

	private SceneCharaEdit.Mode currentMode;

	private SceneCharaEdit.Mode prevMode;

	private GameObject basePanel;

	private SceneCharaEdit.GUI guiData;

	private SelCharaGrowCtrl selCharaGrowCtrl;

	private SelPhotoGrowCtrl selPhotoGrowCtrl;

	private SelCharaPhotoAllCtrl selCharaPhotoAllCtrl;

	private SelPhotoSellCtrl selPhotoSellCtrl;

	private SelMasterSkillCtrl selMasterSkillCtrl;

	private SelAccessoryGrowCtrl selAccessoryGrowCtrl;

	private SelAccessorySellCtrl selAccessorySellCtrl;

	private SceneCharaEdit.Args sceneArgs = new SceneCharaEdit.Args();

	private RenderTextureChara renderTextureChara;

	private SceneManager.SceneName requestNextScene;

	private object requestArgs;

	private bool isTapReturnButton;

	private IEnumerator IEnumOnEnableSceneTask;

	private bool gotoNextStepByTutorial;

	public delegate SceneManager.SceneName OnReturnSceneName();

	public class Args
	{
		public TutorialUtil.Sequence tutorialSequence;

		public SceneCharaEdit.Mode requestMode;

		public SelPhotoGrowCtrl.Mode requestSubMode;

		public SelAccessoryGrowCtrl.Mode requestAccessoryGrowSubMode;

		public int growCharaId;

		public int growTab;

		public bool openItemWindow;

		public bool openDressWindow;

		public int detailCharaId;

		public bool openDetailWindow;

		public bool enableLeftRightButton;

		public bool enableMoreButton;

		public long growPhotoId;

		public long detailPhotoId;

		public long growAccessoryId;

		public long detailAccessoryId;

		public SceneManager.SceneName menuBackSceneName;

		public object menuBackSceneArgs;

		public SceneCharaEdit.Mode menuBackRequestMode;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_CharaDeck = baseTr.Find("All/MenuAll/Btn_CharaDeck").GetComponent<PguiButtonCtrl>();
			this.Btn_CharaGrow = baseTr.Find("All/MenuAll/Btn_CharaGrow").GetComponent<PguiButtonCtrl>();
			Transform transform = baseTr.Find("All/MenuAll/Btn_CharaGrow/BaseImage/Campaign");
			this.CampaignIcon_CharaGrow = transform.gameObject;
			this.CharaGrowCampaignAnim = transform.Find("Popup_Campaign_Friends").GetComponent<SimpleAnimation>();
			transform.Find("Popup_Campaign_Friends/Txt_kind").GetComponent<PguiTextCtrl>().text = "フレンズ成長";
			transform.Find("Popup_Campaign_Friends/Txt_kind").gameObject.SetActive(false);
			this.Btn_PhotoGrow = baseTr.Find("All/MenuAll/Btn_PhotoGrow").GetComponent<PguiButtonCtrl>();
			Transform transform2 = baseTr.Find("All/MenuAll/Btn_PhotoGrow/BaseImage/Campaign");
			this.CampaignIcon_PhotoGrow = transform2.gameObject;
			this.PhotoGrowCampaignAnim = transform2.Find("Popup_Campaign_Friends").GetComponent<SimpleAnimation>();
			transform2.Find("Popup_Campaign_Friends/Txt_kind").GetComponent<PguiTextCtrl>().text = "フォト強化";
			transform2.Find("Popup_Campaign_Friends/Txt_kind").gameObject.SetActive(false);
			this.Btn_PhotoSell = baseTr.Find("All/MenuAll/Btn_PhotoSell").GetComponent<PguiButtonCtrl>();
			this.Btn_CharaAll = baseTr.Find("All/MenuAll/Btn_CharaAll").GetComponent<PguiButtonCtrl>();
			this.Btn_HelperChange = baseTr.Find("All/MenuAll/Btn_HelperChange").GetComponent<PguiButtonCtrl>();
			this.CharaEditTop_SE = baseTr.GetComponent<SimpleAnimation>();
			this.Btn_KemoBoard = baseTr.Find("All/MenuAll/Btn_KemoBoard").GetComponent<PguiButtonCtrl>();
			this.Btn_Training = baseTr.Find("All/MenuAll/Btn_Training").GetComponent<PguiButtonCtrl>();
			this.markLockTraining = this.Btn_Training.transform.Find("Mark_Lock").GetComponent<MarkLockCtrl>();
			this.Btn_MasterSkillGrow = baseTr.Find("All/MenuAll/Btn_UserSkillGrow").GetComponent<PguiButtonCtrl>();
			this.Btn_Accessory = baseTr.Find("All/MenuAll/Btn_Accessory").GetComponent<PguiButtonCtrl>();
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_CharaDeck;

		public PguiButtonCtrl Btn_CharaGrow;

		public PguiButtonCtrl Btn_PhotoGrow;

		public PguiButtonCtrl Btn_CharaAll;

		public PguiButtonCtrl Btn_PhotoSell;

		public PguiButtonCtrl Btn_HelperChange;

		public PguiButtonCtrl Btn_KemoBoard;

		public PguiButtonCtrl Btn_Training;

		public PguiButtonCtrl Btn_MasterSkillGrow;

		public PguiButtonCtrl Btn_Accessory;

		public GameObject CampaignIcon_CharaGrow;

		public GameObject CampaignIcon_PhotoGrow;

		public SimpleAnimation CharaEditTop_SE;

		public SimpleAnimation CharaGrowCampaignAnim;

		public SimpleAnimation PhotoGrowCampaignAnim;

		public MarkLockCtrl markLockTraining;
	}

	public enum Mode
	{
		INVALID,
		TOP,
		DECK,
		CHARA_GROW,
		PHOTO_GROW,
		CHARA_PHOTO_ALL,
		PHOTO_SELL,
		MASTER_SKILL_GROW,
		ACCESSORY_GROW,
		ACCESSORY_SELL
	}
}
