using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

public class SelEventTowerCtrl : MonoBehaviour
{
	public SelEventTowerCtrl.GUI GuiData { get; private set; }

	private int UIType { get; set; }

	private int EventId { get; set; }

	private string LoadAssetPath { get; set; }

	public int MapId { get; set; }

	public void Init(SelEventTowerCtrl.InitParam _initParam, SelEventTowerCtrl.SetupParam _setupParam)
	{
		this.UIType = -1;
		this.EventId = -1;
		this.createGui = null;
		this.initParam = _initParam;
		this.Setup(_setupParam);
	}

	public void Setup(SelEventTowerCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		if (this.createGui != null)
		{
			return;
		}
		this.createGui = Singleton<SceneManager>.Instance.StartCoroutine(this.CreateGUI());
	}

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

	public void Destroy()
	{
		if (this.prefabObj != null)
		{
			AssetManager.UnloadAssetData(this.LoadAssetPath, AssetManager.OWNER.QuestSelector);
			this.prefabObj = null;
		}
	}

	public static void PlayBGM()
	{
		SoundManager.PlayBGM("prd_bgm0032");
	}

	public static void OpenHelpWindow()
	{
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "イベントの遊びかた", new List<string> { "Texture2D/Tutorial_Window/HardEvent/tutorial_hardevent_01", "Texture2D/Tutorial_Window/HardEvent/tutorial_hardevent_02" }, null);
	}

	private IEnumerator LoadAssetObject(string path)
	{
		AssetManager.LoadAssetData(path, AssetManager.OWNER.QuestSelector, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(path))
		{
			yield return null;
		}
		yield break;
	}

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

	private void Start()
	{
	}

	private void Update()
	{
	}

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

	private void OnClickEventInfoBanner(Transform tf)
	{
		QuestUtil.OpenBannerWebViewWindow(this.setupParam.eventData.eventId);
	}

	private static readonly int NormalIndex = 0;

	private static readonly int HardIndex = 1;

	private static readonly int ExtraIndex = 2;

	private SelEventTowerCtrl.InitParam initParam = new SelEventTowerCtrl.InitParam();

	private SelEventTowerCtrl.SetupParam setupParam = new SelEventTowerCtrl.SetupParam();

	private GameObject prefabObj;

	private Coroutine createGui;

	public class InitParam
	{
		public UnityAction reqNextSequenceCB;

		public UnityAction reqBackSequenceCB;

		public UnityAction reqGachaSequenceCB;

		public UnityAction selectObjsCB;
	}

	public class SetupParam
	{
		public DataManagerEvent.EventData eventData;

		public UnityAction reqNextSequenceCB;
	}

	public class GUI
	{
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

		public SelEventTowerCtrl.GUI.EventSelect eventSelect;

		public List<SelEventTowerCtrl.GUI.EventInfo> eventInfo;

		public class EventSelect
		{
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

			public GameObject baseObj;

			public PguiButtonCtrl Btn_Main;

			public PguiButtonCtrl Btn_Hard;

			public MarkLockCtrl Btn_HardMarkLockCtrl;

			public PguiButtonCtrl Btn_Extra;

			public MarkLockCtrl Btn_ExtraMarkLockCtrl;

			public PguiRawImageCtrl Tex_Event_Banner;

			public PguiImageCtrl Tex_EventDay;

			public PguiRawImageCtrl Tex_Photo;

			public PguiTextCtrl Num_Txt_Day;

			public RenderTextureChara renderTextureChara;

			public PguiButtonCtrl Btn_Gacha;

			public PguiButtonCtrl Btn_Mission;

			public PguiTextCtrl Txt_Mission_Num;
		}

		public class EventInfo
		{
			public EventInfo(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Img_ItemIcon = baseTr.Find("Icon_Stone").GetComponent<PguiRawImageCtrl>();
				this.Txt_ItemNum = baseTr.Find("Num_Txt").GetComponent<PguiTextCtrl>();
			}

			public GameObject baseObj;

			public PguiRawImageCtrl Img_ItemIcon;

			public PguiTextCtrl Txt_ItemNum;
		}
	}
}
