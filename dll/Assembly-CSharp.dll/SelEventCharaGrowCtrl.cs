using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000172 RID: 370
public class SelEventCharaGrowCtrl : MonoBehaviour
{
	// Token: 0x1700039D RID: 925
	// (get) Token: 0x0600172F RID: 5935 RVA: 0x0012279E File Offset: 0x0012099E
	// (set) Token: 0x06001730 RID: 5936 RVA: 0x001227A6 File Offset: 0x001209A6
	public SelEventCharaGrowCtrl.GUI GuiData { get; private set; }

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06001732 RID: 5938 RVA: 0x001227B8 File Offset: 0x001209B8
	// (set) Token: 0x06001731 RID: 5937 RVA: 0x001227AF File Offset: 0x001209AF
	private bool OpenedNoticeTutorialWindow { get; set; }

	// Token: 0x06001733 RID: 5939 RVA: 0x001227C0 File Offset: 0x001209C0
	public static bool IsEnableButtonChange(int eventId)
	{
		return DataManager.DmEvent.GetEventData(eventId) != null && SelEventCharaGrowCtrl.CanChangeChara(eventId);
	}

	// Token: 0x06001734 RID: 5940 RVA: 0x001227D8 File Offset: 0x001209D8
	public static bool IsNextSequenceCharaSelect(int eventId)
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(eventId);
		return eventData != null && eventData.SelectGrowthCharaData.Id == 0;
	}

	// Token: 0x06001735 RID: 5941 RVA: 0x00122804 File Offset: 0x00120A04
	public void Init(SelEventCharaGrowCtrl.InitParam _initParam, SelEventCharaGrowCtrl.SetupParam _setupParam)
	{
		this.OpenedNoticeTutorialWindow = false;
		this.initParam = _initParam;
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneEvent/GUI/Prefab/GUI_Event_Gorw_CharaSelect"), base.transform);
		GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneEvent/GUI/Prefab/Quest_ChapterLeft_EventGrow"), this.initParam.chapterObject.transform);
		this.GuiData = new SelEventCharaGrowCtrl.GUI(gameObject.transform, gameObject2.transform);
		this.Setup(_setupParam);
		ReuseScroll scrollView = this.GuiData.charaSelect.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartCharaSelect));
		ReuseScroll scrollView2 = this.GuiData.charaSelect.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateCharaSelect));
		this.GuiData.charaSelect.ScrollView.Setup(10, 0);
		this.GuiData.chapterSelect.Btn_Change.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickChapterButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.GuiData.chapterSelect.Btn_ShopEvent.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickChapterButton), PguiButtonCtrl.SoundType.DEFAULT);
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.CHARA_EVENT_GROW,
			filterButton = this.GuiData.charaSelect.Btn_FilterOnOff,
			sortButton = this.GuiData.charaSelect.Btn_Sort,
			sortUdButton = this.GuiData.charaSelect.Btn_SortUpDown,
			funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
			{
				charaList = this.haveCharaPackList
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispCharaPackList = item.charaList;
				this.sortType = item.sortType;
				this.GuiData.charaSelect.ScrollView.Resize(1 + this.dispCharaPackList.Count / SelEventCharaGrowCtrl.CharaSelectGUI.SCROLL_ITEM_NUN_H, 0);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
	}

	// Token: 0x06001736 RID: 5942 RVA: 0x001229BC File Offset: 0x00120BBC
	public void Setup(SelEventCharaGrowCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		this.haveCharaPackList = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
		this.dispCharaPackList = new List<CharaPackData>(this.haveCharaPackList);
		this.GuiData.charaSelect.ScrollView.Resize(1 + this.dispCharaPackList.Count / SelEventCharaGrowCtrl.CharaSelectGUI.SCROLL_ITEM_NUN_H, 0);
		if (this.actionCoroutine != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.actionCoroutine);
		}
		this.actionCoroutine = null;
		this.growCharaBonus = null;
		HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(this.setupParam.eventData.eventBannerId);
		if (homeBannerData != null)
		{
			this.GuiData.chapterSelect.Banner.banner = homeBannerData.bannerImagePathEvent;
		}
		PrjUtil.AddTouchEventTrigger(this.GuiData.chapterSelect.Banner.gameObject, new UnityAction<Transform>(this.OnClickEventInfoBanner));
		for (int i = 0; i < this.GuiData.chapterSelect.Icon_Chara_List.Count; i++)
		{
			IconCharaCtrl iconCharaCtrl = this.GuiData.chapterSelect.Icon_Chara_List[i];
			bool flag = i < this.setupParam.eventData.GrowthCharaList.Count;
			iconCharaCtrl.transform.parent.gameObject.SetActive(flag);
			if (flag)
			{
				DataManagerEvent.EventData.Bonus bonus = this.setupParam.eventData.GrowthCharaList[i];
				iconCharaCtrl.SetupPrm(new IconCharaCtrl.SetupParam
				{
					cpd = CharaPackData.MakeInitial(bonus.Id),
					eventId = this.setupParam.eventData.eventId,
					iconId = 0
				});
				iconCharaCtrl.DispLevel(false);
				iconCharaCtrl.DispRarity(false);
				iconCharaCtrl.DispWakeUp(false);
			}
		}
		string npcname = DataManagerChara.GetNPCName(this.setupParam.eventData.dispCharaId);
		if (npcname != null)
		{
			this.GuiData.charaSelect.Txt_CharaName.text = npcname;
		}
		else
		{
			CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(this.setupParam.eventData.dispCharaId);
			if (charaStaticData != null)
			{
				this.GuiData.charaSelect.Txt_CharaName.text = charaStaticData.GetName();
			}
		}
		this.GuiData.charaSelect.Txt_Serif.text = "一緒に頑張るフレンズさんは誰にしますか？\nあとで変更もできますよ！";
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.CHARA_EVENT_GROW, null);
	}

	// Token: 0x06001737 RID: 5943 RVA: 0x00122C16 File Offset: 0x00120E16
	public static void OpenHelpWindow()
	{
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "イベントの遊びかた", new List<string> { "Texture2D/Tutorial_Window/Event_Grow/tutorial_eventgrow_01", "Texture2D/Tutorial_Window/Event_Grow/tutorial_eventgrow_02", "Texture2D/Tutorial_Window/Event_Grow/tutorial_eventgrow_03" }, null);
	}

	// Token: 0x06001738 RID: 5944 RVA: 0x00122C50 File Offset: 0x00120E50
	public void RequestTutorialWindow()
	{
		List<DataManagerEvent.ReleaseEffects> releaseEffectsList = DataManager.DmEvent.GetReleaseEffectsList();
		DataManagerEvent.ReleaseEffects releaseEffects = null;
		QuestUtil.GetEnableEventReleaseEffects(ref releaseEffectsList, ref releaseEffects, this.setupParam.eventData);
		if (!this.OpenedNoticeTutorialWindow && SelEventCharaGrowCtrl.IsResettable(this.setupParam.eventData.eventId))
		{
			releaseEffects.TutorialPhase &= -5;
			this.OpenedNoticeTutorialWindow = true;
		}
		if ((releaseEffects.TutorialPhase & 1) == 0)
		{
			SelEventCharaGrowCtrl.OpenHelpWindow();
			releaseEffects.TutorialPhase |= 1;
			DataManager.DmEvent.RequestUpdateReleaseEffects(releaseEffectsList);
			return;
		}
		if (SelEventCharaGrowCtrl.IsResettable(this.setupParam.eventData.eventId) && (releaseEffects.TutorialPhase & 4) == 0)
		{
			CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, SelEventCharaGrowCtrl.TutorialWindowTitle, new List<string> { "Texture2D/Tutorial_Window/Event_Grow/tutorial_eventgrow_05" }, null);
			releaseEffects.TutorialPhase |= 4;
			DataManager.DmEvent.RequestUpdateReleaseEffects(releaseEffectsList);
		}
	}

	// Token: 0x06001739 RID: 5945 RVA: 0x00122D38 File Offset: 0x00120F38
	public void Dest()
	{
		this.SetActiveChapterSelect(false);
		if (this.GuiData.charaSelect.renderTextureChara != null)
		{
			Object.Destroy(this.GuiData.charaSelect.renderTextureChara.gameObject);
		}
		this.GuiData.charaSelect.renderTextureChara = null;
	}

	// Token: 0x0600173A RID: 5946 RVA: 0x00122D8F File Offset: 0x00120F8F
	public void Destroy()
	{
	}

	// Token: 0x0600173B RID: 5947 RVA: 0x00122D91 File Offset: 0x00120F91
	public void SetEnableChangeButton(bool sw)
	{
		this.GuiData.chapterSelect.Btn_Change.gameObject.SetActive(sw);
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x00122DB0 File Offset: 0x00120FB0
	public void UpdateDecoration()
	{
		CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByCharaGrow(true);
		if (this.GuiData.charaSelect.renderTextureChara == null)
		{
			this.GuiData.charaSelect.renderTextureChara = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.GuiData.charaSelect.baseObj.transform.Find("Left/CharSerif").transform).GetComponent<RenderTextureChara>();
			this.GuiData.charaSelect.renderTextureChara.transform.SetSiblingIndex(0);
			this.GuiData.charaSelect.renderTextureChara.postion = new Vector2(100f, 50f);
			this.GuiData.charaSelect.renderTextureChara.fieldOfView = 18f;
			this.GuiData.charaSelect.renderTextureChara.Setup(this.setupParam.eventData.dispCharaId, 0, CharaMotionDefine.ActKey.SCENARIO_STAND_BY, 0, false, true, null, false, null, 0f, null, false, false, false);
		}
		for (int i = 0; i < this.GuiData.chapterSelect.itemOwnBases.Count; i++)
		{
			QuestUtil.ItemOwnBase itemOwnBase = this.GuiData.chapterSelect.itemOwnBases[i];
			itemOwnBase.baseObj.SetActive(i < this.setupParam.eventData.eventCoinIdList.Count);
			if (itemOwnBase.baseObj.activeSelf)
			{
				itemOwnBase.Num_Txt.text = DataManager.DmItem.GetUserItemData(this.setupParam.eventData.eventCoinIdList[i]).num.ToString();
				itemOwnBase.Icon_Stone.SetRawImage(DataManager.DmItem.GetItemStaticBase(this.setupParam.eventData.eventCoinIdList[i]).GetIconName(), true, false, null);
			}
		}
		CanvasManager.SetBgObj(QuestUtil.EVENT_BG);
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x00122FA4 File Offset: 0x001211A4
	public void OpenAttentionTurotialWindow()
	{
		List<DataManagerEvent.ReleaseEffects> releaseEffectsList = DataManager.DmEvent.GetReleaseEffectsList();
		DataManagerEvent.ReleaseEffects releaseEffects = null;
		QuestUtil.GetEnableEventReleaseEffects(ref releaseEffectsList, ref releaseEffects, this.setupParam.eventData);
		if ((releaseEffects.TutorialPhase & 2) == 0)
		{
			CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, SelEventCharaGrowCtrl.TutorialWindowTitle, new List<string> { "Texture2D/Tutorial_Window/Event_Grow/tutorial_eventgrow_06" }, null);
			releaseEffects.TutorialPhase |= 2;
			DataManager.DmEvent.RequestUpdateReleaseEffects(releaseEffectsList);
		}
	}

	// Token: 0x0600173E RID: 5950 RVA: 0x00123018 File Offset: 0x00121218
	public void SetActiveChapterSelect(bool sw)
	{
		this.GuiData.chapterSelect.baseObj.SetActive(sw);
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.setupParam.eventData.eventId);
		if (eventData == null)
		{
			return;
		}
		if (sw && eventData.SelectGrowthCharaData.Id != 0)
		{
			this.GuiData.chapterSelect.Icon_Chara.SetupPrm(new IconCharaCtrl.SetupParam
			{
				cpd = CharaPackData.MakeInitial(eventData.SelectGrowthCharaData.Id),
				eventId = this.setupParam.eventData.eventId,
				iconId = 0
			});
			this.GuiData.chapterSelect.Icon_Chara.DispLevel(false);
			this.GuiData.chapterSelect.Icon_Chara.DispRarity(false);
			this.GuiData.chapterSelect.Icon_Chara.DispWakeUp(false);
		}
		this.SetEnableChangeButton(SelEventCharaGrowCtrl.IsEnableButtonChange(eventData.eventId));
	}

	// Token: 0x0600173F RID: 5951 RVA: 0x00123110 File Offset: 0x00121310
	public static bool IsResettable(int eventId)
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(eventId);
		if (eventData == null)
		{
			return false;
		}
		DateTime questClearDefaultDateTime = QuestUtil.QuestClearDefaultDateTime;
		DataManagerEvent.PeriodData latestPeriodDataList = DataManager.DmEvent.GetLatestPeriodDataList(eventId);
		bool flag = true;
		if (latestPeriodDataList == null)
		{
			flag = false;
		}
		else if (!(questClearDefaultDateTime == eventData.SelectGrowthCharaData.questClearDatetime) && !(eventData.SelectGrowthCharaData.charaSelectDatetime < latestPeriodDataList.StartDatetime) && !(eventData.SelectGrowthCharaData.questClearDatetime < latestPeriodDataList.StartDatetime))
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x06001740 RID: 5952 RVA: 0x00123190 File Offset: 0x00121390
	private static bool CanChangeChara(int eventId)
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(eventId);
		if (eventData == null)
		{
			return false;
		}
		DateTime questClearDefaultDateTime = QuestUtil.QuestClearDefaultDateTime;
		DataManagerEvent.PeriodData latestPeriodDataList = DataManager.DmEvent.GetLatestPeriodDataList(eventId);
		bool flag = true;
		if (latestPeriodDataList == null)
		{
			if (!(questClearDefaultDateTime == eventData.SelectGrowthCharaData.questClearDatetime))
			{
				flag = false;
			}
		}
		else
		{
			flag = SelEventCharaGrowCtrl.IsResettable(eventId);
		}
		return flag;
	}

	// Token: 0x06001741 RID: 5953 RVA: 0x001231E4 File Offset: 0x001213E4
	private IEnumerator SelectChara(int charaId)
	{
		this.GuiData.charaSelect.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
		});
		DataManager.DmEvent.RequestSelectGrowthEventCharaId(this.setupParam.eventData.eventId, charaId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (this.setupParam.eventData.SelectGrowthCharaData.Id == 0)
		{
			DataManager.DmEvent.RequestGetGrowthEventCharaId(this.setupParam.eventData.eventId);
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		while (this.GuiData.charaSelect.anim.ExIsPlaying())
		{
			yield return null;
		}
		while (!this.initParam.onCheckStatusCB())
		{
			yield return null;
		}
		DataManager.DmEvent.GetEventData(this.setupParam.eventData.eventId);
		this.GuiData.charaSelect.baseObj.SetActive(false);
		this.SetActiveChapterSelect(true);
		this.initParam.reqNextSequenceCB();
		yield return new WaitForEndOfFrame();
		while (this.initParam.onWaitEnableCB())
		{
			yield return null;
		}
		while (this.initParam.onPlayAnimCB())
		{
			yield return null;
		}
		this.actionCoroutine = null;
		this.growCharaBonus = null;
		yield break;
	}

	// Token: 0x06001742 RID: 5954 RVA: 0x001231FC File Offset: 0x001213FC
	private void OnStartCharaSelect(int index, GameObject go)
	{
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_Btn_CharaSelect");
		for (int i = 0; i < SelEventCharaGrowCtrl.CharaSelectGUI.SCROLL_ITEM_NUN_H; i++)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, go.transform);
			IconCharaCtrl icon = gameObject2.GetComponent<IconCharaCtrl>();
			gameObject2.name = i.ToString();
			gameObject2.GetComponent<PguiButtonCtrl>().AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				this.OnClickCharaTopButton(icon.charaPackData.id);
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}
	}

	// Token: 0x06001743 RID: 5955 RVA: 0x00123274 File Offset: 0x00121474
	private void OnUpdateCharaSelect(int index, GameObject go)
	{
		for (int i = 0; i < SelEventCharaGrowCtrl.CharaSelectGUI.SCROLL_ITEM_NUN_H; i++)
		{
			int charaIndex = index * SelEventCharaGrowCtrl.CharaSelectGUI.SCROLL_ITEM_NUN_H + i;
			Transform transform = go.transform.Find(i.ToString());
			if (transform)
			{
				IconCharaCtrl component = transform.GetComponent<IconCharaCtrl>();
				if (charaIndex < this.dispCharaPackList.Count)
				{
					component.Setup(this.dispCharaPackList[charaIndex], this.sortType, false, null, 0, -1, 0);
					DataManagerEvent.EventData.Bonus bonus = this.setupParam.eventData.GrowthCharaList.Find((DataManagerEvent.EventData.Bonus item) => item.Id == this.dispCharaPackList[charaIndex].id);
					component.DispEventGrowPickUp(bonus != null);
				}
				else
				{
					transform.GetComponent<IconCharaCtrl>().Setup(null, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
				}
			}
		}
	}

	// Token: 0x06001744 RID: 5956 RVA: 0x0012334C File Offset: 0x0012154C
	private void OnClickCharaTopButton(int charaId)
	{
		this.growCharaBonus = this.setupParam.eventData.GrowthCharaList.Find((DataManagerEvent.EventData.Bonus item) => item.Id == charaId);
		if (this.growCharaBonus != null)
		{
			return;
		}
		if (this.actionCoroutine != null)
		{
			return;
		}
		if (this.GuiData.charaSelect.anim.ExIsPlaying())
		{
			return;
		}
		this.actionCoroutine = Singleton<SceneManager>.Instance.StartCoroutine(this.SelectChara(charaId));
	}

	// Token: 0x06001745 RID: 5957 RVA: 0x001233D4 File Offset: 0x001215D4
	private void OnClickChapterButton(PguiButtonCtrl button)
	{
		if (button == this.GuiData.chapterSelect.Btn_Change)
		{
			this.initParam.reqBackSequenceCB();
			return;
		}
		if (button == this.GuiData.chapterSelect.Btn_ShopEvent)
		{
			this.initParam.reqShopSequenceCB();
		}
	}

	// Token: 0x06001746 RID: 5958 RVA: 0x00123432 File Offset: 0x00121632
	private void OnClickEventInfoBanner(Transform tf)
	{
		QuestUtil.OpenBannerWebViewWindow(this.setupParam.eventData.eventId);
	}

	// Token: 0x04001256 RID: 4694
	private static readonly string TutorialWindowTitle = "隊長さんセレクトについて";

	// Token: 0x04001258 RID: 4696
	private List<CharaPackData> haveCharaPackList;

	// Token: 0x04001259 RID: 4697
	private List<CharaPackData> dispCharaPackList;

	// Token: 0x0400125A RID: 4698
	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	// Token: 0x0400125B RID: 4699
	private SelEventCharaGrowCtrl.InitParam initParam = new SelEventCharaGrowCtrl.InitParam();

	// Token: 0x0400125C RID: 4700
	private SelEventCharaGrowCtrl.SetupParam setupParam = new SelEventCharaGrowCtrl.SetupParam();

	// Token: 0x0400125D RID: 4701
	private Coroutine actionCoroutine;

	// Token: 0x0400125F RID: 4703
	private DataManagerEvent.EventData.Bonus growCharaBonus;

	// Token: 0x02000CD7 RID: 3287
	private enum TutorialType
	{
		// Token: 0x04004CA2 RID: 19618
		HowToPlay,
		// Token: 0x04004CA3 RID: 19619
		Attention,
		// Token: 0x04004CA4 RID: 19620
		Notice
	}

	// Token: 0x02000CD8 RID: 3288
	[EnumFlags]
	private enum TutorialFlags
	{
		// Token: 0x04004CA6 RID: 19622
		HowToPlay = 1,
		// Token: 0x04004CA7 RID: 19623
		Attention,
		// Token: 0x04004CA8 RID: 19624
		Notice = 4
	}

	// Token: 0x02000CD9 RID: 3289
	// (Invoke) Token: 0x0600476F RID: 18287
	public delegate bool OnWaitEnableCB();

	// Token: 0x02000CDA RID: 3290
	// (Invoke) Token: 0x06004773 RID: 18291
	public delegate bool OnPlayAnimCB();

	// Token: 0x02000CDB RID: 3291
	// (Invoke) Token: 0x06004777 RID: 18295
	public delegate bool OnCheckStatusCB();

	// Token: 0x02000CDC RID: 3292
	public class InitParam
	{
		// Token: 0x04004CA9 RID: 19625
		public UnityAction reqNextSequenceCB;

		// Token: 0x04004CAA RID: 19626
		public UnityAction reqBackSequenceCB;

		// Token: 0x04004CAB RID: 19627
		public UnityAction reqShopSequenceCB;

		// Token: 0x04004CAC RID: 19628
		public GameObject chapterObject;

		// Token: 0x04004CAD RID: 19629
		public SelEventCharaGrowCtrl.OnPlayAnimCB onPlayAnimCB;

		// Token: 0x04004CAE RID: 19630
		public SelEventCharaGrowCtrl.OnWaitEnableCB onWaitEnableCB;

		// Token: 0x04004CAF RID: 19631
		public SelEventCharaGrowCtrl.OnCheckStatusCB onCheckStatusCB;
	}

	// Token: 0x02000CDD RID: 3293
	public class SetupParam
	{
		// Token: 0x04004CB0 RID: 19632
		public DataManagerEvent.EventData eventData;
	}

	// Token: 0x02000CDE RID: 3294
	public class GUI
	{
		// Token: 0x0600477C RID: 18300 RVA: 0x0021873C File Offset: 0x0021693C
		public GUI(Transform charaSelectTr, Transform chapterSelectTr)
		{
			this.charaSelect = new SelEventCharaGrowCtrl.CharaSelectGUI(charaSelectTr);
			this.charaSelect.baseObj.SetActive(false);
			if (this.charaSelect.ScrollView.RefScrollRect == null)
			{
				this.charaSelect.ScrollView.InitForce();
			}
			this.chapterSelect = new SelEventCharaGrowCtrl.ChapterSelectGUI(chapterSelectTr);
			this.chapterSelect.baseObj.SetActive(false);
		}

		// Token: 0x04004CB1 RID: 19633
		public SelEventCharaGrowCtrl.CharaSelectGUI charaSelect;

		// Token: 0x04004CB2 RID: 19634
		public SelEventCharaGrowCtrl.ChapterSelectGUI chapterSelect;
	}

	// Token: 0x02000CDF RID: 3295
	public class CharaSelectGUI
	{
		// Token: 0x0600477D RID: 18301 RVA: 0x002187B4 File Offset: 0x002169B4
		public CharaSelectGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_FilterOnOff = baseTr.Find("Right/Window/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.Btn_Sort = baseTr.Find("Right/Window/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SortUpDown = baseTr.Find("Right/Window/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
			this.Txt_Serif = baseTr.Find("Left/CharSerif/Txt_Serif").GetComponent<PguiTextCtrl>();
			this.Txt_Serif.text = "";
			this.Txt_CharaName = baseTr.Find("Left/CharSerif/NameBase/Txt_CharaName").GetComponent<PguiTextCtrl>();
			this.Txt_CharaName.text = "";
			this.anim = baseTr.GetComponent<SimpleAnimation>();
			this.ScrollView = baseTr.Find("Right/Window/ScrollView").GetComponent<ReuseScroll>();
		}

		// Token: 0x04004CB3 RID: 19635
		public static readonly int SCROLL_ITEM_NUN_H = 2;

		// Token: 0x04004CB4 RID: 19636
		public GameObject baseObj;

		// Token: 0x04004CB5 RID: 19637
		public PguiButtonCtrl Btn_FilterOnOff;

		// Token: 0x04004CB6 RID: 19638
		public PguiButtonCtrl Btn_Sort;

		// Token: 0x04004CB7 RID: 19639
		public PguiButtonCtrl Btn_SortUpDown;

		// Token: 0x04004CB8 RID: 19640
		public PguiTextCtrl Txt_Serif;

		// Token: 0x04004CB9 RID: 19641
		public PguiTextCtrl Txt_CharaName;

		// Token: 0x04004CBA RID: 19642
		public SimpleAnimation anim;

		// Token: 0x04004CBB RID: 19643
		public ReuseScroll ScrollView;

		// Token: 0x04004CBC RID: 19644
		public RenderTextureChara renderTextureChara;
	}

	// Token: 0x02000CE0 RID: 3296
	public class ChapterSelectGUI
	{
		// Token: 0x0600477F RID: 18303 RVA: 0x0021888C File Offset: 0x00216A8C
		public ChapterSelectGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Change = baseTr.Find("WindowBase/CharaInfo/Btn_Change").GetComponent<PguiButtonCtrl>();
			this.Btn_ShopEvent = baseTr.Find("WindowBase/Btn_ShopEvent").GetComponent<PguiButtonCtrl>();
			this.Icon_Chara = baseTr.Find("WindowBase/CharaInfo/Icon_Chara_UserSelect/Icon_Chara").GetComponent<IconCharaCtrl>();
			Transform transform = baseTr.Find("WindowBase/CharaInfo/Grid/");
			this.Icon_Chara_List = new List<IconCharaCtrl>
			{
				transform.Find("Icon_Chara01/Icon_Chara").GetComponent<IconCharaCtrl>(),
				transform.Find("Icon_Chara02/Icon_Chara").GetComponent<IconCharaCtrl>(),
				transform.Find("Icon_Chara03/Icon_Chara").GetComponent<IconCharaCtrl>()
			};
			this.Banner = baseTr.Find("WindowBase/Banner").GetComponent<PguiRawImageCtrl>();
			this.itemOwnBases = new List<QuestUtil.ItemOwnBase>
			{
				new QuestUtil.ItemOwnBase(baseTr.Find("WindowBase/Grid/ItemOwnBase01")),
				new QuestUtil.ItemOwnBase(baseTr.Find("WindowBase/Grid/ItemOwnBase02"))
			};
		}

		// Token: 0x04004CBD RID: 19645
		public GameObject baseObj;

		// Token: 0x04004CBE RID: 19646
		public PguiButtonCtrl Btn_Change;

		// Token: 0x04004CBF RID: 19647
		public PguiButtonCtrl Btn_ShopEvent;

		// Token: 0x04004CC0 RID: 19648
		public IconCharaCtrl Icon_Chara;

		// Token: 0x04004CC1 RID: 19649
		public List<IconCharaCtrl> Icon_Chara_List;

		// Token: 0x04004CC2 RID: 19650
		public PguiRawImageCtrl Banner;

		// Token: 0x04004CC3 RID: 19651
		public List<QuestUtil.ItemOwnBase> itemOwnBases;
	}
}
