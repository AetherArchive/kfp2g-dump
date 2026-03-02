using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000179 RID: 377
public class SelSideStoryCtrl : MonoBehaviour
{
	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06001825 RID: 6181 RVA: 0x00129750 File Offset: 0x00127950
	// (set) Token: 0x06001826 RID: 6182 RVA: 0x00129758 File Offset: 0x00127958
	public SelSideStoryCtrl.GUI GuiData { get; private set; }

	// Token: 0x06001827 RID: 6183 RVA: 0x00129761 File Offset: 0x00127961
	public void Init(SelSideStoryCtrl.InitParam _initParam, SelSideStoryCtrl.SetupParam _setupParam)
	{
		this.initParam = _initParam;
		this.CreateGUI();
		this.Setup(_setupParam);
	}

	// Token: 0x06001828 RID: 6184 RVA: 0x00129777 File Offset: 0x00127977
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

	// Token: 0x06001829 RID: 6185 RVA: 0x00129798 File Offset: 0x00127998
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

	// Token: 0x0600182A RID: 6186 RVA: 0x00129910 File Offset: 0x00127B10
	public void Dest()
	{
		if (this.GuiData == null)
		{
			return;
		}
		this.GuiData.mapSelect.baseObj.SetActive(false);
	}

	// Token: 0x0600182B RID: 6187 RVA: 0x00129931 File Offset: 0x00127B31
	public void Destroy()
	{
	}

	// Token: 0x0600182C RID: 6188 RVA: 0x00129934 File Offset: 0x00127B34
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

	// Token: 0x0600182D RID: 6189 RVA: 0x00129A2A File Offset: 0x00127C2A
	private void Start()
	{
	}

	// Token: 0x0600182E RID: 6190 RVA: 0x00129A2C File Offset: 0x00127C2C
	private void Update()
	{
	}

	// Token: 0x0600182F RID: 6191 RVA: 0x00129A30 File Offset: 0x00127C30
	private void UpdateMapButtonLR(int chapterId)
	{
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(chapterId - 1);
		List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(chapterId + 1);
		this.GuiData.mapSelect.Btn_Yaji_Left.gameObject.SetActive(playableMapIdList.Count > 0);
		this.GuiData.mapSelect.Btn_Yaji_Right.gameObject.SetActive(playableMapIdList2.Count > 0);
	}

	// Token: 0x06001830 RID: 6192 RVA: 0x00129A9F File Offset: 0x00127C9F
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

	// Token: 0x06001831 RID: 6193 RVA: 0x00129AB0 File Offset: 0x00127CB0
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

	// Token: 0x06001832 RID: 6194 RVA: 0x00129BEC File Offset: 0x00127DEC
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

	// Token: 0x06001833 RID: 6195 RVA: 0x00129C54 File Offset: 0x00127E54
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

	// Token: 0x040012A6 RID: 4774
	private SelSideStoryCtrl.InitParam initParam = new SelSideStoryCtrl.InitParam();

	// Token: 0x040012A7 RID: 4775
	private SelSideStoryCtrl.SetupParam setupParam = new SelSideStoryCtrl.SetupParam();

	// Token: 0x040012A8 RID: 4776
	private List<SelSideStoryCtrl.GUI.MapSelectParts> mapSelectPartsList = new List<SelSideStoryCtrl.GUI.MapSelectParts>();

	// Token: 0x02000D33 RID: 3379
	public class InitParam
	{
		// Token: 0x04004DB1 RID: 19889
		public UnityAction reqNextSequenceCB;

		// Token: 0x04004DB2 RID: 19890
		public UnityAction reqBackSequenceCB;

		// Token: 0x04004DB3 RID: 19891
		public UnityAction selectObjsCB;

		// Token: 0x04004DB4 RID: 19892
		public Transform prefabPath;
	}

	// Token: 0x02000D34 RID: 3380
	public class SetupParam
	{
		// Token: 0x04004DB5 RID: 19893
		public UnityAction reqNextSequenceCB;

		// Token: 0x04004DB6 RID: 19894
		public QuestUtil.SelectData selectData;
	}

	// Token: 0x02000D35 RID: 3381
	public class GUI
	{
		// Token: 0x0600487B RID: 18555 RVA: 0x0021C26F File Offset: 0x0021A46F
		public GUI(Transform baseTr)
		{
			this.mapSelect = new SelSideStoryCtrl.GUI.MapSelect(baseTr.transform);
		}

		// Token: 0x04004DB7 RID: 19895
		public SelSideStoryCtrl.GUI.MapSelect mapSelect;

		// Token: 0x020011C9 RID: 4553
		public class MapSelect
		{
			// Token: 0x0600571B RID: 22299 RVA: 0x00255F74 File Offset: 0x00254174
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

			// Token: 0x0600571C RID: 22300 RVA: 0x0025603C File Offset: 0x0025423C
			public void UpdateCampaignInfoCategory(int chapterId)
			{
				List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(QuestStaticChapter.Category.SIDE_STORY, chapterId));
				List<string> list2 = new List<string>(QuestUtil.GetCampaignTimeList(QuestStaticChapter.Category.SIDE_STORY, chapterId));
				this.campaignInfo.DispCampaign(list, list2);
			}

			// Token: 0x0600571D RID: 22301 RVA: 0x00256070 File Offset: 0x00254270
			public void ResetCampaignInfoCategory()
			{
				this.campaignInfo.ResetCampaign();
			}

			// Token: 0x04006199 RID: 24985
			public static readonly int SCROLL_ITEM_COLUMN_MAX = 2;

			// Token: 0x0400619A RID: 24986
			public GameObject baseObj;

			// Token: 0x0400619B RID: 24987
			public PguiButtonCtrl Btn_Yaji_Left;

			// Token: 0x0400619C RID: 24988
			public PguiButtonCtrl Btn_Yaji_Right;

			// Token: 0x0400619D RID: 24989
			public PguiColorCtrl Bg_Img_All;

			// Token: 0x0400619E RID: 24990
			public List<PguiImageCtrl> Bg_Img_All_List;

			// Token: 0x0400619F RID: 24991
			public PguiTextCtrl Chapter;

			// Token: 0x040061A0 RID: 24992
			public SimpleAnimation AraiQuest_ChapterChange;

			// Token: 0x040061A1 RID: 24993
			public ReuseScroll ScrollView;

			// Token: 0x040061A2 RID: 24994
			public QuestUtil.CampaignInfo campaignInfo;
		}

		// Token: 0x020011CA RID: 4554
		public class MapSelectParts
		{
			// Token: 0x0600571F RID: 22303 RVA: 0x00256088 File Offset: 0x00254288
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

			// Token: 0x040061A3 RID: 24995
			public GameObject baseObj;

			// Token: 0x040061A4 RID: 24996
			public PguiButtonCtrl button;

			// Token: 0x040061A5 RID: 24997
			public PguiImageCtrl Mark_NowChapter;

			// Token: 0x040061A6 RID: 24998
			public SimpleAnimation BaseImage;

			// Token: 0x040061A7 RID: 24999
			public MarkLockCtrl Mark_Lock;

			// Token: 0x040061A8 RID: 25000
			public PguiTextCtrl Num_Chapter;

			// Token: 0x040061A9 RID: 25001
			public PguiTextCtrl Txt_ChapterName;

			// Token: 0x040061AA RID: 25002
			public GameObject Mark_New;

			// Token: 0x040061AB RID: 25003
			public GameObject MissionInfo;
		}
	}
}
