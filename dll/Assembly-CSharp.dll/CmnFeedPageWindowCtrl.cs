using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

public class CmnFeedPageWindowCtrl : MonoBehaviour
{
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

	public void SetupCheckBox()
	{
		this.guiData.CheckBox.SetActive(false);
		this.guiData.Btn_CheckBox.gameObject.SetActive(true);
	}

	public bool IsActiveCheckBox()
	{
		return this.guiData.CheckBox.activeSelf;
	}

	private void OnClickPageButton(int index)
	{
		this.currentPageIndex += ((index == 0) ? (-1) : 1);
		this.currentPageIndex = Mathf.Clamp(this.currentPageIndex, 0, this.dispTexturePathList.Count - 1);
		this.SetupPage(this.currentPageIndex);
	}

	private bool OnClickWindowButton(int index)
	{
		if (this.onCloseCb != null)
		{
			this.onCloseCb(index != PguiOpenWindowCtrl.CLOSE_BUTTON_INDEX && index != 0);
		}
		return true;
	}

	public bool FinishedClose()
	{
		return this.guiData.window.FinishedClose();
	}

	public void TestAction()
	{
		this.Init();
		this.Open(CmnFeedPageWindowCtrl.Type.SHOP_ESCORT, "aaa", new List<string> { "Texture2D/Tutorial_Window/tutorial_window_01" }, null);
	}

	private CmnFeedPageWindowCtrl.GUI guiData;

	private List<string> dispTexturePathList;

	private int currentPageIndex;

	private UnityAction<bool> onCloseCb;

	private Vector2 defaultWindowSize;

	public enum Type
	{
		INVALID,
		PAGE_FEED,
		SHOP_ESCORT
	}

	public class GUI
	{
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

		public GameObject baseObj;

		public PguiOpenWindowCtrl window;

		public PguiRawImageCtrl Texture;

		public PguiTextCtrl Num;

		public PguiButtonCtrl Btn_Back;

		public PguiButtonCtrl Btn_Front;

		public PguiButtonCtrl Btn_CheckBox;

		public GameObject CheckBox;
	}
}
