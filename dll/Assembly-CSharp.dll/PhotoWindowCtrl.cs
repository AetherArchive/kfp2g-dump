using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

// Token: 0x020001B2 RID: 434
public class PhotoWindowCtrl : MonoBehaviour
{
	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06001D4C RID: 7500 RVA: 0x0016C95A File Offset: 0x0016AB5A
	// (set) Token: 0x06001D4B RID: 7499 RVA: 0x0016C951 File Offset: 0x0016AB51
	private List<PhotoPackData> DispPhotoPacks { get; set; }

	// Token: 0x06001D4D RID: 7501 RVA: 0x0016C962 File Offset: 0x0016AB62
	private bool IsBeforeCharacteristic()
	{
		return this.countCharacteristic % 2 == 0;
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x0016C96F File Offset: 0x0016AB6F
	private void IncCountCharacteristic()
	{
		this.countCharacteristic++;
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x0016C97F File Offset: 0x0016AB7F
	private void ClearCountCharacteristic()
	{
		this.countCharacteristic = 0;
		this.UpdateTextCharacteristic();
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x0016C990 File Offset: 0x0016AB90
	private void UpdateTextCharacteristic()
	{
		if (this.guiData != null)
		{
			this.guiData.Txt_Btn_Info.text = (this.IsBeforeCharacteristic() ? "変化後を見る" : "変化前へ戻る");
			this.guiData.Txt_Title.text = (this.IsBeforeCharacteristic() ? "とくせい" : "とくせい<color=#FF6E00> (変化後)</color>");
		}
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x0016C9F0 File Offset: 0x0016ABF0
	private void SetupButton(bool isOpenPrev)
	{
		PhotoPackData photoPackData = this.photoPackData;
		bool flag = DataManager.DmPhoto.GetUserPhotoData(photoPackData.dataId) != null;
		this.guiData.Btn_Yaji_Left.gameObject.SetActive(this.IsPhotoAlbum || (flag && !isOpenPrev && this.DispPhotoPacks != null && this.DispPhotoPacks.Count > 1));
		this.guiData.Btn_Yaji_Right.gameObject.SetActive(this.IsPhotoAlbum || (flag && !isOpenPrev && this.DispPhotoPacks != null && this.DispPhotoPacks.Count > 1));
		this.guiData.Btn_PhotoGrow.gameObject.SetActive(flag && !photoPackData.staticData.baseData.isForbiddenGrowBase && (photoPackData.dynamicData.level < photoPackData.limitLevel || photoPackData.dynamicData.levelRank < 4));
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x0016CAEC File Offset: 0x0016ACEC
	public void Init()
	{
		if (this.guiData != null)
		{
			return;
		}
		this.ResetPrevData();
		this.guiData = new PhotoWindowCtrl.GUI(base.transform);
		this.guiData.TabGroup.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectDeckTab));
		this.guiData.ParamInfo.SetActive(true);
		this.guiData.PhotoInfo.SetActive(false);
		this.guiData.BtnKey.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickKeyToggle));
		this.guiData.BtnFavorite.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickFavoriteToggle));
		this.guiData.BtnImageChange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickImgChangeButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnQrCode.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickQrCodeButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Info.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			if (this.photoPackData == null)
			{
				return;
			}
			this.IncCountCharacteristic();
			this.UpdateTextCharacteristic();
			this.guiData.Txt_Ability.text = ((this.photoPackData.GetSwitchAbility(!this.IsBeforeCharacteristic()) != null) ? this.photoPackData.GetSwitchAbility(!this.IsBeforeCharacteristic()).abilityEffect : "");
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickLRButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickLRButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_PhotoGrow.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			this.guiData.window.ForceClose();
			this.prevIconPhotoCtrl = this.currentIconPhotoCtrl;
			this.prevPhotoPackData = this.photoPackData;
			this.prevDispMax = this.setupParam.dispMax;
			SceneCharaEdit.Args args = new SceneCharaEdit.Args
			{
				growPhotoId = this.photoPackData.dataId,
				requestSubMode = SelPhotoGrowCtrl.Mode.GROW_MAIN
			};
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneCharaEdit, args);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		PrjUtil.AddTouchEventTrigger(this.guiData.iconPhotoCtrl.TexPhoto.gameObject, new UnityAction<Transform>(this.OnTouchPhoto));
		PrjUtil.AddTouchEventTrigger(this.guiData.iconPhotoCtrlBig.TexPhoto.gameObject, new UnityAction<Transform>(this.OnTouchPhoto));
		PrjUtil.AddTouchEventTrigger(this.guiData.photoBigBase, new UnityAction<Transform>(this.OnTouchPhoto));
		GameObject gameObject = Resources.Load("SelCmn/GUI/Prefab/GUI_PhotoQRCode") as GameObject;
		this.guiQrData = new PhotoWindowCtrl.GUIQR(Object.Instantiate<GameObject>(gameObject, base.transform).transform);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.OVERLAY, this.guiQrData.baseObj.transform, true);
		this.guiQrData.baseObj.SetActive(false);
		this.guiQrData.Btn_HP.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickQrDataHPButton), PguiButtonCtrl.SoundType.DEFAULT);
		PrjUtil.AddTouchEventTrigger(this.guiQrData.Mask.gameObject, new UnityAction<Transform>(this.OnTouchPhoto));
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x0016CD38 File Offset: 0x0016AF38
	public void SetPvpSeasonId(int seasonId)
	{
		this.pvpSeasonId = seasonId;
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x0016CD41 File Offset: 0x0016AF41
	private bool OnSelectDeckTab(int index)
	{
		this.guiData.PhotoInfo.SetActive(index == 1);
		this.guiData.ParamInfo.SetActive(index == 0);
		return true;
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x0016CD6C File Offset: 0x0016AF6C
	private void OnClickImgChangeButton(PguiButtonCtrl button)
	{
		this.currentRevertFlag = !this.currentRevertFlag;
		this.guiData.iconPhotoCtrl.DispPhotoChange(!this.currentRevertFlag);
		this.guiData.iconPhotoCtrlBig.DispPhotoChange(!this.currentRevertFlag);
		bool flag = this.guiData.iconPhotoCtrl.isDispChangePhoto && !string.IsNullOrEmpty(this.photoPackData.staticData.baseData.illustratorafter);
		this.guiData.Txt_Illustrator.text = (flag ? this.photoPackData.staticData.baseData.illustratorafter : this.photoPackData.staticData.baseData.illustrator);
		this.guiData.Txt_FlavorScroll.SetText(this.guiData.iconPhotoCtrl.isDispChangePhoto ? this.photoPackData.staticData.baseData.flavorTextAfter : this.photoPackData.staticData.baseData.flavorText);
		if (this.IsPhotoAlbum)
		{
			PhotoPackData photoPackData;
			if (!this.currentRevertFlag)
			{
				photoPackData = this.photoPackData;
			}
			else
			{
				photoPackData = new PhotoPackData(new PhotoDynamicData
				{
					OwnerType = PhotoDynamicData.PhotoOwnerType.User,
					dataId = -1L,
					photoId = this.photoPackData.staticData.GetId(),
					level = this.photoPackData.staticData.getLimitLevel(0),
					levelRank = 0
				});
			}
			this.UpdateParamText(photoPackData);
			this.ClearCountCharacteristic();
			this.guiData.Btn_Info.gameObject.SetActive(photoPackData.IsEnableSwitchCharacteristic());
			return;
		}
		this.reqRevert = this.photoPackData.dynamicData.imgRevertFlag != this.currentRevertFlag;
		if (this.RequestStatusFunc == null)
		{
			this.RequestStatusFunc = this.RequestUpdateStatus();
		}
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x0016CF44 File Offset: 0x0016B144
	private void OnClickQrCodeButton(PguiButtonCtrl button)
	{
		this.guiQrData.baseObj.SetActive(true);
		Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.SYSTEM).GetComponent<Blur>().enabled = true;
		string text = this.photoPackData.staticData.GetQrImageName();
		if (this.guiData.iconPhotoCtrl.isDispChangePhoto)
		{
			text += "_2";
		}
		this.guiQrData.QrTexture.SetRawImage(text, true, false, null);
	}

	// Token: 0x06001D57 RID: 7511 RVA: 0x0016CFBB File Offset: 0x0016B1BB
	private void OnClickQrDataHPButton(PguiButtonCtrl button)
	{
		Application.OpenURL("https://kemono-friends.sega.jp/mobilegame/#print");
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x0016CFC8 File Offset: 0x0016B1C8
	private void OnClickLRButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.Btn_Yaji_Left || button == this.guiData.Btn_Yaji_Right)
		{
			if (this.DispPhotoPacks == null || !this.DispPhotoPacks.Contains(this.photoPackData))
			{
				return;
			}
			this.RequestClickLRButton(button);
		}
	}

	// Token: 0x06001D59 RID: 7513 RVA: 0x0016D020 File Offset: 0x0016B220
	private void RequestClickLRButton(PguiButtonCtrl button)
	{
		int num = this.DispPhotoPacks.IndexOf(this.photoPackData);
		num += ((button == this.guiData.Btn_Yaji_Left) ? (-1) : 1);
		num = (num + this.DispPhotoPacks.Count) % this.DispPhotoPacks.Count;
		this.ChangePhoto(this.DispPhotoPacks[num], null, false);
		if ((this.reqLock || this.reqRevert) && this.RequestStatusFunc == null)
		{
			this.RequestStatusFunc = this.RequestUpdateStatus();
		}
	}

	// Token: 0x06001D5A RID: 7514 RVA: 0x0016D0AC File Offset: 0x0016B2AC
	private bool OnClickKeyToggle(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		SoundManager.Play("prd_se_photo_lock_unlock", false, false);
		this.reqLock = this.photoPackData.dynamicData.lockFlag != (toggleIndex == 0);
		if (this.RequestStatusFunc == null)
		{
			this.RequestStatusFunc = this.RequestUpdateStatus();
		}
		return true;
	}

	// Token: 0x06001D5B RID: 7515 RVA: 0x0016D0FA File Offset: 0x0016B2FA
	private bool OnClickFavoriteToggle(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		this.reqFavorite = this.photoPackData.dynamicData.favoriteFlag != (toggleIndex == 0);
		if (this.RequestStatusFunc == null)
		{
			this.RequestStatusFunc = this.RequestUpdateStatus();
		}
		return true;
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x0016D130 File Offset: 0x0016B330
	private void OnTouchPhoto(Transform touch)
	{
		if (touch.gameObject == this.guiData.iconPhotoCtrl.TexPhoto.gameObject)
		{
			this.guiData.photoBigBase.SetActive(true);
			this.guiData.iconPhotoCtrlBig.FrameDisp = true;
			this.touchTransform = null;
			return;
		}
		if (touch.gameObject == this.guiData.iconPhotoCtrlBig.TexPhoto.gameObject)
		{
			this.guiData.photoBigBase.SetActive(false);
			this.touchTransform = touch;
			return;
		}
		if (touch.gameObject == this.guiData.photoBigBase)
		{
			this.guiData.iconPhotoCtrlBig.FrameDisp = !this.guiData.iconPhotoCtrlBig.FrameDisp;
			return;
		}
		if (touch.gameObject == this.guiQrData.Mask.gameObject)
		{
			this.guiQrData.baseObj.SetActive(false);
			Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.SYSTEM).GetComponent<Blur>().enabled = false;
		}
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x0016D244 File Offset: 0x0016B444
	private void SetupInfo(bool dispMax)
	{
		this.guiData.Txt_GachaInfo.gameObject.SetActive(dispMax);
		this.guiData.Image_Gage.m_Image.fillAmount = ((this.photoPackData.dynamicData.level >= this.photoPackData.limitLevel || dispMax) ? 1f : ((float)this.photoPackData.dynamicData.exp / (float)DataManager.DmPhoto.GetExpByNextLevel(this.photoPackData)));
	}

	// Token: 0x06001D5E RID: 7518 RVA: 0x0016D2CA File Offset: 0x0016B4CA
	private void ChangePhoto(PhotoPackData ppd, IconPhotoCtrl ipc, bool isOpenPrev)
	{
		this.ChangePhoto(ppd, ipc);
		if (isOpenPrev)
		{
			this.ResetPrevData();
		}
		this.SetupButton(isOpenPrev);
	}

	// Token: 0x06001D5F RID: 7519 RVA: 0x0016D2E4 File Offset: 0x0016B4E4
	private void ChangePhoto(PhotoPackData ppd, IconPhotoCtrl ipc)
	{
		this.photoPackData = ppd;
		this.RequestStatusFunc = null;
		this.reqClose = false;
		this.reqLock = false;
		this.reqRevert = false;
		this.reqFavorite = false;
		PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByPhoto(ppd);
		this.ClearCountCharacteristic();
		this.guiData.Btn_Info.gameObject.SetActive(this.photoPackData.IsEnableSwitchCharacteristic());
		this.guiData.Txt_FlavorScroll.ResetScrollContent();
		this.guiData.iconPhotoCtrl.Setup(ppd, SortFilterDefine.SortType.LEVEL, true, false, -1, false);
		this.guiData.iconPhotoCtrlBig.Setup(ppd, SortFilterDefine.SortType.LEVEL, true, false, -1, false);
		this.currentIconPhotoCtrl = ipc;
		this.guiData.Txt_Effect.transform.parent.gameObject.SetActive(false);
		this.guiData.infoPhotoItemEffectText.gameObject.SetActive(false);
		if (ppd != null && ppd.staticData != null)
		{
			this.guiData.infoPhotoItemEffectCtrl.Setup(new InfoPhotoItemEffectCtrl.SetupParam
			{
				photoPackDatas = new List<PhotoPackData> { this.IsPhotoAlbum ? null : ppd },
				infoText = (this.IsPhotoAlbum ? null : this.guiData.infoPhotoItemEffectText)
			});
		}
		this.guiData.BtnKey.SetToggleIndex(ppd.dynamicData.lockFlag ? 1 : 0);
		this.guiData.BtnFavorite.SetToggleIndex(ppd.dynamicData.favoriteFlag ? 1 : 0);
		this.guiData.BtnKey.gameObject.SetActive(!this.IsPhotoAlbum && ppd.IsMine());
		this.guiData.BtnFavorite.gameObject.SetActive(!this.IsPhotoAlbum && ppd.IsMine());
		this.guiData.BtnQrCode.gameObject.SetActive(ppd.IsMine() && ppd.staticData.baseData.qrDispStartTime <= TimeManager.Now);
		this.guiData.BtnImageChange.gameObject.SetActive(ppd.IsMine() && ppd.IsEnableImageChange());
		this.UpdateParamText(ppd);
		this.SetupInfo(false);
		bool flag = this.guiData.iconPhotoCtrl.isDispChangePhoto && !string.IsNullOrEmpty(ppd.staticData.baseData.illustratorafter);
		this.guiData.Txt_Illustrator.text = (flag ? ppd.staticData.baseData.illustratorafter : ppd.staticData.baseData.illustrator);
		this.guiData.Txt_FlavorScroll.SetText(this.guiData.iconPhotoCtrl.isDispChangePhoto ? ppd.staticData.baseData.flavorTextAfter : ppd.staticData.baseData.flavorText);
		this.guiData.Num_HP.text = paramPreset.hp.ToString();
		this.guiData.Num_ATK.text = paramPreset.atk.ToString();
		this.guiData.Num_DEF.text = paramPreset.def.ToString();
		this.guiData.Num_NextExp.text = (DataManager.DmPhoto.GetExpByNextLevel(ppd) - ppd.dynamicData.exp).ToString();
		this.guiData.Num_Level.text = "Lv." + ppd.dynamicData.level.ToString() + "/" + ppd.limitLevel.ToString();
		this.guiData.Txt_Ability.text = ((ppd.GetCurrentAbility() != null) ? ppd.GetCurrentAbility().abilityEffect : "");
		for (int i = 0; i < this.guiData.rebirthList.Count; i++)
		{
			this.guiData.rebirthList[i].SetActive(i < ppd.dynamicData.levelRank);
		}
		this.currentRevertFlag = ppd.dynamicData.imgRevertFlag;
		this.touchTransform = null;
		this.guiData.window.Setup(ppd.staticData.baseData.photoName, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			this.reqClose = true;
			UnityAction unityAction = this.callbackCloseWindow;
			if (unityAction != null)
			{
				unityAction();
			}
			return true;
		}, null, false);
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x0016D740 File Offset: 0x0016B940
	private void UpdateParamText(PhotoPackData ppd)
	{
		PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByPhoto(ppd);
		bool flag = this.guiData.iconPhotoCtrl.isDispChangePhoto && !string.IsNullOrEmpty(ppd.staticData.baseData.illustratorafter);
		this.guiData.Txt_Illustrator.text = (flag ? ppd.staticData.baseData.illustratorafter : ppd.staticData.baseData.illustrator);
		this.guiData.Txt_FlavorScroll.SetText(this.guiData.iconPhotoCtrl.isDispChangePhoto ? ppd.staticData.baseData.flavorTextAfter : ppd.staticData.baseData.flavorText);
		this.guiData.Num_HP.text = paramPreset.hp.ToString();
		this.guiData.Num_ATK.text = paramPreset.atk.ToString();
		this.guiData.Num_DEF.text = paramPreset.def.ToString();
		this.guiData.Num_NextExp.text = (DataManager.DmPhoto.GetExpByNextLevel(ppd) - ppd.dynamicData.exp).ToString();
		this.guiData.Num_Level.text = "Lv." + ppd.dynamicData.level.ToString() + "/" + ppd.limitLevel.ToString();
		this.guiData.Txt_Ability.text = ((ppd.GetCurrentAbility() != null) ? ppd.GetCurrentAbility().abilityEffect : "");
	}

	// Token: 0x06001D61 RID: 7521 RVA: 0x0016D8E8 File Offset: 0x0016BAE8
	private void Update()
	{
		if (this.touchTransform != null)
		{
			this.OnTouchPhoto(this.touchTransform);
		}
		if (this.RequestStatusFunc != null && !this.RequestStatusFunc.MoveNext())
		{
			this.RequestStatusFunc = null;
			UnityAction unityAction = this.callback;
			if (unityAction == null)
			{
				return;
			}
			unityAction();
		}
	}

	// Token: 0x06001D62 RID: 7522 RVA: 0x0016D93B File Offset: 0x0016BB3B
	private IEnumerator RequestUpdateStatus()
	{
		bool isLock = (this.reqLock ? (!this.photoPackData.dynamicData.lockFlag) : this.photoPackData.dynamicData.lockFlag);
		bool isRevert = (this.reqRevert ? (!this.photoPackData.dynamicData.imgRevertFlag) : this.photoPackData.dynamicData.imgRevertFlag);
		bool isFavorite = (this.reqFavorite ? (!this.photoPackData.dynamicData.favoriteFlag) : this.photoPackData.dynamicData.favoriteFlag);
		DataManager.DmPhoto.RequestActionPhotoUpdateStatus(this.photoPackData.dynamicData.dataId, isLock, isRevert, isFavorite);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (this.currentIconPhotoCtrl != null)
		{
			if (this.reqRevert)
			{
				this.currentIconPhotoCtrl.photoPackData.dynamicData.imgRevertFlag = isRevert;
				this.currentIconPhotoCtrl.DispPhotoChange(!isRevert);
			}
			if (this.reqLock)
			{
				this.currentIconPhotoCtrl.photoPackData.dynamicData.lockFlag = isLock;
				this.currentIconPhotoCtrl.DispLockIcon(isLock);
				this.currentIconPhotoCtrl.CallUpdateLockListener();
			}
			if (this.reqFavorite)
			{
				this.currentIconPhotoCtrl.photoPackData.dynamicData.favoriteFlag = isFavorite;
				this.currentIconPhotoCtrl.DispFavoriteIcon(isFavorite);
				this.currentIconPhotoCtrl.CallUpdateFavoriteListener();
			}
		}
		this.reqLock = false;
		this.reqFavorite = false;
		yield break;
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x0016D94C File Offset: 0x0016BB4C
	public void OpenPrev()
	{
		if (this.prevPhotoPackData == null)
		{
			return;
		}
		if (DataManager.DmPhoto.GetUserPhotoData(this.prevPhotoPackData.dataId) == null)
		{
			return;
		}
		base.gameObject.SetActive(true);
		this.ChangePhoto(this.prevPhotoPackData, this.prevIconPhotoCtrl, true);
		this.SetupInfo(this.prevDispMax);
		this.guiData.window.Open();
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x0016D9B5 File Offset: 0x0016BBB5
	public void Open(PhotoWindowCtrl.SetupParam param)
	{
		this.setupParam = param;
		this.Open(param.ppd, param.ipc, param.cb, param.dispMax, param.dispPhotoPacks, param.closeWindowCB, param.isPhotoAlbum);
	}

	// Token: 0x06001D65 RID: 7525 RVA: 0x0016D9F0 File Offset: 0x0016BBF0
	private void Open(PhotoPackData ppd, IconPhotoCtrl ipc, UnityAction cb, bool dispMax, List<PhotoPackData> photoPackDatas, UnityAction cbClose, bool isPhotoAlbum)
	{
		this.DispPhotoPacks = photoPackDatas;
		this.IsPhotoAlbum = isPhotoAlbum;
		this.callback = cb;
		this.callbackCloseWindow = cbClose;
		base.gameObject.SetActive(true);
		this.ChangePhoto(ppd, ipc, false);
		this.SetupInfo(dispMax);
		this.guiData.window.Open();
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x0016DA49 File Offset: 0x0016BC49
	public void ResetPrevData()
	{
		this.prevIconPhotoCtrl = null;
		this.prevPhotoPackData = null;
		this.prevDispMax = false;
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x0016DA60 File Offset: 0x0016BC60
	private void OnDestroy()
	{
		if (this.guiQrData != null)
		{
			Object.Destroy(this.guiQrData.baseObj);
		}
	}

	// Token: 0x0400158D RID: 5517
	private PhotoWindowCtrl.SetupParam setupParam = new PhotoWindowCtrl.SetupParam();

	// Token: 0x0400158E RID: 5518
	private PhotoWindowCtrl.GUI guiData;

	// Token: 0x0400158F RID: 5519
	private PhotoWindowCtrl.GUIQR guiQrData;

	// Token: 0x04001590 RID: 5520
	private bool reqLock;

	// Token: 0x04001591 RID: 5521
	private bool reqFavorite;

	// Token: 0x04001592 RID: 5522
	private bool reqClose;

	// Token: 0x04001593 RID: 5523
	private bool reqRevert;

	// Token: 0x04001594 RID: 5524
	private bool currentRevertFlag;

	// Token: 0x04001595 RID: 5525
	private PhotoPackData photoPackData;

	// Token: 0x04001596 RID: 5526
	private IconPhotoCtrl currentIconPhotoCtrl;

	// Token: 0x04001597 RID: 5527
	private PhotoPackData prevPhotoPackData;

	// Token: 0x04001598 RID: 5528
	private IconPhotoCtrl prevIconPhotoCtrl;

	// Token: 0x04001599 RID: 5529
	private bool prevDispMax;

	// Token: 0x0400159A RID: 5530
	public bool IsPhotoAlbum;

	// Token: 0x0400159B RID: 5531
	public int pvpSeasonId;

	// Token: 0x0400159C RID: 5532
	private Transform touchTransform;

	// Token: 0x0400159E RID: 5534
	private UnityAction callback;

	// Token: 0x0400159F RID: 5535
	private UnityAction callbackCloseWindow;

	// Token: 0x040015A0 RID: 5536
	private int countCharacteristic;

	// Token: 0x040015A1 RID: 5537
	private IEnumerator requestClickLRButton;

	// Token: 0x040015A2 RID: 5538
	private IEnumerator RequestStatusFunc;

	// Token: 0x02000F4D RID: 3917
	public class GUI
	{
		// Token: 0x06004F2D RID: 20269 RVA: 0x002393E0 File Offset: 0x002375E0
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.TabGroup = baseTr.Find("Base/Window/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.BtnKey = baseTr.Find("Base/Window/BtnKey").GetComponent<PguiToggleButtonCtrl>();
			this.BtnFavorite = baseTr.Find("Base/Window/Btn_Favorite").GetComponent<PguiToggleButtonCtrl>();
			this.BtnImageChange = baseTr.Find("Base/Window/BtnChange").GetComponent<PguiButtonCtrl>();
			this.BtnQrCode = baseTr.Find("Base/Window/BtnQRCode").GetComponent<PguiButtonCtrl>();
			this.Btn_Yaji_Left = baseTr.Find("Base/Window/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
			this.Btn_Yaji_Right = baseTr.Find("Base/Window/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
			this.Btn_PhotoGrow = baseTr.Find("Base/Window/Btn_PhotoGrow").GetComponent<PguiButtonCtrl>();
			this.Num_HP = baseTr.Find("Base/Window/ParamInfo/Box01/Param01/Num_01").GetComponent<PguiTextCtrl>();
			this.Num_ATK = baseTr.Find("Base/Window/ParamInfo/Box01/Param02/Num_01").GetComponent<PguiTextCtrl>();
			this.Num_DEF = baseTr.Find("Base/Window/ParamInfo/Box01/Param03/Num_01").GetComponent<PguiTextCtrl>();
			this.Num_NextExp = baseTr.Find("Base/Window/ParamInfo/Box01/ExpGage/Num_Lv01").GetComponent<PguiTextCtrl>();
			this.Num_Level = baseTr.Find("Base/Window/ParamInfo/Box01/ExpGage/Num_Lv02").GetComponent<PguiTextCtrl>();
			this.Image_Gage = baseTr.Find("Base/Window/ParamInfo/Box01/ExpGage/Gage").GetComponent<PguiImageCtrl>();
			this.Txt_Ability = baseTr.Find("Base/Window/ParamInfo/Box02/Txt").GetComponent<PguiTextCtrl>();
			this.Btn_Info = baseTr.Find("Base/Window/ParamInfo/Btn_Info").GetComponent<PguiButtonCtrl>();
			this.Txt_Btn_Info = baseTr.Find("Base/Window/ParamInfo/Btn_Info/BaseImage/Txt").GetComponent<PguiTextCtrl>();
			this.Txt_Title = baseTr.Find("Base/Window/ParamInfo/Box02/Title").GetComponent<PguiTextCtrl>();
			this.Txt_Illustrator = baseTr.Find("Base/Window/PhotoInfo/Box01/Txt").GetComponent<PguiTextCtrl>();
			this.Txt_FlavorScroll = baseTr.Find("Base/Window/PhotoInfo/Box02/PguiScrollText").GetComponent<PguiScrollText>();
			this.Txt_FlavorScroll.GetComponent<ScrollRect>().scrollSensitivity = ScrollParamDefine.PhotoFlavor;
			this.iconPhotoCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Card_Photo, baseTr.Find("Base/Window/CardPhoto")).GetComponent<IconPhotoCtrl>();
			this.photoBigBase = baseTr.Find("Base/FrontBg").gameObject;
			this.iconPhotoCtrlBig = Object.Instantiate<GameObject>(CanvasManager.RefResource.Card_Photo, baseTr.Find("Base/FrontBg/CardPhoto")).GetComponent<IconPhotoCtrl>();
			this.photoBigBase.gameObject.SetActive(false);
			this.PhotoInfo = baseTr.Find("Base/Window/PhotoInfo").gameObject;
			this.ParamInfo = baseTr.Find("Base/Window/ParamInfo").gameObject;
			this.Txt_Effect = baseTr.Find("Base/Window/PhotoEffect/Txt_Effect").GetComponent<PguiTextCtrl>();
			this.Image_EffectIcon = baseTr.Find("Base/Window/PhotoEffect/Icon_Item").GetComponent<PguiRawImageCtrl>();
			this.infoPhotoItemEffectCtrl = baseTr.Find("Base/Window/Info_PhotoItemEffect").GetComponent<InfoPhotoItemEffectCtrl>();
			this.infoPhotoItemEffectText = baseTr.Find("Base/Window/Info").GetComponent<PguiTextCtrl>();
			this.Txt_GachaInfo = baseTr.Find("Base/Window/Txt_GachaInfo").GetComponent<PguiTextCtrl>();
			GameObject gameObject = AssetManager.GetAssetData("SelCmn/GUI/Prefab/Icon_PhotoRebirth") as GameObject;
			this.rebirthList = new List<GameObject>();
			for (int i = 0; i < 4; i++)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, baseTr.Find("Base/Window/ParamInfo/Box01/RebirthIcon"));
				this.rebirthList.Add(gameObject2.transform.Find("Icon_PhotoRebirth_Act").gameObject);
			}
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		// Token: 0x0400569D RID: 22173
		public GameObject baseObj;

		// Token: 0x0400569E RID: 22174
		public PguiTabGroupCtrl TabGroup;

		// Token: 0x0400569F RID: 22175
		public PguiToggleButtonCtrl BtnKey;

		// Token: 0x040056A0 RID: 22176
		public PguiToggleButtonCtrl BtnFavorite;

		// Token: 0x040056A1 RID: 22177
		public PguiButtonCtrl BtnImageChange;

		// Token: 0x040056A2 RID: 22178
		public PguiButtonCtrl BtnQrCode;

		// Token: 0x040056A3 RID: 22179
		public IconPhotoCtrl iconPhotoCtrl;

		// Token: 0x040056A4 RID: 22180
		public GameObject photoBigBase;

		// Token: 0x040056A5 RID: 22181
		public IconPhotoCtrl iconPhotoCtrlBig;

		// Token: 0x040056A6 RID: 22182
		public PguiButtonCtrl Btn_Yaji_Left;

		// Token: 0x040056A7 RID: 22183
		public PguiButtonCtrl Btn_Yaji_Right;

		// Token: 0x040056A8 RID: 22184
		public PguiButtonCtrl Btn_PhotoGrow;

		// Token: 0x040056A9 RID: 22185
		public PguiTextCtrl Num_HP;

		// Token: 0x040056AA RID: 22186
		public PguiTextCtrl Num_ATK;

		// Token: 0x040056AB RID: 22187
		public PguiTextCtrl Num_DEF;

		// Token: 0x040056AC RID: 22188
		public PguiTextCtrl Num_NextExp;

		// Token: 0x040056AD RID: 22189
		public PguiTextCtrl Num_Level;

		// Token: 0x040056AE RID: 22190
		public PguiImageCtrl Image_Gage;

		// Token: 0x040056AF RID: 22191
		public PguiTextCtrl Txt_Ability;

		// Token: 0x040056B0 RID: 22192
		public PguiTextCtrl Txt_Effect;

		// Token: 0x040056B1 RID: 22193
		public PguiRawImageCtrl Image_EffectIcon;

		// Token: 0x040056B2 RID: 22194
		public InfoPhotoItemEffectCtrl infoPhotoItemEffectCtrl;

		// Token: 0x040056B3 RID: 22195
		public PguiTextCtrl infoPhotoItemEffectText;

		// Token: 0x040056B4 RID: 22196
		public PguiTextCtrl Txt_Illustrator;

		// Token: 0x040056B5 RID: 22197
		public PguiScrollText Txt_FlavorScroll;

		// Token: 0x040056B6 RID: 22198
		public PguiTextCtrl Txt_GachaInfo;

		// Token: 0x040056B7 RID: 22199
		public List<GameObject> rebirthList;

		// Token: 0x040056B8 RID: 22200
		public GameObject PhotoInfo;

		// Token: 0x040056B9 RID: 22201
		public GameObject ParamInfo;

		// Token: 0x040056BA RID: 22202
		public PguiButtonCtrl Btn_Info;

		// Token: 0x040056BB RID: 22203
		public PguiTextCtrl Txt_Btn_Info;

		// Token: 0x040056BC RID: 22204
		public PguiTextCtrl Txt_Title;

		// Token: 0x040056BD RID: 22205
		public PguiOpenWindowCtrl window;
	}

	// Token: 0x02000F4E RID: 3918
	public class GUIQR
	{
		// Token: 0x06004F2E RID: 20270 RVA: 0x0023973C File Offset: 0x0023793C
		public GUIQR(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_HP = baseTr.Find("Btn_HP").GetComponent<PguiButtonCtrl>();
			this.Mask = baseTr.Find("Base").GetComponent<PguiImageCtrl>();
			this.QrTexture = baseTr.Find("Img_Bg/Texture").GetComponent<PguiRawImageCtrl>();
		}

		// Token: 0x040056BE RID: 22206
		public GameObject baseObj;

		// Token: 0x040056BF RID: 22207
		public PguiButtonCtrl Btn_HP;

		// Token: 0x040056C0 RID: 22208
		public PguiImageCtrl Mask;

		// Token: 0x040056C1 RID: 22209
		public PguiRawImageCtrl QrTexture;
	}

	// Token: 0x02000F4F RID: 3919
	public class SetupParam
	{
		// Token: 0x040056C2 RID: 22210
		public PhotoPackData ppd;

		// Token: 0x040056C3 RID: 22211
		public IconPhotoCtrl ipc;

		// Token: 0x040056C4 RID: 22212
		public UnityAction cb;

		// Token: 0x040056C5 RID: 22213
		public bool dispMax;

		// Token: 0x040056C6 RID: 22214
		public List<PhotoPackData> dispPhotoPacks;

		// Token: 0x040056C7 RID: 22215
		public UnityAction closeWindowCB;

		// Token: 0x040056C8 RID: 22216
		public bool isPhotoAlbum;
	}
}
