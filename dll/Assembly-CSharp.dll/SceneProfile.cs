using System;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

public class SceneProfile : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		RectTransform rectTransform = this.basePanel.AddComponent<RectTransform>();
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		rectTransform.localScale = Vector3.one;
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.offsetMin = new Vector2(0f, 0f);
		rectTransform.offsetMax = new Vector2(0f, 0f);
		this.selProfileCtrl = this.basePanel.AddComponent<SelProfileCtrl>();
		this.selProfileCtrl.Init();
		this.basePanel.name = "SelProfile";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
	}

	public override void OnEnableScene(object args)
	{
		this.basePanel.gameObject.SetActive(true);
		this.myArgs = args as SceneProfile.Args;
		bool flag = this.myArgs != null && this.myArgs.isHelperSettingStartFromCharaEdit;
		bool flag2 = this.myArgs != null && this.myArgs.isFromBattleSelecter;
		bool flag3 = this.myArgs != null && this.myArgs.openPhotoDetailWindow;
		if (flag3)
		{
			CanvasManager.HdlPhotoWindowCtrl.OpenPrev();
		}
		bool flag4 = this.myArgs != null && this.myArgs.openAccessoryWindow;
		if (flag4)
		{
			CanvasManager.HdlAccessoryWindowCtrl.OpenPrev();
		}
		SelProfileCtrl selProfileCtrl = this.selProfileCtrl;
		bool flag5 = flag;
		bool flag6 = flag3;
		bool flag7 = flag4;
		bool flag8 = flag2;
		SceneProfile.Args args2 = this.myArgs;
		selProfileCtrl.Setup(flag5, flag6, flag7, flag8, (args2 != null) ? args2.setActiveQuestMapDataCB : null);
		CanvasManager.HdlCmnMenu.SetupMenu(true, flag ? PrjUtil.HELPER_FRIENDS_EDIT : "プロフィール", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		if (flag)
		{
			CanvasManager.SetBgObj("PanelBg_CharaEdit");
		}
		else
		{
			CanvasManager.SetBgTexture("selbg_home_in");
		}
		SoundManager.PlayBGM("prd_bgm0013");
		this.requestNextScene = SceneManager.SceneName.None;
		this.isTapReturnButton = false;
	}

	public override bool OnEnableSceneWait()
	{
		return this.selProfileCtrl.GetStart;
	}

	public override void OnStartControl()
	{
	}

	private void OnClickReturnButton()
	{
		if (this.selProfileCtrl.OnClickReturnButton())
		{
			this.isTapReturnButton = true;
			return;
		}
		this.requestNextScene = SceneManager.SceneName.SceneOtherMenuTop;
	}

	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.selProfileCtrl.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	public override void Update()
	{
		bool flag = true;
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, null);
			flag = false;
		}
		else if (this.isTapReturnButton || (this.selProfileCtrl != null && this.selProfileCtrl.IsProcessing()))
		{
			this.isTapReturnButton = false;
			flag = false;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(flag, true);
	}

	public override void OnDisableScene()
	{
		this.selProfileCtrl.RequestUpdateAvater();
		this.selProfileCtrl.SetActive(false);
		this.basePanel.gameObject.SetActive(false);
	}

	public override bool OnDisableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private GameObject basePanel;

	private SelProfileCtrl selProfileCtrl;

	private SceneProfile.Args myArgs;

	private SceneManager.SceneName requestNextScene;

	private object requestNextArgs;

	private bool isTapReturnButton;

	public class Args
	{
		public bool isHelperSettingStartFromCharaEdit;

		public bool openPhotoDetailWindow;

		public bool openAccessoryWindow;

		public bool isFromBattleSelecter;

		public UnityAction<bool> setActiveQuestMapDataCB;
	}
}
