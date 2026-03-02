using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000143 RID: 323
public class GachaWindowStepInfoCtrl : MonoBehaviour
{
	// Token: 0x060011C1 RID: 4545 RVA: 0x000D8CC9 File Offset: 0x000D6EC9
	private void Update()
	{
		if (this.IEWindowMove != null && !this.IEWindowMove.MoveNext())
		{
			this.IEWindowMove = null;
		}
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x000D8CE7 File Offset: 0x000D6EE7
	public void Initialize()
	{
		this.gachaWindowStepInfoGuiData = new GachaWindowStepInfoCtrl.GachaWindowStepInfoGUI(base.transform);
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x000D8CFA File Offset: 0x000D6EFA
	public void Open(DataManagerGacha.GachaStaticData staticData, UnityAction openEndCb = null, UnityAction closeEndCb = null)
	{
		if (DataManagerGacha.Category.StepUp != staticData.gachaCategory)
		{
			return;
		}
		if (this.IEWindowMove == null)
		{
			this.currentGachaStaticData = staticData;
			this.IEWindowMove = this.OpenWindow(openEndCb);
			this.windowCloseEndCb = closeEndCb;
		}
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x000D8D29 File Offset: 0x000D6F29
	private IEnumerator OpenWindow(UnityAction openEndCb)
	{
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.RefreshBanner();
		this.gachaWindowStepInfoGuiData.baseWindow.Setup("ステップの状態", null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE_GREEN), true, new PguiOpenWindowCtrl.Callback(this.OnClickCloseButton), null, false);
		this.gachaWindowStepInfoGuiData.baseWindow.Open();
		if (openEndCb != null)
		{
			openEndCb();
		}
		yield break;
	}

	// Token: 0x060011C5 RID: 4549 RVA: 0x000D8D3F File Offset: 0x000D6F3F
	private void RefreshBanner()
	{
		this.gachaWindowStepInfoGuiData.BannerTexture.banner = "Texture2D/GachaTop/" + this.currentGachaStaticData.banner;
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x000D8D66 File Offset: 0x000D6F66
	private bool OnClickCloseButton(int index)
	{
		UnityAction unityAction = this.windowCloseEndCb;
		if (unityAction != null)
		{
			unityAction();
		}
		this.windowCloseEndCb = null;
		this.currentGachaStaticData = null;
		return true;
	}

	// Token: 0x04000EF1 RID: 3825
	public GachaWindowStepInfoCtrl.GachaWindowStepInfoGUI gachaWindowStepInfoGuiData;

	// Token: 0x04000EF2 RID: 3826
	private IEnumerator IEWindowMove;

	// Token: 0x04000EF3 RID: 3827
	private UnityAction windowCloseEndCb;

	// Token: 0x04000EF4 RID: 3828
	private DataManagerGacha.GachaStaticData currentGachaStaticData;

	// Token: 0x02000AC0 RID: 2752
	public class GachaWindowStepInfoGUI
	{
		// Token: 0x06004059 RID: 16473 RVA: 0x001F5294 File Offset: 0x001F3494
		public GachaWindowStepInfoGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseObj.GetComponent<PguiPanel>().raycastTarget = false;
			this.baseWindow = baseTr.Find("Window_StepInfo/").GetComponent<PguiOpenWindowCtrl>();
			this.BannerTexture = baseTr.Find("Window_StepInfo/Base/Window/Texture").GetComponent<PguiRawImageCtrl>();
		}

		// Token: 0x0400444D RID: 17485
		public GameObject baseObj;

		// Token: 0x0400444E RID: 17486
		public PguiOpenWindowCtrl baseWindow;

		// Token: 0x0400444F RID: 17487
		public PguiRawImageCtrl BannerTexture;
	}
}
