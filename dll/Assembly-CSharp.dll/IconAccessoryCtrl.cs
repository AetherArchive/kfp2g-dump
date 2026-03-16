using System;
using System.Collections.Generic;
using UnityEngine;

public class IconAccessoryCtrl : MonoBehaviour
{
	public PguiRawImageCtrl TexAccessory
	{
		get
		{
			return this.texAccessory;
		}
	}

	private bool IsEnableLongPress
	{
		get
		{
			return this.setupParam.isEnableLongPress && this.setupParam.isEnableRaycast;
		}
	}

	public DataManagerCharaAccessory.Accessory accessory { get; private set; }

	public DataManagerCharaAccessory.AccessoryData accessoryData { get; private set; }

	public void Setup(IconAccessoryCtrl.SetupParam param)
	{
		this.setupParam = param;
		this.accessory = param.acce;
		if (param.acce != null)
		{
			this.SetupByAccessoryData(param.acce.AccessoryData, param.isEnableRaycast, false);
			this.DispTextParam(true);
			this.SetupTexParam(param.acce, param.sortType);
			return;
		}
		base.gameObject.SetActive(false);
	}

	public void SetupByAccessoryData(DataManagerCharaAccessory.AccessoryData acceData, bool isEnableRaycast = true, bool dispMax = false)
	{
		this.accessoryData = acceData;
		this.dispLvMaxDetail = dispMax;
		if (acceData != null)
		{
			base.gameObject.SetActive(true);
			this.SetupAccessoryImage(isEnableRaycast);
			this.SetupRareFrame();
			this.SetupStar();
			this.SetupImgLock();
			this.DispBase(true);
			this.DispTop(true);
			this.DispIconCharaMini(false, null);
			this.DispMarkNotYetReleased(false);
			this.DispImgDisable(false);
			this.DispParty(false, true);
			this.DispRemove(false);
			this.DispTextDisable(false, null, null);
			this.DispTextParam(false);
			this.DispLockNotOwn(false);
			return;
		}
		base.gameObject.SetActive(false);
	}

	private void SetupAccessoryImage(bool isEnableRaycast)
	{
		if (this.imgBase != null && this.accessoryData != null)
		{
			this.imgBase.Replace(this.accessoryData.Rarity.Rarity);
		}
		if (this.texAccessory != null)
		{
			if (this.accessoryData != null)
			{
				string text = string.Empty;
				IconAccessoryCtrl.Type type = this.type;
				if (type <= IconAccessoryCtrl.Type.ICON_MINI)
				{
					text = this.accessoryData.IconPath;
				}
				this.texAccessory.SetRawImage(text, true, false, null);
			}
			this.texAccessory.m_RawImage.raycastTarget = isEnableRaycast && (!AccessoryUtil.IsInvalid(this.accessory) || this.dispLvMaxDetail);
		}
	}

	private void SetupRareFrame()
	{
		if (this.imgRareFrame != null)
		{
			string text = string.Empty;
			IconAccessoryCtrl.Type type = this.type;
			if (type != IconAccessoryCtrl.Type.ICON)
			{
				if (type == IconAccessoryCtrl.Type.ICON_MINI)
				{
					text = "icon_photomini_frame0" + this.accessoryData.Rarity.Rarity.ToString();
				}
			}
			else
			{
				text = "icon_photo_frame0" + this.accessoryData.Rarity.Rarity.ToString();
			}
			this.imgRareFrame.SetImageByName(text);
		}
	}

	private void SetupStar()
	{
		if (this.imgStar != null)
		{
			for (int i = 0; i < this.imgStar.Count; i++)
			{
				this.imgStar[i].gameObject.SetActive(i < this.accessoryData.Rarity.Rarity);
			}
		}
	}

	private void SetupImgLock()
	{
		if (this.imgLock != null)
		{
			this.imgLock.m_Image.enabled = this.accessory != null && this.accessory.IsLock;
		}
	}

