using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000127 RID: 295
public class SceneAdvertise : BaseScene
{
	// Token: 0x06000F08 RID: 3848 RVA: 0x000B5A78 File Offset: 0x000B3C78
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.AddComponent<RectTransform>();
		this.basePanel.name = "SceneAdvertise";
		GameObject gameObject = Object.Instantiate(Resources.Load("SceneAdvertise/GUI/Prefab/GUI_Advertise"), this.basePanel.transform) as GameObject;
		this.guiData = new SceneAdvertise.GUI(gameObject.transform);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.PRESET, this.basePanel.transform, true);
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x000B5AEF File Offset: 0x000B3CEF
	public override void OnEnableScene(object args)
	{
		GameObject.Find("Main/PresetCamera").SetActive(false);
		this.guiData.Warning.SetActive(false);
		this.guiData.APPILOGO.SetActive(false);
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x000B5B23 File Offset: 0x000B3D23
	public override void OnStartControl()
	{
		Singleton<SceneManager>.Instance.StartCoroutine(this.SceneAction());
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x000B5B36 File Offset: 0x000B3D36
	public override void Update()
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, null);
		}
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x000B5B51 File Offset: 0x000B3D51
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

	// Token: 0x06000F0D RID: 3853 RVA: 0x000B5B60 File Offset: 0x000B3D60
	public override void OnDisableScene()
	{
		this.basePanel.SetActive(false);
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x000B5B6E File Offset: 0x000B3D6E
	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
	}

	// Token: 0x04000DA3 RID: 3491
	private GameObject basePanel;

	// Token: 0x04000DA4 RID: 3492
	private SceneManager.SceneName requestNextScene;

	// Token: 0x04000DA5 RID: 3493
	private SceneAdvertise.GUI guiData;

	// Token: 0x0200096D RID: 2413
	public class GUI
	{
		// Token: 0x06003BD2 RID: 15314 RVA: 0x001D7410 File Offset: 0x001D5610
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.APPILOGO = baseTr.Find("APPILOGO").gameObject;
			this.Warning = baseTr.Find("Warning").gameObject;
			this.LOGO = baseTr.Find("APPILOGO/LOGO").GetComponent<PguiRawImageCtrl>();
		}

		// Token: 0x04003CF1 RID: 15601
		public GameObject baseObj;

		// Token: 0x04003CF2 RID: 15602
		public GameObject APPILOGO;

		// Token: 0x04003CF3 RID: 15603
		public PguiRawImageCtrl LOGO;

		// Token: 0x04003CF4 RID: 15604
		public GameObject Warning;
	}
}
