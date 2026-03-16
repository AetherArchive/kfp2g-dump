using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

public class SceneAchievement : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selAchievementCtrl = this.basePanel.AddComponent<SelAchievementCtrl>();
		this.basePanel.name = "SceneAchievement";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selAchievementCtrl.Init();
	}

	public override void OnEnableScene(object args)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, "称号", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickReturnButton), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		this.requestNextScene = SceneManager.SceneName.None;
		SoundManager.PlayBGM("prd_bgm0013");
		this.basePanel.gameObject.SetActive(true);
		this.selAchievementCtrl.Setup();
		this.newAcquisition = this.RequestNewAcquisition();
	}

	public override void OnStartControl()
	{
	}

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

	public override void OnDisableScene()
	{
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

	private void OnClickReturnButton()
	{
		if (this.selAchievementCtrl.OnClickReturnButton())
		{
			this.requestNextScene = SceneManager.SceneName.SceneOtherMenuTop;
		}
	}

	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.selAchievementCtrl.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

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

	private GameObject basePanel;

	private SelAchievementCtrl selAchievementCtrl;

	private SceneManager.SceneName requestNextScene;

	private object requestNextArgs;

	private bool isTapReturnButton;

	private IEnumerator newAcquisition;
}