	private void SetupTexParam(DataManagerCharaAccessory.Accessory acce, SortFilterDefine.SortType sortType)
	{
		if (this.textParam != null)
		{
			this.textParam.m_Text.enabled = true;
			switch (sortType)
			{
			case SortFilterDefine.SortType.LEVEL:
			case SortFilterDefine.SortType.RARITY:
				this.textParam.text = AccessoryUtil.MakeLevelString(acce, true);
				return;
			case SortFilterDefine.SortType.HP:
				this.textParam.text = acce.Param.Hp.ToString();
				return;
			case SortFilterDefine.SortType.ATK:
				this.textParam.text = acce.Param.Atk.ToString();
				return;
			case SortFilterDefine.SortType.DEF:
				this.textParam.text = acce.Param.Def.ToString();
				return;
			case SortFilterDefine.SortType.NEW:
				this.textParam.text = TimeManager.FormattedTime(acce.GetTime, TimeManager.Format.yyyyMMdd);
				return;
			case SortFilterDefine.SortType.AVOIDANCE:
				this.textParam.text = AccessoryUtil.GetPermillageText(acce.Param.Avoid);
				return;
			case SortFilterDefine.SortType.BEAT_DAMAGE:
				this.textParam.text = AccessoryUtil.GetPermillageText(acce.Param.Beat);
				return;
			case SortFilterDefine.SortType.ACTION_DAMAGE:
				this.textParam.text = AccessoryUtil.GetPermillageText(acce.Param.Action);
				return;
			case SortFilterDefine.SortType.TRY_DAMAGE:
				this.textParam.text = AccessoryUtil.GetPermillageText(acce.Param.Try);
				return;
			}
			this.textParam.m_Text.enabled = false;
		}
	}

	public void DispLockIcon(bool flag)
	{
		if (this.imgLock != null)
		{
			this.imgLock.m_Image.enabled = flag;
		}
	}

