using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerPicnic
{
	public DataManagerPicnic(DataManager p)
	{
		this.parentData = p;
	}

	public bool IsEnablePicnicData { get; private set; }

	public DataManagerPicnic.PicnicStatic PicnicStaticData { get; private set; }

	public DataManagerPicnic.PicnicDynamic PicnicDynamicData
	{
		get
		{
			return new DataManagerPicnic.PicnicDynamic(this.Energy, this.CharaDataList, this.PlayDataList, this.LastUpdateTime, this.UpdateItemList);
		}
	}

	public DataManagerPicnic.MenuBadge MenuBadgeData { get; private set; }

	private List<DataManagerPicnic.CharaData> CharaDataList { get; set; }

	private List<DataManagerPicnic.PlayData> PlayDataList { get; set; }

	private int Energy { get; set; }

	private DateTime LastUpdateTime { get; set; }

	private List<DataManagerPicnic.DropItemData> UpdateItemList { get; set; }

	public long StartTimeTick
	{
		get
		{
			return ((long)this.starttimeHigh << 32) + (long)((ulong)this.starttimeLow);
		}
		private set
		{
			this.starttimeHigh = (int)((value >> 32) & (long)((ulong)(-1)));
			this.starttimeLow = (int)(value & (long)((ulong)(-1)));
		}
	}

	public DateTime StartDateTime
	{
		get
		{
			return new DateTime(((long)this.starttimeHigh << 32) + (long)((ulong)this.starttimeLow));
		}
	}

	private void UpdatePicnicDataByServer(Picnic picnic)
	{
		this.IsEnablePicnicData = true;
		this.LastUpdateTime = TimeManager.Now;
		this.Energy = picnic.energy;
		this.CharaDataList = new List<DataManagerPicnic.CharaData>();
		this.PlayDataList = new List<DataManagerPicnic.PlayData>();
		this.UpdateItemList = new List<DataManagerPicnic.DropItemData>();
		bool flag;
		if (picnic == null)
		{
			flag = null != null;
		}
		else
		{
			Assets assets = picnic.assets;
			flag = ((assets != null) ? assets.update_item_list : null) != null;
		}
		if (flag)
		{
			using (List<Item>.Enumerator enumerator = picnic.assets.update_item_list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Item updateItem = enumerator.Current;
					if (this.PicnicStaticData.FoodDataList.Find((DataManagerPicnic.FoodData x) => x.Id == updateItem.item_id) == null)
					{
						bool flag2 = false;
						if (picnic.assets.lottery_item_list == null || picnic.assets.lottery_item_list.Find((LotteryItem x) => x.after_item.item_id == updateItem.item_id) == null)
						{
							using (List<DataManagerPicnic.PlayTypeData>.Enumerator enumerator2 = this.PicnicStaticData.PlayTypeDataList.GetEnumerator())
							{
								Predicate<DataManagerPicnic.PlayTypeData.GetItem> <>9__3;
								while (enumerator2.MoveNext())
								{
									DataManagerPicnic.PlayTypeData playTypeData = enumerator2.Current;
									List<DataManagerPicnic.PlayTypeData.GetItem> getItemList = playTypeData.GetItemList;
									Predicate<DataManagerPicnic.PlayTypeData.GetItem> predicate;
									if ((predicate = <>9__3) == null)
									{
										predicate = (<>9__3 = (DataManagerPicnic.PlayTypeData.GetItem x) => x.Item.itemId == updateItem.item_id);
									}
									if (getItemList.Find(predicate) != null)
									{
										flag2 = true;
										break;
									}
								}
								goto IL_013F;
							}
							goto IL_013C;
						}
						goto IL_013C;
						IL_013F:
						int num = 0;
						if (!flag2)
						{
							CharaStaticData charaStaticData = DataManager.DmChara.CharaStaticDataList.Find((CharaStaticData x) => x.baseData.rankItemId == updateItem.item_id);
							num = ((charaStaticData != null) ? charaStaticData.GetId() : 0);
							if (num == 0)
							{
								continue;
							}
						}
						DataManagerPicnic.DropItemData dropItemData = new DataManagerPicnic.DropItemData(num, updateItem.item_id, updateItem.item_num - DataManager.DmItem.GetUserItemData(updateItem.item_id).num);
						this.UpdateItemList.Add(dropItemData);
						continue;
						IL_013C:
						flag2 = true;
						goto IL_013F;
					}
				}
			}
		}
		using (List<PicnicChara>.Enumerator enumerator3 = picnic.charalist.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				PicnicChara chara = enumerator3.Current;
				this.CharaDataList.Add(new DataManagerPicnic.CharaData(chara));
				if (chara.chara_id > 0)
				{
					DataManagerPicnic.DropItemData dropItemData2 = this.UpdateItemList.Find((DataManagerPicnic.DropItemData x) => x.CharaId == chara.chara_id);
					if (dropItemData2 != null)
					{
						dropItemData2.MonthlyBonusNum += chara.bonus_num;
						dropItemData2.isCampaign = chara.is_campaign;
					}
				}
			}
		}
		foreach (PicnicPlay picnicPlay in picnic.playlist)
		{
			this.PlayDataList.Add(new DataManagerPicnic.PlayData(picnicPlay));
		}
		DataManagerMonthlyPack.PurchaseMonthlypackData monthlypackData = DataManager.DmMonthlyPack.nowPackData.MonthlypackData;
		if (monthlypackData != null)
		{
			for (int i = 0; i < monthlypackData.PicnicBuffFrameCount; i++)
			{
				this.CharaDataList[i].MonthlyBuffRatio = monthlypackData.PicnicBuffAddRatio;
			}
		}
		LocalPushUtil.ResolveNotification(LocalPushUtil.NotificationID.PICNIC_ENERGY_NOTHING);
	}

	public void RequestPicnicGetUserData()
	{
		this.parentData.ServerRequest(PicnicGetUserDataCmd.Create(0), new Action<Command>(this.CbPicnicGetUserDataCmd));
	}

	public void RequestPicnicSetCharaList(List<int> charaIdList)
	{
		this.parentData.ServerRequest(PicnicSetCharaListCmd.Create(charaIdList), new Action<Command>(this.CbPicnicPicnicSetCharaListCmd));
	}

	public void RequestPicnicSetPlayList(List<int> playIdList)
	{
		this.parentData.ServerRequest(PicnicSetPlayListCmd.Create(playIdList), new Action<Command>(this.CbPicnicPicnicSetPlayListCmd));
	}

	public void RequestPicnicStartTime()
	{
		if (this.starttimeLow == 0 && this.starttimeHigh == 0)
		{
			this.StartTimeTick = TimeManager.Now.Ticks;
			this.parentData.ServerRequest(NewFlgUpdateCmd.Create(this.CreateServerData()), new Action<Command>(this.CbNewFlgUpdateCmd));
		}
	}

	public void RequestPicnicStartTime(DateTime dt)
	{
		this.StartTimeTick = dt.Ticks;
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(this.CreateServerData()), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	public void RequestPicnicUseFood(List<int> useItemList)
	{
		if (useItemList == null)
		{
			return;
		}
		if (useItemList.Count == 0)
		{
			return;
		}
		this.parentData.ServerRequest(PicnicUseFoodCmd.Create(useItemList), new Action<Command>(this.CbPicnicUseFoodCmd));
	}

	public void RequestGetMenuBadgeData()
	{
		this.parentData.ServerRequest(PicnicGetUserDataCmd.Create(1), new Action<Command>(this.CbGetMenuBadgeData));
	}

	private void CbPicnicGetUserDataCmd(Command cmd)
	{
		PicnicGetUserDataResponse picnicGetUserDataResponse = cmd.response as PicnicGetUserDataResponse;
		this.UpdatePicnicDataByServer(picnicGetUserDataResponse.picnicdata);
		this.parentData.UpdateUserAssetByAssets(picnicGetUserDataResponse.picnicdata.assets);
	}

	private void CbPicnicPicnicSetCharaListCmd(Command cmd)
	{
		PicnicSetCharaListResponse picnicSetCharaListResponse = cmd.response as PicnicSetCharaListResponse;
		this.UpdatePicnicDataByServer(picnicSetCharaListResponse.picnicdata);
		this.parentData.UpdateUserAssetByAssets(picnicSetCharaListResponse.picnicdata.assets);
	}

	private void CbPicnicPicnicSetPlayListCmd(Command cmd)
	{
		PicnicSetPlayListResponse picnicSetPlayListResponse = cmd.response as PicnicSetPlayListResponse;
		this.UpdatePicnicDataByServer(picnicSetPlayListResponse.picnicdata);
		this.parentData.UpdateUserAssetByAssets(picnicSetPlayListResponse.picnicdata.assets);
	}

	public void CbNewFlgUpdateCmd(Command cmd)
	{
		NewFlgUpdateRequest newFlgUpdateRequest = cmd.request as NewFlgUpdateRequest;
		this.UpdateUserFlagByServer(newFlgUpdateRequest.new_flg_list);
	}

	public void CbPicnicUseFoodCmd(Command cmd)
	{
		PicnicUseFoodResponse picnicUseFoodResponse = cmd.response as PicnicUseFoodResponse;
		this.UpdatePicnicDataByServer(picnicUseFoodResponse.picnicdata);
		this.parentData.UpdateUserAssetByAssets(picnicUseFoodResponse.picnicdata.assets);
	}

	public void CbGetMenuBadgeData(Command cmd)
	{
		PicnicGetUserDataResponse picnicGetUserDataResponse = cmd.response as PicnicGetUserDataResponse;
		DataManagerPicnic.MenuBadge menuBadge = new DataManagerPicnic.MenuBadge();
		menuBadge.Energy = picnicGetUserDataResponse.picnicdata.energy;
		menuBadge.LastUpdateTime = TimeManager.Now;
		bool flag;
		if (picnicGetUserDataResponse.picnicdata.charalist != null)
		{
			flag = picnicGetUserDataResponse.picnicdata.charalist.Exists((PicnicChara item) => item.chara_id > 0);
		}
		else
		{
			flag = false;
		}
		menuBadge.IsCharaSet = flag;
		this.MenuBadgeData = menuBadge;
	}

	public void UpdateUserFlagByServer(List<NewFlg> newFlagList)
	{
		foreach (NewFlg newFlg in newFlagList)
		{
			if (newFlg.category == 5)
			{
				int any_id = newFlg.any_id;
				if (any_id != 1)
				{
					if (any_id == 2)
					{
						this.starttimeHigh = newFlg.new_mgmt_flg;
					}
				}
				else
				{
					this.starttimeLow = newFlg.new_mgmt_flg;
				}
			}
		}
	}

	public void InitializeMstData(MstManager mstManager)
	{
		List<MstPicnicFoodData> mst = mstManager.GetMst<List<MstPicnicFoodData>>(MstType.PICNIC_FOOD_DATA);
		List<MstPicnicGettimeData> mst2 = mstManager.GetMst<List<MstPicnicGettimeData>>(MstType.PICNIC_GETTIME_DATA);
		List<MstPicnicPlayTypeData> mst3 = mstManager.GetMst<List<MstPicnicPlayTypeData>>(MstType.PICNIC_PLAY_TYPE_DATA);
		List<MstPicnicPlayItemData> mst4 = mstManager.GetMst<List<MstPicnicPlayItemData>>(MstType.PICNIC_PLAY_ITEM_DATA);
		this.PicnicStaticData = new DataManagerPicnic.PicnicStatic(mst, mst2, mst3, mst4);
	}

	public List<NewFlg> CreateServerData()
	{
		return new List<NewFlg>
		{
			new NewFlg
			{
				category = 5,
				any_id = 1,
				new_mgmt_flg = this.starttimeLow
			},
			new NewFlg
			{
				category = 5,
				any_id = 2,
				new_mgmt_flg = this.starttimeHigh
			}
		};
	}

	private DataManager parentData;

	public readonly int active_time_max = 28800;

	public readonly int energy_max = 28800;

	public readonly int food_shop_id = 2000;

	private int starttimeHigh;

	private int starttimeLow;

	public class PicnicStatic
	{
		public int ActiveTimeMax { get; private set; }

		public int EnergyMax { get; private set; }

		public List<DataManagerPicnic.FoodData> FoodDataList { get; private set; }

		public List<DataManagerPicnic.GettimeData> GettimeDataList { get; private set; }

		public List<DataManagerPicnic.PlayTypeData> PlayTypeDataList { get; private set; }

		public List<DataManagerPicnic.PlayItemData> PlayItemDataList { get; private set; }

		public PicnicStatic(List<MstPicnicFoodData> mstFoodList, List<MstPicnicGettimeData> mstGettimeList, List<MstPicnicPlayTypeData> mstPlayTypeList, List<MstPicnicPlayItemData> mstPlayItemList)
		{
			this.FoodDataList = new List<DataManagerPicnic.FoodData>();
			foreach (MstPicnicFoodData mstPicnicFoodData in mstFoodList)
			{
				this.FoodDataList.Add(new DataManagerPicnic.FoodData(mstPicnicFoodData));
			}
			this.GettimeDataList = new List<DataManagerPicnic.GettimeData>();
			foreach (MstPicnicGettimeData mstPicnicGettimeData in mstGettimeList)
			{
				this.GettimeDataList.Add(new DataManagerPicnic.GettimeData(mstPicnicGettimeData));
			}
			this.PlayTypeDataList = new List<DataManagerPicnic.PlayTypeData>();
			foreach (MstPicnicPlayTypeData mstPicnicPlayTypeData in mstPlayTypeList)
			{
				this.PlayTypeDataList.Add(new DataManagerPicnic.PlayTypeData(mstPicnicPlayTypeData));
			}
			this.PlayItemDataList = new List<DataManagerPicnic.PlayItemData>();
			List<ItemStaticBase> list = new List<ItemStaticBase>();
			foreach (MstPicnicPlayItemData mstPicnicPlayItemData in mstPlayItemList)
			{
				DataManagerPicnic.PlayItemData playItemData = new DataManagerPicnic.PlayItemData(mstPicnicPlayItemData);
				this.PlayItemDataList.Add(playItemData);
				list.Add(playItemData);
			}
			DataManager.DmItem.AddMstDataByItem(list);
		}
	}

	public class FoodData
	{
		public int Id { get; private set; }

		public int AddEnergyNum { get; private set; }

		public FoodData(MstPicnicFoodData food)
		{
			this.Id = food.goodsId;
			this.AddEnergyNum = food.addEnergyNum;
		}
	}

	public class GettimeData
	{
		public int GettimeId { get; private set; }

		public int getItemTime { get; private set; }

		public int getItemRankMaxTime { get; private set; }

		public GettimeData(MstPicnicGettimeData gettime)
		{
			this.GettimeId = gettime.picnicGettimeId;
			this.getItemTime = gettime.getItemTime;
			this.getItemRankMaxTime = gettime.getItemRankmaxTime;
		}
	}

	public class PlayTypeData
	{
		public int PlayId { get; private set; }

		public int GetTime { get; private set; }

		public List<DataManagerPicnic.PlayTypeData.GetItem> GetItemList { get; private set; }

		public PlayTypeData(MstPicnicPlayTypeData playType)
		{
			this.PlayId = playType.playTypeId;
			this.GetTime = playType.getItemTime;
			this.GetItemList = new List<DataManagerPicnic.PlayTypeData.GetItem>();
			this.GetItemList.Add(new DataManagerPicnic.PlayTypeData.GetItem(0, playType.getItemId, playType.getItemNum));
			this.GetItemList.Add(new DataManagerPicnic.PlayTypeData.GetItem(1, playType.getItemIdGorgeouspack, playType.getItemNumGorgeouspack));
			this.GetItemList.Add(new DataManagerPicnic.PlayTypeData.GetItem(2, playType.getItemIdStandardpack, playType.getItemNumStandardpack));
			this.GetItemList.Add(new DataManagerPicnic.PlayTypeData.GetItem(3, playType.getItemIdLittlepack, playType.getItemNumLittlepack));
			this.GetItemList.Add(new DataManagerPicnic.PlayTypeData.GetItem(4, playType.getItemIdFirstpack, playType.getItemNumFirstpack));
		}

		public class GetItem
		{
			public int Id { get; private set; }

			public ItemInput Item { get; private set; }

			public GetItem(int id, int itemId, int itemNum)
			{
				this.Id = id;
				this.Item = new ItemInput(itemId, itemNum);
			}
		}
	}

	public class PlayItemData : ItemStaticBase
	{
		public override int GetId()
		{
			return this.ItemId;
		}

		public override ItemDef.Rarity GetRarity()
		{
			return (ItemDef.Rarity)this.Rarity;
		}

		public override ItemDef.Kind GetKind()
		{
			return ItemDef.Kind.PICNIC_PLAYITEM;
		}

		public override string GetName()
		{
			return this.Name;
		}

		public override string GetInfo()
		{
			return this.FlavorText;
		}

		public override int GetStackMax()
		{
			return this.StackMax;
		}

		public int GetCategory()
		{
			return this.Category;
		}

		public override string GetIconName()
		{
			return "Texture2D/Icon_Item/" + this.IconName;
		}

		public override int GetSalePrice()
		{
			return 0;
		}

		private int ItemId { get; set; }

		private int Rarity { get; set; }

		private string Name { get; set; }

		private string FlavorText { get; set; }

		private int StackMax { get; set; }

		private int Category { get; set; }

		private string IconName { get; set; }

		public int CharaReactionId { get; private set; }

		public int AttributeId { get; private set; }

		public int ReplaceItemNum { get; private set; }

		public string StagePackName { get; private set; }

		public List<string> FireworksEffectNameList { get; private set; }

		public PlayItemData(MstPicnicPlayItemData playItem)
		{
			this.ItemId = playItem.id;
			this.Rarity = playItem.rarity;
			this.Name = playItem.name;
			this.FlavorText = playItem.flavorText;
			this.StackMax = playItem.stackMax;
			this.Category = playItem.category;
			this.IconName = playItem.iconName;
			this.CharaReactionId = playItem.charaReactionId;
			this.AttributeId = playItem.attributeId;
			this.ReplaceItemNum = playItem.replaceItemNum;
			this.StagePackName = playItem.stagePackName;
			this.FireworksEffectNameList = new List<string> { playItem.fireworksEffectName01, playItem.fireworksEffectName02, playItem.fireworksEffectName03, playItem.fireworksEffectName04 };
			this.FireworksEffectNameList.RemoveAll((string item) => string.IsNullOrEmpty(item));
		}
	}

	public class PicnicDynamic
	{
		public List<DataManagerPicnic.CharaData> CharaDataList { get; private set; }

		public List<DataManagerPicnic.PlayData> PlayDataList { get; private set; }

		public int Energy { get; private set; }

		public DateTime LastUpdateTime { get; private set; }

		public List<DataManagerPicnic.DropItemData> UpdateItemList { get; private set; }

		public PicnicDynamic(int energy, List<DataManagerPicnic.CharaData> charaDataList, List<DataManagerPicnic.PlayData> playDataList, DateTime lastUpdate, List<DataManagerPicnic.DropItemData> updateItemlist)
		{
			this.Energy = energy;
			this.CharaDataList = charaDataList;
			this.PlayDataList = playDataList;
			this.LastUpdateTime = lastUpdate;
			this.UpdateItemList = updateItemlist;
		}
	}

	public class CharaData
	{
		public int CharaId { get; private set; }

		public long RestTime { get; set; }

		public bool IsRankMax { get; private set; }

		public int GettimeId
		{
			get
			{
				return DataManager.DmChara.GetCharaStaticData(this.CharaId).baseData.picnicGettimeId;
			}
		}

		public int MonthlyBuffRatio { get; set; }

		public CharaData(PicnicChara picnicChar)
		{
			this.CharaId = picnicChar.chara_id;
			this.RestTime = picnicChar.resttime;
			this.IsRankMax = picnicChar.rankmax != 0;
		}
	}

	public class PlayData
	{
		public int PlayId { get; private set; }

		public long RestTime { get; set; }

		public int MonthlyBuffRatio { get; set; }

		public PlayData(PicnicPlay picnicPlay)
		{
			this.PlayId = picnicPlay.play_id;
			this.RestTime = picnicPlay.resttime;
		}
	}

	public class DropItemData
	{
		public int CharaId { get; private set; }

		public int ItemId { get; private set; }

		public int ItemNum { get; set; }

		public int MonthlyBonusNum { get; set; }

		public bool isCampaign { get; set; }

		public DropItemData(int chid, int itemid, int itemnum)
		{
			this.CharaId = chid;
			this.ItemId = itemid;
			this.ItemNum = itemnum;
		}
	}

	public class MenuBadge
	{
		public int Energy { get; set; }

		public DateTime LastUpdateTime { get; set; }

		public bool IsCharaSet { get; set; }
	}
}
