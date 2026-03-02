using System;
using SGNFW.Common;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class SceneFriend : BaseScene
{
	// Token: 0x06001137 RID: 4407 RVA: 0x000D2A4C File Offset: 0x000D0C4C
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selFollowCtrll = this.basePanel.AddComponent<SelFollowCtrl>();
		this.basePanel.name = "SceneFriend";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selFollowCtrll.Init();
	}

	// Token: 0x06001138 RID: 4408 RVA: 0x000D2AAE File Offset: 0x000D0CAE
	public override void OnEnableScene(object args)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, "フォローフォロワー", null, "", null, null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		DataManager.DmHelper.RequestGetFollowsList();
		this.requestNextScene = SceneManager.SceneName.None;
	}

	// Token: 0x06001139 RID: 4409 RVA: 0x000D2AED File Offset: 0x000D0CED
	public override bool OnEnableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	// Token: 0x0600113A RID: 4410 RVA: 0x000D2AF7 File Offset: 0x000D0CF7
	public override void OnStartSceneFade()
	{
		this.basePanel.gameObject.SetActive(true);
		this.selFollowCtrll.Setup();
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x000D2B15 File Offset: 0x000D0D15
	public override void OnStartControl()
	{
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x000D2B17 File Offset: 0x000D0D17
	private void OnClickButton(PguiButtonCtrl buttuon)
	{
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x000D2B1C File Offset: 0x000D0D1C
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

	// Token: 0x0600113E RID: 4414 RVA: 0x000D2B52 File Offset: 0x000D0D52
	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x0600113F RID: 4415 RVA: 0x000D2B65 File Offset: 0x000D0D65
	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x04000E9A RID: 3738
	private GameObject basePanel;

	// Token: 0x04000E9B RID: 3739
	private SelFollowCtrl selFollowCtrll;

	// Token: 0x04000E9C RID: 3740
	private SceneManager.SceneName requestNextScene;
}
