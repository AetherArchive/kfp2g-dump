using System;
using System.Collections;
using SGNFW.Common;

// Token: 0x02000186 RID: 390
public class SceneTutorialEnd : BaseScene
{
	// Token: 0x06001A2C RID: 6700 RVA: 0x00154B65 File Offset: 0x00152D65
	public override void OnCreateScene()
	{
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x00154B67 File Offset: 0x00152D67
	public override void OnEnableScene(object args)
	{
	}

	// Token: 0x06001A2E RID: 6702 RVA: 0x00154B6C File Offset: 0x00152D6C
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

	// Token: 0x06001A2F RID: 6703 RVA: 0x00154BC5 File Offset: 0x00152DC5
	public override void OnStartControl()
	{
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x00154BC7 File Offset: 0x00152DC7
	public override void Update()
	{
		if (!this.initDataManager.MoveNext())
		{
			TutorialUtil.RequestNextSequence(TutorialUtil.Sequence.DATA_RESET);
		}
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x00154BDD File Offset: 0x00152DDD
	public override void OnDisableScene()
	{
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x00154BDF File Offset: 0x00152DDF
	public override void OnDestroyScene()
	{
	}

	// Token: 0x04001413 RID: 5139
	private IEnumerator initDataManager;
}
