using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x020000A6 RID: 166
public class DataManagerShop
{
	// Token: 0x0600074F RID: 1871 RVA: 0x00032591 File Offset: 0x00030791
	public DataManagerShop(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000750 RID: 1872 RVA: 0x000325AB File Offset: 0x000307AB
	// (set) Token: 0x06000751 RID: 1873 RVA: 0x000325B3 File Offset: 0x000307B3
	private List<ShopData> shopDataList { get; set; }

	// Token: 0x06000752 RID: 1874 RVA: 0x000325BC File Offset: 0x000307BC
	public List<ShopData> GetShopDataList(bool isExclusionPicnic = true, bool isExclusionNoItemNoDisp = true, ShopData.TabCategory tabCategory = ShopData.TabCategory.ALL)
	{
		if (this.shopDataList == null)
		{
			return new List<ShopData>();
		}
		List<ShopData> list = this.shopDataList.FindAll((ShopData x) => x.startTime < TimeManager.Now && TimeManager.Now < x.endTime);
		list.Sort((ShopData a, ShopData b) => a.priority - b.priority);
		if (isExclusionPicnic)
		{
			list.RemoveAll((ShopData item) => item.category == ShopData.Category.PICNIC || item.shopId == DataManager.DmPicnic.food_shop_id);
		}
		if (isExclusionNoItemNoDisp)
		{
			list.RemoveAll((ShopData item) => (item.category == ShopData.Category.OTHER_NOITEM_HIDE || item.category == ShopData.Category.EVENT_NOITEM_HIDE) && item.priceItemId != 0 && DataManager.DmItem.GetUserItemData(item.priceItemId).num == 0);
		}
		if (tabCategory != ShopData.TabCategory.ALL)
		{
			return list.FindAll((ShopData item) => item.tabCategory == (int)tabCategory);
		}
		return list;
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x000326A4 File Offset: 0x000308A4
	public ShopData GetShopData(int shopId)
	{
		return this.shopDataList.Find((ShopData x) => shopId == x.shopId);
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x000326D5 File Offset: 0x000308D5
	public MstShopCharaStatusData GetCharaStatusData(int statusId)
	{
		return this.mstShopCharaStatusDataMap[statusId];
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x000326E3 File Offset: 0x000308E3
	public HashSet<int> GetOldGoodsIdList()
	{
		return this.oldGoodsIdList;
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x000326EB File Offset: 0x000308EB
	public void RequestGetShopList()
	{
		if (DataManager.DmChara != null)
		{
			DataManager.DmChara.ShopUpdateRequired = false;
		}
		this.shopDataList = new List<ShopData>();
		this.parentData.ServerRequest(ShopListCmd.Create(), new Action<Command>(this.CbShopListCmd));
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00032726 File Offset: 0x00030926
	public void RequestActionBuyShopItem(int goodsId, int num)
	{
		this.parentData.ServerRequest(ShopBuyCmd.Create(goodsId, num), new Action<Command>(this.CbShopBuyCmd));
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x00032746 File Offset: 0x00030946
	public void RequestActionBulkBuyShopItem(List<ShopData.ItemOne> goodsDataList)
	{
		this.parentData.ServerRequest(ShopBulkBuyCmd.Create(goodsDataList), new Action<Command>(this.CbShopBulkBuyCmd));
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00032768 File Offset: 0x00030968
	public void RequestActionUpdateNewFlag(HashSet<int> oldGoodsId)
	{
		List<NewFlg> list = oldGoodsId.ToList<int>().ConvertAll<NewFlg>((int item) => new NewFlg
		{
			category = 11,
			any_id = item,
			new_mgmt_flg = 1
		});
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(list), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x000327C0 File Offset: 0x000309C0
	private void CbShopBuyCmd(Command cmd)
	{
		ShopBuyRequest req = cmd.request as ShopBuyRequest;
		ShopBuyResponse shopBuyResponse = cmd.response as ShopBuyResponse;
		this.parentData.UpdateUserAssetByAssets(shopBuyResponse.assets);
		ShopData.ItemOne itemOne = this.shopDataList.Find((ShopData item) => item.shopId == this.mstShopItemDataMap[req.goodsId].shopId).oneDataInternalList.Find((ShopData.ItemOne item) => item.goodsId == req.goodsId);
		itemOne.UpdateNowChangeNum(itemOne.nowChangeNum + req.goodsNum);
		this.RefreshShopOneList();
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00032854 File Offset: 0x00030A54
	private void CbShopBulkBuyCmd(Command cmd)
	{
		ShopBulkBuyRequest shopBulkBuyRequest = cmd.request as ShopBulkBuyRequest;
		ShopBulkBuyResponse shopBulkBuyResponse = cmd.response as ShopBulkBuyResponse;
		this.parentData.UpdateUserAssetByAssets(shopBulkBuyResponse.assets);
		using (List<ShopData.ItemOne>.Enumerator enumerator = shopBulkBuyRequest.goodsDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ShopData.ItemOne itemOne = enumerator.Current;
				ShopData.ItemOne itemOne2 = this.shopDataList.Find((ShopData item) => item.shopId == this.mstShopItemDataMap[itemOne.goodsId].shopId).oneDataInternalList.Find((ShopData.ItemOne item) => item.goodsId == itemOne.goodsId);
				itemOne2.UpdateNowChangeNum(itemOne2.maxChangeNum);
			}
		}
		this.RefreshShopOneList();
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x00032918 File Offset: 0x00030B18
	private void CbShopListCmd(Command cmd)
	{
		List<ShopItemInfo> infoList = (cmd.response as ShopListResponse).infoList;
		this.shopDataList = new List<ShopData>();
		foreach (MstShopData mstShopData in this.mstShopDataMap.Values)
		{
			this.shopDataList.Add(new ShopData(mstShopData));
		}
		foreach (ShopItemInfo shopItemInfo in infoList)
		{
			MstShopItemData mstShopItemData = null;
			if (this.mstShopItemDataMap.ContainsKey(shopItemInfo.goodsId))
			{
				mstShopItemData = this.mstShopItemDataMap[shopItemInfo.goodsId];
			}
			if (mstShopItemData != null)
			{
				foreach (ShopData shopData in this.shopDataList)
				{
					if (shopData.shopId == mstShopItemData.shopId)
					{
						ShopData.ItemOne itemOne = new ShopData.ItemOne(mstShopItemData, shopItemInfo);
						if (ItemDef.Id2Kind(itemOne.itemId) == ItemDef.Kind.PRESET && this.mstItemPresetMap.ContainsKey(itemOne.itemId))
						{
							MstItemPreset mstItemPreset = this.mstItemPresetMap[itemOne.itemId];
							itemOne.setItems = new List<ShopData.ItemOne.SetItem>
							{
								new ShopData.ItemOne.SetItem(mstItemPreset.itemId00, mstItemPreset.itemNum00),
								new ShopData.ItemOne.SetItem(mstItemPreset.itemId01, mstItemPreset.itemNum01),
								new ShopData.ItemOne.SetItem(mstItemPreset.itemId02, mstItemPreset.itemNum02),
								new ShopData.ItemOne.SetItem(mstItemPreset.itemId03, mstItemPreset.itemNum03),
								new ShopData.ItemOne.SetItem(mstItemPreset.itemId04, mstItemPreset.itemNum04),
								new ShopData.ItemOne.SetItem(mstItemPreset.itemId05, mstItemPreset.itemNum05)
							};
							itemOne.setItems.RemoveAll((ShopData.ItemOne.SetItem item) => item.itemId == 0 || item.itemNum == 0);
						}
						shopData.oneDataInternalList.Add(itemOne);
						break;
					}
				}
			}
		}
		this.RefreshShopOneList();
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x00032BAC File Offset: 0x00030DAC
	public void InitializeMstData(MstManager mstManager)
	{
		List<MstShopData> mst = mstManager.GetMst<List<MstShopData>>(MstType.SHOP_DATA);
		List<MstShopItemData> mst2 = mstManager.GetMst<List<MstShopItemData>>(MstType.SHOP_ITEM_DATA);
		List<MstItemPreset> mst3 = mstManager.GetMst<List<MstItemPreset>>(MstType.ITEM_PRESET);
		List<MstShopCharaStatusData> mst4 = mstManager.GetMst<List<MstShopCharaStatusData>>(MstType.SHOP_CHARA_STATUS_DATA);
		this.mstShopDataMap = mst.ToDictionary<MstShopData, int, MstShopData>((MstShopData item) => item.shopId, (MstShopData item) => item);
		this.mstShopItemDataMap = mst2.ToDictionary<MstShopItemData, int, MstShopItemData>((MstShopItemData item) => item.goodsId, (MstShopItemData item) => item);
		this.mstItemPresetMap = mst3.ToDictionary<MstItemPreset, int, MstItemPreset>((MstItemPreset preset) => preset.id, (MstItemPreset preset) => preset);
		this.mstShopCharaStatusDataMap = mst4.ToDictionary<MstShopCharaStatusData, int, MstShopCharaStatusData>((MstShopCharaStatusData preset) => preset.statusId, (MstShopCharaStatusData preset) => preset);
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00032D08 File Offset: 0x00030F08
	private void RefreshShopOneList()
	{
		List<int> list = DataManager.DmPhoto.GetUserPhotoMap().Values.ToList<PhotoPackData>().ConvertAll<int>((PhotoPackData item) => item.dynamicData.photoId);
		List<int> list2 = DataManager.DmMission.UserMissionOneList.FindAll((UserMissionOne item) => item.isClear).ConvertAll<int>((UserMissionOne item) => item.missionId);
		DateTime now = TimeManager.Now;
		foreach (ShopData shopData in this.shopDataList)
		{
			shopData.oneDataList = new List<ShopData.ItemOne>();
			using (List<ShopData.ItemOne>.Enumerator enumerator2 = shopData.oneDataInternalList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ShopData.ItemOne oneData = enumerator2.Current;
					bool flag = true;
					if (oneData.dispType == ShopData.ItemOne.DispType.Hide && oneData.disptypeTargetitemId != 0)
					{
						if (ItemDef.Id2Kind(oneData.disptypeTargetitemId) == ItemDef.Kind.PHOTO)
						{
							flag = !list.Contains(oneData.disptypeTargetitemId);
						}
						else
						{
							flag = DataManager.DmItem.GetUserItemData(oneData.disptypeTargetitemId).num <= 0;
						}
					}
					else if (oneData.dispType == ShopData.ItemOne.DispType.Show && oneData.disptypeTargetitemId != 0)
					{
						if (ItemDef.Id2Kind(oneData.disptypeTargetitemId) == ItemDef.Kind.PHOTO)
						{
							flag = list.Contains(oneData.disptypeTargetitemId);
						}
						else
						{
							flag = DataManager.DmItem.GetUserItemData(oneData.disptypeTargetitemId).num > 0;
						}
					}
					else if (oneData.dispType == ShopData.ItemOne.DispType.SpecialOR && oneData.disptypeTargetitemId != 0 && oneData.disptypeTargetitemId02 != 0)
					{
						if (ItemDef.Id2Kind(oneData.disptypeTargetitemId) == ItemDef.Kind.PHOTO)
						{
							flag = list.Contains(oneData.disptypeTargetitemId);
						}
						else
						{
							flag = DataManager.DmItem.GetUserItemData(oneData.disptypeTargetitemId).num > 0;
						}
						if (flag)
						{
							if (ItemDef.Id2Kind(oneData.disptypeTargetitemId02) == ItemDef.Kind.PHOTO)
							{
								flag = !list.Contains(oneData.disptypeTargetitemId02);
							}
							else
							{
								flag = DataManager.DmItem.GetUserItemData(oneData.disptypeTargetitemId02).num <= 0;
							}
						}
					}
					else if (oneData.dispType == ShopData.ItemOne.DispType.SpecialAND && oneData.disptypeTargetitemId != 0 && oneData.disptypeTargetitemId02 != 0)
					{
						if (ItemDef.Id2Kind(oneData.disptypeTargetitemId) == ItemDef.Kind.PHOTO)
						{
							flag = list.Contains(oneData.disptypeTargetitemId);
						}
						else
						{
							flag = DataManager.DmItem.GetUserItemData(oneData.disptypeTargetitemId).num > 0;
						}
						if (flag)
						{
							if (ItemDef.Id2Kind(oneData.disptypeTargetitemId02) == ItemDef.Kind.PHOTO)
							{
								flag = list.Contains(oneData.disptypeTargetitemId02);
							}
							else
							{
								flag = DataManager.DmItem.GetUserItemData(oneData.disptypeTargetitemId02).num > 0;
							}
						}
					}
					if (flag)
					{
						if (oneData.startTime > now)
						{
							oneData.isLockByTime = true;
							flag = flag && oneData.notOpenDispFlag;
						}
						if (oneData.openQuestOneId != 0 && !DataManager.DmQuest.QuestDynamicData.IsClear(oneData.openQuestOneId))
						{
							oneData.isLockByQuest = true;
							flag = flag && oneData.notOpenDispFlag;
						}
						if (oneData.openMissionId != 0 && !list2.Exists((int misId) => misId == oneData.openMissionId))
						{
							oneData.isLockByMission = true;
							flag = flag && oneData.notOpenDispFlag;
						}
					}
					if (flag)
					{
						shopData.oneDataList.Add(oneData);
					}
				}
			}
			PrjUtil.InsertionSort<ShopData.ItemOne>(ref shopData.oneDataList, (ShopData.ItemOne a, ShopData.ItemOne b) => a.priority - b.priority);
			PrjUtil.InsertionSort<ShopData.ItemOne>(ref shopData.oneDataList, (ShopData.ItemOne a, ShopData.ItemOne b) => a.isSoldout.CompareTo(b.isSoldout));
		}
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x00033230 File Offset: 0x00031430
	public void InsertNewList(List<NewFlg> newFlagList)
	{
		this.oldGoodsIdList = new HashSet<int>();
		foreach (NewFlg newFlg in newFlagList)
		{
			if (newFlg.new_mgmt_flg == 1 && newFlg.category == 11)
			{
				this.oldGoodsIdList.Add(newFlg.any_id);
			}
		}
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x000332A8 File Offset: 0x000314A8
	public void CbNewFlgUpdateCmd(Command cmd)
	{
		foreach (NewFlg newFlg in (cmd.request as NewFlgUpdateRequest).new_flg_list)
		{
			if (newFlg.new_mgmt_flg == 1 && newFlg.category == 11)
			{
				this.oldGoodsIdList.Add(newFlg.any_id);
			}
		}
	}

	// Token: 0x0400066B RID: 1643
	private DataManager parentData;

	// Token: 0x0400066D RID: 1645
	private Dictionary<int, MstShopData> mstShopDataMap;

	// Token: 0x0400066E RID: 1646
	private Dictionary<int, MstShopItemData> mstShopItemDataMap;

	// Token: 0x0400066F RID: 1647
	private Dictionary<int, MstItemPreset> mstItemPresetMap;

	// Token: 0x04000670 RID: 1648
	private Dictionary<int, MstShopCharaStatusData> mstShopCharaStatusDataMap;

	// Token: 0x04000671 RID: 1649
	private HashSet<int> oldGoodsIdList = new HashSet<int>();
}
