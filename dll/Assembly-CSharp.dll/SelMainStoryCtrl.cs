using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000178 RID: 376
public class SelMainStoryCtrl : MonoBehaviour
{
	// Token: 0x170003BC RID: 956
	// (get) Token: 0x06001802 RID: 6146 RVA: 0x00127C96 File Offset: 0x00125E96
	// (set) Token: 0x06001803 RID: 6147 RVA: 0x00127C9E File Offset: 0x00125E9E
	public SelMainStoryCtrl.GUI GuiData { get; private set; }

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x06001804 RID: 6148 RVA: 0x00127CA7 File Offset: 0x00125EA7
	public SelMainStoryCtrl.GuiMapData MapData
	{
		get
		{
			SelMainStoryCtrl.GUI guiData = this.GuiData;
			if (guiData == null)
			{
				return null;
			}
			return guiData.mapData;
		}
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x00127CBA File Offset: 0x00125EBA
	public void SetMapBoxObject(GameObject go)
	{
		this.mapBoxObject = go;
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x00127CC3 File Offset: 0x00125EC3
	public static SelMainStoryCtrl.GuiMapData SetupMapGUI(GameObject mapObj, List<GameObject> carObj)
	{
		return new SelMainStoryCtrl.GuiMapData(mapObj.transform, carObj);
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x00127CD1 File Offset: 0x00125ED1
	public void Init(SelMainStoryCtrl.InitParam _initParam, SelMainStoryCtrl.SetupParam _setupParam)
	{
		this.initParam = _initParam;
		this.GuiData = new SelMainStoryCtrl.GUI(this.initParam.prefabPath);
		UnityAction selectObjsCB = this.initParam.selectObjsCB;
		if (selectObjsCB != null)
		{
			selectObjsCB();
		}
		this.Setup(_setupParam);
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x00127D0D File Offset: 0x00125F0D
	public void Setup(SelMainStoryCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		this.TouchMoving = false;
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x00127D1D File Offset: 0x00125F1D
	public void Dest()
	{
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x00127D1F File Offset: 0x00125F1F
	public void Destroy()
	{
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x00127D21 File Offset: 0x00125F21
	public void UpdateDecoration()
	{
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x0600180C RID: 6156 RVA: 0x00127D23 File Offset: 0x00125F23
	// (set) Token: 0x0600180D RID: 6157 RVA: 0x00127D2B File Offset: 0x00125F2B
	public bool TouchMoving { get; private set; }

	// Token: 0x0600180E RID: 6158 RVA: 0x00127D34 File Offset: 0x00125F34
	public bool IsNotNullMapObj()
	{
		return this.GuiData.mapData != null;
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x00127D44 File Offset: 0x00125F44
	public bool IsNotNullMapBaseObj()
	{
		return this.IsNotNullMapObj() && this.GuiData.mapData.baseObj != null;
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x00127D68 File Offset: 0x00125F68
	public void UpdateMapdata(int chapterId, QuestStaticChapter.Category questCategory, UnityAction<Transform> pointTouchCB)
	{
		if (!this.IsNotNullMapObj())
		{
			return;
		}
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(chapterId);
		if (playableMapIdList.Count <= 0)
		{
			return;
		}
		playableMapIdList.Sort((int a, int b) => a - b);
		List<QuestStaticMap> mapDataList = DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId].mapDataList;
		this.GuiData.mapData.SetupMapData(DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId]);
		Vector2 vector = new Vector2(this.GetMapOffsetPosX(this.initParam.getSelectDataCB().questCategory, true), 0f);
		Vector2 vector2 = new Vector2(this.GetMapOffsetPosX(this.initParam.getSelectDataCB().questCategory, false), 0f);
		this.GuiData.mapData.mapObj.GetComponent<RectTransform>().anchoredPosition = vector;
		this.GuiData.mapData.bgObj.GetComponent<RectTransform>().anchoredPosition = vector2;
		QuestWeather questWeather;
		if (playableMapIdList.Count == mapDataList.Count)
		{
			bool flag = true;
			foreach (QuestStaticQuestGroup questStaticQuestGroup in mapDataList[mapDataList.Count - 1].questGroupList)
			{
				foreach (QuestStaticQuestOne questStaticQuestOne in questStaticQuestGroup.questOneList)
				{
					QuestDynamicQuestOne questDynamicQuestOne = null;
					if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questStaticQuestOne.questId))
					{
						questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[questStaticQuestOne.questId];
					}
					QuestOneStatus questOneStatus = ((questDynamicQuestOne != null) ? questDynamicQuestOne.status : QuestOneStatus.INVALID);
					flag &= questOneStatus == QuestOneStatus.COMPLETE || questOneStatus == QuestOneStatus.CLEAR;
				}
			}
			if (flag)
			{
				questWeather = DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId].WeatherType;
			}
			else
			{
				questWeather = mapDataList[playableMapIdList.Count - 1].WeatherType;
			}
		}
		else
		{
			questWeather = mapDataList[playableMapIdList.Count - 1].WeatherType;
		}
		this.GuiData.mapData.SetWeatherType(questWeather);
		this.SetupMapData(chapterId, questCategory, pointTouchCB);
		string text = QuestUtil.MapTexturePrefix;
		string text2 = QuestUtil.MapTextureFolder;
		if (SceneQuest.IsMainStoryPart1_5(questCategory))
		{
			text = QuestUtil.MapTextureCellvalPrefix;
			text2 = QuestUtil.MapTextureCellvalFolder;
		}
		else if (SceneQuest.IsMainStoryPart2(questCategory))
		{
			text = QuestUtil.Map2TexturePrefix;
		}
		else if (SceneQuest.IsMainStoryPart3(questCategory))
		{
			text = QuestUtil.Map3TexturePrefix;
		}
		int num = SceneQuest.GetChapterDataByCategory(questCategory, QuestUtil.IsHardMode(this.initParam.getSelectDataCB()) ? 1 : 0).FindIndex((KeyValuePair<int, QuestStaticChapter> item) => item.Key == chapterId);
		int num2 = num % 3;
		QuestUtil.UpdateBG(this.GuiData.mapData.bgObj.transform, num, num2, text2, text);
		this.UpdatePointSelect(chapterId);
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x001280C0 File Offset: 0x001262C0
	private void UpdatePointSelect(int chapterId)
	{
		foreach (SelMainStoryCtrl.GuiPointSelect.ChapterInfo chapterInfo in this.GuiData.pointSelect.chapterInfoList)
		{
			chapterInfo.baseObj.SetActive(false);
		}
		QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == chapterId);
		if (SceneQuest.IsMainStoryPart1(questStaticChapter.category))
		{
			this.GuiData.pointSelect.chapterInfoList[0].baseObj.SetActive(true);
			this.GuiData.pointSelect.chapterInfoList[0].Txt_ChapterName.text = PrjUtil.MakeMessage(QuestUtil.TitleMain + "第" + questStaticChapter.chapterName);
			this.GuiData.pointSelect.chapterInfoList[0].Txt_Info.text = questStaticChapter.chapterTitle;
		}
		else if (SceneQuest.IsMainStoryPart1_5(questStaticChapter.category))
		{
			this.GuiData.pointSelect.chapterInfoList[1].baseObj.SetActive(true);
			this.GuiData.pointSelect.chapterInfoList[1].Txt_ChapterName.text = PrjUtil.MakeMessage(QuestUtil.TitleCellval + " 第" + questStaticChapter.chapterName);
			this.GuiData.pointSelect.chapterInfoList[1].Txt_Info.text = questStaticChapter.chapterTitle;
		}
		else if (SceneQuest.IsMainStoryPart2(questStaticChapter.category))
		{
			this.GuiData.pointSelect.chapterInfoList[2].baseObj.SetActive(true);
			this.GuiData.pointSelect.chapterInfoList[2].Txt_ChapterName.text = QuestUtil.TitleMain2 + " 第" + questStaticChapter.chapterName;
			this.GuiData.pointSelect.chapterInfoList[2].Txt_Info.text = questStaticChapter.chapterTitle;
		}
		else if (SceneQuest.IsMainStoryPart3(questStaticChapter.category))
		{
			this.GuiData.pointSelect.chapterInfoList[3].baseObj.SetActive(true);
			this.GuiData.pointSelect.chapterInfoList[3].Txt_ChapterName.text = QuestUtil.TitleMain3 + " 第" + questStaticChapter.chapterName;
			this.GuiData.pointSelect.chapterInfoList[3].Txt_Info.text = questStaticChapter.chapterTitle;
		}
		UnityAction reqRewardsCB = this.initParam.reqRewardsCB;
		if (reqRewardsCB == null)
		{
			return;
		}
		reqRewardsCB();
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x001283AC File Offset: 0x001265AC
	public void UpdatePointSelect()
	{
		if (this.GuiData.pointSelect.baseObj.activeSelf)
		{
			this.GuiData.pointSelect.Mark_Hard.gameObject.SetActive(QuestUtil.IsHardMode(this.initParam.getSelectDataCB()));
			QuestOneStatus questOneStatus = QuestOneStatus.INVALID;
			QuestOnePackData questOnePackDataForReleaseIdStoryHardMode = SceneQuest.GetQuestOnePackDataForReleaseIdStoryHardMode(this.initParam.getSelectDataCB().questCategory);
			string text;
			if (questOnePackDataForReleaseIdStoryHardMode != null)
			{
				string mainStoryName = SceneQuest.GetMainStoryName(questOnePackDataForReleaseIdStoryHardMode.questChapter.category, true);
				text = mainStoryName + ((mainStoryName != "") ? "\n" : "") + questOnePackDataForReleaseIdStoryHardMode.questChapter.chapterName + questOnePackDataForReleaseIdStoryHardMode.questGroup.titleName + PrjUtil.MakeMessage("クリア");
				if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questOnePackDataForReleaseIdStoryHardMode.questOne.questId))
				{
					questOneStatus = DataManager.DmQuest.QuestDynamicData.oneDataMap[questOnePackDataForReleaseIdStoryHardMode.questOne.questId].status;
				}
			}
			else
			{
				text = "クエスト情報がありません";
			}
			MarkLockCtrl markLock = this.GuiData.pointSelect.markLock;
			MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();
			setupParam.updateConditionCallback = () => QuestUtil.IsHardMode(this.initParam.getSelectDataCB());
			setupParam.releaseFlag = questOneStatus != QuestOneStatus.INVALID && questOneStatus != QuestOneStatus.NEW;
			setupParam.tagetObject = this.GuiData.pointSelect.Btn_Sel_Difficult.gameObject;
			setupParam.text = text;
			setupParam.updateUserFlagDataCallback = delegate
			{
			};
			markLock.Setup(setupParam, true);
		}
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x00128558 File Offset: 0x00126758
	public void UpdateMission()
	{
		int userClearMissionNum = DataManager.DmMission.GetUserClearMissionNum();
		this.GuiData.pointSelect.Txt_Mission_Num.transform.parent.transform.parent.gameObject.SetActive(userClearMissionNum > 0);
		this.GuiData.pointSelect.Txt_Mission_Num.text = userClearMissionNum.ToString();
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x001285C0 File Offset: 0x001267C0
	public static float GetMapOffsetPosX(QuestStaticChapter.Category category, GameObject mapBoxObject, SelMainStoryCtrl.GuiMapData guiMapData, bool isMap)
	{
		RectTransform rectTransform = mapBoxObject.transform as RectTransform;
		float leftSidePosX = QuestUtil.GetLeftSidePosX(mapBoxObject);
		if (SceneQuest.IsMainStoryPart1(category))
		{
			if (QuestUtil.HasUnsafeAreaLeft(mapBoxObject, leftSidePosX))
			{
				if (-leftSidePosX > QuestUtil.MAP_MASK_IMAGE_WIDTH || (rectTransform.offsetMax.x >= QuestUtil.MAP_MASK_IMAGE_WIDTH && -leftSidePosX < QuestUtil.MAP_MASK_IMAGE_WIDTH))
				{
					return QuestUtil.MAP_MASK_IMAGE_WIDTH + leftSidePosX;
				}
				return 0f;
			}
			else
			{
				if (rectTransform.offsetMax.x >= QuestUtil.MAP_MASK_IMAGE_WIDTH)
				{
					return QuestUtil.MAP_MASK_IMAGE_WIDTH;
				}
				if (rectTransform.offsetMax.x <= 0f)
				{
					return 0f;
				}
				return leftSidePosX;
			}
		}
		else
		{
			if (SceneQuest.IsMainStoryPart1_5(category))
			{
				float num = -((QuestUtil.DEFAULT_BG_WIDTH - rectTransform.rect.width) * 0.5f + guiMapData.bgObj.transform.Find("Tex_Bg").localPosition.x);
				float num2 = 0f;
				if (-leftSidePosX > QuestUtil.MAP_MASK_IMAGE_WIDTH - num + SelMainStoryCtrl.CellvalMapOffsetPosX || (SafeAreaScaler.IsLongDevice() && -leftSidePosX < QuestUtil.MAP_MASK_IMAGE_WIDTH - num + SelMainStoryCtrl.CellvalMapOffsetPosX))
				{
					num2 = -leftSidePosX - (QuestUtil.MAP_MASK_IMAGE_WIDTH - num + SelMainStoryCtrl.CellvalMapOffsetPosX);
				}
				else if (QuestUtil.GetRightSidePosX(mapBoxObject) > QuestUtil.DEFAULT_BG_WIDTH - QuestUtil.MAP_MASK_IMAGE_WIDTH + num - SelMainStoryCtrl.CellvalMapOffsetPosX)
				{
					num2 = -(QuestUtil.GetRightSidePosX(mapBoxObject) - (QuestUtil.DEFAULT_BG_WIDTH - QuestUtil.MAP_MASK_IMAGE_WIDTH + num - SelMainStoryCtrl.CellvalMapOffsetPosX));
				}
				return num - num2;
			}
			float num3 = -((QuestUtil.DEFAULT_BG_WIDTH - rectTransform.rect.width) * 0.5f - QuestUtil.MAP_MASK_IMAGE_WIDTH);
			if (isMap && num3 < 0f)
			{
				num3 = 0f;
			}
			float num4 = 0f;
			if (-leftSidePosX > QuestUtil.MAP_MASK_IMAGE_WIDTH - num3 || (SafeAreaScaler.IsLongDevice() && -leftSidePosX < QuestUtil.MAP_MASK_IMAGE_WIDTH - num3))
			{
				num4 = -leftSidePosX - (QuestUtil.MAP_MASK_IMAGE_WIDTH - num3);
			}
			else if (QuestUtil.GetRightSidePosX(mapBoxObject) > QuestUtil.DEFAULT_BG_WIDTH - QuestUtil.MAP_MASK_IMAGE_WIDTH + num3)
			{
				num4 = -(QuestUtil.GetRightSidePosX(mapBoxObject) - (QuestUtil.DEFAULT_BG_WIDTH - QuestUtil.MAP_MASK_IMAGE_WIDTH + num3));
			}
			return num3 - num4;
		}
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x001287B9 File Offset: 0x001269B9
	public float GetMapOffsetPosX(QuestStaticChapter.Category category, bool isMap)
	{
		return SelMainStoryCtrl.GetMapOffsetPosX(category, this.mapBoxObject, this.GuiData.mapData, isMap);
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x001287D4 File Offset: 0x001269D4
	public static float GetSafeAreaX(QuestStaticChapter.Category category, GameObject mapBoxObject, SelMainStoryCtrl.GuiMapData guiMapData)
	{
		RectTransform rectTransform = mapBoxObject.transform as RectTransform;
		float mapOffsetPosX = SelMainStoryCtrl.GetMapOffsetPosX(category, mapBoxObject, guiMapData, true);
		float num = 0f;
		float num2 = rectTransform.offsetMax.x * 2f - mapOffsetPosX;
		float rightSidePosX = QuestUtil.GetRightSidePosX(mapBoxObject);
		if (QuestUtil.HasUnsafeAreaRight(mapBoxObject, rightSidePosX) || QuestUtil.HasUnsafeAreaLeft(mapBoxObject, QuestUtil.GetLeftSidePosX(mapBoxObject)))
		{
			if (rightSidePosX > num2 + QuestUtil.DEFAULT_SCREEN_WIDTH + QuestUtil.MAP_MASK_IMAGE_WIDTH)
			{
				num = rightSidePosX - (num2 + QuestUtil.DEFAULT_SCREEN_WIDTH + QuestUtil.MAP_MASK_IMAGE_WIDTH);
			}
			else if (rectTransform.offsetMax.x >= QuestUtil.MAP_MASK_IMAGE_WIDTH && rightSidePosX < num2 + QuestUtil.DEFAULT_SCREEN_WIDTH + QuestUtil.MAP_MASK_IMAGE_WIDTH)
			{
				num = rightSidePosX - (num2 + QuestUtil.DEFAULT_SCREEN_WIDTH + QuestUtil.MAP_MASK_IMAGE_WIDTH);
			}
			else
			{
				num = 0f;
			}
		}
		return num2 + num;
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x00128894 File Offset: 0x00126A94
	public static float GetScrollWidth(QuestStaticChapter.Category category, SelMainStoryCtrl.GuiMapData guiMapData)
	{
		return guiMapData.bgObj.transform.Find("Tex_Bg").GetComponent<RectTransform>().sizeDelta.x - QuestUtil.DEFAULT_SCREEN_WIDTH;
	}

	// Token: 0x06001818 RID: 6168 RVA: 0x001288C0 File Offset: 0x00126AC0
	public static QuestCarType GetQuestCarType(int chapterId)
	{
		QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId];
		if (chapterId == 1004 || chapterId == 2004)
		{
			return QuestCarType.FLICKY;
		}
		if (SceneQuest.IsMainStoryPart1_5(questStaticChapter.category))
		{
			return QuestCarType.CELLVAL;
		}
		if (SceneQuest.IsMainStoryPart2(questStaticChapter.category))
		{
			return QuestCarType.HAND;
		}
		if (SceneQuest.IsMainStoryPart3(questStaticChapter.category))
		{
			return QuestCarType.CAMERA;
		}
		return QuestCarType.BUS;
	}

	// Token: 0x06001819 RID: 6169 RVA: 0x00128924 File Offset: 0x00126B24
	public void RemoveTouchEventTriggger()
	{
		for (int i = 0; i < this.GuiData.mapData.mapPointList.Count; i++)
		{
			PrjUtil.RemoveTouchEventTrigger(this.GuiData.mapData.mapPointList[i].pointRtf.gameObject);
		}
	}

	// Token: 0x0600181A RID: 6170 RVA: 0x00128978 File Offset: 0x00126B78
	public static void AdjustMaskPostion(QuestStaticChapter.Category category, SelMainStoryCtrl.GuiMapData guiMapData, GameObject mapBoxObject)
	{
		if (SceneQuest.IsMainStoryPart1_5(category))
		{
			GameObject gameObject = guiMapData.baseObj.transform.Find("Map_Frame").gameObject;
			Vector3 localPosition = gameObject.transform.localPosition;
			gameObject.transform.localPosition = new Vector3(SelMainStoryCtrl.GetMapOffsetPosX(category, mapBoxObject, guiMapData, false) - SelMainStoryCtrl.CellvalMapOffsetPosX, localPosition.y, 0f);
			return;
		}
		GameObject gameObject2 = guiMapData.baseObj.transform.Find("Map_Frame").gameObject;
		Vector3 localPosition2 = gameObject2.transform.localPosition;
		gameObject2.transform.localPosition = new Vector3(SelMainStoryCtrl.GetMapOffsetPosX(category, mapBoxObject, guiMapData, false), localPosition2.y, 0f);
	}

	// Token: 0x0600181B RID: 6171 RVA: 0x00128A28 File Offset: 0x00126C28
	private void SetupMapData(int chapterId, QuestStaticChapter.Category category, UnityAction<Transform> pointTouchCB)
	{
		if (!this.IsNotNullMapObj())
		{
			return;
		}
		SelMainStoryCtrl.AdjustMaskPostion(category, this.GuiData.mapData, this.mapBoxObject);
		this.UpdateMapButtonLR(chapterId);
		SelMainStoryCtrl.<>c__DisplayClass45_0 CS$<>8__locals1 = new SelMainStoryCtrl.<>c__DisplayClass45_0();
		CS$<>8__locals1.playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(chapterId);
		CS$<>8__locals1.mapList = DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId].mapDataList;
		Material material = new Material(Shader.Find("UI/Grayscale"));
		int i;
		int l;
		for (i = 0; i < CS$<>8__locals1.mapList.Count; i = l + 1)
		{
			int num = this.GuiData.mapData.mapPointList.FindIndex((SelMainStoryCtrl.GuiMapData.MapPoint item) => item.GetName() == CS$<>8__locals1.mapList[i].mapObjName);
			if (num < 0)
			{
				num = i;
			}
			QuestStaticMap qsm = CS$<>8__locals1.mapList[i];
			if (i < this.GuiData.mapData.mapPointList.Count)
			{
				SelMainStoryCtrl.GuiMapData.MapPoint mapPoint = this.GuiData.mapData.mapPointList[num];
				mapPoint.pointObj.name = qsm.mapId.ToString();
				mapPoint.pointRtf.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 200f);
				mapPoint.pointRtf.name = qsm.mapId.ToString();
				PrjUtil.AddTouchEventTrigger(mapPoint.pointRtf.gameObject, pointTouchCB);
				mapPoint.Mark_Batch.gameObject.SetActive(false);
				mapPoint.Mark_Batch.m_Image.material = (CS$<>8__locals1.<SetupMapData>g__LocalExistsPlayableMapId|0(qsm) ? null : material);
				mapPoint.Txt_MapName.text = qsm.mapName;
				mapPoint.Txt_MapName.transform.parent.GetComponent<PguiImageCtrl>().m_Image.material = (CS$<>8__locals1.<SetupMapData>g__LocalExistsPlayableMapId|0(qsm) ? null : material);
				mapPoint.SetActiveDayName(qsm.LowStartTimeByGroup >= TimeManager.Now);
				mapPoint.Txt_DayName.text = ((qsm.ReleaseDateFreeText == string.Empty) ? string.Format("{0}月{1}日 公開", qsm.LowStartTimeByGroup.Month, qsm.LowStartTimeByGroup.Day) : qsm.ReleaseDateFreeText);
				PguiReplaceSpriteCtrl componentInParent = mapPoint.Txt_MapName.GetComponentInParent<PguiReplaceSpriteCtrl>();
				if (componentInParent != null)
				{
					componentInParent.Replace(SceneQuest.IsNormalMode(this.initParam.getSelectDifficultCountCB()) ? 1 : 2);
				}
				PguiGradientCtrl component = mapPoint.Txt_MapName.GetComponent<PguiGradientCtrl>();
				if (component != null)
				{
					foreach (Outline outline in mapPoint.Txt_MapName.GetComponents<Outline>())
					{
						if (CS$<>8__locals1.<SetupMapData>g__LocalExistsPlayableMapId|0(qsm))
						{
							outline.effectColor = component.GetOutlineById(SceneQuest.IsNormalMode(this.initParam.getSelectDifficultCountCB()) ? "Normal" : "Hard");
						}
						else
						{
							outline.effectColor = component.GetOutlineById("Disable");
						}
					}
				}
				mapPoint.Tex.m_RawImage.material = (CS$<>8__locals1.<SetupMapData>g__LocalExistsPlayableMapId|0(qsm) ? null : material);
				QuestStaticQuestGroup questStaticQuestGroup = qsm.questGroupList.Find((QuestStaticQuestGroup item) => item.mapId == qsm.mapId);
				int num2 = 0;
				bool flag = false;
				for (int j = 0; j < questStaticQuestGroup.questOneList.Count; j++)
				{
					QuestStaticQuestOne questStaticQuestOne = questStaticQuestGroup.questOneList[j];
					QuestDynamicQuestOne questDynamicQuestOne = null;
					if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questStaticQuestOne.questId))
					{
						questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[questStaticQuestOne.questId];
					}
					QuestOneStatus questOneStatus = ((questDynamicQuestOne != null) ? questDynamicQuestOne.status : QuestOneStatus.INVALID);
					if (questOneStatus == QuestOneStatus.COMPLETE)
					{
						num2++;
					}
					if (SceneQuest.IsMainStoryPart2(category) && questDynamicQuestOne != null && questOneStatus == QuestOneStatus.NEW)
					{
						flag = true;
					}
				}
				mapPoint.Mark_Complete.SetActive(num2 >= questStaticQuestGroup.questOneList.Count);
				if (SceneQuest.IsMainStoryPart2(category))
				{
					mapPoint.Mark_NewPoint.gameObject.SetActive(false);
					if (!CS$<>8__locals1.<SetupMapData>g__LocalExistsPlayableMapId|0(qsm))
					{
						flag = false;
					}
					this.<SetupMapData>g__FuncLocalAnimCtrl|45_4(mapPoint.Mark_NewPoint2, flag);
				}
				else
				{
					mapPoint.Mark_NewPoint2.gameObject.SetActive(false);
					bool flag2 = DataManager.DmQuest.GetPlayableMapIdList(chapterId + 1).Count > 0;
					flag = qsm.mapId == CS$<>8__locals1.playableMapIdList[CS$<>8__locals1.playableMapIdList.Count - 1] && !flag2;
					this.<SetupMapData>g__FuncLocalAnimCtrl|45_4(mapPoint.Mark_NewPoint, flag);
				}
			}
			l = i;
		}
		foreach (BezierLine bezierLine in this.GuiData.mapData.mapLineList)
		{
			bezierLine.color = Color.gray;
		}
		for (int k = 0; k < this.GuiData.mapData.mapLineList.Count; k++)
		{
			BezierLine e = this.GuiData.mapData.mapLineList[k];
			QuestStaticMap mapData = CS$<>8__locals1.mapList.Find((QuestStaticMap item) => item.mapObjName == e.m_end.name);
			if (mapData != null)
			{
				if (CS$<>8__locals1.playableMapIdList.Exists((int item) => item == mapData.mapId))
				{
					e.color = Color.white;
				}
			}
			else if (k < CS$<>8__locals1.playableMapIdList.Count - 1)
			{
				e.color = Color.white;
			}
		}
		foreach (BezierLine bezierLine2 in this.GuiData.mapData.mapLineList)
		{
			bezierLine2.enabled = false;
			bezierLine2.enabled = true;
		}
		int index = 0;
		if (CS$<>8__locals1.playableMapIdList.Exists((int item) => item == this.initParam.getSelectDataCB().mapId))
		{
			index = CS$<>8__locals1.playableMapIdList.FindIndex((int item) => item == this.initParam.getSelectDataCB().mapId);
		}
		else if (CS$<>8__locals1.playableMapIdList.Exists((int item) => item == this.initParam.getSelectDataCB().mapId - 1))
		{
			index = CS$<>8__locals1.playableMapIdList.FindIndex((int item) => item == this.initParam.getSelectDataCB().mapId - 1);
		}
		else
		{
			index = CS$<>8__locals1.playableMapIdList.Count - 1;
		}
		if (index < 0)
		{
			index = 0;
		}
		this.initParam.getSelectDataCB().mapId = CS$<>8__locals1.playableMapIdList[index];
		this.GuiData.mapData.SetCarObjType(SelMainStoryCtrl.GetQuestCarType(chapterId));
		SelMainStoryCtrl.GuiMapData.MapPoint mapPoint2 = this.GuiData.mapData.mapPointList.Find((SelMainStoryCtrl.GuiMapData.MapPoint item) => item.pointObj.name == CS$<>8__locals1.playableMapIdList[index].ToString());
		if (mapPoint2 == null)
		{
			mapPoint2 = this.GuiData.mapData.mapPointList[index];
		}
		this.GuiData.mapData.mapCar.transform.position = mapPoint2.pointObj.transform.position;
		this.GuiData.mapData.PlayCarAnim();
		if (SceneQuest.IsMainStoryPart1(category) || SceneQuest.IsMainStoryPart2(category) || SceneQuest.IsMainStoryPart3(category))
		{
			Vector3 localPosition = this.GuiData.mapData.mapObj.transform.localPosition;
			this.GuiData.mapData.mapObj.transform.localPosition = new Vector3(this.GetMapOffsetPosX(category, true), localPosition.y, 0f);
			if (SceneQuest.IsMainStoryPart1(category))
			{
				Vector3 localPosition2 = this.GuiData.mapData.bgObj.transform.localPosition;
				this.GuiData.mapData.bgObj.transform.localPosition = new Vector3(this.GetMapOffsetPosX(category, false), localPosition2.y, 0f);
			}
			RectTransform rectTransform = this.GuiData.mapData.mapCar.transform as RectTransform;
			Vector3 vector = new Vector3(rectTransform.anchoredPosition.x - (float)(Screen.width / 2) * (QuestUtil.DEFAULT_SCREEN_WIDTH / (float)Screen.width), 0f, 0f);
			this.GuiData.mapData.mapObj.transform.localPosition -= vector;
			if (SceneQuest.IsMainStoryPart1(category))
			{
				this.GuiData.mapData.bgObj.transform.localPosition -= vector;
			}
			float scrollWidth = SelMainStoryCtrl.GetScrollWidth(category, this.GuiData.mapData);
			if (this.GuiData.mapData.mapObj.transform.localPosition.x <= -scrollWidth + SelMainStoryCtrl.GetSafeAreaX(category, this.mapBoxObject, this.GuiData.mapData))
			{
				Vector3 localPosition3 = this.GuiData.mapData.mapObj.transform.localPosition;
				this.GuiData.mapData.mapObj.transform.localPosition = new Vector3(-scrollWidth + SelMainStoryCtrl.GetSafeAreaX(category, this.mapBoxObject, this.GuiData.mapData), localPosition3.y, 0f);
				if (SceneQuest.IsMainStoryPart1(category))
				{
					Vector3 localPosition4 = this.GuiData.mapData.bgObj.transform.localPosition;
					this.GuiData.mapData.bgObj.transform.localPosition = new Vector3(-scrollWidth + SelMainStoryCtrl.GetSafeAreaX(category, this.mapBoxObject, this.GuiData.mapData), localPosition4.y, 0f);
					return;
				}
			}
			else if (this.GuiData.mapData.mapObj.transform.localPosition.x >= this.GetMapOffsetPosX(category, true))
			{
				Vector3 localPosition5 = this.GuiData.mapData.mapObj.transform.localPosition;
				this.GuiData.mapData.mapObj.transform.localPosition = new Vector3(this.GetMapOffsetPosX(category, true), localPosition5.y, 0f);
				if (SceneQuest.IsMainStoryPart1(category))
				{
					Vector3 localPosition6 = this.GuiData.mapData.bgObj.transform.localPosition;
					this.GuiData.mapData.bgObj.transform.localPosition = new Vector3(this.GetMapOffsetPosX(category, false), localPosition6.y, 0f);
				}
			}
		}
	}

	// Token: 0x0600181C RID: 6172 RVA: 0x001295DC File Offset: 0x001277DC
	private void UpdateMapButtonLR(int chapterId)
	{
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(chapterId - 1);
		List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(chapterId + 1);
		this.GuiData.pointSelect.Btn_Yaji_Left.gameObject.SetActive(playableMapIdList.Count > 0);
		this.GuiData.pointSelect.Btn_Yaji_Right.gameObject.SetActive(playableMapIdList2.Count > 0);
	}

	// Token: 0x06001820 RID: 6176 RVA: 0x001296B4 File Offset: 0x001278B4
	[CompilerGenerated]
	private void <SetupMapData>g__FuncLocalAnimCtrl|45_4(PguiAECtrl ae, bool dispFlag)
	{
		ae.gameObject.SetActive(dispFlag);
		if (this.initParam.checkQuestTopCB())
		{
			ae.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			return;
		}
		ae.PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
	}

	// Token: 0x0400129B RID: 4763
	public static readonly string MainStory1MapNamePrefix = "MainStory_";

	// Token: 0x0400129C RID: 4764
	public static readonly string CellvalMapNamePrefix = "CellvalStory_";

	// Token: 0x0400129D RID: 4765
	public static readonly string MainStory2MapNamePrefix = "MainStory_";

	// Token: 0x0400129E RID: 4766
	public static readonly string MainStory3MapNamePrefix = "MainStory_";

	// Token: 0x0400129F RID: 4767
	private static readonly float CellvalMapOffsetPosX = 101f;

	// Token: 0x040012A1 RID: 4769
	private SelMainStoryCtrl.SetupParam setupParam = new SelMainStoryCtrl.SetupParam();

	// Token: 0x040012A2 RID: 4770
	private SelMainStoryCtrl.InitParam initParam = new SelMainStoryCtrl.InitParam();

	// Token: 0x040012A3 RID: 4771
	private GameObject mapBoxObject;

	// Token: 0x02000D23 RID: 3363
	// (Invoke) Token: 0x06004851 RID: 18513
	public delegate GameObject OnGetCurrentSequence();

	// Token: 0x02000D24 RID: 3364
	// (Invoke) Token: 0x06004855 RID: 18517
	public delegate int OnGetCount();

	// Token: 0x02000D25 RID: 3365
	public class InitParam
	{
		// Token: 0x04004D89 RID: 19849
		public UnityAction reqNextSequenceCB;

		// Token: 0x04004D8A RID: 19850
		public UnityAction reqBackSequenceCB;

		// Token: 0x04004D8B RID: 19851
		public UnityAction selectObjsCB;

		// Token: 0x04004D8C RID: 19852
		public Transform prefabPath;

		// Token: 0x04004D8D RID: 19853
		public QuestUtil.OnGetSelectData getSelectDataCB;

		// Token: 0x04004D8E RID: 19854
		public UnityAction reqRewardsCB;

		// Token: 0x04004D8F RID: 19855
		public QuestUtil.OnCheck checkQuestTopCB;

		// Token: 0x04004D90 RID: 19856
		public UnityAction reqIncSelectDifficultCountCB;

		// Token: 0x04004D91 RID: 19857
		public SelMainStoryCtrl.OnGetCount getSelectDifficultCountCB;
	}

	// Token: 0x02000D26 RID: 3366
	public class SetupParam
	{
	}

	// Token: 0x02000D27 RID: 3367
	public class GUI
	{
		// Token: 0x0600485A RID: 18522 RVA: 0x0021BA55 File Offset: 0x00219C55
		public GUI(Transform baseTr)
		{
			this.pointSelect = new SelMainStoryCtrl.GuiPointSelect(baseTr.transform);
			this.pointSelect.baseObj.SetActive(false);
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x0021BA7F File Offset: 0x00219C7F
		public void SetupMapData(GameObject mapObj, List<GameObject> carObj)
		{
			this.mapData = new SelMainStoryCtrl.GuiMapData(mapObj.transform, carObj);
		}

		// Token: 0x04004D92 RID: 19858
		public SelMainStoryCtrl.GuiPointSelect pointSelect;

		// Token: 0x04004D93 RID: 19859
		public SelMainStoryCtrl.GuiMapData mapData;
	}

	// Token: 0x02000D28 RID: 3368
	public class GuiMapData : SceneQuest.IMapData
	{
		// Token: 0x0600485C RID: 18524 RVA: 0x0021BA94 File Offset: 0x00219C94
		public void SetupMapData(QuestStaticChapter chapther)
		{
			this.mapLineList.Clear();
			if (this.mapPointList.Count > 0)
			{
				for (int i = 0; i < this.mapPointList.Count; i++)
				{
					Object.Destroy(this.mapPointList[i].baseObj);
					this.mapPointList[i] = null;
				}
			}
			this.mapPointList.Clear();
			string prefix = SelMainStoryCtrl.MainStory1MapNamePrefix;
			if (SceneQuest.IsMainStoryPart1_5(chapther.category))
			{
				prefix = SelMainStoryCtrl.CellvalMapNamePrefix;
			}
			else if (SceneQuest.IsMainStoryPart2(chapther.category))
			{
				prefix = SelMainStoryCtrl.MainStory2MapNamePrefix;
			}
			else if (SceneQuest.IsMainStoryPart3(chapther.category))
			{
				prefix = SelMainStoryCtrl.MainStory3MapNamePrefix;
			}
			foreach (Transform transform in new List<Transform>(this.baseObj.transform.GetComponentsInChildren<Transform>()).FindAll((Transform item) => Regex.IsMatch(item.name, "^" + prefix + "[0-9]")))
			{
				transform.gameObject.SetActive(false);
			}
			Transform transform2 = this.baseObj.transform.Find(chapther.mapPath);
			this.mapObj = ((transform2 != null) ? transform2.gameObject : null);
			if (this.mapObj != null)
			{
				this.mapObj.SetActive(true);
				this.bgObj = this.baseObj.transform.Find("Bg").gameObject;
				int num = 0;
				Transform transform3 = this.mapObj.transform.Find("Line/Line" + (num + 1).ToString("D2"));
				while (transform3 != null)
				{
					this.mapLineList.Add(transform3.GetComponent<BezierLine>());
					transform3 = this.mapObj.transform.Find("Line/Line" + (++num + 1).ToString("D2"));
				}
				List<PguiRawImageCtrl> list = new List<PguiRawImageCtrl>(this.mapObj.transform.Find("Tex").GetComponentsInChildren<PguiRawImageCtrl>());
				list.Sort(delegate(PguiRawImageCtrl a, PguiRawImageCtrl b)
				{
					int num4 = 0;
					string[] array2 = a.name.Split('_', StringSplitOptions.None);
					if (array2.Length >= 2)
					{
						num4 = int.Parse(array2[0]);
					}
					int num5 = 0;
					string[] array3 = b.name.Split('_', StringSplitOptions.None);
					if (array3.Length >= 2)
					{
						num5 = int.Parse(array3[0]);
					}
					return num4 - num5;
				});
				for (int j = 0; j < list.Count; j++)
				{
					list[j].GetComponent<RawImage>().enabled = false;
					string[] array = list[j].name.Split('_', StringSplitOptions.None);
					if (array.Length >= 2)
					{
						int num2 = int.Parse(array[0]);
						int num3 = int.Parse(array[1]);
						GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/Quest_MapPoint"), this.mapObj.transform.Find("Tex/" + num2.ToString() + "_" + num3.ToString("D3")));
						this.mapPointList.Add(new SelMainStoryCtrl.GuiMapData.MapPoint(gameObject.transform, num3));
					}
				}
				foreach (GameObject gameObject2 in this.carObjList)
				{
					if (gameObject2 != null)
					{
						gameObject2.transform.SetParent(this.mapObj.transform, false);
					}
				}
			}
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x0021BE0C File Offset: 0x0021A00C
		public GuiMapData(Transform baseTr, List<GameObject> carList)
			: base(baseTr, carList)
		{
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x0021BE2C File Offset: 0x0021A02C
		public override void OutAnim()
		{
			foreach (SelMainStoryCtrl.GuiMapData.MapPoint mapPoint in this.mapPointList)
			{
				mapPoint.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
				{
				});
			}
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x0021BEA4 File Offset: 0x0021A0A4
		public override void InAnim()
		{
			foreach (SelMainStoryCtrl.GuiMapData.MapPoint mapPoint in this.mapPointList)
			{
				mapPoint.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
				{
				});
			}
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x0021BF1C File Offset: 0x0021A11C
		public void Destroy()
		{
			Object.Destroy(this.baseObj);
			this.baseObj = null;
		}

		// Token: 0x04004D94 RID: 19860
		public List<BezierLine> mapLineList = new List<BezierLine>();

		// Token: 0x04004D95 RID: 19861
		public List<SelMainStoryCtrl.GuiMapData.MapPoint> mapPointList = new List<SelMainStoryCtrl.GuiMapData.MapPoint>();

		// Token: 0x020011C4 RID: 4548
		public class MapPoint
		{
			// Token: 0x0600570D RID: 22285 RVA: 0x00255B40 File Offset: 0x00253D40
			public MapPoint(Transform baseTr, int num)
			{
				this.baseObj = baseTr.gameObject;
				this.baseRtf = baseTr as RectTransform;
				this.pointObj = this.baseObj;
				this.pointRtf.AddComponent<RectTransform>();
				this.pointRtf.transform.SetParent(this.pointObj.transform, true);
				this.pointRtf.transform.localPosition = new Vector3(0f, 0f, 0f);
				this.pointRtf.transform.localScale = new Vector3(1f, 1f, 1f);
				this.Mark_Batch = baseTr.Find("Mark_Batch").GetComponent<PguiImageCtrl>();
				this.Mark_NewPoint = baseTr.Find("Mark_NewPoint").GetComponent<PguiAECtrl>();
				this.Mark_NewPoint.gameObject.SetActive(false);
				this.Mark_NewPoint2 = baseTr.Find("Mark_NewPoint2").GetComponent<PguiAECtrl>();
				this.Mark_NewPoint2.gameObject.SetActive(false);
				this.Tex = baseTr.Find("Tex").GetComponent<PguiRawImageCtrl>();
				this.Tex.SetRawImage("Texture2D/QuestPointIcon/MainStory_Mapicon_" + num.ToString("D3"), true, false, null);
				this.Txt_MapName = baseTr.Find("Img_NameBase/Txt_MapName").GetComponent<PguiTextCtrl>();
				Resources.Load("SceneQuest/GUI/Prefab/Quest_Memori_MissionComp");
				this.Mark_Complete = baseTr.Find("Mark_Complete").gameObject;
				this.Mark_Complete.SetActive(false);
				this.anim = baseTr.Find("Img_NameBase").GetComponent<SimpleAnimation>();
				this.Txt_DayName = baseTr.Find("Img_DayBase/Txt_MapName").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x0600570E RID: 22286 RVA: 0x00255CFF File Offset: 0x00253EFF
			public void SetActiveDayName(bool sw)
			{
				this.Txt_DayName.transform.parent.gameObject.SetActive(sw);
			}

			// Token: 0x0600570F RID: 22287 RVA: 0x00255D1C File Offset: 0x00253F1C
			public string GetName()
			{
				return this.baseObj.transform.parent.name;
			}

			// Token: 0x04006180 RID: 24960
			public GameObject baseObj;

			// Token: 0x04006181 RID: 24961
			public RectTransform baseRtf;

			// Token: 0x04006182 RID: 24962
			public GameObject pointObj;

			// Token: 0x04006183 RID: 24963
			public GameObject pointRtf = new GameObject();

			// Token: 0x04006184 RID: 24964
			public PguiRawImageCtrl Tex;

			// Token: 0x04006185 RID: 24965
			public PguiImageCtrl Mark_Batch;

			// Token: 0x04006186 RID: 24966
			public PguiAECtrl Mark_NewPoint;

			// Token: 0x04006187 RID: 24967
			public PguiAECtrl Mark_NewPoint2;

			// Token: 0x04006188 RID: 24968
			public PguiTextCtrl Txt_MapName;

			// Token: 0x04006189 RID: 24969
			public GameObject Mark_Complete;

			// Token: 0x0400618A RID: 24970
			public SimpleAnimation anim;

			// Token: 0x0400618B RID: 24971
			public PguiTextCtrl Txt_DayName;
		}
	}

	// Token: 0x02000D29 RID: 3369
	public class GuiPointSelect
	{
		// Token: 0x06004861 RID: 18529 RVA: 0x0021BF30 File Offset: 0x0021A130
		public GuiPointSelect(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_ChapterChange = baseTr.Find("Right/Btn_ChapterChange").GetComponent<PguiButtonCtrl>();
			this.Btn_Mission = baseTr.Find("Right/Btn_Mission").GetComponent<PguiButtonCtrl>();
			this.Mission_New = baseTr.Find("Right/Btn_Mission/BaseImage/Mark_New").gameObject;
			this.Mission_New.SetActive(false);
			this.Txt_Mission_Num = baseTr.Find("Right/Btn_Mission/BaseImage/Badge/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
			this.Btn_Yaji_Left = baseTr.Find("LeftBtn/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
			this.Btn_Yaji_Right = baseTr.Find("RightBtn/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
			this.Btn_Sel_Difficult = baseTr.Find("Right/Btn_Sel_Difficult").GetComponent<PguiButtonCtrl>();
			this.campaignInfo = new QuestUtil.CampaignInfo(baseTr.Find("Right/Campaign"));
			this.markLock = this.Btn_Sel_Difficult.transform.Find("BaseImage/Mark_Lock").GetComponent<MarkLockCtrl>();
			this.Mark_Hard = baseTr.Find("Left/Mark_Hard").GetComponent<PguiImageCtrl>();
			this.Mark_Hard.gameObject.SetActive(false);
			this.chapterInfoList = new List<SelMainStoryCtrl.GuiPointSelect.ChapterInfo>
			{
				new SelMainStoryCtrl.GuiPointSelect.ChapterInfo(baseTr.Find("Left/ChapterInfo")),
				new SelMainStoryCtrl.GuiPointSelect.ChapterInfo(baseTr.Find("Left/ChapterInfo_Cellvall")),
				new SelMainStoryCtrl.GuiPointSelect.ChapterInfo(baseTr.Find("Left/ChapterInfo02")),
				new SelMainStoryCtrl.GuiPointSelect.ChapterInfo(baseTr.Find("Left/ChapterInfo03"))
			};
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x0021C0B8 File Offset: 0x0021A2B8
		public void UpdateCampaignInfoCategory(QuestStaticChapter.Category category, int chapterId)
		{
			List<string> list = new List<string>(QuestUtil.GetCampaignMessageList(category, chapterId));
			List<string> list2 = new List<string>(QuestUtil.GetCampaignTimeList(category, chapterId));
			this.campaignInfo.DispCampaign(list, list2);
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x0021C0EC File Offset: 0x0021A2EC
		public void ResetCampaignInfoCategory()
		{
			this.campaignInfo.ResetCampaign();
		}

		// Token: 0x04004D96 RID: 19862
		public GameObject baseObj;

		// Token: 0x04004D97 RID: 19863
		public PguiButtonCtrl Btn_ChapterChange;

		// Token: 0x04004D98 RID: 19864
		public PguiButtonCtrl Btn_Mission;

		// Token: 0x04004D99 RID: 19865
		public PguiButtonCtrl Btn_Yaji_Left;

		// Token: 0x04004D9A RID: 19866
		public PguiButtonCtrl Btn_Yaji_Right;

		// Token: 0x04004D9B RID: 19867
		public GameObject Mission_New;

		// Token: 0x04004D9C RID: 19868
		public PguiTextCtrl Txt_Mission_Num;

		// Token: 0x04004D9D RID: 19869
		public PguiButtonCtrl Btn_Sel_Difficult;

		// Token: 0x04004D9E RID: 19870
		public QuestUtil.CampaignInfo campaignInfo;

		// Token: 0x04004D9F RID: 19871
		public MarkLockCtrl markLock;

		// Token: 0x04004DA0 RID: 19872
		public PguiImageCtrl Mark_Hard;

		// Token: 0x04004DA1 RID: 19873
		public List<SelMainStoryCtrl.GuiPointSelect.ChapterInfo> chapterInfoList;

		// Token: 0x020011C7 RID: 4551
		public class ItemInfoIcon
		{
			// Token: 0x06005717 RID: 22295 RVA: 0x00255DC6 File Offset: 0x00253FC6
			public ItemInfoIcon(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_Episodes = baseTr.Find("Txt_Day").GetComponent<PguiTextCtrl>();
				this.Mark_Get = baseTr.Find("Mark_Get").GetComponent<PguiImageCtrl>();
			}

			// Token: 0x06005718 RID: 22296 RVA: 0x00255E06 File Offset: 0x00254006
			public void SetItemInfo(bool isGet, string text)
			{
				this.Txt_Episodes.gameObject.SetActive(!isGet);
				this.Txt_Episodes.m_Text.text = text;
				this.Mark_Get.gameObject.SetActive(isGet);
			}

			// Token: 0x06005719 RID: 22297 RVA: 0x00255E3E File Offset: 0x0025403E
			public void SetEmptyStr()
			{
				this.Txt_Episodes.transform.Find("Txt_Ato").GetComponent<PguiTextCtrl>().text = string.Empty;
			}

			// Token: 0x04006191 RID: 24977
			public GameObject baseObj;

			// Token: 0x04006192 RID: 24978
			public PguiTextCtrl Txt_Episodes;

			// Token: 0x04006193 RID: 24979
			public PguiImageCtrl Mark_Get;
		}

		// Token: 0x020011C8 RID: 4552
		public class ChapterInfo
		{
			// Token: 0x0600571A RID: 22298 RVA: 0x00255E64 File Offset: 0x00254064
			public ChapterInfo(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_ChapterName = baseTr.Find("TitleBase/Txt_ChapterName").GetComponent<PguiTextCtrl>();
				this.Txt_Info = baseTr.Find("Txt_Info").GetComponent<PguiTextCtrl>();
				this.ItemInfo = baseTr.Find("ItemInfo").gameObject;
				this.itemInfoIcons = new List<SelMainStoryCtrl.GuiPointSelect.ItemInfoIcon>
				{
					new SelMainStoryCtrl.GuiPointSelect.ItemInfoIcon(baseTr.Find("ItemInfo/Grid/Icon01")),
					new SelMainStoryCtrl.GuiPointSelect.ItemInfoIcon(baseTr.Find("ItemInfo/Grid/Icon02")),
					new SelMainStoryCtrl.GuiPointSelect.ItemInfoIcon(baseTr.Find("ItemInfo/Grid/Icon03")),
					new SelMainStoryCtrl.GuiPointSelect.ItemInfoIcon(baseTr.Find("ItemInfo/Grid/Icon04"))
				};
				foreach (SelMainStoryCtrl.GuiPointSelect.ItemInfoIcon itemInfoIcon in this.itemInfoIcons)
				{
					itemInfoIcon.baseObj.SetActive(false);
				}
			}

			// Token: 0x04006194 RID: 24980
			public GameObject baseObj;

			// Token: 0x04006195 RID: 24981
			public PguiTextCtrl Txt_ChapterName;

			// Token: 0x04006196 RID: 24982
			public PguiTextCtrl Txt_Info;

			// Token: 0x04006197 RID: 24983
			public GameObject ItemInfo;

			// Token: 0x04006198 RID: 24984
			public List<SelMainStoryCtrl.GuiPointSelect.ItemInfoIcon> itemInfoIcons;
		}
	}
}
