using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class PurchaseInfo
	{
		public int productIdCommon;

		public int purchaseCount;

		public int sortIndex;

		public long comebackstarttime;
	}
}
