using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020000EC RID: 236
public class AccessorySortFilter
{
	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0003ED59 File Offset: 0x0003CF59
	// (set) Token: 0x06000AC9 RID: 2761 RVA: 0x0003ED61 File Offset: 0x0003CF61
	private Dictionary<SortFilterDefine.RegisterType, AccessorySortFilter.FilterStatus> FilterStatusMap { get; set; }

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0003ED6A File Offset: 0x0003CF6A
	// (set) Token: 0x06000ACB RID: 2763 RVA: 0x0003ED72 File Offset: 0x0003CF72
	public SortFilterDefine.RegisterType LatestSortWindowRegisterType { get; set; }

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06000ACC RID: 2764 RVA: 0x0003ED7B File Offset: 0x0003CF7B
	// (set) Token: 0x06000ACD RID: 2765 RVA: 0x0003ED83 File Offset: 0x0003CF83
	public SortFilterDefine.SortType LatestSortWindowSortType { get; set; }

	// Token: 0x06000ACE RID: 2766 RVA: 0x0003ED8C File Offset: 0x0003CF8C
	public AccessorySortFilter()
	{
		this.FilterStatusMap = new Dictionary<SortFilterDefine.RegisterType, AccessorySortFilter.FilterStatus>();
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x0003ED9F File Offset: 0x0003CF9F
	public AccessorySortFilter.FilterStatus GetFilterStatus(SortFilterDefine.RegisterType registerType)
	{
		if (!this.FilterStatusMap.ContainsKey(registerType))
		{
			this.FilterStatusMap.Add(registerType, new AccessorySortFilter.FilterStatus());
		}
		return this.FilterStatusMap[registerType];
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x0003EDCC File Offset: 0x0003CFCC
	public void SetFilterStatus(SortFilterDefine.RegisterType registerType, AccessorySortFilter.FilterStatus filterStatus)
	{
		if (!this.FilterStatusMap.ContainsKey(registerType))
		{
			this.FilterStatusMap.Add(registerType, new AccessorySortFilter.FilterStatus());
		}
		this.FilterStatusMap[registerType] = filterStatus;
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x0003EDFC File Offset: 0x0003CFFC
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

	// Token: 0x06000AD2 RID: 2770 RVA: 0x0003EF24 File Offset: 0x0003D124
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

	// Token: 0x020007F5 RID: 2037
	public class FilterStatus
	{
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06003790 RID: 14224 RVA: 0x001C9074 File Offset: 0x001C7274
		// (set) Token: 0x06003791 RID: 14225 RVA: 0x001C907C File Offset: 0x001C727C
		public HashSet<ItemDef.Rarity> SelectRarityList { get; set; }

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06003792 RID: 14226 RVA: 0x001C9085 File Offset: 0x001C7285
		// (set) Token: 0x06003793 RID: 14227 RVA: 0x001C908D File Offset: 0x001C728D
		public HashSet<DataManagerCharaAccessory.DispType> SelectDispTypeList { get; set; }

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06003794 RID: 14228 RVA: 0x001C9096 File Offset: 0x001C7296
		// (set) Token: 0x06003795 RID: 14229 RVA: 0x001C909E File Offset: 0x001C729E
		public HashSet<bool> SelectOwnerStatusList { get; set; }

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06003796 RID: 14230 RVA: 0x001C90A7 File Offset: 0x001C72A7
		// (set) Token: 0x06003797 RID: 14231 RVA: 0x001C90AF File Offset: 0x001C72AF
		public string SearchText { get; set; }

		// Token: 0x06003798 RID: 14232 RVA: 0x001C90B8 File Offset: 0x001C72B8
		public FilterStatus()
		{
			this.SelectRarityList = new HashSet<ItemDef.Rarity>();
			this.SelectDispTypeList = new HashSet<DataManagerCharaAccessory.DispType>();
			this.SelectOwnerStatusList = new HashSet<bool>();
			this.SearchText = "";
		}
	}
}
