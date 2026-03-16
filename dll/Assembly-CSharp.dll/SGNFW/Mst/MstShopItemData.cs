using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstShopItemData
	{
		public int goodsId;

		public int shopId;

		public int itemId;

		public int itemNum;

		public int charaStatusId;

		public int priceItemId;

		public int priceItemNum;

		public int resetType;

		public int maxChangeNum;

		public int priority;

		public long startTime;

		public long endTime;

		public int notOpenDispFlg;

		public int dispType;

		public int disptypeTargetitemId;

		public int disptypeTargetitemId2;

		public int openQuestOneId;

		public string openQuestOneText;

		public int openMissionId;
	}
}
