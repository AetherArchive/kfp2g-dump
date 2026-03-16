using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SortFilterBtnsAllCtrl
{
	private SortFilterDefine.RegisterType RegisterType { get; set; }

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

	public int SelectCharaId { get; set; }

	public DataManagerCharaAccessory.Accessory GrowTargetAccessory { get; set; }

	public List<DataManagerCharaAccessory.Accessory> SelectAccessoryList { get; set; }

	private UnityAction SortFilterChangedCallBack { get; set; }

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

	private void OnSortChanged(bool changed, SortFilterDefine.SortType st)
	{
		if (changed)
		{
			this.SortType = st;
			this.SortFilterChanged();
		}
	}

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

	public void RequestUpdateSortData()
	{
		SortFilterManager.RequestUpdateSortTypeData();
	}

	private PguiButtonCtrl FilterOnOffButton;

	private GameObject FilterOn;

	private GameObject FilterOff;

	private PguiButtonCtrl SortButton;

	private PguiTextCtrl SortText;

	private PguiButtonCtrl SortUpDown;

	private GameObject SortUp;

	private GameObject SortDown;

	private AccessorySortWindowCtrl ASWC;

	private SortFilterDefine.SortType sortType;

	private bool order;
}
