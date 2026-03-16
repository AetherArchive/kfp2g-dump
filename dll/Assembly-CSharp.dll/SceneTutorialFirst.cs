using System;
using UnityEngine;

public class SceneTutorialFirst : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selTutorialFirstCtrl = this.basePanel.AddComponent<SelTutorialFirstCtrl>();
		this.basePanel.name = "SceneTutorialFirst";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
	}

	public override void OnEnableScene(object inArgs)
	{
		CanvasManager.SetBgTexture(null);
		CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
		this.receiveArgs = inArgs as SceneTutorialFirst.Args;
		AssetManager.LoadAssetData(SelTutorialFirstCtrl.introAsset, AssetManager.OWNER.NameEntry, 0, null);
	}

	public override bool OnEnableSceneWait()
	{
		if (!this.selTutorialFirstCtrl.Init())
		{
			return false;
		}
		SoundManager.PlayBGM("prd_bgm0007");
		this.basePanel.gameObject.SetActive(true);
		return true;
	}

	public override void Update()
	{
		if (this.selTutorialFirstCtrl.TutorialEnd())
		{
			TutorialUtil.RequestNextSequence(this.receiveArgs.tutorialSequence);
		}
	}

	public override void OnDisableScene()
	{
		this.selTutorialFirstCtrl.Term();
		this.basePanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		this.selTutorialFirstCtrl = null;
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private GameObject basePanel;

	private SelTutorialFirstCtrl selTutorialFirstCtrl;

	private SceneTutorialFirst.Args receiveArgs;

	public class Args
	{
		public TutorialUtil.Sequence tutorialSequence;
	}
}
