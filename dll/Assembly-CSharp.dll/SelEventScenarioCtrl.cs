using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

public class SelEventScenarioCtrl : MonoBehaviour
{
	public SelEventScenarioCtrl.GUI GuiData { get; private set; }

	private int UIType { get; set; }

	private int EventId { get; set; }

	private string LoadAssetPath { get; set; }

	private float currentFadeTime
	{
		get
		{
			return Singleton<CanvasManager>.Instance.CurrentFadeTimes[1];
		}
		set
		{
			Singleton<CanvasManager>.Instance.CurrentFadeTimes[1] = value;
		}
	}

	public int MapId { get; set; }

	public void Init(SelEventScenarioCtrl.InitParam _initParam, SelEventScenarioCtrl.SetupParam _setupParam)
	{
		this.UIType = -1;
		this.EventId = -1;
		this.createGui = null;
		this.initParam = _initParam;
		this.Setup(_setupParam);
	}

	public void Setup(SelEventScenarioCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		if (this.createGui != null)
		{
			return;
		}
		this.createGui = Singleton<SceneManager>.Instance.StartCoroutine(this.CreateGUI());
	}

	public void UpdateDecoration(bool isInit = false, bool isRecreate = false)
	{
		this.normalIndex = 0;
		this.isInitUpdate = isInit;
		this.isRecreateUpdate = isRecreate;
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.setupParam.eventData.eventId);
		if (eventData == null)
		{
			return;
		}
		List<QuestStaticMap> mapDataList = DataManager.DmQuest.QuestStaticData.chapterDataMap[eventData.eventChapterId].mapDataList;
		DateTime now = TimeManager.Now;
		if (mapDataList.Count > SelEventScenarioCtrl.OtherStoryIndex && mapDataList[SelEventScenarioCtrl.OtherStoryIndex].StartDateTime < now && this.isInitUpdate)
		{
			this.normalIndex = SelEventScenarioCtrl.OtherStoryIndex;
		}
		DateTime startTime = mapDataList[this.normalIndex].questGroupList[0].startTime;
		DateTime endTime = mapDataList[this.normalIndex].questGroupList[0].endTime;
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.EVENT);
		this.GuiData.eventSelect.Btn_Main.SetActEnable(playableMapIdList.Contains(mapDataList[this.normalIndex].mapId), false, false);
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
		this.GuiData.eventSelect.Btn_Yaji_Left.gameObject.SetActive(false);
		this.GuiData.eventSelect.Btn_Yaji_Right.gameObject.SetActive(false);
		if (mapDataList.Count > SelEventScenarioCtrl.OtherStoryIndex)
		{
			this.GuiData.eventSelect.Btn_Yaji_Left.gameObject.SetActive(mapDataList[SelEventScenarioCtrl.OtherStoryIndex].StartDateTime < now);
			this.GuiData.eventSelect.Btn_Yaji_Right.gameObject.SetActive(mapDataList[SelEventScenarioCtrl.OtherStoryIndex].StartDateTime < now);
		}
		this.GuiData.eventSelect.createRTC();
		this.GuiData.eventSelect.renderTextureChara.SetupFace(this.setupParam.eventData.dispCharaId, 0, RenderTextureChara.StrBodyMotionId2CharaModionDefineActKey(this.setupParam.eventData.dispCharaBodyMotion), FacePackData.Id2PackData(this.setupParam.eventData.dispCharaFaceMotion), 0, false, true, null, false);
		string text = ((this.normalIndex == SelEventScenarioCtrl.OtherStoryIndex) ? this.setupParam.eventData.StoryFilename2 : this.setupParam.eventData.StoryFilename);
		this.GuiData.eventSelect.Tex_Photo.banner = text;
		if (this.GuiData.eventSelect.Txt_Title != null)
		{
			this.GuiData.eventSelect.Txt_Title.text = ((this.normalIndex == SelEventScenarioCtrl.OtherStoryIndex) ? this.setupParam.eventData.eventTitleScenario2 : this.setupParam.eventData.eventTitleScenario);
			if (this.GuiData.eventSelect.Txt_Title.text == string.Empty)
			{
				this.GuiData.eventSelect.Txt_Title.text = "メインストーリー";
			}
		}
		if (isRecreate)
		{
			this.currentFadeTime = 0f;
			this.GuiData.eventSelect.Tex_BG = null;
		}
		CanvasManager.SetScenarioBgInQuestBgTexture(this.setupParam.eventData.bgFilename);
		if (this.setupParam.eventData.bgFilename2 != string.Empty && this.GuiData.eventSelect.Tex_BG == null)
		{
			this.initBg = this.InitializeBG();
		}
		else
		{
			PguiRawImageCtrl component = Singleton<CanvasManager>.Instance.GetBg(Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInQuestBg).transform.parent.Find("Texture_BG_Other").GetComponent<PguiRawImageCtrl>();
			if (component != null)
			{
				component.m_RawImage.color = new Color(component.m_RawImage.color.r, component.m_RawImage.color.g, component.m_RawImage.color.b, 0f);
			}
		}
		if (this.initBg == null && (this.isInitUpdate || isRecreate) && this.setupParam.eventData.bgFilename2 != string.Empty)
		{
			this.currentFadeTime = 0f;
			this.animateBg = this.AnimateBg(true);
		}
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
			SelEventScenarioCtrl.<>c__DisplayClass44_0 CS$<>8__locals1 = new SelEventScenarioCtrl.<>c__DisplayClass44_0();
			CS$<>8__locals1.<>4__this = this;
			this.Destroy();
			this.UIType = this.setupParam.eventData.modeUIType;
			this.EventId = this.setupParam.eventData.eventId;
			switch (this.UIType)
			{
			default:
				this.LoadAssetPath = "Gui/Event/GUI_Event_Scenario05";
				break;
			case 2:
				this.LoadAssetPath = "Gui/Event/GUI_Event_Scenario06";
				break;
			case 3:
				this.LoadAssetPath = "Gui/Event/GUI_Event_Scenario07";
				break;
			case 4:
				this.LoadAssetPath = "Gui/Event/GUI_Event_Scenario08";
				break;
			}
			yield return this.LoadAssetObject(this.LoadAssetPath);
			this.prefabObj = AssetManager.InstantiateAssetData(this.LoadAssetPath, base.transform);
			this.GuiData = new SelEventScenarioCtrl.GUI(this.prefabObj.transform);
			this.GuiData.eventSelect.Btn_Extra.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestEvent), PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.eventSelect.Btn_Hard.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestEvent), PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.eventSelect.Btn_Main.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestEvent), PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.eventSelect.Btn_Shop.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestEvent), PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.eventSelect.Btn_Mission.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestEvent), PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.eventSelect.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickArrowButton), PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.eventSelect.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickArrowButton), PguiButtonCtrl.SoundType.DEFAULT);
			CS$<>8__locals1.releaseEffectsList = DataManager.DmEvent.GetReleaseEffectsList();
			CS$<>8__locals1.releaseEffects = null;
			QuestUtil.GetEnableEventReleaseEffects(ref CS$<>8__locals1.releaseEffectsList, ref CS$<>8__locals1.releaseEffects, this.setupParam.eventData);
			DateTime stt0 = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.setupParam.eventData.eventChapterId].mapDataList[SelEventScenarioCtrl.HardIndex].questGroupList[0].startTime;
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
			DateTime stt = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.setupParam.eventData.eventChapterId].mapDataList[SelEventScenarioCtrl.ExtraIndex].questGroupList[0].startTime;
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

	private IEnumerator AnimateBg(bool isFull)
	{
		do
		{
			Singleton<CanvasManager>.Instance.ChangeBg(this.GuiData.eventSelect.Tex_BG.m_RawImage, 1, Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInQuestBg, true, this.normalIndex != SelEventScenarioCtrl.OtherStoryIndex, isFull, null, "Texture_BG_Other");
			yield return null;
		}
		while (this.currentFadeTime < CanvasManager.FADE_DURATION);
		yield break;
	}

	private void UpdateMain()
	{
		string text = ((this.normalIndex == SelEventScenarioCtrl.OtherStoryIndex) ? this.setupParam.eventData.StoryFilename2 : this.setupParam.eventData.StoryFilename);
		this.GuiData.eventSelect.Tex_Photo.banner = text;
		if (this.GuiData.eventSelect.Txt_Title != null)
		{
			this.GuiData.eventSelect.Txt_Title.text = ((this.normalIndex == SelEventScenarioCtrl.OtherStoryIndex) ? this.setupParam.eventData.eventTitleScenario2 : this.setupParam.eventData.eventTitleScenario);
			if (this.GuiData.eventSelect.Txt_Title.text == string.Empty)
			{
				this.GuiData.eventSelect.Txt_Title.text = "メインストーリー";
			}
		}
	}

	private IEnumerator InitializeBG()
	{
		while (Singleton<CanvasManager>.Instance.GetBg(Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInQuestBg) == null)
		{
			yield return null;
		}
		GameObject bg = Singleton<CanvasManager>.Instance.GetBg(Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInQuestBg);
		this.GuiData.eventSelect.Tex_BG = bg.transform.parent.Find("Texture_BG_Other").GetComponent<PguiRawImageCtrl>();
		this.GuiData.eventSelect.Tex_BG.banner = this.setupParam.eventData.bgFilename2;
		IEnumerator ienum = null;
		if ((this.isInitUpdate || this.isRecreateUpdate) && this.setupParam.eventData.bgFilename2 != string.Empty)
		{
			this.currentFadeTime = 0f;
			ienum = this.AnimateBg(true);
		}
		if (ienum != null)
		{
			while (ienum.MoveNext())
			{
				yield return null;
			}
		}
		yield break;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.initBg != null && !this.initBg.MoveNext())
		{
			this.initBg = null;
		}
		if (this.animateBg != null && !this.animateBg.MoveNext())
		{
			this.animateBg = null;
		}
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
				DateTime startTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.setupParam.eventData.eventChapterId].mapDataList[SelEventScenarioCtrl.ExtraIndex].questGroupList[0].startTime;
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
			this.MapId = questStaticChapter.mapDataList[SelEventScenarioCtrl.ExtraIndex].mapId;
			return;
		}
		else if (button == this.GuiData.eventSelect.Btn_Hard)
		{
			if (this.GuiData.eventSelect.Btn_HardMarkLockCtrl.IsActive())
			{
				DateTime startTime2 = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.setupParam.eventData.eventChapterId].mapDataList[SelEventScenarioCtrl.HardIndex].questGroupList[0].startTime;
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
			this.MapId = questStaticChapter.mapDataList[SelEventScenarioCtrl.HardIndex].mapId;
			return;
		}
		else
		{
			if (button == this.GuiData.eventSelect.Btn_Main)
			{
				this.initParam.reqNextSequenceCB();
				this.MapId = questStaticChapter.mapDataList[this.normalIndex].mapId;
				return;
			}
			if (button == this.GuiData.eventSelect.Btn_Shop)
			{
				SceneShopArgs sceneShopArgs = new SceneShopArgs();
				sceneShopArgs.resultNextSceneName = SceneManager.SceneName.SceneQuest;
				sceneShopArgs.resultNextSceneArgs = new SceneQuest.Args
				{
					selectEventId = eventData.eventId,
					category = QuestStaticChapter.Category.EVENT,
					backSequenceGameObject = this.GuiData.eventSelect.baseObj
				};
				sceneShopArgs.shopId = 0;
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneShop, sceneShopArgs);
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

	private void OnClickArrowButton(PguiButtonCtrl button)
	{
		this.normalIndex = ((this.normalIndex == 0) ? SelEventScenarioCtrl.OtherStoryIndex : 0);
		this.UpdateMain();
		if (this.currentFadeTime != 0f)
		{
			this.currentFadeTime = CanvasManager.FADE_DURATION - this.currentFadeTime;
		}
		this.animateBg = this.AnimateBg(false);
	}

	private void OnClickEventInfoBanner(Transform tf)
	{
		QuestUtil.OpenBannerWebViewWindow(this.setupParam.eventData.eventId);
	}

	private int normalIndex;

	private static readonly int HardIndex = 1;

	private static readonly int ExtraIndex = 2;

	private static readonly int OtherStoryIndex = 3;

	private SelEventScenarioCtrl.InitParam initParam = new SelEventScenarioCtrl.InitParam();

	private SelEventScenarioCtrl.SetupParam setupParam = new SelEventScenarioCtrl.SetupParam();

	private GameObject prefabObj;

	private Coroutine createGui;

	private IEnumerator animateBg;

	private IEnumerator initBg;

	private bool isInitUpdate;

	private bool isRecreateUpdate;

	public class InitParam
	{
		public UnityAction reqNextSequenceCB;

		public UnityAction reqBackSequenceCB;

		public UnityAction reqShopSequenceCB;

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
			this.eventSelect = new SelEventScenarioCtrl.GUI.EventSelect(baseTr.transform);
			this.eventSelect.baseObj.SetActive(false);
			this.eventInfo = new List<SelEventScenarioCtrl.GUI.EventInfo>
			{
				new SelEventScenarioCtrl.GUI.EventInfo(baseTr.Find("Tex_EventInfo/Grid/ItemOwnBase01")),
				new SelEventScenarioCtrl.GUI.EventInfo(baseTr.Find("Tex_EventInfo/Grid/ItemOwnBase02"))
			};
		}

		public SelEventScenarioCtrl.GUI.EventSelect eventSelect;

		public List<SelEventScenarioCtrl.GUI.EventInfo> eventInfo;

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
				this.Btn_Shop = baseTr.Find("Tex_EventInfo/Btn_Shop").GetComponent<PguiButtonCtrl>();
				this.Btn_Mission = baseTr.Find("Tex_EventInfo/Btn_Mission").GetComponent<PguiButtonCtrl>();
				this.Txt_Mission_Num = this.Btn_Mission.transform.Find("BaseImage/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
				this.Btn_Yaji_Left = baseTr.Find("LeftBtn/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
				this.Btn_Yaji_Right = baseTr.Find("RightBtn/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
				Transform transform = baseTr.Find("Btn_Main/Txt_Title");
				this.Txt_Title = ((transform != null) ? transform.GetComponent<PguiTextCtrl>() : null);
			}

			public void createRTC()
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

			public PguiButtonCtrl Btn_Shop;

			public PguiButtonCtrl Btn_Mission;

			public PguiTextCtrl Txt_Mission_Num;

			public PguiButtonCtrl Btn_Yaji_Left;

			public PguiButtonCtrl Btn_Yaji_Right;

			public PguiTextCtrl Txt_Title;

			public PguiRawImageCtrl Tex_BG;
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
