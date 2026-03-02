using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;

// Token: 0x0200015A RID: 346
public class SceneOption : BaseScene
{
	// Token: 0x060013D8 RID: 5080 RVA: 0x000F2B2C File Offset: 0x000F0D2C
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selOptionCtrl = this.basePanel.AddComponent<SelOptionCtrl>();
		this.basePanel.name = "SceneOption";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selOptionCtrl.Init();
	}

	// Token: 0x060013D9 RID: 5081 RVA: 0x000F2B90 File Offset: 0x000F0D90
	public override void OnEnableScene(object args)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, PrjUtil.MakeMessage("オプション"), new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		this.basePanel.gameObject.SetActive(true);
		this.selOptionCtrl.Setup();
		this.LoadCueSheet = SoundManager.LoadCueSheetWithDownload(DataManager.DmChara.GetCharaStaticData(DataManager.DmUserInfo.favoriteCharaId).cueSheetName);
		this.requestNextScene = SceneManager.SceneName.None;
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x000F2C2B File Offset: 0x000F0E2B
	public override bool OnEnableSceneWait()
	{
		return !this.LoadCueSheet.MoveNext();
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x000F2C3D File Offset: 0x000F0E3D
	public override void OnStartControl()
	{
	}

	// Token: 0x060013DC RID: 5084 RVA: 0x000F2C40 File Offset: 0x000F0E40
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

	// Token: 0x060013DD RID: 5085 RVA: 0x000F2C76 File Offset: 0x000F0E76
	public override void OnDisableScene()
	{
		SoundManager.UnloadCueSheet(DataManager.DmChara.GetCharaStaticData(DataManager.DmUserInfo.favoriteCharaId).cueSheetName);
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x000F2CA7 File Offset: 0x000F0EA7
	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x000F2CBB File Offset: 0x000F0EBB
	private void OnClickReturnButton()
	{
		if (!this.selOptionCtrl.OnClickReturnButton())
		{
			this.requestNextScene = SceneManager.SceneName.SceneOtherMenuTop;
		}
	}

	// Token: 0x060013E0 RID: 5088 RVA: 0x000F2CD2 File Offset: 0x000F0ED2
	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.selOptionCtrl.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	// Token: 0x0400105F RID: 4191
	private GameObject basePanel;

	// Token: 0x04001060 RID: 4192
	private SceneManager.SceneName requestNextScene;

	// Token: 0x04001061 RID: 4193
	private SelOptionCtrl selOptionCtrl;

	// Token: 0x04001062 RID: 4194
	private IEnumerator LoadCueSheet;
}
