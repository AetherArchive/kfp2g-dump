using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstPurchaseMonthlypackData
	{
		public int monthlyPackId;

		public string packName;

		public int prevBuyGorgeousAddItemId;

		public int prevBuyGorgeousAddItemNum;

		public int prevBuyGreatAddItemId;

		public int prevBuyGreatAddItemNum;

		public int prevBuyAlittleAddItemId;

		public int prevBuyAlittleAddItemNum;

		public int prevBuyFirstAddItemId;

		public int prevBuyFirstAddItemNum;

		public int picnicBuffFramecnt;

		public int picnicBuffAddratio;

		public string webviewLink;

		public int bonusMissionId;

		public string buffText;

		public int masterRoomAddMysetNum;

		public int skippableFlag;

		public int battleRetryFlag;

		public int practiceFlag;

		public int packType;
	}
}
