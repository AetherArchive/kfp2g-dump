using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x020000A7 RID: 167
public class ShopData
{
	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000761 RID: 1889 RVA: 0x00033324 File Offset: 0x00031524
	// (set) Token: 0x06000762 RID: 1890 RVA: 0x0003332C File Offset: 0x0003152C
	public ShopData.Sort SortType { get; private set; }

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000763 RID: 1891 RVA: 0x00033335 File Offset: 0x00031535
	// (set) Token: 0x06000764 RID: 1892 RVA: 0x0003333D File Offset: 0x0003153D
	public bool IsInfinitie { get; private set; }

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000765 RID: 1893 RVA: 0x00033346 File Offset: 0x00031546
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

	// Token: 0x06000766 RID: 1894 RVA: 0x0003336C File Offset: 0x0003156C
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

	// Token: 0x04000672 RID: 1650
	public int shopId;

	// Token: 0x04000673 RID: 1651
	public string shopName;

	// Token: 0x04000674 RID: 1652
	public ShopData.Category category;

	// Token: 0x04000675 RID: 1653
	public int shopCharaId;

	// Token: 0x04000676 RID: 1654
	public string bannerImageName;

	// Token: 0x04000677 RID: 1655
	public int priority;

	// Token: 0x04000678 RID: 1656
	public List<ShopData.ItemOne> oneDataList = new List<ShopData.ItemOne>();

	// Token: 0x0400067A RID: 1658
	public int tabCategory;

	// Token: 0x0400067B RID: 1659
	public string tabName;

	// Token: 0x0400067C RID: 1660
	public DateTime startTime;

	// Token: 0x0400067D RID: 1661
	public DateTime endTime;

	// Token: 0x0400067F RID: 1663
	public List<ShopData.ItemOne> oneDataInternalList = new List<ShopData.ItemOne>();

	// Token: 0x0200078A RID: 1930
	public enum Category
	{
		// Token: 0x04003373 RID: 13171
		INVALID,
		// Token: 0x04003374 RID: 13172
		NORMAL,
		// Token: 0x04003375 RID: 13173
		PVP,
		// Token: 0x04003376 RID: 13174
		CHARGES,
		// Token: 0x04003377 RID: 13175
		EVENT,
		// Token: 0x04003378 RID: 13176
		PURCHASE,
		// Token: 0x04003379 RID: 13177
		MONTHLYPACK,
		// Token: 0x0400337A RID: 13178
		PYROXENE,
		// Token: 0x0400337B RID: 13179
		OTHER = 9,
		// Token: 0x0400337C RID: 13180
		PICNIC = 2000,
		// Token: 0x0400337D RID: 13181
		OTHER_NOITEM_HIDE = 109,
		// Token: 0x0400337E RID: 13182
		EVENT_NOITEM_HIDE = 104,
		// Token: 0x0400337F RID: 13183
		TRAINING = 2100
	}

	// Token: 0x0200078B RID: 1931
	public enum Sort
	{
		// Token: 0x04003381 RID: 13185
		Invalid,
		// Token: 0x04003382 RID: 13186
		Name
	}

	// Token: 0x0200078C RID: 1932
	public enum TabCategory
	{
		// Token: 0x04003384 RID: 13188
		ALL,
		// Token: 0x04003385 RID: 13189
		EVENT,
		// Token: 0x04003386 RID: 13190
		NORMAL,
		// Token: 0x04003387 RID: 13191
		TICKET,
		// Token: 0x04003388 RID: 13192
		MAX
	}

