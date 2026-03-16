using System;
using SGNFW.Common;
using UnityEngine;

public class ScenePresent : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selPresentCtrl = this.basePanel.AddComponent<SelPresentCtrl>();
		this.basePanel.name = "ScenePresent";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selPresentCtrl.Init();
		this.selPresentCtrl.gameObject.SetActive(false);
	}

	public override void OnEnableScene(object args)
	{
		DataManager.DmPresent.RequestGetPresentList();
		DataManager.DmPresent.RequestGetHistoryist();
		CanvasManager.HdlCmnMenu.SetupMenu(true, "プレゼント", null, "", null, null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		this.requestNextScene = SceneManager.SceneName.None;
	}

	public override bool OnEnableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	public override void OnStartSceneFade()
	{
		this.basePanel.gameObject.SetActive(true);
		this.selPresentCtrl.Setup();
	}

	private void OnClickButton(PguiButtonCtrl buttuon)
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
		this.selPresentCtrl.Disable();
		this.basePanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		this.selPresentCtrl.Destroy();
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private GameObject basePanel;

	private SelPresentCtrl selPresentCtrl;

	private SceneManager.SceneName requestNextScene;
}
