using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

public class CmnReleaseConditionWindowCtrl : MonoBehaviour
{
	public void Init()
	{
		this.guiData = new CmnReleaseConditionWindowCtrl.GUI(base.transform);
		this.guiData.baseObj.SetActive(false);
	}

	public void Open(string title, List<CmnReleaseConditionWindowCtrl.SetupParam> setupParam)
	{
		base.gameObject.SetActive(true);
		this.requestSequence = null;
		foreach (GameObject gameObject in this.guiData.OpenInfo)
		{
			gameObject.gameObject.SetActive(false);
		}
		for (int i = 0; i < setupParam.Count; i++)
		{
			if (i < this.guiData.OpenInfo.Count)
			{
				CmnReleaseConditionWindowCtrl.SetupParam setupParam2 = setupParam[i];
				GameObject gameObject2 = this.guiData.OpenInfo[i];
				gameObject2.gameObject.SetActive(true);
				gameObject2.transform.Find("Txt_LockInfo").GetComponent<PguiTextCtrl>().text = setupParam2.text;
				gameObject2.transform.Find("Mark_Clear").gameObject.SetActive(setupParam2.enableClear);
			}
		}
		this.guiData.window.Setup(title, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		this.guiData.window.Open();
	}

	private IEnumerator UpdateGrid()
	{
		yield return new WaitForEndOfFrame();
		this.guiData.grid.GetComponent<ContentSizeFitter>().enabled = false;
		this.guiData.grid.GetComponent<ContentSizeFitter>().enabled = true;
		yield break;
	}

	private void Update()
	{
		if (this.guiData.window.StartOpenAnim() && this.requestSequence == null)
		{
			this.requestSequence = Singleton<SceneManager>.Instance.StartCoroutine(this.UpdateGrid());
		}
	}

	private void OnDestroy()
	{
		if (this.requestSequence != null && Singleton<SceneManager>.Instance != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.requestSequence);
		}
		this.requestSequence = null;
	}

	private CmnReleaseConditionWindowCtrl.GUI guiData;

	private Coroutine requestSequence;

	public class SetupParam
	{
		public string text;

		public bool enableClear;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.grid = baseTr.Find("Base/Window/Grid").gameObject;
			this.OpenInfo = new List<GameObject>
			{
				this.grid.transform.Find("OpenInfo_01").gameObject,
				this.grid.transform.Find("OpenInfo_02").gameObject,
				this.grid.transform.Find("OpenInfo_03").gameObject,
				this.grid.transform.Find("OpenInfo_04").gameObject
			};
			this.Text = baseTr.Find("Base/Window/Title/Text").GetComponent<PguiTextCtrl>();
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		public GameObject baseObj;

		public List<GameObject> OpenInfo;

		public PguiTextCtrl Text;

		public PguiOpenWindowCtrl window;

		public GameObject grid;
	}
}
