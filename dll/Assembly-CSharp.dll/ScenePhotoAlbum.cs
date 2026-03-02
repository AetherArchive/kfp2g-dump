using System;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class ScenePhotoAlbum : BaseScene
{
	// Token: 0x060014DE RID: 5342 RVA: 0x000FDBBB File Offset: 0x000FBDBB
	public override void OnCreateScene()
	{
		this.basePanel = SceneManager.CreateEmptyPanelByBaseCanvas(SceneManager.CanvasType.FRONT, "ScenePhotoAlbum", true);
		this.selPhotoAlbumCtrl = this.basePanel.AddComponent<SelPhotoAlbumCtrl>();
		this.selPhotoAlbumCtrl.Init();
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x000FDBEB File Offset: 0x000FBDEB
	public override bool OnCreateSceneWait()
	{
		return true;
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x000FDBF0 File Offset: 0x000FBDF0
	public override void OnEnableScene(object args)
	{
		this.basePanel.gameObject.SetActive(true);
		CanvasManager.SetBgTexture("selbg_home_in");
		this.requestNextScene = SceneManager.SceneName.None;
		this.sceneArgs = null;
		SoundManager.PlayBGM("prd_bgm0013");
		this.selPhotoAlbumCtrl.Setup();
		this.sceneOpenParam = args as ScenePhotoAlbum.OpenParam;
		CanvasManager.HdlCmnMenu.SetupMenu(true, "フォトアルバム", delegate
		{
			if (this.sceneOpenParam != null && this.sceneOpenParam.resultNextSceneName != SceneManager.SceneName.None)
			{
				this.requestNextScene = this.sceneOpenParam.resultNextSceneName;
				this.sceneArgs = this.sceneOpenParam.resultNextSceneArgs;
			}
			this.requestNextScene = ((this.sceneOpenParam != null) ? this.sceneOpenParam.resultNextSceneName : SceneManager.SceneName.SceneHome);
		}, "", null, null);
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x000FDC6A File Offset: 0x000FBE6A
	public override bool OnEnableSceneWait()
	{
		if (DataManager.IsServerRequesting())
		{
			return false;
		}
		this.selPhotoAlbumCtrl.SetupAlbumData();
		return true;
	}

	// Token: 0x060014E2 RID: 5346 RVA: 0x000FDC81 File Offset: 0x000FBE81
	public override void OnStartControl()
	{
	}

	// Token: 0x060014E3 RID: 5347 RVA: 0x000FDC83 File Offset: 0x000FBE83
	public override void Update()
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.sceneArgs);
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(true, true);
	}

	// Token: 0x060014E4 RID: 5348 RVA: 0x000FDCAF File Offset: 0x000FBEAF
	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x060014E5 RID: 5349 RVA: 0x000FDCC2 File Offset: 0x000FBEC2
	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x040010FA RID: 4346
	private GameObject basePanel;

	// Token: 0x040010FB RID: 4347
	private SelPhotoAlbumCtrl selPhotoAlbumCtrl;

	// Token: 0x040010FC RID: 4348
	private ScenePhotoAlbum.OpenParam sceneOpenParam = new ScenePhotoAlbum.OpenParam();

	// Token: 0x040010FD RID: 4349
	private SceneManager.SceneName requestNextScene;

	// Token: 0x040010FE RID: 4350
	private object sceneArgs;

	// Token: 0x02000BAC RID: 2988
	public class OpenParam
	{
		// Token: 0x0400486A RID: 18538
		public SceneManager.SceneName resultNextSceneName;

		// Token: 0x0400486B RID: 18539
		public object resultNextSceneArgs;
	}
}
