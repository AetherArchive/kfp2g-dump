using System;
using System.Collections.Generic;
using AEAuth3;
using SGNFW.Common;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class IconPhotoCtrl : MonoBehaviour
{
	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06001C67 RID: 7271 RVA: 0x0016718D File Offset: 0x0016538D
	public PguiRawImageCtrl TexPhoto
	{
		get
		{
			return this.texPhoto;
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06001C68 RID: 7272 RVA: 0x00167195 File Offset: 0x00165395
	// (set) Token: 0x06001C69 RID: 7273 RVA: 0x0016719D File Offset: 0x0016539D
	public PhotoPackData photoPackData { get; private set; }

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06001C6A RID: 7274 RVA: 0x001671A6 File Offset: 0x001653A6
	// (set) Token: 0x06001C6B RID: 7275 RVA: 0x001671AE File Offset: 0x001653AE
	public PhotoStaticData photoStaticData { get; private set; }

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06001C6C RID: 7276 RVA: 0x001671B7 File Offset: 0x001653B7
	private bool IsEnableLongPress
	{
		get
		{
			return this.setupParam.isEnableLongPress && this.setupParam.isEnableRaycast;
		}
	}

	// Token: 0x06001C6D RID: 7277 RVA: 0x001671D4 File Offset: 0x001653D4
	private void SetupImgLock()
	{
		if (this.imgLock != null)
		{
			this.imgLock.m_Image.enabled = this.photoPackData != null && this.photoPackData.dynamicData != null && this.photoPackData.dynamicData.lockFlag;
		}
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x00167228 File Offset: 0x00165428
	private void SetupImgFavorite()
	{
		if (this.imgFavorite != null)
		{
			this.imgFavorite.m_Image.enabled = this.photoPackData != null && this.photoPackData.dynamicData != null && this.photoPackData.dynamicData.favoriteFlag;
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06001C6F RID: 7279 RVA: 0x0016727B File Offset: 0x0016547B
	public PguiAECtrl aeEffChange
	{
		get
		{
			return this.changeAe;
		}
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x06001C70 RID: 7280 RVA: 0x00167283 File Offset: 0x00165483
	// (set) Token: 0x06001C71 RID: 7281 RVA: 0x0016728B File Offset: 0x0016548B
	public bool isDispChangePhoto { get; private set; }

	// Token: 0x06001C72 RID: 7282 RVA: 0x00167294 File Offset: 0x00165494
	public void ForceActive()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x06001C73 RID: 7283 RVA: 0x001672A2 File Offset: 0x001654A2
	// (set) Token: 0x06001C74 RID: 7284 RVA: 0x001672AA File Offset: 0x001654AA
	public bool DispMax { get; private set; }

	// Token: 0x06001C75 RID: 7285 RVA: 0x001672B3 File Offset: 0x001654B3
	public void Setup(IconPhotoCtrl.SetupParam param)
	{
		this.setupParam = param;
		this.Setup(param.ppd, param.sortType, param.isEnableRaycast, param.dispMax, param.playQuestOneId, param.isHelper);
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x001672E8 File Offset: 0x001654E8
	public void Setup(PhotoPackData ppd, SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL, bool isEnableRaycast = true, bool dispMax = false, int playQuestOneId = -1, bool isHelper = false)
	{
		this.DispMax = dispMax;
		this.photoPackData = ppd;
		this.DispDrop(false, 0);
		if (ppd != null && !ppd.IsInvalid())
		{
			bool flag = false;
			if ((ppd.IsMine() || (Singleton<DataManager>.Instance != null && DataManager.DmUserInfo.optionData.ViewPhotoAffect)) && ppd.IsImageChange())
			{
				flag = true;
			}
			this.Setup(ppd.staticData, sortType, isEnableRaycast, flag, playQuestOneId, ppd.dynamicData.levelRank, isHelper);
			this.SetupImgLock();
			this.SetupImgFavorite();
			this.DispTextParam(true);
			if (this.textParam != null)
			{
				this.textParam.m_Text.enabled = true;
				PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByPhoto(ppd);
				switch (sortType)
				{
				case SortFilterDefine.SortType.LEVEL:
				case SortFilterDefine.SortType.RARITY:
					this.textParam.text = PrjUtil.MakeMessage("Lv.") + ppd.dynamicData.level.ToString() + PrjUtil.MakeMessage("/") + ppd.staticData.getLimitLevel(ppd.dynamicData.levelRank).ToString();
					break;
				case SortFilterDefine.SortType.HP:
					this.textParam.text = paramPreset.hp.ToString();
					break;
				case SortFilterDefine.SortType.ATK:
					this.textParam.text = paramPreset.atk.ToString();
					break;
				case SortFilterDefine.SortType.DEF:
					this.textParam.text = paramPreset.def.ToString();
					break;
				case SortFilterDefine.SortType.NEW:
					this.textParam.text = ppd.dynamicData.insertTime.ToString("yyyy/MM/dd");
					break;
				case SortFilterDefine.SortType.BREAKTHROUGH_LIMIT:
					this.textParam.m_Text.enabled = false;
					break;
				}
			}
			if (this.imgBreakthroughLimit != null)
			{
				for (int i = 0; i < this.imgBreakthroughLimit.Count; i++)
				{
					GameObject gameObject = this.imgBreakthroughLimit[i];
					gameObject.SetActive(sortType == SortFilterDefine.SortType.BREAKTHROUGH_LIMIT);
					gameObject.transform.Find("Icon_PhotoRebirth_Act").GetComponent<PguiImageCtrl>().gameObject.SetActive(i < ppd.dynamicData.levelRank);
				}
				return;
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x0016751C File Offset: 0x0016571C
	public void Setup(PhotoStaticData psd, SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL, bool isEnableRaycast = true, bool isChangeImage = false, int playQuestOneId = -1, int levelRank = 0, bool isHelper = false)
	{
		this.photoStaticData = psd;
		if (psd != null)
		{
			base.gameObject.SetActive(true);
			if (this.texPhoto != null)
			{
				this.DispPhotoChange(isChangeImage);
				this.texPhoto.m_RawImage.raycastTarget = isEnableRaycast && this.photoPackData != null && !this.photoPackData.IsInvalid();
			}
			if (this.imgAttribute != null)
			{
				if (psd.baseData.type == PhotoDef.Type.PARAMETER)
				{
					this.DispAttribute(true);
					this.imgAttribute.SetImageByName((this.type == IconPhotoCtrl.Type.CARD) ? "icon_atr_parameter" : "icon_photoskill_kind01");
				}
				else if (psd.baseData.type == PhotoDef.Type.ABILITY)
				{
					this.DispAttribute(true);
					this.imgAttribute.SetImageByName((this.type == IconPhotoCtrl.Type.CARD) ? "icon_atr_ability" : "icon_photoskill_kind02");
				}
				else
				{
					this.DispAttribute(false);
				}
			}
			string text = "";
			switch (this.type)
			{
			case IconPhotoCtrl.Type.CARD:
				text = "Texture2D/Frame_CardPhoto/photo_frame_big_0" + ((int)psd.baseData.rarity).ToString();
				break;
			case IconPhotoCtrl.Type.ICON:
				text = "icon_photo_frame0" + ((int)psd.baseData.rarity).ToString();
				break;
			case IconPhotoCtrl.Type.ICON_MINI:
				text = "icon_photomini_frame0" + ((int)psd.baseData.rarity).ToString();
				break;
			}
			if (this.imgRareFrame != null)
			{
				this.imgRareFrame.SetImageByName(text);
			}
			if (this.rawImgRareFrame != null)
			{
				this.rawImgRareFrame.SetRawImage(text, true, false, null);
			}
			if (this.imgStar != null)
			{
				for (int i = 0; i < this.imgStar.Count; i++)
				{
					this.imgStar[i].gameObject.SetActive(i < (int)psd.baseData.rarity);
				}
			}
			if (this.textCardName != null)
			{
				this.textCardName.text = psd.baseData.photoName;
			}
			for (int j = 0; j < this.imgBreakthroughLimit.Count; j++)
			{
				GameObject gameObject = this.imgBreakthroughLimit[j];
				gameObject.transform.Find("Icon_PhotoRebirth_Act").GetComponent<PguiImageCtrl>().gameObject.SetActive(false);
				gameObject.SetActive(false);
			}
			this.DispBase(true);
			this.DispTop(true);
			this.DispIconCharaMini(false, null);
			this.DispMarkNotYetReleased(false);
			this.DispImgDisable(false);
			this.DispParty(false, true);
			this.DispRemove(false);
			this.DispTextDisable(false, null, null);
			this.DispTextParam(false);
			this.DispInfoPop(false, null);
			this.DispUpto(false);
			this.DispLockNotOwn(false);
			this.DispUptoLv(false);
			this.enablePhotoBonus = DataManager.DmPhoto.IsEnablePhotoBonusByTime(psd.GetId(), TimeManager.Now, playQuestOneId);
			DataManagerPhoto.PhotoDropItemData photoDropItemData = null;
			if (playQuestOneId > 0)
			{
				QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(playQuestOneId);
				if (questOnePackData != null)
				{
					photoDropItemData = questOnePackData.questOne.PhotoDropItemList.Find((DataManagerPhoto.PhotoDropItemData itm) => itm.PhotoId == psd.GetId());
				}
			}
			else
			{
				photoDropItemData = DataManager.DmPhoto.GetPhotoQuestDropItemList(TimeManager.Now).Find((DataManagerPhoto.PhotoDropItemData itm) => itm.PhotoId == psd.GetId());
			}
			this.isQuestDrop = ((photoDropItemData == null) ? 0 : ((levelRank >= photoDropItemData.PhotoLimitOverNum && (!isHelper || photoDropItemData.HelperEnabled)) ? 1 : (-1)));
			this.DispDrop(this.enablePhotoBonus, this.isQuestDrop);
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x001678D8 File Offset: 0x00165AD8
	public void DispLockIcon(bool flag)
	{
		if (this.imgLock != null)
		{
			this.imgLock.m_Image.enabled = flag;
		}
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x001678F9 File Offset: 0x00165AF9
	public void DispFavoriteIcon(bool flag)
	{
		if (this.imgFavorite != null)
		{
			this.imgFavorite.m_Image.enabled = flag;
		}
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x0016791A File Offset: 0x00165B1A
	private static string GetPhotoChangeName(string original)
	{
		return original += "_2";
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x0016792C File Offset: 0x00165B2C
	public void DispPhotoChange(bool isChangeByMax)
	{
		this.isDispChangePhoto = isChangeByMax;
		if (this.holoAe != null)
		{
			this.holoAe.gameObject.SetActive(false);
		}
		if (this.texPhoto != null && this.photoStaticData != null)
		{
			string text = "";
			switch (this.type)
			{
			case IconPhotoCtrl.Type.CARD:
				text = this.photoStaticData.GetCardImageName();
				break;
			case IconPhotoCtrl.Type.ICON:
				text = this.photoStaticData.GetIconName();
				break;
			case IconPhotoCtrl.Type.ICON_MINI:
				text = this.photoStaticData.GetMiniIconName();
				break;
			}
			if (isChangeByMax)
			{
				text = IconPhotoCtrl.GetPhotoChangeName(text);
				if (this.holoAe != null)
				{
					this.holoAe.gameObject.SetActive(true);
					this.holoAe.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				}
			}
			this.texPhoto.SetRawImage(text, true, false, null);
		}
	}

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x06001C7D RID: 7293 RVA: 0x00167A58 File Offset: 0x00165C58
	// (set) Token: 0x06001C7C RID: 7292 RVA: 0x00167A0C File Offset: 0x00165C0C
	public bool FrameDisp
	{
		get
		{
			if (this.imgRareFrame != null)
			{
				return this.imgRareFrame.gameObject.activeSelf;
			}
			return this.rawImgRareFrame != null && this.rawImgRareFrame.gameObject.activeSelf;
		}
		set
		{
			if (this.imgRareFrame != null)
			{
				this.imgRareFrame.gameObject.SetActive(value);
				return;
			}
			if (this.rawImgRareFrame != null)
			{
				this.rawImgRareFrame.gameObject.SetActive(value);
			}
		}
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x00167AA4 File Offset: 0x00165CA4
	public void DispTop(bool flag)
	{
		if (this.Top != null)
		{
			this.Top.SetActive(flag);
		}
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x00167AC0 File Offset: 0x00165CC0
	public void DispOver(bool flag)
	{
		if (this.Over != null)
		{
			this.Over.SetActive(flag);
		}
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x00167ADC File Offset: 0x00165CDC
	public void DispBase(bool flag)
	{
		if (this.Base != null)
		{
			this.Base.SetActive(flag);
		}
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x00167AF8 File Offset: 0x00165CF8
	public void DispRemove(bool flag)
	{
		if (this.removeObj != null)
		{
			if (flag)
			{
				base.gameObject.SetActive(flag);
			}
			this.removeObj.SetActive(flag);
			this.DispIconCharaMini(false, null);
			this.DispMarkNotYetReleased(false);
			this.DispTextDisable(false, null, null);
			this.SetupImgLock();
			this.SetupImgFavorite();
			this.DispInfoPop(false, null);
			this.DispTextParam(!flag);
			this.DispAttribute(this.photoPackData != null && this.photoPackData.staticData != null && (this.photoPackData.staticData.baseData.type == PhotoDef.Type.ABILITY || this.photoPackData.staticData.baseData.type == PhotoDef.Type.PARAMETER));
			this.DispUpto(false);
			this.DispLockNotOwn(false);
			this.DispUptoLv(false);
		}
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x00167BD0 File Offset: 0x00165DD0
	public void DispImgDisable(bool flag)
	{
		if (this.imgDisable != null)
		{
			this.imgDisable.gameObject.SetActive(flag);
		}
		if (!flag)
		{
			this.DispDrop(this.enablePhotoBonus, this.isQuestDrop);
			return;
		}
		if (this.imgDrop != null)
		{
			this.imgDrop.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x00167C31 File Offset: 0x00165E31
	public bool CheckImgDisable()
	{
		return this.imgDisable != null && this.imgDisable.gameObject.activeSelf;
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x00167C53 File Offset: 0x00165E53
	public void DispParty(bool flag, bool isEnabelTxt = true)
	{
		if (this.partyObj != null)
		{
			this.partyObj.SetActive(flag);
			this.partyObj.transform.Find("Fnt_party").gameObject.SetActive(isEnabelTxt);
		}
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x00167C90 File Offset: 0x00165E90
	public void DispTextDisable(bool flag, string strResson = null, string str = null)
	{
		if (this.textDisable != null)
		{
			this.textDisable.gameObject.SetActive(flag);
			if (strResson != null)
			{
				this.textDisable.transform.Find("Txt_Reason").GetComponent<PguiTextCtrl>().text = strResson;
			}
			if (str != null)
			{
				this.textDisable.text = str;
			}
		}
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x00167CF0 File Offset: 0x00165EF0
	public bool CheckTextDisable(bool flag = false, string str = null)
	{
		if (!(this.textDisable != null))
		{
			return false;
		}
		if (flag)
		{
			return this.textDisable.gameObject.activeSelf && this.textDisable.transform.Find("Txt_Reason").GetComponent<PguiTextCtrl>().text.Equals(str);
		}
		return this.textDisable.gameObject.activeSelf;
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x00167D5C File Offset: 0x00165F5C
	public void DispIconCharaMini(bool flag, CharaStaticData csd)
	{
		if (this.imgIconCharaMini != null)
		{
			if (csd != null)
			{
				this.imgIconCharaMini.SetRawImage(csd.GetMiniIconName(), true, false, null);
			}
			this.imgIconCharaMini.transform.parent.gameObject.SetActive(flag);
			this.imgIconCharaMini.transform.parent.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		}
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x00167DC5 File Offset: 0x00165FC5
	public void DispMarkNotYetReleased(bool flag)
	{
		if (this.imgMarkNotYetReleased != null)
		{
			this.imgMarkNotYetReleased.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x00167DE8 File Offset: 0x00165FE8
	public void DispInfoPop(bool flag, string str = null)
	{
		if (this.imgInfoPop != null)
		{
			this.imgInfoPop.gameObject.SetActive(flag);
			if (str != null)
			{
				this.imgInfoPop.transform.Find("Txt").GetComponent<PguiTextCtrl>().text = str;
			}
		}
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x00167E38 File Offset: 0x00166038
	public void DispDrop(bool bonusDrop = false, int questDrop = 0)
	{
		if (this.imgDrop != null)
		{
			this.enablePhotoBonus = bonusDrop;
			this.isQuestDrop = questDrop;
			this.imgDrop.gameObject.SetActive(this.enablePhotoBonus || this.isQuestDrop != 0);
			Transform transform = this.imgDrop.transform.Find("AEImage");
			if (transform != null)
			{
				transform.GetComponent<PguiReplaceAECtrl>().Replace((this.isQuestDrop > 0) ? (this.enablePhotoBonus ? "2" : "3") : ((this.isQuestDrop < 0) ? (this.enablePhotoBonus ? "4" : "5") : "1"));
				transform.GetComponent<PguiAECtrl>().PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			}
			PguiReplaceSpriteCtrl component = this.imgDrop.GetComponent<PguiReplaceSpriteCtrl>();
			if (component != null)
			{
				component.Replace((this.isQuestDrop > 0) ? (this.enablePhotoBonus ? 3 : 4) : ((this.isQuestDrop < 0) ? (this.enablePhotoBonus ? 5 : 6) : 1));
			}
		}
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x00167F4C File Offset: 0x0016614C
	public void DispAttribute(bool flag)
	{
		if (this.imgAttribute != null)
		{
			this.imgAttribute.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x00167F6D File Offset: 0x0016616D
	public void DispTextParam(bool flag)
	{
		if (this.textParam != null)
		{
			this.textParam.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x00167F8E File Offset: 0x0016618E
	public void DispUpto(bool flag)
	{
		if (this.uptoAe != null)
		{
			this.uptoAe.gameObject.SetActive(flag);
			if (flag)
			{
				this.uptoAe.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			}
		}
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x00167FBF File Offset: 0x001661BF
	public void DispLockNotOwn(bool flag)
	{
		if (this.imgLockNotOwn != null)
		{
			this.imgLockNotOwn.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C8F RID: 7311 RVA: 0x00167FE0 File Offset: 0x001661E0
	public void DispUptoLv(bool flag)
	{
		if (this.uptolvAe != null)
		{
			this.uptolvAe.gameObject.SetActive(flag);
			if (flag)
			{
				this.uptolvAe.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			}
		}
	}

	// Token: 0x06001C90 RID: 7312 RVA: 0x00168014 File Offset: 0x00166214
	public void SetupNoEquipIcon(PhotoPackData photo)
	{
		if (photo == null)
		{
			base.gameObject.SetActive(true);
			this.DispBase(false);
			this.DispTop(true);
			this.DispIconCharaMini(false, null);
			this.DispMarkNotYetReleased(false);
			this.DispImgDisable(false);
			this.DispParty(false, true);
			this.DispRemove(false);
			this.DispTextDisable(false, null, null);
			this.DispTextParam(false);
			this.DispUpto(false);
			this.DispLockNotOwn(false);
			this.DispUptoLv(false);
		}
	}

	// Token: 0x06001C91 RID: 7313 RVA: 0x00168088 File Offset: 0x00166288
	public void DispRarity(bool flag)
	{
		if (this.imgStar != null)
		{
			for (int i = 0; i < this.imgStar.Count; i++)
			{
				this.imgStar[i].gameObject.SetActive(flag && i < (int)this.photoStaticData.baseData.rarity);
			}
		}
	}

	// Token: 0x06001C92 RID: 7314 RVA: 0x001680E4 File Offset: 0x001662E4
	public void InvalidAE()
	{
		Transform transform = base.transform.Find("AEImage_Eff_Change");
		if (transform != null)
		{
			transform.gameObject.SetActive(false);
		}
		Transform transform2 = base.transform.Find("All");
		if (transform2 != null)
		{
			transform2.gameObject.GetComponent<AELayerConstraint>().enabled = false;
		}
	}

	// Token: 0x06001C93 RID: 7315 RVA: 0x00168142 File Offset: 0x00166342
	public void AddOnClickListener(IconPhotoCtrl.OnClick callback)
	{
		this.callbackCL = callback;
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x0016814B File Offset: 0x0016634B
	public void AddOnUpdateLockListener(IconPhotoCtrl.OnUpdateLockFlag callback)
	{
		this.callbackLF = callback;
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x00168154 File Offset: 0x00166354
	public void CallUpdateLockListener()
	{
		IconPhotoCtrl.OnUpdateLockFlag onUpdateLockFlag = this.callbackLF;
		if (onUpdateLockFlag == null)
		{
			return;
		}
		onUpdateLockFlag(this);
	}

	// Token: 0x06001C96 RID: 7318 RVA: 0x00168167 File Offset: 0x00166367
	public void AddOnUpdateFavoriteListener(IconPhotoCtrl.OnUpdateFavoriteFlag callback)
	{
		this.callbackFF = callback;
	}

	// Token: 0x06001C97 RID: 7319 RVA: 0x00168170 File Offset: 0x00166370
	public void CallUpdateFavoriteListener()
	{
		IconPhotoCtrl.OnUpdateFavoriteFlag onUpdateFavoriteFlag = this.callbackFF;
		if (onUpdateFavoriteFlag == null)
		{
			return;
		}
		onUpdateFavoriteFlag(this);
	}

	// Token: 0x06001C98 RID: 7320 RVA: 0x00168183 File Offset: 0x00166383
	public void AddOnUpdateStatus(IconPhotoCtrl.OnUpdateLockFlag callback)
	{
		this.callbackUpdateStatus = callback;
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x0016818C File Offset: 0x0016638C
	public void AddOnCloseWindow(IconPhotoCtrl.OnUpdateLockFlag callback)
	{
		this.callbackCloseWindow = callback;
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x00168195 File Offset: 0x00166395
	private void CallUpdateStatusListener()
	{
		IconPhotoCtrl.OnUpdateLockFlag onUpdateLockFlag = this.callbackUpdateStatus;
		if (onUpdateLockFlag == null)
		{
			return;
		}
		onUpdateLockFlag(this);
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x001681A8 File Offset: 0x001663A8
	private void CallClaseWindowListener()
	{
		IconPhotoCtrl.OnUpdateLockFlag onUpdateLockFlag = this.callbackCloseWindow;
		if (onUpdateLockFlag == null)
		{
			return;
		}
		onUpdateLockFlag(this);
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x001681BB File Offset: 0x001663BB
	public void OnPointerClick()
	{
		IconPhotoCtrl.OnClick onClick = this.callbackCL;
		if (onClick == null)
		{
			return;
		}
		onClick(this);
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x001681D0 File Offset: 0x001663D0
	public void OnLongPress()
	{
		if (!this.IsEnableLongPress)
		{
			return;
		}
		if (this.photoPackData != null && !this.photoPackData.IsInvalid())
		{
			PhotoWindowCtrl hdlPhotoWindowCtrl = CanvasManager.HdlPhotoWindowCtrl;
			PhotoWindowCtrl.SetupParam setupParam = new PhotoWindowCtrl.SetupParam();
			setupParam.ppd = this.photoPackData;
			setupParam.dispMax = this.DispMax;
			List<PhotoPackData> list;
			if (this.onReturnPhotoPackDataList == null)
			{
				list = null;
			}
			else
			{
				list = this.onReturnPhotoPackDataList().FindAll((PhotoPackData item) => item != null && !item.IsInvalid());
			}
			setupParam.dispPhotoPacks = list;
			setupParam.cb = delegate
			{
				this.CallUpdateStatusListener();
			};
			setupParam.ipc = this;
			setupParam.closeWindowCB = delegate
			{
				this.CallClaseWindowListener();
			};
			hdlPhotoWindowCtrl.Open(setupParam);
		}
	}

	// Token: 0x04001530 RID: 5424
	[SerializeField]
	private IconPhotoCtrl.Type type;

	// Token: 0x04001531 RID: 5425
	[SerializeField]
	private GameObject Base;

	// Token: 0x04001532 RID: 5426
	[SerializeField]
	private GameObject Over;

	// Token: 0x04001533 RID: 5427
	[SerializeField]
	private GameObject Top;

	// Token: 0x04001534 RID: 5428
	[SerializeField]
	private PguiRawImageCtrl texPhoto;

	// Token: 0x04001535 RID: 5429
	[SerializeField]
	private PguiImageCtrl imgAttribute;

	// Token: 0x04001536 RID: 5430
	[SerializeField]
	private PguiImageCtrl imgRareFrame;

	// Token: 0x04001537 RID: 5431
	[SerializeField]
	private PguiRawImageCtrl rawImgRareFrame;

	// Token: 0x04001538 RID: 5432
	[SerializeField]
	private List<PguiImageCtrl> imgStar;

	// Token: 0x04001539 RID: 5433
	[SerializeField]
	private PguiTextCtrl textCardName;

	// Token: 0x0400153A RID: 5434
	[SerializeField]
	private PguiImageCtrl imgLock;

	// Token: 0x0400153B RID: 5435
	[SerializeField]
	private PguiImageCtrl imgFavorite;

	// Token: 0x0400153C RID: 5436
	[SerializeField]
	private PguiAECtrl holoAe;

	// Token: 0x0400153D RID: 5437
	[SerializeField]
	private PguiTextCtrl textParam;

	// Token: 0x0400153E RID: 5438
	[SerializeField]
	private List<GameObject> imgBreakthroughLimit;

	// Token: 0x0400153F RID: 5439
	[SerializeField]
	private GameObject removeObj;

	// Token: 0x04001540 RID: 5440
	[SerializeField]
	private PguiImageCtrl imgDisable;

	// Token: 0x04001541 RID: 5441
	[SerializeField]
	private GameObject partyObj;

	// Token: 0x04001542 RID: 5442
	[SerializeField]
	private PguiTextCtrl textDisable;

	// Token: 0x04001543 RID: 5443
	[SerializeField]
	private PguiRawImageCtrl imgIconCharaMini;

	// Token: 0x04001544 RID: 5444
	[SerializeField]
	private PguiImageCtrl imgMarkNotYetReleased;

	// Token: 0x04001545 RID: 5445
	[SerializeField]
	private PguiAECtrl changeAe;

	// Token: 0x04001546 RID: 5446
	[SerializeField]
	private PguiImageCtrl imgInfoPop;

	// Token: 0x04001547 RID: 5447
	[SerializeField]
	private PguiImageCtrl imgDrop;

	// Token: 0x04001548 RID: 5448
	[SerializeField]
	private PguiAECtrl uptoAe;

	// Token: 0x04001549 RID: 5449
	[SerializeField]
	private PguiImageCtrl imgLockNotOwn;

	// Token: 0x0400154A RID: 5450
	[SerializeField]
	private PguiAECtrl uptolvAe;

	// Token: 0x0400154B RID: 5451
	private IconPhotoCtrl.OnClick callbackCL;

	// Token: 0x0400154C RID: 5452
	private IconPhotoCtrl.OnUpdateLockFlag callbackLF;

	// Token: 0x0400154D RID: 5453
	private IconPhotoCtrl.OnUpdateLockFlag callbackUpdateStatus;

	// Token: 0x0400154E RID: 5454
	private IconPhotoCtrl.OnUpdateLockFlag callbackCloseWindow;

	// Token: 0x0400154F RID: 5455
	private IconPhotoCtrl.OnUpdateFavoriteFlag callbackFF;

	// Token: 0x04001552 RID: 5458
	public IconPhotoCtrl.OnReturnPhotoPackDataList onReturnPhotoPackDataList;

	// Token: 0x04001553 RID: 5459
	private IconPhotoCtrl.SetupParam setupParam = new IconPhotoCtrl.SetupParam();

	// Token: 0x04001556 RID: 5462
	public bool enablePhotoBonus;

	// Token: 0x04001557 RID: 5463
	public int isQuestDrop;

	// Token: 0x02000F0C RID: 3852
	public enum Type
	{
		// Token: 0x040055AC RID: 21932
		CARD,
		// Token: 0x040055AD RID: 21933
		ICON,
		// Token: 0x040055AE RID: 21934
		ICON_MINI
	}

	// Token: 0x02000F0D RID: 3853
	// (Invoke) Token: 0x06004E83 RID: 20099
	public delegate void OnClick(IconPhotoCtrl ipc);

	// Token: 0x02000F0E RID: 3854
	// (Invoke) Token: 0x06004E87 RID: 20103
	public delegate void OnUpdateLockFlag(IconPhotoCtrl ipc);

	// Token: 0x02000F0F RID: 3855
	// (Invoke) Token: 0x06004E8B RID: 20107
	public delegate void OnUpdateFavoriteFlag(IconPhotoCtrl ipc);

	// Token: 0x02000F10 RID: 3856
	// (Invoke) Token: 0x06004E8F RID: 20111
	public delegate List<PhotoPackData> OnReturnPhotoPackDataList();

	// Token: 0x02000F11 RID: 3857
	public class SetupParam
	{
		// Token: 0x06004E92 RID: 20114 RVA: 0x00236CC1 File Offset: 0x00234EC1
		public SetupParam()
		{
			this.ppd = null;
			this.sortType = SortFilterDefine.SortType.LEVEL;
			this.isEnableRaycast = true;
			this.dispMax = false;
			this.playQuestOneId = -1;
			this.isEnableLongPress = true;
			this.isHelper = false;
		}

		// Token: 0x040055AF RID: 21935
		public PhotoPackData ppd;

		// Token: 0x040055B0 RID: 21936
		public SortFilterDefine.SortType sortType;

		// Token: 0x040055B1 RID: 21937
		public bool isEnableRaycast;

		// Token: 0x040055B2 RID: 21938
		public bool dispMax;

		// Token: 0x040055B3 RID: 21939
		public int playQuestOneId = -1;

		// Token: 0x040055B4 RID: 21940
		public bool isEnableLongPress;

		// Token: 0x040055B5 RID: 21941
		public bool isHelper;
	}
}
