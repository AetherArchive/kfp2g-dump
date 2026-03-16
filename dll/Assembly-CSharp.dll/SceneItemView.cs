using System;
using UnityEngine;

public class SceneItemView : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selItemViewCtrl = this.basePanel.AddComponent<SelItemViewCtrl>();
		this.basePanel.name = "SceneItemView";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selItemViewCtrl.Init();
	}

	public override void OnEnableScene(object args)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("所持アイテム一覧"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		this.basePanel.gameObject.SetActive(true);
		this.selItemViewCtrl.Setup();
		this.selItemViewCtrl.InitializeItemInfo();
	}

	public override void OnStartControl()
	{
	}

	public override void Update()
	{
		CanvasManager.HdlCmnMenu.UpdateMenu(!this.selItemViewCtrl.BackAnimPlaying, true);
	}

	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private void OnClickReturnButton()
	{
		this.selItemViewCtrl.OnClickReturnButton();
	}

	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.selItemViewCtrl.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	private GameObject basePanel;

	private SelItemViewCtrl selItemViewCtrl;
}
