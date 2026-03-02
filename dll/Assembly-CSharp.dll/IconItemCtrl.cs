using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A6 RID: 422
public class IconItemCtrl : MonoBehaviour
{
	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x06001C48 RID: 7240 RVA: 0x00165ED9 File Offset: 0x001640D9
	// (set) Token: 0x06001C49 RID: 7241 RVA: 0x00165EE1 File Offset: 0x001640E1
	public ItemStaticBase itemStaticBase { get; private set; }

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x06001C4A RID: 7242 RVA: 0x00165EEA File Offset: 0x001640EA
	// (set) Token: 0x06001C4B RID: 7243 RVA: 0x00165EF2 File Offset: 0x001640F2
	public int dispNum { get; private set; }

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x06001C4C RID: 7244 RVA: 0x00165EFB File Offset: 0x001640FB
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

	// Token: 0x06001C4D RID: 7245 RVA: 0x00165F2C File Offset: 0x0016412C
	public void Clear()
	{
		this.Setup(null);
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x00165F35 File Offset: 0x00164135
	public void initialize()
	{
		this.itemStaticBase = null;
		this.dispNum = 0;
		this.TouchPanel.gameObject.SetActive(true);
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x00165F58 File Offset: 0x00164158
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
	}

	// Token: 0x06001C50 RID: 7248 RVA: 0x00165FC4 File Offset: 0x001641C4
	public void Setup(ItemStaticBase isb)
	{
		IconItemCtrl.SetupParam setupParam = new IconItemCtrl.SetupParam();
		this.SetupInternal(isb, -1, setupParam.useInfo, setupParam.useFrame, setupParam.enableIconScale, setupParam.isCampaign, setupParam.useMaxDetail, setupParam.viewItemCount, setupParam.gentei, setupParam.photoLottery, setupParam.photoDrop, setupParam.noPhotoEffect, setupParam.isDispChangePhoto, setupParam.forceSetup);
	}

	// Token: 0x06001C51 RID: 7249 RVA: 0x00166028 File Offset: 0x00164228
	public void Setup(ItemStaticBase isb, int num)
	{
		IconItemCtrl.SetupParam setupParam = new IconItemCtrl.SetupParam();
		this.SetupInternal(isb, num, setupParam.useInfo, setupParam.useFrame, setupParam.enableIconScale, setupParam.isCampaign, setupParam.useMaxDetail, setupParam.viewItemCount, setupParam.gentei, setupParam.photoLottery, setupParam.photoDrop, setupParam.noPhotoEffect, setupParam.isDispChangePhoto, setupParam.forceSetup);
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x0016608C File Offset: 0x0016428C
	public void Setup(ItemStaticBase isb, IconItemCtrl.SetupParam setupParam)
	{
		this.SetupInternal(isb, -1, setupParam.useInfo, setupParam.useFrame, setupParam.enableIconScale, setupParam.isCampaign, setupParam.useMaxDetail, setupParam.viewItemCount, setupParam.gentei, setupParam.photoLottery, setupParam.photoDrop, setupParam.noPhotoEffect, setupParam.isDispChangePhoto, setupParam.forceSetup);
	}

	// Token: 0x06001C53 RID: 7251 RVA: 0x001660EC File Offset: 0x001642EC
	public void Setup(ItemStaticBase isb, int num, IconItemCtrl.SetupParam setupParam)
	{
		this.SetupInternal(isb, num, setupParam.useInfo, setupParam.useFrame, setupParam.enableIconScale, setupParam.isCampaign, setupParam.useMaxDetail, setupParam.viewItemCount, setupParam.gentei, setupParam.photoLottery, setupParam.photoDrop, setupParam.noPhotoEffect, setupParam.isDispChangePhoto, setupParam.forceSetup);
	}

	// Token: 0x06001C54 RID: 7252 RVA: 0x0016614C File Offset: 0x0016434C
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
		if (kind <= ItemDef.Kind.RANK_UP)
		{
			if (kind == ItemDef.Kind.CHARA)
			{
				this.numText.gameObject.SetActive(false);
				this.itemTexture.gameObject.SetActive(false);
				this.rareFrame.gameObject.SetActive(false);
				this.TouchPanel.gameObject.SetActive(false);
				if (this.iconChara == null)
				{
					Transform transform = base.transform.Find("Icon_Other");
					if (!enableIconScale)
					{
						transform.localScale = new Vector3(1f, 1f, 1f);
					}
					GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, transform);
					this.iconChara = gameObject.GetComponent<IconCharaCtrl>();
				}
				this.iconChara.gameObject.SetActive(true);
				CharaPackData charaPackData = CharaPackData.MakeInitial(DataManager.DmItem.ItemId2ChraId(isb.GetId()));
				this.iconChara.Setup(charaPackData, SortFilterDefine.SortType.INVALID, false, useMaxDetail ? new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.DISPLAY, null) : null, 0, -1, 0);
				this.iconChara.DispRarity(false);
				goto IL_098E;
			}
			if (kind == ItemDef.Kind.PHOTO)
			{
				this.itemTexture.gameObject.SetActive(false);
				this.rareFrame.gameObject.SetActive(false);
				this.TouchPanel.gameObject.SetActive(false);
				if (this.iconPhoto == null)
				{
					Transform transform2 = base.transform.Find("Icon_Other");
					if (!enableIconScale)
					{
						transform2.localScale = new Vector3(1f, 1f, 1f);
					}
					GameObject gameObject2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo, transform2);
					this.iconPhoto = gameObject2.GetComponent<IconPhotoCtrl>();
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
				goto IL_098E;
			}
			if (kind == ItemDef.Kind.RANK_UP)
			{
				CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(isb.GetId()));
				this.charaIcon02.transform.parent.gameObject.SetActive(true);
				if (charaStaticData != null)
				{
					this.charaIcon02.SetRawImage(charaStaticData.GetMiniIconName(), true, false, null);
				}
				this.itemTexture.SetRawImage("Texture2D/Icon_Item/icon_item_gembase02", true, false, null);
				goto IL_098E;
			}
		}
		else
		{
			if (kind > ItemDef.Kind.FURNITURE)
			{
				if (kind != ItemDef.Kind.CLOTHES)
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
							Transform transform3 = base.transform.Find("Icon_Other");
							if (!enableIconScale)
							{
								transform3.localScale = new Vector3(1f, 1f, 1f);
							}
							GameObject gameObject3 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Accessory, transform3);
							this.iconAccessory = gameObject3.GetComponent<IconAccessoryCtrl>();
						}
						this.iconAccessory.gameObject.SetActive(true);
						DataManagerCharaAccessory.AccessoryData accessoryData = DataManager.DmChAccessory.GetAccessoryData(isb.GetId());
						this.iconAccessory.SetupByAccessoryData(accessoryData, true, useMaxDetail);
						this.iconAccessory.DispRarity(false);
						goto IL_098E;
					}
					case ItemDef.Kind.LOTTERY_ITEM:
					case ItemDef.Kind.KEMOBOARD:
					case ItemDef.Kind.PICNIC_PLAYITEM:
						goto IL_097A;
					case ItemDef.Kind.CHARA_CONTACT:
						break;
					case ItemDef.Kind.PROMOTE_EXT:
					{
						CharaStaticData charaStaticData2 = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(isb.GetId()));
						this.charaIcon03.transform.parent.gameObject.SetActive(true);
						if (charaStaticData2 != null)
						{
							this.charaIcon03.SetRawImage(charaStaticData2.GetMiniIconName(), true, false, null);
						}
						this.itemTexture.SetRawImage("Texture2D/Icon_Item/icon_item_gembase03", true, false, null);
						goto IL_098E;
					}
					case ItemDef.Kind.TREEHOUSE_FURNITURE:
					{
						TreeHouseFurnitureStatic treeHouseFurnitureStatic = isb as TreeHouseFurnitureStatic;
						CharaStaticData charaStaticData3 = ((treeHouseFurnitureStatic.iconCharaId <= 0) ? null : DataManager.DmChara.GetCharaStaticData(treeHouseFurnitureStatic.iconCharaId));
						this.charaIconDress.gameObject.SetActive(charaStaticData3 != null);
						if (charaStaticData3 != null)
						{
							this.charaIconDress.SetRawImage(charaStaticData3.GetMiniIconName(), true, false, null);
						}
						this.itemTexture.SetRawImage(isb.GetIconName(), true, false, null);
						goto IL_098E;
					}
					default:
						goto IL_097A;
					}
				}
				CharaStaticData charaStaticData4 = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(isb.GetId()));
				this.charaIconDress.gameObject.SetActive(charaStaticData4 != null);
				if (charaStaticData4 != null)
				{
					this.charaIconDress.SetRawImage(charaStaticData4.GetMiniIconName(), true, false, null);
				}
				this.itemTexture.SetRawImage(isb.GetIconName(), true, false, null);
				goto IL_098E;
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
				goto IL_098E;
			}
			if (kind == ItemDef.Kind.FURNITURE)
			{
				this.SetFurnitureIcon(isb as HomeFurnitureStatic);
				goto IL_098E;
			}
		}
		IL_097A:
		this.itemTexture.SetRawImage(isb.GetIconName(), true, false, null);
		IL_098E:
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

	// Token: 0x06001C55 RID: 7253 RVA: 0x00166B18 File Offset: 0x00164D18
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

	// Token: 0x06001C56 RID: 7254 RVA: 0x00166BA0 File Offset: 0x00164DA0
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

	// Token: 0x06001C57 RID: 7255 RVA: 0x00166C64 File Offset: 0x00164E64
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

	// Token: 0x06001C58 RID: 7256 RVA: 0x00166D28 File Offset: 0x00164F28
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

	// Token: 0x06001C59 RID: 7257 RVA: 0x00166F7C File Offset: 0x0016517C
	public void SetRaycastTargetIconItem(bool enabled)
	{
		Transform transform = this.TouchPanel;
		Image image = ((transform != null) ? transform.GetComponent<Image>() : null);
		if (null != image)
		{
			image.raycastTarget = enabled;
		}
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x00166FAC File Offset: 0x001651AC
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

	// Token: 0x06001C5B RID: 7259 RVA: 0x00166FDD File Offset: 0x001651DD
	private void OpenInfo(bool viewItem)
	{
		CanvasManager.HdlItemDetailCtrl.Open(this, viewItem);
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x00166FEB File Offset: 0x001651EB
	private void CloseInfo()
	{
		CanvasManager.HdlItemDetailCtrl.Close();
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x00166FF7 File Offset: 0x001651F7
	public void AddOnClickListener(IconItemCtrl.OnClick callback)
	{
		this.callbackCL = callback;
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x00167000 File Offset: 0x00165200
	public void OnPointerClick()
	{
		if (this.callbackCL != null)
		{
			this.callbackCL(this);
		}
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x00167016 File Offset: 0x00165216
	public void AddOnLongClickListener(IconItemCtrl.OnLongClick callback)
	{
		this.callbackLCL = callback;
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x0016701F File Offset: 0x0016521F
	public void OnLongPress()
	{
		if (this.callbackLCL != null)
		{
			this.callbackLCL(this);
		}
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x00167035 File Offset: 0x00165235
	public void AddOnLongClickEndListener(IconItemCtrl.OnReleaseClick callback)
	{
		this.callbackRL = callback;
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x0016703E File Offset: 0x0016523E
	public void OnLongPressEnd()
	{
		if (this.callbackRL != null)
		{
			this.callbackRL(this);
		}
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x00167054 File Offset: 0x00165254
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

	// Token: 0x06001C64 RID: 7268 RVA: 0x0016715D File Offset: 0x0016535D
	public IconCharaCtrl GetIconCharaCtrl()
	{
		return this.iconChara;
	}

	// Token: 0x04001517 RID: 5399
	[SerializeField]
	private PguiRawImageCtrl itemTexture;

	// Token: 0x04001518 RID: 5400
	[SerializeField]
	private PguiRawImageCtrl charaIcon01;

	// Token: 0x04001519 RID: 5401
	[SerializeField]
	private PguiRawImageCtrl charaIcon02;

	// Token: 0x0400151A RID: 5402
	[SerializeField]
	private PguiRawImageCtrl charaIcon03;

	// Token: 0x0400151B RID: 5403
	[SerializeField]
	private PguiRawImageCtrl charaIconDress;

	// Token: 0x0400151C RID: 5404
	[SerializeField]
	private PguiRawImageCtrl cover01;

	// Token: 0x0400151D RID: 5405
	[SerializeField]
	private PguiRawImageCtrl cover02;

	// Token: 0x0400151E RID: 5406
	[SerializeField]
	private PguiRawImageCtrl cover03;

	// Token: 0x0400151F RID: 5407
	[SerializeField]
	private PguiTextCtrl numText;

	// Token: 0x04001520 RID: 5408
	[SerializeField]
	private PguiRawImageCtrl rareFrame;

	// Token: 0x04001521 RID: 5409
	[SerializeField]
	private PguiImageCtrl markGentei;

	// Token: 0x04001522 RID: 5410
	[SerializeField]
	private PguiImageCtrl markEvent;

	// Token: 0x04001523 RID: 5411
	[SerializeField]
	private PguiImageCtrl markCondition;

	// Token: 0x04001524 RID: 5412
	[SerializeField]
	private PguiAECtrl markNew;

	// Token: 0x04001525 RID: 5413
	private static readonly Color DisableColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x04001526 RID: 5414
	private IconItemCtrl.OnClick callbackCL;

	// Token: 0x04001527 RID: 5415
	private IconItemCtrl.OnLongClick callbackLCL;

	// Token: 0x04001528 RID: 5416
	private IconItemCtrl.OnReleaseClick callbackRL;

	// Token: 0x04001529 RID: 5417
	private PguiImageCtrl ItemInfo;

	// Token: 0x0400152A RID: 5418
	private IconCharaCtrl iconChara;

	// Token: 0x0400152B RID: 5419
	private IconPhotoCtrl iconPhoto;

	// Token: 0x0400152C RID: 5420
	private IconAccessoryCtrl iconAccessory;

	// Token: 0x0400152F RID: 5423
	private Transform touchPanel;

	// Token: 0x02000F06 RID: 3846
	// (Invoke) Token: 0x06004E6F RID: 20079
	public delegate void OnClick(IconItemCtrl iic);

	// Token: 0x02000F07 RID: 3847
	// (Invoke) Token: 0x06004E73 RID: 20083
	public delegate void OnLongClick(IconItemCtrl iic);

	// Token: 0x02000F08 RID: 3848
	// (Invoke) Token: 0x06004E77 RID: 20087
	public delegate void OnReleaseClick(IconItemCtrl iic);

	// Token: 0x02000F09 RID: 3849
	public class SetupParam
	{
		// Token: 0x06004E7A RID: 20090 RVA: 0x00236C08 File Offset: 0x00234E08
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

		// Token: 0x0400559A RID: 21914
		public bool useInfo;

		// Token: 0x0400559B RID: 21915
		public bool useFrame;

		// Token: 0x0400559C RID: 21916
		public bool enableIconScale;

		// Token: 0x0400559D RID: 21917
		public bool isCampaign;

		// Token: 0x0400559E RID: 21918
		public bool useMaxDetail;

		// Token: 0x0400559F RID: 21919
		public bool viewItemCount;

		// Token: 0x040055A0 RID: 21920
		public bool gentei;

		// Token: 0x040055A1 RID: 21921
		public List<int> photoLottery;

		// Token: 0x040055A2 RID: 21922
		public bool photoDrop;

		// Token: 0x040055A3 RID: 21923
		public bool noPhotoEffect;

		// Token: 0x040055A4 RID: 21924
		public bool isDispChangePhoto;

		// Token: 0x040055A5 RID: 21925
		public bool forceSetup;
	}
}
