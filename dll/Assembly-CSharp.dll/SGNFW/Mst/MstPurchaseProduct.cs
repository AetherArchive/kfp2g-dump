using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstPurchaseProduct
	{
		public int productIdCommon;

		public int storeType;

		public int tab;

		public string productName;

		public string productId;

		public string currencySymbol;

		public int purchaseLimitNum;

		public int purchaseLimitType;

		public long purchaseLimitResetDate;

		public int backCampProductIdCommon;

		public int addStoneCharge;

		public int addStoneFree;

		public int mainItemId;

		public int mainItemNum;

		public string mainItemIconpath;

		public int bonusItemId;

		public int bonusNum;

		public int priceInTax;

		public int infoIcon;

		public int isDiscount;

		public int sortIndex;

		public long startTime;

		public long endTime;

		public int monthlyPackId;

		public int infoType;

		public string infoPicturePath;

		public string infoText;

		public int releaseTime;

		public int releaseQuestId;

		public int releaseGroupId;

		public int releaseMonthlyPackId;

		public int releaseCharaId;

		public int groupId;

		public string optionIconPath;

		public int panelBgType;

		public int commentType;

		public int purchaseItemType;

		public int comebackDay;
	}
}
