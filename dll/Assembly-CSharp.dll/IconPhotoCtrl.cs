using System;
using System.Collections.Generic;
using AEAuth3;
using SGNFW.Common;
using UnityEngine;

public class IconPhotoCtrl : MonoBehaviour
{
	public PguiRawImageCtrl TexPhoto
	{
		get
		{
			return this.texPhoto;
		}
	}

	public PhotoPackData photoPackData { get; private set; }

	public PhotoStaticData photoStaticData { get; private set; }

	private bool IsEnableLongPress
	{
		get
		{
			return this.setupParam.isEnableLongPress && this.setupParam.isEnableRaycast;
		}
	}

	private void SetupImgLock()
	{
		if (this.imgLock != null)
		{
			this.imgLock.m_Image.enabled = this.photoPackData != null && this.photoPackData.dynamicData != null && this.photoPackData.dynamicData.lockFlag;
		}
	}

	private void SetupImgFavorite()
	{
		if (this.imgFavorite != null)
		{
			this.imgFavorite.m_Image.enabled = this.photoPackData != null && this.photoPackData.dynamicData != null && this.photoPackData.dynamicData.favoriteFlag;
		}
	}

	public PguiAECtrl aeEffChange
	{
		get
		{
			return this.changeAe;
		}
	}

	public bool isDispChangePhoto { get; private set; }

	public void ForceActive()
	{
		base.gameObject.SetActive(true);
	}

	public bool DispMax { get; private set; }

	public void Setup(IconPhotoCtrl.SetupParam param)
	{
		this.setupParam = param;
		this.Setup(param.ppd, param.sortType, param.isEnableRaycast, param.dispMax, param.playQuestOneId, param.isHelper);
	}

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

	public void DispLockIcon(bool flag)
	{
		if (this.imgLock != null)
		{
			this.imgLock.m_Image.enabled = flag;
		}
	}

	public void DispFavoriteIcon(bool flag)
	{
		if (this.imgFavorite != null)
		{
			this.imgFavorite.m_Image.enabled = flag;
		}
	}

	private static string GetPhotoChangeName(string original)
	{
		return original += "_2";
	}

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

	public void DispTop(bool flag)
	{
		if (this.Top != null)
		{
			this.Top.SetActive(flag);
		}
	}

	public void DispOver(bool flag)
	{
		if (this.Over != null)
		{
			this.Over.SetActive(flag);
		}
	}

	public void DispBase(bool flag)
	{
		if (this.Base != null)
		{
			this.Base.SetActive(flag);
		}
	}

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

	public bool CheckImgDisable()
	{
		return this.imgDisable != null && this.imgDisable.gameObject.activeSelf;
	}

	public void DispParty(bool flag, bool isEnabelTxt = true)
	{
		if (this.partyObj != null)
		{
			this.partyObj.SetActive(flag);
			this.partyObj.transform.Find("Fnt_party").gameObject.SetActive(isEnabelTxt);
		}
	}

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

	public void DispMarkNotYetReleased(bool flag)
	{
		if (this.imgMarkNotYetReleased != null)
		{
			this.imgMarkNotYetReleased.gameObject.SetActive(flag);
		}
	}

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

	public void DispAttribute(bool flag)
	{
		if (this.imgAttribute != null)
		{
			this.imgAttribute.gameObject.SetActive(flag);
		}
	}

	public void DispTextParam(bool flag)
	{
		if (this.textParam != null)
		{
			this.textParam.gameObject.SetActive(flag);
		}
	}

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

	public void DispLockNotOwn(bool flag)
	{
		if (this.imgLockNotOwn != null)
		{
			this.imgLockNotOwn.gameObject.SetActive(flag);
		}
	}

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

	public void AddOnClickListener(IconPhotoCtrl.OnClick callback)
	{
		this.callbackCL = callback;
	}

	public void AddOnUpdateLockListener(IconPhotoCtrl.OnUpdateLockFlag callback)
	{
		this.callbackLF = callback;
	}

	public void CallUpdateLockListener()
	{
		IconPhotoCtrl.OnUpdateLockFlag onUpdateLockFlag = this.callbackLF;
		if (onUpdateLockFlag == null)
		{
			return;
		}
		onUpdateLockFlag(this);
	}

	public void AddOnUpdateFavoriteListener(IconPhotoCtrl.OnUpdateFavoriteFlag callback)
	{
		this.callbackFF = callback;
	}

