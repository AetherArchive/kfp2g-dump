using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;
using UnityEngine.UI;

public class IconItemCtrl : MonoBehaviour
{
	public ItemStaticBase itemStaticBase { get; private set; }

	public int dispNum { get; private set; }

	private Transform TouchPanel
	{
		get
		{
			if (null == this.touchPanel)
			{
				this.touchPanel = base.gameObject.transform.Find("TouchPanel");
			}
			return this.touchPanel;
		}
	}

	public void Clear()
	{
		this.Setup(null);
	}

	public void initialize()
	{
		this.itemStaticBase = null;
		this.dispNum = 0;
		this.TouchPanel.gameObject.SetActive(true);
	}

	public void Destroy()
	{
		this.initialize();
		if (this.iconChara)
		{
			Object.Destroy(this.iconChara.gameObject);
		}
		if (this.iconPhoto)
		{
			Object.Destroy(this.iconPhoto.gameObject);
		}
		if (this.iconAccessory)
		{
			Object.Destroy(this.iconAccessory.gameObject);
		}
		if (this.iconSticker)
		{
			Object.Destroy(this.iconSticker.gameObject);
		}
	}

	public void Setup(ItemStaticBase isb)
	{
		IconItemCtrl.SetupParam setupParam = new IconItemCtrl.SetupParam();
		this.SetupInternal(isb, -1, setupParam.useInfo, setupParam.useFrame, setupParam.enableIconScale, setupParam.isCampaign, setupParam.useMaxDetail, setupParam.viewItemCount, setupParam.gentei, setupParam.photoLottery, setupParam.photoDrop, setupParam.noPhotoEffect, setupParam.isDispChangePhoto, setupParam.forceSetup);
	}

	public void Setup(ItemStaticBase isb, int num)
	{
		IconItemCtrl.SetupParam setupParam = new IconItemCtrl.SetupParam();
		this.SetupInternal(isb, num, setupParam.useInfo, setupParam.useFrame, setupParam.enableIconScale, setupParam.isCampaign, setupParam.useMaxDetail, setupParam.viewItemCount, setupParam.gentei, setupParam.photoLottery, setupParam.photoDrop, setupParam.noPhotoEffect, setupParam.isDispChangePhoto, setupParam.forceSetup);
	}

	public void Setup(ItemStaticBase isb, IconItemCtrl.SetupParam setupParam)
	{
		this.SetupInternal(isb, -1, setupParam.useInfo, setupParam.useFrame, setupParam.enableIconScale, setupParam.isCampaign, setupParam.useMaxDetail, setupParam.viewItemCount, setupParam.gentei, setupParam.photoLottery, setupParam.photoDrop, setupParam.noPhotoEffect, setupParam.isDispChangePhoto, setupParam.forceSetup);
	}

	public void Setup(ItemStaticBase isb, int num, IconItemCtrl.SetupParam setupParam)
	{
		this.SetupInternal(isb, num, setupParam.useInfo, setupParam.useFrame, setupParam.enableIconScale, setupParam.isCampaign, setupParam.useMaxDetail, setupParam.viewItemCount, setupParam.gentei, setupParam.photoLottery, setupParam.photoDrop, setupParam.noPhotoEffect, setupParam.isDispChangePhoto, setupParam.forceSetup);
	}

