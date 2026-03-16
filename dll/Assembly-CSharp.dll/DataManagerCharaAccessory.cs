using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

public class DataManagerCharaAccessory
{
	public DataManagerCharaAccessory(DataManager p)
	{
		this.parentData = p;
	}

	public int AccessoryStockLimit
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.accessoryStocKLimit;
		}
	}

	public List<DataManagerCharaAccessory.Accessory> GetUserAccessoryList()
	{
		List<DataManagerCharaAccessory.Accessory> list = new List<DataManagerCharaAccessory.Accessory>();
		foreach (DataManagerCharaAccessory.Accessory accessory in this.userAccMap.Values)
		{
			list.Add(accessory);
		}
		return list;
	}

	public DataManagerCharaAccessory.AccessoryData GetAccessoryData(int itemId)
	{
		this.accessoryMap.ContainsKey(itemId);
		return this.accessoryMap[itemId];
	}

	public int GetHaveNumByAccessoryItemId(int itemId)
	{
		return this.userAccMap.Values.Where<DataManagerCharaAccessory.Accessory>((DataManagerCharaAccessory.Accessory accessory) => accessory.ItemId == itemId).Count<DataManagerCharaAccessory.Accessory>();
	}

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

	public void RequestActionGrow(DataManagerCharaAccessory.Accessory acc, List<DataManagerCharaAccessory.Accessory> feedAccList)
	{
		List<long> list = feedAccList.ConvertAll<long>((DataManagerCharaAccessory.Accessory x) => x.UniqId);
		this.parentData.ServerRequest(AccessoryGrowCmd.Create(acc.UniqId, list), new Action<Command>(this.CbGrowCmd));
	}

	public void RequestActionSell(List<DataManagerCharaAccessory.Accessory> sellAccList)
	{
		List<long> list = sellAccList.ConvertAll<long>((DataManagerCharaAccessory.Accessory x) => x.UniqId);
		this.parentData.ServerRequest(AccessorySellCmd.Create(list), new Action<Command>(this.CbSellCmd));
	}

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

	private void CbStatusCmd(Command cmd)
	{
		AccessoryStatusResponse accessoryStatusResponse = cmd.response as AccessoryStatusResponse;
		this.parentData.UpdateUserAssetByAssets(accessoryStatusResponse.assets);
	}

	private void CbGrowCmd(Command cmd)
	{
		AccessoryGrowResponse accessoryGrowResponse = cmd.response as AccessoryGrowResponse;
		this.parentData.UpdateUserAssetByAssets(accessoryGrowResponse.assets);
	}

	private void CbSellCmd(Command cmd)
	{
		AccessorySellResponse accessorySellResponse = cmd.response as AccessorySellResponse;
		this.parentData.UpdateUserAssetByAssets(accessorySellResponse.assets);
	}

	private void CbRemoveCharaAccessory(Command cmd)
	{
		CharaAccessoryEquipResponse charaAccessoryEquipResponse = cmd.response as CharaAccessoryEquipResponse;
		this.parentData.UpdateUserAssetByAssets(charaAccessoryEquipResponse.assets);
	}

	private DataManager parentData;

	private Dictionary<long, DataManagerCharaAccessory.Accessory> userAccMap;

	private Dictionary<int, DataManagerCharaAccessory.RarityData> rarityMap;

	private Dictionary<int, DataManagerCharaAccessory.AccessoryData> accessoryMap;

	public enum DispType
	{
		None,
		Always,
		Battle
	}

	public enum DispPosition
	{
		Origin,
		Head,
		Body,
		RightHand,
		LeftHand,
		Camera
	}

	public class Accessory
	{
		public long UniqId { get; private set; }

		public int ItemId { get; private set; }

		public int Level { get; private set; }

		public int CharaId { get; private set; }

		public bool IsLock { get; private set; }

		public DateTime GetTime { get; private set; }

		public DateTime UpdateTime { get; private set; }

		public DataManagerCharaAccessory.LevelParam Param { get; private set; }

		public DataManagerCharaAccessory.AccessoryData AccessoryData { get; private set; }

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

		public void Update(SGNFW.HttpRequest.Protocol.Accessory acc)
		{
			this.Level = acc.level;
			this.CharaId = acc.owner_id;
			this.IsLock = acc.lock_flg == 1;
			this.UpdateTime = new DateTime(PrjUtil.ConvertTimeToTicks(acc.update_time));
			this.Param = this.AccessoryData.GrowSimulate(this.Level);
		}

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

	public class AccessoryData
	{
		public int ItemId { get; private set; }

		public DataManagerCharaAccessory.RarityData Rarity { get; private set; }

		public string Name { get; private set; }

		public string Reading { get; private set; }

		public string FlavorText { get; private set; }

		private string IconName
		{
			get
			{
				return "icon_accessory_" + this.ItemId.ToString("000000");
			}
		}

		public string IconPath
		{
			get
			{
				return "Texture2D/Icon_Accessory/" + this.IconName;
			}
		}

		public DataManagerCharaAccessory.DispType DispType { get; private set; }

		public bool IsAnchor { get; private set; }

		public List<DataManagerCharaAccessory.DispData> DispDataList { get; private set; }

		private DataManagerCharaAccessory.LevelParam LevelParamMin { get; set; }

		private DataManagerCharaAccessory.LevelParam LevelParamMiddle { get; set; }

		private DataManagerCharaAccessory.LevelParam LevelParamMax { get; set; }

		public int LevelupNum { get; private set; }

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

		public DataManagerCharaAccessory.LevelParam GrowSimulate(int level)
		{
			if (level < this.LevelParamMiddle.Level)
			{
				return DataManagerCharaAccessory.AccessoryData.<GrowSimulate>g__CalcParam|53_0(this.LevelParamMin, this.LevelParamMiddle, level);
			}
			return DataManagerCharaAccessory.AccessoryData.<GrowSimulate>g__CalcParam|53_0(this.LevelParamMiddle, this.LevelParamMax, level);
		}

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

	public class AccessoryStaticData : ItemStaticBase
	{
		public DataManagerCharaAccessory.AccessoryData Accessory { get; private set; }

		public override int GetId()
		{
			return this.Accessory.ItemId;
		}

		public override ItemDef.Kind GetKind()
		{
			return ItemDef.Kind.ACCESSORY_ITEM;
		}

		public override string GetName()
		{
			return this.Accessory.Name;
		}

		public override string GetInfo()
		{
			return this.Accessory.FlavorText;
		}

		public override ItemDef.Rarity GetRarity()
		{
			return (ItemDef.Rarity)this.Accessory.Rarity.Rarity;
		}

		public override int GetStackMax()
		{
			return 99999999;
		}

		public override string GetIconName()
		{
			return this.Accessory.IconPath;
		}

		public override int GetSalePrice()
		{
			return this.Accessory.Rarity.SellPrice;
		}

		public AccessoryStaticData(DataManagerCharaAccessory.AccessoryData accData)
		{
			this.Accessory = accData;
		}

		public new DateTime? endTime;
	}

	public class RarityData
	{
		public int Rarity { get; private set; }

		public string Name { get; private set; }

		public int LevelLimit { get; private set; }

		public int SellPrice { get; private set; }

		public RarityData(MstAccessoryRarityData mstRarity)
		{
			this.Rarity = mstRarity.rarity;
			this.Name = mstRarity.rarityName;
			this.LevelLimit = mstRarity.levelLimit;
			this.SellPrice = mstRarity.sellPrice;
		}
	}

	public class DispData
	{
		public string Name { get; private set; }

		public DataManagerCharaAccessory.DispPosition Position { get; private set; }

		public DispData(string name, int pos)
		{
			this.Name = name;
			this.Position = (DataManagerCharaAccessory.DispPosition)pos;
		}
	}

	public class LevelParam
	{
		public int Level { get; private set; }

		public int Hp { get; private set; }

		public int Atk { get; private set; }

		public int Def { get; private set; }

		public int Avoid { get; private set; }

		public int Beat { get; private set; }

		public int Action { get; private set; }

		public int Try { get; private set; }

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
