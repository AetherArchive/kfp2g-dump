using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelMainStoryCtrl : MonoBehaviour
{
	public SelMainStoryCtrl.GUI GuiData { get; private set; }

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

	public void SetMapBoxObject(GameObject go)
	{
		this.mapBoxObject = go;
	}

	public static SelMainStoryCtrl.GuiMapData SetupMapGUI(GameObject mapObj, List<GameObject> carObj)
	{
		return new SelMainStoryCtrl.GuiMapData(mapObj.transform, carObj);
	}

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

	public void Setup(SelMainStoryCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		this.TouchMoving = false;
	}

	public void Dest()
	{
	}

	public void Destroy()
	{
	}

	public void UpdateDecoration()
	{
	}

	public bool TouchMoving { get; private set; }

	public bool IsNotNullMapObj()
	{
		return this.GuiData.mapData != null;
	}

	public bool IsNotNullMapBaseObj()
	{
		return this.IsNotNullMapObj() && this.GuiData.mapData.baseObj != null;
	}

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

	public void UpdateMission()
	{
		int userClearMissionNum = DataManager.DmMission.GetUserClearMissionNum();
		this.GuiData.pointSelect.Txt_Mission_Num.transform.parent.transform.parent.gameObject.SetActive(userClearMissionNum > 0);
		this.GuiData.pointSelect.Txt_Mission_Num.text = userClearMissionNum.ToString();
	}

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

	public float GetMapOffsetPosX(QuestStaticChapter.Category category, bool isMap)
	{
		return SelMainStoryCtrl.GetMapOffsetPosX(category, this.mapBoxObject, this.GuiData.mapData, isMap);
	}

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

	public static float GetScrollWidth(QuestStaticChapter.Category category, SelMainStoryCtrl.GuiMapData guiMapData)
	{
		return guiMapData.bgObj.transform.Find("Tex_Bg").GetComponent<RectTransform>().sizeDelta.x - QuestUtil.DEFAULT_SCREEN_WIDTH;
	}

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

	public void RemoveTouchEventTriggger()
	{
		for (int i = 0; i < this.GuiData.mapData.mapPointList.Count; i++)
		{
			PrjUtil.RemoveTouchEventTrigger(this.GuiData.mapData.mapPointList[i].pointRtf.gameObject);
		}
	}

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

	private void UpdateMapButtonLR(int chapterId)
	{
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(chapterId - 1);
		List<int> playableMapIdList2 = DataManager.DmQuest.GetPlayableMapIdList(chapterId + 1);
		this.GuiData.pointSelect.Btn_Yaji_Left.gameObject.SetActive(playableMapIdList.Count > 0);
		this.GuiData.pointSelect.Btn_Yaji_Right.gameObject.SetActive(playableMapIdList2.Count > 0);
	}

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

	public static readonly string MainStory1MapNamePrefix = "MainStory_";

	public static readonly string CellvalMapNamePrefix = "CellvalStory_";

	public static readonly string MainStory2MapNamePrefix = "MainStory_";

	public static readonly string MainStory3MapNamePrefix = "MainStory_";

	private static readonly float CellvalMapOffsetPosX = 101f;

	private SelMainStoryCtrl.SetupParam setupParam = new SelMainStoryCtrl.SetupParam();

	private SelMainStoryCtrl.InitParam initParam = new SelMainStoryCtrl.InitParam();

	private GameObject mapBoxObject;

	public delegate GameObject OnGetCurrentSequence();

	public delegate int OnGetCount();

	public class InitParam
	{
		public UnityAction reqNextSequenceCB;

		public UnityAction reqBackSequenceCB;

		public UnityAction selectObjsCB;

		public Transform prefabPath;

		public QuestUtil.OnGetSelectData getSelectDataCB;

		public UnityAction reqRewardsCB;

		public QuestUtil.OnCheck checkQuestTopCB;

		public UnityAction reqIncSelectDifficultCountCB;

		public SelMainStoryCtrl.OnGetCount getSelectDifficultCountCB;
	}

	public class SetupParam
	{
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.pointSelect = new SelMainStoryCtrl.GuiPointSelect(baseTr.transform);
			this.pointSelect.baseObj.SetActive(false);
		}

		public void SetupMapData(GameObject mapObj, List<GameObject> carObj)
		{
			this.mapData = new SelMainStoryCtrl.GuiMapData(mapObj.transform, carObj);
		}

		public SelMainStoryCtrl.GuiPointSelect pointSelect;

		public SelMainStoryCtrl.GuiMapData mapData;
	}

	public class GuiMapData : SceneQuest.IMapData
	{
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

		public GuiMapData(Transform baseTr, List<GameObject> carList)
			: base(baseTr, carList)
		{
		}

		public override void OutAnim()
		{
			foreach (SelMainStoryCtrl.GuiMapData.MapPoint mapPoint in this.mapPointList)
			{
				mapPoint.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
				{
				});
			}
		}

		public override void InAnim()
		{
			foreach (SelMainStoryCtrl.GuiMapData.MapPoint mapPoint in this.mapPointList)
			{
				mapPoint.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
				{
				});
			}
		}

		public void Destroy()
		{
			Object.Destroy(this.baseObj);
			this.baseObj = null;
		}

		public List<BezierLine> mapLineList = new List<BezierLine>();

		public List<SelMainStoryCtrl.GuiMapData.MapPoint> mapPointList = new List<SelMainStoryCtrl.GuiMapData.MapPoint>();

		public class MapPoint
		{
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

			public void SetActiveDayName(bool sw)
			{
				this.Txt_DayName.transform.parent.gameObject.SetActive(sw);
			}

			public string GetName()
			{
				return this.baseObj.transform.parent.name;
			}

			public GameObject baseObj;

			public RectTransform baseRtf;

			public GameObject pointObj;

			public GameObject pointRtf = new GameObject();

			public PguiRawImageCtrl Tex;

			public PguiImageCtrl Mark_Batch;

			public PguiAECtrl Mark_NewPoint;

			public PguiAECtrl Mark_NewPoint2;

			public PguiTextCtrl Txt_MapName;

			public GameObject Mark_Complete;

			public SimpleAnimation anim;

			public PguiTextCtrl Txt_DayName;
		}
	}

	public class GuiPointSelect
	{
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

		public PguiButtonCtrl Btn_ChapterChange;

		public PguiButtonCtrl Btn_Mission;

		public PguiButtonCtrl Btn_Yaji_Left;

		public PguiButtonCtrl Btn_Yaji_Right;

		public GameObject Mission_New;

		public PguiTextCtrl Txt_Mission_Num;

		public PguiButtonCtrl Btn_Sel_Difficult;

		public QuestUtil.CampaignInfo campaignInfo;

		public MarkLockCtrl markLock;

		public PguiImageCtrl Mark_Hard;

		public List<SelMainStoryCtrl.GuiPointSelect.ChapterInfo> chapterInfoList;

		public class ItemInfoIcon
		{
			public ItemInfoIcon(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_Episodes = baseTr.Find("Txt_Day").GetComponent<PguiTextCtrl>();
				this.Mark_Get = baseTr.Find("Mark_Get").GetComponent<PguiImageCtrl>();
			}

			public void SetItemInfo(bool isGet, string text)
			{
				this.Txt_Episodes.gameObject.SetActive(!isGet);
				this.Txt_Episodes.m_Text.text = text;
				this.Mark_Get.gameObject.SetActive(isGet);
			}

			public void SetEmptyStr()
			{
				this.Txt_Episodes.transform.Find("Txt_Ato").GetComponent<PguiTextCtrl>().text = string.Empty;
			}

			public GameObject baseObj;

			public PguiTextCtrl Txt_Episodes;

			public PguiImageCtrl Mark_Get;
		}

		public class ChapterInfo
		{
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

			public GameObject baseObj;

			public PguiTextCtrl Txt_ChapterName;

			public PguiTextCtrl Txt_Info;

			public GameObject ItemInfo;

			public List<SelMainStoryCtrl.GuiPointSelect.ItemInfoIcon> itemInfoIcons;
		}
	}
}
