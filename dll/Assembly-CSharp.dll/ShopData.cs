using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class ShopData
{
	public ShopData.Sort SortType { get; private set; }

	public bool IsInfinitie { get; private set; }

	public int priceItemId
	{
		get
		{
			if (this.oneDataList.Count <= 0)
			{
				return 0;
			}
			return this.oneDataList[0].priceItemId;
		}
	}

	public ShopData(MstShopData mst)
	{
		this.shopId = mst.shopId;
		this.shopName = mst.shopName;
		this.category = (ShopData.Category)mst.category;
		this.SortType = (ShopData.Sort)mst.sortType;
		this.bannerImageName = mst.bannerImagePath;
		this.priority = mst.priority;
		this.tabCategory = mst.tabCategory;
		if (this.tabCategory > 0)
		{
			int num = this.tabCategory;
		}
		this.tabName = mst.tabName;
		this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
		this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.endTime));
		if (this.endTime.Year > TimeManager.Now.Year + 10)
		{
			this.IsInfinitie = true;
		}
	}

	public int shopId;

	public string shopName;

	public ShopData.Category category;

	public int shopCharaId;

	public string bannerImageName;

	public int priority;

	public List<ShopData.ItemOne> oneDataList = new List<ShopData.ItemOne>();

	public int tabCategory;

	public string tabName;

	public DateTime startTime;

	public DateTime endTime;

	public List<ShopData.ItemOne> oneDataInternalList = new List<ShopData.ItemOne>();

	public enum Category
	{
		INVALID,
		NORMAL,
		PVP,
		CHARGES,
		EVENT,
		PURCHASE,
		MONTHLYPACK,
		PYROXENE,
		OTHER = 9,
		PICNIC = 2000,
		OTHER_NOITEM_HIDE = 109,
		EVENT_NOITEM_HIDE = 104,
		TRAINING = 2100
	}

	public enum Sort
	{
		Invalid,
		Name
	}

	public enum TabCategory
	{
		ALL,
		EVENT,
		NORMAL,
		TICKET,
		MAX
	}

	public class ItemOne
	{
		public bool IsInfinitie { get; private set; }

		public bool isSoldout
		{
			get
			{
				return this.maxChangeNum != 0 && this.maxChangeNum <= this.nowChangeNum;
			}
		}

		public ItemOne()
		{
		}

		public ItemOne(MstShopItemData mst, ShopItemInfo server)
		{
			this.mstData = mst;
			this.serverData = server;
			this.goodsId = mst.goodsId;
			this.itemId = mst.itemId;
			this.charaStatusId = mst.charaStatusId;
			this.itemNum = mst.itemNum;
			this.priceItemId = mst.priceItemId;
			this.priceItemNum = mst.priceItemNum;
			this.maxChangeNum = mst.maxChangeNum;
			this.dispType = (ShopData.ItemOne.DispType)mst.dispType;
			this.disptypeTargetitemId = mst.disptypeTargetitemId;
			this.disptypeTargetitemId02 = mst.disptypeTargetitemId2;
			this.infoWindow = false;
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
			this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.endTime));
			this.priority = mst.priority;
			if (this.endTime.Year > TimeManager.Now.Year + 10)
			{
				this.IsInfinitie = true;
			}
			this.nowChangeNum = server.changeNum;
			this.openQuestOneId = mst.openQuestOneId;
			this.openQuestOneIdText = mst.openQuestOneText;
			this.openMissionId = mst.openMissionId;
			this.notOpenDispFlag = mst.notOpenDispFlg != 0;
			this.itemName = "";
			DateTime now = TimeManager.Now;
			if (mst.resetType == 2)
			{
				this.resetTime = new DateTime?(TimeManager.GetTerminalTimeByDay(now));
				this.resetInfoPrefix = "本日";
				return;
			}
			if (mst.resetType == 3)
			{
				this.resetTime = new DateTime?(TimeManager.GetTerminalTimeByWeek(now));
				this.resetInfoPrefix = "今週";
				return;
			}
			if (mst.resetType == 4)
			{
				this.resetTime = new DateTime?(TimeManager.GetTerminalTimeByMonth(now));
				this.resetInfoPrefix = "今月";
			}
		}

		public ShopData.ItemOne Clone()
		{
			return new ShopData.ItemOne(this.mstData, this.serverData);
		}

		public void UpdateNowChangeNum(int num)
		{
			this.nowChangeNum = num;
			if (this.serverData != null)
			{
				this.serverData.changeNum = num;
			}
		}

		public void SetShopItemName(string name)
		{
			this.itemName = name;
		}

		public int goodsId;

		public int itemId;

		public int charaStatusId;

		public int itemNum;

		public int priceItemId;

		public int priceItemNum;

		public int maxChangeNum;

		public int nowChangeNum;

		public ShopData.ItemOne.DispType dispType;

		public int disptypeTargetitemId;

		public int disptypeTargetitemId02;

		public bool infoWindow;

		public DateTime startTime;

		public DateTime endTime;

		public DateTime? resetTime;

		public string resetInfoPrefix = "";

		public int priority;

		public List<ShopData.ItemOne.SetItem> setItems;

		public int openQuestOneId;

		public string openQuestOneIdText;

		public int openMissionId;

		public bool notOpenDispFlag;

		public bool isLockByQuest;

		public bool isLockByMission;

		public bool isLockByTime;

		public string itemName;

		private MstShopItemData mstData;

		private ShopItemInfo serverData;

		public enum DispType
		{
			Always,
			Show,
			Hide,
			SpecialOR,
			SpecialAND
		}

		public class SetItem
		{
			public SetItem(int id, int num)
			{
				this.itemId = id;
				this.itemNum = num;
			}

			public int itemId;

			public int itemNum;
		}

		public class CharaStatusData
		{
			public CharaStatusData(MstShopCharaStatusData mst)
			{
				this.level = mst.level;
				this.rank = mst.rank;
				this.kizunaLevel = mst.kizunaLevel;
				this.promoteNum = mst.promoteNum;
				this.artsLevel = mst.artsLevel;
				this.photoFrameLevel = mst.photoFrameLevel;
			}

			public int level;

			public int rank;

			public int kizunaLevel;

			public int promoteNum;

			public int artsLevel;

			public int photoFrameLevel;
		}
	}
}
