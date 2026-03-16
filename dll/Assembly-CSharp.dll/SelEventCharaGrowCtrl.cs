using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

public class SelEventCharaGrowCtrl : MonoBehaviour
{
	public SelEventCharaGrowCtrl.GUI GuiData { get; private set; }

	private bool OpenedNoticeTutorialWindow { get; set; }

	public static bool IsEnableButtonChange(int eventId)
	{
		return DataManager.DmEvent.GetEventData(eventId) != null && SelEventCharaGrowCtrl.CanChangeChara(eventId);
	}

	public static bool IsNextSequenceCharaSelect(int eventId)
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(eventId);
		return eventData != null && eventData.SelectGrowthCharaData.Id == 0;
	}

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

	public static void OpenHelpWindow()
	{
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "イベントの遊びかた", new List<string> { "Texture2D/Tutorial_Window/Event_Grow/tutorial_eventgrow_01", "Texture2D/Tutorial_Window/Event_Grow/tutorial_eventgrow_02", "Texture2D/Tutorial_Window/Event_Grow/tutorial_eventgrow_03" }, null);
	}

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

	public void Dest()
	{
		this.SetActiveChapterSelect(false);
		if (this.GuiData.charaSelect.renderTextureChara != null)
		{
			Object.Destroy(this.GuiData.charaSelect.renderTextureChara.gameObject);
		}
		this.GuiData.charaSelect.renderTextureChara = null;
	}

	public void Destroy()
	{
	}

	public void SetEnableChangeButton(bool sw)
	{
		this.GuiData.chapterSelect.Btn_Change.gameObject.SetActive(sw);
	}

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

	private void OnClickEventInfoBanner(Transform tf)
	{
		QuestUtil.OpenBannerWebViewWindow(this.setupParam.eventData.eventId);
	}

	private static readonly string TutorialWindowTitle = "隊長さんセレクトについて";

	private List<CharaPackData> haveCharaPackList;

	private List<CharaPackData> dispCharaPackList;

	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	private SelEventCharaGrowCtrl.InitParam initParam = new SelEventCharaGrowCtrl.InitParam();

	private SelEventCharaGrowCtrl.SetupParam setupParam = new SelEventCharaGrowCtrl.SetupParam();

	private Coroutine actionCoroutine;

	private DataManagerEvent.EventData.Bonus growCharaBonus;

	private enum TutorialType
	{
		HowToPlay,
		Attention,
		Notice
	}

	[EnumFlags]
	private enum TutorialFlags
	{
		HowToPlay = 1,
		Attention,
		Notice = 4
	}

	public delegate bool OnWaitEnableCB();

	public delegate bool OnPlayAnimCB();

	public delegate bool OnCheckStatusCB();

	public class InitParam
	{
		public UnityAction reqNextSequenceCB;

		public UnityAction reqBackSequenceCB;

		public UnityAction reqShopSequenceCB;

		public GameObject chapterObject;

		public SelEventCharaGrowCtrl.OnPlayAnimCB onPlayAnimCB;

		public SelEventCharaGrowCtrl.OnWaitEnableCB onWaitEnableCB;

		public SelEventCharaGrowCtrl.OnCheckStatusCB onCheckStatusCB;
	}

	public class SetupParam
	{
		public DataManagerEvent.EventData eventData;
	}

	public class GUI
	{
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

		public SelEventCharaGrowCtrl.CharaSelectGUI charaSelect;

		public SelEventCharaGrowCtrl.ChapterSelectGUI chapterSelect;
	}

	public class CharaSelectGUI
	{
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

		public static readonly int SCROLL_ITEM_NUN_H = 2;

		public GameObject baseObj;

		public PguiButtonCtrl Btn_FilterOnOff;

		public PguiButtonCtrl Btn_Sort;

		public PguiButtonCtrl Btn_SortUpDown;

		public PguiTextCtrl Txt_Serif;

		public PguiTextCtrl Txt_CharaName;

		public SimpleAnimation anim;

		public ReuseScroll ScrollView;

		public RenderTextureChara renderTextureChara;
	}

	public class ChapterSelectGUI
	{
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

		public GameObject baseObj;

		public PguiButtonCtrl Btn_Change;

		public PguiButtonCtrl Btn_ShopEvent;

		public IconCharaCtrl Icon_Chara;

		public List<IconCharaCtrl> Icon_Chara_List;

		public PguiRawImageCtrl Banner;

		public List<QuestUtil.ItemOwnBase> itemOwnBases;
	}
}
