using System;
using System.Collections.Generic;
using SGNFW.Common;

// Token: 0x02000104 RID: 260
public class SortFilterManager : Singleton<SortFilterManager>
{
	// Token: 0x17000320 RID: 800
	// (get) Token: 0x06000C84 RID: 3204 RVA: 0x0004CD8B File Offset: 0x0004AF8B
	// (set) Token: 0x06000C85 RID: 3205 RVA: 0x0004CD92 File Offset: 0x0004AF92
	public static AccessorySortFilter Accessory { get; private set; }

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0004CD9A File Offset: 0x0004AF9A
	// (set) Token: 0x06000C87 RID: 3207 RVA: 0x0004CDA1 File Offset: 0x0004AFA1
	private static Dictionary<SortFilterDefine.RegisterType, DataManagerGameStatus.UserFlagData.SortTypeData> LatestSortTypeDataMap { get; set; }

	// Token: 0x06000C88 RID: 3208 RVA: 0x0004CDA9 File Offset: 0x0004AFA9
	protected override void OnSingletonAwake()
	{
		SortFilterManager.Accessory = new AccessorySortFilter();
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x0004CDB5 File Offset: 0x0004AFB5
	public static DataManagerGameStatus.UserFlagData.SortTypeData GetActiveSortTypeData(SortFilterDefine.RegisterType registerType)
	{
		if (SortFilterManager.LatestSortTypeDataMap != null && SortFilterManager.LatestSortTypeDataMap.ContainsKey(registerType))
		{
			return SortFilterManager.LatestSortTypeDataMap[registerType];
		}
		return DataManager.DmGameStatus.MakeUserFlagData().GetSortTypeData(registerType);
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x0004CDE7 File Offset: 0x0004AFE7
	private static DataManagerGameStatus.UserFlagData.SortTypeData GetServerSortTypeData(SortFilterDefine.RegisterType registerType)
	{
		return DataManager.DmGameStatus.MakeUserFlagData().GetSortTypeData(registerType);
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x0004CDF9 File Offset: 0x0004AFF9
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

	// Token: 0x06000C8C RID: 3212 RVA: 0x0004CE34 File Offset: 0x0004B034
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
