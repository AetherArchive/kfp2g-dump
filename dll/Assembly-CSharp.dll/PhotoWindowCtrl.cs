using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class PhotoWindowCtrl : MonoBehaviour
{
	private List<PhotoPackData> DispPhotoPacks { get; set; }

	private bool IsBeforeCharacteristic()
	{
		return this.countCharacteristic % 2 == 0;
	}

	private void IncCountCharacteristic()
	{
		this.countCharacteristic++;
	}

	private void ClearCountCharacteristic()
	{
		this.countCharacteristic = 0;
		this.UpdateTextCharacteristic();
	}

	private void UpdateTextCharacteristic()
	{
		if (this.guiData != null)
		{
			this.guiData.Txt_Btn_Info.text = (this.IsBeforeCharacteristic() ? "変化後を見る" : "変化前へ戻る");
			this.guiData.Txt_Title.text = (this.IsBeforeCharacteristic() ? "とくせい" : "とくせい<color=#FF6E00> (変化後)</color>");
		}
	}

	private void SetupButton(bool isOpenPrev)
	{
		PhotoPackData photoPackData = this.photoPackData;
		bool flag = DataManager.DmPhoto.GetUserPhotoData(photoPackData.dataId) != null;
		this.guiData.Btn_Yaji_Left.gameObject.SetActive(this.IsPhotoAlbum || (flag && !isOpenPrev && this.DispPhotoPacks != null && this.DispPhotoPacks.Count > 1));
		this.guiData.Btn_Yaji_Right.gameObject.SetActive(this.IsPhotoAlbum || (flag && !isOpenPrev && this.DispPhotoPacks != null && this.DispPhotoPacks.Count > 1));
		this.guiData.Btn_PhotoGrow.gameObject.SetActive(flag && !photoPackData.staticData.baseData.isForbiddenGrowBase && (photoPackData.dynamicData.level < photoPackData.limitLevel || photoPackData.dynamicData.levelRank < 4));
	}

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

	public void SetPvpSeasonId(int seasonId)
	{
		this.pvpSeasonId = seasonId;
	}

	private bool OnSelectDeckTab(int index)
	{
		this.guiData.PhotoInfo.SetActive(index == 1);
		this.guiData.ParamInfo.SetActive(index == 0);
		return true;
	}

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

	private void OnClickQrDataHPButton(PguiButtonCtrl button)
	{
		Application.OpenURL("https://kemono-friends.sega.jp/mobilegame/#print");
	}

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

	private bool OnClickFavoriteToggle(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		this.reqFavorite = this.photoPackData.dynamicData.favoriteFlag != (toggleIndex == 0);
		if (this.RequestStatusFunc == null)
		{
			this.RequestStatusFunc = this.RequestUpdateStatus();
		}
		return true;
	}

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

	private void SetupInfo(bool dispMax)
	{
		this.guiData.Txt_GachaInfo.gameObject.SetActive(dispMax);
		this.guiData.Image_Gage.m_Image.fillAmount = ((this.photoPackData.dynamicData.level >= this.photoPackData.limitLevel || dispMax) ? 1f : ((float)this.photoPackData.dynamicData.exp / (float)DataManager.DmPhoto.GetExpByNextLevel(this.photoPackData)));
	}

	private void ChangePhoto(PhotoPackData ppd, IconPhotoCtrl ipc, bool isOpenPrev)
	{
		this.ChangePhoto(ppd, ipc);
		if (isOpenPrev)
		{
			this.ResetPrevData();
		}
		this.SetupButton(isOpenPrev);
	}

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

	public void Open(PhotoWindowCtrl.SetupParam param)
	{
		this.setupParam = param;
		this.Open(param.ppd, param.ipc, param.cb, param.dispMax, param.dispPhotoPacks, param.closeWindowCB, param.isPhotoAlbum);
	}

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

	public void ResetPrevData()
	{
		this.prevIconPhotoCtrl = null;
		this.prevPhotoPackData = null;
		this.prevDispMax = false;
	}

	private void OnDestroy()
	{
		if (this.guiQrData != null)
		{
			Object.Destroy(this.guiQrData.baseObj);
		}
	}

	private PhotoWindowCtrl.SetupParam setupParam = new PhotoWindowCtrl.SetupParam();

	private PhotoWindowCtrl.GUI guiData;

	private PhotoWindowCtrl.GUIQR guiQrData;

	private bool reqLock;

	private bool reqFavorite;

	private bool reqClose;

	private bool reqRevert;

	private bool currentRevertFlag;

	private PhotoPackData photoPackData;

	private IconPhotoCtrl currentIconPhotoCtrl;

	private PhotoPackData prevPhotoPackData;

	private IconPhotoCtrl prevIconPhotoCtrl;

	private bool prevDispMax;

	public bool IsPhotoAlbum;

	public int pvpSeasonId;

	private Transform touchTransform;

	private UnityAction callback;

	private UnityAction callbackCloseWindow;

	private int countCharacteristic;

	private IEnumerator requestClickLRButton;

	private IEnumerator RequestStatusFunc;

	public class GUI
	{
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

		public GameObject baseObj;

		public PguiTabGroupCtrl TabGroup;

		public PguiToggleButtonCtrl BtnKey;

		public PguiToggleButtonCtrl BtnFavorite;

		public PguiButtonCtrl BtnImageChange;

		public PguiButtonCtrl BtnQrCode;

		public IconPhotoCtrl iconPhotoCtrl;

		public GameObject photoBigBase;

		public IconPhotoCtrl iconPhotoCtrlBig;

		public PguiButtonCtrl Btn_Yaji_Left;

		public PguiButtonCtrl Btn_Yaji_Right;

		public PguiButtonCtrl Btn_PhotoGrow;

		public PguiTextCtrl Num_HP;

		public PguiTextCtrl Num_ATK;

		public PguiTextCtrl Num_DEF;

		public PguiTextCtrl Num_NextExp;

		public PguiTextCtrl Num_Level;

		public PguiImageCtrl Image_Gage;

		public PguiTextCtrl Txt_Ability;

		public PguiTextCtrl Txt_Effect;

		public PguiRawImageCtrl Image_EffectIcon;

		public InfoPhotoItemEffectCtrl infoPhotoItemEffectCtrl;

		public PguiTextCtrl infoPhotoItemEffectText;

		public PguiTextCtrl Txt_Illustrator;

		public PguiScrollText Txt_FlavorScroll;

		public PguiTextCtrl Txt_GachaInfo;

		public List<GameObject> rebirthList;

		public GameObject PhotoInfo;

		public GameObject ParamInfo;

		public PguiButtonCtrl Btn_Info;

		public PguiTextCtrl Txt_Btn_Info;

		public PguiTextCtrl Txt_Title;

		public PguiOpenWindowCtrl window;
	}

	public class GUIQR
	{
		public GUIQR(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_HP = baseTr.Find("Btn_HP").GetComponent<PguiButtonCtrl>();
			this.Mask = baseTr.Find("Base").GetComponent<PguiImageCtrl>();
			this.QrTexture = baseTr.Find("Img_Bg/Texture").GetComponent<PguiRawImageCtrl>();
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_HP;

		public PguiImageCtrl Mask;

		public PguiRawImageCtrl QrTexture;
	}

	public class SetupParam
	{
		public PhotoPackData ppd;

		public IconPhotoCtrl ipc;

		public UnityAction cb;

		public bool dispMax;

		public List<PhotoPackData> dispPhotoPacks;

		public UnityAction closeWindowCB;

		public bool isPhotoAlbum;
	}
}
