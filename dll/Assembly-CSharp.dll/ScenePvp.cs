using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

// Token: 0x0200016A RID: 362
public class ScenePvp : BaseScene
{
	// Token: 0x06001577 RID: 5495 RVA: 0x0010D608 File Offset: 0x0010B808
	public override void OnCreateScene()
	{
		GameObject gameObject = new GameObject();
		gameObject.AddComponent<RectTransform>();
		this.selPvpCtrl = gameObject.AddComponent<SelPvpCtrl>();
		gameObject.name = "ScenePvp";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, gameObject.transform, true);
		this.selPvpCtrl.Init();
	}

	// Token: 0x06001578 RID: 5496 RVA: 0x0010D654 File Offset: 0x0010B854
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

	// Token: 0x06001579 RID: 5497 RVA: 0x0010D78A File Offset: 0x0010B98A
	public override bool OnEnableSceneWait()
	{
		return !this.voiceLoadEnumerator.MoveNext();
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x0010D79C File Offset: 0x0010B99C
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

	// Token: 0x0600157B RID: 5499 RVA: 0x0010D80F File Offset: 0x0010BA0F
	public override void OnDisableScene()
	{
		this.selPvpCtrl.Disable();
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x0010D81C File Offset: 0x0010BA1C
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

	// Token: 0x0600157D RID: 5501 RVA: 0x0010D848 File Offset: 0x0010BA48
	public override void OnDestroyScene()
	{
		this.selPvpCtrl.Destroy();
		Object.Destroy(this.selPvpCtrl.gameObject);
		this.selPvpCtrl = null;
	}

	// Token: 0x040011C0 RID: 4544
	private SelPvpCtrl selPvpCtrl;

	// Token: 0x040011C1 RID: 4545
	private IEnumerator voiceLoadEnumerator;

	// Token: 0x040011C2 RID: 4546
	public static readonly string voiceSheet = "cv_dojo";

	// Token: 0x040011C3 RID: 4547
	public static readonly List<string> voiceNameList = new List<string> { "prd_cv_dojo_training_menu", "prd_cv_dojo_training_after_organize", "prd_cv_dojo_training_after_win", "prd_cv_dojo_training_after_lose", "prd_cv_dojo_training_touch" };

	// Token: 0x040011C4 RID: 4548
	private SceneManager.SceneName requestNextScene;

	// Token: 0x040011C5 RID: 4549
	private object requestNextArgs;

	// Token: 0x02000C02 RID: 3074
	public class Args
	{
		// Token: 0x04004959 RID: 18777
		public int fastPvpSeasonId;

		// Token: 0x0400495A RID: 18778
		public bool isStartTutorial;

		// Token: 0x0400495B RID: 18779
		public bool isStartSpecialPvpTutorial;

		// Token: 0x0400495C RID: 18780
		public bool isReturnFromBattle;

		// Token: 0x0400495D RID: 18781
		public bool isReturnFromPvpDeck;
	}
}
