using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000175 RID: 373
public class SelEventScenarioCtrl : MonoBehaviour
{
	// Token: 0x170003AE RID: 942
	// (get) Token: 0x060017A8 RID: 6056 RVA: 0x00125E81 File Offset: 0x00124081
	// (set) Token: 0x060017A9 RID: 6057 RVA: 0x00125E89 File Offset: 0x00124089
	public SelEventScenarioCtrl.GUI GuiData { get; private set; }

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x060017AA RID: 6058 RVA: 0x00125E92 File Offset: 0x00124092
	// (set) Token: 0x060017AB RID: 6059 RVA: 0x00125E9A File Offset: 0x0012409A
	private int UIType { get; set; }

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x060017AC RID: 6060 RVA: 0x00125EA3 File Offset: 0x001240A3
	// (set) Token: 0x060017AD RID: 6061 RVA: 0x00125EAB File Offset: 0x001240AB
	private int EventId { get; set; }

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x060017AE RID: 6062 RVA: 0x00125EB4 File Offset: 0x001240B4
	// (set) Token: 0x060017AF RID: 6063 RVA: 0x00125EBC File Offset: 0x001240BC
	private string LoadAssetPath { get; set; }

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x060017B0 RID: 6064 RVA: 0x00125EC5 File Offset: 0x001240C5
	// (set) Token: 0x060017B1 RID: 6065 RVA: 0x00125ED7 File Offset: 0x001240D7
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

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x060017B2 RID: 6066 RVA: 0x00125EEA File Offset: 0x001240EA
	// (set) Token: 0x060017B3 RID: 6067 RVA: 0x00125EF2 File Offset: 0x001240F2
	public int MapId { get; set; }

	// Token: 0x060017B4 RID: 6068 RVA: 0x00125EFB File Offset: 0x001240FB
	public void Init(SelEventScenarioCtrl.InitParam _initParam, SelEventScenarioCtrl.SetupParam _setupParam)
	{
		this.UIType = -1;
		this.EventId = -1;
		this.createGui = null;
		this.initParam = _initParam;
		this.Setup(_setupParam);
	}

	// Token: 0x060017B5 RID: 6069 RVA: 0x00125F20 File Offset: 0x00124120
	public void Setup(SelEventScenarioCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		if (this.createGui != null)
		{
			return;
		}
		this.createGui = Singleton<SceneManager>.Instance.StartCoroutine(this.CreateGUI());
	}

	// Token: 0x060017B6 RID: 6070 RVA: 0x00125F48 File Offset: 0x00124148
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

	// Token: 0x060017B7 RID: 6071 RVA: 0x00126530 File Offset: 0x00124730
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

	// Token: 0x060017B8 RID: 6072 RVA: 0x0012659F File Offset: 0x0012479F
	public void Destroy()
	{
		if (this.prefabObj != null)
		{
			AssetManager.UnloadAssetData(this.LoadAssetPath, AssetManager.OWNER.QuestSelector);
			this.prefabObj = null;
		}
	}

