using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Mst;
using UnityEngine;

public class SceneDataInitialize : BaseScene
{
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

	public override void OnEnableScene(object args)
	{
		this.SceneDataInitObj.SetActive(true);
		SceneManager.CanvasSetActive(SceneManager.CanvasType.PRESET, true);
	}

	public override void OnStartControl()
	{
		this.mstChk = false;
		this.sceneAction = DataInitializeResolver.InitializeActionBeforeTitle();
	}

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

	public override void OnDestroyScene()
	{
		Object.Destroy(this.ServerLoadingObj);
		Object.Destroy(this.SceneDataInitObj);
	}

	private GameObject SceneDataInitObj;

	private GameObject ServerLoadingObj;

	private SceneDataInitialize.GUI guiData;

	private IEnumerator sceneAction;

	private bool mstChk;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
		}

		public GameObject baseObj;
	}
}
