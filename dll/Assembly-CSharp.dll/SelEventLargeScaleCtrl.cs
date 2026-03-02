using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SGNFW.Common;
using SGNFW.Touch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000174 RID: 372
public class SelEventLargeScaleCtrl : MonoBehaviour
{
	// Token: 0x06001773 RID: 6003 RVA: 0x001241E3 File Offset: 0x001223E3
	public static void SetPathEventChapterId(int id)
	{
	}

	// Token: 0x06001774 RID: 6004 RVA: 0x001241E5 File Offset: 0x001223E5
	public static int GetPathEventChapterId()
	{
		return SelEventLargeScaleCtrl.pathEventChapterId;
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x06001775 RID: 6005 RVA: 0x001241EC File Offset: 0x001223EC
	// (set) Token: 0x06001776 RID: 6006 RVA: 0x001241F4 File Offset: 0x001223F4
	public SelEventLargeScaleCtrl.GUI GuiData { get; private set; }

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x06001777 RID: 6007 RVA: 0x001241FD File Offset: 0x001223FD
	public SelEventLargeScaleCtrl.MapDataGUI MapData
	{
		get
		{
			SelEventLargeScaleCtrl.GUI guiData = this.GuiData;
			if (guiData == null)
			{
				return null;
			}
			return guiData.mapData;
		}
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x06001778 RID: 6008 RVA: 0x00124210 File Offset: 0x00122410
	private Vector2Int MapDirection
	{
		get
		{
			DataManagerEvent.LargeEventData largeEventData = DataManager.DmEvent.GetLargeEventData(this.setupParam.eventData.eventId);
			if (largeEventData != null)
			{
				return largeEventData.MapDirection;
			}
			return new Vector2Int(0, 1);
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x06001779 RID: 6009 RVA: 0x0012424C File Offset: 0x0012244C
	private Vector2Int MapRangeOrigin
	{
		get
		{
			DataManagerEvent.LargeEventData largeEventData = DataManager.DmEvent.GetLargeEventData(this.setupParam.eventData.eventId);
			if (largeEventData != null)
			{
				return largeEventData.MapRangeOrigin;
			}
			return new Vector2Int(0, 0);
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x0600177A RID: 6010 RVA: 0x00124288 File Offset: 0x00122488
	private Vector2Int MapRangeSize
	{
		get
		{
			DataManagerEvent.LargeEventData largeEventData = DataManager.DmEvent.GetLargeEventData(this.setupParam.eventData.eventId);
			if (largeEventData != null)
			{
				return largeEventData.MapRangeSize;
			}
			return new Vector2Int((int)SelEventLargeScaleCtrl.DEFAULT_SCREEN_WIDTH, (int)SelEventLargeScaleCtrl.DEFAULT_SCREEN_HEIGHT);
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x0600177B RID: 6011 RVA: 0x001242CB File Offset: 0x001224CB
	private Vector2 MapOffset
	{
		get
		{
			return SelEventLargeScaleCtrl.GetMapOffset(this.mapBoxObject, this.GuiData.mapData, this.MapRangeSize, this.setupParam.eventData.eventId);
		}
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x001242FC File Offset: 0x001224FC
	public static Vector2 GetMapOffset(GameObject mapBoxObject, SelEventLargeScaleCtrl.MapDataGUI guiMapData, Vector2Int mapRangeSize, int eventId = 0)
	{
		RectTransform rectTransform = mapBoxObject.transform as RectTransform;
		float leftSidePosX = QuestUtil.GetLeftSidePosX(mapBoxObject);
		float num;
		if (eventId != 0 && mapRangeSize.x > 0)
		{
			if (QuestUtil.HasUnsafeAreaLeft(mapBoxObject, leftSidePosX))
			{
				if (-leftSidePosX > QuestUtil.MAP_MASK_IMAGE_WIDTH || (rectTransform.offsetMax.x >= QuestUtil.MAP_MASK_IMAGE_WIDTH && -leftSidePosX < QuestUtil.MAP_MASK_IMAGE_WIDTH))
				{
					num = QuestUtil.MAP_MASK_IMAGE_WIDTH + leftSidePosX;
				}
				else
				{
					num = 0f;
				}
			}
			else
			{
				num = ((rectTransform.offsetMax.x >= QuestUtil.MAP_MASK_IMAGE_WIDTH) ? QuestUtil.MAP_MASK_IMAGE_WIDTH : 0f);
			}
		}
		else
		{
			float num2 = ((guiMapData.bgObj.transform.Find("Tex_Bg").localPosition.x == 0f) ? 0f : (guiMapData.bgObj.transform.Find("Tex_Bg").localPosition.x + QuestUtil.MAP_MASK_IMAGE_WIDTH));
			num = -((QuestUtil.DEFAULT_BG_WIDTH - rectTransform.rect.width) * 0.5f - QuestUtil.MAP_MASK_IMAGE_WIDTH + num2);
			if (-leftSidePosX > QuestUtil.MAP_MASK_IMAGE_WIDTH - num - num2 || (SafeAreaScaler.IsLongDevice() && -leftSidePosX < QuestUtil.MAP_MASK_IMAGE_WIDTH - num - num2))
			{
				num -= -leftSidePosX - (QuestUtil.MAP_MASK_IMAGE_WIDTH - num - num2);
			}
			else if (QuestUtil.GetRightSidePosX(mapBoxObject) > QuestUtil.DEFAULT_BG_WIDTH - QuestUtil.MAP_MASK_IMAGE_WIDTH + num + num2)
			{
				num += QuestUtil.GetRightSidePosX(mapBoxObject) - (QuestUtil.DEFAULT_BG_WIDTH - QuestUtil.MAP_MASK_IMAGE_WIDTH + num + num2);
			}
		}
		DataManagerEvent.LargeEventData largeEventData = DataManager.DmEvent.GetLargeEventData(eventId);
		if (largeEventData != null)
		{
			return new Vector2(num, largeEventData.MapOffset.y);
		}
		return new Vector2(num, 0f);
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x0012449F File Offset: 0x0012269F
	public void AdjustMaskPostion()
	{
		SelEventLargeScaleCtrl.AdjustMaskPostion(this.GuiData.mapData, this.MapOffset);
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x001244B8 File Offset: 0x001226B8
	public static void AdjustMaskPostion(SelEventLargeScaleCtrl.MapDataGUI mapDataGUI, Vector2 MapOffset)
	{
		float num = ((mapDataGUI.bgObj.transform.Find("Tex_Bg").localPosition.x == 0f) ? 0f : (mapDataGUI.bgObj.transform.Find("Tex_Bg").localPosition.x + QuestUtil.MAP_MASK_IMAGE_WIDTH));
		GameObject gameObject = mapDataGUI.baseObj.transform.Find("Map_Frame").gameObject;
		Vector3 localPosition = gameObject.transform.localPosition;
		gameObject.transform.localPosition = new Vector3(MapOffset.x + num, localPosition.y, 0f);
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x0600177F RID: 6015 RVA: 0x00124560 File Offset: 0x00122760
	private string MapFilePath
	{
		get
		{
			DataManagerEvent.LargeEventData largeEventData = DataManager.DmEvent.GetLargeEventData(this.setupParam.eventData.eventId);
			if (largeEventData != null)
			{
				return largeEventData.MapFilePath;
			}
			return "Gui/QuestMap/GUI_Map_Event_0001";
		}
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x00124598 File Offset: 0x00122798
	private static List<string> GetTipsFilePath(int eventId)
	{
		DataManagerEvent.LargeEventData largeEventData = DataManager.DmEvent.GetLargeEventData(eventId);
		if (largeEventData != null)
		{
			return largeEventData.TipsFilePath;
		}
		return new List<string> { "Texture2D/Tutorial_Window/Event_Map/001/tutorial_eventmap_001_01", "Texture2D/Tutorial_Window/Event_Map/001/tutorial_eventmap_001_02", "Texture2D/Tutorial_Window/Event_Map/001/tutorial_eventmap_001_03", "Texture2D/Tutorial_Window/Event_Map/001/tutorial_eventmap_001_04" };
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x06001781 RID: 6017 RVA: 0x001245EC File Offset: 0x001227EC
	private string MapBgFilePath
	{
		get
		{
			DataManagerEvent.LargeEventData largeEventData = DataManager.DmEvent.GetLargeEventData(this.setupParam.eventData.eventId);
			if (largeEventData != null)
			{
				List<DataManagerEvent.LargeEventData.MapFileData> list = new List<DataManagerEvent.LargeEventData.MapFileData>(largeEventData.MapFileDataList);
				list.Reverse();
				foreach (DataManagerEvent.LargeEventData.MapFileData mapFileData in list)
				{
					if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(mapFileData.openQuestOneId))
					{
						QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[mapFileData.openQuestOneId];
						if (questDynamicQuestOne.status == QuestOneStatus.CLEAR || questDynamicQuestOne.status == QuestOneStatus.COMPLETE)
						{
							return mapFileData.filepath;
						}
					}
				}
				return largeEventData.MapFileDataList[0].filepath;
			}
			return "Texture2D/EventMap/event_map_7008";
		}
	}

	// Token: 0x06001782 RID: 6018 RVA: 0x001246D4 File Offset: 0x001228D4
	public static void OpenHelpWindow(int eventId)
	{
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "イベントの遊びかた", SelEventLargeScaleCtrl.GetTipsFilePath(eventId), null);
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06001783 RID: 6019 RVA: 0x001246ED File Offset: 0x001228ED
	public string LoadAssetPath
	{
		get
		{
			return this.MapFilePath;
		}
	}

	// Token: 0x06001784 RID: 6020 RVA: 0x001246F5 File Offset: 0x001228F5
	public IEnumerator LoadMapObject(GameObject go)
	{
		string path = this.LoadAssetPath;
		this.mapBoxObject = go;
		AssetManager.LoadAssetData(path, AssetManager.OWNER.QuestSelector, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(path))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001785 RID: 6021 RVA: 0x0012470C File Offset: 0x0012290C
	public void Init(SelEventLargeScaleCtrl.InitParam _initParam, SelEventLargeScaleCtrl.SetupParam _setupParam)
	{
		SelEventLargeScaleCtrl.pathEventChapterId = _setupParam.eventData.eventChapterId;
		this.material = new Material(Shader.Find("UI/Grayscale"));
		this.initParam = _initParam;
		GameObject gameObject = AssetManager.InstantiateAssetData("SceneEvent/GUI/Prefab/GUI_Event_Map_PointSelect", base.transform);
		this.GuiData = new SelEventLargeScaleCtrl.GUI(gameObject.transform);
		this.GuiData.pointSelect.Btn_ShopEvent.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.GuiData.pointSelect.Btn_Gacha.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.GuiData.pointSelect.Btn_Mission.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.Setup(_setupParam);
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x001247D4 File Offset: 0x001229D4
	public void Setup(SelEventLargeScaleCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		PrjUtil.AddTouchEventTrigger(this.GuiData.pointSelect.EventBanner.gameObject, new UnityAction<Transform>(this.OnClickEventInfoBanner));
		this.TouchMoving = false;
		List<DataManagerEvent.ReleaseEffects> releaseEffectsList = DataManager.DmEvent.GetReleaseEffectsList();
		DataManagerEvent.ReleaseEffects releaseEffects = null;
		QuestUtil.GetEnableEventReleaseEffects(ref releaseEffectsList, ref releaseEffects, this.setupParam.eventData);
		if (releaseEffects.TutorialPhase == 0)
		{
			SelEventLargeScaleCtrl.OpenHelpWindow(this.setupParam.eventData.eventId);
			releaseEffects.TutorialPhase = 1;
			DataManager.DmEvent.RequestUpdateReleaseEffects(releaseEffectsList);
		}
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x00124865 File Offset: 0x00122A65
	public void Dest()
	{
	}

	// Token: 0x06001788 RID: 6024 RVA: 0x00124867 File Offset: 0x00122A67
	public void Destroy()
	{
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x0012486C File Offset: 0x00122A6C
	public void UpdateDecoration()
	{
		CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByLarge(true, this.setupParam.eventData.eventId);
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.setupParam.eventData.eventId);
		if (eventData == null)
		{
			return;
		}
		HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(this.setupParam.eventData.eventBannerId);
		if (homeBannerData != null)
		{
			this.GuiData.pointSelect.EventBanner.banner = homeBannerData.bannerImagePathEvent;
		}
		DateTime startTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.setupParam.eventData.eventChapterId].mapDataList[0].questGroupList[0].startTime;
		DateTime endTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.setupParam.eventData.eventChapterId].mapDataList[0].questGroupList[0].endTime;
		this.GuiData.pointSelect.Txt_Term.text = startTime.ToString("M/d") + " ～ " + endTime.ToString("M/d HH:mm まで");
		for (int i = 0; i < this.GuiData.pointSelect.itemOwnBases.Count; i++)
		{
			SelEventLargeScaleCtrl.PointSelectGUI.ItemOwnBase itemOwnBase = this.GuiData.pointSelect.itemOwnBases[i];
			itemOwnBase.baseObj.SetActive(i < this.setupParam.eventData.eventCoinIdList.Count);
			if (itemOwnBase.baseObj.activeSelf)
			{
				itemOwnBase.Num_Txt.text = DataManager.DmItem.GetUserItemData(this.setupParam.eventData.eventCoinIdList[i]).num.ToString();
				itemOwnBase.Icon_Stone.SetRawImage(DataManager.DmItem.GetItemStaticBase(this.setupParam.eventData.eventCoinIdList[i]).GetIconName(), true, false, null);
			}
		}
		int userClearEventMissionNum = DataManager.DmMission.GetUserClearEventMissionNum(eventData.eventId);
		this.GuiData.pointSelect.Txt_Mission_Num.transform.parent.transform.parent.gameObject.SetActive(userClearEventMissionNum > 0);
		this.GuiData.pointSelect.Txt_Mission_Num.text = userClearEventMissionNum.ToString();
	}

	// Token: 0x0600178A RID: 6026 RVA: 0x00124AEC File Offset: 0x00122CEC
	public void RequestUseUnlockItem(int mapId)
	{
		QuestStaticMap mapData = DataManager.DmQuest.QuestStaticData.mapDataMap[mapId];
		if (mapData != null)
		{
			SelEventLargeScaleCtrl.MapPointRouteGUI mapPointRouteGUI = this.MapData.mapPointRoutes.Find((SelEventLargeScaleCtrl.MapPointRouteGUI item) => item.GetName() == mapData.mapObjName);
			if (mapPointRouteGUI != null)
			{
				mapPointRouteGUI.mapPoint.markLockCtrl.StartAEForce();
			}
		}
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x00124B54 File Offset: 0x00122D54
	public void RemoveTouchEventTriggger(int chapterId)
	{
		DataManager.DmQuest.GetPlayableMapIdList(chapterId);
		foreach (SelEventLargeScaleCtrl.MapPointRouteGUI mapPointRouteGUI in this.MapData.mapPointRoutes)
		{
			PrjUtil.RemoveTouchEventTrigger(mapPointRouteGUI.mapPoint.pointRtf.gameObject);
		}
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x00124BC4 File Offset: 0x00122DC4
	public void SetupMapData(bool startMarkAE, UnityAction<Transform> pointTouchCB, QuestUtil.SelectData selectData)
	{
		SelEventLargeScaleCtrl.<>c__DisplayClass55_0 CS$<>8__locals1 = new SelEventLargeScaleCtrl.<>c__DisplayClass55_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.selectData = selectData;
		this.AdjustMaskPostion();
		int chapterId = CS$<>8__locals1.selectData.chapterId;
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(chapterId);
		List<QuestStaticMap> mapDataList = DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId].mapDataList;
		using (List<SelEventLargeScaleCtrl.MapPointRouteGUI>.Enumerator enumerator = this.MapData.mapPointRoutes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SelEventLargeScaleCtrl.<>c__DisplayClass55_1 CS$<>8__locals2 = new SelEventLargeScaleCtrl.<>c__DisplayClass55_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.mapPointRoute = enumerator.Current;
				QuestStaticMap mapData2 = mapDataList.Find((QuestStaticMap item) => item.mapObjName == CS$<>8__locals2.mapPointRoute.GetName());
				if (mapData2 != null)
				{
					CS$<>8__locals2.mapPointRoute.mapPoint.pointObj.name = mapData2.mapId.ToString();
					CS$<>8__locals2.mapPointRoute.mapPoint.pointRtf.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 200f);
					CS$<>8__locals2.mapPointRoute.mapPoint.pointRtf.name = mapData2.mapId.ToString();
					PrjUtil.AddTouchEventTrigger(CS$<>8__locals2.mapPointRoute.mapPoint.pointRtf.gameObject, pointTouchCB);
					this.UpdateMaterial(playableMapIdList, CS$<>8__locals2.mapPointRoute, mapData2);
					CS$<>8__locals2.mapPointRoute.mapPoint.Txt_MapName.text = mapData2.mapName;
					this.UpdateNewMark(CS$<>8__locals2.mapPointRoute, mapData2, playableMapIdList, startMarkAE);
					QuestUIMapInfo questUIMapInfo = QuestUIMapInfo.GetQuestUIMapInfo(mapData2.mapId, SceneQuest.TimeStampInScene, this.setupParam.eventData.eventId);
					int num = 0;
					if (questUIMapInfo.isLockByTime)
					{
						num = SelEventLargeScaleCtrl.LockByTimeReplaceId;
					}
					else if (questUIMapInfo.isPaidOpenItem)
					{
						num = SelEventLargeScaleCtrl.LockByPaidKeyReplaceId;
					}
					else if (questUIMapInfo.isLockByItem)
					{
						num = SelEventLargeScaleCtrl.LockByFreeKeyReplaceId;
					}
					if (num > 0)
					{
						CS$<>8__locals2.mapPointRoute.mapPoint.Mark_LockReason.Replace(num);
					}
					MarkLockCtrl markLockCtrl = CS$<>8__locals2.mapPointRoute.mapPoint.markLockCtrl;
					MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();
					setupParam.releaseFlag = false;
					setupParam.tagetObject = null;
					setupParam.text = "";
					setupParam.updateConditionCallback = () => false;
					setupParam.updateUserFlagDataCallback = delegate
					{
						Singleton<SceneManager>.Instance.StartCoroutine(CS$<>8__locals2.CS$<>8__locals1.<>4__this.RequestOpenQuestByItem(CS$<>8__locals2.mapPointRoute, mapData2));
					};
					markLockCtrl.Setup(setupParam, false);
					if (!questUIMapInfo.isLockByItem && !questUIMapInfo.isLockByTime)
					{
						CS$<>8__locals2.mapPointRoute.mapPoint.markLockCtrl.SetActive(false);
						CS$<>8__locals2.mapPointRoute.mapPoint.Mark_LockReason.gameObject.SetActive(CS$<>8__locals2.mapPointRoute.mapPoint.markLockCtrl.IsActive());
					}
					CS$<>8__locals2.mapPointRoute.mapPoint.ItemPop.gameObject.SetActive(false);
					if (questUIMapInfo.pickupRewardItem != null && !questUIMapInfo.isGetPickupRewardItem)
					{
						CS$<>8__locals2.mapPointRoute.mapPoint.ItemPop.gameObject.SetActive(true);
						CS$<>8__locals2.mapPointRoute.mapPoint.ItemPop.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
						CS$<>8__locals2.mapPointRoute.mapPoint.Icon_Item.Setup(questUIMapInfo.pickupRewardItem.staticData, new IconItemCtrl.SetupParam
						{
							useFrame = false
						});
					}
				}
			}
		}
		this.UpdateRouteColor(playableMapIdList);
		SelEventLargeScaleCtrl.MapPointRouteGUI mapPointRouteGUI = null;
		if (playableMapIdList.Exists((int item) => item == CS$<>8__locals1.selectData.mapId))
		{
			QuestStaticMap mapData = mapDataList.Find((QuestStaticMap item) => item.mapId == CS$<>8__locals1.selectData.mapId);
			if (mapData != null)
			{
				mapPointRouteGUI = this.MapData.mapPointRoutes.Find((SelEventLargeScaleCtrl.MapPointRouteGUI item) => item.GetName() == mapData.mapObjName);
			}
		}
		if (mapPointRouteGUI == null)
		{
			mapPointRouteGUI = this.MapData.mapPointRoutes[0];
		}
		SelEventLargeScaleCtrl.MapPointRouteGUI mapPointRouteGUI2 = mapPointRouteGUI;
		this.GuiData.mapData.mapCar.transform.position = mapPointRouteGUI2.mapPoint.pointObj.transform.position;
		this.GuiData.mapData.SetCarObjType((chapterId == 1004 || chapterId == 2004) ? QuestCarType.FLICKY : QuestCarType.BUS);
		this.GuiData.mapData.PlayCarAnim();
		Vector3 localPosition = this.GuiData.mapData.mapObj.transform.localPosition;
		this.GuiData.mapData.mapObj.transform.localPosition = new Vector3(0f, 0f, 0f);
		Vector3 localPosition2 = this.GuiData.mapData.bgObj.transform.localPosition;
		this.GuiData.mapData.bgObj.transform.localPosition = new Vector3(0f, 0f, 0f);
		RectTransform rectTransform = this.GuiData.mapData.mapCar.transform as RectTransform;
		Vector3 vector = new Vector3(rectTransform.anchoredPosition.x - (float)(Screen.width / 2) * (QuestUtil.DEFAULT_SCREEN_WIDTH / (float)Screen.width), rectTransform.anchoredPosition.y + (float)(Screen.height / 2) * (QuestUtil.DEFAULT_SCREEN_HEIGHT / (float)Screen.height));
		this.GuiData.mapData.mapObj.transform.localPosition -= vector;
		this.GuiData.mapData.bgObj.transform.localPosition -= vector;
		this.MapData.bgObj.transform.Find("Tex_Bg").GetComponent<RectTransform>().GetComponent<PguiRawImageCtrl>()
			.SetRawImage(this.MapBgFilePath, true, false, null);
		this.GuiData.mapData.mapObj.transform.localPosition = this.AdjustPosition(this.GuiData.mapData.mapObj.transform.localPosition);
		this.GuiData.mapData.bgObj.transform.localPosition = this.AdjustPosition(this.GuiData.mapData.bgObj.transform.localPosition);
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x0600178D RID: 6029 RVA: 0x001252A4 File Offset: 0x001234A4
	// (set) Token: 0x0600178E RID: 6030 RVA: 0x001252AC File Offset: 0x001234AC
	public bool TouchMoving { get; private set; }

	// Token: 0x0600178F RID: 6031 RVA: 0x001252B5 File Offset: 0x001234B5
	public bool IsNotNullMapObj()
	{
		return this.GuiData.mapData != null;
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x001252C5 File Offset: 0x001234C5
	public bool IsNotNullMapBaseObj()
	{
		return this.IsNotNullMapObj() && this.GuiData.mapData.baseObj != null;
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x001252E7 File Offset: 0x001234E7
	public void SetupMapOffset()
	{
		this.MapData.mapObj.GetComponent<RectTransform>().anchoredPosition = this.MapOffset;
		this.MapData.bgObj.GetComponent<RectTransform>().anchoredPosition = this.MapOffset;
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x00125320 File Offset: 0x00123520
	public static void PlayBGM(int eventId)
	{
		DataManagerEvent.LargeEventData largeEventData = DataManager.DmEvent.GetLargeEventData(eventId);
		if (largeEventData != null)
		{
			SoundManager.PlayBGM(largeEventData.BgmFilePath);
			return;
		}
		SoundManager.PlayBGM("prd_bgm0055");
	}

	// Token: 0x06001793 RID: 6035 RVA: 0x00125352 File Offset: 0x00123552
	public void PlayBGM()
	{
		SelEventLargeScaleCtrl.PlayBGM(this.setupParam.eventData.eventId);
	}

	// Token: 0x06001794 RID: 6036 RVA: 0x0012536C File Offset: 0x0012356C
	private static float GetSafeAreaX(GameObject mapBoxObject, SelEventLargeScaleCtrl selEventLargeScaleCtrl)
	{
		RectTransform rectTransform = mapBoxObject.transform as RectTransform;
		float x = SelEventLargeScaleCtrl.GetMapOffset(mapBoxObject, selEventLargeScaleCtrl.GuiData.mapData, selEventLargeScaleCtrl.MapRangeSize, selEventLargeScaleCtrl.setupParam.eventData.eventId).x;
		float num = 0f;
		float num2 = rectTransform.offsetMax.x * 2f - x;
		if (selEventLargeScaleCtrl.MapRangeSize.x > 0)
		{
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
		}
		return num2 + num;
	}

	// Token: 0x06001795 RID: 6037 RVA: 0x00125464 File Offset: 0x00123664
	private static float GetSafeAreaY(GameObject mapBoxObject)
	{
		if ((mapBoxObject.transform as RectTransform).offsetMin.x >= 0f)
		{
			return 0f;
		}
		return SelEventLargeScaleCtrl.MAP_MASK_IMAGE_HEIGHT;
	}

	// Token: 0x06001796 RID: 6038 RVA: 0x0012548D File Offset: 0x0012368D
	private void OnValidate()
	{
	}

	// Token: 0x06001797 RID: 6039 RVA: 0x0012548F File Offset: 0x0012368F
	private void Update()
	{
	}

	// Token: 0x06001798 RID: 6040 RVA: 0x00125494 File Offset: 0x00123694
	private bool IsExistsMapId(List<int> playableMapIdList, QuestStaticMap mapData)
	{
		return playableMapIdList.Exists((int item) => item == mapData.mapId);
	}

	// Token: 0x06001799 RID: 6041 RVA: 0x001254C0 File Offset: 0x001236C0
	private bool IsEnableGrayscale(QuestStaticMap mapData)
	{
		QuestUIMapInfo questUIMapInfo = QuestUIMapInfo.GetQuestUIMapInfo(mapData.mapId, SceneQuest.TimeStampInScene, this.setupParam.eventData.eventId);
		return SceneQuest.TimeStampInScene < questUIMapInfo.openTime;
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x001254FE File Offset: 0x001236FE
	private IEnumerator RequestOpenQuestByItem(SelEventLargeScaleCtrl.MapPointRouteGUI mapPointRoute, QuestStaticMap mapData)
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		mapPointRoute.mapPoint.Mark_LockReason.gameObject.SetActive(false);
		DataManager.DmQuest.RequestOpenQuestByItem(mapData.mapId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(this.setupParam.eventData.eventChapterId);
		this.UpdateMaterial(playableMapIdList, mapPointRoute, mapData);
		this.UpdateNewMark(mapPointRoute, mapData, playableMapIdList, true);
		this.UpdateRouteColor(playableMapIdList);
		this.SwitchRouteColor(mapPointRoute, playableMapIdList, mapData, false);
		yield return null;
		this.SwitchRouteColor(mapPointRoute, playableMapIdList, mapData, true);
		CanvasManager.SetEnableCmnTouchMask(false);
		yield break;
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x0012551C File Offset: 0x0012371C
	private void UpdateMaterial(List<int> playableMapIdList, SelEventLargeScaleCtrl.MapPointRouteGUI mapPointRoute, QuestStaticMap mapData)
	{
		bool flag = this.IsExistsMapId(playableMapIdList, mapData);
		bool flag2 = this.IsEnableGrayscale(mapData);
		mapPointRoute.mapPoint.Txt_MapName.transform.parent.GetComponent<PguiImageCtrl>().m_Image.material = ((flag2 || !flag) ? this.material : null);
		mapPointRoute.mapPoint.Tex.m_RawImage.material = ((flag2 || !flag) ? this.material : null);
		PguiGradientCtrl component = mapPointRoute.mapPoint.Txt_MapName.GetComponent<PguiGradientCtrl>();
		if (component != null)
		{
			Outline[] components = mapPointRoute.mapPoint.Txt_MapName.GetComponents<Outline>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].effectColor = component.GetOutlineById((flag2 || !flag) ? "Disable" : "Normal");
			}
		}
	}

	// Token: 0x0600179C RID: 6044 RVA: 0x001255F0 File Offset: 0x001237F0
	private void UpdateRouteColor(List<int> playableMapIdList)
	{
		List<QuestStaticMap> mapDataList = DataManager.DmQuest.QuestStaticData.chapterDataMap[this.setupParam.eventData.eventChapterId].mapDataList;
		foreach (SelEventLargeScaleCtrl.MapPointRouteGUI mapPointRouteGUI in this.MapData.mapPointRoutes)
		{
			foreach (BezierLine bezierLine in mapPointRouteGUI.routeList)
			{
				bezierLine.color = Color.gray;
			}
		}
		using (List<SelEventLargeScaleCtrl.MapPointRouteGUI>.Enumerator enumerator = this.MapData.mapPointRoutes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SelEventLargeScaleCtrl.MapPointRouteGUI e = enumerator.Current;
				QuestStaticMap mapData = mapDataList.Find((QuestStaticMap item) => item.mapObjName == e.GetName());
				if (mapData != null && playableMapIdList.Exists((int item) => item == mapData.mapId))
				{
					foreach (BezierLine bezierLine2 in e.routeList)
					{
						bezierLine2.color = Color.white;
					}
				}
			}
		}
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x00125788 File Offset: 0x00123988
	private void SwitchRouteColor(SelEventLargeScaleCtrl.MapPointRouteGUI mapPointRoute, List<int> playableMapIdList, QuestStaticMap mapData, bool sw)
	{
		if (playableMapIdList.Exists((int item) => item == mapData.mapId))
		{
			foreach (BezierLine bezierLine in mapPointRoute.routeList)
			{
				bezierLine.enabled = sw;
			}
		}
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x001257FC File Offset: 0x001239FC
	private void UpdateNewMark(SelEventLargeScaleCtrl.MapPointRouteGUI mapPointRoute, QuestStaticMap mapData, List<int> playableMapIdList, bool startMarkAE)
	{
		bool flag = this.IsExistsMapId(playableMapIdList, mapData);
		bool flag2 = this.IsEnableGrayscale(mapData);
		bool flag3 = false;
		mapPointRoute.mapPoint.Mark_NewPoint.gameObject.SetActive(false);
		bool flag4 = true;
		QuestStaticQuestGroup questStaticQuestGroup = mapData.questGroupList.Find((QuestStaticQuestGroup item) => item.mapId == mapData.mapId);
		int num = 0;
		for (int i = 0; i < questStaticQuestGroup.questOneList.Count; i++)
		{
			QuestStaticQuestOne questStaticQuestOne = questStaticQuestGroup.questOneList[i];
			QuestDynamicQuestOne questDynamicQuestOne = null;
			if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questStaticQuestOne.questId))
			{
				questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[questStaticQuestOne.questId];
			}
			QuestOneStatus questOneStatus = ((questDynamicQuestOne != null) ? questDynamicQuestOne.status : QuestOneStatus.INVALID);
			if (questOneStatus == QuestOneStatus.COMPLETE)
			{
				num++;
			}
			if (questDynamicQuestOne != null && questOneStatus != QuestOneStatus.NEW)
			{
				flag4 = false;
			}
		}
		string text = "DEFAULT";
		QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter item) => item.chapterId == mapData.chapterId);
		if (questStaticChapter != null)
		{
			if (questStaticChapter.MarkerType == 1)
			{
				text = "MUJI";
				if (flag4 && (flag2 || !flag))
				{
					flag4 = false;
				}
			}
			else
			{
				flag4 = false;
				if (playableMapIdList.Count > 0)
				{
					flag4 = mapData.mapId == playableMapIdList[playableMapIdList.Count - 1];
				}
				text = "DEFAULT";
			}
		}
		mapPointRoute.mapPoint.Mark_NewPoint.gameObject.SetActive(flag4);
		mapPointRoute.mapPoint.Mark_NewPoint.GetComponent<PguiReplaceAECtrl>().Replace(text);
		if (startMarkAE)
		{
			mapPointRoute.mapPoint.Mark_NewPoint.PlayAnimation(PguiAECtrl.AmimeType.START, null);
		}
		else
		{
			mapPointRoute.mapPoint.Mark_NewPoint.PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
		}
		mapPointRoute.mapPoint.Mark_Complete.SetActive(num >= questStaticQuestGroup.questOneList.Count);
		mapPointRoute.SetActive(!mapData.StartHideFlag || playableMapIdList.Exists((int item) => item == mapData.mapId), flag3);
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x00125A1F File Offset: 0x00123C1F
	private Vector3 AdjustPosition(Vector3 pos)
	{
		return SelEventLargeScaleCtrl.AdjustPosition(pos, this.MapRangeSize, this.mapBoxObject, this.MapOffset, this);
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x00125A3C File Offset: 0x00123C3C
	public static Vector3 AdjustPosition(Vector3 pos, Vector2Int MapRangeSize, GameObject mapBoxObject, Vector2 MapOffset, SelEventLargeScaleCtrl selEventLargeScaleCtrl)
	{
		float num = (float)MapRangeSize.x;
		float num2 = (float)MapRangeSize.y;
		float safeAreaX = SelEventLargeScaleCtrl.GetSafeAreaX(mapBoxObject, selEventLargeScaleCtrl);
		if (pos.x < -num + safeAreaX)
		{
			pos.x = -num + safeAreaX;
		}
		if (pos.x > MapOffset.x)
		{
			pos.x = MapOffset.x;
		}
		float safeAreaY = SelEventLargeScaleCtrl.GetSafeAreaY(mapBoxObject);
		if (pos.y > num2 - safeAreaY)
		{
			pos.y = num2 - safeAreaY;
		}
		if (pos.y < MapOffset.y)
		{
			pos.y = MapOffset.y;
		}
		return pos;
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x00125AD0 File Offset: 0x00123CD0
	private void OnClickButton(PguiButtonCtrl button)
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.setupParam.eventData.eventId);
		if (eventData == null)
		{
			return;
		}
		if (button == this.GuiData.pointSelect.Btn_ShopEvent)
		{
			this.initParam.reqShopSequenceCB();
			return;
		}
		if (button == this.GuiData.pointSelect.Btn_Gacha)
		{
			SceneGacha.OpenParam openParam = new SceneGacha.OpenParam
			{
				gachaId = this.setupParam.eventData.eventGachaId,
				resultNextSceneName = SceneManager.SceneName.SceneQuest,
				resultNextSceneArgs = new SceneQuest.Args
				{
					selectEventId = eventData.eventId,
					category = QuestStaticChapter.Category.EVENT,
					backSequenceGameObject = this.GuiData.pointSelect.baseObj
				}
			};
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneGacha, openParam);
			return;
		}
		if (button == this.GuiData.pointSelect.Btn_Mission)
		{
			SceneMission.MissionOpenParam missionOpenParam = new SceneMission.MissionOpenParam(MissionType.EVENTTOTAL, eventData.eventId)
			{
				returnSceneName = SceneManager.SceneName.SceneQuest,
				resultNextSceneArgs = new SceneQuest.Args
				{
					selectEventId = eventData.eventId,
					category = QuestStaticChapter.Category.EVENT,
					backSequenceGameObject = this.GuiData.pointSelect.baseObj
				}
			};
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneMission, missionOpenParam);
		}
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x00125C10 File Offset: 0x00123E10
	private void OnClickEventInfoBanner(Transform tf)
	{
		QuestUtil.OpenBannerWebViewWindow(this.setupParam.eventData.eventId);
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x00125C28 File Offset: 0x00123E28
	public void OnTouchMove(Info info)
	{
		if (this.setupParam.checkChapterSelectSequenceCB != null && this.setupParam.checkChapterSelectSequenceCB())
		{
			return;
		}
		if (this.MapData == null)
		{
			return;
		}
		if (this.MapData.mapObj == null)
		{
			return;
		}
		if ((info.CurrentPosition - info.InitPosition).sqrMagnitude > QuestUtil.MOVING_SQR_MAGNITUDE)
		{
			this.TouchMoving = true;
		}
		Vector2 vector = this.MapData.mapObj.transform.localPosition;
		Vector2 vector2 = this.MapData.bgObj.transform.localPosition;
		RectTransform component = this.MapData.bgObj.transform.Find("Tex_Bg").GetComponent<RectTransform>();
		float num = component.sizeDelta.x * 0.5f;
		float num2 = component.sizeDelta.y * 0.5f;
		Transform transform = this.mapBoxObject.transform;
		if (this.MapDirection.x != 0)
		{
			vector.x += info.DeltaPosition.x * (num / (float)Screen.width);
			vector2.x += info.DeltaPosition.x * (num / (float)Screen.width);
		}
		if (this.MapDirection.y != 0)
		{
			vector.y += info.DeltaPosition.y * (num2 / (float)Screen.height);
			vector2.y += info.DeltaPosition.y * (num2 / (float)Screen.height);
		}
		this.MapData.mapObj.transform.localPosition = this.AdjustPosition(vector);
		this.MapData.bgObj.transform.localPosition = this.AdjustPosition(vector2);
	}

	// Token: 0x060017A4 RID: 6052 RVA: 0x00125DFE File Offset: 0x00123FFE
	public void OnTouchRelease(Info info)
	{
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x00125E00 File Offset: 0x00124000
	public void OnTouchStart(Info info)
	{
		this.TouchMoving = false;
	}

	// Token: 0x0400126A RID: 4714
	private static int pathEventChapterId = 0;

	// Token: 0x0400126B RID: 4715
	private static readonly int LockByTimeReplaceId = 1;

	// Token: 0x0400126C RID: 4716
	private static readonly int LockByFreeKeyReplaceId = 2;

	// Token: 0x0400126D RID: 4717
	private static readonly int LockByPaidKeyReplaceId = 3;

	// Token: 0x0400126E RID: 4718
	public static readonly float DEFAULT_SCREEN_HEIGHT = 1760f;

	// Token: 0x0400126F RID: 4719
	public static readonly float DEFAULT_SCREEN_WIDTH = QuestUtil.DEFAULT_SCREEN_WIDTH;

	// Token: 0x04001270 RID: 4720
	private static float MAP_MASK_IMAGE_HEIGHT = SelEventLargeScaleCtrl.DEFAULT_SCREEN_HEIGHT - SelEventLargeScaleCtrl.DEFAULT_SCREEN_HEIGHT;

	// Token: 0x04001271 RID: 4721
	private static float MAP_MASK_IMAGE_WIDTH = SelEventLargeScaleCtrl.DEFAULT_SCREEN_WIDTH - SelEventLargeScaleCtrl.DEFAULT_SCREEN_WIDTH;

	// Token: 0x04001273 RID: 4723
	private SelEventLargeScaleCtrl.SetupParam setupParam = new SelEventLargeScaleCtrl.SetupParam();

	// Token: 0x04001274 RID: 4724
	private SelEventLargeScaleCtrl.InitParam initParam = new SelEventLargeScaleCtrl.InitParam();

	// Token: 0x04001275 RID: 4725
	private GameObject mapBoxObject;

	// Token: 0x04001276 RID: 4726
	private Material material;

	// Token: 0x02000CF3 RID: 3315
	// (Invoke) Token: 0x060047BF RID: 18367
	public delegate bool OnCheck();

	// Token: 0x02000CF4 RID: 3316
	public class InitParam
	{
		// Token: 0x04004CFF RID: 19711
		public UnityAction reqNextSequenceCB;

		// Token: 0x04004D00 RID: 19712
		public UnityAction reqBackSequenceCB;

		// Token: 0x04004D01 RID: 19713
		public UnityAction reqShopSequenceCB;
	}

	// Token: 0x02000CF5 RID: 3317
	public class SetupParam
	{
		// Token: 0x04004D02 RID: 19714
		public DataManagerEvent.EventData eventData;

		// Token: 0x04004D03 RID: 19715
		public SelEventLargeScaleCtrl.OnCheck checkChapterSelectSequenceCB;
	}

	// Token: 0x02000CF6 RID: 3318
	public class GUI
	{
		// Token: 0x060047C4 RID: 18372 RVA: 0x002197E3 File Offset: 0x002179E3
		public GUI(Transform baseTr)
		{
			this.pointSelect = new SelEventLargeScaleCtrl.PointSelectGUI(baseTr.transform);
			this.pointSelect.baseObj.SetActive(false);
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x0021980D File Offset: 0x00217A0D
		public void SetupMapData(GameObject mapObj, List<GameObject> carObj)
		{
			this.mapData = new SelEventLargeScaleCtrl.MapDataGUI(mapObj.transform, carObj);
		}

		// Token: 0x04004D04 RID: 19716
		public SelEventLargeScaleCtrl.PointSelectGUI pointSelect;

		// Token: 0x04004D05 RID: 19717
		public SelEventLargeScaleCtrl.MapDataGUI mapData;
	}

	// Token: 0x02000CF7 RID: 3319
	public class PointSelectGUI
	{
		// Token: 0x060047C6 RID: 18374 RVA: 0x00219824 File Offset: 0x00217A24
		public PointSelectGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_ShopEvent = baseTr.Find("Right/EventInfo/Btn_ShopEvent").GetComponent<PguiButtonCtrl>();
			this.Btn_Gacha = baseTr.Find("Right/EventInfo/Btn_Gacha").GetComponent<PguiButtonCtrl>();
			this.EventBanner = baseTr.Find("Left/EventBanner").GetComponent<PguiRawImageCtrl>();
			this.Txt_Term = baseTr.Find("Left/EventTerm/Txt_Term").GetComponent<PguiTextCtrl>();
			this.Txt_Term.text = "";
			this.GUI_Event_Map_PointSelect = baseTr.GetComponent<SimpleAnimation>();
			this.itemOwnBases = new List<SelEventLargeScaleCtrl.PointSelectGUI.ItemOwnBase>
			{
				new SelEventLargeScaleCtrl.PointSelectGUI.ItemOwnBase(baseTr.Find("Right/EventInfo/ItemOwnBase01")),
				new SelEventLargeScaleCtrl.PointSelectGUI.ItemOwnBase(baseTr.Find("Right/EventInfo/ItemOwnBase02"))
			};
			this.Btn_Mission = baseTr.Find("Right/Btn_Mission").GetComponent<PguiButtonCtrl>();
			this.Btn_Mission.transform.Find("BaseImage/Mark_New").gameObject.SetActive(false);
			if (this.Btn_Mission.transform.Find("BaseImage/Badge/Cmn_Badge/Num") == null)
			{
				this.Btn_Mission.transform.Find("BaseImage/Badge").GetComponent<PguiNestPrefab>().InitForce();
			}
			this.Txt_Mission_Num = this.Btn_Mission.transform.Find("BaseImage/Badge/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x04004D06 RID: 19718
		public GameObject baseObj;

		// Token: 0x04004D07 RID: 19719
		public PguiButtonCtrl Btn_ShopEvent;

		// Token: 0x04004D08 RID: 19720
		public PguiButtonCtrl Btn_Gacha;

		// Token: 0x04004D09 RID: 19721
		public PguiRawImageCtrl EventBanner;

		// Token: 0x04004D0A RID: 19722
		public PguiTextCtrl Txt_Term;

		// Token: 0x04004D0B RID: 19723
		public SimpleAnimation GUI_Event_Map_PointSelect;

		// Token: 0x04004D0C RID: 19724
		public List<SelEventLargeScaleCtrl.PointSelectGUI.ItemOwnBase> itemOwnBases;

		// Token: 0x04004D0D RID: 19725
		public PguiButtonCtrl Btn_Mission;

		// Token: 0x04004D0E RID: 19726
		public PguiTextCtrl Txt_Mission_Num;

		// Token: 0x020011BD RID: 4541
		public class ItemOwnBase
		{
			// Token: 0x060056FF RID: 22271 RVA: 0x0025546D File Offset: 0x0025366D
			public ItemOwnBase(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Num_Txt = baseTr.Find("Num_Txt").GetComponent<PguiTextCtrl>();
				this.Icon_Stone = baseTr.Find("Icon_Stone").GetComponent<PguiRawImageCtrl>();
			}

			// Token: 0x0400614E RID: 24910
			public GameObject baseObj;

			// Token: 0x0400614F RID: 24911
			public PguiRawImageCtrl Icon_Stone;

			// Token: 0x04006150 RID: 24912
			public PguiTextCtrl Num_Txt;
		}
	}

	// Token: 0x02000CF8 RID: 3320
	public class MapDataGUI : SceneQuest.IMapData
	{
		// Token: 0x060047C7 RID: 18375 RVA: 0x00219980 File Offset: 0x00217B80
		public MapDataGUI(Transform baseTr, List<GameObject> carObjList)
			: base(baseTr, carObjList)
		{
			this.SetupMapData();
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x0021999B File Offset: 0x00217B9B
		private void Setup(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x002199AC File Offset: 0x00217BAC
		public void SetupMapData()
		{
			if (this.mapPointRoutes.Count > 0)
			{
				for (int i = 0; i < this.mapPointRoutes.Count; i++)
				{
					Object.Destroy(this.mapPointRoutes[i].mapPoint.baseObj);
					this.mapPointRoutes[i].mapPoint = null;
				}
			}
			this.mapPointRoutes.Clear();
			this.mapObj = this.baseObj.transform.Find("Point").gameObject;
			this.mapObj.SetActive(true);
			this.bgObj = this.baseObj.transform.Find("Bg").gameObject;
			List<Transform> list = new List<Transform>(this.mapObj.transform.GetComponentsInChildren<Transform>()).FindAll((Transform item) => Regex.IsMatch(item.name, "^Route[0-9]"));
			list.Sort((Transform a, Transform b) => int.Parse(Regex.Match(a.name, "[0-9]").Value) - int.Parse(Regex.Match(b.name, "[0-9]").Value));
			foreach (Transform transform in list)
			{
				foreach (Transform transform2 in new List<Transform>(this.mapObj.transform.Find(transform.name + "/Tex").GetComponentsInChildren<Transform>()).FindAll((Transform item) => Regex.IsMatch(item.name, "[0-9]*_[0-9][0-9][0-9]*")))
				{
					string[] array = transform2.name.Split('_', StringSplitOptions.None);
					if (array.Length >= 2)
					{
						int num = int.Parse(array[1]);
						GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/Event_MapPoint"), transform2);
						this.mapPointRoutes.Add(new SelEventLargeScaleCtrl.MapPointRouteGUI(gameObject.transform, num));
					}
				}
			}
			foreach (GameObject gameObject2 in this.carObjList)
			{
				gameObject2.transform.SetParent(this.mapObj.transform, false);
			}
		}

		// Token: 0x04004D0F RID: 19727
		public List<SelEventLargeScaleCtrl.MapPointRouteGUI> mapPointRoutes = new List<SelEventLargeScaleCtrl.MapPointRouteGUI>();
	}

	// Token: 0x02000CF9 RID: 3321
	public class MapPointRouteGUI
	{
		// Token: 0x060047CA RID: 18378 RVA: 0x00219C28 File Offset: 0x00217E28
		public MapPointRouteGUI(Transform baseTr, int num)
		{
			this.baseObj = baseTr.gameObject;
			this.mapPoint = new SelEventLargeScaleCtrl.MapPointGUI(baseTr, num);
			BezierLine[] componentsInChildren = baseTr.parent.parent.parent.Find("Line").GetComponentsInChildren<BezierLine>();
			if (componentsInChildren != null)
			{
				foreach (BezierLine bezierLine in componentsInChildren)
				{
					if (bezierLine.m_end == this.baseObj.transform.parent.gameObject)
					{
						this.routeList.Add(bezierLine);
					}
				}
			}
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x00219CC4 File Offset: 0x00217EC4
		public void SetActive(bool sw, bool startMarkAE)
		{
			this.baseObj.SetActive(sw);
			this.mapPoint.SetActive(sw, startMarkAE);
			foreach (BezierLine bezierLine in this.routeList)
			{
				bezierLine.gameObject.SetActive(sw);
			}
		}

		// Token: 0x060047CC RID: 18380 RVA: 0x00219D34 File Offset: 0x00217F34
		public string GetName()
		{
			return this.baseObj.transform.parent.name;
		}

		// Token: 0x04004D10 RID: 19728
		public GameObject baseObj;

		// Token: 0x04004D11 RID: 19729
		public SelEventLargeScaleCtrl.MapPointGUI mapPoint;

		// Token: 0x04004D12 RID: 19730
		public List<BezierLine> routeList = new List<BezierLine>();
	}

	// Token: 0x02000CFA RID: 3322
	public class MapPointGUI
	{
		// Token: 0x060047CD RID: 18381 RVA: 0x00219D4C File Offset: 0x00217F4C
		public MapPointGUI(Transform baseTr, int num)
		{
			this.baseObj = baseTr.gameObject;
			this.baseRtf = baseTr as RectTransform;
			this.pointObj = this.baseObj;
			this.pointRtf.AddComponent<RectTransform>();
			this.pointRtf.transform.SetParent(this.pointObj.transform, true);
			this.pointRtf.transform.localPosition = new Vector3(0f, 0f, 0f);
			this.pointRtf.transform.localScale = new Vector3(1f, 1f, 1f);
			this.Mark_NewPoint = baseTr.Find("Mark_NewPoint").GetComponent<PguiAECtrl>();
			this.Mark_NewPoint.gameObject.SetActive(false);
			this.Tex = baseTr.Find("Tex").GetComponent<PguiRawImageCtrl>();
			this.Tex.SetRawImage(string.Concat(new string[]
			{
				"Texture2D/EventMap_PointIcon/",
				SelEventLargeScaleCtrl.pathEventChapterId.ToString(),
				"/event_mapicon_",
				SelEventLargeScaleCtrl.pathEventChapterId.ToString(),
				"_",
				num.ToString("D3")
			}), true, true, null);
			this.AEImage = baseTr.Find("AEImage").GetComponent<PguiAECtrl>();
			this.Txt_MapName = baseTr.Find("Img_NameBase/Txt_MapName").GetComponent<PguiTextCtrl>();
			Resources.Load("SceneQuest/GUI/Prefab/Quest_Memori_MissionComp");
			this.Mark_Complete = baseTr.Find("Mark_Complete").gameObject;
			this.Mark_Complete.SetActive(false);
			this.markLockCtrl = baseTr.Find("Mark_Lock").GetComponent<MarkLockCtrl>();
			this.Mark_LockReason = baseTr.Find("Mark_LockReason").GetComponent<PguiReplaceSpriteCtrl>();
			this.ItemPop = baseTr.Find("ItemPop").GetComponent<SimpleAnimation>();
			this.Icon_Item = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, this.ItemPop.transform.Find("Icon_Item")).GetComponent<IconItemCtrl>();
		}

		// Token: 0x060047CE RID: 18382 RVA: 0x00219F5F File Offset: 0x0021815F
		public void SetActive(bool sw, bool startMarkAE)
		{
			this.baseObj.SetActive(sw);
			if (startMarkAE)
			{
				this.AEImage.PlayAnimation(PguiAECtrl.AmimeType.START, null);
				return;
			}
			this.AEImage.PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
		}

		// Token: 0x04004D13 RID: 19731
		public GameObject baseObj;

		// Token: 0x04004D14 RID: 19732
		public RectTransform baseRtf;

		// Token: 0x04004D15 RID: 19733
		public GameObject pointObj;

		// Token: 0x04004D16 RID: 19734
		public GameObject pointRtf = new GameObject();

		// Token: 0x04004D17 RID: 19735
		public PguiRawImageCtrl Tex;

		// Token: 0x04004D18 RID: 19736
		public PguiAECtrl Mark_NewPoint;

		// Token: 0x04004D19 RID: 19737
		public PguiTextCtrl Txt_MapName;

		// Token: 0x04004D1A RID: 19738
		public GameObject Mark_Complete;

		// Token: 0x04004D1B RID: 19739
		public PguiAECtrl AEImage;

		// Token: 0x04004D1C RID: 19740
		public MarkLockCtrl markLockCtrl;

		// Token: 0x04004D1D RID: 19741
		public PguiReplaceSpriteCtrl Mark_LockReason;

		// Token: 0x04004D1E RID: 19742
		public SimpleAnimation ItemPop;

		// Token: 0x04004D1F RID: 19743
		public IconItemCtrl Icon_Item;
	}
}
