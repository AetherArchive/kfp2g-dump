using System;
using SGNFW.Common;
using UnityEngine;

public class SceneMission : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.selMissionCtrl = this.basePanel.AddComponent<SelMissionCtrl>();
		this.basePanel.name = "SceneMission";
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.selMissionCtrl.Init();
	}

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

	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private GameObject basePanel;

	private SelMissionCtrl selMissionCtrl;

	private SceneManager.SceneName requestNextScene;

	private object requestNextSceneArgs;

	private SceneMission.MissionOpenParam missionArgs;

	public class MissionOpenParam
	{
		public MissionOpenParam(MissionType type, int evid)
		{
			this.missionType = type;
			this.eventId = evid;
		}

		public MissionType missionType;

		public int eventId;

		public SceneManager.SceneName returnSceneName = SceneManager.SceneName.SceneHome;

		public object resultNextSceneArgs;
	}
}
