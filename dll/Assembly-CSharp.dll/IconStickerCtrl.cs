using System;
using UnityEngine;

public class IconStickerCtrl : MonoBehaviour
{
	public PguiRawImageCtrl ItemTexture
	{
		get
		{
			return this.itemTexture;
		}
	}

	public DataManagerSticker.StickerPackData stickerPackData { get; private set; }

	public bool FrameDisp
	{
		get
		{
			return this.textureFrame != null && this.textureFrame.gameObject.activeSelf;
		}
		set
		{
			if (this.textureFrame != null)
			{
				this.textureFrame.gameObject.SetActive(value);
			}
		}
	}

	public void Setup(IconStickerCtrl.SetupParam param)
	{
		this.setupParam = param;
		this.SetupPackData(param.spd, false);
	}

	public void SetupPackData(DataManagerSticker.StickerPackData spd, bool isBig = false)
	{
		this.stickerPackData = spd;
		if (spd != null)
		{
			this.SetupStaticData(spd.staticData, isBig);
			if (this.numText != null)
			{
				this.numText.gameObject.SetActive(spd.dynamicData != null);
				if (spd.dynamicData != null)
				{
					this.numText.text = spd.dynamicData.num.ToString();
					return;
				}
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	public void SetupStaticData(DataManagerSticker.StickerStaticData ssd, bool isBig)
	{
		if (ssd != null)
		{
			base.gameObject.SetActive(true);
			if (this.itemTexture != null)
			{
				this.DispStickerChange(false);
				this.itemTexture.m_RawImage.raycastTarget = this.stickerPackData != null && this.stickerPackData.dynamicData.num > 0;
			}
			if (this.textCardName != null)
			{
				this.textCardName.text = ssd.name;
			}
			string text = "";
			string text2 = "";
			IconStickerCtrl.Type type = this.type;
			if (type != IconStickerCtrl.Type.CARD)
			{
				if (type == IconStickerCtrl.Type.ICON)
				{
					text2 = "Texture2D/Frame_Sticker/sticker_frame_0" + ssd.rarity.ToString();
					text = "Texture2D/Frame_ItemIcon/stickericon_frame0" + ssd.rarity.ToString();
				}
			}
			else
			{
				text = "Texture2D/Frame_Sticker/sticker_frame_big_0" + ssd.rarity.ToString();
			}
			if (this.textureFrame != null)
			{
				this.textureFrame.SetRawImage(text, true, false, null);
			}
			if (this.baseFrame != null)
			{
				this.baseFrame.SetRawImage(text2, true, false, null);
			}
			if (this.type == IconStickerCtrl.Type.CARD && this.bgTexture != null)
			{
				this.bgTexture.gameObject.SetActive(!isBig);
				if (isBig)
				{
					base.transform.parent.parent.GetComponent<PguiRawImageCtrl>().SetRawImage(ssd.GetBgTextureName() + "_wide", true, false, null);
					return;
				}
				this.bgTexture.SetRawImage(ssd.GetBgTextureName(), true, false, null);
				return;
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	public void DispStickerChange(bool isBig = false)
	{
		if (this.itemTexture != null && this.stickerPackData.staticData != null)
		{
			if (this.holoAe != null)
			{
				this.holoAe.gameObject.SetActive(this.stickerPackData.staticData.rarity == 5);
			}
			string text = "";
			IconStickerCtrl.Type type = this.type;
			if (type != IconStickerCtrl.Type.CARD)
			{
				if (type == IconStickerCtrl.Type.ICON)
				{
					text = this.stickerPackData.staticData.GetIconName();
				}
			}
			else
			{
				text = this.stickerPackData.staticData.GetCardImageName();
			}
			this.itemTexture.SetRawImage(text, true, false, null);
		}
	}

	public void DispImgDisable(bool flag)
	{
		if (this.imgDisable != null)
		{
			this.imgDisable.gameObject.SetActive(flag);
		}
		if (this.numText != null)
		{
			this.numText.gameObject.SetActive(!flag);
		}
	}

	public void DisableImg()
	{
		if (this.imgDisable != null)
		{
			this.imgDisable.gameObject.SetActive(false);
		}
		if (this.numText != null)
		{
			this.numText.gameObject.SetActive(false);
		}
	}

	public void AddOnClickListener(IconStickerCtrl.OnClick callback)
	{
		this.callbackCL = callback;
	}

	public void OnPointerClick()
	{
		if (this.callbackCL != null)
		{
			IconStickerCtrl.OnClick onClick = this.callbackCL;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}
	}

	public void AddOnLongClickListener(IconStickerCtrl.OnLongClick callback)
	{
		this.callbackLCL = callback;
	}

	public void OnLongPress()
	{
		if (this.callbackLCL != null)
		{
			this.callbackLCL(this);
		}
	}

	public void AddOnLongClickEndListener(IconStickerCtrl.OnReleaseClick callback)
	{
		this.callbackRL = callback;
	}

	public void OnLongPressEnd()
	{
		if (this.callbackRL != null)
		{
			this.callbackRL(this);
		}
	}

	[SerializeField]
	private IconStickerCtrl.Type type;

	[SerializeField]
	private PguiRawImageCtrl itemTexture;

	[SerializeField]
	private PguiTextCtrl numText;

	[SerializeField]
	private PguiRawImageCtrl textureFrame;

	[SerializeField]
	private PguiRawImageCtrl baseFrame;

	[SerializeField]
	private PguiImageCtrl imgDisable;

	[SerializeField]
	private PguiTextCtrl textCardName;

	[SerializeField]
	private PguiRawImageCtrl bgTexture;

	[SerializeField]
	private PguiAECtrl holoAe;

	private IconStickerCtrl.OnClick callbackCL;

	private IconStickerCtrl.OnLongClick callbackLCL;

	private IconStickerCtrl.OnReleaseClick callbackRL;

	private Transform touchPanel;

	private IconStickerCtrl.SetupParam setupParam = new IconStickerCtrl.SetupParam();

	public enum Type
	{
		CARD,
		ICON
	}

	public class SetupParam
	{
		public SetupParam()
		{
			this.spd = null;
			this.useInfo = false;
			this.useFrame = true;
			this.enableIconScale = true;
			this.forceSetup = false;
		}

		public DataManagerSticker.StickerPackData spd;

		public bool useInfo;

		public bool useFrame;

		public bool enableIconScale;

		public bool forceSetup;
	}

	public delegate void OnClick(IconStickerCtrl iic);

	public delegate void OnLongClick(IconStickerCtrl Ac);

	public delegate void OnReleaseClick(IconStickerCtrl Ac);
}
