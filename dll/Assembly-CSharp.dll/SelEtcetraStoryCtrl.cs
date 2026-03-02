using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000171 RID: 369
public class SelEtcetraStoryCtrl : MonoBehaviour
{
	// Token: 0x1700039C RID: 924
	// (get) Token: 0x06001720 RID: 5920 RVA: 0x00121FEA File Offset: 0x001201EA
	// (set) Token: 0x06001721 RID: 5921 RVA: 0x00121FF2 File Offset: 0x001201F2
	public SelEtcetraStoryCtrl.GUI GuiData { get; private set; }

	// Token: 0x06001722 RID: 5922 RVA: 0x00121FFB File Offset: 0x001201FB
	public void Init(SelEtcetraStoryCtrl.InitParam _initParam, SelEtcetraStoryCtrl.SetupParam _setupParam)
	{
		this.initParam = _initParam;
		this.CreateGUI();
		this.Setup(_setupParam);
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x00122011 File Offset: 0x00120211
	public void Setup(SelEtcetraStoryCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		UnityAction reqNextSequenceCB = this.setupParam.reqNextSequenceCB;
		if (reqNextSequenceCB == null)
		{
			return;
		}
		reqNextSequenceCB();
	}

	// Token: 0x06001724 RID: 5924 RVA: 0x00122030 File Offset: 0x00120230
	public void UpdateDecoration()
	{
		this.GuiData.mapSelect.baseObj.SetActive(true);
		foreach (SelEtcetraStoryCtrl.GUI.MapSelectParts mapSelectParts in this.mapSelectPartsList)
		{
			mapSelectParts.baseObj.SetActive(false);
		}
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.setupParam.selectData.questCategory);
		if (this.setupParam.selectData.chapterId == 0 && playableMapIdList.Count > 0)
		{
			QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap[playableMapIdList[0]];
			this.setupParam.selectData.chapterId = questStaticMap.chapterId;
		}
		this.GuiData.mapSelect.ScrollView.Resize(playableMapIdList.Count / SelEtcetraStoryCtrl.GUI.MapSelect.SCROLL_ITEM_COLUMN_MAX + 1, 0);
		Singleton<SceneManager>.Instance.StartCoroutine(this.UpdateGrid());
	}

	// Token: 0x06001725 RID: 5925 RVA: 0x0012213C File Offset: 0x0012033C
	public void Dest()
	{
		if (this.GuiData == null)
		{
			return;
		}
		this.GuiData.mapSelect.baseObj.SetActive(false);
	}

