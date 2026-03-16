using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

public class SceneGacha : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = SceneManager.CreateEmptyPanelByBaseCanvas(SceneManager.CanvasType.FRONT, "SceneGacha", true);
		this.selGachaCtrl = this.basePanel.AddComponent<SelGachaCtrl>();
		this.selGachaCtrl.Init();
		this.selGachaCtrl.requestNextSceneCb = new Action<SceneManager.SceneName, object>(this.RequestSceneChange);
		this.selGachaCtrl.isRequestingNextSceneCb = new Func<bool>(this.IsRequestingSceneChange);
	}

	private void RequestSceneChange(SceneManager.SceneName request, object args)
	{
		this.requestNextScene = request;
		this.requestNextSceneArgs = args;
	}

	private bool IsRequestingSceneChange()
	{
		return this.requestNextScene > SceneManager.SceneName.None;
	}

	public override bool OnCreateSceneWait()
	{
		return true;
	}

	public override void OnEnableScene(object args)
	{
		this.basePanel.gameObject.SetActive(true);
		DataManager.DmGacha.RequestGetGachaList();
		CanvasManager.SetBgTexture("selbg_Gacha");
		DataManager.DmGacha.SelectedGachaIdHashSet = new HashSet<int>();
		this.requestNextScene = SceneManager.SceneName.None;
		this.requestNextSceneArgs = null;
		SoundManager.PlayBGM("prd_bgm0010");
		SceneGacha.OpenParam openParam = args as SceneGacha.OpenParam;
		this.selGachaCtrl.Setup(openParam);
	}

	public override bool OnEnableSceneWait()
	{
		if (DataManager.IsServerRequesting())
		{
			return false;
		}
		if (!this.selGachaCtrl.RenderCharaLBFinishedSetup)
		{
			return false;
		}
		this.selGachaCtrl.SetupGachaData();
		return true;
	}

	public override void OnStartControl()
	{
	}

	public override void Update()
	{
		this.selGachaCtrl.UpdateSel();
		if (this.IsRequestingSceneChange())
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.requestNextSceneArgs);
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(true, true);
	}

	public override void OnDisableScene()
	{
		this.selGachaCtrl.Disable();
		this.basePanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		this.selGachaCtrl.Destroy();
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private GameObject basePanel;

	private SelGachaCtrl selGachaCtrl;

	private SceneManager.SceneName requestNextScene;

	private object requestNextSceneArgs;

	public class OpenParam
	{
		public TutorialUtil.Sequence tutorialSequence;

		public SceneManager.SceneName resultNextSceneName;

		public object resultNextSceneArgs;

		public int gachaId;
	}
}