	private void SetupInternal(ItemStaticBase isb, int num, bool useInfo, bool useFrame, bool enableIconScale, bool isCampaign, bool useMaxDetail, bool viewItemCount, bool gentei, List<int> photoLottery, bool photoDrop, bool noPhotoEffect, bool isDispChangePhoto, bool forceSetup)
	{
		base.gameObject.SetActive(isb != null);
		if (this.markNew)
		{
			this.markNew.gameObject.SetActive(false);
		}
		if (isb == null || (!forceSetup && this.itemStaticBase == isb && this.dispNum == num))
		{
			return;
		}
		this.itemTexture.gameObject.SetActive(true);
		this.itemTexture.transform.localScale = Vector3.one;
		this.rareFrame.gameObject.SetActive(true);
		this.charaIcon01.transform.parent.gameObject.SetActive(false);
		this.charaIcon02.transform.parent.gameObject.SetActive(false);
		this.charaIcon03.transform.parent.gameObject.SetActive(false);
		this.charaIconDress.gameObject.SetActive(false);
		if (this.cover01 != null)
		{
			this.cover01.gameObject.SetActive(false);
		}
		if (num > 0)
		{
			this.numText.gameObject.SetActive(true);
			this.numText.text = num.ToString();
			this.numText.m_Text.color = this.numText.GetComponent<PguiColorCtrl>().GetGameObjectById(isCampaign ? "CAMPAIGN" : "NORMAL");
		}
		else
		{
			this.numText.gameObject.SetActive(false);
		}
		if (this.iconChara)
		{
			this.iconChara.gameObject.SetActive(false);
		}
		if (this.iconPhoto)
		{
			this.iconPhoto.gameObject.SetActive(false);
		}
		if (this.iconAccessory)
		{
			this.iconAccessory.gameObject.SetActive(false);
		}
		if (this.iconSticker)
		{
			this.iconSticker.gameObject.SetActive(false);
		}
		this.dispNum = num;
		switch (isb.GetRarity())
		{
		case ItemDef.Rarity.STAR1:
			this.rareFrame.SetRawImage("Texture2D/Frame_ItemIcon/itemicon_frame01", true, false, null);
			break;
		case ItemDef.Rarity.STAR2:
			this.rareFrame.SetRawImage("Texture2D/Frame_ItemIcon/itemicon_frame02", true, false, null);
			break;
		case ItemDef.Rarity.STAR3:
			this.rareFrame.SetRawImage("Texture2D/Frame_ItemIcon/itemicon_frame03", true, false, null);
			break;
		case ItemDef.Rarity.STAR4:
			this.rareFrame.SetRawImage("Texture2D/Frame_ItemIcon/itemicon_frame04", true, false, null);
			break;
		case ItemDef.Rarity.STAR5:
			this.rareFrame.SetRawImage("Texture2D/Frame_ItemIcon/itemicon_frame05", true, false, null);
			break;
		}
		this.rareFrame.gameObject.SetActive(useFrame);
		if (null != this.markGentei)
		{
			this.markGentei.gameObject.SetActive(gentei);
		}
		if (null != this.markEvent)
		{
			this.markEvent.gameObject.SetActive(photoDrop);
		}
		if (null != this.markCondition)
		{
			this.markCondition.gameObject.SetActive(photoLottery != null && photoLottery.Count > 0);
			if (this.markCondition.gameObject.activeSelf)
			{
				this.AddOnClickListener(delegate(IconItemCtrl iic)
				{
					this.OpenBonusPhotoInfo(photoLottery);
				});
			}
		}
		if (null != this.TouchPanel)
		{
			this.TouchPanel.gameObject.SetActive(true);
		}
		ItemDef.Kind kind = isb.GetKind();
		if (kind > ItemDef.Kind.PHOTO_FRAME_UP)
		{
			if (kind <= ItemDef.Kind.CLOTHES)
			{
				if (kind == ItemDef.Kind.FURNITURE)
				{
					this.SetFurnitureIcon(isb as HomeFurnitureStatic);
					goto IL_0B24;
				}
				if (kind != ItemDef.Kind.CLOTHES)
				{
					goto IL_0B10;
				}
			}
			else
			{
				switch (kind)
				{
				case ItemDef.Kind.ACCESSORY_ITEM:
				{
					this.itemTexture.gameObject.SetActive(false);
					this.rareFrame.gameObject.SetActive(false);
					this.TouchPanel.gameObject.SetActive(false);
					if (this.iconAccessory == null)
					{
						Transform transform = base.transform.Find("Icon_Other");
						if (!enableIconScale)
						{
							transform.localScale = new Vector3(1f, 1f, 1f);
						}
						GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Accessory, transform);
						this.iconAccessory = gameObject.GetComponent<IconAccessoryCtrl>();
					}
					this.iconAccessory.gameObject.SetActive(true);
					DataManagerCharaAccessory.AccessoryData accessoryData = DataManager.DmChAccessory.GetAccessoryData(isb.GetId());
					this.iconAccessory.SetupByAccessoryData(accessoryData, true, useMaxDetail);
					this.iconAccessory.DispRarity(false);
					goto IL_0B24;
				}
				case ItemDef.Kind.LOTTERY_ITEM:
				case ItemDef.Kind.KEMOBOARD:
				case ItemDef.Kind.PICNIC_PLAYITEM:
					goto IL_0B10;
				case ItemDef.Kind.CHARA_CONTACT:
					break;
				case ItemDef.Kind.PROMOTE_EXT:
				{
					CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(isb.GetId()));
					this.charaIcon03.transform.parent.gameObject.SetActive(true);
					if (charaStaticData != null)
					{
						this.charaIcon03.SetRawImage(charaStaticData.GetMiniIconName(), true, false, null);
					}
					this.itemTexture.SetRawImage("Texture2D/Icon_Item/icon_item_gembase03", true, false, null);
					goto IL_0B24;
				}
				case ItemDef.Kind.TREEHOUSE_FURNITURE:
				{
					TreeHouseFurnitureStatic treeHouseFurnitureStatic = isb as TreeHouseFurnitureStatic;
					CharaStaticData charaStaticData2 = ((treeHouseFurnitureStatic.iconCharaId <= 0) ? null : DataManager.DmChara.GetCharaStaticData(treeHouseFurnitureStatic.iconCharaId));
					this.charaIconDress.gameObject.SetActive(charaStaticData2 != null);
					if (charaStaticData2 != null)
					{
						this.charaIconDress.SetRawImage(charaStaticData2.GetMiniIconName(), true, false, null);
					}
					this.itemTexture.SetRawImage(isb.GetIconName(), true, false, null);
					goto IL_0B24;
				}
				default:
				{
					if (kind != ItemDef.Kind.STICKER)
					{
						goto IL_0B10;
					}
					if (this.itemTexture != null)
					{
						this.itemTexture.gameObject.SetActive(false);
					}
					if (this.rareFrame != null)
					{
						this.rareFrame.gameObject.SetActive(false);
					}
					if (this.TouchPanel != null)
					{
						this.TouchPanel.gameObject.SetActive(false);
					}
					if (this.iconSticker == null)
					{
						Transform transform2 = base.transform.Find("Icon_Other");
						if (!enableIconScale)
						{
							transform2.localScale = new Vector3(1f, 1f, 1f);
						}
						GameObject gameObject2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Sticker, transform2);
						this.iconSticker = gameObject2.GetComponent<IconStickerCtrl>();
					}
					this.iconSticker.gameObject.SetActive(true);
					DataManagerSticker.StickerStaticData stickerStaticData = DataManager.DmSticker.GetStickerStaticData(isb.GetId());
					DataManagerSticker.StickerPackData stickerPackData = new DataManagerSticker.StickerPackData(new DataManagerSticker.StickerDynamicData
					{
						id = stickerStaticData.GetId()
					});
					this.iconSticker.Setup(new IconStickerCtrl.SetupParam
					{
						spd = stickerPackData
					});
					this.iconSticker.DispStickerChange(false);
					this.iconSticker.DisableImg();
					if (useInfo)
					{
						this.iconSticker.AddOnLongClickListener(delegate(IconStickerCtrl x)
						{
							this.OpenInfo(viewItemCount);
						});
						this.iconSticker.AddOnLongClickEndListener(delegate(IconStickerCtrl x)
						{
							this.CloseInfo();
						});
						goto IL_0B24;
					}
					goto IL_0B24;
				}
				}
			}
			CharaStaticData charaStaticData3 = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(isb.GetId()));
			this.charaIconDress.gameObject.SetActive(charaStaticData3 != null);
			if (charaStaticData3 != null)
			{
				this.charaIconDress.SetRawImage(charaStaticData3.GetMiniIconName(), true, false, null);
			}
			this.itemTexture.SetRawImage(isb.GetIconName(), true, false, null);
			goto IL_0B24;
		}
		if (kind <= ItemDef.Kind.PHOTO)
		{
			if (kind == ItemDef.Kind.CHARA)
			{
				this.numText.gameObject.SetActive(false);
				this.itemTexture.gameObject.SetActive(false);
				this.rareFrame.gameObject.SetActive(false);
				this.TouchPanel.gameObject.SetActive(false);
				if (this.iconChara == null)
				{
					Transform transform3 = base.transform.Find("Icon_Other");
					if (!enableIconScale)
					{
						transform3.localScale = new Vector3(1f, 1f, 1f);
					}
					GameObject gameObject3 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, transform3);
					this.iconChara = gameObject3.GetComponent<IconCharaCtrl>();
				}
				this.iconChara.gameObject.SetActive(true);
				CharaPackData charaPackData = CharaPackData.MakeInitial(DataManager.DmItem.ItemId2ChraId(isb.GetId()));
				this.iconChara.Setup(charaPackData, SortFilterDefine.SortType.INVALID, false, useMaxDetail ? new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.DISPLAY, null) : null, 0, -1, 0);
				this.iconChara.DispRarity(false);
				goto IL_0B24;
			}
			if (kind == ItemDef.Kind.PHOTO)
			{
				this.itemTexture.gameObject.SetActive(false);
				this.rareFrame.gameObject.SetActive(false);
				this.TouchPanel.gameObject.SetActive(false);
				if (this.iconPhoto == null)
				{
					Transform transform4 = base.transform.Find("Icon_Other");
					if (!enableIconScale)
					{
						transform4.localScale = new Vector3(1f, 1f, 1f);
					}
					GameObject gameObject4 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo, transform4);
					this.iconPhoto = gameObject4.GetComponent<IconPhotoCtrl>();
					this.iconPhoto.transform.Find("AEImage_Eff_Change").gameObject.SetActive(false);
					this.iconPhoto.transform.Find("All").gameObject.GetComponent<AELayerConstraint>().enabled = false;
					RectTransform rectTransform = this.iconPhoto.transform.Find("All").transform as RectTransform;
					if ((double)rectTransform.localScale.x <= 0.05)
					{
						rectTransform.localScale = Vector3.one;
					}
				}
				this.iconPhoto.gameObject.SetActive(true);
				if (useMaxDetail)
				{
					PhotoPackData photoPackData = PhotoPackData.MakeMaximum(isb.GetId(), false, true);
					this.iconPhoto.Setup(new IconPhotoCtrl.SetupParam
					{
						ppd = photoPackData,
						dispMax = true
					});
					this.iconPhoto.DispTextParam(false);
				}
				else
				{
					PhotoStaticData photoStaticData = DataManager.DmPhoto.GetPhotoStaticData(isb.GetId());
					this.iconPhoto.Setup(photoStaticData, SortFilterDefine.SortType.LEVEL, true, false, -1, 0, false);
				}
				this.iconPhoto.DispRarity(false);
				if (noPhotoEffect)
				{
					this.iconPhoto.DispDrop(false, 0);
				}
				this.iconPhoto.DispPhotoChange(isDispChangePhoto);
				goto IL_0B24;
			}
		}
		else
		{
			if (kind == ItemDef.Kind.RANK_UP)
			{
				CharaStaticData charaStaticData4 = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(isb.GetId()));
				this.charaIcon02.transform.parent.gameObject.SetActive(true);
				if (charaStaticData4 != null)
				{
					this.charaIcon02.SetRawImage(charaStaticData4.GetMiniIconName(), true, false, null);
				}
				this.itemTexture.SetRawImage("Texture2D/Icon_Item/icon_item_gembase02", true, false, null);
				goto IL_0B24;
			}
			if (kind == ItemDef.Kind.PHOTO_FRAME_UP)
			{
				CharaStaticData charaStaticData5 = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(isb.GetId()));
				this.charaIcon01.transform.parent.gameObject.SetActive(true);
				if (charaStaticData5 != null)
				{
					this.charaIcon01.SetRawImage(charaStaticData5.GetMiniIconName(), true, false, null);
				}
				if (this.cover01 != null)
				{
					this.cover01.gameObject.SetActive(true);
				}
				this.itemTexture.SetRawImage("Texture2D/Icon_Item/icon_item_gembase01", true, false, null);
				goto IL_0B24;
			}
		}
		IL_0B10:
		this.itemTexture.SetRawImage(isb.GetIconName(), true, false, null);
		IL_0B24:
		this.itemStaticBase = isb;
		if (useInfo)
		{
			this.AddOnLongClickListener(delegate(IconItemCtrl x)
			{
				this.OpenInfo(viewItemCount);
			});
			this.AddOnLongClickEndListener(delegate(IconItemCtrl x)
			{
				this.CloseInfo();
			});
		}
	}

	private void SetFurnitureIcon(HomeFurnitureStatic furn)
	{
		if (string.IsNullOrEmpty(furn.modelFileName))
		{
			this.itemTexture.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			this.itemTexture.SetRawImage(furn.photoTexturePath.Replace("Card_Photo", "Icon_Photo").Replace("card_photo", "icon_photo"), true, false, null);
			return;
		}
		this.itemTexture.SetRawImage(furn.GetIconName(), true, false, null);
	}

	public void Setup(ItemData itemData)
	{
		IconItemCtrl.SetupParam setupParam = new IconItemCtrl.SetupParam();
		if (itemData != null)
		{
			this.SetupInternal(itemData.staticData, itemData.num, setupParam.useInfo, setupParam.useFrame, setupParam.enableIconScale, setupParam.isCampaign, setupParam.useMaxDetail, setupParam.viewItemCount, setupParam.gentei, setupParam.photoLottery, setupParam.photoDrop, setupParam.noPhotoEffect, setupParam.isDispChangePhoto, setupParam.forceSetup);
			return;
		}
		this.SetupInternal(null, -1, setupParam.useInfo, setupParam.useFrame, setupParam.enableIconScale, setupParam.isCampaign, setupParam.useMaxDetail, setupParam.viewItemCount, setupParam.gentei, setupParam.photoLottery, setupParam.photoDrop, setupParam.noPhotoEffect, setupParam.isDispChangePhoto, setupParam.forceSetup);
	}

	public void Setup(ItemData itemData, IconItemCtrl.SetupParam setupParam)
	{
		if (itemData != null)
		{
			this.SetupInternal(itemData.staticData, itemData.num, setupParam.useInfo, setupParam.useFrame, setupParam.enableIconScale, setupParam.isCampaign, setupParam.useMaxDetail, setupParam.viewItemCount, setupParam.gentei, setupParam.photoLottery, setupParam.photoDrop, setupParam.noPhotoEffect, setupParam.isDispChangePhoto, setupParam.forceSetup);
			return;
		}
		IconItemCtrl.SetupParam setupParam2 = new IconItemCtrl.SetupParam();
		this.SetupInternal(null, -1, setupParam2.useInfo, setupParam2.useFrame, setupParam2.enableIconScale, setupParam2.isCampaign, setupParam2.useMaxDetail, setupParam2.viewItemCount, setupParam2.gentei, setupParam2.photoLottery, setupParam2.photoDrop, setupParam2.noPhotoEffect, setupParam2.isDispChangePhoto, setupParam2.forceSetup);
	}

	public void SetActEnable(bool enabled)
	{
		if (this.itemTexture != null)
		{
			this.itemTexture.m_RawImage.color = (enabled ? Color.white : IconItemCtrl.DisableColor);
		}
		if (this.charaIcon01 != null)
		{
			this.charaIcon01.m_RawImage.color = (enabled ? Color.white : IconItemCtrl.DisableColor);
		}
		if (this.charaIcon02 != null)
		{
			this.charaIcon02.m_RawImage.color = (enabled ? Color.white : IconItemCtrl.DisableColor);
		}
		if (this.charaIcon03 != null)
		{
			this.charaIcon03.m_RawImage.color = (enabled ? Color.white : IconItemCtrl.DisableColor);
		}
		if (this.rareFrame != null)
		{
			this.rareFrame.m_RawImage.color = (enabled ? Color.white : IconItemCtrl.DisableColor);
		}
		if (this.cover01 != null)
		{
			this.cover01.m_RawImage.color = (enabled ? Color.white : IconItemCtrl.DisableColor);
		}
		if (this.cover02 != null)
		{
			this.cover02.m_RawImage.color = (enabled ? Color.white : IconItemCtrl.DisableColor);
		}
		if (this.cover03 != null)
		{
			this.cover03.m_RawImage.color = (enabled ? Color.white : IconItemCtrl.DisableColor);
		}
		if (this.numText != null)
		{
			this.numText.textGrayScale = (enabled ? 1f : IconItemCtrl.DisableColor.r);
		}
		if (this.iconChara != null)
		{
			this.iconChara.IsEnableMask(!enabled);
		}
		this.iconPhoto != null;
		if (this.markGentei != null)
		{
			this.markGentei.m_Image.color = (enabled ? Color.white : IconItemCtrl.DisableColor);
		}
		if (this.markEvent != null)
		{
			this.markEvent.m_Image.color = (enabled ? Color.white : IconItemCtrl.DisableColor);
		}
		if (this.markCondition != null)
		{
			this.markCondition.m_Image.color = (enabled ? Color.white : IconItemCtrl.DisableColor);
		}
	}

	public void SetRaycastTargetIconItem(bool enabled)
	{
		Transform transform = this.TouchPanel;
		Image image = ((transform != null) ? transform.GetComponent<Image>() : null);
		if (null != image)
		{
			image.raycastTarget = enabled;
		}
	}

	public void DispNew(bool flag)
	{
		if (this.markNew != null)
		{
			this.markNew.gameObject.SetActive(flag);
			if (flag)
			{
				this.markNew.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			}
		}
	}

	private void OpenInfo(bool viewItem)
	{
		CanvasManager.HdlItemDetailCtrl.Open(this, viewItem);
	}

	private void CloseInfo()
	{
		CanvasManager.HdlItemDetailCtrl.Close();
	}

	public void AddOnClickListener(IconItemCtrl.OnClick callback)
	{
		this.callbackCL = callback;
	}

	public void OnPointerClick()
	{
		if (this.callbackCL != null)
		{
			this.callbackCL(this);
		}
	}

	public void AddOnLongClickListener(IconItemCtrl.OnLongClick callback)
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

	public void AddOnLongClickEndListener(IconItemCtrl.OnReleaseClick callback)
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

	private void OpenBonusPhotoInfo(List<int> photo)
	{
		if (null == this.markCondition || !this.markCondition.gameObject.activeSelf)
		{
			return;
		}
		if (photo == null || photo.Count <= 0)
		{
			return;
		}
		CanvasManager.HdlOpenWindowBonusPhotoInfo.Setup(null, null, null, true, (int x) => true, null, false);
		Transform userInfoContent = CanvasManager.HdlOpenWindowBonusPhotoInfo.m_UserInfoContent;
		int num = 0;
		for (;;)
		{
			Transform transform = userInfoContent.Find("Icon_Photo" + (num + 1).ToString("D2"));
			if (transform == null)
			{
				break;
			}
			PhotoPackData photoPackData = ((num < photo.Count) ? PhotoPackData.MakeMaximum(photo[num], false, true) : null);
			transform.gameObject.SetActive(photoPackData != null);
			IconPhotoCtrl component = transform.Find("Icon_Photo").GetComponent<IconPhotoCtrl>();
			component.Setup(photoPackData, SortFilterDefine.SortType.LEVEL, true, true, -1, false);
			component.DispTextParam(false);
			component.DispDrop(false, 0);
			num++;
		}
		CanvasManager.HdlOpenWindowBonusPhotoInfo.ForceOpen();
	}

	public IconCharaCtrl GetIconCharaCtrl()
	{
		return this.iconChara;
	}

	[SerializeField]
	private PguiRawImageCtrl itemTexture;

	[SerializeField]
	private PguiRawImageCtrl charaIcon01;

	[SerializeField]
	private PguiRawImageCtrl charaIcon02;

	[SerializeField]
	private PguiRawImageCtrl charaIcon03;

	[SerializeField]
	private PguiRawImageCtrl charaIconDress;

	[SerializeField]
	private PguiRawImageCtrl cover01;

	[SerializeField]
	private PguiRawImageCtrl cover02;

	[SerializeField]
	private PguiRawImageCtrl cover03;

	[SerializeField]
	private PguiTextCtrl numText;

	[SerializeField]
	private PguiRawImageCtrl rareFrame;

	[SerializeField]
	private PguiImageCtrl markGentei;

	[SerializeField]
	private PguiImageCtrl markEvent;

	[SerializeField]
	private PguiImageCtrl markCondition;

	[SerializeField]
	private PguiAECtrl markNew;

	private static readonly Color DisableColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	private IconItemCtrl.OnClick callbackCL;

	private IconItemCtrl.OnLongClick callbackLCL;

	private IconItemCtrl.OnReleaseClick callbackRL;

	private PguiImageCtrl ItemInfo;

	private IconCharaCtrl iconChara;

	private IconPhotoCtrl iconPhoto;

	private IconAccessoryCtrl iconAccessory;

	private IconStickerCtrl iconSticker;

	private Transform touchPanel;

	public delegate void OnClick(IconItemCtrl iic);

	public delegate void OnLongClick(IconItemCtrl iic);

	public delegate void OnReleaseClick(IconItemCtrl iic);

	public class SetupParam
	{
		public SetupParam()
		{
			this.useInfo = false;
			this.useFrame = true;
			this.enableIconScale = true;
			this.isCampaign = false;
			this.useMaxDetail = false;
			this.viewItemCount = true;
			this.gentei = false;
			this.photoLottery = null;
			this.photoDrop = false;
			this.noPhotoEffect = false;
			this.isDispChangePhoto = false;
			this.forceSetup = false;
		}

		public bool useInfo;

		public bool useFrame;

		public bool enableIconScale;

		public bool isCampaign;

		public bool useMaxDetail;

		public bool viewItemCount;

		public bool gentei;

		public List<int> photoLottery;

		public bool photoDrop;

		public bool noPhotoEffect;

		public bool isDispChangePhoto;

		public bool forceSetup;
	}
}
