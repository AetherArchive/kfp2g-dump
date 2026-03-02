using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class DataManagerCharaAccessory
{
	// Token: 0x06000410 RID: 1040 RVA: 0x0001BF6A File Offset: 0x0001A16A
	public DataManagerCharaAccessory(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000411 RID: 1041 RVA: 0x0001BF79 File Offset: 0x0001A179
	public int AccessoryStockLimit
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.accessoryStocKLimit;
		}
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0001BF8C File Offset: 0x0001A18C
	public List<DataManagerCharaAccessory.Accessory> GetUserAccessoryList()
	{
		List<DataManagerCharaAccessory.Accessory> list = new List<DataManagerCharaAccessory.Accessory>();
		foreach (DataManagerCharaAccessory.Accessory accessory in this.userAccMap.Values)
		{
			list.Add(accessory);
		}
		return list;
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x0001BFEC File Offset: 0x0001A1EC
	public DataManagerCharaAccessory.AccessoryData GetAccessoryData(int itemId)
	{
		this.accessoryMap.ContainsKey(itemId);
		return this.accessoryMap[itemId];
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0001C008 File Offset: 0x0001A208
	public int GetHaveNumByAccessoryItemId(int itemId)
	{
		return this.userAccMap.Values.Where<DataManagerCharaAccessory.Accessory>((DataManagerCharaAccessory.Accessory accessory) => accessory.ItemId == itemId).Count<DataManagerCharaAccessory.Accessory>();
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x0001C044 File Offset: 0x0001A244
	public void UpdateUserDataByServer(List<SGNFW.HttpRequest.Protocol.Accessory> chAccessoryList)
	{
		foreach (SGNFW.HttpRequest.Protocol.Accessory accessory in chAccessoryList)
		{
			if (this.userAccMap.ContainsKey(accessory.accessory_id))
			{
				int manage_status = accessory.manage_status;
				if (manage_status != 0)
				{
					if (manage_status - 1 <= 1)
					{
						this.userAccMap.Remove(accessory.accessory_id);
					}
				}
				else
				{
					this.userAccMap[accessory.accessory_id].Update(accessory);
				}
			}
			else
			{
				this.userAccMap.Add(accessory.accessory_id, new DataManagerCharaAccessory.Accessory(accessory));
			}
		}
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0001C0F4 File Offset: 0x0001A2F4
	public void UpdateFriendsAccessoryDataByServer(List<Chara> srvChList, List<SGNFW.HttpRequest.Protocol.Accessory> srvChAccList)
	{
		if (srvChList == null)
		{
			return;
		}
		if (srvChAccList == null)
		{
			return;
		}
		foreach (Chara chara in srvChList)
		{
			CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(chara.chara_id);
			if (userCharaData != null)
			{
				if (chara.accessory == null)
				{
					if (userCharaData.dynamicData.accessory != null)
					{
						userCharaData.dynamicData.accessory = null;
					}
				}
				else
				{
					userCharaData.dynamicData.accessory = this.userAccMap[chara.accessory.accessory_id];
				}
			}
		}
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x0001C19C File Offset: 0x0001A39C
	public void InitializeMstData(MstManager mstManager)
	{
		this.userAccMap = new Dictionary<long, DataManagerCharaAccessory.Accessory>();
		List<MstAccessoryRarityData> mst = Singleton<MstManager>.Instance.GetMst<List<MstAccessoryRarityData>>(MstType.ACCESSORY_RARITY_DATA);
		List<MstAccessoryData> mst2 = Singleton<MstManager>.Instance.GetMst<List<MstAccessoryData>>(MstType.ACCESSORY_DATA);
		this.rarityMap = new Dictionary<int, DataManagerCharaAccessory.RarityData>();
		foreach (MstAccessoryRarityData mstAccessoryRarityData in mst)
		{
			if (!this.rarityMap.ContainsKey(mstAccessoryRarityData.rarity))
			{
				this.rarityMap.Add(mstAccessoryRarityData.rarity, new DataManagerCharaAccessory.RarityData(mstAccessoryRarityData));
			}
		}
		List<ItemStaticBase> list = new List<ItemStaticBase>();
		this.accessoryMap = new Dictionary<int, DataManagerCharaAccessory.AccessoryData>();
		foreach (MstAccessoryData mstAccessoryData in mst2)
		{
			if (!this.accessoryMap.ContainsKey(mstAccessoryData.id))
			{
				DataManagerCharaAccessory.AccessoryData accessoryData = new DataManagerCharaAccessory.AccessoryData(mstAccessoryData, this.rarityMap[mstAccessoryData.rarity]);
				this.accessoryMap.Add(mstAccessoryData.id, accessoryData);
				list.Add(new DataManagerCharaAccessory.AccessoryStaticData(accessoryData));
			}
		}
		DataManager.DmItem.AddMstDataByItem(list);
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x0001C2E0 File Offset: 0x0001A4E0
	public void RequestActionUpdateStatus(DataManagerCharaAccessory.Accessory acc, bool isLock)
	{
		List<long> list = new List<long>();
		List<long> list2 = new List<long>();
		if (isLock)
		{
			list.Add(acc.UniqId);
		}
		else
		{
			list2.Add(acc.UniqId);
		}
		this.parentData.ServerRequest(AccessoryStatusCmd.Create(list, list2), new Action<Command>(this.CbStatusCmd));
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0001C334 File Offset: 0x0001A534
	public void RequestActionGrow(DataManagerCharaAccessory.Accessory acc, List<DataManagerCharaAccessory.Accessory> feedAccList)
	{
		List<long> list = feedAccList.ConvertAll<long>((DataManagerCharaAccessory.Accessory x) => x.UniqId);
		this.parentData.ServerRequest(AccessoryGrowCmd.Create(acc.UniqId, list), new Action<Command>(this.CbGrowCmd));
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0001C38C File Offset: 0x0001A58C
	public void RequestActionSell(List<DataManagerCharaAccessory.Accessory> sellAccList)
	{
		List<long> list = sellAccList.ConvertAll<long>((DataManagerCharaAccessory.Accessory x) => x.UniqId);
		this.parentData.ServerRequest(AccessorySellCmd.Create(list), new Action<Command>(this.CbSellCmd));
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0001C3DC File Offset: 0x0001A5DC
	public void RequestActionRemoveCharaAccessory(List<DataManagerCharaAccessory.Accessory> targetAccList)
	{
		Dictionary<int, List<long>> dictionary = new Dictionary<int, List<long>>();
		foreach (DataManagerCharaAccessory.Accessory accessory in targetAccList)
		{
			if (accessory.CharaId != 0)
			{
				if (dictionary.ContainsKey(accessory.CharaId))
				{
					dictionary[accessory.CharaId].Add(accessory.UniqId);
				}
				else
				{
					dictionary.Add(accessory.CharaId, new List<long> { accessory.UniqId });
				}
			}
		}
		List<EquipAccessory> list = new List<EquipAccessory>();
		foreach (int num in dictionary.Keys)
		{
			CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(num);
			if (userCharaData.dynamicData.accessory != null && dictionary[num].Contains(userCharaData.dynamicData.accessory.UniqId))
			{
				list.Add(new EquipAccessory
				{
					chara_id = num,
					accessory_id = 0L
				});
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		this.parentData.ServerRequest(CharaAccessoryEquipCmd.Create(list), new Action<Command>(this.CbRemoveCharaAccessory));
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0001C538 File Offset: 0x0001A738
	private void CbStatusCmd(Command cmd)
	{
		AccessoryStatusResponse accessoryStatusResponse = cmd.response as AccessoryStatusResponse;
		this.parentData.UpdateUserAssetByAssets(accessoryStatusResponse.assets);
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0001C564 File Offset: 0x0001A764
	private void CbGrowCmd(Command cmd)
	{
		AccessoryGrowResponse accessoryGrowResponse = cmd.response as AccessoryGrowResponse;
		this.parentData.UpdateUserAssetByAssets(accessoryGrowResponse.assets);
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0001C590 File Offset: 0x0001A790
	private void CbSellCmd(Command cmd)
	{
		AccessorySellResponse accessorySellResponse = cmd.response as AccessorySellResponse;
		this.parentData.UpdateUserAssetByAssets(accessorySellResponse.assets);
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x0001C5BC File Offset: 0x0001A7BC
	private void CbRemoveCharaAccessory(Command cmd)
	{
		CharaAccessoryEquipResponse charaAccessoryEquipResponse = cmd.response as CharaAccessoryEquipResponse;
		this.parentData.UpdateUserAssetByAssets(charaAccessoryEquipResponse.assets);
	}

	// Token: 0x040004E2 RID: 1250
	private DataManager parentData;

	// Token: 0x040004E3 RID: 1251
	private Dictionary<long, DataManagerCharaAccessory.Accessory> userAccMap;

	// Token: 0x040004E4 RID: 1252
	private Dictionary<int, DataManagerCharaAccessory.RarityData> rarityMap;

	// Token: 0x040004E5 RID: 1253
	private Dictionary<int, DataManagerCharaAccessory.AccessoryData> accessoryMap;

	// Token: 0x02000659 RID: 1625
	public enum DispType
	{
		// Token: 0x04002E98 RID: 11928
		None,
		// Token: 0x04002E99 RID: 11929
		Always,
		// Token: 0x04002E9A RID: 11930
		Battle
	}

	// Token: 0x0200065A RID: 1626
	public enum DispPosition
	{
		// Token: 0x04002E9C RID: 11932
		Origin,
		// Token: 0x04002E9D RID: 11933
		Head,
		// Token: 0x04002E9E RID: 11934
		Body,
		// Token: 0x04002E9F RID: 11935
		RightHand,
		// Token: 0x04002EA0 RID: 11936
		LeftHand,
		// Token: 0x04002EA1 RID: 11937
		Camera
	}

	// Token: 0x0200065B RID: 1627
	public class Accessory
	{
		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060030E4 RID: 12516 RVA: 0x001BC75D File Offset: 0x001BA95D
		// (set) Token: 0x060030E5 RID: 12517 RVA: 0x001BC765 File Offset: 0x001BA965
		public long UniqId { get; private set; }

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060030E6 RID: 12518 RVA: 0x001BC76E File Offset: 0x001BA96E
		// (set) Token: 0x060030E7 RID: 12519 RVA: 0x001BC776 File Offset: 0x001BA976
		public int ItemId { get; private set; }

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060030E8 RID: 12520 RVA: 0x001BC77F File Offset: 0x001BA97F
		// (set) Token: 0x060030E9 RID: 12521 RVA: 0x001BC787 File Offset: 0x001BA987
		public int Level { get; private set; }

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060030EA RID: 12522 RVA: 0x001BC790 File Offset: 0x001BA990
		// (set) Token: 0x060030EB RID: 12523 RVA: 0x001BC798 File Offset: 0x001BA998
		public int CharaId { get; private set; }

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060030EC RID: 12524 RVA: 0x001BC7A1 File Offset: 0x001BA9A1
		// (set) Token: 0x060030ED RID: 12525 RVA: 0x001BC7A9 File Offset: 0x001BA9A9
		public bool IsLock { get; private set; }

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060030EE RID: 12526 RVA: 0x001BC7B2 File Offset: 0x001BA9B2
		// (set) Token: 0x060030EF RID: 12527 RVA: 0x001BC7BA File Offset: 0x001BA9BA
		public DateTime GetTime { get; private set; }

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060030F0 RID: 12528 RVA: 0x001BC7C3 File Offset: 0x001BA9C3
		// (set) Token: 0x060030F1 RID: 12529 RVA: 0x001BC7CB File Offset: 0x001BA9CB
		public DateTime UpdateTime { get; private set; }

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060030F2 RID: 12530 RVA: 0x001BC7D4 File Offset: 0x001BA9D4
		// (set) Token: 0x060030F3 RID: 12531 RVA: 0x001BC7DC File Offset: 0x001BA9DC
		public DataManagerCharaAccessory.LevelParam Param { get; private set; }

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060030F4 RID: 12532 RVA: 0x001BC7E5 File Offset: 0x001BA9E5
		// (set) Token: 0x060030F5 RID: 12533 RVA: 0x001BC7ED File Offset: 0x001BA9ED
		public DataManagerCharaAccessory.AccessoryData AccessoryData { get; private set; }

		// Token: 0x060030F6 RID: 12534 RVA: 0x001BC7F8 File Offset: 0x001BA9F8
		public Accessory(SGNFW.HttpRequest.Protocol.Accessory acc)
		{
			this.UniqId = acc.accessory_id;
			this.ItemId = acc.item_id;
			this.AccessoryData = DataManager.DmChAccessory.GetAccessoryData(this.ItemId);
			this.Level = acc.level;
			this.CharaId = acc.owner_id;
			this.IsLock = acc.lock_flg == 1;
			this.GetTime = new DateTime(PrjUtil.ConvertTimeToTicks(acc.insert_time));
			this.UpdateTime = new DateTime(PrjUtil.ConvertTimeToTicks(acc.update_time));
			this.Param = this.AccessoryData.GrowSimulate(this.Level);
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x001BC8A4 File Offset: 0x001BAAA4
		public void Update(SGNFW.HttpRequest.Protocol.Accessory acc)
		{
			this.Level = acc.level;
			this.CharaId = acc.owner_id;
			this.IsLock = acc.lock_flg == 1;
			this.UpdateTime = new DateTime(PrjUtil.ConvertTimeToTicks(acc.update_time));
			this.Param = this.AccessoryData.GrowSimulate(this.Level);
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x001BC908 File Offset: 0x001BAB08
		public DataManagerCharaAccessory.LevelParam GrowSimulate(List<DataManagerCharaAccessory.Accessory> feedList)
		{
			int num = this.Level;
			foreach (DataManagerCharaAccessory.Accessory accessory in feedList)
			{
				DataManagerCharaAccessory.AccessoryData accessoryData = DataManager.DmChAccessory.GetAccessoryData(accessory.ItemId);
				if (0 < accessoryData.LevelupNum)
				{
					num += accessoryData.LevelupNum;
				}
				else
				{
					num += accessory.Level;
				}
				if (this.AccessoryData.Rarity.LevelLimit <= num)
				{
					num = this.AccessoryData.Rarity.LevelLimit;
					break;
				}
			}
			return this.AccessoryData.GrowSimulate(num);
		}
	}

	// Token: 0x0200065C RID: 1628
	public class AccessoryData
	{
		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060030F9 RID: 12537 RVA: 0x001BC9B8 File Offset: 0x001BABB8
		// (set) Token: 0x060030FA RID: 12538 RVA: 0x001BC9C0 File Offset: 0x001BABC0
		public int ItemId { get; private set; }

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060030FB RID: 12539 RVA: 0x001BC9C9 File Offset: 0x001BABC9
		// (set) Token: 0x060030FC RID: 12540 RVA: 0x001BC9D1 File Offset: 0x001BABD1
		public DataManagerCharaAccessory.RarityData Rarity { get; private set; }

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x001BC9DA File Offset: 0x001BABDA
		// (set) Token: 0x060030FE RID: 12542 RVA: 0x001BC9E2 File Offset: 0x001BABE2
		public string Name { get; private set; }

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060030FF RID: 12543 RVA: 0x001BC9EB File Offset: 0x001BABEB
		// (set) Token: 0x06003100 RID: 12544 RVA: 0x001BC9F3 File Offset: 0x001BABF3
		public string Reading { get; private set; }

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x001BC9FC File Offset: 0x001BABFC
		// (set) Token: 0x06003102 RID: 12546 RVA: 0x001BCA04 File Offset: 0x001BAC04
		public string FlavorText { get; private set; }

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x001BCA10 File Offset: 0x001BAC10
		private string IconName
		{
			get
			{
				return "icon_accessory_" + this.ItemId.ToString("000000");
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06003104 RID: 12548 RVA: 0x001BCA3A File Offset: 0x001BAC3A
		public string IconPath
		{
			get
			{
				return "Texture2D/Icon_Accessory/" + this.IconName;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x001BCA4C File Offset: 0x001BAC4C
		// (set) Token: 0x06003106 RID: 12550 RVA: 0x001BCA54 File Offset: 0x001BAC54
		public DataManagerCharaAccessory.DispType DispType { get; private set; }

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x001BCA5D File Offset: 0x001BAC5D
		// (set) Token: 0x06003108 RID: 12552 RVA: 0x001BCA65 File Offset: 0x001BAC65
		public bool IsAnchor { get; private set; }

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x001BCA6E File Offset: 0x001BAC6E
		// (set) Token: 0x0600310A RID: 12554 RVA: 0x001BCA76 File Offset: 0x001BAC76
		public List<DataManagerCharaAccessory.DispData> DispDataList { get; private set; }

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x001BCA7F File Offset: 0x001BAC7F
		// (set) Token: 0x0600310C RID: 12556 RVA: 0x001BCA87 File Offset: 0x001BAC87
		private DataManagerCharaAccessory.LevelParam LevelParamMin { get; set; }

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x0600310D RID: 12557 RVA: 0x001BCA90 File Offset: 0x001BAC90
		// (set) Token: 0x0600310E RID: 12558 RVA: 0x001BCA98 File Offset: 0x001BAC98
		private DataManagerCharaAccessory.LevelParam LevelParamMiddle { get; set; }

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600310F RID: 12559 RVA: 0x001BCAA1 File Offset: 0x001BACA1
		// (set) Token: 0x06003110 RID: 12560 RVA: 0x001BCAA9 File Offset: 0x001BACA9
		private DataManagerCharaAccessory.LevelParam LevelParamMax { get; set; }

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06003111 RID: 12561 RVA: 0x001BCAB2 File Offset: 0x001BACB2
		// (set) Token: 0x06003112 RID: 12562 RVA: 0x001BCABA File Offset: 0x001BACBA
		public int LevelupNum { get; private set; }

		// Token: 0x06003113 RID: 12563 RVA: 0x001BCAC4 File Offset: 0x001BACC4
		public AccessoryData(MstAccessoryData mstAcc, DataManagerCharaAccessory.RarityData rarityData)
		{
			this.ItemId = mstAcc.id;
			this.Rarity = rarityData;
			this.Name = mstAcc.name;
			this.Reading = mstAcc.reading;
			this.FlavorText = mstAcc.flavorText;
			this.DispType = (DataManagerCharaAccessory.DispType)mstAcc.dispType;
			this.IsAnchor = mstAcc.isAnchor == 1;
			this.DispDataList = new List<DataManagerCharaAccessory.DispData>
			{
				new DataManagerCharaAccessory.DispData(mstAcc.dispEffect01, mstAcc.dispPos01),
				new DataManagerCharaAccessory.DispData(mstAcc.dispEffect02, mstAcc.dispPos02),
				new DataManagerCharaAccessory.DispData(mstAcc.dispEffect03, mstAcc.dispPos03),
				new DataManagerCharaAccessory.DispData(mstAcc.dispEffect04, mstAcc.dispPos04)
			};
			this.LevelupNum = mstAcc.levelupNum;
			this.LevelParamMin = new DataManagerCharaAccessory.LevelParam(1, mstAcc.hpParamLv1, mstAcc.atkParamLv1, mstAcc.defParamLv1, mstAcc.avoidParamLv1, mstAcc.beatParamLv1, mstAcc.actionParamLv1, mstAcc.tryParamLv1);
			this.LevelParamMiddle = new DataManagerCharaAccessory.LevelParam(mstAcc.lvMiddleNum, mstAcc.hpParamLvMiddle, mstAcc.atkParamLvMiddle, mstAcc.defParamLvMiddle, mstAcc.avoidParamLvMiddle, mstAcc.beatParamLvMiddle, mstAcc.actionParamLvMiddle, mstAcc.tryParamLvMiddle);
			this.LevelParamMax = new DataManagerCharaAccessory.LevelParam(mstAcc.lvMaxNum, mstAcc.hpParamLvMax, mstAcc.atkParamLvMax, mstAcc.defParamLvMax, mstAcc.avoidParamLvMax, mstAcc.beatParamLvMax, mstAcc.actionParamLvMax, mstAcc.tryParamLvMax);
			int count = this.LevelParamMin.ParamCheck().Count;
			int count2 = this.LevelParamMiddle.ParamCheck().Count;
			int count3 = this.LevelParamMax.ParamCheck().Count;
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x001BCC81 File Offset: 0x001BAE81
		public DataManagerCharaAccessory.LevelParam GrowSimulate(int level)
		{
			if (level < this.LevelParamMiddle.Level)
			{
				return DataManagerCharaAccessory.AccessoryData.<GrowSimulate>g__CalcParam|53_0(this.LevelParamMin, this.LevelParamMiddle, level);
			}
			return DataManagerCharaAccessory.AccessoryData.<GrowSimulate>g__CalcParam|53_0(this.LevelParamMiddle, this.LevelParamMax, level);
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x001BCCB8 File Offset: 0x001BAEB8
		[CompilerGenerated]
		internal static DataManagerCharaAccessory.LevelParam <GrowSimulate>g__CalcParam|53_0(DataManagerCharaAccessory.LevelParam loParam, DataManagerCharaAccessory.LevelParam hiParam, int lv)
		{
			if (loParam.Level == hiParam.Level)
			{
				return new DataManagerCharaAccessory.LevelParam(hiParam.Level, hiParam.Hp, hiParam.Atk, hiParam.Def, hiParam.Avoid, hiParam.Beat, hiParam.Action, hiParam.Try);
			}
			int level = loParam.Level;
			int num = hiParam.Level - level;
			float num2 = (float)(lv - level) / (float)num;
			int num3 = (int)Mathf.Lerp((float)loParam.Hp, (float)hiParam.Hp, num2);
			int num4 = (int)Mathf.Lerp((float)loParam.Atk, (float)hiParam.Atk, num2);
			int num5 = (int)Mathf.Lerp((float)loParam.Def, (float)hiParam.Def, num2);
			int num6 = (int)Mathf.Lerp((float)loParam.Avoid, (float)hiParam.Avoid, num2);
			int num7 = (int)Mathf.Lerp((float)loParam.Beat, (float)hiParam.Beat, num2);
			int num8 = (int)Mathf.Lerp((float)loParam.Action, (float)hiParam.Action, num2);
			int num9 = (int)Mathf.Lerp((float)loParam.Try, (float)hiParam.Try, num2);
			return new DataManagerCharaAccessory.LevelParam(lv, num3, num4, num5, num6, num7, num8, num9);
		}
	}

	// Token: 0x0200065D RID: 1629
	public class AccessoryStaticData : ItemStaticBase
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06003116 RID: 12566 RVA: 0x001BCDD4 File Offset: 0x001BAFD4
		// (set) Token: 0x06003117 RID: 12567 RVA: 0x001BCDDC File Offset: 0x001BAFDC
		public DataManagerCharaAccessory.AccessoryData Accessory { get; private set; }

		// Token: 0x06003118 RID: 12568 RVA: 0x001BCDE5 File Offset: 0x001BAFE5
		public override int GetId()
		{
			return this.Accessory.ItemId;
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x001BCDF2 File Offset: 0x001BAFF2
		public override ItemDef.Kind GetKind()
		{
			return ItemDef.Kind.ACCESSORY_ITEM;
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x001BCDF6 File Offset: 0x001BAFF6
		public override string GetName()
		{
			return this.Accessory.Name;
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x001BCE03 File Offset: 0x001BB003
		public override string GetInfo()
		{
			return this.Accessory.FlavorText;
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x001BCE10 File Offset: 0x001BB010
		public override ItemDef.Rarity GetRarity()
		{
			return (ItemDef.Rarity)this.Accessory.Rarity.Rarity;
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x001BCE22 File Offset: 0x001BB022
		public override int GetStackMax()
		{
			return 99999999;
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x001BCE29 File Offset: 0x001BB029
		public override string GetIconName()
		{
			return this.Accessory.IconPath;
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x001BCE36 File Offset: 0x001BB036
		public override int GetSalePrice()
		{
			return this.Accessory.Rarity.SellPrice;
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x001BCE48 File Offset: 0x001BB048
		public AccessoryStaticData(DataManagerCharaAccessory.AccessoryData accData)
		{
			this.Accessory = accData;
		}

		// Token: 0x04002EB8 RID: 11960
		public new DateTime? endTime;
	}

	// Token: 0x0200065E RID: 1630
	public class RarityData
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06003121 RID: 12577 RVA: 0x001BCE57 File Offset: 0x001BB057
		// (set) Token: 0x06003122 RID: 12578 RVA: 0x001BCE5F File Offset: 0x001BB05F
		public int Rarity { get; private set; }

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06003123 RID: 12579 RVA: 0x001BCE68 File Offset: 0x001BB068
		// (set) Token: 0x06003124 RID: 12580 RVA: 0x001BCE70 File Offset: 0x001BB070
		public string Name { get; private set; }

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06003125 RID: 12581 RVA: 0x001BCE79 File Offset: 0x001BB079
		// (set) Token: 0x06003126 RID: 12582 RVA: 0x001BCE81 File Offset: 0x001BB081
		public int LevelLimit { get; private set; }

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06003127 RID: 12583 RVA: 0x001BCE8A File Offset: 0x001BB08A
		// (set) Token: 0x06003128 RID: 12584 RVA: 0x001BCE92 File Offset: 0x001BB092
		public int SellPrice { get; private set; }

		// Token: 0x06003129 RID: 12585 RVA: 0x001BCE9B File Offset: 0x001BB09B
		public RarityData(MstAccessoryRarityData mstRarity)
		{
			this.Rarity = mstRarity.rarity;
			this.Name = mstRarity.rarityName;
			this.LevelLimit = mstRarity.levelLimit;
			this.SellPrice = mstRarity.sellPrice;
		}
	}

	// Token: 0x0200065F RID: 1631
	public class DispData
	{
		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x0600312A RID: 12586 RVA: 0x001BCED3 File Offset: 0x001BB0D3
		// (set) Token: 0x0600312B RID: 12587 RVA: 0x001BCEDB File Offset: 0x001BB0DB
		public string Name { get; private set; }

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x001BCEE4 File Offset: 0x001BB0E4
		// (set) Token: 0x0600312D RID: 12589 RVA: 0x001BCEEC File Offset: 0x001BB0EC
		public DataManagerCharaAccessory.DispPosition Position { get; private set; }

		// Token: 0x0600312E RID: 12590 RVA: 0x001BCEF5 File Offset: 0x001BB0F5
		public DispData(string name, int pos)
		{
			this.Name = name;
			this.Position = (DataManagerCharaAccessory.DispPosition)pos;
		}
	}

	// Token: 0x02000660 RID: 1632
	public class LevelParam
	{
		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x001BCF0B File Offset: 0x001BB10B
		// (set) Token: 0x06003130 RID: 12592 RVA: 0x001BCF13 File Offset: 0x001BB113
		public int Level { get; private set; }

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06003131 RID: 12593 RVA: 0x001BCF1C File Offset: 0x001BB11C
		// (set) Token: 0x06003132 RID: 12594 RVA: 0x001BCF24 File Offset: 0x001BB124
		public int Hp { get; private set; }

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06003133 RID: 12595 RVA: 0x001BCF2D File Offset: 0x001BB12D
		// (set) Token: 0x06003134 RID: 12596 RVA: 0x001BCF35 File Offset: 0x001BB135
		public int Atk { get; private set; }

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06003135 RID: 12597 RVA: 0x001BCF3E File Offset: 0x001BB13E
		// (set) Token: 0x06003136 RID: 12598 RVA: 0x001BCF46 File Offset: 0x001BB146
		public int Def { get; private set; }

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06003137 RID: 12599 RVA: 0x001BCF4F File Offset: 0x001BB14F
		// (set) Token: 0x06003138 RID: 12600 RVA: 0x001BCF57 File Offset: 0x001BB157
		public int Avoid { get; private set; }

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x001BCF60 File Offset: 0x001BB160
		// (set) Token: 0x0600313A RID: 12602 RVA: 0x001BCF68 File Offset: 0x001BB168
		public int Beat { get; private set; }

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x001BCF71 File Offset: 0x001BB171
		// (set) Token: 0x0600313C RID: 12604 RVA: 0x001BCF79 File Offset: 0x001BB179
		public int Action { get; private set; }

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x0600313D RID: 12605 RVA: 0x001BCF82 File Offset: 0x001BB182
		// (set) Token: 0x0600313E RID: 12606 RVA: 0x001BCF8A File Offset: 0x001BB18A
		public int Try { get; private set; }

		// Token: 0x0600313F RID: 12607 RVA: 0x001BCF94 File Offset: 0x001BB194
		public LevelParam(int inLv, int inHp, int inAtk, int inDef, int inAvoid, int inBeat, int inAction, int inTry)
		{
			this.Level = inLv;
			this.Hp = inHp;
			this.Atk = inAtk;
			this.Def = inDef;
			this.Avoid = inAvoid;
			this.Beat = inBeat;
			this.Action = inAction;
			this.Try = inTry;
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x001BCFE4 File Offset: 0x001BB1E4
		public List<string> ParamCheck()
		{
			List<string> list = new List<string>();
			if (0 < this.Hp)
			{
				list.Add("Hp");
			}
			if (0 < this.Atk)
			{
				list.Add("Atk");
			}
			if (0 < this.Def)
			{
				list.Add("Def");
			}
			if (0 < this.Avoid)
			{
				list.Add("Avoid");
			}
			if (0 < this.Beat)
			{
				list.Add("Beat");
			}
			if (0 < this.Action)
			{
				list.Add("Action");
			}
			if (0 < this.Try)
			{
				list.Add("Try");
			}
			return list;
		}
	}
}
