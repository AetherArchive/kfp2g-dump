using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000176 RID: 374
public class SelEventTowerCtrl : MonoBehaviour
{
	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x060017CD RID: 6093 RVA: 0x00126E3A File Offset: 0x0012503A
	// (set) Token: 0x060017CE RID: 6094 RVA: 0x00126E42 File Offset: 0x00125042
	public SelEventTowerCtrl.GUI GuiData { get; private set; }

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x060017CF RID: 6095 RVA: 0x00126E4B File Offset: 0x0012504B
	// (set) Token: 0x060017D0 RID: 6096 RVA: 0x00126E53 File Offset: 0x00125053
	private int UIType { get; set; }

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00126E5C File Offset: 0x0012505C
	// (set) Token: 0x060017D2 RID: 6098 RVA: 0x00126E64 File Offset: 0x00125064
	private int EventId { get; set; }

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x060017D3 RID: 6099 RVA: 0x00126E6D File Offset: 0x0012506D
	// (set) Token: 0x060017D4 RID: 6100 RVA: 0x00126E75 File Offset: 0x00125075
	private string LoadAssetPath { get; set; }

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x060017D5 RID: 6101 RVA: 0x00126E7E File Offset: 0x0012507E
	// (set) Token: 0x060017D6 RID: 6102 RVA: 0x00126E86 File Offset: 0x00125086
	public int MapId { get; set; }

