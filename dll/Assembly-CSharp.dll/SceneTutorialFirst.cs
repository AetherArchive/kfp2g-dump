using System;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class SceneTutorialFirst : BaseScene
{
	// Token: 0x06001A34 RID: 6708 RVA: 0x00154BEC File Offset: 0x00152DEC
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selTutorialFirstCtrl = this.basePanel.AddComponent<SelTutorialFirstCtrl>();
		this.basePanel.name = "SceneTutorialFirst";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x00154C43 File Offset: 0x00152E43
	public override void OnEnableScene(object inArgs)
	{
		CanvasManager.SetBgTexture(null);
		CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
		this.receiveArgs = inArgs as SceneTutorialFirst.Args;
		AssetManager.LoadAssetData(SelTutorialFirstCtrl.introAsset, AssetManager.OWNER.NameEntry, 0, null);
	}

	// Token: 0x06001A36 RID: 6710 RVA: 0x00154C7D File Offset: 0x00152E7D
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

	// Token: 0x06001A37 RID: 6711 RVA: 0x00154CAA File Offset: 0x00152EAA
	public override void Update()
	{
		if (this.selTutorialFirstCtrl.TutorialEnd())
		{
			TutorialUtil.RequestNextSequence(this.receiveArgs.tutorialSequence);
		}
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x00154CC9 File Offset: 0x00152EC9
	public override void OnDisableScene()
	{
		this.selTutorialFirstCtrl.Term();
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x00154CE7 File Offset: 0x00152EE7
	public override void OnDestroyScene()
	{
		this.selTutorialFirstCtrl = null;
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x04001414 RID: 5140
	private GameObject basePanel;

	// Token: 0x04001415 RID: 5141
	private SelTutorialFirstCtrl selTutorialFirstCtrl;

	// Token: 0x04001416 RID: 5142
	private SceneTutorialFirst.Args receiveArgs;

	// Token: 0x02000E5D RID: 3677
	public class Args
	{
		// Token: 0x040052A6 RID: 21158
		public TutorialUtil.Sequence tutorialSequence;
	}
}
