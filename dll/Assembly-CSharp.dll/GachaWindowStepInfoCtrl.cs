using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GachaWindowStepInfoCtrl : MonoBehaviour
{
	private void Update()
	{
		if (this.IEWindowMove != null && !this.IEWindowMove.MoveNext())
		{
			this.IEWindowMove = null;
		}
	}

	public void Initialize()
	{
		this.gachaWindowStepInfoGuiData = new GachaWindowStepInfoCtrl.GachaWindowStepInfoGUI(base.transform);
	}

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

	private void RefreshBanner()
	{
		this.gachaWindowStepInfoGuiData.BannerTexture.banner = "Texture2D/GachaTop/" + this.currentGachaStaticData.banner;
	}

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

	public GachaWindowStepInfoCtrl.GachaWindowStepInfoGUI gachaWindowStepInfoGuiData;

	private IEnumerator IEWindowMove;

	private UnityAction windowCloseEndCb;

	private DataManagerGacha.GachaStaticData currentGachaStaticData;

	public class GachaWindowStepInfoGUI
	{
		public GachaWindowStepInfoGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseObj.GetComponent<PguiPanel>().raycastTarget = false;
			this.baseWindow = baseTr.Find("Window_StepInfo/").GetComponent<PguiOpenWindowCtrl>();
			this.BannerTexture = baseTr.Find("Window_StepInfo/Base/Window/Texture").GetComponent<PguiRawImageCtrl>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl baseWindow;

		public PguiRawImageCtrl BannerTexture;
	}
}
