using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelSideStoryCtrl : MonoBehaviour
{
	public SelSideStoryCtrl.GUI GuiData { get; private set; }

	public void Init(SelSideStoryCtrl.InitParam _initParam, SelSideStoryCtrl.SetupParam _setupParam)
	{
		this.initParam = _initParam;
		this.CreateGUI();
		this.Setup(_setupParam);
	}

	public void Setup(SelSideStoryCtrl.SetupParam _setupParam)
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
		foreach (SelSideStoryCtrl.GUI.MapSelectParts mapSelectParts in this.mapSelectPartsList)
		{
			mapSelectParts.baseObj.SetActive(false);
		}
		int chapterId = this.setupParam.selectData.chapterId;
		QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId];
		List<QuestStaticMap> list = new List<QuestStaticMap>(questStaticChapter.mapDataList);
		this.GuiData.mapSelect.ScrollView.Resize(list.Count / SelSideStoryCtrl.GUI.MapSelect.SCROLL_ITEM_COLUMN_MAX + 1, 0);
		Singleton<SceneManager>.Instance.StartCoroutine(this.UpdateGrid());
		this.GuiData.mapSelect.Chapter.text = questStaticChapter.chapterName;
		this.UpdateMapButtonLR(chapterId);
		bool flag = chapterId % 2 == 0;
		Color gameObjectById = this.GuiData.mapSelect.Bg_Img_All.GetGameObjectById(flag ? "EvenNum" : "OddNum");
		foreach (PguiImageCtrl pguiImageCtrl in this.GuiData.mapSelect.Bg_Img_All_List)
		{
			pguiImageCtrl.m_Image.color = gameObjectById;
		}
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
		this.GuiData = new SelSideStoryCtrl.GUI(this.initParam.prefabPath);
		this.GuiData.mapSelect.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMapButtonLR), PguiButtonCtrl.SoundType.DEFAULT);
		this.GuiData.mapSelect.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMapButtonLR), PguiButtonCtrl.SoundType.DEFAULT);
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

	private void UpdateMapButtonLR(int chapterId)
	{
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(chapterId - 1);
		List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(chapterId + 1);
		this.GuiData.mapSelect.Btn_Yaji_Left.gameObject.SetActive(playableMapIdList.Count > 0);
		this.GuiData.mapSelect.Btn_Yaji_Right.gameObject.SetActive(playableMapIdList2.Count > 0);
	}

	private IEnumerator UpdateGrid()
	{
		yield return new WaitForEndOfFrame();
		using (List<SelSideStoryCtrl.GUI.MapSelectParts>.Enumerator enumerator = this.mapSelectPartsList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SelSideStoryCtrl.GUI.MapSelectParts mapSelectParts = enumerator.Current;
				mapSelectParts.Num_Chapter.transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
				mapSelectParts.Num_Chapter.transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
			}
			yield break;
		}
		yield break;
	}

	private void OnClickMapButtonLR(PguiButtonCtrl button)
	{
		if (this.GuiData.mapSelect.AraiQuest_ChapterChange.ExIsPlaying())
		{
			return;
		}
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.setupParam.selectData.chapterId - 1);
		List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(this.setupParam.selectData.chapterId + 1);
		int num = this.setupParam.selectData.chapterId;
		int num2 = 0;
		if (this.GuiData.mapSelect.Btn_Yaji_Left == button)
		{
			if (playableMapIdList.Count > 0)
			{
				num--;
			}
		}
		else if (this.GuiData.mapSelect.Btn_Yaji_Right == button && playableMapIdList2.Count > 0)
		{
			num++;
		}
		List<int> playableMapIdList3 = DataManager.DmQuest.GetPlayableMapIdList(num);
		if (playableMapIdList3.Count > 0)
		{
			int num3 = playableMapIdList3.Count - 1;
			num2 = playableMapIdList3[num3];
		}
		this.setupParam.selectData.mapId = num2;
		this.setupParam.selectData.chapterId = num;
		this.UpdateMapButtonLR(this.setupParam.selectData.chapterId);
		SelSideStoryCtrl.SetupParam setupParam = this.setupParam;
		if (setupParam == null)
		{
			return;
		}
		setupParam.reqNextSequenceCB();
	}

	private void OnStartMapSelect(int index, GameObject go)
	{
		GameObject gameObject = (GameObject)Resources.Load("SceneQuest/GUI/Prefab/AraiQuest_ListBar_ChapterChange");
		for (int i = 0; i < SelSideStoryCtrl.GUI.MapSelect.SCROLL_ITEM_COLUMN_MAX; i++)
		{
			SelSideStoryCtrl.GUI.MapSelectParts mapSelectParts = new SelSideStoryCtrl.GUI.MapSelectParts(Object.Instantiate<GameObject>(gameObject, go.transform).transform);
			mapSelectParts.button.AddOnClickListener(delegate(PguiButtonCtrl ptbc)
			{
				int num = this.mapSelectPartsList.FindIndex((SelSideStoryCtrl.GUI.MapSelectParts item) => item.button == ptbc);
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
		list.Sort((QuestStaticMap a, QuestStaticMap b) => a.mapId - b.mapId);
		for (int i = 0; i < SelSideStoryCtrl.GUI.MapSelect.SCROLL_ITEM_COLUMN_MAX; i++)
		{
			int num = index * SelSideStoryCtrl.GUI.MapSelect.SCROLL_ITEM_COLUMN_MAX + i;
			if (num < this.mapSelectPartsList.Count && num < list.Count)
			{
				SelSideStoryCtrl.GUI.MapSelectParts mapSelectParts = this.mapSelectPartsList[num];
				bool flag = num < playableMapIdList.Count;
				mapSelectParts.baseObj.SetActive(true);
				mapSelectParts.button.SetActEnable(flag, true, false);
				List<QuestStaticQuestGroup> questGroupList = list[num].questGroupList;
				questGroupList.Sort((QuestStaticQuestGroup a, QuestStaticQuestGroup b) => a.questGroupId - b.questGroupId);
				mapSelectParts.Txt_ChapterName.text = questGroupList[0].storyName;
				mapSelectParts.Num_Chapter.text = questGroupList[0].titleName + " ";
				mapSelectParts.Mark_NowChapter.gameObject.SetActive(this.setupParam.selectData.mapId == list[num].mapId);
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
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
								num2++;
							}
							if (questDynamicQuestOne.status == QuestOneStatus.COMPLETE)
							{
								num3++;
							}
						}
						num4++;
					}
				}
				mapSelectParts.Mark_New.SetActive(num2 > 0);
				mapSelectParts.MissionInfo.SetActive(num3 == num4);
				List<QuestStaticQuestOne> list2 = new List<QuestStaticQuestOne>(questGroupList[0].questOneList);
				list2.Sort((QuestStaticQuestOne a, QuestStaticQuestOne b) => a.questId - b.questId);
				QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(list2[0].relQuestId);
				mapSelectParts.Mark_Lock.SetActive(!flag);
				string text = ((questOnePackData != null && questOnePackData.questChapter != null) ? SceneQuest.GetMainStoryName(questOnePackData.questChapter.category, true) : "");
				text += ((text != "") ? "\n" : "");
				mapSelectParts.Mark_Lock.SetText(string.Concat(new string[]
				{
					text,
					questOnePackData.questGroup.titleName,
					" ",
					questOnePackData.questOne.questName,
					" クリア"
				}));
			}
		}
	}

	private SelSideStoryCtrl.InitParam initParam = new SelSideStoryCtrl.InitParam();

	private SelSideStoryCtrl.SetupParam setupParam = new SelSideStoryCtrl.SetupParam();

	private List<SelSideStoryCtrl.GUI.MapSelectParts> mapSelectPartsList = new List<SelSideStoryCtrl.GUI.MapSelectParts>();

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
			this.mapSelect = new SelSideStoryCtrl.GUI.MapSelect(baseTr.transform);
		}

		public SelSideStoryCtrl.GUI.MapSelect mapSelect;

		public class MapSelect
		{
			public MapSelect(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Btn_Yaji_Left = baseTr.Find("LeftBtn/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
				this.Btn_Yaji_Right = baseTr.Find("RightBtn/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
				this.Bg_Img_All = baseTr.Find("All/WindowAll/Bg_Img_All").GetComponent<PguiColorCtrl>();
				this.Bg_Img_All_List = new List<PguiImageCtrl>(this.Bg_Img_All.GetComponentsInChildren<PguiImageCtrl>());
				this.Chapter = baseTr.Find("All/WindowAll/Chapter").GetComponent<PguiTextCtrl>();
				this.AraiQuest_ChapterChange = baseTr.GetComponent<SimpleAnimation>();
				this.ScrollView = baseTr.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
				this.campaignInfo = new QuestUtil.CampaignInfo(baseTr.Find("Campaign"));
			}

			public void UpdateCampaignInfoCategory(int chapterId)
			{
				List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(QuestStaticChapter.Category.SIDE_STORY, chapterId));
				List<string> list2 = new List<string>(QuestUtil.GetCampaignTimeList(QuestStaticChapter.Category.SIDE_STORY, chapterId));
				this.campaignInfo.DispCampaign(list, list2);
			}

			public void ResetCampaignInfoCategory()
			{
				this.campaignInfo.ResetCampaign();
			}

			public static readonly int SCROLL_ITEM_COLUMN_MAX = 2;

			public GameObject baseObj;

			public PguiButtonCtrl Btn_Yaji_Left;

			public PguiButtonCtrl Btn_Yaji_Right;

			public PguiColorCtrl Bg_Img_All;

			public List<PguiImageCtrl> Bg_Img_All_List;

			public PguiTextCtrl Chapter;

			public SimpleAnimation AraiQuest_ChapterChange;

			public ReuseScroll ScrollView;

			public QuestUtil.CampaignInfo campaignInfo;
		}

		public class MapSelectParts
		{
			public MapSelectParts(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.button = baseTr.GetComponent<PguiButtonCtrl>();
				this.Mark_NowChapter = baseTr.Find("BaseImage/Mark_NowChapter").GetComponent<PguiImageCtrl>();
				this.BaseImage = baseTr.Find("BaseImage").GetComponent<SimpleAnimation>();
				this.Mark_Lock = baseTr.Find("Mark_Lock").GetComponent<MarkLockCtrl>();
				this.Num_Chapter = baseTr.Find("BaseImage/Txt_TitleGrid/Num_Chapter").GetComponent<PguiTextCtrl>();
				this.Txt_ChapterName = baseTr.Find("BaseImage/Txt_TitleGrid/Txt_ChapterName").GetComponent<PguiTextCtrl>();
				this.Mark_New = baseTr.Find("BaseImage/Mark_New").gameObject;
				this.MissionInfo = baseTr.Find("BaseImage/MissionInfo").gameObject;
			}

			public GameObject baseObj;

			public PguiButtonCtrl button;

			public PguiImageCtrl Mark_NowChapter;

			public SimpleAnimation BaseImage;

			public MarkLockCtrl Mark_Lock;

			public PguiTextCtrl Num_Chapter;

			public PguiTextCtrl Txt_ChapterName;

			public GameObject Mark_New;

			public GameObject MissionInfo;
		}
	}
}
