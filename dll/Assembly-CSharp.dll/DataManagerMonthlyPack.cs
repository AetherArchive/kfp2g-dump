using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x02000096 RID: 150
public class DataManagerMonthlyPack
{
	// Token: 0x060005F3 RID: 1523 RVA: 0x0002870B File Offset: 0x0002690B
	public DataManagerMonthlyPack(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0002871C File Offset: 0x0002691C
	public bool IsEnableMonthlyPack(DateTime now)
	{
		bool flag = true;
		DateTime dateTime = new DateTime(this.nowPackData.EndDatetime.Year, this.nowPackData.EndDatetime.Month, this.nowPackData.EndDatetime.Day);
		DateTime dateTime2 = new DateTime(this.nextPackData.EndDatetime.Year, this.nextPackData.EndDatetime.Month, this.nextPackData.EndDatetime.Day);
		DateTime dateTime3 = new DateTime(now.Year, now.Month, now.Day);
		if ((this.nowPackData.MonthlypackData == null || dateTime < dateTime3) && (this.nextPackData.MonthlypackData == null || dateTime2 < dateTime3))
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x000287FC File Offset: 0x000269FC
	public void InitializeMstData(MstManager mstManager)
	{
		List<MstPurchaseMonthlypackData> mst = mstManager.GetMst<List<MstPurchaseMonthlypackData>>(MstType.PURCHASE_MONTHLYPACK_DATA);
		List<MstMonthlypackMessageData> mst2 = mstManager.GetMst<List<MstMonthlypackMessageData>>(MstType.MONTHLYPACK_MESSAGE_DATA);
		List<MstMonthlypackContinueData> mst3 = mstManager.GetMst<List<MstMonthlypackContinueData>>(MstType.MONTHLYPACK_CONTINUE_DATA);
		this.purchaseMonthlypackDataList = new List<DataManagerMonthlyPack.PurchaseMonthlypackData>();
		foreach (MstPurchaseMonthlypackData mstPurchaseMonthlypackData in mst)
		{
			DataManagerMonthlyPack.PurchaseMonthlypackData purchaseMonthlypackData = new DataManagerMonthlyPack.PurchaseMonthlypackData(mstPurchaseMonthlypackData);
			this.purchaseMonthlypackDataList.Add(purchaseMonthlypackData);
		}
		this.purchaseMonthlypackMessageDataList = new List<DataManagerMonthlyPack.PurchaseMonthlypackMessageData>();
		foreach (MstMonthlypackMessageData mstMonthlypackMessageData in mst2)
		{
			DataManagerMonthlyPack.PurchaseMonthlypackMessageData purchaseMonthlypackMessageData = new DataManagerMonthlyPack.PurchaseMonthlypackMessageData(mstMonthlypackMessageData);
			this.purchaseMonthlypackMessageDataList.Add(purchaseMonthlypackMessageData);
		}
		this.monthlypackContinueDataList = new List<DataManagerMonthlyPack.MonthlypackContinueData>();
		foreach (MstMonthlypackContinueData mstMonthlypackContinueData in mst3)
		{
			DataManagerMonthlyPack.MonthlypackContinueData monthlypackContinueData = new DataManagerMonthlyPack.MonthlypackContinueData(mstMonthlypackContinueData);
			this.monthlypackContinueDataList.Add(monthlypackContinueData);
		}
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00028924 File Offset: 0x00026B24
	public void UpdateUserDataByServer(PlayerInfo playerInfo)
	{
		this.nowPackData = new DataManagerMonthlyPack.UserPackData(playerInfo.monthlypack_id, playerInfo.monthlypack_endtime);
		this.nextPackData = new DataManagerMonthlyPack.UserPackData(playerInfo.monthlypack_id_next, playerInfo.monthlypack_endtime_next);
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x00028954 File Offset: 0x00026B54
	public DataManagerMonthlyPack.PurchaseMonthlypackData GetValidMonthlyPackData()
	{
		DateTime dateTime = new DateTime(TimeManager.Now.Year, TimeManager.Now.Month, TimeManager.Now.Day);
		DataManagerMonthlyPack.PurchaseMonthlypackData purchaseMonthlypackData = null;
		if (DataManager.DmMonthlyPack.nowPackData.EndDatetime >= dateTime)
		{
			purchaseMonthlypackData = DataManager.DmMonthlyPack.nowPackData.MonthlypackData;
		}
		else if (DataManager.DmMonthlyPack.nextPackData.EndDatetime >= dateTime)
		{
			purchaseMonthlypackData = DataManager.DmMonthlyPack.nextPackData.MonthlypackData;
		}
		return purchaseMonthlypackData;
	}

	// Token: 0x040005D5 RID: 1493
	private DataManager parentData;

	// Token: 0x040005D6 RID: 1494
	public DataManagerMonthlyPack.UserPackData nowPackData;

	// Token: 0x040005D7 RID: 1495
	public DataManagerMonthlyPack.UserPackData nextPackData;

	// Token: 0x040005D8 RID: 1496
	public List<DataManagerMonthlyPack.PurchaseMonthlypackData> purchaseMonthlypackDataList;

	// Token: 0x040005D9 RID: 1497
	public List<DataManagerMonthlyPack.PurchaseMonthlypackMessageData> purchaseMonthlypackMessageDataList;

	// Token: 0x040005DA RID: 1498
	public List<DataManagerMonthlyPack.MonthlypackContinueData> monthlypackContinueDataList;

	// Token: 0x02000704 RID: 1796
	public class UserPackData
	{
		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x001C2864 File Offset: 0x001C0A64
		// (set) Token: 0x06003426 RID: 13350 RVA: 0x001C286C File Offset: 0x001C0A6C
		public int PackId { get; private set; }

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06003427 RID: 13351 RVA: 0x001C2875 File Offset: 0x001C0A75
		// (set) Token: 0x06003428 RID: 13352 RVA: 0x001C287D File Offset: 0x001C0A7D
		public DateTime EndDatetime { get; private set; }

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06003429 RID: 13353 RVA: 0x001C2888 File Offset: 0x001C0A88
		public DataManagerMonthlyPack.PurchaseMonthlypackData MonthlypackData
		{
			get
			{
				if (this.PackId == 0)
				{
					return null;
				}
				return DataManager.DmMonthlyPack.purchaseMonthlypackDataList.Find((DataManagerMonthlyPack.PurchaseMonthlypackData x) => x.PackId == this.PackId);
			}
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x001C28BE File Offset: 0x001C0ABE
		public UserPackData(int id, long endtime)
		{
			this.PackId = id;
			this.EndDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(endtime));
		}
	}

	// Token: 0x02000705 RID: 1797
	public class PurchaseMonthlypackData
	{
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x0600342C RID: 13356 RVA: 0x001C28EE File Offset: 0x001C0AEE
		// (set) Token: 0x0600342D RID: 13357 RVA: 0x001C28F6 File Offset: 0x001C0AF6
		public int PackId { get; private set; }

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x0600342E RID: 13358 RVA: 0x001C28FF File Offset: 0x001C0AFF
		// (set) Token: 0x0600342F RID: 13359 RVA: 0x001C2907 File Offset: 0x001C0B07
		public string PackName { get; private set; }

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06003430 RID: 13360 RVA: 0x001C2910 File Offset: 0x001C0B10
		// (set) Token: 0x06003431 RID: 13361 RVA: 0x001C2918 File Offset: 0x001C0B18
		public ItemInput PrevBuyGorgeous { get; private set; }

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06003432 RID: 13362 RVA: 0x001C2921 File Offset: 0x001C0B21
		// (set) Token: 0x06003433 RID: 13363 RVA: 0x001C2929 File Offset: 0x001C0B29
		public ItemInput PrevBuyGreat { get; private set; }

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06003434 RID: 13364 RVA: 0x001C2932 File Offset: 0x001C0B32
		// (set) Token: 0x06003435 RID: 13365 RVA: 0x001C293A File Offset: 0x001C0B3A
		public ItemInput PrevBuyAlittle { get; private set; }

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06003436 RID: 13366 RVA: 0x001C2943 File Offset: 0x001C0B43
		// (set) Token: 0x06003437 RID: 13367 RVA: 0x001C294B File Offset: 0x001C0B4B
		public ItemInput PrevBuyFirst { get; private set; }

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06003438 RID: 13368 RVA: 0x001C2954 File Offset: 0x001C0B54
		// (set) Token: 0x06003439 RID: 13369 RVA: 0x001C295C File Offset: 0x001C0B5C
		public int PicnicBuffFrameCount { get; private set; }

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x0600343A RID: 13370 RVA: 0x001C2965 File Offset: 0x001C0B65
		// (set) Token: 0x0600343B RID: 13371 RVA: 0x001C296D File Offset: 0x001C0B6D
		public int PicnicBuffAddRatio { get; private set; }

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x0600343C RID: 13372 RVA: 0x001C2976 File Offset: 0x001C0B76
		// (set) Token: 0x0600343D RID: 13373 RVA: 0x001C297E File Offset: 0x001C0B7E
		public string WebviewLink { get; private set; }

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x0600343E RID: 13374 RVA: 0x001C2987 File Offset: 0x001C0B87
		// (set) Token: 0x0600343F RID: 13375 RVA: 0x001C298F File Offset: 0x001C0B8F
		public string BuffText { get; private set; }

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06003440 RID: 13376 RVA: 0x001C2998 File Offset: 0x001C0B98
		// (set) Token: 0x06003441 RID: 13377 RVA: 0x001C29A0 File Offset: 0x001C0BA0
		public bool SkippableFlag { get; private set; }

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06003442 RID: 13378 RVA: 0x001C29A9 File Offset: 0x001C0BA9
		// (set) Token: 0x06003443 RID: 13379 RVA: 0x001C29B1 File Offset: 0x001C0BB1
		public bool BattleRetryFlag { get; private set; }

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06003444 RID: 13380 RVA: 0x001C29BA File Offset: 0x001C0BBA
		// (set) Token: 0x06003445 RID: 13381 RVA: 0x001C29C2 File Offset: 0x001C0BC2
		public bool PracticeFlag { get; private set; }

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06003446 RID: 13382 RVA: 0x001C29CB File Offset: 0x001C0BCB
		// (set) Token: 0x06003447 RID: 13383 RVA: 0x001C29D3 File Offset: 0x001C0BD3
		public int TreeHouseAddMysetNum { get; private set; }

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06003448 RID: 13384 RVA: 0x001C29DC File Offset: 0x001C0BDC
		// (set) Token: 0x06003449 RID: 13385 RVA: 0x001C29E4 File Offset: 0x001C0BE4
		public int PackType { get; private set; }

		// Token: 0x0600344A RID: 13386 RVA: 0x001C29F0 File Offset: 0x001C0BF0
		public PurchaseMonthlypackData(MstPurchaseMonthlypackData mstPurchaseMonthlypackData)
		{
			this.PackId = mstPurchaseMonthlypackData.monthlyPackId;
			this.PackName = mstPurchaseMonthlypackData.packName;
			this.PrevBuyGorgeous = new ItemInput
			{
				itemId = mstPurchaseMonthlypackData.prevBuyGorgeousAddItemId,
				num = mstPurchaseMonthlypackData.prevBuyGorgeousAddItemNum
			};
			this.PrevBuyGreat = new ItemInput
			{
				itemId = mstPurchaseMonthlypackData.prevBuyGreatAddItemId,
				num = mstPurchaseMonthlypackData.prevBuyGreatAddItemNum
			};
			this.PrevBuyAlittle = new ItemInput
			{
				itemId = mstPurchaseMonthlypackData.prevBuyAlittleAddItemId,
				num = mstPurchaseMonthlypackData.prevBuyAlittleAddItemNum
			};
			this.PrevBuyFirst = new ItemInput
			{
				itemId = mstPurchaseMonthlypackData.prevBuyFirstAddItemId,
				num = mstPurchaseMonthlypackData.prevBuyFirstAddItemNum
			};
			this.PicnicBuffFrameCount = mstPurchaseMonthlypackData.picnicBuffFramecnt;
			this.PicnicBuffAddRatio = mstPurchaseMonthlypackData.picnicBuffAddratio;
			this.WebviewLink = mstPurchaseMonthlypackData.webviewLink;
			this.BuffText = mstPurchaseMonthlypackData.buffText;
			this.TreeHouseAddMysetNum = mstPurchaseMonthlypackData.masterRoomAddMysetNum;
			this.SkippableFlag = mstPurchaseMonthlypackData.skippableFlag != 0;
			this.BattleRetryFlag = mstPurchaseMonthlypackData.battleRetryFlag != 0;
			this.PracticeFlag = mstPurchaseMonthlypackData.practiceFlag != 0;
			this.PackType = mstPurchaseMonthlypackData.packType;
		}
	}

	// Token: 0x02000706 RID: 1798
	public class PurchaseMonthlypackMessageData
	{
		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x0600344B RID: 13387 RVA: 0x001C2B1C File Offset: 0x001C0D1C
		// (set) Token: 0x0600344C RID: 13388 RVA: 0x001C2B24 File Offset: 0x001C0D24
		public int Id { get; private set; }

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x0600344D RID: 13389 RVA: 0x001C2B2D File Offset: 0x001C0D2D
		// (set) Token: 0x0600344E RID: 13390 RVA: 0x001C2B35 File Offset: 0x001C0D35
		public string BeforePeriodText { get; private set; }

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x0600344F RID: 13391 RVA: 0x001C2B3E File Offset: 0x001C0D3E
		// (set) Token: 0x06003450 RID: 13392 RVA: 0x001C2B46 File Offset: 0x001C0D46
		public int ReminderDay { get; private set; }

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06003451 RID: 13393 RVA: 0x001C2B4F File Offset: 0x001C0D4F
		// (set) Token: 0x06003452 RID: 13394 RVA: 0x001C2B57 File Offset: 0x001C0D57
		public string AfterPeriodText { get; private set; }

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06003453 RID: 13395 RVA: 0x001C2B60 File Offset: 0x001C0D60
		// (set) Token: 0x06003454 RID: 13396 RVA: 0x001C2B68 File Offset: 0x001C0D68
		public int ContinueLimitDay { get; private set; }

		// Token: 0x06003455 RID: 13397 RVA: 0x001C2B74 File Offset: 0x001C0D74
		public PurchaseMonthlypackMessageData(MstMonthlypackMessageData mstMonthMsgData)
		{
			this.Id = mstMonthMsgData.id;
			this.BeforePeriodText = mstMonthMsgData.beforePeriodText;
			this.ReminderDay = mstMonthMsgData.beforeCanbuyDay;
			this.AfterPeriodText = mstMonthMsgData.afterPeriodText;
			this.ContinueLimitDay = mstMonthMsgData.continueReckonDay;
		}
	}

	// Token: 0x02000707 RID: 1799
	public class MonthlypackContinueData
	{
		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06003456 RID: 13398 RVA: 0x001C2BC3 File Offset: 0x001C0DC3
		// (set) Token: 0x06003457 RID: 13399 RVA: 0x001C2BCB File Offset: 0x001C0DCB
		public int ContinueId { get; private set; }

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06003458 RID: 13400 RVA: 0x001C2BD4 File Offset: 0x001C0DD4
		// (set) Token: 0x06003459 RID: 13401 RVA: 0x001C2BDC File Offset: 0x001C0DDC
		public int PrevMonthlyPackId { get; private set; }

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x0600345A RID: 13402 RVA: 0x001C2BE5 File Offset: 0x001C0DE5
		// (set) Token: 0x0600345B RID: 13403 RVA: 0x001C2BED File Offset: 0x001C0DED
		public int NextMonthlyPackId { get; private set; }

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x0600345C RID: 13404 RVA: 0x001C2BF6 File Offset: 0x001C0DF6
		// (set) Token: 0x0600345D RID: 13405 RVA: 0x001C2BFE File Offset: 0x001C0DFE
		public int AddItemId { get; private set; }

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x0600345E RID: 13406 RVA: 0x001C2C07 File Offset: 0x001C0E07
		// (set) Token: 0x0600345F RID: 13407 RVA: 0x001C2C0F File Offset: 0x001C0E0F
		public int AddItemNum { get; private set; }

		// Token: 0x06003460 RID: 13408 RVA: 0x001C2C18 File Offset: 0x001C0E18
		public MonthlypackContinueData(MstMonthlypackContinueData mstMonthlyContinueData)
		{
			this.ContinueId = mstMonthlyContinueData.continueId;
			this.PrevMonthlyPackId = mstMonthlyContinueData.prevMonthlyPackId;
			this.NextMonthlyPackId = mstMonthlyContinueData.nextMonthlyPackId;
			this.AddItemId = mstMonthlyContinueData.addItemId;
			this.AddItemNum = mstMonthlyContinueData.addItemNum;
		}
	}
}
