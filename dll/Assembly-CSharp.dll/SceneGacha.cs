using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class SceneGacha : BaseScene
{
	// Token: 0x060011E9 RID: 4585 RVA: 0x000D918C File Offset: 0x000D738C
	public override void OnCreateScene()
	{
		this.basePanel = SceneManager.CreateEmptyPanelByBaseCanvas(SceneManager.CanvasType.FRONT, "SceneGacha", true);
		this.selGachaCtrl = this.basePanel.AddComponent<SelGachaCtrl>();
		this.selGachaCtrl.Init();
		this.selGachaCtrl.requestNextSceneCb = new Action<SceneManager.SceneName, object>(this.RequestSceneChange);
		this.selGachaCtrl.isRequestingNextSceneCb = new Func<bool>(this.IsRequestingSceneChange);
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x000D91F5 File Offset: 0x000D73F5
	private void RequestSceneChange(SceneManager.SceneName request, object args)
	{
		this.requestNextScene = request;
		this.requestNextSceneArgs = args;
	}

	// Token: 0x060011EB RID: 4587 RVA: 0x000D9205 File Offset: 0x000D7405
	private bool IsRequestingSceneChange()
	{
		return this.requestNextScene > SceneManager.SceneName.None;
	}

	// Token: 0x060011EC RID: 4588 RVA: 0x000D9210 File Offset: 0x000D7410
	public override bool OnCreateSceneWait()
	{
		return true;
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x000D9214 File Offset: 0x000D7414
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

	// Token: 0x060011EE RID: 4590 RVA: 0x000D9280 File Offset: 0x000D7480
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

	// Token: 0x060011EF RID: 4591 RVA: 0x000D92A6 File Offset: 0x000D74A6
	public override void OnStartControl()
	{
	}

	// Token: 0x060011F0 RID: 4592 RVA: 0x000D92A8 File Offset: 0x000D74A8
	public override void Update()
	{
		this.selGachaCtrl.UpdateSel();
		if (this.IsRequestingSceneChange())
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.requestNextSceneArgs);
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(true, true);
	}

	// Token: 0x060011F1 RID: 4593 RVA: 0x000D92DF File Offset: 0x000D74DF
	public override void OnDisableScene()
	{
		this.selGachaCtrl.Disable();
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x000D92FD File Offset: 0x000D74FD
	public override void OnDestroyScene()
	{
		this.selGachaCtrl.Destroy();
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x04000EF5 RID: 3829
	private GameObject basePanel;

	// Token: 0x04000EF6 RID: 3830
	private SelGachaCtrl selGachaCtrl;

	// Token: 0x04000EF7 RID: 3831
	private SceneManager.SceneName requestNextScene;

	// Token: 0x04000EF8 RID: 3832
	private object requestNextSceneArgs;

	// Token: 0x02000AC4 RID: 2756
	public class OpenParam
	{
		// Token: 0x04004461 RID: 17505
		public TutorialUtil.Sequence tutorialSequence;

		// Token: 0x04004462 RID: 17506
		public SceneManager.SceneName resultNextSceneName;

		// Token: 0x04004463 RID: 17507
		public object resultNextSceneArgs;

		// Token: 0x04004464 RID: 17508
		public int gachaId;
	}
}
