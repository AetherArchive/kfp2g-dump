using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001C4 RID: 452
public class SortFilterBtnsAllCtrl
{
	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x06001EFE RID: 7934 RVA: 0x00180C3E File Offset: 0x0017EE3E
	// (set) Token: 0x06001EFF RID: 7935 RVA: 0x00180C46 File Offset: 0x0017EE46
	private SortFilterDefine.RegisterType RegisterType { get; set; }

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06001F00 RID: 7936 RVA: 0x00180C4F File Offset: 0x0017EE4F
	// (set) Token: 0x06001F01 RID: 7937 RVA: 0x00180C57 File Offset: 0x0017EE57
	public SortFilterDefine.SortType SortType
	{
		get
		{
			return this.sortType;
		}
		private set
		{
			this.sortType = value;
			this.SortText.text = SortFilterDefine.SortTypeDispNameMap[this.sortType];
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x06001F02 RID: 7938 RVA: 0x00180C7B File Offset: 0x0017EE7B
	// (set) Token: 0x06001F03 RID: 7939 RVA: 0x00180C83 File Offset: 0x0017EE83
	private bool Order
	{
		get
		{
			return this.order;
		}
		set
		{
			this.order = value;
			this.SortUp.SetActive(!this.order);
			this.SortDown.SetActive(this.order);
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06001F04 RID: 7940 RVA: 0x00180CB1 File Offset: 0x0017EEB1
	// (set) Token: 0x06001F05 RID: 7941 RVA: 0x00180CB9 File Offset: 0x0017EEB9
	public int SelectCharaId { get; set; }

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x06001F06 RID: 7942 RVA: 0x00180CC2 File Offset: 0x0017EEC2
	// (set) Token: 0x06001F07 RID: 7943 RVA: 0x00180CCA File Offset: 0x0017EECA
	public DataManagerCharaAccessory.Accessory GrowTargetAccessory { get; set; }

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06001F08 RID: 7944 RVA: 0x00180CD3 File Offset: 0x0017EED3
	// (set) Token: 0x06001F09 RID: 7945 RVA: 0x00180CDB File Offset: 0x0017EEDB
	public List<DataManagerCharaAccessory.Accessory> SelectAccessoryList { get; set; }

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06001F0A RID: 7946 RVA: 0x00180CE4 File Offset: 0x0017EEE4
	// (set) Token: 0x06001F0B RID: 7947 RVA: 0x00180CEC File Offset: 0x0017EEEC
	private UnityAction SortFilterChangedCallBack { get; set; }

	// Token: 0x06001F0C RID: 7948 RVA: 0x00180CF8 File Offset: 0x0017EEF8
	public SortFilterBtnsAllCtrl(SortFilterDefine.RegisterType registerType, GameObject go, UnityAction changeCallBack)
	{
		this.FilterOnOffButton = go.transform.Find("Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
		this.FilterOn = go.transform.Find("Btn_FilterOnOff/BaseImage/On").gameObject;
		this.FilterOff = go.transform.Find("Btn_FilterOnOff/BaseImage/Off").gameObject;
		this.SortButton = go.transform.Find("Btn_Sort").GetComponent<PguiButtonCtrl>();
		this.SortText = go.transform.Find("Btn_Sort/BaseImage/Txt_btn").GetComponent<PguiTextCtrl>();
		this.SortUpDown = go.transform.Find("Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
		this.SortUp = go.transform.Find("Btn_SortUpDown/BaseImage/Img_Up").gameObject;
		this.SortDown = go.transform.Find("Btn_SortUpDown/BaseImage/Img_Down").gameObject;
		this.RegisterType = registerType;
		DataManagerGameStatus.UserFlagData.SortTypeData activeSortTypeData = SortFilterManager.GetActiveSortTypeData(this.RegisterType);
		this.SortType = activeSortTypeData.SortType;
		this.Order = activeSortTypeData.Order;
		this.UpdateFilterButton();
		this.SortFilterChangedCallBack = changeCallBack;
		this.FilterOnOffButton.AddOnClickListener(delegate(PguiButtonCtrl x)
		{
			CanvasManager.HdlAccessoryFilterWindow.Open(this.RegisterType, new UnityAction<bool, AccessorySortFilter.FilterStatus>(this.OnFilterChanged));
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.SortButton.AddOnClickListener(delegate(PguiButtonCtrl x)
		{
			CanvasManager.HdlAccessorySortWindow.Open(this.RegisterType, new UnityAction<bool, SortFilterDefine.SortType>(this.OnSortChanged));
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.SortUpDown.AddOnClickListener(delegate(PguiButtonCtrl x)
		{
			this.Order = !this.Order;
			this.SortFilterChanged();
		}, PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x00180E64 File Offset: 0x0017F064
	private void OnFilterChanged(bool changed, AccessorySortFilter.FilterStatus filterStatus)
	{
		SortFilterDefine.RegisterType registerType = this.RegisterType;
		if (registerType - SortFilterDefine.RegisterType.ACCESSORY_ALL <= 4 && changed)
		{
			SortFilterManager.Accessory.SetFilterStatus(this.RegisterType, filterStatus);
			this.UpdateFilterButton();
			this.SortFilterChanged();
		}
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x00180E9F File Offset: 0x0017F09F
	private void OnSortChanged(bool changed, SortFilterDefine.SortType st)
	{
		if (changed)
		{
			this.SortType = st;
			this.SortFilterChanged();
		}
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x00180EB4 File Offset: 0x0017F0B4
	private void UpdateFilterButton()
	{
		AccessorySortFilter.FilterStatus filterStatus = SortFilterManager.Accessory.GetFilterStatus(this.RegisterType);
		int num = filterStatus.SelectRarityList.Count + filterStatus.SelectDispTypeList.Count + filterStatus.SelectOwnerStatusList.Count + filterStatus.SearchText.Length;
		if (0 < num)
		{
			this.FilterOn.SetActive(true);
			this.FilterOff.SetActive(false);
			return;
		}
		this.FilterOn.SetActive(false);
		this.FilterOff.SetActive(true);
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x00180F38 File Offset: 0x0017F138
	private void SortFilterChanged()
	{
		DataManagerGameStatus.UserFlagData.SortTypeData sortTypeData = new DataManagerGameStatus.UserFlagData.SortTypeData(this.RegisterType, this.sortType, this.Order);
		SortFilterManager.UpdateSortFilterData(this.RegisterType, sortTypeData);
		UnityAction sortFilterChangedCallBack = this.SortFilterChangedCallBack;
		if (sortFilterChangedCallBack == null)
		{
			return;
		}
		sortFilterChangedCallBack();
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x00180F7C File Offset: 0x0017F17C
	public List<DataManagerCharaAccessory.Accessory> GetSortFilteredAccessoryList()
	{
		List<DataManagerCharaAccessory.Accessory> list;
		switch (this.RegisterType)
		{
		case SortFilterDefine.RegisterType.ACCESSORY_ALL:
			list = SortFilterManager.Accessory.SortFilteredAccessoryList(this.RegisterType, this.SortType, this.Order, this.SelectAccessoryList);
			break;
		case SortFilterDefine.RegisterType.ACCESSORY_GROW_BASE:
		{
			List<DataManagerCharaAccessory.Accessory> list2 = SortFilterManager.Accessory.SortFilteredAccessoryList(this.RegisterType, this.SortType, this.Order, this.SelectAccessoryList);
			List<DataManagerCharaAccessory.Accessory> list3 = new List<DataManagerCharaAccessory.Accessory>();
			List<DataManagerCharaAccessory.Accessory> list4 = new List<DataManagerCharaAccessory.Accessory>();
			foreach (DataManagerCharaAccessory.Accessory accessory in list2)
			{
				if (accessory.Level < accessory.AccessoryData.Rarity.LevelLimit && accessory.AccessoryData.LevelupNum == 0)
				{
					list3.Add(accessory);
				}
				else
				{
					list4.Add(accessory);
				}
			}
			list = new List<DataManagerCharaAccessory.Accessory>();
			list.AddRange(list3);
			list.AddRange(list4);
			break;
		}
		case SortFilterDefine.RegisterType.ACCESSORY_GROW_MATERIAL:
		{
			List<DataManagerCharaAccessory.Accessory> list5 = SortFilterManager.Accessory.SortFilteredAccessoryList(this.RegisterType, this.SortType, this.Order, this.SelectAccessoryList);
			List<DataManagerCharaAccessory.Accessory> list6 = new List<DataManagerCharaAccessory.Accessory>();
			List<DataManagerCharaAccessory.Accessory> list7 = new List<DataManagerCharaAccessory.Accessory>();
			int num = ((this.GrowTargetAccessory == null) ? 0 : this.GrowTargetAccessory.AccessoryData.ItemId);
			long num2 = ((this.GrowTargetAccessory == null) ? 0L : this.GrowTargetAccessory.UniqId);
			int num3 = ((this.GrowTargetAccessory == null) ? 0 : this.GrowTargetAccessory.AccessoryData.Rarity.Rarity);
			foreach (DataManagerCharaAccessory.Accessory accessory2 in list5)
			{
				if (accessory2.IsLock)
				{
					list7.Add(accessory2);
				}
				else if (num2 == accessory2.UniqId)
				{
					list7.Add(accessory2);
				}
				else if (num == accessory2.AccessoryData.ItemId)
				{
					list6.Add(accessory2);
				}
				else if (0 < accessory2.AccessoryData.LevelupNum && num3 <= accessory2.AccessoryData.Rarity.Rarity)
				{
					list6.Add(accessory2);
				}
				else
				{
					list7.Add(accessory2);
				}
			}
			list = new List<DataManagerCharaAccessory.Accessory>();
			list.AddRange(list6);
			list.AddRange(list7);
			break;
		}
		case SortFilterDefine.RegisterType.ACCESSORY_SELL:
		{
			List<DataManagerCharaAccessory.Accessory> list8 = SortFilterManager.Accessory.SortFilteredAccessoryList(this.RegisterType, this.SortType, this.Order, this.SelectAccessoryList);
			List<DataManagerCharaAccessory.Accessory> list9 = new List<DataManagerCharaAccessory.Accessory>();
			List<DataManagerCharaAccessory.Accessory> list10 = new List<DataManagerCharaAccessory.Accessory>();
			foreach (DataManagerCharaAccessory.Accessory accessory3 in list8)
			{
				if (!accessory3.IsLock)
				{
					list9.Add(accessory3);
				}
				else
				{
					list10.Add(accessory3);
				}
			}
			list = new List<DataManagerCharaAccessory.Accessory>();
			list.AddRange(list9);
			list.AddRange(list10);
			break;
		}
		case SortFilterDefine.RegisterType.ACCESSORY_EQUIP:
		{
			List<DataManagerCharaAccessory.Accessory> list11 = SortFilterManager.Accessory.SortFilteredAccessoryList(this.RegisterType, this.SortType, this.Order, this.SelectAccessoryList);
			list11.RemoveAll((DataManagerCharaAccessory.Accessory item) => !AccessoryUtil.CanEquipped(item));
			list11.RemoveAll((DataManagerCharaAccessory.Accessory item) => item.CharaId != 0 && item.CharaId != this.SelectCharaId);
			List<DataManagerCharaAccessory.Accessory> list12 = new List<DataManagerCharaAccessory.Accessory>();
			List<DataManagerCharaAccessory.Accessory> list13 = new List<DataManagerCharaAccessory.Accessory>();
			foreach (DataManagerCharaAccessory.Accessory accessory4 in list11)
			{
				if (accessory4.CharaId != 0)
				{
					list12.Add(accessory4);
				}
				else
				{
					list13.Add(accessory4);
				}
			}
			list = new List<DataManagerCharaAccessory.Accessory>();
			list.AddRange(list12);
			list.AddRange(list13);
			break;
		}
		default:
			list = new List<DataManagerCharaAccessory.Accessory>();
			break;
		}
		return list;
	}

	// Token: 0x06001F12 RID: 7954 RVA: 0x00181370 File Offset: 0x0017F570
	public void RequestUpdateSortData()
	{
		SortFilterManager.RequestUpdateSortTypeData();
	}

	// Token: 0x0400169B RID: 5787
	private PguiButtonCtrl FilterOnOffButton;

	// Token: 0x0400169C RID: 5788
	private GameObject FilterOn;

	// Token: 0x0400169D RID: 5789
	private GameObject FilterOff;

	// Token: 0x0400169E RID: 5790
	private PguiButtonCtrl SortButton;

	// Token: 0x0400169F RID: 5791
	private PguiTextCtrl SortText;

	// Token: 0x040016A0 RID: 5792
	private PguiButtonCtrl SortUpDown;

	// Token: 0x040016A1 RID: 5793
	private GameObject SortUp;

	// Token: 0x040016A2 RID: 5794
	private GameObject SortDown;

	// Token: 0x040016A3 RID: 5795
	private AccessorySortWindowCtrl ASWC;

	// Token: 0x040016A5 RID: 5797
	private SortFilterDefine.SortType sortType;

	// Token: 0x040016A6 RID: 5798
	private bool order;
}