	public bool FrameDisp
	{
		get
		{
			return this.imgRareFrame != null && this.imgRareFrame.gameObject.activeSelf;
		}
		set
		{
			if (this.imgRareFrame != null)
			{
				this.imgRareFrame.gameObject.SetActive(value);
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
		if (this.removeObj == null)
		{
			return;
		}
		this.removeObj.SetActive(flag);
		if (flag)
		{
			this.DispBase(false);
			this.DispTop(false);
			this.DispOver(true);
			base.gameObject.SetActive(flag);
			this.DispIconCharaMini(false, null);
			this.DispMarkNotYetReleased(false);
			this.DispTextDisable(false, null, null);
			this.DispParty(false, true);
			this.DispImgDisable(false);
		}
	}

	public void DispImgDisable(bool flag)
	{
		if (this.imgDisable != null)
		{
			this.imgDisable.gameObject.SetActive(flag);
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
			this.imgIconCharaMini.transform.parent.GetComponent<SimpleAnimation>().ExInit();
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

	public void DispTextParam(bool flag)
	{
		if (this.textParam != null)
		{
			this.textParam.gameObject.SetActive(flag);
		}
	}

	public void DispLockNotOwn(bool flag)
	{
		if (this.imgLockNotOwn != null)
		{
			this.imgLockNotOwn.gameObject.SetActive(flag);
		}
	}

	public void SetupNoEquipIcon(DataManagerCharaAccessory.Accessory accessory)
	{
		if (AccessoryUtil.IsInvalid(accessory))
		{
			base.gameObject.SetActive(true);
			this.DispBase(false);
			this.DispTop(true);
			this.DispIconCharaMini(false, null);
			this.DispMarkNotYetReleased(false);
			this.DispImgDisable(false);
			this.DispParty(false, true);
			this.DispTextDisable(false, null, null);
			this.DispTextParam(false);
			this.DispLockNotOwn(false);
		}
	}

	public void DispRarity(bool flag)
	{
		if (this.imgStar != null)
		{
			for (int i = 0; i < this.imgStar.Count; i++)
			{
				this.imgStar[i].gameObject.SetActive(flag && i < this.accessory.AccessoryData.Rarity.Rarity);
			}
		}
	}

	public void AddOnClickListener(IconAccessoryCtrl.OnClick callback)
	{
		this.callbackCL = callback;
	}

	public void AddOnUpdateStatus(IconAccessoryCtrl.OnUpdateLockFlag callback)
	{
		this.callbackUpdateStatus = callback;
	}

	public void AddOnCloseWindow(IconAccessoryCtrl.OnUpdateLockFlag callback)
	{
		this.callbackCloseWindow = callback;
	}

	private void CallUpdateStatusListener()
	{
		IconAccessoryCtrl.OnUpdateLockFlag onUpdateLockFlag = this.callbackUpdateStatus;
		if (onUpdateLockFlag == null)
		{
			return;
		}
		onUpdateLockFlag(this);
	}

	private void CallCloseWindowListener()
	{
		IconAccessoryCtrl.OnUpdateLockFlag onUpdateLockFlag = this.callbackCloseWindow;
		if (onUpdateLockFlag == null)
		{
			return;
		}
		onUpdateLockFlag(this);
	}

	public void OnPointerClick()
	{
		IconAccessoryCtrl.OnClick onClick = this.callbackCL;
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
		if (this.accessory != null)
		{
			AccessoryWindowCtrl hdlAccessoryWindowCtrl = CanvasManager.HdlAccessoryWindowCtrl;
			AccessoryWindowCtrl.SetupParam setupParam = new AccessoryWindowCtrl.SetupParam();
			setupParam.acce = this.accessory;
			List<DataManagerCharaAccessory.Accessory> list;
			if (this.onReturnAccessoryList == null)
			{
				list = null;
			}
			else
			{
				list = this.onReturnAccessoryList().FindAll((DataManagerCharaAccessory.Accessory item) => item != null);
			}
			setupParam.acceList = list;
			setupParam.updateStasusEndCb = delegate
			{
				this.CallUpdateStatusListener();
			};
			setupParam.windowCloseEndCb = delegate
			{
				this.CallCloseWindowListener();
			};
			hdlAccessoryWindowCtrl.Open(setupParam);
			return;
		}
		if (this.accessoryData != null && this.dispLvMaxDetail)
		{
			CanvasManager.HdlAccessoryWindowCtrl.Open(new AccessoryWindowCtrl.SetupParam
			{
				dispMaxAccessoryId = this.accessoryData.ItemId
			});
		}
	}

	[SerializeField]
	private IconAccessoryCtrl.Type type;

	[SerializeField]
	private GameObject Base;

	[SerializeField]
	private GameObject Over;

	[SerializeField]
	private GameObject Top;

	[SerializeField]
	private PguiReplaceSpriteCtrl imgBase;

	[SerializeField]
	private PguiRawImageCtrl texAccessory;

	[SerializeField]
	private PguiImageCtrl imgRareFrame;

	[SerializeField]
	private List<PguiImageCtrl> imgStar;

	[SerializeField]
	private PguiImageCtrl imgLock;

	[SerializeField]
	private PguiTextCtrl textParam;

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
	private PguiImageCtrl imgLockNotOwn;

	private IconAccessoryCtrl.OnClick callbackCL;

	private IconAccessoryCtrl.OnUpdateLockFlag callbackLF;

	private IconAccessoryCtrl.OnUpdateLockFlag callbackUpdateStatus;

	private IconAccessoryCtrl.OnUpdateLockFlag callbackCloseWindow;

	private IconAccessoryCtrl.SetupParam setupParam = new IconAccessoryCtrl.SetupParam();

	public IconAccessoryCtrl.OnReturnAccessoryList onReturnAccessoryList;

	private bool dispLvMaxDetail;

	public enum Type
	{
		ICON,
		ICON_MINI
	}

	public delegate void OnClick(IconAccessoryCtrl iac);

	public delegate void OnUpdateLockFlag(IconAccessoryCtrl iac);

	public delegate List<DataManagerCharaAccessory.Accessory> OnReturnAccessoryList();

	public class SetupParam
	{
		public SetupParam()
		{
			this.acce = null;
			this.sortType = SortFilterDefine.SortType.LEVEL;
			this.isEnableRaycast = true;
			this.isEnableLongPress = true;
		}

		public DataManagerCharaAccessory.Accessory acce;

		public SortFilterDefine.SortType sortType;

		public bool isEnableRaycast;

		public bool isEnableLongPress;
	}
}
