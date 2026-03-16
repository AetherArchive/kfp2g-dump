using System;
using SGNFW.Common;
using UnityEngine;

public class SceneRanking : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = SceneManager.CreateEmptyPanelByBaseCanvas(SceneManager.CanvasType.FRONT, "SceneRanking", true);
		this.selRankingCtrl = this.basePanel.AddComponent<SelRankingCtrl>();
		this.selRankingCtrl.Initialize();
	}

	public override void OnEnableScene(object args)
	{
		this.basePanel.gameObject.SetActive(true);
		CanvasManager.SetBgTexture("selbg_home_in");
		this.requestNextScene = SceneManager.SceneName.None;
		this.sceneArgs = null;
		SoundManager.PlayBGM("prd_bgm0013");
		this.sceneOpenParam = args as SceneRanking.OpenParam;
		this.selRankingCtrl.Setup();
		CanvasManager.HdlCmnMenu.SetupMenu(true, "ランキング", delegate
		{
			if (this.sceneOpenParam != null && this.sceneOpenParam.resultNextSceneName != SceneManager.SceneName.None)
			{
				this.requestNextScene = this.sceneOpenParam.resultNextSceneName;
				this.sceneArgs = this.sceneOpenParam.resultNextSceneArgs;
			}
			this.requestNextScene = ((this.sceneOpenParam != null) ? this.sceneOpenParam.resultNextSceneName : SceneManager.SceneName.SceneHome);
		}, "", null, null);
	}

	public override bool OnEnableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	public override void Update()
	{
		this.selRankingCtrl.UpdateSel();
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.sceneArgs);
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(true, true);
	}

	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		this.selRankingCtrl.Destroy();
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private GameObject basePanel;

	private SelRankingCtrl selRankingCtrl;

	private SceneRanking.OpenParam sceneOpenParam = new SceneRanking.OpenParam();

	private SceneManager.SceneName requestNextScene;

	private object sceneArgs;

	public class OpenParam
	{
		public SceneManager.SceneName resultNextSceneName;

		public object resultNextSceneArgs;
	}
}
