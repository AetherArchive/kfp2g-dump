using System;
using SGNFW.Common;
using UnityEngine;

public class SceneFriend : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selFollowCtrll = this.basePanel.AddComponent<SelFollowCtrl>();
		this.basePanel.name = "SceneFriend";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selFollowCtrll.Init();
	}

	public override void OnEnableScene(object args)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, "フォローフォロワー", null, "", null, null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		DataManager.DmHelper.RequestGetFollowsList();
		this.requestNextScene = SceneManager.SceneName.None;
	}

	public override bool OnEnableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	public override void OnStartSceneFade()
	{
		this.basePanel.gameObject.SetActive(true);
		this.selFollowCtrll.Setup();
	}

	public override void OnStartControl()
	{
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
		this.basePanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private GameObject basePanel;

	private SelFollowCtrl selFollowCtrll;

	private SceneManager.SceneName requestNextScene;
}
