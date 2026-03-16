using System;
using SGNFW.Mst;

public class PurchaseProductStatic : MstPurchaseProduct
{
	public bool IsFreeMonthlyPack
	{
		get
		{
			return this.monthlyPackId > 0 && this.priceInTax <= 0;
		}
	}

	public PurchaseProductStatic(MstPurchaseProduct mst)
	{
		this.productIdCommon = mst.productIdCommon;
		this.storeType = mst.storeType;
		this.tab = mst.tab;
		this.productName = mst.productName;
		this.productId = mst.productId;
		this.currencySymbol = mst.currencySymbol;
		this.purchaseLimitNum = mst.purchaseLimitNum;
		this.purchaseLimitType = mst.purchaseLimitType;
		this.purchaseLimitResetDate = mst.purchaseLimitResetDate;
		this.backCampProductIdCommon = mst.backCampProductIdCommon;
		this.addStoneCharge = mst.addStoneCharge;
		this.addStoneFree = mst.addStoneFree;
		this.bonusItemId = mst.bonusItemId;
		this.bonusNum = mst.bonusNum;
		this.priceInTax = mst.priceInTax;
		this.infoIcon = mst.infoIcon;
		this.isDiscount = mst.isDiscount;
		this.sortIndex = mst.sortIndex;
		this.startTime = mst.startTime;
		this.endTime = mst.endTime;
		this.monthlyPackId = mst.monthlyPackId;
		this.infoType = mst.infoType;
		this.infoText = mst.infoText;
		this.infoPicturePath = mst.infoPicturePath;
		this.releaseQuestId = mst.releaseQuestId;
		this.releaseGroupId = mst.releaseGroupId;
		this.releaseMonthlyPackId = mst.releaseMonthlyPackId;
		this.releaseCharaId = mst.releaseCharaId;
		this.releaseTime = mst.releaseTime;
		this.groupId = mst.groupId;
		this.optionIconPath = mst.optionIconPath;
		this.panelBgType = mst.panelBgType;
		this.commentType = mst.commentType;
		this.mainItemId = mst.mainItemId;
		this.mainItemNum = mst.mainItemNum;
		this.mainItemIconpath = mst.mainItemIconpath;
		this.comebackDay = mst.comebackDay;
	}
}
