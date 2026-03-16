using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

public class SelCharaStoryCtrl : MonoBehaviour
{
	public SelCharaStoryCtrl.GUI GuiData { get; private set; }

	public int SelectCharaId { get; set; }

	public List<CharaPackData> HaveCharaPackList { get; set; }

	public List<CharaPackData> DispCharaPackList { get; set; }

	public List<CharaPackData> OriginalDispCharaPackList { get; set; }

	public void Init(SelCharaStoryCtrl.InitParam _initParam, SelCharaStoryCtrl.SetupParam _setupParam)
	{
		this.initParam = _initParam;
		this.HaveCharaPackList = new List<CharaPackData>();
		this.DispCharaPackList = new List<CharaPackData>();
		this.CreateGUI();
		this.Setup(_setupParam);
	}

	public void Setup(SelCharaStoryCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		this.SetupCharaPackList();
		this.SetupCharaQuestSortWindow();
	}

	public void UpdateDecoration()
	{
	}

	public void Dest()
	{
		SelCharaStoryCtrl.GUI guiData = this.GuiData;
	}

	public void Destroy()
	{
	}

	public void SetupCharaQuestSortWindow()
	{
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.CHARA_QUEST_TOP,
			filterButton = this.GuiData.charaSelect.Btn_FilterOnOff,
			sortButton = this.GuiData.charaSelect.Btn_Sort,
			sortUdButton = this.GuiData.charaSelect.Btn_SortUpDown,
			funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
			{
				charaList = this.HaveCharaPackList
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.DispCharaPackList = new List<CharaPackData>(item.charaList);
				this.OriginalDispCharaPackList = new List<CharaPackData>(item.charaList);
				while (this.DispCharaPackList.Count % 3 != 0)
				{
					this.DispCharaPackList.Add(CharaPackData.MakeInvalid());
				}
				this.sortType = item.sortType;
				this.notHaveCharaStaticDataList = CharaUtil.CreateNotHaveCharaStaticDataList(this.HaveCharaPackList);
				CharaUtil.FilterCharaStaticData(ref this.notHaveCharaStaticDataList, SortFilterDefine.RegisterType.CHARA_QUEST_TOP);
				CharaUtil.SortCharaStaticData(ref this.notHaveCharaStaticDataList);
				int num = ((this.notHaveCharaStaticDataList.Count % 3 == 0) ? 1 : 2);
				this.GuiData.charaSelect.ScrollView.Resize(num + (this.DispCharaPackList.Count + this.notHaveCharaStaticDataList.Count) / 3, 0);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
	}

	public void SetupCharaPackList()
	{
		this.HaveCharaPackList.Clear();
		Dictionary<int, CharaPackData> userCharaMap = DataManager.DmChara.GetUserCharaMap();
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.CHARA);
		for (int i = 0; i < playableMapIdList.Count; i++)
		{
			int num = playableMapIdList[i];
			QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap[num];
			if (userCharaMap.ContainsKey(questStaticMap.questCharaId))
			{
				this.HaveCharaPackList.Add(userCharaMap[questStaticMap.questCharaId]);
			}
		}
		this.DispCharaPackList = new List<CharaPackData>(this.HaveCharaPackList);
		this.OriginalDispCharaPackList = new List<CharaPackData>(this.HaveCharaPackList);
		this.notHaveCharaStaticDataList = CharaUtil.CreateNotHaveCharaStaticDataList(this.HaveCharaPackList);
		this.GuiData.charaSelect.Txt_CharaSelect.gameObject.SetActive(this.notHaveCharaStaticDataList.Count > 0);
		CharaUtil.SetupRectTransformOutScreenRange(this.GuiData.charaSelect.Txt_CharaSelect.gameObject, new Vector2(-1000f, 84f));
		this.GuiData.charaSelect.ScrollView.Refresh();
	}

