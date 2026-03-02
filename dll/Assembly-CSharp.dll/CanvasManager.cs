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

// Token: 0x020000EF RID: 239
public class CanvasManager : Singleton<CanvasManager>
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x000401A0 File Offset: 0x0003E3A0
	// (set) Token: 0x06000AF2 RID: 2802 RVA: 0x000401A7 File Offset: 0x0003E3A7
	private static Canvas MainCanvasOverlay { get; set; }

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x000401AF File Offset: 0x0003E3AF
	// (set) Token: 0x06000AF4 RID: 2804 RVA: 0x000401B6 File Offset: 0x0003E3B6
	private static Canvas MainCanvasSystem { get; set; }

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x000401BE File Offset: 0x0003E3BE
	// (set) Token: 0x06000AF6 RID: 2806 RVA: 0x000401C5 File Offset: 0x0003E3C5
	private static Canvas MainCanvasFront { get; set; }

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x000401CD File Offset: 0x0003E3CD
	// (set) Token: 0x06000AF8 RID: 2808 RVA: 0x000401D4 File Offset: 0x0003E3D4
	private static Canvas MainCanvasBack { get; set; }

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x000401DC File Offset: 0x0003E3DC
	// (set) Token: 0x06000AFA RID: 2810 RVA: 0x000401E4 File Offset: 0x0003E3E4
	public Transform SystemMiddleArea { get; private set; }

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06000AFB RID: 2811 RVA: 0x000401ED File Offset: 0x0003E3ED
	// (set) Token: 0x06000AFC RID: 2812 RVA: 0x000401F5 File Offset: 0x0003E3F5
	public Transform OverlayWindowPanel { get; private set; }

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06000AFD RID: 2813 RVA: 0x000401FE File Offset: 0x0003E3FE
	// (set) Token: 0x06000AFE RID: 2814 RVA: 0x00040206 File Offset: 0x0003E406
	public bool IsInitialized { get; private set; }

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06000AFF RID: 2815 RVA: 0x0004020F File Offset: 0x0003E40F
	// (set) Token: 0x06000B00 RID: 2816 RVA: 0x00040217 File Offset: 0x0003E417
	public GameObject SystemPanel { get; private set; }

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00040220 File Offset: 0x0003E420
	// (set) Token: 0x06000B02 RID: 2818 RVA: 0x00040228 File Offset: 0x0003E428
	public GameObject CommonWindow { get; private set; }

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00040231 File Offset: 0x0003E431
	// (set) Token: 0x06000B04 RID: 2820 RVA: 0x00040239 File Offset: 0x0003E439
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

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00040242 File Offset: 0x0003E442
	// (set) Token: 0x06000B06 RID: 2822 RVA: 0x0004024A File Offset: 0x0003E44A
	private List<GameObject> BgObjectPoolList { get; set; }

	// Token: 0x06000B07 RID: 2823 RVA: 0x00040254 File Offset: 0x0003E454
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

	// Token: 0x06000B08 RID: 2824 RVA: 0x000402A4 File Offset: 0x0003E4A4
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

	// Token: 0x06000B09 RID: 2825 RVA: 0x00040334 File Offset: 0x0003E534
	public static void RecursiveDestroy(GameObject go)
	{
		foreach (object obj in go.transform)
		{
			Transform transform = (Transform)obj;
			CanvasManager.RecursiveDestroy(transform.gameObject);
			Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x0004039C File Offset: 0x0003E59C
	private GameObject CanvasInstantiate(string canvasName, Vector2 ancPos)
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("prefab/PguiCanvas"));
		gameObject.name = canvasName;
		(gameObject.transform as RectTransform).anchoredPosition = ancPos;
		return gameObject;
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x000403CA File Offset: 0x0003E5CA
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

	// Token: 0x06000B0C RID: 2828 RVA: 0x00040404 File Offset: 0x0003E604
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

	// Token: 0x06000B0D RID: 2829 RVA: 0x000407E5 File Offset: 0x0003E9E5
	private void CanvasOverlayInstantiate()
	{
		this.TouchEffectInit();
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x000407F0 File Offset: 0x0003E9F0
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

	// Token: 0x06000B0F RID: 2831 RVA: 0x00040B80 File Offset: 0x0003ED80
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

	// Token: 0x06000B10 RID: 2832 RVA: 0x00040C5E File Offset: 0x0003EE5E
	public static void SetEnableCmnTouchMask(bool isEnable)
	{
		if (Singleton<CanvasManager>.Instance.cmnTouchMask)
		{
			Singleton<CanvasManager>.Instance.cmnTouchMask.SetActive(isEnable);
		}
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x00040C81 File Offset: 0x0003EE81
	public static void AddCallbackCmnTouchMask(UnityAction<Transform> callback)
	{
		if (Singleton<CanvasManager>.Instance.cmnTouchMask)
		{
			PrjUtil.AddTouchEventTrigger(Singleton<CanvasManager>.Instance.cmnTouchMask, callback);
		}
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x00040CA4 File Offset: 0x0003EEA4
	public static void RemoveCallbackCmnTouchMask()
	{
		if (Singleton<CanvasManager>.Instance.cmnTouchMask)
		{
			PrjUtil.RemoveTouchEventTrigger(Singleton<CanvasManager>.Instance.cmnTouchMask);
		}
	}

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06000B13 RID: 2835 RVA: 0x00040CC8 File Offset: 0x0003EEC8
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

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06000B14 RID: 2836 RVA: 0x00040D4C File Offset: 0x0003EF4C
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

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00040DD4 File Offset: 0x0003EFD4
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

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00040EAC File Offset: 0x0003F0AC
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

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00040FA4 File Offset: 0x0003F1A4
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

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00041048 File Offset: 0x0003F248
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

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06000B19 RID: 2841 RVA: 0x000410B0 File Offset: 0x0003F2B0
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

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06000B1A RID: 2842 RVA: 0x00041118 File Offset: 0x0003F318
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

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00041174 File Offset: 0x0003F374
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

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06000B1C RID: 2844 RVA: 0x000411C8 File Offset: 0x0003F3C8
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

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00041318 File Offset: 0x0003F518
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

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06000B1E RID: 2846 RVA: 0x000413B0 File Offset: 0x0003F5B0
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

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06000B1F RID: 2847 RVA: 0x0004140C File Offset: 0x0003F60C
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

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06000B20 RID: 2848 RVA: 0x000414A8 File Offset: 0x0003F6A8
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

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06000B21 RID: 2849 RVA: 0x0004150C File Offset: 0x0003F70C
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

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00041570 File Offset: 0x0003F770
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

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06000B23 RID: 2851 RVA: 0x000415FE File Offset: 0x0003F7FE
	public static SelMissionProgressCtrl HdlMissionProgressCtrl
	{
		get
		{
			return Singleton<CanvasManager>.Instance.selMissionProgressCtrl;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06000B24 RID: 2852 RVA: 0x0004160A File Offset: 0x0003F80A
	public static GameObject HdlServerConnectObj
	{
		get
		{
			return Singleton<CanvasManager>.Instance.serverConnectObj;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06000B25 RID: 2853 RVA: 0x00041616 File Offset: 0x0003F816
	public static PguiCmnMenuCtrl HdlCmnMenu
	{
		get
		{
			return Singleton<CanvasManager>.Instance.cmnMenuCtrl;
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06000B26 RID: 2854 RVA: 0x00041622 File Offset: 0x0003F822
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

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00041651 File Offset: 0x0003F851
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

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06000B28 RID: 2856 RVA: 0x00041680 File Offset: 0x0003F880
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

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06000B29 RID: 2857 RVA: 0x000416AF File Offset: 0x0003F8AF
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

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06000B2A RID: 2858 RVA: 0x000416DE File Offset: 0x0003F8DE
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

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x06000B2B RID: 2859 RVA: 0x00041710 File Offset: 0x0003F910
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

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00041774 File Offset: 0x0003F974
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

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06000B2D RID: 2861 RVA: 0x000417DC File Offset: 0x0003F9DC
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

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06000B2E RID: 2862 RVA: 0x00041844 File Offset: 0x0003FA44
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

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06000B2F RID: 2863 RVA: 0x00041878 File Offset: 0x0003FA78
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
			}
			return Singleton<CanvasManager>.Instance.sortWindowCtrl;
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06000B30 RID: 2864 RVA: 0x000419C0 File Offset: 0x0003FBC0
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

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06000B31 RID: 2865 RVA: 0x00041A48 File Offset: 0x0003FC48
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

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06000B32 RID: 2866 RVA: 0x00041AD0 File Offset: 0x0003FCD0
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

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06000B33 RID: 2867 RVA: 0x00041B55 File Offset: 0x0003FD55
	public static PhotoFilterWindowCtrl HdlPhotoFilterWindowCtrl
	{
		get
		{
			return Singleton<CanvasManager>.Instance.photoFilterWindowCtrl;
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06000B34 RID: 2868 RVA: 0x00041B61 File Offset: 0x0003FD61
	public static FriendsFilterWindowCtrl HdlFriendsFilterWindowCtrl
	{
		get
		{
			return Singleton<CanvasManager>.Instance.friendsFilterWindowCtrl;
		}
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00041B70 File Offset: 0x0003FD70
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

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00041BF8 File Offset: 0x0003FDF8
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

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06000B37 RID: 2871 RVA: 0x00041CCC File Offset: 0x0003FECC
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

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00041D24 File Offset: 0x0003FF24
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

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06000B39 RID: 2873 RVA: 0x00041DAC File Offset: 0x0003FFAC
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

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06000B3A RID: 2874 RVA: 0x00041E34 File Offset: 0x00040034
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

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06000B3B RID: 2875 RVA: 0x00041E8C File Offset: 0x0004008C
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

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00041EE4 File Offset: 0x000400E4
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

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00041F4C File Offset: 0x0004014C
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

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00041F98 File Offset: 0x00040198
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

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00042018 File Offset: 0x00040218
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

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00042094 File Offset: 0x00040294
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

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0004210C File Offset: 0x0004030C
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

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06000B42 RID: 2882 RVA: 0x00042184 File Offset: 0x00040384
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

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06000B43 RID: 2883 RVA: 0x000421FC File Offset: 0x000403FC
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

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06000B44 RID: 2884 RVA: 0x00042274 File Offset: 0x00040474
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

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06000B45 RID: 2885 RVA: 0x00042328 File Offset: 0x00040528
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

	// Token: 0x06000B46 RID: 2886 RVA: 0x000423B0 File Offset: 0x000405B0
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

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0004243C File Offset: 0x0004063C
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

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06000B48 RID: 2888 RVA: 0x000424B0 File Offset: 0x000406B0
	public static WebViewWindowCtrl HdlWebViewWindowCtrl
	{
		get
		{
			return Singleton<CanvasManager>.Instance.webViewWindowCtrl;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06000B49 RID: 2889 RVA: 0x000424BC File Offset: 0x000406BC
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

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00042520 File Offset: 0x00040720
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

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06000B4B RID: 2891 RVA: 0x000425E0 File Offset: 0x000407E0
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

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06000B4C RID: 2892 RVA: 0x000426DC File Offset: 0x000408DC
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

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06000B4D RID: 2893 RVA: 0x00042778 File Offset: 0x00040978
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

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06000B4E RID: 2894 RVA: 0x00042800 File Offset: 0x00040A00
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

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x06000B4F RID: 2895 RVA: 0x00042888 File Offset: 0x00040A88
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

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06000B50 RID: 2896 RVA: 0x0004294C File Offset: 0x00040B4C
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

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06000B51 RID: 2897 RVA: 0x000429AC File Offset: 0x00040BAC
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

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06000B52 RID: 2898 RVA: 0x000429FC File Offset: 0x00040BFC
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

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00042A5C File Offset: 0x00040C5C
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

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00042ABC File Offset: 0x00040CBC
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

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06000B55 RID: 2901 RVA: 0x00042B40 File Offset: 0x00040D40
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

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06000B56 RID: 2902 RVA: 0x00042C04 File Offset: 0x00040E04
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

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06000B57 RID: 2903 RVA: 0x00042C94 File Offset: 0x00040E94
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

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06000B58 RID: 2904 RVA: 0x00042D2C File Offset: 0x00040F2C
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

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06000B59 RID: 2905 RVA: 0x00042DB8 File Offset: 0x00040FB8
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

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06000B5A RID: 2906 RVA: 0x00042E14 File Offset: 0x00041014
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

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06000B5B RID: 2907 RVA: 0x00042E94 File Offset: 0x00041094
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

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06000B5C RID: 2908 RVA: 0x00042F59 File Offset: 0x00041159
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

	// Token: 0x06000B5D RID: 2909 RVA: 0x00042F80 File Offset: 0x00041180
	protected override void OnSingletonDestroy()
	{
		SGNFW.Touch.Manager.UnRegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x00042F94 File Offset: 0x00041194
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

	// Token: 0x06000B5F RID: 2911 RVA: 0x0004308F File Offset: 0x0004128F
	public void TouchEffectRemove()
	{
		SGNFW.Touch.Manager.UnRegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
		this.touchEffectObj.SetActive(false);
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x000430B0 File Offset: 0x000412B0
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

	// Token: 0x06000B61 RID: 2913 RVA: 0x000431CC File Offset: 0x000413CC
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

	// Token: 0x06000B62 RID: 2914 RVA: 0x00043258 File Offset: 0x00041458
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

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06000B63 RID: 2915 RVA: 0x000437EA File Offset: 0x000419EA
	public static bool IsFinishFadeAction
	{
		get
		{
			return !Singleton<CanvasManager>.Instance.requestFade && (Singleton<CanvasManager>.Instance.fadeStatus == CanvasManager.FadeStatus.FADE_IN_FINISH || Singleton<CanvasManager>.Instance.fadeStatus == CanvasManager.FadeStatus.FADE_OUT_FINISH);
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06000B64 RID: 2916 RVA: 0x00043816 File Offset: 0x00041A16
	public static bool IsTouchWaitFadeAction
	{
		get
		{
			return Singleton<CanvasManager>.Instance.currentFadeAe != null && !Singleton<CanvasManager>.Instance.TouchFadeMask;
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00043839 File Offset: 0x00041A39
	public static bool IsFadeOut
	{
		get
		{
			return Singleton<CanvasManager>.Instance.fadeStatus == CanvasManager.FadeStatus.FADE_OUT_PLAYING || Singleton<CanvasManager>.Instance.fadeStatus == CanvasManager.FadeStatus.FADE_OUT_FINISH;
		}
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x00043858 File Offset: 0x00041A58
	public static void RestartFade()
	{
		if (Singleton<CanvasManager>.Instance.requestFade || Singleton<CanvasManager>.Instance.fadeStatus != CanvasManager.FadeStatus.FADE_OUT_FINISH)
		{
			Verbose<PrjLog>.LogError(Singleton<CanvasManager>.Instance.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name + "() request overlapping error", null);
		}
		Singleton<CanvasManager>.Instance.requestFade = true;
		Singleton<CanvasManager>.Instance.fadeStatus = CanvasManager.FadeStatus.FADE_OUT_FINISH;
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x000438C4 File Offset: 0x00041AC4
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

	// Token: 0x06000B68 RID: 2920 RVA: 0x0004393C File Offset: 0x00041B3C
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

	// Token: 0x06000B69 RID: 2921 RVA: 0x000439B2 File Offset: 0x00041BB2
	public static void SetBgObj(string objName)
	{
		Singleton<CanvasManager>.Instance.changeBgObjFunc = CanvasManager.ChangeBgObj(objName != null, objName, null, null);
		Singleton<CanvasManager>.Instance.changeBgObjFunc.MoveNext();
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x000439DC File Offset: 0x00041BDC
	public static void SetBgTexture(string textureName)
	{
		if (!string.IsNullOrEmpty(textureName))
		{
			textureName = "Texture2D/Bg_Scene/" + textureName;
		}
		Singleton<CanvasManager>.Instance.changeBgObjFunc = CanvasManager.ChangeBgObj(textureName != null, Singleton<CanvasManager>.Instance.PANEL_BG_SIMPLE, textureName, null);
		Singleton<CanvasManager>.Instance.changeBgObjFunc.MoveNext();
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x00043A2D File Offset: 0x00041C2D
	public static void SetScenarioBgInQuestBgTexture(string textureName)
	{
		Singleton<CanvasManager>.Instance.changeBgObjFunc = CanvasManager.ChangeBgObj(textureName != null, Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInQuestBg, textureName, null);
		Singleton<CanvasManager>.Instance.changeBgObjFunc.MoveNext();
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x00043A5E File Offset: 0x00041C5E
	public static void SetScenarioBgInSideStoryBgTexture(string textureName)
	{
		Singleton<CanvasManager>.Instance.changeBgObjFunc = CanvasManager.ChangeBgObj(textureName != null, Singleton<CanvasManager>.Instance.PanelBg_ScenarioBgInSideStoryBg, textureName, null);
		Singleton<CanvasManager>.Instance.changeBgObjFunc.MoveNext();
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x00043A8F File Offset: 0x00041C8F
	public static void SetBgTex(Texture tex)
	{
		Singleton<CanvasManager>.Instance.changeBgObjFunc = CanvasManager.ChangeBgObj(tex != null, Singleton<CanvasManager>.Instance.PANEL_BG_SIMPLE, null, tex);
		Singleton<CanvasManager>.Instance.changeBgObjFunc.MoveNext();
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x00043AC3 File Offset: 0x00041CC3
	public static void SetBgEnable(bool isEnable)
	{
		Singleton<CanvasManager>.Instance.bgBoxObject.SetActive(isEnable);
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x00043AD8 File Offset: 0x00041CD8
	public GameObject GetBg(string objName)
	{
		if (Singleton<CanvasManager>.Instance.bgBoxObject.transform.Find(objName + "/Texture_BG") == null)
		{
			return null;
		}
		return Singleton<CanvasManager>.Instance.bgBoxObject.transform.Find(objName + "/Texture_BG").gameObject;
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x00043B32 File Offset: 0x00041D32
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
				CanvasManager.<>c__DisplayClass320_0 CS$<>8__locals1 = new CanvasManager.<>c__DisplayClass320_0();
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

	// Token: 0x06000B71 RID: 2929 RVA: 0x00043B58 File Offset: 0x00041D58
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

	// Token: 0x06000B72 RID: 2930 RVA: 0x00043CA5 File Offset: 0x00041EA5
	public static void DisbleCanvasByTestScene()
	{
	}

	// Token: 0x06000B73 RID: 2931
	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern IntPtr FindWindow(string className, string windowName);

	// Token: 0x06000B74 RID: 2932
	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int EnumWindows(CanvasManager.EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

	// Token: 0x06000B75 RID: 2933
	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int GetWindowText(IntPtr hWnd, string lpString, int nMaxCount);

	// Token: 0x06000B76 RID: 2934
	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int GetWindowTextLength(IntPtr hwnd);

	// Token: 0x06000B77 RID: 2935
	[DllImport("user32.dll")]
	private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

	// Token: 0x06000B78 RID: 2936
	[DllImport("user32.dll")]
	private static extern bool AdjustWindowRectEx(ref CanvasManager.WINRECT lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

	// Token: 0x06000B79 RID: 2937
	[DllImport("user32.dll")]
	private static extern bool GetWindowRect(IntPtr hwnd, out CanvasManager.WINRECT lpRect);

	// Token: 0x06000B7A RID: 2938
	[DllImport("user32.dll")]
	private static extern bool GetClientRect(IntPtr hwnd, out CanvasManager.WINRECT lpRect);

	// Token: 0x06000B7B RID: 2939
	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

	// Token: 0x06000B7C RID: 2940
	[DllImport("user32.dll")]
	private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool drow);

	// Token: 0x06000B7D RID: 2941 RVA: 0x00043CA8 File Offset: 0x00041EA8
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

	// Token: 0x06000B7E RID: 2942
	[DllImport("user32.dll")]
	private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	// Token: 0x06000B7F RID: 2943
	[DllImport("user32.dll")]
	private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	// Token: 0x06000B80 RID: 2944
	[DllImport("user32.dll")]
	private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	// Token: 0x06000B81 RID: 2945
	[DllImport("user32.dll")]
	private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

	// Token: 0x06000B82 RID: 2946
	[DllImport("user32.dll")]
	private static extern bool DestroyWindow(IntPtr hWnd);

	// Token: 0x06000B83 RID: 2947 RVA: 0x00043DD4 File Offset: 0x00041FD4
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

	// Token: 0x06000B84 RID: 2948 RVA: 0x0004409C File Offset: 0x0004229C
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

	// Token: 0x06000B85 RID: 2949 RVA: 0x000440F5 File Offset: 0x000422F5
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

	// Token: 0x06000B86 RID: 2950 RVA: 0x00044111 File Offset: 0x00042311
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

	// Token: 0x06000B87 RID: 2951 RVA: 0x0004411C File Offset: 0x0004231C
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

	// Token: 0x06000B88 RID: 2952 RVA: 0x0004418A File Offset: 0x0004238A
	private static string GetCampaignURLParam()
	{
		return CanvasManager.campaignURLParam;
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x00044191 File Offset: 0x00042391
	private static void RemoveCampaignURLParam()
	{
		CanvasManager.campaignURLParam = null;
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x0004419C File Offset: 0x0004239C
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

	// Token: 0x06000B8B RID: 2955 RVA: 0x00044220 File Offset: 0x00042420
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

	// Token: 0x06000B8C RID: 2956 RVA: 0x00044420 File Offset: 0x00042620
	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			return;
		}
		this.returnBackground = true;
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x00044430 File Offset: 0x00042630
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

	// Token: 0x06000B8E RID: 2958 RVA: 0x0004454C File Offset: 0x0004274C
	public void SetDisplayDirection(int dir)
	{
		this.SetDisplayDirection(dir, this.beforeOrientation);
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x0004455C File Offset: 0x0004275C
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

	// Token: 0x06000B90 RID: 2960 RVA: 0x000445C5 File Offset: 0x000427C5
	public void SetDisplayPortrait()
	{
		if (this.screenDirection != 3 && (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight))
		{
			this.beforeOrientation = Screen.orientation;
		}
		Screen.orientation = ScreenOrientation.Portrait;
		this.screenDirection = 3;
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06000B91 RID: 2961 RVA: 0x000445F8 File Offset: 0x000427F8
	// (set) Token: 0x06000B92 RID: 2962 RVA: 0x000445FF File Offset: 0x000427FF
	public static bool IsDispDebug { get; set; }

	// Token: 0x06000B93 RID: 2963 RVA: 0x00044607 File Offset: 0x00042807
	public static void settim()
	{
	}

	// Token: 0x0400089F RID: 2207
	public Dictionary<PguiOpenWindowCtrl.WINDOW_TYPE, PguiOpenWindowCtrl> openWindowCtrlMap;

	// Token: 0x040008A2 RID: 2210
	private bool isDebugScene;

	// Token: 0x040008A3 RID: 2211
	private GameObject bgBoxObject;

	// Token: 0x040008A4 RID: 2212
	public readonly string PANEL_BG_SIMPLE = "PanelBg_Simple";

	// Token: 0x040008A5 RID: 2213
	public readonly string PanelBg_ScenarioBgInQuestBg = "PanelBg_ScenarioBgInQuestBg";

	// Token: 0x040008A6 RID: 2214
	public readonly string PanelBg_ScenarioBgInSideStoryBg = "PanelBg_ScenarioBgInSideStoryBg";

	// Token: 0x040008A7 RID: 2215
	public static readonly float FADE_DURATION = 0.75f;

	// Token: 0x040008A8 RID: 2216
	private List<float> currentFadeTimes = new List<float> { 0f, 0f };

	// Token: 0x040008A9 RID: 2217
	private PguiRawImageCtrl bgRawImage;

	// Token: 0x040008AB RID: 2219
	private Transform SystemFadeMask;

	// Token: 0x040008AC RID: 2220
	private bool TouchFadeMask;

	// Token: 0x040008AD RID: 2221
	private GameObject serverConnectObj;

	// Token: 0x040008AE RID: 2222
	private PguiCmnMenuCtrl cmnMenuCtrl;

	// Token: 0x040008AF RID: 2223
	private GameObject questWindowCtrl;

	// Token: 0x040008B0 RID: 2224
	private SelPurchaseStoneWindowCtrl selPurchaseStoneWindowCtrl;

	// Token: 0x040008B1 RID: 2225
	private PguiOpenWindowCtrl setItemDetailWindow;

	// Token: 0x040008B2 RID: 2226
	private PguiOpenWindowCtrl sellItemInfoWindow;

	// Token: 0x040008B3 RID: 2227
	private PguiOpenWindowCtrl itemBankContentWindow;

	// Token: 0x040008B4 RID: 2228
	private SelCharaGrowWild.WindowWildResult shopWildReleaseWindow;

	// Token: 0x040008B5 RID: 2229
	private SelMonthlyPackWindowCtrl selMonthlyPackWindowCtrl;

	// Token: 0x040008B6 RID: 2230
	private SelMonthlyPackAfterWindowCtrl selMonthlyPackAfterWindowCtrl;

	// Token: 0x040008B7 RID: 2231
	private DressUpWindowCtrl dressUpWindowCtrl;

	// Token: 0x040008B8 RID: 2232
	private FollowWindowCtrl followWindowCtrl;

	// Token: 0x040008B9 RID: 2233
	private GachaWindowInfoCtrl gachaWindowInfoCtrl;

	// Token: 0x040008BA RID: 2234
	private GachaWindowBoxInfoCtrl gachaWindowBoxInfoCtrl;

	// Token: 0x040008BB RID: 2235
	private GachaWindowStepInfoCtrl gachaWindowStepInfoCtrl;

	// Token: 0x040008BC RID: 2236
	private ItemPresetWindowCtrl itemPresetWindowCtrl;

	// Token: 0x040008BD RID: 2237
	private CmnItemWindowCtrl cmnItemWindowCtrl;

	// Token: 0x040008BE RID: 2238
	private SortWindowCtrl sortWindowCtrl;

	// Token: 0x040008BF RID: 2239
	private AccessorySortWindowCtrl accessorySortWindowCtrl;

	// Token: 0x040008C0 RID: 2240
	private AccessoryFilterWindowCtrl accessoryFilterWindowCtrl;

	// Token: 0x040008C1 RID: 2241
	private StaminaRecoveryWindowCtrl staminaRecoveryWindowCtrl;

	// Token: 0x040008C2 RID: 2242
	private SelMissionProgressCtrl selMissionProgressCtrl;

	// Token: 0x040008C3 RID: 2243
	private WebViewWindowCtrl webViewWindowCtrl;

	// Token: 0x040008C4 RID: 2244
	private SelCharaDeckCtrl selCharaDeckCtrl;

	// Token: 0x040008C5 RID: 2245
	private HelpWindowCtrl helpWindowCtrl;

	// Token: 0x040008C6 RID: 2246
	private CmnFeedPageWindowCtrl cmnFeedPageWindowCtrl;

	// Token: 0x040008C7 RID: 2247
	private CharaWindowCtrl charaWindowCtrl;

	// Token: 0x040008C8 RID: 2248
	private SelCharaGrowPhotoPocket.ExchangeWarningWindow exchangeWarningWindow;

	// Token: 0x040008C9 RID: 2249
	private DressUpWipeCtrl dressUpWipeCtrl;

	// Token: 0x040008CA RID: 2250
	private PguiOpenWindowCtrl errorWindowCtrl;

	// Token: 0x040008CB RID: 2251
	private ItemDetailCtrl itemDetailCtrl;

	// Token: 0x040008CC RID: 2252
	private AchievementDetailCtrl achievementDetailCtrl;

	// Token: 0x040008CD RID: 2253
	private CmnReleaseConditionWindowCtrl cmnReleaseConditionWindowCtrl;

	// Token: 0x040008CE RID: 2254
	private LoadAndTipsCtrl loadAndTipsCtrl;

	// Token: 0x040008CF RID: 2255
	private PhotoWindowCtrl photoWindowCtrl;

	// Token: 0x040008D0 RID: 2256
	private SelQuestCountRecoveryWindowCtrl selQuestCountRecoveryWindowCtrl;

	// Token: 0x040008D1 RID: 2257
	private RewardListWindowCtrl rewardListWindowCtrl;

	// Token: 0x040008D2 RID: 2258
	private QuestSkipWindowsCtrl questSkipWindowsCtrl;

	// Token: 0x040008D3 RID: 2259
	private GameObject charaDeckWindow;

	// Token: 0x040008D4 RID: 2260
	private RecommendedDeckWindowCtrl recommendedDeckWindowCtrl;

	// Token: 0x040008D5 RID: 2261
	private PguiOpenWindowCtrl partyNameCtrl;

	// Token: 0x040008D6 RID: 2262
	private PguiOpenWindowCtrl userSkillCtrl;

	// Token: 0x040008D7 RID: 2263
	private SelTacticsSkillChangeWindowCtrl tacticsSkillChangeCtrl;

	// Token: 0x040008D8 RID: 2264
	private SelTacticsSkillInfoWindowCtrl tacticsSkillInfoCtrl;

	// Token: 0x040008D9 RID: 2265
	private PguiOpenWindowCtrl orderCardWindowCtrl;

	// Token: 0x040008DA RID: 2266
	private PguiOpenWindowCtrl partyCautionCtrl;

	// Token: 0x040008DB RID: 2267
	private PguiOpenWindowCtrl trainingEnemyInfoWindowCtrl;

	// Token: 0x040008DC RID: 2268
	private PguiOpenWindowCtrl bonusPhotoInfoWindowCtrl;

	// Token: 0x040008DD RID: 2269
	private CmnQuestSealedInfoWindowCtrl questSealedInfoWindowCtrl;

	// Token: 0x040008DE RID: 2270
	private TreeHouseFurnitureWindowCtrl treeHouseFurnitureWindowCtrl;

	// Token: 0x040008DF RID: 2271
	private AccessoryCheckWindowCtrl accessoryReleaseWindowCtrl;

	// Token: 0x040008E0 RID: 2272
	private AccessoryCheckWindowCtrl accessoryOwnerSettingWindowCtrl;

	// Token: 0x040008E1 RID: 2273
	private AccessoryCheckWindowCtrl accessoryCheckWindowCtrl;

	// Token: 0x040008E2 RID: 2274
	private DetachableAccessoryWindowCtrl detachableAccessoryWindowCtrl;

	// Token: 0x040008E3 RID: 2275
	private AccessoryWindowCtrl accessoryWindowCtrl;

	// Token: 0x040008E4 RID: 2276
	private AdvertiseBannerCtrl adevertiseBannerCtrl;

	// Token: 0x040008E5 RID: 2277
	private PurchaseConfirmWindow purchaseConfirmWindow;

	// Token: 0x040008E6 RID: 2278
	private PguiOpenWindowCtrl monthlyConfirmWindow;

	// Token: 0x040008E7 RID: 2279
	private PguiOpenWindowCtrl kizunaReachedLimitWindow;

	// Token: 0x040008E8 RID: 2280
	private CmnKizunaBuffWindowCtrl kizunaBuffWindowCtrl;

	// Token: 0x040008E9 RID: 2281
	private SelCharaGrowItemExchangeWindowCtrl selCharaGrowItemExchangeWindowCtrl;

	// Token: 0x040008EA RID: 2282
	private IEnumerator changeBgObjFunc;

	// Token: 0x040008EB RID: 2283
	public GameObject cmnTouchMask;

	// Token: 0x040008EC RID: 2284
	public GameObject outFrame;

	// Token: 0x040008ED RID: 2285
	private int outFrameW;

	// Token: 0x040008EE RID: 2286
	private int outFrameH;

	// Token: 0x040008EF RID: 2287
	private TutorialMaskCtrl tutorialMaskCtrl;

	// Token: 0x040008F0 RID: 2288
	private PhotoFilterWindowCtrl photoFilterWindowCtrl;

	// Token: 0x040008F1 RID: 2289
	private FriendsFilterWindowCtrl friendsFilterWindowCtrl;

	// Token: 0x040008F2 RID: 2290
	private CanvasManager.ReferenceResource referenseResource;

	// Token: 0x040008F3 RID: 2291
	private const int TOUCH_EFFECT_MAX = 5;

	// Token: 0x040008F4 RID: 2292
	private List<CanvasManager.TouchEffect> touchEffectList = new List<CanvasManager.TouchEffect>();

	// Token: 0x040008F5 RID: 2293
	private GameObject touchEffectObj;

	// Token: 0x040008F6 RID: 2294
	private CanvasManager.FadeStatus fadeStatus = CanvasManager.FadeStatus.FADE_IN_FINISH;

	// Token: 0x040008F7 RID: 2295
	private CanvasManager.FadeType fadeType;

	// Token: 0x040008F8 RID: 2296
	private bool requestFade;

	// Token: 0x040008F9 RID: 2297
	private SimpleAnimation currentFadeAnimation;

	// Token: 0x040008FA RID: 2298
	private PguiAECtrl currentFadeAe;

	// Token: 0x040008FB RID: 2299
	private AEImage currentFadeAei;

	// Token: 0x040008FC RID: 2300
	private GameObject currentFade;

	// Token: 0x040008FD RID: 2301
	private CriAtomExPlayback loseSe;

	// Token: 0x040008FE RID: 2302
	private static IntPtr oldWndProc = IntPtr.Zero;

	// Token: 0x040008FF RID: 2303
	private static Process prcId = null;

	// Token: 0x04000900 RID: 2304
	private static IntPtr winHdl = IntPtr.Zero;

	// Token: 0x04000901 RID: 2305
	public static int winClose = 0;

	// Token: 0x04000902 RID: 2306
	private static IEnumerator appQuit = null;

	// Token: 0x04000903 RID: 2307
	private bool returnBackground = true;

	// Token: 0x04000904 RID: 2308
	private bool isFromCampaignUrl;

	// Token: 0x04000905 RID: 2309
	private string lastCampaignUrlParam = "";

	// Token: 0x04000906 RID: 2310
	private static string campaignURLParam = null;

	// Token: 0x04000907 RID: 2311
	private const int BasePixelDragThreshold = 11;

	// Token: 0x04000908 RID: 2312
	private const int BaseScreenNum = 921600;

	// Token: 0x04000909 RID: 2313
	private int oldScreenWidth;

	// Token: 0x0400090A RID: 2314
	private int oldScreenHeight;

	// Token: 0x0400090B RID: 2315
	private ScreenOrientation beforeOrientation = ScreenOrientation.LandscapeLeft;

	// Token: 0x0400090C RID: 2316
	private int screenDirection;

	// Token: 0x0400090E RID: 2318
	private bool fpsSet = true;

	// Token: 0x0400090F RID: 2319
	private static List<DateTime> dtl = new List<DateTime>();

	// Token: 0x020007FF RID: 2047
	public class ReferenceResource
	{
		// Token: 0x060037BF RID: 14271 RVA: 0x001C9564 File Offset: 0x001C7764
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
		}

		// Token: 0x040035D9 RID: 13785
		public GameObject Icon_Chara;

		// Token: 0x040035DA RID: 13786
		public GameObject Icon_Photo;

		// Token: 0x040035DB RID: 13787
		public GameObject Icon_Photo_Mini;

		// Token: 0x040035DC RID: 13788
		public GameObject Card_Photo;

		// Token: 0x040035DD RID: 13789
		public GameObject Icon_Item;

		// Token: 0x040035DE RID: 13790
		public GameObject Icon_PhotoSet;

		// Token: 0x040035DF RID: 13791
		public GameObject Icon_CharaSet;

		// Token: 0x040035E0 RID: 13792
		public GameObject Icon_Accessory;

		// Token: 0x040035E1 RID: 13793
		public GameObject Icon_Accessory_Mini;

		// Token: 0x040035E2 RID: 13794
		public GameObject Icon_AccessorySet;
	}

	// Token: 0x02000800 RID: 2048
	private class TouchEffect
	{
		// Token: 0x040035E3 RID: 13795
		public Transform trans;

		// Token: 0x040035E4 RID: 13796
		public ParticleSystem particle;
	}

	// Token: 0x02000801 RID: 2049
	public enum FadeType
	{
		// Token: 0x040035E6 RID: 13798
		INVALID,
		// Token: 0x040035E7 RID: 13799
		NORMAL,
		// Token: 0x040035E8 RID: 13800
		SCENARIO,
		// Token: 0x040035E9 RID: 13801
		TIPS,
		// Token: 0x040035EA RID: 13802
		BATTLE_END,
		// Token: 0x040035EB RID: 13803
		BATTLE_LOSE,
		// Token: 0x040035EC RID: 13804
		SCENARIO2
	}

	// Token: 0x02000802 RID: 2050
	private enum FadeStatus
	{
		// Token: 0x040035EE RID: 13806
		FADE_OUT_PLAYING,
		// Token: 0x040035EF RID: 13807
		FADE_OUT_FINISH,
		// Token: 0x040035F0 RID: 13808
		FADE_IN_PLAYING,
		// Token: 0x040035F1 RID: 13809
		FADE_IN_FINISH
	}

	// Token: 0x02000803 RID: 2051
	// (Invoke) Token: 0x060037C2 RID: 14274
	private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

	// Token: 0x02000804 RID: 2052
	public struct WINRECT
	{
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x060037C5 RID: 14277 RVA: 0x001C9651 File Offset: 0x001C7851
		public int width
		{
			get
			{
				return this.right - this.left;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x060037C6 RID: 14278 RVA: 0x001C9660 File Offset: 0x001C7860
		public int height
		{
			get
			{
				return this.bottom - this.top;
			}
		}

		// Token: 0x040035F2 RID: 13810
		public int left;

		// Token: 0x040035F3 RID: 13811
		public int top;

		// Token: 0x040035F4 RID: 13812
		public int right;

		// Token: 0x040035F5 RID: 13813
		public int bottom;
	}

	// Token: 0x02000805 RID: 2053
	// (Invoke) Token: 0x060037C8 RID: 14280
	private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
}
