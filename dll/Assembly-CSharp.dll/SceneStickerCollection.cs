using System;
using SGNFW.Common;
using UnityEngine;

public class SceneStickerCollection : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = SceneManager.CreateEmptyPanelByBaseCanvas(SceneManager.CanvasType.FRONT, "SceneStickerCollection", true);
		this.selCollectionCtrl = this.basePanel.AddComponent<SelStickerCollectionCtrl>();
		this.selCollectionCtrl.Init();
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
		this.selCollectionCtrl.Setup();
		this.sceneOpenParam = args as SceneStickerCollection.OpenParam;
		CanvasManager.HdlCmnMenu.SetupMenu(true, "シールコレクション", delegate
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
		this.selCollectionCtrl.SetupCollectionData();
		return true;
	}

	public override void Update()
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.sceneArgs);
		}
		this.selCollectionCtrl.Update();
		CanvasManager.HdlCmnMenu.UpdateMenu(true, true);
	}

	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		this.selCollectionCtrl.OnDestroyScene();
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private GameObject basePanel;

	private SelStickerCollectionCtrl selCollectionCtrl;

	private SceneStickerCollection.OpenParam sceneOpenParam = new SceneStickerCollection.OpenParam();

	private SceneManager.SceneName requestNextScene;

	private object sceneArgs;

	public class OpenParam
	{
		public SceneManager.SceneName resultNextSceneName;

		public object resultNextSceneArgs;
	}
}
