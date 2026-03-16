using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerMonthlyPack
{
	public DataManagerMonthlyPack(DataManager p)
	{
		this.parentData = p;
	}

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

	public void UpdateUserDataByServer(PlayerInfo playerInfo)
	{
		this.nowPackData = new DataManagerMonthlyPack.UserPackData(playerInfo.monthlypack_id, playerInfo.monthlypack_endtime);
		this.nextPackData = new DataManagerMonthlyPack.UserPackData(playerInfo.monthlypack_id_next, playerInfo.monthlypack_endtime_next);
	}

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

	private DataManager parentData;

	public DataManagerMonthlyPack.UserPackData nowPackData;

	public DataManagerMonthlyPack.UserPackData nextPackData;

	public List<DataManagerMonthlyPack.PurchaseMonthlypackData> purchaseMonthlypackDataList;

	public List<DataManagerMonthlyPack.PurchaseMonthlypackMessageData> purchaseMonthlypackMessageDataList;

	public List<DataManagerMonthlyPack.MonthlypackContinueData> monthlypackContinueDataList;

	public class UserPackData
	{
		public int PackId { get; private set; }

		public DateTime EndDatetime { get; private set; }

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

		public UserPackData(int id, long endtime)
		{
			this.PackId = id;
			this.EndDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(endtime));
		}
	}

	public class PurchaseMonthlypackData
	{
		public int PackId { get; private set; }

		public string PackName { get; private set; }

		public ItemInput PrevBuyGorgeous { get; private set; }

		public ItemInput PrevBuyGreat { get; private set; }

		public ItemInput PrevBuyAlittle { get; private set; }

		public ItemInput PrevBuyFirst { get; private set; }

		public int PicnicBuffFrameCount { get; private set; }

		public int PicnicBuffAddRatio { get; private set; }

		public string WebviewLink { get; private set; }

		public string BuffText { get; private set; }

		public bool SkippableFlag { get; private set; }

		public bool BattleRetryFlag { get; private set; }

		public bool PracticeFlag { get; private set; }

		public int TreeHouseAddMysetNum { get; private set; }

		public int PackType { get; private set; }

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

	public class PurchaseMonthlypackMessageData
	{
		public int Id { get; private set; }

		public string BeforePeriodText { get; private set; }

		public int ReminderDay { get; private set; }

		public string AfterPeriodText { get; private set; }

		public int ContinueLimitDay { get; private set; }

		public PurchaseMonthlypackMessageData(MstMonthlypackMessageData mstMonthMsgData)
		{
			this.Id = mstMonthMsgData.id;
			this.BeforePeriodText = mstMonthMsgData.beforePeriodText;
			this.ReminderDay = mstMonthMsgData.beforeCanbuyDay;
			this.AfterPeriodText = mstMonthMsgData.afterPeriodText;
			this.ContinueLimitDay = mstMonthMsgData.continueReckonDay;
		}
	}

	public class MonthlypackContinueData
	{
		public int ContinueId { get; private set; }

		public int PrevMonthlyPackId { get; private set; }

		public int NextMonthlyPackId { get; private set; }

		public int AddItemId { get; private set; }

		public int AddItemNum { get; private set; }

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
