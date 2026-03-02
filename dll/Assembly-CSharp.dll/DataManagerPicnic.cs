using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x02000098 RID: 152
public class DataManagerPicnic
{
	// Token: 0x0600062B RID: 1579 RVA: 0x0002A190 File Offset: 0x00028390
	public DataManagerPicnic(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x0600062C RID: 1580 RVA: 0x0002A1C0 File Offset: 0x000283C0
	// (set) Token: 0x0600062D RID: 1581 RVA: 0x0002A1C8 File Offset: 0x000283C8
	public bool IsEnablePicnicData { get; private set; }

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x0600062E RID: 1582 RVA: 0x0002A1D1 File Offset: 0x000283D1
	// (set) Token: 0x0600062F RID: 1583 RVA: 0x0002A1D9 File Offset: 0x000283D9
	public DataManagerPicnic.PicnicStatic PicnicStaticData { get; private set; }

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000630 RID: 1584 RVA: 0x0002A1E2 File Offset: 0x000283E2
	public DataManagerPicnic.PicnicDynamic PicnicDynamicData
	{
		get
		{
			return new DataManagerPicnic.PicnicDynamic(this.Energy, this.CharaDataList, this.PlayDataList, this.LastUpdateTime, this.UpdateItemList);
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000631 RID: 1585 RVA: 0x0002A207 File Offset: 0x00028407
	// (set) Token: 0x06000632 RID: 1586 RVA: 0x0002A20F File Offset: 0x0002840F
	public DataManagerPicnic.MenuBadge MenuBadgeData { get; private set; }

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000633 RID: 1587 RVA: 0x0002A218 File Offset: 0x00028418
	// (set) Token: 0x06000634 RID: 1588 RVA: 0x0002A220 File Offset: 0x00028420
	private List<DataManagerPicnic.CharaData> CharaDataList { get; set; }

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000635 RID: 1589 RVA: 0x0002A229 File Offset: 0x00028429
	// (set) Token: 0x06000636 RID: 1590 RVA: 0x0002A231 File Offset: 0x00028431
	private List<DataManagerPicnic.PlayData> PlayDataList { get; set; }

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000637 RID: 1591 RVA: 0x0002A23A File Offset: 0x0002843A
	// (set) Token: 0x06000638 RID: 1592 RVA: 0x0002A242 File Offset: 0x00028442
	private int Energy { get; set; }

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000639 RID: 1593 RVA: 0x0002A24B File Offset: 0x0002844B
	// (set) Token: 0x0600063A RID: 1594 RVA: 0x0002A253 File Offset: 0x00028453
	private DateTime LastUpdateTime { get; set; }

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x0600063B RID: 1595 RVA: 0x0002A25C File Offset: 0x0002845C
	// (set) Token: 0x0600063C RID: 1596 RVA: 0x0002A264 File Offset: 0x00028464
	private List<DataManagerPicnic.DropItemData> UpdateItemList { get; set; }

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x0600063D RID: 1597 RVA: 0x0002A26D File Offset: 0x0002846D
	// (set) Token: 0x0600063E RID: 1598 RVA: 0x0002A281 File Offset: 0x00028481
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

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x0600063F RID: 1599 RVA: 0x0002A29C File Offset: 0x0002849C
	public DateTime StartDateTime
	{
		get
		{
			return new DateTime(((long)this.starttimeHigh << 32) + (long)((ulong)this.starttimeLow));
		}
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0002A2B8 File Offset: 0x000284B8
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

	// Token: 0x06000641 RID: 1601 RVA: 0x0002A63C File Offset: 0x0002883C
	public void RequestPicnicGetUserData()
	{
		this.parentData.ServerRequest(PicnicGetUserDataCmd.Create(0), new Action<Command>(this.CbPicnicGetUserDataCmd));
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0002A65B File Offset: 0x0002885B
	public void RequestPicnicSetCharaList(List<int> charaIdList)
	{
		this.parentData.ServerRequest(PicnicSetCharaListCmd.Create(charaIdList), new Action<Command>(this.CbPicnicPicnicSetCharaListCmd));
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0002A67A File Offset: 0x0002887A
	public void RequestPicnicSetPlayList(List<int> playIdList)
	{
		this.parentData.ServerRequest(PicnicSetPlayListCmd.Create(playIdList), new Action<Command>(this.CbPicnicPicnicSetPlayListCmd));
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x0002A69C File Offset: 0x0002889C
	public void RequestPicnicStartTime()
	{
		if (this.starttimeLow == 0 && this.starttimeHigh == 0)
		{
			this.StartTimeTick = TimeManager.Now.Ticks;
			this.parentData.ServerRequest(NewFlgUpdateCmd.Create(this.CreateServerData()), new Action<Command>(this.CbNewFlgUpdateCmd));
		}
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x0002A6EE File Offset: 0x000288EE
	public void RequestPicnicStartTime(DateTime dt)
	{
		this.StartTimeTick = dt.Ticks;
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(this.CreateServerData()), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x0002A71F File Offset: 0x0002891F
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

	// Token: 0x06000647 RID: 1607 RVA: 0x0002A74B File Offset: 0x0002894B
	public void RequestGetMenuBadgeData()
	{
		this.parentData.ServerRequest(PicnicGetUserDataCmd.Create(1), new Action<Command>(this.CbGetMenuBadgeData));
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x0002A76C File Offset: 0x0002896C
	private void CbPicnicGetUserDataCmd(Command cmd)
	{
		PicnicGetUserDataResponse picnicGetUserDataResponse = cmd.response as PicnicGetUserDataResponse;
		this.UpdatePicnicDataByServer(picnicGetUserDataResponse.picnicdata);
		this.parentData.UpdateUserAssetByAssets(picnicGetUserDataResponse.picnicdata.assets);
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0002A7A8 File Offset: 0x000289A8
	private void CbPicnicPicnicSetCharaListCmd(Command cmd)
	{
		PicnicSetCharaListResponse picnicSetCharaListResponse = cmd.response as PicnicSetCharaListResponse;
		this.UpdatePicnicDataByServer(picnicSetCharaListResponse.picnicdata);
		this.parentData.UpdateUserAssetByAssets(picnicSetCharaListResponse.picnicdata.assets);
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x0002A7E4 File Offset: 0x000289E4
	private void CbPicnicPicnicSetPlayListCmd(Command cmd)
	{
		PicnicSetPlayListResponse picnicSetPlayListResponse = cmd.response as PicnicSetPlayListResponse;
		this.UpdatePicnicDataByServer(picnicSetPlayListResponse.picnicdata);
		this.parentData.UpdateUserAssetByAssets(picnicSetPlayListResponse.picnicdata.assets);
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x0002A820 File Offset: 0x00028A20
	public void CbNewFlgUpdateCmd(Command cmd)
	{
		NewFlgUpdateRequest newFlgUpdateRequest = cmd.request as NewFlgUpdateRequest;
		this.UpdateUserFlagByServer(newFlgUpdateRequest.new_flg_list);
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x0002A848 File Offset: 0x00028A48
	public void CbPicnicUseFoodCmd(Command cmd)
	{
		PicnicUseFoodResponse picnicUseFoodResponse = cmd.response as PicnicUseFoodResponse;
		this.UpdatePicnicDataByServer(picnicUseFoodResponse.picnicdata);
		this.parentData.UpdateUserAssetByAssets(picnicUseFoodResponse.picnicdata.assets);
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0002A884 File Offset: 0x00028A84
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

	// Token: 0x0600064E RID: 1614 RVA: 0x0002A90C File Offset: 0x00028B0C
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

	// Token: 0x0600064F RID: 1615 RVA: 0x0002A988 File Offset: 0x00028B88
	public void InitializeMstData(MstManager mstManager)
	{
		List<MstPicnicFoodData> mst = mstManager.GetMst<List<MstPicnicFoodData>>(MstType.PICNIC_FOOD_DATA);
		List<MstPicnicGettimeData> mst2 = mstManager.GetMst<List<MstPicnicGettimeData>>(MstType.PICNIC_GETTIME_DATA);
		List<MstPicnicPlayTypeData> mst3 = mstManager.GetMst<List<MstPicnicPlayTypeData>>(MstType.PICNIC_PLAY_TYPE_DATA);
		List<MstPicnicPlayItemData> mst4 = mstManager.GetMst<List<MstPicnicPlayItemData>>(MstType.PICNIC_PLAY_ITEM_DATA);
		this.PicnicStaticData = new DataManagerPicnic.PicnicStatic(mst, mst2, mst3, mst4);
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x0002A9C8 File Offset: 0x00028BC8
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

	// Token: 0x040005E8 RID: 1512
	private DataManager parentData;

	// Token: 0x040005E9 RID: 1513
	public readonly int active_time_max = 28800;

	// Token: 0x040005EA RID: 1514
	public readonly int energy_max = 28800;

	// Token: 0x040005EB RID: 1515
	public readonly int food_shop_id = 2000;

	// Token: 0x040005F4 RID: 1524
	private int starttimeHigh;

	// Token: 0x040005F5 RID: 1525
	private int starttimeLow;

	// Token: 0x02000716 RID: 1814
	public class PicnicStatic
	{
		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060034C1 RID: 13505 RVA: 0x001C3288 File Offset: 0x001C1488
		// (set) Token: 0x060034C2 RID: 13506 RVA: 0x001C3290 File Offset: 0x001C1490
		public int ActiveTimeMax { get; private set; }

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x060034C3 RID: 13507 RVA: 0x001C3299 File Offset: 0x001C1499
		// (set) Token: 0x060034C4 RID: 13508 RVA: 0x001C32A1 File Offset: 0x001C14A1
		public int EnergyMax { get; private set; }

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060034C5 RID: 13509 RVA: 0x001C32AA File Offset: 0x001C14AA
		// (set) Token: 0x060034C6 RID: 13510 RVA: 0x001C32B2 File Offset: 0x001C14B2
		public List<DataManagerPicnic.FoodData> FoodDataList { get; private set; }

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060034C7 RID: 13511 RVA: 0x001C32BB File Offset: 0x001C14BB
		// (set) Token: 0x060034C8 RID: 13512 RVA: 0x001C32C3 File Offset: 0x001C14C3
		public List<DataManagerPicnic.GettimeData> GettimeDataList { get; private set; }

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060034C9 RID: 13513 RVA: 0x001C32CC File Offset: 0x001C14CC
		// (set) Token: 0x060034CA RID: 13514 RVA: 0x001C32D4 File Offset: 0x001C14D4
		public List<DataManagerPicnic.PlayTypeData> PlayTypeDataList { get; private set; }

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060034CB RID: 13515 RVA: 0x001C32DD File Offset: 0x001C14DD
		// (set) Token: 0x060034CC RID: 13516 RVA: 0x001C32E5 File Offset: 0x001C14E5
		public List<DataManagerPicnic.PlayItemData> PlayItemDataList { get; private set; }

		// Token: 0x060034CD RID: 13517 RVA: 0x001C32F0 File Offset: 0x001C14F0
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

	// Token: 0x02000717 RID: 1815
	public class FoodData
	{
		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060034CE RID: 13518 RVA: 0x001C3474 File Offset: 0x001C1674
		// (set) Token: 0x060034CF RID: 13519 RVA: 0x001C347C File Offset: 0x001C167C
		public int Id { get; private set; }

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060034D0 RID: 13520 RVA: 0x001C3485 File Offset: 0x001C1685
		// (set) Token: 0x060034D1 RID: 13521 RVA: 0x001C348D File Offset: 0x001C168D
		public int AddEnergyNum { get; private set; }

		// Token: 0x060034D2 RID: 13522 RVA: 0x001C3496 File Offset: 0x001C1696
		public FoodData(MstPicnicFoodData food)
		{
			this.Id = food.goodsId;
			this.AddEnergyNum = food.addEnergyNum;
		}
	}

	// Token: 0x02000718 RID: 1816
	public class GettimeData
	{
		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060034D3 RID: 13523 RVA: 0x001C34B6 File Offset: 0x001C16B6
		// (set) Token: 0x060034D4 RID: 13524 RVA: 0x001C34BE File Offset: 0x001C16BE
		public int GettimeId { get; private set; }

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060034D5 RID: 13525 RVA: 0x001C34C7 File Offset: 0x001C16C7
		// (set) Token: 0x060034D6 RID: 13526 RVA: 0x001C34CF File Offset: 0x001C16CF
		public int getItemTime { get; private set; }

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060034D7 RID: 13527 RVA: 0x001C34D8 File Offset: 0x001C16D8
		// (set) Token: 0x060034D8 RID: 13528 RVA: 0x001C34E0 File Offset: 0x001C16E0
		public int getItemRankMaxTime { get; private set; }

		// Token: 0x060034D9 RID: 13529 RVA: 0x001C34E9 File Offset: 0x001C16E9
		public GettimeData(MstPicnicGettimeData gettime)
		{
			this.GettimeId = gettime.picnicGettimeId;
			this.getItemTime = gettime.getItemTime;
			this.getItemRankMaxTime = gettime.getItemRankmaxTime;
		}
	}

	// Token: 0x02000719 RID: 1817
	public class PlayTypeData
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060034DA RID: 13530 RVA: 0x001C3515 File Offset: 0x001C1715
		// (set) Token: 0x060034DB RID: 13531 RVA: 0x001C351D File Offset: 0x001C171D
		public int PlayId { get; private set; }

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060034DC RID: 13532 RVA: 0x001C3526 File Offset: 0x001C1726
		// (set) Token: 0x060034DD RID: 13533 RVA: 0x001C352E File Offset: 0x001C172E
		public int GetTime { get; private set; }

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060034DE RID: 13534 RVA: 0x001C3537 File Offset: 0x001C1737
		// (set) Token: 0x060034DF RID: 13535 RVA: 0x001C353F File Offset: 0x001C173F
		public List<DataManagerPicnic.PlayTypeData.GetItem> GetItemList { get; private set; }

		// Token: 0x060034E0 RID: 13536 RVA: 0x001C3548 File Offset: 0x001C1748
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

		// Token: 0x0200113D RID: 4413
		public class GetItem
		{
			// Token: 0x17000C8B RID: 3211
			// (get) Token: 0x06005577 RID: 21879 RVA: 0x0024F1B1 File Offset: 0x0024D3B1
			// (set) Token: 0x06005578 RID: 21880 RVA: 0x0024F1B9 File Offset: 0x0024D3B9
			public int Id { get; private set; }

			// Token: 0x17000C8C RID: 3212
			// (get) Token: 0x06005579 RID: 21881 RVA: 0x0024F1C2 File Offset: 0x0024D3C2
			// (set) Token: 0x0600557A RID: 21882 RVA: 0x0024F1CA File Offset: 0x0024D3CA
			public ItemInput Item { get; private set; }

			// Token: 0x0600557B RID: 21883 RVA: 0x0024F1D3 File Offset: 0x0024D3D3
			public GetItem(int id, int itemId, int itemNum)
			{
				this.Id = id;
				this.Item = new ItemInput(itemId, itemNum);
			}
		}
	}

	// Token: 0x0200071A RID: 1818
	public class PlayItemData : ItemStaticBase
	{
		// Token: 0x060034E1 RID: 13537 RVA: 0x001C360F File Offset: 0x001C180F
		public override int GetId()
		{
			return this.ItemId;
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x001C3617 File Offset: 0x001C1817
		public override ItemDef.Rarity GetRarity()
		{
			return (ItemDef.Rarity)this.Rarity;
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x001C361F File Offset: 0x001C181F
		public override ItemDef.Kind GetKind()
		{
			return ItemDef.Kind.PICNIC_PLAYITEM;
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x001C3623 File Offset: 0x001C1823
		public override string GetName()
		{
			return this.Name;
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x001C362B File Offset: 0x001C182B
		public override string GetInfo()
		{
			return this.FlavorText;
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x001C3633 File Offset: 0x001C1833
		public override int GetStackMax()
		{
			return this.StackMax;
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x001C363B File Offset: 0x001C183B
		public int GetCategory()
		{
			return this.Category;
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x001C3643 File Offset: 0x001C1843
		public override string GetIconName()
		{
			return "Texture2D/Icon_Item/" + this.IconName;
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x001C3655 File Offset: 0x001C1855
		public override int GetSalePrice()
		{
			return 0;
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060034EA RID: 13546 RVA: 0x001C3658 File Offset: 0x001C1858
		// (set) Token: 0x060034EB RID: 13547 RVA: 0x001C3660 File Offset: 0x001C1860
		private int ItemId { get; set; }

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060034EC RID: 13548 RVA: 0x001C3669 File Offset: 0x001C1869
		// (set) Token: 0x060034ED RID: 13549 RVA: 0x001C3671 File Offset: 0x001C1871
		private int Rarity { get; set; }

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060034EE RID: 13550 RVA: 0x001C367A File Offset: 0x001C187A
		// (set) Token: 0x060034EF RID: 13551 RVA: 0x001C3682 File Offset: 0x001C1882
		private string Name { get; set; }

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060034F0 RID: 13552 RVA: 0x001C368B File Offset: 0x001C188B
		// (set) Token: 0x060034F1 RID: 13553 RVA: 0x001C3693 File Offset: 0x001C1893
		private string FlavorText { get; set; }

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060034F2 RID: 13554 RVA: 0x001C369C File Offset: 0x001C189C
		// (set) Token: 0x060034F3 RID: 13555 RVA: 0x001C36A4 File Offset: 0x001C18A4
		private int StackMax { get; set; }

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060034F4 RID: 13556 RVA: 0x001C36AD File Offset: 0x001C18AD
		// (set) Token: 0x060034F5 RID: 13557 RVA: 0x001C36B5 File Offset: 0x001C18B5
		private int Category { get; set; }

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060034F6 RID: 13558 RVA: 0x001C36BE File Offset: 0x001C18BE
		// (set) Token: 0x060034F7 RID: 13559 RVA: 0x001C36C6 File Offset: 0x001C18C6
		private string IconName { get; set; }

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x060034F8 RID: 13560 RVA: 0x001C36CF File Offset: 0x001C18CF
		// (set) Token: 0x060034F9 RID: 13561 RVA: 0x001C36D7 File Offset: 0x001C18D7
		public int CharaReactionId { get; private set; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060034FA RID: 13562 RVA: 0x001C36E0 File Offset: 0x001C18E0
		// (set) Token: 0x060034FB RID: 13563 RVA: 0x001C36E8 File Offset: 0x001C18E8
		public int AttributeId { get; private set; }

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060034FC RID: 13564 RVA: 0x001C36F1 File Offset: 0x001C18F1
		// (set) Token: 0x060034FD RID: 13565 RVA: 0x001C36F9 File Offset: 0x001C18F9
		public int ReplaceItemNum { get; private set; }

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060034FE RID: 13566 RVA: 0x001C3702 File Offset: 0x001C1902
		// (set) Token: 0x060034FF RID: 13567 RVA: 0x001C370A File Offset: 0x001C190A
		public string StagePackName { get; private set; }

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06003500 RID: 13568 RVA: 0x001C3713 File Offset: 0x001C1913
		// (set) Token: 0x06003501 RID: 13569 RVA: 0x001C371B File Offset: 0x001C191B
		public List<string> FireworksEffectNameList { get; private set; }

		// Token: 0x06003502 RID: 13570 RVA: 0x001C3724 File Offset: 0x001C1924
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

	// Token: 0x0200071B RID: 1819
	public class PicnicDynamic
	{
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06003503 RID: 13571 RVA: 0x001C3821 File Offset: 0x001C1A21
		// (set) Token: 0x06003504 RID: 13572 RVA: 0x001C3829 File Offset: 0x001C1A29
		public List<DataManagerPicnic.CharaData> CharaDataList { get; private set; }

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06003505 RID: 13573 RVA: 0x001C3832 File Offset: 0x001C1A32
		// (set) Token: 0x06003506 RID: 13574 RVA: 0x001C383A File Offset: 0x001C1A3A
		public List<DataManagerPicnic.PlayData> PlayDataList { get; private set; }

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06003507 RID: 13575 RVA: 0x001C3843 File Offset: 0x001C1A43
		// (set) Token: 0x06003508 RID: 13576 RVA: 0x001C384B File Offset: 0x001C1A4B
		public int Energy { get; private set; }

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06003509 RID: 13577 RVA: 0x001C3854 File Offset: 0x001C1A54
		// (set) Token: 0x0600350A RID: 13578 RVA: 0x001C385C File Offset: 0x001C1A5C
		public DateTime LastUpdateTime { get; private set; }

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x0600350B RID: 13579 RVA: 0x001C3865 File Offset: 0x001C1A65
		// (set) Token: 0x0600350C RID: 13580 RVA: 0x001C386D File Offset: 0x001C1A6D
		public List<DataManagerPicnic.DropItemData> UpdateItemList { get; private set; }

		// Token: 0x0600350D RID: 13581 RVA: 0x001C3876 File Offset: 0x001C1A76
		public PicnicDynamic(int energy, List<DataManagerPicnic.CharaData> charaDataList, List<DataManagerPicnic.PlayData> playDataList, DateTime lastUpdate, List<DataManagerPicnic.DropItemData> updateItemlist)
		{
			this.Energy = energy;
			this.CharaDataList = charaDataList;
			this.PlayDataList = playDataList;
			this.LastUpdateTime = lastUpdate;
			this.UpdateItemList = updateItemlist;
		}
	}

	// Token: 0x0200071C RID: 1820
	public class CharaData
	{
		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x0600350E RID: 13582 RVA: 0x001C38A3 File Offset: 0x001C1AA3
		// (set) Token: 0x0600350F RID: 13583 RVA: 0x001C38AB File Offset: 0x001C1AAB
		public int CharaId { get; private set; }

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06003510 RID: 13584 RVA: 0x001C38B4 File Offset: 0x001C1AB4
		// (set) Token: 0x06003511 RID: 13585 RVA: 0x001C38BC File Offset: 0x001C1ABC
		public long RestTime { get; set; }

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06003512 RID: 13586 RVA: 0x001C38C5 File Offset: 0x001C1AC5
		// (set) Token: 0x06003513 RID: 13587 RVA: 0x001C38CD File Offset: 0x001C1ACD
		public bool IsRankMax { get; private set; }

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06003514 RID: 13588 RVA: 0x001C38D6 File Offset: 0x001C1AD6
		public int GettimeId
		{
			get
			{
				return DataManager.DmChara.GetCharaStaticData(this.CharaId).baseData.picnicGettimeId;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06003515 RID: 13589 RVA: 0x001C38F2 File Offset: 0x001C1AF2
		// (set) Token: 0x06003516 RID: 13590 RVA: 0x001C38FA File Offset: 0x001C1AFA
		public int MonthlyBuffRatio { get; set; }

		// Token: 0x06003517 RID: 13591 RVA: 0x001C3903 File Offset: 0x001C1B03
		public CharaData(PicnicChara picnicChar)
		{
			this.CharaId = picnicChar.chara_id;
			this.RestTime = picnicChar.resttime;
			this.IsRankMax = picnicChar.rankmax != 0;
		}
	}

	// Token: 0x0200071D RID: 1821
	public class PlayData
	{
		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06003518 RID: 13592 RVA: 0x001C3935 File Offset: 0x001C1B35
		// (set) Token: 0x06003519 RID: 13593 RVA: 0x001C393D File Offset: 0x001C1B3D
		public int PlayId { get; private set; }

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x0600351A RID: 13594 RVA: 0x001C3946 File Offset: 0x001C1B46
		// (set) Token: 0x0600351B RID: 13595 RVA: 0x001C394E File Offset: 0x001C1B4E
		public long RestTime { get; set; }

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x0600351C RID: 13596 RVA: 0x001C3957 File Offset: 0x001C1B57
		// (set) Token: 0x0600351D RID: 13597 RVA: 0x001C395F File Offset: 0x001C1B5F
		public int MonthlyBuffRatio { get; set; }

		// Token: 0x0600351E RID: 13598 RVA: 0x001C3968 File Offset: 0x001C1B68
		public PlayData(PicnicPlay picnicPlay)
		{
			this.PlayId = picnicPlay.play_id;
			this.RestTime = picnicPlay.resttime;
		}
	}

	// Token: 0x0200071E RID: 1822
	public class DropItemData
	{
		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x0600351F RID: 13599 RVA: 0x001C3988 File Offset: 0x001C1B88
		// (set) Token: 0x06003520 RID: 13600 RVA: 0x001C3990 File Offset: 0x001C1B90
		public int CharaId { get; private set; }

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06003521 RID: 13601 RVA: 0x001C3999 File Offset: 0x001C1B99
		// (set) Token: 0x06003522 RID: 13602 RVA: 0x001C39A1 File Offset: 0x001C1BA1
		public int ItemId { get; private set; }

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06003523 RID: 13603 RVA: 0x001C39AA File Offset: 0x001C1BAA
		// (set) Token: 0x06003524 RID: 13604 RVA: 0x001C39B2 File Offset: 0x001C1BB2
		public int ItemNum { get; set; }

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06003525 RID: 13605 RVA: 0x001C39BB File Offset: 0x001C1BBB
		// (set) Token: 0x06003526 RID: 13606 RVA: 0x001C39C3 File Offset: 0x001C1BC3
		public int MonthlyBonusNum { get; set; }

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06003527 RID: 13607 RVA: 0x001C39CC File Offset: 0x001C1BCC
		// (set) Token: 0x06003528 RID: 13608 RVA: 0x001C39D4 File Offset: 0x001C1BD4
		public bool isCampaign { get; set; }

		// Token: 0x06003529 RID: 13609 RVA: 0x001C39DD File Offset: 0x001C1BDD
		public DropItemData(int chid, int itemid, int itemnum)
		{
			this.CharaId = chid;
			this.ItemId = itemid;
			this.ItemNum = itemnum;
		}
	}

	// Token: 0x0200071F RID: 1823
	public class MenuBadge
	{
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x0600352A RID: 13610 RVA: 0x001C39FA File Offset: 0x001C1BFA
		// (set) Token: 0x0600352B RID: 13611 RVA: 0x001C3A02 File Offset: 0x001C1C02
		public int Energy { get; set; }

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x0600352C RID: 13612 RVA: 0x001C3A0B File Offset: 0x001C1C0B
		// (set) Token: 0x0600352D RID: 13613 RVA: 0x001C3A13 File Offset: 0x001C1C13
		public DateTime LastUpdateTime { get; set; }

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x0600352E RID: 13614 RVA: 0x001C3A1C File Offset: 0x001C1C1C
		// (set) Token: 0x0600352F RID: 13615 RVA: 0x001C3A24 File Offset: 0x001C1C24
		public bool IsCharaSet { get; set; }
	}
}
