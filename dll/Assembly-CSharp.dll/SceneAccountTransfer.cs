using System;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000157 RID: 343
public class SceneAccountTransfer : BaseScene
{
	// Token: 0x060013BC RID: 5052 RVA: 0x000F2737 File Offset: 0x000F0937
	public override void OnCreateScene()
	{
		this.basePanel = SceneManager.CreateEmptyPanelByBaseCanvas(SceneManager.CanvasType.FRONT, "SceneAccountTransfer", true);
		this.selTransferCtrl = this.basePanel.AddComponent<SelTransferCtrl>();
		this.selTransferCtrl.Init();
	}

	// Token: 0x060013BD RID: 5053 RVA: 0x000F2768 File Offset: 0x000F0968
	public override void OnEnableScene(object args)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, "データ連携・引き継ぎ", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", null, null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		this.basePanel.gameObject.SetActive(true);
		this.selTransferCtrl.Setup(true);
		this.requestNextScene = SceneManager.SceneName.None;
	}

	// Token: 0x060013BE RID: 5054 RVA: 0x000F27D0 File Offset: 0x000F09D0
	public override void OnStartControl()
	{
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x000F27D4 File Offset: 0x000F09D4
	public override void Update()
	{
		bool flag = true;
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, null);
			flag = false;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(flag, true);
	}

	// Token: 0x060013C0 RID: 5056 RVA: 0x000F280A File Offset: 0x000F0A0A
	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x060013C1 RID: 5057 RVA: 0x000F281D File Offset: 0x000F0A1D
	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x000F2831 File Offset: 0x000F0A31
	private void OnClickReturnButton()
	{
		this.selTransferCtrl.OnClickReturnButton();
	}

	// Token: 0x04001054 RID: 4180
	private GameObject basePanel;

	// Token: 0x04001055 RID: 4181
	private SceneManager.SceneName requestNextScene;

	// Token: 0x04001056 RID: 4182
	private SelTransferCtrl selTransferCtrl;
}
