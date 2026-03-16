using System;
using System.Collections.Generic;
using System.Linq;

public class AccessorySortFilter
{
	private Dictionary<SortFilterDefine.RegisterType, AccessorySortFilter.FilterStatus> FilterStatusMap { get; set; }

	public SortFilterDefine.RegisterType LatestSortWindowRegisterType { get; set; }

	public SortFilterDefine.SortType LatestSortWindowSortType { get; set; }

	public AccessorySortFilter()
	{
		this.FilterStatusMap = new Dictionary<SortFilterDefine.RegisterType, AccessorySortFilter.FilterStatus>();
	}

	public AccessorySortFilter.FilterStatus GetFilterStatus(SortFilterDefine.RegisterType registerType)
	{
		if (!this.FilterStatusMap.ContainsKey(registerType))
		{
			this.FilterStatusMap.Add(registerType, new AccessorySortFilter.FilterStatus());
		}
		return this.FilterStatusMap[registerType];
	}

	public void SetFilterStatus(SortFilterDefine.RegisterType registerType, AccessorySortFilter.FilterStatus filterStatus)
	{
		if (!this.FilterStatusMap.ContainsKey(registerType))
		{
			this.FilterStatusMap.Add(registerType, new AccessorySortFilter.FilterStatus());
		}
		this.FilterStatusMap[registerType] = filterStatus;
	}

	public List<DataManagerCharaAccessory.Accessory> SortFilteredAccessoryList(SortFilterDefine.RegisterType registerType, SortFilterDefine.SortType sortType, bool order, List<DataManagerCharaAccessory.Accessory> selectAccList)
	{
		List<DataManagerCharaAccessory.Accessory> list = DataManager.DmChAccessory.GetUserAccessoryList();
		if (selectAccList != null && 0 < selectAccList.Count)
		{
			list.RemoveAll((DataManagerCharaAccessory.Accessory x) => selectAccList.Contains(x));
		}
		AccessorySortFilter.FilterStatus filterStatus = this.GetFilterStatus(registerType);
		if (0 < filterStatus.SearchText.Length)
		{
			list = list.Where<DataManagerCharaAccessory.Accessory>((DataManagerCharaAccessory.Accessory accessory) => accessory.AccessoryData.Name.Contains(filterStatus.SearchText)).ToList<DataManagerCharaAccessory.Accessory>();
		}
		if (0 < filterStatus.SelectRarityList.Count)
		{
			list.RemoveAll((DataManagerCharaAccessory.Accessory x) => !filterStatus.SelectRarityList.Contains((ItemDef.Rarity)x.AccessoryData.Rarity.Rarity));
		}
		if (0 < filterStatus.SelectDispTypeList.Count)
		{
			list.RemoveAll((DataManagerCharaAccessory.Accessory x) => !filterStatus.SelectDispTypeList.Contains(x.AccessoryData.DispType));
		}
		if (0 < filterStatus.SelectOwnerStatusList.Count)
		{
			list.RemoveAll((DataManagerCharaAccessory.Accessory x) => !filterStatus.SelectOwnerStatusList.Contains(x.CharaId != 0));
		}
		if (selectAccList != null && 0 < selectAccList.Count)
		{
			list.AddRange(selectAccList);
		}
		this.AccSort(ref list, sortType, order);
		return list;
	}

