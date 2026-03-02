using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000170 RID: 368
public class SelCharaStoryCtrl : MonoBehaviour
{
	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06001706 RID: 5894 RVA: 0x001219E0 File Offset: 0x0011FBE0
	// (set) Token: 0x06001707 RID: 5895 RVA: 0x001219E8 File Offset: 0x0011FBE8
	public SelCharaStoryCtrl.GUI GuiData { get; private set; }

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06001708 RID: 5896 RVA: 0x001219F1 File Offset: 0x0011FBF1
	// (set) Token: 0x06001709 RID: 5897 RVA: 0x001219F9 File Offset: 0x0011FBF9
	public int SelectCharaId { get; set; }

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x0600170A RID: 5898 RVA: 0x00121A02 File Offset: 0x0011FC02
	// (set) Token: 0x0600170B RID: 5899 RVA: 0x00121A0A File Offset: 0x0011FC0A
	public List<CharaPackData> HaveCharaPackList { get; set; }

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x0600170C RID: 5900 RVA: 0x00121A13 File Offset: 0x0011FC13
	// (set) Token: 0x0600170D RID: 5901 RVA: 0x00121A1B File Offset: 0x0011FC1B
	public List<CharaPackData> DispCharaPackList { get; set; }

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x0600170E RID: 5902 RVA: 0x00121A24 File Offset: 0x0011FC24
	// (set) Token: 0x0600170F RID: 5903 RVA: 0x00121A2C File Offset: 0x0011FC2C
	public List<CharaPackData> OriginalDispCharaPackList { get; set; }

	// Token: 0x06001710 RID: 5904 RVA: 0x00121A35 File Offset: 0x0011FC35
	public void Init(SelCharaStoryCtrl.InitParam _initParam, SelCharaStoryCtrl.SetupParam _setupParam)
	{
		this.initParam = _initParam;
		this.HaveCharaPackList = new List<CharaPackData>();
		this.DispCharaPackList = new List<CharaPackData>();
		this.CreateGUI();
		this.Setup(_setupParam);
	}

	// Token: 0x06001711 RID: 5905 RVA: 0x00121A61 File Offset: 0x0011FC61
	public void Setup(SelCharaStoryCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		this.SetupCharaPackList();
		this.SetupCharaQuestSortWindow();
	}

	// Token: 0x06001712 RID: 5906 RVA: 0x00121A76 File Offset: 0x0011FC76
	public void UpdateDecoration()
	{
	}

	// Token: 0x06001713 RID: 5907 RVA: 0x00121A78 File Offset: 0x0011FC78
	public void Dest()
	{
		SelCharaStoryCtrl.GUI guiData = this.GuiData;
	}

	// Token: 0x06001714 RID: 5908 RVA: 0x00121A81 File Offset: 0x0011FC81
	public void Destroy()
	{
	}

	// Token: 0x06001715 RID: 5909 RVA: 0x00121A84 File Offset: 0x0011FC84
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

	// Token: 0x06001716 RID: 5910 RVA: 0x00121B14 File Offset: 0x0011FD14
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

	// Token: 0x06001717 RID: 5911 RVA: 0x00121C34 File Offset: 0x0011FE34
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

	// Token: 0x06001718 RID: 5912 RVA: 0x00121CD4 File Offset: 0x0011FED4
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

	// Token: 0x06001719 RID: 5913 RVA: 0x00121D9C File Offset: 0x0011FF9C
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

	// Token: 0x0600171A RID: 5914 RVA: 0x00121E10 File Offset: 0x00120010
	public void OnUpdateCharaTop(int index, GameObject go)
	{
		CharaUtil.OnUpdateChara(index, go, this.DispCharaPackList, this.notHaveCharaStaticDataList, this.sortType, this.GuiData.charaSelect.Txt_CharaSelect.gameObject, true);
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x00121E44 File Offset: 0x00120044
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

	// Token: 0x0400124A RID: 4682
	private SelCharaStoryCtrl.InitParam initParam = new SelCharaStoryCtrl.InitParam();

	// Token: 0x0400124B RID: 4683
	private SelCharaStoryCtrl.SetupParam setupParam = new SelCharaStoryCtrl.SetupParam();

	// Token: 0x0400124F RID: 4687
	public List<CharaStaticData> notHaveCharaStaticDataList;

	// Token: 0x04001251 RID: 4689
	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	// Token: 0x02000CC9 RID: 3273
	public class InitParam
	{
		// Token: 0x04004C83 RID: 19587
		public UnityAction reqBackSequenceCB;

		// Token: 0x04004C84 RID: 19588
		public UnityAction selectObjsCB;

		// Token: 0x04004C85 RID: 19589
		public Transform prefabPath;

		// Token: 0x04004C86 RID: 19590
		public QuestUtil.OnGetSelectData getSelectDataCB;

		// Token: 0x04004C87 RID: 19591
		public QuestUtil.OnCheck isPlayingAnimCB;

		// Token: 0x04004C88 RID: 19592
		public UnityAction<QuestStaticMap> reqNextSequenceCB;
	}

	// Token: 0x02000CCA RID: 3274
	public class SetupParam
	{
	}

	// Token: 0x02000CCB RID: 3275
	public class GUI
	{
		// Token: 0x06004753 RID: 18259 RVA: 0x002184CF File Offset: 0x002166CF
		public GUI(Transform baseTr)
		{
			this.charaSelect = new SelCharaStoryCtrl.GUI.CharaSelect(baseTr.transform);
		}

		// Token: 0x04004C89 RID: 19593
		public SelCharaStoryCtrl.GUI.CharaSelect charaSelect;

		// Token: 0x020011B7 RID: 4535
		public class CharaSelect
		{
			// Token: 0x060056EE RID: 22254 RVA: 0x00254C70 File Offset: 0x00252E70
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

			// Token: 0x060056EF RID: 22255 RVA: 0x00254D14 File Offset: 0x00252F14
			public void UpdateCampaignInfoCategory(QuestStaticChapter.Category category, int chapterId)
			{
				List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(category, chapterId));
				List<string> list2 = new List<string>(QuestUtil.GetCampaignTimeList(category, chapterId));
				this.campaignInfo.DispCampaign(list, list2);
			}

			// Token: 0x060056F0 RID: 22256 RVA: 0x00254D48 File Offset: 0x00252F48
			public void ResetCampaignInfoCategory()
			{
				this.campaignInfo.ResetCampaign();
			}

			// Token: 0x0400612A RID: 24874
			public GameObject baseObj;

			// Token: 0x0400612B RID: 24875
			public PguiButtonCtrl Btn_FilterOnOff;

			// Token: 0x0400612C RID: 24876
			public PguiButtonCtrl Btn_Sort;

			// Token: 0x0400612D RID: 24877
			public PguiButtonCtrl Btn_SortUpDown;

			// Token: 0x0400612E RID: 24878
			public ReuseScroll ScrollView;

			// Token: 0x0400612F RID: 24879
			public QuestUtil.CampaignInfo campaignInfo;

			// Token: 0x04006130 RID: 24880
			public PguiTextCtrl Txt_CharaSelect;
		}
	}
}
