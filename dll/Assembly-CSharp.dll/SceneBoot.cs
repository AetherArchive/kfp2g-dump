using System;
using SGNFW.Common;

public class SceneBoot : BaseScene
{
	public override bool OnCreateSceneWait()
	{
		this.SetupSystemGUI();
		return true;
	}

	public void SetupSystemGUI()
	{
		CanvasManager.Initialize();
	}

	public override void Update()
	{
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneAdvertise, null);
	}
}