	// Token: 0x060017B9 RID: 6073 RVA: 0x001265C3 File Offset: 0x001247C3
	private IEnumerator LoadAssetObject(string path)
	{
		AssetManager.LoadAssetData(path, AssetManager.OWNER.QuestSelector, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(path))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060017BA RID: 6074 RVA: 0x001265D2 File Offset: 0x001247D2
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

	// Token: 0x060017BB RID: 6075 RVA: 0x001265E1 File Offset: 0x001247E1
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

	// Token: 0x060017BC RID: 6076 RVA: 0x001265F8 File Offset: 0x001247F8
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

	// Token: 0x060017BD RID: 6077 RVA: 0x001266E2 File Offset: 0x001248E2
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

	// Token: 0x060017BE RID: 6078 RVA: 0x001266F1 File Offset: 0x001248F1
	private void Start()
	{
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x001266F3 File Offset: 0x001248F3
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

	// Token: 0x060017C0 RID: 6080 RVA: 0x00126730 File Offset: 0x00124930
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

	// Token: 0x060017C1 RID: 6081 RVA: 0x00126CCC File Offset: 0x00124ECC
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

	// Token: 0x060017C2 RID: 6082 RVA: 0x00126D21 File Offset: 0x00124F21
	private void OnClickEventInfoBanner(Transform tf)
	{
		QuestUtil.OpenBannerWebViewWindow(this.setupParam.eventData.eventId);
	}

	// Token: 0x04001278 RID: 4728
	private int normalIndex;

	// Token: 0x04001279 RID: 4729
	private static readonly int HardIndex = 1;

	// Token: 0x0400127A RID: 4730
	private static readonly int ExtraIndex = 2;

	// Token: 0x0400127B RID: 4731
	private static readonly int OtherStoryIndex = 3;

	// Token: 0x0400127D RID: 4733
	private SelEventScenarioCtrl.InitParam initParam = new SelEventScenarioCtrl.InitParam();

	// Token: 0x0400127E RID: 4734
	private SelEventScenarioCtrl.SetupParam setupParam = new SelEventScenarioCtrl.SetupParam();

	// Token: 0x0400127F RID: 4735
	private GameObject prefabObj;

	// Token: 0x04001283 RID: 4739
	private Coroutine createGui;

	// Token: 0x04001284 RID: 4740
	private IEnumerator animateBg;

	// Token: 0x04001285 RID: 4741
	private IEnumerator initBg;

	// Token: 0x04001286 RID: 4742
	private bool isInitUpdate;

	// Token: 0x04001287 RID: 4743
	private bool isRecreateUpdate;

	// Token: 0x02000D08 RID: 3336
	public class InitParam
	{
		// Token: 0x04004D3A RID: 19770
		public UnityAction reqNextSequenceCB;

		// Token: 0x04004D3B RID: 19771
		public UnityAction reqBackSequenceCB;

		// Token: 0x04004D3C RID: 19772
		public UnityAction reqShopSequenceCB;

		// Token: 0x04004D3D RID: 19773
		public UnityAction selectObjsCB;
	}

	// Token: 0x02000D09 RID: 3337
	public class SetupParam
	{
		// Token: 0x04004D3E RID: 19774
		public DataManagerEvent.EventData eventData;

		// Token: 0x04004D3F RID: 19775
		public UnityAction reqNextSequenceCB;
	}

	// Token: 0x02000D0A RID: 3338
	public class GUI
	{
		// Token: 0x060047F7 RID: 18423 RVA: 0x0021A328 File Offset: 0x00218528
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

		// Token: 0x04004D40 RID: 19776
		public SelEventScenarioCtrl.GUI.EventSelect eventSelect;

		// Token: 0x04004D41 RID: 19777
		public List<SelEventScenarioCtrl.GUI.EventInfo> eventInfo;

		// Token: 0x020011BF RID: 4543
		public class EventSelect
		{
			// Token: 0x06005705 RID: 22277 RVA: 0x0025551C File Offset: 0x0025371C
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

			// Token: 0x06005706 RID: 22278 RVA: 0x002556AC File Offset: 0x002538AC
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

			// Token: 0x04006155 RID: 24917
			public GameObject baseObj;

			// Token: 0x04006156 RID: 24918
			public PguiButtonCtrl Btn_Main;

			// Token: 0x04006157 RID: 24919
			public PguiButtonCtrl Btn_Hard;

			// Token: 0x04006158 RID: 24920
			public MarkLockCtrl Btn_HardMarkLockCtrl;

			// Token: 0x04006159 RID: 24921
			public PguiButtonCtrl Btn_Extra;

			// Token: 0x0400615A RID: 24922
			public MarkLockCtrl Btn_ExtraMarkLockCtrl;

			// Token: 0x0400615B RID: 24923
			public PguiRawImageCtrl Tex_Event_Banner;

			// Token: 0x0400615C RID: 24924
			public PguiImageCtrl Tex_EventDay;

			// Token: 0x0400615D RID: 24925
			public PguiRawImageCtrl Tex_Photo;

			// Token: 0x0400615E RID: 24926
			public PguiTextCtrl Num_Txt_Day;

			// Token: 0x0400615F RID: 24927
			public RenderTextureChara renderTextureChara;

			// Token: 0x04006160 RID: 24928
			public PguiButtonCtrl Btn_Shop;

			// Token: 0x04006161 RID: 24929
			public PguiButtonCtrl Btn_Mission;

			// Token: 0x04006162 RID: 24930
			public PguiTextCtrl Txt_Mission_Num;

			// Token: 0x04006163 RID: 24931
			public PguiButtonCtrl Btn_Yaji_Left;

			// Token: 0x04006164 RID: 24932
			public PguiButtonCtrl Btn_Yaji_Right;

			// Token: 0x04006165 RID: 24933
			public PguiTextCtrl Txt_Title;

			// Token: 0x04006166 RID: 24934
			public PguiRawImageCtrl Tex_BG;
		}

		// Token: 0x020011C0 RID: 4544
		public class EventInfo
		{
			// Token: 0x06005707 RID: 22279 RVA: 0x00255742 File Offset: 0x00253942
			public EventInfo(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Img_ItemIcon = baseTr.Find("Icon_Stone").GetComponent<PguiRawImageCtrl>();
				this.Txt_ItemNum = baseTr.Find("Num_Txt").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x04006167 RID: 24935
			public GameObject baseObj;

			// Token: 0x04006168 RID: 24936
			public PguiRawImageCtrl Img_ItemIcon;

			// Token: 0x04006169 RID: 24937
			public PguiTextCtrl Txt_ItemNum;
		}
	}
}