	private void AccSort(ref List<DataManagerCharaAccessory.Accessory> list, SortFilterDefine.SortType sortType, bool order)
	{
		Comparison<DataManagerCharaAccessory.Accessory> comparison = delegate(DataManagerCharaAccessory.Accessory a, DataManagerCharaAccessory.Accessory b)
		{
			int num = a.ItemId.CompareTo(b.ItemId);
			int num2 = a.UniqId.CompareTo(b.UniqId);
			int num3 = a.IsLock.CompareTo(b.IsLock);
			if (num != 0)
			{
				return num;
			}
			if (num3 == 0)
			{
				return num2;
			}
			return num3;
		};
		PrjUtil.InsertionSort<DataManagerCharaAccessory.Accessory>(ref list, comparison);
		switch (sortType)
		{
		case SortFilterDefine.SortType.LEVEL:
			PrjUtil.InsertionSort<DataManagerCharaAccessory.Accessory>(ref list, (DataManagerCharaAccessory.Accessory a, DataManagerCharaAccessory.Accessory b) => a.Level - b.Level);
			break;
		case SortFilterDefine.SortType.HP:
			PrjUtil.InsertionSort<DataManagerCharaAccessory.Accessory>(ref list, (DataManagerCharaAccessory.Accessory a, DataManagerCharaAccessory.Accessory b) => a.Param.Hp - b.Param.Hp);
			break;
		case SortFilterDefine.SortType.ATK:
			PrjUtil.InsertionSort<DataManagerCharaAccessory.Accessory>(ref list, (DataManagerCharaAccessory.Accessory a, DataManagerCharaAccessory.Accessory b) => a.Param.Atk - b.Param.Atk);
			break;
		case SortFilterDefine.SortType.DEF:
			PrjUtil.InsertionSort<DataManagerCharaAccessory.Accessory>(ref list, (DataManagerCharaAccessory.Accessory a, DataManagerCharaAccessory.Accessory b) => a.Param.Def - b.Param.Def);
			break;
		case SortFilterDefine.SortType.RARITY:
			PrjUtil.InsertionSort<DataManagerCharaAccessory.Accessory>(ref list, (DataManagerCharaAccessory.Accessory a, DataManagerCharaAccessory.Accessory b) => a.AccessoryData.Rarity.Rarity - b.AccessoryData.Rarity.Rarity);
			break;
		case SortFilterDefine.SortType.NEW:
			PrjUtil.InsertionSort<DataManagerCharaAccessory.Accessory>(ref list, (DataManagerCharaAccessory.Accessory a, DataManagerCharaAccessory.Accessory b) => a.GetTime.CompareTo(b.GetTime));
			break;
		case SortFilterDefine.SortType.AVOIDANCE:
			PrjUtil.InsertionSort<DataManagerCharaAccessory.Accessory>(ref list, (DataManagerCharaAccessory.Accessory a, DataManagerCharaAccessory.Accessory b) => a.Param.Avoid - b.Param.Avoid);
			break;
		case SortFilterDefine.SortType.BEAT_DAMAGE:
			PrjUtil.InsertionSort<DataManagerCharaAccessory.Accessory>(ref list, (DataManagerCharaAccessory.Accessory a, DataManagerCharaAccessory.Accessory b) => a.Param.Beat - b.Param.Beat);
			break;
		case SortFilterDefine.SortType.ACTION_DAMAGE:
			PrjUtil.InsertionSort<DataManagerCharaAccessory.Accessory>(ref list, (DataManagerCharaAccessory.Accessory a, DataManagerCharaAccessory.Accessory b) => a.Param.Action - b.Param.Action);
			break;
		case SortFilterDefine.SortType.TRY_DAMAGE:
			PrjUtil.InsertionSort<DataManagerCharaAccessory.Accessory>(ref list, (DataManagerCharaAccessory.Accessory a, DataManagerCharaAccessory.Accessory b) => a.Param.Try - b.Param.Try);
			break;
		}
		if (order)
		{
			list.Reverse();
		}
	}

	public class FilterStatus
	{
		public HashSet<ItemDef.Rarity> SelectRarityList { get; set; }

		public HashSet<DataManagerCharaAccessory.DispType> SelectDispTypeList { get; set; }

		public HashSet<bool> SelectOwnerStatusList { get; set; }

		public string SearchText { get; set; }

		public FilterStatus()
		{
			this.SelectRarityList = new HashSet<ItemDef.Rarity>();
			this.SelectDispTypeList = new HashSet<DataManagerCharaAccessory.DispType>();
			this.SelectOwnerStatusList = new HashSet<bool>();
			this.SearchText = "";
		}
	}
}
