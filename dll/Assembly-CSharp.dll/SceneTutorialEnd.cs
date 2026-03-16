using System;
using System.Collections;
using SGNFW.Common;

public class SceneTutorialEnd : BaseScene
{
	public override void OnCreateScene()
	{
	}

	public override void OnEnableScene(object args)
	{
	}

	public override bool OnEnableSceneWait()
	{
		if (!DataManager.IsServerRequesting())
		{
			CanvasManager.SetBgTexture("selbg_home_out");
			CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
			Singleton<DataManager>.Instance.Initialize();
			Singleton<DataManager>.Instance.DisableServerRequestByTutorial = false;
			this.initDataManager = DataInitializeResolver.InitializeActionDataManager();
			return true;
		}
		return false;
	}

	public override void OnStartControl()
	{
	}

	public override void Update()
	{
		if (!this.initDataManager.MoveNext())
		{
			TutorialUtil.RequestNextSequence(TutorialUtil.Sequence.DATA_RESET);
		}
	}

	public override void OnDisableScene()
	{
	}

	public override void OnDestroyScene()
	{
	}

	private IEnumerator initDataManager;
}
