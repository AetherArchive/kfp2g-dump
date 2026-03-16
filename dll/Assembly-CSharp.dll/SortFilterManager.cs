using System;
using System.Collections.Generic;
using SGNFW.Common;

public class SortFilterManager : Singleton<SortFilterManager>
{
	public static AccessorySortFilter Accessory { get; private set; }

	private static Dictionary<SortFilterDefine.RegisterType, DataManagerGameStatus.UserFlagData.SortTypeData> LatestSortTypeDataMap { get; set; }

	protected override void OnSingletonAwake()
	{
		SortFilterManager.Accessory = new AccessorySortFilter();
	}

	public static DataManagerGameStatus.UserFlagData.SortTypeData GetActiveSortTypeData(SortFilterDefine.RegisterType registerType)
	{
		if (SortFilterManager.LatestSortTypeDataMap != null && SortFilterManager.LatestSortTypeDataMap.ContainsKey(registerType))
		{
			return SortFilterManager.LatestSortTypeDataMap[registerType];
		}
		return DataManager.DmGameStatus.MakeUserFlagData().GetSortTypeData(registerType);
	}

	private static DataManagerGameStatus.UserFlagData.SortTypeData GetServerSortTypeData(SortFilterDefine.RegisterType registerType)
	{
		return DataManager.DmGameStatus.MakeUserFlagData().GetSortTypeData(registerType);
	}

	public static void UpdateSortFilterData(SortFilterDefine.RegisterType registerType, DataManagerGameStatus.UserFlagData.SortTypeData sortFilterData)
	{
		if (SortFilterManager.LatestSortTypeDataMap == null)
		{
			SortFilterManager.LatestSortTypeDataMap = new Dictionary<SortFilterDefine.RegisterType, DataManagerGameStatus.UserFlagData.SortTypeData>();
		}
		if (SortFilterManager.LatestSortTypeDataMap.ContainsKey(registerType))
		{
			SortFilterManager.LatestSortTypeDataMap[registerType] = sortFilterData;
			return;
		}
		SortFilterManager.LatestSortTypeDataMap.Add(registerType, sortFilterData);
	}

	public static void RequestUpdateSortTypeData()
	{
		if (SortFilterManager.LatestSortTypeDataMap == null)
		{
			return;
		}
		if (SortFilterManager.LatestSortTypeDataMap.Count == 0)
		{
			return;
		}
		List<DataManagerGameStatus.UserFlagData.SortTypeData> list = new List<DataManagerGameStatus.UserFlagData.SortTypeData>();
		foreach (KeyValuePair<SortFilterDefine.RegisterType, DataManagerGameStatus.UserFlagData.SortTypeData> keyValuePair in SortFilterManager.LatestSortTypeDataMap)
		{
			DataManagerGameStatus.UserFlagData.SortTypeData serverSortTypeData = SortFilterManager.GetServerSortTypeData(keyValuePair.Key);
			if (keyValuePair.Value.SortType != serverSortTypeData.SortType || keyValuePair.Value.Order != serverSortTypeData.Order)
			{
				list.Add(keyValuePair.Value);
			}
		}
		if (0 < list.Count)
		{
			DataManager.DmGameStatus.RequestActionUpdateSortType(list);
		}
		SortFilterManager.LatestSortTypeDataMap.Clear();
	}
}
