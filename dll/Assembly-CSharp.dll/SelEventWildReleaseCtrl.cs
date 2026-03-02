using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000177 RID: 375
public class SelEventWildReleaseCtrl : MonoBehaviour
{
	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x060017EE RID: 6126 RVA: 0x001279D3 File Offset: 0x00125BD3
	// (set) Token: 0x060017EF RID: 6127 RVA: 0x001279DB File Offset: 0x00125BDB
	public SelEventWildReleaseCtrl.GUI GuiData { get; private set; }

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x060017F0 RID: 6128 RVA: 0x001279E4 File Offset: 0x00125BE4
	// (set) Token: 0x060017F1 RID: 6129 RVA: 0x001279EC File Offset: 0x00125BEC
	private string LoadAssetPath { get; set; }

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x060017F2 RID: 6130 RVA: 0x001279F5 File Offset: 0x00125BF5
	// (set) Token: 0x060017F3 RID: 6131 RVA: 0x001279FD File Offset: 0x00125BFD
	public QuestUtil.SelectData SelectData { private get; set; }

	// Token: 0x060017F4 RID: 6132 RVA: 0x00127A06 File Offset: 0x00125C06
	public void Init(SelEventWildReleaseCtrl.InitParam _initParam, SelEventWildReleaseCtrl.SetupParam _setupParam)
	{
		this.createGui = null;
		this.initParam = _initParam;
		this.Setup(_setupParam);
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x00127A1D File Offset: 0x00125C1D
	public void Setup(SelEventWildReleaseCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		if (this.createGui != null)
		{
			return;
		}
		this.createGui = Singleton<SceneManager>.Instance.StartCoroutine(this.CreateGUI());
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x00127A48 File Offset: 0x00125C48
	public void UpdateDecoration()
	{
		CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByWildRelease(true);
		if (DataManager.DmEvent.GetEventData(this.setupParam.eventData.eventId) == null)
		{
			return;
		}
		HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(this.setupParam.eventData.eventBannerId);
		if (homeBannerData != null)
		{
			this.GuiData.questSelect.Banner.banner = homeBannerData.bannerImagePathEvent;
		}
		PrjUtil.AddTouchEventTrigger(this.GuiData.questSelect.Banner.gameObject, delegate(Transform tr)
		{
			QuestUtil.OpenBannerWebViewWindow(this.setupParam.eventData.eventId);
		});
		for (int i = 0; i < this.GuiData.questSelect.itemOwnBases.Count; i++)
		{
			QuestUtil.ItemOwnBase itemOwnBase = this.GuiData.questSelect.itemOwnBases[i];
			itemOwnBase.baseObj.SetActive(i < this.setupParam.eventData.eventCoinIdList.Count);
			if (itemOwnBase.baseObj.activeSelf)
			{
				itemOwnBase.Num_Txt.text = DataManager.DmItem.GetUserItemData(this.setupParam.eventData.eventCoinIdList[i]).num.ToString();
				itemOwnBase.Icon_Stone.SetRawImage(DataManager.DmItem.GetItemStaticBase(this.setupParam.eventData.eventCoinIdList[i]).GetIconName(), true, false, null);
			}
		}
		CanvasManager.SetScenarioBgInQuestBgTexture(this.setupParam.eventData.bgFilename);
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x00127BC9 File Offset: 0x00125DC9
	public void Dest()
	{
		this.SetActiveQuestSelect(false);
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x00127BD2 File Offset: 0x00125DD2
	public void Destroy()
	{
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x00127BD4 File Offset: 0x00125DD4
	public void SetActiveQuestSelect(bool sw)
	{
		if (this.GuiData == null)
		{
			return;
		}
		this.GuiData.questSelect.baseObj.SetActive(sw);
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x00127BF5 File Offset: 0x00125DF5
	public static void PlayBGM()
	{
		SoundManager.PlayBGM("prd_bgm0077");
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x00127C01 File Offset: 0x00125E01
	public static void OpenHelpWindow()
	{
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "イベントの遊びかた", new List<string> { "Texture2D/Tutorial_Window/Event_Release/tutorial_releaseevent_01", "Texture2D/Tutorial_Window/Event_Release/tutorial_releaseevent_02" }, null);
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x00127C2F File Offset: 0x00125E2F
	private IEnumerator LoadAssetObject(string path)
	{
		AssetManager.LoadAssetData(path, AssetManager.OWNER.QuestSelector, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(path))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x00127C3E File Offset: 0x00125E3E
	private IEnumerator CreateGUI()
	{
		if (this.GuiData == null)
		{
			yield return null;
			this.GuiData = new SelEventWildReleaseCtrl.GUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneEvent/GUI/Prefab/Quest_ChapterLeft_EventYasei"), this.initParam.chapterObject.transform).transform);
			this.GuiData.questSelect.Btn_ShopEvent.AddOnClickListener(delegate(PguiButtonCtrl index)
			{
				this.initParam.reqShopSequenceCB();
			}, PguiButtonCtrl.SoundType.DEFAULT);
			List<DataManagerEvent.ReleaseEffects> releaseEffectsList = DataManager.DmEvent.GetReleaseEffectsList();
			DataManagerEvent.ReleaseEffects releaseEffects = null;
			QuestUtil.GetEnableEventReleaseEffects(ref releaseEffectsList, ref releaseEffects, this.setupParam.eventData);
			if (releaseEffects.TutorialPhase == 0)
			{
				SelEventWildReleaseCtrl.OpenHelpWindow();
				releaseEffects.TutorialPhase = 1;
				DataManager.DmEvent.RequestUpdateReleaseEffects(releaseEffectsList);
			}
		}
		this.GuiData.questSelect.Setup(this.setupParam);
		UnityAction reqNextSequenceCB = this.initParam.reqNextSequenceCB;
		if (reqNextSequenceCB != null)
		{
			reqNextSequenceCB();
		}
		this.createGui = null;
		yield break;
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x00127C4D File Offset: 0x00125E4D
	private void Update()
	{
	}

	// Token: 0x04001296 RID: 4758
	private SelEventWildReleaseCtrl.InitParam initParam = new SelEventWildReleaseCtrl.InitParam();

	// Token: 0x04001297 RID: 4759
	private SelEventWildReleaseCtrl.SetupParam setupParam = new SelEventWildReleaseCtrl.SetupParam();

	// Token: 0x04001299 RID: 4761
	private Coroutine createGui;

	// Token: 0x02000D1E RID: 3358
	public class InitParam
	{
		// Token: 0x04004D7E RID: 19838
		public UnityAction reqNextSequenceCB;

		// Token: 0x04004D7F RID: 19839
		public UnityAction reqShopSequenceCB;

		// Token: 0x04004D80 RID: 19840
		public GameObject chapterObject;
	}

	// Token: 0x02000D1F RID: 3359
	public class SetupParam
	{
		// Token: 0x04004D81 RID: 19841
		public DataManagerEvent.EventData eventData;
	}

	// Token: 0x02000D20 RID: 3360
	public class GUI
	{
		// Token: 0x06004843 RID: 18499 RVA: 0x0021B86D File Offset: 0x00219A6D
		public GUI(Transform questBaseTr)
		{
			this.questSelect = new SelEventWildReleaseCtrl.GUI.QuestSelect(questBaseTr);
		}

		// Token: 0x04004D82 RID: 19842
		public SelEventWildReleaseCtrl.GUI.QuestSelect questSelect;

		// Token: 0x020011C3 RID: 4547
		public class QuestSelect
		{
			// Token: 0x0600570B RID: 22283 RVA: 0x002559A4 File Offset: 0x00253BA4
			public QuestSelect(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Btn_ShopEvent = baseTr.Find("WindowBase/Btn_ShopEvent").GetComponent<PguiButtonCtrl>();
				Transform transform = baseTr.Find("WindowBase/CharaInfo/Grid_Chara");
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

			// Token: 0x0600570C RID: 22284 RVA: 0x00255A80 File Offset: 0x00253C80
			public void Setup(SelEventWildReleaseCtrl.SetupParam setupParam)
			{
				for (int i = 0; i < this.Icon_Chara_List.Count; i++)
				{
					IconCharaCtrl iconCharaCtrl = this.Icon_Chara_List[i];
					bool flag = i < setupParam.eventData.GrowthCharaList.Count;
					iconCharaCtrl.transform.parent.gameObject.SetActive(flag);
					if (flag)
					{
						DataManagerEvent.EventData.Bonus bonus = setupParam.eventData.GrowthCharaList[i];
						iconCharaCtrl.SetupPrm(new IconCharaCtrl.SetupParam
						{
							cpd = CharaPackData.MakeInitial(bonus.Id),
							eventId = setupParam.eventData.eventId,
							iconId = 0
						});
						iconCharaCtrl.DispLevel(false);
						iconCharaCtrl.DispRarity(false);
						iconCharaCtrl.DispWakeUp(false);
					}
				}
			}

			// Token: 0x0400617B RID: 24955
			public GameObject baseObj;

			// Token: 0x0400617C RID: 24956
			public PguiButtonCtrl Btn_ShopEvent;

			// Token: 0x0400617D RID: 24957
			public List<IconCharaCtrl> Icon_Chara_List;

			// Token: 0x0400617E RID: 24958
			public PguiRawImageCtrl Banner;

			// Token: 0x0400617F RID: 24959
			public List<QuestUtil.ItemOwnBase> itemOwnBases;
		}
	}
}
