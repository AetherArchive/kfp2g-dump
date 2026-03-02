using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class IconAccessoryCtrl : MonoBehaviour
{
	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x00163958 File Offset: 0x00161B58
	public PguiRawImageCtrl TexAccessory
	{
		get
		{
			return this.texAccessory;
		}
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06001BFA RID: 7162 RVA: 0x00163960 File Offset: 0x00161B60
	private bool IsEnableLongPress
	{
		get
		{
			return this.setupParam.isEnableLongPress && this.setupParam.isEnableRaycast;
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x06001BFB RID: 7163 RVA: 0x0016397C File Offset: 0x00161B7C
	// (set) Token: 0x06001BFC RID: 7164 RVA: 0x00163984 File Offset: 0x00161B84
	public DataManagerCharaAccessory.Accessory accessory { get; private set; }

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x06001BFD RID: 7165 RVA: 0x0016398D File Offset: 0x00161B8D
	// (set) Token: 0x06001BFE RID: 7166 RVA: 0x00163995 File Offset: 0x00161B95
	public DataManagerCharaAccessory.AccessoryData accessoryData { get; private set; }

	// Token: 0x06001BFF RID: 7167 RVA: 0x001639A0 File Offset: 0x00161BA0
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

	// Token: 0x06001C00 RID: 7168 RVA: 0x00163A08 File Offset: 0x00161C08
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

	// Token: 0x06001C01 RID: 7169 RVA: 0x00163AA4 File Offset: 0x00161CA4
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

	// Token: 0x06001C02 RID: 7170 RVA: 0x00163B50 File Offset: 0x00161D50
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

	// Token: 0x06001C03 RID: 7171 RVA: 0x00163BD8 File Offset: 0x00161DD8
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

	// Token: 0x06001C04 RID: 7172 RVA: 0x00163C2C File Offset: 0x00161E2C
	private void SetupImgLock()
	{
		if (this.imgLock != null)
		{
			this.imgLock.m_Image.enabled = this.accessory != null && this.accessory.IsLock;
		}
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x00163C64 File Offset: 0x00161E64
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

	// Token: 0x06001C06 RID: 7174 RVA: 0x00163DF5 File Offset: 0x00161FF5
	public void DispLockIcon(bool flag)
	{
		if (this.imgLock != null)
		{
			this.imgLock.m_Image.enabled = flag;
		}
	}

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x06001C08 RID: 7176 RVA: 0x00163E37 File Offset: 0x00162037
	// (set) Token: 0x06001C07 RID: 7175 RVA: 0x00163E16 File Offset: 0x00162016
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

	// Token: 0x06001C09 RID: 7177 RVA: 0x00163E59 File Offset: 0x00162059
	public void DispTop(bool flag)
	{
		if (this.Top != null)
		{
			this.Top.SetActive(flag);
		}
	}

	// Token: 0x06001C0A RID: 7178 RVA: 0x00163E75 File Offset: 0x00162075
	public void DispOver(bool flag)
	{
		if (this.Over != null)
		{
			this.Over.SetActive(flag);
		}
	}

	// Token: 0x06001C0B RID: 7179 RVA: 0x00163E91 File Offset: 0x00162091
	public void DispBase(bool flag)
	{
		if (this.Base != null)
		{
			this.Base.SetActive(flag);
		}
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x00163EB0 File Offset: 0x001620B0
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

	// Token: 0x06001C0D RID: 7181 RVA: 0x00163F23 File Offset: 0x00162123
	public void DispImgDisable(bool flag)
	{
		if (this.imgDisable != null)
		{
			this.imgDisable.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x00163F44 File Offset: 0x00162144
	public bool CheckImgDisable()
	{
		return this.imgDisable != null && this.imgDisable.gameObject.activeSelf;
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x00163F66 File Offset: 0x00162166
	public void DispParty(bool flag, bool isEnabelTxt = true)
	{
		if (this.partyObj != null)
		{
			this.partyObj.SetActive(flag);
			this.partyObj.transform.Find("Fnt_party").gameObject.SetActive(isEnabelTxt);
		}
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x00163FA4 File Offset: 0x001621A4
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

	// Token: 0x06001C11 RID: 7185 RVA: 0x00164004 File Offset: 0x00162204
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

	// Token: 0x06001C12 RID: 7186 RVA: 0x00164070 File Offset: 0x00162270
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

	// Token: 0x06001C13 RID: 7187 RVA: 0x001640F3 File Offset: 0x001622F3
	public void DispMarkNotYetReleased(bool flag)
	{
		if (this.imgMarkNotYetReleased != null)
		{
			this.imgMarkNotYetReleased.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x00164114 File Offset: 0x00162314
	public void DispTextParam(bool flag)
	{
		if (this.textParam != null)
		{
			this.textParam.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x00164135 File Offset: 0x00162335
	public void DispLockNotOwn(bool flag)
	{
		if (this.imgLockNotOwn != null)
		{
			this.imgLockNotOwn.gameObject.SetActive(flag);
		}
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x00164158 File Offset: 0x00162358
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

	// Token: 0x06001C17 RID: 7191 RVA: 0x001641BC File Offset: 0x001623BC
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

	// Token: 0x06001C18 RID: 7192 RVA: 0x0016421B File Offset: 0x0016241B
	public void AddOnClickListener(IconAccessoryCtrl.OnClick callback)
	{
		this.callbackCL = callback;
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x00164224 File Offset: 0x00162424
	public void AddOnUpdateStatus(IconAccessoryCtrl.OnUpdateLockFlag callback)
	{
		this.callbackUpdateStatus = callback;
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x0016422D File Offset: 0x0016242D
	public void AddOnCloseWindow(IconAccessoryCtrl.OnUpdateLockFlag callback)
	{
		this.callbackCloseWindow = callback;
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x00164236 File Offset: 0x00162436
	private void CallUpdateStatusListener()
	{
		IconAccessoryCtrl.OnUpdateLockFlag onUpdateLockFlag = this.callbackUpdateStatus;
		if (onUpdateLockFlag == null)
		{
			return;
		}
		onUpdateLockFlag(this);
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x00164249 File Offset: 0x00162449
	private void CallCloseWindowListener()
	{
		IconAccessoryCtrl.OnUpdateLockFlag onUpdateLockFlag = this.callbackCloseWindow;
		if (onUpdateLockFlag == null)
		{
			return;
		}
		onUpdateLockFlag(this);
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x0016425C File Offset: 0x0016245C
	public void OnPointerClick()
	{
		IconAccessoryCtrl.OnClick onClick = this.callbackCL;
		if (onClick == null)
		{
			return;
		}
		onClick(this);
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x00164270 File Offset: 0x00162470
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

	// Token: 0x040014D6 RID: 5334
	[SerializeField]
	private IconAccessoryCtrl.Type type;

	// Token: 0x040014D7 RID: 5335
	[SerializeField]
	private GameObject Base;

	// Token: 0x040014D8 RID: 5336
	[SerializeField]
	private GameObject Over;

	// Token: 0x040014D9 RID: 5337
	[SerializeField]
	private GameObject Top;

	// Token: 0x040014DA RID: 5338
	[SerializeField]
	private PguiReplaceSpriteCtrl imgBase;

	// Token: 0x040014DB RID: 5339
	[SerializeField]
	private PguiRawImageCtrl texAccessory;

	// Token: 0x040014DC RID: 5340
	[SerializeField]
	private PguiImageCtrl imgRareFrame;

	// Token: 0x040014DD RID: 5341
	[SerializeField]
	private List<PguiImageCtrl> imgStar;

	// Token: 0x040014DE RID: 5342
	[SerializeField]
	private PguiImageCtrl imgLock;

	// Token: 0x040014DF RID: 5343
	[SerializeField]
	private PguiTextCtrl textParam;

	// Token: 0x040014E0 RID: 5344
	[SerializeField]
	private GameObject removeObj;

	// Token: 0x040014E1 RID: 5345
	[SerializeField]
	private PguiImageCtrl imgDisable;

	// Token: 0x040014E2 RID: 5346
	[SerializeField]
	private GameObject partyObj;

	// Token: 0x040014E3 RID: 5347
	[SerializeField]
	private PguiTextCtrl textDisable;

	// Token: 0x040014E4 RID: 5348
	[SerializeField]
	private PguiRawImageCtrl imgIconCharaMini;

	// Token: 0x040014E5 RID: 5349
	[SerializeField]
	private PguiImageCtrl imgMarkNotYetReleased;

	// Token: 0x040014E6 RID: 5350
	[SerializeField]
	private PguiImageCtrl imgLockNotOwn;

	// Token: 0x040014E7 RID: 5351
	private IconAccessoryCtrl.OnClick callbackCL;

	// Token: 0x040014E8 RID: 5352
	private IconAccessoryCtrl.OnUpdateLockFlag callbackLF;

	// Token: 0x040014E9 RID: 5353
	private IconAccessoryCtrl.OnUpdateLockFlag callbackUpdateStatus;

	// Token: 0x040014EA RID: 5354
	private IconAccessoryCtrl.OnUpdateLockFlag callbackCloseWindow;

	// Token: 0x040014EB RID: 5355
	private IconAccessoryCtrl.SetupParam setupParam = new IconAccessoryCtrl.SetupParam();

	// Token: 0x040014EC RID: 5356
	public IconAccessoryCtrl.OnReturnAccessoryList onReturnAccessoryList;

	// Token: 0x040014EF RID: 5359
	private bool dispLvMaxDetail;

	// Token: 0x02000EF8 RID: 3832
	public enum Type
	{
		// Token: 0x04005578 RID: 21880
		ICON,
		// Token: 0x04005579 RID: 21881
		ICON_MINI
	}

	// Token: 0x02000EF9 RID: 3833
	// (Invoke) Token: 0x06004E47 RID: 20039
	public delegate void OnClick(IconAccessoryCtrl iac);

	// Token: 0x02000EFA RID: 3834
	// (Invoke) Token: 0x06004E4B RID: 20043
	public delegate void OnUpdateLockFlag(IconAccessoryCtrl iac);

	// Token: 0x02000EFB RID: 3835
	// (Invoke) Token: 0x06004E4F RID: 20047
	public delegate List<DataManagerCharaAccessory.Accessory> OnReturnAccessoryList();

	// Token: 0x02000EFC RID: 3836
	public class SetupParam
	{
		// Token: 0x06004E52 RID: 20050 RVA: 0x002366BA File Offset: 0x002348BA
		public SetupParam()
		{
			this.acce = null;
			this.sortType = SortFilterDefine.SortType.LEVEL;
			this.isEnableRaycast = true;
			this.isEnableLongPress = true;
		}

		// Token: 0x0400557A RID: 21882
		public DataManagerCharaAccessory.Accessory acce;

		// Token: 0x0400557B RID: 21883
		public SortFilterDefine.SortType sortType;

		// Token: 0x0400557C RID: 21884
		public bool isEnableRaycast;

		// Token: 0x0400557D RID: 21885
		public bool isEnableLongPress;
	}
}
