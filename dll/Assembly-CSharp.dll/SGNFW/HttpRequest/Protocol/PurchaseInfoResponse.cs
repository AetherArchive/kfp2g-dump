using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004C8 RID: 1224
	public class PurchaseInfoResponse : Response
	{
		// Token: 0x040026F9 RID: 9977
		public Assets assets;

		// Token: 0x040026FA RID: 9978
		public List<PurchaseInfo> purchaseInfoList;

		// Token: 0x040026FB RID: 9979
		public List<int> soldOutIdList;

		// Token: 0x040026FC RID: 9980
		public int residuePurchaseNum;

		// Token: 0x040026FD RID: 9981
		public bool isPendingMonthlyPack;

		// Token: 0x040026FE RID: 9982
		public List<int> pendingIdList;
	}
}
