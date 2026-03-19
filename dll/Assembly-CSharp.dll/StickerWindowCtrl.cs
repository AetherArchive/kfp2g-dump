using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickerWindowCtrl : MonoBehaviour
{
	private List<DataManagerSticker.StickerPackData> DispStickerPacks { get; set; }

	public void Init()
	{
		if (this.guiData != null)
		{
			return;
		}
		this.guiData = new StickerWindowCtrl.GUI(base.transform);
		this.guiData.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickLRButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickLRButton), PguiButtonCtrl.SoundType.DEFAULT);
		PrjUtil.AddTouchEventTrigger(this.guiData.iconStickerCtrl.ItemTexture.gameObject, new UnityAction<Transform>(this.OnTouchSticker));
		PrjUtil.AddTouchEventTrigger(this.guiData.iconStickerCtrlBig.ItemTexture.gameObject, new UnityAction<Transform>(this.OnTouchSticker));
		PrjUtil.AddTouchEventTrigger(this.guiData.stickerBigBase, new UnityAction<Transform>(this.OnTouchSticker));
	}

	private void OnClickLRButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.Btn_Yaji_Left || button == this.guiData.Btn_Yaji_Right)
		{
			if (this.DispStickerPacks == null || !this.DispStickerPacks.Contains(this.stickerPackData))
			{
				return;
			}
			this.RequestClickLRButton(button);
		}
	}

	private void RequestClickLRButton(PguiButtonCtrl button)
	{
		int num = this.DispStickerPacks.IndexOf(this.stickerPackData);
		num += ((button == this.guiData.Btn_Yaji_Left) ? (-1) : 1);
		num = (num + this.DispStickerPacks.Count) % this.DispStickerPacks.Count;
		this.ChangeSticker(this.DispStickerPacks[num], null);
	}

	private void OnTouchSticker(Transform touch)
	{
		if (touch.gameObject == this.guiData.iconStickerCtrl.ItemTexture.gameObject)
		{
			this.guiData.stickerBigBase.SetActive(true);
			this.guiData.iconStickerCtrlBig.FrameDisp = false;
			this.touchTransform = null;
			return;
		}
		if (touch.gameObject == this.guiData.iconStickerCtrlBig.ItemTexture.gameObject)
		{
			this.guiData.stickerBigBase.SetActive(false);
			this.touchTransform = touch;
		}
	}

	private void ChangeSticker(DataManagerSticker.StickerPackData spd, IconStickerCtrl ipc)
	{
		this.ChangeStickerGUI(spd, ipc);
	}

	private void ChangeStickerGUI(DataManagerSticker.StickerPackData spd, IconStickerCtrl isc)
	{
		this.stickerPackData = spd;
		this.guiData.iconStickerCtrl.SetupPackData(spd, false);
		this.guiData.iconStickerCtrlBig.SetupPackData(spd, true);
		this.currentIconStickerCtrl = isc;
		this.touchTransform = null;
		this.guiData.window.Setup(spd.staticData.name, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			UnityAction unityAction = this.callbackCloseWindow;
			if (unityAction != null)
			{
				unityAction();
			}
			return true;
		}, null, false);
	}

	private void Update()
	{
		if (this.touchTransform != null)
		{
			this.OnTouchSticker(this.touchTransform);
		}
	}

	public void Open(StickerWindowCtrl.SetupParam param)
	{
		this.setupParam = param;
		this.Open(param.spd, param.isc, param.cb, param.dispMax, param.dispStickerPacks, param.closeWindowCB);
	}

	private void Open(DataManagerSticker.StickerPackData spd, IconStickerCtrl isc, UnityAction cb, bool dispMax, List<DataManagerSticker.StickerPackData> stickerPackDatas, UnityAction cbClose)
	{
		this.DispStickerPacks = stickerPackDatas;
		this.callback = cb;
		this.callbackCloseWindow = cbClose;
		base.gameObject.SetActive(true);
		this.ChangeSticker(spd, isc);
		this.guiData.window.Open();
	}

	private void OnDestroy()
	{
		if (this.guiData != null)
		{
			Object.Destroy(this.guiData.baseObj);
		}
	}

	private StickerWindowCtrl.SetupParam setupParam = new StickerWindowCtrl.SetupParam();

	private StickerWindowCtrl.GUI guiData;

	private DataManagerSticker.StickerPackData stickerPackData;

	private IconStickerCtrl currentIconStickerCtrl;

	private Transform touchTransform;

	private UnityAction callback;

	private UnityAction callbackCloseWindow;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Yaji_Left = baseTr.Find("Base/Window/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
			this.Btn_Yaji_Right = baseTr.Find("Base/Window/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
			this.iconStickerCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Card_Sticker, baseTr.Find("Base/Window/CardSticker")).GetComponent<IconStickerCtrl>();
			this.stickerBigBase = baseTr.Find("Base/FrontBg").gameObject;
			this.iconStickerCtrlBig = Object.Instantiate<GameObject>(CanvasManager.RefResource.Card_Sticker, baseTr.Find("Base/FrontBg/CardSticker")).GetComponent<IconStickerCtrl>();
			this.stickerBigBase.gameObject.SetActive(false);
			AssetManager.GetAssetData("SelCmn/GUI/Prefab/Icon_PhotoRebirth");
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		public GameObject baseObj;

		public IconStickerCtrl iconStickerCtrl;

		public GameObject stickerBigBase;

		public IconStickerCtrl iconStickerCtrlBig;

		public PguiButtonCtrl Btn_Yaji_Left;

		public PguiButtonCtrl Btn_Yaji_Right;

		public PguiOpenWindowCtrl window;
	}

	public class SetupParam
	{
		public DataManagerSticker.StickerPackData spd;

		public IconStickerCtrl isc;

		public UnityAction cb;

		public bool dispMax;

		public List<DataManagerSticker.StickerPackData> dispStickerPacks;

		public UnityAction closeWindowCB;
	}
}