	// Token: 0x06001726 RID: 5926 RVA: 0x0012215D File Offset: 0x0012035D
	public void Destroy()
	{
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x00122160 File Offset: 0x00120360
	private void CreateGUI()
	{
		this.GuiData = new SelEtcetraStoryCtrl.GUI(this.initParam.prefabPath);
		ReuseScroll scrollView = this.GuiData.mapSelect.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartMapSelect));
		ReuseScroll scrollView2 = this.GuiData.mapSelect.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateMapSelect));
		this.GuiData.mapSelect.ScrollView.Setup(10, 0);
		UnityAction selectObjsCB = this.initParam.selectObjsCB;
		if (selectObjsCB == null)
		{
			return;
		}
		selectObjsCB();
	}

	// Token: 0x06001728 RID: 5928 RVA: 0x00122212 File Offset: 0x00120412
	private void Start()
	{
	}

	// Token: 0x06001729 RID: 5929 RVA: 0x00122214 File Offset: 0x00120414
	private void Update()
	{
	}

	// Token: 0x0600172A RID: 5930 RVA: 0x00122216 File Offset: 0x00120416
	private IEnumerator UpdateGrid()
	{
		yield return new WaitForEndOfFrame();
		using (List<SelEtcetraStoryCtrl.GUI.MapSelectParts>.Enumerator enumerator = this.mapSelectPartsList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SelEtcetraStoryCtrl.GUI.MapSelectParts mapSelectParts = enumerator.Current;
				mapSelectParts.Num_Chapter.transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
				mapSelectParts.Num_Chapter.transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x0600172B RID: 5931 RVA: 0x00122228 File Offset: 0x00120428
	private void OnStartMapSelect(int index, GameObject go)
	{
		GameObject gameObject = (GameObject)Resources.Load("SceneQuest/GUI/Prefab/EtceteraQuest_ListBar_ChapterChange");
		for (int i = 0; i < SelEtcetraStoryCtrl.GUI.MapSelect.SCROLL_ITEM_COLUMN_MAX; i++)
		{
			SelEtcetraStoryCtrl.GUI.MapSelectParts mapSelectParts = new SelEtcetraStoryCtrl.GUI.MapSelectParts(Object.Instantiate<GameObject>(gameObject, go.transform).transform);
			mapSelectParts.button.AddOnClickListener(delegate(PguiButtonCtrl ptbc)
			{
				int num = this.mapSelectPartsList.FindIndex((SelEtcetraStoryCtrl.GUI.MapSelectParts item) => item.button == ptbc);
				int chapterId = this.setupParam.selectData.chapterId;
				List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(chapterId);
				if (num < playableMapIdList.Count)
				{
					this.setupParam.selectData.mapId = playableMapIdList[num];
					this.initParam.reqNextSequenceCB();
					return;
				}
				CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
				{
					new CmnReleaseConditionWindowCtrl.SetupParam
					{
						text = this.mapSelectPartsList[num].Mark_Lock.GetText().Replace("S", "シーズン"),
						enableClear = false
					}
				});
			}, PguiButtonCtrl.SoundType.DEFAULT);
			this.mapSelectPartsList.Add(mapSelectParts);
		}
	}

	// Token: 0x0600172C RID: 5932 RVA: 0x00122290 File Offset: 0x00120490
	private void OnUpdateMapSelect(int index, GameObject go)
	{
		int chapterId = this.setupParam.selectData.chapterId;
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(chapterId);
		List<QuestStaticMap> list = new List<QuestStaticMap>(DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId].mapDataList);
		for (int i = 0; i < SelEtcetraStoryCtrl.GUI.MapSelect.SCROLL_ITEM_COLUMN_MAX; i++)
		{
			int mapIndex = index * SelEtcetraStoryCtrl.GUI.MapSelect.SCROLL_ITEM_COLUMN_MAX + i;
			if (mapIndex < this.mapSelectPartsList.Count && mapIndex < playableMapIdList.Count)
			{
				SelEtcetraStoryCtrl.GUI.MapSelectParts mapSelectParts = this.mapSelectPartsList[mapIndex];
				bool flag = mapIndex < playableMapIdList.Count;
				mapSelectParts.baseObj.SetActive(true);
				mapSelectParts.baseObj.name = playableMapIdList[mapIndex].ToString();
				mapSelectParts.button.SetActEnable(flag, true, false);
				QuestStaticMap questStaticMap = list.Find((QuestStaticMap item) => item.mapId == playableMapIdList[mapIndex]);
				List<QuestStaticQuestGroup> questGroupList = questStaticMap.questGroupList;
				questGroupList.Sort((QuestStaticQuestGroup a, QuestStaticQuestGroup b) => a.questGroupId - b.questGroupId);
				mapSelectParts.Txt_ChapterName.text = questGroupList[0].storyName;
				mapSelectParts.Num_Chapter.text = questGroupList[0].titleName + " ";
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				foreach (QuestStaticQuestGroup questStaticQuestGroup in questGroupList)
				{
					foreach (QuestStaticQuestOne questStaticQuestOne in questStaticQuestGroup.questOneList)
					{
						QuestDynamicQuestOne questDynamicQuestOne = null;
						if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questStaticQuestOne.questId))
						{
							questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[questStaticQuestOne.questId];
						}
						if (questDynamicQuestOne != null)
						{
							if (questDynamicQuestOne.status == QuestOneStatus.NEW)
							{
								num++;
							}
							if (questDynamicQuestOne.status == QuestOneStatus.COMPLETE)
							{
								num2++;
							}
						}
						num3++;
					}
				}
				mapSelectParts.Mark_New.SetActive(num > 0);
				mapSelectParts.MissionInfo.SetActive(num2 == num3);
				List<QuestStaticQuestOne> list2 = new List<QuestStaticQuestOne>(questGroupList[0].questOneList);
				list2.Sort((QuestStaticQuestOne a, QuestStaticQuestOne b) => a.questId - b.questId);
				QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(list2[0].relQuestId);
				mapSelectParts.Mark_Lock.SetActive(!flag);
				string text = ((questOnePackData != null && questOnePackData.questChapter != null) ? SceneQuest.GetMainStoryName(questOnePackData.questChapter.category, true) : "");
				text += ((text != "") ? "\n" : "");
				if (questOnePackData != null)
				{
					mapSelectParts.Mark_Lock.SetText(string.Concat(new string[]
					{
						text,
						questOnePackData.questGroup.titleName,
						" ",
						questOnePackData.questOne.questName,
						" クリア"
					}));
				}
				mapSelectParts.Num_Term01.transform.parent.gameObject.SetActive(!questStaticMap.IsInfiniteEndTime);
				mapSelectParts.Num_Term01.text = TimeManager.MakeTimeResidueText(TimeManager.Now, questStaticMap.HighEndTimeByGroup, false, true);
			}
		}
	}

	// Token: 0x04001253 RID: 4691
	private SelEtcetraStoryCtrl.InitParam initParam = new SelEtcetraStoryCtrl.InitParam();

	// Token: 0x04001254 RID: 4692
	private SelEtcetraStoryCtrl.SetupParam setupParam = new SelEtcetraStoryCtrl.SetupParam();

	// Token: 0x04001255 RID: 4693
	private List<SelEtcetraStoryCtrl.GUI.MapSelectParts> mapSelectPartsList = new List<SelEtcetraStoryCtrl.GUI.MapSelectParts>();

	// Token: 0x02000CCF RID: 3279
	public class InitParam
	{
		// Token: 0x04004C90 RID: 19600
		public UnityAction reqNextSequenceCB;

		// Token: 0x04004C91 RID: 19601
		public UnityAction reqBackSequenceCB;

		// Token: 0x04004C92 RID: 19602
		public UnityAction selectObjsCB;

		// Token: 0x04004C93 RID: 19603
		public Transform prefabPath;
	}

	// Token: 0x02000CD0 RID: 3280
	public class SetupParam
	{
		// Token: 0x04004C94 RID: 19604
		public UnityAction reqNextSequenceCB;

		// Token: 0x04004C95 RID: 19605
		public QuestUtil.SelectData selectData;
	}

	// Token: 0x02000CD1 RID: 3281
	public class GUI
	{
		// Token: 0x0600475E RID: 18270 RVA: 0x002185B0 File Offset: 0x002167B0
		public GUI(Transform baseTr)
		{
			this.mapSelect = new SelEtcetraStoryCtrl.GUI.MapSelect(baseTr.transform);
		}

		// Token: 0x04004C96 RID: 19606
		public SelEtcetraStoryCtrl.GUI.MapSelect mapSelect;

		// Token: 0x020011B8 RID: 4536
		public class MapSelect
		{
			// Token: 0x060056F1 RID: 22257 RVA: 0x00254D58 File Offset: 0x00252F58
			public MapSelect(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.EtceteraQuest_ChapterChange = baseTr.GetComponent<SimpleAnimation>();
				this.ScrollView = baseTr.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
				this.campaignInfo = new QuestUtil.CampaignInfo(baseTr.Find("Campaign"));
			}

			// Token: 0x060056F2 RID: 22258 RVA: 0x00254DB0 File Offset: 0x00252FB0
			public void UpdateCampaignInfoCategory(int chapterId)
			{
				List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(QuestStaticChapter.Category.ETCETERA, chapterId));
				List<string> list2 = new List<string>(QuestUtil.GetCampaignTimeList(QuestStaticChapter.Category.ETCETERA, chapterId));
				this.campaignInfo.DispCampaign(list, list2);
			}

			// Token: 0x060056F3 RID: 22259 RVA: 0x00254DE6 File Offset: 0x00252FE6
			public void ResetCampaignInfoCategory()
			{
				this.campaignInfo.ResetCampaign();
			}

			// Token: 0x04006131 RID: 24881
			public static readonly int SCROLL_ITEM_COLUMN_MAX = 2;

			// Token: 0x04006132 RID: 24882
			public GameObject baseObj;

			// Token: 0x04006133 RID: 24883
			public SimpleAnimation EtceteraQuest_ChapterChange;

			// Token: 0x04006134 RID: 24884
			public ReuseScroll ScrollView;

			// Token: 0x04006135 RID: 24885
			public QuestUtil.CampaignInfo campaignInfo;
		}

		// Token: 0x020011B9 RID: 4537
		public class MapSelectParts
		{
			// Token: 0x060056F5 RID: 22261 RVA: 0x00254DFC File Offset: 0x00252FFC
			public MapSelectParts(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.button = baseTr.GetComponent<PguiButtonCtrl>();
				this.BaseImage = baseTr.Find("BaseImage").GetComponent<SimpleAnimation>();
				this.Mark_Lock = baseTr.Find("Mark_Lock").GetComponent<MarkLockCtrl>();
				this.Num_Chapter = baseTr.Find("BaseImage/Txt_TitleGrid/Num_Chapter").GetComponent<PguiTextCtrl>();
				this.Txt_ChapterName = baseTr.Find("BaseImage/Txt_TitleGrid/Txt_ChapterName").GetComponent<PguiTextCtrl>();
				this.Mark_New = baseTr.Find("BaseImage/Mark_New").gameObject;
				this.MissionInfo = baseTr.Find("BaseImage/MissionInfo").gameObject;
				this.Num_Term01 = this.BaseImage.transform.Find("EventTerm/Num_Term01").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x04006136 RID: 24886
			public GameObject baseObj;

			// Token: 0x04006137 RID: 24887
			public PguiButtonCtrl button;

			// Token: 0x04006138 RID: 24888
			public SimpleAnimation BaseImage;

			// Token: 0x04006139 RID: 24889
			public MarkLockCtrl Mark_Lock;

			// Token: 0x0400613A RID: 24890
			public PguiTextCtrl Num_Chapter;

			// Token: 0x0400613B RID: 24891
			public PguiTextCtrl Txt_ChapterName;

			// Token: 0x0400613C RID: 24892
			public GameObject Mark_New;

			// Token: 0x0400613D RID: 24893
			public GameObject MissionInfo;

			// Token: 0x0400613E RID: 24894
			public PguiTextCtrl Num_Term01;
		}
	}
}
