using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using AEAuth3;
using CriWare;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.Login;
using SGNFW.Touch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
	private static Canvas MainCanvasOverlay { get; set; }

	private static Canvas MainCanvasSystem { get; set; }

	private static Canvas MainCanvasFront { get; set; }

	private static Canvas MainCanvasBack { get; set; }

	public Transform SystemMiddleArea { get; private set; }

	public Transform OverlayWindowPanel { get; private set; }

	public bool IsInitialized { get; private set; }

	public GameObject SystemPanel { get; private set; }

	public GameObject CommonWindow { get; private set; }

	public List<float> CurrentFadeTimes
	{
		get
		{
			return this.currentFadeTimes;
		}
		set
		{
			this.currentFadeTimes = value;
		}
	}

	private List<GameObject> BgObjectPoolList { get; set; }

	public static void Initialize()
	{
		if (Singleton<CanvasManager>.Instance.IsInitialized)
		{
			return;
		}
		Singleton<CanvasManager>.Instance.CanvasCreate();
		Singleton<CanvasManager>.Instance.CanvasOverlayInstantiate();
		Singleton<CanvasManager>.Instance.CanvasSystemInstantiate();
		Singleton<CanvasManager>.Instance.fadeStatus = CanvasManager.FadeStatus.FADE_IN_FINISH;
		Singleton<CanvasManager>.Instance.IsInitialized = true;
	}

	public static void CanvasDestroy()
	{
		Singleton<CanvasManager>.Instance.IsInitialized = false;
		CanvasManager.RecursiveDestroy(CanvasManager.MainCanvasOverlay.gameObject);
		Object.Destroy(CanvasManager.MainCanvasOverlay.gameObject);
		CanvasManager.RecursiveDestroy(CanvasManager.MainCanvasSystem.gameObject);
		Object.Destroy(CanvasManager.MainCanvasSystem.gameObject);
		CanvasManager.RecursiveDestroy(CanvasManager.MainCanvasFront.gameObject);
		Object.Destroy(CanvasManager.MainCanvasFront.gameObject);
		CanvasManager.RecursiveDestroy(CanvasManager.MainCanvasBack.gameObject);
		Object.Destroy(CanvasManager.MainCanvasBack.gameObject);
	}

	public static void RecursiveDestroy(GameObject go)
	{
		foreach (object obj in go.transform)
		{
			Transform transform = (Transform)obj;
			CanvasManager.RecursiveDestroy(transform.gameObject);
			Object.Destroy(transform.gameObject);
		}
	}

	private GameObject CanvasInstantiate(string canvasName, Vector2 ancPos)
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("prefab/PguiCanvas"));
		gameObject.name = canvasName;
		(gameObject.transform as RectTransform).anchoredPosition = ancPos;
		return gameObject;
	}

	private void SetRectTransformStretch(RectTransform rt)
	{
		if (null == rt)
		{
			return;
		}
		rt.anchorMin = Vector2.zero;
		rt.anchorMax = Vector2.one;
		rt.offsetMax = Vector2.zero;
		rt.offsetMin = Vector2.zero;
	}

	private void CanvasCreate()
	{
		SceneManager.CanvasType canvasType = SceneManager.CanvasType.BACK;
		GameObject gameObject = Singleton<CanvasManager>.Instance.CanvasInstantiate("MainCanvasBack", new Vector2(0f, 0f));
		Camera worldCamera = gameObject.GetComponent<Canvas>().worldCamera;
		worldCamera.clearFlags = CameraClearFlags.Color;
		worldCamera.backgroundColor = Color.black;
		worldCamera.depth = (float)SceneManager.CameraDepth[canvasType];
		Singleton<SceneManager>.Instance.SetBaseCanvas(canvasType, gameObject.transform, worldCamera);
		CanvasManager.MainCanvasBack = gameObject.GetComponent<Canvas>();
		SceneManager.CanvasType canvasType2 = SceneManager.CanvasType.FRONT;
		GameObject gameObject2 = Singleton<CanvasManager>.Instance.CanvasInstantiate("MainCanvasFront", new Vector2(0f, 250f));
		Camera worldCamera2 = gameObject2.GetComponent<Canvas>().worldCamera;
		worldCamera2.clearFlags = CameraClearFlags.Depth;
		worldCamera2.depth = (float)SceneManager.CameraDepth[canvasType2];
		Singleton<SceneManager>.Instance.SetBaseCanvas(canvasType2, gameObject2.transform, worldCamera2);
		CanvasManager.MainCanvasFront = gameObject2.GetComponent<Canvas>();
		SceneManager.CanvasType canvasType3 = SceneManager.CanvasType.SYSTEM;
		GameObject gameObject3 = Singleton<CanvasManager>.Instance.CanvasInstantiate("MainCanvasSystem", new Vector2(0f, 500f));
		Camera worldCamera3 = gameObject3.GetComponent<Canvas>().worldCamera;
		worldCamera3.clearFlags = CameraClearFlags.Depth;
		worldCamera3.depth = (float)SceneManager.CameraDepth[canvasType3];
		Singleton<SceneManager>.Instance.SetBaseCanvas(canvasType3, gameObject3.transform, worldCamera3);
		this.SystemPanel = new GameObject();
		this.SystemPanel.name = "SystemPanel";
		this.SystemPanel.transform.SetParent(gameObject3.transform, true);
		this.SystemPanel.transform.localScale = Vector3.one;
		Singleton<CanvasManager>.Instance.SetRectTransformStretch(this.SystemPanel.AddComponent<RectTransform>());
		GameObject gameObject4 = new GameObject();
		gameObject4.name = "MiddleArea";
		gameObject4.transform.SetParent(this.SystemPanel.transform, true);
		gameObject4.transform.localScale = Vector3.one;
		gameObject4.AddComponent<CanvasRenderer>();
		Singleton<CanvasManager>.Instance.SetRectTransformStretch(gameObject4.AddComponent<RectTransform>());
		gameObject4.AddComponent<SafeAreaScaler>();
		this.SystemPanel.transform.SetParent(gameObject3.transform, true);
		this.SystemPanel.transform.localScale = Vector3.one;
		RectTransform rectTransform = this.SystemPanel.transform as RectTransform;
		rectTransform.offsetMax = Vector2.zero;
		rectTransform.offsetMin = Vector2.zero;
		CanvasManager.MainCanvasSystem = gameObject3.GetComponent<Canvas>();
		SceneManager.CanvasType canvasType4 = SceneManager.CanvasType.OVERLAY;
		GameObject gameObject5 = Singleton<CanvasManager>.Instance.CanvasInstantiate("MainCanvasOverlay", new Vector2(0f, 750f));
		Camera worldCamera4 = gameObject5.GetComponent<Canvas>().worldCamera;
		worldCamera4.clearFlags = CameraClearFlags.Depth;
		worldCamera4.depth = (float)SceneManager.CameraDepth[canvasType4];
		Singleton<SceneManager>.Instance.SetBaseCanvas(canvasType4, gameObject5.transform, worldCamera4);
		GameObject gameObject6 = new GameObject();
		gameObject6.name = "WindowPanel";
		gameObject6.transform.SetParent(gameObject5.transform, true);
		gameObject6.transform.localScale = Vector3.one;
		Singleton<CanvasManager>.Instance.SetRectTransformStretch(gameObject6.AddComponent<RectTransform>());
		this.OverlayWindowPanel = gameObject6.transform;
		GameObject gameObject7 = new GameObject();
		gameObject7.name = "LoadingPanel";
		gameObject7.transform.SetParent(gameObject5.transform, true);
		gameObject7.transform.localScale = Vector3.one;
		Singleton<CanvasManager>.Instance.SetRectTransformStretch(gameObject7.AddComponent<RectTransform>());
		GameObject gameObject8 = new GameObject();
		gameObject8.name = "MessagePanel";
		gameObject8.transform.SetParent(gameObject5.transform, true);
		gameObject8.transform.localScale = Vector3.one;
		Singleton<CanvasManager>.Instance.SetRectTransformStretch(gameObject8.AddComponent<RectTransform>());
		this.charaDeckWindow = PrjUtil.CreateEmptyStretchPanel(Singleton<CanvasManager>.Instance.SystemPanel.transform, "GUI_CharaDeck_Window");
		this.charaDeckWindow.AddComponent<SafeAreaScaler>();
		CanvasManager.MainCanvasOverlay = gameObject5.GetComponent<Canvas>();
	}

	private void CanvasOverlayInstantiate()
	{
		this.TouchEffectInit();
	}

	private void CanvasSystemInstantiate()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("Cmn/GUI/Prefab/GUI_CmnMenu"), this.SystemPanel.transform);
		this.cmnMenuCtrl = gameObject.AddComponent<PguiCmnMenuCtrl>();
		gameObject.AddComponent<SafeAreaScaler>();
		gameObject.transform.SetSiblingIndex(0);
		this.cmnTouchMask = Object.Instantiate<GameObject>((GameObject)Resources.Load("Cmn/GUI/Prefab/CmnTouchMask"), this.SystemPanel.transform);
		this.cmnTouchMask.transform.SetSiblingIndex(this.cmnMenuCtrl.transform.GetSiblingIndex() + 1);
		CanvasManager.SetEnableCmnTouchMask(false);
		this.SystemMiddleArea = this.SystemPanel.transform.Find("MiddleArea");
		this.SystemMiddleArea.SetSiblingIndex(this.cmnTouchMask.transform.GetSiblingIndex() + 1);
		this.CommonWindow = new GameObject("CommonWindow", new Type[] { typeof(RectTransform) });
		this.CommonWindow.AddComponent<CanvasRenderer>();
		this.CommonWindow.transform.SetParent(this.SystemPanel.transform, false);
		Singleton<CanvasManager>.Instance.SetRectTransformStretch(this.CommonWindow.GetComponent<RectTransform>());
		this.CommonWindow.transform.SetSiblingIndex(this.SystemMiddleArea.GetSiblingIndex() + 1);
		this.openWindowCtrlMap = new Dictionary<PguiOpenWindowCtrl.WINDOW_TYPE, PguiOpenWindowCtrl>();
		this.SystemFadeMask = new GameObject("FadeMask", new Type[] { typeof(RectTransform) }).transform;
		this.SystemFadeMask.gameObject.AddComponent<CanvasRenderer>();
		this.SystemFadeMask.SetParent(this.SystemPanel.transform, false);
		Singleton<CanvasManager>.Instance.SetRectTransformStretch(this.SystemFadeMask.GetComponent<RectTransform>());
		this.SystemFadeMask.gameObject.AddComponent<PguiCollider>();
		this.SystemFadeMask.gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.TouchFadeMask = true;
		}, null, null, null, null);
		this.SystemFadeMask.transform.SetSiblingIndex(this.CommonWindow.transform.GetSiblingIndex() + 1);
		this.SystemFadeMask.gameObject.SetActive(false);
		this.bgBoxObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("prefab/PguiBaseTemplate"), this.SystemPanel.transform);
		this.bgBoxObject.name = "BG_BOX";
		GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("Cmn/GUI/Prefab/PanelBg_Simple"), this.bgBoxObject.transform);
		gameObject2.name = this.PANEL_BG_SIMPLE;
		this.BgObjectPoolList = new List<GameObject>();
		this.BgObjectPoolList.Add(gameObject2);
		this.bgRawImage = gameObject2.GetComponentInChildren<PguiRawImageCtrl>();
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.BACK, this.bgBoxObject.transform, true);
		GameObject gameObject3 = Object.Instantiate<GameObject>((GameObject)Resources.Load("prefab/PguiBaseTemplate"), this.SystemPanel.transform);
		gameObject3.name = "MissonProgress";
		this.selMissionProgressCtrl = gameObject3.AddComponent<SelMissionProgressCtrl>();
		this.selMissionProgressCtrl.Init();
		this.serverConnectObj = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/Server_Connect", this.SystemPanel.transform);
		this.webViewWindowCtrl = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/CmnWebWindow", this.SystemPanel.transform).GetComponent<WebViewWindowCtrl>();
		this.webViewWindowCtrl.Init();
		this.outFrame = Object.Instantiate<GameObject>((GameObject)Resources.Load("Cmn/GUI/Prefab/Cmn_OutFrame"));
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.OVERLAY, this.outFrame.transform, true);
		this.outFrame.SetActive(true);
		this.SetOutFrame(SafeAreaScaler.ScreenWidth, SafeAreaScaler.ScreenHeight);
	}

	private void CreateOpenWindow(PguiOpenWindowCtrl.WINDOW_TYPE windowType)
	{
		Dictionary<PguiOpenWindowCtrl.WINDOW_TYPE, string> dictionary = new Dictionary<PguiOpenWindowCtrl.WINDOW_TYPE, string>();
		dictionary.Add(PguiOpenWindowCtrl.WINDOW_TYPE.BASIC, "prefab/CmnOpenWindow");
		dictionary.Add(PguiOpenWindowCtrl.WINDOW_TYPE.NO_STONE, "prefab/CmnOpenWindow_NoStone");
		dictionary.Add(PguiOpenWindowCtrl.WINDOW_TYPE.USE_ITEM, "prefab/CmnOpenWindow_Use");
		dictionary.Add(PguiOpenWindowCtrl.WINDOW_TYPE.FOLLOW, "prefab/CmnOpenWindow_Follow");
		dictionary.Add(PguiOpenWindowCtrl.WINDOW_TYPE.ITEM_INFO, "prefab/CmnOpenWindow_ItemInfo");
		dictionary.Add(PguiOpenWindowCtrl.WINDOW_TYPE.GET_ITEM, "prefab/CmnOpenWindow_GetItem");
		dictionary.Add(PguiOpenWindowCtrl.WINDOW_TYPE.GET_ITEM_MULTIPLE, "prefab/CmnOpenWindow_GetItem_Multiple");
		dictionary.Add(PguiOpenWindowCtrl.WINDOW_TYPE.CHECK_BOX, "prefab/CmnOpenWindow_CheckBox");
		dictionary.Add(PguiOpenWindowCtrl.WINDOW_TYPE.GET_ACHIEVEMENT, "prefab/CmnOpenWindow_GetAchievement");
		dictionary.ContainsKey(windowType);
		GameObject gameObject = Object.Instantiate<GameObject>(Resources.Load(dictionary[windowType]) as GameObject, this.CommonWindow.transform);
		PguiOpenWindowCtrl component = gameObject.GetComponent<PguiOpenWindowCtrl>();
		if (!this.openWindowCtrlMap.ContainsKey(component.windowType))
		{
			gameObject.AddComponent<SafeAreaScaler>();
			this.openWindowCtrlMap.Add(component.windowType, component);
		}
	}

	public static void SetEnableCmnTouchMask(bool isEnable)
	{
		if (Singleton<CanvasManager>.Instance.cmnTouchMask)
		{
			Singleton<CanvasManager>.Instance.cmnTouchMask.SetActive(isEnable);
		}
	}

	public static void AddCallbackCmnTouchMask(UnityAction<Transform> callback)
	{
		if (Singleton<CanvasManager>.Instance.cmnTouchMask)
		{
			PrjUtil.AddTouchEventTrigger(Singleton<CanvasManager>.Instance.cmnTouchMask, callback);
		}
	}

	public static void RemoveCallbackCmnTouchMask()
	{
		if (Singleton<CanvasManager>.Instance.cmnTouchMask)
		{
			PrjUtil.RemoveTouchEventTrigger(Singleton<CanvasManager>.Instance.cmnTouchMask);
		}
	}

	public static TutorialMaskCtrl HdlTutorialMaskCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.tutorialMaskCtrl)
			{
				Singleton<CanvasManager>.Instance.tutorialMaskCtrl = AssetManager.InstantiateAssetData("Tutorial/GUI/Tutorial_Mask", Singleton<CanvasManager>.Instance.SystemPanel.transform).GetComponent<TutorialMaskCtrl>();
				Singleton<CanvasManager>.Instance.tutorialMaskCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex());
				Singleton<CanvasManager>.Instance.tutorialMaskCtrl.SetEnable(false);
			}
			return Singleton<CanvasManager>.Instance.tutorialMaskCtrl;
		}
	}

	public static GameObject HdlQuestWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.questWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.questWindowCtrl = AssetManager.InstantiateAssetData("SceneQuest/GUI/Prefab/GUI_Quest_Window", Singleton<CanvasManager>.Instance.SystemPanel.transform);
				Singleton<CanvasManager>.Instance.questWindowCtrl.AddComponent<SafeAreaScaler>();
				Singleton<CanvasManager>.Instance.questWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex() + 1);
			}
			return Singleton<CanvasManager>.Instance.questWindowCtrl;
		}
	}

	public static PhotoWindowCtrl HdlPhotoWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.photoWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.photoWindowCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_PhotoWindow", Singleton<CanvasManager>.Instance.SystemPanel.transform).GetComponent<PhotoWindowCtrl>();
				Singleton<CanvasManager>.Instance.photoWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex());
				if (null != Singleton<CanvasManager>.Instance.followWindowCtrl)
				{
					Singleton<CanvasManager>.Instance.photoWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.followWindowCtrl.transform.GetSiblingIndex() + 1);
				}
				Singleton<CanvasManager>.Instance.photoWindowCtrl.Init();
				Singleton<CanvasManager>.Instance.photoWindowCtrl.gameObject.SetActive(false);
			}
			return Singleton<CanvasManager>.Instance.photoWindowCtrl;
		}
	}

	public static StickerWindowCtrl HdlStickerWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.stickerWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.stickerWindowCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_StickerWindow", Singleton<CanvasManager>.Instance.SystemPanel.transform).GetComponent<StickerWindowCtrl>();
				Singleton<CanvasManager>.Instance.stickerWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex());
				if (null != Singleton<CanvasManager>.Instance.followWindowCtrl)
				{
					Singleton<CanvasManager>.Instance.stickerWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.followWindowCtrl.transform.GetSiblingIndex() + 1);
				}
				Singleton<CanvasManager>.Instance.stickerWindowCtrl.Init();
				Singleton<CanvasManager>.Instance.stickerWindowCtrl.gameObject.SetActive(false);
			}
			return Singleton<CanvasManager>.Instance.stickerWindowCtrl;
		}
	}

	public static FollowWindowCtrl HdlFollowWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.followWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.followWindowCtrl = AssetManager.InstantiateAssetData("SceneFriend/GUI/Prefab/GUI_Friend_Window_FollowData", Singleton<CanvasManager>.Instance.SystemPanel.transform).GetComponent<FollowWindowCtrl>();
				int num = Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex();
				if (null != Singleton<CanvasManager>.Instance.photoWindowCtrl)
				{
					num = Mathf.Min(Singleton<CanvasManager>.Instance.photoWindowCtrl.transform.GetSiblingIndex(), num);
				}
				if (null != Singleton<CanvasManager>.Instance.accessoryWindowCtrl)
				{
					num = Mathf.Min(Singleton<CanvasManager>.Instance.accessoryWindowCtrl.transform.GetSiblingIndex(), num);
				}
				Singleton<CanvasManager>.Instance.followWindowCtrl.transform.SetSiblingIndex(num);
				Singleton<CanvasManager>.Instance.followWindowCtrl.Init();
				Singleton<CanvasManager>.Instance.followWindowCtrl.gameObject.AddComponent<SafeAreaScaler>();
			}
			return Singleton<CanvasManager>.Instance.followWindowCtrl;
		}
	}

	public static GachaWindowInfoCtrl HdlGachaWindowInfoCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.gachaWindowInfoCtrl)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GUI_GachaWindow_Info"), Singleton<CanvasManager>.Instance.SystemMiddleArea);
				Singleton<CanvasManager>.Instance.gachaWindowInfoCtrl = gameObject.GetComponent<GachaWindowInfoCtrl>();
				Singleton<CanvasManager>.Instance.gachaWindowInfoCtrl.Initialize();
				if (null != Singleton<CanvasManager>.Instance.setItemDetailWindow)
				{
					Singleton<CanvasManager>.Instance.gachaWindowInfoCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.setItemDetailWindow.transform.GetSiblingIndex());
				}
			}
			return Singleton<CanvasManager>.Instance.gachaWindowInfoCtrl;
		}
	}

	public static GachaWindowBoxInfoCtrl HdlGachaWindowBoxInfoCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.gachaWindowBoxInfoCtrl)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GUI_GachaWindow_InfoBox"), Singleton<CanvasManager>.Instance.SystemMiddleArea);
				Singleton<CanvasManager>.Instance.gachaWindowBoxInfoCtrl = gameObject.GetComponent<GachaWindowBoxInfoCtrl>();
				Singleton<CanvasManager>.Instance.gachaWindowBoxInfoCtrl.Initialize();
			}
			return Singleton<CanvasManager>.Instance.gachaWindowBoxInfoCtrl;
		}
	}

	public static GachaWindowStepInfoCtrl HdlGachaWindowStepInfoCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.gachaWindowStepInfoCtrl)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GUI_GachaWindow_InfoStep"), Singleton<CanvasManager>.Instance.SystemMiddleArea);
				Singleton<CanvasManager>.Instance.gachaWindowStepInfoCtrl = gameObject.GetComponent<GachaWindowStepInfoCtrl>();
				Singleton<CanvasManager>.Instance.gachaWindowStepInfoCtrl.Initialize();
			}
			return Singleton<CanvasManager>.Instance.gachaWindowStepInfoCtrl;
		}
	}

	public static ItemPresetWindowCtrl HdlItemPresetWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.itemPresetWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.itemPresetWindowCtrl = Singleton<CanvasManager>.Instance.SystemPanel.AddComponent<ItemPresetWindowCtrl>();
				Singleton<CanvasManager>.Instance.itemPresetWindowCtrl.Init(CanvasManager.HdlSetItemDetailWindowCtrl.transform);
			}
			return Singleton<CanvasManager>.Instance.itemPresetWindowCtrl;
		}
	}

	public static CmnItemWindowCtrl HdlCmnItemWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.cmnItemWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.cmnItemWindowCtrl = Singleton<CanvasManager>.Instance.SystemPanel.AddComponent<CmnItemWindowCtrl>();
				Singleton<CanvasManager>.Instance.cmnItemWindowCtrl.Init();
			}
			return Singleton<CanvasManager>.Instance.cmnItemWindowCtrl;
		}
	}

	public static StaminaRecoveryWindowCtrl HdlStaminaRecoveryWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.staminaRecoveryWindowCtrl)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "StaminaRecoveryWindow";
				gameObject.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, true);
				gameObject.transform.localScale = Vector3.one;
				Singleton<CanvasManager>.Instance.SetRectTransformStretch(gameObject.AddComponent<RectTransform>());
				foreach (GameObject gameObject2 in new List<GameObject>
				{
					Resources.Load("Cmn/GUI/Prefab/GUI_Cmn_StaminaUseKirakira") as GameObject,
					Resources.Load("Cmn/GUI/Prefab/GUI_Cmn_StaminaKind") as GameObject,
					Resources.Load("Cmn/GUI/Prefab/GUI_Cmn_StaminaUse") as GameObject,
					Resources.Load("Cmn/GUI/Prefab/GUI_Cmn_QuestSkipUseKirakira") as GameObject,
					Resources.Load("Cmn/GUI/Prefab/GUI_Cmn_QuestSkipRecoveryFinish") as GameObject
				})
				{
					Object.Instantiate<GameObject>(gameObject2, gameObject.transform).AddComponent<SafeAreaScaler>();
				}
				Singleton<CanvasManager>.Instance.staminaRecoveryWindowCtrl = gameObject.AddComponent<StaminaRecoveryWindowCtrl>();
				Singleton<CanvasManager>.Instance.staminaRecoveryWindowCtrl.Initialize();
			}
			return Singleton<CanvasManager>.Instance.staminaRecoveryWindowCtrl;
		}
	}

	public static DressUpWindowCtrl HdlDressUpWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.dressUpWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.dressUpWindowCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_DressUp", Singleton<CanvasManager>.Instance.SystemPanel.transform).GetComponent<DressUpWindowCtrl>();
				Singleton<CanvasManager>.Instance.dressUpWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemMiddleArea.GetSiblingIndex());
				Singleton<CanvasManager>.Instance.dressUpWindowCtrl.Init();
				Singleton<CanvasManager>.Instance.dressUpWindowCtrl.gameObject.SetActive(false);
			}
			return Singleton<CanvasManager>.Instance.dressUpWindowCtrl;
		}
	}

	public static SelCharaGrowWild.WindowWildResult HdlShopWildReleaseWindow
	{
		get
		{
			if (Singleton<CanvasManager>.Instance.shopWildReleaseWindow == null)
			{
				GameObject gameObject = AssetManager.InstantiateAssetData("SceneShop/GUI/Prefab/Shop_Window_Yasei_After", Singleton<CanvasManager>.Instance.CommonWindow.transform);
				gameObject.AddComponent<SafeAreaScaler>();
				Singleton<CanvasManager>.Instance.shopWildReleaseWindow = new SelCharaGrowWild.WindowWildResult(gameObject.transform);
			}
			return Singleton<CanvasManager>.Instance.shopWildReleaseWindow;
		}
	}

	public static SelPurchaseStoneWindowCtrl HdlSelPurchaseStoneWindowCtrl
	{
		get
		{
			if (Singleton<CanvasManager>.Instance.selPurchaseStoneWindowCtrl == null)
			{
				GameObject gameObject = PrjUtil.CreateEmptyStretchPanel(Singleton<CanvasManager>.Instance.SystemMiddleArea.transform, "PurchaseWindow");
				Singleton<CanvasManager>.Instance.selPurchaseStoneWindowCtrl = gameObject.AddComponent<SelPurchaseStoneWindowCtrl>();
				if (null != Singleton<CanvasManager>.Instance.setItemDetailWindow)
				{
					Singleton<CanvasManager>.Instance.selPurchaseStoneWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.setItemDetailWindow.transform.GetSiblingIndex());
				}
				Singleton<CanvasManager>.Instance.selPurchaseStoneWindowCtrl.Init();
			}
			return Singleton<CanvasManager>.Instance.selPurchaseStoneWindowCtrl;
		}
	}

	public static SelMonthlyPackWindowCtrl HdlSelMonthlyPackWindowCtrl
	{
		get
		{
			if (Singleton<CanvasManager>.Instance.selMonthlyPackWindowCtrl == null)
			{
				GameObject gameObject = PrjUtil.CreateEmptyStretchPanel(Singleton<CanvasManager>.Instance.SystemMiddleArea.transform, "MonthlyPackWindow");
				Singleton<CanvasManager>.Instance.selMonthlyPackWindowCtrl = gameObject.AddComponent<SelMonthlyPackWindowCtrl>();
				Singleton<CanvasManager>.Instance.selMonthlyPackWindowCtrl.Init();
			}
			return Singleton<CanvasManager>.Instance.selMonthlyPackWindowCtrl;
		}
	}

	public static SelMonthlyPackAfterWindowCtrl HdlSelMonthlyPackAfterWindowCtrl
	{
		get
		{
			if (Singleton<CanvasManager>.Instance.selMonthlyPackAfterWindowCtrl == null)
			{
				GameObject gameObject = PrjUtil.CreateEmptyStretchPanel(Singleton<CanvasManager>.Instance.SystemMiddleArea.transform, "MonthlyPackAfterWindow");
				Singleton<CanvasManager>.Instance.selMonthlyPackAfterWindowCtrl = gameObject.AddComponent<SelMonthlyPackAfterWindowCtrl>();
				Singleton<CanvasManager>.Instance.selMonthlyPackAfterWindowCtrl.Init();
			}
			return Singleton<CanvasManager>.Instance.selMonthlyPackAfterWindowCtrl;
		}
	}

	public static QuestSkipWindowsCtrl HdlQuestSkipWindowsCtrl
	{
		get
		{
			if (Singleton<CanvasManager>.Instance.questSkipWindowsCtrl == null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("Cmn/GUI/Prefab/GUI_QuestSkipWindow"), Singleton<CanvasManager>.Instance.SystemPanel.transform);
				Singleton<CanvasManager>.Instance.questSkipWindowsCtrl = gameObject.AddComponent<QuestSkipWindowsCtrl>();
				gameObject.AddComponent<SafeAreaScaler>();
				Singleton<CanvasManager>.Instance.questSkipWindowsCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex() + 1);
			}
			return Singleton<CanvasManager>.Instance.questSkipWindowsCtrl;
		}
	}

	public static SelMissionProgressCtrl HdlMissionProgressCtrl
	{
		get
		{
			return Singleton<CanvasManager>.Instance.selMissionProgressCtrl;
		}
	}

	public static GameObject HdlServerConnectObj
	{
		get
		{
			return Singleton<CanvasManager>.Instance.serverConnectObj;
		}
	}

	public static PguiCmnMenuCtrl HdlCmnMenu
	{
		get
		{
			return Singleton<CanvasManager>.Instance.cmnMenuCtrl;
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowBasic
	{
		get
		{
			if (!Singleton<CanvasManager>.Instance.openWindowCtrlMap.ContainsKey(PguiOpenWindowCtrl.WINDOW_TYPE.BASIC))
			{
				Singleton<CanvasManager>.Instance.CreateOpenWindow(PguiOpenWindowCtrl.WINDOW_TYPE.BASIC);
			}
			return Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.BASIC];
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowNoStone
	{
		get
		{
			if (!Singleton<CanvasManager>.Instance.openWindowCtrlMap.ContainsKey(PguiOpenWindowCtrl.WINDOW_TYPE.NO_STONE))
			{
				Singleton<CanvasManager>.Instance.CreateOpenWindow(PguiOpenWindowCtrl.WINDOW_TYPE.NO_STONE);
			}
			return Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.NO_STONE];
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowUseItem
	{
		get
		{
			if (!Singleton<CanvasManager>.Instance.openWindowCtrlMap.ContainsKey(PguiOpenWindowCtrl.WINDOW_TYPE.USE_ITEM))
			{
				Singleton<CanvasManager>.Instance.CreateOpenWindow(PguiOpenWindowCtrl.WINDOW_TYPE.USE_ITEM);
			}
			return Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.USE_ITEM];
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowFollow
	{
		get
		{
			if (!Singleton<CanvasManager>.Instance.openWindowCtrlMap.ContainsKey(PguiOpenWindowCtrl.WINDOW_TYPE.FOLLOW))
			{
				Singleton<CanvasManager>.Instance.CreateOpenWindow(PguiOpenWindowCtrl.WINDOW_TYPE.FOLLOW);
			}
			return Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.FOLLOW];
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowItemInfo
	{
		get
		{
			if (!Singleton<CanvasManager>.Instance.openWindowCtrlMap.ContainsKey(PguiOpenWindowCtrl.WINDOW_TYPE.ITEM_INFO))
			{
				Singleton<CanvasManager>.Instance.CreateOpenWindow(PguiOpenWindowCtrl.WINDOW_TYPE.ITEM_INFO);
			}
			return Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.ITEM_INFO];
		}
	}

	public static GetItemWindowCtrl HdlGetItemWindowCtrl
	{
		get
		{
			if (!Singleton<CanvasManager>.Instance.openWindowCtrlMap.ContainsKey(PguiOpenWindowCtrl.WINDOW_TYPE.GET_ITEM))
			{
				Singleton<CanvasManager>.Instance.CreateOpenWindow(PguiOpenWindowCtrl.WINDOW_TYPE.GET_ITEM);
				GetItemWindowCtrl getItemWindowCtrl = Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.GET_ITEM].gameObject.AddComponent<GetItemWindowCtrl>();
				getItemWindowCtrl.Init(getItemWindowCtrl.transform);
			}
			return Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.GET_ITEM].GetComponent<GetItemWindowCtrl>();
		}
	}

	public static GetMultiItemWindowCtrl HdlGetItemSetWindowCtrl
	{
		get
		{
			if (!Singleton<CanvasManager>.Instance.openWindowCtrlMap.ContainsKey(PguiOpenWindowCtrl.WINDOW_TYPE.GET_ITEM_MULTIPLE))
			{
				Singleton<CanvasManager>.Instance.CreateOpenWindow(PguiOpenWindowCtrl.WINDOW_TYPE.GET_ITEM_MULTIPLE);
				GetMultiItemWindowCtrl getMultiItemWindowCtrl = Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.GET_ITEM_MULTIPLE].gameObject.AddComponent<GetMultiItemWindowCtrl>();
				getMultiItemWindowCtrl.Init(getMultiItemWindowCtrl.transform);
			}
			return Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.GET_ITEM_MULTIPLE].GetComponent<GetMultiItemWindowCtrl>();
		}
	}

	public static GetAchievementWindowCtrl HdlGetAchievementWindowCtrl
	{
		get
		{
			if (!Singleton<CanvasManager>.Instance.openWindowCtrlMap.ContainsKey(PguiOpenWindowCtrl.WINDOW_TYPE.GET_ACHIEVEMENT))
			{
				Singleton<CanvasManager>.Instance.CreateOpenWindow(PguiOpenWindowCtrl.WINDOW_TYPE.GET_ACHIEVEMENT);
				GetAchievementWindowCtrl getAchievementWindowCtrl = Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.GET_ACHIEVEMENT].gameObject.AddComponent<GetAchievementWindowCtrl>();
				getAchievementWindowCtrl.Init(getAchievementWindowCtrl.transform);
			}
			return Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.GET_ACHIEVEMENT].GetComponent<GetAchievementWindowCtrl>();
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowCheckBox
	{
		get
		{
			if (!Singleton<CanvasManager>.Instance.openWindowCtrlMap.ContainsKey(PguiOpenWindowCtrl.WINDOW_TYPE.CHECK_BOX))
			{
				Singleton<CanvasManager>.Instance.CreateOpenWindow(PguiOpenWindowCtrl.WINDOW_TYPE.CHECK_BOX);
			}
			return Singleton<CanvasManager>.Instance.openWindowCtrlMap[PguiOpenWindowCtrl.WINDOW_TYPE.CHECK_BOX];
		}
	}

	public static SortWindowCtrl HdlOpenWindowSortFilter
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.sortWindowCtrl)
			{
				GameObject gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_SortWindow", Singleton<CanvasManager>.Instance.SystemPanel.transform);
				gameObject.AddComponent<SafeAreaScaler>();
				gameObject.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex() + 1);
				Singleton<CanvasManager>.Instance.sortWindowCtrl = gameObject.AddComponent<SortWindowCtrl>();
				Singleton<CanvasManager>.Instance.sortWindowCtrl.Create(gameObject);
				GameObject gameObject2 = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_FilterWindow_Friends", Singleton<CanvasManager>.Instance.SystemPanel.transform);
				gameObject2.AddComponent<SafeAreaScaler>();
				gameObject2.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex() + 1);
				FriendsFilterWindowCtrl friendsFilterWindowCtrl = new FriendsFilterWindowCtrl();
				friendsFilterWindowCtrl.Initialize(gameObject2);
				Singleton<CanvasManager>.Instance.friendsFilterWindowCtrl = friendsFilterWindowCtrl;
				Singleton<CanvasManager>.Instance.sortWindowCtrl.InitializeFriendsFilter(gameObject2);
				GameObject gameObject3 = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_FilterWindow_Photo", Singleton<CanvasManager>.Instance.SystemPanel.transform);
				gameObject3.AddComponent<SafeAreaScaler>();
				gameObject3.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex() + 1);
				PhotoFilterWindowCtrl photoFilterWindowCtrl = new PhotoFilterWindowCtrl();
				photoFilterWindowCtrl.Initialize(gameObject3);
				Singleton<CanvasManager>.Instance.photoFilterWindowCtrl = photoFilterWindowCtrl;
				GameObject gameObject4 = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_FilterWindow_Sticker", Singleton<CanvasManager>.Instance.SystemPanel.transform);
				gameObject4.AddComponent<SafeAreaScaler>();
				gameObject4.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex() + 1);
				StickerFilterWindowCtrl stickerFilterWindowCtrl = new StickerFilterWindowCtrl();
				stickerFilterWindowCtrl.Initialize(gameObject4);
				Singleton<CanvasManager>.Instance.stickerFilterWindowCtrl = stickerFilterWindowCtrl;
			}
			return Singleton<CanvasManager>.Instance.sortWindowCtrl;
		}
	}

	public static AccessorySortWindowCtrl HdlAccessorySortWindow
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.accessorySortWindowCtrl)
			{
				GameObject gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_SortWindow", Singleton<CanvasManager>.Instance.SystemPanel.transform);
				gameObject.AddComponent<SafeAreaScaler>();
				gameObject.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex() + 1);
				Singleton<CanvasManager>.Instance.accessorySortWindowCtrl = gameObject.AddComponent<AccessorySortWindowCtrl>();
				Singleton<CanvasManager>.Instance.accessorySortWindowCtrl.Initialize(gameObject);
			}
			return Singleton<CanvasManager>.Instance.accessorySortWindowCtrl;
		}
	}

	public static AccessoryFilterWindowCtrl HdlAccessoryFilterWindow
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.accessoryFilterWindowCtrl)
			{
				GameObject gameObject = AssetManager.InstantiateAssetData("SceneAccessory/GUI/Prefab/GUI_AccessoryFilter_Window", Singleton<CanvasManager>.Instance.SystemPanel.transform);
				gameObject.AddComponent<SafeAreaScaler>();
				gameObject.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex() + 1);
				Singleton<CanvasManager>.Instance.accessoryFilterWindowCtrl = gameObject.AddComponent<AccessoryFilterWindowCtrl>();
				Singleton<CanvasManager>.Instance.accessoryFilterWindowCtrl.Initialize(gameObject);
			}
			return Singleton<CanvasManager>.Instance.accessoryFilterWindowCtrl;
		}
	}

	public static SelCharaGrowPhotoPocket.ExchangeWarningWindow HdlExchangeWarningWindow
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.exchangeWarningWindow)
			{
				GameObject gameObject = AssetManager.InstantiateAssetData("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Exchange", Singleton<CanvasManager>.Instance.SystemPanel.transform);
				gameObject.AddComponent<SafeAreaScaler>();
				gameObject.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex() + 1);
				Singleton<CanvasManager>.Instance.exchangeWarningWindow = gameObject.AddComponent<SelCharaGrowPhotoPocket.ExchangeWarningWindow>();
				Singleton<CanvasManager>.Instance.exchangeWarningWindow.Initialize();
			}
			return Singleton<CanvasManager>.Instance.exchangeWarningWindow;
		}
	}

	public static PhotoFilterWindowCtrl HdlPhotoFilterWindowCtrl
	{
		get
		{
			return Singleton<CanvasManager>.Instance.photoFilterWindowCtrl;
		}
	}

	public static FriendsFilterWindowCtrl HdlFriendsFilterWindowCtrl
	{
		get
		{
			return Singleton<CanvasManager>.Instance.friendsFilterWindowCtrl;
		}
	}

	public static StickerFilterWindowCtrl HdlStickerFilterWindowCtrl
	{
		get
		{
			return Singleton<CanvasManager>.Instance.stickerFilterWindowCtrl;
		}
	}

	public static RecommendedDeckWindowCtrl HdlOpenWindowRecommendedDeck
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.recommendedDeckWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.recommendedDeckWindowCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_CharaDeck_Window/Window_AutoDeck", Singleton<CanvasManager>.Instance.charaDeckWindow.transform).gameObject.AddComponent<RecommendedDeckWindowCtrl>();
				Singleton<CanvasManager>.Instance.recommendedDeckWindowCtrl.gameObject.name = "Window_AutoDeck";
				Singleton<CanvasManager>.Instance.recommendedDeckWindowCtrl.Init(Singleton<CanvasManager>.Instance.charaDeckWindow);
			}
			return Singleton<CanvasManager>.Instance.recommendedDeckWindowCtrl;
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowPartyName
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.partyNameCtrl)
			{
				Singleton<CanvasManager>.Instance.partyNameCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_CharaDeck_Window/Window_PartyName", Singleton<CanvasManager>.Instance.charaDeckWindow.transform).gameObject.GetComponent<PguiOpenWindowCtrl>();
				InputField inputField = Singleton<CanvasManager>.Instance.partyNameCtrl.m_UserInfoContent.GetComponent<InputField>();
				inputField.onEndEdit.AddListener(delegate(string str)
				{
					inputField.text = PrjUtil.ModifiedPartyName(str);
				});
				inputField.text = "";
				inputField.textComponent.text = "";
				inputField.placeholder.GetComponent<Text>().text = "";
			}
			return Singleton<CanvasManager>.Instance.partyNameCtrl;
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowUserSkill
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.userSkillCtrl)
			{
				Singleton<CanvasManager>.Instance.userSkillCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_CharaDeck_Window/Window_UserSkill", Singleton<CanvasManager>.Instance.charaDeckWindow.transform).gameObject.GetComponent<PguiOpenWindowCtrl>();
			}
			return Singleton<CanvasManager>.Instance.userSkillCtrl;
		}
	}

	public static SelTacticsSkillChangeWindowCtrl HdlOpenWindowTacticsSkillChange
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.tacticsSkillChangeCtrl)
			{
				Singleton<CanvasManager>.Instance.tacticsSkillChangeCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_CharaDeck_Window/Window_TacticsSkill_Change", Singleton<CanvasManager>.Instance.charaDeckWindow.transform).gameObject.AddComponent<SelTacticsSkillChangeWindowCtrl>();
				Singleton<CanvasManager>.Instance.tacticsSkillChangeCtrl.gameObject.name = "Window_TacticsSkill_Change";
				Singleton<CanvasManager>.Instance.tacticsSkillChangeCtrl.Initialize(Singleton<CanvasManager>.Instance.charaDeckWindow);
			}
			return Singleton<CanvasManager>.Instance.tacticsSkillChangeCtrl;
		}
	}

	public static SelTacticsSkillInfoWindowCtrl HdlOpenWindowTacticsSkillInfo
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.tacticsSkillInfoCtrl)
			{
				Singleton<CanvasManager>.Instance.tacticsSkillInfoCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_CharaDeck_Window/Window_TacticsSkill_Info", Singleton<CanvasManager>.Instance.charaDeckWindow.transform).gameObject.AddComponent<SelTacticsSkillInfoWindowCtrl>();
				Singleton<CanvasManager>.Instance.tacticsSkillInfoCtrl.gameObject.name = "Window_TacticsSkill_Info";
				Singleton<CanvasManager>.Instance.tacticsSkillInfoCtrl.Initialize(Singleton<CanvasManager>.Instance.charaDeckWindow);
			}
			return Singleton<CanvasManager>.Instance.tacticsSkillInfoCtrl;
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowPartyCaution
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.partyCautionCtrl)
			{
				Singleton<CanvasManager>.Instance.partyCautionCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_CharaDeck_Window/Window_Caution", Singleton<CanvasManager>.Instance.charaDeckWindow.transform).gameObject.GetComponent<PguiOpenWindowCtrl>();
			}
			return Singleton<CanvasManager>.Instance.partyCautionCtrl;
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowOrderCard
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.orderCardWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.orderCardWindowCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_CharaDeck_Window/Window_OrderCardInfo", Singleton<CanvasManager>.Instance.charaDeckWindow.transform).gameObject.GetComponent<PguiOpenWindowCtrl>();
			}
			return Singleton<CanvasManager>.Instance.orderCardWindowCtrl;
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowTrainingEnemyInfo
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.trainingEnemyInfoWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.trainingEnemyInfoWindowCtrl = AssetManager.InstantiateAssetData("SceneTraining/GUI/Prefab/Training_EnemyInfo_Window", Singleton<CanvasManager>.Instance.SystemPanel.transform).GetComponent<PguiOpenWindowCtrl>();
				Singleton<CanvasManager>.Instance.trainingEnemyInfoWindowCtrl.gameObject.AddComponent<SafeAreaScaler>();
			}
			return Singleton<CanvasManager>.Instance.trainingEnemyInfoWindowCtrl;
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowBonusPhotoInfo
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.bonusPhotoInfoWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.bonusPhotoInfoWindowCtrl = AssetManager.InstantiateAssetData("SceneBattleSelector/GUI/Prefab/GUI_BattleSelector_PhotoInfoWindow", Singleton<CanvasManager>.Instance.SystemMiddleArea).GetComponent<PguiOpenWindowCtrl>();
			}
			return Singleton<CanvasManager>.Instance.bonusPhotoInfoWindowCtrl;
		}
	}

	public static CmnQuestSealedInfoWindowCtrl HdlOpenWindowQUestSealedInfo
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.questSealedInfoWindowCtrl)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneBattleSelector/GUI/Prefab/GUI_Quest_Sealed"));
				SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, gameObject.transform, true);
				Singleton<CanvasManager>.Instance.questSealedInfoWindowCtrl = gameObject.AddComponent<CmnQuestSealedInfoWindowCtrl>();
				Singleton<CanvasManager>.Instance.questSealedInfoWindowCtrl.Setup(gameObject);
				Singleton<CanvasManager>.Instance.questSealedInfoWindowCtrl.transform.SetAsLastSibling();
			}
			return Singleton<CanvasManager>.Instance.questSealedInfoWindowCtrl;
		}
	}

	public static HelpWindowCtrl HdlHelpWindowCtrl
	{
		get
		{
			if (Singleton<CanvasManager>.Instance.helpWindowCtrl == null)
			{
				Singleton<CanvasManager>.Instance.helpWindowCtrl = AssetManager.InstantiateAssetData("SceneOption/GUI/Prefab/GUI_HelpWindow", Singleton<CanvasManager>.Instance.SystemPanel.transform).GetComponent<HelpWindowCtrl>();
				Singleton<CanvasManager>.Instance.helpWindowCtrl.Init((Singleton<DataManager>.Instance != null) ? DataManager.DmServerMst.mstHelpDataList : null);
			}
			return Singleton<CanvasManager>.Instance.helpWindowCtrl;
		}
	}

	public static CmnFeedPageWindowCtrl HdlCmnFeedPageWindowCtrl
	{
		get
		{
			if (Singleton<CanvasManager>.Instance.cmnFeedPageWindowCtrl == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<CmnFeedPageWindowCtrl>();
				gameObject.name = "CmnFeedPageWindowCtrl";
				gameObject.transform.SetParent(Singleton<CanvasManager>.Instance.SystemMiddleArea, false);
				Singleton<CanvasManager>.Instance.cmnFeedPageWindowCtrl = gameObject.GetComponent<CmnFeedPageWindowCtrl>();
				Singleton<CanvasManager>.Instance.cmnFeedPageWindowCtrl.Init();
			}
			return Singleton<CanvasManager>.Instance.cmnFeedPageWindowCtrl;
		}
	}

	public static AccessoryCheckWindowCtrl HdlAccessoryReleaseWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.accessoryReleaseWindowCtrl)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<AccessoryCheckWindowCtrl>();
				gameObject.name = "AccessoryReleaseWindowCtrl";
				gameObject.transform.SetParent(Singleton<CanvasManager>.Instance.SystemMiddleArea, false);
				Singleton<CanvasManager>.Instance.accessoryReleaseWindowCtrl = gameObject.GetComponent<AccessoryCheckWindowCtrl>();
				Singleton<CanvasManager>.Instance.accessoryReleaseWindowCtrl.Init(AccessoryCheckWindowCtrl.Type.Release);
			}
			return Singleton<CanvasManager>.Instance.accessoryReleaseWindowCtrl;
		}
	}

	public static AccessoryCheckWindowCtrl HdlAccessoryOwnerSettingWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.accessoryOwnerSettingWindowCtrl)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<AccessoryCheckWindowCtrl>();
				gameObject.name = "AccessoryOwnerSettingWindowCtrl";
				gameObject.transform.SetParent(Singleton<CanvasManager>.Instance.SystemMiddleArea, false);
				Singleton<CanvasManager>.Instance.accessoryOwnerSettingWindowCtrl = gameObject.GetComponent<AccessoryCheckWindowCtrl>();
				Singleton<CanvasManager>.Instance.accessoryOwnerSettingWindowCtrl.Init(AccessoryCheckWindowCtrl.Type.OwnerSetting);
			}
			return Singleton<CanvasManager>.Instance.accessoryOwnerSettingWindowCtrl;
		}
	}

	public static AccessoryCheckWindowCtrl HdlAccessoryCheckWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.accessoryCheckWindowCtrl)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<AccessoryCheckWindowCtrl>();
				gameObject.name = "AccessoryCheckWindowCtrl";
				gameObject.transform.SetParent(Singleton<CanvasManager>.Instance.SystemMiddleArea, false);
				Singleton<CanvasManager>.Instance.accessoryCheckWindowCtrl = gameObject.GetComponent<AccessoryCheckWindowCtrl>();
				Singleton<CanvasManager>.Instance.accessoryCheckWindowCtrl.Init(AccessoryCheckWindowCtrl.Type.Check);
			}
			return Singleton<CanvasManager>.Instance.accessoryCheckWindowCtrl;
		}
	}

	public static DetachableAccessoryWindowCtrl HdlDetachableAccessoryWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.detachableAccessoryWindowCtrl)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<DetachableAccessoryWindowCtrl>();
				gameObject.name = "DetachableAccessoryWindowCtrl";
				gameObject.transform.SetParent(Singleton<CanvasManager>.Instance.SystemMiddleArea, false);
				Singleton<CanvasManager>.Instance.detachableAccessoryWindowCtrl = gameObject.GetComponent<DetachableAccessoryWindowCtrl>();
				if (null != Singleton<CanvasManager>.Instance.cmnFeedPageWindowCtrl)
				{
					Singleton<CanvasManager>.Instance.detachableAccessoryWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnFeedPageWindowCtrl.transform.GetSiblingIndex());
				}
				Singleton<CanvasManager>.Instance.detachableAccessoryWindowCtrl.Init();
			}
			return Singleton<CanvasManager>.Instance.detachableAccessoryWindowCtrl;
		}
	}

	public static LoadAndTipsCtrl HdlLoadAndTipsCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.loadAndTipsCtrl)
			{
				GameObject gameObject = PrjUtil.CreateEmptyStretchPanel(Singleton<CanvasManager>.Instance.SystemPanel.transform, "LoadAndTips");
				Singleton<CanvasManager>.Instance.loadAndTipsCtrl = gameObject.AddComponent<LoadAndTipsCtrl>();
				Singleton<CanvasManager>.Instance.loadAndTipsCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemMiddleArea.GetSiblingIndex() + 1);
				Singleton<CanvasManager>.Instance.loadAndTipsCtrl.Init();
			}
			return Singleton<CanvasManager>.Instance.loadAndTipsCtrl;
		}
	}

	public static void SwitchLayerHdlLoadAndTipsCtrl(SceneManager.CanvasType canvasType)
	{
		if (canvasType == SceneManager.CanvasType.SYSTEM)
		{
			Singleton<CanvasManager>.Instance.loadAndTipsCtrl.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, false);
			Singleton<CanvasManager>.Instance.loadAndTipsCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemMiddleArea.GetSiblingIndex() + 1);
			return;
		}
		if (canvasType != SceneManager.CanvasType.OVERLAY)
		{
			return;
		}
		Singleton<CanvasManager>.Instance.loadAndTipsCtrl.transform.SetParent(CanvasManager.MainCanvasOverlay.transform.Find("LoadingPanel"), false);
	}

	public static SelCharaDeckCtrl HdlSelCharaDeck
	{
		get
		{
			if (Singleton<CanvasManager>.Instance.selCharaDeckCtrl == null)
			{
				GameObject gameObject = new GameObject("CharaDeckBase");
				Singleton<CanvasManager>.Instance.SetRectTransformStretch(gameObject.AddComponent<RectTransform>());
				Singleton<CanvasManager>.Instance.selCharaDeckCtrl = gameObject.AddComponent<SelCharaDeckCtrl>();
				Singleton<CanvasManager>.Instance.selCharaDeckCtrl.Init();
				Singleton<CanvasManager>.Instance.selCharaDeckCtrl.SetActive(false, false);
			}
			return Singleton<CanvasManager>.Instance.selCharaDeckCtrl;
		}
	}

	public static WebViewWindowCtrl HdlWebViewWindowCtrl
	{
		get
		{
			return Singleton<CanvasManager>.Instance.webViewWindowCtrl;
		}
	}

	public static PguiOpenWindowCtrl HdlOpenWindowServerError
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.errorWindowCtrl)
			{
				GameObject gameObject = CanvasManager.MainCanvasOverlay.transform.Find("MessagePanel").gameObject;
				Singleton<CanvasManager>.Instance.errorWindowCtrl = AssetManager.InstantiateAssetData("prefab/CmnOpenWindow_ServerError", gameObject.transform).GetComponent<PguiOpenWindowCtrl>();
			}
			return Singleton<CanvasManager>.Instance.errorWindowCtrl;
		}
	}

	public static DressUpWipeCtrl HdlDressUpWipeCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.dressUpWipeCtrl)
			{
				GameObject gameObject = CanvasManager.MainCanvasOverlay.transform.Find("WindowPanel").gameObject;
				Singleton<CanvasManager>.Instance.dressUpWipeCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/DressUp_AllWipe", gameObject.transform).GetComponent<DressUpWipeCtrl>();
				Singleton<CanvasManager>.Instance.dressUpWipeCtrl.Init();
				Singleton<CanvasManager>.Instance.dressUpWipeCtrl.gameObject.SetActive(false);
				if (null != Singleton<CanvasManager>.Instance.charaWindowCtrl)
				{
					Singleton<CanvasManager>.Instance.dressUpWipeCtrl.transform.SetSiblingIndex(CanvasManager.HdlCharaWindowCtrl.transform.GetSiblingIndex() + 1);
				}
			}
			return Singleton<CanvasManager>.Instance.dressUpWipeCtrl;
		}
	}

	public static CharaWindowCtrl HdlCharaWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.charaWindowCtrl)
			{
				GameObject gameObject = CanvasManager.MainCanvasOverlay.transform.Find("WindowPanel").gameObject;
				Singleton<CanvasManager>.Instance.charaWindowCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_CharaInfo", gameObject.transform).GetComponent<CharaWindowCtrl>();
				Singleton<CanvasManager>.Instance.charaWindowCtrl.Init();
				Singleton<CanvasManager>.Instance.charaWindowCtrl.gameObject.SetActive(false);
				if (null != Singleton<CanvasManager>.Instance.dressUpWipeCtrl)
				{
					Singleton<CanvasManager>.Instance.charaWindowCtrl.transform.SetSiblingIndex(CanvasManager.HdlDressUpWipeCtrl.transform.GetSiblingIndex());
				}
				if (null != Singleton<CanvasManager>.Instance.cmnReleaseConditionWindowCtrl)
				{
					Singleton<CanvasManager>.Instance.cmnReleaseConditionWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.charaWindowCtrl.transform.GetSiblingIndex() + 1);
				}
			}
			return Singleton<CanvasManager>.Instance.charaWindowCtrl;
		}
	}

	public static ItemDetailCtrl HdlItemDetailCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.itemDetailCtrl)
			{
				GameObject gameObject = CanvasManager.MainCanvasOverlay.transform.Find("WindowPanel").gameObject;
				Singleton<CanvasManager>.Instance.itemDetailCtrl = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/GUI_Cmn_ItemInfo", gameObject.transform).GetComponent<ItemDetailCtrl>();
				Singleton<CanvasManager>.Instance.itemDetailCtrl.Init();
				Singleton<CanvasManager>.Instance.itemDetailCtrl.gameObject.SetActive(false);
				Singleton<CanvasManager>.Instance.itemDetailCtrl.transform.SetAsLastSibling();
			}
			return Singleton<CanvasManager>.Instance.itemDetailCtrl;
		}
	}

	public static AchievementDetailCtrl HdlAchievementDetailCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.achievementDetailCtrl)
			{
				GameObject gameObject = CanvasManager.MainCanvasOverlay.transform.Find("WindowPanel").gameObject;
				Singleton<CanvasManager>.Instance.achievementDetailCtrl = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/GUI_Cmn_AchievementInfo", gameObject.transform).GetComponent<AchievementDetailCtrl>();
				Singleton<CanvasManager>.Instance.achievementDetailCtrl.Init();
				Singleton<CanvasManager>.Instance.achievementDetailCtrl.transform.SetAsLastSibling();
			}
			return Singleton<CanvasManager>.Instance.achievementDetailCtrl;
		}
	}

	public static CmnReleaseConditionWindowCtrl HdlCmnReleaseConditionWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.cmnReleaseConditionWindowCtrl)
			{
				GameObject gameObject = CanvasManager.MainCanvasOverlay.transform.Find("WindowPanel").gameObject;
				Singleton<CanvasManager>.Instance.cmnReleaseConditionWindowCtrl = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/GUI_Cmn_Window_LockInfo", gameObject.transform).GetComponent<CmnReleaseConditionWindowCtrl>();
				Singleton<CanvasManager>.Instance.cmnReleaseConditionWindowCtrl.Init();
				Singleton<CanvasManager>.Instance.cmnReleaseConditionWindowCtrl.transform.SetAsLastSibling();
			}
			return Singleton<CanvasManager>.Instance.cmnReleaseConditionWindowCtrl;
		}
	}

	public static PguiOpenWindowCtrl HdlSetItemDetailWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.setItemDetailWindow)
			{
				Singleton<CanvasManager>.Instance.setItemDetailWindow = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/GUI_Cmn_SetItemDetailWindow", Singleton<CanvasManager>.Instance.SystemMiddleArea).GetComponent<PguiOpenWindowCtrl>();
				int num = 0;
				if (null != Singleton<CanvasManager>.Instance.selPurchaseStoneWindowCtrl)
				{
					num = Singleton<CanvasManager>.Instance.selPurchaseStoneWindowCtrl.transform.GetSiblingIndex() + 1;
				}
				if (null != Singleton<CanvasManager>.Instance.gachaWindowInfoCtrl)
				{
					int num2 = Singleton<CanvasManager>.Instance.gachaWindowInfoCtrl.transform.GetSiblingIndex() + 1;
					num = ((num < num2) ? num2 : num);
				}
				if (num != 0)
				{
					Singleton<CanvasManager>.Instance.setItemDetailWindow.transform.SetSiblingIndex(num);
				}
			}
			return Singleton<CanvasManager>.Instance.setItemDetailWindow;
		}
	}

	public static PguiOpenWindowCtrl HdlSellItemInfoWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.sellItemInfoWindow)
			{
				Singleton<CanvasManager>.Instance.sellItemInfoWindow = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/GUI_Cmn_ShopWindow_Normal", Singleton<CanvasManager>.Instance.SystemMiddleArea).GetComponent<PguiOpenWindowCtrl>();
				Singleton<CanvasManager>.Instance.sellItemInfoWindow.name = "sellItemInfoWindow";
			}
			return Singleton<CanvasManager>.Instance.sellItemInfoWindow;
		}
	}

	public static PguiOpenWindowCtrl HdlBankContentWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.itemBankContentWindow)
			{
				GameObject gameObject = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/GUI_Cmn_BankContentWindow", Singleton<CanvasManager>.Instance.SystemMiddleArea);
				Singleton<CanvasManager>.Instance.itemBankContentWindow = gameObject.GetComponent<PguiOpenWindowCtrl>();
			}
			return Singleton<CanvasManager>.Instance.itemBankContentWindow;
		}
	}

	public static SelQuestCountRecoveryWindowCtrl HdlSelQuestCountRecoveryWindowCtrl
	{
		get
		{
			if (Singleton<CanvasManager>.Instance.selQuestCountRecoveryWindowCtrl == null)
			{
				GameObject gameObject = PrjUtil.CreateEmptyStretchPanel(Singleton<CanvasManager>.Instance.SystemMiddleArea, "QuestCountRecoveryWindow");
				Singleton<CanvasManager>.Instance.selQuestCountRecoveryWindowCtrl = gameObject.AddComponent<SelQuestCountRecoveryWindowCtrl>();
				Singleton<CanvasManager>.Instance.selQuestCountRecoveryWindowCtrl.Init();
			}
			return Singleton<CanvasManager>.Instance.selQuestCountRecoveryWindowCtrl;
		}
	}

	public static RewardListWindowCtrl HdlEventCoopWindowCtrl
	{
		get
		{
			if (Singleton<CanvasManager>.Instance.rewardListWindowCtrl == null)
			{
				GameObject gameObject = PrjUtil.CreateEmptyStretchPanel(Singleton<CanvasManager>.Instance.SystemMiddleArea, "RewardListWindow");
				Singleton<CanvasManager>.Instance.rewardListWindowCtrl = gameObject.AddComponent<RewardListWindowCtrl>();
				Singleton<CanvasManager>.Instance.rewardListWindowCtrl.Init();
			}
			return Singleton<CanvasManager>.Instance.rewardListWindowCtrl;
		}
	}

	public static TreeHouseFurnitureWindowCtrl HdlTreeHouseFurnitureWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.treeHouseFurnitureWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.treeHouseFurnitureWindowCtrl = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_InteriorWindow", Singleton<CanvasManager>.Instance.SystemPanel.transform).GetComponent<TreeHouseFurnitureWindowCtrl>();
				Singleton<CanvasManager>.Instance.treeHouseFurnitureWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex());
				Singleton<CanvasManager>.Instance.treeHouseFurnitureWindowCtrl.Initialize();
			}
			return Singleton<CanvasManager>.Instance.treeHouseFurnitureWindowCtrl;
		}
	}

	public static AccessoryWindowCtrl HdlAccessoryWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.accessoryWindowCtrl)
			{
				Singleton<CanvasManager>.Instance.accessoryWindowCtrl = AssetManager.InstantiateAssetData("SceneAccessory/GUI/Prefab/GUI_AccessoryWindow", Singleton<CanvasManager>.Instance.SystemPanel.transform).GetComponent<AccessoryWindowCtrl>();
				Singleton<CanvasManager>.Instance.accessoryWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.SystemFadeMask.GetSiblingIndex());
				if (null != Singleton<CanvasManager>.Instance.followWindowCtrl)
				{
					Singleton<CanvasManager>.Instance.accessoryWindowCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.followWindowCtrl.transform.GetSiblingIndex() + 1);
				}
				Singleton<CanvasManager>.Instance.accessoryWindowCtrl.Initialize();
			}
			return Singleton<CanvasManager>.Instance.accessoryWindowCtrl;
		}
	}

	public static AdvertiseBannerCtrl HdlAdevertiseBannerCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.adevertiseBannerCtrl)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("prefab/PguiBaseTemplate"), Singleton<CanvasManager>.Instance.SystemPanel.transform);
				gameObject.name = "AdvertiseBanner";
				gameObject.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.selMissionProgressCtrl.transform.GetSiblingIndex() + 1);
				Singleton<CanvasManager>.Instance.adevertiseBannerCtrl = gameObject.AddComponent<AdvertiseBannerCtrl>();
				CanvasManager.SetEnableCmnTouchMask(false);
			}
			return Singleton<CanvasManager>.Instance.adevertiseBannerCtrl;
		}
	}

	public static PurchaseConfirmWindow HdlPurchaseConfirmWindow
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.purchaseConfirmWindow)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneShop/GUI/Prefab/GUI_PurchaseConfirm_Window"), Singleton<CanvasManager>.Instance.SystemPanel.transform);
				gameObject.name = "purchaseConfirmWindow";
				gameObject.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.selMissionProgressCtrl.transform.GetSiblingIndex() + 1);
				gameObject.GetComponent<PguiPanel>().raycastTarget = false;
				Singleton<CanvasManager>.Instance.purchaseConfirmWindow = gameObject.AddComponent<PurchaseConfirmWindow>();
			}
			return Singleton<CanvasManager>.Instance.purchaseConfirmWindow;
		}
	}

	public static PguiOpenWindowCtrl HdlMonthlyConfirmWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.monthlyConfirmWindow)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("prefab/CmnMonthlyConfirmWindow"), Singleton<CanvasManager>.Instance.SystemPanel.transform);
				gameObject.name = "MonthlyConfirmWindow";
				gameObject.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.selMissionProgressCtrl.transform.GetSiblingIndex() + 1);
				Singleton<CanvasManager>.Instance.monthlyConfirmWindow = gameObject.GetComponent<PguiOpenWindowCtrl>();
			}
			return Singleton<CanvasManager>.Instance.monthlyConfirmWindow;
		}
	}

	public static PguiOpenWindowCtrl HdlKizunaReachedLimitWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.kizunaReachedLimitWindow)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/GUI_DeckReachedLimitList_Window"));
				SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, gameObject.transform, true);
				Singleton<CanvasManager>.Instance.kizunaReachedLimitWindow = gameObject.GetComponent<PguiOpenWindowCtrl>();
			}
			return Singleton<CanvasManager>.Instance.kizunaReachedLimitWindow;
		}
	}

	public static CmnKizunaBuffWindowCtrl HdlKizunaKizunaBuffWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.kizunaBuffWindowCtrl)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("Cmn/GUI/Prefab/GUI_Cmn_KizunaBuffWindow"));
				SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, gameObject.transform, true);
				Singleton<CanvasManager>.Instance.kizunaBuffWindowCtrl = gameObject.AddComponent<CmnKizunaBuffWindowCtrl>();
				Singleton<CanvasManager>.Instance.kizunaBuffWindowCtrl.Setup(gameObject);
				Singleton<CanvasManager>.Instance.kizunaBuffWindowCtrl.transform.SetAsLastSibling();
			}
			return Singleton<CanvasManager>.Instance.kizunaBuffWindowCtrl;
		}
	}

	public static SelCharaGrowItemExchangeWindowCtrl HdlSelCharaGrowItemExchangeWindowCtrl
	{
		get
		{
			if (null == Singleton<CanvasManager>.Instance.selCharaGrowItemExchangeWindowCtrl)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "SelCharaGrowItemExchangeWindow";
				gameObject.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, true);
				gameObject.transform.localScale = Vector3.one;
				Singleton<CanvasManager>.Instance.SetRectTransformStretch(gameObject.AddComponent<RectTransform>());
				GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaGrow_ItemExchange"), gameObject.transform);
				SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, gameObject2.transform, true);
				Singleton<CanvasManager>.Instance.selCharaGrowItemExchangeWindowCtrl = gameObject.AddComponent<SelCharaGrowItemExchangeWindowCtrl>();
				Singleton<CanvasManager>.Instance.selCharaGrowItemExchangeWindowCtrl.Initialize(gameObject2.transform);
			}
			return Singleton<CanvasManager>.Instance.selCharaGrowItemExchangeWindowCtrl;
		}
	}

	public static CanvasManager.ReferenceResource RefResource
	{
		get
		{
			if (Singleton<CanvasManager>.Instance.referenseResource == null)
			{
				Singleton<CanvasManager>.Instance.referenseResource = new CanvasManager.ReferenceResource();
			}
			return Singleton<CanvasManager>.Instance.referenseResource;
		}
	}

	protected override void OnSingletonDestroy()
	{
		SGNFW.Touch.Manager.UnRegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
	}

	private void TouchEffectInit()
	{
		this.touchEffectObj = new GameObject();
		this.touchEffectObj.transform.SetParent(CanvasManager.MainCanvasOverlay.transform.Find("WindowPanel").transform, true);
		this.touchEffectObj.transform.localPosition = Vector3.zero;
		this.touchEffectObj.transform.localScale = Vector3.one;
		this.touchEffectObj.name = "TouchEffect";
		this.touchEffectList = new List<CanvasManager.TouchEffect>();
		for (int i = 0; i < 5; i++)
		{
			CanvasManager.TouchEffect touchEffect = new CanvasManager.TouchEffect();
			GameObject gameObject = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/Touch_Effect", this.touchEffectObj.transform);
			gameObject.name = "touchParticle";
			touchEffect.trans = gameObject.GetComponent<Transform>();
			touchEffect.particle = touchEffect.trans.GetComponent<ParticleSystem>();
			gameObject.SetActive(false);
			this.touchEffectList.Add(touchEffect);
		}
		SGNFW.Touch.Manager.RegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
	}

	public void TouchEffectRemove()
	{
		SGNFW.Touch.Manager.UnRegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
		this.touchEffectObj.SetActive(false);
	}

	private void OnTouchStart(Info info)
	{
		Camera worldCamera = CanvasManager.MainCanvasOverlay.worldCamera;
		RectTransform component = CanvasManager.MainCanvasOverlay.gameObject.GetComponent<RectTransform>();
		foreach (CanvasManager.TouchEffect touchEffect in this.touchEffectList)
		{
			if (!touchEffect.particle.isPlaying)
			{
				touchEffect.trans.gameObject.SetActive(true);
				Vector3 vector = worldCamera.ScreenToWorldPoint(info.CurrentPosition);
				Vector3 vector2 = worldCamera.WorldToViewportPoint(vector);
				Vector2 vector3 = new Vector2(vector2.x * component.sizeDelta.x - component.sizeDelta.x * 0.5f, vector2.y * component.sizeDelta.y - component.sizeDelta.y * 0.5f);
				touchEffect.trans.localPosition = vector3;
				touchEffect.particle.Play();
				break;
			}
		}
	}

	public void CloseFade()
	{
		if (this.fadeStatus == CanvasManager.FadeStatus.FADE_OUT_FINISH && this.fadeType == CanvasManager.FadeType.BATTLE_LOSE)
		{
			this.loseSe.Stop();
		}
		this.currentFadeAnimation = null;
		this.currentFadeAe = null;
		this.currentFadeAei = null;
		if (this.currentFade != null)
		{
			Object.Destroy(this.currentFade);
		}
		this.currentFade = null;
		this.SystemFadeMask.gameObject.SetActive(false);
		this.fadeStatus = CanvasManager.FadeStatus.FADE_IN_FINISH;
		this.fadeType = CanvasManager.FadeType.INVALID;
		this.requestFade = false;
		this.TouchFadeMask = false;
	}

	private void FadeUpdate()
	{
		switch (this.fadeStatus)
		{
		case CanvasManager.FadeStatus.FADE_OUT_PLAYING:
		{
			bool flag = true;
			if (this.currentFade != null)
			{
				if (this.currentFade.activeSelf)
				{
					if (this.currentFadeAnimation != null && this.currentFadeAnimation.ExIsPlaying())
					{
						flag = false;
					}
					if (this.currentFadeAe != null && this.currentFadeAe.IsPlaying())
					{
						flag = false;
					}
					if (this.currentFadeAei != null && !this.currentFadeAei.end)
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
					this.currentFade.SetActive(true);
					if (this.currentFadeAnimation != null)
					{
						this.currentFadeAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					}
					if (this.currentFadeAe != null)
					{
						this.currentFadeAe.PlayAnimation(PguiAECtrl.AmimeType.START, null);
					}
					if (this.currentFadeAei != null)
					{
						this.currentFadeAei.autoPlay = true;
						this.currentFadeAei.playOutTime = 1.65f;
						this.currentFadeAei.playTime = (this.currentFadeAei.playInTime = 0f);
					}
					this.TouchFadeMask = false;
					if (this.fadeType == CanvasManager.FadeType.BATTLE_LOSE)
					{
						SoundManager.Play("prd_se_quest_withdrawal_1", false, false);
					}
					else if (this.fadeType == CanvasManager.FadeType.SCENARIO)
					{
						SoundManager.Play("prd_se_selector_bus_wipeout", false, false);
					}
				}
			}
			if (flag || (this.currentFadeAe != null && this.TouchFadeMask))
			{
				if (this.currentFadeAe != null)
				{
					this.currentFadeAe.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				}
				if (this.fadeType == CanvasManager.FadeType.BATTLE_LOSE)
				{
					this.loseSe = SoundManager.Play("prd_se_quest_withdrawal_2", false, false);
				}
				this.fadeStatus = CanvasManager.FadeStatus.FADE_OUT_FINISH;
				this.requestFade = false;
				return;
			}
			break;
		}
		case CanvasManager.FadeStatus.FADE_OUT_FINISH:
			if (this.currentFadeAe != null && this.TouchFadeMask)
			{
				Transform transform = this.currentFadeAe.transform.parent.Find("Txt_Touch");
				if (transform != null)
				{
					transform.gameObject.SetActive(false);
				}
			}
			if (this.requestFade)
			{
				if (this.currentFadeAnimation != null)
				{
					this.currentFadeAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
				}
				if (this.currentFadeAe != null)
				{
					this.currentFadeAe.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				}
				if (this.currentFadeAei != null)
				{
					this.currentFadeAei.autoPlay = true;
					this.currentFadeAei.playOutTime = this.currentFadeAei.duration;
					this.currentFadeAei.playTime = (this.currentFadeAei.playInTime = 1.65f);
				}
				this.fadeStatus = CanvasManager.FadeStatus.FADE_IN_PLAYING;
				if (this.fadeType == CanvasManager.FadeType.BATTLE_LOSE)
				{
					this.loseSe.Stop();
					SoundManager.Play("prd_se_quest_withdrawal_3", false, false);
					return;
				}
			}
			break;
		case CanvasManager.FadeStatus.FADE_IN_PLAYING:
		{
			bool flag2 = true;
			if (this.currentFade != null && this.currentFade.activeSelf)
			{
				if (this.currentFadeAnimation != null && this.currentFadeAnimation.ExIsPlaying())
				{
					flag2 = false;
				}
				if (this.currentFadeAe != null && this.currentFadeAe.IsPlaying())
				{
					flag2 = false;
				}
				if (this.currentFadeAei != null && !this.currentFadeAei.end)
				{
					flag2 = false;
				}
			}
			if (flag2)
			{
				this.currentFadeAnimation = null;
				this.currentFadeAe = null;
				this.currentFadeAei = null;
				if (this.currentFade != null)
				{
					Object.Destroy(this.currentFade);
				}
				this.currentFade = null;
				this.SystemFadeMask.gameObject.SetActive(false);
				this.fadeStatus = CanvasManager.FadeStatus.FADE_IN_FINISH;
				this.requestFade = false;
				return;
			}
			break;
		}
		default:
			if (this.requestFade)
			{
				this.currentFadeAnimation = null;
				this.currentFadeAe = null;
				this.currentFadeAei = null;
				if (this.currentFade != null)
				{
					Object.Destroy(this.currentFade);
				}
				switch (this.fadeType)
				{
				case CanvasManager.FadeType.SCENARIO:
					this.currentFade = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneQuest/GUI/Prefab/GUI_QuestWipe"), this.SystemFadeMask);
					this.currentFade.AddComponent<SafeAreaScaler>();
					this.currentFadeAnimation = this.currentFade.GetComponent<SimpleAnimation>();
					goto IL_055A;
				case CanvasManager.FadeType.BATTLE_END:
					this.currentFade = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneBattle/GUI/Prefab/GUI_BattleEndWipe"), this.SystemFadeMask);
					this.currentFade.AddComponent<SafeAreaScaler>();
					this.currentFadeAnimation = this.currentFade.transform.Find("All").GetComponent<SimpleAnimation>();
					goto IL_055A;
				case CanvasManager.FadeType.BATTLE_LOSE:
					this.currentFade = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneBattle/GUI/Prefab/GUI_BattleQuitWipe"), this.SystemFadeMask);
					this.currentFade.AddComponent<SafeAreaScaler>();
					this.currentFadeAe = this.currentFade.transform.Find("AEImage_Quit").GetComponent<PguiAECtrl>();
					goto IL_055A;
				case CanvasManager.FadeType.SCENARIO2:
					this.currentFade = Object.Instantiate<GameObject>((GameObject)Resources.Load("Cmn/GUI/Prefab/Cmn_Fade_NoMark"), this.SystemFadeMask);
					this.currentFade.AddComponent<SafeAreaScaler>();
					this.currentFadeAei = this.currentFade.GetComponent<AEImage>();
					goto IL_055A;
				}
				this.currentFade = Object.Instantiate<GameObject>((GameObject)Resources.Load("Cmn/GUI/Prefab/GUI_Fade"), this.SystemFadeMask);
				this.currentFade.AddComponent<SafeAreaScaler>();
				this.currentFadeAnimation = this.currentFade.GetComponent<SimpleAnimation>();
				IL_055A:
				this.SystemFadeMask.gameObject.SetActive(true);
				this.currentFade.SetActive(false);
				this.fadeStatus = CanvasManager.FadeStatus.FADE_OUT_PLAYING;
				this.TouchFadeMask = false;
			}
			break;
		}
	}

	public static bool IsFinishFadeAction
	{
		get
		{
			return !Singleton<CanvasManager>.Instance.requestFade && (Singleton<CanvasManager>.Instance.fadeStatus == CanvasManager.FadeStatus.FADE_IN_FINISH || Singleton<CanvasManager>.Instance.fadeStatus == CanvasManager.FadeStatus.FADE_OUT_FINISH);
		}
	}

	public static bool IsTouchWaitFadeAction
	{
		get
		{
			return Singleton<CanvasManager>.Instance.currentFadeAe != null && !Singleton<CanvasManager>.Instance.TouchFadeMask;
		}
	}

	public static bool IsFadeOut
	{
		get
		{
			return Singleton<CanvasManager>.Instance.fadeStatus == CanvasManager.FadeStatus.FADE_OUT_PLAYING || Singleton<CanvasManager>.Instance.fadeStatus == CanvasManager.FadeStatus.FADE_OUT_FINISH;
		}
	}

	public static void RestartFade()
	{
		if (Singleton<CanvasManager>.Instance.requestFade || Singleton<CanvasManager>.Instance.fadeStatus != CanvasManager.FadeStatus.FADE_OUT_FINISH)
		{
			Verbose<PrjLog>.LogError(Singleton<CanvasManager>.Instance.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name + "() request overlapping error", null);
		}
		Singleton<CanvasManager>.Instance.requestFade = true;
		Singleton<CanvasManager>.Instance.fadeStatus = CanvasManager.FadeStatus.FADE_OUT_FINISH;
	}

	public static void RequestFade(CanvasManager.FadeType type = CanvasManager.FadeType.NORMAL)
	{
		if (Singleton<CanvasManager>.Instance.requestFade || Singleton<CanvasManager>.Instance.fadeStatus != CanvasManager.FadeStatus.FADE_IN_FINISH)
		{
			Verbose<PrjLog>.LogError(Singleton<CanvasManager>.Instance.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name + "() request overlapping error", null);
		}
		Singleton<CanvasManager>.Instance.requestFade = true;
		Singleton<CanvasManager>.Instance.fadeStatus = CanvasManager.FadeStatus.FADE_IN_FINISH;
		Singleton<CanvasManager>.Instance.fadeType = type;
	}

	public static void DestoryBgTexture()
	{
		for (int i = 1; i < Singleton<CanvasManager>.Instance.BgObjectPoolList.Count; i++)
		{
			GameObject gameObject = Singleton<CanvasManager>.Instance.BgObjectPoolList[i];
			Object.Destroy(gameObject);
			AssetManager.UnloadAssetData("GuiBg/Bg/" + gameObject.name, AssetManager.OWNER.CanvasBG);
		}
		Singleton<CanvasManager>.Instance.BgObjectPoolList.RemoveRange(1, Singleton<CanvasManager>.Instance.BgObjectPoolList.Count - 1);
	}

	public static void SetBgObj(string objName)
	{
		Singleton<CanvasManager>.Instance.changeBgObjFunc = CanvasManager.ChangeBgObj(objName != null, objName, null, null);
		Singleton<CanvasManager>.Instance.changeBgObjFunc.MoveNext();
	}

	public static void SetBgTexture(string textureName)
	{
		if (!string.IsNullOrEmpty(textureName))
		{
			textureName = "Texture2D/Bg_Scene/" + textureName;
		}
		Singleton<CanvasManager>.Instance.changeBgObjFunc = CanvasManager.ChangeBgObj(textureName != null, Singleton<CanvasManager>.Instance.PANEL_BG_SIMPLE, textureName, null);
		Singleton<CanvasManager>.Instance.changeBgObjFunc.MoveNext();
	}

	public static void SetScenarioBgInQuestBgTexture(string textureName)
	{
		Singleton<CanvasManager>.Instance.changeBgObjFunc = CanvasManager.ChangeBgObj(textureName != null, Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInQuestBg, textureName, null);
		Singleton<CanvasManager>.Instance.changeBgObjFunc.MoveNext();
	}

	public static void SetScenarioBgInSideStoryBgTexture(string textureName)
	{
		Singleton<CanvasManager>.Instance.changeBgObjFunc = CanvasManager.ChangeBgObj(textureName != null, Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInSideStoryBg, textureName, null);
		Singleton<CanvasManager>.Instance.changeBgObjFunc.MoveNext();
	}

	public static void SetBgTex(Texture tex)
	{
		Singleton<CanvasManager>.Instance.changeBgObjFunc = CanvasManager.ChangeBgObj(tex != null, Singleton<CanvasManager>.Instance.PANEL_BG_SIMPLE, null, tex);
		Singleton<CanvasManager>.Instance.changeBgObjFunc.MoveNext();
	}

	public static void SetBgEnable(bool isEnable)
	{
		Singleton<CanvasManager>.Instance.bgBoxObject.SetActive(isEnable);
	}

	public GameObject GetBg(string objName)
	{
		if (Singleton<CanvasManager>.Instance.bgBoxObject.transform.Find(objName + "/Texture_BG") == null)
		{
			return null;
		}
		return Singleton<CanvasManager>.Instance.bgBoxObject.transform.Find(objName + "/Texture_BG").gameObject;
	}

	private static IEnumerator ChangeBgObj(bool enable, string objName, string texName, Texture texture)
	{
		if (!Singleton<CanvasManager>.Instance.IsInitialized)
		{
			yield break;
		}
		if (!enable)
		{
			CanvasManager.SetBgEnable(false);
		}
		else
		{
			bool flag = enable && objName == Singleton<CanvasManager>.Instance.PANEL_BG_SIMPLE && !string.IsNullOrEmpty(texName);
			bool flag2 = enable && string.IsNullOrEmpty(texName) && texture == null;
			string assetName = "GuiBg/Bg/" + objName;
			if (flag || flag2)
			{
				Singleton<SceneManager>.Instance.LockByEnableSceneWaitFromEnableScene = true;
			}
			AssetManager.LoadAssetData(assetName, AssetManager.OWNER.CanvasBG, 0, null);
			while (!AssetManager.IsLoadFinishAssetData(assetName))
			{
				yield return null;
			}
			if (!string.IsNullOrEmpty(texName))
			{
				AssetManager.LoadAssetData(texName, AssetManager.OWNER.CanvasBG, 0, null);
				while (!AssetManager.IsLoadFinishAssetData(texName))
				{
					yield return null;
				}
			}
			CanvasManager.SetBgEnable(true);
			GameObject gameObject = null;
			foreach (GameObject gameObject2 in Singleton<CanvasManager>.Instance.BgObjectPoolList)
			{
				if (!(null == gameObject2))
				{
					if (objName == gameObject2.name)
					{
						gameObject2.SetActive(true);
						gameObject = gameObject2;
					}
					else
					{
						gameObject2.SetActive(false);
					}
				}
			}
			if (gameObject == null)
			{
				gameObject = AssetManager.InstantiateAssetData(assetName, Singleton<CanvasManager>.Instance.bgBoxObject.transform);
				gameObject.name = objName;
				PguiRawImageCtrl componentInChildren = gameObject.GetComponentInChildren<PguiRawImageCtrl>();
				if (componentInChildren != null)
				{
					RectTransform component = componentInChildren.GetComponent<RectTransform>();
					Vector2 sizeDelta = component.sizeDelta;
					component.anchorMin = new Vector2(0.5f, 0.5f);
					component.anchorMax = new Vector2(0.5f, 0.5f);
					component.pivot = new Vector2(0.5f, 0.5f);
					component.offsetMax = new Vector2(0f, 0f);
					component.offsetMin = new Vector2(0f, -44f);
					if (objName == Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInQuestBg || objName == Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInSideStoryBg)
					{
						component.sizeDelta = sizeDelta;
					}
					else
					{
						component.sizeDelta = new Vector2(1654f, 764f);
					}
				}
			}
			if (gameObject.name != Singleton<CanvasManager>.Instance.PANEL_BG_SIMPLE)
			{
				Singleton<CanvasManager>.Instance.BgObjectPoolList.Remove(gameObject);
				Singleton<CanvasManager>.Instance.BgObjectPoolList.Insert(1, gameObject);
				if (Singleton<CanvasManager>.Instance.BgObjectPoolList.Count > 3)
				{
					for (int i = 3; i < Singleton<CanvasManager>.Instance.BgObjectPoolList.Count; i++)
					{
						GameObject gameObject3 = Singleton<CanvasManager>.Instance.BgObjectPoolList[i];
						Object.Destroy(gameObject3);
						AssetManager.UnloadAssetData("GuiBg/Bg/" + gameObject3.name, AssetManager.OWNER.CanvasBG);
					}
					Singleton<CanvasManager>.Instance.BgObjectPoolList.RemoveRange(3, Singleton<CanvasManager>.Instance.BgObjectPoolList.Count - 3);
				}
				if ((objName == Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInQuestBg || objName == Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInSideStoryBg) && !string.IsNullOrEmpty(texName))
				{
					PguiRawImageCtrl componentInChildren2 = gameObject.GetComponentInChildren<PguiRawImageCtrl>();
					if (componentInChildren2 != null)
					{
						componentInChildren2.SetRawImage(texName, false, false, null);
					}
				}
				Singleton<SceneManager>.Instance.LockByEnableSceneWaitFromEnableScene = false;
			}
			else if (!string.IsNullOrEmpty(texName))
			{
				CanvasManager.<>c__DisplayClass326_0 CS$<>8__locals1 = new CanvasManager.<>c__DisplayClass326_0();
				CS$<>8__locals1.isSetFinish = false;
				Singleton<CanvasManager>.Instance.bgRawImage.SetRawImage(texName, false, false, delegate
				{
					CS$<>8__locals1.isSetFinish = true;
				});
				while (!CS$<>8__locals1.isSetFinish)
				{
					yield return null;
				}
				Singleton<SceneManager>.Instance.LockByEnableSceneWaitFromEnableScene = false;
				RectTransform component2 = Singleton<CanvasManager>.Instance.bgRawImage.gameObject.GetComponent<RectTransform>();
				component2.anchorMin = new Vector2(0.5f, 0.5f);
				component2.anchorMax = new Vector2(0.5f, 0.5f);
				component2.pivot = new Vector2(0.5f, 0.5f);
				component2.offsetMax = new Vector2(0f, 0f);
				component2.offsetMin = new Vector2(0f, -44f);
				component2.sizeDelta = new Vector2(1654f, 764f);
				CS$<>8__locals1 = null;
			}
			else if (texture != null)
			{
				Singleton<CanvasManager>.Instance.bgRawImage.SetTexture(texture, true);
				if (texture.width == 1654 && texture.height == 764)
				{
					RectTransform component3 = Singleton<CanvasManager>.Instance.bgRawImage.gameObject.GetComponent<RectTransform>();
					component3.anchorMin = new Vector2(0.5f, 0.5f);
					component3.anchorMax = new Vector2(0.5f, 0.5f);
					component3.pivot = new Vector2(0.5f, 0.5f);
					component3.offsetMax = new Vector2(0f, 0f);
					component3.offsetMin = new Vector2(0f, -44f);
					component3.sizeDelta = new Vector2((float)texture.width, (float)texture.height);
				}
				else
				{
					Singleton<CanvasManager>.Instance.SetRectTransformStretch(Singleton<CanvasManager>.Instance.bgRawImage.gameObject.GetComponent<RectTransform>());
				}
			}
			assetName = null;
		}
		yield break;
	}

	public void ChangeBg(RawImage bg, int idx, string panelName, bool isFixed, bool isOn, bool isFull, UnityAction callback, string targetName = "")
	{
		if (bg == null)
		{
			RawImage component = Singleton<CanvasManager>.Instance.GetBg(panelName).GetComponent<RawImage>();
			if (isFixed)
			{
				bg = component;
			}
			else
			{
				bg = component.transform.parent.Find(targetName).GetComponent<PguiRawImageCtrl>().m_RawImage;
			}
		}
		if (bg == null)
		{
			return;
		}
		if (this.currentFadeTimes[idx] >= CanvasManager.FADE_DURATION)
		{
			return;
		}
		float num;
		if (isFull)
		{
			if (isOn)
			{
				num = 0f;
			}
			else
			{
				num = 1f;
			}
		}
		else if (isOn)
		{
			num = Mathf.Clamp(1f - this.currentFadeTimes[idx] / CanvasManager.FADE_DURATION, 0f, 1f);
		}
		else
		{
			num = Mathf.Clamp(this.currentFadeTimes[idx] / CanvasManager.FADE_DURATION, 0f, 1f);
		}
		bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, num);
		List<float> list = this.currentFadeTimes;
		list[idx] += Time.deltaTime;
		if (this.currentFadeTimes[idx] >= CanvasManager.FADE_DURATION)
		{
			this.currentFadeTimes[idx] = CanvasManager.FADE_DURATION;
			if (callback != null)
			{
				callback();
			}
		}
	}

	public static void DisbleCanvasByTestScene()
	{
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern IntPtr FindWindow(string className, string windowName);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int EnumWindows(CanvasManager.EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int GetWindowText(IntPtr hWnd, string lpString, int nMaxCount);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int GetWindowTextLength(IntPtr hwnd);

	[DllImport("user32.dll")]
	private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

	[DllImport("user32.dll")]
	private static extern bool AdjustWindowRectEx(ref CanvasManager.WINRECT lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

	[DllImport("user32.dll")]
	private static extern bool GetWindowRect(IntPtr hwnd, out CanvasManager.WINRECT lpRect);

	[DllImport("user32.dll")]
	private static extern bool GetClientRect(IntPtr hwnd, out CanvasManager.WINRECT lpRect);

	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

	[DllImport("user32.dll")]
	private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool drow);

	private static void SetWindowProc()
	{
		int windowLong = CanvasManager.GetWindowLong(CanvasManager.winHdl, -16);
		CanvasManager.SetWindowLong(CanvasManager.winHdl, -16, windowLong & -65537);
		SceneManager.screenSize.width = Mathf.Clamp(SceneManager.screenSize.width, 640, 1600);
		SceneManager.screenSize.height = SceneManager.screenSize.width * 9 / 16;
		CanvasManager.WINRECT winrect;
		CanvasManager.GetClientRect(CanvasManager.winHdl, out winrect);
		CanvasManager.WINRECT winrect2;
		CanvasManager.GetWindowRect(CanvasManager.winHdl, out winrect2);
		int num = winrect2.width - winrect.width + SceneManager.screenSize.width;
		int num2 = winrect2.height - winrect.height + SceneManager.screenSize.height;
		CanvasManager.SetWindowPos(CanvasManager.winHdl, IntPtr.Zero, 0, 0, num, num2, 102U);
		IntPtr functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate<CanvasManager.WndProcDelegate>(new CanvasManager.WndProcDelegate(CanvasManager.WindowProc));
		CanvasManager.oldWndProc = ((IntPtr.Size == 8) ? CanvasManager.SetWindowLongPtr(CanvasManager.winHdl, -4, functionPointerForDelegate) : new IntPtr(CanvasManager.SetWindowLong(CanvasManager.winHdl, -4, functionPointerForDelegate.ToInt32())));
		Application.wantsToQuit += CanvasManager.WantsToQuit;
	}

	[DllImport("user32.dll")]
	private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("user32.dll")]
	private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	[DllImport("user32.dll")]
	private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll")]
	private static extern bool DestroyWindow(IntPtr hWnd);

	private static IntPtr WindowProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
	{
		if (msg != 16U && msg != 3U)
		{
			if (msg == 5U)
			{
				int num = lParam.ToInt32() & 65535;
				int num2 = (lParam.ToInt32() >> 16) & 65535;
				if (SceneManager.screenSize.width != num || SceneManager.screenSize.height != num2)
				{
					lParam = new IntPtr((SceneManager.screenSize.height << 16) | SceneManager.screenSize.width);
					CanvasManager.WINRECT winrect;
					CanvasManager.GetClientRect(hwnd, out winrect);
					CanvasManager.WINRECT winrect2;
					CanvasManager.GetWindowRect(hwnd, out winrect2);
					int num3 = winrect2.width - winrect.width + SceneManager.screenSize.width;
					int num4 = winrect2.height - winrect.height + SceneManager.screenSize.height;
					CanvasManager.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, num3, num4, 70U);
				}
			}
			else if (msg != 70U && msg != 71U && msg != 274U)
			{
				if (msg == 532U)
				{
					CanvasManager.WINRECT winrect3 = (CanvasManager.WINRECT)Marshal.PtrToStructure(lParam, typeof(CanvasManager.WINRECT));
					CanvasManager.WINRECT winrect4;
					CanvasManager.GetClientRect(hwnd, out winrect4);
					CanvasManager.WINRECT winrect5;
					CanvasManager.GetWindowRect(hwnd, out winrect5);
					int num5 = winrect5.width - winrect4.width;
					int num6 = winrect5.height - winrect4.height;
					int num7 = Mathf.Clamp(winrect3.width - num5, 640, 1600);
					int num8 = Mathf.Clamp(winrect3.height - num6, 360, 900);
					int num9 = wParam.ToInt32();
					int num10 = num8 * 16 / 9;
					int num11 = num7 * 9 / 16;
					if (num9 >= 9 || num9 <= 0)
					{
						num7 = SceneManager.screenSize.width;
						num8 = SceneManager.screenSize.height;
					}
					else if (num9 == 1 || num9 == 2)
					{
						num8 = num11;
					}
					else if (num9 == 3 || num9 == 6)
					{
						num7 = num10;
					}
					else if (num11 > num8)
					{
						num8 = num11;
					}
					else if (num10 > num7)
					{
						num7 = num10;
					}
					else
					{
						num7 = SceneManager.screenSize.width;
						num8 = SceneManager.screenSize.height;
					}
					if (num9 == 1 || num9 == 4 || num9 == 7)
					{
						winrect3.left = winrect3.right - num7 - num5;
					}
					else
					{
						winrect3.right = winrect3.left + num7 + num5;
					}
					if (num9 == 3 || num9 == 4 || num9 == 5)
					{
						winrect3.top = winrect3.bottom - num8 - num6;
					}
					else
					{
						winrect3.bottom = winrect3.top + num8 + num6;
					}
					SceneManager.screenSize.width = num7;
					SceneManager.screenSize.height = num8;
					Marshal.StructureToPtr<CanvasManager.WINRECT>(winrect3, lParam, false);
				}
			}
		}
		return CanvasManager.CallWindowProc(CanvasManager.oldWndProc, hwnd, msg, wParam, lParam);
	}

	private static bool EnumWindowProc(IntPtr hwnd, IntPtr lparam)
	{
		int num = 0;
		CanvasManager.GetWindowThreadProcessId(hwnd, out num);
		int windowTextLength = CanvasManager.GetWindowTextLength(hwnd);
		string text = new string('\0', windowTextLength + 1);
		CanvasManager.GetWindowText(hwnd, text, text.Length);
		if (CanvasManager.prcId.Id == num && text.IndexOf(Application.productName) >= 0)
		{
			CanvasManager.winHdl = hwnd;
		}
		return true;
	}

	private static bool WantsToQuit()
	{
		if (CanvasManager.appQuit != null)
		{
			return false;
		}
		if (CanvasManager.winClose < 0)
		{
			return true;
		}
		CanvasManager.winClose = 1;
		return false;
	}

	private static IEnumerator AppQuit()
	{
		GameObject owp = Object.Instantiate(Resources.Load("prefab/CmnOpenWindow")) as GameObject;
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.OVERLAY, owp.transform, true);
		PguiOpenWindowCtrl powc = owp.GetComponent<PguiOpenWindowCtrl>();
		powc.Setup("アプリ終了確認", "アプリを終了しますか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int idx)
		{
			CanvasManager.winClose = ((idx == 1) ? (-1) : 0);
			return true;
		}, null, false);
		powc.ForceOpen();
		do
		{
			yield return null;
		}
		while (CanvasManager.winClose > 0);
		while (!powc.FinishedClose())
		{
			yield return null;
		}
		Object.Destroy(owp);
		CanvasManager.appQuit = null;
		if (CanvasManager.winClose < 0)
		{
			Application.Quit();
		}
		yield break;
	}

	public static bool CheckInWindow(Vector2 pos)
	{
		if (CanvasManager.winHdl == IntPtr.Zero)
		{
			return false;
		}
		CanvasManager.WINRECT winrect;
		CanvasManager.GetClientRect(CanvasManager.winHdl, out winrect);
		return pos.x >= (float)winrect.left && pos.x <= (float)winrect.right && pos.y >= (float)winrect.top && pos.y <= (float)winrect.bottom;
	}

	private static string GetCampaignURLParam()
	{
		return CanvasManager.campaignURLParam;
	}

	private static void RemoveCampaignURLParam()
	{
		CanvasManager.campaignURLParam = null;
	}

	private static string GetCampaignUrl()
	{
		if (Singleton<LoginManager>.Instance == null)
		{
			return null;
		}
		string text = "campaign/CampaignTop.do";
		if (Singleton<LoginManager>.Instance.IsProd())
		{
			return "https://www.kemono-friends-3.jp/game/" + text;
		}
		if (Singleton<LoginManager>.Instance.IsStage())
		{
			return "https://www-preview.kemono-friends-3.jp/game/" + text;
		}
		string text2 = SGNFW.Http.Manager.ServerRoot["root"];
		string text3 = (text2.EndsWith("/") ? "" : "/");
		return text2 + text3 + text;
	}

	private void Update()
	{
		this.FadeUpdate();
		this.returnBackground = false;
		int screenWidth = SafeAreaScaler.ScreenWidth;
		int screenHeight = SafeAreaScaler.ScreenHeight;
		if (CanvasManager.prcId == null)
		{
			CanvasManager.prcId = Process.GetCurrentProcess();
		}
		else if (CanvasManager.winHdl == IntPtr.Zero)
		{
			CanvasManager.EnumWindows(new CanvasManager.EnumWindowsDelegate(CanvasManager.EnumWindowProc), IntPtr.Zero);
		}
		else if (CanvasManager.oldWndProc == IntPtr.Zero)
		{
			CanvasManager.SetWindowProc();
		}
		else if (CanvasManager.appQuit != null)
		{
			CanvasManager.appQuit.MoveNext();
		}
		else if (CanvasManager.winClose > 0)
		{
			CanvasManager.appQuit = CanvasManager.AppQuit();
		}
		if (this.changeBgObjFunc != null && !this.changeBgObjFunc.MoveNext())
		{
			this.changeBgObjFunc = null;
		}
		foreach (CanvasManager.TouchEffect touchEffect in this.touchEffectList)
		{
			if (touchEffect.trans.gameObject.activeSelf && !touchEffect.particle.isPlaying)
			{
				touchEffect.trans.gameObject.SetActive(false);
			}
		}
		if (this.outFrame != null && (this.outFrameW != screenWidth || this.outFrameH != screenHeight))
		{
			this.SetOutFrame(screenWidth, screenHeight);
		}
		if (SafeAreaScaler.ScreenWidth != this.oldScreenWidth || SafeAreaScaler.ScreenHeight != this.oldScreenHeight)
		{
			EventSystem.current.pixelDragThreshold = 11 * (SafeAreaScaler.ScreenWidth * SafeAreaScaler.ScreenHeight) / 921600;
			this.oldScreenWidth = SafeAreaScaler.ScreenWidth;
			this.oldScreenHeight = SafeAreaScaler.ScreenHeight;
		}
		if (this.bonusPhotoInfoWindowCtrl != null && this.bonusPhotoInfoWindowCtrl.WindowRectTransform.gameObject.activeInHierarchy && !this.bonusPhotoInfoWindowCtrl.FinishedOpen())
		{
			this.bonusPhotoInfoWindowCtrl.m_UserInfoContent.gameObject.SetActive(false);
			this.bonusPhotoInfoWindowCtrl.m_UserInfoContent.gameObject.SetActive(true);
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			return;
		}
		this.returnBackground = true;
	}

	private void SetOutFrame(int w, int h)
	{
		this.outFrameW = w;
		this.outFrameH = h;
		int num = 0;
		if (this.outFrameW >= this.outFrameH)
		{
			float num2 = (float)this.outFrameW / 16f;
			float num3 = (float)this.outFrameH / 9f;
			if (num3 >= num2)
			{
				num = 3;
			}
			else
			{
				num2 = (float)this.outFrameW / 19.5f;
				if (num2 > num3)
				{
					num = 5;
				}
			}
		}
		else
		{
			if (!SceneHome.nowVertView)
			{
				num = 3;
			}
			float num4 = (float)this.outFrameW / 9f;
			if ((float)this.outFrameH / 19.5f > num4)
			{
				num = 3;
			}
		}
		foreach (object obj in this.outFrame.transform)
		{
			Transform transform = (Transform)obj;
			int num5 = (int)(transform.name[transform.name.Length - 1] - '0');
			transform.gameObject.SetActive(num - 3 < num5 && num5 < num);
		}
	}

	public void SetDisplayDirection(int dir)
	{
		this.SetDisplayDirection(dir, this.beforeOrientation);
	}

	public void SetDisplayDirection(int dir, ScreenOrientation old)
	{
		if (SceneHome.nowVertView)
		{
			Screen.orientation = ScreenOrientation.Portrait;
			this.screenDirection = 3;
			return;
		}
		switch (dir)
		{
		case 0:
			if (Screen.orientation != ScreenOrientation.LandscapeLeft && Screen.orientation != ScreenOrientation.LandscapeRight)
			{
				Screen.orientation = old;
			}
			Screen.orientation = ScreenOrientation.AutoRotation;
			break;
		case 1:
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			break;
		case 2:
			Screen.orientation = ScreenOrientation.LandscapeRight;
			break;
		}
		this.screenDirection = dir;
	}

	public void SetDisplayPortrait()
	{
		if (this.screenDirection != 3 && (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight))
		{
			this.beforeOrientation = Screen.orientation;
		}
		Screen.orientation = ScreenOrientation.Portrait;
		this.screenDirection = 3;
	}

	public static bool IsDispDebug { get; set; }

	public static void settim()
	{
	}

	public Dictionary<PguiOpenWindowCtrl.WINDOW_TYPE, PguiOpenWindowCtrl> openWindowCtrlMap;

	private bool isDebugScene;

	private GameObject bgBoxObject;

	public readonly string PANEL_BG_SIMPLE = "PanelBg_Simple";

	public readonly string PanelBg_ScenarioBgInQuestBg = "PanelBg_ScenarioBgInQuestBg";

	public readonly string PanelBg_ScenarioBgInSideStoryBg = "PanelBg_ScenarioBgInSideStoryBg";

	public static readonly float FADE_DURATION = 0.75f;

	private List<float> currentFadeTimes = new List<float> { 0f, 0f };

	private PguiRawImageCtrl bgRawImage;

	private Transform SystemFadeMask;

	private bool TouchFadeMask;

	private GameObject serverConnectObj;

	private PguiCmnMenuCtrl cmnMenuCtrl;

	private GameObject questWindowCtrl;

	private SelPurchaseStoneWindowCtrl selPurchaseStoneWindowCtrl;

	private PguiOpenWindowCtrl setItemDetailWindow;

	private PguiOpenWindowCtrl sellItemInfoWindow;

	private PguiOpenWindowCtrl itemBankContentWindow;

	private SelCharaGrowWild.WindowWildResult shopWildReleaseWindow;

	private SelMonthlyPackWindowCtrl selMonthlyPackWindowCtrl;

	private SelMonthlyPackAfterWindowCtrl selMonthlyPackAfterWindowCtrl;

	private DressUpWindowCtrl dressUpWindowCtrl;

	private FollowWindowCtrl followWindowCtrl;

	private GachaWindowInfoCtrl gachaWindowInfoCtrl;

	private GachaWindowBoxInfoCtrl gachaWindowBoxInfoCtrl;

	private GachaWindowStepInfoCtrl gachaWindowStepInfoCtrl;

	private ItemPresetWindowCtrl itemPresetWindowCtrl;

	private CmnItemWindowCtrl cmnItemWindowCtrl;

	private SortWindowCtrl sortWindowCtrl;

	private AccessorySortWindowCtrl accessorySortWindowCtrl;

	private AccessoryFilterWindowCtrl accessoryFilterWindowCtrl;

	private StaminaRecoveryWindowCtrl staminaRecoveryWindowCtrl;

	private SelMissionProgressCtrl selMissionProgressCtrl;

	private WebViewWindowCtrl webViewWindowCtrl;

	private SelCharaDeckCtrl selCharaDeckCtrl;

	private HelpWindowCtrl helpWindowCtrl;

	private CmnFeedPageWindowCtrl cmnFeedPageWindowCtrl;

	private CharaWindowCtrl charaWindowCtrl;

	private SelCharaGrowPhotoPocket.ExchangeWarningWindow exchangeWarningWindow;

	private DressUpWipeCtrl dressUpWipeCtrl;

	private PguiOpenWindowCtrl errorWindowCtrl;

	private ItemDetailCtrl itemDetailCtrl;

	private AchievementDetailCtrl achievementDetailCtrl;

	private CmnReleaseConditionWindowCtrl cmnReleaseConditionWindowCtrl;

	private LoadAndTipsCtrl loadAndTipsCtrl;

	private PhotoWindowCtrl photoWindowCtrl;

	private StickerWindowCtrl stickerWindowCtrl;

	private SelQuestCountRecoveryWindowCtrl selQuestCountRecoveryWindowCtrl;

	private RewardListWindowCtrl rewardListWindowCtrl;

	private QuestSkipWindowsCtrl questSkipWindowsCtrl;

	private GameObject charaDeckWindow;

	private RecommendedDeckWindowCtrl recommendedDeckWindowCtrl;

	private PguiOpenWindowCtrl partyNameCtrl;

	private PguiOpenWindowCtrl userSkillCtrl;

	private SelTacticsSkillChangeWindowCtrl tacticsSkillChangeCtrl;

	private SelTacticsSkillInfoWindowCtrl tacticsSkillInfoCtrl;

	private PguiOpenWindowCtrl orderCardWindowCtrl;

	private PguiOpenWindowCtrl partyCautionCtrl;

	private PguiOpenWindowCtrl trainingEnemyInfoWindowCtrl;

	private PguiOpenWindowCtrl bonusPhotoInfoWindowCtrl;

	private CmnQuestSealedInfoWindowCtrl questSealedInfoWindowCtrl;

	private TreeHouseFurnitureWindowCtrl treeHouseFurnitureWindowCtrl;

	private AccessoryCheckWindowCtrl accessoryReleaseWindowCtrl;

	private AccessoryCheckWindowCtrl accessoryOwnerSettingWindowCtrl;

	private AccessoryCheckWindowCtrl accessoryCheckWindowCtrl;

	private DetachableAccessoryWindowCtrl detachableAccessoryWindowCtrl;

	private AccessoryWindowCtrl accessoryWindowCtrl;

	private AdvertiseBannerCtrl adevertiseBannerCtrl;

	private PurchaseConfirmWindow purchaseConfirmWindow;

	private PguiOpenWindowCtrl monthlyConfirmWindow;

	private PguiOpenWindowCtrl kizunaReachedLimitWindow;

	private CmnKizunaBuffWindowCtrl kizunaBuffWindowCtrl;

	private SelCharaGrowItemExchangeWindowCtrl selCharaGrowItemExchangeWindowCtrl;

	private IEnumerator changeBgObjFunc;

	public GameObject cmnTouchMask;

	public GameObject outFrame;

	private int outFrameW;

	private int outFrameH;

	private TutorialMaskCtrl tutorialMaskCtrl;

	private PhotoFilterWindowCtrl photoFilterWindowCtrl;

	private FriendsFilterWindowCtrl friendsFilterWindowCtrl;

	private StickerFilterWindowCtrl stickerFilterWindowCtrl;

	private CanvasManager.ReferenceResource referenseResource;

	private const int TOUCH_EFFECT_MAX = 5;

	private List<CanvasManager.TouchEffect> touchEffectList = new List<CanvasManager.TouchEffect>();

	private GameObject touchEffectObj;

	private CanvasManager.FadeStatus fadeStatus = CanvasManager.FadeStatus.FADE_IN_FINISH;

	private CanvasManager.FadeType fadeType;

	private bool requestFade;

	private SimpleAnimation currentFadeAnimation;

	private PguiAECtrl currentFadeAe;

	private AEImage currentFadeAei;

	private GameObject currentFade;

	private CriAtomExPlayback loseSe;

	private static IntPtr oldWndProc = IntPtr.Zero;

	private static Process prcId = null;

	private static IntPtr winHdl = IntPtr.Zero;

	public static int winClose = 0;

	private static IEnumerator appQuit = null;

	private bool returnBackground = true;

	private bool isFromCampaignUrl;

	private string lastCampaignUrlParam = "";

	private static string campaignURLParam = null;

	private const int BasePixelDragThreshold = 11;

	private const int BaseScreenNum = 921600;

	private int oldScreenWidth;

	private int oldScreenHeight;

	private ScreenOrientation beforeOrientation = ScreenOrientation.LandscapeLeft;

	private int screenDirection;

	private bool fpsSet = true;

	private static List<DateTime> dtl = new List<DateTime>();

	public class ReferenceResource
	{
		public ReferenceResource()
		{
			this.Icon_Chara = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Chara");
			this.Icon_Photo = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Photo");
			this.Icon_Photo_Mini = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Photo_Mini");
			this.Card_Photo = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Card_Photo");
			this.Icon_Item = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item");
			this.Icon_PhotoSet = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_PhotoSet");
			this.Icon_CharaSet = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_CharaSet");
			this.Icon_Accessory = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Accessory");
			this.Icon_Accessory_Mini = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Accessory_mini");
			this.Icon_AccessorySet = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_AccessorySet");
			this.Icon_Sticker = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Sticker");
			this.Card_Sticker = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Card_Sticker");
		}

		public GameObject Icon_Chara;

		public GameObject Icon_Photo;

		public GameObject Icon_Photo_Mini;

		public GameObject Card_Photo;

		public GameObject Icon_Item;

		public GameObject Icon_PhotoSet;

		public GameObject Icon_CharaSet;

		public GameObject Icon_Accessory;

		public GameObject Icon_Accessory_Mini;

		public GameObject Icon_AccessorySet;

		public GameObject Icon_Sticker;

		public GameObject Card_Sticker;
	}

	private class TouchEffect
	{
		public Transform trans;

		public ParticleSystem particle;
	}

	public enum FadeType
	{
		INVALID,
		NORMAL,
		SCENARIO,
		TIPS,
		BATTLE_END,
		BATTLE_LOSE,
		SCENARIO2
	}

	private enum FadeStatus
	{
		FADE_OUT_PLAYING,
		FADE_OUT_FINISH,
		FADE_IN_PLAYING,
		FADE_IN_FINISH
	}

	private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

	public struct WINRECT
	{
		public int width
		{
			get
			{
				return this.right - this.left;
			}
		}

		public int height
		{
			get
			{
				return this.bottom - this.top;
			}
		}

		public int left;

		public int top;

		public int right;

		public int bottom;
	}

	private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
}
