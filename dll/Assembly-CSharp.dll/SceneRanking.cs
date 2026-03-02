using System;
using SGNFW.Common;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class SceneRanking : BaseScene
{
	// Token: 0x060013FE RID: 5118 RVA: 0x000F38E4 File Offset: 0x000F1AE4
	public override void OnCreateScene()
	{
		this.basePanel = SceneManager.CreateEmptyPanelByBaseCanvas(SceneManager.CanvasType.FRONT, "SceneRanking", true);
		this.selRankingCtrl = this.basePanel.AddComponent<SelRankingCtrl>();
		this.selRankingCtrl.Initialize();
	}

	// Token: 0x060013FF RID: 5119 RVA: 0x000F3914 File Offset: 0x000F1B14
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

	// Token: 0x06001400 RID: 5120 RVA: 0x000F398E File Offset: 0x000F1B8E
	public override bool OnEnableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x000F399A File Offset: 0x000F1B9A
	public override void Update()
	{
		this.selRankingCtrl.UpdateSel();
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.sceneArgs);
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(true, true);
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x000F39D1 File Offset: 0x000F1BD1
	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x000F39E4 File Offset: 0x000F1BE4
	public override void OnDestroyScene()
	{
		this.selRankingCtrl.Destroy();
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x04001073 RID: 4211
	private GameObject basePanel;

	// Token: 0x04001074 RID: 4212
	private SelRankingCtrl selRankingCtrl;

	// Token: 0x04001075 RID: 4213
	private SceneRanking.OpenParam sceneOpenParam = new SceneRanking.OpenParam();

	// Token: 0x04001076 RID: 4214
	private SceneManager.SceneName requestNextScene;

	// Token: 0x04001077 RID: 4215
	private object sceneArgs;

	// Token: 0x02000B5A RID: 2906
	public class OpenParam
	{
		// Token: 0x0400471A RID: 18202
		public SceneManager.SceneName resultNextSceneName;

		// Token: 0x0400471B RID: 18203
		public object resultNextSceneArgs;
	}
}
