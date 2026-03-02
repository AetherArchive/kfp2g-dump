using System;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class SceneMission : BaseScene
{
	// Token: 0x06001360 RID: 4960 RVA: 0x000EFA6C File Offset: 0x000EDC6C
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selMissionCtrl = this.basePanel.AddComponent<SelMissionCtrl>();
		this.basePanel.name = "SceneMission";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selMissionCtrl.Init();
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x000EFAD0 File Offset: 0x000EDCD0
	public override void OnEnableScene(object args)
	{
		this.missionArgs = args as SceneMission.MissionOpenParam;
		CanvasManager.HdlCmnMenu.SetupMenu(true, "ミッション", delegate
		{
			if (this.missionArgs != null && this.missionArgs.returnSceneName != SceneManager.SceneName.None)
			{
				this.requestNextScene = this.missionArgs.returnSceneName;
				this.requestNextSceneArgs = this.missionArgs.resultNextSceneArgs;
				return;
			}
			this.requestNextScene = SceneManager.SceneName.SceneHome;
		}, "", null, null);
		CanvasManager.SetBgObj("PanelBg_HomeIn");
		SoundManager.PlayBGM("prd_bgm0013");
		this.requestNextScene = SceneManager.SceneName.None;
		this.requestNextSceneArgs = null;
		DataManager.DmMission.RequestGetMissionList();
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x000EFB38 File Offset: 0x000EDD38
	public override bool OnEnableSceneWait()
	{
		if (DataManager.IsServerRequesting())
		{
			return false;
		}
		this.basePanel.gameObject.SetActive(true);
		this.selMissionCtrl.Setup(this.missionArgs, delegate(SceneManager.SceneName scenenName, object obj)
		{
			this.requestNextScene = scenenName;
			this.requestNextSceneArgs = obj;
		});
		return true;
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x000EFB74 File Offset: 0x000EDD74
	public override void Update()
	{
		bool flag = true;
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.requestNextSceneArgs);
			flag = false;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(flag, true);
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x000EFBAF File Offset: 0x000EDDAF
	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x000EFBC2 File Offset: 0x000EDDC2
	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x0400102E RID: 4142
	private GameObject basePanel;

	// Token: 0x0400102F RID: 4143
	private SelMissionCtrl selMissionCtrl;

	// Token: 0x04001030 RID: 4144
	private SceneManager.SceneName requestNextScene;

	// Token: 0x04001031 RID: 4145
	private object requestNextSceneArgs;

	// Token: 0x04001032 RID: 4146
	private SceneMission.MissionOpenParam missionArgs;

	// Token: 0x02000B2C RID: 2860
	public class MissionOpenParam
	{
		// Token: 0x060041E1 RID: 16865 RVA: 0x001FDD07 File Offset: 0x001FBF07
		public MissionOpenParam(MissionType type, int evid)
		{
			this.missionType = type;
			this.eventId = evid;
		}

		// Token: 0x0400463F RID: 17983
		public MissionType missionType;

		// Token: 0x04004640 RID: 17984
		public int eventId;

		// Token: 0x04004641 RID: 17985
		public SceneManager.SceneName returnSceneName = SceneManager.SceneName.SceneHome;

		// Token: 0x04004642 RID: 17986
		public object resultNextSceneArgs;
	}
}
