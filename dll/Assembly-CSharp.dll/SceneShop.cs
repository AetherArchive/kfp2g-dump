using System;
using SGNFW.Common;
using UnityEngine;

public class SceneShop : BaseScene
{
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

	public override bool OnEnableSceneWait()
	{
		bool isEnableScene = this.selShopCtrl.IsEnableScene;
		bool isEnableScene2 = this.selAssistantCtrl.IsEnableScene;
		return isEnableScene && isEnableScene2;
	}

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

	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.currentMode == SceneShop.Mode.CHARA_EDIT && this.selAssistantCtrl.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

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

	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
		this.selShopCtrl.Teardown();
		if (this.selShopCtrl.DispNewGoodsId.Count > 0)
		{
			DataManager.DmShop.RequestActionUpdateNewFlag(this.selShopCtrl.DispNewGoodsId);
		}
	}

	public override bool OnDisableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	public override void OnDestroyScene()
	{
		this.selShopCtrl.Destroy();
		this.selAssistantCtrl.Destroy();
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private SceneManager.SceneName requestNextScene;

	private object requestNextSceneArgs;

	public GameObject basePanel;

	private SelShopCtrl selShopCtrl;

	private SelAssistantCtrl selAssistantCtrl;

	private SceneShopArgs shopArgs;

	private bool isTapReturnButton;

	private SceneShop.Mode requestMode;

	private SceneShop.Mode currentMode;

	public enum Mode
	{
		INVALID,
		TOP,
		CHARA_EDIT
	}
}
