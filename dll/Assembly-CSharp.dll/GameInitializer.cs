using System;
using DMMHelper;
using SegaUnityCEFBrowser;
using SGNFW.Common;
using SGNFW.Login;
using SGNFW.Mst;
using SGNFW.Thread;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class GameInitializer : MonoBehaviour
{
	// Token: 0x06000BAC RID: 2988 RVA: 0x00044FE0 File Offset: 0x000431E0
	private void Start()
	{
		Verbose<PrjLog>.Enabled = true;
		QualitySettings.SetQualityLevel(1);
		base.gameObject.AddComponent<CanvasManager>();
		base.gameObject.AddComponent<SceneManager>();
		Singleton<SceneManager>.Instance.Initialize();
		base.gameObject.AddComponent<TimeManager>();
		base.gameObject.AddComponent<DataManager>().Initialize();
		base.gameObject.AddComponent<EffectManager>();
		base.gameObject.AddComponent<LoginManager>();
		base.gameObject.AddComponent<ThreadPool>();
		base.gameObject.AddComponent<MstManager>();
		base.gameObject.AddComponent<AssetManager>();
		base.gameObject.AddComponent<SortFilterManager>();
		base.gameObject.AddComponent<DMMHelpManager>();
		Singleton<SoundManager>.Instance.Initialize();
		SceneManager.CanvasType canvasType = SceneManager.CanvasType.PRESET;
		GameObject gameObject = this.CanvasInstantiate("PresetCanvas", new Vector2(0f, -250f));
		Camera worldCamera = gameObject.GetComponent<Canvas>().worldCamera;
		worldCamera.clearFlags = CameraClearFlags.Color;
		worldCamera.backgroundColor = Color.white;
		worldCamera.depth = (float)SceneManager.CameraDepth[canvasType];
		Singleton<SceneManager>.Instance.SetBaseCanvas(canvasType, gameObject.transform, worldCamera);
		SceneManager.InitializeOption();
		BrowserControl.Create(1, true);
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00045100 File Offset: 0x00043300
	private GameObject CanvasInstantiate(string canvasName, Vector2 ancPos)
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("prefab/PguiCanvas"));
		gameObject.name = canvasName;
		(gameObject.transform as RectTransform).anchoredPosition = ancPos;
		return gameObject;
	}
}
