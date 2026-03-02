using System;
using UnityEngine;

// Token: 0x02000159 RID: 345
public class SceneItemView : BaseScene
{
	// Token: 0x060013CF RID: 5071 RVA: 0x000F29E4 File Offset: 0x000F0BE4
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selItemViewCtrl = this.basePanel.AddComponent<SelItemViewCtrl>();
		this.basePanel.name = "SceneItemView";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selItemViewCtrl.Init();
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x000F2A48 File Offset: 0x000F0C48
	public override void OnEnableScene(object args)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("所持アイテム一覧"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		this.basePanel.gameObject.SetActive(true);
		this.selItemViewCtrl.Setup();
		this.selItemViewCtrl.InitializeItemInfo();
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x000F2AC3 File Offset: 0x000F0CC3
	public override void OnStartControl()
	{
	}

	// Token: 0x060013D2 RID: 5074 RVA: 0x000F2AC5 File Offset: 0x000F0CC5
	public override void Update()
	{
		CanvasManager.HdlCmnMenu.UpdateMenu(!this.selItemViewCtrl.BackAnimPlaying, true);
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x000F2AE0 File Offset: 0x000F0CE0
	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x000F2AF3 File Offset: 0x000F0CF3
	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x060013D5 RID: 5077 RVA: 0x000F2B07 File Offset: 0x000F0D07
	private void OnClickReturnButton()
	{
		this.selItemViewCtrl.OnClickReturnButton();
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x000F2B15 File Offset: 0x000F0D15
	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.selItemViewCtrl.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	// Token: 0x0400105D RID: 4189
	private GameObject basePanel;

	// Token: 0x0400105E RID: 4190
	private SelItemViewCtrl selItemViewCtrl;
}
