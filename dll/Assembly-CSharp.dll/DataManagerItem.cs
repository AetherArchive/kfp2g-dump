using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerItem
{
	public DataManagerItem(DataManager p)
	{
		this.parentData = p;
		this.userItemMap = new Dictionary<int, ItemData>();
		this.itemStaticMap = new Dictionary<int, ItemStaticBase>();
		this.UserItemBankMap = new Dictionary<int, ItemBank>();
	}

	public ItemStaticBase GetItemStaticBase(int itemId)
	{
		if (!this.itemStaticMap.ContainsKey(itemId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerItem.GetItemStaticBase : 定義されていないID[" + itemId.ToString() + "]を取得しようとしました。プランナに連絡してください", null);
			return null;
		}
		return this.itemStaticMap[itemId];
	}

	public Dictionary<int, ItemStaticBase> GetItemStaticMap()
	{
		return this.itemStaticMap;
	}

	public List<DataManagerItem.ExchangeRatesData> GetExchageRatesList()
	{
		return this.exchangeRatesList;
	}

	public List<ExchangeExecuteCountInfo> GetExecuteCountInfos()
	{
		return this.executeCountInfos;
	}

	public bool IsNeededUpdateExecuteCountInfos()
	{
		return TimeManager.Now.Date != this.executeCountInfosLastUpdate.Date;
	}

	public ItemData GetUserItemData(int itemId)
	{
		if (this.userItemMap.ContainsKey(itemId))
		{
			return new ItemData(itemId, this.userItemMap[itemId].num);
		}
		return new ItemData(itemId, 0);
	}

	public Dictionary<int, ItemData> GetUserItemMap()
	{
		return this.userItemMap;
	}

	public Dictionary<int, ItemBank> UserItemBankMap { get; private set; }

	public List<ItemData> GetUserTabItemList(Dictionary<int, ItemData> itemMap, DataManagerItem.DispType type)
	{
		List<ItemData> list = new List<ItemData>();
		list.AddRange(itemMap.Values);
		return this.GetUserDispItemList(list, type);
	}

	public List<ItemData> GetUserDispItemList(List<ItemData> itemList, DataManagerItem.DispType type)
	{
		List<ItemData> list;
		switch (type)
		{
		case DataManagerItem.DispType.Common:
			list = new List<ItemData>(itemList).FindAll((ItemData item) => (item.staticData.GetKind() == ItemDef.Kind.STONE || item.staticData.GetKind() == ItemDef.Kind.COMMON || item.staticData.GetKind() == ItemDef.Kind.STAMINA_RECOVERY || item.staticData.GetKind() == ItemDef.Kind.COMMON_RESERVE || item.staticData.GetKind() == ItemDef.Kind.EVENT_ITEM || item.staticData.GetKind() == ItemDef.Kind.GACHA_TICKET) && item.staticData.GetId() != 30100 && (item.staticData.endTime == null || (item.staticData.endTime != null && TimeManager.Now < item.staticData.endTime)) && 0 < item.num);
			list.Sort(new Comparison<ItemData>(DataManagerItem.<GetUserDispItemList>g__SortItem|21_0));
			return list;
		case DataManagerItem.DispType.Growth:
		{
			List<ItemDef.Kind> list2 = new List<ItemDef.Kind>();
			list2.Add(ItemDef.Kind.KEMOBOARD);
			list2.Add(ItemDef.Kind.GROWTH_MASTERSKILL);
			list2.Add(ItemDef.Kind.EXP_ADD);
			list2.Add(ItemDef.Kind.PROMOTE);
			list2.Add(ItemDef.Kind.ARTS_UP);
			list2.Add(ItemDef.Kind.KIZUNA_LIMITRELEASEITEM);
			list2.Add(ItemDef.Kind.PROMOTE_EXT);
			list2.Add(ItemDef.Kind.RANK_UP);
			list2.Add(ItemDef.Kind.PHOTO_FRAME_UP);
			list2.Add(ItemDef.Kind.GROWTH_RESERVE);
			list2.Add(ItemDef.Kind.ABILITY_RELEASE);
			List<ItemData> list3 = new List<ItemData>(itemList);
			list = new List<ItemData>();
			using (List<ItemDef.Kind>.Enumerator enumerator = list2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemDef.Kind searchKind = enumerator.Current;
					List<ItemData> list4 = list3.FindAll((ItemData x) => x.staticData.GetKind() == searchKind && (x.staticData.endTime == null || (x.staticData.endTime != null && TimeManager.Now < x.staticData.endTime)) && 0 < x.num);
					list4.Sort(new Comparison<ItemData>(DataManagerItem.<GetUserDispItemList>g__SortItem|21_0));
					ItemDef.Kind searchKind2 = searchKind;
					if (searchKind2 - ItemDef.Kind.RANK_UP <= 1 || searchKind2 == ItemDef.Kind.PROMOTE_EXT)
					{
						PrjUtil.InsertionSortLight<ItemData>(ref list4, (ItemData a, ItemData b) => PrjUtil.CompareByName(a.id, b.id));
					}
					list.AddRange(list4);
				}
				return list;
			}
			break;
		}
		case DataManagerItem.DispType.Decoration:
			break;
		case DataManagerItem.DispType.Photo:
			list = new List<ItemData>(itemList).FindAll((ItemData item) => item.staticData.GetKind() == ItemDef.Kind.PHOTO && (item.staticData.endTime == null || (item.staticData.endTime != null && TimeManager.Now < item.staticData.endTime)) && 0 < item.num);
			list.Sort(new Comparison<ItemData>(DataManagerItem.<GetUserDispItemList>g__SortPhoto|21_7));
			return list;
		case DataManagerItem.DispType.PlayItem:
			list = new List<ItemData>(itemList).FindAll((ItemData item) => item.staticData.GetKind() == ItemDef.Kind.PICNIC_PLAYITEM && (item.staticData.endTime == null || (item.staticData.endTime != null && TimeManager.Now < item.staticData.endTime)) && 0 < item.num);
			list.Sort(new Comparison<ItemData>(DataManagerItem.<GetUserDispItemList>g__SortItem|21_0));
			return list;
		case DataManagerItem.DispType.Accessory:
			list = new List<ItemData>(itemList).FindAll((ItemData item) => item.staticData.GetKind() == ItemDef.Kind.ACCESSORY_ITEM && (item.staticData.endTime == null || (item.staticData.endTime != null && TimeManager.Now < item.staticData.endTime)) && 0 < item.num);
			list.Sort(new Comparison<ItemData>(DataManagerItem.<GetUserDispItemList>g__SortItem|21_0));
			return list;
		case DataManagerItem.DispType.Achievement:
			return new List<ItemData>(itemList).FindAll((ItemData item) => item.staticData.GetKind() == ItemDef.Kind.ACHIEVEMENT && item.num > 0);
		default:
			return new List<ItemData>();
		}
		list = new List<ItemData>(itemList).FindAll((ItemData item) => (item.staticData.GetKind() == ItemDef.Kind.FURNITURE || item.staticData.GetKind() == ItemDef.Kind.TREEHOUSE_FURNITURE) && (item.staticData.endTime == null || (item.staticData.endTime != null && TimeManager.Now < item.staticData.endTime)) && 0 < item.num);
		list.Sort(new Comparison<ItemData>(DataManagerItem.<GetUserDispItemList>g__SortItem|21_0));
		List<ItemData> list5 = new List<ItemData>(itemList).FindAll((ItemData item) => item.staticData.GetKind() == ItemDef.Kind.CLOTHES && (item.staticData.endTime == null || (item.staticData.endTime != null && TimeManager.Now < item.staticData.endTime)) && 0 < item.num).FindAll((ItemData x) => DataManager.DmChara.GetCharaClothesStaticData(x.id).GetRank == 0);
		list5.Sort(new Comparison<ItemData>(DataManagerItem.<GetUserDispItemList>g__CheckCharaClothes|21_5));
		list.AddRange(list5);
		return list;
	}

	public List<ItemData> GetUserItemListByKind(ItemDef.Kind kind)
	{
		List<ItemData> list = new List<ItemData>();
		foreach (ItemData itemData in this.userItemMap.Values)
		{
			if (itemData != null && itemData.staticData != null && itemData.staticData.GetKind() == kind)
			{
				list.Add(itemData);
			}
		}
		return list;
	}

	public HashSet<int> GetOldItemIdList()
	{
		return this.oldItemIdList;
	}

	public List<DataManagerItem.StaminaRecoveryItemData> StaminaRecoveryItemDataList { get; private set; }

	public void UpdateUserDataByServer(List<Item> haveItemList, List<Chara> haveCharaList, List<Achievement> haveAchievementList)
	{
		foreach (Item item2 in haveItemList)
		{
			int item_id = item2.item_id;
			int item_num = item2.item_num;
			this.InsertUpdateUserMap(item_id, item_num);
		}
		if (haveCharaList != null)
		{
			foreach (Chara chara in haveCharaList)
			{
				int chara_id = chara.chara_id;
				int num = 1;
				this.InsertUpdateUserMap(chara_id, num);
			}
		}
		if (haveAchievementList != null)
		{
			foreach (Achievement achievement in haveAchievementList)
			{
				int achievement_id = achievement.achievement_id;
				int num2 = 1;
				this.InsertUpdateUserMap(achievement_id, num2);
			}
		}
		if (haveItemList.FindIndex((Item item) => item.item_id == 30002 || item.item_id == 30001) >= 0)
		{
			int num3 = this.GetUserItemData(30002).num + this.GetUserItemData(30001).num;
			this.InsertUpdateUserMap(30100, num3);
		}
	}

	public void UpdateUserDataByServer(List<ItemBank> itemBankList)
	{
		if (itemBankList != null)
		{
			foreach (ItemBank itemBank in itemBankList)
			{
				int item_id = itemBank.item_id;
				if (this.UserItemBankMap.ContainsKey(item_id))
				{
					this.UserItemBankMap[item_id] = itemBank;
				}
				else
				{
					this.UserItemBankMap.Add(item_id, itemBank);
				}
			}
		}
	}

	public void UpdateUserDataByDirect(List<ItemInput> useItemList, int calcType)
	{
		for (int i = 0; i < useItemList.Count; i++)
		{
			int itemId = useItemList[i].itemId;
			int num = this.GetUserItemData(itemId).num;
			if (calcType == 1)
			{
				num += useItemList[i].num;
			}
			else if (calcType == 2)
			{
				num -= useItemList[i].num;
			}
			else if (calcType == 0)
			{
				num = useItemList[i].num;
			}
			num = Math.Max(num, 0);
			this.InsertUpdateUserMap(itemId, num);
		}
		if (useItemList.FindIndex((ItemInput item) => item.itemId == 30002 || item.itemId == 30001) >= 0)
		{
			int num2 = this.GetUserItemData(30002).num + this.GetUserItemData(30001).num;
			this.InsertUpdateUserMap(30100, num2);
		}
	}

	public void ClearUserDataByDirect()
	{
		this.userItemMap.Clear();
	}

	private void InsertUpdateUserMap(int id, int num)
	{
		if (this.userItemMap.ContainsKey(id))
		{
			this.userItemMap[id] = new ItemData(id, num);
			return;
		}
		this.userItemMap.Add(id, new ItemData(id, num));
	}

	public void InitializeMstData(MstManager mstManager)
	{
		List<MstItemCommon> mst = mstManager.GetMst<List<MstItemCommon>>(MstType.ITEM_COMMON);
		List<MstItemPreset> mst2 = mstManager.GetMst<List<MstItemPreset>>(MstType.ITEM_PRESET);
		List<MstItemRecovery> mst3 = mstManager.GetMst<List<MstItemRecovery>>(MstType.ITEM_RECOVERY);
		List<MstItemLottery> mst4 = mstManager.GetMst<List<MstItemLottery>>(MstType.ITEM_LOTTERY);
		List<MstItemLotteryLineup> mst5 = mstManager.GetMst<List<MstItemLotteryLineup>>(MstType.ITEM_LOTTERY_LINEUP);
		List<MstItemExchangeRatesData> mst6 = mstManager.GetMst<List<MstItemExchangeRatesData>>(MstType.ITEM_EXCHANGE_RATES_DATA);
		this.itemStaticMap = new Dictionary<int, ItemStaticBase>();
		this.itemCharaIdMap = new Dictionary<int, int>();
		this.StaminaRecoveryItemDataList = new List<DataManagerItem.StaminaRecoveryItemData>();
		foreach (MstItemCommon mstItemCommon in mst)
		{
			if (this.itemStaticMap.ContainsKey(mstItemCommon.id))
			{
				DataManagerItem.<InitializeMstData>g__OverLoadItem|33_0(mstItemCommon.id);
			}
			else
			{
				this.itemStaticMap.Add(mstItemCommon.id, new ItemCommonData(mstItemCommon));
			}
		}
		foreach (MstItemPreset mstItemPreset in mst2)
		{
			if (this.itemStaticMap.ContainsKey(mstItemPreset.id))
			{
				DataManagerItem.<InitializeMstData>g__OverLoadItem|33_0(mstItemPreset.id);
			}
			else
			{
				this.itemStaticMap.Add(mstItemPreset.id, new ItemPresetData(mstItemPreset));
			}
		}
		using (List<MstItemLottery>.Enumerator enumerator3 = mst4.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				MstItemLottery mstItemLottery = enumerator3.Current;
				if (this.itemStaticMap.ContainsKey(mstItemLottery.id))
				{
					DataManagerItem.<InitializeMstData>g__OverLoadItem|33_0(mstItemLottery.id);
				}
				else
				{
					List<MstItemLotteryLineup> list = mst5.FindAll((MstItemLotteryLineup x) => mstItemLottery.id == x.lotteryItemId);
					if (list.Count != 0)
					{
						this.itemStaticMap.Add(mstItemLottery.id, new ItemLotteryData(mstItemLottery, list));
					}
				}
			}
		}
		foreach (MstItemRecovery mstItemRecovery in mst3)
		{
			DataManagerItem.StaminaRecoveryItemData staminaRecoveryItemData = new DataManagerItem.StaminaRecoveryItemData
			{
				category = mstItemRecovery.category,
				flavorText = mstItemRecovery.flavorText,
				iconName = mstItemRecovery.iconName,
				id = mstItemRecovery.id,
				name = mstItemRecovery.name,
				rarity = mstItemRecovery.rarity,
				recoveryValue = mstItemRecovery.recoveryVal,
				salePrice = mstItemRecovery.salePrice,
				stackMax = mstItemRecovery.stackMax,
				unitName = mstItemRecovery.unitName
			};
			this.StaminaRecoveryItemDataList.Add(staminaRecoveryItemData);
		}
		if (mst6 == null)
		{
			return;
		}
		foreach (MstItemExchangeRatesData mstItemExchangeRatesData in mst6)
		{
			DataManagerItem.ExchangeRatesData exchangeRatesData = new DataManagerItem.ExchangeRatesData(mstItemExchangeRatesData);
			this.exchangeRatesList.Add(exchangeRatesData);
		}
	}

	public void InitializeMstDataByItemEndTime(MstManager mstManager)
	{
		foreach (MstItemEndtimeData mstItemEndtimeData in mstManager.GetMst<List<MstItemEndtimeData>>(MstType.ITEM_ENDTIME_DATA))
		{
			ItemStaticBase itemStaticBase = this.GetItemStaticBase(mstItemEndtimeData.itemId);
			if (itemStaticBase != null)
			{
				itemStaticBase.endTime = new DateTime?(new DateTime(PrjUtil.ConvertTimeToTicks(mstItemEndtimeData.endTime)));
			}
		}
	}

	public void AddMstDataByItem(List<ItemStaticBase> addList)
	{
		foreach (ItemStaticBase itemStaticBase in addList)
		{
			if (!this.itemStaticMap.ContainsKey(itemStaticBase.GetId()))
			{
				this.itemStaticMap.Add(itemStaticBase.GetId(), itemStaticBase);
			}
		}
	}

	public void AddMstDataByItemCharaMap(int itemId, int charaId)
	{
		if (itemId == 0)
		{
			return;
		}
		if (!this.itemCharaIdMap.ContainsKey(itemId))
		{
			this.itemCharaIdMap.Add(itemId, charaId);
		}
	}

	public int ItemId2ChraId(int itemId)
	{
		int num;
		if (this.itemCharaIdMap != null && this.itemCharaIdMap.ContainsKey(itemId))
		{
			num = this.itemCharaIdMap[itemId];
		}
		else if (ItemDef.Id2Kind(itemId) == ItemDef.Kind.CHARA)
		{
			num = itemId;
		}
		else
		{
			num = itemId % 1000;
			Verbose<PrjLog>.LogError(string.Concat(new string[]
			{
				base.GetType().FullName,
				".",
				MethodBase.GetCurrentMethod().Name,
				"() : 定義されていないID[",
				itemId.ToString(),
				"]を参照しようとしました"
			}), null);
		}
		return num;
	}

	public bool IsAllAttributeAddExpItem(int itemId)
	{
		DataManagerServerMst.CharaLevelItem charaLevelItem = DataManager.DmServerMst.charaLevelItemDataList.Find((DataManagerServerMst.CharaLevelItem item) => item.itemId == itemId);
		return charaLevelItem != null && charaLevelItem.attribute == CharaDef.AttributeType.ALL;
	}

	public static int GetUserHaveNum(int itemId)
	{
		if (ItemDef.Id2Kind(itemId) == ItemDef.Kind.PHOTO)
		{
			return DataManager.DmPhoto.GetHaveNumByPhotoItemId(itemId);
		}
		if (ItemDef.Id2Kind(itemId) == ItemDef.Kind.ACCESSORY_ITEM)
		{
			return DataManager.DmChAccessory.GetHaveNumByAccessoryItemId(itemId);
		}
		return DataManager.DmItem.GetUserItemData(itemId).num;
	}

	public static bool IsExpectedItemStock(int itemId, long addNum)
	{
		if ((long)DataManagerItem.GetUserHaveNum(itemId) + addNum <= (long)DataManager.DmItem.GetItemStaticBase(itemId).GetStackMax())
		{
			return false;
		}
		int num = ((itemId == 30101) ? 30090 : 0);
		return num != 0 && DataManager.DmItem.GetUserItemData(num).num > 0;
	}

	public static long GetUserHaveMaxNum(int itemId, int replaceBeforeItemId = 0)
	{
		if (itemId == 30101)
		{
			ItemData userItemData = DataManager.DmItem.GetUserItemData(30090);
			if (userItemData != null && userItemData.num > 0)
			{
				return long.MaxValue;
			}
			return (long)DataManager.DmItem.GetItemStaticBase(itemId).GetStackMax();
		}
		else
		{
			ItemDef.Kind kind = ItemDef.Id2Kind(itemId);
			if (kind == ItemDef.Kind.PHOTO)
			{
				int photoStockLimit = DataManager.DmPhoto.PhotoStockLimit;
				int count = DataManager.DmPhoto.GetUserPhotoMap().Count;
				int haveNumByPhotoItemId = DataManager.DmPhoto.GetHaveNumByPhotoItemId(itemId);
				return (long)(photoStockLimit - count + haveNumByPhotoItemId);
			}
			if (kind == ItemDef.Kind.CLOTHES)
			{
				return (long)(DataManager.DmItem.GetUserItemData(itemId).num + 1);
			}
			if (kind == ItemDef.Kind.ACCESSORY_ITEM)
			{
				int accessoryStockLimit = DataManager.DmChAccessory.AccessoryStockLimit;
				int count2 = DataManager.DmChAccessory.GetUserAccessoryList().Count;
				int haveNumByAccessoryItemId = DataManager.DmChAccessory.GetHaveNumByAccessoryItemId(itemId);
				return (long)(accessoryStockLimit - count2 + haveNumByAccessoryItemId);
			}
			if (ItemDef.Id2Kind(replaceBeforeItemId) == ItemDef.Kind.CLOTHES)
			{
				CharaClothStatic charaClothesStaticData = DataManager.DmChara.GetCharaClothesStaticData(replaceBeforeItemId);
				return (long)(DataManager.DmItem.GetUserItemData(itemId).num + charaClothesStaticData.replaceItemNum);
			}
			return (long)DataManager.DmItem.GetItemStaticBase(itemId).GetStackMax();
		}
	}

	public static bool isOverUserHaveMaxNum(int itemId, int addNum)
	{
		return (long)(DataManagerItem.GetUserHaveNum(itemId) + addNum) > DataManagerItem.GetUserHaveMaxNum(itemId, 0);
	}

	public void RequestGetNewFlag()
	{
		this.parentData.ServerRequest(NewFlgGetListCmd.Create(0), new Action<Command>(this.CbNewFlgGetListCmd));
	}

	public void RequestActionUpdateNewFlag(List<int> oldItemId)
	{
		List<NewFlg> list = oldItemId.ConvertAll<NewFlg>((int item) => new NewFlg
		{
			category = 1,
			any_id = item,
			new_mgmt_flg = 1
		});
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(list), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	public void RequestActionItemSell(List<ItemInput> itemInputs)
	{
		List<Item> list = new List<Item>();
		foreach (ItemInput itemInput in itemInputs)
		{
			Item item = new Item
			{
				item_id = itemInput.itemId,
				item_num = itemInput.num
			};
			list.Add(item);
		}
		this.parentData.ServerRequest(ItemSellCmd.Create(list), new Action<Command>(this.CbItemSellCmd));
	}

	public void RequestCharaPxChange(int charaId, int itemNum)
	{
		this.parentData.ServerRequest(CharaPxChangeCmd.Create(charaId, itemNum), new Action<Command>(this.CbCharaPxChange));
	}

	public void RequestCharaSpxChange(int charaId, int itemNum)
	{
		this.parentData.ServerRequest(CharaSpxChangeCmd.Create(charaId, itemNum), new Action<Command>(this.CbCharaSpxChange));
	}

	public void RequestItemExchange(int executeCount, int targetItemId)
	{
		this.parentData.ServerRequest(ItemExchangeCmd.Create(executeCount, targetItemId), new Action<Command>(this.CbItemExchange));
	}

	public void RequestExchangeExecuteList()
	{
		this.parentData.ServerRequest(ExchangeExecuteListCmd.Create(), new Action<Command>(this.CbExchangeExecuteList));
	}

	public void CbNewFlgGetListCmd(Command cmd)
	{
		NewFlgGetListResponse newFlgGetListResponse = cmd.response as NewFlgGetListResponse;
		this.parentData.InsertNewList(newFlgGetListResponse.new_flg_list);
	}

	public void InsertNewList(List<NewFlg> newFlagList)
	{
		this.oldItemIdList = new HashSet<int>();
		foreach (NewFlg newFlg in newFlagList)
		{
			if (newFlg.new_mgmt_flg == 1 && newFlg.category == 1)
			{
				this.oldItemIdList.Add(newFlg.any_id);
			}
		}
	}

	public void CbNewFlgUpdateCmd(Command cmd)
	{
		foreach (NewFlg newFlg in (cmd.request as NewFlgUpdateRequest).new_flg_list)
		{
			if (newFlg.new_mgmt_flg == 1 && newFlg.category == 1)
			{
				this.oldItemIdList.Add(newFlg.any_id);
			}
		}
	}

	public void CbItemSellCmd(Command cmd)
	{
		ItemSellResponse itemSellResponse = cmd.response as ItemSellResponse;
		this.parentData.UpdateUserAssetByAssets(itemSellResponse.assets);
	}

	public void CbCharaPxChange(Command cmd)
	{
		CharaPxChangeResponse charaPxChangeResponse = cmd.response as CharaPxChangeResponse;
		this.parentData.UpdateUserAssetByAssets(charaPxChangeResponse.assets);
	}

	public void CbCharaSpxChange(Command cmd)
	{
		CharaSpxChangeResponse charaSpxChangeResponse = cmd.response as CharaSpxChangeResponse;
		this.parentData.UpdateUserAssetByAssets(charaSpxChangeResponse.assets);
	}

	public void CbItemExchange(Command cmd)
	{
		ItemExchangeResponse itemExchangeResponse = cmd.response as ItemExchangeResponse;
		this.parentData.UpdateUserAssetByAssets(itemExchangeResponse.assets);
		this.executeCountInfos = itemExchangeResponse.infoList;
		this.executeCountInfosLastUpdate = TimeManager.Now;
	}

	public void CbExchangeExecuteList(Command cmd)
	{
		ExchangeExecuteListResponse exchangeExecuteListResponse = cmd.response as ExchangeExecuteListResponse;
		this.executeCountInfos = exchangeExecuteListResponse.infoList;
		this.executeCountInfosLastUpdate = TimeManager.Now;
	}

	[CompilerGenerated]
	internal static int <GetUserDispItemList>g__SortItem|21_0(ItemData a, ItemData b)
	{
		int num = a.id - b.id;
		if (num == 0)
		{
			num = b.num - a.num;
		}
		return num;
	}

	[CompilerGenerated]
	internal static int <GetUserDispItemList>g__CheckCharaClothes|21_5(ItemData a, ItemData b)
	{
		CharaClothStatic charaClothesStaticData = DataManager.DmChara.GetCharaClothesStaticData(a.id);
		CharaClothStatic charaClothesStaticData2 = DataManager.DmChara.GetCharaClothesStaticData(b.id);
		int num = ((charaClothesStaticData.CharaId == charaClothesStaticData2.CharaId) ? (charaClothesStaticData.GetId() - charaClothesStaticData2.GetId()) : PrjUtil.CompareByName(charaClothesStaticData.CharaId, charaClothesStaticData2.CharaId));
		if (num == 0)
		{
			num = b.num - a.num;
		}
		return num;
	}

	[CompilerGenerated]
	internal static int <GetUserDispItemList>g__SortPhoto|21_7(ItemData a, ItemData b)
	{
		int num = b.staticData.GetRarity() - a.staticData.GetRarity();
		if (num == 0)
		{
			num = a.id - b.id;
		}
		if (num == 0)
		{
			num = b.num - a.num;
		}
		return num;
	}

	[CompilerGenerated]
	internal static void <InitializeMstData>g__OverLoadItem|33_0(int itemId)
	{
	}

	private DataManager parentData;

	private Dictionary<int, ItemStaticBase> itemStaticMap;

	private Dictionary<int, int> itemCharaIdMap;

	private HashSet<int> oldItemIdList = new HashSet<int>();

	private List<DataManagerItem.ExchangeRatesData> exchangeRatesList = new List<DataManagerItem.ExchangeRatesData>();

	private List<ExchangeExecuteCountInfo> executeCountInfos;

	private DateTime executeCountInfosLastUpdate;

	private Dictionary<int, ItemData> userItemMap;

	[Serializable]
	public class JsonData<T>
	{
		public T[] data;
	}

	public enum DispType
	{
		Undefined = -1,
		Common,
		Growth,
		Decoration,
		Photo,
		PlayItem,
		Accessory,
		Achievement
	}

	public class StaminaRecoveryItemData
	{
		public int id;

		public int rarity;

		public string name;

		public string flavorText;

		public int stackMax;

		public string iconName;

		public int salePrice;

		public string unitName;

		public int category;

		public int recoveryValue;
	}

	public class ExchangeRatesData
	{
		public ExchangeRatesData(MstItemExchangeRatesData data)
		{
			this.targetItemId = data.targetItemId;
			this.sourceItemId = data.sourceItemId;
			this.useNum = data.useNum;
			this.gainNum = data.gainNum;
			this.monthlyExchangeLimit = data.monthlyExchangeLimit;
		}

		public int targetItemId;

		public int sourceItemId;

		public int useNum;

		public int gainNum;

		public int monthlyExchangeLimit;
	}
}