	// Token: 0x0200078D RID: 1933
	public class ItemOne
	{
		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06003694 RID: 13972 RVA: 0x001C676B File Offset: 0x001C496B
		// (set) Token: 0x06003695 RID: 13973 RVA: 0x001C6773 File Offset: 0x001C4973
		public bool IsInfinitie { get; private set; }

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06003696 RID: 13974 RVA: 0x001C677C File Offset: 0x001C497C
		public bool isSoldout
		{
			get
			{
				return this.maxChangeNum != 0 && this.maxChangeNum <= this.nowChangeNum;
			}
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x001C6799 File Offset: 0x001C4999
		public ItemOne()
		{
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x001C67AC File Offset: 0x001C49AC
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

		// Token: 0x06003699 RID: 13977 RVA: 0x001C6974 File Offset: 0x001C4B74
		public ShopData.ItemOne Clone()
		{
			return new ShopData.ItemOne(this.mstData, this.serverData);
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x001C6987 File Offset: 0x001C4B87
		public void UpdateNowChangeNum(int num)
		{
			this.nowChangeNum = num;
			if (this.serverData != null)
			{
				this.serverData.changeNum = num;
			}
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x001C69A4 File Offset: 0x001C4BA4
		public void SetShopItemName(string name)
		{
			this.itemName = name;
		}

		// Token: 0x04003389 RID: 13193
		public int goodsId;

		// Token: 0x0400338A RID: 13194
		public int itemId;

		// Token: 0x0400338B RID: 13195
		public int charaStatusId;

		// Token: 0x0400338C RID: 13196
		public int itemNum;

		// Token: 0x0400338D RID: 13197
		public int priceItemId;

		// Token: 0x0400338E RID: 13198
		public int priceItemNum;

		// Token: 0x0400338F RID: 13199
		public int maxChangeNum;

		// Token: 0x04003390 RID: 13200
		public int nowChangeNum;

		// Token: 0x04003391 RID: 13201
		public ShopData.ItemOne.DispType dispType;

		// Token: 0x04003392 RID: 13202
		public int disptypeTargetitemId;

		// Token: 0x04003393 RID: 13203
		public int disptypeTargetitemId02;

		// Token: 0x04003394 RID: 13204
		public bool infoWindow;

		// Token: 0x04003395 RID: 13205
		public DateTime startTime;

		// Token: 0x04003396 RID: 13206
		public DateTime endTime;

		// Token: 0x04003397 RID: 13207
		public DateTime? resetTime;

		// Token: 0x04003398 RID: 13208
		public string resetInfoPrefix = "";

		// Token: 0x0400339A RID: 13210
		public int priority;

		// Token: 0x0400339B RID: 13211
		public List<ShopData.ItemOne.SetItem> setItems;

		// Token: 0x0400339C RID: 13212
		public int openQuestOneId;

		// Token: 0x0400339D RID: 13213
		public string openQuestOneIdText;

		// Token: 0x0400339E RID: 13214
		public int openMissionId;

		// Token: 0x0400339F RID: 13215
		public bool notOpenDispFlag;

		// Token: 0x040033A0 RID: 13216
		public bool isLockByQuest;

		// Token: 0x040033A1 RID: 13217
		public bool isLockByMission;

		// Token: 0x040033A2 RID: 13218
		public bool isLockByTime;

		// Token: 0x040033A3 RID: 13219
		public string itemName;

		// Token: 0x040033A4 RID: 13220
		private MstShopItemData mstData;

		// Token: 0x040033A5 RID: 13221
		private ShopItemInfo serverData;

		// Token: 0x02001146 RID: 4422
		public enum DispType
		{
			// Token: 0x04005EF0 RID: 24304
			Always,
			// Token: 0x04005EF1 RID: 24305
			Show,
			// Token: 0x04005EF2 RID: 24306
			Hide,
			// Token: 0x04005EF3 RID: 24307
			SpecialOR,
			// Token: 0x04005EF4 RID: 24308
			SpecialAND
		}

		// Token: 0x02001147 RID: 4423
		public class SetItem
		{
			// Token: 0x0600558D RID: 21901 RVA: 0x0024F2D9 File Offset: 0x0024D4D9
			public SetItem(int id, int num)
			{
				this.itemId = id;
				this.itemNum = num;
			}

			// Token: 0x04005EF5 RID: 24309
			public int itemId;

			// Token: 0x04005EF6 RID: 24310
			public int itemNum;
		}

		// Token: 0x02001148 RID: 4424
		public class CharaStatusData
		{
			// Token: 0x0600558E RID: 21902 RVA: 0x0024F2F0 File Offset: 0x0024D4F0
			public CharaStatusData(MstShopCharaStatusData mst)
			{
				this.level = mst.level;
				this.rank = mst.rank;
				this.kizunaLevel = mst.kizunaLevel;
				this.promoteNum = mst.promoteNum;
				this.artsLevel = mst.artsLevel;
				this.photoFrameLevel = mst.photoFrameLevel;
			}

			// Token: 0x04005EF7 RID: 24311
			public int level;

			// Token: 0x04005EF8 RID: 24312
			public int rank;

			// Token: 0x04005EF9 RID: 24313
			public int kizunaLevel;

			// Token: 0x04005EFA RID: 24314
			public int promoteNum;

			// Token: 0x04005EFB RID: 24315
			public int artsLevel;

			// Token: 0x04005EFC RID: 24316
			public int photoFrameLevel;
		}
	}
}
