using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerShop
{
	public DataManagerShop(DataManager p)
	{
		this.parentData = p;
	}

	private List<ShopData> shopDataList { get; set; }

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

	public ShopData GetShopData(int shopId)
	{
		return this.shopDataList.Find((ShopData x) => shopId == x.shopId);
	}

	public MstShopCharaStatusData GetCharaStatusData(int statusId)
	{
		return this.mstShopCharaStatusDataMap[statusId];
	}

	public HashSet<int> GetOldGoodsIdList()
	{
		return this.oldGoodsIdList;
	}

	public void RequestGetShopList()
	{
		if (DataManager.DmChara != null)
		{
			DataManager.DmChara.ShopUpdateRequired = false;
		}
		this.shopDataList = new List<ShopData>();
		this.parentData.ServerRequest(ShopListCmd.Create(), new Action<Command>(this.CbShopListCmd));
	}

	public void RequestActionBuyShopItem(int goodsId, int num)
	{
		this.parentData.ServerRequest(ShopBuyCmd.Create(goodsId, num), new Action<Command>(this.CbShopBuyCmd));
	}

	public void RequestActionBulkBuyShopItem(List<ShopData.ItemOne> goodsDataList)
	{
		this.parentData.ServerRequest(ShopBulkBuyCmd.Create(goodsDataList), new Action<Command>(this.CbShopBulkBuyCmd));
	}

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

	private void CbShopBuyCmd(Command cmd)
	{
		ShopBuyRequest req = cmd.request as ShopBuyRequest;
		ShopBuyResponse shopBuyResponse = cmd.response as ShopBuyResponse;
		this.parentData.UpdateUserAssetByAssets(shopBuyResponse.assets);
		ShopData.ItemOne itemOne = this.shopDataList.Find((ShopData item) => item.shopId == this.mstShopItemDataMap[req.goodsId].shopId).oneDataInternalList.Find((ShopData.ItemOne item) => item.goodsId == req.goodsId);
		itemOne.UpdateNowChangeNum(itemOne.nowChangeNum + req.goodsNum);
		this.RefreshShopOneList();
	}

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

	private DataManager parentData;

	private Dictionary<int, MstShopData> mstShopDataMap;

	private Dictionary<int, MstShopItemData> mstShopItemDataMap;

	private Dictionary<int, MstItemPreset> mstItemPresetMap;

	private Dictionary<int, MstShopCharaStatusData> mstShopCharaStatusDataMap;

	private HashSet<int> oldGoodsIdList = new HashSet<int>();
}
