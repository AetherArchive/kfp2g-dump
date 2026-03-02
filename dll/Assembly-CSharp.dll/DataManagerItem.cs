using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x02000088 RID: 136
public class DataManagerItem
{
	// Token: 0x06000528 RID: 1320 RVA: 0x00023CB4 File Offset: 0x00021EB4
	public DataManagerItem(DataManager p)
	{
		this.parentData = p;
		this.userItemMap = new Dictionary<int, ItemData>();
		this.itemStaticMap = new Dictionary<int, ItemStaticBase>();
		this.UserItemBankMap = new Dictionary<int, ItemBank>();
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00023D05 File Offset: 0x00021F05
	public ItemStaticBase GetItemStaticBase(int itemId)
	{
		if (!this.itemStaticMap.ContainsKey(itemId))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerItem.GetItemStaticBase : 定義されていないID[" + itemId.ToString() + "]を取得しようとしました。プランナに連絡してください", null);
			return null;
		}
		return this.itemStaticMap[itemId];
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00023D3F File Offset: 0x00021F3F
	public Dictionary<int, ItemStaticBase> GetItemStaticMap()
	{
		return this.itemStaticMap;
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00023D47 File Offset: 0x00021F47
	public List<DataManagerItem.ExchangeRatesData> GetExchageRatesList()
	{
		return this.exchangeRatesList;
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00023D4F File Offset: 0x00021F4F
	public List<ExchangeExecuteCountInfo> GetExecuteCountInfos()
	{
		return this.executeCountInfos;
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00023D58 File Offset: 0x00021F58
	public bool IsNeededUpdateExecuteCountInfos()
	{
		return TimeManager.Now.Date != this.executeCountInfosLastUpdate.Date;
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x00023D82 File Offset: 0x00021F82
	public ItemData GetUserItemData(int itemId)
	{
		if (this.userItemMap.ContainsKey(itemId))
		{
			return new ItemData(itemId, this.userItemMap[itemId].num);
		}
		return new ItemData(itemId, 0);
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x00023DB1 File Offset: 0x00021FB1
	public Dictionary<int, ItemData> GetUserItemMap()
	{
		return this.userItemMap;
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000530 RID: 1328 RVA: 0x00023DB9 File Offset: 0x00021FB9
	// (set) Token: 0x06000531 RID: 1329 RVA: 0x00023DC1 File Offset: 0x00021FC1
	public Dictionary<int, ItemBank> UserItemBankMap { get; private set; }

	// Token: 0x06000532 RID: 1330 RVA: 0x00023DCC File Offset: 0x00021FCC
	public List<ItemData> GetUserTabItemList(Dictionary<int, ItemData> itemMap, DataManagerItem.DispType type)
	{
		List<ItemData> list = new List<ItemData>();
		list.AddRange(itemMap.Values);
		return this.GetUserDispItemList(list, type);
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00023DF4 File Offset: 0x00021FF4
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

	// Token: 0x06000534 RID: 1332 RVA: 0x00024130 File Offset: 0x00022330
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

	// Token: 0x06000535 RID: 1333 RVA: 0x000241A8 File Offset: 0x000223A8
	public HashSet<int> GetOldItemIdList()
	{
		return this.oldItemIdList;
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000536 RID: 1334 RVA: 0x000241B0 File Offset: 0x000223B0
	// (set) Token: 0x06000537 RID: 1335 RVA: 0x000241B8 File Offset: 0x000223B8
	public List<DataManagerItem.StaminaRecoveryItemData> StaminaRecoveryItemDataList { get; private set; }

	// Token: 0x06000538 RID: 1336 RVA: 0x000241C4 File Offset: 0x000223C4
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

	// Token: 0x06000539 RID: 1337 RVA: 0x00024310 File Offset: 0x00022510
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

	// Token: 0x0600053A RID: 1338 RVA: 0x0002438C File Offset: 0x0002258C
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

	// Token: 0x0600053B RID: 1339 RVA: 0x00024464 File Offset: 0x00022664
	public void ClearUserDataByDirect()
	{
		this.userItemMap.Clear();
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00024471 File Offset: 0x00022671
	private void InsertUpdateUserMap(int id, int num)
	{
		if (this.userItemMap.ContainsKey(id))
		{
			this.userItemMap[id] = new ItemData(id, num);
			return;
		}
		this.userItemMap.Add(id, new ItemData(id, num));
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x000244A8 File Offset: 0x000226A8
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

	// Token: 0x0600053E RID: 1342 RVA: 0x000247D8 File Offset: 0x000229D8
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

	// Token: 0x0600053F RID: 1343 RVA: 0x00024854 File Offset: 0x00022A54
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

	// Token: 0x06000540 RID: 1344 RVA: 0x000248C0 File Offset: 0x00022AC0
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

	// Token: 0x06000541 RID: 1345 RVA: 0x000248E4 File Offset: 0x00022AE4
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

	// Token: 0x06000542 RID: 1346 RVA: 0x0002497C File Offset: 0x00022B7C
	public bool IsAllAttributeAddExpItem(int itemId)
	{
		DataManagerServerMst.CharaLevelItem charaLevelItem = DataManager.DmServerMst.charaLevelItemDataList.Find((DataManagerServerMst.CharaLevelItem item) => item.itemId == itemId);
		return charaLevelItem != null && charaLevelItem.attribute == CharaDef.AttributeType.ALL;
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x000249C0 File Offset: 0x00022BC0
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

	// Token: 0x06000544 RID: 1348 RVA: 0x00024A00 File Offset: 0x00022C00
	public static bool IsExpectedItemStock(int itemId, long addNum)
	{
		if ((long)DataManagerItem.GetUserHaveNum(itemId) + addNum <= (long)DataManager.DmItem.GetItemStaticBase(itemId).GetStackMax())
		{
			return false;
		}
		int num = ((itemId == 30101) ? 30090 : 0);
		return num != 0 && DataManager.DmItem.GetUserItemData(num).num > 0;
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x00024A5C File Offset: 0x00022C5C
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

	// Token: 0x06000546 RID: 1350 RVA: 0x00024B76 File Offset: 0x00022D76
	public static bool isOverUserHaveMaxNum(int itemId, int addNum)
	{
		return (long)(DataManagerItem.GetUserHaveNum(itemId) + addNum) > DataManagerItem.GetUserHaveMaxNum(itemId, 0);
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00024B8A File Offset: 0x00022D8A
	public void RequestGetNewFlag()
	{
		this.parentData.ServerRequest(NewFlgGetListCmd.Create(0), new Action<Command>(this.CbNewFlgGetListCmd));
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00024BAC File Offset: 0x00022DAC
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

	// Token: 0x06000549 RID: 1353 RVA: 0x00024BFC File Offset: 0x00022DFC
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

	// Token: 0x0600054A RID: 1354 RVA: 0x00024C8C File Offset: 0x00022E8C
	public void RequestCharaPxChange(int charaId, int itemNum)
	{
		this.parentData.ServerRequest(CharaPxChangeCmd.Create(charaId, itemNum), new Action<Command>(this.CbCharaPxChange));
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x00024CAC File Offset: 0x00022EAC
	public void RequestCharaSpxChange(int charaId, int itemNum)
	{
		this.parentData.ServerRequest(CharaSpxChangeCmd.Create(charaId, itemNum), new Action<Command>(this.CbCharaSpxChange));
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x00024CCC File Offset: 0x00022ECC
	public void RequestItemExchange(int executeCount, int targetItemId)
	{
		this.parentData.ServerRequest(ItemExchangeCmd.Create(executeCount, targetItemId), new Action<Command>(this.CbItemExchange));
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x00024CEC File Offset: 0x00022EEC
	public void RequestExchangeExecuteList()
	{
		this.parentData.ServerRequest(ExchangeExecuteListCmd.Create(), new Action<Command>(this.CbExchangeExecuteList));
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00024D0C File Offset: 0x00022F0C
	public void CbNewFlgGetListCmd(Command cmd)
	{
		NewFlgGetListResponse newFlgGetListResponse = cmd.response as NewFlgGetListResponse;
		this.parentData.InsertNewList(newFlgGetListResponse.new_flg_list);
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00024D38 File Offset: 0x00022F38
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

	// Token: 0x06000550 RID: 1360 RVA: 0x00024DB0 File Offset: 0x00022FB0
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

	// Token: 0x06000551 RID: 1361 RVA: 0x00024E2C File Offset: 0x0002302C
	public void CbItemSellCmd(Command cmd)
	{
		ItemSellResponse itemSellResponse = cmd.response as ItemSellResponse;
		this.parentData.UpdateUserAssetByAssets(itemSellResponse.assets);
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00024E58 File Offset: 0x00023058
	public void CbCharaPxChange(Command cmd)
	{
		CharaPxChangeResponse charaPxChangeResponse = cmd.response as CharaPxChangeResponse;
		this.parentData.UpdateUserAssetByAssets(charaPxChangeResponse.assets);
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x00024E84 File Offset: 0x00023084
	public void CbCharaSpxChange(Command cmd)
	{
		CharaSpxChangeResponse charaSpxChangeResponse = cmd.response as CharaSpxChangeResponse;
		this.parentData.UpdateUserAssetByAssets(charaSpxChangeResponse.assets);
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00024EB0 File Offset: 0x000230B0
	public void CbItemExchange(Command cmd)
	{
		ItemExchangeResponse itemExchangeResponse = cmd.response as ItemExchangeResponse;
		this.parentData.UpdateUserAssetByAssets(itemExchangeResponse.assets);
		this.executeCountInfos = itemExchangeResponse.infoList;
		this.executeCountInfosLastUpdate = TimeManager.Now;
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x00024EF4 File Offset: 0x000230F4
	public void CbExchangeExecuteList(Command cmd)
	{
		ExchangeExecuteListResponse exchangeExecuteListResponse = cmd.response as ExchangeExecuteListResponse;
		this.executeCountInfos = exchangeExecuteListResponse.infoList;
		this.executeCountInfosLastUpdate = TimeManager.Now;
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00024F24 File Offset: 0x00023124
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

	// Token: 0x06000557 RID: 1367 RVA: 0x00024F54 File Offset: 0x00023154
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

	// Token: 0x06000558 RID: 1368 RVA: 0x00024FC4 File Offset: 0x000231C4
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

	// Token: 0x06000559 RID: 1369 RVA: 0x0002500C File Offset: 0x0002320C
	[CompilerGenerated]
	internal static void <InitializeMstData>g__OverLoadItem|33_0(int itemId)
	{
	}

	// Token: 0x04000573 RID: 1395
	private DataManager parentData;

	// Token: 0x04000574 RID: 1396
	private Dictionary<int, ItemStaticBase> itemStaticMap;

	// Token: 0x04000575 RID: 1397
	private Dictionary<int, int> itemCharaIdMap;

	// Token: 0x04000576 RID: 1398
	private HashSet<int> oldItemIdList = new HashSet<int>();

	// Token: 0x04000577 RID: 1399
	private List<DataManagerItem.ExchangeRatesData> exchangeRatesList = new List<DataManagerItem.ExchangeRatesData>();

	// Token: 0x04000578 RID: 1400
	private List<ExchangeExecuteCountInfo> executeCountInfos;

	// Token: 0x04000579 RID: 1401
	private DateTime executeCountInfosLastUpdate;

	// Token: 0x0400057A RID: 1402
	private Dictionary<int, ItemData> userItemMap;

	// Token: 0x020006D2 RID: 1746
	[Serializable]
	public class JsonData<T>
	{
		// Token: 0x040030A7 RID: 12455
		public T[] data;
	}

	// Token: 0x020006D3 RID: 1747
	public enum DispType
	{
		// Token: 0x040030A9 RID: 12457
		Undefined = -1,
		// Token: 0x040030AA RID: 12458
		Common,
		// Token: 0x040030AB RID: 12459
		Growth,
		// Token: 0x040030AC RID: 12460
		Decoration,
		// Token: 0x040030AD RID: 12461
		Photo,
		// Token: 0x040030AE RID: 12462
		PlayItem,
		// Token: 0x040030AF RID: 12463
		Accessory,
		// Token: 0x040030B0 RID: 12464
		Achievement
	}

	// Token: 0x020006D4 RID: 1748
	public class StaminaRecoveryItemData
	{
		// Token: 0x040030B1 RID: 12465
		public int id;

		// Token: 0x040030B2 RID: 12466
		public int rarity;

		// Token: 0x040030B3 RID: 12467
		public string name;

		// Token: 0x040030B4 RID: 12468
		public string flavorText;

		// Token: 0x040030B5 RID: 12469
		public int stackMax;

		// Token: 0x040030B6 RID: 12470
		public string iconName;

		// Token: 0x040030B7 RID: 12471
		public int salePrice;

		// Token: 0x040030B8 RID: 12472
		public string unitName;

		// Token: 0x040030B9 RID: 12473
		public int category;

		// Token: 0x040030BA RID: 12474
		public int recoveryValue;
	}

	// Token: 0x020006D5 RID: 1749
	public class ExchangeRatesData
	{
		// Token: 0x06003321 RID: 13089 RVA: 0x001C1308 File Offset: 0x001BF508
		public ExchangeRatesData(MstItemExchangeRatesData data)
		{
			this.targetItemId = data.targetItemId;
			this.sourceItemId = data.sourceItemId;
			this.useNum = data.useNum;
			this.gainNum = data.gainNum;
			this.monthlyExchangeLimit = data.monthlyExchangeLimit;
		}

		// Token: 0x040030BB RID: 12475
		public int targetItemId;

		// Token: 0x040030BC RID: 12476
		public int sourceItemId;

		// Token: 0x040030BD RID: 12477
		public int useNum;

		// Token: 0x040030BE RID: 12478
		public int gainNum;

		// Token: 0x040030BF RID: 12479
		public int monthlyExchangeLimit;
	}
}
