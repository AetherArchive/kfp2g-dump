using System;
using SGNFW.Common;
using UnityEngine;

public class SceneAccountTransfer : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = SceneManager.CreateEmptyPanelByBaseCanvas(SceneManager.CanvasType.FRONT, "SceneAccountTransfer", true);
		this.selTransferCtrl = this.basePanel.AddComponent<SelTransferCtrl>();
		this.selTransferCtrl.Init();
	}

	public override void OnEnableScene(object args)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, "データ連携・引き継ぎ", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		this.basePanel.gameObject.SetActive(true);
		this.selTransferCtrl.Setup(true);
		this.requestNextScene = SceneManager.SceneName.None;
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
		this.basePanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private void OnClickReturnButton()
	{
		this.selTransferCtrl.OnClickReturnButton();
	}

	private GameObject basePanel;

	private SceneManager.SceneName requestNextScene;

	private SelTransferCtrl selTransferCtrl;
}
