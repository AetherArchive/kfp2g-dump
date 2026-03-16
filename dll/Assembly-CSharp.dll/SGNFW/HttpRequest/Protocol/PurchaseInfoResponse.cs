using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PurchaseInfoResponse : Response
	{
		public Assets assets;

		public List<PurchaseInfo> purchaseInfoList;

		public List<int> soldOutIdList;

		public int residuePurchaseNum;

		public bool isPendingMonthlyPack;

		public List<int> pendingIdList;
	}
}
