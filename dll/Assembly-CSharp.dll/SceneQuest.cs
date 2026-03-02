using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Touch;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

// Token: 0x0200016F RID: 367
public class SceneQuest : BaseScene
{
	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06001610 RID: 5648 RVA: 0x00115895 File Offset: 0x00113A95
	// (set) Token: 0x06001611 RID: 5649 RVA: 0x0011589C File Offset: 0x00113A9C
	public static DateTime TimeStampInScene { get; private set; }

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x06001612 RID: 5650 RVA: 0x001158A4 File Offset: 0x00113AA4
	// (set) Token: 0x06001613 RID: 5651 RVA: 0x001158AC File Offset: 0x00113AAC
	private bool IsPlayingAnim { get; set; }

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06001614 RID: 5652 RVA: 0x001158B5 File Offset: 0x00113AB5
	// (set) Token: 0x06001615 RID: 5653 RVA: 0x001158BD File Offset: 0x00113ABD
	private string LoadAssetPath { get; set; }

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x06001616 RID: 5654 RVA: 0x001158C6 File Offset: 0x00113AC6
	// (set) Token: 0x06001617 RID: 5655 RVA: 0x001158CE File Offset: 0x00113ACE
	private bool TouchMoving { get; set; }

	// Token: 0x06001618 RID: 5656 RVA: 0x001158D8 File Offset: 0x00113AD8
	private void InitEtcetraStory()
	{
		GameObject gameObject = new GameObject();
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.offsetMin = new Vector2(0f, 0f);
		rectTransform.offsetMax = new Vector2(0f, 0f);
		gameObject.name = "SelEtcetraStoryCtrl";
		gameObject.transform.SetParent(this.guiData.basePanel.transform, false);
		this.selEtcetraStoryCtrl = gameObject.AddComponent<SelEtcetraStoryCtrl>();
		this.selEtcetraStoryCtrl.Init(new SelEtcetraStoryCtrl.InitParam
		{
			reqNextSequenceCB = delegate
			{
				this.reqNextSequence = this.guiData.chapterSelect.baseObj;
			},
			reqBackSequenceCB = delegate
			{
				this.reqNextSequence = this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj;
			},
			selectObjsCB = delegate
			{
				this.guiData.selectObjs.Add(this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj);
			},
			prefabPath = this.guiData.basePanel.transform.Find("EtceteraQuest_ChapterChange")
		}, new SelEtcetraStoryCtrl.SetupParam
		{
			reqNextSequenceCB = delegate
			{
				this.reqNextSequence = this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj;
			},
			selectData = this.selectData
		});
		this.guiData.basePanel.transform.Find("EtceteraQuest_ChapterChange").SetParent(this.selEtcetraStoryCtrl.transform, false);
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x00115A2C File Offset: 0x00113C2C
	private void InitCharaQuest()
	{
		GameObject gameObject = new GameObject();
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.offsetMin = new Vector2(0f, 0f);
		rectTransform.offsetMax = new Vector2(0f, 0f);
		gameObject.name = "SelCharaStoryCtrl";
		gameObject.transform.SetParent(this.guiData.basePanel.transform, false);
		this.selCharaStoryCtrl = gameObject.AddComponent<SelCharaStoryCtrl>();
		this.selCharaStoryCtrl.Init(new SelCharaStoryCtrl.InitParam
		{
			reqNextSequenceCB = delegate(QuestStaticMap mapData)
			{
				this.reqNextSequence = this.guiData.chapterSelect.baseObj;
				this.selectData.mapId = mapData.mapId;
				this.selectData.chapterId = mapData.chapterId;
			},
			reqBackSequenceCB = delegate
			{
				this.reqNextSequence = this.selCharaStoryCtrl.GuiData.charaSelect.baseObj;
			},
			selectObjsCB = delegate
			{
				this.guiData.selectObjs.Add(this.selCharaStoryCtrl.GuiData.charaSelect.baseObj);
			},
			prefabPath = this.guiData.basePanel.transform.Find("CharaSelect"),
			getSelectDataCB = () => this.selectData,
			isPlayingAnimCB = () => this.IsPlayingAnim
		}, new SelCharaStoryCtrl.SetupParam());
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x00115B5C File Offset: 0x00113D5C
	private static string GetMainStoryMapPath(QuestStaticChapter.Category category)
	{
		string text = SceneQuest.MainStoryMapPath;
		if (SceneQuest.IsMainStoryPart1(category))
		{
			text = SceneQuest.MainStoryMapPath;
		}
		else if (SceneQuest.IsMainStoryPart1_5(category))
		{
			text = SceneQuest.CellvalMapPath;
		}
		else if (SceneQuest.IsMainStoryPart2(category))
		{
			text = SceneQuest.MainStory2MapPath;
		}
		else if (SceneQuest.IsMainStoryPart3(category))
		{
			text = SceneQuest.MainStory3MapPath;
		}
		return text;
	}

	// Token: 0x0600161B RID: 5659 RVA: 0x00115BB0 File Offset: 0x00113DB0
	public static List<QuestStaticChapter.Category> GetBtnLeftCategoryList(int currentIndex, List<QuestStaticChapter.Category> categoryList)
	{
		List<QuestStaticChapter.Category> list = new List<QuestStaticChapter.Category>();
		int num = ((currentIndex >= categoryList.Count) ? (categoryList.Count - 1) : currentIndex);
		for (int i = 0; i < num; i++)
		{
			list.Add(categoryList[i]);
		}
		return list;
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x00115BF4 File Offset: 0x00113DF4
	public static List<QuestStaticChapter.Category> GetBtnRightCategoryList(int currentIndex, List<QuestStaticChapter.Category> categoryList)
	{
		List<QuestStaticChapter.Category> list = new List<QuestStaticChapter.Category>();
		for (int i = ((currentIndex >= categoryList.Count) ? (categoryList.Count - 1) : (currentIndex + 1)); i < categoryList.Count; i++)
		{
			list.Add(categoryList[i]);
		}
		return list;
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x00115C3C File Offset: 0x00113E3C
	public static List<GameObject> CreateCarObjList(GameObject go)
	{
		return new List<GameObject>
		{
			Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/Quest_MapCar"), go.transform),
			Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/Quest_MapCar01"), go.transform),
			Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/Quest_MapCellvall"), go.transform),
			Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/Quest_MapHand"), go.transform),
			Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/Quest_MapCamera"), go.transform)
		};
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x00115CF0 File Offset: 0x00113EF0
	private bool IsNotNullEventCtrl(DataManagerEvent.Category category)
	{
		switch (category)
		{
		case DataManagerEvent.Category.Growth:
			return this.IsNotNullEventCharaGrowObj();
		case DataManagerEvent.Category.Large:
			return this.IsNotNullEventLargeScaleObj();
		case DataManagerEvent.Category.Tower:
			return this.IsNotNullEventTowerObj();
		case DataManagerEvent.Category.Coop:
			return this.IsNotNullEventCoopObj();
		case DataManagerEvent.Category.WildRelease:
			return this.IsNotNullEventWildReleaseObj();
		}
		return false;
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x00115D43 File Offset: 0x00113F43
	private bool IsNotNullEventScenarioObj()
	{
		return this.selEventScenarioCtrl != null && this.selEventScenarioCtrl.GuiData != null;
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x00115D63 File Offset: 0x00113F63
	private bool IsNotNullEventLargeScaleObj()
	{
		return this.selEventLargeScaleCtrl != null && this.selEventLargeScaleCtrl.GuiData != null;
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x00115D83 File Offset: 0x00113F83
	private bool IsNotNullEventCharaGrowObj()
	{
		return this.selEventCharaGrowCtrl != null && this.selEventCharaGrowCtrl.GuiData != null;
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x00115DA3 File Offset: 0x00113FA3
	private bool IsNotNullEventTowerObj()
	{
		return this.selEventTowerCtrl != null && this.selEventTowerCtrl.GuiData != null;
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x00115DC3 File Offset: 0x00113FC3
	private bool IsNotNullSideStoryObj()
	{
		return this.selSideStoryCtrl != null && this.selSideStoryCtrl.GuiData != null;
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x00115DE3 File Offset: 0x00113FE3
	private bool IsNotNullEtcetraStoryObj()
	{
		return this.selEtcetraStoryCtrl != null && this.selEtcetraStoryCtrl.GuiData != null;
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x00115E03 File Offset: 0x00114003
	private bool IsNotNullMainStoryObj()
	{
		return this.selMainStoryCtrl != null && this.selMainStoryCtrl.GuiData != null;
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x00115E23 File Offset: 0x00114023
	private bool IsNotNullCharaStoryObj()
	{
		return this.selCharaStoryCtrl != null && this.selCharaStoryCtrl.GuiData != null;
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x00115E43 File Offset: 0x00114043
	private bool IsNotNullMapObj()
	{
		return this.IsNotNullMainStoryObj() && this.selMainStoryCtrl.IsNotNullMapObj();
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x00115E5A File Offset: 0x0011405A
	private bool IsNotNullMapBaseObj()
	{
		return this.IsNotNullMainStoryObj() && this.selMainStoryCtrl.IsNotNullMapBaseObj();
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x00115E71 File Offset: 0x00114071
	private bool IsNotNullEventLargeScaleMapObj()
	{
		return this.IsNotNullEventLargeScaleObj() && this.selEventLargeScaleCtrl.IsNotNullMapObj();
	}

	// Token: 0x0600162A RID: 5674 RVA: 0x00115E88 File Offset: 0x00114088
	private bool IsNotNullEventLargeScaleMapBaseObj()
	{
		return this.IsNotNullEventLargeScaleObj() && this.selEventLargeScaleCtrl.IsNotNullMapBaseObj();
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x00115E9F File Offset: 0x0011409F
	private bool IsNotNullEventCoopObj()
	{
		return this.selEventCoopCtrl != null && this.selEventCoopCtrl.GuiData != null;
	}

	// Token: 0x0600162C RID: 5676 RVA: 0x00115EBF File Offset: 0x001140BF
	private bool IsNotNullEventWildReleaseObj()
	{
		return this.selEventWildReleaseCtrl != null && this.selEventWildReleaseCtrl.GuiData != null;
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x00115EDF File Offset: 0x001140DF
	private bool Is16_9over()
	{
		return SceneQuest.Is16_9over(SceneQuest.mapBoxObject);
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x00115EEB File Offset: 0x001140EB
	public static bool Is16_9over(GameObject go)
	{
		return (go.transform as RectTransform).offsetMin.x < 0f;
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x00115F09 File Offset: 0x00114109
	public static bool IsMainStory(QuestStaticChapter.Category category)
	{
		return SceneQuest.IsMainStoryPart1(category) || SceneQuest.IsMainStoryPart1_5(category) || SceneQuest.IsMainStoryPart2(category) || SceneQuest.IsMainStoryPart3(category);
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x00115F2B File Offset: 0x0011412B
	public static bool IsMainStoryPart1(QuestStaticChapter.Category category)
	{
		return category == QuestStaticChapter.Category.STORY;
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x00115F31 File Offset: 0x00114131
	private static bool IsMainStoryPart1(int count)
	{
		return count == 0;
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x00115F37 File Offset: 0x00114137
	public static bool IsMainStoryPart1_5(QuestStaticChapter.Category category)
	{
		return category == QuestStaticChapter.Category.CELLVAL;
	}

	// Token: 0x06001633 RID: 5683 RVA: 0x00115F3E File Offset: 0x0011413E
	public static bool IsMainStoryPart1_5(int count)
	{
		return count == 1;
	}

	// Token: 0x06001634 RID: 5684 RVA: 0x00115F44 File Offset: 0x00114144
	public static bool IsMainStoryPart2(QuestStaticChapter.Category category)
	{
		return category == QuestStaticChapter.Category.STORY2;
	}

	// Token: 0x06001635 RID: 5685 RVA: 0x00115F4B File Offset: 0x0011414B
	private static bool IsMainStoryPart2(int count)
	{
		return count == 2;
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x00115F51 File Offset: 0x00114151
	public static bool IsMainStoryPart3(QuestStaticChapter.Category category)
	{
		return category == QuestStaticChapter.Category.STORY3;
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x00115F58 File Offset: 0x00114158
	private static bool IsMainStoryPart3(int count)
	{
		return count == 3;
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x00115F60 File Offset: 0x00114160
	public static string GetMainStoryName(QuestStaticChapter.Category category, bool reqShortName)
	{
		if (SceneQuest.IsMainStoryPart1(category))
		{
			return "メインストーリー";
		}
		if (SceneQuest.IsMainStoryPart2(category))
		{
			if (reqShortName)
			{
				return "メインストーリーS2";
			}
			return "メインストーリーシーズン2";
		}
		else
		{
			if (!SceneQuest.IsMainStoryPart3(category))
			{
				return "";
			}
			if (reqShortName)
			{
				return "メインストーリーS3";
			}
			return "メインストーリーシーズン3";
		}
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x00115FAE File Offset: 0x001141AE
	private bool IsNormalMode()
	{
		return SceneQuest.IsNormalMode(this.selectDifficultCount);
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x00115FBB File Offset: 0x001141BB
	public static bool IsNormalMode(int count)
	{
		return count % Enum.GetValues(typeof(SceneQuest.MainStoryDifficulty)).Length == 0;
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x00115FD8 File Offset: 0x001141D8
	private void SwitchDifficultButton()
	{
		this.guiData.chapterSelect.Btn_SortFilterOnOff.gameObject.SetActive(true);
		if (SceneQuest.IsMainStory(this.selectData.questCategory))
		{
			if (!this.IsEnableHardMode(false))
			{
				this.selectDifficultCount = 0;
			}
			this.guiData.chapterSelect.Btn_Sel_Difficult.gameObject.SetActive(true);
			if (this.IsNotNullMainStoryObj())
			{
				this.selMainStoryCtrl.GuiData.pointSelect.Btn_Sel_Difficult.gameObject.SetActive(true);
			}
			this.guiData.chapterSelect.Btn_SortFilterOnOff.gameObject.SetActive(this.IsNormalMode());
			GameObject gameObject = this.guiData.chapterSelect.Btn_Sel_Difficult.transform.Find("BaseImage/Mode_Normal").gameObject;
			if (gameObject)
			{
				gameObject.SetActive(this.IsNormalMode());
			}
			GameObject gameObject2 = this.guiData.chapterSelect.Btn_Sel_Difficult.transform.Find("BaseImage/Mode_Hard").gameObject;
			if (gameObject2)
			{
				gameObject2.SetActive(!this.IsNormalMode());
			}
			if (this.IsNotNullMainStoryObj())
			{
				GameObject gameObject3 = this.selMainStoryCtrl.GuiData.pointSelect.Btn_Sel_Difficult.transform.Find("BaseImage/Mode_Normal").gameObject;
				if (gameObject3)
				{
					gameObject3.SetActive(this.IsNormalMode());
				}
				GameObject gameObject4 = this.selMainStoryCtrl.GuiData.pointSelect.Btn_Sel_Difficult.transform.Find("BaseImage/Mode_Hard").gameObject;
				if (gameObject4)
				{
					gameObject4.SetActive(!this.IsNormalMode());
					return;
				}
			}
		}
		else
		{
			this.guiData.chapterSelect.Btn_Sel_Difficult.gameObject.SetActive(false);
			if (this.IsNotNullMainStoryObj())
			{
				this.selMainStoryCtrl.GuiData.pointSelect.Btn_Sel_Difficult.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x001161CC File Offset: 0x001143CC
	private bool IsEnableHardMode(bool isRewrite = false)
	{
		bool flag = false;
		List<QuestStaticChapter> list = QuestUtil.CreateHardChapterDataList(this.selectData);
		QuestStaticChapter chapter = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == this.selectData.chapterId);
		if (chapter != null)
		{
			flag = list.Exists((QuestStaticChapter item) => item.chapterId == this.selectData.chapterId) || list.Exists((QuestStaticChapter item) => item.chapterId == chapter.hardChapterId);
			if (!flag)
			{
				QuestOnePackData qopd = SceneQuest.GetQuestOnePackDataForReleaseIdStoryHardMode(this.selectData.questCategory);
				flag = list.Exists((QuestStaticChapter item) => item.chapterId == qopd.questChapter.chapterId) || list.Exists((QuestStaticChapter item) => item.chapterId == qopd.questChapter.hardChapterId);
				if (flag && isRewrite)
				{
					this.selectData.chapterId = qopd.questChapter.chapterId;
				}
			}
		}
		return flag;
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x001162BC File Offset: 0x001144BC
	private void SetupNextSceneChangeShop(int eventId)
	{
		SceneShopArgs sceneShopArgs = new SceneShopArgs();
		sceneShopArgs.resultNextSceneName = SceneManager.SceneName.SceneQuest;
		sceneShopArgs.resultNextSceneArgs = new SceneQuest.Args
		{
			selectEventId = eventId,
			category = QuestStaticChapter.Category.EVENT,
			backSequenceGameObject = this.currentSequence
		};
		sceneShopArgs.shopId = 0;
		this.requestNextScene = SceneManager.SceneName.SceneShop;
		this.requestNextSceneArgs = sceneShopArgs;
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x00116312 File Offset: 0x00114512
	private IEnumerator GetItemWindowCtrl(SceneQuest.Args.JustBeforeBattle args)
	{
		this.touchScreenAuth = false;
		GetItemWindowCtrl hdlGetItemWindowCtrl = CanvasManager.HdlGetItemWindowCtrl;
		List<ItemData> specialInfoItemList = args.specialInfoItemList;
		GetItemWindowCtrl.SetupParam setupParam = new GetItemWindowCtrl.SetupParam();
		setupParam.isBox = args.specialInfoItemMovePresentBox;
		setupParam.strCharaCb = (GetItemWindowCtrl.WordingCallbackParam param) => PrjUtil.MakeMessage(param.itemStaticBase.GetName() + "\nが探検隊に加わりました\nプレゼントボックスをご確認ください");
		setupParam.strPhotoCb = (GetItemWindowCtrl.WordingCallbackParam param) => PrjUtil.MakeMessage(param.itemStaticBase.GetName() + "\nを受け取りました");
		setupParam.strItemCb = (GetItemWindowCtrl.WordingCallbackParam param) => PrjUtil.MakeMessage(string.Format("{0}\n× {1}を 受け取りました", param.itemStaticBase.GetName(), param.itemNum));
		setupParam.windowFinishedCallback = delegate(int index)
		{
			this.touchScreenAuth = true;
			return true;
		};
		hdlGetItemWindowCtrl.Setup(specialInfoItemList, setupParam);
		CanvasManager.HdlGetItemWindowCtrl.Open();
		while (!this.touchScreenAuth)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x00116328 File Offset: 0x00114528
	private IEnumerator MainChapterChangeEffect()
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		QuestOnePackData qopd = null;
		QuestOnePackData hardModeOnlyQOPD = null;
		bool isOpenWindowHardModeInfo = false;
		if (this.questArgs != null)
		{
			SceneQuest.<>c__DisplayClass133_0 CS$<>8__locals1 = new SceneQuest.<>c__DisplayClass133_0();
			CS$<>8__locals1.args = this.questArgs.justBeforeBattle;
			if (CS$<>8__locals1.args != null)
			{
				if (CS$<>8__locals1.args.isFirstClear && CS$<>8__locals1.args.isMapAllClearEvent)
				{
					this.guiChapterChange.baseObj.SetActive(true);
					this.guiChapterChange.endMain.baseObj.SetActive(true);
					qopd = DataManager.DmQuest.GetQuestOnePackData(CS$<>8__locals1.args.playQuestId);
					if (qopd != null)
					{
						int chapterNumber = qopd.questChapter.chapterNumber;
						this.guiChapterChange.endMain.Num_Chapter.text = (chapterNumber % 100).ToString();
						this.guiChapterChange.endMain.Txt_ChapterName.text = qopd.questChapter.chapterTitle;
					}
					string text = "MAIN";
					string text2 = "prd_se_selector_chapter_end";
					if (qopd != null)
					{
						if (SceneQuest.IsMainStoryPart1(qopd.questChapter.category))
						{
							text = "MAIN";
							text2 = "prd_se_selector_chapter_end";
						}
						else if (SceneQuest.IsMainStoryPart1_5(qopd.questChapter.category))
						{
							text = "CELLVALL";
							text2 = "prd_se_selector_chapter_end_cellval";
						}
						else if (SceneQuest.IsMainStoryPart2(qopd.questChapter.category))
						{
							text = "MAIN";
							text2 = "prd_se_selector_chapter_end";
						}
						else if (SceneQuest.IsMainStoryPart3(qopd.questChapter.category))
						{
							text = "MAIN";
							text2 = "prd_se_selector_chapter_end";
						}
					}
					SoundManager.Play(text2, false, false);
					this.guiChapterChange.endMain.AEImage_ChapterEnd.GetComponent<PguiReplaceAECtrl>().Replace(text);
					this.guiChapterChange.endMain.AEImage_ChapterEnd.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
					{
						this.guiChapterChange.endMain.AEImage_ChapterEnd.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
						PrjUtil.AddTouchEventTrigger(this.guiChapterChange.endMain.baseObj, delegate(Transform x)
						{
							this.OnTouchMask();
						});
					});
					this.touchScreenAuth = false;
					while (!this.touchScreenAuth)
					{
						yield return null;
					}
					PrjUtil.RemoveTouchEventTrigger(this.guiChapterChange.endMain.baseObj);
					this.guiChapterChange.endMain.AEImage_ChapterEnd.PlayAnimation(PguiAECtrl.AmimeType.END, null);
					while (this.guiChapterChange.endMain.AEImage_ChapterEnd.IsPlaying())
					{
						yield return null;
					}
				}
				if (CS$<>8__locals1.args.isFirstClear)
				{
					DataManager.DmQuest.RequestActionUpdateLastPlayQuestByMap(this.selectData.mapId);
					this.getItemWindowCtrl = this.GetItemWindowCtrl(CS$<>8__locals1.args);
					while (this.getItemWindowCtrl != null)
					{
						yield return null;
					}
					List<DataManagerServerMst.ModeReleaseData> list = DataManager.DmServerMst.ModeReleaseDataList.FindAll((DataManagerServerMst.ModeReleaseData item) => item.QuestId == CS$<>8__locals1.args.playQuestId);
					if (list != null)
					{
						SceneQuest.<>c__DisplayClass133_1 CS$<>8__locals2 = new SceneQuest.<>c__DisplayClass133_1();
						list.Sort((DataManagerServerMst.ModeReleaseData a, DataManagerServerMst.ModeReleaseData b) => a.Category - b.Category);
						CS$<>8__locals2.userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
						using (List<DataManagerServerMst.ModeReleaseData>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								DataManagerServerMst.ModeReleaseData e = enumerator.Current;
								this.touchScreenAuth = false;
								if (!delegate
								{
									DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus lockStatus = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked;
									switch (e.Category)
									{
									case DataManagerServerMst.ModeReleaseData.ModeCategory.GrowthQuest:
										lockStatus = CS$<>8__locals2.userFlagData.ReleaseModeFlag.GrowthQuest;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.FriendsStory:
										lockStatus = CS$<>8__locals2.userFlagData.ReleaseModeFlag.FriendsStory;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.PvpMode:
										lockStatus = CS$<>8__locals2.userFlagData.ReleaseModeFlag.PvpMode;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic:
										lockStatus = CS$<>8__locals2.userFlagData.ReleaseModeFlag.Picnic;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic2:
										lockStatus = CS$<>8__locals2.userFlagData.ReleaseModeFlag.Picnic2;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic3:
										lockStatus = CS$<>8__locals2.userFlagData.ReleaseModeFlag.Picnic3;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic4:
										lockStatus = CS$<>8__locals2.userFlagData.ReleaseModeFlag.Picnic4;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.AraiDiary:
										lockStatus = CS$<>8__locals2.userFlagData.ReleaseModeFlag.AraiDiary;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.TrainingMode:
										lockStatus = CS$<>8__locals2.userFlagData.ReleaseModeFlag.TrainingByQuestTop;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.DholeInactive:
									case DataManagerServerMst.ModeReleaseData.ModeCategory.DholeReturns:
										lockStatus = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.Cellval:
										lockStatus = (QuestUtil.IsUnLockInformationCellvalQuest(TimeManager.Now) ? CS$<>8__locals2.userFlagData.ReleaseModeFlag.CellvalQuest : DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked);
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory2:
										lockStatus = (QuestUtil.IsUnLockInformationMainStory2(TimeManager.Now) ? CS$<>8__locals2.userFlagData.ReleaseModeFlag.MainStory2 : DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked);
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory3:
										lockStatus = (QuestUtil.IsUnLockInformationMainStory3(TimeManager.Now) ? CS$<>8__locals2.userFlagData.ReleaseModeFlag.MainStory3 : DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked);
										break;
									}
									return lockStatus == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
								}())
								{
									CanvasManager.HdlOpenWindowBasic.Setup(e.WindowTitle, e.WindowText, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
									{
										this.touchScreenAuth = true;
										return true;
									}, null, false);
									CanvasManager.HdlOpenWindowBasic.Open();
									switch (e.Category)
									{
									case DataManagerServerMst.ModeReleaseData.ModeCategory.GrowthQuest:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.GrowthQuest = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.FriendsStory:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.FriendsStory = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.PvpMode:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.PvpMode = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.Picnic = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic2:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.Picnic2 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic3:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.Picnic3 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic4:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.Picnic4 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.AraiDiary:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.AraiDiary = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.TrainingMode:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.TrainingByQuestTop = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.Cellval:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.CellvalQuest = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory2:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.MainStory2 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									case DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory3:
										CS$<>8__locals2.userFlagData.ReleaseModeFlag.MainStory3 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
										break;
									}
									DataManager.DmGameStatus.RequestActionUpdateUserFlag(CS$<>8__locals2.userFlagData);
									while (!this.touchScreenAuth)
									{
										yield return null;
									}
									while (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
									{
										yield return null;
									}
								}
							}
						}
						List<DataManagerServerMst.ModeReleaseData>.Enumerator enumerator = default(List<DataManagerServerMst.ModeReleaseData>.Enumerator);
						CS$<>8__locals2 = null;
					}
					hardModeOnlyQOPD = DataManager.DmQuest.GetQuestOnePackData(CS$<>8__locals1.args.playQuestId);
					if (hardModeOnlyQOPD != null)
					{
						QuestOnePackData questOnePackDataForReleaseIdStoryHardMode = SceneQuest.GetQuestOnePackDataForReleaseIdStoryHardMode(hardModeOnlyQOPD.questChapter.category);
						if (questOnePackDataForReleaseIdStoryHardMode != null && questOnePackDataForReleaseIdStoryHardMode.questOne.questId == CS$<>8__locals1.args.playQuestId)
						{
							this.touchScreenAuth = false;
							CanvasManager.HdlOpenWindowBasic.Setup("ハードモード解放", "「ハードモード」が解放されました！", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
							{
								this.touchScreenAuth = true;
								return true;
							}, null, false);
							CanvasManager.HdlOpenWindowBasic.Open();
							isOpenWindowHardModeInfo = true;
							while (!this.touchScreenAuth)
							{
								yield return null;
							}
						}
					}
					this.questFirstClearEvent = new QuestFirstClearEvent(CS$<>8__locals1.args.playQuestId);
					while (this.questFirstClearEvent != null)
					{
						yield return null;
					}
				}
				this.questArgs.justBeforeBattle = null;
			}
			CS$<>8__locals1 = null;
		}
		if (qopd != null)
		{
			this.nextChapterId = qopd.questChapter.chapterId + 1;
		}
		else
		{
			List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.questCategory);
			List<KeyValuePair<int, QuestStaticChapter>> hardModeMap = SceneQuest.GetChapterDataByCategory(this.selectData.questCategory, 1);
			playableMapIdList2.RemoveAll((int mapId) => hardModeMap.Exists((KeyValuePair<int, QuestStaticChapter> hardMode) => hardMode.Key == DataManager.DmQuest.QuestStaticData.mapDataMap[mapId].chapterId));
			if (playableMapIdList2.Count > 0)
			{
				int num = playableMapIdList2[playableMapIdList2.Count - 1];
				this.nextChapterId = (DataManager.DmQuest.IsFirstAccessEventByChapterId(DataManager.DmQuest.QuestStaticData.mapDataMap[num].chapterId) ? this.selectData.chapterId : DataManager.DmQuest.QuestStaticData.mapDataMap[num].chapterId);
			}
			else
			{
				this.nextChapterId = this.selectData.chapterId;
			}
		}
		bool flag = QuestUtil.IsHardMode(new QuestUtil.SelectData
		{
			chapterId = this.nextChapterId,
			questCategory = this.selectData.questCategory
		});
		bool flag2 = QuestUtil.IsHardMode(new QuestUtil.SelectData
		{
			chapterId = this.selectData.chapterId,
			questCategory = this.selectData.questCategory
		});
		if (!flag && !flag2 && !DataManager.DmQuest.IsFirstAccessEventByChapterId(this.nextChapterId))
		{
			QuestStaticChapter chapterData = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == this.nextChapterId);
			List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.nextChapterId);
			string text3 = "";
			foreach (int num2 in playableMapIdList)
			{
				text3 = text3 + num2.ToString() + ":";
			}
			if (chapterData != null && playableMapIdList.Count > 0)
			{
				int chapterNumber2 = chapterData.chapterNumber;
				this.guiChapterChange.start.Window_Txt_ChapterName.text = (this.guiChapterChange.start.Num_Chapter.text = (chapterNumber2 % 100).ToString());
				this.guiChapterChange.start.Window_Num_Chapter.text = (this.guiChapterChange.start.Txt_ChapterName.text = chapterData.chapterTitle);
				this.guiChapterChange.baseObj.SetActive(true);
				this.guiChapterChange.start.AEImage_ChapterStart.gameObject.SetActive(false);
				this.guiChapterChange.start.baseObj.SetActive(true);
				GameObject Obj = null;
				string fileName = "Gui/AE/Chapter/AEImage_ChapterAdd01";
				if (SceneQuest.IsMainStoryPart1(chapterData.category))
				{
					fileName = string.Format("Gui/AE/Chapter/AEImage_ChapterAdd{0:D2}", chapterNumber2 % 100);
				}
				else if (SceneQuest.IsMainStoryPart1_5(chapterData.category))
				{
					fileName = string.Format("Gui/AE/ChapterCellvall/AEImage_ChapterCellvallAdd{0:D2}", chapterNumber2 % 100);
				}
				else if (SceneQuest.IsMainStoryPart2(chapterData.category))
				{
					fileName = string.Format("Gui/AE/Chapter02/AEImage_Chapter02Add{0:D2}", chapterNumber2 % 100);
				}
				else if (SceneQuest.IsMainStoryPart3(chapterData.category))
				{
					fileName = string.Format("Gui/AE/Chapter03/AEImage_Chapter03Add{0:D2}", chapterNumber2 % 100);
				}
				AssetManager.LoadAssetData(fileName, AssetManager.OWNER.QuestSelector, 0, null);
				while (!AssetManager.IsLoadFinishAssetData(fileName))
				{
					yield return null;
				}
				Obj = AssetManager.InstantiateAssetData(fileName, null);
				Obj.transform.SetParent(this.guiChapterChange.start.Null_AEImage_ChapterAdd.transform, true);
				Obj.transform.localPosition = new Vector3(0f, 0f, 0f);
				Obj.transform.localScale = new Vector3(1f, 1f, 1f);
				this.guiChapterChange.start.AEImage_ChapterStart.gameObject.SetActive(true);
				PguiAECtrl componentInChildren = this.guiChapterChange.start.Null_AEImage_ChapterAdd.GetComponentInChildren<PguiAECtrl>();
				if (componentInChildren != null)
				{
					componentInChildren.PlayAnimation(PguiAECtrl.AmimeType.START, null);
				}
				this.selectData.chapterId = this.nextChapterId;
				this.reqNextSequence = this.selMainStoryCtrl.GuiData.pointSelect.baseObj;
				if (playableMapIdList.Count > 0)
				{
					DataManager.DmQuest.RequestActionUpdateLastPlayQuestByMap(playableMapIdList[0]);
				}
				string text4 = "prd_se_selector_chapter_start";
				string text5 = "MAIN";
				if (SceneQuest.IsMainStoryPart1(chapterData.category))
				{
					text5 = "MAIN";
					text4 = "prd_se_selector_chapter_start";
				}
				else if (SceneQuest.IsMainStoryPart1_5(chapterData.category))
				{
					text5 = "CELLVALL";
					text4 = "prd_se_selector_chapter_start_cellval";
				}
				else if (SceneQuest.IsMainStoryPart2(chapterData.category))
				{
					text5 = "MAIN";
					text4 = "prd_se_selector_chapter_start";
				}
				else if (SceneQuest.IsMainStoryPart3(chapterData.category))
				{
					text5 = "MAIN";
					text4 = "prd_se_selector_chapter_start";
				}
				SoundManager.Play(text4, false, false);
				this.guiChapterChange.start.AEImage_ChapterStart.GetComponent<PguiReplaceAECtrl>().Replace(text5);
				this.guiChapterChange.start.AEImage_ChapterStart.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
				{
					this.guiChapterChange.start.AEImage_ChapterStart.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
					PrjUtil.AddTouchEventTrigger(this.guiChapterChange.start.baseObj, delegate(Transform x)
					{
						this.OnTouchMask();
					});
				});
				this.guiChapterChange.start.Bg.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
				{
					this.guiChapterChange.start.Bg.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				});
				this.touchScreenAuth = false;
				while (!this.touchScreenAuth)
				{
					yield return null;
				}
				this.guiChapterChange.start.Bg.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				this.guiChapterChange.start.AEImage_ChapterStart.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				PrjUtil.RemoveTouchEventTrigger(this.guiChapterChange.start.baseObj);
				while (this.guiChapterChange.start.Bg.IsPlaying() || this.guiChapterChange.start.AEImage_ChapterStart.IsPlaying())
				{
					yield return null;
				}
				if (Obj != null)
				{
					Object.Destroy(Obj);
				}
				DataManager.DmQuest.RequestActionUpdateFinishChapterEvent(this.nextChapterId);
				Obj = null;
				fileName = null;
			}
			else
			{
				this.guiChapterChange.baseObj.SetActive(true);
				this.guiChapterChange.modeEnd.baseObj.SetActive(true);
				this.modeEndSequenceCtrl = this.guiChapterChange.modeEnd.SequenceCtrl(this.selectData);
				while (this.modeEndSequenceCtrl != null)
				{
					yield return null;
				}
				this.guiChapterChange.modeEnd.baseObj.SetActive(false);
				this.ChangeSceneModeEnd();
			}
			chapterData = null;
			playableMapIdList = null;
		}
		if (isOpenWindowHardModeInfo)
		{
			QuestOnePackData questOnePackDataForReleaseIdStoryHardMode2 = SceneQuest.GetQuestOnePackDataForReleaseIdStoryHardMode((hardModeOnlyQOPD != null) ? hardModeOnlyQOPD.questChapter.category : QuestStaticChapter.Category.STORY);
			string text6;
			if (questOnePackDataForReleaseIdStoryHardMode2 != null)
			{
				string mainStoryName = SceneQuest.GetMainStoryName(questOnePackDataForReleaseIdStoryHardMode2.questChapter.category, true);
				text6 = mainStoryName + ((mainStoryName != "") ? "\n" : "") + questOnePackDataForReleaseIdStoryHardMode2.questChapter.chapterName + questOnePackDataForReleaseIdStoryHardMode2.questGroup.titleName + PrjUtil.MakeMessage("クリア");
				if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questOnePackDataForReleaseIdStoryHardMode2.questOne.questId))
				{
					QuestOneStatus questOneStatus = DataManager.DmQuest.QuestDynamicData.oneDataMap[questOnePackDataForReleaseIdStoryHardMode2.questOne.questId].status;
				}
			}
			else
			{
				text6 = "クエスト情報がありません";
			}
			MarkLockCtrl markLock = this.selMainStoryCtrl.GuiData.pointSelect.markLock;
			MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();
			setupParam.updateConditionCallback = () => true;
			setupParam.releaseFlag = false;
			setupParam.tagetObject = this.selMainStoryCtrl.GuiData.pointSelect.Btn_Sel_Difficult.gameObject;
			setupParam.text = text6;
			setupParam.updateUserFlagDataCallback = delegate
			{
			};
			markLock.Setup(setupParam, true);
		}
		this.guiChapterChange.endMain.baseObj.SetActive(false);
		this.guiChapterChange.start.baseObj.SetActive(false);
		this.guiChapterChange.baseObj.SetActive(false);
		CanvasManager.SetEnableCmnTouchMask(false);
		yield break;
		yield break;
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x00116337 File Offset: 0x00114537
	private IEnumerator SideStoryChapterChangeEffect()
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		QuestOnePackData qopd = null;
		if (this.questArgs != null)
		{
			SceneQuest.Args.JustBeforeBattle args = this.questArgs.justBeforeBattle;
			if (args != null)
			{
				if (args.isFirstClear)
				{
					if (args.isMapAllClearEvent)
					{
						this.guiChapterChange.baseObj.SetActive(true);
						this.guiChapterChange.endSub.baseObj.SetActive(true);
						qopd = DataManager.DmQuest.GetQuestOnePackData(args.playQuestId);
						if (qopd != null)
						{
							this.guiChapterChange.endSub.Num_Chapter.text = qopd.questChapter.chapterName;
							this.guiChapterChange.endSub.Txt_ChapterName.text = qopd.questChapter.chapterTitle;
						}
						SoundManager.Play("prd_se_selector_chapter_end", false, false);
						this.guiChapterChange.endSub.AEImage_ChapterEnd.m_AEImage.GetComponent<PguiReplaceAECtrl>().Replace("END");
						this.guiChapterChange.endSub.AEImage_ChapterEnd.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
						{
							this.guiChapterChange.endSub.AEImage_ChapterEnd.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
							PrjUtil.AddTouchEventTrigger(this.guiChapterChange.endSub.baseObj, delegate(Transform x)
							{
								this.OnTouchMask();
							});
						});
						this.touchScreenAuth = false;
						while (!this.touchScreenAuth)
						{
							yield return null;
						}
						PrjUtil.RemoveTouchEventTrigger(this.guiChapterChange.endSub.baseObj);
						this.guiChapterChange.endSub.AEImage_ChapterEnd.PlayAnimation(PguiAECtrl.AmimeType.END, null);
						while (this.guiChapterChange.endSub.AEImage_ChapterEnd.IsPlaying())
						{
							yield return null;
						}
						this.guiChapterChange.endSub.baseObj.SetActive(false);
						this.guiChapterChange.baseObj.SetActive(false);
					}
					DataManager.DmQuest.RequestActionUpdateLastPlayQuestByMap(this.selectData.mapId);
					this.getItemWindowCtrl = this.GetItemWindowCtrl(args);
					while (this.getItemWindowCtrl != null)
					{
						yield return null;
					}
					this.questFirstClearEvent = new QuestFirstClearEvent(args.playQuestId);
					while (this.questFirstClearEvent != null)
					{
						yield return null;
					}
				}
				this.questArgs.justBeforeBattle = null;
			}
			args = null;
		}
		if (qopd != null)
		{
			this.nextChapterId = qopd.questChapter.chapterId + 1;
		}
		else
		{
			List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.questCategory);
			List<KeyValuePair<int, QuestStaticChapter>> hardModeMap = SceneQuest.GetChapterDataByCategory(this.selectData.questCategory, 1);
			playableMapIdList.RemoveAll((int mapId) => hardModeMap.Exists((KeyValuePair<int, QuestStaticChapter> hardMode) => hardMode.Key == DataManager.DmQuest.QuestStaticData.mapDataMap[mapId].chapterId));
			if (playableMapIdList.Count > 0)
			{
				int num = playableMapIdList[playableMapIdList.Count - 1];
				this.nextChapterId = (DataManager.DmQuest.IsFirstAccessEventByChapterId(DataManager.DmQuest.QuestStaticData.mapDataMap[num].chapterId) ? this.selectData.chapterId : DataManager.DmQuest.QuestStaticData.mapDataMap[num].chapterId);
			}
			else
			{
				this.nextChapterId = this.selectData.chapterId;
			}
		}
		bool flag = QuestUtil.IsHardMode(new QuestUtil.SelectData
		{
			chapterId = this.nextChapterId,
			questCategory = this.selectData.questCategory
		});
		bool flag2 = QuestUtil.IsHardMode(new QuestUtil.SelectData
		{
			chapterId = this.selectData.chapterId,
			questCategory = this.selectData.questCategory
		});
		if (!flag && !flag2 && !DataManager.DmQuest.IsFirstAccessEventByChapterId(this.nextChapterId))
		{
			this.guiChapterChange.baseObj.SetActive(true);
			QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == this.nextChapterId);
			List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(this.nextChapterId);
			if (questStaticChapter != null && playableMapIdList2.Count > 0)
			{
				this.guiChapterChange.endSub.baseObj.SetActive(true);
				SoundManager.Play("prd_se_selector_chapter_side_story_start", false, false);
				this.guiChapterChange.endSub.AEImage_ChapterEnd.m_AEImage.GetComponent<PguiReplaceAECtrl>().Replace("START");
				this.guiChapterChange.endSub.Num_Chapter.text = questStaticChapter.chapterName;
				this.guiChapterChange.endSub.Txt_ChapterName.text = questStaticChapter.chapterTitle;
				this.selectData.chapterId = this.nextChapterId;
				this.selectData.mapId = playableMapIdList2[0];
				this.reqNextSequence = this.selSideStoryCtrl.GuiData.mapSelect.baseObj;
				DataManager.DmQuest.RequestActionUpdateLastPlayQuestByMap(playableMapIdList2[0]);
				this.touchScreenAuth = false;
				this.guiChapterChange.endSub.AEImage_ChapterEnd.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
				{
					this.guiChapterChange.endSub.AEImage_ChapterEnd.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
					{
						this.touchScreenAuth = true;
					});
				});
				while (!this.touchScreenAuth)
				{
					yield return null;
				}
				DataManager.DmQuest.RequestActionUpdateFinishChapterEvent(this.nextChapterId);
			}
			else
			{
				this.guiChapterChange.baseObj.SetActive(true);
				this.guiChapterChange.modeEnd.baseObj.SetActive(true);
				this.modeEndSequenceCtrl = this.guiChapterChange.modeEnd.SequenceCtrl(this.selectData);
				while (this.modeEndSequenceCtrl != null)
				{
					yield return null;
				}
				this.guiChapterChange.modeEnd.baseObj.SetActive(false);
				this.ChangeSceneModeEnd();
			}
		}
		this.guiChapterChange.endSub.baseObj.SetActive(false);
		this.guiChapterChange.baseObj.SetActive(false);
		CanvasManager.SetEnableCmnTouchMask(false);
		yield break;
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x00116348 File Offset: 0x00114548
	private void ChangeSceneModeEnd()
	{
		QuestStaticChapter questStaticChapter = null;
		if (DataManager.DmQuest.QuestStaticData.chapterDataMap.ContainsKey(this.selectData.chapterId))
		{
			questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.selectData.chapterId];
		}
		if (questStaticChapter != null && questStaticChapter.EndType != QuestStaticChapter.ChapterEndType.Default)
		{
			this.requestNextScene = SceneManager.SceneName.SceneHome;
		}
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x001163AB File Offset: 0x001145AB
	private IEnumerator CharaStoryEffect()
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		QuestOnePackData qopd = null;
		if (this.questArgs != null)
		{
			SceneQuest.Args.JustBeforeBattle args = this.questArgs.justBeforeBattle;
			if (args != null)
			{
				if (args.isFirstClear)
				{
					bool isMapAllClearEvent = args.isMapAllClearEvent;
					this.getItemWindowCtrl = this.GetItemWindowCtrl(args);
					while (this.getItemWindowCtrl != null)
					{
						yield return null;
					}
					this.questFirstClearEvent = new QuestFirstClearEvent(args.playQuestId);
					while (this.questFirstClearEvent != null)
					{
						yield return null;
					}
				}
				if (args.isReleaseMaxArts)
				{
					this.touchScreenAuth = false;
					CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("けものミラクル") + PrjUtil.MakeMessage("＋が解放されました！"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
					{
						this.touchScreenAuth = true;
						return true;
					}, null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
					while (!this.touchScreenAuth)
					{
						yield return null;
					}
				}
				this.questArgs.justBeforeBattle = null;
			}
			args = null;
		}
		this.nextChapterId = ((qopd != null) ? (qopd.questChapter.chapterId + 1) : this.selectData.chapterId);
		bool flag = QuestUtil.IsHardMode(new QuestUtil.SelectData
		{
			chapterId = this.nextChapterId,
			questCategory = this.selectData.questCategory
		});
		bool flag2 = QuestUtil.IsHardMode(new QuestUtil.SelectData
		{
			chapterId = this.selectData.chapterId,
			questCategory = this.selectData.questCategory
		});
		if (!flag && !flag2)
		{
			DataManager.DmQuest.IsFirstAccessEventByChapterId(this.nextChapterId);
		}
		CanvasManager.SetEnableCmnTouchMask(false);
		yield break;
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x001163BA File Offset: 0x001145BA
	private IEnumerator ChapterChangeEffect(QuestStaticChapter chapterData)
	{
		List<KeyValuePair<int, QuestStaticChapter>> chapterDataByCategory = SceneQuest.GetChapterDataByCategory(chapterData.category, this.IsNormalMode() ? 0 : 1);
		int num = chapterDataByCategory.FindIndex((KeyValuePair<int, QuestStaticChapter> item) => item.Key == chapterData.chapterId);
		if (num < 0)
		{
			num = 0;
		}
		CanvasManager.SetEnableCmnTouchMask(true);
		this.guiChapterChange.baseObj.SetActive(true);
		this.guiChapterChange.change.AEImage_ChapterStart.gameObject.SetActive(false);
		this.guiChapterChange.change.baseObj.SetActive(true);
		if (chapterDataByCategory.Count > 0)
		{
			int chapterNumber = chapterDataByCategory[num].Value.chapterNumber;
			this.guiChapterChange.change.Num_Chapter.text = (chapterNumber % 100).ToString();
			this.guiChapterChange.change.Txt_Chapter.text = chapterDataByCategory[num].Value.chapterTitle;
		}
		SoundManager.Play("prd_se_selector_chapter_change", false, false);
		GameObject Obj = null;
		string fileName = "Gui/AE/Chapter/AEImage_ChapterAdd01";
		if (SceneQuest.IsMainStoryPart1(chapterData.category))
		{
			fileName = string.Format("Gui/AE/Chapter/AEImage_ChapterAdd{0:D2}", chapterData.chapterNumber % 100);
		}
		else if (SceneQuest.IsMainStoryPart1_5(chapterData.category))
		{
			fileName = string.Format("Gui/AE/ChapterCellvall/AEImage_ChapterCellvallAdd{0:D2}", chapterData.chapterNumber % 100);
		}
		else if (SceneQuest.IsMainStoryPart2(chapterData.category))
		{
			fileName = string.Format("Gui/AE/Chapter02/AEImage_Chapter02Add{0:D2}", chapterData.chapterNumber % 100);
		}
		else if (SceneQuest.IsMainStoryPart3(chapterData.category))
		{
			fileName = string.Format("Gui/AE/Chapter03/AEImage_Chapter03Add{0:D2}", chapterData.chapterNumber % 100);
		}
		AssetManager.LoadAssetData(fileName, AssetManager.OWNER.QuestSelector, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(fileName))
		{
			yield return null;
		}
		Obj = AssetManager.InstantiateAssetData(fileName, null);
		Obj.transform.SetParent(this.guiChapterChange.change.Null_AEImage_ChapterAdd.transform, true);
		Obj.transform.localPosition = new Vector3(0f, 0f, 0f);
		Obj.transform.localScale = new Vector3(1f, 1f, 1f);
		this.guiChapterChange.change.Null_AEImage_ChapterAdd.gameObject.SetActive(true);
		this.guiChapterChange.change.AEImage_ChapterStart.gameObject.SetActive(true);
		PguiAECtrl componentInChildren = this.guiChapterChange.change.Null_AEImage_ChapterAdd.GetComponentInChildren<PguiAECtrl>();
		if (componentInChildren != null)
		{
			componentInChildren.PlayAnimation(PguiAECtrl.AmimeType.START, null);
		}
		this.guiChapterChange.change.AEImage_ChapterStart.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
		});
		this.guiChapterChange.change.Bg.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
		});
		PrjUtil.RemoveTouchEventTrigger(this.guiChapterChange.change.baseObj);
		this.selectData.chapterId = chapterData.chapterId;
		this.selectData.questCategory = chapterData.category;
		if (chapterData.category != this.questChapterChangeWindow.CategoryByOpenWindow)
		{
			string mainStoryMapPath = SceneQuest.GetMainStoryMapPath(chapterData.category);
			this.DestroyMap();
			this.instCarObjList = SceneQuest.CreateCarObjList(SceneQuest.mapBoxObject);
			this.requestSequenceInstantiateAssetData = Singleton<SceneManager>.Instance.StartCoroutine(this.InstantiateAssetData(this.LoadMapObject(mainStoryMapPath), mainStoryMapPath));
			while (this.requestSequenceInstantiateAssetData != null)
			{
				yield return null;
			}
		}
		this.reqNextSequence = this.selMainStoryCtrl.GuiData.pointSelect.baseObj;
		while (this.guiChapterChange.change.Bg.IsPlaying() || this.guiChapterChange.change.AEImage_ChapterStart.IsPlaying())
		{
			yield return null;
		}
		this.guiChapterChange.change.baseObj.SetActive(false);
		this.guiChapterChange.baseObj.SetActive(false);
		CanvasManager.SetEnableCmnTouchMask(false);
		Object.Destroy(Obj);
		yield break;
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x001163D0 File Offset: 0x001145D0
	private IEnumerator EventLargeScaleEffect()
	{
		if (this.questArgs != null)
		{
			SceneQuest.Args.JustBeforeBattle args = this.questArgs.justBeforeBattle;
			if (args != null)
			{
				if (args.isFirstClear)
				{
					DataManager.DmQuest.RequestActionUpdateLastPlayQuestByMap(this.selectData.mapId);
					this.getItemWindowCtrl = this.GetItemWindowCtrl(args);
					while (this.getItemWindowCtrl != null)
					{
						yield return null;
					}
					this.questFirstClearEvent = new QuestFirstClearEvent(args.playQuestId);
					while (this.questFirstClearEvent != null)
					{
						yield return null;
					}
				}
				this.questArgs.justBeforeBattle = null;
			}
			args = null;
		}
		yield break;
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x001163DF File Offset: 0x001145DF
	private void OnTouchMask()
	{
		this.touchScreenAuth = true;
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x001163E8 File Offset: 0x001145E8
	public static QuestOnePackData GetQuestOnePackDataForReleaseIdStoryHardMode(QuestStaticChapter.Category category)
	{
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(category);
		if (SceneQuest.IsMainStory(category) && playableMapIdList.Count > 0)
		{
			QuestStaticMap qsm = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap n) => n.mapId == playableMapIdList[0]);
			QuestStaticChapter qsc = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter n) => n.chapterId == qsm.chapterId);
			List<QuestStaticQuestGroup> list = new List<QuestStaticQuestGroup>(DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap n) => n.chapterId == qsc.hardChapterId).questGroupList);
			list.Sort((QuestStaticQuestGroup a, QuestStaticQuestGroup b) => a.questGroupId - b.questGroupId);
			List<QuestStaticQuestOne> list2 = new List<QuestStaticQuestOne>(list[0].questOneList);
			list2.Sort((QuestStaticQuestOne a, QuestStaticQuestOne b) => a.questId - b.questId);
			return DataManager.DmQuest.GetQuestOnePackData(list2[0].relQuestId);
		}
		return null;
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x00116517 File Offset: 0x00114717
	private IEnumerator RequestQuestCmd()
	{
		while (this.eventStartTimeMap.Count > 0)
		{
			int num = 0;
			foreach (KeyValuePair<int, DateTime> keyValuePair in this.eventStartTimeMap)
			{
				if (keyValuePair.Value <= TimeManager.Now)
				{
					num = keyValuePair.Key;
					break;
				}
			}
			if (num > 0)
			{
				DataManager.DmQuest.RequestGetUserQuestInfo();
				this.eventStartTimeMap.Remove(num);
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
			}
			yield return null;
		}
		this.requestQuestCmd = null;
		yield break;
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x00116528 File Offset: 0x00114728
	private static int SortFunc(DataManagerEvent.EventData a, DataManagerEvent.EventData b)
	{
		int num2;
		int num = (num2 = 0);
		int num3 = 5;
		int num4 = 10;
		if (a.IsEnableChapter)
		{
			num2 = num4;
		}
		else if (!a.IsEnableChapter && a.IsEnableBannerByQuestTop)
		{
			num2 = num3;
		}
		if (b.IsEnableChapter)
		{
			num = num4;
		}
		else if (!b.IsEnableChapter && b.IsEnableBannerByQuestTop)
		{
			num = num3;
		}
		return num - num2;
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06001649 RID: 5705 RVA: 0x0011657D File Offset: 0x0011477D
	// (set) Token: 0x0600164A RID: 5706 RVA: 0x00116585 File Offset: 0x00114785
	private int StoryQuestSwitchCount { get; set; }

	// Token: 0x0600164B RID: 5707 RVA: 0x00116590 File Offset: 0x00114790
	private void UpdateBtnStorySelect(DataManagerGameStatus.UserFlagData userFlagData)
	{
		this.guiData.questTop.storyQuestParts.Btn_StorySelectL_New.SetActive(false);
		foreach (QuestStaticChapter.Category category in this.guiData.questTop.storyQuestParts.GetBtnStorySelectLNewList(this.StoryQuestSwitchCount))
		{
			if (SceneQuest.SetDispNew(DataManager.DmQuest.GetPlayableMapIdList(category)))
			{
				this.guiData.questTop.storyQuestParts.Btn_StorySelectL_New.SetActive(true);
				break;
			}
		}
		this.guiData.questTop.storyQuestParts.Btn_StorySelectR_New.SetActive(false);
		if ((QuestUtil.IsUnLockInformationCellvalQuest(TimeManager.Now) && userFlagData.ReleaseModeFlag.CellvalQuest >= DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked) || (QuestUtil.IsUnLockInformationMainStory2(TimeManager.Now) && userFlagData.ReleaseModeFlag.MainStory2 >= DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked) || (QuestUtil.IsUnLockInformationMainStory3(TimeManager.Now) && userFlagData.ReleaseModeFlag.MainStory3 >= DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked))
		{
			foreach (QuestStaticChapter.Category category2 in this.guiData.questTop.storyQuestParts.GetBtnStorySelectRNewList(this.StoryQuestSwitchCount))
			{
				if (SceneQuest.SetDispNew(DataManager.DmQuest.GetPlayableMapIdList(category2)))
				{
					this.guiData.questTop.storyQuestParts.Btn_StorySelectR_New.SetActive(true);
					break;
				}
			}
		}
		this.SetupQuestTopDecorationCellval(userFlagData);
		QuestStaticChapter.Category category3 = QuestStaticChapter.Category.STORY;
		if (SceneQuest.IsMainStoryPart1_5(this.StoryQuestSwitchCount))
		{
			category3 = QuestStaticChapter.Category.CELLVAL;
		}
		else if (SceneQuest.IsMainStoryPart2(this.StoryQuestSwitchCount))
		{
			category3 = QuestStaticChapter.Category.STORY2;
		}
		else if (SceneQuest.IsMainStoryPart3(this.StoryQuestSwitchCount))
		{
			category3 = QuestStaticChapter.Category.STORY3;
		}
		QuestStaticChapter chapterByNewestStory = DataManager.DmQuest.GetChapterByNewestStory(category3);
		if (chapterByNewestStory != null)
		{
			this.guiData.questTop.storyQuestParts.Texture_StoryPhoto.SetRawImage(chapterByNewestStory.topBoardImagePath, true, false, null);
		}
		this.guiData.questTop.storyQuestParts.SetActiveMarkNew(this.StoryQuestSwitchCount);
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x001167B4 File Offset: 0x001149B4
	private void SetupQuestTopDecorationCellval(DataManagerGameStatus.UserFlagData userFlagData)
	{
		if (SceneQuest.IsMainStoryPart1(this.StoryQuestSwitchCount))
		{
			this.guiData.questTop.storyQuestParts.markLock.gameObject.SetActive(false);
			this.guiData.questTop.storyQuestParts.Btn_StoryQuest.SetActEnable(true, false, false);
			return;
		}
		if (SceneQuest.IsMainStoryPart1_5(this.StoryQuestSwitchCount))
		{
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.CELLVAL));
			string text;
			if (questOnePackData != null)
			{
				string mainStoryName = SceneQuest.GetMainStoryName(questOnePackData.questChapter.category, true);
				text = mainStoryName + ((mainStoryName != "") ? "\n" : "") + questOnePackData.questChapter.chapterName + questOnePackData.questGroup.titleName + PrjUtil.MakeMessage("クリア");
			}
			else
			{
				text = "クエスト情報がありません";
			}
			MarkLockCtrl markLock = this.guiData.questTop.storyQuestParts.markLock;
			MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();
			setupParam.updateConditionCallback = () => QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.CELLVAL);
			setupParam.releaseFlag = userFlagData.ReleaseModeFlag.CellvalQuest == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			setupParam.tagetObject = this.guiData.questTop.storyQuestParts.Btn_StoryQuest.gameObject;
			setupParam.text = text;
			setupParam.updateUserFlagDataCallback = delegate
			{
				userFlagData.ReleaseModeFlag.CellvalQuest = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
				DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
			};
			setupParam.ignoreDisableColorChangeList = new List<string> { "Cmn_Mark_New" };
			markLock.Setup(setupParam, SceneQuest.IsMainStoryPart1_5(this.StoryQuestSwitchCount));
			return;
		}
		if (SceneQuest.IsMainStoryPart2(this.StoryQuestSwitchCount))
		{
			QuestOnePackData questOnePackData2 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.STORY2));
			string text2;
			if (questOnePackData2 != null)
			{
				string mainStoryName2 = SceneQuest.GetMainStoryName(questOnePackData2.questChapter.category, true);
				text2 = mainStoryName2 + ((mainStoryName2 != "") ? "\n" : "") + questOnePackData2.questChapter.chapterName + questOnePackData2.questGroup.titleName + PrjUtil.MakeMessage("クリア");
			}
			else
			{
				text2 = "クエスト情報がありません";
			}
			MarkLockCtrl markLock2 = this.guiData.questTop.storyQuestParts.markLock;
			MarkLockCtrl.SetupParam setupParam2 = new MarkLockCtrl.SetupParam();
			setupParam2.updateConditionCallback = () => QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.STORY2);
			setupParam2.releaseFlag = userFlagData.ReleaseModeFlag.MainStory2 == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			setupParam2.tagetObject = this.guiData.questTop.storyQuestParts.Btn_StoryQuest.gameObject;
			setupParam2.text = text2;
			setupParam2.updateUserFlagDataCallback = delegate
			{
				userFlagData.ReleaseModeFlag.MainStory2 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
				DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
			};
			setupParam2.ignoreDisableColorChangeList = new List<string> { "Cmn_Mark_New" };
			markLock2.Setup(setupParam2, SceneQuest.IsMainStoryPart2(this.StoryQuestSwitchCount));
			return;
		}
		if (SceneQuest.IsMainStoryPart3(this.StoryQuestSwitchCount))
		{
			QuestOnePackData questOnePackData3 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.STORY3));
			string text3;
			if (questOnePackData3 != null)
			{
				string mainStoryName3 = SceneQuest.GetMainStoryName(questOnePackData3.questChapter.category, true);
				text3 = mainStoryName3 + ((mainStoryName3 != "") ? "\n" : "") + questOnePackData3.questChapter.chapterName + questOnePackData3.questGroup.titleName + PrjUtil.MakeMessage("クリア");
			}
			else
			{
				text3 = "クエスト情報がありません";
			}
			MarkLockCtrl markLock3 = this.guiData.questTop.storyQuestParts.markLock;
			MarkLockCtrl.SetupParam setupParam3 = new MarkLockCtrl.SetupParam();
			setupParam3.updateConditionCallback = () => QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.STORY3);
			setupParam3.releaseFlag = userFlagData.ReleaseModeFlag.MainStory3 == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			setupParam3.tagetObject = this.guiData.questTop.storyQuestParts.Btn_StoryQuest.gameObject;
			setupParam3.text = text3;
			setupParam3.updateUserFlagDataCallback = delegate
			{
				userFlagData.ReleaseModeFlag.MainStory3 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
				DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
			};
			setupParam3.ignoreDisableColorChangeList = new List<string> { "Cmn_Mark_New" };
			markLock3.Setup(setupParam3, SceneQuest.IsMainStoryPart3(this.StoryQuestSwitchCount));
		}
	}

	// Token: 0x0600164D RID: 5709 RVA: 0x00116BE8 File Offset: 0x00114DE8
	private void UpdateQuestTopDecoration()
	{
		QuestStaticChapter.Category category = QuestStaticChapter.Category.STORY;
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.STORY);
		List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.CELLVAL);
		List<int> playableMapIdList3 = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.STORY2);
		List<int> playableMapIdList4 = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.STORY3);
		bool flag = SceneQuest.SetDispNew(playableMapIdList);
		bool flag2 = SceneQuest.SetDispNew(playableMapIdList2);
		bool flag3 = SceneQuest.SetDispNew(playableMapIdList3);
		bool flag4 = SceneQuest.SetDispNew(playableMapIdList4);
		if (!flag && flag2 && !flag3 && !flag4)
		{
			category = QuestStaticChapter.Category.CELLVAL;
		}
		else if (flag && !flag2)
		{
			category = QuestStaticChapter.Category.STORY;
		}
		else if (!flag && !flag2 && flag3)
		{
			category = QuestStaticChapter.Category.STORY2;
		}
		else if (!flag && !flag2 && !flag3 && flag4)
		{
			category = QuestStaticChapter.Category.STORY3;
		}
		else
		{
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(DataManager.DmUserInfo.optionData.LastPlayQuestOneIdByMainScenario);
			if (questOnePackData != null)
			{
				category = questOnePackData.questChapter.category;
			}
		}
		if (SceneQuest.IsMainStoryPart1(category))
		{
			this.StoryQuestSwitchCount = 0;
		}
		else if (SceneQuest.IsMainStoryPart1_5(category))
		{
			this.StoryQuestSwitchCount = 1;
		}
		else if (SceneQuest.IsMainStoryPart2(category))
		{
			this.StoryQuestSwitchCount = 2;
		}
		else if (SceneQuest.IsMainStoryPart3(category))
		{
			this.StoryQuestSwitchCount = 3;
		}
		this.guiData.questTop.storyQuestParts.Setup(this.StoryQuestSwitchCount);
		this.UpdateBtnStorySelect(DataManager.DmGameStatus.MakeUserFlagData());
		this.guiData.questTop.ResetCampaignInfoCategory(category, 0);
		this.guiData.questTop.UpdateCampaignInfoCategory(category, 0);
		this.guiData.questTop.storyQuestParts.Btn_StoryQuestAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
		{
			this.guiData.questTop.storyQuestParts.Btn_StoryQuestAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		});
		List<int> playableMapIdList5 = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.GROW);
		this.guiData.questTop.GrowQuest_Mark_New.InitForce();
		this.guiData.questTop.GrowQuest_Mark_New.gameObject.SetActive(SceneQuest.SetDispNew(playableMapIdList5));
		this.guiData.questTop.ResetCampaignInfoCategory(QuestStaticChapter.Category.GROW, 0);
		this.guiData.questTop.UpdateCampaignInfoCategory(QuestStaticChapter.Category.GROW, 0);
		this.guiData.questTop.Btn_GrowQuestAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
		{
			this.guiData.questTop.Btn_GrowQuestAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		});
		List<int> playableMapIdList6 = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.ETCETERA);
		this.guiData.questTop.EtceteraQuest_Mark_New.InitForce();
		this.guiData.questTop.EtceteraQuest_Mark_New.gameObject.SetActive(SceneQuest.SetDispNew(playableMapIdList6));
		this.guiData.questTop.ResetCampaignInfoCategory(QuestStaticChapter.Category.ETCETERA, 0);
		this.guiData.questTop.UpdateCampaignInfoCategory(QuestStaticChapter.Category.ETCETERA, 0);
		this.guiData.questTop.Btn_EtceteraQuestAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
		{
			this.guiData.questTop.Btn_EtceteraQuestAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		});
		this.guiData.questTop.CharQuest_Mark_New.InitForce();
		this.guiData.questTop.CharQuest_Mark_New.gameObject.SetActive(SceneQuest.SetDispNew(SelCharaStoryCtrl.GetPlayableMapDataList()));
		this.guiData.questTop.ResetCampaignInfoCategory(QuestStaticChapter.Category.CHARA, 0);
		this.guiData.questTop.UpdateCampaignInfoCategory(QuestStaticChapter.Category.CHARA, 0);
		List<int> playableMapIdList7 = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.SIDE_STORY);
		this.guiData.questTop.AnotherStory_Mark_New.InitForce();
		this.guiData.questTop.AnotherStory_Mark_New.gameObject.SetActive(SceneQuest.SetDispNew(playableMapIdList7));
		this.guiData.questTop.ResetCampaignInfoCategory(QuestStaticChapter.Category.SIDE_STORY, 0);
		this.guiData.questTop.UpdateCampaignInfoCategory(QuestStaticChapter.Category.SIDE_STORY, 0);
		this.guiData.questTop.Training_Mark_New.InitForce();
		this.guiData.questTop.Training_Mark_New.gameObject.SetActive(false);
		this.guiData.questTop.ResetCampaignInfoCategory(QuestStaticChapter.Category.TRAINING, 0);
		this.guiData.questTop.UpdateCampaignInfoCategory(QuestStaticChapter.Category.TRAINING, 0);
		this.guiData.questTop.ladyBugAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		this.guiData.questTop.pallAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
		{
			this.guiData.questTop.pallAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		});
		DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
		Singleton<SceneManager>.Instance.StartCoroutine(this.SetQuestTopWindow(userFlagData));
		List<DataManagerEvent.EventData> eventDataList = DataManager.DmEvent.GetEventDataList();
		this.enableEventDataList = new List<DataManagerEvent.EventData>();
		for (int i = 0; i < eventDataList.Count; i++)
		{
			if (eventDataList[i].IsEnableBannerByQuestTop)
			{
				this.enableEventDataList.Add(eventDataList[i]);
			}
		}
		this.guiData.questTop.Window_EventAll.gameObject.SetActive(false);
		if (this.enableEventDataList.Count > 0)
		{
			PrjUtil.InsertionSort<DataManagerEvent.EventData>(ref this.enableEventDataList, (DataManagerEvent.EventData a, DataManagerEvent.EventData b) => b.eventId.CompareTo(a.eventId));
			PrjUtil.InsertionSort<DataManagerEvent.EventData>(ref this.enableEventDataList, new Comparison<DataManagerEvent.EventData>(SceneQuest.SortFunc));
			this.guiData.questTop.EventGroup.SetActive(true);
			this.guiData.questTop.renderTextureChara.transform.SetAsLastSibling();
			this.guiData.questTop.renderTextureChara.postion = new Vector2(-553f, -92f);
			this.guiData.questTop.renderTextureChara.rotation = new Vector3(0f, -13f, 0f);
			this.guiData.questTop.renderTextureChara.fieldOfView = 21f;
			this.guiData.questTop.renderTextureChara.OnValidate();
			this.guiData.questTop.ScrollView.InitForce();
			if (this.guiData.questTop.ScrollView.onStartItem == null)
			{
				ReuseScroll scrollView = this.guiData.questTop.ScrollView;
				scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartItemEventAll));
			}
			if (this.guiData.questTop.ScrollView.onUpdateItem == null)
			{
				ReuseScroll scrollView2 = this.guiData.questTop.ScrollView;
				scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemEventAll));
				this.guiData.questTop.ScrollView.Setup(3, 0);
			}
			this.guiData.questTop.ScrollView.Refresh();
			this.guiData.questTop.Btn_EventAll.gameObject.SetActive(this.enableEventDataList.Count >= 3);
			this.UpdateButtonEventAllImage();
			for (int j = 0; j < this.guiData.questTop.eventPartsList.Count; j++)
			{
				if (j < this.enableEventDataList.Count)
				{
					HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(this.enableEventDataList[j].eventBannerId);
					if (homeBannerData != null)
					{
						this.guiData.questTop.eventButton[j].gameObject.SetActive(true);
						this.guiData.questTop.eventPartsList[j].bannerImage.banner = homeBannerData.bannerImagePathByQuestTop;
						DateTime startTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.enableEventDataList[j].eventChapterId].mapDataList[0].questGroupList[0].startTime;
						DateTime endTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.enableEventDataList[j].eventChapterId].mapDataList[0].questGroupList[0].endTime;
						if (startTime >= TimeManager.Now && !this.eventStartTimeMap.ContainsKey(this.enableEventDataList[j].eventChapterId))
						{
							this.eventStartTimeMap.Add(this.enableEventDataList[j].eventChapterId, startTime);
						}
						if (this.enableEventDataList[j].IsEnableChapter)
						{
							this.guiData.questTop.eventPartsList[j].bannerText.text = startTime.ToString("M/d") + "～" + endTime.ToString("M/d HH:mm まで");
							this.guiData.questTop.eventPartsList[j].Mark_EventOpen.gameObject.SetActive(true);
							this.guiData.questTop.eventPartsList[j].Mark_EventBefore.gameObject.SetActive(false);
						}
						else
						{
							this.guiData.questTop.eventPartsList[j].bannerText.text = startTime.ToString("M/d HH:mm から開始！");
							this.guiData.questTop.eventPartsList[j].Mark_EventOpen.gameObject.SetActive(false);
							this.guiData.questTop.eventPartsList[j].Mark_EventBefore.gameObject.SetActive(true);
						}
					}
				}
				else
				{
					this.guiData.questTop.eventButton[j].gameObject.SetActive(false);
				}
			}
			this.requestQuestCmd = Singleton<SceneManager>.Instance.StartCoroutine(this.RequestQuestCmd());
		}
		else
		{
			this.guiData.questTop.EventGroup.SetActive(false);
			this.guiData.questTop.renderTextureChara.postion = new Vector2(405f, -92f);
			this.guiData.questTop.renderTextureChara.rotation = new Vector3(0f, 13f, 0f);
			this.guiData.questTop.renderTextureChara.fieldOfView = 21f;
			this.guiData.questTop.renderTextureChara.OnValidate();
		}
		int num = QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.GROW);
		int num2 = QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.CHARA);
		int num3 = QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.SIDE_STORY);
		int num4 = QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.TRAINING);
		int num5 = QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.ETCETERA);
		List<int> playableQuestIdList = DataManager.DmQuest.GetPlayableQuestIdList(true);
		int[] array = new int[] { num, num2, num3, num4, num5 };
		GameObject[] array2 = new GameObject[]
		{
			this.guiData.questTop.GrowQuest_Mark_New.gameObject,
			this.guiData.questTop.CharQuest_Mark_New.gameObject,
			this.guiData.questTop.AnotherStory_Mark_New.gameObject,
			this.guiData.questTop.Training_Mark_New.gameObject,
			this.guiData.questTop.EtceteraQuest_Mark_New.gameObject
		};
		for (int k = 0; k < array.Length; k++)
		{
			if (!playableQuestIdList.Contains(array[k]) && (DataManager.DmQuest.GetQuestOnePackData(array[k]).questChapter.category != QuestStaticChapter.Category.TUTORIAL || DataManager.DmUserInfo.tutorialSequence != TutorialUtil.Sequence.END))
			{
				array2[k].SetActive(false);
			}
		}
		QuestOnePackData questOnePackData2 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.GROW));
		string text;
		if (questOnePackData2 != null)
		{
			string mainStoryName = SceneQuest.GetMainStoryName(questOnePackData2.questChapter.category, true);
			text = mainStoryName + ((mainStoryName != "") ? "\n" : "") + questOnePackData2.questChapter.chapterName + questOnePackData2.questGroup.titleName + PrjUtil.MakeMessage("クリア");
		}
		else
		{
			text = "クエスト情報がありません";
		}
		MarkLockCtrl markLockGrowQuest = this.guiData.questTop.markLockGrowQuest;
		MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();
		setupParam.updateConditionCallback = () => QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.GROW);
		setupParam.releaseFlag = userFlagData.ReleaseModeFlag.GrowthQuest == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
		setupParam.tagetObject = this.guiData.questTop.Btn_GrowQuest.gameObject;
		setupParam.text = text;
		setupParam.updateUserFlagDataCallback = delegate
		{
			userFlagData.ReleaseModeFlag.GrowthQuest = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		};
		markLockGrowQuest.Setup(setupParam, true);
		QuestOnePackData questOnePackData3 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.ETCETERA));
		string text2;
		if (questOnePackData3 != null)
		{
			string mainStoryName2 = SceneQuest.GetMainStoryName(questOnePackData3.questChapter.category, true);
			text2 = mainStoryName2 + ((mainStoryName2 != "") ? "\n" : "") + questOnePackData3.questChapter.chapterName + questOnePackData3.questGroup.titleName + PrjUtil.MakeMessage("クリア");
		}
		else
		{
			text2 = "クエスト情報がありません";
		}
		MarkLockCtrl markLockEtceteraQuest = this.guiData.questTop.markLockEtceteraQuest;
		MarkLockCtrl.SetupParam setupParam2 = new MarkLockCtrl.SetupParam();
		setupParam2.updateConditionCallback = () => QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.ETCETERA);
		setupParam2.releaseFlag = userFlagData.ReleaseModeFlag.EtceteraQuest == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
		setupParam2.tagetObject = this.guiData.questTop.Btn_EtceteraQuest.gameObject;
		setupParam2.text = text2;
		setupParam2.updateUserFlagDataCallback = delegate
		{
			userFlagData.ReleaseModeFlag.EtceteraQuest = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		};
		markLockEtceteraQuest.Setup(setupParam2, true);
		QuestOnePackData questOnePackData4 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.CHARA));
		string text3;
		if (questOnePackData4 != null)
		{
			string mainStoryName3 = SceneQuest.GetMainStoryName(questOnePackData4.questChapter.category, true);
			text3 = mainStoryName3 + ((mainStoryName3 != "") ? "\n" : "") + questOnePackData4.questChapter.chapterName + questOnePackData4.questGroup.titleName + PrjUtil.MakeMessage("クリア");
		}
		else
		{
			text3 = "クエスト情報がありません";
		}
		MarkLockCtrl markLockCharaQuest = this.guiData.questTop.markLockCharaQuest;
		MarkLockCtrl.SetupParam setupParam3 = new MarkLockCtrl.SetupParam();
		setupParam3.updateConditionCallback = () => QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.CHARA);
		setupParam3.releaseFlag = userFlagData.ReleaseModeFlag.FriendsStory == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
		setupParam3.tagetObject = this.guiData.questTop.Btn_CharQuest.gameObject;
		setupParam3.text = text3;
		setupParam3.updateUserFlagDataCallback = delegate
		{
			userFlagData.ReleaseModeFlag.FriendsStory = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		};
		markLockCharaQuest.Setup(setupParam3, true);
		QuestOnePackData questOnePackData5 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.SIDE_STORY));
		string text4;
		if (questOnePackData5 != null)
		{
			string mainStoryName4 = SceneQuest.GetMainStoryName(questOnePackData5.questChapter.category, true);
			text4 = mainStoryName4 + ((mainStoryName4 != "") ? "\n" : "") + questOnePackData5.questChapter.chapterName + questOnePackData5.questGroup.titleName + PrjUtil.MakeMessage("クリア");
		}
		else
		{
			text4 = "クエスト情報がありません";
		}
		MarkLockCtrl markLockAnotherStoryQuest = this.guiData.questTop.markLockAnotherStoryQuest;
		MarkLockCtrl.SetupParam setupParam4 = new MarkLockCtrl.SetupParam();
		setupParam4.updateConditionCallback = () => QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.SIDE_STORY);
		setupParam4.releaseFlag = userFlagData.ReleaseModeFlag.AraiDiary == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
		setupParam4.tagetObject = this.guiData.questTop.Btn_AnotherStory.gameObject;
		setupParam4.text = text4;
		setupParam4.updateUserFlagDataCallback = delegate
		{
			userFlagData.ReleaseModeFlag.AraiDiary = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		};
		markLockAnotherStoryQuest.Setup(setupParam4, true);
		QuestOnePackData questOnePackData6 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.TRAINING));
		string text5;
		if (questOnePackData6 != null)
		{
			string mainStoryName5 = SceneQuest.GetMainStoryName(questOnePackData6.questChapter.category, true);
			text5 = mainStoryName5 + ((mainStoryName5 != "") ? "\n" : "") + questOnePackData6.questChapter.chapterName + questOnePackData6.questGroup.titleName + PrjUtil.MakeMessage("クリア");
		}
		else
		{
			text5 = "クエスト情報がありません";
		}
		MarkLockCtrl markLockTrainingQuest = this.guiData.questTop.markLockTrainingQuest;
		MarkLockCtrl.SetupParam setupParam5 = new MarkLockCtrl.SetupParam();
		setupParam5.updateConditionCallback = () => QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.TRAINING);
		setupParam5.releaseFlag = userFlagData.ReleaseModeFlag.TrainingByQuestTop == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
		setupParam5.tagetObject = this.guiData.questTop.Btn_Training.gameObject;
		setupParam5.text = text5;
		setupParam5.updateUserFlagDataCallback = delegate
		{
			userFlagData.ReleaseModeFlag.TrainingByQuestTop = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		};
		markLockTrainingQuest.Setup(setupParam5, true);
		QuestOnePackData questOnePackData7 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.ASSISTANT));
		string text6;
		if (questOnePackData7 != null)
		{
			string mainStoryName6 = SceneQuest.GetMainStoryName(questOnePackData7.questChapter.category, true);
			text6 = mainStoryName6 + ((mainStoryName6 != "") ? "\n" : "") + questOnePackData7.questChapter.chapterName + questOnePackData7.questGroup.titleName + PrjUtil.MakeMessage("クリア");
		}
		else
		{
			text6 = "クエスト情報がありません";
		}
		MarkLockCtrl markLockAssistantEdit = this.guiData.questTop.markLockAssistantEdit;
		MarkLockCtrl.SetupParam setupParam6 = new MarkLockCtrl.SetupParam();
		setupParam6.updateConditionCallback = () => QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.ASSISTANT);
		setupParam6.releaseFlag = userFlagData.ReleaseModeFlag.QuestAssistantOpen == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
		setupParam6.tagetObject = this.guiData.questTop.Btn_AssistantEdit.gameObject;
		setupParam6.text = text6;
		setupParam6.updateUserFlagDataCallback = delegate
		{
			userFlagData.ReleaseModeFlag.QuestAssistantOpen = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		};
		markLockAssistantEdit.Setup(setupParam6, true);
		this.SetupQuestTopDecorationCellval(userFlagData);
		this.guiData.questTop.Btn_AssistantEdit.transform.SetAsLastSibling();
		this.guiData.questTop.selAssistantCtrl.transform.SetAsLastSibling();
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x00117DBA File Offset: 0x00115FBA
	private IEnumerator SetQuestTopWindow(DataManagerGameStatus.UserFlagData userFlagData)
	{
		List<DataManagerServerMst.ModeReleaseData> list = DataManager.DmServerMst.ModeReleaseDataList.FindAll((DataManagerServerMst.ModeReleaseData item) => DataManager.DmQuest.GetQuestOnePackData(item.QuestId).questChapter.category == QuestStaticChapter.Category.TUTORIAL);
		if (DataManager.DmUserInfo.tutorialSequence == TutorialUtil.Sequence.END && list.Count > 0)
		{
			list.Sort((DataManagerServerMst.ModeReleaseData a, DataManagerServerMst.ModeReleaseData b) => a.Category - b.Category);
			using (List<DataManagerServerMst.ModeReleaseData>.Enumerator enumerator = list.GetEnumerator())
			{
				PguiOpenWindowCtrl.Callback <>9__4;
				while (enumerator.MoveNext())
				{
					DataManagerServerMst.ModeReleaseData e = enumerator.Current;
					this.touchScreenAuth = false;
					if (!delegate
					{
						DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus lockStatus = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
						DataManagerServerMst.ModeReleaseData.ModeCategory category2 = e.Category;
						if (category2 != DataManagerServerMst.ModeReleaseData.ModeCategory.Cellval)
						{
							if (category2 != DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory2)
							{
								if (category2 == DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory3)
								{
									lockStatus = (QuestUtil.IsUnLockInformationMainStory3(TimeManager.Now) ? userFlagData.ReleaseModeFlag.MainStory3 : DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked);
								}
							}
							else
							{
								lockStatus = (QuestUtil.IsUnLockInformationMainStory2(TimeManager.Now) ? userFlagData.ReleaseModeFlag.MainStory2 : DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked);
							}
						}
						else
						{
							lockStatus = (QuestUtil.IsUnLockInformationCellvalQuest(TimeManager.Now) ? userFlagData.ReleaseModeFlag.CellvalQuest : DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked);
						}
						return lockStatus >= DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
					}())
					{
						bool flag = false;
						int originQuestId = e.OriginQuestId;
						Dictionary<int, QuestDynamicQuestOne> oneDataMap = DataManager.DmQuest.QuestDynamicData.oneDataMap;
						if (originQuestId != 0 && oneDataMap.ContainsKey(originQuestId) && (oneDataMap[originQuestId].status == QuestOneStatus.CLEAR || oneDataMap[originQuestId].status == QuestOneStatus.COMPLETE))
						{
							flag = true;
						}
						if (!flag)
						{
							PguiOpenWindowCtrl hdlOpenWindowBasic = CanvasManager.HdlOpenWindowBasic;
							string windowTitle = e.WindowTitle;
							string windowText = e.WindowText;
							List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> buttonPreset = PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE);
							bool flag2 = true;
							PguiOpenWindowCtrl.Callback callback;
							if ((callback = <>9__4) == null)
							{
								callback = (<>9__4 = delegate(int index)
								{
									this.touchScreenAuth = true;
									return true;
								});
							}
							hdlOpenWindowBasic.Setup(windowTitle, windowText, buttonPreset, flag2, callback, null, false);
							CanvasManager.HdlOpenWindowBasic.Open();
						}
						DataManagerServerMst.ModeReleaseData.ModeCategory category = e.Category;
						if (category != DataManagerServerMst.ModeReleaseData.ModeCategory.Cellval)
						{
							if (category != DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory2)
							{
								if (category == DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory3)
								{
									userFlagData.ReleaseModeFlag.MainStory3 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
								}
							}
							else
							{
								userFlagData.ReleaseModeFlag.MainStory2 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
							}
						}
						else
						{
							userFlagData.ReleaseModeFlag.CellvalQuest = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
						}
						DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
						this.UpdateBtnStorySelect(userFlagData);
						if (!flag)
						{
							while (!this.touchScreenAuth)
							{
								yield return null;
							}
							while (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
							{
								yield return null;
							}
						}
					}
				}
			}
			List<DataManagerServerMst.ModeReleaseData>.Enumerator enumerator = default(List<DataManagerServerMst.ModeReleaseData>.Enumerator);
		}
		if (QuestUtil.IsUnLockInformationCellvalQuest(TimeManager.Now) && userFlagData.ReleaseModeFlag.CellvalQuestOpen == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked)
		{
			CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/Cellval/tutorial_Cellval_01" }, delegate(bool sw)
			{
				PguiAECtrl component = this.guiData.questTop.storyQuestParts.Btn_StorySelectR.transform.Find("AEImage_BtnLight").GetComponent<PguiAECtrl>();
				PguiAECtrl component2 = this.guiData.questTop.storyQuestParts.Btn_StorySelectL.transform.Find("AEImage_BtnLight").GetComponent<PguiAECtrl>();
				component.gameObject.SetActive(true);
				component2.gameObject.SetActive(true);
				component.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				component2.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			});
			userFlagData.ReleaseModeFlag.CellvalQuestOpen = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		}
		yield break;
		yield break;
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x00117DD0 File Offset: 0x00115FD0
	private static bool SetDispNew(List<int> playableList)
	{
		Dictionary<int, QuestStaticMap> mapDataMap = DataManager.DmQuest.QuestStaticData.mapDataMap;
		Dictionary<int, QuestDynamicQuestOne> oneDataMap = DataManager.DmQuest.QuestDynamicData.oneDataMap;
		foreach (int num in playableList)
		{
			if (mapDataMap.ContainsKey(num))
			{
				foreach (QuestStaticQuestGroup questStaticQuestGroup in mapDataMap[num].questGroupList)
				{
					foreach (QuestStaticQuestOne questStaticQuestOne in questStaticQuestGroup.questOneList)
					{
						if (oneDataMap.ContainsKey(questStaticQuestOne.questId) && oneDataMap[questStaticQuestOne.questId].status == QuestOneStatus.NEW)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001650 RID: 5712 RVA: 0x00117EF4 File Offset: 0x001160F4
	private static bool SetDispNew(List<QuestStaticMap> playableList)
	{
		Dictionary<int, QuestDynamicQuestOne> oneDataMap = DataManager.DmQuest.QuestDynamicData.oneDataMap;
		foreach (QuestStaticMap questStaticMap in playableList)
		{
			foreach (QuestStaticQuestGroup questStaticQuestGroup in questStaticMap.questGroupList)
			{
				foreach (QuestStaticQuestOne questStaticQuestOne in questStaticQuestGroup.questOneList)
				{
					if (oneDataMap.ContainsKey(questStaticQuestOne.questId) && oneDataMap[questStaticQuestOne.questId].status == QuestOneStatus.NEW)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001651 RID: 5713 RVA: 0x00117FF4 File Offset: 0x001161F4
	private void UpdateMapdata()
	{
		if (!this.IsNotNullMainStoryObj())
		{
			return;
		}
		this.selMainStoryCtrl.UpdateMapdata(this.selectData.chapterId, this.selectData.questCategory, new UnityAction<Transform>(this.OnClickButtonPointSelect));
	}

	// Token: 0x06001652 RID: 5714 RVA: 0x0011802C File Offset: 0x0011622C
	private void UpdateSwitchSelector()
	{
		this.SwitchDifficultButton();
		this.UpdateButtonLR();
		if (this.IsNotNullMapBaseObj() && this.selMainStoryCtrl.GuiData.mapData.baseObj.activeSelf)
		{
			if (this.selectData.questCategory != QuestStaticChapter.Category.EVENT)
			{
				this.UpdateMapdata();
			}
		}
		else if (this.IsLargeEvent() && this.IsNotNullEventLargeScaleMapBaseObj() && this.selEventLargeScaleCtrl.MapData.baseObj.activeSelf)
		{
			DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId);
			this.selEventLargeScaleCtrl.SetupMapData(this.currentSequence == this.guiData.questTop.baseObj, new UnityAction<Transform>(this.OnClickButtonPointSelectLargeEvent), this.selectData);
		}
		if (this.guiData.questTop.baseObj.activeSelf)
		{
			this.UpdateQuestTopDecoration();
			return;
		}
		if (this.IsNotNullSideStoryObj() && this.selSideStoryCtrl.GuiData.mapSelect.baseObj.activeSelf)
		{
			this.selSideStoryCtrl.UpdateDecoration();
			this.selSideStoryCtrl.GuiData.mapSelect.ResetCampaignInfoCategory();
			this.selSideStoryCtrl.GuiData.mapSelect.UpdateCampaignInfoCategory(this.selectData.chapterId);
			return;
		}
		if (this.IsNotNullEtcetraStoryObj() && this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj.activeSelf)
		{
			this.selEtcetraStoryCtrl.UpdateDecoration();
			this.selEtcetraStoryCtrl.GuiData.mapSelect.ResetCampaignInfoCategory();
			this.selEtcetraStoryCtrl.GuiData.mapSelect.UpdateCampaignInfoCategory(this.selectData.chapterId);
			return;
		}
		if (this.IsNotNullEventScenarioObj() && this.selEventScenarioCtrl.GuiData.eventSelect.baseObj.activeSelf)
		{
			this.selEventScenarioCtrl.UpdateDecoration(!this.isBackToMap, false);
			this.isBackToMap = false;
			return;
		}
		if (this.IsNotNullEventTowerObj() && this.selEventTowerCtrl.GuiData.eventSelect.baseObj.activeSelf)
		{
			this.selEventTowerCtrl.UpdateDecoration();
			return;
		}
		if (this.IsNotNullEventCharaGrowObj() && (this.selEventCharaGrowCtrl.GuiData.charaSelect.baseObj.activeSelf || this.selEventCharaGrowCtrl.GuiData.chapterSelect.baseObj.activeSelf))
		{
			this.selEventCharaGrowCtrl.UpdateDecoration();
			return;
		}
		if (this.IsNotNullEventLargeScaleObj() && this.selEventLargeScaleCtrl.GuiData.pointSelect.baseObj.activeSelf)
		{
			this.selEventLargeScaleCtrl.UpdateDecoration();
			return;
		}
		if (this.IsNotNullEventCoopObj() && this.selEventCoopCtrl.GuiData.mapSelect.baseObj.activeSelf)
		{
			this.selEventCoopCtrl.UpdateDecoration();
			return;
		}
		if (this.IsNotNullEventWildReleaseObj() && this.selEventWildReleaseCtrl.GuiData.questSelect.baseObj.activeSelf)
		{
			this.selEventWildReleaseCtrl.UpdateDecoration();
			return;
		}
		if (this.IsNotNullMainStoryObj())
		{
			bool activeSelf = this.selMainStoryCtrl.GuiData.pointSelect.baseObj.activeSelf;
		}
	}

	// Token: 0x06001653 RID: 5715 RVA: 0x0011834C File Offset: 0x0011654C
	private static bool IsEventCategoryLarge(DataManagerEvent.EventData eventData)
	{
		return eventData != null && eventData.eventCategory == DataManagerEvent.Category.Large;
	}

	// Token: 0x06001654 RID: 5716 RVA: 0x0011835C File Offset: 0x0011655C
	private static bool IsEventCategoryScenario(DataManagerEvent.EventData eventData)
	{
		return eventData != null && eventData.eventCategory == DataManagerEvent.Category.Scenario;
	}

	// Token: 0x06001655 RID: 5717 RVA: 0x0011836C File Offset: 0x0011656C
	private static bool IsEventCategoryGrowth(DataManagerEvent.EventData eventData)
	{
		return eventData != null && eventData.eventCategory == DataManagerEvent.Category.Growth;
	}

	// Token: 0x06001656 RID: 5718 RVA: 0x0011837C File Offset: 0x0011657C
	private static bool IsEventCategoryTower(DataManagerEvent.EventData eventData)
	{
		return eventData != null && eventData.eventCategory == DataManagerEvent.Category.Tower;
	}

	// Token: 0x06001657 RID: 5719 RVA: 0x0011838C File Offset: 0x0011658C
	private static bool IsEventCategoryCoop(DataManagerEvent.EventData eventData)
	{
		return eventData != null && eventData.eventCategory == DataManagerEvent.Category.Coop;
	}

	// Token: 0x06001658 RID: 5720 RVA: 0x0011839C File Offset: 0x0011659C
	private static bool IsEventCategoryWildRelease(DataManagerEvent.EventData eventData)
	{
		return eventData != null && eventData.eventCategory == DataManagerEvent.Category.WildRelease;
	}

	// Token: 0x06001659 RID: 5721 RVA: 0x001183AC File Offset: 0x001165AC
	private bool IsLargeEvent()
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.selectData.eventId);
		return this.selectData.questCategory == QuestStaticChapter.Category.EVENT && this.IsNotNullEventLargeScaleObj() && SceneQuest.IsEventCategoryLarge(eventData);
	}

	// Token: 0x0600165A RID: 5722 RVA: 0x001183F0 File Offset: 0x001165F0
	private bool IsScenarioEvent()
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.selectData.eventId);
		return this.selectData.questCategory == QuestStaticChapter.Category.EVENT && this.IsNotNullEventScenarioObj() && SceneQuest.IsEventCategoryScenario(eventData);
	}

	// Token: 0x0600165B RID: 5723 RVA: 0x00118434 File Offset: 0x00116634
	private bool IsCharaGrowEvent()
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.selectData.eventId);
		return this.selectData.questCategory == QuestStaticChapter.Category.EVENT && this.IsNotNullEventCharaGrowObj() && SceneQuest.IsEventCategoryGrowth(eventData);
	}

	// Token: 0x0600165C RID: 5724 RVA: 0x00118478 File Offset: 0x00116678
	private bool IsTowerEvent()
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.selectData.eventId);
		return this.selectData.questCategory == QuestStaticChapter.Category.EVENT && this.IsNotNullEventTowerObj() && SceneQuest.IsEventCategoryTower(eventData);
	}

	// Token: 0x0600165D RID: 5725 RVA: 0x001184BC File Offset: 0x001166BC
	private bool IsCoopEvent()
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.selectData.eventId);
		return this.selectData.questCategory == QuestStaticChapter.Category.EVENT && this.IsNotNullEventCoopObj() && SceneQuest.IsEventCategoryCoop(eventData);
	}

	// Token: 0x0600165E RID: 5726 RVA: 0x00118500 File Offset: 0x00116700
	private bool IsWildReleaseEvent()
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.selectData.eventId);
		return this.selectData.questCategory == QuestStaticChapter.Category.EVENT && this.IsNotNullEventWildReleaseObj() && SceneQuest.IsEventCategoryWildRelease(eventData);
	}

	// Token: 0x0600165F RID: 5727 RVA: 0x00118541 File Offset: 0x00116741
	private bool IsCoopEventBonusStage()
	{
		return this.IsCoopEvent() && this.selEventCoopCtrl.IsBonus(this.selectData.mapId);
	}

	// Token: 0x06001660 RID: 5728 RVA: 0x00118563 File Offset: 0x00116763
	private bool IsCoopEventNormalStage()
	{
		return this.IsCoopEvent() && !this.selEventCoopCtrl.IsBonus(this.selectData.mapId);
	}

	// Token: 0x06001661 RID: 5729 RVA: 0x00118588 File Offset: 0x00116788
	private bool EnableOffsetQuestButtonGroup()
	{
		return !this.IsCoopEventBonusStage() && this.IsCoopEvent();
	}

	// Token: 0x06001662 RID: 5730 RVA: 0x0011859F File Offset: 0x0011679F
	private bool CheckButtonEventAllImage()
	{
		return this.buttonEventAllCount % 2 == 0;
	}

	// Token: 0x06001663 RID: 5731 RVA: 0x001185AC File Offset: 0x001167AC
	private void UpdateButtonEventAllImage()
	{
		bool sw = this.CheckButtonEventAllImage();
		this.guiData.questTop.Img_Yaji_Down.gameObject.SetActive(!sw);
		this.guiData.questTop.Img_Yaji_Up.gameObject.SetActive(sw);
		if (!sw)
		{
			this.guiData.questTop.Window_EventAll.gameObject.SetActive(!sw);
			this.guiData.questTop.Window_EventAll.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
			{
			});
			return;
		}
		this.guiData.questTop.Window_EventAll.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			this.guiData.questTop.Window_EventAll.gameObject.SetActive(!sw);
		});
	}

	// Token: 0x06001664 RID: 5732 RVA: 0x00118698 File Offset: 0x00116898
	private IEnumerator RequestEventCharaGrow(DataManagerEvent.EventData eventData)
	{
		DataManager.DmEvent.RequestGetGrowthEventCharaId(eventData.eventId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001665 RID: 5733 RVA: 0x001186A7 File Offset: 0x001168A7
	private IEnumerator WaitNextSequenceEventCharaGrow()
	{
		this.reqNextSequence = null;
		yield return this.RequestEventCharaGrow(this.currentEnableEventData);
		this.SetupNextSequenceEventCharaGrow(true);
		yield break;
	}

	// Token: 0x06001666 RID: 5734 RVA: 0x001186B8 File Offset: 0x001168B8
	private void SetupNextSequenceEventCharaGrow(bool callWaitNextSequenceEventCharaGrow)
	{
		if (callWaitNextSequenceEventCharaGrow)
		{
			this.reqNextSequence = ((SelEventCharaGrowCtrl.IsNextSequenceCharaSelect(this.currentEnableEventData.eventId) || SelEventCharaGrowCtrl.IsResettable(this.currentEnableEventData.eventId)) ? this.selEventCharaGrowCtrl.GuiData.charaSelect.baseObj : this.guiData.chapterSelect.baseObj);
		}
		else
		{
			this.reqNextSequence = this.guiData.chapterSelect.baseObj;
		}
		this.selEventCharaGrowCtrl.SetActiveChapterSelect(true);
		if (this.reqNextSequence == this.selEventCharaGrowCtrl.GuiData.charaSelect.baseObj)
		{
			this.selEventCharaGrowCtrl.RequestTutorialWindow();
		}
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x0011876A File Offset: 0x0011696A
	private IEnumerator InstantiateAssetData(IEnumerator cb, string path)
	{
		this.LoadAssetPath = path;
		yield return cb;
		if (!this.IsLargeEvent() && !SceneQuest.IsMainStory(this.selectData.questCategory))
		{
			this.requestSequenceInstantiateAssetData = null;
			yield break;
		}
		if (this.instMapObj != null)
		{
			this.DestroyMap();
			this.instCarObjList = SceneQuest.CreateCarObjList(SceneQuest.mapBoxObject);
		}
		SceneQuest.mapBoxObject.SetActive(true);
		this.instMapObj = AssetManager.InstantiateAssetData(this.LoadAssetPath, SceneQuest.mapBoxObject.transform);
		if (this.instMapObj != null)
		{
			RectTransform rectTransform = this.instMapObj.transform as RectTransform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.offsetMin = new Vector2(0f, 0f);
			rectTransform.offsetMax = new Vector2(0f, 0f);
		}
		if (this.IsLargeEvent())
		{
			if (DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId).Count > 0)
			{
				List<QuestStaticMap> mapDataList = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.selectData.chapterId].mapDataList;
				QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.selectData.chapterId];
				this.selEventLargeScaleCtrl.GuiData.SetupMapData(this.instMapObj, this.instCarObjList);
				this.selEventLargeScaleCtrl.SetupMapData(this.currentSequence == this.guiData.questTop.baseObj, new UnityAction<Transform>(this.OnClickButtonPointSelectLargeEvent), this.selectData);
				this.selEventLargeScaleCtrl.SetupMapOffset();
			}
		}
		else
		{
			this.selMainStoryCtrl.GuiData.mapData = SelMainStoryCtrl.SetupMapGUI(this.instMapObj, this.instCarObjList);
			this.UpdateMapdata();
		}
		this.requestSequenceInstantiateAssetData = null;
		yield break;
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x00118788 File Offset: 0x00116988
	private bool CheckSeal(PguiButtonCtrl button)
	{
		bool flag = false;
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed @sealed = ((homeCheckResult != null) ? homeCheckResult.sealedData : null) ?? new Sealed();
		if (button == this.guiData.questTop.Btn_GrowQuest)
		{
			flag = @sealed.quest_grow == 1;
		}
		else if (button == this.guiData.questTop.storyQuestParts.Btn_StoryQuest)
		{
			flag = @sealed.quest_main == 1;
		}
		else if (button == this.guiData.questTop.Btn_CharQuest)
		{
			flag = @sealed.quest_friends == 1;
		}
		else if (button == this.guiData.questTop.Btn_AnotherStory)
		{
			flag = @sealed.quest_side == 1;
		}
		else if (button == this.guiData.questTop.Btn_Training)
		{
			flag = @sealed.quest_training == 1;
		}
		else if (this.guiData.questTop.eventButton.Contains(button) || this.guiData.questTop.eventBannerButton.Contains(button))
		{
			flag = @sealed.quest_event == 1;
		}
		else if (button == this.guiData.questTop.Btn_EtceteraQuest)
		{
			flag = @sealed.quest_special == 1;
		}
		return flag;
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x001188DC File Offset: 0x00116ADC
	private string GetUrlSeal(PguiButtonCtrl button)
	{
		string text = "";
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed @sealed = ((homeCheckResult != null) ? homeCheckResult.sealedData : null) ?? new Sealed();
		if (button == this.guiData.questTop.Btn_GrowQuest)
		{
			text = @sealed.quest_grow_url;
		}
		else if (button == this.guiData.questTop.storyQuestParts.Btn_StoryQuest)
		{
			text = @sealed.quest_main_url;
		}
		else if (button == this.guiData.questTop.Btn_CharQuest)
		{
			text = @sealed.quest_friends_url;
		}
		else if (button == this.guiData.questTop.Btn_AnotherStory)
		{
			text = @sealed.quest_side_url;
		}
		else if (button == this.guiData.questTop.Btn_Training)
		{
			text = @sealed.quest_training_url;
		}
		else if (this.guiData.questTop.eventButton.Contains(button) || this.guiData.questTop.eventBannerButton.Contains(button))
		{
			text = @sealed.quest_event_url;
		}
		else if (button == this.guiData.questTop.Btn_EtceteraQuest)
		{
			text = @sealed.quest_special_url;
		}
		return text;
	}

	// Token: 0x0600166A RID: 5738 RVA: 0x00118A1C File Offset: 0x00116C1C
	private string GetTextSeal(PguiButtonCtrl button)
	{
		string text = "";
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed @sealed = ((homeCheckResult != null) ? homeCheckResult.sealedData : null) ?? new Sealed();
		if (button == this.guiData.questTop.Btn_GrowQuest)
		{
			text = @sealed.quest_grow_text;
		}
		else if (button == this.guiData.questTop.storyQuestParts.Btn_StoryQuest)
		{
			text = @sealed.quest_main_text;
		}
		else if (button == this.guiData.questTop.Btn_CharQuest)
		{
			text = @sealed.quest_friends_text;
		}
		else if (button == this.guiData.questTop.Btn_AnotherStory)
		{
			text = @sealed.quest_side_text;
		}
		else if (button == this.guiData.questTop.Btn_Training)
		{
			text = @sealed.quest_training_text;
		}
		else if (this.guiData.questTop.eventButton.Contains(button) || this.guiData.questTop.eventBannerButton.Contains(button))
		{
			text = @sealed.quest_event_text;
		}
		else if (button == this.guiData.questTop.Btn_EtceteraQuest)
		{
			text = @sealed.quest_special_text;
		}
		return text.Replace("¥n", "\n");
	}

	// Token: 0x0600166B RID: 5739 RVA: 0x00118B6C File Offset: 0x00116D6C
	private IEnumerator NextScene(int questId)
	{
		yield return this.CurrentAnimOut();
		if (this.IsPlayingAnim)
		{
			yield return null;
		}
		this.selectData.questOneId = questId;
		QuestStaticQuestOne questStaticQuestOne = DataManager.DmQuest.QuestStaticData.oneDataMap[questId];
		if (questStaticQuestOne.StoryOnly && !string.IsNullOrEmpty(questStaticQuestOne.scenarioBeforeId))
		{
			SceneScenario.Args args = new SceneScenario.Args();
			args.scenarioName = questStaticQuestOne.scenarioBeforeId;
			args.questId = this.selectData.questOneId;
			args.storyType = 1;
			args.nextSceneName = SceneManager.SceneName.SceneQuest;
			args.nextSceneArgs = new SceneQuest.Args
			{
				selectQuestOneId = questId,
				justBeforeBattle = new SceneQuest.Args.JustBeforeBattle(),
				justBeforeBattle = 
				{
					playQuestId = questId,
					endStatus = DataManagerQuest.BattleEndStatus.CLEAR,
					isMapAllClearEvent = questStaticQuestOne.clearPerformance,
					isFirstClear = (!DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questId) || DataManager.DmQuest.QuestDynamicData.oneDataMap[questId].playNum == 0)
				}
			};
			this.requestNextScene = SceneManager.SceneName.SceneScenario;
			this.requestNextSceneArgs = args;
		}
		else if (this.requestNextScene == SceneManager.SceneName.SceneTraining)
		{
			this.requestNextSceneArgs = null;
		}
		else
		{
			if (this.questArgs != null && this.questArgs.tutorialSequence == TutorialUtil.Sequence.QUEST_GUIDE)
			{
				this.gotoNextStepByTutorial = true;
			}
			SceneBattleSelector.Args args2 = new SceneBattleSelector.Args();
			args2.selectQuestOneId = this.selectData.questOneId;
			DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.selectData.eventId);
			if (eventData != null && eventData.raidFlg && this.selectData.questCategory == QuestStaticChapter.Category.EVENT)
			{
				args2.coopLastUpdatePoint = DataManager.DmEvent.LastCoopInfo.MapInfoMap[this.selectData.mapId].TotalPoint;
			}
			args2.destroyQuestMapDataCB = delegate
			{
				this.DestroyMap();
			};
			args2.setActiveQuestMapDataCB = delegate(bool flag)
			{
				SceneQuest.IMapData mapData = null;
				if (this.IsLargeEvent())
				{
					mapData = this.selEventLargeScaleCtrl.GuiData.mapData;
				}
				else if (this.IsNotNullMainStoryObj())
				{
					mapData = this.selMainStoryCtrl.GuiData.mapData;
				}
				bool activeSelf = this.guiData.basePanel.activeSelf;
				this.guiData.SetActive(flag, mapData);
				GameObject gameObject = SceneQuest.mapBoxObject;
				if (gameObject != null)
				{
					gameObject.SetActive(flag);
				}
				this.guiData.basePanel.SetActive(activeSelf);
				Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.BACK).GetComponent<Blur>().enabled = true;
			};
			this.requestNextScene = SceneManager.SceneName.SceneBattleSelector;
			this.requestNextSceneArgs = args2;
			if (this.IsCharaGrowEvent() && QuestUtil.EnableEventGrowthExpUpFromQuesOneId(questId))
			{
				this.selEventCharaGrowCtrl.OpenAttentionTurotialWindow();
			}
		}
		this.actionCoroutine = null;
		yield break;
	}

	// Token: 0x0600166C RID: 5740 RVA: 0x00118B84 File Offset: 0x00116D84
	private void UpdateButtonLR()
	{
		bool flag = this.questArgs == null || !this.questArgs.jumpQuest;
		this.guiData.chapterSelect.Btn_Yaji_Left.gameObject.SetActive(this.selectData.questCategory != QuestStaticChapter.Category.GROW && flag);
		this.guiData.chapterSelect.Btn_Yaji_Right.gameObject.SetActive(this.selectData.questCategory != QuestStaticChapter.Category.GROW && flag);
		if (this.IsNotNullCharaStoryObj() && this.selectData.questCategory == QuestStaticChapter.Category.CHARA)
		{
			bool flag2 = this.selCharaStoryCtrl.HaveCharaPackList.Exists((CharaPackData item) => item.id == this.selCharaStoryCtrl.SelectCharaId);
			this.guiData.chapterSelect.Btn_Yaji_Left.SetActEnable(true, false, false);
			this.guiData.chapterSelect.Btn_Yaji_Right.SetActEnable(true, false, false);
			this.guiData.chapterSelect.Btn_Yaji_Left.gameObject.SetActive(flag2);
			this.guiData.chapterSelect.Btn_Yaji_Right.gameObject.SetActive(flag2);
			return;
		}
		if (this.IsUseSubAnim() || this.IsLargeEvent())
		{
			this.guiData.chapterSelect.Btn_Yaji_Left.gameObject.SetActive(false);
			this.guiData.chapterSelect.Btn_Yaji_Right.gameObject.SetActive(false);
			return;
		}
		bool flag3 = false;
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId);
		if (playableMapIdList.Exists((int item) => item == this.selectData.mapId))
		{
			int num = playableMapIdList.FindIndex((int item) => item == this.selectData.mapId);
			this.guiData.chapterSelect.Btn_Yaji_Left.gameObject.SetActive(num > 0);
			this.guiData.chapterSelect.Btn_Yaji_Right.gameObject.SetActive(num < playableMapIdList.Count - 1);
		}
		else if (DataManager.DmQuest.GetPlayableMapIdList(this.selectData.questCategory).Exists((int item) => item == this.selectData.mapId))
		{
			QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap[this.selectData.mapId];
			int num2 = this.selectData.chapterId;
			if (this.selectData.chapterId != questStaticMap.chapterId)
			{
				num2 = questStaticMap.chapterId;
			}
			List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(num2);
			if (playableMapIdList2.Exists((int item) => item == this.selectData.mapId))
			{
				int num3 = playableMapIdList2.FindIndex((int item) => item == this.selectData.mapId);
				this.guiData.chapterSelect.Btn_Yaji_Left.gameObject.SetActive(num3 > 0);
				this.guiData.chapterSelect.Btn_Yaji_Right.gameObject.SetActive(num3 < playableMapIdList.Count - 1);
			}
			else
			{
				flag3 = true;
			}
		}
		else
		{
			flag3 = true;
		}
		if (flag3)
		{
			this.guiData.chapterSelect.Btn_Yaji_Left.gameObject.SetActive(false);
			this.guiData.chapterSelect.Btn_Yaji_Right.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600166D RID: 5741 RVA: 0x00118EAC File Offset: 0x001170AC
	private bool IsUseSubAnim()
	{
		return this.selectData.questCategory == QuestStaticChapter.Category.GROW || SceneQuest.IsEventCategoryScenario(this.currentEnableEventData) || SceneQuest.IsEventCategoryGrowth(this.currentEnableEventData) || SceneQuest.IsEventCategoryTower(this.currentEnableEventData) || SceneQuest.IsEventCategoryCoop(this.currentEnableEventData) || SceneQuest.IsEventCategoryWildRelease(this.currentEnableEventData);
	}

	// Token: 0x0600166E RID: 5742 RVA: 0x00118F0A File Offset: 0x0011710A
	private IEnumerator CurrentAnimOut()
	{
		if (this.guiData.locationInfo.baseObj.activeSelf)
		{
			this.guiData.locationInfo.baseObj.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
				this.guiData.locationInfo.baseObj.SetActive(false);
			});
		}
		SimpleAnimation canim = this.currentSequence.GetComponent<SimpleAnimation>();
		if (canim != null && canim.gameObject.activeSelf)
		{
			this.IsPlayingAnim = true;
			if (this.currentSequence == this.guiData.chapterSelect.baseObj)
			{
				canim.ExPlayAnimation(this.IsUseSubAnim() ? SimpleAnimation.ExPguiStatus.END_SUB : SimpleAnimation.ExPguiStatus.END, null);
			}
			else
			{
				canim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			}
			while (canim.ExIsPlaying())
			{
				yield return null;
			}
		}
		if (this.IsNotNullMainStoryObj() && this.selMainStoryCtrl.GuiData.pointSelect.baseObj.activeSelf)
		{
			this.selMainStoryCtrl.GuiData.pointSelect.Mark_Hard.gameObject.SetActive(QuestUtil.IsHardMode(this.selectData));
		}
		if (this.guiData.locationInfo.baseObj.activeSelf)
		{
			this.guiData.locationInfo.Mark_Hard.gameObject.SetActive(QuestUtil.IsHardMode(this.selectData));
		}
		this.IsPlayingAnim = false;
		yield break;
	}

	// Token: 0x0600166F RID: 5743 RVA: 0x00118F19 File Offset: 0x00117119
	private IEnumerator ChangeScene(GameObject nextSequence)
	{
		if (SceneQuest.IsMainStory(this.selectData.questCategory))
		{
			if (this.IsNotNullMainStoryObj() && nextSequence == this.selMainStoryCtrl.GuiData.pointSelect.baseObj)
			{
				SceneQuest.MainStoryPlayBGM();
			}
			else if (nextSequence == this.guiData.questTop.baseObj)
			{
				SceneQuest.DefaultPlayBGM();
			}
		}
		else if (this.IsTowerEvent())
		{
			if (nextSequence == this.selEventTowerCtrl.GuiData.eventSelect.baseObj)
			{
				SelEventTowerCtrl.PlayBGM();
			}
			else if (nextSequence == this.guiData.questTop.baseObj)
			{
				SceneQuest.DefaultPlayBGM();
				CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByTower(false);
			}
		}
		else if (this.IsNotNullSideStoryObj() && this.selectData.questCategory == QuestStaticChapter.Category.SIDE_STORY)
		{
			if (nextSequence == this.selSideStoryCtrl.GuiData.mapSelect.baseObj)
			{
				CanvasManager.SetScenarioBgInSideStoryBgTexture(QuestUtil.ARAI_STORY_BG);
			}
		}
		else if (this.IsNotNullEtcetraStoryObj() && this.selectData.questCategory == QuestStaticChapter.Category.ETCETERA)
		{
			if (nextSequence == this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj)
			{
				CanvasManager.SetBgObj("PanelBg_CharaQuest");
			}
		}
		else if (this.IsLargeEvent())
		{
			if (nextSequence == this.selEventLargeScaleCtrl.GuiData.pointSelect.baseObj)
			{
				this.selEventLargeScaleCtrl.PlayBGM();
			}
			else if (nextSequence == this.guiData.questTop.baseObj)
			{
				SceneQuest.DefaultPlayBGM();
				CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByLarge(false, 0);
			}
		}
		else if (this.IsCoopEvent())
		{
			if (nextSequence == this.selEventCoopCtrl.GuiData.mapSelect.baseObj)
			{
				SelEventCoopCtrl.PlayBGM();
			}
			else if (nextSequence == this.guiData.questTop.baseObj)
			{
				SceneQuest.DefaultPlayBGM();
				CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByCoop(false, 0);
			}
		}
		else if (this.IsWildReleaseEvent())
		{
			if (nextSequence == this.guiData.chapterSelect.baseObj)
			{
				SelEventWildReleaseCtrl.PlayBGM();
			}
			else if (nextSequence == this.guiData.questTop.baseObj)
			{
				SceneQuest.DefaultPlayBGM();
				CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByWildRelease(false);
			}
		}
		else if (this.IsCharaGrowEvent() && !(nextSequence == this.guiData.chapterSelect.baseObj) && nextSequence == this.guiData.questTop.baseObj)
		{
			CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByCharaGrow(false);
		}
		this.UpdateMenuTitle(nextSequence);
		yield return this.CurrentAnimOut();
		bool largeEvent = this.IsLargeEvent();
		bool flag = largeEvent || this.IsCoopEvent() || SceneQuest.IsMainStory(this.selectData.questCategory);
		bool flag2 = SceneQuest.IsMainStory(this.selectData.questCategory) || largeEvent;
		SceneQuest.IMapData mapData = null;
		GameObject gameObject = null;
		if (largeEvent)
		{
			mapData = this.selEventLargeScaleCtrl.MapData;
			gameObject = this.selEventLargeScaleCtrl.GuiData.pointSelect.baseObj;
		}
		else if (this.IsNotNullMainStoryObj())
		{
			mapData = this.selMainStoryCtrl.GuiData.mapData;
			gameObject = this.selMainStoryCtrl.GuiData.pointSelect.baseObj;
		}
		this.guiData.SwitchSelector(new SceneQuest.GUI.SetupSwitchSelectorParam
		{
			nextObj = nextSequence,
			currentObj = this.currentSequence,
			enableBlur = flag,
			isMapNeedQuest = flag2,
			updateCb = new SceneQuest.UpdateCallback(this.UpdateSwitchSelector),
			touchRegistCb = new SceneQuest.TouchCallback(this.OnRegistTouchMove),
			touchReleaseCb = new SceneQuest.TouchCallback(this.OnReleaseTouchMove),
			releaseRegistCb = new SceneQuest.TouchCallback(this.OnRegistTouchRelease),
			releaseReleaseCb = new SceneQuest.TouchCallback(this.OnReleaseTouchRelease),
			startRegistCb = new SceneQuest.TouchCallback(this.OnRegistTouchStart),
			startReleaseCb = new SceneQuest.TouchCallback(this.OnReleaseTouchStart),
			selectData = this.selectData,
			mapDataGameObj = mapData,
			pointSelectObj = gameObject,
			pointSelectActionCB = delegate
			{
				if (this.IsNotNullMainStoryObj())
				{
					this.selMainStoryCtrl.GuiData.pointSelect.ResetCampaignInfoCategory();
					this.selMainStoryCtrl.GuiData.pointSelect.UpdateCampaignInfoCategory(this.selectData.questCategory, this.selectData.chapterId);
				}
			}
		});
		if (nextSequence == this.guiData.chapterSelect.baseObj)
		{
			this.guiData.chapterSelect.Btn_Mission.gameObject.SetActive(this.IsCharaGrowEvent() || this.IsWildReleaseEvent());
			if (this.IsScenarioEvent())
			{
				this.selectData.mapId = this.selEventScenarioCtrl.MapId;
			}
			else if (this.IsTowerEvent())
			{
				this.selectData.mapId = this.selEventTowerCtrl.MapId;
			}
			this.guiData.chapterSelect.SetupOffset(this.EnableOffsetQuestButtonGroup() ? new Vector3(0f, -this.selEventCoopCtrl.GetOffsetPosY(), 0f) : default(Vector3));
			if (this.IsNotNullEventCoopObj())
			{
				this.selEventCoopCtrl.GuiData.questSelect.SetActive(false);
			}
			if (this.IsCoopEvent())
			{
				this.selEventCoopCtrl.SetupQuestSelect();
				this.guiData.chapterSelect.InactiveParts();
				while (!this.selEventCoopCtrl.FinishedRequestGetCoopInfo())
				{
					yield return null;
				}
			}
			this.guiData.chapterSelect.questButtonGroup.Setup(new QuestButtonGroupCtrl.SetupParam
			{
				selectData = this.selectData,
				callback = new QuestButtonGroupCtrl.UpdateChapterChara(this.UpdateChapterChara),
				callbackText = new QuestButtonGroupCtrl.UpdateChapterChara(this.UpdateChapterCharaText),
				offsetPosY = (this.EnableOffsetQuestButtonGroup() ? this.selEventCoopCtrl.GetOffsetPosY() : 0f)
			});
			if (this.selectData.questCategory == QuestStaticChapter.Category.GROW)
			{
				CanvasManager.SetBgObj("PanelBg_GrowQuest");
			}
			else if (this.selectData.questCategory != QuestStaticChapter.Category.CHARA && this.selectData.questCategory == QuestStaticChapter.Category.EVENT && largeEvent)
			{
			}
		}
		else if (this.IsNotNullMainStoryObj() && nextSequence == this.selMainStoryCtrl.GuiData.pointSelect.baseObj)
		{
			this.selMainStoryCtrl.UpdatePointSelect();
		}
		else if (this.IsNotNullCharaStoryObj() && nextSequence == this.selCharaStoryCtrl.GuiData.charaSelect.baseObj)
		{
			this.selCharaStoryCtrl.GuiData.charaSelect.ScrollView.Refresh();
			CanvasManager.SetBgObj("PanelBg_CharaQuest");
			SimpleAnimation component = nextSequence.GetComponent<SimpleAnimation>();
			if (component != null)
			{
				component.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
			}
			yield return null;
		}
		else if (nextSequence == this.guiData.questTop.baseObj)
		{
			CanvasManager.SetBgObj("PanelBg_HomeOut");
		}
		else if ((!this.IsNotNullEventScenarioObj() || !(nextSequence == this.selEventScenarioCtrl.GuiData.eventSelect.baseObj)) && (!this.IsNotNullEventCharaGrowObj() || (!(nextSequence == this.selEventCharaGrowCtrl.GuiData.chapterSelect.baseObj) && !(nextSequence == this.selEventCharaGrowCtrl.GuiData.charaSelect.baseObj))) && (!this.IsNotNullEventTowerObj() || !(nextSequence == this.selEventTowerCtrl.GuiData.eventSelect.baseObj)) && (!this.IsNotNullEventCoopObj() || !(nextSequence == this.selEventCoopCtrl.GuiData.mapSelect.baseObj)) && this.IsNotNullEventWildReleaseObj())
		{
			nextSequence == this.selEventWildReleaseCtrl.GuiData.questSelect.baseObj;
		}
		if (this.guiData.chapterSelect.baseObj == nextSequence)
		{
			this.guiData.locationInfo.baseObj.SetActive(true);
			this.guiData.locationInfo.baseObj.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.UpdateLocation();
		}
		if (this.IsNotNullMainStoryObj() && nextSequence == this.selMainStoryCtrl.GuiData.pointSelect.baseObj && this.currentSequence == this.selMainStoryCtrl.GuiData.pointSelect.baseObj && this.IsNotNullMapBaseObj() && this.selMainStoryCtrl.GuiData.mapData.baseObj.activeSelf)
		{
			this.selMainStoryCtrl.GuiData.mapData.InAnim();
		}
		SimpleAnimation nanim = nextSequence.GetComponent<SimpleAnimation>();
		if (nanim != null)
		{
			this.IsPlayingAnim = true;
			if (nextSequence == this.guiData.chapterSelect.baseObj)
			{
				nanim.ExPlayAnimation(this.IsUseSubAnim() ? SimpleAnimation.ExPguiStatus.START_SUB : SimpleAnimation.ExPguiStatus.START, null);
			}
			else
			{
				nanim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			}
			while (nanim.ExIsPlaying())
			{
				yield return null;
			}
		}
		this.status = SceneQuest.Status.POLLING_REQUEST;
		this.currentSequence = nextSequence;
		this.reqNextSequence = null;
		this.IsPlayingAnim = false;
		yield break;
	}

	// Token: 0x06001670 RID: 5744 RVA: 0x00118F30 File Offset: 0x00117130
	private void UpdateMenuTitle(GameObject scene)
	{
		if (scene == this.guiData.chapterSelect.baseObj)
		{
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("クエスト選択"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuReturn), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
			return;
		}
		if ((this.IsNotNullMainStoryObj() && scene == this.selMainStoryCtrl.GuiData.pointSelect.baseObj) || (this.IsLargeEvent() && scene == this.selEventLargeScaleCtrl.GuiData.pointSelect.baseObj))
		{
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("マップ地点選択"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuReturn), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
			return;
		}
		if (scene == this.guiData.questTop.baseObj)
		{
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("クエストモード選択"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuReturn), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
			return;
		}
		if ((this.IsNotNullCharaStoryObj() && scene == this.selCharaStoryCtrl.GuiData.charaSelect.baseObj) || (this.selEventCharaGrowCtrl != null && scene == this.selEventCharaGrowCtrl.GuiData.charaSelect.baseObj))
		{
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("フレンズ選択"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuReturn), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
			return;
		}
		if (this.IsNotNullEventScenarioObj() && scene == this.selEventScenarioCtrl.GuiData.eventSelect.baseObj)
		{
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("イベント"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuReturn), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
			return;
		}
		if (this.IsNotNullEventTowerObj() && scene == this.selEventTowerCtrl.GuiData.eventSelect.baseObj)
		{
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("イベント"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuReturn), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
			return;
		}
		if (this.IsNotNullEventCoopObj() && scene == this.selEventCoopCtrl.GuiData.mapSelect.baseObj)
		{
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("イベント"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuReturn), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
			return;
		}
		if (this.IsNotNullSideStoryObj() && scene == this.selSideStoryCtrl.GuiData.mapSelect.baseObj)
		{
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("章・話選択"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuReturn), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
			return;
		}
		if (this.IsNotNullEtcetraStoryObj() && scene == this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj)
		{
			CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage(QuestUtil.TitleEtcetera), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuReturn), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		}
	}

	// Token: 0x06001671 RID: 5745 RVA: 0x00119294 File Offset: 0x00117494
	private void UpdateChapterChara(int groupQuestId)
	{
		if (DataManager.DmQuest.QuestStaticData.groupDataMap.ContainsKey(groupQuestId))
		{
			bool flag = !this.IsCharaGrowEvent() && !this.IsCoopEventNormalStage() && !this.IsWildReleaseEvent();
			QuestStaticQuestGroup questStaticQuestGroup = DataManager.DmQuest.QuestStaticData.groupDataMap[groupQuestId];
			bool flag2 = questStaticQuestGroup.CharaList.Exists((QuestStaticQuestGroup.Chara item) => item.DispNum == 1);
			int num;
			if (questStaticQuestGroup.targetCharaId != 0 || !flag2)
			{
				num = questStaticQuestGroup.targetCharaId;
			}
			else
			{
				num = questStaticQuestGroup.CharaList.Find((QuestStaticQuestGroup.Chara item) => item.DispNum == 1).Id;
			}
			int num2 = num;
			if (ItemDef.Id2Kind(num2) == ItemDef.Kind.CHARA)
			{
				CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(num2);
				if (charaStaticData != null)
				{
					if (this.selectData.questCategory == QuestStaticChapter.Category.SIDE_STORY)
					{
						this.guiData.chapterSelect.InactiveParts();
						QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == this.selectData.chapterId);
						this.guiData.sideStory.Txt_Title.text = questStaticChapter.chapterTitle;
						QuestStaticQuestGroup.Chara chara = questStaticQuestGroup.CharaList.Find((QuestStaticQuestGroup.Chara item) => item.DispNum == 1);
						this.guiData.chapterSelect.renderTextureChara.gameObject.SetActive(flag);
						this.guiData.chapterSelect.renderTextureChara.SetupFace(charaStaticData.GetId(), 1, RenderTextureChara.StrBodyMotionId2CharaModionDefineActKey(chara.BodyMotion), FacePackData.Id2PackData(chara.FaceMotion), 0, false, true, null, false);
						this.guiData.chapterSelect.renderTextureChara.postion = SceneQuest.ARAI_STORY_RENDER_TEXTURE_TRANSFORM01.position;
						this.guiData.chapterSelect.renderTextureChara.rotation = SceneQuest.ARAI_STORY_RENDER_TEXTURE_TRANSFORM01.rotation;
						this.guiData.chapterSelect.renderTextureChara.fieldOfView = SceneQuest.ARAI_STORY_RENDER_TEXTURE_TRANSFORM01.fov;
						if (questStaticQuestGroup.CharaList.Exists((QuestStaticQuestGroup.Chara item) => item.DispNum == 2))
						{
							QuestStaticQuestGroup.Chara chara2 = questStaticQuestGroup.CharaList.Find((QuestStaticQuestGroup.Chara item) => item.DispNum == 2);
							CharaStaticData charaStaticData2 = DataManager.DmChara.GetCharaStaticData(chara2.Id);
							this.guiData.chapterSelect.renderTextureChara2.gameObject.SetActive(flag);
							this.guiData.chapterSelect.renderTextureChara2.SetupFace(charaStaticData2.GetId(), 0, RenderTextureChara.StrBodyMotionId2CharaModionDefineActKey(chara2.BodyMotion), FacePackData.Id2PackData(chara2.FaceMotion), 0, false, true, null, false);
							this.guiData.chapterSelect.renderTextureChara2.postion = SceneQuest.ARAI_STORY_RENDER_TEXTURE_TRANSFORM02.position;
							this.guiData.chapterSelect.renderTextureChara2.rotation = SceneQuest.ARAI_STORY_RENDER_TEXTURE_TRANSFORM02.rotation;
							this.guiData.chapterSelect.renderTextureChara2.fieldOfView = SceneQuest.ARAI_STORY_RENDER_TEXTURE_TRANSFORM02.fov;
						}
					}
					else
					{
						this.guiData.chapterSelect.InactiveParts();
						if (this.IsScenarioEvent() || this.IsLargeEvent() || this.IsTowerEvent())
						{
							this.guiData.locationEvent.Txt_CharaName.text = charaStaticData.GetName();
						}
						else
						{
							if (SceneQuest.IsMainStoryPart1_5(this.selectData.questCategory))
							{
								this.guiData.chapterSelect.parts[1].baseObj.SetActive(flag);
								this.guiData.chapterSelect.parts[1].Txt_CharaName.text = charaStaticData.GetName();
							}
							else if (SceneQuest.IsMainStoryPart2(this.selectData.questCategory))
							{
								this.guiData.chapterSelect.parts[2].baseObj.SetActive(flag);
								this.guiData.chapterSelect.parts[2].Txt_CharaName.text = charaStaticData.GetName();
							}
							else if (SceneQuest.IsMainStoryPart3(this.selectData.questCategory))
							{
								this.guiData.chapterSelect.parts[3].baseObj.SetActive(flag);
								this.guiData.chapterSelect.parts[3].Txt_CharaName.text = charaStaticData.GetName();
							}
							else
							{
								this.guiData.chapterSelect.parts[0].baseObj.SetActive(flag);
								this.guiData.chapterSelect.parts[0].Txt_CharaName.text = charaStaticData.GetName();
							}
							this.guiData.locationInfo.Txt_CharaName.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
							{
								charaStaticData.baseData.NickName,
								charaStaticData.GetName()
							});
							if (charaStaticData.baseData.subAttribute <= CharaDef.AttributeType.ALL)
							{
								this.guiData.locationInfo.Txt_CharaName_ReTr.anchoredPosition = this.guiData.locationInfo.Txt_CharaName_Initial_Pos - new Vector2(30f, 0f);
								this.guiData.locationInfo.Txt_CharaName_ReTr.sizeDelta = this.guiData.locationInfo.Txt_CharaName_Initial_Size + new Vector2(30f, 0f);
								this.guiData.locationInfo.Icon_SubAtr.gameObject.SetActive(false);
							}
							else
							{
								this.guiData.locationInfo.Txt_CharaName_ReTr.anchoredPosition = this.guiData.locationInfo.Txt_CharaName_Initial_Pos;
								this.guiData.locationInfo.Icon_SubAtr.SetImageByName(IconCharaCtrl.SubAttribute2IconName(charaStaticData.baseData.subAttribute));
								this.guiData.locationInfo.Icon_SubAtr.gameObject.SetActive(true);
							}
						}
						this.guiData.locationInfo.Icon_Atr.SetImageByName(IconCharaCtrl.Attribute2IconName(charaStaticData.baseData.attribute));
						this.guiData.chapterSelect.renderTextureChara.gameObject.SetActive(flag);
						this.guiData.chapterSelect.renderTextureChara.SetupFace(charaStaticData.GetId(), 1, RenderTextureChara.StrBodyMotionId2CharaModionDefineActKey(questStaticQuestGroup.charaBodyMotionId), FacePackData.Id2PackData(questStaticQuestGroup.charaFaceMotionId), 0, false, true, null, false);
						this.guiData.chapterSelect.renderTextureChara.postion = SceneQuest.DEFAULT_RENDER_TEXTURE_TRANSFORM.position;
						this.guiData.chapterSelect.renderTextureChara.rotation = SceneQuest.DEFAULT_RENDER_TEXTURE_TRANSFORM.rotation;
						this.guiData.chapterSelect.renderTextureChara.fieldOfView = SceneQuest.DEFAULT_RENDER_TEXTURE_TRANSFORM.fov;
						this.guiData.chapterSelect.renderTextureChara2.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				this.guiData.chapterSelect.renderTextureChara2.gameObject.SetActive(false);
				if (num2 == 0)
				{
					this.guiData.chapterSelect.renderTextureChara.gameObject.SetActive(false);
					this.guiData.chapterSelect.InactiveParts();
				}
				else
				{
					string npcname = DataManagerChara.GetNPCName(num2);
					if (npcname != null)
					{
						this.guiData.chapterSelect.InactiveParts();
						if (this.IsScenarioEvent() || this.IsLargeEvent() || this.IsTowerEvent())
						{
							this.guiData.locationEvent.Txt_CharaName.text = npcname;
						}
						else
						{
							if (SceneQuest.IsMainStoryPart1_5(this.selectData.questCategory))
							{
								this.guiData.chapterSelect.parts[1].baseObj.SetActive(flag);
								this.guiData.chapterSelect.parts[1].Txt_CharaName.text = npcname;
							}
							else if (SceneQuest.IsMainStoryPart2(this.selectData.questCategory))
							{
								this.guiData.chapterSelect.parts[2].baseObj.SetActive(flag);
								this.guiData.chapterSelect.parts[2].Txt_CharaName.text = npcname;
							}
							else if (SceneQuest.IsMainStoryPart3(this.selectData.questCategory))
							{
								this.guiData.chapterSelect.parts[3].baseObj.SetActive(flag);
								this.guiData.chapterSelect.parts[3].Txt_CharaName.text = npcname;
							}
							else
							{
								this.guiData.chapterSelect.parts[0].baseObj.SetActive(flag);
								this.guiData.chapterSelect.parts[0].Txt_CharaName.text = npcname;
							}
							this.guiData.locationInfo.Txt_CharaName.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[] { "", npcname });
						}
						this.guiData.chapterSelect.renderTextureChara.gameObject.SetActive(flag);
						this.guiData.chapterSelect.renderTextureChara.SetupFace(num2, 1, RenderTextureChara.StrBodyMotionId2CharaModionDefineActKey(questStaticQuestGroup.charaBodyMotionId), FacePackData.Id2PackData(questStaticQuestGroup.charaFaceMotionId), 0, false, true, null, false);
						this.guiData.chapterSelect.renderTextureChara.postion = SceneQuest.DEFAULT_RENDER_TEXTURE_TRANSFORM.position;
						this.guiData.chapterSelect.renderTextureChara.rotation = SceneQuest.DEFAULT_RENDER_TEXTURE_TRANSFORM.rotation;
						this.guiData.chapterSelect.renderTextureChara.fieldOfView = SceneQuest.DEFAULT_RENDER_TEXTURE_TRANSFORM.fov;
					}
				}
			}
			this.UpdateChapterCharaText(groupQuestId);
		}
	}

	// Token: 0x06001672 RID: 5746 RVA: 0x00119CB0 File Offset: 0x00117EB0
	private void UpdateChapterCharaText(int groupQuestId)
	{
		if (DataManager.DmQuest.QuestStaticData.groupDataMap.ContainsKey(groupQuestId))
		{
			QuestStaticQuestGroup questStaticQuestGroup = DataManager.DmQuest.QuestStaticData.groupDataMap[groupQuestId];
			this.guiData.locationEvent.Txt_Serif.text = (this.guiData.sideStory.Txt_Info.text = questStaticQuestGroup.charaComment);
			foreach (SceneQuest.GUI.ChapterSelect.Parts parts in this.guiData.chapterSelect.parts)
			{
				parts.Txt_Serif.text = questStaticQuestGroup.charaComment;
			}
		}
	}

	// Token: 0x06001673 RID: 5747 RVA: 0x00119D78 File Offset: 0x00117F78
	private void UpdateLocation()
	{
		if (this.selectData.questCategory == QuestStaticChapter.Category.CHARA)
		{
			foreach (SceneQuest.GUI.LocationInfo.Location location in this.guiData.locationInfo.location)
			{
				location.baseObj.SetActive(false);
			}
			this.guiData.locationInfo.CharaNameTr.gameObject.SetActive(true);
			this.guiData.locationInfo.Btn_MoreInfo.gameObject.SetActive(true);
			this.guiData.locationInfo.All.gameObject.SetActive(true);
			this.guiData.locationEvent.baseObj.SetActive(false);
		}
		else if (this.selectData.questCategory == QuestStaticChapter.Category.EVENT)
		{
			if (this.IsCharaGrowEvent() || this.IsCoopEvent() || this.IsWildReleaseEvent())
			{
				this.guiData.locationEvent.baseObj.SetActive(false);
				this.guiData.locationInfo.baseObj.SetActive(false);
			}
			else
			{
				DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.selectData.eventId);
				if (eventData != null)
				{
					this.guiData.locationInfo.All.SetActive(false);
					this.guiData.locationEvent.baseObj.SetActive(true);
					for (int i = 0; i < this.guiData.locationEvent.itemOwnBases.Count; i++)
					{
						SceneQuest.GUI.LocationEvent.ItemOwnBase itemOwnBase = this.guiData.locationEvent.itemOwnBases[i];
						itemOwnBase.baseObj.SetActive(i < eventData.eventCoinIdList.Count);
						if (itemOwnBase.baseObj.activeSelf)
						{
							itemOwnBase.Num_Own.text = DataManager.DmItem.GetUserItemData(eventData.eventCoinIdList[i]).num.ToString();
							itemOwnBase.Icon_Img.SetRawImage(DataManager.DmItem.GetItemStaticBase(eventData.eventCoinIdList[i]).GetIconName(), true, false, null);
						}
					}
					HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(eventData.eventBannerId);
					this.guiData.locationEvent.Banner.banner = homeBannerData.bannerImagePathEvent;
					PguiTextCtrl component = this.guiData.locationEvent.Btn_ShopEvent.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
					if (this.IsTowerEvent())
					{
						component.text = "イベント\nしょうたい";
						component.m_Text.fontSize = 21;
					}
					else
					{
						component.text = "ショップ";
						component.m_Text.fontSize = 25;
					}
				}
			}
		}
		else
		{
			foreach (SceneQuest.GUI.LocationInfo.Location location2 in this.guiData.locationInfo.location)
			{
				location2.baseObj.SetActive(false);
			}
			foreach (int num in DataManager.DmQuest.GetPlayableMapIdList(this.selectData.questCategory))
			{
				QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap[num];
				if (questStaticMap.mapId == this.selectData.mapId)
				{
					if (SceneQuest.IsMainStoryPart1_5(this.selectData.questCategory))
					{
						this.guiData.locationInfo.location[1].baseObj.SetActive(true);
						this.guiData.locationInfo.location[1].Txt_Location.text = questStaticMap.mapName;
					}
					else if (SceneQuest.IsMainStoryPart2(this.selectData.questCategory))
					{
						this.guiData.locationInfo.location[2].baseObj.SetActive(true);
						this.guiData.locationInfo.location[2].Txt_Location.text = questStaticMap.mapName;
					}
					else if (SceneQuest.IsMainStoryPart3(this.selectData.questCategory))
					{
						this.guiData.locationInfo.location[3].baseObj.SetActive(true);
						this.guiData.locationInfo.location[3].Txt_Location.text = questStaticMap.mapName;
					}
					else
					{
						this.guiData.locationInfo.location[0].baseObj.SetActive(true);
						this.guiData.locationInfo.location[0].Txt_Location.text = questStaticMap.mapName;
					}
					if (this.selectData.questCategory == QuestStaticChapter.Category.GROW)
					{
						for (int j = 0; j < questStaticMap.dispItemIconId.Count; j++)
						{
							int num2 = questStaticMap.dispItemIconId[j];
							if (j < this.guiData.charaGrow.Icon_Item.Count)
							{
								ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(num2);
								this.guiData.charaGrow.Icon_Item[j].Setup(itemStaticBase, new IconItemCtrl.SetupParam
								{
									useFrame = false
								});
								this.guiData.charaGrow.Icon_Item[j].transform.parent.gameObject.SetActive(itemStaticBase != null);
							}
						}
						break;
					}
					break;
				}
			}
			this.guiData.locationInfo.CharaNameTr.gameObject.SetActive(false);
			this.guiData.locationInfo.Btn_MoreInfo.gameObject.SetActive(false);
			this.guiData.locationInfo.All.SetActive(true);
			this.guiData.locationEvent.baseObj.SetActive(false);
		}
		if (this.guiData.locationInfo.baseObj.activeSelf)
		{
			this.guiData.locationInfo.Mark_Hard.gameObject.SetActive(QuestUtil.IsHardMode(this.selectData));
		}
	}

	// Token: 0x06001674 RID: 5748 RVA: 0x0011A40C File Offset: 0x0011860C
	public static List<KeyValuePair<int, QuestStaticChapter>> GetChapterDataByCategory(QuestStaticChapter.Category category, int mode = -1)
	{
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(category);
		Dictionary<int, QuestStaticChapter> dictionary = new Dictionary<int, QuestStaticChapter>();
		List<QuestStaticChapter> list = DataManager.DmQuest.QuestStaticData.chapterDataList.FindAll((QuestStaticChapter item) => item.hardChapterId != 0);
		List<QuestStaticChapter> list2 = new List<QuestStaticChapter>();
		using (List<int>.Enumerator enumerator = playableMapIdList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int e2 = enumerator.Current;
				QuestStaticMap mapData2 = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap item) => item.mapId == e2);
				QuestStaticChapter tempData = list.Find((QuestStaticChapter item) => item.hardChapterId == mapData2.chapterId);
				if (tempData != null)
				{
					QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == tempData.hardChapterId);
					if (questStaticChapter != null)
					{
						list2.Add(questStaticChapter);
					}
				}
			}
		}
		using (List<int>.Enumerator enumerator = playableMapIdList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int e = enumerator.Current;
				bool flag = false;
				QuestStaticMap mapData = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap item) => item.mapId == e);
				QuestStaticChapter chapterData = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == mapData.chapterId);
				if (chapterData != null)
				{
					if (mode < 0 && !dictionary.ContainsKey(chapterData.chapterId))
					{
						flag = true;
					}
					else
					{
						QuestStaticChapter questStaticChapter2 = list2.Find((QuestStaticChapter item) => item.chapterId == chapterData.chapterId);
						if (!dictionary.ContainsKey(chapterData.chapterId))
						{
							if (mode == 0 && questStaticChapter2 == null)
							{
								flag = true;
							}
							else if (mode == 1 && questStaticChapter2 != null)
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						dictionary.Add(mapData.chapterId, chapterData);
					}
				}
			}
		}
		return new List<KeyValuePair<int, QuestStaticChapter>>(dictionary);
	}

	// Token: 0x06001675 RID: 5749 RVA: 0x0011A67C File Offset: 0x0011887C
	private void DestroyMap()
	{
		if (this.instMapObj != null)
		{
			Object.Destroy(this.instMapObj);
			AssetManager.UnloadAssetData(this.LoadAssetPath, AssetManager.OWNER.QuestSelector);
			this.instMapObj = null;
		}
		if (this.instCarObjList != null)
		{
			foreach (GameObject gameObject in this.instCarObjList)
			{
				Object.Destroy(gameObject);
			}
			this.instCarObjList.Clear();
		}
	}

	// Token: 0x06001676 RID: 5750 RVA: 0x0011A70C File Offset: 0x0011890C
	public static void MainStoryPlayBGM()
	{
		SoundManager.PlayBGM("prd_bgm0009");
	}

	// Token: 0x06001677 RID: 5751 RVA: 0x0011A718 File Offset: 0x00118918
	public static void DefaultPlayBGM()
	{
		SoundManager.PlayBGM("prd_bgm0007");
	}

	// Token: 0x06001678 RID: 5752 RVA: 0x0011A724 File Offset: 0x00118924
	public override void OnCreateScene()
	{
		SceneQuest.mapBoxObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("prefab/PguiBaseTemplate"));
		SceneQuest.mapBoxObject.name = "MAP_BOX";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.BACK, SceneQuest.mapBoxObject.transform, true);
		SceneQuest.mapBoxObject.GetComponent<SafeAreaScaler>().ApplySafeArea();
		this.safeArea = SafeAreaScaler.GetSafeArea();
		GameObject hdlQuestWindowCtrl = CanvasManager.HdlQuestWindowCtrl;
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/GUI_Quest_Main"));
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, gameObject.transform, true);
		GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneEvent/GUI/Prefab/Quest_ChapterLeft_EventCmn"));
		gameObject2.transform.SetParent(gameObject.transform, false);
		this.guiData = new SceneQuest.GUI(gameObject, gameObject2);
		this.guiChapterChange = new SceneQuest.GuiChapterChange(hdlQuestWindowCtrl.transform);
		this.guiData.DeactivateGameObject();
		this.selectData = new QuestUtil.SelectData();
		this.questChapterChangeWindow = new SceneQuest.QuestChapterChangeWindowCtrl();
		hdlQuestWindowCtrl.transform.Find("Window_ChapterChange").gameObject.SetActive(true);
		this.questChapterChangeWindow.Init(hdlQuestWindowCtrl.transform.Find("Window_ChapterChange").gameObject, new PguiOpenWindowCtrl.Callback(this.OnQuestChapterChangeWindow));
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.questChapterChangeWindow.guiData.owCtrl.transform, true);
		this.questChapterChangeWindow.guiData.owCtrl.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, true);
		this.questChapterChangeWindow.guiData.owCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex());
		this.questScheduleWindow = new SceneQuest.QuestScheduleWindowCtrl();
		hdlQuestWindowCtrl.transform.Find("Window_Schedule").gameObject.SetActive(true);
		this.questScheduleWindow.Init(hdlQuestWindowCtrl.transform.Find("Window_Schedule").gameObject);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.questScheduleWindow.guiData.owCtrl.transform, true);
		this.questScheduleWindow.guiData.owCtrl.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, true);
		this.questScheduleWindow.guiData.owCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex());
		this.itemInfoWindow = new SceneQuest.ItemInfoWindowCtrl();
		hdlQuestWindowCtrl.transform.Find("Window_ItemInfo").gameObject.SetActive(true);
		this.itemInfoWindow.Init(hdlQuestWindowCtrl.transform.Find("Window_ItemInfo").gameObject);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.itemInfoWindow.guiData.owCtrl.transform, true);
		this.itemInfoWindow.guiData.owCtrl.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, true);
		this.itemInfoWindow.guiData.owCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex());
		this.itemInfoWindowAfter = new SceneQuest.ItemInfoWindowAfterCtrl();
		hdlQuestWindowCtrl.transform.Find("Window_ItemInfo_After").gameObject.SetActive(true);
		this.itemInfoWindowAfter.Init(hdlQuestWindowCtrl.transform.Find("Window_ItemInfo_After").gameObject);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.itemInfoWindowAfter.guiData.owCtrl.transform, true);
		this.itemInfoWindowAfter.guiData.owCtrl.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, true);
		this.itemInfoWindowAfter.guiData.owCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex());
		this.guiData.questTop.storyQuestParts.Btn_StoryQuest.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.questTop.storyQuestParts.Btn_StorySelectL.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.questTop.storyQuestParts.Btn_StorySelectR.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.questTop.Btn_GrowQuest.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.questTop.Btn_EtceteraQuest.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.questTop.Btn_CharQuest.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.questTop.Btn_AnotherStory.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.questTop.Btn_Training.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.questTop.Btn_EventAll.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.questTop.Btn_AssistantEdit.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.questTop.selAssistantCtrl.guiData.Btn_EditOk.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.locationInfo.Btn_MoreInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonLocation), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.locationInfo.baseObj.SetActive(false);
		this.guiData.charaGrow.Btn_Schedule.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonCharaGrow), PguiButtonCtrl.SoundType.DEFAULT);
		foreach (PguiButtonCtrl pguiButtonCtrl in this.guiData.questTop.eventButton)
		{
			pguiButtonCtrl.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
		}
		this.guiData.locationEvent.Btn_ShopEvent.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestEvent), PguiButtonCtrl.SoundType.DEFAULT);
		PrjUtil.AddTouchEventTrigger(this.guiData.locationEvent.Banner.gameObject, new UnityAction<Transform>(this.OnClickEventInfoBanner));
		this.guiData.locationEvent.baseObj.SetActive(false);
		this.guiData.chapterSelect.Btn_Info.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonOther), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.chapterSelect.questButtonGroup.Init(new QuestButtonGroupCtrl.QuestButtonCallback(this.QuestButtonCallback));
		this.guiData.chapterSelect.Btn_SortFilterOnOff.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonOther), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.chapterSelect.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonLR), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.chapterSelect.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonLR), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.chapterSelect.Btn_Sel_Difficult.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonOther), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.chapterSelect.Btn_Mission.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonOther), PguiButtonCtrl.SoundType.DEFAULT);
		this.buttonEventAllCount = 0;
		this.UpdateButtonEventAllImage();
	}

	// Token: 0x06001679 RID: 5753 RVA: 0x0011AE98 File Offset: 0x00119098
	public override bool OnCreateSceneWait()
	{
		return true;
	}

	// Token: 0x0600167A RID: 5754 RVA: 0x0011AE9C File Offset: 0x0011909C
	public override void OnEnableScene(object args)
	{
		this.requestMode = SceneQuest.Mode.TOP;
		this.currentMode = SceneQuest.Mode.INVALID;
		this.requestNextScene = SceneManager.SceneName.None;
		this.requestNextSceneArgs = null;
		this.actionCoroutine = null;
		this.currentSequence = (this.reqNextSequence = this.guiData.questTop.baseObj);
		this.questArgs = args as SceneQuest.Args;
		this.TouchMoving = false;
		this.status = SceneQuest.Status.POLLING_REQUEST;
		this.guiData.questTop.selAssistantCtrl.Setup();
		RenderTextureChara component = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.guiData.chapterSelect.baseObj.transform.Find("Left").transform).GetComponent<RenderTextureChara>();
		component.postion = SceneQuest.DEFAULT_RENDER_TEXTURE_TRANSFORM.position;
		component.fieldOfView = SceneQuest.DEFAULT_RENDER_TEXTURE_TRANSFORM.fov;
		component.rotation = SceneQuest.DEFAULT_RENDER_TEXTURE_TRANSFORM.rotation;
		component.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
		component.transform.SetSiblingIndex(1);
		this.guiData.chapterSelect.renderTextureChara = component;
		RenderTextureChara component2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.guiData.chapterSelect.baseObj.transform.Find("Left").transform).GetComponent<RenderTextureChara>();
		component2.postion = new Vector2(-400f, -230f);
		component2.fieldOfView = 18f;
		component2.rotation = new Vector3(0f, 0f, 0f);
		component2.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
		component2.transform.SetSiblingIndex(1);
		this.guiData.chapterSelect.renderTextureChara2 = component2;
		this.guiData.chapterSelect.renderTextureChara2.gameObject.SetActive(false);
		this.coroutineOnEnableSceneTask = Singleton<SceneManager>.Instance.StartCoroutine(this.OnEnableSceneTask());
		this.selectData.questOneId = 0;
		this.IsPlayingAnim = false;
		this.eventStartTimeMap = new Dictionary<int, DateTime>();
		if (this.requestQuestCmd != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.requestQuestCmd);
		}
		this.requestQuestCmd = null;
		this.requestSequenceInstantiateAssetData = null;
	}

	// Token: 0x0600167B RID: 5755 RVA: 0x0011B0C1 File Offset: 0x001192C1
	private IEnumerator OnEnableSceneTask()
	{
		if (this.questArgs == null)
		{
			DataManager.DmQuest.RequestGetUserQuestInfo();
		}
		else if (this.questArgs != null && this.questArgs.recordCameSceneName != SceneManager.SceneName.SceneBattleSelector)
		{
			DataManager.DmQuest.RequestGetUserQuestInfo();
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (this.questArgs != null)
		{
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(this.questArgs.selectQuestOneId);
			if (questOnePackData != null)
			{
				if (questOnePackData.questChapter.category == QuestStaticChapter.Category.EVENT)
				{
					int num = QuestUtil.GetEventId(this.questArgs.selectQuestOneId, false);
					if (num <= 0)
					{
						num = this.questArgs.selectEventId;
					}
					DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(num);
					if (eventData != null && eventData.IsEnableChapter)
					{
						DataManagerEvent.Category category = eventData.eventCategory;
						if (category != DataManagerEvent.Category.Scenario)
						{
							if (category == DataManagerEvent.Category.Growth && this.IsNotNullEventCharaGrowObj())
							{
								CanvasManager.SetBgObj(QuestUtil.EVENT_BG);
								yield return this.RequestEventCharaGrow(eventData);
							}
						}
						else if (this.IsNotNullEventScenarioObj())
						{
							this.selEventScenarioCtrl.UpdateDecoration(false, true);
						}
					}
				}
			}
			else
			{
				DataManagerEvent.EventData eventData2 = DataManager.DmEvent.GetEventData(this.questArgs.selectEventId);
				if (eventData2 != null && eventData2.IsEnableChapter)
				{
					this.currentEnableEventData = eventData2;
					if (!this.questArgs.isFromBanner)
					{
						this.InitEventCtrl(eventData2.eventCategory);
					}
					DataManagerEvent.Category category = eventData2.eventCategory;
					if (category != DataManagerEvent.Category.Scenario)
					{
						if (category == DataManagerEvent.Category.Growth && this.IsNotNullEventCharaGrowObj())
						{
							CanvasManager.SetBgObj(QuestUtil.EVENT_BG);
							yield return this.RequestEventCharaGrow(eventData2);
						}
					}
					else if (this.IsNotNullEventScenarioObj())
					{
						this.selEventScenarioCtrl.UpdateDecoration(false, true);
					}
				}
			}
		}
		this.coroutineOnEnableSceneTask = null;
		yield break;
	}

	// Token: 0x0600167C RID: 5756 RVA: 0x0011B0D0 File Offset: 0x001192D0
	public override bool OnEnableSceneWait()
	{
		if (this.coroutineOnEnableSceneTask != null)
		{
			return false;
		}
		this.guiData.questTop.selAssistantCtrl.SetupAssistant();
		this.guiData.questTop.selAssistantCtrl.ChangeMode(SelAssistantCtrl.Mode.TOP);
		RenderTextureChara renderTextureChara = this.guiData.questTop.selAssistantCtrl.renderTextureChara;
		renderTextureChara.transform.SetAsLastSibling();
		renderTextureChara.postion = new Vector2(343f, -150f);
		renderTextureChara.fieldOfView = 18f;
		renderTextureChara.rotation = new Vector3(0f, 0f, 0f);
		renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
		renderTextureChara.transform.SetSiblingIndex(3);
		this.guiData.questTop.renderTextureChara = renderTextureChara;
		bool flag = QuestUtil.IsDispDhole();
		this.guiData.questTop.renderTextureChara.gameObject.SetActive(flag);
		SceneQuest.TimeStampInScene = TimeManager.Now;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		int num = 0;
		if (this.questArgs != null)
		{
			QuestOnePackData qopd = DataManager.DmQuest.GetQuestOnePackData(this.questArgs.selectQuestOneId);
			num = this.questArgs.selectQuestOneId;
			Func<bool> func = () => qopd == null || this.questArgs.justBeforeBattle != null;
			if (qopd != null)
			{
				QuestStaticChapter questChapter = qopd.questChapter;
				int num2 = qopd.questMap.mapId;
				if (SceneQuest.IsMainStory(questChapter.category))
				{
					if (this.selMainStoryCtrl == null)
					{
						this.InitSelMainStoryCtrl();
						this.DestroyMap();
						this.mainChapterChangeEffect = this.MainChapterChangeEffect();
						if (SceneQuest.IsMainStoryPart1(this.questArgs.category))
						{
							this.selectData.questCategory = QuestStaticChapter.Category.STORY;
						}
						else if (SceneQuest.IsMainStoryPart1_5(this.questArgs.category))
						{
							this.selectData.questCategory = QuestStaticChapter.Category.CELLVAL;
						}
						else if (SceneQuest.IsMainStoryPart2(this.questArgs.category))
						{
							this.selectData.questCategory = QuestStaticChapter.Category.STORY2;
						}
						else if (SceneQuest.IsMainStoryPart3(this.questArgs.category))
						{
							this.selectData.questCategory = QuestStaticChapter.Category.STORY3;
						}
						string mainStoryMapPath = SceneQuest.GetMainStoryMapPath(this.questArgs.category);
						this.instCarObjList = SceneQuest.CreateCarObjList(SceneQuest.mapBoxObject);
						this.requestSequenceInstantiateAssetData = Singleton<SceneManager>.Instance.StartCoroutine(this.InstantiateAssetData(this.LoadMapObject(mainStoryMapPath), mainStoryMapPath));
					}
					if (this.questArgs.initialMap)
					{
						this.currentSequence = (this.reqNextSequence = this.selMainStoryCtrl.GuiData.pointSelect.baseObj);
					}
					else
					{
						this.currentSequence = (this.reqNextSequence = this.guiData.chapterSelect.baseObj);
						flag2 = true;
					}
					flag3 = true;
					SceneQuest.Args.JustBeforeBattle justBeforeBattle = this.questArgs.justBeforeBattle;
					if (justBeforeBattle != null && justBeforeBattle.isFirstClear && !SceneQuest.IsMainStoryPart2(questChapter.category) && !SceneQuest.IsMainStoryPart3(questChapter.category))
					{
						num2++;
					}
					this.mainChapterChangeEffect = this.MainChapterChangeEffect();
					if (justBeforeBattle != null)
					{
						string mainStoryMapPath2 = SceneQuest.GetMainStoryMapPath(questChapter.category);
						this.DestroyMap();
						this.instCarObjList = SceneQuest.CreateCarObjList(SceneQuest.mapBoxObject);
						this.requestSequenceInstantiateAssetData = Singleton<SceneManager>.Instance.StartCoroutine(this.InstantiateAssetData(this.LoadMapObject(mainStoryMapPath2), mainStoryMapPath2));
					}
				}
				else if (questChapter.category == QuestStaticChapter.Category.GROW)
				{
					this.currentSequence = (this.reqNextSequence = this.guiData.chapterSelect.baseObj);
				}
				else if (questChapter.category == QuestStaticChapter.Category.CHARA)
				{
					if (!this.IsNotNullCharaStoryObj())
					{
						this.InitCharaQuest();
					}
					this.currentSequence = (this.reqNextSequence = this.guiData.chapterSelect.baseObj);
					this.selCharaStoryCtrl.SetupCharaPackList();
					this.selCharaStoryCtrl.SetupCharaQuestSortWindow();
					this.selCharaStoryCtrl.SelectCharaId = qopd.questMap.questCharaId;
					this.charaStoryEffect = this.CharaStoryEffect();
				}
				else if (questChapter.category == QuestStaticChapter.Category.SIDE_STORY)
				{
					if (this.questArgs.initialMap)
					{
						this.currentSequence = (this.reqNextSequence = this.selSideStoryCtrl.GuiData.mapSelect.baseObj);
						num2++;
						if (!DataManager.DmQuest.GetPlayableMapIdList(questChapter.chapterId).Contains(num2))
						{
							num2--;
						}
					}
					else
					{
						this.currentSequence = (this.reqNextSequence = this.guiData.chapterSelect.baseObj);
					}
					this.sideStoryChapterChangeEffect = this.SideStoryChapterChangeEffect();
				}
				else if (questChapter.category == QuestStaticChapter.Category.ETCETERA)
				{
					if (this.questArgs.initialMap)
					{
						this.currentSequence = (this.reqNextSequence = this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj);
						num2++;
						if (!DataManager.DmQuest.GetPlayableMapIdList(questChapter.chapterId).Contains(num2))
						{
							num2--;
						}
					}
					else if (!this.IsNotNullEtcetraStoryObj())
					{
						this.currentSequence = (this.reqNextSequence = this.guiData.questTop.baseObj);
					}
					else
					{
						List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(questChapter.chapterId);
						this.currentSequence = (this.reqNextSequence = (playableMapIdList.Contains(num2) ? this.guiData.chapterSelect.baseObj : this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj));
					}
				}
				else if (questChapter.category == QuestStaticChapter.Category.EVENT)
				{
					flag4 = true;
					flag5 = func();
				}
				this.selectData.questCategory = questChapter.category;
				this.selectData.chapterId = questChapter.chapterId;
				this.selectData.mapId = num2;
				this.selectData.questGroupId = qopd.questGroup.questGroupId;
				if (SceneQuest.IsMainStory(questChapter.category))
				{
					SceneQuest.MainStoryPlayBGM();
				}
				else
				{
					SceneQuest.DefaultPlayBGM();
				}
			}
			else if (this.questArgs.tutorialSequence == TutorialUtil.Sequence.QUEST_GUIDE)
			{
				Singleton<SceneManager>.Instance.StartCoroutine(this.TutorialQuest1());
			}
			else if (SceneQuest.IsMainStory(this.questArgs.category))
			{
				if (this.questArgs.initialMap)
				{
					this.currentSequence = (this.reqNextSequence = this.selMainStoryCtrl.GuiData.pointSelect.baseObj);
				}
				else
				{
					this.currentSequence = (this.reqNextSequence = this.guiData.chapterSelect.baseObj);
					flag2 = true;
				}
				flag3 = true;
				this.DestroyMap();
				this.mainChapterChangeEffect = this.MainChapterChangeEffect();
				if (SceneQuest.IsMainStoryPart1(this.questArgs.category))
				{
					this.selectData.questCategory = QuestStaticChapter.Category.STORY;
				}
				else if (SceneQuest.IsMainStoryPart1_5(this.questArgs.category))
				{
					this.selectData.questCategory = QuestStaticChapter.Category.CELLVAL;
				}
				else if (SceneQuest.IsMainStoryPart2(this.questArgs.category))
				{
					this.selectData.questCategory = QuestStaticChapter.Category.STORY2;
				}
				else if (SceneQuest.IsMainStoryPart3(this.questArgs.category))
				{
					this.selectData.questCategory = QuestStaticChapter.Category.STORY3;
				}
				string mainStoryMapPath3 = SceneQuest.GetMainStoryMapPath(this.questArgs.category);
				this.instCarObjList = SceneQuest.CreateCarObjList(SceneQuest.mapBoxObject);
				this.requestSequenceInstantiateAssetData = Singleton<SceneManager>.Instance.StartCoroutine(this.InstantiateAssetData(this.LoadMapObject(mainStoryMapPath3), mainStoryMapPath3));
			}
			else if (this.questArgs.category == QuestStaticChapter.Category.SIDE_STORY)
			{
				if (this.questArgs.initialMap)
				{
					if (this.IsNotNullMapObj())
					{
						this.currentSequence = (this.reqNextSequence = this.selSideStoryCtrl.GuiData.mapSelect.baseObj);
					}
				}
				else
				{
					this.currentSequence = (this.reqNextSequence = this.guiData.chapterSelect.baseObj);
				}
				this.sideStoryChapterChangeEffect = this.SideStoryChapterChangeEffect();
				this.selectData.questCategory = QuestStaticChapter.Category.SIDE_STORY;
			}
			else if (this.questArgs.category == QuestStaticChapter.Category.ETCETERA)
			{
				this.selectData.questCategory = QuestStaticChapter.Category.ETCETERA;
				if (DataManager.DmQuest.QuestStaticData.chapterDataMap.ContainsKey(this.selectData.chapterId) && DataManager.DmQuest.QuestStaticData.chapterDataMap[this.selectData.chapterId].category != QuestStaticChapter.Category.ETCETERA)
				{
					List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.ETCETERA);
					if (playableMapIdList2.Count > 0 && DataManager.DmQuest.QuestStaticData.mapDataMap.ContainsKey(playableMapIdList2[0]))
					{
						this.selectData.chapterId = DataManager.DmQuest.QuestStaticData.mapDataMap[playableMapIdList2[0]].chapterId;
					}
				}
				if (this.questArgs.initialMap)
				{
					if (!this.IsNotNullEtcetraStoryObj())
					{
						this.InitEtcetraStory();
					}
					this.currentSequence = (this.reqNextSequence = this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj);
				}
				else
				{
					this.currentSequence = (this.reqNextSequence = this.guiData.chapterSelect.baseObj);
				}
			}
			else if (this.questArgs.category == QuestStaticChapter.Category.EVENT)
			{
				flag4 = true;
				flag5 = func();
			}
			else if (this.IsNotNullCharaStoryObj() && this.questArgs.category == QuestStaticChapter.Category.CHARA)
			{
				this.currentSequence = (this.reqNextSequence = this.guiData.chapterSelect.baseObj);
				this.selCharaStoryCtrl.SetupCharaQuestSortWindow();
				if (this.questArgs.selectCharaId != 0)
				{
					this.selCharaStoryCtrl.SelectCharaId = this.questArgs.selectCharaId;
					CanvasManager.HdlCharaWindowCtrl.Open(DataManager.DmChara.GetUserCharaData(this.selCharaStoryCtrl.SelectCharaId), new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY_NO_SCENARIO, null), delegate
					{
					});
				}
				this.selectData.questCategory = QuestStaticChapter.Category.CHARA;
			}
			if (qopd == null)
			{
				if (SceneQuest.IsMainStory(this.questArgs.category))
				{
					SceneQuest.MainStoryPlayBGM();
				}
				else
				{
					SceneQuest.DefaultPlayBGM();
				}
			}
			if (flag4)
			{
				int num3 = QuestUtil.GetEventId(this.questArgs.selectQuestOneId, false);
				if (num3 <= 0)
				{
					num3 = this.questArgs.selectEventId;
				}
				DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(num3);
				if (eventData != null && eventData.IsEnableChapter)
				{
					this.currentEnableEventData = eventData;
					this.selectData.eventId = this.currentEnableEventData.eventId;
					this.selectData.chapterId = this.currentEnableEventData.eventChapterId;
					this.selectData.questCategory = QuestStaticChapter.Category.EVENT;
					switch (this.currentEnableEventData.eventCategory)
					{
					case DataManagerEvent.Category.Scenario:
						if (this.IsNotNullEventScenarioObj())
						{
							if (this.questArgs.backSequenceGameObject == null)
							{
								this.questArgs.backSequenceGameObject = this.selEventScenarioCtrl.GuiData.eventSelect.baseObj;
							}
							this.currentSequence = (this.reqNextSequence = ((this.questArgs.selectQuestOneId > 0) ? this.guiData.chapterSelect.baseObj : this.questArgs.backSequenceGameObject));
						}
						break;
					case DataManagerEvent.Category.Growth:
						if (this.IsNotNullEventCharaGrowObj() && this.questArgs.recordCameSceneName != SceneManager.SceneName.SceneMission)
						{
							this.SetupNextSequenceEventCharaGrow(false);
							CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByCharaGrow(true);
						}
						break;
					case DataManagerEvent.Category.Large:
						if (this.IsNotNullEventLargeScaleObj())
						{
							if (this.questArgs.backSequenceGameObject == null)
							{
								if (this.questArgs.initialMap)
								{
									this.currentSequence = (this.reqNextSequence = this.selEventLargeScaleCtrl.GuiData.pointSelect.baseObj);
								}
								else
								{
									this.currentSequence = (this.reqNextSequence = this.guiData.chapterSelect.baseObj);
									flag2 = true;
								}
							}
							else
							{
								this.currentSequence = (this.reqNextSequence = this.questArgs.backSequenceGameObject);
							}
							if (flag5)
							{
								this.DestroyMap();
								this.instCarObjList = SceneQuest.CreateCarObjList(SceneQuest.mapBoxObject);
								this.requestSequenceInstantiateAssetData = Singleton<SceneManager>.Instance.StartCoroutine(this.InstantiateAssetData(this.selEventLargeScaleCtrl.LoadMapObject(SceneQuest.mapBoxObject), this.selEventLargeScaleCtrl.LoadAssetPath));
							}
							CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByLarge(true, this.currentEnableEventData.eventId);
						}
						this.selEventLargeScaleCtrl.PlayBGM();
						flag3 = true;
						this.eventLargeScaleEffect = this.EventLargeScaleEffect();
						break;
					case DataManagerEvent.Category.Tower:
						if (this.IsNotNullEventTowerObj())
						{
							if (this.questArgs.backSequenceGameObject == null)
							{
								this.questArgs.backSequenceGameObject = this.selEventTowerCtrl.GuiData.eventSelect.baseObj;
							}
							this.currentSequence = (this.reqNextSequence = ((this.questArgs.selectQuestOneId > 0) ? this.guiData.chapterSelect.baseObj : this.questArgs.backSequenceGameObject));
							SelEventTowerCtrl.PlayBGM();
							CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByTower(true);
						}
						break;
					case DataManagerEvent.Category.Coop:
						if (this.IsNotNullEventCoopObj())
						{
							if (this.questArgs.backSequenceGameObject == null)
							{
								this.questArgs.backSequenceGameObject = this.selEventCoopCtrl.GuiData.mapSelect.baseObj;
							}
							this.currentSequence = (this.reqNextSequence = ((this.questArgs.selectQuestOneId > 0) ? this.guiData.chapterSelect.baseObj : this.questArgs.backSequenceGameObject));
							if (this.currentSequence == this.guiData.chapterSelect.baseObj)
							{
								flag2 = true;
							}
							SelEventCoopCtrl.PlayBGM();
							CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByCoop(true, this.currentEnableEventData.eventId);
						}
						break;
					case DataManagerEvent.Category.WildRelease:
						if (this.IsNotNullEventWildReleaseObj())
						{
							this.currentSequence = (this.reqNextSequence = this.guiData.chapterSelect.baseObj);
							SelEventWildReleaseCtrl.PlayBGM();
							this.selEventWildReleaseCtrl.SetActiveQuestSelect(true);
							CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByWildRelease(true);
						}
						break;
					}
				}
			}
		}
		else
		{
			SceneQuest.DefaultPlayBGM();
		}
		SceneQuest.IMapData mapData = null;
		GameObject gameObject = null;
		if (this.IsLargeEvent())
		{
			mapData = this.selEventLargeScaleCtrl.GuiData.mapData;
			gameObject = this.selEventLargeScaleCtrl.GuiData.pointSelect.baseObj;
		}
		else if (this.IsNotNullMainStoryObj())
		{
			mapData = this.selMainStoryCtrl.GuiData.mapData;
			gameObject = this.selMainStoryCtrl.GuiData.pointSelect.baseObj;
		}
		this.guiData.SetActive(true, mapData);
		this.guiData.SwitchSelector(new SceneQuest.GUI.SetupSwitchSelectorParam
		{
			enableBlur = flag2,
			isMapNeedQuest = flag3,
			updateCb = new SceneQuest.UpdateCallback(this.UpdateSwitchSelector),
			selectData = this.selectData,
			mapDataGameObj = mapData,
			pointSelectObj = gameObject
		});
		QuestUtil.SetupBG(num, (this.questArgs != null) ? this.questArgs.category : QuestStaticChapter.Category.INVALID, (this.questArgs != null) ? this.questArgs.selectEventId : 0);
		this.guiData.chapterSelect.Btn_SortFilterOnOff.transform.Find("BaseImage/On").gameObject.SetActive(!DataManager.DmUserInfo.optionData.secondScenarioSkip);
		this.guiData.chapterSelect.Btn_SortFilterOnOff.transform.Find("BaseImage/Off").gameObject.SetActive(DataManager.DmUserInfo.optionData.secondScenarioSkip);
		return true;
	}

	// Token: 0x0600167D RID: 5757 RVA: 0x0011C0AC File Offset: 0x0011A2AC
	public override void OnStartControl()
	{
		SimpleAnimation component = this.reqNextSequence.GetComponent<SimpleAnimation>();
		if (component != null)
		{
			component.ExResumeAnimation(null);
		}
	}

	// Token: 0x0600167E RID: 5758 RVA: 0x0011C0D5 File Offset: 0x0011A2D5
	public IEnumerator LoadMapObject(string path)
	{
		AssetManager.LoadAssetData(path, AssetManager.OWNER.QuestSelector, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(path))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600167F RID: 5759 RVA: 0x0011C0E4 File Offset: 0x0011A2E4
	private IEnumerator UpdateMainStoryScreen()
	{
		if (this.safeArea != SafeAreaScaler.GetSafeArea())
		{
			while (SceneQuest.mapBoxObject.GetComponent<SafeAreaScaler>().applySafeArea != SafeAreaScaler.GetSafeArea())
			{
				yield return null;
			}
			Rect mapBoxRect = default(Rect);
			while (mapBoxRect != SceneQuest.mapBoxObject.GetComponent<RectTransform>().rect)
			{
				if (SceneQuest.IsMainStoryPart1_5(this.selectData.questCategory))
				{
					Vector2 vector = new Vector2(this.selMainStoryCtrl.GetMapOffsetPosX(this.selectData.questCategory, true), this.selMainStoryCtrl.GuiData.mapData.mapObj.GetComponent<RectTransform>().anchoredPosition.y);
					this.selMainStoryCtrl.GuiData.mapData.mapObj.GetComponent<RectTransform>().anchoredPosition = vector;
				}
				if (!SceneQuest.IsMainStoryPart1(this.selectData.questCategory))
				{
					Vector2 vector2 = new Vector2(this.selMainStoryCtrl.GetMapOffsetPosX(this.selectData.questCategory, false), this.selMainStoryCtrl.GuiData.mapData.bgObj.GetComponent<RectTransform>().anchoredPosition.y);
					this.selMainStoryCtrl.GuiData.mapData.bgObj.GetComponent<RectTransform>().anchoredPosition = vector2;
				}
				SelMainStoryCtrl.AdjustMaskPostion(this.selectData.questCategory, this.selMainStoryCtrl.GuiData.mapData, SceneQuest.mapBoxObject);
				if (!SceneQuest.IsMainStoryPart1_5(this.selectData.questCategory))
				{
					SceneQuest.SetMapPosition(new Info(), this.selectData.questCategory, this.selMainStoryCtrl.GuiData.mapData, SceneQuest.mapBoxObject);
				}
				mapBoxRect = SceneQuest.mapBoxObject.GetComponent<RectTransform>().rect;
				yield return null;
			}
			mapBoxRect = default(Rect);
			yield break;
		}
		yield break;
	}

	// Token: 0x06001680 RID: 5760 RVA: 0x0011C0F3 File Offset: 0x0011A2F3
	private IEnumerator UpdateLargeEventScreen()
	{
		if (this.safeArea != SafeAreaScaler.GetSafeArea())
		{
			while (SceneQuest.mapBoxObject.GetComponent<SafeAreaScaler>().applySafeArea != SafeAreaScaler.GetSafeArea())
			{
				yield return null;
			}
			Rect mapBoxRect = default(Rect);
			while (mapBoxRect != SceneQuest.mapBoxObject.GetComponent<RectTransform>().rect)
			{
				this.selEventLargeScaleCtrl.AdjustMaskPostion();
				this.selEventLargeScaleCtrl.OnTouchMove(new Info());
				mapBoxRect = SceneQuest.mapBoxObject.GetComponent<RectTransform>().rect;
				yield return null;
			}
			mapBoxRect = default(Rect);
			yield break;
		}
		yield break;
	}

	// Token: 0x06001681 RID: 5761 RVA: 0x0011C104 File Offset: 0x0011A304
	public override void Update()
	{
		bool flag = true;
		if (this.guiData != null)
		{
			if (this.IsNotNullMainStoryObj())
			{
				this.selMainStoryCtrl.UpdateMission();
			}
			int num = (((this.IsCharaGrowEvent() || this.IsWildReleaseEvent()) && this.currentEnableEventData != null) ? DataManager.DmMission.GetUserClearEventMissionNum(this.currentEnableEventData.eventId) : DataManager.DmMission.GetUserClearMissionNum());
			this.guiData.chapterSelect.Txt_Mission_Num.transform.parent.transform.parent.gameObject.SetActive(num > 0);
			this.guiData.chapterSelect.Txt_Mission_Num.text = num.ToString();
			if (this.guiData.questTop.baseObj.activeSelf)
			{
				for (int i = 0; i < Enum.GetValues(typeof(QuestStaticChapter.Category)).Length; i++)
				{
					QuestStaticChapter.Category category = (QuestStaticChapter.Category)i;
					if (!SceneQuest.IsMainStoryPart1(category) && !SceneQuest.IsMainStoryPart1_5(category))
					{
						this.guiData.questTop.UpdateCampaignInfoCategory(category, 0);
					}
				}
			}
			if (this.IsNotNullCharaStoryObj() && this.selCharaStoryCtrl.GuiData.charaSelect.baseObj.activeSelf)
			{
				this.selCharaStoryCtrl.GuiData.charaSelect.UpdateCampaignInfoCategory(this.selectData.questCategory, this.selectData.chapterId);
			}
			if (this.IsNotNullMainStoryObj() && this.selMainStoryCtrl.GuiData.pointSelect.baseObj.activeSelf)
			{
				this.selMainStoryCtrl.GuiData.pointSelect.UpdateCampaignInfoCategory(this.selectData.questCategory, this.selectData.chapterId);
			}
			if (this.IsNotNullMapBaseObj())
			{
				Singleton<SceneManager>.Instance.StartCoroutine(this.UpdateMainStoryScreen());
			}
			if (this.IsNotNullEventLargeScaleMapBaseObj())
			{
				Singleton<SceneManager>.Instance.StartCoroutine(this.UpdateLargeEventScreen());
			}
			if (this.guiData.chapterSelect.baseObj.activeSelf)
			{
				this.guiData.chapterSelect.UpdateCampaignInfoCategory(this.selectData.questCategory, this.selectData.chapterId);
			}
		}
		if (this.IsNotNullSideStoryObj())
		{
			if (this.selSideStoryCtrl.GuiData.mapSelect.baseObj.activeSelf)
			{
				this.selSideStoryCtrl.GuiData.mapSelect.UpdateCampaignInfoCategory(this.selectData.chapterId);
			}
		}
		else if (this.IsNotNullEtcetraStoryObj() && this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj.activeSelf)
		{
			this.selEtcetraStoryCtrl.GuiData.mapSelect.UpdateCampaignInfoCategory(this.selectData.chapterId);
		}
		if (this.status == SceneQuest.Status.POLLING_REQUEST && this.reqNextSequence != null)
		{
			this.status = SceneQuest.Status.CHANGING_SCENE;
			Singleton<SceneManager>.Instance.StartCoroutine(this.ChangeScene(this.reqNextSequence));
		}
		if (this.mainChapterChangeEffect != null && !this.mainChapterChangeEffect.MoveNext())
		{
			this.mainChapterChangeEffect = null;
			if (this.questArgs != null && this.questArgs.tutorialSequence == TutorialUtil.Sequence.QUEST_GUIDE)
			{
				Singleton<SceneManager>.Instance.StartCoroutine(this.TutorialQuest2());
			}
		}
		if (this.sideStoryChapterChangeEffect != null && !this.sideStoryChapterChangeEffect.MoveNext())
		{
			this.sideStoryChapterChangeEffect = null;
		}
		if (this.charaStoryEffect != null && !this.charaStoryEffect.MoveNext())
		{
			this.charaStoryEffect = null;
		}
		if (this.modeEndSequenceCtrl != null && !this.modeEndSequenceCtrl.MoveNext())
		{
			this.modeEndSequenceCtrl = null;
		}
		if (this.getItemWindowCtrl != null && !this.getItemWindowCtrl.MoveNext())
		{
			this.getItemWindowCtrl = null;
		}
		if (this.chapterChangeEffect != null && !this.chapterChangeEffect.MoveNext())
		{
			this.chapterChangeEffect = null;
		}
		if (this.questFirstClearEvent != null && this.questFirstClearEvent.UpdateResolve().isFinish)
		{
			this.questFirstClearEvent = null;
		}
		if (this.eventLargeScaleEffect != null && !this.eventLargeScaleEffect.MoveNext())
		{
			this.eventLargeScaleEffect = null;
		}
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.requestNextSceneArgs);
			flag = false;
		}
		if (this.requestMode != this.currentMode)
		{
			this.currentMode = this.requestMode;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(flag, true);
		this.safeArea = SafeAreaScaler.GetSafeArea();
	}

	// Token: 0x06001682 RID: 5762 RVA: 0x0011C53C File Offset: 0x0011A73C
	public override void OnDisableScene()
	{
		if (this.selectData.questOneId != 0 && (SceneQuest.IsMainStory(this.selectData.questCategory) || this.IsLargeEvent()))
		{
			this.guiData.basePanel.SetActive(false);
			if (this.IsNotNullMapBaseObj())
			{
				this.selMainStoryCtrl.GuiData.mapData.baseObj.SetActive(true);
			}
		}
		else
		{
			SceneQuest.IMapData mapData = null;
			if (this.IsLargeEvent())
			{
				mapData = this.selEventLargeScaleCtrl.GuiData.mapData;
			}
			else if (this.IsNotNullMainStoryObj())
			{
				mapData = this.selMainStoryCtrl.GuiData.mapData;
			}
			this.guiData.SetActive(false, mapData);
			GameObject gameObject = SceneQuest.mapBoxObject;
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
		this.guiData.locationInfo.baseObj.SetActive(false);
		if (this.requestSequenceInstantiateAssetData != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.requestSequenceInstantiateAssetData);
			this.requestSequenceInstantiateAssetData = null;
		}
		if (this.IsNotNullMapBaseObj())
		{
			this.selMainStoryCtrl.RemoveTouchEventTriggger();
		}
		if (this.IsNotNullEventLargeScaleMapBaseObj())
		{
			this.selEventLargeScaleCtrl.RemoveTouchEventTriggger(this.selectData.chapterId);
		}
		if (this.IsNotNullEventCharaGrowObj())
		{
			this.selEventCharaGrowCtrl.Dest();
		}
		if (this.IsNotNullEventLargeScaleObj())
		{
			this.selEventLargeScaleCtrl.Dest();
		}
		if (this.IsNotNullEventScenarioObj())
		{
			this.selEventScenarioCtrl.Dest();
		}
		if (this.IsNotNullEventTowerObj())
		{
			this.selEventTowerCtrl.Dest();
		}
		if (this.IsNotNullEventCoopObj())
		{
			this.selEventCoopCtrl.Dest();
		}
		if (this.IsNotNullEventWildReleaseObj())
		{
			this.selEventWildReleaseCtrl.Dest();
		}
		if (this.IsNotNullMainStoryObj())
		{
			this.selMainStoryCtrl.Dest();
		}
		if (this.IsNotNullSideStoryObj())
		{
			this.selSideStoryCtrl.Dest();
		}
		if (this.IsNotNullEtcetraStoryObj())
		{
			this.selEtcetraStoryCtrl.Dest();
		}
		if (this.IsNotNullCharaStoryObj())
		{
			this.selCharaStoryCtrl.Dest();
		}
		if (this.guiData.questTop.renderTextureChara != null)
		{
			Object.Destroy(this.guiData.questTop.renderTextureChara.gameObject);
		}
		this.guiData.questTop.renderTextureChara = null;
		if (this.guiData.chapterSelect.renderTextureChara != null)
		{
			Object.Destroy(this.guiData.chapterSelect.renderTextureChara.gameObject);
		}
		this.guiData.chapterSelect.renderTextureChara = null;
		if (this.guiData.chapterSelect.renderTextureChara2 != null)
		{
			Object.Destroy(this.guiData.chapterSelect.renderTextureChara2.gameObject);
		}
		this.guiData.chapterSelect.renderTextureChara2 = null;
		AssetManager.UnLoadByList(AssetManager.OWNER.QuestSelector, "", true);
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x0011C7F4 File Offset: 0x0011A9F4
	public override void OnDestroyScene()
	{
		if (this.IsNotNullEventLargeScaleObj())
		{
			SGNFW.Touch.Manager.UnRegisterMove(new SGNFW.Touch.Manager.SingleAction(this.selEventLargeScaleCtrl.OnTouchMove));
			SGNFW.Touch.Manager.UnRegisterRelease(new SGNFW.Touch.Manager.SingleAction(this.selEventLargeScaleCtrl.OnTouchRelease));
			SGNFW.Touch.Manager.UnRegisterStart(new SGNFW.Touch.Manager.SingleAction(this.selEventLargeScaleCtrl.OnTouchStart));
		}
		SGNFW.Touch.Manager.UnRegisterMove(new SGNFW.Touch.Manager.SingleAction(this.OnTouchMove));
		SGNFW.Touch.Manager.UnRegisterRelease(new SGNFW.Touch.Manager.SingleAction(this.OnTouchRelease));
		SGNFW.Touch.Manager.UnRegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
		this.guiData.Destroy();
		this.guiChapterChange.Destroy();
		if (this.IsNotNullMainStoryObj())
		{
			this.selMainStoryCtrl.GuiData.mapData.Destroy();
		}
		if (this.IsNotNullEventScenarioObj())
		{
			this.selEventScenarioCtrl.Destroy();
		}
		if (this.IsNotNullEventTowerObj())
		{
			this.selEventTowerCtrl.Destroy();
		}
		if (this.IsNotNullEventCoopObj())
		{
			this.selEventCoopCtrl.Destroy();
		}
		if (this.IsNotNullEventWildReleaseObj())
		{
			this.selEventWildReleaseCtrl.Destroy();
		}
		if (this.IsNotNullEventCharaGrowObj())
		{
			this.selEventCharaGrowCtrl.Destroy();
		}
		if (this.IsNotNullEventLargeScaleObj())
		{
			this.selEventLargeScaleCtrl.Destroy();
		}
		if (this.IsNotNullMainStoryObj())
		{
			this.selMainStoryCtrl.Destroy();
		}
		if (this.IsNotNullSideStoryObj())
		{
			this.selSideStoryCtrl.Destroy();
		}
		if (this.IsNotNullEtcetraStoryObj())
		{
			this.selEtcetraStoryCtrl.Destroy();
		}
		if (this.IsNotNullCharaStoryObj())
		{
			this.selCharaStoryCtrl.Destroy();
		}
		this.DestroyMap();
		if (this.questChapterChangeWindow != null)
		{
			Object.Destroy(this.questChapterChangeWindow.guiData.baseObj);
			this.questChapterChangeWindow = null;
		}
		if (this.questScheduleWindow != null)
		{
			Object.Destroy(this.questScheduleWindow.guiData.baseObj);
			this.questScheduleWindow = null;
		}
		if (this.itemInfoWindow != null)
		{
			Object.Destroy(this.itemInfoWindow.guiData.baseObj);
			this.itemInfoWindow = null;
		}
		if (this.itemInfoWindowAfter != null)
		{
			Object.Destroy(this.itemInfoWindowAfter.guiData.baseObj);
			this.itemInfoWindowAfter = null;
		}
		if (SceneQuest.mapBoxObject != null)
		{
			Object.Destroy(SceneQuest.mapBoxObject);
			SceneQuest.mapBoxObject = null;
		}
	}

	// Token: 0x06001684 RID: 5764 RVA: 0x0011CA24 File Offset: 0x0011AC24
	private void OnTouchMove(Info info)
	{
		if (this.questArgs != null && this.questArgs.tutorialSequence == TutorialUtil.Sequence.QUEST_GUIDE)
		{
			return;
		}
		if (SceneQuest.IsMainStoryPart1_5(this.selectData.questCategory))
		{
			return;
		}
		if (this.guiData.chapterSelect.baseObj.activeSelf)
		{
			return;
		}
		if (!this.IsNotNullMapObj())
		{
			return;
		}
		if (this.selMainStoryCtrl.GuiData.mapData.mapObj == null)
		{
			return;
		}
		if ((info.CurrentPosition - info.InitPosition).sqrMagnitude > QuestUtil.MOVING_SQR_MAGNITUDE)
		{
			this.TouchMoving = true;
		}
		this.SetMapPosition(info);
	}

	// Token: 0x06001685 RID: 5765 RVA: 0x0011CACC File Offset: 0x0011ACCC
	public static void SetMapPosition(Info info, QuestStaticChapter.Category category, SelMainStoryCtrl.GuiMapData guiMapData, GameObject mapBoxObject)
	{
		Vector2 vector = guiMapData.mapObj.transform.localPosition;
		Vector2 vector2 = guiMapData.bgObj.transform.localPosition;
		float num = guiMapData.bgObj.transform.Find("Tex_Bg").GetComponent<RectTransform>().sizeDelta.x * 0.5f;
		float scrollWidth = SelMainStoryCtrl.GetScrollWidth(category, guiMapData);
		vector.x += info.DeltaPosition.x * (num / (float)Screen.width);
		if (SceneQuest.IsMainStoryPart1(category))
		{
			vector2.x += info.DeltaPosition.x * (num / (float)Screen.width);
		}
		float safeAreaX = SelMainStoryCtrl.GetSafeAreaX(category, mapBoxObject, guiMapData);
		float mapOffsetPosX = SelMainStoryCtrl.GetMapOffsetPosX(category, mapBoxObject, guiMapData, true);
		float mapOffsetPosX2 = SelMainStoryCtrl.GetMapOffsetPosX(category, mapBoxObject, guiMapData, false);
		if (vector.x < -scrollWidth + safeAreaX)
		{
			vector.x = -scrollWidth + safeAreaX;
		}
		if (vector.x > mapOffsetPosX)
		{
			vector.x = mapOffsetPosX;
		}
		if (SceneQuest.IsMainStoryPart1(category))
		{
			if (vector2.x < -scrollWidth + safeAreaX)
			{
				vector2.x = -scrollWidth + safeAreaX;
			}
			if (vector2.x > mapOffsetPosX2)
			{
				vector2.x = mapOffsetPosX2;
			}
		}
		guiMapData.mapObj.transform.localPosition = vector;
		if (SceneQuest.IsMainStoryPart1(category))
		{
			guiMapData.bgObj.transform.localPosition = vector2;
		}
	}

	// Token: 0x06001686 RID: 5766 RVA: 0x0011CC32 File Offset: 0x0011AE32
	private void SetMapPosition(Info info)
	{
		SceneQuest.SetMapPosition(info, this.selectData.questCategory, this.selMainStoryCtrl.GuiData.mapData, SceneQuest.mapBoxObject);
	}

	// Token: 0x06001687 RID: 5767 RVA: 0x0011CC5A File Offset: 0x0011AE5A
	private void OnTouchRelease(Info info)
	{
	}

	// Token: 0x06001688 RID: 5768 RVA: 0x0011CC5C File Offset: 0x0011AE5C
	private void OnTouchStart(Info info)
	{
		this.TouchMoving = false;
	}

	// Token: 0x06001689 RID: 5769 RVA: 0x0011CC68 File Offset: 0x0011AE68
	private void OnStartItemEventAll(int index, GameObject go)
	{
		PguiButtonCtrl component = go.GetComponent<PguiButtonCtrl>();
		if (component)
		{
			component.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonQuestTop), PguiButtonCtrl.SoundType.DEFAULT);
			this.guiData.questTop.eventBannerButton.Add(component);
		}
		this.OnUpdateItemEventAll(index, go);
	}

	// Token: 0x0600168A RID: 5770 RVA: 0x0011CCB8 File Offset: 0x0011AEB8
	private void OnUpdateItemEventAll(int index, GameObject go)
	{
		if (this.enableEventDataList.Count <= 0)
		{
			return;
		}
		GameObject gameObject = go.transform.Find("EventTerm").gameObject;
		GameObject gameObject2 = go.transform.Find("BaseImage").gameObject;
		GameObject gameObject3 = go.transform.Find("Mark_EventBefore").gameObject;
		GameObject gameObject4 = go.transform.Find("Mark_EventOpen").gameObject;
		for (int i = 0; i < 1; i++)
		{
			int num = index + i;
			if (num < this.enableEventDataList.Count)
			{
				gameObject.SetActive(true);
				gameObject2.SetActive(true);
				DateTime startTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.enableEventDataList[num].eventChapterId].mapDataList[0].questGroupList[0].startTime;
				DateTime endTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.enableEventDataList[num].eventChapterId].mapDataList[0].questGroupList[0].endTime;
				PguiTextCtrl component = go.transform.Find("EventTerm/Num_Term01").GetComponent<PguiTextCtrl>();
				if (component != null)
				{
					if (this.enableEventDataList[num].IsEnableChapter)
					{
						component.m_Text.text = startTime.ToString("M/d") + "～" + endTime.ToString("M/d HH:mm まで");
					}
					else
					{
						component.m_Text.text = startTime.ToString("M/d HH:mm から開始！");
					}
				}
				PguiRawImageCtrl component2 = gameObject2.transform.Find("ChangeTexture").GetComponent<PguiRawImageCtrl>();
				if (component2 != null)
				{
					HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(this.enableEventDataList[num].eventBannerId);
					if (homeBannerData != null)
					{
						component2.banner = homeBannerData.bannerImagePathByQuestTop;
					}
				}
				if (this.enableEventDataList[num].IsEnableChapter)
				{
					gameObject3.SetActive(false);
					gameObject4.SetActive(true);
				}
				else
				{
					gameObject3.SetActive(true);
					gameObject4.SetActive(false);
				}
			}
			else
			{
				gameObject.SetActive(false);
				gameObject2.SetActive(false);
				gameObject3.SetActive(false);
				gameObject4.SetActive(false);
			}
		}
	}

	// Token: 0x0600168B RID: 5771 RVA: 0x0011CF0C File Offset: 0x0011B10C
	private void OnRegistTouchMove()
	{
		this.OnReleaseTouchMove();
		if (this.IsLargeEvent())
		{
			SGNFW.Touch.Manager.RegisterMove(new SGNFW.Touch.Manager.SingleAction(this.selEventLargeScaleCtrl.OnTouchMove));
			return;
		}
		SGNFW.Touch.Manager.RegisterMove(new SGNFW.Touch.Manager.SingleAction(this.OnTouchMove));
	}

	// Token: 0x0600168C RID: 5772 RVA: 0x0011CF44 File Offset: 0x0011B144
	private void OnReleaseTouchMove()
	{
		if (this.IsLargeEvent())
		{
			SGNFW.Touch.Manager.UnRegisterMove(new SGNFW.Touch.Manager.SingleAction(this.selEventLargeScaleCtrl.OnTouchMove));
			return;
		}
		SGNFW.Touch.Manager.UnRegisterMove(new SGNFW.Touch.Manager.SingleAction(this.OnTouchMove));
	}

	// Token: 0x0600168D RID: 5773 RVA: 0x0011CF76 File Offset: 0x0011B176
	private void OnRegistTouchRelease()
	{
		this.OnReleaseTouchRelease();
		if (this.IsLargeEvent())
		{
			SGNFW.Touch.Manager.RegisterRelease(new SGNFW.Touch.Manager.SingleAction(this.selEventLargeScaleCtrl.OnTouchRelease));
			return;
		}
		SGNFW.Touch.Manager.RegisterRelease(new SGNFW.Touch.Manager.SingleAction(this.OnTouchRelease));
	}

	// Token: 0x0600168E RID: 5774 RVA: 0x0011CFAE File Offset: 0x0011B1AE
	private void OnReleaseTouchRelease()
	{
		if (this.IsLargeEvent())
		{
			SGNFW.Touch.Manager.UnRegisterRelease(new SGNFW.Touch.Manager.SingleAction(this.selEventLargeScaleCtrl.OnTouchRelease));
			return;
		}
		SGNFW.Touch.Manager.UnRegisterRelease(new SGNFW.Touch.Manager.SingleAction(this.OnTouchRelease));
	}

	// Token: 0x0600168F RID: 5775 RVA: 0x0011CFE0 File Offset: 0x0011B1E0
	private void OnRegistTouchStart()
	{
		this.OnReleaseTouchStart();
		if (this.IsLargeEvent())
		{
			SGNFW.Touch.Manager.RegisterStart(new SGNFW.Touch.Manager.SingleAction(this.selEventLargeScaleCtrl.OnTouchStart));
			return;
		}
		SGNFW.Touch.Manager.RegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x0011D018 File Offset: 0x0011B218
	private void OnReleaseTouchStart()
	{
		if (this.IsLargeEvent())
		{
			SGNFW.Touch.Manager.UnRegisterStart(new SGNFW.Touch.Manager.SingleAction(this.selEventLargeScaleCtrl.OnTouchStart));
			return;
		}
		SGNFW.Touch.Manager.UnRegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x0011D04C File Offset: 0x0011B24C
	private void OnClickButtonMenuReturn()
	{
		if (this.IsPlayingAnim)
		{
			return;
		}
		if (this.currentMode == SceneQuest.Mode.CHARA_EDIT)
		{
			this.guiData.questTop.selAssistantCtrl.OnClickMenuReturn(delegate
			{
				this.guiData.questTop.Btn_AssistantEdit.gameObject.SetActive(true);
				this.guiData.questTop.selAssistantCtrl.guiData.tapGuard.SetActive(false);
				this.requestMode = SceneQuest.Mode.TOP;
			}, delegate
			{
				this.guiData.questTop.Btn_AssistantEdit.gameObject.SetActive(false);
				this.guiData.questTop.selAssistantCtrl.guiData.tapGuard.SetActive(true);
				this.requestMode = SceneQuest.Mode.CHARA_EDIT;
			});
			return;
		}
		if (this.currentSequence == this.guiData.questTop.baseObj)
		{
			this.requestNextScene = SceneManager.SceneName.SceneHome;
			this.requestNextSceneArgs = null;
		}
		else if (this.IsNotNullMainStoryObj() && this.currentSequence == this.selMainStoryCtrl.GuiData.pointSelect.baseObj)
		{
			this.reqNextSequence = this.guiData.questTop.baseObj;
		}
		else if (this.currentSequence == this.guiData.chapterSelect.baseObj)
		{
			if (SceneQuest.IsMainStory(this.selectData.questCategory))
			{
				this.reqNextSequence = this.selMainStoryCtrl.GuiData.pointSelect.baseObj;
			}
			else if (this.IsNotNullCharaStoryObj() && this.selectData.questCategory == QuestStaticChapter.Category.CHARA)
			{
				this.reqNextSequence = this.selCharaStoryCtrl.GuiData.charaSelect.baseObj;
			}
			else if (this.selectData.questCategory == QuestStaticChapter.Category.SIDE_STORY)
			{
				if (this.IsNotNullSideStoryObj())
				{
					this.reqNextSequence = this.selSideStoryCtrl.GuiData.mapSelect.baseObj;
				}
				else
				{
					this.reqNextSequence = this.guiData.questTop.baseObj;
				}
			}
			else if (this.selectData.questCategory == QuestStaticChapter.Category.ETCETERA)
			{
				if (this.IsNotNullEtcetraStoryObj())
				{
					this.reqNextSequence = this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj;
				}
				else
				{
					this.reqNextSequence = this.guiData.questTop.baseObj;
				}
			}
			else if (this.selectData.questCategory == QuestStaticChapter.Category.EVENT)
			{
				if (this.currentEnableEventData != null)
				{
					switch (this.currentEnableEventData.eventCategory)
					{
					case DataManagerEvent.Category.Scenario:
						if (this.IsNotNullEventScenarioObj())
						{
							this.isBackToMap = true;
							this.reqNextSequence = this.selEventScenarioCtrl.GuiData.eventSelect.baseObj;
						}
						else
						{
							this.reqNextSequence = this.guiData.questTop.baseObj;
						}
						break;
					case DataManagerEvent.Category.Growth:
						if (this.IsNotNullEventCharaGrowObj())
						{
							this.reqNextSequence = this.guiData.questTop.baseObj;
						}
						break;
					case DataManagerEvent.Category.Large:
						if (this.IsNotNullEventLargeScaleObj())
						{
							this.reqNextSequence = this.selEventLargeScaleCtrl.GuiData.pointSelect.baseObj;
						}
						break;
					case DataManagerEvent.Category.Tower:
						if (this.IsNotNullEventTowerObj())
						{
							this.reqNextSequence = this.selEventTowerCtrl.GuiData.eventSelect.baseObj;
						}
						else
						{
							this.reqNextSequence = this.guiData.questTop.baseObj;
						}
						break;
					case DataManagerEvent.Category.Coop:
						if (this.IsNotNullEventCoopObj())
						{
							this.reqNextSequence = this.selEventCoopCtrl.GuiData.mapSelect.baseObj;
						}
						else
						{
							this.reqNextSequence = this.guiData.questTop.baseObj;
						}
						break;
					case DataManagerEvent.Category.WildRelease:
						if (this.IsNotNullEventWildReleaseObj())
						{
							this.reqNextSequence = this.guiData.questTop.baseObj;
						}
						break;
					}
				}
			}
			else
			{
				this.reqNextSequence = this.guiData.questTop.baseObj;
			}
		}
		else if (this.IsNotNullCharaStoryObj() && this.currentSequence == this.selCharaStoryCtrl.GuiData.charaSelect.baseObj)
		{
			this.reqNextSequence = this.guiData.questTop.baseObj;
		}
		else if (this.IsScenarioEvent())
		{
			if (this.currentSequence == this.selEventScenarioCtrl.GuiData.eventSelect.baseObj)
			{
				this.reqNextSequence = this.guiData.questTop.baseObj;
			}
		}
		else if (this.IsCharaGrowEvent())
		{
			if (this.currentSequence == this.selEventCharaGrowCtrl.GuiData.charaSelect.baseObj)
			{
				this.reqNextSequence = ((this.currentEnableEventData.SelectGrowthCharaData.Id == 0) ? this.guiData.questTop.baseObj : this.guiData.chapterSelect.baseObj);
				this.selEventCharaGrowCtrl.SetActiveChapterSelect(true);
			}
		}
		else if (this.IsLargeEvent())
		{
			if (this.currentSequence == this.selEventLargeScaleCtrl.GuiData.pointSelect.baseObj)
			{
				this.reqNextSequence = this.guiData.questTop.baseObj;
			}
		}
		else if (this.IsTowerEvent())
		{
			if (this.currentSequence == this.selEventTowerCtrl.GuiData.eventSelect.baseObj)
			{
				this.reqNextSequence = this.guiData.questTop.baseObj;
			}
		}
		else if (this.IsCoopEvent())
		{
			if (this.currentSequence == this.selEventCoopCtrl.GuiData.mapSelect.baseObj)
			{
				this.reqNextSequence = this.guiData.questTop.baseObj;
			}
		}
		else if (!this.IsWildReleaseEvent())
		{
			if (this.selectData.questCategory == QuestStaticChapter.Category.SIDE_STORY && this.IsNotNullSideStoryObj())
			{
				if (this.currentSequence == this.selSideStoryCtrl.GuiData.mapSelect.baseObj)
				{
					this.reqNextSequence = this.guiData.questTop.baseObj;
				}
			}
			else if (this.selectData.questCategory == QuestStaticChapter.Category.ETCETERA && this.IsNotNullEtcetraStoryObj() && this.currentSequence == this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj)
			{
				if (this.questArgs != null && this.questArgs.menuBackSceneName != SceneManager.SceneName.None)
				{
					this.requestNextScene = this.questArgs.menuBackSceneName;
					this.requestNextSceneArgs = this.questArgs.menuBackSceneArgs;
				}
				else
				{
					this.reqNextSequence = this.guiData.questTop.baseObj;
				}
			}
		}
		if (this.reqNextSequence == null)
		{
			CanvasManager.HdlCmnMenu.AnimEndTitleBase();
		}
	}

	// Token: 0x06001692 RID: 5778 RVA: 0x0011D6CC File Offset: 0x0011B8CC
	private void OnClickButtonOther(PguiButtonCtrl button)
	{
		if (this.IsPlayingAnim)
		{
			return;
		}
		if (button == this.guiData.chapterSelect.Btn_SortFilterOnOff)
		{
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("既存ストーリーの再生設定"), PrjUtil.MakeMessage("クエストをプレイする際に\n" + (DataManager.DmUserInfo.optionData.secondScenarioSkip ? "<color=red>一度見たストーリーを再生する設定</color>" : "<color=red>一度見たストーリーを再生しない設定</color>") + "に変更します。"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return;
		}
		if (button == this.guiData.chapterSelect.Btn_Info)
		{
			this.guiData.chapterSelect.questButtonGroup.SwitchInfo();
			return;
		}
		if (button == this.guiData.chapterSelect.Btn_Sel_Difficult)
		{
			if (this.guiData.chapterSelect.markLock.IsActive())
			{
				QuestOnePackData questOnePackDataForReleaseIdStoryHardMode = SceneQuest.GetQuestOnePackDataForReleaseIdStoryHardMode(this.selectData.questCategory);
				if (questOnePackDataForReleaseIdStoryHardMode != null)
				{
					CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
					{
						new CmnReleaseConditionWindowCtrl.SetupParam
						{
							text = string.Concat(new string[]
							{
								SceneQuest.GetMainStoryName(questOnePackDataForReleaseIdStoryHardMode.questChapter.category, false),
								" ",
								questOnePackDataForReleaseIdStoryHardMode.questChapter.chapterName,
								questOnePackDataForReleaseIdStoryHardMode.questGroup.titleName,
								PrjUtil.MakeMessage("クリア")
							}),
							enableClear = false
						}
					});
					return;
				}
			}
			else if (this.IsEnableHardMode(true))
			{
				int num = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId).FindIndex((int item) => item == this.selectData.mapId);
				this.selectDifficultCount++;
				if (this.IsNormalMode())
				{
					QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.hardChapterId == this.selectData.chapterId);
					if (questStaticChapter != null)
					{
						this.selectData.chapterId = questStaticChapter.chapterId;
						List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId);
						this.selectData.mapId = ((num >= 0 && playableMapIdList.Count > num) ? playableMapIdList[num] : playableMapIdList[playableMapIdList.Count - 1]);
					}
				}
				else
				{
					QuestStaticChapter questStaticChapter2 = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == this.selectData.chapterId);
					if (questStaticChapter2 != null)
					{
						this.selectData.chapterId = questStaticChapter2.hardChapterId;
						List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId);
						this.selectData.mapId = ((num >= 0 && playableMapIdList2.Count > num) ? playableMapIdList2[num] : playableMapIdList2[playableMapIdList2.Count - 1]);
					}
				}
				this.reqNextSequence = this.guiData.chapterSelect.baseObj;
				this.UpdateButtonLR();
				return;
			}
		}
		else if (button == this.guiData.chapterSelect.Btn_Mission && (this.IsCharaGrowEvent() || this.IsWildReleaseEvent()))
		{
			SceneMission.MissionOpenParam missionOpenParam = new SceneMission.MissionOpenParam(MissionType.EVENTTOTAL, this.currentEnableEventData.eventId)
			{
				returnSceneName = SceneManager.SceneName.SceneQuest,
				resultNextSceneArgs = new SceneQuest.Args
				{
					category = QuestStaticChapter.Category.EVENT,
					selectEventId = this.currentEnableEventData.eventId,
					backSequenceGameObject = this.guiData.chapterSelect.baseObj
				}
			};
			this.requestNextScene = SceneManager.SceneName.SceneMission;
			this.requestNextSceneArgs = missionOpenParam;
		}
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x0011DA60 File Offset: 0x0011BC60
	private void OnClickButtonLocation(PguiButtonCtrl buttuon)
	{
		if (this.IsNotNullCharaStoryObj() && buttuon == this.guiData.locationInfo.Btn_MoreInfo)
		{
			CharaPackData charaPackData = DataManager.DmChara.GetUserCharaData(this.selCharaStoryCtrl.SelectCharaId);
			if (charaPackData == null)
			{
				charaPackData = CharaPackData.MakeInitial(this.selCharaStoryCtrl.SelectCharaId);
			}
			CanvasManager.HdlCharaWindowCtrl.Open(charaPackData, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY_NO_SCENARIO, this.selCharaStoryCtrl.OriginalDispCharaPackList), delegate
			{
				this.selCharaStoryCtrl.GuiData.charaSelect.ScrollView.Refresh();
			});
		}
	}

	// Token: 0x06001694 RID: 5780 RVA: 0x0011DAE0 File Offset: 0x0011BCE0
	private void OnClickButtonCharaGrow(PguiButtonCtrl buttuon)
	{
		if (buttuon == this.guiData.charaGrow.Btn_Schedule)
		{
			this.questScheduleWindow.Setup((int index) => true);
		}
	}

	// Token: 0x06001695 RID: 5781 RVA: 0x0011DB30 File Offset: 0x0011BD30
	private bool OnSelectOpenWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			bool flag = !DataManager.DmUserInfo.optionData.secondScenarioSkip;
			this.guiData.chapterSelect.Btn_SortFilterOnOff.transform.Find("BaseImage/On").gameObject.SetActive(!flag);
			this.guiData.chapterSelect.Btn_SortFilterOnOff.transform.Find("BaseImage/Off").gameObject.SetActive(flag);
			UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
			userOptionData.secondScenarioSkip = flag;
			DataManager.DmUserInfo.RequestActionUpdateUserOption(userOptionData);
		}
		return true;
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x0011DBD4 File Offset: 0x0011BDD4
	private void OnClickButtonQuestTop(PguiButtonCtrl button)
	{
		if (this.CheckSeal(button))
		{
			string textSeal = this.GetTextSeal(button);
			string url = this.GetUrlSeal(button);
			List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			list.Clear();
			list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "閉じる"));
			if (!string.IsNullOrEmpty(url))
			{
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "お知らせ"));
			}
			CanvasManager.HdlOpenWindowBasic.Setup("", string.IsNullOrEmpty(textSeal) ? "現在ご利用いただけません" : textSeal, list, false, delegate(int idx)
			{
				if (idx == 1)
				{
					CanvasManager.HdlWebViewWindowCtrl.Open(url);
					return false;
				}
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return;
		}
		if (this.IsPlayingAnim)
		{
			return;
		}
		if (this.IsNotNullEventCharaGrowObj())
		{
			this.selEventCharaGrowCtrl.Dest();
		}
		if (this.IsNotNullEventLargeScaleObj())
		{
			this.selEventLargeScaleCtrl.Dest();
		}
		if (this.IsNotNullEventScenarioObj())
		{
			this.selEventScenarioCtrl.Dest();
		}
		if (this.IsNotNullEventTowerObj())
		{
			this.selEventTowerCtrl.Dest();
		}
		if (this.IsNotNullSideStoryObj())
		{
			this.selSideStoryCtrl.Dest();
		}
		if (this.IsNotNullEtcetraStoryObj())
		{
			this.selEtcetraStoryCtrl.Dest();
		}
		if (this.IsNotNullEventCoopObj())
		{
			this.selEventCoopCtrl.Dest();
		}
		if (this.IsNotNullEventWildReleaseObj())
		{
			this.selEventWildReleaseCtrl.Dest();
		}
		bool flag = false;
		this.currentEnableEventData = null;
		QuestStaticChapter.Category questCategory = this.selectData.questCategory;
		if (button == this.guiData.questTop.Btn_GrowQuest)
		{
			if (this.guiData.questTop.markLockGrowQuest.IsActive())
			{
				QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.GROW));
				if (questOnePackData != null)
				{
					CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
					{
						new CmnReleaseConditionWindowCtrl.SetupParam
						{
							text = string.Concat(new string[]
							{
								SceneQuest.GetMainStoryName(questOnePackData.questChapter.category, false),
								" ",
								questOnePackData.questChapter.chapterName,
								questOnePackData.questGroup.titleName,
								PrjUtil.MakeMessage("クリア")
							}),
							enableClear = false
						}
					});
				}
			}
			else
			{
				this.reqNextSequence = this.guiData.chapterSelect.baseObj;
				this.selectData.questCategory = QuestStaticChapter.Category.GROW;
			}
		}
		else if (button == this.guiData.questTop.Btn_EtceteraQuest)
		{
			if (this.guiData.questTop.markLockEtceteraQuest.IsActive())
			{
				QuestOnePackData questOnePackData2 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.ETCETERA));
				if (questOnePackData2 != null)
				{
					CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
					{
						new CmnReleaseConditionWindowCtrl.SetupParam
						{
							text = string.Concat(new string[]
							{
								SceneQuest.GetMainStoryName(questOnePackData2.questChapter.category, false),
								" ",
								questOnePackData2.questChapter.chapterName,
								questOnePackData2.questGroup.titleName,
								PrjUtil.MakeMessage("クリア")
							}),
							enableClear = false
						}
					});
				}
			}
			else
			{
				if (this.selEtcetraStoryCtrl == null)
				{
					this.InitEtcetraStory();
				}
				else
				{
					this.selEtcetraStoryCtrl.Setup(new SelEtcetraStoryCtrl.SetupParam
					{
						reqNextSequenceCB = delegate
						{
							this.reqNextSequence = this.selEtcetraStoryCtrl.GuiData.mapSelect.baseObj;
						},
						selectData = this.selectData
					});
				}
				this.selectData.questCategory = QuestStaticChapter.Category.ETCETERA;
				flag = true;
			}
		}
		else if (button == this.guiData.questTop.Btn_CharQuest)
		{
			if (this.guiData.questTop.markLockCharaQuest.IsActive())
			{
				QuestOnePackData questOnePackData3 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.CHARA));
				if (questOnePackData3 != null)
				{
					CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
					{
						new CmnReleaseConditionWindowCtrl.SetupParam
						{
							text = string.Concat(new string[]
							{
								SceneQuest.GetMainStoryName(questOnePackData3.questChapter.category, false),
								" ",
								questOnePackData3.questChapter.chapterName,
								questOnePackData3.questGroup.titleName,
								PrjUtil.MakeMessage("クリア")
							}),
							enableClear = false
						}
					});
				}
			}
			else
			{
				if (this.selCharaStoryCtrl == null)
				{
					this.InitCharaQuest();
				}
				else
				{
					this.selCharaStoryCtrl.Setup(new SelCharaStoryCtrl.SetupParam());
				}
				this.reqNextSequence = this.selCharaStoryCtrl.GuiData.charaSelect.baseObj;
				this.selectData.questCategory = QuestStaticChapter.Category.CHARA;
			}
		}
		else if (button == this.guiData.questTop.storyQuestParts.Btn_StoryQuest)
		{
			if (this.guiData.questTop.storyQuestParts.markLock.IsActive())
			{
				if (SceneQuest.IsMainStoryPart1_5(this.StoryQuestSwitchCount))
				{
					QuestOnePackData questOnePackData4 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.CELLVAL));
					if (questOnePackData4 != null)
					{
						CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
						{
							new CmnReleaseConditionWindowCtrl.SetupParam
							{
								text = string.Concat(new string[]
								{
									SceneQuest.GetMainStoryName(questOnePackData4.questChapter.category, false),
									" ",
									questOnePackData4.questChapter.chapterName,
									questOnePackData4.questGroup.titleName,
									PrjUtil.MakeMessage("クリア")
								}),
								enableClear = false
							}
						});
					}
				}
				else if (SceneQuest.IsMainStoryPart2(this.StoryQuestSwitchCount))
				{
					QuestOnePackData questOnePackData5 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.STORY2));
					if (questOnePackData5 != null)
					{
						CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
						{
							new CmnReleaseConditionWindowCtrl.SetupParam
							{
								text = string.Concat(new string[]
								{
									SceneQuest.GetMainStoryName(questOnePackData5.questChapter.category, false),
									" ",
									questOnePackData5.questChapter.chapterName,
									questOnePackData5.questGroup.titleName,
									PrjUtil.MakeMessage("クリア")
								}),
								enableClear = false
							}
						});
					}
				}
				else if (SceneQuest.IsMainStoryPart3(this.StoryQuestSwitchCount))
				{
					QuestOnePackData questOnePackData6 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.STORY3));
					if (questOnePackData6 != null)
					{
						CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
						{
							new CmnReleaseConditionWindowCtrl.SetupParam
							{
								text = string.Concat(new string[]
								{
									SceneQuest.GetMainStoryName(questOnePackData6.questChapter.category, false),
									" ",
									questOnePackData6.questChapter.chapterName,
									questOnePackData6.questGroup.titleName,
									PrjUtil.MakeMessage("クリア")
								}),
								enableClear = false
							}
						});
					}
				}
			}
			else
			{
				if (this.selMainStoryCtrl == null)
				{
					this.InitSelMainStoryCtrl();
				}
				else
				{
					this.selMainStoryCtrl.Setup(new SelMainStoryCtrl.SetupParam());
				}
				this.reqNextSequence = this.selMainStoryCtrl.GuiData.pointSelect.baseObj;
				this.DestroyMap();
				if (this.questArgs != null && this.questArgs.tutorialSequence == TutorialUtil.Sequence.QUEST_GUIDE)
				{
					CanvasManager.HdlTutorialMaskCtrl.SetEnable(false);
					CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(false);
					CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
					CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
					CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
				}
				string text = SceneQuest.MainStoryMapPath;
				if (SceneQuest.IsMainStoryPart1(this.StoryQuestSwitchCount))
				{
					this.selectData.questCategory = QuestStaticChapter.Category.STORY;
					text = SceneQuest.MainStoryMapPath;
				}
				else if (SceneQuest.IsMainStoryPart1_5(this.StoryQuestSwitchCount))
				{
					this.selectData.questCategory = QuestStaticChapter.Category.CELLVAL;
					text = SceneQuest.CellvalMapPath;
				}
				else if (SceneQuest.IsMainStoryPart2(this.StoryQuestSwitchCount))
				{
					this.selectData.questCategory = QuestStaticChapter.Category.STORY2;
					text = SceneQuest.MainStory2MapPath;
				}
				else if (SceneQuest.IsMainStoryPart3(this.StoryQuestSwitchCount))
				{
					this.selectData.questCategory = QuestStaticChapter.Category.STORY3;
					text = SceneQuest.MainStory3MapPath;
				}
				this.mainChapterChangeEffect = this.MainChapterChangeEffect();
				this.instCarObjList = SceneQuest.CreateCarObjList(SceneQuest.mapBoxObject);
				this.requestSequenceInstantiateAssetData = Singleton<SceneManager>.Instance.StartCoroutine(this.InstantiateAssetData(this.LoadMapObject(text), text));
				flag = true;
			}
		}
		else if (button == this.guiData.questTop.storyQuestParts.Btn_StorySelectL || button == this.guiData.questTop.storyQuestParts.Btn_StorySelectR)
		{
			int num = this.StoryQuestSwitchCount;
			int num2 = (this.guiData.questTop.storyQuestParts.Btn_StorySelectR.ActEnable ? 1 : 0);
			int num3 = (this.guiData.questTop.storyQuestParts.Btn_StorySelectL.ActEnable ? (-1) : 0);
			num += ((button == this.guiData.questTop.storyQuestParts.Btn_StorySelectL) ? num3 : num2);
			this.StoryQuestSwitchCount = (num + this.guiData.questTop.storyQuestParts.GetMainStorySize()) % this.guiData.questTop.storyQuestParts.GetMainStorySize();
			if ((this.guiData.questTop.storyQuestParts.Btn_StorySelectR.ActEnable && button == this.guiData.questTop.storyQuestParts.Btn_StorySelectR) || (this.guiData.questTop.storyQuestParts.Btn_StorySelectL.ActEnable && button == this.guiData.questTop.storyQuestParts.Btn_StorySelectL))
			{
				this.guiData.questTop.storyQuestParts.StoryPhoto_Anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
				QuestStaticChapter.Category category = QuestStaticChapter.Category.STORY;
				if (SceneQuest.IsMainStoryPart1_5(this.StoryQuestSwitchCount))
				{
					category = QuestStaticChapter.Category.CELLVAL;
				}
				else if (SceneQuest.IsMainStoryPart2(this.StoryQuestSwitchCount))
				{
					category = QuestStaticChapter.Category.STORY2;
				}
				else if (SceneQuest.IsMainStoryPart3(this.StoryQuestSwitchCount))
				{
					category = QuestStaticChapter.Category.STORY3;
				}
				this.guiData.questTop.ResetCampaignInfoCategory(category, 0);
				this.guiData.questTop.UpdateCampaignInfoCategory(category, 0);
			}
			this.guiData.questTop.storyQuestParts.Setup(this.StoryQuestSwitchCount);
			this.UpdateBtnStorySelect(DataManager.DmGameStatus.MakeUserFlagData());
			DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
			if (userFlagData.ReleaseModeFlag.CellvalQuestOpen == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked)
			{
				PguiAECtrl component = this.guiData.questTop.storyQuestParts.Btn_StorySelectR.transform.Find("AEImage_BtnLight").GetComponent<PguiAECtrl>();
				Component component2 = this.guiData.questTop.storyQuestParts.Btn_StorySelectL.transform.Find("AEImage_BtnLight").GetComponent<PguiAECtrl>();
				component.gameObject.SetActive(false);
				component2.gameObject.SetActive(false);
				userFlagData.ReleaseModeFlag.CellvalQuestOpen = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
				DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
			}
		}
		else if (button == this.guiData.questTop.Btn_AnotherStory)
		{
			if (this.guiData.questTop.markLockAnotherStoryQuest.IsActive())
			{
				QuestOnePackData questOnePackData7 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.SIDE_STORY));
				if (questOnePackData7 != null)
				{
					CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
					{
						new CmnReleaseConditionWindowCtrl.SetupParam
						{
							text = string.Concat(new string[]
							{
								SceneQuest.GetMainStoryName(questOnePackData7.questChapter.category, false),
								" ",
								questOnePackData7.questChapter.chapterName,
								questOnePackData7.questGroup.titleName,
								PrjUtil.MakeMessage("クリア")
							}),
							enableClear = false
						}
					});
				}
			}
			else
			{
				if (this.selSideStoryCtrl == null)
				{
					GameObject gameObject = new GameObject();
					RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
					rectTransform.anchorMin = new Vector2(0f, 0f);
					rectTransform.anchorMax = new Vector2(1f, 1f);
					rectTransform.offsetMin = new Vector2(0f, 0f);
					rectTransform.offsetMax = new Vector2(0f, 0f);
					gameObject.name = "SelSideStoryCtrl";
					gameObject.transform.SetParent(this.guiData.basePanel.transform, false);
					this.selSideStoryCtrl = gameObject.AddComponent<SelSideStoryCtrl>();
					this.selSideStoryCtrl.Init(new SelSideStoryCtrl.InitParam
					{
						reqNextSequenceCB = delegate
						{
							this.reqNextSequence = this.guiData.chapterSelect.baseObj;
						},
						reqBackSequenceCB = delegate
						{
							this.reqNextSequence = this.selSideStoryCtrl.GuiData.mapSelect.baseObj;
						},
						selectObjsCB = delegate
						{
							this.guiData.selectObjs.Add(this.selSideStoryCtrl.GuiData.mapSelect.baseObj);
						},
						prefabPath = this.guiData.basePanel.transform.Find("AraiQuest_ChapterChange")
					}, new SelSideStoryCtrl.SetupParam
					{
						reqNextSequenceCB = delegate
						{
							this.reqNextSequence = this.selSideStoryCtrl.GuiData.mapSelect.baseObj;
						},
						selectData = this.selectData
					});
					this.guiData.basePanel.transform.Find("AraiQuest_ChapterChange").SetParent(this.selSideStoryCtrl.transform, false);
				}
				else
				{
					this.selSideStoryCtrl.Setup(new SelSideStoryCtrl.SetupParam
					{
						reqNextSequenceCB = delegate
						{
							this.reqNextSequence = this.selSideStoryCtrl.GuiData.mapSelect.baseObj;
						},
						selectData = this.selectData
					});
				}
				this.sideStoryChapterChangeEffect = this.SideStoryChapterChangeEffect();
				this.selectData.questCategory = QuestStaticChapter.Category.SIDE_STORY;
				DataManagerGameStatus.UserFlagData userFlagData2 = DataManager.DmGameStatus.MakeUserFlagData();
				if (userFlagData2.ReleaseModeFlag.AraiDiaryOpen == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked)
				{
					CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/Araisanjournal/tutorial_araisanjournal_01" }, null);
					userFlagData2.ReleaseModeFlag.AraiDiaryOpen = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
					DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData2);
				}
				flag = true;
			}
		}
		else if (button == this.guiData.questTop.Btn_Training)
		{
			if (this.guiData.questTop.markLockTrainingQuest.IsActive())
			{
				QuestOnePackData questOnePackData8 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.TRAINING));
				if (questOnePackData8 != null)
				{
					CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
					{
						new CmnReleaseConditionWindowCtrl.SetupParam
						{
							text = string.Concat(new string[]
							{
								SceneQuest.GetMainStoryName(questOnePackData8.questChapter.category, false),
								" ",
								questOnePackData8.questChapter.chapterName,
								questOnePackData8.questGroup.titleName,
								PrjUtil.MakeMessage("クリア")
							}),
							enableClear = false
						}
					});
				}
			}
			else
			{
				this.requestNextScene = SceneManager.SceneName.SceneTraining;
				this.selectData.questCategory = QuestStaticChapter.Category.TRAINING;
			}
		}
		else if (this.guiData.questTop.eventButton.Contains(button) || this.guiData.questTop.eventBannerButton.Contains(button))
		{
			int num4 = (this.guiData.questTop.eventButton.Contains(button) ? this.guiData.questTop.eventButton.IndexOf(button) : this.guiData.questTop.eventBannerButton.IndexOf(button));
			this.currentEnableEventData = this.enableEventDataList[num4];
			int eventId = this.currentEnableEventData.eventId;
			QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.currentEnableEventData.eventChapterId];
			HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(this.currentEnableEventData.eventBannerId);
			if (homeBannerData == null)
			{
				return;
			}
			if (homeBannerData.eventId <= 0)
			{
				CanvasManager.HdlWebViewWindowCtrl.Open(homeBannerData.actionParamURL);
				return;
			}
			this.selectData.eventId = this.currentEnableEventData.eventId;
			this.selectData.chapterId = this.currentEnableEventData.eventChapterId;
			this.selectData.questCategory = QuestStaticChapter.Category.EVENT;
			this.InitEventCtrl(this.currentEnableEventData.eventCategory);
			if (this.currentEnableEventData.eventCategory == DataManagerEvent.Category.Large)
			{
				flag = true;
			}
		}
		else if (button == this.guiData.questTop.Btn_EventAll)
		{
			this.buttonEventAllCount++;
			this.UpdateButtonEventAllImage();
		}
		if (this.currentMode == SceneQuest.Mode.TOP)
		{
			if (button == this.guiData.questTop.Btn_AssistantEdit)
			{
				if (this.guiData.questTop.markLockAssistantEdit.IsActive())
				{
					QuestOnePackData questOnePackData9 = DataManager.DmQuest.GetQuestOnePackData(QuestUtil.ClearConditionQuestOneId(QuestStaticChapter.Category.ASSISTANT));
					if (questOnePackData9 != null)
					{
						CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
						{
							new CmnReleaseConditionWindowCtrl.SetupParam
							{
								text = string.Concat(new string[]
								{
									SceneQuest.GetMainStoryName(questOnePackData9.questChapter.category, false),
									" ",
									questOnePackData9.questChapter.chapterName,
									questOnePackData9.questGroup.titleName,
									PrjUtil.MakeMessage("クリア")
								}),
								enableClear = false
							}
						});
					}
				}
				else
				{
					this.selectData.questCategory = QuestStaticChapter.Category.ASSISTANT;
					button.gameObject.SetActive(false);
					this.guiData.questTop.selAssistantCtrl.OnClickAssistantButton();
					if (!QuestUtil.IsDispDhole())
					{
						this.requestMode = SceneQuest.Mode.TOP;
					}
					else
					{
						this.requestMode = SceneQuest.Mode.CHARA_EDIT;
					}
					this.guiData.questTop.selAssistantCtrl.guiData.tapGuard.SetActive(true);
				}
			}
		}
		else if (this.currentMode == SceneQuest.Mode.CHARA_EDIT && button == this.guiData.questTop.selAssistantCtrl.guiData.Btn_EditOk)
		{
			this.guiData.questTop.Btn_AssistantEdit.gameObject.SetActive(true);
			this.guiData.questTop.selAssistantCtrl.guiData.tapGuard.SetActive(false);
			this.guiData.questTop.selAssistantCtrl.OnClickEditOk(button);
			this.requestMode = SceneQuest.Mode.TOP;
		}
		if (this.selectData.questCategory != QuestStaticChapter.Category.TRAINING && this.selectData.questCategory != QuestStaticChapter.Category.ASSISTANT)
		{
			Action action = delegate
			{
				bool flag2 = this.IsNormalMode();
				bool flag3 = QuestUtil.IsHardMode(this.selectData);
				if (!flag2 && !flag3)
				{
					this.selectDifficultCount = 0;
					return;
				}
				if (flag2 && flag3)
				{
					this.selectDifficultCount = 1;
				}
			};
			int num5 = DataManager.DmUserInfo.optionData.LastPlayQuestOneIdList[(int)this.selectData.questCategory];
			QuestOnePackData questOnePackData10 = DataManager.DmQuest.GetQuestOnePackData(num5);
			if (questOnePackData10 != null && flag)
			{
				if (this.selectData.chapterId == 0 || this.selectData.questCategory != questCategory || this.selectData.mapId != questOnePackData10.questMap.mapId)
				{
					this.selectData.mapId = questOnePackData10.questMap.mapId;
					this.selectData.chapterId = questOnePackData10.questChapter.chapterId;
					action();
					return;
				}
			}
			else if (this.selectData.questCategory != QuestStaticChapter.Category.EVENT)
			{
				List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.questCategory);
				if (playableMapIdList.Count > 0)
				{
					playableMapIdList.Sort((int a, int b) => a - b);
					this.selectData.mapId = playableMapIdList[0];
					QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap n) => n.mapId == this.selectData.mapId);
					this.selectData.chapterId = questStaticMap.chapterId;
					if (SceneQuest.IsMainStory(this.selectData.questCategory))
					{
						action();
					}
				}
			}
		}
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x0011F034 File Offset: 0x0011D234
	private void OnClickButtonPointSelectLargeEvent(Transform point)
	{
		if (this.selEventLargeScaleCtrl.TouchMoving)
		{
			return;
		}
		if (this.IsPlayingAnim)
		{
			return;
		}
		SoundManager.Play("prd_se_click", false, false);
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId);
		int pointNumber = int.Parse(point.name);
		QuestStaticMap mapData = DataManager.DmQuest.QuestStaticData.mapDataMap[pointNumber];
		List<QuestStaticQuestGroup> list = new List<QuestStaticQuestGroup>(mapData.questGroupList);
		list.Sort((QuestStaticQuestGroup a, QuestStaticQuestGroup b) => a.questGroupId - b.questGroupId);
		List<QuestStaticQuestOne> list2 = new List<QuestStaticQuestOne>(list[0].questOneList);
		list2.Sort((QuestStaticQuestOne a, QuestStaticQuestOne b) => a.questId - b.questId);
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(list2[0].relQuestId);
		QuestUIMapInfo questUIMapInfo = QuestUIMapInfo.GetQuestUIMapInfo(pointNumber, SceneQuest.TimeStampInScene, this.currentEnableEventData.eventId);
		List<QuestStaticQuestOne.ReleaseConditions> enableReleaseConditionsList = QuestUtil.GetEnableReleaseConditionsList(pointNumber);
		PguiOpenWindowCtrl.Callback <>9__4;
		Action action = delegate
		{
			if (questUIMapInfo.openItemData != null)
			{
				ItemData userItemData = DataManager.DmItem.GetUserItemData(questUIMapInfo.openItemData.id);
				string name = questUIMapInfo.openItemData.staticData.GetName();
				if (userItemData.num >= questUIMapInfo.openItemData.num)
				{
					string text = string.Format("{0} を{1}個消費して進みますか？", name, questUIMapInfo.openItemData.num);
					PguiOpenWindowCtrl hdlOpenWindowUseItem = CanvasManager.HdlOpenWindowUseItem;
					string text2 = "確認";
					string text3 = text;
					PguiOpenWindowCtrl.Callback callback;
					if ((callback = <>9__4) == null)
					{
						callback = (<>9__4 = delegate(int index)
						{
							if (index == 1)
							{
								this.selEventLargeScaleCtrl.RequestUseUnlockItem(mapData.mapId);
							}
							return true;
						});
					}
					hdlOpenWindowUseItem.SetupByUseItem(text2, text3, callback, questUIMapInfo.openItemData.num, userItemData.num, false);
					CanvasManager.HdlOpenWindowUseItem.Open();
					return;
				}
				string text4 = string.Format("{0} が{1}つ不足しています\n（所持数 {2}{3}{4} / 必要数 {5}）", new object[]
				{
					name,
					Mathf.Abs(userItemData.num - questUIMapInfo.openItemData.num),
					PrjUtil.ColorRedStartTag,
					userItemData.num,
					PrjUtil.ColorEndTag,
					questUIMapInfo.openItemData.num
				});
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), text4, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
			}
		};
		if (!playableMapIdList.Exists((int n) => n == pointNumber))
		{
			if (questUIMapInfo.isLockByTime)
			{
				QuestUtil.OpenReleaseConditionWindow(pointNumber, true, questUIMapInfo);
				return;
			}
			if (!questUIMapInfo.isLockByItem)
			{
				QuestUtil.OpenReleaseConditionWindow(pointNumber, false, questUIMapInfo);
				return;
			}
			bool flag = true;
			using (List<QuestStaticQuestOne.ReleaseConditions>.Enumerator enumerator = enableReleaseConditionsList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestStaticQuestOne.ReleaseConditions e = enumerator.Current;
					QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataList.Find((QuestDynamicQuestOne item) => item.questOneId == e.questId);
					flag &= questDynamicQuestOne != null && questDynamicQuestOne.status != QuestOneStatus.NEW;
				}
			}
			if (questOnePackData == null)
			{
				action();
				return;
			}
			if (flag)
			{
				action();
				return;
			}
			QuestUtil.OpenReleaseConditionWindow(pointNumber, false, questUIMapInfo);
			return;
		}
		else
		{
			if (questUIMapInfo.isLockByTime)
			{
				QuestUtil.OpenReleaseConditionWindow(pointNumber, true, questUIMapInfo);
				return;
			}
			if (questUIMapInfo.isLockByItem)
			{
				action();
				return;
			}
			this.selectData.mapId = pointNumber;
			this.reqNextSequence = this.guiData.chapterSelect.baseObj;
			this.selEventLargeScaleCtrl.MapData.mapCar.transform.position = point.transform.position;
			return;
		}
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x0011F300 File Offset: 0x0011D500
	private void OnClickButtonQuestEvent(PguiButtonCtrl button)
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.selectData.eventId);
		if (eventData == null)
		{
			return;
		}
		if (button == this.guiData.locationEvent.Btn_ShopEvent)
		{
			if (this.IsTowerEvent())
			{
				SceneGacha.OpenParam openParam = new SceneGacha.OpenParam
				{
					gachaId = this.currentEnableEventData.eventGachaId,
					resultNextSceneName = SceneManager.SceneName.SceneQuest,
					resultNextSceneArgs = new SceneQuest.Args
					{
						selectEventId = this.currentEnableEventData.eventId,
						category = QuestStaticChapter.Category.EVENT,
						backSequenceGameObject = this.currentSequence
					}
				};
				this.requestNextScene = SceneManager.SceneName.SceneGacha;
				this.requestNextSceneArgs = openParam;
				return;
			}
			this.SetupNextSceneChangeShop(eventData.eventId);
		}
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x0011F3B0 File Offset: 0x0011D5B0
	private void OnClickEventInfoBanner(Transform tf)
	{
		QuestUtil.OpenBannerWebViewWindow(this.selectData.eventId);
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x0011F3C4 File Offset: 0x0011D5C4
	private void OnClickButtonPointSelect(PguiButtonCtrl button)
	{
		if (this.IsPlayingAnim)
		{
			return;
		}
		if (button == this.selMainStoryCtrl.GuiData.pointSelect.Btn_ChapterChange)
		{
			this.questChapterChangeWindow.Setup(this.selectData.questCategory, this.selectDifficultCount, this.selectData.chapterId);
			this.questChapterChangeWindow.CategoryByOpenWindow = this.selectData.questCategory;
			return;
		}
		if (button == this.selMainStoryCtrl.GuiData.pointSelect.Btn_Mission)
		{
			SceneMission.MissionOpenParam missionOpenParam = new SceneMission.MissionOpenParam(MissionType.DAILY, 0)
			{
				returnSceneName = SceneManager.SceneName.SceneQuest,
				resultNextSceneArgs = new SceneQuest.Args
				{
					category = this.selectData.questCategory,
					initialMap = true
				}
			};
			this.requestNextScene = SceneManager.SceneName.SceneMission;
			this.requestNextSceneArgs = missionOpenParam;
			return;
		}
		if (button == this.selMainStoryCtrl.GuiData.pointSelect.Btn_Sel_Difficult)
		{
			if (this.selMainStoryCtrl.GuiData.pointSelect.markLock.IsActive())
			{
				QuestOnePackData questOnePackDataForReleaseIdStoryHardMode = SceneQuest.GetQuestOnePackDataForReleaseIdStoryHardMode(this.selectData.questCategory);
				if (questOnePackDataForReleaseIdStoryHardMode != null)
				{
					CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
					{
						new CmnReleaseConditionWindowCtrl.SetupParam
						{
							text = string.Concat(new string[]
							{
								SceneQuest.GetMainStoryName(questOnePackDataForReleaseIdStoryHardMode.questChapter.category, false),
								" ",
								questOnePackDataForReleaseIdStoryHardMode.questChapter.chapterName,
								questOnePackDataForReleaseIdStoryHardMode.questGroup.titleName,
								PrjUtil.MakeMessage("クリア")
							}),
							enableClear = false
						}
					});
					return;
				}
			}
			else if (this.IsEnableHardMode(true))
			{
				if (this.IsNormalMode())
				{
					this.prevNormalModeMapId = this.selectData.mapId;
				}
				else
				{
					this.prevHardModeMapId = this.selectData.mapId;
				}
				this.selectDifficultCount++;
				if (this.IsNormalMode())
				{
					QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.hardChapterId == this.selectData.chapterId);
					if (questStaticChapter != null)
					{
						this.selectData.chapterId = questStaticChapter.chapterId;
						this.selectData.mapId = this.prevNormalModeMapId;
					}
				}
				else
				{
					QuestStaticChapter questStaticChapter2 = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == this.selectData.chapterId);
					if (questStaticChapter2 != null)
					{
						this.selectData.chapterId = questStaticChapter2.hardChapterId;
						this.selectData.mapId = this.prevHardModeMapId;
					}
				}
				if (this.IsNotNullMapBaseObj() && this.selMainStoryCtrl.GuiData.mapData.baseObj.activeSelf)
				{
					this.selMainStoryCtrl.GuiData.mapData.OutAnim();
				}
				this.reqNextSequence = this.selMainStoryCtrl.GuiData.pointSelect.baseObj;
			}
		}
	}

	// Token: 0x0600169B RID: 5787 RVA: 0x0011F6A8 File Offset: 0x0011D8A8
	private void OnClickMapButtonLR(PguiButtonCtrl button)
	{
		if (this.IsPlayingAnim)
		{
			return;
		}
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId - 1);
		List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId + 1);
		int num = this.selectData.chapterId;
		int num2 = 0;
		bool flag = false;
		if (this.selMainStoryCtrl.GuiData.pointSelect.Btn_Yaji_Left == button)
		{
			flag = true;
			if (playableMapIdList.Count > 0)
			{
				num--;
			}
		}
		else if (this.selMainStoryCtrl.GuiData.pointSelect.Btn_Yaji_Right == button)
		{
			flag = true;
			if (playableMapIdList2.Count > 0)
			{
				num++;
			}
		}
		List<int> playableMapIdList3 = DataManager.DmQuest.GetPlayableMapIdList(num);
		int num3 = 0;
		if (playableMapIdList3.Count > 0)
		{
			num3 = playableMapIdList3.Count - 1;
			num2 = playableMapIdList3[num3];
		}
		this.selectData.mapId = num2;
		this.selectData.chapterId = num;
		if (flag)
		{
			this.selMainStoryCtrl.GuiData.pointSelect.ResetCampaignInfoCategory();
			this.selMainStoryCtrl.GuiData.pointSelect.UpdateCampaignInfoCategory(this.selectData.questCategory, this.selectData.chapterId);
		}
		this.selMainStoryCtrl.UpdateMapdata(this.selectData.chapterId, this.selectData.questCategory, new UnityAction<Transform>(this.OnClickButtonPointSelect));
		if (SceneQuest.IsMainStory(this.selectData.questCategory) && this.IsNotNullMapObj())
		{
			if (num3 >= this.selMainStoryCtrl.GuiData.mapData.mapPointList.Count)
			{
				num3 = this.selMainStoryCtrl.GuiData.mapData.mapPointList.Count - 1;
			}
			this.selMainStoryCtrl.GuiData.mapData.mapCar.transform.position = this.selMainStoryCtrl.GuiData.mapData.mapPointList[num3].pointObj.transform.position;
		}
	}

	// Token: 0x0600169C RID: 5788 RVA: 0x0011F8BC File Offset: 0x0011DABC
	private void OnClickButtonPointSelect(Transform point)
	{
		if (this.TouchMoving)
		{
			return;
		}
		if (this.IsPlayingAnim)
		{
			return;
		}
		SoundManager.Play("prd_se_click", false, false);
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId);
		int pointNumber = int.Parse(point.name);
		if (playableMapIdList.Find((int n) => n == pointNumber) > 0)
		{
			this.selectData.mapId = pointNumber;
			this.reqNextSequence = this.guiData.chapterSelect.baseObj;
			if (this.IsNotNullMapObj())
			{
				this.selMainStoryCtrl.GuiData.mapData.mapCar.transform.position = point.transform.position;
			}
			if (this.questArgs != null && this.questArgs.tutorialSequence == TutorialUtil.Sequence.QUEST_GUIDE)
			{
				this.gotoNextStepByTutorial = true;
			}
			return;
		}
		List<QuestStaticQuestOne.ReleaseConditions> enableReleaseConditionsList = QuestUtil.GetEnableReleaseConditionsList(pointNumber);
		List<CmnReleaseConditionWindowCtrl.SetupParam> list = new List<CmnReleaseConditionWindowCtrl.SetupParam>();
		using (List<QuestStaticQuestOne.ReleaseConditions>.Enumerator enumerator = enableReleaseConditionsList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestStaticQuestOne.ReleaseConditions e = enumerator.Current;
				QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataList.Find((QuestDynamicQuestOne item) => item.questOneId == e.questId);
				QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(e.questId);
				if (questOnePackData != null)
				{
					bool flag = QuestUtil.IsHardMode(this.selectData);
					bool flag2 = QuestUtil.IsHardMode(new QuestUtil.SelectData
					{
						chapterId = questOnePackData.questChapter.chapterId,
						questCategory = questOnePackData.questChapter.category
					});
					string text = string.Empty;
					if (flag && !flag2)
					{
						text = "ノーマル ";
					}
					list.Add(new CmnReleaseConditionWindowCtrl.SetupParam
					{
						enableClear = (questDynamicQuestOne != null && questDynamicQuestOne.status != QuestOneStatus.NEW),
						text = string.Concat(new string[]
						{
							text,
							" ",
							questOnePackData.questGroup.titleName,
							" ",
							questOnePackData.questOne.questName,
							" クリア"
						})
					});
				}
			}
		}
		if (enableReleaseConditionsList.Count <= 0)
		{
			list.Add(new CmnReleaseConditionWindowCtrl.SetupParam
			{
				text = "挑戦可能なクエストはありません。"
			});
			CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("確認"), list);
			return;
		}
		CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open("解放条件", list);
	}

	// Token: 0x0600169D RID: 5789 RVA: 0x0011FB5C File Offset: 0x0011DD5C
	private void OnClickButtonLR(PguiButtonCtrl button)
	{
		if (this.IsPlayingAnim)
		{
			return;
		}
		if (this.IsNotNullCharaStoryObj() && this.selectData.questCategory == QuestStaticChapter.Category.CHARA)
		{
			int selectCharaIndex = this.selCharaStoryCtrl.OriginalDispCharaPackList.FindIndex((CharaPackData item) => item.id == this.selCharaStoryCtrl.SelectCharaId);
			if (this.guiData.chapterSelect.Btn_Yaji_Left == button || this.guiData.chapterSelect.Btn_Yaji_Right == button)
			{
				selectCharaIndex += ((button == this.guiData.chapterSelect.Btn_Yaji_Left) ? (-1) : 1);
				selectCharaIndex = (selectCharaIndex + this.selCharaStoryCtrl.OriginalDispCharaPackList.Count) % this.selCharaStoryCtrl.OriginalDispCharaPackList.Count;
				this.selCharaStoryCtrl.SelectCharaId = this.selCharaStoryCtrl.OriginalDispCharaPackList[selectCharaIndex].id;
			}
			QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataList.Find((QuestStaticMap item) => item.questCharaId == this.selCharaStoryCtrl.OriginalDispCharaPackList[selectCharaIndex].id);
			this.selectData.mapId = questStaticMap.mapId;
			this.reqNextSequence = this.guiData.chapterSelect.baseObj;
		}
		else
		{
			List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId);
			int num = playableMapIdList.FindIndex((int n) => n == this.selectData.mapId);
			if (this.guiData.chapterSelect.Btn_Yaji_Left == button)
			{
				num--;
				if (num < 0)
				{
					num = 0;
				}
			}
			else if (this.guiData.chapterSelect.Btn_Yaji_Right == button)
			{
				num++;
				if (num >= playableMapIdList.Count)
				{
					num--;
				}
			}
			this.selectData.mapId = playableMapIdList[num];
			this.reqNextSequence = this.guiData.chapterSelect.baseObj;
			if (SceneQuest.IsMainStory(this.selectData.questCategory) && this.IsNotNullMapObj())
			{
				this.selMainStoryCtrl.GuiData.mapData.mapCar.transform.position = this.selMainStoryCtrl.GuiData.mapData.mapPointList[num].pointObj.transform.position;
			}
		}
		this.UpdateButtonLR();
	}

	// Token: 0x0600169E RID: 5790 RVA: 0x0011FDBE File Offset: 0x0011DFBE
	private void QuestButtonCallback(int questId)
	{
		if (this.actionCoroutine != null)
		{
			return;
		}
		if (this.requestNextScene == SceneManager.SceneName.SceneBattleSelector)
		{
			return;
		}
		this.actionCoroutine = Singleton<SceneManager>.Instance.StartCoroutine(this.NextScene(questId));
	}

	// Token: 0x0600169F RID: 5791 RVA: 0x0011FDEA File Offset: 0x0011DFEA
	private bool OnQuestChapterChangeWindow(int index)
	{
		if (index == 1)
		{
			this.selectDifficultCount = this.questChapterChangeWindow.selectDifficultCount;
			this.chapterChangeEffect = this.ChapterChangeEffect(this.questChapterChangeWindow.GetCurrentChapterData());
		}
		return true;
	}

	// Token: 0x060016A0 RID: 5792 RVA: 0x0011FE19 File Offset: 0x0011E019
	private IEnumerator TutorialQuest1()
	{
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(true);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(true);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN,
			postion = new Vector2?(new Vector2(500f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_0022",
			dispInfoChara = true,
			messageList = new List<string> { "お次はここをタッチしてください" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, this.guiData.questTop.storyQuestParts.Btn_StoryQuest.transform as RectTransform, true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(this.guiData.questTop.storyQuestParts.Btn_StoryQuest.transform as RectTransform, 1f);
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060016A1 RID: 5793 RVA: 0x0011FE28 File Offset: 0x0011E028
	private IEnumerator TutorialQuest2()
	{
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(true);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(true);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(true);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN,
			postion = new Vector2?(new Vector2(200f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_0022",
			dispInfoChara = true,
			messageList = new List<string> { "探検の行き先はこの画面で決めますの\n進むことによって行き先も増えていきますの", "まずはここから始めましょう" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		this.gotoNextStepByTutorial = false;
		while (!this.IsNotNullMapObj())
		{
			yield return null;
		}
		RectTransform rectTransform = this.selMainStoryCtrl.GuiData.mapData.mapPointList[0].baseObj.transform as RectTransform;
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, rectTransform, true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(rectTransform, 1f);
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(true);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(true);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN,
			postion = new Vector2?(new Vector2(200f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_0022",
			dispInfoChara = true,
			messageList = new List<string> { "地点を選んだら場所を選んで探検開始ですの", "さぁ、探検の開始ですわ" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.OUT,
			postion = new Vector2?(new Vector2(200f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_0022",
			dispInfoChara = true
		});
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, this.guiData.chapterSelect.questButtonGroup.GetButtonRectTransform(), true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(this.guiData.chapterSelect.questButtonGroup.GetButtonRectTransform(), 1f);
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		TutorialUtil.RequestNextSequence(this.questArgs.tutorialSequence);
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(false);
		yield return null;
		yield break;
	}

	// Token: 0x060016A2 RID: 5794 RVA: 0x0011FE38 File Offset: 0x0011E038
	private void InitSelMainStoryCtrl()
	{
		GameObject gameObject = new GameObject();
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.offsetMin = new Vector2(0f, 0f);
		rectTransform.offsetMax = new Vector2(0f, 0f);
		gameObject.name = "SelMainStoryCtrl";
		gameObject.transform.SetParent(this.guiData.basePanel.transform, false);
		this.selMainStoryCtrl = gameObject.AddComponent<SelMainStoryCtrl>();
		this.selMainStoryCtrl.Init(new SelMainStoryCtrl.InitParam
		{
			reqNextSequenceCB = delegate
			{
				this.reqNextSequence = this.guiData.chapterSelect.baseObj;
			},
			reqBackSequenceCB = delegate
			{
				this.reqNextSequence = this.selMainStoryCtrl.GuiData.pointSelect.baseObj;
			},
			selectObjsCB = delegate
			{
				this.guiData.selectObjs.Add(this.selMainStoryCtrl.GuiData.pointSelect.baseObj);
			},
			prefabPath = this.guiData.basePanel.transform.Find("PointSelect"),
			getSelectDataCB = () => this.selectData,
			reqRewardsCB = delegate
			{
				if (SceneQuest.IsMainStory(this.selectData.questCategory))
				{
					this.rewards.Clear();
					List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.selectData.chapterId);
					List<QuestStaticMap> maps = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == this.selectData.chapterId).mapDataList;
					maps.Sort((QuestStaticMap a, QuestStaticMap b) => a.mapId - b.mapId);
					using (List<QuestStaticMap>.Enumerator enumerator = maps.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							QuestStaticMap map = enumerator.Current;
							QuestStaticQuestOne questStaticQuestOne = map.questGroupList.Find((QuestStaticQuestGroup item) => item.mapId == map.mapId).questOneList.Find((QuestStaticQuestOne item) => item.RewardItemList.Count > 0);
							if (questStaticQuestOne != null)
							{
								this.rewards.Add(questStaticQuestOne);
							}
						}
					}
					Action<SelMainStoryCtrl.GuiPointSelect.ChapterInfo> action = delegate(SelMainStoryCtrl.GuiPointSelect.ChapterInfo chapterInfo)
					{
						int count = this.rewards.Count;
						int count2 = chapterInfo.itemInfoIcons.Count;
						for (int i = 0; i < chapterInfo.itemInfoIcons.Count; i++)
						{
							QuestOneStatus questOneStatus = QuestOneStatus.INVALID;
							chapterInfo.itemInfoIcons[i].baseObj.SetActive(i < this.rewards.Count);
							if (i < this.rewards.Count)
							{
								if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(this.rewards[i].questId))
								{
									QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[this.rewards[i].questId];
									questOneStatus = questDynamicQuestOne.status;
								}
								PrjUtil.AddTouchEventTrigger(chapterInfo.itemInfoIcons[i].baseObj, delegate(Transform x)
								{
									int num2 = chapterInfo.itemInfoIcons.FindIndex((SelMainStoryCtrl.GuiPointSelect.ItemInfoIcon item) => item.baseObj.transform == x);
									if (questOneStatus == QuestOneStatus.COMPLETE || questOneStatus == QuestOneStatus.CLEAR)
									{
										this.itemInfoWindowAfter.Setup(this.rewards[num2]);
										return;
									}
									this.itemInfoWindow.Setup(this.rewards[num2]);
								});
								QuestOnePackData onePackData = DataManager.DmQuest.GetQuestOnePackData(this.rewards[i].questId);
								int num = maps.FindIndex((QuestStaticMap item) => item.mapId == onePackData.questMap.mapId) + 1;
								string text = string.Format("{0}話", num - playableMapIdList.Count + 1);
								if (SceneQuest.IsMainStoryPart2(onePackData.questChapter.category))
								{
									text = onePackData.questMap.mapName + "\nクリア";
								}
								chapterInfo.itemInfoIcons[i].SetItemInfo(questOneStatus == QuestOneStatus.COMPLETE || questOneStatus == QuestOneStatus.CLEAR, text);
								if (SceneQuest.IsMainStoryPart2(onePackData.questChapter.category))
								{
									chapterInfo.itemInfoIcons[i].SetEmptyStr();
								}
							}
						}
						chapterInfo.ItemInfo.SetActive(this.rewards.Count > 0);
					};
					SelMainStoryCtrl.GuiPointSelect.ChapterInfo chapterInfo2 = this.selMainStoryCtrl.GuiData.pointSelect.chapterInfoList[0];
					if (SceneQuest.IsMainStoryPart1_5(this.selectData.questCategory))
					{
						chapterInfo2 = this.selMainStoryCtrl.GuiData.pointSelect.chapterInfoList[1];
					}
					else if (SceneQuest.IsMainStoryPart2(this.selectData.questCategory))
					{
						chapterInfo2 = this.selMainStoryCtrl.GuiData.pointSelect.chapterInfoList[2];
					}
					else if (SceneQuest.IsMainStoryPart3(this.selectData.questCategory))
					{
						chapterInfo2 = this.selMainStoryCtrl.GuiData.pointSelect.chapterInfoList[3];
					}
					action(chapterInfo2);
				}
			},
			checkQuestTopCB = () => this.currentSequence == this.guiData.questTop.baseObj,
			getSelectDifficultCountCB = () => this.selectDifficultCount,
			reqIncSelectDifficultCountCB = delegate
			{
				this.selectDifficultCount++;
			}
		}, new SelMainStoryCtrl.SetupParam());
		this.selMainStoryCtrl.GuiData.pointSelect.Btn_ChapterChange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonPointSelect), PguiButtonCtrl.SoundType.DEFAULT);
		this.selMainStoryCtrl.GuiData.pointSelect.Btn_Mission.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonPointSelect), PguiButtonCtrl.SoundType.DEFAULT);
		this.selMainStoryCtrl.GuiData.pointSelect.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMapButtonLR), PguiButtonCtrl.SoundType.DEFAULT);
		this.selMainStoryCtrl.GuiData.pointSelect.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMapButtonLR), PguiButtonCtrl.SoundType.DEFAULT);
		this.selMainStoryCtrl.GuiData.pointSelect.Btn_Sel_Difficult.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonPointSelect), PguiButtonCtrl.SoundType.DEFAULT);
		this.selMainStoryCtrl.SetMapBoxObject(SceneQuest.mapBoxObject);
	}

	// Token: 0x060016A3 RID: 5795 RVA: 0x00120070 File Offset: 0x0011E270
	private void InitEventCtrl(DataManagerEvent.Category category)
	{
		switch (category)
		{
		case DataManagerEvent.Category.Scenario:
			if (this.selEventScenarioCtrl == null)
			{
				GameObject gameObject = new GameObject();
				RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
				rectTransform.anchorMin = new Vector2(0f, 0f);
				rectTransform.anchorMax = new Vector2(1f, 1f);
				rectTransform.offsetMin = new Vector2(0f, 0f);
				rectTransform.offsetMax = new Vector2(0f, 0f);
				gameObject.name = "SelEventScenario";
				gameObject.transform.SetParent(this.guiData.basePanel.transform, false);
				this.selEventScenarioCtrl = gameObject.AddComponent<SelEventScenarioCtrl>();
				this.selEventScenarioCtrl.Init(new SelEventScenarioCtrl.InitParam
				{
					reqNextSequenceCB = delegate
					{
						this.reqNextSequence = this.guiData.chapterSelect.baseObj;
					},
					reqBackSequenceCB = delegate
					{
						this.reqNextSequence = this.selEventScenarioCtrl.GuiData.eventSelect.baseObj;
					},
					reqShopSequenceCB = delegate
					{
						this.SetupNextSceneChangeShop(this.currentEnableEventData.eventId);
					},
					selectObjsCB = delegate
					{
						this.guiData.selectObjs.Add(this.selEventScenarioCtrl.GuiData.eventSelect.baseObj);
					}
				}, new SelEventScenarioCtrl.SetupParam
				{
					eventData = this.currentEnableEventData,
					reqNextSequenceCB = delegate
					{
						this.reqNextSequence = this.selEventScenarioCtrl.GuiData.eventSelect.baseObj;
					}
				});
				return;
			}
			this.selEventScenarioCtrl.Setup(new SelEventScenarioCtrl.SetupParam
			{
				eventData = this.currentEnableEventData,
				reqNextSequenceCB = delegate
				{
					this.reqNextSequence = this.selEventScenarioCtrl.GuiData.eventSelect.baseObj;
				}
			});
			return;
		case DataManagerEvent.Category.Growth:
			if (this.selEventCharaGrowCtrl == null)
			{
				GameObject gameObject2 = new GameObject();
				RectTransform rectTransform2 = gameObject2.AddComponent<RectTransform>();
				rectTransform2.anchorMin = new Vector2(0f, 0f);
				rectTransform2.anchorMax = new Vector2(1f, 1f);
				rectTransform2.offsetMin = new Vector2(0f, 0f);
				rectTransform2.offsetMax = new Vector2(0f, 0f);
				gameObject2.name = "SelEventCharaGrow";
				gameObject2.transform.SetParent(this.guiData.basePanel.transform, false);
				this.selEventCharaGrowCtrl = gameObject2.AddComponent<SelEventCharaGrowCtrl>();
				this.selEventCharaGrowCtrl.Init(new SelEventCharaGrowCtrl.InitParam
				{
					reqNextSequenceCB = delegate
					{
						this.reqNextSequence = this.guiData.chapterSelect.baseObj;
						this.selEventCharaGrowCtrl.SetEnableChangeButton(SelEventCharaGrowCtrl.IsEnableButtonChange(this.currentEnableEventData.eventId));
					},
					reqBackSequenceCB = delegate
					{
						this.reqNextSequence = this.selEventCharaGrowCtrl.GuiData.charaSelect.baseObj;
					},
					reqShopSequenceCB = delegate
					{
						this.SetupNextSceneChangeShop(this.currentEnableEventData.eventId);
					},
					chapterObject = this.guiData.chapterSelect.baseObj.transform.Find("Left").gameObject,
					onPlayAnimCB = delegate
					{
						if (this.reqNextSequence == null)
						{
							return false;
						}
						if (!this.reqNextSequence.activeSelf)
						{
							return false;
						}
						SimpleAnimation component = this.reqNextSequence.GetComponent<SimpleAnimation>();
						return component != null && component.ExIsPlaying();
					},
					onWaitEnableCB = () => !(this.reqNextSequence == null) && !this.reqNextSequence.activeSelf,
					onCheckStatusCB = () => this.status == SceneQuest.Status.POLLING_REQUEST
				}, new SelEventCharaGrowCtrl.SetupParam
				{
					eventData = this.currentEnableEventData
				});
				this.guiData.selectObjs.Add(this.selEventCharaGrowCtrl.GuiData.charaSelect.baseObj);
			}
			else
			{
				this.selEventCharaGrowCtrl.Setup(new SelEventCharaGrowCtrl.SetupParam
				{
					eventData = this.currentEnableEventData
				});
			}
			Singleton<SceneManager>.Instance.StartCoroutine(this.WaitNextSequenceEventCharaGrow());
			CanvasManager.SetBgObj(QuestUtil.EVENT_BG);
			return;
		case DataManagerEvent.Category.Large:
			if (this.selEventLargeScaleCtrl == null)
			{
				GameObject gameObject3 = new GameObject();
				RectTransform rectTransform3 = gameObject3.AddComponent<RectTransform>();
				rectTransform3.anchorMin = new Vector2(0f, 0f);
				rectTransform3.anchorMax = new Vector2(1f, 1f);
				rectTransform3.offsetMin = new Vector2(0f, 0f);
				rectTransform3.offsetMax = new Vector2(0f, 0f);
				gameObject3.name = "SelEventLargeScaleCtrl";
				gameObject3.transform.SetParent(this.guiData.basePanel.transform, false);
				this.selEventLargeScaleCtrl = gameObject3.AddComponent<SelEventLargeScaleCtrl>();
				this.selEventLargeScaleCtrl.Init(new SelEventLargeScaleCtrl.InitParam
				{
					reqNextSequenceCB = delegate
					{
						this.reqNextSequence = this.guiData.chapterSelect.baseObj;
					},
					reqBackSequenceCB = delegate
					{
						this.reqNextSequence = this.selEventLargeScaleCtrl.GuiData.pointSelect.baseObj;
					},
					reqShopSequenceCB = delegate
					{
						this.SetupNextSceneChangeShop(this.currentEnableEventData.eventId);
					}
				}, new SelEventLargeScaleCtrl.SetupParam
				{
					eventData = this.currentEnableEventData,
					checkChapterSelectSequenceCB = () => this.guiData.chapterSelect.baseObj == this.currentSequence
				});
				this.guiData.selectObjs.Add(this.selEventLargeScaleCtrl.GuiData.pointSelect.baseObj);
			}
			this.selEventLargeScaleCtrl.Setup(new SelEventLargeScaleCtrl.SetupParam
			{
				eventData = this.currentEnableEventData,
				checkChapterSelectSequenceCB = () => this.guiData.chapterSelect.baseObj == this.currentSequence
			});
			this.reqNextSequence = this.selEventLargeScaleCtrl.GuiData.pointSelect.baseObj;
			this.DestroyMap();
			this.instCarObjList = SceneQuest.CreateCarObjList(SceneQuest.mapBoxObject);
			this.requestSequenceInstantiateAssetData = Singleton<SceneManager>.Instance.StartCoroutine(this.InstantiateAssetData(this.selEventLargeScaleCtrl.LoadMapObject(SceneQuest.mapBoxObject), this.selEventLargeScaleCtrl.LoadAssetPath));
			this.selectData.questCategory = QuestStaticChapter.Category.EVENT;
			this.eventLargeScaleEffect = this.EventLargeScaleEffect();
			return;
		case DataManagerEvent.Category.Mission:
			break;
		case DataManagerEvent.Category.Tower:
			if (this.selEventTowerCtrl == null)
			{
				GameObject gameObject4 = new GameObject();
				RectTransform rectTransform4 = gameObject4.AddComponent<RectTransform>();
				rectTransform4.anchorMin = new Vector2(0f, 0f);
				rectTransform4.anchorMax = new Vector2(1f, 1f);
				rectTransform4.offsetMin = new Vector2(0f, 0f);
				rectTransform4.offsetMax = new Vector2(0f, 0f);
				gameObject4.name = "SelEventTower";
				gameObject4.transform.SetParent(this.guiData.basePanel.transform, false);
				this.selEventTowerCtrl = gameObject4.AddComponent<SelEventTowerCtrl>();
				this.selEventTowerCtrl.Init(new SelEventTowerCtrl.InitParam
				{
					reqNextSequenceCB = delegate
					{
						this.reqNextSequence = this.guiData.chapterSelect.baseObj;
					},
					reqBackSequenceCB = delegate
					{
						this.reqNextSequence = this.selEventTowerCtrl.GuiData.eventSelect.baseObj;
					},
					reqGachaSequenceCB = delegate
					{
						SceneGacha.OpenParam openParam = new SceneGacha.OpenParam
						{
							gachaId = this.currentEnableEventData.eventGachaId,
							resultNextSceneName = SceneManager.SceneName.SceneQuest,
							resultNextSceneArgs = new SceneQuest.Args
							{
								selectEventId = this.currentEnableEventData.eventId,
								category = QuestStaticChapter.Category.EVENT,
								backSequenceGameObject = this.currentSequence
							}
						};
						this.requestNextScene = SceneManager.SceneName.SceneGacha;
						this.requestNextSceneArgs = openParam;
					},
					selectObjsCB = delegate
					{
						this.guiData.selectObjs.Add(this.selEventTowerCtrl.GuiData.eventSelect.baseObj);
					}
				}, new SelEventTowerCtrl.SetupParam
				{
					eventData = this.currentEnableEventData,
					reqNextSequenceCB = delegate
					{
						this.reqNextSequence = this.selEventTowerCtrl.GuiData.eventSelect.baseObj;
					}
				});
				return;
			}
			this.selEventTowerCtrl.Setup(new SelEventTowerCtrl.SetupParam
			{
				eventData = this.currentEnableEventData,
				reqNextSequenceCB = delegate
				{
					this.reqNextSequence = this.selEventTowerCtrl.GuiData.eventSelect.baseObj;
				}
			});
			return;
		case DataManagerEvent.Category.Coop:
			if (this.currentEnableEventData.raidFlg && DataManager.DmEvent.GetNowTermData(this.currentEnableEventData.eventId) == null)
			{
				DataManagerEvent.CoopRaidTermData nextTermData = DataManager.DmEvent.GetNextTermData(this.currentEnableEventData);
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
				list.Clear();
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "閉じる"));
				int num = 0;
				if (nextTermData == null)
				{
					return;
				}
				if (nextTermData.startTime <= TimeManager.Now.TimeOfDay)
				{
					num++;
				}
				CanvasManager.HdlOpenWindowBasic.Setup("", string.Format("次回開催は{0}月{1}日 {2}:{3}からです", new object[]
				{
					TimeManager.Now.Month,
					TimeManager.Now.Day + num,
					nextTermData.startTime.Hours.ToString("00"),
					nextTermData.startTime.Minutes.ToString("00")
				}), list, false, (int idx) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				return;
			}
			else
			{
				if (this.selEventCoopCtrl == null)
				{
					GameObject gameObject5 = new GameObject();
					RectTransform rectTransform5 = gameObject5.AddComponent<RectTransform>();
					rectTransform5.anchorMin = new Vector2(0f, 0f);
					rectTransform5.anchorMax = new Vector2(1f, 1f);
					rectTransform5.offsetMin = new Vector2(0f, 0f);
					rectTransform5.offsetMax = new Vector2(0f, 0f);
					gameObject5.name = "SelEventCoop";
					gameObject5.transform.SetParent(this.guiData.basePanel.transform, false);
					this.selEventCoopCtrl = gameObject5.AddComponent<SelEventCoopCtrl>();
					this.selEventCoopCtrl.Init(new SelEventCoopCtrl.InitParam
					{
						reqShopSequenceCB = delegate
						{
							this.SetupNextSceneChangeShop(this.currentEnableEventData.eventId);
						},
						selectObjsCB = delegate
						{
							this.guiData.selectObjs.Add(this.selEventCoopCtrl.GuiData.mapSelect.baseObj);
						},
						chapterLeftObject = this.guiData.chapterSelect.baseObj.transform.Find("Left").gameObject,
						chapterRightObject = this.guiData.chapterSelect.baseObj.transform.Find("Right").gameObject
					}, new SelEventCoopCtrl.SetupParam
					{
						eventData = this.currentEnableEventData,
						reqNextSequenceCB = delegate
						{
							this.reqNextSequence = this.selEventCoopCtrl.GuiData.mapSelect.baseObj;
						},
						pointTouchCB = delegate(Transform point)
						{
							if (this.IsPlayingAnim)
							{
								return;
							}
							SoundManager.Play("prd_se_click", false, false);
							int num2 = int.Parse(point.name);
							if (this.selEventCoopCtrl.IsBonus(num2))
							{
								if (!SelEventCoopCtrl.OpenedBonus())
								{
									CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), "全拠点のもくひょう達成で開放", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int btnIndex) => true, null, false);
									CanvasManager.HdlOpenWindowBasic.Open();
									return;
								}
								Singleton<SceneManager>.Instance.StartCoroutine(this.selEventCoopCtrl.RequestBounusClearReset(num2));
								Singleton<SceneManager>.Instance.StartCoroutine(this.selEventCoopCtrl.RequestBounusClearResetFromGroup(num2));
							}
							this.selectData.mapId = num2;
							this.reqNextSequence = this.guiData.chapterSelect.baseObj;
							this.selEventCoopCtrl.SelectData = this.selectData;
						}
					});
					return;
				}
				this.selEventCoopCtrl.Setup(new SelEventCoopCtrl.SetupParam
				{
					eventData = this.currentEnableEventData,
					reqNextSequenceCB = delegate
					{
						this.reqNextSequence = this.selEventCoopCtrl.GuiData.mapSelect.baseObj;
					}
				});
				return;
			}
			break;
		case DataManagerEvent.Category.WildRelease:
			if (this.selEventWildReleaseCtrl == null)
			{
				GameObject gameObject6 = new GameObject();
				RectTransform rectTransform6 = gameObject6.AddComponent<RectTransform>();
				rectTransform6.anchorMin = new Vector2(0f, 0f);
				rectTransform6.anchorMax = new Vector2(1f, 1f);
				rectTransform6.offsetMin = new Vector2(0f, 0f);
				rectTransform6.offsetMax = new Vector2(0f, 0f);
				gameObject6.name = "SelEventWildRelease";
				gameObject6.transform.SetParent(this.guiData.basePanel.transform, false);
				this.selEventWildReleaseCtrl = gameObject6.AddComponent<SelEventWildReleaseCtrl>();
				this.selEventWildReleaseCtrl.Init(new SelEventWildReleaseCtrl.InitParam
				{
					reqNextSequenceCB = delegate
					{
						this.reqNextSequence = this.guiData.chapterSelect.baseObj;
					},
					reqShopSequenceCB = delegate
					{
						this.SetupNextSceneChangeShop(this.currentEnableEventData.eventId);
					},
					chapterObject = this.guiData.chapterSelect.baseObj.transform.Find("Left").gameObject
				}, new SelEventWildReleaseCtrl.SetupParam
				{
					eventData = this.currentEnableEventData
				});
				return;
			}
			this.selEventWildReleaseCtrl.Setup(new SelEventWildReleaseCtrl.SetupParam
			{
				eventData = this.currentEnableEventData
			});
			this.selEventWildReleaseCtrl.SetActiveQuestSelect(true);
			break;
		default:
			return;
		}
	}

	// Token: 0x060016A4 RID: 5796 RVA: 0x00120AE3 File Offset: 0x0011ECE3
	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		if (this.currentMode == SceneQuest.Mode.CHARA_EDIT)
		{
			this.guiData.questTop.Btn_AssistantEdit.gameObject.SetActive(true);
			return this.guiData.questTop.selAssistantCtrl.OnClickMoveSequenceButton(sceneName, sceneArgs);
		}
		return false;
	}

	// Token: 0x04001204 RID: 4612
	private static readonly SceneQuest.RenderTextureTransform DEFAULT_RENDER_TEXTURE_TRANSFORM = new SceneQuest.RenderTextureTransform
	{
		fov = 18f,
		position = new Vector2(-400f, -230f),
		rotation = new Vector3(0f, 0f, 0f)
	};

	// Token: 0x04001205 RID: 4613
	private static readonly SceneQuest.RenderTextureTransform ARAI_STORY_RENDER_TEXTURE_TRANSFORM01 = new SceneQuest.RenderTextureTransform
	{
		fov = 20f,
		position = new Vector2(-484f, -120f),
		rotation = new Vector3(0f, 0f, 0f)
	};

	// Token: 0x04001206 RID: 4614
	private static readonly SceneQuest.RenderTextureTransform ARAI_STORY_RENDER_TEXTURE_TRANSFORM02 = new SceneQuest.RenderTextureTransform
	{
		fov = 26f,
		position = new Vector2(-330f, -100f),
		rotation = new Vector3(0f, 0f, 0f)
	};

	// Token: 0x04001207 RID: 4615
	private static readonly string MainStoryMapPath = "Gui/QuestMap/GUI_Map_StoryQuest";

	// Token: 0x04001208 RID: 4616
	private static readonly string CellvalMapPath = "Gui/QuestMap/GUI_Map_CellvalQuest";

	// Token: 0x04001209 RID: 4617
	private static readonly string MainStory2MapPath = "Gui/QuestMap/GUI_Map_StoryQuest2";

	// Token: 0x0400120A RID: 4618
	private static readonly string MainStory3MapPath = "Gui/QuestMap/GUI_Map_StoryQuest3";

	// Token: 0x0400120B RID: 4619
	private SceneQuest.UpdateCallback updateCallback;

	// Token: 0x0400120C RID: 4620
	private SceneQuest.TouchCallback touchCallback;

	// Token: 0x0400120D RID: 4621
	private SceneQuest.Mode requestMode;

	// Token: 0x0400120E RID: 4622
	private SceneQuest.Mode currentMode;

	// Token: 0x0400120F RID: 4623
	private SceneQuest.GUI guiData;

	// Token: 0x04001210 RID: 4624
	private SceneQuest.GuiChapterChange guiChapterChange;

	// Token: 0x04001211 RID: 4625
	private SceneQuest.QuestChapterChangeWindowCtrl questChapterChangeWindow;

	// Token: 0x04001212 RID: 4626
	private SceneQuest.QuestScheduleWindowCtrl questScheduleWindow;

	// Token: 0x04001213 RID: 4627
	private SceneQuest.ItemInfoWindowCtrl itemInfoWindow;

	// Token: 0x04001214 RID: 4628
	private SceneQuest.ItemInfoWindowAfterCtrl itemInfoWindowAfter;

	// Token: 0x04001215 RID: 4629
	private SelEventCharaGrowCtrl selEventCharaGrowCtrl;

	// Token: 0x04001216 RID: 4630
	private SelEventLargeScaleCtrl selEventLargeScaleCtrl;

	// Token: 0x04001217 RID: 4631
	private SelEventScenarioCtrl selEventScenarioCtrl;

	// Token: 0x04001218 RID: 4632
	private SelEventTowerCtrl selEventTowerCtrl;

	// Token: 0x04001219 RID: 4633
	private SelSideStoryCtrl selSideStoryCtrl;

	// Token: 0x0400121A RID: 4634
	private SelEventCoopCtrl selEventCoopCtrl;

	// Token: 0x0400121B RID: 4635
	private SelEventWildReleaseCtrl selEventWildReleaseCtrl;

	// Token: 0x0400121C RID: 4636
	private SelEtcetraStoryCtrl selEtcetraStoryCtrl;

	// Token: 0x0400121D RID: 4637
	private SelMainStoryCtrl selMainStoryCtrl;

	// Token: 0x0400121E RID: 4638
	private SelCharaStoryCtrl selCharaStoryCtrl;

	// Token: 0x0400121F RID: 4639
	private GameObject reqNextSequence;

	// Token: 0x04001220 RID: 4640
	private GameObject currentSequence;

	// Token: 0x04001221 RID: 4641
	private SceneQuest.Args questArgs;

	// Token: 0x04001222 RID: 4642
	private int nextChapterId;

	// Token: 0x04001223 RID: 4643
	private float BgDefaultPosX;

	// Token: 0x04001224 RID: 4644
	public static float BgDefaultPosY = 0f;

	// Token: 0x04001225 RID: 4645
	private GameObject instMapObj;

	// Token: 0x04001226 RID: 4646
	private List<GameObject> instCarObjList;

	// Token: 0x04001227 RID: 4647
	private List<DataManagerEvent.EventData> enableEventDataList;

	// Token: 0x04001228 RID: 4648
	private DataManagerEvent.EventData currentEnableEventData;

	// Token: 0x04001229 RID: 4649
	private int buttonEventAllCount;

	// Token: 0x0400122A RID: 4650
	private SceneManager.SceneName requestNextScene;

	// Token: 0x0400122B RID: 4651
	private object requestNextSceneArgs;

	// Token: 0x0400122C RID: 4652
	private Coroutine actionCoroutine;

	// Token: 0x0400122E RID: 4654
	private SceneQuest.Status status;

	// Token: 0x04001230 RID: 4656
	private QuestUtil.SelectData selectData;

	// Token: 0x04001231 RID: 4657
	private int selectDifficultCount;

	// Token: 0x04001232 RID: 4658
	private int prevNormalModeMapId;

	// Token: 0x04001233 RID: 4659
	private int prevHardModeMapId;

	// Token: 0x04001234 RID: 4660
	private IEnumerator mainChapterChangeEffect;

	// Token: 0x04001235 RID: 4661
	private IEnumerator modeEndSequenceCtrl;

	// Token: 0x04001236 RID: 4662
	private IEnumerator getItemWindowCtrl;

	// Token: 0x04001237 RID: 4663
	private QuestFirstClearEvent questFirstClearEvent;

	// Token: 0x04001238 RID: 4664
	private IEnumerator sideStoryChapterChangeEffect;

	// Token: 0x04001239 RID: 4665
	private IEnumerator charaStoryEffect;

	// Token: 0x0400123A RID: 4666
	private IEnumerator chapterChangeEffect;

	// Token: 0x0400123B RID: 4667
	private IEnumerator eventLargeScaleEffect;

	// Token: 0x0400123C RID: 4668
	private Coroutine coroutineOnEnableSceneTask;

	// Token: 0x0400123D RID: 4669
	private bool touchScreenAuth;

	// Token: 0x0400123E RID: 4670
	private Dictionary<int, DateTime> eventStartTimeMap;

	// Token: 0x0400123F RID: 4671
	private Coroutine requestQuestCmd;

	// Token: 0x04001240 RID: 4672
	public static GameObject mapBoxObject;

	// Token: 0x04001241 RID: 4673
	private Coroutine requestSequenceInstantiateAssetData;

	// Token: 0x04001242 RID: 4674
	private List<QuestStaticQuestOne> rewards = new List<QuestStaticQuestOne>();

	// Token: 0x04001245 RID: 4677
	private Rect safeArea;

	// Token: 0x04001246 RID: 4678
	private bool isBackToMap;

	// Token: 0x04001248 RID: 4680
	private bool gotoNextStepByTutorial;

	// Token: 0x02000C83 RID: 3203
	public enum MainStoryDifficulty
	{
		// Token: 0x04004B89 RID: 19337
		Normal,
		// Token: 0x04004B8A RID: 19338
		Hard
	}

	// Token: 0x02000C84 RID: 3204
	private enum Status
	{
		// Token: 0x04004B8C RID: 19340
		POLLING_REQUEST,
		// Token: 0x04004B8D RID: 19341
		CHANGING_SCENE
	}

	// Token: 0x02000C85 RID: 3205
	public enum MainStoryType
	{
		// Token: 0x04004B8F RID: 19343
		Part1,
		// Token: 0x04004B90 RID: 19344
		Part1_5,
		// Token: 0x04004B91 RID: 19345
		Part2,
		// Token: 0x04004B92 RID: 19346
		Part3
	}

	// Token: 0x02000C86 RID: 3206
	private class RenderTextureTransform
	{
		// Token: 0x04004B93 RID: 19347
		public Vector2 position;

		// Token: 0x04004B94 RID: 19348
		public Vector3 rotation;

		// Token: 0x04004B95 RID: 19349
		public float fov;
	}

	// Token: 0x02000C87 RID: 3207
	public class Args
	{
		// Token: 0x04004B96 RID: 19350
		public TutorialUtil.Sequence tutorialSequence;

		// Token: 0x04004B97 RID: 19351
		public int selectQuestOneId;

		// Token: 0x04004B98 RID: 19352
		public bool initialMap;

		// Token: 0x04004B99 RID: 19353
		public QuestStaticChapter.Category category;

		// Token: 0x04004B9A RID: 19354
		public bool jumpQuest;

		// Token: 0x04004B9B RID: 19355
		public SceneManager.SceneName menuBackSceneName;

		// Token: 0x04004B9C RID: 19356
		public object menuBackSceneArgs;

		// Token: 0x04004B9D RID: 19357
		public GameObject backSequenceGameObject;

		// Token: 0x04004B9E RID: 19358
		public SceneManager.SceneName recordCameSceneName;

		// Token: 0x04004B9F RID: 19359
		public SceneQuest.Args.JustBeforeBattle justBeforeBattle;

		// Token: 0x04004BA0 RID: 19360
		public int selectEventId;

		// Token: 0x04004BA1 RID: 19361
		public int selectCharaId;

		// Token: 0x04004BA2 RID: 19362
		public bool isFromBanner;

		// Token: 0x0200119C RID: 4508
		public class JustBeforeBattle
		{
			// Token: 0x0400605B RID: 24667
			public int playQuestId;

			// Token: 0x0400605C RID: 24668
			public DataManagerQuest.BattleEndStatus endStatus;

			// Token: 0x0400605D RID: 24669
			public bool isFirstClear;

			// Token: 0x0400605E RID: 24670
			public bool isMapAllClearEvent;

			// Token: 0x0400605F RID: 24671
			public bool isReleaseMaxArts;

			// Token: 0x04006060 RID: 24672
			public List<ItemData> specialInfoItemList = new List<ItemData>();

			// Token: 0x04006061 RID: 24673
			public bool specialInfoItemMovePresentBox;
		}
	}

	// Token: 0x02000C88 RID: 3208
	// (Invoke) Token: 0x0600461A RID: 17946
	public delegate void UpdateCallback();

	// Token: 0x02000C89 RID: 3209
	// (Invoke) Token: 0x0600461E RID: 17950
	public delegate void TouchCallback();

	// Token: 0x02000C8A RID: 3210
	public class QuestChapterChangeWindowCtrl
	{
		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06004621 RID: 17953 RVA: 0x00210DB0 File Offset: 0x0020EFB0
		// (set) Token: 0x06004622 RID: 17954 RVA: 0x00210DB8 File Offset: 0x0020EFB8
		public QuestStaticChapter.Category CategoryByOpenWindow { get; set; }

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06004623 RID: 17955 RVA: 0x00210DC1 File Offset: 0x0020EFC1
		// (set) Token: 0x06004624 RID: 17956 RVA: 0x00210DC9 File Offset: 0x0020EFC9
		private List<QuestStaticChapter.Category> SwitchCategoryCtrlList { get; set; }

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06004625 RID: 17957 RVA: 0x00210DD2 File Offset: 0x0020EFD2
		// (set) Token: 0x06004626 RID: 17958 RVA: 0x00210DDA File Offset: 0x0020EFDA
		private int SwitchCategoryCtrlIndex { get; set; }

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06004628 RID: 17960 RVA: 0x00210DEC File Offset: 0x0020EFEC
		// (set) Token: 0x06004627 RID: 17959 RVA: 0x00210DE3 File Offset: 0x0020EFE3
		public int selectDifficultCount { get; private set; }

		// Token: 0x06004629 RID: 17961 RVA: 0x00210DF4 File Offset: 0x0020EFF4
		public void Init(GameObject mainObj, PguiOpenWindowCtrl.Callback cb)
		{
			this.currentChapterMap = new Dictionary<SceneQuest.MainStoryDifficulty, QuestStaticChapter>();
			this.pushedCB = cb;
			this.selectDifficultCount = 0;
			this.SwitchCategoryCtrlIndex = 0;
			this.guiData = new SceneQuest.QuestChapterChangeWindowCtrl.GUI(mainObj.transform);
			this.guiData.ScrollView.InitForce();
			if (this.guiData.ScrollView.onStartItem == null)
			{
				ReuseScroll scrollView = this.guiData.ScrollView;
				scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartWindowItem));
			}
			if (this.guiData.ScrollView.onUpdateItem == null)
			{
				ReuseScroll scrollView2 = this.guiData.ScrollView;
				scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateWindowItem));
			}
			this.guiData.ScrollView.Setup(10, 0);
			this.guiData.Btn_Sel_Difficult.AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				if (button == this.guiData.Btn_Sel_Difficult)
				{
					if (this.guiData.markLock.IsActive())
					{
						QuestOnePackData questOnePackDataForReleaseIdStoryHardMode = SceneQuest.GetQuestOnePackDataForReleaseIdStoryHardMode(this.SwitchCategoryCtrlList[this.SwitchCategoryCtrlIndex]);
						if (questOnePackDataForReleaseIdStoryHardMode != null)
						{
							CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
							{
								new CmnReleaseConditionWindowCtrl.SetupParam
								{
									text = string.Concat(new string[]
									{
										SceneQuest.GetMainStoryName(questOnePackDataForReleaseIdStoryHardMode.questChapter.category, false),
										" ",
										questOnePackDataForReleaseIdStoryHardMode.questChapter.chapterName,
										questOnePackDataForReleaseIdStoryHardMode.questGroup.titleName,
										PrjUtil.MakeMessage("クリア")
									}),
									enableClear = false
								}
							});
							return;
						}
					}
					else
					{
						int num = this.selectDifficultCount + 1;
						this.selectDifficultCount = num;
						this.SwitchDifficultButton();
						this.Setup(this.SwitchCategoryCtrlList[this.SwitchCategoryCtrlIndex], this.selectDifficultCount, this.GetChapterId());
					}
				}
			}, PguiButtonCtrl.SoundType.DEFAULT);
			this.guiData.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonLR), PguiButtonCtrl.SoundType.DEFAULT);
			this.guiData.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonLR), PguiButtonCtrl.SoundType.DEFAULT);
			this.SwitchCategoryCtrlList = new List<QuestStaticChapter.Category>
			{
				QuestStaticChapter.Category.STORY,
				QuestStaticChapter.Category.CELLVAL,
				QuestStaticChapter.Category.STORY2,
				QuestStaticChapter.Category.STORY3
			};
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x00210F54 File Offset: 0x0020F154
		public void Setup(QuestStaticChapter.Category _category, int count, int chapterId)
		{
			this.category = _category;
			this.selectDifficultCount = count;
			this.UpdateCurrentChapterData(DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId]);
			this.SwitchDifficultButton();
			int num = 1;
			QuestStaticChapter.Category category = this.category;
			if (category != QuestStaticChapter.Category.STORY)
			{
				switch (category)
				{
				case QuestStaticChapter.Category.CELLVAL:
					num = 2;
					break;
				case QuestStaticChapter.Category.STORY2:
					num = 3;
					break;
				case QuestStaticChapter.Category.STORY3:
					num = 4;
					break;
				}
			}
			else
			{
				num = 1;
			}
			this.SwitchCategoryCtrlIndex = this.SwitchCategoryCtrlList.IndexOf(this.category);
			if (this.SwitchCategoryCtrlIndex < 0)
			{
				this.SwitchCategoryCtrlIndex = 0;
			}
			this.guiData.Mark_StorySelect.GetComponent<PguiReplaceSpriteCtrl>().Replace(num);
			this.guiData.Fint_StorySelect.GetComponent<PguiReplaceSpriteCtrl>().Replace(num);
			int num2 = this.SwitchCategoryCtrlIndex - 1;
			if (num2 < 0)
			{
				num2 = 0;
			}
			bool flag;
			if (this.IsNormalMode())
			{
				List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.SwitchCategoryCtrlList[num2]);
				flag = playableMapIdList != null && playableMapIdList.Count > 0;
			}
			else
			{
				QuestOnePackData questOnePackDataForReleaseIdStoryHardMode = SceneQuest.GetQuestOnePackDataForReleaseIdStoryHardMode(this.SwitchCategoryCtrlList[num2]);
				List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList((questOnePackDataForReleaseIdStoryHardMode != null) ? questOnePackDataForReleaseIdStoryHardMode.questChapter.hardChapterId : 0);
				flag = playableMapIdList2 != null && playableMapIdList2.Count > 0;
			}
			this.guiData.Btn_Yaji_Left.gameObject.SetActive(QuestUtil.IsUnLockInformationCellvalQuest(TimeManager.Now) && this.SwitchCategoryCtrlIndex > 0 && flag);
			int num3 = this.SwitchCategoryCtrlIndex + 1;
			if (num3 >= this.SwitchCategoryCtrlList.Count)
			{
				num3 = this.SwitchCategoryCtrlList.Count - 1;
			}
			bool flag2;
			if (this.IsNormalMode())
			{
				List<int> playableMapIdList3 = DataManager.DmQuest.GetPlayableMapIdList(this.SwitchCategoryCtrlList[num3]);
				flag2 = playableMapIdList3 != null && playableMapIdList3.Count > 0;
			}
			else
			{
				QuestOnePackData questOnePackDataForReleaseIdStoryHardMode2 = SceneQuest.GetQuestOnePackDataForReleaseIdStoryHardMode(this.SwitchCategoryCtrlList[num3]);
				List<int> playableMapIdList4 = DataManager.DmQuest.GetPlayableMapIdList((questOnePackDataForReleaseIdStoryHardMode2 != null) ? questOnePackDataForReleaseIdStoryHardMode2.questChapter.hardChapterId : 0);
				flag2 = playableMapIdList4 != null && playableMapIdList4.Count > 0;
			}
			this.guiData.Btn_Yaji_Right.gameObject.SetActive(QuestUtil.IsUnLockInformationCellvalQuest(TimeManager.Now) && this.SwitchCategoryCtrlIndex < this.SwitchCategoryCtrlList.Count - 1 && flag2);
			this.guiData.owCtrl.Setup(PrjUtil.MakeMessage("章変更"), null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, this.pushedCB, null, false);
			this.guiData.owCtrl.Open();
			Singleton<SceneManager>.Instance.StartCoroutine(this.Anim());
			QuestOneStatus questOneStatus = QuestOneStatus.INVALID;
			QuestOnePackData questOnePack = SceneQuest.GetQuestOnePackDataForReleaseIdStoryHardMode(this.category);
			string text;
			if (questOnePack != null)
			{
				string mainStoryName = SceneQuest.GetMainStoryName(questOnePack.questChapter.category, true);
				text = mainStoryName + ((mainStoryName != "") ? "\n" : "") + questOnePack.questChapter.chapterName + questOnePack.questGroup.titleName + PrjUtil.MakeMessage("クリア");
				if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questOnePack.questOne.questId))
				{
					questOneStatus = DataManager.DmQuest.QuestDynamicData.oneDataMap[questOnePack.questOne.questId].status;
				}
			}
			else
			{
				text = "クエスト情報がありません";
			}
			MarkLockCtrl markLock = this.guiData.markLock;
			MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();
			setupParam.updateConditionCallback = () => QuestUtil.IsHardMode(new QuestUtil.SelectData
			{
				questCategory = ((questOnePack != null) ? questOnePack.questChapter.category : QuestStaticChapter.Category.STORY),
				chapterId = ((questOnePack != null) ? questOnePack.questChapter.chapterId : 0)
			});
			setupParam.releaseFlag = questOneStatus != QuestOneStatus.INVALID && questOneStatus != QuestOneStatus.NEW;
			setupParam.tagetObject = this.guiData.Btn_Sel_Difficult.gameObject;
			setupParam.text = text;
			setupParam.updateUserFlagDataCallback = delegate
			{
			};
			markLock.Setup(setupParam, true);
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x00211378 File Offset: 0x0020F578
		private void OnClickButtonLR(PguiButtonCtrl button)
		{
			if (button == this.guiData.Btn_Yaji_Left || button == this.guiData.Btn_Yaji_Right)
			{
				int num = this.SwitchCategoryCtrlIndex;
				num += ((button == this.guiData.Btn_Yaji_Left) ? (-1) : 1);
				this.SwitchCategoryCtrlIndex = (num + this.SwitchCategoryCtrlList.Count) % this.SwitchCategoryCtrlList.Count;
				this.Setup(this.SwitchCategoryCtrlList[this.SwitchCategoryCtrlIndex], this.selectDifficultCount, this.GetChapterId());
			}
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x00211410 File Offset: 0x0020F610
		private void ClearSimpleAnimationPlayRequestFlag()
		{
			foreach (SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange chapterChange in this.guiData.chapterChangeList)
			{
				chapterChange.SimpleAnimationPlayRequestFlag = false;
			}
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x00211468 File Offset: 0x0020F668
		private IEnumerator Anim()
		{
			if (this.guiData.Mark_Hard.gameObject.activeSelf)
			{
				this.guiData.Mark_Hard.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
				{
					this.guiData.Mark_Hard.gameObject.SetActive(!this.IsNormalMode());
				});
			}
			foreach (SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange chapterChange in this.guiData.chapterChangeList)
			{
				if (chapterChange.baseObj.activeSelf)
				{
					chapterChange.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
				}
			}
			yield return null;
			foreach (SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange chapterChange2 in this.guiData.chapterChangeList)
			{
				if (chapterChange2.baseObj.activeSelf && chapterChange2.anim.ExIsPlaying())
				{
					yield return null;
				}
			}
			List<SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange>.Enumerator enumerator2 = default(List<SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange>.Enumerator);
			List<KeyValuePair<int, QuestStaticChapter>> chapterDataByCategory = SceneQuest.GetChapterDataByCategory(this.category, this.IsNormalMode() ? 0 : 1);
			chapterDataByCategory.Sort((KeyValuePair<int, QuestStaticChapter> a, KeyValuePair<int, QuestStaticChapter> b) => b.Key - a.Key);
			int findIndex = chapterDataByCategory.FindIndex((KeyValuePair<int, QuestStaticChapter> item) => item.Value == this.GetCurrentChapterData());
			this.ClearSimpleAnimationPlayRequestFlag();
			this.guiData.ScrollView.Resize(chapterDataByCategory.Count, 0);
			yield return null;
			foreach (SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange chapterChange3 in this.guiData.chapterChangeList)
			{
				if (chapterChange3.baseObj.activeSelf && chapterChange3.anim.ExIsPlaying())
				{
					yield return null;
				}
			}
			enumerator2 = default(List<SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange>.Enumerator);
			this.guiData.Mark_Hard.gameObject.SetActive(!this.IsNormalMode());
			this.guiData.Mark_Hard.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			while (!this.guiData.owCtrl.FinishedOpen())
			{
				yield return null;
			}
			(this.guiData.ScrollView.transform.Find("Viewport/Content") as RectTransform).anchoredPosition = new Vector2(0f, this.guiData.ScrollView.Size * (float)findIndex / 2f);
			this.guiData.ScrollView.Refresh();
			yield break;
			yield break;
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x00211477 File Offset: 0x0020F677
		private bool IsNormalMode()
		{
			return this.selectDifficultCount % Enum.GetValues(typeof(SceneQuest.MainStoryDifficulty)).Length == 0;
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x00211497 File Offset: 0x0020F697
		private SceneQuest.MainStoryDifficulty GetMainStoryDifficulty()
		{
			return (SceneQuest.MainStoryDifficulty)(this.selectDifficultCount % Enum.GetValues(typeof(SceneQuest.MainStoryDifficulty)).Length);
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x002114B4 File Offset: 0x0020F6B4
		private void SwitchDifficultButton()
		{
			GameObject gameObject = this.guiData.Btn_Sel_Difficult.transform.Find("BaseImage/Mode_Normal").gameObject;
			if (gameObject)
			{
				gameObject.SetActive(this.IsNormalMode());
			}
			GameObject gameObject2 = this.guiData.Btn_Sel_Difficult.transform.Find("BaseImage/Mode_Hard").gameObject;
			if (gameObject2)
			{
				gameObject2.SetActive(!this.IsNormalMode());
			}
			this.SetInactive();
			List<KeyValuePair<int, QuestStaticChapter>> chapterDataByCategory = SceneQuest.GetChapterDataByCategory(this.category, this.IsNormalMode() ? 0 : 1);
			chapterDataByCategory.Sort((KeyValuePair<int, QuestStaticChapter> a, KeyValuePair<int, QuestStaticChapter> b) => b.Key - a.Key);
			int num = chapterDataByCategory.FindIndex((KeyValuePair<int, QuestStaticChapter> item) => item.Value == this.GetCurrentChapterData());
			if (num < 0)
			{
				num = 0;
			}
			if (num >= this.guiData.chapterChangeList.Count)
			{
				num = this.guiData.chapterChangeList.Count - 1;
			}
			this.guiData.chapterChangeList[num].Current.SetActive(true);
			int num2 = chapterDataByCategory.Count - 1 - num;
			List<PguiAECtrl> aecheckMark = this.GetAECheckMark();
			if (0 <= num2 && num2 < aecheckMark.Count)
			{
				aecheckMark[num2].gameObject.SetActive(true);
				aecheckMark[num2].PlayAnimation(PguiAECtrl.AmimeType.START, null);
			}
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x0021160C File Offset: 0x0020F80C
		private List<PguiAECtrl> GetAECheckMark()
		{
			List<PguiAECtrl> list = this.guiData.AEImage_CheckMark;
			if (SceneQuest.IsMainStoryPart1_5(this.category))
			{
				list = this.guiData.AEImage_CheckMarkCellval;
			}
			else if (SceneQuest.IsMainStoryPart2(this.category))
			{
				list = this.guiData.AEImage_CheckMark2;
			}
			else if (SceneQuest.IsMainStoryPart3(this.category))
			{
				list = this.guiData.AEImage_CheckMark3;
			}
			return list;
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x00211678 File Offset: 0x0020F878
		private void SetInactive()
		{
			foreach (SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange chapterChange in this.guiData.chapterChangeList)
			{
				chapterChange.Current.SetActive(false);
			}
			foreach (PguiAECtrl pguiAECtrl in this.guiData.AEImage_CheckMark)
			{
				pguiAECtrl.gameObject.SetActive(false);
			}
			foreach (PguiAECtrl pguiAECtrl2 in this.guiData.AEImage_CheckMarkCellval)
			{
				pguiAECtrl2.gameObject.SetActive(false);
			}
			foreach (PguiAECtrl pguiAECtrl3 in this.guiData.AEImage_CheckMark2)
			{
				pguiAECtrl3.gameObject.SetActive(false);
			}
			foreach (PguiAECtrl pguiAECtrl4 in this.guiData.AEImage_CheckMark3)
			{
				pguiAECtrl4.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x002117FC File Offset: 0x0020F9FC
		private void OnStartWindowItem(int index, GameObject go)
		{
			this.guiData.chapterChangeList.Add(this.CreateChapterChange(go));
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x00211818 File Offset: 0x0020FA18
		private void OnUpdateWindowItem(int index, GameObject go)
		{
			List<KeyValuePair<int, QuestStaticChapter>> chapterDataByCategory = SceneQuest.GetChapterDataByCategory(this.category, this.IsNormalMode() ? 0 : 1);
			chapterDataByCategory.Sort((KeyValuePair<int, QuestStaticChapter> a, KeyValuePair<int, QuestStaticChapter> b) => b.Key - a.Key);
			for (int i = 0; i < SceneQuest.QuestChapterChangeWindowCtrl.GUI.SCROLL_ITEM_NUN_H; i++)
			{
				int num = index * SceneQuest.QuestChapterChangeWindowCtrl.GUI.SCROLL_ITEM_NUN_H + i;
				if (num < 9 && num < chapterDataByCategory.Count)
				{
					SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange chapterChange = this.guiData.chapterChangeList[num];
					chapterChange.baseObj.SetActive(true);
					QuestStaticChapter value = chapterDataByCategory[num].Value;
					if (value != null)
					{
						chapterChange.Num_Chapter.text = value.chapterName;
						chapterChange.Txt_Chapter.text = value.chapterTitle;
						chapterChange.Base_Hard.SetActive(!this.IsNormalMode());
						List<QuestStaticMap> mapDataList = DataManager.DmQuest.QuestStaticData.chapterDataMap[value.chapterId].mapDataList;
						int num2 = 0;
						using (List<QuestStaticMap>.Enumerator enumerator = mapDataList.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								QuestStaticMap map = enumerator.Current;
								QuestStaticQuestGroup questStaticQuestGroup = map.questGroupList.Find((QuestStaticQuestGroup item) => item.mapId == map.mapId);
								int num3 = 0;
								foreach (QuestStaticQuestOne questStaticQuestOne in questStaticQuestGroup.questOneList)
								{
									QuestDynamicQuestOne questDynamicQuestOne = null;
									if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questStaticQuestOne.questId))
									{
										questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[questStaticQuestOne.questId];
									}
									if (((questDynamicQuestOne != null) ? questDynamicQuestOne.status : QuestOneStatus.INVALID) == QuestOneStatus.COMPLETE)
									{
										num3++;
									}
								}
								if (num3 >= questStaticQuestGroup.questOneList.Count)
								{
									num2++;
								}
							}
						}
						chapterChange.Num_Complete.text = PrjUtil.MakeMessage(string.Format("{0}/{1}", num2, mapDataList.Count));
						chapterChange.Mark_Complete_Fnt.gameObject.SetActive(num2 == mapDataList.Count);
						CanvasGroup component = chapterChange.anim.GetComponent<CanvasGroup>();
						if (component != null && component.alpha < 1f && !chapterChange.SimpleAnimationPlayRequestFlag)
						{
							chapterChange.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
							chapterChange.SimpleAnimationPlayRequestFlag = true;
						}
					}
				}
				else
				{
					go.SetActive(false);
				}
			}
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x00211AD8 File Offset: 0x0020FCD8
		private void OnClickChapterChangeButton(PguiButtonCtrl button)
		{
			int num = -1;
			this.SetInactive();
			List<KeyValuePair<int, QuestStaticChapter>> chapterDataByCategory = SceneQuest.GetChapterDataByCategory(this.category, this.IsNormalMode() ? 0 : 1);
			chapterDataByCategory.Sort((KeyValuePair<int, QuestStaticChapter> a, KeyValuePair<int, QuestStaticChapter> b) => b.Key - a.Key);
			int i = 0;
			while (i < this.guiData.chapterChangeList.Count)
			{
				SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange chapterChange = this.guiData.chapterChangeList[i];
				if (chapterChange.Quest_ListBar_ChapterChange == button)
				{
					chapterChange.Current.SetActive(true);
					List<PguiAECtrl> aecheckMark = this.GetAECheckMark();
					if (i >= aecheckMark.Count)
					{
						break;
					}
					num = i;
					int num2 = chapterDataByCategory.Count - 1 - num;
					if (0 <= num2 && num2 < aecheckMark.Count)
					{
						aecheckMark[num2].gameObject.SetActive(true);
						aecheckMark[num2].PlayAnimation(PguiAECtrl.AmimeType.START, null);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (num >= 0)
			{
				this.UpdateCurrentChapterData(chapterDataByCategory[num].Value);
			}
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x00211BE7 File Offset: 0x0020FDE7
		private void UpdateCurrentChapterData(QuestStaticChapter chapterData)
		{
			this.currentChapterMap[this.GetMainStoryDifficulty()] = chapterData;
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x00211BFC File Offset: 0x0020FDFC
		public QuestStaticChapter GetCurrentChapterData()
		{
			if (this.currentChapterMap.ContainsKey(this.GetMainStoryDifficulty()))
			{
				return this.currentChapterMap[this.GetMainStoryDifficulty()];
			}
			List<KeyValuePair<int, QuestStaticChapter>> chapterDataByCategory = SceneQuest.GetChapterDataByCategory(this.category, this.IsNormalMode() ? 0 : 1);
			chapterDataByCategory.Sort((KeyValuePair<int, QuestStaticChapter> a, KeyValuePair<int, QuestStaticChapter> b) => b.Key - a.Key);
			return chapterDataByCategory[0].Value;
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x00211C78 File Offset: 0x0020FE78
		public SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange CreateChapterChange(GameObject go)
		{
			SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange chapterChange = new SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange(go.transform);
			chapterChange.Quest_ListBar_ChapterChange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickChapterChangeButton), PguiButtonCtrl.SoundType.DEFAULT);
			return chapterChange;
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x00211CA0 File Offset: 0x0020FEA0
		private int GetChapterId()
		{
			List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.SwitchCategoryCtrlList[this.SwitchCategoryCtrlIndex]);
			QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[DataManager.DmQuest.QuestStaticData.mapDataMap[playableMapIdList[0]].chapterId];
			if (!this.IsNormalMode())
			{
				return questStaticChapter.hardChapterId;
			}
			return questStaticChapter.chapterId;
		}

		// Token: 0x04004BA3 RID: 19363
		public SceneQuest.QuestChapterChangeWindowCtrl.GUI guiData;

		// Token: 0x04004BA4 RID: 19364
		private QuestStaticChapter.Category category;

		// Token: 0x04004BA6 RID: 19366
		private Dictionary<SceneQuest.MainStoryDifficulty, QuestStaticChapter> currentChapterMap;

		// Token: 0x04004BAA RID: 19370
		private PguiOpenWindowCtrl.Callback pushedCB;

		// Token: 0x0200119D RID: 4509
		public class GUI
		{
			// Token: 0x060056AB RID: 22187 RVA: 0x00253134 File Offset: 0x00251334
			public GUI(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
				this.ScrollView = baseTr.Find("Base/Window/ChapterList/ScrollView").GetComponent<ReuseScroll>();
				for (int i = 0; i < 10; i++)
				{
					Transform transform = baseTr.Find("Base/Window/Img_Map/AEImage_CheckMark_" + (i + 1).ToString("D2"));
					if (transform != null)
					{
						PguiAECtrl component = transform.GetComponent<PguiAECtrl>();
						this.AEImage_CheckMark.Add(component);
						component.gameObject.SetActive(false);
					}
					Transform transform2 = baseTr.Find("Base/Window/Img_Map/AEImage_CheckMark_Cellvall" + (i + 1).ToString("D2"));
					if (transform2 != null)
					{
						PguiAECtrl component2 = transform2.GetComponent<PguiAECtrl>();
						this.AEImage_CheckMarkCellval.Add(component2);
						component2.gameObject.SetActive(false);
					}
					PguiAECtrl component3 = baseTr.Find("Base/Window/Img_Map/AEImage_CheckMark02_" + i.ToString("D2")).GetComponent<PguiAECtrl>();
					this.AEImage_CheckMark2.Add(component3);
					component3.gameObject.SetActive(false);
					Transform transform3 = baseTr.Find("Base/Window/Img_Map/AEImage_CheckMark03_" + i.ToString("D2"));
					if (transform3 != null)
					{
						PguiAECtrl component4 = transform3.GetComponent<PguiAECtrl>();
						this.AEImage_CheckMark3.Add(component4);
						component4.gameObject.SetActive(false);
					}
				}
				this.Btn_Sel_Difficult = baseTr.Find("Base/Window/Btn_Sel_Difficult").GetComponent<PguiButtonCtrl>();
				this.markLock = this.Btn_Sel_Difficult.transform.Find("BaseImage/Mark_Lock").GetComponent<MarkLockCtrl>();
				this.Mark_Hard = baseTr.Find("Base/Window/Mark_Hard").GetComponent<SimpleAnimation>();
				this.Mark_Hard.gameObject.SetActive(false);
				this.Mark_StorySelect = baseTr.Find("Base/Window/Mark_StorySelect").GetComponent<PguiImageCtrl>();
				this.Fint_StorySelect = this.Mark_StorySelect.transform.Find("Fint_StorySelect").GetComponent<PguiImageCtrl>();
				this.Btn_Yaji_Left = baseTr.Find("Base/Window/LeftBtn/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
				this.Btn_Yaji_Right = baseTr.Find("Base/Window/RightBtn/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
			}

			// Token: 0x04006062 RID: 24674
			public static readonly int SCROLL_ITEM_NUN_H = 1;

			// Token: 0x04006063 RID: 24675
			public PguiOpenWindowCtrl owCtrl;

			// Token: 0x04006064 RID: 24676
			public GameObject baseObj;

			// Token: 0x04006065 RID: 24677
			public ReuseScroll ScrollView;

			// Token: 0x04006066 RID: 24678
			public List<PguiAECtrl> AEImage_CheckMark = new List<PguiAECtrl>();

			// Token: 0x04006067 RID: 24679
			public List<PguiAECtrl> AEImage_CheckMarkCellval = new List<PguiAECtrl>();

			// Token: 0x04006068 RID: 24680
			public List<PguiAECtrl> AEImage_CheckMark2 = new List<PguiAECtrl>();

			// Token: 0x04006069 RID: 24681
			public List<PguiAECtrl> AEImage_CheckMark3 = new List<PguiAECtrl>();

			// Token: 0x0400606A RID: 24682
			public List<SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange> chapterChangeList = new List<SceneQuest.QuestChapterChangeWindowCtrl.ChapterChange>();

			// Token: 0x0400606B RID: 24683
			public PguiButtonCtrl Btn_Sel_Difficult;

			// Token: 0x0400606C RID: 24684
			public MarkLockCtrl markLock;

			// Token: 0x0400606D RID: 24685
			public SimpleAnimation Mark_Hard;

			// Token: 0x0400606E RID: 24686
			public PguiImageCtrl Mark_StorySelect;

			// Token: 0x0400606F RID: 24687
			public PguiImageCtrl Fint_StorySelect;

			// Token: 0x04006070 RID: 24688
			public PguiButtonCtrl Btn_Yaji_Left;

			// Token: 0x04006071 RID: 24689
			public PguiButtonCtrl Btn_Yaji_Right;
		}

		// Token: 0x0200119E RID: 4510
		public class ChapterChange
		{
			// Token: 0x17000CFB RID: 3323
			// (get) Token: 0x060056AD RID: 22189 RVA: 0x002533A6 File Offset: 0x002515A6
			// (set) Token: 0x060056AE RID: 22190 RVA: 0x002533AE File Offset: 0x002515AE
			public bool SimpleAnimationPlayRequestFlag { get; set; }

			// Token: 0x060056AF RID: 22191 RVA: 0x002533B8 File Offset: 0x002515B8
			public ChapterChange(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Quest_ListBar_ChapterChange = baseTr.GetComponent<PguiButtonCtrl>();
				this.Current = baseTr.Find("BaseImage/Current").gameObject;
				this.Num_Chapter = baseTr.Find("BaseImage/Num_Chapter").GetComponent<PguiTextCtrl>();
				this.Txt_Chapter = baseTr.Find("BaseImage/Txt_Chapter").GetComponent<PguiTextCtrl>();
				this.Num_Complete = baseTr.Find("BaseImage/Num_Complete").GetComponent<PguiTextCtrl>();
				this.Mark_Complete_Fnt = this.Num_Complete.transform.Find("Mark_Complete_Fnt").GetComponent<PguiImageCtrl>();
				this.Current.SetActive(false);
				this.anim = baseTr.Find("BaseImage").GetComponent<SimpleAnimation>();
				this.Base_Hard = baseTr.Find("BaseImage/Base_Hard").gameObject;
				this.Base_Hard.SetActive(false);
				this.SimpleAnimationPlayRequestFlag = false;
			}

			// Token: 0x04006072 RID: 24690
			public GameObject baseObj;

			// Token: 0x04006073 RID: 24691
			public PguiButtonCtrl Quest_ListBar_ChapterChange;

			// Token: 0x04006074 RID: 24692
			public GameObject Current;

			// Token: 0x04006075 RID: 24693
			public PguiTextCtrl Num_Chapter;

			// Token: 0x04006076 RID: 24694
			public PguiTextCtrl Txt_Chapter;

			// Token: 0x04006077 RID: 24695
			public PguiTextCtrl Num_Complete;

			// Token: 0x04006078 RID: 24696
			public PguiImageCtrl Mark_Complete_Fnt;

			// Token: 0x04006079 RID: 24697
			public SimpleAnimation anim;

			// Token: 0x0400607A RID: 24698
			public GameObject Base_Hard;
		}
	}

	// Token: 0x02000C8B RID: 3211
	public class QuestScheduleWindowCtrl
	{
		// Token: 0x0600463F RID: 17983 RVA: 0x00211E72 File Offset: 0x00210072
		public void Init(GameObject mainObj)
		{
			this.guiData = new SceneQuest.QuestScheduleWindowCtrl.GUI(mainObj.transform);
		}

		// Token: 0x06004640 RID: 17984 RVA: 0x00211E85 File Offset: 0x00210085
		public void Setup(PguiOpenWindowCtrl.Callback cb)
		{
			this.guiData.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, cb, null, false);
			this.guiData.owCtrl.Open();
		}

		// Token: 0x04004BAB RID: 19371
		public SceneQuest.QuestScheduleWindowCtrl.GUI guiData;

		// Token: 0x020011A3 RID: 4515
		public class GUI
		{
			// Token: 0x060056C4 RID: 22212 RVA: 0x002539CF File Offset: 0x00251BCF
			public GUI(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			}

			// Token: 0x0400608A RID: 24714
			public PguiOpenWindowCtrl owCtrl;

			// Token: 0x0400608B RID: 24715
			public GameObject baseObj;
		}
	}

	// Token: 0x02000C8C RID: 3212
	public class ItemInfoWindowCtrl
	{
		// Token: 0x06004642 RID: 17986 RVA: 0x00211EBB File Offset: 0x002100BB
		public void Init(GameObject mainObj)
		{
			this.guiData = new SceneQuest.ItemInfoWindowCtrl.GUI(mainObj.transform);
		}

		// Token: 0x06004643 RID: 17987 RVA: 0x00211ED0 File Offset: 0x002100D0
		public void Setup(QuestStaticQuestOne questOne)
		{
			foreach (SceneQuest.ItemInfoWindowCtrl.ItemIcon itemIcon in this.guiData.itemIcons)
			{
				Object.Destroy(itemIcon.baseObj);
				itemIcon.baseObj = null;
			}
			this.guiData.itemIcons.Clear();
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(questOne.questId);
			this.guiData.owCtrl.Setup(questOnePackData.questChapter.chapterName + questOnePackData.questMap.mapName + "クリアで獲得", null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
			this.guiData.owCtrl.Open();
			foreach (QuestStaticQuestOne.RewardItem rewardItem in questOne.RewardItemList)
			{
				SceneQuest.ItemInfoWindowCtrl.ItemIcon itemIcon2 = new SceneQuest.ItemInfoWindowCtrl.ItemIcon(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/Quest_StoryItemIcon"), this.guiData.Grid.transform).transform);
				itemIcon2.Num_Txt.m_Text.text = string.Format("x{0}", rewardItem.num);
				ItemDef.Kind kind = DataManager.DmItem.GetItemStaticBase(rewardItem.itemId).GetKind();
				int num;
				string text;
				if (kind != ItemDef.Kind.CHARA)
				{
					if (kind != ItemDef.Kind.PHOTO)
					{
						if (kind != ItemDef.Kind.FURNITURE)
						{
							num = 3;
							text = "アイテム";
						}
						else
						{
							num = 2;
							text = "家具";
						}
					}
					else
					{
						num = 4;
						text = "フォト";
					}
				}
				else
				{
					num = 1;
					text = "フレンズ";
				}
				itemIcon2.spriteCtrl.Replace(num);
				itemIcon2.Txt_Kind.text = text;
				this.guiData.itemIcons.Add(itemIcon2);
			}
		}

		// Token: 0x04004BAC RID: 19372
		public SceneQuest.ItemInfoWindowCtrl.GUI guiData;

		// Token: 0x020011A4 RID: 4516
		public class ItemIcon
		{
			// Token: 0x060056C5 RID: 22213 RVA: 0x002539F0 File Offset: 0x00251BF0
			public ItemIcon(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.spriteCtrl = baseTr.GetComponent<PguiReplaceSpriteCtrl>();
				this.Num_Txt = baseTr.Find("NumBase/Num_Txt").GetComponent<PguiTextCtrl>();
				this.Txt_Kind = baseTr.Find("Txt_Kind").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x0400608C RID: 24716
			public GameObject baseObj;

			// Token: 0x0400608D RID: 24717
			public PguiReplaceSpriteCtrl spriteCtrl;

			// Token: 0x0400608E RID: 24718
			public PguiTextCtrl Num_Txt;

			// Token: 0x0400608F RID: 24719
			public PguiTextCtrl Txt_Kind;
		}

		// Token: 0x020011A5 RID: 4517
		public class GUI
		{
			// Token: 0x060056C6 RID: 22214 RVA: 0x00253A48 File Offset: 0x00251C48
			public GUI(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
				this.Grid = baseTr.Find("Base/Window/Grid").gameObject;
				this.itemIcons = new List<SceneQuest.ItemInfoWindowCtrl.ItemIcon>();
			}

			// Token: 0x04006090 RID: 24720
			public PguiOpenWindowCtrl owCtrl;

			// Token: 0x04006091 RID: 24721
			public GameObject baseObj;

			// Token: 0x04006092 RID: 24722
			public GameObject Grid;

			// Token: 0x04006093 RID: 24723
			public List<SceneQuest.ItemInfoWindowCtrl.ItemIcon> itemIcons;
		}
	}

	// Token: 0x02000C8D RID: 3213
	public class ItemInfoWindowAfterCtrl
	{
		// Token: 0x06004645 RID: 17989 RVA: 0x002120E4 File Offset: 0x002102E4
		public void Init(GameObject mainObj)
		{
			this.guiData = new SceneQuest.ItemInfoWindowAfterCtrl.GUI(mainObj.transform);
			this.guiData.ScrollView.InitForce();
			if (this.guiData.ScrollView.onStartItem == null)
			{
				ReuseScroll scrollView = this.guiData.ScrollView;
				scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartWindowItem));
			}
			if (this.guiData.ScrollView.onUpdateItem == null)
			{
				ReuseScroll scrollView2 = this.guiData.ScrollView;
				scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateWindowItem));
			}
			this.guiData.ScrollView.Setup(10, 0);
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x002121A4 File Offset: 0x002103A4
		public void Setup(QuestStaticQuestOne questOne)
		{
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(questOne.questId);
			this.guiData.owCtrl.Setup(questOnePackData.questChapter.chapterName + questOnePackData.questMap.mapName + "クリアで獲得", null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
			this.guiData.owCtrl.Open();
			this.rewardItems = new List<QuestStaticQuestOne.RewardItem>(questOne.RewardItemList);
			this.guiData.ScrollView.Resize(this.rewardItems.Count / SceneQuest.ItemInfoWindowAfterCtrl.GUI.SCROLL_ITEM_NUN_H + 1, 0);
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x00212260 File Offset: 0x00210460
		private void OnStartWindowItem(int index, GameObject go)
		{
			for (int i = 0; i < SceneQuest.ItemInfoWindowAfterCtrl.GUI.SCROLL_ITEM_NUN_H; i++)
			{
				Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/Quest_ItemList_ItemSet"), go.transform).name = i.ToString();
			}
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x002122A4 File Offset: 0x002104A4
		private void OnUpdateWindowItem(int index, GameObject go)
		{
			for (int i = 0; i < SceneQuest.ItemInfoWindowAfterCtrl.GUI.SCROLL_ITEM_NUN_H; i++)
			{
				int num = index * SceneQuest.ItemInfoWindowAfterCtrl.GUI.SCROLL_ITEM_NUN_H + i;
				Transform transform = go.transform.Find(i.ToString());
				if (num < this.rewardItems.Count)
				{
					transform.gameObject.SetActive(true);
					IconItemCtrl component = transform.Find("BaseImage/Icon_Item/Icon_Item").GetComponent<IconItemCtrl>();
					PguiTextCtrl component2 = transform.Find("BaseImage/Title").GetComponent<PguiTextCtrl>();
					PguiTextCtrl component3 = transform.Find("BaseImage/NumBase/Num_Txt").GetComponent<PguiTextCtrl>();
					ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(this.rewardItems[num].itemId);
					component.Setup(itemStaticBase);
					component2.text = itemStaticBase.GetName();
					component3.text = string.Format("x{0}", this.rewardItems[num].num);
				}
				else
				{
					transform.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04004BAD RID: 19373
		public SceneQuest.ItemInfoWindowAfterCtrl.GUI guiData;

		// Token: 0x04004BAE RID: 19374
		private List<QuestStaticQuestOne.RewardItem> rewardItems = new List<QuestStaticQuestOne.RewardItem>();

		// Token: 0x020011A7 RID: 4519
		public class GUI
		{
			// Token: 0x060056CA RID: 22218 RVA: 0x00253AAB File Offset: 0x00251CAB
			public GUI(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
				this.ScrollView = baseTr.Find("Base/Window/List/ScrollView").GetComponent<ReuseScroll>();
			}

			// Token: 0x04006096 RID: 24726
			public static readonly int SCROLL_ITEM_NUN_H = 2;

			// Token: 0x04006097 RID: 24727
			public PguiOpenWindowCtrl owCtrl;

			// Token: 0x04006098 RID: 24728
			public GameObject baseObj;

			// Token: 0x04006099 RID: 24729
			public ReuseScroll ScrollView;
		}
	}

	// Token: 0x02000C8E RID: 3214
	public enum Mode
	{
		// Token: 0x04004BB0 RID: 19376
		INVALID,
		// Token: 0x04004BB1 RID: 19377
		TOP,
		// Token: 0x04004BB2 RID: 19378
		CHARA_EDIT
	}

	// Token: 0x02000C8F RID: 3215
	public class IMapData
	{
		// Token: 0x0600464A RID: 17994 RVA: 0x002123B0 File Offset: 0x002105B0
		public IMapData(Transform baseTr, List<GameObject> carList)
		{
			this.baseObj = baseTr.gameObject;
			this.carObjList = new List<GameObject>(carList);
			foreach (GameObject gameObject in this.carObjList)
			{
				gameObject.SetActive(false);
			}
			this.mapCar = this.carObjList[0].GetComponent<PguiAECtrl>();
			this.mapCar.gameObject.SetActive(true);
			Transform transform = baseTr.Find("Effect");
			if (transform != null)
			{
				this.Rain = transform.transform.Find("Map_Ef_Rain/AEImage").GetComponent<PguiAECtrl>();
				this.Rain_Sunny = transform.transform.Find("Map_Ef_Rain_Sunny/AEImage").GetComponent<PguiAECtrl>();
			}
			Transform transform2 = baseTr.Find("EffectCloudy");
			if (transform2 != null)
			{
				this.Cloudy = transform2.gameObject;
			}
			this.SetWeatherType(QuestWeather.Sunny);
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x002124C8 File Offset: 0x002106C8
		public virtual void OutAnim()
		{
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x002124CA File Offset: 0x002106CA
		public virtual void InAnim()
		{
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x002124CC File Offset: 0x002106CC
		public void SetWeatherType(QuestWeather weatherType)
		{
			PguiAECtrl rain = this.Rain;
			if (rain != null)
			{
				rain.transform.parent.gameObject.SetActive(false);
			}
			GameObject cloudy = this.Cloudy;
			if (cloudy != null)
			{
				cloudy.SetActive(false);
			}
			PguiAECtrl rain_Sunny = this.Rain_Sunny;
			if (rain_Sunny != null)
			{
				rain_Sunny.transform.parent.gameObject.SetActive(false);
			}
			switch (weatherType)
			{
			case QuestWeather.Rain:
			{
				GameObject cloudy2 = this.Cloudy;
				if (cloudy2 != null)
				{
					cloudy2.SetActive(true);
				}
				PguiAECtrl rain2 = this.Rain;
				if (rain2 != null)
				{
					rain2.transform.parent.gameObject.SetActive(true);
				}
				PguiAECtrl rain3 = this.Rain;
				if (rain3 != null)
				{
					rain3.gameObject.SetActive(true);
				}
				PguiAECtrl rain4 = this.Rain;
				if (rain4 == null)
				{
					return;
				}
				rain4.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				return;
			}
			case QuestWeather.Cloudy:
			{
				GameObject cloudy3 = this.Cloudy;
				if (cloudy3 == null)
				{
					return;
				}
				cloudy3.SetActive(true);
				return;
			}
			case QuestWeather.Sunshower:
			{
				PguiAECtrl rain_Sunny2 = this.Rain_Sunny;
				if (rain_Sunny2 != null)
				{
					rain_Sunny2.transform.parent.gameObject.SetActive(true);
				}
				PguiAECtrl rain_Sunny3 = this.Rain_Sunny;
				if (rain_Sunny3 == null)
				{
					return;
				}
				rain_Sunny3.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x002125E4 File Offset: 0x002107E4
		public void SetCarObjType(QuestCarType carType)
		{
			foreach (GameObject gameObject in this.carObjList)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
			if (this.carObjList[(int)carType] != null)
			{
				this.mapCar = this.carObjList[(int)carType].GetComponent<PguiAECtrl>();
				this.mapCar.gameObject.SetActive(true);
			}
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x0021267C File Offset: 0x0021087C
		public void PlayCarAnim()
		{
			if (this.mapCar != null && (!this.mapCar.IsPlaying() || !this.mapCar.m_AEImage.autoPlay))
			{
				this.mapCar.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
				{
					this.mapCar.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				});
			}
		}

		// Token: 0x04004BB3 RID: 19379
		public GameObject baseObj;

		// Token: 0x04004BB4 RID: 19380
		public GameObject mapObj;

		// Token: 0x04004BB5 RID: 19381
		public GameObject bgObj;

		// Token: 0x04004BB6 RID: 19382
		public List<GameObject> carObjList = new List<GameObject>();

		// Token: 0x04004BB7 RID: 19383
		public PguiAECtrl mapCar;

		// Token: 0x04004BB8 RID: 19384
		public PguiAECtrl Rain;

		// Token: 0x04004BB9 RID: 19385
		public PguiAECtrl Rain_Sunny;

		// Token: 0x04004BBA RID: 19386
		public GameObject Cloudy;
	}

	// Token: 0x02000C90 RID: 3216
	public class GuiChapterChange
	{
		// Token: 0x06004651 RID: 18001 RVA: 0x002126E0 File Offset: 0x002108E0
		public GuiChapterChange(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseObj.SetActive(false);
			baseTr.Find("Window_ChapterChange").gameObject.SetActive(false);
			this.endMain = new SceneQuest.GuiChapterChange.EndMain(baseTr.Find("Auth_ChapterEnd_Main"));
			this.endSub = new SceneQuest.GuiChapterChange.EndSub(baseTr.Find("Auth_ChapterEnd_Sub"));
			this.start = new SceneQuest.GuiChapterChange.Start(baseTr.Find("Auth_ChapterStart"));
			this.change = new SceneQuest.GuiChapterChange.Change(baseTr.Find("Auth_ChapterChange"));
			this.modeEnd = new SceneQuest.GuiChapterChange.ModeEnd(baseTr.Find("Auth_ModeEnd"));
		}

		// Token: 0x06004652 RID: 18002 RVA: 0x0021278F File Offset: 0x0021098F
		public void Destroy()
		{
			Object.Destroy(this.baseObj);
			this.baseObj = null;
		}

		// Token: 0x04004BBB RID: 19387
		public GameObject baseObj;

		// Token: 0x04004BBC RID: 19388
		public SceneQuest.GuiChapterChange.EndMain endMain;

		// Token: 0x04004BBD RID: 19389
		public SceneQuest.GuiChapterChange.EndSub endSub;

		// Token: 0x04004BBE RID: 19390
		public SceneQuest.GuiChapterChange.Start start;

		// Token: 0x04004BBF RID: 19391
		public SceneQuest.GuiChapterChange.Change change;

		// Token: 0x04004BC0 RID: 19392
		public SceneQuest.GuiChapterChange.ModeEnd modeEnd;

		// Token: 0x020011A9 RID: 4521
		public class EndMain
		{
			// Token: 0x060056CF RID: 22223 RVA: 0x00253B00 File Offset: 0x00251D00
			public EndMain(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Bg_Pattern = baseTr.Find("Bg/MoveBg_Stripe/Bg_Pattern").GetComponent<PguiImageCtrl>();
				this.Num_Chapter = baseTr.Find("ChapterName_Main/Num_Chapter").GetComponent<PguiTextCtrl>();
				this.Txt = baseTr.Find("ChapterName_Main/Txt").GetComponent<PguiTextCtrl>();
				this.Txt_ChapterName = baseTr.Find("ChapterName_Main/Txt_ChapterName").GetComponent<PguiTextCtrl>();
				this.Txt_Touch = baseTr.Find("Txt_Touch").GetComponent<PguiTextCtrl>();
				this.AEImage_ChapterEnd = baseTr.Find("AEImage_ChapterEnd").GetComponent<PguiAECtrl>();
				this.baseObj.SetActive(false);
			}

			// Token: 0x0400609C RID: 24732
			public GameObject baseObj;

			// Token: 0x0400609D RID: 24733
			public PguiImageCtrl Bg_Pattern;

			// Token: 0x0400609E RID: 24734
			public PguiTextCtrl Num_Chapter;

			// Token: 0x0400609F RID: 24735
			public PguiTextCtrl Txt;

			// Token: 0x040060A0 RID: 24736
			public PguiTextCtrl Txt_ChapterName;

			// Token: 0x040060A1 RID: 24737
			public PguiTextCtrl Txt_Touch;

			// Token: 0x040060A2 RID: 24738
			public PguiAECtrl AEImage_ChapterEnd;
		}

		// Token: 0x020011AA RID: 4522
		public class EndSub
		{
			// Token: 0x060056D0 RID: 22224 RVA: 0x00253BB0 File Offset: 0x00251DB0
			public EndSub(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_Touch = baseTr.Find("Txt_Touch").GetComponent<PguiTextCtrl>();
				this.AEImage_ChapterEnd = baseTr.Find("AEImage_ChapterEnd").GetComponent<PguiAECtrl>();
				this.Num_Chapter = baseTr.Find("ChapterName_Sub/Num_Chapter").GetComponent<PguiTextCtrl>();
				this.Txt_ChapterName = baseTr.Find("ChapterName_Sub/Txt_ChapterName").GetComponent<PguiTextCtrl>();
				this.baseObj.SetActive(false);
			}

			// Token: 0x040060A3 RID: 24739
			public GameObject baseObj;

			// Token: 0x040060A4 RID: 24740
			public PguiTextCtrl Txt_Touch;

			// Token: 0x040060A5 RID: 24741
			public PguiAECtrl AEImage_ChapterEnd;

			// Token: 0x040060A6 RID: 24742
			public PguiTextCtrl Num_Chapter;

			// Token: 0x040060A7 RID: 24743
			public PguiTextCtrl Txt_ChapterName;
		}

		// Token: 0x020011AB RID: 4523
		public class Start
		{
			// Token: 0x060056D1 RID: 22225 RVA: 0x00253C34 File Offset: 0x00251E34
			public Start(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Bg_Pattern = baseTr.Find("Bg/MoveBg_Stripe/Bg_Pattern").GetComponent<PguiImageCtrl>();
				this.Txt = baseTr.Find("NewChapter/ChapterNumBase/Txt").GetComponent<PguiTextCtrl>();
				this.Num_Chapter = baseTr.Find("NewChapter/ChapterNumBase/Num_Chapter").GetComponent<PguiTextCtrl>();
				this.Txt_ChapterName = baseTr.Find("NewChapter/Txt_ChapterName").GetComponent<PguiTextCtrl>();
				this.Txt_Touch = baseTr.Find("Txt_Touch").GetComponent<PguiTextCtrl>();
				this.Bg = baseTr.Find("Bg").GetComponent<PguiAECtrl>();
				this.AEImage_ChapterStart = baseTr.Find("AEImage_ChapterStart").GetComponent<PguiAECtrl>();
				this.Null_AEImage_ChapterAdd = baseTr.Find("Null_AEImage_ChapterAdd").gameObject;
				this.Window_Num_Chapter = baseTr.Find("ChapterTitle/Txt_Chapter").GetComponent<PguiTextCtrl>();
				this.Window_Txt_ChapterName = baseTr.Find("ChapterNumBase/Num_Chapter").GetComponent<PguiTextCtrl>();
				this.baseObj.SetActive(false);
			}

			// Token: 0x040060A8 RID: 24744
			public GameObject baseObj;

			// Token: 0x040060A9 RID: 24745
			public PguiImageCtrl Bg_Pattern;

			// Token: 0x040060AA RID: 24746
			public PguiTextCtrl Txt;

			// Token: 0x040060AB RID: 24747
			public PguiTextCtrl Num_Chapter;

			// Token: 0x040060AC RID: 24748
			public PguiTextCtrl Txt_ChapterName;

			// Token: 0x040060AD RID: 24749
			public PguiTextCtrl Txt_Touch;

			// Token: 0x040060AE RID: 24750
			public PguiAECtrl Bg;

			// Token: 0x040060AF RID: 24751
			public PguiAECtrl AEImage_ChapterStart;

			// Token: 0x040060B0 RID: 24752
			public GameObject Null_AEImage_ChapterAdd;

			// Token: 0x040060B1 RID: 24753
			public PguiTextCtrl Window_Num_Chapter;

			// Token: 0x040060B2 RID: 24754
			public PguiTextCtrl Window_Txt_ChapterName;
		}

		// Token: 0x020011AC RID: 4524
		public class Change
		{
			// Token: 0x060056D2 RID: 22226 RVA: 0x00253D3C File Offset: 0x00251F3C
			public Change(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Bg_Pattern = baseTr.Find("Bg/MoveBg_Stripe/Bg_Pattern").GetComponent<PguiImageCtrl>();
				this.Txt_Chapter = baseTr.Find("ChapterTitle/Txt_Chapter").GetComponent<PguiTextCtrl>();
				this.Txt = baseTr.Find("ChapterNumBase/Txt").GetComponent<PguiTextCtrl>();
				this.Num_Chapter = baseTr.Find("ChapterNumBase/Num_Chapter").GetComponent<PguiTextCtrl>();
				this.Txt_Touch = baseTr.Find("Txt_Touch").GetComponent<PguiTextCtrl>();
				this.Bg = baseTr.Find("Bg").GetComponent<PguiAECtrl>();
				this.AEImage_ChapterStart = baseTr.Find("AEImage_ChapterStart").GetComponent<PguiAECtrl>();
				this.Null_AEImage_ChapterAdd = baseTr.Find("Null_AEImage_ChapterAdd").gameObject;
				this.baseObj.SetActive(false);
			}

			// Token: 0x040060B3 RID: 24755
			public GameObject baseObj;

			// Token: 0x040060B4 RID: 24756
			public PguiImageCtrl Bg_Pattern;

			// Token: 0x040060B5 RID: 24757
			public PguiTextCtrl Txt_Chapter;

			// Token: 0x040060B6 RID: 24758
			public PguiTextCtrl Txt;

			// Token: 0x040060B7 RID: 24759
			public PguiTextCtrl Num_Chapter;

			// Token: 0x040060B8 RID: 24760
			public PguiTextCtrl Txt_Touch;

			// Token: 0x040060B9 RID: 24761
			public PguiAECtrl Bg;

			// Token: 0x040060BA RID: 24762
			public PguiAECtrl AEImage_ChapterStart;

			// Token: 0x040060BB RID: 24763
			public GameObject Null_AEImage_ChapterAdd;
		}

		// Token: 0x020011AD RID: 4525
		public class ModeEnd
		{
			// Token: 0x060056D3 RID: 22227 RVA: 0x00253E18 File Offset: 0x00252018
			public ModeEnd(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_Mode_Main = baseTr.Find("Window_ModeEnd/Mode_Main/Txt_ModeName").GetComponent<PguiTextCtrl>();
				this.Txt_Mode_Arai = baseTr.Find("Window_ModeEnd/Mode_Arai/Txt_ModeName").GetComponent<PguiTextCtrl>();
				this.Image_Mode_Arai = baseTr.Find("Window_ModeEnd/Mode_Arai/Texture_Chara_Arai").GetComponent<PguiRawImageCtrl>();
				this.Image_Mode_Main = baseTr.Find("Window_ModeEnd/Mode_Main/Texture_Chara_Main").GetComponent<PguiRawImageCtrl>();
				this.Txt_Info = baseTr.Find("Window_ModeEnd/Txt_Info").GetComponent<PguiTextCtrl>();
				this.AEImage_ModeEnd = baseTr.Find("AEImage_ModeEnd").GetComponent<PguiAECtrl>();
				this.Fnt_Tsuzuku = baseTr.Find("Window_ModeEnd/Fnt_Tsuzuku").GetComponent<PguiImageCtrl>();
				this.baseObj.SetActive(false);
			}

			// Token: 0x060056D4 RID: 22228 RVA: 0x00253EE0 File Offset: 0x002520E0
			private void SetTextModeMain(string str)
			{
				this.Txt_Mode_Arai.transform.parent.gameObject.SetActive(false);
				this.Txt_Mode_Main.transform.parent.gameObject.SetActive(true);
				this.Txt_Mode_Main.text = str;
			}

			// Token: 0x060056D5 RID: 22229 RVA: 0x00253F30 File Offset: 0x00252130
			private void SetTextModeArai(string str)
			{
				this.Txt_Mode_Main.transform.parent.gameObject.SetActive(false);
				this.Txt_Mode_Arai.transform.parent.gameObject.SetActive(true);
				this.Txt_Mode_Arai.text = str;
			}

			// Token: 0x060056D6 RID: 22230 RVA: 0x00253F7F File Offset: 0x0025217F
			private void SetActiveEnd(bool sw)
			{
				this.Txt_Info.gameObject.SetActive(sw);
				this.Fnt_Tsuzuku.gameObject.SetActive(sw);
			}

			// Token: 0x060056D7 RID: 22231 RVA: 0x00253FA3 File Offset: 0x002521A3
			public IEnumerator SequenceCtrl(QuestUtil.SelectData selectData)
			{
				this.touch = false;
				this.SetActiveEnd(true);
				QuestStaticChapter questStaticChapter = null;
				if (DataManager.DmQuest.QuestStaticData.chapterDataMap.ContainsKey(selectData.chapterId))
				{
					questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[selectData.chapterId];
				}
				if (SceneQuest.IsMainStoryPart1(selectData.questCategory))
				{
					this.SetTextModeMain(QuestUtil.TitleMain);
					this.Image_Mode_Main.transform.parent.GetComponent<PguiReplaceSpriteCtrl>().Replace(1);
					this.Image_Mode_Main.SetRawImage((questStaticChapter != null && questStaticChapter.EndType != QuestStaticChapter.ChapterEndType.Default) ? "Texture2D/QuestEnd/img_chibichara_hanamaru_end" : "Texture2D/QuestEnd/img_chibichara_hanamaru", true, true, null);
				}
				else if (SceneQuest.IsMainStoryPart1_5(selectData.questCategory))
				{
					this.SetTextModeMain(QuestUtil.TitleCellval);
					this.Image_Mode_Main.transform.parent.GetComponent<PguiReplaceSpriteCtrl>().Replace(1);
					this.Image_Mode_Main.SetRawImage((questStaticChapter != null && questStaticChapter.EndType != QuestStaticChapter.ChapterEndType.Default) ? "Texture2D/QuestEnd/img_chibichara_cellvall_end" : "Texture2D/QuestEnd/img_chibichara_cellvall", true, true, null);
				}
				else if (SceneQuest.IsMainStoryPart2(selectData.questCategory))
				{
					this.SetTextModeMain(QuestUtil.TitleMain2);
					this.Image_Mode_Main.transform.parent.GetComponent<PguiReplaceSpriteCtrl>().Replace(2);
					this.Image_Mode_Main.SetRawImage((questStaticChapter != null && questStaticChapter.EndType != QuestStaticChapter.ChapterEndType.Default) ? "Texture2D/QuestEnd/img_chibichara_kako_end" : "Texture2D/QuestEnd/img_chibichara_kako", true, true, null);
				}
				else if (SceneQuest.IsMainStoryPart3(selectData.questCategory))
				{
					this.SetTextModeMain(QuestUtil.TitleMain3);
					this.Image_Mode_Main.transform.parent.GetComponent<PguiReplaceSpriteCtrl>().Replace(3);
					this.Image_Mode_Main.SetRawImage((questStaticChapter != null && questStaticChapter.EndType != QuestStaticChapter.ChapterEndType.Default) ? "Texture2D/QuestEnd/img_chibichara_hikari_end" : "Texture2D/QuestEnd/img_chibichara_hikari", true, true, null);
				}
				else
				{
					this.SetTextModeArai(QuestUtil.TitleArai);
					this.Image_Mode_Arai.SetRawImage((questStaticChapter != null && questStaticChapter.EndType != QuestStaticChapter.ChapterEndType.Default) ? "Texture2D/QuestEnd/img_chibichara_arai_end" : "Texture2D/QuestEnd/img_chibichara_arai", true, true, null);
				}
				if (questStaticChapter != null && questStaticChapter.EndType != QuestStaticChapter.ChapterEndType.Default)
				{
					this.SetActiveEnd(false);
				}
				SoundManager.Play("prd_se_selector_up_to_here", false, false);
				this.AEImage_ModeEnd.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
				{
					this.AEImage_ModeEnd.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
					PrjUtil.AddTouchEventTrigger(this.baseObj, delegate(Transform x)
					{
						this.touch = true;
					});
				});
				while (!this.touch)
				{
					yield return null;
				}
				this.AEImage_ModeEnd.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				while (this.AEImage_ModeEnd.IsPlaying())
				{
					yield return null;
				}
				yield break;
			}

			// Token: 0x040060BC RID: 24764
			public GameObject baseObj;

			// Token: 0x040060BD RID: 24765
			public PguiTextCtrl Txt_Mode_Main;

			// Token: 0x040060BE RID: 24766
			public PguiTextCtrl Txt_Mode_Arai;

			// Token: 0x040060BF RID: 24767
			public PguiTextCtrl Txt_Info;

			// Token: 0x040060C0 RID: 24768
			public PguiAECtrl AEImage_ModeEnd;

			// Token: 0x040060C1 RID: 24769
			public PguiRawImageCtrl Image_Mode_Arai;

			// Token: 0x040060C2 RID: 24770
			public PguiRawImageCtrl Image_Mode_Main;

			// Token: 0x040060C3 RID: 24771
			public PguiImageCtrl Fnt_Tsuzuku;

			// Token: 0x040060C4 RID: 24772
			private bool touch;
		}
	}

	// Token: 0x02000C91 RID: 3217
	public class GUI
	{
		// Token: 0x06004653 RID: 18003 RVA: 0x002127A4 File Offset: 0x002109A4
		public GUI(GameObject basePanel, GameObject eventCmn)
		{
			this.basePanel = basePanel;
			this.questTop = new SceneQuest.GUI.QuestTop(basePanel.transform.Find("QuestTop"));
			this.chapterSelect = new SceneQuest.GUI.ChapterSelect(basePanel.transform.Find("ChapterSelect"));
			this.locationInfo = new SceneQuest.GUI.LocationInfo(basePanel.transform.Find("LocationInfo"));
			this.charaGrow = new SceneQuest.GUI.CharaGrow(basePanel.transform.Find("ChapterSelect/Left/CharaGrow"));
			eventCmn.transform.parent = this.chapterSelect.baseObj.transform.Find("Left");
			this.locationEvent = new SceneQuest.GUI.LocationEvent(eventCmn.transform);
			this.sideStory = new SceneQuest.GUI.SideStory(basePanel.transform.Find("ChapterSelect/Left/AraiQuest"));
			this.selectObjs.Add(this.questTop.baseObj);
			this.selectObjs.Add(this.chapterSelect.baseObj);
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x002128B4 File Offset: 0x00210AB4
		public void DeactivateGameObject()
		{
			this.basePanel.transform.Find("AraiQuest_ChapterChange").gameObject.SetActive(false);
			this.basePanel.transform.Find("EtceteraQuest_ChapterChange").gameObject.SetActive(false);
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x00212904 File Offset: 0x00210B04
		public void SwitchSelector(SceneQuest.GUI.SetupSwitchSelectorParam param)
		{
			foreach (GameObject gameObject in this.selectObjs)
			{
				gameObject.SetActive(gameObject == param.nextObj);
			}
			this.charaGrow.baseObj.SetActive(this.chapterSelect.baseObj == param.nextObj && param.selectData.questCategory == QuestStaticChapter.Category.GROW);
			this.sideStory.baseObj.SetActive(this.chapterSelect.baseObj == param.nextObj && param.selectData.questCategory == QuestStaticChapter.Category.SIDE_STORY);
			if (param.mapDataGameObj != null)
			{
				if (param.mapDataGameObj.baseObj != null)
				{
					GameObject baseObj = param.mapDataGameObj.baseObj;
					if (baseObj != null)
					{
						baseObj.SetActive(param.isMapNeedQuest && this.questTop.baseObj != param.nextObj);
					}
					PguiAECtrl mapCar = param.mapDataGameObj.mapCar;
					if (mapCar != null)
					{
						mapCar.gameObject.SetActive(param.isMapNeedQuest && this.questTop.baseObj != param.nextObj);
					}
				}
				if (param.currentObj != null && param.mapDataGameObj.baseObj != null)
				{
					if (param.currentObj == this.questTop.baseObj)
					{
						param.mapDataGameObj.PlayCarAnim();
					}
					else
					{
						SceneQuest.IMapData mapDataGameObj = param.mapDataGameObj;
						if (mapDataGameObj != null)
						{
							mapDataGameObj.mapCar.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
						}
					}
				}
			}
			if (param.nextObj == param.charaSelectObj)
			{
				UnityAction charaSelectActionCB = param.charaSelectActionCB;
				if (charaSelectActionCB != null)
				{
					charaSelectActionCB();
				}
			}
			else if (param.nextObj == param.pointSelectObj)
			{
				UnityAction pointSelectActionCB = param.pointSelectActionCB;
				if (pointSelectActionCB != null)
				{
					pointSelectActionCB();
				}
			}
			else if (param.nextObj == this.chapterSelect.baseObj)
			{
				this.chapterSelect.ResetCampaignInfoCategory();
				this.chapterSelect.UpdateCampaignInfoCategory(param.selectData.questCategory, param.selectData.chapterId);
				QuestOneStatus questOneStatus = QuestOneStatus.INVALID;
				QuestOnePackData questOnePack = SceneQuest.GetQuestOnePackDataForReleaseIdStoryHardMode(param.selectData.questCategory);
				string text;
				if (questOnePack != null)
				{
					string mainStoryName = SceneQuest.GetMainStoryName(questOnePack.questChapter.category, true);
					text = mainStoryName + ((mainStoryName != "") ? "\n" : "") + questOnePack.questChapter.chapterName + questOnePack.questGroup.titleName + PrjUtil.MakeMessage("クリア");
					if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questOnePack.questOne.questId))
					{
						questOneStatus = DataManager.DmQuest.QuestDynamicData.oneDataMap[questOnePack.questOne.questId].status;
					}
					SceneQuest.IMapData mapDataGameObj2 = param.mapDataGameObj;
					if (mapDataGameObj2 != null)
					{
						mapDataGameObj2.SetCarObjType(SelMainStoryCtrl.GetQuestCarType(questOnePack.questChapter.chapterId));
					}
				}
				else
				{
					text = "クエスト情報がありません";
				}
				MarkLockCtrl markLock = this.chapterSelect.markLock;
				MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();
				setupParam.updateConditionCallback = () => QuestUtil.IsHardMode(new QuestUtil.SelectData
				{
					questCategory = QuestStaticChapter.Category.STORY,
					chapterId = ((questOnePack != null) ? questOnePack.questChapter.chapterId : 0)
				});
				setupParam.releaseFlag = questOneStatus != QuestOneStatus.INVALID && questOneStatus != QuestOneStatus.NEW;
				setupParam.tagetObject = this.chapterSelect.Btn_Sel_Difficult.gameObject;
				setupParam.text = text;
				setupParam.updateUserFlagDataCallback = delegate
				{
				};
				markLock.Setup(setupParam, true);
			}
			if (param.isMapNeedQuest && this.questTop.baseObj != param.nextObj)
			{
				SceneQuest.TouchCallback startRegistCb = param.startRegistCb;
				if (startRegistCb != null)
				{
					startRegistCb();
				}
				SceneQuest.TouchCallback touchRegistCb = param.touchRegistCb;
				if (touchRegistCb != null)
				{
					touchRegistCb();
				}
				SceneQuest.TouchCallback releaseRegistCb = param.releaseRegistCb;
				if (releaseRegistCb != null)
				{
					releaseRegistCb();
				}
			}
			else
			{
				SceneQuest.TouchCallback startReleaseCb = param.startReleaseCb;
				if (startReleaseCb != null)
				{
					startReleaseCb();
				}
				SceneQuest.TouchCallback touchReleaseCb = param.touchReleaseCb;
				if (touchReleaseCb != null)
				{
					touchReleaseCb();
				}
				SceneQuest.TouchCallback releaseReleaseCb = param.releaseReleaseCb;
				if (releaseReleaseCb != null)
				{
					releaseReleaseCb();
				}
			}
			this.basePanel.GetComponent<PguiPanel>().raycastTarget = param.pointSelectObj != param.nextObj;
			if (param.enableBlur)
			{
				Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.BACK).GetComponent<Blur>().enabled = this.chapterSelect.baseObj == param.nextObj;
			}
			SceneQuest.UpdateCallback updateCb = param.updateCb;
			if (updateCb == null)
			{
				return;
			}
			updateCb();
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x00212DDC File Offset: 0x00210FDC
		public void SetActive(bool b, SceneQuest.IMapData mapDataGameObj)
		{
			this.basePanel.SetActive(b);
			if (mapDataGameObj != null && mapDataGameObj.baseObj != null)
			{
				mapDataGameObj.baseObj.SetActive(b);
				if (mapDataGameObj != null)
				{
					PguiAECtrl mapCar = mapDataGameObj.mapCar;
					if (mapCar != null)
					{
						mapCar.gameObject.SetActive(b);
					}
				}
			}
			if (!b)
			{
				Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.BACK).GetComponent<Blur>().enabled = false;
			}
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x00212E45 File Offset: 0x00211045
		public void Destroy()
		{
			Object.Destroy(this.basePanel);
			this.basePanel = null;
		}

		// Token: 0x04004BC1 RID: 19393
		public GameObject basePanel;

		// Token: 0x04004BC2 RID: 19394
		public SceneQuest.GUI.QuestTop questTop;

		// Token: 0x04004BC3 RID: 19395
		public SceneQuest.GUI.ChapterSelect chapterSelect;

		// Token: 0x04004BC4 RID: 19396
		public SceneQuest.GUI.LocationInfo locationInfo;

		// Token: 0x04004BC5 RID: 19397
		public SceneQuest.GUI.CharaGrow charaGrow;

		// Token: 0x04004BC6 RID: 19398
		public SceneQuest.GUI.LocationEvent locationEvent;

		// Token: 0x04004BC7 RID: 19399
		public SceneQuest.GUI.SideStory sideStory;

		// Token: 0x04004BC8 RID: 19400
		public List<GameObject> selectObjs = new List<GameObject>();

		// Token: 0x020011AE RID: 4526
		public class QuestTop
		{
			// Token: 0x060056DA RID: 22234 RVA: 0x00253FE8 File Offset: 0x002521E8
			public QuestTop(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.storyQuestParts = new SceneQuest.GUI.QuestTop.StoryQuestParts(baseTr);
				this.Btn_GrowQuest = baseTr.Find("Btn_GrowQuest/Base").GetComponent<PguiButtonCtrl>();
				this.GrowQuest_Mark_New = baseTr.Find("Btn_GrowQuest/Base/BaseImage/Mark_New").GetComponent<PguiNestPrefab>();
				this.CampaignGrowQuest = new SceneQuest.GUI.QuestTop.Campaign(baseTr.Find("Btn_GrowQuest/Base"));
				this.Btn_GrowQuestAnim = baseTr.Find("Btn_GrowQuest").GetComponent<SimpleAnimation>();
				this.markLockGrowQuest = baseTr.Find("Btn_GrowQuest/Base/Mark_Lock_New").GetComponent<MarkLockCtrl>();
				this.Btn_EtceteraQuest = baseTr.Find("Btn_EtceteraQuest/Base").GetComponent<PguiButtonCtrl>();
				this.EtceteraQuest_Mark_New = baseTr.Find("Btn_EtceteraQuest/Base/BaseImage/Mark_New").GetComponent<PguiNestPrefab>();
				this.CampaignEtceteraQuest = new SceneQuest.GUI.QuestTop.Campaign(baseTr.Find("Btn_EtceteraQuest/Base"));
				this.Btn_EtceteraQuestAnim = baseTr.Find("Btn_EtceteraQuest").GetComponent<SimpleAnimation>();
				this.markLockEtceteraQuest = baseTr.Find("Btn_EtceteraQuest/Base/Mark_Lock_New").GetComponent<MarkLockCtrl>();
				this.Btn_CharQuest = baseTr.Find("Btn_CharQuest").GetComponent<PguiButtonCtrl>();
				this.CharQuest_Mark_New = baseTr.Find("Btn_CharQuest/BaseImage/Mark_New").GetComponent<PguiNestPrefab>();
				this.CampaignCharQuest = new SceneQuest.GUI.QuestTop.Campaign(baseTr.Find("Btn_CharQuest"));
				this.markLockCharaQuest = baseTr.Find("Btn_CharQuest/Mark_Lock_New").GetComponent<MarkLockCtrl>();
				this.Btn_AnotherStory = baseTr.Find("Btn_AnotherStory").GetComponent<PguiButtonCtrl>();
				this.AnotherStory_Mark_New = baseTr.Find("Btn_AnotherStory/BaseImage/Mark_New").GetComponent<PguiNestPrefab>();
				this.CampaignAnotherStory = new SceneQuest.GUI.QuestTop.Campaign(baseTr.Find("Btn_AnotherStory"));
				this.markLockAnotherStoryQuest = baseTr.Find("Btn_AnotherStory/Mark_Lock_New").GetComponent<MarkLockCtrl>();
				this.Btn_Training = baseTr.Find("Btn_Training").GetComponent<PguiButtonCtrl>();
				this.Training_Mark_New = baseTr.Find("Btn_Training/BaseImage/Mark_New").GetComponent<PguiNestPrefab>();
				this.CampaignTraining = new SceneQuest.GUI.QuestTop.Campaign(baseTr.Find("Btn_Training"));
				this.markLockTrainingQuest = baseTr.Find("Btn_Training/Mark_Lock_New").GetComponent<MarkLockCtrl>();
				this.Btn_EventAll = baseTr.Find("EventGroup/Btn_EventAll").GetComponent<PguiButtonCtrl>();
				this.Img_Yaji_Down = this.Btn_EventAll.transform.Find("BaseImage/Img_Yaji_Down").GetComponent<PguiImageCtrl>();
				this.Img_Yaji_Up = this.Btn_EventAll.transform.Find("BaseImage/Img_Yaji_Up").GetComponent<PguiImageCtrl>();
				this.EventGroup = baseTr.Find("EventGroup").gameObject;
				this.AnotherStory_Mark_New.gameObject.SetActive(false);
				this.Window_EventAll = baseTr.Find("EventGroup/Window_EventAll").GetComponent<SimpleAnimation>();
				this.ScrollView = this.Window_EventAll.transform.Find("ScrollView").GetComponent<ReuseScroll>();
				this.eventPartsList = new List<SceneQuest.GUI.QuestTop.EventParts>
				{
					new SceneQuest.GUI.QuestTop.EventParts
					{
						bannerImage = baseTr.Find("EventGroup/Btn_Event01/BaseImage").GetComponent<PguiRawImageCtrl>(),
						bannerText = baseTr.Find("EventGroup/Btn_Event01/EventTerm01/Num_Term01").GetComponent<PguiTextCtrl>(),
						Mark_EventBefore = baseTr.Find("EventGroup/Btn_Event01/Mark_EventBefore").GetComponent<PguiImageCtrl>(),
						Mark_EventOpen = baseTr.Find("EventGroup/Btn_Event01/Mark_EventOpen").GetComponent<PguiImageCtrl>()
					},
					new SceneQuest.GUI.QuestTop.EventParts
					{
						bannerImage = baseTr.Find("EventGroup/Btn_Event02/BaseImage").GetComponent<PguiRawImageCtrl>(),
						bannerText = baseTr.Find("EventGroup/Btn_Event02/EventTerm02/Num_Term01").GetComponent<PguiTextCtrl>(),
						Mark_EventBefore = baseTr.Find("EventGroup/Btn_Event02/Mark_EventBefore").GetComponent<PguiImageCtrl>(),
						Mark_EventOpen = baseTr.Find("EventGroup/Btn_Event02/Mark_EventOpen").GetComponent<PguiImageCtrl>()
					}
				};
				this.eventButton = new List<PguiButtonCtrl>
				{
					baseTr.Find("EventGroup/Btn_Event01").GetComponent<PguiButtonCtrl>(),
					baseTr.Find("EventGroup/Btn_Event02").GetComponent<PguiButtonCtrl>()
				};
				this.ladyBugAnim = baseTr.Find("BgObj_Ladybug").GetComponent<SimpleAnimation>();
				this.pallAnim = baseTr.Find("BgObjl_Pall").GetComponent<SimpleAnimation>();
				this.Btn_AssistantEdit = baseTr.Find("BtnAssistant_Edit").GetComponent<PguiButtonCtrl>();
				this.markLockAssistantEdit = baseTr.Find("BtnAssistant_Edit/Mark_Lock_New").GetComponent<MarkLockCtrl>();
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<RectTransform>();
				gameObject.name = "SelQuestAssistantCtrl";
				gameObject.transform.SetParent(this.baseObj.transform, false);
				this.selAssistantCtrl = gameObject.AddComponent<SelAssistantCtrl>();
				this.selAssistantCtrl.transform.SetAsLastSibling();
				this.selAssistantCtrl.Init(SelAssistantCtrl.Scene.QUEST);
			}

			// Token: 0x060056DB RID: 22235 RVA: 0x0025446C File Offset: 0x0025266C
			public void UpdateCampaignInfoCategory(QuestStaticChapter.Category category, int chapterId)
			{
				List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(category, chapterId));
				switch (category)
				{
				case QuestStaticChapter.Category.STORY:
				case QuestStaticChapter.Category.CELLVAL:
				case QuestStaticChapter.Category.STORY2:
				case QuestStaticChapter.Category.STORY3:
					this.storyQuestParts.CampaignStoryQuest.DispCampaign(list);
					return;
				case QuestStaticChapter.Category.GROW:
					this.CampaignGrowQuest.DispCampaign(list);
					return;
				case QuestStaticChapter.Category.CHARA:
					this.CampaignCharQuest.DispCampaign(list);
					return;
				case QuestStaticChapter.Category.PVP:
				case QuestStaticChapter.Category.EVENT:
				case QuestStaticChapter.Category.TUTORIAL:
				case QuestStaticChapter.Category.SCENARIO_SP_PVP:
					break;
				case QuestStaticChapter.Category.SIDE_STORY:
					this.CampaignAnotherStory.DispCampaign(list);
					return;
				case QuestStaticChapter.Category.TRAINING:
					this.CampaignTraining.DispCampaign(list);
					return;
				case QuestStaticChapter.Category.ETCETERA:
					this.CampaignEtceteraQuest.DispCampaign(list);
					break;
				default:
					return;
				}
			}

			// Token: 0x060056DC RID: 22236 RVA: 0x00254518 File Offset: 0x00252718
			public void ResetCampaignInfoCategory(QuestStaticChapter.Category category, int chapterId)
			{
				switch (category)
				{
				case QuestStaticChapter.Category.STORY:
				case QuestStaticChapter.Category.CELLVAL:
				case QuestStaticChapter.Category.STORY2:
				case QuestStaticChapter.Category.STORY3:
					this.storyQuestParts.CampaignStoryQuest.ResetCampaign();
					return;
				case QuestStaticChapter.Category.GROW:
					this.CampaignGrowQuest.ResetCampaign();
					return;
				case QuestStaticChapter.Category.CHARA:
					this.CampaignCharQuest.ResetCampaign();
					return;
				case QuestStaticChapter.Category.PVP:
				case QuestStaticChapter.Category.EVENT:
				case QuestStaticChapter.Category.TUTORIAL:
				case QuestStaticChapter.Category.SCENARIO_SP_PVP:
					break;
				case QuestStaticChapter.Category.SIDE_STORY:
					this.CampaignAnotherStory.ResetCampaign();
					return;
				case QuestStaticChapter.Category.TRAINING:
					this.CampaignTraining.ResetCampaign();
					return;
				case QuestStaticChapter.Category.ETCETERA:
					this.CampaignEtceteraQuest.ResetCampaign();
					break;
				default:
					return;
				}
			}

			// Token: 0x040060C5 RID: 24773
			public GameObject baseObj;

			// Token: 0x040060C6 RID: 24774
			public SceneQuest.GUI.QuestTop.StoryQuestParts storyQuestParts;

			// Token: 0x040060C7 RID: 24775
			public PguiButtonCtrl Btn_GrowQuest;

			// Token: 0x040060C8 RID: 24776
			public PguiNestPrefab GrowQuest_Mark_New;

			// Token: 0x040060C9 RID: 24777
			public SceneQuest.GUI.QuestTop.Campaign CampaignGrowQuest;

			// Token: 0x040060CA RID: 24778
			public SimpleAnimation Btn_GrowQuestAnim;

			// Token: 0x040060CB RID: 24779
			public MarkLockCtrl markLockGrowQuest;

			// Token: 0x040060CC RID: 24780
			public PguiButtonCtrl Btn_CharQuest;

			// Token: 0x040060CD RID: 24781
			public PguiNestPrefab CharQuest_Mark_New;

			// Token: 0x040060CE RID: 24782
			public SceneQuest.GUI.QuestTop.Campaign CampaignCharQuest;

			// Token: 0x040060CF RID: 24783
			public MarkLockCtrl markLockCharaQuest;

			// Token: 0x040060D0 RID: 24784
			public PguiButtonCtrl Btn_AnotherStory;

			// Token: 0x040060D1 RID: 24785
			public PguiNestPrefab AnotherStory_Mark_New;

			// Token: 0x040060D2 RID: 24786
			public SceneQuest.GUI.QuestTop.Campaign CampaignAnotherStory;

			// Token: 0x040060D3 RID: 24787
			public MarkLockCtrl markLockAnotherStoryQuest;

			// Token: 0x040060D4 RID: 24788
			public PguiButtonCtrl Btn_Training;

			// Token: 0x040060D5 RID: 24789
			public PguiNestPrefab Training_Mark_New;

			// Token: 0x040060D6 RID: 24790
			public SceneQuest.GUI.QuestTop.Campaign CampaignTraining;

			// Token: 0x040060D7 RID: 24791
			public MarkLockCtrl markLockTrainingQuest;

			// Token: 0x040060D8 RID: 24792
			public PguiButtonCtrl Btn_EventAll;

			// Token: 0x040060D9 RID: 24793
			public PguiImageCtrl Img_Yaji_Down;

			// Token: 0x040060DA RID: 24794
			public PguiImageCtrl Img_Yaji_Up;

			// Token: 0x040060DB RID: 24795
			public ReuseScroll ScrollView;

			// Token: 0x040060DC RID: 24796
			public GameObject EventGroup;

			// Token: 0x040060DD RID: 24797
			public RenderTextureChara renderTextureChara;

			// Token: 0x040060DE RID: 24798
			public SimpleAnimation Window_EventAll;

			// Token: 0x040060DF RID: 24799
			public List<SceneQuest.GUI.QuestTop.EventParts> eventPartsList;

			// Token: 0x040060E0 RID: 24800
			public List<PguiButtonCtrl> eventButton;

			// Token: 0x040060E1 RID: 24801
			public List<PguiButtonCtrl> eventBannerButton = new List<PguiButtonCtrl>();

			// Token: 0x040060E2 RID: 24802
			public SimpleAnimation ladyBugAnim;

			// Token: 0x040060E3 RID: 24803
			public SimpleAnimation pallAnim;

			// Token: 0x040060E4 RID: 24804
			public PguiButtonCtrl Btn_EtceteraQuest;

			// Token: 0x040060E5 RID: 24805
			public PguiNestPrefab EtceteraQuest_Mark_New;

			// Token: 0x040060E6 RID: 24806
			public SceneQuest.GUI.QuestTop.Campaign CampaignEtceteraQuest;

			// Token: 0x040060E7 RID: 24807
			public SimpleAnimation Btn_EtceteraQuestAnim;

			// Token: 0x040060E8 RID: 24808
			public MarkLockCtrl markLockEtceteraQuest;

			// Token: 0x040060E9 RID: 24809
			public PguiButtonCtrl Btn_AssistantEdit;

			// Token: 0x040060EA RID: 24810
			public MarkLockCtrl markLockAssistantEdit;

			// Token: 0x040060EB RID: 24811
			public SelAssistantCtrl selAssistantCtrl;

			// Token: 0x0200123B RID: 4667
			public class Campaign
			{
				// Token: 0x06005834 RID: 22580 RVA: 0x0025AA78 File Offset: 0x00258C78
				public Campaign(Transform baseTr)
				{
					this.go = baseTr.Find("Campaign").gameObject;
					this.text = baseTr.Find("Campaign/Popup_Campaign_Cmn/Txt_Campaign").GetComponent<PguiTextCtrl>();
					Transform transform = this.go.transform.Find("Popup_Campaign_Doujou");
					if (transform != null)
					{
						this.dojoText = transform.Find("Txt_Campaign").GetComponent<PguiTextCtrl>();
					}
					this.count = 0;
				}

				// Token: 0x06005835 RID: 22581 RVA: 0x0025AAF4 File Offset: 0x00258CF4
				public void ResetCampaign()
				{
					SimpleAnimation componentInChildren = this.go.GetComponentInChildren<SimpleAnimation>();
					if (componentInChildren != null)
					{
						componentInChildren.ExStop(true);
					}
				}

				// Token: 0x06005836 RID: 22582 RVA: 0x0025AB20 File Offset: 0x00258D20
				public void DispCampaign(List<string> msgList)
				{
					string text = msgList.Find((string item) => item.Equals(QuestUtil.CampaignInfo.DojoText));
					if (text != null && this.dojoText == null)
					{
						msgList.Remove(QuestUtil.CampaignInfo.DojoText);
					}
					bool flag;
					if (msgList.Count <= 0)
					{
						flag = msgList.Exists((string item) => item.Equals(QuestUtil.CampaignInfo.DojoText));
					}
					else
					{
						flag = true;
					}
					bool flag2 = flag;
					this.go.SetActive(flag2);
					if (flag2)
					{
						SimpleAnimation componentInChildren = this.go.GetComponentInChildren<SimpleAnimation>();
						if (this.count >= msgList.Count)
						{
							this.count = msgList.Count - 1;
							componentInChildren.ExStop(true);
						}
						if (this.dojoText == null)
						{
							this.text.transform.parent.gameObject.SetActive(true);
							this.text.text = msgList[this.count];
							if (!componentInChildren.ExIsPlaying())
							{
								componentInChildren.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, delegate
								{
									this.count++;
									if (this.count >= msgList.Count)
									{
										this.count = 0;
									}
									this.text.text = msgList[this.count];
								});
								return;
							}
						}
						else if (this.dojoText != null)
						{
							this.text.transform.parent.gameObject.SetActive(false);
							this.dojoText.transform.parent.gameObject.SetActive(text != null);
							this.dojoText.text = msgList[this.count];
							if (!componentInChildren.ExIsPlaying())
							{
								componentInChildren.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, delegate
								{
									this.count++;
									if (this.count >= msgList.Count)
									{
										this.count = 0;
									}
									this.dojoText.text = msgList[this.count];
								});
							}
						}
					}
				}

				// Token: 0x040063CB RID: 25547
				public GameObject go;

				// Token: 0x040063CC RID: 25548
				public PguiTextCtrl text;

				// Token: 0x040063CD RID: 25549
				public PguiTextCtrl dojoText;

				// Token: 0x040063CE RID: 25550
				public int count;
			}

			// Token: 0x0200123C RID: 4668
			public class EventParts
			{
				// Token: 0x040063CF RID: 25551
				public PguiRawImageCtrl bannerImage;

				// Token: 0x040063D0 RID: 25552
				public PguiTextCtrl bannerText;

				// Token: 0x040063D1 RID: 25553
				public PguiImageCtrl Mark_EventOpen;

				// Token: 0x040063D2 RID: 25554
				public PguiImageCtrl Mark_EventBefore;
			}

			// Token: 0x0200123D RID: 4669
			public class StoryQuestParts
			{
				// Token: 0x06005838 RID: 22584 RVA: 0x0025AD00 File Offset: 0x00258F00
				public StoryQuestParts(Transform baseTr)
				{
					this.Btn_StoryQuest = baseTr.Find("Btn_StoryQuest/Base").GetComponent<PguiButtonCtrl>();
					Transform transform = this.Btn_StoryQuest.transform.Find("BaseImage");
					this.StoryQuest_Mark_New = transform.Find("Mark_New").GetComponent<PguiNestPrefab>();
					this.CampaignStoryQuest = new SceneQuest.GUI.QuestTop.Campaign(this.Btn_StoryQuest.transform);
					this.Btn_StoryQuestAnim = baseTr.Find("Btn_StoryQuest").GetComponent<SimpleAnimation>();
					this.StoryPhoto_Anim = transform.Find("StoryPhoto_Anim").GetComponent<SimpleAnimation>();
					this.Texture_StoryPhoto = transform.Find("StoryPhoto_Anim/Texture_StoryPhoto").GetComponent<PguiRawImageCtrl>();
					this.Fint_Story = transform.Find("Fint_Story").GetComponent<PguiImageCtrl>();
					this.Btn_StorySelectL = transform.Find("Btn_StorySelectL").GetComponent<PguiButtonCtrl>();
					this.Btn_StorySelectL_New = this.Btn_StorySelectL.transform.Find("Cmn_Mark_New").gameObject;
					this.Btn_StorySelectR = transform.Find("Btn_StorySelectR").GetComponent<PguiButtonCtrl>();
					this.Btn_StorySelectR_New = this.Btn_StorySelectR.transform.Find("Cmn_Mark_New").gameObject;
					this.PageDots = new List<SceneQuest.GUI.QuestTop.StoryQuestParts.PageDot>
					{
						new SceneQuest.GUI.QuestTop.StoryQuestParts.PageDot
						{
							pageDot = transform.Find("PageDot/Page01").GetComponent<PguiImageCtrl>(),
							category = QuestStaticChapter.Category.STORY
						},
						new SceneQuest.GUI.QuestTop.StoryQuestParts.PageDot
						{
							pageDot = transform.Find("PageDot/Page02").GetComponent<PguiImageCtrl>(),
							category = QuestStaticChapter.Category.CELLVAL
						},
						new SceneQuest.GUI.QuestTop.StoryQuestParts.PageDot
						{
							pageDot = transform.Find("PageDot/Page03").GetComponent<PguiImageCtrl>(),
							category = QuestStaticChapter.Category.STORY2
						},
						new SceneQuest.GUI.QuestTop.StoryQuestParts.PageDot
						{
							pageDot = transform.Find("PageDot/Page04").GetComponent<PguiImageCtrl>(),
							category = QuestStaticChapter.Category.STORY3
						},
						new SceneQuest.GUI.QuestTop.StoryQuestParts.PageDot
						{
							pageDot = transform.Find("PageDot/Page05").GetComponent<PguiImageCtrl>(),
							category = QuestStaticChapter.Category.INVALID
						}
					};
					this.markLock = this.Btn_StoryQuest.transform.Find("Mark_Lock").GetComponent<MarkLockCtrl>();
				}

				// Token: 0x06005839 RID: 22585 RVA: 0x0025AF28 File Offset: 0x00259128
				public void Setup(int currentIndex)
				{
					this.Btn_StorySelectL.SetSoundType(this.Btn_StorySelectL.ActEnable ? PguiButtonCtrl.SoundType.DEFAULT : PguiButtonCtrl.SoundType.INVALID);
					this.Btn_StorySelectR.SetSoundType(this.Btn_StorySelectR.ActEnable ? PguiButtonCtrl.SoundType.DEFAULT : PguiButtonCtrl.SoundType.INVALID);
					int num = 1;
					int mainStorySize = this.GetMainStorySize();
					if (QuestUtil.IsUnLockInformationCellvalQuest(TimeManager.Now) || QuestUtil.IsUnLockInformationMainStory2(TimeManager.Now) || QuestUtil.IsUnLockInformationMainStory3(TimeManager.Now))
					{
						this.Btn_StorySelectL.SetActEnable(currentIndex > 0, true, false);
						this.Btn_StorySelectR.SetActEnable(currentIndex < mainStorySize - 1, true, false);
						for (int i = 0; i < mainStorySize; i++)
						{
							this.PageDots[i].pageDot.GetComponent<PguiReplaceSpriteCtrl>().Replace((currentIndex == i) ? 1 : 0);
						}
						switch (currentIndex)
						{
						case 0:
							num = 1;
							break;
						case 1:
							num = 2;
							break;
						case 2:
							num = 3;
							break;
						case 3:
							num = 4;
							break;
						}
						this.Fint_Story.GetComponent<PguiReplaceSpriteCtrl>().Replace(num);
					}
					else
					{
						this.Btn_StorySelectL.SetActEnable(false, true, false);
						this.Btn_StorySelectR.SetActEnable(false, true, false);
						this.PageDots[0].pageDot.GetComponent<PguiReplaceSpriteCtrl>().Replace(1);
						this.Fint_Story.GetComponent<PguiReplaceSpriteCtrl>().Replace(num);
					}
					PguiAECtrl component = this.Btn_StorySelectR.transform.Find("AEImage_BtnLight").GetComponent<PguiAECtrl>();
					PguiAECtrl component2 = this.Btn_StorySelectL.transform.Find("AEImage_BtnLight").GetComponent<PguiAECtrl>();
					if (DataManager.DmGameStatus.MakeUserFlagData().ReleaseModeFlag.CellvalQuestOpen == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.UnLocked)
					{
						component.gameObject.SetActive(true);
						component2.gameObject.SetActive(true);
						component.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
						component2.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
						return;
					}
					component.gameObject.SetActive(false);
					component2.gameObject.SetActive(false);
				}

				// Token: 0x0600583A RID: 22586 RVA: 0x0025B108 File Offset: 0x00259308
				public int GetMainStorySize()
				{
					foreach (SceneQuest.GUI.QuestTop.StoryQuestParts.PageDot pageDot in this.PageDots)
					{
						pageDot.pageDot.gameObject.SetActive(false);
					}
					this.PageDots[0].pageDot.gameObject.SetActive(true);
					this.PageDots[1].pageDot.gameObject.SetActive(QuestUtil.IsUnLockInformationCellvalQuest(TimeManager.Now));
					this.PageDots[2].pageDot.gameObject.SetActive(QuestUtil.IsUnLockInformationMainStory2(TimeManager.Now));
					this.PageDots[3].pageDot.gameObject.SetActive(QuestUtil.IsUnLockInformationMainStory3(TimeManager.Now));
					return this.PageDots.FindAll((SceneQuest.GUI.QuestTop.StoryQuestParts.PageDot item) => item.pageDot.gameObject.activeSelf).Count;
				}

				// Token: 0x0600583B RID: 22587 RVA: 0x0025B220 File Offset: 0x00259420
				public List<QuestStaticChapter.Category> GetBtnStorySelectLNewList(int currentIndex)
				{
					List<QuestStaticChapter.Category> list = new List<QuestStaticChapter.Category>();
					foreach (SceneQuest.GUI.QuestTop.StoryQuestParts.PageDot pageDot in this.PageDots)
					{
						list.Add(pageDot.category);
					}
					return SceneQuest.GetBtnLeftCategoryList(currentIndex, list);
				}

				// Token: 0x0600583C RID: 22588 RVA: 0x0025B288 File Offset: 0x00259488
				public List<QuestStaticChapter.Category> GetBtnStorySelectRNewList(int currentIndex)
				{
					List<QuestStaticChapter.Category> list = new List<QuestStaticChapter.Category>();
					foreach (SceneQuest.GUI.QuestTop.StoryQuestParts.PageDot pageDot in this.PageDots)
					{
						list.Add(pageDot.category);
					}
					return SceneQuest.GetBtnRightCategoryList(currentIndex, list);
				}

				// Token: 0x0600583D RID: 22589 RVA: 0x0025B2F0 File Offset: 0x002594F0
				public void SetActiveMarkNew(int count)
				{
					bool flag = SceneQuest.SetDispNew(DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.STORY));
					bool flag2 = SceneQuest.SetDispNew(DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.CELLVAL));
					bool flag3 = SceneQuest.SetDispNew(DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.STORY2));
					bool flag4 = SceneQuest.SetDispNew(DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.STORY3));
					this.StoryQuest_Mark_New.InitForce();
					if (SceneQuest.IsMainStoryPart1(count))
					{
						this.StoryQuest_Mark_New.gameObject.SetActive(flag);
						return;
					}
					if (SceneQuest.IsMainStoryPart1_5(count))
					{
						this.StoryQuest_Mark_New.gameObject.SetActive(flag2);
						return;
					}
					if (SceneQuest.IsMainStoryPart2(count))
					{
						this.StoryQuest_Mark_New.gameObject.SetActive(flag3);
						return;
					}
					if (SceneQuest.IsMainStoryPart3(count))
					{
						this.StoryQuest_Mark_New.gameObject.SetActive(flag4);
					}
				}

				// Token: 0x040063D3 RID: 25555
				public PguiButtonCtrl Btn_StoryQuest;

				// Token: 0x040063D4 RID: 25556
				public PguiNestPrefab StoryQuest_Mark_New;

				// Token: 0x040063D5 RID: 25557
				public SceneQuest.GUI.QuestTop.Campaign CampaignStoryQuest;

				// Token: 0x040063D6 RID: 25558
				public SimpleAnimation Btn_StoryQuestAnim;

				// Token: 0x040063D7 RID: 25559
				public SimpleAnimation StoryPhoto_Anim;

				// Token: 0x040063D8 RID: 25560
				public PguiImageCtrl Fint_Story;

				// Token: 0x040063D9 RID: 25561
				public PguiButtonCtrl Btn_StorySelectR;

				// Token: 0x040063DA RID: 25562
				public PguiButtonCtrl Btn_StorySelectL;

				// Token: 0x040063DB RID: 25563
				public GameObject Btn_StorySelectR_New;

				// Token: 0x040063DC RID: 25564
				public GameObject Btn_StorySelectL_New;

				// Token: 0x040063DD RID: 25565
				private List<SceneQuest.GUI.QuestTop.StoryQuestParts.PageDot> PageDots;

				// Token: 0x040063DE RID: 25566
				public PguiRawImageCtrl Texture_StoryPhoto;

				// Token: 0x040063DF RID: 25567
				public MarkLockCtrl markLock;

				// Token: 0x02001253 RID: 4691
				private class PageDot
				{
					// Token: 0x0400645A RID: 25690
					public PguiImageCtrl pageDot;

					// Token: 0x0400645B RID: 25691
					public QuestStaticChapter.Category category;
				}
			}
		}

		// Token: 0x020011AF RID: 4527
		public class ChapterSelect
		{
			// Token: 0x17000CFE RID: 3326
			// (get) Token: 0x060056DE RID: 22238 RVA: 0x002545B7 File Offset: 0x002527B7
			// (set) Token: 0x060056DD RID: 22237 RVA: 0x002545AE File Offset: 0x002527AE
			public float DefaultScrollbarHeight { get; private set; }

			// Token: 0x060056DF RID: 22239 RVA: 0x002545C0 File Offset: 0x002527C0
			public ChapterSelect(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.OffsetGameObject = baseTr.Find("Right/OffsetGameObject").gameObject;
				this.Btn_SortFilterOnOff = this.OffsetGameObject.transform.Find("Btn_SortFilterOnOff").GetComponent<PguiButtonCtrl>();
				this.Btn_Info = this.OffsetGameObject.transform.Find("Btn_Info").GetComponent<PguiButtonCtrl>();
				this.questButtonGroup = this.OffsetGameObject.transform.Find("ScrollView/Viewport/Content/Group").GetComponent<QuestButtonGroupCtrl>();
				this.scrollbarTr = this.OffsetGameObject.transform.Find("ScrollView/Scrollbar_Vertical").transform as RectTransform;
				this.DefaultScrollbarHeight = this.scrollbarTr.sizeDelta.y;
				this.Btn_Yaji_Left = baseTr.Find("LeftBtn/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
				this.Btn_Yaji_Right = baseTr.Find("RightBtn/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
				this.Btn_Sel_Difficult = baseTr.Find("Right/Btn_Sel_Difficult").GetComponent<PguiButtonCtrl>();
				this.campaignInfo = new QuestUtil.CampaignInfo(baseTr.Find("Right/Campaign"));
				this.markLock = this.Btn_Sel_Difficult.transform.Find("BaseImage/Mark_Lock").GetComponent<MarkLockCtrl>();
				this.Btn_Mission = baseTr.Find("Right/Btn_Mission").GetComponent<PguiButtonCtrl>();
				this.Btn_Mission.transform.Find("BaseImage/Mark_New").gameObject.SetActive(false);
				this.Btn_Mission.gameObject.SetActive(false);
				this.Txt_Mission_Num = this.Btn_Mission.transform.Find("BaseImage/Badges/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
				baseTr.Find("Right/OffsetGameObject/ScrollView").GetComponent<ScrollRect>().scrollSensitivity = ScrollParamDefine.Quest;
				this.parts = new List<SceneQuest.GUI.ChapterSelect.Parts>
				{
					new SceneQuest.GUI.ChapterSelect.Parts(baseTr.Find("Left/CharSerif")),
					new SceneQuest.GUI.ChapterSelect.Parts(baseTr.Find("Left/CharSerif_Cellvall")),
					new SceneQuest.GUI.ChapterSelect.Parts(baseTr.Find("Left/CharSerif02")),
					new SceneQuest.GUI.ChapterSelect.Parts(baseTr.Find("Left/CharSerif03"))
				};
			}

			// Token: 0x060056E0 RID: 22240 RVA: 0x002547EC File Offset: 0x002529EC
			public void InactiveParts()
			{
				foreach (SceneQuest.GUI.ChapterSelect.Parts parts in this.parts)
				{
					parts.baseObj.SetActive(false);
				}
			}

			// Token: 0x060056E1 RID: 22241 RVA: 0x00254844 File Offset: 0x00252A44
			public void UpdateCampaignInfoCategory(QuestStaticChapter.Category category, int chapterId)
			{
				List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(category, chapterId));
				List<string> list2 = new List<string>(QuestUtil.GetCampaignTimeList(category, chapterId));
				this.campaignInfo.DispCampaign(list, list2);
			}

			// Token: 0x060056E2 RID: 22242 RVA: 0x00254878 File Offset: 0x00252A78
			public void ResetCampaignInfoCategory()
			{
				this.campaignInfo.ResetCampaign();
			}

			// Token: 0x060056E3 RID: 22243 RVA: 0x00254885 File Offset: 0x00252A85
			public void SetupOffset(Vector3 pos)
			{
				this.OffsetGameObject.transform.localPosition = pos;
				this.scrollbarTr.sizeDelta = new Vector2(this.scrollbarTr.sizeDelta.x, this.DefaultScrollbarHeight + pos.y);
			}

			// Token: 0x040060EC RID: 24812
			public GameObject baseObj;

			// Token: 0x040060ED RID: 24813
			public PguiButtonCtrl Btn_SortFilterOnOff;

			// Token: 0x040060EE RID: 24814
			public PguiButtonCtrl Btn_Info;

			// Token: 0x040060EF RID: 24815
			public QuestButtonGroupCtrl questButtonGroup;

			// Token: 0x040060F0 RID: 24816
			public RenderTextureChara renderTextureChara;

			// Token: 0x040060F1 RID: 24817
			public RenderTextureChara renderTextureChara2;

			// Token: 0x040060F2 RID: 24818
			public PguiButtonCtrl Btn_Yaji_Left;

			// Token: 0x040060F3 RID: 24819
			public PguiButtonCtrl Btn_Yaji_Right;

			// Token: 0x040060F4 RID: 24820
			public PguiButtonCtrl Btn_Sel_Difficult;

			// Token: 0x040060F5 RID: 24821
			public QuestUtil.CampaignInfo campaignInfo;

			// Token: 0x040060F6 RID: 24822
			public MarkLockCtrl markLock;

			// Token: 0x040060F7 RID: 24823
			public PguiButtonCtrl Btn_Mission;

			// Token: 0x040060F8 RID: 24824
			public PguiTextCtrl Txt_Mission_Num;

			// Token: 0x040060F9 RID: 24825
			public GameObject OffsetGameObject;

			// Token: 0x040060FA RID: 24826
			public RectTransform scrollbarTr;

			// Token: 0x040060FB RID: 24827
			public List<SceneQuest.GUI.ChapterSelect.Parts> parts;

			// Token: 0x0200123E RID: 4670
			public class Parts
			{
				// Token: 0x0600583E RID: 22590 RVA: 0x0025B3B8 File Offset: 0x002595B8
				public Parts(Transform baseTr)
				{
					this.baseObj = baseTr.gameObject;
					this.Txt_Serif = baseTr.Find("Txt_Serif").GetComponent<PguiTextCtrl>();
					this.Txt_Serif.text = "";
					this.Txt_CharaName = baseTr.Find("NameBase/Txt_CharaName").GetComponent<PguiTextCtrl>();
					this.Txt_CharaName.text = "";
				}

				// Token: 0x040063E0 RID: 25568
				public GameObject baseObj;

				// Token: 0x040063E1 RID: 25569
				public PguiTextCtrl Txt_Serif;

				// Token: 0x040063E2 RID: 25570
				public PguiTextCtrl Txt_CharaName;
			}
		}

		// Token: 0x020011B0 RID: 4528
		public class LocationInfo
		{
			// Token: 0x060056E4 RID: 22244 RVA: 0x002548C8 File Offset: 0x00252AC8
			public LocationInfo(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.All = baseTr.Find("All").gameObject;
				this.CharaNameTr = baseTr.Find("All/CharaName");
				this.Txt_CharaName = this.CharaNameTr.Find("Txt_CharaName").GetComponent<PguiTextCtrl>();
				this.Txt_CharaName_ReTr = this.Txt_CharaName.GetComponent<RectTransform>();
				this.Txt_CharaName_Initial_Pos = this.Txt_CharaName_ReTr.anchoredPosition;
				this.Txt_CharaName_Initial_Size = this.Txt_CharaName_ReTr.sizeDelta;
				this.Icon_Atr = this.CharaNameTr.Find("Icon_Atr").GetComponent<PguiImageCtrl>();
				this.Icon_SubAtr = this.CharaNameTr.Find("Icon_SubAtr").GetComponent<PguiImageCtrl>();
				this.Btn_MoreInfo = baseTr.Find("All/Btn_MoreInfo").GetComponent<PguiButtonCtrl>();
				this.CharaNameTr.gameObject.SetActive(false);
				this.Mark_Hard = baseTr.Find("All/Mark_Hard").GetComponent<PguiImageCtrl>();
				this.location = new List<SceneQuest.GUI.LocationInfo.Location>
				{
					new SceneQuest.GUI.LocationInfo.Location(baseTr.Find("All/Txt_Location")),
					new SceneQuest.GUI.LocationInfo.Location(baseTr.Find("All/Txt_Location_Cellvall")),
					new SceneQuest.GUI.LocationInfo.Location(baseTr.Find("All/Txt_Location02")),
					new SceneQuest.GUI.LocationInfo.Location(baseTr.Find("All/Txt_Location03"))
				};
			}

			// Token: 0x040060FD RID: 24829
			public GameObject baseObj;

			// Token: 0x040060FE RID: 24830
			public Transform CharaNameTr;

			// Token: 0x040060FF RID: 24831
			public PguiTextCtrl Txt_CharaName;

			// Token: 0x04006100 RID: 24832
			public RectTransform Txt_CharaName_ReTr;

			// Token: 0x04006101 RID: 24833
			public Vector2 Txt_CharaName_Initial_Pos;

			// Token: 0x04006102 RID: 24834
			public Vector2 Txt_CharaName_Initial_Size;

			// Token: 0x04006103 RID: 24835
			public PguiImageCtrl Icon_Atr;

			// Token: 0x04006104 RID: 24836
			public PguiImageCtrl Icon_SubAtr;

			// Token: 0x04006105 RID: 24837
			public PguiButtonCtrl Btn_MoreInfo;

			// Token: 0x04006106 RID: 24838
			public PguiImageCtrl Mark_Hard;

			// Token: 0x04006107 RID: 24839
			public List<SceneQuest.GUI.LocationInfo.Location> location;

			// Token: 0x04006108 RID: 24840
			public GameObject All;

			// Token: 0x0200123F RID: 4671
			public class Location
			{
				// Token: 0x0600583F RID: 22591 RVA: 0x0025B423 File Offset: 0x00259623
				public Location(Transform baseTr)
				{
					this.baseObj = baseTr.gameObject;
					this.Txt_Location = baseTr.GetComponent<PguiTextCtrl>();
					this.Txt_Location.text = "";
				}

				// Token: 0x040063E3 RID: 25571
				public GameObject baseObj;

				// Token: 0x040063E4 RID: 25572
				public PguiTextCtrl Txt_Location;
			}
		}

		// Token: 0x020011B1 RID: 4529
		public class LocationEvent
		{
			// Token: 0x060056E5 RID: 22245 RVA: 0x00254A34 File Offset: 0x00252C34
			public LocationEvent(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Btn_ShopEvent = baseTr.Find("ItemInfo/Btn_ShopEvent").GetComponent<PguiButtonCtrl>();
				this.Banner = baseTr.Find("Banner").GetComponent<PguiRawImageCtrl>();
				this.itemOwnBases = new List<SceneQuest.GUI.LocationEvent.ItemOwnBase>
				{
					new SceneQuest.GUI.LocationEvent.ItemOwnBase(baseTr.Find("ItemInfo/Grid/ItemOwnBase01")),
					new SceneQuest.GUI.LocationEvent.ItemOwnBase(baseTr.Find("ItemInfo/Grid/ItemOwnBase02"))
				};
				this.Txt_Serif = baseTr.Find("CharSerif/Txt_Serif").GetComponent<PguiTextCtrl>();
				this.Txt_Serif.text = "";
				this.Txt_CharaName = baseTr.Find("CharSerif/NameBase/Txt_CharaName").GetComponent<PguiTextCtrl>();
				this.Txt_CharaName.text = "";
			}

			// Token: 0x04006109 RID: 24841
			public GameObject baseObj;

			// Token: 0x0400610A RID: 24842
			public PguiButtonCtrl Btn_ShopEvent;

			// Token: 0x0400610B RID: 24843
			public PguiRawImageCtrl Banner;

			// Token: 0x0400610C RID: 24844
			public PguiTextCtrl Txt_CharaName;

			// Token: 0x0400610D RID: 24845
			public PguiTextCtrl Txt_Serif;

			// Token: 0x0400610E RID: 24846
			public List<SceneQuest.GUI.LocationEvent.ItemOwnBase> itemOwnBases;

			// Token: 0x02001240 RID: 4672
			public class ItemOwnBase
			{
				// Token: 0x06005840 RID: 22592 RVA: 0x0025B453 File Offset: 0x00259653
				public ItemOwnBase(Transform baseTr)
				{
					this.baseObj = baseTr.gameObject;
					this.Icon_Img = baseTr.Find("Icon_Img").GetComponent<PguiRawImageCtrl>();
					this.Num_Own = baseTr.Find("Num_Own").GetComponent<PguiTextCtrl>();
				}

				// Token: 0x040063E5 RID: 25573
				public GameObject baseObj;

				// Token: 0x040063E6 RID: 25574
				public PguiRawImageCtrl Icon_Img;

				// Token: 0x040063E7 RID: 25575
				public PguiTextCtrl Num_Own;
			}
		}

		// Token: 0x020011B2 RID: 4530
		public class CharaGrow
		{
			// Token: 0x060056E6 RID: 22246 RVA: 0x00254B04 File Offset: 0x00252D04
			public CharaGrow(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Btn_Schedule = baseTr.Find("Btn_Schedule").GetComponent<PguiButtonCtrl>();
				this.Icon_Item = new List<IconItemCtrl>();
				for (int i = 0; i < 6; i++)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, baseTr.Find("ItemLoop/Icon_Item" + (i + 1).ToString("D2")));
					this.Icon_Item.Add(gameObject.GetComponent<IconItemCtrl>());
				}
				this.ItemLoop = baseTr.Find("ItemLoop").GetComponent<PguiAECtrl>();
			}

			// Token: 0x0400610F RID: 24847
			public GameObject baseObj;

			// Token: 0x04006110 RID: 24848
			public PguiButtonCtrl Btn_Schedule;

			// Token: 0x04006111 RID: 24849
			public List<IconItemCtrl> Icon_Item;

			// Token: 0x04006112 RID: 24850
			public PguiAECtrl ItemLoop;
		}

		// Token: 0x020011B3 RID: 4531
		public class SideStory
		{
			// Token: 0x060056E7 RID: 22247 RVA: 0x00254BA8 File Offset: 0x00252DA8
			public SideStory(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_Title = baseTr.Find("Base/TitleBase/Txt_Title").GetComponent<PguiTextCtrl>();
				this.Txt_Title.text = "";
				this.Txt_Info = baseTr.Find("Base/Txt_Info").GetComponent<PguiTextCtrl>();
				this.Txt_Info.text = "";
			}

			// Token: 0x04006113 RID: 24851
			public GameObject baseObj;

			// Token: 0x04006114 RID: 24852
			public PguiTextCtrl Txt_Title;

			// Token: 0x04006115 RID: 24853
			public PguiTextCtrl Txt_Info;
		}

		// Token: 0x020011B4 RID: 4532
		public class SetupSwitchSelectorParam
		{
			// Token: 0x04006116 RID: 24854
			public GameObject nextObj;

			// Token: 0x04006117 RID: 24855
			public GameObject currentObj;

			// Token: 0x04006118 RID: 24856
			public bool enableBlur;

			// Token: 0x04006119 RID: 24857
			public bool isMapNeedQuest;

			// Token: 0x0400611A RID: 24858
			public SceneQuest.UpdateCallback updateCb;

			// Token: 0x0400611B RID: 24859
			public SceneQuest.TouchCallback touchRegistCb;

			// Token: 0x0400611C RID: 24860
			public SceneQuest.TouchCallback touchReleaseCb;

			// Token: 0x0400611D RID: 24861
			public SceneQuest.TouchCallback releaseRegistCb;

			// Token: 0x0400611E RID: 24862
			public SceneQuest.TouchCallback releaseReleaseCb;

			// Token: 0x0400611F RID: 24863
			public SceneQuest.TouchCallback startRegistCb;

			// Token: 0x04006120 RID: 24864
			public SceneQuest.TouchCallback startReleaseCb;

			// Token: 0x04006121 RID: 24865
			public SceneQuest.IMapData mapDataGameObj;

			// Token: 0x04006122 RID: 24866
			public GameObject pointSelectObj;

			// Token: 0x04006123 RID: 24867
			public GameObject charaSelectObj;

			// Token: 0x04006124 RID: 24868
			public QuestUtil.SelectData selectData;

			// Token: 0x04006125 RID: 24869
			public UnityAction pointSelectActionCB;

			// Token: 0x04006126 RID: 24870
			public UnityAction charaSelectActionCB;
		}
	}
}
