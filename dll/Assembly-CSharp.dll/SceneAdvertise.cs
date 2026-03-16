using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;

public class SceneAdvertise : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.basePanel.name = "SceneAdvertise";
		GameObject gameObject = Object.Instantiate(Resources.Load("SceneAdvertise/GUI/Prefab/GUI_Advertise"), this.basePanel.transform) as GameObject;
		this.guiData = new SceneAdvertise.GUI(gameObject.transform);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.PRESET, this.basePanel.transform, true);
	}

	public override void OnEnableScene(object args)
	{
		GameObject.Find("Main/PresetCamera").SetActive(false);
		this.guiData.Warning.SetActive(false);
		this.guiData.APPILOGO.SetActive(false);
	}

	public override void OnStartControl()
	{
		Singleton<SceneManager>.Instance.StartCoroutine(this.SceneAction());
	}

	public override void Update()
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, null);
		}
	}

	private IEnumerator SceneAction()
	{
		yield return new WaitForSeconds(1f);
		this.guiData.APPILOGO.gameObject.SetActive(true);
		SoundManager.randPlay("prd_se_appirits_call", false);
		yield return new WaitForSeconds(2f);
		int fadeFrame = 15;
		int num;
		for (int i = 0; i < fadeFrame; i = num + 1)
		{
			this.guiData.LOGO.m_RawImage.color = new Color(1f, 1f, 1f, (float)(fadeFrame - i) / (float)fadeFrame);
			yield return null;
			num = i;
		}
		this.guiData.LOGO.m_RawImage.color = Color.clear;
		yield return new WaitForSeconds(1f);
		this.guiData.APPILOGO.SetActive(false);
		this.guiData.Warning.SetActive(true);
		bool isTouch = false;
		PrjUtil.AddTouchEventTrigger(this.guiData.Warning.gameObject, delegate(Transform tr)
		{
			isTouch = true;
		});
		while (!isTouch)
		{
			yield return null;
		}
		SoundManager.Play("prd_se_decide", false, false);
		this.requestNextScene = SceneManager.SceneName.SceneDataInitialize;
		yield break;
	}

	public override void OnDisableScene()
	{
		this.basePanel.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
	}

	private GameObject basePanel;

	private SceneManager.SceneName requestNextScene;

	private SceneAdvertise.GUI guiData;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.APPILOGO = baseTr.Find("APPILOGO").gameObject;
			this.Warning = baseTr.Find("Warning").gameObject;
			this.LOGO = baseTr.Find("APPILOGO/LOGO").GetComponent<PguiRawImageCtrl>();
		}

		public GameObject baseObj;

		public GameObject APPILOGO;

		public PguiRawImageCtrl LOGO;

		public GameObject Warning;
	}
}
