using System;
using SGNFW.Common;

// Token: 0x02000128 RID: 296
public class SceneBoot : BaseScene
{
	// Token: 0x06000F10 RID: 3856 RVA: 0x000B5B83 File Offset: 0x000B3D83
	public override bool OnCreateSceneWait()
	{
		this.SetupSystemGUI();
		return true;
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x000B5B8C File Offset: 0x000B3D8C
	public void SetupSystemGUI()
	{
		CanvasManager.Initialize();
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x000B5B93 File Offset: 0x000B3D93
	public override void Update()
	{
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneAdvertise, null);
	}
}
