using System;
using SGNFW.Common;
using UnityEngine;

public class ScenePhotoAlbum : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = SceneManager.CreateEmptyPanelByBaseCanvas(SceneManager.CanvasType.FRONT, "ScenePhotoAlbum", true);
		this.selPhotoAlbumCtrl = this.basePanel.AddComponent<SelPhotoAlbumCtrl>();
		this.selPhotoAlbumCtrl.Init();
	}

	public override bool OnCreateSceneWait()
	{
		return true;
	}

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

	public override bool OnEnableSceneWait()
	{
		if (DataManager.IsServerRequesting())
		{
			return false;
		}
		this.selPhotoAlbumCtrl.SetupAlbumData();
		return true;
	}

	public override void OnStartControl()
	{
	}

	public override void Update()
	{
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
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private GameObject basePanel;

	private SelPhotoAlbumCtrl selPhotoAlbumCtrl;

	private ScenePhotoAlbum.OpenParam sceneOpenParam = new ScenePhotoAlbum.OpenParam();

	private SceneManager.SceneName requestNextScene;

	private object sceneArgs;

	public class OpenParam
	{
		public SceneManager.SceneName resultNextSceneName;

		public object resultNextSceneArgs;
	}
}
