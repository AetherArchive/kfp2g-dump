using System;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200015C RID: 348
public class SceneProfile : BaseScene
{
	// Token: 0x060013F3 RID: 5107 RVA: 0x000F35F0 File Offset: 0x000F17F0
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

	// Token: 0x060013F4 RID: 5108 RVA: 0x000F36C4 File Offset: 0x000F18C4
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

	// Token: 0x060013F5 RID: 5109 RVA: 0x000F37EE File Offset: 0x000F19EE
	public override bool OnEnableSceneWait()
	{
		return this.selProfileCtrl.GetStart;
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x000F3800 File Offset: 0x000F1A00
	public override void OnStartControl()
	{
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x000F3802 File Offset: 0x000F1A02
	private void OnClickReturnButton()
	{
		if (this.selProfileCtrl.OnClickReturnButton())
		{
			this.isTapReturnButton = true;
			return;
		}
		this.requestNextScene = SceneManager.SceneName.SceneOtherMenuTop;
	}

	// Token: 0x060013F8 RID: 5112 RVA: 0x000F3821 File Offset: 0x000F1A21
	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.selProfileCtrl.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x000F3830 File Offset: 0x000F1A30
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

	// Token: 0x060013FA RID: 5114 RVA: 0x000F3894 File Offset: 0x000F1A94
	public override void OnDisableScene()
	{
		this.selProfileCtrl.RequestUpdateAvater();
		this.selProfileCtrl.SetActive(false);
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x000F38BE File Offset: 0x000F1ABE
	public override bool OnDisableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x000F38C8 File Offset: 0x000F1AC8
	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x0400106D RID: 4205
	private GameObject basePanel;

	// Token: 0x0400106E RID: 4206
	private SelProfileCtrl selProfileCtrl;

	// Token: 0x0400106F RID: 4207
	private SceneProfile.Args myArgs;

	// Token: 0x04001070 RID: 4208
	private SceneManager.SceneName requestNextScene;

	// Token: 0x04001071 RID: 4209
	private object requestNextArgs;

	// Token: 0x04001072 RID: 4210
	private bool isTapReturnButton;

	// Token: 0x02000B59 RID: 2905
	public class Args
	{
		// Token: 0x04004715 RID: 18197
		public bool isHelperSettingStartFromCharaEdit;

		// Token: 0x04004716 RID: 18198
		public bool openPhotoDetailWindow;

		// Token: 0x04004717 RID: 18199
		public bool openAccessoryWindow;

		// Token: 0x04004718 RID: 18200
		public bool isFromBattleSelecter;

		// Token: 0x04004719 RID: 18201
		public UnityAction<bool> setActiveQuestMapDataCB;
	}
}
