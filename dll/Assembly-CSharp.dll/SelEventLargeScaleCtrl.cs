using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SGNFW.Common;
using SGNFW.Touch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelEventLargeScaleCtrl : MonoBehaviour
{
	public static void SetPathEventChapterId(int id)
	{
	}

	public static int GetPathEventChapterId()
	{
		return SelEventLargeScaleCtrl.pathEventChapterId;
	}

	public SelEventLargeScaleCtrl.GUI GuiData { get; private set; }

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

	private Vector2 MapOffset
	{
		get
		{
			return SelEventLargeScaleCtrl.GetMapOffset(this.mapBoxObject, this.GuiData.mapData, this.MapRangeSize, this.setupParam.eventData.eventId);
		}
	}

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

	public void AdjustMaskPostion()
	{
		SelEventLargeScaleCtrl.AdjustMaskPostion(this.GuiData.mapData, this.MapOffset);
	}

	public static void AdjustMaskPostion(SelEventLargeScaleCtrl.MapDataGUI mapDataGUI, Vector2 MapOffset)
	{
		float num = ((mapDataGUI.bgObj.transform.Find("Tex_Bg").localPosition.x == 0f) ? 0f : (mapDataGUI.bgObj.transform.Find("Tex_Bg").localPosition.x + QuestUtil.MAP_MASK_IMAGE_WIDTH));
		GameObject gameObject = mapDataGUI.baseObj.transform.Find("Map_Frame").gameObject;
		Vector3 localPosition = gameObject.transform.localPosition;
		gameObject.transform.localPosition = new Vector3(MapOffset.x + num, localPosition.y, 0f);
	}

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

	private static List<string> GetTipsFilePath(int eventId)
	{
		DataManagerEvent.LargeEventData largeEventData = DataManager.DmEvent.GetLargeEventData(eventId);
		if (largeEventData != null)
		{
			return largeEventData.TipsFilePath;
		}
		return new List<string> { "Texture2D/Tutorial_Window/Event_Map/001/tutorial_eventmap_001_01", "Texture2D/Tutorial_Window/Event_Map/001/tutorial_eventmap_001_02", "Texture2D/Tutorial_Window/Event_Map/001/tutorial_eventmap_001_03", "Texture2D/Tutorial_Window/Event_Map/001/tutorial_eventmap_001_04" };
	}

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

	public static void OpenHelpWindow(int eventId)
	{
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "イベントの遊びかた", SelEventLargeScaleCtrl.GetTipsFilePath(eventId), null);
	}

	public string LoadAssetPath
	{
		get
		{
			return this.MapFilePath;
		}
	}

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

	public void Dest()
	{
	}

	public void Destroy()
	{
	}

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

	public void RemoveTouchEventTriggger(int chapterId)
	{
		DataManager.DmQuest.GetPlayableMapIdList(chapterId);
		foreach (SelEventLargeScaleCtrl.MapPointRouteGUI mapPointRouteGUI in this.MapData.mapPointRoutes)
		{
			PrjUtil.RemoveTouchEventTrigger(mapPointRouteGUI.mapPoint.pointRtf.gameObject);
		}
	}

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

	public bool TouchMoving { get; private set; }

	public bool IsNotNullMapObj()
	{
		return this.GuiData.mapData != null;
	}

	public bool IsNotNullMapBaseObj()
	{
		return this.IsNotNullMapObj() && this.GuiData.mapData.baseObj != null;
	}

	public void SetupMapOffset()
	{
		this.MapData.mapObj.GetComponent<RectTransform>().anchoredPosition = this.MapOffset;
		this.MapData.bgObj.GetComponent<RectTransform>().anchoredPosition = this.MapOffset;
	}

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

	public void PlayBGM()
	{
		SelEventLargeScaleCtrl.PlayBGM(this.setupParam.eventData.eventId);
	}

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

	private static float GetSafeAreaY(GameObject mapBoxObject)
	{
		if ((mapBoxObject.transform as RectTransform).offsetMin.x >= 0f)
		{
			return 0f;
		}
		return SelEventLargeScaleCtrl.MAP_MASK_IMAGE_HEIGHT;
	}

	private void OnValidate()
	{
	}

	private void Update()
	{
	}

	private bool IsExistsMapId(List<int> playableMapIdList, QuestStaticMap mapData)
	{
		return playableMapIdList.Exists((int item) => item == mapData.mapId);
	}

	private bool IsEnableGrayscale(QuestStaticMap mapData)
	{
		QuestUIMapInfo questUIMapInfo = QuestUIMapInfo.GetQuestUIMapInfo(mapData.mapId, SceneQuest.TimeStampInScene, this.setupParam.eventData.eventId);
		return SceneQuest.TimeStampInScene < questUIMapInfo.openTime;
	}

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

	private Vector3 AdjustPosition(Vector3 pos)
	{
		return SelEventLargeScaleCtrl.AdjustPosition(pos, this.MapRangeSize, this.mapBoxObject, this.MapOffset, this);
	}

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

	private void OnClickEventInfoBanner(Transform tf)
	{
		QuestUtil.OpenBannerWebViewWindow(this.setupParam.eventData.eventId);
	}

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

	public void OnTouchRelease(Info info)
	{
	}

	public void OnTouchStart(Info info)
	{
		this.TouchMoving = false;
	}

	private static int pathEventChapterId = 0;

	private static readonly int LockByTimeReplaceId = 1;

	private static readonly int LockByFreeKeyReplaceId = 2;

	private static readonly int LockByPaidKeyReplaceId = 3;

	public static readonly float DEFAULT_SCREEN_HEIGHT = 1760f;

	public static readonly float DEFAULT_SCREEN_WIDTH = QuestUtil.DEFAULT_SCREEN_WIDTH;

	private static float MAP_MASK_IMAGE_HEIGHT = SelEventLargeScaleCtrl.DEFAULT_SCREEN_HEIGHT - SelEventLargeScaleCtrl.DEFAULT_SCREEN_HEIGHT;

	private static float MAP_MASK_IMAGE_WIDTH = SelEventLargeScaleCtrl.DEFAULT_SCREEN_WIDTH - SelEventLargeScaleCtrl.DEFAULT_SCREEN_WIDTH;

	private SelEventLargeScaleCtrl.SetupParam setupParam = new SelEventLargeScaleCtrl.SetupParam();

	private SelEventLargeScaleCtrl.InitParam initParam = new SelEventLargeScaleCtrl.InitParam();

	private GameObject mapBoxObject;

	private Material material;

	public delegate bool OnCheck();

	public class InitParam
	{
		public UnityAction reqNextSequenceCB;

		public UnityAction reqBackSequenceCB;

		public UnityAction reqShopSequenceCB;
	}

	public class SetupParam
	{
		public DataManagerEvent.EventData eventData;

		public SelEventLargeScaleCtrl.OnCheck checkChapterSelectSequenceCB;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.pointSelect = new SelEventLargeScaleCtrl.PointSelectGUI(baseTr.transform);
			this.pointSelect.baseObj.SetActive(false);
		}

		public void SetupMapData(GameObject mapObj, List<GameObject> carObj)
		{
			this.mapData = new SelEventLargeScaleCtrl.MapDataGUI(mapObj.transform, carObj);
		}

		public SelEventLargeScaleCtrl.PointSelectGUI pointSelect;

		public SelEventLargeScaleCtrl.MapDataGUI mapData;
	}

	public class PointSelectGUI
	{
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

		public GameObject baseObj;

		public PguiButtonCtrl Btn_ShopEvent;

		public PguiButtonCtrl Btn_Gacha;

		public PguiRawImageCtrl EventBanner;

		public PguiTextCtrl Txt_Term;

		public SimpleAnimation GUI_Event_Map_PointSelect;

		public List<SelEventLargeScaleCtrl.PointSelectGUI.ItemOwnBase> itemOwnBases;

		public PguiButtonCtrl Btn_Mission;

		public PguiTextCtrl Txt_Mission_Num;

		public class ItemOwnBase
		{
			public ItemOwnBase(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Num_Txt = baseTr.Find("Num_Txt").GetComponent<PguiTextCtrl>();
				this.Icon_Stone = baseTr.Find("Icon_Stone").GetComponent<PguiRawImageCtrl>();
			}

			public GameObject baseObj;

			public PguiRawImageCtrl Icon_Stone;

			public PguiTextCtrl Num_Txt;
		}
	}

	public class MapDataGUI : SceneQuest.IMapData
	{
		public MapDataGUI(Transform baseTr, List<GameObject> carObjList)
			: base(baseTr, carObjList)
		{
			this.SetupMapData();
		}

		private void Setup(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
		}

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

		public List<SelEventLargeScaleCtrl.MapPointRouteGUI> mapPointRoutes = new List<SelEventLargeScaleCtrl.MapPointRouteGUI>();
	}

	public class MapPointRouteGUI
	{
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

		public void SetActive(bool sw, bool startMarkAE)
		{
			this.baseObj.SetActive(sw);
			this.mapPoint.SetActive(sw, startMarkAE);
			foreach (BezierLine bezierLine in this.routeList)
			{
				bezierLine.gameObject.SetActive(sw);
			}
		}

		public string GetName()
		{
			return this.baseObj.transform.parent.name;
		}

		public GameObject baseObj;

		public SelEventLargeScaleCtrl.MapPointGUI mapPoint;

		public List<BezierLine> routeList = new List<BezierLine>();
	}

	public class MapPointGUI
	{
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

		public GameObject baseObj;

		public RectTransform baseRtf;

		public GameObject pointObj;

		public GameObject pointRtf = new GameObject();

		public PguiRawImageCtrl Tex;

		public PguiAECtrl Mark_NewPoint;

		public PguiTextCtrl Txt_MapName;

		public GameObject Mark_Complete;

		public PguiAECtrl AEImage;

		public MarkLockCtrl markLockCtrl;

		public PguiReplaceSpriteCtrl Mark_LockReason;

		public SimpleAnimation ItemPop;

		public IconItemCtrl Icon_Item;
	}
}
