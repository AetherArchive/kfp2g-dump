using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

public class ScenePvp : BaseScene
{
	public override void OnCreateScene()
	{
		GameObject gameObject = new GameObject();
		gameObject.AddComponent<RectTransform>();
		this.selPvpCtrl = gameObject.AddComponent<SelPvpCtrl>();
		gameObject.name = "ScenePvp";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, gameObject.transform, true);
		this.selPvpCtrl.Init();
	}

	public override void OnEnableScene(object args)
	{
		ScenePvp.Args args2 = args as ScenePvp.Args;
		if (!DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.PvpFirst && (args2 == null || !args2.isStartTutorial))
		{
			this.requestNextScene = SceneManager.SceneName.SceneScenario;
			this.requestNextArgs = new SceneScenario.Args
			{
				questId = 21,
				storyType = 1,
				scenarioName = DataManager.DmQuest.QuestStaticData.oneDataMap[21].scenarioBeforeId,
				nextSceneName = SceneManager.SceneName.ScenePvp,
				nextSceneArgs = new ScenePvp.Args
				{
					isStartTutorial = true
				}
			};
			int seasonIdByNow = DataManager.DmPvp.GetSeasonIdByNow(TimeManager.Now, PvpStaticData.Type.NORMAL);
			PvpStaticData pvpStaticDataNormalForGuiTop = this.selPvpCtrl.GetPvpStaticDataNormalForGuiTop(seasonIdByNow);
			this.selPvpCtrl.ChangeBGSetting(pvpStaticDataNormalForGuiTop);
		}
		else
		{
			this.selPvpCtrl.gameObject.SetActive(true);
			this.selPvpCtrl.Setup((args2 != null) ? args2.fastPvpSeasonId : 0, args2 != null && args2.isStartTutorial, args2 != null && args2.isStartSpecialPvpTutorial, args2 != null && args2.isReturnFromBattle, args2 != null && args2.isReturnFromPvpDeck);
			this.requestNextScene = SceneManager.SceneName.None;
		}
		this.voiceLoadEnumerator = SoundManager.LoadCueSheetWithDownload(ScenePvp.voiceSheet);
	}

	public override bool OnEnableSceneWait()
	{
		return !this.voiceLoadEnumerator.MoveNext();
	}

	public override void Update()
	{
		if (this.selPvpCtrl.requestNextScene != SceneManager.SceneName.None)
		{
			this.requestNextScene = this.selPvpCtrl.requestNextScene;
			this.requestNextArgs = this.selPvpCtrl.requestNextArgs;
		}
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.requestNextArgs);
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(!this.selPvpCtrl.IsBusy(), true);
	}

	public override void OnDisableScene()
	{
		this.selPvpCtrl.Disable();
	}

	public override bool OnDisableSceneWait()
	{
		if (this.selPvpCtrl.IsBusy())
		{
			return false;
		}
		if (DataManager.IsServerRequesting())
		{
			return false;
		}
		SoundManager.UnloadCueSheet(ScenePvp.voiceSheet);
		this.voiceLoadEnumerator = null;
		return true;
	}

	public override void OnDestroyScene()
	{
		this.selPvpCtrl.Destroy();
		Object.Destroy(this.selPvpCtrl.gameObject);
		this.selPvpCtrl = null;
	}

	private SelPvpCtrl selPvpCtrl;

	private IEnumerator voiceLoadEnumerator;

	public static readonly string voiceSheet = "cv_dojo";

	public static readonly List<string> voiceNameList = new List<string> { "prd_cv_dojo_training_menu", "prd_cv_dojo_training_after_organize", "prd_cv_dojo_training_after_win", "prd_cv_dojo_training_after_lose", "prd_cv_dojo_training_touch" };

	private SceneManager.SceneName requestNextScene;

	private object requestNextArgs;

	public class Args
	{
		public int fastPvpSeasonId;

		public bool isStartTutorial;

		public bool isStartSpecialPvpTutorial;

		public bool isReturnFromBattle;

		public bool isReturnFromPvpDeck;
	}
}
