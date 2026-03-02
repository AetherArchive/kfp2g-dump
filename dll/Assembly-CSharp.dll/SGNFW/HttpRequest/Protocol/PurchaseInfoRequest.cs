using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004C7 RID: 1223
	public class PurchaseInfoRequest : Request
	{
		// Token: 0x040026F7 RID: 9975
		public List<string> notFinishTransactionList;

		// Token: 0x040026F8 RID: 9976
		public bool isStartGetProduct;
	}
}
