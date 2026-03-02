using System;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class ScenePresent : BaseScene
{
	// Token: 0x0600154E RID: 5454 RVA: 0x0010C374 File Offset: 0x0010A574
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selPresentCtrl = this.basePanel.AddComponent<SelPresentCtrl>();
		this.basePanel.name = "ScenePresent";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selPresentCtrl.Init();
		this.selPresentCtrl.gameObject.SetActive(false);
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x0010C3E8 File Offset: 0x0010A5E8
	public override void OnEnableScene(object args)
	{
		DataManager.DmPresent.RequestGetPresentList();
		DataManager.DmPresent.RequestGetHistoryist();
		CanvasManager.HdlCmnMenu.SetupMenu(true, "プレゼント", null, "", null, null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		this.requestNextScene = SceneManager.SceneName.None;
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x0010C43C File Offset: 0x0010A63C
	public override bool OnEnableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x0010C446 File Offset: 0x0010A646
	public override void OnStartSceneFade()
	{
		this.basePanel.gameObject.SetActive(true);
		this.selPresentCtrl.Setup();
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x0010C464 File Offset: 0x0010A664
	private void OnClickButton(PguiButtonCtrl buttuon)
	{
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x0010C468 File Offset: 0x0010A668
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

	// Token: 0x06001554 RID: 5460 RVA: 0x0010C49E File Offset: 0x0010A69E
	public override void OnDisableScene()
	{
		this.selPresentCtrl.Disable();
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x0010C4BC File Offset: 0x0010A6BC
	public override void OnDestroyScene()
	{
		this.selPresentCtrl.Destroy();
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x040011B5 RID: 4533
	private GameObject basePanel;

	// Token: 0x040011B6 RID: 4534
	private SelPresentCtrl selPresentCtrl;

	// Token: 0x040011B7 RID: 4535
	private SceneManager.SceneName requestNextScene;
}