	// Token: 0x060017D7 RID: 6103 RVA: 0x00126E8F File Offset: 0x0012508F
	public void Init(SelEventTowerCtrl.InitParam _initParam, SelEventTowerCtrl.SetupParam _setupParam)
	{
		this.UIType = -1;
		this.EventId = -1;
		this.createGui = null;
		this.initParam = _initParam;
		this.Setup(_setupParam);
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x00126EB4 File Offset: 0x001250B4
	public void Setup(SelEventTowerCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		if (this.createGui != null)
		{
			return;
		}
		this.createGui = Singleton<SceneManager>.Instance.StartCoroutine(this.CreateGUI());
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x00126EDC File Offset: 0x001250DC
	public void UpdateDecoration()
	{
		CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByTower(true);
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.setupParam.eventData.eventId);
		if (eventData == null)
		{
			return;
		}
		DateTime startTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[eventData.eventChapterId].mapDataList[SelEventTowerCtrl.NormalIndex].questGroupList[0].startTime;
		DateTime endTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[eventData.eventChapterId].mapDataList[SelEventTowerCtrl.NormalIndex].questGroupList[0].endTime;
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.EVENT);
		this.GuiData.eventSelect.Btn_Main.SetActEnable(playableMapIdList.Contains(DataManager.DmQuest.QuestStaticData.chapterDataMap[eventData.eventChapterId].mapDataList[SelEventTowerCtrl.NormalIndex].mapId), false, false);
		this.GuiData.eventSelect.Btn_HardMarkLockCtrl.StartAE();
		this.GuiData.eventSelect.Btn_ExtraMarkLockCtrl.StartAE();
		this.GuiData.eventSelect.Num_Txt_Day.text = startTime.ToString("M/d") + " ～ " + endTime.ToString("M/d HH:mm まで");
		int userClearEventMissionNum = DataManager.DmMission.GetUserClearEventMissionNum(eventData.eventId);
		this.GuiData.eventSelect.Txt_Mission_Num.transform.parent.transform.gameObject.SetActive(userClearEventMissionNum > 0);
		this.GuiData.eventSelect.Txt_Mission_Num.text = userClearEventMissionNum.ToString();
		for (int i = 0; i < 2; i++)
		{
			if (i < eventData.eventCoinIdList.Count)
			{
				this.GuiData.eventInfo[i].baseObj.SetActive(true);
				this.GuiData.eventInfo[i].Txt_ItemNum.text = DataManager.DmItem.GetUserItemData(eventData.eventCoinIdList[i]).num.ToString();
				this.GuiData.eventInfo[i].Img_ItemIcon.SetRawImage(DataManager.DmItem.GetItemStaticBase(this.setupParam.eventData.eventCoinIdList[i]).GetIconName(), true, false, null);
			}
			else
			{
				this.GuiData.eventInfo[i].baseObj.SetActive(false);
			}
		}
		this.GuiData.eventSelect.CreateRTC();
		this.GuiData.eventSelect.renderTextureChara.SetupFace(this.setupParam.eventData.dispCharaId, 0, RenderTextureChara.StrBodyMotionId2CharaModionDefineActKey(this.setupParam.eventData.dispCharaBodyMotion), FacePackData.Id2PackData(this.setupParam.eventData.dispCharaFaceMotion), 0, false, true, null, false);
		this.GuiData.eventSelect.Tex_Photo.banner = this.setupParam.eventData.StoryFilename;
		CanvasManager.SetScenarioBgInQuestBgTexture(this.setupParam.eventData.bgFilename);
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x00127224 File Offset: 0x00125424
	public void Dest()
	{
		if (this.GuiData == null)
		{
			return;
		}
		this.GuiData.eventSelect.baseObj.SetActive(false);
		if (this.GuiData.eventSelect.renderTextureChara != null)
		{
			Object.Destroy(this.GuiData.eventSelect.renderTextureChara.gameObject);
		}
		this.GuiData.eventSelect.renderTextureChara = null;
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x00127293 File Offset: 0x00125493
	public void Destroy()
	{
		if (this.prefabObj != null)
		{
			AssetManager.UnloadAssetData(this.LoadAssetPath, AssetManager.OWNER.QuestSelector);
			this.prefabObj = null;
		}
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x001272B7 File Offset: 0x001254B7
	public static void PlayBGM()
	{
		SoundManager.PlayBGM("prd_bgm0032");
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x001272C3 File Offset: 0x001254C3
	public static void OpenHelpWindow()
	{
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "イベントの遊びかた", new List<string> { "Texture2D/Tutorial_Window/HardEvent/tutorial_hardevent_01", "Texture2D/Tutorial_Window/HardEvent/tutorial_hardevent_02" }, null);
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x001272F1 File Offset: 0x001254F1
	private IEnumerator LoadAssetObject(string path)
	{
		AssetManager.LoadAssetData(path, AssetManager.OWNER.QuestSelector, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(path))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x00127300 File Offset: 0x00125500
	private IEnumerator CreateGUI()
	{
		if (this.prefabObj == null || this.setupParam.eventData.modeUIType != this.UIType || this.setupParam.eventData.eventId != this.EventId)
		{
			SelEventTowerCtrl.<>c__DisplayClass38_0 CS$<>8__locals1 = new SelEventTowerCtrl.<>c__DisplayClass38_0();
			CS$<>8__locals1.<>4__this = this;
			this.Destroy();
			this.UIType = this.setupParam.eventData.modeUIType;
			this.EventId = this.setupParam.eventData.eventId;
			int uitype = this.UIType;
			this.LoadAssetPath = "Gui/Event/GUI_Event_Tower01";
			yield return this.LoadAssetObject(this.LoadAssetPath);
			this.prefabObj = AssetManager.InstantiateAssetData(this.LoadAssetPath, base.transform);
			this.GuiData = new SelEventTowerCtrl.GUI(this.prefabObj.transform);
			this.GuiData.eventSelect.Btn_Extra.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestEvent), PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.eventSelect.Btn_Hard.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestEvent), PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.eventSelect.Btn_Main.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestEvent), PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.eventSelect.Btn_Gacha.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestEvent), PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.eventSelect.Btn_Mission.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestEvent), PguiButtonCtrl.SoundType.DEFAULT);
			CS$<>8__locals1.releaseEffectsList = DataManager.DmEvent.GetReleaseEffectsList();
			CS$<>8__locals1.releaseEffects = null;
			QuestUtil.GetEnableEventReleaseEffects(ref CS$<>8__locals1.releaseEffectsList, ref CS$<>8__locals1.releaseEffects, this.setupParam.eventData);
			DateTime stt0 = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.setupParam.eventData.eventChapterId].mapDataList[SelEventTowerCtrl.HardIndex].questGroupList[0].startTime;
			QuestStaticQuestOne questOneData2 = DataManager.DmQuest.QuestStaticData.oneDataList.Find((QuestStaticQuestOne item) => item.questId == this.setupParam.eventData.hardopenQuestOneid);
			QuestStaticQuestGroup questGroupData2 = null;
			QuestStaticMap questStaticMap = null;
			if (questOneData2 != null)
			{
				questGroupData2 = DataManager.DmQuest.QuestStaticData.groupDataList.Find((QuestStaticQuestGroup item) => item.questGroupId == questOneData2.questGroupId);
				if (questGroupData2 != null)
				{
					questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap item) => item.mapId == questGroupData2.mapId);
				}
			}
			string text = "";
			if (questGroupData2 != null && questStaticMap != null)
			{
				text = questStaticMap.mapName + questGroupData2.titleName;
			}
			this.GuiData.eventSelect.Btn_HardMarkLockCtrl.Setup(new MarkLockCtrl.SetupParam
			{
				updateConditionCallback = delegate
				{
					QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataList.Find((QuestDynamicQuestOne item) => item.questOneId == CS$<>8__locals1.<>4__this.setupParam.eventData.hardopenQuestOneid);
					return questDynamicQuestOne != null && questDynamicQuestOne.status != QuestOneStatus.NEW && SceneQuest.TimeStampInScene >= stt0;
				},
				releaseFlag = (CS$<>8__locals1.releaseEffects.ReleaseIdList[0] == 1),
				tagetObject = this.GuiData.eventSelect.Btn_Hard.gameObject,
				text = text + "クリア\n" + stt0.ToString("M/d HH:mm") + " 以降",
				updateUserFlagDataCallback = delegate
				{
					DataManager.DmQuest.RequestGetUserQuestInfo();
					CS$<>8__locals1.releaseEffects.ReleaseIdList[0] = 1;
					DataManager.DmEvent.RequestUpdateReleaseEffects(CS$<>8__locals1.releaseEffectsList);
				}
			}, false);
			DateTime stt = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.setupParam.eventData.eventChapterId].mapDataList[SelEventTowerCtrl.ExtraIndex].questGroupList[0].startTime;
			QuestStaticQuestOne questOneData = DataManager.DmQuest.QuestStaticData.oneDataList.Find((QuestStaticQuestOne item) => item.questId == this.setupParam.eventData.extraopenQuestOneid);
			QuestStaticQuestGroup questGroupData = null;
			QuestStaticMap questStaticMap2 = null;
			if (questOneData != null)
			{
				questGroupData = DataManager.DmQuest.QuestStaticData.groupDataList.Find((QuestStaticQuestGroup item) => item.questGroupId == questOneData.questGroupId);
				if (questGroupData != null)
				{
					questStaticMap2 = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap item) => item.mapId == questGroupData.mapId);
				}
			}
			string text2 = "";
			if (questGroupData != null && questStaticMap2 != null)
			{
				text2 = questStaticMap2.mapName + questGroupData.titleName;
			}
			this.GuiData.eventSelect.Btn_ExtraMarkLockCtrl.Setup(new MarkLockCtrl.SetupParam
			{
				updateConditionCallback = delegate
				{
					QuestDynamicQuestOne questDynamicQuestOne2 = DataManager.DmQuest.QuestDynamicData.oneDataList.Find((QuestDynamicQuestOne item) => item.questOneId == CS$<>8__locals1.<>4__this.setupParam.eventData.extraopenQuestOneid);
					return questDynamicQuestOne2 != null && questDynamicQuestOne2.status != QuestOneStatus.NEW && SceneQuest.TimeStampInScene >= stt;
				},
				releaseFlag = (CS$<>8__locals1.releaseEffects.ReleaseIdList[1] == 1),
				tagetObject = this.GuiData.eventSelect.Btn_Extra.gameObject,
				text = text2 + "クリア\n" + stt.ToString("M/d HH:mm") + " 以降",
				updateUserFlagDataCallback = delegate
				{
					DataManager.DmQuest.RequestGetUserQuestInfo();
					CS$<>8__locals1.releaseEffects.ReleaseIdList[1] = 1;
					DataManager.DmEvent.RequestUpdateReleaseEffects(CS$<>8__locals1.releaseEffectsList);
				}
			}, false);
			HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(this.setupParam.eventData.eventBannerId);
			if (homeBannerData != null)
			{
				this.GuiData.eventSelect.Tex_Event_Banner.banner = homeBannerData.bannerImagePathEvent;
			}
			PrjUtil.AddTouchEventTrigger(this.GuiData.eventSelect.Tex_Event_Banner.gameObject, new UnityAction<Transform>(this.OnClickEventInfoBanner));
			UnityAction selectObjsCB = this.initParam.selectObjsCB;
			if (selectObjsCB != null)
			{
				selectObjsCB();
			}
			if (CS$<>8__locals1.releaseEffects.TutorialPhase == 0)
			{
				SelEventTowerCtrl.OpenHelpWindow();
				CS$<>8__locals1.releaseEffects.TutorialPhase = 1;
				DataManager.DmEvent.RequestUpdateReleaseEffects(CS$<>8__locals1.releaseEffectsList);
			}
			CS$<>8__locals1 = null;
		}
		UnityAction reqNextSequenceCB = this.setupParam.reqNextSequenceCB;
		if (reqNextSequenceCB != null)
		{
			reqNextSequenceCB();
		}
		this.createGui = null;
		yield break;
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x0012730F File Offset: 0x0012550F
	private void Start()
	{
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x00127311 File Offset: 0x00125511
	private void Update()
	{
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x00127314 File Offset: 0x00125514
	private void OnClickButtonQuestEvent(PguiButtonCtrl button)
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.setupParam.eventData.eventId);
		if (eventData == null)
		{
			return;
		}
		QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[eventData.eventChapterId];
		DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.EVENT);
		if (button == this.GuiData.eventSelect.Btn_Extra)
		{
			if (this.GuiData.eventSelect.Btn_ExtraMarkLockCtrl.IsActive())
			{
				DateTime startTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.setupParam.eventData.eventChapterId].mapDataList[SelEventTowerCtrl.ExtraIndex].questGroupList[0].startTime;
				QuestStaticQuestOne questOneData2 = DataManager.DmQuest.QuestStaticData.oneDataList.Find((QuestStaticQuestOne item) => item.questId == this.setupParam.eventData.extraopenQuestOneid);
				QuestStaticQuestGroup questGroupData2 = null;
				QuestStaticMap questStaticMap = null;
				if (questOneData2 != null)
				{
					questGroupData2 = DataManager.DmQuest.QuestStaticData.groupDataList.Find((QuestStaticQuestGroup item) => item.questGroupId == questOneData2.questGroupId);
					if (questGroupData2 != null)
					{
						questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap item) => item.mapId == questGroupData2.mapId);
					}
				}
				string text = "";
				if (questGroupData2 != null && questStaticMap != null)
				{
					text = questStaticMap.mapName + questGroupData2.titleName;
				}
				QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataList.Find((QuestDynamicQuestOne item) => item.questOneId == this.setupParam.eventData.extraopenQuestOneid);
				CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("モード解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
				{
					new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = text + "クリア",
						enableClear = (questDynamicQuestOne != null && questDynamicQuestOne.status != QuestOneStatus.NEW)
					},
					new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = startTime.ToString("M/d HH:mm") + " 以降",
						enableClear = (SceneQuest.TimeStampInScene >= startTime)
					}
				});
				return;
			}
			this.initParam.reqNextSequenceCB();
			this.MapId = questStaticChapter.mapDataList[SelEventTowerCtrl.ExtraIndex].mapId;
			return;
		}
		else if (button == this.GuiData.eventSelect.Btn_Hard)
		{
			if (this.GuiData.eventSelect.Btn_HardMarkLockCtrl.IsActive())
			{
				DateTime startTime2 = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.setupParam.eventData.eventChapterId].mapDataList[SelEventTowerCtrl.HardIndex].questGroupList[0].startTime;
				QuestStaticQuestOne questOneData = DataManager.DmQuest.QuestStaticData.oneDataList.Find((QuestStaticQuestOne item) => item.questId == this.setupParam.eventData.hardopenQuestOneid);
				QuestStaticQuestGroup questGroupData = null;
				QuestStaticMap questStaticMap2 = null;
				if (questOneData != null)
				{
					questGroupData = DataManager.DmQuest.QuestStaticData.groupDataList.Find((QuestStaticQuestGroup item) => item.questGroupId == questOneData.questGroupId);
					if (questGroupData != null)
					{
						questStaticMap2 = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap item) => item.mapId == questGroupData.mapId);
					}
				}
				string text2 = "";
				if (questGroupData != null && questStaticMap2 != null)
				{
					text2 = questStaticMap2.mapName + questGroupData.titleName;
				}
				QuestDynamicQuestOne questDynamicQuestOne2 = DataManager.DmQuest.QuestDynamicData.oneDataList.Find((QuestDynamicQuestOne item) => item.questOneId == this.setupParam.eventData.hardopenQuestOneid);
				CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("モード解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
				{
					new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = text2 + "クリア",
						enableClear = (questDynamicQuestOne2 != null && questDynamicQuestOne2.status != QuestOneStatus.NEW)
					},
					new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = startTime2.ToString("M/d HH:mm") + " 以降",
						enableClear = (SceneQuest.TimeStampInScene >= startTime2)
					}
				});
				return;
			}
			this.initParam.reqNextSequenceCB();
			this.MapId = questStaticChapter.mapDataList[SelEventTowerCtrl.HardIndex].mapId;
			return;
		}
		else
		{
			if (button == this.GuiData.eventSelect.Btn_Main)
			{
				this.initParam.reqNextSequenceCB();
				this.MapId = questStaticChapter.mapDataList[SelEventTowerCtrl.NormalIndex].mapId;
				return;
			}
			if (button == this.GuiData.eventSelect.Btn_Gacha)
			{
				SceneGacha.OpenParam openParam = new SceneGacha.OpenParam
				{
					gachaId = this.setupParam.eventData.eventGachaId,
					resultNextSceneName = SceneManager.SceneName.SceneQuest,
					resultNextSceneArgs = new SceneQuest.Args
					{
						selectEventId = eventData.eventId,
						category = QuestStaticChapter.Category.EVENT,
						backSequenceGameObject = this.GuiData.eventSelect.baseObj
					}
				};
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneGacha, openParam);
				return;
			}
			if (button == this.GuiData.eventSelect.Btn_Mission)
			{
				SceneMission.MissionOpenParam missionOpenParam = new SceneMission.MissionOpenParam(MissionType.EVENTTOTAL, eventData.eventId)
				{
					returnSceneName = SceneManager.SceneName.SceneQuest,
					resultNextSceneArgs = new SceneQuest.Args
					{
						selectEventId = eventData.eventId,
						category = QuestStaticChapter.Category.EVENT,
						backSequenceGameObject = this.GuiData.eventSelect.baseObj
					}
				};
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneMission, missionOpenParam);
			}
			return;
		}
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x001278BA File Offset: 0x00125ABA
	private void OnClickEventInfoBanner(Transform tf)
	{
		QuestUtil.OpenBannerWebViewWindow(this.setupParam.eventData.eventId);
	}

	// Token: 0x04001289 RID: 4745
	private static readonly int NormalIndex = 0;

	// Token: 0x0400128A RID: 4746
	private static readonly int HardIndex = 1;

	// Token: 0x0400128B RID: 4747
	private static readonly int ExtraIndex = 2;

	// Token: 0x0400128D RID: 4749
	private SelEventTowerCtrl.InitParam initParam = new SelEventTowerCtrl.InitParam();

	// Token: 0x0400128E RID: 4750
	private SelEventTowerCtrl.SetupParam setupParam = new SelEventTowerCtrl.SetupParam();

	// Token: 0x0400128F RID: 4751
	private GameObject prefabObj;

	// Token: 0x04001293 RID: 4755
	private Coroutine createGui;

	// Token: 0x02000D14 RID: 3348
	public class InitParam
	{
		// Token: 0x04004D60 RID: 19808
		public UnityAction reqNextSequenceCB;

		// Token: 0x04004D61 RID: 19809
		public UnityAction reqBackSequenceCB;

		// Token: 0x04004D62 RID: 19810
		public UnityAction reqGachaSequenceCB;

		// Token: 0x04004D63 RID: 19811
		public UnityAction selectObjsCB;
	}

	// Token: 0x02000D15 RID: 3349
	public class SetupParam
	{
		// Token: 0x04004D64 RID: 19812
		public DataManagerEvent.EventData eventData;

		// Token: 0x04004D65 RID: 19813
		public UnityAction reqNextSequenceCB;
	}

	// Token: 0x02000D16 RID: 3350
	public class GUI
	{
		// Token: 0x06004823 RID: 18467 RVA: 0x0021AF10 File Offset: 0x00219110
		public GUI(Transform baseTr)
		{
			this.eventSelect = new SelEventTowerCtrl.GUI.EventSelect(baseTr.transform);
			this.eventSelect.baseObj.SetActive(false);
			this.eventInfo = new List<SelEventTowerCtrl.GUI.EventInfo>
			{
				new SelEventTowerCtrl.GUI.EventInfo(baseTr.Find("Tex_EventInfo/Grid/ItemOwnBase01")),
				new SelEventTowerCtrl.GUI.EventInfo(baseTr.Find("Tex_EventInfo/Grid/ItemOwnBase02"))
			};
		}

		// Token: 0x04004D66 RID: 19814
		public SelEventTowerCtrl.GUI.EventSelect eventSelect;

		// Token: 0x04004D67 RID: 19815
		public List<SelEventTowerCtrl.GUI.EventInfo> eventInfo;

		// Token: 0x020011C1 RID: 4545
		public class EventSelect
		{
			// Token: 0x06005708 RID: 22280 RVA: 0x00255784 File Offset: 0x00253984
			public EventSelect(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Btn_Main = baseTr.Find("Btn_Main").GetComponent<PguiButtonCtrl>();
				this.Btn_Hard = baseTr.Find("Btn_Hard").GetComponent<PguiButtonCtrl>();
				this.Btn_HardMarkLockCtrl = this.Btn_Hard.transform.Find("Mark_Lock").GetComponent<MarkLockCtrl>();
				this.Btn_Extra = baseTr.Find("Btn_Extra").GetComponent<PguiButtonCtrl>();
				this.Btn_ExtraMarkLockCtrl = this.Btn_Extra.transform.Find("Mark_Lock").GetComponent<MarkLockCtrl>();
				this.Tex_Event_Banner = baseTr.Find("Tex_Event_Banner").GetComponent<PguiRawImageCtrl>();
				this.Tex_EventDay = baseTr.Find("Tex_EventDay").GetComponent<PguiImageCtrl>();
				this.Tex_Photo = baseTr.Find("Btn_Main/BaseImage/Tex_Photo").GetComponent<PguiRawImageCtrl>();
				this.Num_Txt_Day = baseTr.Find("Tex_EventDay/Num_Txt").GetComponent<PguiTextCtrl>();
				this.Btn_Gacha = baseTr.Find("Tex_EventInfo/Btn_Gacha").GetComponent<PguiButtonCtrl>();
				this.Btn_Mission = baseTr.Find("Tex_EventInfo/Btn_Mission").GetComponent<PguiButtonCtrl>();
				this.Txt_Mission_Num = this.Btn_Mission.transform.Find("BaseImage/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x06005709 RID: 22281 RVA: 0x002558CC File Offset: 0x00253ACC
			public void CreateRTC()
			{
				if (this.renderTextureChara != null)
				{
					return;
				}
				RenderTextureChara component = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.baseObj.transform).GetComponent<RenderTextureChara>();
				component.transform.SetAsFirstSibling();
				component.postion = new Vector2(-450f, -150f);
				component.fieldOfView = 18f;
				component.rotation = new Vector3(0f, -15f, 0f);
				component.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
				this.renderTextureChara = component;
			}

			// Token: 0x0400616A RID: 24938
			public GameObject baseObj;

			// Token: 0x0400616B RID: 24939
			public PguiButtonCtrl Btn_Main;

			// Token: 0x0400616C RID: 24940
			public PguiButtonCtrl Btn_Hard;

			// Token: 0x0400616D RID: 24941
			public MarkLockCtrl Btn_HardMarkLockCtrl;

			// Token: 0x0400616E RID: 24942
			public PguiButtonCtrl Btn_Extra;

			// Token: 0x0400616F RID: 24943
			public MarkLockCtrl Btn_ExtraMarkLockCtrl;

			// Token: 0x04006170 RID: 24944
			public PguiRawImageCtrl Tex_Event_Banner;

			// Token: 0x04006171 RID: 24945
			public PguiImageCtrl Tex_EventDay;

			// Token: 0x04006172 RID: 24946
			public PguiRawImageCtrl Tex_Photo;

			// Token: 0x04006173 RID: 24947
			public PguiTextCtrl Num_Txt_Day;

			// Token: 0x04006174 RID: 24948
			public RenderTextureChara renderTextureChara;

			// Token: 0x04006175 RID: 24949
			public PguiButtonCtrl Btn_Gacha;

			// Token: 0x04006176 RID: 24950
			public PguiButtonCtrl Btn_Mission;

			// Token: 0x04006177 RID: 24951
			public PguiTextCtrl Txt_Mission_Num;
		}

		// Token: 0x020011C2 RID: 4546
		public class EventInfo
		{
			// Token: 0x0600570A RID: 22282 RVA: 0x00255962 File Offset: 0x00253B62
			public EventInfo(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Img_ItemIcon = baseTr.Find("Icon_Stone").GetComponent<PguiRawImageCtrl>();
				this.Txt_ItemNum = baseTr.Find("Num_Txt").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x04006178 RID: 24952
			public GameObject baseObj;

			// Token: 0x04006179 RID: 24953
			public PguiRawImageCtrl Img_ItemIcon;

			// Token: 0x0400617A RID: 24954
			public PguiTextCtrl Txt_ItemNum;
		}
	}
}
