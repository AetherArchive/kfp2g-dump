using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class SceneAchievement : BaseScene
{
	// Token: 0x060013C4 RID: 5060 RVA: 0x000F2848 File Offset: 0x000F0A48
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selAchievementCtrl = this.basePanel.AddComponent<SelAchievementCtrl>();
		this.basePanel.name = "SceneAchievement";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selAchievementCtrl.Init();
	}

	// Token: 0x060013C5 RID: 5061 RVA: 0x000F28AC File Offset: 0x000F0AAC
	public override void OnEnableScene(object args)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, "称号", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		this.requestNextScene = SceneManager.SceneName.None;
		SoundManager.PlayBGM("prd_bgm0013");
		this.basePanel.gameObject.SetActive(true);
		this.selAchievementCtrl.Setup();
		this.newAcquisition = this.RequestNewAcquisition();
	}

	// Token: 0x060013C6 RID: 5062 RVA: 0x000F2920 File Offset: 0x000F0B20
	public override void OnStartControl()
	{
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x000F2924 File Offset: 0x000F0B24
	public override void Update()
	{
		bool flag = true;
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, null);
			flag = false;
		}
		if (this.newAcquisition != null && !this.newAcquisition.MoveNext())
		{
			this.newAcquisition = null;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(flag, true);
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x000F2976 File Offset: 0x000F0B76
	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x000F2989 File Offset: 0x000F0B89
	public override bool OnDisableSceneWait()
	{
		return !DataManager.IsServerRequesting();
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x000F2993 File Offset: 0x000F0B93
	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x000F29A7 File Offset: 0x000F0BA7
	private void OnClickReturnButton()
	{
		if (this.selAchievementCtrl.OnClickReturnButton())
		{
			this.requestNextScene = SceneManager.SceneName.SceneOtherMenuTop;
		}
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x000F29BE File Offset: 0x000F0BBE
	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.selAchievementCtrl.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x000F29CD File Offset: 0x000F0BCD
	private IEnumerator RequestNewAcquisition()
	{
		DataManager.DmAchievement.RequestAchievementNewAcquisition();
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		List<int> latestAcquiredAchievementIdList = DataManager.DmAchievement.GetLatestAcquiredAchievementIdList();
		if (latestAcquiredAchievementIdList.Count != 0)
		{
			List<DataManagerAchievement.AchievementStaticData> list = new List<DataManagerAchievement.AchievementStaticData>();
			foreach (int num in latestAcquiredAchievementIdList)
			{
				list.Add(DataManager.DmAchievement.GetAchievementData(num));
			}
			GetAchievementWindowCtrl.SetupParam setupParam = new GetAchievementWindowCtrl.SetupParam();
			setupParam.titleText = "称号ゲット！";
			setupParam.messageText = "以下の称号を入手しました。";
			setupParam.innerTitleText = "入手した称号";
			setupParam.callBack = (int x) => true;
			GetAchievementWindowCtrl.SetupParam setupParam2 = setupParam;
			CanvasManager.HdlGetAchievementWindowCtrl.Setup(list, setupParam2, 0);
			CanvasManager.HdlGetAchievementWindowCtrl.Open();
		}
		this.selAchievementCtrl.Setup();
		yield break;
	}

	// Token: 0x04001057 RID: 4183
	private GameObject basePanel;

	// Token: 0x04001058 RID: 4184
	private SelAchievementCtrl selAchievementCtrl;

	// Token: 0x04001059 RID: 4185
	private SceneManager.SceneName requestNextScene;

	// Token: 0x0400105A RID: 4186
	private object requestNextArgs;

	// Token: 0x0400105B RID: 4187
	private bool isTapReturnButton;

	// Token: 0x0400105C RID: 4188
	private IEnumerator newAcquisition;
}
