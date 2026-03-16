using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelEtcetraStoryCtrl : MonoBehaviour
{
	public SelEtcetraStoryCtrl.GUI GuiData { get; private set; }

	public void Init(SelEtcetraStoryCtrl.InitParam _initParam, SelEtcetraStoryCtrl.SetupParam _setupParam)
	{
		this.initParam = _initParam;
		this.CreateGUI();
		this.Setup(_setupParam);
	}

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

	public void Dest()
	{
		if (this.GuiData == null)
		{
			return;
		}
		this.GuiData.mapSelect.baseObj.SetActive(false);
	}

	public void Destroy()
	{
	}

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

	private void Start()
	{
	}

	private void Update()
	{
	}

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

	private SelEtcetraStoryCtrl.InitParam initParam = new SelEtcetraStoryCtrl.InitParam();

	private SelEtcetraStoryCtrl.SetupParam setupParam = new SelEtcetraStoryCtrl.SetupParam();

	private List<SelEtcetraStoryCtrl.GUI.MapSelectParts> mapSelectPartsList = new List<SelEtcetraStoryCtrl.GUI.MapSelectParts>();

	public class InitParam
	{
		public UnityAction reqNextSequenceCB;

		public UnityAction reqBackSequenceCB;

		public UnityAction selectObjsCB;

		public Transform prefabPath;
	}

	public class SetupParam
	{
		public UnityAction reqNextSequenceCB;

		public QuestUtil.SelectData selectData;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.mapSelect = new SelEtcetraStoryCtrl.GUI.MapSelect(baseTr.transform);
		}

		public SelEtcetraStoryCtrl.GUI.MapSelect mapSelect;

		public class MapSelect
		{
			public MapSelect(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.EtceteraQuest_ChapterChange = baseTr.GetComponent<SimpleAnimation>();
				this.ScrollView = baseTr.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
				this.campaignInfo = new QuestUtil.CampaignInfo(baseTr.Find("Campaign"));
			}

			public void UpdateCampaignInfoCategory(int chapterId)
			{
				List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(QuestStaticChapter.Category.ETCETERA, chapterId));
				List<string> list2 = new List<string>(QuestUtil.GetCampaignTimeList(QuestStaticChapter.Category.ETCETERA, chapterId));
				this.campaignInfo.DispCampaign(list, list2);
			}

			public void ResetCampaignInfoCategory()
			{
				this.campaignInfo.ResetCampaign();
			}

			public static readonly int SCROLL_ITEM_COLUMN_MAX = 2;

			public GameObject baseObj;

			public SimpleAnimation EtceteraQuest_ChapterChange;

			public ReuseScroll ScrollView;

			public QuestUtil.CampaignInfo campaignInfo;
		}

		public class MapSelectParts
		{
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

			public GameObject baseObj;

			public PguiButtonCtrl button;

			public SimpleAnimation BaseImage;

			public MarkLockCtrl Mark_Lock;

			public PguiTextCtrl Num_Chapter;

			public PguiTextCtrl Txt_ChapterName;

			public GameObject Mark_New;

			public GameObject MissionInfo;

			public PguiTextCtrl Num_Term01;
		}
	}
}