	public void CallUpdateFavoriteListener()
	{
		IconPhotoCtrl.OnUpdateFavoriteFlag onUpdateFavoriteFlag = this.callbackFF;
		if (onUpdateFavoriteFlag == null)
		{
			return;
		}
		onUpdateFavoriteFlag(this);
	}

	public void AddOnUpdateStatus(IconPhotoCtrl.OnUpdateLockFlag callback)
	{
		this.callbackUpdateStatus = callback;
	}

	public void AddOnCloseWindow(IconPhotoCtrl.OnUpdateLockFlag callback)
	{
		this.callbackCloseWindow = callback;
	}

	private void CallUpdateStatusListener()
	{
		IconPhotoCtrl.OnUpdateLockFlag onUpdateLockFlag = this.callbackUpdateStatus;
		if (onUpdateLockFlag == null)
		{
			return;
		}
		onUpdateLockFlag(this);
	}

	private void CallClaseWindowListener()
	{
		IconPhotoCtrl.OnUpdateLockFlag onUpdateLockFlag = this.callbackCloseWindow;
		if (onUpdateLockFlag == null)
		{
			return;
		}
		onUpdateLockFlag(this);
	}

	public void OnPointerClick()
	{
		IconPhotoCtrl.OnClick onClick = this.callbackCL;
		if (onClick == null)
		{
			return;
		}
		onClick(this);
	}

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

	[SerializeField]
	private IconPhotoCtrl.Type type;

	[SerializeField]
	private GameObject Base;

	[SerializeField]
	private GameObject Over;

	[SerializeField]
	private GameObject Top;

	[SerializeField]
	private PguiRawImageCtrl texPhoto;

	[SerializeField]
	private PguiImageCtrl imgAttribute;

	[SerializeField]
	private PguiImageCtrl imgRareFrame;

	[SerializeField]
	private PguiRawImageCtrl rawImgRareFrame;

	[SerializeField]
	private List<PguiImageCtrl> imgStar;

	[SerializeField]
	private PguiTextCtrl textCardName;

	[SerializeField]
	private PguiImageCtrl imgLock;

	[SerializeField]
	private PguiImageCtrl imgFavorite;

	[SerializeField]
	private PguiAECtrl holoAe;

	[SerializeField]
	private PguiTextCtrl textParam;

	[SerializeField]
	private List<GameObject> imgBreakthroughLimit;

	[SerializeField]
	private GameObject removeObj;

	[SerializeField]
	private PguiImageCtrl imgDisable;

	[SerializeField]
	private GameObject partyObj;

	[SerializeField]
	private PguiTextCtrl textDisable;

	[SerializeField]
	private PguiRawImageCtrl imgIconCharaMini;

	[SerializeField]
	private PguiImageCtrl imgMarkNotYetReleased;

	[SerializeField]
	private PguiAECtrl changeAe;

	[SerializeField]
	private PguiImageCtrl imgInfoPop;

	[SerializeField]
	private PguiImageCtrl imgDrop;

	[SerializeField]
	private PguiAECtrl uptoAe;

	[SerializeField]
	private PguiImageCtrl imgLockNotOwn;

	[SerializeField]
	private PguiAECtrl uptolvAe;

	private IconPhotoCtrl.OnClick callbackCL;

	private IconPhotoCtrl.OnUpdateLockFlag callbackLF;

	private IconPhotoCtrl.OnUpdateLockFlag callbackUpdateStatus;

	private IconPhotoCtrl.OnUpdateLockFlag callbackCloseWindow;

	private IconPhotoCtrl.OnUpdateFavoriteFlag callbackFF;

	public IconPhotoCtrl.OnReturnPhotoPackDataList onReturnPhotoPackDataList;

	private IconPhotoCtrl.SetupParam setupParam = new IconPhotoCtrl.SetupParam();

	public bool enablePhotoBonus;

	public int isQuestDrop;

	public enum Type
	{
		CARD,
		ICON,
		ICON_MINI
	}

	public delegate void OnClick(IconPhotoCtrl ipc);

	public delegate void OnUpdateLockFlag(IconPhotoCtrl ipc);

	public delegate void OnUpdateFavoriteFlag(IconPhotoCtrl ipc);

	public delegate List<PhotoPackData> OnReturnPhotoPackDataList();

	public class SetupParam
	{
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

		public PhotoPackData ppd;

		public SortFilterDefine.SortType sortType;

		public bool isEnableRaycast;

		public bool dispMax;

		public int playQuestOneId = -1;

		public bool isEnableLongPress;

		public bool isHelper;
	}
}
