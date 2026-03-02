using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Mst;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class SceneDataInitialize : BaseScene
{
	// Token: 0x06000F14 RID: 3860 RVA: 0x000B5BAC File Offset: 0x000B3DAC
	public override void OnCreateScene()
	{
		Singleton<CanvasManager>.Instance.CloseFade();
		this.SceneDataInitObj = SceneManager.CreateEmptyPanelByBaseCanvas(SceneManager.CanvasType.PRESET, "SceneDataInitialize", true);
		GameObject gameObject = Object.Instantiate(Resources.Load("SceneAdvertise/GUI/Prefab/GUI_SYSTEM_LOG"), this.SceneDataInitObj.transform) as GameObject;
		this.guiData = new SceneDataInitialize.GUI(gameObject.transform);
		this.ServerLoadingObj = Object.Instantiate(Resources.Load("SceneLoading/GUI/Prefab/Server_Loading"), this.SceneDataInitObj.transform.parent) as GameObject;
		this.ServerLoadingObj.transform.SetSiblingIndex(this.SceneDataInitObj.transform.GetSiblingIndex() + 1);
		PguiImageCtrl component = this.ServerLoadingObj.transform.Find("Mask/GageAll").GetComponent<PguiImageCtrl>();
		component.gameObject.SetActive(true);
		component.transform.Find("Txt").gameObject.SetActive(false);
		component.transform.Find("Num_Score_Txt").GetComponent<PguiTextCtrl>().text = "0/1";
		component.transform.Find("Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = 0f;
		component.transform.Find("Num_Percent_Txt").GetComponent<PguiTextCtrl>().text = "0%";
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x000B5CF4 File Offset: 0x000B3EF4
	public override void OnEnableScene(object args)
	{
		this.SceneDataInitObj.SetActive(true);
		SceneManager.CanvasSetActive(SceneManager.CanvasType.PRESET, true);
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x000B5D09 File Offset: 0x000B3F09
	public override void OnStartControl()
	{
		this.mstChk = false;
		this.sceneAction = DataInitializeResolver.InitializeActionBeforeTitle();
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x000B5D20 File Offset: 0x000B3F20
	public override void Update()
	{
		int num = 0;
		int num2 = 0;
		if (this.mstChk)
		{
			Singleton<MstManager>.Instance.GetMstCount(out num, out num2);
		}
		else if (!Singleton<MstManager>.Instance.IsLoadFinish)
		{
			this.mstChk = true;
		}
		num++;
		if (this.sceneAction != null && !this.sceneAction.MoveNext())
		{
			KeyValuePair<SceneManager.SceneName, object>? keyValuePair = this.sceneAction.Current as KeyValuePair<SceneManager.SceneName, object>?;
			if (!this.sceneAction.MoveNext())
			{
				if (keyValuePair != null)
				{
					Singleton<SceneManager>.Instance.SetNextScene(keyValuePair.Value.Key, keyValuePair.Value.Value);
				}
				else
				{
					Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneTitle, null);
				}
				num2 = num;
				this.ServerLoadingObj.transform.Find("Mask/GageAll/Num_Score_Txt").GetComponent<PguiTextCtrl>().text = "1/1";
			}
		}
		float num3 = (float)num2 / (float)num;
		this.ServerLoadingObj.transform.Find("Mask/GageAll/Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = num3;
		this.ServerLoadingObj.transform.Find("Mask/GageAll/Num_Percent_Txt").GetComponent<PguiTextCtrl>().text = ((int)(num3 * 100f)).ToString() + "%";
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x000B5E6A File Offset: 0x000B406A
	public override void OnDisableScene()
	{
		if (null != this.ServerLoadingObj)
		{
			this.ServerLoadingObj.SetActive(false);
		}
		if (null != this.SceneDataInitObj)
		{
			this.SceneDataInitObj.SetActive(false);
		}
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x000B5EA0 File Offset: 0x000B40A0
	public override void OnDestroyScene()
	{
		Object.Destroy(this.ServerLoadingObj);
		Object.Destroy(this.SceneDataInitObj);
	}

	// Token: 0x04000DA6 RID: 3494
	private GameObject SceneDataInitObj;

	// Token: 0x04000DA7 RID: 3495
	private GameObject ServerLoadingObj;

	// Token: 0x04000DA8 RID: 3496
	private SceneDataInitialize.GUI guiData;

	// Token: 0x04000DA9 RID: 3497
	private IEnumerator sceneAction;

	// Token: 0x04000DAA RID: 3498
	private bool mstChk;

	// Token: 0x02000970 RID: 2416
	public class GUI
	{
		// Token: 0x06003BDB RID: 15323 RVA: 0x001D76AA File Offset: 0x001D58AA
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
		}

		// Token: 0x04003CFC RID: 15612
		public GameObject baseObj;
	}
}
