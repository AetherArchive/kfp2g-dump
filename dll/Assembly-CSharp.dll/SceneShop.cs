using System;
using SGNFW.Common;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class SceneShop : BaseScene
{
	// Token: 0x06001841 RID: 6209 RVA: 0x0012A304 File Offset: 0x00128504
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selShopCtrl = this.basePanel.AddComponent<SelShopCtrl>();
		this.selShopCtrl.Init();
		this.basePanel.name = "SelShop";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selAssistantCtrl = this.selShopCtrl.selAssistantCtrl;
		this.selShopCtrl.guiData.BtnAssistantEdit.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.selAssistantCtrl.guiData.Btn_EditOk.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x0012A3BC File Offset: 0x001285BC
	public override void OnEnableScene(object args)
	{
		this.basePanel.gameObject.SetActive(true);
		this.shopArgs = args as SceneShopArgs;
		this.selShopCtrl.Setup(this.shopArgs);
		this.selAssistantCtrl.Setup();
		CanvasManager.HdlCmnMenu.SetupMenu(true, "ショップ", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		SoundManager.PlayBGM("prd_bgm0001");
		this.requestMode = SceneShop.Mode.TOP;
		this.currentMode = SceneShop.Mode.INVALID;
		this.requestNextScene = SceneManager.SceneName.None;
		this.requestNextSceneArgs = null;
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x0012A458 File Offset: 0x00128658
	public override bool OnEnableSceneWait()
	{
		bool isEnableScene = this.selShopCtrl.IsEnableScene;
		bool isEnableScene2 = this.selAssistantCtrl.IsEnableScene;
		return isEnableScene && isEnableScene2;
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x0012A480 File Offset: 0x00128680
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.currentMode == SceneShop.Mode.TOP)
		{
			if (button == this.selShopCtrl.guiData.BtnAssistantEdit)
			{
				button.gameObject.SetActive(false);
				if (!QuestUtil.IsDispDhole())
				{
					this.requestMode = SceneShop.Mode.TOP;
				}
				else
				{
					this.selAssistantCtrl.currentMode = SelAssistantCtrl.Mode.TOP;
					this.requestMode = SceneShop.Mode.CHARA_EDIT;
					this.selAssistantCtrl.guiData.tapGuard.SetActive(true);
				}
				this.selAssistantCtrl.OnClickAssistantButton();
				return;
			}
		}
		else if (this.currentMode == SceneShop.Mode.CHARA_EDIT && button == this.selAssistantCtrl.guiData.Btn_EditOk)
		{
			this.selAssistantCtrl.currentMode = SelAssistantCtrl.Mode.ASSISTANT_EDIT;
			this.selAssistantCtrl.guiData.tapGuard.SetActive(false);
			this.selAssistantCtrl.OnClickEditOk(button);
			this.requestMode = SceneShop.Mode.TOP;
		}
	}

	// Token: 0x06001845 RID: 6213 RVA: 0x0012A558 File Offset: 0x00128758
	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.currentMode == SceneShop.Mode.CHARA_EDIT && this.selAssistantCtrl.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	// Token: 0x06001846 RID: 6214 RVA: 0x0012A574 File Offset: 0x00128774
	private void OnClickReturnButton()
	{
		if (this.currentMode == SceneShop.Mode.TOP)
		{
			if (!this.selShopCtrl.OnClickReturnButton())
			{
				if (this.shopArgs != null && this.shopArgs.resultNextSceneName != SceneManager.SceneName.None)
				{
					this.requestNextScene = this.shopArgs.resultNextSceneName;
					this.requestNextSceneArgs = this.shopArgs.resultNextSceneArgs;
					return;
				}
				this.requestNextScene = SceneManager.SceneName.SceneHome;
				return;
			}
		}
		else if (this.currentMode == SceneShop.Mode.CHARA_EDIT)
		{
			this.selAssistantCtrl.OnClickMenuReturn(delegate
			{
				this.selAssistantCtrl.guiData.tapGuard.SetActive(false);
				this.requestMode = SceneShop.Mode.TOP;
			}, delegate
			{
				this.selAssistantCtrl.guiData.tapGuard.SetActive(true);
				this.requestMode = SceneShop.Mode.CHARA_EDIT;
			});
		}
	}

	// Token: 0x06001847 RID: 6215 RVA: 0x0012A608 File Offset: 0x00128808
	public override void Update()
	{
		this.selShopCtrl.UpdateSel();
		if (this.requestMode == SceneShop.Mode.TOP)
		{
			CanvasManager.HdlOpenWindowSortFilter.RequestActionUpdateSortType();
			SortFilterManager.RequestUpdateSortTypeData();
		}
		bool flag = true;
		if (this.requestNextScene == SceneManager.SceneName.None)
		{
			this.requestNextScene = this.selShopCtrl.RequestNextScene;
			this.requestNextSceneArgs = this.selShopCtrl.RequestNextSceneArgs;
		}
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.requestNextSceneArgs);
			flag = false;
		}
		if (this.requestMode != this.currentMode)
		{
			this.currentMode = this.requestMode;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(flag, true);
	}

	// Token: 0x06001848 RID: 6216 RVA: 0x0012A6AC File Offset: 0x001288AC
	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
		this.selShopCtrl.Teardown();
		if (this.selShopCtrl.DispNewGoodsId.Count > 0)
		{
			DataManager.DmShop.RequestActionUpdateNewFlag(this.selShopCtrl.DispNewGoodsId);
		}
	}

	// Token: 0x06001849 RID: 6217 RVA: 0x0012A6FD File Offset: 0x001288FD
	public override bool OnDisableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x0012A707 File Offset: 0x00128907
	public override void OnDestroyScene()
	{
		this.selShopCtrl.Destroy();
		this.selAssistantCtrl.Destroy();
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x040012B1 RID: 4785
	private SceneManager.SceneName requestNextScene;

	// Token: 0x040012B2 RID: 4786
	private object requestNextSceneArgs;

	// Token: 0x040012B3 RID: 4787
	public GameObject basePanel;

	// Token: 0x040012B4 RID: 4788
	private SelShopCtrl selShopCtrl;

	// Token: 0x040012B5 RID: 4789
	private SelAssistantCtrl selAssistantCtrl;

	// Token: 0x040012B6 RID: 4790
	private SceneShopArgs shopArgs;

	// Token: 0x040012B7 RID: 4791
	private bool isTapReturnButton;

	// Token: 0x040012B8 RID: 4792
	private SceneShop.Mode requestMode;

	// Token: 0x040012B9 RID: 4793
	private SceneShop.Mode currentMode;

	// Token: 0x02000D3C RID: 3388
	public enum Mode
	{
		// Token: 0x04004DCC RID: 19916
		INVALID,
		// Token: 0x04004DCD RID: 19917
		TOP,
		// Token: 0x04004DCE RID: 19918
		CHARA_EDIT
	}
}
