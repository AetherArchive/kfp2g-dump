using System;
using SGNFW.HttpRequest.Protocol;

public class PurchaseProductOne
{
	public int unionNum
	{
		get
		{
			return this.chargeNum + this.freeNum;
		}
	}

	public int MonthlyPackId { get; private set; }

	public string InfoPicturePath { get; private set; }

	public ItemData MainItem { get; private set; }

	public string MainItemIconOptionPath { get; private set; }

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

	public int productId;

	public string productIdString;

	public int chargeNum;

	public int freeNum;

	public int price;

	public string bonusItemTitle;

	public ItemData bonusItem;

	public DateTime? sellStartTime;

	public DateTime? sellEndTime;

	public int purchasePossibleNum;

	public bool isSoldOut;

	public bool isQuestClearLimited;

	public bool isDispInfo;

	public string iconPath;

	public string iconOptionPath;

	public string resultIconPath;

	public string limitText = "";

	public string infoText = "";

	public bool isResetItem;

	public int releaseGroupId;

	public int releaseMonthlyPackId;

	public int releaseCharaId;

	public int groupId;

	public PurchaseProductOne.TabType tabType;

	public PurchaseProductOne.BGType bgType;

	public PurchaseProductOne.CommentType commentType;

	public PurchaseProductOne.InfoType infoType;

	public enum TabType
	{
		Invalid,
		Limited,
		MonthlyPack,
		Kirakira,
		Pack,
		MonthlyPassport = 99
	}

	public enum BGType
	{
		Invalid,
		Rainbow,
		Orange,
		Red,
		Yellow,
		Pink,
		LightPink,
		Green,
		Blue,
		Normal
	}

	public enum CommentType
	{
		Invalid,
		GoodValue,
		Term,
		Growth,
		Limited,
		New
	}

	public enum InfoType
	{
		None,
		QuestClear,
		OnceForThePeriod,
		OnceADayForThePeriod
	}
}
