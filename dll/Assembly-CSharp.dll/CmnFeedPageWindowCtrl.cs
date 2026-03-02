using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000196 RID: 406
public class CmnFeedPageWindowCtrl : MonoBehaviour
{
	// Token: 0x06001B09 RID: 6921 RVA: 0x0015C9B8 File Offset: 0x0015ABB8
	public void Init()
	{
		GameObject gameObject = Resources.Load("Cmn/GUI/Prefab/CmnFeedPageWindow") as GameObject;
		this.guiData = new CmnFeedPageWindowCtrl.GUI(Object.Instantiate<GameObject>(gameObject, base.transform).transform);
		this.guiData.Btn_Back.AddOnClickListener(delegate(PguiButtonCtrl go)
		{
			this.OnClickPageButton(0);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Front.AddOnClickListener(delegate(PguiButtonCtrl go)
		{
			this.OnClickPageButton(1);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.CheckBox.SetActive(false);
		this.guiData.Btn_CheckBox.AddOnClickListener(delegate(PguiButtonCtrl go)
		{
			this.guiData.CheckBox.SetActive(!this.guiData.CheckBox.activeSelf);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.defaultWindowSize = this.guiData.window.WindowRectTransform.sizeDelta;
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x0015CA74 File Offset: 0x0015AC74
	public void Open(CmnFeedPageWindowCtrl.Type type, string title, List<string> dispTexturePathList, UnityAction<bool> cb = null)
	{
		this.onCloseCb = cb;
		base.gameObject.SetActive(true);
		this.dispTexturePathList = new List<string>(dispTexturePathList);
		this.guiData.window.WindowRectTransform.sizeDelta = this.defaultWindowSize;
		if (this.dispTexturePathList.Count > 0)
		{
			AssetPathParameter assetPathParameter = Singleton<AssetManager>.Instance.GetAssetPathParameter();
			if (assetPathParameter != null)
			{
				AssetPathParameter.Data data = assetPathParameter.DataList.Find((AssetPathParameter.Data item) => this.dispTexturePathList[0].StartsWith(item.path));
				if (data != null)
				{
					this.guiData.window.WindowRectTransform.sizeDelta = data.feedWindowSize;
				}
			}
		}
		this.currentPageIndex = 0;
		this.SetupPage(this.currentPageIndex);
		if (type == CmnFeedPageWindowCtrl.Type.PAGE_FEED)
		{
			this.guiData.Num.gameObject.SetActive(true);
			this.guiData.Btn_Back.gameObject.SetActive(true);
			this.guiData.Btn_Front.gameObject.SetActive(true);
		}
		else
		{
			this.guiData.Num.gameObject.SetActive(false);
			this.guiData.Btn_Back.gameObject.SetActive(false);
			this.guiData.Btn_Front.gameObject.SetActive(false);
		}
		this.guiData.CheckBox.SetActive(false);
		this.guiData.Btn_CheckBox.gameObject.SetActive(false);
		this.guiData.window.Setup(title, null, PguiOpenWindowCtrl.GetButtonPreset((type == CmnFeedPageWindowCtrl.Type.PAGE_FEED) ? PguiOpenWindowCtrl.PresetType.LR_CURSOR : PguiOpenWindowCtrl.PresetType.CLOSE_SHOP), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), null, false);
		this.guiData.window.Open();
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x0015CC18 File Offset: 0x0015AE18
	private void SetupPage(int index)
	{
		if (this.dispTexturePathList == null)
		{
			return;
		}
		this.guiData.Num.text = (index + 1).ToString() + "/" + this.dispTexturePathList.Count.ToString();
		this.guiData.Texture.gameObject.SetActive(this.dispTexturePathList.Count > index);
		if (this.dispTexturePathList.Count > index)
		{
			this.guiData.Texture.SetRawImage(this.dispTexturePathList[index], true, true, null);
		}
		this.guiData.Btn_Back.SetActEnable(index > 0, false, false);
		this.guiData.Btn_Front.SetActEnable(index + 1 < this.dispTexturePathList.Count, false, false);
	}

	// Token: 0x06001B0C RID: 6924 RVA: 0x0015CCEE File Offset: 0x0015AEEE
	public void SetupCheckBox()
	{
		this.guiData.CheckBox.SetActive(false);
		this.guiData.Btn_CheckBox.gameObject.SetActive(true);
	}

	// Token: 0x06001B0D RID: 6925 RVA: 0x0015CD17 File Offset: 0x0015AF17
	public bool IsActiveCheckBox()
	{
		return this.guiData.CheckBox.activeSelf;
	}

	// Token: 0x06001B0E RID: 6926 RVA: 0x0015CD2C File Offset: 0x0015AF2C
	private void OnClickPageButton(int index)
	{
		this.currentPageIndex += ((index == 0) ? (-1) : 1);
		this.currentPageIndex = Mathf.Clamp(this.currentPageIndex, 0, this.dispTexturePathList.Count - 1);
		this.SetupPage(this.currentPageIndex);
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x0015CD78 File Offset: 0x0015AF78
	private bool OnClickWindowButton(int index)
	{
		if (this.onCloseCb != null)
		{
			this.onCloseCb(index != PguiOpenWindowCtrl.CLOSE_BUTTON_INDEX && index != 0);
		}
		return true;
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x0015CD9D File Offset: 0x0015AF9D
	public bool FinishedClose()
	{
		return this.guiData.window.FinishedClose();
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x0015CDAF File Offset: 0x0015AFAF
	public void TestAction()
	{
		this.Init();
		this.Open(CmnFeedPageWindowCtrl.Type.SHOP_ESCORT, "aaa", new List<string> { "Texture2D/Tutorial_Window/tutorial_window_01" }, null);
	}

	// Token: 0x0400147A RID: 5242
	private CmnFeedPageWindowCtrl.GUI guiData;

	// Token: 0x0400147B RID: 5243
	private List<string> dispTexturePathList;

	// Token: 0x0400147C RID: 5244
	private int currentPageIndex;

	// Token: 0x0400147D RID: 5245
	private UnityAction<bool> onCloseCb;

	// Token: 0x0400147E RID: 5246
	private Vector2 defaultWindowSize;

	// Token: 0x02000EA5 RID: 3749
	public enum Type
	{
		// Token: 0x040053FC RID: 21500
		INVALID,
		// Token: 0x040053FD RID: 21501
		PAGE_FEED,
		// Token: 0x040053FE RID: 21502
		SHOP_ESCORT
	}

	// Token: 0x02000EA6 RID: 3750
	public class GUI
	{
		// Token: 0x06004D47 RID: 19783 RVA: 0x002319F4 File Offset: 0x0022FBF4
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Texture = baseTr.Find("Base/Window/Texture").GetComponent<PguiRawImageCtrl>();
			this.Num = baseTr.Find("Base/Window/Num").GetComponent<PguiTextCtrl>();
			this.Btn_Back = baseTr.Find("Base/Window/Btn_Back").GetComponent<PguiButtonCtrl>();
			this.Btn_Front = baseTr.Find("Base/Window/Btn_Front").GetComponent<PguiButtonCtrl>();
			this.Btn_CheckBox = baseTr.Find("Base/Window/Btn_CheckBox").GetComponent<PguiButtonCtrl>();
			this.CheckBox = this.Btn_CheckBox.transform.Find("BaseImage/Img_Check").gameObject;
		}

		// Token: 0x040053FF RID: 21503
		public GameObject baseObj;

		// Token: 0x04005400 RID: 21504
		public PguiOpenWindowCtrl window;

		// Token: 0x04005401 RID: 21505
		public PguiRawImageCtrl Texture;

		// Token: 0x04005402 RID: 21506
		public PguiTextCtrl Num;

		// Token: 0x04005403 RID: 21507
		public PguiButtonCtrl Btn_Back;

		// Token: 0x04005404 RID: 21508
		public PguiButtonCtrl Btn_Front;

		// Token: 0x04005405 RID: 21509
		public PguiButtonCtrl Btn_CheckBox;

		// Token: 0x04005406 RID: 21510
		public GameObject CheckBox;
	}
}
