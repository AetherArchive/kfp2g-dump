using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

public class SelEventWildReleaseCtrl : MonoBehaviour
{
	public SelEventWildReleaseCtrl.GUI GuiData { get; private set; }

	private string LoadAssetPath { get; set; }

	public QuestUtil.SelectData SelectData { private get; set; }

	public void Init(SelEventWildReleaseCtrl.InitParam _initParam, SelEventWildReleaseCtrl.SetupParam _setupParam)
	{
		this.createGui = null;
		this.initParam = _initParam;
		this.Setup(_setupParam);
	}

	public void Setup(SelEventWildReleaseCtrl.SetupParam _setupParam)
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

	public void Dest()
	{
		this.SetActiveQuestSelect(false);
	}

	public void Destroy()
	{
	}

	public void SetActiveQuestSelect(bool sw)
	{
		if (this.GuiData == null)
		{
			return;
		}
		this.GuiData.questSelect.baseObj.SetActive(sw);
	}

	public static void PlayBGM()
	{
		SoundManager.PlayBGM("prd_bgm0077");
	}

	public static void OpenHelpWindow()
	{
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "イベントの遊びかた", new List<string> { "Texture2D/Tutorial_Window/Event_Release/tutorial_releaseevent_01", "Texture2D/Tutorial_Window/Event_Release/tutorial_releaseevent_02" }, null);
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

	private void Update()
	{
	}

	private SelEventWildReleaseCtrl.InitParam initParam = new SelEventWildReleaseCtrl.InitParam();

	private SelEventWildReleaseCtrl.SetupParam setupParam = new SelEventWildReleaseCtrl.SetupParam();

	private Coroutine createGui;

	public class InitParam
	{
		public UnityAction reqNextSequenceCB;

		public UnityAction reqShopSequenceCB;

		public GameObject chapterObject;
	}

	public class SetupParam
	{
		public DataManagerEvent.EventData eventData;
	}

	public class GUI
	{
		public GUI(Transform questBaseTr)
		{
			this.questSelect = new SelEventWildReleaseCtrl.GUI.QuestSelect(questBaseTr);
		}

		public SelEventWildReleaseCtrl.GUI.QuestSelect questSelect;

		public class QuestSelect
		{
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

			public GameObject baseObj;

			public PguiButtonCtrl Btn_ShopEvent;

			public List<IconCharaCtrl> Icon_Chara_List;

			public PguiRawImageCtrl Banner;

			public List<QuestUtil.ItemOwnBase> itemOwnBases;
		}
	}
}
