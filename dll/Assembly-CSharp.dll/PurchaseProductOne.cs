using System;
using SGNFW.HttpRequest.Protocol;

// Token: 0x020000FD RID: 253
public class PurchaseProductOne
{
	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06000C23 RID: 3107 RVA: 0x00048305 File Offset: 0x00046505
	public int unionNum
	{
		get
		{
			return this.chargeNum + this.freeNum;
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x06000C24 RID: 3108 RVA: 0x00048314 File Offset: 0x00046514
	// (set) Token: 0x06000C25 RID: 3109 RVA: 0x0004831C File Offset: 0x0004651C
	public int MonthlyPackId { get; private set; }

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x06000C26 RID: 3110 RVA: 0x00048325 File Offset: 0x00046525
	// (set) Token: 0x06000C27 RID: 3111 RVA: 0x0004832D File Offset: 0x0004652D
	public string InfoPicturePath { get; private set; }

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06000C28 RID: 3112 RVA: 0x00048336 File Offset: 0x00046536
	// (set) Token: 0x06000C29 RID: 3113 RVA: 0x0004833E File Offset: 0x0004653E
	public ItemData MainItem { get; private set; }

	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06000C2A RID: 3114 RVA: 0x00048347 File Offset: 0x00046547
	// (set) Token: 0x06000C2B RID: 3115 RVA: 0x0004834F File Offset: 0x0004654F
	public string MainItemIconOptionPath { get; private set; }

	// Token: 0x06000C2C RID: 3116 RVA: 0x00048358 File Offset: 0x00046558
	public PurchaseProductOne(PurchaseProductStatic staticData, PurchaseInfo purchaseInfo)
	{
		this.productId = staticData.productIdCommon;
		this.productIdString = staticData.productId;
		this.chargeNum = staticData.addStoneCharge;
		this.freeNum = staticData.addStoneFree;
		this.price = staticData.priceInTax;
		this.tabType = (PurchaseProductOne.TabType)staticData.tab;
		this.bgType = (PurchaseProductOne.BGType)staticData.panelBgType;
		this.commentType = (PurchaseProductOne.CommentType)staticData.commentType;
		this.isDispInfo = false;
		this.bonusItemTitle = staticData.productName;
		if (staticData.bonusItemId != 0)
		{
			this.bonusItem = new ItemData(staticData.bonusItemId, staticData.bonusNum);
			this.isDispInfo = ItemDef.Id2Kind(staticData.bonusItemId) > ItemDef.Kind.INVALID;
		}
		if (purchaseInfo != null)
		{
			this.purchasePossibleNum = ((staticData.purchaseLimitNum == 0) ? (-1) : (staticData.purchaseLimitNum - purchaseInfo.purchaseCount));
			this.isSoldOut = staticData.purchaseLimitNum != 0 && staticData.purchaseLimitNum <= purchaseInfo.purchaseCount;
		}
		else
		{
			this.isSoldOut = true;
		}
		this.iconPath = "Texture2D/Shop_BuyImg/shop_buyimg0" + staticData.infoIcon.ToString();
		this.iconOptionPath = "Texture2D/Shop_BuyImg_Pack/" + staticData.optionIconPath;
		this.resultIconPath = "SceneShop/GUI/Prefab/ShopImgAE/ShopImg" + staticData.infoIcon.ToString("00");
		this.MonthlyPackId = staticData.monthlyPackId;
		this.infoType = (PurchaseProductOne.InfoType)staticData.infoType;
		this.InfoPicturePath = staticData.infoPicturePath;
		this.infoText = staticData.infoText;
		this.releaseGroupId = staticData.releaseGroupId;
		this.releaseMonthlyPackId = staticData.releaseMonthlyPackId;
		this.releaseCharaId = staticData.releaseCharaId;
		this.groupId = staticData.groupId;
		this.sellStartTime = new DateTime?(new DateTime(PrjUtil.ConvertTimeToTicks(staticData.startTime)));
		this.sellEndTime = new DateTime?(new DateTime(PrjUtil.ConvertTimeToTicks(staticData.endTime)));
		if (staticData.mainItemId != 0)
		{
			this.MainItem = new ItemData(staticData.mainItemId, staticData.mainItemNum);
			this.MainItemIconOptionPath = "Texture2D/Shop_BuyImg_Pack/" + staticData.mainItemIconpath;
		}
		if ((TimeManager.Now - this.sellStartTime.Value).Days >= DataManagerPurchase.LimitItemJudgeDays)
		{
			this.sellStartTime = null;
		}
		if ((this.sellEndTime.Value - TimeManager.Now).Days >= DataManagerPurchase.LimitItemJudgeDays)
		{
			this.sellEndTime = null;
		}
		this.isQuestClearLimited = false;
		QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap.TryGetValueEx(staticData.releaseQuestId, null);
		if (questDynamicQuestOne != null && questDynamicQuestOne.firstClearTime != null)
		{
			this.isQuestClearLimited = true;
			this.sellEndTime = new DateTime?(questDynamicQuestOne.firstClearTime.Value.AddHours((double)staticData.releaseTime));
		}
		if (staticData.comebackDay > 0)
		{
			DateTime dateTime = new DateTime(PrjUtil.ConvertTimeToTicks(purchaseInfo.comebackstarttime));
			this.sellEndTime = new DateTime?(dateTime.AddHours((double)staticData.releaseTime));
		}
		if (this.purchasePossibleNum >= 0)
		{
			this.limitText = PrjUtil.MakeMessage("あと") + this.purchasePossibleNum.ToString() + PrjUtil.MakeMessage("個");
		}
		DateTime now = TimeManager.Now;
		DateTime? dateTime2 = null;
		if (staticData.purchaseLimitType == 1)
		{
			this.isResetItem = true;
			dateTime2 = new DateTime?(TimeManager.GetTerminalTimeByDay(now));
		}
		else if (staticData.purchaseLimitType == 2)
		{
			this.isResetItem = true;
			dateTime2 = new DateTime?(TimeManager.GetTerminalTimeByWeek(now));
		}
		else if (staticData.purchaseLimitType == 3)
		{
			this.isResetItem = true;
			dateTime2 = new DateTime?(TimeManager.GetTerminalTimeByMonth(now));
		}
		if (dateTime2 != null && this.sellEndTime != null)
		{
			DateTime dateTime3 = ((dateTime2.Value.Ticks > this.sellEndTime.Value.Ticks) ? this.sellEndTime.Value : dateTime2.Value);
			if (this.limitText != string.Empty)
			{
				this.limitText += "\u3000";
			}
			this.limitText += TimeManager.MakeTimeResidueText(TimeManager.Now, dateTime3, false, true);
			return;
		}
		if (dateTime2 != null)
		{
			if (this.limitText != string.Empty)
			{
				this.limitText += "\u3000";
			}
			this.limitText += TimeManager.MakeTimeResidueText(TimeManager.Now, dateTime2.Value, false, true);
			return;
		}
		if (this.sellEndTime != null)
		{
			if (this.limitText != string.Empty)
			{
				this.limitText += "\u3000";
			}
			this.limitText += TimeManager.MakeTimeResidueText(TimeManager.Now, this.sellEndTime.Value, false, true);
		}
	}

	// Token: 0x04000952 RID: 2386
	public int productId;

	// Token: 0x04000953 RID: 2387
	public string productIdString;

	// Token: 0x04000954 RID: 2388
	public int chargeNum;

	// Token: 0x04000955 RID: 2389
	public int freeNum;

	// Token: 0x04000956 RID: 2390
	public int price;

	// Token: 0x04000957 RID: 2391
	public string bonusItemTitle;

	// Token: 0x04000958 RID: 2392
	public ItemData bonusItem;

	// Token: 0x04000959 RID: 2393
	public DateTime? sellStartTime;

	// Token: 0x0400095A RID: 2394
	public DateTime? sellEndTime;

	// Token: 0x0400095B RID: 2395
	public int purchasePossibleNum;

	// Token: 0x0400095C RID: 2396
	public bool isSoldOut;

	// Token: 0x0400095D RID: 2397
	public bool isQuestClearLimited;

	// Token: 0x0400095E RID: 2398
	public bool isDispInfo;

	// Token: 0x0400095F RID: 2399
	public string iconPath;

	// Token: 0x04000960 RID: 2400
	public string iconOptionPath;

	// Token: 0x04000961 RID: 2401
	public string resultIconPath;

	// Token: 0x04000962 RID: 2402
	public string limitText = "";

	// Token: 0x04000963 RID: 2403
	public string infoText = "";

	// Token: 0x04000965 RID: 2405
	public bool isResetItem;

	// Token: 0x04000966 RID: 2406
	public int releaseGroupId;

	// Token: 0x04000967 RID: 2407
	public int releaseMonthlyPackId;

	// Token: 0x04000968 RID: 2408
	public int releaseCharaId;

	// Token: 0x04000969 RID: 2409
	public int groupId;

	// Token: 0x0400096A RID: 2410
	public PurchaseProductOne.TabType tabType;

	// Token: 0x0400096B RID: 2411
	public PurchaseProductOne.BGType bgType;

	// Token: 0x0400096C RID: 2412
	public PurchaseProductOne.CommentType commentType;

	// Token: 0x0400096D RID: 2413
	public PurchaseProductOne.InfoType infoType;

	// Token: 0x0200081B RID: 2075
	public enum TabType
	{
		// Token: 0x0400364F RID: 13903
		Invalid,
		// Token: 0x04003650 RID: 13904
		Limited,
		// Token: 0x04003651 RID: 13905
		MonthlyPack,
		// Token: 0x04003652 RID: 13906
		Kirakira,
		// Token: 0x04003653 RID: 13907
		Pack,
		// Token: 0x04003654 RID: 13908
		MonthlyPassport = 99
	}

	// Token: 0x0200081C RID: 2076
	public enum BGType
	{
		// Token: 0x04003656 RID: 13910
		Invalid,
		// Token: 0x04003657 RID: 13911
		Rainbow,
		// Token: 0x04003658 RID: 13912
		Orange,
		// Token: 0x04003659 RID: 13913
		Red,
		// Token: 0x0400365A RID: 13914
		Yellow,
		// Token: 0x0400365B RID: 13915
		Pink,
		// Token: 0x0400365C RID: 13916
		LightPink,
		// Token: 0x0400365D RID: 13917
		Green,
		// Token: 0x0400365E RID: 13918
		Blue,
		// Token: 0x0400365F RID: 13919
		Normal
	}

	// Token: 0x0200081D RID: 2077
	public enum CommentType
	{
		// Token: 0x04003661 RID: 13921
		Invalid,
		// Token: 0x04003662 RID: 13922
		GoodValue,
		// Token: 0x04003663 RID: 13923
		Term,
		// Token: 0x04003664 RID: 13924
		Growth,
		// Token: 0x04003665 RID: 13925
		Limited,
		// Token: 0x04003666 RID: 13926
		New
	}

	// Token: 0x0200081E RID: 2078
	public enum InfoType
	{
		// Token: 0x04003668 RID: 13928
		None,
		// Token: 0x04003669 RID: 13929
		QuestClear,
		// Token: 0x0400366A RID: 13930
		OnceForThePeriod,
		// Token: 0x0400366B RID: 13931
		OnceADayForThePeriod
	}
}
