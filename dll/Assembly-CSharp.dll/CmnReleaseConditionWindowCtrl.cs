using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200019A RID: 410
public class CmnReleaseConditionWindowCtrl : MonoBehaviour
{
	// Token: 0x06001B2A RID: 6954 RVA: 0x0015D99C File Offset: 0x0015BB9C
	public void Init()
	{
		this.guiData = new CmnReleaseConditionWindowCtrl.GUI(base.transform);
		this.guiData.baseObj.SetActive(false);
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x0015D9C0 File Offset: 0x0015BBC0
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

	// Token: 0x06001B2C RID: 6956 RVA: 0x0015DB04 File Offset: 0x0015BD04
	private IEnumerator UpdateGrid()
	{
		yield return new WaitForEndOfFrame();
		this.guiData.grid.GetComponent<ContentSizeFitter>().enabled = false;
		this.guiData.grid.GetComponent<ContentSizeFitter>().enabled = true;
		yield break;
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x0015DB13 File Offset: 0x0015BD13
	private void Update()
	{
		if (this.guiData.window.StartOpenAnim() && this.requestSequence == null)
		{
			this.requestSequence = Singleton<SceneManager>.Instance.StartCoroutine(this.UpdateGrid());
		}
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x0015DB45 File Offset: 0x0015BD45
	private void OnDestroy()
	{
		if (this.requestSequence != null && Singleton<SceneManager>.Instance != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.requestSequence);
		}
		this.requestSequence = null;
	}

	// Token: 0x04001485 RID: 5253
	private CmnReleaseConditionWindowCtrl.GUI guiData;

	// Token: 0x04001486 RID: 5254
	private Coroutine requestSequence;

	// Token: 0x02000EAD RID: 3757
	public class SetupParam
	{
		// Token: 0x04005438 RID: 21560
		public string text;

		// Token: 0x04005439 RID: 21561
		public bool enableClear;
	}

	// Token: 0x02000EAE RID: 3758
	public class GUI
	{
		// Token: 0x06004D5C RID: 19804 RVA: 0x002327E4 File Offset: 0x002309E4
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

		// Token: 0x0400543A RID: 21562
		public GameObject baseObj;

		// Token: 0x0400543B RID: 21563
		public List<GameObject> OpenInfo;

		// Token: 0x0400543C RID: 21564
		public PguiTextCtrl Text;

		// Token: 0x0400543D RID: 21565
		public PguiOpenWindowCtrl window;

		// Token: 0x0400543E RID: 21566
		public GameObject grid;
	}
}
