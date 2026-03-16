using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;

public class SceneOption : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selOptionCtrl = this.basePanel.AddComponent<SelOptionCtrl>();
		this.basePanel.name = "SceneOption";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selOptionCtrl.Init();
	}

	public override void OnEnableScene(object args)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("オプション"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		this.basePanel.gameObject.SetActive(true);
		this.selOptionCtrl.Setup();
		this.LoadCueSheet = SoundManager.LoadCueSheetWithDownload(DataManager.DmChara.GetCharaStaticData(DataManager.DmUserInfo.favoriteCharaId).cueSheetName);
		this.requestNextScene = SceneManager.SceneName.None;
	}

	public override bool OnEnableSceneWait()
	{
		return !this.LoadCueSheet.MoveNext();
	}

	public override void OnStartControl()
	{
	}

	public override void Update()
	{
		bool flag = true;
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, null);
			flag = false;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(flag, true);
	}

	public override void OnDisableScene()
	{
		SoundManager.UnloadCueSheet(DataManager.DmChara.GetCharaStaticData(DataManager.DmUserInfo.favoriteCharaId).cueSheetName);
		this.basePanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private void OnClickReturnButton()
	{
		if (!this.selOptionCtrl.OnClickReturnButton())
		{
			this.requestNextScene = SceneManager.SceneName.SceneOtherMenuTop;
		}
	}

	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.selOptionCtrl.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	private GameObject basePanel;

	private SceneManager.SceneName requestNextScene;

	private SelOptionCtrl selOptionCtrl;

	private IEnumerator LoadCueSheet;
}