	public static List<QuestStaticMap> GetPlayableMapDataList()
	{
		Dictionary<int, CharaPackData> charaMap = new Dictionary<int, CharaPackData>(DataManager.DmChara.GetUserCharaMap());
		List<CharaStaticData> charaStaticDataList = new List<CharaStaticData>(DataManager.DmChara.CharaStaticDataList);
		charaStaticDataList.RemoveAll((CharaStaticData item) => !charaMap.ContainsKey(item.GetId()) && item.baseData.notOpenDispFlg);
		List<int> list = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.CHARA);
		List<QuestStaticMap> list2 = new List<QuestStaticMap>(DataManager.DmQuest.QuestStaticData.mapDataList);
		list2.RemoveAll((QuestStaticMap item) => !charaStaticDataList.Exists((CharaStaticData item2) => item2.GetId() == item.questCharaId));
		list2.RemoveAll((QuestStaticMap item) => !list.Contains(item.mapId));
		return list2;
	}

	private void CreateGUI()
	{
		this.GuiData = new SelCharaStoryCtrl.GUI(this.initParam.prefabPath);
		this.GuiData.charaSelect.ScrollView.InitForce();
		ReuseScroll scrollView = this.GuiData.charaSelect.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartCharaTop));
		ReuseScroll scrollView2 = this.GuiData.charaSelect.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateCharaTop));
		this.GuiData.charaSelect.ScrollView.Setup(10, 0);
		UnityAction selectObjsCB = this.initParam.selectObjsCB;
		if (selectObjsCB == null)
		{
			return;
		}
		selectObjsCB();
	}

	public void OnStartCharaTop(int index, GameObject go)
	{
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_Btn_CharaSelect");
		for (int i = 0; i < 3; i++)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, go.transform);
			IconCharaCtrl icon = gameObject2.GetComponent<IconCharaCtrl>();
			gameObject2.name = i.ToString();
			gameObject2.GetComponent<PguiButtonCtrl>().AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				this.OnClickCharaQuestTopButton(icon.charaPackData.id);
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}
	}

	public void OnUpdateCharaTop(int index, GameObject go)
	{
		CharaUtil.OnUpdateChara(index, go, this.DispCharaPackList, this.notHaveCharaStaticDataList, this.sortType, this.GuiData.charaSelect.Txt_CharaSelect.gameObject, true);
	}

	private void OnClickCharaQuestTopButton(int charaId)
	{
		QuestUtil.OnCheck isPlayingAnimCB = this.initParam.isPlayingAnimCB;
		if (((isPlayingAnimCB != null) ? new bool?(isPlayingAnimCB()) : null).Value)
		{
			return;
		}
		this.SelectCharaId = charaId;
		this.GuiData.charaSelect.baseObj.SetActive(false);
		QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap item) => item.questCharaId == this.SelectCharaId);
		if (questStaticMap != null)
		{
			UnityAction<QuestStaticMap> reqNextSequenceCB = this.initParam.reqNextSequenceCB;
			if (reqNextSequenceCB == null)
			{
				return;
			}
			reqNextSequenceCB(questStaticMap);
		}
	}

	private SelCharaStoryCtrl.InitParam initParam = new SelCharaStoryCtrl.InitParam();

	private SelCharaStoryCtrl.SetupParam setupParam = new SelCharaStoryCtrl.SetupParam();

	public List<CharaStaticData> notHaveCharaStaticDataList;

	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	public class InitParam
	{
		public UnityAction reqBackSequenceCB;

		public UnityAction selectObjsCB;

		public Transform prefabPath;

		public QuestUtil.OnGetSelectData getSelectDataCB;

		public QuestUtil.OnCheck isPlayingAnimCB;

		public UnityAction<QuestStaticMap> reqNextSequenceCB;
	}

	public class SetupParam
	{
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.charaSelect = new SelCharaStoryCtrl.GUI.CharaSelect(baseTr.transform);
		}

		public SelCharaStoryCtrl.GUI.CharaSelect charaSelect;

		public class CharaSelect
		{
			public CharaSelect(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Btn_FilterOnOff = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
				this.Btn_Sort = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
				this.Btn_SortUpDown = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
				this.ScrollView = baseTr.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
				this.campaignInfo = new QuestUtil.CampaignInfo(baseTr.Find("All/WindowAll/Campaign"));
				this.Txt_CharaSelect = baseTr.Find("All/WindowAll/Txt_CharaSelect").GetComponent<PguiTextCtrl>();
			}

			public void UpdateCampaignInfoCategory(QuestStaticChapter.Category category, int chapterId)
			{
				List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(category, chapterId));
				List<string> list2 = new List<string>(QuestUtil.GetCampaignTimeList(category, chapterId));
				this.campaignInfo.DispCampaign(list, list2);
			}

			public void ResetCampaignInfoCategory()
			{
				this.campaignInfo.ResetCampaign();
			}

			public GameObject baseObj;

			public PguiButtonCtrl Btn_FilterOnOff;

			public PguiButtonCtrl Btn_Sort;

			public PguiButtonCtrl Btn_SortUpDown;

			public ReuseScroll ScrollView;

			public QuestUtil.CampaignInfo campaignInfo;

			public PguiTextCtrl Txt_CharaSelect;
		}
	}
}
